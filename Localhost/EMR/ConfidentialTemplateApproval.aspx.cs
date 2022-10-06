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
            SavingStatus = objEMR.EMRConfidentialTemplateTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myInt(ddlConfidentialTemplateApproval.SelectedValue),
           common.myInt( ddlEmployee.SelectedValue), common.myBool(chkApproved.Checked), common.myInt(Session["EmployeeId"]), "A");

            if (SavingStatus.ToUpper().Contains("APPROVE"))
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
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.GetRequestedConfidentialTemplateDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, 0);
            if (ds != null)
            {
                if (common.myInt(ds.Tables.Count) > 0)
                {
                    if (common.myInt(ds.Tables[0].Rows.Count) > 0)
                    {
                        ddlConfidentialTemplateApproval.DataSource = ds.Tables[0];
                        ddlConfidentialTemplateApproval.DataValueField = "RequestedTemplateId";
                        ddlConfidentialTemplateApproval.DataTextField = "RequestedTemplateName";
                        ddlConfidentialTemplateApproval.DataBind();
                    }
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
            objEMR = null;
        }


    }

    protected void ddlTemplateMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.GetRequestedConfidentialTemplateDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ddlConfidentialTemplateApproval.SelectedValue), common.myInt(ddlEmployee.SelectedValue));
            if (ds != null)
            {
                if (common.myInt(ds.Tables.Count) > 1)
                {
                    if (common.myInt(ds.Tables[1].Rows.Count) > 0)
                    {
                        ddlEmployee.DataSource = ds.Tables[1];
                        ddlEmployee.DataValueField = "EmployeeId";
                        ddlEmployee.DataTextField = "EmployeeName";
                        ddlEmployee.DataBind();
                    }
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
            objEMR = null;
        }
    }


}