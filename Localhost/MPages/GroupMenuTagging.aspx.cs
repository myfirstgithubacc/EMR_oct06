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
using System.Text;
using System.Drawing;
public partial class MPages_GroupMenuTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private bool RowSelStatus = false;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            ViewState["GroupId"] = common.myInt(Request.QueryString["GroupId"]);
            lblGroupName.Text = common.myStr(Request.QueryString["GroupName"]);
            BindModuleNames();   
        }
    }
    private void BindModuleNames()
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ddlModuleName.Items.Clear();
            ds = master.GetGroupModuleName();
            ddlModuleName.DataSource = ds.Tables[0];
            ddlModuleName.DataTextField = "ModuleName";
            ddlModuleName.DataValueField = "ModuleId";
            ddlModuleName.DataBind();
            RadComboBoxItem rci = new RadComboBoxItem("Select Module");
            ddlModuleName.Items.Insert(0, rci);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            master = null;
            ds.Dispose();
        }
    }
    protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (ddlModuleName.SelectedIndex > 0)
            {
                ddlModulePages.Items.Clear();
                ds = master.GetGroupPageName(common.myInt(ddlModuleName.SelectedValue));
                ddlModulePages.DataSource = ds.Tables[0];
                ddlModulePages.DataTextField = "PageCode";
                ddlModulePages.DataValueField = "PageCode";
                ddlModulePages.DataBind();
                RadComboBoxItem rci = new RadComboBoxItem("Select Page");
                ddlModulePages.Items.Insert(0, rci);
               
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Module First.";
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
            master = null;
            ds.Dispose();
        }
    }
    protected void ddlModulePages_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvFields.CurrentPageIndex = 0;
        gvFields.DataSource = BindBlankGrid();
        gvFields.DataBind();
        gvSelectedFields.DataSource = BindBlankGrid();
        gvSelectedFields.DataBind();
        ViewState["SelectedDepartment"] = null;
        if (ddlModulePages.SelectedIndex > 0)
        {
            bindFields();
            bindSelectedFields();
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Module Pages.";
        }
    }
    private void bindFields()
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = master.GetGroupOptionName(common.myInt(ddlModuleName.SelectedValue), common.myInt(ViewState["GroupId"]), common.myStr(ddlModulePages.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFields.DataSource = ds.Tables[0];
                gvFields.DataBind();
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
            master = null;
            ds.Dispose();
        }
    }

    private void bindSelectedFields()
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (ddlModulePages.SelectedIndex > 0)
            {
                ds = master.GetGroupOptionName(common.myInt(ddlModuleName.SelectedValue), common.myInt(ViewState["GroupId"]), common.myStr(ddlModulePages.SelectedValue));
                if (ds.Tables[1].Rows.Count > 0)
                {
                    gvSelectedFields.DataSource = ds.Tables[1];
                    gvSelectedFields.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[1].NewRow();
                    ds.Tables[1].Rows.Add(dr);
                    gvSelectedFields.DataSource = ds.Tables[1];
                    gvSelectedFields.DataBind();
                }
                ViewState["SelectedDepartment"] = ds.Tables[1];
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
            master = null;
            ds.Dispose();
        }
    }

    protected void gvFields_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        bindFields();
    }
    private DataTable BindBlankGrid()
    {

        DataTable objDt = new DataTable();
        objDt.Columns.Add("OptionId");
        objDt.Columns.Add("OptionName");
        objDt.Columns.Add("PageCode");
        DataRow dr = objDt.NewRow();
        dr["OptionId"] = "";
        dr["OptionName"] = "";
        dr["PageCode"] = "";
        objDt.Rows.Add(dr);
        return objDt;
    }
    protected void gvFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddToList")
            {
                lblMessage.Text = "&nbsp;";
                RowSelStatus = true;
                gvFields.Items[e.Item.ItemIndex].Selected = true;

                HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
                Label lblDepartmentName = (Label)e.Item.FindControl("lblDepartmentName");
                HiddenField hdnPageCode = (HiddenField)e.Item.FindControl("hdnPageCode");
                
                DataTable dt = (DataTable)ViewState["SelectedDepartment"];

                DataView DV = dt.Copy().DefaultView;

                DV.RowFilter = " OptionId = " + common.myInt(hdnDepartmentId.Value);

                if (DV.ToTable().Rows.Count > 0)
                {
                    lblMessage.Text = "Option Name '" + lblDepartmentName.Text + "' Already Exists !";
                }
                else
                {
                    DataRow DR = dt.NewRow();
                    DR["OptionId"] = common.myInt(hdnDepartmentId.Value);
                    DR["OptionName"] = common.myStr(lblDepartmentName.Text);
                    DR["PageCode"] = common.myStr(hdnPageCode.Value);
                    dt.Rows.Add(DR);

                    gvSelectedFields.DataSource = dt;
                    gvSelectedFields.DataBind();

                    ViewState["SelectedDepartment"] = dt;
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

    protected void gvFields_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            bindFields();
        }
    }

    protected void gvFields_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        bindFields();
    }

    protected void gvFields_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
    }

    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        try
        {
            if (common.myStr(ddlModuleName.SelectedValue) == "")
            {
                Alert.ShowAjaxMsg("Please select Module Name",Page);
                return;
            }

            if (common.myStr(ddlModulePages.SelectedValue) == "")
            {
                Alert.ShowAjaxMsg("Please select Page Name", Page);
                return;
            }

            string sResult = master.RemoveAllSecGroupMenuTagging(common.myInt(ViewState["GroupId"]), common.myInt(ddlModuleName.SelectedValue));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = sResult;
            bindSelectedFields();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            master = null;
        }
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        Hashtable hsout = new Hashtable();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(ViewState["GroupId"]) > 0 && gvSelectedFields.Items.Count != 0)
            {
                StringBuilder strXml = new StringBuilder();
                ArrayList coll = new ArrayList();

                foreach (Telerik.Web.UI.GridItem item in gvSelectedFields.Items)
                {
                    HiddenField hdnDepartmentId = (HiddenField)item.FindControl("hdnDepartmentId");
                    HiddenField hdnPageCode = (HiddenField)item.FindControl("hdnPageCode");
                    string a = hdnDepartmentId.Value.ToString();
                    if (common.myInt(hdnDepartmentId.Value) > 0)
                    {
                        coll.Add(hdnDepartmentId.Value);
                        coll.Add(hdnPageCode.Value);
                        strXml.Append(common.setXmlTable(ref coll));
                    }
                }
                if (strXml.ToString() != "")
                {
                    hsout = master.SaveSecGroupMenuTagging(common.myInt(ViewState["GroupId"]),common.myInt(ddlModuleName.SelectedValue), strXml.ToString(), common.myInt(Session["UserID"]));

                    //if (hsout["@chvErrorStatus"].ToString().Contains("Update") || hsout["@chvErrorStatus"].ToString().Contains("Save"))
                    //{
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        bindFields();
                        bindSelectedFields();
                        lblMessage.Text = hsout["@chvErrorStatus"].ToString();
                   // }
                }
                else
                {

                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record Not Saved...";
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
            master = null;
            hsout = null;
        }
    }

    protected void gvSelectedFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            int selectedIdx = e.Item.ItemIndex;

            if (e.CommandName == "Delete1")
            {
                DataTable dt = (DataTable)ViewState["SelectedDepartment"];

                gvSelectedFields.Items[selectedIdx].Selected = true;

                HiddenField hdnDepartmentId = (HiddenField)gvSelectedFields.SelectedItems[0].FindControl("hdnDepartmentId");

                DataView DV = dt.Copy().DefaultView;
                DV.RowFilter = " OptionId <> " + common.myInt(hdnDepartmentId.Value);

                gvSelectedFields.DataSource = DV.ToTable();
                gvSelectedFields.DataBind();

                ViewState["SelectedDepartment"] = DV.ToTable();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedFields_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            HiddenField hdnDepartmentId = (HiddenField)e.Item.FindControl("hdnDepartmentId");
            ImageButton imgBtn = (ImageButton)e.Item.FindControl("imgDelete");

            if (common.myInt(hdnDepartmentId.Value) < 1)
            {
                imgBtn.Visible = false;
            }
        }
    }

    
}
