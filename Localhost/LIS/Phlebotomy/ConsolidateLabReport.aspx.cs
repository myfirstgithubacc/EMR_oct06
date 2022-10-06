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
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;

public partial class LIS_Phlebotomy_ConsolidateLabReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillSessionAndQuesryStirnValue();
            if (common.myInt(Session["UserId"]) == 0)
            {
                Response.Redirect("~/Login.aspx", false);
            }
            BindPatientHiddenDetails();
            if (common.myStr(Request.QueryString["Master"]) == "NO")
            {
                btnClose.Visible = true;
            }
            else
            {
                btnClose.Visible = false;
            }
        }
    }
    void FillSessionAndQuesryStirnValue()
    {
        if (common.myStr(Request.QueryString["RegNo"]) != "")
            ViewState["RegistrationNo"] = common.myStr(Request.QueryString["RegNo"]);
        else
            ViewState["RegistrationNo"] = common.myStr(Session["RegistrationNo"]);

        if (common.myStr(Request.QueryString["RegId"]) != "")
            ViewState["RegistrationId"] = common.myStr(Request.QueryString["RegId"]);
        else
            ViewState["RegistrationId"] = common.myStr(Session["RegistrationId"]);

        if (common.myStr(Request.QueryString["EncId"]) != "")
            ViewState["EncounterId"] = common.myStr(Request.QueryString["EncId"]);
        else
            ViewState["EncounterId"] = common.myStr(Session["EncounterId"]);

        if (common.myStr(Request.QueryString["EncNo"]) != "")
            ViewState["EncounterNo"] = common.myStr(Request.QueryString["EncNo"]);
        else
            ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);
    }
    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            Session["RTF"] = RTF1.Content;
            RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintConsolidateLabReport.aspx";
            RadWindowPopup.Height = 598;
            RadWindowPopup.Width = 900;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.Modal = true;
            //RadWindowPopup.OnClientClose = "OnClientClose";
            RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowPopup.VisibleStatusbar = false;
            RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
    void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        RTF1.Content = "";
        lblMessage.Text = "";
    }
    protected void btnLabResult_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
            {
                RadWindowPopup.NavigateUrl = "/ICM/PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"])
                    + "&AdmissionDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd")
                    + "&OP_IP=I&Master=Blank&From=Ward&Discharge=Summary&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Lab";
                //RadWindowPopup.Height = 500;
                //RadWindowPopup.Width = 1000;
                //RadWindowPopup.Top = 10;
                //RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "wndAddService_OnClientClose";
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Default;
            }
        }
    }
}
