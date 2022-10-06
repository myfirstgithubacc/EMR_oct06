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
using System.Text;

public partial class LIS_Phlebotomy_PrintYAxisReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    string Flag = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["MD"] != null)
        {
            Flag = common.myStr(Request.QueryString["MD"]);
        }
        ShowReport();
    }

    protected void ShowReport()
    {

        string DefaultFacilityName = "";
        int FacilityId = common.myInt(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultFacility", sConString));

        if (Cache["FACILITY"] == null)
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            Cache["FACILITY"] = ds;
        }
        DataSet dsFacility = (DataSet)Cache["FACILITY"];
        dsFacility.Tables[0].DefaultView.RowFilter = "";
        dsFacility.Tables[0].DefaultView.RowFilter = "FacilityId=" + FacilityId;
        if (dsFacility.Tables[0].DefaultView.Count > 0)
        {
            DefaultFacilityName = common.myStr(dsFacility.Tables[0].DefaultView[0]["FacilityName"]);
        }

        String sServices = string.Empty, sFieldIds = string.Empty, dtFrom = string.Empty, dtTo = string.Empty, sRegNo = string.Empty, sEncId = string.Empty;
        if (Session["sbServices"] != null)
        {
            sServices = common.myStr(Session["sbServices"]);
        }
        if (Session["sbFieldIds"] != null)
        {
            sFieldIds = common.myStr(Session["sbFieldIds"]);
        }

        if ((Request.QueryString["dtFrom"] != null) && (Request.QueryString["dtTo"] != null))
        {
            dtFrom = HttpUtility.UrlDecode(common.myStr(Request.QueryString["dtFrom"]));
            dtTo = HttpUtility.UrlDecode(common.myStr(Request.QueryString["dtTo"]));
        }
        if (Request.QueryString["RegNo"] != null)
        {
            sRegNo = common.myStr(Request.QueryString["RegNo"]);
        }
        if (Request.QueryString["EncId"] != null)
        {
            sEncId = common.myStr(Request.QueryString["EncId"]);
        }

        String strUserName = "";
        BaseC.EMR objEmr = new BaseC.EMR(sConString);
        if (Session["UserID"] != null && Session["HospitalLocationID"] != null)
        {
            SqlDataReader objDr = (SqlDataReader)objEmr.GetEmployeeId(Convert.ToInt32(Session["UserID"]), Convert.ToInt16(Session["HospitalLocationID"]));
            if (objDr.Read())
                strUserName = common.myStr(objDr[1]);
            objDr.Close();
        }


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
        ReportParameter[] p = null;

        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptTestReport";
        p = new ReportParameter[7];
        p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
        p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
        p[2] = new ReportParameter("xmlServiceIds", sServices);
        p[3] = new ReportParameter("intRegistrationNo", sRegNo);
        p[4] = new ReportParameter("intEncounterId", common.myInt(sEncId).ToString());
        p[5] = new ReportParameter("dtFromDate", Convert.ToDateTime(dtFrom).ToShortDateString());
        p[6] = new ReportParameter("dtToDate", Convert.ToDateTime(dtTo).ToShortDateString());
        //p[7] = new ReportParameter("xmlFieldIds", sFieldIds);
        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();

        if (common.myBool(Request.QueryString["IsExport"]))
        {
            return;
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

        MemoryStream oStream = new MemoryStream();
        oStream.Write(bytes, 0, bytes.Length);
        Response.Clear();
        Response.Buffer = true;
        string filename = "PrintYAxisReport.pdf";
        Response.AppendHeader("Content-Disposition", "filename=" + filename + "");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();
    }
}
