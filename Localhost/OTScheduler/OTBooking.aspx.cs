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

public partial class OT_Scheduler_OTBooking : System.Web.UI.Page
{
    protected UserControl uc1;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData pdDataCheck = new BaseC.ParseData();
    BaseC.EMRMasters bMstr;
    BaseC.EMROrders objEMROrders;
    BaseC.clsLabRequest objLabRequest;
    BaseC.RestFulAPI objwcfOt;//= new wcf_Service_OT.ServiceClient();
    BaseC.RestFulAPI objwcfcm;//= new wcf_Service_Common.CommonMasterClient();
    private const int ItemsPerRequest = 50;
    StringBuilder strOtBookingDateTime = new StringBuilder();
    private int RegId, EncId; // added by Sushil Saini
    private string RegNo, EncNo; // added by Sushil Saini
    private int OTRequestID;
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
                objwcfOt = new BaseC.RestFulAPI(sConString);
                objwcfcm = new BaseC.RestFulAPI(sConString);
                dtpOTDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpDOB.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtLMPDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtLMPDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
                //dtLMPDate.SelectedDate = common.myDate(DateTime.Now);
                dtLMPDate.MaxDate = common.myDate(DateTime.Now);
                ddlOTName.Enabled = false;
                //dtpOTDate.Enabled = false;
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
                BindDepartment();
                BindAnaesthesia();
                PopulateOTName();
                BindDiagnosisSearchCode();
                populateBloodUnit();
                Session["cSvCheck"] = "0";

