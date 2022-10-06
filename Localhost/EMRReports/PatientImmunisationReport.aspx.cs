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

public partial class Diet_Report_PatientDietlistreport : System.Web.UI.Page
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
            showreport();

        }
    }
    protected void showreport()
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

        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ImmunisationDashboard";


        ReportParameter[] p = new ReportParameter[11];


        //        --@intHospitalLocationId TINYINT = 1,
        //--@intFacilityId INT = 0,
        //--@chvRegistrationNo VARCHAR(20) = ' ', 
        //--@chvName VARCHAR(50) = ' ', 
        //--@chvMobileNo VARCHAR(50) = ' ',  
        //--@chvMotherName VARCHAR(50) = ' ',  
        //--@chvDateRange VARCHAR(10) = ' ',          
        //--@chrFromDate CHAR(10) = '1900-01-01 00:00', --Format - '1900-01-01 00:00'
        //--@chrToDate CHAR(10) = '2079-01-01 00:00', --Format - '2079-01-01 00:00'
        //--@chrStatus CHAR(10) = ' '

        p[0] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
        p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
        p[2] = new ReportParameter("chvRegistrationNo", common.myStr(Request.QueryString["RegistrationNo"]));
        p[3] = new ReportParameter("chvName", common.myStr(Request.QueryString["Name"]));
        p[4] = new ReportParameter("chvMobileNo", common.myStr(Request.QueryString["MobileNo"]));
        p[5] = new ReportParameter("chvMotherName", common.myStr(Request.QueryString["MotherName"]));
        p[6] = new ReportParameter("chvDateRange", common.myStr(Request.QueryString["DateRange"]));
        p[7] = new ReportParameter("chrFromDate", common.myStr(Request.QueryString["FromDate"]));
        p[8] = new ReportParameter("chrToDate", common.myStr(Request.QueryString["ToDate"]));
        p[9] = new ReportParameter("chrStatus", common.myStr(Request.QueryString["Status"]));
        if (common.myLen(Request.QueryString["VisitType"]) > 0)
        {
            p[10] = new ReportParameter("ChrVisitType", common.myStr(Request.QueryString["VisitType"]));
        }
        else
        {
            p[10] = new ReportParameter("ChrVisitType", " ");

        }
        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();
        //  lblMessage.Text = "";  ="Printed at: "+format(Globals!ExecutionTime,"dd/MM/yyyy HH:mm")

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
