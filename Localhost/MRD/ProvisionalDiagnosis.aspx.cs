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
using System.Text;
using System.Xml;
using System.Drawing;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_Assessment_ProvisionalDiagnosis : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    
         Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        
    
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDiagnosisSearchCode();
            BindPatientProvisionalDiagnosis();
            txtProvisionalDiagnosis.ReadOnly = true;
            ddlDiagnosisSearchCodes.Enabled = false;  
        }
    }

    protected void BindPatientProvisionalDiagnosis()
    {
        try
        {
            DataSet ds;
            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            ds = new DataSet();
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
                txtProvisionalDiagnosis.ReadOnly = true;   
                ddlDiagnosisSearchCodes.SelectedValue = ds.Tables[0].Rows[0]["DiagnosisSearchId"].ToString();
                ddlDiagnosisSearchCodes.Enabled = false;  
                lblEncodedBy.Text = ds.Tables[0].Rows[0]["EncodedBy"].ToString();
                lblEncodedDate.Text = ds.Tables[0].Rows[0]["EncodedDate"].ToString();
                lblLastChangedBy.Text = ds.Tables[0].Rows[0]["LastChangedBy"].ToString();
                lblLastChangedDate.Text = ds.Tables[0].Rows[0]["LastChangedDate"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindDiagnosisSearchCode()
    {
        try
        {
            DataSet ds;

            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "DIAGNOSIS");

            ddlDiagnosisSearchCodes.Items.Clear();
            ddlDiagnosisSearchCodes.DataSource = ds;
            ddlDiagnosisSearchCodes.DataValueField = "Id";
            ddlDiagnosisSearchCodes.DataTextField = "DiagnosisSearchCode";
            ddlDiagnosisSearchCodes.DataBind();
            ddlDiagnosisSearchCodes.Items.Insert(0, new RadComboBoxItem("Select", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }




    protected void btnGetDiagnosisSearchCodes_Click(object sender, EventArgs e)
    {
        BindDiagnosisSearchCode();
    }


    
}
