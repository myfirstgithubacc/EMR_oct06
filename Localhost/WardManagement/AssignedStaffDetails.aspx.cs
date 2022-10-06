using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseC;
using System.Collections;

public partial class WardManagement_AssignedStaffDetails : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindAssinedStaff();
            if (Request.QueryString["EncounterId"] != string.Empty)
                BindDetails(common.myInt(Request.QueryString["EncounterId"]));
        }
    }
    private void BindAssinedStaff()
    {
        ICM objICM = new ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objICM.GetICMNurse(common.myInt(Session["FacilityId"]));
            ddlAssignedStaff.DataSource = ds.Tables[0];
            ddlAssignedStaff.DataTextField = "NurseName";
            ddlAssignedStaff.DataValueField = "Id";

            ddlAssignedStaff.DataBind();
            ddlAssignedStaff.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select", "0"));
            ddlAssignedStaff.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objICM = null;
            ds.Dispose();
        }



    }
    private void BindDetails(int EncounterId)
    {
        WardManagement objWard = new WardManagement();
        DataTable dt = new DataTable();
        try
        {
            dt = objWard.GetEMRPatientNurseTagging(EncounterId);
            if (dt.Rows.Count > 0)
            {
                txtRemarks.Text = common.myStr(dt.Rows[0]["Remarks"]);
                ddlAssignedStaff.SelectedIndex = ddlAssignedStaff.Items.IndexOf(ddlAssignedStaff.Items.FindItemByValue(common.myStr(dt.Rows[0]["NurseId"])));
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objWard = null;
            dt.Dispose();
        }
    }
    private void clearControls()
    {
        txtRemarks.Text = string.Empty;
        ddlAssignedStaff.SelectedIndex = 0;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        WardManagement objWard = new WardManagement();
        try
        {
            if (common.myInt(ddlAssignedStaff.SelectedValue) > 0)
            {
                lblMessage.Text = objWard.SaveUpdatePatientNurseTagging(common.myInt(Request.QueryString["EncounterId"]), common.myInt(ddlAssignedStaff.SelectedValue), txtRemarks.Text, common.myInt(Session["EmployeeId"]));
                lblMessage.ForeColor = System.Drawing.Color.Green;
                clearControls();
            }
            else
            {
                lblMessage.Text = "Please select assigned staff and enter remarks";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objWard = null;
        }


    }
}