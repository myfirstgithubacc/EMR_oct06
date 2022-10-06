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

public partial class Pharmacy_FindItemBrandMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridEncounter : byte
    {
        Select = 0,
        ItemId,
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

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

            objPharmacy = new BaseC.clsPharmacy(sConString);

            clearControl();
            bindData(false);

            txtItemName.Focus();
        }
    }


    private void clearControl()
    {
        try
        {
            txtItemName.Text = "";
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData(true);
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData(false);
    }

    protected void gvBrand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(GridEncounter.ItemId)].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(gvBrand.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text) > 0)
            {

                hdnItemID.Value = common.myStr(common.myInt(gvBrand.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                //Session["PharmacyItemBrandId"] = common.myInt(gvBrand.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text);

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                //return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindData(bool IsShowData)
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            int ItemId = 0;
            if (!IsShowData)
            {
                ItemId = -1;
            }

            DataSet dsSearch = new DataSet();
           // dsSearch = objPharmacy.getItemMaster(ItemId,"", common.myStr(txtItemBrandName.Text), 0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
            dsSearch = objPharmacy.getItemList(ItemId,0,txtItemName.Text, 0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), 0);

            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                gvBrand.DataSource = dsSearch.Tables[0];
            }
            else
            {
                DataRow DR = dsSearch.Tables[0].NewRow();
                dsSearch.Tables[0].Rows.Add(DR);

                gvBrand.DataSource = dsSearch.Tables[0];
            }

            gvBrand.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBrand.PageIndex = e.NewPageIndex;
        bindData(true);
    }
}
