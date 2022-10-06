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
using BaseC;

public partial class EMR_Dashboard_Parts_Tasks : WebPartBase
{
    clsExceptionLog objException = new clsExceptionLog();

    public EMR_Dashboard_Parts_Tasks()
    {
        this.Title = "Task";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {
            BindTasks();
            //lnkNotesRedirect.Enabled = true;
        }
        else
        {

            //lnkNotesRedirect.Enabled = false;
        }
    }

    private void BindTasks()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getTasks(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["registrationid"]), Convert.ToInt32(Session["FacilityID"]));
            gvTasks.DataSource = dsApp;
            gvTasks.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvTasks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTasks.PageIndex = e.NewPageIndex;
        BindTasks();
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
