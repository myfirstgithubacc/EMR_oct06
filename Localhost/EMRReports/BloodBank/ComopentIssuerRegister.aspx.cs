using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class EMRReports_BloodBank_ComopentIssuerRegister : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    DataTable dt, dt1;

    protected void Page_Load(object sender, EventArgs e)
    {


        lblHeader.Text = "Blood Bank Reports";
        if (!IsPostBack)
        {

            bindCtrls();
        }
    }


    public void bindCtrls()
    {
        tdtxtCtr.Visible = false;
        tddate.Visible = false;
    }

    protected void ddlReportType_selectedIndexchanged(object sender, EventArgs e)
    {


        #region
        //if (ddlReportType.SelectedValue == "CIR"       // Component Issue Register
        //   || ddlReportType.SelectedValue == "BCDBW"  // Component Division Bag Wise
        //   || ddlReportType.SelectedValue == "BTR"    // Blood Transfer Report
        //   || ddlReportType.SelectedValue == "CWDD"   // Camp Wise Donor Detail
        //   || ddlReportType.SelectedValue == "CCMR"   // Component Cross Matched Report
        //   || ddlReportType.SelectedValue == "CRR"    // Component Recevied Report
        //   || ddlReportType.SelectedValue == "DR"     // Donor Report
        //   || ddlReportType.SelectedValue == "ES"     // Elisa Screeing
        //   || ddlReportType.SelectedValue == "KSR"    // Kit Stock Report
        //   || ddlReportType.SelectedValue == "VGR"    // Voluntary Genderwise Report
        //   || ddlReportType.SelectedValue == "MWBD"    // Manfacture wise Bag Details
        //   || ddlReportType.SelectedValue == "SR"     // Serum Report  
        //   || ddlReportType.SelectedValue == "CR"      // Cell Report
        //   || ddlReportType.SelectedValue == "WBIR"    //Whole Blood Issue Report
        //   )
        //{
        //    tddate.Visible = true;
        //    tdtxtCtr.Visible = false;
        //    lblfromdate.Visible = true;
        //    lblfromdate.Text = "From Date";
        //    lbltodate.Visible = true;
        //    dtpfromdate.Visible = true;
        //    dtpTodate.Visible = true;
        //}

        //if (ddlReportType.SelectedValue == "BTDW"        // Blood Transfer Date wise
        //    || ddlReportType.SelectedValue == "CS"      // Component Stock
        //    || ddlReportType.SelectedValue == "DSR"     // Daily Stock Report
        //    || ddlReportType.SelectedValue == "DDD"     // Daily Discard Detail
        //    || ddlReportType.SelectedValue == "ED"      // Expiry Detail
        //    || ddlReportType.SelectedValue == "FC"      // Fresh Collection
        //    || ddlReportType.SelectedValue == "TSR"     // Transaction Report 
        //    || ddlReportType.SelectedValue == "UBSR")    // UnSceenBlood Stock Report
        //{
        //    tddate.Visible = true;
        //    tdtxtCtr.Visible = false;
        //    lblfromdate.Visible = true;
        //    lblfromdate.Text = "Date";
        //    lbltodate.Visible = false;

        //    dtpfromdate.Visible = true;
        //    dtpTodate.Visible = false;


        //}

        //if (ddlReportType.SelectedValue == "BD"
        //    || ddlReportType.SelectedValue == "CBR"
        //    || ddlReportType.SelectedValue == "DPE"
        //    || ddlReportType.SelectedValue == "TFR"     // Tranfusion Report
        //    )
        //{

        //    tddate.Visible = false;
        //    tdtxtCtr.Visible = true;
        //    if (ddlReportType.SelectedValue == "CBR")
        //    {
        //        lblBagNo.Text = "Requistion No";
        //    }

        //    else if (ddlReportType.SelectedValue == "DPE")
        //    {
        //        lblBagNo.Text = "Reg. No";

        //    }

        //    else if (ddlReportType.SelectedValue == "TFR")
        //    {
        //        lblBagNo.Text = "Booking No.";
        //    }



        //    else
        //    {
        //        lblBagNo.Text = "Bag No.";

        //    }
        //}

        //if (ddlReportType.SelectedValue == "CD")
        //{
        //    tdtxtCtr.Visible = false;
        //    tddate.Visible = false;


        //}
        #endregion

        //--------------14-07-2014----------------------Prashant Bamania------------

        if (ddlReportType.SelectedValue == "DDRR" ||
            ddlReportType.SelectedValue == "DRD" ||
            ddlReportType.SelectedValue == "DDRR" ||
            ddlReportType.SelectedValue == "SRC" ||
            ddlReportType.SelectedValue == "DSUW"
            )
        {
            tddate.Visible = true;
            tdtxtCtr.Visible = false;
            lblfromdate.Visible = true;
            lblfromdate.Text = "Date";
            lbltodate.Visible = false;

            dtpfromdate.Visible = true;
            pnl.Visible = false;
            dtpTodate.Visible = false;
            pnlReg.Visible = false;
            //pnlReg.Style["display"] = "none";
            pnlFrom.Visible = true;

            lblIPNo.Visible = false;
            ddlEncounterNo.Visible = false;
            if (ddlReportType.SelectedValue == "DDRR")
            {
                //lblfromdate.Text = "From Date";
                //lbltodate.Text = "To Date";
                //dtpTodate.Visible = true;
                //lbltodate.Visible = true;
                tddate.Visible = true;
                tdtxtCtr.Visible = false;
                lblfromdate.Visible = true;
                lblfromdate.Text = "From Date";
                lbltodate.Visible = true;
                dtpfromdate.Visible = true;
                dtpTodate.Visible = true;
                pnl.Visible = true;
                pnlReg.Visible = false;
                //pnlReg.Style["display"] = "none";
                pnlFrom.Visible = true;
                span.Style["display"] = "block";
            }

        }
        if (ddlReportType.SelectedValue == "DRR" ||
            ddlReportType.SelectedValue == "BCITD" ||
            ddlReportType.SelectedValue == "BCITD" ||
            ddlReportType.SelectedValue == "CD" ||
            ddlReportType.SelectedValue == "DDSR" ||
            ddlReportType.SelectedValue == "DRRPD" ||
            ddlReportType.SelectedValue == "NEU" ||
            ddlReportType.SelectedValue == "VDL" ||
            ddlReportType.SelectedValue == "CGR" ||
            ddlReportType.SelectedValue == "DCBBA" ||
            ddlReportType.SelectedValue == "DLGW" ||
            ddlReportType.SelectedValue == "TAT" ||
            ddlReportType.SelectedValue == "BH" ||
            ddlReportType.SelectedValue == "RUC" ||
            ddlReportType.SelectedValue == "BCIR" ||
            ddlReportType.SelectedValue == "DRBGW" ||
            ddlReportType.SelectedValue == "ROES" ||
            ddlReportType.SelectedValue == "BSNR" ||
            ddlReportType.SelectedValue == "CMCSR" ||
            ddlReportType.SelectedValue == "BBSR" ||
            ddlReportType.SelectedValue == "BDR" ||
            ddlReportType.SelectedValue == "CDR" ||
            ddlReportType.SelectedValue == "DRL" ||
            ddlReportType.SelectedValue == "NTR"

            )
        {
            tddate.Visible = true;
            tdtxtCtr.Visible = false;
            lblfromdate.Visible = true;
            lblfromdate.Text = "From Date";
            lbltodate.Visible = true;
            dtpfromdate.Visible = true;
            dtpTodate.Visible = true;
            pnl.Visible = true;
            pnlReg.Visible = false;
            trUHID.Visible = false;
            //pnlReg.Style["display"] = "none";
            pnlFrom.Visible = true;
            span.Style["display"] = "none";
            lblIPNo.Visible = false;
            ddlEncounterNo.Visible = false;


            if (ddlReportType.SelectedValue == "TAT")
            {
                pnlDep.Visible = true;

                dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
                dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            }


            if (ddlReportType.SelectedValue == "DRRPD")
            {
                trUHID.Visible = true;
            }
        }

        if (ddlReportType.SelectedValue == "NTR")
        {

            tddate.Visible = true;
            tdtxtCtr.Visible = false;
            lblfromdate.Visible = true;
            lblfromdate.Text = "From Date";
            lbltodate.Visible = true;
            dtpfromdate.Visible = true;
            dtpTodate.Visible = true;
            pnl.Visible = true;
            pnlReg.Visible = false;
            trUHID.Visible = false;
            //pnlReg.Style["display"] = "none";
            pnlFrom.Visible = true;
            span.Style["display"] = "none";
            lblIPNo.Visible = false;
            ddlEncounterNo.Visible = false;
            rFvtxtBagNo.Enabled = false;

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            tdtxtCtr.Visible = true;
            
        }

        if (ddlReportType.SelectedValue == "CMM")
        {
            //============

            tddate.Visible = true;
            tdtxtCtr.Visible = false;
            //pnlFrom.Visible = false;
            //pnl.Visible = false;
            ////lblfromdate.Visible = false;
            ////lbltodate.Visible = false;
            ////dtpfromdate.Visible = false;
            ////dtpTodate.Visible = false;
            ////pnl.Visible = false;
            lblfromdate.Visible = true;
            lbltodate.Visible = true;
            dtpfromdate.Visible = true;
            dtpTodate.Visible = true;
            pnl.Visible = true;
            //pnlReg.Visible = false;
            pnlFrom.Visible = true;
            txtRegistration.Visible = true;
            lblIPNo.Visible = true;
            ddlEncounterNo.Visible = true;
            pnlReg.Visible = true;
            span.Style["display"] = "none";
            //===============


            //dtpfromdate.Visible = false;
            //dtpTodate.Visible = false;
            //pnl.Visible = false;
            //pnlFrom.Visible = false;

            //pnlReg.Visible = true; 
            //pnlReg.Style["display"] = "block";
            //txtRegistration.Visible = true;


        }
        if (ddlReportType.SelectedValue == "BIRCW")
        {
            tddate.Visible = true;
            tdtxtCtr.Visible = false;
            lblfromdate.Visible = true;
            lblfromdate.Text = "From Date";
            lbltodate.Visible = true;
            dtpfromdate.Visible = true;
            dtpTodate.Visible = true;
            pnl.Visible = true;
            pnlReg.Visible = false;
            //pnlReg.Style["display"] = "none";
            pnlFrom.Visible = true;
            span.Style["display"] = "none";
            BindComponent();
            trOPIP.Visible = true;
            //rblOPIP.Visible = true;
            //rblIssueReceive.Visible = true;
            pnlDep.Visible = true;
            lblIPNo.Visible = false;
            ddlEncounterNo.Visible = false;
        }
        else
        {
            if (ddlReportType.SelectedValue != "TAT")
            {
                pnlDep.Visible = false;
                gvReporttype.DataSource = string.Empty;
                gvReporttype.DataBind();
            }
            trOPIP.Visible = false;
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }

    }




    protected void btnPrintPreview_Click(object sender, EventArgs e)
    {

        if (ddlReportType.SelectedValue == "0")
        {
            lblMessage.Text = "Please Select Report Type";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        #region
        //if (ddlReportType.SelectedValue == "CIR"        // Component Issue Register
        //    || ddlReportType.SelectedValue == "BCDBW"   // Component Division Bag Wise
        //    || ddlReportType.SelectedValue == "BTR"    // Blood Transfer Report
        //    || ddlReportType.SelectedValue == "CWDD"    // Camp Wise Donor Detail
        //    || ddlReportType.SelectedValue == "CCMR"    // Component Cross Matched Report
        //    || ddlReportType.SelectedValue == "CRR"     // Component Recevied Report
        //    || ddlReportType.SelectedValue == "DR"      // Donor Report
        //    || ddlReportType.SelectedValue == "ES"      // Elisa Screeing
        //    || ddlReportType.SelectedValue == "KSR"    // Kit Stock Report
        //    || ddlReportType.SelectedValue == "VGR"     // Voluntary Genderwise Report
        //    || ddlReportType.SelectedValue == "MWBD"   // Manfacture wise Bag Details
        //    || ddlReportType.SelectedValue == "SR"  //Serum Report  
        //    || ddlReportType.SelectedValue == "CR"  //Cell Report
        //    || ddlReportType.SelectedValue == "WBIR"    //Whole Blood Issue Report
        //    )
        //{
        //    //tddate.Visible = true;
        //    //tdtxtCtr.Visible = false;
        //    //lblfromdate.Visible = true;
        //    //lblfromdate.Text = "From Date";
        //    // lbltodate.Visible = true;
        //    // dtpfromdate.Visible = true;
        //    //dtpTodate.Visible = true;

        //    //RadWindowForNew.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&DoctorId=" + setformula + "&Export=" + chkExport.Checked + "&ReportName=AppReport";
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;

        //}







        //if (ddlReportType.SelectedValue == "BTDW"        // Blood Transfer Date wise
        //    || ddlReportType.SelectedValue == "CS"      // Component Stock
        //    || ddlReportType.SelectedValue == "DSR"     // Daily Stock Report
        //    || ddlReportType.SelectedValue == "DDD"     // Daily Discard Detail
        //    || ddlReportType.SelectedValue == "ED"      // Expiry Detail
        //    || ddlReportType.SelectedValue == "FC"      // Fresh Collection
        //    || ddlReportType.SelectedValue == "TSR"     // Transaction Report 
        //    || ddlReportType.SelectedValue == "UBSR"    // UnSceenBlood Stock Report
        //    )
        //{
        //    //  tddate.Visible = true;
        //    // tdtxtCtr.Visible = false;
        //    //lblfromdate.Visible = true;
        //    //lblfromdate.Text = "Date";
        //    //lbltodate.Visible = false;
        //    //dtpfromdate.Visible = true;
        //    // dtpTodate.Visible = false;

        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Cdate=" + dtpfromdate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;


        //}

        //if (ddlReportType.SelectedValue == "BD"
        //    || ddlReportType.SelectedValue == "CBR"
        //    || ddlReportType.SelectedValue == "DPE"
        //    || ddlReportType.SelectedValue == "TFR"     // Tranfusion Report
        //    )
        //{
        //    tddate.Visible = false;
        //    tdtxtCtr.Visible = true;
        //    if (ddlReportType.SelectedValue == "CBR")
        //    {
        //        lblBagNo.Text = "Requistion No";
        //        RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?ReqNo=" + txtBagNo.Text + "&ReportName=" + ddlReportType.SelectedValue;
        //    }

        //    else if (ddlReportType.SelectedValue == "DPE")
        //    {
        //        lblBagNo.Text = "Reg. No";
        //        RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?RegNo=" + txtBagNo.Text + "&ReportName=" + ddlReportType.SelectedValue;

        //    }

        //    else if (ddlReportType.SelectedValue == "TFR") // Tranfusion Report
        //    {
        //        RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?BookingNo=" + txtBagNo.Text + "&ReportName=" + ddlReportType.SelectedValue;
        //    }

        //    else
        //    {
        //        lblBagNo.Text = "Bag No.";
        //        RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?BagNo=" + txtBagNo.Text + "&ReportName=" + ddlReportType.SelectedValue;

        //    }
        //}
        ////if (ddlReportType.SelectedValue == "CD")
        ////{
        ////    tdtxtCtr.Visible = false;
        ////    tddate.Visible = false;
        ////    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?ReportName=" + ddlReportType.SelectedValue;  
        ////}
        #endregion
        //-------------------------------------------New Report 14-07-2014-------------------------------------------------

        //Blood component Issue & Transfuse details
        if (ddlReportType.SelectedValue == "BCITD")
        {

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }
        if (ddlReportType.SelectedValue == "NTR")
        {

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&BagNo=" + txtBagNo.Text + "&ReportName=" + ddlReportType.SelectedValue;
        }
        //Cell Grouping Register
        if (ddlReportType.SelectedValue == "CGR")
        {

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }

        //Daily consumption of blood bag & Apheresis
        if (ddlReportType.SelectedValue == "DCBBA")
        {

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }

        //Deferral Donor Registration Record
        if (ddlReportType.SelectedValue == "DDRR")
        {

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?FromDate=" + Convert.ToDateTime(dtpfromdate.SelectedDate).ToString("yyyy/MM/dd") + "&ReportName=" + ddlReportType.SelectedValue + "&ToDate=" + Convert.ToDateTime(dtpTodate.SelectedDate).ToString("yyyy/MM/dd");
        }
        //Donation Due Status report
        if (ddlReportType.SelectedValue == "DDSR")
        {
            //RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Cdate=" + dtpfromdate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;
            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }
        //
        //Donor Registration Details
        if (ddlReportType.SelectedValue == "DRD")
        {
            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Cdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }
        //Stock Register for component
        if (ddlReportType.SelectedValue == "SRC" || ddlReportType.SelectedValue == "DSUW")
        {
            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue;
        }


        if (
            ddlReportType.SelectedValue == "VDL" || ddlReportType.SelectedValue == "CMCSR" || ddlReportType.SelectedValue == "BSNR"|| ddlReportType.SelectedValue == "ROES" || ddlReportType.SelectedValue == "DLGW" || ddlReportType.SelectedValue == "NEU" || ddlReportType.SelectedValue == "BCIR" || ddlReportType.SelectedValue == "USBS"||ddlReportType.SelectedValue== "DRBGW"||
            ddlReportType.SelectedValue == "DRR" || ddlReportType.SelectedValue == "BBSR" || ddlReportType.SelectedValue == "BDR" || ddlReportType.SelectedValue == "CDR"|| ddlReportType.SelectedValue == "DRL" || 
            ddlReportType.SelectedValue == "CD" || ddlReportType.SelectedValue == "DRRPD"|| ddlReportType.SelectedValue == "BH" || ddlReportType.SelectedValue == "RUC"
           )
        {
            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked + "&UHID=" + txtUHID.Text.Trim();


            if (ddlReportType.SelectedValue == "DRRPD")
            {
                if(txtUHID.Text.Trim()==string.Empty)
                {
                    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked + "&UHID=" + txtUHID.Text.Trim();
                }
                else
                {
                    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + common.myDate(dtpfromdate.SelectedDate.Value) + "&Todate=" + common.myDate(dtpTodate.SelectedDate.Value) + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked+"&UHID="+txtUHID.Text.Trim();
                }
                //trUHID.Visible = true;
            }
        }

        ////Voluntary Donor List
        //if (ddlReportType.SelectedValue == "VDL")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked;
        //}
        ////Gender Wise Donor List
        //if (ddlReportType.SelectedValue == "DLGW")
        //{

        //}
        ////Donor Registration Details
        //if (ddlReportType.SelectedValue == "NEU")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;
        //}
        ////DISCARD REGISTER REPORT
        //if (ddlReportType.SelectedValue == "DRR")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;
        //}
        //        //Component Division
        //if (ddlReportType.SelectedValue == "CD")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;
        //}
        ////Donor Registration Record with Patient Details--    
        //if (ddlReportType.SelectedValue == "DRRPD")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue;
        //}
        ////TAT
        //if (ddlReportType.SelectedValue == "TAT")
        //{
        //    RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked;
        //}




        //Cross Match
        if (ddlReportType.SelectedValue == "CMM")
        {
            string regid = string.Empty;
            BaseC.clsBb objBb = new BaseC.clsBb(sConString);
            DataSet ds = objBb.GetRegistrationId(common.myInt(txtRegistration.Text.Trim()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                regid = common.myStr(ds.Tables[0].Rows[0]["id"]);
                RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?RegNo=" + txtRegistration.Text.Trim() + "&UnitNo=" + ddlUnitNo.SelectedValue + "&ReportName=" + ddlReportType.SelectedValue + "&Regid=" + regid + "&EncounterId=" + ddlEncounterNo.SelectedValue + "&FromDate=" + common.myDate(dtpfromdate.SelectedDate).ToString("yyyy/MM/dd") + "&ToDate=" + common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd") + "&Export=" + chkExport.Checked;
            }

        }

        if (ddlReportType.SelectedValue == "TAT")
        {
            String setformula = "";
            lblMessage.Text = string.Empty;

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
            ViewState["setformula"] = setformula;

            if (dtpfromdate.SelectedDate == null || dtpTodate.SelectedDate == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select filter type !";
                return;
            }

            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select filter type !";
                return;
            }


            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Component=" + setformula + "&ReportName=" + ddlReportType.SelectedValue + "&Export=" + chkExport.Checked;

        }


        //Blood Issue Register Component Wise
        if (ddlReportType.SelectedValue == "BIRCW")
        {
            String setformula = "";
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
            ViewState["setformula"] = setformula;

            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select filter type !";
                return;
            }

            RadWindowFor_Report.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&OPIP=" + rblOPIP.SelectedValue + "&IssueOrReceive=" + rblIssueReceive.SelectedValue + "&Component=" + setformula + "&ReportName=" + ddlReportType.SelectedValue;
        }


        RadWindowFor_Report.Height = 600;
        RadWindowFor_Report.Width = 950;
        RadWindowFor_Report.Top = 40;
        RadWindowFor_Report.Left = 100;
        RadWindowFor_Report.VisibleOnPageLoad = true;
        RadWindowFor_Report.Modal = true;
        RadWindowFor_Report.VisibleStatusbar = false;
        RadWindowFor_Report.Behavior = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowFor_Report.VisibleStatusbar = false;


        //RadWindowFor_Report.NavigateUrl  = "/EMRReports/BloodBank/BloodBankReport.aspx";
        //RadWindowFor_Report.Height = 600;
        //RadWindowFor_Report.Width = 950;
        //RadWindowFor_Report.Top = 40;
        //RadWindowFor_Report.Left = 100;
        //RadWindowFor_Report.VisibleOnPageLoad = true;
        //RadWindowFor_Report.Modal = true;
        //RadWindowFor_Report.VisibleStatusbar = false;
        //RadWindowFor_Report.Behavior = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        //RadWindowFor_Report.VisibleStatusbar = false;   


        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindowForNew.Modal = true;
        //RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        //RadWindowForNew.VisibleStatusbar = false;


    }
    protected void txtRegistration_TextChanged(object sender, EventArgs e)
    {

        BaseC.clsBb objBb = new BaseC.clsBb(sConString);

        dt = new DataTable();
        dt1 = new DataTable();
        dt1 = (objBb.GetUnitNoWithPatient(common.myStr(txtRegistration.Text), 0)).Tables[1];
        if (dt1.Rows.Count > 0)
        {
            ddlEncounterNo.DataSource = null;
            ddlEncounterNo.DataSource = dt1;
            ddlEncounterNo.DataTextField = "EncounterNo";
            ddlEncounterNo.DataValueField = "EncounterId";
            ddlEncounterNo.DataBind();
            ddlEncounterNo.Items.Insert(0, new ListItem("Select All", "0"));
        }
        else
        {
            ddlUnitNo.DataSource = null;
            ddlEncounterNo.DataSource = dt1;
            ddlUnitNo.DataBind();
            ddlEncounterNo.Items.Insert(0, new ListItem("Select All", "0"));
        }

        dt = (objBb.GetUnitNoWithPatient(common.myStr(txtRegistration.Text), 0)).Tables[0];
        if (dt.Rows.Count > 0)
        {
            ddlUnitNo.DataSource = null;
            ddlUnitNo.DataSource = dt;
            ddlUnitNo.DataTextField = "BagNo";
            ddlUnitNo.DataValueField = "TestStockUnitNo";
            ddlUnitNo.DataBind();
            ddlUnitNo.Items.Insert(0, new ListItem("Select", ""));
        }
        else
        {
            ddlUnitNo.DataSource = null;
            ddlUnitNo.DataSource = dt;
            ddlUnitNo.DataBind();
            ddlUnitNo.Items.Insert(0, new ListItem("Select", ""));
        }
    }

    protected void ddlEncounterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseC.clsBb objBb = new BaseC.clsBb(sConString);
        dt = new DataTable();
        dt = (objBb.GetUnitNoWithPatient(common.myStr(txtRegistration.Text), common.myInt(ddlEncounterNo.SelectedValue))).Tables[0];
        //DataRow[] result =dt.Select("EncounterId = " + ddlEncounterNo.SelectedValue);
        //dt2 = result.CopyToDataTable();
        if (dt.Rows.Count > 0)
        {
            ddlUnitNo.DataSource = null;
            ddlUnitNo.DataSource = dt;
            ddlUnitNo.DataTextField = "BagNo";
            ddlUnitNo.DataValueField = "TestStockUnitNo";
            ddlUnitNo.DataBind();
            ddlUnitNo.Items.Insert(0, new ListItem("Select", ""));
        }
        else
        {
            ddlUnitNo.DataSource = null;
            ddlUnitNo.DataSource = dt;
            ddlUnitNo.DataBind();
            ddlUnitNo.Items.Insert(0, new ListItem("Select", ""));
        }
    }

    //============================ for blood issue register component wise
    protected void BindComponent()
    {
        BaseC.clsBb objM = new BaseC.clsBb(sConString);
        //  DataTable tbl = objM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, common.myInt(Session["UserID"]));
        //DataSet dsComponent = objM.GetBloodComponentMaster(0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
        DataSet dsComponent = objM.GetBloodComponentMaster(0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, string.Empty);
        if (dsComponent.Tables.Count > 0)
        {
            if (dsComponent.Tables[0].Rows.Count > 0)
            {
                dsComponent.Tables[0].Columns[0].ColumnName = "Id";
                dsComponent.Tables[0].Columns[4].ColumnName = "Name";

                gvReporttype.DataSource = dsComponent.Tables[0];
                gvReporttype.DataBind();
            }
        }

    }

    private bool Stauts = false;
    clsExceptionLog objException = new clsExceptionLog();
    protected void chkAllDepartment_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
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
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvReporttype_PreRender(object sender, EventArgs e)
    {
        if (Stauts == false)
        {

            if (common.myStr(ddlReportType.SelectedValue) == "BIRCW" || common.myStr(ddlReportType.SelectedValue) == "TAT")
            {
                BindComponent();
            }
        }
    }


}
