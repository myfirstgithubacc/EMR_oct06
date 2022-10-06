using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class WardManagement_NursesAllocationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

        RadWindow1.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=NurseAllocate&FD=" + common.myDate(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd") + "&TD=" + common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd");
        RadWindow1.Height = 550;
        RadWindow1.Width = 850;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;


    }


}
