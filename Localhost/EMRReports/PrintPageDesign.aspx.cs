using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.IO;
using Telerik.Web.UI.Calendar;
public partial class EMRReports_OutstandingSummary : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    DataSet ds;
    private bool Stauts = false;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            trloction.Visible = false;
            FillEntrySite();
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();

            ddlInpatientType.Visible = false;
            if (common.myStr(Request.QueryString["RN"]) == "HPDCW")// Health package reports
            {
                lblHeader.Text = "Health package Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = true;
                dttdfrmdate.Visible = true;
                rblReportWise.Visible = false;
                lable1.Text = "Company";
                ddlPatienttype.Visible = true;
                Label4.Text = "Company";
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                ChkCompany.Checked = true;
                ChkCompany.Visible = false;
                ViewState["chkAllDep"] = "";
                btnExportToExcel.Visible = true;
                btnExportToExcel.Visible = false;
                chkExport.Visible = true;
                // trfacility.Visible = true;
                trhealth.Visible = true;
                Label1.Visible = false;
                ddlOPIP.Visible = false;
                ddlPatienttype_SelectedIndexChanged(sender, e);

            }
            if (common.myStr(Request.QueryString["RN"]) == "OA")
            {
                lblHeader.Text = "Outsanding Summary As On";
                Label3.Text = "As On Date";
                dtpTodate.Enabled = false;
                lbltdfrmdate.Visible = false;
                dttdfrmdate.Visible = false;
                dtpfromdate.DateInput.Text = "";
                rblReportWise.Visible = false;
                chkSummary.Visible = false;

                Label4.Text = "Company";

                chkdetail.Visible = false;
                trdatetime.Visible = false;

                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;

            }

            if (common.myStr(Request.QueryString["RN"]) == "OS")
            {
                lblHeader.Text = "Outsanding";
                rdlsummary.Items[0].Selected = true;
                rdldatewise.Items[0].Selected = true;
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = true;
                dttdfrmdate.Visible = true;
                rblReportWise.Visible = false;

                Bindcompany();
                Label4.Text = "Company";

                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;

                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "OAD")
            {
                Label3.Text = "As On Date";
                dtpTodate.Enabled = false;
                lblHeader.Text = "Outsanding Details As On";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = false;
                dttdfrmdate.Visible = false;
                dtpfromdate.DateInput.Text = "";
                rblReportWise.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;

                Bindcompany();
                Label4.Text = "Company";

                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;

            }

            if (common.myStr(Request.QueryString["RN"]) == "OSD")
            {
                lblHeader.Text = "Outsanding Details Date Wise";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = true;
                dttdfrmdate.Visible = true;
                rblReportWise.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                Bindcompany();
                Label4.Text = "Company";

                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]) == "BillRegister")
            {
                lblHeader.Text = "Bill Register";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = true;
                dttdfrmdate.Visible = true;
                rblReportWise.Visible = false;
                lable1.Text = "Company";
                ddlPatienttype.Visible = true;
                Label4.Text = "Company";
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                ddlOPIP.Items.Insert(3, new RadComboBoxItem("Pharmacy", "P"));
                btnExportToExcel.Visible = true;
                ddlPatienttype_SelectedIndexChanged(sender, e);
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "Disc")
            {
                lblHeader.Text = "Discount";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Text = "Group By";
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                ddlAuthorization.Visible = true;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                trOutStan.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = true;
                lblpaymenttype.Visible = true;
                DropDownPaymentType.Visible = true;
                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
            }

            if (common.myStr(Request.QueryString["RN"]) == "Rev")
            {
                lblHeader.Text = "Revenue";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                Label1.Visible = false;
                ddlOPIP.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                ChkCompany.Visible = true;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;

                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
                rblReportWise_OnSelectedIndexChanged(sender, e);
            }

            if (common.myStr(Request.QueryString["RN"]) == "IPTAT")
            {
                lblHeader.Text = "IP TAT";
                //dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = true;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                Bindcompany();
                Label4.Text = "Company";
                trloction.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "OPTAT")
            {
                lblHeader.Text = "OP TAT";
                //dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }
            if (common.myStr(Request.QueryString["RN"]) == "DSR")
            {
                lblHeader.Text = "Daily Summary Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }



            if (common.myStr(Request.QueryString["RN"]) == "BillCanc")
            {
                lblHeader.Text = "Bill Cancelled";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }

            if (common.myStr(Request.QueryString["RN"]) == "Refund")
            {
                lblHeader.Text = "Refund";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "OPVisit")
            {

                lblHeader.Text = "OP Visit";
                //dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = true;
                ddlUnderPackage.Visible = true;
                chkSummary.Visible = false;
                trloction.Visible = true;
                ddlEntrySite.Visible = true;
                lbllocation.Visible = true;
                trgrid.Visible = true;
                Label4.Text = "Doctors";
            }
            if (common.myStr(Request.QueryString["RN"]) == "OPDZero")
            {
                lblHeader.Text = "OPD Zero Billing";
                trgrid.Visible = true;
                BindUser();
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;

                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;

            }
            if (common.myStr(Request.QueryString["RN"]) == "TCSTAXReport")
            {
                lblHeader.Text = "Service Tax and TCS Tax Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;

            }

            if (common.myStr(Request.QueryString["RN"]) == "Sponsorwise")
            {
                lblHeader.Text = "Occupancy Sponsor Wise";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
            }


            if (common.myStr(Request.QueryString["RN"]) == "ProcedureIPDdetails")
            {
                lblHeader.Text = "Doctor wise/Procedure wise IPD details";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]) == "CREDITCOLL")
            {
                lblHeader.Text = "Credit Collection Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lbltdfrmdate.Visible = true;
                dttdfrmdate.Visible = true;
                rblReportWise.Visible = false;
                lable1.Text = "Company";
                ddlPatienttype.Visible = true;
                Label4.Text = "Company";
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                ChkCompany.Visible = false;
                //chkBillWise.Visible = false;
                ViewState["chkAllDep"] = "";
                ddlOPIP.Items.Insert(3, new RadComboBoxItem("Pharmacy", "P"));
                btnExportToExcel.Visible = true;
                //chkAging.Visible = false;
                btnExportToExcel.Visible = false;
                chkExport.Visible = false;
                ddlPatienttype_SelectedIndexChanged(sender, e);

            }

            if (common.myStr(Request.QueryString["RN"]) == "ConcessionSummary")
            {
                lblHeader.Text = "Concession Summary Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
                chkdetail.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]) == "ProcedureWiseRevenue")
            {
                lblHeader.Text = "Surgery/Procedure Wise Revenue Detail";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = true;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = true;
                Label1.Visible = true;
                chkdetail.Visible = false;
                rblReportWise.Items[1].Selected = true;
                //rblReportWise.Items[0].Attributes.CssStyle.Add("visibility", "hidden");
                //rblReportWise.Items[2].Attributes.CssStyle.Add("visibility", "hidden");
                //rblReportWise.Items[3].Attributes.CssStyle.Add("visibility", "hidden");
                //rblReportWise.Items[4].Attributes.CssStyle.Add("visibility", "hidden");
                rblReportWise.Items[0].Selected = false;



            }

            if (common.myStr(Request.QueryString["RN"]) == "ContributionCompanyType")
            {
                lblHeader.Text = "Contribution By Performing Speciality and Company Type Wise";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                dtpfromdate.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = true;
                Label1.Visible = true;
                chkdetail.Visible = false;
                Label2.Visible = false;
                btnPrintreport.Visible = false;
                btnToExcel.Visible = true;
                chkExport.Visible = false;
                chkSpeciality.Visible = true;
                chkcompanytype.Visible = true;
            }


            if (common.myStr(Request.QueryString["RN"]) == "SponsorwiseOutstanding")
            {
                lblHeader.Text = "Sponsor wise Outstanding and Collection Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                dtpfromdate.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
                chkdetail.Visible = false;
                chkExport.Visible = true;
                Label2.Visible = false;
                Label3.Text = "Date";
            }



            if (common.myStr(Request.QueryString["RN"]) == "CmpMarkup")
            {
                lblHeader.Text = "Company Markup Details Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;

            }

            if (common.myStr(Request.QueryString["RN"]) == "DoctorAccountingWise")
            {

                lblHeader.Text = "Doctor Accounting Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                rdlsummary.Visible = true;
                rdldatewise.Visible = false;
                ddlPatienttype.Visible = false;

                rblReportWise.Visible = true;
                trtype.Visible = true;
                trOutStan.Visible = true;
                chkdetail.Visible = true;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
                chkdetail.Visible = false;
                rblReportWise.Visible = false;
                gvReporttype.Visible = false;
                trgrid.Visible = false;

            }





            //if (common.myStr(Request.QueryString["RN"]) == "CmpMarkupService")
            //{
            //    lblHeader.Text = "Company Markup Details Report";
            //    dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
            //    lable1.Visible = false;
            //    ddlPatienttype.Visible = false;
            //    trgrid.Visible = false;
            //    rblReportWise.Visible = false;
            //    trtype.Visible = true;
            //    trOutStan.Visible = false;
            //    chkdetail.Visible = false;
            //    trdatetime.Visible = false;
            //    lblUnderPackage.Visible = false;
            //    ddlUnderPackage.Visible = false;
            //    chkSummary.Visible = false;
            //    trloction.Visible = false;
            //    ddlOPIP.Visible = false;
            //    Label1.Visible = false;
            //}

            if (common.myStr(Request.QueryString["RN"]) == "OPDBilling")
            {
                lblHeader.Text = "OPDRegistration Billing Report";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = true;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;

            }


            if (common.myStr(Request.QueryString["RN"]) == "DepositExhaustCash")
            {
                lblHeader.Text = "Deposit Exhaust Report Cash Patient";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                dtpfromdate.Enabled = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;
                ddlOPIP.Visible = false;
                Label1.Visible = false;
                Label3.Visible = false;
                dtpTodate.Visible = false;

            }






            if (common.myStr(Request.QueryString["RN"]) == "AmublanceBillOPD")
            {
                lblHeader.Text = "Amublance Billing OPD";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;


            }

            if (common.myStr(Request.QueryString["RN"]) == "OPTOIP")
            {
                lblHeader.Text = "OP TO IP Conversion";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = false;


            }


            if (common.myStr(Request.QueryString["RN"]) == "CashCollec")
            {
                lblHeader.Text = "Cash Collection";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                Label1.Visible = false;
                ddlOPIP.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = true;
                rblReportWise.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = true;
                trdate.Visible = false;
                chkSummary.Visible = true;
                chkModewise.Visible = true;
                Label4.Text = "User";
                chkAccountSummary.Visible = true;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;


                dtfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dtfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dtfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 00:00");

                dttodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dttodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dttodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 23:59");
                if (common.myStr(Request.QueryString["PCall"]) == "Y")
                {
                    bindonlypharmacy();
                }
                else
                {
                    BindUser();

                }


                // RadComboBox1_SelectedIndexChanged(this, null);
            }

            if (common.myStr(Request.QueryString["RN"]) == "CashCollecEntry")
            {
                Radentry.Visible = true;
                Chkentrysite.Visible = true;
                Label7.Visible = true;
                lblHeader.Text = "Cash Collection Entry Wise";
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                Label1.Visible = false;
                ddlOPIP.Visible = false;
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = true;
                rblReportWise.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = true; ;
                trdate.Visible = false;
                chkSummary.Visible = true;

                Label4.Text = "User";
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;

                dtfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dtfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dtfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 00:00");

                dttodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dttodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
                dttodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 23:59");
                bindentrysite();
                // BindUser();
                binduser_entrysitewise();
                // RadComboBox1_SelectedIndexChanged(this, null);
            }
            if (common.myStr(Request.QueryString["RN"]) == "CrNote")
            {
                lblHeader.Text = "Credit Note";
                //dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false; ;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
                lblType.Visible = true;
                ddlType.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "DWS")
            {
                lblHeader.Text = " Departmentwise Surgery Details";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "BOccup")
            {
                lblHeader.Text = " Bed Occupancy Details";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;


            }
            if (common.myStr(Request.QueryString["RN"]) == "ICUUti")
            {
                lblHeader.Text = " ICU Utilization Details";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "InPStat")
            {
                lblHeader.Text = "In-Patient statistics";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = true;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = true;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;

            }
            if (common.myStr(Request.QueryString["RN"]) == "BRITHREG")
            {
                lblHeader.Text = "Birth Register";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "DEFICIENCYRPT")
            {
                lblHeader.Text = "Deficiency Report";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
                trDeficiencyCategory.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "DIRECTBILLWOENC")
            {
                lblHeader.Text = "";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;

            }
            if (common.myStr(Request.QueryString["RN"]) == "DEATHREG")
            {
                lblHeader.Text = "Death Register";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
            }
            if (common.myStr(Request.QueryString["RN"]) == "MRDSURG")
            {
                lblHeader.Text = "Surgery Report";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lblOTStatus.Visible = true;
                ddlOTStatus.Visible = true;

                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                trloction.Visible = true;
                bindBookingStatus();

            }
            if (common.myStr(Request.QueryString["RN"]) == "MRDWELLNESS")
            {
                lblHeader.Text = "Wellness Report";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                trtype.Visible = true;
                lable1.Visible = false;
                Label1.Visible = false;
                Label1.Text = "Report type";
                ddlPatienttype.Visible = false;
                ddlOPIP.Visible = false;
                ddlAuthorization.Visible = false;
                ddlInpatientType.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;

                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;

            }
            if (common.myStr(Request.QueryString["RN"]).Equals("VATREPORT"))
            {
                lblHeader.Text = " VAT Report";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]).Equals("SLOB"))
            {
                lblHeader.Text = " Surgery List Order Base";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }
            if (common.myStr(Request.QueryString["RN"]).Equals("SRDD"))
            {
                lblHeader.Text = " Surgery Report Discharge Datebase";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]).Equals("MDS"))
            {
                lblHeader.Text = " Mis Daily Summary";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
                Label2.Text = "Date";
                Label3.Visible = false;
                dtpTodate.Visible = false;

            }
            if (common.myStr(Request.QueryString["RN"]).Equals("TDS"))
            {
                lblHeader.Text = " TDS Report";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }

            if (common.myStr(Request.QueryString["RN"]).Equals("UCH"))
            {
                lblHeader.Text = " Users Collection Handover";

                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                lable1.Visible = false;
                ddlPatienttype.Visible = false;
                trgrid.Visible = false;
                rblReportWise.Visible = false;
                trtype.Visible = false;
                trOutStan.Visible = false;
                chkdetail.Visible = false;
                trdatetime.Visible = false;
                lblUnderPackage.Visible = false;
                ddlUnderPackage.Visible = false;
                chkSummary.Visible = false;
            }
            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();

            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
            FillDeficiencyCategory();
        }
    }

    private void FillEntrySite()
    {
        try
        {
            int FacilityId;
            FacilityId = common.myInt(Session["FacilityId"]);
            BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
            DataSet ds = obj.getEntrySite(Convert.ToInt16(Session["UserID"]), FacilityId);
            int EntrySiteIdx = ddlEntrySite.SelectedIndex;
            ddlEntrySite.DataSource = ds.Tables[0];
            ddlEntrySite.DataValueField = "ESId";
            ddlEntrySite.DataTextField = "ESName";
            ddlEntrySite.DataBind();
            ddlEntrySite.SelectedIndex = EntrySiteIdx;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }

    }

    private void FillDeficiencyCategory()
    {
        try
        {
            if (trDeficiencyCategory.Visible)
            {
                BaseC.clsLISPhlebotomy obj = new BaseC.clsLISPhlebotomy(sConString);
                ddlDeficiencyCategory.DataSource = (obj.GetDeficiencyCategory()).Tables[0];
                ddlDeficiencyCategory.DataValueField = "Id";
                ddlDeficiencyCategory.DataTextField = "Name";
                ddlDeficiencyCategory.DataBind();
                ddlDeficiencyCategory.Items.Insert(0, new ListItem("--- Select Deficiency Category ---", ""));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }

    private void binduser_entrysitewise()
    {


        string strentryid = "";
        string[] entryid = common.GetCheckedItems(Radentry).Split(',');
        foreach (string Id in entryid)
        {
            if (Id != "")
            {
                if (strentryid == "")
                {
                    strentryid = common.myInt(Id).ToString();
                }
                else
                {
                    strentryid = strentryid + "," + common.myInt(Id);
                }

            }
        }

        DateTime frmdate = common.myDate(dtfromdate.SelectedDate);
        DateTime todate = common.myDate(dttodate.SelectedDate);

        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ds = new DataSet();
        StringBuilder str = new StringBuilder();
        str.Append("exec GetPatientAccountUser_entrysite '" + Convert.ToInt16(Session["HospitalLocationID"]) + "','" + common.myInt(Session["FacilityId"]) + "' ,");
        str.Append(" '" + frmdate.ToString("yyyy/MM/dd") + "','" + todate.AddDays(1).ToString("yyyy/MM/dd") + "'  ,'" + strentryid + "' ");
        ds = dl.FillDataSet(CommandType.Text, str.ToString());

        gvReporttype.DataSource = ds.Tables[0].Copy();

        gvReporttype.DataBind();


    }

    protected void OnSelectedDateChanged_change(object sender, SelectedDateChangedEventArgs e)
    { BindUser(); }

    private void BindUser()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        // BaseC.User objMaster = new BaseC.User(sConString);1
        DataSet ds = new DataSet();
        StringBuilder sb = new StringBuilder();
        DateTime frmdate = common.myDate(dtfromdate.SelectedDate);
        DateTime todate = common.myDate(dttodate.SelectedDate);
        BaseC.RestFulAPI wcfCobj = new BaseC.RestFulAPI(sConString);
        gvReporttype.DataSource = null;
        gvReporttype.DataBind();
        ds = wcfCobj.GetPatientAccount(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            frmdate.ToString("yyyy/MM/dd"), todate.AddDays(1).ToString("yyyy/MM/dd"));

        gvReporttype.DataSource = ds.Tables[0].Copy();
        gvReporttype.DataBind();

    }

    void BindMinut()
    {

        for (int i = 0; i < 60; i++)
        {
            if (i.ToString().Length == 1)
            {

                RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                RadComboBox2.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
            }
            else
            {
                RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                RadComboBox2.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }
        }
        int iMinute = DateTime.Now.Hour;
        RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
        RadComboBoxItem rcbItem1 = (RadComboBoxItem)RadComboBox2.Items.FindItemByText(iMinute.ToString());
        //if (rcbItem != null)
        //{
        //    rcbItem.Selected = true;
        //    rcbItem1.Selected = true;
        //}
    }
    protected void rblReportWise_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        BindNameList(1);
    }

    private void bindentrysite()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ds = new DataSet();
        StringBuilder str = new StringBuilder();
        str.Append("select esid,esname from EntrySiteMaster where FacilityId='" + common.myInt(Session["FacilityId"]) + "' AND Active=1");
        str.Append(" and HospitalLocationId='" + Convert.ToInt16(Session["HospitalLocationID"]) + "'  ORDER BY esname ");
        ds = dl.FillDataSet(CommandType.Text, str.ToString());

        Radentry.DataSource = ds;
        Radentry.DataTextField = ds.Tables[0].Columns[1].ColumnName;
        Radentry.DataValueField = ds.Tables[0].Columns[0].ColumnName;
        Radentry.DataBind();
    }

    public void bindonlypharmacy()
    {

        //GetPatientAccountPhrUser
        DateTime frmdate = common.myDate(dtfromdate.SelectedDate);
        DateTime todate = common.myDate(dttodate.SelectedDate);

        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ds = new DataSet();
        StringBuilder str = new StringBuilder();
        str.Append("exec GetPatientAccountPhrUser '" + Convert.ToInt16(Session["HospitalLocationID"]) + "','" + common.myInt(Session["FacilityId"]) + "' ,");
        str.Append(" '" + frmdate.ToString("yyyy/MM/dd") + "','" + todate.AddDays(1).ToString("yyyy/MM/dd") + "'");
        ds = dl.FillDataSet(CommandType.Text, str.ToString());
        gvReporttype.DataSource = ds.Tables[0];
        gvReporttype.DataBind();

    }





    private void bindBookingStatus()
    {
        try
        {
            BaseC.clsBb objbb = new BaseC.clsBb(sConString);
            ds = objbb.GetStatusMaster("OT");

            DataRow DR = ds.Tables[0].NewRow();
            //DR["Status"] = "All";
            //DR["StatusId"] = "0";
            //  ds.Tables[0].Rows.InsertAt(DR, 0);

            ddlOTStatus.DataSource = ds.Tables[0];
            ddlOTStatus.DataTextField = "Status";
            ddlOTStatus.DataValueField = "StatusId";
            ddlOTStatus.DataBind();

            foreach (RadComboBoxItem item in ddlOTStatus.Items)
            {
                item.Checked = true;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    private void BindNameList(int IsPreRender)
    {
        if (IsPreRender == 1)
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            if (rblReportWise.SelectedValue == "S")
            {
                Label4.Text = "Speciality";
                ds = dl.FillDataSet(CommandType.Text, "SELECT Id, + '(' + Code + ') ' + Name AS 'Name' FROM SpecialisationMaster WHERE Active = 1");
            }
            else if (rblReportWise.SelectedValue == "")
            {
                Label4.Text = "Doctor";
                StringBuilder str = new StringBuilder();
                str.Append("SELECT e.Id, dbo.GetDoctorName(e.ID) AS Name FROM Employee e ");
                str.Append(" WHERE HospitalLocationID='" + Convert.ToInt16(Session["HospitalLocationID"]) + "' and EmployeeType IN (1,2) AND e.Active=1 ORDER BY Name ");
                ds = dl.FillDataSet(CommandType.Text, str.ToString());
            }
            else if (rblReportWise.SelectedValue == "B")
            {
                Label4.Text = "Bed Category";
                StringBuilder str = new StringBuilder();
                str.Append("SELECT ios.ServiceId  AS Id, ios.ServiceName AS Name FROM BedCategoryMaster bcm ");
                str.Append("INNER JOIN ItemOfService ios ON bcm .BedCategoryId=ios.ServiceId ");
                str.Append(" WHERE ios.HospitalLocationID='" + Convert.ToInt16(Session["HospitalLocationID"]) + "' AND Active=1 ORDER BY Name ");
                ds = dl.FillDataSet(CommandType.Text, str.ToString());
            }

            else if (rblReportWise.SelectedValue == "D")
            {
                Label4.Text = "Department";
                StringBuilder str = new StringBuilder();
                str.Append("SELECT DepartmentID  AS Id, DepartmentName AS Name FROM DepartmentMain  ");
                str.Append(" WHERE HospitalLocationID='" + common.myInt(Session["HospitalLocationID"]) + "' AND FacilityId='" + common.myInt(Session["FacilityId"]) + "' AND Active=1 ORDER BY Name ");
                ds = dl.FillDataSet(CommandType.Text, str.ToString());
            }
            if (!rblReportWise.SelectedValue.Equals("W"))
            {
                gvReporttype.DataSource = ds;
                gvReporttype.DataBind();
            }

        }
    }

    protected void Bindcompany()
    {
        BaseC.clsEMRBilling objbilling = new BaseC.clsEMRBilling(sConString);

        DataSet ds = new DataSet();

        BaseC.RestFulAPI wcfCobj = new BaseC.RestFulAPI(sConString);

        //ds = objbilling.getCompanyList(common.myInt(Session["HospitalLocationId"]), "C", 0);
        // ds = wcfCobj.GetCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        ds = wcfCobj.GetCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "O");
        if (ddlPatienttype.SelectedValue == "C")
        {
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "PaymentType = 'C'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                dt.Columns[0].ColumnName = "Id";
                gvReporttype.DataSource = dt;
                gvReporttype.DataBind();
            }
        }
        else if (ddlPatienttype.SelectedValue == "B")
        {
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "PaymentType = 'B'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                dt.Columns[0].ColumnName = "Id";
                gvReporttype.DataSource = dt;
                gvReporttype.DataBind();
            }
        }
        else
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns[0].ColumnName = "Id";
                gvReporttype.DataSource = ds;
                gvReporttype.DataBind();
            }
        }
    }
    protected void gvReporttype_PreRender(object sender, EventArgs e)
    {
        if (Stauts == false)
        {
            if (common.myStr(Request.QueryString["RN"]) == "Rev")
            {
                if (ChkCompany.Checked == true)
                    Bindcompany();
                else
                    BindNameList(1);
            }
            else if (common.myStr(Request.QueryString["RN"]) == "ProcedureWiseRevenue")
            {
                if (ChkCompany.Checked == true)
                    Bindcompany();
                else
                    BindNameList(1);
            }
            else if (common.myStr(Request.QueryString["RN"]) == "DoctorAccountingWise")
            {
                if (ChkCompany.Checked == true)
                    Bindcompany();
                else
                    BindNameList(1);
            }
            else if (common.myStr(Request.QueryString["RN"]) == "CashCollec")
            {
                if (common.myStr(Request.QueryString["PCall"]) == "N")
                {
                    BindUser();
                }
                else
                {
                    bindonlypharmacy();

                }
            }
            else if (common.myStr(Request.QueryString["RN"]) == "CashCollecEntry")
            {
                //BindUser();
                binduser_entrysitewise();

            }
            else if (common.myStr(Request.QueryString["RN"]) == "OPDZero")
            { BindUser(); }
            else if (common.myStr(Request.QueryString["RN"]) == "OPVisit")
            { BindDoctor(); }
            else
            {
                Bindcompany();
            }

        }
    }


    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        string setformula = "";
        string EntrySite;
        String sbIdList = "";
        if (common.myStr(Request.QueryString["RN"]) != "Disc" && common.myStr(Request.QueryString["RN"]) != "Rev"
            && common.myStr(Request.QueryString["RN"]) != "DSR"
            && common.myStr(Request.QueryString["RN"]) != "BillCanc" && common.myStr(Request.QueryString["RN"]) != "Refund"
            && common.myStr(Request.QueryString["RN"]) != "OPVisit" && common.myStr(Request.QueryString["RN"]) != "CashCollec" && common.myStr(Request.QueryString["RN"]) != "CashCollecEntry"
            && common.myStr(Request.QueryString["RN"]) != "CrNote" && common.myStr(Request.QueryString["RN"]) != "DWS"
            && common.myStr(Request.QueryString["RN"]) != "InPStat" && common.myStr(Request.QueryString["RN"]) != "BOccup"
            && common.myStr(Request.QueryString["RN"]) != "OPTAT" && common.myStr(Request.QueryString["RN"]) != "BRITHREG"
            && common.myStr(Request.QueryString["RN"]) != "DEFICIENCYRPT"
            && common.myStr(Request.QueryString["RN"]) != "ICUUti" && common.myStr(Request.QueryString["RN"]) != "DIRECTBILLWOENC"
            && common.myStr(Request.QueryString["RN"]) != "DEATHREG" && common.myStr(Request.QueryString["RN"]) != "MRDSURG"
            && common.myStr(Request.QueryString["RN"]) != "MRDWELLNESS"
            && common.myStr(Request.QueryString["RN"]) != "VATREPORT" && common.myStr(Request.QueryString["RN"]) != "TDS" && common.myStr(Request.QueryString["RN"]) != "UCH"
            && common.myStr(Request.QueryString["RN"]) != "SLOB"
            && common.myStr(Request.QueryString["RN"]) != "SRDD"

            && common.myStr(Request.QueryString["RN"]) != "MDS"
            && common.myStr(Request.QueryString["RN"]) != "AmublanceBillOPD" && common.myStr(Request.QueryString["RN"]) != "OPTOIP" && common.myStr(Request.QueryString["RN"]) != "OPDBilling" && common.myStr(Request.QueryString["RN"]) != "TCSTAXReport"
            && common.myStr(Request.QueryString["RN"]) != "CmpMarkup" && common.myStr(Request.QueryString["RN"]) != "Sponsorwise" && common.myStr(Request.QueryString["RN"]) != "ProcedureIPDdetails" && common.myStr(Request.QueryString["RN"]) != "DepositExhaustCash"
            && common.myStr(Request.QueryString["RN"]) != "ConcessionSummary" && common.myStr(Request.QueryString["RN"]) != "ProcedureWiseRevenue"
            && common.myStr(Request.QueryString["RN"]) != "ContributionCompanyType" && common.myStr(Request.QueryString["RN"]) != "SponsorwiseOutstanding" && common.myStr(Request.QueryString["RN"]) != "DoctorAccountingWise"
            && (common.myStr(Request.QueryString["RN"]).Equals("OS") ||
                common.myStr(Request.QueryString["RN"]).Equals("BillRegister") ||
                common.myStr(Request.QueryString["RN"]).Equals("CREDITCOLL") ||
                common.myStr(Request.QueryString["RN"]).Equals("IPTAT") ||
                common.myStr(Request.QueryString["RN"]).Equals("OPDZero")))
        {
            if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
            {
                sbIdList = "A";
            }
            else
            {
                foreach (GridDataItem item in gvReporttype.Items)
                {
                    if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                    {
                        if (sbIdList == "")
                            sbIdList = ((Label)item.FindControl("lblId")).Text;
                        else
                            sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                    }
                }
            }
            // ViewState["setformula"] = sbIdList;

            if (sbIdList == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                lblMessage.Text = "Please Select Name(s) !";
                return;
            }
        }


        lblMessage.Text = "";
        if (common.myStr(Request.QueryString["RN"]) == "Disc")
        {
            string summary = string.Empty;
            if (chkSummary.Checked)
            {
                summary = "summary";
            }


            if (Convert.ToString(ddlEntrySite.SelectedValue) == "0")
                EntrySite = "A";
            else
                EntrySite = Convert.ToString(ddlEntrySite.SelectedValue) + ",";

            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&RptType=" + ddlAuthorization.SelectedValue + "&EntrySite=" + EntrySite + "&PaymentType=" + DropDownPaymentType.SelectedValue + "&Summary=" + summary + "&OutPut=A&ReportName=Discountbill";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OA")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?PaymentType=" + ddlPatienttype.SelectedValue + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=OutStandingAsone";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OS")
        {

            if (Convert.ToString(ddlEntrySite.SelectedValue) == "0")
                EntrySite = "A";
            else
                EntrySite = Convert.ToString(ddlEntrySite.SelectedValue) + ",";

            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?PaymentType=" + ddlPatienttype.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&SourceType=" + ddlOPIP.SelectedValue + "&ReportType=" + rdldatewise.SelectedValue + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&OutPut=N&ReportName=" + rdlsummary.SelectedValue + "";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OAD")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&SourceType=" + ddlOPIP.SelectedValue + "&ReportName=OAD";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OSD")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&SourceType=" + ddlOPIP.SelectedValue + "&ReportName=OSD";
        }
        if (common.myStr(Request.QueryString["RN"]) == "Rev")
        {
            if (ChkCompany.Checked == true)
            {
                if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
                {
                    sbIdList = "A";
                }
                else
                {
                    foreach (GridDataItem item in gvReporttype.Items)
                    {
                        if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                        {
                            if (sbIdList == "")
                                sbIdList = ((Label)item.FindControl("lblId")).Text;
                            else
                                sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                        }
                    }
                }
                //ViewState["setformula"] = sbIdList;
                if (sbIdList == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "Please Select Company Name(s) !";
                    return;
                }
            }


            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);


            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&GrbType=" + rblReportWise.SelectedValue + "&ReportName=Revenue" + "&EntrySite=" + EntrySite + "&CompanyWise=" + ChkCompany.Checked;
        }
        if (common.myStr(Request.QueryString["RN"]) == "IPTAT")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=IPTAT";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OPTAT")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=OPTAT";
        }
        if (common.myStr(Request.QueryString["RN"]) == "DSR")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&EntrySite=" + EntrySite + "&Export=" + chkExport.Checked + "&ReportName=DSR";
        }
        if (common.myStr(Request.QueryString["RN"]) == "BillCanc")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=BillCanc";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OPDZero")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&UserList=" + sbIdList + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&ReportName=OPDZeroBilling";
        }

        if (common.myStr(Request.QueryString["RN"]) == "TCSTAXReport")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=TCSTAXReport";
        }

        if (common.myStr(Request.QueryString["RN"]) == "CmpMarkup")
        {
            if (chkdetail.Checked == true)
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=CmpMarkup";
            }
            else
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=CmpMarkupService";
            }
        }

        if (common.myStr(Request.QueryString["RN"]) == "Sponsorwise")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=Sponsorwise";
        }

        if (common.myStr(Request.QueryString["RN"]) == "ProcedureIPDdetails")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=ProcedureIPDdetails";
        }

        if (common.myStr(Request.QueryString["RN"]) == "OPDBilling")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=OPDBillingReport";
        }

        if (common.myStr(Request.QueryString["RN"]) == "DepositExhaustCash")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=DepositExhaustCash";
        }

        if (common.myStr(Request.QueryString["RN"]) == "CREDITCOLL")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList
             + "&PatientType=" + ddlPatienttype.SelectedValue + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=True&ReportName=OutstandingCollection";
        }

        if (common.myStr(Request.QueryString["RN"]) == "ConcessionSummary")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=ConcessionSummary";
        }

        if (common.myStr(Request.QueryString["RN"]) == "ProcedureWiseRevenue")
        {
            // foreach (GridDataItem dataItem in gvReporttype.Items)
            // {
            //     CheckBox lbtn = sender as CheckBox;
            //     CheckBox chkAllDep = (CheckBox)dataItem.FindControl("chkAllDepartment");
            //   if (!chkAllDep.Checked)
            //    {
            //        lblMessage.Text = "Please Select Doctor!";
            //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //         return;
            //     }
            //     else
            //     {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&GrbType=" + rblReportWise.SelectedValue + "&SourceType=" + ddlOPIP.SelectedValue + "&ReportName=ProcedureWiseRevenue";
            //     }
            //  }
        }
        if (common.myStr(Request.QueryString["RN"]) == "ContributionCompanyType")
        {
            if (dtpTodate.SelectedDate < DateTime.Now.Date)
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&ReportName=ContributionDoctor";
            }
            else
            {
                Alert.ShowAjaxMsg("Current Date is not Accepted!", Page);
                return;
            }
        }


        if (common.myStr(Request.QueryString["RN"]) == "SponsorwiseOutstanding")
        {
            if (dtpTodate.SelectedDate < DateTime.Now.Date)
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=SponsorwiseOutstanding";
            }
            else
            {
                Alert.ShowAjaxMsg("Current Date is not Accepted!", Page);
                return;
            }
        }

        if (common.myStr(Request.QueryString["RN"]) == "DoctorAccountingWise")
        {
            if (rdlsummary.SelectedValue == "S")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&DoctorType=" + rdlsummary.Items[0].Value + "&ReportName=DoctorAccountingWise";
            }
            else
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&DoctorType=" + rdlsummary.Items[1].Value + "&Doctordetail=" + ViewState["chkAllDep"] + "&ReportName=DoctorAccountingWiseDetail";
            }
        }

        if (common.myStr(Request.QueryString["RN"]) == "OPTOIP")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&ReportName=OPTOIPConversion";
        }
        if (common.myStr(Request.QueryString["RN"]) == "AmublanceBillOPD")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&ReportName=AmbulanceBillingOPD";
        }
        if (common.myStr(Request.QueryString["RN"]) == "Refund")
        {

            if (Convert.ToString(ddlEntrySite.SelectedValue) == "0")
                EntrySite = "A";
            else
                EntrySite = Convert.ToString(ddlEntrySite.SelectedValue) + ",";


            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&EntrySite=" + EntrySite + "&Export=" + chkExport.Checked + "&ReportName=Refund";
        }
        if (common.myStr(Request.QueryString["RN"]) == "BillRegister")
        {

            if (Convert.ToString(ddlEntrySite.SelectedValue) == "0")
                EntrySite = "A";
            else
                EntrySite = Convert.ToString(ddlEntrySite.SelectedValue) + ",";


            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList
                 + "&PatientType=" + ddlPatienttype.SelectedValue + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=BillRegister";
        }
        if (common.myStr(Request.QueryString["RN"]) == "OPVisit")
        {
            if (common.myBool(ViewState["chkAllDep"]) == false)
            {
                foreach (GridDataItem item in gvReporttype.Items)
                {
                    if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                    {
                        if (setformula == "")
                            setformula = ((Label)item.FindControl("lblId")).Text;
                        else
                            setformula = setformula + "," + ((Label)item.FindControl("lblId")).Text;
                    }

                }
                //ViewState["setformula"] = setformula;
            }
            else
            {
                setformula = "A";
            }
            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select doctor !";
                return;
            }




            if (Convert.ToString(ddlEntrySite.SelectedValue) == "0")
                EntrySite = "A";
            else
                EntrySite = Convert.ToString(ddlEntrySite.SelectedValue) + ",";

            //  RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&OPVisitType=" + chkdetail.Checked + "&UnderPackage=" + ddlUnderPackage.SelectedValue + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=OPVisit";
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&OPVisitType=" + chkdetail.Checked + "&UnderPackage=" + ddlUnderPackage.SelectedValue + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=OPVisit" + "&DoctorId=" + setformula;
        }
        if (common.myStr(Request.QueryString["RN"]) == "CashCollec")
        {
            var ddlModes = ddlMode.CheckedItems;
            String val = "";
            StringBuilder sbModes = new StringBuilder();
            if (ddlModes.Count != 0)
            {
                foreach (var item in ddlModes)
                {
                    if (val == "")
                    {
                        val = common.myStr(item.Value);
                    }
                    else
                    {
                        val = val + "," + common.myStr(item.Value);
                    }

                }
                sbModes.Append(val);
            }
            if (chkModewise.Checked && common.myLen(sbModes).Equals(0))
            {
                lblMessage.Text = "Please select Payment Mode";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            int isAccountSummary = 0;
            if (chkAccountSummary.Checked == true)
                isAccountSummary = 1;
            else
                isAccountSummary = 0;
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&Summary=" + chkSummary.Checked + "&ReportName=CashCollection&ModeName=" + common.myStr(sbModes) + "&ModeWise=" + chkModewise.Checked + "&isAccountSummary=" + Convert.ToString(isAccountSummary);
        }
        if (common.myStr(Request.QueryString["RN"]) == "CashCollecEntry")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&entryid=" + Radentry.SelectedItem + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&Summary=" + chkSummary.Checked + "&ReportName=CashCollection_entrysite";
        }

        if (common.myStr(Request.QueryString["RN"]) == "CrNote")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=CreditNote&Status=" + common.myStr(ddlType.SelectedValue);
        }
        if (common.myStr(Request.QueryString["RN"]) == "DWS")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=DWS";
        }
        if (common.myStr(Request.QueryString["RN"]) == "BOccup")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=BOccup";
        }
        if (common.myStr(Request.QueryString["RN"]) == "ICUUti")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=ICUUti";
        }
        if (common.myStr(Request.QueryString["RN"]) == "InPStat")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=InPStat&ReportType=" + ddlInpatientType.SelectedValue.ToString() + "";
        }
        if (common.myStr(Request.QueryString["RN"]) == "BRITHREG")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=BRITHREG";
        }
        if (common.myStr(Request.QueryString["RN"]) == "DEFICIENCYRPT")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=DEFICIENCYRPT&DeficiencyCategoryId=" + common.myStr(common.myInt(ddlDeficiencyCategory.SelectedValue));
        }
        if (common.myStr(Request.QueryString["RN"]) == "DIRECTBILLWOENC")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=DIRECTBILLWOENC";
        }
        if (common.myStr(Request.QueryString["RN"]) == "DEATHREG")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=DEATHREG";
        }
        if (common.myStr(Request.QueryString["RN"]) == "MRDSURG")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var collection = ddlOTStatus.CheckedItems;
            string val = "";
            sb = new StringBuilder();
            if (collection.Count != 0)
            {
                foreach (var item in collection)
                {
                    if (common.myInt(item.Value) == 0)
                    {
                        sb.Append("A");
                        break;
                    }
                    else
                    {
                        if (val == "")
                        {
                            val = common.myStr(item.Value);
                        }
                        else
                        {
                            val = val + "," + common.myStr(item.Value);
                        }
                    }
                    //if (sb.ToString() != string.Empty)
                    //{
                    //    sb.Append(",");
                    //}

                }
                sb.Append(val);
            }



            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=MRDSURG&OTStatusIds=" + sb.ToString();
        }
        if (common.myStr(Request.QueryString["RN"]) == "MRDWELLNESS")
        {
            EntrySite = Convert.ToString(ddlEntrySite.SelectedValue);
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&EntrySite=" + EntrySite + "&ReportName=MRDWELLNESS";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("VATREPORT"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=VATREPORT";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("SLOB"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=SurgeryListOrderBase";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("SRDD"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=SurgeryReportDischargeDatebase";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("MDS"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=MDS";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("TDS"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=TDS";
        }

        if (common.myStr(Request.QueryString["RN"]).Equals("UCH"))//Users Collection Handover
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=UCH";
        }

        if (common.myStr(Request.QueryString["RN"]) == "HPDCW")
        {
            if (ChkCompany.Checked == true)
            {
                if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
                {
                    sbIdList = "A";
                }
                else
                {
                    foreach (GridDataItem item in gvReporttype.Items)
                    {
                        if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                        {
                            if (sbIdList == "")
                                sbIdList = ((Label)item.FindControl("lblId")).Text;
                            else
                                sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                        }
                    }
                }
                //ViewState["setformula"] = sbIdList;
                if (sbIdList == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "Please Select Company Name(s) !";
                    return;
                }
            }

            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate +
                "&CompanyCode=" + sbIdList + "&PatientType=" + ddlPatienttype.SelectedValue + "&Export=" + chkExport.Checked + "&ReportName=HPDCW&Rtype=" +
                ddlReporttype.SelectedValue;


        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1020;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;

        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
        ViewState["chkAllDep"] = "";
    }

    protected void btnPrintreport1_OnClick(object sender, EventArgs e)
    {
        String sbIdList = "";
        string strentryid = "";
        string[] entryid = common.GetCheckedItems(Radentry).Split(',');
        foreach (string Id in entryid)
        {
            if (Id != "")
            {
                if (strentryid == "")
                {
                    strentryid = common.myInt(Id).ToString();
                }
                else
                {
                    strentryid = strentryid + "," + common.myInt(Id);
                }

            }
        }
        if (common.myStr(Request.QueryString["RN"]) == "CashCollecEntry")
        {
            if (strentryid == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                lblMessage.Text = "Please Select Entry Site Name !";
                return;

            }
        }
        //for (int i = 0; i < Radentry.Items.Count; i++)
        //{
        //    if (Radentry.ch == true)
        //    {

        //    }
        //}

        //if (common.myStr(Request.QueryString["RN"]) == "CashCollec")
        //{

        if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
        {
            if (common.myStr(Request.QueryString["Pcall"]) == "Y")  //for avoid query string errpr in Pharmacy
            {
                foreach (GridDataItem item in gvReporttype.Items)
                {
                    if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                    {
                        if (sbIdList == "")
                            sbIdList = ((Label)item.FindControl("lblId")).Text;
                        else
                            sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                    }
                }


            }
            else
            {

                sbIdList = "A";
            }
        }
        else
        {
            foreach (GridDataItem item in gvReporttype.Items)
            {
                if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                {
                    if (sbIdList == "")
                        sbIdList = ((Label)item.FindControl("lblId")).Text;
                    else
                        sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                }
            }
        }
        //ViewState["setformula"] = sbIdList;

        if (sbIdList == "" && chkAccountSummary.Checked == false)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            lblMessage.Text = "Please Select User Name(s) !";
            return;

        }
        //}san

        if (common.myStr(Request.QueryString["RN"]) == "CashCollec")
        {
            var ddlModes = ddlMode.CheckedItems;
            string val = "";
            StringBuilder sbModes = new StringBuilder();
            if (ddlModes.Count != 0)
            {
                foreach (var item in ddlModes)
                {

                    if (val == "")
                    {
                        val = common.myStr(item.Value);
                    }
                    else
                    {
                        val = val + "," + common.myStr(item.Value);
                    }

                }
                sbModes.Append(val);
            }
            if (chkModewise.Checked && common.myLen(sbModes).Equals(0))
            {
                lblMessage.Text = "Please select Payment Mode";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            int isAccountSummary = 0;
            if (chkAccountSummary.Checked == true)
                isAccountSummary = 1;
            else
                isAccountSummary = 0;
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?FromdateTime=" + dtfromdate.SelectedDate + "&TodateTime=" + dttodate.SelectedDate + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&DW=Y&Summary=" + chkSummary.Checked + "&ReportName=CashCollection&PCall=" + Convert.ToString(Request.QueryString["PCall"]) + "&ModeName=" + common.myStr(sbModes) + "&ModeWise=" + chkModewise.Checked + "&isAccountSummary=" + Convert.ToString(isAccountSummary);
        }




        if (common.myStr(Request.QueryString["RN"]) == "CashCollecEntry")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?FromdateTime=" + dtfromdate.SelectedDate + "&TodateTime=" + dttodate.SelectedDate + "&entryid=" + strentryid + "&CompanyCode=" + sbIdList + "&Export=" + chkExport.Checked + "&Summary=" + chkSummary.Checked + "&ReportName=CashCollection_entrysite&PCall=" + Convert.ToString(Request.QueryString["PCall"]);
        }
        ViewState["chkAllDep"] = "";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        //  RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void ddlPatienttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["chkAllDep"] = "";
        Bindcompany();
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dtfromdate.SelectedDate.Value.ToString());
        sb.Remove(dtfromdate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(dtfromdate.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.SelectedItem.Text);
        dtfromdate.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    protected void RadComboBox2_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dttodate.SelectedDate.Value.ToString());
        sb.Remove(dttodate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(dttodate.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox2.SelectedItem.Text);
        dttodate.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    protected void rdlsummary_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["RN"]) == "DoctorAccountingWise")
        {
            if (rdlsummary.SelectedValue == "D")
            {
                trgrid.Visible = true;
                gvReporttype.Visible = true;
                rblReportWise.SelectedValue = "";
            }
        }
    }

    protected void rdldatewise_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myStr(rdldatewise.SelectedValue) == "A")
        {
            Label3.Text = "As On Date";
            // dtpTodate.Enabled = false;
            lbltdfrmdate.Visible = false;
            dttdfrmdate.Visible = false;
            dtpfromdate.DateInput.Text = "";
        }
        else
        {
            Label3.Text = "To Date";
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
            lbltdfrmdate.Visible = true;
            dttdfrmdate.Visible = true;
        }
    }
    protected void cmbentrysite_selectedchange(object sender, EventArgs e)
    {
        binduser_entrysitewise();
    }

    protected void chkentrysite_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chkentrysite.Checked == true)
            {
                common.CheckAllItems(Radentry);
            }
            else
            {
                common.UnCheckAllCheckedItems(Radentry);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void chkAllDepartment_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string chkcollection = "";
            Stauts = true;
            CheckBox lbtn = sender as CheckBox;
            GridTableRow row = lbtn.NamingContainer as GridTableRow;
            CheckBox chkAllDep = (CheckBox)row.FindControl("chkAllDepartment");
            ViewState["chkAllDep"] = ((CheckBox)row.FindControl("chkAllDepartment")).Checked;
            if (chkAllDep.Checked == true)
            {

                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = true;



                }
            }
            else
            {
                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = false;
                    if (chkcollection == "")
                        chkcollection = ((Label)rw.FindControl("lblId")).Text;
                    else
                        chkcollection = chkcollection + "," + ((Label)rw.FindControl("lblId")).Text;
                }
                ViewState["chkAllDep"] = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnExportToExcel_OnClick(object sender, EventArgs e)
    {
        BaseC.clsMISDashboard objval = new BaseC.clsMISDashboard(sConString);
        DataSet ds = new DataSet();
        try
        {
            String sbIdList = "";
            if (common.myStr(Request.QueryString["RN"]) != "Disc" && common.myStr(Request.QueryString["RN"]) != "Rev"
                && common.myStr(Request.QueryString["RN"]) != "IPTAT" && common.myStr(Request.QueryString["RN"]) != "DSR"
                && common.myStr(Request.QueryString["RN"]) != "BillCanc" && common.myStr(Request.QueryString["RN"]) != "Refund"
                && common.myStr(Request.QueryString["RN"]) != "OPVisit" && common.myStr(Request.QueryString["RN"]) != "CashCollec"
                && common.myStr(Request.QueryString["RN"]) != "CrNote" && common.myStr(Request.QueryString["RN"]) != "DWS"
                && common.myStr(Request.QueryString["RN"]) != "InPStat" && common.myStr(Request.QueryString["RN"]) != "BOccup"
                && common.myStr(Request.QueryString["RN"]) != "OPTAT" && common.myStr(Request.QueryString["RN"]) != "ICUUti")
            {
                if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
                {
                    sbIdList = "A";
                }
                else
                {
                    foreach (GridDataItem item in gvReporttype.Items)
                    {
                        if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                        {
                            if (sbIdList == "")
                                sbIdList = ((Label)item.FindControl("lblId")).Text;
                            else
                                sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                        }
                    }
                }
                // ViewState["setformula"] = sbIdList;

                if (sbIdList == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "Please Select Company Name(s) !";
                    return;
                }
            }

            ds = objval.GetBillRegister(dtpfromdate.SelectedDate.Value, dtpTodate.SelectedDate.Value, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), ddlPatienttype.SelectedValue, sbIdList, ddlOPIP.SelectedValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {


                    Response.ContentType = "Application/x-msexcel";
                    Response.AddHeader("content-disposition", "attachment;filename=BillRegister.csv");
                    Response.BufferOutput = true;
                    StringBuilder sbldr = new StringBuilder();
                    if (ds.Tables[0].Columns.Count != 0)
                    {
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            sbldr.Append(col.ColumnName + ',');
                        }
                        sbldr.Append("\r\n");
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            foreach (DataColumn column in ds.Tables[0].Columns)
                            {
                                sbldr.Append(row[column].ToString().Replace(",", " ") + ',');
                            }
                            sbldr.Append("\r\n");
                        }
                    }
                    Response.Write(sbldr.ToString());
                    Response.End();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objval = null;
        }
    }
    protected void ChkCompany_OnCheckedChanged(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["RN"]) == "Rev")
        {
            if (ChkCompany.Checked == true)
            {
                trgrid.Visible = true;
                Bindcompany();
                Label4.Text = "Company";
            }
            else
            {
                trgrid.Visible = false;
                gvReporttype.DataSource = null;
                gvReporttype.DataBind();
            }
        }
    }

    protected void chkModewise_OnCheckedChanged(object sender, EventArgs e)
    {
        ddlMode.Visible = chkModewise.Checked;
        lblMode.Visible = chkModewise.Checked;
        if (chkModewise.Checked)
            BindModeWise();
    }

    void BindModeWise()
    {
        ddlMode.Items.Clear();
        string strsql = "SELECT SubDeptId, Name FROM PaymentMode ORDER BY ID";
        DataSet ds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ds = dl.FillDataSet(CommandType.Text, strsql);
        ddlMode.DataSource = ds.Tables[0];
        ddlMode.DataTextField = "Name";
        ddlMode.DataValueField = "SubDeptId";
        ddlMode.DataBind();
        //ddlMode.Items.Insert(0, new RadComboBoxItem("All", "All"));
    }
    protected void chkAccountSummary_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAccountSummary.Checked == true)
        {
            pnlDep.Visible = false;
        }
        else
        {
            pnlDep.Visible = true;

        }
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        if (dtpTodate.SelectedDate < DateTime.Now.Date)
        {
            DataTable dt = BindData();
            ExportToExcel(dt, "ContributionReport");
        }
        else
        {
            Alert.ShowAjaxMsg("Current Date is not Accepted!", Page);
            return;
        }

    }

    void ExportToExcel(DataTable dt, string FileName)
    {
        if (dt.Rows.Count > 0)
        {
            FileName = FileName + ".csv";

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + "");
            StringBuilder sbldr = new StringBuilder();
            if (dt.Columns.Count != 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        sbldr.Append(row[column].ToString().Replace(",", " ") + ',');
                    }
                    sbldr.Append("\r\n");
                }
            }

            Response.Write(sbldr.ToString());
            Response.End();
        }
    }


    private DataTable BindData()
    {

        Hashtable hshInput = new Hashtable();
        // hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Convert.ToInt16(Session["HospitalLocationId"])));
        hshInput.Add("tdate", common.myDate(dtpTodate.SelectedDate));
        hshInput.Add("OPIP", common.myStr(ddlOPIP.SelectedValue));
        hshInput.Add("@intFacilityId", Convert.ToInt16(Convert.ToInt16(Session["FacilityId"])));



        DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = dal.FillDataSet(CommandType.StoredProcedure, "USP_ShowPerformingDoctorRevenueContribution_sponsor", hshInput);

        return ds.Tables[0];
    }


    protected void chkSpeciality_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSpeciality.Checked == true)
        {
            btnToExcel.Visible = false;
            btnPrintreport.Visible = true;
            chkcompanytype.Checked = false;
            chkExport.Visible = true;
            chkExport.Checked = true;
            chkExport.Enabled = false;
        }
    }

    protected void chkcompanytype_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcompanytype.Checked == true)
        {
            btnToExcel.Visible = true;
            btnPrintreport.Visible = false;
            chkSpeciality.Checked = false;
            chkExport.Visible = false;
        }
    }

    protected void BindDoctor()
    {
        try
        {


            BaseC.clsLISExternalCenter objM = new BaseC.clsLISExternalCenter(sConString);
            //DataTable tbl = objM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, common.myInt(Session["UserID"]));
            DataTable tbl = objM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);

            if (tbl.Rows.Count > 0)
            {
                tbl.Columns[0].ColumnName = "Id";
                tbl.Columns[1].ColumnName = "Name";

                gvReporttype.DataSource = tbl;
                gvReporttype.DataBind();
                // gvDoctorList.DataSource = tbl;
                //gvDoctorList.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }

    }





}
