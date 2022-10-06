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

public partial class Pharmacy_ItemCategoryMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();
    bool RowSelStauts = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myBool(Session["MainFacility"]))
            {
                btnSaveData.Visible = true;
            }
            else
            {
                btnSaveData.Visible = false;
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            this.Title = common.myStr(GetGlobalResourceObject("PRegistration", "ItemCategory"));

            objPharmacy = new BaseC.clsPharmacy(sConString);

            ViewState["_ID"] = "0";

            bindControl();

            bindDetailsData();
        }
    }

    private void bindControl()
    {
        try
        {
            BaseC.clsLISPhlebotomy objPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objPhlebotomy.getStatus(common.myInt(Session["HospitalLocationID"]), "ItemNature", "");

            ddlCategoryType.DataSource = ds.Tables[0];
            ddlCategoryType.DataTextField = "Status";
            ddlCategoryType.DataValueField = "StatusId";
            ddlCategoryType.DataBind();

            ddlCategoryType.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlCategoryType.SelectedIndex = 0;
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

        if (common.myLen(txtCategoryName.Text) == 0)
        {
            strmsg += Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ItemCategory").ToString()) + " can't be blank ! <br />";
            isSave = false;
        }
        if (common.myLen(txtShortName.Text) == 0)
        {
            strmsg += "Short Name can't be blank ! <br />";
            isSave = false;
        }
        if (ddlCategoryType.SelectedIndex == 0)
        {
            strmsg += "Item Nature not Selected !";
            isSave = false;
        }
        if (common.myInt(ViewState["_ID"]) == 0
            && common.myInt(ddlStatus.SelectedValue) != 1)
        {
            strmsg += "Status must be Active for New Data ! <br />";
            isSave = false;
        }

        if (strmsg != "")
        {
            lblMessage.Text = strmsg;
        }
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);

            string strMsg = objPharmacy.SaveItemCategoryMaster(common.myInt(ViewState["_ID"]), common.myStr(txtCategoryName.Text, true),
                        common.myStr(txtShortName.Text, true), common.myInt(ddlCategoryType.SelectedValue), common.myInt(Session["HospitalLocationID"]),
                        common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindDetailsData();
                ViewState["_ID"] = "0";
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

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        ddlCategoryType.SelectedIndex = 0;
        txtCategoryName.Text = "";
        txtShortName.Text = "";
        ddlStatus.SelectedIndex = 0;

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    private void bindDetailsData()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getItemCategoryMaster(0, common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["UserID"]));

            gvDetails.DataSource = ds.Tables[0];

            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        bindDetailsData();
    }

    protected void gvDetails_PreRender(object sender, EventArgs e)
    {
        if (RowSelStauts)
        {
            bindDetailsData();
        }
    }

    protected void gvDetails_OnSelectedIndexChanged(object source, EventArgs e)
    {
        try
        {
            ViewState["_ID"] = "0";
            if (gvDetails.SelectedIndexes.Count < 1)
            {
                return;
            }

            clearControl();

            Label LBL = new Label();
            int Id = 0;

            LBL = (Label)gvDetails.SelectedItems[0].FindControl("lblItemCategoryId");

            if (LBL.Text != "")
            {
                Id = common.myInt(LBL.Text);
            }
            ViewState["_ID"] = common.myInt(Id);

            objPharmacy = new BaseC.clsPharmacy(sConString);

            DataSet ds = objPharmacy.getItemCategoryMaster(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];

                txtCategoryName.Text = common.myStr(DR["ItemCategoryName"]);
                txtShortName.Text = common.myStr(DR["ItemCategoryShortName"]);
                ddlCategoryType.SelectedIndex = ddlCategoryType.Items.IndexOf(ddlCategoryType.Items.FindItemByValue(common.myStr(DR["CategoryTypeId"])));

                ddlStatus.SelectedValue = (common.myBool(DR["Active"]) == true) ? "1" : "0";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvDetails_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFilteringItem)
        {
            //GridFilteringItem item = e.Item as GridFilteringItem;
            //item.Height = Unit.Pixel(0);

            //TextBox TXT = (TextBox)item["SampleName"].Controls[0];
            ////TXT.Font.Size = 7;
            //TXT.Height = Unit.Pixel(15);

            //TextBox TXTSampleUnit = (TextBox)item["UnitName"].Controls[0];
            ////TXTSampleUnit.Font.Size = 7;
            //TXTSampleUnit.Height = Unit.Pixel(15);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void imgBtnItemNature_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/Pharmacy/ItemNatureMaster.aspx";
            RadWindowForNew.Height = 390;
            RadWindowForNew.Width = 420;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
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

    protected void btnGetStatus_Click(object sender, EventArgs e)
    {
        bindControl();
    }
}