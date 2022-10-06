using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OTScheduler_OTBookingFollowupRemarks : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            BindPrevious();
        }
    }
    protected void BindPrevious()
    {
        BaseC.clsOTBooking objOt = new BaseC.clsOTBooking(sConString);

        lblPreviousRemarks.Text = objOt.GetOTBookingFollowupRemarks(common.myInt(Request.QueryString["AppId"]));
    }

    protected void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            lblregNo.Text = common.myStr(Request.QueryString["RegNo"]);
            lblOtBookingNo.Text = common.myStr(Request.QueryString["AppNo"]);
            lblBookingDate.Text = common.myStr(Request.QueryString["OTBDt"]);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        if (common.myInt(Session["FacilityId"]).Equals(0) || common.myInt(Session["UserId"]).Equals(0))
        {
            lblMessage.Text = "Session Expired. Please Re Login to save";
            return;
        }

        if (txtRemarks.Text.Length.Equals(0) || txtRemarks.Text.Length > 100)
        {
            lblMessage.Text = "Some Error Occured. Please contact IT";
            return;
        }
        BaseC.clsOTBooking objOt = new BaseC.clsOTBooking(sConString);
        try
        {
            lblMessage.Text = objOt.SaveOTBookingFollowupRemarks(common.myInt(Request.QueryString["AppId"]), common.myInt(Session["FacilityId"]), txtRemarks.Text, common.myInt(Session["UserId"]), common.myInt(Session["HospitalLocationId"]));

            if (lblMessage.Text.ToUpper().Contains("SUCCESSFULLY"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                txtRemarks.Text = string.Empty;
                BindPrevious();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Some Error Occured. Please contact IT";
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
}