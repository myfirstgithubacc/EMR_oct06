using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;
using System.Linq;
public partial class OTScheduler_OTRequest : System.Web.UI.Page
{
    protected UserControl uc1;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private const int ItemsPerRequest = 50;
    StringBuilder strOtBookingDateTime = new StringBuilder();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Patient objBc = new BaseC.Patient(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Font.Bold = commonLabelSetting.cBold;
        if (!IsPostBack)
        {
            try
            {
                dtpOTDate.MinDate = DateTime.Now;
                BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
                BaseC.RestFulAPI objwcfcm = new BaseC.RestFulAPI(sConString);
                dtpOTDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpDOB.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpOTDate.Enabled = true;
                hdnCurrDate.Value = DateTime.Today.ToString(common.myStr(Application["OutputDateFormat"]));
                ViewState["NewRegNo"] = "0";
                setisAllDoctorDisplayOnAddService();
                setAllowOTBookingForUnregisteredPatient();
                txtRegNo.Enabled = false;
                if (common.myStr(ViewState["AllowOTBookingForUnregisteredPatient"]) == "N")
                {
                    lblStarRegNo.Visible = true;
                }
                else
                {
                    lblStarRegNo.Visible = false;
                }
                BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
                ViewState["TimeViewIntervalForOTInMinutes"] = objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "TimeViewIntervalForOTInMinutes", common.myInt(Session["FacilityId"]));
                if (common.myStr(ViewState["TimeViewIntervalForOTInMinutes"]) == "" || common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]) == 0)
                {
                    ViewState["TimeViewIntervalForOTInMinutes"] = "15";
                }
                //if (Convert.ToString(Session["RegistrationId"]) != null)
                //{
                //    txtAccountNumber.Text = Convert.ToString(Session["RegistrationId"]);
                //}
                //if (Request.QueryString["RegId"] != null)
                //{
                //    txtAccountNumber.Text = Request.QueryString["RegId"].ToString();
                //}
                //DataSet ds = dl.FillDataSet(CommandType.Text, "select * from GetStatus(" + Session["HospitalLocationId"] + ",'Appointment')");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    //hdCheckIn.Value = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                //    // hdUnCon.Value = ds.Tables[0].Rows[1].ItemArray[0].ToString();
                //}
                IsMandatoryField();
                BindDepartment();
                BindProviderList();
                BindAnaesthesia();
                GetgvbindemrotRequest(common.myInt(Session["RegistrationNo"]));
                // BindDiagnosisSearchCode();
                // populateBloodUnit();
                Session["cSvCheck"] = "0";
                RadTimeFrom.TimeView.Interval = new TimeSpan(0, common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]), 0);
                //RadTimeFrom.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["StTime"].ToString()), 0, 0);
                //RadTimeFrom.TimeView.EndTime = new TimeSpan(int.Parse(Request.QueryString["EndTime"].ToString()), 0, 0);
                if (Session["EncounterId"] != null && Session["RegistrationId"] != null
                    && (common.myStr(Session["ModuleName"]) == "EMR" || common.myStr(Session["ModuleName"]) == "Ward Management"))
                {
                    //ddlSearchOn.SelectedValue = "0";
                    txtPatientNo.Text = common.myInt(Session["RegistrationNo"]).ToString();
                    txtPatientNo.Enabled = false;
                    SearchByPatientNo();
                    ddlSurgeon.SelectedIndex = ddlSurgeon.Items.IndexOf(ddlSurgeon.Items.FindItemByValue(common.myInt(Session["DoctorId"]).ToString()));
                    //ddlSurgeon.SelectedValue = common.myInt(Session["DoctorId"]).ToString();
                }
                updrdoImplant.Visible = false;
                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowImplantRequiredInOTBooking", sConString).Equals("Y"))
                {
                    updrdoImplant.Visible = true;
                }
                if (Convert.ToInt64(Session["RegistrationNo"]) > 0)
                {
                    BindOTBookingDetails(common.myInt(Session["RegistrationNo"]));
                    int Hourtime = 0;
                    if (RadTimeFrom.SelectedDate != null)
                    {
                        if (RadTimeFrom.SelectedDate.Value.ToString("tt") == "PM")
                        {
                            Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh")) + 12;
                            if (Hourtime == 24)
                            {
                                Hourtime = 12;
                            }
                        }
                        else
                        {
                            Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh"));
                        }
                    }
                }
                else
                {
                    //RadTimeFrom.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]);
                    //hdnOTBookingDateTime.Value =common.myStr(RadTimeFrom.SelectedDate);
                    //dtpOTDate.SelectedDate = Convert.ToDateTime(Request.QueryString["OTDate"]);//
                }
                txtPatientNo.Focus();
                //PostBackOptions optionsSubmit = new PostBackOptions(ibtnSave);
                //ibtnSave.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";
                //ibtnSave.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);
                ibtnSave.Visible = true;
                if (common.myStr(Session["ModuleName"]) == "EMR" && common.myStr(Session["OPIP"]) == "O")
                {
                    // ibtnOpenSearchPatientPopup.Enabled = false;
                    txtPatientNo.Enabled = false;
                }
                //BaseC.Security baseUs = new BaseC.Security(sConString);
                //bool IsAuthorizeOTCheckIn = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn");
                //bool IsAuthorizeOTCheckOut = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckOut");
                //RadTimeFrom.Enabled = IsAuthorizeOTCheckIn;
                //RadTimeTo.Enabled = IsAuthorizeOTCheckOut;
                //if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "AllowOnlyEquipmentListOnOTBooking", sConString).Equals("Y"))
                //{
                //    chkEquipmentList.Checked = true;
                //    chkEquipmentList_OnCheckedChanged(null, null);
                //    chkEquipmentList.Enabled = false;
                //}
                //else
                //{
                //    chkEquipmentList.Checked = false;
                //    chkEquipmentList_OnCheckedChanged(null, null);
                //    chkEquipmentList.Enabled = true;
                //}
                ShowHideMandatory();
                bindTheatreName();
                bindDiagnosis();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = ex.Message.ToString();
                objException.HandleException(ex);
            }
        }
    }

    private void IsMandatoryField()
    {
        BaseC.HospitalSetup objHS;
        objHS = new BaseC.HospitalSetup(sConString);
        DataSet ds = objHS.getHospitalMandatoryFields(common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(common.myInt(Session["FacilityId"])),
                                    common.myStr("EMR"),
                                    common.myStr("OT Request"));
        bool IsDianosis = common.myBool(ds.Tables[0].Rows[0]["IsMandatoryField"]);

        if (IsDianosis)
        {
            lblStarDiagnosis.Visible = true;
        }
        else
        {
            lblStarDiagnosis.Visible = false;
        }
    }
    private void bindDiagnosis()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.getProvisionalAndFinalDiagnosis(common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProvisionalDiagnosis.Text = common.myStr(ds.Tables[0].Rows[0]["ClinicalDiagnosis"]);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void bindTheatreName()
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
        try
        {
            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlOTTheater.DataSource = ds.Tables[0];
                ddlOTTheater.DataTextField = "TheatreName";
                ddlOTTheater.DataValueField = "TheatreID";
                ddlOTTheater.DataBind();
                ddlOTTheater.Items.Insert(0, new RadComboBoxItem("", "0"));
            }
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
    public void setisAllDoctorDisplayOnAddService()
    {
        string setisAllDoctorDisplayOnAddService = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "isAllDoctorDisplayOnAddService", sConString);
        ViewState["setisAllDoctorDisplayOnAddService"] = "Y";
        if (setisAllDoctorDisplayOnAddService != "")
        {
            ViewState["setisAllDoctorDisplayOnAddService"] = setisAllDoctorDisplayOnAddService.ToUpper();
        }
    }
    public void setAllowOTBookingForUnregisteredPatient()
    {
        string AllowOTBookingForUnregisteredPatient = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "AllowOTBookingForUnregisteredPatient", sConString);
        ViewState["AllowOTBookingForUnregisteredPatient"] = "Y";
        if (AllowOTBookingForUnregisteredPatient != "")
        {
            ViewState["AllowOTBookingForUnregisteredPatient"] = AllowOTBookingForUnregisteredPatient.ToUpper();
        }
    }
    private void BindAnaesthesia()
    {
        try
        {
            BaseC.EMRMasters objSurgery = new BaseC.EMRMasters(sConString);
            DataSet ds = new DataSet();
            ds = objSurgery.GetService((common.myInt(Session["HospitalLocationID"].ToString())), "", "'AN'", common.myInt(Session["FacilityId"]));
            ddlAnaesthesia.DataSource = ds.Tables[0];
            ddlAnaesthesia.DataTextField = "ServiceName";
            ddlAnaesthesia.DataValueField = "ServiceID";
            ddlAnaesthesia.DataBind();
            ddlAnaesthesia.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlAnaesthesia.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindOTBookingDetails(int OTBookingId)
    {
        // BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        OpPrescription objOp = new OpPrescription(sConString);
        try
        {
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            string OTBookingNo = "";
            OTBookingId = 0;
            //  ds = objOTBooking.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            //  ds = objwcfOt.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            ds = objOp.getOTBookingDetails(OTBookingId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                dr = ds.Tables[0].Rows[0];
                hdnOTBookingID.Value = common.myStr(dr["OTRequestID"]);
                //   hdnOTBookingNO.Value = common.myStr(dr["OTBookingNo"]);
                dtpOTDate.SelectedDate = Convert.ToDateTime(dr["OTBookingDate"]);
                if (common.myStr(dr["RegistrationId"]) != "")
                {
                    //ddlSearchOn.SelectedValue = "0";
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    //txtPatientNo.Text = common.myStr(dr["RegistrationNo"]);
                    //txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
                    hdnEncounterId.Value = common.myStr(dr["EncounterID"]);
                }
                txtFName.Text = common.myStr(dr["FirstName"]);
                txtMName.Text = common.myStr(dr["MiddleName"]);
                txtLName.Text = common.myStr(dr["LastName"]);
                dtpDOB.SelectedDate = Convert.ToDateTime(dr["DateOfBirth"]);
                txtMobileNo.Text = common.myStr(dr["MobileNo"]);
                txtAgeYears.Text = common.myStr(dr["AgeYears"]);
                txtAgeMonths.Text = common.myStr(dr["AgeMonths"]);
                txtAgeDays.Text = common.myStr(dr["AgeDays"]);
                ddlGender.SelectedValue = common.myStr(dr["GenderId"]);
                txtDuration.Text = common.myStr(dr["OTDuration"]);
                ddlHours.SelectedValue = common.myStr(dr["OTDurationType"]);
                //lblIPNo.Text = common.myStr(dr["EncounterNo"]);
                //lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                //if (common.myBool(dr["isBloodRequired"]) == true)
                //{
                //    chkisBloodRequired.Checked = true;
                //}
                //else
                //{
                //    chkisBloodRequired.Checked = false;
                //}
                //if (common.myBool(dr["IsICURequired"]) == true)
                //{
                //    chkICUrequired.Checked = true;
                //}
                //else
                //{
                //    chkICUrequired.Checked = false;
                //}
                //ddlBloodUnit.SelectedValue = common.myStr(dr["Bloodunit"]);
                RadTimeFrom.SelectedDate = common.myDate(common.myStr(dr["FromTime"]));
                txtRemarks.Text = common.myStr(dr["Remarks"]);
                txtProvisionalDiagnosis.Text = common.myStr(dr["Diagnosis"]);
                //  ddlDiagnosisSearchCodes.SelectedValue = common.myStr(dr["DiagnosisSearchId"]);
                hdnOTBookingDateTime.Value = common.myStr(RadTimeFrom.SelectedDate);
                //  rdoImpReq.SelectedIndex = rdoImpReq.Items.IndexOf(rdoImpReq.Items.FindByValue(common.myStr(dr["isImplantRequired"])));
                rdoImpReq_SelectedIndexChanged(null, null);
                //txtImpReqRem.Text = common.myStr(dr["ImplantRequiredRemark"]);
                BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                int x = 0;
                Hashtable ht = new Hashtable();
                ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), common.myDate(hdnOTBookingDateTime.Value));
                ViewState["OTBook"] = ds.Tables[0];
                gvOTBook.DataSource = ds.Tables[0];
                gvOTBook.DataBind();
                // BindPatientProvisionalDiagnosis();
            }
            else
            {
                if (common.myInt(hdnRegistrationId.Value).Equals(0))
                {
                    hdnRegistrationId.Value = common.myInt(Session["RegistrationId"]).ToString();
                }
                if (common.myInt(hdnEncounterId.Value).Equals(0))
                {
                    hdnEncounterId.Value = common.myInt(Session["EncounterID"]).ToString();
                }
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
                gvOTBook.DataSource = ds.Tables[0];
                gvOTBook.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void chkEquipmentList_OnCheckedChanged(object sender, EventArgs e)
    {
        //if (chkEquipmentList.Checked == true)
        //{
        //    ddlOTEquipments.Visible = true;
        //    txtOTEquipment.Visible = false;
        //}
        //else
        //{
        //    ddlOTEquipments.Visible = false;
        //    txtOTEquipment.Visible = true;
        //}
    }
    protected void gvOTBook_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearControls();
            HiddenField hdnSurgeon = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnSurgeon");
            HiddenField hdnAsstSurgeon = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnAsstSurgeon");
            HiddenField hdnAnesthetist = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnAnesthetist");
            HiddenField hdnAssttAnesthetist = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnAssttAnesthetist");
            HiddenField hdnAnesthesiaId = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnAnesthesiaId");
            HiddenField hdnScrubNurse = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnScrubNurse");
            HiddenField hdnFloorNurse = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnFloorNurse");
            HiddenField hdnPerfusionist = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnPerfusionist");
            HiddenField hdnTechnician = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnTechnician");
            HiddenField hdnEquipment = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnEquipment");
            HiddenField hdnEquipmentName = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnEquipmentName");
            //if (hdnEquipmentName.Value != "")
            //{
            //    chkEquipmentList.Checked = false;
            //    chkEquipmentList_OnCheckedChanged(null, null);
            //    chkEquipmentList.Enabled = false;
            //}
            //else
            //{
            //    chkEquipmentList.Checked = true;
            //    chkEquipmentList_OnCheckedChanged(null, null);
            //    chkEquipmentList.Enabled = false;
            //}
            HiddenField hdnDepartment = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnDepartment");
            HiddenField hdnSubDepartment = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnSubDepartment");
            hdnSubDepartmentId.Value = hdnSubDepartment.Value;
            HiddenField hdnServiceID = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnServiceID");
            HiddenField hdnRemarks = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnRemarks");
            HiddenField hdnIsMain = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnIsMain");
            HiddenField hdnIsPackage = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnIsPackage");
            HiddenField hdnOTEquipMentsId = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnOTEquipMentsId");
            Label lblSurgeryName = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblSurgeryName");
            Label lblDate = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblDate");
            Label lblStartTime = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblStartTime");
            Label lblEndTime = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblEndTime");
            //CheckItems(hdnSurgeon.Value, ddlSurgeon);
            //CheckItems(hdnAsstSuregry.Value, ddlAsstSurgeon);
            //CheckItems(hdnAnesthetist.Value, ddlAnesthetist);
            if (common.myStr(hdnIsMain.Value).ToUpper().Trim() == "1")
            {
                cbMainSurgery.Checked = true;
            }
            else
            {
                cbMainSurgery.Checked = false;
            }
            string lastSurgonValue = hdnSurgeon.Value.Split(',').LastOrDefault().Trim();
            string lastAsstSurgeon = hdnAsstSurgeon.Value.Split(',').LastOrDefault().Trim(); ;
            string LasthAnesthetist = hdnAnesthetist.Value.Split(',').LastOrDefault().Trim();
            string LastAssttAnesthetist = hdnAssttAnesthetist.Value.Split(',').LastOrDefault().Trim();
            string[] OTEquipMentsId = hdnOTEquipMentsId.Value.Split(',');
            if (lastSurgonValue == string.Empty)
            {
                lastSurgonValue = hdnSurgeon.Value;
            }
            ddlSurgeon.SelectedValue = common.myStr(lastSurgonValue);
            ddlAsstSurgeon.SelectedValue = common.myStr(lastAsstSurgeon);
            ddlAnesthetist.SelectedValue = common.myStr(LasthAnesthetist);
            ddlAssttAnesthetist.SelectedValue = common.myStr(LastAssttAnesthetist);
            ddlAnaesthesia.SelectedValue = common.myStr(hdnAnesthesiaId.Value);
            //CheckItems(hdnScrubNurse.Value, ddlScrubNurse);
            //CheckItems(hdnFloorNurse.Value, ddlFloorNurse);
            //CheckItems(hdnPerfusionist.Value, ddlPerfusionist);
            //CheckItems(hdnTechnician.Value, ddlOTTechnician);
            //if (hdnEquipment.Value == "0" || hdnEquipment.Value == "")
            //{
            //    txtOTEquipment.Text = hdnEquipmentName.Value;
            //    // txtOTEquipment.Text = "Testing";
            //}
            //else
            //{
            //    CheckItems(hdnEquipment.Value, ddlOTEquipments);
            //}
            ddlDepartment.SelectedValue = hdnDepartment.Value;
            ddlDepartment_OnSelectedIndexChanged(this, null);
            ddlSubDepartment.SelectedValue = hdnSubDepartment.Value;
            ddlService.SelectedValue = hdnServiceID.Value;
            ddlService.Text = lblSurgeryName.Text;
            //bool IsMain = false;
            //if (hdnIsMain.Value == "1")
            //    IsMain = true;
            //cbMainSurgery.Checked = IsMain;
            rblServiceType.SelectedIndex = (hdnIsPackage != null && common.myBool(hdnIsPackage.Value)) ? 0 : 1;
            //cbMainSurgery.Enabled = IsMain;
            txtRemarks.Text = common.myStr(hdnRemarks.Value);
            //BaseC.Patient formatdate = new BaseC.Patient(sConString);
            //ViewState["Date"] = formatdate.FormatDate(lblDate.Text, "dd/MM/yyyy", "MM/dd/yyyy");
            dtpOTDate.SelectedDate = common.myDate(lblDate.Text);
            RadTimeFrom.SelectedDate = Convert.ToDateTime(lblStartTime.Text);
            ViewState["UpdateSelectIndex"] = gvOTBook.SelectedIndex;
            btnAddToList.Text = "Update";
            foreach (RadComboBoxItem itm in ddlOTEquipments.Items)
            {
                itm.Checked = false;
                for (int i = 0; i < OTEquipMentsId.Length; i++)
                {
                    if (common.myStr(itm.Value) == common.myStr(OTEquipMentsId[i]))
                        itm.Checked = true;

                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void ibtnOpenSearchPatientPopup_OnClick(object sender, EventArgs e)
    {
        //RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
        //RadWindow1.Height = 550;
        //RadWindow1.Width = 900;
        //RadWindow1.Top = 40;
        //RadWindow1.Left = 100;
        ////RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;
    }
    private void SearchByPatientNo()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        int RegId = common.myInt(hdnRegistrationId.Value) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(hdnRegistrationId.Value);
        string RegistrationNo = hdnRegistrationId.Value == "" ? common.myStr(hdnRegistrationNo.Value) : common.myStr(txtPatientNo.Text);
        int EncounterId = common.myInt(Session["EncounterId"]);
        try
        {
            if (txtPatientNo.Text.Trim().Length > 0)
            {
                int UserId = common.myInt(Session["UserId"]);
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                string PType = "I";
                //if (ddlSearchOn.SelectedValue == "1")
                //{
                //    RegistrationNo = common.myStr(hdnRegistrationNo.Value);
                //    hdnEncounterNo.Value = common.myStr(txtPatientNo.Text);
                //}
                //else
                //{
                //    hdnEncounterNo.Value = "";
                //    RegistrationNo = hdnRegistrationId.Value == "" ? common.myStr(hdnRegistrationNo.Value) : common.myStr(txtPatientNo.Text);
                //}
                ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, common.myInt(RegistrationNo), UserId, EncounterId, common.myStr(hdnEncounterNo.Value));
                DV = new DataView(ds.Tables[0]);
                DV.RowFilter = "StatusId<>8";
                if (DV.ToTable().Rows.Count == 0)
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, common.myInt(RegistrationNo), EncounterId, UserId);
                    DV = new DataView(ds.Tables[0]);
                    PType = "O";
                }
                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["NewRegNo"] = common.myStr(DV.ToTable().Rows[0]["RegistrationNo"]);
                    ViewState["NewRegID"] = common.myStr(DV.ToTable().Rows[0]["RegistrationId"]);
                    hdnRegistrationId.Value = common.myStr(DV.ToTable().Rows[0]["RegistrationId"]);
                    txtRegNo.Text = common.myStr(DV.ToTable().Rows[0]["RegistrationNo"]);
                    bindAdvanceDetails();
                    txtLName.Text = common.myStr(DV.ToTable().Rows[0]["LastName"]);
                    txtFName.Text = common.myStr(DV.ToTable().Rows[0]["FirstName"]);
                    txtMName.Text = common.myStr(DV.ToTable().Rows[0]["MiddleName"]);
                    dtpDOB.SelectedDate = common.myDate(DV.ToTable().Rows[0]["DOB"]);
                    txtMobileNo.Text = common.myStr(DV.ToTable().Rows[0]["MobileNo"]);
                    //if (PType == "O")
                    //{
                    //    txtAdvance.Text = common.myStr(ViewState["AdvanceAmt"]);
                    //    hdnAdvance.Value = txtAdvance.Text;
                    //}
                    //else
                    //{
                    //    txtAdvance.Text = common.myStr(DV.ToTable().Rows[0]["AdvanceAmt"]);
                    //    hdnAdvance.Value = txtAdvance.Text;
                    //}
                    hdnCompanyCode.Value = common.myStr(DV.ToTable().Rows[0]["CompanyCode"]);
                    imgCalYear_Click(this, null);
                    ddlGender.SelectedValue = DV.ToTable().Rows[0]["GenderId"].ToString();
                    //  lblAdmissionDate.Text = "";
                    //  lblIPNo.Text = "";
                    if (DV.ToTable().Rows[0]["OPIP"].ToString().Trim() == "I")
                    {
                        hdnEncounterId.Value = common.myStr(DV.ToTable().Rows[0]["EncounterId"]);
                        //lblAdmissionDate.Text = DV.ToTable().Rows[0]["AdmissionDate"].ToString();
                        // lblIPNo.Text = DV.ToTable().Rows[0]["EncounterNo"].ToString();
                    }
                    lblMessage.Text = "";
                    ds.Dispose();
                    if (common.myInt(hdnRegistrationId.Value) > 0)
                    {
                        BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                        //string strDate =DateTime.Now.ToString("yyyy/MM/dd h:mm:ss");
                        //  DateTime dt =(DateTime) strDate.ToString("yyyy/MM/dd h:mm:ss tt");
                        Hashtable ht = new Hashtable();
                        ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), common.myDate(hdnOTBookingDateTime.Value));
                        hdnIsUnplanned.Value = common.myStr(ht["@bitIsUnPlannedReturnToOT"]);
                        //if (common.myStr(ht["@bitIsUnPlannedReturnToOT"]) == "True")
                        //{
                        //    rblUnplanned.SelectedIndex = 0;
                        //    rblUnplanned.Enabled = true;
                        //}
                        //lblChkoutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);
                    }
                }
                else
                {
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
            GetgvbindemrotRequest(common.myInt(ViewState["NewRegNo"]));
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
            DV.Dispose();
            objIPBill = null;
            bC = null;
            objPatient = null;
        }
    }
    protected void btnSearchByPatientNo_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        int RegId = common.myInt(hdnRegistrationId.Value) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(hdnRegistrationId.Value);
        txtPatientNo.Text = hdnRegistrationId.Value == "" ? common.myStr(hdnRegistrationNo.Value) : common.myStr(txtPatientNo.Text);
        try
        {
            if (txtPatientNo.Text.Trim().Length > 0)
            {
                int EncounterId = 0;
                int UserId = common.myInt(Session["UserId"]);
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                string PType = "I";
                string RegistrationNo = "";
                //if (ddlSearchOn.SelectedValue == "1")
                //{
                //    RegistrationNo = common.myStr(hdnRegistrationNo.Value);
                //    hdnEncounterNo.Value = common.myStr(txtPatientNo.Text);
                //}
                //else
                //{
                //    hdnEncounterNo.Value = "";
                //    RegistrationNo = hdnRegistrationId.Value == "" ? common.myStr(hdnRegistrationNo.Value) : common.myStr(txtPatientNo.Text);
                //}
                ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, common.myInt(RegistrationNo), UserId, EncounterId, common.myStr(hdnEncounterNo.Value));
                DV = new DataView(ds.Tables[0]);
                DV.RowFilter = "StatusId<>8";
                if (DV.ToTable().Rows.Count == 0)
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, common.myInt(RegistrationNo), EncounterId, UserId);
                    DV = new DataView(ds.Tables[0]);
                    PType = "O";
                }
                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["NewRegNo"] = common.myStr(DV.ToTable().Rows[0]["RegistrationNo"]);
                    ViewState["NewRegID"] = common.myStr(DV.ToTable().Rows[0]["RegistrationId"]);
                    hdnRegistrationId.Value = common.myStr(DV.ToTable().Rows[0]["RegistrationId"]);
                    txtRegNo.Text = common.myStr(DV.ToTable().Rows[0]["RegistrationNo"]);
                    bindAdvanceDetails();
                    txtLName.Text = common.myStr(DV.ToTable().Rows[0]["LastName"]);
                    txtFName.Text = common.myStr(DV.ToTable().Rows[0]["FirstName"]);
                    txtMName.Text = common.myStr(DV.ToTable().Rows[0]["MiddleName"]);
                    dtpDOB.SelectedDate = common.myDate(DV.ToTable().Rows[0]["DOB"]);
                    txtMobileNo.Text = common.myStr(DV.ToTable().Rows[0]["MobileNo"]);
                    //if (PType == "O")
                    //{
                    //    txtAdvance.Text = common.myStr(ViewState["AdvanceAmt"]);
                    //    hdnAdvance.Value = txtAdvance.Text;
                    //}
                    //else
                    //{
                    //    txtAdvance.Text = common.myStr(DV.ToTable().Rows[0]["AdvanceAmt"]);
                    //    hdnAdvance.Value = txtAdvance.Text;
                    //}
                    hdnCompanyCode.Value = common.myStr(DV.ToTable().Rows[0]["CompanyCode"]);
                    imgCalYear_Click(this, null);
                    ddlGender.SelectedValue = DV.ToTable().Rows[0]["GenderId"].ToString();
                    //  lblAdmissionDate.Text = "";
                    //  lblIPNo.Text = "";
                    if (DV.ToTable().Rows[0]["OPIP"].ToString().Trim() == "I")
                    {
                        hdnEncounterId.Value = common.myStr(DV.ToTable().Rows[0]["EncounterId"]);
                        //lblAdmissionDate.Text = DV.ToTable().Rows[0]["AdmissionDate"].ToString();
                        // lblIPNo.Text = DV.ToTable().Rows[0]["EncounterNo"].ToString();
                    }
                    lblMessage.Text = "";
                    ds.Dispose();
                    if (common.myInt(hdnRegistrationId.Value) > 0)
                    {
                        BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                        //string strDate =DateTime.Now.ToString("yyyy/MM/dd h:mm:ss");
                        //  DateTime dt =(DateTime) strDate.ToString("yyyy/MM/dd h:mm:ss tt");
                        Hashtable ht = new Hashtable();
                        ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), common.myDate(hdnOTBookingDateTime.Value));
                        hdnIsUnplanned.Value = common.myStr(ht["@bitIsUnPlannedReturnToOT"]);
                        //if (common.myStr(ht["@bitIsUnPlannedReturnToOT"]) == "True")
                        //{
                        //    rblUnplanned.SelectedIndex = 0;
                        //    rblUnplanned.Enabled = true;
                        //}
                        //lblChkoutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);
                    }
                }
                else
                {
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
            GetgvbindemrotRequest(common.myInt(ViewState["NewRegNo"]));
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
            DV.Dispose();
            objIPBill = null;
            bC = null;
            objPatient = null;
        }
    }
    private void bindAdvanceDetails()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            if (common.myInt(hdnRegistrationId.Value) > 0)
            {
                ds = objBill.getAdvanceAndOutstanding(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(hdnRegistrationId.Value),
                                common.myInt(Session["FacilityId"]), "0", "O", 0);
                DataView dv = ds.Tables[3].DefaultView;
                // dv.RowFilter = "Balance >0 and opip in ('I','B') and isadvance=1";
                //dv.RowFilter = "Balance >0 AND DocTypeId = 1";
                DataTable dt = dv.ToTable();
                ViewState["AdvanceAmt"] = common.myStr(dt.Compute("SUM(Balance)", string.Empty));
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { ds.Dispose(); objBill = null; }
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
    }
    private bool ValidateControls()
    {
        if (common.myStr(txtFName.Text) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter first name...";
            return false;
        }
        if (common.myStr(txtDuration.Text) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter Duration";
            return false;
        }
        if (dtpDOB.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select DOB...";
            return false;
        }
        if (common.myInt(ddlGender.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select gender...";
            return false;
        }
        if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select from time...";
            return false;
        }
        if (common.myInt(ddlService.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select a service...";
            return false;
        }
        if (common.myInt(ddlSurgeon.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select a surgeon...";
            Alert.ShowAjaxMsg("Please select surgeon...", Page);
            return false;
        }
        if (lblAnesthetiststar.Visible)
        {
            if (common.myInt(ddlAnesthetist.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select an anesthetist...";
                Alert.ShowAjaxMsg("Please select an anesthetist...", Page);
                return false;
            }
        }
        if (lblAnaesthesiastar.Visible)
        {
            if (common.myInt(ddlAnaesthesia.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select anaesthesia...";
                Alert.ShowAjaxMsg("Please select anaesthesia...", Page);
                return false;
            }
        }
        return true;
    }
    protected void rblServiceType_OnSelectedIndexChanged(object sender, EventArgs E)
    {
        // ddlDepartment.SelectedValue = "0"; ;
        //ddlDepartment.Text = "";
        BindDepartment();
        ddlSubDepartment.Items.Clear();
        ddlSubDepartment.Text = "";
        ddlService.Items.Clear();
        ddlService.Text = "";
    }
    private void BindOTGrid()
    {
        try
        {
            DataRow Dr;
            DataTable dtOT = new DataTable();
            int IsMain = 0;
            if (btnAddToList.Text.Trim() == "Add")
            {
                if (gvOTBook.Rows.Count > 0 && ViewState["OTBook"] != null)
                {
                    dtOT = (DataTable)ViewState["OTBook"];
                    Dr = dtOT.NewRow();
                }
                else
                {
                    if (cbMainSurgery.Checked == true)
                        IsMain = 1;
                    Dr = dtOT.NewRow();
                    dtOT.Columns.Add("OTRequestID");
                    dtOT.Columns.Add("ServiceName");
                    dtOT.Columns.Add("SR");
                    dtOT.Columns.Add("OTBookingDate");
                    dtOT.Columns.Add("OTBookingDateF");
                    dtOT.Columns.Add("FromTime");
                    dtOT.Columns.Add("ToTime");
                    dtOT.Columns.Add("AS");
                    dtOT.Columns.Add("AN");
                    dtOT.Columns.Add("AAN");
                    dtOT.Columns.Add("ServiceID");
                    dtOT.Columns.Add("PR");
                    dtOT.Columns.Add("SN");
                    dtOT.Columns.Add("FN");
                    dtOT.Columns.Add("TC");
                    dtOT.Columns.Add("Equipments");
                    dtOT.Columns.Add("EquipmentName");
                    dtOT.Columns.Add("DepartmentId");
                    dtOT.Columns.Add("SubDepartmentId");
                    dtOT.Columns.Add("AnesthesiaId");
                    dtOT.Columns.Add("Remarks");
                    dtOT.Columns.Add("IsSurgeryMain");
                    dtOT.Columns.Add("OTServiceDetailId");
                    dtOT.Columns.Add("IsPackage");
                    dtOT.Columns.Add("OTEquipMentsId");
                    // dtOT.Columns.Add("IsInfectiousCase");
                }
                Dr["OTRequestID"] = "0";
                Dr["ServiceName"] = ddlService.Text;
                //string strPerfusionistIds = common.GetCheckedItems(ddlPerfusionist);
                //string strScrubNurseIds = common.GetCheckedItems(ddlScrubNurse);
                //string strFloorNurseIds = common.GetCheckedItems(ddlFloorNurse);
                //string strOTTechnicianIds = common.GetCheckedItems(ddlOTTechnician);
                //string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);
                Dr["SR"] = ddlSurgeon.SelectedValue;
                //Dr["SurgeonName"] = ddlSurgeon.SelectedItem.Text;
                Dr["OTBookingDate"] = common.myDate(dtpOTDate.SelectedDate);
                Dr["OTBookingDateF"] = dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                Dr["FromTime"] = RadTimeFrom.SelectedDate.Value.TimeOfDay;
                Dr["AS"] = ddlAsstSurgeon.SelectedValue;
                Dr["AN"] = ddlAnesthetist.SelectedValue;
                Dr["AAN"] = ddlAssttAnesthetist.SelectedValue;
                //Dr["PR"] = strPerfusionistIds;
                //Dr["SN"] = strScrubNurseIds;
                //Dr["FN"] = strFloorNurseIds;
                Dr["ServiceID"] = ddlService.SelectedValue;
                //Dr["TC"] = strOTTechnicianIds;
                if (common.myInt(ddlDepartment.SelectedValue) > 0)
                {
                    Dr["DepartmentId"] = common.myStr(ddlDepartment.SelectedValue);
                }
                else
                {
                    Dr["DepartmentId"] = DBNull.Value;
                }
                if (common.myInt(ddlSubDepartment.SelectedValue) > 0)
                {
                    Dr["SubDepartmentId"] = common.myStr(ddlSubDepartment.SelectedValue);
                }
                else
                {
                    Dr["SubDepartmentId"] = DBNull.Value;
                }
                //Dr["Equipments"] = chkEquipmentList.Checked == false ? null : strEquipmentIds;
                //Dr["EquipmentName"] = chkEquipmentList.Checked == false ? txtOTEquipment.Text : null;
                if (common.myInt(ddlAnaesthesia.SelectedValue) > 0)
                {
                    Dr["AnesthesiaId"] = ddlAnaesthesia.SelectedValue;
                }
                else
                {
                    Dr["AnesthesiaId"] = DBNull.Value;
                }
                Dr["Remarks"] = txtRemarks.Text;
                string OTEquipmentsid = string.Empty;
                foreach (RadComboBoxItem currentItem in ddlOTEquipments.Items)
                {
                    if (currentItem.Checked)
                    {
                        if (string.IsNullOrEmpty(OTEquipmentsid))
                        {
                            OTEquipmentsid = currentItem.Value;
                        }
                        else
                        {
                            OTEquipmentsid = OTEquipmentsid + "," + currentItem.Value;
                        }
                    }
                }
                Dr["OTEquipMentsId"] = OTEquipmentsid;
                Dr["IsSurgeryMain"] = common.myStr(IsMain);
                //Dr["IsPackage"] = rblServiceType.SelectedIndex.Equals(0);
                Dr["OTServiceDetailId"] = 0;
                // Dr["IsInfectiousCase"] = common.myBool(rdoInfectiousCase.SelectedValue);
                dtOT.Rows.Add(Dr);
                ViewState["OTBook"] = dtOT;
                gvOTBook.DataSource = dtOT;
                gvOTBook.DataBind();
                cbMainSurgery.Checked = false;
                if (cbMainSurgery.Checked)
                {
                    cbMainSurgery.Enabled = false;
                }
            }
            else
            {
                if (gvOTBook.Rows.Count > 0)
                {
                    int updIndex = Convert.ToInt32(ViewState["UpdateSelectIndex"]);
                    DataTable dt1 = (DataTable)ViewState["OTBook"];
                    dt1.Rows[updIndex]["ServiceName"] = ddlService.Text;
                    //string strPerfusionistIds = common.GetCheckedItems(ddlPerfusionist);
                    //string strScrubNurseIds = common.GetCheckedItems(ddlScrubNurse);
                    //string strFloorNurseIds = common.GetCheckedItems(ddlFloorNurse);
                    //string strOTTechnicianIds = common.GetCheckedItems(ddlOTTechnician);
                    //string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);
                    IsMain = 0;
                    if (cbMainSurgery.Checked == true)
                        IsMain = 1;
                    dt1.Rows[updIndex]["SR"] = ddlSurgeon.SelectedValue;
                    //dt1.Rows[updIndex]["SurgeonName"] = ddlSurgeon.SelectedItem.Text;
                    dt1.Rows[updIndex]["OTBookingDate"] = common.myDate(dtpOTDate.SelectedDate);
                    dt1.Rows[updIndex]["OTBookingDateF"] = dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                    dt1.Rows[updIndex]["FromTime"] = RadTimeFrom.SelectedDate.Value.TimeOfDay;
                    dt1.Rows[updIndex]["AS"] = ddlAsstSurgeon.SelectedValue;
                    dt1.Rows[updIndex]["AN"] = ddlAnesthetist.SelectedValue;
                    dt1.Rows[updIndex]["AAN"] = ddlAssttAnesthetist.SelectedValue;
                    //dt1.Rows[updIndex]["PR"] = strPerfusionistIds;
                    //dt1.Rows[updIndex]["SN"] = strScrubNurseIds;
                    //dt1.Rows[updIndex]["FN"] = strFloorNurseIds;
                    if (common.myInt(ddlDepartment.SelectedValue) > 0)
                    {
                        dt1.Rows[updIndex]["DepartmentId"] = common.myStr(ddlDepartment.SelectedValue);
                    }
                    else
                    {
                        dt1.Rows[updIndex]["DepartmentId"] = DBNull.Value;
                    }
                    if (common.myInt(ddlSubDepartment.SelectedValue) > 0)
                    {
                        dt1.Rows[updIndex]["SubDepartmentId"] = common.myStr(ddlSubDepartment.SelectedValue);
                    }
                    else
                    {
                        dt1.Rows[updIndex]["SubDepartmentId"] = DBNull.Value;
                    }
                    dt1.Rows[updIndex]["ServiceID"] = ddlService.SelectedValue;
                    //   dt1.Rows[updIndex]["TC"] = strOTTechnicianIds;
                    //dt1.Rows[updIndex]["Equipments"] = chkEquipmentList.Checked == false ? null : strEquipmentIds;
                    //dt1.Rows[updIndex]["EquipmentName"] = chkEquipmentList.Checked == false ? txtOTEquipment.Text : null;
                    if (common.myInt(ddlAnaesthesia.SelectedValue) > 0)
                    {
                        dt1.Rows[updIndex]["AnesthesiaId"] = ddlAnaesthesia.SelectedValue;
                    }
                    else
                    {
                        dt1.Rows[updIndex]["AnesthesiaId"] = DBNull.Value;
                    }
                    string OTEquipmentsid = string.Empty;
                    foreach (RadComboBoxItem currentItem in ddlOTEquipments.Items)
                    {
                        if (currentItem.Checked)
                        {
                            if (string.IsNullOrEmpty(OTEquipmentsid))
                            {
                                OTEquipmentsid = currentItem.Value;
                            }
                            else
                            {
                                OTEquipmentsid = OTEquipmentsid + "," + currentItem.Value;
                            }
                        }
                    }
                    dt1.Rows[updIndex]["OTEquipMentsId"] = OTEquipmentsid;
                    dt1.Rows[updIndex]["Remarks"] = txtRemarks.Text;
                    //dt1.Rows[updIndex]["IsInfectiousCase"] = common.myBool(rdoInfectiousCase.SelectedValue);
                    dt1.Rows[updIndex]["IsSurgeryMain"] = Convert.ToString(IsMain);
                    // dt1.Rows[updIndex]["IsPackage"] = rblServiceType.SelectedIndex.Equals(0);
                    ViewState["OTBook"] = dt1;
                    gvOTBook.DataSource = dt1;
                    btnAddToList.Text = "Add";
                    gvOTBook.DataBind();
                    cbMainSurgery.Checked = false;
                    if (cbMainSurgery.Checked)
                    {
                        cbMainSurgery.Enabled = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    private void ClearControls()
    {
        ddlDepartment.SelectedIndex = 0;
        ddlSubDepartment.SelectedIndex = 0;
        ddlService.Text = "";
        ddlSurgeon.SelectedIndex = 0;
        ddlAsstSurgeon.SelectedIndex = 0;
        ddlAnesthetist.SelectedIndex = 0;
        ddlAssttAnesthetist.SelectedIndex = 0;
        //common.UnCheckAllCheckedItems(ddlSurgeon);
        //common.UnCheckAllCheckedItems(ddlAsstSurgeon);
        //common.UnCheckAllCheckedItems(ddlAnesthetist);
        //Mahendra 
        //common.UnCheckAllCheckedItems(ddlPerfusionist);
        //common.UnCheckAllCheckedItems(ddlScrubNurse);
        //common.UnCheckAllCheckedItems(ddlFloorNurse);
        //common.UnCheckAllCheckedItems(ddlOTTechnician);
        //txtOTEquipment.Text = "";
        //common.UnCheckAllCheckedItems(ddlOTEquipments);
        hdnSubDepartmentId.Value = "0";
    }
    protected void BindDepartment()
    {
        DataSet ds = new DataSet();
        try
        {
            ddlDepartment.Items.Clear();
            BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
            BaseC.RestFulAPI objwcfcm = new BaseC.RestFulAPI(sConString);
            string strDepartmentType = "'" + common.myStr(rblServiceType.SelectedValue) + "'";
            //    string strDepartmentType = common.myStr(rblServiceType.SelectedValue);
            //  DataSet ds = bMstr.GetHospitalDepartment(Convert.ToInt32(Session["HospitalLocationID"].ToString()), strDepartmentType);
            ds = objwcfcm.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), strDepartmentType);
            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", ""));
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
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindSudepartment();
    }
    private void BindSudepartment()
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objwcfcm = new BaseC.RestFulAPI(sConString);
        try
        {
            if (common.myInt(ddlDepartment.SelectedValue) > 0)
            {
                ddlSubDepartment.Items.Clear();
                ddlService.Items.Clear();
                // bMstr = new BaseC.EMRMasters(sConString);
                //  objLabRequest = new BaseC.clsLabRequest(sConString);
                string strDepartmentType = "'" + common.myStr(rblServiceType.SelectedValue) + "'";
                int iSubDeptId = 0;
                if (common.myInt(hdnSubDepartmentId.Value) > 0)
                {
                    strDepartmentType = "";
                    iSubDeptId = common.myInt(hdnSubDepartmentId.Value);
                }
                //  ds = bMstr.GetHospitalSubDepartment(Convert.ToInt32(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue),common.myStr("'S','P'"),0);
                ds = objwcfcm.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), strDepartmentType, iSubDeptId);
                ddlSubDepartment.DataSource = ds.Tables[0];
                ddlSubDepartment.DataTextField = "SubName";
                ddlSubDepartment.DataValueField = "SubDeptId";
                ddlSubDepartment.DataBind();
                ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", ""));
                ddlSubDepartment.SelectedIndex = 0;
            }
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
    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            //if (e.Text != "")
            //{
            DataTable data = GetData(e.Text);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;
            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(data.Rows[i]["ServiceName"]) + " " + common.myStr(data.Rows[i]["RefServiceCode"]) + " " + common.myStr(data.Rows[i]["CPTCode"]);
                item.Value = data.Rows[i]["ServiceId"].ToString();
                //ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ServiceName"].ToString(), data.Rows[i]["ServiceId"].ToString()));
                item.Attributes.Add("ServiceName", data.Rows[i]["ServiceName"].ToString());
                item.Attributes.Add("DepartmentId", data.Rows[i]["DepartmentId"].ToString());
                item.Attributes.Add("SubDeptId", data.Rows[i]["SubDeptId"].ToString());
                item.Attributes.Add("RefServiceCode", data.Rows[i]["RefServiceCode"].ToString());
                item.Attributes.Add("CPTCode", data.Rows[i]["CPTCode"].ToString());
                this.ddlService.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        string ServiceName = text + "%";
        string strDepartmentType = "'" + common.myStr(rblServiceType.SelectedValue) + "'";
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = objCommon.GetHospitalServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), 0, 0);
        DataTable data = new DataTable();
        if (ds.Tables[0].Rows.Count > 0)
        {
            data = ds.Tables[0];
        }
        return data;
    }
    private void BindProviderList()
    {
        try
        {
            DataSet ds = new DataSet();
            DataView dv = new DataView();
            BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
            ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type In('SR','AS','AN')"; // Surgeon
            //  dv.RowFilter = "EmployeeTypeCode='D' OR Type='AN'"; // Surgeon
            if (dv.ToTable().Rows.Count > 0)
            {
                //while (dv.ToTable().Rows.Count > 0)
                //{
                //    // common.myInt(dv[0]["EmployeeId"].ToString());
                //     if ((common.myInt(dv[0]["EmployeeId"].ToString())== common.myInt(Session["EmployeeId"])))
                //     {
                //     }
                //}
                if (common.myStr(ViewState["setisAllDoctorDisplayOnAddService"]).ToUpper() == "N")
                {
                    dv.RowFilter = "Type='SR'";
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        dv.RowFilter = "Type In('SR','AS','AN')";
                    }
                }
                ddlSurgeon.DataSource = dv.ToTable();
                ddlSurgeon.DataTextField = "EmployeeName";
                ddlSurgeon.DataValueField = "EmployeeId";
                ddlSurgeon.DataBind();
                ddlSurgeon.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
                dv.RowFilter = "Type In('SR','AS','AN')"; // Surgeon
                if (dv.ToTable().Rows.Count > 0)
                {
                    for (int i = 0; i < dv.ToTable().Rows.Count; i++)
                    {
                        if ((common.myInt(dv[i]["EmployeeId"].ToString()) == common.myInt(Session["EmployeeId"])))
                        {
                            ddlSurgeon.SelectedIndex = ddlSurgeon.Items.IndexOf(ddlSurgeon.Items.FindItemByValue(common.myInt(Session["EmployeeId"]).ToString()));
                            break;
                        }
                    }
                }
                //  }
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='SR' OR Type='AS' OR Type='AN'"; // Asst. Surgeon
            //   dv.RowFilter = "EmployeeTypeCode='D'";
            if (dv.ToTable().Rows.Count > 0)
            {
                if (common.myStr(ViewState["setisAllDoctorDisplayOnAddService"]).ToUpper() == "N")
                {
                    dv.RowFilter = "Type='AS'";
                    if (dv.ToTable().Rows.Count == 0)
                    {
                        dv.RowFilter = "Type='SR' OR Type='AS' OR Type='AN'";
                    }
                }
                else
                {
                    dv.RowFilter = "Type='SR' OR Type='AS' OR Type='AN'";
                }
                ddlAsstSurgeon.DataSource = dv.ToTable();
                ddlAsstSurgeon.DataTextField = "EmployeeName";
                ddlAsstSurgeon.DataValueField = "EmployeeId";
                ddlAsstSurgeon.DataBind();
                ddlAsstSurgeon.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='AN'"; // Anesthetist
            //dv.RowFilter = "EmployeeTypeCode='D'";
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlAnesthetist.DataSource = dv.ToTable();
                ddlAnesthetist.DataTextField = "EmployeeName";
                ddlAnesthetist.DataValueField = "EmployeeId";
                ddlAnesthetist.DataBind();
                ddlAnesthetist.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='AN'"; // Anesthetist
            //dv.RowFilter = "EmployeeTypeCode='D'";
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlAssttAnesthetist.DataSource = dv.ToTable();
                ddlAssttAnesthetist.DataTextField = "EmployeeName";
                ddlAssttAnesthetist.DataValueField = "EmployeeId";
                ddlAssttAnesthetist.DataBind();
                ddlAssttAnesthetist.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='PR'"; // Perfusionist
            //if (dv.ToTable().Rows.Count > 0)
            //{
            //    ddlPerfusionist.DataSource = dv.ToTable();
            //    ddlPerfusionist.DataTextField = "EmployeeName";
            //    ddlPerfusionist.DataValueField = "EmployeeId";
            //    ddlPerfusionist.DataBind();
            //    //ddlPerfusionist.Items.Insert(0, new RadComboBoxItem("", "0"));
            //    dv.RowFilter = "";
            //}
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='SN'"; // Scrub Nurse
            //if (dv.ToTable().Rows.Count > 0)
            //{
            //    ddlScrubNurse.DataSource = dv.ToTable();
            //    ddlScrubNurse.DataTextField = "EmployeeName";
            //    ddlScrubNurse.DataValueField = "EmployeeId";
            //    ddlScrubNurse.DataBind();
            //    //ddlScrubNurse.Items.Insert(0, new RadComboBoxItem("", "0"));
            //    dv.RowFilter = "";
            //}
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='FN'"; // Scrub Nurse
            //if (dv.ToTable().Rows.Count > 0)
            //{
            //    ddlFloorNurse.DataSource = dv.ToTable();
            //    ddlFloorNurse.DataTextField = "EmployeeName";
            //    ddlFloorNurse.DataValueField = "EmployeeId";
            //    ddlFloorNurse.DataBind();
            //    //ddlFloorNurse.Items.Insert(0, new RadComboBoxItem("", "0"));
            //    dv.RowFilter = "";
            //}
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='TC'"; // Scrub Nurse
            //if (dv.ToTable().Rows.Count > 0)
            //{
            //    ddlOTTechnician.DataSource = dv.ToTable();
            //    ddlOTTechnician.DataTextField = "EmployeeName";
            //    ddlOTTechnician.DataValueField = "EmployeeId";
            //    ddlOTTechnician.DataBind();
            //    //ddlOTTechnician.Items.Insert(0, new RadComboBoxItem("", "0"));
            //    dv.RowFilter = "";
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
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
        string strDateAppointment = ""; //= formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString());
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }
    protected void RadTimeFrom_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
        {
            if (ViewState["Duration"] != null)
            {
                TimeSpan ts = (TimeSpan)ViewState["Duration"];
            }
        }
    }
    protected void RadTimeTo_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
        {
            if (ViewState["Duration"] != null)
            {
                TimeSpan ts = (TimeSpan)ViewState["Duration"];
            }
        }
    }
    protected void btnAddToList_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (ValidateData() && ValidateControls())
        {
            BindOTGrid();
            ClearControls();
            ibtnSave.Visible = true;
        }
    }
    protected void lnkViewDetails_Click(object sender, EventArgs e)
    {
        LinkButton lbtnViewDetails = sender as LinkButton;
        int ServiceId = common.myInt(lbtnViewDetails.CommandArgument);
        //GridViewRow row = (GridViewRow)lbtnViewDetails.NamingContainer;
        RadWindow1.NavigateUrl = "~/OTScheduler/OTDetails.aspx?OTBookingId=" + hdnOTBookingID.Value + "&ServiceId=" + ServiceId;
        RadWindow1.Height = 580;
        RadWindow1.Width = 750;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {

        //yogesh start 20/07/2022


        if (lblStarDiagnosis.Visible == true && txtProvisionalDiagnosis.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select Diagnosis!";
            return;
        }

        else if (dtpOTDate.SelectedDate == null)
        {
            lblMessage.Text = "Select OT Date!";
            return;
        }
        else if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.Text = "Select time!";
            return;
        }

        else if (txtDuration.Text == "")
        {
            lblMessage.Text = "Select Duration!";
            return;
        }
        else if (dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
        {
            DateTime d = RadTimeFrom.SelectedDate.Value;
            DateTime time = common.myDate(d.ToString("HH:mm:ss"));
            if (time < common.myDate(DateTime.Now.ToString("HH:mm:ss")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Valid Time";
                return;
            }

        }
        else if (gvOTBook.Rows.Count <= 0 || ViewState["OTBook"] == null)
        {
            lblMessage.Text = "Please add service!";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }


        //  ValidateControls();

        if (common.myStr(txtFName.Text) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter first name...";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        if (common.myStr(txtDuration.Text) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter Duration";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        if (dtpDOB.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select DOB...";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        if (common.myInt(ddlGender.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select gender...";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select from time...";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        if (common.myInt(ddlService.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select a service...";
            return;
        }
        else
        {
            lblMessage.Text = "";
        }
        //if (common.myInt(ddlSurgeon.SelectedValue) == 0)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Please select a surgeon...";
        //    return;
        //}
        //else
        //{
        //    lblMessage.Text = "";
        //}
        if (lblAnesthetiststar.Visible)
        {
            if (common.myInt(ddlAnesthetist.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select an anesthetist...";
                return;
            }
            else
            {
                lblMessage.Text = "";
            }
        }
        if (lblAnaesthesiastar.Visible)
        {
            if (common.myInt(ddlAnaesthesia.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select anaesthesia...";
                return;
            }
            else
            {
                lblMessage.Text = "";
            }
        }


        // ClearControls();
        // yogesh end 20/07/2022

        OpPrescription objprescription = new OpPrescription(sConString);
        bool IsEmergency = false;
        if (Validatetext() == false)
        {
            return;
        }
        if (dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
        {
            DateTime d = RadTimeFrom.SelectedDate.Value;
            DateTime time = common.myDate(d.ToString("HH:mm:ss"));
            if (time < common.myDate(DateTime.Now.ToString("HH:mm:ss")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Valid Time";
                return;
            }
            else
            {
                lblMessage.Text = String.Empty;
            }
        }
        if (RadioIsElective.Checked)
        {
            IsEmergency = false;
        }
        else
        {
            IsEmergency = true;
        }
        StringBuilder strXMLItems = new StringBuilder();
        StringBuilder sXmlResourceDetails = new StringBuilder();
        ArrayList coll = new ArrayList();
        ArrayList colResource = new ArrayList();
        Hashtable htOut = new Hashtable();
        try
        {
            foreach (GridViewRow item in gvOTBook.Rows)
            {
                HiddenField hdnServiceID = (HiddenField)item.FindControl("hdnServiceID");
                HiddenField hdnOTServiceDetailId = (HiddenField)item.FindControl("hdnOTServiceDetailId");
                HiddenField hdnSurgeon = (HiddenField)item.FindControl("hdnSurgeon");
                HiddenField hdnAsstSurgeon = (HiddenField)item.FindControl("hdnAsstSurgeon");
                HiddenField hdnAnesthetist = (HiddenField)item.FindControl("hdnAnesthetist");
                HiddenField hdnAssttAnesthetist = (HiddenField)item.FindControl("hdnAssttAnesthetist");
                HiddenField hdnPerfusionist = (HiddenField)item.FindControl("hdnPerfusionist");
                HiddenField hdnScrubNurse = (HiddenField)item.FindControl("hdnScrubNurse");
                HiddenField hdnFloorNurse = (HiddenField)item.FindControl("hdnFloorNurse");
                HiddenField hdnTechnician = (HiddenField)item.FindControl("hdnTechnician");
                HiddenField hdnAnesthesiaId = (HiddenField)item.FindControl("hdnAnesthesiaId");
                HiddenField hdnRemarks = (HiddenField)item.FindControl("hdnRemarks");
                HiddenField hdnIsMain = (HiddenField)item.FindControl("hdnIsMain");
                HiddenField hdnOTEquipMentsId = (HiddenField)item.FindControl("hdnOTEquipMentsId");
                // HiddenField hdnIsInfectiousCase = (HiddenField)item.FindControl("hdnIsInfectiousCase");
                coll.Add(common.myInt(hdnOTServiceDetailId.Value));
                coll.Add(common.myInt(hdnServiceID.Value));
                coll.Add(common.myInt(hdnIsMain.Value));
                coll.Add(common.myStr(hdnOTEquipMentsId.Value));
                strXMLItems.Append(common.setXmlTable(ref coll));
                colResource.Add(0); // Id - Zero for New Entry
                colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                colResource.Add(common.myInt(hdnSurgeon.Value.Split(',').LastOrDefault().Trim())); // Resource Id
                colResource.Add(common.myStr("SR")); // Resource Type Surgeon
                sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                //    }
                //}
                //string[] strAsstSurgeonIds = hdnAsstSurgeon.Value.Split(',');
                //foreach (string Id in strAsstSurgeonIds)
                //{
                //    if (Id != "")
                //    {
                colResource.Add(0); // Id - Zero for New Entry
                colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                colResource.Add(common.myInt(hdnAsstSurgeon.Value.Split(',').LastOrDefault().Trim())); // Resource Id
                colResource.Add(common.myStr("AS")); // Resource Type Asst Surgeon
                sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                //    }
                //}
                //string[] strAnesthetistIds = hdnAnesthetist.Value.Split(',');
                //foreach (string Id in strAnesthetistIds)
                //{
                //    if (Id != "")
                //    {
                colResource.Add(0); // Id - Zero for New Entry
                colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                colResource.Add(common.myInt(hdnAnesthetist.Value.Split(',').LastOrDefault().Trim())); // Resource Id
                colResource.Add(common.myStr("AN")); // Resource Type Anesthetist
                sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                colResource.Add(0); // Id - Zero for New Entry
                colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                colResource.Add(common.myInt(hdnAssttAnesthetist.Value.Split(',').LastOrDefault().Trim())); // Resource Id
                colResource.Add(common.myStr("AAN")); // Resource Type Asstt Anesthetist
                sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                if (common.myStr(hdnPerfusionist.Value) != "")
                {
                    string[] strPerfusionistIds = hdnPerfusionist.Value.Split(',');
                    foreach (string Id in strPerfusionistIds)
                    {
                        if (Id != "")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                            colResource.Add(common.myInt(Id)); // Resource Id
                            colResource.Add(common.myStr("PR")); // Resource Type Perfusionist
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }
                    }
                }
                if (common.myStr(hdnScrubNurse.Value) != "")
                {
                    string[] strScrubNurseIds = hdnScrubNurse.Value.Split(',');
                    foreach (string Id in strScrubNurseIds)
                    {
                        if (Id != "")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                            colResource.Add(common.myInt(Id)); // Resource Id
                            colResource.Add(common.myStr("SN")); // Resource Type Scrub Nurse
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }
                    }
                }
                if (common.myStr(hdnFloorNurse.Value) != "")
                {
                    string[] strFloorNurseIds = hdnFloorNurse.Value.Split(',');
                    foreach (string Id in strFloorNurseIds)
                    {
                        if (Id != "")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                            colResource.Add(common.myInt(Id)); // Resource Id
                            colResource.Add(common.myStr("FN")); // Resource Type Floor Nurse
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }
                    }
                }
                if (common.myStr(hdnTechnician.Value) != "")
                {
                    string[] strTechnicianIds = hdnTechnician.Value.Split(',');
                    foreach (string Id in strTechnicianIds)
                    {
                        if (Id != "")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value.Split(',').LastOrDefault().Trim())); // Service Id
                            colResource.Add(common.myInt(Id)); // Resource Id
                            colResource.Add(common.myStr("TC")); // Resource Type Technician
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }
                    }
                }
            }
            string Message = objprescription.SaveEMROTRequest(common.myInt(hdnOTBookingID.Value), IsEmergency, common.myInt(Session["EncounterId"]),
                            common.myInt(Session["RegistrationId"]), txtFName.Text, txtMName.Text, txtLName.Text, common.myDate(dtpDOB.SelectedDate),
                            common.myInt(txtAgeYears.Text), common.myInt(txtAgeMonths.Text), common.myInt(txtAgeDays.Text),
                            common.myInt(ddlGender.SelectedValue), txtMobileNo.Text, common.myInt(txtDuration.Text), ddlHours.SelectedValue,
                            common.myDate(dtpOTDate.SelectedDate), common.myDate(RadTimeFrom.SelectedDate), common.myStr(txtRemarks.Text), true,
                            common.myInt(Session["UserId"]), DateTime.Now, common.myInt(Session["UserId"]), DateTime.Now, common.myStr(strXMLItems),
                            common.myStr(sXmlResourceDetails), txtProvisionalDiagnosis.Text, common.myInt(ddlAnaesthesia.SelectedValue),
                            common.myBool(rdoInfectiousCase.SelectedValue), common.myStr(txtInfectiousCaseRemarks.Text), common.myInt(ddlOTTheater.SelectedValue), common.myBool(rblIsPACRequired.SelectedValue));
            if (common.myStr(Message).Contains("Saved"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Saved";
                GetgvbindemrotRequest(common.myInt(Session["RegistrationNo"]));
                txtDuration.Text = null;
                txtProvisionalDiagnosis.Text = null;
                dtpOTDate.SelectedDate = null;
                RadTimeFrom.SelectedDate = null;
                txtRemarks.Text = null;
            }
            if (common.myStr(Message).Contains("Update"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Update";
                GetgvbindemrotRequest(common.myInt(Session["RegistrationNo"]));
                txtDuration.Text = null;
                txtProvisionalDiagnosis.Text = null;
                dtpOTDate.SelectedDate = null;
                RadTimeFrom.SelectedDate = null;
                txtRemarks.Text = null;
            }
            txtInfectiousCaseRemarks.Text = "";
            bindOTBook();
        }
        catch (Exception ex)
        {
        }
    }
    private void bindOTBook()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("OTServiceDetailId");
        dt.Columns.Add("SR");
        dt.Columns.Add("AN");
        dt.Columns.Add("AAN");
        dt.Columns.Add("AnesthesiaId");
        dt.Columns.Add("ServiceID");
        dt.Columns.Add("SN");
        dt.Columns.Add("FN");
        dt.Columns.Add("PR");
        dt.Columns.Add("TC");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("SubDepartmentId");
        dt.Columns.Add("OTRequestID");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("OTBookingDateF");
        dt.Columns.Add("FromTime");
        gvOTBook.DataSource = dt;
        gvOTBook.DataBind();
    }
    protected void CheckItems(string strIdValues, RadComboBox ddl)
    {
        string[] strIds = strIdValues.Split(',');
        if (strIds.Length > 0)
        {
            foreach (string Id in strIds)
            {
                foreach (RadComboBoxItem chk in ddl.Items)
                {
                    if (chk.Value == Id)
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
    }
    private bool ValidateData()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        // string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);
        if (ddlService.SelectedValue == "")
        {
            lblMessage.Text = "Select a service!";
            return false;
        }

        else if (lblStarDiagnosis.Visible == true && txtProvisionalDiagnosis.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select Diagnosis!";
            return false;
        }
        //else if (txtProvisionalDiagnosis.Text.Equals(string.Empty))
        //{
        //    lblMessage.Text = "Select Diagnosis!";
        //    return false;
        //}
        else if (dtpOTDate.SelectedDate == null)
        {
            lblMessage.Text = "Select OT Date!";
            return false;
        }
        else if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.Text = "Select time!";
            return false;
        }
        //else if (common.myInt(ddlSurgeon.SelectedValue) == 0)
        //{
        //    lblMessage.Text = "Select a surgeon!";
        //    return false;
        //}
        //else if (ddlAnesthetist.SelectedValue == null)
        //{ 
        //    lblMessage.Text = "Select an anesthetist!";
        //    return false;
        //}
        else if (txtDuration.Text == "")
        {
            lblMessage.Text = "Select Duration!";
            return false;
        }
        else if (dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
        {
            DateTime d = RadTimeFrom.SelectedDate.Value;
            DateTime time = common.myDate(d.ToString("HH:mm:ss"));
            if (time < common.myDate(DateTime.Now.ToString("HH:mm:ss")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Valid Time";
                return false;
            }
            else
            {
                lblMessage.Text = String.Empty;
                return true;
            }
        }
        //Mahendra jaiswal
        //else if (strEquipmentIds == "")
        //{
        //    if (chkEquipmentList.Checked == false)
        //    {
        //        if (txtOTEquipment.Text == "")
        //        {
        //            lblMessage.Text = "Type OT Equipment!";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        // lblMessage.Text = "Select an OT Equipment!";
        //        return true;
        //    }
        //}
        else
            return true;
    }
    private bool Validatetext()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (txtDuration.Text == "")
        {
            lblMessage.Text = "Select Duration!";
            return false;
        }
        else if (dtpDOB.SelectedDate == null)
        {
            lblMessage.Text = "Please Select DOB";
            return false;
        }
        else if (txtFName.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select First Name";
            return false;
        }
        else if (txtFName.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select First Name";
            return false;
        }
        else if (txtMobileNo.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select fill Mobile No.";
            return false;
        }
        else if (lblStarDiagnosis.Visible == true && txtProvisionalDiagnosis.Text.Equals(string.Empty))
        {
            lblMessage.Text = "Select Diagnosis!";
            return false;
        }
        //else if (txtProvisionalDiagnosis.Text.Equals(string.Empty))
        //{
        //    lblMessage.Text = "Select Diagnosis!";
        //    return false;
        //}
        else if (dtpOTDate.SelectedDate == null)
        {
            lblMessage.Text = "Select OT Date!";
            return false;
        }
        else if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select from time...";
            return false;
        }
        else
        {
            return true;
        }
    }
    protected void dtpDOB_OnSelectedDateChanged(object sender, EventArgs e)
    {
        imgCalYear_Click(this, null);
    }
    protected void imgCalYear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (dtpDOB.SelectedDate != null)
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                txtAgeYears.Text = "";
                txtAgeMonths.Text = "";
                txtAgeDays.Text = "";
                DateTime datet = dtpDOB.SelectedDate.Value;
                // DateTime dateReg = dtpRegDate.SelectedDate.Value;
                // bC.DOB = datet.ToString("dd/MM/yyyy");
                string[] result = bC.CalculateAge(datet.ToString("yyyy/MM/dd"));
                if (result.Length == 2)
                {
                    if (result[1] == "Yr")
                    {
                        txtAgeYears.Text = result[0];
                    }
                    else if (result[1] == "Mnth")
                    {
                        txtAgeMonths.Text = result[0];
                    }
                    else
                    {
                        txtAgeDays.Text = result[0];
                    }
                }
                if (result.Length == 4)
                {
                    //txtAgeYears.Text = result[0];
                    txtAgeMonths.Text = result[0];
                    txtAgeDays.Text = result[2];
                }
                if (result.Length == 6)
                {
                    txtAgeYears.Text = result[0];
                    txtAgeMonths.Text = result[2];
                    txtAgeDays.Text = result[4];
                }
                if (txtAgeYears.Text == "")
                {
                    txtAgeYears.Text = "0";
                }
                if (txtAgeMonths.Text == "")
                {
                    txtAgeMonths.Text = "0";
                }
                if (txtAgeDays.Text == "")
                {
                    txtAgeDays.Text = "0";
                }
            }
            ddlGender.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void btnCalAge_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.ParseData bc = new BaseC.ParseData();
            Hashtable hshIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            String SqlStr = "select dbo.DOBFromAge(" + val(bc.ParseQ(txtAgeYears.Text)) + "," + val(bc.ParseQ(txtAgeMonths.Text)) + "," + val(bc.ParseQ(txtAgeDays.Text)) + ")";
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, SqlStr, hshIn);
            dr.Read();
            dtpDOB.SelectedDate = common.myDate(dr[0].ToString());
            //txtLAddress1.Focus();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    Double val(String value)
    {
        Double intData = 0;
        Boolean blnTemp = Double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat, out intData);
        return intData;
    }
    protected void txtYear_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        if (txtAgeYears.Text != "")
        {
            txtAgeMonths.Focus();
        }
        else
        {
            dtpDOB.SelectedDate = null;
            txtAgeYears.Text = "";
            txtAgeYears.Focus();
        }
    }
    protected void txtMonth_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        txtAgeDays.Focus();
    }
    protected void txtDays_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        //txtLAddress1.Focus();
    }
    void DOBCalc()
    {
        try
        {
            int year = common.myInt(txtAgeYears.Text);
            int month = common.myInt(txtAgeMonths.Text);
            int day = common.myInt(txtAgeDays.Text);
            DateTime DOB = DateTime.Now.AddDays(-day);
            DOB = DOB.AddMonths(-month);
            DOB = DOB.AddYears(-year);
            if (dtpDOB.MinDate < common.myDate(DOB))
            { dtpDOB.SelectedDate = common.myDate(DOB); }
            else
            {
                txtAgeYears.Text = "";
                //DOBCalc();
                txtAgeYears.Focus();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    //protected void BindPatientProvisionalDiagnosis()
    //{
    //    DataSet ds = new DataSet();
    //    BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
    //    try
    //    {
    //        int RegId = common.myInt(hdnRegistrationId.Value) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(hdnRegistrationId.Value);
    //        int EncId = common.myInt(hdnEncounterId.Value) == 0 ? common.myInt(Session["EncounterId"]) : common.myInt(hdnEncounterId.Value);
    //        ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), RegId, EncId, common.myInt(Session["UserID"]));
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            gvProvisionalDiagnosis.DataSource = ds.Tables[0];
    //            gvProvisionalDiagnosis.DataBind();
    //        }
    //        else
    //        {
    //            DataRow rd = ds.Tables[0].NewRow();
    //            ds.Tables[0].Rows.Add(rd);
    //            gvProvisionalDiagnosis.DataSource = ds.Tables[0];
    //            gvProvisionalDiagnosis.DataBind();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //        objDiag = null;
    //    }
    //}
    protected void btnAddDiagnosisSerchOnClientClose_OnClick(object sender, EventArgs e)
    {
    }
    protected void ibtnDiagnosisSearchCode_Click(object sender, ImageClickEventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Assessment/Status.aspx?Source=pd";
        RadWindow1.Height = 450;
        RadWindow1.Width = 550;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "addDiagnosisSerchOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void rdoImpReq_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtImpReqRem.Visible = false;
            lbltxtImpReqRem.Visible = false;
            lblRemarks.Visible = false;
            if (rdoImpReq.SelectedValue == "Y")
            {
                txtImpReqRem.Visible = true;
                lbltxtImpReqRem.Visible = true;
                lblRemarks.Visible = true;
            }
            updrdoImplant.Update();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void rdoInfectiousCase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Label16.Visible = false;
            txtInfectiousCaseRemarks.Visible = false;
            if (common.myBool(rdoInfectiousCase.SelectedValue) == true)
            {
                Label16.Visible = true;
                txtInfectiousCaseRemarks.Visible = true;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void rblUnplanned_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if ((rblUnplanned.SelectedIndex) == 1)
        //{
        //}
    }
    protected void chkisBloodRequired_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkisBloodRequired.Checked == true)
        //{
        //    ddlBloodUnit.Enabled = true;
        //    lblNoOfBloodUnit.Enabled = true;
        //}
        //else
        //{
        //    ddlBloodUnit.Enabled = false;
        //    lblNoOfBloodUnit.Enabled = false;
        //    ddlBloodUnit.SelectedIndex = 0;
        //}
    }
    public void populateBloodUnit()
    {
        //ddlBloodUnit.Items.Insert(0, new ListItem("Select"));
        //for (int i = 1; i <= 20; i++)
        //{
        //    ddlBloodUnit.Items.Insert(i, new ListItem((i).ToString()));
        //}
    }
    #region MandatoryFieldSet
    private bool IsEnable(DataView dv, string FieldName)
    {
        bool iChk = true;
        try
        {
            if (!FieldName.Equals(""))
            {
                dv.RowFilter = "FieldName = '" + FieldName + "'";
                if (dv.Count > 0)
                {
                    iChk = common.myBool(dv.ToTable().Rows[0]["IsEnableField"]);
                }
                dv.RowFilter = "";
            }
            return iChk;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            return false;
        }
    }
    private void ShowHideMandatory()
    {
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        DataSet dsPage = hsSetup.GetPageMandatoryAndEnableFields(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "Operation Theatre", "OT Request");
        if (dsPage.Tables.Count > 0)
        {
            if (dsPage.Tables[0].Rows.Count > 0)
            {
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblAnesthetiststar, "Anesthetist");
                if (lblAnesthetiststar.Visible) { lblAnesthetiststar.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffce"); }
                ddlAnesthetist.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "Anesthetist");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblAnaesthesiastar, "Anaesthesia");
                if (lblAnaesthesiastar.Visible) { lblAnaesthesiastar.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffce"); }
                ddlAnaesthesia.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "Anaesthesia");
            }
        }
    }
    private void IsMandatory(DataView dv, Label LBL, string FieldName)
    {
        try
        {
            LBL.Visible = false;
            dv.RowFilter = "FieldName = '" + FieldName + "'";
            if (dv.Count > 0)
            {
                LBL.Visible = common.myBool(dv.ToTable().Rows[0]["IsMandatoryField"]);
            }
            if (!LBL.Visible)
            {
                IsEnable(dv, common.myStr(FieldName));
            }
            dv.RowFilter = "";
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion
    public void GetgvbindemrotRequest(int RegistrationNo)
    {
        OpPrescription objprescription = new OpPrescription(sConString);
        DataSet ds = new DataSet();
        ds = objprescription.GetEMROTRequest(RegistrationNo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvbindEMROTRequest.DataSource = ds.Tables[0];
            gvbindEMROTRequest.DataBind();
        }
        else
        {
            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);
            gvbindEMROTRequest.DataSource = ds.Tables[0];
            gvbindEMROTRequest.DataBind();
        }
    }
    protected void gvbindEMROTRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearControls();
        lblMessage.Text = null;
        HiddenField hdnRegistrationId = (HiddenField)gvbindEMROTRequest.Rows[gvbindEMROTRequest.SelectedIndex].FindControl("hdnRegistrationId");
        HiddenField hdnEncounterId = (HiddenField)gvbindEMROTRequest.Rows[gvbindEMROTRequest.SelectedIndex].FindControl("hdnEncounterId");
        HiddenField hdnhdnOTRequestID = (HiddenField)gvbindEMROTRequest.Rows[gvbindEMROTRequest.SelectedIndex].FindControl("hdnOTRequestID");
        DataSet ds = new DataSet();
        OpPrescription objOp = new OpPrescription(sConString);
        try
        {
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            //  ds = objOTBooking.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            //  ds = objwcfOt.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            ds = objOp.getOTBookingDetails(common.myInt(hdnhdnOTRequestID.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                dr = ds.Tables[0].Rows[0];
                hdnOTBookingID.Value = common.myStr(dr["OTRequestID"]);
                //   hdnOTBookingNO.Value = common.myStr(dr["OTBookingNo"]);
                dtpOTDate.SelectedDate = Convert.ToDateTime(dr["OTBookingDate"]);
                if (common.myStr(dr["RegistrationId"]) != "")
                {
                    //ddlSearchOn.SelectedValue = "0";
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    //txtPatientNo.Text = common.myStr(dr["RegistrationNo"]);
                    //txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
                    hdnEncounterId.Value = common.myStr(dr["EncounterID"]);
                }
                txtFName.Text = common.myStr(dr["FirstName"]);
                txtMName.Text = common.myStr(dr["MiddleName"]);
                txtLName.Text = common.myStr(dr["LastName"]);
                dtpDOB.SelectedDate = Convert.ToDateTime(dr["DateOfBirth"]);
                txtMobileNo.Text = common.myStr(dr["MobileNo"]);
                txtAgeYears.Text = common.myStr(dr["AgeYears"]);
                txtAgeMonths.Text = common.myStr(dr["AgeMonths"]);
                txtAgeDays.Text = common.myStr(dr["AgeDays"]);
                ddlGender.SelectedValue = common.myStr(dr["GenderId"]);

                ddlOTTheater.SelectedIndex = ddlOTTheater.Items.IndexOf(ddlOTTheater.Items.FindItemByValue(common.myInt(dr["TheatreId"]).ToString()));

                txtDuration.Text = common.myStr(dr["OTDuration"]);
                ddlHours.SelectedValue = common.myStr(dr["OTDurationType"]);
                ddlAnaesthesia.SelectedValue = common.myStr(dr["AnesthesiaId"]);
                //lblIPNo.Text = common.myStr(dr["EncounterNo"]);
                //lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                //if (common.myBool(dr["isBloodRequired"]) == true)
                //{
                //    chkisBloodRequired.Checked = true;
                //}
                //else
                //{
                //    chkisBloodRequired.Checked = false;
                //}
                //if (common.myBool(dr["IsICURequired"]) == true)
                //{
                //    chkICUrequired.Checked = true;
                //}
                //else
                //{
                //    chkICUrequired.Checked = false;
                //}
                //ddlBloodUnit.SelectedValue = common.myStr(dr["Bloodunit"]);


                if (common.myBool(dr["PACRequired"]) == true)
                    rblIsPACRequired.SelectedValue = "1";
                else
                    rblIsPACRequired.SelectedValue = "0";

                RadTimeFrom.SelectedDate = common.myDate(common.myStr(dr["FromTime"]));
                txtRemarks.Text = common.myStr(dr["Remarks"]);
                txtProvisionalDiagnosis.Text = common.myStr(dr["Diagnosis"]);
                //  ddlDiagnosisSearchCodes.SelectedValue = common.myStr(dr["DiagnosisSearchId"]);
                hdnOTBookingDateTime.Value = common.myStr(RadTimeFrom.SelectedDate);
                //  rdoImpReq.SelectedIndex = rdoImpReq.Items.IndexOf(rdoImpReq.Items.FindByValue(common.myStr(dr["isImplantRequired"])));
                rdoImpReq_SelectedIndexChanged(null, null);
                //txtImpReqRem.Text = common.myStr(dr["ImplantRequiredRemark"]);
                BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                int x = 0;
                Hashtable ht = new Hashtable();
                ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), common.myDate(hdnOTBookingDateTime.Value));
                ViewState["OTBook"] = ds.Tables[0];
                gvOTBook.DataSource = ds.Tables[0];
                gvOTBook.DataBind();
                // BindPatientProvisionalDiagnosis();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void lnkViewDetails_Click1(object sender, EventArgs e)
    {
    }
    protected void ibtnewDelete_Click(object sender, ImageClickEventArgs e)
    {
        //ImageButton ibtnewDelete = (ImageButton)gvbindEMROTRequest.Rows[gvbindEMROTRequest.SelectedIndex].FindControl("ibtnewDelete");
        //if(ibtnewDelete.CommandName.ToUpper().Equals("ITEMDELETE"))
        //{
        //    divAllergy.Visible = true;
        //    int RequestID = common.myInt(ibtnewDelete.CommandArgument);
        //}
    }
    protected void gvbindEMROTRequest_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)gvbindEMROTRequest.Rows[e.RowIndex];
            ImageButton ibtnewDelete = (ImageButton)row.FindControl("ibtnewDelete");
            HiddenField hdnhdnOTRequestID = (HiddenField)row.FindControl("hdnOTRequestID");
            if (ibtnewDelete.CommandName.ToUpper().Equals("DELETE"))
            {
                OpPrescription objprescription = new OpPrescription(sConString);
                string Message = objprescription.DeleteEMROTRequest(common.myInt(hdnhdnOTRequestID.Value), common.myInt(Session["UserId"]));
                if (common.myStr(Message).Contains("deleted"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Record deleted succesfully";
                    GetgvbindemrotRequest(common.myInt(Session["RegistrationNo"]));
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void gvbindEMROTRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblBookDate = (Label)e.Row.FindControl("lblBookDate");
                ImageButton ibtnewDelete = (ImageButton)e.Row.FindControl("ibtnewDelete");
                if (!common.myStr(lblBookDate.Text).Equals(" "))
                {
                    if (!common.myStr(lblBookDate.Text).Equals(string.Empty))
                    {
                        ibtnewDelete.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void gvOTBook_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)gvOTBook.Rows[e.RowIndex];
            HiddenField hdnOTServiceDetailId = (HiddenField)row.FindControl("hdnOTServiceDetailId");
            ImageButton ibtnDelete = (ImageButton)row.FindControl("ibtnDelete");
            if (ibtnDelete.CommandName.ToUpper().Equals("DELETE"))
            {
                DataTable ds = new DataTable();
                ds = (DataTable)ViewState["OTBook"];
                ds.DefaultView.RowFilter = "OTServiceDetailId <> " + hdnOTServiceDetailId.Value + "";
                gvOTBook.DataSource = ds;
                gvOTBook.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    private void PopulateOTEquipment()
    {
        DataSet dt = new DataSet();
        int selectedServiceId = 0;
        int runningServiceId = 0;
        try
        {
            selectedServiceId = common.myInt(ddlService.SelectedValue);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityID", common.myInt(Session["FacilityId"]));
            HashIn.Add("@intTheaterId", common.myInt(0));
            HashIn.Add("@intOTBookingId", common.myInt(0));
            HashIn.Add("@dtOTBookingDate", common.myStr(common.myDate(DateTime.Now)));
            HashIn.Add("@dtFromTime", DateTime.Now.ToString("hh:mm tt"));
            HashIn.Add("@dtToTime", DateTime.Now.ToString("hh:mm tt"));
            HashIn.Add("@intServiceId", selectedServiceId);
            dt = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTEquipmentForBooking", HashIn);
            ddlOTEquipments.Items.Clear();
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dt.Tables[0].Rows[i]["EquipmentName"]);
                item.Value = dt.Tables[0].Rows[i]["EquipmentId"].ToString();
                item.Attributes.Add("EquipmentName", dt.Tables[0].Rows[i]["EquipmentName"].ToString());
                item.Attributes.Add("EquipmentId", dt.Tables[0].Rows[i]["EquipmentId"].ToString());
                item.Attributes.Add("TotalQty", dt.Tables[0].Rows[i]["TotalQty"].ToString());
                item.Attributes.Add("AvailableQty", dt.Tables[0].Rows[i]["AvailableQty"].ToString());
                this.ddlOTEquipments.Items.Add(item);
                item.DataBind();
                item.Checked = false;
                if (common.myInt(dt.Tables[0].Rows[i]["AvailableQty"]) > 0)
                {
                    item.Enabled = true;
                    try
                    {
                        runningServiceId = common.myInt(dt.Tables[0].Rows[i]["ServiceId"]);
                        if (runningServiceId > 0)
                        {
                            item.Checked = true;
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    item.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
        finally
        {
            dt.Dispose();
        }
    }
    protected void ddlService_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (common.myInt(ddlService.SelectedValue) > 0)
        {
            PopulateOTEquipment();
        }
    }
}