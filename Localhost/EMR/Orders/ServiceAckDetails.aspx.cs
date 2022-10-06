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

public partial class Pharmacy_Components_PasswordCheckerAllUser : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.User objUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Viewtype = Request.QueryString["Viewtype"];
            lbl_vServiceName.Text = Request.QueryString["ServiceName"];
            tr5.Visible = false;
            hdnIsAcknowledge.Value = "0";
            if (Viewtype == "V")
            {
                tr1.Visible = true;
                tr2.Visible = true;
                tr3.Visible = true;
                btnSave.Visible = false;
                trChangedDoctorBy.Visible = false;
                BindServiceAckDetails();
                lblDoctorNameLabel.Visible = false;
            }
            else
            {
                tr1.Visible = true;
                if (Viewtype == "UD")
                {
                    lbl_vServiceName.Text = Request.QueryString["servicename"].ToString();
                }
                tr2.Visible = false;
                tr3.Visible = false;
                tr5.Visible = false;
                trChangedDoctorBy.Visible = true;
                FillDoctor();
                lblDoctorName.Text = common.myStr(Request.QueryString["PerformingDoctor"]);
                lblDoctorNameLabel.Visible = true;
                lblDoctorNameLabel.Text ="Performing " + lblNewDoctor.Text;
                lblNewDoctor.Text ="New Performing " + lblNewDoctor.Text;
            }
            if (common.myStr(Session["UserName"]) == "")
            {
                objUser = new BaseC.User(sConString);
                Session["UserName"] = objUser.GetUserName(common.myInt(Session["UserId"]));
            }
            string UseFor = common.myStr(Request.QueryString["UseFor"]);
            if (common.myStr(Request.QueryString["OtherUserId"]) != "")
            {
                objUser = new BaseC.User(sConString);
                lblEmployeeName.Text = common.myStr(Request.QueryString["UserName"]);
            }
            if (common.myStr(Request.QueryString["Status"]) == "1")
            {
                trChangedDoctorBy.Visible = false;
                lblDoctorNameLabel.Visible = false;
                lblDoctorName.Visible = false;
            }
        }
        lblMessage.Text = "";
    }
    public void FillDoctor()
    {
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        DataSet ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlPerformingDoctor.DataSource = ds;
            ddlPerformingDoctor.DataValueField = "DoctorID";
            ddlPerformingDoctor.DataTextField = "DoctorName";
            ddlPerformingDoctor.DataBind();
            ddlPerformingDoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
    }
    public void BindServiceAckDetails()
    {
        if (Request.QueryString["Viewtype"] == "V")
        {
            string source;
            int HospitalID;
            int FaciltyID;
            int RegistraionID = 0;
            int OrderID, OrderDetailID, intServiceID, status;
            source = Request.QueryString["Type"];
            HospitalID = common.myInt(Session["HospitalLocationID"]);
            FaciltyID = common.myInt(Session["FacilityID"]);
            OrderID = common.myInt(Request.QueryString["OrderID"]);
            OrderDetailID = common.myInt(Request.QueryString["Orderdetaild"]);
            intServiceID = common.myInt(Request.QueryString["ServiceID"]);
            status = common.myInt(Request.QueryString["Status"]);
            BaseC.EMROrders objEmrOrder = new BaseC.EMROrders(sConString);
            DataSet objds = objEmrOrder.uspGetProcedureAckOPIP(source, HospitalID, FaciltyID, RegistraionID, OrderID, OrderDetailID, intServiceID, status);
            if (objds.Tables[0].Rows.Count > 0)
            {
                if (status == 0)
                {
                    //tr5.Visible = false;   
                    lbl_vAcknowlageby.Text = objds.Tables[0].Rows[0]["AckBy"].ToString();
                    lbl_vAcknowlagedate.Text = objds.Tables[0].Rows[0]["AckDate"].ToString();
                    lblCancelDate.Text = objds.Tables[0].Rows[0]["CancelBy"].ToString();
                    lblCanceledBy.Text = objds.Tables[0].Rows[0]["CancelledDate"].ToString();
                }
                else
                {
                    //tr5.Visible = true;   
                    lbl_vAcknowlageby.Text = objds.Tables[0].Rows[0]["AckBy"].ToString();
                    lbl_vAcknowlagedate.Text = objds.Tables[0].Rows[0]["AckDate"].ToString();
                    txtRemarks.Text = objds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtRemarks.Enabled = false;
                }
            }
        }
    }
    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            hdnIsAcknowledge.Value = "0";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            return;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Added on 24-04-2014 Start Naushad 
    public void UpdateAcknowlegeService()
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        string PatientType = "";
        int ProcACID = 0;
        int FacliityID = 0;
        string RegID = "";
        string orderId = "0";

        PatientType = Request.QueryString["PatientType"].ToString();
        string sOrdDtlId = Request.QueryString["OrdDtlId"].ToString();
        string sServiceId = Request.QueryString["ServiceId"].ToString();
        string EncounterId = Request.QueryString["EncounterId"].ToString();
        RegID = Request.QueryString["RegId"].ToString();
        orderId = Request.QueryString["OrderId"].ToString();
        FacliityID = common.myInt(Request.QueryString["FacliityId"].ToString());
        ProcACID = common.myInt(Request.QueryString["ProcACId"].ToString());
        int sStatus = common.myInt(Request.QueryString["Status"].ToString());

        int IsMandatoryTemplateEntered = order.EMRCheckOrderTemplateDetails(common.myInt(sOrdDtlId), 0);
        if (IsMandatoryTemplateEntered.Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Mandatory template details not available";
            // return;
        }
        else
        {
            lblMessage.Text = string.Empty;
        }

        if (common.myInt(sServiceId) > 0)
        {
            BaseC.EMROrders objProAckOPIP = new BaseC.EMROrders(sConString);
            Hashtable hshout = objProAckOPIP.UpdateProcedureAckOPIP(PatientType,common.myInt(Session["HospitalLocationId"]),
                common.myInt(FacliityID),common.myInt(RegID),common.myInt(EncounterId),common.myInt(orderId),
                common.myInt(sOrdDtlId),common.myInt(sServiceId),common.myStr(txtRemarks.Text),common.myInt(Session["UserId"].ToString()),
                sStatus == 0 ? 1 : 0, common.myInt(ProcACID), common.myInt(ddlPerformingDoctor.SelectedValue));
            Session["AckRemakrs"] = null;
        }
    }
   
    //Added on 24-04-2014 End naushad
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            UpdateAcknowlegeService();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            return;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
}