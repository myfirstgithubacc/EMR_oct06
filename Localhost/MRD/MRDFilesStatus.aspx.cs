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

public partial class MRD_MRDFilesStatus : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    // wcf_Service_MRD.MRDServiceClient objMRD = new wcf_Service_MRD.MRDServiceClient();
    BaseC.RestFulAPI objMRD = new BaseC.RestFulAPI(sConString);
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.Security sec = new BaseC.Security(sConString); // Added By Akshay 18-Aug-2022 Tirathram


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PageType"] = common.myStr(Request.QueryString["PT"]);

            ViewState["DSStatus"] = null;

            setDate();

            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now;

            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;

            bindControl();

            IsUserAllowIssueMrdFile();
            //bindHeader();
            bindDetailsData();


        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);
            ds = objP.getStatus(common.myInt(Session["HospitalLocationID"]), "MRDFileStatus", "", 0);

            ddlFileStatus.DataSource = ds.Tables[0];
            ddlFileStatus.DataTextField = "Status";
            ddlFileStatus.DataValueField = "StatusId";
            ddlFileStatus.DataBind();

            ViewState["DSStatus"] = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlFileStatus.SelectedIndex = 0;
                tblRemarks.Visible = true;
            }
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

    //private void bindHeader()
    //{
    //    switch (common.myStr(ViewState["PageType"]))
    //    {
    //        case "FACK":
    //            lblMRD.Text = "File Acknowledge";
    //            btnUpdateStatus.Text = "File Acknowledge";
    //            break;
    //        case "FISS":
    //            lblMRD.Text = "File Issue";
    //            btnUpdateStatus.Text = "File Issue";
    //            break;
    //        case "FREC":
    //            lblMRD.Text = "File Receive";
    //            btnUpdateStatus.Text = "File Receive";
    //            break;
    //        case "FRET":
    //            lblMRD.Text = "File Return";
    //            btnUpdateStatus.Text = "File Return";
    //            break;
    //        case "FRACK":
    //            lblMRD.Text = "File Returned Acknowledge";
    //            btnUpdateStatus.Text = "File Returned Acknowledge";
    //            break;
    //        default:
    //            ViewState["PageType"] = "FACK";

    //            lblMRD.Text = "File Acknowledge";
    //            btnUpdateStatus.Text = "File Acknowledge";
    //            break;
    //    }
    //}

    private void bindDetailsData()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "&nbsp;";

        DataSet ds = new DataSet();
        try
        {
            //objMRD = new wcf_Service_MRD.MRDServiceClient();
            objMRD = new BaseC.RestFulAPI(sConString);

            //string searchOn = "";
            //if(ddlSearchOn.SelectedValue=="UHID")
            //{
            //    searchOn = txtSearchOnForUHID.Text;
            //}
            //else if (ddlSearchOn.SelectedValue == "Enc")
            //{
            //    searchOn = txtSearchOn.Text;

            //}
            //else
            //{

            //}

            // Comment By Mukesh Srivastava 29-3-2018

            //ds = objMRD.GetMRDRequestFile(common.myInt(ddlFileStatus.SelectedValue), 0, common.myInt(Session["HospitalLocationID"]),
            //               common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue),
            //               common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value),sConString, txtSearchOnForUHID.Text,txtSearchOnEnc.Text,txtSearchOnPat.Text);

            ds = objMRD.GetMRDReturnFile(common.myInt(ddlFileStatus.SelectedValue), 0, common.myInt(Session["HospitalLocationID"]),
                         common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue),
                         common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value), sConString, txtSearchOnForUHID.Text, txtSearchOnEnc.Text, txtSearchOnPat.Text);


            //ds = objMRD.GetMRDRequestFile(common.myInt(ddlFileStatus.SelectedValue), 0, common.myInt(Session["HospitalLocationID"]),
            //                common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue),
            //                common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value),sConString, txtSearchOnForUHID.Text,txtSearchOnEnc.Text,txtSearchOnPat.Text);

            //switch (common.myStr(ViewState["PageType"]))
            //{
            //    case "FACK":
            //        break;
            //    case "FISS":
            //        break;
            //    case "FREC":
            //        break;
            //    case "FRET":
            //        break;
            //    case "FRACK":
            //        break;
            //    default:
            //        break;
            //}

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
        //reBindData();
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
            string strMsg = string.Empty;
            Telerik.Web.UI.RadContextMenu menuStatus = (RadContextMenu)e.Item.NamingContainer;

            // objMRD = new wcf_Service_MRD.MRDServiceClient();
            objMRD = new BaseC.RestFulAPI(sConString);
            // Akshay 18-Aug-2022 Tirathram
            if ((menuStatus.SelectedItem.Text == "File Approved" || menuStatus.SelectedItem.Text == "File Reject"))
            {
                if (true)
                {
                    bool result;
                    result = sec.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), "IsAllowMRDFileApprovedOrReject");
                    if (result == false)
                    {
                        lblMessage.Text = "You have not permission to File Approved or Reject";
                        return;
                    }
                }
                if (txtFileIssueRemarks.Text.Trim() == "")
                {
                    lblMessage.Text = "Remarks can not be empty!!!";
                    return;
                }
            }
            strMsg = objMRD.UpdateMRDFileStatus(common.myInt(hdnGRequestId.Value), common.myInt(menuStatus.SelectedValue),
                                    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), sConString, txtFileIssueRemarks.Text.Trim());

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                bindDetailsData();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            }
            lblMessage.Text = strMsg;
            txtFileIssueRemarks.Text = string.Empty;
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

            HiddenField hdnRequestId = (HiddenField)e.Row.FindControl("hdnRequestId");
            HiddenField hdnMRDStatusId = (HiddenField)e.Row.FindControl("hdnMRDStatusId");
            HiddenField hdnMRDStatusCode = (HiddenField)e.Row.FindControl("hdnMRDStatusCode");
            ImageButton btnCategory = (ImageButton)e.Row.FindControl("btnCategory");
            HiddenField hdnOPIP = (HiddenField)e.Row.FindControl("hdnOPIP");

            Telerik.Web.UI.RadContextMenu menuStatus = (Telerik.Web.UI.RadContextMenu)e.Row.FindControl("menuStatus");

            DataSet ds = (DataSet)ViewState["DSStatus"];
            DataView DV = new DataView(ds.Tables[0].Copy());

            bool isFound = false;
            for (int rIdx = 0; rIdx < ds.Tables[0].Rows.Count; rIdx++)
            {
                DataRow DR = ds.Tables[0].Rows[rIdx];

                if (isFound)
                {
                    if (common.myStr(hdnOPIP.Value) == "I" && common.myInt(ddlFileStatus.SelectedValue) == 291)
                    {
                        DV.RowFilter = "StatusId=288";
                    }
                    else if (common.myInt(ddlFileStatus.SelectedValue) == 290)
                    {
                        DV.RowFilter = "SequenceNo in(2,10)";
                        tblRemarks.Visible = true;
                    }
                    else
                    {
                        DV.RowFilter = "StatusId=" + common.myInt(DR["StatusId"]);
                    }
                    break;
                }
                if (common.myInt(DR["StatusId"]) == common.myInt(hdnMRDStatusId.Value))
                {
                    isFound = true;
                }
            }
            if (common.myBool(ViewState["IsUserAllowIssueMrdFileValue"]) == true)
            {
                btnCategory.Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "','" + hdnRequestId.ClientID + "','" + hdnMRDStatusId.ClientID + "')");
                menuStatus.DataSource = DV.ToTable();
                menuStatus.DataTextField = "Status";
                menuStatus.DataValueField = "StatusId";
                menuStatus.DataBind();
            }
            else
            {
                btnCategory.Visible = false;
            }


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



            if (common.myStr(hdnOPIP.Value) == "I" && common.myInt(ddlFileStatus.SelectedValue) == 291)
            {
                btnCategory.Visible = true;
            }
            // Akshay_18072022_Tirathram (Added=> common.myStr(hdnMRDStatusCode.Value) == "RCT")
            if (common.myInt(hdnRequestId.Value) == 0 || common.myInt(ddlFileStatus.SelectedValue) == 0 || common.myStr(hdnMRDStatusCode.Value) == "RTN" || common.myStr(hdnMRDStatusCode.Value) == "RCT")
            {
                btnCategory.Visible = false;
            }
        }
    }




    protected void ddlSearchOn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlSearchOn.SelectedValue == "UHID")
        {


            txtSearchOnForUHID.Visible = true;
            txtSearchOnEnc.Visible = false;
            txtSearchOnPat.Visible = false;
            txtSearchOnEnc.Text = string.Empty;
            txtSearchOnPat.Text = string.Empty;

        }
        else if (ddlSearchOn.SelectedValue == "Enc")
        {


            txtSearchOnEnc.Visible = true;
            txtSearchOnForUHID.Visible = false;
            txtSearchOnPat.Visible = false;
            txtSearchOnForUHID.Text = string.Empty;
            txtSearchOnPat.Text = string.Empty;


        }
        else
        {
            txtSearchOnPat.Visible = true;
            txtSearchOnForUHID.Visible = false;
            txtSearchOnEnc.Visible = false;
            txtSearchOnForUHID.Text = string.Empty;
            txtSearchOnEnc.Text = string.Empty;

        }

    }
    protected void IsUserAllowIssueMrdFile()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsUserAllowIssueMrdFile")))
        {
            ViewState["IsUserAllowIssueMrdFileValue"] = true;
        }
        else
        {
            ViewState["IsUserAllowIssueMrdFileValue"] = false;
        }
    }

    //protected void txtSearchOn_TextChanged(object sender, EventArgs e)
    //{

    //    int outbit = 0;
    //    if (int.TryParse(txtSearchOn.Text, out outbit) && ddlSearchOn.SelectedValue== "UHID")
    //    {

    //    }
    //    else
    //    {
    //        txtSearchOn.Text = string.Empty;

    //    }

    //}

    protected void btnApproved_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";
            string strMsg = string.Empty;
            int menuStatusId;
            if (ddlFileApprovalStatus.SelectedValue == "1")
            {
                menuStatusId = common.myInt(ViewState["FileApprovedId"]);
            }
            else
            {
                menuStatusId = common.myInt(ViewState["FileRejectId"]);
            }
            objMRD = new BaseC.RestFulAPI(sConString);
            strMsg = objMRD.UpdateMRDFileStatus(common.myInt(hdnGRequestId.Value), menuStatusId,
                                    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), sConString, txtFileIssueRemarks.Text.Trim());

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                bindDetailsData();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            }
            lblMessage.Text = strMsg;
            txtFileIssueRemarks.Text = string.Empty;
            DivApprovalStatus.Visible = false;
            ddlFileApprovalStatus.ClearSelection();
            txtApproveRemarks.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
}
