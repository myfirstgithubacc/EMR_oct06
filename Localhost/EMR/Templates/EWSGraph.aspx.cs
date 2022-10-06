using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_Templates_EWSGraph : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       if(!IsPostBack)
        {
            hdnTemplateId.Value=common.myStr(common.myInt( Request.QueryString["TemplateId"]));
        }
    }
    
}