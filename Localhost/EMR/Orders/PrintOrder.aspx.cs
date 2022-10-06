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

[Serializable]
public partial class EMR_Orders_PrintOrder : System.Web.UI.Page
{
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ShowReport();
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    protected void ShowReport()
    {
        try
        {
            //if (Session["HospitalLocationId"] != null && Session["RegistrationId"] != null && Session["encounterId"] != null)
            //{

            string ReportServerPath = "http://" + reportServer + "/ReportServer";

            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;
            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PurchaseOrderReport";

            ReportParameter[] p = new ReportParameter[0];


            if (Request.QueryString["rptName"] == "POD")
            {
                p = new ReportParameter[5];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PrintOrderDetails";
                //p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationId"].ToString());
                //p[1] = new ReportParameter("intRegistrationId", Session["RegistrationId"].ToString());
                //p[2] = new ReportParameter("intEncounterId", Session["encounterId"].ToString());
                //p[3] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());

                p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationId"].ToString());
                p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("encounterId", Session["encounterId"].ToString());
                p[3] = new ReportParameter("Registrationid", Session["RegistrationId"].ToString());
                p[4] = new ReportParameter("orderID", Session["OrderID"].ToString()); //Session["FacilityId"].ToString()
                ReportViewer1.ServerReport.SetParameters(p);
            }
            else if (Request.QueryString["rptName"] == "INR")
            {
                p = new ReportParameter[4];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/investigation_status_report";
                p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
                p[1] = new ReportParameter("EncounterId", Session["encounterId"].ToString());
                p[2] = new ReportParameter("RegId", Session["RegistrationId"].ToString());
                p[3] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationId"].ToString());
                ReportViewer1.ServerReport.SetParameters(p);
            }
            else if (Request.QueryString["rptName"] == "PODSave")
            {
                //p = new ReportParameter[4];
                //ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PrintOrderDetails";
                //p[0] = new ReportParameter("HospLocId", Session["HospitalLocationId"].ToString());
                //p[1] = new ReportParameter("encounterId",  common.myStr(Request.QueryString["encounterId"]));
                //p[2] = new ReportParameter("Registrationid", common.myStr(Request.QueryString["RegistrationId"]));
                //p[3] = new ReportParameter("orderID", common.myInt(Request.QueryString["OrderId"]).ToString());
                //ReportViewer1.ServerReport.SetParameters(p);


                p = new ReportParameter[7];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PrintOrderDetails";
                p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationId"].ToString());
                p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());//Session["FacilityId"].ToString()
                p[2] = new ReportParameter("intStoreId", "0");
                p[3] = new ReportParameter("encounterId", common.myStr(Request.QueryString["encounterId"]));
                p[4] = new ReportParameter("Registrationid", common.myStr(Request.QueryString["RegistrationId"]));
                p[5] = new ReportParameter("orderID", common.myStr(Request.QueryString["OrderId"]));
                p[6] = new ReportParameter("intBookingId", "0");  // Added by akshay sharma

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

            //}

        }
        catch (Exception Ex)
        {
            //objException.HandleException(Ex);
        }
    }
}