using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


public partial class EMR_Dashboard_IPPatientDashboardForDoctor : System.Web.UI.Page
{

    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //clsExceptionLog objException = new clsExceptionLog();
    private enum enumProblem : byte
    {
        Problem = 0,
        IsHPI = 1,
        Duration = 2,
        Date = 3,
        EnteredBy = 4,
        Edit = 5,
        Delete = 6
    }
    private enum enumHistory : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumPrevTreatment : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumExamination : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumNutritional : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumPlanOfCare : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumCostAnalysis : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumProvisionalDiagnosis : byte
    {
        ProvisionalDiagnosis = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3,
        Delete = 4
    }
    private enum enumNonDrugOrder : byte
    {
        NonDrugOrders = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    private enum enumPHistory : byte
    {
        FieldName = 0,
        Date = 1,
        EnteredBy = 2,
        Edit = 3
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //Application.Lock();
                //Application["LoginID"] = "annathurai";
                //Application.UnLock();
                hdnIsTransitDataEntered.Value = "0";
                hdnCurrentControlFocused.Value = null;
                Session["PreviousRowIndex"] = null;
                lbtnExpand.Text = "Collapse All";
                ViewState["ExpandAllStatus"] = true;
                if (common.myInt(Session["HospitalLocationId"]).Equals(0) || common.myInt(Session["FacilityId"]).Equals(0)
                    || common.myInt(Session["EncounterId"]).Equals(0))
                {
                    Response.Redirect("/Default.aspx", false);
                    return;
                }
                DataSet ds = new DataSet();
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/Common/GfsQueryCountManagement";
                APIRootClass.GfsCountManagement objRoot = new global::APIRootClass.GfsCountManagement();
                objRoot.Employeeid = common.myInt(Session["EmployeeId"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    Session["CountQueryData"] = ds.Tables[0].Rows.Count;
                    // Session["QueryData"] = ds;
                    //LinkButton lbl = Master.FindControl("lblQty") as LinkButton;
                    //if (lbl != null)
                    //{
                    //    lbl.Text = sValue;
                    //    lbl.ToolTip = "You have " + lbl.Text + " Unrespond Queries from Insurance.";
                    //}
                }
                /*
                 *  DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                 * DataSet ds = dl.FillDataSet(CommandType.Text, "exec UspGetGfsQueryDoc @empID=" + common.myInt(Session["EmployeeId"]));  // 1 kk
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Session["QueryData"] = ds;
                    //RadWindowForNew.NavigateUrl = "../../Approval/QueryResponse.aspx";
                    //RadWindowForNew.Height = 600;
                    //RadWindowForNew.Width = 750;
                    //RadWindowForNew.Top = 10;
                    //RadWindowForNew.Left = 10;
                    //RadWindowForNew.VisibleOnPageLoad = true;
                    //RadWindowForNew.Modal = true;
                    //RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                    //RadWindowForNew.VisibleStatusbar = false;
                    LinkButton lbl = Master.FindControl("lblQty") as LinkButton;
                    if (lbl != null)
                    {
                        lbl.Text = ds.Tables[0].Rows.Count.ToString();
                        lbl.ToolTip = "You have " + lbl.Text + " Unrespond Queries from Insurance.";
                    }
                }
                */


                //dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                //dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                //dtpFromDate.SelectedDate = common.myDate(Session["EncounterDate"]);
                //dtpToDate.SelectedDate = common.myDate(DateTime.Now);
                //bindPatientDetails();
                editorChiefComplaints.Text = string.Empty;
                //ClearEditorControls();
                ClearMessageControl();
                BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
                BindDiagnosisSearchCode();
                ViewState["_ID"] = 0;
                dvConfirmCancelOptionsProvisionalDiag.Visible = false;
                BindDoctor();
                ShowHidePanel(false);
                dvConfirmCancelOptions.Visible = false;
                SetPermission();
                //padam
                //lnkIPExtension
                lnktriageform.Visible = false;
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    lnkIPExtension.Visible = true;
                    lnkpreauth.Visible = false;
                }
                else
                {
                    lnkpreauth.Visible = true;
                    lnkIPExtension.Visible = false;
                }
                if (common.myStr(Session["OPIP"]).Equals("E") || common.myStr(Session["OPIP"]).Equals("O"))
                {


                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    ServiceURL = WebAPIAddress.ToString() + "api/Common/GetERtoken";
                    APIRootClass.ERtoken objtoken = new global::APIRootClass.ERtoken();
                    objtoken.ErEncounterID = common.myStr(Session["EncounterID"]);
                    string strtriageID = string.Empty;
                    inputJson = (new JavaScriptSerializer()).Serialize(objtoken);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    strtriageID = JsonConvert.DeserializeObject<string>(sValue);



                    // DAL.DAL dlx = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); // 2 kk
                    // string strtriageID = common.myStr(dlx.ExecuteScalar(CommandType.Text, "select ID from ERtoken with(nolock) where ErEncounterID=" + Session["EncounterID"])).ToString();
                    if (common.myInt(strtriageID) != 0)
                    {
                        lnktriageform.CommandName = common.myInt(strtriageID).ToString();
                        lnktriageform.Visible = true;
                    }
                    //lnktriageform.co
                    //if (common.myInt(Session["EmployeeId"]) != common.myInt(Session["DoctorID"]))
                    //{

                    //    btnAssigntoMe.Visible = true;
                    //}
                    //else
                    //{
                    //    btnAssigntoMe.Visible = false;
                    //}

                }
                else
                {
                    btnAssigntoMe.Visible = false;
                }

                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    btnSaveAsSigned.Visible = true;
                    btnSaveSign.Visible = true;
                }
                else
                {
                    btnSaveDashboard.Text = "Save (F3)";
                    btnSave.Text = "Save (F3)";
                }
                if (common.myStr(Session["OPIP"]).Equals("O")
                    && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSaveSign.Visible = true;
                    btnSaveAsSigned.Visible = true;
                    btnSaveDashboard.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    btnSaveSign.Visible = false;
                    btnSaveAsSigned.Visible = false;
                    btnSave.Visible = false;
                    btnSaveDashboard.Visible = false;
                    if (!common.myStr(Session["OPIP"]).Equals("O") && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                    {
                        btnSave.Visible = true;
                        btnSaveDashboard.Visible = true;
                    }
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    ClearButtonControl();
                    if (common.myBool(Session["isEMRSuperUser"]))
                    {
                        if (common.myStr(Session["OPIP"]).Equals("O"))
                        {
                            btnDefinalise.Visible = true;
                        }
                        OpenButtonControls();
                    }
                    if (common.myBool(ViewState["SaveEnable"]))
                    {
                        if (common.myStr(Session["OPIP"]).Equals("O"))
                        {
                            btnDefinalise.Visible = true;
                        }
                    }
                }
                #region Interface
                ViewState["IsCIMSInterfaceActive"] = common.myBool(Session["IsCIMSInterfaceActive"]);
                ViewState["IsVIDALInterfaceActive"] = common.myBool(Session["IsVIDALInterfaceActive"]);
                #endregion
                txtHeight.Attributes.Add("onchange", "javascript:CalculateBMI('" + txtHeight.ClientID + "');");
                TxtWeight.Attributes.Add("onchange", "javascript:CalculateBMI('" + TxtWeight.ClientID + "');");
                if (common.myBool(Session["isEMRSuperUser"]) && common.myStr(Session["OPIP"]).Equals("I") && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSaveDashboard.Visible = true;
                    btnSave.Visible = true;
                    btnSaveDashboard.Text = "Save (F3)";
                    btnSave.Text = "Save (F3)";
                }
                ImageButton7.ImageUrl = "~/Images/Collapse.jpg";
                imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
                ImageButton5.ImageUrl = "~/Images/Collapse.jpg";
                PanelVisibility(ImageButton7.ToolTip.Trim());
                PanelVisibility(imgbtnProvisionalDiagnosies.ToolTip.Trim());
                PanelVisibility(ImageButton5.ToolTip.Trim());
                bindDataInTransit();

                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); // 3 kk 
                //ViewState["CheifComplaintFound"] = common.myInt(dl.ExecuteScalar(CommandType.Text, "uspCheckPatientProblem @encounterID=" + common.myInt(Session["EncounterId"])));

                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                ServiceURL = WebAPIAddress.ToString() + "api/Common/CheckPatientProblem";
                APIRootClass.PatientProblem objPatientProblem = new global::APIRootClass.PatientProblem();
                objPatientProblem.EncounterID = common.myInt(Session["EncounterID"]);
                //string strtriageID = string.Empty;
                inputJson = (new JavaScriptSerializer()).Serialize(objPatientProblem);
                sValue = client.UploadString(ServiceURL, inputJson);
                ViewState["CheifComplaintFound"] = JsonConvert.DeserializeObject<string>(sValue);


                if (common.myInt(ViewState["CheifComplaintFound"]).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Chief Complaints not Saved for the patient";
                }

