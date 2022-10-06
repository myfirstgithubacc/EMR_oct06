using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_Dashboard_ViewDoctorProgressNote : System.Web.UI.Page
{
    //private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //StringBuilder strXMLDrug;
    //StringBuilder strXMLOther;
    bool Confirm_value = false;
    //clsExceptionLog objException = new clsExceptionLog();
    private string DellBoomi = System.Configuration.ConfigurationManager.AppSettings["AFG_DellBoomi"].ToString();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails_1.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            loadData();
        }
    }

    public void loadData()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        try
        {
           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDoctorProgressNoteForSingleScreen";
            APIRootClass.GetDoctorProgressNote objRoot = new global::APIRootClass.GetDoctorProgressNote();
            objRoot.DoctorId = 0;
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.ProgressNoteId = common.myInt(Request.QueryString["ProgressNoteId"]);
            //string strtriageID = string.Empty;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if(ds.Tables.Count>0)
            {
                if(ds.Tables[0].Rows.Count>0)
                {
                    lblDoctor.Text = common.myStr(ds.Tables[0].Rows[0]["DoctorName"]);
                    lblLastEncounterDate.Text = "Note Date : " + common.myStr(ds.Tables[0].Rows[0]["EnteredDate"]);
                    lblDetails.Text= common.myStr(ds.Tables[0].Rows[0]["ProgressNote"]);
                }
            }
        }

        catch (Exception Ex)
        {
           
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            client = null;
            //objEMR = null;
            //HshOut = null;
        }

    }
}