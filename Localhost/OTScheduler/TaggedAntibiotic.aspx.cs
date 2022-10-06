using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class OTScheduler_TaggedAntibiotic : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateTaggedAntibiotic();
        }

    }
    private void populateTaggedAntibiotic()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

       

        ds = objEmr.getAntibiloticTaggedWithOTPatient(common.myInt(Request.QueryString["OTBookingId"]), common.myInt(Request.QueryString["serviceId"]));
        if (ds.Tables.Count > 0)
        {
            rlbListAntibiotic.DataTextField = "AntibioticName";
            rlbListAntibiotic.DataValueField = "AntibioticId";
            rlbListAntibiotic.DataSource = ds;
            rlbListAntibiotic.DataBind();
        }
    }
}