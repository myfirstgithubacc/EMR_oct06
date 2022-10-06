using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

public partial class LIS_Phlebotomy_PatientDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            getPataientDetails();
        }
    }

    public void getPataientDetails()
    {
        try
        {
            //
            if (Request.QueryString["RegNo"] != null && Request.QueryString["PName"] != null)
            {
                BaseC.clsLISPhlebotomy objMaster = new BaseC.clsLISPhlebotomy(sConString);
                this.lblPatientDetails.Text = HttpContext.GetGlobalResourceObject("PRegistration", "regno") + ": " + common.myStr(Request.QueryString["RegNo"]) + " | Patient Name: " + common.myStr(Request.QueryString["PName"]).Trim();
                Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegNo"]);
                DataSet objds = objMaster.getPatientDetails(common.myInt(Request.QueryString["RId"]) , common.myInt(Session["FacilityID"]));
                if (objds.Tables.Count > 0)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        txtFacility.Text = common.myStr(objds.Tables[0].Rows[0]["FacilityName"]);
                        txtDob.Text = common.myStr(objds.Tables[0].Rows[0]["DateofBirth"]);
                        txtMartialStatus.Text = common.myStr(objds.Tables[0].Rows[0]["MaritalStatus"]);
                        txtMobileNo.Text = common.myStr(objds.Tables[0].Rows[0]["MobileNo"]);
                        txtPhoneNo.Text = common.myStr(objds.Tables[0].Rows[0]["PhoneHome"]);
                        txtlocalAddress1.Text = common.myStr(objds.Tables[0].Rows[0]["LocalAddress1"]);
                        txtlocalAddress2.Text = common.myStr(objds.Tables[0].Rows[0]["LocalAddress2"]);
                        txtnationality.Text = common.myStr(objds.Tables[0].Rows[0]["Nationality"]);
                        txtPin.Text = common.myStr(objds.Tables[0].Rows[0]["LocalPin"]);
                        txtGender.Text = common.myStr(objds.Tables[0].Rows[0]["Gender"]);
                        txtPayername.Text = common.myStr(objds.Tables[0].Rows[0]["PayerName"]);
                        txtCompanyName.Text = common.myStr(objds.Tables[0].Rows[0]["CompanyName"]);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }

    }

}
