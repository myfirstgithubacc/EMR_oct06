using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections;
using System.Text;
using System.IO;

public partial class EMR_Problems_CheifComplainHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {

                GetCheifComplainHistory();

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }




    protected void GetCheifComplainHistory()
    {
        BaseC.EMRProblems EMRprob = new BaseC.EMRProblems(sConString);
        DataSet ds = new DataSet();
        try
        {


             ds = EMRprob.GetEMRCheifComplainHistory(common.myInt(Session["RegistrationId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvProblemDetails.DataSource = ds.Tables[0];
                gvProblemDetails.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
                gvProblemDetails.DataSource = ds.Tables[0];
                gvProblemDetails.DataBind();
                dr = null;

            }



        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            EMRprob = null;
        
        }
    }
   



    
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
       

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParentPage();", true);
        return;

    }


    


   
}
