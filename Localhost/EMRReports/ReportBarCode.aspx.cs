using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class EMRReports_ReportBarCode : System.Web.UI.Page
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
        //if (Request.QueryString["ReportName"] != null && Request.QueryString["ReportName"].ToString() == "AdmissionBarCodeLabel")
        if (Request.QueryString["ReportName"] != null && (Request.QueryString["ReportName"].ToString() == "AdmissionBarCodeLabel" || Request.QueryString["ReportName"].ToString() == "BloodBarCodeLabel"))
        {
            string ReportServerPath = "http://" + reportServer + "/ReportServer";

            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;

            ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;

            if (Request.QueryString["ReportName"].ToString() == "BloodBarCodeLabel")
            {
             ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BloodBarCodeLabel";
             ReportParameter[] p = new ReportParameter[2];
             p[0] = new ReportParameter("Encno", Request.QueryString["EncNo"].ToString());
             p[1] = new ReportParameter("RequsitionID", Request.QueryString["RequsitionID"].ToString());

             ReportViewer1.ServerReport.SetParameters(p);

            }
            else
            {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionBarCodeLabel";
            ReportParameter[] p = new ReportParameter[2];
            p[0] = new ReportParameter("Encno", Request.QueryString["EncNo"].ToString());
            p[1] = new ReportParameter("intLoginFacilityId", Convert.ToString(Session["FacilityId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            }


            
            
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
}
