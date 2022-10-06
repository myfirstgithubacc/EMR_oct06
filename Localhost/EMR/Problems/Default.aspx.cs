using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Problems_Default : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private string UtdLink = ConfigurationManager.ConnectionStrings["UTDLink"].ConnectionString;
    //clsExceptionLog objException = new clsExceptionLog();
    //private Hashtable hshInput;
    //BaseC.ParseData Parse = new BaseC.ParseData();
    //BaseC.EMRProblems objbc2;
    int iIsChronic = 0;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        switch (common.myStr(Request.QueryString["From"]).ToUpper())
        {
            case "POPUP":
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
                break;
            case "STATICTEMPLATE":
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
                break;
            default:
                Page.MasterPageFile = "/Include/Master/EMRMaster.master";
                break;
        }

        if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //BaseC.Security AuditCA = new BaseC.Security(sConString);
        try
        {
            if (common.myLen(Session["HospitalLocationID"]).Equals(0))
                Response.Redirect("/Login.aspx?Logout=1", false);
            if (common.myStr(Request.QueryString["From"]) == "POPUP")
                btnClose.Visible = true;

            btnSentenceGallery.Attributes.Add("onclick", "return openRadWindow('" + txtSentenceGallery.ClientID + "')");

            Page.Form.DefaultButton = btnSave.UniqueID;
            lblMessage.Text = "";
            if (common.myLen(Session["encounterid"]).Equals(0))
                Response.Redirect("/Default.aspx?RegNo=0", false);
            if (common.myLen(Request.QueryString["Mpg"]) > 0)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, common.myLen(Session["CurrentNode"]) - 1);
            }
            else
            {
                if (common.myLen(Session["Pid"]) > 0)
                    ViewState["PageId"] = Session["Pid"];
                else
                    ViewState["PageId"] = "98";
            }

            if (!IsPostBack)
            {
                tr2.Visible = false;
                tr3.Visible = false;
                tr4.Visible = false;
                tr5.Visible = false;
                btnRemove2.Visible = false;
                dvConfirmCancelOptions.Visible = false;
                dvChronic.Visible = false;
                btnNew.Visible = false;
                hdnIsUnSavedData.Value = "0";
                RetrievePatientProblemsDetail();
                if (common.myLen(Request.QueryString["Id"]) > 0)
                    BindRequestProblemDetails(common.myInt(Request.QueryString["Id"]), common.myInt(Request.QueryString["IsChronic"]));

                txtSCTId.Attributes.Add("onblur", "nSat=1;");

                BindFavouriteList(common.myStr(txtSearchFav.Text));
                rblBTN.SelectedIndex = 0;
                ViewState["BTN"] = "ALL";
                btnAddToFavourite.Visible = true;
                btnRemovefromFavorites.Visible = false;
                rblBTN_SelectedIndexChanged(null, null);

                bindSearchCode();
                BindPullFarwordAndRemarks();
                BindProvider();
                BindPatientHiddenDetails();
                if (common.myLen(Session["IsMedicalAlert"]).Equals(0))
                {
                    lnkAlerts.Enabled = false;
                    lnkAlerts.CssClass = "blinkNone";
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
                }
                else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
                {
                    lnkAlerts.Enabled = true;
                    lnkAlerts.Font.Bold = true;
                    lnkAlerts.CssClass = "blink";
                    lnkAlerts.Font.Size = 11;
                    lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
                }

                if (common.myLen(Request.QueryString["Mpg"]) > 0)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    //ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, Session["CurrentNode"].ToString().Length - 1);

                    if (Session["CurrentNode"].ToString().Contains(","))
                    {
                        ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, Session["CurrentNode"].ToString().IndexOf(',') - 1);
                    }
                    else
                    {
                        ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, Session["CurrentNode"].ToString().Length - 1);
                    }
                }
                else
                {
                    ViewState["PageId"] = "6";
                }

                //AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), 
                //    Convert.ToInt32(Session["RegistrationId"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, 
                //    Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/Common/AuditCommonAccess";
                APIRootClass.AuditCommonAccess objRoot = new global::APIRootClass.AuditCommonAccess();
                objRoot.HospitalLocationId = Convert.ToInt16(Session["HospitalLocationID"]);
                objRoot.FacilityId = Convert.ToInt32(Session["FacilityID"]);
                objRoot.RegistrationId = Convert.ToInt32(Session["RegistrationId"]);
                objRoot.EncounterId = Convert.ToInt32(Session["encounterid"]);
                objRoot.PageId = Convert.ToInt32(ViewState["PageId"]);
                objRoot.TemplateId = 0;
                objRoot.EncodedBy = Convert.ToInt32(Session["UserID"]);
                objRoot.EmployeeId = 0;
                objRoot.AuditStatus = "ACCESSED";
                objRoot.IPAddress = Convert.ToString(Session["IPAddress"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);


                BindODLQCS();

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
                {
                    btnSaveRemarks.Visible = false;
                    btnRemovefromFavorites.Visible = false;
                    btnAddtogrid.Visible = false;
                    gvChronicProblemDetails.Enabled = false;
                    gvProblemDetails.Enabled = false;
                }

                SetPermission();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
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
            HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }

    private void BindPullFarwordAndRemarks()
    {

        //objbc2 = new BaseC.EMRProblems(sConString);
        DataTable dt = new DataTable();
        try
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindPullFarwordAndRemarks";
            APIRootClass.BindPullFarwordAndRemarks objRoot = new global::APIRootClass.BindPullFarwordAndRemarks();
            objRoot.EncounterId = common.myInt(Session["encounterid"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dt = (JsonConvert.DeserializeObject<DataSet>(sValue)).Tables[0];
            //dt = (objbc2.BindPullFarwordAndRemarks(common.myInt(Session["encounterid"]))).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtSentenceGallery.Text = common.myStr(dt.Rows[0]["HPIRemarks"]);
                if (Convert.ToBoolean(dt.Rows[0]["PullForwardComplain"]) == true)
                {
                    chkPullForward.Checked = true;
                }
                else
                {
                    chkPullForward.Checked = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }
    void BindPatientHiddenDetails()
    {
        try
        {
            if (common.myLen(Session["PatientDetailString"]) > 0)
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    public void HandleException(Exception Ex)
    {
        clsExceptionLog objException = new clsExceptionLog();
        objException.HandleException(Ex);
        objException = null;
    }
    #region // Private method to Bind Data into Grids(gvProblemDetails,gvChronicProblemDetais)
    private void RetrievePatientProblemsDetail()
    {

        //objbc2 = new BaseC.EMRProblems(sConString);
        //BaseC.Security AuditCA = new BaseC.Security(sConString);
        DataView dvStTemplate = new DataView();
        DataSet objDs = new DataSet();
        DataTable dtChronicProblemDetail = new DataTable();
        try
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetChiefProblem";
            APIRootClass.GetChiefProblem objRoot = new global::APIRootClass.GetChiefProblem();
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DoctorId = 0;
            objRoot.Daterange = "";
            objRoot.FromDate = "";
            objRoot.ToDate = "";
            objRoot.SearchCriteriya = "%%";
            objRoot.IsDistinct = false;
            objRoot.IsChronic = common.myBool(chkChronics.Checked);
            objRoot.ProblemId = 0;
            objRoot.VisitType = "A";

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //objDs = (DataSet)objbc2.GetChiefProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), 0, 
            //    "", "", "", "%%", false, common.myBool(chkChronics.Checked), 0, "A");
            if (objDs.Tables.Count > 0)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    txtSentenceGallery.Text = common.myStr(objDs.Tables[0].Rows[0]["HPIRemarks"]);

                    dvStTemplate = new DataView(objDs.Tables[0]);
                    if (common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate"))
                    {
                        dvStTemplate.RowFilter = "ISNULL(TemplateFieldId,0)<>0";
                    }

                    DataView dvProblemsDetail = new DataView(dvStTemplate.ToTable());
                    dvProblemsDetail.RowFilter = "IsChronic=1 AND EncounterId=" + common.myInt(Session["EncounterId"]);

                    dtChronicProblemDetail = dvProblemsDetail.ToTable();
                    if (dtChronicProblemDetail.Rows.Count > 0)
                    {
                        gvChronicProblemDetails.DataSource = dtChronicProblemDetail;
                        gvChronicProblemDetails.DataBind();
                    }
                    else
                    {
                        BindBlankChronicProblemDetailGrid();
                    }

                    dvProblemsDetail.RowFilter = string.Empty;

                    dvProblemsDetail.RowFilter = "IsChronic<>1 AND EncounterId=" + common.myInt(Session["EncounterId"]);
                    dtChronicProblemDetail = dvProblemsDetail.ToTable();
                    if (dtChronicProblemDetail.Rows.Count > 0)
                    {
                        gvProblemDetails.DataSource = dtChronicProblemDetail;
                        gvProblemDetails.DataBind();
                    }
                    else
                    {
                        BindBlankProblemDetailGrid();
                    }

                    dvProblemsDetail.RowFilter = "EncounterId=" + common.myInt(Session["EncounterId"]);
                    dtChronicProblemDetail = dvProblemsDetail.ToTable();
                    if (dtChronicProblemDetail.Rows.Count > 0)
                    {
                        ViewState["Record"] = 1;
                    }
                    else
                    {
                        ViewState["Record"] = 0;
                    }
                }
            }
            else
            {
                ViewState["Record"] = 0;
                BindBlankChronicProblemDetailGrid();
                BindBlankProblemDetailGrid();
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            //AuditCA = null;
            dvStTemplate.Dispose();
            objDs.Dispose();
            dtChronicProblemDetail.Dispose();
        }
    }

    #endregion


    /// <summary>
    /// will call on Gridview 'gvProblems Selectindexchnaged
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    /// 

    #region Private method for Create DataTable dynamically
    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        try
        {
            Dt.Columns.Add("SNo").AutoIncrement = true;
            // Dt.Columns["SNo"].AutoIncrement = true;
            Dt.Columns["SNo"].AutoIncrementSeed = 1;
            Dt.Columns["SNo"].AutoIncrementStep = 1;
            Dt.Columns.Add("ID");
            Dt.Columns.Add("ProblemID");
            Dt.Columns.Add("ProblemDescription");
            Dt.Columns.Add("IsHPI");
            Dt.Columns.Add("LocationID");
            Dt.Columns.Add("Location");
            Dt.Columns.Add("OnsetID");
            Dt.Columns.Add("Onset");
            Dt.Columns.Add("DurationID");
            Dt.Columns.Add("Duration");
            Dt.Columns.Add("QualityId1");
            Dt.Columns.Add("Quality");
            Dt.Columns.Add("ContextID");
            Dt.Columns.Add("Context");
            Dt.Columns.Add("SeverityID");
            Dt.Columns.Add("Severity");

            Dt.Columns.Add("Side");
            Dt.Columns.Add("SideDescription");
            Dt.Columns.Add("Condition");
            Dt.Columns.Add("ConditionID");
            Dt.Columns.Add("Percentage");

            Dt.Columns.Add("AssociatedProblemID1");
            Dt.Columns.Add("AssociatedProblem1");
            Dt.Columns.Add("AssociatedProblemID2");
            Dt.Columns.Add("AssociatedProblem2");
            Dt.Columns.Add("AssociatedProblemID3");
            Dt.Columns.Add("AssociatedProblem3");
            Dt.Columns.Add("AssociatedProblemID4");
            Dt.Columns.Add("AssociatedProblem4");
            Dt.Columns.Add("AssociatedProblemID5");
            Dt.Columns.Add("AssociatedProblem5");

            Dt.Columns.Add("DoctorId");
            Dt.Columns.Add("FacilityId");
            Dt.Columns.Add("IsPrimary");
            Dt.Columns.Add("IsChronic");
            Dt.Columns.Add("SNOMEDCode");
            Dt.Columns.Add("Durations");
            Dt.Columns.Add("DurationType");
            Dt.Columns.Add("TemplateFieldId");
            Dt.Columns.Add("ComplaintSearchId");
            Dt.Columns.Add("EncodedBy", System.Type.GetType("System.Int32"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }

        return Dt;
    }
    #endregion

    #region // Grid 'gvProblemDetails 'RowDatabound Event
    protected void gvProblemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
                e.Row.Cells[19].Visible = false;
                e.Row.Cells[20].Visible = false;
                e.Row.Cells[21].Visible = false;
                e.Row.Cells[22].Visible = false;
                e.Row.Cells[25].Visible = false;
                e.Row.Cells[26].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lblProblem = (LinkButton)e.Row.FindControl("lblProblem");
                LinkButton lb = (LinkButton)e.Row.Cells[23].Controls[0];
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                if (lblProblem.Text == "0" || lblProblem.Text == "")
                {
                    lb.Enabled = false;
                    ibtnDelete.Enabled = false;
                }
                if (lblProblem != null)
                {
                    lblProblem.ToolTip = lblProblem.Text;
                }

                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[23].Controls[0];

                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                        ibtnDelete.Visible = false;
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }
    #endregion

    #region  // Grid 'gvProblemDetails ' RowCommand eg. 'Del'

    protected void gvProblemDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Del"))
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                ViewState["strId"] = row.Cells[1].Text;

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;
                }

                if (common.myLen(ViewState["strId"]) > 0)
                {
                    dvConfirmCancelOptions.Visible = true;
                }
            }
            else if (e.CommandName == "HPI" || e.CommandName == "ISHPI")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                if (common.myInt(row.Cells[1].Text) > 0)
                {
                    RadWindowForNew.NavigateUrl = "HPI.aspx?MP=NO&ProbId=" + row.Cells[1].Text + "&Popup=Yes";
                    RadWindowForNew.Height = 500;
                    RadWindowForNew.Width = 600;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.VisibleOnPageLoad = true;
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    #endregion

    #region // Event:Save into data


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
            RetrievePatientProblemsDetail();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }
    #endregion

    #region // Private method Save Data
    private void Save()
    {
        //BaseC.ParseData Parse = new BaseC.ParseData();
        try
        {
            StringBuilder objXML = new StringBuilder();
            foreach (GridViewRow gvr in gvProblemDetails.Rows)
            {
                Label lblProblem = (Label)gvr.FindControl("lblProblem");
                if (common.myLen(lblProblem.Text) > 0)
                {
                    Label lblProblemId = (Label)gvr.FindControl("lblProblemId");
                    LinkButton lblLocation = (LinkButton)gvr.FindControl("lblLocation");
                    Label lblLocationId = (Label)gvr.FindControl("lblLocationId");
                    Label lblOnset = (Label)gvr.FindControl("lblOnset");
                    Label lblOnsetId = (Label)gvr.FindControl("lblOnsetId");

                    Label lblDuration = (Label)gvr.FindControl("lblDuration");
                    Label lblDurationId = (Label)gvr.FindControl("lblDurationId");
                    Label lblQuality = (Label)gvr.FindControl("lblQuality");
                    Label lblQualityId = (Label)gvr.FindControl("lblQualityId");
                    Label lblContext = (Label)gvr.FindControl("lblContext");
                    Label lblContextId = (Label)gvr.FindControl("lblContextId");
                    Label lblSeverity = (Label)gvr.FindControl("lblSeverity");
                    Label lblSeverityId = (Label)gvr.FindControl("lblSeverityId");
                    Label lblProvider = (Label)gvr.FindControl("lblProvider");
                    Label lblFacility = (Label)gvr.FindControl("lblFacility");
                    Label lblPrimary = (Label)gvr.FindControl("lblPrimary");
                    Label lblChronic = (Label)gvr.FindControl("lblChronic");
                    Label lblSCTIdGv = (Label)gvr.FindControl("lblSCTId");

                    Label lblSide = (Label)gvr.FindControl("lblSide");
                    Label lblSideDescription = (Label)gvr.FindControl("lblSideDescription");
                    Label lblConditionId = (Label)gvr.FindControl("lblConditionId");
                    Label lblCondition = (Label)gvr.FindControl("lblCondition");

                    Label lblAssociatedProblemID1 = (Label)gvr.FindControl("lblAssociatedProblemID1");
                    Label lblAssociatedProblem1 = (Label)gvr.FindControl("lblAssociatedProblem1");
                    Label lblAssociatedProblemID2 = (Label)gvr.FindControl("lblAssociatedProblemID2");
                    Label lblAssociatedProblem2 = (Label)gvr.FindControl("lblAssociatedProblem2");
                    Label lblAssociatedProblemID3 = (Label)gvr.FindControl("lblAssociatedProblemID3");
                    Label lblAssociatedProblem3 = (Label)gvr.FindControl("lblAssociatedProblem3");
                    Label lblAssociatedProblemID4 = (Label)gvr.FindControl("lblAssociatedProblemID4");
                    Label lblAssociatedProblem4 = (Label)gvr.FindControl("lblAssociatedProblem4");
                    Label lblAssociatedProblemID5 = (Label)gvr.FindControl("lblAssociatedProblemID5");
                    Label lblAssociatedProblem5 = (Label)gvr.FindControl("lblAssociatedProblem5");




                    objXML.Append("<Table1><c1>");
                    objXML.Append(HttpUtility.HtmlDecode(gvr.Cells[1].Text.ToString().Trim()));
                    objXML.Append("</c1><c2>");
                    objXML.Append(common.ParseString(lblProblemId.Text.Trim()));
                    objXML.Append("</c2><c3>");
                    objXML.Append(common.ParseString(lblProblem.Text.Trim()));
                    objXML.Append("</c3><c4>");
                    objXML.Append(lblDurationId.Text.Trim());
                    objXML.Append("</c4><c5>");
                    objXML.Append(common.ParseString(lblDuration.Text.Trim()));
                    objXML.Append("</c5><c6>");
                    objXML.Append(lblContextId.Text.Trim());
                    objXML.Append("</c6><c7>");
                    objXML.Append(common.ParseString(lblContext.Text.Trim()));
                    objXML.Append("</c7><c8>");
                    objXML.Append(lblSeverityId.Text.Trim());
                    objXML.Append("</c8><c9>");
                    objXML.Append(common.ParseString(lblSeverity.Text.Trim()));
                    objXML.Append("</c9><c10>");
                    objXML.Append(common.ParseString(lblPrimary.Text.Trim()));
                    objXML.Append("</c10><c11>");
                    objXML.Append(common.ParseString(lblChronic.Text.Trim()));
                    objXML.Append("</c11><c12>");
                    objXML.Append(common.ParseString(lblProvider.Text.Trim()));
                    objXML.Append("</c12><c13>");
                    objXML.Append(common.ParseString(lblFacility.Text.Trim()));
                    objXML.Append("</c13><c14>");
                    objXML.Append(common.ParseString(lblSCTIdGv.Text.Trim()));
                    objXML.Append("</c14><c15>");
                    objXML.Append(common.ParseString(lblQuality.Text.Trim()));
                    objXML.Append("</c15><c16>");
                    objXML.Append(lblLocationId.Text.Trim());
                    objXML.Append("</c16><c17>");
                    objXML.Append(common.ParseString(lblLocation.Text.Trim()));
                    objXML.Append("</c17><c18>");
                    objXML.Append(lblOnsetId.Text.Trim());
                    objXML.Append("</c18><c19>");
                    objXML.Append(lblAssociatedProblemID1.Text.Trim());
                    objXML.Append("</c19><c20>");
                    objXML.Append(lblAssociatedProblem1.Text.Trim());
                    objXML.Append("</c20><c21>");
                    objXML.Append(lblAssociatedProblemID2.Text.Trim());
                    objXML.Append("</c21><c22>");
                    objXML.Append(lblAssociatedProblem2.Text.Trim());
                    objXML.Append("</c22><c23>");
                    objXML.Append(lblAssociatedProblemID3.Text.Trim());
                    objXML.Append("</c23><c24>");
                    objXML.Append(lblAssociatedProblem3.Text.Trim());
                    objXML.Append("</c24><c25>");
                    objXML.Append(lblAssociatedProblemID4.Text.Trim());
                    objXML.Append("</c25><c26>");
                    objXML.Append(lblAssociatedProblem4.Text.Trim());
                    objXML.Append("</c26><c27>");
                    objXML.Append(lblAssociatedProblemID5.Text.Trim());
                    objXML.Append("</c27><c28>");
                    objXML.Append(lblAssociatedProblem5.Text.Trim());
                    objXML.Append("</c28><c29>");
                    objXML.Append(lblSide.Text.Trim());
                    objXML.Append("</c29><c30>");
                    objXML.Append(lblCondition.Text.Trim());
                    objXML.Append("</c30></Table1>");
                }
            }
            foreach (GridViewRow gvr in gvChronicProblemDetails.Rows)
            {
                Label lblProblem = (Label)gvr.FindControl("lblProblem");
                if (lblProblem.Text.ToString().Trim().Length > 0)
                {
                    Label lblProblemId = (Label)gvr.FindControl("lblProblemId");

                    Label lblLocation = (Label)gvr.FindControl("lblLocation");
                    Label lblLocationId = (Label)gvr.FindControl("lblLocationId");
                    Label lblOnset = (Label)gvr.FindControl("lblOnset");
                    Label lblOnsetId = (Label)gvr.FindControl("lblOnsetId");

                    Label lblDuration = (Label)gvr.FindControl("lblDuration");
                    Label lblDurationId = (Label)gvr.FindControl("lblDurationId");
                    Label lblQuality = (Label)gvr.FindControl("lblQuality");
                    Label lblQualityId = (Label)gvr.FindControl("lblQualityId");
                    Label lblContext = (Label)gvr.FindControl("lblContext");
                    Label lblContextId = (Label)gvr.FindControl("lblContextId");
                    Label lblSeverity = (Label)gvr.FindControl("lblSeverity");
                    Label lblSeverityId = (Label)gvr.FindControl("lblSeverityId");
                    Label lblProvider = (Label)gvr.FindControl("lblProvider");
                    Label lblFacility = (Label)gvr.FindControl("lblFacility");
                    Label lblPrimary = (Label)gvr.FindControl("lblPrimary");
                    Label lblChronic = (Label)gvr.FindControl("lblChronic");
                    Label lblSCTIdGv = (Label)gvr.FindControl("lblSCTId");

                    Label lblSide = (Label)gvr.FindControl("lblSide");
                    Label lblSideDescription = (Label)gvr.FindControl("lblSideDescription");
                    Label lblCondition = (Label)gvr.FindControl("lblCondition");
                    Label lblConditionID = (Label)gvr.FindControl("lblConditionID");

                    Label lblAssociatedProblemID1 = (Label)gvr.FindControl("lblAssociatedProblemID1");
                    Label lblAssociatedProblem1 = (Label)gvr.FindControl("lblAssociatedProblem1");
                    Label lblAssociatedProblemID2 = (Label)gvr.FindControl("lblAssociatedProblemID2");
                    Label lblAssociatedProblem2 = (Label)gvr.FindControl("lblAssociatedProblem2");
                    Label lblAssociatedProblemID3 = (Label)gvr.FindControl("lblAssociatedProblemID3");
                    Label lblAssociatedProblem3 = (Label)gvr.FindControl("lblAssociatedProblem3");
                    Label lblAssociatedProblemID4 = (Label)gvr.FindControl("lblAssociatedProblemID4");
                    Label lblAssociatedProblem4 = (Label)gvr.FindControl("lblAssociatedProblem4");
                    Label lblAssociatedProblemID5 = (Label)gvr.FindControl("lblAssociatedProblemID5");
                    Label lblAssociatedProblem5 = (Label)gvr.FindControl("lblAssociatedProblem5");

                    objXML.Append("<Table1><c1>");
                    objXML.Append(HttpUtility.HtmlDecode(gvr.Cells[1].Text.ToString().Trim()));
                    objXML.Append("</c1><c2>");
                    objXML.Append(common.ParseString(lblProblemId.Text.Trim()));
                    objXML.Append("</c2><c3>");
                    objXML.Append(common.ParseString(lblProblem.Text.Trim()));
                    objXML.Append("</c3><c4>");
                    objXML.Append(lblDurationId.Text.Trim());
                    objXML.Append("</c4><c5>");
                    objXML.Append(common.ParseString(lblDuration.Text.Trim()));
                    objXML.Append("</c5><c6>");
                    objXML.Append(lblContextId.Text.Trim());
                    objXML.Append("</c6><c7>");
                    objXML.Append(common.ParseString(lblContext.Text.Trim()));
                    objXML.Append("</c7><c8>");
                    objXML.Append(lblSeverityId.Text.Trim());
                    objXML.Append("</c8><c9>");
                    objXML.Append(common.ParseString(lblSeverity.Text.Trim()));
                    objXML.Append("</c9><c10>");
                    objXML.Append(common.ParseString(lblPrimary.Text.Trim()));
                    objXML.Append("</c10><c11>");
                    objXML.Append(common.ParseString(lblChronic.Text.Trim()));
                    objXML.Append("</c11><c12>");
                    objXML.Append(common.ParseString(lblProvider.Text.Trim()));
                    objXML.Append("</c12><c13>");
                    objXML.Append(common.ParseString(lblFacility.Text.Trim()));
                    objXML.Append("</c13><c14>");
                    objXML.Append(common.ParseString(lblSCTIdGv.Text.Trim()));
                    objXML.Append("</c14><c15>");

                    objXML.Append(lblQuality.Text.Trim());
                    objXML.Append("</c15><c16>");
                    objXML.Append(lblLocationId.Text.Trim());
                    objXML.Append("</c16><c17>");
                    objXML.Append(common.ParseString(lblLocation.Text.Trim()));
                    objXML.Append("</c17><c18>");
                    objXML.Append(lblOnsetId.Text.Trim());
                    objXML.Append("</c18><c19>");
                    objXML.Append(lblAssociatedProblemID1.Text.Trim());
                    objXML.Append("</c19><c20>");
                    objXML.Append(lblAssociatedProblem1.Text.Trim());
                    objXML.Append("</c20><c21>");
                    objXML.Append(lblAssociatedProblemID2.Text.Trim());
                    objXML.Append("</c21><c22>");
                    objXML.Append(lblAssociatedProblem2.Text.Trim());
                    objXML.Append("</c22><c23>");
                    objXML.Append(lblAssociatedProblemID3.Text.Trim());
                    objXML.Append("</c23><c24>");
                    objXML.Append(lblAssociatedProblem3.Text.Trim());
                    objXML.Append("</c24><c25>");
                    objXML.Append(lblAssociatedProblemID4.Text.Trim());
                    objXML.Append("</c25><c26>");
                    objXML.Append(lblAssociatedProblem4.Text.Trim());
                    objXML.Append("</c26><c27>");
                    objXML.Append(lblAssociatedProblemID5.Text.Trim());
                    objXML.Append("</c27><c28>");
                    objXML.Append(lblAssociatedProblem5.Text.Trim());
                    objXML.Append("</c28><c29>");
                    objXML.Append(lblSide.Text.Trim());
                    objXML.Append("</c29><c30>");
                    objXML.Append(lblCondition.Text.Trim());
                    objXML.Append("</c30></Table1>");
                }
            }
            //objbc2 = new BaseC.EMRProblems(sConString);
            if (common.myLen(objXML).Equals(0))
            {
                lblMessage.Text = "Nothing to save";
                return;
            }

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientProblems";
            APIRootClass.SavePatientProblems objRoot = new global::APIRootClass.SavePatientProblems();
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.PageId = 0;
            objRoot.xmlProblemDetails = objXML.ToString();
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.Remarks = "";
            objRoot.IsPregment = false;
            objRoot.IsBreastFeed = false;
            objRoot.IsShowNote = false;
            objRoot.DoctorId = common.myInt(Session["DoctorID"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = JsonConvert.DeserializeObject<string>(sValue);


            //lblMessage.Text = objbc2.EMRSavePatientProblems(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 
            //    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, objXML.ToString(), common.myInt(Session["UserId"]), "", false, false, false, 
            //    common.myInt(Session["DoctorID"]));
            if (lblMessage.Text.Contains("Data Saved!"))
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            else
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            ddlQuality.Text = string.Empty;
            for (int i = 0; i < ddlQuality.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlQuality.Text = "";
            cmbProblemName.Text = string.Empty;
            this.cmbProblemName.Enabled = true;
            ClearProblemDetailsControls();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            //Parse = null;
        }
    }
    #endregion

    #region// Bind GvProblem grid

    protected DataTable BindFavouriteProblems(string txt, string CallFrom)
    {
        DataTable DT = new DataTable();
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);
            string strSearchCriteria = string.Empty;

            strSearchCriteria = "%" + txt + "%";

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindFavouriteProblems";
            APIRootClass.BindFavouriteProblems objRoot = new global::APIRootClass.BindFavouriteProblems();
            objRoot.strSearchCriteria = strSearchCriteria;
            objRoot.DoctorID = common.myInt(Session["DoctorID"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DT = (JsonConvert.DeserializeObject<DataSet>(sValue)).Tables[0];

            //DT = ((DataSet)objbc2.BindFavouriteProblems(strSearchCriteria, common.myInt(Session["DoctorID"]))).Tables[0];

            if (DT.Rows.Count > 0)
            {
                DT.Columns.Add("Id");
                DT.Columns.Add("EncounterDate");
                if (CallFrom == "")
                {
                    btnAddToFavourite.Visible = false;
                    btnRemovefromFavorites.Visible = false;
                }
            }
            else
            {
                DT = BindBlankGrid();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        return DT;
    }

    protected void btnAllProblem_OnClick(Object sender, EventArgs e)
    {
        try
        {
            gvChronicProblemDetails.SelectedIndex = -1;
            gvProblemDetails.SelectedIndex = -1;

            ViewState["BTN"] = "ALL";
            PopulateAllProblem("");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    private DataTable PopulateAllProblem(string txt)
    {
        DataTable DT = new DataTable();

        try
        {
            if (common.myLen(Session["encounterid"]) > 0 && common.myLen(Session["RegistrationID"]) > 0)
            {
                //objbc2 = new BaseC.EMRProblems(sConString);

                //hshInput = new Hashtable();
                string strSearchCriteria = string.Empty;
                strSearchCriteria = "%" + txt + "%";

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/PopulateAllProblem";
                APIRootClass.PopulateAllProblem objRoot = new global::APIRootClass.PopulateAllProblem();
                objRoot.strSearchCriteria = strSearchCriteria;
                objRoot.DoctorID = common.myInt(Session["DoctorID"]);
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                DT = (JsonConvert.DeserializeObject<DataSet>(sValue)).Tables[0];

                //DT = ((DataSet)objbc2.PopulateAllProblem(strSearchCriteria, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["DoctorID"]))).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Id");
                    DT.Columns.Add("EncounterDate");
                    btnRemovefromFavorites.Visible = false;
                    btnAddToFavourite.Visible = true;
                }
                else
                {
                    DT = BindBlankGrid();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }

        return DT;
    }


    protected void btnFavourites_OnClick(Object sender, EventArgs e)
    {
        gvChronicProblemDetails.SelectedIndex = -1;
        gvProblemDetails.SelectedIndex = -1;

        ViewState["BTN"] = "FAV";
        BindFavouriteProblems("", "");
    }
    #endregion

    #region // will call on to Add FavouriteList
    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);
            if (common.myInt(cmbProblemName.SelectedValue) > 0 || cmbProblemName.Text != "")
            {
                if (!common.myStr(ViewState["FavList"]).Equals(""))
                {
                    dt = (DataTable)ViewState["FavList"];
                    dv = dt.DefaultView;
                    if (cmbProblemName.SelectedValue != "")
                    {
                        dv.RowFilter = "ProblemId = " + common.myInt(cmbProblemName.SelectedValue) + "";
                        if (dv.Count > 0)
                        {
                            Alert.ShowAjaxMsg("Problem already exists in Favorites", this);
                            return;
                        }
                    }
                }
                string sDescription = cmbProblemName.SelectedValue == "" ? cmbProblemName.Text : "";

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveEMRFavProblem";
                APIRootClass.SaveEMRFavProblem objRoot = new global::APIRootClass.SaveEMRFavProblem();
                objRoot.ProblemId = common.myInt(cmbProblemName.SelectedValue);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.ProblemDescription = sDescription;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                int i = JsonConvert.DeserializeObject<int>(sValue);

                //int i = objbc2.SaveEMRFavProblem(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["DoctorID"]), common.myInt(cmbProblemName.SelectedValue), 
                //    common.myInt(Session["UserID"]), sDescription);
                if (i == 0)
                {
                    lblMessage.Text = "Problem saved in your favorite list";
                    BindFavouriteList(common.myStr(txtSearchFav.Text));
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Select a Problem to Add in Favorites", this);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            dv.Dispose();
        }
    }
    #endregion

    #region Private method for Bind Blank ProblemDetail gridview 'gvProblemdetail
    private void BindBlankProblemDetailGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ProblemID"] = "";
                dr["ProblemDescription"] = "";
                dr["IsHPI"] = "";
                dr["LocationID"] = "0";
                dr["Location"] = "";
                dr["OnsetID"] = "";
                dr["Onset"] = "";
                dr["DurationID"] = "0";
                dr["Duration"] = "";
                dr["QualityID1"] = "0";
                dr["Quality"] = "";
                dr["ContextID"] = "0";
                dr["Context"] = "";
                dr["SeverityID"] = "0";
                dr["Severity"] = "";

                dr["DoctorId"] = "0";

                dr["FacilityId"] = "0";
                dr["IsPrimary"] = "";
                dr["IsChronic"] = "";
                dr["SNOMEDCode"] = "";
                dr["AssociatedProblemID1"] = "0";//
                dr["AssociatedProblemID2"] = "0";//
                dr["AssociatedProblemID3"] = "0";//
                dr["AssociatedProblemID4"] = "0";//
                dr["AssociatedProblemID5"] = "0";//
                dr["AssociatedProblem1"] = "";//
                dr["AssociatedProblem2"] = "";//
                dr["AssociatedProblem3"] = "";//
                dr["AssociatedProblem4"] = "";//
                dr["AssociatedProblem5"] = "";//
                dr["Side"] = "0";
                dr["SideDescription"] = "";
                dr["ConditionID"] = "0";
                dr["Condition"] = "";
                dr["Percentage"] = "";

                dr["Durations"] = "";
                dr["DurationType"] = "";
                dT.Rows.Add(dr);
            }
            gvProblemDetails.DataSource = dT;
            gvProblemDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    #endregion

    #region Private method for bind Blank Problem gridview 'gvProblem'
    private DataTable BindBlankGrid()
    {
        DataTable dT = new DataTable();
        try
        {

            dT.Columns.Add("Id");
            dT.Columns.Add("ProblemId");
            dT.Columns.Add("ProblemDescription");
            dT.Columns.Add("SNOMEDCode");
            dT.Columns.Add("EncounterDate");
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ProblemId"] = 0;
                switch (Convert.ToString(ViewState["BTN"]))
                {
                    case "ALL":
                        dr["ProblemDescription"] = "No Problem Found";
                        break;

                    case "FAV":
                        dr["ProblemDescription"] = "No Favorite Found";
                        break;

                    default:
                        dr["ProblemDescription"] = "No Data Found";
                        break;
                }
                dr["SNOMEDCode"] = "";
                dT.Rows.Add(dr);
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        return dT;
    }

    #endregion

    #region  //Bind problems Details DropDown

    protected void BindProvider()
    {
        //BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        try
        {
            ddlProviders.Items.Clear();

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

            //ds = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, 0, 0);
            if (ds.Tables.Count > 0)
            {
                ddlProviders.DataSource = ds.Tables[0];
                ddlProviders.DataTextField = "DoctorName";
                ddlProviders.DataValueField = "DoctorId";
                ddlProviders.DataBind();
                RadComboBoxItem rcbDoctorId = (RadComboBoxItem)ddlProviders.Items.FindItemByValue(common.myStr(Session["EmployeeId"]));
                if (rcbDoctorId != null)
                    rcbDoctorId.Selected = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void BindODLQCS()
    {
        //objbc2 = new BaseC.EMRProblems(sConString);
        DataSet ds = new DataSet();
        try
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindODLQCS";
            APIRootClass.BindODLQCS objRoot = new global::APIRootClass.BindODLQCS();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);

            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objbc2.BindODLQCS(common.myInt(Session["HospitalLocationID"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlLocation.DataSource = ds.Tables[0];
                    ddlLocation.DataTextField = "Description";
                    ddlLocation.DataValueField = "id";
                    ddlLocation.DataBind();
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlOnset.DataSource = ds.Tables[1];
                    ddlOnset.DataTextField = "Description";
                    ddlOnset.DataValueField = "id";
                    ddlOnset.DataBind();
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    ddlDuration.DataSource = ds.Tables[2];
                    ddlDuration.DataTextField = "Description";
                    ddlDuration.DataValueField = "id";
                    ddlDuration.DataBind();
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    ddlQuality.DataSource = ds.Tables[3];
                    ddlQuality.DataTextField = "Description";
                    ddlQuality.DataValueField = "id";
                    ddlQuality.DataBind();
                }

                if (ds.Tables[4].Rows.Count > 0)
                {
                    ddlContext.DataSource = ds.Tables[4];
                    ddlContext.DataTextField = "Description";
                    ddlContext.DataValueField = "id";
                    ddlContext.DataBind();
                }

                if (ds.Tables[5].Rows.Count > 0)
                {
                    ddlSeverity.DataSource = ds.Tables[5];
                    ddlSeverity.DataTextField = "Description";
                    ddlSeverity.DataValueField = "id";
                    ddlSeverity.DataBind();
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    ddlCondition.DataSource = ds.Tables[6];
                    ddlCondition.DataTextField = "Description";
                    ddlCondition.DataValueField = "ID";
                    ddlCondition.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    #endregion

    #region  This button will be called when data add to Grids

    protected void btnAddtogrid_Click(object sender, EventArgs e)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //objbc2 = new BaseC.EMRProblems(sConString);
        DataSet ds = new DataSet();
        //hshInput = new Hashtable();
        //Hashtable hshOutput = new Hashtable();
        ArrayList col = new ArrayList();
        StringBuilder objXML = new StringBuilder();
        //BaseC.ParseData Parse = new BaseC.ParseData();
        try
        {
            if (cmbProblemName.Text.Trim() == "")
            {
                hdnIsUnSavedData.Value = "0";
                return;
            }

            if (common.myInt(txtDuration.Text) > 0 && ddlDurationType.SelectedValue.Equals(""))
            {
                Alert.ShowAjaxMsg("Please! Select Duration Type...", this.Page);
                return;
            }

            if (rblShowNote.SelectedIndex == -1)
            {
                Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                return;
            }

            if (btnAddtogrid.Text.Equals("Update List"))
            {
                if (chkChronics.Checked)
                {
                    foreach (GridViewRow gv in gvChronicProblemDetails.Rows)
                    {
                        if (common.myInt(gv.Cells[1].Text) != common.myInt(txtedit.Text))
                        {
                            if (common.myStr(prblmID.Value) == common.myStr(((Label)gv.FindControl("lblProblemId")).Text))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "This  (" + cmbProblemName.Text.Trim() + ")  already exists in Chronics Problems!";
                                hdnIsUnSavedData.Value = "0";
                                return;
                            }
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow gv in gvProblemDetails.Rows)
                    {
                        if (!common.myInt(gv.Cells[1].Text).Equals(common.myInt(txtedit.Text)))
                        {
                            if (common.myStr(prblmID.Value) == common.myStr(((Label)gv.FindControl("lblProblemId")).Text))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "This  (" + cmbProblemName.Text.Trim() + ")  already exists in Today's Problems!";
                                hdnIsUnSavedData.Value = "0";
                                return;
                            }
                        }
                    }
                }
            }

            string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate" ? common.myStr(Request.QueryString["TemplateFieldId"]) : null;
            string Side = ddlSide.SelectedIndex == -1 ? null : ddlSide.SelectedValue;
            string Condition = ddlCondition.SelectedIndex == -1 ? null : ddlCondition.SelectedValue;

            col.Add(txtedit.Text);//Id
            col.Add(common.ParseString(prblmID.Value));//ProblemId
            col.Add(common.ParseString(cmbProblemName.Text.Trim()));//Problem
            col.Add(rdoDurationList.SelectedValue);//DurationID
            if (common.myStr(ddlDurationType.SelectedValue) == "O")
                col.Add(common.ParseString(txtDuration.Text));//Duration            
            else
                col.Add(string.Empty);//Duration

            col.Add(ddlContext.SelectedValue);//ContextID
            col.Add(common.ParseString(ddlContext.Text.Trim()));//Context
            col.Add(ddlSeverity.SelectedValue);//SeverityId
            col.Add(ddlSeverity.Text.Trim());//Severity
            col.Add(chkPrimarys.Checked);//IsPrimary
            col.Add(chkChronics.Checked);//IsChronic
            col.Add(common.ParseString(ddlProviders.SelectedValue));//DoctorId
            col.Add(common.myInt(Session["FacilityId"]));//FacilityId
            col.Add(common.ParseString(txtSCTId.Text));//SCTId
            col.Add(common.ParseString(txtQualityIds.Text));//QualityIDs
            col.Add(ddlLocation.SelectedValue);//LocationID
            col.Add(common.ParseString(ddlLocation.Text.Trim()));//Location
            col.Add(ddlOnset.SelectedValue);//OnsetID
            col.Add(txtAdditionalProblemId1.Text);//AssociatedProblemId1
            col.Add(cmbAdd1.Text.ToString().Trim());//AssociatedProblem1
            col.Add(txtAdditionalProblemId2.Text);//AssociatedProblemId2
            col.Add(cmbAdd2.Text.ToString().Trim());//AssociatedProblem2
            col.Add(txtAdditionalProblemId3.Text);//AssociatedProblemId3
            col.Add(cmbAdd3.Text.ToString().Trim());//AssociatedProblem3
            col.Add(txtAdditionalProblemId4.Text);//AssociatedProblemId4
            col.Add(cmbAdd4.Text.ToString().Trim());//AssociatedProblem4
            col.Add(txtAdditionalProblemId5.Text);//AssociatedProblemId5
            col.Add(cmbAdd5.Text.ToString().Trim());//AssociatedProblem5
            col.Add(Side);//Side
            col.Add(Condition);//ConditionId
            col.Add(TxtPercentage.Text.Trim());//Percentage
            if (common.myStr(ddlDurationType.SelectedValue) == "O")
            {
                col.Add(DBNull.Value);//Durations
            }
            else
            {
                col.Add(common.ParseString(rdoDurationList.SelectedValue));//Durations
            }
            col.Add(ddlDurationType.SelectedValue);//DurationType
            col.Add(TemplateId);//TemplateFieldId
            col.Add(common.myInt(ddlComplaintSearchCodes.SelectedValue));//ComplaintSearchId

            objXML.Append(common.setXmlTable(ref col));

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckDuplicateProblem";
            APIRootClass.CheckDuplicateProblem objRoot = new global::APIRootClass.CheckDuplicateProblem();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.ProblemId = common.myInt(prblmID.Value);
            objRoot.IsChronic = chkChronics.Checked;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objbc2.CheckDuplicateProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 
            //    common.myInt(prblmID.Value), common.myBool(chkChronics.Checked));


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (btnAddtogrid.Text == "Add To List")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (common.myStr(prblmID.Value).Equals(common.myStr(ds.Tables[0].Rows[i]["ProblemId"])))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            if (chkChronics.Checked == true)
                            {
                                lblMessage.Text = "This  (" + cmbProblemName.Text.Trim() + ")  already exists in Chronics's Problems!";
                                hdnIsUnSavedData.Value = "0";
                            }
                            else
                            {
                                lblMessage.Text = "This  (" + cmbProblemName.Text.Trim() + ")  already exists in Today's Problems!";
                                hdnIsUnSavedData.Value = "0";
                            }
                            return;
                        }
                    }
                }

            }
            if (common.myLen(objXML).Equals(0))
            {
                lblMessage.Text = "Nothing to Add";
                return;
            }

            client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientProblems";
            APIRootClass.SavePatientProblems objRoot1 = new global::APIRootClass.SavePatientProblems();
            objRoot1.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot1.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot1.PageId = common.myInt(ViewState["PageId"]);
            objRoot1.xmlProblemDetails = objXML.ToString();
            objRoot1.UserId = common.myInt(Session["UserId"]);
            objRoot1.Remarks = common.myStr(txtSentenceGallery.Text);
            objRoot1.IsPregment = false;
            objRoot1.IsBreastFeed = false;
            objRoot1.IsShowNote = common.myBool(rblShowNote.SelectedItem.Value);
            objRoot1.UserId = common.myInt(Session["UserId"]);

            inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
            sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = JsonConvert.DeserializeObject<string>(sValue);

            //lblMessage.Text = objbc2.EMRSavePatientProblems(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(ViewState["PageId"]),
            //                    objXML.ToString(), common.myInt(Session["UserId"]), common.myStr(txtSentenceGallery.Text), false, false,
            //                    common.myBool(rblShowNote.SelectedItem.Value), common.myInt(Session["DoctorID"]));

            if (lblMessage.Text.Contains("Data Saved!"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
            }
            else
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            hdnIsUnSavedData.Value = "0";
            btnNew.Visible = true;
            txtQualityIds.Text = string.Empty;
            ClearProblemDetailsControls();
            RetrievePatientProblemsDetail();
            gvChronicProblemDetails.SelectedIndex = -1;
            gvProblemDetails.SelectedIndex = -1;

            txtedit.Text = "";

            rblBTN_SelectedIndexChanged(this, null);

            btnAddtogrid.Text = "Add To List";
            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            objXML = null;
            col = null;
            //objDl = null;
            ds.Dispose();
            //hshInput = null;
            //hshOutput = null;
            //Parse = null;
        }
    }
    #endregion




    #region  // Clear ProblemDetails Controls

    private void ClearProblemDetailsControls()
    {
        try
        {

            ViewState["RowIndexP"] = null;
            ViewState["RowIndexC"] = null;
            cmbProblemName.Text = string.Empty;
            this.cmbProblemName.Enabled = true;
            rblBTN.Enabled = true;

            ddlLocation.SelectedValue = "0";
            ddlLocation.Text = string.Empty;
            ddlOnset.SelectedValue = "0";
            ddlOnset.Text = string.Empty;

            ddlDuration.SelectedValue = "0";
            ddlDuration.Text = string.Empty;
            txtDuration.Text = string.Empty;

            txtQualityIds.Text = "";
            ddlQuality.SelectedValue = "0";
            ddlQuality.Text = string.Empty;
            for (int i = 0; i < ddlQuality.Items.Count; i++)
            {
                CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                checkbox.Checked = false;
            }
            ddlQuality.Text = "";

            ddlContext.SelectedValue = "0";
            ddlContext.Text = string.Empty;
            ddlSeverity.SelectedValue = "0";
            ddlSeverity.Text = string.Empty;
            ddlCondition.SelectedValue = "0";
            ddlCondition.Text = string.Empty;
            ddlSide.SelectedValue = "0";
            ddlSide.Text = string.Empty;
            chkChronics.Checked = false;
            chkPrimarys.Checked = false;
            cmbAdd1.Text = "";
            cmbAdd2.Text = "";
            cmbAdd3.Text = "";
            cmbAdd4.Text = "";
            cmbAdd5.Text = "";
            txtAdditionalProblemId1.Text = "0";
            txtAdditionalProblemId2.Text = "0";
            txtAdditionalProblemId3.Text = "0";
            txtAdditionalProblemId4.Text = "0";
            txtAdditionalProblemId5.Text = "0";
            tr2.Visible = false;
            tr3.Visible = false;
            tr4.Visible = false;
            tr5.Visible = false;
            TxtPercentage.Text = "";

            rdoDurationList.SelectedValue = "1";
            ddlDurationType.SelectedValue = "D";
            btnAddToFavourite.Visible = true;
            btnRemovefromFavorites.Visible = false;
            btnNew.Visible = false;
            ddlComplaintSearchCodes.SelectedIndex = 0;

            btnAddtogrid.Text = "Add To List";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    #endregion

    #region  will call when click 'Edit' button of Grid 'gvProblemDetails'

    protected void gvProblemDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnNew.Visible = true;
        btnAddToFavourite.Visible = true;
        btnRemovefromFavorites.Visible = false;
        btnAdd1.Visible = true;
        gvChronicProblemDetails.SelectedIndex = -1;
        ProblemDetailsSelectedIndexChanged();

    }
    #endregion

    #region // Private method for Grid ProblemDetails Selected Index Changed Event

    private void ProblemDetailsSelectedIndexChanged()
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            string strQualityIds = "";

            if (gvProblemDetails.SelectedRow != null)
            {

                ViewState["BTN"] = "ALL";
                rblBTN.SelectedValue = "ALL";
                this.cmbProblemName.Text = "";
                this.cmbProblemName.SelectedValue = "";
                this.cmbProblemName.Enabled = false;
                rblBTN.Enabled = false;

                Label lblProblemId = (Label)gvProblemDetails.SelectedRow.FindControl("lblProblemId");
                LinkButton lblProblem = (LinkButton)gvProblemDetails.SelectedRow.FindControl("lblProblem");
                Label lblLocationId = (Label)gvProblemDetails.SelectedRow.FindControl("lblLocationId");
                Label lblOnset = (Label)gvProblemDetails.SelectedRow.FindControl("lblOnset");
                Label lblOnsetId = (Label)gvProblemDetails.SelectedRow.FindControl("lblOnsetId");
                Label lblDuration = (Label)gvProblemDetails.SelectedRow.FindControl("lblDuration");
                Label lblDurationId = (Label)gvProblemDetails.SelectedRow.FindControl("lblDurationId");
                Label lblQuality = (Label)gvProblemDetails.SelectedRow.FindControl("lblQuality");
                Label lblQualityId = (Label)gvProblemDetails.SelectedRow.FindControl("lblQualityId");
                Label lblContext = (Label)gvProblemDetails.SelectedRow.FindControl("lblContext");
                Label lblContextId = (Label)gvProblemDetails.SelectedRow.FindControl("lblContextId");
                Label lblSeverity = (Label)gvProblemDetails.SelectedRow.FindControl("lblSeverity");
                Label lblSeverityId = (Label)gvProblemDetails.SelectedRow.FindControl("lblSeverityId");
                Label lblProvider = (Label)gvProblemDetails.SelectedRow.FindControl("lblProvider");
                Label lblFacility = (Label)gvProblemDetails.SelectedRow.FindControl("lblFacility");
                Label lblPrimary = (Label)gvProblemDetails.SelectedRow.FindControl("lblPrimary");
                Label lblChronic = (Label)gvProblemDetails.SelectedRow.FindControl("lblChronic");
                Label lblSCTId1 = (Label)gvProblemDetails.SelectedRow.FindControl("lblSCTId");
                Label lblSide = (Label)gvProblemDetails.SelectedRow.FindControl("lblSide");
                Label lblSideDescription = (Label)gvProblemDetails.SelectedRow.FindControl("lblSideDescription");
                Label lblCondition = (Label)gvProblemDetails.SelectedRow.FindControl("lblCondition");
                Label lblConditionID = (Label)gvProblemDetails.SelectedRow.FindControl("lblConditionID");

                Label lblAssociatedProblemID1 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblemID1");
                Label lblAssociatedProblem1 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblem1");
                Label lblAssociatedProblemID2 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblemID2");
                Label lblAssociatedProblem2 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblem2");
                Label lblAssociatedProblemID3 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblemID3");
                Label lblAssociatedProblem3 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblem3");
                Label lblAssociatedProblemID4 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblemID4");
                Label lblAssociatedProblem4 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblem4");
                Label lblAssociatedProblemID5 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblemID5");
                Label lblAssociatedProblem5 = (Label)gvProblemDetails.SelectedRow.FindControl("lblAssociatedProblem5");
                Label lblPercent = (Label)gvProblemDetails.SelectedRow.FindControl("lblPercentage");

                cmbAdd1.Text = lblAssociatedProblem1.Text;
                cmbAdd2.Text = lblAssociatedProblem2.Text;
                cmbAdd3.Text = lblAssociatedProblem3.Text;
                cmbAdd4.Text = lblAssociatedProblem4.Text;
                cmbAdd5.Text = lblAssociatedProblem5.Text;
                txtAdditionalProblemId1.Text = lblAssociatedProblemID1.Text;
                txtAdditionalProblemId2.Text = lblAssociatedProblemID2.Text;
                txtAdditionalProblemId3.Text = lblAssociatedProblemID3.Text;
                txtAdditionalProblemId4.Text = lblAssociatedProblemID4.Text;
                txtAdditionalProblemId5.Text = lblAssociatedProblemID5.Text;

                if (!common.myInt(lblProblemId.Text).Equals(0))
                    prblmID.Value = common.myStr(lblProblemId.Text);

                if (common.myInt(lblPercent.Text).Equals(0))
                    TxtPercentage.Text = "";
                else
                    TxtPercentage.Text = lblPercent.Text;

                if (common.myInt(lblAssociatedProblem2.Text.Trim()).Equals(0))
                {
                    txtAdditionalProblemId2.Text = "0";
                    tr2.Visible = true;
                }
                else
                {
                    txtAdditionalProblemId2.Text = "0";
                    tr2.Visible = false;
                }
                if (common.myInt(lblAssociatedProblem3.Text.Trim()) > 0)
                    tr3.Visible = true;
                else
                {
                    txtAdditionalProblemId3.Text = "0";
                    tr3.Visible = false;
                }
                if (common.myInt(lblAssociatedProblem4.Text.Trim()) > 0)
                    tr4.Visible = true;
                else
                {
                    lblAssociatedProblem4.Text = "0";
                    tr4.Visible = false;
                }
                if (common.myInt(lblAssociatedProblem5.Text.Trim()) > 0)
                    tr5.Visible = true;
                else
                {
                    txtAdditionalProblemId5.Text = "0";
                    tr5.Visible = false;
                }

                ViewState["RowIndexP"] = gvProblemDetails.SelectedRow.RowIndex;
                ViewState["ProblemId"] = lblProblemId.Text;
                cmbProblemName.Text = lblProblem.Text;
                cmbProblemName.SelectedValue = lblProblemId.Text.Trim();


                if (common.myInt(lblLocationId.Text) > 0)
                    ddlLocation.SelectedValue = lblLocationId.Text;
                else
                {
                    ddlLocation.SelectedValue = "0";
                    ddlLocation.Text = string.Empty;
                }
                if (common.myInt(lblOnsetId.Text) > 0)
                    ddlOnset.SelectedValue = lblOnsetId.Text;
                else
                {
                    ddlOnset.SelectedValue = "0";
                    ddlOnset.Text = string.Empty;
                }

                HiddenField hdnDurations = (HiddenField)gvProblemDetails.SelectedRow.FindControl("hdnPDurations");
                HiddenField hdnDurationType = (HiddenField)gvProblemDetails.SelectedRow.FindControl("hdnPDurationType");
                HiddenField hdnComplaintSearchId = (HiddenField)gvProblemDetails.SelectedRow.FindControl("hdnComplaintSearchId");

                ddlComplaintSearchCodes.SelectedIndex = ddlComplaintSearchCodes.Items.IndexOf(ddlComplaintSearchCodes.Items.FindItemByValue(common.myStr(hdnComplaintSearchId.Value)));

                txtDuration.Visible = false;
                ddlDuration.Visible = false;
                if (hdnDurationType.Value != "O")
                {
                    rdoDurationList.Visible = true;
                    txtDuration.Visible = false;
                    if (common.myInt(hdnDurations.Value) > 0)
                    {
                        rdoDurationList.SelectedValue = hdnDurations.Value;
                        ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindByValue(common.myStr(hdnDurationType.Value)));
                    }
                    else
                    {
                        rdoDurationList.SelectedIndex = -1;
                        ddlDurationType.SelectedIndex = -1;
                    }
                }
                else
                {
                    rdoDurationList.Visible = false;
                    txtDuration.Visible = true;
                    txtDuration.Text = common.myStr(hdnDurations.Value);
                    ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindByValue(common.myStr(hdnDurationType.Value)));
                }


                if (common.myInt(lblQuality.Text) > 0)
                {
                    txtQualityIds.Text = lblQuality.Text;
                    ddlQuality.Text = "";

                    strQualityIds = lblQuality.Text;
                    string strQuantityID = "";
                    int i, j;
                    string[] arInfo = new string[4];
                    char[] splitter = { ',' };
                    arInfo = strQualityIds.Split(splitter);
                    for (i = 0; i < ddlQuality.Items.Count; i++)
                    {
                        strQuantityID = ddlQuality.Items[i].Value.ToString().Trim();

                        for (j = 0; j < arInfo.Length; j++)
                        {
                            if (arInfo[j].Trim() == strQuantityID.Trim())
                            {
                                CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                                ddlQuality.Text += ddlQuality.Items[i].Text.ToString() + ",";
                                checkbox.Checked = true;

                            }
                        }
                    }

                    if (ddlQuality.Text.Length > 0)
                    {

                        ddlQuality.Text = ddlQuality.Text.Substring(0, ddlQuality.Text.Length - 1);
                    }

                }
                else
                {
                    ddlQuality.SelectedValue = "0";
                    ddlQuality.Text = string.Empty;
                    for (int i = 0; i < ddlQuality.Items.Count; i++)
                    {
                        CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                        checkbox.Checked = false;
                    }
                    ddlQuality.Text = "";
                }
                if (lblConditionID.Text != "0" && lblConditionID.Text != "")
                {
                    ddlCondition.SelectedValue = lblConditionID.Text;
                    ddlCondition.Visible = true;

                }
                else
                {
                    ddlCondition.SelectedValue = "0";
                    ddlCondition.Text = string.Empty;
                }
                if (lblSide.Text != "0" && lblSide.Text != "-")
                {
                    ddlSide.SelectedValue = lblSide.Text;
                    ddlSide.Visible = true;
                }
                else
                {
                    ddlSide.SelectedValue = "0";
                    ddlSide.Text = string.Empty;
                }


                if (lblContextId.Text != "0" && lblContextId.Text != "")
                {
                    ddlContext.SelectedValue = lblContextId.Text;
                    ddlContext.Visible = true;

                }
                else
                {
                    ddlContext.SelectedValue = "0";
                    ddlContext.Text = string.Empty;
                }
                if (lblContextId.Text == "0")
                {

                    ddlContext.Visible = false;

                }
                if (lblSeverityId.Text != "0" && lblSeverityId.Text != "")
                {
                    ddlSeverity.SelectedValue = lblSeverityId.Text;
                    ddlSeverity.Visible = true;

                }
                else
                {
                    ddlSeverity.SelectedValue = "0";
                    ddlSeverity.Text = string.Empty;
                }
                if (lblSeverityId.Text == "0")
                {

                    ddlSeverity.Visible = false;

                }
                if (lblProvider.Text != "0")
                {
                    ddlProviders.SelectedValue = lblProvider.Text;

                }

                if (lblPrimary.Text.ToLower() == "true")
                {
                    chkPrimarys.Checked = true;
                }
                else
                {
                    chkPrimarys.Checked = false;
                }
                if (lblChronic.Text.ToLower() == "true")
                {
                    chkChronics.Checked = true;
                }
                else
                {
                    chkChronics.Checked = false;
                }
                BindPullFarwordAndRemarks();
                txtSCTId.Text = lblSCTId1.Text;
                btnAddtogrid.Text = "Update List";
                txtedit.Text = gvProblemDetails.SelectedRow.Cells[1].Text.Trim();


            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    #endregion

    #region // Working On Gridview 'gvChronicProblemDetails'

    private void BindBlankChronicProblemDetailGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ProblemID"] = "";
                dr["ProblemDescription"] = "";
                dr["LocationID"] = "0";
                dr["Location"] = "";
                dr["OnsetID"] = "";
                dr["Onset"] = "";
                dr["DurationID"] = "0";
                dr["Duration"] = "";
                dr["QualityID1"] = "0";
                dr["Quality"] = "";
                dr["ContextID"] = "0";
                dr["Context"] = "";
                dr["SeverityID"] = "0";
                dr["Severity"] = "";
                dr["Side"] = "0";
                dr["SideDescription"] = "";
                dr["ConditionID"] = "0";
                dr["Condition"] = "";

                dr["DoctorId"] = "0";

                dr["FacilityId"] = "0";
                dr["IsPrimary"] = "";
                dr["IsChronic"] = "";
                dr["SNOMEDCode"] = "";
                dr["AssociatedProblemID1"] = "0";//
                dr["AssociatedProblemID2"] = "0";//
                dr["AssociatedProblemID3"] = "0";//
                dr["AssociatedProblemID4"] = "0";//
                dr["AssociatedProblemID5"] = "0";//
                dr["AssociatedProblem1"] = "";//
                dr["AssociatedProblem2"] = "";//
                dr["AssociatedProblem3"] = "";//
                dr["AssociatedProblem4"] = "";//
                dr["AssociatedProblem5"] = "";//
                dr["Percentage"] = "";
                dr["Durations"] = "";
                dr["DurationType"] = "";
                dr["TemplateFieldId"] = "";

                dT.Rows.Add(dr);
            }

            gvChronicProblemDetails.DataSource = dT;
            gvChronicProblemDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    protected void gvChronicProblemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
                e.Row.Cells[19].Visible = false;
                e.Row.Cells[20].Visible = false;
                e.Row.Cells[21].Visible = false;
                e.Row.Cells[22].Visible = false;
                e.Row.Cells[25].Visible = false;
            }
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
            {
                Label lblProblem = (Label)e.Row.FindControl("lblProblem");
                LinkButton lb = (LinkButton)e.Row.Cells[23].Controls[0];
                ImageButton ibtn = (ImageButton)e.Row.FindControl("ibtnDelete");
                CheckBox chkSelectChronic = (CheckBox)e.Row.FindControl("chkSelectChronic");
                if (lblProblem.Text == "0" || lblProblem.Text == "")
                {
                    lb.Enabled = false;
                    ibtn.Enabled = false;
                    chkSelectChronic.Enabled = false;
                }
                if (lblProblem != null)
                {
                    lblProblem.ToolTip = lblProblem.Text;
                }

                HiddenField hdnEncodedById = (HiddenField)e.Row.FindControl("hdnEncodedById");
                LinkButton lnkEdit = (LinkButton)e.Row.Cells[23].Controls[0];
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");
                if (common.myInt(hdnEncodedById.Value) > 0)
                {
                    if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                    {
                        lnkEdit.Visible = false;
                        ibtnDelete.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    protected void gvChronicProblemDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!common.myBool(ViewState["IsAllowEdit"]))
            {
                Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                return;
            }

            btnNew.Visible = true;
            btnAddToFavourite.Visible = true;
            btnRemovefromFavorites.Visible = false;


            ViewState["BTN"] = "ALL";
            rblBTN.SelectedValue = "ALL";
            this.cmbProblemName.Text = "";
            this.cmbProblemName.SelectedValue = "";
            this.cmbProblemName.Enabled = false;
            rblBTN.Enabled = false;

            if (gvProblemDetails.SelectedIndex != -1)
                gvProblemDetails.SelectedIndex = -1;
            string strQualityIds = "";
            GridViewRow row = (GridViewRow)gvChronicProblemDetails.SelectedRow;
            Label lblProblemId = (Label)row.FindControl("lblProblemId");
            Label lblProblem = (Label)row.FindControl("lblProblem");
            Label lblLocation = (Label)row.FindControl("lblLocation");
            Label lblLocationId = (Label)row.FindControl("lblLocationId");
            Label lblOnset = (Label)row.FindControl("lblOnset");
            Label lblOnsetId = (Label)row.FindControl("lblOnsetId");
            Label lblDuration = (Label)row.FindControl("lblDuration");
            Label lblDurationId = (Label)row.FindControl("lblDurationId");
            Label lblQuality = (Label)row.FindControl("lblQuality");
            Label lblQualityId = (Label)row.FindControl("lblQualityId");
            Label lblContext = (Label)row.FindControl("lblContext");
            Label lblContextId = (Label)row.FindControl("lblContextId");
            Label lblSeverity = (Label)row.FindControl("lblSeverity");
            Label lblSeverityId = (Label)row.FindControl("lblSeverityId");
            Label lblProvider = (Label)row.FindControl("lblProvider");
            Label lblFacility = (Label)row.FindControl("lblFacility");
            Label lblPrimary = (Label)row.FindControl("lblPrimary");
            Label lblChronic = (Label)row.FindControl("lblChronic");
            Label lblSCTId1 = (Label)row.FindControl("lblSCTId");
            Label lblSide = (Label)row.FindControl("lblSide");
            Label lblSideDescription = (Label)row.FindControl("lblSideDescription");
            Label lblCondition = (Label)row.FindControl("lblCondition");
            Label lblConditionId = (Label)row.FindControl("lblConditionId");

            Label lblAssociatedProblemID1 = (Label)row.FindControl("lblAssociatedProblemID1");
            Label lblAssociatedProblem1 = (Label)row.FindControl("lblAssociatedProblem1");
            Label lblAssociatedProblemID2 = (Label)row.FindControl("lblAssociatedProblemID2");
            Label lblAssociatedProblem2 = (Label)row.FindControl("lblAssociatedProblem2");
            Label lblAssociatedProblemID3 = (Label)row.FindControl("lblAssociatedProblemID3");
            Label lblAssociatedProblem3 = (Label)row.FindControl("lblAssociatedProblem3");
            Label lblAssociatedProblemID4 = (Label)row.FindControl("lblAssociatedProblemID4");
            Label lblAssociatedProblem4 = (Label)row.FindControl("lblAssociatedProblem4");
            Label lblAssociatedProblemID5 = (Label)row.FindControl("lblAssociatedProblemID5");
            Label lblAssociatedProblem5 = (Label)row.FindControl("lblAssociatedProblem5");
            Label lblPercent = (Label)row.FindControl("lblPercentage");
            cmbAdd1.Text = lblAssociatedProblem1.Text;
            cmbAdd2.Text = lblAssociatedProblem2.Text;
            cmbAdd3.Text = lblAssociatedProblem3.Text;
            cmbAdd4.Text = lblAssociatedProblem4.Text;
            cmbAdd5.Text = lblAssociatedProblem5.Text;
            txtAdditionalProblemId1.Text = lblAssociatedProblemID1.Text;
            txtAdditionalProblemId2.Text = lblAssociatedProblemID2.Text;
            txtAdditionalProblemId3.Text = lblAssociatedProblemID3.Text;
            txtAdditionalProblemId4.Text = lblAssociatedProblemID4.Text;
            txtAdditionalProblemId5.Text = lblAssociatedProblemID5.Text;

            if (common.myInt(lblProblemId.Text) > 0)
                prblmID.Value = lblProblemId.Text;

            if (common.myInt(lblPercent.Text) > 0)
            {
                TxtPercentage.Text = lblPercent.Text;
            }
            else
            {
                TxtPercentage.Text = string.Empty;
            }

            if (common.myInt(lblAssociatedProblem2.Text.Trim()).Equals(0))
            {
                txtAdditionalProblemId2.Text = "0";
                tr2.Visible = false;
            }
            else
                tr2.Visible = true;

            if (common.myInt(lblAssociatedProblem3.Text.Trim()) > 0)
                tr3.Visible = true;
            else
            {
                txtAdditionalProblemId3.Text = "0";
                tr3.Visible = false;
            }
            if (Convert.ToString(lblAssociatedProblem4.Text.Trim()) != "" && Convert.ToString(lblAssociatedProblem4.Text.Trim()) != "0")
            {
                tr4.Visible = true;
            }
            else
            {
                lblAssociatedProblem4.Text = "0";
                tr4.Visible = false;
            }
            if (common.myInt(lblAssociatedProblem5.Text.Trim()) > 0)
                tr5.Visible = true;
            else
            {
                txtAdditionalProblemId5.Text = "0";
                tr5.Visible = false;
            }



            ViewState["RowIndexC"] = row.RowIndex;
            cmbProblemName.Text = lblProblem.Text;
            cmbProblemName.SelectedValue = lblProblemId.Text.Trim();
            if (lblLocationId.Text != "0" && lblLocationId.Text != "")
            {
                ddlLocation.SelectedValue = lblLocationId.Text;
            }
            else
            {
                ddlLocation.SelectedValue = "0";
                ddlLocation.Text = string.Empty;
            }
            if (lblOnsetId.Text != "0" && lblOnsetId.Text != "")
            {
                ddlOnset.SelectedValue = lblOnsetId.Text;
            }
            else
            {
                ddlOnset.SelectedValue = "0";
                ddlOnset.Text = string.Empty;
            }
            HiddenField hdnDurations = (HiddenField)row.FindControl("hdnCDurations");
            HiddenField hdnDurationType = (HiddenField)row.FindControl("hdnCDurationType");
            HiddenField hdnComplaintSearchId = (HiddenField)row.FindControl("hdnComplaintSearchId");

            ddlComplaintSearchCodes.SelectedIndex = ddlComplaintSearchCodes.Items.IndexOf(ddlComplaintSearchCodes.Items.FindItemByValue(common.myStr(hdnComplaintSearchId.Value)));

            txtDuration.Visible = false;
            //ddlDurationType.Visible = false;
            ddlDuration.Visible = false;
            if (hdnDurationType.Value != "O")
            {
                rdoDurationList.Visible = true;
                txtDuration.Visible = false;
                rdoDurationList.SelectedValue = hdnDurations.Value;
                ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindByValue(common.myStr(hdnDurationType.Value)));
            }
            else
            {
                rdoDurationList.Visible = false;
                txtDuration.Visible = true;
                txtDuration.Text = common.myStr(hdnDurations.Value);
                ddlDurationType.SelectedIndex = ddlDurationType.Items.IndexOf(ddlDurationType.Items.FindByValue(common.myStr(hdnDurationType.Value)));
            }

            if (lblQuality.Text != "0" && lblQuality.Text != "")
            {
                txtQualityIds.Text = lblQuality.Text;
                ddlQuality.Text = "";

                strQualityIds = lblQuality.Text;
                string strQuantityID = "";
                int i, j;
                string[] arInfo = new string[4];
                char[] splitter = { ',' };
                arInfo = strQualityIds.Split(splitter);
                for (i = 0; i < ddlQuality.Items.Count; i++)
                {
                    strQuantityID = ddlQuality.Items[i].Value.ToString().Trim();

                    for (j = 0; j < arInfo.Length; j++)
                    {
                        if (arInfo[j].Trim() == strQuantityID.Trim())
                        {
                            CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                            ddlQuality.Text += ddlQuality.Items[i].Text.ToString() + ",";
                            checkbox.Checked = true;

                        }
                    }
                }

                if (ddlQuality.Text.Length > 0)
                {
                    ddlQuality.Text = ddlQuality.Text.Substring(0, ddlQuality.Text.Length - 1);
                }

            }

            else
            {
                ddlQuality.SelectedValue = "0";
                ddlQuality.Text = string.Empty;
                for (int i = 0; i < ddlQuality.Items.Count; i++)
                {
                    CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
                    checkbox.Checked = false;
                }
                ddlQuality.Text = "";
            }
            if (lblContextId.Text != "0" && lblContextId.Text != "")
            {
                ddlContext.SelectedValue = lblContextId.Text;
            }
            else
            {
                ddlContext.SelectedValue = "0";
                ddlContext.Text = string.Empty;
            }

            if (lblSeverityId.Text != "0" && lblSeverityId.Text != "")
            {
                ddlSeverity.SelectedValue = lblSeverityId.Text;
            }
            else
            {
                ddlSeverity.SelectedValue = "0";
                ddlSeverity.Text = string.Empty;
            }
            if (lblSide.Text != "0" && lblSide.Text != "-")
            {
                ddlSide.SelectedValue = lblSide.Text;
            }
            else
            {
                ddlSide.SelectedValue = "0";
                ddlSide.Text = string.Empty;
            }

            if (lblConditionId.Text != "0" && lblConditionId.Text != "-1")
            {
                ddlCondition.SelectedValue = lblConditionId.Text;
            }
            else
            {
                ddlCondition.SelectedValue = "0";
                ddlCondition.Text = string.Empty;
            }
            if (lblProvider.Text != "0")
            {
                ddlProviders.SelectedValue = lblProvider.Text;
            }

            if (lblPrimary.Text.ToLower() == "true")
            {
                chkPrimarys.Checked = true;
            }
            else
            {
                chkPrimarys.Checked = false;
            }
            if (lblChronic.Text.ToLower() == "true")
            {
                chkChronics.Checked = true;
            }
            else
            {
                chkChronics.Checked = false;
            }

            BindPullFarwordAndRemarks();
            txtSCTId.Text = lblSCTId1.Text;

            btnAddtogrid.Text = "Update List";
            txtedit.Text = row.Cells[1].Text.Trim();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    protected void gvChronicProblemDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string strId = row.Cells[1].Text;
                ViewState["strIdCr"] = strId;

                if (rblShowNote.SelectedIndex == -1)
                {
                    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                    return;

                }
                if (!string.IsNullOrEmpty(strId))
                {
                    dvChronic.Visible = true;
                }

            }
            else if (e.CommandName == "Edt")
            {
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }

    #endregion

    #region  // will call on remove item from Favorites lists
    protected void btnRemovefromFavorites_Click(object sender, EventArgs e)
    {
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);
            if (common.myInt(cmbProblemName.SelectedValue) > 0)
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteEMRFavProblem";
                APIRootClass.DeleteEMRFavProblem objRoot = new global::APIRootClass.DeleteEMRFavProblem();
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.ProblemId = common.myInt(cmbProblemName.SelectedValue);
                objRoot.UserId = common.myInt(Session["UserID"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);
                //int i = objbc2.DeleteEMRFavProblem(common.myInt(Session["DoctorID"]), common.myInt(cmbProblemName.SelectedValue), common.myInt(Session["UserID"]));
                if (i == 0)
                    lblMessage.Text = "Problem removed from  favorite list";
            }
            else
            {
                Alert.ShowAjaxMsg("Select a Problem to Delete From Favorites", this);
            }
            cmbProblemName.Text = string.Empty;
            this.cmbProblemName.Items.Clear();

            BindFavouriteProblems("", "");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }
    #endregion


    #region // BindRequestProblemDetails
    private void BindRequestProblemDetails(Int32 iMainId, int blIsChronic)
    {
        DataSet objDs = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        DataTable DT = new DataTable();
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);

            //objDs = (DataSet)objbc2.GetChiefProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]), 0, "", 
            //    "", "", "%%", false, common.myBool(chkChronics.Checked), 0, "A");
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetChiefProblem";
            APIRootClass.GetChiefProblem objRoot = new global::APIRootClass.GetChiefProblem();
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DoctorId = 0;
            objRoot.Daterange = "";
            objRoot.FromDate = "";
            objRoot.ToDate = "";
            objRoot.SearchCriteriya = "%%";
            objRoot.IsDistinct = false;
            objRoot.IsChronic = common.myBool(chkChronics.Checked);
            objRoot.ProblemId = 0;
            objRoot.VisitType = "A";

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            dv = new DataView(objDs.Tables[0]);
            dv.RowFilter = "Id=" + iMainId;
            dt = dv.ToTable();
            lblMessage.Text = "";

            if (blIsChronic == 0)
            {
                DT = (DataTable)Cache["ProblemDetails"];
                if (DT == null)
                {
                    DT = CreateTable();
                }
            }
            else
            {
                DT = (DataTable)Cache["ChronicProblemDetails"];
                if (DT == null)
                {
                    DT = CreateTable();
                }
            }

            DataRow[] datarow = DT.Select("ProblemId=" + Convert.ToInt32(dt.Rows[0]["ProblemID"]));
            if (datarow.Length > 0)
            {
                datarow[0].BeginEdit();
                datarow[0]["ProblemID"] = Convert.ToInt32(dt.Rows[0]["ProblemID"]);
                datarow[0]["ProblemDescription"] = dt.Rows[0]["ProblemDescription"].ToString();
                if (common.myLen(dt.Rows[0]["DurationID"]) > 0)
                {
                    datarow[0]["DurationID"] = dt.Rows[0]["DurationID"].ToString().Trim();
                }
                datarow[0]["Duration"] = dt.Rows[0]["Duration"].ToString();

                if (common.myLen(dt.Rows[0]["ConditionID"]) > 0)
                {
                    datarow[0]["ConditionID"] = dt.Rows[0]["ConditionID"].ToString().Trim();
                }
                datarow[0]["Condition"] = dt.Rows[0]["Condition"].ToString();
                if (common.myLen(dt.Rows[0]["Side"]) > 0)
                {
                    datarow[0]["Side"] = dt.Rows[0]["Side"].ToString().Trim();
                }
                datarow[0]["SideDescription"] = dt.Rows[0]["SideDescription"].ToString();

                if (common.myLen(dt.Rows[0]["QualityID1"]) > 0)
                {
                    datarow[0]["QualityID"] = dt.Rows[0]["QualityID1"].ToString().Trim();
                }
                datarow[0]["Quality"] = dt.Rows[0]["Quality1"].ToString();
                if (common.myLen(dt.Rows[0]["ContextID"]) > 0)
                {
                    datarow[0]["ContextID"] = dt.Rows[0]["ContextID"].ToString().Trim();
                }


                datarow[0]["Context"] = dt.Rows[0]["Context"].ToString();

                if (common.myLen(dt.Rows[0]["SeverityID"]) > 0)
                {
                    datarow[0]["SeverityID"] = dt.Rows[0]["SeverityID"].ToString().Trim();
                }

                datarow[0]["Severity"] = dt.Rows[0]["Severity"].ToString();
                datarow[0]["DoctorId"] = dt.Rows[0]["DoctorId"].ToString();
                datarow[0]["FacilityId"] = dt.Rows[0]["FacilityId"].ToString();
                datarow[0]["IsPrimary"] = dt.Rows[0]["IsPrimary"].ToString();
                datarow[0]["IsChronic"] = iIsChronic == 0 ? "False" : "True";
                datarow[0]["SNOMEDCode"] = dt.Rows[0]["SNOMEDCode"].ToString();
                datarow[0].EndEdit();
            }
            else
            {
                DataRow dr;
                dr = DT.NewRow();
                dr["ID"] = "0";
                dr["ProblemID"] = Convert.ToInt32(dt.Rows[0]["ProblemID"]);
                dr["ProblemDescription"] = dt.Rows[0]["ProblemDescription"].ToString();
                if (!String.IsNullOrEmpty(dt.Rows[0]["DurationID"].ToString().Trim()))
                {
                    dr["DurationID"] = dt.Rows[0]["DurationID"].ToString().Trim();
                }
                dr["Duration"] = dt.Rows[0]["Duration"].ToString();
                if (!String.IsNullOrEmpty(dt.Rows[0]["QualityID1"].ToString().Trim()))
                {
                    dr["QualityID"] = dt.Rows[0]["QualityID1"].ToString().Trim();
                }

                dr["Quality"] = dt.Rows[0]["Quality1"].ToString();

                if (!String.IsNullOrEmpty(dt.Rows[0]["ContextID"].ToString().Trim()))
                {
                    dr["ContextID"] = dt.Rows[0]["ContextID"].ToString().Trim();
                }

                dr["Context"] = dt.Rows[0]["Context"].ToString();
                ///
                if (!String.IsNullOrEmpty(dt.Rows[0]["ConditionID"].ToString().Trim()))
                {
                    datarow[0]["ConditionID"] = dt.Rows[0]["ConditionID"].ToString().Trim();
                }
                datarow[0]["Condition"] = dt.Rows[0]["Condition"].ToString();
                if (!String.IsNullOrEmpty(dt.Rows[0]["Side"].ToString().Trim()))
                {
                    datarow[0]["Side"] = dt.Rows[0]["Side"].ToString().Trim();
                }
                datarow[0]["SideDescription"] = dt.Rows[0]["SideDescription"].ToString();

                if (!String.IsNullOrEmpty(dt.Rows[0]["SeverityID"].ToString().Trim()))
                {
                    dr["SeverityID"] = dt.Rows[0]["SeverityID"].ToString().Trim();
                }

                dr["Severity"] = dt.Rows[0]["Severity"].ToString();
                dr["DoctorId"] = dt.Rows[0]["DoctorId"].ToString();
                dr["FacilityId"] = dt.Rows[0]["FacilityId"].ToString();
                dr["IsPrimary"] = dt.Rows[0]["IsPrimary"].ToString();
                dr["IsChronic"] = iIsChronic == 0 ? "False" : "True";
                dr["SNOMEDCode"] = dt.Rows[0]["SNOMEDCode"].ToString();
                DT.Rows.Add(dr);
            }
            if (blIsChronic == 0)
            {
                Cache.Insert("ProblemDetails", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                gvProblemDetails.DataSource = DT;
                gvProblemDetails.DataBind();

            }
            else
            {
                Cache.Insert("ChronicProblemDetails", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                gvChronicProblemDetails.DataSource = DT;
                gvChronicProblemDetails.DataBind();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            objDs.Dispose();
            dv.Dispose();
            dt.Dispose();
            DT.Dispose();
        }
    }

    #endregion




    protected void btnSentence_click(object sender, EventArgs e)
    {
        txtSentenceGallery.Text = "";
    }
    protected void cmbAdd1_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = BindBlankGrid();
            if (e.Text.Trim().Length > 1)
                data = BindSearchProblemCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ProblemDescription"];
                item.Value = data.Rows[i]["ProblemId"].ToString();
                item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
                this.cmbAdd1.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected void cmbAdd2_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = BindBlankGrid();
            if (e.Text.Trim().Length > 1)
                data = BindSearchProblemCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ProblemDescription"];
                item.Value = data.Rows[i]["ProblemId"].ToString();
                item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
                this.cmbAdd2.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected void cmbAdd3_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = BindBlankGrid();
            if (e.Text.Trim().Length > 1)
                data = BindSearchProblemCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ProblemDescription"];
                item.Value = data.Rows[i]["ProblemId"].ToString();
                item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
                this.cmbAdd3.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected void cmbAdd4_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = BindBlankGrid();
            if (e.Text.Trim().Length > 1)
                data = BindSearchProblemCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ProblemDescription"];
                item.Value = data.Rows[i]["ProblemId"].ToString();
                item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
                this.cmbAdd4.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected void cmbAdd5_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            data = BindBlankGrid();
            if (e.Text.Trim().Length > 1)
                data = BindSearchProblemCombo(e.Text);

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ProblemDescription"];
                item.Value = data.Rows[i]["ProblemId"].ToString();
                item.Attributes["SNOMEDCode"] = data.Rows[i]["SNOMEDCode"].ToString();
                this.cmbAdd5.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected void btnAdd1_Click(object o, EventArgs e)
    {
        btnAdd1.Visible = false;
        tr2.Visible = true;
        btnRemove2.Visible = true;
    }
    protected void btnAdd2_Click(object o, EventArgs e)
    {
        btnAdd2.Visible = false;
        tr3.Visible = true;
        btnRemove2.Visible = false;
    }
    protected void btnRemove2_Click(object o, EventArgs e)
    {
        btnAdd1.Visible = true;
        tr2.Visible = false;
        cmbAdd2.Text = "";
        txtAdditionalProblemId2.Text = "0";
    }
    protected void btnAdd3_Click(object o, EventArgs e)
    {
        btnAdd3.Visible = false;
        tr4.Visible = true;
        btnRemove3.Visible = false;
    }
    protected void btnRemove3_Click(object o, EventArgs e)
    {
        btnAdd2.Visible = true;
        btnRemove2.Visible = true;
        tr3.Visible = false;
        cmbAdd3.Text = "";
        txtAdditionalProblemId3.Text = "0";
    }
    protected void btnAdd4_Click(object o, EventArgs e)
    {
        btnAdd4.Visible = false;
        tr5.Visible = true;
        btnRemove4.Visible = false;
    }
    protected void btnRemove4_Click(object o, EventArgs e)
    {
        btnAdd3.Visible = true;
        btnRemove3.Visible = true;
        tr4.Visible = false;
        cmbAdd4.Text = "";
        txtAdditionalProblemId4.Text = "0";
    }
    protected void btnRemove5_Click(object o, EventArgs e)
    {
        btnAdd4.Visible = true;
        btnRemove4.Visible = true;
        tr5.Visible = false;
        cmbAdd5.Text = "";
        txtAdditionalProblemId5.Text = "0";
    }
    protected void ibtnLocation_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Location";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnOnset_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Onset";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnDuration_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Duration";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnQuality_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Quality";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnContext_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Context";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnSeverity_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Severity";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnCondition_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/PriorityStatus.aspx?prbls=Condition";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClosepopup";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnCloseDetail_OnClick(object sender, EventArgs e)
    {
        divProblemDetails.Visible = false;
    }

    protected void lnkModeDetails_OnClick(object sender, EventArgs e)
    {
        divProblemDetails.Visible = true;
    }
    protected void btnSaveRemarks_OnClick(object sender, EventArgs e)
    {
        //objbc2 = new BaseC.EMRProblems(sConString);  
        try
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveHPIRemarks";
            APIRootClass.SaveHPIRemarks objRoot = new global::APIRootClass.SaveHPIRemarks();
            objRoot.EncounterId = common.myInt(Session["encounterid"]);
            objRoot.Remarks = common.myStr(txtSentenceGallery.Text);
            objRoot.IsPullForwardComplaint = chkPullForward.Checked;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            int i = JsonConvert.DeserializeObject<int>(sValue);

            //int i = objbc2.SaveHPIRemarks(common.myInt(Session["encounterid"]), common.myStr(txtSentenceGallery.Text), common.myBool(chkPullForward.Checked));
            if (i == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Remarks Updated.. ";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }
    protected void cmbProblemName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            switch (common.myStr(rblBTN.SelectedValue))
            {
                case "ALL":
                    if (e.Text.Trim().Length > 1)
                        data = PopulateAllProblem(e.Text);

                    break;

                case "FAV":
                    data = BindFavouriteProblems(e.Text, "");

                    break;
            }

            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbProblemName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(data.Rows[i]["ProblemDescription"]);
                item.ToolTip = common.myStr(data.Rows[i]["ProblemDescription"]);
                item.Value = common.myStr(data.Rows[i]["ProblemId"]);

                item.Attributes["SNOMEDCode"] = common.myStr(data.Rows[i]["SNOMEDCode"]);

                this.cmbProblemName.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    protected DataTable BindSearchProblemCombo(String etext)
    {
        DataSet ds = new DataSet();

        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hshin = new Hashtable();
        try
        {
            //if (etext.ToString().Trim() != "")
            //{
            //    hshin.Add("@chvSearchCriteria", "%" + etext + "%");
            //}
            //hshin.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            //ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsList", hshin);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProblemsList";
            APIRootClass.GetProblemsList objRoot = new global::APIRootClass.GetProblemsList();
            objRoot.SearchText = "%" + etext + "%";
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        return ds.Tables[0];
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    protected void rblBTN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["BTN"] = "ALL";
            if (common.myStr(rblBTN.SelectedValue) != "")
            {
                ViewState["BTN"] = common.myStr(rblBTN.SelectedValue);
            }

            switch (common.myStr(rblBTN.SelectedValue))
            {
                case "ALL":
                    gvChronicProblemDetails.SelectedIndex = -1;
                    gvProblemDetails.SelectedIndex = -1;
                    btnAddToFavourite.Visible = true;
                    btnRemovefromFavorites.Visible = false;
                    break;


                case "FAV":

                    gvChronicProblemDetails.SelectedIndex = -1;
                    gvProblemDetails.SelectedIndex = -1;
                    btnAddToFavourite.Visible = false;
                    btnRemovefromFavorites.Visible = false;
                    break;
            }

            this.cmbProblemName.Text = "";
            this.cmbProblemName.SelectedValue = "";
            this.cmbProblemName.Items.Clear();
            this.cmbProblemName.DataSource = null;
            this.cmbProblemName.DataBind();


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Problems/ViewPastPatientProblems.aspx?MP=NO";
        RadWindowForNew.Height = 620;
        RadWindowForNew.Width = 1100;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtedit.Text = "";
        prblmID.Value = "";
        btnAdd1.Visible = true;
        ddlQuality.Text = string.Empty;
        for (int i = 0; i < ddlQuality.Items.Count; i++)
        {
            CheckBox checkbox = (CheckBox)ddlQuality.Items[i].FindControl("chk1");
            checkbox.Checked = false;
        }
        ddlQuality.Text = "";


        ClearProblemDetailsControls();
        cmbProblemName.Text = string.Empty;
        this.cmbProblemName.Enabled = true;
    }
    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        //objbc2 = new BaseC.EMRProblems(sConString);
        //DataSet ds = new DataSet();

        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/Canceltodayproblem";
        APIRootClass.Canceltodayproblem objRoot = new global::APIRootClass.Canceltodayproblem();
        objRoot.ProblemId = common.myInt(ViewState["strId"]);
        objRoot.RegistrationID = common.myInt(Session["RegistrationID"]);
        objRoot.Encounterid = common.myInt(Session["encounterid"]);
        objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
        objRoot.FacilityId = common.myInt(Session["FacilityID"]);
        objRoot.Pageid = common.myInt(ViewState["PageId"]);
        objRoot.UserId = common.myInt(Session["UserID"]);
        objRoot.Shownote = common.myInt(rblShowNote.SelectedItem.Value);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);

        //objbc2.Canceltodayproblem(common.myInt(ViewState["strId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), common.myInt(Session["UserID"]), common.myInt(rblShowNote.SelectedItem.Value));

        RetrievePatientProblemsDetail();
        dvConfirmCancelOptions.Visible = false;
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }
    protected void ButtonOk1_OnClick(object sender, EventArgs e)
    {
        //objbc2 = new BaseC.EMRProblems(sConString);
        //DataSet ds = new DataSet();

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CancelChronicProblem";
        APIRootClass.Canceltodayproblem objRoot = new global::APIRootClass.Canceltodayproblem();
        objRoot.ProblemId = common.myInt(ViewState["strIdCr"]);
        objRoot.RegistrationID = common.myInt(Session["RegistrationID"]);
        objRoot.Encounterid = common.myInt(Session["encounterid"]);
        objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
        objRoot.FacilityId = common.myInt(Session["FacilityID"]);
        objRoot.Pageid = common.myInt(ViewState["PageId"]);
        objRoot.UserId = common.myInt(Session["UserID"]);
        objRoot.Shownote = common.myInt(rblShowNote.SelectedItem.Value);
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);

        //objbc2.Cancelchronicproblem(common.myInt(ViewState["strIdCr"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), 
        //    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), 
        //    common.myInt(Session["UserID"]), common.myInt(rblShowNote.SelectedItem.Value));

        RetrievePatientProblemsDetail();
        dvChronic.Visible = false;
    }
    protected void ButtonCancel1_OnClick(object sender, EventArgs e)
    {
        dvChronic.Visible = false;
    }
    protected void btnfind_click(object sender, EventArgs e)
    {
        BindODLQCS();
        //BindLocation();
        //BindOnset();
        //BindDuration();
        //BindQuality();
        //BindContext();
        //BindSeverity();
        //BindCondition();
    }
    protected void imgUTD_OnClick(object sender, EventArgs e)
    {
        if (cmbProblemName.Text.Trim().Length < 3)
        {
            Alert.ShowAjaxMsg("Please Type Problem then Continue..", this);
            cmbProblemName.Focus();
            return;
        }
        //RadWindowForNew.NavigateUrl = UtdLink + "/search?sp=0&source=USER_PREF&search=" + cmbProblemName.Text  + "&searchType=PLAIN_TEXT"; //"http://www.uptodate.com/contents/search?sp=0&source=USER_PREF&search=" + cmbProblemName.Text + "&searchType=PLAIN_TEXT";
        RadWindowForNew.NavigateUrl = UtdLink + "/contents/search?srcsys=HMGR374606&id=" + common.myStr(Session["EmployeeId"]) + "&search=" + cmbProblemName.Text;

        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
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
            HandleException(Ex);
        }

    }
    protected void BindFavouriteList(string tSearch)
    {
        DataTable dt = BindFavouriteProblems(tSearch, "List");
        gvFav.DataSource = dt;
        gvFav.DataBind();
        ViewState["FavList"] = dt;
    }
    protected void gvFav_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {


        try
        {
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                prblmID.Value = ((HiddenField)row.FindControl("hdnProblemId")).Value;
                //objbc2 = new BaseC.EMRProblems(sConString);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteEMRFavProblem";
                APIRootClass.DeleteEMRFavProblem objRoot = new global::APIRootClass.DeleteEMRFavProblem();
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.ProblemId = common.myInt(prblmID.Value);
                objRoot.UserId = common.myInt(Session["UserID"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);
                //int i = objbc2.DeleteEMRFavProblem(common.myInt(Session["DoctorID"]), common.myInt(prblmID.Value), common.myInt(Session["UserID"]));
                if (i == 0)
                {
                    lblMessage.Text = "Problem removed from  favorite list";
                    BindFavouriteList(common.myStr(txtSearchFav.Text));
                }

            }
            else if (e.CommandName == "FAVLIST")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                prblmID.Value = ((HiddenField)row.FindControl("hdnProblemId")).Value.ToString();
                txtSCTId.Text = ((HiddenField)row.FindControl("hdnSNOMEDCode")).Value.ToString();
                hdnIsUnSavedData.Value = "1";
                cmbProblemName.Text = common.clearHTMLTags(((LinkButton)row.FindControl("lnkFavName")).Text.ToString());
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {

        }
    }
    protected void btnSearchFav_OnClick(object sender, EventArgs e)
    {
        BindFavouriteList(common.myStr(txtSearchFav.Text));
    }
    protected void ddlDurationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDuration.Text = string.Empty;
        if (ddlDurationType.SelectedValue == "O")
        {
            txtDuration.Visible = true;
            rdoDurationList.Visible = false;
        }
        else
        {
            txtDuration.Visible = false;
            rdoDurationList.Visible = true;
        }
    }

    protected void ibtnComplaintSearchCode_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Status.aspx?Source=COMPLAINT";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientCloseSearch";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnGetComplaintSearchCodes_OnClick(object sender, EventArgs e)
    {
        bindSearchCode();
    }

    protected void bindSearchCode()
    {

        DataSet ds = new DataSet();
        //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProvisionalDiagnosisSearchCodes";
            APIRootClass.GetProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.GetProvisionalDiagnosisSearchCodes();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.KeywordType = "COMPLAINT";


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            //dt = JsonConvert.DeserializeObject<DataTable>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "COMPLAINT");
            ddlComplaintSearchCodes.Items.Clear();
            ddlComplaintSearchCodes.DataSource = ds;
            ddlComplaintSearchCodes.DataValueField = "Id";
            ddlComplaintSearchCodes.DataTextField = "DiagnosisSearchCode";
            ddlComplaintSearchCodes.DataBind();
            ddlComplaintSearchCodes.Items.Insert(0, new RadComboBoxItem("Select", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            HandleException(Ex);
        }
        finally
        {
            //objDiag = null;
            ds.Dispose();
        }
    }
}