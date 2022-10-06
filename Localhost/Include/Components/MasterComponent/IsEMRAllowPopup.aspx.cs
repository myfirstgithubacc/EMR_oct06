using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using Telerik.Web.UI;
using BaseC;

public partial class Include_Components_MasterComponent_IsEMRAllowPopup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsEMRBilling baseEBill;
    ManageInsurance miObj = new ManageInsurance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string RegistrationID = Request.QueryString["RegistrationID"].ToString();
            string Encounterid = Request.QueryString["Encounterid"].ToString();
            ViewState["RegistrationID"] = RegistrationID;
            ViewState["Encounterid"] = Encounterid;
            bindPatientDetails();
        }
    }

    protected bool isSave()
    {
        try
        {
            lblMessage.Text =string.Empty;  
            bool isValid = true;

            if (txtRemark.Text.Trim().Equals(string.Empty))
            {
                lblMessage.Text = "Remark can't be blank !";
                isValid = false;
                return isValid;
            }
            
            return isValid;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return false;
        }
    }
    private void bindPatientDetails()
    {
        
        BaseC.Patient bC = new BaseC.Patient(sConString);
        string RegistrationNo = common.myStr(Request.QueryString["RegistrationNo"].ToString());
        string EncounterNo = common.myStr(Request.QueryString["EncounterNo"].ToString());
        string sRegNoTitle = Resources.PRegistration.regno;
        DataSet dsPatientDetail = new DataSet();
        try
        {
            dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), RegistrationNo,
                    EncounterNo, common.myInt(Session["UserId"]), 0);
            if (dsPatientDetail.Tables.Count > 0 && dsPatientDetail.Tables[0].Rows.Count > 0)
            {
                lblPatientDetail.Text = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                     + "&nbsp;Enc #:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>";

            }
            else
            {
                lblPatientDetail.Text = string.Empty;
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
            bC = null;
            RegistrationNo = string.Empty;
            EncounterNo = string.Empty;
            sRegNoTitle = string.Empty;
            dsPatientDetail.Dispose();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        EMRMasters objEMRMasters = new BaseC.EMRMasters(sConString);
        Hashtable htOut = new Hashtable();
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            lblMessage.Text = string.Empty;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSave())
            {
                return;
            }
            htOut = objEMRMasters.SaveEMRFileRequest(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["RegistrationID"]), common.myInt(ViewState["Encounterid"]), common.myStr(txtRemark.Text.Trim()), Convert.ToInt32(Session["UserId"]));

            string strMsg = common.myStr(htOut["@chvErrorStatus"]);

            if (strMsg.Contains("Succeeded"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Thank You , Your Request Has Been Sent Successfully.";
                txtRemark.Text = string.Empty;
            }
            else
            {
                lblMessage.Text = strMsg;
                txtRemark.Text = string.Empty;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

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
            objEMRMasters = null;
            htOut = null;
            objEMR = null;
        }
    }

    protected void linkSendtoCEO_Click(object sender, EventArgs e)
    {
        txtRemark.Text = ""; 
        btnSave.Visible = true; 
    }
}