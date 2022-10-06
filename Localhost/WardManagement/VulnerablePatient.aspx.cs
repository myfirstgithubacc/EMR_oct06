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

public partial class WardManagement_ChangeEncounterPatient : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.ATD objatd;
    BaseC.clsEMRBilling baseEBill;
    BaseC.clsLISMaster objLISMaster;
    BaseC.EMRBilling.clsOrderNBill bOrdernBill;
   
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindVulnerableStatus();
            BindVulnerablePatientDetails();
        }
    }


    private void BindVulnerableStatus()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objwd = new WardManagement(); 
        objatd = new BaseC.ATD(sConString);
        ds = objwd.GetVulnerableType();

        ddlVulnerableType.DataSource = ds;
        ddlVulnerableType.DataTextField = "Status";
        ddlVulnerableType.DataValueField = "StatusId";
        ddlVulnerableType.DataBind();
        ddlVulnerableType.Items.Insert(0, new RadComboBoxItem("Select", "0"));
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.WardManagement objwd = new WardManagement();

        try
        {
            string status = string.Empty;
            lblMessage.Text = string.Empty;

            if (common.myInt(ddlVulnerableType.SelectedValue) == 0 && (chkIsVulnerable.Checked))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Status not selected !";
                return;
            }

            status = objwd.SaveVulnerablePatientDetails(common.myInt(Session["EncounterId"]), common.myInt(Session["Hospitallocationid"]), common.myInt(Session["FacilityId"])
                , common.myInt(Session["RegistrationID"]), common.myBool(chkIsVulnerable.Checked), common.myInt(ddlVulnerableType.SelectedValue)
                , common.myInt(Session["UserId"]));

            if (status.ToUpper().Contains("SAVED"))
            {
                lblMessage.Text = status;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindVulnerablePatientDetails()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objwd = new WardManagement();
        objatd = new BaseC.ATD(sConString);
        ds = objwd.GetVulnerablePatientDetails(common.myInt(Session["Hospitallocationid"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]));
        if(common.myInt( ds.Tables.Count)>0)
        {
            if(common.myInt(ds.Tables[0].Rows.Count)>0)
            {
                chkIsVulnerable.Checked = common.myBool(ds.Tables[0].Rows[0]["IsHandleWithCare"]);
                ddlVulnerableType.SelectedIndex = ddlVulnerableType.Items.IndexOf(ddlVulnerableType.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["VulnerableTypeId"])));

            }
        }
       
    }

    protected void chkIsVulnerable_CheckedChanged(Object sender, EventArgs args)
    {
        if(!chkIsVulnerable.Checked)
        {
            ddlVulnerableType.SelectedIndex = 0;
            ddlVulnerableType.Enabled = false;
        }
        else
        {
            ddlVulnerableType.Enabled = true;
        }
      
    }


}
