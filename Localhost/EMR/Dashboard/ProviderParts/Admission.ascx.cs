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

public partial class EMR_Dashboard_ProviderParts_Admission : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillData();
        }
    }
    public void FillData()
    {
        BaseC.WardManagement Objstatus = new BaseC.WardManagement();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objds = Objstatus.GetAdmissionDetails(common.myInt(Session["HospitalLocationId"]),
                                             common.myInt(txtAdmission.Text) );
        if (objds.Tables[0].Rows.Count > 0)
        {
            gvAdmission.DataSource = objds;
            gvAdmission.DataBind();
        }
        else
        {
            gvAdmission.DataSource = null;
            gvAdmission.DataBind();
        }
    }
}
