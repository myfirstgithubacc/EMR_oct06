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
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.IO;



public partial class MRD_Reports_DiseaseStatistics : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
   // wcf_Service_Common.CommonMasterClient objCommon = new wcf_Service_Common.CommonMasterClient();
    DAL.DAL dl;
    clsExceptionLog objException = new clsExceptionLog();

    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                rdoRptType.SelectedIndex = 0;
                fillSubGroup();
                dtpFromdate.SelectedDate = DateTime.Now;
                dtpTodate.SelectedDate = DateTime.Now;
                bindYearMonth();
                chkAllDiag.Checked = true;
                btnPerformanceAnalysis_OnClick(null, null);
            }
            catch (Exception Ex)
            {

                objException.HandleException(Ex);
            }
        }
    }

    protected void fillSubGroup()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = dl.FillDataSet(CommandType.Text, "SELECT SubGroupId,SubGroupName from ICD9SubGroup order by SubGroupName");
        ddlCategory.DataSource = ds;
        ddlCategory.DataTextField = "SubGroupName";
        ddlCategory.DataValueField = "SubGroupId";
        ddlCategory.DataBind();
        ddlCategory.SelectedIndex = 0;
    }


    private void bindYearMonth()
    {
        try
        {

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void btnPerformanceAnalysis_OnClick(object sender, EventArgs e)
    {
        String sRptName = "";
        string[] sDiagnosis;

        sDiagnosis = common.GetCheckedItems(ddlDiagnosis).Split(',');

        ReportParameter[] p = new ReportParameter[12];

        p = new ReportParameter[12];
        sRptName = "rptDiagnosisStatistics";
        p[0] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
        p[1] = new ReportParameter("iFacilityId", common.myStr(Session["FacilityId"]));
        p[2] = new ReportParameter("cOPIP", common.myStr(ddlType.SelectedValue));
        p[3] = new ReportParameter("cGroupType", common.myStr(ddlICDorICP.SelectedValue));
        p[4] = new ReportParameter("dFromDate", common.myDate(dtpFromdate.SelectedDate).ToString("yyyy/MM/dd"));
        p[5] = new ReportParameter("dToDate", common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd"));
        if (chkAllDiag.Checked == true)
            p[6] = new ReportParameter("sICDID", "A");
        else
            p[6] = new ReportParameter("sICDID", sDiagnosis);
        p[7] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
        p[8] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
        p[9] = new ReportParameter("intStoreId", "0");

        if (rdoRptType.SelectedIndex == 0)
            p[10] = new ReportParameter("rptHeader", "Dieases Wise Statistics");
        else if (rdoRptType.SelectedIndex == 1)
            p[10] = new ReportParameter("rptHeader", "Doctor Wise Statistics");
        else if (rdoRptType.SelectedIndex == 2)
            p[10] = new ReportParameter("rptHeader", "Department Statistics");

        p[11] = new ReportParameter("cReportType", common.myStr(rdoRptType.SelectedValue.ToString()));

        PrintReport(sRptName, p);

    }

    private void PrintReport(string sReportName, ReportParameter[] para)
    {

        string ReportServerPath = "http://" + reportServer + "/ReportServer";
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;
        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;

        ReportViewer1.ShowParameterPrompts = false;

        ReportViewer1.ShowBackButton = true;
        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + sReportName;
        ReportViewer1.ServerReport.SetParameters(para);

        ReportViewer1.ServerReport.Refresh();


        /*
        string[] streamids = null;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string mimeType;
        string encoding;
        string extension;
        //string deviceInfo = string.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.7in", "8.3in");
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);

        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
        byte[] bytes = this.ReportViewer1.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        MemoryStream oStream = new MemoryStream(); ;
        oStream.Write(bytes, 0, bytes.Length);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();*/

    }
    protected void chkAllDiag_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkAllDiag.Checked == true)
        {
            tdDiag.Visible = false;
            tdddlDiag.Visible = false;
            tdCate.Visible = false;
            tdddlCate.Visible = false;
        }
        else
        {
            tdDiag.Visible = true;
            tdddlDiag.Visible = true;
            tdCate.Visible = true;
            tdddlCate.Visible = true;
        }

    }
    protected void rdoRptType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnPerformanceAnalysis_OnClick(null, null);

    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myInt(ddlCategory.SelectedValue) > 0)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            hsIn.Add("@SubGroupId", common.myInt(ddlCategory.SelectedValue));
            DataSet ds = dl.FillDataSet(CommandType.Text, "SELECT ICDID, Description FROM ICD9SubDisease where SubGroupId = @SubGroupId order by Description", hsIn);
            ddlDiagnosis.DataSource = ds;
            ddlDiagnosis.DataTextField = "Description";
            ddlDiagnosis.DataValueField = "ICDID";
            ddlDiagnosis.DataBind();
            ddlDiagnosis.SelectedIndex = 0;

        }

    }
}
