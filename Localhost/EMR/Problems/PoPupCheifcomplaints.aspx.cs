using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
public partial class EMR_Problems_PoPupCheifcomplaints : System.Web.UI.Page
{
   // private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //clsExceptionLog objException = new clsExceptionLog();
    //BaseC.EMRProblems objbc2;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    

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
           
            if (common.myStr(Request.QueryString["ProblemId"]) != "")
            {
                lblproblem.Text = common.myStr(Request.QueryString["PName"].ToString());
                BindProblem();
            }

        }
    }
    void BindProblem()
    {
        //objbc2 = new BaseC.EMRProblems(sConString);
        DataSet ds = new DataSet();

        //ds = objbc2.GetChiefProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Request.QueryString["FacilityId"]), common.myInt(Session["RegistrationId"]), 
        //    common.myInt(Request.QueryString["DoctorID"]), "", "", "", "%%", false, common.myBool(Request.QueryString["Chronic"]), common.myInt(Request.QueryString["ProblemId"]), "A");


        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetChiefProblem";
        APIRootClass.GetChiefProblem objRoot = new global::APIRootClass.GetChiefProblem();
        objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
        objRoot.FacilityId = common.myInt(Session["FacilityId"]);
        objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
        objRoot.DoctorId = common.myInt(Request.QueryString["DoctorID"]);
        objRoot.Daterange = "";
        objRoot.FromDate = "";
        objRoot.ToDate = "";
        objRoot.SearchCriteriya = "%%";
        objRoot.IsDistinct = false;
        objRoot.IsChronic = common.myBool(Request.QueryString["Chronic"]);
        objRoot.ProblemId = common.myInt(Request.QueryString["ProblemId"]);
        objRoot.VisitType = "A";

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
       // sValue = JsonConvert.DeserializeObject<string>(sValue);

        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblLocation.Text = common.myStr(ds.Tables[0].Rows[0]["Location"].ToString());
            lblSide.Text = common.myStr(ds.Tables[0].Rows[0]["SideDescription"].ToString());
            lblOnset.Text = common.myStr(ds.Tables[0].Rows[0]["OnSet"].ToString());
            lblDuration.Text = common.myStr(ds.Tables[0].Rows[0]["Duration"].ToString());
            lblQuality.Text = common.myStr(ds.Tables[0].Rows[0]["QualityName"].ToString());
            lblContext.Text = common.myStr(ds.Tables[0].Rows[0]["Context"].ToString());
            lblSeverity.Text = common.myStr(ds.Tables[0].Rows[0]["Severity"].ToString());
            lblCondition.Text = common.myStr(ds.Tables[0].Rows[0]["Condition"].ToString());
            if (common.myInt(ds.Tables[0].Rows[0]["Percentage"].ToString()) == 0)
                lblPercentage.Text = "";
            else
            lblPercentage.Text = common.myStr(ds.Tables[0].Rows[0]["Percentage"].ToString());

            if (common.myStr(ds.Tables[0].Rows[0]["Chronic"].ToString()) == "Y")
                lblChronics.Text = "Yes";
            else
                lblChronics.Text = common.myStr(ds.Tables[0].Rows[0]["Chronic"].ToString());

            if (common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem1"].ToString()) == "")
            {
                lblAssociatedProblem1.Text = "";
            }
            else
            {
                lblAssociatedProblem1.Text = common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem1"].ToString());
            }
            if (common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem2"].ToString()) == "")
            {
                lblAssociatedProblem2.Text = "";
            }
            else
            {
                lblAssociatedProblem2.Text = common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem2"].ToString());
            }
            if (common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem3"].ToString()) == "")
            {
                lblAssociatedProblem3.Text = "";
            }
            else
            {
                lblAssociatedProblem3.Text = common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem3"].ToString());
            }
            if (common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem4"].ToString()) == "")
            {
                lblAssociatedProblem4.Text = "";
            }
            else
            {
                lblAssociatedProblem4.Text = common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem4"].ToString());
            }
            if (common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem5"].ToString()) == "")
            {
                lblAssociatedProblem5.Text = "";
            }
            else 
            {
                lblAssociatedProblem5.Text = common.myStr(ds.Tables[0].Rows[0]["AssociatedProblem5"].ToString());
            }



        }

    }
}
