using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_ConfidentialTemplateRequest : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindConfidentialTemplate();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        string SavingStatus = string.Empty;
        try
        {
            SavingStatus = objEMR.EMRConfidentialTemplateTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ddlConfidentialTemplate.SelectedValue),
                common.myInt(Session["EmployeeId"]), false, 0, "R");

            if (SavingStatus.ToUpper().Contains("SAVE") || SavingStatus.ToUpper().Contains("REQUEST"))
            {
                lblMessage.Text = SavingStatus;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void BindConfidentialTemplate()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = objEMR.GetConfidentialTemplate();
            ddlConfidentialTemplate.DataSource = dt;
            ddlConfidentialTemplate.DataValueField = "ID";
            ddlConfidentialTemplate.DataTextField = "TemplateName";
            ddlConfidentialTemplate.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }


}