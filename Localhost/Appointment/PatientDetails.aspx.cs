using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;


public partial class Appointment_PatientDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    BaseC.Appointment Obj;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["Status"]) == "Confm")
            {
                PatientStatus.Text = "Confirm";
            }
            else if (common.myStr(Request.QueryString["Status"]) == "OldAp")
            {
                PatientStatus.Text = "Appointment";
            }
            else if (common.myStr(Request.QueryString["Status"]) == "NewAp")
            {
                PatientStatus.Text = "New Patient";
            }
            else if (common.myStr(Request.QueryString["Status"]) == "ChkIn")
            {
                PatientStatus.Text = "Checked In";
            }
            else if (common.myStr(Request.QueryString["Status"]) == "Cancl")
            {
                PatientStatus.Text = "Cancelled";
            }
            else if (common.myStr(Request.QueryString["Status"]) == "Paym")
            {
                PatientStatus.Text = "Payment";
            }
            getAppointmentStatus();
            BindProvider();
            BindPatientDetail();
        }
    }

    protected void BindPatientDetail()
    {
        string frmDate = common.myStr(Request.QueryString["Date"]);
        string DoctorId = common.myStr(Session["AppDoctor"]);
        string Status = common.myStr(Request.QueryString["Status"]);
        string DateOption = common.myStr(Request.QueryString["DateOption"]);


        string PatientName = string.Empty;
        string ProviderIds = string.Empty;
        string RegistrationNo = "0";
        string EnrolleNo = string.Empty;
        string MobileNo = string.Empty;

        ProviderIds = (common.myInt(ddlProvider.SelectedValue) > 0) ? common.myInt(ddlProvider.SelectedValue).ToString() : "A";


        switch (common.myStr(ddlName.SelectedValue))
        {
            case "R":
                RegistrationNo = common.myStr(txtSearchN.Text);
                break;
            case "N":
                PatientName = common.myStr(txtSearch.Text).Trim();
                break;
            case "EN":
                EnrolleNo = common.myStr(txtSearch.Text).Trim();
                break;
            case "MN":
                MobileNo = common.myStr(txtSearch.Text).Trim();
                break;
        }


        Obj = new BaseC.Appointment(sConString);
        ds = new DataSet();
        ds = Obj.GetAppintmentDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), frmDate, DoctorId, Status, DateOption, ProviderIds, common.myInt(ddlAppointmentStatus.SelectedValue), RegistrationNo, PatientName, EnrolleNo, MobileNo);
        if (ds.Tables[0].Rows.Count == 0)
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvPatientDetails.DataSource = ds;
            gvPatientDetails.DataBind();
        }
        else
        {
            gvPatientDetails.DataSource = ds;
            gvPatientDetails.DataBind();
        }
    }
    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientDetail();
    }
    protected void gvPatientDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["PrevRow"] = null;
            ViewState["CurrRow"] = null;
            if (ViewState["Blank"] == null)
            {
                String AppStatusColor = common.myStr(((HiddenField)e.Row.FindControl("hdnStatusColor")).Value);
                Label lbl_StatusColor = new Label();

                if (common.myStr(AppStatusColor).Trim() != "Blank" && common.myStr(AppStatusColor).Trim() != "" && common.myStr(AppStatusColor).Trim() != "&nbsp;")
                {
                    lbl_StatusColor.BackColor = System.Drawing.ColorTranslator.FromHtml(AppStatusColor.Trim());

                    A = lbl_StatusColor.BackColor.A;
                    B = lbl_StatusColor.BackColor.B;
                    R = lbl_StatusColor.BackColor.R;
                    G = lbl_StatusColor.BackColor.G;
                    htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(common.myStr(htmlHexColorValue));
                    }
                }
                else
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ViewState["PrevRow"] = null;
                        ViewState["CurrRow"] = null;
                    }
                }
            }
        }
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

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        BindPatientDetail();
    }

    protected void btnResetFilter_OnClick(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        txtSearchN.Text = string.Empty;
        ddlAppointmentStatus.SelectedIndex = 0;
        ddlProvider.SelectedValue = "0";
        BindPatientDetail();
    }
    private DataSet getAppointmentStatus()
    {
        DataSet ds = new DataSet();
        BaseC.clsLISPhlebotomy objLP = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            ds = objLP.getStatus(common.myInt(Session["HospitalLocationId"]), "Appointment", string.Empty);
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objLP = null;
        }
        return ds;
    }
    private void BindProvider()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = new DataSet();
        Hashtable hsIn = new Hashtable();

        try
        {
            BaseC.clsPharmacy objdoc = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();
            ds = objdoc.getEmployeeWithResource(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]), 0, common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlProvider.DataSource = ds;
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataValueField = "DoctorID";
                ddlProvider.DataBind();
            }
            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlProvider.Items[0].Value = "0";
            ddlProvider.SelectedValue = "0";

            #region Status
            ddlAppointmentStatus.Items.Clear();

            RadComboBoxItem lstStatus = new RadComboBoxItem();
            lstStatus.Value = "";
            lstStatus.Text = "Select All";
            lstStatus.Selected = true;
            ddlAppointmentStatus.Items.Add(lstStatus);

            ds = getAppointmentStatus();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lstStatus = new RadComboBoxItem();
                lstStatus.Attributes.Add("style", "background-color:" + common.myStr(ds.Tables[0].Rows[i]["StatusColor"]) + ";");
                lstStatus.Text = common.myStr(ds.Tables[0].Rows[i]["Status"]);
                lstStatus.Value = common.myStr(ds.Tables[0].Rows[i]["StatusId"]);

                ddlAppointmentStatus.Items.Add(lstStatus);
            }

            #endregion
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;
            objDs.Dispose();
        }
    }
}
