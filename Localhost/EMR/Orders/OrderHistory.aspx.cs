using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
public partial class EMR_Orders_OrderHistory_t : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objCommon ;
    DataSet ds;
    private void BindDetailsGrid()
    {
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            ds = new DataSet();
            ds = order.GetPatientServices(Convert.ToInt16(Session["HospitalLocationId"]), 0, "", 0, ViewState["OrderId"] == null ? 0 : Convert.ToInt32(ViewState["OrderId"]), true);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOrderHistory.DataSource = ds;
                gvOrderHistory.DataBind();
            }
            else
            {
                BindBlnkGridDetails();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindDateRange()
    {
        RadComboBoxItem ls = new RadComboBoxItem();
        ls.Text = "All";
        ls.Value = "";
        ddldateRange.Items.Add(ls);

        RadComboBoxItem lst1 = new RadComboBoxItem();
        lst1.Text = "Today";
        lst1.Value = "DD0";
        lst1.Selected = true;
        ddldateRange.Items.Add(lst1);

        RadComboBoxItem lst2 = new RadComboBoxItem();
        lst2.Text = "Last Week";
        lst2.Value = "WW-1";
        ddldateRange.Items.Add(lst2);

        RadComboBoxItem lst3 = new RadComboBoxItem();
        lst3.Text = "Last Month";
        lst3.Value = "MM-1";
        ddldateRange.Items.Add(lst3);

        RadComboBoxItem lst4 = new RadComboBoxItem();
        lst4.Text = "Last Six Month";
        lst4.Value = "MM-6";
        ddldateRange.Items.Add(lst4);

        RadComboBoxItem lst5 = new RadComboBoxItem();
        lst5.Text = "Last Year";
        lst5.Value = "YY-1";
        ddldateRange.Items.Add(lst5);

        RadComboBoxItem lst6 = new RadComboBoxItem();
        lst6.Text = "Date Range";
        lst6.Value = "DR";
        ddldateRange.Items.Add(lst6);

    }
    private void BindMainGrid()
    {
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            ds = new DataSet();
            ds = order.GetPatientOrderMainHistory(Convert.ToInt16(Session["HospitalLocationId"]), ViewState["RegistrationId"] != null ? common.myInt(ViewState["RegistrationId"]) : 0,
                ViewState["EncounterId"] != null ? common.myInt(ViewState["EncounterId"]) : 0,
                ddlProvider.SelectedValue == "" ? 0 : Convert.ToInt32(ddlProvider.SelectedValue), ddlFacility.SelectedValue == "" ? 0 : Convert.ToInt32(ddlFacility.SelectedValue),
                ddldateRange.SelectedValue, ddldateRange.SelectedValue == "DR" ? Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy/MM/dd") : "",
                ddldateRange.SelectedValue == "DR" ? Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd") : "", ddlVisitType.SelectedValue == "" ? "" : ddlVisitType.SelectedValue,
                Convert.ToInt16(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOrderMain.DataSource = ds;
                gvOrderMain.DataBind();
            }
            else
            {
                BindBlnkGridMain();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindDropDownList()
    {
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            Hashtable hshInput = new Hashtable();
            ds = new DataSet();
            ds = order.GetEncounterDoctors(Convert.ToInt16(Session["HospitalLocationId"]));
            ddlProvider.DataSource = ds;
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataBind();

            ds = new DataSet();
            ds = objCommon.GetFacilityList(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["UserId"]),
                Convert.ToInt16(Session["GroupId"]), 0);
            ddlFacility.DataSource = ds;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();

            ddlFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlFacility.Items[0].Value = "0";

            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataTable CreateDetailsTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("OrderDate");
        dt.Columns.Add("DepartmentName");
        dt.Columns.Add("SubName");
        dt.Columns.Add("VisitType");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("FacilityName");
        dt.Columns.Add("ICDID");
        dt.Columns.Add("Stat");
        dt.Columns.Add("ServiceID");
        dt.Columns.Add("DoctorID");
        dt.Columns.Add("FacilityId");
        dt.Columns.Add("ServiceStatus");
        dt.Columns.Add("RefServiceCode");
        dt.Columns.Add("LabStatus");
        dt.Columns.Add("PreauthorizedNo");

        return dt;
    }

    private DataTable CreateMainTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("OrderId");
        dt.Columns.Add("OrderNo");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("AgeGender");
        dt.Columns.Add("BillStatus");
        dt.Columns.Add("MobileNo");
        dt.Columns.Add("OPIP");
        dt.Columns.Add("OrderDate");
        dt.Columns.Add("VisitType");
        dt.Columns.Add("VisitDate");
        dt.Columns.Add("FacilityName");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("OrderStatus");
        dt.Columns.Add("Remarks");
        return dt;
    }
    private void BindBlnkGridMain()
    {
        try
        {
            //Cache.Remove("OrderHistory");
            DataTable datatable = CreateMainTable();
            DataRow datarow = datatable.NewRow();
            datatable.Rows.Add(datarow);
            gvOrderMain.DataSource = datatable;
            gvOrderMain.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindBlnkGridDetails()
    {
        try
        {
            //Cache.Remove("OrderHistory");
            DataTable datatable = CreateDetailsTable();
            DataRow datarow = datatable.NewRow();
            datatable.Rows.Add(datarow);
            gvOrderHistory.DataSource = datatable;
            gvOrderHistory.DataBind();



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"].ToString() == "FROM")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        objCommon = new BaseC.RestFulAPI(sConString);
        if (Session["Orderpid"] != null)
        {
            ViewState["PageId"] = Session["Orderpid"].ToString();
        }
        else
        {
            ViewState["PageId"] = "0";
        }
        if (!IsPostBack)
        {
            dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
            dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
            dtpfromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;

            if (Request.QueryString["EncId"] != null)
            {
                ViewState["EncounterId"] = Request.QueryString["EncId"].ToString();
                ViewState["RegistrationId"] = Request.QueryString["RegId"].ToString();
            }
            ddlVisitType.SelectedValue = Session["OPIP"] != null ? Session["OPIP"].ToString() : "O";

            BindDropDownList();
            BindDateRange();
            BindMainGrid();
            BindBlnkGridDetails();

        }
    }

    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        if (ddldateRange.SelectedValue == "DR")
        {
            pnlDatarng.Visible = true;
        }
        else
        {
            pnlDatarng.Visible = false;
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BaseC.EMR objBb = new BaseC.EMR(sConString);
        if (common.myStr(txtRegno.Text) != "")
        {
            DataSet ds = objBb.GetRegistrationId(common.myInt(txtRegno.Text.Trim()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["RegistrationId"] = common.myStr(ds.Tables[0].Rows[0]["id"]);
            }
            else
            {
                ViewState["RegistrationId"] = null;
            }
        }
        BindMainGrid();
    }

    protected void gvOrderHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            HiddenField hdnServiceStatus = (HiddenField)e.Row.FindControl("hdnServiceStatus");
            //CheckBox chkInner = (CheckBox)e.Row.FindControl("chkInner");
            //if (hdnServiceStatus.Value == "Service Cancel")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Aqua;
            //    chkInner.Visible = false;
            //}
            //else
            //{
            //    chkInner.Visible = true;
            //}
            if (e.Row.Cells[9].Text == "False")
            {
                e.Row.Cells[9].Text = "";
            }
            else if (e.Row.Cells[9].Text != "&nbsp;")
            {
                e.Row.Cells[9].Text = "Y";
            }

        }
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        try
        {
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
            ddlProvider.SelectedValue = "0";
            ddldateRange.SelectedValue = "0";
            ddlVisitType.SelectedValue = "0";
            txtRegno.Text = "";

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnAddToDay_Click(object o, EventArgs e)
    {
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            StringBuilder strXML = new StringBuilder();
            foreach (GridViewRow row in gvOrderHistory.Rows)
            {
                if (((CheckBox)row.FindControl("chkInner")).Checked)
                {
                    Label lblOrderId = (Label)row.FindControl("lblId");
                    Label lblIcdId = (Label)row.FindControl("lblIcdId");
                    HiddenField hdnFacilityId = (HiddenField)row.FindControl("hdnFacilityId");
                    HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                    TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                    HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");
                    HiddenField hdnStat = (HiddenField)row.FindControl("hdnStat");
                    Label lblOrderDate = (Label)row.FindControl("lblOrderDate");
                    string[] sDate = lblOrderDate.Text.Split(' ');
                    if (!string.IsNullOrEmpty(lblOrderId.Text) && sDate[0] != Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy"))
                    {
                        strXML.Append("<Table1><c1>");
                        strXML.Append(common.myInt(hdnServiceId.Value));
                        strXML.Append("</c1><c2>");
                        strXML.Append("</c2><c3>");
                        strXML.Append(1);
                        strXML.Append("</c3><c4>");
                        strXML.Append(hdnDoctorId.Value);
                        strXML.Append("</c4><c5>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c5><c6>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c6><c7>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c7><c8>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c8><c9>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c9><c10>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c10><c11>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c11><c12>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c12><c13>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c13><c14>");
                        strXML.Append(0);
                        strXML.Append("</c14><c15>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c15><c16>");
                        strXML.Append(lblIcdId.Text == "&nbsp;" ? "" : lblIcdId.Text);
                        strXML.Append("</c16><c17>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c17><c18>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c18><c19>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c19><c20>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c20><c21>");
                        strXML.Append(txtRemarks.Text == "&nbsp;" ? "" : txtRemarks.Text);
                        strXML.Append("</c21><c22>");
                        strXML.Append(DBNull.Value);
                        strXML.Append("</c22><c23>");
                        strXML.Append(hdnFacilityId.Value);
                        strXML.Append("</c23><c24>");
                        strXML.Append(hdnStat.Value);
                        strXML.Append("</c24></Table1>");
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("This services are not allow to add", Page);
                        return;
                    }
                }
            }
            if (strXML.ToString() != "")
            {
                ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
                string sChargeCalculationRequired = "Y";
                string stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                string opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                int CompanyId = 0, InsuranceId = 0, CardId = 0;
                if (ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }

                Hashtable hshOut = order.saveOrders(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()),
                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), strXML.ToString(), "", "", Convert.ToInt32(Session["UserID"].ToString()),
                    common.myInt(ddlProvider.SelectedValue), CompanyId, stype, common.myStr("E"), common.myStr(opip), InsuranceId, CardId,
                    Convert.ToDateTime(DateTime.Now), sChargeCalculationRequired, false, 1, 0, "", common.myInt(Session["EntrySite"]));
                if (hshOut["chvErrorStatus"].ToString().Length == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Order Saved Successfully";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "There is some error while saving order." + hshOut["chvErrorStatus"].ToString();
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Service", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvOrderHistory_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LinkButton lnkSelect = (LinkButton)gvOrderHistory.Rows[e.NewSelectedIndex].FindControl("lnkSelect");
        int OrderId = common.myInt(lnkSelect.CommandArgument);

        if (OrderId > 0)
        {
            RadWindowPopup.NavigateUrl = "~/EMR/Orders/PatientLabDashboard.aspx?OrderId=" + OrderId;

            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 1000;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.OnClientClose = "";
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
        }
    }
    protected void btnNewOrder_OnClick(object o, EventArgs e)
    {
        Response.Redirect("~/EMR/Orders/Orders.aspx?Mpg=P17", false);
    }


    protected void gvOrderMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label lblOrderId = (Label)gvOrderMain.SelectedRow.FindControl("lblOrderId");
        Label lblOrderNo = (Label)gvOrderMain.SelectedRow.FindControl("lblOrderNo");
        //gvOrderMain.SelectedRow.BackColor = System.Drawing.Color.FromName("#CC6666");
        if (lblOrderId.Text != "")
        {
            ViewState["OrderId"] = lblOrderId.Text;
            BindDetailsGrid();
        }
        lblTitleOrderNo.Text = "(Order No :" + lblOrderNo.Text + ")";
    }
    protected void gvOrderMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
            Label lblOrderId = (Label)e.Row.FindControl("lblOrderId");
            if (lblOrderId.Text != "")
            {
                if (ddldateRange.SelectedValue == "DD0")
                {
                    e.Row.BackColor = System.Drawing.Color.Aqua;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.White;
            }
            //Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");
            //if (lblOrderStatus.Text =="Order Canceled")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Aqua;
            //}
        }
    }
    protected void gvOrderMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrderMain.PageIndex = e.NewPageIndex;
        BindMainGrid();
    }
}
