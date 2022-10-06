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

        string sFromDate = Request.QueryString["FDate"].ToString();
        //int wardid=common.myInt(Request.QueryString["WardId"].ToString());
        int WardStnId = common.myInt(Request.QueryString["WardStnId"].ToString());
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;


        if (common.myStr(Request.QueryString["Fb"]) == "FB")
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientDietListFB";
        else
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientDietList";


        ReportParameter[] p = new ReportParameter[8];

        p[0] = new ReportParameter("insFacilityId", Session["FacilityId"].ToString());
        p[1] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
        p[2] = new ReportParameter("FrmDate", sFromDate);
        //p[3] = new ReportParameter("WardId", Convert.ToString(wardid));      
        p[3] = new ReportParameter("WardStationId", Convert.ToString(WardStnId));
        p[4] = new ReportParameter("UHID", (string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));
        p[5] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IpNo"));
        p[6] = new ReportParameter("User_id", sUserName);
        p[7] = new ReportParameter("DietSheetDate", Convert.ToDateTime(sFromDate).Date.ToString("dd/MM/yyyy"));

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
