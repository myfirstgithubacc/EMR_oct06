using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

public partial class OTScheduler_ViewPtientdetails : System.Web.UI.Page
{
    BaseC.RestFulAPI objwcfot;//= new wcf_Service_OT.ServiceClient();
    DataSet ds = new DataSet();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objwcfot = new BaseC.RestFulAPI(sConString);
            if(Request.QueryString["BookingId"]!=null)
            {
                ds = objwcfot.GetOTPatientDetails(common.myInt(Request.QueryString["BookingId"]));
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
        }
    }
}
