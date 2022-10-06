using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class OTScheduler_OTOrderList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dvConfirmCancel.Visible = false;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            dtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtFromDate.SelectedDate = DateTime.Now;
            dtToDate.SelectedDate = DateTime.Now;
            btnFilter_OnClick(this, null);
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        int FilterType = 1; // For OT Order Grid (Top)
        int OrderId = 0;
        ShowOrderGrid(FilterType, OrderId);
        lblMessage.Text = "";
        gvService.DataSource = null;
        gvService.DataBind();
    }

    private void ShowOrderGrid(int FilterType, int OrderId)
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.RestFulAPI objOT = new BaseC.RestFulAPI(sConString);
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            string fDate = dtFromDate.SelectedDate.Value.ToString("yyyy/MM/dd"); //yyyy-MM-dd
            string tDate = dtToDate.SelectedDate.Value.ToString("yyyy/MM/dd"); //yyyy-MM-dd

            ds = objOT.GetOTOrderList(HospId, FacilityId, fDate, tDate, FilterType, OrderId);
            if (FilterType == 1)
            {
                gvOTOrder.DataSource = ds.Tables[0];
                gvOTOrder.DataBind();
            }
            else
            {
                gvService.DataSource = ds.Tables[0];
                gvService.DataBind();
            }            
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void gvOTOrder_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvOTOrder.CurrentPageIndex = e.NewPageIndex;
        int FilterType = 1; // For OT Order Grid (Top)
        int OrderId = 0;
        ShowOrderGrid(FilterType, OrderId);
        gvService.DataSource = null;
        gvService.DataBind();
        //BlankGrid();
    }
    protected void gvOTOrder_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                //ViewState["Selected"] = "1";
                lblMessage.Text = "";
                Label lblOrderId = (Label)e.Item.FindControl("lblOrderId");
                int FilterType = 2; // for Service Grid
                ShowOrderGrid(FilterType, common.myInt(lblOrderId.Text));


            }
            //    else if (e.CommandName == "PatientDetails")
            //    {
            //        LinkButton lnkPatientName = (LinkButton)e.Item.FindControl("lnkPatientName");
            //        Label lblFacilityName = (Label)e.Item.FindControl("lblFacilityName");
            //        Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
            //        if (lblRegistrationNo.Text != "")
            //        {
            //            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PatientDetails.aspx?RegNo=" + common.myStr(lblRegistrationNo.Text) + "&PName=" + lnkPatientName.Text + "&facility=" + common.myStr(lblFacilityName.Text);

            //            RadWindowForNew.Height = 300;
            //            RadWindowForNew.Width = 900;
            //            RadWindowForNew.Top = 10;
            //            RadWindowForNew.Left = 10;
            //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //            RadWindowForNew.Modal = true;
            //            RadWindowForNew.VisibleStatusbar = false;

            //        }
            //        else
            //        {
            //            Alert.ShowAjaxMsg("MR# No. Not Exist", Page);
            //        }

            //    }
            //    else
            //    {
            //        ViewState["LabNo"] = null;
            //        bindTestDetailsData();

            //    }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvOTOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");

            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Label lblInvoiceNo = (Label)e.Item.FindControl("lblInvoiceNo");
            Label lblStatus = (Label)e.Item.FindControl("lblStatus");
            if (common.myStr(lblInvoiceNo.Text) != "" || common.myStr(lblStatus.Text) == "InActive")
            {
                GridDataItem di = e.Item as GridDataItem;
                TableCell cell = di["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];
                CHK.Visible = false;
            }
        }

    }

    protected void gvService_ItemDataBound(object sender, GridItemEventArgs e)
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
    protected void btnCancelOrder_OnClick(object sender, EventArgs e)
    {
        int OrderId = 0;
        int EncounterId = 0;
        foreach (GridDataItem dataItem in gvOTOrder.SelectedItems)
        {
            TableCell cell = dataItem["chkCollection"];
            CheckBox CHK = (CheckBox)cell.Controls[0];

            if (CHK.Checked == true && CHK.Visible == true)
            {
                Label lblOrderId = (Label)dataItem.FindControl("lblOrderId");
                HiddenField hdnEncounterId = (HiddenField)dataItem.FindControl("hdnEncounterId");

                if (lblOrderId.Text != "")
                {
                    OrderId = common.myInt(lblOrderId.Text);
                    EncounterId = common.myInt(hdnEncounterId.Value);
                }
            }
        }
        if (OrderId == 0)
        {
            return;
        }
        else
        {
            dvConfirmCancel.Visible = true;
        }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        CancelOrder();
        dvConfirmCancel.Visible = false;
        int FilterType = 1; // For OT Order Grid (Top) 
        int OrderId = 0;
        ShowOrderGrid(FilterType, OrderId);
        gvService.DataSource = null;
        gvService.DataBind();
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        dvConfirmCancel.Visible = false;
    }
    void CancelOrder()
    {
        try
        {
            int OrderId = 0;
            int EncounterId = 0;
            foreach (GridDataItem dataItem in gvOTOrder.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label lblOrderId = (Label)dataItem.FindControl("lblOrderId");
                    HiddenField hdnEncounterId = (HiddenField)dataItem.FindControl("hdnEncounterId");

                    if (lblOrderId.Text != "")
                    {
                        OrderId = common.myInt(lblOrderId.Text);
                        EncounterId = common.myInt(hdnEncounterId.Value);
                    }
                }
            }
            if (OrderId == 0)
            {
                return;
            }
            //System.Collections.Generic.Dictionary<object, object> htOut = new System.Collections.Generic.Dictionary<object, object>();
            BaseC.RestFulAPI objOT = new BaseC.RestFulAPI(sConString);
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int UserId = common.myInt(Session["UserId"]);
            string Remarks = "";

            string result = objOT.CancelOTOrder(HospId, FacilityId, EncounterId, OrderId, Remarks, UserId);
            if (result.Contains("successfully"))
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            else
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = result;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message;
            objException.HandleException(ex);
        }
    }
}
