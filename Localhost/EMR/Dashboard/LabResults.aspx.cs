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
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
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
        try
        {
            if (!IsPostBack)
            {
                Session["GridCheckedItems"] = null;
                Session["GridCheckedIds"] = null;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }

                if (common.myInt(Request.QueryString["mainRegNo"]) > 0)
                {
                    txtSearchCretria.Text = common.myStr(Request.QueryString["mainRegNo"]);
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
                bindTestData();
                bindRegNoDetails();
            }
            legend();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void SaveGridCheckedItems(GridView grv, string chkControlName, string hdnUniqueId)
    {
        ArrayList arr = new ArrayList();
        ArrayList arr1 = new ArrayList();
        int index = -1;
        if (Session["GridCheckedItems"] != null)
            arr = Session["GridCheckedItems"] as ArrayList;
        if (Session["GridCheckedIds"] != null)
            arr1 = Session["GridCheckedIds"] as ArrayList;
        string resultid = "";
        if (Session["strResultid"] != null)
            resultid = common.myStr(Session["strResultid"]);
        string diagsampleid = "";
        if (Session["strDiagsampleid"] != null)
            diagsampleid = common.myStr(Session["strDiagsampleid"]);
        StringBuilder strXML = new StringBuilder();
        string strResultReviewDiagSampleIds = "";
        if (Session["ResultReviewDiagSampleIds"] != null)
            strResultReviewDiagSampleIds = common.myStr(Session["strDiagsampleid"]);
        foreach (GridViewRow gvrow in grv.Rows)
        {
            index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);
            bool result = ((CheckBox)gvrow.FindControl(chkControlName)).Checked;
            int hdnId = common.myInt(((HiddenField)gvrow.FindControl(hdnUniqueId)).Value);
            if (result)
            {
                if (!arr.Contains(index))
                {
                    arr.Add(index);
                }
                else
                        if (arr.Contains(index))
                    arr.Remove(index);
                if (!arr1.Contains(hdnId))
                {
                    arr1.Add(hdnId);
                }
                else
                {
                    if (arr1.Contains(hdnId))
                        arr1.Remove(hdnId);
                }

                CheckBox chkselect = (CheckBox)gvResultFinal.Rows[index].FindControl("chkselect");
                if (chkselect.Checked && chkselect.Visible)
                {
                    ArrayList coll = new ArrayList();
                    Label lblSource = (Label)gvResultFinal.Rows[index].FindControl("lblSource");

                    HiddenField hdnResultId = (HiddenField)gvResultFinal.Rows[index].FindControl("hdnResultId");
                    int ResultId = common.myInt(hdnResultId.Value);
                    resultid = resultid + "," + ResultId.ToString();

                    string lblDiagSampleID = ((Label)gvResultFinal.Rows[index].FindControl("lblDiagSampleID")).Text;
                    Label lbl = (Label)gvResultFinal.Rows[index].FindControl("lblSource");
                    diagsampleid = diagsampleid + "," + lblDiagSampleID;

                    coll.Add(common.myStr(lblDiagSampleID));
                    coll.Add(common.myStr(lblSource.Text));
                    strXML.Append(common.setXmlTable(ref coll));
                }

            }
        }

        Session["strResultid"] = resultid;
        Session["strDiagsampleid"] = resultid;
        Session["ResultReviewDiagSampleIds"] = strResultReviewDiagSampleIds + strXML.ToString();

















        Session["GridCheckedItems"] = arr;
        Session["GridCheckedIds"] = arr1;
    }

    public void SetGridCheckedItems(GridView grv, string chkControlName)
    {
        if (Session["GridCheckedItems"] == null)
            return;
        ArrayList arr = new ArrayList();
        arr = Session["GridCheckedItems"] as ArrayList;
        foreach (GridViewRow gvrow in grv.Rows)
        {
            int index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);

            if (arr.Contains(index))
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = true;
            else
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = false;
        }

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

    //protected void BindProvider()
    //{
    //    Hashtable hshInput = new Hashtable();
    //    DataSet objDs = new DataSet();
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    try
    //    {

    //        hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
    //        objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);
    //        if (objDs.Tables[0].Rows.Count > 0)
    //        {
    //            ddlProviders.Items.Clear();
    //            ddlProviders.DataSource = objDs;
    //            ddlProviders.DataValueField = "DoctorId";
    //            ddlProviders.DataTextField = "DoctorName";
    //            ddlProviders.DataBind();

    //            CheckUserDoctorOrNot();
    //        }
    //        if (common.myInt(Session["EmployeeId"]) != 0)
    //        {
    //            ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.Items.FindItemByValue(common.myStr(Session["EmployeeId"])));
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        hshInput = null;
    //        objDs.Dispose();
    //        dl = null;
    //    }
    //}

    protected void BindProvider()
    {
        BaseC.clsLISMaster lis = new BaseC.clsLISMaster(sConString);
        DataTable objDs = new DataTable();
        try
        {

            if (!common.myStr(Request.QueryString["FindPatientDoctorId"]).Equals(string.Empty) || !common.myStr(Request.QueryString["ShowAllProvider"]).Equals(string.Empty))
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
        BaseC.EMR objEmr = new BaseC.EMR(sConString);
        SqlDataReader objDr = null;
        try
        {
           

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
                        // ddlProviders.Enabled = false;
                    }
                    else
                    {
                        ddlProviders.Items.Insert(0, new RadComboBoxItem("All", "0"));
                        ddlProviders.SelectedIndex = 0;
                        //ddlProviders.Enabled = true;
                    }
                }
                objDr.Close();
                objDr.Dispose();
            }
            txtProviderId.Text = ddlProviders.SelectedValue.ToString();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEmr = null;
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void legend()
    {
        Label LBL = new Label();
        TableRow tr = new TableRow();
        TableCell td = new TableCell();
        int colIdx = 0;

        try
        {

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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            LBL.Dispose();
            tr.Dispose();
            td.Dispose();
        }
    }

    private void clearControl()
    {
        try
        {
            lblMessage.Text = "";

            txtFromDate.SelectedDate = (Request.QueryString["EncounterDate"] != null) ? common.myDate(Request.QueryString["EncounterDate"]) : DateTime.Now.AddMonths(-3);

            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            txtToDate.SelectedDate = DateTime.Now;
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    public void bindTestData()
    {
        DataTable dt = new DataTable();
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataView dv = new DataView();
        DataSet ds = new DataSet();
        try
        {
            setDate();
            // string sConStrings = "server=akhil;database=paras;uid=sa;pwd=;MultipleActiveResultSets=True; MAX POOL SIZE=300;";
            // BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConStrings);


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
            if (ddlSearch.Visible)
            {
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
            }

            if (common.myInt(Request.QueryString["mainRegNo"]) > 0)
            {
                RegNo = common.myStr(Request.QueryString["mainRegNo"]);
            }

            if (common.myInt(RegNo).Equals(0))
            {
                RegNo = string.Empty;
            }

            lblMessage.Text = "";
            //gvResultFinal.DataSource = null;
            //gvResultFinal.DataBind();

            ds = objval.getPatientLabResultHistoryDash(common.myInt(Session["FacilityID"]),
                                   common.myInt(Session["HospitalLocationID"]),
                                   common.myDate(txtFromDate.SelectedDate),
                                   common.myDate(txtToDate.SelectedDate),
                                   common.myStr(RegNo),
                                   iProviderID,
                                   gvResultFinal.PageSize,
                                   pageindex,
                                   chkAbnormalValue.Checked,
                                   chkCriticalValue.Checked,
                                   common.myInt(0),
                                   common.myInt(Session["FacilityID"]),
                                   EncounterNo, common.myInt(ddlReviewedStatus.SelectedValue),
                                   common.myStr(Pname), common.myInt(Session["UserId"]),
                                   chkL.Checked,
                                   chkH.Checked);

            gvResultFinal.DataSource = ds.Tables[0];
            gvResultFinal.DataBind();

            lblResultChanged.Text = "";
            lblNew.Text = "";

            dv = ds.Tables[0].DefaultView;
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dv.Dispose();
            ds.Dispose();
            dt.Dispose();
            objval = null;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            bindTestData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnAttachment_OnClick(Object sender, EventArgs e)
    {
    }

    protected void gvResultFinal_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblresult = (Label)e.Row.FindControl("lblresult");
                Label lblAbnormalValue = (Label)e.Row.FindControl("lblAbnormalValue");
                Label lblCriticalValue = (Label)e.Row.FindControl("lblCriticalValue");
                Label lblPatientNameGrid = (Label)e.Row.FindControl("lblPatientName");
                LinkButton lnkResult = (LinkButton)e.Row.FindControl("lnkResult");
                LinkButton lnkprint = (LinkButton)e.Row.FindControl("lnkprint");
                HiddenField hdnFinalizeResultCount = (HiddenField)e.Row.FindControl("hdnFinalizeResultCount");
                HiddenField hdnPrescribedTest = (HiddenField)e.Row.FindControl("hdnPrescribedTest");
                Label lblLabNo = (Label)e.Row.FindControl("lblLabNo");
                if (common.myInt(hdnPrescribedTest.Value) > common.myInt(hdnFinalizeResultCount.Value))
                {
                    e.Row.ToolTip = "Some Result still pending for finalized." + Environment.NewLine + "Ordered Test : " + hdnPrescribedTest.Value + "." + Environment.NewLine + "Finalize Test : " + hdnFinalizeResultCount.Value.ToString() + Environment.NewLine + "Pending Test : " + (common.myInt(hdnPrescribedTest.Value) - common.myInt(hdnFinalizeResultCount.Value)).ToString();
                    e.Row.BackColor = System.Drawing.Color.PeachPuff;
                }
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
                    lblresult.Visible = false;
                    lnkResult.Visible = true;
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                RadWindowPopup.OnClientClose = "OnClientCloseReviewed";
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
                        Session["FileName"] = path;// uristring;
                        string uristring = "/EMR/viewDocs.aspx";
                        RadWindowPopup.NavigateUrl = uristring.ToString();//"/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
                        RadWindowPopup.Height = 600;
                        RadWindowPopup.Width = 1000;
                        RadWindowPopup.Top = 40;
                        RadWindowPopup.Left = 100;
                        //RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
                        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindowPopup.Modal = true;
                        RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
                        RadWindowPopup.VisibleStatusbar = false;
                        //System.IO.FileInfo file = new System.IO.FileInfo(path);
                        //if (file.Exists)
                        //{
                        //    Response.Clear();
                        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        //    Response.AddHeader("Content-Length", file.Length.ToString());
                        //    Response.ContentType = "application/octet-stream";
                        //    Response.WriteFile(file.FullName);
                        //    Response.End();
                        //}
                        //else
                        //{
                        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //    lblMessage.Text = "File does not exist...";
                        //}
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
                Label lblServiceId = (Label)row.FindControl("lblServiceId");



                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE="
                    + common.myStr(lblSource.Text)
                    + "&LABNO=" + common.myInt(lblLabNo.Text)
                    + "&StationId=" + common.myInt(lblStationId.Text)
                     + "&ServiceIds=" + common.myInt(lblServiceId.Text)
                    ;

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

                string lblDiagSampleID = ((Label)row.FindControl("lblDiagSampleID")).Text;
                Label lblSource = (Label)row.FindControl("lblSource");

                HiddenField hdnFinalizeResultCount = (HiddenField)row.FindControl("hdnFinalizeResultCount");
                HiddenField hdnPrescribedTest = (HiddenField)row.FindControl("hdnPrescribedTest");

                ArrayList coll = new ArrayList();
                StringBuilder strXML = new StringBuilder();

                coll.Add(common.myStr(lblDiagSampleID));
                coll.Add(common.myStr(lblSource.Text));
                strXML.Append(common.setXmlTable(ref coll));

                if (common.myInt(hdnResultId.Value).Equals("0"))
                {
                    Alert.ShowAjaxMsg("Please Select Investigation(s) ", this.Page);
                    return;
                }

                Session["ResultReviewDiagSampleIds"] = strXML.ToString();

                //RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/LabTestResultReview.aspx?ResultId=" + ResultId + "&PrescribedTest=" + hdnPrescribedTest.Value + "&FinalizeResultCount=" + hdnFinalizeResultCount.Value + "&DiagSampleId=" + lblDiagSampleID + "&Source=" + opip;
                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/LabTestResultReview.aspx?ResultId=" + common.myInt(hdnResultId.Value) +
                                            "&DiagSampleId=" + lblDiagSampleID + "&Source=B";

                RadWindowPopup.Height = 400;
                RadWindowPopup.Width = 550;
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
        try
        {
            SaveGridCheckedItems(gvResultFinal, "chkselect", "hdnResultId");
            gvResultFinal.PageIndex = e.NewPageIndex;
            bindTestData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

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
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void chkselectAll_ChckChanged(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < gvResultFinal.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvResultFinal.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    chk.Checked = false;
                }
                else { chk.Checked = true; }
            }
            if (chkselectAll.Text == "De-Select All")
            {
                chkselectAll.Text = "Select All";
            }
            else
            {
                chkselectAll.Text = "De-Select All";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            setDate();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void ReviewAll(object sender, EventArgs e)
    {
        ArrayList coll = new ArrayList();
        StringBuilder strXML = new StringBuilder();

        string resultid = "0";
        string diagsampleid = "0";

        try
        {
            int LoopMaxCount = 0;
            if(Session["GridCheckedItems"] != null)
            for (int i = 0; i < gvResultFinal.Rows.Count; i++)
            {
                CheckBox chkselect = (CheckBox)gvResultFinal.Rows[i].FindControl("chkselect");
                if (chkselect.Checked && chkselect.Visible)
                {
                    coll = new ArrayList();
                    Label lblSource = (Label)gvResultFinal.Rows[i].FindControl("lblSource");

                    HiddenField hdnResultId = (HiddenField)gvResultFinal.Rows[i].FindControl("hdnResultId");
                    int ResultId = common.myInt(hdnResultId.Value);
                    resultid = resultid + "," + ResultId.ToString();

                    string lblDiagSampleID = ((Label)gvResultFinal.Rows[i].FindControl("lblDiagSampleID")).Text;
                    Label lbl = (Label)gvResultFinal.Rows[i].FindControl("lblSource");
                    diagsampleid = diagsampleid + "," + lblDiagSampleID;

                    coll.Add(common.myStr(lblDiagSampleID));
                    coll.Add(common.myStr(lblSource.Text));
                    strXML.Append(common.setXmlTable(ref coll));
                }


            }

            if (resultid.Equals("0"))
            {
                Alert.ShowAjaxMsg("Please Select Investigation(s) ", this.Page);
                return;
            }

            Session["ResultReviewDiagSampleIds"] = strXML.ToString();

            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/LabTestResultReview.aspx?ResultId=" + resultid + "&DiagSampleId=" + diagsampleid + "&Source=B";
            RadWindowPopup.Height = 400;
            RadWindowPopup.Width = 550;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.OnClientClose = "OnClientCloseReviewed";
            RadWindowPopup.VisibleOnPageLoad = true;
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
            coll = null;
            strXML = null;
            resultid = null;
            diagsampleid = null;



        }

    }
    protected void lnkResult_OnClick(object sender, EventArgs e)
    {
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
}
