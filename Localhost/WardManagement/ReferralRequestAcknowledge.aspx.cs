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
using System.Drawing;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;

public partial class WardManagement_ReferralRequestAcknowledge : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (common.myInt(Session["EncounterId"]) == 0)
        //{
        //    Response.Redirect("/default.aspx?RegNo=0");
        //}

        if (!IsPostBack)
        {
            dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpfromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            dtpToDate.SelectedDate = DateTime.Now;
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);



            ViewState["PatientOPIPType"] = common.myStr("O");
            ViewState["PT"] = common.myStr(Request.QueryString["PT"]);

            bindStatus();

            //ddlWard.SelectedValue=GetCurrentWardNo(common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncounterId"])).ToString();


            bindMainData();
            //  bindDetailsData("");



            //if (common.myStr(ViewState["PT"]) == "P")
            //{
            //    lblHeader.Text = "Prescription";
            //    tdDispenseBy.Visible = false;
            //    BtnAcknowledge.Visible = false;

            //    gvIssueDurgDetail.Enabled = false;
            //}
        }
    }


    private void bindStatus()
    {
        clsIVF obj = new clsIVF(sConString);
        DataSet ds = new DataSet();
        try
        {
            //NRR - Nurse Referral Request, RRA -Referral Request Acknowledge,
            //RRR - Referral Request Reject, RRC -Referral Request Completed

            ds = obj.GetStatusMaster("WardReferralRequest");

            ddlStatus.DataSource = ds.Tables[0];
            ddlStatus.DataValueField = "TypeId";
            ddlStatus.DataTextField = "TypeName";
            ddlStatus.DataBind();

            ddlStatus.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlStatus.SelectedIndex = 0; //ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue("NRR"));
        }
        catch (Exception)
        {
        }
        finally
        {
            obj = null;
            ds.Dispose();
        }
    }


    private void clearControl()
    {


        //ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));



        bindMainData();


    }




    void bindMainData()
    {
        lblMessage.Text = string.Empty;

        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        int regno = 0;
        string encno = "";
        string patientName = "";
        try
        {
            switch (common.myStr(ddlName.SelectedValue))
            {
                case "R":
                    regno = common.myInt(txtSearchN.Text);
                    break;
                case "ENC":
                    encno = common.myStr(txtSearch.Text);
                    break;
                case "N":
                    patientName = common.myStr(txtSearch.Text);
                    break;
            }

            ds = objwd.getIPReferralRequestHIS(common.myInt(Session["FacilityId"]), common.myInt(ddlStatus.SelectedValue),
                                               common.myInt(Session["EmployeeId"]), regno, encno, patientName,
                                               common.myDate(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                               common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPatientDurgStatus.DataSource = ds;
                gvPatientDurgStatus.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);

                gvPatientDurgStatus.DataSource = ds;
                gvPatientDurgStatus.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    public DataSet getAcknowlegeDurgMain(int HospId, int FacilityId, int RegistraionID,
        int EncoutnerID, int WardId, string Status)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistraionid", RegistraionID);
            HshIn.Add("@intEncouterID", EncoutnerID);
            HshIn.Add("@intWardid", WardId);
            HshIn.Add("@chrAckStatus", Status);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrMain", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getAcknowlegeDurgDetail(int RegistraionID, int EncoutnerID, int IssueID)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intRegistraionid", RegistraionID);
            HshIn.Add("@intEncouterID", EncoutnerID);
            HshIn.Add("@intIssueId", IssueID);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrDetail", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    private void reBindData()
    {
        bindMainData();
    }


    public int GetCurrentWardNo(int RegistrionId, int EncounterId)
    {
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string strupdate = "";

        try
        {
            strupdate = "select  CurrentWardID,wM.WardNo,wM.WardName  from  Admission ad INNER  JOIN WardMaster wM on " +
                         "WM.WardId=ad.CurrentWardID  where RegistrationId=" + common.myInt(RegistrionId) + " and  EncounterId=" + EncounterId;

            ds = objDl.FillDataSet(CommandType.Text, strupdate);


            return common.myInt(ds.Tables[0].Rows[0]["CurrentWardID"]);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return 0;
        }
        finally
        {
            ds.Dispose();
            objDl = null;
        }
    }


    protected void gvPatientDurgStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //NRR - Requested, RRA - Acknowledged, RRC - Completed

            HiddenField hdnRequestStatusCode = (HiddenField)e.Row.FindControl("hdnRequestStatusCode");
            LinkButton lnkBtnAcknowledge = (LinkButton)e.Row.FindControl("lnkBtnAcknowledge");
            LinkButton lnkBtnReferralSlip = (LinkButton)e.Row.FindControl("lnkBtnReferralSlip");
            
            lnkBtnAcknowledge.Visible = false;
            lnkBtnReferralSlip.Visible = false;

            switch (common.myStr(hdnRequestStatusCode.Value))
            {
                case "NRR":
                    // lnkBtnAcknowledge.Visible = true;
                    lnkBtnReferralSlip.Visible = true;
                    break;
                //case "RRA":
                //    lnkBtnReferralSlip.Visible = true;
                //    break;
                case "RRC":
                    break;
                default:
                    break;
            }


        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        reBindData();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        clearControl();
        lblMessage.Text = "";

        //ddlAcknowledge.SelectedIndex = 1;
        // txtSearch.Text = "";
        bindMainData();
        // bindDetailsData("");
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        //setDate();
    }

    protected void gvPatientDurgStatus_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDurgStatus.PageIndex = e.NewPageIndex;

        reBindData();
    }

    protected void gvPatientDurgStatus_OnRowCommand(object source, GridViewCommandEventArgs e)
    {
        BaseC.WardManagement objw = new BaseC.WardManagement();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        try
        {
            if (e.CommandName.ToUpper().Equals("ACK"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField hdnRequestId = (HiddenField)row.FindControl("hdnRequestId");
                if (common.myInt(hdnRequestId.Value) > 0)
                {
                    string strmsg = objw.updateReferralRequest(common.myInt(hdnRequestId.Value), "RRA", common.myInt(Session["UserId"]));

                    if (strmsg.ToUpper().Contains("UPDATE"))
                    {
                        bindMainData();
                        lblMessage.Text = "Referral Request Acknowledged";
                        Alert.ShowAjaxMsg("Referral Request Acknowledged", this.Page);
                    }
                }
            }
            else if (e.CommandName.ToUpper().Equals("RS"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField hdnRequestId = (HiddenField)row.FindControl("hdnRequestId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                HiddenField hdnRequestType = (HiddenField)row.FindControl("hdnRequestType");
                HiddenField hdnRequestToDoctorId = (HiddenField)row.FindControl("hdnRequestToDoctorId");
                Label lblRegistrationNo = (Label)row.FindControl("lblRegistrationNo");
                Label lblEncounterNo = (Label)row.FindControl("lblEncounterNo");

                if (common.myInt(hdnRequestId.Value) > 0)
                {
                    RadWindow1.NavigateUrl = "~/EMR/ReferralSlip.aspx?RequestId=" + common.myInt(hdnRequestId.Value) +
                                            "&RequestType=" + common.myInt(hdnRequestType.Value) +
                                            "&Regid=" + common.myInt(hdnRegistrationId.Value) +
                                            "&RegNo=" + common.myStr(lblRegistrationNo.Text) +
                                            "&EId=" + common.myInt(hdnEncounterId.Value) +
                                            "&EncNo=" + common.myStr(lblEncounterNo.Text) +
                                            "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=NO";

                    Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                    Session["PatientDetailString"] = null;
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]),
                                                                                        common.myInt(Session["FacilityId"]), common.myStr(lblRegistrationNo.Text),
                                                                                        common.myStr(lblEncounterNo.Text), common.myInt(Session["UserId"]), 0);

                    bC = null;

                    RadWindow1.Height = 500;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    //RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;

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

    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        txtSearchN.Visible = false;
        txtSearch.Visible = false;

        if (common.myStr(ddlName.SelectedValue).Equals("R"))
        {
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
        }
        // bindStatus(rblSearchCriteria.SelectedValue);
    }

}
