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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;
using BaseC;
using System.Drawing;


public partial class Appointment_ResourceAppointment_ResourceAppointmentGView : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static bool status = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpfromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromDate.SelectedDate = DateTime.Now;
            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.SelectedDate = DateTime.Now;
            ddlrange.SelectedValue = "DD0";
            ddlrange_SelectedIndexChanged(sender, e);
            PopulateResourceName();
            ViewState["GridData"] = null;
            BindResourceGrid();
        }
    }
    protected void BindResourceGrid()
    {
        if (common.myInt(ddlResourceName.SelectedValue) > 0)
        {
            BaseC.Appointment app = new BaseC.Appointment(sConString);
            DataSet ds = new DataSet();
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            if (ViewState["GridData"] == null)
            {
                ds = app.uspGetBookingDetails(HospId, FacilityId, 0, common.myInt(ddlResourceName.SelectedValue), common.myDate(dtpfromDate.SelectedDate).ToString("yyyy/MM/dd"), common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["GridData"] = ds;
                }
            }
            gvResourceDtl.DataSource = (DataSet)ViewState["GridData"];
            gvResourceDtl.DataBind();

        }

    }
    private void PopulateResourceName()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hospital hs = new Hospital(sConString);
            DataSet ds = hs.GetResourceMaster(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), 1, 0, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["ResourceName"];
                item.Value = dr["ResourceId"].ToString();
                item.Attributes.Add("SubDeptId", common.myStr(dr["SubDeptId"]));
                ddlResourceName.Items.Add(item);
                item.DataBind();
            }
            ddlResourceName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;

        }
    }
    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrange.SelectedItem.Value.ToString() == "4")
        {
            dtpfromDate.SelectedDate = DateTime.Now.AddDays(-1);
            dtpToDate.SelectedDate = DateTime.Now;
            dtpfromDate.Enabled = true;
            dtpToDate.Enabled = true;
        }
        else
        {
            dtpfromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
            dtpfromDate.Enabled = false;
            dtpToDate.Enabled = false;
            dtpToDate.SelectedDate = DateTime.Now;
            if (ddlrange.SelectedValue.ToString() == "DD0")
                dtpfromDate.SelectedDate = DateTime.Now;
            else if (ddlrange.SelectedValue.ToString() == "WW-1")
                dtpfromDate.SelectedDate = DateTime.Now.AddDays(-7);
            else if (ddlrange.SelectedValue.ToString() == "WW-2")
                dtpfromDate.SelectedDate = DateTime.Now.AddDays(-14);
            else if (ddlrange.SelectedValue.ToString() == "MM0")
            {
                DateTime dtp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpfromDate.SelectedDate = dtp;
            }
            else if (ddlrange.SelectedValue.ToString() == "MM-1")
                dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-1);
            else if (ddlrange.SelectedValue.ToString() == "YY-1")
                dtpfromDate.SelectedDate = DateTime.Now.AddYears(-1);
            else if (ddlrange.SelectedValue.ToString() == "")
                dtpfromDate.SelectedDate = DateTime.Now.AddYears(-2);
        }

    }
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        ViewState["GridData"] = null;
        BindResourceGrid();
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        ddlResourceName.Text = "";
        ddlResourceName.SelectedIndex = -1;
        ddlrange.SelectedValue = "DD0";
        ddlrange_SelectedIndexChanged(sender, e);
    }
    protected void gvResourceDtl_PreRender(object sender, EventArgs e)
    {
        if (status == false)
        {
            BindResourceGrid();
        }

    }
    protected void gvResourceDtl_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            Label lblStatusColor = (Label)e.Item.FindControl("lblStatusColor");
            lblStatusColor.BackColor = System.Drawing.Color.FromName(common.myStr(((HiddenField)e.Item.FindControl("hdnStatusColor")).Value));
            lblStatusColor.ForeColor = System.Drawing.Color.FromName(common.myStr(((HiddenField)e.Item.FindControl("hdnStatusColor")).Value)); 
        }
    }
}
