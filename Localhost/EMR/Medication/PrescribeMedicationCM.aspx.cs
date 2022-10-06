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

public partial class EMR_Medication_PrescribeMedicationCM : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private enum enumColumns : byte
    {
        BrandName = 0,
        Dose = 1,
        Unit = 2,
        Strength = 3,
        TotalQty = 4,
        BrandDetailsCIMS = 5,
        MonographCIMS = 6,
        InteractionCIMS = 7,
        DIInteractionCIMS = 8,
        DHInteractionCIMS = 9,
        DAInteractionCIMS = 10,
        BrandDetailsVIDAL = 11,
        MonographVIDAL = 12,
        InteractionVIDAL = 13,
        DHInteractionVIDAL = 14,
        DAInteractionVIDAL = 15,
        Delete = 16
    }

    private enum enumColumnsPrevious : byte
    {
        Sno = 0,
        Reorder = 1,
        IndentNo = 2,
        PrescriptionDetail = 3,
        InstructionRemarks = 4,
        IndentDate = 5,
        StartDate = 6,
        AddNormalDrug = 7,
        Cancel = 8,
        Stop = 9
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
        if (!IsPostBack)
        {
            try
            {
                Session["FacilityName"] = Session["FacilityName"];


                System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        "AllowZeroStockItemInIPPrescription,AllowZeroStockItemInOPPrescription,DefaultERIndentStoreId,DefaultOPIndentStoreId,DefaultOTIndentStoreId,DefaultWardIndentStoreId,eclaimWebServiceLoginID,eclaimWebServicePassword,EMRPrescriptionDoseShowInFractionalValue,EpresActive,IncludeToContinueDurationInPrescription,IsRemovePrescriptionPopUpMsgForItemsNotInStock,PrescriptionAllowForItemsNotInStockShowAlert,RouteMandatory,ShowStockQtyInPrescription,EMRCompoundedDrugNameRequired,IsCancelWithOutRemarksInEmrPreviousMedication", sConString);

                if (collHospitalSetupValues.ContainsKey("EMRPrescriptionDoseShowInFractionalValue"))
                    chkFractionalDose.Checked = common.myStr(collHospitalSetupValues["EMRPrescriptionDoseShowInFractionalValue"]).ToUpper().Equals("Y");
                if (collHospitalSetupValues.ContainsKey("RouteMandatory"))
                    spnRoute.Visible = common.myStr(collHospitalSetupValues["RouteMandatory"]).ToUpper().Equals("Y");
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
                if (collHospitalSetupValues.ContainsKey("EMRCompoundedDrugNameRequired"))
                    ViewState["EMRCompoundedDrugNameRequired"] = common.myStr(collHospitalSetupValues["EMRCompoundedDrugNameRequired"]);
                if (collHospitalSetupValues.ContainsKey("IsCancelWithOutRemarksInEmrPreviousMedication"))
                    ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"] = common.myStr(collHospitalSetupValues["IsCancelWithOutRemarksInEmrPreviousMedication"]);

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

                dvCompoundedDrugName.Visible = common.myStr(ViewState["EMRCompoundedDrugNameRequired"]).ToUpper().Equals("Y");
                dvPrescriptionDetailConfirm.Visible = false;

                Session["CurentSavedPrescriptionId"] = string.Empty;
                hdnTemplateFieldId.Value = common.myInt(Request.QueryString["TemplateFieldId"]).ToString();
                hdnControlId.Value = common.myStr(Request.QueryString["ID"]).Trim();
                hdnControlType.Value = common.myStr(Request.QueryString["ControlType"]).Trim();

                if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
                {
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

                }

                //if (common.myStr(Session["OPIP"]).Equals("I"))
                //{
                //    chkApprovalRequired.Visible = true;
                //}
                if (common.myInt(ViewState["EncId"]).Equals(0))
                {
                    Response.Redirect("/default.aspx?RegNo=0", false);
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
                    //chkCustomMedication.Enabled = false;
                    //chkNotToPharmacy.Enabled = false;
                    //lnkStopMedication.Visible = false;
                    //btnPreviousMedications.Text = "Previous Order";
                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    //ddlRoute.Items.Insert(0, new ListItem(string.Empty));
                    ddlRoute.Items[0].Value = "0";
                    ddlRoute.SelectedIndex = 0;
                    ddlRoute.Enabled = false;
                }

                bindOnLoadData();

                //btnCopyLastPrescription.Enabled = true;
                ////BindGrid(string.Empty, string.Empty);
                //rdoSearchMedication_OnSelectedIndexChanged(null, null);
                BindBlankItemGrid();
                BindPreviousDataGrid();

                BaseC.Security objSecurity = new BaseC.Security(sConString);  //
                ViewState["IsAllowToAddBlockedItem"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedItem");
                objSecurity = null;

                divConfirmation.Visible = false;

                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    spnStartdate.Visible = true;
                }
                else
                {
                    spnStartdate.Visible = false;
                }
                txtStartDate.SelectedDate = null;
                txtStartDate.MinDate = DateTime.Now;
                BindPatientHiddenDetails();
                SetPermission();
                dvConfirmAlreadyExistOptions.Visible = false;
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSave.Visible = false;
                    //btnCopyLastPrescription.Visible = false;
                    //ddlGeneric.Enabled = false;
                    ddlBrand.Enabled = false;
                    //btnProceedFavourite.Enabled = false;
                    //chkCustomMedication.Enabled = false;
                    //btnAddItem.Enabled = false;
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
                if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1") || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
                {
                    btnclose1.Visible = true;
                }
                else
                {
                    btnclose1.Visible = false;
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

                if (!common.myStr(Session["OPIP"]).Equals("I"))
                {
                    btnSave.Visible = false;
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
                //if (spnStartdate.Visible)
                //{
                txtStartDate.SelectedDate = DateTime.Now.Date;
                //}
            }



            if (!common.myStr(Session["OPIP"]).Equals("I"))
            {
                if (common.myStr(ViewState["EMRIsHideBrandInPrescription"]).Equals("Y"))
                {
                    ddlBrand.Visible = false;
                    lblInfoBrand.Visible = false;
                    //ddlGeneric.Focus();
                }
            }

            setPrescriptionMandatoryFieldSetup();

            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
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
    private DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));

        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("GenericName", typeof(string));

        dt.Columns.Add("UnitId", typeof(int));
        dt.Columns.Add("UnitName", typeof(string));

        dt.Columns.Add("StrengthValue", typeof(string));
        dt.Columns.Add("Dose", typeof(double));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("IsInfusion", typeof(bool));
        dt.Columns.Add("IsInjection", typeof(bool));

        dt.Columns.Add("CIMSItemId", typeof(string));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));

        dt.Columns.Add("OverrideComments", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugToDrug", typeof(string));
        dt.Columns.Add("OverrideCommentsDrugHealth", typeof(string));

        return dt;
    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();

        DataRow dr = dt.NewRow();

        dt.Rows.Add(dr);
        dt.AcceptChanges();

        gvItem.DataSource = dt;
        gvItem.DataBind();
    }

    private void BindPreviousDataGrid()
    {
        BaseC.WardManagement objw = new BaseC.WardManagement();
        DataSet ds = new DataSet();

        try
        {
            ds = objw.getCompoundedDrugOrder(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }

            gvPreviousItems.DataSource = ds.Tables[0];
            gvPreviousItems.DataBind();

        }
        catch
        {
        }

    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        int ItemId = 0;
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            if (common.myStr(ViewState["ADDNORMALDRUG"]).Equals("Y"))
            {
                if (gvItem.Rows.Count > 0 && common.myInt(ViewState["SelectedIndentId"]) > 0)
                {
                    foreach (GridViewRow dataItem in gvItem.Rows)
                    {
                        HiddenField hdnItemIdGD = (HiddenField)dataItem.FindControl("hdnItemId");

                        if (common.myInt(hdnItemIdGD.Value) > 0)
                        {
                            lblMessage.Text = "Normal drug allow only single item to prescribled!";
                            Alert.ShowAjaxMsg(lblMessage.Text, this.Page);

                            ddlBrand.ClearSelection();
                            ddlBrand.Text = string.Empty;
                            hdnItemId.Value = string.Empty;

                            return;
                        }
                    }
                }
            }

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
            if (ItemId > 0)
            {
                hdnGenericId.Value = "0";
                //lblGenericName.Text = string.Empty;

                int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
                if (DoctorId.Equals(0))
                {
                    DoctorId = common.myInt(Session["LoginDoctorId"]);
                }
                if (DoctorId.Equals(0))
                {
                    DoctorId = common.myInt(Session["EmployeeId"]);
                }

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


                    ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
                    ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);

                    hdnInfusion.Value = common.myStr(DR["IsInfusion"]);
                    hdnIsInjection.Value = common.myBool(DR["IsInjection"]).ToString();



                    //dvPharmacistInstruction.Visible = false;
                    if (common.myStr(DR["FieldCode"]).Equals("INSPH"))
                    {
                        if (common.myLen(DR["ValueText"]) > 0)
                        {
                            //lnkPharmacistInstruction.Visible = true;
                            //txtPharmacistInstruction.Text = common.myStr(DR["ValueText"]);
                            //txtInstructions.Text = common.myStr(DR["ValueText"]);
                        }
                    }
                    else
                    {
                        //lnkPharmacistInstruction.Visible = false;
                        //txtPharmacistInstruction.Text = string.Empty;
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
                    }

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

                    AddBrandInList(ds.Tables[0]);

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
            }

            //CheckgenerateInstruction(ItemId);

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

    private DataTable PreservePreviousItemData()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        try
        {
            dt = CreateItemTable();

            if (dt != null)
            {
                #region preserve previous data
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");

                    if (common.myInt(hdnItemId.Value) > 0)
                    {
                        Label lblItemName = (Label)dataItem.FindControl("lblItemName");

                        HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                        HiddenField hdnGenericName = (HiddenField)dataItem.FindControl("hdnGenericName");
                        HiddenField hdnIsInfusion = (HiddenField)dataItem.FindControl("hdnIsInfusion");
                        HiddenField hdnIsInjection = (HiddenField)dataItem.FindControl("hdnIsInjection");

                        TextBox txtDoseGD = (TextBox)dataItem.FindControl("txtDoseGD");
                        RadComboBox ddlUnitGD = (RadComboBox)dataItem.FindControl("ddlUnitGD");
                        RadComboBox ddlStrengthValueGD = (RadComboBox)dataItem.FindControl("ddlStrengthValueGD");

                        TextBox txtQtyGD = (TextBox)dataItem.FindControl("txtQtyGD");

                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        HiddenField hdnVIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");

                        HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                        HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                        HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");



                        dr = dt.NewRow();

                        dr["ItemId"] = common.myInt(hdnItemId.Value);
                        dr["ItemName"] = common.myStr(lblItemName.Text);

                        dr["GenericId"] = common.myInt(hdnGenericId.Value);
                        dr["GenericName"] = common.myStr(hdnGenericName.Value);

                        dr["UnitId"] = common.myInt(ddlUnitGD.SelectedValue);
                        dr["UnitName"] = common.myStr(ddlUnitGD.SelectedItem.Text);
                        dr["StrengthValue"] = common.myStr(ddlStrengthValueGD.SelectedValue);

                        dr["Dose"] = common.myDec(txtDoseGD.Text).ToString("0.##");
                        dr["Qty"] = common.myDec(txtQtyGD.Text).ToString("0.##");
                        dr["IsInfusion"] = common.myBool(hdnIsInfusion.Value);
                        dr["IsInjection"] = common.myBool(hdnIsInjection.Value);

                        dr["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                        dr["CIMSType"] = common.myStr(hdnCIMSType.Value);
                        dr["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);

                        dr["OverrideComments"] = common.myStr(hdnCommentsDrugAllergy.Value);
                        dr["OverrideCommentsDrugToDrug"] = common.myStr(hdnCommentsDrugToDrug.Value);
                        dr["OverrideCommentsDrugHealth"] = common.myStr(hdnCommentsDrugHealth.Value);

                        dt.Rows.Add(dr);
                    }
                }
                #endregion
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

        return dt;
    }

    private void AddBrandInList(DataTable dtNewData)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            dt = PreservePreviousItemData();

            #region Add new data
            if (dtNewData != null)
            {
                if (dtNewData.Rows.Count > 0)
                {
                    DataRow drNewData = dtNewData.Rows[0];

                    hdnItemId.Value = common.myInt(drNewData["ItemId"]).ToString();
                    hdnItemName.Value = common.myStr(drNewData["ItemName"]);

                    dv = dt.Copy().DefaultView;
                    dv.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value);

                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblMessage.Text = "Drug name already added";
                        Alert.ShowAjaxMsg(lblMessage.Text, this.Page);
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();

                        dr["ItemId"] = common.myInt(hdnItemId.Value);
                        dr["ItemName"] = common.myStr(hdnItemName.Value);

                        dr["GenericId"] = common.myInt(drNewData["GenericId"]);
                        dr["GenericName"] = common.myStr(drNewData["GenericName"]);

                        dr["UnitId"] = common.myInt(drNewData["ItemUnitId"]);
                        dr["StrengthValue"] = common.myStr(drNewData["StrengthValue"]);

                        if (common.myDec(drNewData["Dose"]) > 0)
                        {
                            dr["Dose"] = common.myDec(drNewData["Dose"]).ToString("0.##");
                        }
                        else
                        {
                            dr["Dose"] = 1;
                        }

                        dr["Qty"] = 1;
                        dr["IsInfusion"] = common.myBool(drNewData["IsInfusion"]);
                        dr["IsInjection"] = common.myBool(drNewData["IsInjection"]);

                        dr["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                        dr["CIMSType"] = common.myStr(hdnCIMSType.Value);
                        dr["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);

                        dr["OverrideComments"] = DBNull.Value;
                        dr["OverrideCommentsDrugToDrug"] = DBNull.Value;
                        dr["OverrideCommentsDrugHealth"] = DBNull.Value;

                        dt.Rows.Add(dr);
                    }
                }
            }
            #endregion

            gvItem.DataSource = dt;
            gvItem.DataBind();
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
            dv.Dispose();
        }
    }

    protected void gvItem_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (e.CommandName.ToUpper().Equals("ITEMDELETE"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");

                if (common.myInt(hdnItemId.Value) > 0)
                {
                    dv = PreservePreviousItemData().Copy().DefaultView;

                    dv.RowFilter = "ISNULL(ItemId,0)<>" + common.myInt(hdnItemId.Value);

                    dt = dv.ToTable();

                    if (dt.Rows.Count == 0)
                    {
                        DataRow dr = dt.NewRow();

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }

                    gvItem.DataSource = dt;
                    gvItem.DataBind();

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
            dt.Dispose();
            dv.Dispose();
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
                        //spnDose.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "FieldCode='DUNT'"; //Unit
                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        //spnUnit.Visible = common.myBool(ds.Tables[0].DefaultView[0]["IsMandatory"]);
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

    protected void gvPreviousItems_OnRowCreated(object sender, GridViewRowEventArgs e)
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
    protected void gvPreviousItems_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
            //    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") &&
            //    (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
            //{
            //    e.Row.Cells[(byte)enumColumnsPrevious.TotalQty].Visible = true;
            //}
            //else
            //{
            //    //e.Row.Cells[(byte)enumColumns.TotalQty].Visible = false;
            //}
            //e.Row.Cells[(byte)enumColumns.Sno].Visible = false;
            //e.Row.Cells[(byte)enumColumns.IndentType].Visible = false;
            //e.Row.Cells[(byte)enumColumns.PrescriptionDetail].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                //if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
                //    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") &&
                //    (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
                //{
                //    e.Row.Cells[(byte)enumColumnsPrevious.TotalQty].Visible = true;
                //}
                //else
                //{
                //    //e.Row.Cells[(byte)enumColumnsPrevious.TotalQty].Visible = false;
                //}
                //e.Row.Cells[(byte)enumColumnsPrevious.Sno].Visible = false;
                //e.Row.Cells[(byte)enumColumnsPrevious.IndentType].Visible = false;
                //e.Row.Cells[(byte)enumColumnsPrevious.PrescriptionDetail].Visible = false;
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
    }

    protected void gvPreviousItems_OnRowCommand(object Sender, GridViewCommandEventArgs e)
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
        DataSet dsCM = new DataSet();
        try
        {
            lblMessage.Text = string.Empty;

            if (e.CommandName.ToUpper().Equals("REORDER"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnIndentId.Value).Equals(0))
                {
                    return;
                }

                dsCM = objEMR.getCompoundedDrugDetails(common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId.Value), common.myInt(hdnIndentId.Value));

                if (dsCM.Tables[0].Rows.Count > 0)
                {
                    gvItem.DataSource = dsCM.Tables[0];
                    gvItem.DataBind();

                    DataRow DR = dsCM.Tables[0].Rows[0];

                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DR["FrequencyId"]).ToString()));
                    txtDuration.Text = common.myDbl(DR["Duration"]).ToString("0.##");
                    ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DR["TypeDuration"])));
                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DR["FoodRelationshipId"]).ToString()));
                    txtStartDate.SelectedDate = DateTime.Now;
                    ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myInt(DR["DoseTypeId"]).ToString()));
                    txtFlowRateUnit.Text = common.myStr(DR["FlowRate"]);
                    ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue(common.myInt(DR["FlowRateUnit"]).ToString()));
                    txtTimeInfusion.Text = common.myStr(DR["InfusionTime"]);
                    ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue(common.myStr(DR["TimeUnit"])));
                    txtInstructions.Text = common.myStr(DR["InstructionRemarks"]);

                    ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindByValue(common.myStr(DR["IndentType"])));
                    ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myInt(DR["AdvisingDoctorId"]).ToString()));
                    txtCompoundedDrugName.Text = common.myStr(DR["CompoundedDrugName"]);
                    txtCompoundedDose.Text = common.myStr(DR["CompoundedDose"]);
                    ddlCompoundedUnitId.SelectedIndex = ddlCompoundedUnitId.Items.IndexOf(ddlCompoundedUnitId.Items.FindItemByValue(common.myInt(DR["CompoundedUnitId"]).ToString()));
                }
            }
            else if (e.CommandName.ToUpper().Equals("ADDNORMALDRUG"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnIndentId.Value).Equals(0))
                {
                    return;
                }

                ViewState["SelectedIndentId"] = common.myInt(hdnIndentId.Value);
                ViewState["ADDNORMALDRUG"] = "Y";
                dvCompoundedDrugName.Visible = false;
            }
            else if (e.CommandName.ToUpper().Equals("ITEMCANCEL"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnCompoundedItemId = (HiddenField)row.FindControl("hdnCompoundedItemId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnIndentId.Value).Equals(0))
                {
                    return;
                }

                if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                {
                    Alert.ShowAjaxMsg("Cancellation allowed only on current selected encounter !", this.Page);
                    return;
                }

                ViewState["SelectedIndentId"] = common.myInt(hdnIndentId.Value);
                ViewState["SelectedCompoundedItemId"] = common.myInt(hdnCompoundedItemId.Value);

                if (common.myStr(ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"]).Equals("Y"))
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = false;
                    btnStopMedication_OnClick(null, null);
                }
                else
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;
                    lblCancelStopMedicationRemarks.Text = "Cancel Medication Remarks";
                    btnStopMedication.Text = "Cancel";
                }

                setVisiblilityInteraction();
            }
            else if (e.CommandName.ToUpper().Equals("ITEMSTOP"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnCompoundedItemId = (HiddenField)row.FindControl("hdnCompoundedItemId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                if (common.myInt(hdnIndentId.Value).Equals(0))
                {
                    return;
                }

                if (!common.myInt(hdnEncounterId.Value).Equals(common.myInt(Session["EncounterId"])))
                {
                    Alert.ShowAjaxMsg("Cancellation allowed only on current selected encounter !", this.Page);
                    return;
                }

                ViewState["SelectedIndentId"] = common.myInt(hdnIndentId.Value);
                ViewState["SelectedCompoundedItemId"] = common.myInt(hdnCompoundedItemId.Value);

                if (common.myStr(ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"]).Equals("Y"))
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = false;
                    btnStopMedication_OnClick(null, null);
                }
                else
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;
                    lblCancelStopMedicationRemarks.Text = "Stop Medication Remarks";
                    btnStopMedication.Text = "Stop";
                }

                setVisiblilityInteraction();
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

                if (ItemId <= 0 && common.myBool(hdnCustomMedication.Value))
                {
                    //txtCustomMedication.Text = common.myStr(lblItemName.Text);
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
                            //CheckgenerateInstruction(ItemId);
                        }

                        ddlBrand.Enabled = false;

                        // btnGetInfo_Click(null, null);

                    }
                    catch
                    {
                    }
                }
                hdnItemId.Value = ItemId.ToString();
                hdnGenericId.Value = GenericId.ToString();

                //ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                //ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

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
                                //txtDose.Text = common.myStr(dtDetail.Rows[0]["Dose"]);
                            }
                            else
                            {
                                //txtDose.Text = string.Empty;
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
                            //ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["ReferanceItemId"])));
                            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FoodRelationshipID"]).ToString()));
                            ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["DoseTypeId"]).ToString()));
                            //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["UnitId"]).ToString()));
                            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FormulationId"]).ToString()));
                            ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FrequencyId"]).ToString()));
                            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["RouteId"]).ToString()));
                            //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(common.myInt(dtDetail.Rows[0]["RouteId"]).ToString()));
                            //if (common.myDbl(dtDetail.Rows[0]["Dose"]) > 0)
                            //{
                            //    txtDose.Text = common.myStr(dtDetail.Rows[0]["Dose"]);
                            //}
                            //else
                            //{
                            //    txtDose.Text = string.Empty;
                            //}
                            txtTimeInfusion.Text = common.myStr(dtDetail.Rows[0]["InfusionTime"]);
                            //txtTotalVolumn.Text = common.myStr(dtDetail.Rows[0]["TotalVolume"]);
                            //txtVolume.Text = common.myStr(dtDetail.Rows[0]["Volume"]);
                            //ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["VolumeUnitId"]).ToString()));
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
                            //chkSubstituteNotAllow.Checked = common.myBool(dtDetail.Rows[0]["IsSubstituteNotAllowed"]);
                            //txtICDCode.Text = common.myStr(dtDetail.Rows[0]["ICDCode"]);
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
                        //txtDose.Text = common.myStr(hdnDose.Value);
                        //ddlUnit.SelectedValue = common.myStr(hdnUnitId.Value);
                        ddlFrequencyId.SelectedValue = common.myStr(hdnFrequencyId.Value);
                        txtDuration.Text = common.myStr(hdnDuration.Value);
                        ddlDoseType.SelectedValue = common.myStr(hdnDurationType.Value);
                        ddlFoodRelation.SelectedValue = common.myStr(hdnFoodRelationshipId.Value);
                        ddlRoute.SelectedValue = common.myStr(hdnRouteId.Value);
                        ddlFormulation.SelectedValue = common.myStr(hdnFormulationId.Value);
                        //ddlStrengthValue.SelectedIndex = ddlStrengthValue.Items.IndexOf(ddlStrengthValue.Items.FindItemByText(common.myStr(hdnStrengthValue.Value)));
                        //ddlStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                        txtInstructions.Text = common.myStr(hdnRemarks.Value);
                    }
                }
                dsCalisreq = objPhr.ISCalculationRequired(ItemId);
                if (dsCalisreq.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
                }

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

    protected void imgAllergyAlert_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), 
                common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]), common.myInt(Session["RegistrationID"]),
                common.myStr(Session["EncounterNo"]), common.myInt(Session["EncounterId"]));
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

            Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), 
                            common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]), common.myInt(Session["RegistrationID"]),
                            common.myStr(Session["EncounterNo"]), common.myInt(Session["EncounterId"]));

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
                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {
                    DataSet dsIndentType = new DataSet();
                    dsIndentType = objEMR.GetIndentType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                    ddlIndentType.DataSource = dsIndentType.Tables[0];
                    ddlIndentType.DataValueField = "Id";
                    ddlIndentType.DataTextField = "IndentType";
                    ddlIndentType.DataBind();
                }
                else
                {
                    ddlIndentType.DataSource = ds.Tables[2];
                    ddlIndentType.DataValueField = "Id";
                    ddlIndentType.DataTextField = "IndentType";
                    ddlIndentType.DataBind();
                }
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
                //ViewState["OrderSetMaster"] = ds.Tables[4];
            }
            #endregion
            #region Table 5-ICDCode PatientDiagnosis
            if (ds.Tables.Count > 5)
            {
                ViewState["ICDCodes"] = null;

                if (ds.Tables[5].Rows.Count > 0)
                {
                    ViewState["ICDCodeMaster"] = ds.Tables[5];
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
                    ddlCompoundedUnitId.Items.Add(item);
                }

                ddlCompoundedUnitId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                ddlCompoundedUnitId.SelectedIndex = 0;

                //DV = new DataView();
                //DV = ds.Tables[7].DefaultView;
                //DV.RowFilter = "IsVolumeUnit=1";

                //ddlVolumeUnit.DataSource = DV.ToTable();
                //ddlVolumeUnit.DataValueField = "Id";
                //ddlVolumeUnit.DataTextField = "UnitName";
                //ddlVolumeUnit.DataBind();
                //ddlVolumeUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                //ddlVolumeUnit.SelectedIndex = 0;

                DataView DV1 = new DataView();
                DV1 = ds.Tables[7].DefaultView;
                DV1.RowFilter = "IsInfusionUnit=1";

                ddlFlowRateUnit.DataSource = DV1.ToTable();
                ddlFlowRateUnit.DataValueField = "Id";
                ddlFlowRateUnit.DataTextField = "UnitName";
                ddlFlowRateUnit.DataBind();
                ddlFlowRateUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
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
                    item.Text = common.myStr(DR["FormulationName"]);
                    item.Value = common.myStr(common.myInt(DR["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(DR["DefaultRouteId"]));

                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
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

                if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
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
                //if (ds.Tables[16].Rows.Count > 0)
                //{
                //    ViewState["UnApprovedPrescriptions"] = ds.Tables[16];
                //}
            }
            #endregion
            #region Table 17-CheckDiagnosisPrimaryForPatient
            if (ds.Tables.Count > 17)
            {
                //if (ds.Tables[17].Rows.Count > 0)
                //{
                //    ViewState["IsPrimaryDiagnosis"] = (common.myInt(ds.Tables[17].Rows[0]["CNT"]) > 0) ? true : false;
                //}
            }
            #endregion
            #region Table 18-IsEPrescriptionEnabled
            if (ds.Tables.Count > 18)
            {
                //if (ds.Tables[18].Rows.Count > 0)
                //{
                //    ViewState["IsEPrescriptionEnabled"] = common.myBool(ds.Tables[18].Rows[0]["IsEPrescriptionEnabled"]);
                //}
            }
            #endregion
            #region Table 21-Strength Master
            ViewState["StrengthMasterList"] = null;
            if (ds.Tables.Count > 20)
            {
                if (ds.Tables[21].Rows.Count > 0)
                {
                    ViewState["StrengthMasterList"] = ds.Tables[21];
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

            saveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string GenerateCompoundedPrescriptionDetail(DataTable dtPreviousData)
    {
        StringBuilder sbPrescriptionDetail = new StringBuilder();
        try
        {
            if (dtPreviousData != null)
            {
                if (dtPreviousData.Rows.Count > 0)
                {
                    /*<Compounded Drug> - <Dose Unit>
                      <Form> - <Brand Name> - <Dose Unit (Concatenate)>, <Strength:> and 
                      <Form> - <Brand Name> - <Dose Unit (Concatenate)>, <Strength:> 
                      <Frequency> (<Route>) <@Flow Rate unit for time>*/

                    if (common.myStr(ViewState["EMRCompoundedDrugNameRequired"]).ToUpper().Equals("Y"))
                    {
                        if (common.myLen(txtCompoundedDrugName.Text) > 0)
                        {
                            sbPrescriptionDetail.Append(common.myStr(txtCompoundedDrugName.Text).Trim());

                            if (common.myDbl(txtCompoundedDose.Text) > 0)
                            {
                                sbPrescriptionDetail.Append(" - " + common.myDbl(txtCompoundedDose.Text).ToString("0.##"));
                            }
                            if (common.myInt(ddlCompoundedUnitId.SelectedValue) > 0)
                            {
                                sbPrescriptionDetail.Append(" " + common.myStr(ddlCompoundedUnitId.SelectedItem.Text));
                            }
                        }
                    }

                    for (int rowIdx = 0; rowIdx < dtPreviousData.Rows.Count; rowIdx++)
                    {
                        DataRow drPD = dtPreviousData.Rows[rowIdx];

                        if (common.myInt(drPD["ItemId"]) > 0)
                        {
                            if (rowIdx > 0)
                            {
                                sbPrescriptionDetail.Append(" and<br/>");
                            }
                            else if (sbPrescriptionDetail.ToString() != string.Empty)
                            {
                                sbPrescriptionDetail.Append("<br/>");
                            }

                            if (common.myInt(ddlFormulation.SelectedValue) > 0)
                            {
                                sbPrescriptionDetail.Append(common.myStr(ddlFormulation.SelectedItem.Text));
                            }
                            if (common.myLen(drPD["ItemName"]) > 0)
                            {
                                sbPrescriptionDetail.Append(" - " + common.myStr(drPD["ItemName"]));
                            }
                            if (common.myDbl(drPD["Dose"]) > 0)
                            {
                                sbPrescriptionDetail.Append(" - " + common.myDbl(drPD["Dose"]).ToString("0.##"));
                            }
                            if (common.myLen(drPD["UnitName"]) > 0)
                            {
                                sbPrescriptionDetail.Append(" " + common.myStr(drPD["UnitName"]));
                            }
                            if (common.myLen(drPD["StrengthValue"]) > 0)
                            {
                                sbPrescriptionDetail.Append(", Strength: " + common.myStr(drPD["StrengthValue"]));
                            }
                        }
                    }
                    if (sbPrescriptionDetail.ToString() != string.Empty)
                    {
                        sbPrescriptionDetail.Append("<br/>");
                        if (common.myInt(ddlFrequencyId.SelectedValue) > 0)
                        {
                            sbPrescriptionDetail.Append(common.myStr(ddlFrequencyId.SelectedItem.Text));
                        }
                        if (common.myInt(ddlRoute.SelectedValue) > 0)
                        {
                            sbPrescriptionDetail.Append(" (" + common.myStr(ddlRoute.SelectedItem.Text) + ")");
                        }
                        if (common.myDbl(txtFlowRateUnit.Text) > 0)
                        {
                            sbPrescriptionDetail.Append(" @" + common.myDbl(txtFlowRateUnit.Text).ToString("0.##"));
                        }
                        if (common.myInt(ddlFlowRateUnit.SelectedIndex) > 0)
                        {
                            sbPrescriptionDetail.Append(" " + common.myStr(ddlFlowRateUnit.SelectedItem.Text));
                        }
                        if (common.myDbl(txtTimeInfusion.Text) > 0)
                        {
                            sbPrescriptionDetail.Append(" for " + common.myDbl(txtTimeInfusion.Text).ToString("0.##"));
                        }
                        if (common.myInt(ddlTimeUnit.SelectedIndex) > 0)
                        {
                            sbPrescriptionDetail.Append(" " + common.myStr(ddlTimeUnit.SelectedItem.Text));
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            dtPreviousData.Dispose();
        }
        return sbPrescriptionDetail.ToString();
    }

    private string GeneratePrescriptionDetail(double Dose, string UnitName)
    {
        StringBuilder sbPrescriptionDetail = new StringBuilder();
        try
        {
            /*1.5 Tablet(s)  -  2 times a day for 7 Day(s)  (Oral), After Dinner, Start Date: 15/01/2019*/
            /*<Dose> <Unit>  -  Frequency for <duration+unit> (<Route>), <FoodRelation>, <Start Date:> */

            if (common.myDbl(Dose) > 0)
            {
                sbPrescriptionDetail.Append(common.myDbl(Dose).ToString("0.##"));

                if (common.myLen(UnitName) > 0)
                {
                    sbPrescriptionDetail.Append(" " + common.myStr(UnitName));
                }
                if (common.myInt(ddlFrequencyId.SelectedValue) > 0)
                {
                    sbPrescriptionDetail.Append(" - " + common.myStr(ddlFrequencyId.SelectedItem.Text));
                }
                if (common.myDbl(txtDuration.Text) > 0)
                {
                    sbPrescriptionDetail.Append(" for " + common.myStr(txtDuration.Text));
                }
                if (common.myStr(ddlPeriodType.SelectedValue) != string.Empty)
                {
                    sbPrescriptionDetail.Append(" " + common.myStr(ddlPeriodType.SelectedItem.Text));
                }
                if (common.myInt(ddlRoute.SelectedValue) > 0)
                {
                    sbPrescriptionDetail.Append(" (" + common.myStr(ddlRoute.SelectedItem.Text) + ")");
                }
                if (common.myInt(ddlFoodRelation.SelectedValue) > 0)
                {
                    sbPrescriptionDetail.Append(", " + common.myStr(ddlFoodRelation.SelectedItem.Text));
                }
                if (common.myLen(txtStartDate.SelectedDate) > 0)
                {
                    sbPrescriptionDetail.Append(", Start Date:" + Convert.ToDateTime(txtStartDate.SelectedDate.Value).ToString("dd/MM/yyyy"));
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
        }

        return sbPrescriptionDetail.ToString();
    }

    private void clearControl()
    {
        try
        {
            hdnItemId.Value = string.Empty;
            hdnItemName.Value = string.Empty;

            hdnGenericId.Value = string.Empty;
            hdnGenericName.Value = string.Empty;

            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = string.Empty;

            ddlFrequencyId.SelectedIndex = 0;
            txtDuration.Text = string.Empty;
            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;
            ddlFoodRelation.SelectedIndex = 0;
            txtStartDate.SelectedDate = null;
            txtStartDate.Clear();
            ddlDoseType.SelectedIndex = 0;
            txtFlowRateUnit.Text = string.Empty;
            ddlFlowRateUnit.SelectedIndex = 0;
            txtTimeInfusion.Text = string.Empty;
            ddlTimeUnit.SelectedIndex = 0;
            txtInstructions.Text = string.Empty;


            ddlIndentType.SelectedIndex = 0;
            hdnCIMSItemId.Value = string.Empty;
            hdnCIMSType.Value = string.Empty;
            hdnVIDALItemId.Value = "0";
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;
            ddlFrequencyId.SelectedIndex = 0;
            txtDuration.Text = string.Empty;
            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
            txtStartDate.SelectedDate = null;
            txtInstructions.Text = string.Empty;
            //lnkPharmacistInstruction.Visible = false;

        }
        catch (Exception)
        {
        }
    }

    protected void saveData()
    {
        DataTable dtPreviousData = new DataTable();
        string CompoundedPrescriptionDetail = string.Empty;
        string PrescriptionDetail = string.Empty;
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

            if (!common.myStr(ViewState["ADDNORMALDRUG"]).Equals("Y") && common.myStr(ViewState["EMRCompoundedDrugNameRequired"]).ToUpper().Equals("Y"))
            {
                if (common.myLen(txtCompoundedDrugName.Text).Equals(0))
                {
                    lblMessage.Text = "Please enter compounded drug name!";
                    return;
                }

                if (common.myLen(txtCompoundedDose.Text).Equals(0))
                {
                    lblMessage.Text = "Please enter compounded dose!";
                    return;
                }

                if (common.myInt(ddlCompoundedUnitId.SelectedValue).Equals(0))
                {
                    lblMessage.Text = "Please select compounded unit!";
                    return;
                }
            }

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                if (common.myStr(ViewState["ADDNORMALDRUG"]).Equals("Y"))
                {
                    saveDataFinalNormalDrug();
                }
                else
                {
                    dtPreviousData = PreservePreviousItemData();
                    CompoundedPrescriptionDetail = GenerateCompoundedPrescriptionDetail(dtPreviousData);

                    if (common.myLen(CompoundedPrescriptionDetail) > 0)
                    {
                        txtPrescriptionDetailConfirm.Text = CompoundedPrescriptionDetail.Replace("<br/>", Environment.NewLine);
                        dvPrescriptionDetailConfirm.Visible = true;
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
            dtPreviousData.Dispose();
        }
    }

    protected void saveDataFinal()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataSet dsXmlFrequency = new DataSet();
        DataSet dsVariableDose = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        Hashtable hshOutput = new Hashtable();

        DataTable dtPreviousData = new DataTable();
        string CompoundedPrescriptionDetail = string.Empty;
        string PrescriptionDetail = string.Empty;
        int CompoundedItemId = 0;
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

            if (common.myStr(ViewState["EMRCompoundedDrugNameRequired"]).ToUpper().Equals("Y"))
            {
                if (common.myLen(txtCompoundedDrugName.Text).Equals(0))
                {
                    lblMessage.Text = "Please enter compounded drug name!";
                    return;
                }

                if (common.myLen(txtCompoundedDose.Text).Equals(0))
                {
                    lblMessage.Text = "Please enter compounded dose!";
                    return;
                }

                if (common.myInt(ddlCompoundedUnitId.SelectedValue).Equals(0))
                {
                    lblMessage.Text = "Please select compounded unit!";
                    return;
                }
            }

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                dtPreviousData = PreservePreviousItemData();
                CompoundedPrescriptionDetail = GenerateCompoundedPrescriptionDetail(dtPreviousData);

                DateTime endDate = endDateCalculate(Convert.ToDateTime(txtStartDate.SelectedDate), common.myInt(txtDuration.Text), common.myStr(ddlPeriodType.SelectedValue));

                for (int rowIdx = 0; rowIdx < dtPreviousData.Rows.Count; rowIdx++)
                {
                    DataRow drPD = dtPreviousData.Rows[rowIdx];
                    coll1 = new ArrayList();

                    if (common.myInt(drPD["ItemId"]) > 0)
                    {
                        if (rowIdx == 0)
                        {
                            CompoundedItemId = common.myInt(drPD["ItemId"]);
                        }

                        PrescriptionDetail = GeneratePrescriptionDetail(common.myDbl(drPD["Dose"]), common.myStr(drPD["UnitName"]));

                        coll1.Add(0);//IndentId INT,
                        coll1.Add(common.myInt(drPD["ItemId"]));//ItemId INT,
                        coll1.Add(DBNull.Value);//CustomMedication VARCHAR(1000),
                        coll1.Add(0);//GenericId INT,
                        coll1.Add(common.myInt(ddlFormulation.SelectedValue));//FormulationId INT,
                        coll1.Add(common.myInt(ddlRoute.SelectedValue));//RouteId INT,
                        coll1.Add(DBNull.Value);//StrengthId INT,
                        coll1.Add(common.myDec(drPD["Qty"]));//TotalQty DECIMAL(10, 3),
                        coll1.Add((common.myLen(txtStartDate.SelectedDate) > 0) ? Convert.ToDateTime(txtStartDate.SelectedDate).ToString("yyyy-MM-dd") : string.Empty);//StartDate SMALLDATETIME,
                        coll1.Add((common.myLen(endDate) > 0) ? Convert.ToDateTime(endDate).ToString("yyyy-MM-dd") : string.Empty);//EndDate SMALLDATETIME,
                        coll1.Add(0);//NotToPharmacy BIT,
                        coll1.Add(common.myStr(drPD["OverrideComments"]));//OverrideComments VARCHAR(500),
                        coll1.Add(common.myStr(drPD["OverrideCommentsDrugToDrug"]));//OverrideCommentsDrugToDrug VARCHAR(500),
                        coll1.Add(common.myStr(drPD["OverrideCommentsDrugHealth"]));//OverrideCommentsDrugHealth VARCHAR(500),
                        coll1.Add(common.myStr(PrescriptionDetail));//PrescriptionDetail VARCHAR(1000),
                        coll1.Add(common.myStr(drPD["StrengthValue"]));//StrengthValue VARCHAR(255),
                        coll1.Add(common.myDec(drPD["Qty"]));//OriginalQty DECIMAL(10, 3)

                        strXML1.Append(common.setXmlTable(ref coll1));


                        //------------------------Item Details---------------------------------//

                        coll = new ArrayList();

                        coll.Add(common.myInt(drPD["ItemId"]));//ItemId INT,
                        coll.Add(common.myInt(ddlFrequencyId.SelectedValue));//FrequencyId TINYINT,      
                        coll.Add(common.myDec(drPD["Dose"]));//Dose DECIMAL(10, 3),      
                        coll.Add(common.myInt(txtDuration.Text));//Duration VARCHAR(10),
                        coll.Add(common.myStr(ddlPeriodType.SelectedValue));//Type CHAR(1),
                        coll.Add(common.myStr(txtInstructions.Text));//Instructions VARCHAR(2000),
                        coll.Add(common.myInt(drPD["UnitId"]));//UnitId INT,
                        coll.Add(-1);//ReferanceItemId INT,
                        coll.Add(common.myInt(ddlFoodRelation.SelectedValue));//FoodRelationshipId INT,
                        coll.Add(common.myInt(ddlDoseType.SelectedValue));//DoseTypeId INT,
                        coll.Add(DBNull.Value);//Volume VARCHAR(20),
                        coll.Add(DBNull.Value);//VolumeUnitId SMALLINT,
                        coll.Add(common.myStr(txtTimeInfusion.Text));//InfusionTime VARCHAR(20),

                        if (common.myInt(ddlTimeUnit.SelectedIndex) > 0)
                        {
                            coll.Add(common.myStr(ddlTimeUnit.SelectedValue));//TimeUnit CHAR(1),
                        }
                        else
                        {
                            coll.Add(DBNull.Value);//TimeUnit CHAR(1),
                        }

                        coll.Add(DBNull.Value);//TotalVolume VARCHAR(50),
                        coll.Add(common.myStr(txtFlowRateUnit.Text));//FlowRate VARCHAR(50),
                        coll.Add(common.myInt(ddlFlowRateUnit.SelectedValue));//FlowRateUnit SMALLINT,
                        coll.Add(DBNull.Value);//VariableDoseDate SMALLDATETIME,
                        coll.Add(false);//IsSubstituteNotAllow bit,
                        coll.Add(DBNull.Value);//ICDCode varchar(200),
                        coll.Add(rowIdx);//SequenceNo tinyint,
                        coll.Add(DBNull.Value);//GenericId INT

                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }
            if (strXML1.ToString().Equals(string.Empty))
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO");
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : string.Empty;

            hshOutput = new Hashtable();
            bool ApprovalRequired = false;
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                hshOutput = objWM.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
                    common.myInt(ddlAdvisingDoctor.SelectedValue), strXML1.ToString(), strXML.ToString(), string.Empty,
                    Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0,
                    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                    isConsumable, sXMLFre, strXMLUnAppPrescIds.ToString(),0, false, ApprovalRequired, false, string.Empty,
                    true, common.myStr(txtCompoundedDrugName.Text).Trim(), common.myStr(txtCompoundedDose.Text), common.myInt(ddlCompoundedUnitId.SelectedValue),
                    CompoundedPrescriptionDetail, CompoundedItemId);
            }
            //else
            //{
            //    hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
            //                        common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ddlAdvisingDoctor.SelectedValue),
            //                        0, 0, strXML1.ToString(), strXML.ToString(), string.Empty,
            //                       common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
            //                        sXMLFre, isConsumable, strXMLUnAppPrescIds.ToString());
            //}
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
                                //strMsg = "Prescription No : " +
                                //    ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(hshOutput["@chvPrescriptionNo"])).Split('$')[0] +
                                //    " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
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
                            //strMsg = "Prescription No : " +
                            //    ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(hshOutput["@chvPrescriptionNo"])).Split('$')[0] +
                            //    " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
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

                Session["CurentSavedPrescriptionId"] = string.Empty;

                if (common.myInt(hshOutput["@intPrescriptionId"]) > 0)
                {
                    Session["CurentSavedPrescriptionId"] = common.myInt(hshOutput["@intPrescriptionId"]).ToString();
                    //dvConfirmPrint.Visible = true;
                }
                hdnCopyLastPresc.Value = "0";
                btnBrandDetailsViewOnItemBased.Visible = false;
                btnMonographViewOnItemBased.Visible = false;
            }
            lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);
            if (!common.myStr(lblMessage.Text).ToUpper().Contains("ALREADY PENDING")
                && common.myStr(lblMessage.Text).ToUpper().Contains("DATA SAVED"))
            {
                BindBlankItemGrid();

                BindPreviousDataGrid();

                clearControl();

                txtCompoundedDrugName.Text = string.Empty;
                txtCompoundedDose.Text = string.Empty;
                ddlCompoundedUnitId.SelectedIndex = 0;
            }

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
            dtPreviousData.Dispose();
        }
    }

    protected void saveDataFinalNormalDrug()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataSet dsXmlFrequency = new DataSet();
        DataSet dsVariableDose = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        Hashtable hshOutput = new Hashtable();

        DataTable dtPreviousData = new DataTable();
        string CompoundedPrescriptionDetail = string.Empty;
        string PrescriptionDetail = string.Empty;
        int CompoundedItemId = 0;
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

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                dtPreviousData = PreservePreviousItemData();
                //CompoundedPrescriptionDetail = GenerateCompoundedPrescriptionDetail(dtPreviousData);

                DateTime endDate = endDateCalculate(Convert.ToDateTime(txtStartDate.SelectedDate), common.myInt(txtDuration.Text), common.myStr(ddlPeriodType.SelectedValue));

                for (int rowIdx = 0; rowIdx < dtPreviousData.Rows.Count; rowIdx++)
                {
                    DataRow drPD = dtPreviousData.Rows[rowIdx];
                    coll1 = new ArrayList();

                    if (common.myInt(drPD["ItemId"]) > 0)
                    {
                        if (rowIdx == 0)
                        {
                            CompoundedItemId = common.myInt(drPD["ItemId"]);
                        }

                        PrescriptionDetail = GeneratePrescriptionDetail(common.myDbl(drPD["Dose"]), common.myStr(drPD["UnitName"]));

                        coll1.Add(0);//IndentId INT,
                        coll1.Add(common.myInt(drPD["ItemId"]));//ItemId INT,
                        coll1.Add(DBNull.Value);//CustomMedication VARCHAR(1000),
                        coll1.Add(0);//GenericId INT,
                        coll1.Add(common.myInt(ddlFormulation.SelectedValue));//FormulationId INT,
                        coll1.Add(common.myInt(ddlRoute.SelectedValue));//RouteId INT,
                        coll1.Add(DBNull.Value);//StrengthId INT,
                        coll1.Add(common.myDec(drPD["Qty"]));//TotalQty DECIMAL(10, 3),
                        coll1.Add((common.myLen(txtStartDate.SelectedDate) > 0) ? Convert.ToDateTime(txtStartDate.SelectedDate).ToString("yyyy-MM-dd") : string.Empty);//StartDate SMALLDATETIME,
                        coll1.Add((common.myLen(endDate) > 0) ? Convert.ToDateTime(endDate).ToString("yyyy-MM-dd") : string.Empty);//EndDate SMALLDATETIME,
                        coll1.Add(0);//NotToPharmacy BIT,
                        coll1.Add(common.myStr(drPD["OverrideComments"]));//OverrideComments VARCHAR(500),
                        coll1.Add(common.myStr(drPD["OverrideCommentsDrugToDrug"]));//OverrideCommentsDrugToDrug VARCHAR(500),
                        coll1.Add(common.myStr(drPD["OverrideCommentsDrugHealth"]));//OverrideCommentsDrugHealth VARCHAR(500),
                        coll1.Add(common.myStr(PrescriptionDetail));//PrescriptionDetail VARCHAR(1000),
                        coll1.Add(common.myStr(drPD["StrengthValue"]));//StrengthValue VARCHAR(255),
                        coll1.Add(common.myDec(drPD["Qty"]));//OriginalQty DECIMAL(10, 3)

                        strXML1.Append(common.setXmlTable(ref coll1));


                        //------------------------Item Details---------------------------------//

                        coll = new ArrayList();

                        coll.Add(common.myInt(drPD["ItemId"]));//ItemId INT,
                        coll.Add(common.myInt(ddlFrequencyId.SelectedValue));//FrequencyId TINYINT,      
                        coll.Add(common.myDec(drPD["Dose"]));//Dose DECIMAL(10, 3),      
                        coll.Add(common.myInt(txtDuration.Text));//Duration VARCHAR(10),
                        coll.Add(common.myStr(ddlPeriodType.SelectedValue));//Type CHAR(1),
                        coll.Add(common.myStr(txtInstructions.Text));//Instructions VARCHAR(2000),
                        coll.Add(common.myInt(drPD["UnitId"]));//UnitId INT,
                        coll.Add(-1);//ReferanceItemId INT,
                        coll.Add(common.myInt(ddlFoodRelation.SelectedValue));//FoodRelationshipId INT,
                        coll.Add(common.myInt(ddlDoseType.SelectedValue));//DoseTypeId INT,
                        coll.Add(DBNull.Value);//Volume VARCHAR(20),
                        coll.Add(DBNull.Value);//VolumeUnitId SMALLINT,
                        coll.Add(common.myStr(txtTimeInfusion.Text));//InfusionTime VARCHAR(20),

                        if (common.myInt(ddlTimeUnit.SelectedIndex) > 0)
                        {
                            coll.Add(common.myStr(ddlTimeUnit.SelectedValue));//TimeUnit CHAR(1),
                        }
                        else
                        {
                            coll.Add(DBNull.Value);//TimeUnit CHAR(1),
                        }

                        coll.Add(DBNull.Value);//TotalVolume VARCHAR(50),
                        coll.Add(common.myStr(txtFlowRateUnit.Text));//FlowRate VARCHAR(50),
                        coll.Add(common.myInt(ddlFlowRateUnit.SelectedValue));//FlowRateUnit SMALLINT,
                        coll.Add(DBNull.Value);//VariableDoseDate SMALLDATETIME,
                        coll.Add(false);//IsSubstituteNotAllow bit,
                        coll.Add(DBNull.Value);//ICDCode varchar(200),
                        coll.Add(rowIdx);//SequenceNo tinyint,
                        coll.Add(DBNull.Value);//GenericId INT

                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }
            if (strXML1.ToString().Equals(string.Empty))
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO");
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : string.Empty;

            hshOutput = new Hashtable();
            bool ApprovalRequired = false;
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                hshOutput = objWM.SaveEMRMedicineAddNewDrug(common.myInt(ViewState["SelectedIndentId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), strXML1.ToString(), strXML.ToString(), string.Empty, string.Empty,
                    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]));
            }
            //else
            //{
            //    hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //                        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
            //                        common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ddlAdvisingDoctor.SelectedValue),
            //                        0, 0, strXML1.ToString(), strXML.ToString(), string.Empty,
            //                       common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
            //                        sXMLFre, isConsumable, strXMLUnAppPrescIds.ToString());
            //}
            if ((common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("UPDATE") || common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("SAVED"))
                && !common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("USP"))
            {
                string strMsg = common.myStr(hshOutput["@chvErrorStatus"]);
                hdnReturnIndentDetailsIds.Value = string.Empty;

                if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                {

                    //////string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

                    //////ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                    //////return;
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                    //return;
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                Session["CurentSavedPrescriptionId"] = string.Empty;

                if (common.myInt(hshOutput["@intPrescriptionId"]) > 0)
                {
                    Session["CurentSavedPrescriptionId"] = common.myInt(hshOutput["@intPrescriptionId"]).ToString();
                    //dvConfirmPrint.Visible = true;
                }
                hdnCopyLastPresc.Value = "0";
                btnBrandDetailsViewOnItemBased.Visible = false;
                btnMonographViewOnItemBased.Visible = false;
            }
            lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);
            if (!common.myStr(lblMessage.Text).ToUpper().Contains("ALREADY PENDING")
                && common.myStr(lblMessage.Text).ToUpper().Contains("DATA SAVED"))
            {
                BindBlankItemGrid();

                BindPreviousDataGrid();

                clearControl();

                txtCompoundedDrugName.Text = string.Empty;
                txtCompoundedDose.Text = string.Empty;
                ddlCompoundedUnitId.SelectedIndex = 0;

                ViewState["SelectedIndentId"] = string.Empty;
                ViewState["ADDNORMALDRUG"] = string.Empty;
                dvCompoundedDrugName.Visible = common.myStr(ViewState["EMRCompoundedDrugNameRequired"]).ToUpper().Equals("Y");
            }

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
            dtPreviousData.Dispose();
        }
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
            }
            else
            {
                ddlRoute.Text = string.Empty;
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
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
            //if (common.myStr(ddlGeneric.Text).Length > 0)
            //{
            //    string selectedValue = common.myStr(e.Context["GenericId"]);
            //    if (common.myInt(selectedValue) > 0)
            //    {
            //        GenericId = common.myInt(selectedValue);
            //    }
            //}
            if (common.myInt(hdnStoreId.Value).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Store not selected !";
                return;
            }
            data = GetBrandData(common.myStr(e.Text), GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
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

            dsSearch = objPhr.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, itemBrandId, GenericId,
                                                common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
                                                text.Replace("'", "''"), WithStockOnly, string.Empty, iOT, (common.myStr(Session["OPIP"]).Equals("I")) ? "IP" : string.Empty,
                                                common.myBool(IsPackagePatient), common.myBool(IsPanelPatient),
                                                common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), "",0,0);

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
            ddlBrand.SelectedIndex = 0;
            ddlBrand.Text = string.Empty;

            hdnItemId.Value = "0";
            //ddlBrand.Text = string.Empty;
            //ddlBrand.SelectedValue = null;
            hdnGenericId.Value = "0";

            hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();

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

    protected void gvItem_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            clsCIMS objCIMS = new clsCIMS();
            try
            {
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                TextBox txtDoseGD = (TextBox)e.Row.FindControl("txtDoseGD");

                RadComboBox ddlUnitGD = (RadComboBox)e.Row.FindControl("ddlUnitGD");
                HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");

                RadComboBox ddlStrengthValueGD = (RadComboBox)e.Row.FindControl("ddlStrengthValueGD");
                HiddenField hdnStrengthValue = (HiddenField)e.Row.FindControl("hdnStrengthValue");

                TextBox txtQtyGD = (TextBox)e.Row.FindControl("txtQtyGD");

                txtDoseGD.Attributes.Add("onkeypress", "return clickEnterKey('" + txtDoseGD.ClientID + "', event)");
                txtQtyGD.Attributes.Add("onkeypress", "return clickEnterKey('" + txtQtyGD.ClientID + "', event)");

                txtDoseGD.Text = common.myDbl(txtDoseGD.Text).ToString("0.##");
                txtQtyGD.Text = common.myDbl(txtQtyGD.Text).ToString("0.##");

                try
                {
                    if (ViewState["UnitMaster"] != null)
                    {
                        DataTable dtUM = (DataTable)ViewState["UnitMaster"];
                        if (dtUM != null)
                        {
                            for (int rowIdx = 0; rowIdx < dtUM.Rows.Count; rowIdx++)
                            {
                                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                                item.Text = common.myStr(dtUM.Rows[rowIdx]["UnitName"]);
                                item.Value = common.myStr(common.myInt(dtUM.Rows[rowIdx]["Id"]));
                                item.Attributes.Add("IsUnitCalculationRequired", common.myStr(dtUM.Rows[rowIdx]["IsUnitCalculationRequired"]));
                                ddlUnitGD.Items.Add(item);
                            }

                            ddlUnitGD.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                            ddlUnitGD.SelectedIndex = 0;

                            ddlUnitGD.SelectedIndex = ddlUnitGD.Items.IndexOf(ddlUnitGD.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                        }
                        dtUM.Dispose();
                    }
                }
                catch
                {
                }

                try
                {
                    if (ViewState["StrengthMasterList"] != null)
                    {
                        DataTable dtSM = (DataTable)ViewState["StrengthMasterList"];
                        if (dtSM != null)
                        {
                            ddlStrengthValueGD.DataSource = dtSM;
                            ddlStrengthValueGD.DataTextField = "StrengthValue";
                            ddlStrengthValueGD.DataValueField = "StrengthValue";
                            ddlStrengthValueGD.DataBind();

                            ddlStrengthValueGD.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                            ddlStrengthValueGD.SelectedIndex = 0;

                            ddlStrengthValueGD.SelectedIndex = ddlStrengthValueGD.Items.IndexOf(ddlStrengthValueGD.Items.FindItemByValue(common.myStr(hdnStrengthValue.Value)));
                        }
                        dtSM.Dispose();
                    }
                }
                catch
                {
                }


                /////---------------------------------Interaction--------------------
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

    protected void btnRefresh_OnClick(object sender, System.EventArgs e)
    {
        try
        {
            //BindGrid(common.myStr(hdnReturnIndentIds.Value), common.myStr(hdnReturnItemIds.Value), common.myStr(hdnReturnGenericIds.Value));
        }
        catch
        {
        }
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
                //dvAllergy.RowFilter = "AllergyType='Drug' AND ItemType='P'";
                dvAllergy.RowFilter = "AllergyType='Drug'";
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
                //AddBrandInList();
                hdnItemId.Value = "0";
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;
                hdnGenericId.Value = "0";
                //ddlGeneric.Text = string.Empty;
                //ddlGeneric.SelectedValue = null;
                //txtCustomMedication.Text = string.Empty;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlDoseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFrequencyId.Enabled = true;
            //ddlReferanceItem.Enabled = true;
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
                //ddlReferanceItem.Enabled = false;
                txtDuration.Enabled = false;
                ddlPeriodType.Enabled = false;
            }
            else if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("URGENT"))
            {
                ddlFrequencyId.Enabled = true;
                //ddlReferanceItem.Enabled = true;
                txtDuration.Enabled = true;
                ddlPeriodType.Enabled = true;
            }
            else
            {
                // ddlFrequencyId.Items.Remove(0);
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));

                ddlFrequencyId.Enabled = true;
                //ddlReferanceItem.Enabled = true;
                txtDuration.Enabled = true;
                ddlPeriodType.Enabled = true;
            }
        }
        catch
        {
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
                foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
                    foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
                    foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
                foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
                foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
            foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
                foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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
            foreach (GridViewRow dataItem in gvPreviousItems.Rows)
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

            }
            else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = chkShowDetails.Checked;
            }
            setGridColor();
            //setVisiblilityInteraction();
        }
        catch
        {
        }
    }
    protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
    {
        try
        {
            AddBrandInList(null);

            dvConfirmAlreadyExistOptions.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmAlreadyExistOptions.Visible = false;
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
        }
        catch
        {
        }
        return dtData;
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (common.myBool(ViewState["HighValueItem"]))
            //{
            //    string strMsg = "This is high value item do you want to proceed";
            //    //Alert.ShowAjaxMsg(strMsg, this.Page);
            //}

            dvConfirm.Visible = false;
            //txtDose.Focus();
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
            //ddlBrand.Items.Clear();
            //ddlBrand.SelectedIndex  = 0;
            //  ddlBrand.Focus();
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

    protected void btnAddItem_OnClick(object sender, EventArgs e)
    {
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
                //AddOrderSet(common.myStr(ViewState["ICDCodes"]));
            }
            else
            {
                if (!isSavedItem())
                {
                    return;
                }
                if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
                {
                    //if (CallVariableDose(true).Equals(0))
                    //{
                    //    return;
                    //}
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
                string customMedication = "";// common.myStr(txtCustomMedication.Text).Trim();
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
                                            "&G=0" + //common.myStr(ddlGeneric.Text).Trim() +
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

            if (common.myInt(hdnGenericId.Value).Equals(0) && common.myInt(hdnItemId.Value).Equals(0))
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


            string sLOCATION = common.myStr(Request.QueryString["LOCATION"]);

            if (!sLOCATION.ToUpper().Equals("WARD"))
            {
                //if (common.myDbl(txtDose.Text).Equals(0.0) && spnDose.Visible)
                //{
                //    if (common.myStr(Session["FacilityName"]).ToUpper().Contains("SPS") || common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                //    {
                //        strmsg += "Qty can not be zero or blank !";
                //    }
                //    else
                //    {
                //        strmsg += "Dose can not be zero or blank !";
                //    }

                //    isSave = false;
                //}

                if (common.myDbl(txtDuration.Text).Equals(0)
                    && !common.myStr(ddlDoseType.Text).ToUpper().Equals("STAT") && spnDuration.Visible && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("TO BE CONTINUED") && !common.myStr(ddlPeriodType.Text).ToUpper().Equals("AS INSTRUCTED"))
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


            //if (!common.myBool(ViewState["IsPrimaryDiagnosis"]))
            //{
            //    if (!common.myStr(Session["OPIP"]).ToUpper().Equals("I") && trDiagnosis.Visible)
            //    {
            //        strmsg += "Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..";
            //        Alert.ShowAjaxMsg(strmsg, this);
            //        isSave = false;
            //    }
            //}
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

    protected void btnPrescriptionDetailConfirmYes_OnClick(object sender, System.EventArgs e)
    {
        try
        {
            saveDataFinal();

            dvPrescriptionDetailConfirm.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnPrescriptionDetailConfirmNo_OnClick(object sender, System.EventArgs e)
    {
        dvPrescriptionDetailConfirm.Visible = false;
    }

    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0
                && !common.myStr(ViewState["IsCancelWithOutRemarksInEmrPreviousMedication"]).Equals("Y"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Remarks can't be blank !";
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["SelectedIndentId"]) > 0)
            {
                Hashtable hshOutput = new Hashtable();

                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                common.myInt(ViewState["SelectedIndentId"]), 0, common.myInt(Session["UserId"]),
                                                common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                                common.myStr(btnStopMedication.Text).ToUpper().Equals("STOP") ? 0 : 1, common.myStr(txtStopRemarks.Text).Trim(),
                                                common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                BindPreviousDataGrid();

                ViewState["SelectedIndentId"] = "0";
                ViewState["SelectedCompoundedItemId"] = "0";

                if (common.myStr(hshOutput["@chvOutPut"]).ToUpper().Contains("SUCCESSFULL"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);

                return;
            }

            ViewState["SelectedIndentId"] = string.Empty;
            ViewState["SelectedCompoundedItemId"] = string.Empty;

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
        finally
        {
        }
    }

    protected void btnPreviousMedications_Click(Object sender, EventArgs e)
    {
        try
        {
            RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?IsFromCompounded=Y&Consumable=0&OPIP=" + common.myStr(Session["OPIP"]);
            RadWindow1.Height = 570;
            RadWindow1.Width = 850;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }

}
