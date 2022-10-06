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

public partial class EMRReports_Doctorwiseprocedure : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private bool Stauts = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDoctor();
            FillEntrySite();
            ViewState["chkAllDep"] = false;
            cbIsBilled.Visible = false;
            dtpfromdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            if (common.myStr(Request.QueryString["RN"]) == "AppReport")
            {
                lblHeader.Text = "Appointment Report";
                lblReportType.Visible = false;
                ddlReportType.Visible = false;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "AppList")
            {
                lblHeader.Text = "Appointment List";

            }
            else
            {
                lblHeader.Text = "Doctor wise Services";
                cbIsBilled.Visible = true;
                ddlReportType.Visible = false;
                lblReportType.Visible = false;
            }
        }

    }
    protected void btnPrintData_OnClick(object sender, EventArgs e)
    {
        string setformula = "";

        if (common.myStr(dtpfromdate.SelectedDate) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter From date !";
            dtpfromdate.Focus();
            return;
        }
        if (common.myStr(dtpTodate.SelectedDate) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter To date !";
            dtpTodate.Focus();
            return;
        }



        if (common.myStr(Request.QueryString["RN"]) == "AppReport")
        {
            foreach (GridDataItem item in gvReporttype.Items)
            {
                if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                {
                    if (setformula == "")
                        setformula = ((Label)item.FindControl("lblId")).Text;
                    else
                        setformula = setformula + "," + ((Label)item.FindControl("lblId")).Text;
                }
            }
            ViewState["setformula"] = setformula;

            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select doctor !";
                return;
            }

            RadWindowForNew.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&DoctorId=" + setformula + "&Export=" + chkExport.Checked + "&ReportName=AppReport";
        }
        else if (common.myStr(Request.QueryString["RN"]) == "AppList")
        {
            foreach (GridDataItem item in gvReporttype.Items)
            {
                if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                {
                    if (setformula == "")
                        setformula = "<Table1><c1>" + ((Label)item.FindControl("lblId")).Text + "</c1></Table1>";
                    else
                        setformula = setformula + "<Table1><c1>" + ((Label)item.FindControl("lblId")).Text + "</c1></Table1>";
                }
            }


            ViewState["setformula"] = setformula;

            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select doctor !";
                return;
            }

            Session["DoctorId"] = common.myStr(setformula);
            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate="
                + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export="
                + chkExport.Checked + "&ReportName=AppList" + "&ReportType=" + ddlReportType.SelectedValue;
        }
        else
        {
            if (common.myBool(ViewState["chkAllDep"]) == false)
            {
                foreach (GridDataItem item in gvReporttype.Items)
                {
                    if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                    {
                        if (setformula == "")
                            setformula = ((Label)item.FindControl("lblId")).Text;
                        else
                            setformula = setformula + "," + ((Label)item.FindControl("lblId")).Text;
                    }
                }
                ViewState["setformula"] = setformula;
            }
            else
            {
                setformula = "A";
            }


            if (common.myStr(setformula) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select doctor !";
                return;
            }
            int IsBilled = 0;
            if (cbIsBilled.Checked == true)
                IsBilled = 1;
            int IsDetail = 0;
            if (cbIsDetail.Checked == true)
                IsDetail = 1;


            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + dtpfromdate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&Todate=" + dtpTodate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&DoctorId=" + setformula + "&Export=" + chkExport.Checked + "&IsBilled=" + IsBilled + "&IsDetail=" + IsDetail + "&EntrySite=" + ddlEntrySite.SelectedValue;
        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    private void FillEntrySite()
    {
        try
        {
            int FacilityId;
            FacilityId = common.myInt(Session["FacilityId"]);
            BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
            DataSet ds = obj.getEntrySite(Convert.ToInt16(Session["UserID"]), FacilityId);
            int EntrySiteIdx = ddlEntrySite.SelectedIndex;
            ddlEntrySite.DataSource = ds.Tables[0];
            ddlEntrySite.DataValueField = "ESId";
            ddlEntrySite.DataTextField = "ESName";
            ddlEntrySite.DataBind();
            ddlEntrySite.SelectedIndex = EntrySiteIdx;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    protected void chkAllDepartment_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Stauts = true;
            CheckBox lbtn = sender as CheckBox;
            GridTableRow row = lbtn.NamingContainer as GridTableRow;
            CheckBox chkAllDep = (CheckBox)row.FindControl("chkAllDepartment");
            ViewState["chkAllDep"] = ((CheckBox)row.FindControl("chkAllDepartment")).Checked;
            if (chkAllDep.Checked == true)
            {

                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = true;
                }
            }
            else
            {
                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = false;
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

    protected void gvReporttype_PreRender(object sender, EventArgs e)
    {
        if (Stauts == false)
        {

            BindDoctor();


        }
    }
    protected void BindDoctor()
    {
        BaseC.clsLISExternalCenter objM = new BaseC.clsLISExternalCenter(sConString);
        //DataTable tbl = objM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, 0, common.myInt(Session["UserID"]));
        DataTable tbl = objM.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);
        if (tbl.Rows.Count > 0)
        {
            tbl.Columns[0].ColumnName = "Id";
            tbl.Columns[1].ColumnName = "Name";

            gvReporttype.DataSource = tbl;
            gvReporttype.DataBind();
        }

    }

    protected void chkDepartment_OnCheckedChanged(object sender, EventArgs e)
    {
        //CheckBox lbtn = sender as CheckBox;
        //GridTableRow row = lbtn.NamingContainer as GridTableRow;
        //CheckBox chkAllDep = (CheckBox)row.FindControl("chkDepartment");
        ViewState["chkAllDep"] = false;
    }
}
