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

public partial class EMR_Vitals_ViewDoctorVitalTemplate : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindVitalServiceGrid();
            BindDoctorGrid();
        }
    }

    private void BindDoctorGrid()
    {
        try
        {
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            DataSet ds = objEMRVitals.VitalTemplateDoctors(Convert.ToInt32(Request.QueryString["TempID"].ToString()));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvDoctors.DataSource = ds;
                        gvDoctors.DataBind();
                    }
                    else
                    {
                        gvDoctors.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindVitalServiceGrid()
    {
        try
        {
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            DataSet ds = objEMRVitals.GetVitalTemplateDetails(Convert.ToInt32(Request.QueryString["TempID"].ToString()), 0, 0);
            lbldescription.Text = ds.Tables[0].Rows[0]["MeasurementSystem"].ToString();
            //  lblMeasurement.Text = ds.Tables[0].Rows[0]["TemplateDescription"].ToString();

            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Active = 1";

            gvVitalServices.DataSource = dv;
            gvVitalServices.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctors_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            //   e.Row.Cells[0].Visible = false;
        }
    }

    protected void gvDoctors_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvDoctors.PageIndex = e.NewPageIndex;
        BindDoctorGrid();
    }

    protected void gvVitalServices_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void gvVitalServices_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvVitalServices.PageIndex = e.NewPageIndex;
        BindVitalServiceGrid();
    }
    protected void gvDoctors_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMRVitals objVitals = new BaseC.EMRVitals(sConString);
        try
        {
            if (e.CommandName.Equals("Del"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                int Id = common.myInt(((HiddenField)row.FindControl("hdnId")).Value);
                objVitals.DeActivateVitalTemplateDoctors(Id);
                BindDoctorGrid();
                lblMessage.Text = "Doctor De-activated successfully";

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

}
