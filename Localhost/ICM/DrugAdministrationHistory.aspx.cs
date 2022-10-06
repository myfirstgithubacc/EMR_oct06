using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using Telerik.Web.UI;
using System.IO;


public partial class ICM_DrugAdministrationHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.ICM obj;
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["Master"]) == "No")
        {
            this.MasterPageFile ="~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    void BindData()
    {
        try
        {
            obj = new BaseC.ICM(sConString);
            ds = new DataSet();

            ds = obj.getDrugAdministratorHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["EncId"]), common.myInt(Request.QueryString["RegId"]));
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].AcceptChanges();
            }
            gvDrugAdministrator.DataSource = ds;
            gvDrugAdministrator.DataBind();
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void gvDrugAdministrator_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDrugAdministrator.PageIndex = e.NewPageIndex;
        BindData();
    }
   
}
