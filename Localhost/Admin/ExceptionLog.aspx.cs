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
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class Admin_ExceptionLog : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet ds;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         

            dtpfromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            //ShowExceptionLogReport();
        }
    }

    protected void gvExceptionLog_PreRender(object sender, System.EventArgs e)
    {
        ShowExceptionLogReport();
    }
    protected void gvExceptionLog_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        ShowExceptionLogReport();
    }
    protected void gvExceptionLog_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ShowExceptionLogReport();
    }
    protected void gvExceptionLog_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        ShowExceptionLogReport();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ShowExceptionLogReport();
    }
    protected void btn_ClearFilter_Click(object sender, EventArgs e)
    {
        Clear();
        ShowExceptionLogReport();
    }

    protected void ShowExceptionLogReport()
    {
       BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

        try
        {
            Hashtable hshIn = new Hashtable();
            string sFromDate = "", sToDate = "";
            hshIn.Add("inyHospitalLocationId", common.myInt(ddlHospitalLocation.SelectedValue));
            hshIn.Add("chvSource", common.myStr(ddlSource.SelectedValue));
            hshIn.Add("chvUserIP", common.myStr(ddlIPAddress.SelectedValue));
            hshIn.Add("chvBrowser", "%" + common.myStr(ddlBrowser.SelectedValue) + "%");
            hshIn.Add("chvServer", common.myStr(ddlServer.SelectedValue));
            if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                //sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
                //sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";
                sFromDate = Convert.ToString(dtpfromDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                sToDate = Convert.ToString(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                hshIn.Add("@chrFromDate", sFromDate);
                hshIn.Add("@chrToDate", sToDate);
            }

            objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetExceptionLog", hshIn);
                
            gvExceptionLog.DataSource = ds;
            gvExceptionLog.DataBind();
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = ex.Message;
        }
        finally
        {
            objtlc = null;
        }
    }

    void Clear()
    {
        lbl_Msg.Text = "";
        ddlHospitalLocation.SelectedIndex = 0;
        ddlSource.SelectedIndex = 0;
        ddlIPAddress.SelectedIndex = 0;
        ddlServer.SelectedIndex = 0;
        ddlBrowser.SelectedIndex = 0;
        if (dtpfromDate.SelectedDate != null)
            dtpfromDate.SelectedDate = null;
        if (dtpToDate.SelectedDate != null)
            dtpToDate.SelectedDate = null;
    }

    protected void btn_Export_Click(object sender, EventArgs e)
    { 
        gvExceptionLog.ExportSettings.IgnorePaging = true;       
        gvExceptionLog.ExportSettings.OpenInNewWindow = true;
        //gvExceptionLog.MasterTableView.ExportToPdf();
        gvExceptionLog.MasterTableView.ExportToExcel();
    }
}
