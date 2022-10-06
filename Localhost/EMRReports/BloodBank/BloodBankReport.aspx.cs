using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class EMRReports_BloodBank_BloodBankReport : System.Web.UI.Page
{
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];


    protected void Page_Load(object sender, EventArgs e)
    {

        string Reportname = Request.QueryString["ReportName"].ToString();
        DateTime FromDate;
        DateTime ToDate;

        string UserId = Session["UserId"].ToString();

        //string[] strDoctorId = common.myStr(setformula).Split(',');
        string ReportServerPath = "http://" + reportServer + "/ReportServer";
        rptview_blookBank.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        rptview_blookBank.ServerReport.ReportServerCredentials = irsc;
        rptview_blookBank.ProcessingMode = ProcessingMode.Remote;
        rptview_blookBank.ShowCredentialPrompts = false;
        rptview_blookBank.ShowFindControls = false;
        rptview_blookBank.ShowParameterPrompts = false;



        #region old code
        //if (Reportname == "CIR"         //Component Issue Register
        //    || Reportname == "BCDBW"      //Component Division Bag Wise  
        //    || Reportname == "BTR"    // Blood Transfer Report
        //    || Reportname == "CWDD"     //prashant  Camp Wise Donor Detail
        //    || Reportname == "CCMR"     //Prashant  Component Cross Matched Report
        //    || Reportname == "CRR"      //prashant  Component Recevied Report
        //    || Reportname == "DR"       // Prashant Donor Report
        //    || Reportname == "ES"       //  Prashant Elisa Screeing
        //    || Reportname == "KSR"      // Kit Stock Report
        //    || Reportname == "VGR"      //Vouluntary Report 
        //    || Reportname == "MWBD"     // Manfacture wise Bag Details
        //    || Reportname == "SR"       // Serum Report
        //    || Reportname == "CR"       //Cell Report
        //    || Reportname == "WBIR"       //Whole Blood Issue Report" 


        //    )
        //{


        //    FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
        //    ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
        //    ReportParameter[] p = new ReportParameter[3];
        //    p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //    p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //    p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //    if (Reportname == "CIR")  //Component Issue Register
        //    {

        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/BBComIssueregister";

        //    }
        //    if (Reportname == "BCDBW") //Component Division Bag Wise  
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/BloodComponentDivisionBagWise";

        //    }

        //    if (Reportname == "BTR")
        //    {
        //        p[0] = new ReportParameter("FROMDate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/BloodTransferReport";

        //    }
        //    if (Reportname == "CWDD")//prashant   CampWiseDonorDetail.rdl
        //    {
        //        p[0] = new ReportParameter("FromDate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserID", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/CampWiseDonorDetail";

        //    }
        //    if (Reportname == "CCMR") //prashant ComponentCrossMatchedReport.rdl
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ComponentCrossMatchedReport";

        //    }
        //    if (Reportname == "CRR") //prashant ComponentReceived.rdl
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ComponentReceived";

        //    }

        //    if (Reportname == "DR") //prashant DonarReport.rdl
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/DonarReport";

        //    }
        //    if (Reportname == "ES") //prashant ElisaScreening.rdl
        //    {
        //        p[0] = new ReportParameter("FromDate", Convert.ToString(FromDate));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ElisaScreening";

        //    }

        //    if (Reportname == "KSR")  // Kit Stock Report
        //    {

        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/KitStockReport";
        //    }

        //    if (Reportname == "VGR") // Vouuntary Gender wise Report
        //    {
        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/Voluntary_Genderwise";

        //    }

        //    if (Reportname == "MWBD") //Manufacture wise Bag Details
        //    {

        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/Manfacturewisebagdetail";
        //    }


        //    if (Reportname == "SR")  //Serum Report
        //    {
        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/Serumreport";

        //    }

        //    if (Reportname == "CR")
        //    {

        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/CellReport";
        //    }


        //    if (Reportname == "WBIR")
        //    {
        //        p[0] = new ReportParameter("Fromdate", Convert.ToString(FromDate));
        //        p[1] = new ReportParameter("ToDate", Convert.ToString(ToDate));
        //        p[2] = new ReportParameter("UserId", common.myStr(UserId));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/WholeBloodIssueregister";

        //    }


        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();
        //}

        //if (Reportname == "BTDW"
        //    || Reportname == "CS"   //prashant CS(component stock)
        //    || Reportname == "DSR"    // prashant Daily Stock Report
        //    || Reportname == "DDD"  // Prashant Daily Discard Detail DDD
        //    || Reportname == "ED"   // prashant Expiry Detail
        //    || Reportname == "FC" //Prashant   Fresh Collection
        //    || Reportname == "UBSR"    // UnSceenBlood Stock Report" 
        //    || Reportname == "TSR"

        //    )
        //{

        //    DateTime Date = Convert.ToDateTime(Request.QueryString["Cdate"].ToString());
        //    ReportParameter[] p = new ReportParameter[2];

        //    if (Reportname == "BTDW")
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/BloodTransferDatewise";
        //        p[0] = new ReportParameter("Date", Convert.ToString(Date));
        //    }

        //    else if (Reportname == "CS")// prashant  ComponentStockReport.rdl
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ComponentStockReport";
        //        p[0] = new ReportParameter("Date", Convert.ToString(Date));

        //    }
        //    else if (Reportname == "DSR")// prashant  DailyStockReport.rdl
        //    {
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/DailyStockReport";
        //        p[0] = new ReportParameter("Date", Convert.ToString(Date));

        //    }

        //    else if (Reportname == "DDD")  // prashant  DiscardDetail.rdl
        //    {

        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/DiscardDetail";
        //        p[0] = new ReportParameter("Cdate", Convert.ToString(Date));

        //    }
        //    else if (Reportname == "ED")  // prashant  Expiry Detail
        //    {
        //        p[0] = new ReportParameter("CDate", Convert.ToString(Date));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ExpiryDetail";
        //    }
        //    else if (Reportname == "FC")  // prashant  Fresh Collection
        //    {
        //        p[0] = new ReportParameter("CDate", Convert.ToString(Date));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/FreshCollection";
        //    }

        //    else if (Reportname == "UBSR") //Unscreen BloodBank Report
        //    {

        //        p[0] = new ReportParameter("Date", Convert.ToString(Date));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/UnscreenBloodStockReport";
        //    }

        //    else if (Reportname == "TSR")
        //    {

        //        p[0] = new ReportParameter("CDate", Convert.ToString(Date));
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/TransactionReport";
        //    }

        //    p[1] = new ReportParameter("UserId", common.myStr(UserId));

        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();
        //}



        //if (Reportname == "BD" //Bag Detail Report
        //    || Reportname == "TFR"  //Tranfustion Report
        //    )
        //{

        //    ReportParameter[] p = new ReportParameter[2];
        //    if (Reportname == "BD") //Bag Details
        //    {
        //        string BagNo = Request.QueryString["BagNo"].ToString();
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/BagDetails";
        //        p[0] = new ReportParameter("bagNo", Convert.ToString(BagNo));
        //        p[1] = new ReportParameter("UserId", common.myStr(UserId));
        //    }
        //    else if (Reportname == "TFR")  // Tranfustion Report
        //    {
        //        string bookingNo = Request.QueryString["BookingNo"].ToString();
        //        rptview_blookBank.ServerReport.ReportPath = "/EMRReports/TranfusionReport";
        //        p[0] = new ReportParameter("bookingNo", Convert.ToString(bookingNo));
        //        p[1] = new ReportParameter("UserId", common.myStr(UserId));

        //    }

        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();

        //}


        //if (Reportname == "CD")
        //{
        //    ReportParameter[] p = new ReportParameter[1];
        //    p[0] = new ReportParameter("UserId", common.myStr(UserId));
        //    rptview_blookBank.ServerReport.ReportPath = "/EMRReports/ComponentDivsion";
        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();
        //}
        //if (Reportname == "CBR")//prashant  Compatibility.rdl
        //{
        //    string ReqNo = Request.QueryString["ReqNo"].ToString();
        //    //BagDetails.rdl

        //    rptview_blookBank.ServerReport.ReportPath = "/EMRReports/Compatibility";
        //    ReportParameter[] p = new ReportParameter[2];
        //    p[0] = new ReportParameter("Requisition", Convert.ToString(ReqNo));
        //    p[1] = new ReportParameter("UserId", common.myStr(UserId));
        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();
        //}
        //if (Reportname == "DPE")//prashant  DonorPhysicalExamination.rdl
        //{
        //    string RegNo = Request.QueryString["RegNo"].ToString();
        //    //BagDetails.rdl

        //    rptview_blookBank.ServerReport.ReportPath = "/EMRReports/DonorPhysicalExamination";
        //    ReportParameter[] p = new ReportParameter[2];
        //    p[0] = new ReportParameter("DonorRegistrationNo", Convert.ToString(RegNo));
        //    p[1] = new ReportParameter("UserId", common.myStr(UserId));
        //    rptview_blookBank.ServerReport.SetParameters(p);
        //    rptview_blookBank.ServerReport.Refresh();
        //}


        #endregion

        //-------------------------------------------New Report 14-07-2014-------------------------------------------------

      
        if (Reportname == "ComponentRequisition")//prashant  Requisition
        {
            string RegNo = Request.QueryString["RegNo"].ToString();
            string RequisId = Request.QueryString["RequisId"].ToString();
            //BagDetails.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("UHID", RegNo);
            p[1] = new ReportParameter("UserId", common.myStr(Session["UserId"]));
            p[2] = new ReportParameter("intHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("intRequisitionId", RequisId);

            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/ComponentRequisition";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "ComponentIssue")//prashant  ComponentIssue
        {
            string RegNo = Request.QueryString["RegNo"].ToString();
            //BagDetails.rdl

            ReportParameter[] p = new ReportParameter[7];
            p[0] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("UHID", RegNo);
            p[3] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));
            p[4] = new ReportParameter("CrossMatchId", "0");
            p[5] = new ReportParameter("CrossMatchNo", "");
            if (common.myStr(Request.QueryString["ReportName"]) == "ComponentIssue")
            {
                p[6] = new ReportParameter("IssueNo", common.myStr(Request.QueryString["IssueNo"]));
            }
            else
            {
                p[6] = new ReportParameter("IssueNo", "");
            }

            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/TranfusionReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        if (Reportname == "BCITD")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //BBComponentIssueTransfuseDetails.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));//@intHospitalLocationID
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBComponentIssueTransfuseDetails";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();

        }
        //CellGroupRegister.rdl

        if (Reportname == "CGR")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //BBComponentIssueTransfuseDetails.rdl

            ReportParameter[] p = new ReportParameter[6];


            //p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            //p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            //p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            //p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));//@intHospitalLocationID
            //p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[0] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[1] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[2] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));//@intHospitalLocationID
            p[5] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));//@intHospitalLocationID



            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/CellGroupRegister";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();

        }

        if (Reportname == "DCBBA")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //DailyStockReport.rdl.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DailyStockReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //DDRR  

        if (Reportname == "DDRR")
        {
            string sFromDate = Request.QueryString["Fromdate"].ToString();
            string sToDate = Request.QueryString["Todate"].ToString();
            //DefferalDonorRegistrationDon.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[1] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[2] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[3] = new ReportParameter("FromDate", common.myStr(sFromDate));
            p[4] = new ReportParameter("ToDate", Convert.ToString(sToDate));
            //p[5] = new ReportParameter("Cdate", common.myStr(sFromDate));//@UserId
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DefferalDonorRegistrationDon";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //DISCARD REGISTER REPORT
        if (Reportname == "DRR")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //DiscardDetail.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate.ToString("yyyy/MM/dd")));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DiscardDetail";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }


        //Component Division
        if (Reportname == "CD")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //ComponentDivsion.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/ComponentDivsion";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }


        // Donor Registration with patient Details
        if (Reportname == "DRRPD")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //DonorRegistrationWithPatient.rdl
            if (common.myInt(Request.QueryString["UHID"].ToString())<0)
            {
                ReportParameter[] p = new ReportParameter[5];
                p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
                p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
                p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
                p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
                p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonorRegistrationWithPatient";
                rptview_blookBank.ServerReport.SetParameters(p);
                rptview_blookBank.ServerReport.Refresh();
            }
            else
            {
                ReportParameter[] p = new ReportParameter[5];
                p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
                p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
                ////p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
                //p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
                //p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("RegistrationId", common.myStr(Request.QueryString["UHID"]));

                p[3] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
                p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonorPatientComponentDonatedAndIssue";
                rptview_blookBank.ServerReport.SetParameters(p);
                rptview_blookBank.ServerReport.Refresh();
            }
            
        }

        //Donation Due Status report
        if (Reportname == "DDSR")
        {
            //FromDate = Convert.ToDateTime(Request.QueryString["Cdate"].ToString());
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //DonationDueStatus.rdl

            ReportParameter[] p = new ReportParameter[5];
            //p[0] = new ReportParameter("Cdate", common.myStr(FromDate));
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonationDueStatus";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //Donor Registration Details
        if (Reportname == "DRD")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Cdate"].ToString());

            //DonorRegistration.rdl

            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("CDate", common.myStr(FromDate));
            p[1] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[2] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));

            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonorRegistration";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //Nearest Expiry Unit
        if (Reportname == "NEU")
        {

            FromDate =common.myDate(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //ExpiryDetail.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/ExpiryDetail";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        if (Reportname == "TAT")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            string Component = Request.QueryString["Component"].ToString();

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("FDate", common.myStr(FromDate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("TDate", common.myStr(ToDate.ToString("yyyy/MM/dd")));// 
            p[2] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("Component", Component);
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/TATBloodBankRequest";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        //Stock Registration For Component  --SRC
        if (Reportname == "SRC")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            //ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Cdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ComponentId", common.myStr(1));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/StockRegister";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //Voluntary Donor List  --VDL
        if (Reportname == "VDL")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //Voluntary_Genderwise.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/Voluntary_Genderwise";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        //Gender Wise Donor List  --VDL
        if (Reportname == "DLGW")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //Voluntary_Genderwise.rdl

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("dtFromDate", common.myStr(FromDate.ToString("yyyy/MM/dd")));
            p[1] = new ReportParameter("dtToDate", common.myStr(ToDate.ToString("yyyy/MM/dd")));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationId", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[5] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonorRegisterGenderWise";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }

        //CrossMatch  --Compatible Details
        if (Reportname == "CMM")
        {

            string RegNo = Request.QueryString["RegNo"].ToString();
            string UnitNo = Request.QueryString["UnitNo"].ToString();
            string RegId = Request.QueryString["Regid"].ToString();
            string EncounterId = Request.QueryString["EncounterId"].ToString();
            string strFromDate = Request.QueryString["FromDate"].ToString();
            string strToDate = Request.QueryString["ToDate"].ToString();

            ReportParameter[] p = new ReportParameter[11];
            p[0] = new ReportParameter("UHID", RegNo);
            p[1] = new ReportParameter("CrossMatchNO", "");
            p[2] = new ReportParameter("UserId", common.myStr(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"]));
            p[5] = new ReportParameter("registrationID", RegId.Trim());
            p[6] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            if (common.myInt(EncounterId) != 0)
                p[7] = new ReportParameter("intEncounterId", common.myStr(EncounterId));
            else
                p[7] = new ReportParameter("intEncounterId", "");
            p[8] = new ReportParameter("chvBagNo", Request.QueryString["UnitNo"].ToString());
            p[9] = new ReportParameter("chvFromDate", common.myStr(Request.QueryString["FromDate"].ToString()));
            p[10] = new ReportParameter("chvToDate", common.myStr(Request.QueryString["ToDate"].ToString()));

            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/CrossMatchNew";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();

        }
        if (Reportname == "CrossMatchNew")
        {
            //ReportParameter[] p = new ReportParameter[11];
            //p[0] = new ReportParameter("UHID", Request.QueryString["UHID"].ToString());
            //p[1] = new ReportParameter("CrossMatchNO", Request.QueryString["CrossMatchNo"].ToString());//
            //p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            //p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            //p[4] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"]));
            //p[5] = new ReportParameter("registrationID", Request.QueryString["RegId"].ToString());
            //p[6] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            //if (common.myInt(Request.QueryString["EncounterId"]).ToString() != "0")
            //    p[7] = new ReportParameter("intEncounterId", common.myStr(Request.QueryString["EncounterId"].ToString()));
            //else
            //    p[7] = new ReportParameter("intEncounterId","");
            //p[8] = new ReportParameter("chvBagNo", Request.QueryString["UnitNo"].ToString());
            //p[9] = new ReportParameter("chvFromDate", common.myStr(Request.QueryString["FromDate"].ToString()));
            //p[10] = new ReportParameter("chvToDate", common.myStr(Request.QueryString["ToDate"].ToString()));

            ReportParameter[] p = new ReportParameter[11];
            p[0] = new ReportParameter("UHID", Request.QueryString["UHID"].ToString());
            p[1] = new ReportParameter("CrossMatchNO", Request.QueryString["CrossMatchNo"].ToString());//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[5] = new ReportParameter("registrationID", Request.QueryString["RegId"].ToString());
            p[6] = new ReportParameter("intEncounterId", Request.QueryString["EncoId"].ToString());
            //[8] = new ReportParameter("UnitNo", "0"); 
            // Added By Om Date 14-12-2016
            p[7] = new ReportParameter("chvBagNo", Request.QueryString["BagNo"]);
            p[8] = new ReportParameter("chvFromDate",(Request.QueryString["Fromdate"]));
            p[9] = new ReportParameter("chvToDate", Convert.ToString(DateTime.Now.ToString()));
            p[10] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"]));
            //end
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/CrossMatchNew";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "BBComponentIssueLabel") // Print Label 
        {
            string[] arrBagno = Request.QueryString["Bagno"].Split(',');

            for (int i = 0; i < arrBagno.Length; i++)
            {
                ReportParameter[] p = new ReportParameter[7];
                p[0] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RegistrationId"]));
                p[1] = new ReportParameter("IssueNo", common.myStr(Request.QueryString["IssueNo"]));
                p[2] = new ReportParameter("BagNo", common.myStr(arrBagno[i]));
                p[3] = new ReportParameter("UserId", common.myStr(Session["UserId"]));
                p[4] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationID"]));
                p[5] = new ReportParameter("insFacilityId", common.myStr(Session["FacilityId"]));
                p[6] = new ReportParameter("intStockiID", common.myStr(Request.QueryString["Stockid"]));

                rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBComponentIssueLabel";

                rptview_blookBank.ServerReport.SetParameters(p);
                rptview_blookBank.ServerReport.Refresh();

            }
        }
        if (Reportname == "BIRCW")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            string OPIP = Request.QueryString["OPIP"].ToString();
            string Component = Request.QueryString["Component"].ToString();
            string IssueOrReceive = Request.QueryString["IssueOrReceive"].ToString();

            ReportParameter[] p = new ReportParameter[8];
            p[0] = new ReportParameter("Fromdate", common.myStr(FromDate));
            p[1] = new ReportParameter("ToDate", common.myStr(ToDate));//
            p[2] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));//@UserId
            p[3] = new ReportParameter("intHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));//@intHospitalLocationID
            p[4] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[5] = new ReportParameter("OPIP", OPIP);
            p[6] = new ReportParameter("ComponentID", Component);
            p[7] = new ReportParameter("IssueOrReceive", IssueOrReceive);
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BloodIssueRegisterComponentWise";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();

        }
        #region == Added By Om Prakash 21-12-2016===

        if (Reportname == "BH")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("FDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptBagUnitHistory";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        #endregion
        #region == Added By Om Prakash 22-12-2016===
        if (Reportname == "RUC")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptReserveUnitsStok";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        #endregion
        #region om 23-12-20
        if (Reportname == "DSUW")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            //ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[3];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate)); 
            p[1] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[2] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptDailyStockUnitWise";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "BCIR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptBloodComponentIssueRegister";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        #endregion
        #region ===== BY Om Prakash 24-12-2016====


        if (Reportname == "USBS")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[2];
            //p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            //p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[0] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptUnscreenedBloodStock";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "DRBGW")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptDonorRegistrationBloodGroupWise";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "ROES")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptResultsOfElisaScreening";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "BSNR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/uspBagScreeningNetResults";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "CMCSR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptCrossMatchChargeSlipReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        #endregion
        #region === Om Prakash 26-12-2016====
        if (Reportname == "RequiReleaseAck")
        {

            //FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            //ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];

            p[0] = new ReportParameter("RequisitionId", common.myStr(Request.QueryString["ReqId"]));
            p[1] = new ReportParameter("HospitalLocationId", Convert.ToString(Session["HospitalLocationID"]));
            p[2] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
            p[3] = new ReportParameter("ReleaseId", common.myStr(Request.QueryString["ReleaseId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptIssueSlip";

            rptview_blookBank.ServerReport.SetParameters(p);
        }
        #endregion
        #region ==== By Om prakash 27-12-2016=====
        if (Reportname == "BBSR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBBloodbankservicesReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "BDR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHospitalLocationId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBRPTDiscardReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "CDR")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHospitalLocationId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBRPTComponentDivisionReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "DRL")
        {

            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[5];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("inyHospitalLocationID", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));
            
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/DonorRegistrationList";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "NTR")
        {
            FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            string BagNo = Convert.ToString(Request.QueryString["BagNo"].ToString());
            string NATResult = "";
            //StockRegister.rdl

            ReportParameter[] p = new ReportParameter[6];
            p[0] = new ReportParameter("fDate", common.myStr(FromDate));
            p[1] = new ReportParameter("tDate", common.myStr(ToDate));
            p[2] = new ReportParameter("intHosLocId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[4] = new ReportParameter("BagNo", BagNo);
            p[5] = new ReportParameter("NATResult", common.myStr(NATResult));
          
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/rptBloodNATReport";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        if (Reportname == "CrossMatchLabel")
        {

            //FromDate = Convert.ToDateTime(Request.QueryString["Fromdate"].ToString());
            //ToDate = Convert.ToDateTime(Request.QueryString["Todate"].ToString());
            ////StockRegister.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("UserId", Convert.ToString(Session["UserId"]));
            p[1] = new ReportParameter("CrossMatchNo", common.myStr(Request.QueryString["CrossMatchNo"]));
            p[2] = new ReportParameter("intHospitalLocationId", Convert.ToString(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            rptview_blookBank.ServerReport.ReportPath = "/" + reportFolder + "/BBrptCrossMatchLabel";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
        #endregion

        if (Reportname == "DonorPhlebotomy")//  DonorPhlebotomy
         {
            string DonorRegistrationId = Request.QueryString["DonorRegistrationId"].ToString();
            //BagDetails.rdl

            ReportParameter[] p = new ReportParameter[4];
            p[0] = new ReportParameter("DonorRegistrationNo", DonorRegistrationId);
            p[1] = new ReportParameter("UserId", common.myStr(Session["UserName"]));
            p[2] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
            p[3] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));

            rptview_blookBank.ServerReport.ReportPath = "/"+reportFolder+"/rptDonorPhlebotomy";

            rptview_blookBank.ServerReport.SetParameters(p);
            rptview_blookBank.ServerReport.Refresh();
        }
         if (common.myStr(Request.QueryString["Export"]) != "True")
        {
            string[] streamids = null;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string mimeType;
            string encoding;
            string extension;
            rptview_blookBank.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            rptview_blookBank.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
            byte[] bytes = this.rptview_blookBank.ServerReport.Render(
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
}
