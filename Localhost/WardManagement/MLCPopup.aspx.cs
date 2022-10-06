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
using Telerik.Web.UI;

public partial class WardManagement_MLCPopup : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["PageName"]) == "MLC")
            {
                lblHeader.Text = "MLC";
                BindMLCGrid();
            }
            else
            {
                lblHeader.Text = "Discharge Summary Pending";
                BindDischargeGrid();
            }

        }
    }

    void BindMLCGrid()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        int count = 0;
        try
        {
            ds = objwd.WardNotificationDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["WardId"]));
            if (ds.Tables[0].Rows.Count == 0)
            {
                count = 1;
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.AcceptChanges();
            }

            gvAdmission.DataSource = ds.Tables[0];
            gvAdmission.DataBind();

            //if (count == 0)
            //{
            //    lblNoOfDischarge.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            //}
            //else
            //{
            //    lblNoOfDischarge.Text = "(" + 0 + ")";
            //}
        }
        catch (Exception)
        {
        }
        finally
        {
            objwd = null;
            ds.Dispose();
        }
    }
    void BindDischargeGrid()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        try
        {
            int count = 0;

            ds = objwd.WardNotificationDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["WardId"]));
            if (ds.Tables[1].Rows.Count == 0)
            {
                count = 1;
                ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                ds.AcceptChanges();
            }

            gvAdmission.DataSource = ds.Tables[1];
            gvAdmission.DataBind();

            //if (count == 0)
            //{
            //    lblNoOfDischarge.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
            //}
            //else
            //{
            //    lblNoOfDischarge.Text = "(" + 0 + ")";
            //}
        }
        catch
        {
        }
        finally
        {
        }
    }
}
