using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;

public partial class Editor_MedicalIllustration : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //private Hashtable hstInput;
    clsExceptionLog objException = new clsExceptionLog();
    bool SignStatus = false;
    string path = string.Empty;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Login.aspx?Logout=1", false);
        }

        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = common.myStr(Request.QueryString["Mpg"]);
        }
        if (common.myInt(Session["RegistrationId"]).Equals(0) && common.myStr(Request.QueryString["callby"]).Equals(string.Empty))
        {
            Response.Redirect("/Default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {
            ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
            ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);

            if (common.myInt(ViewState["RegistrationId"]).Equals(0))
            {
                ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
            }
            if (common.myInt(ViewState["EncounterId"]).Equals(0))
            {
                ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
            }

            BindPatientIllustrationImages();
            RTF1.EditModes = EditModes.Preview;
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                RTF1.Enabled = false;
            }
            IsCopyCaseSheetAuthorized();
        }
    }

    protected void btnImage_Click(object sender, EventArgs e)
    {

        RadWindow2.NavigateUrl = "../ImageEditor/Annotator.aspx?Page=I";
        RadWindow2.OnClientClose = "GetImageOnClientClose";
        RadWindow2.MinHeight = 650;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Default;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {

        BindPatientIllustrationImages();
    }

    private void BindPatientIllustrationImages()
    {
        hdnImagePath.Value = "";
        string ImageData = string.Empty;
        RTF1.Content = "";
        if (hdnImagePath.Value != "")
        {
            ImageData = "<table><tbody><tr><td><img src='" + hdnImagePath.Value + "' width='300px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>" + RTF1.Content;
        }
        else
        {
            BaseC.EMR emr = new BaseC.EMR(sConString);
            DataSet ds = emr.GetPatientIllustrationImages(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("~/PatientDocuments/" + common.myInt(Session["HospitalLocationId"]) + "/" + common.myInt(ViewState["RegistrationId"]) + "/" + common.myInt(ViewState["EncounterId"]) + "/"));
                if (objDir.Exists)
                {
                    //     string path = "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Request.QueryString["EncId"] != null ? Request.QueryString["EncId"].ToString() : common.myStr(Session["EncounterId"]) + "/";
                    string path = "/PatientDocuments/" + common.myInt(Session["HospitalLocationID"]).ToString() + "/" + common.myInt(ViewState["RegistrationId"]).ToString() + "/" + common.myInt(ViewState["EncounterId"]) + "/";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        hdnImagePath.Value = hdnImagePath.Value + " <table><tbody><tr><td><img src='" + path + ds.Tables[0].Rows[i]["ImageName"].ToString() + "' width='300px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table><br/>";
                    }
                    RTF1.Content = hdnImagePath.Value;
                }
            }
            ds.Dispose();
            emr = null;
        }
        hdnImagePath.Value = "";
    }

    public void IsCopyCaseSheetAuthorized()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsAuthorized = objSecurity.IsCopyCaseSheetAuthorized(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));
        hdnIsCopyCaseSheetAuthorized.Value = common.myStr(IsAuthorized);
    }
}