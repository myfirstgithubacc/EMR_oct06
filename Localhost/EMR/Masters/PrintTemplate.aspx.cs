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
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;


public partial class EMR_Masters_PrintTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowReport();
    }
    protected void ShowReport()
    {
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/TemplatePrint";

        ReportParameter[] p = new ReportParameter[3];
        p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
        p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
        p[2] = new ReportParameter("intTemplateId", common.myStr(Request.QueryString["TemplateId"]));
        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();
    }
}
