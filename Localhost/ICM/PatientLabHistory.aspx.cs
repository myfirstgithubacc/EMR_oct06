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
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using BaseC;

public partial class ICM_PatientLabHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsLISPhlebotomy objval;
    clsExceptionLog objException = new clsExceptionLog();
    string Flag = "";
    private const int ItemsPerRequest = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        dtpFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        if (!IsPostBack)
        {
            ViewState["AddLabCart"] = "";
            BindStation();
            BindSubDepartment();
            BindStatusCombo();
            BindBlankGrid();

            BindBlankLabCartGrid();

            //if (Request.QueryString["CF"] != null)
            //{
            //    if ((Request.QueryString["CF"] != null) && (Request.QueryString["CF"] == "EHR") || (Request.QueryString["CF"].Contains("EHR") == true))
            //    {
            //        btnclose.Visible = false;

            //    }
            //}
            if (Request.QueryString["MD"] != null)
            {
                Flag = common.myStr(Request.QueryString["MD"]);
            }
            gvResultFinal.CurrentPageIndex = 0;
            if (common.myStr(Request.QueryString["AdmissionDate"]) == "")
                dtpFromDate.SelectedDate = DateTime.Now;
            else
                dtpFromDate.SelectedDate = Convert.ToDateTime(Request.QueryString["AdmissionDate"]);// DateTime.Now.AddDays(-(Convert.ToDateTime(Request.QueryString["Admisiondate"]).Day));

            dtpToDate.SelectedDate = DateTime.Now;
            btnSearch_OnClick(null, null);
            //  gvResultFinal.Columns[0].Visible = false; //Source
            gvResultFinal.Columns[9].Visible = false;//print

        }
    }
    void BindStatusCombo()
    {
        try
        {
            DataView dv = new DataView();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            if (Cache["LEGEND"] == null)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
                Cache["LEGEND"] = ds;

            }
            else
                ds = (DataSet)Cache["LEGEND"];

            dv = ds.Tables[0].DefaultView;
            string StatusType = "'RF'";
            dv.RowFilter = "Code IN(" + StatusType + ") ";
            dt = dv.ToTable();

            this.ddlStatus.DataValueField = "StatusId";
            this.ddlStatus.DataTextField = "Status";
            this.ddlStatus.DataSource = dt;
            this.ddlStatus.DataBind();
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
        string StationId = string.Empty;
        DataSet ds = objMaster.GetStation(common.myInt(Session["HospitalLocationId"]), 0);
        DataView dv = new DataView(ds.Tables[0]);
        if (common.myStr(Request.QueryString["Station"]).ToUpper().Equals("LAB"))
        {
            dv.RowFilter = "LabType='G' and FlagName = 'LIS'  and Active = 1";
        }
        else if (common.myStr(Request.QueryString["Station"]).ToUpper().Equals("RADIOLOGY"))
        {
            dv.RowFilter = "LabType='X' and FlagName = 'RIS' and Active = 1";
        }
        else if (common.myStr(Request.QueryString["Station"]).ToUpper().Equals("OTHER"))
        {
            dv.RowFilter = "LabType in ('X','G') and FlagName = 'OTH'  and Active = 1";
        }
        if (dv.ToTable().Rows.Count > 0)
        {
            ddlReportFor.DataSource = dv.ToTable();
            ddlReportFor.DataValueField = "StationId";
            ddlReportFor.DataTextField = "StationName";
            ddlReportFor.DataBind();
            ddlReportFor.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlReportFor.SelectedIndex = 0;

            for (int i = 0; i <= common.myInt(dv.ToTable().Rows.Count) - 1; i++)
            {

                if (!i.Equals(common.myInt(dv.ToTable().Rows.Count) - 1))
                {
                    StationId = StationId + common.myStr(dv.ToTable().Rows[i]["StationId"]) + ",";
                }
                else
                {
                    StationId = StationId + common.myStr(dv.ToTable().Rows[i]["StationId"]);
                }
            }
            ViewState["StationId"] = StationId;
        }
        else
        {
            ddlReportFor.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlReportFor.SelectedIndex = 0;
        }


    }

    protected void ddlReportFor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubDepartment();
        BindResultGrid();
    }
    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlService.ClearSelection();
        ddlService.Text = "";
        BindResultGrid();
        // BindSubDepartment();
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        BindResultGrid();
    }

    protected void BindResultGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = GetLabResultData();
            if (dt.Rows.Count > 0)
            {
                gvResultFinal.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["TotalRecordsCount"]);
            }
            else
            {
                gvResultFinal.VirtualItemCount = Convert.ToInt32(0);
            }
            DataView dv = dt.DefaultView;
            DataTable dtFilter = new DataTable();

            //if (ddlReportFor.SelectedValue.ToString() != "A")
            if (ddlReportFor.SelectedValue.ToString() != "0")
                dv.RowFilter = "StationId = '" + ddlReportFor.SelectedValue.ToString() + "'";
            //else
            //    dv.RowFilter = "StationId = '" + ddlReportFor.SelectedValue.ToString() + "'";
            else
            {
                if (ViewState["StationId"] != null && !common.myStr(ViewState["StationId"]).Equals(string.Empty))
                {
                    dv.RowFilter = "StationId in ( " + common.myStr(ViewState["StationId"]) + ")  ";
                }
            }


            dtFilter = dv.ToTable();

            ViewState["LabData"] = dtFilter;
            gvResultFinal.DataSource = dtFilter;
            gvResultFinal.DataBind();
            lblMessage.Text = "";
            //if (txtRegNo.Text.Length > 0)
            //{
            gvResultFinal.Columns.FindByUniqueName("PatientName").Visible = false;
            gvResultFinal.Columns.FindByUniqueName("RegistrationNo").Visible = false;
            //}
            //else
            //{
            //    gvResultFinal.Columns.FindByUniqueName("PatientName").Visible = true;
            //    gvResultFinal.Columns.FindByUniqueName("RegistrationNo").Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected DataTable GetLabResultData()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        objval = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
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
            RegNo = common.myStr(Request.QueryString["RegNo"]);
            EncounterNo = common.myStr(Request.QueryString["EncNo"]);

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
                                        0, common.myInt(Session["FacilityId"]), EncounterNo, "B",
                                        common.myInt(ddlSubDepartment.SelectedValue),
                                        common.myInt(ddlService.SelectedValue), common.myInt(ddlReportFor.SelectedValue), "",
                                        common.myStr(Request.QueryString["Discharge"]), common.myInt(Session["UserId"]), 1, false);


            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "StatusCode='RF'";
            dt = dv.ToTable();
            dv.Dispose();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            dt.Dispose();
            objval = null;
        }
        return dt;
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

            //Label lblSource = (Label)e.Item.FindControl("lblSource");
            //if (common.myStr(Request.QueryString["From"]) == "Ward")
            //{
            //    lblSource.Visible = false;
            //}
            if (img.CommandName.Length > 0)
            {
                img.Visible = true;
            }
            else
            {
                img.Visible = false;
            }
            //if (common.myStr(lblStationId.Text) == "1")
            //    lnkServiceName.Enabled = true;
            //else
            //    lnkServiceName.Enabled = false;

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
            //if (txtRegNo.Text.Length > 0)
            //{
            lblPatientName.Text = "Patient Name : " + lblPatientNameGrid.Text;
            //}
            //else
            //{
            //    lblPatientName.Text = "";
            //}

            // ChangeColour();

            DataTable dt = (DataTable)ViewState["AddLabCart"];
            DataView dv = dt.DefaultView;
            dv.RowFilter = "DiagSampleID =" + ((Label)e.Item.FindControl("lblDiagSampleID")).Text;
            if (dv.ToTable().Rows.Count > 0)
            {
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                e.Item.BackColor = System.Drawing.Color.White;
            }

        }
    }
    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Result")
            {
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");

                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + lblSource.Text
                                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                                            + "&SERVICEID=" + common.myInt(lblServiceId.Text)
                                            + "&AgeInDays=" + common.myStr(lblAgeGender.Text)
                                            + "&StatusCode=" + common.myStr(lblStatusCode.Text)
                                            + "&ServiceName=" + common.myStr(lnkServiceName.Text);

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
                if (lblSource.Text == "OPD")
                {
                    ds = objval.FillAttachmentDownloadDropdownOP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");
                }
                else
                {
                    ds = objval.FillAttachmentDownloadDropdownIP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");
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
                        RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/Download.aspx?SampleId="
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
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblStationId = (Label)e.Item.FindControl("lblStationId");

                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE="
                    + common.myStr(lblSource.Text)
                    + "&LABNO=" + common.myInt(lblLabNo.Text)
                    + "&StationId=" + common.myInt(lblStationId.Text);
                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;
                RadWindowPopup.Left = 10;
                RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
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
                foreach (GridDataItem dataItem in gvResultFinal.Items)
                {
                    Label lblServiceId_G = (Label)dataItem.FindControl("lblServiceId");

                    if (common.myInt(lblServiceId_G.Text) == common.myInt(lblServiceId.Text))
                    {
                        Label lblDiagSampleID_G = (Label)dataItem.FindControl("lblDiagSampleID");

                        if (DIAG_SAMPLEID_S != "")
                        {
                            DIAG_SAMPLEID_S += "," + common.myInt(lblDiagSampleID_G.Text).ToString();
                        }
                        else
                        {
                            DIAG_SAMPLEID_S = common.myInt(lblDiagSampleID_G.Text).ToString();
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
            else if (e.CommandName == "AddList")
            {
                DataTable dt = (DataTable)ViewState["AddLabCart"];
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "DiagSampleID=" + ((Label)e.Item.FindControl("lblDiagSampleID")).Text;
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["LabNo"] = ((Label)e.Item.FindControl("lblLabNo")).Text;
                        dr["Provider"] = ((Label)e.Item.FindControl("lblProvider")).Text;
                        dr["ServiceName"] = ((Label)e.Item.FindControl("lblServiceName")).Text;
                        dr["Result"] = ((Label)e.Item.FindControl("lblresult")).Text;
                        dr["ServiceId"] = ((Label)e.Item.FindControl("lblServiceId")).Text;
                        dr["DiagSampleID"] = ((Label)e.Item.FindControl("lblDiagSampleID")).Text;
                        dr["OrderDate"] = ((Label)e.Item.FindControl("lblOrderDate")).Text;
                        dr["Source"] = ((Label)e.Item.FindControl("lblSource")).Text;

                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["LabNo"] = ((Label)e.Item.FindControl("lblLabNo")).Text;
                    dr["Provider"] = ((Label)e.Item.FindControl("lblProvider")).Text;
                    dr["ServiceName"] = ((Label)e.Item.FindControl("lblServiceName")).Text;
                    dr["Result"] = ((Label)e.Item.FindControl("lblresult")).Text;
                    dr["ServiceId"] = ((Label)e.Item.FindControl("lblServiceId")).Text;
                    dr["DiagSampleID"] = ((Label)e.Item.FindControl("lblDiagSampleID")).Text;
                    dr["OrderDate"] = ((Label)e.Item.FindControl("lblOrderDate")).Text;
                    dr["Source"] = ((Label)e.Item.FindControl("lblSource")).Text;
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
                gvLabHistoryCart.DataSource = dt.Copy();
                gvLabHistoryCart.DataBind();
                ViewState["AddLabCart"] = dt;
                ChangeColour();
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

        //ChangeColour();
    }

    protected void imgViewImage_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string viewer = dl.ExecuteScalar(CommandType.Text, "select viewerurl from DiagHISPACSIntegrationSetup").ToString();
        string key = dl.ExecuteScalar(CommandType.Text, "select SharedKey from DiagHISPACSIntegrationSetup").ToString();
        EncryptDecrypt en = new EncryptDecrypt();
        ImageButton img = (ImageButton)sender;
        string accessionno = img.CommandName.ToString();
        viewer = viewer.Replace("@accessionNo", en.Encrypt(accessionno, key, true, ""));
        viewer = viewer.Replace("@userid", en.Encrypt(Session["UserID"].ToString(), key, true, ""));
        viewer = viewer.Replace("@datetime", en.Encrypt(DateTime.Now.ToString("yyyyyMMddHHmm"), key, true, ""));
        RadWindowPopup.NavigateUrl = viewer;
        RadWindowPopup.Height = 550;
        RadWindowPopup.Width = 800;
        RadWindowPopup.Top = 45;
        RadWindowPopup.Left = 10;
        RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowPopup.Modal = true;
        RadWindowPopup.VisibleStatusbar = false;
    }

    void ChangeColour()
    {
        DataTable dt = (DataTable)ViewState["AddLabCart"];
        foreach (GridItem dr in gvResultFinal.Items)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "DiagSampleID =" + ((Label)dr.FindControl("lblDiagSampleID")).Text;
            if (dv.ToTable().Rows.Count > 0)
            {
                dr.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                dr.BackColor = System.Drawing.Color.White;
            }
        }
    }
    protected void btnCloseW_Click(object sender, EventArgs e)
    {
        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();

        if (common.myInt(gvLabHistoryCart.Items.Count) > 0 && !chkAllInvestigation.Checked)
        {
            foreach (GridDataItem dataItem in gvLabHistoryCart.Items)
            {
                Label lblDiagSampleID = (Label)dataItem.FindControl("lblDiagSampleID");
                Label lblSource = (Label)dataItem.FindControl("lblSource");

                coll.Add(common.myInt(lblDiagSampleID.Text.Trim()));//SampleID INT
                coll.Add(common.myStr(lblSource.Text.Trim()));//SampleID INT

                objXML.Append(common.setXmlTable(ref coll));
            }
        }
        else
        {
            DataTable dt = (DataTable)ViewState["LabData"];

            foreach (DataRow dr in dt.Rows)
            {
                coll.Add(common.myInt(dr["DiagSampleID"]));//SampleID INT
                coll.Add(common.myStr(dr["Source"]));//SampleID INT
                objXML.Append(common.setXmlTable(ref coll));
            }
        }

        BindLab(common.myStr(objXML));
        ViewState["AddLabCart"] = "";
        //Session["SampleId"] = objXML;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }

    private void BindLab(string SampleId)
    {
        #region set view state values

        ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
        ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);

        if (common.myInt(ViewState["RegistrationId"]).Equals(0))
        {
            ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
        }
        if (common.myInt(ViewState["EncounterId"]).Equals(0))
        {
            ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
        }

        #endregion
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
        BaseC.DiagnosisDA fun;

        int HospitalId = Convert.ToInt32(Session["HospitalLocationID"]);
        Int16 UserId = Convert.ToInt16(Session["UserID"]);

        DL_Funs ff = new DL_Funs();
        BindSummary bnotes = new BindSummary(sConString);
        fun = new BaseC.DiagnosisDA(sConString);
        string DoctorId = fun.GetDoctorId(HospitalId, UserId);

        dsTemplateStyle = ObjIcm.GetICMTemplateStyle(Convert.ToInt32(Session["HospitalLocationID"]));
        string sFromDate = "", sToDate = "";

        sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        string sDischarge = common.myStr(Request.QueryString["Discharge"]) != "" ? "D" : "";

        if (common.myStr(Request.QueryString["HC"]).Equals("HC"))
        {
            sDischarge = "D";
        }

        bool btShowAllParameters = common.myStr(Session["ModuleName"]) != "EMR" ? false : true;

        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
        dv.RowFilter = "PageId=1098";
        if (dv.Count > 0)
        {
            drTemplateStyle = dv[0].Row;
        }

        string chk = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsNormalFormatDischargeSummaryLabResult", sConString));
        if (chk.ToUpper().Equals("Y"))
        {
            bnotes.BindLabTestResultNormal(HospitalId, common.myInt(ViewState["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["FacilityId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, SampleId, sDischarge, btShowAllParameters);
        }
        else
        {
            bnotes.BindLabTestResult(HospitalId, common.myInt(ViewState["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["FacilityId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, SampleId, sDischarge, btShowAllParameters, false, common.myStr(Request.QueryString["HC"]));
        }

        hdnLabData.Value = sbTemp.ToString();
    }

    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        //if (e.Text != "")
        //{
        //if (e.Text.Trim().Length > 2)
        //{
        DataTable data = GetData(e.Text);

        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        ddlService.Items.Clear();

        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["ServiceName"];
            // item.Value = data.Rows[i]["ServiceId"].ToString() + "##" + data.Rows[i]["ServiceType"].ToString();
            item.Value = data.Rows[i]["ServiceId"].ToString();// +"##" + data.Rows[i]["ServiceType"].ToString();
            //hdServiceType.Value = ;
            item.Attributes.Add("ServiceName", data.Rows[i]["ServiceName"].ToString());
            item.Attributes.Add("RefServiceCode", data.Rows[i]["RefServiceCode"].ToString());
            this.ddlService.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        // }
        //}
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        string ServiceName = text + "%";
        string strDepartmentType = "";
        //if (common.myStr(ViewState["OP_IP"]) == "O")
        //{
        //    strDepartmentType = "'I','IS','P','HPP','C','O','OPP','CL','RF'";
        //}
        //else
        //{
        strDepartmentType = "'I','IS','P','HPP','C','O','OPP'";
        //}
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
        BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
        //DataSet ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]),
        //    common.myInt(ddlDepartment.SelectedValue),
        //    common.myInt(ddlSubDepartment.SelectedValue),
        //    strDepartmentType, ServiceName);


        DataSet ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]),
            0, common.myInt(ddlSubDepartment.SelectedValue),
            strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]),0,0);

        DataTable data = new DataTable();
        data = ds.Tables[0];
        return data;
    }
    protected void BindSubDepartment()
    {
        try
        {
            ddlSubDepartment.Items.Clear();
            //ddlSubDepartment.Text = "";
            BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = new DataSet();
            ds = objLISMaster.GetSubDepartment(common.myInt(ddlReportFor.SelectedValue), 0);
            ddlSubDepartment.DataSource = ds.Tables[0];
            ddlSubDepartment.DataTextField = "SubName";
            ddlSubDepartment.DataValueField = "SubDeptId";
            ddlSubDepartment.DataBind();
            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlSubDepartment.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    void BindBlankLabCartGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LabNo");
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("Result");
            dt.Columns.Add("ServiceId");
            dt.Columns.Add("Provider");
            dt.Columns.Add("DiagSampleID");
            dt.Columns.Add("OrderDate");
            dt.Columns.Add("Source");
            gvLabHistoryCart.DataSource = dt;
            gvLabHistoryCart.DataBind();
            ViewState["AddLabCart"] = dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvLabHistoryCart_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = (DataTable)ViewState["AddLabCart"];

                DataView dv = dt.DefaultView;
                dv.RowFilter = "DiagSampleID <>" + ((Label)e.Item.FindControl("lblDiagSampleID")).Text;
                dt = dv.ToTable();
                dt.AcceptChanges();
                gvLabHistoryCart.DataSource = dt.Copy();
                gvLabHistoryCart.DataBind();
                ViewState["AddLabCart"] = dt;
                ChangeColour();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
}
