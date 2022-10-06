using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;

public partial class EMR_Assessment_ProvisionalDiagnosisHistory : System.Web.UI.Page
{
    #region Page level variable declration section
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData Parse = new BaseC.ParseData();

    BaseC.DiagnosisDA objdiag;
    DataSet ds;

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
        try
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

            //   btnSave.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }



    private void BindDrpProvider()
    {
        try
        {
            ds = new DataSet();
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            ds = objEmr.GetEMRDoctorPatientwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));
            ddlProvider.Items.Clear();
            ddlProvider.DataSource = ds;
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataBind();
            ddlProvider.Items.Insert(0, new RadComboBoxItem("Select All", "0")); //ListItem("","0")
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

    protected void BindDrpFacility()
    {
        Hashtable hshInputs = new Hashtable();
        DataSet objDs1 = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            hshInputs.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            hshInputs.Add("@intUserId", Session["UserID"]);
            hshInputs.Add("@intGroupId", Session["GroupID"]);
            hshInputs.Add("@chvFacilityType", "O");
            objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", hshInputs);


            ddlFacility.Items.Clear();
            ddlFacility.DataSource = objDs1;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("Select All", "0"));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            hshInputs = null;
            objDs1.Dispose();
            dl = null;
        }
    }

    protected void BindDrpStatus()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = new DataSet();

        try
        {
            string strSql = "Select StatusId, Description From EMRDiagnosisStatusMaster Where Active=1";
            objDs = dl.FillDataSet(CommandType.Text, strSql);

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
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
            objDs.Dispose();
        }
    }

    protected void BindgvDiagnosisHistory()
    {
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        DataTable dt = new DataTable();

        try
        {
            ds = new DataSet();
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
                    ds = objDiag.GetPatientProvisionalDiagnosisHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(ddlProvider.SelectedValue), common.myStr(ddlSource.SelectedValue), FrmDate.ToString("yyyy/MM/dd"), ToDate.ToString("yyyy/MM/dd"), common.myStr(Session["UserID"]));
                    //(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(ddlProvider.SelectedValue), 0);     //(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), 0, common.myInt(ddlProvider.SelectedValue), 0, 0, "", FrmDate.ToString("yyyy/MM/dd"), ToDate.ToString("yyyy/MM/dd"), "%%", false, common.myInt(ddlStatus.SelectedValue), common.myStr(ddlSource.SelectedValue), false, 0);

                }
                else
                {
                    // ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), 0, common.myInt(ddlProvider.SelectedValue), 0, 0, common.myStr(ddldateRange.SelectedValue), "", "", "%%", false, common.myInt(ddlStatus.SelectedValue), common.myStr(ddlSource.SelectedValue), true , 0);
                    ds = objDiag.GetPatientProvisionalDiagnosisHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(ddlProvider.SelectedValue), common.myStr(ddlSource.SelectedValue), string.Empty, string.Empty, common.myStr(Session["UserID"]));

                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
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
            objException.HandleException(Ex);
        }
        finally
        {
            objDiag = null;
            objDl = null;
            AuditCA = null;
            ds.Dispose();
            dt.Dispose();
        }
    }

    protected void gvDiagnosisHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvPatientHistory_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
            //ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
            //Session["RegistrationID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();

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
            objException.HandleException(Ex);
        }
    }

    protected void btnClearFilter_Click(object o, EventArgs e)
    {
        try
        {
            ddlProvider.SelectedIndex = 0;
            ddlFacility.SelectedIndex = 0;
            ddldateRange.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlSource.SelectedIndex = 0;
            //chkChronics.Checked = false;

            dtpfromDate.DateInput.Text = "";
            dtpfromDate.SelectedDate = null;
            dtpToDate.DateInput.Text = "";
            dtpToDate.SelectedDate = null;
            ddldateRange_OnSelectedIndexChanged(o, e);
            BindgvDiagnosisHistory();
            lblMessage.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindgvDiagnosisHistory();
    }

    private DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        try
        {
           
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }

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
            objException.HandleException(Ex);
        }
    }

    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objXML = new StringBuilder();
        try
        {

            string strsave = "";
            //StringBuilder objXML = new StringBuilder();
            Label lblIcdId = null;

            foreach (GridDataItem item in gvDiagnosisHistory.MasterTableView.Items)
            {
                CheckBox chkRow = (CheckBox)item["View"].FindControl("CheckBox1");

                if (chkRow.Checked == true)
                {

                    Label lblId = (Label)item.FindControl("lblId");
                    LinkButton lblDescription = (LinkButton)item.FindControl("lblDescription");


                    string doctorid = null;
                    if (common.myStr(Session["DoctorID"]) != "")
                    {
                        doctorid = common.myStr(Session["DoctorID"]);
                    }
                    objXML.Append("<Table1><c1>");
                    objXML.Append(lblId.Text );
                    objXML.Append("</c1></Table1>");

                    //strsave = objDiag.EMRSavePatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myStr(txtProvisionalDiagnosis.Text).Trim(), common.myInt(ddlDiagnosisSearchCodes.SelectedValue), common.myInt(Session["UserId"]), common.myInt(ViewState["_ID"]), intProviderId, dtChangeDate);
                    //(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), doctorid, common.myInt(ViewState["PageId"]), objXML.ToString(), "", common.myInt(Session["UserId"]), false, false, 0);

                }
            }
//            @intHospitalLocationId SMALLINT,
//@intRegistrationId INT, 
//@intEncounterId INT,
//@intEncodedBy INT,
//@chvErrorStatus VARCHAR(200) OUTPUT,
//@xmlID XML

            strsave = objDiag.EMRCopyPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),  common.myInt(Session["UserId"]), objXML.ToString());



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
            //}
            //else
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Please select the diagnosis to add...";
            //    return;
            //}

            BindgvDiagnosisHistory();
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
            dl = null;
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("/EMR/Assessment/ProvisionalDiagnosis.aspx?Diag=Tx&From=POPUP", false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void lnkbtnProblem_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkBtn = (LinkButton)sender;
            string lnkbtnProblem = common.myStr(((LinkButton)lnkBtn.FindControl("lblDescription")).Text);
            Label lblId = (Label)lnkBtn.FindControl("lblId");
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
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void gvDiagnosisHistory_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        //gvDiagnosisHistory.PageIndexChanged .PageIndex = e.NewPageIndex;
        //gvDiagnosisHistory.PageIndexChanged = e.NewPageIndex;
        BindgvDiagnosisHistory();
    }
}