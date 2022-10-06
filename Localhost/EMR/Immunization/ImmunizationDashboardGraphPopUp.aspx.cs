using System;
using System.Configuration;
using System.Data;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;

[Serializable]
public partial class EMR_Immunization_ImmunizationDashboardGraphPopUp : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindYearMonth();
            BindControl();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            //tbComaprision.Visible = false;
            //btnShowComparision.Visible = false;
            dtpMTDYTDDate.SelectedDate = System.DateTime.Now;
            BaseC.clsMISDashboard clsMIS = new BaseC.clsMISDashboard(sConString);
            dtpMTDYTDDate.SelectedDate = System.DateTime.Now;
            ds = new DataSet();
            ds = clsMIS.getMISFinancialYear(Convert.ToInt16(ddlYear.SelectedValue), Convert.ToInt16(ddlMonth1.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["FinancialYear"] = ds.Tables[0].Rows[0]["FinancialYear"].ToString();
                ViewState["FinancialMonth"] = ds.Tables[0].Rows[0]["FinancialMonth"].ToString();
            }

            //if (Request.QueryString["ReportName"].ToString() == "MisDashboardRegistrationMonthDayGraph")
            //{
            //    //pnlCommon.Visible = true;
            //    ddlMonthYear.Enabled = false;
            //    ddlYear.Enabled = true;
            //    ddlYear.Visible = true;
            //    ddlMonthYear.Visible = true;
            //    tdLMonth.Visible = true;
            //    tdNMonth.Visible = true;
            //    pnlSurgery.Visible = false;
            //    pnlReceivable.Visible = false;
            //    pnlRevenue.Visible = false;
            //    pnlRevenueOption.Visible = false;
            //    pnlRegistration.Visible = false;
            //}
           // PrintReport(Request.QueryString["ReportName"].ToString());
        }

    }
    private void BindControl()
    {

        BaseC.clsMISDashboard clsMIS = new BaseC.clsMISDashboard(sConString);
        ds = new DataSet();

        //ds = clsMIS.getMISDashBoardMasterPages(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), Convert.ToInt16(Request.QueryString["SequenceNo"]), 0);
        string seqno = "0";
        if (Convert.ToInt16(Request.QueryString["SequenceNo"]) == 3 || Convert.ToInt16(Request.QueryString["SequenceNo"]) == 6)//Admission and Admitted Patients
        {
            seqno = "7";
        }
        else if (Convert.ToInt16(Request.QueryString["SequenceNo"]) == 9)//Night Occupancy
        {
            seqno = "9";
        }
        else if (Convert.ToInt16(Request.QueryString["SequenceNo"]) == 12)//Surgery
        {
            seqno = "14";
        }
        else if (Convert.ToInt16(Request.QueryString["SequenceNo"]) == 17)//Revenue
        {
            seqno = "18";
        }
        else
        {
            seqno = Request.QueryString["SequenceNo"];
        }
        ds = clsMIS.getMISDashBoardMasterPages(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), Convert.ToInt16(seqno), 0);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //ddlCommon.DataSource = ds.Tables[0];
            //ddlCommon.DataTextField = "DisplayCaption";
            //ddlCommon.DataValueField = "ID";
            //ddlCommon.DataBind();
        }
        // if (Request.QueryString["SequenceNo"].ToString() == "14")
        //if (Request.QueryString["SequenceNo"].ToString() == "12")//Surgery
        //{
        //   // pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = true;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //else if (Request.QueryString["SequenceNo"].ToString() == "3")// Admitted Patients
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //// else if (Request.QueryString["SequenceNo"].ToString() == "11")
        //else if (Request.QueryString["SequenceNo"].ToString() == "9")//Night Occupancy
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //// else if (Request.QueryString["SequenceNo"].ToString() == "7")
        //else if (Request.QueryString["SequenceNo"].ToString() == "6")//Admission
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //// else if (Request.QueryString["SequenceNo"].ToString() == "17")
        //else if (Request.QueryString["SequenceNo"].ToString() == "19")//Collection summary
        //{
        //    //pnlCommon.Visible = false;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = false; //yk
        //    tdNMonth.Visible = false; //yk
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //// else if (Request.QueryString["SequenceNo"].ToString() == "18")
        //else if (Request.QueryString["SequenceNo"].ToString() == "17")//Revenue
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = false;
        //    pnlRevenue.Visible = true;
        //    pnlReceivable.Visible = false;
        //    ddlRYear.Visible = false;
        //    Label4.Visible = false;
        //    lable1.Visible = false;
        //    ddlRMonth.Visible = false;
        //    Label1.Visible = false;
        //    ddlMonth1.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
        //else if (Request.QueryString["SequenceNo"].ToString() == "20")//Account Receivable
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Visible = false;
        //    ddlYear.Visible = false;
        //    tdLMonth.Visible = false;
        //    tdNMonth.Visible = false;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = true;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlReceivable.Visible = true;
        //    pnlRegistration.Visible = false;
        //}
        //else if (Request.QueryString["SequenceNo"].ToString() == "1")//Registrations
        //{
        //    //pnlCommon.Visible = true;
        //    ddlMonthYear.Visible = false;
        //    ddlYear.Visible = false;
        //    tdLMonth.Visible = false;
        //    tdNMonth.Visible = false;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRegistration.Visible = true;

        //}
        //else if (Request.QueryString["SequenceNo"].ToString() == "2")//OPD Consultations
        //{
        //    ////pnlCommon.Visible = true;
        //    ddlMonthYear.Enabled = false;
        //    ddlYear.Enabled = true;
        //    tdLMonth.Visible = true;
        //    tdNMonth.Visible = true;
        //    pnlSurgery.Visible = false;
        //    pnlReceivable.Visible = false;
        //    pnlRevenue.Visible = false;
        //    pnlRevenueOption.Visible = false;
        //    pnlRegistration.Visible = false;
        //}
    }
    protected void ddlMonthYear_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        if (common.myStr(ddlMonthYear.SelectedValue) == "M")
        {
            ddlMonth.Visible = true;
            ddlYear.Visible = false;
        }
        else
        {
            ddlMonth.Visible = false;
            ddlYear.Visible = true;
        }
        bindYearMonth();
    }
    private void bindYearMonth()
    {
        try
        {
            BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
            DataSet ds = objCommon.getMonthYear("M");
            ddlMonth.DataSource = ds.Tables[0];
            ddlMonth.DataValueField = "Id";
            ddlMonth.DataTextField = "Months";
            ddlMonth.DataBind();

            ddlMonth1.DataSource = ds.Tables[0];
            ddlMonth1.DataValueField = "Id";
            ddlMonth1.DataTextField = "Months";
            ddlMonth1.DataBind();


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindItemByValue(common.myStr(ds.Tables[1].Rows[0]["CurrentMonth"])));
                    ddlMonth1.SelectedIndex = ddlMonth1.Items.IndexOf(ddlMonth1.Items.FindItemByValue(common.myStr(ds.Tables[1].Rows[0]["CurrentMonth"])));
                }
            }

            ds = objCommon.getMonthYear("Y");
            ddlYear.DataSource = ds.Tables[0];
            ddlYear.DataValueField = "Years";
            //  ddlYear.DataTextField = "Years";
            ddlYear.DataTextField = "FyYears";
            ddlYear.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    // ddlYear.SelectedValue = common.myStr(ds.Tables[1].Rows[0]["CurrentYear"]);
                    ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindItemByText(common.myStr(ds.Tables[1].Rows[0]["FinancialYear"])));
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
    }
}