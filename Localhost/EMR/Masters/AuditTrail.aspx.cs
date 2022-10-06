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
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_Masters_AuditTrail : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.RestFulAPI objCommon;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.Security audit;

    public enum enumAudit
    {
        SNo = 0,
        Regno = 1,
        EncNo = 2,
        PatientName = 3,
        ServiceName = 4,
        CompanyName = 5,
        EmployeeNo = 6,
        EmployeeName = 7,
        FacilityName = 8,
        FieldName = 9,
        OldValue = 10,
        NewValue = 11,
        LastChangedBy = 12,
        LastChangedDate = 13
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objCommon = new BaseC.RestFulAPI(sConString);
        try
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }

                audit = new BaseC.Security(sConString);

                dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
                dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
                dtpfromDate.SelectedDate = System.DateTime.Now;
                dtpToDate.SelectedDate = System.DateTime.Now;

                bindControl();
                setOption();
                bindDetailsData();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            ds = new DataSet();
            ds = objCommon.GetFacilityList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]), 0, 0);
            
            ddlFacility.DataSource = ds.Tables[0];
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();

            ddlFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));
            

            objCommon = new BaseC.RestFulAPI(sConString);
            ds = objCommon.GetHospitalServices(common.myInt(Session["HospitallocationId"]), 0, 0, "", "", common.myInt(Session["FacilityId"]),0,0);

            ddlServiceId.DataSource = ds.Tables[0];
            ddlServiceId.DataValueField = "ServiceId";
            ddlServiceId.DataTextField = "ServiceName";
            ddlServiceId.DataBind();

            ddlServiceId.Items.Insert(0, new RadComboBoxItem("All", "0"));
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

    void clearControl()
    {
        lblMessage.Text = "";
        txtSearch.Text = "";
        ddlFacility.SelectedIndex = 0;
        ddlServiceId.SelectedIndex = 0;
        ddlSearchOn.SelectedIndex = 0;
        ddlOptions.SelectedIndex = 0;

        dtpfromDate.SelectedDate = System.DateTime.Now;
        dtpToDate.SelectedDate = System.DateTime.Now;
    }

    protected void bindDetailsData()
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlRange.SelectedValue == "4")
            {
                if (dtpfromDate.SelectedDate == null || dtpToDate.SelectedDate == null)
                {
                    Alert.ShowAjaxMsg("Select From and To Date", Page);
                    return;
                }
            }
            if (ddlRange.SelectedValue == "WW-2")
            {
                dtpfromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now.AddDays(-14);
            }
            else if (ddlRange.SelectedValue == "DD0")
            {
                dtpfromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (ddlRange.SelectedValue == "WW-1")
            {
                dtpfromDate.SelectedDate = DateTime.Now.AddDays(-7);
                dtpToDate.SelectedDate = DateTime.Now;
            }
            else if (ddlRange.SelectedValue == "YY-1")
            {
                dtpfromDate.SelectedDate = DateTime.Now.AddYears(-1);
                dtpToDate.SelectedDate = DateTime.Now;
            }
            audit = new BaseC.Security(sConString);

            if (common.myStr(ddlOptions.SelectedValue) == "T")
            {
                txtSearch.Text = common.myStr(common.myInt(ddlServiceId.SelectedValue));
            }

            ds = AccessAuditDetails(common.myStr(ddlOptions.SelectedValue), common.myInt(Session["HospitalLocationId"]),
                        common.myInt(Session["FacilityId"]), common.myInt(ddlFacility.SelectedValue),
                        common.myStr(ddlSearchOn.SelectedValue), common.myStr(txtSearch.Text).Trim(),
                        ddlRange.SelectedValue == "0" ? null : Convert.ToDateTime(dtpfromDate.SelectedDate).Date.ToString("yyyy/MM/dd"),
                        ddlRange.SelectedValue == "0" ? null : Convert.ToDateTime(dtpToDate.SelectedDate).Date.ToString("yyyy/MM/dd"));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvAudit.DataSource = ds;
            gvAudit.DataBind();

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

    protected void setOption()
    {
        try
        {
            ddlSearchOn.Items.Clear();
            RadComboBoxItem ls = new RadComboBoxItem();
            txtSearch.Visible = false;
            ddlServiceId.Visible = false;

            switch (common.myStr(ddlOptions.SelectedValue))
            {
                case "A"://Admission
                    ls.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "Regno"));
                    ls.Value = "RN";
                    ddlSearchOn.Items.Add(ls);

                    ls = new RadComboBoxItem();
                    ls.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "ipno"));
                    ls.Value = "ENC";
                    ddlSearchOn.Items.Add(ls);

                    txtSearch.Visible = true;

                    break;
                case "C"://Company
                    ls.Text = "Company Name";
                    ls.Value = "CMPN";
                    ddlSearchOn.Items.Add(ls);

                    txtSearch.Visible = true;

                    break;
                case "E"://Employee
                    ls.Text = "EmployeeNo";
                    ls.Value = "EMPNO";
                    ddlSearchOn.Items.Add(ls);

                    ls = new RadComboBoxItem();
                    ls.Text = "Employee Name";
                    ls.Value = "EMPN";
                    ddlSearchOn.Items.Add(ls);

                    txtSearch.Visible = true;

                    break;
                case "T"://Tariff
                    ls.Text = "Service";
                    ls.Value = "IOS";
                    ddlSearchOn.Items.Add(ls);

                    ddlServiceId.Visible = true;

                    break;
                case "R"://Registration
                    ls.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "Regno"));
                    ls.Value = "RN";

                    ddlSearchOn.Items.Add(ls);

                    txtSearch.Visible = true;
                    
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlOptions_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        txtSearch.Text = "";
        ddlServiceId.SelectedIndex = 0;

        setOption();
        bindDetailsData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        bindDetailsData();
    }

    protected void btn_ClearFilter_Click(object sender, EventArgs e)
    {
        clearControl();
        setOption();
        bindDetailsData();
    }

    protected void ddlRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRange.SelectedValue == "4")
        {
            tdDate.Visible = true;
        }
        else
        {
            tdDate.Visible = false;
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            string sRegistratioNo = ddlOptions.SelectedValue == "R" || ddlOptions.SelectedValue == "A" ? txtSearch.Text.Trim() : null;
            string sFromDate = ddlRange.SelectedValue == "0" ? null : Convert.ToDateTime(dtpfromDate.SelectedDate).Date.ToString("yyyy/MM/dd");
            string sToDate = ddlRange.SelectedValue == "0" ? null : Convert.ToDateTime(dtpToDate.SelectedDate).Date.ToString("yyyy/MM/dd");


            RadWindowForNew.NavigateUrl = "~/EMRReports/AuditReport.aspx?RegistrationNo=" + sRegistratioNo
                + "&FacilityId=" + ddlFacility.SelectedValue + "&FromDate=" + sFromDate + "&ToDate=" + sToDate;
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 990;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.Title = "Audit Trail";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.ReloadOnShow = true;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAudit_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAudit.PageIndex = e.NewPageIndex;

        bindDetailsData();
    }

    protected void ddlServiceId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        bindDetailsData();
    }

    protected void gvAudit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblRemark = (Label)e.Row.FindControl("lblRemark");  

            gvAudit.Columns[(byte)enumAudit.EncNo].Visible = false;
            gvAudit.Columns[(byte)enumAudit.Regno].Visible = false;
            gvAudit.Columns[(byte)enumAudit.PatientName].Visible = false;
            gvAudit.Columns[(byte)enumAudit.ServiceName].Visible = false;
            gvAudit.Columns[(byte)enumAudit.CompanyName].Visible = false;
            gvAudit.Columns[(byte)enumAudit.EmployeeNo].Visible = false;
            gvAudit.Columns[(byte)enumAudit.EmployeeName].Visible = false;

            switch (common.myStr(ddlOptions.SelectedValue))
            {
                case "A"://Admission
                    gvAudit.Columns[(byte)enumAudit.EncNo].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.Regno].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.PatientName].Visible = true;

                    break;
                case "C"://Company
                    gvAudit.Columns[(byte)enumAudit.CompanyName].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.FacilityName].Visible = false;

                    break;
                case "E"://Employee
                    gvAudit.Columns[(byte)enumAudit.EmployeeNo].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.EmployeeName].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.FacilityName].Visible = false;

                    break;
                case "T"://Tariff
                    gvAudit.Columns[(byte)enumAudit.ServiceName].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.CompanyName].Visible = true;

                    break;
                case "R"://Registration
                    gvAudit.Columns[(byte)enumAudit.Regno].Visible = true;
                    gvAudit.Columns[(byte)enumAudit.PatientName].Visible = true;

                    break;
            }
        }
    }

    public DataSet AccessAuditDetails(string AuditTrailOption, int HospId, int LogInFacilityId, int FacilityId,
                          string SearchOn, string SearchText, string FromDate, string ToDate)
    {
        Hashtable HshIn = new Hashtable();

        string spName = "uspAuditTrailRegistration";
        switch (AuditTrailOption)
        {
            case "A"://Admission
                spName = "uspAuditTrailAdmission";

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LogInFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (SearchText.Trim().Length > 0)
                {
                    if (SearchOn == "RN")
                    {
                        HshIn.Add("@chvRegistrationNo", SearchText);
                    }
                    else if (SearchOn == "ENC")
                    {
                        HshIn.Add("@chvEncounterNo", SearchText);
                    }
                }

                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                break;
            case "C"://Company
                spName = "uspAuditTrailCompany";

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LogInFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (SearchText.Trim().Length > 0)
                {
                    if (SearchOn == "CMPN")
                    {
                        HshIn.Add("@chvCompanyName", SearchText);
                    }
                }
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                break;
            case "E"://Employee
                spName = "uspAuditTrailEmployee";

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LogInFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (SearchText.Trim().Length > 0)
                {
                    if (SearchOn == "EMPNO")
                    {
                        HshIn.Add("@chvEmployeeNo", SearchText);
                    }
                    else if (SearchOn == "EMPN")
                    {
                        HshIn.Add("@chvEmployeeName", SearchText);
                    }
                }

                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                break;
            case "T"://Tariff
                spName = "uspAuditTrailTariff";

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LogInFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (SearchText.Trim().Length > 0)
                {
                    if (SearchOn == "IOS")
                    {
                        HshIn.Add("@intServiceId", SearchText);
                    }
                }

                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                break;
            case "R"://Registration
                spName = "uspAuditTrailRegistration";

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LogInFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (SearchText.Trim().Length > 0)
                {
                    if (SearchOn == "RN")
                    {
                        HshIn.Add("@chvRegistrationNo", SearchText);
                    }
                }

                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                break;
        }

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, spName, HshIn);


        if (!ds.Tables[0].Columns.Contains("RegistrationNo"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("RegistrationNo"));
        }
        if (!ds.Tables[0].Columns.Contains("EncounterNo"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("EncounterNo"));
        }
        if (!ds.Tables[0].Columns.Contains("PatientName"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("PatientName"));
        }
        if (!ds.Tables[0].Columns.Contains("CompanyName"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("CompanyName"));
        }
        if (!ds.Tables[0].Columns.Contains("EmployeeNo"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("EmployeeNo"));
        }
        if (!ds.Tables[0].Columns.Contains("EmployeeName"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("EmployeeName"));
        }
        if (!ds.Tables[0].Columns.Contains("ServiceName"))
        {
            ds.Tables[0].Columns.Add(new DataColumn("ServiceName"));
        }

        return ds;
    }

}