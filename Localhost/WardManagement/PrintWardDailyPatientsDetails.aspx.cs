using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.IO;
public partial class WardManagement_PrintWardDailyPatientsDetails : System.Web.UI.Page
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

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);

        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;

        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        ReportParameter[] p;
        string wid = common.myStr(Request.QueryString["WID"]);
        string chk= common.myStr(Request.QueryString["CHK"]);
        switch (common.myStr(Request.QueryString["PT"]))
        {

            case "W":
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDailyPatientsDetails";
                p = new ReportParameter[3];
                p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intWardId", wid);
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
                
                if (Request.QueryString["preview"] != null)
                {
                    if (common.myStr(Request.QueryString["preview"]) == "PP")
                    {
                        return;
                    }
                }
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
                break;
            case "E":
                //ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDailyPatientsDetails";
                //p = new ReportParameter[2];
                //p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                //p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));

                //ReportViewer1.ServerReport.SetParameters(p);
                //ReportViewer1.ServerReport.Refresh();

                //if (Request.QueryString["preview"] != null)
                //{
                //    if (common.myStr(Request.QueryString["preview"]) == "PP")
                //    {
                //        return;
                //    }
                //}
                //ReportViewer1.ServerReport.SetParameters(p);
                //ReportViewer1.ServerReport.Refresh();
                //break;


                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDietPatientsDetails";
                p = new ReportParameter[3];
                p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intWardId", wid);
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                if (Request.QueryString["preview"] != null)
                {
                    if (common.myStr(Request.QueryString["preview"]) == "PP")
                    {
                        return;
                    }
                }
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
                break;
        }
        if(chk.Equals("F"))
        {
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
}