using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Assessment_DiagnosisHistory : System.Web.UI.Page
{
    #region Page level variable declration section
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //clsExceptionLog objException = new clsExceptionLog();
    //BaseC.ParseData Parse = new BaseC.ParseData();

    //BaseC.DiagnosisDA objdiag;
    //DataSet ds;

    #endregion
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
        if (Session["diagpId"] != null)
        {
            ViewState["PageId"] = Session["diagpId"].ToString();
        }
        else
        {
            ViewState["PageId"] = "0";
        }
        if (!IsPostBack)
        {

            if (Session["encounterid"] != null)
            {
                dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
                dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();

                BindDrpProvider();
                BindDrpFacility();
                BindDrpStatus();           
                BindgvDiagnosisHistory();

            }
            else
            {
                Response.Redirect("/Default.aspx?RegNo=0", false);
            }
        }
        //add by Balkishan start
        if (common.myStr(Request.QueryString["callby"]) == "mrd")
        {
            btnBack.Visible = false;
            btnSave.Visible = false;

        }
        else
        {
            btnBack.Visible = true;
            btnSave.Visible = true;
        }
        //add by Balkishan end

        ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myInt(Session["FacilityId"]).ToString()));
        ddlFacility.Enabled = false;


    }


   
    private void BindDrpProvider()
    {
        DataSet ds = new DataSet();
        try
        {
            //objdiag = new BaseC.DiagnosisDA(sConString);
            //ds = new DataSet();
            //ds = objdiag.GetDoctorName(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));
            //ds = new DataSet();
            //BaseC.EMR objEmr = new BaseC.EMR(sConString);
            //ds = objEmr.GetEMRDoctorPatientwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRDoctorPatientwise";

            APIRootClass.GetEMRDoctorPatientwise objRoot = new global::APIRootClass.GetEMRDoctorPatientwise();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlProvider.Items.Clear();
                ddlProvider.DataSource = ds;
                ddlProvider.DataValueField = "DoctorId";
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataBind();
                ddlProvider.Items.Insert(0, new RadComboBoxItem("Select All", "0")); //ListItem("","0")
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
        }
    }

    protected void BindDrpFacility()
    {
        DataSet ds = new DataSet();
        try
        {
            //Hashtable hshInputs = new Hashtable();
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //hshInputs.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            //hshInputs.Add("@intUserId", Session["UserID"]);
            //hshInputs.Add("@intGroupId", Session["GroupID"]);
            //hshInputs.Add("@chvFacilityType", "O");
            //DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", hshInputs);
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

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
            ddlFacility.DataSource = ds;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
            //RadComboBoxItem rcbFacilityId = (RadComboBoxItem)ddlFacility.Items.FindItemByText(Convert.ToString(Session["Facility"]));
            //if (rcbFacilityId != null)
            //    rcbFacilityId.Selected = true;
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

    protected void BindDrpStatus()
    {
        DataSet objDs = new DataSet();
        try
        {
            string strSql = "Select StatusId, Description From EMRDiagnosisStatusMaster Where Active=1";
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet objDs = dl.FillDataSet(CommandType.Text, strSql);
            
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            ddlStatus.Items.Clear();
            ddlStatus.DataSource = objDs;
            ddlStatus.DataValueField = "StatusId";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
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
            objDs.Dispose();
        }
    }

    protected void BindgvDiagnosisHistory()
    {
        DataSet ds = new DataSet();
        try
        {
            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            //ds=new DataSet();
            if (Session["encounterid"] != null)
            {
                //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //BaseC.Security AuditCA = new BaseC.Security(sConString);
                          
                System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
           
               
                  DateTime FrmDate = common.myDate(dtpfromDate.SelectedDate);
                  DateTime ToDate = common.myDate(dtpToDate.SelectedDate);
                if (common.myStr(dtpfromDate.SelectedDate) != "" && common.myStr(dtpToDate.SelectedDate) != "")
                {

                    //ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), 0, common.myInt(ddlProvider.SelectedValue), 0, 0, "", FrmDate.ToString("yyyy/MM/dd"), ToDate.ToString("yyyy/MM/dd"), "%%", false, common.myInt(ddlStatus.SelectedValue), common.myStr(ddlSource.SelectedValue), chkChronics.Checked, 0);

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
                    APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
                    objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(ddlFacility.SelectedValue);
                    objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                    objRoot.EncounterId = 0;
                    objRoot.DoctorId = common.myInt(ddlProvider.SelectedValue);
                    objRoot.DiagnosisGroupId = 0;
                    objRoot.DiagnosisSubGroupId = 0;
                    objRoot.DateRange = "";
                    objRoot.FromDate = FrmDate.ToString("yyyy/MM/dd");
                    objRoot.ToDate = ToDate.ToString("yyyy/MM/dd");
                    objRoot.SearchKeyword = "%%";
                    objRoot.IsDistinct = false;
                    objRoot.StatusId = common.myInt(ddlStatus.SelectedValue);
                    objRoot.VisitType = common.myStr(ddlSource.SelectedValue);
                    objRoot.IsChronic = chkChronics.Checked;
                    objRoot.DiagnosisId = 0;
                    
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                }
                else
                {
                    //ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), 0, 
                    //    common.myInt(ddlProvider.SelectedValue), 0, 0, common.myStr(ddldateRange.SelectedValue), "", "", "%%", false, common.myInt(ddlStatus.SelectedValue), 
                    //    common.myStr(ddlSource.SelectedValue), chkChronics.Checked, 0);

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
                    APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
                    objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(ddlFacility.SelectedValue);
                    objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                    objRoot.EncounterId = 0;
                    objRoot.DoctorId = common.myInt(ddlProvider.SelectedValue);
                    objRoot.DiagnosisGroupId = 0;
                    objRoot.DiagnosisSubGroupId = 0;
                    objRoot.DateRange = common.myStr(ddldateRange.SelectedValue);
                    objRoot.FromDate = "";
                    objRoot.ToDate = "";
                    objRoot.SearchKeyword = "%%";
                    objRoot.IsDistinct = false;
                    objRoot.StatusId = common.myInt(ddlStatus.SelectedValue);
                    objRoot.VisitType = common.myStr(ddlSource.SelectedValue);
                    objRoot.IsChronic = chkChronics.Checked;
                    objRoot.DiagnosisId = 0;

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];


                    gvDiagnosisHistory.DataSource = dt;
                    gvDiagnosisHistory.DataBind();
                }
                else
                {
                    //BindBlnkGrid();
                    gvDiagnosisHistory.DataSource = ds.Tables[0];
                    gvDiagnosisHistory.DataBind();
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

    protected void gvDiagnosisHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void gvDiagnosisHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblPrimary = (Label)e.Row.FindControl("lblPrimary");
            if (lblPrimary.Text != "")
            {
                lblPrimary.Text = lblPrimary.Text.ToLower() == "true" ? "Y" : string.Empty;
            }

            Label lblChronic = (Label)e.Row.FindControl("lblChronic");
            if (lblChronic.Text != "")
            {
                lblChronic.Text = lblChronic.Text.ToLower() == "true" ? "Y" : string.Empty;
            }

            Label lblResolved = (Label)e.Row.FindControl("lblResolved");
            if (lblResolved.Text != "")
            {
                lblResolved.Text = lblResolved.Text.ToLower() == "true" ? "Y" : string.Empty;
            }
        }

        if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Header))
        {
            e.Row.Cells[1].Visible = false; //Id
            e.Row.Cells[2].Visible = false;//ICDID

            e.Row.Cells[9].Visible = false;//Onset Date
            e.Row.Cells[10].Visible = false;//Location
            e.Row.Cells[11].Visible = false;//Type
            e.Row.Cells[12].Visible = false;//Condition(s)

            e.Row.Cells[17].Visible = false;//DoctorId
            e.Row.Cells[18].Visible = false;//FacilityId
            e.Row.Cells[19].Visible = false;//TypeId
            e.Row.Cells[20].Visible = false;//ConditionIds
            e.Row.Cells[21].Visible = false;//Date Modified
        }
    }

    protected void gvPatientHistory_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item)
        {

        }
    }

    protected void gvPatientHistory_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
            ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
            Session["RegistrationID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();

            if (e.CommandName == "Add")
            {

                if (common.myStr(ViewState["EncounterId"]) != "" && common.myStr(ViewState["RegistrationId"]) != "")
                {
                    //RadWindowForNew.NavigateUrl = "/EMR/Masters/ViewPatientHistory.aspx?RegId=" + common.myStr(ViewState["RegistrationId"]) + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "";
                    //RadWindowForNew.Height = 600;
                    //RadWindowForNew.Width = 900;
                    //RadWindowForNew.Top = 20;
                    //RadWindowForNew.Left = 20;
                    //// RadWindowForNew.Title = "Time Slot";
                    //RadWindowForNew.OnClientClose = "OnClientClose";
                    //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindowForNew.Modal = true;
                    //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                    //RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Patient!";
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

    protected void btnClearFilter_Click(object o, EventArgs e)
    {
        ddlProvider.SelectedIndex = 0;
        ddlFacility.SelectedIndex = 0;
        ddldateRange.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlSource.SelectedIndex = 0;
        chkChronics.Checked = false;

        dtpfromDate.DateInput.Text = "";
        dtpfromDate.SelectedDate = null;
        dtpToDate.DateInput.Text = "";
        dtpToDate.SelectedDate = null;
        ddldateRange_OnSelectedIndexChanged(o, e);
        BindgvDiagnosisHistory();
        lblMessage.Text = "";
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindgvDiagnosisHistory();
    }

    private DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ICDID");
        dt.Columns.Add("EncounterDate");
        dt.Columns.Add("ICDCode");
        dt.Columns.Add("ICDDescription");
        dt.Columns.Add("DiagnosisStatus");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("FacilityName");
        dt.Columns.Add("IsChronic");
        dt.Columns.Add("IsResolved");
        dt.Columns.Add("ModifierDate");
        return dt;
    }

    private void BindBlnkGrid()
    {
        try
        {
            DataTable datatable = CreateTable();
            DataRow datarow = datatable.NewRow();
            datatable.Rows.Add(datarow);
            gvDiagnosisHistory.DataSource = datatable;
            gvDiagnosisHistory.DataBind();
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

    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
       
        if (ddldateRange.SelectedValue == "6")
        {
            pnlDatarng.Visible = true;


            dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

        }
        else
        {
            pnlDatarng.Visible = false;

            dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.Text = "";
            dtpfromDate.SelectedDate = null;

            dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpToDate.DateInput.Text = "";
            dtpToDate.SelectedDate = null;

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder objXML = new StringBuilder();
            Label lblIcdId = null;
            clsIVF Parse = new clsIVF(string.Empty);
            ParseData Parse1 = new ParseData();
            foreach (GridDataItem item in gvDiagnosisHistory.MasterTableView.Items)
            {
                CheckBox chkRow = (CheckBox)item["View"].FindControl("CheckBox1");

                if (chkRow.Checked == true)
                {
                    lblIcdId = (Label)item.FindControl("lblIcdId");
                    Label lblOnsetDate = (Label)item.FindControl("lblOnsetDate");
                    Label lblSide = (Label)item.FindControl("lblSide");

                    Label lblPrimary = (Label)item.FindControl("lblPrimary");
                    Label lblChronic = (Label)item.FindControl("lblChronic");

                    Label lblddlStatus = (Label)item.FindControl("lblddlStatus");//
                    Label lblddlType = (Label)item.FindControl("lblType");//   
                    Label lblResolved = (Label)item.FindControl("lblResolved");//
                    Label lblICDCode = (Label)item.FindControl("lblICDCode");

                    Label lblddlLocation = (Label)item.FindControl("lblddlLocation");
                    Label lblComments = (Label)item.FindControl("lblRemarks");
                    Label lblId = (Label)item.FindControl("lblId");
                    LinkButton lblDescription = (LinkButton)item.FindControl("lblDescription");

                    HiddenField hdnDoctorId = (HiddenField)item.FindControl("hdnDoctorId");
                    HiddenField hdnFacilityId = (HiddenField)item.FindControl("hdnFacilityId");

                    HiddenField TypeId = (HiddenField)item.FindControl("TypeId");
                    HiddenField hdnConditionIds = (HiddenField)item.FindControl("hdnConditionIds");

                    objXML.Append("<Table1><c1>");
                    objXML.Append("0");
                    objXML.Append("</c1><c2>");
                    objXML.Append(Parse.ParseQ(lblIcdId.Text.Trim()));
                    objXML.Append("</c2><c3>");
                    if (lblPrimary.Text.Trim().ToUpper() == "TRUE" || lblPrimary.Text.Trim() == "Y")
                    {
                        objXML.Append("1");
                    }
                    else
                    {
                        objXML.Append("0");
                    }

                    objXML.Append("</c3><c4>");
                    if (common.myStr(lblChronic.Text) == "Y")
                    {
                        objXML.Append(1);
                    }
                    else
                    {
                        objXML.Append(0);
                    }
                    objXML.Append("</c4>");
                    objXML.Append("<c5>0</c5><c6>");
                    if (lblOnsetDate.Text == "")
                    {
                        objXML.Append("");
                    }
                    else
                    {
                        objXML.Append(Parse1.FormatDate(lblOnsetDate.Text));
                    }
                    objXML.Append("</c6><c7>");
                    if (lblSide.Text.Trim() == "Left")
                    {
                        objXML.Append("1");
                    }
                    else if (lblSide.Text.Trim() == "Right")
                    {
                        objXML.Append("2");
                    }
                    else if (lblSide.Text.Trim() == "Front")
                    {
                        objXML.Append("3");
                    }
                    else if (lblSide.Text.Trim() == "Back")
                    {
                        objXML.Append("4");
                    }
                    else if (lblSide.Text.Trim() == "")
                    {
                        objXML.Append("");
                    }
                    else
                    {
                        objXML.Append("5");
                    }
                    objXML.Append("</c7><c8>");
                    objXML.Append(hdnDoctorId.Value);//Doctor Id
                    objXML.Append("</c8><c9>");
                    objXML.Append(hdnFacilityId.Value);//Facility Id
                    objXML.Append("</c9><c10>");
                    objXML.Append(Parse.ParseQ(lblComments.Text.Trim()));
                    objXML.Append("</c10><c11></c11><c12>");
                    if (TypeId.Value.Trim() == "&nbsp;")
                    {
                        objXML.Append("");
                    }
                    else
                    {
                        objXML.Append(TypeId.Value.Trim());
                    }
                    objXML.Append("</c12><c13>");
                    if (lblResolved.Text.Trim() == "Y")
                    {
                        objXML.Append("1");
                    }
                    else
                    {
                        objXML.Append("0");
                    }
                    objXML.Append("</c13></Table1>");


                    //ds = objDiag.CheckDuplicateProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(lblIcdId.Text), common.myBool(chkChronics.Checked));
                    WebClient client1 = new WebClient();
                    client1.Headers["Content-type"] = "application/json";
                    client1.Encoding = Encoding.UTF8;
                    string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/CheckDuplicateDiagnosis";
                    APIRootClass.CheckDuplicateDiagnosis objRoot1 = new global::APIRootClass.CheckDuplicateDiagnosis();
                    objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
                    objRoot1.EncounterId = common.myInt(Session["EncounterId"]);
                    objRoot1.DiagnosisId = common.myInt(lblIcdId.Text);
                    objRoot1.IsChronic = chkChronics.Checked;

                    string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
                    string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                    sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                    DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue1);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (btnRefresh.Text == "Filter")
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (lblIcdId.Text.ToString().Trim() == ds.Tables[0].Rows[i]["icdid"].ToString().Trim())
                                {
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                                    lblMessage.Text = "This  (" + lblDescription.Text.Trim() + ")  already exists!";
                                    return;
                                }
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Diagnosis Already Exist...";
                        return;
                    }
                    //BaseC.EMROrders objEOdr = new BaseC.EMROrders(sConString);
                    ds = new DataSet();
                    //ds = objEOdr.GetICDCode(lblICDCode.Text.Trim());

                    client1 = new WebClient();
                    client1.Headers["Content-type"] = "application/json";
                    client1.Encoding = Encoding.UTF8;
                    ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/GetICDCode";
                    APIRootClass.GetICDCode objRoot2 = new global::APIRootClass.GetICDCode();
                    objRoot2.ICDCode = common.myStr(lblICDCode.Text);
                    inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot2);
                    sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                    sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                    ds = JsonConvert.DeserializeObject<DataSet>(sValue1);


                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "This  (" + lblDescription.Text.Trim() + ") Diagnosis is Deactivated!";
                            return;
                        }
                    }
                }
            }
            if (objXML.ToString() != "")
            {
                string doctorid = null;
                if (common.myStr(Session["DoctorID"]) != "")
                {
                    doctorid = common.myStr(Session["DoctorID"]);
                }

                string strsave = "";
                //strsave = objDiag.EMRSavePatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), 
                //    common.myInt(Session["EncounterId"]), doctorid, common.myInt(ViewState["PageId"]), objXML.ToString(), "", common.myInt(Session["UserId"]), false, false, 0);

                WebClient client1 = new WebClient();
                client1.Headers["Content-type"] = "application/json";
                client1.Encoding = Encoding.UTF8;
                string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientDiagnosis";
                APIRootClass.SavePatientDiagnosis objRoot2 = new global::APIRootClass.SavePatientDiagnosis();
                objRoot2.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot2.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot2.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot2.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot2.DoctorId = doctorid;
                objRoot2.PageId = common.myInt(ViewState["PageId"]);
                objRoot2.DiagnosisXML = objXML.ToString();
                objRoot2.PatientAlertXML = "";
                objRoot2.UserId = common.myInt(Session["UserId"]);
                objRoot2.IsPullDiagnosis = false;
                objRoot2.IsShowNote = false;
                objRoot2.MRDCode = 0;

                string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot2);
                string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                strsave = JsonConvert.DeserializeObject<string>(sValue1);


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
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Pleaze select the diagnosis to add...";
                return;
            }

            BindgvDiagnosisHistory();
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
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Assessment/Diagnosis.aspx?From=" + common.myStr(Request.QueryString["From"]), false);
    }
    protected void lnkbtnProblem_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkBtn = (LinkButton)sender;
        string lnkbtnProblem = common.myStr(((LinkButton)lnkBtn.FindControl("lblDescription")).Text);
        Label lblId=(Label)lnkBtn.FindControl("lblId");
        Label lblIcdId = (Label)lnkBtn.FindControl("lblIcdId");      
        HiddenField hdnEncounterId = (HiddenField)lnkBtn.FindControl("hdnEncounterId");
        HiddenField hdnRegistrationId = (HiddenField)lnkBtn.FindControl("hdnRegistrationId");
        HiddenField hdnDoctorId = (HiddenField)lnkBtn.FindControl("hdnDoctorId");
        HiddenField hdnFacilityId = (HiddenField)lnkBtn.FindControl("hdnFacilityId");
        HiddenField hdnChronic = (HiddenField)lnkBtn.FindControl("hdnChronic");

        if (common.myInt(lblIcdId.Text) > 0)
        {
            RadWindowForNew.NavigateUrl = "/EMR/Assessment/PopupDiagnosisdetails.aspx?DiagId=" + lblId.Text + "&RegId=" + hdnRegistrationId.Value + "&EncId=" + hdnEncounterId.Value + "&FacilityId=" + hdnFacilityId.Value + "&DoctorID=" + hdnDoctorId.Value + "&Chronic=" + hdnChronic.Value + "&DName=" + lnkbtnProblem;
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 750;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            //    RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Default;
        }

    }

}