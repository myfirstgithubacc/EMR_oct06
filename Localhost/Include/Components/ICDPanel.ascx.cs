using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Data;
using Newtonsoft.Json;

public partial class Include_Components_ICDPanel : System.Web.UI.UserControl
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();
    int _width = 0;

    public System.Web.UI.WebControls.Unit width
    {
        get
        {
            if (_width == 0)
                return System.Web.UI.WebControls.Unit.Pixel(400);

            else
                return System.Web.UI.WebControls.Unit.Pixel(_width);
        }
        set
        {
            if (value == 0)
                _width = 800;
            else
                _width = Convert.ToInt32(value.Value);
        }
    }

    public string PanelName
    {
        get
        {
            return hdnPnl.Value;
        }
        set
        {
            hdnPnl.Value = value;
        }
    }

    public string ICDTextBox
    {
        get
        {
            return hdnICDTextBox.Value;
        }
        set
        {
            hdnICDTextBox.Value = value;
        }
    }

    public string GridClientId
    {
        get
        {
            return rptrICDCodes.ClientID;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //foreach (GridViewRow item in rptrICDCodes.Rows)
            //{
            //    CheckBox chk = (CheckBox)item.FindControl("chkICDCodesz");
            //    chk.Checked = false;
            //}

            rptrICDCodes.Width = System.Web.UI.WebControls.Unit.Pixel(_width);
            getCurrentICDCodes();
        }
    }

    private void getCurrentICDCodes()
    {
        DataSet ds = new DataSet();
        try
        {
            if (Session["EncounterID"] != null && Session["registrationId"] != null)
            {
                //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hshIn = new Hashtable();
                //hshIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"].ToString());
                //hshIn.Add("@intRegistrationId", Session["RegistrationID"].ToString());
                //hshIn.Add("@intEncounterId", Session["EncounterId"].ToString());

                //DataSet ds = objSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshIn);

                System.Net.WebClient client = new System.Net.WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
                APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = 0;
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["encounterid"]);
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
                objRoot.IsChronic = false;
                objRoot.DiagnosisId = 0;


                string inputJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);
                rptrICDCodes.DataSource = ds;
                rptrICDCodes.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}