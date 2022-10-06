using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_SearchPatient : System.Web.UI.Page
{
    private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    "IMAGEmateEMRScannedDocumentPath", sConString);

            if (collHospitalSetupValues.ContainsKey("IMAGEmateEMRScannedDocumentPath"))
                ViewState["IMAGEmateEMRScannedDocumentPath"] = collHospitalSetupValues["IMAGEmateEMRScannedDocumentPath"];

            BindControl();
            CompanyBind();
        }
    }
    public void BindData()
    {
        DateTime? Dob = null;
        string PDateofbirth = "";
        if (common.myStr(txtDOB.Text) != "")
        {
            Dob = common.myDate(txtDOB.Text);
        }

        if (Dob.HasValue)
        {
            PDateofbirth = Dob.Value.ToString("yyyy-MM-dd");
        }
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        DataSet ds = objBill.GetRegistrationList(common.myInt(txtRegistrationNo.Text), txtPatientName.Text, txtMobileNo.Text,
            txtAddress.Text, ddlcompany.SelectedValue, txtOldRegNo.Text, common.myStr(Dob), common.myStr(txtEmpNo.Text), common.myStr(txtGName.Text));
        gvRegistration.DataSource = ds; gvRegistration.DataBind();
    }
    private void CompanyBind()
    {
        DataTable dtCompany = new DataTable();
        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
        dtCompany = baseEBill.getCompanyList(common.myInt(Session["HospitalLocationId"]), "A", 0,
            common.myInt(Session["FacilityId"])).Tables[0];
        
        ddlcompany.DataSource = dtCompany;
        ddlcompany.DataTextField = "Name";
        ddlcompany.DataValueField = "CompanyId";
        ddlcompany.DataBind();
        ddlcompany.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
    }

    private void BindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            if (ds.Tables.Count > 0)
            {
                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "Active = 1 ";
                ddlLocation.Items.Add(new ListItem("Select All", "0"));
                ddlLocation.DataSource = dv;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "FacilityID";
                ddlLocation.DataBind();
                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(common.myStr(common.myInt(Session["FacilityId"]))));
            }

        }
        catch (Exception Ex)
        {
            Alert.ShowAjaxMsg("Error: " + Ex.Message, this.Page);
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void gvRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            hdnRegistrationId.Value = ((HiddenField)row.FindControl("hdnRegId")).Value;
            hdnRegistrationNo.Value = ((Label)row.FindControl("lblRegistrationNo")).Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            return;
        }
        if (e.CommandName == "SD")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            hdnRegistrationNo.Value = ((Label)row.FindControl("lblRegistrationNo")).Text;
            
            if (common.myStr(ViewState["IMAGEmateEMRScannedDocumentPath"]) != "")
            {
                string url = common.myStr(ViewState["IMAGEmateEMRScannedDocumentPath"]) + "HHNO=" + common.myLong(common.myStr(hdnRegistrationNo.Value)) + "%20&DoctorCode=301";
                string script = " <script type=\"text/javascript\">  window.open('" + url + "');   </script> ";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, false);
            }
            return;
        }
    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtRegistrationNo.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        ddlcompany.SelectedIndex = 0;
        txtAddress.Text = string.Empty;
        txtOldRegNo.Text = string.Empty;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
}