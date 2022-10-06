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
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System.IO;

public partial class LIS_Phlebotomy_PrintInvestigationLabelsReport : System.Web.UI.Page
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
            this.Title = "Investigation Labels";
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ShowReport();
    }

    protected void ShowReport()
    {
        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));
        string ReportServerPath = "http://" + reportServer + "/ReportServer";


        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        ReportParameter[] p = null;
        if (common.myStr(Request.QueryString["STATION"]) != "RIS")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/LISLabel";
            p = new ReportParameter[4];
            p[0] = new ReportParameter("chrSource", common.myStr(Request.QueryString["SOURCE"]));
            p[1] = new ReportParameter("intLabNo", common.myStr(Request.QueryString["LABNO"]));
            p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Request.QueryString["LoginFacilityId"]));
            p[3] = new ReportParameter("chvDiagSampleId", common.myStr(Request.QueryString["DiagSampleId"]));
           //Crystal Lable
            //ReportDocument crystalReport = new ReportDocument();
            //crystalReport.Load(Server.MapPath("/LIS/Report/InvestigationLabels.rpt"));
            //crystalReport.SetParameterValue("@chrSource", common.myStr(Request.QueryString["SOURCE"]));
            //crystalReport.SetParameterValue("@intLabNo", common.myInt(Request.QueryString["LABNO"]));
            //crystalReport.SetParameterValue("@intLoginFacilityId", common.myInt(Request.QueryString["LoginFacilityId"]));
            //crystalReport.SetParameterValue("@intDiagSampleId", common.myInt(Request.QueryString["DiagSampleId"])); //Daily Serial No
            //CrystalReportViewer1.DisplayGroupTree = false;
            //CrystalReportViewer1.ReportSource = crystalReport;
            
            //MemoryStream oStream;
            //oStream = (MemoryStream)crystalReport.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();


            //SSRS Lable with Report Viewer
            //ReportViewer1.ServerReport.SetParameters(p);
            //ReportViewer1.ServerReport.Refresh();
            //ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            //ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
            //ReportViewer1.Visible = true;

            //SSRS Lable with PDF
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
            //Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RISLabel";
            p = new ReportParameter[4];
            p[0] = new ReportParameter("chrSource", common.myStr(Request.QueryString["SOURCE"]));
            p[1] = new ReportParameter("intLabNo", common.myStr(Request.QueryString["LABNO"]));
            p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Request.QueryString["LoginFacilityId"]));
            p[3] = new ReportParameter("intDiagSampleId", common.myStr(Request.QueryString["DiagSampleId"]));
            //////////////////*******CHANGED FOR LIS
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
            //Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
       
    }

}
