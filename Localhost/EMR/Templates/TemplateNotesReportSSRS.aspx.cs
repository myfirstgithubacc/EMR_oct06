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

public partial class EMR_Templates_TemplateNotesReportSSRS : System.Web.UI.Page
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
        int doctorId = 0;

        string IsPrintLoginDoctorSignatureInEMRTemplate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]), "IsPrintLoginDoctorSignatureInEMRTemplate", sConString);

        if (IsPrintLoginDoctorSignatureInEMRTemplate.Equals("Y"))
        {
            if (common.myBool(Session["IsLoginDoctor"]))
            {
                doctorId = common.myInt(Session["EmployeeId"]);
            }
            else
            {
                doctorId = 0;
            }
        }

        string ReportServerPath = "http://" + reportServer + "/ReportServer";
        string sRptName = "InvestigationResult";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        //ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PurchaseOrderReport";

        //  ReportViewer1.ServerReport.ReportPath = "/EMRReports/SickLeaveFinal";

        if (common.myInt(Request.QueryString["TemplateId"]) > 0)
        {
            ReportParameter[] p = new ReportParameter[6];
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptConsentForms";

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
            p[2] = new ReportParameter("intRegistrationId", common.myInt(Session["RegistrationId"]).ToString());
            p[3] = new ReportParameter("intEncounterId", common.myInt(Session["EncounterId"]).ToString());
            p[4] = new ReportParameter("intTemplateId", common.myInt(Request.QueryString["TemplateId"]).ToString());
            p[5] = new ReportParameter("intDoctorId", common.myInt(doctorId).ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

            //  PrintReport(sRptName, p);

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
                if (common.myStr(Request.QueryString["preview"]) == "PP")
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

            //if (common.myStr(Request.QueryString["EMail"]).Trim().Length > 3)
            //{
            //    EMRMasters.EMRFacility objf = new EMRMasters.EMRFacility(sConString);
            //    DataSet ds = objf.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            DataView dv = ds.Tables[0].DefaultView;
            //            dv.RowFilter = "FacilityId = " + common.myStr(Session["FacilityID"]);
            //            if (dv.Count > 0)
            //            {
            //                DataTable dt = dv.ToTable();
            //                string SMTPMailServer = common.myStr(dt.Rows[0]["SMTPMailServer"]);
            //                string SMTPMailServerPort = common.myStr(dt.Rows[0]["SMTPMailServerPort"]);
            //                string DefaultFromMailId = common.myStr(dt.Rows[0]["DefaultFromMailId"]);
            //                string DefaultFromMailPws = common.myStr(dt.Rows[0]["DefaultFromMailPws"]);

            //                MemoryStream ms = new MemoryStream(bytes); ;
            //                MailMessage mail = new MailMessage();
            //                mail.Attachments.Add(new Attachment(ms, common.myStr(Request.QueryString["LABNO"]).Trim() + ".pdf"));

            //                mail.From = new MailAddress(DefaultFromMailId, DefaultFromMailId);
            //                mail.To.Add(common.myStr(Request.QueryString["EMail"]).Trim());
            //                mail.Subject = "Lab Result For Lab No. " + common.myStr(Request.QueryString["LABNO"]).Trim();
            //                mail.Body = "Dear " + common.myStr(Request.QueryString["PName"]).Trim() + "," + System.Environment.NewLine + System.Environment.NewLine + "Please! Find your attached Lab Report.";
            //                mail.IsBodyHtml = false;
            //                mail.BodyEncoding = System.Text.Encoding.UTF8;

            //                SmtpClient client = new SmtpClient(SMTPMailServer, common.myInt(SMTPMailServerPort));
            //                client.Credentials = new NetworkCredential(DefaultFromMailId, DefaultFromMailPws);
            //                client.Host = SMTPMailServer;
            //                client.Port = common.myInt(SMTPMailServerPort);
            //                client.Send(mail);
            //                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            MemoryStream oStream = new MemoryStream();
            oStream.Write(bytes, 0, bytes.Length);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();
            //}
        }
        catch (Exception ex)
        {
            //msg.Text = "1 " + ex.Message;
        }
    }
}
