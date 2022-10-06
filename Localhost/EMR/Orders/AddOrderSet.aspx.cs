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

public partial class EMR_Orders_AddOrderSet : System.Web.UI.Page
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

            if (Request.QueryString["For"].Equals("Orders"))
            {
                bindInvestigationSet();
            }
            if (Request.QueryString["For"].Equals("Medication"))
            {
                bindMedicationSet();
            }
            BindProvider();

            if (common.myInt(Request.QueryString["DoctorId"]) > 0)
            {
                ddlDepartment.Enabled = false;
            }

            if (Request.QueryString["For"] != null)
            {
                if (Request.QueryString["For"].Equals("Orders"))
                {
                    gvOrderSet.Visible = true;
                    gvMedication.Visible = false;
                    Label2.Text = "Order Set Name";
                    lblHeaderText.Text = "Order Set";
                }
                else if (Request.QueryString["For"].Equals("Medication"))
                {
                    gvOrderSet.Visible = false;
                    gvMedication.Visible = true;
                    Label2.Text = "Medication Set Name";
                    lblHeaderText.Text = "Medication Set";
                }
            }
            //  btnSave_Click(this, null);
        }
    }

    private void BindProvider()
    {
        DataSet objDs = new DataSet();
        DataView DV = new DataView();
        try
        {
            ddlProvider.Items.Clear();
            ddlProvider.ClearSelection();

            if (common.myStr(ddlDepartment.SelectedValue) == string.Empty)
            {
                ddlDepartment.SelectedValue = "0";
            }

            objDs = objEMR_Order.fillDoctorCombo(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityID"]), 0);

            DV = objDs.Tables[0].DefaultView;
            DV.Sort = "DoctorName";

            ddlProvider.DataSource = DV.ToTable();
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem("All"));
            ddlProvider.SelectedIndex = 0;

            ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.Items.FindItemByValue(common.myInt(Request.QueryString["DoctorId"]).ToString()));

            if (objDs.Tables[0].Rows.Count > 1)
            {
                ddlProvider.Enabled = true;
            }
            else
            {
                ddlProvider.Enabled = false;
            }

            if (common.myInt(Request.QueryString["DoctorId"]) > 0)
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
        finally
        {
            DV.Dispose();
            objDs.Dispose();
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
        //if (common.myInt(Request.QueryString["DoctorId"]) > 0)
        //{
        //    Alert.ShowAjaxMsg("Not allow to edit the record!", this.Page);
        //    return;
        //}

        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
        Label lblSetName = (Label)row.FindControl("lblSetName");
        HiddenField hdnDepartmentId = (HiddenField)row.FindControl("hdnDepartmentId");
        HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
        HiddenField hdnIsWardOrderSet = (HiddenField)row.FindControl("hdnIsWardOrderSet");

        ddlDepartment.SelectedValue = hdnDepartmentId.Value;

        ddlProvider.SelectedValue = hdnDoctorId.Value;

        txtInvestigationSet.Text = lblSetName.Text;
        hdnSelectedSetId.Value = hdnSetId.Value;
        chkIsWardOrderSet.Checked = false;
        if (common.myBool(hdnIsWardOrderSet.Value).Equals(true))
        {
            ddlDepartment.Enabled = false;
            ddlProvider.Enabled = false;
            chkIsWardOrderSet.Checked = true;
        }

    }
    protected void bindInvestigationSet()
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = objEMR.GetFavouriteInvestigationSet(common.myInt(Session["HospitalLocationID"]), common.myInt(Request.QueryString["DoctorId"]));

            if (dt.Rows.Count > 0)
            {
                gvOrderSet.DataSource = dt;
                gvOrderSet.DataBind();
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
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        Hashtable hshOutPut = new Hashtable();
        int savedSetId = 0;
        try
        {
            if (txtInvestigationSet.Text == "")
            {
                Alert.ShowAjaxMsg("Please type Order Set", Page);
                return;
            }
            string departmentId = common.myStr(ddlDepartment.SelectedValue) == "" ? null : common.myStr(ddlDepartment.SelectedValue);

            if (Request.QueryString["For"] != null)
            {
                if (Request.QueryString["For"].Equals("Orders"))
                {
                    int IsWardOrderSet = 0;
                    if (chkIsWardOrderSet.Checked)
                    {
                        departmentId = null;
                        ddlProvider.SelectedValue = null;
                        IsWardOrderSet = 1;
                    }
                    lblMessage.Text = order.SaveOrderSetName(common.myInt(hdnSelectedSetId.Value), common.myInt(Session["HospitalLocationID"]),
                                        txtInvestigationSet.Text, departmentId, common.myInt(Session["UserID"]),
                                        common.myInt(ddlProvider.SelectedValue), out savedSetId, IsWardOrderSet);

                    if (common.myInt(Request.QueryString["DoctorId"]) > 0 && savedSetId > 0 && common.myLen(Session["OrderSetServiceIds"]) > 0)
                    {

                        hshOutPut = order.SaveOrderSetDetails(savedSetId, common.myStr(Session["OrderSetServiceIds"]));

                        Session["OrderSetServiceIds"] = null;
                    }
                }
                else if (Request.QueryString["For"].Equals("Medication"))
                {
                    lblMessage.Text = order.SaveMedicationSetName(common.myInt(hdnSelectedSetId.Value), common.myInt(Session["HospitalLocationID"]),
                                        txtInvestigationSet.Text, departmentId, common.myInt(Session["UserID"]),
                                        common.myInt(ddlProvider.SelectedValue), out savedSetId);

                    if (common.myInt(Request.QueryString["DoctorId"]) > 0 && savedSetId > 0 && common.myLen(Session["MedicationSetItemIds"]) > 0)
                    {
                        hshOutPut = objEMR.SaveMedicationItemSet(common.myStr(Session["MedicationSetItemIds"]), savedSetId, common.myInt(Session["UserID"]));

                        Session["MedicationSetItemIds"] = null;
                    }
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            if (Request.QueryString["For"] != null)
            {
                if (Request.QueryString["For"].Equals("Orders"))
                {
                    bindInvestigationSet();
                }
                else if (Request.QueryString["For"].Equals("Medication"))
                {
                    bindMedicationSet();
                }
            }


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
            //if (common.myInt(Request.QueryString["DoctorId"]) > 0)
            //{
            //    Alert.ShowAjaxMsg("Not allow to delete the record!", this.Page);
            //    return;
            //}

            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
                objEMR.DeleteInvestigationSet(common.myInt(hdnSetId.Value));
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

    protected void gvMedication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        try
        {
            //if (common.myInt(Request.QueryString["DoctorId"]) > 0)
            //{
            //    Alert.ShowAjaxMsg("Not allow to delete the record!", this.Page);
            //    return;
            //}

            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnSetId = (HiddenField)row.FindControl("hdnSetId");
                objEMR.DeleteMedicationSet(common.myInt(hdnSetId.Value));
                bindMedicationSet();
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

    protected void bindMedicationSet()
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = objEMR.GetFavouriteMedicationSet(common.myInt(Session["HospitalLocationID"]), common.myInt(Request.QueryString["DoctorId"]));

            if (dt.Rows.Count > 0)
            {
                //gvOrderSet.DataSource = dt;
                //gvOrderSet.DataBind();
                gvMedication.DataSource = dt;
                gvMedication.DataBind();
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
    protected void lnkEditMedication_OnClik(object sender, EventArgs e)
    {
        //if (common.myInt(Request.QueryString["DoctorId"]) > 0)
        //{
        //    Alert.ShowAjaxMsg("Not allow to edit the record!", this.Page);
        //    return;
        //}

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

    protected void chkIsWardOrderSet_CheckedChanged(object sender, EventArgs e)
    {
        ddlProvider.Enabled = true;
        ddlDepartment.Enabled = true;
        if (chkIsWardOrderSet.Checked)
        {
            ddlProvider.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlProvider.Enabled = false;
            ddlDepartment.Enabled = false;
        }


    }
}
