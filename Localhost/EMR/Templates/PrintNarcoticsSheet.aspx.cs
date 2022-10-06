using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class EMR_Templates_PrintNarcoticsSheet : System.Web.UI.Page
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
    {   //Pra     
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;

        ReportParameter[] p;
        string IndentId = Convert.ToString(Request.QueryString["IndentId"]);
        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/NarcoticPrescriptionReport";
        p = new ReportParameter[3];
        p[0] = new ReportParameter("intEncounterId", common.myInt(Session["EncounterId"]).ToString());
        p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
        p[2] = new ReportParameter("intIndentId", common.myInt(IndentId).ToString());
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
        string filename = common.myStr(Request.QueryString["ReportName"]) + ".pdf";
        Response.AppendHeader("Content-Disposition", "filename=" + filename + "");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();
    }
}