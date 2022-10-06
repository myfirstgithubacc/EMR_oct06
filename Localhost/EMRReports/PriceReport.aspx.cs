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

public partial class EMRReports_PriceReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowReport();
    }
    protected void ShowReport()
    {
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;

        ReportViewer1.ServerReport.ReportPath = "/"+ reportFolder + "/AppointmentSlip";

        ReportParameter[] p = new ReportParameter[2];

        p[0] = new ReportParameter("intRegistrationId", Request.QueryString["RegistrationId"]);
        p[1] = new ReportParameter("intLoginFacilityId", Session["FacilityId"].ToString());

        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();
    }
}
