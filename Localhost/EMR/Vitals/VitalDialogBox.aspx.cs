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

public partial class EMR_Vitals_VitalDialogBox : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strValueID = Request.QueryString["ValueID"].ToString();
            string strImages = "select ImageName,ImagePath from EMRVitalValues where ValueID=" + strValueID + "";
            DataSet ds = dl.FillDataSet(CommandType.Text, strImages);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Image1.ImageUrl = ds.Tables[0].Rows[0]["ImagePath"].ToString();
                lblMessage.Visible = false;
            }
            else
            {
                Image1.Visible = false;
                lblMessage.Text = "No Image for this Vital";
            }
            strValueID = null;
        }
    }
}
