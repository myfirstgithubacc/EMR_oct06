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
using BaseC;

public partial class EMR_Dashboard_ProviderParts_Task : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DL_Funs fun = new DL_Funs();
    Hashtable hsTb = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (txtTasks.Text != "")
        {
            BindGVTask(Convert.ToInt32(txtTasks.Text.Trim()));
        }
        else
        {
            GVTask.DataSource = null;
            GVTask.DataBind();
        }
    }
    public void BindGVTask(Int32 EmpId)
    {
        try
        {
            if (EmpId != 0)
            {
                BaseC.Dashboard dsh = new BaseC.Dashboard();

                DataSet dsTask = new DataSet();
                DataTable dtNewTask = new DataTable();

                dsTask = dsh.getPDTask(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(EmpId), Convert.ToInt32(Session["FacilityID"]));
                DataView dvTask = new DataView(dsTask.Tables[0]);
                dvTask.RowFilter = "StatusId<>31";
                dtNewTask = dvTask.ToTable();
                GVTask.DataSource = dtNewTask;
                GVTask.DataBind();
            }
            else
            {
                GVTask.DataSource = null;
                GVTask.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GVTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVTask.PageIndex = e.NewPageIndex;
        BindGVTask(Convert.ToInt32(txtTasks.Text.Trim()));
    }

    protected void lnkName_Click(object sender, EventArgs e)
    {
        string strvalue;
        LinkButton lnkPatient1 = sender as LinkButton;

        strvalue = lnkPatient1.CommandArgument;

        string intTaskID = strvalue;

        RadWindowForNew.NavigateUrl = "~/Tasks/TaskPages.aspx?TaskID=" + intTaskID;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
}
