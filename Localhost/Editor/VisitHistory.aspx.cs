using System;
using System.Web.UI;

public partial class Editor_VisitHistory : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["Category"]) == "PopUp")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Case Sheet " + common.myDate(Session["EncounterDate"]).ToString("dd/MM/yyyy");
        if (!IsPostBack)
        {
        }

    }

}




