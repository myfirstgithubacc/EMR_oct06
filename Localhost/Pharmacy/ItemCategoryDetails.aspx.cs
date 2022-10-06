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

using Telerik.Web.UI;

public partial class Pharmacy_ItemCategoryDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();

    bool RowSelStauts = false;
    bool RowSelStautsDetails = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myBool(Session["MainFacility"]))
            {
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ViewState["CategoryId"] = "0";
            ViewState["SubCategoryId"] = "0";
            ViewState["MParentId"] = "0";
            ViewState["ParentId"] = "0";
            bindItemCategoryMaster();
            bindDetailsData(false, "");

            lblAddCategory.Text = "Add New " + Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ItemCategory").ToString());
            ibtnPopup.ToolTip = "Add New " + Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ItemCategory").ToString());


            txtName.Attributes.Add("onblur", "nSat=1;");
            txtShortName.Attributes.Add("onblur", "nSat=1;");
        }
    }

    private void clearControl()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            txtName.Text = "";
            txtShortName.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtSearchValue.Text = "";

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindItemCategoryMaster()
    {
        try
        {
            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);

            ds = objPharmacy.getItemCategoryMaster(0, common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserID"]));

            gvInvoice.DataSource = ds.Tables[0];
            gvInvoice.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindDetailsData(bool BindStatus, string SearchValue)
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            DataSet ds = objPharmacy.getItemCategoryDetails(0, common.parseQManually(SearchValue), common.myInt(ViewState["CategoryId"]), common.myInt(Session["HospitalLocationId"]),
                                                    0, common.myInt(Session["UserID"]));

            if (common.myInt(ViewState["CategoryId"]) == 0)
            {
                RTLDetails.DataSource = ds.Tables[0].Clone();
            }
            else
            {
                RTLDetails.DataSource = ds.Tables[0];
            }
            if (!BindStatus)
            {
                RTLDetails.DataBind();
            }
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
        try
        {
            string strmsg = "";

            if (common.myInt(ViewState["CategoryId"]) == 0)
            {
                isSave = false;
                strmsg += Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ItemCategory").ToString()) + " not Selected !";
            }
            if (common.myLen(txtName.Text) == 0)
            {
                isSave = false;
                strmsg += Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ItemSubCategory").ToString()) + " can't be blank !";
            }
            if (common.myLen(txtShortName.Text) == 0)
            {
                isSave = false;
                strmsg += "Short Name can't be blank !";
            }
            if (strmsg != "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strmsg;
            }
            return isSave;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
            return false;
        }

    }

    protected void btnSave_Click(Object sender, EventArgs e)
    {
        if (!isSaved())
        {
            return;
        }
        try
        {
            int iSubCategoryID = 0;
            int iMainParentID = 0;
            int iParentID = 0;
            if (rblIsSubCategory.SelectedItem.Value == "1")
            {
                if (common.myInt(ViewState["MParentId"]) == 0)
                {
                    iMainParentID = common.myInt(ViewState["SubCategoryId"]);
                }
                else
                {
                    iMainParentID = common.myInt(ViewState["MParentId"]);
                }
                iParentID = common.myInt(ViewState["SubCategoryId"]);
            }
            else
            {
                iSubCategoryID = common.myInt(ViewState["SubCategoryId"]);
                iMainParentID = common.myInt(ViewState["MParentId"]);
                iParentID = common.myInt(ViewState["ParentId"]);
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);

            string strMsg = objPharmacy.SaveItemCategoryDetails(common.myInt(iSubCategoryID), common.myStr(txtName.Text, true), common.myStr(txtShortName.Text.ToUpper(), true),
                            common.myInt(ViewState["CategoryId"]), common.myInt(rblIsSubCategory.SelectedValue), common.myInt(iMainParentID), common.myInt(iParentID), common.myInt(Session["HospitalLocationId"]),
                            common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                if (rblIsSubCategory.SelectedValue == "0")
                    ViewState["SubCategoryId"] = "0";
                ViewState["MParentId"] = "0";
                ViewState["ParentId"] = "0";
                btnSave.Text = "Save";
                bindDetailsData(true, "");
                RTLDetails.SelectedItems.Clear();

                ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void gvInvoice_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        bindItemCategoryMaster();
    }

    protected void gvInvoice_PreRender(object sender, EventArgs e)
    {
        if (RowSelStauts == false && RowSelStautsDetails == false)
        {
            bindItemCategoryMaster();
        }
    }

    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                RowSelStauts = true;
                txtSearchValue.Text = "";
                ViewState["SubCategoryId"] = null;
                ViewState["MParentId"] = null;
                ViewState["ParentId"] = null;
                trIsSubCat.Visible = false;
                rblIsSubCategory.SelectedValue = "0";
                ViewState["CategoryId"] = common.myInt(e.CommandArgument);
                objPharmacy = new BaseC.clsPharmacy(sConString);
                DataSet ds = new DataSet();
                ds = objPharmacy.getItemCategoryMaster(common.myInt(e.CommandArgument), common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserID"]));
                clearControl();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblItemcategoryname.Text = common.myStr(ds.Tables[0].Rows[0]["ItemCategoryName"]);
                }
                bindDetailsData(false, "");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ibtnPopup_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/ItemCategoryMaster.aspx";
            RadWindowForNew.Height = 450;
            RadWindowForNew.Width = 600;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientFindClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void OnClientFindClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            ViewState["CategoryId"] = "0";
            ViewState["SubCategoryId"] = "0";
            ViewState["MParentId"] = "0";
            ViewState["ParentId"] = "0";
            clearControl();
            bindItemCategoryMaster();
            bindDetailsData(false, "");
            txtSearchValue.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public DataTable PopulateGridRadComboBoxCategoryType()
    {
        DataTable dt = new DataTable();
        try
        {
            BaseC.clsLISPhlebotomy objPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objPhlebotomy.getStatus(common.myInt(Session["HospitalLocationID"]), "ItemNature", "");

            ds.Tables[0].Columns["StatusId"].ColumnName = "CategoryTypeId";
            ds.Tables[0].Columns["Status"].ColumnName = "CategoryType";

            DataRow DR = ds.Tables[0].NewRow();
            DR["CategoryTypeId"] = "0";
            DR["CategoryType"] = "All";
            ds.Tables[0].Rows.InsertAt(DR, 0);

            dt = ds.Tables[0];
            return dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return dt;
        }
    }

    protected void gvInvoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void RTLDetails_OnItemCommand(object source, Telerik.Web.UI.TreeListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                RowSelStautsDetails = true;

                Label LBL = new Label();
                LBL = (Label)e.Item.FindControl("lblItemSubCategoryId");
                Label lblMainParentId = (Label)e.Item.FindControl("lblMainParentId");
                Label lblParentId = (Label)e.Item.FindControl("lblParentId");
                ViewState["SubCategoryId"] = common.myInt(LBL.Text);
                ViewState["MParentId"] = common.myInt(lblMainParentId.Text);
                ViewState["ParentId"] = common.myInt(lblParentId.Text);
                objPharmacy = new BaseC.clsPharmacy(sConString);

                DataSet ds = objPharmacy.getItemCategoryDetails(common.myInt(ViewState["SubCategoryId"]), "",
                                    common.myInt(ViewState["CategoryId"]), common.myInt(Session["HospitalLocationId"]),
                                    0, common.myInt(Session["UserID"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    clearControl();
                    DataRow DR = ds.Tables[0].Rows[0];
                    txtName.Text = common.myStr(DR["ItemSubCategoryName"]);
                    txtShortName.Text = common.myStr(DR["ItemSubCategoryShortName"]).ToUpper();
                    rblIsSubCategory.SelectedValue = "0";
                    ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";
                    trIsSubCat.Visible = true;
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

    protected void rblIsSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblIsSubCategory.SelectedItem.Value == "1")
        {
            txtName.Text = "";
            txtShortName.Text = "";
            ddlStatus.SelectedValue = "0";
            ddlStatus.SelectedValue = "1";
        }
        else
        {
            ViewState["SubCategoryId"] = null;
            ViewState["MParentId"] = null;
            ViewState["ParentId"] = null;
        }
    }

    protected void RTLDetails_PageIndexChanged(object source, Telerik.Web.UI.TreeListPageChangedEventArgs e)
    {
        try
        {
            RTLDetails.CurrentPageIndex = e.NewPageIndex;
            string SearchVal = "";
            if (txtSearchValue.Text != "")
            {
                SearchVal = txtSearchValue.Text.Trim();
            }
            bindDetailsData(true, SearchVal);
        }
        catch
        {
        }
    }

    protected void RTLDetails_NeedDataSource(object source, TreeListNeedDataSourceEventArgs e)
    {
        try
        {
            string SearchVal = "";
            if (txtSearchValue.Text != "")
            {
                SearchVal = txtSearchValue.Text.Trim();
            }
            bindDetailsData(true, SearchVal);
        }
        catch
        { }
    }

    protected void btnSearchField_OnClick(Object sender, EventArgs e)
    {
        bindDetailsData(false, common.myStr(txtSearchValue.Text).Trim());
    }
}