using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

public partial class Pharmacy_SaleIssue_ViewDocument : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    BaseC.clsEMRBilling baseEBill;
    StringBuilder objXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    bool RowSelStauts = false;

    private enum GridItemDetails : byte
    {
        Select = 0,
        ReturnNo = 1,
        ReturnDate = 2,
        IssueNo = 3,
        IssueDate = 4,
        RegNo = 5,
        EncounterNo = 6,
        CurrentBedNo = 7,
        PatientName = 8,
        NetAmount = 9,
        EncodedBy = 10,
        EncodedDate = 11,
        StoreName = 12,
        IssueId = 13,
        Id = 14
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objPharmacy = new BaseC.clsPharmacy(sConString);

        if (!IsPostBack)
        {
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);

            hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myInt(hdnDecimalPlaces.Value) == 0)
            {
                hdnDecimalPlaces.Value = "2";
            }
            lblStoreName.Text = common.myStr(Request.QueryString["StoreName"]); 
            ViewState["UseFor"] = common.myStr(Request.QueryString["UseFor"]);
            ViewState["OPIP"] = common.myStr(Request.QueryString["OPIP"]);

            if (common.myStr(ViewState["OPIP"]) == "O")
            {
                if (common.myStr(ViewState["UseFor"]) == "R")
                {
                    if (common.myInt(Request.QueryString["ReturnRequest"]).Equals(1))
                    {
                        ViewState["ReturnRequest"] = true;
                    }

                    lblIssueFromDate.Text = lblIssueFromDate.Text.Replace("Issue", "Return");
                    lblIssueToDate.Text = lblIssueToDate.Text.Replace("Issue", "Return");
                }
                else if (common.myStr(ViewState["UseFor"]) == "I")
                {
                    ddlSearchOn.Items.Remove(0);
                }
                if (common.myStr(ViewState["UseFor"]) == "S")
                {
                    lblIssueFromDate.Text = lblIssueFromDate.Text.Replace("Issue", "Sale");
                    lblIssueToDate.Text = lblIssueToDate.Text.Replace("Issue", "Sale");
                }
            }
            if (common.myStr(ViewState["OPIP"]) == "I")
            {
                if (common.myStr(ViewState["UseFor"]) == "R")
                {
                    ddlSearchOn.Items.Remove(1);
                    lblIssueFromDate.Text = lblIssueFromDate.Text.Replace("Issue", "Return");
                    lblIssueToDate.Text = lblIssueToDate.Text.Replace("Issue", "Return");
                }
                else if (common.myStr(ViewState["UseFor"]) == "I")
                {
                    ddlSearchOn.Items.Remove(0);
                }
            }
            if (common.myStr(ViewState["UseFor"]) == "R")
            {
                ddlStatus.SelectedValue = "O";
            }
            else
            {
                ddlStatus.SelectedValue = "P";
            }
            clearControl();
            if (Convert.ToString(Request.QueryString["DocNo"]) != "")
            {
                ViewState["DocNo"] = common.myStr(Request.QueryString["DocNo"]);
                ViewState["SetupId"] = Request.QueryString["SetupId"].ToString().Trim();
            }
            ViewState["SetupId"] = Request.QueryString["SetupId"].ToString().Trim();
            txtFromDate.MaxDate = System.DateTime.Now;
            bindData();
            hdnDocXmlString.Value = "";

            if (common.myBool(ViewState["ReturnRequest"]))
            {
                ddlSearchOn.SelectedValue = "4";
                ddlSearchOn.Enabled = false;
                ddlStatus.SelectedValue = "P";
                ddlStatus.Enabled = false;
            }
        }
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }

    protected void bindData()
    {
        try
        {
            txtDocNo.Visible = true;
            txtRegNo.Visible = false;

            DataSet ds = new DataSet();
            objPharmacy = new BaseC.clsPharmacy(sConString);

            string IssueNo = "";
            string ReturnNo = "";
            string RegNo = "";
            string PatientName = "";
            string EncounterNo = "";
            string CurrentBedNo = "", status = "";

            switch (common.myInt(ddlSearchOn.SelectedValue))
            {
                case 1: // ReturnNo
                    ReturnNo = common.myStr(txtDocNo.Text);
                    break;
                case 2: // IssueNo
                    IssueNo = common.myStr(txtDocNo.Text);
                    break;
                case 3: // RegNo
                    txtDocNo.Visible = false;
                    txtRegNo.Visible = true;
                    RegNo = common.myStr(txtRegNo.Text);
                    break;
                case 4: // PatientName
                    PatientName = common.myStr(txtDocNo.Text);
                    break;
                case 5: // Encounter No
                    EncounterNo = common.myStr(txtDocNo.Text);
                    break;
                case 6: // Current bed No
                    CurrentBedNo = common.myStr(txtDocNo.Text);
                    break;
                case 7: // status
                    status = common.myStr(txtDocNo.Text);
                    break;
            }

            string strOPIP = common.myStr(ViewState["OPIP"]);
            string StoreId = common.myStr(Request.QueryString["StoreId"]);//common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
            if (common.myStr(Request.QueryString["ED"]) == "1" && StoreId != common.myStr(Session["StoreId"]))
            {
                StoreId = "0";
            }
            else
            {
                if (common.myInt(Session["StoreId"]) == 0 && StoreId == "0")
                {
                    StoreId = common.myStr(Session["StoreId"]);
                }
            }
            if (common.myBool(ViewState["ReturnRequest"]))
            {
                BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);

                ds = objP.getReturnDump(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(RegNo), Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"), "%" + PatientName + "%", 0);
            }
            else
            {
                ds = objPharmacy.GetDocumentDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(StoreId),
                    common.myInt(ViewState["SetupId"]), IssueNo, ReturnNo, RegNo, PatientName, EncounterNo, CurrentBedNo,
                    common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate), common.myStr(ViewState["UseFor"]),
                    common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]), strOPIP, ddlStatus.SelectedValue);
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
                if (common.myStr(ViewState["UseFor"]) == "R")
                {
                    lblTotalReturn.Text = "Total " + common.myStr(ddlStatus.SelectedItem.Text) + " Return " + "(" + 0 + ")";
                }
                else if (common.myStr(ViewState["UseFor"]) == "I")
                {
                    lblTotalReturn.Text = "Total " + common.myStr(ddlStatus.SelectedItem.Text) + " Issue " + "(" + 0 + ")";
                }
            }
            else
            {
                //  lblTotalReturn.Text = "Total " + common.myStr(ddlStatus.SelectedItem.Text) + " + UseFor + " + "(" + common.myInt(ds.Tables[0].Rows.Count) + ")";
                if (common.myStr(ViewState["UseFor"]) == "R")
                {
                    lblTotalReturn.Text = "Total " + common.myStr(ddlStatus.SelectedItem.Text) + " Return " + "(" + common.myInt(ds.Tables[0].Rows.Count) + ")";
                }
                else if (common.myStr(ViewState["UseFor"]) == "I")
                {
                    lblTotalReturn.Text = "Total " + common.myStr(ddlStatus.SelectedItem.Text) + " Issue " + "(" + common.myInt(ds.Tables[0].Rows.Count) + ")";
                }
            }

            gvItemDetails.DataSource = ds.Tables[0];
            gvItemDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvItemDetails_PreRender(object sender, EventArgs e)
    {
        if (!RowSelStauts)
        {
            bindData();
        }
    }

    protected void gvItemDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sDocNo = "";
            if (common.myInt(gvItemDetails.SelectedRow.Cells[Convert.ToByte(GridItemDetails.IssueId)].Text) > 0)
            {
                Label lblOrderEnrty = (Label)gvItemDetails.Rows[gvItemDetails.SelectedIndex].FindControl("lblOrderEnrty");
                //if (common.myInt(lblOrderEnrty.Text) == 0)
                //{
                Label lblIssueNo = (Label)gvItemDetails.SelectedRow.FindControl("lblIssueNo");
                Label lblReturnNo = (Label)gvItemDetails.SelectedRow.FindControl("lblReturnNo");
                HiddenField hdnInvoiceId = (HiddenField)gvItemDetails.SelectedRow.FindControl("hdnInvoiceId");

                if (common.myStr(ViewState["UseFor"]) == "R" && common.myStr(ViewState["OPIP"]) == "O")
                {
                    sDocNo = lblReturnNo.Text;
                }
                else
                {
                    sDocNo = lblIssueNo.Text;
                }

                hdnDocXmlString.Value = sDocNo;
                hdnIssueId.Value = common.myInt(gvItemDetails.SelectedRow.Cells[Convert.ToByte(GridItemDetails.IssueId)].Text).ToString();
                hdnGlobleInvoiceId.Value = common.myInt(hdnInvoiceId.Value).ToString();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;

                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txtnetamount = (Label)e.Row.FindControl("txtnetamount");
                Label lblProcessStatus = (Label)e.Row.FindControl("lblProcessStatus");


                txtnetamount.Text = common.myDbl(txtnetamount.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                if (common.myStr(lblProcessStatus.Text) == "P")
                {
                    lblProcessStatus.Text = "Post";
                }
                else if (common.myStr(lblProcessStatus.Text) == "B")
                {
                    lblProcessStatus.Text = "Billed";
                }
                else if (common.myStr(lblProcessStatus.Text) == "U")
                {
                    lblProcessStatus.Text = "Un Billed";
                }
                else if (common.myStr(lblProcessStatus.Text) == "T")
                {
                    lblProcessStatus.Text = "Transfer to IP";
                }
                else if (common.myStr(lblProcessStatus.Text) == "C")
                {
                    lblProcessStatus.Text = "Cancel";
                }
                else
                {
                    lblProcessStatus.Text = "Open";
                }
                Label lblReturnNo = (Label)e.Row.FindControl("lblReturnNo");
                Label lblReturnDate = (Label)e.Row.FindControl("lblReturnDate");
                Label lblIssueNo = (Label)e.Row.FindControl("lblIssueNo");
                Label lblIssueDate = (Label)e.Row.FindControl("lblIssueDate");
                if (common.myStr(ViewState["OPIP"]) == "I")
                {
                    if (common.myStr(ViewState["UseFor"]) == "R")
                    {
                        lblReturnNo.Text = common.myStr(lblIssueNo.Text);
                        lblReturnDate.Text = common.myStr(lblIssueDate.Text);
                    }


                }
                else if (common.myStr(ViewState["OPIP"]) == "O")
                {
                    if (common.myStr(ViewState["UseFor"]) == "R")
                    {
                        if (!common.myBool(ViewState["ReturnRequest"]))
                        {
                            lblReturnNo.Text = common.myStr(lblIssueNo.Text);
                            lblReturnDate.Text = common.myStr(lblIssueDate.Text);
                        }
                    }


                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (common.myStr(ViewState["OPIP"]) == "I")
                {
                    gvItemDetails.Columns[6].Visible = true;
                    gvItemDetails.Columns[7].Visible = true;
                }
                else if (common.myStr(ViewState["OPIP"]) == "O")
                {
                    gvItemDetails.Columns[6].Visible = false;
                    gvItemDetails.Columns[7].Visible = false;
                }
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(GridItemDetails.StoreName)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridItemDetails.IssueId)].Visible = false;
                //e.Row.Cells[Convert.ToByte(GridItemDetails.SaleSetupId)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridItemDetails.Id)].Visible = false;
                //e.Row.Cells[Convert.ToByte(GridItemDetails.StoreId)].Visible = false;

                if (common.myStr(ViewState["OPIP"]) == "I")
                {
                    if (common.myStr(ViewState["UseFor"]) == "I")
                    {
                        e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnNo)].Visible = false;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnDate)].Visible = false;
                    }
                    else if (common.myStr(ViewState["UseFor"]) == "R")
                    {
                        e.Row.Cells[Convert.ToByte(GridItemDetails.IssueNo)].Visible = false;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.IssueDate)].Visible = false;
                    }
                }
                else if (common.myStr(ViewState["OPIP"]) == "O")
                {
                    if (common.myStr(ViewState["UseFor"]) == "I")
                    {
                        e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnNo)].Visible = false;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.ReturnDate)].Visible = false;
                    }
                    else if (common.myStr(ViewState["UseFor"]) == "R")
                    {
                        e.Row.Cells[Convert.ToByte(GridItemDetails.IssueNo)].Visible = false;
                        e.Row.Cells[Convert.ToByte(GridItemDetails.IssueDate)].Visible = false;
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
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        gvItemDetails.PageIndex = 0;
        bindData();
    }

    protected void btnClearSearch_OnClick(object sender, EventArgs e)
    {
        clearControl();
        bindData();
    }

    private void clearControl()
    {
        ddlSearchOn.SelectedIndex = 0;
        txtDocNo.Text = "";

        lblMessage.Text = "&nbsp;";

        txtFromDate.SelectedDate = DateTime.Now.AddDays(-2);
        txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        txtToDate.SelectedDate = DateTime.Now;
        txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void gvItemDetails_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvItemDetails.PageIndex = e.NewPageIndex;
        bindData();
    }

}
