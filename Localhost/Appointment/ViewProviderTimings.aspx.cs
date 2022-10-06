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
using Telerik.Web.UI;

public partial class Appointment_ViewProviderTimings : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    BaseC.ParseData bc = new BaseC.ParseData();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateFacility();
            PopulateProvider();
            if (ddlProvider.Items.Count > 0)
            {
                ddlProvider_SelectedIndexChanged(this, null);
            }
        }
    }

    private void populateFacility()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationID"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active = 1 ";
            ddlFacility.DataSource = dv;
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataValueField = "FacilityId";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlFacility.Items[0].Value = "0";
            //ListItem lstFacility = (ListItem)ddlFacility.Items.FindByValue(Session["FacilityId"].ToString());
            //if (lstFacility != null)
            //{
            //    lstFacility.Selected = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateProvider()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@intFacilityId ", ddlFacility.SelectedValue);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);
            ddlProvider.Items.Clear();
            if (objDs.Tables[0].Rows.Count > 0)
            {

                ddlProvider.DataSource = objDs;
                ddlProvider.DataValueField = "DoctorId";
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataBind();
                ddlProvider.Items.Insert(0, new RadComboBoxItem("Select Doctor", "0"));
                ddlProvider.Items[0].Value = "0";
            }
            if (Request.QueryString["Mpg"] == "P781")
            {
                ddlProvider.Enabled = false;
            }
            else
            {
                ddlProvider.Enabled = true;
            }
            ddlProvider_SelectedIndexChanged(this, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlFacility_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          
            if (ddlProvider.SelectedValue != "")
            {
                BindGrid();
            }
            else
            {
                BindBlankGrid();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindGrid()
    {
        
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (ddlProvider.SelectedValue != "")
            {
                hshInput.Add("@intDoctorID", Convert.ToInt32(ddlProvider.SelectedValue));
                hshInput.Add("@intFacilityID", Convert.ToInt32(ddlFacility.SelectedValue));

                DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspShowDoctorAppTimings", hshInput);

                if (objDs.Tables[0].Rows.Count > 0)
                {
                    gvProviderTiming.DataSource = objDs;
                    gvProviderTiming.DataBind();

                }
                else
                {
                    BindBlankGrid();
                }
            }
    }

    private void BindBlankGrid()
    {
        try
        {
            DataTable dT = new DataTable();
            dT.Columns.Add("FacilityName");
            dT.Columns.Add("DateFrom");
            dT.Columns.Add("DateTo");
            dT.Columns.Add("Day");
            dT.Columns.Add("FromTime");
            dT.Columns.Add("ToTime");
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["FacilityName"] = DBNull.Value;
                dr["DateFrom"] = DBNull.Value;
                dr["DateTo"] = DBNull.Value;
                dr["Day"] = DBNull.Value;
                dr["FromTime"] = DBNull.Value;
                dr["ToTime"] = DBNull.Value;
                dT.Rows.Add(dr);
            }
            gvProviderTiming.DataSource = dT;
            gvProviderTiming.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
