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
using System.Text;
using System.IO;

public partial class EMRReports_AppointmentSlip : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
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
        int RegId,  AppointmentId=0, FormId = 0;
        BaseC.EMRMasters.Fonts objfonts = new BaseC.EMRMasters.Fonts();
        string Fonts = " font-family:Times New Roman ; font-size:9pt ";

        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;

        Fonts = objfonts.GetFontNameAndSize(common.myInt(Session["HospitalLocationID"]), FormId);
        RegId = common.myInt(Request.QueryString["RegistrationId"]);
        AppointmentId = common.myInt(Request.QueryString["AppointmentId"]);

        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;

        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AppointmentSlipPrintNew";

        ReportParameter[] p = new ReportParameter[3];

        p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
        p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));

        p[2] = new ReportParameter("AppointmentId", common.myStr(AppointmentId));

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
        string filename = common.myStr("AppointmentSlip") + ".pdf";
        Response.AppendHeader("Content-Disposition", "filename=" + filename + "");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();


    }


    

}
