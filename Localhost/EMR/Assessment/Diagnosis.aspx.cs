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
using System.Text;
using System.Xml;
using System.Drawing;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Diagnosis_Default : System.Web.UI.Page
{
    #region Page level variable declration section
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private string UtdLink = ConfigurationManager.ConnectionStrings["UTDLink"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    //BaseC.ParseData Parse = new BaseC.ParseData();
    //BaseC.DiagnosisDA objDiag;

    Hashtable hsNewPage = new Hashtable();
    DL_Funs fun = new DL_Funs();
    static DataView DvPatientDiagnosis = new DataView();
    bool statusDis = false;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).ToUpper() == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }

    #endregion
    #region PageLoad section

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblMessage.Text = "";
            //tbDiagnosis.Tabs[1].Visible = false; 
            if (Session["encounterid"] == null)
            {
                Response.Redirect("/Default.aspx?RegNo=0", false);
            }
            if (common.myStr(Request.QueryString["From"]) == "POPUP")
            {
                //btnBackPDashboard.Visible = false;
                //tblNavigation.Visible = false;
            }
            if (!IsPostBack)
            {

                divExcludedService.Visible = false;
                //Added By Ashutosh Prashar :13/05/2013:: For Deletion Confirmation
                BindFavouriteDiagnosis("");
                dvConfirmDeletion.Visible = false;
                dvChronicConfirmDeletion.Visible = false;
                //End Here

                rdpOnsetDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

                txtIcdCodes.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnAddtogrid.UniqueID + "','')");

                hdnIsUnSavedData.Value = "0";
                if (Request.QueryString["Mpg"] != null)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    string pid = Session["CurrentNode"].ToString();
                    int len = pid.Length;
                    ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
                    Session["diagpId"] = ViewState["PageId"];
                }
                else
                {
                    if (Session["diagpId"] != null)
                        ViewState["PageId"] = Session["diagpId"];
                    else
                        ViewState["PageId"] = "133";
                }


                //getDoctorID();
                ViewState["DoctorID"] = common.myStr(Session["EmployeeId"]);
                BindFacility();

                BindProvider();

                BindDiagnosisType();
                BindDiagnosisStatus();
                //BindPatientAlert();
                BindDrpCategory();//19th April10
                BindSubCategory("");
                // BindPatientHiddenDetails();
                //if (common.myStr(Session["IsMedicalAlert"]) == "")
                //{
                //    lnkAlerts.Enabled = false;
                //    lnkAlerts.CssClass = "blinkNone";
                //    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
                //}
                //else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
                //{
                //    lnkAlerts.Enabled = true;
                //    lnkAlerts.Font.Bold = true;
                //    lnkAlerts.CssClass = "blink";
                //    lnkAlerts.Font.Size = 11;
                //    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
                //}

                RetrievePatientDiagnosis();

                if (Request.QueryString["Page"] != null) //1 May 
                {

                    btnBackToSuperBill.Visible = true;
                }
                if (Request.QueryString["DiagnosisId"] != null && Request.QueryString["DiagnosisId"] != "")
                {
                    int iDiagnosisId = common.myInt(Request.QueryString["DiagnosisId"].ToString());
                    btnBackToSuperBill.Visible = true;
                    FillDiagnosisDetailControls(iDiagnosisId);
                }

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
                {
                    btnAddtogrid.Visible = false;
                    ddlCategory.Enabled = false;
                    ddlSubCategory.Enabled = false;
                    ddlDiagnosiss.Enabled = false;
                    gvChronicDiagnosis.Enabled = false;
                    gvDiagnosisDetails.Enabled = false;
                }

                txtIcdCodes.Attributes.Add("onblur", "nSat=1;");
                SetPermission();
                txtSearch.Focus();

            }
            string controlName = Request.Params.Get("__EVENTTARGET");

            if (hdnRowIndex.Value != "" && gvDiagnosisDetails.Rows.Count >= common.myInt(hdnRowIndex.Value))
            {
                GridViewRow gvr = gvDiagnosisDetails.Rows[common.myInt(hdnRowIndex.Value)];
                Literal lbl = (Literal)gvr.FindControl("lblDiagnosisXML");
                lbl.Text = hdnProblems.Value;
                TextBox txtProblem = (TextBox)gvr.FindControl("txtDiagnosisDetails");
                Literal ltrl = new Literal();
                ltrl.Mode = LiteralMode.Transform;
                ltrl.Text = HttpUtility.HtmlDecode(formatString(hdnProblems.Value));
                txtProblem.Text = ltrl.Text;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void SetPermission()
    {
        //UserAuthorisations ua1 = new UserAuthorisations(sConString);
        UserAuthorisations ua1 = new UserAuthorisations(string.Empty);
        try
        {
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
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);
    //    }
    //}
    protected void lnkDiagnosisDetails_Click(object sender, EventArgs e)
    {
        try
        {

            RadWindowForNew.NavigateUrl = "/EMR/Assessment/DiagnosisDetails.aspx";

            RadWindowForNew.Title = "ICD 10 Help";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //void BindPatientAlert()
    //{
    //    /*************** Patient Alert ***************/
    //    try
    //    {
    //        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
    //        ds = new DataSet();

    //        ds = objEMR.getEMRPatientAlertDetails(common.myInt(Session["RegistrationId"]));

    //        ddlPatientAlert.DataSource = ds.Tables[0];
    //        ddlPatientAlert.DataValueField = "AlertId";
    //        ddlPatientAlert.DataTextField = "AlertDescription";
    //        ddlPatientAlert.DataBind();

    //        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
    //        {
    //            ddlPatientAlert.Items[idx].Checked = common.myBool(ds.Tables[0].Rows[idx]["IsChk"]);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    protected Boolean ISDiagnosesExits()
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);
            //ds = (DataSet)objDiag.ISDiagnosesExits(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]));
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/ISDiagnosesExits";
            APIRootClass.ISDiagnosesExits objRoot = new global::APIRootClass.ISDiagnosesExits();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
            objRoot.EncounterId = common.myInt(Session["encounterid"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["isDiagnoses"] = "Y";
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return false;
        }
        finally
        {
            ds.Dispose();
        }

    }

    #endregion
    private void FillDiagnosisDetailControls(Int32 iICDID)
    {
        DataTable dtDignosisDetails = (DataTable)Cache["DignosisDetails"];
        DataView dv = new DataView(dtDignosisDetails);
        DataTable dt = new DataTable();
        try
        {
            dv.RowFilter = "Id=" + iICDID;
            if (dv.Count > 0)
            {
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    hdnDiagnosisId.Value = dt.Rows[0]["ICDID"].ToString();
                    txtIcdCodes.Text = dt.Rows[0]["ICDCode"].ToString();


                    ddlDiagnosiss.Text = dt.Rows[0]["ICDDescription"].ToString();
                    if (dt.Rows[0]["LocationId"] != DBNull.Value)
                        ddlSides.SelectedIndex = ddlSides.Items.IndexOf(ddlSides.Items.FindItemByValue(dt.Rows[0]["LocationId"].ToString()));
                    chkPrimarys.Checked = dt.Rows[0]["PrimaryDiagnosis"].ToString().ToLower() == "true" ? true : false;
                    chkResolve.Checked = dt.Rows[0]["IsResolved"].ToString().ToLower() == "true" ? true : false;
                    chkChronics.Checked = dt.Rows[0]["IsChronic"].ToString().ToLower() == "true" ? true : false;
                    ViewState["chkDiagnosisTF"] = chkChronics.Checked ? true : false;
                    chkIsFinalDiagnosis.Checked = common.myBool(dt.Rows[0]["IsFinalDiagnosis"]);
                    ddlDiagnosisStatus.SelectedItem.Text = dt.Rows[0]["Conditions"].ToString();
                    txtstatusIds.Text = dt.Rows[0]["Conditionids"].ToString();
                    if (dt.Rows[0]["TypeId"] != DBNull.Value)
                        ddlDiagnosisType.SelectedIndex = ddlDiagnosisType.Items.IndexOf(ddlDiagnosisType.Items.FindItemByValue(dt.Rows[0]["TypeId"].ToString()));

                    if (dt.Rows[0]["DoctorId"] != DBNull.Value)
                        ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(dt.Rows[0]["DoctorId"].ToString()));


                    if (dt.Rows[0]["OnsetDate"] != DBNull.Value)
                        rdpOnsetDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["OnsetDate"]);

                    ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(dt.Rows[0]["FacilityId"].ToString()));
                    txtcomments.Text = dt.Rows[0]["Remarks"].ToString();
                    if (ddlDiagnosiss.Text != "")
                    {
                        btnAddtogrid.Text = "Update List";
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
            dtDignosisDetails.Dispose();
            dv.Dispose();
            dt.Dispose();
        }
    }
    protected void btnBackPDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Dashboard/PatientDashboard.aspx", false);
    }
    #region Get data and Retrive data section
    private void getDoctorID()
    {
        try
        {
            //BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
            //SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), Convert.ToInt16(common.myInt(Session["UserId"])));
            //if (dr.HasRows == true)
            //{
            //    dr.Read();
            //    String strSelID = dr[0].ToString();
            //    if (strSelID != "")
            //    {
            //        char[] chArray = { ',' };
            //        string[] serviceIdXml = strSelID.Split(chArray);
            //        ViewState["DoctorID"] = serviceIdXml[0].ToString();
            //    }
            //}
            ViewState["DoctorID"] = Session["EmployeeId"];
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string getEmpID()
    {
        //string EmpId = "";
        //try
        //{
        //    BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
        //    SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), Convert.ToInt16(common.myInt(Session["UserID"])));
        //    if (dr.HasRows == true)
        //    {
        //        dr.Read();
        //        String strSelID = dr[0].ToString();
        //        if (strSelID != "")
        //        {
        //            char[] chArray = { ',' };
        //            string[] serviceIdXml = strSelID.Split(chArray);
        //            EmpId = serviceIdXml[0].ToString();
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
        //return EmpId;
        return common.myStr(Session["EmployeeId"]);
    }

    protected void RetrievePatientDiagnosis()
    {
        DataSet ds = new DataSet();
        DataView dvStTemplate = new DataView();
        DataView dvDiagnosisDetail = new DataView();
        //yogesh 28-09-2022
        DataView dvProvisional = new DataView();
        DataTable dtProvisional = new DataTable();
        DataView dvResolved = new DataView();
        DataTable dtResolved = new DataTable();
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

                    dvDiagnosisDetail.RowFilter = "ISNULL(IsChronic,0)=1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    dtChronicDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtChronicDiagnosisDetail.Rows.Count > 0)
                    {
                        gvChronicDiagnosis.DataSource = dtChronicDiagnosisDetail;
                        gvChronicDiagnosis.DataBind();
                    }
                    else
                    {
                        BindBlankChronicDiagnosisGrid();
                    }

                    //yogesh provisionl 28-09-2022
                    dvProvisional = new DataView(dvStTemplate.ToTable());

                    dvProvisional.RowFilter = "ISNULL(IsQuery,0)=1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    dtProvisional = dvProvisional.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        gvProvisionalDiag.DataSource = dtDiagnosisDetail;
                        gvProvisionalDiag.DataBind();
                        //chkPullDiagnosis.Checked = Convert.ToString(dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"]) == "" ? false : (Boolean)dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"];
                        
                        gvDiagnosisDetails.Columns[4].Visible = true;
                    }
                    else
                    {
                        BindBlankProvisionalDiagnosisGrid();
                        chkQuery.Checked = false;
                    }
                    //yogesh Resolved 28-09-2022
                    dvResolved = new DataView(dvStTemplate.ToTable());

                    dvResolved.RowFilter = "ISNULL(IsResolved,0)=1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    dtResolved = dvResolved.ToTable();
                    if (dtResolved.Rows.Count > 0)
                    {
                        ResolvedGrid.DataSource = dtResolved;
                        ResolvedGrid.DataBind();
                        //chkPullDiagnosis.Checked = Convert.ToString(dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"]) == "" ? false : (Boolean)dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"];
                        
                        gvDiagnosisDetails.Columns[4].Visible = true;
                    }
                    else
                    {
                        BindBlankResolvedDiagnosisGrid();
                        chkQuery.Checked = false;
                    }






                    dvDiagnosisDetail.RowFilter = "ISNULL(IsChronic,0) <> 1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        gvDiagnosisDetails.DataSource = dtDiagnosisDetail;
                        gvDiagnosisDetails.DataBind();
                        chkPullDiagnosis.Checked = Convert.ToString(dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"]) == "" ? false : (Boolean)dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"];
                        //Cache.Insert("DignosisDetails", dtDiagnosisDetail, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        gvDiagnosisDetails.Columns[4].Visible = true;
                    }
                    else
                    {
                        BindBlankDiagnosisDetailGrid();
                        chkPullDiagnosis.Checked = false;
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
                    BindBlankChronicDiagnosisGrid();
                    BindBlankDiagnosisDetailGrid();
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
            dvStTemplate.Dispose();
            dvDiagnosisDetail.Dispose();
            dtChronicDiagnosisDetail.Dispose();
            dtDiagnosisDetail.Dispose();
        }
    }

    #endregion


    #region XML Formate section

    protected string createXML(string sDiagnosisId, string sDiagnosisCode, string sDiagnosisName, string sOnsetDate, string sPrimary, string sChronic, string sRemarks)
    {
        StringBuilder objStr = new StringBuilder();
        objStr.Append("<Table1>");

        objStr.Append("<c1>");
        objStr.Append(sDiagnosisId);
        objStr.Append("</c1>");

        objStr.Append("<c2>");
        objStr.Append(sDiagnosisCode);
        objStr.Append("</c2>");

        objStr.Append("<c3>");
        objStr.Append(sDiagnosisName);
        objStr.Append("</c3>");

        objStr.Append("<c4>");
        objStr.Append(sOnsetDate);
        objStr.Append("</c4>");

        objStr.Append("<c5>");
        objStr.Append(sPrimary);
        objStr.Append("</c5>");

        objStr.Append("<c6>");
        objStr.Append(sChronic);
        objStr.Append("</c6>");

        objStr.Append("<c7>");
        objStr.Append(sRemarks);
        objStr.Append("</c7>");

        objStr.Append("</Table1>");
        return objStr.ToString();
    }

    protected string formatString(string sXML)
    {
        StringBuilder objStr = new StringBuilder();
        try
        {
            if (sXML != "")
            {
                XmlDocument objXml = new XmlDocument();
                XmlNodeList objNode;
                objXml.LoadXml(sXML);
                objNode = objXml.GetElementsByTagName("c3");
                if (objNode[0].InnerText != "")
                {
                    objStr.Append("Diagnosis: " + objNode[0].InnerText);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return objStr.ToString();
    }

    #endregion


    #region Assessment Details link section

    protected void lnkNewForm_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["Record"] != null)
            {
                if (common.myInt(ViewState["Record"]) == 1)
                    Response.Redirect("DetailAssessment.aspx", false);
                else
                    Alert.ShowAjaxMsg("Kindly save atleast one Diagnosis!", Page);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #endregion

    #region Search section
    protected void btnSearchICDCode_Click(object sender, EventArgs e)
    {

        //BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
        DataSet objDs = new DataSet();
        //objDs = (DataSet)ObjDiagnosis.selectDiscriptionandICDID(txtIcdCodes.Text.Trim());

        try
        {

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiscriptionandICDID";
            APIRootClass.GetDiscriptionandICDID objRoot = new global::APIRootClass.GetDiscriptionandICDID();
            objRoot.IcdCode = txtIcdCodes.Text.Trim();


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (objDs.Tables[0].Rows.Count > 0)
            {
                ViewState["hdnDiagnosisId"] = objDs.Tables[0].Rows[0][0].ToString();
                ddlDiagnosiss.Text = objDs.Tables[0].Rows[0][1].ToString();
                hdnDiagnosisId.Value = objDs.Tables[0].Rows[0][0].ToString();
                ddlDiagnosiss.SelectedValue = objDs.Tables[0].Rows[0][0].ToString();
                //txtOnsetDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                //rdpOnsetDate.SelectedDate = DateTime.Now;
            }
            else
            {
                ddlDiagnosiss.Text = "";
                lblMessage.Text = "No Data Found...";
                lblMessage.Visible = true;
            }
            ddlSides.SelectedValue = "0";
            chkPrimarys.Checked = false;
            chkChronics.Checked = false;
            chkIsFinalDiagnosis.Checked = false;

            ddlDiagnosisStatus.SelectedValue = "0";
            ddlDiagnosisType.SelectedValue = "0";
            chkResolve.Checked = false;

            //ddlProviders.SelectedValue = "0";
            //ddlFacility.SelectedValue = "0";
            txtcomments.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDs.Dispose();
        }
    }

    // Populating Category Dropdown
    private void BindDrpCategory()
    {
        DataSet ds = new DataSet();
        try
        {
            ddlCategory.Items.Clear();
            //objDiag = new BaseC.DiagnosisDA(sConString);

            //ds = objDiag.BindCategory();

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetICDGroup";

            string inputJson = (new JavaScriptSerializer()).Serialize(null);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            RadComboBoxItem lsts;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                lsts = new RadComboBoxItem();
                lsts.Text = dr["GroupName"].ToString();
                lsts.Value = dr["GroupId"].ToString();
                lsts.Attributes.Add("GroupType", dr["GroupType"].ToString());
                ddlCategory.Items.Add(lsts);
            }
            ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "0"));

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

    private void BindSubCategory(string GroupId)
    {
        DataSet ds = new DataSet();
        try
        {
            ddlSubCategory.Items.Clear();
            if (GroupId != "")
            {
                //objDiag = new BaseC.DiagnosisDA(sConString);

                //ds = objDiag.BindSubCategory(common.myStr(GroupId));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetICDSubGroup";
                APIRootClass.GetICDSubGroup objRoot = new global::APIRootClass.GetICDSubGroup();
                objRoot.DiagnosisGroupId = GroupId;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                ddlSubCategory.Items.Clear();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubCategory.DataSource = ds.Tables[0];
                    ddlSubCategory.DataValueField = "SubGroupId";
                    ddlSubCategory.DataTextField = "SubGroupName";
                    ddlSubCategory.DataBind();
                    ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", "0"));
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
        }
    }

    protected void ddlSubCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            //Hashtable hstInput = new Hashtable();
            //hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            //hstInput.Add("@intSubGroupId", ddlSubCategory.SelectedValue);
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);


            //WebClient client = new WebClient();
            //client.Headers["Content-type"] = "application/json";
            //client.Encoding = Encoding.UTF8;
            //string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiagnosisList";
            //APIRootClass.GetDiagnosisList objRoot = new global::APIRootClass.GetDiagnosisList();
            //objRoot.DiagnosisGroupId = common.myInt(ddlCategory.SelectedValue);
            //objRoot.DiagnosisSubGroupId = common.myInt(ddlSubCategory.SelectedValue);

            //string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            //string sValue = client.UploadString(ServiceURL, inputJson);
            //sValue = JsonConvert.DeserializeObject<string>(sValue);
            //ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    //DataTable dt = new DataTable();
            //    //dt = ds.Tables[0];
            //    ds.Tables[0].Columns.Add("Id");
            //    ds.Tables[0].Columns.Add("EncounterDate");

            //}
            //else
            //{
            //    BindBlankGrid();
            //}
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

    protected void ddlCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            BindSubCategory(ddlCategory.SelectedValue);
            //Hashtable hstInput = new Hashtable();
            //hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);

            //WebClient client = new WebClient();
            //client.Headers["Content-type"] = "application/json";
            //client.Encoding = Encoding.UTF8;
            //string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiagnosisList";
            //APIRootClass.GetDiagnosisList objRoot = new global::APIRootClass.GetDiagnosisList();
            //objRoot.DiagnosisGroupId = common.myInt(ddlCategory.SelectedValue);
            //objRoot.DiagnosisSubGroupId = 0;

            //string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            //string sValue = client.UploadString(ServiceURL, inputJson);
            //sValue = JsonConvert.DeserializeObject<string>(sValue);
            //ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    //DataTable dt = new DataTable();
            //    //dt = ds.Tables[0];
            //    ds.Tables[0].Columns.Add("Id");
            //    ds.Tables[0].Columns.Add("EncounterDate");

            //}
            //else
            //{
            //    BindBlankGrid();
            //}
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

    #endregion

    #region Telerik RadGrid section

    private void create_window(string url)
    {
        // This window is created for each match of the selected ICD value for a patient to a rule .
        // If a patient matches any rule in the rule engine then this window should pop up at the selection of an ICD9 value.
        try
        {
            RadWindow newwindow = new RadWindow();
            //set the properties of rad window here
            newwindow.ID = "RadWindowForNew";
            newwindow.Width = 950;
            newwindow.Height = 510;
            newwindow.Title = "Decision Support";
            newwindow.AutoSize = false;
            newwindow.NavigateUrl = url;
            newwindow.VisibleOnPageLoad = true;
            //The newly created RadWindow is added to the RadWindowManager here.
            RadWindowManager.Windows.Add(newwindow);
            //RadWindowManager.VisibleTitlebar = false;
            RadWindowManager.VisibleStatusbar = false;
            RadWindowManager.EnableViewState = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    #endregion

    #region TopButtonsData section

    protected void btnGoDiagnosisHistory_OnClick(Object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("DiagnosisHistory.aspx", false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataTable PopulateAllDiagnosis(string txt)
    {
        DataTable DT = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {

            ViewState["BTN"] = "ALL";

            if (Session["encounterid"] != null)
            {
                //objDiag = new BaseC.DiagnosisDA(sConString);


                //ds = objDiag.BindDiagnosis(common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), strSearchCriteria); 

                string strSearchCriteria = string.Empty;

                strSearchCriteria = "%" + common.myStr(txt, true) + "%";

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindDiagnosis";
                APIRootClass.BindDiagnosis objRoot = new global::APIRootClass.BindDiagnosis();
                objRoot.DiagnosisGroupId = common.myInt(ddlCategory.SelectedValue);
                objRoot.DiagnosisSubGroupId = common.myInt(ddlSubCategory.SelectedValue);
                objRoot.DiagnosisCode = strSearchCriteria;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    dt = ds.Tables[0];
                    dt.Columns.Add("Id");
                    dt.Columns.Add("EncounterDate");
                    DT = dt;
                }
                else
                {
                    BindBlankGrid();
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
            dt.Dispose();
        }

        return DT;
    }
    protected void BindFavouriteDiagnosis(string text)
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);

            //ds = objDiag.BindFavoriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), text);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindFavouriteDiagnosis";
            APIRootClass.BindFavouriteDiagnosis objRoot = new global::APIRootClass.BindFavouriteDiagnosis();
            objRoot.DoctorId = common.myInt(Session["DoctorID"]);
            objRoot.DiagnosisGroupId = common.myInt(ddlCategory.SelectedValue);
            objRoot.DiagnosisSubGroupId = common.myInt(ddlSubCategory.SelectedValue);
            objRoot.DiagnosisCode = text;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFavourite.DataSource = ds.Tables[0];
                gvFavourite.DataBind();
            }
            else
            {
                gvFavourite.DataSource = null;
                gvFavourite.DataBind();
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
    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        try
        {
            //BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
            if (common.myInt(ddlDiagnosiss.SelectedValue) > 0)
            {
                //string strsave = ObjDiagnosis.SaveFavouriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(ddlDiagnosiss.SelectedValue), common.myInt(Session["UserID"]));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveFavouriteDiagnosis";
                APIRootClass.SaveFavouriteDiagnosis objRoot = new global::APIRootClass.SaveFavouriteDiagnosis();
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.DiagnosisId = common.myInt(ddlDiagnosiss.SelectedValue);
                objRoot.UserId = common.myInt(Session["UserID"]);
                string OutResult = string.Empty;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                string strsave = JsonConvert.DeserializeObject<string>(sValue);

                if (strsave.Contains("AlReady Exist in your favorite list"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;

                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;

                }
            }
            else
            {
                Alert.ShowAjaxMsg("Select Diagnosis to add into favorite list", Page);
                return;
            }
            BindFavouriteDiagnosis("");
            ddlDiagnosiss.SelectedValue = "0";
            ddlDiagnosiss.Text = "";
            ddlDiagnosiss.Items.Clear();
            txtIcdCodes.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvFavourite_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #endregion

    #region BindDiagnosisGrid and Chronic Grid section

    protected void BindDiagnosisDetailsGrid(int iICDID, string sICDCode, string sDiagnosisName, string sDiagnosisDetails, string sDiagnosisXML)
    {
        DataTable dT = new DataTable();
        try
        {
            DataRow dr;
            dT = CreateTable();

            //checking problem grid have any rows or not
            if (gvDiagnosisDetails.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvDiagnosisDetails.Rows)
                {
                    if (iICDID.ToString() != gvr.Cells[0].Text)
                    {
                        dr = dT.NewRow();

                        TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");//
                        TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                        if (txtDiagnosisName.Text != "")
                        {
                            TextBox txtOnsetDate = (TextBox)gvr.FindControl("txtOnsetDate");
                            DropDownList ddlSide = (DropDownList)gvr.FindControl("ddlSide");
                            CheckBox chkPrimary = (CheckBox)gvr.FindControl("chkPrimary");
                            CheckBox chkChronic = (CheckBox)gvr.FindControl("chkChronic");

                            DropDownList ddlStatus = (DropDownList)gvr.FindControl("StatusId");
                            DropDownList ddlType = (DropDownList)gvr.FindControl("TypeId");
                            CheckBox chkResolved = (CheckBox)gvr.FindControl("IsResolved");

                            DropDownList ddlProvider = (DropDownList)gvr.FindControl("DoctorId");
                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("FacilityId");
                            TextBox txtComments = (TextBox)gvr.FindControl("txtComments");
                            Label lblId = (Label)gvr.FindControl("lblId");

                            dr["ICDID"] = gvr.Cells[0].Text;
                            dr["IcdCode"] = txtIcdCode.Text;
                            dr["ICDDescription"] = txtDiagnosisName.Text;
                            dr["OnsetDate"] = rdpOnsetDate.SelectedDate;
                            dr["LocationId"] = ddlSide.SelectedValue;
                            dr["PrimaryDiagnosis"] = chkPrimary.Checked;
                            dr["Chronic"] = chkChronic.Checked;

                            dr["ConditionIds"] = ddlStatus.SelectedValue;
                            dr["TypeId"] = ddlType.SelectedValue;
                            dr["IsResolved"] = chkResolved.Checked;

                            dr["DoctorId"] = ddlProvider.SelectedValue;
                            dr["FacilityId"] = ddlLocation.SelectedValue;
                            dr["Remarks"] = txtComments.Text;
                            dr["Id"] = lblId.Text;
                            dT.Rows.Add(dr);
                        }
                    }
                }
                dr = dT.NewRow();
                dr["ICDID"] = iICDID;
                dr["IcdCode"] = sICDCode;
                dr["ICDDescription"] = sDiagnosisName;
                dr["OnsetDate"] = "";
                dr["OnsetDateWithoutFormat"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["Chronic"] = "";

                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";

                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "0";
                dT.Rows.Add(dr);
            }
            else
            {
                //if grid does not have any row then bind it through selected data               
                dr = dT.NewRow();
                dr["ICDID"] = iICDID;
                dr["IcdCode"] = sICDCode;
                dr["ICDDescription"] = sDiagnosisName;
                dr["OnsetDate"] = "";
                dr["OnsetDateWithoutFormat"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["Chronic"] = "0";

                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";

                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "0";
                dT.Rows.Add(dr);
            }
            Cache.Insert("DignosisDetails", dT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            gvDiagnosisDetails.DataSource = (DataTable)Cache["DignosisDetails"];
            gvDiagnosisDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    protected void BindChronicGrid(int iICDID, string sICDCode, string sDiagnosisName, string sDiagnosisDetails, string sDiagnosisXML)
    {
        DataTable dT = new DataTable();
        try
        {
            DataRow dr;
            dT = CreateTable();

            //checking problem grid have any rows or not
            if (gvChronicDiagnosis.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvChronicDiagnosis.Rows)
                {
                    if (iICDID.ToString() != gvr.Cells[0].Text)
                    {
                        dr = dT.NewRow();

                        TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");//
                        TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                        if (txtDiagnosisName.Text != "")
                        {
                            TextBox txtOnsetDate = (TextBox)gvr.FindControl("txtOnsetDate");
                            string hndOnsetDateWithoutFormat = ((HiddenField)gvr.FindControl("HdnOnsetDate")).Value;
                            DropDownList ddlSide = (DropDownList)gvr.FindControl("ddlSide");
                            CheckBox chkPrimary = (CheckBox)gvr.FindControl("chkPrimary");
                            CheckBox chkChronic = (CheckBox)gvr.FindControl("chkChronic");

                            DropDownList ddlStatus = (DropDownList)gvr.FindControl("StatusId");
                            DropDownList ddlType = (DropDownList)gvr.FindControl("TypeId");
                            CheckBox chkResolved = (CheckBox)gvr.FindControl("IsResolved");

                            DropDownList ddlProvider = (DropDownList)gvr.FindControl("DoctorId");
                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("FacilityId");
                            TextBox txtComments = (TextBox)gvr.FindControl("txtComments");
                            Label lblId = (Label)gvr.FindControl("lblId");

                            dr["ICDID"] = gvr.Cells[0].Text;
                            dr["IcdCode"] = txtIcdCode.Text;
                            dr["ICDDescription"] = txtDiagnosisName.Text;
                            dr["OnsetDate"] = rdpOnsetDate.SelectedDate;
                            dr["OnsetDateWithoutFormat"] = rdpOnsetDate.SelectedDate;
                            dr["LocationId"] = ddlSide.SelectedValue;
                            dr["PrimaryDiagnosis"] = chkPrimary.Checked;
                            dr["IsChronic"] = chkChronic.Checked;

                            dr["ConditionIds"] = ddlStatus.SelectedValue;
                            dr["TypeId"] = ddlType.SelectedValue;
                            dr["IsResolved"] = chkResolved.Checked;

                            dr["DoctorId"] = ddlProvider.SelectedValue;
                            dr["FacilityId"] = ddlLocation.SelectedValue;
                            dr["Remarks"] = txtComments.Text;
                            dr["Id"] = lblId.Text;
                            dT.Rows.Add(dr);
                        }
                    }
                }
                dr = dT.NewRow();
                dr["ICDID"] = iICDID;
                dr["IcdCode"] = sICDCode;
                dr["ICDDescription"] = sDiagnosisName;
                dr["OnsetDate"] = "";
                dr["OnsetDateWithoutFormat"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "";

                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";

                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "0";
                dT.Rows.Add(dr);
            }
            else
            {
                //if grid does not have any row then bind it through selected data               
                dr = dT.NewRow();
                dr["ICDID"] = iICDID;
                dr["IcdCode"] = sICDCode;//
                dr["ICDDescription"] = sDiagnosisName;
                dr["OnsetDate"] = "";
                dr["OnsetDateWithoutFormat"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "0";

                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";

                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "0";
                dT.Rows.Add(dr);
            }
            Cache.Insert("ChronicDignosisDetails", dT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            gvChronicDiagnosis.DataSource = (DataTable)Cache["ChronicDignosisDetails"];
            gvChronicDiagnosis.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        DataColumn dc = new DataColumn("ID");
        dc.DefaultValue = 0;
        Dt.Columns.Add(dc);
        Dt.Columns.Add("ICDID");
        Dt.Columns.Add("IcdCode");
        Dt.Columns.Add("ICDDescription");
        Dt.Columns.Add("OnsetDate");
        Dt.Columns.Add("OnsetDateWithoutFormat");
        Dt.Columns.Add("LocationId");
        Dt.Columns.Add("PrimaryDiagnosis");
        Dt.Columns.Add("IsChronic");
        Dt.Columns.Add("IsQuery");
        Dt.Columns.Add("ConditionIds");
        Dt.Columns.Add("TypeId");
        Dt.Columns.Add("IsResolved");
        Dt.Columns.Add("DoctorId");
        Dt.Columns.Add("FacilityId");
        Dt.Columns.Add("Remarks");
        Dt.Columns.Add("TemplateFieldId");
        Dt.Columns.Add("IsFinalDiagnosis");
        Dt.Columns.Add("EncodedBy", System.Type.GetType("System.Int32"));

        return Dt;
    }

    protected void gvDiagnosisDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {

            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false; //IcdId
                // e.Row.Cells[1].Visible = false; // Icd Code
                // e.Row.Cells[2].Visible = false; //Description

                e.Row.Cells[3].Visible = false; //
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                //e.Row.Cells[13].Visible = false; //Edit
                //e.Row.Cells[14].Visible = false;// // Delete
                e.Row.Cells[15].Visible = false;// Id
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPrimary = (Label)e.Row.FindControl("lblPrimary");
                if (lblPrimary.Text == "True")
                {
                    lblPrimary.Text = "P";
                }
                else
                {
                    lblPrimary.Text = "S";
                }

                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[13].Controls[0];

                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        ibtnDelete.Visible = false;
                        lnkEdit.Visible = false;
                    }
                }

                Label lblIcdId = (Label)e.Row.FindControl("lblIcdId");
                if (common.myInt(lblIcdId.Text).Equals(0))
                {
                    lblPrimary.Visible = false;
                    ibtnDelete.Visible = false;
                    lnkEdit.Visible = false;
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

    protected void gvChronicDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false; //IcdId
                // e.Row.Cells[1].Visible = false; // Icd Code
                // e.Row.Cells[2].Visible = false; //Description
                e.Row.Cells[3].Visible = false; //
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;// Select
                //e.Row.Cells[14].Visible = false;//Edit
                //e.Row.Cells[15].Visible = false;// Delete
                e.Row.Cells[16].Visible = false;// Id
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");

                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[14].Controls[0];

                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        ibtnDelete.Visible = false;
                        lnkEdit.Visible = false;
                    }
                }

                Label lblIcdId = (Label)e.Row.FindControl("lblIcdId");
                if (common.myInt(lblIcdId.Text).Equals(0))
                {
                    ibtnDelete.Visible = false;
                    lnkEdit.Visible = false;
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

    protected void gvDiagnosisDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDiagnosisDetails.PageIndex = e.NewPageIndex;
            if (ddlDiagnosiss.SelectedIndex > 0)
            {

                string selectedDataKey = ddlDiagnosiss.SelectedValue;
                string selectedDataCode = ddlDiagnosiss.Attributes["ICDCode"].ToString();

                BindDiagnosisDetailsGrid(common.myInt(selectedDataKey), selectedDataCode, "", "", "");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvChronicDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            gvChronicDiagnosis.PageIndex = e.NewPageIndex;
            if (ddlDiagnosiss.SelectedIndex > 0)
            {

                string selectedDataKey = ddlDiagnosiss.SelectedValue;
                string selectedDataCode = ddlDiagnosiss.Attributes["ICDCode"].ToString();

                BindChronicGrid(common.myInt(selectedDataKey), selectedDataCode, "", "", "");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDiagnosisDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            this.ddlDiagnosiss.Text = "";
            this.ddlDiagnosiss.SelectedValue = "";

            if (gvChronicDiagnosis.SelectedIndex != -1)
                gvChronicDiagnosis.SelectedIndex = -1;

            ViewState["chkDiagnosisTF"] = "";
            ViewState["chkChronicTF"] = "";

            int RowIndex = 0;
            RowIndex = common.myInt(gvDiagnosisDetails.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;



            Label lblIcdId = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblIcdId");

            txtIcdId.Text = lblIcdId.Text;

            Label lblId = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblId");

            txtid.Text = lblId.Text;

            if (common.myInt(lblIcdId.Text).Equals(0))
            {
                return;
            }

            this.ddlDiagnosiss.Enabled = false;
            ddlCategory.Enabled = false;
            ddlSubCategory.Enabled = false;

            Label lblICDCode = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblICDCode");
            Label lblDescription = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblDescription");
            Label lblSide = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblSide");
            Label lblPrimary = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblPrimary");
            Label lblChronic = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblChronic");

            HiddenField hdnIsQuery = (HiddenField)gvDiagnosisDetails.SelectedRow.FindControl("hdnIsQuery");


            Label lblddlStatus = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblddlStatus");
            Label lblddlType = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblddlType");
            Label lblResolved = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblResolved");

            Label lblddlProvider = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblddlProvider");
            Label lblOnsetDate = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblOnsetDate");
            Label lblddlLocation = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblddlLocation");
            Label lblComments = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblComments");
            HiddenField HdnOnsetDate = (HiddenField)gvDiagnosisDetails.SelectedRow.FindControl("HdnOnsetDate");
            HiddenField hdnIsFinalDiagnosis = (HiddenField)gvDiagnosisDetails.SelectedRow.FindControl("hdnIsFinalDiagnosis");

            hdnDiagnosisId.Value = lblIcdId.Text;
            txtIcdCodes.Text = lblICDCode.Text.Trim();
            hdnIsUnSavedData.Value = "1";
            ddlDiagnosiss.Text = lblDescription.Text.Trim();
            ddlSides.SelectedValue = lblSide.Text;
            if (lblPrimary.Text == "P")
            {
                chkPrimarys.Checked = true;
            }
            else
            {
                chkPrimarys.Checked = false;
            }

            if (lblResolved.Text == "True")
            {
                chkResolve.Checked = true;
            }
            else
            {
                chkResolve.Checked = false;
            }

            if (lblChronic.Text == "True")
            {
                chkChronics.Checked = true;
                ViewState["chkDiagnosisTF"] = true;
            }
            else
            {
                chkChronics.Checked = false;
                ViewState["chkDiagnosisTF"] = false;
            }
            chkQuery.Checked = common.myBool(hdnIsQuery.Value);
            //ddlDiagnosisStatus.SelectedValue = lblddlStatus.Text;

            chkIsFinalDiagnosis.Checked = common.myBool(hdnIsFinalDiagnosis.Value);

            string strQualityIds = "";
            string conditionText = "";
            if (lblddlStatus.Text != "0" || lblddlStatus.Text != "")
            {
                txtstatusIds.Text = lblddlStatus.Text;
                ddlDiagnosisStatus.Text = "";
                //checking Quality checkbox
                strQualityIds = lblddlStatus.Text;
                string strQuantityID = "";

                int i, j;
                string[] arInfo = new string[4];
                char[] splitter = { ',' };
                arInfo = strQualityIds.Split(splitter);
                for (i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
                {
                    strQuantityID = ddlDiagnosisStatus.Items[i].Value.ToString().Trim();

                    for (j = 0; j < arInfo.Length; j++)
                    {
                        CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                        if (arInfo[j].Trim() == strQuantityID.Trim())
                        {

                            //ddlDiagnosisStatus.Text = ddlDiagnosisStatus.Text + ddlDiagnosisStatus.Items[i].Text.ToString() + ",";

                            conditionText = conditionText + ddlDiagnosisStatus.Items[i].Text.ToString() + ",";

                            checkbox.Checked = true;

                            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Javascript", "javascript: onCheckBoxClick('" + ddlDiagnosisStatus + "');", true);


                        }
                    }
                }
            }

            if (conditionText.Length > 0)
            {
                conditionText = conditionText.Substring(0, conditionText.Length - 1);
            }
            ddlDiagnosisStatus.SelectedItem.Text = conditionText;
            ddlDiagnosisType.SelectedValue = lblddlType.Text;

            ddlProviders.SelectedValue = lblddlProvider.Text;
            if (Convert.ToString(lblOnsetDate.Text).Trim() != "")
                rdpOnsetDate.SelectedDate = Convert.ToDateTime(HdnOnsetDate.Value);
            else
            {
                rdpOnsetDate.Clear();
            }
            ddlFacility.SelectedValue = lblddlLocation.Text;
            txtcomments.Text = lblComments.Text.Trim();
            if (ddlDiagnosiss.Text.Trim() != "")
            {
                btnAddtogrid.Text = "Update List";
            }

            //rdpOnsetDate.Clear(); ;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDiagnosisDetails_PreRender(object sender, EventArgs e)
    {
        if (statusDis == false)
        {
            RetrievePatientDiagnosis();
        }
    }

    protected void gvChronicDiagnosis_PreRender(object sender, EventArgs e)
    {
        if (statusDis == false)
        {
            RetrievePatientDiagnosis();
        }
    }

    protected void gvChronicDiagnosis_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ddlDiagnosiss.Text = "";
            this.ddlDiagnosiss.SelectedValue = "";



            if (gvDiagnosisDetails.SelectedIndex != -1)
                gvDiagnosisDetails.SelectedIndex = -1;

            ViewState["chkDiagnosisTF"] = "";
            ViewState["chkChronicTF"] = "";

            int RowIndex = 0;
            RowIndex = common.myInt(gvChronicDiagnosis.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;

            Label lblIcdId = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblIcdId");
            txtIcdId.Text = lblIcdId.Text;
            Label lblId = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblId");
            txtid.Text = lblId.Text;

            if (common.myInt(lblIcdId.Text).Equals(0))
            {
                return;
            }

            this.ddlDiagnosiss.Enabled = false;
            ddlCategory.Enabled = false;
            ddlSubCategory.Enabled = false;

            Label lblICDCode = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblICDCode");
            Label lblDescription = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblDescription");
            Label lblSide = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblSide");
            Label lblPrimary = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblPrimary");
            Label lblChronic = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblChronic");

            Label lblddlStatus = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblddlStatus");
            Label lblddlType = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblddlType");
            Label lblResolved = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblResolved");

            Label lblddlProvider = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblddlProvider");
            Label lblOnsetDate = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblOnsetDate");
            Label lblddlLocation = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblddlLocation");
            Label lblComments = (Label)gvChronicDiagnosis.SelectedRow.FindControl("lblComments");

            hdnDiagnosisId.Value = lblIcdId.Text;
            txtIcdCodes.Text = lblICDCode.Text.Trim();
            hdnIsUnSavedData.Value = "1";
            ddlDiagnosiss.Text = lblDescription.Text.Trim();

            ddlSides.SelectedValue = lblSide.Text;
            if (lblPrimary.Text == "True")
            {
                chkPrimarys.Checked = true;
            }
            else
            {
                chkPrimarys.Checked = false;
            }

            if (lblResolved.Text == "True")
            {
                chkResolve.Checked = true;
            }
            else
            {
                chkResolve.Checked = false;
            }

            if (lblChronic.Text == "True")
            {
                chkChronics.Checked = true;
                ViewState["chkChronicTF"] = true;
            }
            else
            {
                chkChronics.Checked = false;
                ViewState["chkChronicTF"] = false;
            }

            //ddlDiagnosisStatus.SelectedValue = lblddlStatus.Text;
            string strQualityIds = "";
            string conditionText = "";
            if (lblddlStatus.Text != "0" || lblddlStatus.Text != "")
            {
                txtstatusIds.Text = lblddlStatus.Text;
                ddlDiagnosisStatus.Text = "";
                //checking Quality checkbox
                strQualityIds = lblddlStatus.Text;
                string strQuantityID = "";
                int i, j;
                string[] arInfo = new string[4];
                char[] splitter = { ',' };
                arInfo = strQualityIds.Split(splitter);
                for (i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
                {
                    strQuantityID = ddlDiagnosisStatus.Items[i].Value.ToString().Trim();

                    for (j = 0; j < arInfo.Length; j++)
                    {
                        if (arInfo[j].Trim() == strQuantityID.Trim())
                        {
                            CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                            //ddlDiagnosisStatus.Text += ddlDiagnosisStatus.Items[i].Text.ToString() + ",";

                            conditionText = conditionText + ddlDiagnosisStatus.Items[i].Text.ToString() + ",";
                            checkbox.Checked = true;
                            //chk = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                            //chk.Checked = true;
                            //break;
                        }
                    }
                }


            }


            if (conditionText.Length > 0)
            {

                conditionText = conditionText.Substring(0, conditionText.Length - 1);
            }
            ddlDiagnosisType.SelectedValue = lblddlType.Text;

            ddlProviders.SelectedValue = lblddlProvider.Text;
            if (common.myStr(lblOnsetDate.Text.Trim()) != "")
            {
                rdpOnsetDate.SelectedDate = common.myDate(lblOnsetDate.Text);
            }
            else
            {
                rdpOnsetDate.Clear();
            }
            ddlFacility.SelectedValue = lblddlLocation.Text;
            txtcomments.Text = lblComments.Text.Trim();
            if (ddlDiagnosiss.Text != "")
            {
                btnAddtogrid.Text = "Update List";
            }
            ddlDiagnosisStatus.SelectedItem.Text = conditionText;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //Diagnosis "Cancel"
    protected void btnCancelDeletion_OnClick(object sender, EventArgs e)
    {
        dvConfirmDeletion.Visible = false;
    }
    //Chronic "Cancel"
    protected void btnChronicCancelDeletion_OnClick(object sender, EventArgs e)
    {
        dvChronicConfirmDeletion.Visible = false;
    }
    //Chronic "YES"
    protected void btnChronicDeletion_OnClick(object sender, EventArgs e)
    {

        try
        {

            dvChronicConfirmDeletion.Visible = false;
            //After Yes Button Press ::selected row is deleting
            if (hdnChroniclblId.Value != "0")
            {
                //Hashtable hshInput = new Hashtable();
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //hshInput.Add("@intEncodedBy", common.myInt(Session["UserID"]));
                //hshInput.Add("@intDiagnosisId", common.myInt(hdnChroniclblId.Value.Trim()));
                //hshInput.Add("@intLoginFacilityId", Session["FacilityID"]);
                //hshInput.Add("@intPageId", common.myInt(ViewState["PageId"]));
                //hshInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                //hshInput.Add("@intRegistrationId", common.myInt(Session["RegistrationID"]));
                //hshInput.Add("@intEncounterId", common.myInt(Session["encounterid"]));
                //dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientDiagnosis", hshInput);


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeletePatientDiagnosis";
                APIRootClass.DeletePatientDiagnosis objRoot = new global::APIRootClass.DeletePatientDiagnosis();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                objRoot.EncounterId = common.myInt(Session["encounterid"]);
                objRoot.DiagnosisId = common.myInt(hdnChroniclblId.Value.Trim());
                objRoot.PageId = common.myInt(ViewState["PageId"]);
                objRoot.UserId = common.myInt(Session["UserID"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                //string sQuery = "";
                //sQuery += " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value + "  ";
                //sQuery += " FROM EMRPatientForms epf    ";
                //sQuery += " INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId ";
                //sQuery += " AND epfd.PageId = " + ViewState["PageId"] + "  WHERE epf.EncounterId = " + Convert.ToInt32(Session["encounterid"]) + " ";
                //sQuery += " AND epf.RegistrationId = " + Convert.ToInt32(Session["RegistrationID"]) + "    ";
                //sQuery += " AND epf.Active = 1 ";

                //dl.ExecuteNonQuery(CommandType.Text, sQuery);

                RetrievePatientDiagnosis();
            }
            statusDis = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    //Diagnosis "Yes"
    protected void btnDeletion_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvConfirmDeletion.Visible = false;

            //After Yes Button Press ::selected row is deleting
            if (hdnlblId.Value != "0")
            {
                // Hashtable hshInput = new Hashtable();
                //// DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                // hshInput.Add("@intEncodedBy", common.myInt(Session["UserID"]));
                // hshInput.Add("@intDiagnosisId", common.myInt(hdnlblId.Value.Trim()));
                // hshInput.Add("@intLoginFacilityId", Session["FacilityID"]);
                // hshInput.Add("@intPageId", common.myInt(ViewState["PageId"]));
                // hshInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                // hshInput.Add("@intRegistrationId", common.myInt(Session["RegistrationID"]));
                // hshInput.Add("@intEncounterId", common.myInt(Session["encounterid"]));
                // dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientDiagnosis", hshInput);


                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeletePatientDiagnosis";
                APIRootClass.DeletePatientDiagnosis objRoot = new global::APIRootClass.DeletePatientDiagnosis();

                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.DiagnosisId = common.myInt(hdnlblId.Value.Trim());
                objRoot.FacilityId = common.myInt(Session["FacilityID"]);
                objRoot.PageId = common.myInt(ViewState["PageId"]);
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                objRoot.EncounterId = common.myInt(Session["encounterid"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);



                //string sQuery = "";
                //sQuery += " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value + "  ";
                //sQuery += " FROM EMRPatientForms epf    ";
                //sQuery += " INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId ";
                //sQuery += " AND epfd.PageId = " + ViewState["PageId"] + "  WHERE epf.EncounterId = " + Convert.ToInt32(Session["encounterid"]) + " ";
                //sQuery += " AND epf.RegistrationId = " + Convert.ToInt32(Session["RegistrationID"]) + "    ";
                //sQuery += " AND epf.Active = 1 ";

                //dl.ExecuteNonQuery(CommandType.Text, sQuery);

            }
            RetrievePatientDiagnosis();
            statusDis = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        //End Here
    }

    protected void gvDiagnosisDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable dT = CreateTable();
        try
        {
            if (e.CommandName == "DiagnosisCookie")
            {
                GridViewRow gvr = gvDiagnosisDetails.Rows[common.myInt(e.CommandArgument) - 1];
                Literal ltrlProblemXML = (Literal)gvr.FindControl("lblDiagnosisXML");
                HttpCookie htC = new HttpCookie("DiagnosisCookie");
                htC.Value = ltrlProblemXML.Text;
                htC.Expires = DateTime.Now.AddHours(3);
                Response.Cookies.Add(htC);

                TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");
                TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                Button btnProblemDetails = (Button)gvr.FindControl("btnProblemDetails");
                Literal ltrlXML = (Literal)gvr.FindControl("lblDiagnosisXML");
                string scriptString = "<script language='JavaScript'>pos=window.open('/EMR/Assessment/Diagnosis.aspx?RowIndex=" + gvr.RowIndex.ToString() + "&PN=" + txtDiagnosisName.Text + "&XCtrl=" + ltrlXML.Text + "&PId=" + gvr.Cells[1].Text + "',\"mywindow\", \"menubar=0,resizable=0,width=600,height=300,status=0,toolbars=0\"); pos.moveTo(0,110);if (window.focus) {pos.focus()};</script>";
                if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptString))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", scriptString);
                }
            }
            if (e.CommandName == "Remove")
            {

                DataRow dr;
                if (gvDiagnosisDetails.Rows.Count > 0)
                {
                    //if grid have rows then store it into datatable
                    foreach (GridViewRow gvr in gvDiagnosisDetails.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            if (e.CommandArgument.ToString() != HttpUtility.HtmlDecode(gvr.Cells[0].Text.ToString().Trim()))
                            {
                                dr = dT.NewRow();
                                String sDiagnosisId = gvr.Cells[0].Text.ToString().Trim();
                                TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");
                                TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                                TextBox txtOnsetDate = (TextBox)gvr.FindControl("txtOnsetDate");
                                DropDownList ddlSide = (DropDownList)gvr.FindControl("ddlSide");
                                CheckBox chkPrimary = (CheckBox)gvr.FindControl("chkPrimary");
                                CheckBox chkChronic = (CheckBox)gvr.FindControl("chkChronic");

                                DropDownList ddlStatus = (DropDownList)gvr.FindControl("StatusId");
                                DropDownList ddlType = (DropDownList)gvr.FindControl("TypeId");
                                CheckBox chkResolved = (CheckBox)gvr.FindControl("IsResolved");

                                DropDownList ddlProvider = (DropDownList)gvr.FindControl("DoctorId");
                                DropDownList ddlLocation = (DropDownList)gvr.FindControl("FacilityId");
                                TextBox txtComments = (TextBox)gvr.FindControl("txtComments");
                                Label lblId = (Label)gvr.FindControl("lblId");

                                dr["ICDID"] = gvr.Cells[1].Text;
                                dr["IcdCode"] = txtIcdCode.Text;
                                dr["ICDDescription"] = txtDiagnosisName.Text;
                                dr["OnsetDate"] = txtOnsetDate.Text;
                                dr["OnsetDateWithoutFormat"] = txtOnsetDate.Text;

                                dr["LocationId"] = ddlSide.SelectedValue;
                                dr["PrimaryDiagnosis"] = chkPrimary.Checked;
                                dr["IsChronic"] = chkChronic.Checked;

                                dr["ConditionIds"] = txtstatusIds.Text; //ddlStatus.SelectedValue;
                                dr["TypeId"] = ddlType.SelectedValue;
                                dr["IsResolved"] = chkResolved.Checked;

                                dr["DoctorId"] = ddlProvider.SelectedValue;
                                dr["FacilityId"] = ddlLocation.SelectedValue;
                                dr["Remarks"] = txtComments.Text;
                                dr["Id"] = lblId.Text;
                                dT.Rows.Add(dr);
                            }
                        }
                    }

                }
                gvDiagnosisDetails.DataSource = dT;
                gvDiagnosisDetails.DataBind();
                if (gvDiagnosisDetails.Rows.Count == 0)
                {
                    BindBlankDiagnosisDetailGrid();
                }
            }
            if (e.CommandName == "Search")
            {
                foreach (GridViewRow gvr in gvDiagnosisDetails.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        //string txtIcdcode = ((TextBox)(gvDiagnosisDetails.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].FindControl("txtIcdCode"))).Text;
                        string txtIcdcode = ((TextBox)(gvr.Cells[2].FindControl("txtIcdCode"))).Text;

                        //DataSet objDs = objDiag.selectDiscription(txtIcdcode);

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiagnosisDescription";
                        APIRootClass.GetDiagnosisDescription objRoot = new global::APIRootClass.GetDiagnosisDescription();
                        objRoot.IcdCode = txtIcdcode;

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                        //string cmdstr = "SELECT [Description] FROM ICD9SubDisease WHERE [ICDCode]='" + txtIcdcode + "'";
                        //DataSet objDs = (DataSet)objDl.FillDataSet(CommandType.Text, cmdstr);

                        //TextBox txtDesc = (TextBox)(gvDiagnosisDetails.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].FindControl("txtDiagnosisName"));
                        TextBox txtDesc = (TextBox)(gvr.Cells[2].FindControl("txtDiagnosisName"));
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            //DataTable tbl = objDs.Tables[0];
                            txtDesc.Text = objDs.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            BindBlankGrid();
                        }
                    }
                }
            }

            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                //if (rblShowNote.SelectedIndex == -1)
                //{
                //    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                //    return;
                //}

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblId = (Label)gvDiagnosisDetails.Rows[row.RowIndex].FindControl("lblId");
                hdnlblId.Value = lblId.Text;
                if (!string.IsNullOrEmpty(lblId.Text))
                {
                    dvConfirmDeletion.Visible = true;
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
            objDs.Dispose();
            dT.Dispose();
        }
    }

    protected void gvChronicDiagnosis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable tbl = new DataTable();
        DataTable dT = CreateTable();
        try
        {
            if (e.CommandName == "ChronicDiagnosisCookie")
            {
                GridViewRow gvr = gvChronicDiagnosis.Rows[common.myInt(e.CommandArgument) - 1];
                Literal ltrlProblemXML = (Literal)gvr.FindControl("lblDiagnosisXML");
                HttpCookie htC = new HttpCookie("ChronicDiagnosisCookie");
                htC.Value = ltrlProblemXML.Text;
                htC.Expires = DateTime.Now.AddHours(3);
                Response.Cookies.Add(htC);

                TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");
                TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                Button btnProblemDetails = (Button)gvr.FindControl("btnProblemDetails");
                Literal ltrlXML = (Literal)gvr.FindControl("lblDiagnosisXML");
                string scriptString = "<script language='JavaScript'>pos=window.open('/EMR/Assessment/Diagnosis.aspx?RowIndex=" + gvr.RowIndex.ToString() + "&PN=" + txtDiagnosisName.Text + "&XCtrl=" + ltrlXML.Text + "&PId=" + gvr.Cells[1].Text + "',\"mywindow\", \"menubar=0,resizable=0,width=600,height=300,status=0,toolbars=0\"); pos.moveTo(0,110);if (window.focus) {pos.focus()};</script>";
                if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptString))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", scriptString);
                }
            }
            if (e.CommandName == "Remove")
            {

                DataRow dr;
                if (gvDiagnosisDetails.Rows.Count > 0)
                {
                    //if grid have rows then store it into datatable
                    foreach (GridViewRow gvr in gvChronicDiagnosis.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            if (e.CommandArgument.ToString() != HttpUtility.HtmlDecode(gvr.Cells[0].Text.ToString().Trim()))
                            {
                                dr = dT.NewRow();
                                String sDiagnosisId = gvr.Cells[0].Text.ToString().Trim();
                                TextBox txtIcdCode = (TextBox)gvr.FindControl("txtIcdCode");
                                TextBox txtDiagnosisName = (TextBox)gvr.FindControl("txtDiagnosisName");
                                TextBox txtOnsetDate = (TextBox)gvr.FindControl("txtOnsetDate");
                                DropDownList ddlSide = (DropDownList)gvr.FindControl("ddlSide");
                                CheckBox chkPrimary = (CheckBox)gvr.FindControl("chkPrimary");
                                CheckBox chkChronic = (CheckBox)gvr.FindControl("chkChronic");

                                DropDownList ddlStatus = (DropDownList)gvr.FindControl("StatusId");
                                DropDownList ddlType = (DropDownList)gvr.FindControl("TypeId");
                                CheckBox chkResolved = (CheckBox)gvr.FindControl("IsResolved");

                                DropDownList ddlProvider = (DropDownList)gvr.FindControl("DoctorId");
                                DropDownList ddlLocation = (DropDownList)gvr.FindControl("FacilityId");
                                TextBox txtComments = (TextBox)gvr.FindControl("txtComments");
                                Label lblId = (Label)gvr.FindControl("lblId");

                                dr["ICDID"] = gvr.Cells[1].Text;
                                dr["IcdCode"] = txtIcdCode.Text;
                                dr["ICDDescription"] = txtDiagnosisName.Text;
                                dr["OnsetDate"] = txtOnsetDate.Text;
                                dr["LocationId"] = ddlSide.SelectedValue;
                                dr["PrimaryDiagnosis"] = chkPrimary.Checked;
                                dr["IsChronic"] = chkChronic.Checked;

                                dr["ConditionIds"] = txtstatusIds.Text; //ddlStatus.SelectedValue;
                                dr["TypeId"] = ddlType.SelectedValue;
                                dr["IsResolved"] = chkResolved.Checked;

                                dr["DoctorId"] = ddlProvider.SelectedValue;
                                dr["FacilityId"] = ddlLocation.SelectedValue;
                                dr["Remarks"] = txtComments.Text;
                                dr["Id"] = lblId.Text;
                                dT.Rows.Add(dr);
                            }
                        }
                    }
                }
                gvChronicDiagnosis.DataSource = dT;
                gvChronicDiagnosis.DataBind();
                if (gvChronicDiagnosis.Rows.Count == 0)
                {
                    BindBlankDiagnosisDetailGrid();
                }
            }
            if (e.CommandName == "Search")
            {
                foreach (GridViewRow gvr in gvChronicDiagnosis.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        string txtIcdcode = ((TextBox)(gvr.Cells[2].FindControl("txtIcdCode"))).Text;

                        //BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
                        //DataSet objDs = #nosis.selectICDSubDiseas(txtIcdcode);

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiagnosisDescription";
                        APIRootClass.GetDiagnosisDescription objRoot = new global::APIRootClass.GetDiagnosisDescription();
                        objRoot.IcdCode = txtIcdcode;

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                        TextBox txtDesc = (TextBox)(gvr.Cells[2].FindControl("txtDiagnosisName"));
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            tbl = objDs.Tables[0];
                            txtDesc.Text = tbl.Rows[0][0].ToString();
                        }
                        else
                        {
                            BindBlankGrid();
                        }
                    }
                }
            }

            if (e.CommandName == "Del")
            {
                //if (rblShowNote.SelectedIndex == -1)
                //{
                //    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                //    return;
                //}

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblId = (Label)gvChronicDiagnosis.Rows[row.RowIndex].FindControl("lblId");
                hdnChroniclblId.Value = lblId.Text;

                if (!string.IsNullOrEmpty(lblId.Text))
                {

                    dvChronicConfirmDeletion.Visible = true;
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
            objDs.Dispose();
            tbl.Dispose();
            dT.Dispose();
        }
    }

    #endregion

    protected void btnExtenalEdu_Click(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        try
        {
            if (txtIcdCodes.Text.Trim() != "" && txtIcdCodes.Text.Trim() != "0")
            {
                if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
                {
                    #region Log for each encounter if the diagnosis(Education Meterial), order(Education Meterial), lab result or medication(MonographButton) print button is pressed
                    //Hashtable logHash = new Hashtable();
                    //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //logHash.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                    //logHash.Add("@intRegistrationId", common.myInt(Session["RegistrationID"]));
                    //logHash.Add("@intEncounterId", common.myInt(Session["encounterid"]));
                    //logHash.Add("@intDoctorId", common.myInt(Session["DoctorID"]));
                    //logHash.Add("@intEncodedBy", common.myInt(Session["UserID"]));
                    //objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogEducationAndMonograph", logHash);

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/LogEducationAndMonograph";
                    APIRootClass.LogEducationAndMonograph objRoot = new global::APIRootClass.LogEducationAndMonograph();
                    objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                    objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                    objRoot.EncounterId = common.myInt(Session["encounterid"]);
                    objRoot.UserId = common.myInt(Session["UserID"]);

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


                    #endregion
                }

                string url = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.103&mainSearchCriteria.v.c=" + txtIcdCodes.Text.Trim() + "";
                string fullURL = "window.open('" + url + "', '_blank', 'status=yes,toolbar=yes,menubar=yes,location=no,scrollbars=yes,resizable=yes,titlebar=yes' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


            }
            else
            {
                Alert.ShowAjaxMsg("Please select Diagnosis", Page);
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
            objDs.Dispose();
        }
    }



    #region Bind Blank Grid section

    private void BindBlankDiagnosisDetailGrid()
    {
        DataTable dT = CreateTable();
        try
        {

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                dr["ICDCode"] = "";
                dr["ICDDescription"] = "";
                dr["OnsetDate"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "0";
                dr["IsQuery"] = "0";
                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";
                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "";
                dT.Rows.Add(dr);
            }
            gvDiagnosisDetails.DataSource = dT;
            gvDiagnosisDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    private void BindBlankChronicDiagnosisGrid()
    {
        DataTable dT = CreateTable();
        try
        {

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                dr["ICDCode"] = "";
                dr["ICDDescription"] = "";
                dr["OnsetDate"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "0";
                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";
                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "";
                dr["TemplateFieldId"] = "";
                dT.Rows.Add(dr);
            }
            gvChronicDiagnosis.DataSource = dT;
            gvChronicDiagnosis.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    //yogesh 28-09-2022
    private void BindBlankProvisionalDiagnosisGrid()
    {
        DataTable dT = CreateTable();
        try
        {

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                dr["ICDCode"] = "";
                dr["ICDDescription"] = "";
                dr["OnsetDate"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "0";
                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";
                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "";
                dr["TemplateFieldId"] = "";
                dT.Rows.Add(dr);
            }
            gvProvisionalDiag.DataSource = dT;
            gvProvisionalDiag.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    //yogesh 28-09-2022
    private void BindBlankResolvedDiagnosisGrid()
    {
        DataTable dT = CreateTable();
        try
        {

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                dr["ICDCode"] = "";
                dr["ICDDescription"] = "";
                dr["OnsetDate"] = "";
                dr["LocationId"] = "0";
                dr["PrimaryDiagnosis"] = "";
                dr["IsChronic"] = "0";
                dr["ConditionIds"] = "0";
                dr["TypeId"] = "0";
                dr["IsResolved"] = "0";
                dr["DoctorId"] = "0";
                dr["FacilityId"] = "0";
                dr["Remarks"] = "";
                dr["Id"] = "";
                dr["TemplateFieldId"] = "";
                dT.Rows.Add(dr);
            }
            ResolvedGrid.DataSource = dT;
            ResolvedGrid.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dT.Dispose();
        }
    }

    private DataTable BindBlankGrid()
    {
        DataTable dT = new DataTable();
        try
        {

            dT.Columns.Add("ICDID");
            dT.Columns.Add("ICDCode");
            dT.Columns.Add("ICDDescription");
            dT.Columns.Add("Id");
            dT.Columns.Add("EncounterDate");
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDID"] = 0;
                if (Convert.ToString(ViewState["BTN"]) == "ALL")
                {
                    dr["ICDDescription"] = "No Data Found";

                }

                else if (Convert.ToString(ViewState["BTN"]) == "FAV")
                {
                    dr["ICDDescription"] = "No Favorite Found";

                }
                else
                {
                    dr["ICDDescription"] = "No Data Found";

                }
                dT.Rows.Add(dr);
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dT;
    }

    #endregion

    #region Bind DropdownBox section

    protected void BindProvider()
    {
        DataSet ds = new DataSet();
        try
        {
            //BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
            //ds = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, 0, 0);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/getDoctorList";
            APIRootClass.getDoctorList objRoot = new global::APIRootClass.getDoctorList();
            objRoot.DoctorId = 0;
            objRoot.DoctorName = "";
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            objRoot.SpecialisationId = 0;
            objRoot.FacilityId = 0;
            objRoot.IsMedicalProvider = 0;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            ddlProviders.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlProviders.DataSource = ds;
                ddlProviders.DataValueField = "DoctorId";
                ddlProviders.DataTextField = "DoctorName";
                ddlProviders.DataBind();
            }
            ddlProviders.Items.Insert(0, new RadComboBoxItem("Select", ""));
            RadComboBoxItem rcbDoctorId = (RadComboBoxItem)ddlProviders.Items.FindItemByValue(Convert.ToString(Session["EmployeeId"]));
            if (rcbDoctorId != null)
                rcbDoctorId.Selected = true;
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

    protected void BindFacility()
    {
        DataSet ds = new DataSet();
        try
        {
            //BaseC.EMRProblems objProb = new BaseC.EMRProblems(sConString);

            //ds = objProb.GetFacility(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), common.myInt(Session["GroupID"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/Common/getFacilityList";

            APIRootClass.getFacilityList objRoot = new global::APIRootClass.getFacilityList();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.GroupID = common.myInt(Session["GroupID"]);
            objRoot.EncodedBy = 0;
            objRoot.FacilityType = "O";

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            ddlFacility.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlFacility.DataSource = ds;
                ddlFacility.DataValueField = "FacilityID";
                ddlFacility.DataTextField = "FacilityName";
                ddlFacility.DataBind();
            }
            //ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            RadComboBoxItem rcbFacilityId = (RadComboBoxItem)ddlFacility.Items.FindItemByText(Convert.ToString(Session["Facility"]));
            if (rcbFacilityId != null)
                rcbFacilityId.Selected = true;
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

    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();
    }

    protected void BindDiagnosisType()
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindDiagnosistype";


            string inputJson = (new JavaScriptSerializer()).Serialize(null);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objDiag.BindDiagnosistype();
            ddlDiagnosisType.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDiagnosisType.DataSource = ds;
                ddlDiagnosisType.DataValueField = "TypeId";
                ddlDiagnosisType.DataTextField = "Description";
                ddlDiagnosisType.DataBind();
            }
            //ddlProviders.Items.Insert(0, new ListItem("Select", "0"));
            ddlDiagnosisType.Items.Insert(0, new RadComboBoxItem("", "0"));
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

    protected void BindDiagnosisStatus()
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);

            //ds = objDiag.BindDiagnosisCondition();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindDiagnosisCondition";


            string inputJson = (new JavaScriptSerializer()).Serialize(null);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            ddlDiagnosisStatus.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDiagnosisStatus.DataSource = ds;
                ddlDiagnosisStatus.DataValueField = "StatusId";
                ddlDiagnosisStatus.DataTextField = "Description";
                ddlDiagnosisStatus.DataBind();
            }
            //ddlProviders.Items.Insert(0, new ListItem("Select", "0"));
            ddlDiagnosisStatus.Items.Insert(0, new RadComboBoxItem("", "0"));
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

    #endregion

    #region Bind Gridview section both Today's Diagnosis and Chronic

    void BindDiagnosisDetails()
    {
        DataTable DT = new DataTable();
        try
        {
            lblMessage.Text = "";
            DT = (DataTable)Cache["DignosisDetails"];
            if (DT == null)
            {
                DT = CreateTable();
            }


            if (hdnDiagnosisId.Value == "")
            {
                hdnDiagnosisId.Value = ViewState["hdnDiagnosisId"].ToString();
            }

            DataRow[] datarow = DT.Select("ICDID=" + hdnDiagnosisId.Value);
            if (datarow.Length > 0)
            {
                datarow[0].BeginEdit();
                datarow[0]["ICDID"] = hdnDiagnosisId.Value;
                datarow[0]["IcdCode"] = txtIcdCodes.Text.Trim();
                datarow[0]["ICDDescription"] = ddlDiagnosiss.Text.Trim();

                datarow[0]["OnsetDate"] = rdpOnsetDate.SelectedDate;
                datarow[0]["LocationId"] = ddlSides.SelectedValue;
                datarow[0]["PrimaryDiagnosis"] = chkPrimarys.Checked;
                datarow[0]["IsChronic"] = chkChronics.Checked;
                datarow[0]["IsFinalDiagnosis"] = chkIsFinalDiagnosis.Checked;

                datarow[0]["ConditionIds"] = txtstatusIds.Text.Trim(); //ddlDiagnosisStatus.SelectedValue;
                datarow[0]["TypeId"] = ddlDiagnosisType.SelectedValue;
                datarow[0]["IsResolved"] = chkResolve.Checked;

                datarow[0]["DoctorId"] = ddlProviders.SelectedValue;
                datarow[0]["FacilityId"] = ddlFacility.SelectedValue;
                datarow[0]["Remarks"] = txtcomments.Text.Trim();
                datarow[0].EndEdit();
            }
            else
            {
                DataRow dr;
                dr = DT.NewRow();
                dr["ICDID"] = hdnDiagnosisId.Value;
                dr["IcdCode"] = txtIcdCodes.Text.Trim();
                dr["ICDDescription"] = ddlDiagnosiss.Text.Trim();
                dr["OnsetDate"] = rdpOnsetDate.SelectedDate;
                dr["LocationId"] = ddlSides.SelectedValue;
                dr["PrimaryDiagnosis"] = chkPrimarys.Checked;
                dr["IsChronic"] = chkChronics.Checked;
                dr["IsFinalDiagnosis"] = chkIsFinalDiagnosis.Checked;

                dr["ConditionIds"] = txtstatusIds.Text.Trim(); // ddlDiagnosisStatus.SelectedValue;
                dr["TypeId"] = ddlDiagnosisType.SelectedValue;
                dr["IsResolved"] = chkResolve.Checked;

                dr["DoctorId"] = ddlProviders.SelectedValue;
                dr["FacilityId"] = ddlFacility.SelectedValue;
                dr["Remarks"] = txtcomments.Text;
                dr["Id"] = "0";
                DT.Rows.Add(dr);
            }
            Cache.Insert("DignosisDetails", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            gvDiagnosisDetails.DataSource = DT;
            gvDiagnosisDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            DT.Dispose();
        }
    }

    void BindChronicDiagnosisDetails()
    {
        DataTable DT = new DataTable();
        try
        {
            lblMessage.Text = "";
            DT = (DataTable)Cache["ChronicDignosisDetails"];
            if (DT == null)
            {
                DT = CreateTable();
            }

            if (hdnDiagnosisId.Value == "")
            {
                hdnDiagnosisId.Value = ViewState["hdnDiagnosisId"].ToString();
            }

            DataRow[] datarow = DT.Select("ICDID=" + hdnDiagnosisId.Value);
            if (datarow.Length > 0)
            {
                datarow[0].BeginEdit();
                datarow[0]["ICDID"] = hdnDiagnosisId.Value;
                datarow[0]["IcdCode"] = txtIcdCodes.Text.Trim();
                datarow[0]["ICDDescription"] = ddlDiagnosiss.Text.Trim();
                datarow[0]["OnsetDate"] = rdpOnsetDate.SelectedDate;
                datarow[0]["LocationId"] = ddlSides.SelectedValue;
                datarow[0]["PrimaryDiagnosis"] = chkPrimarys.Checked;
                datarow[0]["IsChronic"] = chkChronics.Checked;
                datarow[0]["IsFinalDiagnosis"] = chkIsFinalDiagnosis.Checked;

                datarow[0]["ConditionIds"] = txtstatusIds.Text; //ddlDiagnosisStatus.SelectedValue;
                datarow[0]["TypeId"] = ddlDiagnosisType.SelectedValue;
                datarow[0]["IsResolved"] = chkResolve.Checked;

                datarow[0]["DoctorId"] = ddlProviders.SelectedValue;
                datarow[0]["FacilityId"] = ddlFacility.SelectedValue;
                datarow[0]["Remarks"] = txtcomments.Text.Trim();
                datarow[0].EndEdit();
            }
            else
            {
                DataRow dr;
                dr = DT.NewRow();
                dr["ICDID"] = hdnDiagnosisId.Value;
                dr["IcdCode"] = txtIcdCodes.Text.Trim();
                dr["ICDDescription"] = ddlDiagnosiss.Text.Trim();
                dr["OnsetDate"] = rdpOnsetDate.SelectedDate;
                dr["LocationId"] = ddlSides.SelectedValue;
                dr["PrimaryDiagnosis"] = chkPrimarys.Checked;
                dr["IsChronic"] = chkChronics.Checked;
                dr["IsFinalDiagnosis"] = chkIsFinalDiagnosis.Checked;

                dr["ConditionIds"] = txtstatusIds.Text.Trim();  //ddlDiagnosisStatus.SelectedValue;
                dr["TypeId"] = ddlDiagnosisType.SelectedValue;
                dr["IsResolved"] = chkResolve.Checked;

                dr["DoctorId"] = ddlProviders.SelectedValue;
                dr["FacilityId"] = ddlFacility.SelectedValue;
                dr["Remarks"] = txtcomments.Text;
                dr["Id"] = "0";
                DT.Rows.Add(dr);
            }
            Cache.Insert("ChronicDignosisDetails", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            gvChronicDiagnosis.DataSource = DT;
            gvChronicDiagnosis.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            DT.Dispose();
        }
    }

    #endregion

    #region AddToList AddToToday Section

    protected void btnAddtogrid_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);
            StringBuilder strXMLAleart = new StringBuilder();
            ArrayList coll = new ArrayList();

            //DataSet ds = objDiag.CheckDiagnosisExcluded(common.myInt(Session["HospitalLocationId"]),
            //                        common.myInt(Session["Facilityid"]), common.myInt(Session["RegistrationId"]), txtIcdCodes.Text);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckDiagnosisExcluded";
            APIRootClass.CheckDiagnosisExcluded objRoot = new global::APIRootClass.CheckDiagnosisExcluded();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.Facilityid = common.myInt(Session["Facilityid"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.IcdCode = txtIcdCodes.Text;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divExcludedService.Visible = true;
                    return;
                }
            }
            saveData();
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
    public void saveData()
    {
        StringBuilder strXMLAleart = new StringBuilder();
        ArrayList coll = new ArrayList();
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
        DataSet dsprimary = new DataSet();
        DataSet ds = new DataSet();
        try
        {
            if (ddlDiagnosiss.Text.Trim() == "")
            {
                hdnIsUnSavedData.Value = "0";
                return;
            }
            if (btnAddtogrid.Text == "Add To List")
            {
                if (common.myInt(ddlDiagnosiss.SelectedValue) == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "";
                    hdnIsUnSavedData.Value = "0";
                    Alert.ShowAjaxMsg("Please Select Diagnosis !", this.Page);
                    return;
                }
            }

            //if (rblShowNote.SelectedIndex == -1)
            //{
            //    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
            //    return;
            //}

            object ob;
            //DateTime dt=new DateTime();
            string dt = "";

            if (rdpOnsetDate.SelectedDate != null)
            {
                dt = common.myDate(rdpOnsetDate.SelectedDate).ToString("yyyy-MM-dd");

                ob = dt;
            }
            else
            {
                ob = DBNull.Value;
            }
            if (btnAddtogrid.Text != "Update List")
            {
                if (gvDiagnosisDetails.Rows.Count == 1)
                {
                    chkPrimarys.Checked = true;
                }
            }
            //string strSQL = "";
            //Hashtable hash2 = new Hashtable();

            //hash2.Add("@inticdid", hdnDiagnosisId.Value);
            //hash2.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            //hash2.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"]));
            //BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
            if (chkPrimarys.Checked)
            {
                //DataSet dsprimary = ObjDiagnosis.SelectDiagnosispatientdtl(common.myInt(hdnDiagnosisId.Value), common.myInt(Session["RegistrationID"]),
                //                                common.myInt(Session["EncounterID"]));
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckPatientPrimaryDiagnosis";
                APIRootClass.CheckPatientPrimaryDiagnosis objRoot = new global::APIRootClass.CheckPatientPrimaryDiagnosis();
                objRoot.DiagnosisId = common.myInt(hdnDiagnosisId.Value);
                objRoot.EncounterId = common.myInt(Session["EncounterID"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);


                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dsprimary = JsonConvert.DeserializeObject<DataSet>(sValue);
                //sikandar
                if (dsprimary.Tables[0].Rows.Count > 0)
                {
                    if (!common.myInt(hdnDiagnosisId.Value).Equals(common.myInt(dsprimary.Tables[0].Rows[0]["icdid"])) || chkPrimarys.Checked == true)
                    {
                        //Alert.ShowAjaxMsg("Duplicate data add...", Page);
                        //lblMessage.Text = "Only One ICD can be Primary for Visit.";
                        //Alert.ShowAjaxMsg("Only One ICD can be Primary for Visit.", Page);
                        chkPrimarys.Checked = false;
                        //return;
                    }
                }
            }

            if (ddlDiagnosiss.SelectedValue != "" || hdnDiagnosisId.Value != "0" || hdnDiagnosisId.Value != "")
            {
                //bool bResult = objDiag.CheckValidForPrimaryDiagnosis(common.myInt(ddlDiagnosiss.SelectedValue == "" ? hdnDiagnosisId.Value : ddlDiagnosiss.SelectedValue));
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckValidForPrimaryDiagnosis";
                APIRootClass.CheckValidForPrimaryDiagnosis objRoot = new global::APIRootClass.CheckValidForPrimaryDiagnosis();
                objRoot.DiagnosisId = common.myInt(ddlDiagnosiss.SelectedValue == "" ? hdnDiagnosisId.Value : ddlDiagnosiss.SelectedValue);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                bool bResult = JsonConvert.DeserializeObject<bool>(sValue);

                if (bResult == false && chkPrimarys.Checked == true)
                {
                    hdnIsUnSavedData.Value = "0";
                    Alert.ShowAjaxMsg("This ICD is not valid for Primary Diagnosis", Page);
                    return;
                }
            }

            if (btnAddtogrid.Text == "Update List")
            {
                if (chkChronics.Checked)
                {
                    foreach (GridViewRow gv in gvChronicDiagnosis.Rows)
                    {
                        if (common.myInt(((Label)gv.FindControl("lblId")).Text) != common.myInt(txtid.Text))
                        {
                            if (common.myStr(txtIcdId.Text) == common.myStr(((Label)gv.FindControl("lblIcdId")).Text))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                                lblMessage.Text = "This  (" + ddlDiagnosiss.Text.Trim() + ")  already exists in Chronics Diagnosis!";
                                hdnIsUnSavedData.Value = "0";
                                return;
                            }
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow gv in gvDiagnosisDetails.Rows)
                    {
                        if (common.myInt(((Label)gv.FindControl("lblId")).Text) != common.myInt(txtid.Text))
                        {
                            if (common.myStr(txtIcdId.Text) == common.myStr(((Label)gv.FindControl("lblIcdId")).Text))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "This  (" + ddlDiagnosiss.Text.Trim() + ")  already exists in Today's Diagnosis!";
                                hdnIsUnSavedData.Value = "0";
                                return;
                            }
                        }
                    }
                }
            }
            StringBuilder objXML = new StringBuilder();

            coll = new ArrayList();

            coll.Add(common.ParseString(txtid.Text.Trim()));
            coll.Add(common.myInt(hdnDiagnosisId.Value));
            coll.Add(chkPrimarys.Checked);
            coll.Add(chkChronics.Checked);
            coll.Add(chkQuery.Checked);
            coll.Add(ob);
            coll.Add(ddlSides.SelectedValue);
            coll.Add((common.myInt(ddlProviders.SelectedValue) > 0) ? common.myStr(ddlProviders.SelectedValue) : string.Empty);
            coll.Add(ddlFacility.SelectedValue);
            coll.Add(common.ParseString(txtcomments.Text.Trim()));
            coll.Add(txtstatusIds.Text);
            coll.Add(ddlDiagnosisType.SelectedValue);
            coll.Add(chkResolve.Checked);
            coll.Add(Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate" ? Request.QueryString["TemplateFieldId"].ToString() : "0");
            coll.Add(chkIsFinalDiagnosis.Checked);

            objXML.Append(common.setXmlTable(ref coll));

            //objXML.Append("<Table1><c1>");
            //objXML.Append(common.ParseString(txtid.Text.Trim()));
            //objXML.Append("</c1><c2>");
            //// objXML.Append(common.ParseString(txtIcdId.Text.Trim()));hdnDiagnosisId.Value
            //objXML.Append(common.myInt(hdnDiagnosisId.Value));
            //objXML.Append("</c2><c3>");
            //objXML.Append(chkPrimarys.Checked);
            //objXML.Append("</c3><c4>");
            //objXML.Append(chkChronics.Checked);
            //objXML.Append("</c4><c5>");
            //objXML.Append(chkQuery.Checked);
            //objXML.Append("</c5><c6>");
            //objXML.Append(ob);
            //objXML.Append("</c6><c7>");
            //objXML.Append(ddlSides.SelectedValue);
            //objXML.Append("</c7><c8>");
            //if (common.myInt(ddlProviders.SelectedValue) != 0)
            //{
            //    objXML.Append(ddlProviders.SelectedValue);
            //}
            //else
            //{
            //    objXML.Append(DBNull.Value);
            //}
            //objXML.Append("</c8><c9>");
            //objXML.Append(ddlFacility.SelectedValue);
            //objXML.Append("</c9><c10>");
            //objXML.Append(common.ParseString(txtcomments.Text.Trim()));
            //objXML.Append("</c10><c11>");
            //objXML.Append(txtstatusIds.Text);
            //objXML.Append("</c11><c12>");
            //objXML.Append(ddlDiagnosisType.SelectedValue);
            //objXML.Append("</c12><c13>");
            //objXML.Append(chkResolve.Checked);
            //objXML.Append("</c13><c14>");
            //objXML.Append(Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate" ? Request.QueryString["TemplateFieldId"].ToString() : "0");
            //objXML.Append("</c14><c15>");
            //objXML.Append(chkIsFinalDiagnosis.Checked);
            //objXML.Append("</c15></Table1>");

            //ds = objDiag.CheckDuplicateProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
            //            common.myInt(Session["EncounterId"]), common.myInt(hdnDiagnosisId.Value), common.myBool(chkChronics.Checked));
            WebClient client1 = new WebClient();
            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/CheckDuplicateDiagnosis";
            APIRootClass.CheckDuplicateDiagnosis objRoot1 = new global::APIRootClass.CheckDuplicateDiagnosis();
            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot1.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot1.DiagnosisId = common.myInt(hdnDiagnosisId.Value);
            objRoot1.IsChronic = chkChronics.Checked;

            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (btnAddtogrid.Text == "Add To List")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (hdnDiagnosisId.Value.ToString().Trim() == ds.Tables[0].Rows[i]["icdid"].ToString().Trim())
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            if (chkChronics.Checked == true)
                            {
                                lblMessage.Text = "This  (" + ddlDiagnosiss.Text.Trim() + ")  already exists in Chronics Diagnosis!";
                            }
                            else
                            {
                                lblMessage.Text = "This  (" + ddlDiagnosiss.Text.Trim() + ")  already exists in Today's Diagnosis!";
                            }
                            hdnIsUnSavedData.Value = "0";
                            return;
                        }
                    }
                }
            }

            //foreach (RadComboBoxItem item in ddlPatientAlert.Items)
            //{
            //    if (item.Checked)
            //    {
            //        coll.Add(common.myInt(item.Value));//AlertId SMALLINT

            //        strXMLAleart.Append(common.setXmlTable(ref coll));
            //    }
            //}

            string doctorid = null;
            if (common.myStr(Session["DoctorID"]) != "")
            {
                doctorid = common.myStr(Session["DoctorID"]);
            }

            string strsave = "";
            //strsave = objDiag.EMRSavePatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //                            common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), doctorid,
            //                            common.myInt(ViewState["PageId"]), objXML.ToString(), strXMLAleart.ToString(), common.myInt(Session["UserId"]),
            //                            chkPullDiagnosis.Checked, false, 0);


            client1 = new WebClient();
            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientDiagnosis";
            APIRootClass.SavePatientDiagnosis objRoot2 = new global::APIRootClass.SavePatientDiagnosis();
            objRoot2.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot2.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot2.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot2.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot2.DoctorId = doctorid;
            objRoot2.PageId = common.myInt(ViewState["PageId"]);
            objRoot2.DiagnosisXML = objXML.ToString();
            objRoot2.PatientAlertXML = strXMLAleart.ToString();
            objRoot2.UserId = common.myInt(Session["UserId"]);
            objRoot2.IsPullDiagnosis = chkPullDiagnosis.Checked;
            objRoot2.IsShowNote = false;
            objRoot2.MRDCode = 0;

            inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot2);
            sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            strsave = JsonConvert.DeserializeObject<string>(sValue1);



            ///Tagging Static Template with Template Field
            if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
            {
                //BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                //Hashtable hshOut = emr.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                //    common.myInt(Request.QueryString["SectionId"].ToString()), common.myInt(Request.QueryString["TemplateFieldId"].ToString()),
                //    Request.QueryString["StaticTemplateId"] != null ? common.myInt(Request.QueryString["StaticTemplateId"]) : 133, common.myInt(Session["UserId"]));

                client1 = new WebClient();
                client1.Headers["Content-type"] = "application/json";
                client1.Encoding = Encoding.UTF8;
                ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/TaggingStaticTemplateWithTemplateField";
                APIRootClass.TaggingStaticTemplateWithTemplateField objRoot3 = new global::APIRootClass.TaggingStaticTemplateWithTemplateField();
                objRoot3.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot3.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot3.SectionId = common.myInt(Request.QueryString["SectionId"]);
                objRoot3.FieldId = common.myInt(Request.QueryString["TemplateFieldId"]);
                objRoot3.TemplateId = Request.QueryString["StaticTemplateId"] != null ? common.myInt(Request.QueryString["StaticTemplateId"]) : 133;
                objRoot3.UserId = common.myInt(Session["UserId"]);

                inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot3);
                sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                strsave = JsonConvert.DeserializeObject<string>(sValue1);
            }
            ///end

            if (strsave.Contains("Data Saved!"))
            {
                //if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                //{
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                //}

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strsave;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strsave;
            }
            txtid.Text = "";
            ClearDiagnosisDetailsControls();
            hdnIsUnSavedData.Value = "0";
            btnAddtogrid.Text = "Add To List";
            txtstatusIds.Text = "";
            for (int i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlDiagnosisStatus.SelectedItem.Text = "";
            //BindPatientAlert();
            RetrievePatientDiagnosis();


            txtIcdCodes.Text = "";
            ddlDiagnosiss.Text = "";
            this.ddlDiagnosiss.Enabled = true;
            ddlDiagnosiss.Text = string.Empty;
            statusDis = true;

            #region In each encounter, log once when a diagnosis is saved

            //if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
            //{
            //    //objDiag.EMRMUDLogSaveDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["DoctorID"]), common.myInt(Session["UserID"]));
            //}
            #endregion
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnChronicToToday_Click(object sender, EventArgs e)
    {
        DataTable dtch = new DataTable();
        DataTable dtp = new DataTable();
        try
        {

            ArrayList checkedItems = new ArrayList();
            string chkBoxIndex = string.Empty;

            dtch = (DataTable)Cache["ChronicDignosisDetails"];
            dtp = (DataTable)Cache["DignosisDetails"];
            if (dtch == null)
            {
                return;
            }
            if (dtp == null)
            {
                dtp = CreateTable();
            }

            DataRow[] dr = null;
            if (gvChronicDiagnosis.Rows.Count > 0)
            {
                for (int i = 0; i < gvChronicDiagnosis.Rows.Count; i++)
                {
                    Label lblIcdId = (Label)gvChronicDiagnosis.Rows[i].FindControl("lblIcdId");
                    CheckBox chkSelectChronic = (CheckBox)gvChronicDiagnosis.Rows[i].FindControl("chkSelectChronic");
                    if (chkSelectChronic.Checked == true)
                    {
                        DataRow[] drcheck = dtp.Select("ICDID=" + lblIcdId.Text);
                        if (drcheck.Length == 0)
                        {
                            dr = dtch.Select("ICDID=" + lblIcdId.Text);
                            if (dr.Length != 0)
                            {
                                DataRow drw = dtp.NewRow();

                                drw[0] = dr[0]["Id"];
                                if (dr[0]["Id"].ToString() == "0")
                                {
                                    drw[1] = common.myInt(Session["RegistrationID"]);
                                    drw[2] = common.myInt(Session["encounterid"]);
                                }
                                else
                                {
                                    drw[1] = dr[0][1];
                                    drw[2] = dr[0][2];
                                }
                                drw[3] = dr[0]["ICDId"];
                                drw[4] = dr[0]["ICDCode"];
                                drw[5] = dr[0]["ICDDescription"];
                                drw[6] = dr[0]["PrimaryDiagnosis"];

                                //dr[0]["IsChronic"] = false;

                                drw[7] = false; //dr[0]["IsChronic"];
                                drw[8] = dr[0]["OnsetDate"];
                                drw[9] = dr[0]["DoctorId"];
                                //drw[10] = dr[0]["DoctorName"];
                                drw[11] = dr[0]["FacilityId"];
                                //drw[12] = dr[0]["FacilityName"];
                                drw[13] = dr[0]["LocationId"];
                                drw[14] = dr[0]["Remarks"];
                                drw[15] = dr[0]["ConditionIds"];
                                drw[16] = dr[0]["TypeId"];
                                //drw[17] = dr[0]["DiagnosisStatus"];
                                //drw[18] = dr[0]["DiagnosisType"];
                                drw[19] = dr[0]["IsResolved"];
                                //drw[20] = dr[0]["EncounterDate"];

                                dtp.Rows.Add(drw);
                            }
                        }
                        else
                        {
                            //Alert.ShowAjaxMsg("Duplicate data add...",Page);
                        }
                        chkSelectChronic.Checked = false;
                    }
                }
            }
            gvDiagnosisDetails.DataSource = dtp;
            gvDiagnosisDetails.DataBind();
            Cache.Insert("DignosisDetails", dtp, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            //updDiagnosisDetails.Update();           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dtch.Dispose();
            dtp.Dispose();
        }
    }

    #endregion



    protected void btnBackToSuperBill_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("PatientSuperbill.aspx", false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            BindFavouriteDiagnosis(txtSearch.Text);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnGetCondition_Click(object sender, EventArgs e)
    {
        try
        {
            BindDiagnosisStatus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnReferredBy_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Status.aspx";
            RadWindowForNew.Height = 450;
            RadWindowForNew.Width = 550;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;

            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void ddlDiagnosiss_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = CreateTable();
            data = PopulateAllDiagnosis(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.ddlDiagnosiss.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset.Equals(data.Rows.Count);//endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ICDDescription"];
                item.Value = data.Rows[i]["ICDID"].ToString();
                item.Attributes["ICDID"] = data.Rows[i]["ICDID"].ToString();
                item.Attributes["ICDCode"] = data.Rows[i]["ICDCode"].ToString();
                item.Attributes["ICDDescription"] = data.Rows[i]["ICDDescription"].ToString();

                this.ddlDiagnosiss.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }


    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected DataTable BindSearchDiagnosis(String etext)
    {
        DataSet ds = new DataSet();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);
            //ds = new DataSet();
            //ds = objDiag.BindDiagnosis(common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), etext);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindDiagnosis";
            APIRootClass.BindDiagnosis objRoot = new global::APIRootClass.BindDiagnosis();
            objRoot.DiagnosisGroupId = common.myInt(ddlCategory.SelectedValue);
            objRoot.DiagnosisSubGroupId = common.myInt(ddlSubCategory.SelectedValue);
            objRoot.DiagnosisCode = etext;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds.Tables[0];

    }

    protected void btnCommonOrder_OnClick(object sender, EventArgs e)
    {
        DataSet objDs = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            ViewState["hdnDiagnosisId"] = null;
            //objDiag = new BaseC.DiagnosisDA(sConString);

            //DataSet objDs = (DataSet)objDiag.GetICD9SubDisease(common.myStr(txtIcdCodes.Text));

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDiscriptionandICDID";
            APIRootClass.GetDiscriptionandICDID objRoot = new global::APIRootClass.GetDiscriptionandICDID();
            objRoot.IcdCode = txtIcdCodes.Text.Trim();


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (objDs.Tables[0].Rows.Count > 0)
            {
                dt = objDs.Tables[0];
                ViewState["hdnDiagnosisId"] = dt.Rows[0][0].ToString();
                ddlDiagnosiss.Text = dt.Rows[0][1].ToString();
                hdnIsUnSavedData.Value = "1";
                //rdpOnsetDate.SelectedDate = DateTime.Now;
            }
            else
            {
                ddlDiagnosiss.Text = "";
                lblMessage.Text = "No Data Found...";
                lblMessage.Visible = true;
                hdnIsUnSavedData.Value = "0";
            }
            ddlSides.SelectedValue = "0";
            chkPrimarys.Checked = false;
            chkChronics.Checked = false;
            chkIsFinalDiagnosis.Checked = false;

            for (int i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlDiagnosisStatus.SelectedValue = "0";

            ddlDiagnosisType.SelectedValue = "0";
            chkResolve.Checked = false;

            // ddlProviders.SelectedValue = "0";
            // ddlFacility.SelectedValue = "0";
            txtcomments.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDs.Dispose(); dt.Dispose();
        }
    }

    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindDiagnosisDetails();
        BindChronicDiagnosisDetails();
    }

    protected void btnHistory_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Assessment/DiagnosisHistory.aspx?From=" + common.myStr(Request.QueryString["From"]), false);
    }

    protected void btnCaseSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Editor/WordProcessor.aspx", false);
    }

    void ClearDiagnosisDetailsControls()
    {
        try
        {
            hdnDiagnosisId.Value = "";

            this.ddlDiagnosiss.Enabled = true;
            ddlDiagnosiss.Text = "";
            ddlDiagnosiss.Text = string.Empty;

            ddlCategory.Enabled = true;
            ddlSubCategory.Enabled = true;

            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.Items.Clear();

            ddlSides.SelectedValue = "0";
            chkPrimarys.Checked = false;
            chkChronics.Checked = false;
            chkQuery.Checked = false;
            chkIsFinalDiagnosis.Checked = false;
            rdpOnsetDate.Clear();
            for (int i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlDiagnosisStatus.SelectedValue = "0";
            ddlDiagnosisType.SelectedValue = "0";
            chkResolve.Checked = false;

            txtcomments.Text = "";
            btnAddtogrid.Text = "Add To List";

            txtstatusIds.Text = "";
            for (int i = 0; i < ddlDiagnosisStatus.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlDiagnosisStatus.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlDiagnosisStatus.SelectedItem.Text = "";
            RetrievePatientDiagnosis();


            txtIcdCodes.Text = "";
            ddlDiagnosiss.Text = "";
            statusDis = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void imgUTD_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (ddlDiagnosiss.Text.Trim().Length < 3)
            {
                Alert.ShowAjaxMsg("Please Type Diagnosis then Continue..", this);
                ddlDiagnosiss.Focus();
                return;
            }
            //RadWindowForNew.NavigateUrl = UtdLink + "/search?sp=0&source=USER_PREF&search=" + ddlDiagnosiss.Text + "&searchType=PLAIN_TEXT"; //"http://www.uptodate.com/contents/search?sp=0&source=USER_PREF&search=" + cmbProblemName.Text + "&searchType=PLAIN_TEXT";
            RadWindowForNew.NavigateUrl = UtdLink + "/contents/search?srcsys=HMGR374606&id=" + common.myStr(Session["EmployeeId"]) + "&search=" + ddlDiagnosiss.Text;

            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 600;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myStr(Session["RegistrationId"]);
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 600;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }


    protected void lbtnName_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["hdnDiagnosisId"] = null;
            string strvalue;
            LinkButton lnkPatient1 = sender as LinkButton;
            HiddenField hdnICDID = (HiddenField)lnkPatient1.FindControl("hdnICDID");

            strvalue = lnkPatient1.CommandArgument;
            txtIcdCodes.Text = hdnICDID.Value.ToString();

            ViewState["hdnDiagnosisId"] = strvalue;
            hdnDiagnosisId.Value = strvalue;
            //ddlDiagnosiss.Text = strvalue;
            ddlDiagnosiss.SelectedValue = strvalue;
            ddlDiagnosiss.Text = lnkPatient1.Text;
            hdnIsUnSavedData.Value = "1";
            rdpOnsetDate.SelectedDate = DateTime.Now;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvFavourite_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                //BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnICDID = (HiddenField)row.FindControl("hdnICDID");
                Label lblDescription = (Label)row.FindControl("lblDescription");
                if (common.myLen(hdnICDID.Value) > 0)
                {
                    //ObjDiagnosis.DeleteFavouriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(hdnICDID.Value), common.myInt(Session["UserID"]));

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteFavouriteDiagnosis";
                    APIRootClass.DeleteFavouriteDiagnosis objRoot = new global::APIRootClass.DeleteFavouriteDiagnosis();
                    objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                    objRoot.DiagnosisId = common.myInt(hdnICDID.Value);
                    objRoot.UserId = common.myInt(Session["UserID"]);

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);

                    lblMessage.Text = "Diagnosis deleted from your favorite list";
                }
                else
                {
                    Alert.ShowAjaxMsg("Select Diagnosis to delete from favorite list", Page);
                }
                BindFavouriteDiagnosis("");

                ddlDiagnosiss.Text = string.Empty;
                this.ddlDiagnosiss.Text = "";
                this.ddlDiagnosiss.SelectedValue = "";
                this.ddlDiagnosiss.Items.Clear();
                this.ddlDiagnosiss.DataSource = null;
                this.ddlDiagnosiss.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnExcludedServiceCancel_OnClick(object sender, EventArgs e)
    {
        divExcludedService.Visible = false;

    }
    protected void btnExcludedService_OnClick(object sender, EventArgs e)
    {
        try
        {
            divExcludedService.Visible = false;
            saveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void gvFavourite_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        //HiddenField hdnCalculationBase = (HiddenField)e.Row.FindControl("hdnCalculationBase");
    //        if (e.Row.RowIndex.Equals(0))
    //        {
    //            LinkButton lnkDescription = e.Row.FindControl("lnkDescription") as LinkButton;
    //            lnkDescription.Focus();
    //        }
    //    }
    //}
}
