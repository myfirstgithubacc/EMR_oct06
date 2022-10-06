using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;

public partial class EMR_WindowsApplicationDetails : System.Web.UI.Page
{
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();


    public void HiddenItem()
    {
        foreach (ListItem item in this.rdoResultView.Items)
        {
            if (item.Text == "Horizontal View")
            {
                item.Attributes.CssStyle.Add("visibility", "hidden");
                // Or you can try to use
                // item.Attributes.CssStyle.Add("display", "none");
            }
            if (item.Text == "Grid View")
            {
                item.Attributes.CssStyle.Add("visibility", "hidden");
                // Or you can try to use
                // item.Attributes.CssStyle.Add("display", "none");
            }

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if(ChkInvestigation.SelectedValue!=string.Empty )
        //{ 
        //ChkInvestigation.SelectedValue = common.myStr(ViewState["ChkInvestigationSelectedValue"]);
        //if (ChkInvestigation.SelectedValue != string.Empty)
        //{
        //    ChkInvestigation.Items.FindByValue(common.myStr(ViewState["ChkInvestigationSelectedValue"])).Selected = true;
        //}
        //}
        BindGrid();

        if (Request.QueryString["FlagDepatment"] != null)
        {
            if (common.myStr(Request.QueryString["FlagDepatment"]) != string.Empty)
            {
                if (Request.QueryString["FlagDepatment"] == "XR")
                {
                    // rdoResultView.Items[0].Enabled = false;


                    HiddenItem();
                    rdoResultView.SelectedValue = "YA";
                    rdoResultView_OnSelectedIndexChanged(null, null);
                }
               
            }
        }

        if (!IsPostBack)
        {

            if (Request.QueryString["FromEMR"] != null)
            {
                if (common.myStr(Request.QueryString["FromEMR"]) != string.Empty)
                {
                    if (common.myStr(Request.QueryString["FromEMR"]).Equals("1"))
                    {
                        asplHeaderUDEMR.Visible = true;
                        asplHeaderUD.Visible = false;
                    }
                    else
                    {
                        asplHeaderUDEMR.Visible = false;
                        asplHeaderUD.Visible = true;
                    }

                }
                else
                {
                    asplHeaderUDEMR.Visible = false;
                    asplHeaderUD.Visible = true;
                }

            }
            else
            {
                asplHeaderUDEMR.Visible = false;
                asplHeaderUD.Visible = true;
            }

            rdoInvestigations.SelectedValue = "0";
            txtFromDate.SelectedDate = DateTime.Now.AddMonths(-6);
            txtToDate.SelectedDate = DateTime.Now;

            BindCheckBox();
            BindChkrdoInvestigations();
            BindGrid();
            
        }

 }


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]).ToUpper() == "BLANK")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }

    }
    protected void gvDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        String itemSelected = String.Empty;
        foreach (ListItem item in ChkInvestigation.Items)
        {
            if (item.Selected)
                itemSelected = itemSelected + item.Value + ",";

        }
        itemSelected = itemSelected.TrimEnd(',');
        ViewState["ChkInvestigationSelected"] = itemSelected;

        BindGrid();
        BindCheckBox();
        BindChkrdoInvestigations();


        if (ViewState["ChkInvestigationSelected"] != null)
        {
            string[] bookItems = ViewState["ChkInvestigationSelected"].ToString().Split(',');
            foreach (string bookId in bookItems)
            {
                ListItem item = ChkInvestigation.Items.FindByValue(bookId);
                if (item != null)
                    item.Selected = true;
            }
        }

        ViewState["ChkInvestigationSelected"] = null;
    }
    public void BindGrid()
    {
        lblMessage.Text = string.Empty;
        BaseC.clsEMR objwd = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        if (Session["ChkInvestigationSelected"] != null)
        {
            string[] bookItems = Session["ChkInvestigationSelected"].ToString().Split(',');
            foreach (string bookId in bookItems)
            {
                ListItem item = ChkInvestigation.Items.FindByValue(bookId);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }
        StringBuilder sbServiceCodes = new StringBuilder();

        string regno = common.myStr(Session["RegistrationNo"]);
        try
        {

            if (common.myInt(rdoInvestigations.SelectedValue).Equals(1))
            {
                foreach (ListItem item in ChkInvestigation.Items)
                {
                    if (item.Selected)
                    {
                        if (sbServiceCodes.ToString() != "")
                        {
                            sbServiceCodes.Append(",");
                        }
                        sbServiceCodes.Append(item.Value);
                    }
                }
            }
           // Request.QueryString["FlagDepatment"] == "XR"
            if (Request.QueryString ["FlagDepatment"]!=null )
            {
                if(common.myStr(Request.QueryString["FlagDepatment"])!=string.Empty )
                {
                    if(Request.QueryString["FlagDepatment"] == "XR")
                    {
                        ds = objwd.GetWindowPaitentInvestigationResult(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                        common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                        sbServiceCodes.ToString(), common.myStr(rdoResultView.SelectedValue), "XR");
                    }
                    else
                    {
                        ds = objwd.GetWindowPaitentInvestigationResult(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                                    common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                                    sbServiceCodes.ToString(), common.myStr(rdoResultView.SelectedValue), string.Empty);
                    }

                }
            }
            else
            {
                ds = objwd.GetWindowPaitentInvestigationResult(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                            common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                            sbServiceCodes.ToString(), common.myStr(rdoResultView.SelectedValue), string.Empty);

                //ds = objwd.GetWindowPaitentInvestigationResult(common.myInt(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                //                                            common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                //                                            sbServiceCodes.ToString(), common.myStr(rdoResultView.SelectedValue),string.Empty );
            }

            //ds = objwd.GetWindowPaitentInvestigationResult(115275, common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
            //                                            common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
            //                                            sbServiceCodes.ToString(), common.myStr(rdoResultView.SelectedValue));

            ViewState["dsInvestigationDetails"] = ds;

            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            if (common.myStr(rdoResultView.SelectedValue).Equals("XA"))
            {
                gvLabDetailsXaxis.DataSource = ds.Tables[0];
                gvLabDetailsXaxis.DataBind();

                //setLinkControl(ds.Tables[0]);
            }
            else
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwd = null;
            ds.Dispose();
        }
    }


    private void setLinkControl(DataTable tbl)
    {
        try
        {
            if (common.myStr(rdoResultView.SelectedValue).Equals("XA"))
            {
                //0-InvestigationName,1-Report_type

                string strReport_type = string.Empty;
                string strInv_Result = string.Empty;

                for (int rIdx = 0; rIdx < tbl.Rows.Count; rIdx++)
                {
                    for (int cIdx = 0; cIdx < tbl.Columns.Count; cIdx++)
                    {
                        if (cIdx > 1)
                        {
                            strReport_type = common.myStr(gvLabDetailsXaxis.Rows[rIdx].Cells[1].Text).Replace("&nbsp;", "");
                            strInv_Result = common.myStr(gvLabDetailsXaxis.Rows[rIdx].Cells[cIdx].Text).Replace("&nbsp;", "");

                            if (common.myLen(strInv_Result) > 0)
                            {
                                if (common.myStr(strReport_type).Equals("T"))
                                {
                                    LinkButton lnk = (LinkButton)gvLabDetailsXaxis.Rows[rIdx].Cells[cIdx].FindControl("lnk" + rIdx.ToString() + cIdx.ToString());

                                    if (lnk != null)
                                    {
                                        if (common.myLen(lnk.Text) > 0)
                                        {
                                            lnk.Text = "Text Report";
                                            lnk.ToolTip = "Click to show results";
                                           // Session["Inv_Result"] = strInv_Result;
                                           lnk.Attributes.Add("onclick", "showResultPopup(event,'" + strInv_Result + "');");
                                            //hdnShowInv_Result.Value = strInv_Result;
                                            //btnShowInv_Result_OnClick(null, null);
                                            //Alert.ShowAjaxMsg(strInv_Result, this);
                                            //Alert.ShowAjaxMsg(common.myInt(e.Row.RowIndex).ToString() + i.ToString(), this);
                                            //  lnk.Attributes.Add("onclick", "window.open('../Editor/WordProcessorInvestigationResult.aspx', 'Window1', 'menubar=no,width=800,height=600,toolbar=no'); ");
                                            //  lnk.Attributes.Add("onclick", "window.radopen('../Editor/WordProcessorInvestigationResult.aspx', 'Window1'); ");


                                            //e.Row.Cells[i].ForeColor = System.Drawing.Color.Transparent;
                                            //e.Row.Cells[i].Controls.Add(lnk);
                                        }
                                    }
                                }

                                //else
                                //{
                                //    Label lbl = new Label();
                                //    if (common.myLen(strInv_Result) > 0)
                                //    {
                                //        if (common.myLen(strReferenceRange) > 0)
                                //        {
                                //            lbl.Text = strInv_Result + " (" + common.myStr(strReferenceRange).Trim() + ")";
                                //        }
                                //        else
                                //        {
                                //            lbl.Text = strInv_Result;
                                //        }
                                //    }
                                //    e.Row.Cells[i].Controls.Add(lbl);
                                //}

                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
    }

    public void BindCheckBox()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objwd = new BaseC.clsEMR(sConString);
        try
        {
            /* START Binding CheckboxList */
            ChkInvestigation.Items.Clear();

            if (Request.QueryString["FlagDepatment"] != null)
            {
                if (common.myStr(Request.QueryString["FlagDepatment"]) != string.Empty)
                {
                    if (Request.QueryString["FlagDepatment"] == "XR")
                    {
                        ds = objwd.GetOPIP_Investigation_Report_Status(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"), common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),"XR");

                    }
                    else
                    {
                        ds = objwd.GetOPIP_Investigation_Report_Status(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"), common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"),string.Empty );

                    }

                }
            }
            else
            {
                ds = objwd.GetOPIP_Investigation_Report_Status(common.myLong(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"), common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"), string.Empty);

            }




            // ds = objwd.GetOPIP_Investigation_Report_Status(common.myInt(Session["RegistrationNo"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"), common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"));
            // ds = objwd.GetOPIP_Investigation_Report_Status(common.myInt(115275), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"), common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"));








            //DataView dv = ds.Tables[0].DefaultView;
            //DataView dvSelected = new DataView(dv.ToTable());

            //dvSelected.RowFilter = "Result1 is not null";

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = common.myStr(ds.Tables[0].Rows[i]["InvestigationName"]);
                        item.Value = common.myStr(ds.Tables[0].Rows[i]["service_code"]);
                        //item.Selected = Convert.ToBoolean(sdr["IsSelected"]);
                        ChkInvestigation.Items.Add(item);
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            if (ds != null)
            {
                ds.Dispose();
            }
        }


    }

    protected void gvDetails_OnItemDataBound(object source, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
            {
                Label lblInv_Result = (Label)e.Item.FindControl("lblInv_Result");
                LinkButton lnkInv_Result = (LinkButton)e.Item.FindControl("lnkInv_Result");

                HiddenField hdnReport_type = (HiddenField)e.Item.FindControl("hdnReport_type");

                if (common.myStr(hdnReport_type.Value).Equals("T"))
                {
                    lnkInv_Result.Visible = true;
                    lblInv_Result.Visible = false;
                    lnkInv_Result.Text = "Text Report";
                    lnkInv_Result.ToolTip = "Click to show results";
                    //ViewState["gvDetailsItemIndex"] = e.Item.ItemIndex;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }

    protected void lnkInv_Result_click(object sender, EventArgs e)
    {

        try
        {

            //Telerik.Web.UI.GridDataItem Row = (Telerik.Web.UI.GridDataItem)chkIsActive.NamingContainer;

            GridDataItem row = (GridDataItem)(((LinkButton)sender).NamingContainer);
            string lblInv_Result = ((Label)row.FindControl("lblInv_Result")).Text;
            //HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");
            Session["Inv_Result"] = common.myStr(lblInv_Result);

            //  RadWindow3.NavigateUrl = "~/Editor/WordProcessorInvestigationResult.aspx?Inv_Result="+common.myStr(lblInv_Result);
            RadWindow3.NavigateUrl = "~/Editor/WordProcessorInvestigationResult.aspx";

            RadWindow3.Height = 500;
            RadWindow3.Width = 1100;
            RadWindow3.Top = 20;
            RadWindow3.Left = 20;
            // RadWindowForNew.Title = "Time Slot";
            //RadWindow3.OnClientClose = "OnClientClose";
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    protected void btnRefreshData_Click(object sender, EventArgs e)
    {
        //ViewState["ChkInvestigationSelectedValue"] = ChkInvestigation.SelectedValue;
        String itemSelected = String.Empty;
        foreach (ListItem item in ChkInvestigation.Items)
        {
            if (item.Selected)
                itemSelected = itemSelected + item.Value + ",";

        }
        itemSelected = itemSelected.TrimEnd(',');
        ViewState["ChkInvestigationSelected"] = itemSelected;

        BindGrid();
        BindCheckBox();
        BindChkrdoInvestigations();

        if (ViewState["ChkInvestigationSelected"] != null)
        {
            string[] bookItems = ViewState["ChkInvestigationSelected"].ToString().Split(',');
            foreach (string bookId in bookItems)
            {
                ListItem item = ChkInvestigation.Items.FindByValue(bookId);
                if (item != null)
                    item.Selected = true;
            }
        }

        ViewState["ChkInvestigationSelected"] = null;

    }

    void CheckUncheckAll(bool tf)
    {
        try
        {
            foreach (ListItem item in ChkInvestigation.Items)
            {
                item.Selected = tf;
                //item.Selected = true;

            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }

    protected void rdoInvestigations_SelectedIndexChanged(object sender, EventArgs e)
    {
        // BindGrid();
        BindChkrdoInvestigations();
    }

    private void BindChkrdoInvestigations()
    {
        try
        {
            if (rdoInvestigations.SelectedValue == "0")
            {
                CheckUncheckAll(true);
            }
            else if (rdoInvestigations.SelectedValue == "1")
            {
                CheckUncheckAll(false);
            }

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }

    protected void rdoResultView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvDetails.Visible = false;
            gvLabDetailsXaxis.Visible = false;

            if (common.myStr(rdoResultView.SelectedValue).Equals("XA"))
            {
                gvLabDetailsXaxis.Visible = true;
            }
            else
            {
                gvDetails.Visible = true;
            }
            BindGrid();
        }
        catch
        {
        }

    }

    protected void gvLabDetailsXaxis_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = true;
              //  e.Row.Cells[0].Width = Unit.Pixel(35);

                e.Row.Cells[1].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //0-InvestigationName,1-Report_type

                string strReport_type = string.Empty;
                string strInv_Result = string.Empty;

                Session["Inv_Result"] = string.Empty;

                if (e.Row.Cells.Count > 2)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        strReport_type = common.myStr(e.Row.Cells[1].Text).Replace("&nbsp;", "");

                        if (i > 1)
                        {
                            strInv_Result = common.myStr(e.Row.Cells[i].Text).Replace("&nbsp;", "");
                            if (common.myLen(strInv_Result) > 0)
                            {
                                if (common.myStr(strReport_type).Equals("T"))
                                {
                                    if (common.myLen(strInv_Result) > 0)
                                    {
                                        LinkButton lnk = new LinkButton();

                                        lnk.ID = "lnk" + common.myInt(e.Row.RowIndex).ToString() + i.ToString();
                                        lnk.Text = "Text Report";
                                        lnk.ToolTip = "Click to show results";
                                       // Session["Inv_Result"] = strInv_Result;
                                        lnk.Attributes.Add("onclick", "showResultPopup(event,'" + strInv_Result + "');");
                                        //hdnShowInv_Result.Value = strInv_Result;
                                        //btnShowInv_Result_OnClick(null, null);
                                        //Alert.ShowAjaxMsg(common.myStr( strInv_Result.Length ), this);
                                        //  Alert.ShowAjaxMsg(strInv_Result, this);
                                        // lnk.Attributes.Add("onclick", "window.open('../Editor/WordProcessorInvestigationResult.aspx','Window1', 'menubar=no,width=1100,height=350,top=100,left=300,bottom=500,right=300, toolbar=no');");
                                        // lnk.Attributes.Add("onclick", "window.radopen('../Editor/WordProcessorInvestigationResult.aspx','Window1');");

                                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Transparent;
                                        e.Row.Cells[i].Controls.Add(lnk);
                                    }
                                }
                                else
                                {
                                    Label lbl = new Label();
                                    if (common.myLen(strInv_Result) > 0)
                                    {
                                        lbl.Text = strInv_Result;
                                    }
                                    e.Row.Cells[i].Controls.Add(lbl);
                                }
                            }
                        }
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
    protected void gvLabDetailsXaxis_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLabDetailsXaxis.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void btnShowInv_Result_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myLen(hdnShowInv_Result.Value) > 0)
            {
                Session["Inv_Result"] = common.myStr(hdnShowInv_Result.Value);

                RadWindow3.NavigateUrl = "~/Editor/WordProcessorInvestigationResult.aspx";

                RadWindow3.Height = 500;
                RadWindow3.Width = 1100;
                RadWindow3.Top = 20;
                RadWindow3.Left = 20;
                // RadWindowForNew.Title = "Time Slot";
                //RadWindow3.OnClientClose = "OnClientClose";
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }

}