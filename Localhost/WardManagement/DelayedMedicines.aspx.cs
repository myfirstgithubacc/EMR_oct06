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
using System.Drawing;
using System.IO;
using System.Xml;
using Telerik.Web.UI;

public partial class WardManagement_Delayed_Medicines : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }
    public  void BindGrid()
    {
        BaseC.WardManagement objW = new BaseC.WardManagement();
        ds= objW.EMRGetNotifications(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvEMRWardDashBoardDetails.DataSource = ds.Tables[0];
                    gvEMRWardDashBoardDetails.DataBind();
                }
                else
                {
                    BlankGrid();
                }
            }
            else
            {
                BlankGrid();
            }
        }

    }
    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("FrequencyTime");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("WardName");
        dt.Columns.Add("BedNo");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("ItemName");

        DataRow dr = null;
        dr = dt.NewRow();
        //dr["id"] = "1";
        dt.Rows.Add(dr);

        gvEMRWardDashBoardDetails.DataSource = dt;
        gvEMRWardDashBoardDetails.DataBind();
    }
}