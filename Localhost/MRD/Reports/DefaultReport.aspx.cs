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
using Microsoft.Reporting.WebForms;
using System.IO;


public partial class MRD_Reports_DefaultReport : System.Web.UI.Page
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
        if (Request.QueryString["CF"] != null)
        {
            if (Request.QueryString["CF"] == "IPStat")
            {
                String sRptName = "";
                ReportParameter[] p = new ReportParameter[9];

                p = new ReportParameter[9];
                sRptName = "AdmissionDischargeNDeathStat";
                p[0] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("iFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("dFromDate", common.myStr(Request.QueryString["fdt"].ToString()));
                p[3] = new ReportParameter("dToDate", common.myStr(Request.QueryString["tdt"].ToString()));
                p[4] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[5] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[6] = new ReportParameter("intStoreId", "0");
                p[7] = new ReportParameter("cReportType", common.myStr(Request.QueryString["rtyp"].ToString()));
                if (Request.QueryString["rtyp"] == "D")
                    p[8] = new ReportParameter("rptHeader", "In-Patient Statistics(Doctor Wise)");
                else if (Request.QueryString["rtyp"] == "S")
                    p[8] = new ReportParameter("rptHeader", "In-Patient Statistics(Department Wise)");
                else if (Request.QueryString["rtyp"] == "A")
                    p[8] = new ReportParameter("rptHeader", "Admission Register");


                PrintReport(sRptName, p);
            }
        }
    }
    private void PrintReport(string sReportName, ReportParameter[] para)
    {

        string ReportServerPath = "http://" + reportServer + "/ReportServer";
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;

        ReportViewer1.ShowParameterPrompts = false;

        ReportViewer1.ShowBackButton = true;
        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + sReportName;
        ReportViewer1.ServerReport.SetParameters(para);

        ReportViewer1.ServerReport.Refresh();



        string[] streamids = null;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string mimeType;
        string encoding;
        string extension;
        //string deviceInfo = string.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.7in", "8.3in");
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);

        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
        byte[] bytes = this.ReportViewer1.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        MemoryStream oStream = new MemoryStream(); ;
        oStream.Write(bytes, 0, bytes.Length);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();

    }
}
