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



public partial class EMR_Diagnosis_Default : System.Web.UI.Page
{
    #region Page level variable declration section
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    //  wcf_Service_Common.CommonMasterClient objCommon = new wcf_Service_Common.CommonMasterClient();
    private Hashtable hshInput;
    BaseC.ParseData Parse = new BaseC.ParseData();
    BaseC.DiagnosisDA objDiag;
    BaseC.RestFulAPI objMRD;
    DataSet ds;
    Hashtable hsNewPage = new Hashtable();
    DL_Funs fun = new DL_Funs();
    static DataView DvPatientDiagnosis = new DataView();
    bool statusDis = false;
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
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    #endregion

    #region PageLoad section

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        //tbDiagnosis.Tabs[1].Visible = false; 
        if (Session["encounterid"] == null)
        {
            Response.Redirect("/Default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {

            //Added By Ashutosh Prashar :13/05/2013:: For Deletion Confirmation
            //BindFavouriteDiagnosis("");
            dvConfirmDeletion.Visible = false;
            //dvChronicConfirmDeletion.Visible = false;
            //End Here

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

            if (Request.QueryString["IsExpire"] != "0" && Request.QueryString["IsEncStatus"] != "8")
            {
                pnlDeatReg.Enabled = true;
                pnlBirthRegDetail.Enabled = false;
                btnSaveDetail.Enabled = true;
            }
            else if (Request.QueryString["IsNewBorn"] != "0" && Request.QueryString["IsEncStatus"] != "8")
            {
                pnlDeatReg.Enabled = false;
                pnlBirthRegDetail.Enabled = true;
                btnSaveDetail.Enabled = true;
            }


            else
            {
                pnlDeatReg.Enabled = false;
                pnlBirthRegDetail.Enabled = false;
                btnSaveDetail.Enabled = false;
            }

            //if (Request.QueryString["MLC"].ToUpper().Equals("Yes") && Request.QueryString["IsEncStatus"] != "8")
            //{

                pnlMLCDetails.Enabled = true;

            //}
            //else
            //{

            //    pnlMLCDetails.Enabled = false;
            //}

            drdponlineDeathRegDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            drdponlineDeathRegDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
            //drdponlineDeathRegDate.SelectedDate = common.myDate(DateTime.Now);
            drdponlineDeathRegDate.MaxDate = common.myDate(DateTime.Now);
            dtonlinebirthRegDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtonlinebirthRegDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
            //dtonlinebirthRegDate.SelectedDate = common.myDate(DateTime.Now);
            dtonlinebirthRegDate.MaxDate = common.myDate(DateTime.Now);
            //getDoctorID();
            dtpDeliveryDateTime.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpDeliveryDateTime.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtpDeliveryDateTime.SelectedDate = common.myDate(DateTime.Now);
            dtpDeliveryDateTime.MaxDate = common.myDate(DateTime.Now);
            ViewState["DoctorID"] = common.myStr(Session["EmployeeId"]);
            BindFacility();

            BindProvider();

            BindDiagnosisType();
            //BindPatientAlert();
            BindDrpCategory();//19th April10
            BindSubCategory("");

            RetrievePatientDiagnosis();

            if (Request.QueryString["DiagnosisId"] != null && Request.QueryString["DiagnosisId"] != "")
            {
                Int32 iDiagnosisId = Convert.ToInt32(Request.QueryString["DiagnosisId"].ToString());
                FillDiagnosisDetailControls(iDiagnosisId);
            }

            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                btnAddtogrid.Visible = false;
                ddlCategory.Enabled = false;
                ddlSubCategory.Enabled = false;
                ddlDiagnosiss.Enabled = false;

                gvDiagnosisDetails.Enabled = false;
            }

            txtIcdCodes.Attributes.Add("onblur", "nSat=1;");

            // bindDeficiency();

            RadioButtonList1_SelectedIndexChanged(null, null);

        }
        string controlName = Request.Params.Get("__EVENTTARGET");

        if (hdnRowIndex.Value != "" && gvDiagnosisDetails.Rows.Count >= Convert.ToInt32(hdnRowIndex.Value))
        {
            GridViewRow gvr = gvDiagnosisDetails.Rows[Convert.ToInt32(hdnRowIndex.Value)];
            Literal lbl = (Literal)gvr.FindControl("lblDiagnosisXML");
            lbl.Text = hdnProblems.Value;
            TextBox txtProblem = (TextBox)gvr.FindControl("txtDiagnosisDetails");
            Literal ltrl = new Literal();
            ltrl.Mode = LiteralMode.Transform;
            ltrl.Text = HttpUtility.HtmlDecode(formatString(hdnProblems.Value));
            txtProblem.Text = ltrl.Text;
        }

