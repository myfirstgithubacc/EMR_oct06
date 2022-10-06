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

public partial class LIS_Phlebotomy_PrintInvestigationList : System.Web.UI.Page
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

        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvestigationList";
        p = new ReportParameter[7];
        p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));

        p[1] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
        p[2] = new ReportParameter("chrSource", common.myStr(Request.QueryString["SOURCE"]));
        p[3] = new ReportParameter("intLabNo", common.myStr(Request.QueryString["LABNO"]));
        p[4] = new ReportParameter("RegNo", common.myStr(GetGlobalResourceObject("PRegistration", "regno")));
        if (Flag.ToString() == "RIS")
        {
            p[5] = new ReportParameter("chvReportType", common.myStr("Radiology Investigation List"));
        }
        else
        {
            p[5] = new ReportParameter("chvReportType", common.myStr("Laboratory Investigation List"));
        }
        p[6] = new ReportParameter("bitIsExternalCenterOnly", common.myBool(Request.QueryString["IsExternalCenterOnly"]).ToString());        

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
        string filename = common.myStr(Request.QueryString["ReportName"]) + ".pdf";
        Response.AppendHeader("Content-Disposition", "filename=" + filename + "");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();
    }
}
