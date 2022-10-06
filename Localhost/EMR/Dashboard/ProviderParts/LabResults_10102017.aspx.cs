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
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class EMR_Dashboard_ProviderParts_LabResults : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }


            ViewState["OrderId"] = 0;
            if (common.myLen(Request.QueryString["OrderId"]) > 0)
            {
                ViewState["OrderId"] = common.myInt(Request.QueryString["OrderId"]);
            }
            BindProvider();
            clearControl();
            tblDate.Visible = false;
            //   ddlProviders.Enabled = false;
            btnFilter_Click(null, null);
            bindRegNoDetails();
        }

        legend();
    }

    private void bindRegNoDetails()
    {
        if (Request.QueryString["RegNo"] != null)
        {
            if (common.myStr(Request.QueryString["RegNo"]) != string.Empty)
            {
                txtSearchCretria.Text = common.myStr(Request.QueryString["RegNo"]);
                txtSearchCretria.Enabled = false;
                ddlSearch.Enabled = false;
            }
            else
            {
                txtSearchCretria.Enabled = true;
                ddlSearch.Enabled = true;
            }
        }
        else
        {
            txtSearchCretria.Enabled = true;
            ddlSearch.Enabled = true;
        }
        btnFilter_Click(null, null);
    }

    protected void BindProvider()
    {
        BaseC.clsLISMaster lis = new BaseC.clsLISMaster(sConString);
        DataTable objDs = new DataTable();
        try
        {

            if (!common.myStr(Request.QueryString["FindPatientDoctorId"]).Equals(string.Empty))
            {
                objDs = lis.getDoctorList(0, "", common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["FacilityId"]), 0);
            }
            else
            {
                objDs = lis.getDoctorList(0, "", common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]));
            }

            if (objDs.Rows.Count > 0)
            {
                ddlProviders.Items.Clear();
                ddlProviders.DataSource = objDs;
                ddlProviders.DataValueField = "DoctorId";
                ddlProviders.DataTextField = "DoctorName";
                ddlProviders.DataBind();

                CheckUserDoctorOrNot();
            }

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            lis = null;
            objDs.Dispose();
        }
    }

    private void CheckUserDoctorOrNot()
    {
        try
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            SqlDataReader objDr = null;

            if (Request.QueryString["FindPatientDoctorId"] != null)
            {
                if (!common.myStr(Request.QueryString["FindPatientDoctorId"]).Equals(string.Empty))
                {
                    // objDr = (SqlDataReader)objEmr.CheckUserDoctorOrNot(common.myInt(Session["HospitalLocationID"]), common.myInt(Request.QueryString["FindPatientDoctorId"]));
                    ddlProviders.Items[0].Selected = false;
                    ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.FindItemByValue(Convert.ToString(Request.QueryString["FindPatientDoctorId"])));
                    ddlProviders.Enabled = false;
                }
                else
                {
                    ddlProviders.Items.Insert(0, new RadComboBoxItem("All", "0"));
                    ddlProviders.SelectedIndex = 0;
                    ddlProviders.Enabled = true;
                }

            }
            else if (Session["UserID"] != null)
            {
                objDr = (SqlDataReader)objEmr.CheckUserDoctorOrNot(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

                if (objDr.Read())
                {
                    if ((Convert.ToString(objDr[0]) != "") && (objDr[0] != null))
                    {
                        ddlProviders.Items[0].Selected = false;
                        ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.FindItemByValue(Convert.ToString(objDr[0])));
                        ddlProviders.Enabled = false;
                    }
                    else
                    {
                        ddlProviders.Items.Insert(0, new RadComboBoxItem("All", "0"));
                        ddlProviders.SelectedIndex = 0;
                        ddlProviders.Enabled = true;
                    }
                }
                objDr.Close();
            }
            txtProviderId.Text = ddlProviders.SelectedValue.ToString();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlProviders_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            //bindTestData();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void legend()
    {
        Label LBL;
        TableRow tr = new TableRow();
        TableCell td;
        int colIdx = 0;

        td = new TableCell();
        LBL = new Label();
        LBL.BorderWidth = Unit.Pixel(1);
        LBL.ID = "LabelStatusColor" + colIdx;
        LBL.BackColor = System.Drawing.Color.LightYellow;
        LBL.SkinID = "label";
        LBL.Width = Unit.Pixel(18);
        LBL.Height = Unit.Pixel(14);

        td.Controls.Add(LBL);
        tr.Cells.Add(td);

        td = new TableCell();
        LBL = new Label();
        LBL.ID = "LabelStatus" + colIdx;
        LBL.Text = "Result Abnormal";
        LBL.Font.Size = 8;

        LBL.SkinID = "label";

        td.Controls.Add(LBL);
        tr.Cells.Add(td);
        colIdx++;


        td = new TableCell();
        LBL = new Label();
        LBL.BorderWidth = Unit.Pixel(1);
        LBL.ID = "LabelStatusColor" + colIdx;
        LBL.BackColor = System.Drawing.Color.LightGreen;
        LBL.SkinID = "label";
        LBL.Width = Unit.Pixel(18);
        LBL.Height = Unit.Pixel(14);

        td.Controls.Add(LBL);
        tr.Cells.Add(td);

        td = new TableCell();
        LBL = new Label();
        LBL.ID = "LabelStatus" + colIdx;
        LBL.Text = "Result Reviewed";
        LBL.Font.Size = 8;

        LBL.SkinID = "label";

        td.Controls.Add(LBL);
        tr.Cells.Add(td);
        colIdx++;

        tblLegend.Rows.Add(tr);
    }

    private void clearControl()
    {
        lblMessage.Text = "";

        txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
        txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        txtToDate.SelectedDate = DateTime.Now;
        txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    public void bindTestData()
    {
        try
        {

            setDate();
            // string sConStrings = "server=akhil;database=paras;uid=sa;pwd=;MultipleActiveResultSets=True; MAX POOL SIZE=300;";
            // BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConStrings);

            DataTable dt = new DataTable();
            BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            int iProviderID = common.myInt(ddlProviders.SelectedValue);

            int pageindex = 0;
            string EncounterNo = "", RegNo = "", Pname = "";
            if (gvResultFinal.Rows.Count > 0)
            {
                pageindex = gvResultFinal.PageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }
            if (ddlSearch.SelectedValue == "IP")
            {
                EncounterNo = txtSearchCretria.Text.Trim();
            }
            else if (ddlSearch.SelectedValue == "PN")
            {
                Pname = txtSearchCretria.Text.Trim();
            }
            else if (ddlSearch.SelectedValue == "RN")
            {
                int SearchRegNo = common.myInt(txtSearchCretria.Text);
                RegNo = common.myStr(SearchRegNo) == "0" ? "" : common.myStr(SearchRegNo);
            }
            lblMessage.Text = "";
            DataSet ds = objval.getPatientLabResultHistoryDash(common.myInt(Session["FacilityID"]), common.myInt(Session["HospitalLocationID"]),
                                    common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate), common.myStr(RegNo),
                                    iProviderID, gvResultFinal.PageSize, pageindex, chkAbnormalValue.Checked, chkCriticalValue.Checked,
                                    common.myInt(0), common.myInt(Session["FacilityID"]), EncounterNo, common.myInt(ddlReviewedStatus.SelectedValue),
                                    common.myStr(Pname), common.myInt(Session["UserId"]), false, false);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataView dvResult = new DataView(ds.Tables[0]);
                    dvResult.RowFilter = "StatusCode='RF'";



                    gvResultFinal.DataSource = dvResult.ToTable();
                    gvResultFinal.DataBind();

                    lblResultChanged.Text = "";
                    lblNew.Text = "";

                    DataView dv = dvResult;
                    dv.RowFilter = "AuditDiagSampleId = 1";
                    lblResultChanged.Text = " Result changed after provisional release : " + dv.Count.ToString();
                    if (common.myInt(dv.Count) > 0)
                        lblResultChanged.CssClass = "blink";
                    else
                        lblResultChanged.CssClass = "noblink";

                    dv.RowFilter = "";
                    dv.RowFilter = "ReviewedStatus = 1";

                    lblNew.Text = " New results : " + (common.myInt(ds.Tables[0].Rows.Count) - common.myInt(dv.Count));
                    if ((common.myInt(ds.Tables[0].Rows.Count) - common.myInt(dv.Count)) > 0)
                        lblNew.CssClass = "blink";
                    else
                        lblNew.CssClass = "noblink";
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        bindTestData();
    }

    protected void btnAttachment_OnClick(Object sender, EventArgs e)
    {
    }

    protected void gvResultFinal_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblresult = (Label)e.Row.FindControl("lblresult");
            Label lblAbnormalValue = (Label)e.Row.FindControl("lblAbnormalValue");
            Label lblCriticalValue = (Label)e.Row.FindControl("lblCriticalValue");
            Label lblPatientNameGrid = (Label)e.Row.FindControl("lblPatientName");
            LinkButton lnkResult = (LinkButton)e.Row.FindControl("lnkResult");
            LinkButton lnkprint = (LinkButton)e.Row.FindControl("lnkprint");

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


        }
    }

    protected void gvResultFinal_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Result")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblAgeGender = (Label)row.FindControl("lblAgeGender");
                Label lblStatusCode = (Label)row.FindControl("lblStatusCode");
                Label lblDiagSampleID = (Label)row.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)row.FindControl("lblServiceId");
                Label lblSource = (Label)row.FindControl("lblSource");
                Label lblServiceName = (Label)row.FindControl("lblServiceName");

                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + lblSource.Text
                                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                                            + "&SERVICEID=" + common.myInt(lblServiceId.Text)
                                            + "&AgeInDays=" + common.myStr(lblAgeGender.Text)
                                            + "&StatusCode=" + common.myStr(lblStatusCode.Text)
                                            + "&ServiceName=" + common.myStr(lblServiceName.Text);

                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 850;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
            }
            if (e.CommandName == "Download")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
                Label lblSource = (Label)row.FindControl("lblSource");
                DataSet ds = new DataSet();
                if (lblSource.Text == "OPD")
                {
                    ds = objval.FillAttachmentDownloadDropdownOP(((Label)row.FindControl("lblDiagSampleID")).Text, "");
                }
                else
                {
                    ds = objval.FillAttachmentDownloadDropdownIP(((Label)row.FindControl("lblDiagSampleID")).Text, "");
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
                          + ((Label)row.FindControl("lblDiagSampleID")).Text + "&SOURCE=" + strSource;

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
            if (e.CommandName == "Print")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblSource = (Label)row.FindControl("lblSource");
                Label lblLabNo = (Label)row.FindControl("lblLabNo");
                Label lblStationId = (Label)row.FindControl("lblStationId");


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
            if (e.CommandName == "Select")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnResultId = (HiddenField)row.FindControl("hdnResultId");
                int ResultId = common.myInt(hdnResultId.Value);
                Label lblSource = (Label)row.FindControl("lblSource");
                string lblDiagSampleID = ((Label)row.FindControl("lblDiagSampleID")).Text;

                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/LabTestResultReview.aspx?ResultId=" + ResultId + "&DiagSampleId=" + lblDiagSampleID + "&Source=" + lblSource.Text;

                RadWindowPopup.Height = 1000;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "OnClientCloseReviewed";
                RadWindowPopup.VisibleOnPageLoad = true;
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
    }

    protected void gvResultFinal_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvResultFinal.PageIndex = e.NewPageIndex;
        bindTestData();

    }

    void setDate()
    {
        try
        {
            tblDate.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":
                    //txtFromDate.SelectedDate = DateTime.Now;
                    //txtToDate.SelectedDate = DateTime.Now;

                    tblDate.Visible = true;
                    break;
                case "LastThreeDays":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
            }
        }
        catch
        {
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDate();
    }

    protected void lnkResult_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkBtn = (LinkButton)sender;
        HiddenField hdnAge = (HiddenField)lnkBtn.FindControl("hdnAge");
        Label lblStatusCode = (Label)lnkBtn.FindControl("lblStatusCode");
        HiddenField hdnDiagSampleId = (HiddenField)lnkBtn.FindControl("hdnDiagSampleId");
        HiddenField hdnServiceId = (HiddenField)lnkBtn.FindControl("hdnServiceId");
        Label lblSource = (Label)lnkBtn.FindControl("lblSource");
        HiddenField hdnServiceName = (HiddenField)lnkBtn.FindControl("hdnServiceName");

        RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/previewResult.aspx?SOURCE=abc"
                                    + "&DIAG_SAMPLEID=" + common.myInt(hdnDiagSampleId.Value)
                                    + "&SERVICEID=" + common.myInt(hdnServiceId.Value)
                                    + "&AgeInDays=" + common.myStr(hdnAge.Value)
                                    + "&StatusCode=a"
                                    + "&ServiceName=" + common.myStr(hdnServiceName.Value);

        RadWindowPopup.Height = 550;
        RadWindowPopup.Width = 850;
        RadWindowPopup.Top = 10;
        RadWindowPopup.Left = 10;
        RadWindowPopup.VisibleOnPageLoad = true;
        RadWindowPopup.Modal = true;
        RadWindowPopup.VisibleStatusbar = false;




    }
}
