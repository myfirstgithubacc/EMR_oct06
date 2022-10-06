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
using System.IO;

public partial class Pharmacy_Reports_IPSaleDocReport : System.Web.UI.Page
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
        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;

        ReportParameter[] p;
        string PatientName = "";
        string EncounterNo = "";
        string rptName = "";

        EncounterNo = common.myStr(Request.QueryString["EncounterNo"]);
        if (EncounterNo == "")
        {
            EncounterNo = " ";
        }
        string strOPIP = common.myStr(Request.QueryString["OPIP"]);
        switch (common.myStr(Request.QueryString["UseFor"]))
        {
            case "DOC":

                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPSaleIssueDoc";

                if (common.myStr(Request.QueryString["IssueReturn"]) == "R")
                {
                    if (common.myStr(Request.QueryString["SetupId"]) == "204")
                    {
                        rptName = "EMERGENCY REFUND";
                    }
                    else
                    {
                        rptName = "IP REFUND";
                    }
                }
                else
                {
                    rptName = "BILL / INVOICE";
                }

                p = new ReportParameter[13];
                string StoreId = "0";
                if (common.myInt(Request.QueryString["StoreId"]) == 0)
                    StoreId = common.myStr(Session["StoreId"]);
                else
                    StoreId = common.myStr(Request.QueryString["StoreId"]);

                p[0] = new ReportParameter("intIssueId", common.myStr(common.myInt(Request.QueryString["IssueId"])));
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[3] = new ReportParameter("intSaleSetupId", Request.QueryString["SetupId"]);
                p[4] = new ReportParameter("intStoreId", common.myStr(StoreId));
                p[5] = new ReportParameter("UHID", Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "RegNo").ToString()));
                p[6] = new ReportParameter("chvSNo", common.myStr(GetGlobalResourceObject("PRegistration", "SerialNo")));
                p[7] = new ReportParameter("ReportHeaderName", rptName);
                p[8] = new ReportParameter("IPNO", Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
                p[9] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[10] = new ReportParameter("chrIssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                p[11] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[12] = new ReportParameter("chvOPIP", common.myStr(Request.QueryString["OPIP"]) == "E" ? "E" : "I");

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                break;
            case "IPD":
                //ReportViewer1.ServerReport.ReportPath = "/EMRReport/IPNOwiseIssueSaleReport";
                if (common.myStr(Request.QueryString["MRPCostPrice"]) == "P")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProfitMargin";
                    rptName = "Issue Return Profit Margin Statement ";
                }
                else
                {
                    if (strOPIP == "IP")
                    {
                        //rptName = "IP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                        rptName = "IP Issue/Return Details Statement ";
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPNOwiseIssueSaleReport";
                    }
                    else if (strOPIP == "OP")
                    {
                        // rptName = "OP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                        rptName = "OP Issue/Return Details Statement ";
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPNOwiseIssueSaleReport";
                    }
                    else if (strOPIP == "ECHS")
                    {
                        // rptName = "OP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                        rptName = "OP Issue/Return Details Statement ";
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPNOwiseIssueSaleReport_Echs";
                    }
                }

                p = new ReportParameter[13];

                //rptName = "IP Issue/Return Details Statement";

                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", common.myStr(common.myDate("1900/01/01")));
                p[6] = new ReportParameter("dtToDate", common.myStr(common.myDate("2079/01/01")));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                //  p[12] = new ReportParameter("intLoginStoreId", common.myStr(Session["StoreId"]));

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                break;
            case "DRW":
                if (common.myStr(Request.QueryString["MRPCostPrice"]) == "P")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProfitMargin";
                    rptName = "Issue Return Profit Margin Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }
                else
                {
                    if (strOPIP == "IP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPNOwiseIssueSaleReport";
                        rptName = "IP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                    }
                    else if (strOPIP == "OP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPNOwiseIssueSaleReport";
                        //ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPNOwiseIssueSaleReportItemWise";
                        rptName = "OP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                    }
                }
                p = new ReportParameter[13];

                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", common.myStr(Request.QueryString["FromDate"]));
                p[6] = new ReportParameter("dtToDate", common.myStr(Request.QueryString["ToDate"]));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                
                //p[12] = new ReportParameter("intLoginStoreId",common.myStr(Session["StoreId"]));

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                break;
            case "IPS":

                if (common.myStr(Request.QueryString["MRPCostPrice"]) == "P")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProfitMargin";
                    rptName = "Issue Return Profit Margin Statement ";
                }
                else
                {
                    if (strOPIP == "IP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPNOwiseIssueSaleSummary";
                        rptName = "IP Issue/Return Summary Statement ";
                    }
                    else if (strOPIP == "OP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPNOwiseIssueSaleSummary";
                        //rptName = "OP Issue/Return Summary Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                        rptName = "OP Issue/Return Summary Statement ";
                    }
                }

                p = new ReportParameter[13];

                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", common.myStr(common.myDate("1900/01/01")));
                p[6] = new ReportParameter("dtToDate", common.myStr(common.myDate("2079/01/01")));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                //p[12] = new ReportParameter("intLoginStoreId", common.myStr(Session["StoreId"]));

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                break;
            case "DRS":
                if (common.myStr(Request.QueryString["MRPCostPrice"]) == "P")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProfitMargin";
                    rptName = "Issue Return Profit Margin Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }
                else
                {
                    if (strOPIP == "IP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPNOwiseIssueSaleSummary";
                        rptName = "IP Issue/Return Summary Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                    }
                    else if (strOPIP == "OP")
                    {
                        ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPNOwiseIssueSaleSummary";
                        rptName = "OP Issue/Return Summary Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                    }
                }
                p = new ReportParameter[13];

                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", Convert.ToDateTime(common.myStr(Request.QueryString["FromDate"])).ToString("yyyy-MM-dd"));
                p[6] = new ReportParameter("dtToDate", Convert.ToDateTime(common.myStr(Request.QueryString["ToDate"])).ToString("yyyy-MM-dd"));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
                break;

            case "DRSS":
                if (strOPIP == "IP")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPIssueSaleSummary";
                    rptName = "Patient Issue/Return Summary Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }
                else
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPIssueSaleSummary";
                    rptName = "Patient Issue/Return Summary Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }


                p = new ReportParameter[13];
                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", Convert.ToDateTime(common.myStr(Request.QueryString["FromDate"])).ToString("yyyy-MM-dd"));
                p[6] = new ReportParameter("dtToDate", Convert.ToDateTime(common.myStr(Request.QueryString["ToDate"])).ToString("yyyy-MM-dd"));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

                break;
            case "DRWD":
                if (strOPIP == "IP")
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPIssueSaleDetail";
                    rptName = "IP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }
                else
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPIssueSaleDetail";
                    rptName = "OP Issue/Return Details Statement ( From : " + common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy") + " To : " + common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy") + " )";
                }

                p = new ReportParameter[13];
                p[0] = new ReportParameter("ReportHeaderName", rptName);
                p[1] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityIds"]));
                p[3] = new ReportParameter("chvEncounterNo", EncounterNo);
                p[4] = new ReportParameter("intRegNo", common.myStr(common.myInt(Request.QueryString["RegNo"])));
                p[5] = new ReportParameter("dtFromDate", common.myStr(Request.QueryString["FromDate"]));
                p[6] = new ReportParameter("dtToDate", common.myStr(Request.QueryString["ToDate"]));
                p[7] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[8] = new ReportParameter("User", common.myStr(Session["UserName"]));
                p[9] = new ReportParameter("MRPCostPrice", common.myStr(Request.QueryString["MRPCostPrice"]));
                p[10] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreId"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PType"]));
                p[12] = new ReportParameter("IssueReturn", common.myStr(Request.QueryString["IssueReturn"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
                break;
        }
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
