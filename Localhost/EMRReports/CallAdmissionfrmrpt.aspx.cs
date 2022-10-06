using System;
using System.Configuration;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class EMRReports_CallAdmissionfrmrpt : System.Web.UI.Page
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
            // if (common.myStr(Session["IPNO"]) != "")
            ShowReport();
        }
    }

    protected void ShowReport()
    {

        DateTime frmdate = Convert.ToDateTime(Request.QueryString["Fromdate"]);
        DateTime todate = Convert.ToDateTime(Request.QueryString["Todate"]);


        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        //string ReportServerPath = reportServer;
        string ReportServerPath = "http://" + reportServer + "/ReportServer";
        ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        ReportViewer1.ShowCredentialPrompts = false;
        ReportViewer1.ShowFindControls = false;
        ReportViewer1.ShowParameterPrompts = false;

        string ExclusiveForSMIH = common.GetFlagValueHospitalSetup(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()), "ExclusiveForSMIH", sConString);
        if (common.myStr(Request.QueryString["ReportName"]) == "Admfrom")
        {
            if (common.myStr(Request.QueryString["ReportType"]) == "F")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionFormNew";


                ReportParameter[] p = new ReportParameter[6];

                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("vcIPNO", common.myStr(Session["IPNO"]));
                p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[3] = new ReportParameter("User_Id", sUserName);
                p[4] = new ReportParameter("ReasonForPrinting", common.myStr(Request.QueryString["Reason"]));
                p[5] = new ReportParameter("intEncodedBy", common.myStr(Session["UserID"]));

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            //Form60
            else if (common.myStr(Request.QueryString["ReportType"]) == "FS")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Form60";
                ReportParameter[] p = new ReportParameter[5];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("vcIPNO", common.myStr(Session["IPNO"]));
                p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[3] = new ReportParameter("User_Id", sUserName);
                p[4] = new ReportParameter("intEncodedBy", common.myStr(Session["UserID"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

            else if (common.myStr(Request.QueryString["ReportType"]) == "V")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ValuableHandoverform";

                ReportParameter[] p = new ReportParameter[4];

                p[0] = new ReportParameter("Enc_no", common.myStr(Session["IPNO"]));
                p[1] = new ReportParameter("intLoginFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
                p[3] = new ReportParameter("Used_Id", sUserName);

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "R")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RoomRentReport";

                ReportParameter[] p = new ReportParameter[4];

                p[0] = new ReportParameter("Enc_no", common.myStr(Session["IPNO"]));
                p[1] = new ReportParameter("intLoginFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("inyHospitalLocationId", Session["HospitalLocationID"].ToString());
                p[3] = new ReportParameter("Used_Id", sUserName);

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "AR")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PreAdmissionRecordConsentform";

                ReportParameter[] p = new ReportParameter[8];

                p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[3] = new ReportParameter("intRegistrationId", common.myStr(0));
                p[4] = new ReportParameter("chvRegistrationNo", common.myStr(Request.QueryString["RegNo"]));
                p[5] = new ReportParameter("intEncounterId", common.myStr(0));
                p[6] = new ReportParameter("intEncodedBy", Convert.ToString(Session["UserID"]));
                p[7] = new ReportParameter("chvErrorStatus", " ");

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "PD")
            {

                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionInfo";

                ReportParameter[] p = new ReportParameter[3];

                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intLoginFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
                p[2] = new ReportParameter("vcIPNO", common.myStr(Session["IPNO"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "AD")
            {

                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdvanceTagged";

                ReportParameter[] p = new ReportParameter[3];

                p[0] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("iFacilityId", common.myStr(common.myInt(Session["FacilityId"])));
                p[2] = new ReportParameter("sEncounterNo", common.myStr(Session["IPNO"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();

            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "L")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionLable";


                ReportParameter[] p = new ReportParameter[4];

                p[0] = new ReportParameter("Encno", common.myStr(Session["IPNO"]));
                p[1] = new ReportParameter("intLoginFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("ReasonForPrinting", common.myStr(Request.QueryString["Reason"]));
                p[3] = new ReportParameter("intEncodedBy", common.myStr(Session["UserID"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
            else if (common.myStr(Request.QueryString["ReportType"]) == "SL")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/SingleAdmissionLable";
                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("Encno", common.myStr(Session["IPNO"]));
                p[1] = new ReportParameter("intLoginFacilityId", Session["FacilityId"].ToString());
                p[2] = new ReportParameter("ReasonForPrinting", common.myStr(Request.QueryString["Reason"]));
                p[3] = new ReportParameter("intEncodedBy", common.myStr(Session["UserID"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }


            if (common.myStr(Request.QueryString["ReportType"]) == "PS")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientAttendantSatisfactionCertificate";
                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("IPNo", common.myStr(Session["IPNO"]));
                p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[3] = new ReportParameter("RegID", common.myStr(Request.QueryString["UHID"]));
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }


            if (common.myStr(Request.QueryString["ReportType"]) == "EF")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ExtensionFormRequest";
                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["UHID"]));
                p[2] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));


                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }

        }
        //change palendra 13-05-2020
        if (common.myStr(Request.QueryString["ReportName"]) == "ERCheckIN")
        {
            if (common.myStr(Request.QueryString["ReportType"]) == "F")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ERCheckInSheet";


                ReportParameter[] p = new ReportParameter[6];

                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[1] = new ReportParameter("vcIPNO", common.myStr(Request.QueryString["IPNO"]));
                p[2] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityId"]));
                p[3] = new ReportParameter("User_Id", sUserName);
                p[4] = new ReportParameter("ReasonForPrinting", "");
                p[5] = new ReportParameter("intEncodedBy", common.myStr(Session["UserID"]));

                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
        }

        if (common.myStr(Request.QueryString["ReportName"]) == "ERCheckINNew")
        {
            if (common.myStr(Request.QueryString["ReportType"]) == "F")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/ERCheckInSheetNew";
                ReportParameter[] p = new ReportParameter[2];             
                p[0] = new ReportParameter("EncounterNo", common.myStr(Request.QueryString["IPNO"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
               
                ReportViewer1.ServerReport.SetParameters(p);
                ReportViewer1.ServerReport.Refresh();
            }
        }

        if (common.myStr(Request.QueryString["ReportName"]) == "Transfer")
        {

            string strprovidrid = "";
            strprovidrid = common.myStr(Request.QueryString["Provider"]);

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PatientTransferNew";
            ReportParameter[] p = new ReportParameter[13];
            p[0] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            p[1] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            string part2 = "";
            part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);
            if (part2 == "12:00:00 AM")
            {
                p[2] = new ReportParameter("dtsFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[2] = new ReportParameter("dtsFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            }
            p[3] = new ReportParameter("dtsToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[4] = new ReportParameter("chvErrorStatus", " ");
            p[5] = new ReportParameter("userid", sUserName);
            p[6] = new ReportParameter("AcountH", common.myStr(Session["RegistrationLabelName"]));
            p[7] = new ReportParameter("PNameH", "Patient Name");
            p[8] = new ReportParameter("ProviderH", common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "Provider").ToString()));
            p[9] = new ReportParameter("ipnoH", common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
            p[10] = new ReportParameter("sexH", common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "SEX").ToString()));
            p[11] = new ReportParameter("ChvFilter", common.myStr(Request.QueryString["ReportType"]));
            p[12] = new ReportParameter("entrysite", common.myStr(Request.QueryString["EntrySite"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();



        }
        if (common.myStr(Request.QueryString["ReportName"]) == "Admitted")
        {
            string sValue = "";
            //sValue = common.myStr(Request.QueryString["sValue"]);
            sValue = common.myStr(Session["AdmittedasonsbIdList"]);
            string[] strDoctorIds = null;
            if (sValue.Length > 0)
            {
                strDoctorIds = sValue.Split(',');
            }
            else
                sValue = "1";
            ViewState["GroupId"] = common.myStr(Request.QueryString["GroupId"]);

            if (ViewState["GroupId"] == "")
            {
                ViewState["GroupId"] = "Datewise";
            }

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmittedList";



            ReportParameter[] p = new ReportParameter[8];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());

            if (todate != null)
                p[2] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            else
                p[2] = new ReportParameter("chrToDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            p[3] = new ReportParameter("RegitrationNo", common.myStr(Session["RegistrationLabelName"]));
            p[4] = new ReportParameter("GroupType", common.myStr(ViewState["GroupId"]));

            p[5] = new ReportParameter("UserId", sUserName);
            p[6] = new ReportParameter("Reporttype", common.myStr("CA"));
            if (strDoctorIds != null)
                p[7] = new ReportParameter("SetFormula", strDoctorIds);
            else
                p[7] = new ReportParameter("SetFormula", "1");

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();


        }


        if (common.myStr(Request.QueryString["ReportName"]) == "Admittedason")
        {
            string sValue = "";
            sValue = common.myStr(Session["AdmittedasonsbIdList"]);

            string[] strDoctorIds = null;
            if (sValue.Length > 0)
            {
                strDoctorIds = sValue.Split(',');
            }
            else
                sValue = "1";
            ViewState["GroupId"] = common.myStr(Request.QueryString["GroupId"]);

            if (ViewState["GroupId"] == "")
            {
                ViewState["GroupId"] = "Datewise";
            }

            if (Request.QueryString["Summary"] == "False")
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmittedList";
            else
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmittedListSummary";

            ReportParameter[] p = new ReportParameter[9];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());

            if (todate != null)
                p[2] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            else
                p[2] = new ReportParameter("chrToDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            p[3] = new ReportParameter("RegitrationNo", common.myStr(Session["RegistrationLabelName"]));
            p[4] = new ReportParameter("GroupType", common.myStr(ViewState["GroupId"]));

            p[5] = new ReportParameter("UserId", sUserName);
            p[6] = new ReportParameter("Reporttype", common.myStr("CA"));
            if (strDoctorIds != null)
                p[7] = new ReportParameter("SetFormula", strDoctorIds);
            else
                p[7] = new ReportParameter("SetFormula", "1");
            p[8] = new ReportParameter("entrysite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]) == "PatientAck")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Adm_Patient_Acknowledge";


            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationID"].ToString()));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));

            //p[4] = new ReportParameter("UserId", sUserName);
            //p[5] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        if (common.myStr(Request.QueryString["ReportName"]) == "IPPatientStatus")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptMarkedForDischargeReport";


            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"].ToString()));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[2] = new ReportParameter("fromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            p[3] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[4] = new ReportParameter("EncounterId", common.myStr(0));
            //p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("EntrySite", common.myStr(0));
            //common.myStr(Request.QueryString["EntrySite"])
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }



        if (common.myStr(Request.QueryString["ReportName"]) == "DischargeDetailList")
        {
            string sValue = "";
            //sValue = common.myStr(Request.QueryString["sValue"]);
            sValue = common.myStr(Session["AdmittedasonsbIdList"]);
            string[] strDoctorIds = null;
            if (sValue.Length > 0)
            {
                strDoctorIds = sValue.Split(',');
            }
            else
                sValue = "1";
            ViewState["GroupId"] = common.myStr(Request.QueryString["GroupId"]);

            if (ViewState["GroupId"] == "")
            {
                ViewState["GroupId"] = "Datewise";
            }
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeDetailList";


            ReportParameter[] p = new ReportParameter[11];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
            {
                string part2 = "";

                part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);

                if (part2 == "12:00:00 AM")
                    p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
                else
                    p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
                p[7] = new ReportParameter("Reporttype", "CA56");
            }
            else
            {
                p[2] = new ReportParameter("chrFromDate", "1900/01/01");
                p[7] = new ReportParameter("Reporttype", common.myStr("CA"));
            }
            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            else
                p[3] = new ReportParameter("chrToDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            p[4] = new ReportParameter("RegitrationNo", common.myStr(Session["RegistrationLabelName"]));
            p[5] = new ReportParameter("GroupType", common.myStr(ViewState["GroupId"]));

            p[6] = new ReportParameter("UserId", sUserName);

            if (strDoctorIds != null)
                p[8] = new ReportParameter("SetFormula", strDoctorIds);
            else
                p[8] = new ReportParameter("SetFormula", "1");

            p[9] = new ReportParameter("IsEmergencyPatient", "N");
            p[10] = new ReportParameter("entrysite", common.myStr(Request.QueryString["EntrySite"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        if (common.myStr(Request.QueryString["ReportName"]) == "AdmER")
        {

            if (Request.QueryString["Summary"] == "False")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionReportEr";
            }
            else  // 1
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionReportSummary";
            }




            ReportParameter[] p = new ReportParameter[5];


            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());

            string part2 = "";

            part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);

            if (part2 == "12:00:00 AM")
            {
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            }
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[4] = new ReportParameter("UserId", sUserName);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        if (common.myStr(Request.QueryString["ReportName"]) == "Admission")
        {
            string sValue = "";
            //sValue = common.myStr(Request.QueryString["sValue"]);
            sValue = common.myStr(Session["AdmittedasonsbIdList"]);
            string[] strDoctorIds = null;
            if (sValue.Length > 0)
            {
                strDoctorIds = sValue.Split(',');
            }
            else
                sValue = "1";
            ViewState["GroupId"] = common.myStr(Request.QueryString["GroupId"]);

            if (ViewState["GroupId"] == "")
            {
                ViewState["GroupId"] = "Datewise";
            }

            if (Request.QueryString["Summary"] == "False")
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionReport";
            }
            else  // 1
            {
                ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/AdmissionReportSummary";
            }




            ReportParameter[] p = new ReportParameter[9];


            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());

            string part2 = "";

            part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);

            if (part2 == "12:00:00 AM")
            {
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            }
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            if (Request.QueryString["Summary"] == "False")
            {
                p[4] = new ReportParameter("GroupType", common.myStr(ViewState["GroupId"]));
            }
            else
            {
                p[4] = new ReportParameter("GroupType", common.myStr(ViewState["GroupId"]));


            }
            p[5] = new ReportParameter("RegitrationNo", common.myStr(Session["RegistrationLabelName"]));
            p[6] = new ReportParameter("UserId", sUserName);

            if (strDoctorIds != null)
                p[7] = new ReportParameter("SetFormula", strDoctorIds);
            else
                p[7] = new ReportParameter("SetFormula", "1");

            p[8] = new ReportParameter("entrysite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }


        if (common.myStr(Request.QueryString["ReportName"]) == "DischargeListWOBilling")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeListWOBilling";


            ReportParameter[] p = new ReportParameter[6];

            p[0] = new ReportParameter("fromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            p[1] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        if (common.myStr(Request.QueryString["ReportName"]) == "updownbdCatg")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RptPatientBedCategory";


            ReportParameter[] p = new ReportParameter[7];

            p[0] = new ReportParameter("dtFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd")));
            p[1] = new ReportParameter("dtToDate", common.myStr(todate.ToString("yyyy-MM-dd")));
            p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("chvUserName", sUserName);
            p[5] = new ReportParameter("isAdmitted", common.myStr(Request.QueryString["isAdm"]));
            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));



            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        //Added on 29/082013

        if (common.myStr(Request.QueryString["ReportName"]) == "DischargeDetailListEmergency")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeReportEr";


            ReportParameter[] p = new ReportParameter[5];

            p[0] = new ReportParameter("intFacilityId", Session["FacilityId"].ToString());
            p[1] = new ReportParameter("inyHospitalLocationID", Session["HospitalLocationID"].ToString());
            if (frmdate != null)
            {
                string part2 = "";
                part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);
                if (part2 == "12:00:00 AM")
                    p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
                else
                    p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[2] = new ReportParameter("chrFromDate", "1900/01/01");
            }
            if (todate != null)
                p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            else
                p[3] = new ReportParameter("chrToDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            p[4] = new ReportParameter("UserId", sUserName);


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        //added on 24/06/2015
        if (common.myStr(Request.QueryString["ReportName"]) == "Registration")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/RegistrationListAreaWise";

            this.Page.Title = "Regisration List";
            ReportParameter[] p = new ReportParameter[6];

            p[0] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationID"].ToString()));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[2] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            p[5] = new ReportParameter("UserId", sUserName);

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }

        //Added on 13/10/2015
        if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_RevenueWardWise"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_RevenueWardWise";
            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            p[5] = new ReportParameter("CompanyTypeId", "0");
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_ImcomeDoctorTypeWiseSummary"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_ImcomeDoctorTypeWiseSummary";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("DoctorStatuTypeId", common.myStr(Request.QueryString["DoctorStatuTypeId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_ImcomeDoctorTypeWiseDetail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_ImcomeDoctorTypeWiseDetail";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("DoctorStatuTypeId", common.myStr(Request.QueryString["DoctorStatuTypeId"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_IPVisitRevenueBreakup"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_IPVisitRevenueBreakup";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_CreditCardIncome"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_CreditCardIncome";
            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_RevenueLABWise"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_RevenueLABWise";
            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            p[5] = new ReportParameter("UnderPackage", common.myStr(Request.QueryString["UnderPackage"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_PerformanceForThePeriodEnded"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/Custom_PerformanceForThe PeriodEnded";
            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("Custom_SurgeryDetail")
              || common.myStr(Request.QueryString["ReportName"]).Equals("Custom_SurgeonWiseSurgeryDetail")
              || common.myStr(Request.QueryString["ReportName"]).Equals("Custom_TheatreSurgeryDetail"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + common.myStr(Request.QueryString["ReportName"]);
            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("IntHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("IntYear", common.myStr(Request.QueryString["IntYear"]));
            p[3] = new ReportParameter("chvMonth", common.myStr(Request.QueryString["chvMonth"]));
            p[4] = new ReportParameter("UnderPackage", common.myStr(Request.QueryString["UnderPackage"]));
            p[5] = new ReportParameter("FType", "F");
            p[6] = new ReportParameter("IsBillBased", common.myStr(Request.QueryString["IsBillBased"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }


        else if (common.myStr(Request.QueryString["ReportName"]).Equals("OPIPSummaryReport"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPIPSummaryReport";
            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FromDate", common.myStr(Request.QueryString["dtFromDate"]));
            p[3] = new ReportParameter("ToDate", common.myStr(Request.QueryString["dtToDate"]));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["Criteria"]));
            p[5] = new ReportParameter("regno", common.myInt(Request.QueryString["regno"]).ToString());
            p[6] = new ReportParameter("EncounterNo", common.myStr(Request.QueryString["EncounterNo"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }

        if (common.myStr(Request.QueryString["ReportName"]) == "AdmittedPatientWithBillAmount")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/rptAdmittedPatientWithBillAmount";


            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("fromDate", common.myStr(frmdate.ToString("yyyy/MM/dd HH:mm")));
            p[1] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd HH:mm")));
            p[2] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("EncounterId", "0");
            p[5] = new ReportParameter("UserId", common.myStr(Session["UserID"]));
            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));


            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        if (common.myStr(Request.QueryString["ReportName"]) == "IPSAS")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPAdmissionSummary";
            ReportParameter[] p = new ReportParameter[6];
            string part2 = "";
            part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);
            if (part2 == "12:00:00 AM")
            {
                p[0] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[0] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            }
            p[1] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));

            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        if (common.myStr(Request.QueryString["ReportName"]) == "IPSADS")
        {

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPAdmissionDischargeSummary";
            ReportParameter[] p = new ReportParameter[6];
            string part2 = "";
            part2 = frmdate.ToString().Substring(frmdate.ToString().Length - 11, 11);
            if (part2 == "12:00:00 AM")
            {
                p[0] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd 00:00")));
            }
            else
            {
                p[0] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy-MM-dd HH:mm")));
            }
            p[1] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy-MM-dd HH:mm")));
            p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();
        }
        if (common.myStr(Request.QueryString["ReportName"]) == "IPSAOS")
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPOccupancySummary";
            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("chrFromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("chrToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"].ToString()));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"].ToString()));
            p[4] = new ReportParameter("UserId", sUserName);
            p[5] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            ReportViewer1.ServerReport.SetParameters(p);
            ReportViewer1.ServerReport.Refresh();

        }
        //Added on 03/11/2015
        else if (common.myStr(Request.QueryString["ReportName"]).Equals("AreaWiseBussiness"))
        {
            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/CityAreaWiseBusinessreport";
            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("HospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            p[1] = new ReportParameter("FacilityID", common.myInt(Session["FacilityId"]).ToString());
            p[2] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
            p[3] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
            p[4] = new ReportParameter("OPIP", common.myStr(Request.QueryString["OPIP"]));
            p[5] = new ReportParameter("ReportType", common.myStr(Request.QueryString["ReportType"]));
            p[6] = new ReportParameter("Areaid", common.myInt(Request.QueryString["AreaId"]).ToString());
            p[7] = new ReportParameter("CityId", common.myInt(Request.QueryString["CityId"]).ToString());
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
            string deviceInfo = null;
            if (common.myStr(ExclusiveForSMIH) != "Y")
                deviceInfo = string.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.95in", "8.3in");
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));

            if (common.myStr(Request.QueryString["ReportType"]) != "L")
            {
                deviceInfo = null;
            }

            byte[] bytes = this.ReportViewer1.ServerReport.Render(
                  "PDF", deviceInfo, out mimeType, out encoding,
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
