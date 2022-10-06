using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Script.Services;
using System.Web.Services;
using Telerik.Web.UI;
using BaseC;

public partial class EMR_Medication_PrescribeMedicationNew : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    int count = 0;
    string path = string.Empty;
    string FlagShowPreviousStoreInDrugIndent = string.Empty;
    private enum enumColumns : byte
    {
        Sno = 0,
        BrandName = 1,
        StartDate = 2,
        EndDate = 3,
        IndentType = 4,
        TotalQty = 5,
        PrescriptionDetail = 6,
        BrandDetailsCIMS = 7,
        MonographCIMS = 8,
        InteractionCIMS = 9,
        DIInteractionCIMS = 10,
        DHInteractionCIMS = 11,
        DAInteractionCIMS = 12,
        BrandDetailsVIDAL = 13,
        MonographVIDAL = 14,
        InteractionVIDAL = 15,
        DHInteractionVIDAL = 16,
        DAInteractionVIDAL = 17,
        Edit = 18,
        Delete = 19
    }
    private enum enumFavourite : byte
    {
        chkAll = 0,
        DrugName = 1,
        Remove = 2
    }
    private enum enumCurrent : byte
    {
        chkAll = 0,
        OrderNo = 1,
        OrderDate = 2,
        DrugName = 3,
        IndentType = 4,
        TotalQty = 5,
        PrescriptionDetail = 6,
        StartDate = 7,
        EndDate = 8,
        BrandDetailsCIMS = 9,
        MonographCIMS = 10,
        InteractionCIMS = 11,
        DIInteractionCIMS = 12,
        DHInteractionCIMS = 13,
        DAInteractionCIMS = 14,
        BrandDetailsVIDAL = 15,
        MonographVIDAL = 16,
        InteractionVIDAL = 17,
        DHInteractionVIDAL = 18,
        DAInteractionVIDAL = 19,
        StopRemarks = 20,
        Stop = 21
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        try
        {
            Page.Theme = "DefaultControls";
            if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
            {
                this.MasterPageFile = "~/Include/Master/EMRMaster.master";
            }
            else if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP") || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
            {
                this.MasterPageFile = "~/Include/Master/BlankMaster.master";
            }
            if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
        catch
        {
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["FacilityName"] = Session["FacilityName"];
        btnSave.Enabled = true;

        if (common.myStr(Session["OPIP"]).Equals("O"))
        {
            maindivofControl.Visible = false;
            btnVariableDose.Visible = false;
            if (!PatientandClinicianValidateion().Equals(string.Empty))
            {
                dverx.Visible = true;
                return;
            }
        }
        if (common.myStr(Session["OPIP"]).Equals("I"))
        {
            maindivofControl.Visible = true;
            btnVariableDose.Visible = true;
            btnCopyLastPrescription.Visible = false;
        }

        if (!IsPostBack)
        {
            try
            {
                Session["FacilityName"] = Session["FacilityName"];

                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("SPS") || common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                {
                    lblDose.Text = "Qty";
                    lblStrength.Text = "Dose";
                }

                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        "AllowZeroStockItemInIPPrescription,AllowZeroStockItemInOPPrescription,DefaultERIndentStoreId,DefaultOPIndentStoreId,DefaultOTIndentStoreId,DefaultWardIndentStoreId,eclaimWebServiceLoginID,eclaimWebServicePassword,EMRPrescriptionDoseShowInFractionalValue,EpresActive,IncludeToContinueDurationInPrescription,IsRemovePrescriptionPopUpMsgForItemsNotInStock,PrescriptionAllowForItemsNotInStockShowAlert,RouteMandatory,ShowStockQtyInPrescription,EMRPrescriptionDoseCalculateBasedOnWeight,EMRIsHideBrandInPrescription,PrescriptionAllowForItemsNotInStock,IsAutoAddFavirateInPrescription,ISShowProfitPercentageInPrescription,isRequireIPBillOfflineMarking", sConString);

                if (collHospitalSetupValues.ContainsKey("EMRPrescriptionDoseShowInFractionalValue"))
                    chkFractionalDose.Checked = common.myStr(collHospitalSetupValues["EMRPrescriptionDoseShowInFractionalValue"]).Equals("Y");
                if (collHospitalSetupValues.ContainsKey("RouteMandatory"))
                    hdnIsRouteMandatory.Value = common.myStr(collHospitalSetupValues["RouteMandatory"]);
                if (collHospitalSetupValues.ContainsKey("EpresActive"))
                    hdnEpresActive.Value = common.myStr(collHospitalSetupValues["EpresActive"]);
                if (collHospitalSetupValues.ContainsKey("eclaimWebServiceLoginID"))
                    hdneclaimWebServiceLoginID.Value = common.myStr(collHospitalSetupValues["eclaimWebServiceLoginID"]);
                if (collHospitalSetupValues.ContainsKey("eclaimWebServicePassword"))
                    hdneclaimWebServicePassword.Value = common.myStr(collHospitalSetupValues["eclaimWebServicePassword"]);
                if (collHospitalSetupValues.ContainsKey("IsRemovePrescriptionPopUpMsgForItemsNotInStock"))
                    ViewState["IsRemovePrescriptionPopUpMsgForItemsNotInStock"] = common.myStr(collHospitalSetupValues["IsRemovePrescriptionPopUpMsgForItemsNotInStock"]);
                if (collHospitalSetupValues.ContainsKey("ShowStockQtyInPrescription"))
                    ViewState["ShowStockQtyInPrescription"] = common.myStr(collHospitalSetupValues["ShowStockQtyInPrescription"]);
                if (collHospitalSetupValues.ContainsKey("DefaultWardIndentStoreId"))
                    ViewState["DefaultWardIndentStoreId"] = common.myStr(collHospitalSetupValues["DefaultWardIndentStoreId"]);
                if (collHospitalSetupValues.ContainsKey("DefaultOPIndentStoreId"))
                    ViewState["DefaultOPIndentStoreId"] = common.myStr(collHospitalSetupValues["DefaultOPIndentStoreId"]);
                if (collHospitalSetupValues.ContainsKey("DefaultOTIndentStoreId"))
                    ViewState["DefaultOTIndentStoreId"] = common.myStr(collHospitalSetupValues["DefaultOTIndentStoreId"]);
                if (collHospitalSetupValues.ContainsKey("DefaultERIndentStoreId"))
                    ViewState["DefaultERIndentStoreId"] = common.myStr(collHospitalSetupValues["DefaultERIndentStoreId"]);
                if (collHospitalSetupValues.ContainsKey("AllowZeroStockItemInIPPrescription"))
                    ViewState["AllowZeroStockItemInIPPrescription"] = common.myStr(collHospitalSetupValues["AllowZeroStockItemInIPPrescription"]);
                if (collHospitalSetupValues.ContainsKey("AllowZeroStockItemInOPPrescription"))
                    ViewState["AllowZeroStockItemInOPPrescription"] = common.myStr(collHospitalSetupValues["AllowZeroStockItemInOPPrescription"]);
                if (collHospitalSetupValues.ContainsKey("PrescriptionAllowForItemsNotInStockShowAlert"))
                    ViewState["PrescriptionAllowForItemsNotInStockShowAlert"] = common.myStr(collHospitalSetupValues["PrescriptionAllowForItemsNotInStockShowAlert"]);
                if (collHospitalSetupValues.ContainsKey("IncludeToContinueDurationInPrescription"))
                {
                    if (!common.myStr(collHospitalSetupValues["IncludeToContinueDurationInPrescription"]).Equals("Y"))
                    {
                        foreach (Telerik.Web.UI.RadComboBoxItem item in ddlPeriodType.Items)
                        {
                            if (common.myStr(item.Value).Equals("C"))
                            {
                                ddlPeriodType.Items.Remove(item);
                                break;
                            }
                        }
                    }
                }

                if (collHospitalSetupValues.ContainsKey("EMRPrescriptionDoseCalculateBasedOnWeight"))
                    ViewState["EMRPrescriptionDoseCalculateBasedOnWeight"] = common.myStr(collHospitalSetupValues["EMRPrescriptionDoseCalculateBasedOnWeight"]);
                if (collHospitalSetupValues.ContainsKey("EMRIsHideBrandInPrescription"))
                    ViewState["EMRIsHideBrandInPrescription"] = common.myStr(collHospitalSetupValues["EMRIsHideBrandInPrescription"]);
                if (collHospitalSetupValues.ContainsKey("PrescriptionAllowForItemsNotInStock"))
                    ViewState["PrescriptionAllowForItemsNotInStock"] = common.myStr(collHospitalSetupValues["PrescriptionAllowForItemsNotInStock"]);
                if (collHospitalSetupValues.ContainsKey("IsAutoAddFavirateInPrescription"))
                    ViewState["IsAutoAddFavirateInPrescription"] = common.myStr(collHospitalSetupValues["IsAutoAddFavirateInPrescription"]);
                if (collHospitalSetupValues.ContainsKey("ISShowProfitPercentageInPrescription"))
                    ViewState["ISShowProfitPercentageInPrescription"] = common.myStr(collHospitalSetupValues["ISShowProfitPercentageInPrescription"]);

                //added by bhakti
                if (collHospitalSetupValues.ContainsKey("isRequireIPBillOfflineMarking"))
                    ViewState["isRequireIPBillOfflineMarking"] = common.myStr(collHospitalSetupValues["isRequireIPBillOfflineMarking"]);

                hdnCopyLastPresc.Value = "0";
                ViewState["GridDataItem"] = null;
                ViewState["Item"] = null;
                ViewState["ItemDetail"] = null;
                ViewState["StopItemDetail"] = null;
                ViewState["Edit"] = null;
                hdnMainUnAppPrescriptionId.Value = null;

                if (common.myStr(hdnIsRouteMandatory.Value).Equals("Y"))
                {
                    spnRoute.Visible = true;
                }
                else
                {
                    spnRoute.Visible = false;
                }

                dvConfirmStop.Visible = false;
                lbtnFrequencyTime.Visible = common.myStr(Session["OPIP"]).Equals("O") ? false : true;
                Session["CurentSavedPrescriptionId"] = string.Empty;
                hdnTemplateFieldId.Value = common.myInt(Request.QueryString["TemplateFieldId"]).ToString();
                hdnControlId.Value = common.myStr(Request.QueryString["ID"]).Trim();
                hdnControlType.Value = common.myStr(Request.QueryString["ControlType"]).Trim();
                removeDropDownFromTextBox(txtFavouriteItemName);
                removeDropDownFromTextBox(txtDose);
                removeDropDownFromTextBox(txtDuration);
                removeDropDownFromTextBox(txtCurrentItemName);
                if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
                {
                    chkApprovalRequired.Visible = true;
                    ViewState["Regno"] = common.myStr(Request.QueryString["Regno"]);
                    ViewState["Encno"] = common.myStr(Request.QueryString["Encno"]);
                    ViewState["RegId"] = common.myStr(Request.QueryString["RegId"]);
                    ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
                }
                else
                {
                    ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
                    ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
                    ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
                    ViewState["EncId"] = common.myStr(Session["EncounterId"]);
                }
                if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
                {
                    ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
                    ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
                    ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
                    ViewState["EncId"] = common.myStr(Session["EncounterId"]);
                    btnclose.Visible = false;
                }

                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    chkApprovalRequired.Visible = true;
                }
                if (common.myInt(ViewState["EncId"]).Equals(0))
                {
                    Response.Redirect("/default.aspx?RegNo=0", false);
                }

                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    rdoSearchMedication.SelectedIndex = rdoSearchMedication.Items.IndexOf(rdoSearchMedication.Items.FindByValue("C"));
                    //rdoSearchMedication_OnSelectedIndexChanged(null, null);
                }
                ViewState["ConversioinFactor"] = true;
                hdnStoreId.Value = "0";
                hdnGenericId.Value = "0";
                hdnItemId.Value = "0";
                lblMessage.Text = "&nbsp;";
                ViewState["GridDataItem"] = null;
                btnPrint.Visible = common.myStr(Session["OPIP"]).Equals("O") ? true : false;
                if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
                {
                    chkCustomMedication.Enabled = false;
                    chkNotToPharmacy.Enabled = false;
                    lnkStopMedication.Visible = false;
                    btnPreviousMedications.Text = "Previous Order";
                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    //ddlRoute.Items.Insert(0, new ListItem(string.Empty));
                    ddlRoute.Items[0].Value = "0";
                    ddlRoute.SelectedIndex = 0;
                    ddlRoute.Enabled = false;
                }

                rdoSearchMedication_OnSelectedIndexChanged(null, null);
                //Comment By Himanshu On Date 02/03/2022
                //BindBlankItemGrid();

                bindOnLoadData();

                Session["MedicationSetItemIds"] = null;
                if (!common.myBool(Session["IsLoginDoctor"]))
                {
                    btnOrderSet.Visible = false;
                }

                btnCopyLastPrescription.Enabled = true;
                //BindGrid(string.Empty, string.Empty);

                bindUnApprovedPrescriptions();

                BaseC.Security objSecurity = new BaseC.Security(sConString);  //
                ViewState["IsAllowToAddBlockedItem"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedItem");
                objSecurity = null;

                divConfirmation.Visible = false;
                if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP") || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
                {
                    btnclose.Visible = true;
                }
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    spnStartdate.Visible = true;
                    Accordion1.SelectedIndex = -1; //0;
                }
                else
                {
                    spnStartdate.Visible = false;
                    Accordion1.SelectedIndex = -1;
                }
                txtStartDate.SelectedDate = null;
                txtStartDate.MinDate = DateTime.Now;
                txtEndDate.MinDate = DateTime.Now;
                GetBrandFavouriteData(string.Empty);
                BindPatientHiddenDetails();
                //txtDose.Attributes.Add("onblur", "javascript:CalcChange()");
                //txtDuration.Attributes.Add("onblur", "javascript:CalcChange()");
                SetPermission();
                dvConfirmAlreadyExistOptions.Visible = false;
                //dvExpensiveDrugs.Visible = false;

                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSave.Visible = false;
                    btnCopyLastPrescription.Visible = false;
                    ddlGeneric.Enabled = false;
                    ddlBrand.Enabled = false;
                    btnProceedFavourite.Enabled = false;
                    chkCustomMedication.Enabled = false;
                    btnAddItem.Enabled = false;
                    // lstFavourite.Enabled = false;
                    //foreach (GridViewRow dataItem in lstFavourite.Rows)
                    //{
                    //    LinkButton lnkItemName = (LinkButton)dataItem.FindControl("lnkItemName");
                    //    CheckBox  chkRow = (CheckBox)dataItem.FindControl("chkRow");
                    //    ImageButton ibtnDelete = (ImageButton)dataItem.FindControl("ibtnDelete");

                    //    rdoSearchMedication.Enabled = false;
                    //    ibtnDelete.Enabled = false;
                    //    lnkItemName.Enabled = false;
                    //    chkRow.Enabled = false;
                    //}
                }

                //ddlBrand.Focus();
                if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                {
                    btnclose1.Visible = true;
                }
                if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
                {
                    var bli = rdoSearchMedication.Items[1];
                    bli.Attributes.Add("hidden", "hidden");
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
                #region Care Plan Item name add in add list by default
                if (common.myInt(Request.QueryString["PlanId"]) > 0)
                {
                    string xmlSchema = common.myStr(Session["CarePlanDrugLists"]).Trim();
                    Session["CarePlanDrugLists"] = null;
                    StringReader sr = new StringReader(xmlSchema);
                    DataSet dsXml = new DataSet();
                    dsXml.ReadXml(sr);
                    AddPatientPlanCareItems(dsXml);
                    sr = null;
                    dsXml.Dispose();
                    xmlSchema = string.Empty;
                }
                #endregion
                FlagShowPreviousStoreInDrugIndent = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                           common.myInt(Session["FacilityId"]), "ShowRequestingWardInDrugIndent", sConString);
                if (FlagShowPreviousStoreInDrugIndent.Equals("Y"))
                {
                    ChkReqestFromOtherWard.Visible = true;
                    lblRequestFromOtherWard.Visible = true;
                    bindRequestFromOtherWard();
                }
                else
                {
                    ChkReqestFromOtherWard.Visible = false;
                    lblRequestFromOtherWard.Visible = false;
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

            Label lblClosingBalance = (Label)ddlBrand.Header.FindControl("lblClosingBalance");

            if (common.myStr(ViewState["ShowStockQtyInPrescription"]).Equals("Y"))
            {
                lblClosingBalance.Visible = true;
            }
            else
            {
                lblClosingBalance.Visible = false;
            }
            ddlBrand.Focus();

            if (txtStartDate.SelectedDate == null)
            {
                if (spnStartdate.Visible)
                {
                    txtStartDate.SelectedDate = DateTime.Now.Date;
                }
            }



            if (!common.myStr(Session["OPIP"]).Equals("I"))
            {
                if (common.myStr(ViewState["EMRIsHideBrandInPrescription"]).Equals("Y"))
                {
                    ddlBrand.Visible = false;
                    lblInfoBrand.Visible = false;
                    ddlGeneric.Focus();
                }
            }

            setPrescriptionMandatoryFieldSetup();
            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
            {
                chkApprovalRequired.Visible = false;
            }
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);

            CheckNoOfIndentInOneDay();
        }

        if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
        {
            var bli = rdoSearchMedication.Items[1];
            bli.Attributes.Add("hidden", "hidden");
        }

        //btnPrint.Visible = false;
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemId.Value) > 1)
            {
                if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                {
                    btnBrandDetailsViewOnItemBased.Visible = true;
                }
                btnMonographViewOnItemBased.Visible = true;
            }
            else
            {
                btnBrandDetailsViewOnItemBased.Visible = false;
                btnMonographViewOnItemBased.Visible = false;
            }
            setGridColor();
        }
        else
        {
            btnBrandDetailsViewOnItemBased.Visible = false;
            btnMonographViewOnItemBased.Visible = false;
        }
        // Akshay_Tirathram
        dvNarcoticDrugsPopup.Visible = false;
        dvExpensiveDrugs.Visible = false;
    }
    protected void AddPatientPlanCareItems(DataSet dsPlanCare)
    {
        DataTable tblItem = new DataTable();
        DataTable dtItem = new DataTable();
        DataTable dt1ItemDetail = new DataTable();
        try
        {
            dtItem = dsPlanCare.Tables[0];


            tblItem = CreateItemTable();
            dt1ItemDetail = CreateItemTable();

            ViewState["ItemDetail"] = tblItem;

            foreach (DataRow drM in dtItem.Rows)
            {
                DataRow DR1 = tblItem.NewRow();
                DR1["Id"] = 0;
                DR1["IndentNo"] = 0;
                DR1["IndentDate"] = string.Empty;
                DR1["IndentTypeId"] = 0;
                DR1["IndentType"] = string.Empty;
                DR1["IndentId"] = 0;
                DR1["OverrideComments"] = string.Empty;
                DR1["OverrideCommentsDrugToDrug"] = string.Empty;
                DR1["OverrideCommentsDrugHealth"] = string.Empty;
                DR1["IsSubstituteNotAllowed"] = 0;
                DR1["ICDCode"] = common.myStr(txtICDCode.Text);
                DR1["StrengthValue"] = string.Empty;
                DR1["ReferanceItemId"] = 0;
                DR1["UnitId"] = common.myInt(drM["UnitId"]);
                DR1["NotToPharmacy"] = false;
                DR1["IsInfusion"] = 0;
                DR1["IsInjection"] = 0;
                DR1["RouteName"] = common.myStr(drM["RouteName"]);
                DR1["Duration"] = common.myInt(drM["Duration"]);
                DR1["Type"] = common.myStr(drM["Type"]);
                DR1["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Type"]);
                DR1["FormulationName"] = DBNull.Value;
                DR1["XMLFrequencyTime"] = string.Empty;
                DR1["XMLVariableDose"] = string.Empty;
                DR1["FoodRelationshipId"] = 0;
                DR1["DoseTypeId"] = 0;
                DR1["VolumeUnitId"] = 0;
                DR1["FlowRateUnit"] = 0;
                DR1["DetailsId"] = 0;
                DR1["FoodRelationship"] = DBNull.Value;
                DR1["FoodRelationshipID"] = common.myInt(drM["FoodRelationshipID"]);// DBNull.Value;
                DR1["FoodRelationshipName"] = common.myStr(drM["FoodRelationshipName"]);// DBNull.Value;
                DR1["ReferanceItemName"] = DBNull.Value;
                DR1["Instructions"] = common.myStr(drM["Instructions"]);
                DR1["DoseTypeName"] = DBNull.Value;
                DR1["Volume"] = string.Empty;
                DR1["InfusionTime"] = string.Empty;
                DR1["TimeUnit"] = string.Empty;
                DR1["TotalVolume"] = string.Empty;
                DR1["FlowRate"] = "0";

                if (common.myInt(drM["itemid"]) > 0)
                {
                    DR1["GenericId"] = DBNull.Value;
                    DR1["GenericName"] = string.Empty;
                    DR1["ItemId"] = common.myInt(drM["itemid"]);
                    DR1["ItemName"] = common.myStr(drM["itemName"]);
                }
                else
                {
                    DR1["GenericId"] = common.myInt(drM["GenericId"]);
                    DR1["GenericName"] = common.myStr(drM["GenericName"]);
                    DR1["ItemId"] = DBNull.Value;
                    DR1["ItemName"] = string.Empty;
                }

                DR1["FormulationId"] = 0;
                DR1["RouteId"] = common.myInt(drM["RouteId"]);
                DR1["StrengthId"] = DBNull.Value;
                DR1["Dose"] = common.myDbl(drM["Dose"]);
                DR1["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                DR1["Frequency"] = common.myStr(drM["Frequency"]);
                DR1["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                DR1["Days"] = common.myInt(drM["Days"]);
                DR1["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                DR1["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                DR1["Qty"] = common.myDbl(drM["Qty"]);
                DR1["UnitName"] = common.myStr(drM["UnitName"]);
                DR1["CustomMedication"] = 0;

                tblItem.Rows.Add(DR1);
                tblItem.AcceptChanges();
                tblItem.TableName = "ItemDetail";
                StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                tblItem.WriteXml(writer);


                DataRow DR = dt1ItemDetail.NewRow();
                DR["Id"] = 0;
                DR["IndentNo"] = 0;
                DR["IndentDate"] = string.Empty;
                DR["IndentTypeId"] = 0;
                DR["IndentType"] = string.Empty;
                DR["IndentId"] = 0;
                DR["OverrideComments"] = string.Empty;
                DR["OverrideCommentsDrugToDrug"] = string.Empty;
                DR["OverrideCommentsDrugHealth"] = string.Empty;
                DR["IsSubstituteNotAllowed"] = 0;
                DR["ICDCode"] = common.myStr(txtICDCode.Text);
                DR["StrengthValue"] = string.Empty;
                DR["ReferanceItemId"] = 0;
                DR["UnitId"] = common.myInt(drM["UnitId"]);
                DR["NotToPharmacy"] = 0;
                DR["IsInfusion"] = 0;
                DR["IsInjection"] = 0;
                DR["RouteName"] = common.myStr(drM["RouteName"]);
                DR["Duration"] = common.myInt(drM["Duration"]);
                DR["Type"] = common.myStr(drM["Type"]);
                DR["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Type"]);
                DR["FormulationName"] = DBNull.Value;
                DR["XMLFrequencyTime"] = string.Empty;
                DR["XMLVariableDose"] = string.Empty;
                DR["FoodRelationshipId"] = common.myInt(drM["FoodRelationshipId"]);//0;
                DR["FoodRelationshipName"] = common.myStr(drM["FoodRelationshipName"]);// DBNull.Value;

                DR["DoseTypeId"] = 0;
                DR["VolumeUnitId"] = 0;
                DR["FlowRateUnit"] = 0;
                DR["DetailsId"] = 0;
                DR["FoodRelationship"] = DBNull.Value;
                DR["ReferanceItemName"] = DBNull.Value;
                DR["Instructions"] = common.myStr(drM["Instructions"]); ;
                DR["DoseTypeName"] = DBNull.Value;
                DR["Volume"] = string.Empty;
                DR["InfusionTime"] = string.Empty;
                DR["TimeUnit"] = string.Empty;
                DR["TotalVolume"] = string.Empty;
                DR["FlowRate"] = "0";

                if (common.myInt(drM["itemid"]) > 0)
                {
                    DR["GenericId"] = DBNull.Value;
                    DR["GenericName"] = string.Empty;
                    DR["ItemId"] = common.myInt(drM["itemid"]);
                    DR["ItemName"] = common.myStr(drM["itemName"]);
                }
                else
                {
                    DR["GenericId"] = common.myInt(drM["GenericId"]);
                    DR["GenericName"] = common.myStr(drM["GenericName"]);
                    DR["ItemId"] = DBNull.Value;
                    DR["ItemName"] = string.Empty;
                }

                DR["FormulationId"] = 0;
                DR["RouteId"] = common.myInt(drM["RouteId"]);
                DR["StrengthId"] = DBNull.Value;
                DR["Dose"] = common.myDbl(drM["Dose"]);
                DR["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                DR["Frequency"] = common.myStr(drM["Frequency"]);
                DR["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                DR["Days"] = common.myInt(drM["Days"]);
                DR["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                DR["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                DR["Qty"] = common.myDbl(drM["Qty"]).ToString("F2");
                DR["UnitName"] = common.myStr(drM["UnitName"]);
                DR["CustomMedication"] = 0;
                DR["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                DR["CIMSType"] = common.myStr(hdnCIMSType.Value);
                DR["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                DR["XMLData"] = common.myStr(writer);
                DR["PrescriptionDetail"] = "";
                dt1ItemDetail.Rows.Add(DR);
            }
            tblItem.AcceptChanges();
            dt1ItemDetail.AcceptChanges();
            ViewState["ItemDetail"] = dt1ItemDetail.Copy();
            ViewState["Item"] = dt1ItemDetail.Copy();



            ddlGeneric.Text = string.Empty;
            ddlBrand.Text = string.Empty;
            lblMessage.Text = string.Empty;
            hdnGenericId.Value = "0";
            hdnItemId.Value = "0";
            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = string.Empty;
            hdnGenericName.Value = string.Empty;
            hdnItemName.Value = string.Empty;
            txtCustomMedication.Text = string.Empty;
            clearItemDetails();
            tblItem = addColumnInItemGrid(tblItem.Copy());
            try
            {
                foreach (DataRow item in tblItem.Rows)
                {
                    if (common.myInt(item["UnAppPrescriptionId"]).Equals(0))
                    {
                        int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(item);
                        item["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                    }
                }
            }
            catch
            {
            }
            ViewState["Item"] = tblItem;
            gvItem.DataSource = tblItem.Copy();
            gvItem.DataBind();
            setVisiblilityInteraction();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            ddlBrand.Focus();
            btnCopyLastPrescription.Enabled = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ViewState["OrderSetId"] = "0";
            tblItem.Dispose();
            dtItem.Dispose();
            dt1ItemDetail.Dispose();
        }
    }
    private void setPrescriptionMandatoryFieldSetup()
    {
        clsIVF objIVF = new clsIVF(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {
            ds = objIVF.getPrescriptionMandatoryFieldSetup(common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]), common.myStr(Session["OPIP"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Dose          DOSE
                    //Unit          DUNT
                    //Frequency     FRQU
                    //Duration      DURA
                    //Route         ROUT
                    //Start Date    STDT

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='DOSE'"; //Dose
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnDose.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='DUNT'"; //Unit
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnUnit.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='FRQU'"; //Frequency     
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnFrequency.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='DURA'"; //Duration      
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnDuration.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='ROUT'"; //Route
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnRoute.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='STDT'"; //Start Date    
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        spnStartdate.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
            {
                spnDuration.Visible = false;
            }

            objIVF = null;
            ds.Dispose();
            dv.Dispose();
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
    protected string PatientandClinicianValidateion()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        StringBuilder strb = new StringBuilder();
        string ValidationFail = string.Empty;
        try
        {
            ds = objEMR.ValidateErxPatientXML(common.myInt(ViewState["RegId"]), common.myInt(Session["UserId"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (common.myStr(ds.Tables[0].Rows[0][i]).Contains("NV"))
                        {
                            ValidationFail = "Failed";
                            strb.Append("<b>" + common.myStr(ds.Tables[0].Columns[i].Caption) + "</b> : Invalid <br/>");
                        }
                        else
                        {
                            strb.Append("<b>" + common.myStr(ds.Tables[0].Columns[i].Caption) + "</b> : " + ds.Tables[0].Rows[0][i] + "<br/>");
                        }
                    }

                    ViewState["IsInsuranceCompany"] = ds.Tables[0].Rows[0]["IsInsuranceCompany"];
                }
            }

            dvInfo.InnerHtml = strb.ToString();

            if (!common.myBool(ViewState["IsInsuranceCompany"]))
            {
                ValidationFail = string.Empty;
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
            objEMR = null;
            strb = null;
        }
        return ValidationFail;
    }
    private void SetPermission()
    {
        UserAuthorisations objUA = new UserAuthorisations(sConString);
        try
        {
            btnSave.Enabled = objUA.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(ViewState["EncId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objUA.Dispose();
        }
    }
    private void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void removeDropDownFromTextBox(TextBox TXT)
    {
        try
        {
            TXT.Attributes.Add("autocomplete", "off");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void bindOnLoadData()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        DataSet dsENC = new DataSet();
        try
        {
            ddlStore.Items.Clear();
            ddlStore.ClearSelection();

            ddlAdvisingDoctor.Items.Clear();
            ddlAdvisingDoctor.ClearSelection();

            ddlIndentType.Items.Clear();
            ddlIndentType.ClearSelection();

            ddlUnit.Items.Clear();
            ddlUnit.ClearSelection();

            ddlVolumeUnit.Items.Clear();
            ddlVolumeUnit.ClearSelection();

            ddlFlowRateUnit.Items.Clear();
            ddlFlowRateUnit.ClearSelection();

            ddlFormulation.Items.Clear();
            ddlFormulation.ClearSelection();

            ddlRoute.Items.Clear();
            ddlRoute.ClearSelection();

            ddlFrequencyId.Items.Clear();
            ddlFrequencyId.ClearSelection();

            ddlFoodRelation.Items.Clear();
            ddlFoodRelation.ClearSelection();

            ddlDoseType.Items.Clear();
            ddlDoseType.ClearSelection();

            DataSet dsPatientDetailPrescription = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];

            if (dsPatientDetailPrescription != null)
            {
                ds = objEMR.GetPrescriptionMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(Session["UserID"]),
                                                    common.myInt(Session["LoginDepartmentId"]), common.myInt(Session["EmployeeId"]),
                                                    common.myInt(dsPatientDetailPrescription.Tables[0].Rows[0]["CurrentWardID"]));
            }
            #region Table 0-Store
            if (ds.Tables.Count > 0)
            {
                ddlStore.DataSource = ds.Tables[0];
                ddlStore.DataTextField = "DepartmentName";
                ddlStore.DataValueField = "StoreId";
                ddlStore.DataBind();
                ddlStore.Items.Insert(0, new ListItem(string.Empty, "0"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myStr(Session["OPIP"]).Equals("I") && Request.QueryString["DRUGORDERCODE"] == null)
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(ViewState["DefaultWardIndentStoreId"])));
                        ViewState["Consumable"] = false;
                    }
                    if (common.myStr(Session["OPIP"]).Equals("I")
                        && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") && Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"))
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(ViewState["DefaultWardIndentStoreId"])));
                        ViewState["Consumable"] = true;
                    }
                    else if (common.myStr(Session["OPIP"]).Equals("O") && Request.QueryString["DRUGORDERCODE"] == null)
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(ViewState["DefaultOPIndentStoreId"])));
                        ViewState["Consumable"] = false;
                    }
                    else if (common.myStr(Session["OPIP"]).Equals("I")
                        && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") && Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Equals("OT"))
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(ViewState["DefaultOTIndentStoreId"])));
                        ViewState["Consumable"] = true;
                    }
                    else if (common.myStr(Session["OPIP"]).Equals("E"))
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(ViewState["DefaultERIndentStoreId"])));
                        ViewState["Consumable"] = false;
                    }
                    hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();
                }
            }
            #endregion
            #region Table 1-Advising Doctor            
            if (ds.Tables.Count > 1)
            {
                ddlAdvisingDoctor.DataSource = ds.Tables[1];
                ddlAdvisingDoctor.DataTextField = "DoctorName";
                ddlAdvisingDoctor.DataValueField = "DoctorId";
                ddlAdvisingDoctor.DataBind();
                ddlAdvisingDoctor.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlAdvisingDoctor.Items.Insert(0, new ListItem(string.Empty, "0"));

                DV = new DataView();
                DV = ds.Tables[1].DefaultView;
                DV.RowFilter = "IsDefault=1";

                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {
                    dsENC = objEMR.getEncounterDoctor(common.myInt(Session["EncounterId"]));
                    if (dsENC.Tables[0].Rows.Count > 0)
                    {
                        if (hdnIsSelectEncounterDoctorId.Value.Equals("0"))
                            ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myInt(dsENC.Tables[0].Rows[0]["ConsultingDoctorId"]).ToString()));
                        else
                            ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myInt(dsENC.Tables[0].Rows[0]["DoctorId"]).ToString()));
                    }
                    else
                    {
                        ddlAdvisingDoctor.SelectedIndex = 0;
                    }

                }
                else
                {
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myInt(DV.ToTable().Rows[0]["DoctorId"]).ToString()));
                        //ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindByValue(common.myInt(DV.ToTable().Rows[0]["DoctorId"]).ToString()));
                    }
                    else
                    {
                        ddlAdvisingDoctor.SelectedIndex = 0;
                    }
                }
            }
            #endregion
            #region Table 2-IndentType            
            if (ds.Tables.Count > 2)
            {
                //if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                //{
                //    DataSet dsIndentType = new DataSet();
                //    dsIndentType = objEMR.GetIndentType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                //    ddlIndentType.DataSource = dsIndentType.Tables[0];
                //    ddlIndentType.DataValueField = "Id";
                //    ddlIndentType.DataTextField = "IndentType";
                //    ddlIndentType.DataBind();
                //}
                //else
                //{
                //    ddlIndentType.DataSource = ds.Tables[2];
                //    ddlIndentType.DataValueField = "Id";
                //    ddlIndentType.DataTextField = "IndentType";
                //    ddlIndentType.DataBind();
                //}

                //string IndentTypeId = string.Empty;


                DataView dvIndentType = new DataView(ds.Tables[2]);
                dvIndentType.RowFilter = "isnull(IndentCode,'')<>'CONOD'";

                foreach (DataRow DR in dvIndentType.ToTable().Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = common.myStr(DR["IndentType"]);
                    item.Value = common.myStr(common.myInt(DR["Id"]));
                    item.Attributes.Add("IndentCode", common.myStr(DR["IndentCode"]));

                    //if (common.myBool(DR["IsFirstIndent"]))
                    //{
                    //    IndentTypeId = common.myStr(DR["Id"]);
                    //}

                    ddlIndentType.Items.Add(item);
                }

                ddlIndentType.SelectedIndex = 0;

                //if (!IndentTypeId.Equals(string.Empty))
                //{
                //    ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindByValue(common.myStr(IndentTypeId)));
                //}
                //else
                //{
                //    ddlIndentType.SelectedIndex = 0;
                //}
            }
            #endregion
            #region Table 3-ProvisionalDiagnosis

            if (ds.Tables.Count > 3)
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[3].Rows[0]["ProvisionalDiagnosis"]);
                    txtProvisionalDiagnosis.ToolTip = txtProvisionalDiagnosis.Text;
                }
            }
            #endregion
            #region Table 4-Order Set
            if (ds.Tables.Count > 4)
            {
                ViewState["OrderSetMaster"] = ds.Tables[4];
                bindOrderSet();
            }
            #endregion
            #region Table 5-ICDCode PatientDiagnosis
            if (ds.Tables.Count > 5)
            {
                ViewState["ICDCodes"] = null;
                txtICDCode.Text = string.Empty;

                if (trDiagnosis.Visible)
                {
                    string sICDCodes = string.Empty;
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        ViewState["ICDCodeMaster"] = ds.Tables[5];

                        for (int idx = 0; idx < ds.Tables[5].Rows.Count; idx++)
                        {
                            if (sICDCodes.Equals(string.Empty))
                            {
                                sICDCodes += common.myStr(ds.Tables[5].Rows[idx]["ICDCode"]);
                            }
                            else
                            {
                                sICDCodes += "," + common.myStr(ds.Tables[5].Rows[idx]["ICDCode"]);
                            }
                        }

                        ViewState["ICDCodes"] = common.myStr(sICDCodes);
                        txtICDCode.Text = common.myStr(sICDCodes);

                        BindICDPanel();
                    }
                }
                else
                {
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        ViewState["ICDCodeMaster"] = ds.Tables[5];
                    }
                }
            }
            #endregion
            #region Table 6-PatientDetails Vital Details
            if (ds.Tables.Count > 6)
            {
                if (ds.Tables[6].Rows.Count > 0)
                {
                    double? weight = null;

                    lbl_Weight.Text = string.Empty;
                    txtHeight.Text = string.Empty;
                    //lbl_BSA.Text = "";
                    for (int idx = 0; idx < ds.Tables[6].Rows.Count; idx++)
                    {
                        if (common.myStr(ds.Tables[6].Rows[idx][0]).Equals("Gender"))
                        {
                            ViewState["PatientGender"] = common.myStr(ds.Tables[6].Rows[idx][1]);
                        }
                        else if (common.myStr(ds.Tables[6].Rows[idx][0]).Equals("Age"))
                        {
                            ViewState["PatientDOB"] = DateTime.Now.AddDays(-common.myInt(ds.Tables[6].Rows[idx][1])).ToString("yyyy-MM-dd");
                        }

                        if (idx > 1)
                        {
                            if (common.myStr(ds.Tables[6].Rows[idx][0]).Equals("WT"))// Weight
                            {
                                weight = common.myDbl(ds.Tables[6].Rows[idx][1]);
                                lbl_Weight.Text = common.myStr(ds.Tables[6].Rows[idx][1]);
                            }
                            else if (common.myStr(ds.Tables[6].Rows[idx][0]).Equals("HT"))// Height
                            {
                                txtHeight.Text = common.myStr(ds.Tables[6].Rows[idx][1]);
                            }
                            //else if (common.myStr(ds.Tables[6].Rows[idx][0]).Equals("BSA"))
                            //{
                            //    lbl_BSA.Text = common.myStr(ds.Tables[6].Rows[idx][1]);
                            //}
                        }
                    }

                    if (common.myDbl(weight) > 0)
                    {
                        ViewState["PatientWeight"] = common.myDbl(weight);
                    }
                }
            }
            #endregion
            #region Table 7-Unit
            if (ds.Tables.Count > 7)
            {
                ViewState["UnitMaster"] = ds.Tables[7];


                foreach (DataRow DR in ds.Tables[7].Rows)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = common.myStr(DR["UnitName"]);
                    item.Value = common.myStr(common.myInt(DR["Id"]));
                    item.Attributes.Add("IsUnitCalculationRequired", common.myStr(DR["IsUnitCalculationRequired"]));
                    ddlUnit.Items.Add(item);
                }


                ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlUnit.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlUnit.SelectedIndex = 0;

                DV = new DataView();
                DV = ds.Tables[7].DefaultView;
                DV.RowFilter = "IsVolumeUnit=1";

                ddlVolumeUnit.DataSource = DV.ToTable();
                ddlVolumeUnit.DataValueField = "Id";
                ddlVolumeUnit.DataTextField = "UnitName";
                ddlVolumeUnit.DataBind();
                ddlVolumeUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlVolumeUnit.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlVolumeUnit.SelectedIndex = 0;

                DataView DV1 = new DataView();
                DV1 = ds.Tables[7].DefaultView;
                DV1.RowFilter = "IsInfusionUnit=1";


                ddlFlowRateUnit.DataSource = DV1.ToTable();
                ddlFlowRateUnit.DataValueField = "Id";
                ddlFlowRateUnit.DataTextField = "UnitName";
                ddlFlowRateUnit.DataBind();
                ddlFlowRateUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlFlowRateUnit.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlFlowRateUnit.SelectedIndex = 0;
                DV1.Dispose();
            }
            #endregion
            #region Table 8-Formulation
            if (ds.Tables.Count > 8)
            {
                foreach (DataRow DR in ds.Tables[8].Rows)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    //ListItem item = new ListItem();
                    item.Text = common.myStr(DR["FormulationName"]);
                    item.Value = common.myStr(common.myInt(DR["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(DR["DefaultRouteId"]));

                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlFormulation.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlFormulation.SelectedIndex = 0;
            }
            #endregion
            #region Table 9-Route            
            if (ds.Tables.Count > 9)
            {
                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    ddlRoute.DataSource = ds.Tables[9];
                    ddlRoute.DataValueField = "Id";
                    ddlRoute.DataTextField = "RouteName";
                    ddlRoute.DataBind();
                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                    //ddlRoute.Items.Insert(0, new ListItem(string.Empty, "0"));
                    ddlRoute.SelectedIndex = 0;
                }
            }
            #endregion
            #region Table 10-Frequency           
            if (ds.Tables.Count > 10)
            {
                foreach (DataRow dr in ds.Tables[10].Rows)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    //ListItem item = new ListItem();
                    item.Text = common.myStr(dr["Description"]);
                    item.Value = common.myStr(common.myInt(dr["Id"]));
                    item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));

                    ddlFrequencyId.Items.Add(item);
                }
                if (ddlFrequencyId != null)
                {
                    if (ddlFrequencyId.FindItemByValue("0") == null)
                    {
                        ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                    }
                }
                //ddlFrequencyId.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlFrequencyId.SelectedIndex = 0;
            }
            #endregion
            #region Table 11-Food            
            if (ds.Tables.Count > 11)
            {
                ddlFoodRelation.DataSource = ds.Tables[11];
                ddlFoodRelation.DataValueField = "Id";
                ddlFoodRelation.DataTextField = "FoodName";
                ddlFoodRelation.DataBind();
                ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlFoodRelation.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlFoodRelation.SelectedIndex = 0;
            }
            #endregion
            #region Table 12-DoseType            
            if (ds.Tables.Count > 12)
            {
                ddlDoseType.DataSource = ds.Tables[12];
                ddlDoseType.DataValueField = "StatusId";
                ddlDoseType.DataTextField = "Status";
                ddlDoseType.DataBind();
                ddlDoseType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlDoseType.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlDoseType.SelectedIndex = 0;
            }
            #endregion
            #region Table 13-FacilityInterfaceDetails
            if (ds.Tables.Count > 13)
            {
                chkShowDetails.Visible = false;

                if (ds.Tables[13].Rows.Count > 0)
                {
                    if (common.myBool(ds.Tables[13].Rows[0]["IsCIMSInterfaceActive"]))
                    {
                        Session["IsCIMSInterfaceActive"] = common.myBool(ds.Tables[13].Rows[0]["IsCIMSInterfaceActive"]);
                        Session["CIMSDatabasePath"] = common.myStr(ds.Tables[13].Rows[0]["CIMSDatabasePath"]);
                        Session["CIMSDatabasePassword"] = common.myStr(ds.Tables[13].Rows[0]["CIMSDatabasePassword"]);
                        chkShowDetails.Visible = true;
                        chkShowDetails.Text = "Show CIMS Details";
                    }
                    else if (common.myBool(ds.Tables[13].Rows[0]["IsVIDALInterfaceActive"]))
                    {
                        Session["IsVIDALInterfaceActive"] = common.myBool(ds.Tables[13].Rows[0]["IsVIDALInterfaceActive"]);
                        chkShowDetails.Visible = true;
                        chkShowDetails.Text = "Show VIDAL Details";
                    }
                }
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    string CIMSDatabasePath = string.Empty;
                    if (ds.Tables[13].Rows.Count > 0)
                    {
                        CIMSDatabasePath = common.myStr(ds.Tables[13].Rows[0]["CIMSDatabasePath"]);
                    }
                    if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc") && !File.Exists(CIMSDatabasePath + "FastTrackData.mr2"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "CIMS database not available !";
                    }
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    try
                    {
                        //VSDocumentService.documentServiceClient objDocumentService;
                        //objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");
                        //WebClient client = new WebClient();
                        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sVidalConString + "DocumentService");
                        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        //if (response.StatusCode != HttpStatusCode.OK)
                        //{
                        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //    lblMessage.Text = "VIDAL web-services not running now !";
                        //}
                    }
                    catch
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "VIDAL web-services not running now !";
                    }
                }

                setDiagnosis();
            }
            #endregion
            #region Table 14-DrugAllergiesInterfaceCode
            if (ds.Tables.Count > 14)
            {
                if (ds.Tables[14].Rows.Count > 0)
                {
                    ViewState["DrugAllergiesInterfaceCode"] = ds.Tables[14];
                    setAllergiesWithInterfaceCode();
                }
            }
            #endregion
            #region Table 15-Interface LegendColor
            if (ds.Tables.Count > 15)
            {
                ViewState["BrandDetailsColor"] = "#FFC48A";
                ViewState["DrugMonographColor"] = "#98AFC7";
                ViewState["DrugtoDrugInteractionColor"] = "#ECBBBB";
                ViewState["DrugHealthInteractionColor"] = "#82AB76";
                ViewState["DrugAllergyColor"] = "#82CAFA";
                ViewState["DuplicateIngredient"] = "#F36F6FD9";

                if (common.myBool(Session["IsCIMSInterfaceActive"])
                    || common.myBool(Session["IsVIDALInterfaceActive"]))
                {

                    if (ds.Tables[15].Rows.Count > 0)
                    {
                        ds.Tables[15].DefaultView.RowFilter = "Code='BD'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["BrandDetailsColor"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }
                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                        ds.Tables[15].DefaultView.RowFilter = "Code='MO'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["DrugMonographColor"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }
                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                        ds.Tables[15].DefaultView.RowFilter = "Code='IN'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["DrugtoDrugInteractionColor"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }
                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                        ds.Tables[15].DefaultView.RowFilter = "Code='HI'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["DrugHealthInteractionColor"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }
                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                        ds.Tables[15].DefaultView.RowFilter = "Code='DA'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["DrugAllergyColor"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }

                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                        ds.Tables[15].DefaultView.RowFilter = "Code='DI'";
                        if (ds.Tables[15].DefaultView.Count > 0)
                        {
                            ViewState["DuplicateIngredient"] = ds.Tables[15].DefaultView[0]["StatusColor"];
                        }
                        ds.Tables[15].DefaultView.RowFilter = string.Empty;
                    }
                }
            }
            #endregion
            #region Table 16-UnApprovedPrescriptions
            if (ds.Tables.Count > 16)
            {
                if (ds.Tables[16].Rows.Count > 0)
                {
                    ViewState["UnApprovedPrescriptions"] = ds.Tables[16];
                }
            }
            #endregion
            #region Table 17-CheckDiagnosisPrimaryForPatient
            if (ds.Tables.Count > 17)
            {
                if (ds.Tables[17].Rows.Count > 0)
                {
                    ViewState["IsPrimaryDiagnosis"] = (common.myInt(ds.Tables[17].Rows[0]["CNT"]) > 0) ? true : false;
                }
            }
            #endregion
            #region Table 18-IsEPrescriptionEnabled
            if (ds.Tables.Count > 18)
            {
                if (ds.Tables[18].Rows.Count > 0)
                {
                    ViewState["IsEPrescriptionEnabled"] = common.myBool(ds.Tables[18].Rows[0]["IsEPrescriptionEnabled"]);
                }
            }
            #endregion
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            ds.Dispose();
            DV.Dispose();
        }
    }
    private void bindOnLoadDataBrandBase(int FormId, int RouteId, int UnitId)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        DataSet dsENC = new DataSet();
        try
        {




            DataSet dsPatientDetailPrescription = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];

            if (dsPatientDetailPrescription != null)
            {
                ds = objEMR.GetPrescriptionMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(Session["UserID"]),
                                                    common.myInt(Session["LoginDepartmentId"]), common.myInt(Session["EmployeeId"]),
                                                    common.myInt(dsPatientDetailPrescription.Tables[0].Rows[0]["CurrentWardID"]), FormId);
            }

            #region Table 7-Unit
            if (ds.Tables.Count > 7)
            {
                ddlUnit.Items.Clear();
                ddlUnit.ClearSelection();

                ViewState["UnitMaster"] = ds.Tables[7];


                foreach (DataRow DR in ds.Tables[7].Rows)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = common.myStr(DR["UnitName"]);
                    item.Value = common.myStr(common.myInt(DR["Id"]));
                    item.Attributes.Add("IsUnitCalculationRequired", common.myStr(DR["IsUnitCalculationRequired"]));
                    ddlUnit.Items.Add(item);
                }


                ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlUnit.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(UnitId).ToString()));



            }
            #endregion
            #region Table 9-Route            
            if (ds.Tables.Count > 9)
            {
                ddlRoute.Items.Clear();
                ddlRoute.ClearSelection();

                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    ddlRoute.DataSource = ds.Tables[9];
                    ddlRoute.DataValueField = "Id";
                    ddlRoute.DataTextField = "RouteName";
                    ddlRoute.DataBind();
                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                    //ddlRoute.Items.Insert(0, new ListItem(string.Empty, "0"));
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(RouteId).ToString()));

                }
            }
            #endregion
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            ds.Dispose();
            DV.Dispose();
        }
    }
    private void bindOrderSet()
    {
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            if (common.myStr(rdoSearchMedication.SelectedValue).Equals("OS"))
            {
                if (ViewState["OrderSetMaster"] != null)
                {
                    tbl = (DataTable)ViewState["OrderSetMaster"];
                    if (tbl != null)
                    {
                        DV = tbl.Copy().DefaultView;
                        if (common.myLen(txtOrderSetName.Text) > 0)
                        {
                            DV.RowFilter = "SetName LIKE '%" + common.myStr(txtOrderSetName.Text, true) + "%'";
                        }

                        gvOrderSet.DataSource = DV.ToTable();
                        gvOrderSet.DataBind();

                        DV.RowFilter = string.Empty;
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
            DV.Dispose();
        }
    }
    private bool IsPasswordRequired()
    {
        bool IsRequired = false;
        try
        {
            foreach (GridViewRow dataItem in gvItem.Rows)
            {
                HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");
                if (common.myLen(hdnCommentsDrugAllergy.Value) > 0
                    || common.myLen(hdnCommentsDrugToDrug.Value) > 0
                    || common.myLen(hdnCommentsDrugHealth.Value) > 0)
                {
                    IsRequired = IsRequired | true;
                }
            }
        }
        catch
        { }
        return IsRequired;
    }
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                lblMessage.Text = "Invalid Username/Password!";
                return;
            }
            saveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Onclick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(ViewState["EncId"]).Equals(0))
            {
                lblMessage.Text = "Patient has no appointment !";
                return;
            }
            if (common.myInt(ddlAdvisingDoctor.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Advising Doctor not selected!";
                return;
            }

            if (common.myBool(hdnIsPasswordRequired.Value)) //If transactional username & password required
            {
                IsValidPassword();
                return;
            }

            if (common.myBool(ViewState["IsReasonManditory"]))
            {
                if (common.myInt(ddlReason.SelectedValue).Equals(0))
                {
                    lblMessage.Text = "Reason not selected !";
                    return;
                }
            }

            try
            {
                //added by bhakti
                if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["PatientCompanyType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            saveData();
            btnAddItem.Enabled = true;
            CheckNoOfIndentInOneDay();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void saveData()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataSet dsXmlFrequency = new DataSet();
        DataSet dsVariableDose = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        Hashtable hshOutput = new Hashtable();
        try
        {
            StringBuilder strXML = new StringBuilder();
            StringBuilder strXML1 = new StringBuilder();
            StringBuilder strXMLUnAppPrescIds = new StringBuilder();
            ArrayList coll = new ArrayList();
            ArrayList coll1 = new ArrayList();
            ArrayList collUnAppPrescIds = new ArrayList();
            StringBuilder strXMLFre = new StringBuilder();
            ArrayList collFre = new ArrayList();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(ViewState["EncId"]).Equals(0))
            {
                lblMessage.Text = "Patient has no appointment !";
                return;
            }
            if (common.myInt(ddlAdvisingDoctor.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Advising Doctor not selected!";
                return;
            }
            double iConversionFactor = 0;
            double sQuantity = 0;

            // Akshay_Tirathram
            DataTable ItemExpensiveDetails = new DataTable();
            ItemExpensiveDetails.Columns.Add("ItemId");
            ItemExpensiveDetails.Columns.Add("ItemName");
            ItemExpensiveDetails.Columns.Add("IsExpensive");
            ItemExpensiveDetails.Columns.Add("OrderedQty");
            ItemExpensiveDetails.Columns.Add("MaxNarcoticDrugsQty");

            if (common.myStr(Session["OPIP"]).Equals("O") || common.myStr(Session["OPIP"]).Equals("E"))
            {
                #region OP Drug
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    coll1 = new ArrayList();
                    HiddenField hdnGDGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnGDItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnUnAppPrescriptionId = (HiddenField)dataItem.FindControl("hdnUnAppPrescriptionId");
                    HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");//Akshay_Tirathram

                    // Akshay_Tirathram
                    DataTable tempDt = new DataTable();
                    tempDt = objPhr.GetPhrItemMaster(Convert.ToInt32(hdnGDItemId.Value), "", 0, common.myInt(Session["HospitalLocationId"])).Tables[1];
                    ItemExpensiveDetails.Rows.Add();
                    int index = ItemExpensiveDetails.Rows.Count;
                    if (tempDt.Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["ItemId"] = tempDt.Rows[0]["ItemId"];
                        ItemExpensiveDetails.Rows[index - 1]["ItemName"] = tempDt.Rows[0]["ItemName"];
                        ItemExpensiveDetails.Rows[index - 1]["IsExpensive"] = tempDt.Rows[0]["IsExpensive"];
                    }
                    DataSet tempDs1 = new DataSet();
                    tempDs1 = objPhr.GetNarcoticDrugsDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]), Convert.ToInt32(hdnGDItemId.Value));
                    if (tempDs1.Tables[0].Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["MaxNarcoticDrugsQty"] = tempDs1.Tables[0].Rows[0]["MaxDrugDose"];
                    }
                    ItemExpensiveDetails.Rows[index - 1]["OrderedQty"] = common.myInt(hdnDose.Value);
                    tempDt = null;
                    tempDs1 = null;


                    if ((ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                        || common.myInt(hdnReturnIndentDetailsIds.Value) > 0
                        || common.myBool(hdnCopyLastPresc.Value)
                        || common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        hdnIndentId.Value = "0";
                    }
                    //if (Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO")
                    //    && common.myInt(txtTotalQty.Text).Equals(0))
                    //{
                    //    Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                    //    return;
                    //}
                    if (common.myInt(hdnIndentId.Value).Equals(0))
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");
                        HiddenField hdnCustomMedication = (HiddenField)dataItem.FindControl("hdnCustomMedication");
                        HiddenField hdnXmlVariableDose = (HiddenField)dataItem.FindControl("hdnXmlVariableDose");

                        if (common.myBool(hdnCustomMedication.Value))
                        {
                            coll1.Add(DBNull.Value);//GenericId int
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            if (common.myInt(hdnGDGenericId.Value) > 0 && common.myInt(hdnGDItemId.Value).Equals(0))
                            {
                                coll1.Add(hdnGDGenericId.Value);//GenericId int
                                coll1.Add(DBNull.Value);//ItemId INT,
                                coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                            }
                            else
                            {
                                coll1.Add(hdnGDGenericId.Value);//GenericId int
                                coll1.Add(common.myInt(hdnGDItemId.Value));//ItemId INT,
                                coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                            }
                        }

                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT
                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (common.myStr(hdnXMLData.Value).Trim().Equals(string.Empty))
                        {
                            lblMessage.Text = "Please add drug to save.";
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);
                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        if (common.myInt(hdnGDItemId.Value) > 0)
                        {
                            if (common.myStr(hdnStartDate.Value) != "")
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "'";
                            }
                            else
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + "";
                            }
                        }
                        else
                        {
                            if (common.myInt(hdnGDGenericId.Value) > 0)
                            {
                                if (common.myStr(hdnStartDate.Value) != "")
                                {
                                    dv.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGDGenericId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "' ";
                                }
                                else
                                {
                                    dv.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGDGenericId.Value) + "";
                                }
                            }
                            else
                            {
                                if (common.myStr(hdnStartDate.Value) != "")
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "' And ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                                }
                                else
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + "  And ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                                }

                            }
                        }
                        dt = dv.ToTable();

                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("Dose"))
                                    {
                                        dt.Columns.Add("Dose", typeof(double));
                                    }
                                    if (!dt.Columns.Contains("Duration"))
                                    {
                                        dt.Columns.Add("Duration", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Type"))
                                    {
                                        dt.Columns.Add("Type", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Instructions"))
                                    {
                                        dt.Columns.Add("Instructions", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("ReferanceItemId"))
                                    {
                                        dt.Columns.Add("ReferanceItemId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationshipID"))
                                    {
                                        dt.Columns.Add("FoodRelationshipID", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("DoseTypeId"))
                                    {
                                        dt.Columns.Add("DoseTypeId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FormulationId"))
                                    {
                                        dt.Columns.Add("FormulationId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("RouteId"))
                                    {
                                        dt.Columns.Add("RouteId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("InfusionTime"))
                                    {
                                        dt.Columns.Add("InfusionTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("TotalVolume"))
                                    {
                                        dt.Columns.Add("TotalVolume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Volume"))
                                    {
                                        dt.Columns.Add("Volume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("VolumeUnitId"))
                                    {
                                        dt.Columns.Add("VolumeUnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("TimeUnit"))
                                    {
                                        dt.Columns.Add("TimeUnit", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRate"))
                                    {
                                        dt.Columns.Add("FlowRate", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRateUnit"))
                                    {
                                        dt.Columns.Add("FlowRateUnit", typeof(int));
                                    }

                                    try
                                    {
                                        if (!dt.Columns.Contains("XMLVariableDose"))
                                        {
                                            dt.Columns.Add("XMLVariableDose", typeof(string));
                                        }
                                    }
                                    catch
                                    { }
                                    if (!dt.Columns.Contains("IsSubstituteNotAllowed"))
                                    {
                                        dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                    }
                                    if (!dt.Columns.Contains("ICDCode"))
                                    {
                                        dt.Columns.Add("ICDCode", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("XMLFrequencyTime"))
                                    {
                                        dt.Columns.Add("XMLFrequencyTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("DurationText"))
                                    {
                                        dt.Columns.Add("DurationText", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationship"))
                                    {
                                        dt.Columns.Add("FoodRelationship", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("StartDate"))
                                    {
                                        dt.Columns.Add("StartDate", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("GenericId"))
                                    {
                                        dt.Columns.Add("GenericId", typeof(int));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Telerik.Web.UI.RadDatePicker dtStartTime = new Telerik.Web.UI.RadDatePicker();
                            Telerik.Web.UI.RadDatePicker dtEndTime = new Telerik.Web.UI.RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = common.myStr(dt.Rows[i]["Dose"]);
                                //string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = common.myStr(dt.Rows[i]["Duration"]);
                                string sType = common.myStr(dt.Rows[i]["Type"]);
                                if (common.myDbl(sDose) <= 0 && spnDose.Visible)
                                {
                                    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    return;
                                }
                                if (!common.myStr(dt.Rows[i]["XMLFrequencyTime"]).Trim().Equals(string.Empty))//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                    dsXmlFrequency.Dispose();
                                }
                                //if (!common.myStr(dt.Rows[i]["XMLVariableDose"]).Trim().Equals(string.Empty))//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                if (common.myLen(hdnXmlVariableDose.Value) > 0)
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(hdnXmlVariableDose.Value));//new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);
                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);
                                            if (!dt.Columns.Contains("UnitId"))
                                            {
                                                dt.Columns.Add("UnitId", typeof(int));
                                            }
                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i.Equals(0))
                                            {
                                                if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = common.myStr(dt.Rows[i]["StartDate"]);
                                                    // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                                }
                                            }
                                            if (i.Equals((dt.Rows.Count - 1)))
                                            {
                                                if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                                {
                                                    sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                                        //sStartDate = dt.Rows[i]["StartDate"].ToString();
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            string sInstructions = common.myStr(dt.Rows[i]["Instructions"]);
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);
                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            string strICDCode = common.myStr(dt.Rows[i]["ICDCode"]);
                                            int intGenericId = common.myInt(dt.Rows[i]["GenericId"]);

                                            coll.Add(common.myInt(hdnGDItemId.Value));//ItemId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            //  coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//
                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(Col);// variable sequence no         
                                            coll.Add(bitIsSubstituteNotAllow);
                                            coll.Add(strICDCode);
                                            coll.Add(common.myInt(hdnGDGenericId.Value));

                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i.Equals(0))
                                    {
                                        if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = common.myStr(dt.Rows[i]["StartDate"]);
                                            // sEndDate = dt.Rows[i]["EndDate"].ToString();
                                        }
                                    }
                                    if (i.Equals((dt.Rows.Count - 1)))
                                    {
                                        if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                        {
                                            if (!common.myLen(sDuration).Equals(0))
                                            {
                                                sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                sEndDate = sStartDate;
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                                //sStartDate = dt.Rows[i]["StartDate"].ToString();
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    string sInstructions = common.myStr(dt.Rows[i]["Instructions"]);
                                    int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);
                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    string strICDCode = common.myStr(dt.Rows[i]["ICDCode"]);
                                    int intGenericId = common.myInt(dt.Rows[i]["GenericId"]);
                                    //if (common.myInt(iUnit) == 0)
                                    //{
                                    //    Alert.ShowAjaxMsg("Quantity should not be Zero !", this.Page);
                                    //    return;
                                    //}
                                    coll.Add(common.myInt(hdnGDItemId.Value));//ItemId int, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//
                                    coll.Add(string.Empty);//
                                    coll.Add(string.Empty);// variable sequence no
                                    coll.Add(bitIsSubstituteNotAllow);
                                    coll.Add(strICDCode);
                                    coll.Add(common.myInt(hdnGDGenericId.Value));

                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }
                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");
                            coll1.Add(!(sStartDate.Equals(string.Empty)) ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(!(sEndDate.Equals(string.Empty)) ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
                            coll1.Add(DBNull.Value);//ICD CODE VARCHAR
                            coll1.Add(common.myInt(0));//Refill INT
                            coll1.Add(common.myBool(0));//Is Override BIT
                            coll1.Add(hdnCommentsDrugAllergy.Value);//OverrideComments VARCHAR
                            coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
                            coll1.Add(common.myInt(424));//PrescriptionModeId INT
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)

                            strXML1.Append(common.setXmlTable(ref coll1));

                            if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                            {
                                collUnAppPrescIds = new ArrayList();
                                collUnAppPrescIds.Add(common.myInt(hdnUnAppPrescriptionId.Value));
                                strXMLUnAppPrescIds.Append(common.setXmlTable(ref collUnAppPrescIds));
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region IP Drug
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    coll1 = new ArrayList();
                    HiddenField hdnGDGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnGDItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                    HiddenField hdnUnAppPrescriptionId = (HiddenField)dataItem.FindControl("hdnUnAppPrescriptionId");
                    HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");//Akshay_Tirathram

                    // Akshay_Tirathram
                    DataTable tempDt = new DataTable();
                    tempDt = objPhr.GetPhrItemMaster(Convert.ToInt32(hdnGDItemId.Value), "", 0, common.myInt(Session["HospitalLocationId"])).Tables[1];
                    ItemExpensiveDetails.Rows.Add();
                    int index = ItemExpensiveDetails.Rows.Count;
                    if (tempDt.Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["ItemId"] = tempDt.Rows[0]["ItemId"];
                        ItemExpensiveDetails.Rows[index - 1]["ItemName"] = tempDt.Rows[0]["ItemName"];
                        ItemExpensiveDetails.Rows[index - 1]["IsExpensive"] = tempDt.Rows[0]["IsExpensive"];
                    }
                    DataSet tempDs1 = new DataSet();
                    tempDs1 = objPhr.GetNarcoticDrugsDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]), Convert.ToInt32(hdnGDItemId.Value));
                    if (tempDs1.Tables[0].Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["MaxNarcoticDrugsQty"] = tempDs1.Tables[0].Rows[0]["MaxDrugDose"];
                    }
                    ItemExpensiveDetails.Rows[index - 1]["OrderedQty"] = common.myInt(hdnDose.Value);
                    tempDt = null;
                    tempDs1 = null;

                    if ((ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                        || common.myInt(hdnReturnIndentDetailsIds.Value) > 0
                        || common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        hdnIndentId.Value = "0";
                    }
                    //if (Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO")
                    //    && common.myInt(txtTotalQty.Text).Equals(0))
                    //{
                    //    Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                    //    return;
                    //}
                    if (common.myInt(hdnIndentId.Value).Equals(0))
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");
                        HiddenField hdnCustomMedication = (HiddenField)dataItem.FindControl("hdnCustomMedication");
                        HiddenField hdnXmlVariableDose = (HiddenField)dataItem.FindControl("hdnXmlVariableDose");

                        coll1.Add(common.myInt(hdnIndentId.Value));//FrequencyId TINYINT,

                        if (common.myBool(hdnCustomMedication.Value))
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                            coll1.Add(DBNull.Value);//GenericId int
                        }
                        else
                        {
                            if (common.myInt(hdnGDGenericId.Value) > 0 && common.myInt(hdnGDItemId.Value).Equals(0))
                            {
                                coll1.Add(DBNull.Value);//ItemId INT,
                                coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                                coll1.Add(hdnGDGenericId.Value);//GenericId int
                            }
                            else
                            {
                                coll1.Add(common.myInt(hdnGDItemId.Value));//ItemId INT,
                                coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                                coll1.Add(hdnGDGenericId.Value);//GenericId int
                            }
                        }

                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT
                        coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (common.myStr(hdnXMLData.Value).Trim().Equals(string.Empty))
                        {
                            lblMessage.Text = "Please add drug to save.";
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value).Trim();
                        StringReader sr = new StringReader(xmlSchema);
                        dsXml.ReadXml(sr);

                        if (!dsXml.Tables[0].Columns.Contains("StartDate"))
                        {
                            dsXml.Tables[0].Columns.Add("StartDate", typeof(string));
                        }

                        dv = new DataView(dsXml.Tables[0]);

                        if (common.myLen(hdnStartDate.Value).Equals(0))
                        {
                            if (common.myInt(hdnGDItemId.Value) > 0)
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                            }
                            else
                            {
                                if (common.myInt(hdnGDGenericId.Value) > 0)
                                {
                                    dv.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGDGenericId.Value);
                                }
                                else
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                                }
                            }
                        }
                        else
                        {
                            if (common.myInt(hdnGDItemId.Value) > 0)
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "'";
                            }
                            else
                            {
                                if (common.myInt(hdnGDGenericId.Value) > 0)
                                {
                                    dv.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGDGenericId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "' ";
                                }
                                else
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + common.myStr(hdnStartDate.Value) + "' And ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                                }
                            }
                        }
                        dt = dv.ToTable();
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("Dose"))
                                    {
                                        dt.Columns.Add("Dose", typeof(double));
                                    }
                                    if (!dt.Columns.Contains("Duration"))
                                    {
                                        dt.Columns.Add("Duration", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Type"))
                                    {
                                        dt.Columns.Add("Type", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Instructions"))
                                    {
                                        dt.Columns.Add("Instructions", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("ReferanceItemId"))
                                    {
                                        dt.Columns.Add("ReferanceItemId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationshipID"))
                                    {
                                        dt.Columns.Add("FoodRelationshipID", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("DoseTypeId"))
                                    {
                                        dt.Columns.Add("DoseTypeId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("UnitId"))
                                    {
                                        dt.Columns.Add("UnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FormulationId"))
                                    {
                                        dt.Columns.Add("FormulationId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("FrequencyId"))
                                    {
                                        dt.Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("RouteId"))
                                    {
                                        dt.Columns.Add("RouteId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("InfusionTime"))
                                    {
                                        dt.Columns.Add("InfusionTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("TotalVolume"))
                                    {
                                        dt.Columns.Add("TotalVolume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("Volume"))
                                    {
                                        dt.Columns.Add("Volume", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("VolumeUnitId"))
                                    {
                                        dt.Columns.Add("VolumeUnitId", typeof(int));
                                    }
                                    if (!dt.Columns.Contains("TimeUnit"))
                                    {
                                        dt.Columns.Add("TimeUnit", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRate"))
                                    {
                                        dt.Columns.Add("FlowRate", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FlowRateUnit"))
                                    {
                                        dt.Columns.Add("FlowRateUnit", typeof(int));
                                    }

                                    try
                                    {
                                        if (!dt.Columns.Contains("XMLVariableDose"))
                                        {
                                            dt.Columns.Add("XMLVariableDose", typeof(string));
                                        }
                                    }
                                    catch
                                    { }
                                    if (!dt.Columns.Contains("IsSubstituteNotAllowed"))
                                    {
                                        dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                    }
                                    if (!dt.Columns.Contains("ICDCode"))
                                    {
                                        dt.Columns.Add("ICDCode", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("XMLFrequencyTime"))
                                    {
                                        dt.Columns.Add("XMLFrequencyTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("DurationText"))
                                    {
                                        dt.Columns.Add("DurationText", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("FoodRelationship"))
                                    {
                                        dt.Columns.Add("FoodRelationship", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("GenericId"))
                                    {
                                        dt.Columns.Add("GenericId", typeof(int));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        if (dt.Rows.Count > 0)
                        {
                            Telerik.Web.UI.RadDatePicker dtStartTime = new Telerik.Web.UI.RadDatePicker();
                            Telerik.Web.UI.RadDatePicker dtEndTime = new Telerik.Web.UI.RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = common.myStr(dt.Rows[i]["Dose"]);
                                string sDuration = common.myStr(dt.Rows[i]["Duration"]);
                                string sType = common.myStr(dt.Rows[i]["Type"]);
                                if (common.myDbl(sDose) <= 0 && spnDose.Visible)
                                {
                                    Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    return;
                                }
                                if (!common.myStr(dt.Rows[i]["XMLFrequencyTime"]).Trim().Equals(string.Empty))//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    dsXmlFrequency = new DataSet();
                                    dsXmlFrequency.ReadXml(srFrequency);
                                    dv = new DataView(dsXmlFrequency.Tables[0]);
                                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                                    if (dv.ToTable().Rows.Count > 0)
                                    {
                                        for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                        {
                                            string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                            int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                            string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                            int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                            bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                            collFre.Add(common.myInt(ItemId));//ItemId INT,
                                            collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                            collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                            collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                            collFre.Add(DoseEnable);//FrequencyDetailId INT
                                            strXMLFre.Append(common.setXmlTable(ref collFre));
                                        }
                                    }
                                }


                                //if (!common.myStr(dt.Rows[i]["XMLFrequencyTime"]).Trim().Equals(string.Empty))//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                //{
                                //    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                //    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                //    dsXmlFrequency = new DataSet();
                                //    dsXmlFrequency.ReadXml(srFrequency);
                                else if (common.myInt(dsXml.Tables.Count) > 3)
                                {
                                    if (dsXml.Tables[3].Rows.Count > 0)
                                    {
                                        dv = new DataView(dsXml.Tables[3]);
                                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                                        if (dv.ToTable().Rows.Count > 0)
                                        {
                                            for (int fr = 0; fr < dv.ToTable().Rows.Count; fr++)
                                            {
                                                string FrequencyId = common.myStr(dv.ToTable().Rows[fr]["FrequencyId"]);
                                                int ItemId = common.myInt(dv.ToTable().Rows[fr]["ItemId"]);
                                                string DoseTime = common.myStr(dv.ToTable().Rows[fr]["DoseTime"]);
                                                int FrequencyDetailId = common.myInt(dv.ToTable().Rows[fr]["FrequencyDetailId"]);
                                                bool DoseEnable = common.myBool(dv.ToTable().Rows[fr]["DoseEnable"]);
                                                collFre.Add(common.myInt(ItemId));//ItemId INT,
                                                collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                                                collFre.Add(common.myStr(DoseTime));//DoseTime String 
                                                collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                                                collFre.Add(DoseEnable);//FrequencyDetailId INT
                                                strXMLFre.Append(common.setXmlTable(ref collFre));
                                            }
                                        }
                                    }
                                }
                                //  }

                                // return;

                                //if (!common.myStr(dt.Rows[i]["XMLVariableDose"]).Trim().Equals(string.Empty))//&& (common.myStr( dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                if (common.myLen(hdnXmlVariableDose.Value) > 0)
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(hdnXmlVariableDose.Value));//new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);
                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);
                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i.Equals(0))
                                            {
                                                if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                                {
                                                    sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sStartDate = common.myStr(dt.Rows[i]["StartDate"]);
                                                }
                                            }
                                            if (i.Equals((dt.Rows.Count - 1)))
                                            {
                                                if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                                {
                                                    if (!common.myLen(sDuration).Equals(0))
                                                    {
                                                        sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                                    }
                                                    else
                                                    {
                                                        sEndDate = sStartDate;
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            string sInstructions = common.myStr(dt.Rows[i]["Instructions"]);
                                            int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                            int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                            int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);
                                            string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                            int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                            string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                            string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                            string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                            string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                            string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                            bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                            string strICDCode = common.myStr(dt.Rows[i]["ICDCode"]);
                                            int intGenericId = common.myInt(dt.Rows[i]["GenericId"]);
                                            coll.Add(common.myInt(hdnGDItemId.Value));//ItemId TINYINT, 
                                            coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                            // coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                            coll.Add(common.myDec(variableDose));
                                            coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                            coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                            coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                            coll.Add(common.myInt(iUnit));//UNITID INT,
                                            coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                            coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                            coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
                                            coll.Add(Volume);//
                                            coll.Add(VolumeUnitId);//
                                            coll.Add(InfusionTime);//
                                            coll.Add(TimeUnit);//
                                            coll.Add(TotalVolume);//
                                            coll.Add(FlowRate);//
                                            coll.Add(FlowRateUnit);//
                                            coll.Add(common.myDate(variableDoseDate).ToString("yyyy-MM-dd"));//
                                            coll.Add(bitIsSubstituteNotAllow);
                                            coll.Add(strICDCode);

                                            coll.Add(Col);//SequenceNo tinyint,
                                            coll.Add(common.myInt(hdnGDGenericId.Value));//GenericId INT                                            
                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }
                                }
                                else// without variable dose
                                {
                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i.Equals(0))
                                    {
                                        if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                        {
                                            sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            sStartDate = common.myStr(dt.Rows[i]["StartDate"]);
                                        }
                                    }
                                    if (i.Equals((dt.Rows.Count - 1)))
                                    {
                                        if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                        {
                                            if (!common.myLen(sDuration).Equals(0))
                                            {
                                                sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                sEndDate = sStartDate;
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    string sInstructions = common.myStr(dt.Rows[i]["Instructions"]);
                                    int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                    int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                    int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);
                                    string Volume = common.myStr(dt.Rows[i]["Volume"]);
                                    int VolumeUnitId = common.myInt(dt.Rows[i]["VolumeUnitId"]);
                                    string InfusionTime = common.myStr(dt.Rows[i]["InfusionTime"]);
                                    string TimeUnit = common.myStr(dt.Rows[i]["TimeUnit"]);
                                    string TotalVolume = common.myStr(dt.Rows[i]["TotalVolume"]);
                                    string FlowRate = common.myStr(dt.Rows[i]["FlowRate"]);
                                    string FlowRateUnit = common.myStr(dt.Rows[i]["FlowRateUnit"]);
                                    bool bitIsSubstituteNotAllow = common.myBool(dt.Rows[i]["IsSubstituteNotAllowed"]);
                                    string strICDCode = common.myStr(dt.Rows[i]["ICDCode"]);
                                    int intGenericId = common.myInt(dt.Rows[i]["GenericId"]);

                                    coll.Add(common.myInt(hdnGDItemId.Value));//ItemId INT, 
                                    coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                    coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                    coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                    coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                    coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                    coll.Add(common.myInt(iUnit));//UNITID INT,
                                    coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                    coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                    coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
                                    coll.Add(Volume);//
                                    coll.Add(VolumeUnitId);//
                                    coll.Add(InfusionTime);//
                                    coll.Add(TimeUnit);//
                                    coll.Add(TotalVolume);//
                                    coll.Add(FlowRate);//
                                    coll.Add(FlowRateUnit);//
                                    coll.Add(string.Empty);//                                    
                                    coll.Add(bitIsSubstituteNotAllow);
                                    coll.Add(strICDCode);
                                    coll.Add(i);//SequenceNo tinyint,
                                    coll.Add(common.myInt(hdnGDGenericId.Value));//GenericId INT

                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }
                            coll1.Add(!(sStartDate.Equals(string.Empty)) ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(!(sEndDate.Equals(string.Empty)) ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");
                            coll1.Add(hdnCommentsDrugAllergy.Value); //OverrideComments VARCHAR(500),
                            coll1.Add(hdnCommentsDrugToDrug.Value); //OverrideCommentsDrugToDrug VARCHAR(500),
                            coll1.Add(hdnCommentsDrugHealth.Value); //OverrideCommentsDrugHealth VARCHAR(500)
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR(1000)
                            coll1.Add(hdnStrengthValue.Value); //StrengthValue varchar(255)
                            strXML1.Append(common.setXmlTable(ref coll1));
                            if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                            {
                                collUnAppPrescIds = new ArrayList();
                                collUnAppPrescIds.Add(common.myInt(hdnUnAppPrescriptionId.Value));
                                strXMLUnAppPrescIds.Append(common.setXmlTable(ref collUnAppPrescIds));
                            }
                        }
                        else
                        {
                            return;
                        }

                        //if (common.myBool(ViewState["ConversioinFactor"]))
                        //{
                        //    ds = objWM.GetItemConversionFactor(common.myInt(hdnGDItemId.Value));
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        iConversionFactor = common.myDbl(ds.Tables[0].Rows[0]["ConversionFactor2"]);
                        //    }
                        //    if (!iConversionFactor.Equals(0))
                        //    {
                        //        for (int i = 1; i <= (iConversionFactor * common.myDbl(txtTotalQty.Text)); i++)
                        //        {
                        //            if (common.myDbl(txtTotalQty.Text) <= (iConversionFactor * i))
                        //            {
                        //                sQuantity = iConversionFactor * i;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        //if (common.myBool(ViewState["ConversioinFactor"]) && !iConversionFactor.Equals(0))
                        //{
                        //    sQuantity = common.myDbl(sQuantity.ToString("F2"));
                        //}
                        //else
                        //{
                        //    sQuantity = common.myDbl(txtTotalQty.Text);
                        //}
                    }
                }
                #endregion
            }
            if (strXML.ToString().Equals(string.Empty))
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO");
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : string.Empty;

            hshOutput = new Hashtable();
            bool ApprovalRequired = chkApprovalRequired.Checked;

            //Akshay_Tirathram 
            DataView dv1 = new DataView(ItemExpensiveDetails);
            dv = new DataView(ItemExpensiveDetails);
            dv.RowFilter = "IsExpensive = " + true;
            if (dv.ToTable().Rows.Count > 0)
            {
                dvExpensiveDrugs.Visible = true;

                gvExpensiveDrugs.DataSource = dv.ToTable();
                gvExpensiveDrugs.DataBind();
            }
            dv1.RowFilter = "MaxNarcoticDrugsQty < OrderedQty";

            if (dv1.ToTable().Rows.Count > 0)
            {
                dvNarcoticDrugsPopup.Visible = true;

                gvNarcoticDrugs.DataSource = dv1.ToTable();
                gvNarcoticDrugs.DataBind();
                return;
            }

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                //hshOutput = objWM.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
                //    common.myInt(ddlAdvisingDoctor.SelectedValue), strXML1.ToString(), strXML.ToString(), string.Empty,
                //    Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0,
                //    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                //    isConsumable, sXMLFre, strXMLUnAppPrescIds.ToString(), 0, false, ApprovalRequired,
                //    chkIsReadBack.Checked, txtIsReadBackNote.Text, false, "", "", 0, "", 0, common.myInt(ddlReason.SelectedValue));

                hshOutput = objWM.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
                            common.myInt(ddlAdvisingDoctor.SelectedValue), strXML1.ToString(), strXML.ToString(), string.Empty,
                            Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0,
                            common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                            isConsumable, sXMLFre, strXMLUnAppPrescIds.ToString(),
                            ((ChkReqestFromOtherWard.Checked) ? common.myInt(ddlReqestFromOtherWard.SelectedValue) : 0),
                            false, ApprovalRequired,
                            chkIsReadBack.Checked, txtIsReadBackNote.Text, false, "", "", 0, "", 0, common.myInt(ddlReason.SelectedValue));
            }
            else
            {
                hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
                                    common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ddlAdvisingDoctor.SelectedValue),
                                    0, 0, strXML1.ToString(), strXML.ToString(), string.Empty,
                                   common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                                    sXMLFre, isConsumable, strXMLUnAppPrescIds.ToString());
            }
            if ((common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("UPDATE") || common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("SAVED"))
                && !common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("USP"))
            {
                #region Tagging Static Template with Template Field
                if (common.myStr(Request.QueryString["POPUP"]).ToUpper().Equals("STATICTEMPLATE") && common.myInt(Request.QueryString["StaticTemplateId"]) > 0)
                {
                    Hashtable htOut = new Hashtable();

                    htOut = objEMR.TaggingStaticTemplateWithTemplateField(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                       common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                                       common.myInt(Request.QueryString["StaticTemplateId"]), common.myInt(Session["UserId"]));
                }
                #endregion
                string strMsg = common.myStr(hshOutput["@chvErrorStatus"]);
                hdnReturnIndentDetailsIds.Value = string.Empty;
                #region ePrescription
                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    string OutRefno = "0";
                    int indentid = common.myInt(common.myStr(hshOutput["@chvPrescriptionNo"]).Split('$')[1]);

                    string active = common.myStr(hdnEpresActive.Value);
                    string s = "Operation is successful";
                    if (common.myBool(ViewState["IsInsuranceCompany"]))
                    {
                        if (common.myInt(active).Equals(1))
                        {
                            if (common.myBool(ViewState["IsEPrescriptionEnabled"]))
                            {
                                s = PostePrescription(common.myInt(common.myStr(hshOutput["@chvPrescriptionNo"]).Split('$')[1]), "PRODUCTION", out OutRefno);
                                Alert.ShowAjaxMsg(s, this);
                            }
                            else
                            {
                                objEMR.UpdateDHARefNo(common.myStr(OutRefno), common.myInt(indentid));
                            }
                        }
                        else
                        {
                            objEMR.UpdateDHARefNo(common.myStr(OutRefno), common.myInt(indentid));
                        }
                        if (s.Contains("Operation is successful"))
                        {

                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            try
                            {
                                strMsg = "Prescription No : " +
                                    ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(hshOutput["@chvPrescriptionNo"])).Split('$')[0] +
                                    " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;

                                // Akshay
                                gvItem.DataSource = null;
                                gvItem.DataBind();
                            }
                            catch
                            {
                            }
                            objEMR.UpdateDHARefNo(common.myStr(OutRefno), common.myInt(indentid));
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = s;
                        }
                    }
                    else
                    {
                        objEMR.UpdateDHARefNo(common.myStr(OutRefno), common.myInt(indentid));
                    }
                    if (s.Contains("Operation is successful"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        try
                        {
                            strMsg = "Prescription No : " +
                                ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(hshOutput["@chvPrescriptionNo"])).Split('$')[0] +
                                " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;

                            // Akshay
                            gvItem.DataSource = null;
                            gvItem.DataBind();
                        }
                        catch
                        {
                        }
                        objEMR.UpdateDHARefNo(common.myStr(OutRefno), common.myInt(indentid));

                        lblMessage.Text = s;

                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = s;
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strMsg;
                }
                #endregion
                if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                {

                    //////string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

                    //////ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                    //////return;
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                    //return;
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                txtCustomMedication.Text = string.Empty;
                hdnGenericId.Value = "0";
                hdnGenericName.Value = string.Empty;
                hdnItemId.Value = "0";
                hdnItemName.Value = string.Empty;
                ddlIndentType.SelectedIndex = 0;
                hdnCIMSItemId.Value = string.Empty;
                hdnCIMSType.Value = string.Empty;
                hdnVIDALItemId.Value = "0";
                txtTotQty.Text = string.Empty;
                ddlFormulation.SelectedIndex = 0;
                ddlRoute.SelectedIndex = 0;
                ddlStrengthValue.ClearSelection();
                ddlStrengthValue.Text = string.Empty;
                txtDose.Text = string.Empty;
                ddlFrequencyId.SelectedIndex = 0;
                txtDuration.Text = string.Empty;
                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                txtStartDate.SelectedDate = null;
                txtEndDate.SelectedDate = DateTime.Now;
                txtInstructions.Text = string.Empty;
                clearItemDetails();
                dvPharmacistInstruction.Visible = false;
                lnkPharmacistInstruction.Visible = false;
                Session["CurentSavedPrescriptionId"] = string.Empty;

                if (common.myInt(hshOutput["@intPrescriptionId"]) > 0)
                {
                    Session["CurentSavedPrescriptionId"] = common.myInt(hshOutput["@intPrescriptionId"]).ToString();
                    //dvConfirmPrint.Visible = true;
                }
                hdnCopyLastPresc.Value = "0";
                ViewState["ItemDetail"] = null;
                hdnMainUnAppPrescriptionId.Value = null;
                btnBrandDetailsViewOnItemBased.Visible = false;
                btnMonographViewOnItemBased.Visible = false;
            }
            lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);
            if (!common.myStr(lblMessage.Text).ToUpper().Contains("ALREADY PENDING"))
            {
                BindGrid(string.Empty, string.Empty, string.Empty);
                //Comment By Himanshu On Date 02/03/2022
                //BindBlankItemGrid();
                ViewState["Item"] = null;
                ViewState["Stop"] = null;
                ViewState["Edit"] = null;
            }
            btnCopyLastPrescription.Enabled = true;

            if (txtStartDate.SelectedDate == null)
            {
                if (spnStartdate.Visible)
                {
                    txtStartDate.SelectedDate = DateTime.Now.Date;
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
            objEMR = null;
            objWM = null;
            dsXml.Dispose();
            ds.Dispose();
            dsXmlFrequency.Dispose();
            dsVariableDose.Dispose();
            dt.Dispose();
            dv.Dispose();
            hshOutput = null;


        }
    }
    protected void ddlGeneric_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            Telerik.Web.UI.RadComboBox ddl = sender as Telerik.Web.UI.RadComboBox;
            dt = GetGenericData(common.myStr(e.Text));
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, dt.Rows.Count);
            e.EndOfItems = endOffset.Equals(dt.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = common.myStr(dt.Rows[i]["GenericName"]);
                item.Value = common.myStr(dt.Rows[i]["GenericId"]);
                item.Attributes.Add("CIMSItemId", common.myStr(dt.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(dt.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myInt(dt.Rows[i]["VIDALItemId"]).ToString());
                ddl.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
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
    private DataTable GetGenericData(string text)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objPhr.GetGenericDetails(0, common.myStr(text), 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPhr = null;
        }
        return ds.Tables[0];
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    protected void ddlFormulation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlFormulation.SelectedValue) > 0)
            {
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ddlFormulation.SelectedItem.Attributes["DefaultRouteId"]).ToString()));
                //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(ddlFormulation.SelectedItem.Attributes["DefaultRouteId"]).ToString()));
            }
            else
            {
                ddlRoute.Text = string.Empty;
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
                //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue("0"));
            }
        }
        catch
        {
        }
    }
    protected void ddlBrand_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            Telerik.Web.UI.RadComboBox ddl = sender as Telerik.Web.UI.RadComboBox;
            //if (e.Text != string.Empty && e.Text.Length > 1)
            //{
            int GenericId = 0;
            if (common.myStr(ddlGeneric.Text).Length > 0)
            {
                string selectedValue = common.myStr(e.Context["GenericId"]);
                if (common.myInt(selectedValue) > 0)
                {
                    GenericId = common.myInt(selectedValue);
                }
            }
            if (common.myInt(hdnStoreId.Value).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Store not selected !";
                return;
            }
            data = GetBrandData(common.myStr(e.Text), GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 100, data.Rows.Count);
            e.EndOfItems = endOffset.Equals(data.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);
                //  item.Attributes.Add("ItemSubCategoryShortName", data.Rows[i]["ItemSubCategoryShortName"].ToString());
                item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));
                item.Attributes.Add("CIMSItemId", common.myStr(data.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(data.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myInt(data.Rows[i]["VIDALItemId"]).ToString());
                item.Attributes.Add("ProfitPercentage", common.myStr(data.Rows[i]["ProfitPercentage"]));
                item.Attributes.Add("ItemFlagName", common.myStr(data.Rows[i]["ItemFlagName"]));
                item.Attributes.Add("ItemFlagCode", common.myStr(data.Rows[i]["ItemFlagCode"]));
                this.ddlBrand.Items.Add(item);
                item.DataBind();
            }
            if (common.myStr(ViewState["ShowStockQtyInPrescription"]).Equals("Y"))
            {
                for (int j = itemOffset; j < endOffset; j++)
                {
                    Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
                    lblClosingBalanceItem.Visible = true;
                }
            }
            else
            {
                for (int j = itemOffset; j < endOffset; j++)
                {
                    Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
                    lblClosingBalanceItem.Visible = false;

                }
            }
            if (common.myStr(ViewState["ISShowProfitPercentageInPrescription"]).Equals("Y"))
            {
                for (int j = itemOffset; j < endOffset; j++)
                {

                    Label lblProfitPercentage = (Label)ddlBrand.Items[j].FindControl("lblProfitPercentage");
                    lblProfitPercentage.Visible = true;
                }
            }
            else
            {
                for (int j = itemOffset; j < endOffset; j++)
                {

                    Label lblProfitPercentage = (Label)ddlBrand.Items[j].FindControl("lblProfitPercentage");
                    lblProfitPercentage.Visible = false;
                }
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            // }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            data.Dispose();
        }
    }
    private DataTable GetBrandData(string text, int GenericId)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            int StoreId = common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
            int ItemId = 0;
            int itemBrandId = 0;
            int WithStockOnly = 0;
            int iOT = 3;
            int CompanyId = 0;
            if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Equals("OT")
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
            {
                iOT = 2;
            }
            else if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
            {
                iOT = 2;
            }
            else
            {
                iOT = 1;
            }
            if (common.myDbl(ViewState["QtyBal"]) > 0
                   && common.myInt(Request.QueryString["ItemId"]) > 0)
            {
                ItemId = common.myInt(hdnItemId.Value);
            }

            string IsPackagePatient = common.myStr(Session["IsPackagePatient"]);
            string IsPanelPatient = common.myStr(Session["IsPanelPatient"]);

            //if (common.myBool(Session["IsLoginDoctor"]))
            //{
            //    if (common.myInt(Session["DoctorID"]).Equals(common.myInt(Session["EmployeeId"])))
            //    {
            //        IsPackagePatient = "0";
            //        IsPanelPatient = "0";
            //    }
            //}

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                if (common.myStr(ViewState["AllowZeroStockItemInIPPrescription"]).Equals("Y"))
                {
                    WithStockOnly = 0;
                }
                else
                {
                    WithStockOnly = 1;
                }
            }
            else
            {
                if (common.myStr(ViewState["AllowZeroStockItemInOPPrescription"]).Equals("Y"))
                {
                    WithStockOnly = 0;
                }
                else
                {
                    WithStockOnly = 1;
                }
            }

            if (!common.myInt(Request.QueryString["CompanyId"]).Equals(0))
            {
                CompanyId = common.myInt(Request.QueryString["CompanyId"]);
            }
            else
            {
                CompanyId = common.myInt(Session["SponsorIdPayorId"]);
            }


            dsSearch = objPhr.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, itemBrandId, GenericId,
                                                common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
                                                text.Replace("'", "''"), WithStockOnly, string.Empty, iOT, (common.myStr(Session["OPIP"]).Equals("I")) ? "IP" : string.Empty,
                                                common.myBool(IsPackagePatient), common.myBool(IsPanelPatient),
                                                common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), "I", CompanyId, 0);

            //dsSearch = objPhr.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, itemBrandId, GenericId,
            //                                                    common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
            //                                                    text.Replace("'", "''"), WithStockOnly, string.Empty, iOT, "P",
            //                                                    common.myBool(IsPackagePatient), common.myBool(IsPanelPatient));

            if (dsSearch.Tables.Count > 0)
            {
                if (dsSearch.Tables[0].Rows.Count > 0)
                {
                    dt = dsSearch.Tables[0];
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
            objPhr = null;
        }
        return dt;
    }
    protected void ddlFrequency_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFrequencyId.SelectedIndex > 0)
            {
                //if (common.myInt(ddlFrequencyId.SelectedItem.Attributes["Frequency"]) > 0)
                //{
                //    txtDuration.Text = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                //}
                //else
                //{
                //    if (common.myStr(txtDuration.Text) == "")
                //    {
                //        txtDuration.Text = "";
                //    }
                //}
                try
                {
                    if (common.myStr(ddlFrequencyId.SelectedItem.Text).StartsWith("STAT"))
                    {
                        txtDuration.Text = "1";
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                        //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
                    }
                }
                catch
                { }
            }
            txtDuration.Focus();
        }
        catch
        {
        }
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!chkCustomMedication.Checked)
            {
                ddlGeneric.SelectedIndex = 0;
                ddlGeneric.Text = string.Empty;

                ddlBrand.SelectedIndex = 0;
                ddlBrand.Text = string.Empty;

                hdnItemId.Value = "0";
                //ddlBrand.Text = string.Empty;
                //ddlBrand.SelectedValue = null;
                hdnGenericId.Value = "0";
                //ddlGeneric.Text = string.Empty;
                //ddlGeneric.SelectedValue = null;
                txtCustomMedication.Text = string.Empty;
            }

            hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();

        }
        catch
        {
        }
    }
    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient objPt = new BaseC.Patient(sConString);
        string firstCurrentDate = string.Empty;
        string newCurrentDate = string.Empty;
        try
        {
            string currentdate = objPt.FormatDateDateMonthYear(outputCurrentDate);
            string strformatCurrDate = objPt.FormatDate(currentdate, "MM/dd/yyyy", "yyyy/MM/dd");
            firstCurrentDate = strformatCurrDate.Remove(4, 1);
            newCurrentDate = firstCurrentDate.Remove(6, 1);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPt = null;
        }
        return newCurrentDate;
    }
    protected void btnCalc_OnClick(object sender, EventArgs e)
    {
        endDateChange();
    }
    private void endDateChange()
    {
        try
        {
            calcTotalQty();
            DateTime dt = common.myDate(txtStartDate.SelectedDate);
            int days = common.myInt(txtDuration.Text);
            switch (common.myStr(ddlPeriodType.SelectedValue))
            {
                case "D":
                    days = days * 1;
                    break;
                case "W":
                    days = days * 7;
                    break;
                case "M":
                    days = days * 30;
                    break;
                case "Y":
                    days = days * 365;
                    break;
                default:
                    days = days * 1;
                    break;
            }
            if (days > 0)
            {
                txtEndDate.SelectedDate = dt.AddDays(days - 1);
            }
            else
            {
                txtEndDate.SelectedDate = dt;
            }
        }
        catch
        {
        }
    }
    private DateTime endDateCalculate(DateTime dtStartDate, int Duration, string PeriodType)
    {
        DateTime dt = common.myDate(dtStartDate);
        DateTime dtEndDate = dt;
        try
        {
            int days = common.myInt(Duration);
            switch (common.myStr(PeriodType))
            {
                case "D":
                    days = days * 1;
                    break;
                case "W":
                    days = days * 7;
                    break;
                case "M":
                    days = days * 30;
                    break;
                case "Y":
                    days = days * 365;
                    break;
                default:
                    days = days * 1;
                    break;
            }
            if (days > 1)
            {
                dtEndDate = dt.AddDays(days - 1);
            }
        }
        catch
        {
        }
        return dtEndDate;
    }
    private void calcTotalQty()
    {
        try
        {
            //if (common.myInt(txtDose.Text) == 0)
            //{
            //    //if (ddlUnits.SelectedIndex != -1)
            //    //{
            //    //    txtDose.Focus();
            //    //}
            //    return;
            //    //txtDose.Text = "1";
            //}
            if (common.myDbl(txtDuration.Text).Equals(0))
            {
                //txtDuration.Text = "1";
            }
            //double dose = common.myDbl(txtDose.Text);
            ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ddlFrequencyId.SelectedValue).ToString()));
            //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(ddlFrequencyId.SelectedValue).ToString()));
            double frequency = common.myInt(ddlFrequencyId.SelectedValue).Equals(0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            double days = common.myDbl(txtDuration.Text);
            double totalQty = 0;
            string unitname = string.Empty;
            unitname = (common.myStr(ddlUnit.Text).Trim().Equals(string.Empty)) ? "(Advised Unit) " : " (" + common.myStr(ddlUnit.Text) + ") ";

            double dose = 1;

            switch (common.myStr(ddlPeriodType.SelectedValue))
            {
                case "H":
                    days = days * 1;
                    break;
                case "D":
                    days = days * 1;
                    break;
                case "W":
                    days = days * 7;
                    break;
                case "M":
                    days = days * 30;
                    break;
                case "Y":
                    days = days * 365;
                    break;
                default:
                    days = days * 1;
                    break;
            }

            if (common.myBool(ViewState["ISCalculationRequired"]) && common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]))
            {
                if (!common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT") && common.myInt(frequency).Equals(0))
                {
                    if (common.myInt(ddlUnit.SelectedValue) != 0)
                    {
                        bool IsUnitCalculationRequired = common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]);
                        if (IsUnitCalculationRequired)
                        {
                            dose = common.myDbl(txtDose.Text);
                        }
                    }
                    totalQty = 1 * days * dose;
                    txtTotQty.Text = totalQty.ToString("F2");
                }
                else if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT") && common.myInt(frequency).Equals(0))
                {
                    if (common.myInt(ddlUnit.SelectedValue) > 0)
                    {
                        bool IsUnitCalculationRequired = common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]);
                        if (IsUnitCalculationRequired)
                        {
                            dose = common.myDbl(txtDose.Text);
                        }
                    }
                    totalQty = 1 * days * dose;
                    txtTotQty.Text = totalQty.ToString("F2");
                }
                else
                {
                    if (common.myInt(ddlUnit.SelectedValue) > 0)
                    {
                        bool IsUnitCalculationRequired = common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]);
                        if (IsUnitCalculationRequired)
                        {
                            dose = common.myDbl(txtDose.Text);
                        }
                    }
                    totalQty = frequency * days * dose;
                    txtTotQty.Text = totalQty.ToString("F2");
                }
            }
            else
            {
                txtTotQty.Text = "1";
            }
            txtSpecialInstrucation.Attributes.Add("Style", "Width:100%");
            txtSpecialInstrucation.Text = dose.ToString() + " " + unitname + common.myStr(ddlFrequencyId.Text) + " For " + common.myStr(txtDuration.Text) + " " + common.myStr(ddlPeriodType.Text);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvItem_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
                && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") &&
                (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
            {
                e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
            }
            else
            {
                e.Row.Cells[(byte)enumColumns.TotalQty].Visible = false;
            }
            e.Row.Cells[(byte)enumColumns.Sno].Visible = false;
            e.Row.Cells[(byte)enumColumns.IndentType].Visible = false;
            //e.Row.Cells[(byte)enumColumns.PrescriptionDetail].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            clsCIMS objCIMS = new clsCIMS();
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            DataTable dt = new DataTable();
            DataTable dtData = new DataTable();
            DataTable dtData1 = new DataTable();
            try
            {
                if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
                    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") &&
                    (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
                {
                    e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
                }
                else
                {
                    e.Row.Cells[(byte)enumColumns.TotalQty].Visible = false;
                }
                e.Row.Cells[(byte)enumColumns.Sno].Visible = false;
                e.Row.Cells[(byte)enumColumns.IndentType].Visible = false;
                //e.Row.Cells[(byte)enumColumns.PrescriptionDetail].Visible = false;
                TextBox txtTotalQty = (TextBox)e.Row.FindControl("txtTotalQty");
                txtTotalQty.Text = common.myDbl(txtTotalQty.Text).ToString("F2");
                gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = false;

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = true;
                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDIInteractionCIMS");
                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                    if (common.myLen(hdnCIMSItemId.Value).Equals(0)
                        || common.myStr(hdnCIMSItemId.Value).Trim().Equals("0"))
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                        lnkBtnDAInteractionCIMS.Visible = false;
                        lnkBtnDIInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");
                        string strXML = string.Empty;
                        if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        {
                            strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));
                            if (!strXML.Equals(string.Empty))
                            {
                                string outputValues = objCIMS.getFastTrack5Output(strXML);
                                if (outputValues != null)
                                {
                                    string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" name=";
                                    if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                                    {
                                        lnkBtnBrandDetailsCIMS.Visible = true;
                                    }
                                }
                            }
                        }
                        lnkBtnMonographCIMS.Visible = false;
                        strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));
                        if (!strXML.Equals(string.Empty))
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);
                            if (outputValues != null)
                            {
                                if (outputValues.ToUpper().Contains("<MONOGRAPH>"))
                                {
                                    lnkBtnMonographCIMS.Visible = true;
                                }
                            }
                        }
                    }
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = true;
                    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsVIDAL");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");
                    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionVIDAL");
                    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnBrandDetailsVIDAL.Visible = true;
                    lnkBtnMonographVIDAL.Visible = true;
                    lnkBtnInteractionVIDAL.Visible = true;
                    lnkBtnDHInteractionVIDAL.Visible = true;
                    //lnkBtnDAInteractionVIDAL.Visible = true;
                    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    {
                        lnkBtnBrandDetailsVIDAL.Visible = false;
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                        lnkBtnDAInteractionVIDAL.Visible = false;
                    }
                }
                if (!chkShowDetails.Checked)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;
                        gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                        gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
                        gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
                        gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;
                        gvItem.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = false;

                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
                        gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                        gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                        gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
                        gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;
                    }
                }
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnGDItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
                HiddenField hdnXMLData = (HiddenField)e.Row.FindControl("hdnXMLData");
                Label lblItemName = (Label)e.Row.FindControl("lblItemName");
                HiddenField hdnGDGenericId = (HiddenField)e.Row.FindControl("hdnGenericId");
                HiddenField hdnGenericName = (HiddenField)e.Row.FindControl("hdnGenericName");


                #region Qty available in stock
                HiddenField hdnClosingBalanceQty = (HiddenField)e.Row.FindControl("hdnClosingBalanceQty");
                if (common.myInt(txtTotalQty.Text) <= common.myInt(hdnClosingBalanceQty.Value) && !common.myStr(lblItemName.Text).Equals(string.Empty))
                {
                    e.Row.Cells[(byte)enumColumns.BrandName].BackColor = System.Drawing.Color.LightGreen;
                }
                #endregion

                #region LASA / Non-LASA Item
                HiddenField hdnItemFlagCode = (HiddenField)e.Row.FindControl("hdnItemFlagCode");
                if (common.myStr(hdnItemFlagCode.Value).Equals("LASA"))
                {
                    e.Row.Cells[(byte)enumColumns.BrandName].BackColor = System.Drawing.Color.LightGray;
                }
                else if (common.myStr(hdnItemFlagCode.Value).Equals("SC") || common.myStr(hdnItemFlagCode.Value).Equals("HighRisk") || common.myStr(hdnItemFlagCode.Value).Equals("HighValue"))
                {
                    e.Row.Cells[(byte)enumColumns.BrandName].BackColor = System.Drawing.Color.Yellow;
                }

                //Add By Himanshu On Date 17 / 02 / 2022 Given By Varsha Parshad 
                else if (common.myStr(hdnItemFlagCode.Value).Equals("NARCOTIC"))
                {
                    e.Row.Cells[(byte)enumColumns.BrandName].BackColor = System.Drawing.Color.DarkGreen;
                    btnAddItem.Enabled = false;
                }
                //end
                #endregion


                if (common.myStr(hdnGenericName.Value).Trim().Length > 0)
                {
                    //lblItemName.Text = common.myStr(hdnGenericName.Value) + ((common.myLen(lblItemName.Text) > 0) ? " - " + lblItemName.Text : lblItemName.Text);
                    lblItemName.Text = (common.myLen(lblItemName.Text) > 0) ? common.myStr(lblItemName.Text).Trim() : common.myStr(hdnGenericName.Value).Trim();
                }
                if (ViewState["ItemDetail"] != null)
                {
                    dt = (DataTable)ViewState["ItemDetail"];
                    try
                    {
                        if (dt != null)
                        {
                            if (dt.Columns.Count > 0)
                            {
                                if (!dt.Columns.Contains("FrequencyId"))
                                {
                                    dt.Columns.Add("FrequencyId", typeof(int));
                                }
                                if (!dt.Columns.Contains("Dose"))
                                {
                                    dt.Columns.Add("Dose", typeof(double));
                                }
                                if (!dt.Columns.Contains("Duration"))
                                {
                                    dt.Columns.Add("Duration", typeof(string));
                                }
                                if (!dt.Columns.Contains("Type"))
                                {
                                    dt.Columns.Add("Type", typeof(string));
                                }
                                if (!dt.Columns.Contains("Instructions"))
                                {
                                    dt.Columns.Add("Instructions", typeof(string));
                                }
                                if (!dt.Columns.Contains("ReferanceItemId"))
                                {
                                    dt.Columns.Add("ReferanceItemId", typeof(int));
                                }
                                if (!dt.Columns.Contains("FoodRelationshipID"))
                                {
                                    dt.Columns.Add("FoodRelationshipID", typeof(int));
                                }
                                if (!dt.Columns.Contains("DoseTypeId"))
                                {
                                    dt.Columns.Add("DoseTypeId", typeof(int));
                                }
                                if (!dt.Columns.Contains("UnitId"))
                                {
                                    dt.Columns.Add("UnitId", typeof(int));
                                }
                                if (!dt.Columns.Contains("FormulationId"))
                                {
                                    dt.Columns.Add("FormulationId", typeof(int));
                                }
                                if (!dt.Columns.Contains("FrequencyId"))
                                {
                                    dt.Columns.Add("FrequencyId", typeof(int));
                                }
                                if (!dt.Columns.Contains("RouteId"))
                                {
                                    dt.Columns.Add("RouteId", typeof(int));
                                }
                                if (!dt.Columns.Contains("InfusionTime"))
                                {
                                    dt.Columns.Add("InfusionTime", typeof(string));
                                }
                                if (!dt.Columns.Contains("TotalVolume"))
                                {
                                    dt.Columns.Add("TotalVolume", typeof(string));
                                }
                                if (!dt.Columns.Contains("Volume"))
                                {
                                    dt.Columns.Add("Volume", typeof(string));
                                }
                                if (!dt.Columns.Contains("VolumeUnitId"))
                                {
                                    dt.Columns.Add("VolumeUnitId", typeof(int));
                                }
                                if (!dt.Columns.Contains("TimeUnit"))
                                {
                                    dt.Columns.Add("TimeUnit", typeof(string));
                                }
                                if (!dt.Columns.Contains("FlowRate"))
                                {
                                    dt.Columns.Add("FlowRate", typeof(string));
                                }
                                if (!dt.Columns.Contains("FlowRateUnit"))
                                {
                                    dt.Columns.Add("FlowRateUnit", typeof(int));
                                }
                                if (!dt.Columns.Contains("XMLVariableDose"))
                                {
                                    dt.Columns.Add("XMLVariableDose", typeof(string));
                                }
                                if (!dt.Columns.Contains("IsSubstituteNotAllowed"))
                                {
                                    dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                }
                                if (!dt.Columns.Contains("ICDCode"))
                                {
                                    dt.Columns.Add("ICDCode", typeof(string));
                                }
                                if (!dt.Columns.Contains("XMLFrequencyTime"))
                                {
                                    dt.Columns.Add("XMLFrequencyTime", typeof(string));
                                }
                                if (!dt.Columns.Contains("DurationText"))
                                {
                                    dt.Columns.Add("DurationText", typeof(string));
                                }
                                if (!dt.Columns.Contains("FoodRelationship"))
                                {
                                    dt.Columns.Add("FoodRelationship", typeof(string));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        ViewState["ItemDetail"] = dt;
                    }
                    if (common.myInt(ViewState["OrderSetId"]).Equals(0) || common.myLen(lblPrescriptionDetail.Text).Equals(0))
                    {
                        dv = new DataView(dt);
                        ////if (hdnCustomMedication.Value == "1")
                        ////{
                        ////    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        ////}
                        ////else
                        ////{
                        //dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                        ////}

                        if (common.myInt(hdnGDItemId.Value) > 0)
                        {
                            dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                        }
                        else
                        {
                            if (common.myInt(hdnGDGenericId.Value) > 0)
                            {
                                dv.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGDGenericId.Value);
                            }
                            else
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                            }
                        }

                        HiddenField hdnUnAppPrescriptionId = (HiddenField)e.Row.FindControl("hdnUnAppPrescriptionId");
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            dtData = dv.ToTable().Copy();
                            foreach (DataRow item in dtData.Rows)
                            {
                                item["XMLData"] = string.Empty;
                            }
                            if (common.myInt(hdnUnAppPrescriptionId.Value).Equals(0) || common.myLen(lblPrescriptionDetail.Text).Equals(0))
                            {
                                lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringNew(dtData, chkFractionalDose.Checked ? "Y" : "N");
                                lblItemName.ToolTip = common.myStr(lblPrescriptionDetail.Text);
                            }


                            dtData.TableName = "ItemDetail";

                            StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                            StringWriter writer = new StringWriter(builder);// Write the schema into the StringWriter.
                            dtData.WriteXml(writer, XmlWriteMode.IgnoreSchema);
                            string xmlSchema = writer.ToString();
                            xmlSchema = xmlSchema.Replace("&lt;", "<");
                            xmlSchema = xmlSchema.Replace("&gt;", ">");
                            hdnXMLData.Value = xmlSchema;
                        }
                        else
                        {
                            dv1 = new DataView(dt);
                            //dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnGDItemId.Value);
                            dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                            if (dv1.ToTable().Rows.Count > 0 && common.myInt(hdnIndentId.Value) > 0)
                            {
                                dtData1 = dv1.ToTable().Copy();
                                foreach (DataRow item in dtData1.Rows)
                                {
                                    item["XMLData"] = string.Empty;
                                }
                                if (common.myInt(hdnUnAppPrescriptionId.Value).Equals(0))
                                {
                                    lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringNew(dtData1, chkFractionalDose.Checked ? "Y" : "N");
                                    lblItemName.ToolTip = common.myStr(lblPrescriptionDetail.Text);
                                }
                                dt.TableName = "ItemDetail";
                                StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                                System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                                dtData1.WriteXml(writer, XmlWriteMode.IgnoreSchema);
                                string xmlSchema = writer.ToString();
                                xmlSchema = xmlSchema.Replace("&lt;", "<");
                                xmlSchema = xmlSchema.Replace("&gt;", ">");
                                hdnXMLData.Value = xmlSchema;
                            }
                            else
                            {
                                //lblPrescriptionDetail.Text = string.Empty;
                            }
                        }
                        dv.RowFilter = string.Empty;
                        dv1.RowFilter = string.Empty;
                    }
                }

                //HiddenField hdnIsInfusion = (HiddenField)e.Row.FindControl("hdnIsInfusion");
                //dtPrevious = (DataTable)ViewState["Item"];
                //if (common.myInt(DR["IsInfusion"]) == 1)
                //{
                //    RadComboBoxItem item = new RadComboBoxItem();
                //    item.Text = common.myStr(DR["ItemName"]);
                //    item.Value = common.myStr(common.myInt(DR["ItemId"]));
                //    item.Attributes.Add("IsInfusion", common.myStr(DR["IsInfusion"]));
                //    item.DataBind();
                //    ddlReferanceItem.Items.Add(item);
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
                objEMR = null;
                objCIMS = null;
                dv.Dispose();
                dv1.Dispose();
                dt.Dispose();
                dtData.Dispose();
                dtData1.Dispose();
            }
        }
    }

    protected void gvOrderSet_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkSetName = (LinkButton)e.Row.FindControl("lnkSetName");
                HiddenField hdnDrugName = (HiddenField)e.Row.FindControl("hdnDrugName");

                if (common.myLen(hdnDrugName.Value) > 0)
                {
                    lnkSetName.ToolTip = common.myStr(hdnDrugName.Value).Replace(",", "," + Environment.NewLine);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnRemoveItem_OnClick(object sender, EventArgs e)
    {
        Removeitem();
    }
    private void Removeitem()
    {
        try
        {
            ddlFrequencyId.Enabled = true;
            ddlReferanceItem.Enabled = true;
            txtDuration.Enabled = true;
            ddlPeriodType.Enabled = true;
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            ddlStrengthValue.Enabled = true;
            ddlBrand.Focus();
            ddlBrand.ClearSelection();
            ddlBrand.Items.Clear();
            ddlBrand.Text = string.Empty;
            ddlBrand.Enabled = true;
            ddlBrand.SelectedValue = null;
            ddlGeneric.Items.Clear();
            ddlGeneric.Text = string.Empty;
            ddlGeneric.Enabled = true;
            ddlGeneric.SelectedValue = null;
            txtInstructions.Text = string.Empty;
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
            //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue("0"));
            ddlFoodRelation.Text = string.Empty;
            chkNotToPharmacy.Checked = false;
            ddlFrequencyId.SelectedIndex = 0;
            txtDose.Text = string.Empty;//"1"
            txtDuration.Text = string.Empty;
            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
            //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
            ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));
            //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue("0"));
            txtInstructions.Text = string.Empty;
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("-1"));
            //ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindByValue("-1"));
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
            //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue("0"));
            ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue("0"));
            //ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindByValue("0"));
            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue("0"));
            //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue("0"));
            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
            //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue("0"));
            txtEndDate.SelectedDate = null;
            txtStartDate.SelectedDate = null;
            txtEndDate.SelectedDate = null;
            ViewState["Edit"] = null;
            txtCustomMedication.Text = string.Empty;
            hdnXmlVariableDoseString.Value = string.Empty;
            hdnvariableDoseDuration.Value = string.Empty;
            hdnvariableDoseFrequency.Value = string.Empty;
            hdnVariabledose.Value = string.Empty;
            Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));

            if (txtStartDate.SelectedDate == null)
            {
                if (spnStartdate.Visible)
                {
                    txtStartDate.SelectedDate = DateTime.Now.Date;
                }
            }
        }
        catch
        { }
    }
    protected void gvItem_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsCalisreq = new DataSet();
        DataSet dsXml = new DataSet();
        DataTable dtDetail = new DataTable();
        DataView DV = new DataView();
        DataView DV1 = new DataView();
        DataTable dt = new DataTable();
        DataView dvFilter = new DataView();
        DataTable tbl = new DataTable();
        DataTable tbl1 = new DataTable();
        DataTable dtData = new DataTable();
        DataTable dtData1 = new DataTable();
        try
        {
            if (e.CommandName.ToUpper().Equals("ITEMDELETE"))
            {
                //int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);
                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);
                HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");
                HiddenField hdnUnAppPrescriptionId = (HiddenField)row.FindControl("hdnUnAppPrescriptionId");

                if (ViewState["ItemDetail"] != null && (ItemId > 0 || GenericId > 0 || common.myBool(hdnCustomMedication.Value)))
                {
                    tbl = (DataTable)ViewState["Item"];
                    tbl1 = (DataTable)ViewState["ItemDetail"];
                    DV = tbl.Copy().DefaultView;
                    DV1 = tbl1.Copy().DefaultView;
                    if (common.myBool(hdnCustomMedication.Value))
                    {
                        Label lblItemName = (Label)row.FindControl("lblItemName");
                        //DV.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                        //DV1.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                        DV.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
                        DV1.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
                    }
                    else
                    {
                        if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                        {
                            DV.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
                            DV1.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
                        }
                        else
                        {
                            if (ItemId > 0)
                            {
                                DV.RowFilter = "ISNULL(ItemId,0) <> " + common.myInt(ItemId);// +" AND IndentId<>" + IndentId;
                                DV1.RowFilter = "ISNULL(ItemId,0) <> " + common.myInt(ItemId);// +" AND IndentId<>" + IndentId;
                            }
                            else
                            {
                                DV.RowFilter = "ISNULL(GenericId, 0) <> " + common.myInt(GenericId);
                                DV1.RowFilter = "ISNULL(GenericId, 0) <> " + common.myInt(GenericId);
                            }
                        }
                    }
                    tbl = DV.ToTable();
                    ViewState["GridDataItem"] = DV.ToTable();
                    ViewState["Item"] = DV.ToTable();
                    ViewState["ItemDetail"] = DV1.ToTable();
                    //Add By Himanshu 17/02/2022
                    btnAddItem.Enabled = true;
                    //end
                    if (tbl.Rows.Count.Equals(0))
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();
                    }
                    dtData = DV.ToTable().Copy();
                    dtData = addColumnInItemGrid(dtData);

                    dtData1 = DV1.ToTable().Copy();
                    dtData1 = addColumnInItemGrid(dtData1);

                    #region delete from EMRUnApprovedPrescriptions

                    if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        string strMsg = objEMR.deleteUnApprovedPrescriptions(common.myInt(hdnUnAppPrescriptionId.Value), common.myInt(ViewState["EncId"]),
                                        common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]));
                    }
                    #endregion

                    ViewState["GridDataItem"] = dtData;
                    ViewState["Item"] = dtData;

                    ViewState["ItemDetail"] = dtData1;

                    ViewState["MaintainCheckValueData"] = ViewState["ItemDetail"];

                    // changes made by ashu for order set deletion issue
                    gvItem.DataSource = dtData;
                    gvItem.DataBind();

                    //if (DV1.ToTable().Rows.Count.Equals(0))
                    if (DV.ToTable().Rows.Count.Equals(0))
                    {
                        ViewState["GridDataItem"] = null;
                        ViewState["Item"] = null;
                        ViewState["ItemDetail"] = null;
                        //BindBlankItemGrid();
                    }
                    ViewState["StopItemDetail"] = null;
                    ViewState["Edit"] = null;
                    setVisiblilityInteraction();
                }
            }
            else if (e.CommandName.ToUpper().Equals("BRANDDETAILSCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("MONOGRAPHCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("INTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }


            else if (e.CommandName.ToUpper().Equals("DIINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showDIIntreraction();
            }
            else if (e.CommandName.ToUpper().Equals("DHINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName.ToUpper().Equals("DAINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("A");
            }
            else if (e.CommandName.ToUpper().Equals("BRANDDETAILSVIDAL"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getBrandDetailsVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName.ToUpper().Equals("MONOGRAPHVIDAL"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName.ToUpper().Equals("INTERACTIONVIDAL"))
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName.ToUpper().Equals("DHINTERACTIONVIDAL"))
            {
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName.ToUpper().Equals("DAINTERACTIONVIDAL"))
            {
                showHealthOrAllergiesIntreraction("A");
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                dsXml = new DataSet();
                dtDetail = new DataTable();
                DV = new DataView();
                dt = new DataTable();
                dvFilter = new DataView();

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");
                HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                HiddenField hdnUnitId = (HiddenField)row.FindControl("hdnUnitId");
                HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                HiddenField hdnDuration = (HiddenField)row.FindControl("hdnDuration");
                HiddenField hdnDurationType = (HiddenField)row.FindControl("hdnDurationType");
                HiddenField hdnFoodRelationshipId = (HiddenField)row.FindControl("hdnFoodRelationshipId");

                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");


                HiddenField hdnDays = (HiddenField)row.FindControl("hdnDays");
                HiddenField hdnStartDate = (HiddenField)row.FindControl("hdnStartDate");
                HiddenField hdnEndDate = (HiddenField)row.FindControl("hdnEndDate");
                HiddenField hdnRemarks = (HiddenField)row.FindControl("hdnRemarks");
                Label lblPrescriptionDetail = (Label)row.FindControl("lblPrescriptionDetail");

                chkCustomMedication.Checked = common.myBool(hdnCustomMedication.Value);
                setCustomMedicationChange();

                int ItemId = common.myInt(e.CommandArgument);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                string GenericName = common.myStr(((HiddenField)row.FindControl("hdnGenericName")).Value);
                txtCommentsDrugAllergy.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugAllergy")).Value);
                txtCommentsDrugToDrug.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugToDrug")).Value);
                txtCommentsDrugHealth.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugHealth")).Value);
                hdnCIMSItemId.Value = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                hdnCIMSType.Value = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);
                hdnVIDALItemId.Value = common.myInt(((HiddenField)row.FindControl("hdnVIDALItemId")).Value).ToString();
                HiddenField hdnStrengthValue = (HiddenField)row.FindControl("hdnStrengthValue");

                Label lblItemName = (Label)row.FindControl("lblItemName");
                HiddenField hdnUnAppPrescriptionId = (HiddenField)row.FindControl("hdnUnAppPrescriptionId");
                hdnMainUnAppPrescriptionId.Value = common.myInt(hdnUnAppPrescriptionId.Value).ToString();

                if (ItemId <= 0 && common.myBool(hdnCustomMedication.Value))
                {
                    txtCustomMedication.Text = common.myStr(lblItemName.Text);
                }
                else
                {
                    try
                    {
                        if (ItemId <= 0)
                        {
                            ddlBrand.Text = string.Empty;
                            ddlBrand.SelectedValue = null;
                        }
                        else
                        {
                            ddlBrand.Text = common.myStr(lblItemName.Text);
                            ddlBrand.SelectedValue = common.myStr(ItemId);
                            CheckgenerateInstruction(ItemId);
                        }

                        ddlBrand.Enabled = false;

                        //btnGetInfo_Click(null, null);

                        ddlGeneric.Text = common.myStr(GenericName);
                        ddlGeneric.SelectedValue = common.myStr(GenericId);
                        ddlGeneric.Enabled = false;
                    }
                    catch
                    {
                    }
                }
                hdnItemId.Value = ItemId.ToString();
                hdnGenericId.Value = GenericId.ToString();
                ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                if (ViewState["ItemDetail"] != null)
                {
                    dt = (DataTable)ViewState["ItemDetail"];
                }
                else
                {
                    return;
                }
                DV = new DataView(dt);
                if (ItemId <= 0 && common.myBool(hdnCustomMedication.Value))
                {
                    // DV.RowFilter = "ISNULL(ItemId,0)=" + ItemId + " AND CustomMedication=1 ";
                    try
                    {
                        DV.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId) + " AND ISNULL(CustomMedication,False)=True AND ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                    }
                    catch
                    {
                        DV.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId) + " AND ISNULL(CustomMedication,0)=1 AND ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                    }
                }
                else
                {
                    if (ItemId <= 0)
                    {
                        DV.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(GenericId);
                    }
                    else
                    {
                        DV.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId);
                    }
                }
                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["StopItemDetail"] = DV.ToTable();
                    HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
                    string xmlSchema = common.myStr(hdnXMLData.Value).Trim();
                    if (xmlSchema != "")
                    {
                        StringReader sr = new StringReader(xmlSchema);
                        dsXml.ReadXml(sr);
                        try
                        {
                            if (dsXml != null)
                            {
                                if (dsXml.Tables[0].Columns.Count > 0)
                                {
                                    if (!dsXml.Tables[0].Columns.Contains("FrequencyId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("Dose"))
                                    {
                                        dsXml.Tables[0].Columns.Add("Dose", typeof(double));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("Duration"))
                                    {
                                        dsXml.Tables[0].Columns.Add("Duration", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("Type"))
                                    {
                                        dsXml.Tables[0].Columns.Add("Type", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("Instructions"))
                                    {
                                        dsXml.Tables[0].Columns.Add("Instructions", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("ReferanceItemId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("ReferanceItemId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FoodRelationshipID"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FoodRelationshipID", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("DoseTypeId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("DoseTypeId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("UnitId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("UnitId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FormulationId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FormulationId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FrequencyId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FrequencyId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("RouteId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("RouteId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("InfusionTime"))
                                    {
                                        dsXml.Tables[0].Columns.Add("InfusionTime", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("TotalVolume"))
                                    {
                                        dsXml.Tables[0].Columns.Add("TotalVolume", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("Volume"))
                                    {
                                        dsXml.Tables[0].Columns.Add("Volume", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("VolumeUnitId"))
                                    {
                                        dsXml.Tables[0].Columns.Add("VolumeUnitId", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("TimeUnit"))
                                    {
                                        dsXml.Tables[0].Columns.Add("TimeUnit", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FlowRate"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FlowRate", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FlowRateUnit"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FlowRateUnit", typeof(int));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("XMLVariableDose"))
                                    {
                                        dsXml.Tables[0].Columns.Add("XMLVariableDose", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("IsSubstituteNotAllowed"))
                                    {
                                        dsXml.Tables[0].Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("ICDCode"))
                                    {
                                        dsXml.Tables[0].Columns.Add("ICDCode", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("XMLFrequencyTime"))
                                    {
                                        dsXml.Tables[0].Columns.Add("XMLFrequencyTime", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("DurationText"))
                                    {
                                        dsXml.Tables[0].Columns.Add("DurationText", typeof(string));
                                    }
                                    if (!dsXml.Tables[0].Columns.Contains("FoodRelationship"))
                                    {
                                        dsXml.Tables[0].Columns.Add("FoodRelationship", typeof(string));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                        }
                        dvFilter = new DataView(dsXml.Tables[0]);

                        if (ItemId <= 0 && common.myBool(hdnCustomMedication.Value))
                        {
                            try
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId) + " AND ISNULL(CustomMedication,False)=True AND ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                            }
                            catch
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId) + " AND ISNULL(CustomMedication,0)=1 AND ItemName='" + common.myStr(lblItemName.Text, true) + "'";
                            }
                        }
                        else
                        {
                            if (ItemId <= 0)
                            {
                                dvFilter.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(GenericId);
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(ItemId);
                            }
                        }

                        dtDetail = dvFilter.ToTable();
                        if (dtDetail.Rows.Count > 0)
                        {
                            ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FrequencyId"]).ToString()));
                            //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(dtDetail.Rows[0]["FrequencyId"]).ToString()));                        
                            if (common.myDbl(dtDetail.Rows[0]["Dose"]) > 0)
                            {
                                txtDose.Text = common.myStr(dtDetail.Rows[0]["Dose"]);
                            }
                            else
                            {
                                txtDose.Text = string.Empty;
                            }
                            if (common.myDbl(dtDetail.Rows[0]["Duration"]) > 0)
                            {
                                txtDuration.Text = common.myStr(dtDetail.Rows[0]["Duration"]);
                            }
                            else
                            {
                                txtDuration.Text = string.Empty;
                            }
                            if (common.myLen(dtDetail.Rows[0]["Type"]).Equals(0))
                            {
                                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                                //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
                            }
                            else
                            {
                                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["Type"])));
                                //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue(common.myStr(dtDetail.Rows[0]["Type"])));
                            }
                            txtInstructions.Text = common.myStr(dtDetail.Rows[0]["Instructions"]);
                            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["ReferanceItemId"])));
                            //ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindByValue(common.myStr(dtDetail.Rows[0]["ReferanceItemId"])));
                            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FoodRelationshipID"]).ToString()));
                            //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue(common.myInt(dtDetail.Rows[0]["FoodRelationshipID"]).ToString()));
                            ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["DoseTypeId"]).ToString()));
                            //ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindByValue(common.myInt(dtDetail.Rows[0]["DoseTypeId"]).ToString()));
                            ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["UnitId"]).ToString()));
                            //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue(common.myInt(dtDetail.Rows[0]["UnitId"]).ToString()));
                            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FormulationId"]).ToString()));
                            //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(dtDetail.Rows[0]["FormulationId"]).ToString()));
                            ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FrequencyId"]).ToString()));
                            //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(dtDetail.Rows[0]["FrequencyId"]).ToString()));
                            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["RouteId"]).ToString()));
                            //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(dtDetail.Rows[0]["RouteId"]).ToString()));
                            if (common.myDbl(dtDetail.Rows[0]["Dose"]) > 0)
                            {
                                txtDose.Text = common.myStr(dtDetail.Rows[0]["Dose"]);
                            }
                            else
                            {
                                txtDose.Text = string.Empty;
                            }
                            txtTimeInfusion.Text = common.myStr(dtDetail.Rows[0]["InfusionTime"]);
                            txtTotalVolumn.Text = common.myStr(dtDetail.Rows[0]["TotalVolume"]);
                            txtVolume.Text = common.myStr(dtDetail.Rows[0]["Volume"]);
                            ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["VolumeUnitId"]).ToString()));
                            //ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindByValue(common.myInt(dtDetail.Rows[0]["VolumeUnitId"]).ToString()));
                            ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["TimeUnit"])));
                            //ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindByValue(common.myStr(dtDetail.Rows[0]["TimeUnit"])));
                            txtFlowRateUnit.Text = common.myStr(dtDetail.Rows[0]["FlowRate"]);
                            ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FlowRateUnit"]).ToString()));
                            //ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindByValue(common.myInt(dtDetail.Rows[0]["FlowRateUnit"]).ToString()));
                            hdnXmlVariableDoseString.Value = common.myStr(dtDetail.Rows[0]["XMLVariableDose"]).Trim();

                            try
                            {
                                if (dsXml.Tables[0].Columns.Contains("XMLFrequencyTime"))
                                {
                                    hdnXmlFrequencyTime.Value = common.myStr(dtDetail.Rows[0]["XMLFrequencyTime"]);
                                }
                                else
                                {
                                    if (DV.ToTable().Rows.Count > 0)
                                    {
                                        if (DV.ToTable().Columns.Contains("XMLFrequencyTime"))
                                        {
                                            hdnXmlFrequencyTime.Value = common.myStr(DV.ToTable().Rows[0]["XMLFrequencyTime"]);
                                        }
                                    }

                                }
                            }
                            catch
                            { }
                            chkSubstituteNotAllow.Checked = common.myBool(dtDetail.Rows[0]["IsSubstituteNotAllowed"]);
                            txtICDCode.Text = common.myStr(dtDetail.Rows[0]["ICDCode"]);
                            //if (spnStartdate.Visible)
                            //{
                            //    txtStartDate.SelectedDate = common.myDate(dtDetail.Rows[0]["StartDate"]);
                            //}
                            //txtEndDate.SelectedDate = endDateChange(txtStartDate, txtDuration.Text, ddlPeriodType.SelectedValue);
                            //txtEndDate.SelectedDate = common.myDate(dtDetail.Rows[0]["EndDate"]);
                        }

                        dvFilter.RowFilter = string.Empty;
                    }
                    else
                    {
                        txtDose.Text = common.myStr(hdnDose.Value);
                        ddlUnit.SelectedValue = common.myStr(hdnUnitId.Value);
                        ddlFrequencyId.SelectedValue = common.myStr(hdnFrequencyId.Value);
                        txtDuration.Text = common.myStr(hdnDuration.Value);
                        ddlDoseType.SelectedValue = common.myStr(hdnDurationType.Value);
                        ddlFoodRelation.SelectedValue = common.myStr(hdnFoodRelationshipId.Value);
                        ddlRoute.SelectedValue = common.myStr(hdnRouteId.Value);
                        ddlFormulation.SelectedValue = common.myStr(hdnFormulationId.Value);
                        ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                        ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                        txtInstructions.Text = common.myStr(hdnRemarks.Value);
                    }
                }
                BindStopItemDetail();
                dsCalisreq = objPhr.ISCalculationRequired(ItemId);
                if (dsCalisreq.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
                }
                ViewState["Edit"] = true;
                endDateChange();
            }
            chkShowDetails_OnCheckedChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPhr = null;
            objEMR = null;
            dsCalisreq.Dispose();
            dsXml.Dispose();
            dtDetail.Dispose();
            DV.Dispose();
            DV1.Dispose();
            dt.Dispose();
            dvFilter.Dispose();
            tbl.Dispose();
            tbl1.Dispose();
            dtData.Dispose();
            dtData1.Dispose();
        }
    }
    //protected void ibtnFavourite_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (common.myInt(hdnItemId.Value) > 0)
    //        {
    //            string sDoctorId = Request.QueryString["DoctorId"] == null ? common.myStr(Session["LoginDoctorId"]) : common.myStr(Request.QueryString["DoctorId"]);
    //            RadWindow1.NavigateUrl = "~/EMR/Medication/DrugFavourite.aspx?DoctorId=" + sDoctorId + "&ItemId=" + common.myInt(hdnItemId.Value)
    //                + "&ItemName=" + common.myStr(ddlBrand.Text) + "&GenericId=" + common.myInt(hdnGenericId.Value);
    //            RadWindow1.Height = 500;
    //            RadWindow1.Width = 500;
    //            RadWindow1.Top = 200;
    //            RadWindow1.Left = 500;
    //            RadWindow1.OnClientClose = "OnClientCloseFavourite";
    //            RadWindow1.VisibleOnPageLoad = true;
    //            RadWindow1.Modal = true;
    //            //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    //            RadWindow1.VisibleStatusbar = false;
    //        }
    //        else
    //        {
    //            Alert.ShowAjaxMsg("Please select drug to add favourite", Page);
    //            return;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    private void BindStopItemDetail()
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {
            dt = CreateItemTable();
            DataRow dr;
            int count = 0;
            if (ViewState["StopItemDetail"] != null)
            {
                dt1 = (DataTable)ViewState["StopItemDetail"];
                if (dt1 != null && ViewState["Stop"] == null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["Id"] = count + 1;
                        dr["IndentTypeId"] = 0;
                        dr["GenericId"] = common.myInt(dt1.Rows[i]["GenericId"]);
                        dr["ItemId"] = common.myInt(dt1.Rows[i]["ItemId"]);
                        dr["IndentId"] = common.myInt(dt1.Rows[i]["IndentId"]);
                        dr["GenericName"] = common.myStr(dt1.Rows[i]["GenericName"]);
                        dr["ItemName"] = common.myStr(dt1.Rows[i]["ItemName"]);
                        dr["FormulationId"] = common.myInt(dt1.Rows[i]["FormulationId"]);
                        dr["RouteId"] = common.myInt(dt1.Rows[i]["RouteId"]);
                        dr["RouteName"] = common.myInt(dt1.Rows[i]["RouteName"]);
                        dr["FrequencyId"] = common.myInt(dt1.Rows[i]["FrequencyId"]);
                        dr["FrequencyName"] = string.Empty;
                        dr["Duration"] = common.myStr(dt1.Rows[i]["Duration"]);
                        dr["Type"] = common.myStr(dt1.Rows[i]["Type"]);
                        dr["DurationText"] = common.myStr(dt1.Rows[i]["DurationText"]);
                        dr["Dose"] = common.myDbl(dt1.Rows[i]["Dose"]);
                        dr["UnitId"] = common.myInt(dt1.Rows[i]["UnitId"]);
                        dr["UnitName"] = string.Empty;
                        dr["FoodRelationshipId"] = 0;
                        dr["FoodRelationship"] = string.Empty;
                        dr["StartDate"] = count.Equals(0) ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(1);
                        dr["EndDate"] = count.Equals(0) ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[i]["Duration"]) - 1) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(common.myInt(dt1.Rows[i]["Duration"]));
                        //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                        //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
                        //dr["Qty"] = "";
                        dr["PrescriptionDetail"] = string.Empty;
                        dr["ReferanceItemId"] = common.myInt(dt1.Rows[i]["ReferanceItemId"]);
                        dr["ReferanceItemName"] = common.myStr(dt1.Rows[i]["ReferanceItemName"]);
                        dr["Instructions"] = common.myStr(dt1.Rows[i]["Instructions"]);
                        //dr["XMLData"] = "";
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        count++;
                    }
                }
                else if (common.myBool(ViewState["Stop"]))
                {
                    dr = dt.NewRow();
                    dr["Id"] = count + 1;
                    dr["IndentTypeId"] = 0;
                    dr["GenericId"] = common.myInt(dt1.Rows[0]["GenericId"]);
                    dr["ItemId"] = common.myInt(dt1.Rows[0]["ItemId"]);
                    dr["IndentId"] = common.myInt(dt1.Rows[0]["IndentId"]);
                    dr["GenericName"] = common.myStr(dt1.Rows[0]["GenericName"]);
                    dr["ItemName"] = common.myStr(dt1.Rows[0]["ItemName"]);
                    dr["FormulationId"] = common.myInt(dt1.Rows[0]["FormulationId"]);
                    dr["RouteId"] = common.myInt(dt1.Rows[0]["RouteId"]);
                    dr["RouteName"] = common.myInt(dt1.Rows[0]["RouteName"]);
                    dr["FrequencyId"] = common.myInt(dt1.Rows[0]["FrequencyId"]);
                    dr["FrequencyName"] = string.Empty;
                    dr["Duration"] = common.myStr(dt1.Rows[0]["Duration"]);
                    dr["Type"] = common.myStr(dt1.Rows[0]["Type"]);
                    dr["DurationText"] = common.myStr(dt1.Rows[0]["DurationText"]);
                    dr["Dose"] = common.myDbl(dt1.Rows[0]["Dose"]);
                    dr["UnitId"] = common.myInt(dt1.Rows[0]["UnitId"]);
                    dr["UnitName"] = string.Empty;
                    dr["FoodRelationshipId"] = 0;
                    dr["FoodRelationship"] = string.Empty;
                    dr["StartDate"] = count.Equals(0) ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(1);
                    dr["EndDate"] = count.Equals(0) ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[0]["Duration"]) - 1) : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(common.myInt(dt1.Rows[0]["Duration"]));
                    //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                    //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
                    //dr["Qty"] = "";
                    dr["PrescriptionDetail"] = string.Empty;
                    dr["ReferanceItemId"] = common.myInt(dt1.Rows[0]["ReferanceItemId"]);
                    dr["ReferanceItemName"] = common.myStr(dt1.Rows[0]["ReferanceItemName"]);
                    dr["Instructions"] = common.myStr(dt1.Rows[0]["Instructions"]);
                    //dr["XMLData"] = "";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    count++;
                }
            }
            //ViewState["ItemDetail"] = null;
            ViewState["DataTableDetail"] = dt;
            ViewState["StopItemDetail"] = null;
            ViewState["Stop"] = null;
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
            dt1.Dispose();
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int PrescriptionId = common.myInt(Session["CurentSavedPrescriptionId"]);
            if (PrescriptionId.Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Prescription not Selected !";
                dvConfirmPrint.Visible = false;
                return;
            }
            lblMessage.Text = string.Empty;
            RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(ViewState["EncId"]) + "&PId=" + PrescriptionId;
            RadWindow1.Height = 600;
            RadWindow1.Width = 800;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvPrevious_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (common.myBool(Session["IsCIMSInterfaceActive"])
            //    || common.myBool(Session["IsVIDALInterfaceActive"]))
            //{
            //    string strInterface = "CIMS";
            //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
            //    {
            //        strInterface = "CIMS";
            //    }
            //    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            //    {
            //        strInterface = "VIDAL";
            //    }
            //    GridView HeaderGrid = (GridView)sender;
            //    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //    TableCell Cell_Header = new TableCell();
            //    Cell_Header.Text = "";
            //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
            //    Cell_Header.ColumnSpan = 2;
            //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
            //    HeaderRow.Cells.Add(Cell_Header);
            //    Cell_Header = new TableCell();
            //    Cell_Header.Text = strInterface + " Information";
            //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
            //    Cell_Header.ColumnSpan = 5;
            //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
            //    Cell_Header.ForeColor = System.Drawing.Color.Red;
            //    Cell_Header.Font.Bold = true;
            //    HeaderRow.Cells.Add(Cell_Header);
            //    Cell_Header = new TableCell();
            //    Cell_Header.Text = "";
            //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
            //    Cell_Header.ColumnSpan = 1;
            //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
            //    HeaderRow.Cells.Add(Cell_Header);
            //    gvPrevious.Controls[0].Controls.AddAt(0, HeaderRow);
            //}
        }
    }
    protected void gvPrevious_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //Find the checkbox control in header and add an attribute
                    ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllCurrent('" +
                        ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
                }
                e.Row.Cells[(byte)enumCurrent.OrderNo].Visible = false;
                e.Row.Cells[(byte)enumCurrent.OrderDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.IndentType].Visible = false;
                e.Row.Cells[(byte)enumCurrent.TotalQty].Visible = false;
                e.Row.Cells[(byte)enumCurrent.PrescriptionDetail].Visible = true;
                e.Row.Cells[(byte)enumCurrent.StartDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.EndDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.StopRemarks].Visible = false;
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            clsCIMS objCIMS = new clsCIMS();
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            try
            {
                Label TotalQty = (Label)e.Row.FindControl("lblTotalQty");
                TotalQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                e.Row.Cells[(byte)enumCurrent.OrderNo].Visible = false;
                e.Row.Cells[(byte)enumCurrent.OrderDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.IndentType].Visible = false;
                e.Row.Cells[(byte)enumCurrent.TotalQty].Visible = false;
                e.Row.Cells[(byte)enumCurrent.PrescriptionDetail].Visible = true;
                e.Row.Cells[(byte)enumCurrent.StartDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.EndDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.StopRemarks].Visible = false;

                gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.DIInteractionCIMS].Visible = false;

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.DIInteractionCIMS].Visible = true;
                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDIInteractionCIMS");
                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                    if (common.myLen(hdnCIMSItemId.Value).Equals(0)
                        || common.myStr(hdnCIMSItemId.Value).Trim().Equals("0"))
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                        lnkBtnDAInteractionCIMS.Visible = false;
                        lnkBtnDIInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");
                        string strXML = string.Empty;
                        if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        {
                            strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));
                            if (!strXML.Equals(string.Empty))
                            {
                                string outputValues = objCIMS.getFastTrack5Output(strXML);
                                if (outputValues != null)
                                {
                                    string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" name=";
                                    if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                                    {
                                        lnkBtnBrandDetailsCIMS.Visible = true;
                                    }
                                }
                            }
                        }
                        lnkBtnMonographCIMS.Visible = false;
                        strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));
                        if (!strXML.Equals(string.Empty))
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);
                            if (outputValues != null)
                            {
                                if (outputValues.ToUpper().Contains("<MONOGRAPH>"))
                                {
                                    lnkBtnMonographCIMS.Visible = true;
                                }
                            }
                        }
                    }
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = true;
                    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsVIDAL");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");
                    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionVIDAL");
                    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    lnkBtnBrandDetailsVIDAL.Visible = true;
                    lnkBtnMonographVIDAL.Visible = true;
                    lnkBtnInteractionVIDAL.Visible = true;
                    lnkBtnDHInteractionVIDAL.Visible = true;
                    //lnkBtnDAInteractionVIDAL.Visible = true;
                    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    {
                        lnkBtnBrandDetailsVIDAL.Visible = false;
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                        lnkBtnDAInteractionVIDAL.Visible = false;
                    }
                }
                if (!chkShowDetails.Checked)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.DIInteractionCIMS].Visible = false;
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = false;
                    }
                }
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");

                HiddenField hdnGGenericId = (HiddenField)e.Row.FindControl("hdnGenericId");
                HiddenField hdnGDItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
                LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");
                Label lblGenericName = (Label)e.Row.FindControl("lblGenericName");
                lblGenericName.Visible = false;
                lnkItemName.Text = common.myStr(lnkItemName.Text).Trim();
                if (ViewState["GridDataDetail"] != null)
                {
                    dt = (DataTable)ViewState["GridDataDetail"];
                    dv = new DataView(dt);
                    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value) + " AND ISNULL(GenericId,0)=" + common.myInt(hdnGGenericId.Value) + " AND ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringNew(dv.ToTable(), chkFractionalDose.Checked ? "Y" : "N");
                        lnkItemName.ToolTip = common.myStr(lblPrescriptionDetail.Text);
                    }
                    else
                    {
                        dv1 = new DataView(dt);
                        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringNew(dv1.ToTable(), chkFractionalDose.Checked ? "Y" : "N");
                            lnkItemName.ToolTip = common.myStr(lblPrescriptionDetail.Text);
                        }
                    }
                }

                if (common.myInt(hdnGGenericId.Value) > 0 && common.myInt(hdnGDItemId.Value) <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
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
                objEMR = null;
                objCIMS = null;
                dt.Dispose();
                dv.Dispose();
                dv1.Dispose();
            }
        }
    }
    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECTITEM"))
            {
                chkCustomMedication.Checked = false;
                setCustomMedicationChange();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                //if (ItemId == 0 && GenericId == 0)
                //{
                //    return;
                //}
                if (ItemId <= 0 && GenericId.Equals(0))
                {
                    return;
                }
                string CIMSItemId = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                string CIMSType = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);
                int VIDALItemId = common.myInt(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

                string strType = common.myStr(((HiddenField)row.FindControl("hdnType")).Value);
                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(strType));
                if (ItemId > 0)
                {
                    CheckgenerateInstruction(ItemId);
                    //ddlFormulation.Enabled = false;
                    //ddlRoute.Enabled = false;
                    //ddlStrength.Enabled = false;
                }
                else
                {
                    ddlFormulation.Enabled = true;
                    ddlRoute.Enabled = true;
                    //ddlStrength.Enabled = true;
                    ddlStrengthValue.Enabled = true;
                }
                hdnGenericId.Value = common.myStr(GenericId);
                hdnItemId.Value = common.myStr(ItemId);
                //hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                //hdnCIMSType.Value = common.myStr(CIMSType);
                //hdnVIDALItemId.Value = common.myStr(VIDALItemId);
                Label GenericName = (Label)row.FindControl("lblGenericName");
                LinkButton lnkItemName = (LinkButton)row.FindControl("lnkItemName");
                Label TotalQty = (Label)row.FindControl("lblTotalQty");
                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
                HiddenField hdnStrengthValue = (HiddenField)row.FindControl("hdnStrengthValue");
                HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                HiddenField hdnDays = (HiddenField)row.FindControl("hdnDays");
                HiddenField hdnStartDate = (HiddenField)row.FindControl("hdnStartDate");
                HiddenField hdnEndDate = (HiddenField)row.FindControl("hdnEndDate");
                HiddenField hdnRemarks = (HiddenField)row.FindControl("hdnRemarks");
                HiddenField hdnUnitId = (HiddenField)row.FindControl("hdnUnitId");
                HiddenField hdnDuration = (HiddenField)row.FindControl("hdnDuration");
                HiddenField hdnInstructions = (HiddenField)row.FindControl("hdnInstructions");
                //HiddenField hdnIsInfusion = (HiddenField)row.FindControl("hdnIsInfusion");
                HiddenField hdnFoodRelationshipId = (HiddenField)row.FindControl("hdnFoodRelationshipId");
                checkitemquintity(ItemId);
                try
                {
                    hdnItemName.Value = string.Empty;
                    hdnGenericName.Value = string.Empty;
                    lblGenericName.Text = string.Empty;

                    ddlBrand.Text = string.Empty;
                    ddlBrand.SelectedValue = null;
                    ddlGeneric.Text = string.Empty;
                    ddlGeneric.SelectedValue = null;

                    if (ItemId > 0)
                    {
                        ddlBrand.Text = common.myStr(lnkItemName.Text);
                        ddlBrand.SelectedValue = ItemId.ToString();
                    }
                    else if (GenericId > 0 && ItemId <= 0)
                    {
                        ddlGeneric.Text = common.myStr(lnkItemName.Text);
                        ddlGeneric.SelectedValue = GenericId.ToString();
                    }
                }
                catch
                {
                }
                double days = common.myDbl(hdnDays.Value);
                double period = days;
                string periodType = "D";
                if ((days % 356).Equals(0))
                {
                    period = days / 365;
                    periodType = "Y";
                }
                else if ((days % 30).Equals(0))
                {
                    period = days / 30;
                    periodType = "M";
                }
                else if ((days % 7).Equals(0))
                {
                    period = days / 7;
                    periodType = "W";
                }
                else
                {
                    period = days;
                    periodType = "D";
                }

                if (ItemId > 0)
                {
                    hdnItemName.Value = common.myStr(lnkItemName.Text);
                    hdnGenericName.Value = common.myStr(GenericName.Text);
                    lblGenericName.Text = common.myStr(GenericName.Text);
                }
                else if (GenericId > 0 && ItemId <= 0)
                {
                    hdnGenericName.Value = common.myStr(lnkItemName.Text);
                    lblGenericName.Text = common.myStr(lnkItemName.Text);
                }

                //txtTotQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));
                //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(hdnFormulationId.Value).ToString()));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(hdnRouteId.Value).ToString()));
                //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));
                ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                txtDose.Text = hdnDose.Value;//"1"
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    txtDuration.Text = hdnDuration.Value;
                }
                else
                {
                    if (common.myDbl(hdnDuration.Value) > 0)
                    {
                        txtDuration.Text = hdnDuration.Value;
                    }
                }

                setDefaultFavourite(ItemId);
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myStr(hdnFrequencyId.Value)));
                txtInstructions.Text = hdnInstructions.Value;
                // txtDays.Text = common.myStr(period);
                // ddlPeriodType.SelectedValue = periodType;
                //txtStartDate.SelectedDate = common.myDate(hdnStartDate.Value);
                //txtEndDate.SelectedDate = common.myDate(hdnEndDate.Value);
                //txtRemarks.Text = common.myStr(hdnRemarks.Value);
                //if (common.myInt(hdnIsInfusion.Value) == 1)
                //{
                //    RadComboBoxItem item = new RadComboBoxItem();
                //    item.Text = common.myStr(hdnItemName.Value);
                //    item.Value = common.myStr(ItemId);
                //    item.Attributes.Add("IsInfusion", common.myStr(hdnIsInfusion.Value));
                //    item.DataBind();
                //    ddlReferanceItem.Items.Add(item);
                //}
            }
            else if (e.CommandName.ToUpper().Equals("ITEMSTOP")) // || e.CommandName == "ItemCancel")
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnIndentDetailsId = (HiddenField)row.FindControl("hdnIndentDetailsId");
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                if (common.myInt(hdnIndentId.Value).Equals(0) && ItemId.Equals(0))
                {
                    return;
                }
                ViewState["IndentDetailsId"] = common.myInt(hdnIndentDetailsId.Value).ToString();
                ViewState["StopItemId"] = ItemId.ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);
                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                if (e.CommandName.ToUpper().Equals("ITEMSTOP"))//Stop
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;
                    //hshOutput = emr.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    //               common.myInt(hdnIndentId.Value), ItemId, common.myInt(Session["UserId"]), common.myInt(ViewState["RegId"]),
                    //               common.myInt(ViewState["EncId"]), 0, txtRemarks.Text, common.myStr(Session["OPIP"]));
                    //lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
                }
                else//Cancel
                {
                    //hshOutput = emr.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),
                    //              Convert.ToInt32(hdnIndentId.Value), ItemId, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(ViewState["RegId"]),
                    //              Convert.ToInt32(ViewState["EncId"]), 1, txtRemarks.Text, Session["OPIP"].ToString());
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindGrid(string.Empty, string.Empty, string.Empty);
                //setVisiblilityInteraction();
            }
            else if (e.CommandName.ToUpper().Equals("BRANDDETAILSCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("MONOGRAPHCIMS"))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName.ToUpper().Equals("INTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            else if (e.CommandName.ToUpper().Equals("DIINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showDiIntreraction();
            }
            else if (e.CommandName.ToUpper().Equals("DHINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName.ToUpper().Equals("DAINTERACTIONCIMS"))
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("A");
            }
            else if (e.CommandName.ToUpper().Equals("BRANDDETAILSVIDAL"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getBrandDetailsVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName.ToUpper().Equals("MONOGRAPHVIDAL"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName.ToUpper().Equals("INTERACTIONVIDAL"))
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName.ToUpper().Equals("DHINTERACTIONVIDAL"))
            {
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName.ToUpper().Equals("DAINTERACTIONVIDAL"))
            {
                showHealthOrAllergiesIntreraction("A");
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
            objEMR = null;
        }
    }
    protected void lstFavourite_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllFavourite('" +
                    ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //HiddenField hdnCalculationBase = (HiddenField)e.Row.FindControl("hdnCalculationBase");
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");
                    CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
                    ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");

                    rdoSearchMedication.Enabled = false;
                    ibtnDelete.Enabled = false;
                    lnkItemName.Enabled = false;
                    chkRow.Enabled = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btnRefresh_OnClick(object sender, System.EventArgs e)
    {
        try
        {
            BindGrid(common.myStr(hdnReturnIndentIds.Value), common.myStr(hdnReturnItemIds.Value), common.myStr(hdnReturnGenericIds.Value));
        }
        catch
        {
        }
    }
    protected void btnGetFavourite_OnClick(object sender, System.EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsSearch = new DataSet();
        try
        {
            try
            {
                ddlBrand.ClearSelection();
                ddlBrand.Items.Clear();
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = "0";
            }
            catch
            {
            }
            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0,
                                0, common.myInt(Request.QueryString["DoctorId"]), string.Empty, string.Empty);
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSearch.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = (string)dsSearch.Tables[0].Rows[i]["ItemName"];
                    item.Value = common.myStr(dsSearch.Tables[0].Rows[i]["ItemId"]);
                    item.Attributes.Add("ClosingBalance", common.myStr(dsSearch.Tables[0].Rows[i]["ClosingBalance"]));
                    item.Attributes.Add("CIMSItemId", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSItemId"]));
                    item.Attributes.Add("CIMSType", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSType"]));
                    item.Attributes.Add("VIDALItemId", common.myInt(dsSearch.Tables[0].Rows[i]["VIDALItemId"]).ToString());

                    item.Attributes.Add("ItemFlagName", common.myStr(dsSearch.Tables[0].Rows[i]["ItemFlagName"]));
                    item.Attributes.Add("ItemFlagCode", common.myInt(dsSearch.Tables[0].Rows[i]["ItemFlagCode"]).ToString());
                    this.ddlBrand.Items.Add(item);
                    item.DataBind();
                }
            }
            try
            {
                ddlBrand.SelectedValue = common.myInt(hdnItemId.Value).ToString();
            }
            catch
            {
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
            dsSearch.Dispose();
            objEMR = null;
        }
    }
    protected void btnPreviousMedications_Click(Object sender, EventArgs e)
    {
        try
        {
            Session["RunningCIMSXMLInputData"] = getRunningInterationXML();
            ViewState["Mode"] = "A";
            RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=" + common.myBool(ViewState["Consumable"]);
            RadWindow1.Height = 570;
            RadWindow1.Width = 850;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }

    protected void BindGrid(string IdentId, string ItemId, string GenericId)
    {
        //if (rdoSearchMedication.SelectedValue != "C")
        //{
        //    return;
        //}
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dvNew = new DataView();
        DataView dv1New = new DataView();
        DataView dvConsumable = new DataView();
        DataView dvDistinct = new DataView();
        DataTable tblDistinct = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable tblReorderOverrideComments = new DataTable();
        DataTable tblDataItemDetails = new DataTable();
        DataTable dtDataItem = new DataTable();
        DataTable dtData = new DataTable();
        try
        {
            ViewState["Mode"] = IdentId.Equals(string.Empty) && ItemId.Equals(string.Empty) ? "P" : ViewState["Mode"];
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ds = objPhr.getPreviousMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                common.myInt(ViewState["EncId"]), 0, 0, ViewState["Mode"] == null ? "P" : common.myStr(ViewState["Mode"]),
                                common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty, string.Empty);
            }
            else
            {
                if (common.myLen(hdnReturnIndentDetailsIds.Value) > 0 && !IdentId.Equals(string.Empty) && !ItemId.Equals(string.Empty))
                {
                    ds = objEMR.getPreviousMedicationOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["EncId"]), common.myInt(ViewState["RegId"]), 0, 0, "P",
                                    common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty);
                }
                else
                {
                    ds = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["EncId"]), common.myInt(ViewState["RegId"]), 0, 0, ViewState["Mode"] == null ? "P" : common.myStr(ViewState["Mode"]),
                                    common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty);
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                dv = new DataView(ds.Tables[0]);
                dvNew = new DataView(ds.Tables[0]);
                dv1 = new DataView(ds.Tables[1]);
                dv1New = new DataView(ds.Tables[1]);
                dvConsumable = new DataView();

                if (!IdentId.Equals(string.Empty) && (!ItemId.Equals(string.Empty) || !GenericId.Equals(string.Empty)))
                {
                    if (common.myLen(hdnReturnIndentDetailsIds.Value) > 0)
                    {
                        ViewState["Stop"] = true;
                        if (!ItemId.Equals(string.Empty))
                        {
                            dv.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";

                            dvNew.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ")  AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";
                            dv1.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(ItemId,0) IN (" + ItemId + ")  AND ISNULL(IndentId,0) IN (" + IdentId + ")";

                            dv1New.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") ";
                        }
                        else
                        {
                            if (!GenericId.Equals(string.Empty))
                            {
                                dv.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(GenericId,0) IN (" + GenericId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";

                                dvNew.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(GenericId,0) IN (" + GenericId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";
                                dv1.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(GenericId,0) IN (" + GenericId + ")  AND ISNULL(IndentId,0) IN (" + IdentId + ")";

                                dv1New.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ")  AND ISNULL(GenericId,0) IN (" + GenericId + ")";
                            }
                        }
                    }
                    else
                    {
                        dv.RowFilter = "ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";
                        dv1.RowFilter = "ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";
                    }
                    //  dv.Table.Merge(dvNew.Table);
                    //  dv1.Table.Merge(dv1New.Table);
                    //dv.ToTable().Merge(dvNew.ToTable());
                    //dv1.ToTable().Merge(dv1New.ToTable());
                    dt = dv.ToTable().Copy();
                    dt.Merge(dvNew.ToTable());
                    dt1 = dv1.ToTable().Copy();
                    dt1.Merge(dv1New.ToTable());

                    if (!IdentId.Equals(string.Empty) && !ItemId.Equals(string.Empty))
                    {
                        try
                        {
                            int duration = 1;
                            for (int idx = 0; idx < dt.Rows.Count; idx++)
                            {
                                try
                                {
                                    if (dt.Rows[idx]["StartDate"] != null && dt.Rows[idx]["EndDate"] != null)
                                    {
                                        if (common.myDate(dt.Rows[idx]["StartDate"]) < common.myDate(dt.Rows[idx]["EndDate"]))
                                        {
                                            TimeSpan t = common.myDate(dt.Rows[idx]["EndDate"]) - common.myDate(dt.Rows[idx]["StartDate"]);
                                            duration = (int)t.TotalDays;
                                            if (duration > 1)
                                            {
                                                duration += 1;
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                if (duration < 1)
                                {
                                    duration = 1;
                                }
                                dt.Rows[idx]["StartDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                                dt.Rows[idx]["EndDate"] = endDateCalculate(DateTime.Now, duration, "D").ToString("dd/MM/yyyy");
                            }
                            dt.AcceptChanges();
                            for (int idx = 0; idx < dt1.Rows.Count; idx++)
                            {
                                try
                                {
                                    if (dt1.Rows[idx]["StartDate"] != null && dt1.Rows[idx]["EndDate"] != null)
                                    {
                                        if (common.myDate(dt1.Rows[idx]["StartDate"]) < common.myDate(dt1.Rows[idx]["EndDate"]))
                                        {
                                            TimeSpan t = common.myDate(dt1.Rows[idx]["EndDate"]) - common.myDate(dt1.Rows[idx]["StartDate"]);
                                            duration = (int)t.TotalDays;
                                            if (duration > 1)
                                            {
                                                duration += 1;
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                if (duration < 1)
                                {
                                    duration = 1;
                                }
                                dt1.Rows[idx]["StartDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                                dt1.Rows[idx]["EndDate"] = endDateCalculate(DateTime.Now, duration, "D").ToString("dd/MM/yyyy");
                            }
                            dt1.AcceptChanges();
                        }
                        catch
                        {
                        }
                    }
                    if (ViewState["Item"] == null && ViewState["ItemDetail"] == null)
                    {
                        ViewState["Item"] = dt;
                        ViewState["ItemDetail"] = dt1;
                    }
                    else
                    {
                        BindItemWithItemDetail(dt, dt1);
                    }
                    ViewState["Stop"] = true;
                    if (common.myBool(Session["IsCIMSInterfaceActive"])
                        || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        if (Session["TblReorderOverrideComments"] != null)
                        {
                            tblReorderOverrideComments = (DataTable)Session["TblReorderOverrideComments"];
                            if (tblReorderOverrideComments != null)
                            {
                                if (ViewState["Item"] != null)
                                {
                                    tblDataItemDetails = (DataTable)ViewState["Item"];
                                    foreach (DataRow DR in tblDataItemDetails.Rows)
                                    {
                                        if (common.myInt(DR["ItemId"]) > 0)
                                        {
                                            tblReorderOverrideComments.DefaultView.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(DR["ItemId"]);
                                        }
                                        else
                                        {
                                            tblReorderOverrideComments.DefaultView.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(DR["GenericId"]);
                                        }
                                        if (tblReorderOverrideComments.DefaultView.Count > 0)
                                        {
                                            DR["OverrideComments"] = common.myStr(tblReorderOverrideComments.DefaultView[0]["CommentsDrugAllergy"]);
                                            DR["OverrideCommentsDrugToDrug"] = common.myStr(tblReorderOverrideComments.DefaultView[0]["CommentsDrugToDrug"]);
                                            DR["OverrideCommentsDrugHealth"] = common.myStr(tblReorderOverrideComments.DefaultView[0]["CommentsDrugHealth"]);
                                            tblDataItemDetails.AcceptChanges();
                                        }
                                        tblReorderOverrideComments.DefaultView.RowFilter = string.Empty;
                                    }
                                    ViewState["Item"] = tblDataItemDetails;
                                }
                            }
                        }
                    }
                    try
                    {
                        dtDataItem = (DataTable)ViewState["Item"];
                        dtDataItem = addColumnInItemGrid(dtDataItem);
                        ViewState["Item"] = dtDataItem;

                        dtData = (DataTable)ViewState["ItemDetail"];
                        dtData = addColumnInItemGrid(dtData);
                        ViewState["ItemDetail"] = dtData;

                        foreach (DataRow item in dtData.Rows)
                        {
                            if (common.myInt(item["UnAppPrescriptionId"]).Equals(0))
                            {
                                int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(item);
                                item["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                            }
                        }
                    }
                    catch
                    {
                    }
                    ViewState["Item"] = dtData;
                    ViewState["ItemDetail"] = dtData;
                    gvItem.DataSource = dtData;
                    gvItem.DataBind();
                }
                else
                {
                    ViewState["GridDataItem"] = ds.Tables[0];
                    ViewState["GridDataDetail"] = ds.Tables[1];
                    if (Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
                    {
                        gvPrevious.Visible = false;
                    }
                    else
                    {
                        if (ViewState["Consumable"] != null && common.myBool(ViewState["Consumable"]))
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF','OMC')";

                            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                            {
                                try
                                {
                                    dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ISNULL(CustomMedication,False) <> True AND ItemCategoryShortName IN ('MC') ";
                                }
                                catch
                                {
                                    dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ISNULL(CustomMedication,0) <> 1  AND ItemCategoryShortName IN ('MC') ";
                                }
                            }
                            else
                            {
                                try
                                {
                                    dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ISNULL(CustomMedication,False) <> True ";
                                }
                                catch
                                {
                                    dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ISNULL(CustomMedication,0) <> 1 ";
                                }
                            }
                        }
                        else
                        {
                            dvConsumable = new DataView(ds.Tables[1]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED','NUT')";

                            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                            {
                                try
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(CustomMedication,False) <> True AND ItemCategoryShortName IN ('MED','NUT') ";
                                }
                                catch
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(CustomMedication,0) <> 1 AND ItemCategoryShortName IN ('MED','NUT') ";
                                }
                            }
                            else
                            {
                                try
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(CustomMedication,False) <> True ";
                                }
                                catch
                                {
                                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ISNULL(CustomMedication,0) <> 1 ";
                                }
                            }
                        }
                        //remove repeated drug
                        tblDistinct = dvConsumable.ToTable().Clone();
                        foreach (DataRow item in dvConsumable.ToTable().Rows)
                        {
                            dvDistinct = tblDistinct.Copy().DefaultView;
                            dvDistinct.RowFilter = "ItemName='" + common.myStr(item["ItemName"], true) + "'";
                            if (dvDistinct.Count.Equals(0))
                            {
                                dvDistinct.RowFilter = string.Empty;
                                tblDistinct.ImportRow(item);
                            }
                        }
                        if (tblDistinct.Rows.Count == 0)
                        {
                            DataRow DR = tblDistinct.NewRow();
                            tblDistinct.Rows.Add(DR);
                        }
                        gvPrevious.DataSource = tblDistinct;
                        gvPrevious.DataBind();
                        setVisiblilityInteraction();
                    }
                }
            }
            else
            {
                if (Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
                {
                    gvPrevious.Visible = false;
                }
                BindBlankPreviousGrid();
            }
            setVisiblilityInteraction();
            ViewState["Mode"] = null;
            bindOnLoadData();
            bindUnApprovedPrescriptions();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPhr = null;
            objEMR = null;
            ds.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dvConsumable.Dispose();
            dvDistinct.Dispose();
            tblDistinct.Dispose();
            dt.Dispose();
            dt1.Dispose();
            tblReorderOverrideComments.Dispose();
            tblDataItemDetails.Dispose();
            dtDataItem.Dispose();
            dtData.Dispose();
        }
    }

    private DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("IndentNo", typeof(int));
        dt.Columns.Add("IndentDate", typeof(string));
        dt.Columns.Add("IndentTypeId", typeof(int));
        dt.Columns.Add("IndentType", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("GenericName", typeof(string));
        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("IndentId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("FormulationId", typeof(int));
        dt.Columns.Add("RouteId", typeof(int));
        dt.Columns.Add("StrengthId", typeof(int));
        dt.Columns.Add("StrengthValue", typeof(string));
        dt.Columns.Add("CIMSItemId", typeof(string));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("PrescriptionDetail", typeof(string));
        dt.Columns.Add("ReferanceItemId", typeof(int));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        dt.Columns.Add("CustomMedication", typeof(bool));
        dt.Columns.Add("NotToPharmacy", typeof(bool));
        dt.Columns.Add("XMLData", typeof(string));
        dt.Columns.Add("IsInfusion", typeof(bool));
        dt.Columns.Add("IsInjection", typeof(bool));
        dt.Columns.Add("Dose", typeof(double));
        dt.Columns.Add("Days", typeof(double));
        dt.Columns.Add("RouteName", typeof(string));
        dt.Columns.Add("Frequency", typeof(decimal));
        dt.Columns.Add("FrequencyName", typeof(string));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Duration", typeof(string));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("DurationText", typeof(string));
        dt.Columns.Add("UnitId", typeof(int));
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("FoodRelationshipId", typeof(int));
        dt.Columns.Add("FoodRelationshipName", typeof(string));
        dt.Columns.Add("FoodRelationship", typeof(string));
        dt.Columns.Add("ReferanceItemName", typeof(string));
        dt.Columns.Add("Instructions", typeof(string));
        dt.Columns.Add("DoseTypeId", typeof(int));
        dt.Columns.Add("DoseTypeName", typeof(string));
        dt.Columns.Add("Volume", typeof(string));
        dt.Columns.Add("VolumeUnitId", typeof(int));
        dt.Columns.Add("InfusionTime", typeof(string));
        dt.Columns.Add("TimeUnit", typeof(string));
        dt.Columns.Add("TotalVolume", typeof(string));
        dt.Columns.Add("FlowRate", typeof(string));
        dt.Columns.Add("FlowRateUnit", typeof(int));
        dt.Columns.Add("XMLVariableDose", typeof(string));
        dt.Columns.Add("XMLFrequencyTime", typeof(string));
        dt.Columns.Add("FormulationName", typeof(string));
        dt.Columns.Add("DetailsId", typeof(int));
        dt.Columns.Add("OverrideComments", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugToDrug", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugHealth", typeof(string));
        dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
        dt.Columns.Add("ICDCode", typeof(string));
        dt.Columns.Add("UnAppPrescriptionId", typeof(int));
        dt.Columns.Add("InstructionRemarks", typeof(string));

        dt.Columns.Add("DurationType", typeof(string));
        dt.Columns.Add("ClosingBalanceQty", typeof(string));

        dt.Columns.Add("ItemFlagName", typeof(string));
        dt.Columns.Add("ItemFlagCode", typeof(string));
        return dt;
    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        dr["IndentTypeId"] = 0;
        dr["IndentType"] = string.Empty;
        dr["IndentNo"] = 0;
        dr["IndentDate"] = string.Empty;
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = string.Empty;
        dr["ItemName"] = string.Empty;
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["StrengthId"] = 0;
        dr["StrengthValue"] = string.Empty;
        dr["CIMSItemId"] = string.Empty;
        dr["CIMSType"] = string.Empty;
        dr["VIDALItemId"] = 0;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = string.Empty;
        dr["ReferanceItemId"] = 0;
        dr["StartDate"] = string.Empty;
        dr["EndDate"] = string.Empty;
        dr["CustomMedication"] = 0;
        dr["XMLData"] = string.Empty;
        dr["NotToPharmacy"] = false;
        dr["IsInfusion"] = false;
        dr["IsInjection"] = false;
        dr["InstructionRemarks"] = string.Empty;
        dr["DurationType"] = string.Empty;
        dr["ClosingBalanceQty"] = 0;

        dr["ItemFlagName"] = string.Empty;
        dr["ItemFlagCode"] = string.Empty;
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ItemDetail"] = null;
        gvItem.DataSource = dt;
        gvItem.DataBind();
        ViewState["DataTableItem"] = dt;
    }
    private void BindBlankPreviousGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        dr["IndentTypeId"] = 0;
        dr["IndentType"] = string.Empty;
        dr["IndentNo"] = 0;
        dr["IndentDate"] = string.Empty;
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = string.Empty;
        dr["ItemName"] = string.Empty;
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["StrengthId"] = 0;
        dr["StrengthValue"] = string.Empty;
        dr["CIMSItemId"] = string.Empty;
        dr["CIMSType"] = string.Empty;
        dr["VIDALItemId"] = 0;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = string.Empty;
        dr["ReferanceItemId"] = 0;
        dr["StartDate"] = string.Empty;
        dr["EndDate"] = string.Empty;
        dr["CustomMedication"] = 0;
        dr["NotToPharmacy"] = false;
        dr["XMLData"] = string.Empty;
        dr["IsInfusion"] = false;
        dr["IsInjection"] = false;
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ItemDetail"] = null;
        gvPrevious.DataSource = dt;
        gvPrevious.DataBind();
        ViewState["DataTableItem"] = dt;
        ViewState["GridDataItem"] = dt;
    }
    private void clearItemDetails()
    {
        try
        {
            lblGenericName.Text = string.Empty;
            //txtTotQty.Text = "0.00";
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;
            //ddlStrength.SelectedIndex = 0;
            ddlStrengthValue.ClearSelection();
            ddlStrengthValue.Text = string.Empty;
            //txtDose.Text = "";
            //ddlFrequency.SelectedIndex = 0;
            //txtDays.Text = "";
            //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
            //txtStartDate.SelectedDate = DateTime.Now;
            //txtEndDate.SelectedDate = DateTime.Now;
            //txtRemarks.Text = "";
            //txtICDCode.Text = string.Empty;
        }
        catch
        {
        }
    }
    private void BindICDPanel()
    {
        BaseC.EMROrders objOrd = new BaseC.EMROrders(sConString);
        DataSet dsTemp = new DataSet();
        DataTable dt = new DataTable();
        DataView DV = new DataView();
        DataTable tbl = new DataTable();
        try
        {
            if (ViewState["ICDCodes"] != null)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ICDCodes");
                dt.Columns.Add("Description");
                dt.Columns["ID"].AutoIncrement = true;
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;
                char[] chArray = { ',' };
                string[] serviceIdXml = common.myStr(ViewState["ICDCodes"]).Split(chArray);
                foreach (string item in serviceIdXml)
                {
                    DataRow drdt = dt.NewRow();

                    if (ViewState["ICDCodeMaster"] != null)
                    {
                        tbl = (DataTable)ViewState["ICDCodeMaster"];
                        DV = tbl.Copy().DefaultView;
                        if (tbl != null)
                        {
                            DV.RowFilter = "ICDCode='" + item.ToString() + "'";

                            if (DV.ToTable().Rows.Count > 0)
                            {
                                drdt["ICDCodes"] = item.ToString();
                                drdt["Description"] = common.myStr(DV.ToTable().Rows[0]["Description"]);
                                dt.Rows.Add(drdt);
                            }
                        }
                    }
                }
                //if (dt.Rows.Count == 1)
                //{
                //    txtICDCode.Text = dt.Rows[0]["ICDCodes"].ToString();
                //    hdnICDCode.Value = dt.Rows[0]["ICDCodes"].ToString();
                //}
            }
            //else
            //{
            //    txtICDCode.ReadOnly = true;
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
            dsTemp.Dispose();
            objOrd = null;
            DV.Dispose();
            tbl.Dispose();
        }
    }
    protected void AddOrderSet(string sICDCode)
    {
        DataSet dsDetail = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
        DataView dvRemoveBlank = new DataView();
        DataView DVItem = new DataView();
        DataView dvRemoveSameColumn = new DataView();
        DataView dv = new DataView();
        DataView dvv = new DataView();
        DataTable tblItem = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtt = new DataTable();
        try
        {
            string Type = string.Empty;
            tblItem = (DataTable)ViewState["ItemDetail"];//"GridData"
            if (tblItem == null)
            {
                tblItem = CreateItemTable();
                ViewState["ItemDetail"] = tblItem;
            }
            dvRemoveBlank = tblItem.Copy().DefaultView;
            //dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR ISNULL(CustomMedication,0)<>0";
            try
            {
                dvRemoveBlank.RowFilter = "ISNULL(GenericId,0) <> 0 OR ISNULL(ItemId,0) > 0 OR ISNULL(CustomMedication,False) <> False ";
            }
            catch
            {
                dvRemoveBlank.RowFilter = "ISNULL(GenericId,0) <> 0 OR ISNULL(ItemId,0) > 0 OR ISNULL(CustomMedication,0) <> 0 ";
            }

            tblItem = dvRemoveBlank.ToTable();
            ViewState["ItemDetail"] = tblItem;
            dt1 = tblItem.Copy();
            DVItem = tblItem.Copy().DefaultView;
            string customMedication = common.myStr(txtCustomMedication.Text).Trim();
            if (customMedication.Length > 0)
            {
                try
                {
                    DVItem.RowFilter = "ISNULL(CustomMedication,False)=" + chkCustomMedication.Checked;
                }
                catch
                {
                    DVItem.RowFilter = "ISNULL(CustomMedication,0)=" + (chkCustomMedication.Checked ? "1" : "0");
                }
            }
            else
            {
                if (common.myInt(hdnItemId.Value) > 0)
                {
                    DVItem.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                }
                else if (common.myInt(hdnGenericId.Value) > 0)
                {
                    DVItem.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGenericId.Value);
                }
            }
            bool IsAdded = false;
            if (DVItem.ToTable().Rows.Count.Equals(0))
            {
                string strIndentId = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (strIndentId.Equals(string.Empty))
                    {
                        strIndentId = strIndentId + dr["ItemId"];
                    }
                    else
                    {
                        strIndentId = strIndentId + "," + dr["ItemId"];
                    }
                }
                dsDetail = new DataSet();
                dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;

                        if (common.myStr(ViewState["EMRIsHideBrandInPrescription"]).Equals("Y"))
                        {
                            if (common.myStr(Session["OPIP"]).Equals("O"))
                            {
                                dv.RowFilter = "ISNULL(GenericId,0)>0 ";
                            }
                            else
                            {
                                dv.RowFilter = "ISNULL(ItemId,0)>0 ";
                            }
                        }
                        else
                        {
                            dv.RowFilter = " 2=2 ";
                        }

                        if (!strIndentId.Equals(string.Empty))
                        {
                            dv.RowFilter += " AND ISNULL(ItemId,0) NOT IN(" + strIndentId + ")";
                        }

                    }
                }
                if (dv.Count > 0)
                {
                    dt = dv.ToTable().Copy();
                    foreach (DataRow drM in dt.Rows)
                    {
                        DataRow DR1 = dt1.NewRow();
                        DR1["Id"] = 0;
                        DR1["IndentNo"] = 0;
                        DR1["IndentDate"] = string.Empty;
                        DR1["IndentTypeId"] = 0;
                        DR1["IndentType"] = string.Empty;
                        DR1["IndentId"] = 0;
                        DR1["OverrideComments"] = string.Empty;
                        DR1["OverrideCommentsDrugToDrug"] = string.Empty;
                        DR1["OverrideCommentsDrugHealth"] = string.Empty;
                        DR1["IsSubstituteNotAllowed"] = 0;
                        DR1["ICDCode"] = common.myStr(txtICDCode.Text);
                        DR1["StrengthValue"] = string.Empty;
                        DR1["ReferanceItemId"] = 0;
                        DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR1["NotToPharmacy"] = false;
                        DR1["IsInfusion"] = 0;
                        DR1["IsInjection"] = 0;
                        DR1["RouteName"] = common.myStr(drM["RouteName"]);
                        DR1["Duration"] = common.myInt(drM["Duration"]);
                        DR1["Type"] = common.myStr(drM["DtypeId"]);
                        DR1["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Dtype"]);
                        DR1["FormulationName"] = DBNull.Value;
                        DR1["XMLFrequencyTime"] = string.Empty;
                        DR1["XMLVariableDose"] = string.Empty;
                        DR1["FoodRelationshipId"] = 0;
                        DR1["DoseTypeId"] = 0;
                        DR1["VolumeUnitId"] = 0;
                        DR1["FlowRateUnit"] = 0;
                        DR1["DetailsId"] = 0;
                        DR1["FoodRelationship"] = DBNull.Value;
                        DR1["FoodRelationshipID"] = common.myInt(drM["FoodID"]);// DBNull.Value;
                        DR1["FoodRelationshipName"] = common.myStr(drM["FoodName"]);// DBNull.Value;
                        DR1["ReferanceItemName"] = DBNull.Value;
                        DR1["Instructions"] = common.myStr(drM["Instructions"]);
                        DR1["DoseTypeName"] = DBNull.Value;
                        DR1["Volume"] = string.Empty;
                        DR1["InfusionTime"] = string.Empty;
                        DR1["TimeUnit"] = string.Empty;
                        DR1["TotalVolume"] = string.Empty;
                        DR1["FlowRate"] = "0";

                        if (common.myInt(drM["itemid"]) > 0)
                        {
                            DR1["GenericId"] = DBNull.Value;
                            DR1["GenericName"] = string.Empty;
                            DR1["ItemId"] = common.myInt(drM["itemid"]);
                            DR1["ItemName"] = common.myStr(drM["itemName"]);
                        }
                        else
                        {
                            DR1["GenericId"] = common.myInt(drM["GenericId"]);
                            DR1["GenericName"] = common.myStr(drM["GenericName"]);
                            DR1["ItemId"] = DBNull.Value;
                            DR1["ItemName"] = string.Empty;
                        }

                        DR1["FormulationId"] = common.myInt(drM["FormulationId"]);
                        DR1["RouteId"] = common.myInt(drM["RouteId"]);
                        DR1["StrengthId"] = DBNull.Value;
                        DR1["Dose"] = common.myDbl(drM["Dose"]);
                        DR1["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                        DR1["Frequency"] = common.myStr(drM["Frequency"]);
                        DR1["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                        DR1["Days"] = common.myInt(drM["TotalDays"]);
                        DR1["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                        DR1["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                        DR1["Qty"] = common.myDbl(drM["Qty"]);
                        DR1["UnitName"] = common.myStr(drM["DoseUnit"]);
                        DR1["CustomMedication"] = 0;
                        //DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                        //DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
                        //DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                        dt1.Rows.Add(DR1);
                        dt1.AcceptChanges();
                        dt1.TableName = "ItemDetail";
                        StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                        dt1.WriteXml(writer);
                        DataRow DR = tblItem.NewRow();
                        DR["Id"] = 0;
                        DR["IndentNo"] = 0;
                        DR["IndentDate"] = string.Empty;
                        DR["IndentTypeId"] = 0;
                        DR["IndentType"] = string.Empty;
                        DR["IndentId"] = 0;
                        DR["OverrideComments"] = string.Empty;
                        DR["OverrideCommentsDrugToDrug"] = string.Empty;
                        DR["OverrideCommentsDrugHealth"] = string.Empty;
                        DR["IsSubstituteNotAllowed"] = 0;
                        DR["ICDCode"] = common.myStr(txtICDCode.Text);
                        DR["StrengthValue"] = string.Empty;
                        DR["ReferanceItemId"] = 0;
                        DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR["NotToPharmacy"] = false;
                        DR["IsInfusion"] = 0;
                        DR["IsInjection"] = 0;
                        DR["RouteName"] = common.myStr(drM["RouteName"]);
                        DR["Duration"] = common.myInt(drM["Duration"]);
                        DR["Type"] = common.myStr(drM["DtypeId"]);
                        DR["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Dtype"]);
                        DR["FormulationName"] = DBNull.Value;
                        DR["XMLFrequencyTime"] = string.Empty;
                        DR["XMLVariableDose"] = string.Empty;
                        DR["FoodRelationshipId"] = common.myInt(drM["FoodID"]);//0;
                        DR["FoodRelationshipName"] = common.myStr(drM["FoodName"]);// DBNull.Value;

                        DR["DoseTypeId"] = 0;
                        DR["VolumeUnitId"] = 0;
                        DR["FlowRateUnit"] = 0;
                        DR["DetailsId"] = 0;
                        DR["FoodRelationship"] = DBNull.Value;
                        DR["ReferanceItemName"] = DBNull.Value;
                        DR["Instructions"] = common.myStr(drM["Instructions"]); ;
                        DR["DoseTypeName"] = DBNull.Value;
                        DR["Volume"] = string.Empty;
                        DR["InfusionTime"] = string.Empty;
                        DR["TimeUnit"] = string.Empty;
                        DR["TotalVolume"] = string.Empty;
                        DR["FlowRate"] = "0";

                        if (common.myInt(drM["itemid"]) > 0)
                        {
                            DR["GenericId"] = DBNull.Value;
                            DR["GenericName"] = string.Empty;
                            DR["ItemId"] = common.myInt(drM["itemid"]);
                            DR["ItemName"] = common.myStr(drM["itemName"]);
                        }
                        else
                        {
                            DR["GenericId"] = common.myInt(drM["GenericId"]);
                            DR["GenericName"] = common.myStr(drM["GenericName"]);
                            DR["ItemId"] = DBNull.Value;
                            DR["ItemName"] = string.Empty;
                        }

                        DR["FormulationId"] = common.myInt(drM["FormulationId"]);
                        DR["RouteId"] = common.myInt(drM["RouteId"]);
                        DR["StrengthId"] = DBNull.Value;
                        DR["Dose"] = common.myDbl(drM["Dose"]);
                        DR["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                        DR["Frequency"] = common.myStr(drM["Frequency"]);
                        DR["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                        DR["Days"] = common.myInt(drM["TotalDays"]);
                        DR["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                        DR["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                        DR["Qty"] = common.myDbl(drM["Qty"]).ToString("F2");
                        DR["UnitName"] = common.myStr(drM["DoseUnit"]);
                        DR["CustomMedication"] = 0;
                        DR["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                        DR["CIMSType"] = common.myStr(hdnCIMSType.Value);
                        DR["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                        DR["XMLData"] = common.myStr(writer);
                        DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringNew(dt1);
                        tblItem.Rows.Add(DR);
                    }
                    tblItem.AcceptChanges();
                    dvRemoveSameColumn = tblItem.Copy().DefaultView;
                    dvRemoveSameColumn.RowFilter = "XMLData <> ''";
                    tblItem = dvRemoveSameColumn.ToTable();
                    ViewState["ItemDetail"] = tblItem.Copy();
                    ViewState["Item"] = tblItem.Copy();
                }
            }
            else
            {
                string strItemIds = string.Empty;
                string strGenericIds = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (common.myInt(dr["ItemId"]) > 0)
                    {
                        if (strItemIds.Equals(string.Empty))
                        {
                            strItemIds = strItemIds + common.myInt(dr["ItemId"]);
                        }
                        else
                        {
                            strItemIds = strItemIds + "," + common.myInt(dr["ItemId"]);
                        }
                    }
                    else
                    {
                        if (common.myInt(dr["GenericId"]) > 0)
                        {
                            if (strGenericIds.Equals(string.Empty))
                            {
                                strGenericIds = strGenericIds + common.myInt(dr["GenericId"]);
                            }
                            else
                            {
                                strGenericIds = strGenericIds + "," + common.myInt(dr["GenericId"]);
                            }
                        }
                    }
                }
                dsDetail = new DataSet();
                dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;
                        string strRowFilter = string.Empty;

                        if (!strItemIds.Equals(string.Empty))
                        {
                            strRowFilter = "ISNULL(ItemId,0) NOT IN(" + strItemIds + ")";
                        }

                        if (!strGenericIds.Equals(string.Empty))
                        {
                            if (!strRowFilter.Equals(string.Empty))
                            {
                                strRowFilter += " OR ";
                            }

                            strRowFilter += " ISNULL(GenericId,0) NOT IN(" + strGenericIds + ")";
                        }

                        if (!strRowFilter.Equals(string.Empty))
                        {
                            dv.RowFilter = strRowFilter;
                        }
                    }
                }
                if (dv.Count > 0)
                {
                    string[] TobeDistinct = { };
                    dt = dv.ToTable().Copy();
                    foreach (DataRow drM in dt.Rows)
                    {
                        bool IsExists = false;
                        dtt = new DataTable();
                        dtt = (DataTable)ViewState["Item"];//"GridData"
                        if (dtt != null)
                        {
                            dvv = dtt.Copy().DefaultView;
                            dvv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(drM["ItemId"]) + " AND ISNULL(GenericId,0)=" + common.myInt(drM["GenericId"]);
                            IsExists = common.myBool(dvv.ToTable().Rows.Count > 0);
                        }
                        if (!IsExists)
                        {
                            DataColumnCollection columns = tblItem.Columns;
                            DataRow DR1 = dt1.NewRow();
                            if (columns.Contains("Id"))
                            {
                                DR1["Id"] = 0;
                            }
                            if (columns.Contains("IndentNo"))
                            {
                                DR1["IndentNo"] = 0;
                            }
                            DR1["IndentDate"] = string.Empty;
                            DR1["IndentTypeId"] = 0;
                            DR1["IndentType"] = string.Empty;
                            DR1["IndentId"] = 0;
                            DR1["OverrideComments"] = string.Empty;
                            DR1["OverrideCommentsDrugToDrug"] = string.Empty;
                            DR1["OverrideCommentsDrugHealth"] = string.Empty;
                            DR1["IsSubstituteNotAllowed"] = 0;
                            DR1["ICDCode"] = common.myStr(txtICDCode.Text);
                            DR1["StrengthValue"] = string.Empty;
                            DR1["ReferanceItemId"] = 0;
                            DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR1["NotToPharmacy"] = false;
                            DR1["IsInfusion"] = 0;
                            DR1["IsInjection"] = 0;
                            DR1["RouteName"] = common.myStr(drM["RouteName"]);
                            DR1["Duration"] = common.myInt(drM["Duration"]);
                            DR1["Type"] = common.myStr(drM["DtypeId"]);
                            DR1["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Dtype"]);
                            DR1["FormulationName"] = DBNull.Value;
                            DR1["XMLFrequencyTime"] = string.Empty;
                            DR1["XMLVariableDose"] = string.Empty;
                            DR1["FoodRelationshipId"] = 0;
                            DR1["DoseTypeId"] = 0;
                            DR1["VolumeUnitId"] = 0;
                            DR1["FlowRateUnit"] = 0;
                            if (columns.Contains("DetailsId"))
                            {
                                DR1["DetailsId"] = 0;
                            }
                            DR1["FoodRelationship"] = DBNull.Value;
                            DR1["ReferanceItemName"] = DBNull.Value;
                            DR1["Instructions"] = string.Empty;
                            DR1["DoseTypeName"] = DBNull.Value;
                            DR1["Volume"] = string.Empty;
                            DR1["InfusionTime"] = string.Empty;
                            DR1["TimeUnit"] = string.Empty;
                            DR1["TotalVolume"] = string.Empty;
                            DR1["FlowRate"] = "0";
                            DR1["GenericId"] = common.myInt(drM["GenericId"]);
                            DR1["GenericName"] = common.myStr(drM["GenericName"]);
                            DR1["ItemId"] = common.myInt(drM["ItemId"]);
                            DR1["ItemName"] = common.myStr(drM["ItemName"]);
                            DR1["FormulationId"] = common.myInt(drM["FormulationId"]);
                            DR1["RouteId"] = common.myInt(drM["RouteId"]);
                            DR1["StrengthId"] = DBNull.Value;
                            DR1["Dose"] = common.myDbl(drM["Dose"]);
                            DR1["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                            DR1["Frequency"] = common.myStr(drM["Frequency"]);
                            DR1["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                            if (columns.Contains("Days"))
                            {
                                DR1["Days"] = common.myInt(drM["TotalDays"]);
                            }
                            DR1["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                            DR1["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                            DR1["Qty"] = common.myDbl(drM["Qty"]).ToString("F2");
                            DR1["UnitName"] = common.myStr(drM["DoseUnit"]);
                            DR1["CustomMedication"] = 0;
                            //DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                            //DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
                            //DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                            dt1.Rows.Add(DR1);
                            dt1.AcceptChanges();
                            dt1.TableName = "ItemDetail";
                            StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                            dt1.WriteXml(writer);
                            DataRow DR = tblItem.NewRow();
                            if (columns.Contains("Id"))
                            {
                                DR["Id"] = 0;
                            }
                            if (columns.Contains("IndentNo"))
                            {
                                DR["IndentNo"] = 0;
                            }
                            DR["IndentDate"] = string.Empty;
                            DR["IndentTypeId"] = 0;
                            DR["IndentType"] = string.Empty;
                            DR["IndentId"] = 0;
                            DR["OverrideComments"] = string.Empty;
                            DR["OverrideCommentsDrugToDrug"] = string.Empty;
                            DR["OverrideCommentsDrugHealth"] = string.Empty;
                            DR["IsSubstituteNotAllowed"] = 0;
                            DR["ICDCode"] = common.myStr(txtICDCode.Text);
                            DR["StrengthValue"] = string.Empty;
                            DR["ReferanceItemId"] = 0;
                            DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR["NotToPharmacy"] = false;
                            DR["IsInfusion"] = 0;
                            DR["IsInjection"] = 0;
                            DR["RouteName"] = common.myStr(drM["RouteName"]);
                            DR["Duration"] = common.myInt(drM["Duration"]);
                            DR["Type"] = common.myStr(drM["DtypeId"]);
                            DR["DurationText"] = common.myStr(drM["Days"]) + " " + common.myStr(drM["Dtype"]);
                            DR["FormulationName"] = DBNull.Value;
                            DR["XMLFrequencyTime"] = string.Empty;
                            DR["XMLVariableDose"] = string.Empty;
                            DR["FoodRelationshipId"] = 0;
                            DR["DoseTypeId"] = 0;
                            DR["VolumeUnitId"] = 0;
                            DR["FlowRateUnit"] = 0;
                            if (columns.Contains("DetailsId"))
                            {
                                DR["DetailsId"] = 0;
                            }
                            DR["FoodRelationship"] = DBNull.Value;
                            DR["ReferanceItemName"] = DBNull.Value;
                            DR["Instructions"] = string.Empty;
                            DR["DoseTypeName"] = DBNull.Value;
                            DR["Volume"] = string.Empty;
                            DR["InfusionTime"] = string.Empty;
                            DR["TimeUnit"] = string.Empty;
                            DR["TotalVolume"] = string.Empty;
                            DR["FlowRate"] = "0";
                            DR["GenericId"] = common.myInt(drM["GenericId"]);
                            DR["GenericName"] = common.myStr(drM["GenericName"]);
                            DR["ItemId"] = common.myInt(drM["ItemId"]);
                            DR["ItemName"] = common.myStr(drM["ItemName"]);
                            DR["FormulationId"] = common.myInt(drM["FormulationId"]);
                            DR["RouteId"] = common.myInt(drM["RouteId"]);
                            DR["StrengthId"] = DBNull.Value;
                            DR["Dose"] = common.myDbl(drM["Dose"]);
                            DR["FrequencyId"] = common.myInt(drM["FrequencyId"]);
                            DR["Frequency"] = common.myStr(drM["Frequency"]);
                            DR["FrequencyName"] = common.myStr(drM["FrequencyName"]);
                            if (columns.Contains("Days"))
                            {
                                DR["Days"] = common.myInt(drM["TotalDays"]);
                            }
                            DR["StartDate"] = common.myDate(drM["StartDate"]).ToString("dd/MM/yyyy");
                            DR["EndDate"] = common.myDate(drM["EndDate"]).ToString("dd/MM/yyyy");
                            DR["Qty"] = common.myDbl(drM["Qty"]).ToString("F2");
                            DR["UnitName"] = common.myStr(drM["DoseUnit"]);
                            DR["CustomMedication"] = 0;
                            //DR["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                            //DR["CIMSType"] = common.myStr(hdnCIMSType.Value);
                            //DR["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                            DR["XMLData"] = common.myStr(writer);
                            DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringNew(dt1);
                            tblItem.Rows.Add(DR);
                        }
                        tblItem.AcceptChanges();
                        dvRemoveSameColumn = tblItem.Copy().DefaultView;
                        //dvRemoveSameColumn.RowFilter = "((ISNULL(XMLData,'') <> '' and GenericId is null) OR (ISNULL(XMLData,'') = '' AND ISNULL(GenericId,0) >=0))";
                        tblItem = dvRemoveSameColumn.ToTable();
                        ViewState["ItemDetail"] = tblItem.Copy();
                        ViewState["Item"] = tblItem.Copy();
                    }
                }
            }
            ddlGeneric.Text = string.Empty;
            ddlBrand.Text = string.Empty;
            lblMessage.Text = string.Empty;
            hdnGenericId.Value = "0";
            hdnItemId.Value = "0";
            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = string.Empty;
            //hdnIsUnSavedData.Value = "1";
            hdnGenericName.Value = string.Empty;
            hdnItemName.Value = string.Empty;
            txtCustomMedication.Text = string.Empty;
            clearItemDetails();
            //DataView dvRemoveSameColumn = tblItem.Copy().DefaultView;
            //dvRemoveSameColumn.RowFilter = "XMLData <> ''";
            //tblItem = dvRemoveSameColumn.ToTable();
            //ViewState["ItemDetail"] = tblItem.Copy();
            //ViewState["Item"] = tblItem.Copy();
            //ViewState["ItemDetail"] = ViewState["GridDataItem"];
            tblItem = addColumnInItemGrid(tblItem.Copy());
            try
            {
                foreach (DataRow item in tblItem.Rows)
                {
                    if (common.myInt(item["UnAppPrescriptionId"]).Equals(0))
                    {
                        int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(item);
                        item["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                    }
                }
            }
            catch
            {
            }
            if (tblItem != null)
            {
                if (tblItem.Columns.Count > 0)
                {
                    if (!tblItem.Columns.Contains("ItemFlagName"))
                    {
                        tblItem.Columns.Add("ItemFlagName", typeof(string));
                    }
                    if (!tblItem.Columns.Contains("ItemFlagCode"))
                    {
                        tblItem.Columns.Add("ItemFlagCode", typeof(string));
                    }
                }
            }

            ViewState["Item"] = tblItem;
            gvItem.DataSource = tblItem.Copy();
            gvItem.DataBind();
            setVisiblilityInteraction();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            ddlBrand.Focus();

            //btnCopyLastPrescription.Enabled = false;
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            //lblMessage.Text = "Drug added in list !";
            btnCopyLastPrescription.Enabled = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ViewState["OrderSetId"] = "0";

            dsDetail.Dispose();
            dvRemoveBlank.Dispose();
            DVItem.Dispose();
            dvRemoveSameColumn.Dispose();
            dv.Dispose();
            dvv.Dispose();
            tblItem.Dispose();
            dt1.Dispose();
            dt.Dispose();
            dtt.Dispose();
            objEMR = null;
            objPhr = null;
            objMst = null;
        }
    }
    private bool isSavedItem()
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        bool isSave = true;
        string strmsg = string.Empty;
        try
        {
            if (common.myInt(ddlStore.SelectedValue).Equals(0))
            {
                strmsg += " Store not selected !";
                isSave = false;
            }

            //if (common.myInt(ddlGeneric.SelectedValue) == 0 && common.myInt(ddlBrand.SelectedValue) == 0 && !chkCustomMedication.Checked)
            if (common.myInt(hdnGenericId.Value).Equals(0) && common.myInt(hdnItemId.Value).Equals(0) && !chkCustomMedication.Checked)
            {
                if (common.myInt(hdnGenericId.Value).Equals(0) && common.myInt(hdnItemId.Value).Equals(0) && common.myInt(ddlBrand.SelectedValue) > 0)
                {
                    hdnItemId.Value = common.myInt(ddlBrand.SelectedValue).ToString();
                }
                else
                {
                    strmsg += " Generic Or Item not selected !";
                    isSave = false;
                }
            }
            //if (common.myInt(ddlGeneric.SelectedValue) == 0 && common.myInt(ddlBrand.SelectedValue) == 0
            //    && (chkCustomMedication.Checked && common.myLen(txtCustomMedication.Text) == 0))

            if (common.myInt(hdnGenericId.Value).Equals(0) && common.myInt(hdnItemId.Value).Equals(0)
                && (chkCustomMedication.Checked && common.myLen(txtCustomMedication.Text).Equals(0)))
            {
                strmsg += "Custom medication can't be blank !";
                isSave = false;
            }
            string sLOCATION = common.myStr(Request.QueryString["LOCATION"]);

            if (!sLOCATION.ToUpper().Equals("WARD"))
            {
                if (common.myDbl(txtDose.Text).Equals(0.0) && spnDose.Visible)
                {
                    if (common.myStr(Session["FacilityName"]).ToUpper().Contains("SPS") || common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                    {
                        strmsg += "Qty can not be zero or blank !";
                    }
                    else
                    {
                        strmsg += "Dose can not be zero or blank !";
                    }

                    isSave = false;
                }

                if (common.myDbl(txtDuration.Text).Equals(0)
                    && common.myStr(ddlDoseType.Text).ToUpper().Equals("PRN"))
                {
                    txtDuration.Text = "1";
                }

                if (common.myDbl(txtDuration.Text).Equals(0)
                    && !common.myStr(ddlDoseType.Text).ToUpper().Equals("STAT") && spnDuration.Visible && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("TO BE CONTINUED") && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("TILL NEXT VISIT"))
                {
                    strmsg += "Duration can not be zero or blank !";
                    isSave = false;
                }
            }
            //if (common.myDbl(txtTotQty.Text) == 0)
            //{
            //    strmsg += "Total quantity can not be zero or blank !";
            //    isSave = false;
            //}


            if (!common.myBool(ViewState["IsPrimaryDiagnosis"]))
            {
                if (!common.myStr(Session["OPIP"]).ToUpper().Equals("I") && trDiagnosis.Visible)
                {
                    strmsg += "Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..";
                    Alert.ShowAjaxMsg(strmsg, this);
                    isSave = false;
                }
            }


        }
        catch
        { }
        finally
        {
            objPhr = null;
        }
        lblMessage.Text = strmsg;
        return isSave;
    }
    protected void btnOkay_Click(object sender, EventArgs e)
    {
        dverx.Visible = false;
    }
    protected void RetrievePatientAllergies()
    {
        BaseC.Dashboard objD = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        DataView dvAllergy = new DataView();
        try
        {
            ViewState["DrugAllergy"] = null;
            ds = objD.getAllergies(common.myInt(ViewState["RegId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                dvAllergy = new DataView(ds.Tables[0]);
                dvAllergy.RowFilter = "AllergyType='Drug' AND ItemType='P'";
                // dvAllergy.RowFilter = "AllergyType='Drug'";
                ViewState["DrugAllergy"] = dvAllergy.ToTable();
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
            objD = null;
            ds.Dispose();
            dvAllergy.Dispose();
        }
    }
    protected void btnMedicationOverride_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnIsOverride.Value) > 0)
            {
                addItem(common.myStr(ViewState["ICDCodes"]));
                hdnItemId.Value = "0";
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;
                hdnGenericId.Value = "0";
                ddlGeneric.Text = string.Empty;
                ddlGeneric.SelectedValue = null;
                txtCustomMedication.Text = string.Empty;
                if (common.myStr(ViewState["ProceedValue"]).Equals("F"))
                {
                    proceedFavourite(true);
                }
                else if (common.myStr(ViewState["ProceedValue"]).Equals("C"))
                {
                    proceedCurrent(true);
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
    protected void btnAddItem_OnClick(object sender, EventArgs e)
    {
        clsEMR objEMR = new clsEMR(sConString);
        DataTable dt1old = (DataTable)ViewState["UnApprovedPrescriptions"];
        int ItemlistCount = gvItem.Rows.Count;
        string itemname = "";

        if (common.myInt(gvItem.Rows.Count) > 0)
        {
            int ItemId = common.myInt(ddlBrand.SelectedValue);
            string ItemCode = objEMR.checkDrugType(ItemId);
            if (ItemlistCount > 0 || itemname.Length > 0)
            {
                if (ItemCode.Contains("NARCOTIC"))
                {
                    Alert.ShowAjaxMsg("Selected Item is Narcotic,please remove items from list ", Page);
                    return;
                }
            }
        }
        if (common.myStr(ViewState["ICDCodes"]).Equals(string.Empty) && trDiagnosis.Visible)
        {
            if (!common.myStr(Session["OPIP"]).ToUpper().Equals("I"))
            {
                Alert.ShowAjaxMsg("Please Select ICDcode First!..", Page.Page);
                lblMessage.Text = "Please Select ICDcode First!..";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
        }
        if (ViewState["BlockItemId"] != null)
        {
            if (common.myBool(ViewState["IsAllowToAddBlockedItem"]) == false)
            {
                Alert.ShowAjaxMsg("Not authorized to Prescribe this Item. Selected Item is blocked for this company.", this.Page);
                return;
            }
            if (common.myBool(ViewState["Yes"]) == false)
            {
                divConfirmation.Visible = true;
                return;
            }
        }

        ////if (!common.myStr(Session["FacilityName"]).ToUpper().Contains("CHAITANYA"))
        {
            if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("PRN") && common.myStr(txtInstructions.Text).Trim().Equals(string.Empty))
            {
                lblMessage.Text = "Instructions are manditory when Order Type is PRN";
                Alert.ShowAjaxMsg("Instructions are manditory when Order Type is PRN", Page);
                return;
            }
        }

        BaseC.EMR objEMR2 = new BaseC.EMR(sConString);
        DataTable tblItem = new DataTable();
        DataView DVItem = new DataView();
        DataView dvDrugAllergy = new DataView();
        DataTable dt = new DataTable();
        DataTable tblDrugAllergy = new DataTable();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(ViewState["OrderSetId"]) > 0)
            {
                AddOrderSet(common.myStr(ViewState["ICDCodes"]));
            }
            else
            {
                endDateChange();
                if (!isSavedItem())
                {
                    return;
                }
                if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
                {
                    if (CallVariableDose(true).Equals(0))
                    {
                        return;
                    }
                }
                else
                {
                    Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                }
                dt = objEMR2.GetEMRExistingMedicationOrder(common.myInt(Session["HospitalLocationid"]), common.myInt(Session["FacilityId"]),
                                                            common.myInt(ViewState["EncId"]), common.myInt(ViewState["RegId"]),
                                                            common.myInt(hdnItemId.Value), common.myStr(Session["OPIP"]));
                if (dt.Rows.Count > 0)
                {
                    dvConfirmAlreadyExistOptions.Visible = true;
                    lblItemName.Text = common.myStr(dt.Rows[0]["ItemName"]);
                    lblEnteredBy.Text = common.myStr(dt.Rows[0]["EnteredBy"]);
                    lblEnteredOn.Text = common.myStr(dt.Rows[0]["EncodedDate"]);
                    dt.Dispose();
                    return;
                }
                bool IsAllergy = false;
                string customMedication = common.myStr(txtCustomMedication.Text).Trim();
                if (customMedication.Length.Equals(0))
                {
                    RetrievePatientAllergies();
                    if (ViewState["DrugAllergy"] != null)
                    {
                        tblDrugAllergy = (DataTable)ViewState["DrugAllergy"];
                        dvDrugAllergy = tblDrugAllergy.Copy().DefaultView;
                        if (tblDrugAllergy.Rows.Count > 0)
                        {
                            for (int j = 0; j <= tblDrugAllergy.Rows.Count - 1; j++)
                            {
                                if (!common.myStr(tblDrugAllergy.Rows[j]["ItemType"]).Equals("O"))
                                {

                                    if (common.myInt(hdnGenericId.Value) > 0)
                                    {
                                        dvDrugAllergy.RowFilter = "ISNULL(Generic_Id,0)=" + common.myInt(hdnGenericId.Value);
                                    }

                                    if (common.myInt(hdnItemId.Value) > 0 && dvDrugAllergy.ToTable().Rows.Count == 0)
                                    {
                                        dvDrugAllergy.RowFilter = "ISNULL(AllergyID,0)=" + common.myInt(hdnItemId.Value);
                                        if (dvDrugAllergy.ToTable().Rows.Count == 0)
                                        {
                                            //dvDrugAllergy = tblDrugAllergy.Copy().DefaultView;
                                            if (common.myInt(hdnGenericId.Value) > 0)
                                            {
                                                dvDrugAllergy.RowFilter = "ISNULL(Generic_Id,0)=" + common.myInt(hdnGenericId.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dvDrugAllergy.RowFilter = "ISNULL(Generic_Id,0)=" + common.myInt(hdnGenericId.Value);
                                    }
                                }

                                /*var brandValue=   common.myStr(ddlBrand.Text).Trim();
                                var genValue= common.myStr(ddlGeneric.Text).Trim();
                                var allergyName = common.myStr(tblDrugAllergy.Rows[j]["AllergyName"]).Trim().ToUpper();
                                if (brandValue.Contains(allergyName) || genValue.Contains(allergyName))
                                {
                                    IsAllergy = true;
                                }*/

                            }
                        }

                        //dvDrugAllergy.RowFilter = "ItemType<>'O'";
                        if (dvDrugAllergy.ToTable().Rows.Count > 0)
                        {
                            IsAllergy = true;
                        }
                    }
                }
                if (IsAllergy)
                {
                    lblMessage.Text = "Patient is allergic to this drug !";
                    //RadWindow1.NavigateUrl = "/EMR/Medication/MedicationOverride.aspx?" +
                    //                        "D=" + common.myStr(ddlBrand.Text).Trim() +
                    //                        "&IsO=" + common.myInt(hdnIsOverride.Value) +
                    //                        "&OC=" + common.myStr(hdnOverrideComments.Value) +
                    //                        "&DASR=" + common.myStr(hdnDrugAllergyScreeningResult.Value);
                    RadWindow1.NavigateUrl = "/EMR/Medication/MedicationOverride.aspx?" +
                                            "D=" + common.myStr(ddlBrand.Text).Trim() +
                                            "&G=" + common.myStr(ddlGeneric.Text).Trim() +
                                            "&IsO=0&OC=&DASR=";
                    RadWindow1.Height = 280;
                    RadWindow1.Width = 620;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = "OnClientMedicationOverrideClose";
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    return;
                }
                if (!common.myStr(ViewState["ProceedValue"]).Equals("F"))
                {
                    if (common.myStr(ViewState["IsAutoAddFavirateInPrescription"]).Equals("Y"))
                    {
                        btnAddtoFav_Click(null, null);
                    }
                }
                addItem(common.myStr(ViewState["ICDCodes"]));
                if (common.myStr(ViewState["ProceedValue"]).Equals("F"))
                {
                    proceedFavourite(true);
                }
                else if (common.myStr(ViewState["ProceedValue"]).Equals("C"))
                {
                    proceedCurrent(true);
                }
                lblMessage.Text = string.Empty;
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
            tblItem.Dispose();
            DVItem.Dispose();
            dvDrugAllergy.Dispose();
            dt.Dispose();
            tblDrugAllergy.Dispose();
        }
    }
    protected void ddlDoseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFrequencyId.Enabled = true;
            ddlReferanceItem.Enabled = true;
            txtDuration.Enabled = true;
            ddlPeriodType.Enabled = true;

            if (common.myInt(ddlDoseType.SelectedValue) > 0 && !common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT"))
            {
                if (ddlFrequencyId != null)
                {
                    if (ddlFrequencyId.FindItemByValue("0") == null)
                    {
                        ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                    }
                }
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));
                ddlFrequencyId.Enabled = false;
                ddlReferanceItem.Enabled = false;
                txtDuration.Enabled = false;
                ddlPeriodType.Enabled = false;
            }
            else if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT"))
            {
                ddlFrequencyId.Enabled = true;
                ddlReferanceItem.Enabled = true;
                txtDuration.Enabled = true;
                ddlPeriodType.Enabled = true;
            }
            else
            {
                // ddlFrequencyId.Items.Remove(0);
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));

                ddlFrequencyId.Enabled = true;
                ddlReferanceItem.Enabled = true;
                txtDuration.Enabled = true;
                ddlPeriodType.Enabled = true;
            }
        }
        catch
        {
        }
    }

    private void addItem(string sICDCode)
    {
        if (dvConfirm.Visible)
        {
            return;
        }

        bool TaperingDose = chkTaperingDose.Checked;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataSet ds = new DataSet();
        DataSet dsXmlData = new DataSet();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dvRemoveBlank = new DataView();
        DataView DVItem = new DataView();
        DataView dvFilter = new DataView();
        DataTable tblItem = new DataTable();
        DataTable dtDetail = new DataTable();
        DataTable dtPD = new DataTable();

        DataTable dtTempLastData = new DataTable();

        try
        {
            DataRow DR;
            DataRow DR1;
            decimal dQty = 0;
            int countRow = 0;
            if (common.myInt(hdnItemId.Value).Equals(0) && common.myInt(ddlBrand.SelectedValue) > 0)
            {
                hdnItemId.Value = common.myInt(ddlBrand.SelectedValue).ToString();
            }
            if (common.myInt(hdnItemId.Value).Equals(0)
                && common.myInt(hdnGenericId.Value).Equals(0)
                && common.myLen(txtCustomMedication.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select drug", Page);
                return;
            }
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                if (common.myStr(txtStartDate.SelectedDate) == "" && spnStartdate.Visible)
                {
                    txtStartDate.Focus();
                    Alert.ShowAjaxMsg("Please select Start Date", Page);
                    return;
                }
            }
            if (!chkCustomMedication.Checked)
            {
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    if (common.myInt(hdnItemId.Value).Equals(0) && common.myInt(hdnGenericId.Value) > 0)
                    {
                        lblMessage.Text = "Please prescribe brand for IP patients!";
                        Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
                        return;
                    }
                }

                tblItem = (DataTable)ViewState["ItemDetail"];
                if (tblItem != null)
                {
                    dvRemoveBlank = tblItem.Copy().DefaultView;
                    //dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR ISNULL(CustomMedication,False) <> True ";
                    dvRemoveBlank.RowFilter = "ISNULL(UnAppPrescriptionId,0) <> 0";

                    tblItem = dvRemoveBlank.ToTable();
                    DVItem = tblItem.Copy().DefaultView;
                    if (common.myInt(hdnItemId.Value) > 0)
                    {
                        try
                        {
                            DVItem.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " AND ISNULL(CustomMedication,0) <> 1 ";
                        }
                        catch
                        {
                            DVItem.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) + " AND ISNULL(CustomMedication,False) <> True ";
                        }
                    }
                    else
                    {
                        try
                        {
                            DVItem.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGenericId.Value) + " AND ISNULL(CustomMedication,0)<> 1 ";
                        }
                        catch
                        {
                            DVItem.RowFilter = "ISNULL(GenericId,0)=" + common.myInt(hdnGenericId.Value) + " AND ISNULL(CustomMedication,False)<> True ";
                        }
                    }

                    if (!common.myBool(ViewState["Edit"]))
                    {
                        if (DVItem.ToTable().Rows.Count > 0 && common.myInt(ddlReferanceItem.SelectedValue) < 0)
                        {
                            if (common.myStr(txtStartDate.SelectedDate) != "")
                            {
                                string rowFilter = DVItem.RowFilter;

                                DVItem.RowFilter = string.Empty;
                                DVItem = tblItem.Copy().DefaultView;

                                //DVItem.RowFilter = rowFilter + " AND StartDate <> '01/01/1900' AND ('" + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy") + "' >= StartDate AND '" + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy") + "' <= EndDate)";

                                //if (DVItem.ToTable().Rows.Count > 0)
                                //{
                                //    Alert.ShowAjaxMsg("Drug name already added", Page);
                                //    return;
                                //}
                            }
                            else
                            {
                                Alert.ShowAjaxMsg("Drug name already added", Page);
                                return;
                            }
                        }
                    }
                }
                //if (ddlBrand.SelectedValue == string.Empty || ddlBrand.SelectedValue == "0")
                //{
                //    Alert.ShowAjaxMsg("Please select drug", Page); 
                //    return;
                //}
                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
                    //{
                    //    Alert.ShowAjaxMsg("Please select Formulation", Page);
                    //    return;
                    //}
                    if (common.myStr(hdnIsRouteMandatory.Value).Equals("Y") && spnRoute.Visible)
                    {
                        if (common.myInt(ddlRoute.SelectedValue).Equals(0))
                        {
                            Alert.ShowAjaxMsg("Please select Route", Page);
                            return;
                        }
                    }
                }
            }
            if (ViewState["Item"] == null && ViewState["Edit"] == null)
            {
                dt = CreateItemTable();
                dt1 = CreateItemTable();
            }
            else
            {
                dtold = (DataTable)ViewState["Item"];
                //BaseC.clsIndent objIndent = new BaseC.clsIndent(sConString);
                //if (objIndent.IsAllergyWithPrescribingMedicine(common.myInt(ViewState["EncId"]), common.myInt(ViewState["ItemId"])))
                //{
                //    return;
                //}
                if (common.myBool(ViewState["Edit"]) && ViewState["Item"] != null)
                {
                    dv = new DataView(dtold);

                    if (chkCustomMedication.Checked)
                    {
                        dv.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
                    }
                    else
                    {
                        if (common.myInt(hdnItemId.Value) > 0)
                        {
                            dv.RowFilter = "ISNULL(ItemId,0)<>" + common.myInt(hdnItemId.Value);
                        }
                        else if (common.myInt(hdnGenericId.Value) > 0)
                        {
                            dv.RowFilter = "ISNULL(GenericId,0)<>" + common.myInt(hdnGenericId.Value);
                        }
                    }

                    dt = dv.ToTable();
                }
                else
                {
                    dt = (DataTable)ViewState["Item"];
                }
                if (dt.Rows.Count > 0)
                {
                    if (ViewState["Edit"] == null)
                    {
                        foreach (GridViewRow dataItem in gvItem.Rows)
                        {
                            TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
                            dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
                            countRow++;
                            dt.AcceptChanges();
                        }
                    }
                }
                if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
                {
                    dt1 = CreateItemTable();
                }
                else
                {
                    dt1old = (DataTable)ViewState["ItemDetail"];
                    if (common.myBool(ViewState["Edit"]) && ViewState["ItemDetail"] != null)
                    {
                        dv1 = new DataView(dt1old);

                        if (chkCustomMedication.Checked)
                        {
                            dv1.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
                        }
                        else
                        {
                            if (common.myInt(hdnItemId.Value) > 0)
                            {
                                dv1.RowFilter = "ISNULL(ItemId,0)<>" + common.myInt(hdnItemId.Value);
                            }
                            else if (common.myInt(hdnGenericId.Value) > 0)
                            {
                                dv1.RowFilter = "ISNULL(GenericId,0)<>" + common.myInt(hdnGenericId.Value);
                            }
                        }
                        dt1 = dv1.ToTable();
                    }
                    else
                    {
                        dt1 = (DataTable)ViewState["ItemDetail"];
                    }
                }
            }
            if (!chkCustomMedication.Checked)
            {
                foreach (GridViewRow row in gvItem.Rows)
                {
                    HiddenField hdnGDItemId = (HiddenField)row.FindControl("hdnItemId");

                    if (common.myInt(hdnGDItemId.Value).Equals(common.myInt(hdnItemId.Value)) && ViewState["Edit"] == null)
                    {
                        dsXmlData = new DataSet();
                        dvFilter = new DataView();
                        dtDetail = new DataTable();
                        HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
                        if (common.myInt(hdnGDItemId.Value) > 0)
                        {
                            if (common.myLen(hdnXMLData.Value) > 0)
                            {
                                StringReader sr = new StringReader(hdnXMLData.Value);
                                dsXmlData.ReadXml(sr);

                                if (!dsXmlData.Tables[0].Columns.Contains("StartDate"))
                                {
                                    dsXmlData.Tables[0].Columns.Add("StartDate", typeof(string));
                                }

                                dvFilter = new DataView(dsXmlData.Tables[0]);
                                try
                                {
                                    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " AND ISNULL(CustomMedication,False) <> True ";
                                }
                                catch
                                {
                                    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " AND ISNULL(CustomMedication,0) <> 1 ";
                                }

                                dtDetail = dvFilter.ToTable();
                                if (dtDetail.Rows.Count > 0)
                                {
                                    Telerik.Web.UI.RadDatePicker startdate = new Telerik.Web.UI.RadDatePicker();
                                    try
                                    {
                                        if (!common.myDate(dtDetail.Rows[0]["StartDate"]).ToString("dd/MM/yyyy").Equals("01/01/1900"))
                                        {
                                            startdate.SelectedDate = Convert.ToDateTime(common.myDate(dtDetail.Rows[0]["StartDate"]));
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    endDateChange();
                                    DateTime endDateForNewDose = Convert.ToDateTime(txtEndDate.SelectedDate);
                                    if (txtStartDate.SelectedDate <= endDateForNewDose)
                                    {
                                        if (common.myStr(txtStartDate.SelectedDate) != "")
                                        {
                                            string rowFilter = dvFilter.RowFilter;

                                            dvFilter.RowFilter = string.Empty;
                                            dvFilter = new DataView(dsXmlData.Tables[0]);

                                            //dvFilter.RowFilter = rowFilter + " AND StartDate <> '01/01/1900' AND ('" + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy") + "' >= StartDate AND '" + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy") + "' <= EndDate)";

                                            //if (DVItem.ToTable().Rows.Count > 0)
                                            //{
                                            //    Alert.ShowAjaxMsg("Drug name already added", Page);
                                            //    return;
                                            //}
                                        }
                                        else
                                        {
                                            Alert.ShowAjaxMsg("Drug name already added", Page);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!dt.Columns.Contains("Frequency"))
            {
                dt.Columns.Add("Frequency", typeof(decimal));
            }
            //DataView dvChkCustomMedication = dt.DefaultView;
            //try
            //{
            //    if (chkCustomMedication.Checked)
            //    {
            //        dvChkCustomMedication.RowFilter = "ISNULL(CustomMedication,False) = True";
            //if (dvChkCustomMedication.ToTable().Rows.Count > 0)
            //{
            //    if (ViewState["Edit"] == null || (!ddlBrand.Enabled && common.myLen(txtCustomMedication.Text) > 0))
            //    {
            //        Alert.ShowAjaxMsg("Multiple custom medications not allowed in one prescription!", this.Page);
            //        return;
            //    }
            //}
            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    dvChkCustomMedication.RowFilter = string.Empty;
            //    dvChkCustomMedication.Dispose();
            //}
            //Item
            DR = dt.NewRow();
            if (common.myInt(ddlIndentType.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select Indent Type", Page);
                return;
            }
            DR["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
            DR["IndentType"] = common.myInt(ddlIndentType.SelectedValue).Equals(0) ? string.Empty : common.myStr(ddlIndentType.Text);
            DR["IndentId"] = 0;
            //DR["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
            DR["GenericId"] = common.myInt(hdnGenericId.Value);
            if (common.myInt(hdnItemId.Value).Equals(0) && chkCustomMedication.Checked)
            {
                DR["ItemId"] = DBNull.Value;
            }
            else if (common.myInt(hdnItemId.Value) > 0 && !chkCustomMedication.Checked)
            {
                DR["ItemId"] = common.myInt(hdnItemId.Value);
            }
            DR["GenericName"] = common.myStr(ddlGeneric.Text);
            //DR["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : ((common.myInt(hdnItemId.Value) > 0) ? common.myStr(ddlBrand.Text) : common.myStr(ddlGeneric.Text));
            DR["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
            DR["StrengthValue"] = common.myStr(ddlStrengthValue.Text);
            DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
            DR["CustomMedication"] = chkCustomMedication.Checked;
            DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
            DR["IsInfusion"] = common.myStr(hdnInfusion.Value).Equals("1");
            DR["IsInjection"] = common.myBool(hdnIsInjection.Value);
            DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            if (ddlFormulation.SelectedItem != null)
            {
                DR["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
            }
            if (common.myStr(txtStartDate.SelectedDate) != "")
            {
                DR["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            }
            else
            {
                DR["StartDate"] = null;
            }
            if (common.myStr(ddlPeriodType.SelectedValue).Equals("C"))
            {
                DR["EndDate"] = null;
            }
            else
            {
                DR["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");
            }
            ///Item Detail
            // TextBox txtInstructions = new TextBox();
            //RadComboBox ddlFoodRelation = new RadComboBox();
            #region Item Add
            if (!dt1.Columns.Contains("DurationText"))
            {
                dt1.Columns.Add("DurationText", typeof(string));
            }
            if (!dt1.Columns.Contains("ReferanceItemName"))
            {
                dt1.Columns.Add("ReferanceItemName", typeof(string));
            }
            DR1 = dt1.NewRow();
            string sFrequency = "0";
            //RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
            //TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
            if (common.myInt(ddlFrequencyId.SelectedValue) > 0)
            {
                sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            }
            //TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
            //RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
            //RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");
            //RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");
            string sLocation = common.myStr(Request.QueryString["LOCATION"]);
            if (common.myInt(ddlUnit.SelectedValue).Equals(0) && !sLocation.ToUpper().Equals("WARD") && spnUnit.Visible)
            {
                Alert.ShowAjaxMsg("Please select unit !", this.Page);
                return;
            }
            if (Request.QueryString["DRUGORDERCODE"] == null)
            {
                //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
                //{
                //    Alert.ShowAjaxMsg("Please select Formulation", Page);
                //    return;
                //}
                if (common.myInt(ddlDoseType.SelectedValue).Equals(0))
                {
                    if (common.myInt(ddlFrequencyId.SelectedValue).Equals(0) && spnFrequency.Visible)
                    {
                        Alert.ShowAjaxMsg("Please select Frequency", Page);
                        return;
                    }
                    if (common.myInt(ddlReferanceItem.SelectedValue) <= 0)
                    {
                        if (common.myDbl(txtDose.Text).Equals(0.0) && spnDose.Visible)
                        {
                            Alert.ShowAjaxMsg("Please type Dose", Page);
                            return;
                        }
                        if ((common.myStr(txtDuration.Text).Equals(string.Empty) || common.myStr(txtDuration.Text).Equals("0")
                            || common.myStr(txtDuration.Text).Equals("0.") || common.myStr(txtDuration.Text).Equals(".0")
                            || common.myStr(txtDuration.Text).Equals(".")) && spnDuration.Visible && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("TO BE CONTINUED")
                            && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("TILL NEXT VISIT"))
                        {
                            Alert.ShowAjaxMsg("Please type Duration", Page);
                            ddlFormulation.Focus();
                            return;
                        }
                    }
                }
            }
            //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
            string Type = string.Empty;
            decimal dDuration = 0;
            switch (common.myStr(ddlPeriodType.SelectedValue))
            {
                case "N":
                    Type = common.myStr(txtDuration.Text) + " Minute(s)";
                    dDuration = 1;
                    break;
                case "H":
                    Type = common.myStr(txtDuration.Text) + " Hour(s)";
                    dDuration = 1;
                    break;
                case "D":
                    Type = common.myStr(txtDuration.Text) + " Day(s)";
                    dDuration = 1;
                    break;
                case "W":
                    Type = common.myStr(txtDuration.Text) + " Week(s)";
                    dDuration = 7;
                    break;
                case "M":
                    Type = common.myStr(txtDuration.Text) + " Month(s)";
                    dDuration = 30;
                    break;
                case "Y":
                    Type = common.myStr(txtDuration.Text) + " Year(s)";
                    dDuration = 365;
                    break;
                case "C":
                    Type = "To Be Continued";
                    break;
            }
            if (Request.QueryString["DRUGORDERCODE"] == null)
            {
                if (common.myBool(ViewState["ISCalculationRequired"]))
                {
                    decimal dose = 0;
                    if (common.myInt(ddlUnit.SelectedValue) > 0)
                    {
                        bool IsUnitCalculationRequired = common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]);

                        if (IsUnitCalculationRequired)
                        {
                            dose = common.myDec(txtDose.Text);
                        }
                        else
                        {
                            dose = 1;
                        }
                    }
                    else
                    {
                        dose = 1;
                    }
                    dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                    txtTotQty.Text = dQty.ToString("F2");
                }
                else
                {
                    dQty = 0;
                    txtTotQty.Text = dQty.ToString("F2");
                }

                DR1["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                DR1["RouteName"] = common.myStr(ddlRoute.SelectedItem.Text);
            }
            else
            {
                dQty = 0;
            }

            if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("STAT"))
            {
                DR1["Qty"] = common.myDbl(txtDose.Text).ToString("F2"); //dQty.ToString("F2");
            }
            else
            {
                DR1["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");
            }

            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("SPS") || common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
            {
                if (!common.myBool(ViewState["ISCalculationRequired"]) && !common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("STAT"))
                {
                    DR1["Qty"] = common.myDbl(txtDose.Text).ToString("F2");
                }
            }

            if (common.myDbl(DR1["Qty"]).Equals(0.0))
            {
                DR1["Qty"] = Math.Ceiling(common.myDbl(txtDose.Text)).ToString("F2");
            }
            //DR1["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
            DR1["GenericId"] = common.myInt(hdnGenericId.Value);
            DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
            DR1["GenericName"] = common.myStr(ddlGeneric.Text);
            //DR1["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : ((common.myInt(hdnItemId.Value) > 0) ? common.myStr(ddlBrand.Text) : common.myStr(ddlGeneric.Text));
            DR1["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
            DR1["StrengthValue"] = common.myStr(ddlStrengthValue.Text);
            //  DR1["Dose"] = common.myStr(common.myInt(txtDose.Text));
            DR1["Dose"] = common.myStr(common.myDbl(txtDose.Text));
            DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue).Equals(0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            if (common.myStr(ddlPeriodType.SelectedValue).Equals("C") && common.myStr(txtDuration.Text).Equals(string.Empty))
            {
                DR1["Duration"] = "1";
            }
            else
            {
                DR1["Duration"] = common.myStr(txtDuration.Text);
            }
            if (!spnDuration.Visible && common.myLen(txtDuration.Text).Equals(0))
            {
                DR1["DurationText"] = string.Empty;
                DR1["Type"] = string.Empty;
            }
            else
            {
                DR1["DurationText"] = common.myStr(Type);
                DR1["Type"] = common.myStr(ddlPeriodType.SelectedValue);
            }

            if (common.myStr(ddlPeriodType.SelectedValue).Equals("C"))
            {
                DR1["DurationText"] = common.myStr(Type);
                DR1["Type"] = common.myStr(ddlPeriodType.SelectedValue);
            }

            if (common.myStr(txtStartDate.SelectedDate) != "")
                DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            else
                DR1["StartDate"] = null;
            if (common.myStr(ddlPeriodType.SelectedValue).Equals("C"))
            {
                DR1["EndDate"] = null;
            }
            else
            {
                DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");
            }
            DR1["FoodRelationshipId"] = common.myInt(ddlFoodRelation.SelectedValue);
            DR1["FoodRelationship"] = common.myInt(ddlFoodRelation.SelectedValue).Equals(0) ? string.Empty : common.myStr(ddlFoodRelation.Text);
            DR1["UnitId"] = common.myInt(ddlUnit.SelectedValue);
            DR1["UnitName"] = common.myInt(ddlUnit.SelectedValue).Equals(0) ? string.Empty : common.myStr(ddlUnit.Text);
            DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
            DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
            DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
            DR1["PrescriptionDetail"] = string.Empty;
            DR1["CustomMedication"] = chkCustomMedication.Checked;
            DR1["Instructions"] = common.clearHTMLTags(txtInstructions.Text);
            DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
            DR1["ReferanceItemName"] = common.myInt(ddlReferanceItem.SelectedValue).Equals(-1) ? string.Empty : common.myStr(ddlReferanceItem.Text);
            DR1["DoseTypeId"] = common.myInt(ddlDoseType.SelectedValue);
            DR1["DoseTypeName"] = common.myInt(ddlDoseType.SelectedValue).Equals(0) ? string.Empty : common.myStr(ddlDoseType.Text);
            DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            if (ddlFormulation.SelectedItem != null)
            {
                DR1["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
            }

            try
            {
                if (common.myStr(hdnInfusion.Value).Equals("1") || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"])))
                {
                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    DR1["FrequencyName"] = common.myStr(ddlFrequencyId.SelectedItem.Text);
                    DR1["IsInfusion"] = true;
                    DR["IsInfusion"] = true;
                }
                else if (common.myInt(ddlDoseType.SelectedValue) > 0 && !common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT"))
                {
                    DR1["IsInfusion"] = false;
                    DR1["FrequencyId"] = 0;
                    DR1["FrequencyName"] = string.Empty;
                }
                else
                {
                    DR1["IsInfusion"] = false;
                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    DR1["FrequencyName"] = common.myStr(ddlFrequencyId.SelectedItem.Text);
                }
            }
            catch
            {
                DR1["IsInfusion"] = false;
                DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                DR1["FrequencyName"] = common.myStr(ddlFrequencyId.SelectedItem.Text);
            }
            DR1["Volume"] = common.myStr(txtVolume.Text);
            DR1["VolumeUnitId"] = common.myInt(ddlVolumeUnit.SelectedValue);
            DR1["InfusionTime"] = common.myStr(txtTimeInfusion.Text);
            DR1["TimeUnit"] = common.myStr(ddlTimeUnit.SelectedValue);
            DR1["TotalVolume"] = common.myStr(txtTotalVolumn.Text);
            if (!string.IsNullOrEmpty(common.myStr(txtFlowRateUnit.Text)))
            {
                DR1["FlowRate"] = common.myStr(txtFlowRateUnit.Text);
            }
            else
            {
                DR1["FlowRate"] = "0";
            }
            DR1["FlowRateUnit"] = common.myInt(ddlFlowRateUnit.SelectedValue);
            DR1["XMLVariableDose"] = common.myStr(hdnXmlVariableDoseString.Value).Trim();
            DR1["XMLFrequencyTime"] = common.myStr(hdnXmlFrequencyTime.Value).Trim();
            DR1["OverrideComments"] = common.myStr(txtCommentsDrugAllergy.Text);
            DR1["OverrideCommentsDrugToDrug"] = common.myStr(txtCommentsDrugToDrug.Text);
            DR1["OverrideCommentsDrugHealth"] = common.myStr(txtCommentsDrugHealth.Text);
            DR1["IsSubstituteNotAllowed"] = chkSubstituteNotAllow.Checked;
            DR1["ICDCode"] = common.myStr(txtICDCode.Text);
            DR1["NotToPharmacy"] = chkNotToPharmacy.Checked;
            DR1["ClosingBalanceQty"] = common.myInt(hdnClosingBalance.Value);

            DR1["ItemFlagName"] = common.myStr(hdnItemFlagName.Value);
            DR1["ItemFlagCode"] = common.myStr(hdnItemFlagCode.Value);

            dt1.Rows.Add(DR1);
            dt1.AcceptChanges();
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("-1"));
            //ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindByValue("-1"));
            txtInstructions.Text = string.Empty;

            #endregion

            dt1.TableName = "ItemDetail";

            dtTempLastData = dt1.Copy().Clone();
            dtTempLastData.Rows.Add(DR1.ItemArray);

            StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            dtTempLastData.WriteXml(writer);

            string xmlSchema = writer.ToString();
            xmlSchema = xmlSchema.Replace("&lt;", "<");
            xmlSchema = xmlSchema.Replace("&gt;", ">");
            DR["XMLData"] = xmlSchema;
            dtPD = dt1.Copy().Clone();
            dtPD.Rows.Add(DR1.ItemArray);

            string strPDS = objEMR.GetPrescriptionDetailStringNew(dtPD, chkFractionalDose.Checked ? "Y" : "N").Trim();

            if (common.myLen(strPDS) > 0)
            {
                if (common.myStr(txtStartDate.SelectedDate) != string.Empty
                    && !common.myStr(Session["FacilityName"]).ToUpper().Contains("BELIEVERS CHURCH"))
                {
                    DR["PrescriptionDetail"] = strPDS + ", Start Date: " + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    DR["PrescriptionDetail"] = strPDS;
                }
            }
            else if (common.myStr(txtStartDate.SelectedDate) != string.Empty
                 && !common.myStr(Session["FacilityName"]).ToUpper().Contains("BELIEVERS CHURCH"))
            {
                DR["PrescriptionDetail"] = "Start Date: " + common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            }

            DR1["PrescriptionDetail"] = common.myStr(DR["PrescriptionDetail"]);
            DR1["XMLData"] = common.myStr(DR["XMLData"]);

            if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("STAT"))
            {
                DR["Qty"] = common.myDbl(txtDose.Text).ToString("F2"); //dQty.ToString("F2");
            }
            else
            {
                DR["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");
            }

            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("SPS") || common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
            {
                if (!common.myBool(ViewState["ISCalculationRequired"]) && !common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("STAT"))
                {
                    DR["Qty"] = common.myDbl(txtDose.Text).ToString("F2");
                }
            }

            //DR["Qty"] = 0;
            dt.Rows.Add(DR);
            dt.AcceptChanges();
            try
            {
                int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(DR1);
                if (dt1 != null)
                {
                    int maxRow = dt1.Rows.Count;
                    if (maxRow > 0)
                    {
                        dt1.Rows[maxRow - 1]["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                    }
                }
            }
            catch
            {
            }
            ViewState["DataTableDetail"] = null;
            dt1 = addColumnInItemGrid(dt1);
            ViewState["ItemDetail"] = dt1;
            ViewState["Item"] = dt1;
            gvItem.DataSource = dt1;
            gvItem.DataBind();
            try
            {
                if (dt1 != null)
                {
                    if (common.myBool(ViewState["Edit"]) && dt1.Rows.Count > 0)
                    {
                        gvItem.SelectedIndex = dt1.Rows.Count - 1;
                    }
                }
            }
            catch
            {
            }
            ViewState["Edit"] = null;
            setVisiblilityInteraction();
            dvPharmacistInstruction.Visible = false;
            lnkPharmacistInstruction.Visible = false;
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            ddlStrengthValue.Enabled = true;
            if (TaperingDose)
            {
                try
                {
                    ddlFrequencyId.SelectedIndex = 0;
                    txtDose.Text = string.Empty;//"1"                    

                    DataTable tblItemDate = new DataTable();
                    tblItemDate = (DataTable)ViewState["ItemDetail"];
                    if (tblItemDate != null)
                    {
                        if (common.myInt(hdnItemId.Value) > 0)
                        {
                            DataView dvItem = tblItemDate.DefaultView;
                            dvItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value);

                            if (common.myInt(dvItem.ToTable().Rows.Count) > 0)
                            {
                                txtStartDate.SelectedDate = common.myDate(dvItem.ToTable().Rows[dvItem.ToTable().Rows.Count - 1]["EndDate"]).AddDays(1);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

                chkTaperingDose.Checked = false;
            }
            else
            {
                try
                {
                    hdnItemId.Value = "0";
                    ddlBrand.Focus();
                    ddlBrand.ClearSelection();
                    ddlBrand.Items.Clear();
                    ddlBrand.Text = string.Empty;
                    ddlBrand.Enabled = true;
                    ddlBrand.SelectedValue = null;

                    hdnGenericId.Value = "0";
                    ddlGeneric.Items.Clear();
                    ddlGeneric.Text = string.Empty;
                    ddlGeneric.Enabled = true;
                    ddlGeneric.SelectedValue = null;

                    txtCustomMedication.Text = string.Empty;
                }
                catch
                { }
                ddlStrengthValue.ClearSelection();
                ddlStrengthValue.Text = string.Empty;
                txtInstructions.Text = string.Empty;
                ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
                //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue("0"));
                //ddlFoodRelation.Text = string.Empty;
                chkNotToPharmacy.Checked = false;
                ddlFrequencyId.Enabled = true;
                ddlReferanceItem.Enabled = true;
                txtDuration.Enabled = true;

                ddlPeriodType.Enabled = true;

                ddlFrequencyId.SelectedIndex = 0;
                txtDose.Text = string.Empty;//"1"
                txtDuration.Text = string.Empty;
                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));
                //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue("0"));
                txtInstructions.Text = string.Empty;
                ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("-1"));
                //ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindByValue("-1"));
                ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
                //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue("0"));
                ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue("0"));
                //ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindByValue("0"));
                txtVolume.Text = string.Empty;
                txtTotalVolumn.Text = string.Empty;
                txtTimeInfusion.Text = string.Empty;
                txtFlowRateUnit.Text = string.Empty;
                ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue("0"));
                //ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindByValue("0"));
                ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue("0"));
                //ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindByValue("0"));
                ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue("0"));
                //ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindByValue("0"));
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue("0"));
                //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue("0"));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
                //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue("0"));
                txtStartDate.SelectedDate = null;
                txtEndDate.SelectedDate = null;
                ViewState["Edit"] = null;
                txtCustomMedication.Text = string.Empty;
                hdnXmlVariableDoseString.Value = string.Empty;
                hdnvariableDoseDuration.Value = string.Empty;
                hdnvariableDoseFrequency.Value = string.Empty;
                hdnVariabledose.Value = string.Empty;
                hdnXmlFrequencyTime.Value = string.Empty;
                txtCommentsDrugAllergy.Text = string.Empty;
                txtCommentsDrugToDrug.Text = string.Empty;
                txtCommentsDrugHealth.Text = string.Empty;
                lblGenericName.Text = string.Empty;
                chkSubstituteNotAllow.Checked = false;
                //txtICDCode.Text = string.Empty;
                txtTotQty.Text = string.Empty;
                txtSpecialInstrucation.Text = string.Empty;
                btnCopyLastPrescription.Enabled = false;

                //if (txtStartDate.SelectedDate == null)
                //{
                //if (spnStartdate.Visible)
                //{
                //Awadhesh
                //txtStartDate.SelectedDate = DateTime.Now.Date;
                //
                //}
                //}
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
            dtold.Dispose();
            dt1old.Dispose();
            dt.Dispose();
            dt1.Dispose();
            ds.Dispose();
            dsXmlData.Dispose();
            objEMR = null;
            dv.Dispose();
            dv1.Dispose();
            dvRemoveBlank.Dispose();
            DVItem.Dispose();
            dvFilter.Dispose();
            if (tblItem != null)
            {
                tblItem.Dispose();
            }
            dtDetail.Dispose();
            dtPD.Dispose();
            dtTempLastData.Dispose();
        }
    }



    protected void chkCustomMedication_OnCheckedChanged(object sender, EventArgs e)
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);  //
        if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]),
            common.myInt(Session["FacilityId"]), "IsAuthorizedToCustomMedicationForEMR")) == true)
        {
            setCustomMedicationChange();

        }
        else
        {
            chkCustomMedication.Checked = false;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "You are not authorized to fill custom medicines.";

        }
        objSecurity = null;


    }
    private void setCustomMedicationChange()
    {
        try
        {
            if (chkCustomMedication.Checked)
            {
                chkNotToPharmacy.Visible = false;
                txtCustomMedication.Text = string.Empty;
                btnAddtoFav.Visible = false;
            }
            else
            {
                hdnGenericId.Value = "0";
                hdnGenericName.Value = string.Empty;
                ddlGeneric.Text = string.Empty;
                ddlGeneric.SelectedValue = null;
                hdnItemId.Value = "0";
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;

                hdnCIMSItemId.Value = string.Empty;
                hdnCIMSType.Value = string.Empty;
                hdnVIDALItemId.Value = string.Empty;

                ViewState["ISCalculationRequired"] = null;
                chkNotToPharmacy.Visible = true;
                btnAddtoFav.Visible = true;
            }
            trGeneric.Visible = !chkCustomMedication.Checked;
            trDrugs.Visible = !chkCustomMedication.Checked;
            trCustomMedication.Visible = chkCustomMedication.Checked;
        }
        catch
        {
        }
    }
    private void SetStrengthFormUnitRouteOfSelectedDrug()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.getGenericRouteUnitOfSelectedDrug(common.myInt(hdnItemId.Value), common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(Session["Facilityid"]));
            //ddlGeneric.Items.Add(new RadComboBoxItem(ds.Tables[0].Rows[0][""].ToString
            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (!common.myInt(ds.Tables[0].Rows[0]["StrengthId"]).Equals(0))
                //{
                //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["StrengthId"]).ToString()));
                //ddlStrength.Enabled = false;
                ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(ds.Tables[0].Rows[0]["StrengthValue"])));
                ddlStrengthValue.Text = common.myStr(ds.Tables[0].Rows[0]["StrengthValue"]);
                ddlStrengthValue.Enabled = true;
                //}
                //else
                //{
                //    //ddlStrength.SelectedIndex = 0;
                //    //ddlStrength.Enabled = true;
                //}
                if (!common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).Equals(0))
                {
                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).ToString()));
                    //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).ToString()));
                    //ddlFormulation.Enabled = false;
                }
                else
                {
                    ddlFormulation.SelectedIndex = 0;
                    ddlFormulation.Enabled = true;
                }
                if (!common.myInt(ds.Tables[0].Rows[0]["RouteId"]).Equals(0))
                {
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["RouteId"]).ToString()));
                    //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(ds.Tables[0].Rows[0]["RouteId"]).ToString()));
                    //ddlRoute.Enabled = false;
                }
                else
                {
                    ddlRoute.SelectedIndex = 0;
                    ddlRoute.Enabled = true;
                }
                if (!common.myInt(ds.Tables[0].Rows[0]["ItemFrequencyId"]).Equals(0))
                {
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["ItemFrequencyId"]).ToString()));
                    //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(ds.Tables[0].Rows[0]["ItemFrequencyId"]).ToString()));
                    //ddlFrequencyId.Enabled = false;
                }
                else
                {
                    ddlFrequencyId.SelectedIndex = 0;
                    ddlFrequencyId.Enabled = true;
                }
                if (!common.myInt(ds.Tables[0].Rows[0]["ItemUnitId"]).Equals(0))
                {
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["ItemUnitId"]).ToString()));
                    //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue(common.myInt(ds.Tables[0].Rows[0]["ItemUnitId"]).ToString()));
                    //ddlUnit.Enabled = false;
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                    ddlUnit.Enabled = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlRoute.DataSource = ds.Tables[1];
                    ddlRoute.DataValueField = "Id";
                    ddlRoute.DataTextField = "RouteName";
                    ddlRoute.DataBind();
                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                    //ddlRoute.Items.Insert(0, new ListItem(string.Empty, "0"));
                    ddlRoute.SelectedIndex = 0;
                }
            }
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[2].Rows.Count > 0)
            //    {
            //        ViewState["UnitMaster"] = ds.Tables[2];
            //    }
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
            ds.Dispose();
            objEMR = null;
        }
    }
    protected void btnGetInfoGeneric_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            lblMessage.Text = string.Empty;
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            ddlStrengthValue.Enabled = true;
            int GenericId = common.myInt(hdnGenericId.Value);
            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);

            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }

            ds = objPhr.getGenericAttributes(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnStoreId.Value),
                                             GenericId, common.myInt(Session["UserId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myInt(ds.Tables[0].Rows[0]["ClosingBalance"]).Equals(0) && !common.myStr(Session["OPIP"]).Equals("E"))
                {
                    if (!common.myStr(ViewState["IsRemovePrescriptionPopUpMsgForItemsNotInStock"]).Equals("Y"))
                    {
                        string strMsg = "Stock of the item not available. Delivery of the item will be delayed.";
                        if (!common.myStr(ViewState["PrescriptionAllowForItemsNotInStock"]).Equals("Y"))
                        {
                            ddlBrand.Text = string.Empty;
                            hdnItemId.Value = string.Empty;
                            hdnCIMSItemId.Value = string.Empty;
                            hdnCIMSType.Value = string.Empty;
                            hdnVIDALItemId.Value = string.Empty;
                            clearItemDetails();
                            strMsg = "Stock of the item not available. You can not prescribe the item.";
                            Alert.ShowAjaxMsg(strMsg, this.Page);
                        }
                        else
                        {
                            if (common.myStr(ViewState["PrescriptionAllowForItemsNotInStockShowAlert"]).Equals("Y"))
                            {
                                Alert.ShowAjaxMsg(strMsg, this.Page);
                            }
                        }
                    }
                }
                ViewState["ISCalculationRequired"] = "0";

                ddlFormulation.Items.Clear();
                ddlFormulation.ClearSelection();

                foreach (DataRow DR in ds.Tables[1].Rows)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = common.myStr(DR["FormulationName"]);
                    item.Value = common.myStr(common.myInt(DR["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(DR["DefaultRouteId"]));

                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlFormulation.SelectedIndex = 0;

                //------------------------
                ddlStrengthValue.Items.Clear();
                ddlStrengthValue.ClearSelection();
                ddlStrengthValue.Text = string.Empty;

                ddlStrengthValue.DataSource = ds.Tables[2];
                ddlStrengthValue.DataTextField = "StrengthValue";
                ddlStrengthValue.DataTextField = "StrengthValue";
                ddlStrengthValue.DataBind();

                ddlStrengthValue.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlStrengthValue.SelectedIndex = 0;

                if (common.myDbl(ds.Tables[0].Rows[0]["DrugDose"]) > 0)
                    txtDose.Text = common.myDbl(ds.Tables[0].Rows[0]["DrugDose"]).ToString("0.##");
                else
                    txtDose.Text = string.Empty;

                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["ItemUnitId"]).ToString()));
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).ToString()));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["RouteId"]).ToString()));

            }

            if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    if (common.myLen(hdnCIMSItemId.Value) < 2)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "This Drug is not mapped with CIMS.";
                        return;
                    }
                    chkIsInteraction(0);
                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "This Drug is not mapped with VIDAL.";
                        return;
                    }
                }
            }

            if (!ddlBrand.Visible)
            {
                txtDose.Focus();
            }
            else
            {
                ddlBrand.Focus();

                //ddlBrand.ZIndex = 50000;
                //ddlBrand.ClearSelection();
                //ddlBrand.Items.Clear();

                //ddlBrand.OpenDropDownOnLoad = true;
                //RadComboBoxItemsRequestedEventArgs Arg = new RadComboBoxItemsRequestedEventArgs();
                //Arg.Text = "%%";
                //Arg.Context["GenericId"] = (common.myInt(hdnGenericId.Value) > 0) ? common.myStr(hdnGenericId.Value) : (ViewState["Genericid"] != null) ? common.myStr(ViewState["Genericid"]) : "";
                //ddlBrand_OnItemsRequested(null, Arg);
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
            objPhr = null;
        }
    }
    protected void btnProceed_OnClick(object sender, EventArgs e)
    {
        ViewState["Yes"] = true;
        btnAddItem_OnClick(this, null);
        divConfirmation.Visible = false;
        ViewState["Yes"] = null;

    }
    protected void btnProceedCancel_OnClick(object sender, EventArgs e)
    {
        divConfirmation.Visible = false;
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        int ItemId = 0;
        try
        {
            lblMessage.Text = string.Empty;

            ItemId = common.myInt(hdnItemId.Value);
            //if (common.myInt(ItemId).Equals(0))
            //{
            //    hdnItemId.Value = common.myInt(ddlBrand.SelectedValue).ToString();
            //    ItemId = common.myInt(hdnItemId.Value);
            //}


            if (ItemId.Equals(0))
            {
                try
                {
                    ddlBrand.ClearSelection();
                    ddlBrand.Items.Clear();
                    ddlBrand.Text = string.Empty;
                    ddlBrand.SelectedValue = null;
                    return;
                }
                catch
                {
                }
            }

            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);
            clearItemDetails();
            BindICDPanel();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            ddlStrengthValue.Enabled = true;
            int iOT = 3;
            if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim().Equals("OT")
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim().Equals("CO"))
            {
                iOT = 2;
            }
            else if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim().Equals("WARD")
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim().Equals("CO"))
            {
                iOT = 2;
            }
            else
            {
                iOT = 1;
            }
            //SetStrengthFormUnitRouteOfSelectedDrug();
            if (ItemId > 0)
            {
                hdnGenericId.Value = "0";
                lblGenericName.Text = string.Empty;

                int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
                if (DoctorId.Equals(0))
                {
                    DoctorId = common.myInt(Session["LoginDoctorId"]);
                }
                if (DoctorId.Equals(0))
                {
                    DoctorId = common.myInt(Session["EmployeeId"]);
                }

                //ds = objPharmacy.getItemMaster(ItemId, string.Empty, string.Empty, 1, common.myInt(Session["HospitalLocationID"]),
                //                                common.myInt(Session["UserId"]), iOT, common.myInt(hdnStoreId.Value), common.myInt(Session["FacilityId"]));
                ds = objPhr.getItemAttributes(common.myInt(Session["HospitalLocationID"]), ItemId, common.myInt(hdnStoreId.Value),
                                            common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), DoctorId, common.myInt(Session["CompanyId"]), 0);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];
                    if (common.myBool(ds.Tables[0].Rows[0]["IsBlocked"]))
                    {
                        ViewState["BlockItemId"] = ItemId;
                        if (common.myBool(ViewState["IsAllowToAddBlockedItem"]) == false)
                        {
                            Alert.ShowAjaxMsg("Not authorized to Prescribe this Item. Selected Item is blocked for this company.", this.Page);
                            return;
                        }
                    }
                    else
                    {
                        ViewState["BlockItemId"] = null;
                    }


                    //comment for not set unit and route based on formulation
                    hdnGenericId.Value = common.myInt(DR["GenericId"]).ToString();
                    lblGenericName.Text = common.myStr(DR["GenericName"]);

                    //palendra change 

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));

                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["ItemUnitId"]).ToString()));
                    bindOnLoadDataBrandBase(common.myInt(DR["FormulationId"]), common.myInt(DR["RouteId"]), common.myInt(DR["ItemUnitId"]));

                    ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(DR["StrengthValue"])));
                    ddlStrengthValue.Text = common.myStr(DR["StrengthValue"]);
                    ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
                    ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);
                    hdnInfusion.Value = common.myStr(DR["IsInfusion"]);
                    hdnIsInjection.Value = common.myBool(DR["IsInjection"]).ToString();

                    //ddlFormulation_OnSelectedIndexChanged(this, null);

                    if (common.myInt(ddlFormulation.SelectedValue) > 0 && common.myInt(ddlRoute.SelectedValue).Equals(0))
                    {
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ddlFormulation.SelectedItem.Attributes["DefaultRouteId"]).ToString()));
                    }
                    if (common.myInt(ddlRoute.SelectedValue).Equals(0))
                    {
                        ddlRoute.Text = string.Empty;
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
                    }
                    if (common.myDbl(DR["Dose"]) > 0)
                    {
                        if (common.myStr(ViewState["EMRPrescriptionDoseCalculateBasedOnWeight"]).Equals("Y")
                            && common.myDbl(ViewState["PatientWeight"]) > 0.0)
                        {
                            txtDose.Text = common.myDbl(common.myDbl(DR["Dose"]) * common.myDbl(ViewState["PatientWeight"])).ToString("F2");
                        }
                        else
                        {
                            txtDose.Text = common.myDbl(DR["Dose"]).ToString();
                        }
                    }
                    else
                    {
                        txtDose.Text = string.Empty;//"1"
                    }

                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DR["FrequencyId"]).ToString()));

                    dvPharmacistInstruction.Visible = false;
                    if (common.myStr(DR["FieldCode"]).Equals("INSPH"))
                    {
                        if (common.myLen(DR["ValueText"]) > 0)
                        {
                            lnkPharmacistInstruction.Visible = true;
                            txtPharmacistInstruction.Text = common.myStr(DR["ValueText"]);
                            //txtInstructions.Text = common.myStr(DR["ValueText"]);
                        }
                    }
                    else
                    {
                        lnkPharmacistInstruction.Visible = false;
                        txtPharmacistInstruction.Text = string.Empty;
                        //txtInstructions.Text = "";
                    }

                    if (common.myInt(DR["Duration"]) > 0)
                    {
                        txtDuration.Text = common.myInt(DR["Duration"]).ToString();
                    }
                    else
                    {
                        txtDuration.Text = string.Empty;//"1"
                    }

                    if (common.myLen(DR["DurationType"]) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DR["DurationType"]).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                    }

                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DR["FoodRelationshipId"]).ToString()));

                    //ddlStrength.Enabled = false;
                    //txtStrengthValue.Enabled = false;
                    if (common.myBool(DR["IsInfusion"]) || common.myBool(DR["IsInjection"]))
                    {
                        Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                        item.Text = common.myStr(DR["ItemName"]);
                        item.Value = common.myInt(DR["ItemId"]).ToString();
                        if (common.myBool(DR["IsInfusion"]))
                        {
                            item.Attributes.Add("IsInfusion", common.myStr(DR["IsInfusion"]));
                        }
                        else if (common.myBool(DR["IsInjection"]))
                        {
                            item.Attributes.Add("IsInjection", common.myStr(DR["IsInjection"]));
                        }
                        item.DataBind();
                        ddlReferanceItem.Items.Add(item);
                    }
                    txtDose.Focus();
                    //calcTotalQty();
                    if (common.myInt(DR["ClosingBalance"]).Equals(0) && !common.myStr(Session["OPIP"]).Equals("E"))
                    {
                        if (common.myStr(ViewState["IsRemovePrescriptionPopUpMsgForItemsNotInStock"]).Equals("Y"))
                        {

                        }
                        else
                        {
                            string strMsg = "Stock of the item not available. Delivery of the item will be delayed.";
                            if (!common.myStr(ViewState["PrescriptionAllowForItemsNotInStock"]).Equals("Y"))
                            {
                                ddlBrand.Text = string.Empty;
                                hdnItemId.Value = string.Empty;
                                hdnCIMSItemId.Value = string.Empty;
                                hdnCIMSType.Value = string.Empty;
                                hdnVIDALItemId.Value = string.Empty;
                                clearItemDetails();
                                strMsg = "Stock of the item not available. You can not prescribe the item.";
                                Alert.ShowAjaxMsg(strMsg, this.Page);
                            }
                            else
                            {
                                if (common.myStr(ViewState["PrescriptionAllowForItemsNotInStockShowAlert"]).Equals("Y"))
                                {
                                    Alert.ShowAjaxMsg(strMsg, this.Page);
                                }
                            }
                        }
                    }
                    ViewState["ISCalculationRequired"] = common.myBool(DR["CalculationRequired"]);

                    //ViewState["HighValueItem"] = DR["HighValueItem"];
                    if (common.myBool(DR["HighValueItem"]) || common.myBool(DR["ExpensiveValueItem"]) || common.myBool(DR["LasaValueItem"]))
                    {
                        //string strMsg = "This is high value item do you want to proceed";
                        //Alert.ShowAjaxMsg(strMsg, this.Page);
                        //clearItemDetails();

                        dvConfirm.Visible = true;
                        string strConfirmMessage = string.Empty; //This is high value item do you want to proceed

                        if (common.myBool(DR["HighValueItem"]))
                        {
                            strConfirmMessage += "High risk item";
                        }
                        if (common.myBool(DR["ExpensiveValueItem"]))
                        {
                            if (common.myLen(strConfirmMessage) > 0)
                            {
                                strConfirmMessage += "<br/>";
                            }
                            strConfirmMessage += "High value item";
                        }
                        if (common.myBool(DR["LasaValueItem"]))
                        {
                            if (common.myLen(strConfirmMessage) > 0)
                            {
                                strConfirmMessage += "<br/>";
                            }
                            strConfirmMessage += "LASA item";
                        }

                        lblConfirmHighValue.Text = strConfirmMessage + "<br/>Do you want to proceed!";
                    }


                    try
                    {
                        DataTable tblItem = new DataTable();
                        tblItem = (DataTable)ViewState["ItemDetail"];
                        if (tblItem != null)
                        {
                            if (common.myInt(ItemId) > 0)
                            {
                                DataView dvItem = tblItem.DefaultView;
                                dvItem.RowFilter = "ItemId=" + ItemId;

                                if (common.myInt(dvItem.ToTable().Rows.Count) > 0)
                                {
                                    //txtStartDate.SelectedDate = common.myDate(dvItem.ToTable().Compute("Max(EndDate)", string.Empty)).AddDays(1);
                                    txtStartDate.SelectedDate = common.myDate(dvItem.ToTable().Rows[dvItem.ToTable().Rows.Count - 1]["EndDate"]).AddDays(1);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        if (common.myLen(hdnCIMSItemId.Value) < 2)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "This Drug is not mapped with CIMS.";
                            return;
                        }
                        chkIsInteraction(ItemId);
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "This Drug is not mapped with VIDAL.";
                            return;
                        }
                    }
                }
                //setDefaultFavourite(ItemId);
                //DataSet dsCalisreq = objPharmacy.ISCalculationRequired(common.myInt(hdnItemId.Value));
                //if (dsCalisreq.Tables[0].Rows.Count > 0)
                //{
                //    ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
                //}


            }

            CheckgenerateInstruction(ItemId);
            //BindBlankItemDetailGrid();
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
            objPhr = null;
        }
    }

    protected void lnkStopMedication_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sRegId = common.myStr(ViewState["RegId"]);
            string sEncId = common.myStr(ViewState["EncId"]);
            ViewState["Mode"] = "S";
            RadWindow1.NavigateUrl = "/EMR/Medication/StopMedication.aspx?RegId=" + sRegId + "&EncId=" + sEncId;
            RadWindow1.Height = 600;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch
        {
        }
    }

    protected void lnkCompoundedDrugOrder_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sRegId = common.myStr(ViewState["RegId"]);
            string sEncId = common.myStr(ViewState["EncId"]);
            ViewState["Mode"] = "S";
            RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationCM.aspx?MASTER=NO";
            RadWindow1.Height = 600;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = string.Empty; //"OnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch
        {
        }
    }

    protected void lnkLabHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(ViewState["EncId"]) + "&RegNo=" + common.myStr(ViewState["Regno"]);
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnClose_OnClick(object sender, EventArgs e)
    {


        hdnCtrlValue.Value = fetchPrescriptionData();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        return;
    }
    private string fetchPrescriptionData()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataTable dtEnterByItem = new DataTable();
        DataView dvEnterByItem = new DataView();
        DataView dvFilter = new DataView();
        try
        {
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ds = objPhr.getPreviousMedicines(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncId"]),
                                        0, 0, "A", string.Empty, string.Empty, string.Empty);//uspGetPreviousMedicinesIP
            }
            else
            {
                ds = objEMR.getOPMedicines(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncId"]),
                                     common.myInt(ViewState["RegId"]), 0, 0, "A", string.Empty, string.Empty, string.Empty);//uspGetOPMedicines
            }
            dv = new DataView(ds.Tables[0]);
            dv1 = new DataView(ds.Tables[1]);
            //dv.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED','NUT')";
            dv.RowFilter = "ISNULL(DoctorCategory,0)=1 ";
            if (dv.ToTable() != null)
            {
                if (dv.ToTable().Rows.Count > 0)
                {
                    sb.Append("Medications Prescribed:");
                    dtEnterByItem = dv.ToTable(true, "EncodedBy");
                    foreach (DataRow row in dtEnterByItem.Rows)
                    {
                        dvEnterByItem = new DataView(dv.ToTable());
                        dvEnterByItem.RowFilter = "EncodedBy=" + common.myStr(row["EncodedBy"]);
                        foreach (DataRowView dr in dvEnterByItem)
                        {
                            sbPrescribed.Append(Environment.NewLine + common.myStr(dr["ItemName"]) + " : ");
                            dvFilter = new DataView(dv1.ToTable());
                            if (common.myBool(dr["CustomMedication"]))
                            {
                                //dvFilter.RowFilter = "ISNULL(DetailsId,0)=" + common.myInt(dr["DetailsId"]).ToString() + " AND IndentId =" + common.myInt(dr["IndentId"]).ToString();
                                dvFilter.RowFilter = "ISNULL(IndentId,0) =" + common.myInt(dr["IndentId"]);
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(dr["ItemId"]) + " AND ISNULL(IndentId,0) =" + common.myInt(dr["IndentId"]);
                            }
                            string sPrescriptionString = objEMR.GetPrescriptionDetailStringNew(dvFilter.ToTable(), chkFractionalDose.Checked ? "Y" : "N");
                            sbPrescribed.Append(sPrescriptionString);
                        }
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
            objEMR = null;
            objPhr = null;
            ds.Dispose();
            dv = null;
            dv1 = null;
            dtEnterByItem = null;
            dvEnterByItem = null;
            dvFilter.Dispose();
        }
        sb.Append(sbPrescribed.ToString());
        return sb.ToString();
    }
    #region Favourite
    protected void btnAddtoFav_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        int ItemId = 0;
        int GenericId = 0;
        try
        {
            ItemId = common.myInt(hdnItemId.Value);
            if (ItemId == 0)
            {
                ItemId = common.myInt(ddlBrand.SelectedValue);
            }

            if (ItemId > 0)
            {
                GenericId = 0;
            }
            else
            {
                GenericId = common.myInt(hdnGenericId.Value);
                if (GenericId == 0)
                {
                    GenericId = common.myInt(ddlGeneric.SelectedValue);
                }
            }

            if (!chkCustomMedication.Checked && GenericId.Equals(0) && ItemId.Equals(0))
            {
                Alert.ShowAjaxMsg("Please select generic or drug to save favourite", Page);
                return;
            }

            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }
            if (DoctorId.Equals(0))
            {
                return;
            }
            string strMsg = objEMR.SaveFavouriteDrugs(DoctorId, ItemId, string.Empty, common.myInt(Session["UserID"]),
                                    common.myDbl(txtDose.Text), common.myInt(ddlUnit.SelectedValue), 0,
                                    common.myInt(ddlFormulation.SelectedValue), common.myInt(ddlRoute.SelectedValue),
                                    common.myInt(ddlFrequencyId.SelectedValue), common.myInt(txtDuration.Text),
                                    common.myStr(ddlPeriodType.SelectedValue), common.myInt(ddlFoodRelation.SelectedValue),
                                    common.myStr(ddlStrengthValue.Text), common.myStr(txtInstructions.Text), GenericId);

            lblMessage.Text = strMsg;
            GetBrandFavouriteData(string.Empty);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }
    protected void lstFavourite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lstFavourite.PageIndex = e.NewPageIndex;

        GetBrandFavouriteData(common.myStr(txtFavouriteItemName.Text));

    }

    private void GetBrandFavouriteData(string ItemName)
    {
        if (!common.myStr(rdoSearchMedication.SelectedValue).Equals("F"))
        {
            return;
        }
        int GenericId = 0;
        int ItemId = 0;
        int DoctorId = common.myInt(Request.QueryString["DoctorId"]);

        ItemName = ItemName.Replace("'", "''");

        string strSearchCriteria = string.Empty;
        strSearchCriteria = "%" + ItemName + "%";

        if (DoctorId.Equals(0))
        {
            DoctorId = common.myInt(Session["LoginDoctorId"]);
        }
        if (DoctorId.Equals(0))
        {
            DoctorId = common.myInt(Session["EmployeeId"]);
        }
        DataSet dsSearch = new DataSet();
        DataView dv = new DataView();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
                                GenericId, DoctorId, string.Empty, strSearchCriteria);

            dv = dsSearch.Tables[0].Copy().DefaultView;

            if (common.myStr(ViewState["EMRIsHideBrandInPrescription"]).Equals("Y"))
            {
                if (common.myStr(Session["OPIP"]).Equals("O"))
                {
                    dv.RowFilter = "ISNULL(GenericId,0)>0 ";
                }
            }

            if (dv.ToTable().Rows.Count > 0)
            {
                lstFavourite.DataSource = dv.ToTable();
                lstFavourite.DataBind();
            }
            else
            {
                BlankFavouriteGrid();
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
            dsSearch.Dispose();
            dv.Dispose();
            objEMR = null;
        }
    }
    private void BlankFavouriteGrid()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("ItemNameWithAttributes", typeof(string));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("Dose", typeof(decimal));
        dt.Columns.Add("UnitId", typeof(int));
        dt.Columns.Add("StrengthId", typeof(int));
        dt.Columns.Add("StrengthValue", typeof(string));
        dt.Columns.Add("FormulationId", typeof(int));
        dt.Columns.Add("RouteId", typeof(int));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Duration", typeof(int));
        dt.Columns.Add("DurationType", typeof(string));
        dt.Columns.Add("FoodRelationshipId", typeof(int));
        dt.Columns.Add("CIMSItemId", typeof(string));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(string));
        dt.Columns.Add("Instructions", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("Prescription"); // Tirathram_Akshay_05082022
        DataRow dr = dt.NewRow();
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["ItemNameWithAttributes"] = string.Empty;
        dr["ItemName"] = string.Empty;
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        lstFavourite.DataSource = dt;
        lstFavourite.DataBind();
    }
    #endregion
    protected void btnSearchFavourite_OnClick(object sender, EventArgs e)
    {
        try
        {
            GetBrandFavouriteData(common.myStr(txtFavouriteItemName.Text));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSearchCurrent_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindGrid(string.Empty, string.Empty, string.Empty);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSearchOrderSet_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindOrderSet();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lstFavourite_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        int ItemId = 0;
        int GenericId = 0;
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECTITEM"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnGridItemId = (HiddenField)row.FindControl("hdnItemId");
                ItemId = common.myInt(hdnGridItemId.Value);
                HiddenField hdnGridGenericId = (HiddenField)row.FindControl("hdnGenericId");
                GenericId = common.myInt(hdnGridGenericId.Value);

                if (ItemId > 0 || GenericId > 0)
                {
                    chkCustomMedication.Checked = false;
                    setCustomMedicationChange();

                    //LinkButton lnkItemName = (LinkButton)row.FindControl("lnkItemName");
                    HiddenField hdnFavItemName = (HiddenField)row.FindControl("hdnFavItemName");

                    HiddenField hdnFCIMSItemId = (HiddenField)row.FindControl("hdnFCIMSItemId");
                    HiddenField hdnFCIMSType = (HiddenField)row.FindControl("hdnFCIMSType");
                    HiddenField hdnFVIDALItemId = (HiddenField)row.FindControl("hdnFVIDALItemId");
                    HiddenField hdnInstructions = (HiddenField)row.FindControl("hdnInstructions");
                    hdnCIMSItemId.Value = common.myStr(hdnFCIMSItemId.Value);
                    hdnCIMSType.Value = common.myStr(hdnFCIMSType.Value);
                    hdnVIDALItemId.Value = common.myStr(hdnFVIDALItemId.Value);
                    try
                    {
                        if (ItemId > 0)
                        {
                            ddlBrand.Text = common.myStr(hdnFavItemName.Value);
                            ddlBrand.SelectedValue = ItemId.ToString();
                            hdnItemId.Value = ItemId.ToString();
                            btnGetInfo_Click(null, null);
                        }
                        else if (GenericId > 0)
                        {
                            ddlGeneric.Text = common.myStr(hdnFavItemName.Value);
                            ddlBrand.SelectedValue = ItemId.ToString();
                            hdnGenericId.Value = GenericId.ToString();
                            btnGetInfoGeneric_Click(null, null);
                        }
                    }
                    catch
                    { }

                    HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                    HiddenField hdnUnitId = (HiddenField)row.FindControl("hdnUnitId");
                    HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
                    HiddenField hdnStrengthValue = (HiddenField)row.FindControl("hdnStrengthValue");
                    HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                    HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                    HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                    HiddenField hdnDuration = (HiddenField)row.FindControl("hdnDuration");
                    HiddenField hdnDurationType = (HiddenField)row.FindControl("hdnDurationType");
                    HiddenField hdnFoodRelationshipId = (HiddenField)row.FindControl("hdnFoodRelationshipId");
                    //comment for not set unit and route based on formulation
                    //SetStrengthFormUnitRouteOfSelectedDrug();

                    checkitemquintity(ItemId);

                    if (common.myDbl(hdnDose.Value) > 0)
                    {
                        //  txtDose.Text = common.myInt(hdnDose.Value).ToString();
                        txtDose.Text = common.myDbl(hdnDose.Value).ToString();
                    }
                    else
                    {
                        txtDose.Text = string.Empty;//"1"
                    }
                    if (common.myInt(hdnDuration.Value) > 0)
                    {
                        txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                    }
                    else
                    {
                        txtDuration.Text = string.Empty;//"1"
                    }
                    if (common.myLen(hdnDurationType.Value) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                    }
                    txtInstructions.Text = common.myStr(hdnInstructions.Value);
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                    //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue(common.myInt(hdnUnitId.Value).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(hdnStrengthId.Value).ToString()));

                    ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(hdnStrengthValue.Value));
                    ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));
                    //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(hdnFormulationId.Value).ToString()));
                    ddlFormulation_OnSelectedIndexChanged(this, null);
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                    //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(hdnRouteId.Value).ToString()));
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                    //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                    if (common.myInt(hdnDuration.Value) > 0)
                    {
                        txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                    }
                    else
                    {
                        txtDuration.Text = string.Empty;//"1"
                    }
                    if (common.myLen(hdnDurationType.Value) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                        //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue(common.myStr(hdnDurationType.Value).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                        //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
                    }
                    endDateChange();
                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));
                    //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));
                    if (common.myDbl(txtDose.Text).Equals(0.0))
                    {
                        txtDose.Text = string.Empty;//"1"
                    }
                    if (common.myStr(Session["OPIP"]).Equals("I"))
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            txtDuration.Text = string.Empty;//"1"
                        }
                    }
                    if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        if (common.myBool(Session["IsCIMSInterfaceActive"]))
                        {
                            if (common.myLen(hdnCIMSItemId.Value) < 2)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "This Drug is not mapped with CIMS.";
                            }
                            else
                            {
                                chkIsInteraction(ItemId);
                            }
                        }
                        else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                        {
                            if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "This Drug is not mapped with VIDAL.";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName.ToUpper().Equals("ITEMDELETE"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnGridItemId = (HiddenField)row.FindControl("hdnItemId");
                HiddenField hdnGridGenericId = (HiddenField)row.FindControl("hdnGenericId");

                ItemId = common.myInt(hdnGridItemId.Value);
                GenericId = common.myInt(hdnGridGenericId.Value);

                if (ItemId > 0)
                {
                    GenericId = 0;
                }

                int doctorid = Session["OPIP"] != null && common.myStr(Session["OPIP"]).Trim().Equals("I") ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["LoginDoctorId"]);
                if (doctorid.Equals(0))
                {
                    doctorid = common.myInt(Session["LoginDoctorId"]);
                }

                string sResult = objEMR.DeleteFavoriteDrugs(doctorid, ItemId, string.Empty, common.myInt(Session["UserId"]), GenericId);
                lblMessage.Text = "Favourite Deleted";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                GetBrandFavouriteData(string.Empty);
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
            objEMR = null;
        }
    }

    public void checkitemquintity(int ItemId)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        hdnItemId.Value = common.myInt(ItemId).ToString();
        int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
        ds = objPhr.getItemAttributes(common.myInt(Session["HospitalLocationID"]), ItemId, common.myInt(hdnStoreId.Value),
                                     common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), DoctorId, common.myInt(Session["CompanyId"]), 0);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["ISCalculationRequired"] = common.myBool(ds.Tables[0].Rows[0]["CalculationRequired"]);
            DataRow DR = ds.Tables[0].Rows[0];
            if (common.myInt(DR["ClosingBalance"]).Equals(0) && common.myStr(Session["OPIP"]).Equals("I"))
            {
                string strMsg = common.myStr(DR["ItemName"]) + "  Stock of the item not available. Delivery of the item will be delayed.";
                //strMsg = "Stock of the item not available. You can not prescribe the item.";
                Alert.ShowAjaxMsg(strMsg, this.Page);
            }
        }
    }
    protected void btnProceedFavourite_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["ProceedValue"] = "F";
            if (txtFavouriteItemName.Text == string.Empty)
            {
                proceedFavourite(false);
            }
            else
            {
                proceedFavourite(false);
                GetBrandFavouriteData(common.myStr(txtFavouriteItemName.Text));

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void btnProceedFavourite_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //ViewState["ProceedValue"] = "F";
    //        //proceedFavourite(false);
    //        GetBrandFavouriteData(common.myStr(txtFavouriteItemName.Text));
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    private void proceedFavourite(bool IsInContinue)
    {
        try
        {
            // Akshay_13072022_Evercare_Hyderabad
            DataTable dt = new DataTable();
            DataRow DR;
            dt.Columns.Add("UnAppPrescriptionId");
            dt.Columns.Add("IndentId");
            dt.Columns.Add("IndentNo");
            dt.Columns.Add("IndentDate");
            dt.Columns.Add("IndentTypeId");
            dt.Columns.Add("IndentType");
            dt.Columns.Add("CustomMedication");
            dt.Columns.Add("FrequencyId");
            dt.Columns.Add("FrequencyName");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Dose");
            dt.Columns.Add("Duration");
            dt.Columns.Add("DurationText");
            dt.Columns.Add("Instructions");
            dt.Columns.Add("GenericId");
            dt.Columns.Add("GenericName");
            dt.Columns.Add("ItemId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("UnitId");
            dt.Columns.Add("UnitName");
            dt.Columns.Add("Type");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("CIMSItemId");
            dt.Columns.Add("CIMSType");
            dt.Columns.Add("VIDALItemId");
            dt.Columns.Add("XMLData");
            dt.Columns.Add("PrescriptionDetail");
            dt.Columns.Add("FormulationId");
            dt.Columns.Add("FormulationName");
            dt.Columns.Add("RouteId");
            dt.Columns.Add("RouteName");
            dt.Columns.Add("StrengthId");
            dt.Columns.Add("StrengthValue");
            dt.Columns.Add("Qty");
            dt.Columns.Add("FoodRelationship");
            dt.Columns.Add("FoodRelationshipId");
            dt.Columns.Add("RefrenceItemId");
            dt.Columns.Add("RefrenceItemName");
            dt.Columns.Add("DoseTypeId");
            dt.Columns.Add("DoseTypeName");
            dt.Columns.Add("NotToPharmacy");
            dt.Columns.Add("IsInfusion");
            dt.Columns.Add("IsInjection");
            dt.Columns.Add("IsStop");
            dt.Columns.Add("IsCancel");
            dt.Columns.Add("Volume");
            dt.Columns.Add("VolumeUnitId");
            dt.Columns.Add("TotalVolume");
            dt.Columns.Add("InfusionTime");
            dt.Columns.Add("TimeUnit");
            dt.Columns.Add("FlowRate");
            dt.Columns.Add("FlowRateUnit");
            dt.Columns.Add("VolumeUnit");
            dt.Columns.Add("XmlVariableDose");
            dt.Columns.Add("IsOverride");
            dt.Columns.Add("OverrideComments");
            dt.Columns.Add("OverrideCommentsDrugToDrug");
            dt.Columns.Add("OverrideCommentsDrugHealth");
            dt.Columns.Add("XMLFrequencyTime");
            dt.Columns.Add("IsSubstitudeNotAllowed");
            dt.Columns.Add("ICDCode");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("InstructionRemarks");
            dt.Columns.Add("AfterTestDose");
            dt.Columns.Add("ClosingBalanceQty");
            dt.Columns.Add("ItemFlagName");
            dt.Columns.Add("ItemFlagCode");

            bool isFirst = true;
            foreach (GridViewRow dataItem in lstFavourite.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnFItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    if (common.myInt(hdnFItemId.Value) > 0)
                    {
                        LinkButton lnkItemName = (LinkButton)dataItem.FindControl("lnkItemName"); // 

                        HiddenField hdnFavItemName = (HiddenField)dataItem.FindControl("hdnFavItemName");
                        ddlBrand.Text = common.myStr(hdnFavItemName.Value);
                        try
                        {
                            ddlBrand.SelectedValue = common.myInt(hdnFItemId.Value).ToString();
                        }
                        catch
                        { }
                        hdnItemId.Value = common.myInt(hdnFItemId.Value).ToString();
                        HiddenField hdnFCIMSItemId = (HiddenField)dataItem.FindControl("hdnFCIMSItemId");
                        HiddenField hdnFCIMSType = (HiddenField)dataItem.FindControl("hdnFCIMSType");
                        HiddenField hdnFVIDALItemId = (HiddenField)dataItem.FindControl("hdnFVIDALItemId");
                        hdnCIMSItemId.Value = common.myStr(hdnFCIMSItemId.Value);
                        hdnCIMSType.Value = common.myStr(hdnFCIMSType.Value);
                        hdnVIDALItemId.Value = common.myStr(hdnFVIDALItemId.Value);
                        if (IsInContinue)
                        {
                            if (isFirst)
                            {
                                chkRow.Checked = false;
                                isFirst = false;
                                continue;
                            }
                        }
                        //SetStrengthFormUnitRouteOfSelectedDrug();
                        HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");
                        HiddenField hdnUnitId = (HiddenField)dataItem.FindControl("hdnUnitId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyId");
                        HiddenField hdnDuration = (HiddenField)dataItem.FindControl("hdnDuration");
                        HiddenField hdnDurationType = (HiddenField)dataItem.FindControl("hdnDurationType");
                        HiddenField hdnFoodRelationshipId = (HiddenField)dataItem.FindControl("hdnFoodRelationshipId");
                        if (common.myDbl(hdnDose.Value) > 0)
                        {
                            txtDose.Text = common.myInt(hdnDose.Value).ToString();
                        }
                        else
                        {
                            txtDose.Text = string.Empty;//"1"
                        }
                        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                        //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue(common.myInt(hdnUnitId.Value).ToString()));
                        //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(hdnStrengthId.Value).ToString()));
                        ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                        ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));
                        //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(hdnFormulationId.Value).ToString()));
                        ddlFormulation_OnSelectedIndexChanged(this, null);
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                        //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(hdnRouteId.Value).ToString()));
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                        //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                        if (common.myInt(hdnDuration.Value) > 0)
                        {
                            txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                        }
                        else
                        {
                            txtDuration.Text = string.Empty;//"1"
                        }
                        if (common.myLen(hdnDurationType.Value) > 0)
                        {
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                            //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue(common.myStr(hdnDurationType.Value).ToString()));
                        }
                        else
                        {
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                            //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue("D"));
                        }
                        endDateChange();
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));
                        //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));
                        if (common.myDbl(txtDose.Text).Equals(0.0))
                        {
                            txtDose.Text = string.Empty;//"1"
                        }
                        if (common.myStr(Session["OPIP"]).Equals("I"))
                        {
                            if (common.myInt(txtDuration.Text).Equals(0))
                            {
                                txtDuration.Text = string.Empty;//"1"
                            }
                        }
                        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                        {
                            if (common.myBool(Session["IsCIMSInterfaceActive"]))
                            {
                                if (common.myLen(hdnCIMSItemId.Value) < 2)
                                {
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                    lblMessage.Text = "This Drug is not mapped with CIMS.";
                                }
                                else
                                {
                                    chkIsInteraction(common.myInt(hdnFItemId.Value));
                                }
                            }
                            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                            {
                                if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                                {
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                    lblMessage.Text = "This Drug is not mapped with VIDAL.";
                                }
                            }
                        }
                        // Akshay_13072022_Evercare_Hyderabad
                        HiddenField hdnPrescriptionDetails = (HiddenField)dataItem.FindControl("hdnPrescription");
                        DR = dt.NewRow();
                        DR["ItemId"] = common.myInt(hdnFItemId.Value);
                        DR["ItemName"] = common.myStr(hdnFavItemName.Value);
                        DR["CIMSItemId"] = common.myStr(hdnFCIMSItemId.Value);
                        DR["CIMSType"] = common.myStr(hdnFCIMSType.Value);
                        DR["VIDALItemId"] = common.myStr(hdnFVIDALItemId.Value);
                        DR["UnitId"] = common.myInt(hdnUnitId.Value);
                        DR["StrengthValue"] = common.myStr(hdnStrengthValue.Value);
                        DR["FormulationId"] = common.myInt(hdnFormulationId.Value);
                        DR["RouteId"] = common.myInt(hdnRouteId.Value);
                        DR["FrequencyId"] = common.myInt(hdnFrequencyId.Value);
                        DR["Duration"] = common.myInt(hdnDuration.Value);
                        DR["DurationType"] = common.myLen(hdnDurationType.Value);
                        DR["FoodRelationshipId"] = common.myInt(hdnFoodRelationshipId.Value);
                        if (common.myStr(hdnPrescriptionDetails.Value).Trim() != "0")
                        {
                            DR["PrescriptionDetail"] = common.myStr(hdnPrescriptionDetails.Value);
                        }
                        dt.Rows.Add(DR);
                        //return;
                    }
                }
            }
            // Akshay_13072022_Evercare_Hyderabad
            dt.AcceptChanges();
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnProceedCurrent_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["ProceedValue"] = "C";
            proceedCurrent(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void proceedCurrent(bool IsInContinue)
    {
        try
        {
            bool isFirst = true;
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnItemIdC = (HiddenField)dataItem.FindControl("hdnItemId");
                    int ItemId = common.myInt(hdnItemIdC.Value);
                    int GenericId = common.myInt(((HiddenField)dataItem.FindControl("hdnGenericId")).Value);
                    if (ItemId > 0 || GenericId > 0)
                    {
                        if (ItemId <= 0 && GenericId.Equals(0))
                        {
                            return;
                        }
                        string CIMSItemId = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSItemId")).Value);
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value);
                        int VIDALItemId = common.myInt(((HiddenField)dataItem.FindControl("hdnVIDALItemId")).Value);
                        if (ItemId > 0)
                        {
                            //ddlFormulation.Enabled = false;
                            //ddlRoute.Enabled = false;
                            //ddlStrength.Enabled = false;
                        }
                        else
                        {
                            ddlFormulation.Enabled = true;
                            ddlRoute.Enabled = true;
                            //ddlStrength.Enabled = true;
                            ddlStrengthValue.Enabled = true;
                        }
                        hdnGenericId.Value = common.myStr(GenericId);
                        hdnItemId.Value = common.myStr(ItemId);
                        hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                        hdnCIMSType.Value = common.myStr(CIMSType);
                        hdnVIDALItemId.Value = common.myStr(VIDALItemId);
                        Label GenericName = (Label)dataItem.FindControl("lblGenericName");
                        LinkButton lnkItemName = (LinkButton)dataItem.FindControl("lnkItemName");
                        Label TotalQty = (Label)dataItem.FindControl("lblTotalQty");
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnFrequencyId = (HiddenField)dataItem.FindControl("hdnFrequencyId");
                        HiddenField hdnDose = (HiddenField)dataItem.FindControl("hdnDose");
                        HiddenField hdnDays = (HiddenField)dataItem.FindControl("hdnDays");
                        HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                        HiddenField hdnEndDate = (HiddenField)dataItem.FindControl("hdnEndDate");
                        HiddenField hdnRemarks = (HiddenField)dataItem.FindControl("hdnRemarks");
                        try
                        {
                            hdnItemName.Value = string.Empty;
                            hdnGenericName.Value = string.Empty;
                            lblGenericName.Text = string.Empty;

                            ddlBrand.Text = string.Empty;
                            ddlBrand.SelectedValue = null;
                            ddlGeneric.Text = string.Empty;
                            ddlGeneric.SelectedValue = null;

                            if (ItemId > 0)
                            {
                                ddlBrand.Text = common.myStr(lnkItemName.Text);
                                ddlBrand.SelectedValue = ItemId.ToString();
                            }
                            else if (GenericId > 0 && ItemId <= 0)
                            {
                                ddlGeneric.Text = common.myStr(lnkItemName.Text);
                                ddlGeneric.SelectedValue = GenericId.ToString();
                            }
                        }
                        catch
                        {
                        }
                        double days = common.myDbl(hdnDays.Value);
                        double period = days;
                        string periodType = "D";
                        if ((days % 356).Equals(0))
                        {
                            period = days / 365;
                            periodType = "Y";
                        }
                        else if ((days % 30).Equals(0))
                        {
                            period = days / 30;
                            periodType = "M";
                        }
                        else if ((days % 7).Equals(0))
                        {
                            period = days / 7;
                            periodType = "W";
                        }
                        else
                        {
                            period = days;
                            periodType = "D";
                        }

                        if (ItemId > 0)
                        {
                            hdnItemName.Value = common.myStr(lnkItemName.Text);
                            hdnGenericName.Value = common.myStr(GenericName.Text);
                            lblGenericName.Text = common.myStr(GenericName.Text);
                        }
                        else if (GenericId > 0 && ItemId <= 0)
                        {
                            hdnGenericName.Value = common.myStr(lnkItemName.Text);
                            lblGenericName.Text = common.myStr(lnkItemName.Text);
                        }

                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                        //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myStr(hdnFormulationId.Value)));
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                        //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myStr(hdnRouteId.Value)));
                        //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                        ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                        ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                        if (IsInContinue)
                        {
                            if (isFirst)
                            {
                                chkRow.Checked = false;
                                isFirst = false;
                                continue;
                            }
                        }
                        chkIsInteraction(ItemId);
                        setDefaultFavourite(ItemId);
                        return;
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
    private void BindItemWithItemDetail(DataTable dtItem, DataTable dtItemDetail)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["Item"];
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["ItemDetail"];
        DataTable dtNew = new DataTable();
        DataSet ds = new DataSet();
        DataTable dtTemp = new DataTable();
        DataRow DR;
        DataRow DR1;
        decimal dQty = 0;
        int countRow = 0;
        try
        {
            if (dt1 == null)
            {
                dt1 = new DataTable();
            }
            for (int counter = 0; counter < dtItem.Rows.Count; counter++)
            {
                dtTemp = new DataTable();
                dtTemp = (DataTable)ViewState["Item"];
                int exists = 0;
                for (int counter1 = 0; counter1 < dtTemp.Rows.Count; counter1++)
                {
                    if (common.myInt(dtItem.Rows[counter]["ItemId"]).Equals(common.myInt(dtTemp.Rows[counter1]["ItemId"])))
                    {
                        exists = 1;
                        break;
                        //counter1 = dtTemp.Rows.Count;
                    }
                }
                if (exists.Equals(0))
                {
                    DR = dt.NewRow();
                    if (common.myInt(ddlIndentType.SelectedValue).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Please select Indent Type", Page);
                        return;
                    }
                    DR["IndentTypeId"] = dtItem.Rows[counter]["IndentTypeId"];
                    DR["IndentType"] = dtItem.Rows[counter]["IndentType"];
                    DR["IndentId"] = 0;
                    DR["GenericId"] = dtItem.Rows[counter]["GenericId"];
                    DR["ItemId"] = dtItem.Rows[counter]["ItemId"];
                    DR["GenericName"] = dtItem.Rows[counter]["GenericName"];
                    DR["ItemName"] = dtItem.Rows[counter]["ItemName"];
                    DR["RouteId"] = dtItem.Rows[counter]["RouteId"];
                    DR["CustomMedication"] = dtItem.Rows[counter]["CustomMedication"];
                    DR["NotToPharmacy"] = common.myBool(dtItem.Rows[counter]["NotToPharmacy"]);
                    DR["IsInfusion"] = dtItem.Rows[counter]["IsInfusion"];
                    DR["IsInjection"] = dtItem.Rows[counter]["IsInjection"];
                    DR["FormulationId"] = dtItem.Rows[counter]["FormulationId"];
                    DR["IsInfusion"] = true;
                    try
                    {
                        int duration = 1;
                        try
                        {
                            if (dtItem.Rows[counter]["StartDate"] != null && dtItem.Rows[counter]["EndDate"] != null)
                            {
                                if (common.myDate(dtItem.Rows[counter]["StartDate"]) < common.myDate(dtItem.Rows[counter]["EndDate"]))
                                {
                                    TimeSpan t = common.myDate(dtItem.Rows[counter]["EndDate"]) - common.myDate(dtItem.Rows[counter]["StartDate"]);
                                    duration = (int)t.TotalDays;
                                    if (duration > 1)
                                    {
                                        duration += 1;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        if (duration < 1)
                        {
                            duration = 1;
                        }
                        DR["StartDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                        DR["EndDate"] = endDateCalculate(DateTime.Now, duration, "D").ToString("dd/MM/yyyy");
                    }
                    catch
                    {
                    }
                    if (dtItemDetail.Rows.Count > counter)
                    {
                        DR1 = dt1.NewRow();
                        dt1.AcceptChanges();
                        DR1["RouteId"] = dtItemDetail.Rows[counter]["RouteId"];
                        DR1["RouteName"] = dtItemDetail.Rows[counter]["RouteName"];
                        DR1["GenericId"] = dtItemDetail.Rows[counter]["GenericId"];
                        DR1["ItemId"] = dtItemDetail.Rows[counter]["ItemId"];
                        DR1["GenericName"] = dtItemDetail.Rows[counter]["GenericName"];
                        DR1["ItemName"] = dtItemDetail.Rows[counter]["ItemName"];
                        DR1["Dose"] = dtItemDetail.Rows[counter]["Dose"];
                        DR1["Frequency"] = dtItemDetail.Rows[counter]["Frequency"];
                        DR1["Duration"] = dtItemDetail.Rows[counter]["Duration"];
                        DR1["DurationText"] = dtItemDetail.Rows[counter]["DurationText"];
                        DR1["Type"] = dtItemDetail.Rows[counter]["Type"];
                        //DR1["StartDate"] = dtItemDetail.Rows[counter]["StartDate"];
                        //DR1["EndDate"] = dtItemDetail.Rows[counter]["EndDate"];
                        try
                        {
                            int duration = 1;
                            try
                            {
                                if (dtItemDetail.Rows[counter]["StartDate"] != null && dtItemDetail.Rows[counter]["EndDate"] != null)
                                {
                                    if (common.myDate(dtItemDetail.Rows[counter]["StartDate"]) < common.myDate(dtItemDetail.Rows[counter]["EndDate"]))
                                    {
                                        TimeSpan t = common.myDate(dtItemDetail.Rows[counter]["EndDate"]) - common.myDate(dtItemDetail.Rows[counter]["StartDate"]);
                                        duration = (int)t.TotalDays;
                                        if (duration > 1)
                                        {
                                            duration += 1;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            if (duration < 1)
                            {
                                duration = 1;
                            }
                            DR1["StartDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                            DR1["EndDate"] = endDateCalculate(DateTime.Now, duration, "D").ToString("dd/MM/yyyy");
                        }
                        catch
                        {
                        }
                        DR1["FoodRelationshipId"] = dtItemDetail.Rows[counter]["FoodRelationshipId"];
                        DR1["FoodRelationship"] = dtItemDetail.Rows[counter]["FoodRelationship"];
                        DR1["UnitId"] = dtItemDetail.Rows[counter]["UnitId"]; //0
                        DR1["UnitName"] = dtItemDetail.Rows[counter]["UnitName"];
                        DR1["CIMSItemId"] = common.myStr(dtItemDetail.Rows[counter]["CIMSItemId"]);
                        DR1["CIMSType"] = common.myStr(dtItemDetail.Rows[counter]["CIMSType"]);
                        DR1["VIDALItemId"] = common.myInt(dtItemDetail.Rows[counter]["VIDALItemId"]);
                        DR1["PrescriptionDetail"] = dtItemDetail.Rows[counter]["PrescriptionDetail"];
                        DR1["Instructions"] = dtItemDetail.Rows[counter]["Instructions"];
                        DR1["ReferanceItemId"] = dtItemDetail.Rows[counter]["ReferanceItemId"];
                        DR1["ReferanceItemName"] = dtItemDetail.Rows[counter]["ReferanceItemName"];
                        DR1["DoseTypeId"] = dtItemDetail.Rows[counter]["DoseTypeId"];
                        DR1["DoseTypeName"] = dtItemDetail.Rows[counter]["DoseTypeName"];
                        DR1["FrequencyId"] = dtItemDetail.Rows[counter]["FrequencyId"];
                        DR1["FrequencyName"] = dtItemDetail.Rows[counter]["FrequencyName"];
                        DR1["IsInfusion"] = true;
                        dt1.Rows.Add(DR1);
                        dt1.AcceptChanges();
                        dt1.TableName = "ItemDetail";
                    }
                    StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.
                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    dt1.WriteXml(writer);
                    string xmlSchema = writer.ToString();
                    xmlSchema = xmlSchema.Replace("&lt;", "<");
                    xmlSchema = xmlSchema.Replace("&gt;", ">");
                    DR["XMLData"] = xmlSchema;
                    DR["PrescriptionDetail"] = objEMR.GetPrescriptionDetailStringNew(dt1, chkFractionalDose.Checked ? "Y" : "N");
                    //Item Details end
                    dt.Rows.Add(DR);
                    dt.AcceptChanges();
                }
            }
            ViewState["Item"] = dt;
            ViewState["ItemDetail"] = dt1;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dtold.Dispose();
            dt1old.Dispose();
            dt.Dispose();
            dt1.Dispose();
            dtNew.Dispose();
            ds.Dispose();
            dtTemp.Dispose();
            objEMR = null;
        }
    }
    protected void rdoSearchMedication_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblFavouriteSearch.Visible = false;
            pnlFavouriteDetails.Visible = false;
            tblCurrentSearch.Visible = false;
            pnlCurrentDetails.Visible = false;
            tblOrderSetSearch.Visible = false;
            pnlOrderSet.Visible = false;
            ViewState["OrderSetId"] = "0";
            switch (common.myStr(rdoSearchMedication.SelectedValue))
            {
                case "F": //Favourite Medication
                    tblFavouriteSearch.Visible = true;
                    pnlFavouriteDetails.Visible = true;
                    GetBrandFavouriteData(string.Empty);
                    break;
                case "C": //Current Medication
                    tblCurrentSearch.Visible = true;
                    pnlCurrentDetails.Visible = true;
                    ViewState["Mode"] = "P";
                    BindGrid(string.Empty, string.Empty, string.Empty);
                    break;
                case "OS":
                    tblOrderSetSearch.Visible = true;
                    pnlOrderSet.Visible = true;
                    bindOrderSet();
                    break;
                case "P": //Previous Medication
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        Hashtable hshOutput = new Hashtable();
        try
        {
            if (common.myLen(txtStopRemarks.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }
            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0)
            {
                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                               common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]), common.myInt(ViewState["RegId"]),
                               common.myInt(ViewState["EncId"]), 0, common.myStr(txtStopRemarks.Text), common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));
                BindGrid(string.Empty, string.Empty, string.Empty);
                txtStopRemarks.Text = string.Empty;
                ViewState["IndentDetailsId"] = "0";
                lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);
                return;
            }
            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;
            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            hshOutput = null;
        }
    }
    protected void btnStopClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnVariableDose_OnClick(object sender, EventArgs e)
    {
        CallVariableDose(false);
    }
    private int CallVariableDose(bool checkforDose)
    {
        lblMessage.Text = string.Empty;
        if (common.myStr(ddlDoseType.Text).Equals("PRN") || common.myStr(ddlDoseType.Text).Equals("STAT"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Variable Dose not applicable for " + common.myStr(ddlDoseType.Text);
            return 0;
        }
        if (!common.myStr(ddlDoseType.Text).Equals("PRN") || !common.myStr(ddlDoseType.Text).Equals("STAT"))
        {
            if (common.myInt(ddlFrequencyId.SelectedValue).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select frequency.";
                return 0;
            }
            if (common.myInt(txtDuration.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter duration values.";
                return 0;
            }
            if (!common.myStr(ddlPeriodType.SelectedValue).Equals("D"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Variable dose allow only for Days(s).";
                return 0;
            }
        }
        int dosemessage = 0;
        if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
        {
            Cache.Insert("VariableDose" + common.myStr(Session["UserId"]), common.myStr(hdnXmlVariableDoseString.Value), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        else
        {
            Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
        }
        if (checkforDose && !common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
        {
            dosemessage = 1;
            if (common.myInt(ddlFrequencyId.SelectedValue).Equals(common.myInt(hdnvariableDoseFrequency.Value)))
            {
                if (common.myDbl(txtDose.Text).Equals(common.myDbl(hdnVariabledose.Value)))
                {
                    if (common.myDbl(txtDuration.Text).Equals(common.myDbl(hdnvariableDoseDuration.Value)))
                    {
                        // Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                        return 1;
                    }
                }
            }
        }

        if (common.myInt(hdnItemId.Value).Equals(0) && common.myInt(ddlBrand.SelectedValue).Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Variable dose allow only for prescribe brand.";
            Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
            return 0;
        }

        RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?Day=" + common.myInt(txtDuration.Text) + "&FrequencyId=" + common.myInt(ddlFrequencyId.SelectedValue) + "&Dose=" + txtDose.Text + "&IName=" + ddlBrand.Text + "&DisplayMsg=" + dosemessage;
        RadWindow1.Height = 500;
        RadWindow1.Width = 800;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientCloseVariableDose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
        return 0;

    }
    protected void lbtnFrequencyTime_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlFrequencyId.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select frequency.", Page);
                return;
            }
            if (common.myInt(txtDuration.Text).Equals(0))
            {
                Alert.ShowAjaxMsg("Please enter duration values.", Page);
                return;
            }
            if (common.myInt(hdnItemId.Value).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select Drug", Page);
                return;
            }
            int days = 0;
            if (common.myStr(ddlPeriodType.SelectedValue).Equals("D"))
            {
                days = common.myInt(txtDuration.Text);
            }
            else if (common.myStr(ddlPeriodType.SelectedValue).Equals("W"))
            {
                days = common.myInt(txtDuration.Text) * 7;
            }
            else if (common.myStr(ddlPeriodType.SelectedValue).Equals("M"))
            {
                days = common.myInt(txtDuration.Text) * 30;
            }
            else
            {
                days = 1;
            }
            //RadWindow1.NavigateUrl = "/EMR/Medication/DoseTime.aspx?Day=" + days +
            //                        "&FrequencyId=" + common.myInt(ddlFrequencyId.SelectedValue) +
            //                        "&ItemId=" + common.myInt(hdnItemId.Value) +
            //                        "&ItemName=" + common.myStr(ddlBrand.Text).Replace(":", "-");// +"&FrequencyData=" + hdnXmlFrequencyTime.Value;

            RadWindow1.NavigateUrl = "/EMR/Medication/DoseTime.aspx?Day=" + days +
                                "&FrequencyId=" + common.myInt(ddlFrequencyId.SelectedValue) +
                                "&ItemId=" + common.myInt(hdnItemId.Value) +
                            // "&IndentId=" + common.myInt(hdnIndentId.Value) +
                            "&RegId=" + common.myStr(ViewState["RegId"]) +
                            "&EncId=" + common.myStr(ViewState["EncId"]) +
                            "&From=Ward" +
                                //"&InitialDate=" + common.myDate(lblStartDate.Text).ToString("yyyy/MM/dd") +
                                "&ItemName=" + common.myStr(ddlBrand.Text).Replace(":", "-");// +"&FrequencyData=" + hdnXmlFrequencyTime.Value;

            RadWindow1.Height = 500;
            RadWindow1.Width = 1100;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "OnClientCloseFrequencyTime";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch
        {
        }
    }
    private void CallDoseTime(bool checkforDose)
    {
        try
        {
            lblMessage.Text = string.Empty;
            if (common.myStr(ddlDoseType.Text).Equals("PRN") || common.myStr(ddlDoseType.Text).Equals("STAT"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Variable Dose not applicable for " + common.myStr(ddlDoseType.Text);
                // return 0;
            }
            if (!common.myStr(ddlDoseType.Text).Equals("PRN") || !common.myStr(ddlDoseType.Text).Equals("STAT"))
            {
                if (common.myInt(ddlFrequencyId.SelectedValue).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select frequency.";
                    // return 0;
                }
                if (common.myInt(txtDuration.Text).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter duration values.";
                    //  return 0;
                }
                if (!common.myStr(ddlPeriodType.SelectedValue).Equals("D"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Variable dose allow only for Days(s).";
                    // return 0;
                }
            }
            int dosemessage = 0;
            if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
            {
                Cache.Insert("VariableDose" + common.myStr(Session["UserId"]), common.myStr(hdnXmlVariableDoseString.Value), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
            }
            if (checkforDose && !common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
            {
                dosemessage = 1;
                if (common.myInt(ddlFrequencyId.SelectedValue).Equals(common.myInt(hdnvariableDoseFrequency.Value)))
                {
                    if (common.myDbl(txtDose.Text).Equals(common.myDbl(hdnVariabledose.Value)))
                    {
                        if (common.myDbl(txtDuration.Text).Equals(common.myDbl(hdnvariableDoseDuration.Value)))
                        {
                            // Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                            // return 1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void lnkPharmacistInstruction_OnClick(object sender, EventArgs e)
    {
        dvPharmacistInstruction.Visible = true;
    }
    protected void btnPharmacistInstructionClose_OnClick(object sender, EventArgs e)
    {
        dvPharmacistInstruction.Visible = false;
    }
    ///////////////CIMS/////////////////////////////

    private void setGridColor()
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");
                        LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                        LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                        LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                        LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                        LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                        lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                        lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                        lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                        lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                        lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                        lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsVIDAL");
                        LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
                        LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                        LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
                        LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionVIDAL");
                        LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                        lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                        lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                        lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                        lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                        lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                        lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                    }
                }
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");
                        LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                        LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                        LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                        LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                        LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                        lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                        lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                        lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                        lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                        lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                        lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));
                    }
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsVIDAL");
                        LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
                        LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                        LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
                        LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionVIDAL");
                        lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                        lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                        lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                        lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                        lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                    }
                }
                lnkDrugAllergy.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
            }
        }
        catch
        {
        }
    }
    private void setDiagnosis()
    {
        DataTable tbl = new DataTable();
        try
        {
            if (ViewState["ICDCodeMaster"] != null)
            {
                tbl = (DataTable)ViewState["ICDCodeMaster"];
                if (tbl != null)
                {
                    ViewState["PatientDiagnosisXML"] = string.Empty;
                    //<HealthIssueCodes>
                    //    <HealthIssueCode code="K22" codeType="ICD10" />
                    //    <HealthIssueCode code="K22.0" codeType="ICD10" />
                    //</HealthIssueCodes>
                    //ViewState["PatientDiagnosisXML"] = "<HealthIssueCodes><HealthIssueCode code=\"J45\" codeType=\"ICD10\" /><HealthIssueCode code=\"N17\" codeType=\"ICD10\" /><HealthIssueCode code=\"I11\" codeType=\"ICD10\" /><HealthIssueCode code=\"F32\" codeType=\"ICD10\" /></HealthIssueCodes>";
                    if (common.myBool(Session["IsCIMSInterfaceActive"])
                        || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {

                        if (tbl.Rows.Count > 0)
                        {
                            if (common.myBool(Session["IsCIMSInterfaceActive"]))
                            {
                                StringBuilder HealthIssueCodes = new StringBuilder();
                                StringBuilder HealthCode = new StringBuilder();
                                foreach (DataRow DR in tbl.Rows)
                                {
                                    if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                                    {
                                        HealthCode.Append("<HealthIssueCode code=\"" + common.myStr(DR["ICDCode"]).Trim() + "\" codeType=\"ICD10\" />");
                                    }
                                }
                                if (common.myLen(HealthCode) > 0)
                                {
                                    HealthIssueCodes.Append("<HealthIssueCodes>" + HealthCode.ToString() + "</HealthIssueCodes>");
                                }
                                else
                                {
                                    HealthIssueCodes.Append("<HealthIssueCodes />");
                                }
                                ViewState["PatientDiagnosisXML"] = HealthIssueCodes.ToString();
                            }
                            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                            {
                                List<string> list = new List<string>();
                                foreach (DataRow DR in tbl.Rows)
                                {
                                    if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                                    {
                                        list.Add(common.myStr(DR["ICDCode"]).Trim().Replace(".", string.Empty));
                                    }
                                }
                                ViewState["PatientDiagnosisXML"] = list;
                            }
                        }
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
    private void setAllergiesWithInterfaceCode()
    {
        DataTable dt = new DataTable();
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            if (ViewState["DrugAllergiesInterfaceCode"] != null)
            {
                ViewState["PatientAllergyXML"] = string.Empty;

                if (common.myBool(Session["IsCIMSInterfaceActive"])
                    || common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    dt = (DataTable)ViewState["DrugAllergiesInterfaceCode"];
                    if (dt != null)
                    {
                        DV = dt.DefaultView;
                        tbl = new DataTable();
                        if (common.myBool(Session["IsCIMSInterfaceActive"]))
                        {
                            DV.RowFilter = "AllergyType='CIMS'";
                            tbl = DV.ToTable();
                            if (tbl.Rows.Count > 0)
                            {
                                StringBuilder Allergies = new StringBuilder();
                                StringBuilder itemsDetails = new StringBuilder();
                                foreach (DataRow DR in tbl.Rows)
                                {
                                    if (common.myStr(DR["InterfaceCode"]).Trim().Length > 0)
                                    {
                                        itemsDetails.Append("<" + common.myStr(DR["CIMSTYPE"]).Trim() + " reference=\"" + common.myStr(DR["InterfaceCode"]).Trim() + "\" />");
                                    }
                                }
                                if (common.myLen(itemsDetails) > 0)
                                {
                                    Allergies.Append("<Allergies>" + itemsDetails.ToString() + "</Allergies>");
                                }
                                else
                                {
                                    Allergies.Append("<Allergies />");
                                }
                                ViewState["PatientAllergyXML"] = Allergies.ToString();
                            }
                        }
                        else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                        {
                            DV.RowFilter = "AllergyType='VIDAL'";
                            tbl = DV.ToTable();
                            if (tbl.Rows.Count > 0)
                            {
                                List<int?> list = new List<int?>();
                                foreach (DataRow DR in tbl.Rows)
                                {
                                    if (common.myStr(DR["InterfaceCode"]).Trim().Length > 0)
                                    {
                                        list.Add(common.myInt(DR["InterfaceCode"]));
                                    }
                                }
                                int?[] allergyIds = list.ToArray();
                                ViewState["PatientAllergyXML"] = allergyIds;
                            }
                        }
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
            ViewState["DrugAllergiesInterfaceCode"] = null;
            dt.Dispose();
            tbl.Dispose();
            DV.Dispose();
        }
    }
    protected void btnBrandDetailsView_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemId.Value) < 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Generic of Brnad not selected ";
                return;
            }
            showBrandDetails(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
        }
    }
    protected void btnMonographView_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myLen(hdnCIMSItemId.Value) < 2)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Generic of Brnad not selected ";
                    return;
                }
                showMonograph(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
        }
        catch
        {
        }
    }
    private void showMonograph(string CIMSItemId, string CIMSType)
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = getMonographXML(CIMSType, common.myStr(CIMSItemId));
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        { }
    }
    private void showBrandDetails(string CIMSItemId, string CIMSType)
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = getBrandDetailsXMLCIMS(CIMSType, common.myStr(CIMSItemId));
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(true);
            }
        }
        catch
        {
        }
    }
    protected void btnInteractionView_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";
                string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);
                if (!strXML.Equals(string.Empty))
                {
                    Session["CIMSXMLInputData"] = strXML;
                    openWindowsCIMS(false);
                }
            }
        }
        catch
        {
        }
    }
    private void showDiIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getDIInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }
    private void showIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }



    private void showDIIntreraction()
    {
        try
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }
            string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getDIInterationXML(string.Empty);
            if (!strXML.Equals(string.Empty))
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        catch
        {
        }
    }
    private void showHealthOrAllergiesIntreraction(string HealthOrAllergies)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }
                string strXML = getHealthOrAllergiesInterationXML("B", string.Empty);
                if (!strXML.Equals(string.Empty))
                {
                    Session["CIMSXMLInputData"] = strXML;
                    openWindowsCIMS(false);
                }
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myStr(HealthOrAllergies).Equals("H"))//Health
                {
                    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                    if (commonNameGroupIds.Length > 0)
                    {
                        getDrugHealthInteractionVidal(commonNameGroupIds);
                    }
                }
                else if (common.myStr(HealthOrAllergies).Equals("A"))//Allergies
                {
                    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                    if (commonNameGroupIds.Length > 0)
                    {
                        getDrugAllergyVidal(commonNameGroupIds);
                    }
                }
            }
        }
        catch
        {
        }
    }
    private void chkIsInteraction(int ItemId)
    {
        clsCIMS objCIMS = new clsCIMS();
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //DataTable tblItem = new DataTable();
                //tblItem = (DataTable)ViewState["GridDataItem"];
                //DataView DVItem = tblItem.Copy().DefaultView;
                //if (common.myInt(ItemId) > 0)
                //{
                //    DVItem.RowFilter = "ItemId = " + common.myInt(ItemId);
                //}
                ////else
                ////{
                ////    DVItem.RowFilter = "GenericId = " + common.myInt(hdnGenericId.Value);
                ////}
                spnCommentsDrugAllergy.Visible = false;
                spnCommentsDrugToDrug.Visible = false;
                spnCommentsDrugHealth.Visible = false;
                txtCommentsDrugAllergy.Enabled = false;
                txtCommentsDrugToDrug.Enabled = false;
                txtCommentsDrugHealth.Enabled = false;
                txtCommentsDrugAllergy.Text = string.Empty;
                txtCommentsDrugToDrug.Text = string.Empty;
                txtCommentsDrugHealth.Text = string.Empty;
                lblIntreactionMessage.Text = string.Empty;
                txtInteractionBetweenMessage.Text = string.Empty;
                //string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";
                string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";
                string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);
                if (!strXML.Equals(string.Empty))
                {
                    string outputValues = objCIMS.getFastTrack5Output(strXML);
                    if (outputValues != null)
                    {
                        //if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
                        //{
                        //    string strFindCIMSItemId = "<PRODUCT REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=\"\"></PRODUCT>";
                        //    if (!outputValues.ToUpper().Contains(strFindCIMSItemId.ToUpper()))
                        //    {
                        //        IsInteraction = true;
                        //        ViewState["NewPrescribing"] = strXML;
                        //        dvInteraction.Visible = true;
                        //    }
                        //}
                        //if (outputValues.ToUpper().Contains("<SEVERITY NAME=\"SEVERE\" RANKING=\"5\">SEVERE</SEVERITY>")
                        //    || outputValues.ToUpper().Contains("<SEVERITY NAME=\"MODERATE\" RANKING=\"4\">MODERATE</SEVERITY>")
                        //    || outputValues.ToUpper().Contains("<SEVERITY NAME=\"MINOR\" RANKING=\"3\">MINOR</SEVERITY>")
                        //    || outputValues.ToUpper().Contains("<SEVERITY NAME=\"CAUTION\" RANKING=\"2\">CAUTION</SEVERITY>"))
                        //{
                        //    string strPatternMatch = "<PRODUCT REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                        //    if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                        //    {
                        //        IsInteraction = true;
                        //        ViewState["NewPrescribing"] = strXML;
                        //        dvInteraction.Visible = true;
                        //    }
                        //}
                        //Drug to Drug Interation
                        if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues, true) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                            if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                            {
                                ViewState["NewPrescribing"] = strXML;
                                //if (outputValues.ToUpper().Contains("<SEVERITY NAME=\"SEVERE\" RANKING=\"5\">SEVERE</SEVERITY>"))
                                dvInteraction.Visible = true;
                                btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));
                                spnCommentsDrugToDrug.Visible = true;
                                txtCommentsDrugToDrug.Enabled = true;
                            }
                        }
                        //Drug Health Interation
                        //if (outputValues.ToUpper().Contains("<SEVERITY NAME=\"CONTRAINDICATED\" RANKING=\"3\">")
                        //    || outputValues.ToUpper().Contains("<SEVERITY NAME=\"EXTREME CAUTION\" RANKING=\"2\">")
                        //    || outputValues.ToUpper().Contains("<SEVERITY NAME=\"CAUTION\" RANKING=\"1\">"))
                        /*
                        if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                            ViewState["NewPrescribing"] = strXML;
                            dvInteraction.Visible = true;
                            btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));
                            spnCommentsDrugHealth.Visible = true;
                            txtCommentsDrugHealth.Enabled = true;
                        }
                        */
                        //Drug Allergy Interation
                        //if (outputValues.ToUpper().Contains("<ALLERGY>"))
                        if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                            ViewState["NewPrescribing"] = strXML;
                            dvInteraction.Visible = true;
                            btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));
                            spnCommentsDrugAllergy.Visible = true;
                            txtCommentsDrugAllergy.Enabled = true;
                        }
                        if (dvInteraction.Visible)
                        {
                            txtInteractionBetweenMessage.Text = getInterfaceBetweenDrug(strXML);
                            txtInteractionBetweenMessage.ToolTip = txtInteractionBetweenMessage.Text;
                        }
                    }
                }
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                //return;
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
            objCIMS = null;
        }
    }
    private string getInterfaceBetweenDrug(string strXML)
    {
        StringBuilder sbOutput = new StringBuilder();
        Session["CIMSXMLInputData"] = strXML;
        clsCIMS objCIMS = new clsCIMS();
        try
        {
            string strFinalOutput = objCIMS.getCIMSFinalOutupt(false);
            using (StringReader reader = new StringReader(strFinalOutput))
            {
                string linePrevious = string.Empty;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(" vs "))
                    {
                        if (!sbOutput.ToString().Equals(string.Empty))
                        {
                            sbOutput.Append(Environment.NewLine);
                        }
                        if (line.Trim().StartsWith("vs "))
                        {
                            //D2H
                            sbOutput.Append((linePrevious.Trim() + " " + line.Trim()).Trim());
                        }
                        else
                        {
                            //D2D
                            if (line.Trim().Length < 400)
                            {
                                sbOutput.Append(line.Trim());
                            }
                        }
                    }
                    else if (line.ToUpper().Contains("PATIENT MAY BE ALLERGIC TO THE PRESCRIBING ITEM"))
                    {
                        //D2A
                        try
                        {
                            int startIdx = line.IndexOf("<a href=\"#\">") + 12;
                            string strTemp = line.Substring(startIdx, line.Length - startIdx);
                            strTemp = strTemp.Substring(0, strTemp.IndexOf("</a>"));
                            if (!sbOutput.ToString().Equals(string.Empty))
                            {
                                sbOutput.Append(Environment.NewLine);
                            }
                            sbOutput.Append(strTemp.Trim());
                        }
                        catch
                        {
                        }
                    }
                    linePrevious = line;
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
        finally
        {
            objCIMS = null;
        }
        return sbOutput.ToString();
    }
    protected void btnInteractionContinue_OnClick(object sender, EventArgs e)
    {
        lblIntreactionMessage.Text = string.Empty;
        if (spnCommentsDrugAllergy.Visible && common.myLen(txtCommentsDrugAllergy.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug Allergy Comments can't be blank ! ";
        }
        if (spnCommentsDrugToDrug.Visible && common.myLen(txtCommentsDrugToDrug.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug To Drug Interaction Comments can't be blank ! ";
        }
        if (spnCommentsDrugHealth.Visible && common.myLen(txtCommentsDrugHealth.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug Health Interaction Comments can't be blank ! ";
        }
        if (common.myLen(lblIntreactionMessage.Text).Equals(0))
        {
            dvInteraction.Visible = false;
        }
    }
    protected void btnInteractionCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            hdnGenericId.Value = "0";
            hdnGenericName.Value = string.Empty;
            hdnItemId.Value = "0";
            hdnItemName.Value = string.Empty;
            ddlIndentType.SelectedIndex = 0;
            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = "0";
            Removeitem();
            dvInteraction.Visible = false;
        }
        catch
        { }
    }
    private void setVisiblilityInteraction()
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        clsCIMS objCIMS = new clsCIMS();
        Hashtable collVitalItemIdFound = new Hashtable();
        Hashtable collVitalItemIdFoundDH = new Hashtable();
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                lnkDrugAllergy.Visible = false;
                string strXMLDD = getInterationXML(string.Empty);//DrugToDrug
                string strXMLDH = getHealthOrAllergiesInterationXML("H", string.Empty);//Helth
                string strXMLDA = getHealthOrAllergiesInterationXML("A", string.Empty);//Allergies
                string strXMDI = getDIInterationXML(string.Empty);//Drug to Ingredient
                string outputValuesDD = string.Empty;
                string outputValuesDH = string.Empty;
                string outputValuesDA = string.Empty;
                string outputValuesDI = string.Empty;
                if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                {
                    outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                    outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                    outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);
                    outputValuesDI = objCIMS.getFastTrack5Output(strXMDI);
                    foreach (GridViewRow dataItem in gvPrevious.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            //Duplicate Ingredient
                            LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                            lnkBtnDIInteractionCIMS.Visible = false;
                            lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));


                            LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            lnkBtnDHInteractionCIMS.Visible = false;
                            lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                            LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            lnkBtnDAInteractionCIMS.Visible = false;
                            lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                            if (!string.IsNullOrEmpty(outputValuesDD))
                            {
                                if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                                {
                                    if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(outputValuesDI))
                            {
                                if (objCIMS.IsDuplicateInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDI, false) > 0)
                                {
                                    if (outputValuesDI.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDIInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(outputValuesDH))
                            {
                                if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                                {
                                    if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDHInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(outputValuesDA))
                            {
                                if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDA) > 0)
                                {
                                    if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDAInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    foreach (GridViewRow dataItem in gvItem.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            //Duplicate Ingredient
                            LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                            lnkBtnDIInteractionCIMS.Visible = false;
                            lnkBtnDIInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DuplicateIngredient"]));


                            LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            lnkBtnDHInteractionCIMS.Visible = false;
                            lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                            LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            lnkBtnDAInteractionCIMS.Visible = false;
                            lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
                            if (!string.IsNullOrEmpty(outputValuesDD))
                            {
                                if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                                {
                                    if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(outputValuesDI))
                            {
                                if (objCIMS.IsDuplicateInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDI, false) > 0)
                                {
                                    if (outputValuesDI.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDIInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(outputValuesDH))
                            {
                                if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                                {
                                    if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDHInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(outputValuesDA))
                            {
                                if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDA) > 0)
                                {
                                    if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDAInteractionCIMS.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                //int count = 0;
                //int rIdx = 0;
                //int rIdxDataFound = 0;
                //foreach (GridViewRow dataItem in gvStore.Rows)
                //{
                //    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                //    if (common.myStr(CIMSItemId.Value).Trim().Length > 0)
                //    {
                //        if (rIdxDataFound == 0)
                //        {
                //            rIdxDataFound = count;
                //        }
                //        rIdx++;
                //    }
                //    count++;
                //}
                //if (rIdx == 1)
                //{
                //    LinkButton lnkBtnInteractionCIMS = (LinkButton)gvStore.Rows[rIdxDataFound].FindControl("lnkBtnInteractionCIMS");
                //    if (lnkBtnInteractionCIMS.Visible)
                //    {
                //        lnkBtnInteractionCIMS.Visible = false;
                //    }
                //}
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                lnkDrugAllergy.Visible = false;
                if (commonNameGroupIds.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    collVitalItemIdFound = new Hashtable();
                    sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);
                    DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
                    //int? weight = common.myInt(lbl_Weight.Text);//In kilograms 
                    //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
                    int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
                    int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value
                    collVitalItemIdFoundDH = new Hashtable();
                    StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                                    (!common.myStr(ViewState["PatientDiagnosisXML"]).Equals(string.Empty)) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                                    out collVitalItemIdFoundDH);
                    foreach (GridViewRow dataItem in gvItem.Rows)
                    {
                        HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                        if (common.myInt(VIDALItemId.Value) > 0)
                        {
                            LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                            LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
                            if (collVitalItemIdFound.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnInteractionVIDAL.Visible = false;
                            }
                            if (sbDHI.ToString().Length > 0 || collVitalItemIdFoundDH.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnDHInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnDHInteractionVIDAL.Visible = false;
                            }
                        }
                    }
                    foreach (GridViewRow dataItem in gvPrevious.Rows)
                    {
                        HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                        if (common.myInt(VIDALItemId.Value) > 0)
                        {
                            LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                            LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
                            if (collVitalItemIdFound.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnInteractionVIDAL.Visible = false;
                            }
                            if (sbDHI.ToString().Length > 0 || collVitalItemIdFoundDH.ContainsValue(common.myInt(VIDALItemId.Value)))
                            {
                                lnkBtnDHInteractionVIDAL.Visible = true;
                            }
                            else
                            {
                                lnkBtnDHInteractionVIDAL.Visible = false;
                            }
                        }
                    }
                    int?[] allergyIds = null; //new int?[] { 114 };
                    int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };
                    if (!common.myStr(ViewState["PatientAllergyXML"]).Equals(string.Empty))
                    {
                        allergyIds = (int?[])ViewState["PatientAllergyXML"];
                    }
                    sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);
                    if (sb.ToString().Length > 0)
                    {
                        lnkDrugAllergy.Visible = true;
                    }
                    else
                    {
                        lnkDrugAllergy.Visible = false;
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
            objVIDAL = null;
            objCIMS = null;
            collVitalItemIdFound = null;
            collVitalItemIdFoundDH = null;
        }
    }
    private string getBrandDetailsXMLCIMS(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //  <Detail>
            //    <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}">
            //      <Items />
            //      <Packages />
            //      <Images />
            //      <TherapeuticClasses />
            //      <ATCCodes />
            //      <Companies />
            //      <Identifiers />
            //    </Product>
            //  </Detail>
            //</Request>
            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
            strXML = "<Request><Detail><" + CIMSType + " reference=\"" + CIMSItemId + "\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getMonographXML(string CIMSType, string CIMSItemId)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Content>
            //        <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //    </Content>
            //</Request>
            //strXML = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";
            // <MONOGRAPH>
            CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
            strXML = "<Request><Content><" + CIMSType + " reference=\"" + CIMSItemId + "\" /></Content></Request>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }

    private string getDIInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <DuplicateTherapy />
                //        <DuplicateIngredient />
                //        <References/>
                //    </Interaction>
                //</Request>
                string strPrescribing = string.Empty;
                StringBuilder ItemIds = new StringBuilder();
                if (!strNewPrescribing.Equals(string.Empty))
                {
                    ItemIds.Append(strNewPrescribing);
                }
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton DIInteractionVIDAL = (LinkButton)dataItem.FindControl("DIInteractionVIDAL");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnDIInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDIInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }
                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><DuplicateTherapy /><DuplicateIngredient /><References /></Interaction></Request>";

                //strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getInterationXML(string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                //<Request>
                //    <Interaction>
                //        <Prescribing>
                //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
                //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
                //        </Prescribing>
                //        <Allergies />
                //        <References/>
                //    </Interaction>
                //</Request>
                string strPrescribing = string.Empty;
                StringBuilder ItemIds = new StringBuilder();
                if (!strNewPrescribing.Equals(string.Empty))
                {
                    ItemIds.Append(strNewPrescribing);
                }
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    //&& lnkBtnInteractionCIMS.Visible
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }
                //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
                strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getHealthOrAllergiesInterationXML(string useFor, string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {
            //<Request>
            //    <Interaction>
            //        <Prescribing>
            //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //        </Prescribing>
            //        <Prescribed>
            //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
            //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
            //        </Prescribed>
            //        <Allergies>
            //            <Product reference="{8A4E15CD-ACE3-41D9-A367-55658256C2D4}" />
            //            <Product reference="{6D8F3E40-FA33-49C9-9D34-7C13F88E00FD}" />
            //        </Allergies>
            //        <HealthIssueCodes>
            //            <HealthIssueCode code="K22" codeType="ICD10" />
            //            <HealthIssueCode code="K22.0" codeType="ICD10" />
            //        </HealthIssueCodes>
            //        <References/>
            //    </Interaction>
            //</Request>
            string strPrescribing = string.Empty;
            StringBuilder ItemIds = new StringBuilder();
            if (!strNewPrescribing.Equals(string.Empty))
            {
                ItemIds.Append(strNewPrescribing);
            }
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                if (common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0"))
                {
                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                    if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                    {
                        ItemIds.Append(strPres);
                    }
                }
            }
            foreach (GridViewRow dataItem in gvItem.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                if (common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0"))
                {
                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                    if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                    {
                        ItemIds.Append(strPres);
                    }
                }
            }
            if (ItemIds.ToString().Equals(string.Empty))
            {
                return string.Empty;
            }
            strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
            switch (useFor)
            {
                case "H"://Helth Interaction
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
                case "A"://Allergies
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + "<References /></Interaction></Request>";
                    break;
                case "B"://Both
                    strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    private string getRunningInterationXML()
    {
        string strXML = string.Empty;
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                string strPrescribing = string.Empty;
                StringBuilder ItemIds = new StringBuilder();
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
                        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
                        {
                            ItemIds.Append(strPres);
                        }
                    }
                }
                if (ItemIds.ToString().Equals(string.Empty))
                {
                    return string.Empty;
                }
                strXML = ItemIds.ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return strXML;
    }
    protected void lnkDrugAllergy_OnClick(object sender, EventArgs e)
    {
        showHealthOrAllergiesIntreraction("A");
    }
    private void openWindowsCIMS(bool IsBrandDetails)
    {
        clsCIMS objCIMS = new clsCIMS();
        try
        {
            hdnCIMSOutput.Value = objCIMS.getCIMSFinalOutupt(IsBrandDetails);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "OpenCIMSWindow();", true);
            return;
        }
        catch
        {
        }
        finally
        {
            objCIMS = null;
        }
    }
    ///////////////VIDAL/////////////////////////////
    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();
            foreach (GridViewRow dataItem in gvItem.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    list.Add(common.myInt(VIDALItemId.Value));
                }
            }
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    list.Add(common.myInt(VIDALItemId.Value));
                }
            }
            commonNameGroupIds = list.ToArray();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return commonNameGroupIds;
    }
    private void getBrandDetailsVidal(int? commonNameGroupId)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        DataTable tbl = new DataTable();
        try
        {
            tbl = objVIDAL.getVIDALBrandDetails(commonNameGroupId);
            if (tbl.Rows.Count > 0)
            {
                openWindowsVIDAL("?UseFor=MO&URL=" + common.myStr(tbl.Rows[0]["URL"]));
            }
            //ViewState["tblMonographVidal"] = tbl;
            //gvMonographVidal.DataSource = tbl;
            //gvMonographVidal.DataBind();
            //DivMonographVidal.Visible = false;
            //if (tbl.Rows.Count > 0)
            //{
            //    DivMonographVidal.Visible = true;
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
            objVIDAL = null;
            tbl.Dispose();
        }
    }
    private void getMonographVidal(int? commonNameGroupId)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        DataTable tbl = new DataTable();
        try
        {
            tbl = objVIDAL.getVIDALMonograph(commonNameGroupId);
            if (tbl.Rows.Count > 0)
            {
                openWindowsVIDAL("?UseFor=MO&URL=" + common.myStr(tbl.Rows[0]["URL"]));
            }
            //ViewState["tblMonographVidal"] = tbl;
            //gvMonographVidal.DataSource = tbl;
            //gvMonographVidal.DataBind();
            //DivMonographVidal.Visible = false;
            //if (tbl.Rows.Count > 0)
            //{
            //    DivMonographVidal.Visible = true;
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
            objVIDAL = null;
            tbl.Dispose();
        }
    }
    private void getDrugToDrugInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        Hashtable collVitalItemIdFound = new Hashtable();
        try
        {
            //commonNameGroupIds = new int?[] { 15223, 15070, 1524, 4025, 4212, 516 };
            StringBuilder sb = new StringBuilder();

            sb = objVIDAL.getVIDALDrugToDrugInteraction(false, commonNameGroupIds, out collVitalItemIdFound);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                openWindowsVIDAL("?UseFor=IN");
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
            objVIDAL = null;
            collVitalItemIdFound = null;
        }
    }
    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        Hashtable collVitalItemIdFoundDH = new Hashtable();
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };
            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(lbl_Weight.Text);//In kilograms
            //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                     (!common.myStr(ViewState["PatientDiagnosisXML"]).Equals(string.Empty)) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                    out collVitalItemIdFoundDH);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                openWindowsVIDAL("?UseFor=HI");
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
            objVIDAL = null;
            collVitalItemIdFoundDH = null;
        }
    }
    private void getDrugAllergyVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //commonNameGroupIds = new int?[] { 4025, 4212, 516 };
            int?[] allergyIds = null; //new int?[] { 114 };
            int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };
            if (!common.myStr(ViewState["PatientAllergyXML"]).Equals(string.Empty))
            {
                allergyIds = (int?[])ViewState["PatientAllergyXML"];
            }
            StringBuilder sb = new StringBuilder();
            sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                openWindowsVIDAL("?UseFor=DA");
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
            objVIDAL = null;
        }
    }
    private void getWarningVidal(int? commonNameGroupId)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            StringBuilder sb = new StringBuilder();
            sb = objVIDAL.getVIDALDrugWarning(commonNameGroupId);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                openWindowsVIDAL("?UseFor=WS");
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
            objVIDAL = null;
        }
    }
    private void getSideEffectVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            StringBuilder sb = new StringBuilder();
            sb = objVIDAL.getVIDALDrugSideEffect(commonNameGroupIds);
            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }
            if (!sb.ToString().Equals(string.Empty))
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                openWindowsVIDAL("?UseFor=SE");
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
            objVIDAL = null;
        }
    }
    private void openWindowsVIDAL(string parameters)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/MonographVidal.aspx" + parameters;
        RadWindow1.Height = 580;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        if (parameters.Contains("UseFor=MO"))
        {
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
        RadWindow1.VisibleStatusbar = false;
    }
    protected void gvItem_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (chkShowDetails.Checked)
            {
                //if (common.myBool(Session["IsCIMSInterfaceActive"])
                //    || common.myBool(Session["IsVIDALInterfaceActive"]))
                //{
                //    string strInterface = "CIMS";
                //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                //    {
                //        strInterface = "CIMS";
                //    }
                //    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //    {
                //        strInterface = "VIDAL";
                //    }
                //    GridView HeaderGrid = (GridView)sender;
                //    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                //    TableCell Cell_Header = new TableCell();
                //    Cell_Header.Text = "";
                //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                //    Cell_Header.ColumnSpan = 2;
                //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                //    HeaderRow.Cells.Add(Cell_Header);
                //    Cell_Header = new TableCell();
                //    Cell_Header.Text = strInterface + " Information";
                //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                //    Cell_Header.ForeColor = System.Drawing.Color.Red;
                //    Cell_Header.Font.Bold = true;
                //    Cell_Header.ColumnSpan = 5;
                //    HeaderRow.Cells.Add(Cell_Header);
                //    Cell_Header = new TableCell();
                //    Cell_Header.Text = "";
                //    Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                //    Cell_Header.ColumnSpan = 2;
                //    Cell_Header.BackColor = System.Drawing.Color.FromName(common.myStr("#81BEF7"));
                //    HeaderRow.Cells.Add(Cell_Header);
                //    gvItem.Controls[0].Controls.AddAt(0, HeaderRow);
                //}
            }
        }
    }
    protected void chkShowDetails_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DIInteractionCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = chkShowDetails.Checked;
            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = chkShowDetails.Checked;
            }
            setGridColor();
            //setVisiblilityInteraction();
        }
        catch
        {
        }
    }
    private void setDefaultFavourite(int ItemId)
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        int GenericId = 0;
        int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
        if (DoctorId.Equals(0))
        {
            DoctorId = common.myInt(Session["LoginDoctorId"]);
        }
        try
        {
            if (ItemId.Equals(0))
            {
                return;
            }
            ds = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
                                GenericId, DoctorId, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];
                    if (common.myInt(DR["Dose"]) > 0)
                    {
                        txtDose.Text = common.myInt(DR["Dose"]).ToString();
                    }
                    if (common.myInt(DR["UnitId"]) > 0)
                    {
                        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["UnitId"]).ToString()));
                        //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindByValue(common.myInt(DR["UnitId"]).ToString()));
                    }
                    //if (common.myInt(DR["StrengthId"]) > 0)
                    //{
                    //    ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
                    //}

                    ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(DR["StrengthValue"])));
                    ddlStrengthValue.Text = common.myStr(DR["StrengthValue"]);
                    if (common.myInt(DR["FormulationId"]) > 0)
                    {
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                        //ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindByValue(common.myInt(DR["FormulationId"]).ToString()));
                        ddlFormulation_OnSelectedIndexChanged(this, null);
                    }
                    if (common.myInt(DR["RouteId"]) > 0)
                    {
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                        //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(DR["RouteId"]).ToString()));
                    }
                    if (common.myInt(DR["FrequencyId"]) > 0)
                    {
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DR["FrequencyId"]).ToString()));
                        //ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindByValue(common.myInt(DR["FrequencyId"]).ToString()));
                    }
                    if (common.myInt(DR["Duration"]) > 0)
                    {
                        txtDuration.Text = common.myInt(DR["Duration"]).ToString();
                    }
                    if (!common.myStr(DR["DurationType"]).Equals(string.Empty))
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DR["DurationType"]).ToString()));
                        //ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindByValue(common.myStr(DR["DurationType"]).ToString()));
                        endDateChange();
                    }
                    if (common.myInt(DR["FoodRelationshipId"]) > 0)
                    {
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DR["FoodRelationshipId"]).ToString()));
                        //ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindByValue(common.myInt(DR["FoodRelationshipId"]).ToString()));
                    }
                    if (common.myDbl(txtDose.Text).Equals(0.0))
                    {
                        txtDose.Text = string.Empty;//"1"
                    }
                    if (common.myStr(Session["OPIP"]).Equals("I"))
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            txtDuration.Text = string.Empty;//"1"
                        }
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
            ds.Dispose();
            objEMR = null;
        }
    }
    protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
    {
        try
        {
            addItem(common.myStr(ViewState["ICDCodes"]));
            if (common.myStr(ViewState["ProceedValue"]).Equals("F"))
            {
                proceedFavourite(true);
            }
            else if (common.myStr(ViewState["ProceedValue"]).Equals("C"))
            {
                proceedCurrent(true);
            }
            else
            {
                proceedCurrent(true);
            }
            dvConfirmAlreadyExistOptions.Visible = false;
            //dvExpensiveDrugs.Visible = false;

        }
        catch
        {
        }
    }
    protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmAlreadyExistOptions.Visible = false;
        //dvExpensiveDrugs.Visible = false;
    }
    protected void txtICDCode_TextChanged(object sender, EventArgs e)
    {
        ViewState["ICDCodes"] = null;
        if (!common.myStr(txtICDCode.Text).Equals(string.Empty))
        {
            ViewState["ICDCodes"] = common.myStr(txtICDCode.Text);
        }
    }
    protected void ddlPrescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            lblMessage.Text = string.Empty;
            if (common.myInt(ddlPrescription.SelectedValue) > 0)
            {
                //btnCancel.Visible = true;
                btnPrint.Visible = false;
                //btnCopyLastPrescription.Enabled = false;
            }
            else
            {
                //btnCancel.Visible = false;
                btnPrint.Visible = false;
                //btnCopyLastPrescription.Enabled = true;
            }
            //spnPrescriptionRemarks.Visible = false;
            hdnGenericId.Value = "0";
            hdnGenericName.Value = string.Empty;
            hdnItemId.Value = "0";
            hdnItemName.Value = string.Empty;
            ddlGeneric.Text = string.Empty;
            ddlBrand.Text = string.Empty;
            txtCustomMedication.Text = string.Empty;
            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = string.Empty;
            ddlIndentType.SelectedIndex = 0;
            //chkNoMedicine.Checked = false;
            clearItemDetails();
            ds = objEMR.getMedicinesOPList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(ViewState["EncId"]), common.myInt(ddlPrescription.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];
                // ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByValue(common.myStr(DR["IndentType"])));
                ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindByValue(common.myStr(DR["IndentType"])));
                ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myStr(DR["AdvisingDoctorId"])));
                //ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindByValue(common.myStr(DR["AdvisingDoctorId"])));
                //txtPrescriptionRemarks.Text = common.myStr(DR["Remarks"]);
                if (common.myStr(DR["Remarks"]).Trim().Length > 0)
                {
                    //spnPrescriptionRemarks.Visible = true;
                }
                //chkNoMedicine.Checked = common.myBool(DR["IsNoCurrentMedications"]);
            }
            //bindGridData(common.myInt(ddlPrescription.SelectedValue));           
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
            objEMR = null;
        }
    }
    protected void btnPrintYes_OnClick(object sender, EventArgs e)
    {
        int PrescriptionId = common.myInt(Session["CurentSavedPrescriptionId"]);
        if (PrescriptionId.Equals(0))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Prescription not Selected !";
            dvConfirmPrint.Visible = false;
            return;
        }
        RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(ViewState["EncId"]) + "&PId=" + PrescriptionId;
        RadWindow1.Height = 600;
        RadWindow1.Width = 800;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;
        dvConfirmPrint.Visible = false;
    }
    protected void btnPrintNo_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrint.Visible = false;
    }
    protected void btnHelp_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "GenericHelp.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.Title = "Generic Help";
        RadWindow1.OnClientClose = "OnClientGenericHelpClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = true;
    }
    private string PostePrescription(int PrescriptionID, string XMLflag, out string refno)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsdoctor = new DataSet();
        string outerrormessage = string.Empty;
        refno = "0";
        try
        {
            //if (!PatientandClinicianValidateion().Equals(string.Empty))
            //{
            //    dverx.Visible = true;
            //    refno = "0";
            //    return "Failed";
            //}

            //string strReturnXML = objEMR.GenerateeOPRxXml(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //             common.myInt(PrescriptionID), XMLflag);

            //string filename = PrescriptionID.ToString() + "_" + DateTime.Now.ToString("ddMMyyHHmm") + ".xml";
            //string fileLoc = Server.MapPath("~/PatientDocuments/" + filename);
            //FileStream fs = null;
            //if (File.Exists(fileLoc))
            //{
            //    File.Delete(fileLoc);
            //}
            //try
            //{
            //    string strxmltxt = common.myStr(strReturnXML);
            //    var xdoc = new XmlDocument();
            //    xdoc.LoadXml(strxmltxt);
            //    xdoc.Save(fileLoc);
            //}
            //catch (Exception ex)
            //{
            //    Alert.ShowAjaxMsg(ex.Message, this);
            //}

            //int outputrefno = 0;
            //byte[] outfile;


            //string uname = common.myStr(hdneclaimWebServiceLoginID.Value);
            //string password = common.myStr(hdneclaimWebServicePassword.Value);
            //NewErx.eRxValidateTransaction vt = new NewErx.eRxValidateTransaction();
            ////int postrncount = vt.UploadERxRequest(uname, password, "DHA-P-0242455", "DHA-P-0242455", common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);

            //dsdoctor = objEMR.getClinicianLoginforErx(common.myInt(Session["UserID"]));
            //if (dsdoctor.Tables[0].Rows.Count > 0)
            //{
            //    string cluname = common.myStr(dsdoctor.Tables[0].Rows[0]["UserName"]);
            //    string clpwd = common.myStr(dsdoctor.Tables[0].Rows[0]["Password"]);
            //    int trncount = vt.UploadERxRequest(uname, password, cluname, clpwd, common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
            //    //int trncount = vt.UploadERxRequest("Aster002", "insurance002", cluname, clpwd, common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
            //    refno = outputrefno.ToString();
            //}
            //else
            //{
            //    Alert.ShowAjaxMsg("No Doctor Login Found", this);
            //    refno = "0";
            //    return "No Doctor Login Found";
            //}
            //if (outfile != null)
            //{
            //    string erfilename = Server.MapPath("~/PatientDocuments/Error.Csv");
            //    if (File.Exists(erfilename))
            //    {
            //        File.Delete(erfilename);
            //    }
            //    Response.BinaryWrite(outfile);
            //    common.ByteArrayToFile(erfilename, outfile, out erfilename);
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
            dsdoctor.Dispose();
        }
        return outerrormessage;
    }
    protected void gvOrderSet_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECTORDERSET"))
            {
                lblMessage.Text = string.Empty;
                ViewState["OrderSetId"] = common.myInt(e.CommandArgument).ToString();
                AddOrderSet(common.myStr(ViewState["ICDCodes"]));
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
            objEMR = null;
        }
    }
    protected void btnCopyLastPrescription_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dtData1 = new DataTable();
        DataTable dtData2 = new DataTable();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            ViewState["Stop"] = true;
            //if (common.myStr(ViewState["PatientOPIPType"]) == "I")
            //{
            //    ds = objEMR.getCopyPreviousMedicinesIP(common.myInt(Session["HospitalLocationID"]),
            //                        common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
            //                        common.myInt(ViewState["EncId"]), common.myInt(Session["UserId"]), common.myInt(Session["UserId"]));
            //}
            //else
            //{
            ds = objEMR.getCopyPreviousMedicinesOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(Session["UserId"]));
            //}
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record not found!";
                ds.AcceptChanges();
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ViewState["Item"] = null;
                ViewState["ItemDetail"] = null;
                //Comment By Himanshu On Date 02/03/2022
                //BindBlankItemGrid();
            }
            else
            {
                try
                {
                    dtData1 = ds.Tables[0].Copy();
                    dtData1 = addColumnInItemGrid(dtData1);

                    if (dtData1 != null)
                    {
                        if (dtData1.Columns.Count > 0)
                        {
                            if (!dtData1.Columns.Contains("ItemFlagName"))
                            {
                                dtData1.Columns.Add("ItemFlagName", typeof(string));
                            }
                            if (!dtData1.Columns.Contains("ItemFlagCode"))
                            {
                                dtData1.Columns.Add("ItemFlagCode", typeof(string));
                            }
                        }
                    }

                    ViewState["Item"] = dtData1;

                    dtData2 = ds.Tables[1].Copy();
                    dtData2 = addColumnInItemGrid(dtData2);
                    ViewState["ItemDetail"] = dtData2;
                    hdnCopyLastPresc.Value = "1";
                    foreach (DataRow item in dtData2.Rows)
                    {
                        if (common.myInt(item["UnAppPrescriptionId"]).Equals(0))
                        {
                            int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(item);
                            item["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                        }
                    }

                    gvItem.DataSource = dtData1;
                    gvItem.DataBind();
                }
                catch
                {
                }
            }
            setVisiblilityInteraction();
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
            dtData1.Dispose();
            dtData2.Dispose();
            objEMR = null;
        }
    }
    protected void bindUnApprovedPrescriptions()
    {
        DataTable tbl = new DataTable();
        try
        {
            if (ViewState["UnApprovedPrescriptions"] != null)
            {
                tbl = (DataTable)ViewState["UnApprovedPrescriptions"];
                if (tbl != null)
                {
                    if (tbl.Rows.Count > 0)
                    {
                        ViewState["Item"] = tbl;
                        ViewState["ItemDetail"] = tbl;
                        gvItem.DataSource = tbl;
                        gvItem.DataBind();
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
            tbl.Dispose();
            ViewState["UnApprovedPrescriptions"] = null;
        }
    }
    private DataTable addColumnInItemGrid(DataTable dtData)
    {
        try
        {
            if (!dtData.Columns.Contains("UnAppPrescriptionId"))
            {
                dtData.Columns.Add("UnAppPrescriptionId", typeof(int));
            }
            if (!dtData.Columns.Contains("DurationType"))
            {
                dtData.Columns.Add("DurationType", typeof(string));
            }
            if (!dtData.Columns.Contains("InstructionRemarks"))
            {
                dtData.Columns.Add("InstructionRemarks", typeof(string));
            }
            if (!dtData.Columns.Contains("ClosingBalanceQty"))
            {
                dtData.Columns.Add("ClosingBalanceQty", typeof(int));
            }
            //added by bhakti
            if (!dtData.Columns.Contains("ItemFlagName"))
            {
                dtData.Columns.Add("ItemFlagName", typeof(string));
            }
            if (!dtData.Columns.Contains("ItemFlagCode"))
            {
                dtData.Columns.Add("ItemFlagCode", typeof(string));
            }
        }
        catch
        {
        }
        return dtData;
    }
    private int saveAsUnApprovedPrescriptions(DataRow DRData)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable dtData = new DataTable();
        string ReturnUnAppPrescriptionId = common.myInt(hdnMainUnAppPrescriptionId.Value).ToString();
        try
        {
            if (DRData != null)
            {
                //if (common.myInt(DRData["RouteId"]) > 0 || common.myInt(ViewState["OrderSetId"]) > 0)
                //{
                string PrescriptionDetail = common.myStr(DRData["PrescriptionDetail"]);
                if (common.myLen(PrescriptionDetail).Equals(0))
                {
                    dtData = DRData.Table.Clone();
                    dtData.Rows.Add(DRData.ItemArray);
                    PrescriptionDetail = objEMR.GetPrescriptionDetailStringNew(dtData, chkFractionalDose.Checked ? "Y" : "N");
                }
                if (!DRData.Table.Columns.Contains("FrequencyName"))
                {
                    DRData.Table.Columns.Add("FrequencyName", typeof(string));
                }

                string strMsg = objEMR.SaveUnApprovedPrescriptions(common.myInt(hdnMainUnAppPrescriptionId.Value), common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncId"]), common.myInt(DRData["IndentId"]),
                                    common.myStr(DRData["IndentNo"]).Equals("0") ? string.Empty : common.myStr(DRData["IndentNo"]),
                                    !common.myStr(DRData["IndentDate"]).Equals(string.Empty) ? common.myDate(DRData["IndentDate"]).ToString("yyyy-MM-dd") : null,
                                    common.myInt(DRData["IndentTypeId"]),
                                    common.myStr(DRData["IndentType"]), common.myInt(DRData["GenericId"]), common.myStr(DRData["GenericName"]),
                                    common.myInt(DRData["ItemId"]), common.myStr(DRData["ItemName"]), common.myBool(DRData["CustomMedication"]),
                                    common.myInt(DRData["FrequencyId"]), common.myStr(DRData["FrequencyName"]), common.myDbl(DRData["Frequency"]),
                                    common.myDbl(DRData["Dose"]), common.myStr(DRData["Duration"]), common.myStr(DRData["DurationText"]),
                                    common.myStr(DRData["Instructions"]), common.myInt(DRData["UnitId"]), common.myStr(DRData["UnitName"]),
                                    common.myStr(DRData["Type"]),
                                    !common.myStr(DRData["StartDate"]).Equals(string.Empty) ? common.myDate(DRData["StartDate"]).ToString("yyyy-MM-dd") : null,
                                    !common.myStr(DRData["EndDate"]).Equals(string.Empty) ? common.myDate(DRData["EndDate"]).ToString("yyyy-MM-dd") : null,
                                    common.myStr(DRData["CIMSItemId"]), common.myStr(DRData["CIMSType"]), common.myInt(DRData["VIDALItemId"]),
                                    string.Empty,//common.myStr(DRData["XMLData"]),
                                    PrescriptionDetail, common.myInt(DRData["FormulationId"]),
                                    common.myStr(DRData["FormulationName"]), common.myInt(DRData["RouteId"]), common.myStr(DRData["RouteName"]),
                                    common.myInt(DRData["StrengthId"]), common.myStr(DRData["StrengthValue"]), common.myDbl(DRData["Qty"]),
                                    common.myStr(DRData["FoodRelationship"]), common.myInt(DRData["FoodRelationshipID"]), common.myInt(DRData["ReferanceItemId"]),
                                    common.myStr(DRData["ReferanceItemName"]), common.myInt(DRData["DoseTypeId"]), common.myStr(DRData["DoseTypeName"]),
                                    common.myBool(DRData["NotToPharmacy"]), common.myBool(DRData["IsInfusion"]), common.myBool(DRData["IsInjection"]),
                                    false, false, common.myStr(DRData["Volume"]), common.myInt(DRData["VolumeUnitId"]), common.myStr(DRData["TotalVolume"]),
                                    common.myStr(DRData["InfusionTime"]), common.myStr(DRData["TimeUnit"]), common.myInt(DRData["FlowRate"]),
                                    common.myInt(DRData["FlowRateUnit"]), string.Empty, common.myStr(DRData["XmlVariableDose"]),
                                    false, common.myStr(DRData["OverrideComments"]), common.myStr(DRData["OverrideCommentsDrugToDrug"]),
                                    common.myStr(DRData["OverrideCommentsDrugHealth"]), common.myStr(DRData["XMLFrequencyTime"]),
                                    common.myBool(DRData["IsSubstituteNotAllowed"]), common.myStr(DRData["ICDCode"]), common.myInt(Session["UserId"]),
                                    ref ReturnUnAppPrescriptionId, common.myInt(DRData["ClosingBalanceQty"]));
                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
                {
                    hdnMainUnAppPrescriptionId.Value = null;
                }
                //   }
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
            dtData.Dispose();
            objEMR = null;
        }
        return common.myInt(ReturnUnAppPrescriptionId);
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (common.myBool(ViewState["HighValueItem"]))
            //{
            //    string strMsg = "This is high value item do you want to proceed";
            //    //Alert.ShowAjaxMsg(strMsg, this.Page);
            //    //clearItemDetails();
            //}

            dvConfirm.Visible = false;
            txtDose.Focus();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {
            dvConfirm.Visible = false;
            lblConfirmHighValue.Text = "This is high value item do you want to proceed ";
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            clearItemDetails();
            //ddlBrand.Items.Clear();
            //ddlBrand.SelectedIndex  = 0;
            //  ddlBrand.Focus();
            ddlBrand.ClearSelection();
            ddlBrand.Items.Clear();
            ddlBrand.Text = string.Empty;
            ddlBrand.Enabled = true;
            ddlBrand.SelectedValue = null;

            dvConfirm.Visible = false;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {

            dvConfirm.Visible = false;
            lblConfirmHighValue.Text = "This is high value item do you want to proceed ";

        }


    }


    protected void btngenerateInstruction_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        string Instruction = "";

        if (!common.myStr(txtmonday.Text).Equals(string.Empty))
        {
            Instruction += "M: " + objEMR.convertNumberToFraction(txtmonday.Text) + "mg, ";
        }

        if (!common.myStr(txttuesday.Text).Equals(string.Empty))
        {
            Instruction += "T: " + objEMR.convertNumberToFraction(txttuesday.Text) + "mg, ";
        }

        if (!common.myStr(txtW.Text).Equals(string.Empty))
        {
            Instruction += "W: " + objEMR.convertNumberToFraction(txtW.Text) + "mg, ";
        }

        if (!common.myStr(txtth.Text).Equals(string.Empty))
        {
            Instruction += "Th: " + objEMR.convertNumberToFraction(txtth.Text) + "mg, ";
        }

        if (!common.myStr(txtF.Text).Equals(string.Empty))
        {
            Instruction += "F: " + objEMR.convertNumberToFraction(txtF.Text) + "mg, ";
        }

        if (!common.myStr(txtsa.Text).Equals(string.Empty))
        {
            Instruction += "Sa: " + objEMR.convertNumberToFraction(txtsa.Text) + "mg, ";
        }

        if (!common.myStr(txtsu.Text).Equals(string.Empty))
        {
            Instruction += "Su: " + objEMR.convertNumberToFraction(txtsu.Text) + "mg,";
        }

        txtInstructions.Text = Instruction.Trim(',');

    }
    public void CheckgenerateInstruction(int ddlBrandId)
    {
        try
        {
            clsIVF objclsIVF = new clsIVF(sConString);

            if (objclsIVF.GetCheckgenerateInstruction(ddlBrandId))
            {
                divgenerateInstruction.Visible = true;
                btngenerateInstruction.Visible = true;
            }
            else
            {
                divgenerateInstruction.Visible = false;
                btngenerateInstruction.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
    }

    protected void btnOrderSet_OnClick(object sender, EventArgs e)
    {
        Session["MedicationSetItemIds"] = null;
        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        int genericId = 0;
        int itemId = 0;

        try
        {
            foreach (GridViewRow row in gvItem.Rows)
            {
                HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");

                itemId = common.myInt(hdnItemId.Value);
                genericId = common.myInt(hdnGenericId.Value);

                if (itemId.Equals(0) && genericId.Equals(0))
                {
                    break;
                }

                if (itemId > 0)
                {
                    genericId = 0;
                }
                if (genericId > 0)
                {
                    itemId = 0;
                }

                if (itemId > 0 || genericId > 0)
                {
                    HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                    HiddenField hdnDuration = (HiddenField)row.FindControl("hdnDuration");
                    HiddenField hdnDurationType = (HiddenField)row.FindControl("hdnDurationType");
                    HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                    HiddenField hdnUnitId = (HiddenField)row.FindControl("hdnUnitId");
                    HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                    HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                    HiddenField hdnFoodRelationshipId = (HiddenField)row.FindControl("hdnFoodRelationshipId");
                    HiddenField hdnRemarks = (HiddenField)row.FindControl("hdnRemarks");

                    coll.Add(0);//SetId SMALLINT,
                    coll.Add(itemId);//ItemID INT,
                    coll.Add(common.myInt(hdnFrequencyId.Value));//FrequencyId TINYINT,
                    coll.Add(common.myInt(hdnDuration.Value));//Days VARCHAR(100),
                    coll.Add(common.myStr(hdnDurationType.Value));//DaysType CHAR(1) ,
                    coll.Add(common.myDbl(hdnDose.Value));//Dose decimal,
                    coll.Add(common.myStr(hdnUnitId.Value));//DoseUnitId int,
                    coll.Add(common.myInt(hdnFormulationId.Value));//FormulationId int,
                    coll.Add(common.myInt(hdnRouteId.Value));//RouteId int
                    coll.Add(common.myInt(hdnFoodRelationshipId.Value));//FoodID SMALLINT
                    coll.Add(common.myStr(hdnRemarks.Value));//InstructionRemarks VARCHAR(1000),
                    coll.Add(genericId);//GenericId INT

                    objXML.Append(common.setXmlTable(ref coll));
                }
            }


            if (common.myLen(objXML.ToString()) > 0)
            {
                Session["MedicationSetItemIds"] = objXML.ToString();

                RadWindow1.NavigateUrl = "/EMR/Orders/AddOrderSet.aspx?For=Medication&DoctorId=" + common.myInt(Session["EmployeeId"]);

                RadWindow1.Height = 500;
                RadWindow1.Width = 800;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = "AddOrderSet_OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                                     //RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Please add items in list before make medication set!", this.Page);
            }
        }
        catch
        {
        }
    }

    protected void btnAddOrderSetClose_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable DT = new DataTable();
        try
        {
            DT = objEMR.getMedicationSetMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["LoginDepartmentId"]), common.myInt(Session["EmployeeId"]), common.myStr(Session["OPIP"]));


            ViewState["OrderSetMaster"] = DT;

            txtOrderSetName.Text = string.Empty;

            gvOrderSet.DataSource = DT;
            gvOrderSet.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            DT.Dispose();
        }
    }

    #region Reason After 4th  Indent In a day
    private void CheckNoOfIndentInOneDay()
    {

        WardManagement objwd = new BaseC.WardManagement();
        DataTable dt = new DataTable();

        ViewState["IsReasonManditory"] = false;
        dvReason.Visible = false;

        string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();


        dt = objwd.CheckNoOfIndentInOneDay(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                     common.myInt(Session["UserID"]), common.myInt(sEncId));
        if ((common.myInt(dt.Rows.Count) > 0) && common.myStr(Session["OPIP"]).Equals("I"))
        {
            ViewState["IsReasonManditory"] = true;
            dvReason.Visible = true;
            BindDdlReason();

        }

    }



    private void BindDdlReason()
    {
        WardManagement objwd = new BaseC.WardManagement();
        DataTable dt = new DataTable();
        try
        {
            ddlReason.Items.Clear();

            dt = objwd.GetReasonMasterList(1, common.myInt(Session["FacilityId"]), "DO");
            if (common.myInt(dt.Rows.Count) > 0)
            {
                ddlReason.DataSource = dt;
                ddlReason.DataTextField = "ReasonName";
                ddlReason.DataValueField = "ReasonId";
                ddlReason.DataBind();
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
            objwd = null;
        }
    }

    #endregion


    protected void ChkReqestFromOtherWard_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkReqestFromOtherWard.Checked)
            {
                ddlReqestFromOtherWard.Visible = true;
                bindRequestFromOtherWard();
            }
            else
            {
                ddlReqestFromOtherWard.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void bindRequestFromOtherWard()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWD = new BaseC.WardManagement();
        try
        {
            ds = objWD.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));

            ddlReqestFromOtherWard.DataSource = ds.Tables[0];
            ddlReqestFromOtherWard.DataTextField = "WardName";
            ddlReqestFromOtherWard.DataValueField = "WardId";
            ddlReqestFromOtherWard.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objWD = null;
            ds.Dispose();
        }

    }

    //Akshay_Tirathram
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            proceedCurrent(true);
            dvExpensiveDrugs.Visible = false;

        }
        catch
        {
        }
    }

    // Akshay_Tirathram
    protected void btnCancels_Click(object sender, EventArgs e)
    {
        dvExpensiveDrugs.Visible = false;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvNarcoticDrugsPopup.Visible = false;
    }
}
