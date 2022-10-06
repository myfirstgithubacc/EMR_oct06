using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class ICM_OnCologyScheduleRegister : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    string FromDate = string.Empty, ToDate = string.Empty;
    string PatientName = string.Empty;
    Int64 RegistrationNo = 0;
    string OldRegistrationNo = string.Empty;
    string EnrolleNo = string.Empty;
    string MobileNo = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["MRDNO"]) && !string.IsNullOrEmpty(Request.QueryString["Encounter"]))
            {
                FromDate = "1900/01/01";
                ToDate = "2079/01/01";
                int RegistrationId = common.myInt(Request.QueryString["MRDNO"]);
                int EncounterId = common.myInt(Request.QueryString["Encounter"]);
                BindOncologyDetails(RegistrationId, EncounterId, FromDate, ToDate);
                if (common.myStr(ddlName.SelectedValue) == "R")
                {
                    txtSearch.Visible = false;
                    txtSearchN.Visible = true;
                }
                else
                {
                    txtSearch.Visible = true;
                    txtSearchN.Visible = false;
                }
            }
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        int RegistrationId = 0; int EncounterId = 0;
        FromDate = string.IsNullOrEmpty(common.myStr(dtpfrmdate.SelectedDate)) ? "1900/01/01" : Convert.ToDateTime(dtpfrmdate.SelectedDate).ToString("yyyy/MM/dd");
        ToDate = string.IsNullOrEmpty(common.myStr(dtpTodate.SelectedDate)) ? "2079/01/01" : Convert.ToDateTime(dtpTodate.SelectedDate).ToString("yyyy/MM/dd");
        BindOncologyDetails(RegistrationId, EncounterId, FromDate, ToDate);
    }

    private void BindOncologyDetails(int RegistrationId, int EncounterId, string FromDate, string ToDate)
    {
        BaseC.clsEMR objIcm = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {

            int Cycle = 0;
            switch (common.myStr(ddlName.SelectedValue))
            {
                case "R":
                    RegistrationNo = common.myLong(txtSearchN.Text);
                    break;
                case "N":
                    PatientName = common.myStr(txtSearch.Text).Trim();
                    break;

            }
            ds = objIcm.GetEMRChemoTherapyCycleDetails(common.myDate(FromDate), common.myDate(ToDate), common.myInt(Cycle), RegistrationId, EncounterId, PatientName, MobileNo, RegistrationNo);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();

            }
            DataView DV = ds.Tables[0].DefaultView;
            DV.Sort = "CycleIntervalDate ASC";
            grvOncologyScheduleRegister.DataSource = ds.Tables[0];
            grvOncologyScheduleRegister.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objIcm = null;
            ds.Dispose();
        }
    }

    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["MRDNO"]) && !string.IsNullOrEmpty(Request.QueryString["Encounter"]))
        {
            FromDate = "1900/01/01";
            ToDate = "2079/01/01";
            int RegistrationId = common.myInt(Request.QueryString["MRDNO"]);
            int EncounterId = common.myInt(Request.QueryString["Encounter"]);
            BindOncologyDetails(RegistrationId, EncounterId, FromDate, ToDate);
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        txtSearch.Text = string.Empty;
        txtSearchN.Text = string.Empty;
        dtpfrmdate.SelectedDate = null;
        dtpTodate.SelectedDate = null;
    }

    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearch.Visible = false;
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            txtSearchN.Visible = false;
        }
    }

    protected void btnPrintData_OnClick(object sender, EventArgs e)
    {
        int RegistrationId = 0;
        int EncounterId = 0;
        FromDate = string.IsNullOrEmpty(common.myStr(dtpfrmdate.SelectedDate)) ? "1900/01/01" : Convert.ToDateTime(dtpfrmdate.SelectedDate).ToString("yyyy/MM/dd");
        ToDate = string.IsNullOrEmpty(common.myStr(dtpTodate.SelectedDate)) ? "2079/01/01" : Convert.ToDateTime(dtpTodate.SelectedDate).ToString("yyyy/MM/dd");
        if (string.IsNullOrEmpty(common.myStr(dtpfrmdate.SelectedDate)) && string.IsNullOrEmpty(common.myStr(dtpfrmdate.SelectedDate)))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["MRDNO"]) && !string.IsNullOrEmpty(Request.QueryString["Encounter"]))
            {
                RegistrationId = common.myInt(Request.QueryString["MRDNO"]);
                EncounterId = common.myInt(Request.QueryString["Encounter"]);
            }
        }
        else
        {
            switch (common.myStr(ddlName.SelectedValue))
            {
                case "R":
                    RegistrationNo = common.myLong(txtSearchN.Text);
                    break;
                case "N":
                    PatientName = common.myStr(txtSearch.Text).Trim();
                    break;

            }
        }
        string Schedulerpt = "Register";
        RadWindowForReport.NavigateUrl = "/EMRReports/OncologySchedulerpt.aspx?RegId=" + RegistrationId + "&Encounter=" + EncounterId + "&fromDate=" + FromDate + "&toDate=" + ToDate + "&Regno=" + RegistrationNo + "&cvName=" + PatientName + "&Schedulerpt=" + Schedulerpt;
        RadWindowForReport.Height = 580;
        RadWindowForReport.Width = 1130;
        RadWindowForReport.Top = 10;
        RadWindowForReport.Left = 10;
        RadWindowForReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForReport.Modal = true;
        RadWindowForReport.VisibleStatusbar = false;
    }

    protected void grvOncologyScheduleRegister_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvOncologyScheduleRegister.PageIndex = e.NewPageIndex;
        int RegistrationId = 0; int EncounterId = 0;
        FromDate = string.IsNullOrEmpty(common.myStr(dtpfrmdate.SelectedDate)) ? "1900/01/01" : Convert.ToDateTime(dtpfrmdate.SelectedDate).ToString("yyyy/MM/dd");
        ToDate = string.IsNullOrEmpty(common.myStr(dtpTodate.SelectedDate)) ? "2079/01/01" : Convert.ToDateTime(dtpTodate.SelectedDate).ToString("yyyy/MM/dd");
        BindOncologyDetails(RegistrationId, EncounterId, FromDate, ToDate);
    }
}

