using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MRD_Reports_OPVisitSummaryMRD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = DateTime.Now;
            dtpTodate.SelectedDate = DateTime.Now;
        }
    }
    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&UPackage=" + common.myStr(ddlUnderPackage.SelectedValue) + "&ReportName=MRDOPVISITSUMM";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1020;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnExportToExcel_OnClick(object sender, EventArgs e)
    {
    }
}
