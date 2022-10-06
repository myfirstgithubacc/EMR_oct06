using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class OTScheduler_ReportOTBooking : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

          
            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
        }

    }

    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&RptName=OT&RptType=Detail&Status=" +common.myStr(ddlStatus.SelectedValue) + "&IsActual=0";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;        
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
}
