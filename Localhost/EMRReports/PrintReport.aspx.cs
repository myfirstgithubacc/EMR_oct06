using System;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.IO;

public partial class EMRReports_PrintReport : System.Web.UI.Page
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
        string[] strDoctorId = common.myStr(setformula).Split(',');

        DateTime frmdate = Convert.ToDateTime(Request.QueryString["Fromdate"]);
        DateTime todate = Convert.ToDateTime(Request.QueryString["Todate"]);


        DateTime frmdatetime = Convert.ToDateTime(Request.QueryString["FromdateTime"]);
        DateTime todatetime = Convert.ToDateTime(Request.QueryString["TodateTime"]);



        string sbIdList = common.myStr(Request.QueryString["CompanyCode"]);

        string[] strIdList = sbIdList.ToString().Split(',');

        string entryid = common.myStr(Request.QueryString["entryid"]);
        string[] strentryid = entryid.ToString().Split(',');

        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        string ReportServerPath = "http://" + reportServer + "/ReportServer";

        String strUserName = "";
        BaseC.EMR objEmr = new BaseC.EMR(sConString);
        if (Session["UserID"] != null && Session["HospitalLocationID"] != null)
        {
            SqlDataReader objDr = (SqlDataReader)objEmr.GetEmployeeId(Convert.ToInt32(Session["UserID"]), Convert.ToInt16(Session["HospitalLocationID"]));
            if (objDr.Read())
                strUserName = common.myStr(objDr[1]);
            objDr.Close();
        }
        strUserName = common.myStr(Session["UserID"]);
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;


        if (common.myStr(Request.QueryString["ReportName"]) == "S")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OutstandingSummary";

            ReportParameter[] p = new ReportParameter[13];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("PatientType", common.myStr(Request.QueryString["PaymentType"]));
            p[3] = new ReportParameter("User_id", strUserName);
            //  p[4] = new ReportParameter("CompanyCode", strIdList);
            p[4] = new ReportParameter("CompanyCode", sbIdList);
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[8] = new ReportParameter("Todate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[9] = new ReportParameter("ReportHeader", "OutStanding Summary  As on");
            if (common.myStr(Request.QueryString["ReportType"]) == "A")
            {
                p[10] = new ReportParameter("OutType", "OA");
            }
            else
            {
                p[10] = new ReportParameter("OutType", "OD");
            }

            p[11] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[12] = new ReportParameter("ForExport", common.myStr(Request.QueryString["Export"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "D")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BILLREGISTER";
            ReportParameter[] p = new ReportParameter[13];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("PatientType", common.myStr(Request.QueryString["PaymentType"]));
            p[3] = new ReportParameter("User_id", strUserName);
            //  p[4] = new ReportParameter("CompanyCode", strIdList);
            p[4] = new ReportParameter("CompanyCode", sbIdList);
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[8] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[9] = new ReportParameter("ReportHeader", "OutStanding Details");
            if (common.myStr(Request.QueryString["ReportType"]) == "A")
            {
                p[10] = new ReportParameter("OutType", "OA");
            }
            else
            {
                p[10] = new ReportParameter("OutType", "OD");
            }
            p[11] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[12] = new ReportParameter("ForExport", common.myStr(Request.QueryString["Export"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "OSD")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BILLREGISTER";
            ReportParameter[] p = new ReportParameter[13];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("PatientType", "A");
            p[3] = new ReportParameter("User_id", strUserName);
            p[4] = new ReportParameter("CompanyCode", strIdList);
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[8] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[9] = new ReportParameter("ReportHeader", "OutStanding Details Date Wise");
            p[10] = new ReportParameter("OutType", "OD");
            p[11] = new ReportParameter("EntrySite", "A");
            p[12] = new ReportParameter("ForExport", common.myStr(Request.QueryString["Export"]));



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        //palendra 
        else if (common.myStr(Request.QueryString["PrintType"]) == "DischargeNotification")
        {
            //Awadhesh
            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeNotification";
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeNotificationWard";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("EncounterNo", common.myStr(Request.QueryString["IpNo"]).Trim());
            p[1] = new ReportParameter("UserID", Session["UserID"].ToString());
            p[2] = new ReportParameter("intFacilityId", Session["FacilityID"].ToString());
            p[3] = new ReportParameter("RptHeadText", "No Dues");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "Discountbill")
        {
            if (common.myStr(Request.QueryString["RptType"]) == "AW")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DiscountAuthoritywise";

                ReportParameter[] p = new ReportParameter[12];
                p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
                p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
                p[5] = new ReportParameter("ReportHeader", "Discount Report(Authority wise) ");
                p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
                p[7] = new ReportParameter("User_id", strUserName);
                p[8] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IpNo"));
                p[9] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[10] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
                //p[11] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
                p[11] = new ReportParameter("chrPaymentType", common.myStr(Request.QueryString["PaymentType"]));


                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ConcessionReportDetailBillWise";

                ReportParameter[] p = new ReportParameter[12];
                p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("User_id", strUserName);
                p[3] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("ReportHeader", "Department Wise Revenue ");
                p[6] = new ReportParameter("Currency", "Rs.");
                p[7] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
                p[8] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
                p[9] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IpNo"));
                p[10] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
                p[11] = new ReportParameter("PaymentType", common.myStr(Request.QueryString["PaymentType"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }


        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "Revenue")
        {
            ReportParameter[] p;
            if (common.myStr(Request.QueryString["CompanyWise"]) == "False")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DEPARTMENTWISEREVENUE";
                p = new ReportParameter[11];
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DepartmentWiseCompanyRevenue";
                p = new ReportParameter[11];
            }


            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("User_id", strUserName);
            if (common.myStr(Request.QueryString["GrbType"]) != "")
                p[3] = new ReportParameter("GrbType", common.myStr(Request.QueryString["GrbType"]));
            else
                p[3] = new ReportParameter("GrbType", "A");
            p[4] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("ReportHeader", "Department Wise Revenue ");
            p[7] = new ReportParameter("Currency", "Rs.");
            p[8] = new ReportParameter("DCode", " ");
            p[9] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            if (common.myStr(Request.QueryString["CompanyWise"]) != "False")
                p[10] = new ReportParameter("CompanyCode", sbIdList);
            else
                p[10] = new ReportParameter("CompanyCode", "A");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "RevenueDetails")
        {
            if (common.myStr(Request.QueryString["ForReport"]) == "RCB")//Revenue Contribution
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DoctorSpecialityRevenueContribution";
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CUSTOMWISEREVENUE";
            }

            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("User_id", strUserName);
            p[3] = new ReportParameter("GrbType", common.myStr(Request.QueryString["Type"]));
            p[4] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("ReportHeader", "Department Wise Revenue ");
            p[7] = new ReportParameter("Currency", "Rs.");
            p[8] = new ReportParameter("DCode", common.myStr(Request.QueryString["DCode"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "IPTAT")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPTAT";

            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("UserID", strUserName);
            p[3] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "ipno"));
            p[4] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[5] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[7] = new ReportParameter("CompanyCode", strIdList);
            p[8] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "OPTAT")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPTAT";

            ReportParameter[] p = new ReportParameter[3];
            //p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[0] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            //p[2] = new ReportParameter("UserID", strUserName);
            //p[3] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "ipno"));
            //p[4] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[1] = new ReportParameter("ForDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DSR")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisDailySummaryReport";

            ReportParameter[] p = new ReportParameter[5];

            p[0] = new ReportParameter("chvFrmDate", frmdate.ToString("yyyy/MM/dd"));
            p[1] = new ReportParameter("chvToDate", todate.ToString("yyyy/MM/dd"));
            p[2] = new ReportParameter("intHospitalLoccationid", common.myStr(Session["HospitalLocationId"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "BillCanc")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CancelledReport";

            ReportParameter[] p = new ReportParameter[10];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Type", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("User_id", strUserName);
            p[4] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[5] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[7] = new ReportParameter("ReportHeader", "Bill Cancelled");
            p[8] = new ReportParameter("Currency", "Rs.");
            p[9] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "PatientRegDetails")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientRegistrationDetails";

            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RegNo"]));
            //p[3] = new ReportParameter("intOldRegistrationId","");

            //p[4] = new ReportParameter("intEncounterId", common.myStr(0));
            //p[5] = new ReportParameter("intEncodedBy", Convert.ToString(Session["UserID"]));
            //p[6] = new ReportParameter("chvErrorStatus", " ");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }






        else if (common.myStr(Request.QueryString["ReportName"]) == "BillDispatch")
        {
            if (common.myStr(Request.QueryString["RptType"]) == "DPT")//Bil Dispatch And Acknowledge Report
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptBillDispatchedDetailsReport";

                ReportParameter[] p = new ReportParameter[12];

                p[0] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chrToDate", common.myStr(todate.AddDays(1).ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("chrSearchon", common.myStr(Request.QueryString["Searchon"]));
                p[6] = new ReportParameter("chvSearchvalue", common.myStr(Request.QueryString["Searchvalue"]));
                p[7] = new ReportParameter("statusID", "RC");
                p[8] = new ReportParameter("chrOPIP", common.myStr(Request.QueryString["SourceType"]));
                p[9] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[10] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
                p[11] = new ReportParameter("companyId", common.myStr(Request.QueryString["CompanyId"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

            else if (common.myStr(Request.QueryString["RptType"]) == "DAC")// Bill Dispatch Acknowledge Cancelled
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptBillDispatchedAckCancelReport";

                ReportParameter[] p = new ReportParameter[12];

                p[0] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chrToDate", common.myStr(todate.AddDays(1).ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("chrSearchon", common.myStr(Request.QueryString["Searchon"]));
                p[6] = new ReportParameter("chvSearchvalue", common.myStr(Request.QueryString["Searchvalue"]));
                p[7] = new ReportParameter("statusID", "RC");
                p[8] = new ReportParameter("chrOPIP", common.myStr(Request.QueryString["SourceType"]));
                p[9] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[10] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
                p[11] = new ReportParameter("companyId", common.myStr(Request.QueryString["CompanyId"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

            else if (common.myStr(Request.QueryString["RptType"]) == "BTPTAT")// Bill Dispatch Acknowledge Cancelled
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptBillDispatchedTAT";

                ReportParameter[] p = new ReportParameter[12];

                p[0] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chrToDate", common.myStr(todate.AddDays(1).ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("chrSearchon", common.myStr(Request.QueryString["Searchon"]));
                p[6] = new ReportParameter("chvSearchvalue", common.myStr(Request.QueryString["Searchvalue"]));
                p[7] = new ReportParameter("statusID", "RC");
                p[8] = new ReportParameter("chrOPIP", common.myStr(Request.QueryString["SourceType"]));
                p[9] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[10] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
                p[11] = new ReportParameter("companyId", common.myStr(Request.QueryString["CompanyId"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

            else if (common.myStr(Request.QueryString["RptType"]) == "DPC")// Bill Dispatch Cancelled
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptBillDispatchedCancelReport";

                ReportParameter[] p = new ReportParameter[12];

                p[0] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[3] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chrToDate", common.myStr(todate.AddDays(1).ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("chrSearchon", common.myStr(Request.QueryString["Searchon"]));
                p[6] = new ReportParameter("chvSearchvalue", common.myStr(Request.QueryString["Searchvalue"]));
                p[7] = new ReportParameter("statusID", "RC");
                p[8] = new ReportParameter("chrOPIP", common.myStr(Request.QueryString["SourceType"]));
                p[9] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[10] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
                p[11] = new ReportParameter("companyId", common.myStr(Request.QueryString["CompanyId"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

        }


        else if (common.myStr(Request.QueryString["ReportName"]).Equals("OPDZeroBilling"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPDZeroBillingReportNew";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("hospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("user_id", strUserName);
            p[5] = new ReportParameter("UserList", common.myStr(Request.QueryString["UserList"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("TCSTAXReport"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ServiceTaxandTCSTaxReport";

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("chrfromdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("chrTodate", common.myStr(todate.ToString("yyyy/MM/dd")));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("CmpMarkup"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CompanyMarkupDetails";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
            p[5] = new ReportParameter("chvErrorMessage", common.myStr(0));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("CmpMarkupService"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPDCompanyMarkUpServiceWiseDetails";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("intEncodedby", common.myStr(Session["UserID"]));
            p[5] = new ReportParameter("chvErrorMessage", common.myStr(0));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("Sponsorwise"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OccupancySponsorWiseReport";

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("intHospitallocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("Type", common.myStr("T"));



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("ProcedureIPDdetails"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ServiceRegisterSurgeonByfurcation";
            ReportParameter[] p = new ReportParameter[9];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IPNo"));
            p[4] = new ReportParameter("Userid", common.myStr(Session["UserID"]));
            p[5] = new ReportParameter("Doctor", common.myStr(0));
            p[6] = new ReportParameter("OPIP", common.myStr(0));
            p[7] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[8] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("OutstandingCollection"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OutstandingCollection";
            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Todate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[3] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());

            p[4] = new ReportParameter("CompanyId", common.myStr(Request.QueryString["CompanyCode"]));
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]).ToString());
            p[6] = new ReportParameter("EntrySite", "A");
            p[7] = new ReportParameter("Userid", sUserName);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("DoctorAccountingWise"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MISDoctorShareStatementSummary";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("SummaryDetail", common.myStr(Request.QueryString["DoctorType"]).ToString());
            p[5] = new ReportParameter("User_id", common.myStr(Session["UserID"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("CustomBLKIPInvoiceLineItem"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_BLK_IPInvoiceLine_Item";

            ReportParameter[] p = new ReportParameter[5];


            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));
            if (common.myStr(Request.QueryString["rdlreport"]) == "True")
            {
                p[3] = new ReportParameter("SurgeryID", common.myStr(0));
                p[4] = new ReportParameter("SpecialisationId", common.myStr("A"));
            }
            else
            {
                p[3] = new ReportParameter("SurgeryID", common.myStr(0));
                p[4] = new ReportParameter("SpecialisationId", common.myStr("A"));
            }

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }



        if (common.myStr(Request.QueryString["ReportName"]).Equals("DoctorAccountingWiseDetail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MISDoctorShareStatementDetail";

            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[5] = new ReportParameter("SummaryDetail", common.myStr(Request.QueryString["DoctorType"]).ToString());
            if (common.myStr(Request.QueryString["Doctordetail"]) == "True")
            {
                p[6] = new ReportParameter("vcDoctorCodes", common.myStr(0));
            }
            else
            {
                p[6] = new ReportParameter("vcDoctorCodes", common.myStr("2344"));
            }
            p[7] = new ReportParameter("User_id", common.myStr(Session["UserID"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("ProcedureWiseRevenue"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/SurgeryProcedurewiseRevenue";
            ReportParameter[] p = new ReportParameter[9];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("IPNO", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IPNo"));
            p[4] = new ReportParameter("Userid", sUserName);
            if (common.myStr(Request.QueryString["GrbType"]).Equals(""))
            {
                p[5] = new ReportParameter("Doctor", common.myStr("A"));
            }
            else
            {
                p[5] = new ReportParameter("Doctor", common.myStr(" "));
            }
            p[6] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]).ToString());
            p[7] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[8] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("ContributionDoctor"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MIS_RevenueContributionByPerformingDoctor";
            ReportParameter[] p = new ReportParameter[6];

            //p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[0] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]).ToString());
            p[2] = new ReportParameter("ReportHeader", "Contribution By Performing Speciality and Company Type Wise");
            p[3] = new ReportParameter("User_id", common.myStr(Session["UserID"]));
            p[4] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[5] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        if (common.myStr(Request.QueryString["ReportName"]).Equals("InvestigateReport"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ServiceWiseAnalysis_BLK";
            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));

            p[1] = new ReportParameter("modid", common.myStr(Request.QueryString["Mode"]).ToString());

            p[2] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        if (common.myStr(Request.QueryString["ReportName"]).Equals("MisDashBoard"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardCurrentAdmissionDetails_BLK";
            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        if (common.myStr(Request.QueryString["ReportName"]).Equals("CountryWisePatient"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisdashBoard_current_admission_countrywise";
            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("DischargeWithoutInvoice"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Discharge Without Bills";
            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("Sponsortypewiserevenue"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CategorywiseStatBlk";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));
            p[3] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }



        if (common.myStr(Request.QueryString["ReportName"]).Equals("Countrywisemonthwiserevenue"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CountrywiseMonthwiseRev2";
            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("AdmissionUpgrade"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionUpgrade";
            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }



        if (common.myStr(Request.QueryString["ReportName"]).Equals("SponsorwiseOutstanding"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_CompanyTypeWise_Outstanding_Coollection_Reve_Summary";
            ReportParameter[] p = new ReportParameter[3];

            //p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[0] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }



        if (common.myStr(Request.QueryString["ReportName"]).Equals("DepositExhaustCash"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DepositExhaustIPDaily";
            ReportParameter[] p = new ReportParameter[6];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("CurrDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[4] = new ReportParameter("IPNo", (string)HttpContext.GetGlobalResourceObject("PRegistration", "IPNo"));
            p[5] = new ReportParameter("UserId", common.myStr(Session["UserID"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }



        if (common.myStr(Request.QueryString["ReportName"]).Equals("ConcessionSummary"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ConcessionSummaryReport";


            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("User_id", common.myStr(Session["UserID"]));
            p[3] = new ReportParameter("ReportHeader", "Concession Report Summary Authorization Wise");
            p[4] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[5] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[6] = new ReportParameter("OPIP", "B");
            p[7] = new ReportParameter("IPNO", common.myStr(0));
            p[8] = new ReportParameter("EntrySite", "A");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }


        if (common.myStr(Request.QueryString["ReportName"]).Equals("RevenueReport"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Rev Report";

            ReportParameter[] p = new ReportParameter[2];

            p[0] = new ReportParameter("mydate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        if (common.myStr(Request.QueryString["ReportName"]).Equals("Sourcewisetypewiserev_UnderPackage_New_Pharma"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Sourcewisetypewiserev_UnderPackage_New_Pharma";

            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("Facilityid", common.myStr(Session["FacilityId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        if (common.myStr(Request.QueryString["ReportName"]).Equals("RevenueReportMKt"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Rev Report Mkt";

            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }


        if (common.myStr(Request.QueryString["ReportName"]).Equals("DailyMISAutomail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DailyMIS_ReportAutomail";

            ReportParameter[] p = new ReportParameter[1];

            p[0] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }




        if (common.myStr(Request.QueryString["ReportName"]).Equals("MonthWiseDoctorPerformance"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MonthWiseDoctorPerformanceReport";

            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("UserId", common.myStr(Session["UserID"]));
            p[1] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Loc_id", Session["HospitalLocationID"].ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }






        if (common.myStr(Request.QueryString["ReportName"]).Equals("OPDBillingReport"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPDRegistrationBillingReport";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[3] = new ReportParameter("chrFromdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("chrtoDate", common.myStr(todate.ToString("yyyy/MM/dd")));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }




        else if (common.myStr(Request.QueryString["ReportName"]).Equals("OPTOIPConversion"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPTOIPConversion";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", strUserName);
            p[3] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("AmbulanceBillingOPD"))
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AmbulanceBilling";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[3] = new ReportParameter("HospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[4] = new ReportParameter("userid", strUserName);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "Refund")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RefundReport";

            ReportParameter[] p = new ReportParameter[10];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("User_id", strUserName);
            p[4] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[5] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[7] = new ReportParameter("EncodedBy", "A");
            p[8] = new ReportParameter("Currency", "Rs.");

            p[9] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "BookingList")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BedBookingReport";
            ReportParameter[] p = new ReportParameter[13];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("intBookingId", common.myInt(Request.QueryString["BI"]).ToString());
            p[3] = new ReportParameter("chvBookingNo", common.myStr(Request.QueryString["BN"]));
            p[4] = new ReportParameter("chvRegistrationNo", common.myStr(Request.QueryString["RN"]));
            p[5] = new ReportParameter("chvDoctorName", common.myStr(Request.QueryString["DN"]));
            p[6] = new ReportParameter("chrBookingType", common.myStr(Request.QueryString["BT"]));
            p[7] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[8] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[9] = new ReportParameter("ReportHeader", "Bed Booking Details");
            p[10] = new ReportParameter("reportType", common.myStr(Request.QueryString["RT"]));
            p[11] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[12] = new ReportParameter("EncounterId", common.myStr(Session["Encounterid"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "BillRegister")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BILLREGISTER";

            ReportParameter[] p = new ReportParameter[13];


            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("PatientType", common.myStr(Request.QueryString["PatientType"]));
            p[3] = new ReportParameter("User_id", strUserName);
            p[4] = new ReportParameter("CompanyCode", sbIdList);// strIdList);
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[8] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[9] = new ReportParameter("ReportHeader", "Bill Register");
            p[10] = new ReportParameter("OutType", " ");
            p[11] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[12] = new ReportParameter("ForExport", common.myStr(Request.QueryString["Export"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("CompSattlement"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceSettlementCompanyBatch";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("hospLocationID", common.myStr(common.myInt(Session["HospitalLocationID"])));
            p[1] = new ReportParameter("facilityID", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("BatchNo", common.myInt(Request.QueryString["BatchNo"]).ToString());
            p[3] = new ReportParameter("UserId", sUserName);
            p[4] = new ReportParameter("bitUnderPackage", common.myStr(Request.QueryString["UnderPackage"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "OPVisit")
        {
            if (common.myStr(Request.QueryString["OPVisitType"]) == "True")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPVisitDetail";
            else
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPVisitSummary";



            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("chrFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("chrToDate", " ");

            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("bitUnderPackage", common.myStr(Request.QueryString["UnderPackage"]));
            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[7] = new ReportParameter("Doctor", strDoctorId);



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "CashCollection")
        {
            if (common.myStr(Request.QueryString["Summary"]).Equals("True"))
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCollection_UserWise";
            else if (common.myStr(Request.QueryString["DW"]).Equals("Y") && common.myBool(Request.QueryString["ModeWise"]))
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCollection_ModeWise";
            else
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCollection";
            ReportParameter[] p;
            if (!common.myBool(Request.QueryString["ModeWise"]) || common.myBool(Request.QueryString["Summary"]))
                p = new ReportParameter[9];
            else if (common.myBool(Request.QueryString["ModeWise"]))
            {
                string[] ModeName = common.myStr(Request.QueryString["ModeName"]).Split(',');
                p = new ReportParameter[10];
                p[9] = new ReportParameter("ModeName", ModeName);

            }
            else
                p = new ReportParameter[9];


            string part2 = "";
            string Part3 = "";

            part2 = frmdatetime.ToString().Substring(frmdatetime.ToString().Length - 11, 11);

            if (part2 == "12:00:00 AM")
            {
                p[0] = new ReportParameter("FDate", common.myStr(frmdatetime.ToString("dd/MM/yyyy 00:00")));
            }
            else
            {
                p[0] = new ReportParameter("FDate", common.myStr(frmdatetime.ToString("dd/MM/yyyy HH:mm")));
            }

            Part3 = frmdatetime.ToString().Substring(frmdatetime.ToString().Length - 11, 11);

            if (Part3 == "12:00:00 AM")
            {
                p[1] = new ReportParameter("TDate", common.myStr(todatetime.ToString("dd/MM/yyyy 23:59")));
            }
            else
            {
                p[1] = new ReportParameter("TDate", common.myStr(todatetime.ToString("dd/MM/yyyy HH:mm")));
            }
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("empcode", strIdList);
            p[4] = new ReportParameter("type", "C");
            p[5] = new ReportParameter("Userid", strUserName);
            p[6] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[7] = new ReportParameter("showPharmacyCol", common.myStr(Request.QueryString["PCall"]));
            p[8] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));

            if (common.myStr(Request.QueryString["ReportName"]) == "CashCollection" && common.myStr(Request.QueryString["isAccountSummary"]) == "1")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisDaySummary";
                p = new ReportParameter[4];
                p[0] = new ReportParameter("Fdate", common.myStr(frmdatetime.ToString("yyyy/MM/dd")));
                p[1] = new ReportParameter("Tdate", common.myStr(todatetime.ToString("yyyy/MM/dd")));
                p[2] = new ReportParameter("IntFacilityId", Session["FacilityId"].ToString());
                p[3] = new ReportParameter("Locid", Session["HospitalLocationID"].ToString());
            }
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "CashCollection_entrysite")
        {
            if (common.myStr(Request.QueryString["Summary"]) == "False")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCollection_entrysite";
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCollection_UserWise_CentreWise";
            }

            ReportParameter[] p = new ReportParameter[10];


            string part2 = "";
            string Part3 = "";

            part2 = frmdatetime.ToString().Substring(frmdatetime.ToString().Length - 11, 11);

            if (part2 == "12:00:00 AM")
            {
                p[0] = new ReportParameter("FDate", common.myStr(frmdatetime.ToString("dd/MM/yyyy 00:00")));
            }
            else
            {
                p[0] = new ReportParameter("FDate", common.myStr(frmdatetime.ToString("dd/MM/yyyy HH:mm")));
            }

            Part3 = frmdatetime.ToString().Substring(frmdatetime.ToString().Length - 11, 11);

            if (Part3 == "12:00:00 AM")
            {
                p[1] = new ReportParameter("TDate", common.myStr(todatetime.ToString("dd/MM/yyyy 23:59")));
            }
            else
            {
                p[1] = new ReportParameter("TDate", common.myStr(todatetime.ToString("dd/MM/yyyy HH:mm")));
            }
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("empcode", strIdList);
            p[4] = new ReportParameter("type", "C");
            p[5] = new ReportParameter("Userid", strUserName);
            p[6] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[7] = new ReportParameter("showPharmacyCol", common.myStr(Request.QueryString["PCall"]));
            p[8] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[9] = new ReportParameter("entrysite", strentryid);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "CreditNote")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CreditNote";

            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("chrFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("chrToDate", " ");

            p[4] = new ReportParameter("UserId", sUserName);

            if (common.myStr(Request.QueryString["Status"]) == "A")
            {
                p[5] = new ReportParameter("HeaderName", "Credit Note / Write Off");
            }
            else if (common.myStr(Request.QueryString["Status"]) == "W")
            {
                p[5] = new ReportParameter("HeaderName", "Write Off");

            }
            else if (common.myStr(Request.QueryString["Status"]) == "D")
            {

                p[5] = new ReportParameter("HeaderName", "Credit Note");
            }


            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[7] = new ReportParameter("Status", common.myStr(Request.QueryString["Status"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DWS")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDepartmentWiseSurgery";

            ReportParameter[] p = new ReportParameter[7];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("FDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("TDate", " ");
            p[5] = new ReportParameter("ReportHeader", "Departmentwise Surgery Details");
            p[4] = new ReportParameter("User_id", sUserName);
            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "BOccup")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BedOccupancyDateRange";

            ReportParameter[] p = new ReportParameter[6];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("FDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("TDate", " ");
            p[5] = new ReportParameter("ReportHeader", "Bed Occupancy Details");
            p[4] = new ReportParameter("User_id", sUserName);



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "ICUUti")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ICUTransferUtilization";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("iHospitalLocationId", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("iFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "InPStat")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptAdmissionDischargeStatistics";

            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("FDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("TDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("TDate", " ");

            p[4] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            p[5] = new ReportParameter("User_id", sUserName);
            p[6] = new ReportParameter("ReportHeader", "InPatient Statistics");
            p[7] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "BRITHREG")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/BirthRegister";

            ReportParameter[] p = new ReportParameter[5];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("chrFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("chrToDate", " ");
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DEFICIENCYRPT")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DeficiencyReport";

            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("iHospitalLocationId", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("iFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("sFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("sToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("user_id", strUserName);

            p[5] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[6] = new ReportParameter("iDeficiencyCategoryId", common.myStr(Request.QueryString["DeficiencyCategoryId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "TREATMENTOPIPREPORT")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/TreatmentOPIPReport";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RID"]));
            p[3] = new ReportParameter("sFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("sToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("sRegistrationNo", common.myStr(Request.QueryString["RNO"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "ReAdmForm")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ReAdmittedList";

            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("NoOfDays", common.myStr(Request.QueryString["NoOfDays"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DIRECTBILLWOENC")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DirectBillingForLab";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("iHospitalLocationId", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("iFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("sFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("sToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("user_id", strUserName);
            p[5] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DEATHREG")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MRDDeathRegister";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("intHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("chrFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("chrToDate", " ");
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "MRDSURG")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/SurgeryReport";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("dtFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("dtFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("dtToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("dtToDate", " ");

            p[4] = new ReportParameter("ChvOTStatusId", (common.myStr(Request.QueryString["OTStatusIds"]).Trim() == "") ? " " : common.myStr(Request.QueryString["OTStatusIds"]).Trim());
            p[5] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "MRDWELLNESS")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/WelnessReport";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
                p[2] = new ReportParameter("dtFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            else
                p[2] = new ReportParameter("dtFromDate", " ");

            if (todate != null)
                p[3] = new ReportParameter("dtToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            else
                p[3] = new ReportParameter("dtToDate", " ");
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "MRDOPVISITSUMM")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPVisitSummaryMRD";
            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("bitUnderPackage", common.myStr(Request.QueryString["UPackage"]));
            p[5] = new ReportParameter("UserId", Session["UserId"].ToString());


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "InvoiceClassified")
        {
            if (common.myStr(Request.QueryString["ReportType"]) == "B")
            {
                //ReportViewer1.ServerReport.ReportPath = "/EMRReports/InvoiceDetailReport";
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassified";
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassified";
            }


            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("UserId", strUserName);
            p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            p[6] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            p[7] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[8] = new ReportParameter("CompanyId", common.myStr(Request.QueryString["Sponsor"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "InvoiceClassifiedS")
        {
            if (common.myStr(Request.QueryString["Summary"]) == "Y")
            {
                //ReportViewer1.ServerReport.ReportPath = "/EMRReports/InvoiceDetailReport";
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassifiedReportSummary";
            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassifiedReport";
            }


            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("UserId", strUserName);
            p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            //p[6] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            p[6] = new ReportParameter("ReportType", "D");
            p[7] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));
            p[8] = new ReportParameter("CompanyId", common.myStr(Request.QueryString["Sponsor"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "OPHealthPackage")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPHealthPackageDetail";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("Locid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            //p[6] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            //p[2] = new ReportParameter("UserId", strUserName);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        /////////////////////////////////////////////////////

        else if (common.myStr(Request.QueryString["ReportName"]) == "RegCard")
        {

            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeReportEr";
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationCard";

            ReportParameter[] p = new ReportParameter[3];

            p[1] = new ReportParameter("FacilityID", Session["FacilityId"].ToString());
            p[0] = new ReportParameter("HospitalLocationID", Session["HospitalLocationID"].ToString());
            //if (frmdate != null)
            //{
            //    string part2 = "";
            //    part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);
            //    if (part2 == "12:00:00 AM")
            //        p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            //    else
            //        p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            //}
            //else
            //{
            //    p[2] = new ReportParameter("chrFromDate", "1900/01/01");
            //}
            //if (todate != null)
            //    p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            //else
            //    p[3] = new ReportParameter("chrToDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            p[2] = new ReportParameter("regID", Request.QueryString["Vlue"]);


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DonationCard")
        {

            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeReportEr";
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DonorRegistrationForm";

            ReportParameter[] p = new ReportParameter[5];

            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[0] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());

            p[2] = new ReportParameter("chvRegistrationNo", Request.QueryString["Vlue"]);

            p[3] = new ReportParameter("chvEncounterNo", "0");
            p[4] = new ReportParameter("intEncodedBy", Session["UserId"].ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "DonorCard")
        {

            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeReportEr";
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DonorCard";

            ReportParameter[] p = new ReportParameter[5];


            p[0] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[2] = new ReportParameter("chvRegistrationNo", Request.QueryString["Vlue"]);

            p[3] = new ReportParameter("chvEncounterNo", "0");
            p[4] = new ReportParameter("intEncodedBy", Session["UserId"].ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }









        ////////////////////////////////////////////////

        else if (common.myStr(Request.QueryString["ReportName"]) == "AnesthsiaFee")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptAnesthsiaFee";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("User_id", strUserName);
            p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "ReceiptSettlementDateWise")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ReceiptSettlementDetailDateWise";

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("hospLocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("facilityID", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("ComapanyId", common.myStr(Request.QueryString["ComapanyId"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "ReceiptSettlementReceiptWise")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ReceiptSettlementReceiptWise";

            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("hospLocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("facilityID", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("ReceiptNo", common.myStr(Request.QueryString["ReceiptNo"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "DEPARTMENTWISEREVENUEUnBilled")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DEPARTMENTWISEREVENUEUnBilled";

            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("User_id", sUserName);
            p[3] = new ReportParameter("GrbType", common.myStr(Request.QueryString["Type"]));
            p[4] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("ReportHeader", "Department Wise Revenue ");
            p[7] = new ReportParameter("Currency", "Rs.");
            p[8] = new ReportParameter("DCode", common.myStr("A"));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "MisEquipmentUtilisation")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisEquipmentUtilisation";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("Type", common.myStr(Request.QueryString["Type"]));
            p[4] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "MisOTUtilisation")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisOTUtilisation";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("Type", common.myStr(Request.QueryString["Type"]));
            p[4] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "MisRevenueVsDoctorPayout")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisRevenueVsDoctorPayout";


            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("chvYear", common.myStr(Request.QueryString["StrYear"]));

            p[1] = new ReportParameter("chvMonth", common.myStr(Request.QueryString["StrMonth"]));
            p[2] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[3] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            //p[4] = new ReportParameter("FilterType", Request.QueryString["Type"]);
            p[4] = new ReportParameter("FilterType", common.myStr(Request.QueryString["FilterType"]));
            p[5] = new ReportParameter("FilterValue", common.myStr(Request.QueryString["FilterValue"]));

            p[6] = new ReportParameter("chvType", common.myStr(Request.QueryString["chvType"]));
            //p[7] = new ReportParameter("DCode", common.myStr(Request.QueryString["DCode"]));
            //p[7] = new ReportParameter("ReportHeader", "Department Wise Revenue ");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "MisOPIPConvertion")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisOPIPConvertion";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            //p[3] = new ReportParameter("Type", common.myStr(Request.QueryString["Type"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "MisOPIPReferalDoctorFee") //OPIP Regferal Doctor Fee
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisOPIPReferalDoctorFee";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("FDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            //p[3] = new ReportParameter("Type", common.myStr(Request.QueryString["Type"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "MisSpecialityWiseStatistics") //OPIP Regferal Doctor Fee
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisSpecialityWiseStatistics";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("IntHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            //p[3] = new ReportParameter("Type", common.myStr(Request.QueryString["Type"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }



        //MisDashBoardBusinessAnalysis

        else if (common.myStr(Request.QueryString["ReportName"]) == "MisDashBoardBusinessAnalysis")// Dasbooard Bussiness Anlysis
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisDashBoardBusinessAnalysis";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("CDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("ChvType", common.myStr(Request.QueryString["ChvType"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "MisMaterialUsedInSurgeryBill")// MisMaterialUsedInSurgeryBill Report
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisMaterialUsedInSurgeryBill";
            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Userid", sUserName);
            p[3] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("Type", common.myStr(Request.QueryString["ChvType"]));
            p[6] = new ReportParameter("ChvUnderPackage", common.myStr(Request.QueryString["ChvUnderPackage"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "EclaimSubmittionreportDetail")// EclaimSubmittionreportDetail Report
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/EclaimSubmittionreportDetail";
            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("dtDateFrom", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("dtDateTo", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("type", common.myStr(Request.QueryString["reportType"]));
            p[6] = new ReportParameter("companyid", common.myStr(Request.QueryString["CompanyId"]));
            p[7] = new ReportParameter("invoiceNo", common.myStr(Request.QueryString["InvoiceNo"]));
            p[8] = new ReportParameter("DispatachNo", common.myStr(Request.QueryString["DispatchNo"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "EclaimSubmittionReportSummary")// EclaimSubmittionreportDetail Report
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/EclaimSubmittionReportSummary";
            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("dtDateFrom", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("dtDateTo", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("type", common.myStr(Request.QueryString["reportType"]));
            p[6] = new ReportParameter("companyid", common.myStr(Request.QueryString["CompanyId"]));
            p[7] = new ReportParameter("invoiceNo", common.myStr(Request.QueryString["InvoiceNo"]));
            p[8] = new ReportParameter("DispatachNo", common.myStr(Request.QueryString["DispatchNo"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        //if (common.myStr(Request.QueryString["ReportName"]) == "UNA")
        //{
        //    ReportViewer1.ServerReport.ReportPath = "/EMRReports/CompanyUnadjusmentAdvance";

        //    ReportParameter[] p = new ReportParameter[7];

        //    p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
        //    p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
        //    p[2] = new ReportParameter("Locid", common.myStr(Session["HospitalLocationId"]));
        //    p[3] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
        //    p[4] = new ReportParameter("user_id", strUserName);
        //    p[5] = new ReportParameter("CompanyCode", "0");
        //    p[6] = new ReportParameter("OPDIPD", Convert.ToString(Request.QueryString["OPIP"]));



        //    ReportViewer1.ServerReport.SetParameters(p);
        //    ReportViewer1.ServerReport.Refresh();
        //}
        else if (common.myStr(Request.QueryString["ReportName"]) == "UNA")
        {
            if (common.myStr(Request.QueryString["AdvType"]) == "P")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientUnadjusmentAdvance";
            else
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CompanyUnadjusmentAdvance";

            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("fdate", common.myStr(Request.QueryString["FromDate"]));
            p[1] = new ReportParameter("tdate", common.myStr(Request.QueryString["ToDate"]));
            p[2] = new ReportParameter("Locid", common.myStr(Session["HospitalLocationId"]));
            p[3] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("user_id", strUserName);
            p[5] = new ReportParameter("CompanyCode", "0");
            p[6] = new ReportParameter("OPDIPD", Convert.ToString(Request.QueryString["OPIP"]));
            p[7] = new ReportParameter("ASONDATE", common.myStr(Request.QueryString["asondate"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "CUSTOMMIS")
        {
            if (common.myStr(Request.QueryString["RptType"]) == "KPI")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_KPI_Dashboard";
            else if (common.myStr(Request.QueryString["RptType"]) == "OST")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_MisDash_Operational_Statistics";
            else if (common.myStr(Request.QueryString["RptType"]) == "RST")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_MisDash_Revenue_Statistics";
            else if (common.myStr(Request.QueryString["RptType"]) == "NIP")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_MisDash_Non_Invasive_Procedure_Statistics";
            else if (common.myStr(Request.QueryString["RptType"]) == "BST")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_MisDash_Beds_Statistics";

            ReportParameter[] p;

            if (common.myStr(Request.QueryString["RptType"]) == "KPI" || common.myStr(Request.QueryString["RptType"]) == "BST" || common.myStr(Request.QueryString["RptType"]) == "NIP")
                p = new ReportParameter[6];
            else
                p = new ReportParameter[5];


            p[0] = new ReportParameter("IntYear", common.myStr(Request.QueryString["IntYear"]));
            p[1] = new ReportParameter("chvMonth", common.myStr(Request.QueryString["chvMonth"]));
            p[2] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
            p[3] = new ReportParameter("IntHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[4] = new ReportParameter("FType", common.myStr(Request.QueryString["FType"]));
            if (common.myStr(Request.QueryString["RptType"]) == "KPI" || common.myStr(Request.QueryString["RptType"]) == "BST" || common.myStr(Request.QueryString["RptType"]) == "NIP")
                p[5] = new ReportParameter("UnderPackage", common.myStr(Request.QueryString["UnderPackage"]));



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]) == "Census")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Census";

            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("dtDate", Convert.ToDateTime(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd"));
            p[1] = new ReportParameter("intHospitalLocationid", common.myInt(Session["HospitalLocationID"]).ToString());
            p[2] = new ReportParameter("intFacilityid", common.myInt(Session["FacilityId"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("VATREPORT"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Phr_VAT_Report_BillBased";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("SurgeryListOrderBase"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/SurgeryListOrderBase";
            ReportParameter[] p = new ReportParameter[3];

            //p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[0] = new ReportParameter("IntFacilityid", common.myInt(Session["FacilityId"]).ToString());
            p[1] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("SurgeryReportDischargeDatebase"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/SurgeryReportDischargeDatebase";
            ReportParameter[] p = new ReportParameter[3];

            //p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[0] = new ReportParameter("IntFacilityid", common.myInt(Session["FacilityId"]).ToString());
            p[1] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MDS"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MisDailySummaryData";
            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("TDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("IntFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("IntHospitalLocationid", common.myInt(Session["HospitalLocationID"]).ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("TDS"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/TDSRepot";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("UCH"))//Users Collection Handover
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/UserCollectionHandover";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("FDate", common.myStr(frmdate.ToString("dd/MM/yyyy")));
            p[3] = new ReportParameter("TDate", common.myStr(todate.ToString("dd/MM/yyyy")));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("UCAck"))//Users Collection Acknowledge
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DenominationCashCollectionAcknowledge";
            ReportParameter[] p = new ReportParameter[5];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("docNo", common.myStr(Request.QueryString["DocNo"]));
            p[3] = new ReportParameter("Userid", common.myStr(Session["User"]));
            p[4] = new ReportParameter("isDuplicate", common.myStr(Request.QueryString["Duplicate"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("DEPTISSUE"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PhrCustomDepartmentIssue";
            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("IntFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[3] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[4] = new ReportParameter("LoginStoreId", common.myInt(Request.QueryString["LoginStoreId"]).ToString());
            p[5] = new ReportParameter("ChvStoreId", common.myStr(Request.QueryString["StoreIds"]).ToString());
            p[6] = new ReportParameter("chvItemId", common.myStr(Request.QueryString["ItemIds"]).ToString());
            p[7] = new ReportParameter("GrpType", common.myStr(Request.QueryString["GrpType"]).ToString());

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "IPOPBillRegister")
        {
            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/IPOPBILLREGISTER";
            //ReportParameter[] p = new ReportParameter[13];
            //p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            //p[1] = new ReportParameter("intFacilityId",common.myStr(Request.QueryString["FacilityId"]));
            //p[2] = new ReportParameter("PatientType", "A");
            //p[3] = new ReportParameter("User_id", strUserName);
            //p[4] = new ReportParameter("CompanyCode", "A");
            //p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
            //p[6] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            //p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            //p[8] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            //if (common.myStr(Request.QueryString["SourceType"]) == "I")
            //{
            //    p[9] = new ReportParameter("ReportHeader", "IPD Billing Report");
            //}
            //else
            //{
            //    p[9] = new ReportParameter("ReportHeader", "OPD Billing Report");
            //}
            //if (common.myStr(Request.QueryString["ReportType"]) == "A")
            //{
            //    p[10] = new ReportParameter("OutType", "OA");
            //}
            //else
            //{
            //    p[10] = new ReportParameter("OutType", "OD");
            //}
            //p[11] = new ReportParameter("EntrySite", "A");
            //p[12] = new ReportParameter("ForExport", "N");
            ReportParameter[] p;
            if (common.myStr(Request.QueryString["SourceType"]) == "I")
            {
                p = new ReportParameter[7];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassifiedReportCustomizeIP";
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityId"]));
                p[2] = new ReportParameter("UserId", strUserName);
                p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
                //  p[6] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
                p[6] = new ReportParameter("ReportType", "T");

            }
            else if (common.myStr(Request.QueryString["SourceType"]) == "O")
            {
                p = new ReportParameter[7];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceClassifiedReportCustomizeOP";
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityId"]));
                p[2] = new ReportParameter("UserId", strUserName);
                p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
                p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
                p[6] = new ReportParameter("ReportType", "T");

            }
            else //c
            {
                p = new ReportParameter[5];
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CashCompany";
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityId"]));
                p[2] = new ReportParameter("UserId", strUserName);
                p[3] = new ReportParameter("chvFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
                p[4] = new ReportParameter("chvToDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            }
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MRDNEO"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/mrdNeoplasinReport";
            ReportParameter[] p = new ReportParameter[10];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("cOPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("cGroupType", "A");
            p[4] = new ReportParameter("dFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("dToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("sICDID", "A");
            p[7] = new ReportParameter("chvICDCode", "''");
            p[8] = new ReportParameter("chvDoctorIdCSV", "''");
            p[9] = new ReportParameter("ICDFlagId", common.myStr(Request.QueryString["ICDFlagId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MRDNOT"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/mrdNotifiedInfectionAndOtherDetails";
            ReportParameter[] p = new ReportParameter[11];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("cOPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("cGroupType", "A");
            p[4] = new ReportParameter("dFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("dToDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            p[6] = new ReportParameter("sICDID", "A");
            p[7] = new ReportParameter("chvICDCode", "''");
            p[8] = new ReportParameter("chvDoctorIdCSV", "''");
            p[9] = new ReportParameter("cReportType", "A W");
            p[10] = new ReportParameter("ICDFlagId", common.myStr(Request.QueryString["ICDFlagId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("DiseaseSt"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptDiagnosisDiesease";
            ReportParameter[] p = new ReportParameter[12];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("cOPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("cGroupType", "A");
            p[4] = new ReportParameter("dFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("dToDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            p[6] = new ReportParameter("sICDID", new string[] { "" }, false);
            p[7] = new ReportParameter("chvICDCode", new string[] { "" }, false);
            p[8] = new ReportParameter("chvDoctorIdCSV", new string[] { "" }, false);
            p[9] = new ReportParameter("cReportType", "A W");
            p[10] = new ReportParameter("iHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[11] = new ReportParameter("iFacilityId", common.myInt(Session["FacilityId"]).ToString());



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("DiseaseStM"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptDiagnosisDieseaseMonthWise";
            ReportParameter[] p = new ReportParameter[12];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("cOPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("cGroupType", "A");
            p[4] = new ReportParameter("dFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("dToDate", common.myStr(todate.ToString("yyyy/MM/dd")));

            p[6] = new ReportParameter("sICDID", new string[] { "" }, false);
            p[7] = new ReportParameter("chvICDCode", new string[] { "" }, false);
            p[8] = new ReportParameter("chvDoctorIdCSV", new string[] { "" }, false);
            p[9] = new ReportParameter("cReportType", "A W");
            p[10] = new ReportParameter("iHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[11] = new ReportParameter("iFacilityId", common.myInt(Session["FacilityId"]).ToString());


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("FORMP"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/mrdReportFormP";
            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("cOPIP", common.myStr(Request.QueryString["SourceType"]));
            p[3] = new ReportParameter("cGroupType", "A");
            p[4] = new ReportParameter("dFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[5] = new ReportParameter("dToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[6] = new ReportParameter("chvUserName", strUserName);
            p[7] = new ReportParameter("GroupId", common.myStr(Request.QueryString["GroupId"]));
            p[8] = new ReportParameter("SubGroupId", common.myStr(Request.QueryString["SubGroupId"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MLCDetail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptMLCDetails";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("Fdate", common.myStr(common.myDate(Request.QueryString["Fromdate"]).ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(common.myDate(Request.QueryString["Todate"]).ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("inthospitallocationid", common.myInt(Session["HospitalLocationID"]).ToString());
            p[3] = new ReportParameter("chvFacilityid", common.myInt(Session["FacilityId"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("TeleGRN"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/TallyImport_GRN";
            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("facilityid", common.myInt(Session["FacilityId"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("TeleGRNReturn"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/TallyImport_GRNReturn";
            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("facilityid", common.myInt(Session["FacilityId"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]).Equals("ProvisionalDiagnosisReport"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptProvisionalDiagnosis";
            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("intDiagnosisSearchId", common.myInt(Request.QueryString["DiagnosisSearchId"]).ToString());
            p[1] = new ReportParameter("intHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[2] = new ReportParameter("intEncodedBy", common.myInt(Session["UserID"]).ToString());
            p[3] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[4] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[5] = new ReportParameter("dtFromDate", common.myDate(Request.QueryString["Fromdate"]).ToString());
            p[6] = new ReportParameter("dtToDate", common.myDate(Request.QueryString["Todate"]).ToString());
            p[7] = new ReportParameter("chvSearchKeyword", common.myStr(Request.QueryString["SearchKeyword"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("ChiefComplaintReport"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptChiefComplaint";
            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("intComplaintSearchId", common.myInt(Request.QueryString["ComplaintSearchId"]).ToString());
            p[1] = new ReportParameter("intHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[2] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[3] = new ReportParameter("intEncodedBy", common.myInt(Session["UserID"]).ToString());
            p[4] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            p[5] = new ReportParameter("dtFromDate", common.myDate(Request.QueryString["Fromdate"]).ToString());
            p[6] = new ReportParameter("dtToDate", common.myDate(Request.QueryString["Todate"]).ToString());
            p[7] = new ReportParameter("chvSearchKeyword", common.myStr(Request.QueryString["SearchKeyword"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("CustomMedication"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptCustomMedicationDetail";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("intDoctorId", common.myInt("0").ToString());
            p[1] = new ReportParameter("chvDateFrom", common.myStr(Request.QueryString["Fromdate"]));
            p[2] = new ReportParameter("chvDateTo", common.myStr(Request.QueryString["Todate"]));
            p[3] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[4] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("BookingSlip"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptAdmissionBookingSlip";
            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[1] = new ReportParameter("intFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("intBookingId", common.myStr(common.myInt(Request.QueryString["intBookingId"])));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("RenderedServices"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptPrintPackage";
            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("Loc_id", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[1] = new ReportParameter("intFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("BillNo", common.myStr(Request.QueryString["BillNo"]));
            p[3] = new ReportParameter("UHid", common.myStr(Session["RegistrationLabelName"]));
            p[4] = new ReportParameter("Invoiceid", common.myStr(Request.QueryString["Invoiceid"]));
            p[5] = new ReportParameter("year_id", common.myStr(Request.QueryString["year_id"]));
            p[6] = new ReportParameter("intProlongPackageId", common.myStr(Request.QueryString["intProlongPackageId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("InvoiceSettlementCompanyBatch"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/InvoiceSettlementCompanyBatch1";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("hospLocationID", common.myStr(common.myInt(Session["HospitalLocationID"])));
            p[1] = new ReportParameter("facilityID", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("xmlInvoiceList", common.myStr(Request.QueryString["sbxmlInvoiceList"]));
            p[3] = new ReportParameter("UserId", sUserName);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("CreditNoteReceipt"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptCreditNoteReceipt";
            ReportParameter[] p = new ReportParameter[14];
            p[0] = new ReportParameter("intCreditNoteId", Request.QueryString["DocumentID"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[2] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[3] = new ReportParameter("intRegistrationId", "0");
            p[4] = new ReportParameter("chvPatientName", "");
            p[5] = new ReportParameter("chvDocumentNo", Request.QueryString["DocumentNo"].ToString());
            p[6] = new ReportParameter("chrDocumentType", "");
            p[7] = new ReportParameter("dtsFromDate", common.myDate("1753-01-01").ToString("yyyy/MM/dd"));
            p[8] = new ReportParameter("dtsToDate", common.myDate("9999-12-31").ToString("yyyy/MM/dd"));
            p[9] = new ReportParameter("chvRecordButton", "");
            p[10] = new ReportParameter("intRowNo", "0");
            p[11] = new ReportParameter("intPageSize", "1000");
            p[12] = new ReportParameter("intEncodedBy", common.myInt(Session["UserId"]).ToString());
            p[13] = new ReportParameter("chvErrorStatus", "");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (Request.QueryString["rptType"] == "SingleGRN")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptPrintGRN";

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intGRNId", common.myInt(Request.QueryString["GrnId"]).ToString());
            p[2] = new ReportParameter("intStoreId", common.myStr(Request.QueryString["StoreID"]));
            p[3] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[4] = new ReportParameter("chvGRNNo", " ");
            p[5] = new ReportParameter("chvUserName", sUserName);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }



        else if (common.myStr(Request.QueryString["ReportName"]) == "WB")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptWristBandAdult";
            //ReportViewer1.ServerReport.ReportPath = "/MotherVersionReports/rptWristBandAdult";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("intHospitallocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityid", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("EncounterId", common.myStr(Session["encounterid"]));
            p[3] = new ReportParameter("IntRegistratioId", common.myStr(Session["RegistrationID"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "WBC")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptWristBandForKids";
            //ReportViewer1.ServerReport.ReportPath = "/MotherVersionReports/rptWristBandForKids";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("intHospitallocationid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityid", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("EncounterId", common.myStr(Session["encounterid"]));
            p[3] = new ReportParameter("IntRegistratioId", common.myStr(Session["RegistrationID"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        //added by bhakti
        else if (common.myStr(Request.QueryString["ReportName"]) == "Immunization")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientDetailsImmunizationPrint";
            //ReportViewer1.ServerReport.ReportPath = "/MotherVersionReports/rptWristBandForKids";
            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("chvEncounterNo", common.myStr(Session["encounterid"]));
            p[3] = new ReportParameter("intRegistrationId", common.myStr(Session["RegistrationID"]));
            p[4] = new ReportParameter("intRegId", common.myStr(Session["RegistrationID"]));
            p[5] = new ReportParameter("chvRegistrationNo", common.myStr(Session["RegistrationNo"]));
            p[6] = new ReportParameter("dob", common.myStr(Request.QueryString["dob"]));
            // p[7] = new ReportParameter("IntRegistratioId", common.myStr(Session["RegistrationID"]));
            p[7] = new ReportParameter("ReportHeader", "IMMUNIZATION RECORD");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]) == "Adminfo")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionInfo";
            //ReportViewer1.ServerReport.ReportPath = "/MotherVersionReports/rptWristBandForKids";
            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intLoginFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[2] = new ReportParameter("vcIPNO", common.myStr(Session["IPNO"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        else if (common.myStr(Request.QueryString["ReportName"]) == "MRDFileStatus")
        {
            int iDefault = 0;
            string Blankstr = "";
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MRDDischargeFileStatus";
            ReportParameter[] p = new ReportParameter[16];
            p[0] = new ReportParameter("intHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("intRegid", common.myStr(iDefault));
            p[3] = new ReportParameter("intEncid", common.myStr(iDefault));
            p[4] = new ReportParameter("chvRegNo", common.myStr(Blankstr));
            p[5] = new ReportParameter("chvEncNo", common.myStr(Blankstr));
            p[6] = new ReportParameter("chvRackNo", common.myStr(Blankstr));
            p[7] = new ReportParameter("chvPatientName", common.myStr(Blankstr));

            if (common.myStr(common.myStr(frmdate.ToString("yyyy/MM/dd"))) != "0001/01/01")
            {
                p[8] = new ReportParameter("dtsEncFromDate", common.myStr(common.myStr(frmdate.ToString("yyyy-MM-dd"))));
                p[9] = new ReportParameter("dtsEncToDate", common.myStr(common.myStr(todate.ToString("yyyy-MM-dd"))));
            }
            else
            {
                p[8] = new ReportParameter("dtsEncFromDate", common.myStr(null));
                p[9] = new ReportParameter("dtsEncToDate", common.myStr(null));
            }
            p[10] = new ReportParameter("intMRDStatusId", common.myStr(Request.QueryString["MRDStatus"]));
            p[11] = new ReportParameter("bitMoreThenTimeLimit", common.myStr(iDefault));
            p[12] = new ReportParameter("intWardId", common.myStr("A"));
            p[13] = new ReportParameter("CompanyId", common.myStr("A"));
            p[14] = new ReportParameter("intCompanyid", common.myStr(iDefault));
            p[15] = new ReportParameter("chvErrorStatus", "");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        #region ---| Advance Reports |---
        // Done by Abhishek Goel 11 Feb 2016 Begin
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("UnAdjAdv"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceUnadjusted";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("pdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Userid", strUserName);
            p[2] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[3] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            string a = common.myStr(frmdate.ToString("dd/MM/yyyy"));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("UnAdjAdvA"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptAdvanceUnadjustedDetails";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("pdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Userid", strUserName);
            p[2] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[3] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("AdvanceDetail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceDetail";
            ReportParameter[] p = new ReportParameter[9];
            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("CompanyId", strIdList);
            p[4] = new ReportParameter("Userid", strUserName);
            p[5] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[6] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[7] = new ReportParameter("AdvanceTypeId", entryid);
            p[8] = new ReportParameter("IsAdvance", "1");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("ComAdvColl"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceDetailCompanyWise";
            ReportParameter[] p = new ReportParameter[9];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("CompanyId", strIdList);
            p[4] = new ReportParameter("Userid", strUserName);
            p[5] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[6] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[7] = new ReportParameter("AdvanceTypeId", entryid);
            p[8] = new ReportParameter("IsAdvance", "0");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("AdvAdj"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceAdjustedinBill";
            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("CompanyId", strIdList);
            p[4] = new ReportParameter("Userid", strUserName);
            p[5] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[6] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[7] = new ReportParameter("AdvanceTypeId", entryid);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("AdvAdjR"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceAdjustedReceiptDateWise";
            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
            p[3] = new ReportParameter("CompanyId", strIdList);
            p[4] = new ReportParameter("Userid", strUserName);
            p[5] = new ReportParameter("intHospitalLocationId", common.myStr(common.myInt(Session["HospitalLocationId"])));
            p[6] = new ReportParameter("IntFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
            p[7] = new ReportParameter("AdvanceTypeId", entryid);
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        // Done by Abhishek Goel 11 Feb 2016 End
        #endregion

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("IPChargeSlipbasedcosting"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPChargeSlipbasedcosting";
            ReportParameter[] p = new ReportParameter[1];
            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("HPDCW"))//Health Package report
        {
            if (common.myStr(Request.QueryString["Rtype"]) == "S") //Summary
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/HealthPackageCompayWiseSummary";
            }
            else if (common.myStr(Request.QueryString["Rtype"]) == "D") //Details
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/HealthPackageCompayWiseDetail";
            }
            else if (common.myStr(Request.QueryString["Rtype"]) == "B") //Bill No Wise
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/HealthPackageBillNoWise";
            }
            else if (common.myStr(Request.QueryString["Rtype"]) == "C") //Comparision
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/HealthPackageCompayMonthcomparision";
            }

            ReportParameter[] p = new ReportParameter[6];

            p[0] = new ReportParameter("Fdate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("Tdate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("IntFacilityid", common.myInt(Session["FacilityId"]).ToString());
            p[3] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[4] = new ReportParameter("CompanyId", common.myStr(Request.QueryString["CompanyCode"]));
            p[5] = new ReportParameter("PaymentType", common.myStr(Request.QueryString["PatientType"]).ToString());
            // p[6] = new ReportParameter("Userid", sUserName);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        // Done by Chandan 17 Feb 2017 End

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MLCP"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MLCPrintReport";
            ReportParameter[] p = new ReportParameter[3];

            p[0] = new ReportParameter("id", common.myStr(Request.QueryString["Id"]));
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]).ToString());
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("OPDPrintEMR"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/EMRCaseSheetPrint";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RID"]));
            p[3] = new ReportParameter("intEncounterId", common.myStr(Request.QueryString["EID"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("MRDIssueRegisterReports"))
        {


            ReportParameter[] p = new ReportParameter[10];
            p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityId"]));
            p[2] = new ReportParameter("intRegistrationId", "0");
            p[3] = new ReportParameter("chvRegistrationNo", "0");
            p[4] = new ReportParameter("chrFromDate", Convert.ToDateTime(common.myStr(Request.QueryString["Fromdate"])).ToString("yyyy-MM-dd"));
            p[5] = new ReportParameter("chrToDate", Convert.ToDateTime(common.myStr(Request.QueryString["Todate"])).ToString("yyyy-MM-dd"));
            p[6] = new ReportParameter("BitMLC", common.myStr(Request.QueryString["BitMLC"]));
            p[7] = new ReportParameter("chvEncounterno", "0");
            p[8] = new ReportParameter("Reporttype", common.myStr(Request.QueryString["Reporttype"]));
            if (common.myInt(common.myStr(Request.QueryString["Reporttype"])) == 1)
            {
                p[9] = new ReportParameter("Reportheader", "MLC Register");
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptMRDMLCRegisterReports";
            }
            else if (common.myInt(common.myStr(Request.QueryString["Reporttype"])) == 2)
            {
                p[9] = new ReportParameter("Reportheader", "Medical Record Legal Issue Register");
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptMRDLegalissueRegisterReports";
            }
            else if (common.myInt(common.myStr(Request.QueryString["Reporttype"])) == 3)
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/MRDIssueRegister";
                p[9] = new ReportParameter("Reportheader", "Medical Record Issue Register");
            }
            else if (common.myInt(common.myStr(Request.QueryString["Reporttype"])) == 4)
            {
                p[9] = new ReportParameter("Reportheader", "Application Register");
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptMRDpatientApplicationAndIssueRegistered";
            }

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        else if (common.myStr(Request.QueryString["ReportName"]).Equals("DischargeFileReports"))
        {
            ReportParameter[] p = new ReportParameter[12];
            p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Request.QueryString["FacilityId"]));
            p[2] = new ReportParameter("chvRegNo", common.myStr(Request.QueryString["RegNo"]));
            p[3] = new ReportParameter("chvEncNo", common.myStr(Request.QueryString["EncNo"]));
            p[4] = new ReportParameter("chvPatientName", common.myStr(Request.QueryString["PatientName"]));
            p[5] = new ReportParameter("chvRackNo", common.myStr(Request.QueryString["RackNo"]));
            p[6] = new ReportParameter("dtsEncFromDate", Convert.ToDateTime(common.myStr(Request.QueryString["Fromdate"])).ToString("yyyy-MM-dd"));
            p[7] = new ReportParameter("dtsEncToDate", Convert.ToDateTime(common.myStr(Request.QueryString["Todate"])).ToString("yyyy-MM-dd"));
            p[8] = new ReportParameter("intMRDStatusId", common.myStr(Request.QueryString["Status"]));
            p[9] = new ReportParameter("bitMoreThenTimeLimit", common.myStr(Request.QueryString["chklimittime"]));
            p[10] = new ReportParameter("intWardId", common.myStr(Request.QueryString["Ward"]));
            p[11] = new ReportParameter("CompanyId", common.myStr(Request.QueryString["Companyd"]));

            if (common.myStr(Request.QueryString["Status"]).Equals(1))
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDischargeOpenPatientListWardtoMRD";

            }
            else
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptDischargePatientListWardtoMRD";
            }
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]).Equals("NurseAllocate"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/NursesAllocationReport";
            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("fromDate", common.myStr(Request.QueryString["FD"]));
            p[3] = new ReportParameter("todate", common.myStr(Request.QueryString["TD"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
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

            MemoryStream oStream = new MemoryStream();
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
}