                setTabVisibility();
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
        reSetTimer();
        setFocus(false);
        if (common.myLen(txtHeight.Text) > 0 && common.myLen(TxtWeight.Text) > 0)
        {
            if (common.myLen(txtBMI.Text).Equals(0) && common.myLen(txtBSA.Text).Equals(0))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "CalculateBMI('" + txtHeight.ClientID + "');", true);
            }
        }
        bindTestData();
    }
    [System.Web.Services.WebMethod]
    public static string CalculateBMIAndBSA(string sConString, string txtHeight, string hdnHeight, string TxtWeight, string hdnWeight)
    {
        //BaseC.EMRVitals objVital = new BaseC.EMRVitals(sConString); // 4 kk
        DataSet ds = new DataSet();
        StringBuilder objStr = new StringBuilder();
        ArrayList coll = new ArrayList();
        string bmiandBSA = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(txtHeight))
            {
                coll.Add(common.myInt(hdnHeight));
                coll.Add(common.myInt(5));
                coll.Add(common.myInt(txtHeight));
                coll.Add(common.myInt(0));
                coll.Add(common.myInt(0));
                objStr.Append(common.setXmlTable(ref coll));
            }
            if (!string.IsNullOrEmpty(TxtWeight))
            {
                coll.Add(common.myInt(hdnWeight));
                coll.Add(common.myInt(1));
                coll.Add(common.myInt(TxtWeight));
                coll.Add(common.myInt(0));
                coll.Add(common.myInt(0));
                objStr.Append(common.setXmlTable(ref coll));
            }
            if (!string.IsNullOrEmpty(common.myStr(objStr)))
            {
                // ds = objVital.CalculateVitalsValue(common.myInt(HttpContext.Current.Session["HospitalLocationID"]), objStr.ToString());

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/Common/CalculateVitalsValue";
                APIRootClass.VitalsValue objVitalsValue = new global::APIRootClass.VitalsValue();
                objVitalsValue.HospitalLocationID = common.myInt(HttpContext.Current.Session["HospitalLocationID"]);
                objVitalsValue.xmlstr = objStr.ToString();

                string inputJson = (new JavaScriptSerializer()).Serialize(objVitalsValue);
                string sValue = client.UploadString(ServiceURL, inputJson);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (common.myStr(dr["DisplayName"]).Equals("BMI") && !string.IsNullOrEmpty(common.myStr(dr["Value"])))
                        {
                            bmiandBSA = common.myStr(dr["Value"]);
                        }
                        if (common.myStr(dr["DisplayName"]).Equals("BSA") && !string.IsNullOrEmpty(common.myStr(dr["Value"])))
                        {
                            bmiandBSA = bmiandBSA + "," + common.myStr(dr["Value"]);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            //objVital = null;
            ds.Dispose();
            objStr = null;
            coll = null;
        }
        return bmiandBSA;
    }
    #region Common method
    protected void lbtnPastClinicalNotes_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(Session["RegistrationID"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }
            if (common.myInt(Session["encounterid"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }
            autoSaveDataInTransit(true);
            Session["SelectedCaseSheet"] = "PN";
            RadWindowForNew.NavigateUrl = "/WardManagement/VisitHistory.aspx?RNo=" + common.myStr(Session["RegistrationNo"])
                                + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
                                + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"])
                                + "&FromWard=Y&OP_IP=I&Category=PopUp";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = string.Empty;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void lbtnExpand_Click(object sender, EventArgs e)
    {
        ClearMessageControl();
        if (common.myBool(ViewState["ExpandAllStatus"]))
        {
            ViewState["ExpandAllStatus"] = false;
            lbtnExpand.Text = "Expand All";
            ShowHidePanel(true);
        }
        else
        {
            ViewState["ExpandAllStatus"] = true;
            lbtnExpand.Text = "Collapse All";
            ShowHidePanel(false);
        }
    }
    private void ShowHidePanel(Boolean flag)
    {
        ClearMessageControl();
        if (!flag)
        {
            pnlChiefComplaints.Visible = true;
            Panel2.Visible = true;
            // Panel4.Visible = true;
            //Panel8.Visible = true;
            Panel10.Visible = true;
            // Panel11.Visible = true;
            // Panel12.Visible = true;
            // Panel5.Visible = true;
            //  Panel7.Visible = true;
            Panel3.Visible = true;
            Panel6.Visible = true;
            Panel13.Visible = true;
            Panel14.Visible = true;
            Panel20.Visible = true;
            // Panel21.Visible = true;
            // Panel22.Visible = true;
            Panel23.Visible = true;

            pnlChiefComplaints.Visible = true;
            // pnlAllergies.Visible = true;
            pnlVitals.Visible = true;
            pnlHistory.Visible = true;
            //   pnlExamination.Visible = true;
            //  pnlPlanOfCare.Visible = true;
            pnlOtherNotes.Visible = true;
            pnlProvisionalDiagnosis.Visible = true;
            pnlOrderProcedures.Visible = true;
            pnlPrescription.Visible = true;
            pnlDiagnosis.Visible = true;



            imgbtnChiefComplaints.ImageUrl = "~/Images/Expand.jpg";
            ImageButton3.ImageUrl = "~/Images/Expand.jpg";
            imgVbtnVital.ImageUrl = "~/Images/Expand.jpg";
            imbtnHistory.ImageUrl = "~/Images/Expand.jpg";
            imgbtnTemplate.ImageUrl = "~/Images/Expand.jpg";
            imgbtntherNotes.ImageUrl = "~/Images/Expand.jpg";
            imgbtnOrdersAndProcedures.ImageUrl = "~/Images/Expand.jpg";
            imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Expand.jpg";
            imgbtnPrescription.ImageUrl = "~/Images/Expand.jpg";
            ImageButton2.ImageUrl = "~/Images/Expand.jpg";
            ibtNutritionalStatus.ImageUrl = "~/Images/Expand.jpg";
            ImageButton4.ImageUrl = "~/Images/Expand.jpg";
            ImageButton7.ImageUrl = "~/Images/Expand.jpg";
            ImageButton5.ImageUrl = "~/Images/Expand.jpg";
            ImageButton9.ImageUrl = "~/Images/Expand.jpg";
            ImageButton10.ImageUrl = "~/Images/Expand.jpg";
            ImageButton11.ImageUrl = "~/Images/Expand.jpg";
            ImageButton12.ImageUrl = "~/Images/Expand.jpg";
            ImageButton8.ImageUrl = "~/Images/Expand.jpg";
            ImageButton13.ImageUrl = "~/Images/Expand.jpg";
            ImageButton14.ImageUrl = "~/Images/Expand.jpg";
        }
        else
        {
            pnlChiefComplaints.Visible = false;
            Panel2.Visible = false;
            //Panel4.Visible = false;
            //  Panel8.Visible = false;
            Panel10.Visible = false;
            // Panel11.Visible = false;
            // Panel12.Visible = false;
            // Panel5.Visible = false;
            //Panel7.Visible = false;
            Panel3.Visible = false;
            Panel6.Visible = false;
            Panel13.Visible = false;
            Panel14.Visible = false;
            Panel20.Visible = false;
            // Panel21.Visible = false;
            // Panel22.Visible = false;
            Panel23.Visible = false;

            pnlChiefComplaints.Visible = false;
            // pnlAllergies.Visible = false;
            pnlVitals.Visible = false;
            pnlHistory.Visible = false;
            //  pnlExamination.Visible = false;
            // pnlPlanOfCare.Visible = false;
            pnlOtherNotes.Visible = false;
            pnlProvisionalDiagnosis.Visible = false;
            pnlOrderProcedures.Visible = false;
            pnlPrescription.Visible = false;
            pnlDiagnosis.Visible = false;

            imgbtnChiefComplaints.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton3.ImageUrl = "~/Images/Collapse.jpg";
            imgVbtnVital.ImageUrl = "~/Images/Collapse.jpg";
            imbtnHistory.ImageUrl = "~/Images/Collapse.jpg";
            imgbtnTemplate.ImageUrl = "~/Images/Collapse.jpg";
            imgbtntherNotes.ImageUrl = "~/Images/Collapse.jpg";
            imgbtnOrdersAndProcedures.ImageUrl = "~/Images/Collapse.jpg";
            imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
            imgbtnPrescription.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton2.ImageUrl = "~/Images/Collapse.jpg";
            ibtNutritionalStatus.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton4.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton7.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton5.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton8.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton9.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton10.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton11.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton12.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton13.ImageUrl = "~/Images/Collapse.jpg";
            ImageButton14.ImageUrl = "~/Images/Collapse.jpg";
        }
    }
    private void ClearMessageControl()
    {
        lblCostAnalysisMessage.Text = string.Empty;
        lblPlanOfCareMessage.Text = string.Empty;
        lblNutritionalStatusMessage.Text = string.Empty;
        lblPrevTreatmentMessage.Text = string.Empty;
        lblExamMessage.Text = string.Empty;
        lblHistoryMessage.Text = string.Empty;
        lblPHistoryMessage.Text = string.Empty;
        lblVitalMessage.Text = string.Empty;
        lblChiefMessage.Text = string.Empty;
        lblAllergyMessage.Text = string.Empty;
        lblVitalMessage.Text = string.Empty;
        lblProvDiag.Text = string.Empty;
    }
    private void ClearButtonControl()
    {
        imgBtnAddChiefComplaints.Visible = false;
        imgBtnAddAllergies.Visible = false;
        lnkImmunisationHistory.Visible = false;
        imgVbtnVital.Visible = false;
        imgBtnAddVitals.Visible = false;
        ImageButton1.Visible = false;
        imgBtnTemplates.Visible = false;
        ImageButton6.Visible = false;
        lnkAddTemplates_All.Visible = false;
        imgBtnProvisionalDiagnosis.Visible = false;
        imgbtnAddOrdersAndProcedures.Visible = false;
        imgBtnAddPrescriptions.Visible = false;
        imgNonDrugOrder.Visible = false;
        lnkEducationCounseling.Visible = false;
        lnkReferrals.Visible = false;
        lnkAnaesthesiaCritical.Visible = false;
        lnkMultidisciplinaryEvaluation.Visible = false;
    }
    private void OpenButtonControls()
    {
        imgBtnAddChiefComplaints.Visible = true;
        imgBtnAddAllergies.Visible = true;
        lnkImmunisationHistory.Visible = true;
        imgBtnAddVitals.Visible = true;
        ImageButton1.Visible = true;
        imgBtnTemplates.Visible = true;
        ImageButton6.Visible = true;
        lnkAddTemplates_All.Visible = true;
        imgBtnProvisionalDiagnosis.Visible = true;
        imgbtnAddOrdersAndProcedures.Visible = true;
        imgBtnAddPrescriptions.Visible = true;
        imgNonDrugOrder.Visible = true;
        lnkEducationCounseling.Visible = true;
        lnkReferrals.Visible = true;
        lnkAnaesthesiaCritical.Visible = true;
        lnkMultidisciplinaryEvaluation.Visible = true;
    }
    private void ClearEditorControls()
    {
        editorChiefComplaints.Text = string.Empty;
        txtWHistory.Text = string.Empty;
        txtWCostAnalysis.Text = string.Empty;
        txtWExamination.Text = string.Empty;
        txtWNutritionalStatus.Text = string.Empty;
        txtWPlanOfCare.Text = string.Empty;
        txtWPrevTreatment.Text = string.Empty;
        editorProvDiagnosis.Text = string.Empty;
        editorNonDrugOrder.Text = string.Empty;
        txtPHistory.Text = string.Empty;
    }
    protected void ViewHide_OnClick(object sender, EventArgs e)
    {
        ImageButton imgbtn = (ImageButton)sender;
        PanelVisibility(imgbtn.ToolTip.Trim());
    }
    private void PanelVisibility(string strtoollTip)
    {
        if (strtoollTip.Equals("Chief Complaints"))
        {
            if (pnlChiefComplaints.Visible)
            {
                imgbtnChiefComplaints.ImageUrl = "~/Images/Collapse.jpg";
                pnlChiefComplaints.Visible = false;
            }
            else
            {
                imgbtnChiefComplaints.ImageUrl = "~/Images/Expand.jpg";
                pnlChiefComplaints.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Allergies"))
        {
            if (Panel2.Visible)
            {
                ImageButton3.ImageUrl = "~/Images/Collapse.jpg";
                Panel2.Visible = false;
                tblAllergiesDetail.Visible = false;
            }
            else
            {
                ImageButton3.ImageUrl = "~/Images/Expand.jpg";
                Panel2.Visible = true;
                tblAllergiesDetail.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Vitals"))
        {
            if (pnlVitals.Visible)
            {
                imgVbtnVital.ImageUrl = "~/Images/Collapse.jpg";
                pnlVitals.Visible = false;
            }
            else
            {
                imgVbtnVital.ImageUrl = "~/Images/Expand.jpg";
                pnlVitals.Visible = true;
            }
        }
        else if (strtoollTip.Equals("History"))
        {
            if (pnlHistory.Visible)
            {
                imbtnHistory.ImageUrl = "~/Images/Collapse.jpg";
                pnlHistory.Visible = false;
            }
            else
            {
                imbtnHistory.ImageUrl = "~/Images/Expand.jpg";
                pnlHistory.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Template"))
        {
            if (Panel10.Visible)
            {
                imgbtnTemplate.ImageUrl = "~/Images/Collapse.jpg";
                Panel10.Visible = false;
            }
            else
            {
                imgbtnTemplate.ImageUrl = "~/Images/Expand.jpg";
                Panel10.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Other Notes"))
        {
            if (pnlOtherNotes.Visible)
            {
                imgbtntherNotes.ImageUrl = "~/Images/Collapse.jpg";
                pnlOtherNotes.Visible = false;
            }
            else
            {
                imgbtntherNotes.ImageUrl = "~/Images/Expand.jpg";
                pnlOtherNotes.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Provisional Diagnosis"))
        {
            if (pnlProvisionalDiagnosis.Visible)
            {
                imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
                pnlProvisionalDiagnosis.Visible = false;
            }
            else
            {
                imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Expand.jpg";
                pnlProvisionalDiagnosis.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Orders And Procedures"))
        {
            if (pnlOrderProcedures.Visible)
            {
                imgbtnOrdersAndProcedures.ImageUrl = "~/Images/Collapse.jpg";
                pnlOrderProcedures.Visible = false;
                gvOrdersAndProcedures.Visible = false;
            }
            else
            {
                imgbtnOrdersAndProcedures.ImageUrl = "~/Images/Expand.jpg";
                pnlOrderProcedures.Visible = true;
                gvOrdersAndProcedures.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Prescriptions"))
        {
            if (pnlPrescription.Visible)
            {
                imgbtnPrescription.ImageUrl = "~/Images/Collapse.jpg";
                pnlPrescription.Visible = false;
            }
            else
            {
                imgbtnPrescription.ImageUrl = "~/Images/Expand.jpg";
                pnlPrescription.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Previous Treatment"))
        {
            if (Panel3.Visible)
            {
                ImageButton2.ImageUrl = "~/Images/Collapse.jpg";
                Panel3.Visible = false;
            }
            else
            {
                ImageButton2.ImageUrl = "~/Images/Expand.jpg";
                Panel3.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Nutritional Status"))
        {
            if (Panel6.Visible)
            {
                ibtNutritionalStatus.ImageUrl = "~/Images/Collapse.jpg";
                Panel6.Visible = false;
            }
            else
            {
                ibtNutritionalStatus.ImageUrl = "~/Images/Expand.jpg";
                Panel6.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Plan Of Care"))
        {
            if (Panel13.Visible)
            {
                ImageButton4.ImageUrl = "~/Images/Collapse.jpg";
                Panel13.Visible = false;
            }
            else
            {
                ImageButton4.ImageUrl = "~/Images/Expand.jpg";
                Panel13.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Cost Analysis"))
        {
            if (Panel14.Visible)
            {
                ImageButton7.ImageUrl = "~/Images/Collapse.jpg";
                Panel14.Visible = false;
            }
            else
            {
                ImageButton7.ImageUrl = "~/Images/Expand.jpg";
                Panel14.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Non Drug Order"))
        {
            if (Panel20.Visible)
            {
                ImageButton7.ImageUrl = "~/Images/Collapse.jpg";
                Panel20.Visible = false;
            }
            else
            {
                ImageButton7.ImageUrl = "~/Images/Expand.jpg";
                Panel20.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Final Diagnosis"))
        {
            if (pnlDiagnosis.Visible)
            {
                ImageButton13.ImageUrl = "~/Images/Collapse.jpg";
                pnlDiagnosis.Visible = false;
            }
            else
            {
                ImageButton13.ImageUrl = "~/Images/Expand.jpg";
                pnlDiagnosis.Visible = true;
            }
        }
        else if (strtoollTip.Equals("Past History"))
        {
            if (Panel23.Visible)
            {
                ImageButton14.ImageUrl = "~/Images/Collapse.jpg";
                Panel23.Visible = false;
            }
            else
            {
                ImageButton14.ImageUrl = "~/Images/Expand.jpg";
                Panel23.Visible = true;
            }
        }
    }
    private void BindCommonData(string sTemplateName, string sTemplateType, string sTemplateCode, int iTemplateId, int PageNo)
    {
        DataSet ds = new DataSet();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString); // 5 kk
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
        dvErdata.Visible = false;
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRDataForSingleScreen";
            APIRootClass.GetEMRDataForSingleScreen objRoot = new global::APIRootClass.GetEMRDataForSingleScreen();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.sTemplateType = sTemplateType;
            objRoot.iTemplateId = iTemplateId;
            objRoot.sTemplateName = sTemplateName;
            objRoot.EncounterDate = common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");
            objRoot.ToDate = common.myDate(DateTime.Now).ToString("yyyy/MM/dd");
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.pageSize = pageSize;
            objRoot.PageNo = PageNo;
            objRoot.IsCopyLastOPDSummary = false;

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            var sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);




            //ds = objEMR.GetEMRDataForSingleScreen(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //                    common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]), sTemplateType, iTemplateId, sTemplateName,
            //                    common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"), common.myDate(DateTime.Now).ToString("yyyy/MM/dd"),
            //                    pageSize, PageNo);


            if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals(string.Empty))
            {
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[0].NewRow();
                    //ds.Tables[0].Rows.Add(dr);
                    //gvProblemDetails.DataSource = ds.Tables[0];
                    //gvProblemDetails.DataBind();
                    //dr = null;
                }
                else
                {
                    gvProblemDetails.DataSource = ds.Tables[0];
                    gvProblemDetails.DataBind();
                }
                bindAllergies(ds);
                if (ds.Tables[3].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[3].NewRow();
                    //ds.Tables[3].Rows.Add(dr);
                    //gvData.DataSource = ds.Tables[3];
                    //gvData.DataBind();
                    //dr = null;
                }
                else
                {
                    gvData.DataSource = ds.Tables[3];
                    gvData.DataBind();
                }
                if (ds.Tables[4].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[4].NewRow();
                    //ds.Tables[4].Rows.Add(dr);
                    //gvOrdersAndProcedures.DataSource = ds.Tables[4];
                    //gvOrdersAndProcedures.DataBind();
                    //populatePager(rptPagerOrdersAndProcedures, 0, PageNo, pageSize);
                    //dr = null;
                }
                else
                {
                    gvOrdersAndProcedures.DataSource = ds.Tables[4];
                    gvOrdersAndProcedures.DataBind();
                    populatePager(rptPagerOrdersAndProcedures, common.myInt(ds.Tables[4].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                }
                if (ds.Tables[5].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[5].NewRow();
                    //ds.Tables[5].Rows.Add(dr);
                    //gvPrescriptions.DataSource = ds.Tables[5];
                    //gvPrescriptions.DataBind();
                    //populatePager(rptPagerPrescriptions, 0, PageNo, pageSize);
                    //dr = null;
                }
                else
                {
                    ViewState["GridDataDetail"] = ds.Tables[6];
                    gvPrescriptions.DataSource = ds.Tables[5];
                    gvPrescriptions.DataBind();
                    populatePager(rptPagerPrescriptions, common.myInt(ds.Tables[5].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                }
                if (ds.Tables[7].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[7].NewRow();
                    //ds.Tables[7].Rows.Add(dr);
                    ////gvVitals.DataSource = ds.Tables[7];
                    ////gvVitals.DataBind();
                    //populatePager(rptPagerVitals, 0, PageNo, pageSize);
                    //dr = null;
                    if (lnktriageform.Visible == true)
                    {
                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEvitalresult";
                        APIRootClass.GetEvitalresult objRoot1 = new global::APIRootClass.GetEvitalresult();
                        objRoot1.EncounterID = common.myInt(Session["EncounterID"]);


                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;

                        inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                        sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        DataSet dsx = JsonConvert.DeserializeObject<DataSet>(sValue);


                        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); // 6 kk
                        //DataSet dsx = dl.FillDataSet(CommandType.Text, "Exec uspGetEvitalresult @encounterID=" + common.myInt(Session["EncounterID"]));
                        for (int i = 0; i < dsx.Tables[0].Rows.Count; i++)
                        {
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "T")
                            {
                                TxtTemperature.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "BMI")
                            {
                                txtBMI.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                //hdnBMI.Value = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "SPO2")
                            {
                                txtSpO2.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "MAP")
                            {
                                txtMAC.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "BPD")
                            {
                                txtBPDiastolic.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "BPS")
                            {
                                txtBPSystolic.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "P")
                            {
                                txtPulse.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "Res")
                            {
                                txtRespiration.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }

                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "HT")
                            {
                                txtHeight.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                //hdnHeight.Value = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }
                            if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "WT")
                            {
                                TxtWeight.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                //hdnWeight.Value = dsx.Tables[0].Rows[i]["Result"].ToString();
                            }

                        }

                        if (dsx.Tables[1].Rows.Count != 0)
                        {
                            txtBMI.Text = dsx.Tables[1].Rows[0]["BMI"].ToString();
                            //hdnBMI.Value = dsx.Tables[1].Rows[0]["BMI"].ToString();

                            txtRespiration.Text = dsx.Tables[1].Rows[0]["Resp"].ToString();



                            txtHeight.Text = dsx.Tables[1].Rows[0]["HT"].ToString();
                            //hdnHeight.Value = dsx.Tables[1].Rows[0]["HT"].ToString();

                            TxtWeight.Text = dsx.Tables[1].Rows[0]["WT"].ToString();
                            //hdnWeight.Value = dsx.Tables[1].Rows[0]["WT"].ToString();
                            String strData = "<b>Triage Details</b></br><b>CheifCompl:</b>" + dsx.Tables[1].Rows[0]["CheifCompl"].ToString();
                            strData = strData + "</br><b>Initial Observation:</b>" + dsx.Tables[1].Rows[0]["IO"].ToString();
                            dvErdata.Visible = true;
                            dvErdata.InnerHtml = strData;
                        }

                    }
                }
                else
                {
                    if (common.myStr(ds.Tables[7].Rows[0]["HT"]) == "")
                    {
                        if (lnktriageform.Visible == true)
                        {
                            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEvitalresult";
                            APIRootClass.GetEvitalresult objRoot1 = new global::APIRootClass.GetEvitalresult();
                            objRoot1.EncounterID = common.myInt(Session["EncounterID"]);


                            client = new WebClient();
                            client.Headers["Content-type"] = "application/json";
                            client.Encoding = Encoding.UTF8;

                            inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                            sValue = client.UploadString(ServiceURL, inputJson);
                            sValue = JsonConvert.DeserializeObject<string>(sValue);
                            DataSet dsx = JsonConvert.DeserializeObject<DataSet>(sValue);

                            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); // 7 kk
                            //DataSet dsx = dl.FillDataSet(CommandType.Text, "Exec uspGetEvitalresult @encounterID=" + common.myInt(Session["EncounterID"]));
                            for (int i = 0; i < dsx.Tables[0].Rows.Count; i++)
                            {
                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "T")
                                {
                                    TxtTemperature.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }

                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "SPO2")
                                {
                                    txtSpO2.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }
                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "MAP")
                                {
                                    txtMAC.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }
                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "BPD")
                                {
                                    txtBPDiastolic.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }
                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "BPS")
                                {
                                    txtBPSystolic.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }
                                if (dsx.Tables[0].Rows[i]["VitalName"].ToString() == "P")
                                {
                                    txtPulse.Text = dsx.Tables[0].Rows[i]["Result"].ToString();
                                }

                            }
                            if (dsx.Tables[1].Rows.Count != 0)
                            {
                                txtBMI.Text = dsx.Tables[1].Rows[0]["BMI"].ToString();
                                //hdnBMI.Value = dsx.Tables[1].Rows[0]["BMI"].ToString();

                                txtRespiration.Text = dsx.Tables[1].Rows[0]["Resp"].ToString();



                                txtHeight.Text = dsx.Tables[1].Rows[0]["HT"].ToString();
                                //hdnHeight.Value = dsx.Tables[1].Rows[0]["HT"].ToString();

                                TxtWeight.Text = dsx.Tables[1].Rows[0]["WT"].ToString();
                                //hdnWeight.Value = dsx.Tables[1].Rows[0]["WT"].ToString();
                                String strData = "<b>Triage Details</b></br><b>CheifCompl:</b>" + dsx.Tables[1].Rows[0]["CheifCompl"].ToString();
                                strData = strData + "</br><b>Initial Observation:</b>" + dsx.Tables[1].Rows[0]["IO"].ToString();
                                dvErdata.Visible = true;
                                dvErdata.InnerHtml = strData;
                            }
                        }
                    }

                    if (ds.Tables[7].Rows.Count > 0)
                        gvVitals.DataSource = ds.Tables[7];
                    else
                        gvVitals.DataSource = null;
                    gvVitals.DataBind();
                    populatePager(rptPagerVitals, common.myInt(ds.Tables[7].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                }
                if (ds.Tables[8].Rows.Count.Equals(0))
                {
                    //DataRow dr = ds.Tables[8].NewRow();
                    //ds.Tables[8].Rows.Add(dr);
                    //gvNonDrugOrder.DataSource = ds.Tables[8];
                    //gvNonDrugOrder.DataBind();
                    //dr = null;
                }
                else
                {
                    gvNonDrugOrder.DataSource = ds.Tables[8];
                    gvNonDrugOrder.DataBind();
                }
                dvFilterHistory = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterHistory.RowFilter = "TemplateCode='HIS' AND IsFreeTextTemplate=1";
                if (dvFilterHistory.ToTable().Rows.Count > 0)
                {
                    gvHistory.DataSource = dvFilterHistory.ToTable();
                    gvHistory.DataBind();
                }
                else
                {
                    gvHistory.DataSource = BindHistoryAndExaminationGrid();
                    gvHistory.DataBind();
                }
                dvFilterHistory.Dispose();
                dvFilterHistory = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterHistory.RowFilter = "TemplateCode='PHIS' AND IsFreeTextTemplate=1";
                if (dvFilterHistory.ToTable().Rows.Count > 0)
                {
                    gvPHistory.DataSource = dvFilterHistory.ToTable();
                    gvPHistory.DataBind();
                }
                else
                {
                    gvPHistory.DataSource = BindHistoryAndExaminationGrid();
                    gvPHistory.DataBind();
                }
                dvFilterHistory.Dispose();
                dvPreviousTreatment = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvPreviousTreatment.RowFilter = "TemplateCode='PT' AND IsFreeTextTemplate=1";
                if (dvPreviousTreatment.ToTable().Rows.Count > 0)
                {
                    gvPrevTreatment.DataSource = dvPreviousTreatment.ToTable();
                    gvPrevTreatment.DataBind();
                }
                else
                {
                    gvPrevTreatment.DataSource = BindHistoryAndExaminationGrid();
                    gvPrevTreatment.DataBind();
                }
                dvPreviousTreatment.Dispose();
                dvCostAnalysis = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvCostAnalysis.RowFilter = "TemplateCode='CA' AND IsFreeTextTemplate=1";
                if (dvCostAnalysis.ToTable().Rows.Count > 0)
                {
                    gvCostAnalysis.DataSource = dvCostAnalysis.ToTable();
                    gvCostAnalysis.DataBind();
                }
                else
                {
                    gvCostAnalysis.DataSource = BindHistoryAndExaminationGrid();
                    gvCostAnalysis.DataBind();
                }
                dvCostAnalysis.Dispose();
                dvFilterExamination = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterExamination.RowFilter = "TemplateCode='EXM' AND IsFreeTextTemplate=1";
                if (dvFilterExamination.ToTable().Rows.Count > 0)
                {
                    gvExamination.DataSource = dvFilterExamination.ToTable();
                    gvExamination.DataBind();
                }
                else
                {
                    gvExamination.DataSource = BindHistoryAndExaminationGrid();
                    gvExamination.DataBind();
                }
                dvFilterExamination.Dispose();
                dvFilterNutritional = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterNutritional.RowFilter = "TemplateCode='NS' AND IsFreeTextTemplate=1";
                if (dvFilterNutritional.ToTable().Rows.Count > 0)
                {
                    gvNutritional.DataSource = dvFilterNutritional.ToTable();
                    gvNutritional.DataBind();
                }
                else
                {
                    gvNutritional.DataSource = BindHistoryAndExaminationGrid();
                    gvNutritional.DataBind();
                }
                dvFilterNutritional.Dispose();
                dvFilterPlanOfCare = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterPlanOfCare.RowFilter = "TemplateCode='POC' AND IsFreeTextTemplate=1";
                if (dvFilterPlanOfCare.ToTable().Rows.Count > 0)
                {
                    gvPlanOfCare.DataSource = dvFilterPlanOfCare.ToTable();
                    gvPlanOfCare.DataBind();
                }
                else
                {
                    gvPlanOfCare.DataSource = BindHistoryAndExaminationGrid();
                    gvPlanOfCare.DataBind();
                }
                dvFilterPlanOfCare.Dispose();
                dvFilterOtherNotes = new DataView(ds.Tables[9].Copy());
                if (ds.Tables[9].Rows.Count > 0)
                    dvFilterOtherNotes.RowFilter = "(TemplateCode<>'CA' AND TemplateCode<>'PT' AND TemplateCode<>'NS' AND TemplateCode<>'POC' AND IsFreeTextTemplate=0) OR ISNULL(TemplateCode,'')=''";
                if (dvFilterOtherNotes.ToTable().Rows.Count > 0)
                {
                    gvOtherNotes.DataSource = dvFilterOtherNotes.ToTable();
                    gvOtherNotes.DataBind();
                }
                else
                {
                    gvOtherNotes.DataSource = BindHistoryAndExaminationGrid();
                    gvOtherNotes.DataBind();
                }
                dvFilterOtherNotes.Dispose();
            }
            else
            {
                if ((sTemplateName.Equals("Chief Complaints") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[0].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[0].NewRow();
                        //ds.Tables[0].Rows.Add(dr);
                        //gvProblemDetails.DataSource = ds.Tables[0];
                        //gvProblemDetails.DataBind();
                        //dr = null;
                    }
                    else
                    {
                        gvProblemDetails.DataSource = ds.Tables[0];
                        gvProblemDetails.DataBind();
                    }
                }
                if ((sTemplateName.Equals("Allergies") && sTemplateType.Equals("S")))
                {
                    bindAllergies(ds);
                }
                if ((sTemplateName.Equals("Provisional Diagnosis") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[3].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[3].NewRow();
                        //ds.Tables[3].Rows.Add(dr);
                        //gvData.DataSource = ds.Tables[3];
                        //gvData.DataBind();
                        //dr = null;
                    }
                    else
                    {
                        gvData.DataSource = ds.Tables[3];
                        gvData.DataBind();
                    }
                }
                if ((sTemplateName.Equals("Orders And Procedures") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[4].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[4].NewRow();
                        //ds.Tables[4].Rows.Add(dr);
                        //gvOrdersAndProcedures.DataSource = ds.Tables[4];
                        //gvOrdersAndProcedures.DataBind();
                        //populatePager(rptPagerOrdersAndProcedures, 0, PageNo, pageSize);
                        //dr = null;
                    }
                    else
                    {
                        gvOrdersAndProcedures.DataSource = ds.Tables[4];
                        gvOrdersAndProcedures.DataBind();
                        populatePager(rptPagerOrdersAndProcedures, common.myInt(ds.Tables[4].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                    }
                }
                if ((sTemplateName.Equals("Prescription") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[5].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[5].NewRow();
                        //ds.Tables[5].Rows.Add(dr);
                        //gvPrescriptions.DataSource = ds.Tables[5];
                        //gvPrescriptions.DataBind();
                        //populatePager(rptPagerPrescriptions, 0, PageNo, pageSize);
                        //dr = null;
                    }
                    else
                    {
                        ViewState["GridDataDetail"] = ds.Tables[6];
                        gvPrescriptions.DataSource = ds.Tables[5];
                        gvPrescriptions.DataBind();
                        populatePager(rptPagerPrescriptions, common.myInt(ds.Tables[5].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                    }
                }
                if ((sTemplateName.Equals("Vitals") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[7].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[7].NewRow();
                        //ds.Tables[7].Rows.Add(dr);
                        //gvVitals.DataSource = ds.Tables[7];
                        //gvVitals.DataBind();
                        //populatePager(rptPagerVitals, 0, PageNo, pageSize);
                        //dr = null;
                    }
                    else
                    {
                        gvVitals.DataSource = ds.Tables[7];
                        gvVitals.DataBind();
                        populatePager(rptPagerVitals, common.myInt(ds.Tables[7].Rows[0]["TotalRecordsCount"]), PageNo, pageSize);
                    }
                }
                if (sTemplateName.Equals("Non Drug Order") && sTemplateType.Equals("S"))
                {
                    if (ds.Tables[8].Rows.Count.Equals(0))
                    {
                        //DataRow dr = ds.Tables[8].NewRow();
                        //ds.Tables[8].Rows.Add(dr);
                        //gvNonDrugOrder.DataSource = ds.Tables[8];
                        //gvNonDrugOrder.DataBind();
                        //dr = null;
                    }
                    else
                    {
                        gvNonDrugOrder.DataSource = ds.Tables[8];
                        gvNonDrugOrder.DataBind();
                    }
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("HIS"))
                {
                    dvFilterHistory = new DataView(ds.Tables[9].Copy());
                    dvFilterHistory.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterHistory.ToTable().Rows.Count > 0)
                    {
                        gvHistory.DataSource = dvFilterHistory.ToTable();
                        gvHistory.DataBind();
                    }
                    else
                    {
                        gvHistory.DataSource = BindHistoryAndExaminationGrid();
                        gvHistory.DataBind();
                    }
                    dvFilterHistory.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("PHIS"))
                {
                    dvFilterHistory = new DataView(ds.Tables[9].Copy());
                    dvFilterHistory.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterHistory.ToTable().Rows.Count > 0)
                    {
                        gvPHistory.DataSource = dvFilterHistory.ToTable();
                        gvPHistory.DataBind();
                    }
                    else
                    {
                        gvPHistory.DataSource = BindHistoryAndExaminationGrid();
                        gvPHistory.DataBind();
                    }
                    dvFilterHistory.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("PT"))
                {
                    dvFilterPreTreatment = new DataView(ds.Tables[9].Copy());
                    dvFilterPreTreatment.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterPreTreatment.ToTable().Rows.Count > 0)
                    {
                        gvPrevTreatment.DataSource = dvFilterPreTreatment.ToTable();
                        gvPrevTreatment.DataBind();
                    }
                    else
                    {
                        gvPrevTreatment.DataSource = BindHistoryAndExaminationGrid();
                        gvPrevTreatment.DataBind();
                    }
                    dvFilterPreTreatment.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("CA"))
                {
                    dvFiltergvCostAnalysis = new DataView(ds.Tables[9].Copy());
                    dvFiltergvCostAnalysis.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFiltergvCostAnalysis.ToTable().Rows.Count > 0)
                    {
                        gvCostAnalysis.DataSource = dvFiltergvCostAnalysis.ToTable();
                        gvCostAnalysis.DataBind();
                    }
                    else
                    {
                        gvCostAnalysis.DataSource = BindHistoryAndExaminationGrid();
                        gvCostAnalysis.DataBind();
                    }
                    dvFiltergvCostAnalysis.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("EXM"))
                {
                    dvFilterExamination = new DataView(ds.Tables[9].Copy());
                    dvFilterExamination.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterExamination.ToTable().Rows.Count > 0)
                    {
                        gvExamination.DataSource = dvFilterExamination.ToTable();
                        gvExamination.DataBind();
                    }
                    else
                    {
                        gvExamination.DataSource = BindHistoryAndExaminationGrid();
                        gvExamination.DataBind();
                    }
                    dvFilterExamination.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("NS"))
                {
                    dvFilterNutritional = new DataView(ds.Tables[9].Copy());
                    dvFilterNutritional.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterNutritional.ToTable().Rows.Count > 0)
                    {
                        gvNutritional.DataSource = dvFilterNutritional.ToTable();
                        gvNutritional.DataBind();
                    }
                    else
                    {
                        gvNutritional.DataSource = BindHistoryAndExaminationGrid();
                        gvNutritional.DataBind();
                    }
                    dvFilterNutritional.Dispose();
                }
                if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("POC"))
                {
                    dvFilterPlanOfCare = new DataView(ds.Tables[9].Copy());
                    dvFilterPlanOfCare.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                    if (dvFilterPlanOfCare.ToTable().Rows.Count > 0)
                    {
                        gvPlanOfCare.DataSource = dvFilterPlanOfCare.ToTable();
                        gvPlanOfCare.DataBind();
                    }
                    else
                    {
                        gvPlanOfCare.DataSource = BindHistoryAndExaminationGrid();
                        gvPlanOfCare.DataBind();
                    }
                    dvFilterPlanOfCare.Dispose();
                }
                if (ds.Tables[9] != null && sTemplateType.Equals("D"))
                {
                    dvFilterOtherNotes = new DataView(ds.Tables[9].Copy());
                    dvFilterOtherNotes.RowFilter = "(TemplateCode<>'CA' AND TemplateCode<>'PT' AND TemplateCode<>'NS' AND TemplateCode<>'POC' AND IsFreeTextTemplate=0) OR ISNULL(TemplateCode,'')=''";
                    if (dvFilterOtherNotes.ToTable().Rows.Count > 0)
                    {
                        gvOtherNotes.DataSource = dvFilterOtherNotes.ToTable();
                        gvOtherNotes.DataBind();
                    }
                    else
                    {
                        gvOtherNotes.DataSource = BindHistoryAndExaminationGrid();
                        gvOtherNotes.DataBind();
                    }
                    dvFilterOtherNotes.Dispose();
                }
            }
            if (ds.Tables[10] != null)
            {
                gvDiagnosisDetails.DataSource = ds.Tables[10];
                gvDiagnosisDetails.DataBind();
            }
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
            //objEMR = null;
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
        DataRow dr = dt.NewRow();
        dr["TemplateName"] = string.Empty;
        dr["RecordId"] = string.Empty;
        dr["DocDate"] = string.Empty;
        dr["EncodedBy"] = string.Empty;
        dr["TemplateId"] = string.Empty;
        dr["TemplateType"] = string.Empty;
        dt.Rows.Add(dr);
        return dt;
    }
    #endregion
    #region Chief Complaints
    protected void gvProblemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowState == DataControlRowState.Alternate)
        {
            e.Row.Cells[(byte)enumProblem.IsHPI].Visible = false;
        }
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            try
            {
                HiddenField hdnProblemId = (HiddenField)e.Row.FindControl("hdnProblemId");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                HiddenField hdnProblem = (HiddenField)e.Row.FindControl("hdnProblem");
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumProblem.Edit].Controls[0];
                if (common.myInt(hdnProblemId.Value).Equals(0))
                {
                    ibtnDelete.Enabled = false;
                }
                LinkButton lblHPI = (LinkButton)e.Row.FindControl("lblHPI");
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    ibtnDelete.Visible = false;
                }
                TextBox editorProblem = (TextBox)e.Row.FindControl("editorProblem");
                editorProblem.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorProblem.Text = common.clearHTMLTags(hdnProblem.Value);
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                        ibtnDelete.Visible = false;
                    }
                }
                setControlHeight(editorProblem, 80);
            }
            catch (Exception Ex)
            {
                clsExceptionLog objException = new clsExceptionLog();
                objException.HandleException(Ex);
                objException = null;
            }
        }
    }
    protected void gvProblemDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Del"))
            {
                if (!common.myBool(ViewState["DeleteEnable"]))
                {
                    Alert.ShowAjaxMsg("You are not authorized to delete", Page);
                    return;
                }
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int intId = common.myInt(((HiddenField)row.FindControl("hdnProblemId")).Value);
                ViewState["strId"] = intId;
                if (intId > 0)
                {
                    dvConfirmCancelOptions.Visible = true;
                }
            }
            if (e.CommandName.Equals("HPI") || e.CommandName.Equals("ISHPI"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int intProblemId = common.myInt(((HiddenField)row.FindControl("hdnProblemId")).Value);
                if (intProblemId > 0)
                {
                    RadWindowForNew.NavigateUrl = "../Problems/hpi.aspx?Mpg=P98&PopUp=Yes&ProbId=" + intProblemId;
                    RadWindowForNew.Width = 1200;
                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = "addHPIClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void lnkAddChiefComplaints_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Problems/Default.aspx?IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addChiefComplaintsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddChiefComplaintsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            RetrievePatientProblemsDetail();
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
    private void RetrievePatientProblemsDetail()
    {
        BindCommonData("Chief Complaints", "S", string.Empty, 0, 1);
    }
    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        //BaseC.EMRProblems objProb = new BaseC.EMRProblems(sConString);
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/Canceltodayproblem";
            APIRootClass.Canceltodayproblem objRoot = new global::APIRootClass.Canceltodayproblem();
            objRoot.ProblemId = common.myInt(ViewState["strId"]);
            objRoot.RegistrationID = common.myInt(Session["RegistrationID"]);
            objRoot.Encounterid = common.myInt(Session["encounterid"]);
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
            objRoot.FacilityId = common.myInt(Session["FacilityID"]);
            objRoot.Pageid = common.myInt(ViewState["PageId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.Shownote = 0;

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);


            //objProb.Canceltodayproblem(common.myInt(ViewState["strId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), 
            //    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), 
            //    common.myInt(Session["UserID"]), common.myInt(0));
            RetrievePatientProblemsDetail();
            dvConfirmCancelOptions.Visible = false;
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
            //objProb = null;
        }
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }
    protected void gvProblemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProblemDetails.PageIndex = e.NewPageIndex;
        RetrievePatientProblemsDetail();
    }
    protected void btnAddHPIClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            RetrievePatientProblemsDetail();
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
    protected void gvProblemDetails_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvProblemDetails.EditIndex = -1;
        BindCommonData("Chief Complaints", "S", string.Empty, 0, 1);
    }
    protected void gvProblemDetails_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        //BaseC.ParseData objParse = new BaseC.ParseData();
        ArrayList col = new ArrayList();
        StringBuilder objXMLProblem = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            HiddenField hdnProblemId = (HiddenField)gvProblemDetails.Rows[e.RowIndex].FindControl("hdnProblemId");
            TextBox editorProblem = (TextBox)gvProblemDetails.Rows[e.RowIndex].FindControl("editorProblem");
            if (common.myInt(hdnProblemId.Value).Equals(0))
            {
                return;
            }
            txtedit.Text = common.myStr(hdnProblemId.Value);
            #region  Problem
            if (!common.myStr(editorProblem.Text).Equals(string.Empty))
            {
                string editID;
                if (!(common.myStr(txtedit.Text).Trim().Equals(string.Empty)))
                {
                    editID = common.myStr(txtedit.Text);
                }
                else
                {
                    editID = string.Empty;
                }
                string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate") ? common.myStr(Request.QueryString["TemplateFieldId"]) : "0";
                string strProblem = common.ParseString(editorProblem.Text).Trim().Replace("\n", "<br/>");

                if (common.myLen(strProblem) > 2000)
                {
                    Alert.ShowAjaxMsg("Chief complaints (free text) length must be less than 2000 character!", this.Page);
                    return;
                }
                col.Add(editID);//Id
                col.Add(0);//ProblemId
                col.Add(strProblem);//Problem
                col.Add(0);//DurationID
                col.Add(string.Empty);//Duration
                col.Add(0);//ContextID
                col.Add(string.Empty);//Context
                col.Add(0);//SeverityId
                col.Add(string.Empty);//Severity
                col.Add(0);//IsPrimary
                col.Add(0);//IsChronic
                col.Add(common.myStr(Session["DoctorID"]));//DoctorId
                col.Add(common.myStr(Session["FacilityId"]));//FacilityId
                col.Add(0);//SCTId
                col.Add(string.Empty);//QualityIDs
                col.Add(0);//LocationID
                col.Add(string.Empty);//Location
                col.Add(0);//OnsetID
                col.Add(0);//AssociatedProblemId1
                col.Add(string.Empty);//AssociatedProblem1
                col.Add(0);//AssociatedProblemId2
                col.Add(string.Empty);//AssociatedProblem2
                col.Add(0);//AssociatedProblemId3
                col.Add(string.Empty);//AssociatedProblem3
                col.Add(0);//AssociatedProblemId4
                col.Add(string.Empty);//AssociatedProblem4
                col.Add(0);//AssociatedProblemId5
                col.Add(string.Empty);//AssociatedProblem5
                col.Add(string.Empty);//Side
                col.Add(0);//ConditionId
                col.Add(0);//Percentage
                col.Add(0);//Durations
                col.Add(string.Empty);//DurationType
                col.Add(TemplateId);//TemplateFieldId
                col.Add(0);//ComplaintSearchId
                objXMLProblem.Append(common.setXmlTable(ref col));
            }
            #endregion

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
            APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.DoctorId = common.myInt(Session["DoctorID"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.xmlProblemDetails = objXMLProblem.ToString();
            objRoot.sProvisionalDiagnosis = string.Empty;
            objRoot.xmlVitalString = string.Empty;
            objRoot.strXMLDrug = string.Empty;
            objRoot.strXMLOther = string.Empty;
            objRoot.ProvisionalDiagnosisId = 0;
            objRoot.DiagnosisSearchId = 0;
            objRoot.bitNKDA = 0;
            objRoot.xmlTemplateDetails = string.Empty;
            objRoot.iSign = 0;
            objRoot.xmlNonDrugOrder = string.Empty;


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            //Hashtable HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
            //                       common.myInt(Session["UserId"]), objXMLProblem.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, string.Empty, 0, string.Empty);
            gvProblemDetails.EditIndex = -1;
            BindCommonData("Chief Complaints", "S", string.Empty, 0, 1);
            if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = sValue;
                lblChiefMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblChiefMessage.Text = sValue;
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
            //objParse = null;
            col = null;
            objXMLProblem = null;
            txtedit.Text = string.Empty;
        }
    }
    protected void gvProblemDetails_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
        {
            Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
            return;
        }
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
        {
            return;
        }
        else
        {
            GridViewRow row = gvProblemDetails.Rows[e.NewEditIndex];
            HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
            if (common.myInt(hdnEncodedById.Value) > 0)
            {
                gvProblemDetails.EditIndex = e.NewEditIndex;
                BindCommonData("Chief Complaints", "S", string.Empty, 0, 1);
            }
        }
    }
    #endregion
    #region Allergies
    private bool isSavedAllergy()
    {
        bool isSave = true;
        string strmsg = string.Empty;
        if (common.myInt(Session["RegistrationId"]).Equals(0))
        {
            strmsg += "Registration not selected ! ";
            isSave = false;
        }
        if (common.myStr(hdnAllergyType.Value).Trim().Equals(string.Empty) || common.myInt(hdnItemId.Value).Equals(0))
        {
            strmsg += "Allergy not selected ! ";
            isSave = false;
        }
        if (common.myInt(ddlAllergySeverity.SelectedValue).Equals(0))
        {
            strmsg += "Interaction severity not selected ! ";
            isSave = false;
        }
        lblAllergyMessage.Text = strmsg;
        return isSave;
    }
    public void bindAllergies(DataSet dsAllergy)
    {
        ClearMessageControl();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbDrugAllergy = new StringBuilder();
        StringBuilder sbOtherAllergy = new StringBuilder();
        DataView dvDrug = new DataView();
        DataView dvOther = new DataView();
        try
        {
            int t = 0;
            if (dsAllergy.Tables[1] != null && dsAllergy.Tables[1].Rows.Count > 0)
            {
                dvDrug = new DataView(dsAllergy.Tables[1]);
                dvDrug.RowFilter = "AllergyType IN ('Generic','Drug','CIMS','VIDAL')";
                dvOther = new DataView(dsAllergy.Tables[1]);
                dvOther.RowFilter = "AllergyType NOT IN ('Generic','Drug','CIMS','VIDAL')";
                if (dsAllergy.Tables[2].Rows.Count > 0)
                {
                    if (common.myBool(dsAllergy.Tables[2].Rows[0]["NoAllergies"]))
                    {
                        sb.Append("Allergies: " + Environment.NewLine);
                    }
                    if (!common.myBool(dsAllergy.Tables[2].Rows[0]["NoAllergies"]))
                    {
                        foreach (DataRowView dr in dvDrug)
                        {
                            if (t.Equals(0))
                            {
                                t = 1;
                            }
                            sbDrugAllergy.Append(" " + common.myStr(dr["AllergyName"]) + " (" + ((common.myInt(dr["Generic_Id"]) > 0) ? "Generic" : "Generic: " + common.myStr(dr["Generic_Name"])) + ")");
                            if (!common.myStr(dr["AllergyDate"]).Equals(string.Empty))
                            {
                                sbDrugAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]));
                            }
                            if (!common.myStr(dr["Reaction"]).Equals(string.Empty))
                            {
                                if (!common.myBool(dr["Intolerance"]) && common.myStr(dr["Remarks"]).Equals(string.Empty))
                                {
                                    sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]) + ".");
                                }
                                else
                                {
                                    sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]));
                                }
                            }
                            if (!common.myStr(dr["AllergySeverity"]).Equals(string.Empty))
                            {
                                sbDrugAllergy.Append(", Severity level : " + common.myStr(dr["AllergySeverity"]));
                            }
                            if (common.myBool(dr["Intolerance"]))
                            {
                                if (common.myStr(dr["Remarks"]).Equals(string.Empty))
                                {
                                    sbDrugAllergy.Append(", Intolerable.");
                                }
                                else
                                {
                                    sbDrugAllergy.Append(", Intolerable");
                                }
                            }
                            if (!common.myStr(dr["Remarks"]).Equals(string.Empty))
                            {
                                sbDrugAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]) + ".");
                            }
                            sbDrugAllergy.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        chkNoAllergies.Checked = common.myBool(dsAllergy.Tables[2].Rows[0]["NoAllergies"]);
                        chkNoAllergies_OnCheckedChanged(null, null);
                        sbDrugAllergy.Append(Environment.NewLine + " No Allergies." + Environment.NewLine);
                    }
                }
                t = 0;
                foreach (DataRowView dr in dvOther)
                {
                    if (t.Equals(0))
                    {
                        sbOtherAllergy.Append(Environment.NewLine + "Food/ Other Allergies: " + Environment.NewLine);
                        t = 1;
                    }
                    sbOtherAllergy.Append(" " + common.myStr(dr["AllergyName"]));
                    if (!common.myStr(dr["AllergyDate"]).Equals(string.Empty))
                    {
                        sbOtherAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]));
                    }
                    if (!common.myStr(dr["Reaction"]).Equals(string.Empty))
                    {
                        if (!common.myBool(dr["Intolerance"]) && common.myStr(dr["Remarks"]).Equals(string.Empty))
                        {
                            sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]) + ".");
                        }
                        else
                        {
                            sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]));
                        }
                    }
                    if (common.myBool(dr["Intolerance"]))
                    {
                        if (common.myStr(dr["Remarks"]).Equals(string.Empty))
                            sbOtherAllergy.Append(", Intolerable.");
                        else
                            sbOtherAllergy.Append(", Intolerable");
                    }
                    if (!common.myStr(dr["Remarks"]).Equals(string.Empty))
                    {
                        sbOtherAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]) + ".");
                    }
                    sbOtherAllergy.Append(Environment.NewLine);
                }
                sb.Append(sbDrugAllergy);
                sb.Append(sbOtherAllergy);
                editorAllergy.Text = sb.ToString();
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
            sb = null;
            sbDrugAllergy = null;
            sbOtherAllergy = null;
            dvDrug.Dispose();
            dvOther.Dispose();
        }
    }
    protected void lnkAddAllergies_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "/EMR/Allergy/Allergy.aspx?Regno=" + common.myInt(Session["RegistrationNo"]) + "&Encno=" + common.myStr(Session["EncounterNo"]) + "&IsEMRPopUp=1&Source=IPD";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addAllergiesOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddAllergiesClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindCommonData("Allergies", "S", string.Empty, 0, 1);
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
    protected void chkNoAllergies_OnCheckedChanged(object sender, EventArgs e)
    {
        ddlBrand.Enabled = !chkNoAllergies.Checked;
        ddlAllergySeverity.Enabled = !chkNoAllergies.Checked;
    }
    protected void ddlBrand_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        if (common.myStr(e.Text).Equals(string.Empty) || common.myStr(e.Text).Length < 2)
        {
            return;
        }
        int GenericId = 0;
        DataTable data = new DataTable();
        try
        {
            data = GetBrandData(common.myStr(e.Text), GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset.Equals(data.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);
                item.Attributes.Add("AllergyType", common.myStr(data.Rows[i]["AllergyType"]));
                this.ddlBrand.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
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
            data.Dispose();
        }
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetBrandData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();
        //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
        DataView DV = new DataView();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/getAllergyItemList";
            APIRootClass.AllergyItemList objRoot = new global::APIRootClass.AllergyItemList();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.ItemName = text.Replace("'", "''");
            objRoot.EncodedBy = common.myInt(Session["UserId"]);


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);

            //dsSearch = objAllergy.getAllergyItemList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            //                        text.Replace("'", "''"), common.myInt(Session["UserId"]));
            DV = dsSearch.Tables[0].Copy().DefaultView;
            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                DV.RowFilter = "AllergyType IN('Generic','CIMS','Food','Others')";
            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                DV.RowFilter = "AllergyType IN('Generic','VIDAL','Food','Others')";
            }
            else
            {
                DV.RowFilter = "AllergyType IN('Generic','Food','Others','Drug')";
            }
            dt = DV.ToTable().Copy();
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
            dsSearch.Dispose();
            //objAllergy = null;
            DV.Dispose();
        }
        return dt;
    }
    #endregion
    #region Vitals
    protected void gvVitals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.Header) || e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    string HeaderColumn = string.Empty;
                    //LinkButton lnkHT = (LinkButton)e.Row.FindControl("lnkHT");
                    //LinkButton lnkWT = (LinkButton)e.Row.FindControl("lnkWT");
                    //LinkButton lnkHC = (LinkButton)e.Row.FindControl("lnkHC");
                    //LinkButton lnkT = (LinkButton)e.Row.FindControl("lnkT");
                    //LinkButton lnkR = (LinkButton)e.Row.FindControl("lnkR");
                    //LinkButton lnkP = (LinkButton)e.Row.FindControl("lnkP");
                    //LinkButton lnkBPS = (LinkButton)e.Row.FindControl("lnkBPS");
                    //LinkButton lnkBPD = (LinkButton)e.Row.FindControl("lnkBPD");
                    //LinkButton lnkMAC = (LinkButton)e.Row.FindControl("lnkMAC");
                    //LinkButton lnkSpO2 = (LinkButton)e.Row.FindControl("lnkSpO2");
                    //LinkButton lnkBMI = (LinkButton)e.Row.FindControl("lnkBMI");
                    //LinkButton lnkBSA = (LinkButton)e.Row.FindControl("lnkBSA");
                    //for (int idx = 0; idx < e.Row.Cells.Count; idx++)
                    //{
                    //    if (!common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper().Equals("VITAL DATE")
                    //        && !common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper().Equals("ENTERED BY")
                    //        && !common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper().Equals("&NBSP;")
                    //        && !common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper().Equals(string.Empty))
                    //    {
                    //        string HeaderColumn = common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text);
                    //        e.Row.Cells[idx].Attributes.Add("onclick", "setVitalValue('" + common.myStr(e.Row.Cells[idx].Text) + "','" + HeaderColumn + "');");
                    //    }
                    //}
                }
                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnT_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnT_ABNORMAL_VALUE");
                    if (common.myInt(hdnT_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        LinkButton lnkT = (LinkButton)e.Row.FindControl("lnkT");
                        lnkT.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnR_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnR_ABNORMAL_VALUE");
                    if (common.myInt(hdnR_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        LinkButton lnkR = (LinkButton)e.Row.FindControl("lnkR");
                        lnkR.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnP_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnP_ABNORMAL_VALUE");
                    if (common.myInt(hdnP_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        LinkButton lnkP = (LinkButton)e.Row.FindControl("lnkP");
                        lnkP.ForeColor = System.Drawing.Color.Red;
                    }
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
    protected void gvVitals_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            autoSaveDataInTransit(true);
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            switch (common.myStr(e.CommandName).ToUpper())
            {
                case "HT":
                    LinkButton lnkHT = (LinkButton)row.FindControl("lnkHT");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkHT.Text) + "&Name=HT";
                    break;
                case "WT":
                    LinkButton lnkWT = (LinkButton)row.FindControl("lnkWT");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkWT.Text) + "&Name=WT";
                    break;
                case "HC":
                    LinkButton lnkHC = (LinkButton)row.FindControl("lnkHC");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkHC.Text) + "&Name=HC";
                    break;
                case "T":
                    LinkButton lnkT = (LinkButton)row.FindControl("lnkT");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkT.Text) + "&Name=T";
                    break;
                case "R":
                    LinkButton lnkR = (LinkButton)row.FindControl("lnkR");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkR.Text) + "&Name=R";
                    break;
                case "P":
                    LinkButton lnkP = (LinkButton)row.FindControl("lnkP");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkP.Text) + "&Name=P";
                    break;
                case "BPS":
                    LinkButton lnkBPS = (LinkButton)row.FindControl("lnkBPS");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkBPS.Text) + "&Name=BPS";
                    break;
                case "BPD":
                    LinkButton lnkBPD = (LinkButton)row.FindControl("lnkBPD");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkBPD.Text) + "&Name=BPD";
                    break;
                case "MAC":
                    LinkButton lnkMAC = (LinkButton)row.FindControl("lnkMAC");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkMAC.Text) + "&Name=MAC";
                    break;
                case "SPO2":
                    LinkButton lnkSpO2 = (LinkButton)row.FindControl("lnkSpO2");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkSpO2.Text) + "&Name=SpO2";
                    break;
                case "BMI":
                    LinkButton lnkBMI = (LinkButton)row.FindControl("lnkBMI");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkBMI.Text) + "&Name=BMI";
                    break;
                case "BSA":
                    LinkButton lnkBSA = (LinkButton)row.FindControl("lnkBSA");
                    RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=" + common.myStr(lnkBSA.Text) + "&Name=BSA";
                    break;
            }
            RadWindowForNew.Width = 1000;
            RadWindowForNew.Height = 620;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "addVitalsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            //RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void gvVitals_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVitals.PageIndex = e.NewPageIndex;
        bindVitals();
    }
    public void bindVitals()
    {
        BindCommonData("Vitals", "S", string.Empty, 0, 1);
    }
    protected void lnkAddVitals_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Vitals/Vitals.aspx?IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addVitalsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddVitalsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindVitals();
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
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        //BaseC.EMRVitals objVital = new BaseC.EMRVitals(sConString);
        DataSet ds = new DataSet();
        StringBuilder objStr = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            if (common.myLen(txtHeight.Text) > 0)
            {
                coll.Add(common.myInt(hdnHeight.Value));
                coll.Add(common.myInt(5));
                coll.Add(common.myInt(txtHeight.Text));
                coll.Add(common.myInt(0));
                coll.Add(common.myInt(0));
                objStr.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(TxtWeight.Text) > 0)
            {
                coll.Add(common.myInt(hdnWeight.Value));
                coll.Add(common.myInt(1));
                coll.Add(common.myInt(TxtWeight.Text));
                coll.Add(common.myInt(0));
                coll.Add(common.myInt(0));
                objStr.Append(common.setXmlTable(ref coll));
            }
            if (!objStr.ToString().Equals(string.Empty))
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CalculateVitalsValue";
                APIRootClass.CalculateVitalsValue objRoot = new global::APIRootClass.CalculateVitalsValue();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.xmlString = objStr.ToString();


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                //ds = objVital.CalculateVitalsValue(common.myInt(Session["HospitalLocationID"]), objStr.ToString());
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (common.myStr(ds.Tables[0].Rows[i]["DisplayName"]).Equals("BMI")
                            && !common.myStr(ds.Tables[0].Rows[i]["Value"]).Equals(string.Empty))
                        {
                            txtBMI.Text = common.myStr(ds.Tables[0].Rows[i]["Value"]);
                            hdnBMIValue.Value = common.myStr(ds.Tables[0].Rows[i]["Value"]);
                        }
                        if (common.myStr(ds.Tables[0].Rows[i]["DisplayName"]).Equals("BSA")
                            && !common.myStr(ds.Tables[0].Rows[i]["Value"]).Equals(string.Empty))
                        {
                            txtBSA.Text = common.myStr(ds.Tables[0].Rows[i]["Value"]);
                            hdnBSAValue.Value = common.myStr(ds.Tables[0].Rows[i]["Value"]);
                        }
                    }
                }
                if (common.myStr(TxtWeight.Text).Equals(string.Empty))
                {
                    TxtWeight.Focus();
                }
                else
                {
                    txtHC.Focus();
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
            //objVital = null;
            ds.Dispose();
            objStr = null;
            coll = null;
        }
    }
    #endregion
    #region History
    protected void lnkAddTemplatesHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3145";
            // RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3025";
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3145&SingleScreenTemplateCode=HIS";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addHistoryOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void gvHistory_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            try
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorHistory = (TextBox)e.Row.FindControl("editorHistory");
                editorHistory.Text = common.myStr(hdnTemplateName.Value);
                editorHistory.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorHistory.Text = common.clearHTMLTags(editorHistory.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumHistory.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorHistory, 125);
            }
            catch
            {
            }
        }
    }
    protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHistory.PageIndex = e.NewPageIndex;
        BindHistoryData("HIS");
    }
    public void BindHistoryData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void lnkViewHistory_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
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
    }
    protected void lnkEditHistory_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWHistory.Text = common.myStr(hdnTemplateName.Value);
                hdnHistoryRecordId.Value = common.myStr(hdnRecordId.Value);
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
    protected void btnBindhistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindHistoryData("HIS");
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
    protected void gvHistory_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvHistory.EditIndex = -1;
        BindHistoryData("HIS");
    }
    protected void gvHistory_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvHistory.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvHistory.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorHistory = (TextBox)gvHistory.Rows[e.RowIndex].FindControl("editorHistory");
            if (common.myLen(editorHistory.Text) > 0)
            {
                string strHistory = common.myStr(editorHistory.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79244);
                coll.Add("W");
                coll.Add(strHistory);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17552);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblHistoryMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblHistoryMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvHistory.EditIndex = -1;
            BindHistoryData("HIS");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvHistory_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
        {
            Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
            return;
        }
        else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
        {
            return;
        }
        else
        {
            GridViewRow row = gvHistory.Rows[e.NewEditIndex];
            HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
            if (common.myInt(hdnEncodedById.Value) > 0)
            {
                gvHistory.EditIndex = e.NewEditIndex;
                BindHistoryData("HIS");
            }
        }
    }
    #endregion
    #region Previous Treatment
    protected void btnPreviousTreatmentClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindOtherNotes("ALL");
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
    protected void lnkPreviousTreatment_OnClick(object sender, EventArgs e)//Previous Treatment
    {
        try
        {
            ClearMessageControl();
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3146";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addPreviousTreatmentOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void gvPrevTreatment_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            try
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorPrevTreatment = (TextBox)e.Row.FindControl("editorPrevTreatment");
                editorPrevTreatment.Text = common.myStr(hdnTemplateName.Value);
                editorPrevTreatment.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorPrevTreatment.Text = common.clearHTMLTags(editorPrevTreatment.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumPrevTreatment.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorPrevTreatment, 125);
            }
            catch
            {
            }
        }
    }
    protected void gvPrevTreatment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrevTreatment.PageIndex = e.NewPageIndex;
        BindPreviousTreatmentData("PT");
    }
    public void BindPreviousTreatmentData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void lnkViewPrevTreatment_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
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
    }
    protected void lnkEditPrevTreatment_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWPrevTreatment.Text = common.myStr(hdnTemplateName.Value);
                hdnPreviousTreatmentRecordId.Value = common.myStr(hdnRecordId.Value);
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
    protected void gvPrevTreatment_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvPrevTreatment.EditIndex = -1;
        BindPreviousTreatmentData("PT");
    }
    protected void gvPrevTreatment_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvPrevTreatment.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvPrevTreatment.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorPrevTreatment = (TextBox)gvPrevTreatment.Rows[e.RowIndex].FindControl("editorPrevTreatment");
            if (common.myLen(editorPrevTreatment.Text) > 0)
            {
                string strPreTre = common.myStr(editorPrevTreatment.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79246);
                coll.Add("W");
                coll.Add(strPreTre);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17554);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblPrevTreatmentMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblPrevTreatmentMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvPrevTreatment.EditIndex = -1;
            BindPreviousTreatmentData("PT");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvPrevTreatment_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
        {
            Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
            return;
        }
        else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
        {
            return;
        }
        else
        {
            GridViewRow row = gvPrevTreatment.Rows[e.NewEditIndex];
            HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
            if (common.myInt(hdnEncodedById.Value) > 0)
            {
                gvPrevTreatment.EditIndex = e.NewEditIndex;
                BindPreviousTreatmentData("PT");
            }
        }
    }
    #endregion
    #region Examination
    protected void gvExamination_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExamination.PageIndex = e.NewPageIndex;
        BindExaminationData("EXM");
    }
    protected void lnkViewExamination_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
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
    }
    protected void lnkEditExamination_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWExamination.Text = common.myStr(hdnTemplateName.Value);
                hdnExaminationRecordId.Value = common.myStr(hdnRecordId.Value);
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
    public void BindExaminationData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void gvExamination_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorExamination = (TextBox)e.Row.FindControl("editorExamination");
                editorExamination.Text = common.myStr(hdnTemplateName.Value);
                editorExamination.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorExamination.Text = common.clearHTMLTags(editorExamination.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumExamination.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorExamination, 125);
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
    protected void lnkAddTemplates_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3140&SingleScreenTemplateCode=EXM";
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3140&SingleScreenTemplateCode=EXM";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddTemplatesClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindExaminationData("EXM");
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
    protected void gvExamination_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvExamination.EditIndex = -1;
        BindExaminationData("EXM");
    }
    protected void gvExamination_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvExamination.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvExamination.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorExamination = (TextBox)gvExamination.Rows[e.RowIndex].FindControl("editorExamination");
            if (common.myLen(editorExamination.Text) > 0)
            {
                string strExamination = common.myStr(editorExamination.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79239);
                coll.Add("W");
                coll.Add(strExamination);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17546);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblExamMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblExamMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvExamination.EditIndex = -1;
            BindExaminationData("EXM");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvExamination_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvExamination.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvExamination.EditIndex = e.NewEditIndex;
                    BindExaminationData("EXM");
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
    #region Nutritional Status
    protected void lnkNutritionalStatus_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3147";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addNutritionalStatusOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnNutritionalStatusClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindNutritionalData("NS");
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
    public void BindNutritionalData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void gvNutritional_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNutritional.PageIndex = e.NewPageIndex;
        BindNutritionalData("NS");
    }
    protected void lnkViewNutritional_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
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
    }
    protected void lnkEditNutritional_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWNutritionalStatus.Text = common.myStr(hdnTemplateName.Value);
                hdnNutritionalStatusRecordId.Value = common.myStr(hdnRecordId.Value);
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
    protected void gvNutritional_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorNutritional = (TextBox)e.Row.FindControl("editorNutritional");
                editorNutritional.Text = common.myStr(hdnTemplateName.Value);
                editorNutritional.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorNutritional.Text = common.clearHTMLTags(editorNutritional.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumNutritional.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorNutritional, 125);
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
    protected void gvNutritional_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvNutritional.EditIndex = -1;
        BindNutritionalData("NS");
    }
    protected void gvNutritional_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvNutritional.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvNutritional.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorNutritional = (TextBox)gvNutritional.Rows[e.RowIndex].FindControl("editorNutritional");
            if (common.myLen(editorNutritional.Text) > 0)
            {
                string strNutritional = common.myStr(editorNutritional.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79247);
                coll.Add("W");
                coll.Add(strNutritional);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17555);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblNutritionalStatusMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblNutritionalStatusMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvNutritional.EditIndex = -1;
            BindNutritionalData("NS");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvNutritional_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvNutritional.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvNutritional.EditIndex = e.NewEditIndex;
                    BindNutritionalData("NS");
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
    #region Plan Of Care
    protected void gvPlanOfCare_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPlanOfCare.PageIndex = e.NewPageIndex;
        BindPlanOfCareData("POC");
    }
    protected void lnkViewPlanOfCare_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                try
                {
                    RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                    RadWindowForNew.Width = 1200;
                    RadWindowForNew.Height = 630;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
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
    protected void lnkEditPlanOfCare_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWPlanOfCare.Text = common.myStr(hdnTemplateName.Value);
                hdnPlanOfCareRecordId.Value = common.myStr(hdnRecordId.Value);
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
    protected void lnkPlanOfCare_OnClick(object sender, EventArgs e)//Previous Treatment
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3024&SingleScreenTemplateCode=POC";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addPlanOfCareOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnPlanOfCareClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindPlanOfCareData("POC");
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
    public void BindPlanOfCareData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void gvPlanOfCare_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorPlanOfCare = (TextBox)e.Row.FindControl("editorPlanOfCare");
                editorPlanOfCare.Text = common.myStr(hdnTemplateName.Value);
                editorPlanOfCare.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorPlanOfCare.Text = common.clearHTMLTags(editorPlanOfCare.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumPlanOfCare.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorPlanOfCare, 125);
            }
        }
        catch
        {
        }
    }
    protected void gvPlanOfCare_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvPlanOfCare.EditIndex = -1;
        BindPlanOfCareData("POC");
    }
    protected void gvPlanOfCare_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvPlanOfCare.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvPlanOfCare.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorPlanOfCare = (TextBox)gvPlanOfCare.Rows[e.RowIndex].FindControl("editorPlanOfCare");
            if (common.myLen(editorPlanOfCare.Text) > 0)
            {
                string strPlanOfCare = common.myStr(editorPlanOfCare.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(73518);
                coll.Add("W");
                coll.Add(strPlanOfCare);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17078);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }

            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblPlanOfCareMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblPlanOfCareMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvPlanOfCare.EditIndex = -1;
            BindPlanOfCareData("POC");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
        }
    }
    protected void gvPlanOfCare_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvPlanOfCare.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvPlanOfCare.EditIndex = e.NewEditIndex;
                    BindPlanOfCareData("POC");
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    #region Cost Analysis
    protected void lnkCostAnalysis_OnClick(object sender, EventArgs e)//Previous Treatment
    {
        try
        {
            ClearMessageControl();
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3142";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addCostAnalysisOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnCostAnalysisClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindCostAnalysisData("CA");
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
    public void BindCostAnalysisData(string TemplateType)
    {
        BindCommonData(string.Empty, "D", TemplateType, 0, 1);
    }
    protected void lnkViewCostAnalysis_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
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
    }
    protected void lnkEditCostAnalysis_OnClik(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
            HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            HiddenField hdnRecordId = (HiddenField)row.FindControl("hdnRecordId");
            if (!common.myStr(TemplateId).Equals(string.Empty))
            {
                txtWCostAnalysis.Text = common.myStr(hdnTemplateName.Value);
                hdnCostAnalysisRecordId.Value = common.myStr(hdnRecordId.Value);
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
    protected void gvCostAnalysis_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorCostAnalysis = (TextBox)e.Row.FindControl("editorCostAnalysis");
                editorCostAnalysis.Text = common.myStr(hdnTemplateName.Value);
                editorCostAnalysis.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorCostAnalysis.Text = common.clearHTMLTags(editorCostAnalysis.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumCostAnalysis.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorCostAnalysis, 125);
            }
        }
        catch
        {
        }
    }
    protected void gvCostAnalysis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCostAnalysis.PageIndex = e.NewPageIndex;
        BindCostAnalysisData("CA");
    }
    protected void gvCostAnalysis_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvCostAnalysis.EditIndex = -1;
        BindCostAnalysisData("CA");
    }
    protected void gvCostAnalysis_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //Hashtable HshOut = new Hashtable();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvCostAnalysis.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvCostAnalysis.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorCostAnalysis = (TextBox)gvCostAnalysis.Rows[e.RowIndex].FindControl("editorCostAnalysis");
            if (common.myLen(editorCostAnalysis.Text) > 0)
            {
                string strCostAnalysis = common.myStr(editorCostAnalysis.Text).Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79241);
                coll.Add("W");
                coll.Add(strCostAnalysis);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17549);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }

            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblCostAnalysisMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblCostAnalysisMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvCostAnalysis.EditIndex = -1;
            BindCostAnalysisData("CA");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
        }
    }
    protected void gvCostAnalysis_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvCostAnalysis.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvCostAnalysis.EditIndex = e.NewEditIndex;
                    BindCostAnalysisData("CA");
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
    #region Other Notes
    protected void gvOtherNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOtherNotes.PageIndex = e.NewPageIndex;
        BindOtherNotes("ALL");
    }
    public void BindOtherNotes(string sTemplateType)
    {
        BindCommonData(string.Empty, "D", sTemplateType, 0, 1);
    }
    protected void gvOtherNotes_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal)
                || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
            {
                LinkButton lnlEdit = (LinkButton)e.Row.FindControl("lnlEdit");
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    lnlEdit.Visible = false;
                }
                if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                {
                    lnlEdit.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void lnkView_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
        ClearMessageControl();
        autoSaveDataInTransit(true);
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        HiddenField hdnTemplateType = (HiddenField)row.FindControl("hdnTemplateType");
        Label lblTemplateName = (Label)row.FindControl("lblTemplateName");
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                if (common.myStr(hdnTemplateType.Value).Equals("P"))
                {
                    if (common.myStr(lblTemplateName.Text).Equals("Doctor Progress Note"))
                    {
                        RadWindowForNew.NavigateUrl = "~/EMR/Templates/DoctorProgressNote.aspx?MP=NO";
                    }
                    else if (common.myStr(lblTemplateName.Text).Equals("Diet Order"))
                    {
                        RadWindowForNew.NavigateUrl = "~/Diet/EMRPatientDietRequisition.aspx?Regid=" + common.myInt(Session["RegistrationID"])
                             + "&RegNo=" + common.myStr(Session["RegistrationNo"])
                             + "&EncId=" + common.myInt(Session["Encounterid"])
                             + "&EncNo=" + common.myStr(Session["EncounterNo"])
                             + "&Ward=Ward";
                    }
                }
                else
                {
                    RadWindowForNew.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId);
                }
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    }
    protected void lnlEdit_OnClik(object sender, EventArgs e)
    {
        if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
        {
            Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
            return;
        }
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
        {
            return;
        }
        ClearMessageControl();
        autoSaveDataInTransit(true);
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        string TemplateId = ((HiddenField)row.FindControl("hdnTemplateID")).Value;
        HiddenField hdnTemplateType = (HiddenField)row.FindControl("hdnTemplateType");
        Label lblTemplateName = (Label)row.FindControl("lblTemplateName");
        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                if (common.myStr(hdnTemplateType.Value).Equals("P"))
                {
                    if (common.myStr(lblTemplateName.Text).Equals("Doctor Progress Note"))
                    {
                        RadWindowForNew.NavigateUrl = "~/EMR/Templates/DoctorProgressNote.aspx?MP=NO";
                    }
                    else if (common.myStr(lblTemplateName.Text).Equals("Diet Order"))
                    {
                        RadWindowForNew.NavigateUrl = "~/Diet/EMRPatientDietRequisition.aspx?Ward=Ward";
                    }
                }
                else
                {
                    RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId) + "&SingleScreenTemplateCode=OTH";
                }
                RadWindowForNew.Width = 1200;
                RadWindowForNew.Height = 630;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    }
    protected void lnkAddTemplates_All_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/TemplateNotes.aspx?From=POPUP&SingleScreenTemplateCode=OTH";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddTemplatesClose_All_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindOtherNotes("ALL");
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
    #region Provisional Diagnosis
    protected void BindDiagnosisSearchCode()
    {
        //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        DataTable dt = new DataTable();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProvisionalDiagnosisSearchCodes";
            APIRootClass.GetProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.GetProvisionalDiagnosisSearchCodes();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.KeywordType = string.Empty;


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            //dt = JsonConvert.DeserializeObject<DataTable>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            //ds = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), string.Empty);
            ddlDiagnosisSearchCodes.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDiagnosisSearchCodes.DataSource = ds.Tables[0];
                ddlDiagnosisSearchCodes.DataValueField = "Id";
                ddlDiagnosisSearchCodes.DataTextField = "DiagnosisSearchCode";
                ddlDiagnosisSearchCodes.DataBind();
            }
            ddlDiagnosisSearchCodes.Items.Insert(0, new ListItem("Select", string.Empty));
            //ddlDiagnosisSearchCodes.Items.Insert(0, new ListItem("kuldeep", string.Empty));
            ddlDiagnosisSearchCodes.SelectedIndex = 0;
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
            //objDiag = null;
            dt.Dispose();
        }
    }
    protected void imgBtnProvisionalDiagnosis_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/ProvisionalDiagnosis.aspx?Diag=Tx&IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addProvisionalDiagnosisClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void imgBtnFinalDiagnosis_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Diagnosis.aspx?Diag=Tx&IsEMRPopUp=1&From=POPUP";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addFinalDiagnosisClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnFinalDiagnosisClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            BindPatientProvisionalDiagnosis();
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
    protected void btnProvisionalDiagnosisClose_OnClick(object sender, EventArgs e)
    {
        ClearMessageControl();
        try
        {
            BindPatientProvisionalDiagnosis();
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
    public void BindPatientProvisionalDiagnosis()
    {
        BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
    }
    protected void btnAddDiag_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Status.aspx?Source=pd";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addDiagnosisSerchOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
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
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        BindPatientProvisionalDiagnosis();
    }
    protected void gvData_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        //DataSet ds = new DataSet();
        //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            lblMessage.Text = string.Empty;
            if (!common.myBool(ViewState["DeleteEnable"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to delete", Page);
                return;
            }
            if (e.CommandName.Equals("Del"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnProvisionalDiagnosisID = (HiddenField)row.FindControl("hdnProvisionalDiagnosisID");
                ViewState["_ID"] = common.myInt(hdnProvisionalDiagnosisID.Value);
                if (common.myInt(ViewState["_ID"]) > 0)
                {
                    dvConfirmCancelOptionsProvisionalDiag.Visible = true;
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
            //ds.Dispose();
            //objDiag = null;
        }
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                HiddenField hdnProvisionalDiagnosis = (HiddenField)e.Row.FindControl("hdnProvisionalDiagnosis");
                TextBox editorProvisionalDiagnosis = (TextBox)e.Row.FindControl("editorProvisionalDiagnosis");
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    ibtnDelete.Visible = false;
                }
                editorProvisionalDiagnosis.Text = common.myStr(hdnProvisionalDiagnosis.Value);
                editorProvisionalDiagnosis.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorProvisionalDiagnosis.Text = common.clearHTMLTags(editorProvisionalDiagnosis.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumProvisionalDiagnosis.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                        ibtnDelete.Visible = false;
                    }
                }
                setControlHeight(editorProvisionalDiagnosis, 80);
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
    protected void ButtonOkProvisionalDiag_OnClick(object sender, EventArgs e)
    {
        //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            if (common.myInt(ViewState["_ID"]) > 0)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeletePatientProvisionalDiagnosis";
                APIRootClass.DeletePatientProvisionalDiagnosis objRoot = new global::APIRootClass.DeletePatientProvisionalDiagnosis();
                objRoot.ProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
                objRoot.UserId = common.myInt(Session["UserID"]);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                int i = JsonConvert.DeserializeObject<int>(sValue);

                //int i = objDiag.DeletePatientProvisionalDiagnosis(common.myInt(ViewState["_ID"]), common.myInt(Session["UserID"]));
                if (i.Equals(0))
                {
                    lblProvDiag.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblProvDiag.Text = "Provisional Diagnosis deleted.";
                }
                BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
            }
            else
            {
                lblProvDiag.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblProvDiag.Text = "Select a provisional diagnosis";
            }
            dvConfirmCancelOptionsProvisionalDiag.Visible = false;
            BindPatientProvisionalDiagnosis();
            ViewState["_ID"] = 0;
            editorProvDiagnosis.Text = string.Empty;
            ddlDiagnosisSearchCodes.SelectedIndex = 0;
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
            //objDiag = null;
        }
    }
    protected void ButtonCancelProvisionalDiag_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptionsProvisionalDiag.Visible = false;
    }
    protected void btnAddDiagnosisSerchOnClientClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindDiagnosisSearchCode();
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
    protected void gvData_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvData.EditIndex = -1;
        BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
    }
    protected void gvData_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        //ArrayList coll = new ArrayList();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            HiddenField hdnProvisionalDiagnosisID = (HiddenField)gvData.Rows[e.RowIndex].FindControl("hdnProvisionalDiagnosisID");
            HiddenField hdnDiagnosisSearchId = (HiddenField)gvData.Rows[e.RowIndex].FindControl("hdnDiagnosisSearchId");
            TextBox editorProvisionalDiagnosis = (TextBox)gvData.Rows[e.RowIndex].FindControl("editorProvisionalDiagnosis");
            ViewState["_ID"] = common.myInt(hdnProvisionalDiagnosisID.Value);
            #region provisionalDiagnosis
            int intProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
            string ProvisionalDiagnosis = common.myStr(editorProvisionalDiagnosis.Text).Replace("\n", "<br/>");
            int DiagnosisSearchId = common.myInt(hdnDiagnosisSearchId.Value);
            #endregion

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
            APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.DoctorId = common.myInt(Session["DoctorID"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.xmlProblemDetails = string.Empty;
            objRoot.sProvisionalDiagnosis = ProvisionalDiagnosis;
            objRoot.xmlVitalString = string.Empty;
            objRoot.strXMLDrug = string.Empty;
            objRoot.strXMLOther = string.Empty;
            objRoot.ProvisionalDiagnosisId = intProvisionalDiagnosisId;
            objRoot.DiagnosisSearchId = DiagnosisSearchId;
            objRoot.bitNKDA = 0;
            objRoot.xmlTemplateDetails = string.Empty;
            objRoot.iSign = 0;
            objRoot.xmlNonDrugOrder = string.Empty;


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            //Hashtable HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
            //                       common.myInt(Session["UserId"]), string.Empty, ProvisionalDiagnosis, string.Empty, string.Empty, string.Empty, intProvisionalDiagnosisId, DiagnosisSearchId, 0, string.Empty, 0, string.Empty);
            gvData.EditIndex = -1;
            BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
            if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = sValue;
                lblProvDiag.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblProvDiag.Text = sValue;
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
            //coll = null;
            //objEMR = null;
            ViewState["_ID"] = null;
        }
    }
    protected void gvData_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvData.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvData.EditIndex = e.NewEditIndex;
                    BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    #region Order And Procedure
    protected void gvOrdersAndProcedures_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrdersAndProcedures.PageIndex = e.NewPageIndex;
        bindOrdersAndProcedures();
    }
    public void bindOrdersAndProcedures()
    {
        BindCommonData("Orders And Procedures", "S", string.Empty, 0, 1);
    }
    protected void lnkAddOrdersAndProcedures_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ViewState["CheifComplaintFound"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Please save Chief Complaint first", this.Page);
                return;
            }
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "/EMR/Orders/Orders.aspx?IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addOrdersAndProceduresOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddOrdersAndProceduresClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindOrdersAndProcedures();
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
    #region Prescription
    protected void gvPrescriptions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
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
    protected void gvPrescriptions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrescriptions.PageIndex = e.NewPageIndex;
        bindPrescriptions();
    }
    public void bindPrescriptions()
    {
        BindCommonData("Prescription", "S", string.Empty, 0, 1);
    }
    protected void lnkAddPrescriptions_OnClick(object sender, EventArgs e)
    {
        try
        {
            autoSaveDataInTransit(true);
            if (common.myInt(ViewState["CheifComplaintFound"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Please save Chief Complaint first", this.Page);
                return;
            }
            RadWindowForNew.NavigateUrl = "/EMR/Medication/PrescribeMedicationNew.aspx?IsEMRPopUp=1&Regid=" + common.myInt(Session["RegistrationID"])
                                    + "&RegNo=" + common.myInt(Session["RegistrationNo"])
                                    + "&EncId=" + common.myInt(Session["EncounterId"])
                                    + "&EncNo=" + common.myInt(Session["EncounterNo"]);
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addPrescriptionsOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnAddPrescriptionsClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindPrescriptions();
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
    #region Non Drug Order
    protected void gvNonDrugOrder_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Select"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblPrescription = (Label)row.FindControl("lblPrescription");
                HiddenField NonDrugOrderId = (HiddenField)row.FindControl("hdnNonDrugOrderId");
                HiddenField hdnOrderType = (HiddenField)row.FindControl("hdnOrderType");
                HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                editorNonDrugOrder.Text = common.myStr(lblPrescription.Text);
                ddlOrderType.SelectedIndex = ddlOrderType.Items.IndexOf(ddlOrderType.Items.FindByValue(common.myStr(hdnOrderType.Value)));
                ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindByValue(common.myStr(hdnDoctorId.Value)));
                hdnNonDrugOrderId.Value = common.myStr(NonDrugOrderId.Value);
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
    protected void gvNonDrugOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnAcknowledgeBy = (HiddenField)e.Row.FindControl("hdnAcknowledgeBy");
                HiddenField hdnPrescription = (HiddenField)e.Row.FindControl("hdnPrescription");
                TextBox edNonDrugOrder = (TextBox)e.Row.FindControl("edNonDrugOrder");
                if (common.myBool(hdnAcknowledgeBy.Value))
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }
                edNonDrugOrder.Text = common.myStr(hdnPrescription.Value);
                edNonDrugOrder.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                edNonDrugOrder.Text = common.clearHTMLTags(edNonDrugOrder.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumNonDrugOrder.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(edNonDrugOrder, 80);
            }
        }
        catch
        {
        }
    }
    protected void imgNonDrugOrder_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "/ICM/ICMNONDrugOrder.aspx?POPUP=POPUP&IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addNonDrugOrderOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnNonDrugOrder_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindCommonData("Non Drug Order", "S", string.Empty, 0, 1);
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
    private void BindDoctor()
    {
        //BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetICMSignDoctors";
            APIRootClass.GetICMSignDoctors objRoot = new global::APIRootClass.GetICMSignDoctors();
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            ddlDoctor.Items.Clear();
            //ds = objICM.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlDoctor.DataSource = ds.Tables[0];
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataValueField = "ID";
                ddlDoctor.DataBind();
            }
            ddlDoctor.Items.Insert(0, new ListItem("Select", string.Empty));
            ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindByValue(common.myStr(Session["EmployeeId"])));
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ds.Dispose();
            //objICM = null;
        }
    }
    protected void gvNonDrugOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonDrugOrder.PageIndex = e.NewPageIndex;
        BindCommonData("Non Drug Order", "S", string.Empty, 0, 1);
    }
    protected void gvNonDrugOrder_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvNonDrugOrder.EditIndex = -1;
        BindCommonData("Non Drug Order", "S", string.Empty, 0, 1);
    }
    protected void gvNonDrugOrder_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder objXMLNonDrugOrder = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnNonDrugOrderId = (HiddenField)gvNonDrugOrder.Rows[e.RowIndex].FindControl("hdnNonDrugOrderId");
            HiddenField hdnOrderType = (HiddenField)gvNonDrugOrder.Rows[e.RowIndex].FindControl("hdnOrderType");
            TextBox edNonDrugOrder = (TextBox)gvNonDrugOrder.Rows[e.RowIndex].FindControl("edNonDrugOrder");
            if (common.myLen(edNonDrugOrder.Text) > 0)
            {
                string strNonDrugOrder = common.myStr(edNonDrugOrder.Text).Replace("\n", "<br/>");
                coll.Add(hdnNonDrugOrderId.Value);
                coll.Add(strNonDrugOrder);
                coll.Add(hdnOrderType.Value);
                coll.Add(ddlDoctor.SelectedValue);
                objXMLNonDrugOrder.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(objXMLNonDrugOrder.ToString()) >= 1)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = string.Empty;
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = objXMLNonDrugOrder.ToString();


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblNonDrugOrder.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblNonDrugOrder.Text = sValue;
                    gvNonDrugOrder.EditIndex = -1;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, string.Empty, 0, objXMLNonDrugOrder.ToString());
            }
            gvNonDrugOrder.EditIndex = -1;
            BindCommonData("Non Drug Order", "S", string.Empty, 0, 1);

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
            coll = null;
            objXMLNonDrugOrder = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvNonDrugOrder_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                GridViewRow row = gvNonDrugOrder.Rows[e.NewEditIndex];
                HiddenField hdnEncodedById = (HiddenField)row.Cells[0].FindControl("hdnEncodedById");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    gvNonDrugOrder.EditIndex = e.NewEditIndex;
                    BindCommonData("Non Drug Order", "S", string.Empty, 0, 1);
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    #region Other Events
    protected void lnkImmunisationHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=6039&SingleScreenTemplateCode=IH";
            RadWindowForNew.NavigateUrl = "/EMR/Immunization/ImmunizationBabyDueDate.aspx";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void lnkEducationCounseling_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=5998&SingleScreenTemplateCode=EC";
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3101&SingleScreenTemplateCode=NT";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void lnkReferrals_OnClick(object sender, EventArgs e)
    {
        try
        {
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=B&MASTER=NO&RegNo=" + common.myStr(Session["RegistrationNo"])
                + "&EId=" + common.myInt(Session["EncounterId"]) + "&IsEMRPopUp=1";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void lnkAnaesthesiaCritical_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateGroupId=1113&TemplateId=0";
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=0&TemplateGroupId=1406";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void lnkMultidisciplinaryEvaluation_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=6019&SingleScreenTemplateCode=EP";
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3119&SingleScreenTemplateCode=EP";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnSaveDashboard_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ValidatTemplate())
            {
                SaveDasboardData(0);//Not
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void btnSaveAsSigned_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ValidatTemplate())
            {
                SaveDasboardData(1);//Signed
                Session["EncounterStatus"] = "CLOSE";
                btnSaveSign.Visible = false;
                btnSaveAsSigned.Visible = false;
                btnSave.Visible = false;
                btnSaveDashboard.Visible = false;
                if ((common.myBool(Session["isEMRSuperUser"]) || common.myBool(ViewState["SaveEnable"]))
                            && common.myStr(Session["OPIP"]).Equals("O"))
                {
                    btnDefinalise.Visible = true;
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
    private void SaveDasboardData(int iSign)
    {
        //BaseC.ParseData objParse = new BaseC.ParseData();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        string strMsg = string.Empty;
        //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
        DataSet dsAllergy = new DataSet();
        StringBuilder objXMLVital = new StringBuilder();
        StringBuilder strXMLDrug = new StringBuilder();
        StringBuilder strXMLOther = new StringBuilder();
        StringBuilder objXMLProblem = new StringBuilder();
        StringBuilder objXMLNonDrugOrder = new StringBuilder();
        StringBuilder strNonTabularH = new StringBuilder();
        StringBuilder strNonTabularPT = new StringBuilder();
        StringBuilder strNonTabularE = new StringBuilder();
        StringBuilder strNonTabularN = new StringBuilder();
        StringBuilder strNonTabularP = new StringBuilder();
        StringBuilder strNonTabularC = new StringBuilder();
        StringBuilder xmlTemplateDetails = new StringBuilder();
        StringBuilder strNonTabularPH = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            if (!common.myBool(ViewState["SaveEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Save", Page);
                return;
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE")
                && !common.myBool(Session["isEMRSuperUser"]))
            {
                if (!common.myBool(ViewState["SaveEnable"]))
                {
                    Alert.ShowAjaxMsg("You are not authorized to Save", Page);
                    return;
                }
            }
            ClearMessageControl();
            #region  Problem
            coll = new ArrayList();
            if (!editorChiefComplaints.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                foreach (GridViewRow gv in gvProblemDetails.Rows)
                {
                    if (common.myStr(editorChiefComplaints.Text).Trim().Equals(common.myStr(((HiddenField)gv.FindControl("hdnProblem")).Value)))
                    {
                        if (!common.myStr(txtedit.Text).Trim().Equals(common.myStr(((HiddenField)gv.FindControl("hdnProblemId")).Value)))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "This  (" + common.myStr(editorChiefComplaints.Text).Trim() + ")  already exists in Today's Problems!";
                            return;
                        }
                    }
                }
                string editID;
                if (!(txtedit.Text.Trim().Equals(string.Empty)))
                {
                    editID = txtedit.Text;
                }
                else
                {
                    editID = string.Empty;
                }
                string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate") ? common.myStr(Request.QueryString["TemplateFieldId"]) : "0";
                //string strProblem = objParse.ParseQ(common.myStr(editorChiefComplaints.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                string strProblem = editorChiefComplaints.Text.Trim();
                if (common.myLen(strProblem) > 2000)
                {
                    Alert.ShowAjaxMsg("Chief complaints (free text) length must be less than 2000 character!", this.Page);
                    return;
                }
                strProblem = strProblem.Replace("\r\n", "<br/>");
                coll.Add(editID);//Id
                coll.Add(0);//ProblemId
                coll.Add(strProblem);//Problem
                coll.Add(0);//DurationID
                coll.Add(string.Empty);//Duration
                coll.Add(0);//ContextID
                coll.Add(string.Empty);//Context
                coll.Add(0);//SeverityId
                coll.Add(string.Empty);//Severity
                coll.Add(0);//IsPrimary
                coll.Add(0);//IsChronic
                coll.Add(common.myStr(Session["DoctorID"]));//DoctorId
                coll.Add(common.myStr(Session["FacilityId"]));//FacilityId
                coll.Add(0);//SCTId
                coll.Add(string.Empty);//QualityIDs
                coll.Add(0);//LocationID
                coll.Add(string.Empty);//Location
                coll.Add(0);//OnsetID
                coll.Add(0);//AssociatedProblemId1
                coll.Add(string.Empty);//AssociatedProblem1
                coll.Add(0);//AssociatedProblemId2
                coll.Add(string.Empty);//AssociatedProblem2
                coll.Add(0);//AssociatedProblemId3
                coll.Add(string.Empty);//AssociatedProblem3
                coll.Add(0);//AssociatedProblemId4
                coll.Add(string.Empty);//AssociatedProblem4
                coll.Add(0);//AssociatedProblemId5
                coll.Add(string.Empty);//AssociatedProblem5
                coll.Add(string.Empty);//Side
                coll.Add(0);//ConditionId
                coll.Add(0);//Percentage
                coll.Add(0);//Durations
                coll.Add(string.Empty);//DurationType
                coll.Add(TemplateId);//TemplateFieldId
                coll.Add(0);//ComplaintSearchId
                objXMLProblem.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region provisionalDiagnosis
            coll = new ArrayList();
            if (common.myInt(ddlDiagnosisSearchCodes.SelectedValue).Equals(0)
                && !common.myStr(editorProvDiagnosis.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select search keyword for provisional diagnosis", Page);
                return;
            }
            int intProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
            string ProvisionalDiagnosis = common.myStr(editorProvDiagnosis.Text).Replace("\n", "<br/>"); ;
            int DiagnosisSearchId = common.myInt(ddlDiagnosisSearchCodes.SelectedValue);
            #endregion
            #region  Vital
            coll = new ArrayList();
            if (common.myLen(txtHeight.Text) > 0)
            {
                coll.Add(common.myStr(hdnHeight.Value));//VitalId smallint,              
                coll.Add(common.myStr(txtHeight.Text).Trim());//EnteredVitalValue1 varchar(50),
                coll.Add(5);//EnteredUnitId1 smallint,
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(TxtWeight.Text) > 0)
            {
                coll.Add(common.myStr(hdnWeight.Value));
                coll.Add(common.myStr(TxtWeight.Text).Trim());
                coll.Add(1);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtHC.Text) > 0)
            {
                coll.Add(common.myStr(hdnHC.Value));
                coll.Add(common.myStr(txtHC.Text).Trim());
                coll.Add(5);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(TxtTemperature.Text) > 0)
            {
                coll.Add(common.myStr(hdnTemperature.Value));
                coll.Add(common.myStr(TxtTemperature.Text).Trim());
                coll.Add(9);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtRespiration.Text) > 0)
            {
                coll.Add(common.myStr(hdnRespiration.Value));
                coll.Add(common.myStr(txtRespiration.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtPulse.Text) > 0)
            {
                coll.Add(common.myStr(hdnPulse.Value));
                coll.Add(common.myStr(txtPulse.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtBPSystolic.Text) > 0)
            {
                coll.Add(common.myStr(hdnBPSystolic.Value));
                coll.Add(common.myStr(txtBPSystolic.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtBPDiastolic.Text) > 0)
            {
                coll.Add(common.myStr(hdnBPDiastolic.Value));
                coll.Add(common.myStr(txtBPDiastolic.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtMAC.Text) > 0)
            {
                coll.Add(common.myStr(hdnMAC.Value));
                coll.Add(common.myStr(txtMAC.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(txtSpO2.Text) > 0)
            {
                coll.Add(common.myStr(hdnSpO2.Value));
                coll.Add(common.myStr(txtSpO2.Text).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(hdnBMIValue.Value) > 0)
            {
                coll.Add(common.myStr(hdnBMI.Value));
                coll.Add(common.myStr(hdnBMIValue.Value).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(hdnBSAValue.Value) > 0)
            {
                coll.Add(common.myStr(hdnBSA.Value));
                coll.Add(common.myStr(hdnBSAValue.Value).Trim());
                coll.Add(0);
                objXMLVital.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region Allergy
            coll = new ArrayList();
            if (common.myInt(hdnItemId.Value) > 0 && common.myInt(ddlAllergySeverity.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select interaction severity", Page);
                return;
            }
            int bitNKDA = 0;
            if (chkNoAllergies.Checked)
            {
                bitNKDA = 1;
            }
            if (common.myLen(hdnItemId.Value) > 0)
            {
                switch (common.myStr(hdnAllergyType.Value))
                {
                    case "Drug":
                        coll.Add(0);//Id int,             
                        coll.Add(string.Empty);//Drug_Id varchar(10),          
                        coll.Add(0);//Drug_Syn_ID int,
                        coll.Add(0);//GenericId int,
                        coll.Add(common.myInt(hdnItemId.Value));//ItemId int,
                        coll.Add(string.Empty);//Reaction varchar(500),          
                        coll.Add(0);//Intolerance bit,          
                        coll.Add(string.Empty);//AllergyDate varchar(10),          
                        coll.Add(string.Empty);//Remarks varchar(500),  
                        coll.Add(common.myInt(ddlAllergySeverity.SelectedValue));//AllerySeverity int 
                        coll.Add(0);
                        strXMLDrug.Append(common.setXmlTable(ref coll));
                        break;
                    case "Generic":
                        coll.Add(0);//Id int,             
                        coll.Add(string.Empty);//Drug_Id varchar(10),          
                        coll.Add(0);//Drug_Syn_ID int,
                        coll.Add(common.myInt(hdnItemId.Value));//GenericId int,
                        coll.Add(0);//ItemId int,
                        coll.Add(string.Empty);//Reaction varchar(500),          
                        coll.Add(0);//Intolerance bit,          
                        coll.Add(string.Empty);//AllergyDate varchar(10),          
                        coll.Add(string.Empty);//Remarks varchar(500),  
                        coll.Add(common.myInt(ddlAllergySeverity.SelectedValue));//AllerySeverity int 
                        coll.Add(0);
                        strXMLDrug.Append(common.setXmlTable(ref coll));
                        break;
                    case "CIMS":
                    case "VIDAL":
                    case "Food":
                    case "Others":
                        coll.Add(0);//Id int,             
                        coll.Add(common.myInt(hdnItemId.Value));//AllergyId int,          
                        coll.Add(string.Empty);//Reaction varchar(500),          
                        coll.Add(0);//Intolerance bit,          
                        coll.Add(string.Empty);//AllergyDate varchar(10),          
                        coll.Add(string.Empty);//Remarks varchar(500),  
                        coll.Add(0);
                        strXMLOther.Append(common.setXmlTable(ref coll));
                        break;
                }
            }
            #endregion
            #region History
            coll = new ArrayList();
            //bindVisitRecord(3145);
            if (!common.myStr(txtWHistory.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strHistory = common.myStr(txtWHistory.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strHistory = strHistory.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79244); //FieldId
                coll.Add("W");
                coll.Add(strHistory);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnHistoryRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnHistoryRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17552);
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region  PreviousTreatment
            coll = new ArrayList();
            //bindVisitRecord(3146);
            if (!common.myStr(txtWPrevTreatment.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strPreTre = common.myStr(txtWPrevTreatment.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strPreTre = strPreTre.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79246); //FieldId
                coll.Add("W");
                coll.Add(strPreTre);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnPreviousTreatmentRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnPreviousTreatmentRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17554);
                strNonTabularPT.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region Examination
            coll = new ArrayList();
            //bindVisitRecord(3140);
            if (!common.myStr(txtWExamination.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strExamination = common.myStr(txtWExamination.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strExamination = strExamination.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79239); //fieldId
                coll.Add("W");
                coll.Add(strExamination);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnExaminationRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnExaminationRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17546);
                strNonTabularE.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region  Nutrition
            coll = new ArrayList();
            //bindVisitRecord(3147);
            if (!common.myStr(txtWNutritionalStatus.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strNutritional = common.myStr(txtWNutritionalStatus.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strNutritional = strNutritional.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79247); //FieldId
                coll.Add("W");
                coll.Add(strNutritional);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnNutritionalStatusRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnNutritionalStatusRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17555);
                strNonTabularN.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region PlanofCare
            coll = new ArrayList();
            //bindVisitRecord(3024);
            if (!common.myStr(txtWPlanOfCare.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strPlanOfCare = common.myStr(txtWPlanOfCare.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(73518); //coll.Add(item2.Cells[0].Text);
                coll.Add("W");
                coll.Add(strPlanOfCare);
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnPlanOfCareRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnPlanOfCareRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17078);
                strNonTabularP.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region CostAnalysis
            coll = new ArrayList();
            //bindVisitRecord(3142);
            if (!common.myStr(txtWCostAnalysis.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strCostAnalysis = common.myStr(txtWCostAnalysis.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strCostAnalysis = strCostAnalysis.Replace("\n", "<br/>");
                coll = new ArrayList();
                coll.Add(79241); //FieldId
                coll.Add("W");//FieldType
                coll.Add(strCostAnalysis);//FieldValue
                coll.Add("0");//RowNo
                coll.Add(0); //RowCaptionId
                if (!common.myStr(hdnCostAnalysisRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnCostAnalysisRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17549); // RowSEcitonid
                strNonTabularC.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region NonDrugOrder
            coll = new ArrayList();
            if (common.myInt(ddlDoctor.SelectedValue).Equals(0)
                && !common.myStr(editorNonDrugOrder.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select doctor", Page);
                return;
            }
            if (common.myStr(ddlOrderType.SelectedValue).Equals(string.Empty)
                && !common.myStr(editorNonDrugOrder.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select order type", Page);
                return;
            }
            if (!common.myStr(editorNonDrugOrder.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strNonDrugOrder = common.myStr(editorNonDrugOrder.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strNonDrugOrder = strNonDrugOrder.Replace("\n", "<br/>");
                coll.Add(hdnNonDrugOrderId.Value);
                coll.Add(strNonDrugOrder);
                coll.Add(ddlOrderType.SelectedValue);
                coll.Add(ddlDoctor.SelectedValue);
                objXMLNonDrugOrder.Append(common.setXmlTable(ref coll));
            }
            #endregion
            #region Past History
            coll = new ArrayList();
            //bindVisitRecord(3163);   // Template Id
            if (!common.myStr(txtPHistory.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                coll = new ArrayList();
                coll.Add(79727); //FieldId
                coll.Add("W");
                coll.Add(common.myStr(txtPHistory.Text).Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                coll.Add("0");
                coll.Add(0); //coll.Add(RowCaptionId);
                if (!common.myStr(hdnPastHistoryRecordId.Value).Equals(string.Empty))
                {
                    coll.Add(common.myInt(hdnPastHistoryRecordId.Value));
                }
                else
                {
                    coll.Add(0);//common.myInt(ViewState["RecordId"])
                }
                coll.Add(17615);  // SectionId
                strNonTabularPH.Append(common.setXmlTable(ref coll));
            }
            #endregion
            xmlTemplateDetails.Append(strNonTabularH.ToString());
            xmlTemplateDetails.Append(strNonTabularPT.ToString());
            xmlTemplateDetails.Append(strNonTabularE.ToString());
            xmlTemplateDetails.Append(strNonTabularN.ToString());
            xmlTemplateDetails.Append(strNonTabularP.ToString());
            xmlTemplateDetails.Append(strNonTabularC.ToString());
            xmlTemplateDetails.Append(strNonTabularPH.ToString());

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
            APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.DoctorId = common.myInt(Session["DoctorID"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.xmlProblemDetails = objXMLProblem.ToString();
            objRoot.sProvisionalDiagnosis = ProvisionalDiagnosis;
            objRoot.xmlVitalString = objXMLVital.ToString();
            objRoot.strXMLDrug = strXMLDrug.ToString();
            objRoot.strXMLOther = strXMLOther.ToString();
            objRoot.ProvisionalDiagnosisId = intProvisionalDiagnosisId;
            objRoot.DiagnosisSearchId = DiagnosisSearchId;
            objRoot.bitNKDA = bitNKDA;
            objRoot.xmlTemplateDetails = xmlTemplateDetails.ToString();
            objRoot.iSign = iSign;
            objRoot.xmlNonDrugOrder = objXMLNonDrugOrder.ToString();


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            //Hashtable HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //                        common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
            //                        common.myInt(Session["UserId"]), objXMLProblem.ToString(), ProvisionalDiagnosis, objXMLVital.ToString(),
            //                        strXMLDrug.ToString(), strXMLOther.ToString(), intProvisionalDiagnosisId, DiagnosisSearchId,
            //                        bitNKDA, xmlTemplateDetails.ToString(), iSign, objXMLNonDrugOrder.ToString());

            gvProblemDetails.EditIndex = -1;
            gvHistory.EditIndex = -1;
            gvPrevTreatment.EditIndex = -1;
            gvExamination.EditIndex = -1;
            gvNutritional.EditIndex = -1;
            gvPlanOfCare.EditIndex = -1;
            gvCostAnalysis.EditIndex = -1;
            gvData.EditIndex = -1;
            gvNonDrugOrder.EditIndex = -1;
            gvPHistory.EditIndex = -1;

            BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
            ClearMessageControl();
            txtHeight.Text = string.Empty;
            TxtWeight.Text = string.Empty;
            TxtTemperature.Text = string.Empty;
            txtRespiration.Text = string.Empty;
            txtPulse.Text = string.Empty;
            txtBPSystolic.Text = string.Empty;
            txtBPDiastolic.Text = string.Empty;
            txtHC.Text = string.Empty;
            txtMAC.Text = string.Empty;
            txtSpO2.Text = string.Empty;
            txtBMI.Text = string.Empty;
            hdnBMIValue.Value = string.Empty;
            txtBSA.Text = string.Empty;
            hdnBSAValue.Value = string.Empty;
            txtedit.Text = string.Empty;
            editorChiefComplaints.Text = string.Empty;
            ClearEditorControls();
            ddlAllergySeverity.SelectedIndex = 0;
            hdnAllergyType.Value = string.Empty;
            ddlBrand.Text = string.Empty;
            hdnItemId.Value = string.Empty;
            hdnItemName.Value = string.Empty;
            ViewState["strId"] = null;
            ViewState["_ID"] = null;
            hdnNonDrugOrderId.Value = null;
            hdnHistoryRecordId.Value = string.Empty;
            hdnPastHistoryRecordId.Value = string.Empty;
            hdnPreviousTreatmentRecordId.Value = string.Empty;
            hdnExaminationRecordId.Value = string.Empty;
            hdnNutritionalStatusRecordId.Value = string.Empty;
            hdnPlanOfCareRecordId.Value = string.Empty;
            hdnCostAnalysisRecordId.Value = string.Empty;
            ddlDiagnosisSearchCodes.SelectedIndex = 0;
            ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindByValue(common.myStr(Session["EmployeeId"])));
            if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = sValue;
                lblmessage1.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmessage1.Text = sValue;
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "Message('" + sValue + "');", true);
                hdnCurrentControlFocused.Value = null;
                hdnIsTransitDataEntered.Value = "0";
                reSetTimer();
            }
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);



            //ViewState["CheifComplaintFound"] = common.myInt(dl.ExecuteScalar(CommandType.Text, "uspCheckPatientProblem @encounterID=" + common.myInt(Session["EncounterId"])));
            if (!common.myInt(ViewState["CheifComplaintFound"]).Equals(0) || common.myLen(objXMLProblem.ToString()) > 0)
                ViewState["CheifComplaintFound"] = 1;
            else
                ViewState["CheifComplaintFound"] = 0;
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
            //objParse = null;
            //objEMR = null;
            strMsg = string.Empty;
            //objAllergy = null;
            dsAllergy.Dispose();
            objXMLVital = null;
            strXMLDrug = null;
            strXMLOther = null;
            objXMLProblem = null;
            objXMLNonDrugOrder = null;
            strNonTabularH = null;
            strNonTabularPT = null;
            strNonTabularE = null;
            strNonTabularN = null;
            strNonTabularP = null;
            strNonTabularC = null;
            xmlTemplateDetails = null;
            strNonTabularPH = null;
            coll = null;
        }
    }
    protected void btnICCA_OnClick(object sender, EventArgs e)
    {
        //string fileOfpatient = "";
        //BaseC.Dynamic objdy = new BaseC.Dynamic(sConString);
        //DataSet ds = objdy.GetTableWiseData(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "FacilityMaster");
        //string strIccaLocationfile = common.myStr(ds.Tables[0].Rows[0]["ICCAPatientDocumentPath"]);
        //if (common.myStr(ds.Tables[0].Rows[0]["ICCAPatientDocumentPath"]) != "")
        //{
        //    DirectoryInfo objDir = new DirectoryInfo(@"" + common.myStr(ds.Tables[0].Rows[0]["ICCAPatientDocumentPath"]));
        //    // string[] fi_array = Directory.GetFiles(@"" + common.myStr(ds.Tables[0].Rows[0]["ICCAPatientDocumentPath"]) + Convert.ToString(Session["RegistrationNo"]) + "*");
        //    // string[] fi_array = Directory.GetFiles(@"D:\Rajeev\icca\" + Convert.ToString(Session["RegistrationNo"]) + "*");
        //    if (objDir.Exists == true)
        //    {
        //        FileInfo[] fi_array = objDir.GetFiles(Convert.ToString(Session["RegistrationNo"]) + "*");
        //        if (fi_array.Length > 0)
        //        {
        //            fileOfpatient = fi_array[0].Name;
        //            if (fi_array.Length == 1)
        //            {
        //                string path = @"" + strIccaLocationfile + fi_array[0].Name;
        //                WebClient client = new WebClient();
        //                Byte[] buffer = client.DownloadData(path);
        //                if (buffer != null)
        //                {
        //                    Response.ContentType = "application/pdf";
        //                    Response.AddHeader("content-length", buffer.Length.ToString());
        //                    Response.BinaryWrite(buffer);
        //                }
        //            }
        //            else
        //            {
        //                RadWindowForNew.NavigateUrl = "~/EMR/Dashboard/ICCAFile.aspx";
        //                RadWindowForNew.Width = 800;
        //                RadWindowForNew.Height = 330;
        //                RadWindowForNew.Top = 10;
        //                RadWindowForNew.Left = 10;
        //                RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
        //                RadWindowForNew.Modal = true;
        //                RadWindowForNew.VisibleStatusbar = false;
        //            }
        //        }
        //        else
        //        {
        //            Alert.ShowAjaxMsg("File Not Exists !", this.Page);
        //        }
        //    }
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("File location is not save in database  !", this.Page);
        //}
    }
    private void SetPermission()
    {
        if (!common.myInt(Session["ModuleId"]).Equals(3))
        {
            return;
        }
        //UserAuthorisations ua1 = new UserAuthorisations(sConString);
        UserAuthorisations ua1 = new UserAuthorisations();
        try
        {
            ViewState["SaveEnable"] = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["EditEnable"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["DeleteEnable"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
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
    protected void btnDefinalise_OnClick(object sender, EventArgs e)
    {
        try
        {
            SaveDasboardData(2);//Unsign
            Session["EncounterStatus"] = "OPEN";
            btnDefinalise.Visible = false;
            btnSaveDashboard.Visible = true;
            btnSave.Visible = true;
            if (common.myStr(Session["OPIP"]).Equals("O"))
            {
                btnSaveSign.Visible = true;
                btnSaveAsSigned.Visible = true;
                btnSaveDashboard.Text = "Save As Unsigned (F3)";
                btnSave.Text = "Save As Unsigned (F3)";
            }
            else
            {
                btnSaveSign.Visible = false;
                btnSaveAsSigned.Visible = false;
                btnSaveDashboard.Text = "Save (F3)";
                btnSave.Text = "Save (F3)";
            }
            OpenButtonControls();
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
    #region Paging
    protected void lnkPageVitals_OnClick(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        BindCommonData("Vitals", "S", string.Empty, 0, pageIndex);
    }
    protected void lnkPageOrdersAndProcedures_OnClick(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        BindCommonData("Orders And Procedures", "S", string.Empty, 0, pageIndex);
    }
    protected void lnkPagePrescriptions_OnClick(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        BindCommonData("Prescription", "S", string.Empty, 0, pageIndex);
    }
    private void populatePager(Repeater rpt, int TotalRecordsCount, int PageNo, int PageSize)
    {
        try
        {
            double dblPageCount = (double)((decimal)TotalRecordsCount / decimal.Parse(PageSize.ToString()));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                pages.Add(new ListItem("First", "1", PageNo > 1));
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), !i.Equals(PageNo)));
                }
                pages.Add(new ListItem("Last", pageCount.ToString(), PageNo < pageCount));
            }
            rpt.DataSource = pages;
            rpt.DataBind();
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
    #region Past History
    protected void lnkPastHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            ClearMessageControl();
            autoSaveDataInTransit(true);
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3025";
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=3025&SingleScreenTemplateCode=PHIS";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "addPastHistoryOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
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
    protected void btnBindPasthistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindHistoryData("PHIS");
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
    protected void gvPHistory_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
                || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                HiddenField hdnTemplateName = (HiddenField)e.Row.FindControl("hdnTemplateName");
                TextBox editorPHistory = (TextBox)e.Row.FindControl("editorHistory");
                editorPHistory.Text = common.myStr(hdnTemplateName.Value);
                editorPHistory.Enabled = !(e.Row.RowState.Equals(DataControlRowState.Normal) | e.Row.RowState.Equals(DataControlRowState.Alternate));
                editorPHistory.Text = common.clearHTMLTags(editorPHistory.Text);
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[(byte)enumPHistory.Edit].Controls[0];
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                    }
                }
                setControlHeight(editorPHistory, 125);
            }
        }
        catch
        {
        }
    }
    protected void gvPHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPHistory.PageIndex = e.NewPageIndex;
        BindHistoryData("PHIS");
    }
    protected void gvPHistory_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvPHistory.EditIndex = -1;
        BindHistoryData("PHIS");
    }
    protected void gvPHistory_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strNonTabularH = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();

        try
        {
            HiddenField hdnTemplateID = (HiddenField)gvPHistory.Rows[e.RowIndex].FindControl("hdnTemplateID");
            HiddenField hdnRecordId = (HiddenField)gvPHistory.Rows[e.RowIndex].FindControl("hdnRecordId");
            TextBox editorHistory = (TextBox)gvPHistory.Rows[e.RowIndex].FindControl("editorHistory");
            if (common.myLen(editorHistory.Text) > 0)
            {
                coll = new ArrayList();
                coll.Add(79727);  //FieldID
                coll.Add("W");
                coll.Add(editorHistory.Text);
                coll.Add("0");
                coll.Add(0);
                coll.Add(common.myInt(hdnRecordId.Value));
                coll.Add(17615); // SectionID
                strNonTabularH.Append(common.setXmlTable(ref coll));
            }
            if (common.myLen(strNonTabularH.ToString()) >= 1)
            {

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDashboard";
                APIRootClass.SaveSingleScreenDashboard objRoot = new global::APIRootClass.SaveSingleScreenDashboard();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.xmlProblemDetails = string.Empty;
                objRoot.sProvisionalDiagnosis = string.Empty;
                objRoot.xmlVitalString = string.Empty;
                objRoot.strXMLDrug = string.Empty;
                objRoot.strXMLOther = string.Empty;
                objRoot.ProvisionalDiagnosisId = 0;
                objRoot.DiagnosisSearchId = 0;
                objRoot.bitNKDA = 0;
                objRoot.xmlTemplateDetails = strNonTabularH.ToString();
                objRoot.iSign = 0;
                objRoot.xmlNonDrugOrder = string.Empty;


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                if (sValue.ToUpper().Contains(" UPDATED") || sValue.ToUpper().Contains(" SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = sValue;
                    lblPHistoryMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblPHistoryMessage.Text = sValue;
                }
                //HshOut = objEMR.SaveSingleScreenDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                       common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                       common.myInt(Session["UserId"]), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, strNonTabularH.ToString(), 0, string.Empty);
            }
            gvPHistory.EditIndex = -1;
            BindHistoryData("PHIS");

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
            coll = null;
            strNonTabularH = null;
            //objEMR = null;
            //HshOut = null;
        }
    }
    protected void gvPHistory_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["EditEnable"]) && !common.myBool(Session["isEMRSuperUser"]))
            {
                Alert.ShowAjaxMsg("You are not authorized to Edit", Page);
                return;
            }
            else if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                return;
            }
            else
            {
                gvPHistory.EditIndex = e.NewEditIndex;
                BindHistoryData("PHIS");
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
    protected void btnBackToMenu_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Templates/TemplateNotes.aspx?Mpg=P1265", false);
    }
    private void setControlHeight(TextBox TXT, int intMaxLength)
    {
        try
        {
            if (common.myLen(TXT.Text) > 4)
            {
                double dblLineChar = 60.00;
                if (intMaxLength > 90)
                {
                    dblLineChar = 80.00;
                }
                string[] lines = Regex.Split(common.myStr(TXT.Text), "\r\n");
                double intHeight = 20;
                intHeight = (common.myLen(TXT.Text) / dblLineChar) * 20.00;
                if (intHeight < (lines.Length * 20))
                {
                    intHeight = (lines.Length * 20);
                }
                if (intHeight > 20 && intHeight < 40)
                {
                    intHeight = 40;
                }
                if (intHeight > 40 && intHeight < 60)
                {
                    intHeight = 60;
                }
                if (intHeight > 60 && intHeight < 80)
                {
                    intHeight = 80;
                }
                if (intHeight > intMaxLength)
                {
                    intHeight = intMaxLength;
                }
                TXT.Height = common.myInt(intHeight);
            }
        }
        catch
        {
        }
    }
    private void bindDataInTransit()
    {
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getEMRSingleScreenDataInTransit";
            APIRootClass.getEMRSingleScreenDataInTransit objRoot = new global::APIRootClass.getEMRSingleScreenDataInTransit();
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objEMR.getEMRSingleScreenDataInTransit(common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];
                    editorChiefComplaints.Text = common.myStr(DR["Complaints"]);
                    chkNoAllergies.Checked = common.myBool(DR["NoAllergies"]);
                    ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindItemByValue(common.myStr(DR["AllergyId"])));
                    ddlBrand.Text = common.myStr(DR["AllergyName"]);
                    hdnItemId.Value = common.myStr(DR["AllergyId"]);
                    hdnAllergyType.Value = common.myStr(DR["AllergyType"]);
                    ddlAllergySeverity.SelectedIndex = ddlAllergySeverity.Items.IndexOf(ddlAllergySeverity.Items.FindByValue(common.myStr(DR["ServerityId"])));
                    txtHeight.Text = common.myStr(DR["HT"]);
                    TxtWeight.Text = common.myStr(DR["WT"]);
                    txtHC.Text = common.myStr(DR["HC"]);
                    TxtTemperature.Text = common.myStr(DR["T"]);
                    txtRespiration.Text = common.myStr(DR["R"]);
                    txtPulse.Text = common.myStr(DR["P"]);
                    txtBPSystolic.Text = common.myStr(DR["BPS"]);
                    txtBPDiastolic.Text = common.myStr(DR["BPD"]);
                    txtMAC.Text = common.myStr(DR["MAC"]);
                    txtSpO2.Text = common.myStr(DR["SPO2"]);
                    txtBMI.Text = common.myStr(DR["BMI"]);
                    hdnBMIValue.Value = common.myStr(DR["BMI"]);
                    txtBSA.Text = common.myStr(DR["BSA"]);
                    hdnBSAValue.Value = common.myStr(DR["BSA"]);
                    txtWHistory.Text = common.myStr(DR["History"]);
                    txtPHistory.Text = common.myStr(DR["PastHistory"]);
                    txtWPrevTreatment.Text = common.myStr(DR["PreviousTreatment"]);
                    txtWExamination.Text = common.myStr(DR["Examination"]);
                    txtWNutritionalStatus.Text = common.myStr(DR["NutritionalStatus"]);
                    txtWCostAnalysis.Text = common.myStr(DR["CostAnalysis"]);
                    txtWPlanOfCare.Text = common.myStr(DR["PlanOfCare"]);
                    ddlDiagnosisSearchCodes.SelectedIndex = ddlDiagnosisSearchCodes.Items.IndexOf(ddlDiagnosisSearchCodes.Items.FindByValue(common.myStr(DR["DiagnosisSearchKeyId"])));
                    editorProvDiagnosis.Text = common.myStr(DR["ProvisionalDiagnosis"]);
                    ddlOrderType.SelectedIndex = ddlOrderType.Items.IndexOf(ddlOrderType.Items.FindByValue(common.myStr(DR["OrderType"])));
                    ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindByValue(common.myStr(DR["NonDrugDoctorId"])));
                    editorNonDrugOrder.Text = common.myStr(DR["NonDrugOrder"]);
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
            //objEMR = null;
            ds.Dispose();
        }
    }
    private void autoSaveDataInTransit(bool IsPopupOpen)
    {
        //BaseC.ParseData objParse = new BaseC.ParseData();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myInt(Session["EncounterId"]) > 0)
            {
                if (common.myBool(hdnIsTransitDataEntered.Value))
                {
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveSingleScreenDataInTransit";
                    APIRootClass.SaveSingleScreenDataInTransit objRoot = new global::APIRootClass.SaveSingleScreenDataInTransit();
                    objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                    objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                    objRoot.Complaints = common.ParseString(common.myStr(editorChiefComplaints.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.IsNoAllergies = chkNoAllergies.Checked;
                    objRoot.AllergyId = common.myInt(hdnItemId.Value);
                    objRoot.AllergyName = common.myStr(ddlBrand.Text);
                    objRoot.AllergyType = common.myStr(hdnAllergyType.Value);
                    objRoot.ServerityId = common.myInt(ddlAllergySeverity.SelectedValue);
                    objRoot.VitalHT = common.myStr(txtHeight.Text);
                    objRoot.VitalWT = common.myStr(TxtWeight.Text);
                    objRoot.VitalHC = common.myStr(txtHC.Text);
                    objRoot.VitalT = common.myStr(TxtTemperature.Text);
                    objRoot.VitalR = common.myStr(txtRespiration.Text);
                    objRoot.VitalP = common.myStr(txtPulse.Text);
                    objRoot.VitalBPS = common.myStr(txtBPSystolic.Text);
                    objRoot.VitalBPD = common.myStr(txtBPDiastolic.Text);
                    objRoot.VitalMAC = common.myStr(txtMAC.Text);
                    objRoot.VitalSPO2 = common.myStr(txtSpO2.Text);
                    objRoot.VitalBMI = common.myStr(hdnBMIValue.Value);
                    objRoot.VitalBSA = common.myStr(hdnBSAValue.Value);
                    objRoot.History = common.ParseString(common.myStr(txtWHistory.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.PastHistory = common.ParseString(common.myStr(txtPHistory.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.PreviousTreatment = common.ParseString(common.myStr(txtWPrevTreatment.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.Examination = common.ParseString(common.myStr(txtWExamination.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.NutritionalStatus = common.ParseString(common.myStr(txtWNutritionalStatus.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.CostAnalysis = common.ParseString(common.myStr(txtWCostAnalysis.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.PlanOfCare = common.ParseString(common.myStr(txtWPlanOfCare.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.DiagnosisSearchKeyId = common.myInt(ddlDiagnosisSearchCodes.SelectedValue);
                    objRoot.ProvisionalDiagnosis = common.ParseString(common.myStr(editorProvDiagnosis.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.OrderType = common.myStr(ddlOrderType.SelectedValue);
                    objRoot.NonDrugDoctorId = common.myInt(ddlDoctor.SelectedValue);
                    objRoot.NonDrugOrder = common.ParseString(common.myStr(editorNonDrugOrder.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty));
                    objRoot.EncodedBy = common.myInt(Session["UserId"]);


                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    string strMsg = JsonConvert.DeserializeObject<string>(sValue);
                    //string strMsg = objEMR.SaveSingleScreenDataInTransit(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                    //                                     common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]),
                    //                                     objParse.ParseQ(common.myStr(editorChiefComplaints.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     chkNoAllergies.Checked, common.myInt(hdnItemId.Value), common.myStr(ddlBrand.Text), common.myStr(hdnAllergyType.Value), common.myInt(ddlAllergySeverity.SelectedValue),
                    //                                     common.myStr(txtHeight.Text).Trim(), common.myStr(TxtWeight.Text).Trim(), common.myStr(txtHC.Text).Trim(), common.myStr(TxtTemperature.Text).Trim(), common.myStr(txtRespiration.Text).Trim(), common.myStr(txtPulse.Text).Trim(), common.myStr(txtBPSystolic.Text).Trim(), common.myStr(txtBPDiastolic.Text).Trim(), common.myStr(txtMAC.Text).Trim(), common.myStr(txtSpO2.Text).Trim(), common.myStr(hdnBMIValue.Value).Trim(), common.myStr(hdnBSAValue.Value).Trim(),
                    //                                     objParse.ParseQ(common.myStr(txtWHistory.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtPHistory.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtWPrevTreatment.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtWExamination.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtWNutritionalStatus.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtWCostAnalysis.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     objParse.ParseQ(common.myStr(txtWPlanOfCare.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)), common.myInt(ddlDiagnosisSearchCodes.SelectedValue),
                    //                                     objParse.ParseQ(common.myStr(editorProvDiagnosis.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     common.myStr(ddlOrderType.SelectedValue), common.myInt(ddlDoctor.SelectedValue),
                    //                                     objParse.ParseQ(common.myStr(editorNonDrugOrder.Text).Trim().Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty)),
                    //                                     common.myInt(Session["UserId"]));
                    if (common.myStr(strMsg).ToUpper().Contains(" UPDATED") || common.myStr(strMsg).ToUpper().Contains(" SAVED"))
                    {
                        hdnIsTransitDataEntered.Value = "0";
                    }
                }
                if (IsPopupOpen)
                {
                    hdnCurrentControlFocused.Value = null;
                    hdnIsTransitDataEntered.Value = "0";
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
            //objEMR = null;
            //objParse = null;
            reSetTimer();
            setFocus(IsPopupOpen);
        }
    }
    private void setFocus(bool IsPopupOpen)
    {
        try
        {
            if (!common.myBool(hdnIsTransitDataEntered.Value) && !IsPopupOpen)
            {
                //lblMessage.Text = DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                switch (common.myStr(hdnCurrentControlFocused.Value))
                {
                    case "ctl00$ContentPlaceHolder1$editorChiefComplaints":
                        editorChiefComplaints.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + editorChiefComplaints.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$chkNoAllergies":
                        chkNoAllergies.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$ddlBrand":
                        ddlBrand.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$ddlAllergySeverity":
                        ddlAllergySeverity.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$txtHeight":
                        txtHeight.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtHeight.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$TxtWeight":
                        TxtWeight.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + TxtWeight.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtHC":
                        txtHC.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtHC.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$TxtTemperature":
                        TxtTemperature.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + TxtTemperature.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtRespiration":
                        txtRespiration.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtRespiration.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtPulse":
                        txtPulse.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtPulse.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtBPSystolic":
                        txtBPSystolic.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtBPSystolic.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtBPDiastolic":
                        txtBPDiastolic.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtBPDiastolic.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtMAC":
                        txtMAC.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtMAC.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtSpO2":
                        txtSpO2.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtSpO2.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWHistory":
                        txtWHistory.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWHistory.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtPHistory":
                        txtPHistory.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtPHistory.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWPrevTreatment":
                        txtWPrevTreatment.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWPrevTreatment.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWExamination":
                        txtWExamination.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWExamination.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWNutritionalStatus":
                        txtWNutritionalStatus.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWNutritionalStatus.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWCostAnalysis":
                        txtWCostAnalysis.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWCostAnalysis.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$ddlDiagnosisSearchCodes":
                        ddlDiagnosisSearchCodes.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$editorProvDiagnosis":
                        editorProvDiagnosis.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + editorProvDiagnosis.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$txtWPlanOfCare":
                        txtWPlanOfCare.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + txtWPlanOfCare.ClientID + "');", true);
                        break;
                    case "ctl00$ContentPlaceHolder1$ddlOrderType":
                        ddlOrderType.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$ddlDoctor":
                        ddlDoctor.Focus();
                        break;
                    case "ctl00$ContentPlaceHolder1$editorNonDrugOrder":
                        editorNonDrugOrder.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "SetFocusAtEnd('" + editorNonDrugOrder.ClientID + "');", true);
                        break;
                }
            }
        }
        catch
        {
        }
    }
    private void reSetTimer()
    {
        try
        {
            //true For starting the timer
            //false For stop the timer
            TimerAutoSaveDataInTransit.Enabled = common.myBool(hdnIsTransitDataEntered.Value);
            if (common.myBool(hdnIsTransitDataEntered.Value))
            {
                editorChiefComplaints.Style["backgroundColor"] = "AntiqueWhite";
                txtHeight.Style["backgroundColor"] = "AntiqueWhite";
                TxtWeight.Style["backgroundColor"] = "AntiqueWhite";
                txtHC.Style["backgroundColor"] = "AntiqueWhite";
                TxtTemperature.Style["backgroundColor"] = "AntiqueWhite";
                txtRespiration.Style["backgroundColor"] = "AntiqueWhite";
                txtPulse.Style["backgroundColor"] = "AntiqueWhite";
                txtBPSystolic.Style["backgroundColor"] = "AntiqueWhite";
                txtBPDiastolic.Style["backgroundColor"] = "AntiqueWhite";
                txtMAC.Style["backgroundColor"] = "AntiqueWhite";
                txtSpO2.Style["backgroundColor"] = "AntiqueWhite";
                txtWHistory.Style["backgroundColor"] = "AntiqueWhite";
                txtPHistory.Style["backgroundColor"] = "AntiqueWhite";
                txtWPrevTreatment.Style["backgroundColor"] = "AntiqueWhite";
                txtWExamination.Style["backgroundColor"] = "AntiqueWhite";
                txtWNutritionalStatus.Style["backgroundColor"] = "AntiqueWhite";
                txtWCostAnalysis.Style["backgroundColor"] = "AntiqueWhite";
                editorProvDiagnosis.Style["backgroundColor"] = "AntiqueWhite";
                txtWPlanOfCare.Style["backgroundColor"] = "AntiqueWhite";
                editorNonDrugOrder.Style["backgroundColor"] = "AntiqueWhite";
            }
            else
            {
                editorChiefComplaints.Style["backgroundColor"] = "none";
                txtHeight.Style["backgroundColor"] = "none";
                TxtWeight.Style["backgroundColor"] = "none";
                txtHC.Style["backgroundColor"] = "none";
                TxtTemperature.Style["backgroundColor"] = "none";
                txtRespiration.Style["backgroundColor"] = "none";
                txtPulse.Style["backgroundColor"] = "none";
                txtBPSystolic.Style["backgroundColor"] = "none";
                txtBPDiastolic.Style["backgroundColor"] = "none";
                txtMAC.Style["backgroundColor"] = "none";
                txtSpO2.Style["backgroundColor"] = "none";
                txtWHistory.Style["backgroundColor"] = "none";
                txtPHistory.Style["backgroundColor"] = "none";
                txtWPrevTreatment.Style["backgroundColor"] = "none";
                txtWExamination.Style["backgroundColor"] = "none";
                txtWNutritionalStatus.Style["backgroundColor"] = "none";
                txtWCostAnalysis.Style["backgroundColor"] = "none";
                editorProvDiagnosis.Style["backgroundColor"] = "none";
                txtWPlanOfCare.Style["backgroundColor"] = "none";
                editorNonDrugOrder.Style["backgroundColor"] = "none";
            }
        }
        catch
        {
        }
    }

    protected void TimerAutoSaveDataInTransit_OnTick(object sender, EventArgs e)
    {
        try
        {
            //lblMessage.Text = DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            if (common.myBool(hdnIsTransitDataEntered.Value))
            {
                autoSaveDataInTransit(false);
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
            reSetTimer();
        }
    }

    protected void btnAssigntoMe_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["IsLoginDoctor"]) == false)
            {
                Alert.ShowAjaxMsg("Only Doctor Can Assign !", this);
                return;
            }

            string ServiceURL = WebAPIAddress.ToString() + "api/Common/ChangeEncounterDoctor";
            APIRootClass.ChangeEncounterDoctor objRoot = new global::APIRootClass.ChangeEncounterDoctor();
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.DoctorId = common.myInt(Session["EmployeeId"]);
            objRoot.UserId = common.myInt(Session["UserId"]);


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //dl.ExecuteNonQuery(CommandType.Text, "Exec UspChangeEncounterDoctor @encounterID=" + common.myInt(Session["EncounterId"]).ToString() + ",@doctorID =" + 
            //common.myInt(common.myInt(Session["EmployeeId"])).ToString() + ",@userID =" + common.myInt(Session["UserId"]));
        }
        catch (Exception ex)
        {

        }
        //btnAssigntoMe
    }

    protected void btnShowHistory_Click(object sender, EventArgs e)
    {
        try
        {//common.myInt(Session["RegistrationID"])\
            Button btn = sender as Button;
            if (btn.ID.Contains("History"))
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getPAstHistory";
                APIRootClass.getPAstHistory objRoot = new global::APIRootClass.getPAstHistory();
                objRoot.regID = common.myInt(Session["RegistrationID"]);
                objRoot.encounterID = common.myInt(Session["EncounterId"]);
                objRoot.doctorID = common.myInt(Session["EmployeeId"]);


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //DataSet ds = dl.FillDataSet(CommandType.Text, "Exec uspgetPAstHistory @regID=" + common.myInt(Session["RegistrationID"]) + ", @encounterID=" + common.myInt(Session["EncounterId"]).ToString() + ",@doctorID =" + common.myInt(common.myInt(Session["EmployeeId"])).ToString());
                gvhistoryautopop.DataSource = ds.Tables[0];
                gvhistoryautopop.DataBind();
                dvHhostory.Visible = true;
            }
            else
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getPAstCheifComplaints";
                APIRootClass.getPAstCheifComplaints objRoot = new global::APIRootClass.getPAstCheifComplaints();
                objRoot.regID = common.myInt(Session["RegistrationID"]);
                objRoot.encounterID = common.myInt(Session["EncounterId"]);
                objRoot.doctorID = common.myInt(Session["EmployeeId"]);


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //DataSet ds = dl.FillDataSet(CommandType.Text, "Exec uspgetPAstCheifComplaints @regID=" + common.myInt(Session["RegistrationID"]) + ", @encounterID=" + common.myInt(Session["EncounterId"]).ToString() + ",@doctorID =" + common.myInt(common.myInt(Session["EmployeeId"])).ToString());
                gvhistoryData.DataSource = ds.Tables[0];
                gvhistoryData.DataBind();
                dvchfhistory.Visible = true;
            }
            //exec uspgetPAstCheifComplaints 196226,1871,0
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnCopyselected_Click(object sender, EventArgs e)
    {
        string seelctedText = "";
        for (int i = 0; i < gvhistoryData.Rows.Count; i++)
        {
            if ((gvhistoryData.Rows[i].FindControl("chkselect") as CheckBox).Checked == true)
            {
                seelctedText = seelctedText + (gvhistoryData.Rows[i].FindControl("txtHistory") as TextBox).Text + System.Environment.NewLine;
            }
        }
        editorChiefComplaints.Text = seelctedText;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvchfhistory.Visible = false;
        dvHhostory.Visible = false;
    }

    protected void btnhistoryCopy_Click(object sender, EventArgs e)
    {
        string seelctedText = "";
        for (int i = 0; i < gvhistoryautopop.Rows.Count; i++)
        {
            if ((gvhistoryautopop.Rows[i].FindControl("chkselect") as CheckBox).Checked == true)
            {
                seelctedText = seelctedText + (gvhistoryautopop.Rows[i].FindControl("txtHistory") as TextBox).Text + System.Environment.NewLine;
            }
        }
        txtWHistory.Text = seelctedText;
    }

    protected void lnkIPExtension_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/Approval/Extensions.aspx?RNo=" + common.myStr(Session["RegistrationNo"])
                               + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
                               + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"])
                               + "&FromWard=Y&OP_IP=I&Category=PopUp";
        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 630;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = string.Empty;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
    }

    protected void btnCopyExamination_Click(object sender, EventArgs e)
    {

    }

    protected void lnkpreauth_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Newpreauth.aspx?EncId=" + common.myInt(Session["EncounterId"]);
        //"/Approval/Extensions.aspx?RNo=" + common.myStr(Session["RegistrationNo"])
        //                     + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
        //                     + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"])
        //                     + "&FromWard=Y&OP_IP=I&Category=PopUp";
        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 630;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = string.Empty;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
    }

    protected void lnktriageform_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=TrigData&ToeknNo=" + common.myInt(lnktriageform.CommandName).ToString();
        //RadWindow1.Height = 400;
        //RadWindow1.Width = 500;
        RadWindowForNew.Top = 50;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    private void setTabVisibility()
    {
        //clsIVF objI = new clsIVF(sConString);
        DataSet dsSpec = new DataSet();
        DataSet ds = new DataSet();
        try
        {
            trChiefComplaints.Visible = false;
            trAllergies.Visible = false;
            trVitals.Visible = false;
            trHistory.Visible = false;
            trExamination.Visible = false;
            trPlanOfCare.Visible = false;
            trOtherNotes.Visible = false;
            trProvisionalDiagnosis.Visible = false;
            trOrdersAndProcedures.Visible = false;
            trPrescriptions.Visible = false;
            divDiagnosisDetails.Visible = false;


            /* New Templates*/
            trImmunisationHistory.Visible = false;
            trPastHistory.Visible = false;
            trPreviousTreatment.Visible = false;
            trNutritionalStatus.Visible = false;
            trCostAnalysis.Visible = false;
            trNonDrugOrder.Visible = false;
            trPatientFamilyEducationCounseling.Visible = false;
            trReferralsReplyToReferrals.Visible = false;
            trAnaesthesiaCriticalCareNotes.Visible = false;
            trMultidisciplinaryEvaluationPlanOfCare.Visible = false;
            //trTreatment.Visible = false;
            //trRemark.Visible = false;
            //trLabResult.Visible = false;

            if (common.myInt(Session["FacilityId"]) > 0)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getSingleScreenUserTemplates";
                APIRootClass.getSingleScreenUserTemplates objRoot = new global::APIRootClass.getSingleScreenUserTemplates();
                objRoot.SpecialisationId = 0;
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.DoctorId = common.myInt(Session["EmployeeId"]);


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                //ds = objI.getSingleScreenUserTemplates(0, common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));
                ViewState["dsgetSingleScreenUserTemplates"] = ds;
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int rowIdx = 0; rowIdx < ds.Tables[0].Rows.Count; rowIdx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[rowIdx];
                            switch (common.myStr(DR["TemplateCode"]))
                            {
                                //case "TG"://Treatment Given
                                //    trTreatment.Visible = true;
                                //    spnTreatmentGivenStar.Visible = common.myBool(DR["IsMandatory"]);
                                //    if (common.myBool(DR["IsCollapse"]))
                                //    {
                                //        pnlTreatmentGiven.Visible = false;
                                //        imgTreatmentGiven.ImageUrl = "~/Images/Collapse.jpg";
                                //    }
                                //    break;
                                //case "IN"://Remarks
                                //    trRemark.Visible = true;
                                //    spnInstructionsStar.Visible = common.myBool(DR["IsMandatory"]);
                                //    if (common.myBool(DR["IsCollapse"]))
                                //    {
                                //        pnlInstructions.Visible = false;
                                //        imgInstructions.ImageUrl = "~/Images/Collapse.jpg";
                                //    }
                                //    break;
                                case "COM"://Chief Complaints
                                    trChiefComplaints.Visible = true;
                                    spnChiefComplaintsStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlChiefComplaints.Visible = false;
                                        imgbtnChiefComplaints.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "ALG"://Allergies
                                    trAllergies.Visible = true;
                                    spnAllergiesStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel2.Visible = false;
                                        tblAllergiesDetail.Visible = false;
                                        ImageButton3.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "IMH"://Immunisation History
                                    trImmunisationHistory.Visible = true;
                                    //spnImmunisationHistory.Visible = common.myBool(DR["IsMandatory"]);
                                    break;


                                case "VTL"://Vitals
                                    trVitals.Visible = true;
                                    spnVitalsStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlVitals.Visible = false;
                                        imgVbtnVital.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "HIS"://History
                                    trHistory.Visible = true;
                                    spnHistoryStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlHistory.Visible = false;
                                        imbtnHistory.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "PRT"://Previous Treatment
                                    trPreviousTreatment.Visible = true;
                                    spnPreviousTreatment.Visible = common.myBool(DR["IsMandatory"]);
                                    Panel3.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel3.Visible = false;
                                        ImageButton2.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "EXM"://Examination
                                    trExamination.Visible = true;
                                    spnExaminationStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel10.Visible = false;
                                        imgbtnTemplate.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "NTS"://Nutritional Status
                                    trNutritionalStatus.Visible = true;
                                    spnNutritionalStatus.Visible = common.myBool(DR["IsMandatory"]);
                                    Panel6.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel6.Visible = false;
                                        imgbtnTemplate.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "POC"://Plan Of Care
                                    trPlanOfCare.Visible = true;
                                    spnPlanOfCareStar.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel13.Visible = false;
                                        ImageButton4.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "COA"://Cost Analysis
                                    trCostAnalysis.Visible = true;
                                    spnCostAnalysis.Visible = common.myBool(DR["IsMandatory"]);
                                    Panel14.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel14.Visible = false;
                                        ImageButton4.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "OTN"://Other Notes
                                    trOtherNotes.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlOtherNotes.Visible = false;
                                        imgbtntherNotes.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "PDG"://Provisional Diagnosis
                                    trProvisionalDiagnosis.Visible = true;
                                    spnProvisionalDiagnosisStar.Visible = common.myBool(DR["IsMandatory"]);
                                    pnlProvisionalDiagnosis.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlProvisionalDiagnosis.Visible = false;
                                        imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "ORD"://Orders And Procedures
                                    trOrdersAndProcedures.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlOrderProcedures.Visible = false;
                                        gvOrdersAndProcedures.Visible = false;
                                        imgbtnOrdersAndProcedures.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "PRS"://Prescriptions
                                    trPrescriptions.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlPrescription.Visible = false;
                                        gvPrescriptions.Visible = false;
                                        imgbtnPrescription.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "NDO"://Non Drug Order
                                    trNonDrugOrder.Visible = true;
                                    Panel20.Visible = true;
                                    spnNonDrugOrder.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel20.Visible = false;
                                        imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "ATM"://Attach Documents
                                           ////trAttachDocuments.Visible = true;
                                    break;
                                case "PFE"://Patient and family education and counseling
                                    trPatientFamilyEducationCounseling.Visible = true;

                                    break;
                                case "RRR"://Referrals and Reply to referrals
                                    trReferralsReplyToReferrals.Visible = true;
                                    break;
                                case "ACN"://Anaesthesia and Critical care notes
                                    trAnaesthesiaCriticalCareNotes.Visible = true;
                                    break;
                                case "MEP"://Multidisciplinary evaluation and plan of care
                                    trMultidisciplinaryEvaluationPlanOfCare.Visible = true;
                                    break;
                                case "DGN"://Diagnosis
                                    divDiagnosisDetails.Visible = true;
                                    pnlDiagnosis.Visible = true;
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        pnlDiagnosis.Visible = false;
                                        ImageButton13.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                case "PH"://Past History
                                    trPastHistory.Visible = true;
                                    Panel23.Visible = true;
                                    spnPastHistory.Visible = common.myBool(DR["IsMandatory"]);
                                    if (common.myBool(DR["IsCollapse"]))
                                    {
                                        Panel23.Visible = false;
                                        imgbtnProvisionalDiagnosies.ImageUrl = "~/Images/Collapse.jpg";
                                    }
                                    break;
                                    //case "LAB"://LAB
                                    //    trLabResult.Visible = true;
                                    //    break;
                            }
                        }
                    }
                    else
                    {
                        btnSave.Visible = false;
                    }
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
        }
    }

    private bool ValidatTemplate()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        ds = (DataSet)ViewState["dsgetSingleScreenUserTemplates"];
        try
        {
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DV = ds.Tables[0].DefaultView;
                        //Chief Complaints      [COM]
                        DV.RowFilter = "TemplateCode='COM' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            // trChiefComplaints.Visible = true;
                            if (common.myInt(gvProblemDetails.Rows.Count).Equals(1) && common.myStr(editorChiefComplaints.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvProblemDetails.Rows[0];
                                TextBox txteditorProblem = (TextBox)gv1.FindControl("editorProblem");
                                string strProblem = txteditorProblem.Text.Trim();
                                if (strProblem.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter chief complaints !", Page.Page);
                                    txteditorProblem.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Allergies             [ALG]                        
                        //DV.RowFilter = "TemplateCode='ALG' AND IsMandatory=true";
                        //if (DV.ToTable().Rows.Count > 0)
                        //{
                        //    //trAllergies.Visible = true;
                        //    if (common.myInt(gvAllergies.Rows.Count).Equals(0) && !chkNoAllergies.Checked)
                        //    {
                        //        Alert.ShowAjaxMsg("Please enter allergies !", Page.Page);
                        //        chkNoAllergies.Focus();
                        //        return false;
                        //    }
                        //}
                        //DV.RowFilter = string.Empty;


                        //Allergies[ALG]
                        DV.RowFilter = "TemplateCode='ALG' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            //trAllergies.Visible = true;
                            if ((common.myLen(editorAllergy.Text).Equals(0) && (common.myInt(ddlBrand.SelectedValue).Equals(0) || common.myInt(ddlAllergySeverity.SelectedValue).Equals(0))) && (!chkNoAllergies.Checked))
                            {
                                Alert.ShowAjaxMsg("Please enter allergies !", Page.Page);
                                chkNoAllergies.Focus();
                                return false;
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Vitals                [VTL]                        
                        DV.RowFilter = "TemplateCode='VTL' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            //  trVitals.Visible = true;
                            if (common.myInt(gvVitals.Rows.Count).Equals(0))
                            {
                                if ((txtHeight.Text).Equals(string.Empty)
                                && (TxtWeight.Text).Equals(string.Empty) && (txtHC.Text).Equals(string.Empty) && (TxtTemperature.Text).Equals(string.Empty)
                                && (txtRespiration.Text).Equals(string.Empty) && (txtPulse.Text).Equals(string.Empty) && (txtBPSystolic.Text).Equals(string.Empty)
                                && (txtBPDiastolic.Text).Equals(string.Empty) && (txtMAC.Text).Equals(string.Empty) && (txtSpO2.Text).Equals(string.Empty)
                                && (txtBMI.Text).Equals(string.Empty) && (txtBSA.Text).Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter vitals !", Page.Page);
                                    txtHeight.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //History               [HIS]                        
                        DV.RowFilter = "TemplateCode='HIS' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvHistory.Rows.Count).Equals(1))
                            {
                                GridViewRow gv1 = gvHistory.Rows[0];
                                TextBox txteditorHistory = (TextBox)gv1.FindControl("editorHistory");
                                string streditorHistory = txteditorHistory.Text.Trim();
                                if (streditorHistory.Equals(string.Empty) && common.myStr(txtWHistory.Text).Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter history !", Page.Page);
                                    txteditorHistory.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Examination           [EXM]                        
                        DV.RowFilter = "TemplateCode='EXM' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvExamination.Rows.Count).Equals(1) && common.myStr(txtWExamination.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvExamination.Rows[0];
                                TextBox txteditorExamination = (TextBox)gv1.FindControl("editorExamination");
                                string strExamination = txteditorExamination.Text.Trim();
                                if (strExamination.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter examination !", Page.Page);
                                    txteditorExamination.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        // TG 
                        //DV.RowFilter = "TemplateCode='TG' AND IsMandatory=true";
                        //if (DV.ToTable().Rows.Count > 0)
                        //{
                        //    if (common.myInt(gvTreatmentGiven.Rows.Count).Equals(1))
                        //    {
                        //        GridViewRow gv1 = gvTreatmentGiven.Rows[0];
                        //        TextBox txteditorTreatmentGiven = (TextBox)gv1.FindControl("editorTreatmentGiven");
                        //        string strTreatmentGiven = txteditorTreatmentGiven.Text.Trim();
                        //        if (strTreatmentGiven.Equals(string.Empty))
                        //        {
                        //            Alert.ShowAjaxMsg("Please enter Treatment Given details !", Page.Page);
                        //            txteditorTreatmentGiven.Focus();
                        //            return false;
                        //        }
                        //    }
                        //}
                        //DV.RowFilter = string.Empty;
                        //Plan Of Care          [POC]                        
                        DV.RowFilter = "TemplateCode='POC' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            // trPlanOfCare.Visible = true;
                            if (common.myInt(gvPlanOfCare.Rows.Count).Equals(1) && common.myStr(txtWPlanOfCare.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvPlanOfCare.Rows[0];
                                TextBox txteditorPlanOfCare = (TextBox)gv1.FindControl("editorPlanOfCare");
                                string strPlanOfCare = txteditorPlanOfCare.Text.Trim();
                                if (strPlanOfCare.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter plan of care !", Page.Page);
                                    txteditorPlanOfCare.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Other Notes           [OTN]                        
                        //DV.RowFilter = "TemplateCode='OTN' AND IsMandatory=true";
                        //if (DV.ToTable().Rows.Count > 0)
                        //{
                        //    //trOtherNotes.Visible = true;gvOtherNotes
                        //    if (common.myInt(gvOtherNotes.Rows.Count).Equals(1))
                        //    {
                        //        Alert.ShowAjaxMsg("Please enter other notes !", Page.Page);
                        //        return false;
                        //    }
                        //}
                        //DV.RowFilter = string.Empty;
                        //Provisional Diagnosis [PDG]                        
                        DV.RowFilter = "TemplateCode='PDG' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            //trProvisionalDiagnosis.Visible = true; gvData
                            if (common.myInt(gvData.Rows.Count).Equals(1))
                            {
                                GridViewRow gv1 = gvData.Rows[0];
                                TextBox txteditorProvisionalDiagnosis = (TextBox)gv1.FindControl("editorProvisionalDiagnosis");
                                string strProvisionalDiagnosis = txteditorProvisionalDiagnosis.Text.Trim();
                                if (strProvisionalDiagnosis.Equals(string.Empty) && common.myInt(ddlDiagnosisSearchCodes.SelectedValue).Equals(0))
                                {
                                    Alert.ShowAjaxMsg("Please enter provisional diagnosis !", Page.Page);
                                    txteditorProvisionalDiagnosis.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;

                        #region new Templates addition
                        // Past History PH
                        DV.RowFilter = "TemplateCode='PH' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvPHistory.Rows.Count).Equals(1) && common.myStr(txtPHistory.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvPHistory.Rows[0];
                                TextBox txteditorHistory = (TextBox)gv1.FindControl("editorHistory");
                                string strHistory = txteditorHistory.Text.Trim();
                                if (strHistory.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter Past History !", Page.Page);
                                    txteditorHistory.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Previous Treatment     [PT]
                        DV.RowFilter = "TemplateCode='PT' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvPrevTreatment.Rows.Count).Equals(1) && common.myStr(txtWPrevTreatment.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvPrevTreatment.Rows[0];
                                TextBox txteditorPrevTreatment = (TextBox)gv1.FindControl("editorPrevTreatment");
                                string strPrevTreatment = txteditorPrevTreatment.Text.Trim();
                                if (strPrevTreatment.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter Previous Treatment !", Page.Page);
                                    txteditorPrevTreatment.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;

                        // Nutritional Status NTS
                        DV.RowFilter = "TemplateCode='NTS' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvNutritional.Rows.Count).Equals(1) && common.myStr(txtWNutritionalStatus.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvNutritional.Rows[0];
                                TextBox txteditorNutritional = (TextBox)gv1.FindControl("editorNutritional");
                                string streditorNutritional = txteditorNutritional.Text.Trim();
                                if (streditorNutritional.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter Nutritional Status !", Page.Page);
                                    txteditorNutritional.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;


                        //Cost Analysis     [COA]
                        DV.RowFilter = "TemplateCode='COA' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvCostAnalysis.Rows.Count).Equals(1) && common.myStr(txtWCostAnalysis.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvCostAnalysis.Rows[0];
                                TextBox txteditorCostAnalysis = (TextBox)gv1.FindControl("editorCostAnalysis");
                                string strCostAnalysis = txteditorCostAnalysis.Text.Trim();
                                if (strCostAnalysis.Equals(string.Empty))
                                {
                                    Alert.ShowAjaxMsg("Please enter Cost Analysis !", Page.Page);
                                    txteditorCostAnalysis.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;


                        //Non Drug Order [NDO]
                        DV.RowFilter = "TemplateCode='NDO' AND IsMandatory=true";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            if (common.myInt(gvNonDrugOrder.Rows.Count).Equals(1) && common.myStr(editorNonDrugOrder.Text).Equals(string.Empty))
                            {
                                GridViewRow gv1 = gvNonDrugOrder.Rows[0];
                                TextBox txtedNonDrugOrder = (TextBox)gv1.FindControl("edNonDrugOrder");
                                string stredNonDrugOrder = txtedNonDrugOrder.Text.Trim();
                                if (stredNonDrugOrder.Equals(string.Empty) && (common.myInt(ddlOrderType.SelectedValue).Equals(0) || common.myInt(ddlDoctor.SelectedValue).Equals(0)))
                                {
                                    Alert.ShowAjaxMsg("Please enter Non Drug Order !", Page.Page);
                                    editorNonDrugOrder.Focus();
                                    return false;
                                }
                            }
                        }
                        DV.RowFilter = string.Empty;
                        //Remarks     [IN]
                        //DV.RowFilter = "TemplateCode='IN' AND IsMandatory=true";
                        //if (DV.ToTable().Rows.Count > 0)
                        //{
                        //    // trRemark.Visible = true;
                        //    if (common.myInt(gvRemarks.Rows.Count).Equals(1))
                        //    {
                        //        GridViewRow gv1 = gvRemarks.Rows[0];
                        //        TextBox txteditorRemarks = (TextBox)gv1.FindControl("editorRemarks");
                        //        string streditorRemarks = txteditorRemarks.Text.Trim();
                        //        if (streditorRemarks.Equals(string.Empty))
                        //        {
                        //            Alert.ShowAjaxMsg("Please enter instructions !", Page.Page);
                        //            txteditorRemarks.Focus();
                        //            return false;
                        //        }
                        //    }
                        //}
                        //DV.RowFilter = string.Empty;
                        //Remarks     [IN]
                        //DV.RowFilter = "TemplateCode='IN' AND IsMandatory=true";
                        //if (DV.ToTable().Rows.Count > 0)
                        //{
                        //    // trRemark.Visible = true;
                        //    if (common.myInt(gvRemarks.Rows.Count).Equals(1))
                        //    {
                        //        GridViewRow gv1 = gvRemarks.Rows[0];
                        //        TextBox txteditorRemarks = (TextBox)gv1.FindControl("editorRemarks");
                        //        string streditorRemarks = txteditorRemarks.Text.Trim();
                        //        if (streditorRemarks.Equals(string.Empty))
                        //        {
                        //            Alert.ShowAjaxMsg("Please enter instructions !", Page.Page);
                        //            txteditorRemarks.Focus();
                        //            return false;
                        //        }
                        //    }
                        //}
                        //DV.RowFilter = string.Empty;
                        #endregion
                    }
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
        return true;
    }
    protected void lnkCopyLastPrescription_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/EMR/Dashboard/PopUpPatientDashboardForDoctorNew.aspx?From=POPUP&CloseButtonShow=Yes";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "CopyLastPrescription";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
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
    protected void btnEnableControl_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (hdnButtonId.Value != "" && hdnButtonId.Value != "undefined")
            {
                string ButtonId = hdnButtonId.Value;
                switch (ButtonId)
                {
                    case "btnAddChiefComplaintsClose":
                        RetrievePatientProblemsDetail();
                        chkNoAllergies.Focus();
                        break;
                    case "btnAddAllergyClose":
                        btnAddVitalsClose.Focus();
                        Response.Redirect(Request.RawUrl.ToString()); // redirect on itself
                        break;
                    case "btnAddVitalsClose":
                        bindVitals();
                        break;
                    case "btnBindhistoryHistory":
                        BindHistoryData("HIS");
                        imgBtnTemplates.Focus();
                        break;
                    case "btnBindhistoryAddTemplate":
                        BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
                        imgBtnTemplates.Focus();
                        break;
                    case "btnBindhistoryExamination":
                        BindHistoryData("EXM");
                        imgbtnAddOrdersAndProcedures.Focus();
                        break;
                    case "btnAddOrdersAndProceduresClose":
                        bindOrdersAndProcedures();
                        break;
                    case "btnBindOrderGrid":
                        BindGridOrders();
                        break;
                    case "btnProvisionalDiagnosisClose":
                        BindCommonData("Provisional Diagnosis", "S", string.Empty, 0, 1);
                        imgBtnFinalDiagnosis.Focus();
                        break;
                    case "btnFinalDiagnosisClose":
                        BindCommonData("Diagnosis", "S", string.Empty, 0, 1);
                        imgBtnAddPrescriptions.Focus();
                        break;
                    case "btnAddDiagnosisSerchOnClientClose":
                        BindDiagnosisSearchCode();
                        imgBtnAddPrescriptions.Focus();
                        break;
                    case "btnAddPrescriptionsClose":
                        bindPrescriptions();
                        break;
                    case "btnBindPresGrid":
                        BindGrid();
                        break;
                    case "btnBindInstructionsTemplate":
                        BindHistoryData("EXM");
                        break;
                    case "btnBindhistoryPOF":
                        BindHistoryData("POC");
                        break;
                    case "btnBindOrderPriscriptionPlaneOfCare":
                        BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
                        break;
                    case "btnReloadSingleScreen":
                        BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
                        break;
                    case "":
                        BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
                        break;
                }
            }
            else
            {
                BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1);
            }
            if (chkNoAllergies.Checked)
            {
                ddlBrand.Enabled = false;
                ddlAllergySeverity.Enabled = false;
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
    public void BindGridOrders()
    {
        BindCommonData("Orders And Procedures", "S", string.Empty, 0, 1);
        if (common.myStr(Session["PlanOfCare"]) != null || common.myStr(Session["PlanOfCare"]) != string.Empty)
        {
            GridViewRow gridPlanOfCare = gvPlanOfCare.Rows[0];
            TextBox txteditorPlanOfCare = (TextBox)gridPlanOfCare.FindControl("editorPlanOfCare");
            txteditorPlanOfCare.Text = common.clearHTMLTags(common.myStr(Session["PlanOfCare"]));
        }
    }

    public void BindGrid()
    {
        BindCommonData("Prescription", "S", string.Empty, 0, 1);
    }


    public void bindTestData()
    {
        try
        {

            //setDate();
            // string sConStrings = "server=akhil;database=paras;uid=sa;pwd=;MultipleActiveResultSets=True; MAX POOL SIZE=300;";
            // BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConStrings);

            DataTable dt = new DataTable();
            // BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            int iProviderID = 0;// common.myInt(common.myInt(Session["UserId"]));

            int pageindex = 0;
            string EncounterNo = "", RegNo = "", Pname = "";
            //if (gvResultFinal.Rows.Count > 0)
            //{
            //    pageindex = gvResultFinal.PageIndex + 1;
            //}
            //else
            //{
            //    pageindex = 1;
            //}
            //if (ddlSearch.Visible)
            //{
            //    if (ddlSearch.SelectedValue == "IP")
            //    {
            //        EncounterNo = txtSearchCretria.Text.Trim();
            //    }
            //    else if (ddlSearch.SelectedValue == "PN")
            //    {
            //        Pname = txtSearchCretria.Text.Trim();
            //    }
            //    else if (ddlSearch.SelectedValue == "RN")
            //    {
            //        RegNo = common.myInt(txtSearchCretria.Text).ToString();
            //    }
            //}

            //if (common.myInt(Request.QueryString["mainRegNo"]) > 0)
            //{
            //    RegNo = common.myStr(Request.QueryString["mainRegNo"]);
            //}

            if (common.myInt(RegNo).Equals(0))
            {
                RegNo = string.Empty;
            }

            lblMessage.Text = "";
            //gvResultFinal.DataSource = null;
            //gvResultFinal.DataBind();
            bool isER = false;
            //if (Request.QueryString["IsER"] != null)
            //{
            //    if (common.myInt(Request.QueryString["IsER"]) == 1)
            //    {
            //        isER = true;
            //        lblheader.Text = "Lab Result  [ER]  ";
            //    }
            //}

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getPatientLabResultHistoryDash";
            APIRootClass.LabResultHistory objRoot = new global::APIRootClass.LabResultHistory();
            objRoot.FacilityId = common.myInt(Session["FacilityID"]);
            objRoot.iHostId = common.myInt(Session["HospitalLocationID"]);
            objRoot.fromDate = "";
            objRoot.toDate = "";
            objRoot.iRegNo = common.myStr(RegNo);
            objRoot.iProviderId = iProviderID;
            objRoot.iPageSize = 15;
            objRoot.iPageNo = 1;
            objRoot.AbnormalResult = false;
            objRoot.CriticalResult = false;
            objRoot.iStatusId = common.myInt(0);
            objRoot.iLoginFacilityId = common.myInt(Session["FacilityID"]);
            objRoot.chvEncounterNo = EncounterNo;
            objRoot.ReviewedStatus = common.myInt(0);
            objRoot.PatientName = common.myStr(Pname);
            objRoot.iUserId = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "AuditDiagSampleId = 1";
                //lblResultChanged.Text = " Result changed after provisional release : " + dv.Count.ToString();
                //if (common.myInt(dv.Count) > 0)
                //    lblResultChanged.CssClass = "blink";
                //else
                //    lblResultChanged.CssClass = "noblink";

                dv.RowFilter = "";
                dv.RowFilter = "ReviewedStatus = 1";
                Label lbl = Master.FindControl("labResultId") as Label;
                int count = (common.myInt(ds.Tables[0].Rows.Count) - common.myInt(dv.Count));
                if (count > 9)
                {
                    lbl.Text = "9" + "<sup>+</sup>";
                }
                else
                {
                    lbl.Text = "" + (common.myInt(ds.Tables[0].Rows.Count) - common.myInt(dv.Count));
                }


                //if ((common.myInt(ds.Tables[0].Rows.Count) - common.myInt(dv.Count)) > 0)
                //    lblNew.CssClass = "blink";
                //else
                //    lblNew.CssClass = "noblink";
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

        }
    }

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    string time = "Data Saved";
    //    //string script = " Message('" + time + "');";
    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "Message('" + time + "');", true);
    //    //ClientScript.RegisterStartupScript(this.GetType(), "Message", script, true);
    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "name", "Message('" + time + "');", true);
    //}
}