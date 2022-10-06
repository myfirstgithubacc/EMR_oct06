using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_AddChargeTimewisV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            if (!Page.IsPostBack)
            {
                ViewState["RegID"] = common.myStr(Request.QueryString["RegId"]);
                ViewState["RegNo"] = common.myStr(Request.QueryString["RegNo"]);
                ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
                ViewState["EncNo"] = common.myStr(Request.QueryString["EncNo"]);

                ViewState["CompanyId"] = common.myStr(Request.QueryString["CompanyId"]);
                ViewState["InsuranceId"] = common.myStr(Request.QueryString["InsuranceId"]);
                ViewState["CardId"] = common.myStr(Request.QueryString["CardId"]);

                dtpfromdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpfromdate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
                dtpTodate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpTodate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtpTodate.SelectedDate = common.myDate(DateTime.Now);
                hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                ViewState["GracePariodInMinuteForTimeBasedServices"] = common.myStr(objBill.getHospitalSetupValue("GracePariodInMinuteForTimeBasedServices", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                BindGrid();
                BindProvider();
                FillService();
                CalculateTime();
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
    public void FillService()
    {
        BaseC.CompanyServiceSetup objCompanyServiceSetup = new BaseC.CompanyServiceSetup(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objCompanyServiceSetup.getGetItemofserviceChargeType(common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRowView drChargeType in ds.Tables[0].DefaultView)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)drChargeType["ServiceName"];
                        item.Value = drChargeType["ServiceId"].ToString();
                        item.Attributes.Add("ChargeType", common.myStr(drChargeType["ChargeType"]));
                        ddlService.Items.Add(item);
                        item.DataBind();

                    }
                    RadComboBoxItem All = new RadComboBoxItem(" Select ");
                    ddlService.Items.Insert(0, All);

                    getCharge();
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
            hshServiceDetail = BaseBill.getSingleServiceAmountTimeBase(common.myInt(Session["HospitalLocationID"]),
                   common.myInt(Session["FacilityId"]),
                   common.myInt(ViewState["CompanyId"]),
                   common.myInt(ViewState["InsuranceId"]),
                   common.myInt(0),
                   common.myStr(Request.QueryString["OP_IP"]),
                   common.myInt(ddlService.SelectedValue),
                   common.myInt(ViewState["RegID"]),
                   common.myInt(ViewState["EncId"]), 0, 0, 0);
            txtCharge.Text = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            txtDrCharge.Text = common.myDec(common.myDec(hshServiceDetail["DNchr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            hdnChargeType.Value = common.myStr(hshServiceDetail["ChargeType"]);//PH,PD
            hdnDiscount.Value = common.myStr(hshServiceDetail["DiscountPerc"]);
            ddlDoctor.SelectedIndex = 0;
            if (common.myStr(hshServiceDetail["DoctorRequired"]) == "True")
            {
                hdnDoctorRequired.Value = "1";
                ddlDoctor.Enabled = true;
                lblStarDoctor.Visible = true;
            }
            else
            {
                hdnDoctorRequired.Value = "0";
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

            int GracePariod = common.myInt(ViewState["GracePariodInMinuteForTimeBasedServices"]);

            lblMessage.Text = "";
            DateTime dtStart = dtpfromdate.SelectedDate.Value;
            DateTime dtEnd = dtpTodate.SelectedDate.Value;
            if (dtEnd >= dtStart)
            {
                double TotalMinutesDiff = Math.Round((dtEnd - dtStart).TotalMinutes, 0) + GracePariod;
                double TotalMinutesWithoutGrace = Math.Round((dtEnd - dtStart).TotalMinutes, 0);

                double TotalDay = 0;
                double TotalHours = 0;
                double TotalMinutes = 0;


                if (common.myStr(hdnChargeType.Value) == "PD")// per day
                {
                    TotalDay = Math.Round((TotalMinutesDiff / (60 * 24)), 0);
                    //if (TotalDay > 0)
                    //{
                    txtUnit.Text = common.myStr(TotalDay == 0 ? 1 : TotalDay); ;
                    //}
                    //else
                    //{
                    //    txtUnit.Text = common.myStr(TotalDay);
                    //}
                    lblUnitDescription.Text = TotalDay.ToString() + " Days" + " " + TotalHours + " Hours ";
                }
                else if (common.myStr(hdnChargeType.Value) == "PH")// per hour
                {
                    TotalHours = Math.Round((TotalMinutesDiff / 60), 0);
                    txtUnit.Text = common.myStr(TotalHours);
                    lblUnitDescription.Text = TotalHours + " Hours " + TotalMinutesWithoutGrace + " Minutes";
                }
                else if (common.myStr(hdnChargeType.Value) == "PM")// per 30 Minutes  is equal to 1 unit
                {
                    TotalMinutes = Math.Round((TotalMinutesDiff / 30), 0);
                    txtUnit.Text = common.myStr(TotalMinutes == 0 ? 1 : TotalMinutes);
                    lblUnitDescription.Text = TotalHours + " Hours " + TotalMinutesWithoutGrace + " Minutes";
                }
                else if (common.myStr(hdnChargeType.Value) == "" || common.myStr(hdnChargeType.Value) == "0")
                {
                    txtUnit.Text = "1";
                    lblUnitDescription.Text = "";
                }

                //lblUnitDescription.Text = TotalDay.ToString() + " Days" + " " + TotalHours + " Hours " + TotalMinutesWithoutGrace + " Minutes";
            }

            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "From Date should not be greater than To Date !";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
                dtpTodate.SelectedDate = common.myDate(DateTime.Now);
                txtUnit.Text = "0";
                lblUnitDescription.Text = "0 Days" + " 0 Hours";

                return;
            }

            //}
            //else
            //{
            //    lblMessage.Text = "";
            //    DateTime dtStart = dtpfromdate.SelectedDate.Value;
            //    DateTime dtEnd = dtpTodate.SelectedDate.Value;
            //    if (dtEnd >= dtStart)
            //    {
            //        int TotalHoursDiff = common.myInt(Math.Round((dtEnd - dtStart).TotalHours, 0));
            //        int TotalDay = common.myInt(TotalHoursDiff / 24);
            //        int TotalHours = TotalHoursDiff - (TotalDay * 24);
            //        lblUnitDescription.Text = TotalDay.ToString() + " Days" + " " + TotalHours + " Hours";

            //        if (hdnChargeType.Value == "PD")// per day
            //        {
            //            if (TotalHours > 0)
            //            {
            //                txtUnit.Text = common.myStr(TotalDay + 1);
            //            }
            //            else
            //            {
            //                txtUnit.Text = common.myStr(TotalDay);
            //            }
            //        }
            //        else if (hdnChargeType.Value == "PH")// per hour
            //        {
            //            txtUnit.Text = common.myStr(TotalHoursDiff);
            //        }
            //    }
            //    else
            //    {
            //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //        lblMessage.Text = "From Date should not be greater than To Date !";

            //        dtpfromdate.SelectedDate = common.myDate(DateTime.Now);
            //        dtpTodate.SelectedDate = common.myDate(DateTime.Now);
            //        txtUnit.Text = "0";
            //        lblUnitDescription.Text = "0 Days" + " 0 Hours";

            //        return;
            //    }

            //}
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
    public void btnsave_OnClick(object sender, EventArgs e)
    {
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        try
        {
            if (common.myInt(hdnDoctorRequired.Value).Equals(1) && common.myInt(ddlDoctor.SelectedValue).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Doctor !";
                return;

            }
            if (common.myInt(ddlService.SelectedValue).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Service !";
                return;

            }

            int Unit = common.myInt(txtUnit.Text) == 0 ? 1 : common.myInt(txtUnit.Text);
            Hashtable outHash = BaseBill.TimeBasedServiceSaveData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegID"])
               , common.myInt(ViewState["EncId"]), common.myStr(txtRemark.Text), common.myInt(Session["UserId"]), common.myInt(ddlDoctor.SelectedValue)
               , common.myInt(ddlService.SelectedValue), Unit, common.myStr(Request.QueryString["OP_IP"])
               , common.myDate(dtOrderDate.SelectedDate), common.myDate(dtpfromdate.SelectedDate), common.myDate(dtpTodate.SelectedDate), common.myDbl(txtCharge.Text), common.myDbl(txtDrCharge.Text),
               common.myDbl(hdnDiscount.Value));


            if (common.myStr(outHash["chvErrorStatus"]) != "")
            {
                BindGrid();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = common.myStr(outHash["chvErrorStatus"]);
                getCharge();
                txtRemark.Text = "";
                ddlService.SelectedValue = "0";
                ddlService.SelectedIndex = -1;
                ddlService.Text = "";
                txtUnit.Text = "";
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
    private void BindGrid()
    {
        BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
        DataTable dt = new DataTable();
        Hashtable hashIn = new Hashtable();
        try
        {
            hashIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hashIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hashIn.Add("@intEncounterId", common.myInt(ViewState["EncId"]));
            dt = obj.GetEncounterTimeBaseServices(hashIn);
            if (dt.Rows.Count > 0)
            {
                gvTimeBaseService.DataSource = dt;
                gvTimeBaseService.DataBind();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            obj = null;
            dt.Dispose();
            hashIn = null;
        }
    }



}
