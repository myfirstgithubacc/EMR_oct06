using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;


public partial class MPages_EmployeeDiscountSetup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindControl();
            bindgridview();
        }
    }
    private void bindControl()
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
        try
        {           
            /**********Facility************/
            ds = objlis.getFacilityList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]), common.myInt(Session["GroupId"]), common.myInt(Session["UserID"]));

            ddlFacility.DataValueField = "FacilityId";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataSource = ds;
            ddlFacility.DataBind();
            ddlFacility.SelectedIndex = 0;
            ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            if (ddlFacility.Items.Count > 0)
            {
                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
            }
            else
            {
                ddlFacility.SelectedIndex = 0;
            }


            /**********Level************/
            ds1 = baseEBill.GetApprovalMasterDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
            ddlLevel.DataSource = ds1;
            ddlLevel.DataValueField = "Id";
            ddlLevel.DataTextField = "LevelName";
            ddlLevel.DataBind();
            ddlLevel.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlLevel.SelectedIndex = 0;

            /**********Employee************/
            ds = objlis.getEmployeeData(common.myInt(Session["HospitalLocationID"]), 0, "", "", 0, 0);
            ddlEmployee.DataSource = ds.Tables[0].Copy();
            ddlEmployee.DataValueField = "EmployeeId";
            ddlEmployee.DataTextField = "EmployeeNameWithNo";
            ddlEmployee.DataBind();

            ddlEmployee.Items.Insert(0, new RadComboBoxItem("", "0"));
            
            ddlEmployee.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
            ds.Dispose();
            ds1.Dispose();
            baseEBill = null;
            objlis = null;
        }
    }

    private void bindgridview()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            ds = baseEBill.GetEmployeeApprovalDetail(common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvData.DataSource = ds;
                gvData.DataBind();
            }
        }
        catch (Exception Ex)
        {
            objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
            ds.Dispose();
            baseEBill = null;
        }
    }
    protected void gvData_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        bindgridview();
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void gvData_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (e.CommandName == "Select")
            {
                ViewState["ApprovalId"] = "0";
                HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnId");
                HiddenField hdnFacilityid = (HiddenField)e.Item.FindControl("hdnFacilityid");
                HiddenField hdnApprovalId = (HiddenField)e.Item.FindControl("hdnApprovalId");
                HiddenField hdnEmployeeId = (HiddenField)e.Item.FindControl("hdnEmployeeId");

                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(hdnFacilityid.Value)));
                ddlLevel.SelectedIndex = ddlLevel.Items.IndexOf(ddlLevel.Items.FindItemByValue(common.myStr(hdnApprovalId.Value)));
                ddlEmployee.SelectedIndex = ddlEmployee.Items.IndexOf(ddlEmployee.Items.FindItemByValue(common.myStr(hdnEmployeeId.Value)));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSaved())
            {
                return;
            }

            string strMsg = baseEBill.SaveEmployeeDiscountDetail(common.myInt(Session["FacilityId"]),
                common.myInt(Session["HospitalLocationID"]), common.myInt(ddlLevel.SelectedValue),
                common.myInt(ddlEmployee.SelectedValue), common.myInt(ddlStatus.SelectedValue),
                common.myInt(Session["UserID"]));
            bindgridview();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ddlFacility.SelectedIndex)==0)
        {
            strmsg += "Facility not selected !";
            isSave = false;
        }

        if (common.myInt(ddlLevel.SelectedIndex) == 0)
        {
            strmsg += "Level not selected !";
            isSave = false;
        }

        if (common.myInt(ddlEmployee.SelectedIndex) == 0)
        {
            strmsg += "Employee not selected !";
            isSave = false;
        }
        
        lblMessage.Text = strmsg;
        return isSave;
    }
}
