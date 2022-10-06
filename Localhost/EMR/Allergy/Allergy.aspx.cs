using System;
using System.Text;
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
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Allergy_Allergy : System.Web.UI.Page
{
    // private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();
    ParseData bc = new ParseData();
    private const int ItemsPerRequest = 50;
    // BaseC.clsPharmacy objPharmacy;
    //  BaseC.clsEMR objEMR;
    StringBuilder strXMLDrug;
    StringBuilder strXMLOther;
    ArrayList coll;
    // BaseC.EMRAllergy objemrallrgy;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
            ViewState["RegistrationNo"] = common.myInt(Session["RegistrationNo"]);
            ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);

            if (common.myInt(Request.QueryString["RegId"]) > 0)
            {
                lnkDemographics.Visible = false;
                lblAllergies.Visible = false;
                //lnkPatientRelation.Visible = true;
                lnkOtherDetails.Visible = false;
                lnkResponsibleParty.Visible = false;
                lnkPayment.Visible = false;
                lnkAttachDocument.Visible = false;

                ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
                ViewState["EncounterId"] = "0";

                // BaseC.Patient objP = new BaseC.Patient(sConString);
                string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientRegistrationNo";
                APIRootClass.Allergy objRoot1 = new global::APIRootClass.Allergy();
                objRoot1.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                WebClient client1 = new WebClient();
                client1.Headers["Content-type"] = "application/json";
                client1.Encoding = Encoding.UTF8;
                string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
                string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                ViewState["RegistrationNo"] = sValue1;
                //ViewState["RegistrationNo"] = objP.GetPatientRegistrationNo(common.myInt(ViewState["RegistrationId"]));
            }

            //ViewState["RegistrationId"] = "94795";
            //ViewState["EncounterId"] = "0";

            if (common.myInt(ViewState["RegistrationId"]) == 0)
            {
                Response.Redirect("/default.aspx?RegNo=0", false);
            }

            //objEMR = new BaseC.clsEMR(sConString);
            //objPharmacy = new BaseC.clsPharmacy(sConString);

            dtpAllergyDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpAllergyDate.DateInput.DisplayDateFormat = common.myStr(Session["OutputDateFormat"]);

            #region Interface
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getFacilityInterfaceDetails";
            APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
            objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.interfaceFor = common.myInt(common.enumCIMSorVIDALInterfaceFor.None);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet dsInterface = JsonConvert.DeserializeObject<DataSet>(sValue);
            // DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),common.myInt(BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.None));

            ViewState["IsCIMSInterfaceActive"] = false;
            ViewState["IsVIDALInterfaceActive"] = false;

            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    ViewState["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                }
                else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                {
                    ViewState["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                }
            }

            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                //rdoAllergyType.Items[0].Enabled = false;

                rdoAllergyType.Items[1].Enabled = true;
                rdoAllergyType.Items[1].Selected = true;
            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                //rdoAllergyType.Items[0].Enabled = false;

                rdoAllergyType.Items[2].Enabled = true;
                //rdoAllergyType.Items[2].Selected = true;

                rdoAllergyType.Items[0].Enabled = true;
                rdoAllergyType.Items[0].Selected = true;
            }

            #endregion

            if (common.myStr(Request.QueryString["From"]) == "POPUP")
            {
                btnClose.Visible = true;
            }
            hdnIsUnSavedData.Value = "0";
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                string pid = Session["CurrentNode"].ToString();
                int len = pid.Length;
                ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
            }
            else
            {
                ViewState["PageId"] = "8";
            }
            if (common.myStr(Session["IsMedicalAlert"]) == "")
            {
                lnkAlerts.Enabled = false;
                lnkAlerts.CssClass = "blinkNone";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
            }
            else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
            {
                lnkAlerts.Enabled = true;
                lnkAlerts.Font.Bold = true;
                lnkAlerts.Font.Size = 11;
                lnkAlerts.CssClass = "blink";
                lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            {
                btnNew.Visible = false;
                btnSave.Visible = false;
                btnupdate.Visible = false;
                lnkAllergyMaster.Visible = false;
                gvDrugAllergy.Enabled = false;
                gvCIMSAllergy.Enabled = false;
                gvVIDALAllergy.Enabled = false;
                gvDeactivateAllergy.Enabled = false;
                gvOtherAllergy.Enabled = false;
                chkNKA.Enabled = false;
                btnAddtogrid.Visible = false;
                ddlGeneric.Enabled = false;
                ddlBrand.Enabled = false;
            }
            //BindPatientHiddenDetails();
            //txtReaction.Attributes.Add("onblur", "nSat=1;");

            Cache.Remove("DrugAllergy" + Session["UserId"]);
            Cache.Remove("CIMSAllergy" + Session["UserId"]);
            Cache.Remove("VIDALAllergy" + Session["UserId"]);
            Cache.Remove("OtherAllergy" + Session["UserId"]);
            Cache.Remove("PatientAllergy" + ViewState["RegistrationId"]);
            RetrievePatientAllergies();
            Session["AllergyTypeSelected"] = null;
        }
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations("");
        try
        {
            btnupdate.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            btnAddtogrid.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));

            ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["IsAllowCancel"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }


    //void BindPatientHiddenDetails()
    //{
    //    try
    //    {
    //        if (Session["PatientDetailString"] != null)
    //        {
    //            lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //private void PopulateAllergy(string strText)
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();
    //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        Hashtable hshin = new Hashtable();
    //        hshin.Add("@inyHospitalLocationID", Session["HospitalLocationId"]);
    //        hshin.Add("@inyTypeID", rdoAllergyType.SelectedValue);
    //        if (strText.Length > 0)
    //        {
    //            hshin.Add("@chvSearchCriteria", strText);
    //        }
    //        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspGetAllergyMaster", hshin);

    //        gvAllergyList.DataSource = ds.Tables[0];
    //        gvAllergyList.DataBind();
    //        lblMessage.Text = "";
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    protected void GetDeActivityAllergies()
    {
        //BaseC.Dashboard objDas = new BaseC.Dashboard();
        try
        {
            if (Session["HospitalLocationId"] != null && ViewState["RegistrationId"] != null)
            {

                //DataSet objDs = objDas.getAllergies(common.myInt(ViewState["RegistrationId"]), 0);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientAllergies";
                APIRootClass.GetPatientAllergies objRoot = new global::APIRootClass.GetPatientAllergies();
                objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                objRoot.FromDate = "";
                objRoot.ToDate = "";
                objRoot.GroupingDate = "";
                objRoot.Active = 0;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                //sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


                gvDeactivateAllergy.DataSource = objDs.Tables[0];
                gvDeactivateAllergy.DataBind();
                tbpnlDeactivateAllergy.HeaderText = "De-Activate Allergies(" + common.myStr(objDs.Tables[0].Rows.Count) + ")";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            //objDas = null;
        }
    }

    protected void RetrievePatientAllergies()
    {
        BindNotes bNote = new BindNotes(string.Empty);
        //BaseC.Security AuditCA = new BaseC.Security(string.Empty);

        //BaseC.Dashboard objDas = new BaseC.Dashboard();
        try
        {
            bool bitNoDrugData = false;
            bool bitNoCIMSData = false;
            bool bitNoVIDALData = false;
            bool bitNoOtherData = false;

            if (Session["HospitalLocationId"] != null && ViewState["RegistrationId"] != null)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getAllergies";
                APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
                objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                //DataSet objDs = objDas.getAllergies(common.myInt(ViewState["RegistrationId"]), 1);

                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataView dvStTemplate = new DataView();
                    if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"] == "StaticTemplate")
                    {
                        dvStTemplate = new DataView(objDs.Tables[0]);
                        dvStTemplate.RowFilter = "ISNULL(TemplateFieldId,0)<>0";
                    }
                    else
                    {
                        dvStTemplate = new DataView(objDs.Tables[0]);
                    }

                    DataView dvAllergy = new DataView(dvStTemplate.ToTable());
                    dvAllergy.RowFilter = "AllergyType='Drug'";
                    DataTable dtDrugAllergy = dvAllergy.ToTable();

                    if (dtDrugAllergy.Rows.Count > 0)
                    {
                        gvDrugAllergy.DataSource = dtDrugAllergy;
                        gvDrugAllergy.DataBind();

                        Cache.Insert("DrugAllergy" + Session["UserId"], dtDrugAllergy, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        tbpnlDrugAllergy.HeaderText = "Drug Allergies(" + common.myStr(dtDrugAllergy.Rows.Count) + ")";

                        bitNoDrugData = true;
                    }
                    else
                    {
                        // BindBlankChronicDiagnosisGrid();
                        gvDrugAllergy.DataSource = null;
                        gvDrugAllergy.DataBind();
                    }

                    dvAllergy.RowFilter = "";




                    dvAllergy.RowFilter = "AllergyType='CIMS'";
                    DataTable dtCIMSAllergy = dvAllergy.ToTable();
                    if (dtCIMSAllergy.Rows.Count > 0)
                    {
                        gvCIMSAllergy.DataSource = dtCIMSAllergy;
                        gvCIMSAllergy.DataBind();

                        Cache.Insert("CIMSAllergy" + Session["UserId"], dtCIMSAllergy, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        tbpnlCIMSAllergy.HeaderText = "CIMS Allergies(" + common.myStr(dtCIMSAllergy.Rows.Count) + ")";

                        bitNoCIMSData = true;
                    }
                    dvAllergy.RowFilter = "";





                    dvAllergy.RowFilter = "AllergyType='VIDAL'";
                    DataTable dtVIDALAllergy = dvAllergy.ToTable();
                    if (dtVIDALAllergy.Rows.Count > 0)
                    {
                        gvVIDALAllergy.DataSource = dtVIDALAllergy;
                        gvVIDALAllergy.DataBind();

                        Cache.Insert("VIDALAllergy" + Session["UserId"], dtVIDALAllergy, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        tbpnlVIDALAllergy.HeaderText = "VIDAL Allergies(" + common.myStr(dtVIDALAllergy.Rows.Count) + ")";

                        bitNoVIDALData = true;
                    }
                    dvAllergy.RowFilter = "";





                    dvAllergy.RowFilter = "AllergyType in('Food','Others')";
                    DataTable dtOtherAllergy = dvAllergy.ToTable();
                    if (dtOtherAllergy.Rows.Count > 0)
                    {
                        gvOtherAllergy.DataSource = dtOtherAllergy;
                        gvOtherAllergy.DataBind();

                        Cache.Insert("OtherAllergy" + Session["UserId"], dtOtherAllergy, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        tbpnlOtherAllergy.HeaderText = "Other Allergies(" + common.myStr(dtOtherAllergy.Rows.Count) + ")";

                        bitNoOtherData = true;
                    }
                    else
                    {
                        gvOtherAllergy.DataSource = null;
                        gvOtherAllergy.DataBind();
                        //BindBlankDiagnosisDetailGrid();
                    }
                    dvAllergy.RowFilter = "";


                    if (!IsPostBack)
                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/AuditCommonAccess";
                    APIRootClass.Security obj = new global::APIRootClass.Security();
                    obj.iHospID = Convert.ToInt16(Session["HospitalLocationID"]);
                    obj.iFacilityID = common.myInt(Session["FacilityID"]);
                    obj.iRegId = common.myInt(ViewState["RegistrationId"]);
                    obj.iEncountId = common.myInt(ViewState["EncounterId"]);
                    obj.iPageId = common.myInt(ViewState["PageId"]);
                    obj.iTemplateId = 0;
                    obj.iEncodedBy = common.myInt(Session["UserID"]);
                    obj.iEmpId = 0;
                    obj.iAuditStatus = "ACCESSED";
                    obj.chvIPAddres = common.myStr(Session["IPAddress"]);
                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    inputJson = (new JavaScriptSerializer()).Serialize(obj);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    // AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["PageId"]), 0, common.myInt(Session["UserID"]), 0, "ACCESSED", common.myStr(Session["IPAddress"]));
                    DisplayUserName(objDs.Tables[0].Rows[0]["EncodedBy"].ToString(), objDs.Tables[0].Rows[0]["EncodedDate"].ToString());
                }
                else
                {
                    ViewState["Record"] = 0;
                    //BindBlankChronicDiagnosisGrid();
                    //BindBlankDiagnosisDetailGrid();
                    gvDrugAllergy.DataSource = null;
                    gvDrugAllergy.DataBind();
                    gvOtherAllergy.DataSource = null;
                    gvOtherAllergy.DataBind();
                }

                if (objDs.Tables[1].Rows.Count > 0)
                {
                    if (objDs.Tables[1].Rows[0]["NoAllergies"].ToString().ToLower() == "false")
                        chkNKA.Checked = false;
                    else
                        chkNKA.Checked = true;
                    chkNKA_OnCheckedChanged(this, null);

                }
                else
                {
                    chkNKA.Checked = true;
                }

                #region
                if (bitNoDrugData || bitNoCIMSData || bitNoVIDALData || bitNoOtherData || chkNKA.Checked)
                {
                    if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
                    {
                        //objEMR = new BaseC.clsEMR(sConString);
                        //string mst = objEMR.EMRMUDLogMedicationAllergy(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationId"]),
                        //                 common.myInt(ViewState["EncounterId"]), common.myInt(Session["DoctorID"]),
                        //                 common.myInt(Session["UserID"]));
                    }
                }
                #endregion

                //DataSet objDsIA = objDas.getAllergies(common.myInt(ViewState["RegistrationId"]), 0);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientAllergies";
                APIRootClass.GetPatientAllergies objRoot1 = new global::APIRootClass.GetPatientAllergies();
                objRoot1.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                objRoot1.FromDate = "";
                objRoot1.ToDate = "";
                objRoot1.GroupingDate = "";
                objRoot1.Active = 0;

                inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                // sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet objDsIA = JsonConvert.DeserializeObject<DataSet>(sValue);

                gvDeactivateAllergy.DataSource = objDsIA.Tables[0];
                gvDeactivateAllergy.DataBind();
                tbpnlDeactivateAllergy.HeaderText = "De-Activate Allergies(" + common.myStr(objDsIA.Tables[0].Rows.Count) + ")";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            bNote = null;
            //AuditCA = null;
            //objDas = null;
        }
    }

    protected void rdoAllergyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (common.myInt(rdoAllergyType.SelectedValue) != common.myInt(Session["AllergyTypeSelected"]))
        {
            ddlBrand.Text = "";
            hdnItemId.Value = "0";
            ddlGeneric.Text = "";
            ddlBrand.Text = "";
            ddlGeneric.Items.Clear();
            ddlBrand.Items.Clear();
            ddlGeneric.SelectedValue = "0";
            ddlBrand.SelectedValue = "0";
            hdnGenericId.Value = "0";
            hdnItemId.Value = "0";
        }
        Session["AllergyTypeSelected"] = rdoAllergyType.SelectedValue;
        switch (common.myInt(rdoAllergyType.SelectedValue))
        {
            case 1://Drugs
                trInteractionPlausibility.Visible = true;
                ddlAllergyPlausibility.SelectedIndex = -1;
                //chkNKA.Visible = true;
                tdFormularyType.Visible = true;

                lblGeneric.Text = "Generic";
                lblBrand.Text = "Drug";

                break;
            case 2://CIMS
                tdFormularyType.Visible = false;

                trInteractionPlausibility.Visible = false;
                ddlAllergyPlausibility.SelectedIndex = -1;

                lblGeneric.Text = "Generic";
                lblBrand.Text = "Drug";

                // chkNKA.Visible = false;
                dtpAllergyDate.Enabled = true;
                chkIntolerance.Enabled = true;
                txtReaction.Enabled = true;
                txtRemarks.Enabled = true;
                btnAddtogrid.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = true;
                txtReaction.Text = "";
                txtRemarks.Text = "";

                break;
            case 3: //VIDAL
                tdFormularyType.Visible = false;

                trInteractionPlausibility.Visible = false;
                ddlAllergyPlausibility.SelectedIndex = -1;

                lblGeneric.Text = "Generic";
                lblBrand.Text = "Drug";

                // chkNKA.Visible = false;
                dtpAllergyDate.Enabled = true;
                chkIntolerance.Enabled = true;
                txtReaction.Enabled = true;
                txtRemarks.Enabled = true;
                btnAddtogrid.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = true;
                txtReaction.Text = "";
                txtRemarks.Text = "";

                break;
            default://Other
                //bhakti
                //ddlGeneric.Items.Clear();
                tdFormularyType.Visible = false;

                trInteractionPlausibility.Visible = false;

                lblGeneric.Text = "Allergy&nbsp;Type";
                lblBrand.Text = "Allergy";

                // chkNKA.Visible = false;
                dtpAllergyDate.Enabled = true;
                chkIntolerance.Enabled = true;
                txtReaction.Enabled = true;
                txtRemarks.Enabled = true;
                btnAddtogrid.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = true;
                txtReaction.Text = "";
                txtRemarks.Text = "";

                break;
        }

        chkNKA_OnCheckedChanged(this, null);
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ViewState["RegistrationId"]) == 0)
        {
            strmsg += "Registration not selected !";
            isSave = false;
        }

        if (common.myInt(rdoAllergyType.SelectedValue) == 1)
        {
            if (common.myInt(hdnGenericId.Value) == 0
                && common.myInt(hdnItemId.Value) == 0)
            {
                strmsg += "Generic or Drug not selected !";
                isSave = false;
            }
        }
        else
        {
            if (common.myInt(hdnItemId.Value) == 0)
            {
                hdnIsUnSavedData.Value = "0";
                strmsg += "Allergy not selected !";
                isSave = false;
            }
        }

        //if (common.myStr(txtReaction.Text).Trim().Length == 0)
        //{
        //    strmsg += "Reaction can't be blank !";
        //    isSave = false;
        //}

        if (common.myInt(ddlAllergyPlausibility.SelectedValue) == 0
            && trInteractionPlausibility.Visible)
        {
            strmsg += "Interaction severity not selected !";
            isSave = false;
        }

        if (rblShowNote.SelectedIndex == -1)
        {
            Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnAddtogrid_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            strXMLDrug = new StringBuilder();
            strXMLOther = new StringBuilder();
            coll = new ArrayList();

            string allergyDate = "";
            if (common.myStr(dtpAllergyDate.SelectedDate).Trim() != "")
            {
                allergyDate = Convert.ToDateTime(dtpAllergyDate.SelectedDate).ToString("yyyy/MM/dd");
            }

            if (common.myInt(rdoAllergyType.SelectedValue) == 1)
            {
                coll = new ArrayList();

                coll.Add(common.myInt(txteditno.Text));//Id int,             
                coll.Add("");//Drug_Id varchar(10),          
                coll.Add("0");//Drug_Syn_ID int,
                coll.Add(common.myInt(hdnGenericId.Value));//GenericId int,
                coll.Add(common.myInt(hdnItemId.Value));//ItemId int,
                coll.Add(bc.ParseQ(HttpUtility.HtmlDecode(common.myStr(txtReaction.Text))));//Reaction varchar(500),          
                coll.Add(chkIntolerance.Checked ? 1 : 0);//Intolerance bit,          
                coll.Add(allergyDate);//AllergyDate varchar(10),          
                coll.Add(bc.ParseQ(HttpUtility.HtmlDecode(common.myStr(txtRemarks.Text))));//Remarks varchar(500),  
                coll.Add(common.myInt(ddlAllergyPlausibility.SelectedItem.Value));//AllerySeverity int 
                if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"] == "StaticTemplate")
                {
                    coll.Add(common.myInt(Request.QueryString["TemplateFieldId"]));//TemplateFieldId int 
                }
                else
                {
                    coll.Add(0);
                }

                strXMLDrug.Append(common.setXmlTable(ref coll));
            }
            else
            {
                coll = new ArrayList();

                coll.Add(common.myInt(txteditno.Text));//Id int,             
                coll.Add(common.myInt(hdnItemId.Value));//AllergyId int,          
                coll.Add(bc.ParseQ(HttpUtility.HtmlDecode(common.myStr(txtReaction.Text))));//Reaction varchar(500),          
                coll.Add(chkIntolerance.Checked ? 1 : 0);//Intolerance bit,          
                coll.Add(allergyDate);//AllergyDate varchar(10),          
                coll.Add(bc.ParseQ(HttpUtility.HtmlDecode(common.myStr(txtRemarks.Text))));//Remarks varchar(500)--,  
                if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"] == "StaticTemplate")
                {
                    coll.Add(common.myInt(Request.QueryString["TemplateFieldId"]));//TemplateFieldId int 
                }
                else
                {
                    coll.Add(0);
                }

                strXMLOther.Append(common.setXmlTable(ref coll));
            }

            if (strXMLDrug.ToString() == "" && strXMLOther.ToString() == "")
            {
                lblMessage.Text = "Please select any allergy before saving !";
                hdnIsUnSavedData.Value = "0";
                return;
            }

            //objEMR = new BaseC.clsEMR(sConString);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientAllergy";
            APIRootClass.AllergySaveInputs objRoot = new global::APIRootClass.AllergySaveInputs();
            objRoot.HospId = common.myInt(Session["HospitalLocationID"]);
            objRoot.FacilityId = common.myInt(Session["FacilityID"]);
            objRoot.PageId = common.myInt(ViewState["PageId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
            objRoot.xmlDrugAllergyDetails = strXMLDrug.ToString();
            objRoot.xmlOtherAllergyDetails = strXMLOther.ToString();
            objRoot.EncodedBy = common.myInt(Session["UserId"]);
            objRoot.IsNKDA = chkNKA.Checked ? 1 : 0;
            objRoot.IsShowNote = common.myInt(rblShowNote.SelectedItem.Value);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string strMsg = client.UploadString(ServiceURL, inputJson);
            strMsg = JsonConvert.DeserializeObject<string>(strMsg);

            //string strMsg = objEMR.SavePatientAllergy(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), common.myInt(ViewState["RegistrationId"]),
            //                         common.myInt(ViewState["EncounterId"]), strXMLDrug.ToString(), strXMLOther.ToString(),
            //                         common.myInt(Session["UserId"]), chkNKA.Checked ? 1 : 0, common.myInt(rblShowNote.SelectedItem.Value));

            ///Tagging Static Template with Template Field
            if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
            {
                // kuldeep Hashtable hshOut = objEMR.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                //Convert.ToInt32(Request.QueryString["SectionId"].ToString()), Convert.ToInt32(Request.QueryString["TemplateFieldId"].ToString()),
                //Convert.ToInt32(Request.QueryString["StaticTemplateId"].ToString()), common.myInt(Session["UserId"]));
            }
            ///end



            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                //if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                //}
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg; //"Record(s) Has Been Saved...";

                ddlGeneric.Text = "";
                hdnGenericId.Value = "";
                hdnGenericName.Value = "";

                ddlBrand.Text = "";
                hdnItemId.Value = "";
                hdnItemName.Value = "";

                dtpAllergyDate.SelectedDate = null;
                chkIntolerance.Checked = false;
                txtReaction.Text = "";
                txtRemarks.Text = "";
                RetrievePatientAllergies();
                txtdaCancellationRemarks.Text = "";
                txtoaCancellationRemarks.Text = "";


                ddlAllergyPlausibility.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
            }
            else
            {
                lblMessage.Text = "";
            }
            hdnIsUnSavedData.Value = "0";
            dtpAllergyDate.SelectedDate = null;
            chkIntolerance.Checked = false;
            txtReaction.Text = "";
            txtRemarks.Text = "";
            ddlAllergyPlausibility.SelectedIndex = 0;
            btnAddtogrid.Text = "Add to List";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize

        finally
        {
            // objEMR = null;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
    }

    protected void gvDrugAllergy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            rdoAllergyType.SelectedValue = "1";
            rdoAllergyType_SelectedIndexChanged(this, null);
            int RowIndex = 0;
            RowIndex = common.myInt(gvDrugAllergy.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;
            HiddenField hdndaRowId = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaId");
            txteditno.Text = hdndaRowId.Value;
            HiddenField hdndaAllergyID = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaAllergyID");
            HiddenField hdndaGENERIC_ID = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaGENERIC_ID");
            Label lbldaAllergyDate = (Label)gvDrugAllergy.SelectedRow.FindControl("lbldaAllergyDate");
            HiddenField hdndaAllergyDate = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaAllergyDate");
            Label lbldaAllergyName = (Label)gvDrugAllergy.SelectedRow.FindControl("lbldaAllergyName");
            HiddenField hdndaTYPE = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaTYPE");
            Label lbldaReaction = (Label)gvDrugAllergy.SelectedRow.FindControl("lbldaReaction");
            HiddenField hdndaRemarks = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaRemarks");
            HiddenField hdnAllergySeverity = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdnAllergySeverity");
            HiddenField hdndaIntolerance = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndaIntolerance");
            HiddenField hdndsGeneric_Name = (HiddenField)gvDrugAllergy.SelectedRow.FindControl("hdndsGeneric_Name");



            ddlAllergyPlausibility.SelectedIndex = ddlAllergyPlausibility.Items.IndexOf(ddlAllergyPlausibility.Items.FindItemByValue(hdnAllergySeverity.Value.ToString()));
            ltrhdnCurrentRowId.Text = hdndaRowId.Value;

            lblhdn_GENERIC_ID.Text = hdndaGENERIC_ID.Value;
            lblhdn_ALLERGYID.Text = hdndaAllergyID.Value;
            hdnIsUnSavedData.Value = "1";
            hdnGenericId.Value = hdndaGENERIC_ID.Value;
            hdnItemId.Value = hdndaAllergyID.Value;

            ddlBrand.Text = lbldaAllergyName.Text;

            ddlGeneric.Text = hdndsGeneric_Name.Value;
            lblhdn_TYPE.Text = hdndaTYPE.Value;


            if (lbldaAllergyDate.Text != "")
            {
                dtpAllergyDate.SelectedDate = Convert.ToDateTime(hdndaAllergyDate.Value);
            }
            else
            {
                dtpAllergyDate.Clear();
            }
            chkIntolerance.Checked = common.myBool(hdndaIntolerance.Value);
            txtReaction.Text = lbldaReaction.Text;
            txtRemarks.Text = hdndaRemarks.Value;
            lblMessage.Text = "";
            btnAddtogrid.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvOtherAllergy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            rdoAllergyType.SelectedValue = "0";
            rdoAllergyType_SelectedIndexChanged(this, null);
            int RowIndex = 0;
            RowIndex = common.myInt(gvOtherAllergy.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;
            HiddenField hdnoaId = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaId");
            txteditno.Text = hdnoaId.Value;
            HiddenField hdnoaAllergyDate = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaAllergyDate");
            HiddenField hdnoaAllergyID = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaAllergyID");
            HiddenField hdnoaGENERIC_ID = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaGENERIC_ID");
            Label lbloaAllergyDate = (Label)gvOtherAllergy.SelectedRow.FindControl("lbloaAllergyDate");
            Label lbloaAllergyName = (Label)gvOtherAllergy.SelectedRow.FindControl("lbloaAllergyName");
            HiddenField hdnoaTYPE = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaTYPE");
            CheckBox chkoaIntolerance = (CheckBox)gvOtherAllergy.SelectedRow.FindControl("chkoaIntolerance");
            Label lbloaReaction = (Label)gvOtherAllergy.SelectedRow.FindControl("lbloaReaction");
            HiddenField hdnoaRemarks = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaRemarks");
            HiddenField hdnoaIntolerance = (HiddenField)gvOtherAllergy.SelectedRow.FindControl("hdnoaIntolerance");
            //ddlAllergyPlausibility.SelectedIndex = ddlAllergyPlausibility.Items.IndexOf(ddlAllergyPlausibility.Items.FindByValue(lblAllergySeverity.Text.ToString()));

            ltrhdnCurrentRowId.Text = hdnoaId.Value;
            lblhdn_GENERIC_ID.Text = hdnoaGENERIC_ID.Value;
            lblhdn_ALLERGYID.Text = hdnoaAllergyID.Value;
            lblhdn_TYPE.Text = hdnoaTYPE.Value;
            ddlGeneric.Text = hdnoaTYPE.Value;
            hdnItemId.Value = hdnoaAllergyID.Value;
            hdnIsUnSavedData.Value = "1";
            if (lbloaAllergyDate.Text.Trim() != "")
            {
                dtpAllergyDate.SelectedDate = Convert.ToDateTime(hdnoaAllergyDate.Value);
            }
            else
            {
                dtpAllergyDate.Clear();
            }
            chkIntolerance.Checked = common.myBool(hdnoaIntolerance.Value);
            txtReaction.Text = lbloaReaction.Text;
            txtRemarks.Text = hdnoaRemarks.Value;
            ddlBrand.Text = lbloaAllergyName.Text;
            lblMessage.Text = "";
            btnAddtogrid.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvOtherAllergy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[6].Visible = false;
        }
    }
    protected void gvDrugAllergy_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
    protected void gvDrugAllergy_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        // DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            //  objemrallrgy = new BaseC.EMRAllergy(sConString);

            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);


                int intId = common.myInt(e.CommandArgument);

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;
                }

                if (intId == 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["DrugAllergy" + Session["UserId"]];
                    dt.Rows.RemoveAt(row.RowIndex);
                    gvDrugAllergy.DataSource = dt;
                    gvDrugAllergy.DataBind();
                    tbcAllergy.ActiveTabIndex = 0;
                    tbpnlDrugAllergy.HeaderText = "Drug Allergies(" + common.myStr(dt.Rows.Count) + ")";
                    txtdaCancellationRemarks.Text = "";
                    dtpAllergyDate.SelectedDate = null;
                    chkIntolerance.Checked = false;
                    txtReaction.Text = "";
                    txtRemarks.Text = "";
                    txtdaCancellationRemarks.Text = "";
                    lblMessage.Text = "Records DeActivated";
                }
                else
                {
                    if (common.myStr(txtdaCancellationRemarks.Text).Trim().Length == 0)
                    {
                        lblMessage.Text = "De-activation remarks can't be blank !";
                        Page.SetFocus(txtdaCancellationRemarks);

                        return;
                    }

                    //objEMR = new BaseC.clsEMR(sConString);

                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/deActivatePatientAllergy";
                    APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
                    objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                    objRoot.PageId = common.myInt(ViewState["PageId"]);
                    objRoot.Flag = 1;
                    objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
                    objRoot.Id = intId;
                    objRoot.CancelRemarks = common.myStr(txtdaCancellationRemarks.Text);
                    objRoot.EncodedBy = common.myInt(Session["UserId"]);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string strMsg = client.UploadString(ServiceURL, inputJson);
                    strMsg = JsonConvert.DeserializeObject<string>(strMsg);

                    //string strMsg = objEMR.deActivatePatientAllergy(common.myInt(Session["HospitalLocationId"]),
                    //                common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]),
                    //                1, common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                    //                intId, common.myStr(txtdaCancellationRemarks.Text),
                    //                common.myInt(Session["UserId"]));
                    //sikandar 
                    //string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value +
                    //            " FROM EMRPatientForms epf " +
                    //            " INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId AND epfd.PageId = " + ViewState["PageId"].ToString() +
                    //            " WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) +
                    //            " AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
                    //            " AND epf.Active = 1 ";

                    //objDl.ExecuteNonQuery(CommandType.Text, sQuery);

                    //code optimization-instead of above code query, below produre containing function will return the desired ouptut however this delete command is not firing  : Sikandar
                    ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/updateshownote";
                    APIRootClass.Allergy Allergy = new global::APIRootClass.Allergy();
                    Allergy.showNotesVal = rblShowNote.SelectedItem.Value.ToString();
                    Allergy.PageId = common.myInt(ViewState["PageId"]);
                    Allergy.EncounterId = common.myInt(ViewState["EncounterId"]);
                    Allergy.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    inputJson = (new JavaScriptSerializer()).Serialize(Allergy);
                    string strmsg = client.UploadString(ServiceURL, inputJson);
                    strmsg = JsonConvert.DeserializeObject<string>(strmsg);


                    // string strmsg = objemrallrgy.updateshownote(rblShowNote.SelectedItem.Value.ToString(), common.myInt(ViewState["PageId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));


                    lblMessage.Text = strMsg;
                    if (strMsg.Equals(string.Empty) || strMsg.ToUpper().ToString().Contains("DEACTIVATED"))
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                        DataTable dt = new DataTable();
                        dt = (DataTable)Cache["DrugAllergy" + Session["UserId"]];
                        dt.Rows.RemoveAt(row.RowIndex);
                        gvDrugAllergy.DataSource = dt;
                        gvDrugAllergy.DataBind();
                        tbcAllergy.ActiveTabIndex = 0;
                        tbpnlDrugAllergy.HeaderText = "Drug Allergies(" + common.myStr(dt.Rows.Count) + ")";
                        txtdaCancellationRemarks.Text = "";
                        dtpAllergyDate.SelectedDate = null;
                        chkIntolerance.Checked = false;
                        txtReaction.Text = "";
                        txtRemarks.Text = "";
                        txtdaCancellationRemarks.Text = "";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    }

                }
                GetDeActivityAllergies();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            //objDl = null;
            // objemrallrgy = null;
        }
    }

    protected void gvOtherAllergy_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        // DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            // objemrallrgy = new BaseC.EMRAllergy(sConString);

            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);


                int intId = common.myInt(e.CommandArgument);

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;
                }

                if (intId == 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["OtherAllergy" + Session["UserId"]];
                    dt.Rows.RemoveAt(row.RowIndex);
                    gvOtherAllergy.DataSource = dt;
                    gvOtherAllergy.DataBind();
                    tbcAllergy.ActiveTabIndex = 3;
                    tbpnlOtherAllergy.HeaderText = "Other Allergies(" + common.myStr(dt.Rows.Count) + ")";
                    txtoaCancellationRemarks.Text = "";
                    dtpAllergyDate.SelectedDate = null;
                    chkIntolerance.Checked = false;
                    txtReaction.Text = "";
                    txtRemarks.Text = "";
                    txtdaCancellationRemarks.Text = "";
                    lblMessage.Text = "Records DeActivated";
                }
                else
                {
                    if (common.myStr(txtoaCancellationRemarks.Text).Trim().Length == 0)
                    {
                        lblMessage.Text = "De-activation remarks can't be blank !";
                        Page.SetFocus(txtoaCancellationRemarks);

                        return;
                    }

                    // objEMR = new BaseC.clsEMR(sConString);

                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/deActivatePatientAllergy";
                    APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
                    objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                    objRoot.PageId = common.myInt(ViewState["PageId"]);
                    objRoot.Flag = 2;
                    objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
                    objRoot.Id = intId;
                    objRoot.CancelRemarks = common.myStr(txtoaCancellationRemarks.Text);
                    objRoot.EncodedBy = common.myInt(Session["UserId"]);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string strMsg = client.UploadString(ServiceURL, inputJson);
                    strMsg = JsonConvert.DeserializeObject<string>(strMsg);
                    lblMessage.Text = strMsg;
                    //string strMsg = objEMR.deActivatePatientAllergy(
                    //    common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                    //                   common.myInt(ViewState["PageId"]), 2, common.myInt(ViewState["RegistrationId"]),
                    //                   common.myInt(ViewState["EncounterId"]), intId,
                    //                   common.myStr(txtoaCancellationRemarks.Text), common.myInt(Session["UserId"]));

                    //if (strMsg.ToUpper().ToString().Contains("DEACTIVATED"))
                    //{
                        //sikandar
                        //string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value +
                        //" FROM EMRPatientForms epf " +
                        //" INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId " +
                        //" AND epfd.PageId = " + ViewState["PageId"].ToString() + "  WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) +
                        //" AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
                        //" AND epf.Active = 1 ";

                        //code optimization-instead of above code query, below produre containing function will return the desired ouptut however this delete command is not firing  : Sikandar

                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/updateshownote";
                        APIRootClass.Allergy Allergy = new global::APIRootClass.Allergy();
                        Allergy.showNotesVal = rblShowNote.SelectedItem.Value.ToString();
                        Allergy.PageId = common.myInt(ViewState["PageId"]);
                        Allergy.EncounterId = common.myInt(ViewState["EncounterId"]);
                        Allergy.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        inputJson = (new JavaScriptSerializer()).Serialize(Allergy);
                        string strmsg = client.UploadString(ServiceURL, inputJson);
                        strmsg = JsonConvert.DeserializeObject<string>(strmsg);

                        //string strmsg = objemrallrgy.updateshownote(rblShowNote.SelectedItem.Value.ToString(), common.myInt(ViewState["PageId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));

                        //objDl.ExecuteNonQuery(CommandType.Text, sQuery);

                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                        DataTable dt = new DataTable();
                        dt = (DataTable)Cache["OtherAllergy" + Session["UserId"]];
                        dt.Rows.RemoveAt(row.RowIndex);
                        gvOtherAllergy.DataSource = dt;
                        gvOtherAllergy.DataBind();
                        //tbcAllergy.ActiveTabIndex = 3;

                        txtoaCancellationRemarks.Text = "";
                        dtpAllergyDate.SelectedDate = null;
                        chkIntolerance.Checked = false;
                        txtReaction.Text = "";
                        txtRemarks.Text = "";
                        txtoaCancellationRemarks.Text = "";
                        tbpnlOtherAllergy.HeaderText = "Other Allergies(" + common.myStr(dt.Rows.Count) + ")";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    //}
                    //else
                    //{
                    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //}

                }
                GetDeActivityAllergies();
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
            // objDl = null;
            // objemrallrgy = null;
        }
    }

    protected void chkNKA_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkNKA.Checked == true)
        {
            if (gvDrugAllergy.Rows.Count > 0 || gvCIMSAllergy.Rows.Count > 0 || gvVIDALAllergy.Rows.Count > 0 || gvOtherAllergy.Rows.Count > 0)
            {
                lblMessage.Text = "Remove Allergies Before Checking - Not Known Allergies.";
                chkNKA.Checked = false;
            }
            else
            {
                rdoAllergyType.Enabled = false;

                dtpAllergyDate.Enabled = false;
                chkIntolerance.Enabled = false;
                txtReaction.Enabled = false;
                txtRemarks.Enabled = false;
                btnAddtogrid.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = true;

                lblMessage.Text = "";
                dtpAllergyDate.Enabled = false;
                chkIntolerance.Enabled = false;
                txtReaction.Enabled = false;
                txtRemarks.Enabled = false;
                btnAddtogrid.Enabled = false;
                btnNew.Enabled = false;
                btnSave.Enabled = false;

                dtpAllergyDate.SelectedDate = null;
                chkIntolerance.Checked = false;
                txtReaction.Text = "";
                txtRemarks.Text = "";
            }
        }
        else
        {
            rdoAllergyType.Enabled = true;
            dtpAllergyDate.Enabled = true;
            chkIntolerance.Enabled = true;
            txtReaction.Enabled = true;
            txtRemarks.Enabled = true;
            btnAddtogrid.Enabled = true;
            btnNew.Enabled = true;
            dtpAllergyDate.Enabled = true;
            chkIntolerance.Enabled = true;
            txtReaction.Enabled = true;
            txtRemarks.Enabled = true;
            btnAddtogrid.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            SetPermission();
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        // DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            // objemrallrgy = new BaseC.EMRAllergy(sConString);

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            Hashtable hsinput = new Hashtable();


            if (rblShowNote.SelectedIndex == -1)
            {
                Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                return;

            }

            if (chkNKA.Checked == true)
            {
                hsinput.Add("@bitNKDA", 1);
                hsinput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));


                string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/Inline";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = "UPDATE Registration SET NoAllergies = 1 WHERE ID = '" + common.myInt(ViewState["RegistrationId"]) + "' ";
                WebClient client1 = new WebClient();
                client1.Headers["Content-type"] = "application/json";
                client1.Encoding = Encoding.UTF8;
                string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot);
                string sqlstr = client1.UploadString(ServiceURL1, inputJson1);
                sqlstr = JsonConvert.DeserializeObject<string>(sqlstr);

                //string sqlstr = "UPDATE Registration SET NoAllergies = @bitNKDA WHERE ID = @intRegistrationId ";

                //dl.ExecuteNonQuery(CommandType.Text, sqlstr, hsinput);

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Update Not Known Allergies succeeded !";
            }
            else
            {
                hsinput.Add("@bitNKDA", 0);
                hsinput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));

                string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/Inline";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = "UPDATE Registration SET NoAllergies = 0 WHERE ID = '" + common.myInt(ViewState["RegistrationId"]) + "' ";
                WebClient client1 = new WebClient();
                client1.Headers["Content-type"] = "application/json";
                client1.Encoding = Encoding.UTF8;
                string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot);
                string sqlstr = client1.UploadString(ServiceURL1, inputJson1);
                sqlstr = JsonConvert.DeserializeObject<string>(sqlstr);

                //string sqlstr = "UPDATE Registration SET NoAllergies = @bitNKDA WHERE ID = @intRegistrationId ";

                //dl.ExecuteNonQuery(CommandType.Text, sqlstr, hsinput);

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Removed patient from Not Known Allergies ! ";
            }

            //string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value +
            //        " FROM EMRPatientForms epf " +
            //        " INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId AND epfd.PageId = " + ViewState["PageId"].ToString() +
            //        " WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) +
            //        " AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
            //        " AND epf.Active = 1 ";

            //dl.ExecuteNonQuery(CommandType.Text, sQuery);


            //code optimization-instead of above code query, below produre containing function will return the desired ouptut however this delete command is not firing  : Sikandar
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/updateshownote";
            APIRootClass.Allergy Allergy = new global::APIRootClass.Allergy();
            Allergy.showNotesVal = rblShowNote.SelectedItem.Value.ToString();
            Allergy.PageId = common.myInt(ViewState["PageId"]);
            Allergy.EncounterId = common.myInt(ViewState["EncounterId"]);
            Allergy.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(Allergy);
            string strmsg = client.UploadString(ServiceURL, inputJson);
            strmsg = JsonConvert.DeserializeObject<string>(strmsg);

            // string strmsg = objemrallrgy.updateshownote(rblShowNote.SelectedItem.Value.ToString(), common.myInt(ViewState["PageId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            //dl = null;
            //objemrallrgy = null;
        }
    }

    private void DisplayUserName(string userID, string encodedDate)
    {
        // BaseC.Patient bC = new BaseC.Patient(sConString);
        try
        {
            // DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder sb = new StringBuilder();
            sb.Append("<br/>");
            //Hashtable hshtable = new Hashtable();
            StringBuilder sbDisplayName = new StringBuilder();
            //hshtable.Add("@intTemplateId", pageID);
            //string strDisplayUserName = "select DisplayUserName from EMRTemplateStatic where pageid=@intTemplateId";
            //DataSet dsDisplayName = dl.FillDataSet(CommandType.Text, strDisplayUserName, hshtable);
            //if (dsDisplayName.Tables[0].Rows.Count > 0)
            //{
            // if (dsDisplayName.Tables[0].Rows[0]["DisplayUserName"].ToString() == "True")
            //{

            Hashtable hshUser = new Hashtable();
            hshUser.Add("@UserID", userID);
            hshUser.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));

            //string strUser = "Select ISNULL(FirstName,'') + ISNULL(MiddleName,'') + ISNULL(LastName,'') AS EmployeeName  FROM Employee em INNER JOIN Users us ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
            //DataSet dsUser = dl.FillDataSet(CommandType.Text, strUser, hshUser);


            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/PreAuthPatientDtl";
            APIRootClass.PreAuth PreAuth = new global::APIRootClass.PreAuth();
            PreAuth.Query = "Select ISNULL(FirstName,'') + ISNULL(MiddleName,'') + ISNULL(LastName,'') AS EmployeeName  FROM Employee em INNER JOIN Users us ON em.ID=us.EmpID WHERE us.ID= '" + userID + "' and em.HospitalLocationId='" + common.myInt(Session["HospitalLocationID"]) + "'";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(PreAuth);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet dsUser = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (dsUser.Tables[0].Rows.Count > 0)
            {
                //lblUserName.Text = "Entered and Verified by " + dsUser.Tables[0].Rows[0]["EmployeeName"].ToString() + " " + Convert.ToString(Convert.ToDateTime(encodedDate).Date.ToString("MMMM dd yyyy"));
                lblUserName.Text = "Entered and Verified by " + dsUser.Tables[0].Rows[0]["EmployeeName"].ToString() + " " + common.myDate(encodedDate).ToString("MMMM dd yyyy");
            }

            //}
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            bc = null;
        }
    }

    protected void lnkAllergyMaster_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Allergy/AllergyMaster.aspx?From=POPUP";
        RadWindow1.Height = 600;
        RadWindow1.Width = 990;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void rdoFormularyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ddlBrand.Text = "";
        hdnItemId.Value = "0";
    }

    protected void ddlGeneric_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;

        DataTable data = GetGenericData(e.Text);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            ddl.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["GenericName"]), common.myStr(data.Rows[i]["GenericId"])));
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }


    protected void ddlBrand_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (common.myInt(rdoAllergyType.SelectedValue) == 1)
        {
            if (e.Text == "" || e.Text.Length < 2)
            {
                return;
            }
        }

        RadComboBox ddl = sender as RadComboBox;

        int GenericId = 0;

        if (common.myStr(ddlGeneric.Text).Length > 0)
        {
            string selectedValue = common.myStr(e.Context["GenericId"]);
            if (common.myInt(selectedValue) > 0)
            {
                GenericId = common.myInt(selectedValue);
            }
        }

        //if (common.myInt(hdnStoreId.Value) == 0)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Store not selected !";

        //    return;
        //}

        DataTable data = new DataTable();
        data = GetBrandData(e.Text, GenericId);

        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["ItemName"];
            item.Value = common.myStr(data.Rows[i]["ItemId"]);
            //item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));

            this.ddlBrand.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }

    private DataTable GetGenericData(string text)
    {

        DataSet dsSearch = new DataSet();

        switch (common.myInt(rdoAllergyType.SelectedValue))
        {
            case 1://Drugs
                   // objPharmacy = new BaseC.clsPharmacy(sConString);
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetGenericDetails";
                APIRootClass.GenericDetail objRoot = new global::APIRootClass.GenericDetail();
                objRoot.GenericId = 0;
                objRoot.GenericName = text;
                objRoot.Active = 1;
                objRoot.HospId = common.myInt(Session["HospitalLocationID"]);
                objRoot.EncodedBy = common.myInt(Session["UserId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);

                // dsSearch = objPharmacy.GetGenericDetails(0, text, 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

                break;
            case 2://CIMS
                   //objEMR = new BaseC.clsEMR(sConString);
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getAllergyTypeMaster";
                APIRootClass.Allergy objRoot1 = new global::APIRootClass.Allergy();
                objRoot1.HospId = common.myInt(Session["HospitalLocationId"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);
                //dsSearch = objEMR.getAllergyTypeMaster(common.myInt(Session["HospitalLocationID"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    DataView DV = dsSearch.Tables[0].DefaultView;
                    DV.RowFilter = "GenericName='CIMS'";

                    dsSearch.Tables.RemoveAt(0);

                    dsSearch.Tables.Add(DV.ToTable());
                }

                break;

            case 3://VIDAL

                //objEMR = new BaseC.clsEMR(sConString);
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getAllergyTypeMaster";
                APIRootClass.Allergy objRoot3 = new global::APIRootClass.Allergy();
                objRoot3.HospId = common.myInt(Session["HospitalLocationId"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot3);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);


                //dsSearch = objEMR.getAllergyTypeMaster(common.myInt(Session["HospitalLocationID"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    DataView DV = dsSearch.Tables[0].DefaultView;
                    DV.RowFilter = "GenericName='VIDAL'";

                    dsSearch.Tables.RemoveAt(0);

                    dsSearch.Tables.Add(DV.ToTable());
                }

                break;

            default://Other
                    // objEMR = new BaseC.clsEMR(sConString);
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getAllergyTypeMaster";
                APIRootClass.Allergy objRoot2 = new global::APIRootClass.Allergy();
                objRoot2.HospId = common.myInt(Session["HospitalLocationId"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);
                //dsSearch = objEMR.getAllergyTypeMaster(common.myInt(Session["HospitalLocationID"]));

                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    DataView DV = dsSearch.Tables[0].DefaultView;
                    DV.RowFilter = "GenericName='Food' OR GenericName='Others'";

                    dsSearch.Tables.RemoveAt(0);

                    dsSearch.Tables.Add(DV.ToTable());
                }

                break;
        }

        return dsSearch.Tables[0];
    }

    private DataTable GetBrandData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();

        //objPharmacy = new BaseC.clsPharmacy(sConString);

        int StoreId = 0; //common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
        int ItemId = 0;

        int itemBrandId = 0;
        int WithStockOnly = 0;

        if (common.myDbl(ViewState["QtyBal"]) > 0
               && common.myInt(Request.QueryString["ItemId"]) > 0)
        {
            ItemId = common.myInt(ViewState["ItemId"]);
        }

        DataView DV = new DataView();

        if (common.myInt(rdoAllergyType.SelectedValue) == 1)
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getItemsWithStock";
            APIRootClass.Pharmacy objRoot = new global::APIRootClass.Pharmacy();
            objRoot.HospId = common.myInt(Session["HospitalLocationID"]);
            objRoot.StoreId = StoreId;
            objRoot.ItemId = ItemId;
            objRoot.ItemBrandId = itemBrandId;
            objRoot.GenericId = GenericId;
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.SupplierId = 0;
            objRoot.ItemName = text.Replace("'", "''");
            objRoot.WithStockOnly = WithStockOnly;
            objRoot.NHISDrugsList = getFormularyType();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);


            //dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, itemBrandId, GenericId,
            //                                common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
            //                                text.Replace("'", "''"), WithStockOnly, getFormularyType());

            DV = dsSearch.Tables[0].DefaultView;

        }
        else
        {
            //objEMR = new BaseC.clsEMR(sConString);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getAllergyMaster";
            APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
            objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
            objRoot.ItemName = common.myStr(text).Replace("'", "''");
            objRoot.AllergyTypeId = GenericId;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);


            //dsSearch = objEMR.getAllergyMaster(common.myInt(Session["HospitalLocationID"]),
            //                        common.myStr(text).Replace("'", "''"), GenericId);

            DV = dsSearch.Tables[0].DefaultView;



        }

        if (DV.ToTable().Rows.Count > 0)
        {
            dt = DV.ToTable();
            //if (common.myInt(rdoAllergyType.SelectedValue) == 2)
            //{
            //    DV.RowFilter = "AllergyType='CIMS'";
            //}
            //else if (common.myInt(rdoAllergyType.SelectedValue) == 3)
            //{
            //    DV.RowFilter = "AllergyType='VIDAL'";
            //}
            if (common.myInt(rdoAllergyType.SelectedValue) == 0)
            {
                DV.RowFilter = "AllergyType IN('Food','Others')";
            }
        }

        return dt;
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        //objPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            int ItemId = common.myInt(hdnItemId.Value);

            clearItemDetails();
            hdnIsUnSavedData.Value = "1";
            //ddlFormulation.Enabled = true;
            //ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;

            if (ItemId > 0)
            {
                hdnGenericId.Value = "0";

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getItemMaster";
                APIRootClass.Pharmacy objRoot = new global::APIRootClass.Pharmacy();
                objRoot.ItemId = ItemId;
                objRoot.ItemNo = "";
                objRoot.ItemsearchName = "";
                objRoot.active = 1;
                objRoot.HospId = common.myInt(Session["HospitalLocationID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);



                // DataSet ds = objPharmacy.getItemMaster(ItemId, "", "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];

                    hdnGenericId.Value = common.myInt(DR["GenericId"]).ToString();
                    //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                    //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
                    ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
                    //ddlFormulation.Enabled = false;
                    //ddlRoute.Enabled = false;
                    //ddlStrength.Enabled = false;

                    //txtDose.Focus();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize

        finally
        {
            // objPharmacy = null;
        }

    }

    protected void btnGetInfoGeneric_Click(object sender, EventArgs e)
    {
        //ddlFormulation.Enabled = true;
        //ddlRoute.Enabled = true;
        //ddlStrength.Enabled = true;

        ddlBrand.Focus();
    }

    private string getFormularyType()
    {
        string FormularyType = "H";
        switch (common.myInt(rdoFormularyType.SelectedValue))
        {
            case 0:
                FormularyType = "H";
                break;
            case 1:
                FormularyType = "N";
                break;
        }

        return FormularyType;
    }

    private void clearItemDetails()
    {
        txtdaCancellationRemarks.Text = "";
        dtpAllergyDate.SelectedDate = null;
        chkIntolerance.Checked = false;
        txtReaction.Text = "";
        txtRemarks.Text = "";
        txtdaCancellationRemarks.Text = "";
    }

    protected void gvAllergyList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            //Literal lbl_ALLERGYID = ((Literal)(gvAllergyList.SelectedRow.FindControl("lbl_ALLERGYID")));
            //Literal lbl_GENERIC_ID = ((Literal)(gvAllergyList.SelectedRow.FindControl("lbl_GENERIC_ID")));
            //Label lblAllergyName = ((Label)(gvAllergyList.SelectedRow.FindControl("lbl_AllergyName")));
            //Literal lblTYPE = ((Literal)(gvAllergyList.SelectedRow.FindControl("lbl_Type")));

            //lblhdn_GENERIC_ID.Text = lbl_GENERIC_ID.Text;
            //lblhdn_ALLERGYID.Text = lbl_ALLERGYID.Text;
            //lblhdn_TYPE.Text = lblTYPE.Text;
            //lblhdn_AllergyName.Text = lblAllergyName.Text;
            //if (gvDrugAllergy.SelectedIndex != -1)
            //{
            //    gvDrugAllergy.SelectedIndex = -1;
            //}
            //if (gvOtherAllergy.SelectedIndex != -1)
            //{
            //    gvOtherAllergy.SelectedIndex = -1;
            //}
            //btnAddtogrid.Text = "Add to Grid";
            //txteditno.Text = "0";
            //lblMessage.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAllergyList_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='';";
        //    e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvAllergyList, "Select$" + e.Row.RowIndex);
        //}
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    if (rdoAllergyType.SelectedValue == "0")
        //    {
        //        e.Row.Cells[2].Text = "Master List Of Other Allergies";
        //    }
        //    else
        //    {
        //        e.Row.Cells[2].Text = "Master List of Drug Allergies";
        //    }
        //}
    }

    //protected void lnkPatientDashboard_Click(Object sender, EventArgs e)
    //{
    //    Response.Redirect("~/EMR/Dashboard/PatientDashboard.aspx", false);
    //}

    protected void lnkDemographics_Click(Object sender, EventArgs e)
    {
        Response.Redirect("/PRegistration/Demographics.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&mode=E&Mpg=P1", false);
    }

    protected void lnkPatientRelation_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C91", false);
    }

    protected void lnkOtherDetails_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C89", false);
    }

    protected void lnkResponsibleParty_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C94", false);
    }

    protected void lnkPayment_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C90", false);
    }

    protected void lnkAttachDocument_OnClick(object sender, EventArgs e)
    {
        //Response.Redirect("/emr/AttachDocument.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C119", false);
        Response.Redirect("/emr/AttachDocumentFTP.aspx?RNo=" + common.myInt(ViewState["RegistrationNo"]) + "&Mpg=C119", false);
    }

    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(Session["RegistrationId"]);
            RadWindow1.Height = 600;
            RadWindow1.Width = 600;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

    protected void gvVIDALAllergy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            rdoAllergyType.SelectedValue = "3";
            rdoAllergyType_SelectedIndexChanged(this, null);
            int RowIndex = 0;
            RowIndex = common.myInt(gvVIDALAllergy.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;
            HiddenField hdndaRowId = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaId");
            txteditno.Text = hdndaRowId.Value;
            HiddenField hdndaAllergyID = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaAllergyID");
            HiddenField hdndaGENERIC_ID = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaGENERIC_ID");
            Label lbldaAllergyDate = (Label)gvVIDALAllergy.SelectedRow.FindControl("lbldaAllergyDate");
            HiddenField hdndaAllergyDate = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaAllergyDate");
            Label lbldaAllergyName = (Label)gvVIDALAllergy.SelectedRow.FindControl("lbldaAllergyName");
            HiddenField hdndaTYPE = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaTYPE");
            Label lbldaReaction = (Label)gvVIDALAllergy.SelectedRow.FindControl("lbldaReaction");
            HiddenField hdndaRemarks = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaRemarks");
            HiddenField hdnAllergySeverity = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdnAllergySeverity");
            HiddenField hdndaIntolerance = (HiddenField)gvVIDALAllergy.SelectedRow.FindControl("hdndaIntolerance");

            ddlAllergyPlausibility.SelectedIndex = ddlAllergyPlausibility.Items.IndexOf(ddlAllergyPlausibility.Items.FindItemByValue(hdnAllergySeverity.Value.ToString()));
            ltrhdnCurrentRowId.Text = hdndaRowId.Value;

            lblhdn_GENERIC_ID.Text = hdndaGENERIC_ID.Value;
            lblhdn_ALLERGYID.Text = hdndaAllergyID.Value;
            hdnIsUnSavedData.Value = "1";
            hdnGenericId.Value = hdndaGENERIC_ID.Value;
            hdnItemId.Value = hdndaAllergyID.Value;

            ddlBrand.Text = lbldaAllergyName.Text;

            lblhdn_TYPE.Text = hdndaTYPE.Value;
            if (lbldaAllergyDate.Text != "")
            {
                dtpAllergyDate.SelectedDate = Convert.ToDateTime(hdndaAllergyDate.Value);
            }
            else
            {
                dtpAllergyDate.Clear();
            }
            chkIntolerance.Checked = common.myBool(hdndaIntolerance.Value);
            txtReaction.Text = lbldaReaction.Text;
            txtRemarks.Text = hdndaRemarks.Value;
            lblMessage.Text = "";
            btnAddtogrid.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVIDALAllergy_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            // objemrallrgy = new BaseC.EMRAllergy(sConString);

            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);


                int intId = common.myInt(e.CommandArgument);

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;
                }

                if (intId == 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["VIDALAllergy" + Session["UserId"]];
                    dt.Rows.RemoveAt(row.RowIndex);
                    gvVIDALAllergy.DataSource = dt;
                    gvVIDALAllergy.DataBind();
                    tbcAllergy.ActiveTabIndex = 2;
                    tbpnlVIDALAllergy.HeaderText = "VIDAL Allergies(" + common.myStr(dt.Rows.Count) + ")";
                    txtVIDALCancellationRemarks.Text = "";
                    dtpAllergyDate.SelectedDate = null;
                    chkIntolerance.Checked = false;
                    txtReaction.Text = "";
                    txtRemarks.Text = "";
                    lblMessage.Text = "Records DeActivated";
                }
                else
                {
                    if (common.myStr(txtVIDALCancellationRemarks.Text).Trim().Length == 0)
                    {
                        lblMessage.Text = "De-activation remarks can't be blank !";
                        Page.SetFocus(txtVIDALCancellationRemarks);

                        return;
                    }

                    // objEMR = new BaseC.clsEMR(sConString);
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/deActivatePatientAllergy";
                    APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
                    objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                    objRoot.PageId = common.myInt(ViewState["PageId"]);
                    objRoot.Flag = 2;
                    objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
                    objRoot.Id = intId;
                    objRoot.CancelRemarks = common.myStr(txtdaCancellationRemarks.Text);
                    objRoot.EncodedBy = common.myInt(Session["UserId"]);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string strMsg = client.UploadString(ServiceURL, inputJson);
                    strMsg = JsonConvert.DeserializeObject<string>(strMsg);

                    //string strMsg = objEMR.deActivatePatientAllergy(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                    //                   common.myInt(ViewState["PageId"]), 2, common.myInt(ViewState["RegistrationId"]),
                    //                   common.myInt(ViewState["EncounterId"]), intId,
                    //                   common.myStr(txtVIDALCancellationRemarks.Text), common.myInt(Session["UserId"]));

                    if (strMsg == "")
                    {
                        //string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value +
                        //" FROM EMRPatientForms epf " +
                        //" INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId " +
                        //" AND epfd.PageId = " + ViewState["PageId"].ToString() + "  WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) +
                        //" AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
                        //" AND epf.Active = 1 ";

                        //code optimization-instead of above code query, below produre containing function will return the desired ouptut however this delete command is not firing  : Sikandar
                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/updateshownote";
                        APIRootClass.Allergy Allergy = new global::APIRootClass.Allergy();
                        Allergy.showNotesVal = rblShowNote.SelectedItem.Value.ToString();
                        Allergy.PageId = common.myInt(ViewState["PageId"]);
                        Allergy.EncounterId = common.myInt(ViewState["EncounterId"]);
                        Allergy.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        inputJson = (new JavaScriptSerializer()).Serialize(Allergy);
                        string strmsg = client.UploadString(ServiceURL, inputJson);
                        strmsg = JsonConvert.DeserializeObject<string>(strmsg);


                        //string strmsg = objemrallrgy.updateshownote(rblShowNote.SelectedItem.Value.ToString(), common.myInt(ViewState["PageId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));
                        //objDl.ExecuteNonQuery(CommandType.Text, sQuery);

                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                        DataTable dt = new DataTable();
                        dt = (DataTable)Cache["VIDALAllergy" + Session["UserId"]];
                        dt.Rows.RemoveAt(row.RowIndex);
                        gvVIDALAllergy.DataSource = dt;
                        gvVIDALAllergy.DataBind();
                        tbcAllergy.ActiveTabIndex = 2;
                        tbpnlVIDALAllergy.HeaderText = "VIDAL Allergies(" + common.myStr(dt.Rows.Count) + ")";
                        txtVIDALCancellationRemarks.Text = "";
                        dtpAllergyDate.SelectedDate = null;
                        chkIntolerance.Checked = false;
                        txtReaction.Text = "";
                        txtRemarks.Text = "";
                        lblMessage.Text = "Records DeActivated";
                    }
                    else
                    {
                        lblMessage.Text = strMsg;
                    }

                }
                GetDeActivityAllergies();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            //objDl = null;
            //objEMR = null;
            //objemrallrgy = null;
        }
    }


    protected void gvCIMSAllergy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            rdoAllergyType.SelectedValue = "2";
            rdoAllergyType_SelectedIndexChanged(this, null);
            int RowIndex = 0;
            RowIndex = common.myInt(gvCIMSAllergy.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;
            HiddenField hdndaRowId = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaId");
            txteditno.Text = hdndaRowId.Value;
            HiddenField hdndaAllergyID = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaAllergyID");
            HiddenField hdndaGENERIC_ID = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaGENERIC_ID");
            Label lbldaAllergyDate = (Label)gvCIMSAllergy.SelectedRow.FindControl("lbldaAllergyDate");
            HiddenField hdndaAllergyDate = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaAllergyDate");
            Label lbldaAllergyName = (Label)gvCIMSAllergy.SelectedRow.FindControl("lbldaAllergyName");
            HiddenField hdndaTYPE = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaTYPE");
            Label lbldaReaction = (Label)gvCIMSAllergy.SelectedRow.FindControl("lbldaReaction");
            HiddenField hdndaRemarks = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaRemarks");
            HiddenField hdnAllergySeverity = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdnAllergySeverity");
            HiddenField hdndaIntolerance = (HiddenField)gvCIMSAllergy.SelectedRow.FindControl("hdndaIntolerance");

            ddlAllergyPlausibility.SelectedIndex = ddlAllergyPlausibility.Items.IndexOf(ddlAllergyPlausibility.Items.FindItemByValue(hdnAllergySeverity.Value.ToString()));
            ltrhdnCurrentRowId.Text = hdndaRowId.Value;

            lblhdn_GENERIC_ID.Text = hdndaGENERIC_ID.Value;
            lblhdn_ALLERGYID.Text = hdndaAllergyID.Value;
            hdnIsUnSavedData.Value = "1";
            hdnGenericId.Value = hdndaGENERIC_ID.Value;
            hdnItemId.Value = hdndaAllergyID.Value;

            ddlBrand.Text = lbldaAllergyName.Text;

            lblhdn_TYPE.Text = hdndaTYPE.Value;
            if (lbldaAllergyDate.Text != "")
            {
                dtpAllergyDate.SelectedDate = Convert.ToDateTime(hdndaAllergyDate.Value);
            }
            else
            {
                dtpAllergyDate.Clear();
            }
            chkIntolerance.Checked = common.myBool(hdndaIntolerance.Value);
            txtReaction.Text = lbldaReaction.Text;
            txtRemarks.Text = hdndaRemarks.Value;
            lblMessage.Text = "";
            btnAddtogrid.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvCIMSAllergy_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            //objemrallrgy = new BaseC.EMRAllergy(sConString);

            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);


                int intId = common.myInt(e.CommandArgument);

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;
                }

                if (intId == 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Cache["CIMSAllergy" + Session["UserId"]];
                    dt.Rows.RemoveAt(row.RowIndex);
                    gvCIMSAllergy.DataSource = dt;
                    gvCIMSAllergy.DataBind();
                    tbcAllergy.ActiveTabIndex = 1;
                    tbpnlCIMSAllergy.HeaderText = "CIMS Allergies(" + common.myStr(dt.Rows.Count) + ")";
                    txtCIMSCancellationRemarks.Text = "";
                    dtpAllergyDate.SelectedDate = null;
                    chkIntolerance.Checked = false;
                    txtReaction.Text = "";
                    txtRemarks.Text = "";
                    lblMessage.Text = "Records DeActivated";
                }
                else
                {
                    if (common.myStr(txtCIMSCancellationRemarks.Text).Trim().Length == 0)
                    {
                        lblMessage.Text = "De-activation remarks can't be blank !";
                        Page.SetFocus(txtCIMSCancellationRemarks);

                        return;
                    }
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/deActivatePatientAllergy";
                    APIRootClass.Allergy objRoot = new global::APIRootClass.Allergy();
                    objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                    objRoot.PageId = common.myInt(ViewState["PageId"]);
                    objRoot.Flag = 2;
                    objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
                    objRoot.Id = intId;
                    objRoot.CancelRemarks = common.myStr(txtdaCancellationRemarks.Text);
                    objRoot.EncodedBy = common.myInt(Session["UserId"]);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string strMsg = client.UploadString(ServiceURL, inputJson);
                    strMsg = JsonConvert.DeserializeObject<string>(strMsg);
                    //objEMR = new BaseC.clsEMR(sConString);
                    //string strMsg = objEMR.deActivatePatientAllergy(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                    //                   common.myInt(ViewState["PageId"]), 2, common.myInt(ViewState["RegistrationId"]),
                    //                   common.myInt(ViewState["EncounterId"]), intId,
                    //                   common.myStr(txtCIMSCancellationRemarks.Text), common.myInt(Session["UserId"]));

                    if (strMsg == "")
                    {
                        //string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value +
                        //" FROM EMRPatientForms epf " +
                        //" INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId " +
                        //" AND epfd.PageId = " + ViewState["PageId"].ToString() + "  WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) +
                        //" AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
                        //" AND epf.Active = 1 ";

                        //code optimization-instead of above code query, below produre containing function will return the desired ouptut however this delete command is not firing  : Sikandar

                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/updateshownote";
                        APIRootClass.Allergy Allergy = new global::APIRootClass.Allergy();
                        Allergy.showNotesVal = rblShowNote.SelectedItem.Value.ToString();
                        Allergy.PageId = common.myInt(ViewState["PageId"]);
                        Allergy.EncounterId = common.myInt(ViewState["EncounterId"]);
                        Allergy.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        inputJson = (new JavaScriptSerializer()).Serialize(Allergy);
                        string strmsg = client.UploadString(ServiceURL, inputJson);
                        strmsg = JsonConvert.DeserializeObject<string>(strmsg);

                        // string strmsg = objemrallrgy.updateshownote(rblShowNote.SelectedItem.Value.ToString(), common.myInt(ViewState["PageId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));

                        //objDl.ExecuteNonQuery(CommandType.Text, sQuery);

                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        //Label lblProdId = (Label)row.FindControl(“lblproductId”);
                        DataTable dt = new DataTable();
                        dt = (DataTable)Cache["CIMSAllergy" + Session["UserId"]];
                        dt.Rows.RemoveAt(row.RowIndex);
                        gvCIMSAllergy.DataSource = dt;
                        gvCIMSAllergy.DataBind();
                        tbcAllergy.ActiveTabIndex = 1;
                        tbpnlCIMSAllergy.HeaderText = "CIMS Allergies(" + common.myStr(dt.Rows.Count) + ")";
                        txtCIMSCancellationRemarks.Text = "";
                        dtpAllergyDate.SelectedDate = null;
                        chkIntolerance.Checked = false;
                        txtReaction.Text = "";
                        txtRemarks.Text = "";
                        txtoaCancellationRemarks.Text = "";
                        lblMessage.Text = "Records DeActivated";
                    }
                    else
                    {
                        lblMessage.Text = strMsg;
                    }

                }
                GetDeActivityAllergies();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            //objDl = null;
            //objEMR = null;
            //objemrallrgy = null;
        }
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {

    }
    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        divDelete.Visible = false;
    }
    protected void btnAlert_Click(object sender, EventArgs e)
    {
        divDelete.Visible = true;
        Response.Redirect("~" + Request.Url.PathAndQuery, false);
        //string str=Request.Url.;
        //Request.Url.PathAndQuery;

    }

    //added by bhakti
    public void BindGen()
    {
      
        rdoAllergyType.Items[1].Enabled = true;
        rdoAllergyType.SelectedValue = "0";
        DataTable data = GetGenericData("");
        if (data.Rows.Count > 0)
        {
            DataRow[] dr = data.Select("GenericName='Others'");
            if (dr.Length > 0)
            {
                data = dr.CopyToDataTable();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    ddlGeneric.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["GenericName"]), common.myStr(data.Rows[i]["GenericId"])));
                }
                ddlGeneric.SelectedIndex = 0;

            }
        }
        rdoAllergyType_SelectedIndexChanged(this, null);
    }

    //added by bhakti
    public void BindGenOther()
    {
        ddlGeneric.Items.Clear();
        rdoAllergyType.Items[1].Enabled = true;
        rdoAllergyType.SelectedValue = "0";
        DataTable data = GetGenericData("");
        if (data.Rows.Count > 0)
        {
            DataRow[] dr = data.Select("GenericName='Food' OR GenericName='Others'");
            if (dr.Length > 0)
            {
                data = dr.CopyToDataTable();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    ddlGeneric.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["GenericName"]), common.myStr(data.Rows[i]["GenericId"])));
                }
                ddlGeneric.SelectedIndex = 0;

            }
        }
    }
}