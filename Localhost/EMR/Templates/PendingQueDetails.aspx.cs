using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Pharmacy_SaleIssue_PendingQueDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMRBilling objVal;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridEncounter : byte
    {
        Select = 2,
        OPIP = 3,
        RegistrationNo = 4,
        EncounterNo = 5,
        Name = 6,
        AgeGender = 7,
        DoctorName = 8,
        CurrentBedNo = 9,
        EncDate = 10,
        DischargeStatus = 11,
        CompanyName = 13,
        PhoneHome = 14,
        MobileNo = 15,
        DOB = 16,
        Address = 17,
        REGID = 18,
        ENCID = 19,
        CompanyCode = 20,
        InsuranceCode = 21,
        CardId = 22,
        RowNo = 23,
        EncounterStatus = 12
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (!string.IsNullOrEmpty(commonLabelSetting.cFont))
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }
                dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

                hdnRegistrationId.Value = string.Empty;
                hdnRegistrationNo.Value = string.Empty;
                hdnEncounterId.Value = string.Empty;
                hdnEncounterNo.Value = string.Empty;
                hdnCompanyCode.Value = string.Empty;
                hdnInsuranceCode.Value = string.Empty;
                hdnCardId.Value = string.Empty;
                hdnEncounterDate.Value = string.Empty;
                hdnAgeGender.Value = string.Empty;
                hdnPhoneHome.Value = string.Empty;
                hdnMobileNo.Value = string.Empty;
                hdnPatientName.Value = string.Empty;
                hdnDOB.Value = string.Empty;
                hdnAddress.Value = string.Empty;
                hdnFacilityId.Value = string.Empty;
                bindControl();
                if (common.myInt(Request.QueryString["RegEnc"]).Equals(1))
                {
                    rdoRegEnc.SelectedValue = "1";
                    rdoRegEnc.Items[1].Text = "Admission";
                    gvEncounter.PageSize = 200;
                }
                else if (common.myInt(Request.QueryString["RegEnc"]).Equals(2))
                {
                    rdoRegEnc.SelectedValue = "2";
                    rdoRegEnc.Items[2].Text = "Discharge";
                    gvEncounter.PageSize = 200;
                }
                else if (common.myInt(Request.QueryString["RegEnc"]).Equals(0))
                {
                    rdoRegEnc.SelectedValue = "0";
                    gvEncounter.PageSize = 10;
                    txtBedNo.Enabled = false;
                    txtEncounterNo.Enabled = false;
                }
                if (!string.IsNullOrEmpty(common.myStr(Request.QueryString["SearchOn"])))
                {
                    if (common.myInt(Request.QueryString["SearchOn"]).Equals(0))
                    {
                        rdoRegEnc.SelectedValue = "0";
                        rdoRegEnc.Items[1].Enabled = false;
                        gvEncounter.PageSize = 10;
                    }
                    else if (common.myInt(Request.QueryString["SearchOn"]).Equals(1))
                    {
                        gvEncounter.PageSize = 200;
                    }
                }

                if (common.myStr(Request.QueryString["OPIP"]).Equals("O"))
                {
                    rdoRegEnc.SelectedValue = "0";
                    rdoRegEnc.Enabled = false;
                }

                if (common.myStr(Request.QueryString["SalType"]).Equals("IP"))
                {
                    ViewState["OPIP"] = "I";
                }
                else if (common.myStr(Request.QueryString["SalType"]).Equals("OP"))
                {
                    ViewState["OPIP"] = "O";
                }
                else if (common.myStr(Request.QueryString["SalType"]).Equals("HD"))
                {
                    ViewState["OPIP"] = "H";
                }
                CreateTable();
                // bindData("F", 0);
                dtpFromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
                bindData("F", 0);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            int facilityId = common.myInt(Request.QueryString["AppFacilityId"]);
            if (facilityId.Equals(0) && common.myStr(Request.QueryString["AllFA"]) != "Y")
                facilityId = common.myInt(Session["FacilityId"]);

            if (ds.Tables.Count > 0)
            {
                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "Active = 1 ";
                ddlLocation.Items.Add(new ListItem("Select All", "0"));
                ddlLocation.DataSource = dv;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "FacilityID";
                ddlLocation.DataBind();
                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(common.myStr(facilityId)));
            }

            ListItem lst = new ListItem();
            bool tf = true;
            bool tEncounter = true;
            if (common.myInt(Request.QueryString["RegEnc"]).Equals(1))
            {
                tf = false;
            }
            if (common.myInt(Request.QueryString["RegEnc"]).Equals(0))
            {
                tEncounter = false;
            }

            lst = new ListItem("Registration", "0", tf);
            rdoRegEnc.Items.Add(lst);
            lst = new ListItem("Encounter", "1", tEncounter);
            rdoRegEnc.Items.Add(lst);
            lst = new ListItem("Discharge", "2", true);
            rdoRegEnc.Items.Add(lst);
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

    protected void btnNew_OnClick(object sender, EventArgs e)
    {

    }

    protected void rdoRegEnc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblDate.Visible = false;
            if (common.myInt(rdoRegEnc.SelectedValue).Equals(0))
                gvEncounter.PageSize = 10;
            else if (common.myInt(rdoRegEnc.SelectedValue).Equals(1))
                gvEncounter.PageSize = 200;
            else if (common.myInt(rdoRegEnc.SelectedValue).Equals(2))
            {
                tblDate.Visible = true;
                gvEncounter.PageSize = 200;
            }
            bindData("F", 0);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void CreateTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("EncounterNo");
            dt.Columns.Add("EncDate");
            dt.Columns.Add("OPIP");
            dt.Columns.Add("REGID");
            dt.Columns.Add("ENCID");
            dt.Columns.Add("CompanyCode");
            dt.Columns.Add("InsuranceCode");
            dt.Columns.Add("CardId");
            dt.Columns.Add("RowNo");
            dt.Columns.Add("GenderAge");
            dt.Columns.Add("DoctorName");
            dt.Columns.Add("PhoneHome");
            dt.Columns.Add("MobileNo");
            dt.Columns.Add("DOB");
            dt.Columns.Add("PatientAddress");
            dt.Columns.Add("CompanyName");
            dt.Columns.Add("CurrentBedNo");
            dt.Columns.Add("KinName");
            dt.Columns.Add("DischargeStatus");
            dt.Columns.Add("EncounterStatus");
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            gvEncounter.DataSource = dt;
            gvEncounter.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    private void bindData(string RecordButton, int RowNo)
    {
        try
        {
            objVal = new BaseC.clsEMRBilling(sConString);
            string BedNo = string.Empty, EncNo = string.Empty, RegNo = string.Empty, PatientName = string.Empty, PhoneNo = string.Empty,
                Mobile = string.Empty, CompanyName = string.Empty, PassportNo = string.Empty, Identityno = string.Empty, PEmail = string.Empty,
                RegistrationOld = string.Empty, PDateofbirth = string.Empty;

            DateTime? FromDate = null, ToDate = null, Dob = null;
            if (!string.IsNullOrEmpty(common.myStr(dtpFromDate.SelectedDate)) && !string.IsNullOrEmpty(common.myStr(dtpToDate.SelectedDate)))
            {
                FromDate = common.myDate(dtpFromDate.SelectedDate);
                ToDate = common.myDate(dtpToDate.SelectedDate);
            }
            EncNo = common.myStr(txtEncounterNo.Text);
            RegNo = common.myStr(txtRegistrationNo.Text);
            BedNo = common.myStr(txtBedNo.Text);
            PatientName = common.myStr(txtPatientName.Text);
            PhoneNo = common.myStr(txtPhoneNo.Text);
            Mobile = common.myStr(txtMobileNo.Text);
            CompanyName = common.myStr(txtCompany.Text);
            PassportNo = common.myStr(txtPassportno.Text);
            Identityno = common.myStr(txtCprno.Text);
            if (!string.IsNullOrEmpty(common.myStr(txtDob.Text)))
                Dob = common.myDate(txtDob.Text);

            if (Dob.HasValue)
            {
                PDateofbirth = Dob.Value.ToString("yyyy-MM-dd");
            }
            PEmail = common.myStr(txtEmailId.Text);
            RegistrationOld = common.myStr(txtOldRegistrationno.Text);

            DataSet dsSearch = objVal.getOPIPPendingSignature(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlLocation.SelectedValue), common.myStr(ViewState["OPIP"]), 0, 0, common.myInt(rdoRegEnc.SelectedValue), RegNo, EncNo, common.escapeCharString(PatientName, false), FromDate, ToDate, RecordButton, RowNo, common.myInt(gvEncounter.PageSize), common.myInt(Session["UserId"]), gvEncounter.CurrentPageIndex + 1, common.myStr(BedNo), 0,
                CompanyName, "", PhoneNo, Mobile, Identityno, PassportNo, PDateofbirth, PEmail, RegistrationOld);
           
            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    gvEncounter.VirtualItemCount = Convert.ToInt32(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]);
                }
                else
                {
                    gvEncounter.VirtualItemCount = 0;
                }
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    gvEncounter.DataSource = dsSearch.Tables[0];
                }
                else
                {
                    DataRow DR = dsSearch.Tables[0].NewRow();
                    dsSearch.Tables[0].Rows.Add(DR);
                    gvEncounter.DataSource = dsSearch.Tables[0];
                }
                ViewState["GridDataAdmission"] = dsSearch.Tables[0];
                gvEncounter.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
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
            if (e.Item is GridHeaderItem || e.Item is GridDataItem)
            {
                if (e.Item is GridHeaderItem)
                {
                    if (rdoRegEnc.SelectedValue.Equals("2"))
                    {
                        e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Text = "Discharge Date";
                    }
                }
                if (rdoRegEnc.SelectedValue.Equals("0"))
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.EncounterNo)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.DoctorName)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.CurrentBedNo)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Visible = false;
                }
                if (rdoRegEnc.SelectedValue.Equals("0") || rdoRegEnc.SelectedValue.Equals("1"))
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.DischargeStatus)].Visible = false;
                }
                if (rdoRegEnc.SelectedValue.Equals("2"))
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;
                }
                if (common.myStr(Request.QueryString["OPIP"]).Equals("I"))
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
                }
                if (common.myStr(Request.QueryString["OPIP"]).Equals("O"))
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
                    e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                Label lblName = (Label)e.Item.FindControl("lblName");
                Label lblPatientAddress = (Label)e.Item.FindControl("lblPatientAddress");
                HiddenField hdnKinName = (HiddenField)e.Item.FindControl("hdnKinName");
                if (lblName != null && lblPatientAddress != null && hdnKinName != null)
                {
                    e.Item.Attributes.Add("onclick", "javascript:ShowPatientDetails('" + lblName.ClientID +
                                    "','" + lblPatientAddress.ClientID + "','" + hdnKinName.ClientID + "');");
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

    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            gvEncounter.CurrentPageIndex = e.NewPageIndex;
            bindData("F", 0);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GridDataAdmission"] != null)
            {
                gvEncounter.DataSource = (DataTable)ViewState["GridDataAdmission"];
                gvEncounter.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Select"))
            {
                if (common.myInt(((Label)e.Item.FindControl("lblREGID")).Text) > 0)
                {
                    hdnRegistrationId.Value = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                    hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    hdnEncounterId.Value = common.myStr(((Label)e.Item.FindControl("lblENCID")).Text);
                    hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    hdnCompanyCode.Value = common.myStr(((Label)e.Item.FindControl("lblCompanyCode")).Text);
                    hdnInsuranceCode.Value = common.myStr(((Label)e.Item.FindControl("lblInsuranceCode")).Text);
                    hdnCardId.Value = common.myStr(((Label)e.Item.FindControl("lblCardId")).Text);
                    hdnEncounterDate.Value = common.myStr(((Label)e.Item.FindControl("lblEncDate")).Text);
                    hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblGenderAge")).Text);
                    hdnPhoneHome.Value = common.myStr(((Label)e.Item.FindControl("lblPhoneHome")).Text);
                    hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                    hdnPatientName.Value = common.myStr(((Label)e.Item.FindControl("lblName")).Text);
                    hdnDOB.Value = common.myStr(((Label)e.Item.FindControl("lblDOB")).Text);
                    hdnAddress.Value = common.myStr(((Label)e.Item.FindControl("lblPatientAddress")).Text);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
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

    protected void rdoSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoSearch.SelectedValue.Equals("0"))
            {
                tblsearch.Visible = true;
                tblDate.Visible = false;
                dtpFromDate.SelectedDate = null;
                dtpToDate.SelectedDate = null;
            }
            else
            {
                tblsearch.Visible = false;
                tblDate.Visible = true;
                dtpFromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        try
        {
            bindData("F", 0);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        txtRegistrationNo.Text = string.Empty;
        txtEncounterNo.Text = string.Empty;
        txtBedNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtCompany.Text = string.Empty;
        txtPassportno.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        txtDob.Text = string.Empty;
        txtOldRegistrationno.Text = string.Empty;
        txtCprno.Text = string.Empty;
        lblMessage.Text = string.Empty;

        bindData("F", 0);
    }
}