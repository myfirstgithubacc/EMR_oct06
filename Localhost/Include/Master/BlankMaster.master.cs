using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class Include_BlankMaster : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if (!Page.IsCallback)
            {
                if (Session["StrO"] == null)
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Session Expired", false);
                    return;
                }
                else if (Session["StrO"] != null && common.myStr(Request.QueryString["irtrf"]) != "" && common.myStr(Session["StrO"]) != common.myStr(Request.QueryString["irtrf"]))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Invalid URL", false);
                }
                else if (Session["StrO"] != null)
                {
                    string output = "";
                    BaseC.User usr = new BaseC.User(sConString);
                    usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                    if (output.Contains("Expired") || output.Contains("Invalid"))
                    {
                        Session["UserID"] = null;
                        Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                        return;
                    }
                    usr = null;
                }
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                    return;
                }
            }
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }
            if (Request.QueryString["EncId"] != null)
            {
                Session["EncounterId"] = Request.QueryString["EncId"];
            }
            if (Request.QueryString["mdlId"] != null)
            {
                Session["ModuleId"] = Request.QueryString["mdlId"];
            }
            if (common.myStr(Request.QueryString["irtrf"]) != ""
                && Request.QueryString["OP"] != null && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
            {
                Session["irtrf"] = null;
                Session["IsAdminGroup"] = null;
                Session["LoginIsAdminGroup"] = null;
                Session["HospitalLocationID"] = null;
                Session["FacilityID"] = null;
                Session["GroupID"] = null;
                Session["FinancialYearId"] = null;
                Session["EntrySite"] = null;
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["ModuleId"] = null;
                Session["URLPId"] = null;


                Session["irtrf"] = Request.QueryString["irtrf"];
                Session["IsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[0]);
                Session["LoginIsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[1]);
                Session["HospitalLocationID"] = common.myStr(Request.QueryString["OP"].Split('_')[2]);
                Session["FacilityID"] = common.myStr(Request.QueryString["OP"].Split('_')[3]);
                Session["GroupID"] = common.myStr(Request.QueryString["OP"].Split('_')[4]);
                Session["FinancialYearId"] = common.myStr(Request.QueryString["OP"].Split('_')[5]);
                Session["EntrySite"] = common.myStr(Request.QueryString["OP"].Split('_')[6]);
                Session["UserID"] = common.myStr(Request.QueryString["OP"].Split('_')[7]);
                Session["UserName"] = common.myStr(Request.QueryString["OP"].Split('_')[8]).Replace("%", " ");
                Session["ModuleId"] = common.myStr(Request.QueryString["OP"].Split('_')[9]);
                Session["URLPId"] = common.myStr(Request.QueryString["OP"].Split('_')[10]);



                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 11)
                    Session["FacilityName"] = common.myStr(Request.QueryString["OP"]).Split('_')[11].Replace("%", " ");
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 12)
                    Session["CanDownloadPatientDocument"] = common.myStr(Request.QueryString["OP"]).Split('_')[12];
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 13)
                    Session["FacilityStateId"] = common.myStr(Request.QueryString["OP"]).Split('_')[13];

            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "PageInit");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
