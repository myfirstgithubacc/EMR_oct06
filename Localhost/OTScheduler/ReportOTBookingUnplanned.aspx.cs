using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OTScheduler_ReportOTBookingUnplanned : System.Web.UI.Page
{

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.AddDays(-2).ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
           
            
        }
    }

   protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        // if(ddlReportType.SelectedIndex==0)
        if (ddlReportType.SelectedValue == "1")

        {
            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.AddDays(-2).ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }
        //else if(ddlReportType.SelectedIndex==1)
        //{
        //    dtpfromdate.Enabled = false;
        //    dtpTodate.Enabled = false;

        //    dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
        //    dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
        //    dtpfromdate.SelectedDate = common.myDate(DateTime.Now.AddHours(-48).ToString(common.myStr(Session["OutputDateFormat"])));

        //    dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
        //    dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
        //    dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        //}
        // else if(ddlReportType.SelectedIndex==1)// 7 days
        else if (ddlReportType.SelectedValue == "0")
        {
            dtpfromdate.Enabled =true;
            dtpTodate.Enabled = true;

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.AddDays(-7).ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }
        // else if(ddlReportType.SelectedIndex==2)
        else if (ddlReportType.SelectedValue == "2")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }
        //  else if(ddlReportType.SelectedIndex==3)
        else if (ddlReportType.SelectedValue == "3")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
        }
        else
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;

            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

        }

    }
    protected void btnPrintreport_Click(object sender, EventArgs e)
    {
        int x = 0;
        // if (ddlReportType.SelectedIndex == 0)
        if (ddlReportType.SelectedValue == "1")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;


            int days = dtpTodate.SelectedDate.Value.Subtract(dtpfromdate.SelectedDate.Value).Days;
            if (days <= 2)
            {
                //RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=UnplannedSurgery" + "&ReportType=" + days;
                RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=UnplannedSurgery" + "&ReportType=1" ;
            }
            else
            {
                Alert.ShowAjaxMsg("Please select 2 days diff.", this.Page);
                return;
            }
        }
        // else if (ddlReportType.SelectedIndex == 1)
        else if (ddlReportType.SelectedValue == "0")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;
            x = 1;
            int days = dtpTodate.SelectedDate.Value.Subtract(dtpfromdate.SelectedDate.Value).Days;
            if (days <= 7)
            {
                //RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=UnplannedSurgery" + "&ReportType=" + days;
                RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=UnplannedSurgery" + "&ReportType=0";
            }
            else
            {
                Alert.ShowAjaxMsg("Please select only 7days diff.", this.Page);
                return;
            }
        }

        //    else if (ddlReportType.SelectedIndex == 2)
        else if (ddlReportType.SelectedValue == "2")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;
            RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=Abort";
        }
        //else
        //{
        //    RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=UnplannedSurgery" + "&ReportType=" + x;
        //}
        //   else if (ddlReportType.SelectedIndex == 3)
        else if (ddlReportType.SelectedValue == "3")
        {
            dtpfromdate.Enabled = true;
            dtpTodate.Enabled = true;
            RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=BallonTimeReport";
        }
        else
        {
            RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&RptName=OT&RptType=Register";
        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;

    }
}