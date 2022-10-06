using System;
using System.Text;
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

public partial class Appointment_CancelAppointment : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet ds;
    BaseC.RestFulAPI objCommon ;

    protected void Page_Load(object sender, EventArgs e)
    {
        objCommon = new BaseC.RestFulAPI(sConString);
        lblMessage.Text = string.Empty;
        if (!IsPostBack)
        {
            BindProvider();
            BindFacility();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            dtpMoveDate.SelectedDate = DateTime.Now;
            tdMoveDate.Visible = false;
            tdMovePro.Visible = false;
            BindBlnkGrid();
        }
    }
    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        DataColumn dc = new DataColumn("AppointmentId", typeof(int));
        dc.DefaultValue = 0;
        Dt.Columns.Add(dc);
        Dt.Columns.Add("RegistrationNo", typeof(string));
        Dt.Columns.Add("Name", typeof(int));
        Dt.Columns.Add("AgeGender", typeof(string));
        Dt.Columns.Add("FromTime", typeof(string));
        Dt.Columns.Add("ToTime", typeof(string));
        Dt.Columns.Add("Mobile", typeof(string));
        Dt.Columns.Add("Email", typeof(string));
        Dt.Columns.Add("Remarks", typeof(bool));
        Dt.Columns.Add("DateOfAppointment", typeof(DateTime));
        Dt.Columns.Add("StatusColor", typeof(DateTime));
        Dt.Columns.Add("StatusCode", typeof(DateTime));
        return Dt;
    }
    private void BindBlnkGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            DataRow dr;
            dr = dT.NewRow();
            dr[1] = DBNull.Value;
            dr[2] = DBNull.Value;
            dr[3] = DBNull.Value;
            dr[4] = DBNull.Value;
            dr[5] = DBNull.Value;
            dr[6] = DBNull.Value;
            dr[7] = DBNull.Value;
            dr[8] = DBNull.Value;
            dr[9] = DBNull.Value;
            dr[10] = DBNull.Value;
            dr[11] = DBNull.Value;
            dT.Rows.Add(dr);
            gvAppointment.DataSource = dT;
            gvAppointment.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindProvider()
    {

        try
        {
            BaseC.Hospital hospital = new BaseC.Hospital(sConString);
            ds = new DataSet();
            ds = hospital.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationId"]),0,0);
            ddlProvider.Items.Clear();
            ddlProvider.DataSource = ds.Tables[0];
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataBind();
            ddlProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlProvider.ClearSelection();

            ddlMoveProvider.Items.Clear();
            ddlMoveProvider.DataSource = ds.Tables[0];
            ddlMoveProvider.DataValueField = "DoctorId";
            ddlMoveProvider.DataTextField = "DoctorName";
            ddlMoveProvider.DataBind();
            ddlMoveProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlMoveProvider.ClearSelection();
            
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindFacility()
    {
        try
        {
            ds = new DataSet();
            ds = objCommon.GetFacilityList(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(Session["USerId"]), 0, 0);
            ddlFacility.Items.Clear();
            ddlFacility.DataSource = ds.Tables[0];
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            ddlFacility.ClearSelection();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindAppointment()
    {
        try
        {
            if (ddlProvider.SelectedValue != "0" && ddlProvider.SelectedValue != "")
            {
                ds = new DataSet();
                BaseC.Dashboard appointment = new BaseC.Dashboard();
                ds = appointment.getPDAppointment(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(ddlProvider.SelectedValue), "4",
                    Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd"), Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy/MM/dd"), Convert.ToInt32(ddlFacility.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvAppointment.DataSource = ds.Tables[0];
                    gvAppointment.DataBind();
                }
                else
                {
                    BindBlnkGrid();
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Doctor", Page);
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
    protected void ddlOptions_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOptions.SelectedValue == "M")
        {
            tdMoveDate.Visible = true;
            tdMovePro.Visible = true;
            chkMoveDate.Checked = true;
            dtpMoveDate.SelectedDate = null;
        }
        else
        {
            tdMoveDate.Visible = false;
            tdMovePro.Visible = false;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        if (ddlFacility.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please select Facility", Page);
            return;
        }
        if (ddlProvider.SelectedValue == "0" || ddlProvider.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please select Provider", Page);
            return;
        }
        BindAppointment();
    }
    protected void btnCancelMove_Click(object sender, EventArgs e)
    {
        Hashtable hshOut = new Hashtable();
        BaseC.Appointment appointment = new BaseC.Appointment(sConString);
        StringBuilder sb = new StringBuilder();
        int iMoveDoctorId = 0;
        string sMoveDate = "";
        try
        {
            if (ddlOptions.SelectedValue != "")
            {
                if (txtCancelRemarks.Text == "")
                {
                    Alert.ShowAjaxMsg("Please enter Remarks", Page);
                    return;
                }
                if (ddlProvider.SelectedValue == "" || ddlProvider.SelectedValue == "0")
                {
                    Alert.ShowAjaxMsg("Please select Doctor", Page);
                    return;
                }
                if (ddlOptions.SelectedValue == "M")
                {
                    if (ddlMoveProvider.SelectedValue == ddlProvider.SelectedValue)
                    {
                        Alert.ShowAjaxMsg("Please select different move Doctor", Page);
                        return;
                    }
                    if (ddlMoveProvider.SelectedValue == "" || ddlMoveProvider.SelectedValue == "0")
                    {
                        Alert.ShowAjaxMsg("Please select move Doctor", Page);
                        return;
                    }
                    iMoveDoctorId = common.myInt(ddlMoveProvider.SelectedValue);
                    sMoveDate = chkMoveDate.Checked == false ? Convert.ToDateTime(dtpMoveDate.SelectedDate).Date.ToString("dd/MM/yyyy/") : "";
                    foreach (GridViewRow row in gvAppointment.Rows)
                    {
                        TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
                        TextBox txtMobile = (TextBox)row.FindControl("txtMobile");
                        Label lblPatientName = (Label)row.FindControl("lblPatientName");
                        Label lblAppointmentID = (Label)row.FindControl("lblAppointmentID");
                        CheckBox chkAllItems = (CheckBox)row.FindControl("chkAllItems");
                        CheckBox chkT = (CheckBox)row.FindControl("chkT");
                        if (chkT.Checked == true && (chkAllItems == null || chkAllItems.Checked == false))
                        {
                            sb.Append("<table1><c1>");
                            sb.Append(lblAppointmentID.Text);
                            sb.Append("</c1><c2>");
                            sb.Append(txtMobile.Text);
                            sb.Append("</c2><c3>");
                            sb.Append(txtEmail.Text);
                            sb.Append("</c3></table1>");
                        }
                        else if (chkAllItems != null && chkAllItems.Checked == true)
                        {
                            sb.Append("<table1><c1>");
                            sb.Append(lblAppointmentID.Text);
                            sb.Append("</c1><c2>");
                            sb.Append(txtMobile.Text);
                            sb.Append("</c2><c3>");
                            sb.Append(txtEmail.Text);
                            sb.Append("</c3></table1>");
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow row in gvAppointment.Rows)
                    {
                        Label lblAppointmentID = (Label)row.FindControl("lblAppointmentID");
                        TextBox txtMobile = (TextBox)row.FindControl("txtMobile");
                        TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
                        CheckBox chkAllItems = (CheckBox)row.FindControl("chkAllItems");
                        CheckBox chkT = (CheckBox)row.FindControl("chkT");
                        if (chkT.Checked == true && (chkAllItems == null || chkAllItems.Checked == false))
                        {
                            if (txtMobile.Text != "" || txtEmail.Text != "")
                            {
                                sb.Append("<table1><c1>");
                                sb.Append(lblAppointmentID.Text);
                                sb.Append("</c1><c2>");
                                sb.Append(txtMobile.Text);
                                sb.Append("</c2><c3>");
                                sb.Append(txtEmail.Text);
                                sb.Append("</c3></table1>");
                            }
                        }
                        else if (chkAllItems != null && chkAllItems.Checked == true)
                        {
                            if (txtMobile.Text != "" || txtEmail.Text != "")
                            {
                                sb.Append("<table1><c1>");
                                sb.Append(lblAppointmentID.Text);
                                sb.Append("</c1><c2>");
                                sb.Append(txtMobile.Text);
                                sb.Append("</c2><c3>");
                                sb.Append(txtEmail.Text);
                                sb.Append("</c3></table1>");
                            }
                        }
                    }
                }
                if (sb.ToString() != "")
                {
                    if (ddlOptions.SelectedValue == "M")
                    {
                        hshOut = appointment.AppointmentsCancelAndUpdate(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
                        common.myInt(ddlProvider.SelectedValue), iMoveDoctorId, sMoveDate, sb.ToString(), common.myStr(txtCancelRemarks.Text),
                        common.myStr(ddlOptions.SelectedValue), common.myInt(Session["UserId"]));
                    }
                    else
                    {
                        hshOut = appointment.AppointmentsCancelAndUpdate(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
                         common.myInt(ddlProvider.SelectedValue), 0, "", sb.ToString(), common.myStr(txtCancelRemarks.Text),
                          common.myStr(ddlOptions.SelectedValue), common.myInt(Session["UserId"]));
                    }
                    lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select the appointment", Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please select appointment Option", Page);
                return;
            }
            BindAppointment();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            hshOut =null;
            appointment = null;
            sb = null;
            iMoveDoctorId = 0;
            sMoveDate = "";
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Hashtable hshOut = new Hashtable();
        BaseC.Appointment appointment = new BaseC.Appointment(sConString);
        StringBuilder sb = new StringBuilder();

        foreach (GridViewRow row in gvAppointment.Rows)
        {
            Label lblAppointmentID = (Label)row.FindControl("lblAppointmentID");
            TextBox txtMobile = (TextBox)row.FindControl("txtMobile");
            TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
            if (txtMobile.Text != "" || txtEmail.Text != "")
            {
                sb.Append("<table1><c1>");
                sb.Append(lblAppointmentID.Text);
                sb.Append("</c1><c2>");
                sb.Append(txtMobile.Text);
                sb.Append("</c2><c3>");
                sb.Append(txtEmail.Text);
                sb.Append("</c3></table1>");
            }
        }
        if (sb.ToString() != "")
        {
            hshOut = appointment.AppointmentsCancelAndUpdate(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(ddlFacility.SelectedValue),
                 Convert.ToInt32(ddlProvider.SelectedValue), ddlMoveProvider.SelectedValue == "" ? 0 : Convert.ToInt32(ddlMoveProvider.SelectedValue),"", sb.ToString(), Convert.ToString(txtCancelRemarks.Text), "U", Convert.ToInt32(Session["UserId"]));
            lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        BindAppointment();
    }
    protected void gvAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            HiddenField hdnColor=(HiddenField)e.Row.FindControl("hdnStatusColor");
            HiddenField hdnStatusCode = (HiddenField)e.Row.FindControl("hdnStatusCode");
            CheckBox chkT = (CheckBox)e.Row.FindControl("chkT");
            String AppStatusColor = hdnColor.Value;
            if (hdnColor.Value != "")
            {
                if (hdnStatusCode.Value == "U" || hdnStatusCode.Value == "A")
                {
                    chkT.Visible = true;
                }
                else
                {
                    chkT.Visible = false;

                }
                Label lbl_StatusColor = new Label();
                if (AppStatusColor.ToString().Trim() != "Blank" && AppStatusColor.ToString().Trim() != "&nbsp;")
                {
                    lbl_StatusColor.BackColor = System.Drawing.ColorTranslator.FromHtml(AppStatusColor.Trim());

                    A = lbl_StatusColor.BackColor.A;
                    B = lbl_StatusColor.BackColor.B;
                    R = lbl_StatusColor.BackColor.R;
                    G = lbl_StatusColor.BackColor.G;
                    htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlHexColorValue.ToString());
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAllItems")).Attributes.Add("onclick", "javascript:SelectAll('" +
                    ((CheckBox)e.Row.FindControl("chkAllItems")).ClientID + "','" + gvAppointment.ClientID + "')");

        }
    }
    protected void chkMoveDate_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkMoveDate.Checked == true)
        {
            dtpMoveDate.Enabled = false;
            dtpMoveDate.SelectedDate = null;
        }
        else
        {
            dtpMoveDate.Enabled = true;
            dtpMoveDate.SelectedDate = System.DateTime.Now;
        }
    }
    
}