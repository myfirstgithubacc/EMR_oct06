using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

public partial class OTScheduler_OTAdmissionAlert : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsOTBooking objVal;
    BaseC.RestFulAPI objwcfOt;
    clsExceptionLog objException = new clsExceptionLog();
    int x = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getAdmissionOTAlert();
            PopulateOTName();
            bindWard();
        }

    }
    private void getAdmissionOTAlert()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        DataSet ds = new DataSet();
        objVal = new BaseC.clsOTBooking(sConString);
        int x = 0;
        string IP = string.Empty;
        if (ddlSearchOn.SelectedIndex == 0)
        {
            x = common.myInt(txtSearchNumeric.Text);
            IP = "";

        }
        else
        {
            x = 0;
            IP = txtSearch.Text;
        }


        if (chkSelect.Checked)
        {
            ds = objVal.GetOTAlertForAdmission(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnTheatreId.Value), common.myInt(hdnWardId.Value), x, IP, true);

        }
        else
        {
            ds = objVal.GetOTAlertForAdmission(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnTheatreId.Value), common.myInt(hdnWardId.Value), x, IP, false);

        }

        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {

            dt.Rows.Add(dt.NewRow());
            dt.AcceptChanges();
            gvOTAlertDetail.DataSource = dt;
            gvOTAlertDetail.DataBind();
        }
        else
        {
            gvOTAlertDetail.DataSource = dt;
            gvOTAlertDetail.DataBind();
        }

        // lblCount.Text = common.myStr(x);
    }



    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {


        getAdmissionOTAlert();


    }

    protected void gvOTAlertDetail_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvOTAlertDetail.CurrentPageIndex = e.NewPageIndex;
        getAdmissionOTAlert();
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (common.myInt(ddlSearchOn.SelectedValue).Equals(0))
            {
                txtSearchNumeric.Visible = true;
                txtSearch.Visible = false;
            }
            else
            {
                txtSearch.Visible = true;
                txtSearchNumeric.Visible = false;
            }
            txtSearchNumeric.Text = string.Empty;

            txtSearch.Text = string.Empty;

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    private void PopulateOTName()
    {
        objwcfOt = new BaseC.RestFulAPI(sConString);
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds = new DataSet();
            ds1 = objwcfOt.GetEmployeeId(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            int employeeId = 0;
            string empid = null;

            employeeId = common.myInt(empid);
            if (employeeId == 0)
            {

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);




                //Hashtable HashIn = new Hashtable();
                //HashIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                //HashIn.Add("@intFacilityID", ddlFacility.SelectedValue);
                //ds= dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HashIn);

                ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), employeeId);

                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    //  dv.RowFilter = "Active=1";

                    ddlOTSearch.DataSource = dv.ToTable();
                    ddlOTSearch.DataTextField = "TheatreName";
                    ddlOTSearch.DataValueField = "TheatreID";
                    ddlOTSearch.DataBind();
                    common.CheckAllItems(ddlOTSearch);

                    ddlOTSearch.Items.Insert(0, new RadComboBoxItem("ALL", "0"));
                    ddlOTSearch.SelectedIndex = 0;




                }
            }
            else
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {

                    empid = dr["Empid"].ToString();
                }
                ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), employeeId);
                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    // dv.RowFilter = "Active=1";

                    ddlOTSearch.DataSource = dv.ToTable();
                    ddlOTSearch.DataTextField = "TheatreName";
                    ddlOTSearch.DataValueField = "TheatreID";
                    ddlOTSearch.DataBind();
                    common.CheckAllItems(ddlOTSearch);

                    ddlOTSearch.Items.Insert(0, new RadComboBoxItem("ALL", "0"));
                    ddlOTSearch.SelectedIndex = 0;

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

    private void bindWard()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {


            ds = objadt.getWardMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("ALL", "0"));
            ddlWard.SelectedIndex = 0;


        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objadt = null; ds.Dispose(); }
    }

    protected void ddlWard_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        hdnWardId.Value = ddlWard.SelectedValue;
        getAdmissionOTAlert();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        getAdmissionOTAlert();
    }

    protected void ddlOTSearch_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        hdnTheatreId.Value = ddlOTSearch.SelectedValue;
        getAdmissionOTAlert();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        hdnTheatreId.Value = null;
        hdnWardId.Value = null;
        txtSearch.Text = "";
        txtSearchNumeric.Text = "";
        ddlOTSearch.SelectedIndex = 0;
        ddlSearchOn.SelectedIndex = 0;
        ddlWard.SelectedIndex = 0;
        chkSelect.Checked = true;
        getAdmissionOTAlert();
        
    }

    //protected void gvOTAlertDetail_CustomAggregate(object sender, GridCustomAggregateEventArgs e)
    //{
    //    if (((GridTemplateColumn)e.Column).UniqueName == "RegistrationNo")
    //    {
    //        int count = 0;
    //        foreach (GridDataItem item in gvOTAlertDetail.MasterTableView.Items)
    //        {
    //            if (item["RegistrationNo"].Text != null)
    //            {
    //                count++;//common.myInt(item["RegistrationNo"].Text);
    //            }
    //        }
    //        e.Result = count;
    //    }
    //}
}