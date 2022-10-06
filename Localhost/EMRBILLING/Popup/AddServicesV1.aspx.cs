using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRBILLING_Popup_AddServicesV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    public string strxmlstring = "";
    clsExceptionLog objException = new clsExceptionLog();
    private static string flagIsAllowToDisplayServicePrice, isShowEmergencyCheckBoxForEmergencyCharge = "N", isShowBedSideCheckBoxForBedSideCharge = "N", IsAllowBedSideChargeUncheck = "N";
    public string Getxmlstring
    {
        get { return strxmlstring; }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        RadDateTimePicker1.MinDate = System.DateTime.Now.Date;
        if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
        {
            Response.Redirect("~/Login.aspx?Logout=1", false);
            return;
        }
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        try
        {
            if (!IsPostBack)
            {
                if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR") && common.myStr(Session["OPIP"]).Equals("O"))
                {
                    Response.Redirect("~/Default.aspx", false);
                    return;
                }

                //yogesh 3/10/2022
                BindFevoriteGrid("");
                ViewState["IsShowFavouriteInWardServiceOrder"] = common.myStr(objBill.getHospitalSetupValue("IsShowFavouriteInWardServiceOrder", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

                if(common.myStr(ViewState["IsShowFavouriteInWardServiceOrder"]).Equals("Y"))
                {
                    cmdfavorite.Visible = true;
                    DivFeb.Visible = true;
                } else
                {
                    cmdfavorite.Visible = false;
                    DivFeb.Visible = false;
                }



                gethospitalSetupvalues();
                BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
                DataSet dsStatus = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "OPBILL", "");
                if (dsStatus != null)
                {
                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BRS'";
                        ViewState["BRSColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BES'";
                        ViewState["BESColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                        dsStatus.Tables[0].DefaultView.RowFilter = "Code='BAR'";
                        ViewState["BARColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    }
                }

                Session["check"] = 0;
                ViewState["Regid"] = common.myStr(Request.QueryString["Regid"]);
                ViewState["RegNo"] = common.myStr(Request.QueryString["RegNo"]);
                ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
                ViewState["EncNo"] = common.myStr(Request.QueryString["EncNo"]);
                ViewState["OP_IP"] = common.myStr(Request.QueryString["OP_IP"]);
                ViewState["FromWard"] = common.myStr(Request.QueryString["FromWard"]);
                ViewState["CompanyId"] = common.myStr(Request.QueryString["CompanyId"]);
                ViewState["InsuranceId"] = common.myStr(Request.QueryString["InsuranceId"]);
                ViewState["CardId"] = common.myStr(Request.QueryString["CardId"]);
                ViewState["PayerType"] = common.myStr(Request.QueryString["PayerType"]);
                ViewState["BType"] = common.myStr(Request.QueryString["BType"]);
                ViewState["From"] = common.myStr(Request.QueryString["From"]);
                if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
                {
                    ViewState["Regid"] = common.myStr(Session["RegistrationId"]);
                    ViewState["RegNo"] = common.myStr(Session["RegistrationNo"]);
                    ViewState["EncId"] = common.myStr(Session["EncounterId"]);
                    ViewState["EncNo"] = common.myStr(Session["EncounterNo"]);
                    ViewState["OP_IP"] = "I";
                    ViewState["FromWard"] = "Y";
                    ViewState["CompanyId"] = "0";
                    ViewState["InsuranceId"] = "0";
                    ViewState["CardId"] = "0";
                    ViewState["PayerType"] = "";
                    ViewState["BType"] = "";
                    ViewState["From"] = "";
                    ibtnClose.Visible = false;
                }
                divExcludedService.Visible = false;
                divServicelimit.Visible = false;

                if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                {
                    lblPageType.Text = "OP Order";
                    ibtnSave.Text = "Proceed";
                    ibtnSave.ToolTip = "Click to proceed to OP Bill Page...";
                }
                else// I
                {
                    lblPageType.Text = "IP Order";
                }
                hdnXmlString.Value = "";
                hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                hdnCardId.Value = common.myStr(ViewState["CardId"]);
                dtOrderDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
                dtOrderDate.SelectedDate = common.myDate(DateTime.Now);
                BindMinutes();
                if (!common.myStr(ViewState["PayerType"]).Equals(""))
                {
                    ViewState["PayParty"] = common.myStr(ViewState["PayerType"]);
                    ViewState["BillType"] = common.myStr(ViewState["BType"]);
                    hdnBilltype.Value = common.myStr(ViewState["BType"]);
                }
                if (common.myStr(ViewState["From"]).Equals("WARD") || common.myStr(ViewState["FromWard"]).Equals("Y"))
                {
                    lnkViewLabOrders.Visible = true;
                    lnkSampleCollection.Visible = true;
                    chkApprovalRequired.Visible = true;
                }
                else
                {
                    lnkSampleCollection.Visible = false;
                    //lnkViewLabOrders.Visible = false;
                    if (common.myStr(ViewState["From"]).Equals("BILL"))  //Added By Manoj Puri
                    {
                        lnkViewLabOrders.Visible = true;
                    }
                }
                hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

                BaseC.Security objSecurity = new BaseC.Security(sConString);  //
                ViewState["IsAllowToAddBlockedService"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedService");
                objSecurity = null;

                ViewState["IsSampleCollectionRequiredFromWard"] = common.myStr(objBill.getHospitalSetupValue("IsSampleCollectionRequiredFromWard", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

                //added by bhakti  
                ViewState["isRequireIPBillOfflineMarking"] = common.myStr(objBill.getHospitalSetupValue("isRequireIPBillOfflineMarking", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

                #region isShowEmergency
                isShowEmergencyCheckBoxForEmergencyCharge = common.myStr(objBill.getHospitalSetupValue("isShowEmergencyCheckBoxForEmergencyCharge", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                chkIsEmergency.Visible = isShowEmergencyCheckBoxForEmergencyCharge.Equals("Y");
                #endregion

                #region isShowBedSideCharge
                isShowBedSideCheckBoxForBedSideCharge = common.myStr(objBill.getHospitalSetupValue("isShowBedSideCheckBoxForBedSideCharge", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
                chkisbedsidecharges.Visible = isShowBedSideCheckBoxForBedSideCharge.Equals("Y");//by Palendra
                #endregion

                if (common.myStr(ViewState["IsSampleCollectionRequiredFromWard"]) == "N")
                {
                    lnkSampleCollection.Visible = false;
                }
                divMiscellaneousRemarks.Visible = false;
                divConfirmation.Visible = false;
                if (common.myInt(hdnDecimalPlaces.Value).Equals(0))
                {
                    hdnDecimalPlaces.Value = "2";
                }
                dvConfirmPrintingOptions.Visible = false;
                BindPatientHiddenDetails(common.myInt(ViewState["RegNo"]));
                BindComboBox();
                BindPatientProvisionalDiagnosis();
                chkIsGenerateAdvance.Visible = false;
                if (common.myStr(ViewState["OP_IP"]).Equals("I"))
                {
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), "IsGenerateAdvanceAgainstOrder", sConString).Equals("Y"))
                        chkIsGenerateAdvance.Visible = true;
                    tdAdviserDoctor.Visible = true;
                    objBill = new BaseC.clsEMRBilling(sConString);
                    int iAdviserDoctor = 0;
                    iAdviserDoctor = objBill.GetPatientConsultingDoctor(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]));
                    Session["AdviserDoctorId"] = iAdviserDoctor;
                    BindAdviserDoctor();
                    ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myStr(iAdviserDoctor)));
                }
                else
                {
                    tdAdviserDoctor.Visible = false;
                }
                BindGrid();
                if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                {
                    dtOrderDate.Enabled = false;
                    ddlOrderMinutes.Enabled = false;
                }
                else
                {
                    dtOrderDate.Enabled = true;
                    ddlOrderMinutes.Enabled = true;
                }
                setisAllDoctorDisplayOnAddService();
                if (common.myStr(Request.QueryString["For"]).Equals("IVF") && common.myInt(Request.QueryString["IVFPackageId"]) > 0)
                {
                    PopulateIVFPackageService(common.myInt(Request.QueryString["IVFPackageId"]));
                    ddlService.Enabled = false;
                    cmdAddtoGrid_OnClick(sender, e);
                    cmdAddtoGrid.Enabled = false;
                }
                for (int i = 0; i < 60; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    }
                    else
                    {
                        RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    }
                }
                int iMinute = DateTime.Now.Minute;
                RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
                if (rcbItem != null)
                {
                    rcbItem.Selected = true;
                }
                if (common.myBool(Session["AllergiesAlert"]))
                {
                    imgAllergyAlert.Visible = true;
                    liAllergyAlert.Visible = true;
                }

                if (common.myBool(Session["MedicalAlert"]))
                {
                    imgMedicalAlert.Visible = true;
                    liMedicalAlert.Visible = true;
                }
                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {
                    chkApprovalRequired.Visible = false;
                    lnkCommonServices.Visible = false;
                }

                hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
                if (common.myBool(hdnIsPasswordRequired.Value))
                    hdnPasswordScreenType.Value = PasswordRequiredHelper.CheckPasswordScreenType(30, "SV", sConString);

                #region Care Plan service name populate by default
                if (common.myInt(Request.QueryString["CarePlan"]) == 1 && common.myLen(Session["ServiceOrderSaveMessage"]) == 0)
                {
                    string xmlSchema = common.myStr(Request.QueryString["ServiceIds"]).Trim();
                    StringReader sr = new StringReader(xmlSchema);
                    DataSet dsXml = new DataSet();
                    dsXml.ReadXml(sr);
                    addServiceFromOrderSet(dsXml.Tables[0]);
                    sr = null;
                    dsXml.Dispose();
                    xmlSchema = string.Empty;
                }
                #endregion

                if (common.myLen(Session["ServiceOrderSaveMessage"]) > 0)
                {
                    lblMessage.Text = common.myStr(Session["ServiceOrderSaveMessage"]);
                    Session["ServiceOrderSaveMessage"] = string.Empty;

                    ViewState["OrderId"] = Session["ServiceOrderSaveOrderId"];
                    Session["ServiceOrderSaveOrderId"] = string.Empty;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            objBill = null;
            Legend1.loadLegend("LabOthers", "'Stat', 'Urgent', 'POCRequest','outsrc','biohazard','AddOn'");
        }
    }

    private void gethospitalSetupvalues()
    {
        flagIsAllowToDisplayServicePrice = string.Empty;
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        flagIsAllowToDisplayServicePrice = common.myStr(objBill.getHospitalSetupValue("IsAllowToDisplayServicePice", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
        IsAllowBedSideChargeUncheck = common.myStr(objBill.getHospitalSetupValue("IsAllowBedSideChargeUncheck", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
        ViewState["IsWardServicesWithEntrySite"] = common.myStr(objBill.getHospitalSetupValue("IsWardServicesWithEntrySite", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
    }

    private void bindAssignToDoctor()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        StringBuilder strType = new StringBuilder();
        ArrayList coll = new ArrayList();

        try
        {
            ddlAssignToEmpId.ClearSelection();
            ddlAssignToEmpId.Items.Clear();

            coll.Add("LDIR");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("D");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("LD");
            strType.Append(common.setXmlTable(ref coll));

            int StationId = 1;
            if (common.myInt(hdnGlobleStationId.Value) > 0)
            {
                StationId = common.myInt(hdnGlobleStationId.Value);
            }

            ds = objMaster.getEmployeeData(common.myInt(Session["HospitalLocationID"]), StationId, 0, strType.ToString(), "", 0, common.myInt(Session["UserId"]), "", common.myInt(Session["FacilityId"]));

            ddlAssignToEmpId.SelectedIndex = -1;
            ddlAssignToEmpId.DataSource = ds.Tables[0].Copy();
            ddlAssignToEmpId.DataValueField = "EmployeeId";
            ddlAssignToEmpId.DataTextField = "EmployeeNameWithNo";
            ddlAssignToEmpId.DataBind();
            ddlAssignToEmpId.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlAssignToEmpId.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            objMaster = null;
            ds.Dispose();
            strType = null;
            coll = null;
        }
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        if (ddlService.SelectedValue != "")
        {
            bindAssignToDoctor();

            visible(common.myStr(hdnGlobleServiceType.Value));
        }
        if (common.myBool(hndEquipmentType.Value))
        {

            dtEquipmentType.Visible = true;

            dtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtFromDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";

            dtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";
            dtToDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]) + " HH:mm";

            BindToMinutes();
            BindFromMinutes();

            dtFromDate.SelectedDate = Convert.ToDateTime(dtOrderDate.SelectedDate);
            dtToDate.SelectedDate = Convert.ToDateTime(dtOrderDate.SelectedDate);
        }
        else
        {
            dtEquipmentType.Visible = false;
        }
    }

    protected void visible(string type)
    {
        try
        {
            if (type == "IS")  ///////IS [ Radiology Services ]
            {
                ddlAssignToEmpId.Enabled = true;
            }
            else if (type == "I") ////// I [ Lab Services ]
            {
                ddlAssignToEmpId.Enabled = true;
            }
            else  ////////////////////////// [ Others ]
            {
                if (common.myLen(type) > 0)
                {
                    ddlAssignToEmpId.Enabled = false;
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

    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(Session["RegistrationID"]), common.myStr(Session["EncounterId"]), common.myInt(Session["UserId"]), 0);
            //}
            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
                + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
                + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";
            RadWindow1.Height = 400;
            RadWindow1.Width = 1050;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void imgMedicalAlert_OnClick(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                   common.myStr(Session["RegistrationID"]), common.myStr(Session["EncounterId"]), common.myInt(Session["UserId"]), 0);

            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&FromEMR=1&EId=" + common.myStr(Session["EncounterId"])
               + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"])
               + "&PAG=" + common.myStr(Session["AgeGender"]) + "" + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&SepPat=Y";


            RadWindow1.Height = 400;
            RadWindow1.Width = 1050;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }


    public void setisAllDoctorDisplayOnAddService()
    {
        string setisAllDoctorDisplayOnAddService = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isAllDoctorDisplayOnAddService", sConString);
        ViewState["setisAllDoctorDisplayOnAddService"] = "Y";
        if (!setisAllDoctorDisplayOnAddService.Equals(""))
        {
            ViewState["setisAllDoctorDisplayOnAddService"] = setisAllDoctorDisplayOnAddService.ToUpper();
        }
    }
    protected void chkApprovalRequired_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkApprovalRequired.Checked)
        {
            chkIsReadBack.Visible = true;
            lblReadBackNote.Visible = true;
            txtIsReadBackNote.Visible = true;
        }
        else
        {
            chkIsReadBack.Visible = false;
            lblReadBackNote.Visible = false;
            txtIsReadBackNote.Visible = false;

            txtIsReadBackNote.Text = "";
            chkIsReadBack.Checked = false;
        }
    }
    private void BindAdviserDoctor()
    {
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        DataTable tbl = new DataTable();
        try
        {
            tbl = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                ddlAdvisingDoctor.DataSource = tbl;
                ddlAdvisingDoctor.DataTextField = "DoctorName";
                ddlAdvisingDoctor.DataValueField = "DoctorId";
                ddlAdvisingDoctor.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally { objlis = null; tbl = null; }
    }
    private void BindMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (common.myStr(i).Length.Equals(1))
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                }
                else
                {
                    ddlOrderMinutes.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void ddlOrderMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(common.myStr(dtOrderDate.SelectedDate.Value));
        sb.Remove(common.myStr(dtOrderDate.SelectedDate.Value).IndexOf(":") + 1, 2);
        sb.Insert(common.myStr(dtOrderDate.SelectedDate.Value).IndexOf(":") + 1, ddlOrderMinutes.Text);
        //dtOrderDate.SelectedDate = common.myDate(common.myStr(sb));
        dtOrderDate.SelectedDate = Convert.ToDateTime(common.myStr(sb));
        dtOrderDate_SelectedDateChanged(null, null);
    }
    protected void lnkViewLabOrders_OnClick(object sender, EventArgs e)
    {
        if (!common.myStr(ViewState["EncId"]).Equals("") && !common.myStr(ViewState["RegNo"]).Equals(""))
        {
            RadWindow1.NavigateUrl = "/EMRBilling/Popup/Servicedetails.aspx?Deptid=0&EncId=" + common.myStr(ViewState["EncId"]) + "&RegNo=" + common.myStr(ViewState["RegNo"]) + "&BillId=0&PType=WD";
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
    }
    protected void lnkSampleCollect_OnClick(object sender, EventArgs e)
    {
        if (!common.myStr(ViewState["EncId"]).Equals("") && !common.myStr(ViewState["RegNo"]).Equals(""))
        {
            RadWindow1.NavigateUrl = "/LIS/Phlebotomy/Phlebotomy.aspx?Deptid=0&EncId=" + common.myStr(ViewState["EncId"]) + "&RegNo=" + common.myStr(ViewState["RegNo"]) + "&BillId=0&PType=WD&IpNo=" + lblEncounterNo.Text;

            RadWindow1.Height = 520;
            RadWindow1.Width = 650;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
    }
    protected void samplecollect(object sender, EventArgs e)
    { }
    #region  PageFunctions
    protected void BindComboBox()
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        try
        {
            ddlDepartment.Items.Clear();
            string strDepartmentType = "";
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                strDepartmentType = "'I','P','HPP','O','OPP','IS','RF'";
            }
            else
            {
                strDepartmentType = "'I','P','HPP','O','OPP','IS'";
            }
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                ds = bMstr.GetHospitalDepartmentByFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), strDepartmentType, common.myInt(Session["EntrySite"]));
            }
            else
            {//yogesh 29/09/2022

                //yogesh 29/09/2022
                if (common.myInt(Session["EntrySite"]) > 0 && common.myStr(ViewState["IsWardServicesWithEntrySite"]).Equals("Y"))
                {
                    ds = bMstr.GetHospitalDepartmentByEntrySite(common.myInt(Session["HospitalLocationID"]), strDepartmentType, common.myInt(Session["EntrySite"]), common.myInt(Session["FacilityId"]));
                }
                else
                {
                    ds = bMstr.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), strDepartmentType);
                }   
                
                
                   
            }
            if (ds.Tables.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
            }

            
            ddlDepartment.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            bMstr = null;
        }
    }
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        BaseC.clsLabRequest objLabRequest = new BaseC.clsLabRequest(sConString);
        try
        {
            if (ddlDepartment.SelectedValue != "-1")
            {
                ddlSubDepartment.Items.Clear();
                ddlSubDepartment.Text = "";
                ds = objLabRequest.GetDepartmentSubMaster(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSubDepartment.DataSource = ds.Tables[0];
                        ddlSubDepartment.DataTextField = "SubName";
                        ddlSubDepartment.DataValueField = "SubDeptId";
                        ddlSubDepartment.DataBind();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            bMstr = null;
            objLabRequest = null;
        }
    }
    public DataTable BindServiceComboBox()
    {
        BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
        try
        {
            DataSet ds = new DataSet();
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), "", "", common.myInt(Session["FacilityId"]), "Y", common.myInt(Session["EntrySite"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));
            }
            else
            {
                ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), "", "", common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));
            }
            return ds.Tables[0];
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            return new DataTable();
        }
        finally
        {
            objCommonService = null;
        }
    }
    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            if (!e.Text.Equals(""))
            {
                if (e.Text.Trim().Length > 1)
                {
                    data = GetData(e.Text);

                    int itemOffset = e.NumberOfItems;
                    int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
                    e.EndOfItems = endOffset == data.Rows.Count;
                    ddlService.Items.Clear();
                    for (int i = itemOffset; i < endOffset; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)data.Rows[i]["ServiceName"];
                        item.Value = common.myStr(data.Rows[i]["ServiceId"]) + "##" + common.myStr(data.Rows[i]["ServiceType"]) + "##" + common.myStr(data.Rows[i]["IsOrderSet"]) + "##" + common.myStr(data.Rows[i]["IsLinkService"]);
                        item.Attributes.Add("ServiceName", common.myStr(data.Rows[i]["ServiceName"]));
                        item.Attributes.Add("RefServiceCode", common.myStr(data.Rows[i]["RefServiceCode"]));
                        item.Attributes["IsStatOrderAllowed"] = common.myStr(data.Rows[i]["IsStatOrderAllowed"]);
                        item.Attributes["isServiceRemarkMandatory"] = common.myStr(data.Rows[i]["isServiceRemarkMandatory"]);
                        item.Attributes["ServiceType"] = common.myStr(data.Rows[i]["ServiceType"]);
                        item.Attributes["StationId"] = data.Rows[i]["StationId"].ToString();
                        item.Attributes["EquipmentType"] = data.Rows[i]["EquipmentType"].ToString();
                        item.Attributes["ChargesPeriod"] = data.Rows[i]["ChargesPeriod"].ToString();
                        item.Attributes["GracePeriod"] = data.Rows[i]["GracePeriod"].ToString();
                        this.ddlService.Items.Add(item);
                        item.DataBind();
                    }
                    e.Message = GetStatusMessage(endOffset, data.Rows.Count);
                }
            }
            hdnIsServiceRemarkMandatory.Value = common.myStr(hdnIsServiceRemarkMandatory.Value);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally { data = null; }
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
        BaseC.RestFulAPI objCommonService = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        DataTable data = new DataTable();
        try
        {
            string ServiceName = text + "%";
            string strDepartmentType = "";
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                strDepartmentType = "'I','IS','P','HPP','C','O','OPP','RF'";//'CL'
            }
            else
            {

                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {
                    strDepartmentType = "'I','IS','P','HPP','C','O','OPP','EQ'"; //Venkateshwar
                }
                //palendra
                else if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), "IsVisitTypeChargeFromWardServiceOrder", sConString).Equals("Y"))
                {

                    strDepartmentType = "'I','IS','P','HPP','C','O','OPP','OS','VF','VS'"; //'OS' for Moolchand
                }
                else
                {
                    strDepartmentType = "'I','IS','P','HPP','C','O','OPP','OS'"; //'OS' for Moolchand
                }
            }
            if (common.myBool(Request.QueryString["IsExternal"]) && common.myBool(objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "SpecialBehaviorForDiagnosticOnlyChkOnReg", common.myInt(Session["FacilityId"]))))
            {
                strDepartmentType = "'I','IS'";
            }
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                if (common.myBool(Request.QueryString["IsPatientExpired"]))
                {
                    strDepartmentType = "'OS'";
                }

                ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), "N", common.myInt(Session["EntrySite"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));
            }
            else
            {
                //yogesh 29/09/2022
                if (common.myInt(Session["EntrySite"]) > 0 && common.myStr(ViewState["IsWardServicesWithEntrySite"]).Equals("Y"))
                {
                    ds = objCommonService.GetHospitalServicesByEntrySite(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["EntrySite"]));
                }
                else
                {
                    ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]));
                }


            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Columns.Contains("ApplicableFor"))
                {
                    DataView dvF = new DataView(ds.Tables[0]);
                    dvF.RowFilter = "ApplicableFor IN ('" + common.myStr(ViewState["OP_IP"]) + "','B')";
                    dvF = dvF.ToTable().DefaultView;
                    data = dvF.ToTable();
                    dvF.RowFilter = string.Empty;
                }
                else
                { data = ds.Tables[0]; }
            }
            return data;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            return data;
        }
        finally
        {
            objHosp = null;
            bMstr = null;
            objEMROrders = null;
            objCommonService = null;
            ds.Dispose();
            data.Dispose();
        }
    }
    private void FillDropDownList(CommandType CT, string strcommandname, RadComboBox RCBControl, string strDataTextField, string strDataValueField, bool blIsListItem, string strListItemText, Hashtable hshIn)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsObj = null;
        try
        {
            if (hshIn.Count > 0)
            {
                switch (CT)
                {
                    case CommandType.StoredProcedure:
                        dsObj = dl.FillDataSet(CommandType.StoredProcedure, strcommandname, hshIn);
                        break;
                    case CommandType.Text:
                        dsObj = dl.FillDataSet(CommandType.Text, strcommandname, hshIn);
                        break;
                }
            }
            else
            {
                switch (CT)
                {
                    case CommandType.StoredProcedure:
                        dsObj = dl.FillDataSet(CommandType.StoredProcedure, strcommandname);
                        break;
                    case CommandType.Text:
                        dsObj = dl.FillDataSet(CommandType.Text, strcommandname);
                        break;
                }
            }
            if (dsObj.Tables.Count > 0)
            {
                RCBControl.DataSource = dsObj;
                RCBControl.DataTextField = strDataTextField;
                RCBControl.DataValueField = strDataValueField;
                RCBControl.DataBind();
            }
            if (blIsListItem == true)
            {
                RCBControl.Items.Insert(0, new RadComboBoxItem(strListItemText, "0"));
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally { dl = null; dsObj.Dispose(); }
    }
    protected void BindGrid()
    {
        try
        {
            gvService.DataSource = CreateTable();
            gvService.DataBind();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
    }
    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 0;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("DetailId");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("OrderId");
        dt.Columns.Add("UnderPackage");
        dt.Columns.Add("PackageId");
        dt.Columns.Add("ServiceType");
        dt.Columns.Add("DoctorID");
        dt.Columns.Add("DoctorRequired");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("Units");
        dt.Columns.Add("Charge");
        dt.Columns.Add("ServiceAmount");
        dt.Columns.Add("DoctorAmount");
        dt.Columns.Add("ServiceDiscountPercentage");
        dt.Columns.Add("ServiceDiscountAmount");
        dt.Columns.Add("DoctorDiscountPercentage");
        dt.Columns.Add("DoctorDiscountAmount");
        dt.Columns.Add("TotalDiscount");
        dt.Columns.Add("AmountPayableByPatient");
        dt.Columns.Add("AmountPayableByPayer");
        dt.Columns.Add("IsPackageMain");
        dt.Columns.Add("IsPackageService");
        dt.Columns.Add("MainSurgeryId");
        dt.Columns.Add("IsSurgeryMain");
        dt.Columns.Add("IsSurgeryService");
        dt.Columns.Add("ServiceRemarks");
        dt.Columns.Add("ResourceId");
        dt.Columns.Add("ServiceStatus");
        dt.Columns.Add("IsExcluded");
        dt.Columns.Add("IsApprovalReq");
        dt.Columns.Add("IsPriceEditable");
        dt.Columns.Add("NetCharge");
        dt.Columns.Add("ChargePercentage");
        dt.Columns.Add("CopayAmt");
        dt.Columns.Add("CopayPerc");
        dt.Columns.Add("IsCoPayOnNet");
        dt.Columns.Add("DeductableAmount");
        dt.Columns.Add("Stat");
        dt.Columns.Add("Urgent");
        dt.Columns.Add("ServiceInstructions");
        dt.Columns.Add("InvestigationDate");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("POCRequest");
        dt.Columns.Add("IsBioHazard");
        dt.Columns.Add("IsReadBack");
        dt.Columns.Add("ReadBackNote");
        dt.Columns.Add("OutsourceInvestigation", typeof(bool));
        dt.Columns.Add("StationId", typeof(int));
        dt.Columns.Add("AssignToEmpId", typeof(int));
        dt.Columns.Add("IsAddOnTest");
        dt.Columns.Add("ServicelimitAmount");
        dt.Columns.Add("IsServiceApprovalRequired", typeof(bool));

        dt.Columns.Add("FromDate");
        dt.Columns.Add("ToDate");
        dt.Columns.Add("EquipmentType", typeof(bool));
        dt.Columns.Add("TariffId", typeof(int));
        dt.Columns.Add("IsERService", typeof(int));
        dt.Columns.Add("IsBedChargeService", typeof(int));
        DataRow dr = dt.NewRow();
        dr["ServiceName"] = " ";
        dr["DeductableAmount"] = 0;
        dt.Rows.Add(dr);
        ViewState["OPServicesInv_"] = dt;
        return dt;
    }
    #endregion
    protected void btnContinue_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(txtMiscellaneousRemarks.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type remarks", Page);
            return;
        }

        btnYes_OnClick(sender, e);
        divMiscellaneousRemarks.Visible = false;
    }
    protected void btnContinueCancel_OnClick(object sender, EventArgs e)
    {
        divMiscellaneousRemarks.Visible = false;
        txtMiscellaneousRemarks.Text = "";
    }
    protected void btnProceed_OnClick(object sender, EventArgs e)
    {
        ViewState["Yes"] = true;
        btnYes_OnClick(sender, e);
        divConfirmation.Visible = false;
        ViewState["Yes"] = null;
    }
    protected void btnProceedCancel_OnClick(object sender, EventArgs e)
    {
        divConfirmation.Visible = false;
    }

    protected void cmdAddtoGrid_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (common.myBool(hndEquipmentType.Value))
            {
                if (dtFromDate.SelectedDate == null)
                {
                    Alert.ShowAjaxMsg("Please select From date for equipment type " + common.myStr(ddlService.Text), Page);
                    dtFromDate.Focus();
                    return;
                }
                if (dtToDate.SelectedDate == null)
                {
                    Alert.ShowAjaxMsg("Please select To date for equipment type " + common.myStr(ddlService.Text), Page);
                    dtToDate.Focus();
                    return;
                }

                int diffFromDateToDate = DateTime.Compare(Convert.ToDateTime(dtFromDate.SelectedDate), Convert.ToDateTime(dtToDate.SelectedDate));
                int diffOrderDateToDate = DateTime.Compare(Convert.ToDateTime(dtToDate.SelectedDate), Convert.ToDateTime(dtOrderDate.SelectedDate));

                // checking 
                if (diffFromDateToDate > 0)
                {
                    Alert.ShowAjaxMsg("From Date should not be greater than To date", Page);
                    dtFromDate.Focus();
                    return;
                }

                if (diffOrderDateToDate > 0)
                {
                    Alert.ShowAjaxMsg("To Date should not be greater than Order date", Page);
                    dtToDate.Focus();
                    return;
                }


            }

            if (common.myBool(hdnIsServiceRemarkMandatory.Value) && common.myStr(txtMiscellaneousRemarks.Text).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Remarks / Rationale / Clinical Indication is Mandatory for : " + common.myStr(ddlService.Text), Page);
                txtMiscellaneousRemarks.Focus();
                return;
            }
            //   if (RadDateTimePicker1.SelectedDate != null)
            //   {
            //      if (DateTime.Compare(Convert.ToDateTime(RadDateTimePicker1.SelectedDate), DateTime.Now.Date) < 0)
            //      {
            //          Alert.ShowAjaxMsg("please select correct investigation date", Page);
            //          return;
            //       }
            //   }

            UpdateDataTable();
            if (common.myStr(ddlService.SelectedValue).Equals(""))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Service does not exist, Please select from list !";
                Alert.ShowAjaxMsg("Service does not exist, Please select from list !", Page.Page);
                return;
            }
            string[] stringSeparators_ShowDia = new string[] { "##" };
            string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);


            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            string MISCELLANEOUSSERVICEID = common.myStr(objBill.getHospitalSetupValue("MISCELLANEOUSSERVICEID", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));
            string MandatoryRequiredForMiscellaneousServices = common.myStr(objBill.getHospitalSetupValue("MandatoryRequiredForMiscellaneousServices", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"])));

            if (MISCELLANEOUSSERVICEID == serviceId[0] && MandatoryRequiredForMiscellaneousServices == "Y"
                && common.myStr(txtMiscellaneousRemarks.Text) == "")
            {
                //divMiscellaneousRemarks.Visible = true;
                divMiscellaneousRemarks.Visible = false;
                objBill = null;
                return;
            }

            if (common.myStr(ViewState["OP_IP"]).Equals("I"))
            {
                if (common.myInt(ViewState["EncId"]) > 0)
                {

                    if (common.myStr(chkStat.SelectedValue) == "STAT")
                    {
                        if (!common.myBool(hdnStatValueContainer.Value))
                        {
                            Alert.ShowAjaxMsg("This service can not be ordered as Stat", Page);
                            return;
                        }
                    }

                    DataSet datas = new DataSet();
                    Hashtable hshInput = new Hashtable();
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                    hshInput.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                    hshInput.Add("@ServiceId", common.myInt(serviceId[0]));
                    hshInput.Add("@EncounterId", common.myInt(ViewState["EncId"]));
                    hshInput.Add("@RegistrationId", common.myInt(ViewState["Regid"]));
                    datas = dl.FillDataSet(CommandType.Text, "Select SD.ServiceId, I.ServiceName,ISNULL(EM.FirstName,'') + ' '  + ISNULL(EM.MiddleName,'') + ' ' + ISNULL(EM.LASTNAME,'') AS EnteredBy, dbo.GetDateFormatUTC(s.EncodedDate,'DT', F.TimeZoneOffSetMinutes) OrderDate FROM ServiceOrderMain S With(NoLock)  INNER JOIN ServiceOrderDetail SD With(NoLock) ON S.Id = SD.OrderId  INNER JOIN ItemOfService I With(NoLock) ON SD.ServiceId = I.ServiceId  INNER JOIN FacilityMaster F With(NoLock) ON S.FacilityId = F.FacilityID  INNER JOIN Users US With(NoLock) ON S.EncodedBy=US.ID INNER JOIN Employee EM With(NoLock) ON EM.ID=US.EmpID WHERE ISNULL(S.EncounterId,'') =  @EncounterId  AND S.RegistrationId = @RegistrationId  AND CONVERT(VARCHAR,S.OrderDate,111) = CONVERT(VARCHAR,GETUTCDATE(),111)  AND S.HospitalLocationId = @HospitalLocationId  AND S.FacilityId = @FacilityId  AND SD.ServiceId = @ServiceId And S.ACTIVE = 1 AND SD.ACTIVE = 1 ", hshInput);
                    if (datas.Tables.Count > 0)
                    {
                        if (datas.Tables[0].Rows.Count > 0)
                        {
                            lblServiceName.Text = common.myStr(datas.Tables[0].Rows[0]["ServiceName"]);
                            lblEnteredBy.Text = common.myStr(datas.Tables[0].Rows[0]["EnteredBy"]);
                            lblEnteredOn.Text = common.myStr(datas.Tables[0].Rows[0]["OrderDate"]);
                            ViewState["DuplicateService"] = "1";
                            //btnYes_OnClick(sender, e);
                        }
                    }

                    btnYes_OnClick(sender, e);

                }
            }
            else
            {
                btnYes_OnClick(sender, e);
            }
            hdnStatValueContainer.Value = "0";
            // ClearControl();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
    }

    private void ClearControl()
    {
        ddlService.Text = "";
        ddlService.Items.Clear();
        ddlService.ClearSelection();
        ddlService.EmptyMessage = "";
        RadDateTimePicker1.SelectedDate = null;
        RadComboBox1.SelectedIndex = 0;
        //dtOrderDate.SelectedDate= System.DateTime.Now.Date;
        //ddlDepartment.SelectedIndex = 0;
        //ddlSubDepartment.SelectedIndex = 0;
        txtMiscellaneousRemarks.Text = "";
        //chkApprovalRequired.Checked = false;
        chkIsReadBack.Checked = false;
        Page.SetFocus(ddlService);
        ddlAssignToEmpId.SelectedIndex = 0;
    }
    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataSet dsNew = new DataSet();
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }
            }
            if (e.Row.RowType.Equals(DataControlRowType.Header) || e.Row.RowType.Equals(DataControlRowType.Footer) || e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (string.Equals(flagIsAllowToDisplayServicePrice, "Y"))// By Tripti
                {
                    BaseC.Security sec = new BaseC.Security(sConString);
                    if (!sec.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowServicePriceDisplay"))
                    {
                        e.Row.Cells[4].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //yogesh
                HiddenField hdnServiceId = (HiddenField)e.Row.FindControl("hdnServiceId");
                ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                HiddenField hdnIsPackageService = (HiddenField)e.Row.FindControl("hdnIsPackageService");
                HiddenField hdnDocReq = (HiddenField)e.Row.FindControl("hdnDocReq");
                Label lblDoctorID = (Label)e.Row.FindControl("lblDoctorID");
                RadComboBox ddlDoctor = (RadComboBox)e.Row.FindControl("ddlDoctor");
                HiddenField hdnServType = (HiddenField)e.Row.FindControl("hdnlblServType");
                HiddenField hdnStat = (HiddenField)e.Row.FindControl("hdnStat");
                HiddenField hdnUrgent = (HiddenField)e.Row.FindControl("hdnUrgent");
                HiddenField hdnisExcluded = (HiddenField)e.Row.FindControl("hdnisExcluded");
                HiddenField hdnPOCRequest = (HiddenField)e.Row.FindControl("hdnPOCRequest");

                TextBox txtUnits = (TextBox)e.Row.FindControl("txtUnits");
                TextBox txtServiceAmount = (TextBox)e.Row.FindControl("txtServiceAmount");
                TextBox txtDoctorAmount = (TextBox)e.Row.FindControl("txtDoctorAmount");
                TextBox txtDiscountPercent = (TextBox)e.Row.FindControl("txtDiscountPercent");
                TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");
                TextBox txtNetCharge = (TextBox)e.Row.FindControl("txtNetCharge");
                TextBox txtAmountPayableByPatient = (TextBox)e.Row.FindControl("txtAmountPayableByPatient");
                TextBox txtAmountPayableByPayer = (TextBox)e.Row.FindControl("txtAmountPayableByPayer");
                HiddenField hdnDeptId = (HiddenField)e.Row.FindControl("hdnDeptId");
                LinkButton lnkServiceName = (LinkButton)e.Row.FindControl("lnkServiceName");
                HiddenField hdnServiceInstructions = (HiddenField)e.Row.FindControl("hdnServiceInstructions");
                HiddenField hdnServiceRemarks = (HiddenField)e.Row.FindControl("hdnServiceRemarks");
                Literal lblServiceName = (Literal)e.Row.FindControl("lblServiceName");
                HiddenField hdnOutsourceInvestigation = (HiddenField)e.Row.FindControl("hdnOutsourceInvestigation");
                HiddenField hdnBiohazard = (HiddenField)e.Row.FindControl("hdnBiohazard");
                HiddenField hdnAddOnTest = (HiddenField)e.Row.FindControl("hdnAddOnTest");


                //HiddenField hdnFromDate = (HiddenField)e.Row.FindControl("hdnFromDate");
                //HiddenField hdnToDate = (HiddenField)e.Row.FindControl("hdnToDate");
                HiddenField hdngvEquipmentType = (HiddenField)e.Row.FindControl("hdngvEquipmentType");
                // ImageButton ibtnE = (ImageButton)e.Row.FindControl("ibtnE");

                if (common.myBool(hdngvEquipmentType.Value))
                {
                    //ibtnE.Enabled = false;
                    txtUnits.Enabled = false;
                }


                if (common.myStr(hdnServiceInstructions.Value).Equals(""))
                {
                    lnkServiceName.Visible = false;
                    lblServiceName.Visible = true;
                }
                else
                {
                    lnkServiceName.Attributes.Add("onclick", "javascript:return ShowServiceInstructions('" + hdnServiceInstructions.Value + "');");
                    lnkServiceName.Visible = true;
                    lblServiceName.Visible = false;
                }

                lblServiceRemarks.Text = hdnServiceRemarks.Value;
                HiddenField hdnIsPriceEditable = (HiddenField)e.Row.FindControl("hdnIsPriceEditable");
                txtUnits.Attributes.Add("onchange", "javascript:CalculateEditablePrice('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + txtUnits.ClientID + "','" + txtDiscountPercent.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetCharge.ClientID + "','" + txtAmountPayableByPatient.ClientID + "','" + txtAmountPayableByPayer.ClientID + "' );");
                txtServiceAmount.Attributes.Add("onchange", "javascript:CalculateEditablePrice('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + txtUnits.ClientID + "','" + txtDiscountPercent.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetCharge.ClientID + "','" + txtAmountPayableByPatient.ClientID + "','" + txtAmountPayableByPayer.ClientID + "' );");
                txtDoctorAmount.Attributes.Add("onchange", "javascript:CalculateEditablePrice('" + txtServiceAmount.ClientID + "','" + txtDoctorAmount.ClientID + "','" + txtUnits.ClientID + "','" + txtDiscountPercent.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetCharge.ClientID + "','" + txtAmountPayableByPatient.ClientID + "','" + txtAmountPayableByPayer.ClientID + "');");
                if (common.myInt(hdnIsPackageService.Value).Equals(1))
                {
                    ibtndaDelete.Visible = false;
                }
                //yogesh 13/06/2022


                //if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                //{
                if (hdnIsPriceEditable.Value == "True")
                {
                    txtServiceAmount.Enabled = true;
                }
                //}

                if (common.myBool(hdnStat.Value))
                {
                    dsNew = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Stat");
                    if (!dsNew.Equals(null))
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsNew.Tables[0].Rows[0]["StatusColor"]));
                        }
                    }
                }
                if (common.myBool(hdnUrgent.Value))
                {
                    dsNew = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Urgent");
                    if (!dsNew.Equals(null))
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsNew.Tables[0].Rows[0]["StatusColor"]));
                        }
                    }
                }
                if (common.myBool(hdnPOCRequest.Value))
                {
                    dsNew = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "POCRequest");
                    if (!dsNew.Equals(null))
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsNew.Tables[0].Rows[0]["StatusColor"]));
                        }
                    }
                }
                if (common.myBool(hdnBiohazard.Value))
                {
                    dsNew = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "biohazard");
                    if (!dsNew.Equals(null))
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[0].BackColor = System.Drawing.Color.FromName(common.myStr(dsNew.Tables[0].Rows[0]["StatusColor"]));
                        }
                    }
                }
                if (common.myBool(hdnAddOnTest.Value))
                {
                    dsNew = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "AddOn");
                    if (!dsNew.Equals(null))
                    {
                        if (dsNew.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[2].BackColor = System.Drawing.Color.FromName(common.myStr(dsNew.Tables[0].Rows[0]["StatusColor"]));
                        }
                    }
                }

                if ((common.myStr(hdnServType.Value).Trim().Equals("I")) || (common.myStr(hdnServType.Value).Trim().Equals("IS")))
                {
                    txtUnits.Enabled = false;
                }
                if ((hdnisExcluded.Value == "1") || (common.myBool(hdnisExcluded.Value) == true))
                {
                    e.Row.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BESColorCode"]));
                }
                if (common.myInt(hdnDocReq.Value).Equals(1) || common.myBool(hdnDocReq.Value))
                {
                    ddlDoctor.Visible = true;
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataTable dt = new DataTable();


                    if (common.myStr(ViewState["setisAllDoctorDisplayOnAddService"]).ToUpper().Equals("N"))
                    {
                        //yogesh
                        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                        ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0);
                        DataView dvF = new DataView(ds.Tables[0]);
                        dvF.RowFilter = "Type IN ('D','TM','SR','AN','OD','LD')";
                        dvF = dvF.ToTable().DefaultView;
                        if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                        {
                            dvF.RowFilter = "ProvidingService IN ('O','B')";
                        }
                        else if (common.myStr(ViewState["OP_IP"]).Equals("I"))
                        {
                            dvF.RowFilter = "ProvidingService IN ('I','B')";
                        }
                        dt = dvF.ToTable();
                    }//yogesh
                    else
                    {
                        //yogesh
                        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
                        ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]), 0, 0, common.myInt(hdnServiceId.Value));
                        DataView dvF = new DataView(ds.Tables[0]);
                        dvF.RowFilter = "Type IN ('D','TM','SR','AN','OD','LD')";
                        dvF = dvF.ToTable().DefaultView;
                        if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                        {
                            dvF.RowFilter = "ProvidingService IN ('O','B')";
                        }
                        else if (common.myStr(ViewState["OP_IP"]).Equals("I"))
                        {
                            dvF.RowFilter = "ProvidingService IN ('I','B')";
                        }
                        dt = dvF.ToTable();
                        // ViewState["EmpClassi"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ddlDoctor.Items.Clear();
                        ddlDoctor.DataSource = dt;
                        ddlDoctor.DataValueField = "EmployeeId";
                        ddlDoctor.DataTextField = "EmployeeName";
                        ddlDoctor.DataBind();
                        ddlDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                        ddlDoctor.SelectedIndex = 0;
                    }
                    if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                    {
                        if (hdnIsPriceEditable.Value == "True")
                        {
                            txtDoctorAmount.Enabled = true;
                        }
                    }
                    ddlDoctor.SelectedValue = common.myStr(DataBinder.Eval(e.Row.DataItem, "DoctorId")); ;
                }
                else
                {
                    ddlDoctor.Visible = false;
                }

                lblDoctorID.Visible = false;

                if (common.myBool(hdnOutsourceInvestigation.Value))
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                    e.Row.Cells[1].ToolTip = "Outsource Investigation";
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblInfoTotal = (Label)e.Row.FindControl("lblInfoTotal");
                Label lblTotDiscountAmt = (Label)e.Row.FindControl("lblTotDiscountAmt");
                Label lblAmountPayableByPatient = (Label)e.Row.FindControl("lblAmountPayableByPatient");
                Label lblAmountPayableByPayer = (Label)e.Row.FindControl("lblAmountPayableByPayer");
                int rowcnt = 0;
                foreach (GridViewRow gvrow in gvService.Rows)
                {
                    HiddenField hdnlblServType = (HiddenField)gvrow.FindControl("hdnlblServType");
                    HiddenField hdispackagemain = (HiddenField)gvrow.FindControl("hdispackagemain");
                    HiddenField hdUnderPackage = (HiddenField)gvrow.FindControl("hdUnderPackage");
                    rowcnt = rowcnt + 1;
                    if ((common.myStr(hdnlblServType.Value).Equals("OPP")))
                    {
                        if (common.myStr(hdispackagemain.Value).Equals("1"))
                        {
                            lblInfoTotal.Text = "Total: " + common.myStr(rowcnt) + " Service(s) ";
                            lblTotDiscountAmt.Text = common.FormatNumber(common.myStr(common.myDbl(lblTotDiscountAmt.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtDiscountAmt")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                            lblAmountPayableByPayer.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPayer.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPayer")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                            lblAmountPayableByPatient.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPatient.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPatient")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        }
                    }
                    else
                    {
                        if (common.myStr(hdUnderPackage.Value).Equals("0"))
                        {
                            lblInfoTotal.Text = "Total: " + common.myStr(rowcnt) + " Service(s) ";
                            lblTotDiscountAmt.Text = common.FormatNumber(common.myStr(common.myDbl(lblTotDiscountAmt.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtDiscountAmt")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                            lblAmountPayableByPayer.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPayer.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPayer")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                            lblAmountPayableByPatient.Text = common.FormatNumber(common.myStr(common.myDbl(lblAmountPayableByPatient.Text) + common.myDbl(((TextBox)gvrow.FindControl("txtAmountPayableByPatient")).Text)), common.myInt(Session["HospitalLocationID"]), sConString, common.myInt(Session["FacilityId"]));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
    }
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                int intId = common.myInt(e.CommandArgument);
                if (intId != 0)
                {
                    UpdateDataTable();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    HiddenField hdnIsPackageMain = (HiddenField)row.FindControl("hdnIsPackageMain");
                    HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");
                    DataTable dt = new DataTable();
                    dt = (DataTable)ViewState["OPServicesInv_"];
                    if (!dt.Columns.Contains("TariffId"))
                    {
                        dt.Columns.Add("TariffId", typeof(int));
                    }
                    if (common.myInt(hdnIsPackageMain.Value).Equals(1))
                    {
                        List<DataRow> rowsToRemove = new List<DataRow>();
                        foreach (GridViewRow gvrow in gvService.Rows)
                        {
                            HiddenField hdnIsPackageService = (HiddenField)gvrow.FindControl("hdnIsPackageService");
                            HiddenField hdnPackageId = (HiddenField)gvrow.FindControl("hdnPackageId");
                            if (common.myInt(hdnIsPackageService.Value).Equals(1))
                            {
                                if (common.myStr(hdnServiceId.Value).Equals(common.myStr(hdnPackageId.Value)))
                                {
                                    rowsToRemove.Add(dt.Rows[gvrow.RowIndex]);
                                }
                            }
                        }
                        rowsToRemove.Add(dt.Rows[row.RowIndex]);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (var dr in rowsToRemove)
                            {
                                dt.Rows.Remove(dr);
                            }
                        }
                    }
                    else if (common.myInt(hdnIsPackageMain.Value).Equals(0))
                    {
                        dt.Rows.RemoveAt(row.RowIndex);
                    }
                    dt.AcceptChanges();

                    if (dt != null)
                    {
                        if (dt.Columns.Count > 0)
                        {
                            if (!dt.Columns.Contains("FromDate"))
                            {
                                dt.Columns.Add("FromDate", typeof(string));
                            }
                            if (!dt.Columns.Contains("ToDate"))
                            {
                                dt.Columns.Add("ToDate", typeof(string));
                            }
                            if (!dt.Columns.Contains("EquipmentType"))
                            {
                                dt.Columns.Add("EquipmentType", typeof(bool));
                            }
                        }
                    }
                    ViewState["OPServices"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        gvService.DataSource = dt;
                    }
                    else
                    {
                        gvService.DataSource = CreateTable();
                    }
                    gvService.DataBind();
                    dtFromDate.SelectedDate = null;
                    dtFromMinutes.SelectedIndex = 0;
                    dtToDate.SelectedDate = null;
                    dtToMinutes.SelectedIndex = 0;
                    dtEquipmentType.Visible = false;
                }
            }
            if (e.CommandName == "Edit")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnBiohazard = (HiddenField)row.FindControl("hndBiohazard");
                chkIsBioHazard.Checked = common.myBool(hdnBiohazard.Value);
                string vendid = Convert.ToString(e.CommandArgument.ToString());
                lblMessage.Text = vendid;
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataTable dtData = null;
                int intId = common.myInt(e.CommandArgument);
                if (intId != 0)
                {
                    dtData = (DataTable)ViewState["OPServicesInv_"];
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
    }
    //Done by ujjwal 16 Jan 2016 for user authentication before saving start
    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
        //added by bhakti    
        try
        {
            if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(ViewState["OP_IP"]).Equals("I") && common.myStr(Session["PaymentType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                return;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (common.myStr(ViewState["OP_IP"]).Equals("I") && common.myBool(objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "PasswordAuthenticationRequiredOnAddService", common.myInt(Session["FacilityId"]))))
        {
            if (common.myBool(hdnIsPasswordRequired.Value))
            {
                IsValidPassword();
            }
            else
            {
                hdnIsPasswordRequired.Value = "0";
                SaveRecords();
            }
        }
        else
        {
            hdnIsPasswordRequired.Value = "0";
            SaveRecords();
        }
    }

    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";

        if (hdnPasswordScreenType.Value.Equals("U"))
            RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        else
            RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerV1.aspx?UseFor=OPIP";


        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnIsValidPassword_OnClick(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (!common.myBool(hdnIsValidPassword.Value))
        {
            lblMessage.Text = "Invalid Username/Password !";
            return;
        }
        if (common.myInt(hdnIsValidPassword.Value).Equals(1))
        {
            lblMessage.Text = "";
            SaveRecords();
        }
    }
    public void SaveRecords()
    {
        try
        {
            Session["ServiceOrderSaveMessage"] = string.Empty;
            Session["ServiceOrderSaveOrderId"] = string.Empty;

            if (ViewState["OPServicesInv_"] == null)
            {
                return;
            }
            if (common.myStr(ViewState["OP_IP"]).Equals("I"))
            {
                BaseC.Security sec = new BaseC.Security(sConString);
                if (sec.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedToAddIPService") == false)
                {
                    Alert.ShowAjaxMsg("You are not authorized for add services! Contact your administrator.", Page.Page);
                    return;
                }
                sec = null;
            }

            BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (dt != null && dt.Rows.Count > 0 && common.myInt(dt.Rows[0]["ServiceId"]).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please select service !";
                Alert.ShowAjaxMsg("Please select service !", Page);
                return;
            }
            if (ddlAdvisingDoctor.SelectedValue.Equals("") && !common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                Alert.ShowAjaxMsg("Please select Advising Doctor", Page);
                return;
            }
            /*if (common.myStr(txtMiscellaneousRemarks.Text) == "")
            {
                Alert.ShowAjaxMsg("Please type remarks", Page);
                return;
            }*/

            foreach (GridViewRow grow in gvService.Rows)
            {
                HiddenField hdnServiceId = (HiddenField)grow.FindControl("hdnServiceId");
                TextBox txtUnits = (TextBox)grow.FindControl("txtUnits");
                RadComboBox ddlDoctor = (RadComboBox)grow.FindControl("ddlDoctor");
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "ServiceId=" + common.myInt(hdnServiceId.Value);
                if (dt.DefaultView.Count > 0)
                {
                    dt.DefaultView[0]["Units"] = common.myStr(common.myDec(txtUnits.Text));
                    dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                    dt.AcceptChanges();
                    dt.DefaultView.RowFilter = "";
                    dt.AcceptChanges();
                }
            }
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                updateAmount();
                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["DoctorRequired"]).Equals(1) || common.myBool(dr["DoctorRequired"]) == true)
                    {
                        if (common.myInt(dr["DoctorID"]).Equals(0))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }
                    if (common.myStr(dr["ServiceType"]).ToUpper().Equals("CL"))
                    {
                        if (common.myInt(dr["DoctorID"]).Equals(0))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }
                }
                DataSet xmlDS = new DataSet();
                xmlDS.Tables.Add(dt.Copy());
                xmlDS.Tables[0].DefaultView.RowFilter = "ServiceId > 0";
                if (xmlDS.Tables[0].DefaultView.Count > 0)
                {
                    xmlDS.Tables[0].Rows[0]["ResourceId"] = " ";
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    xmlDS.WriteXml(writer);
                    //put schema in string
                    string xmlSchema = writer.ToString();
                    hdnXmlString.Value = xmlSchema;
                    //  Session["Serviestringxml"] = xmlSchema;
                    ViewState["OPServicesInv_"] = null;
                    BindGrid();
                }
                if (!common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                }
                return;
            }
            else
            {
                Hashtable hshInput = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                StringBuilder strXML = new StringBuilder();
                ArrayList coll = new ArrayList();
                dt = UpdateDataTable();
                dt = applyCoPayment(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (common.myInt(dr["DoctorRequired"]).Equals(1) || common.myBool(dr["DoctorRequired"]) == true)
                    {
                        if (common.myInt(dr["DoctorID"]).Equals(0))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please select doctor !";
                            return;
                        }
                    }
                    coll.Add(common.myInt(dr["ServiceId"])); //ServiceId INT   1
                    if (common.myBool(dr["EquipmentType"]))
                    {
                        coll.Add(Convert.ToDateTime(dr["FromDate"]).ToString("yyyy-MM-dd HH:mm:ss")); //VisitDate SMALLDATETIME 2   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                    }
                    else
                    {
                        coll.Add(DBNull.Value); //VisitDate SMALLDATETIME 2   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                    }
                    // coll.Add(DBNull.Value); //VisitDate SMALLDATETIME 2   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                    coll.Add(common.myInt(dr["Units"])); //Units SMALLINT  3
                    coll.Add(common.myInt(dr["DoctorID"])); //DoctorId INT    4 
                    coll.Add(common.myDec(dr["ServiceAmount"])); //ServiceAmount MONEY 5
                    coll.Add(common.myDec(dr["DoctorAmount"])); //DoctorAmount MONEY  6  
                    coll.Add(common.myDec(dr["ServiceDiscountAmount"])); //ServiceDiscountAmount MONEY 7
                    coll.Add(common.myDec(dr["DoctorDiscountAmount"])); //DoctorDiscountAmount MONEY  8
                    coll.Add(common.myDec(dr["AmountPayableByPatient"])); //AmountPayableByPatient MONEY    9
                    coll.Add(common.myDec(dr["AmountPayableByPayer"])); //AmountPayableByPayer MONEY  10
                    coll.Add(common.myDec(dr["ServiceDiscountPercentage"])); //ServiceDiscountPer MONEY    11
                    coll.Add(common.myDec(dr["DoctorDiscountPercentage"])); //DoctorDiscountPer MONEY 12
                    coll.Add(common.myInt(dr["PackageId"])); //PackageId INT   13  
                    coll.Add(common.myInt(dr["OrderId"])); //OrderId INT 14
                    coll.Add(common.myInt(dr["UnderPackage"])); //UnderPackage BIT    15            
                    coll.Add(DBNull.Value); //ICDID VARCHAR(100)  16
                    coll.Add(DBNull.Value); //ResourceID INT  17
                    coll.Add(DBNull.Value); //SurgeryAmount MONEY 18
                    coll.Add(DBNull.Value); //ProviderPercent MONEY   19
                    coll.Add(common.myInt(dr["SNo"])); //SeQNo INT   20
                    coll.Add(common.myStr(dr["ServiceRemarks"]));//Serviceremarks VARCHAR(250) NULL    21
                    coll.Add(0);//DetailId INT    22 - 0 in case of new order
                    coll.Add(0);//23//Er Order
                    coll.Add(0);//24//pharmacyOrder
                    coll.Add(common.myDec(dr["CopayAmt"]));//CoPayAmt MONEY  25
                    coll.Add(common.myDec(dr["DeductableAmount"]));//DeductableAmount MONEY  26
                    coll.Add(DBNull.Value);//ApprovalCode VARCHAR(50)    27
                    coll.Add(common.myStr(common.myInt(Session["FacilityId"])));//FacilityId SMALLINT 28
                    coll.Add(common.myStr(dr["Stat"]));//Stat or routine order BIT    29
                    coll.Add(common.myBool(dr["IsExcluded"])); //IsExcluded BIT  30
                    coll.Add(common.myStr(dr["InvestigationDate"]));//TestDateTime SMALLDATETIME  31
                    coll.Add(false);//FreeTest bit    32
                    coll.Add(DBNull.Value);//CPOERemark VARCHAR(300) 33
                    coll.Add(DBNull.Value);//34
                    coll.Add(DBNull.Value);//35
                    coll.Add(0);//IsDoneByAsstSurgeon BIT 36
                    coll.Add(0);//isNonDiscService Bit    37
                    coll.Add(DBNull.Value);//IsPriceEditableFromEMR BIT  38
                    coll.Add(dr["Urgent"]);//Urgent BIT  39
                    coll.Add(dr["POCRequest"]);//POCRequest BIT  40
                    coll.Add(common.myDec(dr["CopayPerc"]));//CoPayPerc 41
                    coll.Add(DBNull.Value);//ServiceDurationId INT   42
                    coll.Add(DBNull.Value);// 43 SurgeryId
                    coll.Add(common.myStr(dr["IsBioHazard"]));// Biohaqard 44
                    coll.Add(DBNull.Value);// 45 SurgeryComponentId
                    coll.Add(common.myInt(dr["AssignToEmpId"]));//AssignToEmpId 46
                    coll.Add(common.myStr(dr["IsAddOnTest"]));// AddOnTest 47
                    coll.Add(common.myBool(dr["IsServiceApprovalRequired"]));//48
                    if (common.myBool(dr["EquipmentType"]))
                    {
                        coll.Add(Convert.ToDateTime(dr["ToDate"]).ToString("yyyy-MM-dd HH:mm:ss")); //ToDate 49
                    }
                    else
                    {
                        coll.Add(DBNull.Value); //ToDate 49
                    }

                    coll.Add(common.myInt(dr["TariffId"]));//50 DefaultTariffId INT
                    coll.Add(common.myInt(dr["IsBedChargeService"]));//51 //51 IsBedChargeService INT INT

                    //  coll.Add(common.myBool(hndEquipmentType.Value));//50

                    //coll.Add(common.myDec(dr["ServicelimitAmount"]));//48


                    //                  ,POCRequest--40
                    // 			,CoPayPerc--41
                    // 			,ServiceDurationId--42
                    // 			,SurgeryId--43
                    // 			,isBioHazard--44
                    // 			,SurgeryComponentId--45
                    //	,AssignToEmpId--46
                    //	,IsAddOnTest--47,
                    //,IsServiceApprovalRequired--48

                    strXML.Append(common.setXmlTable(ref coll));
                }
                if (strXML.ToString().Trim().Length > 1)
                {
                    string sChargeCalculationRequired = "N";
                    string stype = "O" + common.myStr(ViewState["OP_IP"]).ToUpper();
                    bool isGenerateAdvance = false;
                    #region isShowEmergency
                    int IsEmergency = common.myStr(ViewState["OP_IP"]) == "E" ? 1 : 0;
                    IsEmergency = (IsEmergency.Equals(0) && chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;

                    #endregion
                    if (common.myStr(ViewState["OP_IP"]).Equals("I"))
                    { isGenerateAdvance = chkIsGenerateAdvance.Checked; }

                    // Session["check"] = 0;
                    //if (common.myInt(Session["check"]).Equals(0))
                    //{
                    //    Session["check"] = 1;

                    bool ApprovalRequired = chkApprovalRequired.Checked;

                    Hashtable hshOut = BaseBill.saveOrders(
                        common.myInt(Session["HospitalLocationID"]),
                        common.myInt(Session["FacilityId"]),
                        common.myInt(ViewState["Regid"]),
                        common.myInt(ViewState["EncId"]),
                        strXML.ToString(), txtMiscellaneousRemarks.Text,
                        (common.myBool(hdnIsPasswordRequired.Value) && hdnPasswordScreenType.Value.Equals("U") ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserID"])),
                        common.myInt(ddlAdvisingDoctor.SelectedValue),
                        common.myInt(hdnCompanyId.Value),
                        stype,
                        common.myStr(ViewState["PayerType"]),
                        common.myStr(ViewState["OP_IP"]),
                        common.myInt(hdnInsuranceId.Value),
                        common.myInt(hdnCardId.Value),
                        Convert.ToDateTime(dtOrderDate.SelectedDate), sChargeCalculationRequired,
                        common.myInt(Session["EntrySite"]), IsEmergency, isGenerateAdvance, ApprovalRequired,
                        common.myBool(chkIsReadBack.Checked), common.myStr(txtIsReadBackNote.Text));

                    if (common.myStr(hshOut["chvErrorStatus"]).Length.Equals(0) || common.myStr(hshOut["chvErrorStatus"]).Equals("Record Saved...") || common.myStr(hshOut["intOrderId"]).Length > 0)
                    {
                        if (common.myInt(hshOut["intNEncounterID"]) > 0)
                        {
                            Session["EncounterID"] = common.myInt(hshOut["intNEncounterID"]);
                        }
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Order Saved Successfully. Order No: " + common.myStr(hshOut["intOrderNo"]);
                        string msg = lblMessage.Text;

                        ViewState["OPServicesInv_"] = null;
                        ViewState["OrderId"] = common.myStr(hshOut["intOrderId"]);
                        BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
                        if (common.myBool(objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "RetainAddServicePageAfterSave", common.myInt(Session["FacilityId"]))))
                        {
                            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), "IsAllowAutoDepartmentReceiving", sConString).Equals("Y"))
                            {
                                ibtnSave.Visible = false;
                                RadWindow1.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=PODSave&encounterId=" + common.myStr(ViewState["EncId"]) + "&RegistrationId=" + common.myStr(ViewState["Regid"]) + "&OrderId=" + common.myStr(ViewState["OrderId"]);
                                RadWindow1.Height = 600;
                                RadWindow1.Width = 900;
                                RadWindow1.Top = 40;
                                RadWindow1.Left = 100;
                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                RadWindow1.Modal = true;
                                RadWindow1.VisibleStatusbar = false;
                            }
                            else
                            {
                                ibtnSave.Visible = false;
                                btnorderprint.Visible = true;

                                Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
                                Session["ServiceOrderSaveMessage"] = msg;
                                Session["ServiceOrderSaveOrderId"] = ViewState["OrderId"];
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                        }
                    }
                    else
                    {
                        Session["check"] = 0;

                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        //lblMessage.Text = "There is some error while saving order." + common.myStr(hshOut["chvErrorStatus"]);
                        lblMessage.Text = common.myStr(hshOut["chvErrorStatus"]);
                    }
                }
            }
            //}
            txtMiscellaneousRemarks.Text = "";
            BindGrid();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    //Done by ujjwal 16 Jan 2016 for user authentication before saving end
    protected void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (RegistrationNo > 0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegId = 0;
                int EncounterId = 0;
                int EncodedBy = common.myInt(Session["UserId"]);
                lblregNo.Text = RegistrationNo.ToString();
                if (common.myStr(ViewState["OP_IP"]).Equals("O"))
                {
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
                    lblInfoEncNo.Visible = false;
                    lblInfoAdmissionDt.Visible = false;
                    lblEncounterNo.Visible = false;
                    lblAdmissionDate.Visible = false;
                }
                else
                {
                    BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
                    //ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
                    ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, common.myInt(ViewState["Regid"]), RegistrationNo, EncodedBy, common.myInt(ViewState["EncId"]), "");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                        lblMessage.Text = "";
                        if (common.myStr(ViewState["From"]).Equals("BILL"))
                        {
                            hdnCompanyId.Value = common.myStr(ViewState["CompanyId"]);
                            hdnInsuranceId.Value = common.myStr(ViewState["InsuranceId"]);
                            hdnCardId.Value = common.myStr(ViewState["CardId"]);
                        }
                        else
                        {
                            hdnCompanyId.Value = common.myStr(dr["PayorId"]);
                            hdnInsuranceId.Value = common.myStr(dr["SponsorId"]);
                            hdnCardId.Value = common.myStr(dr["InsuranceCardId"]);
                        }
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Patient not found !";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            bC = null;
            ds.Dispose();
        }
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        //if (common.myBool(ViewState["IsAllowToAddBlockedService"]) == false)
        //{
        //    Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
        //    return;
        //}

        DataTable dt = new DataTable();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            int maxId = 0;
            string[] stringSeparators_ShowDia = new string[] { "##" };
            string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);

            if (chkIsBioHazard.Checked && !serviceId[1].ToUpper().Equals("I") && !serviceId[1].ToUpper().Equals("IS"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Biohazard is not applicable for this service";
                chkIsBioHazard.Checked = false;
            }

            dt = (DataTable)ViewState["OPServicesInv_"];
            if (common.myInt(ViewState["EditIndx"]) > -1 && ViewState["EditIndx"] != null)
            {
                dt.Rows.RemoveAt(common.myInt(ViewState["EditIndx"]));
            }
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
                //Check duplicate service------------------------------------------------------------------
                DataView dvdup = new DataView();
                dvdup = dt.Copy().DefaultView;
                dvdup.RowFilter = " ServiceId = " + common.myStr(serviceId[0]);
                if (dvdup.ToTable().Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service already exist !";
                    ddlService.Text = "";
                    ddlService.ClearSelection();
                    ddlService.EmptyMessage = "";
                    Page.SetFocus(ddlService);
                    return;
                }
            }
            if (common.myStr(serviceId[1]).Equals("OPP"))
            {
                ds = objBilling.getPackageServiceDetails(common.myInt(serviceId[0]), common.myInt(Session["HospitalLocationID"]),
                    common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value),
                    common.myInt(hdnInsuranceId.Value), common.myInt(hdnCardId.Value),
                    common.myStr(ViewState["OP_IP"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!ds.Tables[0].Columns.Contains("DeductableAmount"))
                        {
                            ds.Tables[0].Columns.Add("DeductableAmount", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("IsCoPayOnNet"))
                        {
                            ds.Tables[0].Columns.Add("IsCoPayOnNet", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("CopayPerc"))
                        {
                            ds.Tables[0].Columns.Add("CopayPerc", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("TariffId"))
                        {
                            ds.Tables[0].Columns.Add("TariffId", typeof(int));
                        }
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["DeductableAmount"] = 0;
                            dr["IsCoPayOnNet"] = 0;
                            dr["CopayPerc"] = 0;
                            dr["ServiceRemarks"] = common.myStr(txtMiscellaneousRemarks.Text);
                            dt.ImportRow(dr);
                        }

                        if (dt != null)
                        {
                            if (dt.Columns.Count > 0)
                            {
                                if (!dt.Columns.Contains("FromDate"))
                                {
                                    dt.Columns.Add("FromDate", typeof(string));
                                }
                                if (!dt.Columns.Contains("ToDate"))
                                {
                                    dt.Columns.Add("ToDate", typeof(string));
                                }
                                if (!dt.Columns.Contains("EquipmentType"))
                                {
                                    dt.Columns.Add("EquipmentType", typeof(bool));
                                }
                            }
                        }

                        ViewState["OPServicesInv_"] = dt;
                        gvService.DataSource = dt;
                        gvService.DataBind();
                        ddlService.Text = "";
                        ddlService.ClearSelection();
                        ddlService.EmptyMessage = "";
                        Page.SetFocus(ddlService);
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Package Amount !";
                }
            }
            else if (common.myBool(serviceId[2]) && !common.myBool(serviceId[3]))
            {
                BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
                DataTable dt1 = objEMROrders.GetOrderSets(common.myInt(serviceId[0]));
                addServiceFromOrderSet(dt1);
            }
            else
            {

                int iEMergencyCharge = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;

                #region isShowEmergency
                if (isShowEmergencyCheckBoxForEmergencyCharge == "Y")
                {
                    iEMergencyCharge = chkIsEmergency.Checked ? 1 : 0;
                }
                #endregion
                ViewState["ServiceId"] = common.myInt(serviceId[0]);
                ViewState["ServiceName"] = common.myStr(ddlService.Text);

                //change Palendra
                int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;//by nishu
                //change Palendra
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                           common.myInt(Session["FacilityId"]),
                                           common.myInt(hdnCompanyId.Value),
                                           common.myInt(hdnInsuranceId.Value),
                                           common.myInt(hdnCardId.Value),
                                           common.myStr(ViewState["OP_IP"]),
                                           common.myInt(serviceId[0]),
                                           common.myInt(ViewState["Regid"]),
                                           common.myInt(ViewState["EncId"]), 0, 0, iEMergencyCharge, Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);


                if (common.myBool(hshServiceDetail["IsBlocked"]) && common.myBool(ViewState["Yes"]) == false)
                {
                    if (common.myBool(ViewState["IsAllowToAddBlockedService"]) == false)
                    {
                        Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                        return;
                    }
                    divConfirmation.Visible = true;
                    return;
                }
                //if ((common.myStr(ViewState["OP_IP"]).Equals("I")) && (common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                if ((common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                {
                    divExcludedService.Visible = true;
                    lblExcludedServiceName.Text = common.myStr(ViewState["ServiceName"]);
                }

                else if (common.myBool(common.myStr(hshServiceDetail["IsServiceApprovalRequired"])))
                {
                    divServicelimit.Visible = true;
                    lblSName.Text = common.myStr(ViewState["ServiceName"]);
                    lblServcieLimit.Text = "Selected service is not under the limit . (" + common.myDec(common.myStr(hshServiceDetail["ServicelimitAmount"])).ToString("N2") + ")";
                }
                else if (common.myInt(ViewState["DuplicateService"]).Equals(1))
                {
                    dvConfirmPrintingOptions.Visible = true;
                }
                else
                {
                    addService();
                    dvConfirmPrintingOptions.Visible = false;
                    divExcludedService.Visible = false;
                    divServicelimit.Visible = false;
                }

                if (!common.myBool(serviceId[2]) && common.myBool(serviceId[3]))
                {
                    addServiceFromLinkService(common.myInt(serviceId[0]), common.myDbl(hshServiceDetail["NChr"]));
                }
            }
            //Awadhesh
            // if (common.myStr(IsAllowBedSideChargeUncheck).Equals("Y"))
            // {
            //    chkisbedsidecharges.Checked = false;
            //}
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objBilling = null;
            BaseBill = null;
            hshServiceDetail = null;
            ds.Dispose();
        }
    }
    public void addServiceFromOrderSet(DataTable dtOrderSet)
    {
        DataTable dt = new DataTable();
        try
        {
            if (ViewState["OPServicesInv_"] != null)
                dt = (DataTable)ViewState["OPServicesInv_"];

            if (!dt.Columns.Contains("TariffId"))
            {
                dt.Columns.Add("TariffId", typeof(int));
            }
            int maxId = 0;
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
            }

            foreach (DataRow dtr in dtOrderSet.Rows)
            {
                DataRow dr = dt.NewRow();
                Hashtable hshServiceDetail = new Hashtable();
                BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);

                int iEMergencyCharge = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                #region isShowEmergency
                if (isShowEmergencyCheckBoxForEmergencyCharge == "Y")
                {
                    iEMergencyCharge = chkIsEmergency.Checked ? 1 : 0;
                }
                #endregion

                //change Palendra
                int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;//by nishu
                //change Palendra
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                                          common.myInt(Session["FacilityId"]),
                                                          common.myInt(hdnCompanyId.Value),
                                                          common.myInt(hdnInsuranceId.Value),
                                                          common.myInt(hdnCardId.Value),
                                                          common.myStr(ViewState["OP_IP"]),
                                                          common.myInt(dtr["ServiceId"]),
                                                          common.myInt(ViewState["Regid"]),
                                                          common.myInt(ViewState["EncId"]), 0, 0,
                                                          iEMergencyCharge, Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);

                dr["Sno"] = maxId + 1;
                dr["ServiceId"] = common.myInt(dtr["ServiceId"]);
                dr["UnderPackage"] = common.myInt(0);
                dr["PackageId"] = common.myInt(0);
                dr["DoctorID"] = common.myInt(0);
                if (common.myStr(hshServiceDetail["ServiceType"]).ToUpper().Equals("CL"))
                { dr["DoctorRequired"] = "True"; }
                else
                { dr["DoctorRequired"] = common.myStr(hshServiceDetail["DoctorRequired"]); }
                dr["DepartmentId"] = common.myInt(hshServiceDetail["DepartmentId"]);
                dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
                dr["ServiceName"] = common.myStr(dtr["ServiceName"]);
                dr["Units"] = common.myInt(1);
                dr["Charge"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceAmount"] = common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["DoctorAmount"] = common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["ServiceDiscountAmount"] = common.myDec(hshServiceDetail["DiscountNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                dr["DoctorDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["DoctorDiscountAmount"] = common.myDec(hshServiceDetail["DiscountDNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["IsPackageMain"] = common.myInt(0);
                dr["IsPackageService"] = common.myInt(0);
                dr["ResourceId"] = "  ";
                dr["MainSurgeryId"] = common.myInt(0);
                dr["IsSurgeryMain"] = common.myInt(0);
                dr["IsSurgeryService"] = common.myInt(0);
                dr["ServiceRemarks"] = "";
                dr["ServiceStatus"] = "  ";
                //dr["IsExcluded"] = "  ";
                dr["IsExcluded"] = common.myBool(hshServiceDetail["IsExcluded"]);
                dr["IsPriceEditable"] = common.myStr(hshServiceDetail["IsPriceEditable"]);
                dr["IsApprovalReq"] = common.myStr(hshServiceDetail["IsApproval"]);
                decimal totPayable = common.myDec(hshServiceDetail["PatientNPayable"]) + common.myDec(hshServiceDetail["PayorNPayable"]);
                dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                dr["ChargePercentage"] = 0;
                dr["CopayAmt"] = common.myDbl(hshServiceDetail["insCoPayAmt"]);
                dr["CopayPerc"] = common.myDbl(hshServiceDetail["insCoPayPerc"]);
                dr["IsCoPayOnNet"] = common.myDbl(hshServiceDetail["IsCoPayOnNet"]);
                dr["DeductableAmount"] = common.myDbl(hshServiceDetail["mnDeductibleAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;
                dr["POCRequest"] = common.myBool(chkPOCRequest.Checked);

                dr["ServiceInstructions"] = common.myStr(hshServiceDetail["chvServiceInstructions"]);
                dr["TariffId"] = common.myInt(hshServiceDetail["DefaultTariffId"]);

                dt.Rows.Add(dr);
                maxId = maxId + 1;
            }

            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("FromDate"))
                    {
                        dt.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToDate"))
                    {
                        dt.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("EquipmentType"))
                    {
                        dt.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            ViewState["OPServicesInv_"] = dt;
            gvService.DataSource = dt;
            gvService.DataBind();
            ViewState["DuplicateService"] = 0;
            lblMessage.Text = "";
            dvConfirmPrintingOptions.Visible = false;
            divExcludedService.Visible = false;
            divServicelimit.Visible = false;
            ddlService.Text = "";
            ddlService.ClearSelection();
            ddlService.EmptyMessage = "";
            Page.SetFocus(ddlService);
            foreach (ListItem item in chkStat.Items)
            {
                item.Selected = false;
            }
            chkPOCRequest.Checked = false;
            hdnStatValueContainer.Value = "0";
            chkIsBioHazard.Checked = false;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnExcludedService_OnClick(object sender, EventArgs e)
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        DataTable dt = new DataTable();
        try
        {
            divExcludedService.Visible = false;
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (common.myInt(ViewState["DuplicateService"]).Equals(1))
            {
                dvConfirmPrintingOptions.Visible = true;
            }
            else
            {
                addService();
                divExcludedService.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }
    protected void btnAlredyExist_OnClick(object sender, EventArgs e)
    {
        addService();
    }
    public void addService()
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        DataTable dt = new DataTable();
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        try
        {
            dt = (DataTable)ViewState["OPServicesInv_"];

            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("FromDate"))
                    {
                        dt.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToDate"))
                    {
                        dt.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("EquipmentType"))
                    {
                        dt.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            int maxId = 0;
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
            }
            if (!dt.Columns.Contains("TariffId"))
            {
                dt.Columns.Add("TariffId", typeof(int));
            }
            DataRow dr = dt.NewRow();
            #region isShowEmergency
            int IsEmergency = (chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;
            #endregion
            //change Palendra
            int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;//by palendra
                                                                                                        //change Palendra
            hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                       common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value), common.myInt(hdnInsuranceId.Value),
                                       common.myInt(hdnCardId.Value), common.myStr(ViewState["OP_IP"]),
                                       common.myInt(ViewState["ServiceId"]),
                                       common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]),
                                       0, 0, IsEmergency, Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);

            // string a = common.myStr(hshServiceDetail["chvServiceInstructions"]);

            decimal totPayable = common.myDec(hshServiceDetail["PatientNPayable"]) + common.myDec(hshServiceDetail["PayorNPayable"]);
            if (common.myBool(hndEquipmentType.Value))
            {

                CalculateEquipmentTypeEditablePrice(common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value)), common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value))
                    , CalculateUnitInMinutesForEquipmentType(), common.myStr(common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value)))
                    , common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value))
                    , common.myStr(common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)))), common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value))
                    , common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value))
                    );
            }

            if (common.myInt(ViewState["EditIndx"]) > 0)
            {
                if (ViewState["ESrNo"] != null)
                {
                    dr["Sno"] = common.myInt(ViewState["ESrNo"]) + 1;
                }
            }
            else
            {
                dr["Sno"] = maxId + 1;
            }
            dr["ServiceId"] = common.myInt(ViewState["ServiceId"]);
            dr["UnderPackage"] = common.myInt(0);
            dr["PackageId"] = common.myInt(0);
            dr["DoctorID"] = common.myInt(0);
            if (common.myStr(hshServiceDetail["ServiceType"]).ToUpper().Equals("CL"))
            { dr["DoctorRequired"] = "True"; }
            else
            { dr["DoctorRequired"] = common.myStr(hshServiceDetail["DoctorRequired"]); }
            dr["DepartmentId"] = common.myInt(hshServiceDetail["DepartmentId"]);
            dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
            if (common.myBool(hndEquipmentType.Value))
            {
                dr["ServiceName"] = common.myStr(ViewState["ServiceName"]) + " </br> " + common.myStr(dtFromDate.SelectedDate.Value.ToString("dd/MM/yyyy HH:mm")) + "</br>" + common.myStr(dtToDate.SelectedDate.Value.ToString("dd/MM/yyyy HH:mm"));
            }
            else
            {
                dr["ServiceName"] = common.myStr(ViewState["ServiceName"]);
            }

            if (common.myBool(hndEquipmentType.Value))
            {
                dr["Units"] = CalculateUnitInMinutesForEquipmentType();
            }
            else
            {
                dr["Units"] = common.myInt(1);
            }
            dr["Charge"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["ServiceAmount"] = common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["DoctorAmount"] = common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["ServiceDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["ServiceDiscountAmount"] = common.myDec(hshServiceDetail["DiscountNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

            dr["DoctorDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["DoctorDiscountAmount"] = common.myDec(hshServiceDetail["DiscountDNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            //  dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            if (common.myBool(hndEquipmentType.Value))
            {
                dr["TotalDiscount"] = common.myDec(ViewState["DiscountAmt"]);
            }
            else
            {
                dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            }
            // dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            if (common.myBool(hndEquipmentType.Value))
            {
                dr["AmountPayableByPatient"] = common.myDec(ViewState["AmountPayableByPatient"]);
            }
            else
            {
                dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            }
            //dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            if (common.myBool(hndEquipmentType.Value))
            {
                dr["AmountPayableByPayer"] = common.myDec(ViewState["AmountPayableByPayer"]);
            }
            else
            {
                dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            }

            dr["IsPackageMain"] = common.myInt(0);
            dr["IsPackageService"] = common.myInt(0);
            dr["ResourceId"] = "  ";
            dr["MainSurgeryId"] = common.myInt(0);
            dr["IsSurgeryMain"] = common.myInt(0);
            dr["IsSurgeryService"] = common.myInt(0);
            dr["ServiceRemarks"] = common.myStr(txtMiscellaneousRemarks.Text);
            dr["ServiceStatus"] = "  ";
            dr["IsExcluded"] = common.myBool(hshServiceDetail["IsExcluded"]);
            dr["IsPriceEditable"] = common.myStr(hshServiceDetail["IsPriceEditable"]);
            dr["IsApprovalReq"] = common.myStr(hshServiceDetail["IsApproval"]);
            // dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
            if (common.myBool(hndEquipmentType.Value))
            {
                dr["NetCharge"] = common.myDec(ViewState["NetCharge"]);
            }
            else
            {
                dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
            }
            dr["ChargePercentage"] = 0;
            dr["CopayAmt"] = common.myDbl(hshServiceDetail["insCoPayAmt"]);
            dr["CopayPerc"] = common.myDbl(hshServiceDetail["insCoPayPerc"]);
            dr["IsCoPayOnNet"] = common.myDbl(hshServiceDetail["IsCoPayOnNet"]);
            dr["DeductableAmount"] = common.myDbl(hshServiceDetail["mnDeductibleAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
            dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
            dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;
            dr["POCRequest"] = common.myBool(chkPOCRequest.Checked);
            dr["ServiceInstructions"] = common.myStr(hshServiceDetail["chvServiceInstructions"]);
            dr["OutsourceInvestigation"] = common.myBool(hshServiceDetail["OutsourceInvestigation"]);
            if (RadDateTimePicker1.SelectedDate != null)
                dr["InvestigationDate"] = common.myStr(common.myDate(RadDateTimePicker1.SelectedDate).ToString("dd/MM/yyyy HH:mm tt"));
            else
                dr["InvestigationDate"] = DBNull.Value;

            dr["Remarks"] = common.myStr(hshServiceDetail["chvServiceRemarks"]);
            dr["IsBioHazard"] = common.myBool(chkIsBioHazard.Checked);
            dr["IsReadBack"] = common.myBool(chkIsReadBack.Checked);
            dr["ReadBackNote"] = txtIsReadBackNote.Text;

            if (common.myInt(hdnGlobleStationId.Value) > 0)
            {
                dr["StationId"] = common.myStr(hdnGlobleStationId.Value);
            }

            dr["AssignToEmpId"] = common.myInt(ddlAssignToEmpId.SelectedValue);
            dr["IsAddOnTest"] = common.myBool(chkIsAddOnTest.Checked);
            dr["ServicelimitAmount"] = common.myDec(hshServiceDetail["ServicelimitAmount"]);
            dr["IsServiceApprovalRequired"] = common.myBool(hshServiceDetail["IsServiceApprovalRequired"]);

            dr["FromDate"] = Convert.ToDateTime(dtFromDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ToDate"] = Convert.ToDateTime(dtToDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
            dr["EquipmentType"] = common.myBool(hndEquipmentType.Value);

            dr["TariffId"] = common.myInt(hshServiceDetail["DefaultTariffId"]);
            dr["IsERService"] = IsEmergency;
            dr["IsBedChargeService"] = chkisbedsidecharges.Checked ? 1 : 0;
            dt.Rows.Add(dr);
            ViewState["OPServicesInv_"] = dt;
            gvService.DataSource = dt;
            gvService.DataBind();
            ViewState["DuplicateService"] = 0;
            lblMessage.Text = "";
            dvConfirmPrintingOptions.Visible = false;
            divExcludedService.Visible = false;
            ddlService.Text = "";
            ddlService.ClearSelection();
            ddlService.EmptyMessage = "";

            dtFromDate.SelectedDate = null;
            dtFromMinutes.SelectedIndex = 0;
            dtToDate.SelectedDate = null;
            dtToMinutes.SelectedIndex = 0;
            //  dtEquipmentType.Visible = false;

            ViewState["EditIndx"] = null;
            Page.SetFocus(ddlService);
            foreach (ListItem item in chkStat.Items)
            {
                item.Selected = false;
            }
            chkPOCRequest.Checked = false;
            chkIsBioHazard.Checked = false;
            chkIsAddOnTest.Checked = false;
            //Awadhesh
            if (common.myStr(IsAllowBedSideChargeUncheck).Equals("Y"))
            {
                chkisbedsidecharges.Checked = false;
            }

            ClearControl();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            hshServiceDetail = null;
            BaseBill = null;
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrintingOptions.Visible = false;
        ViewState["DuplicateService"] = 0;
    }
    protected void lblServiceName_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkName = (LinkButton)sender;
            HiddenField hdnServiceRemarks = (HiddenField)lnkName.FindControl("hdnServiceRemarks");
            lblServiceRemarks.Text = hdnServiceRemarks.Value;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnExcludedServiceCancel_OnClick(object sender, EventArgs e)
    {
        divExcludedService.Visible = false;
    }
    private DataTable UpdateDataTable()
    {
        DataTable dt = new DataTable();
        if (ViewState["OPServicesInv_"] == null)
            return dt;
        try
        {
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "ServiceId>0";
                if (dt.DefaultView.Count > 0)
                {
                    updateAmount();
                }
            }
            dt = (DataTable)ViewState["OPServicesInv_"];
            foreach (GridViewRow gitem in gvService.Rows)
            {
                HiddenField hdnServiceId = (HiddenField)gitem.FindControl("hdnServiceId");
                HiddenField hdnDocReq = (HiddenField)gitem.FindControl("hdnDocReq");
                RadComboBox ddlDoctor = (RadComboBox)gitem.FindControl("ddlDoctor");
                if (common.myInt(hdnServiceId.Value) > 0)
                {
                    dt.DefaultView.RowFilter = "";
                    dt.DefaultView.RowFilter = "ServiceId= " + common.myInt(hdnServiceId.Value);
                    if (dt.DefaultView.Count > 0)
                    {
                        if (common.myInt(hdnDocReq.Value).Equals(1) || common.myBool(hdnDocReq.Value))
                        {
                            dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                        }
                        dt.AcceptChanges();
                        dt.DefaultView.RowFilter = "";
                    }
                }
            }
            return dt;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }
    }
    public void updateAmount()
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        DataTable dt = new DataTable();
        try
        {
            dt = (DataTable)ViewState["OPServicesInv_"];

            if (!dt.Columns.Contains("TariffId"))
            {
                dt.Columns.Add("TariffId", typeof(int));
            }
            foreach (GridViewRow gitem in gvService.Rows)
            {
                HiddenField hdnServiceId = (HiddenField)gitem.FindControl("hdnServiceId");
                HiddenField hdnDocReq = (HiddenField)gitem.FindControl("hdnDocReq");
                RadComboBox ddlDoctor = (RadComboBox)gitem.FindControl("ddlDoctor");
                TextBox txtUnits = (TextBox)gitem.FindControl("txtUnits");
                TextBox txtServiceAmount = (TextBox)gitem.FindControl("txtServiceAmount");
                TextBox txtDoctorAmount = (TextBox)gitem.FindControl("txtDoctorAmount");
                TextBox txtDiscountPercent = (TextBox)gitem.FindControl("txtDiscountPercent");
                TextBox txtDiscountAmt = (TextBox)gitem.FindControl("txtDiscountAmt");
                TextBox txtNetCharge = (TextBox)gitem.FindControl("txtNetCharge");
                TextBox txtAmountPayableByPatient = (TextBox)gitem.FindControl("txtAmountPayableByPatient");
                TextBox txtAmountPayableByPayer = (TextBox)gitem.FindControl("txtAmountPayableByPayer");
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.RowFilter = "ServiceId= " + common.myInt(hdnServiceId.Value);
                if (dt.DefaultView.Count > 0)
                {
                    dt.DefaultView[0]["Charge"] = common.myDec(txtServiceAmount.Text) + common.myDec(txtDoctorAmount.Text);
                    dt.DefaultView[0]["ServiceAmount"] = common.myDec(txtServiceAmount.Text);
                    dt.DefaultView[0]["DoctorAmount"] = common.myDec(txtDoctorAmount.Text);
                    dt.DefaultView[0]["NetCharge"] = common.myDec(txtNetCharge.Text);
                    dt.DefaultView[0]["TotalDiscount"] = common.myDec(txtDiscountAmt.Text);
                    //    dt.DefaultView[0]["ServiceDiscountAmount"] = common.myDec(txtDiscountAmt.Text);
                    dt.DefaultView[0]["Units"] = common.myDec(txtUnits.Text);
                    if (common.myInt(ViewState["BillType"]).Equals(1))
                    {
                        dt.DefaultView[0]["AmountPayableByPatient"] = common.myDec(txtAmountPayableByPatient.Text);
                    }
                    else
                    {
                        dt.DefaultView[0]["AmountPayableByPayer"] = common.myDec(txtAmountPayableByPayer.Text);
                    }
                    if (common.myInt(hdnDocReq.Value).Equals(1) || common.myBool(hdnDocReq.Value) == true)
                    {
                        dt.DefaultView[0]["DoctorID"] = common.myInt(ddlDoctor.SelectedValue);
                    }
                    dt.AcceptChanges();
                    dt.DefaultView.RowFilter = "";
                }
            }
            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("FromDate"))
                    {
                        dt.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToDate"))
                    {
                        dt.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("EquipmentType"))
                    {
                        dt.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            ViewState["OPServicesInv_"] = dt;
            gvService.DataSource = dt;
            gvService.DataBind();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        { dt.Dispose(); }
    }
    protected void btnGetBalance_OnClick(object sender, EventArgs e)
    {
        updateAmount();
    }
    public DataTable applyCoPayment(DataTable dt)
    {
        if (ViewState["OPServicesInv_"] == null)
            return dt;
        try
        {
            decimal CopayPercentage = 0, CopayAmt = 0, DeductableAmt = 0, DeductablePerc = 0, TotalNetCharge = 0;
            decimal totalAmount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!dt.Columns.Contains("IsCoPayOnNet"))
                {
                    dt.Columns.Add("CopayPerc");
                }
                if (!dt.Columns.Contains("CopayPerc"))
                {
                    dt.Columns.Add("IsCoPayOnNet");
                }
                if (!dt.Columns.Contains("TariffId"))
                {
                    dt.Columns.Add("TariffId", typeof(int));
                }
                //CopayPerc
                if (common.myInt(dr["IsCoPayOnNet"]).Equals(0))//Gross
                {
                    if (common.myInt(dr["CopayPerc"]) > 0)
                    {
                        totalAmount = totalAmount + common.myDec(dr["Charge"]);
                    }
                }
                else
                {
                    if (common.myInt(dr["CopayPerc"]) > 0)
                    {
                        totalAmount = totalAmount + common.myDec(common.myDec(dr["AmountPayableByPayer"]) + common.myDec(dr["AmountPayableByPatient"]));
                    }
                }
                if (common.myInt(dr["DeductableAmount"]) > 0)
                {
                    TotalNetCharge = TotalNetCharge + common.myDec(common.myDec(dr["AmountPayableByPayer"]) + common.myDec(dr["AmountPayableByPatient"]));
                }
            }
            foreach (DataRow dr1 in dt.Rows)
            {
                decimal TotalAmount = 0;
                if ((common.myInt(dr1["DeductableAmount"]) > 0))
                {
                    if (common.myInt(dr1["IsCoPayOnNet"]).Equals(0))//Gross
                    {
                        if (common.myInt(dr1["DeductableAmount"]) > 0)
                        {
                            TotalAmount = common.myDec(dr1["Charge"]);
                        }
                    }
                    else//net
                    {
                        if (common.myInt(dr1["DeductableAmount"]) > 0)
                        {
                            TotalAmount = common.myDec(common.myDec(dr1["AmountPayableByPayer"]) + common.myDec(dr1["AmountPayableByPatient"]));
                        }
                    }
                    CopayPercentage = common.myDec(dr1["CopayPerc"]);
                    CopayAmt = CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"]));
                    DeductableAmt = common.myDec(dr1["DeductableAmount"]);
                    decimal AmountPayableByPatient = 0, AmountPayableByPayer = 0;
                    if (common.myInt(dr1["IsCoPayOnNet"]).Equals(0))//Gross
                    {
                        AmountPayableByPatient = CopayAmt + DeductableAmt;
                        AmountPayableByPayer = TotalAmount - DeductableAmt - common.myDec(CopayAmt) - common.myDec(dr1["ServiceDiscountAmount"]);
                    }
                    else
                    {
                        AmountPayableByPatient = CopayAmt + DeductableAmt;
                        AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt) - DeductableAmt;
                    }
                    dr1["CopayAmt"] = common.myDbl(CopayAmt).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["CopayPerc"] = common.myDbl(CopayPercentage).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["DeductableAmount"] = common.myDbl(DeductableAmt).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["AmountPayableByPatient"] = common.myDbl(AmountPayableByPatient).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["AmountPayableByPayer"] = common.myDbl(AmountPayableByPayer).ToString("F" + hdnDecimalPlaces.Value);
                    dt.AcceptChanges();
                }
                if ((common.myInt(dr1["CopayPerc"]) > 0))
                {
                    if (common.myInt(dr1["IsCoPayOnNet"]).Equals(0))//Gross
                    {
                        if (common.myInt(dr1["CopayPerc"]) > 0)
                        {
                            TotalAmount = common.myDec(dr1["Charge"]);
                        }
                    }
                    else//net
                    {
                        if (common.myInt(dr1["CopayPerc"]) > 0)
                        {
                            TotalAmount = common.myDec(common.myDec(dr1["AmountPayableByPayer"]) + common.myDec(dr1["AmountPayableByPatient"]));
                        }
                    }
                    CopayPercentage = common.myDec(dr1["CopayPerc"]);
                    CopayAmt = CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"]));
                    DeductableAmt = common.myDec(dr1["DeductableAmount"]);
                    decimal AmountPayableByPatient = 0, AmountPayableByPayer = 0;
                    if (common.myInt(dr1["IsCoPayOnNet"]).Equals(0))//Gross
                    {
                        AmountPayableByPatient = CopayAmt + DeductableAmt;
                        AmountPayableByPayer = TotalAmount - DeductableAmt - common.myDec(CopayAmt) - common.myDec(dr1["ServiceDiscountAmount"]);
                    }
                    else
                    {
                        AmountPayableByPatient = CopayAmt + DeductableAmt;
                        AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt) - DeductableAmt;
                    }
                    dr1["CopayAmt"] = common.myDbl(CopayAmt).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["CopayPerc"] = common.myDbl(CopayPercentage).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["DeductableAmount"] = common.myDbl(DeductableAmt).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["AmountPayableByPatient"] = common.myDbl(AmountPayableByPatient).ToString("F" + hdnDecimalPlaces.Value);
                    dr1["AmountPayableByPayer"] = common.myDbl(AmountPayableByPayer).ToString("F" + hdnDecimalPlaces.Value);
                    dt.AcceptChanges();
                }
            }
            return dt;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }
    }
    protected void BindPatientProvisionalDiagnosis()
    {
        DataSet ds = new DataSet();
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProvisionalDiagnosis.Text = common.myStr(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objDiag = null;
        }
    }
    /// <summary>
    /// Used when called from IVF module. Work done by Ujjwal 17 Feb 2016
    /// </summary>
    /// <param name="PackageId"></param>
    public void PopulateIVFPackageService(int PackageId)
    {
        DataTable dt = new DataTable();
        clsIVF objivf = new clsIVF(sConString);
        RadComboBoxItem item = new RadComboBoxItem();
        try
        {
            dt = objivf.GetIVFPackageServiceDetails(PackageId);
            if (dt.Rows.Count > 0)
            {
                item.Text = (string)dt.Rows[0]["ServiceName"];
                item.Value = common.myStr(dt.Rows[0]["ServiceId"]) + "##" + common.myStr(dt.Rows[0]["ServiceType"]).Trim();
                item.Attributes.Add("ServiceName", common.myStr(dt.Rows[0]["ServiceName"]));
                item.Attributes.Add("RefServiceCode", common.myStr(dt.Rows[0]["RefServiceCode"]));
                this.ddlService.Items.Add(item);
                ddlService.SelectedIndex = 0;
                ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindItemByValue(common.myStr(dt.Rows[0]["DepartmentId"])));
                ddlDepartment.Enabled = false;
                ddlSubDepartment.SelectedIndex = ddlSubDepartment.Items.IndexOf(ddlSubDepartment.Items.FindItemByValue(common.myStr(dt.Rows[0]["SubDeptId"])));
                ddlSubDepartment.Enabled = false;
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objivf = null;
            item = null;
        }
    }

    public void addServiceFromLinkService(int MainServiceId, double mainServiceAmt)
    {
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataTable dt = new DataTable();
        DataTable dtLinkServies = new DataTable();
        bool IsPercentage = false;
        double Percentage = 0.0;
        double PercentageAmt = 0.0;
        decimal totPayable = 0;
        try
        {
            dtLinkServies = objEMROrders.getLinkServiceSetupDetails(common.myInt(Session["FacilityId"]), MainServiceId);

            if (ViewState["OPServicesInv_"] != null)
            {
                dt = (DataTable)ViewState["OPServicesInv_"];
            }

            if (!dt.Columns.Contains("TariffId"))
            {
                dt.Columns.Add("TariffId", typeof(int));
            }

            int maxId = 0;
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
            }

            foreach (DataRow dtr in dtLinkServies.Rows)
            {
                DataRow dr = dt.NewRow();

                IsPercentage = common.myBool(dtr["IsPercentage"]);
                Percentage = common.myDbl(dtr["Percentage"]);

                hshServiceDetail = new Hashtable();

                #region isShowEmergency
                int IsEmergency = (chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;
                #endregion
                //change Palendra
                int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;
                //change Palendra
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(Session["FacilityId"]),
                                            common.myInt(hdnCompanyId.Value),
                                            common.myInt(hdnInsuranceId.Value),
                                            common.myInt(hdnCardId.Value),
                                            common.myStr(ViewState["OP_IP"]),
                                            common.myInt(dtr["ServiceId"]),
                                            common.myInt(ViewState["Regid"]),
                                            common.myInt(ViewState["EncId"]), 0, 0, IsEmergency,
                                            Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);

                dr["Sno"] = maxId + 1;
                dr["ServiceId"] = common.myInt(dtr["ServiceId"]);
                dr["UnderPackage"] = common.myInt(0);
                dr["PackageId"] = common.myInt(0);
                dr["DoctorID"] = common.myInt(0);
                if (common.myStr(hshServiceDetail["ServiceType"]).ToUpper().Equals("CL"))
                {
                    dr["DoctorRequired"] = "True";
                }
                else
                {
                    dr["DoctorRequired"] = common.myStr(hshServiceDetail["DoctorRequired"]);
                }
                dr["DepartmentId"] = common.myInt(hshServiceDetail["DepartmentId"]);
                dr["ServiceType"] = common.myStr(hshServiceDetail["ServiceType"]);
                dr["ServiceName"] = common.myStr(dtr["ServiceName"]);
                dr["Units"] = common.myInt(1);
                dr["IsPackageMain"] = 0;
                dr["IsPackageService"] = 0;
                dr["ResourceId"] = "  ";
                dr["MainSurgeryId"] = 0;
                dr["IsSurgeryMain"] = 0;
                dr["IsSurgeryService"] = 0;
                dr["ServiceRemarks"] = string.Empty;
                dr["ServiceStatus"] = "  ";
                //dr["IsExcluded"] = "  ";
                dr["TariffId"] = common.myInt(hshServiceDetail["DefaultTariffId"]);

                if (IsPercentage)
                {
                    if (Percentage > 0)
                    {
                        PercentageAmt = (mainServiceAmt * (Percentage / 100.00));
                    }

                    dr["Charge"] = common.myDec(PercentageAmt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["ServiceAmount"] = common.myDec(PercentageAmt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["DoctorAmount"] = 0;
                    dr["ServiceDiscountPercentage"] = 0;
                    dr["ServiceDiscountAmount"] = 0;
                    dr["DoctorDiscountPercentage"] = 0;
                    dr["DoctorDiscountAmount"] = 0;
                    dr["TotalDiscount"] = 0;
                    dr["IsExcluded"] = false;
                    dr["IsPriceEditable"] = "0";
                    dr["IsApprovalReq"] = "0";
                    dr["ChargePercentage"] = 0;
                    dr["CopayAmt"] = 0;
                    dr["CopayPerc"] = 0;
                    dr["IsCoPayOnNet"] = 0;
                    dr["DeductableAmount"] = 0;
                    dr["Stat"] = false;
                    dr["Urgent"] = false;
                    dr["POCRequest"] = false;
                    dr["ServiceInstructions"] = string.Empty;
                    dr["IsBioHazard"] = false;

                    if (common.myInt(ViewState["BType"]).Equals(2)) //2 Credit
                    {
                        dr["AmountPayableByPayer"] = common.myDec(PercentageAmt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    }
                    else //1 or 3 Cash
                    {
                        dr["AmountPayableByPatient"] = common.myDec(PercentageAmt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    }

                    totPayable = common.myDec(dr["AmountPayableByPatient"]) + common.myDec(dr["AmountPayableByPayer"]);
                    dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                }
                else
                {
                    dr["Charge"] = common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNchr"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["ServiceAmount"] = common.myDec(hshServiceDetail["NChr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["DoctorAmount"] = common.myDec(hshServiceDetail["DNchr"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["ServiceDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["ServiceDiscountAmount"] = common.myDec(hshServiceDetail["DiscountNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                    dr["DoctorDiscountPercentage"] = common.myDec(hshServiceDetail["DiscountPerc"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["DoctorDiscountAmount"] = common.myDec(hshServiceDetail["DiscountDNAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["IsExcluded"] = common.myBool(hshServiceDetail["IsExcluded"]);
                    dr["IsPriceEditable"] = common.myStr(hshServiceDetail["IsPriceEditable"]);
                    dr["IsApprovalReq"] = common.myStr(hshServiceDetail["IsApproval"]);
                    dr["ChargePercentage"] = 0;
                    dr["CopayAmt"] = common.myDbl(hshServiceDetail["insCoPayAmt"]);
                    dr["CopayPerc"] = common.myDbl(hshServiceDetail["insCoPayPerc"]);
                    dr["IsCoPayOnNet"] = common.myDbl(hshServiceDetail["IsCoPayOnNet"]);
                    dr["DeductableAmount"] = common.myDbl(hshServiceDetail["mnDeductibleAmt"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                    dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;
                    dr["POCRequest"] = common.myBool(chkPOCRequest.Checked);
                    dr["ServiceInstructions"] = common.myStr(hshServiceDetail["chvServiceInstructions"]);
                    dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    dr["IsBioHazard"] = common.myBool(chkIsBioHazard.Checked);

                    totPayable = common.myDec(hshServiceDetail["PatientNPayable"]) + common.myDec(hshServiceDetail["PayorNPayable"]);
                    dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                }

                dt.Rows.Add(dr);
                maxId = maxId + 1;
            }

            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("FromDate"))
                    {
                        dt.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToDate"))
                    {
                        dt.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("EquipmentType"))
                    {
                        dt.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            ViewState["OPServicesInv_"] = dt;
            gvService.DataSource = dt;
            gvService.DataBind();

            ViewState["DuplicateService"] = 0;
            lblMessage.Text = string.Empty;
            dvConfirmPrintingOptions.Visible = false;
            divExcludedService.Visible = false;
            ddlService.Text = string.Empty;
            ddlService.ClearSelection();
            ddlService.EmptyMessage = string.Empty;
            Page.SetFocus(ddlService);
            foreach (ListItem item in chkStat.Items)
            {
                item.Selected = false;
            }
            chkPOCRequest.Checked = false;
            chkIsBioHazard.Checked = false;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            BaseBill = null;
            objEMROrders = null;
            hshServiceDetail = null;
            dtLinkServies.Dispose();
            dt.Dispose();
        }
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //try catch and finally added by sikandar for code optimization
        StringBuilder sb = new StringBuilder();

        try
        {
            if (RadDateTimePicker1.SelectedDate == null)
            {
                RadDateTimePicker1.SelectedDate = DateTime.Now;
            }

            sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
            sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sb = null;
        }
    }

    #region Adding Common Services 29-june-2017
    protected void lnkCommonServices_OnClick(object sender, EventArgs e)
    {
        try
        {
            hdnfilterOrderSet.Value = "0";
            divCommonServices.Visible = true;
            BindCommonServices(0);

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            BindCommonServices(common.myInt(hdnfilterOrderSet.Value));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }



    protected void gvCommonServices_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCommonServices.PageIndex = e.NewPageIndex;
            BindCommonServices(0);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void gvorder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvorder.PageIndex = e.NewPageIndex;
            BindCommonServices((common.myInt(Session["LoginDepartmentId"])));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            divCommonServices.Visible = false;
            lblMessage.Text = string.Empty;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    private void BindCommonServices(int LoginDepartmentId)
    {
        DataTable dt = new DataTable();
        BaseC.clsEMR objCommonService = new BaseC.clsEMR(sConString);
        BaseC.EMROrders objEMROrdersnew = new BaseC.EMROrders(sConString);
        BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
        try
        {

            string strDepartmentType = string.Empty;
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                strDepartmentType = "'I','IS','P','HPP','C','O','OPP','RF'";//'CL'
            }
            else
            {
                strDepartmentType = "'I','IS','P','HPP','C','O','OPP','OS'"; //'OS' for Moolchand
            }
            if (common.myBool(Request.QueryString["IsExternal"]) && common.myBool(objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "SpecialBehaviorForDiagnosticOnlyChkOnReg", common.myInt(Session["FacilityId"]))))
            {
                strDepartmentType = "'I','IS'";
            }
            if (common.myStr(ViewState["OP_IP"]).Equals("O"))
            {
                if (common.myBool(Request.QueryString["IsPatientExpired"]))
                {
                    strDepartmentType = "'OS'";
                }

                //ds = objCommonService.GetHospitalServices(common.myInt(Session["HospitallocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), "Y", common.myInt(Session["EntrySite"]));
            }
            if (LoginDepartmentId > 0)
            {

                SqlDataReader objDrOrder = objEMROrdersnew.EMRpopulateInvestigationSetMain(Convert.ToInt16(Session["HospitalLocationID"]),
               common.myInt(0), common.myStr(txtCommonServiceSearch.Text));
                dt.Load(objDrOrder);
                gvCommonServices.Visible = false;
                hdnfilterOrderSet.Value = "1";
                gvorder.Visible = true;
                gvorder.DataSource = dt;
                gvorder.DataBind();
            }
            else
            {
                dt = objCommonService.GetCommonServices(common.myInt(Session["FacilityID"]), strDepartmentType, common.myStr(txtCommonServiceSearch.Text), common.myStr(ViewState["OP_IP"]));
                gvorder.Visible = false;
                hdnfilterOrderSet.Value = "0";
                gvCommonServices.Visible = true;
                gvCommonServices.DataSource = dt;
                gvCommonServices.DataBind();
            }


            //ViewState["OPServicesInv_"] = dt;


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
            objCommonService = null;
            objEMROrdersnew = null;
            objHosp = null;
        }

    }

    protected void btnCommonServiceProceed_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindMultipleService();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            if (common.myStr(ViewState["ShowdivCommonServices"]).Equals("1"))
            {
                divCommonServices.Visible = true;
                ViewState["ShowdivCommonServices"] = "0";
            }
            else
            {
                divCommonServices.Visible = false;
            }
        }
    }

    private void BindMultipleService() // Replica of btnYes_OnClick
    {
        lblMessage.Text = string.Empty;
        if (ViewState["OPServicesInv_"] == null)
            return;

        DataTable dt = new DataTable();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataSet ds = new DataSet();
        StringBuilder strXMLSetID = new StringBuilder();
        try
        {
            if (common.myInt(hdnfilterOrderSet.Value) > 0)
            {
                foreach (GridViewRow dataItem in gvorder.Rows)
                {
                    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                    if (chkRow.Checked)
                    {
                        //string SetID;
                        HiddenField SetID = (HiddenField)dataItem.FindControl("hdnProblemId");
                        //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        //SetID = ((HiddenField)row.FindControl("hdnProblemId")).Value.ToString();
                        // ddldrpOrder.SelectedValue = gvorder.SelectedRow.Cells[1].ToString();

                        //rdoOrder.SelectedValue = "OS";
                        //visible(hdnServiceType.Value);
                        strXMLSetID.Append("<Table1><c1>");
                        strXMLSetID.Append(common.myInt(SetID.Value));
                        strXMLSetID.Append("</c1></Table1>");


                        //AddOrder("OS", common.myInt(0), common.myInt(SetID.Value));
                    }
                }

                AddOrder("OS", common.myInt(0), strXMLSetID.ToString());

                UpdateDataTable();

                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["OPServicesInv_"];
                if (!dt1.Columns.Contains("TariffId"))
                {
                    dt1.Columns.Add("TariffId", typeof(int));
                }


                //dt.AcceptChanges();

                if (dt1 != null)
                {
                    if (dt1.Columns.Count > 0)
                    {
                        if (!dt1.Columns.Contains("FromDate"))
                        {
                            dt1.Columns.Add("FromDate", typeof(string));
                        }
                        if (!dt1.Columns.Contains("ToDate"))
                        {
                            dt1.Columns.Add("ToDate", typeof(string));
                        }
                        if (!dt1.Columns.Contains("EquipmentType"))
                        {
                            dt1.Columns.Add("EquipmentType", typeof(bool));
                        }
                    }
                }
                ViewState["OPServices"] = dt1;
                if (dt1.Rows.Count > 0)
                {
                    gvService.DataSource = dt1;
                }
                else
                {
                    gvService.DataSource = CreateTable();
                }
                gvService.DataBind();
                dtFromDate.SelectedDate = null;
                dtFromMinutes.SelectedIndex = 0;
                dtToDate.SelectedDate = null;
                dtToMinutes.SelectedIndex = 0;
                dtEquipmentType.Visible = false;
            }
            else
            {
                foreach (GridViewRow dataItem in gvCommonServices.Rows)
                {
                    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                    if (chkRow.Checked)
                    {
                        HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");
                        HiddenField hdnServiceType = (HiddenField)dataItem.FindControl("hdnServiceType");
                        HiddenField hdnRefServiceCode = (HiddenField)dataItem.FindControl("hdnRefServiceCode");
                        HiddenField hdnIsOrderSet = (HiddenField)dataItem.FindControl("hdnIsOrderSet");
                        HiddenField hdnIsLinkService = (HiddenField)dataItem.FindControl("hdnIsLinkService");
                        HiddenField hdnIsStatOrderAllowed = (HiddenField)dataItem.FindControl("hdnIsStatOrderAllowed");
                        LinkButton lnkServiceName = (LinkButton)dataItem.FindControl("lnkServiceName");
                        //if (common.myInt(hdnServiceId.Value) > 0)
                        //{

                        //}

                        int maxId = 0;

                        dt = (DataTable)ViewState["OPServicesInv_"];
                        if (dt.Rows.Count > 0)
                        {
                            if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                            {
                                dt.Rows.Clear();
                            }
                            else
                            {
                                DataView dv = new DataView(dt);
                                dv.Sort = "Sno Desc";
                                maxId = common.myInt(dv[0]["Sno"]);
                            }
                            //Check duplicate service------------------------------------------------------------------
                            DataView dvdup = new DataView();
                            dvdup = dt.Copy().DefaultView;
                            dvdup.RowFilter = " ServiceId = " + common.myStr(hdnServiceId.Value);
                            if (dvdup.ToTable().Rows.Count > 0)
                            {
                                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                //lblMessage.Text = "Service : "+common.myStr(lnkServiceName.Text)+" already exist !";
                                ddlService.Text = "";
                                ddlService.ClearSelection();
                                ddlService.EmptyMessage = "";
                                Page.SetFocus(ddlService);
                                ViewState["CommonServicesAlreadyExist"] = "1";
                                ViewState["CommonServicesNameAlreadyExist"] = common.myStr(ViewState["CommonServicesNameAlreadyExist"]) + " , " + common.myStr(lnkServiceName.Text);
                                //  return;
                            }
                        }
                        if (common.myStr(hdnServiceType.Value).Equals("OPP"))
                        {
                            ds = objBilling.getPackageServiceDetails(common.myInt(hdnServiceId.Value), common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value),
                                common.myInt(hdnInsuranceId.Value), common.myInt(hdnCardId.Value),
                                common.myStr(ViewState["OP_IP"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0);
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (!ds.Tables[0].Columns.Contains("DeductableAmount"))
                                    {
                                        ds.Tables[0].Columns.Add("DeductableAmount", typeof(double));
                                    }
                                    if (!ds.Tables[0].Columns.Contains("IsCoPayOnNet"))
                                    {
                                        ds.Tables[0].Columns.Add("IsCoPayOnNet", typeof(double));
                                    }
                                    if (!ds.Tables[0].Columns.Contains("CopayPerc"))
                                    {
                                        ds.Tables[0].Columns.Add("CopayPerc", typeof(double));
                                    }
                                    if (!ds.Tables[0].Columns.Contains("TariffId"))
                                    {
                                        ds.Tables[0].Columns.Add("TariffId", typeof(int));
                                    }

                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        dr["DeductableAmount"] = 0;
                                        dr["IsCoPayOnNet"] = 0;
                                        dr["CopayPerc"] = 0;
                                        dt.ImportRow(dr);
                                    }

                                    if (dt != null)
                                    {
                                        if (dt.Columns.Count > 0)
                                        {
                                            if (!dt.Columns.Contains("FromDate"))
                                            {
                                                dt.Columns.Add("FromDate", typeof(string));
                                            }
                                            if (!dt.Columns.Contains("ToDate"))
                                            {
                                                dt.Columns.Add("ToDate", typeof(string));
                                            }
                                            if (!dt.Columns.Contains("EquipmentType"))
                                            {
                                                dt.Columns.Add("EquipmentType", typeof(bool));
                                            }
                                        }
                                    }

                                    ViewState["OPServicesInv_"] = dt;
                                    gvService.DataSource = dt;
                                    gvService.DataBind();
                                    ddlService.Text = "";
                                    ddlService.ClearSelection();
                                    ddlService.EmptyMessage = "";
                                    Page.SetFocus(ddlService);
                                }
                            }
                            else
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Text = "Invalid Package Amount !";
                                ViewState["CommonServicesInvalidPackageAmount"] = "1";
                            }
                        }
                        else if (common.myBool(hdnIsOrderSet.Value) && !common.myBool(hdnIsLinkService.Value))
                        {
                            BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
                            DataTable dt1 = objEMROrders.GetOrderSets(common.myInt(hdnServiceId.Value));
                            if (!common.myInt(ViewState["CommonServicesAlreadyExist"]).Equals(1))
                            {
                                addServiceFromOrderSet(dt1);
                            }
                        }
                        else
                        {
                            ViewState["ServiceId"] = common.myInt(hdnServiceId.Value);
                            //ViewState["ServiceName"] = common.myStr(ddlService.Text);
                            ViewState["ServiceName"] = common.myStr(lnkServiceName.Text);
                            //change Palendra
                            int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;
                            //change Palendra
                            hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                                       common.myInt(Session["FacilityId"]),
                                                       common.myInt(hdnCompanyId.Value),
                                                       common.myInt(hdnInsuranceId.Value),
                                                       common.myInt(hdnCardId.Value),
                                                       common.myStr(ViewState["OP_IP"]),
                                                       common.myInt(hdnServiceId.Value),
                                                       common.myInt(ViewState["Regid"]),
                                                       common.myInt(ViewState["EncId"]), 0, 0, 0,
                                                       Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);


                            if (common.myBool(hshServiceDetail["IsBlocked"]) && common.myBool(ViewState["Yes"]) == false)
                            {
                                if (common.myBool(ViewState["IsAllowToAddBlockedService"]) == false)
                                {
                                    Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                                    return;
                                }
                                divConfirmation.Visible = true;
                                return;
                            }
                            //if ((common.myStr(ViewState["OP_IP"]).Equals("I")) && (common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                            if ((common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                            {
                                divExcludedService.Visible = true;
                                lblExcludedServiceName.Text = common.myStr(ViewState["ServiceName"]);


                            }
                            else if (common.myInt(ViewState["DuplicateService"]).Equals(1))
                            {
                                dvConfirmPrintingOptions.Visible = true;
                            }
                            else
                            {
                                if (!common.myInt(ViewState["CommonServicesAlreadyExist"]).Equals(1))
                                {
                                    addService();
                                }
                                dvConfirmPrintingOptions.Visible = false;
                                divExcludedService.Visible = false;
                            }

                            if (!common.myBool(hdnIsOrderSet.Value) && common.myBool(hdnIsLinkService.Value))
                            {
                                if (!common.myInt(ViewState["CommonServicesAlreadyExist"]).Equals(1))
                                {
                                    addServiceFromLinkService(common.myInt(hdnServiceId.Value), common.myDbl(hshServiceDetail["NChr"]));
                                }
                            }
                        }
                    }

                    ViewState["CommonServicesAlreadyExist"] = 0;
                }


                if (!common.myStr(ViewState["CommonServicesNameAlreadyExist"]).Equals(string.Empty) && ViewState["CommonServicesNameAlreadyExist"] != null)
                {
                    ViewState["CommonServicesNameAlreadyExist"] = common.myStr(ViewState["CommonServicesNameAlreadyExist"]).Remove(0, 3);
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service : " + common.myStr(ViewState["CommonServicesNameAlreadyExist"]) + " already exist !";
                    ViewState["CommonServicesNameAlreadyExist"] = string.Empty;
                }
            }



        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objBilling = null;
            BaseBill = null;
            hshServiceDetail = null;
            ds.Dispose();
            txtCommonServiceSearch.Text = string.Empty;
            txtMiscellaneousRemarks.Text = string.Empty;
        }
    }

    protected void lnkServiceName_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (ViewState["OPServicesInv_"] == null)
            return;

        DataTable dt = new DataTable();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            GridViewRow dataItem = (GridViewRow)(((LinkButton)sender).NamingContainer);
            // HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");

            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");
            HiddenField hdnServiceType = (HiddenField)dataItem.FindControl("hdnServiceType");
            HiddenField hdnRefServiceCode = (HiddenField)dataItem.FindControl("hdnRefServiceCode");
            HiddenField hdnIsOrderSet = (HiddenField)dataItem.FindControl("hdnIsOrderSet");
            HiddenField hdnIsLinkService = (HiddenField)dataItem.FindControl("hdnIsLinkService");
            HiddenField hdnIsStatOrderAllowed = (HiddenField)dataItem.FindControl("hdnIsStatOrderAllowed");
            LinkButton lnkServiceName = (LinkButton)dataItem.FindControl("lnkServiceName");


            //if (common.myInt(hdnServiceId.Value) > 0)
            //{

            //}

            int maxId = 0;

            dt = (DataTable)ViewState["OPServicesInv_"];
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
                //Check duplicate service------------------------------------------------------------------
                DataView dvdup = new DataView();
                dvdup = dt.Copy().DefaultView;
                dvdup.RowFilter = " ServiceId = " + common.myStr(hdnServiceId.Value);
                if (dvdup.ToTable().Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service : " + common.myStr(lnkServiceName.Text) + " already exist !";
                    ddlService.Text = "";
                    ddlService.ClearSelection();
                    ddlService.EmptyMessage = "";
                    Page.SetFocus(ddlService);
                    ViewState["ShowdivCommonServices"] = "1";
                    return;
                }
            }
            if (common.myStr(hdnServiceType.Value).Equals("OPP"))
            {
                ds = objBilling.getPackageServiceDetails(common.myInt(hdnServiceId.Value), common.myInt(Session["HospitalLocationID"]),
                    common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value),
                    common.myInt(hdnInsuranceId.Value), common.myInt(hdnCardId.Value),
                    common.myStr(ViewState["OP_IP"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!ds.Tables[0].Columns.Contains("DeductableAmount"))
                        {
                            ds.Tables[0].Columns.Add("DeductableAmount", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("IsCoPayOnNet"))
                        {
                            ds.Tables[0].Columns.Add("IsCoPayOnNet", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("CopayPerc"))
                        {
                            ds.Tables[0].Columns.Add("CopayPerc", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("TariffId"))
                        {
                            ds.Tables[0].Columns.Add("TariffId", typeof(int));
                        }

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["DeductableAmount"] = 0;
                            dr["IsCoPayOnNet"] = 0;
                            dr["CopayPerc"] = 0;
                            dt.ImportRow(dr);
                        }

                        if (dt != null)
                        {
                            if (dt.Columns.Count > 0)
                            {
                                if (!dt.Columns.Contains("FromDate"))
                                {
                                    dt.Columns.Add("FromDate", typeof(string));
                                }
                                if (!dt.Columns.Contains("ToDate"))
                                {
                                    dt.Columns.Add("ToDate", typeof(string));
                                }
                                if (!dt.Columns.Contains("EquipmentType"))
                                {
                                    dt.Columns.Add("EquipmentType", typeof(bool));
                                }
                            }
                        }

                        ViewState["OPServicesInv_"] = dt;
                        gvService.DataSource = dt;
                        gvService.DataBind();
                        ddlService.Text = "";
                        ddlService.ClearSelection();
                        ddlService.EmptyMessage = "";
                        Page.SetFocus(ddlService);
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Package Amount !";
                    ViewState["ShowdivCommonServices"] = "1";
                }
            }
            else if (common.myBool(hdnIsOrderSet.Value) && !common.myBool(hdnIsLinkService.Value))
            {
                BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
                DataTable dt1 = objEMROrders.GetOrderSets(common.myInt(hdnServiceId.Value));
                addServiceFromOrderSet(dt1);
            }
            else
            {
                ViewState["ServiceId"] = common.myInt(hdnServiceId.Value);
                //ViewState["ServiceName"] = common.myStr(ddlService.Text);
                ViewState["ServiceName"] = common.myStr(lnkServiceName.Text);
                //change Palendra
                int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;
                //change Palendra
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                           common.myInt(Session["FacilityId"]),
                                           common.myInt(hdnCompanyId.Value),
                                           common.myInt(hdnInsuranceId.Value),
                                           common.myInt(hdnCardId.Value),
                                           common.myStr(ViewState["OP_IP"]),
                                           common.myInt(hdnServiceId.Value),
                                           common.myInt(ViewState["Regid"]),
                                           common.myInt(ViewState["EncId"]), 0, 0, 0,
                                           Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);


                if (common.myBool(hshServiceDetail["IsBlocked"]) && common.myBool(ViewState["Yes"]) == false)
                {
                    if (common.myBool(ViewState["IsAllowToAddBlockedService"]) == false)
                    {
                        Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                        return;
                    }
                    divConfirmation.Visible = true;
                    return;
                }
                //if ((common.myStr(ViewState["OP_IP"]).Equals("I")) && (common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                if (common.myBool(hshServiceDetail["IsExcluded"]))
                {
                    divExcludedService.Visible = true;
                    lblExcludedServiceName.Text = common.myStr(ViewState["ServiceName"]);


                }
                else if (common.myInt(ViewState["DuplicateService"]).Equals(1))
                {
                    dvConfirmPrintingOptions.Visible = true;
                }
                else
                {
                    addService();
                    dvConfirmPrintingOptions.Visible = false;
                    divExcludedService.Visible = false;
                }

                if (!common.myBool(hdnIsOrderSet.Value) && common.myBool(hdnIsLinkService.Value))
                {
                    addServiceFromLinkService(common.myInt(hdnServiceId.Value), common.myDbl(hshServiceDetail["NChr"]));
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
            objBilling = null;
            BaseBill = null;
            hshServiceDetail = null;
            ds.Dispose();
            txtCommonServiceSearch.Text = string.Empty;
            txtMiscellaneousRemarks.Text = string.Empty;
        }
    }



    #endregion

    protected void ibtnE_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.EMRBilling BaseBill = new BaseC.EMRBilling(sConString);
            DataTable dtEquipment = null;

            DataTable dtData = null;
            ImageButton ibtnEdit = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)ibtnEdit.NamingContainer;
            HiddenField hdnSerId = (HiddenField)gvr.FindControl("hndSerId");
            DataTable dt = GetData(string.Empty);
            DataView dv = new DataView(dt);
            dv.RowFilter = "ServiceId=" + hdnSerId.Value + "";
            int indx = common.myInt(gvr.RowIndex);
            ViewState["EditIndx"] = indx;
            RadComboBoxItem item = new RadComboBoxItem();
            if (ddlService.Items.Count > 0)
                ddlService.ClearSelection();

            item.Text = (string)dv[0]["ServiceName"];
            item.Value = common.myStr(dv[0]["ServiceId"]) + "##" + common.myStr(dv[0]["ServiceType"]) + "##" + common.myStr(dv[0]["IsOrderSet"]) + "##" + common.myStr(dv[0]["IsLinkService"]);
            item.Attributes.Add("ServiceName", common.myStr(dv[0]["ServiceName"]));
            item.Attributes.Add("RefServiceCode", common.myStr(dv[0]["RefServiceCode"]));
            item.Attributes["IsStatOrderAllowed"] = dv[0]["IsStatOrderAllowed"].ToString();
            item.Attributes["isServiceRemarkMandatory"] = dv[0]["isServiceRemarkMandatory"].ToString();
            item.Attributes["EquipmentType"] = dv[0]["EquipmentType"].ToString();
            item.Attributes["ChargesPeriod"] = dv[0]["ChargesPeriod"].ToString();
            item.Attributes["GracePeriod"] = dv[0]["GracePeriod"].ToString();
            item.DataBind();
            this.ddlService.Items.Add(item);

            dtData = (DataTable)ViewState["OPServicesInv_"];

            DataView dvdup = new DataView();
            dvdup = dtData.Copy().DefaultView;
            dvdup.RowFilter = " ServiceId = " + common.myStr(hdnSerId.Value);
            ViewState["ESrNo"] = common.myInt(dvdup[0]["Sno"]);
            if (common.myBool(dvdup[0]["EquipmentType"]))
            {
                dtEquipment = BaseBill.GetEquipmentDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnSerId.Value));

                if (common.myInt(dtEquipment.Rows.Count) > 0)
                {
                    hndEquipmentType.Value = common.myStr(dtEquipment.Rows[0]["IsEquipmentType"]);
                    hndChargesPeriod.Value = common.myStr(dtEquipment.Rows[0]["ChargesPeriod"]);
                    hndGracePeriod.Value = common.myStr(dtEquipment.Rows[0]["GracePeriod"]);
                }
            }
            else
            {
                dtEquipmentType.Visible = false;
                hndEquipmentType.Value = "false";
                hndChargesPeriod.Value = "0";
                hndGracePeriod.Value = "0";
            }
            //txtProvisionalDiagnosis.Text =common.myStr(dvdup[0]["ProvisionalDiagnosis"]);
            txtMiscellaneousRemarks.Text = common.myStr(dvdup[0]["ServiceRemarks"]);
            string stat = common.myStr(dvdup[0]["STAT"]);
            string urgent = common.myStr(dvdup[0]["URGENT"]);
            if (stat.Equals("1"))
            {
                chkStat.SelectedIndex = 0;
            }
            if (urgent.Equals("1"))
            {
                chkStat.SelectedIndex = 1;
            }
            chkIsBioHazard.Checked = common.myBool(dvdup[0]["IsBiohazard"]);
            chkPOCRequest.Checked = common.myBool(dvdup[0]["POCRequest"]);
            chkisbedsidecharges.Checked = common.myBool(dvdup[0]["IsBedChargeService"]);


            if (common.myDate(dvdup[0]["InvestigationDate"]) != null)
            {
                RadDateTimePicker1.SelectedDate = common.myDate(dvdup[0]["InvestigationDate"]);
            }
            chkApprovalRequired.Checked = common.myBool(dvdup[0]["IsApprovalReq"]);

            //txtIsReadBackNote.Text = common.myStr(dvdup[0]["POCRequest"]);

            if (chkApprovalRequired.Checked)
            {
                chkIsReadBack.Visible = true;
                lblReadBackNote.Visible = true;
                txtIsReadBackNote.Visible = true;
            }
            else
            {
                chkIsReadBack.Visible = false;
                lblReadBackNote.Visible = false;
                txtIsReadBackNote.Visible = false;

                txtIsReadBackNote.Text = "";
                chkIsReadBack.Checked = false;
            }

            chkIsReadBack.Checked = common.myBool(dvdup[0]["IsReadBack"]);
            txtIsReadBackNote.Text = common.myStr(dvdup[0]["ReadBackNote"]);



            hdnGlobleServiceType.Value = common.myStr(dvdup[0]["ServiceType"]).ToString();
            hdnGlobleStationId.Value = common.myInt(dvdup[0]["StationId"]).ToString();

            if (common.myBool(dvdup[0]["EquipmentType"]))
            {
                dtEquipmentType.Visible = true;
            }
            dtFromDate.SelectedDate = Convert.ToDateTime(dvdup[0]["FromDate"]);
            dtToDate.SelectedDate = Convert.ToDateTime(dvdup[0]["ToDate"]);

            dtFromMinutes.SelectedValue = common.myStr(dtFromDate.SelectedDate).Substring(common.myStr(dvdup[0]["FromDate"]).Length - 8, 2);
            dtToMinutes.SelectedValue = common.myStr(dtToDate.SelectedDate).Substring(common.myStr(dvdup[0]["ToDate"]).Length - 8, 2);

            bindAssignToDoctor();
            visible(common.myStr(hdnGlobleServiceType.Value));

            ddlAssignToEmpId.SelectedIndex = ddlAssignToEmpId.Items.IndexOf(ddlAssignToEmpId.Items.FindItemByValue(common.myInt(dvdup[0]["AssignToEmpId"]).ToString()));



            //DataView DV =new DataView();
            //DV = dtData.Copy().DefaultView;
            //DV.RowFilter = " ServiceId <> " + common.myStr(hdnSerId.Value);


            //dtData = DV.ToTable();

            dtData.AcceptChanges();

            if (dtData != null)
            {
                if (dtData.Columns.Count > 0)
                {
                    if (!dtData.Columns.Contains("FromDate"))
                    {
                        dtData.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dtData.Columns.Contains("ToDate"))
                    {
                        dtData.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dtData.Columns.Contains("EquipmentType"))
                    {
                        dtData.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            ViewState["OPServices"] = dtData;
            ViewState["OPServicesInv_"] = dtData;
            if (dtData.Rows.Count > 0)
            {
                gvService.DataSource = dtData;
            }
            else
            {
                gvService.DataSource = CreateTable();
            }
            gvService.DataBind();
            //ddlService.Text = "";
            //ddlService.ClearSelection();
            //ddlService.EmptyMessage = "";
            //RadDateTimePicker1.SelectedDate = null;
            //Page.SetFocus(ddlService);

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
        finally { }
    }

    protected void btnReprint_Click(object sender, EventArgs e)
    {
        string sRegNo = common.myStr(lblregNo.Text);
        string sRegId = common.myStr(ViewState["RegId"]);//15009
        string sEncId = common.myStr(ViewState["EncId"]);//1316
        string sEncDate = common.myStr(lblAdmissionDate.Text);

        RadWindow1.NavigateUrl = "~/WardManagement/ServiceActivityDetails.aspx?Regno=" + common.myStr(sRegNo) +
                                "&RegID=" + common.myStr(sRegId) +
                                "&From=POPUP&EncounterId=" + common.myInt(sEncId) +
                                "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy");
        RadWindow1.Height = 400;
        RadWindow1.Width = 700;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;

    }

    protected void btnorderprint_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "~/EMR/Orders/PrintOrder.aspx?rptName=PODSave&encounterId=" + common.myStr(ViewState["EncId"]) + "&RegistrationId=" + common.myStr(ViewState["Regid"]) + "&OrderId=" + common.myStr(ViewState["OrderId"]);
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnPrintIPServiceOrder_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ViewState["OrderId"]) > 0)
            {
                RadWindow1.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?MPG=P22277&EncId=" + common.myInt(ViewState["EncId"])
                + "&RptType=ORDERPRINT"
                + "&RegId=" + common.myStr(ViewState["Regid"])
                + "&OrderId=" + common.myStr(ViewState["OrderId"]);

                RadWindow1.Height = 600;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Please save order and and print", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void chkIsAddOnTest_CheckedChanged(object sender, EventArgs e)
    {
        string str;
        if (chkIsAddOnTest.Checked.Equals(true))
        {
            str = " A) Previous sample sent within 8 hours." + "\\n" + " B) Call to Lab about sufficient qty. of sample available. " + "\\n" + " C) Send ‘TRF’ marked as ‘Add On’ to Lab.";
            Alert.ShowAjaxMsg(str, Page);
            return;
        }
    }

    protected void btnServicelimit_Click(object sender, EventArgs e)
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        DataTable dt = new DataTable();
        try
        {
            divServicelimit.Visible = false;
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (common.myInt(ViewState["DuplicateService"]).Equals(1))
            {
                dvConfirmPrintingOptions.Visible = true;
            }
            else
            {
                addService();
                divServicelimit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    protected void btnServicelimitCancel_Click(object sender, EventArgs e)
    {
        divServicelimit.Visible = false;

    }

    private void BindFromMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (common.myStr(i).Length.Equals(1))
                {
                    dtFromMinutes.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                }
                else
                {
                    dtFromMinutes.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void BindToMinutes()
    {
        try
        {
            for (int i = 0; i < 60; i++)
            {
                if (common.myStr(i).Length.Equals(1))
                {
                    dtToMinutes.Items.Add(new RadComboBoxItem("0" + common.myStr(i), "0" + common.myStr(i)));
                }
                else
                {
                    dtToMinutes.Items.Add(new RadComboBoxItem(common.myStr(i), common.myStr(i)));
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void dtFromMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(common.myStr(dtFromDate.SelectedDate.Value));
        sb.Remove(common.myStr(dtFromDate.SelectedDate.Value).IndexOf(":") + 1, 2);
        sb.Insert(common.myStr(dtFromDate.SelectedDate.Value).IndexOf(":") + 1, dtFromMinutes.Text);
        //dtFromDate.SelectedDate = common.myDate(common.myStr(sb));
        dtFromDate.SelectedDate = Convert.ToDateTime(common.myStr(sb));
    }
    protected void dtToMinutes_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(common.myStr(dtToDate.SelectedDate.Value));
        sb.Remove(common.myStr(dtToDate.SelectedDate.Value).IndexOf(":") + 1, 2);
        sb.Insert(common.myStr(dtToDate.SelectedDate.Value).IndexOf(":") + 1, dtToMinutes.Text);
        //dtToDate.SelectedDate = common.myDate(common.myStr(sb));
        dtToDate.SelectedDate = Convert.ToDateTime(common.myStr(sb));
    }

    private int CalculateUnitInMinutesForEquipmentType()
    {
        int FinalMinutes = 0, slot = 0, slotmin = 60, GraceMin = 15;
        int mi, hr, days;
        try
        {
            slotmin = common.myInt(hndChargesPeriod.Value); GraceMin = common.myInt(hndGracePeriod.Value);

            TimeSpan span = Convert.ToDateTime(dtToDate.SelectedDate.Value).Subtract(Convert.ToDateTime(dtFromDate.SelectedDate.Value));

            // s = span.Seconds;
            mi = span.Minutes;
            hr = span.Hours;
            days = span.Days;

            //if (days > 0)
            //{
            //    FinalMinutes = FinalMinutes + (days * 1440);
            //}
            //if (hr > 0)
            //{
            //    FinalMinutes = FinalMinutes + (hr * 60);
            //}
            //if (mi > 0)
            //{
            //    //For One hourly slot
            //    FinalMinutes = FinalMinutes + 60;
            //    //For Half hourly slot
            //    //  FinalMinutes = FinalMinutes + 30;
            //}

            if (days > 0)
            {
                FinalMinutes = FinalMinutes + (days * 1440);
            }
            if (hr > 0)
            {
                FinalMinutes = FinalMinutes + (hr * 60);
            }
            if (mi > 0)
            {
                FinalMinutes = FinalMinutes + mi;
            }

            slot = FinalMinutes / slotmin;

            if ((common.myInt(FinalMinutes % slotmin) != 0) && (common.myInt(FinalMinutes % slotmin) > GraceMin))
            {
                slot = slot + 1;
            }

            if (slot.Equals(0))
            {
                slot = 1;
            }

            FinalMinutes = (slot * slotmin);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

        return slot;
    }

    private void CalculateEquipmentTypeEditablePrice(string txtServiceAmount, string txtDoctorAmount, int txtUnits, string txtDiscountPercent, string txtDiscountAmt, string txtNetCharge, string txtAmountPayableByPatient, string txtAmountPayableByPayer)
    {

        var DecimalPlaces = hdnDecimalPlaces.Value;

        decimal DiscountPerc = common.myDec(txtDiscountPercent);
        decimal totalAmount = common.myDec(txtServiceAmount) + common.myDec(txtDoctorAmount);
        decimal DiscountAmt;
        if (DiscountPerc > 0)
        {
            DiscountAmt = common.myDec((((DiscountPerc * 1) / 100) * ((txtUnits * 1) * ((totalAmount * 1)))));
        }
        else
        {
            DiscountAmt = common.myDec((0 * 1));
        }

        decimal ServiceCharge = common.myDec(txtServiceAmount);
        decimal DoctorCharge = common.myDec(txtDoctorAmount);
        int Units = txtUnits;
        totalAmount = (ServiceCharge) + DoctorCharge;


        // document.getElementById(txtDiscountAmt).value = parseFloat(DiscountAmt).toFixed(DecimalPlaces);
        decimal NetAmount = (Units * 1) * totalAmount;
        ViewState["DiscountAmt"] = DiscountAmt;

        // document.getElementById(txtNetCharge).value = parseFloat(NetCharge).toFixed(DecimalPlaces);
        decimal NetCharge = common.myDec(NetAmount - DiscountAmt);
        ViewState["NetCharge"] = NetCharge;



        //var hdnBilltype = $get('<%=hdnBilltype.ClientID%>')

        //  alert(parseInt(hdnBilltype.value));

        if (common.myInt(hdnBilltype.Value) == 1)
        {
            //  document.getElementById(txtAmountPayableByPatient).value = parseFloat(NetCharge).toFixed(DecimalPlaces);
            ViewState["AmountPayableByPatient"] = NetCharge;
        }
        else
        {
            // document.getElementById(txtAmountPayableByPayer).value = parseFloat(NetCharge).toFixed(DecimalPlaces);
            ViewState["AmountPayableByPayer"] = NetCharge;
        }

    }

    protected void dtOrderDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        try
        {
            if (common.myBool(hndEquipmentType.Value))
            {
                dtFromDate.SelectedDate = Convert.ToDateTime(dtOrderDate.SelectedDate);
                dtToDate.SelectedDate = Convert.ToDateTime(dtOrderDate.SelectedDate);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

    }

    protected void lnkOrderset_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfilterOrderSet.Value = "1";
            divCommonServices.Visible = true;
            BindCommonServices(common.myInt(Session["LoginDepartmentId"]));

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    public void AddOrder(string from, int serviceId, string OrderSetId)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["OPServicesInv_"];

            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("FromDate"))
                    {
                        dt.Columns.Add("FromDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToDate"))
                    {
                        dt.Columns.Add("ToDate", typeof(string));
                    }
                    if (!dt.Columns.Contains("EquipmentType"))
                    {
                        dt.Columns.Add("EquipmentType", typeof(bool));
                    }
                }
            }

            int maxId = 0;
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0)) //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
            }
            if (!dt.Columns.Contains("TariffId"))
            {
                dt.Columns.Add("TariffId", typeof(int));
            }
            DataRow dr = dt.NewRow();

            int IsEmergency = (chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;

            int CompanyId = 0, InsuranceId = 0, CardId = 0, DOCTORID = 0;

            string dnm = "";
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);

            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string flagEMROderDefaultDoctorSelection = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]), "EMROrderSetDefaultServiceDoctor", sConString);
            DataSet dsCharge = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));
            if (RadDateTimePicker1.SelectedDate != null)
            {
                if (DateTime.Compare(Convert.ToDateTime(RadDateTimePicker1.SelectedDate), DateTime.Now.Date) < 0)
                {
                    Alert.ShowAjaxMsg("please select correct investigation date", Page);
                    return;
                }
            }
            if (dsCharge.Tables.Count > 0)
            {
                if (dsCharge.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(dsCharge.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (dsCharge.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(dsCharge.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (dsCharge.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(dsCharge.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }


                ///////////////////////changes by avanish////////////////////////////
                if (flagEMROderDefaultDoctorSelection.Equals("Y"))
                {
                    if (dsCharge.Tables[0].Rows[0]["DOCTORID"].ToString().Trim() != "")
                    {
                        DOCTORID = common.myInt(dsCharge.Tables[0].Rows[0]["DOCTORID"].ToString().Trim());
                        DataSet ds3 = dal.FillDataSet(CommandType.Text, "select 'Doctor_Name'=isnull(t.Name+' ','')+isnull(e.FirstName,'')+isnull(' '+e.MiddleName,'')+isnull(' '+e.LastName,'') from Employee e inner join TitleMaster t on e.TitleId=t.TitleID where e.id='" + DOCTORID + "'");
                        dnm = ds3.Tables[0].Rows[0]["Doctor_Name"].ToString();
                    }
                }
                ///////////////////////changes by avanish////////////////////////////

            }
            ///////////////////////changes by avanish////////////////////////////
            string doctorid = "0", dnm1 = "";
            int f = 0, did = 0;
            if (Request.QueryString["For"] != null && Request.QueryString["DoctorId"] != null)
            {
                doctorid = common.myStr(Request.QueryString["DoctorId"]);
                f = 1;
            }
            else
            {
                doctorid = Session["EmployeeId"].ToString();

                if (flagEMROderDefaultDoctorSelection.Equals("Y"))
                {

                    DataSet ds2 = dal.FillDataSet(CommandType.Text, "select 'Employee_Type' =EmployeeType from EmployeeType where id =(select EmployeeType from Employee where id='" + doctorid + "')");
                    string emptype = ds2.Tables[0].Rows[0]["Employee_Type"].ToString();

                    string[] drlst = { "D", "TM", "SR", "AN", "OD", "LD" };
                    if (drlst.Contains(emptype))
                    {
                        f = 1;
                        DataSet ds3 = dal.FillDataSet(CommandType.Text, "select 'Doctor_Name'=isnull(t.Name+' ','')+isnull(e.FirstName,'')+isnull(' '+e.MiddleName,'')+isnull(' '+e.LastName,'') from Employee e inner join TitleMaster t on e.TitleId=t.TitleID where e.id='" + doctorid + "'");
                        dnm1 = ds3.Tables[0].Rows[0]["Doctor_Name"].ToString();
                    }
                }
            }

            if (flagEMROderDefaultDoctorSelection.Equals("Y"))
            {
                string Drname = "";
                if (f == 1)
                {
                    Drname = dnm1;
                    did = common.myInt(doctorid);
                }
                else
                {
                    Drname = dnm;
                    did = Convert.ToInt32(DOCTORID);
                }
                Session["DN"] = Drname;
                Session["Did"] = did;
            }


            ///////////////////////changes by avanish////////////////////////////


            double dblServiceAmt = 0.0;
            bool IsLinkService = false;
            bool IsBlocked = false;
            DataTable Newdata = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = order.GetServiceDescriptionForOrderpage(common.myInt(Session["HospitallocationId"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), serviceId,
                                                    0, CompanyId, InsuranceId, CardId, 1, 0, OrderSetId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsServiceBlocked"]))
                        {
                            IsBlocked = true;
                        }
                    }
                    catch
                    {
                    }

                    if (common.myBool(IsBlocked))
                    {
                        if (!common.myBool(ViewState["Yes"]))
                        {
                            if (!common.myBool(ViewState["IsAllowToAddBlockedService"]))
                            {
                                Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                                return;
                            }
                            divConfirmation.Visible = true;
                            return;
                        }
                    }



                    if (ViewState["GridData"] != null)
                    {
                        if (ViewState["GridData"] != string.Empty)
                        {
                            dt = (DataTable)ViewState["GridData"];
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt = CreateTable();
                    }

                    //if (common.myInt(ViewState["EditIndx"]) > -1 && ViewState["EditIndx"] != null)
                    //{

                    //}
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (common.myInt(dt.Rows[0].ItemArray[0]).Equals(0))
                        {
                            dt.Rows.RemoveAt(0);
                        }


                    }
                    foreach (DataRow data in ds.Tables[0].Rows)
                    {

                        if (serviceId > 0)
                        {
                            dt.DefaultView.RowFilter = "ServiceId=" + common.myInt(serviceId);
                        }
                        else
                        {
                            dt.DefaultView.RowFilter = "ServiceId=" + common.myInt(data["ServiceId"]);
                        }

                        DataView dvdup = new DataView();
                        dvdup = dt.Copy().DefaultView;
                        dvdup.RowFilter = " ServiceId = " + common.myInt(data["ServiceId"]);
                        ViewState["CommonServicesAlreadyExist"] = null;
                        if (dvdup.ToTable().Rows.Count > 0)
                        {
                            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            //lblMessage.Text = "Service : "+common.myStr(lnkServiceName.Text)+" already exist !";
                            ddlService.Text = "";
                            ddlService.ClearSelection();
                            ddlService.EmptyMessage = "";
                            Page.SetFocus(ddlService);
                            ViewState["CommonServicesAlreadyExist"] = "1";
                            ViewState["CommonServicesNameAlreadyExist"] = common.myStr(ViewState["CommonServicesNameAlreadyExist"]) + " , " + common.myStr(data["ServiceName"].ToString());
                            //  return;
                        }
                        if (dt.DefaultView.Count == 0 || serviceId == 0)
                        {
                            maxId = maxId + 1;
                            if (common.myInt(ViewState["EditIndx"]) > 0)
                            {
                                if (ViewState["ESrNo"] != null)
                                {
                                    dr["Sno"] = common.myInt(ViewState["ESrNo"]) + 1;
                                }
                            }
                            else
                            {
                                dr["SNo"] = maxId;
                            }
                            dr["DetailId"] = 0;
                            dr["ServiceId"] = data["ServiceId"].ToString();
                            dr["OrderId"] = 0;
                            dr["UnderPackage"] = common.myInt(0);
                            dr["PackageId"] = common.myInt(0);
                            dr["ServiceType"] = data["ServiceType"].ToString();
                            dr["DoctorID"] = common.myInt(0);
                            if (common.myStr(data["ServiceType"].ToString()).ToUpper().Equals("CL"))
                            { dr["DoctorRequired"] = "True"; }
                            else
                            { dr["DoctorRequired"] = common.myStr(0); }
                            dr["DepartmentId"] = common.myInt(0);


                            dr["ServiceName"] = common.myStr(data["ServiceName"].ToString());



                            dr["Units"] = common.myInt(1);

                            dr["Charge"] = common.myDec(common.myDbl(data["Charges"]));
                            dr["ServiceAmount"] = common.myDec(common.myDbl(data["Charges"]));
                            dr["DoctorAmount"] = common.myDec(0);
                            dr["ServiceDiscountPercentage"] = common.myDec(0);
                            dr["ServiceDiscountAmount"] = common.myDec(0);

                            dr["DoctorDiscountPercentage"] = common.myDec(0);
                            dr["DoctorDiscountAmount"] = common.myDec(0);
                            //  dr["TotalDiscount"] = common.myDec(common.myDec(hshServiceDetail["DiscountNAmt"]) + common.myDec(hshServiceDetail["DiscountDNAmt"])).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                            dr["TotalDiscount"] = common.myDec(0);

                            // dr["AmountPayableByPatient"] = common.myDec(hshServiceDetail["PatientNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                            dr["AmountPayableByPatient"] = common.myDec(common.myDbl(data["Charges"]));

                            //dr["AmountPayableByPayer"] = common.myDec(hshServiceDetail["PayorNPayable"]).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                            dr["AmountPayableByPayer"] = common.myDec(0);


                            dr["IsPackageMain"] = common.myInt(0);
                            dr["IsPackageService"] = common.myInt(0);

                            dr["MainSurgeryId"] = common.myInt(0);
                            dr["IsSurgeryMain"] = common.myInt(0);
                            dr["IsSurgeryService"] = common.myInt(0);
                            dr["ServiceRemarks"] = common.myStr(txtMiscellaneousRemarks.Text);

                            dr["ResourceId"] = "  ";
                            dr["ServiceStatus"] = "  ";
                            dr["IsExcluded"] = common.myBool(data["IsExcluded"]);

                            dr["IsApprovalReq"] = common.myStr(0);
                            dr["IsPriceEditable"] = common.myStr(0);
                            // dr["NetCharge"] = common.myDec(totPayable.ToString("F" + common.myInt(hdnDecimalPlaces.Value)));

                            dr["NetCharge"] = common.myDec(data["Charges"]);

                            dr["ChargePercentage"] = 0;
                            dr["CopayAmt"] = 0;
                            dr["CopayPerc"] = 0;
                            dr["IsCoPayOnNet"] = 0;
                            dr["DeductableAmount"] = 0;
                            dr["Stat"] = common.myStr(chkStat.SelectedValue) == "STAT" ? 1 : 0;
                            dr["Urgent"] = common.myStr(chkStat.SelectedValue) == "URGENT" ? 1 : 0;

                            dr["ServiceInstructions"] = common.myStr(0);

                            if (RadDateTimePicker1.SelectedDate != null)
                                dr["InvestigationDate"] = common.myStr(common.myDate(RadDateTimePicker1.SelectedDate).ToString("dd/MM/yyyy HH:mm tt"));
                            else
                                dr["InvestigationDate"] = DBNull.Value;
                            dr["Remarks"] = common.myStr("");
                            dr["POCRequest"] = common.myBool(chkPOCRequest.Checked);
                            dr["IsBioHazard"] = common.myBool(chkIsBioHazard.Checked);
                            dr["IsReadBack"] = common.myBool(chkIsReadBack.Checked);
                            dr["ReadBackNote"] = txtIsReadBackNote.Text;
                            dr["OutsourceInvestigation"] = common.myBool(0);


                            if (common.myInt(hdnGlobleStationId.Value) > 0)
                            {
                                dr["StationId"] = common.myStr(hdnGlobleStationId.Value);
                            }

                            dr["AssignToEmpId"] = common.myInt(ddlAssignToEmpId.SelectedValue);
                            dr["IsAddOnTest"] = common.myBool(chkIsAddOnTest.Checked);
                            dr["ServicelimitAmount"] = common.myDec(0);
                            dr["IsServiceApprovalRequired"] = common.myBool(0);

                            dr["FromDate"] = Convert.ToDateTime(dtFromDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
                            dr["ToDate"] = Convert.ToDateTime(dtToDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
                            dr["EquipmentType"] = common.myBool(hndEquipmentType.Value);

                            dr["TariffId"] = common.myInt(0);
                            dr["IsERService"] = IsEmergency;


                            if (ViewState["CommonServicesAlreadyExist"] == null)
                            {
                                dt.Rows.Add(dr.ItemArray);
                            }


                            //if (common.myInt(serviceId) > 0)P
                            //{
                            //    break;
                            //}
                        }
                    }
                    if (!common.myStr(ViewState["CommonServicesNameAlreadyExist"]).Equals(string.Empty) && ViewState["CommonServicesNameAlreadyExist"] != null)
                    {
                        ViewState["CommonServicesNameAlreadyExist"] = common.myStr(ViewState["CommonServicesNameAlreadyExist"]).Remove(0, 3);
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Service : " + common.myStr(ViewState["CommonServicesNameAlreadyExist"]) + " already exist !";
                        ViewState["CommonServicesNameAlreadyExist"] = string.Empty;
                    }
                    //dt.Rows.Add(dr);
                    ViewState["OPServicesInv_"] = dt;
                    gvService.DataSource = dt;
                    gvService.DataBind();
                    ViewState["DuplicateService"] = 0;
                    lblMessage.Text = "";
                    dvConfirmPrintingOptions.Visible = false;
                    divExcludedService.Visible = false;
                    ddlService.Text = "";
                    ddlService.ClearSelection();
                    ddlService.EmptyMessage = "";

                    dtFromDate.SelectedDate = null;
                    dtFromMinutes.SelectedIndex = 0;
                    dtToDate.SelectedDate = null;
                    dtToMinutes.SelectedIndex = 0;
                    //  dtEquipmentType.Visible = false;

                    ViewState["EditIndx"] = null;
                    Page.SetFocus(ddlService);
                    foreach (ListItem item in chkStat.Items)
                    {
                        item.Selected = false;
                    }
                    chkPOCRequest.Checked = false;
                    chkIsBioHazard.Checked = false;
                    chkIsAddOnTest.Checked = false;
                    ClearControl();


                    //Newdata.DefaultView.RowFilter = "";
                    ////dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT','RF' )";
                    ////ViewState["Service"] = dvmain.ToTable(); //sa
                    //ViewState["OPServicesInv_"] = Newdata;
                    //gvService.DataSource = Newdata;
                    //gvService.DataBind();
                    ViewState["DuplicateService"] = 0;

                    //Declare an object variable. 
                    //object sumObject;
                    //sumObject = Newdata.Compute("Sum(Charges)", "");
                    //lblTotCharges.Text = common.myStr(sumObject);
                    //ViewState["GridData"] = Newdata;
                    //txtUnit.Text = "1";
                    //foreach (ListItem item in chkStat.Items)
                    //{
                    //    item.Selected = false;
                    //}
                    //hdnStatValueContainer.Value = "0";
                    //hdnIsServiceRemarkMandatory.Value = "0";
                    //ViewState["EditIndx"] = null;
                    //chkIsBioHazard.Checked = false;
                    //if (serviceId == 0 || OrderSetId > 0)
                    //{
                    //    IsLinkService = false;
                    //}

                    //if (IsLinkService)
                    //{
                    //    addServiceFromLinkService(serviceId, common.myDbl(dblServiceAmt), CompanyId, InsuranceId, CardId);
                    //}
                }
                else
                {
                    //BindBlnkGrid();
                }
            }
            else
            {
                //BindBlnkGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //yogesh 3/10/2022
    protected void cmdfavorite_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            string[] stringSeparators_ShowDia = new string[] { "##" };
            string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);
            string sResult = order.SaveFavorite(common.myInt(Session["EmployeeId"]), common.myInt(serviceId[0]),
                                    common.myInt(Session["UserId"]));
            lblMessage.Text = sResult;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindFevoriteGrid("");
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.ToString();
        }
    }


    //yogesh 3/10/2022
    public DataSet GetFavorites(int iDoctorId, int iDepartmentId, int FacilityId, string sTextSearch)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {

            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intDepartmentId", iDepartmentId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvSearchCriteria", "%" + sTextSearch + "%");
            HshIn.Add("@intExternalCenterId", null);
            return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", HshIn);
        }

        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }

    }

    //yogesh 3/10/2022
    private void BindFevoriteGrid(string sTextSearch)
    {
        DataSet dt = new DataSet();
        //BaseC.EMRBilling order = new BaseC.EMRBilling(sConString);

        try
        {
            dt = GetFavorites(common.myInt(Session["EmployeeId"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityId"]), sTextSearch);

            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    gvfaveroite.DataSource = dt.Tables[0];
                    gvfaveroite.DataBind();
                }
                else
                {
                    gvfaveroite.DataSource = bindBlankfevoritekgrid();
                    gvfaveroite.DataBind();
                }
            }
            else
            {
                gvfaveroite.DataSource = bindBlankfevoritekgrid();
                gvfaveroite.DataBind();
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();

        }
    }

    //yogesh 3/10/2022
    protected DataTable bindBlankfevoritekgrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("ServiceName");
        dt.Columns.Add("ServiceType");
        dt.Columns.Add("IsLinkService");
        dt.Columns.Add("DoctorId");
        DataRow dr = dt.NewRow();
        dr["ServiceId"] = 0;
        dt.Rows.Add(dr);
        ViewState["ServiceName"] = dt;
        return dt;
    }

    //yogesh 3/10/2022
    public DataSet GetValidateServicelimit(int FacilityId, int serviceId, int EncId, int CompanyId)
    {
        DataSet datas = new DataSet();
        Hashtable hshInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            hshInput.Add("@FacilityId", FacilityId);
            hshInput.Add("@ServiceId", serviceId);
            hshInput.Add("@EncounterId", EncId);
            hshInput.Add("@Companyid", CompanyId);
            datas = dl.FillDataSet(CommandType.StoredProcedure, "uspgetValidateServicelimit", hshInput);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

            dl = null; hshInput = null;
        }
        return datas;
    }

    //yogesh 3/10/2022
    private void GetService(int serviceId, string servicetype, bool isServiceLink, string ServiceName)
    {
        try
        {
            UpdateDataTable();
            if (common.myStr(serviceId).Equals(""))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Service does not exist, Please select from list !";
                Alert.ShowAjaxMsg("Service does not exist, Please select from list !", Page.Page);
                return;
            }


            if (common.myInt(ViewState["EncId"]) > 0)
            {
                DataSet datas = new DataSet();


                datas = GetValidateServicelimit(common.myInt(Session["FacilityId"]), common.myInt(serviceId), common.myInt(ViewState["EncId"]), common.myInt(ViewState["CompanyId"]));
                int Servicelimit = 0;

                if (datas.Tables.Count > 0 && datas.Tables[0].Rows.Count > 0)
                {
                    //if (string.Equals(IsvalidateCompanyDailyServiceUnitlimit.Trim().ToUpper(), "Y"))
                    //{
                    //    Servicelimit = common.myInt(datas.Tables[0].Rows[0]["ServiceLimit"]);
                    //    if (datas.Tables[0].Rows.Count > Servicelimit)
                    //    {
                    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //        lblMessage.Text = "Service limit exceed..";
                    //        Alert.ShowAjaxMsg("Service limit exceed..", Page);
                    //        return;
                    //    }
                    //}
                }
                GetOponeServive(serviceId, servicetype, isServiceLink, ServiceName);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
        }
        finally
        {
            //obj = null;
        }
    }

    //yogesh 3/10/2022

    protected void txtFevSearch_TextChanged(object sender, EventArgs e)
    {
        BindFevoriteGrid(common.myStr(txtFevSearch.Text.Trim()));
    }

    //yogesh 3/10/2022
    protected void imgSearch_Click(object sender, ImageClickEventArgs e)
    {
        BindFevoriteGrid(common.myStr(txtFevSearch.Text.Trim()));
    }

    //yogesh 3/10/2022

    public void DeletefavroiteService(int iServiceId, int iEncodedBy, int iDoctorId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeleteFavService", HshIn);
        }
        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }
    }


    //yogesh 3/10/2022
    protected void gvfaveroite_RowCommand1(object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMRBilling BaseBill = new BaseC.EMRBilling(sConString);
        int iEncodedBy = 0, iServiceId = 0, iDoctorId = 0;
        try
        {
            if (e.CommandName == "FavDel")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                iEncodedBy = common.myInt(Session["UserId"]);
                iServiceId = common.myInt(e.CommandArgument);
                if (iServiceId > 0 && iEncodedBy > 0 && common.myInt(Session["EmployeeId"]) > 0)
                {
                    DeletefavroiteService(iServiceId, iEncodedBy, common.myInt(Session["EmployeeId"]));
                    BindFevoriteGrid("");
                }
            }
            else if (e.CommandName.Equals("AddToList"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                iServiceId = common.myInt(e.CommandArgument);
                HiddenField hdnServiceType = (HiddenField)row.FindControl("hdnServiceType");
                HiddenField hdnServiceLink = (HiddenField)row.FindControl("hdnServiceLink");
                Label lblServiceNName = (Label)row.FindControl("lblServiceNName");
                if (iServiceId > 0)
                {
                    GetService(iServiceId, hdnServiceType.Value, common.myBool(hdnServiceLink.Value), lblServiceNName.Text);
                    BindFevoriteGrid("");

                }
            }
        }
        catch (Exception Ex)
        {
            Ex.ToString();
        }
        finally { BaseBill = null; }
    }

    //yogesh 3/10/2022
    private void GetOponeServive(int Serviceid, string Servicetype, bool ServiceLink, string ServiceName)
    {
        if (ViewState["OPServicesInv_"] == null)
            return;
        DataTable dt = new DataTable();
        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        BaseC.EMRBilling.clsOrderNBill BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        Hashtable hshServiceDetail = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            int maxId = 0;

            dt = (DataTable)ViewState["OPServicesInv_"];
            if (dt.Rows.Count > 0)
            {
                if (common.myInt(dt.Rows[0][2]).Equals(0))
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
                //Check duplicate service
                DataView dvdup = new DataView();
                dvdup = dt.Copy().DefaultView;
                dvdup.RowFilter = " ServiceId = " + common.myStr(Serviceid);
                if (dvdup.ToTable().Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Service already exist !";
                    ddlService.Text = "";
                    ddlService.ClearSelection();
                    ddlService.EmptyMessage = "";
                    Page.SetFocus(ddlService);
                    return;
                }
            }
            if (common.myStr(Servicetype).Equals("OPP"))
            {
                ds = objBilling.getPackageServiceDetails(common.myInt(Serviceid), common.myInt(Session["HospitalLocationID"]),
                    common.myInt(Session["FacilityId"]), common.myInt(hdnCompanyId.Value),
                    common.myInt(hdnInsuranceId.Value), common.myInt(hdnCardId.Value),
                    common.myStr(ViewState["OP_IP"]), common.myInt(ViewState["Regid"]), common.myInt(ViewState["EncId"]), 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!ds.Tables[0].Columns.Contains("DeductableAmount"))
                        {
                            ds.Tables[0].Columns.Add("DeductableAmount", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("IsCoPayOnNet"))
                        {
                            ds.Tables[0].Columns.Add("IsCoPayOnNet", typeof(double));
                        }
                        if (!ds.Tables[0].Columns.Contains("CopayPerc"))
                        {
                            ds.Tables[0].Columns.Add("CopayPerc", typeof(double));
                        }
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["DeductableAmount"] = 0;
                            dr["IsCoPayOnNet"] = 0;
                            dr["CopayPerc"] = 0;
                            dt.ImportRow(dr);
                        }
                        ViewState["OPServicesInv_"] = dt;
                        gvService.DataSource = dt;
                        gvService.DataBind();
                        ddlService.Text = "";
                        ddlService.ClearSelection();
                        ddlService.EmptyMessage = "";
                        Page.SetFocus(ddlService);
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Package Amount !";
                }
            }
            else if (common.myBool(false) && !common.myBool(ServiceLink))
            {
                BaseC.EMROrders objEMROrders = new BaseC.EMROrders(sConString);
                DataTable dt1 = objEMROrders.GetOrderSets(common.myInt(Serviceid));
                addServiceFromOrderSet(dt1);
            }
            else
            {
                ViewState["ServiceId"] = common.myInt(Serviceid);
                ViewState["ServiceName"] = common.myStr(ServiceName);
                int IsEmergency = (chkIsEmergency.Visible && chkIsEmergency.Checked) ? 1 : 0;
                int Isbedsidecharges = (chkisbedsidecharges.Visible && chkisbedsidecharges.Checked) ? 1 : 0;
                hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                                          common.myInt(Session["FacilityId"]),
                                                          common.myInt(hdnCompanyId.Value),
                                                          common.myInt(hdnInsuranceId.Value),
                                                          common.myInt(hdnCardId.Value),
                                                          common.myStr(ViewState["OP_IP"]),
                                                          common.myInt(Serviceid),
                                                          common.myInt(ViewState["Regid"]),
                                                          common.myInt(ViewState["EncId"]), 0, 0,
                                                          IsEmergency, Convert.ToDateTime(dtOrderDate.SelectedDate).ToString("yyyy-MM-dd"), Isbedsidecharges);


                if (common.myBool(hshServiceDetail["IsBlocked"]) && common.myBool(ViewState["Yes"]) == false)
                {
                    if (common.myBool(ViewState["IsAllowToAddBlockedService"]) == false)
                    {
                        Alert.ShowAjaxMsg("Not authorized to Add Service. Selected service is blocked for this company.", this.Page);
                        return;
                    }
                    divConfirmation.Visible = true;
                    return;
                }
                if ((common.myStr(hshServiceDetail["IsExcluded"]) == "True") || (common.myStr(hshServiceDetail["IsExcluded"]).Equals("1")))
                {
                    divExcludedService.Visible = true;
                    lblExcludedServiceName.Text = common.myStr(ViewState["ServiceName"]);
                }

                else if (common.myBool(common.myStr(hshServiceDetail["IsServiceApprovalRequired"])))
                {
                    //yogesh 04/10/2022
                   string serviceName ="Service Name :"+ " "+ common.myStr(ViewState["ServiceName"]);
                   string serviceLimit = "Selected service amount " + common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNChr"])).ToString("N2") + " is not under the define limit (" + common.myDec(common.myStr(hshServiceDetail["ServicelimitAmount"])).ToString("N2") + ")";
                   
                    //string s = "Swal.fire({" +
                    //      "title: '"+serviceName+"'," +
                    //      "text: '"+ serviceLimit + "', " +
                    //      "icon: 'warning'," +
                    //      "showCancelButton: true," +
                    //      "confirmButtonColor: '#3085d6'," +
                    //      "cancelButtonColor: '#d33'," +
                    //      "confirmButtonText: 'Yes, Proceed!'" +
                    //      "}).then((result) => {" +
                    //      "if (result.isConfirmed)" +
                    //      "{" +
                    //            '"'+ ProceedConfirmYo() + '"' +
                    //      "}" +
                    //  "})";

                   //string s =  "Swal.fire({ title: 'Are you sure?', text: 'You wont be able to revert this', icon: 'warning', showCancelButton: true, confirmButtonColor: '#3085d6',cancelButtonColor: '#d33', confirmButtonText:  'Yes, delete it!'}).then((result) => {if (result.isConfirmed)  {  '"+ProceedConfirmYo()+"'   } })";


                   // ScriptManager.RegisterStartupScript(this, GetType(), "showalertYogesh", s, true);

                    //old 
                    divServicelimit.Visible = true;
                    lblSName.Text = common.myStr(ViewState["ServiceName"]);
                    lblServcieLimit.Text = "Selected service amount " + common.myDec(common.myDec(hshServiceDetail["NChr"]) + common.myDec(hshServiceDetail["DNChr"])).ToString("N2") + " is not under the define limit (" + common.myDec(common.myStr(hshServiceDetail["ServicelimitAmount"])).ToString("N2") + ")";


                }
                else if (common.myInt(ViewState["DuplicateService"]).Equals(1))
                {
                    dvConfirmPrintingOptions.Visible = true;
                }
                else
                {
                    addService();
                    dvConfirmPrintingOptions.Visible = false;
                    divExcludedService.Visible = false;
                    divServicelimit.Visible = false;
                }


            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objBilling = null;
            BaseBill = null;
            hshServiceDetail = null;
            ds.Dispose();
        }
    }


    //yogesh 04/10/2022
    private string ProceedConfirmYo()
    {

        if (ViewState["OPServicesInv_"] == null)
            return "";
        DataTable dt = new DataTable();
        try
        {
            divServicelimit.Visible = false;
            dt = (DataTable)ViewState["OPServicesInv_"];
            if (common.myInt(ViewState["DuplicateService"]).Equals(1))
            {
                dvConfirmPrintingOptions.Visible = true;
            }
            else
            {
                addService();
                divServicelimit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }


        return "Yogesh";
    }

}
