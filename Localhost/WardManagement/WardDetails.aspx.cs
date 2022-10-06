using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

public partial class WardManagement_WardDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private enum GridWard : byte
    {
        Patient = 2,
        AgeGender = 3,
        RegistrationNo = 4,
        EncounterNo = 5,
        EncounterDate = 6,
        BedNo = 7,
        DoctorName = 8,
        EncounterStatus = 9,
        WardName = 10,
        PatientAddress = 11,
        SecondaryDoctorName = 12,
        Company = 13,
        CompanyType = 14,
        Notification = 15
    }

    private enum GridEncounter : byte
    {
        Select = 1,
        OPIP = 2,
        RegistrationNo = 3,
        EncounterNo = 4,
        PatientName = 5,
        GenderAge = 6,
        DoctorName = 7,
        CurrentBedNo = 8,
        AdmissionDate = 9,
        DischargeStatus = 10,
        EncounterStatus = 11,
        CompanyName = 12,
        PhoneHome = 13,
        MobileNo = 14,
        DOB = 15,
        PatientAddress = 16,
        REGID = 17,
        ENCID = 18,
        CompanyCode = 19,
        InsuranceCode = 20,
        CardId = 21,
        RowNo = 22,
        DischargeDate = 23,
        FinalizeDate = 24,
        Status = 25
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]),
                        "IMAGEmateEMRScannedDocumentPath,IsShowWardComputerTaggingDetails", sConString);

                if (collHospitalSetupValues.ContainsKey("IMAGEmateEMRScannedDocumentPath"))
                    ViewState["IMAGEmateEMRScannedDocumentPath"] = collHospitalSetupValues["IMAGEmateEMRScannedDocumentPath"];
                if (collHospitalSetupValues.ContainsKey("IsShowWardComputerTaggingDetails"))
                    ViewState["IsShowWardComputerTaggingDetails"] = collHospitalSetupValues["IsShowWardComputerTaggingDetails"];

                hdnFacilityName.Value = common.myStr(Session["FacilityName"]);

                txtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                txtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);

                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;

                hdnApolloDhaka.Value = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ChangeDischargeProcessApolloDhaka", sConString));

                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "isRequiredAllFacilityBed", sConString).Equals("Y"))
                {
                    ViewState["isRequiredAllFacilityBed"] = "Y";
                }

                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowBalanceAmtInWard", sConString).Equals("Y"))
                {
                    LblProvisionalBillAmt.Visible = true;
                    jlblProvisionalBillAmt.Visible = true;
                    LblDepositAmt.Visible = true;
                    jlblDepositAmt.Visible = true;
                    LblApprovedAmt.Visible = true;
                    jlblApprovedAmt.Visible = true;
                    LblBalanceAmt.Visible = true;
                    jlblBalanceAmt.Visible = true;
                }

                bindControl();
                bindStation();
                fillSampleCollectedStatus();
                BindEncounterStatus();
                BindSpeciliazation();
                BindProvider();
                chkAll_CheckedChanged(null, null);
                string IsWardAckRequiredForBedTransfer = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsWardAckRequiredForBedTransfer", sConString));
                ViewState["IsWardAckRequiredForBedTransfer"] = IsWardAckRequiredForBedTransfer;
                if (IsWardAckRequiredForBedTransfer.Equals("Y"))
                {
                    lnkBedTransfer.Visible = true;
                    lblNoOfBedTransfer.Visible = true;
                }
                else
                {
                    lnkBedTransfer.Visible = false;
                    lblNoOfBedTransfer.Visible = false;
                }
                ddlName.SelectedValue = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultSearchCriteriaForWard", sConString));

                string isInitialAssessmentCompulsory = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isInitialAssessmentCompulsory", sConString));
                ViewState["isInitialAssessmentCompulsory"] = isInitialAssessmentCompulsory;
                ViewState["IsStopIndentForGeneralWard"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsStopIndentForGeneralWard", sConString));
                //Add By Himanshu On Date 30/03/2022
                ViewState["IsPrintCustomDischargeAndDeathSummary"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsPrintCustomDischargeAndDeathSummary", sConString));


            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    private void BindEncounterStatus()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        try
        {
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "Encounter", string.Empty);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "Code NOT IN ('VT','S','RF')";

                    RadComboBoxItem lst;
                    for (int idx = 0; idx < dv.ToTable().Rows.Count; idx++)
                    {
                        lst = new RadComboBoxItem();
                        lst.Attributes.Add("style", "background-color:" + common.myStr(dv.ToTable().Rows[idx]["StatusColor"]) + ";");

                        lst.Value = common.myStr(dv.ToTable().Rows[idx]["StatusId"]);
                        lst.Text = common.myStr(dv.ToTable().Rows[idx]["Status"]);
                        ddlEncounterStatus.Items.Add(lst);
                    }
                    dv.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objval = null;
            ds.Dispose();
        }
    }
    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            dvDischarge.Visible = false;
            gvEncounter.DataSource = null;
            gvEncounter.DataBind();
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;

            //ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]));

            if (common.myStr(ViewState["IsShowWardComputerTaggingDetails"]).Equals("Y"))
            {
                ds = objWD.GetWardComputerTagging(common.myInt(Session["HospitalLocationID"]), true, common.myStr(Environment.MachineName), common.myInt(Session["FacilityId"]));

            }
            else
            {
                ds = objWD.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    if (common.myStr(ViewState["isRequiredAllFacilityBed"]) == "Y")
                    {
                        ds = objadt.GetWard(common.myInt(0));
                    }
                    else
                    {
                        ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
                    }
                }
            }

            ddlWard.DataSource = ds.Tables[0];
            //ddlWard.DataTextField = "StationName";
            //ddlWard.DataValueField = "ID";
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ds.Tables[0].DefaultView.RowFilter = "FavouriteWard = '1'";
            DataTable dt = (ds.Tables[0].DefaultView).ToTable();
            if (dt.Rows.Count > 0)
            {
                List<Int16> lstWARDID = dt.Rows.OfType<DataRow>().Select(dr => (Int16)dr["WardId"]).ToList();
                // List<string> lstWARDID = new List<string>();
                // lstWARDID = (List<string>)Session["WARDDETAILSFINDWARDID"];
                foreach (RadComboBoxItem currentItem in ddlWard.Items)
                {
                    foreach (int WardValueChecked in lstWARDID)
                    {
                        if (currentItem.Value.Equals(common.myStr(WardValueChecked)))
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
            }
            else
            {
                if (common.myLen(Session["WARDDETAILSFINDWARDID"]) > 0)
                {
                    try
                    {
                        List<string> lstWARDID = new List<string>();
                        lstWARDID = (List<string>)Session["WARDDETAILSFINDWARDID"];
                        foreach (RadComboBoxItem currentItem in ddlWard.Items)
                        {
                            currentItem.Checked = true;
                            foreach (string WardValueChecked in lstWARDID)
                            {
                                if (currentItem.Value.Equals(WardValueChecked))
                                {
                                    currentItem.Checked = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (RadComboBoxItem currentItem in ddlWard.Items)
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
                else
                {
                    foreach (RadComboBoxItem currentItem in ddlWard.Items)
                    {
                        currentItem.Checked = true;
                    }
                }
            }


            // ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            //ddlWard.SelectedIndex = 0;


            //  BindGroupTaggingMenu();
            BindStatus();
        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }
    private void bindStation()
    {
        DataSet ds = new DataSet();
        //BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            dvDischarge.Visible = false;
            gvEncounter.DataSource = null;
            gvEncounter.DataBind();
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));

            ddlStation.DataSource = ds.Tables[0];
            ddlStation.DataTextField = "StationName";
            ddlStation.DataValueField = "ID";
            ddlStation.DataBind();
            ddlStation.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlStation.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            // objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }
    protected void ddlWard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                fillData();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void ddlStation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                fillData();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        DataSet dsp = new DataSet();

        try
        {
            //ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
            //    30//common.myInt(Session["ModuleId"])

            if (chkDischarge.Checked)
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                             common.myInt(Session["ModuleId"]), "WARDDETAIL-Discharge");
            }
            else
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "WARDDETAILS");
            }
            if (common.myInt(ds.Tables[0].Rows.Count).Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            //dsp = ViewState["PatientDetails"];

            //DataView dv = dsp.Tables[0].DefaultView;
            //dv.RowFilter ="ISNULL(IsAcknowledged,0) = 0";

            RadMenuItem menu;
            menuStatus.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                menu = new RadMenuItem();
                menu.Value = common.myStr(ds.Tables[0].Rows[i]["OptionId"]);
                menu.Text = common.myStr(ds.Tables[0].Rows[i]["OptionName"]);
                menu.Attributes.Add("Code", common.myStr(ds.Tables[0].Rows[i]["OptionCode"]));
                menu.Attributes.Add("InterhospTransEnable", common.myStr(ds.Tables[0].Rows[i]["InterhospTransEnable"]));
                menuStatus.Items.Add(menu);
            }
            if (ds.Tables[0].Rows.Count <= 20)
            {
                menuStatus.DefaultGroupSettings.Height = Unit.Percentage(100);
            }
            else
            {
                menuStatus.DefaultGroupSettings.Height = Unit.Pixel(470);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            user = null;
            ds.Dispose();
            dsp.Dispose();
        }
    }
    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        DataSet dsStatus = new DataSet();
        try
        {
            string sRegNo = common.myStr(hdnMnuRegNo.Value);
            string sEncNo = common.myStr(hdnMnuEncNo.Value);
            string sRegId = common.myStr(hdnMnuRegId.Value);
            string sEncId = common.myStr(hdnMnuEncId.Value);
            string sPatName = common.myStr(hdnMnuPatName.Value);
            string sDocName = common.myStr(hdnMnuDocName.Value);
            string sEncDate = common.myStr(hdnMnuEncDate.Value);
            int iBedId = common.myInt(hdnMnuBedId.Value);
            string sBedNo = common.myStr(hdnMnuBedNo.Value);
            string sDoctorId = common.myStr(hdnMnuDoctorId.Value);
            string sWardName = common.myStr(hdnMnuWardName.Value);
            string sMobileNo = common.myStr(hdnMnuMobileNo.Value);
            //added by bhakti
            string sBillCat = common.myStr(hdnMnuBillCat.Value);
            string sPayername = common.myStr(hdnMnuPayername.Value);
            string sAgeGender = common.myStr(hdnMnuAgeGender.Value);
            string sinvoiceid = common.myStr(hdnInvoiceId.Value);
            string sWardNo = common.myStr(hdnWardNo.Value);
            bool sIsGeneralWard = common.myBool(hdnIsGeneralWard.Value);
            string SponsorIdPayorId = common.myStr(hdnMnuSponsorIdPayorId.Value);
            int AgeYear = 0;

            try
            {
                Session["IsPackagePatient"] = null;
                Session["IsPanelPatient"] = null;
                Session["AllowPanelExcludedItems"] = null;

                if (common.myInt(hdnMnuPackageId.Value) > 0)
                {
                    Session["IsPackagePatient"] = true;
                }

                Session["IsPanelPatient"] = common.myBool(hdnMnuIsPanel.Value);
                Session["AllowPanelExcludedItems"] = common.myBool(hdnAllowPanelExcludedItems.Value);
            }
            catch
            {
            }

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                            common.myStr(sRegNo), common.myStr(sEncNo), common.myInt(Session["UserId"]), 0);

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            Session["AllergiesAlert"] = common.myBool(hdnMnuAllergyAlert.Value);
            Session["MedicalAlert"] = common.myBool(hdnMnuMedicalAlert.Value);
            var isPasswordRequired = PasswordRequiredHelper.CheckIsPasswordRequiredForSecModuleOptionPages(30, common.myStr(menuStatus.SelectedItem.Attributes["Code"]), sConString);



            #region Patient Detail

            Session["OPIP"] = "I";
            Session["EncounterId"] = common.myInt(sEncId);
            Session["RegistrationID"] = common.myInt(sRegId);
            Session["EncounterDate"] = common.myStr(sEncDate);
            Session["DoctorID"] = common.myInt(sDoctorId);
            Session["RegistrationNo"] = common.myInt(sRegNo);
            Session["Regno"] = common.myStr(sRegNo);
            Session["Encno"] = common.myStr(sEncNo).Trim();
            Session["InvoiceId"] = common.myStr(sinvoiceid).Trim();
            // Session["EncounterNo"] = common.myStr(sEncNo).Trim();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["CompanyId"] = common.myStr(ds.Tables[0].Rows[0]["PayorId"]);
                        Session["SponsorId"] = common.myStr(ds.Tables[0].Rows[0]["SponsorId"]);
                        Session["InsuranceCardId"] = common.myStr(ds.Tables[0].Rows[0]["InsuranceCardId"]);
                        Session["PaymentType"] = common.myStr(ds.Tables[0].Rows[0]["PaymentType"]);
                        AgeYear = common.myInt(ds.Tables[0].Rows[0]["AgeYear"]);
                        //bhakti
                        Session["OPIP"] = common.myStr(ds.Tables[0].Rows[0]["OPIP"]);
                        Session["IsOffline"] = common.myStr(ds.Tables[0].Rows[0]["IsOffline"]);
                    }
                }
            }
            string sRegNoTitle = common.myStr(Resources.PRegistration.regno);
            string sDoctorTitle = common.myStr(Resources.PRegistration.Doctor);
            string DateTitle = common.myStr("Admission Date");
            //Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(sPatName)
            Session["PatientDetailString"] = "&nbsp;Patient Name:&nbsp;<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["PatientName"])
            + ", " + common.myStr(sAgeGender) + "</span>"
             + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sRegNo) + "</span>"
             + "&nbsp;Enc #.:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(sEncNo) + "</span>"
             + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sDocName) + "</span>&nbsp;"
             + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sEncDate) + "</span>"
             + "&nbsp;Bed No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sBedNo) + "</span>"
             + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sWardName) + "</span>"
             + "&nbsp;Mobile No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sMobileNo).Replace("\r", "").Trim().Replace("'", "") + "</span>"
             + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(sPayername) + "</span>"
               + "&nbsp;Bill Category:<span style='color: #990066;font-weight: bold;'>" + common.myStr(sBillCat) + "</span>"
             + "</b>";



            #endregion
            #region Allergy
            if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("AL"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //Response.Redirect("/EMR/Allergy/Allergy.aspx?Regno=" + common.myStr(sRegNo).Trim() + "&Encno=" + common.myStr(sEncNo).Trim(), false);
                            RadWindow1.NavigateUrl = "/EMR/Allergy/Allergy.aspx?Regno=" + common.myStr(sRegNo).Trim() + "&Encno=" + common.myStr(sEncNo).Trim() + "&From=POPUP" +
                                                        "&IsPasswordRequired=" + isPasswordRequired;
                            RadWindow1.Height = 300;
                            RadWindow1.Width = 500;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = "OnClearClientClose";
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Bed Transfer
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BT"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                    {
                        RadWindow1.NavigateUrl = "/ATD/BedTransferVenkateshwar.aspx?Regno=" + common.myStr(sRegNo).Trim() +
                                               "&encno=" + common.myStr(sEncNo).Trim() +
                                               "&bedno=" + common.myStr(sBedNo).Trim() +
                                               "&BT=WM" +
                                               "&AgeYear=" + AgeYear;
                    }
                    else
                    {
                        RadWindow1.NavigateUrl = "/ATD/BedTransfer1.aspx?Regno=" + common.myStr(sRegNo).Trim() +
                                                "&encno=" + common.myStr(sEncNo).Trim() +
                                                "&bedno=" + common.myStr(sBedNo).Trim() +
                                                "&BT=WM" +
                                                "&AgeYear=" + AgeYear +
                                                "&IsPasswordRequired=" + isPasswordRequired;
                    }

                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }

            }
            //palendra DischargeGatePass
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("GPP"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    string RptHeadText = "DISCHARGE NOTIFICATION";
                    //if (lblInfoReceivable_Refundable.Text.Equals("Receivable"))
                    //{
                    //    RptHeadText = "Discharge Gatepass";
                    //}
                    //else
                    //{
                    //    RptHeadText = "No Dues Slip";
                    //}
                    DataSet dsReleased = new DataSet();
                    dsReleased = CheckBedReleased();
                    if (dsReleased.Tables[0].Rows[0]["BedRelease"].ToString() == "0")
                    {
                        Alert.ShowAjaxMsg("Bed Not Released. So Gate Pass Report Not Generated !!", Page);
                        return;
                    }
                    else
                    {
                        RadWindow1.NavigateUrl = "/EMRReports/PrintReport.aspx?RegNo= " + sRegNo + "&IpNo= " + sEncNo + "&RptHeadText=" + RptHeadText + "&PrintType=DischargeNotification";
                        RadWindow1.Height = 500;
                        RadWindow1.Width = 750;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.VisibleOnPageLoad = true;
                        RadWindow1.Modal = true;
                        //RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                        RadWindow1.VisibleStatusbar = false;
                    }
                }

            }
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BTW"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/ATD/BedTransfer1.aspx?Regno=" + common.myStr(sRegNo).Trim() +
                                            "&encno=" + common.myStr(sEncNo).Trim() +
                                            "&bedno=" + common.myStr(sBedNo).Trim() +
                                            "&AgeYear=" + AgeYear +
                                            "&IsPasswordRequired=" + isPasswordRequired;

                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = string.Empty;//"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }

            }
            #endregion
            #region Drug Order
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DG"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationNew.aspx?POPUP=POPUP&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&DoctorId=" + common.myStr(sDoctorId) +
                                            "&IsPasswordRequired=" + isPasswordRequired +
                                             "&CompanyId=" + SponsorIdPayorId;
                    //"&LOCATION=WARD" ;

                    RadWindow1.Height = 620;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

                }
            }
            #endregion
            #region File Request
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("FR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/MRD/MRDFileRequest.aspx?Regno=" + sRegNo.Trim() +
                                            "&Encno=" + sEncNo.Trim() +
                                            "&RegId=" + common.myStr(sRegId).Trim() +
                                            "&EncId=" + common.myStr(sEncId).Trim() +
                                            "&POPUP=POPUP" +
                                            "&DoctorId=" + common.myStr(sDoctorId) +
                                            "&RequestFrom=WARD" +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 620;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Admission Form
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("AF"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMRReports/PrintpageAdmission.aspx?Regno=" + sRegNo.Trim() +
                                            "&Encno=" + sEncNo.Trim() +
                                            "&RegId=" + common.myStr(sRegId).Trim() +
                                            "&EncId=" + common.myStr(sEncId).Trim() +
                                            "&MASTER=NO" +
                                            "&DoctorId=" + common.myStr(sDoctorId) +
                                            "&RequestFrom=WARD" +
                                            "&RN=Admform" +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 620;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Non Drug order
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("NDO"))// Non Drug order
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    if (common.myStr(Session["EmployeeType"]).Equals("N"))
                    {
                        RadWindow1.NavigateUrl = "/ICM/AckNonDrugOrder.aspx?RegId=" + common.myStr(sRegId).Trim() + "&POPUP=POPUP&Ward=Y";
                        RadWindow1.Height = 620;
                        RadWindow1.Width = 1000;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;

                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                    else
                    {
                        RadWindow1.NavigateUrl = "/ICM/ICMNONDrugOrder.aspx?RegId=" + common.myStr(sRegId).Trim() + "&POPUP=POPUP&Ward=Y";
                        RadWindow1.Height = 620;
                        RadWindow1.Width = 1000;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
            }
            #endregion
            #region Order Consumable Items
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("CO"))
            {
                if (sIsGeneralWard && common.myStr(ViewState["IsStopIndentForGeneralWard"]) == "Y")
                {
                    Alert.ShowAjaxMsg("Authorization required!", Page);
                    return;
                }
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);
                string PrescribeMedicationForCarehospital = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                   "PrescribeMedicationForCarehospital", sConString);

                if (PrescribeMedicationForCarehospital.Equals("N") || PrescribeMedicationForCarehospital.Equals(string.Empty))
                {
                    RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?Regno=" + sRegNo.Trim() +
                                        "&Encno=" + common.myStr(sEncNo.Trim()) +
                                        "&RegId=" + common.myStr(sRegId).Trim() +
                                        "&EncId=" + common.myStr(sEncId).Trim() +
                                        "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                        "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                        "&DoctorId=" + common.myStr(sDoctorId) +
                                        "&LOCATION=WARD" +
                                        "&IsPasswordRequired=" + isPasswordRequired +
                                        "&CompanyId=" + SponsorIdPayorId;
                }
                else
                {
                    RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationCare.aspx?Regno=" + sRegNo.Trim() +
                                                           "&Encno=" + common.myStr(sEncNo.Trim()) +
                                                           "&RegId=" + common.myStr(sRegId).Trim() +
                                                           "&EncId=" + common.myStr(sEncId).Trim() +
                                                           "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                                           "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                                           "&DoctorId=" + common.myStr(sDoctorId) +
                                                           "&LOCATION=WARD" +
                                                           "&IsPasswordRequired=" + isPasswordRequired +
                                                           "&CompanyId=" + SponsorIdPayorId;
                }
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Height = 600;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = string.Empty;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            }
            #endregion
            #region Only Consumable Items
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("COONLY"))
            {
                dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);
                RadWindow1.NavigateUrl = "/EMR/Medication/ConsumableOrder.aspx?Regno=" + sRegNo.Trim() +
                                        "&Encno=" + common.myStr(sEncNo.Trim()) +
                                        "&RegId=" + common.myStr(sRegId).Trim() +
                                        "&EncId=" + common.myStr(sEncId).Trim() +
                                        "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                        "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                        "&DoctorId=" + common.myStr(sDoctorId) +
                                        "&LOCATION=WARD" +
                                        "&CompanyId=" + SponsorIdPayorId; ;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Height = 600;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = string.Empty;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            }
            #endregion
            #region Service Order
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("SV"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    //RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?From=WARD&Regid=" + common.myInt(sRegId) +
                    //                        "&RegNo=" + common.myLong(sRegNo) +
                    //                        "&EncId=" + common.myInt(sEncId) +
                    //                        "&EncNo=" + common.myInt(sEncNo) +
                    //                        "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                    //                        "&InsuranceId=0&CardId=0&PayerType="+common.myInt(ds.Tables[0].Rows[0]["PayerType"])+ "&BType=" + common.myInt(ds.Tables[0].Rows[0]["CurrentBillCategory"]) +
                    //                        "&IsPasswordRequired=" + isPasswordRequired;

                    string BType = "";
                    if (common.myStr(ds.Tables[0].Rows[0]["PaymentType"]).Equals("B"))
                        BType = "2";
                    else if (common.myStr(ds.Tables[0].Rows[0]["PaymentType"]).Equals("C"))
                        BType = "1";


                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?From=WARD&Regid=" + common.myInt(sRegId) +
                                       "&RegNo=" + common.myLong(sRegNo) +
                                       "&EncId=" + common.myInt(sEncId) +
                                       "&EncNo=" + common.myInt(sEncNo) +
                                       "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["SponsorId"]) +
                                       "&InsuranceId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                       "&CardId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                       "&PayerType=" + common.myInt(ds.Tables[0].Rows[0]["PayerType"]) +
                                           //"&BType=" + common.myInt(ds.Tables[0].Rows[0]["CurrentBillCategory"]) +
                                           "&BType=" + BType +
                                       "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("AS"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddSurgeryV1.aspx?From=WARD&RegId=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myInt(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myInt(sEncNo) +
                                            "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                            "&InsuranceId=0&CardId=0&PayerType=&BType=" +
                                            "&IsPasswordRequired=" + isPasswordRequired +
                                            "&EditPostedOTSurgery=N";
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("OPSD"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/LIS/Phlebotomy/LabRequestDetails.aspx?From=WARD&RegId=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myInt(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myInt(sEncNo) +
                                            "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                            "&InsuranceId=0&CardId=0&PayerType=&BType=" +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Add Packages
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("AP")) //Add Packages
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    //RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddPackagesV1.aspx?MPG=P22321&From=WARD&Regid=" + common.myInt(sRegId) +
                    //                        "&RegNo=" + common.myInt(sRegNo) +
                    //                        "&EncId=" + common.myInt(sEncId) +
                    //                        "&EncNo=" + common.myInt(sEncNo) +
                    //                        "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                    //                        "&InsuranceId=0" + SponsorId
                    //                        "&CardId=0" + 
                    //                        "&PayerType=" + PayerType
                    //                        "&BType=" +
                    //                        "&BillCatId=" + CurrentBillCategory
                    //                        "&PayType="; PaymentType

                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddPackagesV1.aspx?MPG=P22321&Regid=" + common.myStr(sRegId).Trim() + "&RegNo=" + common.myStr(sRegNo)
                        + "&EncId=" + common.myStr(sEncId).Trim() + "&EncNo=" + common.myStr(sEncNo).Trim() +
                        "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"])
                        + "&InsuranceId=" + common.myInt(ds.Tables[0].Rows[0]["SponsorId"])  //SponsorId
                        + "&CardId=0"
                        + "&PayerType=" + common.myInt(ds.Tables[0].Rows[0]["PayerType"])  //PayerType
                        + "&BillCatId=" + common.myInt(ds.Tables[0].Rows[0]["CurrentBillCategory"])  //CurrentBillCategory
                        + "&PayType=" + common.myStr(ds.Tables[0].Rows[0]["PaymentType"]); //PaymentType

                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Patient Dashboard
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PD"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/emr/Dashboard/PatientDashboard.aspx?Regno=" + sRegNo.Trim() +
                                            "&Encno=" + sEncNo.Trim() +
                                            "&From=POPUP&Source=IPD";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;// "SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Add Services
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("IV"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?Regid=" + common.myInt(sRegId) +
                                        "&RegNo=" + sRegNo.Trim() + "&EncId=" + common.myInt(sEncId) + "&EncNo=" + sEncNo +
                                        "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["SponsorId"]) +
                                        "&InsuranceId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                        "&CardId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                        "&PayerType=" + common.myInt(ds.Tables[0].Rows[0]["PayerType"]) +
                                        "&BType=" + common.myInt(ds.Tables[0].Rows[0]["CurrentBillCategory"]) +
                                        "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Change Encounter Status
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VC"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/ChangeEncounterPatient.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) + "&encno=" + common.myStr(sEncNo) + "&cid=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Change ProbableDischargeDate
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PDD"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/ChangeEncounterDate.aspx?EncId=" + sEncId.Trim() +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Discharge Outlier
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DOL"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/DischargeOutlier.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno = " + common.myStr(sEncNo);
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Patient Acknowledgement
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PACK"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/Patientacknowledgement.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno=" + common.myStr(sEncNo) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Drug Return
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/IPDItemReturn.aspx?MASTER=No&Regno=" + common.myStr(sRegNo) +
                                            "&Encno=" + common.myStr(sEncNo) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 940;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Doctor Visit
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DV"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/VisitPopup.aspx?Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&Page=Ward";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 940;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Result Finalization
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("RV") || common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DRV"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    string admDate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (common.myLen(sEncDate) > 9)
                    {
                        admDate = common.myDate(common.myStr(sEncDate).Substring(0, 10)).ToString("MM/dd/yyyy");
                    }

                    RadWindow1.NavigateUrl = "/LIS/Phlebotomy/ResultFinalization.aspx?RegNo=" + common.myStr(sRegNo) +
                                            "&Admisiondate=" + admDate +
                                            "&OP_IP=I&Master=WARD";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 990;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                RadWindow1.VisibleStatusbar = false;
            }
            #endregion
            #region Referral Slip
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("RS"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMR/ReferralSlip.aspx?Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=NO";

                    RadWindow1.Height = 550;
                    RadWindow1.Width = 940;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Referral Request
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("NRR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/WardManagement/ReferralRequest.aspx?Regid=" + common.myInt(sRegId) +
                                        "&RegNo=" + common.myStr(sRegNo) +
                                        "&EId=" + common.myInt(sEncId) +
                                        "&EncNo=" + common.myStr(sEncNo) +
                                        "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=NO";


                    RadWindow1.Height = 550;
                    RadWindow1.Width = 940;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region UnacknowledgedServices
            ////else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("UPS")) 
            //else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("USAI"))
            //{
            //    if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
            //    {
            //        RadWindow1.NavigateUrl = "/EMRBILLING/Popup/UnacknowledgedServicesV1.aspx?encId=" + common.myInt(sEncId);
            //        RadWindow1.Height = 600;
            //        RadWindow1.Width = 700;
            //        RadWindow1.Top = 40;
            //        RadWindow1.Left = 100;
            //        //RadWindow1.OnClientClose = "OnClientClose";
            //        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //        RadWindow1.Modal = true;
            //        RadWindow1.VisibleStatusbar = false;
            //        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            //    }
            //}
            #endregion
            #region Attach Document
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VD"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    //if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "UseFTPAttachDocument", sConString).Equals("N"))
                    //    sPath = "/emr/AttachDocument.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119&Category=PopUp";
                    //else
                    //    sPath = "/emr/AttachDocumentFTP.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119&Category=PopUp";

                    //RadWindow1.NavigateUrl = "/EMR/AttachDocument.aspx?RNo=" + common.myStr(sRegNo) +
                    //                        "&Regid=" + common.myInt(sRegId) +
                    //                        "&RegNo=" + common.myStr(sRegNo) +
                    //                        "&EncId=" + common.myInt(sEncId) +
                    //                        "&EncNo=" + common.myStr(sEncNo) +
                    //                        "&Category=PopUp&FromWard=Y&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=No";

                    RadWindow1.NavigateUrl = "/EMR/AttachDocumentFTP.aspx?RNo=" + common.myStr(sRegNo) +
                                           "&Regid=" + common.myInt(sRegId) +
                                           "&RegNo=" + common.myStr(sRegNo) +
                                           "&EncId=" + common.myInt(sEncId) +
                                           "&EncNo=" + common.myStr(sEncNo) +
                                           "&Category=PopUp&FromWard=Y&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=No";


                    RadWindow1.Height = 630;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Attach Document New
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VDN"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/AttachDocumentFTP.aspx?RNo=" + common.myStr(sRegNo) +
                                            "&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&Category=PopUp&FromWard=Y&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=No";

                    RadWindow1.Height = 630;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region VisitHistory / Past Clinical Notes Discharge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VH") || common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PCND"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    Session["SelectedCaseSheet"] = "PN";
                    RadWindow1.NavigateUrl = "/WardManagement/VisitHistory.aspx?RNo=" + common.myStr(sRegNo) +
                                            "&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&FromWard=Y&OP_IP=I&Category=PopUp&FromEMR=1";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 980;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region DischargeSummary
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DS") || common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DDS"))
            {
                // Add by Himanshu On Date 30/03/2022
                #region  Commont by kuldeep
                /*
                BaseC.clsEMR IsNewDischargeSummary = new BaseC.clsEMR(sConString);
                DataSet ds1;
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsIn = new Hashtable();

                ds1 = IsNewDischargeSummary.getDischargeSummary(common.myInt(Session["EncounterId"]));
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(ds1.Tables[0].Rows[0]["IsNewDischargeSummary"]) == true)
                        {
                            if (common.myStr(ViewState["IsPrintCustomDischargeAndDeathSummary"]).ToUpper().Contains("Y"))
                            {
                                //Response.Redirect("/EMR/Templates/Printdischargesummary.aspx?EncId = " + common.myStr(Session["EncounterId"]);;
                                RadWindow1.NavigateUrl = "/EMR/Templates/Printdischargesummary.aspx?EncId = " + common.myStr(Session["EncounterId"]);
                                RadWindow1.Height = 598;
                                RadWindow1.Width = 900;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;
                                RadWindow1.Modal = true;
                                RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                                RadWindow1.VisibleStatusbar = false;
                                //RadWindow1.NavigateUrl = "~/EMR/Templates/Default.aspx?From=POPUP&EREncounterId=" +
                                //                        "&EncId=" + common.myInt(sEncId) +
                                //                        "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy")+
                                //                        "&IsPrintCustomDischargeAndDeathSummary=" + common.myStr(ViewState["IsPrintCustomDischargeAndDeathSummary"]).ToUpper().Contains("Y");

                                //RadWindow1.Height = 610;
                                //RadWindow1.Width = 980;
                                //RadWindow1.Top = 10;
                                //RadWindow1.Left = 10;
                                //RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                                //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                //RadWindow1.Modal = true;
                                //RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                                //RadWindow1.Title = menuStatus.SelectedItem.Text;
                                //RadWindow1.VisibleStatusbar = false;
                                //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            }

                        }
                        else
                        {

                            if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                            {
                                Session["FollowUpDoctorId"] = common.myStr(sDoctorId);
                                Session["FollowUpRegistrationId"] = common.myStr(sRegId).Trim();
                                RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(sRegId) +
                                                        "&RegNo=" + common.myStr(sRegNo) +
                                                        "&EncId=" + common.myInt(sEncId) +
                                                        "&EncNo=" + common.myStr(sEncNo) +
                                                        "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy") +
                                                        "&AdmDId=" + common.myStr(sDoctorId);
                                RadWindow1.Height = 610;
                                RadWindow1.Width = 980;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;
                                RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                RadWindow1.Modal = true;
                                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                                RadWindow1.Title = menuStatus.SelectedItem.Text;
                                RadWindow1.VisibleStatusbar = false;
                                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            }
                        }
                    }
                    else
                    {
                        if (common.myStr(ViewState["IsPrintCustomDischargeAndDeathSummary"]).ToUpper().Contains("Y"))
                        {
                            RadWindow1.NavigateUrl = "/EMR/Templates/Printdischargesummary.aspx?EncId = " + common.myStr(Session["EncounterId"]);
                            RadWindow1.Height = 598;
                            RadWindow1.Width = 900;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.Modal = true;
                            RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                            RadWindow1.VisibleStatusbar = false;

                        }

                        else
                        {

                            if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                            {
                                Session["FollowUpDoctorId"] = common.myStr(sDoctorId);
                                Session["FollowUpRegistrationId"] = common.myStr(sRegId).Trim();
                                RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(sRegId) +
                                                        "&RegNo=" + common.myStr(sRegNo) +
                                                        "&EncId=" + common.myInt(sEncId) +
                                                        "&EncNo=" + common.myStr(sEncNo) +
                                                        "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy") +
                                                        "&AdmDId=" + common.myStr(sDoctorId);
                                RadWindow1.Height = 610;
                                RadWindow1.Width = 980;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;
                                RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                RadWindow1.Modal = true;
                                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                                RadWindow1.Title = menuStatus.SelectedItem.Text;
                                RadWindow1.VisibleStatusbar = false;
                                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            }
                        }
                    }

                }
*/
                #endregion
                // Optimize code  by kuldeep
                if (common.myStr(ViewState["IsPrintCustomDischargeAndDeathSummary"]).ToUpper().Contains("Y"))
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    BaseC.clsEMR IsNewDischargeSummary = new BaseC.clsEMR(sConString);
                    DataSet ds1;
                    Hashtable hsIn = new Hashtable();
                    ds1 = IsNewDischargeSummary.getDischargeSummary(common.myInt(Session["EncounterId"]));
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(ds1.Tables[0].Rows[0]["IsNewDischargeSummary"]) == true)
                            {
                                RadWindow1.NavigateUrl = "/EMR/Templates/Printdischargesummary.aspx?EncId = " + common.myStr(Session["EncounterId"]);
                                RadWindow1.Height = 598;
                                RadWindow1.Width = 900;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;
                                RadWindow1.Modal = true;
                                RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                                RadWindow1.VisibleStatusbar = false;

                            }
                        }
                    }
                }
                else
                {

                    if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                    {
                        Session["FollowUpDoctorId"] = common.myStr(sDoctorId);
                        Session["FollowUpRegistrationId"] = common.myStr(sRegId).Trim();
                        RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(sRegId) +
                                                "&RegNo=" + common.myStr(sRegNo) +
                                                "&EncId=" + common.myInt(sEncId) +
                                                "&EncNo=" + common.myStr(sEncNo) +
                                                "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy") +
                                                "&AdmDId=" + common.myStr(sDoctorId);
                        RadWindow1.Height = 610;
                        RadWindow1.Width = 980;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                        RadWindow1.Title = menuStatus.SelectedItem.Text;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
            }

            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DSN") || common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DDSN"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    Session["FollowUpDoctorId"] = common.myStr(sDoctorId);
                    Session["FollowUpRegistrationId"] = common.myStr(sRegId).Trim();
                    RadWindow1.NavigateUrl = "~/ICM/DischargeSummaryNew.aspx?Master=NO&RegId=" + common.myInt(sRegId) +
                                                            "&RegNo=" + common.myStr(sRegNo) +
                                                            "&EncId=" + common.myInt(sEncId) +
                                                            "&EncNo=" + common.myStr(sEncNo) +
                                                            "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy") +
                                                            "&AdmDId=" + common.myStr(sDoctorId);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 980;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region DeathSummary
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DE"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    Session["FollowUpDoctorId"] = common.myStr(sDoctorId);
                    Session["FollowUpRegistrationId"] = common.myStr(sRegId).Trim();
                    RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?For=DthSum&Master=NO&RegId=" + common.myInt(sRegId) +
                                                                "&RegNo=" + common.myStr(sRegNo) +
                                                                "&EncId=" + common.myInt(sEncId) +
                                                                "&EncNo=" + common.myStr(sEncNo) +
                                                                "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy") +
                                                                "&AdmDId=" + common.myStr(sDoctorId);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 980;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Diet Requisition
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DIR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/Diet/EMRPatientDietRequisition.aspx?Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&Ward=Ward" +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 880;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Diet Patient List
            //else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DPL"))
            //{
            //    if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
            //    {
            //        RadWindow1.NavigateUrl = "~/Diet/PatientDietList.aspx?POPUP=POPUP";
            //        RadWindow1.Height = 610;
            //        RadWindow1.Width = 880;
            //        RadWindow1.Top = 10;
            //        RadWindow1.Left = 10;
            //        RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
            //        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //        RadWindow1.Modal = true;
            //        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            //        RadWindow1.Title = menuStatus.SelectedItem.Text;
            //        RadWindow1.VisibleStatusbar = false;
            //        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            //    }
            //}
            #endregion
            #region Clinical Template
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("NN"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&EREncounterId=" + common.myInt(ds.Tables[0].Rows[0]["EREncounterId"]) +
                                                    "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy");
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 880;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Consolidated Amount Report
            //  else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("CA"))
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PBD"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRReports/PrintBillDetails.aspx?RN=BillPrint&EncNo=" + common.myStr(sEncNo) +
                                            "&AdmDt=" + common.myDate(sEncDate) +
                                            "&RegId=" + common.myInt(sRegId) +
                                            "&EncId= " + common.myInt(sEncId);
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;

                    //    RadWindow1.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(sEncId)
                    //      + "&RptName=IPBill&RptType=D"
                    //      + "&BillId=" + common.myInt(0)
                    //      + "&AdmDt=" + common.myDate(sEncDate)
                    //      + "&Adv=Y"
                    //      + "&Disc=Y"
                    //      + "&RegId=" + common.myInt(sRegId)
                    //      + "&FromDate=" + common.myDate(DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                    //      + "&ToDate=" + common.myDate(DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                    //      + "&IsFilterByDate=0"
                    //      + "&ReportType=D";
                    //    RadWindow1.Height = 600;
                    //    RadWindow1.Width = 1000;
                    //    RadWindow1.Top = 10;
                    //    RadWindow1.Left = 10;
                    //    RadWindow1.VisibleOnPageLoad = true;
                    //    RadWindow1.Modal = true;
                    //    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    //    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region DrugAdministered
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DA"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/ICM/DrugAdministered.aspx?Master=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy");
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 980;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Case Sheet
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("CS"))   //Case Sheet         
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    //RadWindow1.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&EREncounterId=" + common.myInt(ds.Tables[0].Rows[0]["EREncounterId"]) + 
                    //"&EncounterDate=" + common.myDate(sEncDate).ToString("yyyy/MM/dd") + "&OPIP=I";

                    RadWindow1.NavigateUrl = "/Editor/VisitHistory.aspx?Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myInt(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&FromWard=Y&Category=PopUp";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 990;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region DoctorProgressNote
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PN"))//Progress Notes
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMR/Templates/DoctorProgressNote.aspx?Mpg=P1013&RegId=" + common.myStr(sRegId) + "&OPIP=I&MP=NO";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 990;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Blood Bank
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BR"))//start blood bank menu list
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ComponentRequisition.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    //RadWindow1.Height = 610;
                    //RadWindow1.Width = 960;
                    //RadWindow1.Top = 10;
                    //RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Default;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region RequisitionAcknowledge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BRA"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/RequisitionAcknowledge.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region RequisitionReleaseAcknowledge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BRR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/RequisitionReleaseAcknowledge.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region ReleaseAcknowledge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("RA"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ReleaseAcknowledge.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo);
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region BloodAcknowledgeFromWard
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BA"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/BloodAcknowledgeFromWard.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncounterNo=" + common.myStr(sEncNo);
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region BloodTransfusionDetails
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BTD"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/BloodTransfusionDetails.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Blood Component Return
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BCR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ComponentReturn.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Adverse Transfusion Reaction Workup Report
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("ATRWR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/AdverseTransfusionReactionWorkupReport.aspx?MP=NO&Regid=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Referral History
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("RH"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/ReferralSlipHistory.aspx?MASTER=NO&Regno=" + common.myStr(sRegNo) +
                                            "&RegId=" + common.myStr(sRegId) +
                                            "&EncId=" + common.myStr(sEncId);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 940;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region View Critical Tests
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VCT"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    Session["EncounterId"] = common.myInt(sEncId);
                    Session["RegistrationID"] = common.myInt(sRegId);
                    //RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(sEncId)
                    //    + "&RegNo=" + common.myStr(sRegNo) + "&PageSource=Ward";

                    //RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(sEncId) + "&RegNo=" + common.myStr(sRegNo) + "&Source=I&Flag=LIS&Station=All&CloseButtonShow=No&PageSource=Ward";
                    RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(sEncId) + "&RegNo=" + common.myStr(sRegNo) + "&PageSource=Ward";

                    RadWindow1.Height = 600;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 20;
                    RadWindow1.Left = 20;
                    RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region View Unperformed Service
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VUS"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/UnacknowledgedServicesV1.aspx?CF=&Master=Blank&EncId=" + common.myStr(sEncId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncounterNo=" + sEncNo.Trim() +
                                            "&PageSource=Ward" +
                                            "&RegId=" + sRegId +
                                            "&Type=Unperformed" +
                                            "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy");

                    //RadWindow1.NavigateUrl = "UnacknowledgedServices.aspx?CF=&Master=Blank&EncId=" + common.myStr(sEncId)
                    //    + "&RegNo=" + common.myStr(sRegNo) + "&EncounterNo=" + sEncNo.Trim() + "&PageSource=Ward";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 20;
                    RadWindow1.Left = 20;
                    RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Drug Acknowledge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DAK"))   //Drug Acknowledge)
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/WardManagement/DrugAcknowledge.aspx?Regno=" + sRegNo.Trim() +
                                            "&RegID=" + common.myInt(sRegId) +
                                            "&From=POPUP&EncounterId=" + common.myInt(sEncId) +
                                            "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy");
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 990;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Send to OT
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("STOT"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/WardManagement/PatientTransferWardtoOT.aspx?RegNo=" + common.myStr(sRegNo);
                    RadWindow1.Height = 400;
                    RadWindow1.Width = 700;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Patient Allergy/Alert
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VAA"))
            {
                RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=ALL&CF=PTA&Ward=Y";
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
            #endregion
            #region vital
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VE"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMR/Vitals/Vitals.aspx?From=POPUP&btnCopy=False";
                    RadWindow1.Width = 1200;
                    RadWindow1.Height = 630;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Diagnosis
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DI"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMR/Assessment/Diagnosis.aspx?From=POPUP";
                    RadWindow1.Width = 1200;
                    RadWindow1.Height = 630;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Provisional Diagnosis
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PDI"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "~/EMR/Assessment/ProvisionalDiagnosis.aspx?Diag=Tx&From=POPUP";
                    RadWindow1.Width = 1200;
                    RadWindow1.Height = 630;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Allergy
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("AG"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/Allergy/Allergy.aspx?From=POPUP" +
                                            "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Width = 1200;
                    RadWindow1.Height = 630;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Unperformed Service and Items
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("USAI"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + "1900/01/01" +
                                            "&Todate=" + "2079/01/01" +
                                            "&HospitalLocationId=" + common.myInt(Session["HospitalLocationID"]) +
                                            "&FacilityId=" + common.myInt(Session["FacilityID"]) +
                                            "&EncounterId=" + common.myInt(Session["encounterid"]) +
                                            "&ReportName=USAI&Export=True";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 950;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            //#region Inter Hospital transfer
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("IHT"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/InterHospitalTransferReq.aspx?RegNo=" + sRegNo.Trim() +
                                            "&encId=" + common.myInt(sEncId) + "&encno=" + common.myStr(sEncNo) + "&IsSecondaryFacility=" + common.myInt(hdnMnuIsSecondaryFacility.Value) + "&IsPrimaryFacility=" + common.myInt(hdnMnuPrimaryFacility.Value) + "&cid=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]);

                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            //#endregion
            #region Service Activity Detail
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("SAD")
                    || common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DSAD"))
            {
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
            #endregion
            #region Bill Details
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("BillDetails"))
            {
                if (sRegNo.Trim() != string.Empty && sEncId.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRReports/PrintBillDetails.aspx?RN=BillPrint&EncNo=" + sEncId + "&InvId=" + sinvoiceid.Trim();
                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Lab Result
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("LH"))
            {
                if (sRegNo.Trim() != string.Empty && sEncId.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No&FromEMR=1";
                    //  ifrmpage.Attributes["src"] = path + "EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=LIS&Station=All&CloseButtonShow=No&FromEMR=1";

                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1300;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Radiology Result
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("RDH"))
            {
                if (sRegNo.Trim() != string.Empty && sEncId.Trim() != string.Empty)
                {
                    // RadWindow1.NavigateUrl = "/EMRReports/PrintBillDetails.aspx?RN=BillPrint&EncNo=" + sEncId + "&InvId=" + sinvoiceid.Trim();
                    RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS&Station=All&CloseButtonShow=No";

                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1300;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Diagnostic History
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DIGH"))
            {
                if (sRegNo.Trim() != string.Empty && sEncId.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=&Station=All&CloseButtonShow=No&FromEMR=1";

                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1300;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Discharge Discharge Outlier
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DDOL"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/DischargeOutlier.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno = " + common.myStr(sEncNo);
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Chemotheraphy Schedule
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("CHEMOS"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/ICM/MedicalOncologySchedule.aspx?POPUP=1&From=ward";
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Clinical Examination
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("CE"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?IsEMRPopUp=1&WardStatus=Close";
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Insert MRD Details in database by Mukesh Srivastava
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("SFTMRD"))
            {
                string strMsg = "";

                BaseC.RestFulAPI objMRD;

                objMRD = new BaseC.RestFulAPI(sConString);



                strMsg = objMRD.MRDSaveRequestFileFromWardManagement(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                            common.myInt(sRegId),
                           common.myStr(sEncNo),
                            0, //common.myInt(sDoctorId),
                            0,
                           //dtpRequiredDate.SelectedDate.Value, 
                           common.myStr(""),
                           //common.myStr(ViewState["PageType"]), 
                           common.myInt(Session["UserID"]), sConString, common.myInt(0));

                lblMessage.Text = strMsg;
                if (strMsg.Contains("File Issued Saved ..."))
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                else
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            }

            #endregion

            #region ICU Clearance
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("ICUCL"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    //RadWindow1.NavigateUrl = "/ATD/BedClearance.aspx";
                    //RadWindow1.Height = 300;
                    //RadWindow1.Width = 500;
                    //RadWindow1.Top = 10;
                    //RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = string.Empty;
                    //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow1.Modal = true;
                    //RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.NavigateUrl = "/ATD/BedClearance.aspx";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 1100;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Housekeeping Request
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("HKR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/WardManagement/HousekeepingRequest.aspx?BedId=" + iBedId;

                            RadWindow1.Height = 600;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                        }
                    }
                }
            }
            #endregion

            #region Patient Location
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PL"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/CurrentPatientLocation.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno=" + common.myStr(sEncNo);



                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region In-Patients Issues
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("IPS"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "DO", 0);

                            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/ItemIssueSaleReturn.aspx?From=Ward&OPIP=I&RegId=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&Wardid=" + common.myStr(sWardNo) +
                                            "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"]) +
                                            "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"]) +
                                            "&DoctorId=" + common.myStr(sDoctorId) +
                                            "&LOCATION=WARD";

                            RadWindow1.Height = 600;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            RadWindow1.VisibleStatusbar = false;
                        }
                    }
                }
            }
            #endregion

            #region Department Consumption
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DC"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/DepartmenConsumption.aspx?From=Ward&RegId=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myStr(sRegNo) +
                                            "&EncId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myStr(sEncNo) +
                                            "&Wardid=" + common.myStr(sWardNo);

                            RadWindow1.Height = 600;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            RadWindow1.VisibleStatusbar = false;
                        }
                    }
                }
            }
            #endregion
            #region Print Patient Consent form
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("PNT"))
            {
                if (sRegNo.Trim() != string.Empty && sEncId.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMR/Templates/TemplateNotesPrint.aspx?From=POPUP";

                    RadWindow1.Height = 650;
                    RadWindow1.Width = 1300;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Vulnerable Patient
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("VP"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/WardManagement/VulnerablePatient.aspx?POPUP=POPUP";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 880;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose"; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Discharge Summary Acknowledge
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DSA"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/DischargeSummaryAcknowledgement.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno=" + common.myStr(sEncNo);
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Initial Assessment by Doctor
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("IAD"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/InitialAssessmentbyDoctor.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno=" + common.myStr(sEncNo) +
                                             "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Initial Assessment by Nurse
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("IAN"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/WardManagement/InitialAssessmentbyNurse.aspx?RegNo=" + sRegNo.Trim() +
                                            "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]) +
                                            "&Encno=" + common.myStr(sEncNo) +
                                             "&IsPasswordRequired=" + isPasswordRequired;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Discharge Result View
            //else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DRV"))
            //{
            //    if (common.myStr(sRegNo).Trim() != string.Empty  && common.myStr(sEncNo).Trim() != string.Empty)
            //    {
            //        string admDate = DateTime.Now.ToString("dd/MM/yyyy");

            //        if (common.myLen(sEncDate) > 9)
            //        {
            //            admDate = common.myDate(common.myStr(sEncDate).Substring(0, 10)).ToString("MM/dd/yyyy");
            //        }

            //        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/ResultFinalization.aspx?RegNo=" + common.myStr(sRegNo) + "&Admisiondate=" + admDate + "&OP_IP=I&Master=WARD";
            //        RadWindow1.Height = 600;
            //        RadWindow1.Width = 990;
            //        RadWindow1.Top = 10;
            //        RadWindow1.Left = 10;
            //        RadWindow1.OnClientClose = string.Empty); //"SearchPatientOnClientClose";//
            //        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //        RadWindow1.Modal = true;
            //        RadWindow1.Title = menuStatus.SelectedItem.Text;
            //        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            //    }
            //    RadWindow1.VisibleStatusbar = false;
            //}
            #endregion

            #region Discharge Service Activity Details
            //else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]) == "DSAD")
            //{
            //    RadWindow1.NavigateUrl = "~/WardManagement/ServiceActivityDetails.aspx?Regno=" + common.myStr(sRegNo) + "&RegID="
            //                    + common.myStr(sRegId) + "&From=POPUP&EncounterId="
            //                    + common.myInt(sEncId)
            //                    + "&AdmissionDate=" + common.myDate(sEncDate).ToString("MM/dd/yyyy");
            //    RadWindow1.Height = 400;
            //    RadWindow1.Width = 700;
            //    RadWindow1.Top = 10;
            //    RadWindow1.Left = 10;
            //    RadWindow1.OnClientClose = string.Empty); //"SearchPatientOnClientClose";//
            //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //    RadWindow1.Modal = true;
            //    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            //    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            //    RadWindow1.VisibleStatusbar = false;
            //}
            #endregion

            #region Early Warning Score
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("EWS"))
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/EWSTemplate.aspx?HospitalLocationId=" + common.myInt(Session["HospitalLocationID"]) + "&FacilityId=" + common.myInt(Session["FacilityID"]) + "&EncounterId=" + common.myInt(Session["encounterid"]) + "&From=POPUP&DisplayMenu=1";
                RadWindow1.Height = 600;
                RadWindow1.Width = 950;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = string.Empty;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }

            #endregion
            #region Service Order
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("SAC"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/ServiceActivity.aspx?From=WARD&RegID=" + common.myInt(sRegId) +
                                            "&RegNo=" + common.myInt(sRegNo) +
                                            "&encId=" + common.myInt(sEncId) +
                                            "&EncNo=" + common.myInt(sEncNo) +
                                            "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) +
                                            "&InsuranceId=0&CardId=0&PayerType=&BType=";
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.OnClientClose = string.Empty;//"SearchPatientOnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion

            #region OT request
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("OTR"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/OTScheduler/OTRequest.aspx?POPUP=POPUP";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 880;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Nursing Notes
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("NPN"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/EMR/Templates/ProgressNote.aspx";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 880;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region OT Notes
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("OPN"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/EMR/Templates/OTProgressNote.aspx";
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 880;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Discharge Patient
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("DP") && !hdnApolloDhaka.Value.Equals("N"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    RadWindow1.NavigateUrl = "/ATD/discharge.aspx?MPG=P1262&Regno=" + common.myStr(sRegNo) + "&encno=" + common.myStr(sEncNo) + "&cid=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) + "&PaymentType=" + common.myInt(ds.Tables[0].Rows[0]["PaymentType"]) + "&MASTER=No";
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 950;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Assigned Staff Details
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("ASD"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {

                    RadWindow1.NavigateUrl = "~/WardManagement/AssignedStaffDetails.aspx?EncounterId=" + sEncId;
                    RadWindow1.Height = 300;
                    RadWindow1.Width = 500;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClearClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion
            #region Time Based Service
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("TBS"))
            {
                if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                {
                    //RadWindow1.NavigateUrl = "~/EMRBILLING/Popup/AddTimeBasedService.aspx?MPG=P22333&RegId=" + sRegNo + "&RegNo=" + sRegNo + "&EncId=" + sEncId + "&EncNo=" + sEncNo + "&OP_IP=I&CompanyId="
                    //    + common.myInt(Session["CompanyId"]) + "&InsuranceId=" + common.myInt(Session["SponsorId"]) +
                    //    "&CardId=" + common.myInt(Session["InsuranceCardId"]) + "&ConsultId=" + common.myStr(sDoctorId);
                    RadWindow1.NavigateUrl = "~/EMRBILLING/Popup/AddServicesV1.aspx?MPG=P22333&RegId=" + sRegNo + "&RegNo=" + sRegNo + "&EncId=" + sEncId + "&EncNo=" + sEncNo + "&OP_IP=I&CompanyId="
                       + common.myInt(Session["CompanyId"]) + "&InsuranceId=" + common.myInt(Session["SponsorId"]) +
                       "&CardId=" + common.myInt(Session["InsuranceCardId"]) + "&ConsultId=" + common.myStr(sDoctorId);

                    RadWindow1.Width = 1200;
                    RadWindow1.Height = 630;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty;
                    RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region Template Group Tagged Menu
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Contains("TG-"))
            {
                if (sRegNo.Trim() != string.Empty && sEncNo.Trim() != string.Empty)
                {
                    var TemplateGroupId = menuStatus.SelectedItem.Attributes["Code"].Split('-');
                    ///EMR/Templates/Default.aspx?DisplayMenu=1&TemplateId=0&TemplateGroupId=1446
                    RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?IsEMRPopUp=1&TemplateId=0&TemplateGroupId=" + TemplateGroupId[1];
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 980;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    //RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize  | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = menuStatus.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            #endregion

            #region POC
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Contains("POC"))
            {
                if (!common.myStr(Session["EncounterId"]).Equals("") && !common.myStr(Session["Regno"]).Equals(""))
                {
                    RadWindow1.NavigateUrl = "~/EMRBilling/Popup/POCServiceDetails.aspx?EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(Session["Regno"]) + "&BillId=0&PType=WD";
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion

            #region Add Charge Time Wise
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Contains("ACTW"))
            {
                string BType = "";
                if (common.myStr(ds.Tables[0].Rows[0]["PaymentType"]).Equals("B"))
                    BType = "2";
                else if (common.myStr(ds.Tables[0].Rows[0]["PaymentType"]).Equals("C"))
                    BType = "1";

                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddChargeTimewisV1.aspx?MPG=P22333&RegId=" + common.myStr(sRegId).Trim() + "&RegNo=" + common.myStr(sRegNo) + "&EncId=" + common.myStr(sEncId).Trim() + "&EncNo=" + common.myStr(sEncNo).Trim() + "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) + "&InsuranceId=" + common.myInt(ds.Tables[0].Rows[0]["SponsorId"]) + "&CardId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"]) + "&PayerType=" + common.myStr(ds.Tables[0].Rows[0]["PayerType"]) + "&BType=" + common.myStr(BType) + "&ConsultId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                //RadWindow1.OnClientClose = "wndAddService_OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }

            #endregion
            else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).ToUpper().Equals("VIEWSCANDOC"))
            {
                //-------------Added on 05082019----------


                if (common.myStr(ViewState["IMAGEmateEMRScannedDocumentPath"]) != "")
                {
                    //-------------End on 05082019-----------
                    if (common.myStr(sRegNo).Trim() != string.Empty && common.myStr(sEncNo).Trim() != string.Empty)
                    {
                        //string url = "http://edms/IMAGEmateEMR_DRM/frmTreeViewer.aspx?HHNO=" + common.myInt(sRegNo) + "%20&DoctorCode=" + common.myInt(sDoctorId);
                        //string script = " <script type=\"text/javascript\">  window.open('" + url + "');   </script> ";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, false);

                        //string url = "http://edms/testDRM/frmTreeViewer.aspx?HHNO=" + common.myInt(sRegNo) + "%20&DoctorCode=301";
                        //string script = " <script type=\"text/javascript\">  window.open('" + url + "');   </script> ";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, false);

                        //-------------Modified on 05082019------
                        string url = common.myStr(ViewState["IMAGEmateEMRScannedDocumentPath"]) + "HHNO=" + common.myLong(common.myStr(Session["RegistrationNo"])) + "%20&DoctorCode=301";
                        string script = " <script type=\"text/javascript\">  window.open('" + url + "');   </script> ";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, false);
                    }
                }
            }
            if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).ToUpper() != "VIEWSCANDOC")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowPatientDetails()", true);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            status = null;
            patient = null;
            ds.Dispose();
            dsStatus.Dispose();
        }
    }

    protected void BindStatus()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        try
        {
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "BedStatus", string.Empty);
            tbl = ds.Tables[0];
            RadComboBoxItem lst;
            for (int idx = 0; idx < tbl.Rows.Count; idx++)
            {
                lst = new RadComboBoxItem();
                lst.Attributes.Add("style", "background-color:" + common.myStr(tbl.Rows[idx]["StatusColor"]) + ";");
                lst.Value = common.myStr(tbl.Rows[idx]["Code"]);
                lst.Text = common.myStr(tbl.Rows[idx]["Status"]);
                lst.Font.Bold = true;
                lst.ForeColor = System.Drawing.Color.Black;

                ddlBedStatus.Items.Add(lst);
            }
            ddlBedStatus.SelectedIndex = ddlBedStatus.Items.IndexOf(ddlBedStatus.Items.FindItemByValue("O"));
            //ddlbedstatus_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objval = null;
            ds.Dispose();
            tbl.Dispose();
        }
    }

    protected void ddlbedstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void fillData()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        BaseC.WardManagement Objstatus = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsIn = new Hashtable();

        lblrowCount.Text = string.Empty;
        try
        {
            gvWardDtl.VirtualItemCount = 0;

            string iwardId = "";
            int sIsPatientACK = 0;
            string sSearchContent = string.Empty;
            /* if (common.myInt(ddlWard.SelectedValue) > 0)
             {
                 iwardId = common.myInt(ddlWard.SelectedValue);
             }*/
            iwardId = showCheckAllItems(ddlWard);



            gvWardDtl.DataSource = null;
            gvWardDtl.DataBind();

            if (txtSearchContent.Text.Trim().Length > 0)
            {
                sSearchContent = common.myStr(txtSearchContent.Text).Trim();
            }

            if (chkNotAck.Checked)
            {
                sIsPatientACK = 1;
            }

            hsIn = new Hashtable();
            hsIn.Add("@anyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hsIn.Add("@intUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intRegistrationId", 0);

            hsIn.Add("@intWardId", showCheckAllItems(ddlWard));
            //hsIn.Add("@intWardId", iwardId);

            //hsIn.Add("@intDoctorId", 0);
            hsIn.Add("@sBedStatusCode", common.myStr(ddlBedStatus.SelectedValue));
            hsIn.Add("@sSearchContent", sSearchContent);
            hsIn.Add("@bitUnAcknowledgedPatients", sIsPatientACK);
            hsIn.Add("@bitReferralPatients", common.myBool(chkReferral.Checked));
            hsIn.Add("@bitAssignedPatients", common.myBool(chkUnAssignedToStaff.Checked));
            if (common.myInt(ddlStation.SelectedValue) > 0)
            {
                hsIn.Add("@intWardStationId", common.myInt(ddlStation.SelectedValue));
            }

            if (common.myInt(ddlEncounterStatus.SelectedValue) > 0)
            {
                hsIn.Add("@intEncounterStatusId", common.myInt(ddlEncounterStatus.SelectedValue));
            }

            hsIn.Add("@inyPageSize", common.myInt(gvWardDtl.PageSize));
            hsIn.Add("@intPageNo", common.myInt(gvWardDtl.CurrentPageIndex) + 1);
            /* Session["WARDDETAILSFINDWARDID"] =*/
            showCheckAllItems(ddlWard);
            List<string> lstFindPatientWARDID = new List<string>();
            lstFindPatientWARDID = common.myStr(showCheckAllItems(ddlWard)).Trim().Split(',').ToList();

            Session["WARDDETAILSFINDWARDID"] = lstFindPatientWARDID;
            if (chkAll.Checked)
            {
                hsIn.Add("@intDoctorId", 0);
                hsIn.Add("@intSpecialisationId", 0);
                hsIn.Add("@bitAllPatients", 1);
            }
            else
            {
                hsIn.Add("@intDoctorId", common.myInt(ddlProvider.SelectedValue));
                hsIn.Add("@intSpecialisationId", common.myInt(ddlSpecilization.SelectedValue));
                hsIn.Add("@bitAllPatients", 0);
            }
            if (chkVIP.Checked)
            {
                hsIn.Add("@IsVIPPatient", 1);
            }
            if (chkMLC.Checked)
            {
                hsIn.Add("@bitIsMLCPatient", 1);
            }

            ds = dl.FillDataSet(CommandType.StoredProcedure, "USPGetWardDoctorwiseWithFilter", hsIn);

            if (common.myInt(ds.Tables[0].Rows.Count) > 0)
            {
                if (!ds.Tables[0].Columns.Contains("PackageId"))
                {
                    ds.Tables[0].Columns.Add("PackageId", typeof(int));
                }
                if (!ds.Tables[0].Columns.Contains("IsPanel"))
                {
                    ds.Tables[0].Columns.Add("IsPanel", typeof(bool));
                }
                if (!ds.Tables[0].Columns.Contains("TotalRecordsCount"))
                {
                    ds.Tables[0].Columns.Add("TotalRecordsCount", typeof(int));
                }
                string sShowBloodBankRequestOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowBloodBankRequestOnWard", sConString));

                lnkbloodRe.Visible = sShowBloodBankRequestOnWard.ToUpper().Equals("Y") ? true : false;// common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowBloodBankRequestOnWard"));
                lblNoOfRequest.ForeColor = System.Drawing.Color.DarkRed;
                lblNoOfRequest.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["BloodBankRequests"]) + ")";
                lblNoOfRequest.Visible = lnkbloodRe.Visible;
                lblOralGiven.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["OralGivenCount"]) + ")";

                lblNoOfBedTransfer.ForeColor = System.Drawing.Color.DarkRed;
                //lblNoOfBedTransfer.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountAckByWard"]) + ")";
                lblNoOfBedTransfer.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountCheckIn"]) + ")";

                string sShowUnPerformServiceOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowUnPerformServiceOnWard", sConString));
                lnkunperformedSer.Visible = lblunperformedSer.Visible = sShowUnPerformServiceOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowUnPerformServiceOnWard"));
                lblunperformedSer.ForeColor = System.Drawing.Color.DarkRed;
                lblunperformedSer.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountUnperformedService"]) + ")";

                string sShowDrugOrderOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowDrugOrderOnWard", sConString));
                lnkdrugordercount.Visible = lbldrugordercount.Visible = sShowDrugOrderOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowDrugOrderOnWard"));
                lbldrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                lbldrugordercount.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountDrugOrder"]) + ")";

                //lblEstimation Order Work

                string sShowEstimationOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowEstimationOnWard", sConString));
                lnkEstimationOrder.Visible = lblEstimationOrder.Visible = sShowEstimationOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowDrugOrderOnWard"));
                lblEstimationOrder.ForeColor = System.Drawing.Color.DarkRed;
                lblEstimationOrder.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountEstimationOrder"]) + ")";

                //lblEstimation order work

                string sShowNonDrugOrderOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowNonDrugOrderOnWard", sConString));
                lnknondrugordercount.Visible = lblnondrugordercount.Visible = sShowNonDrugOrderOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowNonDrugOrderOnWard"));
                lblnondrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                lblnondrugordercount.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountNonDrugOrder"]) + ")";


                lblNoOfCarePlan.ForeColor = System.Drawing.Color.DarkRed;
                lblNoOfCarePlan.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CarePlanCount"]) + ")";

                lblReferralRequestCount.ForeColor = System.Drawing.Color.DarkRed;
                lblReferralRequestCount.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountIPReferralRequest"]) + ")";
                lblReferralRequestCount.Visible = lnkBtnReferralRequestHistory.Visible;
                if (common.myStr(ds.Tables[0].Rows[0]["ICUClearanceRequiredForBedTransfer"]).Equals("Y"))
                {
                    lnkBedClearance.Visible = true;
                    lblBedClearanceCount.Visible = true;
                    lblBedClearanceCount.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["CountICUClearanceRequest"]) + ")";
                }
                else
                {
                    lnkBedClearance.Visible = false;
                    lblBedClearanceCount.Visible = false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvWardDtl.VirtualItemCount = common.myInt(ds.Tables[0].Rows[0]["TotalRecordsCount"]);
                }
                else
                {
                    gvWardDtl.VirtualItemCount = 0;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvWardDtl.DataSource = ds.Tables[0];
                }

                lblrowCount.Text = "Total Record(s) " + common.myStr(common.myInt(ds.Tables[0].Rows[0]["TotalRecordsCount"]));
                gvWardDtl.DataBind();

            }
            else
            {
                gvWardDtl.DataSource = ds.Tables[0];
                gvWardDtl.DataBind();

                gvWardDtl.VirtualItemCount = 0;

                string sShowBloodBankRequestOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowBloodBankRequestOnWard", sConString));

                lnkbloodRe.Visible = sShowBloodBankRequestOnWard.ToUpper().Equals("Y") ? true : false;
                lblNoOfRequest.ForeColor = System.Drawing.Color.DarkRed;
                lblNoOfRequest.Text = "(0)";
                lblNoOfRequest.Visible = lnkbloodRe.Visible;
                lblOralGiven.Text = "(0)";

                lblNoOfBedTransfer.ForeColor = System.Drawing.Color.DarkRed;
                lblNoOfBedTransfer.Text = "(0)";

                string sShowUnPerformServiceOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowUnPerformServiceOnWard", sConString));
                lnkunperformedSer.Visible = lblunperformedSer.Visible = sShowUnPerformServiceOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowUnPerformServiceOnWard"));
                lblunperformedSer.ForeColor = System.Drawing.Color.DarkRed;
                lblunperformedSer.Text = "(0)";

                string sShowDrugOrderOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowDrugOrderOnWard", sConString));
                lnkdrugordercount.Visible = lbldrugordercount.Visible = sShowDrugOrderOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowDrugOrderOnWard"));
                lbldrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                lbldrugordercount.Text = "(0)";

                string sShowNonDrugOrderOnWard = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ShowNonDrugOrderOnWard", sConString));
                lnknondrugordercount.Visible = lblnondrugordercount.Visible = sShowNonDrugOrderOnWard.ToUpper().Equals("Y") ? true : false;//common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "ShowNonDrugOrderOnWard"));
                lblnondrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                lblnondrugordercount.Text = "(0)";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myStr(ds.Tables[0].Rows[0]["ICUClearanceRequiredForBedTransfer"]).Equals("Y"))
                    {
                        lnkBedClearance.Visible = true;
                        lblBedClearanceCount.Visible = true;
                        lblBedClearanceCount.Text = "(0)";
                    }
                    else
                    {
                        lnkBedClearance.Visible = false;
                        lblBedClearanceCount.Visible = false;
                    }
                }
                else
                {
                    lnkBedClearance.Visible = false;
                    lblBedClearanceCount.Visible = false;
                }
            }
            objSecurity = null;
            //lblrowCount.Text = "Total Record(s) Found : " + common.myStr(gvWardDtl.Items.Count);
            BindGroupTaggingMenu();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            Objstatus = null;
            Objstatus = null;
            ds.Dispose();
            dl = null;
            hsIn = null;
        }
    }
    protected void lnkbloodRe_OnClick(object sender, EventArgs e)
    {
        string strCode = string.Empty;
        if (menuStatus.SelectedItem != null)
        {
            strCode = common.myStr(menuStatus.SelectedItem.Attributes["Code"]);
        }

        var isPasswordRequired = PasswordRequiredHelper.CheckIsPasswordRequiredForSecModuleOptionPages(30, strCode, sConString);
        RadWindow1.NavigateUrl = "/BloodBank/SetupMaster/ComponentRequisitionList.aspx?MP=NO&Regid=0&RegNo= &EncId=0&EncNo= &AckStatus=Ack&Ptype=I" +
                                 "&IsPasswordRequired=" + isPasswordRequired +
                                 "&WardId=" + common.myInt(ddlWard.SelectedValue) +
                                 "&WardName=" + common.myStr(ddlWard.SelectedItem.Text);
        RadWindow1.Height = 550;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = string.Empty;//"OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void lnkBedTransfer_OnClick(object sender, EventArgs e)
    {
        //RadWindow1.NavigateUrl = "/WardManagement/AckByWard.aspx?Type=BedTransferBy&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text);
        RadWindow1.NavigateUrl = "/WardManagement/BedTransferCheckInCheckOut.aspx?Type=BedTransferBy&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text);

        RadWindow1.Height = 650;
        RadWindow1.Width = 1125;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void lnkunperformedSer_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=Unperformed&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text) + "&WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 650;
        RadWindow1.Width = 1125;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void lnkdrugordercount_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=DrugOrder&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text) + "&WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void lnknondrugordercount_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=NonDrugOrder&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text) + "&WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void gvWardDtl_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
            //if (e.Item is GridDataItem || e.Item is GridHeaderItem)
            //{
            //    if (common.myInt(ddlWard.SelectedValue) > 0)
            //    {
            //        e.Item.Cells[2].Visible = false;
            //    }
            //}
            if (e.Item is GridDataItem)
            {
                HiddenField hdnEncounterStatusColor = (HiddenField)e.Item.FindControl("hdnEncounterStatusColor");
                HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");
                HiddenField hdnRegistrationId = (HiddenField)e.Item.FindControl("hdnRegistrationId");
                HiddenField hdnBedCategoryNameForDisplay = (HiddenField)e.Item.FindControl("hdnBedCategoryNameForDisplay");
                HiddenField hdnBedStatusColor = (HiddenField)e.Item.FindControl("hdnBedStatusColor");
                HiddenField hdnConsultingDoctorId = (HiddenField)e.Item.FindControl("hdnConsultingDoctorId");
                HiddenField hdnMobileNo = (HiddenField)e.Item.FindControl("hdnMobileNo");
                //added by bhakti
                HiddenField hdnBillCat = (HiddenField)e.Item.FindControl("hdnBillCat");
                HiddenField hdnPayername = (HiddenField)e.Item.FindControl("hdnPayername");
                HiddenField hdnAgeGender = (HiddenField)e.Item.FindControl("hdnAgeGender");
                HiddenField hdnIsVIPPatient = (HiddenField)e.Item.FindControl("hdnIsVIPPatient");
                HiddenField hdnIsHandleWithCare = (HiddenField)e.Item.FindControl("hdnIsHandleWithCare");
                HiddenField hdnAllergiesAlert = (HiddenField)e.Item.FindControl("hdnAllergiesAlert");
                HiddenField hdnMedicalAlert = (HiddenField)e.Item.FindControl("hdnMedicalAlert");
                HiddenField hdnUnPerformedServices = (HiddenField)e.Item.FindControl("hdnUnPerformedServices");
                HiddenField hdnCriticalLabResult = (HiddenField)e.Item.FindControl("hdnCriticalLabResult");
                HiddenField hdnIsAckVal = (HiddenField)e.Item.FindControl("hdnIsAcknowledged");
                HiddenField hdnPackageId = (HiddenField)e.Item.FindControl("hdnPackageId");
                HiddenField hdnIsPanel = (HiddenField)e.Item.FindControl("hdnIsPanel");
                HiddenField hdnRefrralDoctor = (HiddenField)e.Item.FindControl("hdnRefrralDoctor");
                HiddenField hdnEncounterStatusID = (HiddenField)e.Item.FindControl("hdnEncounterStatusID");
                HiddenField hdnPatientAddress = (HiddenField)e.Item.FindControl("hdnPatientAddress");
                HiddenField hdnSecondaryDoctorName = (HiddenField)e.Item.FindControl("hdnSecondaryDoctorName");
                HiddenField hdnRejectedDrugs = (HiddenField)e.Item.FindControl("hdnRejectedDrugs");
                HiddenField hdnAllowPanelExcludedItems = (HiddenField)e.Item.FindControl("hdnAllowPanelExcludedItems");
                HiddenField hdnPackageIdDetails = (HiddenField)e.Item.FindControl("hdnPackageIdDetails");
                HiddenField hdnBedId = (HiddenField)e.Item.FindControl("hdnBedId");
                HiddenField hdnIsUnAckBedTransfer = (HiddenField)e.Item.FindControl("hdnIsUnAckBedTransfer");
                HiddenField hdnWardNo = (HiddenField)e.Item.FindControl("hdnWardNo");
                HiddenField hdnDiagSample = (HiddenField)e.Item.FindControl("hdnDiagSample");
                HiddenField hdnVulnerableType = (HiddenField)e.Item.FindControl("hdnVulnerableType");
                HiddenField hdnIsDischargeSummaryFinalized = (HiddenField)e.Item.FindControl("hdnIsDischargeSummaryFinalized");
                HiddenField hdnIsGeneralWard = (HiddenField)e.Item.FindControl("hdnIsGeneralWard");
                HiddenField hdnProbableDischarge = (HiddenField)e.Item.FindControl("hdnProbableDischarge");
                HiddenField hdnMLC = (HiddenField)e.Item.FindControl("hdnMLC");
                HiddenField hdnEWSEncodedBy = (HiddenField)e.Item.FindControl("hdnEWSEncodedBy");
                HiddenField hdnEWSEncodedDate = (HiddenField)e.Item.FindControl("hdnEWSEncodedDate");
                HiddenField hdnIsUnAssigned = (HiddenField)e.Item.FindControl("hdnIsUnAssigned");
                HiddenField hdnAssignedNurseName = (HiddenField)e.Item.FindControl("hdnAssignedNurseName");

                HiddenField hdnDepositAmount = (HiddenField)e.Item.FindControl("hdnDepositAmount");
                HiddenField hdnApprovedAmount = (HiddenField)e.Item.FindControl("hdnApprovedAmount");
                HiddenField hdnProvisionalBillAmt = (HiddenField)e.Item.FindControl("hdnProvisionalBillAmt");
                HiddenField hdnBalanceAmt = (HiddenField)e.Item.FindControl("hdnBalanceAmt");
                HiddenField hdnSponsorIdPayorId = (HiddenField)e.Item.FindControl("hdnSponsorIdPayorId");
                HiddenField hdnDoctorSpecialisation = (HiddenField)e.Item.FindControl("hdnDoctorSpecialisation");
                ImageButton imgAllergyAlert1 = (ImageButton)e.Item.FindControl("imgAllergyAlert1");
                ImageButton imgMedicalAlert1 = (ImageButton)e.Item.FindControl("imgMedicalAlert1");
                ImageButton imgUnPerformService = (ImageButton)e.Item.FindControl("imgUnPerformService");
                ImageButton imgCriticalLabResult = (ImageButton)e.Item.FindControl("imgCriticalLabResult");
                ImageButton imgRejectedDrugs = (ImageButton)e.Item.FindControl("imgRejectedDrugs");
                ImageButton imgVIP = (ImageButton)e.Item.FindControl("imgVIP");
                ImageButton imgHandleWithCare = (ImageButton)e.Item.FindControl("imgHandleWithCare");
                ImageButton imgRejectedSample = (ImageButton)e.Item.FindControl("imgRejectedSample");
                ImageButton imgBedTransfer = (ImageButton)e.Item.FindControl("imgBedTransfer");
                ImageButton imgUnAssignedNurse = (ImageButton)e.Item.FindControl("imgUnAssignedNurse");

                ImageButton ibtnEWSGraphPopup = (ImageButton)e.Item.FindControl("ibtnEWSGraphPopup");
                Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
                LinkButton lnkEWSScore = (LinkButton)e.Item.FindControl("lnkEWSScore");

                HiddenField hdnVIPNarration = (HiddenField)e.Item.FindControl("hdnVIPNarration");
                ImageButton imgCarePlan = (ImageButton)e.Item.FindControl("imgCarePlan");

                HiddenField hdnIsCarePlan = (HiddenField)e.Item.FindControl("hdnIsCarePlan");
                imgCarePlan.Visible = false;
                if (common.myBool(hdnIsCarePlan.Value))
                {
                    imgCarePlan.Visible = true;
                }
                if (lnkEWSScore.Text.Trim() != string.Empty)
                {
                    lnkEWSScore.Visible = true;
                    ibtnEWSGraphPopup.Visible = true;

                    lnkEWSScore.ForeColor = common.myDec(lnkEWSScore.Text) >= 7 ? Color.Red : Color.Black;
                }
                Label lblProbableDischarge = (Label)e.Item.FindControl("lblProbableDischarge");
                Label lblMLCPatient = (Label)e.Item.FindControl("lblMLCPatient");

                imgAllergyAlert1.Visible = false;
                imgMedicalAlert1.Visible = false;
                imgUnPerformService.Visible = false;
                imgCriticalLabResult.Visible = false;
                imgRejectedDrugs.Visible = false;
                imgRejectedSample.Visible = false;
                imgUnAssignedNurse.Visible = common.myBool(hdnIsUnAssigned.Value);

                lblProbableDischarge.Visible = common.myBool(hdnProbableDischarge.Value);
                lblMLCPatient.Visible = common.myBool(hdnMLC.Value);

                if (common.myBool(hdnIsDischargeSummaryFinalized.Value))
                {
                    e.Item.Attributes.Add("style", "background-color:#CCFFFF;");
                }

                e.Item.Cells[Convert.ToByte(GridWard.EncounterStatus)].Attributes.Add("style", "background-color:" + common.myStr(hdnEncounterStatusColor.Value) + ";");

                if (common.myInt(hdnDiagSample.Value) > 0)
                {
                    imgRejectedSample.Visible = true;
                }
                if (!common.myBool(hdnIsAckVal.Value))
                {
                    e.Item.Cells[Convert.ToByte(GridWard.RegistrationNo)].Attributes.Add("style", "background-color:#ff99ff;");
                }
                if (common.myBool(hdnIsUnAckBedTransfer.Value))
                {
                    if (common.myStr(ViewState["IsWardAckRequiredForBedTransfer"]).Equals("Y"))
                    {
                        e.Item.Cells[Convert.ToByte(GridWard.RegistrationNo)].Attributes.Add("style", "background-color:#ff99ff;");
                    }
                    imgBedTransfer.Visible = true;
                }

                if (common.myBool(hdnIsHandleWithCare.Value))
                {

                    e.Item.Cells[Convert.ToByte(GridWard.EncounterNo)].Attributes.Add("style", "background-color:#fa8072;");
                }
                if (common.myBool(hdnIsVIPPatient.Value))
                {
                    if (hdnVIPNarration.Value != " ")
                    {
                        e.Item.Cells[Convert.ToByte(GridWard.Patient)].ToolTip = "VIP Narration: " + hdnVIPNarration.Value;
                    }

                    e.Item.Cells[Convert.ToByte(GridWard.Patient)].Attributes.Add("style", "background-color:yellow;");
                }
                imgAllergyAlert1.Visible = common.myBool(hdnAllergiesAlert.Value);
                imgMedicalAlert1.Visible = common.myBool(hdnMedicalAlert.Value);

                //Akshay_11072022
                BaseC.EMRBilling objval = new BaseC.EMRBilling(sConString);
                DataSet ds = new DataSet();
                ds = objval.getPatientTransefers(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId.Value), "uspGetIPUnperformedServices", 0);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        imgUnPerformService.Visible = true;
                    }
                    else
                    {
                        imgUnPerformService.Visible = false;
                    }
                }
                //imgUnPerformService.Visible = common.myBool(hdnUnPerformedServices.Value);

                imgCriticalLabResult.Visible = common.myBool(hdnCriticalLabResult.Value);

                imgRejectedDrugs.Visible = common.myBool(hdnRejectedDrugs.Value);

                for (int i = 0; i <= e.Item.Cells.Count - 2; i++)
                {
                    HiddenField hdnIsAck = (HiddenField)e.Item.FindControl("hdnIsAcknowledged");
                    //palendra
                    HiddenField hdnIsInterHospTranfer = (HiddenField)e.Item.FindControl("hdnInterHospitalTranfer");
                    HiddenField hdnIsSecondaryFacility = (HiddenField)e.Item.FindControl("hdnIsSecondaryFacility");
                    HiddenField hdnPrimaryFacility = (HiddenField)e.Item.FindControl("hdnPrimaryFacility");
                    //palendra
                    HiddenField hdnUnIsAck = (HiddenField)e.Item.FindControl("hdnIsUnAckBedTransfer");

                    if (!i.Equals(13))
                    {
                        Label lblPName = e.Item.FindControl("lblPatientName") as Label;

                        if (i <= 2)
                        {
                            e.Item.Cells[i].Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "','" + //menu
                                                        common.myInt(hdnRegistrationId.Value) + "','" + // RegistrationId
                                                        common.myInt(hdnEncounterId.Value) + "','" + // EncounterId
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text) + "','" + // RegistrationNo
                                                        common.myStr(common.myStr(lblEncounterNo.Text)) + "','" + // EncounterNo
                                                        common.myStr(lblPName.Text).Trim().Replace("'", "") + "','" + // PatientName
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.DoctorName)].Text).Trim().Replace("'", "") + "','" + // DoctorName
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.EncounterDate)].Text) + "','" + // EncounterDate
                                                        common.myInt(hdnBedId.Value) + "','" + // BedId
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.BedNo)].Text) + "','" + // BedNo
                                                        common.myInt(hdnConsultingDoctorId.Value) + "','" + // DoctorId
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.WardName)].Text).Replace("\r", "").Trim().Replace("'", "") + "','" + // WardName
                                                        common.myStr(hdnMobileNo.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // MobileNo
                                                        common.myStr(hdnBillCat.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // BillCat
                                                        common.myStr(hdnPayername.Value).Trim().Replace("'", "") + "','" + // PayerName
                                                        common.myStr(hdnAgeGender.Value) + "','" + // hdnAgeGender
                                                        common.myStr(hdnIsAck.Value) + "','" + // IsAcknowledged
                                                        common.myStr(hdnUnIsAck.Value) + "','" + // IsUnAckBedTransfer
                                                        common.myStr(hdnPackageId.Value) + "','" + // hdnPackageId
                                                        common.myStr(hdnEncounterStatusID.Value) + "','" + // Statusvalue
                                                        common.myStr(hdnIsPanel.Value) + "','" +// hdnIsPanel
                                                        common.myStr(hdnSecondaryDoctorName.Value).Trim().Replace("'", "") + "','" +// Secondary Doc
                                                        common.myStr(hdnPatientAddress.Value).Trim().Replace("'", "") + "','" +// PatientAdd   
                                                        common.myStr(hdnAllowPanelExcludedItems.Value).Trim() + "','" +// AllowPanelExcludedItems                                         
                                                        common.myStr(hdnPackageIdDetails.Value).Trim() + "','" +// PackageIdDetails   
                                                        common.myStr(hdnWardNo.Value).Trim() + "','" +// WardNo    
                                                        common.myStr(hdnVulnerableType.Value).Trim() + "','" +// VulnerableType
                                                        common.myBool(hdnAllergiesAlert.Value) + "','" +//AllergiesAlert
                                                        common.myBool(hdnMedicalAlert.Value) + "','" +//MedicalAlert														
                                                        common.myBool(hdnIsGeneralWard.Value) + "','" +//hdnIsGeneralWard
                                                        common.myInt(e.Item.ItemIndex) + "','" +//RowIdx
                                                        common.myStr(hdnAssignedNurseName.Value) + "','" + //AssignedNurseName
                                                        common.myStr(hdnDepositAmount.Value) + "','" + //DepositAmount 
                                                        common.myStr(hdnApprovedAmount.Value) + "','" + //ApprovedAmount 
                                                        common.myStr(hdnProvisionalBillAmt.Value) + "','" + //ProvisionalBillAmt 
                                                        common.myStr(hdnBalanceAmt.Value) + "','" +//BalanceAmt 
                                                        common.myStr(hdnSponsorIdPayorId.Value).Trim() + "','" + // SponsorIdPayorId
                                                                                                                 //common.myStr("False").Trim() + "','" +
                                                                                                                 // common.myStr("False").Trim() + "','" +
                                                                                                                 // common.myStr("False").Trim() + "','" +
                                                        common.myStr(hdnIsInterHospTranfer.Value).Trim() + "','" + // SponsorIdPayorId
                                                        common.myStr(hdnIsSecondaryFacility.Value).Trim() + "','" + // SponsorIdPayorId
                                                        common.myStr(hdnPrimaryFacility.Value).Trim() + "','" + // SponsorIdPayorId
                                                                                                                //palendra
                                                        common.myStr(hdnDoctorSpecialisation.Value) + "')");  // DoctorSpecialisation

                        }
                        else
                        {

                            e.Item.Cells[i].Attributes.Add("OnClick", "showDemographicDetails(event,'" + menuStatus.ClientID + "','" + //menu
                                                        common.myInt(hdnRegistrationId.Value) + "','" + // RegistrationId
                                                        common.myInt(hdnEncounterId.Value) + "','" + // EncounterId
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text) + "','" + // RegistrationNo
                                                        common.myStr(common.myStr(lblEncounterNo.Text)) + "','" + // EncounterNo
                                                        common.myStr(lblPName.Text).Trim().Replace("'", "") + "','" + // PatientName
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.DoctorName)].Text).Trim().Replace("'", "") + "','" + // DoctorName
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.EncounterDate)].Text) + "','" + // EncounterDate
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.BedNo)].Text) + "','" + // BedNo
                                                        common.myInt(hdnConsultingDoctorId.Value) + "','" + // DoctorId
                                                        common.myStr(e.Item.Cells[Convert.ToByte(GridWard.WardName)].Text).Replace("\r", "").Trim().Replace("'", "") + "','" + // WardName
                                                        common.myStr(hdnMobileNo.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // MobileNo
                                                        common.myStr(hdnBillCat.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // BillCat
                                                        common.myStr(hdnPayername.Value).Trim().Replace("'", "") + "','" + // PayerName
                                                        common.myStr(hdnAgeGender.Value) + "','" + // hdnAgeGender
                                                        common.myStr(hdnIsAck.Value) + "','" + // IsAcknowledged
                                                        common.myStr(hdnUnIsAck.Value) + "','" + // IsUnAckBedTransfer
                                                        common.myStr(hdnPackageId.Value) + "','" + // hdnPackageId
                                                        common.myStr(hdnEncounterStatusID.Value) + "','" + // Statusvalue
                                                        common.myStr(hdnIsPanel.Value) + "','" +// hdnIsPanel
                                                        common.myStr(hdnSecondaryDoctorName.Value).Trim().Replace("'", "") + "','" +// Secondary Doc
                                                        common.myStr(hdnPatientAddress.Value).Trim().Replace("'", "") + "','" +// PatientAdd  
                                                        common.myStr(hdnAllowPanelExcludedItems.Value).Trim() + "','" +// AllowPanelExcludedItems                                       
                                                        common.myStr(hdnPackageIdDetails.Value).Trim() + "','" +// PackageIdDetails  
                                                        common.myStr(hdnVulnerableType.Value).Trim() + "','" +// VulnerableType  
                                                        common.myBool(hdnAllergiesAlert.Value) + "','" +// AllergiesAlert
                                                        common.myBool(hdnMedicalAlert.Value) + "','" +//MedicalAlert
                                                        common.myBool(hdnIsGeneralWard.Value) + "','" +//hdnIsGeneralWard
                                                        common.myInt(e.Item.ItemIndex) + "','" +//RowIdx
                                                        common.myStr(hdnAssignedNurseName.Value) + "','" + //AssignedNurseName
                                                        common.myStr(hdnDepositAmount.Value) + "','" + //DepositAmount 
                                                        common.myStr(hdnApprovedAmount.Value) + "','" + //ApprovedAmount 
                                                        common.myStr(hdnProvisionalBillAmt.Value) + "','" + //ProvisionalBillAmt 
                                                        common.myStr(hdnBalanceAmt.Value) + "')");//BalanceAmt
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                Label lblREGID = (Label)e.Item.FindControl("lblREGID");
                Label lblENCID = (Label)e.Item.FindControl("lblENCID");
                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
                Label lblName = (Label)e.Item.FindControl("lblName");
                Label lblDoctorName = (Label)e.Item.FindControl("lblDoctorName");
                Label lblEncDate = (Label)e.Item.FindControl("lblEncDate");
                Label lblCurrentBedNo = (Label)e.Item.FindControl("lblCurrentBedNo");
                Label lblMobileNo = (Label)e.Item.FindControl("lblMobileNo");
                Label lblCompanyName = (Label)e.Item.FindControl("lblCompanyName");
                Label lblGenderAge = (Label)e.Item.FindControl("lblGenderAge");
                Label lblDischargeStatus = (Label)e.Item.FindControl("lblDischargeStatus");

                HiddenField hdnPatientAddress = (HiddenField)e.Item.FindControl("hdnPatientAddress");
                HiddenField hdnSecondaryDoctorName = (HiddenField)e.Item.FindControl("hdnSecondaryDoctorName");
                HiddenField hdnCurrentWard = (HiddenField)e.Item.FindControl("hdnCurrentWard");
                HiddenField hdnDoctorId = (HiddenField)e.Item.FindControl("hdnDoctorId");
                HiddenField hdnCommonRemarks = (HiddenField)e.Item.FindControl("hdnCommonRemarks");
                //HiddenField hdnIsGeneralWard = (HiddenField)e.Item.FindControl("hdnIsGeneralWard");
                HiddenField hdnBedCategoryNameForDisplay = (HiddenField)e.Item.FindControl("hdnBedCategoryNameForDisplay");
                //added by bhakti
                //HiddenField hdnMnuBillCat = (HiddenField)e.Item.FindControl("hdnMnuBillCat");
                if (common.myLen(hdnCommonRemarks.Value) > 0)
                {
                    //lblDischargeStatus.Text = lblDischargeStatus.Text + " - " + common.myStr(hdnCommonRemarks.Value);
                    lblDischargeStatus.ToolTip = "Remarks: " + common.myStr(hdnCommonRemarks.Value);
                }

                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    HiddenField hdnIsAck = (HiddenField)e.Item.FindControl("hdnIsAcknowledged");

                    e.Item.Cells[i].Attributes.Add("OnClick", "showMenuDischarge(event,'" + menuStatus.ClientID + "','" +
                                                    common.myStr(lblREGID.Text) + "','" + // RegistrationId lblREGID
                                                    common.myStr(lblENCID.Text) + "','" + // EncounterId lblENCID
                                                    common.myStr(lblRegistrationNo.Text) + "','" + // RegistrationNo  lblRegistrationNo
                                                    common.myStr(lblEncounterNo.Text) + "','" + // EncounterNo lblEncounterNo 
                                                    common.myStr(lblName.Text).Trim().Replace("'", "") + "','" + // PatientName lblName
                                                    common.myStr(lblDoctorName.Text).Trim().Replace("'", "") + "','" + // DoctorName lblDoctorName
                                                    common.myStr(lblEncDate.Text) + "','" + // EncounterDate lblEncDate
                                                    common.myStr("0") + "','" + // BedId
                                                    common.myStr(lblCurrentBedNo.Text) + "','" + // BedNo lblCurrentBedNo
                                                    common.myInt(hdnDoctorId.Value) + "','" + // DoctorId 
                                                    common.myStr(hdnCurrentWard.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // WardName
                                                    common.myStr(lblMobileNo.Text).Replace("\r", "").Trim().Replace("'", "") + "','" + // MobileNo lblMobileNo
                                                                                                                                       // common.myStr(hdnBedCategoryNameForDisplay.Value).Replace("\r", "").Trim().Replace("'", "") + "','" + // BillCat
                                                    common.myStr(lblCompanyName.Text).Trim().Replace("'", "") + "','" + // PayerName lblCompanyName
                                                    common.myStr(lblGenderAge.Text) + "','" + // hdnAgeGender lblGenderAge
                                                    common.myBool(true) + "','" + // IsAcknowledged
                                                    common.myStr(false) + "','" + // IsUnAckBedTransfer
                                                    common.myStr("0") + "','" + // hdnPackageId
                                                    common.myStr(ddlEncounterStatus.SelectedValue) + "','" + // Statusvalue
                                                    common.myStr("0") + "','" +// hdnIsPanel
                                                    common.myStr(hdnSecondaryDoctorName.Value).Trim().Replace("'", "") + "','" +// Secondary Doc
                                                    common.myStr(hdnPatientAddress.Value).Trim().Replace("'", "") + "','" +// PatientAdd  
                                                    common.myStr("0") + "','" +// AllowPanelExcludedItems  
                                                    common.myStr("0") + "','" +// PackageIdDetails  
                                                    common.myStr(hdnVulnerableType.Value).Trim() + "','" +// VulnerableType  
                                                                                                          //common.myBool(hdnIsGeneralWard.Value) + "','" +//hdnIsGeneralWard                                                      
                                                    common.myInt(e.Item.ItemIndex) + "','" +
                                                    common.myStr(hdnBedCategoryNameForDisplay.Value) +
                                                    "')"); //RowIdx
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            fillData();
            //txtSearchContent.Focus();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void imgCriticalLabResult_OnClick(object sender, EventArgs e)
    {
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");

            string sRegistrationNo = common.myStr(hdnRegistrationNo.Value);
            string sEncounterNo = common.myStr(hdnEncounterNo.Value);

            RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + sEncounterNo +
                                        "&RegNo=" + sRegistrationNo + "&PageSource=Ward";
            RadWindow1.Height = 400;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void imgUnPerformService_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");

            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");


            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {
                ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/UnacknowledgedServicesV1.aspx?CF=&Master=Blank" +
                                        "&EncId=" + common.myStr(hdnEncounterId.Value) +
                                        "&RegId=" + common.myInt(hdnRegistrationId.Value) +
                                        "&RegNo=" + common.myStr(hdnRegistrationNo.Value) +
                                        "&EncounterNo=" + common.myStr(hdnEncounterNo.Value) +
                                        "&PageSource=Ward&Type=Unperformed";

                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 20;
                RadWindow1.Left = 20;
                RadWindow1.OnClientClose = string.Empty;//"OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            patient = null;
            ds.Dispose();
        }
    }

    protected void imgReffHistory_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");

            if (common.myInt(hdnRegistrationId.Value) > 0 && common.myInt(hdnEncounterId.Value) > 0)
            {
                ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                               common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value).Trim(), common.myInt(Session["UserId"]), 0);

                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

                RadWindow1.NavigateUrl = "~/EMR/ReferralSlipHistory.aspx?Mpg=P1081&MASTER=NO&EncId=" + common.myInt(hdnEncounterId.Value) +
                                          "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&RegNo=" + common.myStr(hdnRegistrationNo.Value) +
                                          "&BindDetail=Y&EncNo=" + common.myStr(hdnEncounterNo.Value);

                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = string.Empty;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            patient = null;
            ds.Dispose();
        }
    }


    protected void imgMedicalAlert1_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;
            HiddenField hdnMobileNo = (HiddenField)gv.FindControl("hdnMobileNo");
            HiddenField hdnPayername = (HiddenField)gv.FindControl("hdnPayername");
            HiddenField hdnAgeGender = (HiddenField)gv.FindControl("hdnAgeGender");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");
            HiddenField hdnCurrentWard = (HiddenField)gv.FindControl("hdnCurrentWard");

            string sRegistrationNo = common.myStr(hdnRegistrationNo.Value);

            string sWardName = common.myStr(hdnCurrentWard.Value);
            string sRegNo = gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text;
            string sEncNo = gv.Cells[Convert.ToByte(GridWard.EncounterNo)].Text;
            if (common.myInt(ddlWard.SelectedValue) > 0)
            {
                sWardName = common.myStr(ddlWard.SelectedItem.Text);
            }

            if (common.myInt(Session["EncounterId"]) != common.myInt(hdnEncounterId.Value))
            {
                //Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //    common.myStr(hdnRegistrationId.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]));

                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    sRegNo, sEncNo, common.myInt(Session["UserId"]), 0);
            }

            RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myInt(hdnEncounterId.Value) +
                                    "&PId=" + common.myStr(hdnRegistrationId.Value) +
                                    "&PN=" + common.myStr(Session["PatientName"]) +
                                    "&PNo=" + sRegistrationNo +
                                    "&PAG=" + common.myStr(Session["AgeGender"]) +
                                    "&EncNo=" + common.myStr(hdnEncounterNo.Value) +
                                    "&SepPat=Y&FromEMR=1&WardDetails=1";

            RadWindow1.Height = 400;
            RadWindow1.Width = 800;
            RadWindow1.Top = 20;
            RadWindow1.Left = 20;
            RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            patient = null;
        }
    }
    protected void imgAllergyAlert1_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;
            HiddenField hdnMobileNo = (HiddenField)gv.FindControl("hdnMobileNo");
            HiddenField hdnPayername = (HiddenField)gv.FindControl("hdnPayername");
            HiddenField hdnAgeGender = (HiddenField)gv.FindControl("hdnAgeGender");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnCurrentWard = (HiddenField)gv.FindControl("hdnCurrentWard");

            string sWardName = common.myStr(hdnCurrentWard.Value);
            string sRegNo = gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text;
            string sEncNo = gv.Cells[Convert.ToByte(GridWard.EncounterNo)].Text;
            if (common.myInt(ddlWard.SelectedValue) > 0)
            {
                sWardName = common.myStr(ddlWard.SelectedItem.Text);
            }

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
               common.myStr(sRegNo), common.myStr(sEncNo), common.myInt(Session["UserId"]), 0);

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            string sRegNoTitle = common.myStr(Resources.PRegistration.regno);
            string sDoctorTitle = common.myStr(Resources.PRegistration.Doctor);
            string DateTitle = common.myStr("Admission Date");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["PatientName"])
                      + ", " + common.myStr(common.myStr(hdnAgeGender.Value)) + "</span>"
                      + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text) + "</span>"
                      + "&nbsp;Enc #.:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text) + "</span>"
                      + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(gv.Cells[Convert.ToByte(GridWard.EncounterStatus)].Text) + "</span>&nbsp;"
                      + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(gv.Cells[Convert.ToByte(GridWard.DoctorName)].Text) + "</span>"
                      + "&nbsp;Bed No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text) + "</span>"
                      + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(sWardName) + "</span>"
                      + "&nbsp;Mobile No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnMobileNo.Value).Replace("\r", "").Trim().Replace("'", "") + "</span>"
                      + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(common.myStr(hdnPayername.Value)) + "</span>"
                      + "</b>";
            }
            else
            {
                Session["PatientDetailString"] = null;
            }

            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {

                Session["EncounterId"] = string.Empty;
                //RadWindow1.NavigateUrl = "/EMR/Allergy/Allergy.aspx?From=POPUP&RegId=" + common.myStr(hdnRegistrationId.Value);
                RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + hdnEncounterId.Value +
                                        "&PId=" + hdnRegistrationId.Value +
                                        "&PN=" + common.myStr(Session["PatientName"]) +
                                        "&PNo=" + sRegNo +
                                        "&PAG=" + common.myStr(Session["AgeGender"]) +
                                        "&EncNo=" + sEncNo +
                                        "&SepPat=Y&WardDetails=1";
                RadWindow1.Width = 1200;
                RadWindow1.Height = 630;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = string.Empty;
                RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            patient = null;
            ds.Dispose();
        }
    }
    protected void chkDischarge_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                dvWardDtl.Visible = false;
                gvWardDtl.DataSource = null;
                gvWardDtl.DataBind();
                dvDischarge.Visible = true;
                gvEncounter.DataSource = null;
                gvEncounter.DataBind();
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                ddlBedStatus.SelectedIndex = 0;
                ddlWard.SelectedIndex = 0;
                ddlBedStatus.Enabled = false;
                ddlWard.Enabled = true;
                BindGroupTaggingMenu();
                bindData("F", 0);
                lnkAdvanceSearch.Visible = true;

                chkAll.Checked = false;
                ddlSpecilization.Enabled = true;
                ddlProvider.Enabled = true;
                chkAll.Visible = false;
                chkMLC.Checked = false;
                chkMLC.Visible = false;
            }
            else
            {
                dvWardDtl.Visible = true;
                gvWardDtl.DataSource = null;
                gvWardDtl.DataBind();
                dvDischarge.Visible = false;
                gvEncounter.DataSource = null;
                gvEncounter.DataBind();
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                ddlWard.SelectedIndex = 0;
                ddlWard.Enabled = true;

                foreach (RadComboBoxItem currentItem in ddlWard.Items)
                {
                    currentItem.Checked = true;
                }
                ddlBedStatus.Enabled = true;
                ddlBedStatus.SelectedValue = "O";
                //ddlbedstatus_SelectedIndexChanged(null, null);
                lnkAdvanceSearch.Visible = true;
                chkAll.Visible = true;
                chkMLC.Checked = false;
                chkMLC.Visible = true;
                fillData();
                gvWardDtl.CurrentPageIndex = 0;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void chkReferral_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                fillData();
                gvWardDtl.CurrentPageIndex = 0;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                gvWardDtl.CurrentPageIndex = 0;
                fillData();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    private void bindData(string RecordButton, int RowNo)
    {
        BaseC.clsEMRBilling objVal = new BaseC.clsEMRBilling(sConString);
        DataSet dsSearch = new DataSet();
        try
        {
            lblrowCount.Text = string.Empty;

            string RegNo = string.Empty;
            string EncNo = string.Empty;
            string PatientName = string.Empty;

            switch (common.myStr(ddlName.SelectedValue))
            {
                case "R":
                    RegNo = txtSearchContent.Text;
                    break;
                case "N":
                    PatientName = txtSearchContent.Text;
                    break;
                case "ENC":
                    EncNo = txtSearchContent.Text;
                    break;
            }

            int DoctorId = 0;
            int SpecialisationId = 0;
            bool AllPatients = true;

            if (!chkAll.Checked)
            {
                DoctorId = common.myInt(ddlProvider.SelectedValue);
                SpecialisationId = common.myInt(ddlSpecilization.SelectedValue);
                AllPatients = false;
            }
            int ddlWardid = 0;
            int count = ddlWard.Items.Count;
            int counter = 0;
            List<string> lstWARDID = new List<string>();
            lstWARDID = (List<string>)Session["WARDDETAILSFINDWARDID"];
            foreach (RadComboBoxItem currentItem in ddlWard.Items)
            {
                currentItem.Checked = false;
                foreach (string WardValueChecked in lstWARDID)
                {
                    if (currentItem.Value.Equals(WardValueChecked))
                    {
                        counter++;
                    }
                }
            }
            if (counter == count)
            {
                ddlWardid = 0;
            }
            else
            {
                ddlWardid = common.myInt(ddlWard.SelectedValue);
            }


            dsSearch = objVal.getOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                 string.Empty, 0, 0, 2, RegNo, EncNo, PatientName, common.myDate(txtFromDate.SelectedDate),
                                                 common.myDate(txtToDate.SelectedDate), "F", 0, common.myInt(gvEncounter.PageSize),
                                                 common.myInt(Session["UserId"]), gvEncounter.CurrentPageIndex + 1, string.Empty, 0,
                                                 string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                 string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty,
                                                 ddlWardid, DoctorId, SpecialisationId, AllPatients, string.Empty, common.myInt(ddlStation.SelectedValue));

            gvEncounter.VirtualItemCount = 0;

            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    gvEncounter.VirtualItemCount = common.myInt(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]);
                }
                else
                {
                    DataRow DR = dsSearch.Tables[0].NewRow();
                    dsSearch.Tables[0].Rows.Add(DR);
                }

                gvEncounter.DataSource = dsSearch.Tables[0];
                gvEncounter.DataBind();
                lblrowCount.Text = "Total Record(s)" + common.myStr(common.myInt(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]));

            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objVal = null;
            dsSearch.Dispose();
        }
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            gvEncounter.CurrentPageIndex = e.NewPageIndex;
            bindData("F", 0);
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        BaseC.ATD Objstatus = new BaseC.ATD(sConString);
        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
            {
                if (common.myInt(((Label)e.Item.FindControl("lblREGID")).Text) > 0)
                {
                    string sRegistrationId = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                    string sRegistrationNo = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    string sEncounterId = common.myStr(((Label)e.Item.FindControl("lblENCID")).Text);
                    string sEncounterNo = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", string.Empty);

                    Session["OPIP"] = "I";
                    Session["EncounterId"] = common.myInt(sEncounterId);
                    Session["RegistrationId"] = common.myInt(sRegistrationId);

                    ds = Objstatus.GetRegistrationDS(common.myInt(sRegistrationNo.Trim()));
                    DateTime adminsiondate = DateTime.Now;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        adminsiondate = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
                        Session["FollowUpDoctorId"] = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                        Session["FollowUpRegistrationId"] = sRegistrationId;
                        Session["DoctorId"] = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                    }

                    //if (chkDeathSummary.Checked)
                    //{
                    //    RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?For=DthSum&Master=NO&RegId=" + common.myInt(sRegistrationId)
                    //  + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                    //  + "&EncId=" + common.myInt(hdnEncounterId.Value)
                    //  + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                    //   + "&AdmissionDate=" + (adminsiondate).ToString("MM/dd/yyyy")
                    //  + "&AdmDId=" + common.myStr(Session["DoctorId"]);
                    //}
                    //else
                    //{
                    RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(sRegistrationId) +
                                            "&RegNo=" + common.myStr(sRegistrationNo) +
                                            "&EncId=" + common.myInt(sEncounterId) +
                                            "&EncNo=" + common.myStr(sEncounterNo) +
                                            "&AdmissionDate=" + (adminsiondate).ToString("MM/dd/yyyy") +
                                            "&AdmDId=" + common.myStr(Session["DoctorId"]);
                    //}
                    RadWindow1.Height = 590;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            Objstatus = null;
            ds.Dispose();
        }
    }

    protected void imgRejectedSample_OnClick(object sender, EventArgs e)
    {
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            string sRegNo = gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text;

            showRejectedSample("IPD", sRegNo);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void lnkBtnRejectedSampleIPCount_OnClick(object sender, EventArgs e)
    {
        try
        {
            showRejectedSample("IPD", txtSearchContent.Text);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void showRejectedSample(string Source, string SearchValue)
    {
        try
        {
            RadWindow1.NavigateUrl = "~/LIS/Phlebotomy/RejectedSampleDetails.aspx?SOURCE=" + common.myStr(Source) +
                                        "&FDate=" + common.myDate(DateTime.Today).ToString("yyyy-MM-dd") +
                                        "&TDate=" + common.myDate(DateTime.Today).ToString("yyyy-MM-dd") +
                                        "&CallFrom=0&Searchcriteria=" + common.myStr(ddlName.SelectedValue) +
                                        "&SearchValue=" + common.myStr(SearchValue) + "&WardId=" + common.myInt(ddlWard.SelectedValue) +
                                        "&WardName=" + common.myStr(ddlWard.SelectedItem.Text);

            RadWindow1.Height = 600;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void fillSampleCollectedStatus()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        int stationID = common.myInt(Session["StationId"]);
        try
        {
            ds = objval.GetSampleCollectedStatus(common.myInt(Session["FacilityId"]), common.myDate(DateTime.Today), common.myDate(DateTime.Today));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //lblRejectedSampleStart.Text = "&nbsp;(&nbsp;Rejected&nbsp;Sample&nbsp;";
                    //lblRejectedSampleEnd.Text = ")";
                    lnkBtnRejectedSampleIPCount.Text = "&nbsp;&nbsp;&nbsp;Rejected IP&nbsp";
                    lblRejectedSampleEnd.Text = "(" + common.myInt(ds.Tables[0].Rows[0]["IPRS"]) + ")";
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
            objval = null;
            ds.Dispose();
        }
    }
    protected void ReferralRequestHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/ReferralRequestHistory.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void lnkBedClearance_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/ATD/BedClearance.aspx?WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 600;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void ddlEncounterStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DataTable dt = new DataTable();
            //DataView dv = new DataView();

            //dt = (DataTable)ViewState["gvWardDetails"];
            //dv = dt.DefaultView;
            //dv.RowFilter = "EncounterStatusId=" + common.myInt(ddlEncounterStatus.SelectedValue);
            //gvWardDtl.DataSource = dv.ToTable();
            //gvWardDtl.DataBind();

            fillData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void lnkOralGiven_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/OralGiven.aspx?Type=OralGiven&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text) + "&WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void gvWardDtl_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            gvWardDtl.CurrentPageIndex = e.NewPageIndex;
            fillData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void lnkAdvanceSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvAdvanceSearch.Visible = true;
            //BindSpeciliazation();
            //BindProvider();
            //chkAll_CheckedChanged(null, null);
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    private void BindSpeciliazation()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsh = new Hashtable();
        DataSet dsSpeciliazation = new DataSet();
        try
        {
            hsh.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsh.Add("@intFacilityId", common.myInt(Session["facilityId"]));

            dsSpeciliazation = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", hsh);

            ddlSpecilization.Text = string.Empty;
            ddlSpecilization.Items.Clear();

            ddlSpecilization.DataSource = dsSpeciliazation.Tables[0];
            ddlSpecilization.DataTextField = "NAME";
            ddlSpecilization.DataValueField = "id";
            ddlSpecilization.DataBind();

            ddlSpecilization.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlSpecilization.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
            hsh = null;
            dsSpeciliazation.Dispose();
        }
    }

    protected void ddlSpecilization_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();
    }


    private void BindProvider()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = new DataSet();
        Hashtable hsIn = new Hashtable();
        try
        {
            hsIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@iUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intSpecializationId", common.myInt(ddlSpecilization.SelectedValue));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", hsIn);

            ddlProvider.Text = string.Empty;
            ddlProvider.Items.Clear();

            ddlProvider.DataSource = objDs.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
            ddlProvider.Items[0].Selected = true;
            ddlProvider.Enabled = true;

            if (objDs.Tables[0].Rows.Count > 1)
            {
                ddlProvider.Enabled = true;
            }
            else
            {
                ddlProvider.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDl = null;
            objDs.Dispose();
            hsIn = null;
        }
    }

    private void CheckUserDoctorOrNot()
    {
        try
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            if (Session["UserID"] != null)
            {
                if (common.myInt(Session["LoginIsAdminGroup"]) == 1)
                {
                    ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                    ddlProvider.Items[0].Selected = true;
                    ddlProvider.Enabled = true;
                    ViewState["IsDoctor"] = "0";
                }
                else
                {
                    SqlDataReader objDr = (SqlDataReader)objEmr.CheckUserDoctorOrNot(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
                    if (objDr != null)
                    {
                        if (objDr.Read())
                        {
                            if ((common.myStr(objDr[0]) != "") && (objDr[0] != null) && (common.myInt(Session["LoginIsAdminGroup"]) == 0))
                            {
                                ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                                ddlProvider.Items[0].Selected = false;
                                ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(objDr[0])));
                                //if (ViewState["OPIP"] != null && ViewState["OPIP"] == "E")
                                //{
                                //ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                                //ddlProvider.Items[0].Selected = true;
                                //}
                                ViewState["IsDoctor"] = "1";
                            }
                            else
                            {
                                ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                                ddlProvider.Items[0].Selected = true;
                                ViewState["IsDoctor"] = "0";
                            }
                        }
                        objDr.Close();
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                fillData();
            }
            dvAdvanceSearch.Visible = false;
            //ddlSpecilization.SelectedIndex = 0;
            //ddlProvider.Items.Clear();
            //ddlProvider.Text = string.Empty;
            //chkAll.Checked = false;
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
            dvAdvanceSearch.Visible = false;
            //ddlSpecilization.SelectedIndex = 0;
            //ddlProvider.Items.Clear();
            //ddlProvider.Text = string.Empty;
            //chkAll.Checked = false;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkAll.Checked)
            {
                ddlProvider.SelectedIndex = 0;
                ddlSpecilization.SelectedIndex = 0;
                ddlProvider.Enabled = false;
                ddlSpecilization.Enabled = false;
            }
            else
            {
                ddlProvider.Enabled = true;
                ddlSpecilization.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }


    #region sorting 

    protected void gvWardDtl_OnPreRender(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                if (gvWardDtl.SelectedItems.Count.Equals(0))
                {
                    bindData("F", 0);
                }
            }
            else
            {
                if (gvWardDtl.SelectedItems.Count.Equals(0))
                {
                    fillData();
                }

            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void gvEncounter_OnPreRender(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                if (gvEncounter.SelectedItems.Count.Equals(0))
                {
                    bindData("F", 0);
                }
            }
            else
            {
                fillData();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    #endregion

    protected void imgRejectedDrugs_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");

            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");

            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {
                ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/RejectedDrug.aspx?CF=&Master=Blank" +
                                        "&EncId=" + common.myStr(hdnEncounterId.Value);

                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 20;
                RadWindow1.Left = 20;
                RadWindow1.OnClientClose = string.Empty;//"OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            patient = null;
            ds.Dispose();
        }
    }

    protected void chkVIP_OnClick(object sender, EventArgs e)
    {
        fillData();
    }
    protected void chkUnAssignedToStaff_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                bindData("F", 0);
            }
            else
            {
                fillData();
                gvWardDtl.CurrentPageIndex = 0;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void chkMLC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDischarge.Checked)
            {
                //bindData("F", 0);
            }
            else
            {
                gvWardDtl.CurrentPageIndex = 0;
                fillData();
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void imgUnAssignedNurse_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;


            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");


            if (common.myStr(hdnRegistrationNo.Value).Trim() != string.Empty && common.myStr(hdnEncounterNo.Value).Trim() != string.Empty)
            {

                RadWindow1.NavigateUrl = "~/WardManagement/AssignedStaffDetails.aspx?EncounterId=" + hdnEncounterId.Value;
                RadWindow1.Height = 300;
                RadWindow1.Width = 500;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "OnClearClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            }
        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void ibtnEWSPopup_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            Session["RegistrationId"] = hdnRegistrationId.Value;
            Session["EncounterId"] = hdnEncounterId.Value;

            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/EWSTemplate.aspx?HospitalLocationId="
                                            + common.myInt(Session["HospitalLocationID"])
                                            + "&FacilityId=" + common.myInt(Session["FacilityID"])
                                            + "&EncounterId=" + common.myInt(hdnEncounterId.Value)
                                            + "&RegistrationId=" + hdnRegistrationId.Value
                                            + "&From=POPUP&DisplayMenu=1";
                RadWindow1.Height = 400;
                RadWindow1.Width = 800;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "OnClearClientClose"; ; //"SearchPatientOnClientClose";//
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow1.Title = "Early Warnning Score";
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

            }

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            patient = null;
        }

    }

    protected void ibtnEWSGraphPopup_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ImageButton img = (ImageButton)sender;
            GridItem gv = (GridItem)img.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");
            HiddenField hdnEWSTemplateId = (HiddenField)gv.FindControl("hdnEWSTemplateId");

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                          common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            Session["RegistrationId"] = hdnRegistrationId.Value;
            Session["EncounterId"] = hdnEncounterId.Value;
            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/EWSGraph.aspx?HospitalLocationId="
                                            + common.myInt(Session["HospitalLocationID"])
                                            + "&FacilityId=" + common.myInt(Session["FacilityID"])
                                            + "&EncounterId=" + common.myInt(hdnEncounterId.Value)
                                            + "&RegistrationId=" + hdnRegistrationId.Value
                                            + "&TemplateId=" + common.myInt(hdnEWSTemplateId.Value)
                                            + "&From=POPUP&DisplayMenu=1";
                RadWindow1.Height = 400;
                RadWindow1.Width = 500;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = string.Empty; ; //"SearchPatientOnClientClose";//
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow1.Title = "Early Warnning Score";
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

            }

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            patient = null;
        }
    }

    protected void lnkEWSScore_Click(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        try
        {
            LinkButton lnk = (LinkButton)sender;
            GridItem gv = (GridItem)lnk.NamingContainer;

            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterNo = (HiddenField)gv.FindControl("hdnEncounterNo");
            HiddenField hdnRegistrationNo = (HiddenField)gv.FindControl("hdnRegistrationNo");

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            Session["RegistrationId"] = hdnRegistrationId.Value;
            Session["EncounterId"] = hdnEncounterId.Value;

            if (common.myStr(hdnRegistrationId.Value) != "" && common.myStr(hdnEncounterId.Value) != "")
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/EWSTemplate.aspx?HospitalLocationId="
                                            + common.myInt(Session["HospitalLocationID"])
                                            + "&FacilityId=" + common.myInt(Session["FacilityID"])
                                            + "&EncounterId=" + common.myInt(hdnEncounterId.Value)
                                            + "&RegistrationId=" + hdnRegistrationId.Value
                                            + "&From=POPUP&DisplayMenu=1";
                RadWindow1.Height = 400;
                RadWindow1.Width = 800;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "OnClearClientClose"; ; //"SearchPatientOnClientClose";//
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow1.Title = "Early Warnning Score";
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            }

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            patient = null;
        }
    }

    protected void lnkPatientCarePlan_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindow1.NavigateUrl = "~/EMR/ClinicalPathway/PatientTreatmentPlan.aspx?From=POPUP";
            RadWindow1.Height = 400;
            RadWindow1.Width = 500;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.OnClientClose = "OnClearClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void imgCarePlan_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnk = (ImageButton)sender;
            GridItem gv = (GridItem)lnk.NamingContainer;
            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");

            RadWindow1.NavigateUrl = "~/EMR/ClinicalPathway/PatientTreatmentPlan.aspx?From=POPUP&EncId=" + common.myInt(hdnEncounterId.Value)
                + "&RegId=" + common.myInt(hdnRegistrationId.Value);
            RadWindow1.Height = 400;
            RadWindow1.Width = 500;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.OnClientClose = "OnClearClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    private string showCheckAllItems(RadComboBox rcb)
    {
        string checkAll = String.Empty;
        var collectionChecked = rcb.CheckedItems;
        if (collectionChecked.Count > 0)
        {
            foreach (var item in collectionChecked)
            {
                if (checkAll.Equals(String.Empty))
                {
                    checkAll = item.Value;
                    item.Attributes.ToString();
                }
                else
                {
                    checkAll = checkAll + "," + item.Value;
                }
            }
        }
        return checkAll;
    }

    protected void imgSaveFavouriteWardTagging_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.WardManagement objwm = new BaseC.WardManagement();
        StringBuilder strXML = new StringBuilder();
        StringBuilder strEmpXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            foreach (var item in ddlWard.CheckedItems)
            {
                coll.Add(common.myInt(item.Value));
                strXML.Append(common.setXmlTable(ref coll));
            }

            if (common.myInt(Session["EmployeeId"]) > 0)
            {
                coll.Add(common.myInt(Session["EmployeeId"]));
                strEmpXML.Append(common.setXmlTable(ref coll));
            }

            if (strEmpXML.ToString() == "")
            {
                Alert.ShowAjaxMsg("Please Select Employee and  Ward !", Page);
                return;
            }
            string strMsg = objwm.SaveFavouriteWardlagTagging(strEmpXML.ToString(), strXML.ToString(), common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"]));

            if (strMsg.Contains("Record Saved..."))
            {
                bindControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            lblMessage.Text = strMsg;
            bindControl();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkEstimationOrder_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/ATD/SearchCounselingDetails.aspx?EstimationOrderLOSExp=WARDDETAIL&Searchcriteria=" + common.myStr(ddlName.SelectedValue) + "&SearchValue=" + common.myStr(txtSearchContent.Text) + "&WardId=" + common.myInt(ddlWard.SelectedValue);
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    private DataSet CheckBedReleased()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsh = new Hashtable();
        DataSet dsBedReleased = new DataSet();
        try
        {

            hsh.Add("@intFacilityId", common.myInt(Session["facilityId"]));
            hsh.Add("@intEncounterId", common.myInt(Session["EncounterId"]));


            dsBedReleased = dl.FillDataSet(CommandType.StoredProcedure, "uspWardCheckBedReleaseWard", hsh);

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
            hsh = null;


        }
        return dsBedReleased;
    }

    // Akshay
    protected void imgBloodTransfusionReaction_OnClick(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);

        ImageButton img = (ImageButton)sender;
        GridItem gv = (GridItem)img.NamingContainer;

        HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationId");
        HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
        string sRegNo = gv.Cells[Convert.ToByte(GridWard.RegistrationNo)].Text;
        string sEncNo = gv.Cells[Convert.ToByte(GridWard.EncounterNo)].Text;
        try
        {
            RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/BloodTransfusionReactionDetails.aspx?MP=NO&Regid=" + common.myInt(hdnRegistrationId.Value) + "&EncounterNo = "
                + common.myStr(sEncNo) + "&RegNo=" + common.myStr(sRegNo) + "&EncId = " + common.myStr(hdnEncounterId.Value);

            RadWindow1.Height = 500;
            RadWindow1.Width = 960;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Title = "Blood Transfusion Reaction Details";
            RadWindow1.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindow1.Title = menuStatus.SelectedItem.Text;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
}
