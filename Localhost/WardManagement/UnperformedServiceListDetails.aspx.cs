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
using Telerik.Web.UI;

public partial class WardManagement_UnperformedServiceListDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dtpDatefrom.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpDateto.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            ViewState["CurrentOption"] = common.myStr(Request.QueryString["Type"]);
            ViewState["StatusCode"] = common.myStr(Request.QueryString["StatusCode"]);
            dtpDatefrom.SelectedDate = DateTime.Now;
            // dtpDatefrom.SelectedDate = common.myStr(Request.QueryString["FromDate"]) != "" ? common.myDate(Request.QueryString["FromDate"]) : DateTime.Now.AddMonths(-1);
            dtpDatefrom.MaxDate = DateTime.Now;

            dtpDateto.SelectedDate = DateTime.Now;
            dtpDateto.MaxDate = DateTime.Now;

            bindWard();

            tblunperformesServices.Visible = false;
            tbldrugorder.Visible = false;
            tblnondrugorder.Visible = false;
            divStopMedication.Visible = false;

            switch (common.myStr(ViewState["CurrentOption"]).ToUpper())
            {
                case "UNPERFORMED":
                    tblunperformesServices.Visible = true;
                    this.Page.Title = "Unperformed Services";
                    break;
                case "DRUGORDER":
                    tbldrugorder.Visible = true;
                    this.Page.Title = "Drug Order Details";
                    break;
                case "NONDRUGORDER":
                    tblnondrugorder.Visible = true;
                    this.Page.Title = "Non Drug Order Details";
                    break;
                case "G":
                    tblunperformesServices.Visible = true;
                    this.Page.Title = "Unperformed Services";
                    break;
                case "X":
                    tblunperformesServices.Visible = true;
                    this.Page.Title = "Pending for Acknowledgement";
                    break;
                case "STOPMEDICATION":
                    divStopMedication.Visible = true;
                    this.Page.Title = "Stop Medication";
                    break;
            }
            ddlWard.Visible = false;
            txtSearchRegNo.Visible = false;
            txtSearch.Visible = false;

            switch (common.myStr(Request.QueryString["SearchCriteria"]))
            {
                case "R":
                    txtSearchRegNo.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearchRegNo.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "ENC":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "N":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
            }

            if (common.myLen(Request.QueryString["WardId"]) > 0)
            {
                ddlSearchCriteria.SelectedIndex = ddlSearchCriteria.Items.IndexOf(ddlSearchCriteria.Items.FindItemByValue("W"));
                ddlSearchCriteria_SelectedIndexChanged(null, null);
                ddlWard.SelectedIndex = ddlWard.Items.IndexOf(ddlWard.Items.FindItemByValue(common.myInt(Request.QueryString["WardId"]).ToString()));
            }

            fetchData();
        }

    }

    private void bindWard()
    {
        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(Session["LoginEmployeeType"]).Equals("N") && common.myInt(Session["EmployeeId"]) > 0)
            {
                BaseC.WardManagement objWM = new BaseC.WardManagement();

                ds = objWM.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
            }
            else
            {
                BaseC.ATD objadt = new BaseC.ATD(sConString);
                ds = objadt.getWardMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            }

            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardID";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlWard.SelectedIndex = 0;
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

    protected void gvUnacknowledgedServices_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUnacknowledgedServices.PageIndex = e.NewPageIndex;
        BindgvUnacknowledgedServices();
    }

    protected void gvIPPharmacyStore_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIPPharmacyStore.PageIndex = e.NewPageIndex;
        BindgvIPPharmacyStore();
    }

    protected void gvNonDrugOrder_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonDrugOrder.PageIndex = e.NewPageIndex;
        BindgvNonDrugOrder();
    }

    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlWard.Visible = false;
        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {
            case "W":
                ddlWard.Visible = true;
                break;
            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "ENC":
                txtSearch.Visible = true;
                break;
            case "P":
                txtSearch.Visible = true;
                break;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            fetchData();
            tblDrugOrderDetails.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void fetchData()
    {
        try
        {
            switch (common.myStr(ViewState["CurrentOption"]).ToUpper())
            {
                case "UNPERFORMED":
                    BindgvUnacknowledgedServices();
                    break;
                case "DRUGORDER":
                    BindgvIPPharmacyStore();
                    break;
                case "NONDRUGORDER":
                    BindgvNonDrugOrder();
                    break;
                case "G":
                    BindgvUnacknowledgedServices();
                    break;
                case "X":
                    BindgvUnacknowledgedServices();
                    break;
                case "STOPMEDICATION":
                    StopMedicationNotification();
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
        }
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlSearchCriteria.SelectedIndex = 0;
        txtSearchRegNo.Text = string.Empty;
        txtSearch.Text = string.Empty;

        dtpDatefrom.SelectedDate = DateTime.Now;
        dtpDatefrom.MaxDate = DateTime.Now;

        dtpDateto.SelectedDate = DateTime.Now;
        dtpDateto.MaxDate = DateTime.Now;

        fetchData();
    }

    private void BindgvUnacknowledgedServices()
    {
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet ds = new DataSet();

        int WardId = 0;
        int RegistrationNo = 0;
        string EncounterNo = string.Empty;
        string PatientName = string.Empty;

        try
        {
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "W":
                    WardId = common.myInt(ddlWard.SelectedValue);
                    break;
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }
            if (ViewState["StatusCode"].Equals(string.Empty))
            {

                ds = objWM.getUnacknowledgeServices(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                            Convert.ToDateTime(dtpDatefrom.SelectedDate).ToString("yyyy-MM-dd"),
                                            Convert.ToDateTime(dtpDateto.SelectedDate).ToString("yyyy-MM-dd"),
                                            WardId, RegistrationNo, EncounterNo, PatientName);
            }
            else
            {
                ds = objWM.getUnacknowledgeServices(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                            Convert.ToDateTime(dtpDatefrom.SelectedDate).ToString("yyyy-MM-dd"),
                                            Convert.ToDateTime(dtpDateto.SelectedDate).ToString("yyyy-MM-dd"),
                                            WardId, RegistrationNo, EncounterNo, PatientName,
                                            0, common.myStr(ViewState["CurrentOption"]), common.myStr(ViewState["StatusCode"]));
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                lbitotalRecord.Text = "(0)";
            }
            else
            {
                lbitotalRecord.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            }

            gvUnacknowledgedServices.DataSource = ds.Tables[0];
            gvUnacknowledgedServices.DataBind();

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[1].Rows[0]["NoOfPatient"]) + ")";
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
            objWM = null;
        }
    }

    private void BindgvIPPharmacyStore()
    {
        BaseC.WardManagement objWM = new BaseC.WardManagement();

        string storeId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
        DataSet ds = new DataSet();

        int WardId = 0;
        int RegistrationNo = 0;
        string EncounterNo = string.Empty;
        string PatientName = string.Empty;

        try
        {
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "W":
                    WardId = common.myInt(ddlWard.SelectedValue);
                    break;
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            ds = objWM.getIPPatientRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterNo, 0, "A",
                                    Convert.ToDateTime(dtpDatefrom.SelectedDate).ToString("yyyy-MM-dd"),
                                    Convert.ToDateTime(dtpDateto.SelectedDate).ToString("yyyy-MM-dd"),
                                    WardId, RegistrationNo, PatientName);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                lbitotalRecord.Text = "(0)";
            }
            else
            {
                lbitotalRecord.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            }

            gvIPPharmacyStore.DataSource = ds.Tables[0];
            gvIPPharmacyStore.DataBind();

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[1].Rows[0]["NoOfPatient"]) + ")";
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
            objWM = null;
        }
    }

    private void BindgvNonDrugOrder()
    {
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet ds = new DataSet();

        int WardId = 0;
        int RegistrationNo = 0;
        string EncounterNo = string.Empty;
        string PatientName = string.Empty;

        try
        {
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "W":
                    WardId = common.myInt(ddlWard.SelectedValue);
                    break;
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            ds = objWM.NonDrugOrderForWard((common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityId"]),
                                    Convert.ToDateTime(dtpDatefrom.SelectedDate).ToString("yyyy-MM-dd"),
                                    Convert.ToDateTime(dtpDateto.SelectedDate).ToString("yyyy-MM-dd"),
                                    WardId, RegistrationNo, EncounterNo, PatientName);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                lbitotalRecord.Text = "(0)";
            }
            else
            {
                lbitotalRecord.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            }

            gvNonDrugOrder.DataSource = ds.Tables[0];
            gvNonDrugOrder.DataBind();

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[1].Rows[0]["NoOfPatient"]) + ")";
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
            objWM = null;
        }
    }

    protected void bindDetailsData(int IndentId, int EncounterId)
    {
        BaseC.WardManagement objWard = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        try
        {
            ds = objWard.GetDrugOrderDetails((common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityId"]), IndentId, EncounterId);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "No Records Found";
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
            objWard = null;
            ds.Dispose();
        }
    }
    protected void gvIPPharmacyStore_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Details")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int EncounterId = common.myInt(((HiddenField)row.FindControl("hdnEncounterId")).Value.ToString());
                bindDetailsData(common.myInt(e.CommandArgument.ToString()), EncounterId);
                tblDrugOrderDetails.Visible = true;
                foreach (GridViewRow rownew in gvIPPharmacyStore.Rows)
                {
                    rownew.BackColor = System.Drawing.Color.FromName("White");
                }
                row.BackColor = System.Drawing.Color.FromName("LightSkyBlue");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow row = e.Row;
            HiddenField quantity = (HiddenField)row.FindControl("hdnQuantity");

            int issueQuantity = common.myInt(quantity.Value);
            if (issueQuantity > 0)
                row.BackColor = System.Drawing.Color.FromName("DarkSeaGreen");
            else
                row.BackColor = System.Drawing.Color.FromName("#FFFBC7");
        }
    }

    protected void gvUnacknowledgedServices_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDateToBePerformed = (Label)e.Row.FindControl("lblDateToBePerformed");
            Label lblSampleStatus = (Label)e.Row.FindControl("lblSampleStatus");
            HiddenField hdnLabSampleNotes = (HiddenField)e.Row.FindControl("hdnLabSampleNotes");
            ImageButton ibtnForNotes = (ImageButton)e.Row.FindControl("ibtnForNotes");
            if (common.myInt(hdnLabSampleNotes.Value) > 0)
            {
                ibtnForNotes.Visible = true;
            }
            if (DateTime.Compare(common.myDate(lblDateToBePerformed.Text), common.myDate(System.DateTime.Now)) > 0)
            {
                e.Row.BackColor = System.Drawing.Color.Aqua;
            }
            if (common.myStr(lblSampleStatus.Text).Equals("Sample Not Collected"))
            {
                e.Row.FindControl("lnkRegistrationNo").Visible = true;
                e.Row.FindControl("lblRegistrationNo").Visible = false;
            }
            else
            {
                e.Row.FindControl("lnkRegistrationNo").Visible = false;
                e.Row.FindControl("lblRegistrationNo").Visible = true;
            }
        }
    }
    protected void gvUnacknowledgedServices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Collect")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                string EncounterDate = common.myDate(((HiddenField)row.FindControl("hdnEncounterDate")).Value.ToString()).ToString("dd/MM/yyyy");
                int RegistrationNo = common.myInt(((LinkButton)row.FindControl("lnkRegistrationNo")).Text.ToString());
                string EncounterNo = common.myStr(((Label)row.FindControl("lblEncounterNo")).Text.ToString());
                RadWindow RadWindow1 = (RadWindow)this.Page.FindControl("RadWindow1");
                RadWindow1.NavigateUrl = "/LIS/Phlebotomy/Phlebotomy.aspx?PT=COLL&Mpg=P585&PageFrom=ward&UHID=" + RegistrationNo + "&DT=" + EncounterDate + "&IpNo=" + EncounterNo;
                RadWindow1.Height = 650;
                RadWindow1.Width = 1100;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "OnCollectClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else if (e.CommandName == "sel")
            {
                string Flag = "LIS";
                string Source = "IPD";
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton ibtnForNotes = (ImageButton)row.FindControl("ibtnForNotes");

                //Label lblRegistrationNo = (Label)e.FindControl("lblRegistrationNo");
                //Label lblEncounterno = (Label)e.FindControl("lblEncounterno");

                Label lblRegistrationNo = (Label)row.FindControl("lblRegistrationNo");
                Label lblEncounterno = (Label)row.FindControl("lblEncounterno");
                RadWindow1.NavigateUrl = "~/LIS/Format/LISNotes.aspx?MD=" + Flag +
                                           "&SOURCE=" + common.myStr(Source) +
                                           "&eno=" + common.myStr(lblEncounterno.Text) +
                                           "&RegNo=" + common.myStr(lblRegistrationNo.Text) +
                                           "&LABNO=" + common.myStr(ibtnForNotes.CommandArgument) +
                                           "&Servicedetails=" + 1 +
                                           "&OrderId=" + 0;

                RadWindow1.Height = 580;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnForNotes_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void StopMedicationNotification()
    {
        BaseC.WardManagement objWard = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        try
        {
            int WardId = 0;
            string RegistrationNo = string.Empty;
            string EncounterNo = string.Empty;
            string PatientName = string.Empty;


            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "W":
                    WardId = common.myInt(ddlWard.SelectedValue);
                    break;
                case "R":
                    RegistrationNo = common.myStr(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            gvStopMedication.DataSource = null;
            gvStopMedication.DataBind();

            ds = objWard.GetStopMedicationNotification((common.myInt(Session["HospitalLocationID"])), common.myInt(Session["FacilityId"]),
                            RegistrationNo, EncounterNo, PatientName, WardId, common.myDate(dtpDatefrom.SelectedDate).ToString("yyyy-MM-dd"),
                            common.myDate(dtpDateto.SelectedDate).ToString("yyyy-MM-dd"));

            if (ds.Tables[0].Rows.Count == 0)
            {
                lbitotalRecord.Text = "(0)";
            }
            else
            {
                lbitotalRecord.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvStopMedication.DataSource = ds.Tables[0];
                gvStopMedication.DataBind();
                lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
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
            objWard = null;
            ds.Dispose();
        }
    }

    protected void gvStopMedication_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvStopMedication.PageIndex = e.NewPageIndex;
        StopMedicationNotification();
    }
}


