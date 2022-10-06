using System;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Web.UI;

public partial class EMRBILLING_Report_PreView : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowReport();
    }

    protected void ShowReport()
    {
        if (common.myStr(Request.QueryString["rptType"]).Equals(""))
        {
            return;
        }

        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        String sRptName = "";
        ReportParameter[] p = new ReportParameter[0];

        if (common.myStr(Request.QueryString["RptName"]).Equals("IPBill"))
        {
            int IsFilterByDate = common.myInt(common.myStr(Request.QueryString["IsFilterByDate"]));
            string sFromDate = common.myDate(Request.QueryString["FromDate"]).ToString("dd/MM/yyyy HH:mm");
            string sToDate = common.myDate(Request.QueryString["ToDate"]).ToString("dd/MM/yyyy HH:mm");
            // string ShowPatientAmt = common.myStr(common.myBool(1));
            string ShowPatientAmt = common.myStr("Y");

            if (!common.myStr(Request.QueryString["ShowPatientAmt"]).Equals(""))
                ShowPatientAmt = common.myStr(Request.QueryString["ShowPatientAmt"]);

            string ShowDefaultCurrency = common.myStr(Request.QueryString["ShowDefaultCurrency"]);
            if (common.myLen(ShowDefaultCurrency).Equals(0))
            {
                ShowDefaultCurrency = "N";
            }

            if (common.myStr(Request.QueryString["RptType"]).Equals("S"))
            {

                sRptName = "rptBillSummary";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[13];
                p = new ReportParameter[25];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("fdt", sFromDate);
                p[4] = new ReportParameter("tdat", sToDate);
                p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]).ToUpper());
                p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]).ToUpper());
                p[7] = new ReportParameter("ReportHeader", "patient bill (Summary)");
                p[8] = new ReportParameter("HospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[9] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
                p[10] = new ReportParameter("RegistrationId", common.myStr(Request.QueryString["RegId"]));
                p[11] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                p[12] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[13] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                p[14] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
                // by Om prakash
                p[15] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[16] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[17] = new ReportParameter("ShowPatientAmt", ShowPatientAmt);
                p[18] = new ReportParameter("ShowDefaultCurrency", ShowDefaultCurrency);
                p[19] = new ReportParameter("SingleBill", common.myStr(Request.QueryString["SB"]) == "" ? "N" : common.myStr(Request.QueryString["SB"]));
                p[20] = new ReportParameter("DepartmentID", (common.myLen(Request.QueryString["DepartmentID"]) > 0) ? common.myStr(Request.QueryString["DepartmentID"]) : "A");
                p[21] = new ReportParameter("strFromDate", sFromDate);
                p[22] = new ReportParameter("strToDate", sToDate);
                p[23] = new ReportParameter("Invoiceid", common.myStr(Request.QueryString["BillId"]));
                p[24] = new ReportParameter("DeptType", (common.myLen(Request.QueryString["DeptType"]) > 0) ? common.myStr(Request.QueryString["DeptType"]) : "D");

                #endregion
            }

            else if (common.myStr(Request.QueryString["RptType"]) == "C")
            {
                p = new ReportParameter[3];

                //p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                // p[2] = new ReportParameter("User_id", sUserName);
                //p[13] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                // p[7] = new ReportParameter("ReportHeader", "Patient Bill (Summary)");
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("chvEncounterNo", common.myStr(Request.QueryString["EncId"]));
                // p[12] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                //#region --- || Added By Abhishek Goel 02/03/2016 ||---

                //p[14] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
                //#endregion

            }
            else if (common.myStr(Request.QueryString["RptType"]) == "SC")
            {
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                if (common.myStr(Request.QueryString["RptFor"]) == "B")
                {
                    if (common.myStr(Request.QueryString["PatType"]) == "I")
                    {
                        sRptName = "rptBillSummaryCancelledInvoices";
                        //p = new ReportParameter[13];
                        p = new ReportParameter[14];
                        #endregion
                        p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                        p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                        p[2] = new ReportParameter("User_id", sUserName);
                        p[3] = new ReportParameter("fdt", sFromDate);
                        p[4] = new ReportParameter("tdat", sToDate);
                        p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]).ToUpper());
                        p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]).ToUpper());
                        p[7] = new ReportParameter("ReportHeader", " Patient Bill (Summary)");
                        p[8] = new ReportParameter("HospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                        p[9] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
                        p[10] = new ReportParameter("RegistrationId", common.myStr(Request.QueryString["RegId"]));
                        p[11] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                        p[12] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                        #region --- || Added By Abhishek Goel 02/03/2016 ||---
                        p[13] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                    }
                    #endregion
                    #region --- || Altered By Om Prakash 21/12/2016 ||---
                    else if (common.myStr(Request.QueryString["PatType"]) == "O")
                    {
                        sRptName = "rptspOPBillProvisionalForCancelledInvoice";
                        p = new ReportParameter[13];
                        p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                        p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                        //p[2] = new ReportParameter("User_id", sUserName);
                        p[2] = new ReportParameter("strFromDate", sFromDate);
                        p[3] = new ReportParameter("strToDate", sToDate);
                        p[4] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]).ToUpper());
                        p[5] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]).ToUpper());
                        p[6] = new ReportParameter("ReportHeader", " Patient Bill (Summary)");
                        p[7] = new ReportParameter("HospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                        p[8] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
                        //p[9] = new ReportParameter("RegistrationId", common.myStr(Request.QueryString["RegId"]));
                        p[9] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                        p[10] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                        p[11] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                        p[12] = new ReportParameter("UHID", common.myStr(Session["RegistrationLabelName"]));
                    }
                    #endregion
                }

            }
            else if (common.myStr(Request.QueryString["RptType"]) == "O")
            {

                sRptName = "rptBillSummaryOneLine";
                p = new ReportParameter[14];
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("fdt", sFromDate);
                p[4] = new ReportParameter("tdat", sToDate);
                p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]).ToUpper());
                p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]).ToUpper());
                p[7] = new ReportParameter("ReportHeader", "Patient Bill (One-Line)");
                p[8] = new ReportParameter("HospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[9] = new ReportParameter("FacilityId", common.myStr(Session["FacilityId"]));
                p[10] = new ReportParameter("RegistrationId", common.myStr(Request.QueryString["RegId"]));
                p[11] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                // by Om prakash
                p[12] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[13] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));

            }
            else if (common.myStr(Request.QueryString["RptType"]).Equals("D"))
            {
                if (common.myStr(Request.QueryString["ShowPharmacy"]).ToUpper().Equals("Y"))
                {
                    sRptName = "rptPharmacyBillDetail";
                    #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                    //p = new ReportParameter[10];
                    p = new ReportParameter[15];
                    #endregion
                    p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                    p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                    p[2] = new ReportParameter("User_id", sUserName);
                    p[3] = new ReportParameter("strToDate", sFromDate);
                    p[4] = new ReportParameter("strFromDate", sToDate);
                    p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]));
                    p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]));
                    p[7] = new ReportParameter("ReportHeader", "Patient Bill (Details)");
                    p[8] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                    p[9] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                    #region --- || Added By Abhishek Goel 02/03/2016 ||---
                    p[10] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                    p[11] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
                    p[12] = new ReportParameter("IsShowEncoded", common.myStr(Request.QueryString["ShowEncoded"]));
                    p[13] = new ReportParameter("ShowDefaultCurrency", ShowDefaultCurrency);
                    p[14] = new ReportParameter("SingleBill", " ");

                    #endregion
                }
                else
                {

                    sRptName = "rptBillDetail";
                    #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                    //p = new ReportParameter[10];
                    p = new ReportParameter[22];
                    #endregion
                    p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                    p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                    p[2] = new ReportParameter("User_id", sUserName);
                    p[3] = new ReportParameter("strToDate", sToDate);
                    p[4] = new ReportParameter("strFromDate", sFromDate);
                    p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]));
                    p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]));
                    p[7] = new ReportParameter("ReportHeader", "patient bill (Details)");
                    p[8] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                    p[9] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                    #region --- || Added By Abhishek Goel 02/03/2016 ||---
                    p[10] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                    p[11] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
                    p[12] = new ReportParameter("IsShowEncoded", common.myStr(Request.QueryString["ShowEncoded"]));
                    // By Om 12/12/2016
                    p[13] = new ReportParameter("iHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                    p[14] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                    p[15] = new ReportParameter("ShowPatientAmt", common.myStr(ShowPatientAmt));
                    p[16] = new ReportParameter("ShowDefaultCurrency", ShowDefaultCurrency);
                    p[17] = new ReportParameter("SingleBill", common.myStr(Request.QueryString["SB"]) == "" ? "N" : common.myStr(Request.QueryString["SB"]));
                    p[18] = new ReportParameter("DepartmentID", common.myInt(Request.QueryString["DepartmentID"]).ToString());
                    p[19] = new ReportParameter("Invoiceid", common.myStr(Request.QueryString["BillId"]));
                    p[20] = new ReportParameter("DeptType", (common.myLen(Request.QueryString["DeptType"]) > 0) ? common.myStr(Request.QueryString["DeptType"]) : "D");
                    p[21] = new ReportParameter("Hmed", "0");
                    #endregion
                }

            }
            else if (common.myStr(Request.QueryString["RptType"]) == "A")
            {
                sRptName = "rptAttendantIPPateintDetails";
                p = new ReportParameter[5];
                p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[2] = new ReportParameter("UserId", sUserName);
                p[3] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RegId"]));
                p[4] = new ReportParameter("chvEncounterId", common.myStr(Request.QueryString["EncId"]));
            }
            else if (common.myStr(Request.QueryString["RptType"]) == "DC")
            {
                sRptName = "rptBillDetailCancelledInvoices";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[10];
                p = new ReportParameter[12];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("strToDate", sFromDate);
                p[4] = new ReportParameter("strFromDate", sToDate);
                p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]));
                p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]));
                p[7] = new ReportParameter("ReportHeader", "Patient Bill (Details)");
                p[8] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                p[9] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[10] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                #endregion
                p[11] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));

            }
            else if (common.myStr(Request.QueryString["RptType"]) == "N")
            {
                sRptName = "rptNonPayableBillDetail";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[10];
                p = new ReportParameter[12];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("strToDate", sFromDate);
                p[4] = new ReportParameter("strFromDate", sToDate);
                p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]));
                p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]));
                p[7] = new ReportParameter("ReportHeader", "Non Payable Bill (Details)");
                p[8] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                p[9] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[10] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                #endregion
                p[11] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
            }
            else if (common.myStr(Request.QueryString["RptType"]) == "A")
            {
                sRptName = "rptAttendantIPPateintDetails";
                p = new ReportParameter[5];
                p[0] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[1] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[2] = new ReportParameter("UserId", sUserName);
                p[3] = new ReportParameter("intRegistrationId", common.myStr(Request.QueryString["RegId"]));
                p[4] = new ReportParameter("chvEncounterId", common.myStr(Request.QueryString["EncId"]));
            }
            else if (common.myStr(Request.QueryString["RptType"]) == "DC")
            {
                sRptName = "rptBillDetailCancelledInvoices";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[10];
                p = new ReportParameter[12];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("InvNo", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("strToDate", sFromDate);
                p[4] = new ReportParameter("strFromDate", sToDate);
                p[5] = new ReportParameter("ShowDiscount", common.myStr(Request.QueryString["Disc"]));
                p[6] = new ReportParameter("ShowAdvance", common.myStr(Request.QueryString["Adv"]));
                p[7] = new ReportParameter("ReportHeader", "Patient Bill (Details)");
                p[8] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                p[9] = new ReportParameter("ReportType", common.myStr(common.myStr(Request.QueryString["ReportType"])));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[10] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                #endregion
                p[11] = new ReportParameter("ShowOrginialFlag", common.myStr(common.myStr(Request.QueryString["ShowOrginal"])));
            }
            else if (common.myStr(Request.QueryString["RptType"]) == "P")
            {
                sRptName = "rptPackageBill";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[7];
                p = new ReportParameter[8];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("Invoiceid", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("strFromDate", sFromDate);
                p[4] = new ReportParameter("strToDate", sToDate);
                p[5] = new ReportParameter("ReportHeader", "Patient Bill (Package)");
                p[6] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[7] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                #endregion

            }

            #region Add by Rahul for Package inclusion report For Moolchand
            else if (common.myStr(Request.QueryString["RptType"]) == "PI")
            {
                sRptName = "rptPackageInclusionDetails";
                #region --- || Altered By Abhishek Goel 02/03/2016 ||---
                //p = new ReportParameter[7];
                p = new ReportParameter[8];
                #endregion
                p[0] = new ReportParameter("IPNo", common.myStr(Request.QueryString["EncId"]));
                p[1] = new ReportParameter("Invoiceid", common.myStr(Request.QueryString["BillId"]));
                p[2] = new ReportParameter("User_id", sUserName);
                p[3] = new ReportParameter("strFromDate", sFromDate);
                p[4] = new ReportParameter("strToDate", sToDate);
                p[5] = new ReportParameter("ReportHeader", "Patient Bill (Package)");
                p[6] = new ReportParameter("FilterByDate", common.myStr(IsFilterByDate));
                #region --- || Added By Abhishek Goel 02/03/2016 ||---
                p[7] = new ReportParameter("userid", common.myStr(common.myInt(Session["UserID"])));
                #endregion

            }
            #endregion

            PrintReport(sRptName, p);
        }

        else if (common.myStr(Request.QueryString["RptName"]) == "OT")
        {
            if (common.myStr(Request.QueryString["RptType"]) == "Detail")
            {
                sRptName = "rptOTBookingDetails";
                p = new ReportParameter[10];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("intTheaterId", "0");
                p[3] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[5] = new ReportParameter("intStoreId", "0");
                p[6] = new ReportParameter("chvUserName", sUserName);
                p[7] = new ReportParameter("Reportheader", "OT Booking Details");
                p[8] = new ReportParameter("Status", common.myStr(Request.QueryString["Status"]));
                p[9] = new ReportParameter("IsActual", common.myStr(Request.QueryString["IsActual"]));
            }
            if (common.myStr(Request.QueryString["RptType"]) == "Theatre")
            {
                sRptName = "RptOtPerSurListOtWise";
                p = new ReportParameter[7];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("intTheaterId", common.myStr(Request.QueryString["Otlst"]));
                p[3] = new ReportParameter("FDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("TDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[5] = new ReportParameter("chvUserName", sUserName);
                p[6] = new ReportParameter("Reportheader", "OT Performed Surgery List");
            }
            if (common.myStr(Request.QueryString["RptType"]) == "Period")
            {
                sRptName = "RptOTSurPeriodWise";
                p = new ReportParameter[6];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("FDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[3] = new ReportParameter("TDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("chvUserName", sUserName);
                p[5] = new ReportParameter("Reportheader", "OT Performed Surgery List");
            }
            if (common.myStr(Request.QueryString["RptType"]) == "SurCat")
            {
                sRptName = "RptOtSurListCatWise";
                //string[] strIdList = common.myStr(Request.QueryString["Otcat"]).ToString().Split(',');
                p = new ReportParameter[7];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                //p[2] = new ReportParameter("OtCatId", common.myStr(Request.QueryString["RptType"].ToString()));
                p[2] = new ReportParameter("OtCatId", common.myStr(Request.QueryString["Otcat"]));
                p[3] = new ReportParameter("FDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("TDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[5] = new ReportParameter("chvUserName", common.myStr(sUserName));
                p[6] = new ReportParameter("Reportheader", "OT Performed Surgery List");
            }

            if (common.myStr(Request.QueryString["RptType"]) == "UnplannedSurgery")
            {

                sRptName = "rptUnplannedOTReturn";
                p = new ReportParameter[6];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[3] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("bitUnPlannedSurgeryReport", common.myStr(Request.QueryString["ReportType"]));
                p[5] = new ReportParameter("RegNo", common.myStr(Resources.PRegistration.regno));

            }

            if (common.myStr(Request.QueryString["RptType"]) == "Abort")
            {

                sRptName = "rptOtAbortReport";
                p = new ReportParameter[5];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[3] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("RegNo", common.myStr(Resources.PRegistration.regno));

            }
            if (common.myStr(Request.QueryString["RptType"]) == "BallonTimeReport")
            {
                sRptName = "rptBallonTimeReport";
                p = new ReportParameter[5];
                p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[3] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("RegNo", common.myStr(Resources.PRegistration.regno));
            }

            if (common.myStr(Request.QueryString["RptType"]) == "Register")
            {
                sRptName = "rptOTRegistre";
                p = new ReportParameter[4];
                p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[3] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
            }

            if (common.myStr(Request.QueryString["RptType"]) == "CancelOTBooking")
            {
                sRptName = "CancelOTBookingDetails";
                p = new ReportParameter[5];
                p[0] = new ReportParameter("inyHospitalLocationID", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("intTheaterId", common.myStr(Request.QueryString["Theater"]));
                p[3] = new ReportParameter("dtFromDate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy/MM/dd"));
                p[4] = new ReportParameter("dtToDate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy/MM/dd"));
            }

            if (common.myStr(Request.QueryString["RptType"]) == "DoctorDeptSurgeryCases")
            {
                if (common.myStr(Request.QueryString["ReportType"]).Equals("DO"))
                {
                    sRptName = "DoctorWiseSurgeryCases";
                }
                else if (common.myStr(Request.QueryString["ReportType"]).Equals("DE"))
                {
                    sRptName = "DepartmentWiseSurgeryCases";
                }
                else if (common.myStr(Request.QueryString["ReportType"]).Equals("DS"))
                {
                    sRptName = "DoctorWiseSurgeryCasesSummary";
                }

                p = new ReportParameter[6];
                p[0] = new ReportParameter("Fdate", Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("yyyy-MM-dd"));
                p[1] = new ReportParameter("Tdate", Convert.ToDateTime(Request.QueryString["Todate"]).ToString("yyyy-MM-dd"));
                p[2] = new ReportParameter("IntFacilityid", common.myStr(Session["FacilityId"]));

                if (common.myStr(Request.QueryString["DeptIds"]) != "")
                {
                    p[3] = new ReportParameter("Department", common.myStr(Request.QueryString["DeptIds"]));
                }
                else
                {
                    p[3] = new ReportParameter("Department", common.myStr("A"));
                }

                if (common.myStr(Request.QueryString["DoctorIds"]) != "")
                {
                    p[4] = new ReportParameter("Doctor", common.myStr(Request.QueryString["DoctorIds"]));
                }
                else
                {
                    p[4] = new ReportParameter("Doctor", common.myStr("A"));
                }
                p[5] = new ReportParameter("Type", common.myStr("0"));

            }

            PrintReport(sRptName, p);
        }
        else if (common.myStr(Request.QueryString["RptName"]) == "MISDashboard")
        {
            if (common.myStr(Request.QueryString["RptType"]) == "FlashReport")
            {
                sRptName = "MisDashBoardFlashReport";
                p = new ReportParameter[4];
                p[0] = new ReportParameter("IntHospitalLocationid", common.myStr(Session["HospitalLocationId"]));
                p[1] = new ReportParameter("IntFacilityId", common.myStr(Session["FacilityId"]));
                p[2] = new ReportParameter("CDate", common.myStr(Request.QueryString["Date"]));
                p[3] = new ReportParameter("ChvType", "F");
            }
            PrintReport(sRptName, p);
        }
        else if (common.myStr(Request.QueryString["RptName"]) == "OPIPSummaryStat")
        {
            sRptName = "PerformanceReport";
            p = new ReportParameter[7];
            p[0] = new ReportParameter("intHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("FDate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("TDate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("GrpType", common.myStr(Request.QueryString["GroupId"]));
            p[5] = new ReportParameter("User_id", common.myStr(Session["UserId"]));
            p[6] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));

            PrintReport(sRptName, p);
        }
        else if (common.myStr(Request.QueryString["RptName"]) == "OPDepartmentStatisticsDoctorWise")
        {
            sRptName = "MisDoctorStatisticCityWise";
            p = new ReportParameter[6];
            p[0] = new ReportParameter("intHospLocationId", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("chvDeptId", common.myStr(Request.QueryString["DeptId"]));
            p[3] = new ReportParameter("intMonth", common.myStr(Request.QueryString["Month"]));
            p[4] = new ReportParameter("intYear", common.myStr(Request.QueryString["Year"]));
            p[5] = new ReportParameter("EntrySite", common.myStr("0"));

            PrintReport(sRptName, p);
        }
        else if (common.myStr(Request.QueryString["RptName"]) == "OPBill")
        {
            sRptName = "MisOPBillGenerated";
            p = new ReportParameter[5];
            p[0] = new ReportParameter("Locid", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("Facilityid", common.myStr(Session["FacilityId"]));
            p[2] = new ReportParameter("Fdate", common.myStr(Request.QueryString["FromDate"]));
            p[3] = new ReportParameter("Tdate", common.myStr(Request.QueryString["ToDate"]));
            p[4] = new ReportParameter("EntrySite", common.myStr(Request.QueryString["EntrySite"]));


            PrintReport(sRptName, p);
        }
        else if (common.myStr(Request.QueryString["rptType"]) == "ORDERPRINT")
        {
            sRptName = "PrintOrderDetails";
            p = new ReportParameter[4];

            p[0] = new ReportParameter("HospLocId", Session["HospitalLocationId"].ToString());
            p[1] = new ReportParameter("encounterId", common.myStr(Request.QueryString["EncId"]));
            p[2] = new ReportParameter("Registrationid", common.myStr(Request.QueryString["RegId"]));
            p[3] = new ReportParameter("orderID", common.myStr(Request.QueryString["OrderId"]));


            PrintReport(sRptName, p);
        }

        else if (Request.QueryString["rptType"].ToString() == "DischargeNotification")
        {
            // ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DischargeNotification";
            sRptName = "DischargeNotification_Final";
            p = new ReportParameter[3];
            p[0] = new ReportParameter("EncounterNo", common.myStr(Request.QueryString["IpNo"]).Trim());
            p[1] = new ReportParameter("UserID", Session["UserID"].ToString());
            p[2] = new ReportParameter("intFacilityId", Session["FacilityID"].ToString());
            PrintReport(sRptName, p);
        }



        if (common.myStr(Request.QueryString["RptType"]) == "PrintDepartmentAck")
        {
            sRptName = "rptDepartmentAcknowledgeSlipIP";
            p = new ReportParameter[8];
            p[0] = new ReportParameter("Loc_id", common.myStr(Session["HospitalLocationId"]));
            p[1] = new ReportParameter("encounterId", common.myStr(Request.QueryString["EncId"]));
            p[2] = new ReportParameter("Invoiceid", "0");
            p[3] = new ReportParameter("orderID", common.myStr(Request.QueryString["OrderId"]));
            p[4] = new ReportParameter("user_id", sUserName);
            p[5] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
            p[6] = new ReportParameter("BillNo", "Bill No");
            p[7] = new ReportParameter("UHid", "0");
            PrintReport(sRptName, p);
        }
    }

    private void PrintReport(string sReportName, ReportParameter[] para)
    {
        try
        {
            string ReportServerPath = "http://" + reportServer + "/ReportServer";
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + sReportName;
            ReportViewer1.ServerReport.SetParameters(para);
            ReportViewer1.ServerReport.Refresh();

            if (Request.QueryString["preview"] != null)
            {
                if (common.myStr(Request.QueryString["preview"]) == "PP")
                {
                    return;
                }
            }
            if (common.myBool(Request.QueryString["Export"]) != true)
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

                if (common.myStr(Request.QueryString["EMailID"]).Trim().Length > 3 && common.myInt(Request.QueryString["IsEMail"]) == 1)
                {
                    BaseC.EMRMasters.EMRFacility objf = new BaseC.EMRMasters.EMRFacility(sConString);
                    BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                    DataSet ds = objf.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataView dv = ds.Tables[0].DefaultView;
                            dv.RowFilter = "FacilityId = " + common.myStr(Session["FacilityID"]);
                            if (dv.Count > 0)
                            {
                                DataTable dt = dv.ToTable();
                                string SMTPMailServer = common.myStr(dt.Rows[0]["SMTPMailServer"]);
                                string SMTPMailServerPort = common.myStr(dt.Rows[0]["SMTPMailServerPort"]);
                                string DefaultFromMailId = common.myStr(dt.Rows[0]["DefaultFromMailId"]);
                                string DefaultFromMailPws = common.myStr(dt.Rows[0]["DefaultFromMailPws"]);

                                MemoryStream ms = new MemoryStream(bytes); ;
                                MailMessage mail = new MailMessage();
                                mail.Attachments.Add(new Attachment(ms, common.myStr(Request.QueryString["IPNo"]).Trim() + ".pdf"));

                                mail.From = new MailAddress(DefaultFromMailId, DefaultFromMailId);
                                mail.To.Add(common.myStr(Request.QueryString["EMail"]).Trim());
                                mail.Subject = "IP Bill Report " + common.myStr(Request.QueryString["IPNo"]).Trim();
                                //mail.Body = "Dear " + common.myStr(Request.QueryString["PName"]).Trim() + "," + System.Environment.NewLine + System.Environment.NewLine + "Please! Find your attached Lab Report.";
                                mail.Body = "Please! Find your attached IP Bill Report";
                                mail.IsBodyHtml = false;
                                mail.BodyEncoding = System.Text.Encoding.UTF8;

                                SmtpClient client = new SmtpClient(SMTPMailServer, common.myInt(SMTPMailServerPort));
                                client.Credentials = new NetworkCredential(DefaultFromMailId, DefaultFromMailPws);
                                client.Host = SMTPMailServer;
                                client.Port = common.myInt(SMTPMailServerPort);
                                client.Send(mail);
                                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                            }
                        }
                    }
                }
                else
                {
                    MemoryStream oStream = new MemoryStream();
                    oStream.Write(bytes, 0, bytes.Length);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(oStream.ToArray());
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            msg.Text = "1 " + ex.Message;
        }
    }

}
