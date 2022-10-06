using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_Masters_AddTreatmentPlan : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.EMROrders objEMR_Order = new BaseC.EMROrders(sConString);

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownServiceCategories();
            bindInvestigationSet();

            BindProvider();

            //  btnSave_Click(this, null);
        }
    }

    private void BindProvider()
    {
        try
        {
            ddlProvider.Items.Clear();
            ddlProvider.Text = string.Empty;
            
            DataSet objDs = new DataSet();
            if (common.myStr(ddlDepartment.SelectedValue) == string.Empty)
            {
                ddlDepartment.SelectedValue = "0";
            }

            objDs = objEMR_Order.fillDoctorCombo(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityID"]), 0);
            ddlProvider.DataSource = objDs.Tables[0];
            int i = objDs.Tables[0].Rows.Count;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();

            if (objDs.Tables[0].Rows.Count > 1)
            {
                ddlProvider.Enabled = true;
            }
            else
            {
                ddlProvider.Enabled = false;
            }
            

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    private void BindDropDownServiceCategories()
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = order.GetServiceCategories(Convert.ToInt32(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }
    protected void lnkEdit_OnClik(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
        Label lblSetName = (Label)row.FindControl("lblSetName");
        HiddenField hdnDepartmentId = (HiddenField)row.FindControl("hdnDepartmentId");
        HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");

        ddlDepartment.SelectedValue = hdnDepartmentId.Value;

        ddlProvider.SelectedValue = hdnDoctorId.Value;

        txtInvestigationSet.Text = lblSetName.Text;
        hdnSelectedSetId.Value = hdnSetId.Value;
    }
    protected void bindInvestigationSet()
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            ds = objEMR.GetEMRTreatmentPlanTemplates(common.myInt(Session["FacilityId"]),0);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        gvOrderSet.DataSource = dt;
                        gvOrderSet.DataBind();
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
        finally
        {
            objEMR = null;
            dt.Dispose();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            if (txtInvestigationSet.Text == "")
            {
                Alert.ShowAjaxMsg("Please type Order Set", Page);
                return;
            }
            string departmentId = common.myStr(ddlDepartment.SelectedValue) == "" ? null : common.myStr(ddlDepartment.SelectedValue);
            //strMsg = bHos.SaveEMRTreatmentPlanName(common.myInt(Session["FacilityId"]), common.myStr(txtstatus.Text.Trim()), common.myInt(ddlSpecialisation.SelectedValue), common.myInt(Session["UserId"]));
            //lblMsg.Text = strMsg;

            lblMessage.Text = bHos.SaveEMRTreatmentPlanMaster(common.myInt(hdnSelectedSetId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(departmentId), common.myStr(txtInvestigationSet.Text), common.myInt(Session["UserID"]), common.myInt(ddlProvider.SelectedValue));


            //(common.myInt(Session["FacilityId"]), common.myStr(txtInvestigationSet.Text.Trim()), common.myInt(ddlSpecialisation.SelectedValue), common.myInt(Session["UserId"]));

            //lblMessage.Text = order.SaveOrderSetName(common.myInt(hdnSelectedSetId.Value), common.myInt(Session["HospitalLocationID"]),
            //    txtInvestigationSet.Text, departmentId, common.myInt(Session["UserID"]), common.myInt(ddlProvider.SelectedValue));


            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);


            bindInvestigationSet();



            txtInvestigationSet.Text = "";
            hdnSelectedSetId.Value = "";
            ddlDepartment.SelectedIndex = -1;

            ddlDepartment.SelectedValue = "0";
            ddlDepartment.Text = "";
            ddlProvider.SelectedValue = "0";
            ddlProvider.Text = "";
           

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            order = null;
        }
    }
    protected void gvOrderSet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
                objEMR.DeleteTreatmentPlan(common.myInt(hdnSetId.Value));
                bindInvestigationSet();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }

    public DataSet GetInvestigationSetDetail(int iSetID)
    {
        DataSet objDs = new DataSet();
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            objDs = order.GetInvestigationSetDetail(iSetID);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return objDs;
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindProvider();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {

        }
    }
    //protected void btnClose_Click(object sender, EventArgs e)
    //{
    //    // Response.Redirect("~/EMR/Masters/EMRTreatmentPlan.aspx");
    //    //ScriptManager.RegisterClientScriptBlock(Page, 'GetType(Page)', "close", "window.close();", true )
    //   ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "close", "window.close();", true);
    //    Response.Redirect("~/EMR/Masters/EMRTreatmentPlan.aspx");

    //}




}