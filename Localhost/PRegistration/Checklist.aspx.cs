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
using System.Drawing;
using System.Text;

public partial class PRegistration_Checklist : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog exlog = new clsExceptionLog();
    DAL.DAL dl;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            fillRegistrationChecklist(common.myInt(Request.QueryString["RegistrationId"]));
            BindPatientHiddenDetails(common.myInt(Request.QueryString["RegistrationNo"]));

        }
    }
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);

        if (RegistrationNo > 0)
        {
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();
            lblInfoEncNo.Visible = false;
            lblInfoAdmissionDt.Visible = false;
            lblEncounterNo.Visible = false;
            lblAdmissionDate.Visible = false;

            ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblregNo.Text = RegistrationNo.ToString();
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                    lblMessage.Text = "";

                }
            }

        }
    }
    public void fillRegistrationChecklist(int RegistrationId)
    {
        BaseC.EMRBilling bCEMRBilling = new BaseC.EMRBilling(sConString);
        DataSet ds = bCEMRBilling.getRegistrationChecklist(RegistrationId, common.myStr(Request.QueryString["From"]));
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkList.DataSource = ds;
            chkList.DataTextField = "Description";
            chkList.DataValueField = "Id";
            chkList.DataBind();

            foreach (ListItem li in chkList.Items)
            {
                li.Selected = false;
                ds.Tables[0].DefaultView.RowFilter = "ChecklistId=" + common.myInt(li.Value);
                if (ds.Tables[0].DefaultView.Count > 0)
                {
                    li.Selected = true;
                }
                ds.Tables[0].DefaultView.RowFilter = "";
            }
        }
        else
        {
            chkList.Items.Clear();
            chkList.DataSource = null;

        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);
            string strsave = "";
            //if (common.myInt(Request.QueryString["RegistrationId"]) != 0)
            //{
            #region Checklist XML

            StringBuilder strXMLChecklist = new StringBuilder();
            ArrayList colChecklist = new ArrayList();
            foreach (ListItem li in chkList.Items)
            {
                if (li.Selected == true)
                {
                    colChecklist.Add(common.myStr(li.Value));
                    strXMLChecklist.Append(common.setXmlTable(ref colChecklist));
                }
            }
            #endregion

            if (strXMLChecklist.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select check list !";
                return;
            }

            else
            {
                hdnchecklist.Value = strXMLChecklist.ToString();

            }

            if (common.myStr(Request.QueryString["From"]) == "D")
            {
                BaseC.ATD objATD = new ATD(sConString);
                int i = objATD.SaveDischargeChecklist(common.myInt(Session["HospitalLocationId"]),
                    common.myInt(Session["FacilityId"]), common.myInt(Session["userId"]), common.myInt(Request.QueryString["RegistrationId"]), strXMLChecklist.ToString(), common.myInt(Request.QueryString["EncounterId"]));
            }

            if (common.myStr(Request.QueryString["From"]) == "R" && common.myInt(Request.QueryString["RegistrationId"]) > 0)
            {
                BaseC.Patient objPatient = new Patient(sConString);
                string i = objPatient.SaveRegistrationCheckList(common.myInt(Session["HospitalLocationId"]),
                    common.myInt(Session["FacilityId"]),common.myInt(Request.QueryString["RegistrationId"]), strXMLChecklist.ToString(),common.myInt(Session["userId"]));
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }


}
