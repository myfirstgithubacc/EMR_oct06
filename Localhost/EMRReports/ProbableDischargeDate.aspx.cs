using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.IO;

public partial class EMRReports_ProbableDischargeDate : System.Web.UI.Page
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
       string UserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        string ExpectedDateOfDischarge = Request.QueryString["ExpectedDateOfDischarge"].ToString();
     
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;


    
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProbableDischargeDate";
    


        ReportParameter[] p = new ReportParameter[4];

        p[0] = new ReportParameter("intUserId", common.myStr(Session["UserID"]));
        p[1] = new ReportParameter("intFacilityId",common.myStr(Session["FacilityId"]));
        p[2] = new ReportParameter("ExpectedDateOfDischarge", ExpectedDateOfDischarge);
        p[3] = new ReportParameter("UserName", UserName);



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