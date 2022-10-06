using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MRD_MRDFilesStatus : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.RestFulAPI objMRD;//= new wcf_Service_MRD.MRDServiceClient();
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(ddlName.SelectedValue) == "RN")
            {
                txtSearch.Visible = false;
                txtRegNo.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
                txtRegNo.Visible = false;
            }

            ViewState["Mpg"] = common.myStr(Request.QueryString["Mpg"]);

            ViewState["PageType"] = common.myStr(Request.QueryString["PT"]);
            setDate();
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now;
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;
            bindStatus();
            bindDetailsData();
        }
    }
    private void bindStatus()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {

            BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "MRDFileStatus", "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dv = new DataView(ds.Tables[0]);
                if (common.myStr(Request.QueryString["Status"]) == "Req")
                {
                    dv.RowFilter = "Code IN ('ISS','RTRQ')";
                    lblStatus1.Text = dv.ToTable().Rows[0]["Status"].ToString();
                    lblStatus2.BackColor = System.Drawing.ColorTranslator.FromHtml(dv.ToTable().Rows[0]["StatusColor"].ToString());

                    lblStatus3.Text = dv.ToTable().Rows[1]["Status"].ToString();
                    lblStatus4.BackColor = System.Drawing.ColorTranslator.FromHtml(dv.ToTable().Rows[1]["StatusColor"].ToString());

                    lblTitle.Text = "File Request/Return";
                }
                else if (common.myStr(Request.QueryString["Status"]) == "Ack")
                {
                    dv.RowFilter = "Code IN ('RTRQ','RTN')";
                    lblStatus1.Text = dv.ToTable().Rows[0]["Status"].ToString();
                    lblStatus2.BackColor = System.Drawing.ColorTranslator.FromHtml(dv.ToTable().Rows[0]["StatusColor"].ToString());

                    lblStatus3.Text = dv.ToTable().Rows[1]["Status"].ToString();
                    lblStatus4.BackColor = System.Drawing.ColorTranslator.FromHtml(dv.ToTable().Rows[1]["StatusColor"].ToString());

                    lblTitle.Text = "File Return/Acknowledge";
                }
                if (dv.ToTable().Rows.Count > 0)
                {
                    ddlFileStatus.DataSource = dv.ToTable();
                    ddlFileStatus.DataTextField = "Status";
                    ddlFileStatus.DataValueField = "Code";
                    ddlFileStatus.DataBind();
                    if (common.myStr(Request.QueryString["Status"]) == "Req")
                    {
                        ddlFileStatus.SelectedValue = "ISS";
                    }
                    else if (common.myStr(Request.QueryString["Status"]) == "Ack")
                    {
                        ddlFileStatus.SelectedValue = "RTRQ";
                    }
                }
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
    private void bindDetailsData()
    {
        BaseC.clsMRD mrd = new BaseC.clsMRD(sConString);
        DataSet ds = new DataSet();
        string sPatientName = "";
        string sRegNo = "";
        string sEncNo = "";
        string sMobileNo = "";
        try
        {
            if (ddlName.SelectedValue == "RN")
            {
                //Added by ujjwal 06 July 2015 to validate UHID start
                if (!txtRegNo.Text.Trim().Length.Equals(0))
                {
                    Int64 UHID;
                    Int64.TryParse(txtRegNo.Text, out UHID);
                    if ((UHID > 9223372036854775807 || UHID.Equals(0)))
                    {
                        Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
                        return;
                    }
                }
                //Added by ujjwal 06 July 2015 to validate UHID start
                sRegNo = txtRegNo.Text;
            }
            else if (ddlName.SelectedValue == "NM")
            {
                sPatientName = txtSearch.Text;
            }
            else if (ddlName.SelectedValue == "EN")
            {
                sEncNo = txtSearch.Text;
            }
            else if (ddlName.SelectedValue == "MN")
            {
                sMobileNo = txtSearch.Text;
            }

            ds = GetMRDIssueFile(0, 0, common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue), common.myStr(ddlFileStatus.SelectedValue),
                            common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value),
                            sPatientName, sRegNo, sEncNo, sMobileNo);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvData.DataSource = ds.Tables[0];
                    gvData.DataBind();
                }
                else
                {
                    BindBlankGrid();
                }
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
            mrd = null;
            sPatientName = "";
            sRegNo = "";
            sEncNo = "";
            sMobileNo = "";
        }
    }

    public DataSet GetMRDIssueFile(int StatusId, int RegistrationId, int HospitalLocationId, int FacilityId,
                                 string OPIP, string StatusType, DateTime FromDate, DateTime ToDate,
            string sPatientName, string sRegNo, string sEncNo, string sMobileNo)
    {
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
        string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

        hstInput.Add("@intStatusId", 0);
        hstInput.Add("@intRegistrationId", RegistrationId);
        hstInput.Add("@inyHospitalLocationId", HospitalLocationId);
        hstInput.Add("@intFacilityId", FacilityId);
        hstInput.Add("@chvOPIP", OPIP);
        hstInput.Add("@chvStatusType", StatusType);
        hstInput.Add("@dtFromDate", fDate);
        hstInput.Add("@dtToDate", tDate);

        hstInput.Add("@chvPatientName", sPatientName);
        hstInput.Add("@chvRegistrationNo", sRegNo);
        hstInput.Add("@chvEncounterNo", sEncNo);
        hstInput.Add("@chvMobileNo", sMobileNo);


        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDIssueFile", hstInput);
        return ds;
    }
    private void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("StatusCode");

        dt.Columns.Add("DisChargeDate");
        dt.Columns.Add("ReturnedDate");
        dt.Columns.Add("ReceivedDate");

        dt.Columns.Add("RequestId");
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("PatientAgeGender");
        dt.Columns.Add("DepartmentName");
        dt.Columns.Add("RequestedBy");
        dt.Columns.Add("WardNo");
        dt.Columns.Add("BedNo");
        dt.Columns.Add("ReturnedBy");
        dt.Columns.Add("ReceivedBy");
        dt.Columns.Add("IssueID");
        dt.Columns.Add("StatusColor");
        DataRow dr = dt.NewRow();
        dr["StatusCode"] = "";
       
        dr["DisChargeDate"] = "";
        dr["ReturnedDate"] = "";
        dr["ReceivedDate"] = "";

        dr["RequestId"] = 0;
        dr["RegistrationId"] = 0;
        dr["EncounterId"] = 0;
        dr["RegistrationNo"] = "";
        dr["EncounterNo"] = "";
        dr["PatientName"] = "";
        dr["PatientAgeGender"] = "";
        dr["DepartmentName"] = "";
        dr["RequestedBy"] = 0;
        dr["WardNo"] = "";
        dr["BedNo"]="";
        dr["ReturnedBy"] = "";
        dr["ReceivedBy"] = "";
        dr["IssueID"] = 0;
        dr["StatusColor"] = "";
        dt.Rows.Add(dr);
        gvData.DataSource = dt;
        gvData.DataBind();
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
        BaseC.clsMRD MRD = new BaseC.clsMRD(sConString);
        Hashtable hshOut = new Hashtable();
        try
        {
            if (e.CommandName == "RetrunFileRequest")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnIssueId = (HiddenField)row.FindControl("hdnIssueId");
                if (common.myInt(hdnIssueId.Value) != 0)
                {
                    HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                    HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                    if (common.myStr(Request.QueryString["Status"]) == "Ack")
                    {
                        hshOut = MRD.SaveFileReturn(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                          common.myInt(Session["UserId"]), common.myInt(hdnIssueId.Value));
                    }
                    else
                    {
                        if (common.myStr(ViewState["Mpg"]) == "")
                        {
                            hshOut = MRD.UpdateRequestReturnStatus(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                            common.myInt(hdnIssueId.Value),true,common.myInt(Session["UserId"]));
                        }
                        else
                        {
                            hshOut = MRD.UpdateRequestReturnStatus(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                               common.myInt(hdnIssueId.Value), false, common.myInt(Session["UserId"]));
                        }
                    }
                    lblMessage.Text = hshOut["@chvOutPut"].ToString();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    bindDetailsData();
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select file", Page);
                    return;
                }
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
            MRD = null;
            hshOut = null;
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
            objMRD = new BaseC.RestFulAPI(sConString);
            string strMsg = objMRD.UpdateMRDFileStatus(common.myInt(hdnGRequestId.Value), common.myInt(menuStatus.SelectedValue),
                                    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), sConString,"");
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
    //Added on 02-09-2014 Start Naushad Ali
    protected void btnYes_OnClick(object sender, EventArgs e)
    {

        dvConfirm.Visible = false;

    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirm.Visible = false;
    }

    //Added on 02-09-2014 End Naushad Ali
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lblAck = (LinkButton)e.Row.FindControl("lblAck");
            HiddenField hdnStatusColor = (HiddenField)e.Row.FindControl("hdnStatusColor");
            if (common.myStr(hdnStatusColor.Value) != "")
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(hdnStatusColor.Value);
            }
            if (common.myStr(Request.QueryString["Status"]) == "Ack")
            {
                lblAck.Text = "Return Acknowledge";
            }
        }
    }

    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtSearch.Text = "";

        if (common.myStr(ddlName.SelectedValue) == "RN")
        {
            txtSearch.Visible = false;
            txtRegNo.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            txtRegNo.Visible = false;
        }

    }


}
