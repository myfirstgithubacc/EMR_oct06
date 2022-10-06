using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_Orders_ServiceAck : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objCommon;//= new wcf_Service_Common.CommonMasterClient();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objCommon = new BaseC.RestFulAPI(sConString);
            if (!IsPostBack)
            {

                //{
                //    Response.Redirect("/default.aspx?RegNo=0");
                //}
                //if (Session["RegistrationId"] != null && Session["EncounterId"] != null)
                //{
                //    lblPatientInfoName.Text = "";
                //    lblPatientInfoAge.Text = "";
                //    lblPatientInfoRegNo.Text = "";
                //    lblPatientInfoDOB.Text = "";
                //    lblPatientInfoHome.Text = "";
                //    lblPatientInfoMobile.Text = "";
                dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
                dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
                BindControl();
                // BindData();
                //}
                BindBlankGrid();
                //Added by rakesh start
                lblCancelRefund.BackColor = System.Drawing.Color.Bisque;
                lblAcknowledged.BackColor = System.Drawing.Color.Aquamarine;
                btnShowData_OnClick(btnShowData, new EventArgs());
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void ClearControls()
    {
        dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
        dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
        dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
        dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
        dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
        dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
        BindControl();
        // BindData();
        //}
        BindBlankGrid();
        lblMessage.Text = "";
        //Added by rakesh start
        lblAcknowledged.BackColor = System.Drawing.Color.Bisque;
    }

    protected void BindControl()
    {
        DataSet ds;
        ds = new DataSet();
        BaseC.EMRMasters objEmr = new BaseC.EMRMasters(sConString);
        //Added on 23-04-2014 Start Naushad
        int sGroupId = common.myInt(Session["GroupID"].ToString());

        string DeparpmentTtype = "'P','IS','I','C'";
        DataTable dtDepartmentbyGroupid = getDepartment_byGroupId(common.myInt(Session["HospitalLocationID"]), DeparpmentTtype, sGroupId);
        if (dtDepartmentbyGroupid.Rows.Count > 0)
        {
            if (dtDepartmentbyGroupid.Rows.Count == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "No Deparment Assigned to this User";
                return;

            }
            ViewState["departmentds"] = dtDepartmentbyGroupid;
            ddlDepartment.Items.Clear();
            ddlSubDepartment.Items.Clear();
            ddlDepartment.DataSource = dtDepartmentbyGroupid;
            //ddlDepartment.DataSource = ds;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlDepartment.SelectedValue = "0";

            ds = new DataSet();
            ds = objCommon.GetFacilityList(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["UserId"]),
                Convert.ToInt16(Session["GroupId"]), 0);
            ddlFacility.Items.Clear();
            ddlFacility.DataSource = ds;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlFacility.SelectedIndex = 0;
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));

            bind_ServiceName("", "");
        }
        else
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "No Deparment Assigned to this User";
            return;
        }
    }

    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubDepartment.Items.Clear();
        ddlSubDepartment.Text = "";
        ddlServiceName.Items.Clear();
        ddlServiceName.Text = "";
        if (common.myInt(ddlDepartment.SelectedValue) > 0)
        {

            BaseC.EMRMasters objEmr = new BaseC.EMRMasters(sConString);
            DataSet ds = objEmr.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), "'P','IS','I','C'", 0);
            ddlSubDepartment.DataSource = ds;
            ddlSubDepartment.DataTextField = "SubName";
            ddlSubDepartment.DataValueField = "SubDeptId";
            ddlSubDepartment.DataBind();
            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlSubDepartment.SelectedValue = "0";
            bind_ServiceName(ddlDepartment.SelectedValue, "");
        }
    }
    protected void BindData()
    {
        try
        {
            string Orderid = "";
            string PatientName = "";
            string RegistionNo = "";
            if (drpSearchby.SelectedValue == "0")
            {
                PatientName = txtSerachby.Text;
            }
            else if (drpSearchby.SelectedValue == "1")
            {
                RegistionNo = txtSerachby.Text;
            }
            else if (drpSearchby.SelectedValue == "2")
            {
                Orderid = txtSerachby.Text;
            }
            else
            {
                Orderid = "";
                PatientName = "";
                RegistionNo = "";

            }
            BaseC.EMROrders objB = new BaseC.EMROrders(sConString);
            ViewState["GridData"] = null;
            DataSet ds = objB.GetServiceOrderFurtherAck(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue),
                0, 0, common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue),
                common.myDate(dtpfromdate.SelectedDate).ToString("yyyy/MM/dd"), common.myDate(dtpTodate.SelectedDate).ToString("yyyy/MM/dd"),
                common.myInt(ddlServiceName.SelectedValue), common.myStr(ddlPatientType.SelectedValue), common.myStr(ddlProcedureType.SelectedValue)
            , common.myStr(Orderid), common.myStr(PatientName), common.myInt(RegistionNo), common.myInt(Session["GroupId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["GridData"] = ds.Tables[0];
                    gvData.DataSource = ds.Tables[0];
                    gvData.DataBind();
                }
                else
                {
                    BindBlankGrid();
                }
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        //Addded on  19-04-2014 naushad Start 
        dt.Columns.Add("Source");
        dt.Columns.Add("OrderID");
        dt.Columns.Add("OrderNo");
        //Addded on  19-04-2014 naushad End
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("AgeGender");
        dt.Columns.Add("CompanyType");
        dt.Columns.Add("OrderId");
        dt.Columns.Add("OrderDate");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("DoctorId");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("OrderDetailId");
        dt.Columns.Add("SubDeptId");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("FacilityName");
        dt.Columns.Add("IsAcknowledge");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("ProAckId");
        dt.Columns.Add("Active");
        dt.Columns.Add("Status");
        dt.Columns.Add("PerformingDoctorName");
        dt.Columns.Add("InvoiceNo");

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        gvData.DataSource = dt;
        gvData.DataBind();
    }
    protected void gvData_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int hdnIsAcknowledge = common.myInt(((HiddenField)e.Row.FindControl("hdnIsAcknowledge")).Value);
            LinkButton lnkAck = (LinkButton)e.Row.FindControl("lnkAck");
            LinkButton lblServiceName = (LinkButton)e.Row.FindControl("lblServiceName");
            HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
            if (hdnActive.Value == "2" || hdnActive.Value == "0")
            {
                lnkAck.Text = "";
                e.Row.BackColor = System.Drawing.Color.Bisque;
                lnkAck.Visible = false;
                lblServiceName.Enabled = false;
            }
            else
            {
                if (hdnIsAcknowledge > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    lnkAck.Text = "Un-Acknowledge";
                }

                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                    lblServiceName.Enabled = false;
                    lnkAck.Text = "Acknowledge";
                    lnkAck.Visible = true;
                }
            }
        }
    }
    protected void gvData_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        if (ViewState["GridData"] != null)
        {
            gvData.DataSource = (DataTable)ViewState["GridData"];
            gvData.DataBind();
        }
    }
    protected void lnkOpenTemplate_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkOpen = (LinkButton)sender;
        string sServiceId = ((HiddenField)lnkOpen.FindControl("hdnServiceId")).Value.ToString().Trim();
        string sOrdDtlId = ((HiddenField)lnkOpen.FindControl("hdnServiceDtlId")).Value.ToString().Trim();
        Session["EncounterId"] = ((HiddenField)lnkOpen.FindControl("hdnEncounterId")).Value.ToString().Trim();
        Session["RegistrationId"] = ((HiddenField)lnkOpen.FindControl("hdnRegistrationId")).Value.ToString().Trim();
        if (common.myInt(sServiceId) > 0)
        {
            RadWindow3.NavigateUrl = "~/EMR/Templates/Default.aspx?MASTER=No&ServId=" + sServiceId + "&SOD=" + sOrdDtlId + "&From=POPUP";
            RadWindow3.Height = 600;
            RadWindow3.Width = 900;
            RadWindow3.Top = 20;
            RadWindow3.Left = 20;
            RadWindow3.OnClientClose = "OnClientClose";
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow3.VisibleStatusbar = false;
        }
    }
    protected void lnkViewDetails_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkOpen = (LinkButton)sender;
        string sServiceId = ((HiddenField)lnkOpen.FindControl("hdnServiceId")).Value.ToString().Trim();
        string lblServiceName = ((LinkButton)lnkOpen.FindControl("lblServiceName")).Text.Trim();
        //lblServiceName

        string sRegistrationNo = ((Label)lnkOpen.FindControl("lblRegistrationNo")).Text;

        string hdnEncounterId = ((HiddenField)lnkOpen.FindControl("hdnEncounterId")).Value.ToString().Trim();
        string sOrdDtlId = ((HiddenField)lnkOpen.FindControl("hdnServiceDtlId")).Value.ToString().Trim();

        //Added on 24-04-2014 naushad Start 
        string PatientType = "";
        int ProcACID = 0;
        int FacliityID = common.myInt(ddlFacility.SelectedValue);
        string RegID = "";
        int orderId = 0;


        PatientType = ((Label)lnkOpen.FindControl("lblPatientType")).Text;
        RegID = ((HiddenField)lnkOpen.FindControl("hdnRegistrationId")).Value.ToString().Trim();
        ProcACID = common.myInt(((HiddenField)lnkOpen.FindControl("hdnProAcId")).Value.ToString().Trim());
        orderId = common.myInt(((HiddenField)lnkOpen.FindControl("hdnOrderId")).Value.ToString().Trim());
        //Added on 24-04-2014 naushad End

        if (common.myInt(sServiceId) > 0)
        {
            //RadWindow3.NavigateUrl = "ViewDetails.aspx?MASTER=No&ServId=" + sServiceId + "&ServName=" + 
            //    lblServiceName + "&From=POPUP&RegNo=" + common.myStr(sRegistrationNo) + "&EncounterId=" +
            //    hdnEncounterId + "&sOrdDtlId=" + sOrdDtlId;

            RadWindow3.NavigateUrl = "ViewDetails.aspx?MASTER=No&ServId=" + sServiceId + "&ServName=" +
                lblServiceName + "&From=POPUP&RegNo=" + common.myStr(sRegistrationNo) + "&EncounterId=" +
                hdnEncounterId + "&sOrdDtlId=" + sOrdDtlId + "&PatientType=" + PatientType + "&RegID=" + RegID +
                "&ProcACID=" + ProcACID + "&orderId=" + orderId + "&FacliityID=" + FacliityID;

            RadWindow3.Height = 600;
            RadWindow3.Width = 800;
            RadWindow3.Top = 20;
            RadWindow3.Left = 20;
            RadWindow3.OnClientClose = "OnClientClose";
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            //RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow3.VisibleStatusbar = false;
        }
    }
    //added on 21-04-2014  By nausahd
    public DataTable getDepartment_byGroupId(int HospitalLocationID, string DepartmentType, int groupID)
    {

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsIn = new Hashtable();
        hsIn.Add("@inyHospitalLocationID", HospitalLocationID);
        hsIn.Add("@chvDepartmentType", DepartmentType);
        hsIn.Add("@intGroupID", groupID);
        DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDepartmentbyGroupID", hsIn);
        return ds.Tables[0];
    }
    protected void lnkViewServiceAck_OnClick(object sender, EventArgs e)
    {

        LinkButton lnkAck = (LinkButton)sender;
        string servicename = ((LinkButton)lnkAck.FindControl("lblServiceName")).Text;

        string sServiceId = ((HiddenField)lnkAck.FindControl("hdnServiceId")).Value.ToString().Trim();
        string sOrdDtlId = ((HiddenField)lnkAck.FindControl("hdnServiceDtlId")).Value.ToString().Trim();
        //Added on 19-04-2014 Stat naushad Ali
        string sType = ((Label)lnkAck.FindControl("lblPatientType")).Text;
        hdn_RegistrationId.Value = ((HiddenField)lnkAck.FindControl("hdnRegistrationId")).Value.ToString().Trim();
        hdn_EncounterId.Value = ((HiddenField)lnkAck.FindControl("hdnEncounterId")).Value.ToString().Trim();
        string sStatus = ((HiddenField)lnkAck.FindControl("hdnIsAcknowledge")).Value.ToString().Trim();
        hdn_hdnProAcId.Value = ((HiddenField)lnkAck.FindControl("hdnProAcId")).Value.ToString().Trim();
        int orderId = common.myInt(((HiddenField)lnkAck.FindControl("hdnOrderId")).Value.ToString().Trim());



        RadWindow3.NavigateUrl = "~/EMR/Orders/ServiceAckDetails.aspx?Type=" + sType +
            "&OrderID=" + orderId + "&Orderdetaild=" + sOrdDtlId + "&ServiceID=" + sServiceId +
            "&Status=" + sStatus + "&Viewtype=V&servicename=" + servicename;
        RadWindow3.Height = 300;
        RadWindow3.Width = 500;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        //RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow3.VisibleStatusbar = false;
    }
    protected void lnkAck_OnClick(object sender, EventArgs e)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        LinkButton lnkAck = (LinkButton)sender;
        string sServiceId = ((HiddenField)lnkAck.FindControl("hdnServiceId")).Value.ToString().Trim();
        string sOrdDtlId = ((HiddenField)lnkAck.FindControl("hdnServiceDtlId")).Value.ToString().Trim();
        hdn_PatientType.Value = ((Label)lnkAck.FindControl("lblPatientType")).Text;
        hdn_RegistrationId.Value = ((HiddenField)lnkAck.FindControl("hdnRegistrationId")).Value.ToString().Trim();
        hdn_EncounterId.Value = ((HiddenField)lnkAck.FindControl("hdnEncounterId")).Value.ToString().Trim();
        hdn_IsAcknowledge.Value = ((HiddenField)lnkAck.FindControl("hdnIsAcknowledge")).Value.ToString().Trim();
        hdn_hdnProAcId.Value = ((HiddenField)lnkAck.FindControl("hdnProAcId")).Value.ToString().Trim();
        string servicename = ((LinkButton)lnkAck.FindControl("lblServiceName")).Text;

        int ProcACID = common.myInt(((HiddenField)lnkAck.FindControl("hdnProAcId")).Value.ToString().Trim());
        

        GridViewRow gv = (GridViewRow)lnkAck.NamingContainer;

        int orderId = common.myInt(((HiddenField)lnkAck.FindControl("hdnOrderId")).Value.ToString().Trim());

        if (hdn_IsAcknowledge.Value == "0")
        {
            int IsMandatoryTemplateEntered = order.EMRCheckOrderTemplateDetails(common.myInt(sOrdDtlId), 0);
            if (IsMandatoryTemplateEntered.Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Mandatory template details not available";
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }
        }

        hdn_ServiceID.Value = sServiceId;
        hdn_OrderId.Value = orderId.ToString();
        hdn_ServiecDeID.Value = sOrdDtlId;

        //Added on 19-04-2014  by Naushad Ali


        LinkButton lnkPatient = (LinkButton)sender;
        string sRegistrationNo = ((Label)lnkPatient.FindControl("lblRegistrationNo")).Text;
        string hdnFacilityName = ((HiddenField)lnkPatient.FindControl("hdnFacilityName")).Value.ToString();

        Label lblPerformingDoctorName = (Label)lnkPatient.FindControl("lblPerformingDoctorName");
        if (hdn_ServiceID.Value != "0" && hdn_ServiceID.Value != "")
        {
            if (common.myStr(sRegistrationNo).Trim() != "")
            {

                RadWindow3.NavigateUrl = "~/EMR/Orders/ServiceAckDetails.aspx?ServiceId=" + sServiceId + "&ServiceName=" +
                servicename + "&RegNo=" + common.myStr(sRegistrationNo) + "&Status=" + hdn_IsAcknowledge.Value + "&EncounterId=" + 
                hdn_EncounterId.Value + "&OrdDtlId=" + sOrdDtlId + "&PatientType=" + hdn_PatientType.Value + "&RegId=" + hdn_RegistrationId.Value +
                "&ProcACId=" + ProcACID + "&OrderId=" + orderId + "&FacliityId=" + ddlFacility.SelectedValue + "&PerformingDoctor=" + lblPerformingDoctorName.Text;

               // RadWindow3.NavigateUrl = "/EMR/Orders/ServiceAckDetails.aspx?DoctorName=" + lblPerformingDoctorName.Text + "&servicename=" + servicename + "&Viewtype=UD";
                RadWindow3.Height = 400;
                RadWindow3.Width = 600;
                RadWindow3.Behaviors = WindowBehaviors.None;
                RadWindow3.Top = 10;
                RadWindow3.Left = 10;
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.OnClientClose = "OnClientClose";

                RadWindow3.Modal = true;
                RadWindow3.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select a patient", Page.Page);
                return;
            }
        }
        return;
    }
    //Added on  19-04-2014  Naushad 
    public void bind_ServiceName(string DepartID, string SubDepartID)
    {
        DataSet ds = new DataSet();
        BaseC.EMROrders objmaster = new BaseC.EMROrders(sConString);
        ds = objmaster.GetHospitalServices(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), common.myInt(Session["FacilityId"]));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlServiceName.DataSource = ds.Tables[0];
                ddlServiceName.DataTextField = "ServiceNameWithCode";
                ddlServiceName.DataValueField = "ServiceId";
                ddlServiceName.DataBind();
                ddlServiceName.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlServiceName.SelectedIndex = 0;
            }
        }
    }
    protected void btnAck_RemarksClose_OnClick(object sender, EventArgs e)
    {
        Label lblremark = new Label();
        lblremark.Text = hdnAckRemarks.Value;

        string sServiceId = hdn_ServiceID.Value;
        string sOrdDtlId = hdn_ServiecDeID.Value;
        int orderId = common.myInt(hdn_OrderId.Value);

        int SetAcknowledge = common.myInt(hdn_setAck.Value);
        if (SetAcknowledge > 0)
        {
            if (common.myInt(sServiceId) > 0)
            {
                int AckStatus = common.myInt(hdn_IsAcknowledge.Value);
                //BindData();
                BaseC.EMROrders objProAckOPIP = new BaseC.EMROrders(sConString);
                //Label lbllblPatientType=(Label).
                Hashtable hshout = objProAckOPIP.UpdateProcedureAckOPIP(hdn_PatientType.Value,
                    common.myInt(Session["HospitalLocationId"]),
                    common.myInt(ddlFacility.SelectedValue),
                    common.myInt(hdn_RegistrationId.Value),
                    common.myInt(hdn_EncounterId.Value),
                    common.myInt(orderId),
                    common.myInt(sOrdDtlId),
                    common.myInt(sServiceId),
                    common.myStr(hdnAckRemarks.Value),
                    common.myInt(Session["UserId"].ToString()),
                    AckStatus == 0 ? 1 : 0,
                    common.myInt(hdn_hdnProAcId.Value), 0);

                btnShowData_OnClick(btnShowData, new EventArgs());
                Session["AckRemakrs"] = null;
            }
        }
    }
    protected void btnShowData_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        BindData();
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        ddlDepartment.SelectedIndex = -1;
        ddlSubDepartment.SelectedIndex = -1;
        ddlSubDepartment.Text = "";
        dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
        dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
        //txtRegistrationNo.Text = string.Empty;
        // txtPatientName.Text = string.Empty;
        txtSerachby.Text = string.Empty;
        gvData.DataSource = null;
        gvData.DataBind();
        ClearControls();
    }
    protected void lnkPatient_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkPatient = (LinkButton)sender;
        string sRegistrationNo = ((Label)lnkPatient.FindControl("lblRegistrationNo")).Text;
        string hdnFacilityName = ((HiddenField)lnkPatient.FindControl("hdnFacilityName")).Value.ToString();

        string sRegID = ((HiddenField)lnkPatient.FindControl("hdnRegistrationId")).Value;


        if (common.myStr(sRegistrationNo).Trim() != "")
        {
            RadWindow3.NavigateUrl = "~/LIS/Phlebotomy/PatientDetails.aspx?RId=" + sRegID + "&RegNo=" + sRegistrationNo + "&PName=" + lnkPatient.Text + "&facility=" + common.myStr(hdnFacilityName);

            RadWindow3.Height = 300;
            RadWindow3.Width = 900;
            RadWindow3.Top = 10;
            RadWindow3.Left = 10;
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select a patient", Page.Page);
            return;
        }
    }
    //Added by rakesh start for adding service drop down on behalf of the selected subdepartment start
    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubDepartment.SelectedValue != "-1")
            {
                DataSet ds = new DataSet();
                BaseC.clsLISMaster objmaster = new BaseC.clsLISMaster(sConString);
                ds = objmaster.GetHospitalServices(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(0), 
                    Convert.ToInt16(ddlSubDepartment.SelectedValue), common.myInt(Session["FacilityId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.Sort = "ServiceName";
                        ddlServiceName.DataSource = ds.Tables[0].DefaultView;
                        ddlServiceName.DataTextField = "ServiceNameWithCode";
                        ddlServiceName.DataValueField = "ServiceId";
                        ddlServiceName.DataBind();
                    }
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
}
   
