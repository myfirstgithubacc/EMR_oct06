using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WardManagement_CurrentPatientLocation : System.Web.UI.Page
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
            if (common.myStr(Request.QueryString["RegNo"]) != "")
            {
                if (common.myStr(Request.QueryString["Encno"]) != "")
                {
                    GetCurrentLocation();
                }
            }
        }
    }

    public void GetCurrentLocation()
    {
        try
        {
            OpPrescription otb = new OpPrescription(sConString);
            ds = new DataSet();
            ds = otb.GetAdmissionCurrentLocation(common.myInt(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["Encno"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label2.Text = common.myStr(ds.Tables[0].Rows[0]["LocationName"]);
            }
           else
            {
                Label2.Text = "Not Defined";
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddlLocationName.DataSource = ds.Tables[1];
                ddlLocationName.DataValueField = "locationId";
                ddlLocationName.DataTextField = "locationName";
                ddlLocationName.DataBind();
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
        try
        {
            OpPrescription otb = new OpPrescription(sConString);
            String strMessage = otb.UpdateAdmissionPatientLocation(common.myInt(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["Encno"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(ddlLocationName.SelectedValue));
            if (strMessage.ToString().ToUpper().Equals("SAVE DATA."))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Update Successfully";
                GetCurrentLocation();

            }
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}