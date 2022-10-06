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
using System.IO;

public partial class EMRReports_Patientdemographics : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ShowReport();
        }

    }
    protected void ShowReport()
    {
        string Pageid = Request.QueryString["Pid"].ToString();
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));



        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        if (Pageid == "Demographic")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationDemographics";
        }

        if (Pageid == "Guarantor")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationGuarantor";
        }
        if (Pageid == "Otherdetails")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationOtherdetails";
        }

        if (Pageid == "Contacts")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationContactDetails";
        }
        if (Pageid == "Payer")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationPayer";
        }
        if (Pageid == "DemographicLabel")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationLable";
        }


        ReportParameter[] p = null;
        if (Pageid != "DemographicLabel")
        {

            if (Pageid == "Demographic")
                p = new ReportParameter[4];

            else
                p = new ReportParameter[3];

            p[0] = new ReportParameter("intRegistrationId", Request.QueryString["RegistrationId"].ToString());

            if (Pageid == "Demographic")
            {
                p[1] = new ReportParameter("UHID", common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "UHID").ToString()));
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
            }

        }
        else
        {
            p = new ReportParameter[1];
            p[0] = new ReportParameter("EncounterId", Request.QueryString["RegistrationId"].ToString());
        }

        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();


        string[] streamids = null;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string mimeType;
        string encoding;
        string extension;

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));



        byte[] bytes = this.ReportViewer1.ServerReport.Render(
              "PDF", null, out mimeType, out encoding,
               out extension,
              out streamids, out warnings);
        MemoryStream oStream = new MemoryStream(); ;
        oStream.Write(bytes, 0, bytes.Length);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();
    }

}
