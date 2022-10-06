using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;

public partial class WardManagement_VisitHistory : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["Category"]) == "PopUp")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            Page.Title = "Past Clinical Notes";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
            {
             //  btnClose.Visible = false;
            }

        }

    }
}
