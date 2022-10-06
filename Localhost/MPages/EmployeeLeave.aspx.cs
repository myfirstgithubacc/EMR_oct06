using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MPages_EmployeeLeave : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objCommon;//= new wcf_Service_Common.CommonMasterClient();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objCommon = new BaseC.RestFulAPI(sConString);
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            BlankGrid();
            BindEmployees();
        }
    }
    private void BindEmployees()
    {
        DataSet ds = objCommon.SearchEmployeeByName(common.myInt(Session["HospitalLocationId"]), "");
        ddlEmployee.DataSource = ds.Tables[0];
        ddlEmployee.DataValueField = "EmployeeId";
        ddlEmployee.DataTextField = "EmployeeName";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
        ddlEmployee.Items[0].Value = "0";
        
    }

    protected void btnSaveEmployeeLeave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlEmployee.SelectedValue == "0" || ddlEmployee.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please select Employee", Page);
                return;
            }
            if (txtRemarks.Text == "")
            {
                Alert.ShowAjaxMsg("Please type Remarks", Page);
                return;
            }
            BaseC.Hospital hospital = new BaseC.Hospital(sConString);
            Hashtable hshOutput = hospital.SaveUpdateHospitalEmployeeLeaves(0, Convert.ToInt32(ddlEmployee.SelectedValue),
                 Convert.ToDateTime(dtpFromDate.SelectedDate).Date.ToString("yyyy/MM/dd"), Convert.ToDateTime(dtpToDate.SelectedDate).Date.ToString("yyyy/MM/dd"),
                 txtRemarks.Text, Convert.ToInt32(Session["UserId"]));
            lblMessage.Text = hshOutput["@chvErrorOutput"].ToString();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindEmployeeLeave(ddlEmployee.SelectedValue);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEmployeeLeaves_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int iLeaveId = Convert.ToInt32(gvEmployeeLeaves.DataKeys[e.RowIndex].Value);

            TextBox txtRemarks = (TextBox)gvEmployeeLeaves.Rows[e.RowIndex].Cells[0].FindControl("txtRemarks");

            RadDatePicker dtpFromDate = (RadDatePicker)gvEmployeeLeaves.Rows[e.RowIndex].Cells[0].FindControl("dtpFromDate");
            RadDatePicker dtpToDate = (RadDatePicker)gvEmployeeLeaves.Rows[e.RowIndex].Cells[0].FindControl("dtpToDate");

            if (Convert.ToInt32(FindCurrentDate(Convert.ToDateTime(System.DateTime.Now).Date.ToString("dd/MM/yyyy"))) >= 
                Convert.ToInt32(FindFutureDate(Convert.ToDateTime(dtpFromDate.SelectedDate).Date.ToString("dd/MM/yyyy"))))
            {
                Alert.ShowAjaxMsg("Old Record(s) can not be Updated", Page);
                return;
            }


            BaseC.Hospital hospital = new BaseC.Hospital(sConString);
            Hashtable hshOutput = hospital.SaveUpdateHospitalEmployeeLeaves(iLeaveId, Convert.ToInt32(ddlEmployee.SelectedValue),
                 Convert.ToDateTime(dtpFromDate.SelectedDate).Date.ToString("yyyy/MM/dd"), Convert.ToDateTime(dtpToDate.SelectedDate).Date.ToString("yyyy/MM/dd"),
                 txtRemarks.Text, Convert.ToInt32(Session["UserId"]));
            lblMessage.Text = hshOutput["@chvErrorOutput"].ToString();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            gvEmployeeLeaves.EditIndex = -1;
            BindEmployeeLeave(ddlEmployee.SelectedValue);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEmployeeLeaves_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvEmployeeLeaves.EditIndex = e.NewEditIndex;
        if (ddlEmployee.SelectedValue != "")
        {
            BindEmployeeLeave(ddlEmployee.SelectedValue);
        }
    }

    protected void gvEmployeeLeaves_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvEmployeeLeaves.EditIndex = -1;
        BindEmployeeLeave(ddlEmployee.SelectedValue);
    }
    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = "";
        string newCurrentDate = "";
        string currentdate = formatdate.FormatDateDateMonthYear(System.DateTime.Today.ToShortDateString());
        string strformatCurrDate = formatdate.FormatDate(currentdate, "dd/MM/yyyy", "yyyy/MM/dd");
        firstCurrentDate = strformatCurrDate.Remove(4, 1);
        newCurrentDate = firstCurrentDate.Remove(6, 1);
        return newCurrentDate;
    }

    private string FindFutureDate(string outputFutureDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstApptDate = "";
        string NewApptDate = "";
        string strDateAppointment = formatdate.FormatDateDateMonthYear(outputFutureDate);
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "MM/dd/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }
    protected void gvEmployeeLeaves_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (gvEmployeeLeaves.DataKeys[e.RowIndex].Value == "")
            {
                return;
            }
            int iLeaveId = Convert.ToInt32(gvEmployeeLeaves.DataKeys[e.RowIndex].Value);

            Label lblFromDate = (Label)gvEmployeeLeaves.Rows[e.RowIndex].Cells[0].FindControl("lblFromDate");
            Label lblToDate = (Label)gvEmployeeLeaves.Rows[e.RowIndex].Cells[0].FindControl("lblToDate");

            if(Convert.ToInt32(FindCurrentDate(Convert.ToDateTime(System.DateTime.Now).Date.ToString("dd/MM/yyyy")))>=Convert.ToInt32(FindFutureDate(lblFromDate.Text)))
            {
                Alert.ShowAjaxMsg("Old Record(s) can not be Deleted", Page);
                return;
            }
            BaseC.Hospital hospital = new BaseC.Hospital(sConString);
            int i = hospital.DeleteHospitalEmployeeLeave(Convert.ToInt32(ddlEmployee.SelectedValue),iLeaveId, Convert.ToInt32(Session["UserId"]));
            if (i == 0)
            {
                lblMessage.Text = "Record(s) deleted";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            BindEmployeeLeave(ddlEmployee.SelectedValue);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        ddlEmployee.SelectedValue = "0";
        ddlEmployee.Text = "";
        txtRemarks.Text = "";
        BlankGrid();
    }
    protected void ddlEmployee_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEmployee.SelectedValue != "" && ddlEmployee.SelectedValue!="0")
            {
                BindEmployeeLeave(ddlEmployee.SelectedValue);
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Employee", Page);
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
    private void BindEmployeeLeave(string sEmployeeId)
    {
        StringBuilder sb = new StringBuilder();
        BaseC.Hospital hospital = new BaseC.Hospital(sConString);
        sb.Append("<Table1><c1>" + Convert.ToInt32(ddlEmployee.SelectedValue) + "</c1></Table1>");
        DataSet ds = hospital.GetHospitalEmployeeLeave(sb.ToString(), "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvEmployeeLeaves.DataSource = ds.Tables[0];
            gvEmployeeLeaves.DataBind();
        }
        else
        {
            BlankGrid();
        }
    }
    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("LeaveId");
        dt.Columns.Add("EmployeeID");
        dt.Columns.Add("EmployeeName");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("Active");
        dt.Columns.Add("FromDate");
        dt.Columns.Add("ToDate");

        dt.Columns.Add("DisplayFromDate");
        dt.Columns.Add("DisplayToDate");

        DataRow dr = dt.NewRow();
        dr["LeaveId"]="";
        dr["EmployeeID"]="";
        dr["EmployeeName"]="";
        dr["Remarks"]="";
        dr["Active"]="";
        dr["FromDate"]="";
        dr["ToDate"]="";

        dr["DisplayFromDate"] = "";
        dr["DisplayToDate"] = "";

        dt.Rows.Add(dr);
        gvEmployeeLeaves.DataSource = dt;
        gvEmployeeLeaves.DataBind();
    }
}
