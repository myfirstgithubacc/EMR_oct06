using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class LIS_Phlebotomy_PatientHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string folderpath = ConfigurationManager.AppSettings["folderpath"];
    clsExceptionLog objException = new clsExceptionLog();
    string Flag = string.Empty;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    private string LabResultPath = ConfigurationManager.AppSettings["LabResultPath"];

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]).ToUpper().Equals("STATICTEMPLATE"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (Request.QueryString["Master"] != null)
        {
            if (common.myStr(Request.QueryString["Master"]).ToUpper().Equals("BLANK"))
            {
                this.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        dtpFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        if (!IsPostBack)
        {
            BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
            DAL.DAL dlf = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                if (common.myStr(Cache["folderpath"]).Trim().Equals(string.Empty))
                {
                    string folderpath = common.myStr(dlf.ExecuteScalar(CommandType.Text, "selecT isnull(folderpath,'') From fileserversetup with(nolock) where facilityID=" + common.myStr(Session["FacilityId"]) + ""));
                    if (folderpath.Trim() != "")
                    {
                        Cache["folderpath"] = folderpath;
                    }
                    else
                    {
                        Cache["folderpath"] = Server.MapPath("/PatientDocuments/");
                    }
                    dlf = null;
                    emr = null;
                }
                dtpFromDate.SelectedDate = DateTime.Now.AddDays(-60);
                dtpToDate.SelectedDate = DateTime.Now;

                //if (common.myInt(Session["StationId"]) > 0 && common.myLen(Request.QueryString["SOURCE"]).Equals(0) && common.myLen(Request.QueryString["RegNo"]).Equals(0))
                //{
                //ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindItemByValue("YA"));//Pra 23 Mar 2022
                ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindItemByValue("GV"));//Pra 23 Mar 2022
                //}

                BindBlankGrid();
                BindFacility();
                BindStation();
                if (Request.QueryString["PageSource"] != null)
                {
                    if (Request.QueryString["PageSource"].Equals("Ward"))
                    {
                        chkCriticalValue.Checked = true;
                        txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                        txtRegNo.ReadOnly = true;

                        ddlFacility.Enabled = false;
                        ddlReportFor.Enabled = false;
                        dtpFromDate.Enabled = false;
                        dtpToDate.Enabled = false;
                        ddlSearchCriteria.Enabled = false;
                        txtRegNo.Enabled = false;
                        ddlServiceName.Enabled = false;
                        //ddlView.Enabled = false;
                        chkAbnormalValue.Enabled = false;
                        chkCriticalValue.Enabled = false;
                        btnSearch.Enabled = false;

                        bindLabService();
                        BindResultGrid();
                    }
                }
                else
                {
                    if ((Request.QueryString["FacilityId"] != null))
                    {
                        ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Request.QueryString["FacilityId"])));
                        ddlFacility.Enabled = false;
                    }
                    if ((Request.QueryString["RegNo"] != null))
                    {
                        ddlSearchCriteria.SelectedIndex = ddlSearchCriteria.Items.IndexOf(ddlSearchCriteria.Items.FindItemByValue("2"));
                        txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);

                        bindLabService();

                        txtRegNo.Enabled = false;
                        ddlSearchCriteria.Enabled = false;
                        btnclose.Visible = true;
                    }
                    if (Request.QueryString["CF"] != null)
                    {
                        if (common.myStr(Request.QueryString["CF"]).ToUpper().Contains("EHR"))
                        {
                            btnclose.Visible = false;
                            ddlSearchCriteria.SelectedIndex = ddlSearchCriteria.Items.IndexOf(ddlSearchCriteria.Items.FindItemByValue("2"));
                            ddlSearchCriteria.Enabled = false;
                            if (Session["RegistrationNo"] == null)
                            {
                                if (common.myInt(Session["RegistrationNo"]).Equals(0))
                                {
                                    Response.Redirect("/default.aspx?RegNo=0", false);
                                }
                            }
                            else
                            {
                                txtRegNo.Text = common.myStr(Session["RegistrationNo"]);
                                txtRegNo.Enabled = false;
                            }
                            bindLabService();
                        }
                    }
                    if (Request.QueryString["MD"] != null)
                    {
                        Flag = common.myStr(Request.QueryString["MD"]);
                    }
                    gvResultFinal.CurrentPageIndex = 0;

                    if (common.myStr(txtRegNo.Text.Trim()) != string.Empty)
                    {
                        BindResultGrid();
                    }
                }
                ViewState["IsPrintProvisional"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "WardAllowToPrintPRovisionalResult", sConString));
                ViewState["IsViewProvisional"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToViewProvisionalResult", sConString));  //Ritika(20-09-2022)View Provisional Report
                if (common.myStr(txtRegNo.Text) != "")
                {
                    ddlView_OnSelectedIndexChanged(this, null);
                }

                bindcontrolforsercivceAck(); //For Page Call from ServiceAcknowledge

            }
            catch (Exception Ex)
            {
                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //lblMessage.Text = "Error: " + Ex.Message;

                objException.HandleException(Ex);
            }
            finally
            {
                emr = null;
                dlf = null;
            }
        }
        //bindLabService();
    }

    public void bindcontrolforsercivceAck()
    {
        try
        {
            if (common.myStr(Request.QueryString["PageID"]).ToUpper().Equals("SACK"))
            {
                Label4.Visible = true;
                ddlFacility.Visible = true;
                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
                lblReportType.Visible = true;
                ddlReportFor.Visible = true;
                ddlReportFor.SelectedIndex = ddlReportFor.Items.IndexOf(ddlReportFor.Items.FindItemByValue(common.myStr(Session["StationId"])));
                ddlView.Visible = false;
                Label7.Visible = false;
                //chkAbnormalValue.Visible = false;
                //chkCriticalValue.Visible = false;
                lblServiceName.Visible = true;
                ddlServiceName.Visible = true;
                txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                dtpFromDate.SelectedDate = System.DateTime.Now.AddYears(-5);
                bindLabService();
                BindResultGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void txtRegNo_TextChanged(object sender, EventArgs e)
    {
        bindLabService();
        //bindLabFields();
        //BindResultGrid();
        ddlView_OnSelectedIndexChanged(this, null);
    }
    private void bindLabFields()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (common.myInt(ddlField.Items.Count) == 0)
            {
                ddlField.Items.Clear();

                ds = objval.GetDiagPatientLabField(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
                                dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                                (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2")) ? common.myStr(txtRegNo.Text) : "", "");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlField.DataSource = ds.Tables[0];
                        ddlField.DataTextField = "FieldName";
                        ddlField.DataValueField = "FieldId";
                        ddlField.DataBind();
                        foreach (RadComboBoxItem currentItem in ddlField.Items)
                        {
                            currentItem.Checked = true;
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
            objval = null;
            ds.Dispose();
        }
    }

    void BindFacility()
    {
        BaseC.User valUser = new BaseC.User(sConString);
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (Cache["FACILITY"] == null)
            {
                ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
                Cache["FACILITY"] = ds;
            }
            ddlFacility.SelectedIndex = -1;
            ddlFacility.DataSource = (DataSet)Cache["FACILITY"];
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            string FacilityId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultFacility", sConString);
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue("0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            valUser = null;
            objMaster = null;
            ds.Dispose();
        }
    }


    void BindBlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Source");
            dt.Columns.Add("OrderDate");
            dt.Columns.Add("LabNo");
            dt.Columns.Add("EncounterNo");
            //dt.Columns.Add("ResultDate");
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("Result");
            dt.Columns.Add("RegistrationId");
            dt.Columns.Add("AbnormalValue");
            dt.Columns.Add("CriticalValue");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("PatientName");
            dt.Columns.Add("AgeGender");
            dt.Columns.Add("StatusColor");
            dt.Columns.Add("DiagSampleID");
            dt.Columns.Add("StatusID");
            dt.Columns.Add("StationId");
            dt.Columns.Add("ServiceId");
            dt.Columns.Add("ResultRemarksId");
            dt.Columns.Add("StatusCode");
            dt.Columns.Add("Provider");

            gvResultFinal.DataSource = dt;
            gvResultFinal.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void BindStation()
    {
        BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        ViewState["Flag"] = common.myStr(Request.QueryString["Flag"]).ToUpper();
        try
        {
            ds = objMaster.GetStation(common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string sPageFlag = "'LIS','RIS'";

                    if (common.myStr(Request.QueryString["Flag"]) != string.Empty)
                    {
                        if (common.myStr(Request.QueryString["Flag"]).ToUpper().Equals("LIS"))
                        {
                            sPageFlag = "'LIS'";

                            ViewState["Flag"] = "LIS";
                        }
                        else
                        {
                            sPageFlag = "'RIS'";

                            ViewState["Flag"] = "RIS";
                        }

                    }

                    dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "Active=1 AND FlagName IN (" + sPageFlag + ")";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ddlReportFor.DataSource = dv.ToTable();
                        ddlReportFor.DataValueField = "StationId";
                        ddlReportFor.DataTextField = "StationName";
                        ddlReportFor.DataBind();
                        ddlReportFor.Items.Insert(0, new RadComboBoxItem("All", "ALL"));
                        ddlReportFor.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            objMaster = null;
            dv.Dispose();
            ds.Dispose();
        }
    }

    void bindLabService()
    {
        BaseC.clsLISPhlebotomy cLab = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (common.myInt(ddlServiceName.Items.Count) == 0)
            {
                ddlServiceName.Items.Clear();
                if (!string.IsNullOrEmpty(txtEncNo.Text) && common.myInt(ddlSearchCriteria.SelectedValue).Equals(2))
                {
                    int UHID;
                    int.TryParse(txtEncNo.Text, out UHID);
                    if ((UHID > 2147483647 || UHID.Equals(0)))
                    {
                        lblMessage.Text = "Invalid UHID No";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        return;
                    }
                }
                string cEncId = common.myStr(Request.QueryString["EncId"]);

                if (cEncId == string.Empty)
                {
                    if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("1"))
                    {
                        clsIVF objIvf = new clsIVF(sConString);
                        cEncId = objIvf.getEncounterId(txtEncNo.Text, common.myInt(Session["FacilityId"])).ToString();
                    }
                }
                ViewState["DataServiceView"] = string.Empty;
                string sLabType = string.Empty;

                if (common.myStr(Request.QueryString["Flag"]) != string.Empty)
                {
                    if (common.myStr(Request.QueryString["Flag"]).ToUpper().Equals("LIS"))
                    {
                        sLabType = "G";
                    }
                    else
                    {
                        sLabType = "X";
                    }
                }
                else if (common.myStr(Request.QueryString["MD"]) != string.Empty)
                {
                    if (common.myStr(Request.QueryString["MD"]).ToUpper().Equals("LIS"))
                    {
                        sLabType = "G";
                    }
                    else
                    {
                        sLabType = "X";
                    }
                }

                ds = cLab.getAllLabServices(common.myInt(ddlFacility.SelectedValue), common.myInt(cEncId), common.myStr(txtRegNo.Text), sLabType);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlServiceName.DataSource = ds.Tables[0];
                        ddlServiceName.DataTextField = "ServiceName";
                        ddlServiceName.DataValueField = "ServiceId";
                        ddlServiceName.DataBind();
                        ViewState["DataServiceView"] = ds;

                        foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
                        {
                            currentItem.Checked = true;
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
            cLab = null;
            ds.Dispose();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindLabService();
            //bindLabFields();
            //BindResultGrid();
            ddlView_OnSelectedIndexChanged(this, null);

        }
        catch (Exception Ex)
        {
        }
    }

    protected void ddlSearchCriteria_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = string.Empty;
        txtEncNo.Text = string.Empty;
        lblMessage.Text = string.Empty;
        txtRegNo.Visible = false;
        txtEncNo.Visible = false;

        if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2"))
        {
            txtRegNo.Visible = true;
        }
        else
        {
            txtEncNo.Visible = true;
        }
    }

    protected void gvResultFinal_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
            if (e.Item.ItemType == GridItemType.Header)
            {
                if (common.myStr(Flag).ToUpper().Equals("RIS"))
                    ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
                else
                    ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
            }
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                Label lblresult = (Label)e.Item.FindControl("lblresult");
                Label lblAbnormalValue = (Label)e.Item.FindControl("lblAbnormalValue");
                Label lblCriticalValue = (Label)e.Item.FindControl("lblCriticalValue");
                Label lblPatientNameGrid = (Label)e.Item.FindControl("lblPatientName");
                LinkButton lnkResult = (LinkButton)e.Item.FindControl("lnkResult");
                LinkButton lnkprint = (LinkButton)e.Item.FindControl("lnkprint");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");
                Label lblStationId = (Label)e.Item.FindControl("lblStationId");
                ImageButton img = (ImageButton)e.Item.FindControl("imgViewImage");
                HiddenField hdnFlag = (HiddenField)e.Item.FindControl("hdnFlag");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                HiddenField hdnAccessionNo = (HiddenField)e.Item.FindControl("hdnAccessionNo");
                HiddenField hdnReviewedStatus = (HiddenField)e.Item.FindControl("hdnReviewedStatus");
                HiddenField hdnReviewedComments = (HiddenField)e.Item.FindControl("hdnReviewedComments");

                if (common.myBool(hdnReviewedStatus.Value))
                {
                    e.Item.ToolTip = "Reviewed: Yes" + Environment.NewLine + "Comments: " + common.myStr(hdnReviewedComments.Value);
                }
                else
                {
                    e.Item.ToolTip = "Reviewed: No";
                }
                if (common.myInt(hdnAccessionNo.Value) > 0)
                {
                    img.Visible = true;
                }
                else
                {
                    img.Visible = false;
                }

                if (common.myStr(hdnFlag.Value) == "LIS")
                    lnkServiceName.Enabled = true;
                else
                    lnkServiceName.Enabled = false;

                if (common.myStr(lblresult.Text).Trim().ToUpper().Equals("RESULT"))
                {
                    lnkResult.CommandName = "Result";
                    lblresult.Visible = false;
                    lnkResult.Visible = true;
                }
                else if (common.myStr(lblresult.Text).Trim().ToUpper().Equals("DOWNLOAD"))
                {
                    lnkResult.CommandName = "Download";
                    lblresult.Visible = false;
                    lnkResult.Visible = true;
                    lnkprint.Visible = false;
                }
                //Ritika(15 - 09 - 2022)View Option for Provisional Report
                else if (common.myStr(ViewState["IsViewProvisional"]) == "Y")
                {
                    lnkResult.CommandName = "Result (Provisional)";
                    lnkResult.Visible = true;
                    lnkprint.Visible = false;
                    lnkResult.Enabled = true;
                }
                else
                {
                    lblresult.Visible = true;
                    lnkResult.Visible = false;
                }

                if (common.myStr(lblStatusCode.Text).ToUpper().Equals("RP") && common.myStr(ViewState["IsViewProvisional"]) == "N") //Ritika(15-09-2022)View Option for Provisional Report
                {
                    lnkResult.CommandName = "Result";
                    lblresult.Visible = true;
                    lnkResult.Visible = false;
                }

                if (common.myBool(lblAbnormalValue.Text) && !common.myBool(lblCriticalValue.Text))
                {
                    lblresult.ForeColor = System.Drawing.Color.DarkViolet;
                }
                if (common.myBool(lblCriticalValue.Text))
                {
                    lblresult.ForeColor = System.Drawing.Color.Red;
                }
                if (common.myStr(lblStatusCode.Text) == "RP" && common.myStr(ViewState["IsPrintProvisional"]).Equals("N"))
                {
                    lnkprint.Visible = false;

                }
                else
                {
                    lnkprint.Visible = true;
                }
                if (txtRegNo.Text.Length > 0)
                {
                    lblPatientName.Text = "Patient Name : " + lblPatientNameGrid.Text;
                }
                else
                {
                    lblPatientName.Text = string.Empty;
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

    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        DataTable dtgraph = new DataTable();
        try
        {
            lblMessage.Text = string.Empty;
            if (common.myStr(e.CommandName).ToUpper().Equals("RESULT") || common.myStr(ViewState["IsViewProvisional"]) == "Y")  //Ritika(15-09-2022)
            {
                string Source = string.Empty;
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");

                if (common.myStr(lblSource.Text).Trim().ToUpper().Equals("ER"))
                {
                    Source = "OPD";
                }
                else
                {
                    Source = lblSource.Text;
                }
                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + Source
                                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                                            + "&SERVICEID=" + common.myInt(lblServiceId.Text)
                                            + "&AgeInDays=" + common.myStr(lblAgeGender.Text)
                                            + "&StatusCode=" + common.myStr(lblStatusCode.Text)
                                            + "&ServiceName=" + common.myStr(lnkServiceName.Text)
                                            + "&LabNo=" + common.myStr(lblLabNo.Text);

                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 850;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("DOWNLOAD"))
            {
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                ds = new DataSet();
                if (common.myStr(lblSource.Text).ToUpper().Equals("IPD"))
                {
                    ds = objval.FillAttachmentDownloadDropdownIP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, string.Empty);
                }
                else
                {
                    ds = objval.FillAttachmentDownloadDropdownOP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, string.Empty);
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            string Splitter = ConfigurationManager.AppSettings["Split"];
                            if (common.myLen(Splitter).Equals(0))
                            {
                                Splitter = "!";
                            }

                            var csplitter = Splitter.ToCharArray();

                            string sFileName = common.myStr(ds.Tables[0].Rows[0]["DocumentName"]);
                            string sSavePath = common.myStr(folderpath) + "LabResult/";
                            string path = sSavePath + sFileName;
                            //Create FTP Request.
                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftppath.Split(csplitter)[0].ToString() + Rootfolder + ConfigurationManager.AppSettings["LabResultPath"] + "/" + sFileName);
                            request.Method = WebRequestMethods.Ftp.DownloadFile;

                            //Enter FTP Server credentials.
                            request.Credentials = new NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
                            request.UsePassive = true;
                            request.UseBinary = true;
                            request.EnableSsl = false;

                            //Fetch the Response and read it into a MemoryStream object.
                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                            using (MemoryStream stream = new MemoryStream())
                            {
                                //Download the File.
                                response.GetResponseStream().CopyTo(stream);
                                Response.AddHeader("content-disposition", "attachment;filename=" + sFileName);
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.BinaryWrite(stream.ToArray());

                                Response.End();
                            }
                        }
                        else
                        {
                            string strSource = lblSource.Text;
                            RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/Download.aspx?SampleId="
                              + ((Label)e.Item.FindControl("lblDiagSampleID")).Text + "&SOURCE=" + strSource;

                            RadWindowPopup.Height = 300;
                            RadWindowPopup.Width = 600;
                            RadWindowPopup.Top = 10;
                            RadWindowPopup.Left = 10;
                            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindowPopup.Modal = true;
                            RadWindowPopup.VisibleStatusbar = false;
                        }
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "File does not exist...";
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "File does not exist...";
                }
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("PRINT"))
            {
                HiddenField hdnResultHTML = (HiddenField)e.Item.FindControl("hdnResultHTML");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblStationId = (Label)e.Item.FindControl("lblStationId");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblRegistrationId = (Label)e.Item.FindControl("lblRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");
                Label LblFormatID = (Label)e.Item.FindControl("LblFormatID");
                Label LblWinReportType = (Label)e.Item.FindControl("LblWinReportType");
                Label LblSubDeptCode = (Label)e.Item.FindControl("LblSubDeptCode");
                Label LblDepartmentCode = (Label)e.Item.FindControl("LblDepartmentCode");
                Label LblIsArchive = (Label)e.Item.FindControl("LblIsArchive");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");

                if (common.myStr(lblStatusCode.Text).Trim().ToUpper().Equals("RE"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Result is not certified. Print not allowed!";
                    Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
                    return;
                }

                if (!common.myStr(hdnResultHTML.Value).ToUpper().Equals("RESULTHTML"))
                {
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + common.myStr(lblSource.Text) +
                                                "&LABNO=" + common.myInt(lblLabNo.Text) +
                                                "&StationId=" + common.myInt(lblStationId.Text) +
                                                "&ServiceIds=" + common.myStr(lblServiceId.Text) +
                                                "&DiagSampleId=" + common.myInt(lblDiagSampleID.Text) +
                                                "&FormatID=" + common.myInt(LblFormatID.Text) +
                                                "&ReportType=" + common.myStr(LblWinReportType.Text) +
                                                "&SubDeptCode=" + common.myStr(LblSubDeptCode.Text) +
                                                "&DepartmentCode=" + common.myStr(LblDepartmentCode.Text) +
                                                "&IsArchive=" + common.myStr(LblIsArchive.Text);
                }
                else
                {
                    ArrayList coll = new ArrayList();
                    StringBuilder objXML = new StringBuilder();
                    coll.Add(common.myStr(lblDiagSampleID.Text));
                    coll.Add(common.myStr(lblSource.Text));
                    objXML.Append(common.setXmlTable(ref coll));

                    RadWindowPopup.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + common.myStr(lblSource.Text) +
                                        "&LABNO=" + common.myInt(lblLabNo.Text) + "&ServiceIds=" + common.myStr(lblServiceId.Text) +
                                        "&StationId=" + common.myInt(lblStationId.Text) + "&Flag=" + Flag + "&RegId=" + common.myInt(lblRegistrationId.Text) +
                                        "&EncId=" + common.myInt(hdnEncounterId.Value) + "&DiagSampleId=" + objXML.ToString();
                }
                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;
                RadWindowPopup.Left = 10;
                RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("INVESTIGATION"))
            {
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");

                string DIAG_SAMPLEID_S = string.Empty;
                //foreach (GridDataItem dataItem in gvResultFinal.Items)
                //{
                //    Label lblServiceId_G = (Label)dataItem.FindControl("lblServiceId");
                //    if (common.myInt(lblServiceId_G.Text) == common.myInt(lblServiceId.Text))
                //    {
                //        Label lblDiagSampleID_G = (Label)dataItem.FindControl("lblDiagSampleID");
                //        if (DIAG_SAMPLEID_S != string.Empty)
                //        {
                //            DIAG_SAMPLEID_S += "," + common.myInt(lblDiagSampleID_G.Text).ToString();
                //        }
                //        else
                //        {
                //            DIAG_SAMPLEID_S = common.myInt(lblDiagSampleID_G.Text).ToString();
                //        }
                //    }
                //}
                dtgraph = new DataTable();
                dtgraph = (DataTable)ViewState["GraphData"];

                foreach (DataRow dr in dtgraph.Rows)
                {
                    int lblServiceId_G = common.myInt(dr["ServiceId"]);
                    if (common.myInt(lblServiceId_G).Equals(common.myInt(lblServiceId.Text)))
                    {
                        int lblDiagSampleID_G = (common.myInt(dr["DiagSampleID"]));

                        if (DIAG_SAMPLEID_S != string.Empty)
                        {
                            DIAG_SAMPLEID_S += "," + common.myInt(lblDiagSampleID_G).ToString();
                        }
                        else
                        {
                            DIAG_SAMPLEID_S = common.myInt(lblDiagSampleID_G).ToString();
                        }
                    }
                }

                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/LabResultGraph.aspx?SOURCE=" + lblSource.Text
                                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                                            + "&DIAG_SAMPLEID_S=" + common.myStr(DIAG_SAMPLEID_S)
                                            + "&SERVICEID=" + common.myInt(lblServiceId.Text)
                                            + "&AgeInDays=" + common.myStr(lblAgeGender.Text)
                                            + "&StatusCode=" + common.myStr(lblStatusCode.Text)
                                            + "&ServiceName=" + common.myStr(lnkServiceName.Text)
                                            + "&RegNo=" + common.myStr(txtRegNo.Text)
                                            + "&PName=" + common.myStr(Request.QueryString["PName"]);


                RadWindowPopup.Height = 600;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
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
            objval = null;
            ds.Dispose();
            dtgraph.Dispose();
        }
    }

    protected void gvResultFinal_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvResultFinal.CurrentPageIndex = e.NewPageIndex;
        BindResultGrid();
    }

    protected void btnCustomView_OnClick(object sender, EventArgs e)
    {
        if (txtRegNo.Text.Trim().Length == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter the " + common.myStr(ddlSearchCriteria.SelectedItem.Text) + "#";
            return;
        }
        else
        {
            int EncId = common.myInt(Request.QueryString["EncId"]);

            if (EncId == 0)
            {
                EncId = common.myInt(Session["EncounterId"]);
            }

            RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PatientLabHistoryDynamic.aspx?Master=Blank&EncId=" + EncId + "&RegNo=" + common.myStr(txtRegNo.Text.Trim());
            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 1040;
            RadWindowPopup.Top = 20;
            RadWindowPopup.Left = 20;

            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.Modal = true;
            //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowPopup.VisibleStatusbar = false;
        }

    }

    protected void imgViewImage_Click(object sender, EventArgs e)
    {
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            string viewer = dl.ExecuteScalar(CommandType.Text, "select viewerurl from DiagHISPACSIntegrationSetup with(nolock) where FacilityId= " + common.myInt(Session["FacilityId"])).ToString();
            string key = dl.ExecuteScalar(CommandType.Text, "select SharedKey from DiagHISPACSIntegrationSetup with(nolock) where FacilityId= " + common.myInt(Session["FacilityId"])).ToString();

            ImageButton img = (ImageButton)sender;
            string accessionno = img.CommandName.ToString().Split('|')[0];
            string PatientNo = img.CommandName.ToString().Split('|')[1];
            string flagforEncryptedURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToEncryptPACSImageViewerURL", sConString));
            string flagforImageViewerInNewTab = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "flagforImageViewerInNewTab", sConString));
            if (flagforEncryptedURL == "Y")
            {
                viewer = viewer.Replace("@accessionNo", en.Encrypt(accessionno, key, true, string.Empty));
                viewer = viewer.Replace("@userid", en.Encrypt(Session["UserID"].ToString(), key, true, string.Empty));
                viewer = viewer.Replace("@mrn", en.Encrypt(PatientNo, key, true, string.Empty));
                viewer = viewer.Replace("@datetime", en.Encrypt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), key, true, string.Empty));
            }
            else
            {
                viewer = viewer.Replace("@accessionNo", accessionno);
                viewer = viewer.Replace("@mrn", PatientNo);
                viewer = viewer.Replace("@userid", Session["UserID"].ToString());
                viewer = viewer.Replace("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            }
            if (flagforImageViewerInNewTab == "Y")
            {
                string s = "window.open('" + viewer + "', 'popup_window', 'width=900,height=800,left=25,top=10,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            else
            {
                RadWindowPopup.NavigateUrl = viewer.Replace("+", "%2B");
                //RadWindowPopup.NavigateUrl = HttpUtility.UrlDecode(viewer.Replace("+", "%2b"));
                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;

                RadWindowPopup.Left = 10;
                RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
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
            en = null;
            dl = null;
        }
    }

    protected void gvLabDetailsXaxis_OnRowDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem || e.Item is GridDataItem)
            {
                e.Item.Cells[0].Visible = true;
                //e.Item.Cells[0].Width = Unit.Pixel(100);
            }
            if (e.Item is GridDataItem)
            {
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    e.Item.Cells[i].Wrap = false;

                    string S = common.myStr(e.Item.Cells[i].Text).Replace("&nbsp;", string.Empty);

                    if (i != 0 && S != string.Empty)
                    {
                        LinkButton lnk = new LinkButton();
                        string sDiagSampleId = string.Empty, sServiceId = string.Empty, sSource = string.Empty, sStatusCode = string.Empty, sServiceName = string.Empty, sAgeGender = string.Empty, sType = string.Empty, sFieldId = string.Empty;
                        if (S.ToString().Contains("Result"))
                        {
                            sType = "Result";
                            lnk.ToolTip = "Click to show results";
                        }
                        else if (S.ToString().Contains("Result") == false)
                        {
                            sType = "Value";
                            lnk.ToolTip = "Click to show Graph";
                        }
                        int iStart = 0;
                        int iEnd = 0;

                        iEnd = (int)S.IndexOf("#");
                        if (iEnd > -1)
                        {
                            lnk.Text = S.ToString().Substring(iStart, iEnd);
                        }
                        else
                        {
                            lnk.Text = S.ToString();
                        }

                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf("#") + 1;
                        iEnd = (int)S.IndexOf("@");
                        if ((iEnd - iStart) > -1)
                        {
                            sDiagSampleId = S.ToString().Substring(iStart, iEnd - iStart);
                        }
                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf("@") + 1;
                        iEnd = (int)S.IndexOf(",SId");
                        if ((iEnd - iStart) > -1)
                        {
                            sSource = S.ToString().Substring(iStart, iEnd - iStart);
                        }
                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf(",SId=") + 5;
                        iEnd = (int)S.IndexOf(",SNM=");
                        if ((iEnd - iStart) > -1)
                        {
                            sServiceId = S.ToString().Substring(iStart, iEnd - iStart);
                        }
                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf(",SNM=") + 5;
                        iEnd = (int)S.IndexOf(",SCD=");
                        if ((iEnd - iStart) > -1)
                        {
                            sServiceName = S.ToString().Substring(iStart, iEnd - iStart);
                        }
                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf(",SCD=") + 5;
                        iEnd = (int)S.IndexOf(",AB=");
                        if ((iEnd - iStart) > -1)
                        {
                            sStatusCode = S.ToString().Substring(iStart, iEnd - iStart);
                        }

                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf(",FID=") + 5;
                        iEnd = (int)S.IndexOf(",DOB=");
                        if ((iEnd - iStart) > -1)
                        {
                            sFieldId = S.ToString().Substring(iStart, iEnd - iStart);
                        }

                        iStart = 0;
                        iEnd = 0;
                        iStart = (int)S.IndexOf(",DOB=") + 5;
                        iEnd = (int)S.Length;
                        if ((iEnd - iStart) > -1)
                        {
                            sAgeGender = S.ToString().Substring(iStart, iEnd - iStart);
                        }

                        if (S.ToString().Contains("AB=1"))
                        {
                            lnk.ForeColor = System.Drawing.Color.Magenta;
                        }
                        else if (S.ToString().Contains("CR=1"))
                        {
                            lnk.ForeColor = System.Drawing.Color.Yellow;
                        }
                        else if (S.ToString().Contains("AB=1,CR=1"))
                        {
                            lnk.ForeColor = System.Drawing.Color.Red;
                        }
                        //lnk.ID = "LnkResult" + i.ToString();
                        //e.Item.Cells[i].Width = Unit.Pixel(100);
                        if ((int)S.IndexOf("#") > -1 || common.myStr(S) == "RESULT")
                        {
                            if (common.myStr(S) == "Result")
                            {
                                sType = "Result";
                            }
                            lnk.Enabled = true;
                            lnk.Attributes.Add("OnClick", "showResultPopup('" + sDiagSampleId + "','" + sServiceId + "','" + sSource + "','" + sStatusCode + "','" + sServiceName + "','" + sAgeGender + "','" + sFieldId + "','" + sType + "','" + i + "' );");
                        }
                        else
                        {
                            //e.Item.Cells[i].Attributes.Add("HeaderStyle-Width", "600");//..Width = Unit.Pixel(300);
                            lnk.Enabled = false;
                            lnk.OnClientClick = null;
                            lnk.Attributes.Clear();
                        }
                        e.Item.Cells[i].Text = string.Empty;
                        e.Item.Cells[i].ForeColor = System.Drawing.Color.Transparent;
                        e.Item.Cells[i].Controls.Add(lnk);
                    }
                    else
                    {
                        Label lbl = new Label();

                        lbl.Text = S.ToString();
                        e.Item.Cells[i].Controls.Add(lbl);
                    }

                    // e.Row.Cells[i].Text = lnk.Text.ToString();
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

    protected void btnPrintPopup_OnClick(object sender, EventArgs e)
    {
        string[] sLabNoEncIdRegId = common.myStr(hdnsLabNoEncIdRegId.Value).Split(',');
        BaseC.clsLISPhlebotomy oclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = oclsLISPhlebotomy.GetDiagServiceDetails(common.myInt(hdnsServiceId.Value), common.myInt(Session["FacilityID"]));
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0 && common.myBool(ds.Tables[0].Rows[0]["ResultHTML"]))
            {
                RadWindowPopup.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + hdnsSource.Value +
                                                "&LABNO=" + common.myStr(sLabNoEncIdRegId[0]) + "&ServiceIds=" + common.myStr(hdnsServiceId.Value) +
                                                "&StationId=" + common.myInt(ViewState["STATION_ID"]) + "&Flag=" + common.myStr(ViewState["MDFlag"]) +
                                                "&RegId=" + common.myStr(sLabNoEncIdRegId[2]) + "&EncId=" + common.myStr(sLabNoEncIdRegId[1]) +
                                                "&DiagSampleId=" + common.myInt(hdnsDiagSampleId.Value) +
                                                "&ShowReference=" + common.myBool(ds.Tables[0].Rows[0]["PrintReferenceRangeInHTML"]);
            }
            else
            {
                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + hdnsSource.Value +
                                                "&LABNO=" + common.myStr(sLabNoEncIdRegId[0]) +
                                                "&StationId=" + common.myInt(ViewState["STATION_ID"]) +
                                                "&Flag=" + common.myStr(ViewState["MDFlag"]) +
                                                "&ServiceIds=" + common.myStr(hdnsServiceId.Value) +
                                                "&DiagSampleId=" + common.myInt(hdnsDiagSampleId.Value);
            }
            RadWindowPopup.Height = 550;
            RadWindowPopup.Width = 800;
            RadWindowPopup.Top = 45;
            RadWindowPopup.Left = 10;
            RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.OnClientClose = "SearchOnClientClose";
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            oclsLISPhlebotomy = null;
            ds.Dispose();
        }
    }
    protected void btnCallcombo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ViewState["DataServiceView"]) == string.Empty)
            {
                return;
            }
            if (((DataSet)ViewState["DataServiceView"]).Tables.Count > 0)
            {
                if (((DataSet)ViewState["DataServiceView"]).Tables[0].Rows.Count > 0)
                {
                    gvLabDetailsXaxis.DataSource = null;
                    gvLabDetailsXaxis.DataBind();

                    string strItem = string.Empty;

                    foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
                    {
                        if (currentItem.Checked)
                        {
                            if (strItem != string.Empty)
                                strItem = strItem + "," + common.myStr(currentItem.Value);
                            else
                                strItem = common.myStr(currentItem.Value);
                        }
                    }

                    //DataView dvField = ((DataSet)ViewState["DataServiceView"]).Tables[0].DefaultView;
                    //if (strItem != string.Empty)
                    //{
                    //    dvField.RowFilter = "ServiceId In (" + strItem + ") And FieldType in ('N','F') AND RowNos > 1";
                    //    foreach (DataRowView drReferal in ((DataTable)dvField.ToTable(true, "FieldId", "FieldName", "ServiceId")).DefaultView)
                    //    {
                    //        RadComboBoxItem item = new RadComboBoxItem();
                    //        item.Text = (string)drReferal["FieldName"];
                    //        item.Value = drReferal["FieldId"].ToString();
                    //        item.Attributes.Add("ServiceID", common.myStr(drReferal["ServiceID"]));
                    //        ddlFieldName.Items.Add(item);
                    //        item.DataBind();
                    //    }
                    //}
                }
                gvLabDetailsXaxis.DataSource = null;
                gvLabDetailsXaxis.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCallPopup_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(hdnsType.Value) != string.Empty)
            {
                if (common.myStr(hdnsType.Value).ToUpper().Equals("RESULT"))
                {
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + hdnsSource.Value
                                                             + "&DIAG_SAMPLEID=" + common.myInt(hdnsDiagSampleId.Value)
                                                             + "&SERVICEID=" + common.myInt(hdnsServiceId.Value)
                                                             + "&AgeInDays=" + common.myStr(hdnsAgeGender.Value)
                                                             + "&StatusCode=" + common.myStr(hdnsStatusCode.Value)
                                                             + "&ServiceName=" + common.myStr(hdnsServiceName.Value);
                }
                else
                {
                    string sDiagSampleIds = string.Empty;

                    for (int i = 0; i <= gvLabDetailsXaxis.Items.Count - 1; i++)
                    {
                        string S = common.myStr(gvLabDetailsXaxis.Items[i].Cells[common.myInt(hdnsCellId.Value)].Text).Replace("&nbsp;", string.Empty);
                        if (S.ToString() != string.Empty)
                        {
                            int iStart = 0;
                            int iEnd = 0;
                            String sDiagSampleId = string.Empty;
                            iStart = (int)S.IndexOf("#") + 1;
                            iEnd = (int)S.IndexOf("@");
                            if ((iEnd - iStart) > -1)
                            {
                                sDiagSampleId = S.ToString().Substring(iStart, iEnd - iStart);
                            }

                            if (sDiagSampleIds.ToString() == string.Empty && sDiagSampleId != string.Empty)
                            {
                                sDiagSampleIds = sDiagSampleId;
                            }
                            else if (sDiagSampleId != string.Empty)
                            {
                                sDiagSampleIds = sDiagSampleIds + "," + sDiagSampleId;
                            }
                        }
                    }
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/LabResultGraph.aspx?SOURCE=" + hdnsSource.Value
                                                           + "&DIAG_SAMPLEID=" + common.myInt(hdnsDiagSampleId.Value)
                                                           + "&DIAG_SAMPLEID_S=" + common.myStr(sDiagSampleIds)
                                                           + "&SERVICEID=" + common.myInt(hdnsServiceId.Value)
                                                           + "&AgeInDays=" + common.myStr(hdnsAgeGender.Value)
                                                           + "&StatusCode=" + common.myStr(hdnsStatusCode.Value)
                                                           + "&ServiceName=" + common.myStr(hdnsServiceName.Value)
                                                           + "&FieldId=" + common.myStr(hdnsFieldId.Value)
                                           + "&RegNo=" + common.myStr(txtRegNo.Text)
                                           + "&PName=" + common.myStr(Request.QueryString["PName"]);
                }
                RadWindowPopup.Height = 620;
                RadWindowPopup.Width = 1100;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.OnClientClose = "SearchOnClientClose";
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnFilterView_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DataHistory"] != null && common.myStr(ViewState["DataHistory"]) != "")
            {
                if (((DataTable)ViewState["DataHistory"]).Rows.Count > 0)
                {
                    gvLabDetailsXaxis.DataSource = ((DataTable)ViewState["DataHistory"]);
                    gvLabDetailsXaxis.DataBind();
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

    protected void ddlView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvResultFinal.Visible = false;
            gvResultFinal.Enabled = false;
            gvLabDetailsXaxis.Visible = false;
            gvLabDetailsXaxis.Enabled = false;
            switch (ddlView.SelectedValue)
            {
                case "GV":
                    gvResultFinal.Visible = true;
                    gvResultFinal.Enabled = true;
                    break;

                case "XA":
                    gvLabDetailsXaxis.Visible = true;
                    gvLabDetailsXaxis.Enabled = true;
                    break;

                case "YA":
                    gvLabDetailsXaxis.Visible = true;
                    gvLabDetailsXaxis.Enabled = true;
                    break;
            }
            string sFlag = string.Empty, sMD = string.Empty;
            sFlag = common.myStr(Request.QueryString["Flag"]);
            sMD = common.myStr(Request.QueryString["MD"]);
            if (ddlView.SelectedValue == "YA" && (sFlag == "LIS" || sMD.Contains("LIS")))
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
            BindResultGrid();
        }
        catch
        {
        }
    }

    protected DataTable GetLabResultData()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            int iProviderID = 0;
            int pageindex = 0;
            string EncounterNo = string.Empty, RegNo = string.Empty;
            if (gvResultFinal.Items.Count > 0)
            {
                pageindex = gvResultFinal.CurrentPageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }
            if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2"))
            {
                RegNo = txtRegNo.Text.Trim();
                if (Request.QueryString["PageSource"] != null)
                {
                    if (Request.QueryString["PageSource"].Equals("Ward"))
                    {
                        int EncId = common.myInt(Request.QueryString["EncId"]);

                        if (EncId > 0)
                        {
                            BaseC.clsEMR objE = new BaseC.clsEMR(sConString);

                            DataSet dse = objE.getEncounterList(EncId, 0, common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

                            if (dse.Tables[0].Rows.Count > 0)
                            {
                                EncounterNo = common.myStr(dse.Tables[0].Rows[0]["EncounterNo"]);
                            }

                            dse.Dispose();
                            objE = null;
                        }

                        if (common.myLen(EncounterNo).Equals(0))
                        {
                            EncounterNo = common.myStr(Session["EncounterNo"]);
                        }

                    }
                }
            }
            else
            {
                EncounterNo = txtEncNo.Text.Trim();
            }

            int StationID = 0;
            if (common.myStr(Request.QueryString["PageID"]).ToUpper().Equals("SACK"))
            {
                StationID = common.myInt(Session["StationId"]);
            }
            else
            {
                StationID = common.myInt(ddlReportFor.SelectedValue);
            }

            ds = objval.getPatientLabResultHistory(common.myInt(Session["FacilityID"]), common.myInt(Session["HospitalLocationID"]),
                                    common.myDate(dtpFromDate.SelectedDate), common.myDate(dtpToDate.SelectedDate),
                                    common.myStr(RegNo), iProviderID, 0, pageindex, chkAbnormalValue.Checked, chkCriticalValue.Checked,
                                    0, common.myInt(ddlFacility.SelectedValue), EncounterNo, "B", 0, common.myInt(ddlServiceName.SelectedValue),
                                    StationID, "RE", string.Empty, common.myInt(Session["EmployeeId"]), 1, false); // Search flag should be 0

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
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
            objval = null;
        }

        return dt;
    }

    protected void BindResultGrid()
    {
        //if (common.myStr(Request.QueryString["Flag"]).ToUpper().Equals("RIS"))
        //{
        //    try
        //    {
        //        ddlView.Items.Remove(ddlView.Items.FindItemByValue("YA"));
        //    }
        //    catch
        //    {
        //    }

        //    try
        //    {
        //        ddlView.Items.Remove(ddlView.Items.FindItemByValue("XA"));
        //    }
        //    catch
        //    {
        //    }

        //    ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindItemByValue("GV"));
        //}
        lblMessage.Text = string.Empty;

        if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2"))
        {
            if (txtRegNo.Text.Trim().Length == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter the " + ddlSearchCriteria.SelectedItem.Text + "#";
                return;
            }
        }
        else
        {
            if (txtEncNo.Text.Trim().Length == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter the " + ddlSearchCriteria.SelectedItem.Text + "#";
                return;
            }
        }
        if (!string.IsNullOrEmpty(txtEncNo.Text) && common.myInt(ddlSearchCriteria.SelectedValue).Equals(2))
        {
            int UHID;
            int.TryParse(txtEncNo.Text, out UHID);
            if ((UHID > 2147483647 || UHID.Equals(0)))
            {
                lblMessage.Text = "Invalid UHID No";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
        }
        BaseC.clsLISPhlebotomy clsLab = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.Patient oPatient = new BaseC.Patient(sConString);
        StringBuilder xmlService = new StringBuilder();
        StringBuilder xmlFieldId = new StringBuilder();
        StringBuilder xmlStationId = new StringBuilder();
        ArrayList coll = new ArrayList();

        DataView DV = new DataView();
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtFilter = new DataTable();
        string cEncId = string.Empty;
        string EncounterNo = string.Empty;
        string RegNo = string.Empty;
        if (common.myStr(lblPatientName.Text) == "" && common.myInt(txtRegNo.Text.Length) > 0)
        {
            lblPatientName.Text = "Patient Name : " + common.myStr(oPatient.getPatientDetailsFromRegistrationNo(common.myStr(txtRegNo.Text), common.myInt(Session["FacilityId"])));
        }
        try
        {
            switch (ddlView.SelectedValue)
            {
                case "GV":
                    //ddlServiceName.Visible = true;
                    //ddlField.Visible = false;
                    dt = GetLabResultData();

                    if (dt.Rows.Count > 0)
                    {
                        gvResultFinal.VirtualItemCount = common.myInt(dt.Rows[0]["TotalRecordsCount"]);
                        ViewState["GraphData"] = dt;
                    }
                    else
                    {
                        gvResultFinal.VirtualItemCount = 0;
                    }
                    DV = dt.DefaultView;


                    if (common.myStr(ddlReportFor.SelectedValue) != "A" && !common.myStr(ddlReportFor.SelectedValue).ToUpper().Equals("ALL"))
                    {
                        DV.RowFilter = "StationId = '" + common.myStr(ddlReportFor.SelectedValue) + "'";
                    }
                    if (common.myStr(ddlReportFor.SelectedValue).ToUpper().Equals("ALL"))
                    {
                        if (!string.IsNullOrEmpty((common.myStr(ViewState["Flag"]))))
                        {
                            if (DV.Count > 0)
                                DV.RowFilter = "FlagName = '" + ViewState["Flag"] + "'";
                        }
                    }
                    dtFilter = DV.ToTable();
                    if (dtFilter.Rows.Count > 0)
                    {
                        gvResultFinal.DataSource = dtFilter;
                        gvResultFinal.DataBind();
                    }
                    else
                    {
                        gvResultFinal.DataSource = dtFilter;
                        gvResultFinal.DataBind();
                    }
                    lblMessage.Text = string.Empty;
                    if (txtRegNo.Text.Length > 0)
                    {
                        gvResultFinal.Columns.FindByUniqueName("PatientName").Visible = false;
                        gvResultFinal.Columns.FindByUniqueName("RegistrationNo").Visible = false;
                    }
                    else
                    {
                        gvResultFinal.Columns.FindByUniqueName("PatientName").Visible = true;
                        gvResultFinal.Columns.FindByUniqueName("RegistrationNo").Visible = true;
                    }

                    break;

                case "XA":

                    //ddlServiceName.Visible = false;
                    //ddlField.Visible = true;

                    xmlFieldId = new StringBuilder();
                    coll = new ArrayList();
                    ArrayList coll1 = new ArrayList();
                    //foreach (RadComboBoxItem FieldItem in ddlField.Items)
                    //{
                    //    if (FieldItem.Checked)
                    //    {
                    //        coll1.Add(common.myInt(FieldItem.Value));
                    //        xmlFieldId.Append(common.setXmlTable(ref coll1));
                    //    }
                    //}

                    foreach (RadComboBoxItem ServiceItem in ddlServiceName.Items)
                    {
                        if (ServiceItem.Checked)
                        {
                            coll.Add(common.myInt(ServiceItem.Value));
                            xmlService.Append(common.setXmlTable(ref coll));
                        }
                    }

                    cEncId = common.myStr(Request.QueryString["EncId"]);

                    if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2"))
                    {
                        RegNo = txtRegNo.Text.Trim();
                    }
                    else
                    {
                        EncounterNo = txtEncNo.Text.Trim();
                    }

                    if (xmlService.ToString() != string.Empty || xmlFieldId.ToString() != string.Empty)
                    {
                        xmlStationId = new StringBuilder();
                        coll = new ArrayList();

                        if (common.myInt(ddlReportFor.SelectedValue) > 0)
                        {
                            coll.Add(common.myInt(ddlReportFor.SelectedValue));
                            xmlStationId.Append(common.setXmlTable(ref coll));
                        }
                        else
                        {
                            for (int idx = 0; idx < common.myInt(ddlReportFor.Items.Count); idx++)
                            {
                                if (common.myInt(ddlReportFor.Items[idx].Value) > 0)
                                {
                                    coll.Add(common.myInt(ddlReportFor.Items[idx].Value));
                                    xmlStationId.Append(common.setXmlTable(ref coll));
                                }
                            }
                        }

                        ViewState["DataHistory"] = string.Empty;

                        ds = clsLab.GetDiagLabResultDynamicGrid(common.myInt(Session["FacilityId"]),
                                                common.myInt(Session["HospitalLocationId"]), dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                                                RegNo, 0, 100, 1, chkAbnormalValue.Checked, chkCriticalValue.Checked, 0, common.myInt(Session["FacilityId"]),
                                                EncounterNo, xmlService.ToString(), xmlFieldId.ToString(), string.Empty, xmlStationId.ToString(), common.myInt(Session["EmployeeId"]));

                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DV = ds.Tables[0].DefaultView;
                                //if (ds.Tables[0].Columns.Contains("Test Result Date"))
                                //{
                                //    DV.Sort = "[Test Result Date] DESC";
                                //}

                                gvLabDetailsXaxis.DataSource = DV.ToTable();
                                gvLabDetailsXaxis.DataBind();
                                ViewState["DataHistory"] = DV.ToTable();
                            }
                        }
                        else
                        {
                            gvLabDetailsXaxis.DataSource = null;
                            gvLabDetailsXaxis.DataBind();
                        }
                    }

                    break;

                case "YA":

                    //ddlField.Visible = true;
                    //ddlServiceName.Visible = false;
                    xmlFieldId = new StringBuilder();
                    coll = new ArrayList();
                    ArrayList coll2 = new ArrayList();
                    //foreach (RadComboBoxItem FieldItem in ddlField.Items)
                    //{
                    //    if (FieldItem.Checked)
                    //    {
                    //        coll2.Add(common.myInt(FieldItem.Value));
                    //        xmlFieldId.Append(common.setXmlTable(ref coll2));
                    //    }
                    //}

                    foreach (RadComboBoxItem ServiceItem in ddlServiceName.Items)
                    {
                        if (ServiceItem.Checked)
                        {
                            coll.Add(common.myInt(ServiceItem.Value));
                            xmlService.Append(common.setXmlTable(ref coll));
                        }
                    }
                    cEncId = common.myStr(Request.QueryString["EncId"]);

                    if (common.myStr(ddlSearchCriteria.SelectedValue).Equals("2"))
                    {
                        RegNo = txtRegNo.Text.Trim();
                    }
                    else
                    {
                        EncounterNo = txtEncNo.Text.Trim();
                    }

                    if (xmlService.ToString() != string.Empty || xmlFieldId.ToString() != string.Empty)
                    {
                        xmlStationId = new StringBuilder();
                        coll = new ArrayList();

                        if (common.myInt(ddlReportFor.SelectedValue) > 0)
                        {
                            coll.Add(common.myInt(ddlReportFor.SelectedValue));
                            xmlStationId.Append(common.setXmlTable(ref coll));
                        }
                        else
                        {
                            for (int idx = 0; idx < common.myInt(ddlReportFor.Items.Count); idx++)
                            {
                                if (common.myInt(ddlReportFor.Items[idx].Value) > 0)
                                {
                                    coll.Add(common.myInt(ddlReportFor.Items[idx].Value));
                                    xmlStationId.Append(common.setXmlTable(ref coll));
                                }
                            }
                        }

                        ViewState["DataHistory"] = string.Empty;

                        ds = clsLab.GetDiagLabResultDynamicGrid(common.myInt(Session["FacilityId"]),
                                                common.myInt(Session["HospitalLocationId"]), dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                                                RegNo, 0, 100, 1, chkAbnormalValue.Checked, chkCriticalValue.Checked, 0, common.myInt(Session["FacilityId"]),
                                                EncounterNo, xmlService.ToString(), xmlFieldId.ToString(), string.Empty, xmlStationId.ToString(), common.myInt(Session["EmployeeId"]));

                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                tbl = ds.Tables[0];
                                DV = tbl.DefaultView;

                                //if (tbl.Columns.Contains("Test Result Date"))
                                //{
                                //    DV.Sort = "[Test Result Date] DESC";
                                //}

                                tbl = common.GenerateTransposedTable(DV.ToTable());

                                if (tbl.Columns.Contains("Test Result Date"))
                                {
                                    tbl.Columns["Test Result Date"].ColumnName = "Test Name";
                                }

                                gvLabDetailsXaxis.DataSource = tbl;
                                gvLabDetailsXaxis.DataBind();
                                ViewState["DataHistory"] = tbl;
                            }
                        }
                        else
                        {
                            gvLabDetailsXaxis.DataSource = null;
                            gvLabDetailsXaxis.DataBind();
                        }
                    }

                    break;
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
            clsLab = null;
            DV.Dispose();
            ds.Dispose();
            tbl.Dispose();
            dt.Dispose();
            dtFilter.Dispose();
        }
    }

    private void FillFields(DataTable objDt)
    {
        if (objDt.Rows.Count > 0)
        {
            ddlField.Items.Clear();
            ddlField.DataSource = objDt;
            ddlField.DataTextField = "FieldName";
            ddlField.DataValueField = "FieldId";
            ddlField.DataBind();
            foreach (RadComboBoxItem currentItem in ddlField.Items)
            {
                currentItem.Checked = true;
            }
        }
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        if (txtRegNo.Text.Trim().Length == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter the " + ddlSearchCriteria.SelectedItem.Text + "#";
            return;
        }
        else
        {
            lblMessage.Text = "";
            StringBuilder sbServices = new StringBuilder();
            ArrayList coll = new ArrayList();
            foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
            {
                if (currentItem.Checked)
                {
                    coll.Add(common.myInt(currentItem.Value));
                    sbServices.Append(common.setXmlTable(ref coll));
                }
            }

            string cEncId = string.Empty, RegNo = string.Empty, EncounterNo = string.Empty;
            cEncId = common.myStr(Request.QueryString["EncId"]);

            if (ddlSearchCriteria.SelectedValue == "2")
            {
                RegNo = txtRegNo.Text.Trim();
            }
            else
            {
                EncounterNo = txtEncNo.Text.Trim();
            }

            Session["sbServices"] = sbServices.ToString();
            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/PrintYAxisReport.aspx?MD=" + Flag
                                          + "&RegNo=" + RegNo + "&EncId=" + common.myStr(cEncId)
                                          + "&dtFrom=" + HttpUtility.UrlEncode(common.myStr(dtpFromDate.SelectedDate.Value))
                                          + "&dtTo=" + HttpUtility.UrlEncode(common.myStr(dtpToDate.SelectedDate.Value));

            RadWindowPopup.Height = 560;
            RadWindowPopup.Width = 900;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.OnClientClose = "SearchOnClientClose";
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;

        }
    }
}
