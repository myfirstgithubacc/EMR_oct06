using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_PatientApprovalList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetOrderApproval();
        }
        lblMessage.Visible = false;
    }

    public void GetOrderApproval()
    {
        string UHID = "";
        string PatientName = "";
        string BedNo = "";
        string OrderName = "";
        string Ward = "";

        if (ddlSearch.SelectedValue.ToUpper().Equals("ALL"))
        {
            txtsearchType.Text = "";
        }
        else if (ddlSearch.SelectedValue.ToUpper().Equals("UHID"))
        {
            UHID = txtsearchType.Text.Trim();
        }
        else if (ddlSearch.SelectedValue.ToUpper().Equals("PATIENTNAME"))
        {
            PatientName = txtsearchType.Text.Trim();
        }
        else if (ddlSearch.SelectedValue.ToUpper().Equals("BEDNO"))
        {
            BedNo = txtsearchType.Text.Trim();
        }

        else if (ddlSearch.SelectedValue.ToUpper().Equals("ORDERNAME"))
        {
            OrderName = txtsearchType.Text.Trim();
        }
        else if (ddlSearch.SelectedValue.ToUpper().Equals("WARD"))
        {
            Ward = txtsearchType.Text.Trim();
        }

        BaseC.WardManagement wm = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        ds = wm.GetEMRGetOrdersForApproval(common.myInt(Session["FacilityId"]), 0, common.myInt(ddlApproval.SelectedValue), UHID, PatientName, Ward, BedNo, OrderName);

        lblTotalRecord.Text = "( " + (common.myStr(ds.Tables[0].Rows.Count)) + " )";

        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        DataView dataView = dt.DefaultView;

        if (ddlOrderType.SelectedValue.ToUpper().Equals("ALL"))
        {
            gvOrderApproval.DataSource = ds.Tables[0];

            gvOrderApproval.DataBind();
        }
        else
        {
            dataView.RowFilter = "OrderType = '" + ddlOrderType.SelectedValue.ToUpper() + "'";
            gvOrderApproval.DataSource = dataView;

            gvOrderApproval.DataBind();
            lblTotalRecord.Text = "( " + (common.myStr(dataView.Count)) + " )";

        }
    }


    protected void gvOrderApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CheckBox headerchk = (CheckBox)gvOrderApproval.HeaderRow.FindControl("chkheader");
            //CheckBox childchk = (CheckBox)e.Row.FindControl("chkSelecct");
            //childchk.Attributes.Add("onclick", "javascript:Selectchildcheckboxes('" + headerchk.ClientID + "')");
        }
    }


    protected void ddlSearch_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlSearch.SelectedValue.ToUpper().Equals("ALL"))
        {
            txtsearchType.ReadOnly = true;
            txtsearchType.Text = "";
        }
        else if (ddlSearch.SelectedValue.ToUpper().Equals("UHID"))
        {
            txtsearchType.TextMode = TextBoxMode.Number;
            txtsearchType.ReadOnly = false;
            txtsearchType.Text = "";
        }
        else
        {
            txtsearchType.Enabled = true;
            txtsearchType.Text = "";
            txtsearchType.TextMode = TextBoxMode.SingleLine;
            txtsearchType.ReadOnly = false;
        }
    }


    protected void btnFilter_Click(object sender, EventArgs e)
    {
        GetOrderApproval();

    }

    protected void btnFilterReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("patientApprovalList.aspx");
    }

    protected void gvOrderApproval_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            Label LblOrderType = (Label)e.Item.FindControl("LblOrderType");
            ImageButton ibtnForNotes = (ImageButton)e.Item.FindControl("ibtnForNotes");
            if (LblOrderType.Text.ToUpper().Equals("CLINICAL NOTES"))
            {
                ibtnForNotes.Visible = true;
            }

        }
    }

    protected void gvOrderApproval_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvOrderApproval.CurrentPageIndex = e.NewPageIndex;
            GetOrderApproval();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void gvOrderApproval_PreRender(object sender, EventArgs e)
    {
        if (gvOrderApproval.SelectedItems.Count.Equals(0))
        {
            GetOrderApproval();
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {


        CheckBox headerCheckBox = (sender as CheckBox);
        foreach (GridDataItem dataItem in gvOrderApproval.MasterTableView.Items)
        {
            (dataItem.FindControl("chkrow") as CheckBox).Checked = headerCheckBox.Checked;
            dataItem.Selected = headerCheckBox.Checked;
        }
    }

    protected void chkrow_CheckedChanged(object sender, EventArgs e)
    {
        ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
        bool checkHeader = true;
        foreach (GridDataItem dataItem in gvOrderApproval.MasterTableView.Items)
        {
            if (!(dataItem.FindControl("chkrow") as CheckBox).Checked)
            {
                checkHeader = false;
                break;
            }
        }
        GridHeaderItem headerItem = gvOrderApproval.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        (headerItem.FindControl("chkAll") as CheckBox).Checked = checkHeader;
    }

    protected void ddlApproval_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    protected void ibtnForNotes_Click(object sender, ImageClickEventArgs e)
    {
    }

    protected void gvOrderApproval_EditCommand(object sender, GridCommandEventArgs e)
    {
        //        if (e.CommandName.ToUpper().Equals("EDIT"))
        //        {
        //            //GridDataItem item = (GridDataItem)gvOrderApproval.NamingContainer;
        //            ImageButton lnk = (ImageButton)e.Item.FindControl("ibtnForNotes");
        //            HiddenField hdRegistrationID = (HiddenField)e.Item.FindControl("hdnId");
        //            //Session["RegistrationID"] = hdRegistrationID.Value;
        //            //Session["EncounterId"] = lnk.AlternateText;

        //            string strPopup = "<script language='javascript' ID='script1'>"
        //+ "window.open('/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(lnk.CommandArgument) + "','new window', 'top=10, left=10, width=2000, height=630, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')" + "</script>";
        //            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        //        }
    }

    protected void gvOrderApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SEL"))
        {
            HiddenField hdnRegistrationId = (HiddenField)e.Item.FindControl("hdnRegistrationId");
            ImageButton lnk = (ImageButton)e.Item.FindControl("ibtnForNotes");
            Session["RegistrationID"] = hdnRegistrationId.Value;
            Session["EncounterId"] = lnk.AlternateText;
            string strPopup = "<script language='javascript' ID='script1'>"
+ "window.open('/Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(lnk.CommandArgument) + "','new window', 'top=10, left=10, width=2000, height=630, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')" + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
    }
}