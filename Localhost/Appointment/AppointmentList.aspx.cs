using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class Appointment_AppointmentList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPatientHiddenDetails(common.myStr(Request.QueryString["regForDetails"]));
            fillGridData();

        }
    }
    protected void fillGridData()
    {
        try
        {
            lblMessage.Text = "";
            Hashtable hsInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsInput.Add("@intLoginFacilityId", common.myInt(Session["FacilityId"]));
            hsInput.Add("@intRegistrationId", common.myInt(Request.QueryString["regForDetails"]));
            hsInput.Add("@cShowAll", common.myStr(ddlrange.SelectedValue));

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspAppointmentSlip", hsInput);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    ds.AcceptChanges();
                }
                    gvAppointment.DataSource = ds.Tables[0];
                    gvAppointment.DataBind();

                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

        }
    }
    void BindPatientHiddenDetails(String RegistrationNo)
    {
        //BaseC.ParseData bParse = new BaseC.ParseData();
        //BaseC.Patient bC = new BaseC.Patient(sConString);
        //objLISMaster = new BaseC.clsLISMaster(sConString);
        if (RegistrationNo != "")
        {
            Hashtable hsInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hsInput.Add("@intRegistrationId", common.myInt(Request.QueryString["regForDetails"]));
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetails", hsInput);


            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    hdnRegId.Value = common.myStr(dr["RegistrationId"]);
                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["Gender"]) + "-" + common.myStr(dr["Age"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);

                    //   lblMessage.Text = "";
                }

            }
            else
            {
                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //lblMessage.Text = "Patient not found !";
                return;
            }
        }
    }
    protected void gvAppointment_OnPageIndexChanged(Object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAppointment.PageIndex = e.NewPageIndex;
            fillGridData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

        }
    }
    protected void ddlrange_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillGridData();
    }



    protected void gvAppointment_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        Int32 rowind = gvRow.RowIndex;
        if (e.CommandName == "Print")
        {

            HiddenField hdnRegistrationId = (HiddenField)gvRow.FindControl("hdnRegistrationId");
            HiddenField hdnAppointmentId = (HiddenField)gvRow.FindControl("hdnAppointmentId");

            RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(hdnRegistrationId.Value) + "&AppointmentId=" + common.myInt(hdnAppointmentId.Value);
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 750;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.ReloadOnShow = true;

        }
        if (e.CommandName == "CancelApp")
        {
            HiddenField hdnAppointmentId = (HiddenField)gvRow.FindControl("hdnAppointmentId");
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsStatus = new DataSet();
            bool bStatus = false;
            ViewState["AppId"] = hdnAppointmentId.Value.Trim();
            string strStatus = "select d.StatusID,s.Code from DoctorAppointment D with (nolock) inner join StatusMaster s on d.StatusId = s.StatusId where d.AppointmentID =" + hdnAppointmentId.Value;

            dsStatus = dl.FillDataSet(CommandType.Text, strStatus);
            if (dsStatus.Tables[0].Rows.Count > 0)
            {
                //if (Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 3 || Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 5)
                if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "P" || common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                {
                    bStatus = true;
                }
            }

            if (bStatus == true)
            {
                Alert.ShowAjaxMsg("Appointment Already Checked-In. Appointment Cannot Be Deleted.", Page.Page);
                return;
            }
            else
            {
                dvDelete.Visible = true;
                fillRemarks();
            }



            

        }
    }

    protected void gvAppointment_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAppDate = (Label)e.Row.FindControl("lblAppDate");
            HiddenField hdnStatusCode = (HiddenField)e.Row.FindControl("hdnStatusCode");
            if (common.myDate(common.myStr(lblAppDate.Text).Trim()) < common.myDate(DateTime.Now.Date)
                || hdnStatusCode.Value=="PC")
            {

                ImageButton lnkCancel = (ImageButton)e.Row.FindControl("ibtnDelete");

                lnkCancel.Visible = false;
            }
        }
    }
    protected void btnDeleteApp_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Appointment appoint = new BaseC.Appointment(sConString);

            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarkss.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }



            //Hashtable hshIn = new Hashtable();
            hshIn.Add("@intAppointmentId", common.myInt(ViewState["AppId"]));
            hshIn.Add("@intLastChangedBy", Session["UserId"]);
            hshIn.Add("@chvCancelRemark", txtCancel.Text);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
          //  Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = hshOut["@chvErrorStatus"].ToString();
            fillGridData();
            dvDelete.Visible = false;


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }
    protected void fillRemarks()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "AppCancel", "AppCancel");
       
        ddlRemarkss.Items.Clear();

        ddlRemarkss.DataSource = ds;
        ddlRemarkss.DataTextField = "Status";
        ddlRemarkss.DataValueField = "StatusId";
        ddlRemarkss.DataBind();
        ddlRemarkss.Items.Insert(0, "Select a Reason");
        ddlRemarkss.Items[0].Value = "0";
    }
}
