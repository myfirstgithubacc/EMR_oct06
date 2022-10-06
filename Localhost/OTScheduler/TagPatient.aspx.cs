using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class OTScheduler_TagPatient : System.Web.UI.Page
{
    BaseC.RestFulAPI objwcfot;//= new wcf_Service_OT.ServiceClient();
    DataSet ds = new DataSet();
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objwcfot = new BaseC.RestFulAPI(sConString);

            if (Request.QueryString["FromPage"] == "BILL")
            {
                trOtDetails.Visible = false;
                trOtDetails1.Visible = false;
                trOtDetails2.Visible = false;
                trOtDetails3.Visible = false;
                FillPreAuthpatientDetails();
            }
            else
            {

                if (Request.QueryString["BookingId"] != null)
                {
                    ShowOTDetails();
                    rblOPIP_OnSelectedIndexChanged(this, null);
                }
            }
        }
    }

    private void ShowOTDetails()
    {
        int HospId = common.myInt(Session["HospitalLocationId"]);
        int facilityId = common.myInt(Session["FacilityId"]);
        ds = objwcfot.getOTBookingDetails(HospId, facilityId, common.myInt(Request.QueryString["BookingId"]), "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblRegNo.Text = common.myStr(dr["RegistrationNo"]);
            lblFName.Text = common.myStr(dr["FirstName"]);
            lblMName.Text = common.myStr(dr["MiddleName"]);
            lblLName.Text = common.myStr(dr["LastName"]);
            lblDOB.Text = common.myStr(dr["DOB"]);
            lblAgeY.Text = common.myStr(dr["AgeYears"]);
            lblAgeM.Text = common.myStr(dr["AgeMonths"]);
            lblAgeD.Text = common.myStr(dr["AgeDays"]);
            lblGender.Text = common.myStr(dr["Gender"]);
            lblTheaterName.Text = common.myStr(dr["TheatreName"]);
            lblDate.Text = common.myStr(dr["OTBookingDateF"]);
            lblTimeFrom.Text = common.myStr(dr["FromTime"]);
            lblTimeTo.Text = common.myStr(dr["ToTime"]);
            lblService.Text = common.myStr(dr["ServiceName"]);

        }
    }
    protected void ibtnOpenSearchPatientPopup_OnClick(object sender, EventArgs e)
    {
        if (ddlSearchOn.SelectedValue == "1")
        {
            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
        }
        else
            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=O&RegEnc=2&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
        RadWindow1.Height = 550;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnSearchByPatientNo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (txtPatientNo.Text.Trim().Length > 0)
            {
                //ViewState["regid"] = txtPatientNo.Text.Trim();
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                int EncounterId = 0;
                int UserId = common.myInt(Session["UserId"]);
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegId = 0;
                string RegistrationNo = "";
                DataSet ds = new DataSet();
                //ds = objPatient.getPatientDetails(HospId, FacilityId, RegId, txtPatientNo.Text, EncounterId, EncodedBy);

                if (ddlSearchOn.SelectedValue == "1")
                {
                    RegistrationNo = common.myStr(hdnRegistrationNo.Value);
                    hdnEncounterNo.Value = common.myStr(txtPatientNo.Text);
                }
                else
                {
                    hdnEncounterNo.Value = "";
                    RegistrationNo = common.myStr(txtPatientNo.Text);
                }

                BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
                BaseC.Patient bC = new BaseC.Patient(sConString);
                if (ddlSearchOn.SelectedValue == "0")
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, common.myInt( RegistrationNo), EncounterId, UserId);
                else
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, common.myInt(RegistrationNo), UserId, EncounterId, common.myStr(hdnEncounterNo.Value));
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (ddlSearchOn.SelectedValue == "1")
                    {
                        lblInfoTReg.Visible = true;
                        lblTRegNo.Visible = true;
                        lblTRegNo.Text = common.myStr(dt.Rows[0]["RegistrationNo"]);
                        hdnEncounterId.Value = common.myStr(dt.Rows[0]["EncounterId"]);
                    }
                    else
                    {
                        lblInfoTReg.Visible = false;
                        lblTRegNo.Visible = false;
                    }
                    hdnRegistrationId.Value = common.myStr(dt.Rows[0]["RegistrationId"]);

                    lblTLName.Text = common.myStr(dt.Rows[0]["LastName"]);
                    lblTFName.Text = common.myStr(dt.Rows[0]["FirstName"]);
                    lbltMName.Text = common.myStr(dt.Rows[0]["MiddleName"]);
                    dtpTDOB.SelectedDate = common.myDate(dt.Rows[0]["DOB"]);
                    //lblTAgeY.Text = common.myStr(dt.Rows[0]["AgeYears"]);
                    //lblTAgeM.Text = common.myStr(dt.Rows[0]["AgeMonths"]);
                    //lblTAgeD.Text = common.myStr(dt.Rows[0]["AgeDays"]);
                    lblTGender.Text = common.myStr(dt.Rows[0]["Gender"]);
                    lblTAdmissionDate.Text = common.myStr(dt.Rows[0]["EncounterDate"]);
                    lblTBedNo.Text = common.myStr(dt.Rows[0]["BedNo"]);
                    imgCalYear_Click(this, null);

                    lblMessage.Text = "";
                }
                else
                {
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void rblOPIP_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblOPIP.SelectedValue == "O")
        {
            ddlSearchOn.SelectedValue = "0";
        }
        else
            ddlSearchOn.SelectedValue = "1";
        ddlSearchOn.Enabled = false;
        txtPatientNo.Focus();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string result = "";
        if (Request.QueryString["FromPage"] == "BILL")
        {
            BaseC.EMRBilling objEmrBiling = new BaseC.EMRBilling(sConString);
            result = objEmrBiling.tagPreAuthPatient(common.myInt(Request.QueryString["BookingId"]), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), common.myInt(Session["UserId"]));
        }
        else
        {
            if (Validate() == false)
            {
                return;
            }
            BaseC.RestFulAPI objOt = new BaseC.RestFulAPI(sConString);
            //System.Collections.Generic.Dictionary<object, object> htOut = new System.Collections.Generic.Dictionary<object, object>();
            int otBookingId = common.myInt(Request.QueryString["BookingId"]);
            int UserId = common.myInt(Session["UserId"]);
            int TagRegId = common.myInt(hdnRegistrationId.Value);
            int TagEncounterId = common.myInt(hdnEncounterId.Value);
            string TagEncounterNo = common.myStr(hdnEncounterNo.Value);
            result = objOt.TagPatient(otBookingId, TagRegId, TagEncounterId, TagEncounterNo, UserId);
            if (result.Contains("successfully"))
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            else
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        lblMessage.Text = result;
    }
    protected void imgCalYear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (dtpTDOB.SelectedDate != null)
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                lblTAgeY.Text = "";
                lblTAgeM.Text = "";
                lblTAgeD.Text = "";
                DateTime datet = dtpTDOB.SelectedDate.Value;
                // DateTime dateReg = dtpRegDate.SelectedDate.Value;
                // bC.DOB = datet.ToString("dd/MM/yyyy");
                string[] result = bC.CalculateAge(datet.ToString("yyyy/MM/dd"));

                if (result.Length == 2)
                {
                    if (result[1] == "Yr")
                    {
                        lblTAgeY.Text = result[0];
                    }
                    else if (result[1] == "Mnth")
                    {
                        lblTAgeM.Text = result[0];
                    }
                    else
                    {
                        lblTAgeD.Text = result[0];
                    }
                }

                if (result.Length == 4)
                {
                    //txtAgeYears.Text = result[0];
                    lblTAgeM.Text = result[0];
                    lblTAgeD.Text = result[2];
                }
                if (result.Length == 6)
                {
                    lblTAgeY.Text = result[0];
                    lblTAgeM.Text = result[2];
                    lblTAgeD.Text = result[4];
                }

                if (lblTAgeY.Text == "")
                {
                    lblTAgeY.Text = "0";
                }
                if (lblTAgeM.Text == "")
                {
                    lblTAgeM.Text = "0";
                }
                if (lblTAgeD.Text == "")
                {
                    lblTAgeD.Text = "0";
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

    private bool Validate()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (common.myStr(txtPatientNo.Text) == "" || common.myStr(lblTFName.Text) == "")
        {
            lblMessage.Text = "Select a patient to tag !";
            return false;
        }
        return true;
    }
    public void FillPreAuthpatientDetails()
    {
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);

        DataSet ds = objBill.getPreAuthData(common.myInt(Request.QueryString["BookingId"]), common.myInt(Session["HospitalLocationId"]),
                            0, common.myInt(Session["FacilityID"]), false, DateTime.Now, DateTime.Now);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblFName.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
            }
        }



    }
}
