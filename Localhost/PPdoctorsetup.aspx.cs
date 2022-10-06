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
using BaseC;
using System.IO;
using System.Xml;

public partial class WardManagement_ChangeEncounterDate : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.ATD objatd;
    BaseC.clsEMRBilling baseEBill;
    BaseC.clsLISMaster objLISMaster;
    BaseC.EMRBilling.clsOrderNBill bOrdernBill;
    BaseC.WardManagement objwd;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLocation();
            BindProvider();
            BindTeam();
            GetPPdoctorsetup();
           

            RadMorningSlotCutOffTime.TimeView.StartTime = new TimeSpan(8, 0, 0);
            RadMorningSlotCutOffTime.TimeView.EndTime = new TimeSpan(18, 0, 0);
            RadMorningSlotCutOffTime.TimeView.Interval = new TimeSpan(0, 15, 0);

            //dtpEod.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            //dtpEod.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            //dtpEod.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            //dtpEod.MinDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            //hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
        }
    }

    private void BindLocation()
    {
        try
        {

            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds1 = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);


            ddlLocation.DataSource = ds1;
            ddlLocation.DataTextField = "FacilityName";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new RadComboBoxItem("All", ""));
            ddlLocation.SelectedIndex = 0;
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindItemByValue(common.myStr(Session["FacilityId"])));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindProvider()
    {
        try
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet objDs = new DataSet();


            Hashtable hsIn = new Hashtable();
            hsIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@iUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intSpecializationId", 0);
            hsIn.Add("@intFacilityId", common.myInt(ddlLocation.SelectedValue));
            hsIn.Add("@intAdvisingDoctorId", common.myInt(Session["EmployeeId"]));
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", hsIn);

            //Cache.Insert("Doctor", objDs, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            ddlProvider.DataSource = objDs;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void RadMorningSlotCutOffTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {

    }

    protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindProvider();
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
        BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
        try
        {
            if(common.myInt(ddlTeam.SelectedValue).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Provider is not tagged to any Team";
                return;
            }

            Hashtable HshOut = objEMR.SavePPdoctorsetup(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlProvider.SelectedValue), common.myInt(ddlLocation.SelectedValue), common.myInt(txtDiscountAmount.Text), common.myInt(txtDiscountPercentage.Text)
                , common.myInt(txtDiscountAmountOnArrival.Text), common.myInt(txtDiscountPercentageOnArrival.Text), chkIsOnlinePaymentMandatory.Checked
                ,common.myInt(ddlTeam.SelectedValue)
                , common.myInt(txtMorningSlots.Text), chkMorningExtraChargeAllow.Checked, common.myInt(txtMorningExtraChargePercent.Text), common.myInt(txtMorningExtraChargeAmt.Text), common.myInt(txtEveningSlots.Text), chkEveningExtraChargeAllow.Checked, common.myInt(txtEveningExtraChargePercent.Text),
                common.myInt(txtEveningExtraChargeAmt.Text), common.myStr(RadMorningSlotCutOffTime.SelectedDate), common.myInt(Session["UserId"]));

            if (common.myStr(HshOut["@chvErrorStatus"]).ToUpper().Contains(" UPDATE")
             || common.myStr(HshOut["@chvErrorStatus"]).ToUpper().Contains(" SAVE"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = common.myStr(HshOut["@chvErrorStatus"]);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = common.myStr(HshOut["@chvErrorStatus"]);
            }
            HshOut = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }

    private void BindTeam()
    {
        BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = objEMR.GetTeamByDoctors(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlLocation.SelectedValue), 
                common.myInt(ddlProvider.SelectedValue)
                );
            ddlTeam.DataSource = dt;
            ddlTeam.DataTextField = "Team";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            dt.Dispose();
        }
    }
    private void GetPPdoctorsetup()
    {
        BaseC.EMRMasters objEMR = new BaseC.EMRMasters(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = objEMR.GetPPdoctorsetup(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlLocation.SelectedValue), common.myInt(ddlProvider.SelectedValue));
            if (common.myInt(dt.Rows.Count) > 0)
            {
                ddlProvider.SelectedValue = common.myStr(dt.Rows[0]["DoctorId"]);
                txtDiscountAmount.Text = common.myStr(dt.Rows[0]["DiscountAmount"]);
                txtDiscountPercentage.Text = common.myStr(dt.Rows[0]["DiscountPercentage"]);
                txtDiscountAmountOnArrival.Text = common.myStr(dt.Rows[0]["DiscountAmountOnArrival"]);
                txtDiscountPercentageOnArrival.Text = common.myStr(dt.Rows[0]["DiscountPercentageOnArrival"]);
                chkIsOnlinePaymentMandatory.Checked = common.myBool(dt.Rows[0]["IsOnlinePaymentMandatory"]);
                ddlTeam.SelectedValue = common.myStr(dt.Rows[0]["TeamId"]);
                txtMorningSlots.Text = common.myStr(dt.Rows[0]["MorningSlots"]);
                chkMorningExtraChargeAllow.Checked = common.myBool(dt.Rows[0]["MorningExtraChargeAllow"]);
                txtMorningExtraChargePercent.Text = common.myStr(dt.Rows[0]["MorningExtraChargePercent"]);
                txtMorningExtraChargeAmt.Text = common.myStr(dt.Rows[0]["MorningExtraChargeAmt"]);
                txtEveningSlots.Text = common.myStr(dt.Rows[0]["EveningSlots"]);
                chkEveningExtraChargeAllow.Checked = common.myBool(dt.Rows[0]["EveningExtraChargeAllow"]);
                txtEveningExtraChargePercent.Text = common.myStr(dt.Rows[0]["EveningExtraChargePercent"]);
                txtEveningExtraChargeAmt.Text = common.myStr(dt.Rows[0]["EveningExtraChargeAmt"]);
                RadMorningSlotCutOffTime.SelectedDate =   common.myDate( (common.myStr("2019-01-01 ")+common.myStr(dt.Rows[0]["MorningSlotCutOffTime"]))     );
            }
            else
            {
                txtDiscountAmount.Text =string.Empty;
                txtDiscountPercentage.Text = string.Empty;
                txtDiscountAmountOnArrival.Text = string.Empty;
                txtDiscountPercentageOnArrival.Text = string.Empty;
                chkIsOnlinePaymentMandatory.Checked = false;
                txtMorningSlots.Text = string.Empty;
                chkMorningExtraChargeAllow.Checked = false;
                txtMorningExtraChargePercent.Text = string.Empty;
                txtMorningExtraChargeAmt.Text = string.Empty;
                txtEveningSlots.Text = string.Empty;
                chkEveningExtraChargeAllow.Checked =false;
                txtEveningExtraChargePercent.Text = string.Empty;
                txtEveningExtraChargeAmt.Text = string.Empty;
                RadMorningSlotCutOffTime.SelectedDate = null; 
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlProvider_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindTeam();
            GetPPdoctorsetup();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
}
