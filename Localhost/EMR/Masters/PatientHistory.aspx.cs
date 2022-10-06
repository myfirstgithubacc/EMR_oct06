using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;
using BaseC;
using System.Collections;
using System.Text;
using System.IO;

public partial class LIS_Phlebotomy_PatientHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsLISPhlebotomy objval;
    clsExceptionLog objException = new clsExceptionLog();
    string Flag = "";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (Request.QueryString["Master"] != null)
        {
            if (common.myStr(Request.QueryString["Master"]) == "Blank")
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
            lblFromDate.Visible = false;
            lblToDate.Visible = false;
            dtpFromDate.Visible = false;
            dtpToDate.Visible = false;

            //dtpFromDate.SelectedDate = DateTime.Now.AddDays(-30);

            dtpFromDate.SelectedDate = DateTime.Now.AddYears(-1); //AddMonths(-3);
            dtpToDate.SelectedDate = DateTime.Now;

            BindStatusCombo();
            BindBlankGrid();
            BindFacility();
            BindStation();
            //Added by rakesh start
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
            //Added by rakesh end
            else
            {
                if ((Request.QueryString["FacilityId"] != null))
                {
                    ddlFacility.SelectedValue = common.myStr(Request.QueryString["FacilityId"]);
                    ddlFacility.Enabled = false;
                }
                if ((Request.QueryString["RegNo"] != null))
                {
                    ddlSearchCriteria.SelectedValue = "2";
                    txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);

                    bindLabService();

                    txtRegNo.Enabled = false;
                    ddlSearchCriteria.Enabled = false;
                    btnclose.Visible = true;
                }
                if (Request.QueryString["CF"] != null)
                {
                    if ((Request.QueryString["CF"] != null) && (Request.QueryString["CF"] == "EHR") || (Request.QueryString["CF"].Contains("EHR") == true))
                    {
                        btnclose.Visible = false;
                        ddlSearchCriteria.SelectedValue = "2";
                        ddlSearchCriteria.Enabled = false;
                        if (Session["RegistrationNo"] == null)
                        {
                            if (common.myLong(Session["RegistrationNo"]) == 0)
                            {
                                Response.Redirect("/default.aspx?RegNo=0", false);
                            }
                        }
                        else
                        {
                            txtRegNo.Text = Session["RegistrationNo"].ToString();
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

                if (common.myStr(txtRegNo.Text.Trim()) != "")
                {
                    BindResultGrid();
                }
            }
            //ddlView_OnSelectedIndexChanged(this, null);

            rdbTestaxis_SelectedIndexChanged(this, null);

            bindcontrolforsercivceAck(); //For Page Call from ServiceAcknowledge
        }
    }

    public void bindcontrolforsercivceAck()
    {
        if (Request.QueryString["PageID"] == "SAck")
        {

            Label4.Visible = true;
            ddlFacility.Visible = true;
            ddlFacility.SelectedValue = common.myStr(Session["FacilityId"]);
            lblReportType.Visible = true;
            ddlReportFor.Visible = true;
            ddlReportFor.SelectedValue = common.myStr(Session["StationId"]);
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


    protected void txtRegNo_TextChanged(object sender, EventArgs e)
    {
        bindLabService();
    }

    void BindFacility()
    {
        try
        {
            BaseC.User valUser = new BaseC.User(sConString);
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = new DataSet();
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

            ddlFacility.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void BindStatusCombo()
    {
        try
        {
            //DataView dv = new DataView();
            //DataTable dt = new DataTable();
            //DataSet ds = new DataSet();

            //if (Cache["LEGEND"] == null)
            //{
            //    objval = new BaseC.clsLISPhlebotomy(sConString);
            //    ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
            //    Cache["LEGEND"] = ds;

            //}
            //else
            //    ds = (DataSet)Cache["LEGEND"];

            //dv = ds.Tables[0].DefaultView;
            //string StatusType = "'RF'";
            //dv.RowFilter = "Code IN(" + StatusType + ")";
            //dt = dv.ToTable();

            //this.ddlStatus.DataValueField = "StatusId";
            //this.ddlStatus.DataTextField = "Status";
            //this.ddlStatus.DataSource = dt;
            //this.ddlStatus.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
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
        DataView dv = new DataView();
        DataSet ds = new DataSet();
        BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
        try
        {
            ds = objMaster.GetStation(common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region Calling from Icon Menu(LIS/RIS)EMR
                    if (common.myStr(Request.QueryString["Flag"]) != "")
                    {
                        if (common.myStr(Request.QueryString["Flag"]) == "LIS")
                        {
                            dv = new DataView(ds.Tables[0]);
                            dv.RowFilter = "Active=1 AND FlagName ='LIS'";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                ddlReportFor.DataSource = dv.ToTable();
                                ddlReportFor.DataValueField = "StationId";
                                ddlReportFor.DataTextField = "StationName";
                                ddlReportFor.DataBind();
                                ViewState["Flag"] = "LIS";
                            }
                        }
                        else
                        {
                            dv = new DataView(ds.Tables[0]);
                            dv.RowFilter = "Active=1 AND FlagName ='RIS'";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                ddlReportFor.DataSource = dv.ToTable();
                                ddlReportFor.DataValueField = "StationId";
                                ddlReportFor.DataTextField = "StationName";
                                ddlReportFor.DataBind();
                                ViewState["Flag"] = "RIS";
                            }
                        }
                        if (common.myStr(Request.QueryString["Station"]).ToLower().Equals("all"))
                        {
                            ddlReportFor.Items.Insert(0, new RadComboBoxItem("All", "ALL"));
                            ddlReportFor.SelectedIndex = 0;
                            ddlReportFor.SelectedIndex = ddlReportFor.Items.IndexOf(ddlReportFor.Items.FindItemByValue("ALL"));
                        }
                    }
                    #endregion
                    else
                    {
                        dv = new DataView(ds.Tables[0]);
                        dv.RowFilter = "Active=1 AND FlagName IN ('LIS','RIS')";
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            ddlReportFor.DataSource = dv.ToTable();
                            ddlReportFor.DataValueField = "StationId";
                            ddlReportFor.DataTextField = "StationName";
                            ddlReportFor.DataBind();
                            ddlReportFor.Items.Insert(0, new RadComboBoxItem("All", "A"));
                            ddlReportFor.SelectedIndex = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            dv.Dispose();
            ds.Dispose();
        }
    }


    void bindLabService()
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
        clsLISPhlebotomy cLab = new clsLISPhlebotomy(sConString);
        string cEncId = common.myStr(Request.QueryString["EncId"]);

        if (cEncId == "")
        {
            if (ddlSearchCriteria.SelectedValue == "1")
            {
                clsIVF objIvf = new clsIVF(sConString);
                cEncId = objIvf.getEncounterId(txtEncNo.Text, common.myInt(Session["FacilityId"])).ToString();
            }
        }

        ViewState["DataServiceView"] = "";
        string currentdate = "1/27/2013 12:00:00 AM";


        //DataSet ds = cLab.getDiagPatientLabServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
        //                dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
        //                (ddlSearchCriteria.SelectedValue == "2") ? common.myStr(txtRegNo.Text) : "", cEncId);

        DataSet ds = cLab.getDiagPatientLabServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
                      dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                      (ddlSearchCriteria.SelectedValue == "2") ? common.myStr(txtRegNo.Text) : "", cEncId);


        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ServiceName";
            DataTable dt = dv.ToTable(true, "ServiceName", "ServiceId");
            ddlServiceName.DataSource = dt;
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

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            //bindLabService();

            BindResultGrid();
        }
        catch
        {
        }
    }

    protected void ddlSearchCriteria_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtEncNo.Text = "";
        lblMessage.Text = string.Empty;
        txtRegNo.Visible = false;
        txtEncNo.Visible = false;

        if (ddlSearchCriteria.SelectedValue == "2")
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
            if (Flag.ToString() == "RIS")
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
            if (img.CommandName.Length > 0)
            {
                img.Visible = true;
            }
            else
            {
                img.Visible = false;
            }
            if (common.myStr(lblStationId.Text) == "1")
                lnkServiceName.Enabled = true;
            else
                lnkServiceName.Enabled = false;

            if (lblresult.Text.Trim() == "Result")
            {
                lnkResult.CommandName = "Result";
                lblresult.Visible = false;
                lnkResult.Visible = true;

            }
            else if (lblresult.Text.Trim() == "Download")
            {
                lnkResult.CommandName = "Download";
                lblresult.Visible = false;
                lnkResult.Visible = true;
                lnkprint.Visible = false;
            }
            else
            {
                lblresult.Visible = true;
                lnkResult.Visible = false;
            }
            if (common.myBool(lblAbnormalValue.Text) == true && common.myBool(lblCriticalValue.Text) == false)
            {
                lblresult.ForeColor = System.Drawing.Color.DarkViolet;
            }
            if (common.myBool(lblCriticalValue.Text) == true)
            {
                lblresult.ForeColor = System.Drawing.Color.Red;
            }
            if (txtRegNo.Text.Length > 0)
            {
                lblPatientName.Text = "Patient Name : " + lblPatientNameGrid.Text;
            }
            else
            {
                lblPatientName.Text = "";
            }
        }
    }

    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            if (e.CommandName == "Result")
            {
                string Source = "";
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");

                if (lblSource.Text.Trim() == "ER")
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
            else if (e.CommandName == "Download")
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                DataSet ds = new DataSet();
                if (lblSource.Text == "IPD")
                {
                    ds = objval.FillAttachmentDownloadDropdownIP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");

                }
                else
                {
                    ds = objval.FillAttachmentDownloadDropdownOP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string sFileName = ds.Tables[0].Rows[0]["DocumentName"].ToString();
                        string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
                        string path = Server.MapPath(sSavePath + sFileName);
                        System.IO.FileInfo file = new System.IO.FileInfo(path);
                        if (file.Exists)
                        {
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                            Response.AddHeader("Content-Length", file.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(file.FullName);
                            Response.BinaryWrite(File.ReadAllBytes(file.FullName));
                            Response.End();




                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "File does not exist...";
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
            else if (e.CommandName == "Print")
            {
                HiddenField hdnResultHTML = (HiddenField)e.Item.FindControl("hdnResultHTML");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblStationId = (Label)e.Item.FindControl("lblStationId");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblRegistrationId = (Label)e.Item.FindControl("lblRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");

                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");

                if (common.myStr(lblStatusCode.Text).Trim().ToUpper().Equals("RE"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Result is not certified. Print not allowed!";
                    Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
                    return;
                }

                if (hdnResultHTML.Value != "ResultHTML")
                {
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + common.myStr(lblSource.Text)
                                                + "&LABNO=" + common.myInt(lblLabNo.Text) + "&StationId=" + common.myInt(lblStationId.Text) +
                                                "&ServiceIds=" + lblServiceId.Text;
                }
                else
                {
                    ArrayList coll = new ArrayList();
                    StringBuilder objXML = new StringBuilder();
                    coll.Add(common.myStr(lblDiagSampleID.Text));
                    coll.Add(common.myStr(lblSource.Text));
                    objXML.Append(common.setXmlTable(ref coll));

                    RadWindowPopup.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + common.myStr(lblSource.Text) +
                                        "&LABNO=" + common.myInt(lblLabNo.Text) + "&ServiceIds=" + lblServiceId.Text +
                                        "&StationId=" + common.myInt(lblStationId.Text) + "&Flag=" + Flag + "&RegId=" + common.myInt(lblRegistrationId.Text) +
                                        "&EncId=" + common.myInt(hdnEncounterId.Value) + "&DiagSampleId=" + objXML.ToString();
                }
                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;
                RadWindowPopup.Left = 10;
                RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (e.CommandName == "Investigation")
            {
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");

                string DIAG_SAMPLEID_S = "";
                //foreach (GridDataItem dataItem in gvResultFinal.Items)
                //{
                //    Label lblServiceId_G = (Label)dataItem.FindControl("lblServiceId");
                //    if (common.myInt(lblServiceId_G.Text) == common.myInt(lblServiceId.Text))
                //    {
                //        Label lblDiagSampleID_G = (Label)dataItem.FindControl("lblDiagSampleID");
                //        if (DIAG_SAMPLEID_S != "")
                //        {
                //            DIAG_SAMPLEID_S += "," + common.myInt(lblDiagSampleID_G.Text).ToString();
                //        }
                //        else
                //        {
                //            DIAG_SAMPLEID_S = common.myInt(lblDiagSampleID_G.Text).ToString();
                //        }
                //    }
                //}
                DataTable dtgraph = (DataTable)ViewState["GraphData"];

                foreach (DataRow dr in dtgraph.Rows)
                {
                    int lblServiceId_G = common.myInt(dr["ServiceId"]);
                    if (common.myInt(lblServiceId_G) == common.myInt(lblServiceId.Text))
                    {
                        int lblDiagSampleID_G = (common.myInt(dr["DiagSampleID"]));

                        if (DIAG_SAMPLEID_S != "")
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
                                            + "&ServiceName=" + common.myStr(lnkServiceName.Text);

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
            lblMessage.Text = "Please Enter the " + ddlSearchCriteria.SelectedItem.Text + "#";
            return;
        }
        else
        {
            RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientLabHistoryDynamic.aspx?Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(txtRegNo.Text.Trim()) + "";
            RadWindow1.Height = 600;
            RadWindow1.Width = 1040;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;

            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }

    }
    protected void imgViewImage_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string viewer = dl.ExecuteScalar(CommandType.Text, "select viewerurl from DiagHISPACSIntegrationSetup where FacilityId= " + common.myInt(Session["FacilityId"])).ToString();
        string key = dl.ExecuteScalar(CommandType.Text, "select SharedKey from DiagHISPACSIntegrationSetup where FacilityId= " + common.myInt(Session["FacilityId"])).ToString();
        EncryptDecrypt en = new EncryptDecrypt();
        ImageButton img = (ImageButton)sender;
        string accessionno = img.CommandName.ToString();
        viewer = viewer.Replace("@accessionNo", en.Encrypt(accessionno, key, true, ""));
        //viewer = viewer.Replace("@userid", en.Encrypt(Session["UserID"].ToString(), key, true, ""));
        //viewer = viewer.Replace("@datetime", en.Encrypt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), key, true, ""));
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


    protected void gvLabDetailsXaxis_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[0].Width = Unit.Pixel(35);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    String S = e.Row.Cells[i].Text.ToString().Replace("&nbsp;", "");

                    if (i != 0 && S != "")
                    {
                        LinkButton lnk = new LinkButton();
                        string sDiagSampleId = "", sServiceId = "", sSource = "", sStatusCode = "", sServiceName = "", sAgeGender = "", sType = "", sFieldId = ""; ;
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
                        int iStart = 0, iEnd = 0; ;

                        iEnd = (int)S.IndexOf("#");
                        lnk.Text = S.ToString().Substring(iStart, iEnd);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf("#") + 1;
                        iEnd = (int)S.IndexOf("@");
                        sDiagSampleId = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf("@") + 1;
                        iEnd = (int)S.IndexOf(",SId");
                        sSource = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf(",SId=") + 5;
                        iEnd = (int)S.IndexOf(",SNM=");
                        sServiceId = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf(",SNM=") + 5;
                        iEnd = (int)S.IndexOf(",SCD=");
                        sServiceName = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf(",SCD=") + 5;
                        iEnd = (int)S.IndexOf(",AB=");
                        sStatusCode = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf(",FID=") + 5;
                        iEnd = (int)S.IndexOf(",DOB=");
                        sFieldId = S.ToString().Substring(iStart, iEnd - iStart);

                        iStart = 0; iEnd = 0;
                        iStart = (int)S.IndexOf(",DOB=") + 5;
                        iEnd = (int)S.Length;
                        sAgeGender = S.ToString().Substring(iStart, iEnd - iStart);


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

                        lnk.Attributes.Add("OnClick", "showResultPopup(event,'" + sDiagSampleId + "','" + sServiceId + "','" + sSource + "','" + sStatusCode + "','" + sServiceName + "','" + sAgeGender + "','" + sFieldId + "','" + sType + "','" + i + "','" + common.myInt(e.Row.RowIndex) + "' );");
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Transparent;
                        e.Row.Cells[i].Controls.Add(lnk);
                    }
                    else
                    {
                        Label lbl = new Label();
                        lbl.Text = S.ToString();
                        e.Row.Cells[i].Controls.Add(lbl);
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

    protected void btnCallcombo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DataServiceView"].ToString() == "")
            {
                return;
            }
            if (((DataSet)ViewState["DataServiceView"]).Tables.Count > 0)
            {
                if (((DataSet)ViewState["DataServiceView"]).Tables[0].Rows.Count > 0)
                {
                    gvLabDetailsXaxis.DataSource = null;
                    gvLabDetailsXaxis.DataBind();

                    string strItem = "";

                    foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
                    {
                        if (currentItem.Checked == true)
                        {
                            if (strItem != "")
                                strItem = strItem + "," + common.myStr(currentItem.Value);
                            else
                                strItem = common.myStr(currentItem.Value);
                        }
                    }

                    //DataView dvField = ((DataSet)ViewState["DataServiceView"]).Tables[0].DefaultView;
                    //if (strItem != "")
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
            if (hdnsType.Value.ToString() != "")
            {
                if (hdnsType.Value.ToString() == "Result")
                {
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + hdnsSource.Value
                                                           + "&DIAG_SAMPLEID=" + common.myInt(hdnsDiagSampleId.Value)
                                                           + "&SERVICEID=" + common.myInt(hdnsServiceId.Value)
                                                           + "&AgeInDays=" + common.myStr(hdnsAgeGender.Value.ToString())
                                                           + "&StatusCode=" + common.myStr(hdnsStatusCode.Value)
                                                           + "&ServiceName=" + common.myStr(hdnsServiceName.Value);
                }
                else
                {
                    string S = string.Empty;
                    int iStart = 0;
                    int iEnd = 0;
                    string sDSId = string.Empty;
                    string sDiagSampleIds = string.Empty;

                    switch (common.myStr(rdbTestaxis.SelectedValue))
                    {
                        case "XA": //Test on X-axis
                            for (int i = 0; i < gvLabDetailsXaxis.Rows.Count; i++)
                            {
                                S = common.myStr(gvLabDetailsXaxis.Rows[i].Cells[common.myInt(hdnsCellId.Value)].Text).Replace("&nbsp;", "");
                                if (common.myLen(S) > 0)
                                {
                                    iStart = 0;
                                    iEnd = 0;
                                    sDSId = string.Empty;
                                    iStart = (int)S.IndexOf("#") + 1;
                                    iEnd = (int)S.IndexOf("@");
                                    sDSId = common.myStr(S).Substring(iStart, iEnd - iStart);

                                    if (common.myLen(sDiagSampleIds).Equals(0) && common.myLen(sDSId) > 0)
                                    {
                                        sDiagSampleIds = sDSId;
                                    }
                                    else if (common.myLen(sDSId) > 0)
                                    {
                                        sDiagSampleIds = sDiagSampleIds + "," + sDSId;
                                    }
                                }
                            }
                            break;

                        case "YA": //Test on Y-axis
                            if (common.myInt(hdnsRowId.Value) > 0)
                            {
                                if (ViewState["DataHistory"] != null)
                                {
                                    DataTable tblDataHistory = (DataTable)ViewState["DataHistory"];
                                    try
                                    {
                                        if (tblDataHistory != null)
                                        {
                                            for (int i = 1; i < tblDataHistory.Columns.Count; i++)
                                            {
                                                S = common.myStr(gvLabDetailsXaxis.Rows[common.myInt(hdnsRowId.Value)].Cells[i].Text).Replace("&nbsp;", "");
                                                if (common.myLen(S) > 0)
                                                {
                                                    iStart = 0;
                                                    iEnd = 0;
                                                    sDSId = string.Empty;
                                                    iStart = (int)S.IndexOf("#") + 1;
                                                    iEnd = (int)S.IndexOf("@");
                                                    sDSId = common.myStr(S).Substring(iStart, iEnd - iStart);

                                                    if (common.myLen(sDiagSampleIds).Equals(0) && common.myLen(sDSId) > 0)
                                                    {
                                                        sDiagSampleIds = sDSId;
                                                    }
                                                    else if (common.myLen(sDSId) > 0)
                                                    {
                                                        sDiagSampleIds = sDiagSampleIds + "," + sDSId;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    finally
                                    {
                                        tblDataHistory.Dispose();
                                    }
                                }
                            }
                            break;
                    }

                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/LabResultGraph.aspx?SOURCE=" + hdnsSource.Value
                                                + "&DIAG_SAMPLEID=" + common.myInt(hdnsDiagSampleId.Value)
                                                + "&DIAG_SAMPLEID_S=" + common.myStr(sDiagSampleIds)
                                                + "&SERVICEID=" + common.myInt(hdnsServiceId.Value)
                                                + "&AgeInDays=" + common.myStr(hdnsAgeGender.Value)
                                                + "&StatusCode=" + common.myStr(hdnsStatusCode.Value)
                                                + "&ServiceName=" + common.myStr(hdnsServiceName.Value)
                                                + "&FieldId=" + common.myStr(hdnsFieldId.Value)
                                                + "&FDate=" + common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd")
                                                + "&TDate=" + common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd");

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
            if (((DataTable)ViewState["DataHistory"]).Rows.Count > 0)
            {
                gvLabDetailsXaxis.DataSource = ((DataTable)ViewState["DataHistory"]);
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

    protected void ddlView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvResultFinal.Visible = false;
            gvLabDetailsXaxis.Visible = false;

            switch (ddlView.SelectedValue)
            {
                case "GV":
                    gvResultFinal.Visible = true;
                    break;

                case "XA":
                    gvLabDetailsXaxis.Visible = true;
                    break;

                case "YA":
                    gvLabDetailsXaxis.Visible = true;
                    break;
            }
            BindResultGrid();
        }
        catch
        {
        }
    }

    protected DataTable GetLabResultData()
    {
        DataSet ds = new DataSet();
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            int iProviderID = 0;
            int pageindex = 0;
            string EncounterNo = "", RegNo = "";
            if (gvResultFinal.Items.Count > 0)
            {
                pageindex = gvResultFinal.CurrentPageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }
            if (ddlSearchCriteria.SelectedValue == "2")
            {
                RegNo = txtRegNo.Text.Trim();
                if (Request.QueryString["PageSource"] != null)
                {
                    if (Request.QueryString["PageSource"].Equals("Ward"))
                    {
                        EncounterNo = common.myStr(Session["EncounterNo"]);
                    }
                }
            }
            else
            {
                EncounterNo = txtEncNo.Text.Trim();
            }

            int StationID = 0;
            if (common.myStr(Request.QueryString["PageID"]) == "SAck")
            {
                StationID = common.myInt(Session["StationId"]);
            }
            ds = objval.getPatientLabResultHistory(common.myInt(Session["FacilityID"]),
                                    common.myInt(Session["HospitalLocationID"]),
                                    common.myDate(dtpFromDate.SelectedDate),
                                    common.myDate(dtpToDate.SelectedDate),
                                    common.myStr(RegNo),
                                    iProviderID,
                                    0,
                                    pageindex,
                                    chkAbnormalValue.Checked,
                                    chkCriticalValue.Checked,
                                    0,
                                    common.myInt(ddlFacility.SelectedValue), EncounterNo, "B", 0, common.myInt(ddlServiceName.SelectedValue), 
                                    StationID, "RE", "", 0, 0, false);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds.Tables[0];
    }

    protected void BindResultGrid()
    {
        lblMessage.Text = "";

        if (ddlSearchCriteria.SelectedValue == "2")
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
        StringBuilder xmlService = new StringBuilder();
        StringBuilder xmlFieldId = new StringBuilder();
        ArrayList coll = new ArrayList();
        DataView DV = new DataView();
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtFilter = new DataTable();
        string cEncId = "";
        string EncounterNo = "";
        string RegNo = "";
        try
        {

            //switch (ddlView.SelectedValue)
            switch (rdbTestaxis.SelectedValue)
            {
                case "GV":
                    dt = GetLabResultData();

                    if (dt.Rows.Count > 0)
                    {
                        gvResultFinal.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["TotalRecordsCount"]);
                        ViewState["GraphData"] = dt;
                    }
                    else
                    {
                        gvResultFinal.VirtualItemCount = Convert.ToInt32(0);
                    }
                    DV = dt.DefaultView;


                    if (ddlReportFor.SelectedValue.ToString() != "A" && !ddlReportFor.SelectedValue.ToString().ToUpper().Equals("ALL"))
                    {
                        DV.RowFilter = "StationId = '" + ddlReportFor.SelectedValue.ToString() + "'";
                    }
                    if (ddlReportFor.SelectedValue.ToString().ToUpper().Equals("ALL"))
                    {
                        if (!string.IsNullOrEmpty((common.myStr(ViewState["Flag"]))))
                            DV.RowFilter = "FlagName = '" + ViewState["Flag"] + "'";
                    }
                    dtFilter = DV.ToTable();

                    gvResultFinal.DataSource = dtFilter;
                    gvResultFinal.DataBind();
                    lblMessage.Text = "";
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

                    foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
                    {
                        if (currentItem.Checked)
                        {
                            coll.Add(common.myInt(currentItem.Value));
                            xmlService.Append(common.setXmlTable(ref coll));
                        }
                    }

                    xmlFieldId = new StringBuilder();
                    coll = new ArrayList();

                    cEncId = common.myStr(Request.QueryString["EncId"]);

                    if (ddlSearchCriteria.SelectedValue == "2")
                    {
                        RegNo = txtRegNo.Text.Trim();
                    }
                    else
                    {
                        EncounterNo = txtEncNo.Text.Trim();
                    }

                    if (xmlService.ToString() != "")
                    {
                        ViewState["DataHistory"] = "";
                        clsLISPhlebotomy clsLab = new clsLISPhlebotomy(sConString);


                        //ds = clsLab.GetDiagLabResultDynamicGrid(common.myInt(Session["FacilityId"]),
                        //    common.myInt(Session["HospitalLocationId"]), dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                        //    RegNo, 0, 100, 1, chkAbnormalValue.Checked, chkCriticalValue.Checked, 0, common.myInt(Session["FacilityId"]), EncounterNo
                        //    , xmlService.ToString(), xmlFieldId.ToString(), cEncId, ddlReportFor.SelectedValue);

                        ds = clsLab.GetDiagLabResultDynamicGrid(common.myInt(Session["FacilityId"]),
                                  common.myInt(Session["HospitalLocationId"]), dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                                  RegNo, 0, 100, 1, chkAbnormalValue.Checked, chkCriticalValue.Checked, 0, common.myInt(Session["FacilityId"]), EncounterNo,
                                  xmlService.ToString(), xmlFieldId.ToString(), cEncId, "A", common.myInt(Session["EmployeeId"]),false);

                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DV = ds.Tables[0].DefaultView;
                                if (ds.Tables[0].Columns.Contains("Test Result Date"))
                                {
                                    DV.Sort = "[Test Result Date] DESC";
                                }

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

                    foreach (RadComboBoxItem currentItem in ddlServiceName.Items)
                    {
                        if (currentItem.Checked)
                        {
                            coll.Add(common.myInt(currentItem.Value));
                            xmlService.Append(common.setXmlTable(ref coll));
                        }
                    }

                    xmlFieldId = new StringBuilder();
                    coll = new ArrayList();

                    cEncId = common.myStr(Request.QueryString["EncId"]);

                    if (ddlSearchCriteria.SelectedValue == "2")
                    {
                        RegNo = txtRegNo.Text.Trim();
                    }
                    else
                    {
                        EncounterNo = txtEncNo.Text.Trim();
                    }

                    if (xmlService.ToString() != "")
                    {
                        ViewState["DataHistory"] = "";
                        clsLISPhlebotomy clsLab = new clsLISPhlebotomy(sConString);
                        ds = clsLab.GetDiagLabResultDynamicGrid(common.myInt(Session["FacilityId"]),
                            common.myInt(Session["HospitalLocationId"]), dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value,
                            RegNo, 0, 100, 1, chkAbnormalValue.Checked, chkCriticalValue.Checked, 0, common.myInt(Session["FacilityId"]), EncounterNo
                            , xmlService.ToString(), xmlFieldId.ToString(), cEncId, "A", common.myInt(Session["EmployeeId"]));

                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                tbl = ds.Tables[0];
                                DV = tbl.DefaultView;

                                if (tbl.Columns.Contains("Test Result Date"))
                                {
                                    DV.Sort = "[Test Result Date] DESC";
                                }

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
            DV.Dispose();
            ds.Dispose();
            tbl.Dispose();
            dt.Dispose();
            dtFilter.Dispose();
        }
    }

    protected void rbtnSearh_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblFromDate.Visible = false;
            lblToDate.Visible = false;
            dtpFromDate.Visible = false;
            dtpToDate.Visible = false;

            dtpFromDate.SelectedDate = DateTime.Now.AddYears(-1);//AddMonths(-3);
            dtpToDate.SelectedDate = DateTime.Now;

            switch (common.myStr(rbtnSearh.SelectedValue))
            {
                case "DateRange":
                    lblFromDate.Visible = true;
                    lblToDate.Visible = true;
                    dtpFromDate.Visible = true;
                    dtpToDate.Visible = true;
                    break;
                case "MM-1":
                    dtpFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    break;
                case "MM-3":
                    dtpFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    break;
                case "MM-6":
                    dtpFromDate.SelectedDate = DateTime.Now.AddMonths(-6);
                    break;
                case "YY-1":
                    dtpFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    break;
            }

            BindResultGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected void rdbTestaxis_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvResultFinal.Visible = false;
            gvLabDetailsXaxis.Visible = false;

            //switch (ddlView.SelectedValue)

            switch (rdbTestaxis.SelectedValue)
            {
                case "GV":
                    gvResultFinal.Visible = true;
                    break;

                case "XA":
                    gvLabDetailsXaxis.Visible = true;
                    break;

                case "YA":
                    gvLabDetailsXaxis.Visible = true;
                    break;
            }
            BindResultGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }


    }
}
