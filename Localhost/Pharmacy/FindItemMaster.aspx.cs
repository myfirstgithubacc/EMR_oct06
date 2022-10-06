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

public partial class Pharmacy_FindItemMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridEncounter : byte
    {
        Select = 0,
        ItemId,
        ItemNo
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

            bindControl();
           // gvItem.PageIndex = 0;
            bindData(false);
            txtName.Focus();
        }
    }

    private void bindControl()
    {
        try
        {
            //BaseC.clsLISPhlebotomy objPh = new BaseC.clsLISPhlebotomy(sConString);
            //DataSet ds = objPh.getStatus(common.myInt(Session["HospitalLocationID"]), "CreditNote", "");

            //ddlDocType.DataSource = ds.Tables[0];
            //ddlDocType.DataTextField = "Status";
            //ddlDocType.DataValueField = "Code";
            //ddlDocType.DataBind();

            //ddlDocType.Items.Insert(0, new RadComboBoxItem("", ""));

            //ddlDocType.SelectedIndex = 0;
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
        try
        {
            txtName.Text = "";
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
        gvItem.PageIndex = 0;
        bindData(true);
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData(false);
    }

    protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager && e.Row.RowType !=DataControlRowType.EmptyDataRow)
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

    protected void gvItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(gvItem.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text) > 0)
            {
                hdnItemID.Value = common.myStr(common.myInt(gvItem.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);

                //Session["PharmacyItemId"] = common.myInt(gvItem.SelectedRow.Cells[Convert.ToByte(GridEncounter.ItemId)].Text);

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
            dsSearch = objPharmacy.getItemList(ItemId,  0, common.myStr(txtName.Text), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

            gvItem.DataSource = dsSearch.Tables[0];
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
               
            }
            else
            {
                //DataRow DR = dsSearch.Tables[0].NewRow();
                //dsSearch.Tables[0].Rows.Add(DR);

                //gvItem.DataSource = dsSearch.Tables[0];
                

            }
           gvItem.DataBind();
            txtName.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvItem_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItem.PageIndex = e.NewPageIndex;
        bindData(true);
    }
}