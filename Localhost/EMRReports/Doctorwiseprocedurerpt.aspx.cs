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

public partial class EMRReports_Doctorwiseprocedurerpt : System.Web.UI.Page
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
        string setformula = "";
        BaseC.User valUser = new BaseC.User(sConString);
        if (common.myStr(Request.QueryString["DoctorId"]) != "")
        {
            setformula = (common.myStr(Request.QueryString["DoctorId"]));
        }

        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        bool val = false;

        string[] strDoctorId = common.myStr(setformula).Split(',');


        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;


        ReportParameter[] p = new ReportParameter[0];
        if (common.myStr(Request.QueryString["ReportName"]) == "AppReport")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptAppointment";

            p = new ReportParameter[8];

            p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationId"].ToString());
            p[2] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            if (common.myStr(Request.QueryString["DoctorId"]) != "")
            {
                p[1] = new ReportParameter("chvProviderId", Convert.ToString(Request.QueryString["DoctorId"]));

            }
            else
            {
                p[1] = new ReportParameter("chvProviderId", "A");
            }


            //   p[1] = new ReportParameter("chvProviderId", strDoctorId);
            p[3] = new ReportParameter("GroupParameter1", "FacilityId");
            p[4] = new ReportParameter("chrFromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("chrToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("UserId", sUserName);
            p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"].ToString()));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "AppList")
        {

            if (common.myStr(Request.QueryString["ReportType"]) == "CA")
            {
                p = new ReportParameter[7];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AppointmentCancellation";
                p[0] = new ReportParameter("chrToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
                p[2] = new ReportParameter("chrFromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
                p[1] = new ReportParameter("xmlDoctorString", common.myStr(Session["DoctorId"]));
                p[3] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[4] = new ReportParameter("intfacilityId", common.myStr(Session["FacilityId"]));
                p[5] = new ReportParameter("userName", sUserName);
                p[6] = new ReportParameter("UHID", (string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));
            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "BAB")
            {
                p = new ReportParameter[7];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BreakAndBlockUserWise";
                p[0] = new ReportParameter("chrToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
                p[2] = new ReportParameter("chrFromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
                p[1] = new ReportParameter("xmlDoctorString", common.myStr(Session["DoctorId"]));
                p[3] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[5] = new ReportParameter("UserId", sUserName);
                p[6] = new ReportParameter("ReportHeader", "Break And Block Report");

            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "AL")
            {
                p = new ReportParameter[6];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AppointmentListing";
                p[0] = new ReportParameter("ToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
                p[2] = new ReportParameter("fromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
                p[1] = new ReportParameter("DoctorId", common.myStr(Session["DoctorId"]));
                p[3] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[5] = new ReportParameter("UserId", sUserName);

            }

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "Anethisia")
        {
            p = new ReportParameter[5];
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptAnesthsiaFee";
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("dtToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy-MM-dd")));
            p[3] = new ReportParameter("dtFromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy-MM-dd")));
            p[4] = new ReportParameter("User_id", sUserName);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "EncWiseUnbilled")
        {
            p = new ReportParameter[6];
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/EncounterWisePatientDetails";
            p[0] = new ReportParameter("Hospitallocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("Facilityid", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("ToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("FromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("statusId", "0");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "PACKRN")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptPackageServiceRenderDtl";
            p = new ReportParameter[4];
            p[0] = new ReportParameter("Hospitallocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("Facilityid", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("ToDate", Convert.ToString(Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "USAI")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/UnperformedServiceAndPendingDrugOrder";
            p = new ReportParameter[8];
            p[0] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[1] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[2] = new ReportParameter("EncounterId", common.myStr(Session["EncounterId"]));
            p[3] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
            p[4] = new ReportParameter("Fdate", Convert.ToString(Request.QueryString["Fromdate"]));
            p[5] = new ReportParameter("Tdate", Convert.ToString(Request.QueryString["Todate"]));
            p[6] = new ReportParameter("intStoreId", "0");
            p[7] = new ReportParameter("intEncounterId", "0");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else
        {
            if (common.myStr(Request.QueryString["IsDetail"]) == "1")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DoctorWiseServiceDetails"; // for details report
            }
            else
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DoctorWiseProcedureStat"; // for summary report

            p = new ReportParameter[8];

            p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[1] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            p[2] = new ReportParameter("fromdate", Convert.ToString(Request.QueryString["Fromdate"]));
            p[3] = new ReportParameter("ToDate", Convert.ToString(Request.QueryString["Todate"]));
            p[4] = new ReportParameter("Doctor", strDoctorId);
            p[5] = new ReportParameter("Userid", sUserName);
            p[6] = new ReportParameter("IsBilled", common.myStr(Request.QueryString["IsBilled"]));
            p[7] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        //

        if (common.myStr(Request.QueryString["Export"]) != "True")
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