        string FiletransferWardtoMRDAfterEnterDiagnosticEntry = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "FiletransferWardtoMRDAfterEnterDiagnosticEntry", sConString);
        if (FiletransferWardtoMRDAfterEnterDiagnosticEntry.ToUpper().Equals("Y") && common.myInt(Request.QueryString["AcknowledmentStatus"])==0)
        {
            pnlRecordVisit.Enabled = false;
        }

    }


    protected void lnkDiagnosisDetails_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/Assessment/DiagnosisDetails.aspx";

        RadWindowForNew.Title = "ICD 10 Help";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected Boolean ISDiagnosesExits()
    {
        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            BaseC.Security AuditCA = new BaseC.Security(sConString);

            DataSet ds = objDiag.ISDiagnosesExits(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]));

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
    }

    #endregion

    private void FillDiagnosisDetailControls(Int32 iICDID)
    {
        try
        {
            DataTable dtDignosisDetails = (DataTable)Cache["DignosisDetails"];
            DataView dv = new DataView(dtDignosisDetails);
            dv.RowFilter = "Id=" + iICDID;
            DataTable dt = new DataTable();
            if (dv.Count > 0)
            {

                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    hdnDiagnosisId.Value = dt.Rows[0]["ICDID"].ToString();
                    txtIcdCodes.Text = dt.Rows[0]["ICDCode"].ToString();

                    ddlDiagnosiss.Text = dt.Rows[0]["ICDDescription"].ToString();
                    txtstatusIds.Text = dt.Rows[0]["Conditionids"].ToString();

                    if (dt.Rows[0]["DoctorId"] != DBNull.Value)
                        ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(dt.Rows[0]["DoctorId"].ToString()));



                    ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(dt.Rows[0]["FacilityId"].ToString()));
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
            BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
            SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserId"]));
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
        string EmpId = "";
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
                    EmpId = serviceIdXml[0].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return EmpId;
    }

    protected void RetrievePatientDiagnosis()
    {
        try
        {
            BindNotes bNotes = new BindNotes(sConString);
            objDiag = new BaseC.DiagnosisDA(sConString);


            if (Session["encounterid"] != null)
            {

                BaseC.Security AuditCA = new BaseC.Security(sConString);
                ds = new DataSet();
                ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["RegistrationId"]), common.myInt(Session["encounterid"]), 0, 0, 0, "", "", "", "%%", false, 0, "", false, 0);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataView dvStTemplate = new DataView();
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
                    DataView dvDiagnosisDetail = new DataView(dvStTemplate.ToTable());

                    dvDiagnosisDetail.RowFilter = "IsChronic=1 and EncounterID=" + Convert.ToString(Session["encounterid"]);
                    DataTable dtChronicDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    //if (dtChronicDiagnosisDetail.Rows.Count > 0)
                    //{
                    //    gvChronicDiagnosis.DataSource = dtChronicDiagnosisDetail;
                    //    gvChronicDiagnosis.DataBind();

                    //}
                    //else
                    //{
                    //    BindBlankChronicDiagnosisGrid();
                    //}
                    string MRDCode = common.myStr(Request.QueryString["CF"]) == "MRDM" ? "1" : "0";
                    dvDiagnosisDetail.RowFilter = "IsChronic=0 and EncounterID=" + Convert.ToString(Session["encounterid"]) + " and MRDCode=" + MRDCode;
                    DataTable dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        gvDiagnosisDetails.DataSource = dtDiagnosisDetail;
                        gvDiagnosisDetails.DataBind();
                        gvDiagnosisDetails.Columns[4].Visible = true;
                    }
                    else
                    {
                        BindBlankDiagnosisDetailGrid();

                    }

                }
                else
                {
                    ViewState["Record"] = 0;
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
        if (ViewState["Record"] != null)
        {
            if (Convert.ToInt32(ViewState["Record"]) == 1)
                Response.Redirect("DetailAssessment.aspx", false);
            else
                Alert.ShowAjaxMsg("Kindly save atleast one Diagnosis!", Page);
        }
    }

    #endregion

    #region Search section

    protected void btnSearchICDCode_Click(object sender, EventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //string strSQL = "SELECT ICDID, Description FROM ICD9SubDisease WHERE ICDCode='" + txtIcdCodes.Text.Trim() + "'"; //f270117 comment
        string strSQL = "SELECT ICDID, Description,ICDCode FROM ICD9SubDisease WHERE ICDCode='" + txtIcdCodes.Text.Trim() + "'"; //f270117

        DataSet objDs = objDl.FillDataSet(CommandType.Text, strSQL);

        if (objDs.Tables[0].Rows.Count > 0)
        {
            DataTable dt = objDs.Tables[0];
            ViewState["hdnDiagnosisId"] = dt.Rows[0][0].ToString();

            ddlDiagnosiss.Text = dt.Rows[0][1].ToString(); //f270117 comment

            //int length = dt.Rows[0][2].ToString().Length;
            //int intfinallength = 20 - length;
            //char strfinallength = Convert.ToChar(intfinallength);
            //ddlDiagnosiss.Text = dt.Rows[0][2].ToString() + strfinallength + "    " + dt.Rows[0][1].ToString(); //f270117

            hdnDiagnosisId.Value = dt.Rows[0][0].ToString();
            ddlDiagnosiss.SelectedValue = dt.Rows[0][0].ToString();
        }
        else
        {
            ddlDiagnosiss.Text = "";
            lblMessage.Text = "No Data Found...";
            lblMessage.Visible = true;
        }
    }

    // Populating Category Dropdown
    private void BindDrpCategory()
    {
        try
        {
            ddlCategory.Items.Clear();
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.BindCategory();

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
    }

    private void BindSubCategory(string GroupId)
    {
        try
        {
            ddlSubCategory.Items.Clear();
            if (GroupId != "")
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();
                ds = objDiag.BindSubCategory(common.myStr(GroupId));

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
    }

    protected void ddlSubCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            hstInput.Add("@intSubGroupId", ddlSubCategory.SelectedValue);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Add("Id");
                dt.Columns.Add("EncounterDate");

            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindSubCategory(ddlCategory.SelectedValue);
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intGroupId", ddlCategory.SelectedValue);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Add("Id");
                dt.Columns.Add("EncounterDate");

            }
            else
            {
                BindBlankGrid();
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

    #region Telerik RadGrid section

    private void create_window(string url)
    {
        // This window is created for each match of the selected ICD value for a patient to a rule .
        // If a patient matches any rule in the rule engine then this window should pop up at the selection of an ICD9 value.
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


    #endregion

    #region TopButtonsData section

    protected void btnGoDiagnosisHistory_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("DiagnosisHistory.aspx", false);
    }

    private DataTable PopulateAllDiagnosis(string txt)
    {
        DataTable DT = new DataTable();
        try
        {

            ViewState["BTN"] = "ALL";

            if (Session["encounterid"] != null)
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                ds = new DataSet();

                string strSearchCriteria = string.Empty;

                strSearchCriteria = "%" + txt + "%";
                ds = objDiag.BindDiagnosis(common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), strSearchCriteria);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
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
        return DT;
    }
    //protected void BindFavouriteDiagnosis(string text)
    //{
    //    try
    //    {
    //        objDiag = new BaseC.DiagnosisDA(sConString);
    //        ds = new DataSet();
    //        ds = objDiag.BindFavoriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), text);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            gvFavourite.DataSource = ds.Tables[0];
    //            gvFavourite.DataBind();
    //        }
    //        else
    //        {
    //            gvFavourite.DataSource = null;
    //            gvFavourite.DataBind();
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    //protected void btnAddToFavourite_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
    //        if (common.myInt(ddlDiagnosiss.SelectedValue) > 0)
    //        {
    //            string strsave = ObjDiagnosis.SaveFavouriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(ddlDiagnosiss.SelectedValue), common.myInt(Session["UserID"]));
    //            if (strsave.Contains("AlReady Exist in your favorite list"))
    //            {
    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //                lblMessage.Text = strsave;

    //            }
    //            else
    //            {
    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
    //                lblMessage.Text = strsave;

    //            }
    //        }
    //        else
    //        {
    //            Alert.ShowAjaxMsg("Select Diagnosis to add into favorite list", Page);
    //            return;
    //        }
    //        //BindFavouriteDiagnosis("");
    //        ddlDiagnosiss.SelectedValue = "0";
    //        ddlDiagnosiss.Text = "";
    //        ddlDiagnosiss.Items.Clear();
    //        txtIcdCodes.Text = "";
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    protected void gvFavourite_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #endregion

    #region BindDiagnosisGrid and Chronic Grid section

    protected void BindDiagnosisDetailsGrid(int iICDID, string sICDCode, string sDiagnosisName, string sDiagnosisDetails, string sDiagnosisXML)
    {
        try
        {
            DataRow dr;
            DataTable dT = CreateTable();

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

        return Dt;
    }

    protected void gvDiagnosisDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false; //IcdId
            // e.Row.Cells[1].Visible = false; // Icd Code
            // e.Row.Cells[2].Visible = false; //Description

            e.Row.Cells[3].Visible = false; //
            //e.Row.Cells[4].Visible = true;
            e.Row.Cells[4].Visible = false;//primary
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
                lblPrimary.Text = "Y";
            }
            else
            {
                lblPrimary.Text = "";
            }
        }
    }

    protected void gvChronicDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }

    protected void gvDiagnosisDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDiagnosisDetails.PageIndex = e.NewPageIndex;
        if (ddlDiagnosiss.SelectedIndex > 0)
        {

            string selectedDataKey = ddlDiagnosiss.SelectedValue;
            string selectedDataCode = ddlDiagnosiss.Attributes["ICDCode"].ToString();

            BindDiagnosisDetailsGrid(Convert.ToInt32(selectedDataKey), selectedDataCode, "", "", "");
        }
    }


    protected void gvDiagnosisDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ddlDiagnosiss.Text = "";
            this.ddlDiagnosiss.SelectedValue = "";
            this.ddlDiagnosiss.Enabled = false;
            ddlCategory.Enabled = false;
            ddlSubCategory.Enabled = false;

            ViewState["chkDiagnosisTF"] = "";
            ViewState["chkChronicTF"] = "";

            int RowIndex = 0;
            RowIndex = Convert.ToInt32(gvDiagnosisDetails.SelectedRow.RowIndex);
            ViewState["RowIndex"] = RowIndex;



            Label lblIcdId = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblIcdId");

            txtIcdId.Text = lblIcdId.Text;

            Label lblId = (Label)gvDiagnosisDetails.SelectedRow.FindControl("lblId");

            txtid.Text = lblId.Text;


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
            hdnDiagnosisId.Value = lblIcdId.Text;
            txtIcdCodes.Text = lblICDCode.Text.Trim();
            txtIcdCodes.Enabled = false;
            hdnIsUnSavedData.Value = "1";
            ddlDiagnosiss.Text = lblDescription.Text.Trim();
            //ddlDiagnosisStatus.SelectedValue = lblddlStatus.Text;


            string strQualityIds = "";
            string conditionText = "";
            if (lblddlStatus.Text != "0" || lblddlStatus.Text != "")
            {
                txtstatusIds.Text = lblddlStatus.Text;
                strQualityIds = lblddlStatus.Text;
                string strQuantityID = "";

                int i, j;
                string[] arInfo = new string[4];
                char[] splitter = { ',' };
                arInfo = strQualityIds.Split(splitter);

            }

            if (conditionText.Length > 0)
            {
                conditionText = conditionText.Substring(0, conditionText.Length - 1);
            }

            ddlProviders.SelectedValue = lblddlProvider.Text;

            ddlFacility.SelectedValue = lblddlLocation.Text;
            if (ddlDiagnosiss.Text.Trim() != "")
            {
                btnAddtogrid.Text = "Update List";
            }

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


    //Diagnosis "Cancel"
    protected void btnCancelDeletion_OnClick(object sender, EventArgs e)
    {
        dvConfirmDeletion.Visible = false;

    }
    //Chronic "YES"
    //Diagnosis "Yes"
    protected void btnCancelDeletioncpt_OnClick(object sender, EventArgs e)
    {
        Div1.Visible = false;

    }

    protected void btnDeletioncpt_OnClick(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        Div1.Visible = false;

        //After Yes Button Press ::selected row is deleting

        if (hdnCPTid.Value != "0")
        {

            string strsql = "";
            hshInput.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            hshInput.Add("@intId", common.myInt(hdnCPTid.Value));
            strsql = "UPDATE MRDSurgeryCPTCodeDetails SET Active=0, LastChangedBy=@intEncodedBy,LastChangedDate=GETUTCDATE() " +
                    " WHERE Id=@intId ";
            dl.ExecuteNonQuery(CommandType.Text, strsql, hshInput);


        }

        BindMultipleCPTDetails();
    }

    protected void btnDeletion_OnClick(object sender, EventArgs e)
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        dvConfirmDeletion.Visible = false;
        if (hdnlblId.Value != "0")
        {
            hshInput.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            hshInput.Add("@intDiagnosisId", Convert.ToInt32(hdnlblId.Value.Trim()));
            hshInput.Add("@intLoginFacilityId", Session["FacilityID"]);
            hshInput.Add("@intPageId", Convert.ToInt32(ViewState["PageId"]));
            hshInput.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            hshInput.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
            dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientDiagnosis", hshInput);

            //string sQuery = "";
            //sQuery += " UPDATE EMRPatientFormDetails SET ShowNote = " + rblShowNote.SelectedItem.Value + "  ";
            //sQuery += " FROM EMRPatientForms epf    ";
            //sQuery += " INNER JOIN EMRPatientFormDetails epfd ON epf.PatientFormId = epfd.PatientFormId ";
            //sQuery += " AND epfd.PageId = " + ViewState["PageId"] + "  WHERE epf.EncounterId = " + Convert.ToInt32(Session["encounterid"]) + " ";
            //sQuery += " AND epf.RegistrationId = " + Convert.ToInt32(Session["RegistrationID"]) + "    ";
            //sQuery += " AND epf.Active = 1 ";

            //dl.ExecuteNonQuery(CommandType.Text, sQuery);

            RetrievePatientDiagnosis();
            statusDis = true;

        }


        //End Here
    }

    protected void gvDiagnosisDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DiagnosisCookie")
            {
                GridViewRow gvr = gvDiagnosisDetails.Rows[Convert.ToInt32(e.CommandArgument) - 1];
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
                DataTable dT = CreateTable();
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
                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        hshInput = new Hashtable();
                        hshInput.Add("intEncounterId", Convert.ToInt32(Session["encounterid"]));
                        string cmdstr = "SELECT [Description] FROM ICD9SubDisease WHERE [ICDCode]='" + txtIcdcode + "'";
                        DataSet objDs = objDl.FillDataSet(CommandType.Text, cmdstr);
                        //TextBox txtDesc = (TextBox)(gvDiagnosisDetails.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].FindControl("txtDiagnosisName"));
                        TextBox txtDesc = (TextBox)(gvr.Cells[2].FindControl("txtDiagnosisName"));
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            DataTable tbl = objDs.Tables[0];
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
    }

    #endregion

    //protected void btnExtenalEdu_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txtIcdCodes.Text.Trim() != "" && txtIcdCodes.Text.Trim() != "0")
    //        {
    //            if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
    //            {
    //                #region Log for each encounter if the diagnosis(Education Meterial), order(Education Meterial), lab result or medication(MonographButton) print button is pressed
    //                Hashtable logHash = new Hashtable();
    //                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //                logHash.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
    //                logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
    //                logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
    //                logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
    //                logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
    //                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogEducationAndMonograph", logHash);
    //                #endregion
    //            }

    //            string url = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.103&mainSearchCriteria.v.c=" + txtIcdCodes.Text.Trim() + "";
    //            string fullURL = "window.open('" + url + "', '_blank', 'status=yes,toolbar=yes,menubar=yes,location=no,scrollbars=yes,resizable=yes,titlebar=yes' );";
    //            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


    //        }
    //        else
    //        {
    //            Alert.ShowAjaxMsg("Please select Diagnosis", Page);
    //            return;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}



    #region Bind Blank Grid section

    private void BindBlankDiagnosisDetailGrid()
    {
        try
        {
            DataTable dT = CreateTable();
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
        try
        {

            //objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            //  ds = objCommon.GetDoctors(Convert.ToInt16(Session["HospitalLocationId"]));
            //BaseC.Hospital oHs = new BaseC.Hospital(sConString);
            //ds = oHs.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationId"]), 0, 0);
            BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
            ds = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, 0, 0);
            ddlProviders.Items.Clear();
            ddlProviders.DataSource = ds;
            ddlProviders.DataValueField = "DoctorId";
            ddlProviders.DataTextField = "DoctorName";
            ddlProviders.DataBind();
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
    }

    protected void BindFacility()
    {
        try
        {
            BaseC.EMRProblems objProb = new BaseC.EMRProblems(sConString);
            ds = new DataSet();
            ds = objProb.GetFacility(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), common.myInt(Session["GroupID"]));

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = ds;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
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
    }

    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();
    }

    protected void BindDiagnosisType()
    {
        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.BindDiagnosistype();

            //ddlProviders.Items.Insert(0, new ListItem("Select", "0"));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    #endregion

    #region Bind Gridview section both Today's Diagnosis and Chronic

    void BindDiagnosisDetails()
    {
        try
        {
            lblMessage.Text = "";
            DataTable DT = (DataTable)Cache["DignosisDetails"];
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

                datarow[0]["ConditionIds"] = txtstatusIds.Text.Trim(); //ddlDiagnosisStatus.SelectedValue;

                datarow[0]["DoctorId"] = ddlProviders.SelectedValue;
                datarow[0]["FacilityId"] = ddlFacility.SelectedValue;
                datarow[0].EndEdit();
            }
            else
            {
                DataRow dr;
                dr = DT.NewRow();
                dr["ICDID"] = hdnDiagnosisId.Value;
                dr["IcdCode"] = txtIcdCodes.Text.Trim();
                dr["ICDDescription"] = ddlDiagnosiss.Text.Trim();

                dr["ConditionIds"] = txtstatusIds.Text.Trim(); // ddlDiagnosisStatus.SelectedValue;

                dr["DoctorId"] = ddlProviders.SelectedValue;
                dr["FacilityId"] = ddlFacility.SelectedValue;
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
    }


    #endregion

    #region AddToList AddToToday Section

    protected void btnAddtogrid_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bc = new BaseC.Patient(sConString);

            objDiag = new BaseC.DiagnosisDA(sConString);
            StringBuilder strXMLAleart = new StringBuilder();
            ArrayList coll = new ArrayList();

            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);

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


            string strSQL = "";
            Hashtable hash2 = new Hashtable();
            hash2.Add("@inticdid", hdnDiagnosisId.Value);
            hash2.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            hash2.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"]));

            //if (chkPrimarys.Checked)
            //{
            //    strSQL = "select icdid from EMRPatientDiagnosisDetails where RegistrationId=@intRegistrationId  and PrimaryDiagnosis=1 and Active=1 and EncounterId=@intEncounterId ";
            //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //    DataSet dsprimary = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hash2);
            //    if (dsprimary.Tables[0].Rows.Count > 0)
            //    {
            //        //Alert.ShowAjaxMsg("Duplicate data add...", Page);
            //        Alert.ShowAjaxMsg("Only One ICD can be Primary for Patient.", Page);
            //        lblMessage.Text = "Only One ICD can be Primary for Patient.";
            //        Alert.ShowAjaxMsg("Only One ICD can be Primary for Patient.", Page);

            //        return;
            //    }
            //}

            if (ddlDiagnosiss.SelectedValue != "" || hdnDiagnosisId.Value != "0" || hdnDiagnosisId.Value != "")
            {
                bool bResult = objDiag.CheckValidForPrimaryDiagnosis(Convert.ToInt32(ddlDiagnosiss.SelectedValue == "" ? hdnDiagnosisId.Value : ddlDiagnosiss.SelectedValue));
                if (bResult == false)
                {
                    hdnIsUnSavedData.Value = "0";
                    Alert.ShowAjaxMsg("This ICD is not valid for Primary Diagnosis", Page);
                    return;
                }
            }

            if (btnAddtogrid.Text == "Update List")
            {

            }
            StringBuilder objXML = new StringBuilder();

            objXML.Append("<Table1><c1>");
            objXML.Append(Parse.ParseQ(txtid.Text.Trim()));
            objXML.Append("</c1><c2>");
            // objXML.Append(Parse.ParseQ(txtIcdId.Text.Trim()));hdnDiagnosisId.Value
            objXML.Append(Convert.ToInt32(hdnDiagnosisId.Value));
            objXML.Append("</c2><c3>");
            if (common.myStr(Request.QueryString["CF"]).ToUpper() == "MRDM")
            {
                objXML.Append(common.myBool(0));//PrimaryDiagnosis
            }
            else
            {
                objXML.Append(chkPrimarys.Checked);//PrimaryDiagnosis
            }
            objXML.Append("</c3><c4>");
            objXML.Append("</c4><c5>");
            objXML.Append("</c5><c6>");
            objXML.Append("</c6><c7>");
            objXML.Append("</c7><c8>");
            if (common.myInt(ddlProviders.SelectedValue) != 0)
            {
                objXML.Append(ddlProviders.SelectedValue);
            }
            else
            {
                objXML.Append(DBNull.Value);
            }
            objXML.Append("</c8><c9>");
            objXML.Append(ddlFacility.SelectedValue);
            objXML.Append("</c9><c10>");
            objXML.Append("</c10><c11>");
            objXML.Append(txtstatusIds.Text);
            objXML.Append("</c11><c12>");
            objXML.Append("</c12><c13>");
            objXML.Append("</c13><c14>");
            objXML.Append(Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate"
                ? Request.QueryString["TemplateFieldId"].ToString() : "0");

            objXML.Append("</c14></Table1>");



            ds = objDiag.CheckDuplicateProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(hdnDiagnosisId.Value), true);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (btnAddtogrid.Text == "Add To List")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (hdnDiagnosisId.Value.ToString().Trim() == ds.Tables[0].Rows[i]["icdid"].ToString().Trim())
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                            hdnIsUnSavedData.Value = "0";
                            return;
                        }
                    }
                }
            }

            string doctorid = null;
            if (common.myStr(Session["DoctorID"]) != "")
            {
                doctorid = common.myStr(Session["DoctorID"]);
            }

            string strsave = "";
            int MRDCode = common.myStr(Request.QueryString["CF"]).ToUpper() == "MRDM" ? 1 : 0;
            strsave = objDiag.EMRSavePatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), doctorid, common.myInt(ViewState["PageId"]), objXML.ToString(), strXMLAleart.ToString(), common.myInt(Session["UserId"]), chkPullDiagnosis.Checked, false, MRDCode);

            ///Tagging Static Template with Template Field
            if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "StaticTemplate")
            {
                BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                Hashtable hshOut = emr.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                    Convert.ToInt32(Request.QueryString["SectionId"].ToString()), Convert.ToInt32(Request.QueryString["TemplateFieldId"].ToString()),
                    Request.QueryString["StaticTemplateId"] != null ? common.myInt(Request.QueryString["StaticTemplateId"]) : 133, common.myInt(Session["UserId"]));
            }
            ///end

            if (strsave.Contains("Data Saved!"))
            {
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
            //BindPatientAlert();
            RetrievePatientDiagnosis();

            txtIcdCodes.Text = "";
            ddlDiagnosiss.Text = "";
            this.ddlDiagnosiss.Enabled = true;
            ddlDiagnosiss.Text = string.Empty;
            statusDis = true;

            #region In each encounter, log once when a diagnosis is saved

                if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
            {
                //objDiag.EMRMUDLogSaveDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["DoctorID"]), common.myInt(Session["UserID"]));
            }
            #endregion
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    
    #endregion

    protected void btnBackToSuperBill_Click(object sender, EventArgs e)
    {
        Response.Redirect("PatientSuperbill.aspx", false);
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        //BindFavouriteDiagnosis(txtSearch.Text);
    }
    protected void btnGetCondition_Click(object sender, EventArgs e)
    {

    }

    protected void ibtnReferredBy_Click(object sender, ImageClickEventArgs e)
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

    //Added on  25-08-2014 Start Naushad Ali

    public void ddlCPTCode_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        BinddCPT(common.myInt(Session["encounterid"]), e.Text);
    }
    //Added on 25-08-2014 End Naushad ali

    public void ddlDiagnosiss_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            DataTable data = new DataTable();
            data = CreateTable();
            data = PopulateAllDiagnosis(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.ddlDiagnosiss.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = (string)data.Rows[i]["ICDDescription"]; //f270117 comment

                //int length = data.Rows[i]["ICDCode"].ToString().Length;
                //int intfinallength = 20 - length;
                //char strfinallength = Convert.ToChar(intfinallength);
                //item.Text = (string)data.Rows[i]["ICDCode"]+strfinallength+"    "+(string)data.Rows[i]["ICDDescription"]; //f270117

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

    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected DataTable BindSearchDiagnosis(String etext)
    {

        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.BindDiagnosis(common.myInt(ddlCategory.SelectedValue), common.myInt(ddlSubCategory.SelectedValue), etext);

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
        try
        {
            ViewState["hdnDiagnosisId"] = null;
            objDiag = new BaseC.DiagnosisDA(sConString);

            DataSet objDs = objDiag.GetICD9SubDisease(common.myStr(txtIcdCodes.Text));
            if (objDs.Tables[0].Rows.Count > 0)
            {
                DataTable dt = objDs.Tables[0];
                ViewState["hdnDiagnosisId"] = dt.Rows[0][0].ToString();

                ddlDiagnosiss.Text = dt.Rows[0][1].ToString(); //f27012017 comment


                //int length = txtIcdCodes.Text.Length;
                //int intfinallength = 20 - length;
                //char strfinallength = Convert.ToChar(intfinallength);
                //ddlDiagnosiss.Text = common.myStr(txtIcdCodes.Text) + strfinallength + "    " + dt.Rows[0][1].ToString();//f27012017

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
            // ddlProviders.SelectedValue = "0";
            // ddlFacility.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindDiagnosisDetails();
    }

    void ClearDiagnosisDetailsControls()
    {
        hdnDiagnosisId.Value = "";

        this.ddlDiagnosiss.Enabled = true;
        ddlDiagnosiss.Text = "";
        ddlDiagnosiss.Text = string.Empty;

        ddlCategory.Enabled = true;
        ddlSubCategory.Enabled = true;

        ddlCategory.SelectedIndex = 0;
        ddlSubCategory.Items.Clear();

        btnAddtogrid.Text = "Add To List";

        txtstatusIds.Text = "";
        RetrievePatientDiagnosis();


        txtIcdCodes.Text = "";
        ddlDiagnosiss.Text = "";
        statusDis = true;
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
        ViewState["hdnDiagnosisId"] = null;
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
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
    }
    protected void gvFavourite_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnICDID = (HiddenField)row.FindControl("hdnICDID");
                Label lblDescription = (Label)row.FindControl("lblDescription");
                if (common.myInt(hdnICDID.Value) > 0)
                {
                    ObjDiagnosis.DeleteFavouriteDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(hdnICDID.Value), common.myInt(Session["UserID"]));
                    lblMessage.Text = "Diagnosis deleted from your favorite list";
                }
                else
                {
                    Alert.ShowAjaxMsg("Select Diagnosis to delete from favorite list", Page);
                }

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
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl1.Enabled = true;
        pnl2.Enabled = true;
        pnl3.Visible = false;
        btnTag.Visible = false;
        ddlCPTCode.Visible = false;
        Label3.Visible = false;
        trMLCDetails.Visible = false;

        if (common.myInt(RadioButtonList1.SelectedValue) == 0)
        {

            tbl1.Visible = true;
            pnl1.Visible = true;
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            pnl2.Visible = true;
            pnl3.Visible = false;
            dvDiagnosis.Visible = true;
            pnlgrid.Visible = true;
            gvDiagnosisDetails.Visible = true;
            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            Label1.Visible = true;
            lblMrdLabel.Visible = true;

            TrgvDiagnosis.Visible = true;
            TrgvSurgeryProcedure.Visible = false;
            trMLCDetails.Visible = false;
            BindgvDiagnosis();
        }
        else if (common.myInt(RadioButtonList1.SelectedValue) == 2)
        {
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            TrgvDiagnosis.Visible = true;
            TrgvSurgeryProcedure.Visible = false;
            BindgvDiagnosis();
            bindDeficiency();
            pnl1.Enabled = false;
            pnl2.Enabled = false;
            pnl3.Visible = true;
            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            Label1.Visible = true;
            lblMrdLabel.Visible = true;
            trMLCDetails.Visible = false;
        }
        else if (common.myInt(RadioButtonList1.SelectedValue) == 3)
        {
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            TrgvDiagnosis.Visible = true;
            TrgvSurgeryProcedure.Visible = false;
            BindgvDiagnosis();
            bindDeficiency();
            pnl1.Enabled = false;
            pnl2.Enabled = false;
            pnl3.Visible = true;
            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            Label1.Visible = true;
            lblMrdLabel.Visible = true;
            trMLCDetails.Visible = false;
        }

        else if (common.myInt(RadioButtonList1.SelectedValue) == 4)
        {
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            TrgvDiagnosis.Visible = true;
            TrgvSurgeryProcedure.Visible = false;
            BindgvDiagnosis();
            bindDeficiency();
            pnl1.Enabled = false;
            pnl2.Enabled = false;
            pnl3.Visible = true;

            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            Label1.Visible = true;
            lblMrdLabel.Visible = true;
            trMLCDetails.Visible = false;
        }
        else if (common.myInt(RadioButtonList1.SelectedValue) == 5)
        {
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            TrgvDiagnosis.Visible = false;
            TrgvSurgeryProcedure.Visible = false;
            pnl1.Visible = false;
            pnl2.Visible = false;
            pnl3.Visible = false;
            trDeathReg.Visible = true;
            Label1.Visible = false;
            lblMrdLabel.Visible = false;
            dvDiagnosis.Visible = false;
            dvSurgey.Visible = false;
            trBirthReg.Visible = true;
            trMLCDetails.Visible = false;
            BindOnlineDeathRegDetail();
            BindBornDetails();
            BindGestationalWeek();
        }
        else if (common.myInt(RadioButtonList1.SelectedValue) == 6)
        {
            dvSurgey.Visible = false;
            pnlSurgery.Visible = false;
            gvSurgery.Visible = false;
            TrgvDiagnosis.Visible = false;
            TrgvSurgeryProcedure.Visible = false;
            pnl1.Visible = false;
            pnl2.Visible = false;
            pnl3.Visible = false;
            trDeathReg.Visible = true;
            Label1.Visible = false;
            lblMrdLabel.Visible = false;
            dvDiagnosis.Visible = false;
            dvSurgey.Visible = false;
            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            trMLCDetails.Visible = true;
            BindMLCandLegalDetail();
            //BindOnlineDeathRegDetail();
            //BindBornDetails();
            //BindGestationalWeek();
        }
        else
        {

            dvSurgey.Visible = true;
            pnlSurgery.Visible = true;
            gvSurgery.Visible = true;

            trDeathReg.Visible = false;
            trBirthReg.Visible = false;
            dvDiagnosis.Visible = false;
            pnlgrid.Visible = false;
            gvDiagnosisDetails.Visible = false;
            pnl1.Visible = false;
            tbl1.Visible = false;
            Label1.Visible = true;
            lblMrdLabel.Visible = true;
            TrgvDiagnosis.Visible = false;
            TrgvSurgeryProcedure.Visible = true;
            btnTag.Visible = true;
            ddlCPTCode.Visible = true;
            Label3.Visible = true;


            BindgvSurgeryProcedure();
            BindMultipleCPTDetails();
        }

    }

    void BindBlankGridDiagnosis()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ICDCode");
            dt.Columns.Add("Description");

            DataRow dr = dt.NewRow();
            dr["ICDCode"] = "";
            dr["Description"] = "";
            dt.Rows.Add(dr);
            gvDiagnosis.DataSource = dt;
            gvDiagnosis.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void BindBlankSurgeryProcedure()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("ServiceType");
            dt.Columns.Add("PackageName");
            dt.Columns.Add("DoctorName");
            dt.Columns.Add("SurgeryDate");
            dt.Columns.Add("MrdCptId");
            dt.Columns.Add("detailid");
            dt.Columns.Add("description");

            DataRow dr = dt.NewRow();
            dr["ServiceName"] = "";
            dr["ServiceType"] = "";
            dr["PackageName"] = "";
            dr["DoctorName"] = "";
            dr["SurgeryDate"] = "";
            dr["MrdCptId"] = "";
            dr["detailid"] = "";
            dr["description"] = "";
            dt.Rows.Add(dr);
            gvSurgeryProcedure.DataSource = dt;
            gvSurgeryProcedure.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void BindgvDiagnosis()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);

        DataSet ds = objMaster.GetClinicalDetails("", common.myStr(Session["EncounterNo"]), 0, common.myInt(Session["encounterid"]), 0);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDiagnosis.DataSource = ds;
                gvDiagnosis.DataBind();

            }
            else
            {
                BindBlankGridDiagnosis();
            }
        }
        else
        {
            BindBlankGridDiagnosis();
        }
    }

    private void BindgvSurgeryProcedure()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        //DataSet ds;
        DataSet ds = objMaster.GetSurgeryandProcedure(common.myInt(Session["encounterid"]), ddlCPTCode.Text);



        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCPTCode.Items.Clear();
                ddlCPTCode.DataSource = ds.Tables[1];
                ddlCPTCode.DataTextField = "Description";
                ddlCPTCode.DataValueField = "id";
                ddlCPTCode.DataBind();

                ddlCPTCode.Items.Insert(0, new RadComboBoxItem("", "0"));


                gvSurgeryProcedure.DataSource = ds;
                gvSurgeryProcedure.DataBind();
            }
            else
            {
                BindBlankSurgeryProcedure();
            }
        }
        else
        {
            BindBlankSurgeryProcedure();
        }
    }


    protected void btnDeficiency_OnClick(object sender, EventArgs e)
    {
        objDiag = new BaseC.DiagnosisDA(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();

        try
        {
            foreach (GridViewRow item in gvDeficiency.Rows)
            {
                TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
                DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
                DropDownList ddlActive = (DropDownList)item.FindControl("ddlActive");

                if (common.myInt(ddlStatus.SelectedValue) == 2) //common.myLen(txtRemarks.Text) == 0 && 
                {
                    continue;
                }

                //if (common.myLen(txtRemarks.Text) == 0 && common.myInt(ddlStatus.SelectedValue) == 0)
                //{
                //    Alert.ShowAjaxMsg("Status close not allow with remarks !", this.Page);
                //    return;
                //}

                HiddenField hdnQualityFormId = (HiddenField)item.FindControl("hdnQualityFormId");

                coll.Add(hdnQualityFormId.Value);
                coll.Add(txtRemarks.Text);
                coll.Add(ddlStatus.SelectedValue);
                coll.Add(ddlActive.SelectedValue);

                strXML.Append(common.setXmlTable(ref coll));
            }

            if (strXML.ToString() == string.Empty)
            {
                lblMessage.Text = "Remarks can't be blank !";
                return;
            }

            string strMsg = objDiag.saveMRDEncounterDeficiency(common.myInt(Session["EncounterId"]), strXML.ToString(),
                                    common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                bindDeficiency();

                lblMessage.Text = strMsg;
                Alert.ShowAjaxMsg(strMsg, this.Page);
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
            objDiag = null;
        }
    }

    private void bindDeficiency()
    {
        objDiag = new BaseC.DiagnosisDA(sConString);
        DataSet ds = new DataSet();
        try
        {
            string OPIP = "B";
            ViewState["cmddeficiency"] = null;
            if (common.myInt(RadioButtonList1.SelectedValue) == 3)
                OPIP = "O";

            if (common.myInt(RadioButtonList1.SelectedValue) == 4)
                OPIP = "I";

            ds = objDiag.getMRDEncounterDeficiency(common.myInt(Session["EncounterId"]), OPIP);
            ViewState["deficiency"] = ds.Tables[0];
            gvDeficiency.DataSource = ds.Tables[0];
            gvDeficiency.DataBind();

            ddlAddDeficiency.DataSource = ds.Tables[1];
            ddlAddDeficiency.DataTextField = "FormName";
            ddlAddDeficiency.DataValueField = "QualityFormId";
            ddlAddDeficiency.AppendDataBoundItems = false;
            ddlAddDeficiency.DataBind();

            ddlAddDeficiency.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDiag = null;
            ds.Dispose();
        }
    }

    protected void gvDeficiency_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
            DropDownList ddlActive = (DropDownList)e.Row.FindControl("ddlActive");

            if (common.myBool(hdnActive.Value))
            {
                ddlActive.SelectedIndex = ddlActive.Items.IndexOf(ddlActive.Items.FindByValue("1"));
            }
            else
            {
                if (hdnActive.Value != "")
                {
                    ddlActive.SelectedIndex = ddlActive.Items.IndexOf(ddlActive.Items.FindByValue("0"));
                }
            }

            txtRemarks.Enabled = false;
            ddlStatus.Enabled = false;
            ddlActive.Enabled = false;

            if (!common.myBool(hdnActive.Value)) //common.myLen(txtRemarks.Text) == 0 && 
            {
                txtRemarks.Enabled = false;
                ddlStatus.Enabled = false;
                ddlActive.Enabled = false;
            }
            //else if (common.myLen(txtRemarks.Text) > 0 && !common.myBool(hdnActive.Value))
            //{
            //    txtRemarks.Enabled = false;
            //    ddlStatus.Enabled = false;
            //    ddlActive.Enabled = false;
            //}
            else
            {
                txtRemarks.Enabled = true;
                ddlStatus.Enabled = true;
                ddlActive.Enabled = true;
            }
        }
    }

    protected void btnAddDeficiency_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlAddDeficiency.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Kindly Select The Deficiency First", Page);
            return;
        }
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["deficiency"];
        DataRow[] drDuplicate = dt.Select("QualityFormId=" + common.myInt(ddlAddDeficiency.SelectedValue) + "");
        if (drDuplicate.Length > 0)
        {
            Alert.ShowAjaxMsg("Already Selected", Page);
            return;
        }
        DataRow dr = dt.NewRow();
        dr["QualityFormId"] = common.myInt(ddlAddDeficiency.SelectedValue);
        dr["FormName"] = common.myStr(ddlAddDeficiency.SelectedItem.Text);
        dr["Remarks"] = String.Empty;
        dr["Active"] = common.myBool("True");
        dr["OpenStatus"] = common.myBool("True");
        dt.Rows.Add(dr);

        gvDeficiency.DataSource = dt;
        gvDeficiency.DataBind();
    }
    protected void btnTag_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            if (common.myInt(ddlCPTCode.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please Select The Service Name From Grid First", Page);
                return;
            }
            objDiag = new BaseC.DiagnosisDA(sConString);
            //   string strMsg = objDiag.UpdateMRDEncounterCptCode(common.myInt(ViewState["hdnDetailid"]), common.myInt(ddlCPTCode.SelectedValue));

            hshInput.Add("@intOrederId", common.myInt(ViewState["hdnDetailid"]));
            hshInput.Add("@intCPTId", common.myInt(ddlCPTCode.SelectedValue));
            hshInput.Add("@btStatus", 1);
            hshInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveMRDSurgeryCPTCode", hshInput, hshOutput);

            if (common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("SAVE"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindgvSurgeryProcedure();
                BindMultipleCPTDetails();
                lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);
                //Alert.ShowAjaxMsg(strMsg, this.Page);

            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);

                Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
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
            objDiag = null;
        }
    }

    protected void gvSurgeryProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hdnDetailid = (HiddenField)gvSurgeryProcedure.SelectedRow.FindControl("hdnDetailid");
        HiddenField hdnMrdCptId = (HiddenField)gvSurgeryProcedure.SelectedRow.FindControl("hdnMrdCptId");
        //if (common.myInt(hdnMrdCptId.Value) > 0)
        //{
        //    ddlCPTCode.SelectedValue = common.myStr(hdnMrdCptId.Value);
        //}
        //else
        //{
        //    ddlCPTCode.SelectedValue = common.myStr(0);
        //}
        ViewState["hdnDetailid"] = common.myStr(hdnDetailid.Value);
        ViewState["hdnMrdCptId"] = common.myStr(hdnMrdCptId.Value);

        BindMultipleCPTDetails();
    }



    public void BinddCPT(int Encounter, string text)
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = objMaster.GetSurgeryandProcedure(common.myInt(Session["encounterid"]), text);
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlCPTCode.DataSource = ds.Tables[1];
            ddlCPTCode.DataTextField = "Description";
            ddlCPTCode.DataValueField = "id";
            ddlCPTCode.DataBind();
            ddlCPTCode.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
    }

    protected void BindMultipleCPTDetails()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        string strsql = "";


        strsql = "SELECT so.ID,so.OrderDetailId,so.CPTId,som.CPTCode,ios.ServiceName,som.Description  FROM MRDSurgeryCPTCodeDetails so " +
                "INNER JOIN MrdSurgeryCptCodeMaster som WITH(NOLOCK) ON so.CPTId=som.id " +
                "INNER JOIN ServiceOrderDetail sod with(nolock) on sod.Id=so.OrderDetailId  and sod.active = 1" +
                "INNER JOIN itemofservice ios WITH(NOLOCK) ON ios.serviceid=sod.ServiceId " +
                "INNER JOIN SurgeryOrder sro with(nolock) on sro.OrderID =sod.OrderId  " +
                " WHERE sro.EncounterId=" + common.myInt(Session["encounterid"]) + " and so.Active=1 ";


        ds = dl.FillDataSet(CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count == 0)
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.AcceptChanges();
            gvSurgery.DataSource = ds;
        }
        else
        {
            gvSurgery.DataSource = ds;
        }
        gvSurgery.DataBind();
    }



    protected void gvSurgery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblId = (Label)gvSurgery.Rows[row.RowIndex].FindControl("lblIcdId");
                hdnCPTid.Value = lblId.Text;
                if (!string.IsNullOrEmpty(lblId.Text))
                {
                    Div1.Visible = true;
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

    protected void gvSurgery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false; //IcdId

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void gvSurgeryProcedure_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnMrdCptId = (HiddenField)e.Row.FindControl("hdnMrdCptId");
                //  Label lblMrdDescription = (Label)e.Row.FindControl("lblMrdDescription");
                if (common.myStr(hdnMrdCptId.Value) != "")
                {
                    e.Row.BackColor = System.Drawing.Color.YellowGreen;
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

    protected void gvSurgery_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSaveDeathOnlineReg_Click(object sender, EventArgs e)
    {
        objDiag = new BaseC.DiagnosisDA(sConString);


        string strMsg = objDiag.InsertUpdateOnlineDeathReg(common.myInt(Session["EncounterId"]), common.myStr(txtonlineregNo.Text), common.myStr(common.myDate(drdponlineDeathRegDate.SelectedDate)), common.myBool(chkAnesthesiarelatedDeath.Checked),
                                common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]), common.myBool(Request.QueryString["IsExpire"])
                                , common.myBool(Request.QueryString["IsNewBorn"]), common.myStr(txtBirthRegno.Text), common.myStr(common.myDate(dtonlinebirthRegDate.SelectedDate)), common.myInt(Session["RegistrationID"]), common.myInt(ddlDeliveryType.SelectedValue), common.myStr(common.myDate(dtpDeliveryDateTime.SelectedDate)), common.myInt(txtWeight.Text.Trim()), common.myInt(txtHeight.Text.Trim()), common.myInt(rcbGender.SelectedValue), common.myInt(rcbGestationalWeek.SelectedValue), txtOtherSpecification.Text.Trim(), common.myBool(chkDeathRelated.Checked), common.myBool(chkInfantDeath.Checked));

        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = strMsg;
            Alert.ShowAjaxMsg(strMsg, this.Page);
        }
    }


    private void BindOnlineDeathRegDetail()
    {
        objDiag = new BaseC.DiagnosisDA(sConString);
        //DataSet ds;
        DataSet ds = objDiag.GetOnlineBirthDeathRegDetail(common.myInt(Session["encounterid"]), common.myInt(Session["RegistrationID"]));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                dr = ds.Tables[0].Rows[0];

                txtonlineregNo.Text = common.myStr(dr["OnlineDeathRegNo"]);
                drdponlineDeathRegDate.SelectedDate = common.myDate(dr["OnlineDeathRegDate"]);
                chkAnesthesiarelatedDeath.Checked = common.myBool(dr["ISAnesthesiarelatedDeath"]);
            }

        }
    }

    protected void RadComboBox1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append(common.myStr(dtpDeliveryDateTime.SelectedDate.Value));
            sb.Remove(common.myStr(dtpDeliveryDateTime.SelectedDate.Value).IndexOf(":") + 1, 2);
            sb.Insert(common.myStr(dtpDeliveryDateTime.SelectedDate.Value).IndexOf(":") + 1, RadComboBox1.Text);

            //DateTime cDate = DateTime.Now;
            //if (common.myDate(cDate) < common.myDate(sb))
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Please SelectedDate should be between 'Min Time' and 'Max Time' ! ";
            //    return;
            //}
            lblMessage.Text = "";
            dtpDeliveryDateTime.SelectedDate = Convert.ToDateTime(sb.ToString());
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

    private void BindBornDetails()
    {
        try
        {
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, "SELECT ID,DeliverytypeName from DeliveryType WHERE Active =1 ORDER BY DeliverytypeName");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDeliveryType.DataSource = ds.Tables[0];
                ddlDeliveryType.DataTextField = "DeliverytypeName";
                ddlDeliveryType.DataValueField = "id";
                ddlDeliveryType.DataBind();
                ddlDeliveryType.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlDeliveryType.Items[0].Value = "0";
            }
            //Bind New Born
            if (common.myStr(Session["RegistrationID"]) != "")
            {
                DataSet objDs = (DataSet)objPatient.GetPatientRecord(common.myInt(Session["RegistrationNo"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
                if (objDs.Tables[2].Rows.Count > 0)
                {
                    ddlDeliveryType.SelectedValue = common.myStr(objDs.Tables[2].Rows[0]["DeliveryTypeId"]);
                    //txtBirthSequence.Text = common.myStr(objDs.Tables[2].Rows[0]["BirthSequence"]);
                    try
                    {
                        if (common.myStr(objDs.Tables[2].Rows[0]["DeliveryDateTime"]) != "" && common.myStr(objDs.Tables[2].Rows[0]["DeliveryDateTime"]) != "1/1/1900 12:00:00 AM")
                        {
                            dtpDeliveryDateTime.SelectedDate = Convert.ToDateTime(objDs.Tables[2].Rows[0]["DeliveryDateTime"]);
                        }
                    }
                    catch
                    {
                    }

                    txtWeight.Text = common.myDbl(objDs.Tables[2].Rows[0]["Weight"]).ToString("F2");
                    txtHeight.Text = common.myDbl(objDs.Tables[2].Rows[0]["Height"]).ToString("F2");
                    ddlDeliveryType.SelectedValue = common.myStr(objDs.Tables[2].Rows[0]["DeliveryTypeId"]);
                    rcbGender.SelectedValue = common.myStr(objDs.Tables[0].Rows[0]["Gender"]);
                    txtBirthRegno.Text = common.myStr(objDs.Tables[2].Rows[0]["OnlineBirthRegNo"]);
                    if (common.myStr(objDs.Tables[2].Rows[0]["OnlineBirthRegDate"]) != "")
                    {
                        dtonlinebirthRegDate.SelectedDate = Convert.ToDateTime(objDs.Tables[2].Rows[0]["OnlineBirthRegDate"]);
                    }
                    txtOtherSpecification.Text = common.myStr(objDs.Tables[2].Rows[0]["OtherSpecification"]);
                    rcbGestationalWeek.SelectedValue = common.myStr(objDs.Tables[2].Rows[0]["GestationalWeek"]);
                }
            }
            //Finish
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
            if (rcbItem != null)
            {
                rcbItem.Selected = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindGestationalWeek()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetGestationalWeek");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rcbGestationalWeek.DataSource = ds.Tables[0];
                rcbGestationalWeek.DataTextField = "Weeks";
                rcbGestationalWeek.DataValueField = "Weeks";
                rcbGestationalWeek.DataBind();
                rcbGestationalWeek.Items.Insert(0, new RadComboBoxItem(" ", "0"));
                rcbGestationalWeek.Items[0].Value = "0";

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void rcbGestationalWeek_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void btnMLCDetails_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "";
        string strMsg = "";
        objMRD = new BaseC.RestFulAPI(sConString);
        strMsg = objMRD.MRDSaveMLCAndLegalDetails(common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]),
            common.myInt(Session["FacilityId"]), common.myInt(1), common.myInt(Session["UserId"]),
            common.myDate(rdtReceivedDate.SelectedDate), txtPIno.Text, txtmlrno.Text, txtremark.Text, "", "");

        if ((strMsg.ToUpper().Contains("successfully") || strMsg.ToUpper().Contains("SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

        }
        Alert.ShowAjaxMsg(strMsg, Page);
        lblMessage.Text = strMsg;

    }

    protected void btnSaveLegalDetail_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "";
        string strMsg = "";
        objMRD = new BaseC.RestFulAPI(sConString);
        strMsg = objMRD.MRDSaveMLCAndLegalDetails(common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]),
            common.myInt(Session["FacilityId"]), common.myInt(0), common.myInt(Session["UserId"]),
            common.myDate(rdtDateofEvidance.SelectedDate), "", "", "", txtCaseBetween.Text, txtNameofJudge.Text);

        if ((strMsg.ToUpper().Contains("successfully") || strMsg.ToUpper().Contains("SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

        }
        Alert.ShowAjaxMsg(strMsg, Page);
        lblMessage.Text = strMsg;
    }

    private void BindMLCandLegalDetail()
    {
        objDiag = new BaseC.DiagnosisDA(sConString);
        //DataSet ds;
        DataSet ds = objDiag.GetGetMLCandLegalDetail(common.myInt(Session["encounterid"]), common.myInt(Session["RegistrationID"]));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                txtPIno.Text = common.myStr(ds.Tables[0].Rows[0]["PINo"]);
                rdtReceivedDate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["ReceivedDate"]);
                txtmlrno.Text = common.myStr(ds.Tables[0].Rows[0]["MLRNo"]);
                txtremark.Text = common.myStr(ds.Tables[0].Rows[0]["Remark"]);
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                txtCaseBetween.Text = common.myStr(ds.Tables[1].Rows[0]["CaseBetween"]);
                rdtDateofEvidance.SelectedDate = common.myDate(ds.Tables[1].Rows[0]["DateofEvidance"]);
                txtNameofJudge.Text = common.myStr(ds.Tables[1].Rows[0]["NameofJudge"]);
            }
        }
    }
    
    protected void btnSaveMRDFile_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "";
        string strMsg = "";
        objMRD = new BaseC.RestFulAPI(sConString);
        strMsg = objMRD.MRDSaveMRDFileDetails(common.myStr(Request.QueryString["OPIP"]),common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]),
            common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]));

        if ((strMsg.ToUpper().Contains("successfully") || strMsg.ToUpper().Contains("SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

        }
        Alert.ShowAjaxMsg(strMsg, Page);
        lblMessage.Text = strMsg;
    }
}
