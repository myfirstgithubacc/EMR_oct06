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
using System.Text;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class WardManagement_HousekeepingApproval : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private enum enumCol : byte
    {
        BedNo = 0,
        RegistrationNo = 1,
        EncounterNo = 2,
        PatientName = 3,
        Reason = 4,
        RequestedBy = 5,
        RequestDateTime = 6,
        AcknowledgedBy = 7,
        AcknowledgedDateTime = 8,
        ClosedBy = 9,
        ClosedDateTime = 10,
        RequestSelect = 11,
        RequestDelete = 12
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            txtFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            txtToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();

            txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
            txtToDate.SelectedDate = DateTime.Now;

            clearControl();

            bindData();
        }
    }

    private void clearControl()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            txtSearchNumeric.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
            txtToDate.SelectedDate = DateTime.Now;

            ddlApproved.SelectedIndex = 0;
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
        bindData();
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData();
    }

    protected void gvEncounter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnRequestId = (HiddenField)e.Row.FindControl("hdnRequestId");
                LinkButton lnkAcknowledge = (LinkButton)e.Row.FindControl("lnkAcknowledge");
                HiddenField hdnStatus = (HiddenField)e.Row.FindControl("hdnStatus");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");

                lnkAcknowledge.Visible = true;
                ibtnDelete.Visible = true;

                switch (common.myStr(hdnStatus.Value).ToUpper())
                {
                    case "R":
                        lnkAcknowledge.Text = "Acknowledge";
                        e.Row.BackColor = System.Drawing.Color.LightCyan;
                        ibtnDelete.Visible = false;
                        break;
                    case "A":
                        lnkAcknowledge.Text = "Close";
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                        break;
                    case "C":
                        lnkAcknowledge.Text = "Acknowledge";
                        lnkAcknowledge.Visible = false;
                        e.Row.BackColor = System.Drawing.Color.LightGreen;
                        break;
                    default:
                        lnkAcknowledge.Text = "Acknowledge";
                        break;
                }

                if (common.myInt(hdnRequestId.Value).Equals(0))
                {
                    lnkAcknowledge.Visible = false;
                    ibtnDelete.Visible = false;
                }

                //if (common.myInt(hdnRefundApprovedAckBy.Value) > 0)
                //{
                //    e.Row.BackColor = System.Drawing.Color.LightGreen;
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

    protected void gvEncounter_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.WardManagement objP = new BaseC.WardManagement();
        string strMsg = string.Empty;
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("REQUESTSELECT"))
            {
                int RequestId = common.myInt(e.CommandArgument);
                if (RequestId > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                    HiddenField hdnStatus = (HiddenField)row.FindControl("hdnStatus");

                    if (common.myStr(hdnStatus.Value).ToUpper().Equals("R"))
                    {
                        strMsg = objP.SaveUpdateHousekeepingRequest(RequestId, common.myInt(Session["FacilityId"]), 0,
                                            common.myInt(hdnEncounterId.Value), 0, string.Empty, "A", false, common.myInt(Session["UserId"]), common.myInt(null));
                    }
                    else if (common.myStr(hdnStatus.Value).ToUpper().Equals("A"))
                    {
                        strMsg = objP.SaveUpdateHousekeepingRequest(RequestId, common.myInt(Session["FacilityId"]), 0,
                                            common.myInt(hdnEncounterId.Value), 0, string.Empty, "C", false, common.myInt(Session["UserId"]), common.myInt(null));
                    }

                    lblMessage.Text = strMsg;
                    if (strMsg.ToUpper().Contains(" SUCCESSFULLY") && !strMsg.ToUpper().Contains("USP"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        bindData();
                    }
                }
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("REQUESTDELETE"))
            {
                int RequestId = common.myInt(e.CommandArgument);
                if (RequestId > 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                    HiddenField hdnStatus = (HiddenField)row.FindControl("hdnStatus");

                    strMsg = objP.SaveUpdateHousekeepingRequest(RequestId, common.myInt(Session["FacilityId"]), 0,
                                            common.myInt(hdnEncounterId.Value), 0, string.Empty, common.myStr(hdnStatus.Value), true, common.myInt(Session["UserId"]), common.myInt(null));

                    lblMessage.Text = strMsg;
                    if (strMsg.ToUpper().Contains(" SUCCESSFULLY") && !strMsg.ToUpper().Contains("USP"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        bindData();
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

    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        bindData();
    }

    private void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objP = new BaseC.WardManagement();
        try
        {
            string BedNo = string.Empty;
            int RegistrationNo = 0;
            string EncounterNo = string.Empty;

            switch (common.myStr(ddlSearchOn.SelectedValue).ToUpper())
            {
                case "BED":
                    BedNo = common.myStr(txtSearch.Text);
                    break;
                case "REG":
                    RegistrationNo = common.myInt(txtSearchNumeric.Text);
                    break;
                case "ENC":
                    EncounterNo = common.myStr(txtSearch.Text);
                    break;
            }

            ds = objP.getWardHousekeepingRequest(common.myInt(Session["FacilityId"]), 0, RegistrationNo, EncounterNo, BedNo,
                                            Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                            Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"), common.myStr(ddlApproved.SelectedValue), common.myInt(null));

            lblTotalRecordCount.Text = "Total Records: ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEncounter.DataSource = ds.Tables[0];

                lblTotalRecordCount.Text = lblTotalRecordCount.Text + " " + common.myInt(ds.Tables[0].Rows.Count).ToString();
            }
            else
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                gvEncounter.DataSource = ds.Tables[0];

                lblTotalRecordCount.Text = lblTotalRecordCount.Text + " 0";
            }

            gvEncounter.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void gvEncounter_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEncounter.PageIndex = e.NewPageIndex;
        bindData();
    }

    protected void ddlApproved_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            bindData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {

        }
    }


    protected void ddlSearchOn_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtSearchNumeric.Text = string.Empty;
            txtSearch.Text = string.Empty;

            txtSearchNumeric.Visible = false;
            txtSearch.Visible = false;

            if (common.myStr(ddlSearchOn.SelectedValue).ToUpper().Equals("REG"))
            {
                txtSearchNumeric.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {

        }
    }

}
