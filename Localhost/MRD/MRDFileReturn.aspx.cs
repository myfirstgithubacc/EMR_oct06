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

public partial class MRD_MRDFileReturn : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    //wcf_Service_MRD.MRDServiceClient objMRD = new wcf_Service_MRD.MRDServiceClient();
    BaseC.RestFulAPI objMRD = new BaseC.RestFulAPI(sConString);
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PageType"] = common.myStr(Request.QueryString["PT"]);

            ViewState["tblStatus"] = null;
            
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now;

            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;

            bindControl();

            bindDetailsData();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            DataTable tbl = new DataTable();

            DataColumn col = new DataColumn("StatusCode");
            tbl.Columns.Add(col);

            col = new DataColumn("Status");
            tbl.Columns.Add(col);

            DataRow DR = tbl.NewRow();
            DR["StatusCode"] = "RTN";
            DR["Status"] = "File Returned";

            tbl.Rows.Add(DR);

            ViewState["tblStatus"] = tbl;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
        finally
        {
            ds.Dispose();
        }
    }

    private void bindDetailsData()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "&nbsp;";

        DataSet ds = new DataSet();
        try
        {
            setDate();

            //objMRD = new wcf_Service_MRD.MRDServiceClient();
            objMRD = new BaseC.RestFulAPI(sConString);

            ds = objMRD.getManualMRDFileIssueRetun(common.myStr(ddlFileStatus.SelectedValue), 0, common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue),
                            common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value),sConString);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvData.DataSource = ds.Tables[0];
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void ddlFileStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        setDate();
    }

    void setDate()
    {
        try
        {
            tblDate.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":
                    //txtFromDate.SelectedDate = DateTime.Now;
                    //txtToDate.SelectedDate = DateTime.Now;

                    tblDate.Visible = true;
                    break;
            }
        }
        catch
        {
        }
    }

    protected void gvData_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                //HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                //int QuotationId = common.myInt(((Label)row.FindControl("lblQuotationId")).Text);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            Telerik.Web.UI.RadContextMenu menuStatus = (RadContextMenu)e.Item.NamingContainer;

            //objMRD = new wcf_Service_MRD.MRDServiceClient();
            objMRD = new BaseC.RestFulAPI(sConString);
            string strMsg = objMRD.UpdateManualMRDFileIssueRetun(common.myInt(hdnGIssueId.Value),
                            common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]),sConString);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                bindDetailsData();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

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

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //    HiddenField hdnDispenseStatus = (HiddenField)e.Row.FindControl("hdnDispenseStatus");

            //    System.Drawing.Color color = System.Drawing.Color.White;

            //    if (common.myInt(hdnDispenseStatus.Value) == 3)
            //    {
            //        e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
            //    }
            //    else if (common.myInt(hdnDispenseStatus.Value) == 2)
            //    {
            //        e.Row.BackColor = System.Drawing.Color.Bisque;
            //    }

            HiddenField hdnIssueId = (HiddenField)e.Row.FindControl("hdnIssueId");
            HiddenField hdnMRDStatusId = (HiddenField)e.Row.FindControl("hdnMRDStatusId");
            HiddenField hdnMRDStatusCode = (HiddenField)e.Row.FindControl("hdnMRDStatusCode");
            ImageButton btnCategory = (ImageButton)e.Row.FindControl("btnCategory");

            Telerik.Web.UI.RadContextMenu menuStatus = (Telerik.Web.UI.RadContextMenu)e.Row.FindControl("menuStatus");

            DataTable tbl = (DataTable)ViewState["tblStatus"];

            btnCategory.Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "','" + hdnIssueId.ClientID + "')");
            menuStatus.DataSource = tbl;
            menuStatus.DataTextField = "Status";
            menuStatus.DataValueField = "StatusCode";
            menuStatus.DataBind();

            //if (common.myStr(ViewState["PT"]) == "REQ")
            //{
            //    if (common.myInt(hdnStatusId.Value) == common.myInt(ViewState["SentForBilling"]))
            //    {
            //        btnCategory.Visible = true;
            //    }
            //    else
            //    {
            //        btnCategory.Visible = false;
            //    }
            //}

            if (common.myInt(hdnIssueId.Value) == 0
                || common.myStr(hdnMRDStatusCode.Value) == "RTN")
            {
                btnCategory.Visible = false;
            }
        }
    }



}
