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
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class LIS_Phlebotomy_LabTestResultReview : System.Web.UI.Page
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
            //RadPane mpLeftPannel = (RadPane)Master.FindControl("LeftPnl");
            //mpLeftPannel.Visible = false;
            //RadSplitBar mpSplitBar = (RadSplitBar)Master.FindControl("Radsplitbar1");
            //mpSplitBar.Visible = false;
            //RadPane mpTopPnl = (RadPane)Master.FindControl("TopPnl");
            //mpTopPnl.Visible = false;
            //RadPane mpEndPane = (RadPane)Master.FindControl("EndPane");
            //mpEndPane.Visible = false;
            //System.Web.UI.HtmlControls.HtmlTable tblEnd = (System.Web.UI.HtmlControls.HtmlTable)Master.FindControl("tblEnd");
            //tblEnd.Visible = false;


            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }


            ViewState["DiagSampleId"] = 0;
            if (common.myLen(Request.QueryString["DiagSampleId"]) > 0)
            {
                ViewState["DiagSampleId"] = Request.QueryString["DiagSampleId"].ToString();
            }
            chkNoSMS.Checked = false;
            bindControl();
            bindData();
        }
    }

    private void bindControl()
    {
        try
        {
            #region fill minute
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {

                    ddlMinute.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    ddlMinute.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem ddltemp = (RadComboBoxItem)ddlMinute.Items.FindItemByText(iMinute.ToString());
            if (ddltemp != null)
            {
                ddltemp.Selected = true;
            }
            #endregion

            // Fill Employee Dropdown
            StringBuilder strType = new StringBuilder();
            ArrayList coll = new ArrayList();
            DataSet ds = new DataSet();
            BaseC.User valUser = new BaseC.User(sConString);
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);

            string strEmployeeType = valUser.getEmployeeType(common.myInt(Session["EmployeeId"]));

            //if (strEmployeeType == "LIC")
            //{
            //    coll.Add("LS");
            //    strType.Append(common.setXmlTable(ref coll));
            //    coll.Add("LD");
            //    strType.Append(common.setXmlTable(ref coll));
            //}
            //else if (strEmployeeType == "LS")
            //{
            //    coll.Add("LD");
            //    strType.Append(common.setXmlTable(ref coll));
            //}
            //else
            //{
            coll.Add("D");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LIC");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LS");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LD");
            strType.Append(common.setXmlTable(ref coll));
            //}

            ds = objMaster.getEmployeeData(common.myInt(Session["HospitalLocationID"]), 0, 0, "", "", 0, common.myInt(Session["UserId"]), "", 0);
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "UserId = " + common.myStr(Session["UserId"]) + "";

            //ddlReviewedBy.DataSource = ds.Tables[0].Copy();
            ddlReviewedBy.DataSource = dv.ToTable();
            ddlReviewedBy.DataValueField = "EmployeeId";
            ddlReviewedBy.DataTextField = "EmployeeNameWithNo";
            ddlReviewedBy.DataBind();

            ddlReviewedBy.SelectedIndex = ddlReviewedBy.Items.IndexOf(ddlReviewedBy.FindItemByValue(common.myInt(Session["EmployeeId"]).ToString()));
            //ddlReviewedBy.SelectedValue = common.myStr(Session["EmployeeId"]);

            ddlReviewedBy.Enabled = false;
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
            clearControl();

            BaseC.clsLISLabOther objLab = new BaseC.clsLISLabOther(sConString);
            DataSet ds = objLab.getResultReviewedData(common.myInt(ViewState["DiagSampleId"]), common.myStr(Request.QueryString["Source"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];
                if (common.myInt(DR["Active"]) > 0 || common.myStr(DR["Active"]) == "True")
                {
                    if (common.myInt(DR["ReviewedStatus"]) > 0 || common.myStr(DR["ReviewedStatus"]) == "True")
                    {
                        if (common.myInt(DR["ReviewedStatus"]) > 0 || common.myStr(DR["ReviewedStatus"]) == "True")
                            rblReviewedStatus.SelectedValue = "1";
                        else
                            rblReviewedStatus.SelectedValue = "0";

                        dtpReviewedDate.SelectedDate = common.myDate(DR["ReviewedDate"]);
                        rblReviewedStatus_SelectedIndexChanged(null, null);

                        int iMinute = common.myDate(DR["ReviewedDate"]).Minute;
                        RadComboBoxItem ddltemp = (RadComboBoxItem)ddlMinute.Items.FindItemByText(iMinute.ToString());
                        if (ddltemp != null)
                        {
                            ddltemp.Selected = true;
                        }

                        txtLabFlagValue.Text = common.myStr(DR["LabFlagValue"]);
                        txtTestResultStatus.Text = common.myStr(DR["TestResultStatus"]);

                        txtReviewedComments.Text = common.myStr(DR["ReviewedComments"]).Trim();

                        if (ddlReviewedBy.SelectedIndex > -1)
                        {
                        }
                        else
                        {
                            ddlReviewedBy.SelectedIndex = ddlReviewedBy.Items.IndexOf(ddlReviewedBy.Items.FindItemByValue(common.myInt(DR["ReviewedBy"]).ToString()));
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

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";

        rblReviewedStatus.SelectedIndex = 0;
        dtpReviewedDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]) + " hh:mm tt";
        dtpReviewedDate.DateInput.DisplayDateFormat = common.myStr(Session["OutputDateFormat"]) + " hh:mm tt";
        dtpReviewedDate.SelectedDate = DateTime.Now;

        int iMinute = DateTime.Now.Minute;
        RadComboBoxItem ddltemp = (RadComboBoxItem)ddlMinute.Items.FindItemByText(iMinute.ToString());
        if (ddltemp != null)
        {
            ddltemp.Selected = true;
        }
        txtLabFlagValue.Text = "";
        txtTestResultStatus.Text = "";
        txtReviewedComments.Text = string.Empty;
        ddlReviewedBy.SelectedIndex = 0;
        ddlReviewedBy.Enabled = (common.myInt(rblReviewedStatus.SelectedValue) == 1) ? true : false;

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void ddlMinute_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dtpReviewedDate.SelectedDate.Value.ToString());
        sb.Remove(dtpReviewedDate.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(dtpReviewedDate.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlMinute.Text);

        dtpReviewedDate.SelectedDate = common.myDate(sb.ToString());
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearControl();
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        review("Y");
        dvReviewAlert.Visible = false;
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        dvReviewAlert.Visible = false;
    }
    private bool isSaved(string Source)
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(rblReviewedStatus.SelectedValue) >= 1)
        {
            //if (common.myLen(txtReviewedComments.Text) == 0)
            //{
            //    strmsg += "Please Enter Comments !";
            //    isSave = false;
            //}

            if (common.myInt(ddlReviewedBy.SelectedValue) == 0)
            {
                strmsg += "Please Select Reviewed By !";
                isSave = false;
            }
        }

        lblMessage.Text = strmsg;
        if (Source != "Y")
        {
            if (common.myInt(rblReviewedStatus.SelectedValue) == 2)
            {
                isSave = false;
                dvReviewAlert.Visible = true;
            }
        }
        return isSave;
    }
    void review(string Source)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved(Source))
            {
                return;
            }

            BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);

            Label LBL = new Label();

            string strDiagSampleId = common.myStr(ViewState["DiagSampleId"]);
            //if (common.myLen(strDiagSampleId) > 2)
            //{
            //    if (common.myStr(strDiagSampleId).StartsWith("0,"))
            //    {
            //        strDiagSampleId = strDiagSampleId.Split(',')[1];
            //    }
            //}

            if (!common.myStr(Session["ResultReviewDiagSampleIds"]).Equals(string.Empty))
            {
                strDiagSampleId = common.myStr(Session["ResultReviewDiagSampleIds"]);
            }
            string source = "OPD";
            if (common.myStr(Request.QueryString["Source"]) == "O")
            {
                source = "OPD";
            }
            else if (common.myStr(Request.QueryString["Source"]) == "I")
            {
                source = "IPD";
            }
                int iNoSMS = 0;
            if (chkNoSMS.Checked == true)
                iNoSMS = 1;

            string strMsg = objval.updateReviewedResultByDiagSampleId(strDiagSampleId,
                common.myInt(rblReviewedStatus.SelectedValue), common.myDate(dtpReviewedDate.SelectedDate.Value.ToString("yyyy-MM-dd hh:mm")),
                txtReviewedComments.Text.Trim() + "$" + source,
                common.myInt(ddlReviewedBy.SelectedValue), common.myStr(txtLabFlagValue.Text), common.myStr(txtTestResultStatus.Text), iNoSMS);//nosms

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                // Alert.ShowAjaxMsg(strMsg, this.Page);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ////
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                ////
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        review("-");
    }

    protected void rblReviewedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReviewedBy.SelectedIndex = 0;
        ddlReviewedBy.Enabled = (common.myInt(rblReviewedStatus.SelectedValue) == 1) ? true : false;
    }
}