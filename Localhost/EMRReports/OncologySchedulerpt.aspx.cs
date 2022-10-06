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


public partial class EMRReports_OncologySchedulerpt : System.Web.UI.Page
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
        String sRptName = "";
        if (common.myStr(Request.QueryString["Schedulerpt"]).ToUpper().Equals("SCHEDULE"))
        {
            sRptName = "rptOncologySchedule";
            ReportParameter[] p = new ReportParameter[2];
            p[0] = new ReportParameter("RegistrationId", Request.QueryString["RegId"]);
            //p[1] = new ReportParameter("EncounterId", Request.QueryString["Encounter"]);
            p[1] = new ReportParameter("EncounterId", "0");
            PrintReport(sRptName, p);
        }
        if (common.myStr(Request.QueryString["Schedulerpt"]).ToUpper().Equals("REGISTER"))
        {
            sRptName = "rptOncologyScheduleRegister";
            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("RegistrationId", Request.QueryString["RegId"]);
            p[1] = new ReportParameter("EncounterId", Request.QueryString["Encounter"]);
            p[2] = new ReportParameter("fdate", Request.QueryString["fromDate"]);
            p[3] = new ReportParameter("tdate", Request.QueryString["toDate"]);
            p[4] = new ReportParameter("chvName", Request.QueryString["cvName"]);
            p[5] = new ReportParameter("chvRegistrationNo", Request.QueryString["Regno"]);
            PrintReport(sRptName, p);
        }

    }

    private void PrintReport(string sReportName, ReportParameter[] para)
    {
        try
        {
            string ReportServerPath = "http://" + reportServer + "/ReportServer";
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + sReportName;
            ReportViewer1.ServerReport.SetParameters(para);
            ReportViewer1.ServerReport.Refresh();

            if (Request.QueryString["preview"] != null)
            {
                if (common.myStr(Request.QueryString["preview"]) == "PP" || common.myStr(Request.QueryString["preview"]) == "EX")
                {
                    return;
                }
            }
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

            MemoryStream oStream = new MemoryStream();
            oStream.Write(bytes, 0, bytes.Length);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();

        }
        catch (Exception ex)
        {
         
        }
    }
}