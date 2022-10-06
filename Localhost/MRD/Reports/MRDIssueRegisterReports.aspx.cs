using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MRD_Reports_MRDIssueRegisterReports : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEntrySite();
            dtpfromdate.SelectedDate = DateTime.Now.AddDays(-30);
            dtpTodate.SelectedDate = DateTime.Now;

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

    protected void btnPrintreport_Click(object sender, EventArgs e)
    {
        if (common.myInt(ddlReportType.SelectedValue)==1)
        {
            chkMLC.Checked = true;
        }
        else
        {
            chkMLC.Checked = false;
        }
        RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate.ToString() + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&FacilityId=" + common.myStr(ddlEntrySite.SelectedValue) + "&BitMLC=" + chkMLC.Checked + "&Reporttype="+ ddlReportType.SelectedValue + "&ReportName=MRDIssueRegisterReports";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1020;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
}