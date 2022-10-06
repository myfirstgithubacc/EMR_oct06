using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_AddTimeBasedService : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
                Response.Redirect("~/Login.aspx?Logout=1", false);
                return;
            }
            if (!Page.IsPostBack)
            {

                lblReg.Text = common.myStr(Session["RegistrationLabelName"]);
                dtpfromdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpfromdate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
                dtpTodate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpTodate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpfromdate.MaxDate = common.myDate(DateTime.Now);
                dtpTodate.MaxDate = common.myDate(DateTime.Now);
                hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                BindPatientHiddenDetails(common.myInt(Request.QueryString["RegNo"]));
                BindProvider();
                FillService();
                CalculateTime();
                BindAllOrders();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objBill = null; }
    }
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (RegistrationNo>0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegId = 0;
                int EncounterId = 0;
                int EncodedBy = common.myInt(Session["UserId"]);
                lblregNo.Text = RegistrationNo.ToString();
                if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
                    lblInfoEncNo.Visible = false;
                    lblInfoAdmissionDt.Visible = false;
                    lblEncounterNo.Visible = false;
                    lblAdmissionDate.Visible = false;
                }
                else
                {
                    BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                        dtpfromdate.MinDate = common.myDate(dr["EncounterDate"]);
                        lblMessage.Text = "";
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            bC = null;
            ds.Dispose();
        }
    }
    public void FillService()
    {
        BaseC.CompanyServiceSetup objCompanyServiceSetup = new BaseC.CompanyServiceSetup(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objCompanyServiceSetup.getGetItemofserviceChargeType(common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "ChargeType IN ('PH', 'PD')";
                if (dv != null && dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRowView drChargeType in dv)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)drChargeType["ServiceName"];
                        item.Value = drChargeType["ServiceId"].ToString();
                        item.Attributes.Add("ChargeType", common.myStr(drChargeType["ChargeType"]));
                        ddlService.Items.Add(item);
                        item.DataBind();

                    }
                    ddlService.Items.Insert(0, new RadComboBoxItem("Select Service", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objCompanyServiceSetup = null; ds.Dispose(); }
    }
    protected void ddlService_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try { getCharge(); }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void dtpfromdate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        try { CalculateTime(); }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void dtpTodate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        try { CalculateTime(); }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    public void getCharge()
    {
        Hashtable hshServiceDetail = new Hashtable();
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        try
        {
            hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                   common.myInt(Session["FacilityId"]),
                   common.myInt(Request.QueryString["CompanyId"]),
                   common.myInt(Request.QueryString["InsuranceId"]),
                   common.myInt(Request.QueryString["CardId"]),
                   common.myStr(Request.QueryString["OP_IP"]),
                   common.myInt(ddlService.SelectedValue),
                   common.myInt(Request.QueryString["RegId"]),
                   common.myInt(Request.QueryString["EncId"]), 0, 0, 0, string.Empty);

            txtCharge.Text = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            txtDrCharge.Text = common.myDec(common.myDec(hshServiceDetail["DNchr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            hdnChargeType.Value = common.myStr(hshServiceDetail["ChargeType"]);//PH,PD
            ddlDoctor.SelectedIndex = 0;
            if (common.myBool(hshServiceDetail["DoctorRequired"]))
            {
                hdnDoctorRequired.Value = "1";
                ddlDoctor.Enabled = true;
                ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(Request.QueryString["ConsultId"]));
                lblStarDoctor.Visible = true;
            }
            else
            {
                hdnDoctorRequired.Value = "0";
                ddlDoctor.SelectedIndex = 0;
                ddlDoctor.Enabled = false;
                lblStarDoctor.Visible = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { hshServiceDetail = null; BaseBill = null; }
    }

    public void CalculateTime()
    {
        try
        {
            lblMessage.Text = "";
            DateTime dtStart = dtpfromdate.SelectedDate.Value;
            DateTime dtEnd = dtpTodate.SelectedDate.Value;
            if (dtEnd >= dtStart)
            {
                int TotalHoursDiff = common.myInt(Math.Round((dtEnd - dtStart).TotalHours, 0));
                int TotalDay = common.myInt(TotalHoursDiff / 24);
                int TotalHours = TotalHoursDiff - (TotalDay * 24);
                lblPeriodDescription.Text = TotalDay.ToString() + " Days" + " " + TotalHours + " Hours";

                if (hdnChargeType.Value == "PD")// per day
                {
                    lblPeriodDescription.Text = lblPeriodDescription.Text + " Calculated Daily";
                    if (TotalHours > 0)
                    {
                        txtUnit.Text = common.myStr(TotalDay + 1);
                    }
                    else
                    {
                        txtUnit.Text = common.myStr(TotalDay);
                    }
                }
                else if (hdnChargeType.Value == "PH")// per hour
                {
                    lblPeriodDescription.Text = lblPeriodDescription.Text + " Calculated Hourly";
                    txtUnit.Text = common.myStr(TotalHoursDiff);
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "From Date should not be greater than To Date !";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
                dtpTodate.SelectedDate = common.myDate(DateTime.Now);
                txtUnit.Text = "0";
                lblPeriodDescription.Text = "0 Days" + " 0 Hours";
                return;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }


    private void BindProvider()
    {
        if (Session["HospitalLocationID"] == null)
            return;
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        try
        {

            ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                ddlDoctor.DataSource = ds;
                ddlDoctor.DataValueField = "DoctorID";
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataBind();
                ddlDoctor.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { ds.Dispose(); objCM = null; }
    }
    private void BindAllOrders()
    {
        if (Session["HospitalLocationID"] == null)
            return;
        DataSet ds = new DataSet();
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        try
        {
            ds = BaseBill.GetPatientTimeBasedService(common.myInt(Request.QueryString["EncId"]));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvService.DataSource = ds;
            }
            else
                gvService.DataSource = null;
            gvService.DataBind();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { ds.Dispose(); BaseBill = null; }
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        if (Session["HospitalLocationID"] == null)
            return;
        if (common.myInt(hdnDoctorRequired.Value).Equals(1) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Doctor !";
            return;
        }
        if (common.myInt(ddlService.SelectedItem.Value).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Service !";
            return;
        }
        if (dtpfromdate.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Start !";
            return;
        }
        if (common.myInt(hdnDoctorRequired.Value).Equals(1) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Doctor !";
            return;
        }
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        try
        {
            lblMessage.Text = BaseBill.SavePatientTimeBasedService(common.myInt(hdnChargeId.Value), common.myInt(Request.QueryString["EncId"]), common.myInt(ddlService.SelectedValue),
                common.myInt(ddlDoctor.SelectedValue), common.myDate(dtpfromdate.SelectedDate), dtpTodate.SelectedDate, false,
                common.myInt(hdnOrderId), common.myInt(hdnServiceOrderDetailId), common.myStr(lblPeriodDescription.Text), common.myInt(lblUnit.Text), common.myStr(txtRemark.Text), common.myInt(Session["UserId"]));


            if (lblMessage.Text.ToLower().Contains(" saved"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                getCharge();
                BindAllOrders();
                txtRemark.Text = "";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            objException.HandleException(ex);
            objException = null;
        }
        finally { BaseBill = null; }
    }

    protected void btnEnd_Click(object sender, EventArgs e)
    {
        if (Session["HospitalLocationID"] == null)
            return;
        if (common.myInt(hdnDoctorRequired.Value).Equals(1) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Doctor !";
            return;
        }
        if (dtpTodate.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            dtpTodate.Focus();
            lblMessage.Text = "Please Select End Date Time !";
            return;
        }
        if (common.myInt(hdnChargeId.Value).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Order !";
            return;
        }
        if (common.myInt(hdnDoctorRequired.Value).Equals(1) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Doctor !";
            return;
        }


        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);

        try
        {

            lblMessage.Text = BaseBill.SavePatientTimeBasedService(common.myInt(hdnChargeId.Value), common.myInt(Request.QueryString["EncId"]),
                common.myInt(ddlService.SelectedValue),
                common.myInt(ddlDoctor.SelectedValue), common.myDate(dtpfromdate.SelectedDate), dtpTodate.SelectedDate, true,
                common.myInt(hdnOrderId), common.myInt(hdnServiceOrderDetailId), common.myStr(lblPeriodDescription.Text), common.myInt(txtUnit.Text), common.myStr(txtRemark.Text), common.myInt(Session["UserId"]));


            if (lblMessage.Text.ToLower().Contains(" saved"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                getCharge();
                BindAllOrders();
                txtRemark.Text = "";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { BaseBill = null; }
    }

    protected void gvService_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int intId = common.myInt(e.CommandArgument);
            hdnChargeId.Value = intId.ToString();
            BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
            lblMessage.Text = BaseBill.CancelPatientTimeBasedService(common.myInt(hdnChargeId.Value), common.myInt(Session["UserId"]));
            if (lblMessage.Text.ToLower().Contains(" saved"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                getCharge();
                BindAllOrders();
                txtRemark.Text = "";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }

        }
        else if (e.CommandName == "Sel")
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int intId = common.myInt(e.CommandArgument);
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            hdnChargeId.Value = intId.ToString();
            ddlService.SelectedIndex = ddlService.Items.IndexOf(ddlService.Items.FindItemByValue(common.myStr((row.FindControl("hdnServiceId") as HiddenField).Value)));

            dtpfromdate.SelectedDate = Convert.ToDateTime((row.FindControl("hdnStartDateTime") as HiddenField).Value);

            if (!common.myStr((row.FindControl("hdnEndDateTime") as HiddenField).Value).Equals(""))
                dtpTodate.SelectedDate = Convert.ToDateTime((row.FindControl("hdnEndDateTime") as HiddenField).Value);
            txtCharge.Text = (row.FindControl("lblServiceAmount") as Label).Text;
            txtDrCharge.Text = (row.FindControl("lblDoctorAmount") as Label).Text;
            ddlService_OnSelectedIndexChanged(null, null);

            txtUnit.Text = (row.FindControl("lblUnitsCalculated") as Label).Text;
            lblPeriodDescription.Text = (row.FindControl("lblOrderPeriod") as Label).Text;
            txtRemark.Text = (row.FindControl("lblRemarks") as Label).Text;
            dtpTodate.Enabled = true;
            ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(common.myStr((row.FindControl("hdnDoctorId") as HiddenField).Value)));

        }
    }

    protected void gvService_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
            hdnChargeId.Value = "0";
        }
    }
}
