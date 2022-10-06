using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;

public partial class EMR_Assessment_PopupDiagnosisdetails : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //clsExceptionLog objException = new clsExceptionLog();
    
    //BaseC.DiagnosisDA objDiag;
    //DataSet ds;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         
            if (common.myStr(Request.QueryString["DiagId"]) != "")
            {
                lblDiagnosisname.Text = common.myStr(Request.QueryString["DName"].ToString());
                BindDiagnosis();
            }

        }
    }
    void BindDiagnosis()
    {
        //objDiag = new BaseC.DiagnosisDA(sConString);
        DataSet ds = new DataSet();
        //ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["RegistrationId"]), 0, 0, 0, 0, "", "", "", "%%", false, 0, "", common.myBool(Request.QueryString["Chronic"]), common.myInt(Request.QueryString["DiagId"]));
        try
        {

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
            APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = 0;
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = 0;
            objRoot.DoctorId = 0;
            objRoot.DiagnosisGroupId = 0;
            objRoot.DiagnosisSubGroupId = 0;
            objRoot.DateRange = "";
            objRoot.FromDate = "";
            objRoot.ToDate = "";
            objRoot.SearchKeyword = "%%";
            objRoot.IsDistinct = false;
            objRoot.StatusId = 0;
            objRoot.VisitType = "";
            objRoot.IsChronic = common.myBool(Request.QueryString["Chronic"]);
            objRoot.DiagnosisId = common.myInt(Request.QueryString["DiagId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (common.myStr(ds.Tables[0].Rows[0]["Chronic"]) == "Y")
            {
                lblChronic.Text = "Yes";
            }
            else
            {
                lblChronic.Text = "";
            }
            lblICDCode.Text = ds.Tables[0].Rows[0]["ICDCode"].ToString().Trim();
            lblOnsetDate.Text = ds.Tables[0].Rows[0]["OnsetDate"].ToString().Trim();
            lblLocation.Text = ds.Tables[0].Rows[0]["Location"].ToString().Trim();
            lblType.Text = ds.Tables[0].Rows[0]["TypeName"].ToString().Trim();
            lblCondition.Text = ds.Tables[0].Rows[0]["Conditions"].ToString().Trim();
            if (common.myStr(ds.Tables[0].Rows[0]["IsResolved"]) == "False")
            {
                lblPrimary.Text = "";
            }
            else
            {
                lblPrimary.Text = "Yes";
            }

            if (common.myStr(ds.Tables[0].Rows[0]["IsResolved"]) == "False")
            {
                lblResolved.Text = "";

            }
            else
            {
                lblResolved.Text = "Yes";
            }

            lblRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString().Trim();
        }
        catch(Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
}
