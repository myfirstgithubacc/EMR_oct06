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

public partial class OTScheduler_ReportCancelOTBooking : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
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

            GetTheatreData();
        }

    }

    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        string TheatreId = "0";
        if (ddlTheatre.SelectedValue.ToUpper().ToString().Equals(""))
        {
            TheatreId = "0";
        }
        else
        {
            TheatreId = ddlTheatre.SelectedValue.ToString();
        }
        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&RptName=OT&RptType=CancelOTBooking&Theater=" + common.myStr(TheatreId);
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

    protected void GetTheatreData()
    {
        try
        {

            DataSet ds = new DataSet();
            BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            if (ds.Tables.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                //  dv.RowFilter = "Active=1";

                ddlTheatre.DataSource = dv.ToTable();
                ddlTheatre.DataTextField = "TheatreName";
                ddlTheatre.DataValueField = "TheatreID";
                ddlTheatre.DataBind();
                common.CheckAllItems(ddlTheatre);
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
