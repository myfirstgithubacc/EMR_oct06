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

public partial class MISDashboard_deathbirthReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    DataSet ds;
    private bool Stauts = false;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString() + " HH:mm";
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString() + " HH:mm";
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])) + " 00:00");

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])) + " HH:mm");
            if (common.myStr(Request.QueryString["From"])!= "ICD")
            {
                lblSource.Visible = false;
                lblReportType.Visible = false;
                ddlSource.Visible = false;
                rdoICDCPT.Visible = false;
            }

            // ShowReport();
        }
    }
    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["From"]) != string.Empty)
            ShowReport();
    }
    protected void ShowReport()
    {
        BaseC.User valUser = new BaseC.User(sConString);

        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        string ReportServerPath = "http://" + reportServer + "/ReportServer";


        BaseC.EMR objEmr = new BaseC.EMR(sConString);

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;
        

        ReportParameter[] p;
        if (common.myStr(Request.QueryString["From"]) == "ICD")
            p = new ReportParameter[7];
        else
            p = new ReportParameter[5];

        //p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
        p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
        p[2] = new ReportParameter("chrFromDate", dtpfromdate.SelectedDate.Value.ToString("yyyy/MM/dd"));
        p[3] = new ReportParameter("chrToDate", dtpTodate.SelectedDate.Value.ToString("yyyy/MM/dd"));

        if (common.myStr(Request.QueryString["From"]) == "DR")
        {
            ReportViewer1.ServerReport.ReportPath = "/"+ reportFolder + "/rptDeathRegister";
            p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[4] = new ReportParameter("IPNoText", Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
        }
        if (common.myStr(Request.QueryString["From"]) == "BR")
        {
            ReportViewer1.ServerReport.ReportPath = "/"+ reportFolder + "/rptBirthRegister";
            p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[4] = new ReportParameter("IPNoText", Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
        }
        if (common.myStr(Request.QueryString["From"]) == "ICD")
        {
            ReportViewer1.ServerReport.ReportPath = "/"+ reportFolder + "/rptDiagnosisStatisticsWithOutICD";
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));

            p[4] = new ReportParameter("intStoreId", common.myStr("1"));
            p[5] = new ReportParameter("chvReportType", common.myStr(rdoICDCPT.SelectedValue));
            p[6] = new ReportParameter("chvSource", common.myStr(ddlSource.SelectedValue).ToString());
        }



        ReportViewer1.ServerReport.SetParameters(p);
        ReportViewer1.ServerReport.Refresh();

    }
}
