using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Pharmacy_SaleIssue_DepartmentConsumptionList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsDepartmentConsumption objDeptCon;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (common.myInt(Session["StoreId"]) == 0)
        //{
        //    Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + common.myStr(Request.Url.PathAndQuery), false);
        //}

        if (!IsPostBack)
        {
            objDeptCon = new BaseC.clsDepartmentConsumption(sConString);

            dtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtFromDate.SelectedDate = DateTime.Now;

            dtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtToDate.SelectedDate = DateTime.Now;

            hdnDecimalPlaces.Value = "2";
            if (common.myInt(Request.QueryString["DecimalPlaces"]) > 0)
            {
                hdnDecimalPlaces.Value = common.myStr(Request.QueryString["DecimalPlaces"]);
            }

            lblStoreName.Text = common.myStr(Request.QueryString["StoreName"]);

            BindBlankGrid();
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Font.Bold = commonLabelSetting.cBold;
            //if (commonLabelSetting.cFont != "")
            //{
            //    lblMessage.Font.Name = commonLabelSetting.cFont;
            //}

            //objPharmacy = new BaseC.clsPharmacy(sConString);

            //clearControl();

            bindData();
        }
    }

    private void clearControl()
    {
        ddlStatus.SelectedIndex = 0;
        dtFromDate.SelectedDate = DateTime.Now;
        dtToDate.SelectedDate = DateTime.Now;
        cbShowCancelled.Checked = false;
        lblMessage.Text = "";
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData();
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData();
    }

    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //((Label)e.Row.FindControl("lblBillAmount")).Text = common.myDbl(((Label)e.Row.FindControl("lblBillAmount")).Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //((Label)e.Row.FindControl("lblDiff")).Text = common.myDbl(((Label)e.Row.FindControl("lblDiff")).Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //HiddenField hdnGRNID = (HiddenField)e.Row.FindControl("hdnGRNID");
                //if (common.myInt(hdnGRNID.Value) == 0)
                //{
                //    e.Row.Cells[0].Enabled = false;
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

    protected void gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(((HiddenField)gv.SelectedRow.FindControl("hdnDocId")).Value) > 0)
            {
                hdnDocId.Value = common.myStr(((HiddenField)gv.SelectedRow.FindControl("hdnDocId")).Value);
                hdnDocNo.Value = common.myStr(((Label)gv.SelectedRow.FindControl("lblDocNo")).Text);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindData()
    {
        try
        {
            objDeptCon = new BaseC.clsDepartmentConsumption(sConString);

            int Cancelled = 0;
            if (cbShowCancelled.Checked)
            {
                Cancelled = 1;
            }

            string Status = common.myStr(ddlStatus.SelectedValue);
            string sFromDate = Convert.ToString(dtFromDate.SelectedDate.Value.ToString("yyyy-MM-dd")) + " 00:00";
            string sToDate = Convert.ToString(dtToDate.SelectedDate.Value.ToString("yyyy-MM-dd")) + " 23:59";

            DataSet dsSearch = objDeptCon.GetDepartmentConsumptionList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                            common.myInt(Request.QueryString["StoreId"]), Status, sFromDate, sToDate, "", Cancelled);

            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                gv.DataSource = dsSearch.Tables[0];
            }
            else
            {
                DataRow DR = dsSearch.Tables[0].NewRow();
                dsSearch.Tables[0].Rows.Add(DR);

                gv.DataSource = dsSearch.Tables[0];
            }

            gv.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gv_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv.PageIndex = e.NewPageIndex;
        bindData();
    }

    void BindBlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CreateTable(5);
            gv.DataSource = dt;
            gv.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable CreateTable(int RowCount)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ConsumptionNo");
        dt.Columns.Add("ConsumptionId");
        dt.Columns.Add("ConsumptionDate");
        dt.Columns.Add("ProcessStatus");
        dt.Columns.Add("EncodedBy");
        dt.Columns.Add("EncodedDate");

        for (int i = 0; i < RowCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["ConsumptionNo"] = DBNull.Value;
            dr["ConsumptionId"] = DBNull.Value;
            dr["ConsumptionDate"] = DBNull.Value;
            dr["ProcessStatus"] = DBNull.Value;
            dr["EncodedBy"] = DBNull.Value;
            dr["EncodedDate"] = DBNull.Value;
            dt.Rows.Add(dr);
        }

        return dt;
    }
}
