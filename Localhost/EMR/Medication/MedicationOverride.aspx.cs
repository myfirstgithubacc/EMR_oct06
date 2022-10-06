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

public partial class EMR_Medication_MedicationOverride : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objException = new clsExceptionLog();

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            chkOverride.Checked = (common.myInt(Request.QueryString["IsO"]) == 1) ? true : false;
            hdnOverrideComments.Value = common.myStr(Request.QueryString["OC"]);
            hdnDrugAllergyScreeningResult.Value = common.myStr(Request.QueryString["DASR"]);

            txtOverrideComments.Text = common.myStr(Request.QueryString["OC"]);
            txtDrugAllergyScreeningResult.Text = common.myStr(Request.QueryString["DASR"]);

            if (common.myStr(Request.QueryString["DASR"]).Trim().Length == 0)
            {
                if(Request.QueryString["D"]!=null || Request.QueryString["G"] != null)
                    {
                    if(common.myStr(Request.QueryString["D"])!=String.Empty )
                    { 
                hdnDrugAllergyScreeningResult.Value = "Patient is allergic to this drug - " + common.myStr(Request.QueryString["D"]);
                txtDrugAllergyScreeningResult.Text = "Patient is allergic to this drug - " + common.myStr(Request.QueryString["D"]);
                    }
                    else if(common.myStr(Request.QueryString["G"]) != String.Empty)
                    {
                        hdnDrugAllergyScreeningResult.Value = "Patient is allergic to this drug - " + common.myStr(Request.QueryString["G"]);
                        txtDrugAllergyScreeningResult.Text = "Patient is allergic to this drug - " + common.myStr(Request.QueryString["G"]);

                    }
                }
            
            }
        }
    }

    protected void btnAddToPrescription_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            if (!chkOverride.Checked)
            {
                lblMessage.Text = "Please checked override checkbox !";
                return;
            }
            if (chkOverride.Checked && common.myStr(txtOverrideComments.Text).Trim().Length == 0)
            {
                lblMessage.Text = "Override comments can't be blank !";
                return;
            }
            if (!chkOverride.Checked && common.myStr(txtOverrideComments.Text).Trim().Length > 0)
            {
                lblMessage.Text = "Please checked override checkbox !";
                return;
            }

            hdnIsOverride.Value = chkOverride.Checked ? "1" : "0";
            hdnOverrideComments.Value = common.myStr(txtOverrideComments.Text).Trim();
            hdnDrugAllergyScreeningResult.Value = common.myStr(txtDrugAllergyScreeningResult.Text).Trim();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            hdnIsOverride.Value = chkOverride.Checked ? "1" : "0";
            hdnOverrideComments.Value = common.myStr(txtOverrideComments.Text).Trim();
            hdnDrugAllergyScreeningResult.Value = common.myStr(txtDrugAllergyScreeningResult.Text).Trim();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}