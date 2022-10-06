using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Configuration;
using System.Collections;
public partial class MPages_DoctorNotesDetail : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMR objGeneric;
    SqlDataReader dr;
    DAL.DAL dl;
    Hashtable hshIn;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["CF"]) != null)
        {
            if (common.myStr(Request.QueryString["CF"]) == "DoctorNotes")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["PreviousNotes"]= common.myStr(Request.QueryString["Doctorid"]);
        if (!IsPostBack)
        {
           
            ShowGrid();
        }
    }

    protected void ShowGrid()
    {
        try
        {
            objGeneric = new BaseC.clsEMR(sConString);
            DataSet ds = objGeneric.DoctorPreviousNotes(common.myInt(ViewState["PreviousNotes"]));
            gvDoctorPreviousDetails.DataSource = ds;
            gvDoctorPreviousDetails.DataBind();
          
        }
        catch (Exception Ex)
        {
           // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }



}