                RadTimeFrom.TimeView.Interval = new TimeSpan(0, common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]), 0);
                RadTimeTo.TimeView.Interval = new TimeSpan(0, common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]), 0);
                if (Request.QueryString["StTime"] != null)
                {
                    RadTimeFrom.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["StTime"].ToString()), 0, 0);
                    RadTimeTo.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["StTime"].ToString()), 0, 0);
                    RadTimeFrom.TimeView.EndTime = new TimeSpan(int.Parse(Request.QueryString["EndTime"].ToString()), 0, 0);
                    RadTimeTo.TimeView.EndTime = new TimeSpan(int.Parse(Request.QueryString["EndTime"].ToString()), common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]), 0);

                    if (Request.QueryString["OTID"].ToString() != "")
                    {
                        ddlOTName.SelectedValue = Request.QueryString["OTID"].ToString();
                    }
                    if (Session["EncounterId"] != null && Session["RegistrationId"] != null && common.myStr(Session["ModuleName"]) == "EMR")
                    {
                        DateTime dt = new DateTime();
                        dt = common.myDate(Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]).AddMinutes(common.myInt(ViewState["TimeViewIntervalForOTInMinutes"])));
                        hdnOTBookingDateTime.Value = dt.ToString("yyyy/MM/dd HH:mm:ss");


                        ddlSearchOn.SelectedValue = "0";
                        txtPatientNo.Text = common.myInt(Session["RegistrationNo"]).ToString();
                        txtPatientNo.Enabled = false;
                        btnSearchByPatientNo_OnClick(this, null);
                        ddlSurgeon.SelectedValue = common.myInt(Session["DoctorId"]).ToString();
                    }
                    updrdoImplant.Visible = false;
                    updrdoEquipment.Visible = false;
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "SurgeryComponentWise", sConString).Equals("Y"))
                    {
                        //palendra change OT AssttAnesthetist enable 
                        //ddlAsstSurgeon1.Enabled = false;
                        ddlAssttAnesthetist.Enabled = false;
                        ddlAnaesthesia.Enabled = false;
                    }
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowImplantRequiredInOTBooking", sConString).Equals("Y"))
                    {
                        updrdoImplant.Visible = true;
                    }
                    //Added By Manoj Puri
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowEquipmentRequiredInOTBooking", sConString).Equals("Y"))
                    {
                        updrdoEquipment.Visible = true;
                    }

                    if (Convert.ToInt64(Request.QueryString["appid"]) > 0)
                    {
                        BindOTBookingDetails(common.myInt(Request.QueryString["appid"]));

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
                        RadTimeFrom.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]);
                        RadTimeTo.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]).AddMinutes(common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]));
                        //hdnOTBookingDateTime.Value =common.myStr(RadTimeFrom.SelectedDate);

                        RadTimeTo.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["FromTimeHour"]), int.Parse(RadTimeFrom.SelectedDate.Value.ToString("mm")) + common.myInt(ViewState["TimeViewIntervalForOTInMinutes"]), 0);
                        dtpOTDate.SelectedDate = Convert.ToDateTime(Request.QueryString["OTDate"]);//
                        DateTime dt = new DateTime();
                        dt = common.myDate(RadTimeTo.SelectedDate);
                        hdnOTBookingDateTime.Value = dt.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                }
                PopulateOTEquipment();
                BindProviderList();
                txtPatientNo.Focus();
                //PostBackOptions optionsSubmit = new PostBackOptions(ibtnSave);
                //ibtnSave.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";
                //ibtnSave.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);

                ibtnSave.Visible = true;
                if (common.myStr(Session["ModuleName"]) == "EMR" && common.myStr(Session["OPIP"]) == "O")
                {
                    ibtnOpenSearchPatientPopup.Enabled = false;
                    txtPatientNo.Enabled = false;
                }

                //BaseC.Security baseUs = new BaseC.Security(sConString);
                //bool IsAuthorizeOTCheckIn = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckIn");
                //bool IsAuthorizeOTCheckOut = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForOTCheckOut");

                //RadTimeFrom.Enabled = IsAuthorizeOTCheckIn;
                //RadTimeTo.Enabled = IsAuthorizeOTCheckOut;

                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "AllowOnlyEquipmentListOnOTBooking", sConString).Equals("Y"))
                {
                    chkEquipmentList.Checked = true;
                    chkEquipmentList_OnCheckedChanged(null, null);
                    chkEquipmentList.Enabled = false;
                }
                else
                {
                    chkEquipmentList.Checked = false;
                    chkEquipmentList_OnCheckedChanged(null, null);
                    chkEquipmentList.Enabled = true;
                }

                ShowHideMandatory();

                BaseC.Security baseUs = new BaseC.Security(sConString);
                bool IsAuthForEditOTNurseTechnician = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForEditOTScrubNurseTechnician");

                if (IsAuthForEditOTNurseTechnician && common.myInt(ViewState["OTOrderId"]) > 0)
                {
                    DisableControls(Page.Controls);

                    ddlPerfusionist.Enabled = true;
                    ddlScrubNurse.Enabled = true;
                    ddlFloorNurse.Enabled = true;
                    ddlOTTechnician.Enabled = true;
                }

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        "IsOTEmergencyValidation", sConString);

                if (collHospitalSetupValues.ContainsKey("IsOTEmergencyValidation"))
                    ViewState["IsOTEmergencyValidation"] = common.myStr(collHospitalSetupValues["IsOTEmergencyValidation"]);


            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = ex.Message.ToString();
                objException.HandleException(ex);
            }
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

    protected void BindDiagnosisSearchCode()
    {
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "");
            ddlDiagnosisSearchCodes.Items.Clear();
            ddlDiagnosisSearchCodes.DataSource = ds;
            ddlDiagnosisSearchCodes.DataValueField = "Id";
            ddlDiagnosisSearchCodes.DataTextField = "DiagnosisSearchCode";
            ddlDiagnosisSearchCodes.DataBind();
            ddlDiagnosisSearchCodes.Items.Insert(0, new RadComboBoxItem("Select", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDiag = null;
            ds.Dispose();
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
    private void PopulateOTName()
    {
        DataSet ds = new DataSet();

        try
        {
            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlOTName.DataSource = ds.Tables[0];
                ddlOTName.DataTextField = "TheatreName";
                ddlOTName.DataValueField = "TheatreID";
                ddlOTName.DataBind();
                ddlOTName.Items.Insert(0, new RadComboBoxItem("", "0"));
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
            HashIn.Add("@intTheaterId", common.myInt(ddlOTName.SelectedValue));
            HashIn.Add("@intOTBookingId", common.myInt(hdnOTBookingID.Value));
            HashIn.Add("@dtOTBookingDate", common.myStr(common.myDate(dtpOTDate.SelectedDate)));
            HashIn.Add("@dtFromTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mm tt"));
            HashIn.Add("@dtToTime", RadTimeTo.SelectedDate.Value.ToString("hh:mm tt"));
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
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }
    private void BindOTBookingDetails(int OTBookingId)
    {
        // BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        try
        {
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            string OTBookingNo = "";

            //  ds = objOTBooking.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            ds = objwcfOt.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                dr = ds.Tables[0].Rows[0];
                hdnOTBookingID.Value = common.myStr(dr["OTBookingID"]);
                hdnOTBookingNO.Value = common.myStr(dr["OTBookingNo"]);
                dtpOTDate.SelectedDate = Convert.ToDateTime(dr["OTBookingDate"]);
                if (common.myStr(dr["RegistrationNo"]) != "")
                {
                    ddlSearchOn.SelectedValue = "0";
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    txtPatientNo.Text = common.myStr(dr["RegistrationNo"]);
                    txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
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

                lblIPNo.Text = common.myStr(dr["EncounterNo"]);
                lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);

                ViewState["OTOrderId"] = common.myStr(dr["OrderID"]);

                if (common.myBool(dr["isBloodRequired"]) == true)
                {
                    chkisBloodRequired.Checked = true;
                }
                else
                {
                    chkisBloodRequired.Checked = false;

                }

                if (common.myBool(dr["IsICURequired"]) == true)
                {
                    chkICUrequired.Checked = true;
                }
                else
                {
                    chkICUrequired.Checked = false;
                }

                ddlBloodUnit.SelectedValue = common.myStr(dr["Bloodunit"]);
                ddlOTName.SelectedValue = common.myStr(dr["TheaterId"]);
                RadTimeFrom.SelectedDate = common.myDate(common.myStr(dr["FromTime"]));
                RadTimeTo.SelectedDate = common.myDate(common.myStr(dr["ToTime"]));
                txtRemarks.Text = common.myStr(dr["Remarks"]);
                txtProvisionalDiagnosis.Text = common.myStr(dr["ProvisionalDiagnosis"]);
                ddlDiagnosisSearchCodes.SelectedValue = common.myStr(dr["DiagnosisSearchId"]);

                hdnOTBookingDateTime.Value = common.myStr(RadTimeFrom.SelectedDate);

                rdoImpReq.SelectedIndex = rdoImpReq.Items.IndexOf(rdoImpReq.Items.FindByValue(common.myStr(dr["isImplantRequired"])));
                rdoImpReq_SelectedIndexChanged(null, null);
                txtImpReqRem.Text = common.myStr(dr["ImplantRequiredRemark"]);
                txtEquiReqRem.Text = common.myStr(dr["EquipmentRequiredRemark"]);
                if (common.myInt(dr["GenderId"]) == 1)
                {
                    tdLMPDate.Visible = true;
                    dtLMPDate.SelectedDate = Convert.ToDateTime(dr["LMPDatetime"]);
                }
                chkVentilatorRequired.Checked = common.myBool(dr["IsVentilatorRequired"]);

                if (common.myBool(dr["isEmergency"]))
                    rblIsEmergency.SelectedIndex = 1;
                else
                    rblIsEmergency.SelectedIndex = 0;



                BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                int x = 0;
                Hashtable ht = new Hashtable();


                //ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId), common.myInt(Session["FacilityId"]), common.myDate(dtpOTDate.SelectedDate));
                ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingDateTime.Value));
                if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]) || common.myStr(ht["@chvLastCheckoutTime"]).Trim() == "")
                {
                    if (common.myBool(dr["IsUnplannedReturnToOT"]))
                    {
                        rblUnplanned.SelectedIndex = 0;
                        rblUnplanned.Enabled = true;
                    }
                    else if (!common.myBool(dr["IsUnplannedReturnToOT"]))
                    {
                        rblUnplanned.SelectedIndex = 1;
                    }
                    else
                    {
                        rblUnplanned.SelectedIndex = 1;
                        rblUnplanned.Enabled = true;
                    }


                }

                // DateTime dt = new DateTime();
                // dt = common.myDate(dr["OTCheckouttime"]);
                //lblChkoutTime.Text = common.myStr(dt);
                //lblChkoutTime.Text =Convert.ToDateTime(common.myStr(dr["OTCheckouttime"]));
                lblChkoutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);

                ViewState["OTBook"] = ds.Tables[0];
                gvOTBook.DataSource = ds.Tables[0];
                gvOTBook.DataBind();
                BindPatientProvisionalDiagnosis();
                bindAdvanceDetails();
                txtAdvance.Text = common.myStr(ViewState["AdvanceAmt"]);
                hdnAdvance.Value = txtAdvance.Text;

                if (Convert.ToString(Session["BloodGroupId"]) == "")
                {
                    DataTable dtBllod = (DataTable)Session["BloodGroups"];
                    ddlBloodGroup.DataSource = dtBllod;
                    ddlBloodGroup.DataTextField = "BloodGroup";
                    ddlBloodGroup.DataValueField = "BloodGroupId";
                    ddlBloodGroup.DataBind();
                }
                else
                {
                    ddlBloodGroup.DataSource = (DataTable)Session["BloodGroups"];
                    ddlBloodGroup.DataTextField = "BloodGroup";
                    ddlBloodGroup.DataValueField = "BloodGroupId";
                    ddlBloodGroup.DataBind();
                    ddlBloodGroup.SelectedValue = Convert.ToString(Session["BloodGroupId"]);
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
    protected void chkEquipmentList_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkEquipmentList.Checked == true)
        {
            ddlOTEquipments.Visible = true;
            txtOTEquipment.Visible = false;
        }
        else
        {
            ddlOTEquipments.Visible = false;
            txtOTEquipment.Visible = true;
        }
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

            if (hdnEquipmentName.Value != "")
            {
                chkEquipmentList.Checked = false;
                chkEquipmentList_OnCheckedChanged(null, null);
                chkEquipmentList.Enabled = false;
            }
            else
            {
                chkEquipmentList.Checked = true;
                chkEquipmentList_OnCheckedChanged(null, null);
                chkEquipmentList.Enabled = false;
            }

            HiddenField hdnDepartment = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnDepartment");
            HiddenField hdnSubDepartment = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnSubDepartment");
            hdnSubDepartmentId.Value = hdnSubDepartment.Value;
            HiddenField hdnServiceID = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnServiceID");
            //HiddenField hdnRemarks = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnRemarks");
            HiddenField hdnIsMain = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnIsMain");
            HiddenField hdnIsPackage = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnIsPackage");
            HiddenField hdnTheatreId = (HiddenField)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("hdnTheatreId");

            Label lblSurgeryName = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblSurgeryName");
            Label lblDate = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblDate");
            Label lblStartTime = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblStartTime");
            Label lblEndTime = (Label)gvOTBook.Rows[gvOTBook.SelectedIndex].FindControl("lblEndTime");


            //CheckItems(hdnSurgeon.Value, ddlSurgeon);
            //CheckItems(hdnAsstSuregry.Value, ddlAsstSurgeon1);
            //CheckItems(hdnAnesthetist.Value, ddlAnesthetist);
            //ddlSurgeon.SelectedValue = common.myStr(hdnSurgeon.Value);
            CheckItems(hdnSurgeon.Value, ddlSurgeon);

            string[] AsstSurgeon = hdnAsstSurgeon.Value.Split(',');
            if (AsstSurgeon.Length > 1)
            {
                ddlAsstSurgeon1.SelectedValue = common.myStr(AsstSurgeon[0]).Trim();
                ddlAsstSurgeon2.SelectedValue = common.myStr(AsstSurgeon[1]).Trim();
            }
            else
            {
                ddlAsstSurgeon1.SelectedValue = common.myStr(AsstSurgeon[0]);
                ddlAsstSurgeon2.SelectedValue = "";
            }

            ddlAnesthetist.SelectedValue = common.myStr(hdnAnesthetist.Value);
            ddlAssttAnesthetist.SelectedValue = common.myStr(hdnAssttAnesthetist.Value);
            ddlAnaesthesia.SelectedValue = common.myStr(hdnAnesthesiaId.Value);

            CheckItems(hdnScrubNurse.Value, ddlScrubNurse);
            CheckItems(hdnFloorNurse.Value, ddlFloorNurse);
            CheckItems(hdnPerfusionist.Value, ddlPerfusionist);
            CheckItems(hdnTechnician.Value, ddlOTTechnician);

            PopulateOTEquipment();

            if (hdnEquipment.Value == "0" || hdnEquipment.Value == "")
            {
                txtOTEquipment.Text = hdnEquipmentName.Value;
                // txtOTEquipment.Text = "Testing";
            }
            else
            {
                CheckItems(hdnEquipment.Value, ddlOTEquipments);
                ViewState["EquipmentLists"] = hdnEquipment.Value;
            }

            ddlDepartment.SelectedValue = hdnDepartment.Value;

            ddlDepartment_OnSelectedIndexChanged(this, null);
            ddlSubDepartment.SelectedValue = hdnSubDepartment.Value;


            ddlService.SelectedValue = hdnServiceID.Value;
            ddlService.Text = lblSurgeryName.Text;
            bool IsMain = false;
            if (hdnIsMain.Value == "1" || common.myBool(hdnIsMain.Value))
                IsMain = true;
            cbMainSurgery.Checked = IsMain;

            rblServiceType.SelectedIndex = (hdnIsPackage != null && common.myBool(hdnIsPackage.Value)) ? 0 : 1;

            //cbMainSurgery.Enabled = IsMain;
            //txtRemarks.Text = common.myStr(hdnRemarks.Value);
            //BaseC.Patient formatdate = new BaseC.Patient(sConString);

            //ViewState["Date"] = formatdate.FormatDate(lblDate.Text, "dd/MM/yyyy", "MM/dd/yyyy");
            dtpOTDate.SelectedDate = common.myDate(lblDate.Text);
            RadTimeFrom.SelectedDate = Convert.ToDateTime(lblStartTime.Text);
            RadTimeTo.SelectedDate = Convert.ToDateTime(lblEndTime.Text);

            if (common.myInt(hdnTheatreId.Value) > 0)
            {
                ddlOTName.SelectedIndex = ddlOTName.Items.IndexOf(ddlOTName.Items.FindItemByValue(common.myInt(hdnTheatreId.Value).ToString()));
            }

            ViewState["UpdateSelectIndex"] = gvOTBook.SelectedIndex;
            btnAddToList.Text = "Update";
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

        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=1&SearchOn=" + common.myInt(ddlSearchOn.SelectedValue) + "";
        RadWindow1.Height = 550;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

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
                if (ddlSearchOn.SelectedValue == "1")
                {
                    RegistrationNo = common.myStr(hdnRegistrationNo.Value);
                    hdnEncounterNo.Value = common.myStr(txtPatientNo.Text);
                }
                else
                {
                    hdnEncounterNo.Value = "";
                    RegistrationNo = hdnRegistrationId.Value == "" ? common.myStr(hdnRegistrationNo.Value) : common.myStr(txtPatientNo.Text);

                }
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

                    // Akshay 
                    if (PType == "O")
                    {
                        Session["BloodGroups"] = ds.Tables[9];
                        ddlBloodGroup.DataSource = ds.Tables[9];
                    }
                    else
                    {
                        Session["BloodGroups"] = ds.Tables[1];
                        ddlBloodGroup.DataSource = ds.Tables[1];
                    }
                    
                    ddlBloodGroup.DataTextField = "BloodGroup";
                    ddlBloodGroup.DataValueField = "BloodGroupId";
                    ddlBloodGroup.DataBind();
                    ddlBloodGroup.Items.Insert(0, "Select");
                    int rowCheck = common.myInt(ds.Tables[0].Rows[0]["BloodGroupId"]);
                    if (rowCheck <= 0)
                    {
                        ddlBloodGroup.SelectedIndex = 0;

                    }
                    else
                    {
                        ddlBloodGroup.SelectedValue = common.myStr(rowCheck);
                    }

                    if (PType == "O")
                    {
                        txtAdvance.Text = common.myStr(ViewState["AdvanceAmt"]);
                        hdnAdvance.Value = txtAdvance.Text;
                    }
                    else
                    {
                        txtAdvance.Text = common.myStr(DV.ToTable().Rows[0]["AdvanceAmt"]);
                        hdnAdvance.Value = txtAdvance.Text;
                    }

                    hdnCompanyCode.Value = common.myStr(DV.ToTable().Rows[0]["CompanyCode"]);

                    imgCalYear_Click(this, null);
                    ddlGender.SelectedValue = DV.ToTable().Rows[0]["GenderId"].ToString();
                    lblAdmissionDate.Text = "";
                    lblIPNo.Text = "";
                    if (common.myInt(DV.ToTable().Rows[0]["GenderId"]) == 1)
                    {
                        tdLMPDate.Visible = true;
                    }
                    if (DV.ToTable().Rows[0]["OPIP"].ToString().Trim() == "I")
                    {
                        hdnEncounterId.Value = common.myStr(DV.ToTable().Rows[0]["EncounterId"]);
                        lblAdmissionDate.Text = DV.ToTable().Rows[0]["AdmissionDate"].ToString();
                        lblIPNo.Text = DV.ToTable().Rows[0]["EncounterNo"].ToString();
                    }
                    lblMessage.Text = "";
                    ds.Dispose();
                    BindPatientProvisionalDiagnosis();

                    if (common.myInt(hdnRegistrationId.Value) > 0)
                    {
                        BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                        //string strDate =DateTime.Now.ToString("yyyy/MM/dd h:mm:ss");
                        //  DateTime dt =(DateTime) strDate.ToString("yyyy/MM/dd h:mm:ss tt");
                        Hashtable ht = new Hashtable();
                        ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingDateTime.Value));


                        hdnIsUnplanned.Value = common.myStr(ht["@bitIsUnPlannedReturnToOT"]);


                        if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]))
                        {
                            rblUnplanned.SelectedIndex = 0;
                            rblUnplanned.Enabled = true;

                        }
                        else
                        {
                            rblUnplanned.SelectedIndex = 1;
                            rblUnplanned.Enabled = false;
                        }

                        //if ((common.myBool(ht["@bitIsUnPlannedReturnToOT"]) == false) && common.myStr(ht["@chvLastCheckoutTime"]) == "")
                        //{
                        //    rblUnplanned.SelectedIndex = 1;
                        //    rblUnplanned.Enabled = true;
                        //}
                        if (common.myStr(ht["@chvLastCheckoutTime"]) != "")
                            lblChkoutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);
                        rblIsEmergency.SelectedIndex = 0;
                    }

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
        //if (dtpDOB.SelectedDate == null)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Please select DOB...";
        //    return false;
        //}
        if (common.myInt(ddlGender.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select gender...";
            return false;
        }
        if (RadTimeFrom.SelectedDate == null || RadTimeTo.SelectedDate == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select from and to time...";
            return false;
        }
        if (common.myInt(ddlService.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select a service...";
            return false;
        }
        if (common.myStr(common.GetCheckedItems(ddlSurgeon)) == "") //if (common.myInt(ddlSurgeon.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select a surgeon...";
            return false;
        }

        if (lblAnesthetiststar.Visible)
        {
            if (common.myInt(ddlAnesthetist.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select an anesthetist...";
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
        if (lblOTEquipmentsStar.Visible)
        {
            var chkedItems = ddlOTEquipments.CheckedItems;
            if (common.myInt(chkedItems[0].Value) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select an OT Equipment(s)...";
                return false;
            }
        }
        //if (lblOTEquipmentsStar.Visible == true && chkEquipmentList.Checked == false && strEquipmentIds == "")
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Please select an OT Equipment(s)...";
        //    return false;
        //    Alert.ShowAjaxMsg("Please select equiptment", Page);
        //    return;
        //}
        //if (common.myInt(ddlAnaesthesia.SelectedValue) == 0)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Please select anesthesia...";
        //    return false;
        //}
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
            if (ValidateControls() == false)
            {
                return;
            }
            if (btnAddToList.Text.Trim() == "Add")
            {
                if (gvOTBook.Rows.Count > 0)
                {
                    dtOT = (DataTable)ViewState["OTBook"];
                    Dr = dtOT.NewRow();
                }
                else
                {
                    if (cbMainSurgery.Checked == true)
                        IsMain = 1;

                    Dr = dtOT.NewRow();
                    dtOT.Columns.Add("OTBookingID");
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
                    dtOT.Columns.Add("IsMain");
                    dtOT.Columns.Add("OTServiceDetailId");
                    dtOT.Columns.Add("IsPackage");
                    dtOT.Columns.Add("TheatreId", typeof(int));
                }
                Dr["OTBookingID"] = "0";
                Dr["ServiceName"] = ddlService.Text;
                string strPerfusionistIds = common.GetCheckedItems(ddlPerfusionist);
                string strScrubNurseIds = common.GetCheckedItems(ddlScrubNurse);
                string strFloorNurseIds = common.GetCheckedItems(ddlFloorNurse);
                string strOTTechnicianIds = common.GetCheckedItems(ddlOTTechnician);
                string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);
                //Dr["SR"] = ddlSurgeon.SelectedValue;
                string strSurgeonIds = common.GetCheckedItems(ddlSurgeon);
                Dr["SR"] = strSurgeonIds;
                //Dr["SurgeonName"] = ddlSurgeon.SelectedItem.Text;
                Dr["OTBookingDate"] = common.myDate(dtpOTDate.SelectedDate);
                Dr["OTBookingDateF"] = dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                Dr["FromTime"] = RadTimeFrom.SelectedDate.Value.TimeOfDay;
                Dr["ToTime"] = RadTimeTo.SelectedDate.Value.TimeOfDay;

                string AsstSurgeon2 = common.myStr(ddlAsstSurgeon2.SelectedValue) == "" ? "" : "," + common.myStr(ddlAsstSurgeon2.SelectedValue);

                Dr["AS"] = common.myStr(ddlAsstSurgeon1.SelectedValue) + "" + AsstSurgeon2.Trim();
                Dr["AN"] = ddlAnesthetist.SelectedValue;
                Dr["AAN"] = ddlAssttAnesthetist.SelectedValue;
                Dr["PR"] = strPerfusionistIds;
                Dr["SN"] = strScrubNurseIds;
                Dr["FN"] = strFloorNurseIds;
                Dr["ServiceID"] = ddlService.SelectedValue;
                Dr["TC"] = strOTTechnicianIds;
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

                Dr["Equipments"] = chkEquipmentList.Checked == false ? null : strEquipmentIds;
                Dr["EquipmentName"] = chkEquipmentList.Checked == false ? txtOTEquipment.Text : null;
                Dr["AnesthesiaId"] = ddlAnaesthesia.SelectedValue;
                Dr["Remarks"] = txtRemarks.Text;
                Dr["IsMain"] = IsMain;
                Dr["IsPackage"] = rblServiceType.SelectedIndex.Equals(0);
                Dr["OTServiceDetailId"] = 0;
                // Added by kuldeep kumar 3/2/2021
                bool exists = dtOT.Select().ToList().Exists(row => row["ServiceID"].ToString() == ddlService.SelectedValue);
                if (exists)
                {
                    Alert.ShowAjaxMsg("Service already added in list", Page);
                    return;
                }
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
                    string strPerfusionistIds = common.GetCheckedItems(ddlPerfusionist);
                    string strScrubNurseIds = common.GetCheckedItems(ddlScrubNurse);
                    string strFloorNurseIds = common.GetCheckedItems(ddlFloorNurse);
                    string strOTTechnicianIds = common.GetCheckedItems(ddlOTTechnician);
                    string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);

                    IsMain = 0;
                    if (cbMainSurgery.Checked == true)
                        IsMain = 1;

                    //dt1.Rows[updIndex]["SR"] = ddlSurgeon.SelectedValue;
                    string strSurgeonIds = common.GetCheckedItems(ddlSurgeon);
                    dt1.Rows[updIndex]["SR"] = strSurgeonIds;
                    //dt1.Rows[updIndex]["SurgeonName"] = ddlSurgeon.SelectedItem.Text;
                    dt1.Rows[updIndex]["OTBookingDate"] = common.myDate(dtpOTDate.SelectedDate);
                    dt1.Rows[updIndex]["OTBookingDateF"] = dtpOTDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                    dt1.Rows[updIndex]["FromTime"] = RadTimeFrom.SelectedDate.Value.TimeOfDay;
                    dt1.Rows[updIndex]["ToTime"] = RadTimeTo.SelectedDate.Value.TimeOfDay;

                    string AsstSurgeon2 = common.myStr(ddlAsstSurgeon2.SelectedValue) == "" ? "" : "," + common.myStr(ddlAsstSurgeon2.SelectedValue);

                    dt1.Rows[updIndex]["AS"] = common.myStr(ddlAsstSurgeon1.SelectedValue) + "" + AsstSurgeon2.Trim();
                    //dt1.Rows[updIndex]["AS1"] = ddlAsstSurgeon2.SelectedValue;
                    dt1.Rows[updIndex]["AN"] = ddlAnesthetist.SelectedValue;
                    dt1.Rows[updIndex]["AAN"] = ddlAssttAnesthetist.SelectedValue;
                    dt1.Rows[updIndex]["PR"] = strPerfusionistIds;
                    dt1.Rows[updIndex]["SN"] = strScrubNurseIds;
                    dt1.Rows[updIndex]["FN"] = strFloorNurseIds;

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
                    dt1.Rows[updIndex]["TC"] = strOTTechnicianIds;
                    dt1.Rows[updIndex]["Equipments"] = chkEquipmentList.Checked == false ? null : strEquipmentIds;
                    dt1.Rows[updIndex]["EquipmentName"] = chkEquipmentList.Checked == false ? txtOTEquipment.Text : null;
                    dt1.Rows[updIndex]["AnesthesiaId"] = ddlAnaesthesia.SelectedValue;
                    dt1.Rows[updIndex]["Remarks"] = txtRemarks.Text;
                    if (IsMain.Equals(1))
                    {
                        dt1.Rows[updIndex]["IsMain"] = true;
                    }
                    else
                    {
                        dt1.Rows[updIndex]["IsMain"] = false;
                    }

                    //   dt1.Rows[updIndex]["IsMain"] = Convert.ToString(IsMain);

                    dt1.Rows[updIndex]["IsPackage"] = rblServiceType.SelectedIndex.Equals(0);
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

    }
    private void ClearControls()
    {
        ddlDepartment.SelectedIndex = 0;
        ddlSubDepartment.SelectedIndex = 0;
        ddlService.Text = "";

        //ddlSurgeon.SelectedIndex = 0;
        common.UnCheckAllCheckedItems(ddlSurgeon);
        ddlAsstSurgeon1.SelectedIndex = 0;
        ddlAsstSurgeon2.SelectedIndex = 0;
        ddlAnesthetist.SelectedIndex = 0;
        ddlAssttAnesthetist.SelectedIndex = 0;
        ddlAnaesthesia.SelectedIndex = 0;
        //common.UnCheckAllCheckedItems(ddlSurgeon);
        //common.UnCheckAllCheckedItems(ddlAsstSurgeon1);
        //common.UnCheckAllCheckedItems(ddlAnesthetist);
        common.UnCheckAllCheckedItems(ddlPerfusionist);
        common.UnCheckAllCheckedItems(ddlScrubNurse);
        common.UnCheckAllCheckedItems(ddlFloorNurse);
        common.UnCheckAllCheckedItems(ddlOTTechnician);
        txtOTEquipment.Text = "";
        common.UnCheckAllCheckedItems(ddlOTEquipments);
        hdnSubDepartmentId.Value = "0";
    }
    protected void BindDepartment()
    {
        DataSet ds = new DataSet();
        try
        {
            ddlDepartment.Items.Clear();
            bMstr = new BaseC.EMRMasters(sConString);
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
            objwcfcm = null;
        }
    }
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindSudepartment();
    }
    private void BindSudepartment()
    {
        DataSet ds = new DataSet();
        objwcfcm = new BaseC.RestFulAPI(sConString);
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

    protected void btnGetInfoService_OnClick(object sender, EventArgs e)
    {
        try
        {
            PopulateOTEquipment();
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

    protected void ddlSurgeon_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        BindProviderList();
    }

    private void BindProviderList()
    {
        try
        {
            DataSet ds = new DataSet();
            DataView dv = new DataView();
            BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
            BaseC.Security baseUs = new BaseC.Security(sConString);
            bool IsAuthorizeForFullSurgeonList = baseUs.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForFullOTSurgeonListEvenTaged");

            if (ViewState["OTBookingSurgeonListOnTagBase"] == null)
            {
                ViewState["OTBookingSurgeonListOnTagBase"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                         common.myInt(Session["FacilityId"]), "OTBookingSurgeonListOnTagBase", sConString);
            }

            if (ViewState["OTBookingSurgeonListOnTagBase"].Equals("Y") && !IsAuthorizeForFullSurgeonList)
            {
                int iWeekday = 0;
                String WeekDayName = string.Empty;
                DateTime dt = new DateTime();
                dt = common.myDate(dtpOTDate.SelectedDate);

                if (ViewState["WeekDayName"] == null)
                {
                    WeekDayName = common.myStr(dt.DayOfWeek);
                }
                else
                {
                    WeekDayName = common.myStr(dt.DayOfWeek);
                }

                if (WeekDayName.Trim().ToUpper().Equals("SUNDAY"))
                {
                    iWeekday = 1;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("MONDAY"))
                {
                    iWeekday = 2;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("TUESDAY"))
                {
                    iWeekday = 3;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("WEDNESDAY"))
                {
                    iWeekday = 4;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("THURSDAY"))
                {
                    iWeekday = 5;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("FRIDAY"))
                {
                    iWeekday = 6;
                }
                else if (WeekDayName.Trim().ToUpper().Equals("SATURDAY"))
                {
                    iWeekday = 7;
                }

                ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), common.myInt(ddlOTName.SelectedValue), iWeekday);
            }
            else
            {
                ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
            }
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

                ddlSurgeon.ClearSelection();

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

            ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);

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

                ddlAsstSurgeon2.DataSource = dv.ToTable();
                ddlAsstSurgeon2.DataTextField = "EmployeeName";
                ddlAsstSurgeon2.DataValueField = "EmployeeId";
                ddlAsstSurgeon2.DataBind();
                ddlAsstSurgeon2.Items.Insert(0, new RadComboBoxItem("", "0"));


                ddlAsstSurgeon1.DataSource = dv.ToTable();
                ddlAsstSurgeon1.DataTextField = "EmployeeName";
                ddlAsstSurgeon1.DataValueField = "EmployeeId";
                ddlAsstSurgeon1.DataBind();
                ddlAsstSurgeon1.Items.Insert(0, new RadComboBoxItem("", "0"));

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
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlPerfusionist.DataSource = dv.ToTable();
                ddlPerfusionist.DataTextField = "EmployeeName";
                ddlPerfusionist.DataValueField = "EmployeeId";
                ddlPerfusionist.DataBind();
                //ddlPerfusionist.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='SN'"; // Scrub Nurse
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlScrubNurse.DataSource = dv.ToTable();
                ddlScrubNurse.DataTextField = "EmployeeName";
                ddlScrubNurse.DataValueField = "EmployeeId";
                ddlScrubNurse.DataBind();
                //ddlScrubNurse.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='FN'"; // Scrub Nurse
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlFloorNurse.DataSource = dv.ToTable();
                ddlFloorNurse.DataTextField = "EmployeeName";
                ddlFloorNurse.DataValueField = "EmployeeId";
                ddlFloorNurse.DataBind();
                //ddlFloorNurse.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
            }

            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Type='TC'"; // Scrub Nurse
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlOTTechnician.DataSource = dv.ToTable();
                ddlOTTechnician.DataTextField = "EmployeeName";
                ddlOTTechnician.DataValueField = "EmployeeId";
                ddlOTTechnician.DataBind();
                //ddlOTTechnician.Items.Insert(0, new RadComboBoxItem("", "0"));
                dv.RowFilter = "";
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
        PopulateOTEquipment();
        CheckItems(common.myStr(ViewState["EquipmentLists"]), ddlOTEquipments);
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
        PopulateOTEquipment();
        CheckItems(common.myStr(ViewState["EquipmentLists"]), ddlOTEquipments);
    }
    protected void btnAddToList_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (ValidateData() && ValidateControls() && ValidateEmergencyData())
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
        try
        {
            if (!ValidateEmergencyData())
            {
                return;
            }

            BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
            //int iCheck = objOTBooking.CheckOTBookingExist(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            // common.myInt(ddlOTName.SelectedValue), common.myDate(dtpOTDate.SelectedDate), RadTimeFrom.SelectedDate.Value.ToString("HH:mm"),
            // RadTimeTo.SelectedDate.Value.ToString("HH:mm"), Convert.ToInt32(hdnRegistrationId.Value), "OT-UNCONF", Convert.ToInt32(ddlService.SelectedValue));
            //if (iCheck == 1)
            //{
            //    return;
            //}

            BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
            Hashtable ht = new Hashtable();
            ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingDateTime.Value));
            //common.myStr(hdnOTBookingDateTime.Value));//.ToString("yyyy-MM-dd hh:mm:ss tt"));
            //BaseC.clsEMRBilling objEmrBilling = new BaseC.clsEMRBilling(sConString);
            //int x = objEmrBilling.getIsOtAdvanceMandatory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["CompanyId"]));
            //if (common.myInt(Session["CompanyId"]) != 0)
            //{
            //    if (x == 1 && (common.myInt(txtAdvance.Text) < 2000))
            //    {

            //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //        lblMessage.Text = "OT Advance Is mandatory agianst this company...";
            //        return;

            //    }

            //}

            StringBuilder sXmlServiceDetails = new StringBuilder();
            StringBuilder sXmlResourceDetails = new StringBuilder();
            StringBuilder sXmlEquipmentDetails = new StringBuilder();
            ArrayList colService = new ArrayList();
            ArrayList colResource = new ArrayList();
            ArrayList colEqp = new ArrayList();
            Hashtable htOut = new Hashtable();

            if (common.myStr(ViewState["AllowOTBookingForUnregisteredPatient"]) == "N")
            {
                if (common.myStr(txtRegNo.Text.Trim()) == string.Empty)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select patient...";
                    return;
                }
            }

            if (common.myStr(txtFName.Text) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter first name...";
                return;
            }
            if (dtpDOB.SelectedDate == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select DOB...";
                return;
            }
            if (common.myInt(ddlGender.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select gender...";
                return;
            }
            if (common.myStr(txtMobileNo.Text) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter mobile no...";
                return;
            }
            if (RadTimeFrom.SelectedDate == null || RadTimeTo.SelectedDate == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select from and to time...";
                return;
            }

            if (common.myStr(rdoImpReq.SelectedValue).Equals("Y"))
            {
                if (lbltxtImpReqRem.Visible && (txtImpReqRem.Text.Trim()).Length.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter Implant Item Remark...";
                    txtImpReqRem.Focus();
                    return;
                }
            }

            if (common.myStr(rdoEquiReq.SelectedValue).Equals("Y"))
            {
                if (lbltxtEquReqRem.Visible && (txtEquiReqRem.Text.Trim()).Length.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter Equipment Remark...";
                    txtEquiReqRem.Focus();
                    return;
                }
            }
            int intTo = 0, intFrom = 0;
            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("HH:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("HH:mm");
            if (FromTime.Contains(":") == true)
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = Convert.ToInt16(strFromTime);
            }


            if (FromTo.Contains(":") == true)
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = Convert.ToInt16(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time...", Page);
                return;
            }

            if (common.myStr(txtProvisionalDiagnosis.Text) == "")
            {
                foreach (GridViewRow item in gvProvisionalDiagnosis.Rows)
                {
                    HiddenField hdnProvisionalDiagnosisID = (HiddenField)item.FindControl("hdnProvisionalDiagnosisID");
                    if (common.myInt(hdnProvisionalDiagnosisID.Value) == 0)
                    {
                        Alert.ShowAjaxMsg("Please Enter Provisional Diagnosis", Page);
                        return;
                    }
                }
            }
            //else if (common.myStr(txtProvisionalDiagnosis.Text) != "")
            //{
            //    if (common.myInt(ddlDiagnosisSearchCodes.SelectedValue) == 0)
            //    {
            //        Alert.ShowAjaxMsg("Please select Search Keyword", Page);
            //        return;
            //    }
            //}

            bool IsMain = false;

            foreach (GridViewRow gvrow in gvOTBook.Rows)
            {
                HiddenField hdnOTServiceDetailId = (HiddenField)gvrow.FindControl("hdnOTServiceDetailId");
                HiddenField hdnServiceID = (HiddenField)gvrow.FindControl("hdnServiceID");
                HiddenField hdnSurgeon = (HiddenField)gvrow.FindControl("hdnSurgeon");
                HiddenField hdnAsstSurgeon = (HiddenField)gvrow.FindControl("hdnAsstSurgeon");

                HiddenField hdnAnesthetist = (HiddenField)gvrow.FindControl("hdnAnesthetist");
                HiddenField hdnAssttAnesthetist = (HiddenField)gvrow.FindControl("hdnAssttAnesthetist");
                HiddenField hdnPerfusionist = (HiddenField)gvrow.FindControl("hdnPerfusionist");
                HiddenField hdnScrubNurse = (HiddenField)gvrow.FindControl("hdnScrubNurse");
                HiddenField hdnFloorNurse = (HiddenField)gvrow.FindControl("hdnFloorNurse");
                HiddenField hdnTechnician = (HiddenField)gvrow.FindControl("hdnTechnician");

                HiddenField hdnEquipment = (HiddenField)gvrow.FindControl("hdnEquipment");
                HiddenField hdnEquipmentName = (HiddenField)gvrow.FindControl("hdnEquipmentName");

                HiddenField hdnAnesthesiaId = (HiddenField)gvrow.FindControl("hdnAnesthesiaId");
                //HiddenField hdnRemarks = (HiddenField)gvrow.FindControl("hdnRemarks");
                HiddenField hdnIsMain = (HiddenField)gvrow.FindControl("hdnIsMain");

                if (common.myStr(hdnIsMain.Value).ToUpper().Equals("TRUE"))
                {
                    hdnIsMain.Value = "1";
                }

                IsMain = IsMain | common.myBool(common.myInt(hdnIsMain.Value));

                //ViewState["Remarks"] = common.myStr(hdnRemarks.Value);
                ViewState["AnesthesiaId"] = common.myInt(hdnAnesthesiaId.Value);
                colService.Add(common.myInt(hdnOTServiceDetailId.Value)); // Id - Zero for New Entry
                colService.Add(common.myInt(hdnServiceID.Value)); // Service Id
                colService.Add(common.myInt(hdnIsMain.Value));
                sXmlServiceDetails.Append(common.setXmlTable(ref colService));


                //if (hdnSurgeon.Value != "" && hdnSurgeon.Value != "0")
                //{
                //    colResource.Add(0); // Id - Zero for New Entry
                //    colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                //    colResource.Add(common.myInt(hdnSurgeon.Value)); // Resource Id
                //    colResource.Add(common.myStr("SR")); // Resource Type Surgeon
                //    sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                //}
                if (common.myStr(hdnSurgeon.Value) != "")
                {
                    string[] strSurgeonIds = hdnSurgeon.Value.Split(',');
                    foreach (string Id in strSurgeonIds)
                    {
                        if (Id != "")
                        {
                            if (hdnSurgeon.Value != "" && hdnSurgeon.Value != "0")
                            {
                                colResource.Add(0); // Id - Zero for New Entry
                                colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                                colResource.Add(common.myInt(Id)); // Resource Id
                                colResource.Add(common.myStr("SR")); // Resource Type Surgeon
                                sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                            }
                        }
                    }
                }

                //string[] strAsstSurgeonIds = hdnAsstSurgeon1.Value.Split(',');
                //foreach (string Id in strAsstSurgeonIds)
                //{
                //    if (Id != "")
                //    {
                string[] AsstSurgeon = hdnAsstSurgeon.Value.Split(',');
                if (AsstSurgeon.Length > 1)
                {
                    if (hdnAsstSurgeon.Value != "" && hdnAsstSurgeon.Value != "0")
                    {
                        colResource.Add(0); // Id - Zero for New Entry
                        colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                        colResource.Add(common.myInt(AsstSurgeon[0])); // Resource Id
                        colResource.Add(common.myStr("AS")); // Resource Type Asst Surgeon
                        sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                    }

                    if (hdnAsstSurgeon.Value != "" && hdnAsstSurgeon.Value != "0")
                    {
                        colResource.Add(0); // Id - Zero for New Entry
                        colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                        colResource.Add(common.myInt(AsstSurgeon[1])); // Resource Id
                        colResource.Add(common.myStr("AS")); // Resource Type Asst Surgeon
                        sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                    }
                }
                else
                {
                    if (AsstSurgeon.ToString() != "")
                    {
                        if (hdnAsstSurgeon.Value != "" && hdnAsstSurgeon.Value != "0")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                            colResource.Add(common.myInt(AsstSurgeon[0])); // Resource Id
                            colResource.Add(common.myStr("AS")); // Resource Type Asst Surgeon
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }

                    }
                }
                //    }
                //}
                //string[] strAnesthetistIds = hdnAnesthetist.Value.Split(',');
                //foreach (string Id in strAnesthetistIds)
                //{
                //    if (Id != "")
                //    {
                if (hdnAnesthetist.Value != "" && hdnAnesthetist.Value != "0")
                {
                    colResource.Add(0); // Id - Zero for New Entry
                    colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                    colResource.Add(common.myInt(hdnAnesthetist.Value)); // Resource Id
                    colResource.Add(common.myStr("AN")); // Resource Type Anesthetist
                    sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                }

                if (hdnAssttAnesthetist.Value != "" && hdnAssttAnesthetist.Value != "0")
                {
                    colResource.Add(0); // Id - Zero for New Entry
                    colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                    colResource.Add(common.myInt(hdnAssttAnesthetist.Value)); // Resource Id
                    colResource.Add(common.myStr("AAN")); // Resource Type Asstt Anesthetist
                    sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                }

                if (common.myStr(hdnPerfusionist.Value) != "")
                {
                    string[] strPerfusionistIds = hdnPerfusionist.Value.Split(',');
                    foreach (string Id in strPerfusionistIds)
                    {
                        if (Id != "")
                        {
                            colResource.Add(0); // Id - Zero for New Entry
                            colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
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
                            colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
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
                            colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
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
                            colResource.Add(common.myInt(hdnServiceID.Value)); // Service Id
                            colResource.Add(common.myInt(Id)); // Resource Id
                            colResource.Add(common.myStr("TC")); // Resource Type Technician
                            sXmlResourceDetails.Append(common.setXmlTable(ref colResource));
                        }
                    }
                }
                if (chkEquipmentList.Checked == true)
                {
                    if (common.myStr(hdnEquipment.Value) != "")
                    {
                        string[] strEquipmentIds = hdnEquipment.Value.Split(',');
                        foreach (string Id in strEquipmentIds)
                        {
                            if (Id != "")
                            {
                                colEqp.Add(0); // Id - Zero for New Entry
                                colEqp.Add(common.myInt(hdnServiceID.Value)); // Service Id
                                colEqp.Add(common.myInt(Id)); // Equipment Id
                                colEqp.Add(DBNull.Value); // Equipment Name  

                                sXmlEquipmentDetails.Append(common.setXmlTable(ref colEqp));
                            }
                        }
                    }
                }
                else
                {
                    if (hdnEquipmentName.Value != "")
                    {
                        colEqp.Add(0); // Id - Zero for New Entry
                        colEqp.Add(common.myInt(hdnServiceID.Value)); // Service Id
                        colEqp.Add(DBNull.Value); // Equipment Id  
                        colEqp.Add(hdnEquipmentName.Value); // Equipment Name
                        sXmlEquipmentDetails.Append(common.setXmlTable(ref colEqp));
                    }
                }
            }

            if (sXmlServiceDetails.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please add atleast one surgery before saving...";
                return;
            }
            if (sXmlResourceDetails.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please add surgeon details before saving...";
                return;
            }

            if (!IsMain)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Atleast one service set to main...";
                return;
            }

            if (common.myInt(Session["cSvCheck"]) == 0)
            {
                objOTBooking.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objOTBooking.LoginFacilityId = common.myInt(Session["FacilityId"]);
                objOTBooking.UserId = common.myInt(Session["UserId"]);
                objOTBooking.RegId = common.myInt(hdnRegistrationId.Value);
                objOTBooking.RegistrationNo = common.myStr(txtRegNo.Text);
                objOTBooking.EncounterId = common.myInt(hdnEncounterId.Value);
                if (common.myStr(Session["OPIP"]) != "I" && common.myStr(Session["ModuleName"]) == "EMR")
                {
                    objOTBooking.BookingEncounterId = common.myInt(Session["EncounterId"]);
                }

                objOTBooking.FirstName = common.myStr(txtFName.Text);
                objOTBooking.MiddleName = common.myStr(txtMName.Text);
                objOTBooking.LastName = common.myStr(txtLName.Text);
                objOTBooking.AgeYears = common.myInt(txtAgeYears.Text);
                objOTBooking.AgeMonths = common.myInt(txtAgeMonths.Text);
                objOTBooking.AgeDays = common.myInt(txtAgeDays.Text);
                objOTBooking.DOB = common.myStr(common.myDate(dtpDOB.SelectedDate));
                objOTBooking.Gender = common.myInt(ddlGender.SelectedValue);
                objOTBooking.TheaterId = common.myInt(ddlOTName.SelectedValue);
                objOTBooking.OTBookingDate = common.myStr(common.myDate(dtpOTDate.SelectedDate));
                objOTBooking.FromTime = RadTimeFrom.SelectedDate.Value.ToString("hh:mm tt");
                objOTBooking.ToTime = RadTimeTo.SelectedDate.Value.ToString("hh:mm tt");
                objOTBooking.Remarks = common.myStr(txtRemarks.Text);
                objOTBooking.OTBookingId = common.myInt(hdnOTBookingID.Value);
                objOTBooking.Active = 1;
                objOTBooking.AnesthesiaId = common.myInt(ViewState["AnesthesiaId"]);
                objOTBooking.chrMobileNo = txtMobileNo.Text;
                int IsPackage = 0;
                if (common.myStr(rblServiceType.SelectedValue) == "IPP")
                    IsPackage = 1;
                objOTBooking.IsPackage = IsPackage;
                objOTBooking.XMLServiceDetails = sXmlServiceDetails;
                objOTBooking.XMLResourceDetails = sXmlResourceDetails;

                if (sXmlEquipmentDetails.ToString() != "")
                    objOTBooking.XMLEquipmentDetails = sXmlEquipmentDetails;

                if (common.myStr(txtProvisionalDiagnosis.Text) != "")
                {
                    objOTBooking.ProvisionalDiagnosis = common.myStr(txtProvisionalDiagnosis.Text);
                }
                if (common.myInt(ddlDiagnosisSearchCodes.SelectedValue) != 0)
                {
                    objOTBooking.DiagnsisSearchKeyId = common.myStr(ddlDiagnosisSearchCodes.SelectedValue);
                }
                if (rdoImpReq.SelectedValue.Equals("Y") && !txtImpReqRem.Text.Trim().Length.Equals(0))
                {
                    objOTBooking.isImplantRequired = 1;
                    objOTBooking.ImplantRequiredRemark = txtImpReqRem.Text.Trim();
                }
                if (rdoEquiReq.SelectedValue.Equals("Y") && !txtEquiReqRem.Text.Trim().Length.Equals(0))
                {
                    objOTBooking.isEquipmentRequired = 1;
                    objOTBooking.EquipmentRequiredRemark = txtEquiReqRem.Text.Trim();
                }
                if (chkisBloodRequired.Checked)
                {
                    objOTBooking.isBloodRequired = 1;
                    objOTBooking.BloodUnit = common.myInt(ddlBloodUnit.SelectedItem);
                }
                else
                {
                    objOTBooking.isBloodRequired = 0;
                }


                //if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]) == false)
                //{
                //    //rblUnplanned.SelectedIndex = 1;
                //    rblUnplanned.SelectedValue = "0";
                //}
                //else
                //{
                //    //rblUnplanned.SelectedIndex = 0;
                //    rblUnplanned.SelectedValue = "1";
                //}

                //  rblUnplanned.SelectedIndex = (hdnIsUnplanned != null && common.myBool(hdnIsUnplanned.Value)) ? 0 : 1 ;



                objOTBooking.intPatientAdvance = common.myDec(hdnAdvance.Value);
                objOTBooking.isUnplannedOT = common.myInt(rblUnplanned.SelectedValue);
                objOTBooking.chvStatusType = common.myStr("UnplannedOTMsg");
                objOTBooking.IsEmergency = common.myInt(rblIsEmergency.SelectedValue);
                objOTBooking.OTRequestID = common.myInt(ViewState["OTRequestID"]);
                if (chkICUrequired.Checked)
                {
                    objOTBooking.IsICURequired = 1;
                }
                else
                {
                    objOTBooking.IsICURequired = 0;
                }
                if (common.myInt(ddlGender.SelectedValue) == 1)
                {
                    objOTBooking.LMPDatetime = common.myStr(common.myDate(dtLMPDate.SelectedDate));
                }
                else
                {
                    objOTBooking.LMPDatetime = common.myStr("");
                }
                if (chkVentilatorRequired.Checked)
                {
                    objOTBooking.IschkVentilatorRequired = 1;
                }
                else
                {
                    objOTBooking.IschkVentilatorRequired = 0;

                }
                if (chkReExploration.Checked == true)
                {
                    objOTBooking.ReExploration = true;
                }
                else
                {
                    objOTBooking.ReExploration = false;
                }

                objOTBooking.BloodGroupId = common.myInt(ViewState["BloodGroupId"]);// Akshay
                // test
                htOut = objOTBooking.SaveOTBooking(objOTBooking);
                Session["cSvCheck"] = "1";
                ibtnSave.Visible = false;
                if (common.myStr(htOut["chvErrorStatus"]).Contains("Saved"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Record Saved with OT Booking No: " + common.myStr(htOut["chvOTBookingNo"]);
                }
                else if (common.myStr(htOut["chvErrorStatus"]).Contains("Updated"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Record Updated for OT Booking No: " + common.myStr(hdnOTBookingNO.Value);
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
                    Session["cSvCheck"] = "0";
                    ibtnSave.Visible = true;
                }
                hdnOTBookingID.Value = htOut["intId"].ToString();
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
    protected void CheckItems(string strIdValues, RadComboBox ddl)
    {
        string[] strIds = strIdValues.Split(',');
        if (strIds.Length > 0)
        {
            foreach (string Id in strIds)
            {
                foreach (RadComboBoxItem chk in ddl.Items)
                {
                    if (chk.Value.Trim() == Id.Trim())
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
        string strEquipmentIds = common.GetCheckedItems(ddlOTEquipments);
        if (ddlService.SelectedValue == "")
        {
            lblMessage.Text = "Select a service!";
            return false;
        }
        else if (ddlOTName.SelectedValue == "")
        {
            lblMessage.Text = "Select a theater!";
            return false;
        }
        else if (dtpOTDate.SelectedDate == null)
        {
            lblMessage.Text = "Select booking date!";
            return false;
        }
        else if (RadTimeFrom.SelectedDate == null)
        {
            lblMessage.Text = "Select booking from time!";
            return false;
        }
        else if (RadTimeTo.SelectedDate == null)
        {
            lblMessage.Text = "Select booking to time!";
            return false;
        }
        else if (ddlOTName.SelectedValue == null)
        {
            lblMessage.Text = "Select a theater!";
            return false;
        }
        else if (common.myStr(common.GetCheckedItems(ddlSurgeon)) == null) //else if (ddlSurgeon.SelectedValue == null)
        {
            lblMessage.Text = "Select a surgeon!";
            return false;
        }

        else if (strEquipmentIds == "")
        {
            if (chkEquipmentList.Checked == false)
            {
                if (txtOTEquipment.Text == "")
                {
                    lblMessage.Text = "Type OT Equipment!";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (lblOTEquipmentsStar.Visible == true)
                {
                    lblMessage.Text = "Select an OT Equipment!";
                    Alert.ShowAjaxMsg("Select an OT Equipment!", Page);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        else
        {
            return true;
        }




    }

    private bool ValidateEmergencyData()
    {
        #region Emergency Validation
        if (!common.myStr(ViewState["IsOTEmergencyValidation"]).Equals("Y"))
        {
            return true;
        }

        BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataTable dtHolidays = new DataTable();
        if (common.myStr(common.myDate(dtpOTDate.SelectedDate).DayOfWeek).ToUpper().Equals("SUNDAY") && common.myInt(rblIsEmergency.SelectedItem.Value).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Its Sunday please select OT Type as emeregency";
            Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
            return false;
        }

        var Fromdate = RadTimeFrom.SelectedDate;
        var Todate = RadTimeTo.SelectedDate;
        if (((Convert.ToDateTime(Fromdate).Hour >= 19 || Convert.ToDateTime(Fromdate).Hour < 7) || ((Convert.ToDateTime(Todate).Hour >= 19 && Convert.ToDateTime(Todate).Minute > 0) || Convert.ToDateTime(Todate).Hour < 7)) && common.myInt(rblIsEmergency.SelectedItem.Value).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Time duration is between 7 PM and 7 AM ,please select OT Type emeregency";
            Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
            return false;
        }


        dtHolidays = objOTBooking.GetHospitalHolidays(common.myInt(Session["HospitalLocationId"]), common.myDate(dtpOTDate.SelectedDate));

        if (common.myInt(dtHolidays.Rows.Count) > 0)
        {
            if (common.myInt(dtHolidays.Rows.Count) > 0 && common.myInt(rblIsEmergency.SelectedItem.Value).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Its Holiday for " + common.myStr(dtHolidays.Rows[0]["HolidayDescription"]) + " please select OT Type as emeregency";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return false;
            }
        }
        dtHolidays.Dispose();
        objOTBooking = null;
        return true;

        #endregion

    }
    protected void dtpDOB_OnSelectedDateChanged(object sender, EventArgs e)
    {
        imgCalYear_Click(this, null);
    }

    protected void dtpOTDate_OnSelectedDateChanged(object sender, EventArgs e)
    {
        DateTime dt = new DateTime();
        dt = common.myDate(dtpOTDate.SelectedDate);
        ViewState["WeekDayName"] = dt.DayOfWeek;
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

    protected void BindPatientProvisionalDiagnosis()
    {
        DataSet ds = new DataSet();
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            int RegId = common.myInt(hdnRegistrationId.Value) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(hdnRegistrationId.Value);
            int EncId = common.myInt(hdnEncounterId.Value) == 0 ? common.myInt(Session["EncounterId"]) : common.myInt(hdnEncounterId.Value);
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), RegId, EncId, common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvProvisionalDiagnosis.DataSource = ds.Tables[0];
                gvProvisionalDiagnosis.DataBind();
            }
            else
            {
                DataRow rd = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(rd);
                gvProvisionalDiagnosis.DataSource = ds.Tables[0];
                gvProvisionalDiagnosis.DataBind();
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
            objDiag = null;
        }
    }

    protected void btnAddDiagnosisSerchOnClientClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindDiagnosisSearchCode();
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

    protected void rblUnplanned_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((rblUnplanned.SelectedIndex) == 1)
        {

        }
    }

    protected void chkisBloodRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkisBloodRequired.Checked == true)
        {
            ddlBloodUnit.Enabled = true;
            lblNoOfBloodUnit.Enabled = true;

        }
        else
        {
            ddlBloodUnit.Enabled = false;
            lblNoOfBloodUnit.Enabled = false;

            ddlBloodUnit.SelectedIndex = 0;

        }

    }
    public void populateBloodUnit()
    {
        ddlBloodUnit.Items.Insert(0, new ListItem("Select"));
        for (int i = 1; i <= 20; i++)
        {
            ddlBloodUnit.Items.Insert(i, new ListItem((i).ToString()));
        }
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
        DataSet dsPage = hsSetup.GetPageMandatoryAndEnableFields(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "Operation Theatre", "OT Booking");
        if (dsPage.Tables.Count > 0)
        {
            if (dsPage.Tables[0].Rows.Count > 0)
            {
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblAnesthetiststar, "Anesthetist");
                if (lblAnesthetiststar.Visible) { lblAnesthetiststar.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffce"); }
                ddlAnesthetist.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "Anesthetist");

                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblOTEquipmentsStar, "OTEquipments");
                if (lblOTEquipmentsStar.Visible) { lblOTEquipmentsStar.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffce"); }
                ddlOTEquipments.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "OTEquipments");

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
    //--------------------- Working on this function for getting value from OT Request Table
    private void BindOTRequestDetails(int OTRequestId)
    {
        // BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        try
        {
            int HospId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            objwcfOt = new BaseC.RestFulAPI(sConString);
            ds = objwcfOt.getOTRequestDetails(HospId, FacilityId, OTRequestId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                dr = ds.Tables[0].Rows[0];
                dtpOTDate.SelectedDate = Convert.ToDateTime(dr["OTBookingDate"]);
                if (common.myStr(dr["RegistrationNo"]) != "")
                {
                    ddlSearchOn.SelectedValue = "0";
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    txtPatientNo.Text = common.myStr(dr["RegistrationNo"]);
                    txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
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

                if (common.myInt(dr["GenderId"]) == 1)
                {
                    tdLMPDate.Visible = true;
                }
                lblIPNo.Text = common.myStr(dr["EncounterNo"]);
                lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);

                //ddlBloodUnit.SelectedValue = common.myStr(dr["Bloodunit"]);
                //ddlOTName.Enabled = true;

                if (common.myInt(dr["TheatreId"]) > 0)
                {
                    ddlOTName.SelectedIndex = ddlOTName.Items.IndexOf(ddlOTName.Items.FindItemByValue(common.myInt(dr["TheatreId"]).ToString()));
                    ddlOTName.Enabled = true;
                }

                RadTimeFrom.SelectedDate = common.myDate(common.myStr(dr["FromTime"]));
                RadTimeTo.SelectedDate = common.myDate(common.myStr(dr["ToTime"]));
                txtRemarks.Text = common.myStr(dr["Remarks"]);
                txtProvisionalDiagnosis.Text = common.myStr(dr["Diagnosis"]);
                //ddlDiagnosisSearchCodes.SelectedValue = common.myStr(dr["DiagnosisSearchId"]);

                rdoInfectiousCase.SelectedValue = common.myBool(dr["IsInfectiousCase"]) == true ? "1" : "0";

                txtInfectiousCaseRemarks.Text = common.myStr(dr["InfectiousCaseRemarks"]);

                hdnOTBookingDateTime.Value = common.myStr(RadTimeFrom.SelectedDate);

                //rdoImpReq.SelectedIndex = rdoImpReq.Items.IndexOf(rdoImpReq.Items.FindByValue(common.myStr(dr["isImplantRequired"])));
                //rdoImpReq_SelectedIndexChanged(null, null);
                //txtImpReqRem.Text = common.myStr(dr["ImplantRequiredRemark"]);

                if (common.myBool(dr["isEmergency"]) == true)
                    rblIsEmergency.SelectedIndex = 1;
                else
                    rblIsEmergency.SelectedIndex = 0;

                //BaseC.RestFulAPI objRest = new BaseC.RestFulAPI(sConString);
                //int x = 0;
                //Hashtable ht = new Hashtable();

                //ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId), common.myInt(Session["FacilityId"]), common.myDate(dtpOTDate.SelectedDate));
                //ht = objRest.GetIsunPlannedReturnToOt(common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), Convert.ToDateTime(hdnOTBookingDateTime.Value));
                //if (common.myBool(ht["@bitIsUnPlannedReturnToOT"]) == true || common.myStr(ht["@chvLastCheckoutTime"]).Trim() == "")
                //{
                //    if (common.myBool(dr["IsUnplannedReturnToOT"]) == true)
                //    {
                //        rblUnplanned.SelectedIndex = 0;
                //        rblUnplanned.Enabled = true;
                //    }
                //    else if (common.myBool(dr["IsUnplannedReturnToOT"]) == false)
                //    {
                //        rblUnplanned.SelectedIndex = 1;
                //    }
                //    else
                //    {
                //        rblUnplanned.SelectedIndex = 1;
                //        rblUnplanned.Enabled = true;
                //    }
                //}

                // DateTime dt = new DateTime();
                // dt = common.myDate(dr["OTCheckouttime"]);
                //lblChkoutTime.Text = common.myStr(dt);
                //lblChkoutTime.Text =Convert.ToDateTime(common.myStr(dr["OTCheckouttime"]));
                //   lblChkoutTime.Text = common.myStr(ht["@chvLastCheckoutTime"]);

                ViewState["OTBook"] = ds.Tables[0];
                gvOTBook.DataSource = ds.Tables[0];
                gvOTBook.DataBind();
                BindPatientProvisionalDiagnosis();
                bindAdvanceDetails();
                txtAdvance.Text = common.myStr(ViewState["AdvanceAmt"]);
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

    protected void rblIsEmergency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((rblIsEmergency.SelectedIndex) == 1)
        {

        }
    }

    protected void btnOTReqList_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindow1.NavigateUrl = "~/OTScheduler/OTRequestList.aspx?From=POPUP";
            RadWindow1.Height = 550;
            RadWindow1.Width = 900;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.OnClientClose = "OtRequestOnClientClose";
            //RadWindow1.VisibleOnPageLoad = true;
            //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            //RadWindow1.Modal = true;
            //RadWindow1.VisibleStatusbar = false;

            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnGetOTRequestDetails_Click(object sender, EventArgs e)
    {
        if (common.myInt(Session["SelectReq"]) == 1)
        {
            hdnRegistrationId.Value = common.myStr(Session["RegistrationId"]);
            hdnRegistrationNo.Value = common.myStr(Session["RegistrationNo"]);
            hdnEncounterId.Value = common.myStr(Session["EncounterId"]);
            hdnEncounterNo.Value = common.myStr(Session["EncounterNo"]);
            hdnOTRequestID.Value = common.myStr(Session["OTRequestID"]);


            OTRequestID = common.myInt(hdnOTRequestID.Value);
            ViewState["OTRequestID"] = OTRequestID;

            BindOTRequestDetails(OTRequestID);
            ddlOTName.Focus();
        }
    }

    public void DisableControls(ControlCollection con)
    {
        foreach (Control c in con)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Enabled = false;
            }
            else if (c is RadioButton)
            {
                ((RadioButton)c).Enabled = false;
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = false;
            }
            else if (c is RadioButtonList)
            {
                ((RadioButtonList)c).Enabled = false;
            }
            else if (c is RadComboBox)
            {
                ((RadComboBox)c).Enabled = false;
            }
            DisableControls(c.Controls);
        }
    }

    //Added By Manoj Puri
    protected void rdoEquiReq_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtEquiReqRem.Visible = false;
            lbltxtEquReqRem.Visible = false;
            lblRemarksEqu.Visible = false;
            if (rdoEquiReq.SelectedValue == "Y")
            {
                txtEquiReqRem.Visible = true;
                lbltxtEquReqRem.Visible = true;
                lblRemarksEqu.Visible = true;
            }
            updrdoEquipment.Update();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }

    protected void ddlBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["BloodGroupId"] = ddlBloodGroup.SelectedValue;
    }
}
