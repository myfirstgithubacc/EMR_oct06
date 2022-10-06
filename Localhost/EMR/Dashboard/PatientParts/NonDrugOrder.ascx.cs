using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Collections;
using Telerik.Web.UI;

public partial class EMR_Dashboard_PatientParts_NonDrugOrder : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }
    void BindGrid()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objICM.NonDrugOrder(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(Session["UserId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

            if (ds.Tables.Count == 2)
            {
                ViewState["EmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]).Trim();
                gvNonDrugOrder.DataSource = ds;
                gvNonDrugOrder.DataBind();
            }
            else
            {
                ViewState["EmployeeType"] = "";
                gvNonDrugOrder.DataSource = null;
                gvNonDrugOrder.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            objICM = null;
            ds.Dispose();
        }
    }
    protected void gvNonDrugOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonDrugOrder.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    
}