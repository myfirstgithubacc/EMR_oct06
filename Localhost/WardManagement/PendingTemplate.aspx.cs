using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WardManagement_PendingTemplate : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindGridTemplateList();
        }
    }
    public void BindGridTemplateList()
    {
        DataSet ds = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        ds = objivf.getWardGetPendingTemplates(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 
                                            common.myInt(Session["RegistrationID"]), common.myInt(Request.QueryString["WardId"]));
        gvTemplateList.DataSource = ds.Tables[0];
        gvTemplateList.DataBind();

        lblMessage.Text =  "Total record(s) (" +common.myStr(ds.Tables[0].Rows.Count) + ")";
        lblMessage.ForeColor = System.Drawing.Color.Red;

    }
    protected void gvTemplateList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTemplateList.PageIndex = e.NewPageIndex;
        BindGridTemplateList();
    }
}