using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Configuration;
using System.Net;
using BaseC;

public partial class EMR_Medication_PrescribeMedication : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    private string reportServer = System.Configuration.ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    private string reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = System.Configuration.ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = System.Configuration.ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = System.Configuration.ConfigurationManager.AppSettings["SysDomain"];

    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsEMR objEMR;
    BaseC.clsPharmacy objPharmacy;
    clsVIDAL objVIDAL;
    private const int ItemsPerRequest = 50;
    StringBuilder strXML;
    ArrayList coll;
    int count = 0;

    private Hashtable hstInput;
    string Saved_RTF_Content;
    StringBuilder sb = new StringBuilder();
    string Fonts = "";
    static string gBegin = "<u>";
    static string gEnd = "</u>";
    StringBuilder objStrTmp = new StringBuilder();
    private int iPrevId = 0;
    string sFontSize = "";
    string FlagShowPreviousStoreInDrugIndent = string.Empty;
    clsCIMS objCIMS = new clsCIMS();

    private enum enumColumns : byte
    {
        Sno = 0,
        Brand = 1,
        IndentType = 2,
        PackSize = 3,
        TotalQty = 4,
        Unit = 5,
        PrescriptionDetail = 6,
        MonographCIMS = 7,
        InteractionCIMS = 8,
        DHInteractionCIMS = 9,
        MonographVIDAL = 10,
        InteractionVIDAL = 11,
        DHInteractionVIDAL = 12,
        Edit = 13,
        Delete = 14
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (common.myInt(Session["EncounterId"]).Equals(0) && common.myInt(Request.QueryString["EncId"]).Equals(0))
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
            return;
        }
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        if (!IsPostBack)
        {
            BaseC.Security objSecurity = new BaseC.Security(sConString);  //
            ViewState["IsAllowToAddBlockedItem"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToAddBlockedItem");
            objSecurity = null;

            objVIDAL = new clsVIDAL(sConString);
            objCIMS = new clsCIMS();
            Session["DrugOrderDuplicateCheck"] = "0";

            ViewState["AllowZeroStockItemInIPPrescription"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInIPPrescription", sConString);

            ViewState["AllowZeroStockItemInIPPrescriptionMedication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInIPPrescriptionMedication", sConString);

            ViewState["AllowZeroStockItemInOPPrescription"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInOPPrescription", sConString);

            ViewState["AllowZeroStockItemInOPPrescriptionMedication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInOPPrescriptionMedication", sConString);
            // Added date 23.08.21
            // ViewState["IsIgnorePacksCalculationInWard"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsIgnorePacksCalculationInWard", sConString);

            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    "ShowStockQtyInPrescription,WardDurationInDaysForConsumableOrder,RemoteDirectPRintingURL,IsDirectPrintingRequiredInPrescription,IsShowFieldsForParas,isRequireIPBillOfflineMarking,IsItemExcludedItemAndCompanyWise,IsAllowSubtituteItemForLowCostItem", sConString);

            //added by bhakti
            if (collHospitalSetupValues.ContainsKey("IsItemExcludedItemAndCompanyWise"))
                ViewState["IsItemExcludedItemAndCompanyWise"] = common.myStr(collHospitalSetupValues["IsItemExcludedItemAndCompanyWise"]);

            if (collHospitalSetupValues.ContainsKey("isRequireIPBillOfflineMarking"))
                ViewState["isRequireIPBillOfflineMarking"] = common.myStr(collHospitalSetupValues["isRequireIPBillOfflineMarking"]);

            if (collHospitalSetupValues.ContainsKey("ShowStockQtyInPrescription"))
                ViewState["ShowStockQtyInPrescription"] = common.myStr(collHospitalSetupValues["ShowStockQtyInPrescription"]);
            if (collHospitalSetupValues.ContainsKey("WardDurationInDaysForConsumableOrder"))
                ViewState["WardDurationInDaysForConsumableOrder"] = common.myStr(collHospitalSetupValues["WardDurationInDaysForConsumableOrder"]);
            if (collHospitalSetupValues.ContainsKey("RemoteDirectPRintingURL"))
                ViewState["RemoteDirectPRintingURL"] = common.myStr(collHospitalSetupValues["RemoteDirectPRintingURL"]);
            if (collHospitalSetupValues.ContainsKey("IsDirectPrintingRequiredInPrescription"))
                ViewState["IsDirectPrintingRequiredInPrescription"] = common.myStr(collHospitalSetupValues["IsDirectPrintingRequiredInPrescription"]);
            if (collHospitalSetupValues.ContainsKey("IsShowFieldsForParas"))
                ViewState["IsShowFieldsForParas"] = common.myStr(collHospitalSetupValues["IsShowFieldsForParas"]);
            if (collHospitalSetupValues.ContainsKey("IsAllowSubtituteItemForLowCostItem"))
                ViewState["IsAllowSubtituteItemForLowCostItem"] = common.myStr(collHospitalSetupValues["IsAllowSubtituteItemForLowCostItem"]);

            if (common.myStr(Request.QueryString["LOCATION"]) == "WARD")
            {
                chkApprovalRequired.Visible = true;
            }
            if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
            {
                ViewState["Regno"] = common.myStr(Request.QueryString["Regno"]);
                ViewState["Encno"] = common.myStr(Request.QueryString["Encno"]);
                ViewState["RegId"] = common.myStr(Request.QueryString["RegId"]);
                ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
                //added by bhakti
                var str = common.myStr(Session["PaymentType"]);
                var str1 = common.myStr(Request.QueryString["EncId"]);
            }
            else
            {
                ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
                ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
                ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
                ViewState["EncId"] = common.myStr(Session["EncounterId"]);
            }

            if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
            {
                ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
                ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
                ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
                ViewState["EncId"] = common.myStr(Session["EncounterId"]);

                btnclose.Visible = false;
            }

            setTemplateData();

            ViewState["ConversioinFactor"] = true;
            hdnStoreId.Value = "0";
            hdnGenericId.Value = "0";
            hdnItemId.Value = "0";

            lblMessage.Text = "&nbsp;";
            ViewState["GridDataItem"] = null;

            btnPrint.Visible = common.myStr(Session["OPIP"]).Equals("O");

            lblTitle.Text = "Drug/Consumable Order";

            if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
            {
                //chkCustomMedication.Enabled = false;
                chkNotToPharmacy.Enabled = false;
                //  lnkStopMedication.Visible = false;
                btnPreviousMedications.Text = "Previous Order";
                lblDrugDetail.Text = "Consumable Indent";
                lblPrescription.Text = "Selected Indent";
                lblCurrentMedication.Visible = false;
                //ddlFormulation.Enabled = false;
                ddlStrength.Enabled = false;
                ddlRoute.Items.Insert(0, new RadComboBoxItem("", "0"));
                //ddlRoute.Enabled = false;
                lblTitle.Text = "Consumable Order";
                //ddlStore.Enabled = false;
            }
            bindControl();
            //if (common.myStr(ViewState["RegId"]) != "")
            //{
            //    BindPatientHiddenDetails();
            //}
            ShowPatientDetails();
            //PostBackOptions optionsSubmit = new PostBackOptions(btnSave);
            //btnSave.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";
            //btnSave.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);
            //Session["DrugReqSavecheck"] = 0;

            #region Interface

            setPatientInfo();
            getLegnedColor();

            ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
            ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");

            objEMR = new BaseC.clsEMR(sConString);
            DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForWordDrugRequisition);
            Session["CIMSDatabasePath"] = "";
            Session["CIMSDatabasePassword"] = "";

            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    ViewState["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                    Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                }
                else
                {
                    ViewState["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                }
            }

            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                string CIMSDatabasePath = string.Empty;
                if (dsInterface.Tables[0].Rows.Count > 0)
                {
                    CIMSDatabasePath = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                }

                if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc") && !File.Exists(CIMSDatabasePath + "FastTrackData.mr2"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "CIMS database not available !";
                    //Alert.ShowAjaxMsg("CIMS database not available !", this);
                }
            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
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

                    //    //Alert.ShowAjaxMsg(lblMessage.Text, this);

                    //    return;
                    //}
                }
                catch
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "VIDAL web-services not running now !";

                    //Alert.ShowAjaxMsg(lblMessage.Text, this);

                    return;
                }
            }

            #endregion
            ViewState["IsSaleInPackUnit"] = common.myStr(objBill.getHospitalSetupValue("IsSaleInPackUnitWard", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            BindGrid("", "");
            BindBlankItemGrid();
            BindBlankItemDetailGrid();
            BindPatientProvisionalDiagnosis();
            ddlGeneric.Focus();
            if (common.myStr(ViewState["IsShowFieldsForParas"]) == "Y")
            {
                ViewState["IsShowFieldsForParas"] = "Y";
                dvParas.Visible = true;
                bindUniqueCat();
            }

            if (common.myStr(ViewState["IsSaleInPackUnit"]) == "Y")
            {
                divgvItemDetails.Visible = false;
            }
            else
            {
                divgvItemDetails.Visible = true;
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            {
                btnSave.Visible = false;
            }
            if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
            {
                btnclose.Visible = true;
            }

            #region set focus
            //ddlBrand.Focus();
            //ddlBrand.Attributes.Add("onkeypress", "return clickEnterInGrid('" + gvItemDetails.ClientID + "', event)");

            //RadComboBox ddlFrequencyId = gvItemDetails.Rows[0].Cells[1].FindControl("ddlFrequencyId") as RadComboBox;
            //TextBox txtDuration = gvItemDetails.Rows[0].Cells[2].FindControl("txtDuration") as TextBox;
            //RadComboBox ddlPeriodType = gvItemDetails.Rows[0].Cells[2].FindControl("ddlPeriodType") as RadComboBox;

            //ddlBrand.Attributes.Add("onkeypress", "return clickEnterInGrid('" + ddlFrequencyId.ClientID + "', event)");
            //ddlFrequencyId.Attributes.Add("onkeypress", "return clickEnterInGrid('" + txtDuration.ClientID + "', event)");
            //txtDuration.Attributes.Add("onkeypress", "return clickEnterInGrid('" + ddlPeriodType.ClientID + "', event)");
            //ddlPeriodType.Attributes.Add("onkeypress", "return clickEnterInGrid('" + btnAddItem.ClientID + "', event)");
            #endregion

            ViewState["PrescriptionAllowForItemsNotInStock"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                             common.myInt(Session["FacilityId"]), "PrescriptionAllowForItemsNotInStock", sConString);


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


            FlagShowPreviousStoreInDrugIndent = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(Session["FacilityId"]), "ShowRequestingWardInDrugIndent", sConString);
            if (FlagShowPreviousStoreInDrugIndent.Equals("Y"))
            {
                ChkReqestFromOtherWard.Visible = true;
                lblRequestFromOtherWard.Visible = true;
                bindPatientPreviousWards();
            }
            else
            {
                ChkReqestFromOtherWard.Visible = false;
                lblRequestFromOtherWard.Visible = false;
            }
            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
            {
                chkApprovalRequired.Visible = false;
            }
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);

            CheckNoOfIndentInOneDay();


            int CompanyId = 0;
            if (!common.myInt(Request.QueryString["CompanyId"]).Equals(0))
            {
                CompanyId = common.myInt(Request.QueryString["CompanyId"]);
            }
            else
            {
                CompanyId = common.myInt(Session["SponsorIdPayorId"]);
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet dsCompany = objPharmacy.GetLowCostCompanyTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(CompanyId), common.myInt(Session["FacilityId"]));
            if (dsCompany.Tables.Count > 0)
            {
                if (dsCompany.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(dsCompany.Tables[0].Rows[0]["Active"]) == true)
                    {
                        ViewState["IsCompanyLowCost"] = true;
                    }
                    else
                    {
                        ViewState["IsCompanyLowCost"] = false;
                    }

                }
                else
                {
                    ViewState["IsCompanyLowCost"] = false;
                }
            }
            else
            {
                ViewState["IsCompanyLowCost"] = false;
            }

        }


        //Legend1.loadLegend("CIMSInterface", "");

        setGridColor();

    }


    //Added on 19-Mar-2015 Start


    void bindPatientProfileItemDetails()
    {
        try
        {
            //hdnTotQty.Value = "0";
            //hdnTotUnit.Value = "0";
            //hdnTotCharge.Value = "0";
            //hdnTotTax.Value = "0";
            //hdnTotDiscAmt.Value = "0";
            //hdnTotPatientAmt.Value = "0";
            //hdnTotPayerAmt.Value = "0";
            //hdnTotNetAmt.Value = "0";

            DataSet dsItem = new DataSet();
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            dsItem = objPharmacy.getPatientProfileItemDetails(common.myInt(ddlProfileItem.SelectedValue), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

            addItemFromKit(dsItem.Tables[0]);
            //if (dsItem.Tables[0].Rows.Count == 0)
            //{
            //    lblMessage.Text = "Item not found !";
            //    return;
            //}

            //for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
            //{
            //    dsItem.Tables[0].Rows[rIdx]["Qty"] = 0;
            //}

            //dsItem.Tables[0].AcceptChanges();

            ////ViewState["RequestFromWardItems"] = null;

            //if (dsItem.Tables[0].Rows.Count == 0)
            //{
            //    DataRow newdr = dsItem.Tables[0].NewRow();
            //    dsItem.Tables[0].Rows.Add(newdr);
            //}
            //else
            //{
            //    ViewState["RequestFromWardItems"] = dsItem.Tables[0];

            //}
            //gvService.DataSource = dsItem.Tables[0];
            //gvService.DataBind();

            //setVisiblilityInteraction();

            //txtRegistrationNo.Enabled = false;
            //txtEncounter.Enabled = false;
            //txtPatientName.Enabled = false;
            //txtAge.Enabled = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    private void bindProfileItem()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.getProfileItems(common.myInt(Session["HospitalLocationID"]));

            ddlProfileItem.DataSource = ds.Tables[0];
            ddlProfileItem.DataTextField = "ItemName";
            ddlProfileItem.DataValueField = "ItemId";
            ddlProfileItem.DataBind();
            ddlProfileItem.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlProfileItem.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void btnSurgicalKit_OnClick(object sender, EventArgs e)
    {
        try
        {
            //txtNetAmount.Text = Request[txtNetAmount.UniqueID];
            //txtLAmt.Text = Request[txtLAmt.UniqueID];
            //txtRounding.Text = Request[txtRounding.UniqueID];
            //txtReceived.Text = Request[txtReceived.UniqueID];

            dvConfirmProfileItem.Visible = false;
            //if (common.myStr(txtPatientName.Text).Trim() == "")
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Patient not selected ! ";
            //    return;
            //}
            bindProfileItem();
            dvConfirmProfileItem.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

    }

    protected void btnOkProfileItem_OnClick(object sender, EventArgs e)
    {
        bindPatientProfileItemDetails();
        dvConfirmProfileItem.Visible = false;
    }

    protected void btnCancelProfileItem_OnClick(object sender, EventArgs e)
    {
        dvConfirmProfileItem.Visible = false;
    }

    //Added on 19-Mar-2015 End

    private void setTemplateData()
    {
        BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();

        try
        {
            tdTemplate.InnerHtml = BindEditor(true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objbc = null;
            ds.Dispose();
        }
    }

    private void getLegnedColor()
    {
        try
        {
            ViewState["DrugMonographColor"] = "#98AFC7";
            ViewState["DrugtoDrugInteractionColor"] = "#ECBBBB";

            BaseC.clsBb objBb = new BaseC.clsBb(sConString);
            DataSet ds = objBb.GetStatusMaster("CIMSInterface");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.RowFilter = "Code='MO'";

                if (ds.Tables[0].DefaultView.Count > 0)
                {
                    ViewState["DrugMonographColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                }

                ds.Tables[0].DefaultView.RowFilter = "";

                ds.Tables[0].DefaultView.RowFilter = "Code='IN'";

                if (ds.Tables[0].DefaultView.Count > 0)
                {
                    ViewState["DrugtoDrugInteractionColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                }

                ds.Tables[0].DefaultView.RowFilter = "";

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void setGridColor()
    {
        if (common.myBool(ViewState["IsCIMSInterfaceActive"])
            || common.myBool(ViewState["IsVIDALInterfaceActive"]))
        {



            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                {
                    LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                }
                else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
                {
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");

                    //lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                }
            }
        }
    }

    private void bindControl()
    {
        DataSet dsRoute = new DataSet();
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataView dv = new DataView();
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            /*************** Store ***************/

            //BaseC.clsPharmacy objMaster = new BaseC.clsPharmacy(sConString);
            //ds = objPharmacy.GetStoreToChangefromWard(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["GroupID"]), 
            //                                        common.myInt(Session["FacilityId"]), "IPD Issue", 0);

            string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();
            //if (ChkReqestFromOtherWard.Checked)
            //{
            //    bindGetPreviousStores();
            //}
            //else
            //{
            //    bindDdlstore();
            //}

            bindDdlstore();

            GetPatientEncounterStatus();
            /*************** Advising Doctor ***************/
            BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
            tbl = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);

            ddlAdvisingDoctor.DataSource = tbl;
            ddlAdvisingDoctor.DataTextField = "DoctorName";
            ddlAdvisingDoctor.DataValueField = "DoctorId";
            ddlAdvisingDoctor.DataBind();

            ddlAdvisingDoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlAdvisingDoctor.SelectedValue = common.myStr(Session["DoctorId"]);

            /*************** Formulation ***************/
            ds = new DataSet();
            ds = objPharmacy.GetFormulationMaster(0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));

            ddlFormulation.DataSource = ds.Tables[0];
            ddlFormulation.DataTextField = "FormulationName";
            ddlFormulation.DataValueField = "FormulationId";
            ddlFormulation.DataBind();

            ddlFormulation.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlFormulation.SelectedIndex = 0;

            /*************** Route ***************/
            //if (Request.QueryString["DRUGORDERCODE"] == null)
            //{

            //ddlRoute.Items.Clear();


            OpPrescription OpPre = new OpPrescription(sConString);
            dsRoute = new DataSet();
            dsRoute = OpPre.dsGetRouteDetails();
            if (dsRoute.Tables[0].Rows.Count > 0)
            {
                ddlRoute.Items.Clear();
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataValueField = "ID";
                ddlRoute.DataSource = dsRoute.Tables[0];
                ddlRoute.DataBind();
                dv = new DataView(dsRoute.Tables[0]);
                dv.RowFilter = "IsDefault=1";
                if (dv.ToTable().Rows.Count > 0)
                {
                    ddlRoute.SelectedValue = common.myStr(dv.ToTable().Rows[0]["Id"]);
                }
            }
            else
            {
                ddlRoute.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlRoute.SelectedIndex = 0;
            }
            //}

            /*************** Strength ***************/
            objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = new DataSet();
            ds = objPharmacy.GetItemStrength(0, 0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));

            ddlStrength.DataSource = ds.Tables[0];
            ddlStrength.DataValueField = "StrengthId";
            ddlStrength.DataTextField = "Strength";
            ddlStrength.DataBind();

            ddlStrength.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlStrength.SelectedIndex = 0;

            /*************** Frequency ***************/
            ds = objPharmacy.getFrequencyMaster();
            ViewState["FrequencyData"] = ds.Tables[0];

            ds = new DataSet();

            //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            ds = objEMR.getEMRPrescriptionRemarks(0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, 0);
            ViewState["InstructionData"] = ds.Tables[0];


            ds = new DataSet();
            ds = objPharmacy.GetFood(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ViewState["FoodRelation"] = ds.Tables[0];

            ds = new DataSet();
            BaseC.clsBb status = new BaseC.clsBb(sConString);
            ds = status.GetStatusMaster("DoseType");
            ViewState["DoseType"] = ds.Tables[0];

            ds = new DataSet();
            ds = objEMR.GetIndentType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            DataView dvIndentType = new DataView(ds.Tables[0]);
            dvIndentType.RowFilter = "isnull(IndentCode,'')<>'CONOD'";
            //ddlIndentType Change before after
            int AfterIndentDischarge = 0, BeforeIndentDischarge = 0;
            if (!common.myStr(ViewState["MarkForDischarge"]).Equals(""))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i]["AfterIndentDischarge"]).Equals("True"))// after
                    {
                        AfterIndentDischarge = 1;

                    }
                }
                if (AfterIndentDischarge.Equals(1))
                {
                    dvIndentType.RowFilter = "isnull(AfterIndentDischarge,'')='True'";

                }
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i]["BeforeIndentDischarge"]).Equals("True"))// before
                    {
                        BeforeIndentDischarge = 1;

                    }
                }
                if (BeforeIndentDischarge.Equals(1))
                {
                    dvIndentType.RowFilter = "isnull(BeforeIndentDischarge,'')='True'";
                }
            }
            //ddlIndentType Change before after


            ddlIndentType.DataSource = dvIndentType.ToTable();
            ddlIndentType.DataValueField = "Id";
            ddlIndentType.DataTextField = "IndentType";
            ddlIndentType.DataBind();
            dvIndentType.Dispose();
            if (AfterIndentDischarge.Equals(1))
            {
                ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByText(common.myStr("Discharge")));

            }
            ds = new DataSet();
            ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            ViewState["UnitMaster"] = ds.Tables[0];
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
            dv.Dispose();
            dsRoute.Dispose();
            tbl.Dispose();
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
    public DataSet GetphrGenericOfSelectedDrug(int HospId, int FacilityId, int ItemID)
    {
        Hashtable HshIn = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            HshIn.Add("ItemID", ItemID);
            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrGenericRouteUnitOfSelectedDrug", HshIn);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            HshIn = null;
            ds.Dispose();
            objDl = null;
        }
        return ds;
    }

    protected void btnSave_Onclick(object sender, EventArgs e)
    {
        if (common.myBool(hdnIsPasswordRequired.Value)) //If transactional username & password required
        {
            IsValidPassword();
            return;
        }
        //if (common.myBool(ViewState["IsReasonManditory"]))
        //{
        //    if (common.myInt(ddlReason.SelectedValue).Equals(0))
        //    {
        //        lblMessage.Text = "Reason not selected !";
        //        return;
        //    }
        //}

        //added by bhakti
        try
        {
            if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["PaymentType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
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
        CheckNoOfIndentInOneDay();
    }
    private void saveData()
    {
        try
        {
            DataSet dsXml;
            DataTable dt;
            strXML = new StringBuilder();
            StringBuilder strXML1 = new StringBuilder();
            BaseC.Patient patient = new BaseC.Patient(sConString);
            coll = new ArrayList();
            ArrayList coll1 = new ArrayList();
            BaseC.WardManagement ward = new BaseC.WardManagement();
            DataSet ds = new DataSet();
            //yogesh 09_02_2022
            DataTable tempDt = new DataTable();
            DataTable tempDt1 = new DataTable();

            //if (common.myInt(Session["DrugReqSavecheck"]) == 0)
            //{
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                lblMessage.Text = "Patient has no appointment !";
                return;
            }
            if (common.myInt(ddlAdvisingDoctor.SelectedValue) == 0)
            {
                lblMessage.Text = "Advising Doctor not selected!";
                return;
            }



            // Akshay_Tirathram_12082022
            DataTable ItemExpensiveDetails = new DataTable();
            ItemExpensiveDetails.Columns.Add("ItemId");
            ItemExpensiveDetails.Columns.Add("ItemName");
            ItemExpensiveDetails.Columns.Add("IsExpensive");
            ItemExpensiveDetails.Columns.Add("OrderedQty");
            ItemExpensiveDetails.Columns.Add("MaxNarcoticDrugsQty");
            ItemExpensiveDetails.Columns.Add("TotalQty");

            double iConversionFactor = 0;
            double sQuantity = 0;
            if (!common.myStr(Session["OPIP"]).Equals("I"))
            {
                #region OP Drug
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");

                    if (common.myBool(ViewState["Stop"]))
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myDbl(txtTotalQty.Text).Equals(0.00))
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == "")
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");

                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT

                        dsXml = new DataSet();
                        string xmlSchema = "";
                        dt = new DataTable();
                        if (hdnXMLData.Value == "")
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        DataView dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        dt = dv.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = "", sEndDate = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = dt.Rows[i]["Dose"].ToString();
                                //string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = dt.Rows[i]["Duration"].ToString();
                                string sType = dt.Rows[i]["Type"].ToString();
                                if (!dt.Columns.Contains("UnitId"))
                                {
                                    dt.Columns.Add("UnitId", typeof(int));

                                }
                                int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                if (i == 0)
                                {
                                    if (common.myBool(ViewState["Stop"]))
                                    {
                                        sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        sStartDate = dt.Rows[i]["StartDate"].ToString();
                                    }
                                }
                                if (i == dt.Rows.Count - 1)
                                {
                                    if (common.myBool(ViewState["Stop"]))
                                    {
                                        sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        sEndDate = dt.Rows[i]["EndDate"].ToString();
                                    }
                                }
                                string sInstructions = dt.Rows[i]["Instructions"].ToString();
                                int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);
                                coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
                                coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                coll.Add(common.myInt(iUnit));//UNITID INT,
                                coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                            coll1.Add(sStartDate != "" ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != "" ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,



                            coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
                            coll1.Add(DBNull.Value);//ICD CODE VARCHAR
                            coll1.Add(common.myInt(0));//Refill INT
                            coll1.Add(common.myBool(0));//Is Override BIT
                            coll1.Add(DBNull.Value);//OverrideComments VARCHAR
                            coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
                            coll1.Add(common.myInt(424));//PrescriptionModeId INT
                            coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR

                            strXML1.Append(common.setXmlTable(ref coll1));
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
                    HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");



                    // Akshay_Tirathram_12082022
                    BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);


                    DataSet dsdataset = objPhr.GetPhrItemMaster(Convert.ToInt32(hdnItemId.Value), "", 0, common.myInt(Session["HospitalLocationId"]));
                    tempDt = dsdataset.Tables[1];
                    //Kuldeep + yogesh_02_09_2022
                    tempDt1 = dsdataset.Tables[2];
                    ItemExpensiveDetails.Rows.Add();
                    int index = ItemExpensiveDetails.Rows.Count;
                    if (tempDt.Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["ItemId"] = tempDt.Rows[0]["ItemId"];
                        ItemExpensiveDetails.Rows[index - 1]["ItemName"] = tempDt.Rows[0]["ItemName"];
                        ItemExpensiveDetails.Rows[index - 1]["IsExpensive"] = tempDt.Rows[0]["IsExpensive"];
                    }
                    DataSet tempDs1 = new DataSet();
                    tempDs1 = objPhr.GetNarcoticDrugsDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]), Convert.ToInt32(hdnItemId.Value));
                    // yogesh 2_09_2022




                    if (tempDs1.Tables[0].Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["MaxNarcoticDrugsQty"] = tempDs1.Tables[0].Rows[0]["MaxDrugDose"];
                    }
                    ItemExpensiveDetails.Rows[index - 1]["OrderedQty"] = common.myInt(txtTotalQty.Text);
                    DataSet tempDs2 = new DataSet();
                    BaseC.WardManagement wards = new BaseC.WardManagement();
                    tempDs2 = wards.GetNarcoticDetailsForOneDay(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]), common.myInt(ViewState["RegId"]), Convert.ToInt32(hdnItemId.Value));
                    if (tempDs2.Tables[0].Rows.Count > 0)
                    {
                        ItemExpensiveDetails.Rows[index - 1]["TotalQty"] = Convert.ToDecimal(tempDs2.Tables[0].Compute("SUM(Qty)", string.Empty)) + Convert.ToDecimal(txtTotalQty.Text);
                    }
                    else
                    {
                        ItemExpensiveDetails.Rows[index - 1]["TotalQty"] = Convert.ToDecimal(txtTotalQty.Text);
                    }
                    tempDt = null;
                    tempDs1 = null;







                    if (common.myBool(ViewState["Stop"]))
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myDbl(txtTotalQty.Text).Equals(0.00))
                    {
                        Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == "")
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        //string IsIgnorePacksCalculationInWard = ViewState["IsIgnorePacksCalculationInWard"].ToString();

                        //if (IsIgnorePacksCalculationInWard == "")
                        //{
                        //    ViewState["IsIgnorePacksCalculationInWard"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsIgnorePacksCalculation", sConString);

                        //}

                        //if (common.myStr(ViewState["IsSaleInPackUnitWard"]) == "Y")  // Commented By Akshay_Mission_01082022 Added new condition
                        if (common.myStr(ViewState["IsSaleInPackUnit"]) == "Y")
                        {
                            #region TotalQty Roundoff Calucation
                            if (common.myBool(ViewState["ConversioinFactor"]))
                            {
                                ds = ward.GetItemConversionFactor(common.myInt(hdnItemId.Value));

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    iConversionFactor = common.myDbl(ds.Tables[0].Rows[0]["ConversionFactor2"]);
                                }
                                if (iConversionFactor != 0)
                                {
                                    for (int i = 1; i <= iConversionFactor * common.myDbl(txtTotalQty.Text); i++)
                                    {
                                        if (common.myDbl(txtTotalQty.Text) <= iConversionFactor * i)
                                        {
                                            sQuantity = iConversionFactor * i;
                                            break;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        if (common.myBool(ViewState["ConversioinFactor"]) && iConversionFactor != 0)
                        {
                            sQuantity = common.myDbl(sQuantity.ToString("F2"));
                        }
                        else
                        {
                            sQuantity = common.myDbl(txtTotalQty.Text);
                        }

                        coll1.Add(common.myInt(hdnIndentId.Value));//IndentId INT,
                        if (common.myInt(hdnItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }

                        coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT                                                                     
                        coll1.Add(common.myDec(sQuantity));//TotalQty INT

                        dsXml = new DataSet();
                        string xmlSchema = "";
                        dt = new DataTable();
                        if (hdnXMLData.Value == "")
                        {
                            Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        DataView dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        dt = dv.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = "", sEndDate = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = common.myStr(dt.Rows[i]["Dose"]);
                                // string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = common.myStr(dt.Rows[i]["Duration"]);
                                string sType = common.myStr(dt.Rows[i]["Type"]);
                                if (!dt.Columns.Contains("UnitId"))
                                {
                                    dt.Columns.Add("UnitId", typeof(int));
                                }

                                int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                if (i == 0)
                                {
                                    if (common.myBool(ViewState["Stop"]))
                                    {
                                        sStartDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        sStartDate = common.myStr(dt.Rows[i]["StartDate"]);
                                    }
                                }
                                if (i == dt.Rows.Count - 1)
                                {
                                    if (common.myBool(ViewState["Stop"]))
                                    {
                                        sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                    }
                                }
                                string sInstructions = common.myStr(dt.Rows[i]["Instructions"]);
                                int iReferanceItemId = common.myInt(dt.Rows[i]["ReferanceItemId"]);
                                int iFoodRelationshipID = common.myInt(dt.Rows[i]["FoodRelationshipID"]);
                                int iDoseTypeId = common.myInt(dt.Rows[i]["DoseTypeId"]);

                                coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT,
                                coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
                                coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
                                coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
                                coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
                                coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
                                coll.Add(common.myInt(iUnit));//UNITID INT,
                                coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
                                coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
                                coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,

                                strXML.Append(common.setXmlTable(ref coll));
                            }

                            coll1.Add(sStartDate != "" ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != "" ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
                            coll1.Add(common.myBool(hdnNotToPharmcy.Value) ? 1 : 0);//NotToPharmcy BIT
                            coll1.Add(string.Empty);//OverrideComments
                            coll1.Add(string.Empty);//OverrideCommentsDrugToDrug
                            coll1.Add(string.Empty);//OverrideCommentsDrugHealth
                            coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR(1000)
                            coll1.Add(string.Empty);//StrengthValue
                            coll1.Add(common.myDec(txtTotalQty.Text));//Prescription detail VARCHAR(1000)


                            strXML1.Append(common.setXmlTable(ref coll1));
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                #endregion
            }



            //Akshay_Tirathram_12082022
            DataView dv1 = new DataView(ItemExpensiveDetails);
            DataView dv2 = new DataView(ItemExpensiveDetails);
            dv2.RowFilter = "IsExpensive = " + true;

            string ExpensiveDrugs = "";
            for (int i = 0; i < dv2.ToTable().Rows.Count; i++)
            {
                ExpensiveDrugs += common.myStr(dv2.ToTable().Rows[i]["ItemName"]) + ", ";
            }

            if (dv2.ToTable().Rows.Count > 0)
            {
                Alert.ShowAjaxMsg("There are some expensive drugs: " + ExpensiveDrugs, Page);
            }


            dv1.RowFilter = " TotalQty < = MaxNarcoticDrugsQty";
            string NarcoticDrugsdetails = "";
            for (int i = 0; i < dv1.ToTable().Rows.Count; i++)
            {
                NarcoticDrugsdetails += common.myStr(dv1.ToTable().Rows[i]["ItemName"]) + "-" + common.myStr(dv1.ToTable().Rows[i]["OrderedQty"]) + "-" + common.myStr(dv1.ToTable().Rows[i]["MaxNarcoticDrugsQty"]) + ", ";
            }
            //Kuldeep + yogesh 9_02_2022
            DataView dvNarcotics = new DataView(tempDt1);

            if (dvNarcotics.ToTable().Rows.Count > 0)
            {
                dvNarcotics.RowFilter = "ItemFlagName='NARCOTIC'";

                if (dvNarcotics.ToTable().Rows.Count > 0)
                {
                    if (dv1.ToTable().Rows.Count > 0)
                    {
                        var totalQul = Convert.ToDecimal(ItemExpensiveDetails.Rows[0]["TotalQty"]);
                        var MaxNarcoticValue = Convert.ToDecimal(ItemExpensiveDetails.Rows[0]["MaxNarcoticDrugsQty"]);
                        if (totalQul > MaxNarcoticValue)
                        {
                            Alert.ShowAjaxMsg("There are some Narcotic Drugs which can not order more than maximum quantity in a Day (Item Name - Ordered Qty - MaxQty): " + NarcoticDrugsdetails, Page);
                            return;
                        }
                    }
                }

            }



            if (strXML.ToString() == string.Empty)
            {
                lblMessage.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" ? true : false;
            Hashtable hshOutput = new Hashtable();
            if (common.myStr(Session["DrugOrderDuplicateCheck"]) == "0")
            {
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    BaseC.WardManagement objwd = new BaseC.WardManagement();
                    bool ApprovalRequired = chkApprovalRequired.Checked;

                    hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
                            common.myInt(ddlAdvisingDoctor.SelectedValue), strXML1.ToString(), strXML.ToString(), common.myStr(txtremarks.Text),
                            Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0,
                            common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                            isConsumable, string.Empty, string.Empty, ((ChkReqestFromOtherWard.Checked) ? common.myInt(ddlReqestFromOtherWard.SelectedValue) : 0),
                            common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"), ApprovalRequired, common.myBool(chkIsReadBack.Checked), common.myStr(txtIsReadBackNote.Text),
                            false, string.Empty, string.Empty, 0, string.Empty, 0, common.myInt(ddlReason.SelectedValue));
                }
                else
                {
                    objEMR = new BaseC.clsEMR(sConString);
                    hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                            common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
                            common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ddlAdvisingDoctor.SelectedValue),
                            0, 0, strXML1.ToString(), strXML.ToString(), "",
                            common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                            "", isConsumable, string.Empty);
                }

                if ((hshOutput["@chvErrorStatus"].ToString().Contains("UPDATE") || hshOutput["@chvErrorStatus"].ToString().Contains("Saved"))
                    && !hshOutput["@chvErrorStatus"].ToString().Contains("USP"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                    Session["DrugOrderDuplicateCheck"] = "1";
                    hdnGenericId.Value = "0";
                    hdnGenericName.Value = "";
                    hdnItemId.Value = "0";
                    hdnItemName.Value = "";
                    ddlIndentType.SelectedValue = "0";

                    hdnCIMSItemId.Value = "";
                    hdnCIMSType.Value = "";
                    hdnVIDALItemId.Value = "";

                    clearItemDetails();

                    chkCustomMedication.Checked = false;
                    chkCustomMedication_OnCheckedChanged(null, null);
                    if (common.myStr(ViewState["IsDirectPrintingRequiredInPrescription"]) == "Y")
                    {
                        PrintPrescription(common.myInt(hshOutput["@intPrescriptionId"]));
                    }

                }

                lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);

                if (common.myStr(lblMessage.Text).Contains("Saved"))
                //&& !common.myStr(lblMessage.Text).Contains("ALREADY PENDING") && !common.myStr(lblMessage.Text).ToUpper().Contains("RESTRICT") && !common.myStr(lblMessage.Text).ToUpper().Contains("DURATION CANNOT BE MORE THAN"))
                {
                    BindGrid("", "");
                    BindBlankItemGrid();
                    ViewState["Item"] = null;
                    ViewState["Stop"] = null;
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
            Session["DrugOrderDuplicateCheck"] = "0";
        }
    }
    void BindPatientHiddenDetails()
    {
        try
        {
            //if (Session["PatientDetailString"] != null)
            //{
            //    lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            //}
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlGeneric_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;

        if (common.myStr(ddlGeneric.Text.Trim()) == "" || common.myStr(e.Text.Trim()) == "")
        {
            ddlGeneric.SelectedValue = "";
            ddlGeneric.Text = "";
        }

        DataTable data = GetGenericData(e.Text);
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            ddl.Items.Add(new RadComboBoxItem(common.myStr(data.Rows[i]["GenericName"]), common.myStr(data.Rows[i]["GenericId"])));
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);


    }

    private DataTable GetGenericData(string text)
    {
        DataSet dsSearch = new DataSet();

        objPharmacy = new BaseC.clsPharmacy(sConString);
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int EncodedBy = common.myInt(Session["UserId"]);
        int GenericId = 0;
        int Active = 1;
        dsSearch = objPharmacy.GetGenericDetails(GenericId, text, Active, HospId, EncodedBy);
        return dsSearch.Tables[0];
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found...";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected void ddlBrand_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        //if (e.Text.Length <= 2)
        //{
        //    return;
        //}
        RadComboBox ddl = sender as RadComboBox;


        //int GenericId = 0;
        DataTable data = new DataTable();
        //if (common.myStr(ddlGeneric.Text).Length > 0)
        //{
        //    string selectedValue = common.myStr(e.Context["GenericId"]);
        //    if (common.myInt(selectedValue) > 0)
        //    {
        //        GenericId = common.myInt(selectedValue);
        //    }
        //}

        if (common.myInt(hdnStoreId.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Store not selected !";

            return;
        }
        //if (common.myInt(ddlGeneric.SelectedValue) > 0)
        //{
        //    data = GetBrandData(e.Text, GenericId);
        //}

        if (chkFavouriteOnly.Checked)
        {
            data = GetBrandData(e.Text, 0);
        }
        else
        {
            //if (e.Text != "" && e.Text.Length > 2)
            //{
            //    data = GetBrandData(e.Text, GenericId);
            //}
            if (common.myStr(ddlGeneric.Text.Trim()) == "")
            {
                ddlGeneric.SelectedValue = "";
            }

            data = GetBrandData(e.Text, common.myInt(ddlGeneric.SelectedValue));
        }
        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;
        try
        {
            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);
                //  item.Attributes.Add("ItemSubCategoryShortName", data.Rows[i]["ItemSubCategoryShortName"].ToString());
                item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));
                item.Attributes.Add("CIMSItemId", common.myStr(data.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(data.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myStr(data.Rows[i]["VIDALItemId"]));
                item.Attributes.Add("RestrictedItemForPanel", common.myStr(data.Rows[i]["RestrictedItemForPanel"]));
                item.Attributes.Add("ItemFlagName", common.myStr(data.Rows[i]["ItemFlagName"]));
                item.Attributes.Add("ItemFlagCode", common.myStr(data.Rows[i]["ItemFlagCode"]));
                item.Attributes.Add("ConversionFactor2", common.myStr(data.Rows[i]["ConversionFactor2"]));
                item.Attributes.Add("LowCost", common.myStr(data.Rows[i]["LowCost"]));

                if (!common.myBool(data.Rows[i]["LowCost"]) && common.myBool(ViewState["IsCompanyLowCost"]))
                {
                    item.BackColor = System.Drawing.Color.LightCoral;
                }

                this.ddlBrand.Items.Add(item);
                item.DataBind();
                Label lblClosingBalanceItem = (Label)ddlBrand.Items[i].FindControl("lblClosingBalanceItem");
                lblClosingBalanceItem.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }




        if (common.myStr(ViewState["ShowStockQtyInPrescription"]).Equals("Y"))
        {
            for (int j = itemOffset; j < endOffset; j++)
            {
                Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
                lblClosingBalanceItem.Visible = true;
            }
        }
        //else
        //{
        //for (int j = itemOffset; j < endOffset; j++)
        //{
        //    Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
        //    lblClosingBalanceItem.Visible = false;

        //}
        //}

        e.Message = GetStatusMessage(endOffset, data.Rows.Count);

    }

    private DataTable GetBrandData(string text, int GenericId)
    {
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();

        if (common.myStr(ddlGeneric.Text.Trim()) == "")
        {
            GenericId = 0;
        }

        objPharmacy = new BaseC.clsPharmacy(sConString);
        objEMR = new BaseC.clsEMR(sConString);

        int StoreId = common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
        int ItemId = 0;

        int itemBrandId = 0;
        int WithStockOnly = 0;

        int iOT = 3;
        int CompanyId = 0;

        if (common.myStr(Request.QueryString["LOCATION"]).Equals("OT")
            && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
        {
            iOT = 2;
        }
        else if (common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")
            && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO"))
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
            ItemId = common.myInt(ViewState["ItemId"]);
        }

        if (chkFavouriteOnly.Checked)
        {
            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }

            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                             common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0,
                             0, common.myInt(DoctorId), "", "");
        }
        //else
        //{
        //dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId == 0 && ViewState["ItemId"] != null ? Convert.ToInt32(ViewState["ItemId"]) : ItemId, itemBrandId, GenericId,
        //    common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
        //   text.Replace("'", "''"), WithStockOnly, "", iOT);
        //}

        if (dsSearch.Tables.Count > 0)
        {
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                dt = dsSearch.Tables[0];
            }
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
            if (ViewState["AllowZeroStockItemInIPPrescription"].Equals("Y"))
            {
                WithStockOnly = 0;
            }
            else
            {
                if (ViewState["AllowZeroStockItemInIPPrescriptionMedication"].Equals("Y"))
                {
                    WithStockOnly = 0;
                }
                else
                {
                    WithStockOnly = 1;
                }
            }
        }
        else
        {
            if (ViewState["AllowZeroStockItemInOPPrescription"].Equals("Y"))
            {
                WithStockOnly = 0;
            }
            else
            {
                if (ViewState["AllowZeroStockItemInOPPrescriptionMedication"].Equals("Y"))
                {
                    WithStockOnly = 0;
                }
                else
                {
                    WithStockOnly = 1;
                }
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
        if (!chkFavouriteOnly.Checked)
        {
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId,
                                              ItemId == 0 && common.myInt(ViewState["ItemId"]) > 0 ? common.myInt(ViewState["ItemId"]) : ItemId,
                                              itemBrandId, GenericId, common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]),
                                              0, text.Replace("'", "''"), WithStockOnly, "", iOT, "IP",
                                              common.myBool(IsPackagePatient), common.myBool(IsPanelPatient), common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), "I", CompanyId, common.myInt(ddlUniqueCategory.SelectedValue));
            }
            else
            {
                dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId,
                                              ItemId == 0 && common.myInt(ViewState["ItemId"]) > 0 ? common.myInt(ViewState["ItemId"]) : ItemId,
                                              itemBrandId, GenericId, common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]),
                                              0, text.Replace("'", "''"), WithStockOnly, "", iOT, string.Empty,
                                              common.myBool(IsPackagePatient), common.myBool(IsPanelPatient), common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), "I", CompanyId, common.myInt(ddlUniqueCategory.SelectedValue));
            }

        }

        if (dsSearch.Tables.Count > 0)
        {
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                if (!common.myBool(hdnLowCost.Value) && common.myBool(ViewState["IsCompanyLowCost"]))
                {
                    if (common.myStr(ViewState["IsAllowSubtituteItemForLowCostItem"]).ToUpper() == "Y")
                    {
                        dt = dsSearch.Tables[0];
                    }
                    else
                    {
                        DataView DV = new DataView(dsSearch.Tables[0]);
                        DV.RowFilter = "LowCost=true";
                        dt = DV.ToTable();
                    }
                }
                else
                {
                    dt = dsSearch.Tables[0];
                }
            }
        }
        return dt;
    }

    protected void btnAddtoFav_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (!common.myStr(hdnItemId.Value).Equals(string.Empty) && ddlBrand.SelectedValue != null && !chkCustomMedication.Checked)
            {
                int DoctorId = 0;
                int FrequencyId = 0;
                int Duration = 0;
                string DurationType = string.Empty;
                double Dose = 0.0;
                int UnitId = 0;
                int FoodRelationshipId = 0;
                string Instructions = string.Empty;

                DoctorId = common.myInt(Request.QueryString["DoctorId"]);
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

                foreach (GridViewRow dataItem in gvItemDetails.Rows)
                {
                    RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
                    TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
                    RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
                    TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
                    RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
                    RadComboBox ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
                    TextBox txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

                    FrequencyId = common.myInt(ddlFrequencyId.SelectedValue);
                    Duration = common.myInt(txtDuration.Text);
                    DurationType = common.myStr(ddlPeriodType.SelectedValue);
                    Dose = common.myDbl(txtDose.Text);
                    UnitId = common.myInt(ddlUnit.SelectedValue);
                    FoodRelationshipId = common.myInt(ddlFoodRelation.SelectedValue);
                    Instructions = common.myStr(txtInstructions.Text).Trim();

                    break;
                }

                string strMsg = objEMR.SaveFavouriteDrugs(common.myInt(DoctorId), common.myInt(ddlBrand.SelectedValue), string.Empty, common.myInt(Session["UserID"]),
                                                Dose, UnitId, 0, common.myInt(ddlFormulation.SelectedValue), common.myInt(ddlRoute.SelectedValue),
                                                FrequencyId, Duration, DurationType, FoodRelationshipId, string.Empty, Instructions, 0);

                lblMessage.Text = strMsg;
            }
            else
            {
                Alert.ShowAjaxMsg("Please select drug to save favourite", Page);
                return;
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

    protected void ddlPeriodType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        RadComboBox PeriodType = (RadComboBox)sender;
        GridViewRow item = (GridViewRow)PeriodType.NamingContainer;
        RadDatePicker txtStartDate = (RadDatePicker)item.FindControl("txtStartDate");
        RadDatePicker txtEndDate = (RadDatePicker)item.FindControl("txtEndDate");

        TextBox txtDuration = (TextBox)item.FindControl("txtDuration");
        RadComboBox ddlPeriodType = (RadComboBox)item.FindControl("ddlPeriodType");
        TextBox txtDose = (TextBox)item.FindControl("txtDose");
        txtEndDate.SelectedDate = endDateChange(txtStartDate, txtDuration.Text, ddlPeriodType.SelectedValue);
        txtDose.Focus();
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();
    }

    protected void txtDuration_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox Duration = (TextBox)sender;
            GridViewRow item = (GridViewRow)Duration.NamingContainer;
            RadDatePicker txtStartDate = (RadDatePicker)item.FindControl("txtStartDate");
            RadDatePicker txtEndDate = (RadDatePicker)item.FindControl("txtEndDate");
            TextBox txtDose = (TextBox)item.FindControl("txtDose");
            TextBox txtDuration = (TextBox)item.FindControl("txtDuration");
            RadComboBox ddlPeriodType = (RadComboBox)item.FindControl("ddlPeriodType");
            txtEndDate.SelectedDate = endDateChange(txtStartDate, txtDuration.Text, ddlPeriodType.SelectedValue);
            //txtDose.Focus();

            TextBox txtInstructions = (TextBox)item.FindControl("txtInstructions");
            txtInstructions.Focus();
        }
        catch
        {
        }
    }

    protected void txtStartDate_OnSelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        RadDatePicker datePicker1 = (RadDatePicker)sender;
        GridViewRow item = (GridViewRow)datePicker1.NamingContainer;
        RadDatePicker txtStartDate = (RadDatePicker)item.FindControl("txtStartDate");
        RadDatePicker txtEndDate = (RadDatePicker)item.FindControl("txtEndDate");

        if (common.myInt(FindCurrentDate(txtStartDate.SelectedDate.Value.ToString("dd/MM/yyyy")))
           < common.myInt(FindCurrentDate(Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy"))))
        {
            txtStartDate.SelectedDate = System.DateTime.Now;
            Alert.ShowAjaxMsg("Past date selection not allowed", Page);
            return;
        }

        TextBox txtDuration = (TextBox)item.FindControl("txtDuration");
        RadComboBox ddlPeriodType = (RadComboBox)item.FindControl("ddlPeriodType");
        txtEndDate.SelectedDate = endDateChange(txtStartDate, txtDuration.Text, ddlPeriodType.SelectedValue);
    }
    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = "";
        string newCurrentDate = "";
        string currentdate = formatdate.FormatDateDateMonthYear(outputCurrentDate);
        string strformatCurrDate = formatdate.FormatDate(currentdate, "MM/dd/yyyy", "yyyy/MM/dd");
        firstCurrentDate = strformatCurrDate.Remove(4, 1);
        newCurrentDate = firstCurrentDate.Remove(6, 1);
        return newCurrentDate;
    }
    private DateTime endDateChange(RadDatePicker txtStartDate, string sDuration, string Type)
    {
        DateTime dt = common.myDate(txtStartDate.SelectedDate);
        int days = common.myInt(sDuration);
        double dHours = 0.0;
        switch (Type)
        {
            case "H":
                days = days * 1;
                if (days > 24)
                {
                    dHours = days - 24;
                    if (common.myInt(dHours) < 24)
                    {
                        days = 2;
                    }
                }
                else if (days <= 24)
                {
                    days = 1;
                }
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
                days = days * 1;
                break;
            default:
                days = days * 1;
                break;
        }
        return dt.AddDays(days - 1);
    }


    private int CalculateDays(string sDuration, string Type)
    {
        int days = common.myInt(sDuration);
        double dHours = 0.0;
        switch (Type)
        {
            case "H":
                days = days * 1;
                if (days > 24)
                {
                    dHours = days - 24;
                    if (common.myInt(dHours) < 24)
                    {
                        days = 2;
                    }
                }
                else if (days <= 24)
                {
                    days = 1;
                }
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
        return days;
    }
    private string calcTotalQty(string sDose, string sFrequency, string sDay, string sType)
    {
        string sTotal = "";
        try
        {
            if (common.myDbl(sDose) == 0)
            {
                sDose = "1";
            }
            if (common.myDbl(sDay) == 0)
            {
                sDay = "1";
            }
            double dose = common.myDbl(sDose);
            double frequency = common.myDbl(sFrequency);
            double days = common.myDbl(sDay);
            double totalQty = 0;

            switch (common.myStr(sType))
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
            totalQty = dose * frequency * days;
            sTotal = totalQty.ToString("F2");

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return sTotal;
    }
    protected void gvItem_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO")
                    && (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
                {
                    e.Row.Cells[3].Visible = true;
                }
                else
                {
                    // e.Row.Cells[3].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TextBox txtTotalQty = (TextBox)e.Row.FindControl("txtTotalQty");
                TextBox txtPackSize = (TextBox)e.Row.FindControl("txtPackSize");
                HiddenField hdnConversionFactor2 = (HiddenField)e.Row.FindControl("hdnConversionFactor2");

                gvItem.Columns[3].Visible = false;

                if (common.myStr(ViewState["IsSaleInPackUnit"]).ToUpper().Equals("Y"))
                {
                    gvItem.Columns[3].Visible = true;
                    //  txtTotalQty.Attributes.Add("readonly", "readonly");

                    if (common.myInt(txtPackSize.Text) > 0 || common.myInt(hdnConversionFactor2.Value) > 0)
                    {
                        txtTotalQty.Text = (common.myInt(txtPackSize.Text) * common.myInt(hdnConversionFactor2.Value)).ToString();
                        if (common.myInt(txtTotalQty.Text).Equals(0))
                        {
                            txtTotalQty.Text = "1";
                        }
                        txtPackSize.Attributes.Add("onkeyup", "javascript:CalculateQty('" + txtPackSize.ClientID + "','" + hdnConversionFactor2.ClientID + "','" + txtTotalQty.ClientID + "');");
                    }
                    txtPackSize.Enabled = false;// Akshay
                }
                else
                {
                    gvItem.Columns[(byte)enumColumns.PackSize].Visible = false;
                    gvItem.Columns[(byte)enumColumns.Unit].Visible = false;
                    txtTotalQty.ReadOnly = false;

                }
                #region CIMS VIDAL
                gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;

                gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;

                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0)
                    {
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");

                        string strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        if (strXML != "")
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);

                            if (!outputValues.ToUpper().Contains("<MONOGRAPH>"))
                            {
                                lnkBtnMonographCIMS.Visible = false;
                            }
                        }
                    }
                }
                else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
                {

                    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;


                    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");

                    //lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                    if (common.myStr(hdnVIDALItemId.Value).Trim().Length == 0)
                    {
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                    }
                }
                #endregion

                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
                Label lblItemName = (Label)e.Row.FindControl("lblItemName");
                HiddenField hdnXMLData = (HiddenField)e.Row.FindControl("hdnXMLData");
                HiddenField hdnCustomMedication = (HiddenField)e.Row.FindControl("hdnCustomMedication");



                //string IsIgnorePacksCalculationInWard = ViewState["IsIgnorePacksCalculationInWard"].ToString();

                //if (IsIgnorePacksCalculationInWard == "")
                //{
                //    ViewState["IsIgnorePacksCalculationInWard"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsIgnorePacksCalculationInWard", sConString);

                //}
                //if (common.myStr(ViewState["IsSaleInPackUnitWard"]) == "N")
                //{
                //    gvItem.Columns[(byte)enumColumns.PackSize].Visible = false;
                //    gvItem.Columns[(byte)enumColumns.Unit].Visible = false;
                //    txtTotalQty.ReadOnly = false;

                //}
                #region LASA / Non-LASA Item
                HiddenField hdnItemFlagCode = (HiddenField)e.Row.FindControl("hdnItemFlagCode");
                if (common.myStr(hdnItemFlagCode.Value).Equals("LASA"))
                {
                    e.Row.Cells[(byte)enumColumns.Brand].BackColor = System.Drawing.Color.LightGray;
                }
                else if (common.myStr(hdnItemFlagCode.Value).Equals("SC") || common.myStr(hdnItemFlagCode.Value).Equals("HighValue"))
                {
                    e.Row.Cells[(byte)enumColumns.Brand].BackColor = System.Drawing.Color.Yellow;
                }
                //added by bhakti
                else if (common.myStr(hdnItemFlagCode.Value).Equals("HighRisk"))
                {
                    e.Row.Cells[(byte)enumColumns.Brand].BackColor = System.Drawing.Color.Olive;
                }
                #endregion

                if (ViewState["ItemDetail"] != null)
                {
                    DataTable dt = (DataTable)ViewState["ItemDetail"];
                    DataView dv = new DataView(dt);
                    //if (hdnCustomMedication.Value == "1")
                    //{
                    //    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                    //}
                    //else
                    //{
                    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                    //}
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

                        if (dv.ToTable().Rows.Count > 1)
                        {
                            DataTable tblPres = dt.Copy().Clone();
                            if (e.Row.RowIndex <= dt.Rows.Count)
                            {
                                tblPres.Rows.Add(dt.Copy().Rows[e.Row.RowIndex].ItemArray);
                            }
                            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(tblPres);
                        }
                        else
                        {
                            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv.ToTable());
                        }

                        // dt.TableName = "ItemDetail";
                        StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.

                        StringWriter writer = new StringWriter(builder);// Write the schema into the StringWriter.
                        dv.ToTable().WriteXml(writer, XmlWriteMode.IgnoreSchema);

                        string xmlSchema = writer.ToString();
                        hdnXMLData.Value = xmlSchema;
                    }
                    else
                    {
                        DataView dv1 = new DataView(dt);
                        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnItemId.Value);
                        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv1.ToTable());

                            dt.TableName = "ItemDetail";
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                            dt.WriteXml(writer, XmlWriteMode.IgnoreSchema);

                            string xmlSchema = writer.ToString();
                            hdnXMLData.Value = xmlSchema;
                        }
                        else
                        {
                            lblPrescriptionDetail.Text = "";
                        }
                    }

                }
                if (common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO")
                    && (common.myStr(Request.QueryString["LOCATION"]).Equals("OT") || common.myStr(Request.QueryString["LOCATION"]).Equals("WARD")))
                {
                    e.Row.Cells[3].Visible = true;
                }
                else
                {
                    //   e.Row.Cells[3].Visible = false;
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
    private void FillEditedItem()
    {

        DataTable dt = new DataTable();
        DataRow dr;

        dt.Columns.Add(new System.Data.DataColumn("IndentNo", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("IndentDate", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("IndentTypeId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("IndentType", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("GenericId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("GenericName", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("ItemId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("IndentId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("FormulationId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("RouteId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("StrengthId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("CIMSItemId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("CIMSType", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("VIDALItemId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("Qty", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("PrescriptionDetail", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("ReferanceItemId", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("StartDate", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("EndDate", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("CustomMedication", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("NotToPharmacy", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("XMLData", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("IsInfusion", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("UnitName", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("PackSize", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("ConversionFactor2", typeof(String)));

        dt.Columns.Add(new System.Data.DataColumn("ItemFlagName", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("ItemFlagCode", typeof(String)));
        //dt.Columns.Add(new System.Data.DataColumn("RestrictedItemForPanel", typeof(String)));


        foreach (GridViewRow row in gvItem.Rows)
        {
            HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
            HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
            HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
            HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
            HiddenField hdnCIMSItemId = (HiddenField)row.FindControl("hdnCIMSItemId");
            HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");
            HiddenField hdnVIDALItemId = (HiddenField)row.FindControl("hdnVIDALItemId");
            HiddenField hdnIsInfusion = (HiddenField)row.FindControl("hdnIsInfusion");
            HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
            HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
            HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
            HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");
            HiddenField hdnNotToPharmcy = (HiddenField)row.FindControl("hdnNotToPharmcy");

            HiddenField hdnItemFlagName = (HiddenField)row.FindControl("hdnCustomMedication");
            HiddenField hdnItemFlagCode = (HiddenField)row.FindControl("hdnNotToPharmcy");
            //HiddenField hdnRestrictedItemForPanel = (HiddenField)row.FindControl("hdnRestrictedItemForPanel");


            Label lblItemName = (Label)row.FindControl("lblItemName");
            Label lblIndentType = (Label)row.FindControl("lblIndentType");
            HiddenField hdnIndentTypeId = (HiddenField)row.FindControl("hdnIndentTypeId");
            TextBox txtPackSize = (TextBox)row.FindControl("txtPackSize");
            HiddenField hdnConversionFactor2 = (HiddenField)row.FindControl("hdnConversionFactor2");

            TextBox txtTotalQty = (TextBox)row.FindControl("txtTotalQty");
            Label lblItemUnit = (Label)row.FindControl("lblItemUnit");
            Label lblPrescriptionDetail = (Label)row.FindControl("lblPrescriptionDetail");

            LinkButton lnkBtnMonographCIMS = (LinkButton)row.FindControl("lnkBtnMonographCIMS");
            LinkButton lnkBtnMonographVIDAL = (LinkButton)row.FindControl("lnkBtnMonographVIDAL");
            LinkButton ibtnEdit = (LinkButton)row.FindControl("ibtnEdit");
            ImageButton ibtnDelete = (ImageButton)row.FindControl("ibtnDelete");

            dr = dt.NewRow();
            dr[0] = string.Empty;//IndentNo
            dr[1] = "";//IndentDate
            dr[2] = hdnIndentTypeId.Value;
            dr[3] = lblIndentType.Text;
            dr[4] = hdnGenericId.Value;
            dr[5] = hdnGenericName.Value;
            dr[6] = hdnItemId.Value;
            dr[7] = hdnIndentId.Value;
            dr[8] = lblItemName.Text;
            dr[9] = hdnFormulationId.Value;
            dr[10] = hdnRouteId.Value;
            dr[11] = hdnStrengthId.Value;

            dr[12] = hdnCIMSItemId.Value;
            dr[13] = hdnCIMSType.Value;
            dr[14] = hdnVIDALItemId.Value;
            dr[15] = txtTotalQty.Text;
            dr[16] = lblPrescriptionDetail.Text;
            dr[17] = string.Empty;//ReferanceItemId
            dr[18] = string.Empty;//StartDate
            dr[19] = string.Empty;//EndDate
            dr[20] = hdnCustomMedication.Value;
            dr[21] = hdnNotToPharmcy.Value;
            dr[22] = hdnXMLData.Value;
            dr[23] = hdnIsInfusion.Value;
            dr[24] = lblItemUnit.Text;
            dr[25] = txtPackSize.Text;
            dr[26] = hdnConversionFactor2.Value;
            dr[27] = hdnItemFlagName.Value;
            dr[28] = hdnItemFlagCode.Value;
            //dr[27] = hdnRestrictedItemForPanel.Value;

            dt.Rows.Add(dr);
        }

        ViewState["Item"] = dt;
        gvItem.DataSource = dt;
        gvItem.DataBind();
    }
    protected void gvItem_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                int ItemId = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);
                FillEditedItem();

                if (ViewState["ItemDetail"] != null && (ItemId > 0 || GenericId > 0))
                {
                    DataTable tbl = (DataTable)ViewState["ItemDetail"];
                    DataTable tbl1 = (DataTable)ViewState["Item"];
                    DataView DV = tbl.Copy().DefaultView;
                    DataView DV1 = tbl1.Copy().DefaultView;
                    if (ItemId > 0)
                    {
                        DV.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
                        DV1.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
                    }
                    else
                    {
                        DV.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
                        DV1.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
                    }

                    tbl = DV.ToTable();

                    ViewState["GridDataItem"] = DV1.ToTable();
                    ViewState["ItemDetail"] = tbl;
                    ViewState["Item"] = DV1.ToTable();

                    if (tbl.Rows.Count == 0)
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();
                    }

                    gvItem.DataSource = DV1.ToTable();
                    gvItem.DataBind();

                    if (DV1.ToTable().Rows.Count == 0)
                    {
                        BindBlankItemGrid();
                    }

                    ViewState["StopItemDetail"] = null;
                    ViewState["Edit"] = null;
                    ViewState["ItemId"] = 0;
                    setVisiblilityInteraction();
                }
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
                {
                    Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showIntreraction();
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "MonographVIDAL")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName == "InteractionVIDAL")
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName == "DHInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction("H");

            }
            else if (e.CommandName == "Select")
            {
                //ddlBrand.Items.Clear();
                //ddlBrand.Text = "";
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                Label lblItemName = (Label)row.FindControl("lblItemName");

                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
                ViewState["ItemId"] = ItemId;
                //DataTable dtSearch = GetBrandData("", GenericId);
                //if (dtSearch.Rows.Count > 0)
                //{
                //    ddlBrand.DataSource = dtSearch;
                //    ddlBrand.DataValueField = "ItemId";
                //    ddlBrand.DataTextField = "ItemName";
                //    ddlBrand.DataBind();
                //    ddlBrand.SelectedValue = ItemId.ToString();
                //    ddlBrand.Enabled = false;
                //}
                ddlBrand.Text = lblItemName.Text;
                ddlBrand.SelectedValue = ItemId.ToString();
                ddlBrand.Enabled = false;
                ddlGeneric.SelectedValue = GenericId.ToString();
                DataTable dt = (DataTable)ViewState["ItemDetail"];

                DataView dv = new DataView(dt);
                dv.RowFilter = "ItemId=" + ItemId;
                if (common.myInt(dv.ToTable().Rows.Count) > 0)
                {
                    ViewState["StopItemDetail"] = dv.ToTable();
                }
                BindStopItemDetail();

                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                objPharmacy = new BaseC.clsPharmacy(sConString);
                DataSet dsCalisreq = objPharmacy.ISCalculationRequired(ItemId);
                ViewState["ISCalculationRequired"] = null;
                if (dsCalisreq.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
                }
                ViewState["Edit"] = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void ibtnFavourite_Click(object sender, EventArgs e)
    {
        //if (ddlBrand.SelectedValue != "")
        //{
        //string sDoctorId = Request.QueryString["DoctorId"] == null ? common.myStr(Session["DoctorID"]) : Request.QueryString["DoctorId"].ToString();
        int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
        if (DoctorId.Equals(0))
        {
            DoctorId = common.myInt(Session["LoginDoctorId"]);
        }
        if (DoctorId.Equals(0))
        {
            DoctorId = common.myInt(Session["EmployeeId"]);
        }

        RadWindow1.NavigateUrl = "~/EMR/Medication/DrugFavourite.aspx?DoctorId=" + DoctorId + "&ItemId=" + ddlBrand.SelectedValue
            + "&ItemName=" + ddlBrand.Text + "&GenericId=" + ddlGeneric.SelectedValue;
        RadWindow1.Height = 500;
        RadWindow1.Width = 500;
        RadWindow1.Top = 200;
        RadWindow1.Left = 500;
        RadWindow1.OnClientClose = "OnClientCloseFavourite";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("Please select drug to add favourite", Page);
        //    return;
        //}
    }

    private void BindStopItemDetail()
    {
        DataTable dt = new DataTable();
        dt = CreateItemDetailTable();
        DataRow dr;
        int count = 0;
        if (ViewState["StopItemDetail"] != null)
        {
            DataTable dt1 = (DataTable)ViewState["StopItemDetail"];

            if (ViewState["Stop"] == null)
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
                    dr["FrequencyName"] = "";
                    dr["Duration"] = common.myStr(dt1.Rows[i]["Duration"]);
                    dr["Type"] = common.myStr(dt1.Rows[i]["Type"]);
                    dr["DurationText"] = common.myStr(dt1.Rows[i]["DurationText"]);
                    dr["Dose"] = common.myStr(dt1.Rows[i]["Dose"]);
                    dr["UnitId"] = common.myInt(dt1.Rows[i]["UnitId"]);
                    dr["UnitName"] = "";
                    dr["FoodRelationshipId"] = 0;
                    dr["FoodRelationship"] = "";

                    if (common.myStr(Session["OPIP"]).Equals("I"))
                    {
                        dr["StartDate"] = count.Equals(0) ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(1);
                    }

                    {
                        dr["EndDate"] = null;
                    }

                    //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                    //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
                    //dr["Qty"] = "";
                    dr["PrescriptionDetail"] = "";
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
                dr["RouteName"] = common.myStr(dt1.Rows[0]["RouteName"]);
                dr["FrequencyId"] = common.myInt(dt1.Rows[0]["FrequencyId"]);
                dr["FrequencyName"] = "";
                dr["Duration"] = common.myStr(dt1.Rows[0]["Duration"]);
                dr["Type"] = common.myStr(dt1.Rows[0]["Type"]);
                dr["DurationText"] = common.myStr(dt1.Rows[0]["DurationText"]);
                dr["Dose"] = common.myStr(dt1.Rows[0]["Dose"]);
                dr["UnitId"] = common.myInt(dt1.Rows[0]["UnitId"]);
                dr["UnitName"] = "";
                dr["FoodRelationshipId"] = 0;
                dr["FoodRelationship"] = "";
                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    dr["StartDate"] = count.Equals(0) ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(1);
                }
                else
                {
                    dr["StartDate"] = null;
                }
                dr["EndDate"] = count.Equals(0) ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[0]["Duration"]) - 1) : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(common.myInt(dt1.Rows[0]["Duration"]));

                //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
                //dr["Qty"] = "";
                dr["PrescriptionDetail"] = "";
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
        gvItemDetails.DataSource = dt;
        gvItemDetails.DataBind();
        ViewState["StopItemDetail"] = null;
        ViewState["Stop"] = null;

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            lblMessage.Text = "";
            RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(Session["EncounterId"]);
            RadWindow1.Height = 650;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkAddNewRow_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataRow DR;
        RadDatePicker txtEndDate = new RadDatePicker();
        HiddenField hdnId = new HiddenField();
        if (ViewState["DataTableDetail"] == null)
        {
            dt = (DataTable)CreateItemDetailTable();
        }
        else
        {
            dt = (DataTable)ViewState["DataTableDetail"];
        }
        for (int i = 0; i < gvItemDetails.Rows.Count; i++)
        {
            // DR = dt.NewRow();
            string sFrequency = "";

            RadComboBox ddlFrequencyId = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlFrequencyId");
            TextBox txtDose = (TextBox)gvItemDetails.Rows[i].FindControl("txtDose");
            if (ddlFrequencyId.SelectedValue != "0")
            {
                sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            }
            TextBox txtDuration = (TextBox)gvItemDetails.Rows[i].FindControl("txtDuration");
            RadComboBox ddlPeriodType = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlPeriodType");

            Label lblTotalQty = (Label)gvItemDetails.Rows[i].FindControl("lblTotalQty");
            string Type = "";
            decimal dDuration = 0;

            switch (common.myStr(ddlPeriodType.SelectedValue))
            {
                case "N":
                    Type = txtDuration.Text + " Minute(s)";
                    dDuration = 1;
                    break;
                case "H":
                    Type = txtDuration.Text + " Hour(s)";
                    dDuration = 1;
                    break;
                case "D":
                    Type = txtDuration.Text + " Day(s)";
                    dDuration = 1;
                    break;
                case "W":
                    Type = txtDuration.Text + " Week(s)";
                    dDuration = 7;
                    break;
                case "M":
                    Type = txtDuration.Text + " Month(s)";
                    dDuration = 30;
                    break;
                case "Y":
                    Type = txtDuration.Text + " Year(s)";
                    dDuration = 365;
                    break;
            }
            RadComboBox ddlUnit = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlUnit");
            HiddenField Remarks = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnRemarks");
            RadComboBox ddlFoodRelation = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlFoodRelation");
            RadDatePicker txtStartDate = (RadDatePicker)gvItemDetails.Rows[i].FindControl("txtStartDate");
            txtEndDate = (RadDatePicker)gvItemDetails.Rows[i].FindControl("txtEndDate");
            HiddenField hdnCIMSItemId = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnCIMSItemId");
            HiddenField hdnCIMSType = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnCIMSType");
            HiddenField hdnVIDALItemId = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnVIDALItemId");
            HiddenField hdnQty = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnQty");
            RadComboBox ddlReferanceItem = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlReferanceItem");

            RadComboBox ddlDoseType = (RadComboBox)gvItemDetails.Rows[i].FindControl("ddlDoseType");

            TextBox txtInstructions = (TextBox)gvItemDetails.Rows[i].FindControl("txtInstructions");


            hdnId = (HiddenField)gvItemDetails.Rows[i].FindControl("hdnId");

            //decimal dQty = Convert.ToDecimal(sFrequency) * Convert.ToDecimal(txtDose.Text == "" ? 0 : Convert.ToDecimal(txtDose.Text)) * Convert.ToDecimal(txtDuration.Text == "" ? 0 : Convert.ToDecimal(txtDuration.Text)) * dDuration;
            //hdnQty.Value = dQty.ToString("F2");

            dt.Rows[i]["Id"] = hdnId.Value;
            dt.Rows[i]["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
            dt.Rows[i]["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;

            dt.Rows[i]["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
            dt.Rows[i]["ItemId"] = ddlBrand.SelectedValue == "" ? 0 : common.myInt(ddlBrand.SelectedValue);
            dt.Rows[i]["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
            dt.Rows[i]["ItemName"] = ddlBrand.SelectedValue == "" ? "" : ddlBrand.Text;
            dt.Rows[i]["IndentId"] = 0;
            dt.Rows[i]["RouteId"] = ddlRoute.SelectedValue;
            dt.Rows[i]["RouteName"] = ddlRoute.SelectedItem.Text;
            dt.Rows[i]["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
            dt.Rows[i]["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
            dt.Rows[i]["Dose"] = common.myDbl(txtDose.Text);
            dt.Rows[i]["Frequency"] = (common.myInt(ddlFrequencyId.SelectedValue) == 0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            dt.Rows[i]["Duration"] = txtDuration.Text;
            dt.Rows[i]["DurationText"] = Type;
            dt.Rows[i]["Type"] = ddlPeriodType.SelectedValue;
            dt.Rows[i]["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            dt.Rows[i]["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");
            dt.Rows[i]["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
            dt.Rows[i]["FoodRelationship"] = ddlFoodRelation.SelectedValue == "" ? "" : common.myStr(ddlFoodRelation.Text);
            dt.Rows[i]["UnitId"] = ddlUnit.SelectedValue == "" ? 0 : common.myInt(ddlUnit.SelectedValue);
            dt.Rows[i]["UnitName"] = ddlUnit.SelectedValue == "" ? "" : ddlUnit.Text;
            dt.Rows[i]["CIMSItemId"] = common.myInt(hdnCIMSItemId.Value);
            dt.Rows[i]["CIMSType"] = hdnCIMSType.Value;
            dt.Rows[i]["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
            dt.Rows[i]["PrescriptionDetail"] = "";
            dt.Rows[i]["Instructions"] = txtInstructions.Text;
            dt.Rows[i]["Qty"] = common.myDec(hdnQty.Value);
            dt.Rows[i]["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
            dt.Rows[i]["ReferanceItemName"] = common.myStr(ddlReferanceItem.Text);
            dt.Rows[i]["DoseTypeId"] = ddlDoseType.SelectedValue;
            dt.Rows[i]["DoseTypeName"] = ddlDoseType.Text;

            if (common.myInt(ddlDoseType.SelectedValue) > 0)
            {
                ddlPeriodType.Enabled = false;
                txtDuration.Enabled = false;
                ddlFrequencyId.Enabled = false;
                ddlReferanceItem.Enabled = false;
            }



            dt.AcceptChanges();
            ViewState["DataTableDetail"] = dt;
        }

        dt = (DataTable)ViewState["DataTableDetail"];

        ViewState["DataTableDetail"] = dt;


        DR = dt.NewRow();
        DR["Id"] = common.myInt(hdnId.Value) + 1;
        DR["IndentTypeId"] = 0;
        DR["IndentType"] = "";
        DR["GenericId"] = 0;
        DR["ItemId"] = 0;
        DR["IndentId"] = 0;
        DR["GenericName"] = "";
        DR["ItemName"] = "";
        DR["FormulationId"] = 0;
        DR["RouteId"] = 0;
        DR["RouteName"] = "";
        DR["StrengthId"] = 0;
        DR["CIMSItemId"] = 0;
        DR["CIMSType"] = "";
        DR["VIDALItemId"] = 0;
        DR["Frequency"] = 0.00;
        DR["FrequencyId"] = 0;
        DR["FrequencyName"] = "";
        DR["Duration"] = "";
        DR["Type"] = "D";
        DR["DurationText"] = "";
        DR["Dose"] = "";
        DR["UnitId"] = 0;
        DR["UnitName"] = "";
        DR["FoodRelationshipID"] = 0;
        DR["FoodRelationship"] = "";
        DR["StartDate"] = Convert.ToDateTime(txtEndDate.SelectedDate.Value.AddDays(1)).ToString("dd/MM/yyyy");
        DR["EndDate"] = Convert.ToDateTime(txtEndDate.SelectedDate.Value.AddDays(1)).ToString("dd/MM/yyyy");
        DR["Qty"] = 0.00;
        DR["PrescriptionDetail"] = "";
        DR["ReferanceItemId"] = 0;
        DR["ReferanceItemName"] = "";
        DR["Instructions"] = "";
        DR["DoseTypeId"] = 0;
        DR["DoseTypeName"] = "";


        if (chkFavouriteOnly.Checked && common.myInt(ddlBrand.SelectedValue) > 0)
        {
            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            DataSet dsFavourite = objEMR.getEMRFavouriteDrugsAttributes(DoctorId, common.myInt(ddlBrand.SelectedValue));

            if (dsFavourite.Tables.Count > 0)
            {
                if (dsFavourite.Tables[0].Rows.Count > 0)
                {
                    DataRow DrFavourite = dsFavourite.Tables[0].Rows[0];

                    if (common.myInt(DrFavourite["FormulationId"]) > 0)
                    {
                        DR["FormulationId"] = common.myInt(DrFavourite["FormulationId"]);
                    }
                    if (common.myInt(DrFavourite["RouteId"]) > 0)
                    {
                        DR["RouteId"] = common.myInt(DrFavourite["RouteId"]);
                    }

                    if (common.myInt(DrFavourite["FrequencyId"]) > 0)
                    {
                        DR["FrequencyId"] = common.myInt(DrFavourite["FrequencyId"]);
                    }
                    if (common.myInt(DrFavourite["Duration"]) > 0)
                    {
                        DR["Duration"] = common.myInt(DrFavourite["Duration"]);
                    }
                    if (common.myLen(DrFavourite["DurationType"]) > 0)
                    {
                        DR["Type"] = common.myStr(DrFavourite["DurationType"]);
                    }
                    if (common.myDbl(DrFavourite["Dose"]) > 0)
                    {
                        double numberToSplit = 0;
                        double decimalresult = 0;

                        numberToSplit = common.myDbl(DrFavourite["Dose"]);
                        decimalresult = (int)numberToSplit - numberToSplit;
                        if (numberToSplit == 0.0)
                        {
                        }
                        else
                        {
                            if (decimalresult == 0)
                            {
                                DR["Dose"] = ((int)numberToSplit).ToString();
                            }
                            else
                            {
                                DR["Dose"] = numberToSplit.ToString("F2");
                            }
                        }

                    }
                    if (common.myInt(DrFavourite["UnitId"]) > 0)
                    {
                        DR["UnitId"] = common.myInt(DrFavourite["UnitId"]);
                    }

                    if (common.myInt(DrFavourite["FoodRelationshipId"]) > 0)
                    {
                        DR["FoodRelationshipID"] = common.myInt(DrFavourite["FoodRelationshipId"]);
                    }
                    DR["Instructions"] = common.myStr(DrFavourite["Instructions"]);
                }
            }
        }

        dt.Rows.Add(DR);
        dt.AcceptChanges();

        ViewState["DataTableDetail"] = dt;
        gvItemDetails.DataSource = dt;
        gvItemDetails.DataBind();
    }

    protected void gvItemDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                RadComboBox ddlFrequencyId = (RadComboBox)e.Row.FindControl("ddlFrequencyId");
                if (ViewState["FrequencyData"] != null)
                {
                    DataTable dt = (DataTable)ViewState["FrequencyData"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(dr["Description"]);
                        item.Value = common.myStr(common.myInt(dr["Id"]));
                        item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));
                        item.DataBind();
                        ddlFrequencyId.Items.Add(item);
                    }

                    ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", "1"));

                }

                RadComboBox ddlFoodRelation = (RadComboBox)e.Row.FindControl("ddlFoodRelation");
                if (ViewState["FoodRelation"] != null)
                {
                    DataTable dt = (DataTable)ViewState["FoodRelation"];
                    if (dt.Rows.Count > 0)
                    {
                        ddlFoodRelation.DataSource = dt;
                        ddlFoodRelation.DataValueField = "Id";
                        ddlFoodRelation.DataTextField = "FoodName";
                        ddlFoodRelation.DataBind();
                        ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                        ddlFoodRelation.Items[0].Value = "0";
                    }
                }
                RadComboBox ddlUnit = (RadComboBox)e.Row.FindControl("ddlUnit");
                if (ViewState["UnitMaster"] != null)
                {
                    DataTable dt = (DataTable)ViewState["UnitMaster"];
                    ddlUnit.DataSource = dt;
                    ddlUnit.DataValueField = "Id";
                    ddlUnit.DataTextField = "UnitName";
                    ddlUnit.DataBind();
                    ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                    ddlUnit.Items[0].Value = "0";
                    ddlUnit.SelectedValue = "0";
                }

                RadComboBox ddlDoseType = (RadComboBox)e.Row.FindControl("ddlDoseType");
                if (ViewState["DoseType"] != null)
                {
                    DataTable dt = (DataTable)ViewState["DoseType"];
                    ddlDoseType.DataSource = dt;
                    ddlDoseType.DataValueField = "StatusId";
                    ddlDoseType.DataTextField = "Status";
                    ddlDoseType.DataBind();
                    ddlDoseType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                    ddlDoseType.Items[0].Value = "0";
                }

                TextBox txtDuration = (TextBox)e.Row.FindControl("txtDuration");
                TextBox txtDose = (TextBox)e.Row.FindControl("txtDose");
                RadComboBox ddlPeriodType = (RadComboBox)e.Row.FindControl("ddlPeriodType");
                RadComboBox ddlUnitId = (RadComboBox)e.Row.FindControl("ddlUnit");
                RadComboBox ddlReferanceItem = (RadComboBox)e.Row.FindControl("ddlReferanceItem");
                TextBox txtInstructions = (TextBox)e.Row.FindControl("txtInstructions");

                RadDatePicker txtStartDate = (RadDatePicker)e.Row.FindControl("txtStartDate");
                RadDatePicker txtEndDate = (RadDatePicker)e.Row.FindControl("txtEndDate");


                LinkButton lnkAddNewRow = (LinkButton)e.Row.FindControl("lnkAddNewRow");
                DataTable dT = new DataTable();
                DataTable dtPrevious = new DataTable();

                if (ViewState["Item"] != null)
                {
                    dtPrevious = (DataTable)ViewState["Item"];
                    foreach (DataRow dr in dtPrevious.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(dr["ItemName"]);
                        item.Value = common.myStr(common.myInt(dr["ItemId"]));
                        item.Attributes.Add("IsInfusion", common.myStr(dr["IsInfusion"]));
                        item.DataBind();
                        ddlReferanceItem.Items.Add(item);
                    }
                }

                if (ViewState["ItemDetail"] != null && ViewState["Edit"] != null)
                {
                    dT = (DataTable)ViewState["ItemDetail"];
                    if (dT != null && count < dT.Rows.Count)
                    {
                        ddlFrequencyId.SelectedValue = common.myStr(dT.Rows[count]["FrequencyId"]);
                        ddlFoodRelation.SelectedValue = common.myStr(dT.Rows[count]["FoodRelationshipId"]);
                        ddlPeriodType.SelectedValue = common.myStr(dT.Rows[count]["Type"]);
                        ddlUnitId.SelectedValue = common.myStr(dT.Rows[count]["UnitId"]);
                        ddlReferanceItem.SelectedValue = common.myStr(dT.Rows[count]["ReferanceItemId"]);
                        ddlFoodRelation.SelectedValue = common.myStr(dT.Rows[count]["FoodRelationshipID"]);
                        txtInstructions.Text = common.myStr(dT.Rows[count]["Instructions"]);
                        ddlDoseType.SelectedValue = common.myStr(dT.Rows[count]["DoseTypeId"]);
                    }

                }
                else if (ViewState["DataTableDetail"] != null)
                {
                    dT = (DataTable)ViewState["DataTableDetail"];
                    if (dT != null && count < dT.Rows.Count)
                    {
                        ddlFrequencyId.SelectedValue = common.myStr(dT.Rows[count]["FrequencyId"]);
                        ddlFoodRelation.SelectedValue = common.myStr(dT.Rows[count]["FoodRelationshipId"]);
                        ddlPeriodType.SelectedValue = common.myStr(dT.Rows[count]["Type"]);
                        ddlUnitId.SelectedValue = common.myStr(dT.Rows[count]["UnitId"]);
                        ddlReferanceItem.SelectedValue = common.myInt(dT.Rows[count]["ReferanceItemId"]).ToString();
                        ddlFoodRelation.SelectedValue = common.myStr(dT.Rows[count]["FoodRelationshipID"]);
                        txtInstructions.Text = common.myStr(dT.Rows[count]["Instructions"]);
                        ddlDoseType.SelectedValue = common.myStr(dT.Rows[count]["DoseTypeId"]);
                    }
                }
                count++;
                //if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO")
                //{
                //    ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                //    ddlFrequencyId.Items[0].Value = "0";
                //    ddlFrequencyId.SelectedValue = "0";
                //    ddlFrequencyId.Enabled = false;

                //    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                //    ddlRoute.Items[0].Value = "0";


                //    ddlPeriodType.Enabled = false;
                //    txtDose.Text = "0";
                //    txtDose.Enabled = false;
                //    txtDuration.Text = "0";
                //    txtDuration.Enabled = false;
                //    txtStartDate.Enabled = false;
                //    txtEndDate.Enabled = false;
                //    ddlDoseType.Enabled = false;
                //    ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select Unit"));
                //    ddlUnit.Items[0].Value = "0";
                //    ddlUnit.Enabled = false;
                //    ddlReferanceItem.Enabled = false;
                //    ddlFoodRelation.Enabled = false;
                //    lnkAddNewRow.Enabled = false;

                //}
                //else if (ddlDoseType.SelectedValue != "0")
                //{
                //    ddlPeriodType.Enabled = false;
                //    txtDose.Enabled = true;
                //    ddlReferanceItem.Enabled = false;

                //    ddlFrequencyId.Enabled = false;
                //    ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
                //    ddlFrequencyId.Items[0].Value = "0";
                //    ddlFrequencyId.SelectedValue = "0";

                //    txtDuration.Enabled = false;
                //    txtStartDate.Enabled = true;
                //    ddlDoseType.Enabled = true;
                //    ddlUnit.Enabled = true;
                //    ddlFoodRelation.Enabled = true;
                //    lnkAddNewRow.Enabled = true;
                //}
                //else
                //{
                //    ddlReferanceItem.Enabled = true;
                //    ddlPeriodType.Enabled = true;
                //    txtDose.Enabled = true;
                //    txtDuration.Enabled = true;
                //    txtStartDate.Enabled = true;
                //    ddlDoseType.Enabled = true;
                //    ddlUnit.Enabled = true;

                //    ddlFoodRelation.Enabled = true;
                //    lnkAddNewRow.Enabled = true;

                //}
                dT.Dispose();
                dtPrevious.Dispose();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void BindPatientProvisionalDiagnosis()
    {
        try
        {
            DataSet ds;
            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            ds = new DataSet();
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //Change Palendra
    protected void GetPatientEncounterStatus()
    {
        try
        {
            ViewState["MarkForDischarge"] = "";
            Hashtable hshOutput = new Hashtable();
            BaseC.EMRBilling.clsOrderNBill objDiag = new BaseC.EMRBilling.clsOrderNBill(sConString);
            string StatusCode = "Encounter";
            hshOutput = objDiag.GetPatientEncounterStatus(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), StatusCode);
            if (common.myStr(hshOutput["@vpStatus"]).ToUpper().Equals("MARKED FOR DISCHARGED"))
            {
                ViewState["MarkForDischarge"] = 1;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //Change Palendra
    protected void gvItemDetails_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);

                if (ItemId == 0 && GenericId == 0)
                {
                    return;
                }

                string CIMSItemId = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                string CIMSType = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);

                string VIDALItemId = common.myStr(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

                if (ItemId > 0)
                {
                    ddlFormulation.Enabled = false;
                    //ddlRoute.Enabled = false;
                    ddlStrength.Enabled = false;
                }
                else
                {
                    ddlFormulation.Enabled = true;
                    ddlRoute.Enabled = true;
                    ddlStrength.Enabled = true;
                }

                hdnGenericId.Value = common.myStr(GenericId);
                hdnItemId.Value = common.myStr(ItemId);

                hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                hdnCIMSType.Value = common.myStr(CIMSType);

                hdnVIDALItemId.Value = common.myStr(VIDALItemId);

                Label GenericName = (Label)row.FindControl("lblGenericName");
                Label ItemName = (Label)row.FindControl("lblItemName");
                Label TotalQty = (Label)row.FindControl("lblTotalQty");

                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
                HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                HiddenField hdnDays = (HiddenField)row.FindControl("hdnDays");
                HiddenField hdnStartDate = (HiddenField)row.FindControl("hdnStartDate");
                HiddenField hdnEndDate = (HiddenField)row.FindControl("hdnEndDate");
                HiddenField hdnRemarks = (HiddenField)row.FindControl("hdnRemarks");

                double days = common.myDbl(hdnDays.Value);
                double period = days;
                string periodType = "D";

                if (days % 356 == 0)
                {
                    period = days / 365;
                    periodType = "Y";
                }
                else if (days % 30 == 0)
                {
                    period = days / 30;
                    periodType = "M";
                }
                else if (days % 7 == 0)
                {
                    period = days / 7;
                    periodType = "W";
                }
                else
                {
                    period = days;
                    periodType = "D";
                }

                //ddlPeriodType.SelectedValue = "D";

                hdnGenericName.Value = common.myStr(GenericName.Text);
                hdnItemName.Value = common.myStr(ItemName.Text);
                lblGenericName.Text = common.myStr(GenericName.Text);
                //  txtTotQty.Text = common.myDbl(TotalQty.Text).ToString("F2");

                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                // txtDose.Text = common.myStr(hdnDose.Value);
                //ddlFrequency.SelectedIndex = ddlFrequency.Items.IndexOf(ddlFrequency.Items.FindItemByValue(common.myStr(hdnFrequencyId.Value)));
                // txtDays.Text = common.myStr(period);
                // ddlPeriodType.SelectedValue = periodType;
                //txtStartDate.SelectedDate = common.myDate(hdnStartDate.Value);
                //txtEndDate.SelectedDate = common.myDate(hdnEndDate.Value);
                //txtRemarks.Text = common.myStr(hdnRemarks.Value);
            }
            else if (e.CommandName == "ItemDelete")
            {
                int Id = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                if (ViewState["DataTableDetail"] != null && (Id > 0))
                {
                    DataTable tbl = (DataTable)ViewState["DataTableDetail"];

                    DataView DV = tbl.Copy().DefaultView;
                    if (Id > 0)
                    {
                        DV.RowFilter = "ISNULL(Id, 0) <> " + Id;
                    }
                    tbl = DV.ToTable();

                    ViewState["DataTableDetail"] = tbl;

                    if (tbl.Rows.Count == 0)
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();
                    }

                    gvItemDetails.DataSource = tbl;
                    gvItemDetails.DataBind();

                    setVisiblilityInteraction();
                }
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
                {
                    Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showIntreraction();
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "MonographVIDAL")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName == "InteractionVIDAL")
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName == "DHInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction("H");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvPrevious_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label TotalQty = (Label)e.Row.FindControl("lblTotalQty");

                TotalQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                //gvPrevious.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                //gvPrevious.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;
                //gvPrevious.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;

                //gvPrevious.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;

                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0)
                    {
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");

                        string strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        if (strXML != "")
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);

                            if (!outputValues.ToUpper().Contains("<MONOGRAPH>"))
                            {
                                lnkBtnMonographCIMS.Visible = false;
                            }
                        }
                    }
                }
                else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                    gvPrevious.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;

                    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");

                    //lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                    if (common.myStr(hdnVIDALItemId.Value).Trim().Length == 0)
                    {
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                    }
                }

                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

                if (ViewState["GridDataDetail"] != null)
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                    DataTable dt = (DataTable)ViewState["GridDataDetail"];
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value) + " AND ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv.ToTable());
                    }
                    else
                    {
                        DataView dv1 = new DataView(dt);
                        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv1.ToTable());
                        }
                    }
                    dt.Dispose();
                    dv.Dispose();
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

    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);

                if (ItemId == 0 && GenericId == 0)
                {
                    return;
                }

                string CIMSItemId = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                string CIMSType = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);

                string VIDALItemId = common.myStr(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

                if (ItemId > 0)
                {
                    ddlFormulation.Enabled = false;
                    //ddlRoute.Enabled = false;
                    ddlStrength.Enabled = false;
                }
                else
                {
                    ddlFormulation.Enabled = true;
                    ddlRoute.Enabled = true;
                    ddlStrength.Enabled = true;
                }

                hdnGenericId.Value = common.myStr(GenericId);
                hdnItemId.Value = common.myStr(ItemId);

                hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                hdnCIMSType.Value = common.myStr(CIMSType);

                hdnVIDALItemId.Value = common.myStr(VIDALItemId);

                Label GenericName = (Label)row.FindControl("lblGenericName");
                Label ItemName = (Label)row.FindControl("lblItemName");
                Label TotalQty = (Label)row.FindControl("lblTotalQty");

                HiddenField hdnFormulationId = (HiddenField)row.FindControl("hdnFormulationId");
                HiddenField hdnRouteId = (HiddenField)row.FindControl("hdnRouteId");
                HiddenField hdnStrengthId = (HiddenField)row.FindControl("hdnStrengthId");
                HiddenField hdnFrequencyId = (HiddenField)row.FindControl("hdnFrequencyId");
                HiddenField hdnDose = (HiddenField)row.FindControl("hdnDose");
                HiddenField hdnDays = (HiddenField)row.FindControl("hdnDays");
                HiddenField hdnStartDate = (HiddenField)row.FindControl("hdnStartDate");
                HiddenField hdnEndDate = (HiddenField)row.FindControl("hdnEndDate");
                HiddenField hdnRemarks = (HiddenField)row.FindControl("hdnRemarks");

                double days = common.myDbl(hdnDays.Value);
                double period = days;
                string periodType = "D";

                if (days % 356 == 0)
                {
                    period = days / 365;
                    periodType = "Y";
                }
                else if (days % 30 == 0)
                {
                    period = days / 30;
                    periodType = "M";
                }
                else if (days % 7 == 0)
                {
                    period = days / 7;
                    periodType = "W";
                }
                else
                {
                    period = days;
                    periodType = "D";
                }

                //ddlPeriodType.SelectedValue = "D";

                hdnGenericName.Value = common.myStr(GenericName.Text);
                hdnItemName.Value = common.myStr(ItemName.Text);
                lblGenericName.Text = common.myStr(GenericName.Text);
                //txtTotQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                // txtDose.Text = common.myStr(hdnDose.Value);
                //ddlFrequency.SelectedIndex = ddlFrequency.Items.IndexOf(ddlFrequency.Items.FindItemByValue(common.myStr(hdnFrequencyId.Value)));
                // txtDays.Text = common.myStr(period);
                // ddlPeriodType.SelectedValue = periodType;
                //txtStartDate.SelectedDate = common.myDate(hdnStartDate.Value);
                //txtEndDate.SelectedDate = common.myDate(hdnEndDate.Value);
                //txtRemarks.Text = common.myStr(hdnRemarks.Value);
            }
            else if (e.CommandName == "ItemStop" || e.CommandName == "ItemCancel")
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");

                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                if ((hdnIndentId.Value == "" || hdnIndentId.Value == "0") && ItemId == 0)
                {
                    return;
                }
                TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                Hashtable hshOutput = new Hashtable();



                ViewState["StopItemId"] = common.myInt(ItemId).ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                if (e.CommandName == "ItemStop")//Stop
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;

                    lblCancelStopMedicationRemarks.Text = "Stop Medication Remarks";
                    btnStopMedication.Text = "Stop";

                    //if (txtRemarks.Text == "")
                    //{
                    //    Alert.ShowAjaxMsg("Please enter remarks", Page);
                    //    return;
                    //}
                    //hshOutput = emr.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    //   common.myInt(hdnIndentId.Value), ItemId, common.myInt(Session["UserId"]), common.myInt(Session["RegistrationId"]),
                    //   common.myInt(Session["EncounterId"]), 0, txtRemarks.Text, common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                    ViewState["IndentDetailsId"] = "0";
                }
                else//Cancel
                {

                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;

                    lblCancelStopMedicationRemarks.Text = "Cancel Medication Remarks";
                    btnStopMedication.Text = "Cancel";

                    //hshOutput = emr.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    //  common.myInt(hdnIndentId.Value), ItemId, common.myInt(Session["UserId"]), common.myInt(Session["RegistrationId"]),
                    //  common.myInt(Session["EncounterId"]), 1, txtRemarks.Text, common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                    ViewState["IndentDetailsId"] = "0";
                }

                //lblMessage.Text = hshOutput["@chvOutPut"].ToString();
                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //BindGrid("", "");
                //setVisiblilityInteraction();
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
                {
                    Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showIntreraction();
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = "";
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "MonographVIDAL")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    getMonographVidal((int?)common.myInt(e.CommandArgument));
                }
            }
            else if (e.CommandName == "InteractionVIDAL")
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugToDrugInteractionVidal(commonNameGroupIds);
                }
            }
            else if (e.CommandName == "DHInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction("H");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnRefresh_OnClick(object sender, System.EventArgs e)
    {
        BindGrid(hdnReturnIndentIds.Value, hdnReturnItemIds.Value);
    }
    protected void btnGetFavourite_OnClick(object sender, System.EventArgs e)
    {
        ddlBrand.Items.Clear();
        ddlBrand.Text = "";
        ddlBrand.SelectedValue = "0";

        int DoctorId = 0;
        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            DoctorId = common.myInt(Request.QueryString["DoctorId"]);
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

            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), common.myInt(hdnItemId.Value),
                                0, DoctorId, "", "");

            if (dsSearch.Tables.Count > 0)
            {
                for (int i = 0; i < dsSearch.Tables[0].Rows.Count; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dsSearch.Tables[0].Rows[i]["ItemName"];
                    item.Value = common.myStr(dsSearch.Tables[0].Rows[i]["ItemId"]);
                    item.Attributes.Add("ClosingBalance", common.myStr(dsSearch.Tables[0].Rows[i]["ClosingBalance"]));
                    item.Attributes.Add("CIMSItemId", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSItemId"]));
                    item.Attributes.Add("CIMSType", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSType"]));
                    item.Attributes.Add("VIDALItemId", common.myStr(dsSearch.Tables[0].Rows[i]["VIDALItemId"]));
                    item.Attributes.Add("ItemFlagName", common.myStr(dsSearch.Tables[0].Rows[i]["ItemFlagName"]));
                    item.Attributes.Add("ItemFlagCode", common.myStr(dsSearch.Tables[0].Rows[i]["ItemFlagCode"]));

                    this.ddlBrand.Items.Add(item);
                    item.DataBind();
                }
            }

            ddlBrand.SelectedValue = hdnItemId.Value;

            if (common.myInt(hdnItemId.Value) > 0)
            {
                DataSet dsFavourite = objEMR.getEMRFavouriteDrugsAttributes(DoctorId, common.myInt(hdnItemId.Value));

                if (dsFavourite.Tables.Count > 0)
                {
                    if (dsFavourite.Tables[0].Rows.Count > 0)
                    {
                        chkFavouriteOnly.Checked = true;

                        if (common.myInt(dsFavourite.Tables[0].Rows[0]["FormulationId"]) > 0)
                        {
                            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(dsFavourite.Tables[0].Rows[0]["FormulationId"]).ToString()));
                        }
                        if (common.myInt(dsFavourite.Tables[0].Rows[0]["RouteId"]) > 0)
                        {
                            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(dsFavourite.Tables[0].Rows[0]["RouteId"]).ToString()));
                        }

                        foreach (GridViewRow dataItem in gvItemDetails.Rows)
                        {
                            RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
                            TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
                            RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
                            TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
                            RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
                            RadComboBox ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
                            TextBox txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

                            DataRow DrFavourite = dsFavourite.Tables[0].Rows[0];

                            if (common.myInt(DrFavourite["FrequencyId"]) > 0)
                            {
                                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DrFavourite["FrequencyId"]).ToString()));
                            }
                            if (common.myInt(DrFavourite["Duration"]) > 0)
                            {
                                txtDuration.Text = common.myInt(DrFavourite["Duration"]).ToString();
                            }
                            if (common.myLen(DrFavourite["DurationType"]) > 0)
                            {
                                ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DrFavourite["DurationType"])));
                            }
                            if (common.myDbl(DrFavourite["Dose"]) > 0)
                            {
                                double numberToSplit = 0;
                                double decimalresult = 0;

                                numberToSplit = common.myDbl(DrFavourite["Dose"]);
                                decimalresult = (int)numberToSplit - numberToSplit;
                                if (numberToSplit == 0.0)
                                {
                                }
                                else
                                {
                                    if (decimalresult == 0)
                                    {
                                        txtDose.Text = ((int)numberToSplit).ToString();
                                    }
                                    else
                                    {
                                        txtDose.Text = numberToSplit.ToString("F2");
                                    }
                                }

                            }
                            if (common.myInt(DrFavourite["UnitId"]) > 0)
                            {
                                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DrFavourite["UnitId"]).ToString()));
                            }

                            if (common.myInt(DrFavourite["FoodRelationshipId"]) > 0)
                            {
                                ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DrFavourite["FoodRelationshipId"]).ToString()));
                            }

                            txtInstructions.Text = common.myStr(DrFavourite["Instructions"]);

                            break;
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
    }
    //protected void btnPreviousMedications_Click(Object sender, EventArgs e)
    //{
    //    ViewState["Mode"] = "A";
    //    bool IsConsumable = Convert.ToBoolean(ViewState["Consumable"] != null ? ViewState["Consumable"] : "False");
    //    RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMain.aspx?Consumable=" + IsConsumable;
    //    RadWindow1.Height = 570;
    //    RadWindow1.Width = 850;
    //    RadWindow1.Top = 10;
    //    RadWindow1.Left = 10;
    //    RadWindow1.OnClientClose = "OnClientClose";
    //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //    RadWindow1.Modal = true;
    //    RadWindow1.VisibleStatusbar = false;
    //    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    //}
    protected void btnPreviousMedications_Click(Object sender, EventArgs e)
    {
        try
        {
            Session["RunningCIMSXMLInputData"] = getRunningInterationXML();
            ViewState["Mode"] = "A";
            if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
            {
                RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=" + common.myBool(ViewState["Consumable"]);
            }
            else
            {
                RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=";
            }
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
    private void ShowPatientDetails()
    {
        try
        {
            objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Weight.Text = "";
                txtHeight.Text = "";
                lbl_BSA.Text = "";
                for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("WT"))// Weight
                    {
                        lbl_Weight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                    }
                    else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("HT"))// Height
                    {
                        txtHeight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                    }
                    else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("BSA"))
                    {
                        lbl_BSA.Text = common.myStr(ds.Tables[0].Rows[i][1]);
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
    protected void BindGrid(string IdentId, string ItemId)
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            ViewState["Mode"] = IdentId == "" && ItemId == "" ? "P" : ViewState["Mode"];

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(Session["EncounterId"]), 0, 0, common.myStr(ViewState["Mode"]).Equals(string.Empty) ? "P" : common.myStr(ViewState["Mode"]), "", "", "");
            }
            else
            {
                ds = objEMR.getOPMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, common.myStr(ViewState["Mode"]).Equals(string.Empty) ? "P" : common.myStr(ViewState["Mode"]));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                DataView dv1 = new DataView(ds.Tables[1]);
                DataView dvConsumable = new DataView();
                if (IdentId != "" && ItemId != "")
                {
                    dv.RowFilter = "ItemId IN (" + ItemId + ") AND IndentId IN (" + IdentId + ")";
                    dv1.RowFilter = "ItemId IN (" + ItemId + ") AND IndentId IN (" + IdentId + ")";
                    if (ViewState["Item"] == null && ViewState["ItemDetail"] == null)
                    {
                        ViewState["Item"] = dv.ToTable();
                        ViewState["ItemDetail"] = dv1.ToTable();
                    }
                    else
                    {
                        BindItemWithItemDetail(dv.ToTable(), dv1.ToTable());

                    }
                    ViewState["Stop"] = true;

                    // Akshay_08072022
                    DataTable dt = new DataTable();
                    dt = dv.ToTable();
                    Hashtable hTable = new Hashtable();
                    ArrayList duplicateList = new ArrayList();
                    foreach (DataRow drow in dt.Rows)
                    {
                        if (hTable.Contains(drow["ItemId"]))
                            duplicateList.Add(drow);
                        else
                            hTable.Add(drow["ItemId"], string.Empty);
                    }
                    foreach (DataRow dRow in duplicateList)
                        dt.Rows.Remove(dRow);


                    gvItem.DataSource = dt;
                    //gvItem.DataSource = ViewState["Item"];
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
                            dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";

                        }
                        else
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED')";
                        }
                        gvPrevious.DataSource = dvConsumable.ToTable();
                        gvPrevious.DataBind();
                    }
                }
                ds.Dispose();
                dv.Dispose();
                dv1.Dispose();
                dvConsumable.Dispose();
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
        //dt.Columns.Add("Id", typeof(int));
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
        dt.Columns.Add("CIMSItemId", typeof(int));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("PrescriptionDetail", typeof(string));
        dt.Columns.Add("ReferanceItemId", typeof(int));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        dt.Columns.Add("CustomMedication", typeof(bool));
        dt.Columns.Add("NotToPharmacy", typeof(bool));
        dt.Columns.Add("XMLData", typeof(String));
        dt.Columns.Add("IsInfusion", typeof(bool));
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("PackSize", typeof(int));
        dt.Columns.Add("ConversionFactor2", typeof(int));
        dt.Columns.Add("ItemFlagName", typeof(string));
        dt.Columns.Add("ItemFlagCode", typeof(string));
        //dt.Columns.Add("RestrictedItemForPanel", typeof(bool));
        return dt;

    }
    private DataTable CreateItemDetailTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("IndentTypeId", typeof(int));
        dt.Columns.Add("IndentType", typeof(string));
        dt.Columns.Add("GenericId", typeof(int));
        dt.Columns.Add("GenericName", typeof(string));
        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("IndentId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        dt.Columns.Add("FormulationId", typeof(int));
        dt.Columns.Add("RouteId", typeof(int));
        dt.Columns.Add("RouteName", typeof(string));
        dt.Columns.Add("StrengthId", typeof(int));
        dt.Columns.Add("CIMSItemId", typeof(int));
        dt.Columns.Add("CIMSType", typeof(string));
        dt.Columns.Add("VIDALItemId", typeof(int));
        dt.Columns.Add("Frequency", typeof(decimal));
        dt.Columns.Add("FrequencyName", typeof(string));
        dt.Columns.Add("FrequencyId", typeof(int));
        dt.Columns.Add("Duration", typeof(string));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("DurationText", typeof(string));
        dt.Columns.Add("Dose", typeof(string));
        dt.Columns.Add("UnitId", typeof(int));
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("FoodRelationshipId", typeof(int));
        dt.Columns.Add("FoodRelationship", typeof(string));
        dt.Columns.Add("StartDate", typeof(string));
        dt.Columns.Add("EndDate", typeof(string));
        dt.Columns.Add("Qty", typeof(decimal));
        dt.Columns.Add("PrescriptionDetail", typeof(string));
        dt.Columns.Add("ReferanceItemId", typeof(int));
        dt.Columns.Add("ReferanceItemName", typeof(string));
        dt.Columns.Add("Instructions", typeof(string));
        dt.Columns.Add("DoseTypeId", typeof(int));
        dt.Columns.Add("DoseTypeName", typeof(string));
        dt.Columns.Add("CustomMedication", typeof(bool));
        dt.Columns.Add("NotToPharmacy", typeof(bool));
        dt.Columns.Add("XMLData", typeof(String));
        dt.Columns.Add("IsInfusion", typeof(bool));
        dt.Columns.Add("ItemFlagName", typeof(string));
        dt.Columns.Add("ItemFlagCode", typeof(string));
        return dt;

    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        dr["IndentTypeId"] = 0;
        dr["IndentType"] = "";
        dr["IndentNo"] = 0;
        dr["IndentDate"] = "";
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = "";
        dr["ItemName"] = "";
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["StrengthId"] = 0;
        dr["CIMSItemId"] = 0;
        dr["CIMSType"] = "";
        dr["VIDALItemId"] = 0;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = "";
        dr["ReferanceItemId"] = 0;

        dr["StartDate"] = "";
        dr["EndDate"] = "";
        dr["CustomMedication"] = 0;
        dr["XMLData"] = "";
        dr["NotToPharmacy"] = 0;
        dr["IsInfusion"] = false;
        dr["UnitName"] = string.Empty;
        dr["PackSize"] = 0;
        dr["ConversionFactor2"] = 0;
        //dr["RestrictedItemForPanel"] = false; ;
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
        dr["IndentType"] = "";
        dr["IndentNo"] = 0;
        dr["IndentDate"] = "";
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = "";
        dr["ItemName"] = "";
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["StrengthId"] = 0;
        dr["CIMSItemId"] = 0;
        dr["CIMSType"] = "";
        dr["VIDALItemId"] = 0;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = "";
        dr["ReferanceItemId"] = 0;

        dr["StartDate"] = "";
        dr["EndDate"] = "";
        dr["CustomMedication"] = 0;
        dr["NotToPharmacy"] = 0;
        dr["XMLData"] = "";

        dt.Rows.Add(dr);
        dt.AcceptChanges();
        ViewState["ItemDetail"] = null;
        gvPrevious.DataSource = dt;
        gvPrevious.DataBind();
        ViewState["DataTableItem"] = dt;
    }
    private void BindBlankItemDetailGrid()
    {
        DataTable dt = CreateItemDetailTable();
        DataRow dr = dt.NewRow();
        dr["Id"] = 1;
        dr["IndentTypeId"] = 0;
        dr["IndentType"] = "";
        dr["GenericId"] = 0;
        dr["ItemId"] = 0;
        dr["IndentId"] = 0;
        dr["GenericName"] = "";
        dr["ItemName"] = "";
        dr["FormulationId"] = 0;
        dr["RouteId"] = 0;
        dr["RouteName"] = "";
        dr["StrengthId"] = 0;
        dr["CIMSItemId"] = 0;
        dr["CIMSType"] = "";
        dr["VIDALItemId"] = 0;
        dr["Frequency"] = 0.00;
        dr["FrequencyId"] = DBNull.Value;
        dr["FrequencyName"] = "";
        dr["Duration"] = "";
        dr["Type"] = "D";
        dr["DurationText"] = "";
        dr["Dose"] = "";
        dr["UnitId"] = 0;
        dr["UnitName"] = "";
        dr["FoodRelationshipId"] = DBNull.Value;
        dr["FoodRelationship"] = "";
        if (common.myStr(Session["OPIP"]).Equals("I"))
        {
            dr["StartDate"] = System.DateTime.Now;
        }
        else
        {
            dr["StartDate"] = "";
        }
        dr["EndDate"] = System.DateTime.Now;
        dr["Qty"] = 0.00;
        dr["PrescriptionDetail"] = "";
        dr["ReferanceItemId"] = DBNull.Value;
        dr["ReferanceItemName"] = "";
        dr["Instructions"] = "";
        dr["DoseTypeId"] = 0;
        dr["DoseTypeName"] = "";
        dr["XMLData"] = "";
        dr["CustomMedication"] = 0;
        dr["IsInfusion"] = false;
        dt.Rows.Add(dr);
        gvItemDetails.DataSource = dt;
        gvItemDetails.DataBind();
        ViewState["DataTableDetail"] = dt;
    }
    private void clearItemDetails()
    {
        Session["DrugOrderDuplicateCheck"] = "0";
        lblGenericName.Text = "";
        //txtTotQty.Text = "0.00";
        ddlFormulation.SelectedIndex = 0;
        ddlRoute.SelectedIndex = 0;
        ddlStrength.SelectedIndex = 0;
        //txtDose.Text = "";
        // ddlFrequency.SelectedIndex = 0;
        // txtDays.Text = "";
        // ddlPeriodType.SelectedIndex = 0;
        //txtStartDate.SelectedDate = DateTime.Now;
        //txtEndDate.SelectedDate = DateTime.Now;
        //txtRemarks.Text = "";
    }

    private bool isSavedItem()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ddlGeneric.SelectedValue) == 0 && common.myInt(ddlBrand.SelectedValue) == 0 && chkCustomMedication.Checked == false)
        {
            strmsg += "Generic Or Item not selected !";
            isSave = false;
        }

        //if (common.myDbl(txtTotQty.Text) == 0)
        //{
        //    strmsg += "Total quantity can not be zero or blank !";
        //    isSave = false;
        //}

        lblMessage.Text = strmsg;
        return isSave;
    }



    protected void ddlDoseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        RadComboBox DoseType = (RadComboBox)sender;
        GridViewRow item = (GridViewRow)DoseType.NamingContainer;
        RadComboBox ddlFrequencyId = (RadComboBox)item.FindControl("ddlFrequencyId");
        RadComboBox ddlReferanceItem = (RadComboBox)item.FindControl("ddlReferanceItem");
        TextBox txtDuration = (TextBox)item.FindControl("txtDuration");
        RadComboBox ddlPeriodType = (RadComboBox)item.FindControl("ddlPeriodType");
        if (common.myInt(DoseType.SelectedValue) > 0)
        {
            ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(""));
            ddlFrequencyId.Items[0].Value = "0";
            ddlFrequencyId.SelectedValue = "0";
            ddlFrequencyId.Enabled = false;
            ddlReferanceItem.Enabled = false;
            txtDuration.Enabled = false;
            ddlPeriodType.Enabled = false;
        }
        else
        {
            ddlFrequencyId.Items.Remove(0);
            ddlFrequencyId.SelectedValue = "0";
            ddlFrequencyId.Enabled = true;
            ddlReferanceItem.Enabled = true;
            txtDuration.Enabled = true;
            ddlPeriodType.Enabled = true;
        }
    }


    private void addItemFromKit(DataTable objKitdt)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtNew = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            DataRow DR;
            DataRow DR1;
            decimal dQty = 0;
            int countRow = 0;
            if (ViewState["Item"] == null && ViewState["Edit"] == null)
            {
                dt = CreateItemTable();
                dt1 = CreateItemDetailTable();
            }
            else
            {
                dtold = (DataTable)ViewState["Item"];
                if (common.myBool(ViewState["Edit"]) && ViewState["Item"] != null)
                {
                    DataView dv = new DataView(dtold);
                    dv.RowFilter = "ItemId<>" + common.myInt(ViewState["ItemId"]).ToString();
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
                    dt1 = CreateItemDetailTable();
                }
                else
                {
                    dt1old = (DataTable)ViewState["ItemDetail"];
                    if (common.myBool(ViewState["Edit"]) && ViewState["ItemDetail"] != null)
                    {
                        DataView dv1 = new DataView(dt1old);
                        dv1.RowFilter = "ItemId<>" + common.myInt(ViewState["ItemId"]).ToString();
                        dt1 = dv1.ToTable();
                    }
                    else
                    {
                        dt1 = (DataTable)ViewState["ItemDetail"];
                    }
                }
            }
            foreach (DataRow dr in objKitdt.Rows)
            {
                DataRow[] FindRow = dt.Select("itemid=" + common.myInt(dr["itemid"]) + "");

                if (FindRow.Length > 0)
                {
                    //return;
                    continue;
                }
                else
                {
                    //foreach (GridViewRow row in gvItem.Rows)
                    //{
                    //    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                    //    if (hdnItemId.Value == common.myStr(dr["itemid"]))
                    //    {
                    //        Alert.ShowAjaxMsg("Item already added", Page);
                    //        return;
                    //        //break; 
                    //    }
                    //    else
                    //    {
                    DR = dt.NewRow();
                    DR["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
                    DR["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;
                    DR["IndentId"] = 0;
                    DR["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
                    DR["ItemId"] = common.myInt(dr["itemid"]);
                    DR["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
                    DR["ItemName"] = common.myStr(dr["itemname"]);
                    DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                    DR["CustomMedication"] = chkCustomMedication.Checked;
                    DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
                    DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;
                    DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
                    ///Item Detail
                    TextBox txtInstructions = new TextBox();
                    RadComboBox ddlFoodRelation = new RadComboBox();
                    //---------------------
                    foreach (GridViewRow dataItem in gvItemDetails.Rows)
                    {
                        DR1 = dt1.NewRow();
                        string sFrequency = "0";
                        RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
                        TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
                        if (ddlFrequencyId.SelectedValue != "0")
                        {
                            sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                        }

                        TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");

                        RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");

                        RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");

                        RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

                        if (Request.QueryString["DRUGORDERCODE"] == null)
                        {
                            if (ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0")
                            {
                                if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == "")
                                {
                                    Alert.ShowAjaxMsg("Please select Frequency", Page);
                                    return;
                                }

                                if (ddlReferanceItem.SelectedValue == "0" || ddlReferanceItem.SelectedValue == "")
                                {
                                    if (common.myDbl(txtDose.Text) == 0.0)
                                    {
                                        txtDose.Text = "1";
                                        //Alert.ShowAjaxMsg("Please type Dose", Page);
                                        //return;
                                    }
                                    if (txtDuration.Text == "" || txtDuration.Text == "0"
                                        || txtDuration.Text == "0." || txtDuration.Text == ".0"
                                        || txtDuration.Text == ".")
                                    {
                                        Alert.ShowAjaxMsg("Please type Duration", Page);
                                        return;
                                    }
                                }
                            }
                        }

                        //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");

                        string Type = "";
                        decimal dDuration = 0;

                        switch (common.myStr(ddlPeriodType.SelectedValue))
                        {
                            case "N":
                                Type = txtDuration.Text + " Minute(s)";
                                dDuration = 1;
                                break;
                            case "H":
                                Type = txtDuration.Text + " Hour(s)";
                                dDuration = 1;
                                break;
                            case "D":
                                Type = txtDuration.Text + " Day(s)";
                                dDuration = 1;
                                break;
                            case "W":
                                Type = txtDuration.Text + " Week(s)";
                                dDuration = 7;
                                break;
                            case "M":
                                Type = txtDuration.Text + " Month(s)";
                                dDuration = 30;
                                break;
                            case "Y":
                                Type = txtDuration.Text + " Year(s)";
                                dDuration = 365;
                                break;
                        }
                        RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
                        HiddenField StartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                        HiddenField EndDate = (HiddenField)dataItem.FindControl("hdnEndDate");
                        HiddenField Remarks = (HiddenField)dataItem.FindControl("hdnRemarks");
                        ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
                        RadDatePicker txtStartDate = (RadDatePicker)dataItem.FindControl("txtStartDate");
                        RadDatePicker txtEndDate = (RadDatePicker)dataItem.FindControl("txtEndDate");
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        HiddenField hdnVIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                        HiddenField hdnQty = (HiddenField)dataItem.FindControl("hdnQty");

                        txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        //if (Request.QueryString["DRUGORDERCODE"] == null)
                        //{
                        if (common.myBool(ViewState["ISCalculationRequired"]))
                        {
                            dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                        }
                        else
                        {
                            dQty = 1;
                        }
                        DR1["RouteId"] = ddlRoute.SelectedValue;
                        DR1["RouteName"] = ddlRoute.SelectedItem.Text;
                        //}
                        //else
                        //{
                        //    dQty = 0;
                        //}

                        DR1["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
                        DR1["ItemId"] = common.myInt(dr["itemid"]);
                        DR1["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
                        DR1["ItemName"] = common.myStr(dr["itemname"]);


                        DR1["Dose"] = txtDose.Text;
                        DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                        DR1["Duration"] = txtDuration.Text;
                        DR1["DurationText"] = Type;
                        DR1["Type"] = ddlPeriodType.SelectedValue;

                        DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                        DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

                        DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
                        DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? "" : common.myStr(ddlFoodRelation.Text);
                        DR1["UnitId"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
                        DR1["UnitName"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? "" : ddlUnit.Text;
                        DR1["CIMSItemId"] = common.myInt(hdnCIMSItemId.Value);
                        DR1["CIMSType"] = hdnCIMSType.Value;
                        DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                        DR1["PrescriptionDetail"] = "";
                        DR1["Instructions"] = txtInstructions.Text;
                        DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
                        DR1["ReferanceItemName"] = common.myStr(ddlReferanceItem.Text);
                        DR1["DoseTypeId"] = ddlDoseType.SelectedValue;
                        DR1["DoseTypeName"] = ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0" ? "" : ddlDoseType.Text;
                        if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
                        {
                            DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                            DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                            DR1["IsInfusion"] = true;
                            DR["IsInfusion"] = true;
                        }
                        else if (ddlDoseType.SelectedValue != "0")
                        {
                            DR1["IsInfusion"] = false;
                            DR1["FrequencyId"] = 0;
                            DR1["FrequencyName"] = "";
                        }
                        else
                        {
                            DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                            DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                            DR1["IsInfusion"] = false;
                        }
                        dt1.Rows.Add(DR1);
                        dt1.AcceptChanges();
                        ddlReferanceItem.SelectedValue = "0";
                        txtInstructions.Text = "";
                    }
                    dt1.TableName = "ItemDetail";
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    dt1.WriteXml(writer);

                    string xmlSchema = writer.ToString();
                    DR["XMLData"] = xmlSchema;
                    DR["PrescriptionDetail"] = emr.GetPrescriptionDetailString(dt1);
                    //DR["Qty"] = dQty.ToString("F2");
                    //---------------------

                    DR["Qty"] = dr["Qty"];

                    dt.Rows.Add(DR);
                    dt.AcceptChanges();
                }
            }
            //}

            //-----------------------------
            // dt1.TableName = "ItemDetail";
            // System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
            //System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            //dt1.WriteXml(writer);
            //string xmlSchema = writer.ToString();
            ViewState["DataTableDetail"] = null;

            if (dt1 != null)
            {
                if (dt1.Columns.Count > 0)
                {
                    if (!dt1.Columns.Contains("ItemFlagName"))
                    {
                        dt1.Columns.Add("ItemFlagName", typeof(string));
                    }
                    if (!dt1.Columns.Contains("ItemFlagCode"))
                    {
                        dt1.Columns.Add("ItemFlagCode", typeof(string));
                    }
                }
            }

            if (dt != null)
            {
                if (dt.Columns.Count > 0)
                {
                    if (!dt.Columns.Contains("ItemFlagName"))
                    {
                        dt.Columns.Add("ItemFlagName", typeof(string));
                    }
                    if (!dt.Columns.Contains("ItemFlagCode"))
                    {
                        dt.Columns.Add("ItemFlagCode", typeof(string));
                    }
                }
            }

            ViewState["ItemDetail"] = dt1;

            ViewState["Item"] = dt;
            gvItem.DataSource = dt;
            gvItem.DataBind();

            BindBlankItemDetailGrid();
            setVisiblilityInteraction();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            ddlStrength.Enabled = true;
            ddlBrand.Focus();
            ddlBrand.Items.Clear();
            ddlBrand.Text = "";
            ddlGeneric.Items.Clear();
            ddlGeneric.Text = "";
            ddlBrand.Enabled = true;
            ddlBrand.SelectedValue = "0";


            ViewState["Edit"] = null;
            ViewState["ItemId"] = null;
            txtCustomMedication.Text = "";

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
            emr = null;
        }
    }

    private void addItem()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtNew = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            DataRow DR;
            DataRow DR1;
            decimal dQty = 0;
            int countRow = 0;
            if (!chkCustomMedication.Checked)
            {
                if (ddlBrand.SelectedValue == "" || ddlBrand.SelectedValue == "0")
                {
                    Alert.ShowAjaxMsg("Please select drug", Page);
                    return;
                }
                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    //     string RouteMandatory = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //common.myInt(Session["FacilityId"]), "RouteMandatory", sConString);
                    //if (RouteMandatory == "Y")
                    //{
                    //    if (ddlRoute.SelectedValue == "" || ddlRoute.SelectedValue == "0")
                    //    {
                    //        Alert.ShowAjaxMsg("Please select Route", Page); return;
                    //    }
                    //}
                }
            }
            if (ViewState["Item"] == null && ViewState["Edit"] == null)
            {
                dt = CreateItemTable();
                dt1 = CreateItemDetailTable();
            }
            else
            {
                dtold = (DataTable)ViewState["Item"];

                if (common.myBool(ViewState["Edit"]) && ViewState["Item"] != null)
                {
                    DataView dv = new DataView(dtold);
                    dv.RowFilter = "ItemId<>" + common.myInt(ViewState["ItemId"]).ToString();
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
                            TextBox txtPackSize = (TextBox)dataItem.FindControl("txtPackSize");
                            dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
                            dt.Rows[countRow]["PackSize"] = common.myInt(txtPackSize.Text);
                            countRow++;
                            dt.AcceptChanges();
                        }
                    }
                }
                if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
                {
                    dt1 = CreateItemDetailTable();
                }
                else
                {
                    dt1old = (DataTable)ViewState["ItemDetail"];
                    if (common.myBool(ViewState["Edit"]) && ViewState["ItemDetail"] != null)
                    {
                        DataView dv1 = new DataView(dt1old);
                        dv1.RowFilter = "ItemId<>" + common.myInt(ViewState["ItemId"]).ToString();
                        dt1 = dv1.ToTable();
                    }
                    else
                    {
                        dt1 = (DataTable)ViewState["ItemDetail"];
                    }
                }
            }
            if (chkCustomMedication.Checked == false)
            {
                foreach (GridViewRow row in gvItem.Rows)
                {
                    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                    if (hdnItemId.Value == ddlBrand.SelectedValue && ViewState["Edit"] == null)
                    {
                        Alert.ShowAjaxMsg("Item already added", Page);
                        return;
                    }
                }
            }
            //Item
            DR = dt.NewRow();

            if (ddlIndentType.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please select Indent Type", Page);
                return;
            }
            DR["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
            DR["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;
            DR["IndentId"] = 0;
            DR["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
            if (ddlBrand.SelectedValue == "" && chkCustomMedication.Checked == true)
            {
                DR["ItemId"] = DBNull.Value;
            }
            else if (ddlBrand.SelectedValue != "" && chkCustomMedication.Checked == false)
            {
                DR["ItemId"] = common.myInt(ddlBrand.SelectedValue);
            }
            DR["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
            DR["ItemName"] = chkCustomMedication.Checked == false && ddlBrand.SelectedValue != ""
                && ddlBrand.SelectedValue != "0" ? ddlBrand.Text : txtCustomMedication.Text;
            DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
            DR["CustomMedication"] = chkCustomMedication.Checked;
            DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
            DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;

            DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            DR["UnitName"] = common.myStr(ViewState["UnitName"]);
            DR["ConversionFactor2"] = common.myInt(hdnConversionFactor2Globel.Value); //common.myInt(ViewState["ConversionFactor2"]);
            ///Item Detail
            TextBox txtInstructions = new TextBox();
            RadComboBox ddlFoodRelation = new RadComboBox();
            foreach (GridViewRow dataItem in gvItemDetails.Rows)
            {
                DR1 = dt1.NewRow();
                string sFrequency = "0";
                RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
                TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
                if (ddlFrequencyId.SelectedValue != "0")
                {
                    sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                }

                TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");

                RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");

                RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");

                RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    if (ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0")
                    {
                        if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == "")
                        {
                            Alert.ShowAjaxMsg("Please select Frequency", Page);
                            return;
                        }

                        if (ddlReferanceItem.SelectedValue == "0" || ddlReferanceItem.SelectedValue == "")
                        {
                            if (common.myDbl(txtDose.Text) == 0.0)
                            {
                                txtDose.Text = "1";
                                //Alert.ShowAjaxMsg("Please type Dose", Page);
                                //return;
                            }
                            if (txtDuration.Text == "" || txtDuration.Text == "0"
                                || txtDuration.Text == "0." || txtDuration.Text == ".0"
                                || txtDuration.Text == ".")
                            {
                                Alert.ShowAjaxMsg("Please type Duration", Page);
                                return;
                            }
                        }
                    }
                }

                Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
                string Type = "";
                decimal dDuration = 0;


                switch (common.myStr(ddlPeriodType.SelectedValue))
                {
                    case "N":
                        Type = txtDuration.Text + " Minute(s)";
                        dDuration = 1;
                        break;
                    case "H":
                        Type = txtDuration.Text + " Hour(s)";
                        dDuration = 1;
                        break;
                    case "D":
                        Type = txtDuration.Text + " Day(s)";
                        dDuration = 1;
                        break;
                    case "W":
                        Type = txtDuration.Text + " Week(s)";
                        dDuration = 7;
                        break;
                    case "M":
                        Type = txtDuration.Text + " Month(s)";
                        dDuration = 30;
                        break;
                    case "Y":
                        Type = txtDuration.Text + " Year(s)";
                        dDuration = 365;
                        break;
                }
                RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
                HiddenField StartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                HiddenField EndDate = (HiddenField)dataItem.FindControl("hdnEndDate");
                HiddenField Remarks = (HiddenField)dataItem.FindControl("hdnRemarks");
                ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
                RadDatePicker txtStartDate = (RadDatePicker)dataItem.FindControl("txtStartDate");
                RadDatePicker txtEndDate = (RadDatePicker)dataItem.FindControl("txtEndDate");
                HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                HiddenField hdnVIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                HiddenField hdnQty = (HiddenField)dataItem.FindControl("hdnQty");


                txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");
                DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);

                HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                //if (Request.QueryString["DRUGORDERCODE"] == null)
                //{
                if (common.myBool(ViewState["ISCalculationRequired"]))
                {
                    // dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                    dQty = common.myDec(calcTotalQty(txtDose.Text, sFrequency, txtDuration.Text, ddlPeriodType.SelectedValue));
                    if (common.myDbl(dQty) == 0.0)
                    {
                        dQty = 1;
                    }
                }
                else
                {
                    dQty = 1;
                }

                DR1["RouteId"] = common.myInt(ddlRoute.SelectedValue);
                if (!common.myInt(ddlRoute.SelectedValue).Equals(0))
                {
                    DR1["RouteName"] = common.myStr(ddlRoute.SelectedItem.Text);
                }
                else
                {
                    DR1["RouteName"] = string.Empty;
                }
                //}
                //else
                //{
                //    dQty = 0;
                //}
                //dQty = common.myDec(calcTotalQty(txtDose.Text, sFrequency, txtDuration.Text, ddlPeriodType.SelectedValue));


                DR1["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
                DR1["ItemId"] = ddlBrand.SelectedValue == "" ? 0 : common.myInt(ddlBrand.SelectedValue);
                DR1["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
                DR1["ItemName"] = ddlBrand.SelectedValue == "" ? "" : ddlBrand.Text;


                DR1["Dose"] = common.myDec(txtDose.Text);
                DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                DR1["Duration"] = txtDuration.Text;
                DR1["DurationText"] = Type;
                DR1["Type"] = ddlPeriodType.SelectedValue;
                if (common.myStr(Session["OPIP"]) == "I")
                {
                    DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    if (common.myStr(txtStartDate.SelectedDate) == "")
                        DR1["StartDate"] = "";
                    else
                        DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
                }

                DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

                DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
                DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? "" : common.myStr(ddlFoodRelation.Text);

                if (common.myInt(ddlUnit.SelectedValue).Equals(0))
                {
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByText("Nos"));
                }

                DR1["UnitId"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
                DR1["UnitName"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? "" : ddlUnit.Text;
                DR1["CIMSItemId"] = common.myInt(hdnCIMSItemId.Value);
                DR1["CIMSType"] = hdnCIMSType.Value;
                DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
                DR1["PrescriptionDetail"] = "";
                DR1["Instructions"] = txtInstructions.Text;
                DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
                DR1["ReferanceItemName"] = common.myStr(ddlReferanceItem.Text);
                DR1["DoseTypeId"] = ddlDoseType.SelectedValue;
                DR1["DoseTypeName"] = ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0" ? "" : ddlDoseType.Text;
                if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
                {
                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                    DR1["IsInfusion"] = true;
                    DR["IsInfusion"] = true;
                }
                else if (ddlDoseType.SelectedValue != "0")
                {
                    DR1["IsInfusion"] = false;
                    DR1["FrequencyId"] = 0;
                    DR1["FrequencyName"] = "";
                }
                else
                {
                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                    DR1["IsInfusion"] = false;
                }

                DR1["ItemFlagName"] = common.myStr(hdnItemFlagName.Value);
                DR1["ItemFlagCode"] = common.myStr(hdnItemFlagCode.Value);

                dt1.Rows.Add(DR1);
                dt1.AcceptChanges();
                ddlReferanceItem.SelectedValue = "0";
                txtInstructions.Text = "";
            }
            dt1.TableName = "ItemDetail";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            dt1.WriteXml(writer);

            string xmlSchema = writer.ToString();
            DR["XMLData"] = xmlSchema;
            DR["PrescriptionDetail"] = emr.GetPrescriptionDetailString(dt1);
            DR["Qty"] = dQty.ToString("F2");
            DR["PackSize"] = common.myInt(dQty);
            // DR["Qty"] = 0;

            DR["ItemFlagName"] = common.myStr(hdnItemFlagName.Value);
            DR["ItemFlagCode"] = common.myStr(hdnItemFlagCode.Value);

            dt.Rows.Add(DR);
            dt.AcceptChanges();

            ViewState["DataTableDetail"] = null;
            ViewState["ItemDetail"] = dt1;
            ViewState["Item"] = dt;
            gvItem.DataSource = dt;
            gvItem.DataBind();

            BindBlankItemDetailGrid();

            setVisiblilityInteraction();

            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            ddlStrength.Enabled = true;

            ddlBrand.Focus();
            ddlBrand.Items.Clear();
            ddlBrand.Text = "";
            lblGenericName.Text = "";

            ddlGeneric.Items.Clear();
            ddlGeneric.Text = "";
            ddlGeneric.SelectedValue = "";

            ddlBrand.Enabled = true;
            ddlBrand.SelectedValue = "0";
            txtInstructions.Text = "";
            ddlFoodRelation.SelectedValue = "0";
            ddlFoodRelation.Text = "";
            chkNotToPharmacy.Checked = false;
            //ViewState["ItemDetail"] = null;

            ViewState["Edit"] = null;
            ViewState["ItemId"] = null;
            txtCustomMedication.Text = "";
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
            emr = null;
        }
    }
    protected void chkFavouriteOnly_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkFavouriteOnly.Checked)
        {
        }
    }
    protected void chkCustomMedication_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomMedication.Checked)
        {
            hdnGenericId.Value = "0";
            ddlGeneric.Text = "";
            hdnItemId.Value = "0";
            ddlBrand.Text = "";
            ddlBrand.SelectedValue = "*";
            hdnCIMSItemId.Value = "";
            hdnCIMSType.Value = "";
            hdnVIDALItemId.Value = "";
            ViewState["ISCalculationRequired"] = null;
            chkNotToPharmacy.Visible = false;

            chkFavouriteOnly.Checked = false;
            chkFavouriteOnly.Visible = false;
            btnAddtoFav.Visible = false;
            ibtnFavourite.Visible = false;
        }
        else
        {
            chkNotToPharmacy.Visible = true;
            txtCustomMedication.Text = "";

            chkFavouriteOnly.Visible = true;
            btnAddtoFav.Visible = true;
            ibtnFavourite.Visible = true;
        }

        chkCustomMedicationChanged();
    }
    private void chkCustomMedicationChanged()
    {
        trGeneric.Visible = !chkCustomMedication.Checked;
        trDrugs.Visible = !chkCustomMedication.Checked;
        trCustomMedication.Visible = chkCustomMedication.Checked;
    }
    protected void btnGetInfoGeneric_Click(object sender, EventArgs e)
    {
        ddlFormulation.Enabled = true;
        ddlRoute.Enabled = true;
        ddlStrength.Enabled = true;
        ddlBrand.Focus();
        if (common.myStr(ddlGeneric.Text.Trim()) == "")
        {
            ddlGeneric.SelectedValue = "";

        }
    }
    private void SetStrengthFormUnitRouteOfSelectedDrug()
    {
        DataSet ds = new DataSet();
        try
        {
            string brandID = common.myStr(ddlBrand.SelectedValue);
            ds = GetphrGenericOfSelectedDrug(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["Facilityid"]), common.myInt(brandID));

            //ddlGeneric.Items.Add(new RadComboBoxItem(ds.Tables[0].Rows[0][""].ToString
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!common.myStr(ds.Tables[0].Rows[0]["StrengthId"]).Equals(string.Empty))
                {
                    ddlStrength.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["StrengthId"]);
                    ddlStrength.Enabled = false;
                }
                else
                {
                    ddlStrength.SelectedIndex = 0;
                    ddlStrength.Enabled = true;
                }
                if (!common.myStr(ds.Tables[0].Rows[0]["FormulationId"]).Equals(string.Empty))
                {
                    ddlFormulation.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["FormulationId"]);
                    ddlFormulation.Enabled = false;
                }
                else
                {
                    ddlFormulation.SelectedIndex = 0;
                    ddlFormulation.Enabled = true;
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlRoute.DataSource = ds.Tables[1];
                    ddlRoute.DataValueField = "Id";
                    ddlRoute.DataTextField = "RouteName";
                    ddlRoute.DataBind();

                    //ddlRoute.Items.Insert(0, new RadComboBoxItem("", "0"));
                    //ddlRoute.SelectedIndex = 0;
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
                            DV.RowFilter = "ICDCode='" + common.myStr(item) + "'";

                            if (DV.ToTable().Rows.Count > 0)
                            {
                                drdt["ICDCodes"] = common.myStr(item);
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
            clearItemDetails();
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
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsFavourite = new DataSet();
        try
        {
            lblMessage.Text = string.Empty;
            //Change palendra
            if (common.myStr(hdnRestrictedItemForPanel.Value).Equals("1") && common.myStr(ddlIndentType.SelectedItem.Text).Equals("Discharge"))
            {
                Alert.ShowAjaxMsg("this brand is restricted!", Page);
                ddlBrand.Items.Clear();
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;
                return;
            }
            //Change palendra
            int ItemId = common.myInt(hdnItemId.Value);
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

            if (ItemId > 0)
            {
                if (!common.myBool(hdnLowCost.Value) && common.myBool(ViewState["IsCompanyLowCost"]))
                {
                    if (common.myStr(ViewState["IsAllowSubtituteItemForLowCostItem"]).ToUpper() == "Y")
                    {
                        ddlBrand.Focus();
                        getSubtituteItems(new RadComboBoxItemsRequestedEventArgs(), common.myInt(hdnItemId.Value));
                        return;
                    }
                }
                else
                {
                    ddlBrand.OpenDropDownOnLoad = false;
                }
            }

            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);
            clearItemDetails();
            BindICDPanel();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            ddlStrength.Enabled = true;
            //txtStrengthValue.Enabled = true;
            //int iOT = 3;
            //if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim().Equals("OT")
            //    && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim().Equals("CO"))
            //{
            //    iOT = 2;
            //}
            //else if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim().Equals("WARD")
            //    && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim().Equals("CO"))
            //{
            //    iOT = 2;
            //}
            //else
            //{
            //    iOT = 1;
            //}
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

                ViewState["ISCalculationRequired"] = null;
                ViewState["UnitName"] = "NOS";
                if (chkFavouriteOnly.Checked)
                {

                    dsFavourite = objEMR.getEMRFavouriteDrugsAttributes(DoctorId, ItemId);

                    if (dsFavourite.Tables.Count > 0)
                    {
                        if (dsFavourite.Tables[0].Rows.Count > 0)
                        {
                            if (common.myInt(dsFavourite.Tables[0].Rows[0]["FormulationId"]) > 0)
                            {
                                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(dsFavourite.Tables[0].Rows[0]["FormulationId"]).ToString()));
                            }
                            if (common.myInt(dsFavourite.Tables[0].Rows[0]["RouteId"]) > 0)
                            {
                                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(dsFavourite.Tables[0].Rows[0]["RouteId"]).ToString()));
                            }

                            foreach (GridViewRow dataItem in gvItemDetails.Rows)
                            {
                                RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
                                TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
                                RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
                                TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
                                RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
                                RadComboBox ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
                                TextBox txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

                                DataRow DrFavourite = dsFavourite.Tables[0].Rows[0];

                                if (common.myInt(DrFavourite["FrequencyId"]) > 0)
                                {
                                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DrFavourite["FrequencyId"]).ToString()));
                                }
                                if (common.myInt(DrFavourite["Duration"]) > 0)
                                {
                                    txtDuration.Text = common.myInt(DrFavourite["Duration"]).ToString();
                                }
                                if (common.myLen(DrFavourite["DurationType"]) > 0)
                                {
                                    ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DrFavourite["DurationType"])));
                                }
                                if (common.myDbl(DrFavourite["Dose"]) > 0)
                                {
                                    double numberToSplit = 0;
                                    double decimalresult = 0;

                                    numberToSplit = common.myDbl(DrFavourite["Dose"]);
                                    decimalresult = (int)numberToSplit - numberToSplit;
                                    if (numberToSplit == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        if (decimalresult == 0)
                                        {
                                            txtDose.Text = ((int)numberToSplit).ToString();
                                        }
                                        else
                                        {
                                            txtDose.Text = numberToSplit.ToString("F2");
                                        }
                                    }

                                }
                                if (common.myInt(DrFavourite["UnitId"]) > 0)
                                {
                                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DrFavourite["UnitId"]).ToString()));
                                }
                                if (common.myInt(DrFavourite["FoodRelationshipId"]) > 0)
                                {
                                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DrFavourite["FoodRelationshipId"]).ToString()));
                                }

                                txtInstructions.Text = common.myStr(DrFavourite["Instructions"]);

                                break;
                            }
                        }
                    }
                }
                else
                {
                    //ds = objPharmacy.getItemMaster(ItemId, string.Empty, string.Empty, 1, common.myInt(Session["HospitalLocationID"]),
                    //                                common.myInt(Session["UserId"]), iOT, common.myInt(hdnStoreId.Value), common.myInt(Session["FacilityId"]));
                    ds = objPhr.getItemAttributes(common.myInt(Session["HospitalLocationID"]), ItemId, common.myInt(hdnStoreId.Value),
                                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), DoctorId,
                                                common.myInt(Session["CompanyId"]), 0);
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
                        if (ds.Tables[0].Columns.Contains("IsExclude"))
                        {
                            if (common.myBool(ds.Tables[0].Rows[0]["IsExclude"]))
                            {
                                ViewState["ExcludeItemId"] = ItemId;
                                Alert.ShowAjaxMsg("Selected item is excluded for the payer.", this.Page);
                            }
                            else
                            {
                                ViewState["ExcludeItemId"] = null;
                            }
                        }
                        //comment for not set unit and route based on formulation
                        hdnGenericId.Value = common.myInt(DR["GenericId"]).ToString();
                        lblGenericName.Text = common.myStr(DR["GenericName"]);
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                        //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
                        //ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["ItemUnitId"]).ToString()));
                        //txtStrengthValue.Text = common.myStr(DR["StrengthValue"]);
                        ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
                        //ViewState["ConversionFactor2"] = common.myInt(DR["ConversionFactor2"]);
                        ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);
                        hdnInfusion.Value = common.myStr(DR["IsInfusion"]);
                        //hdnIsInjection.Value = common.myBool(DR["IsInjection"]).ToString();

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
                            //ddlReferanceItem.Items.Add(item);
                        }
                        //txtDose.Focus();
                        //calcTotalQty();
                        if (common.myInt(DR["ClosingBalance"]).Equals(0))
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
                                ViewState["PrescriptionAllowForItemsNotInStockShowAlert"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "PrescriptionAllowForItemsNotInStockShowAlert", sConString);

                                if (common.myStr(ViewState["PrescriptionAllowForItemsNotInStockShowAlert"]).Equals("Y"))
                                {
                                    Alert.ShowAjaxMsg(strMsg, this.Page);
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
                                strConfirmMessage += "High risk Item";
                            }
                            if (common.myBool(DR["ExpensiveValueItem"]))
                            {
                                if (common.myLen(strConfirmMessage) > 0)
                                {
                                    strConfirmMessage += "<br/>";
                                }
                                strConfirmMessage += "High value Item";
                            }
                            if (common.myBool(DR["LasaValueItem"]))
                            {
                                if (common.myLen(strConfirmMessage) > 0)
                                {
                                    strConfirmMessage += "<br/>";
                                }
                                strConfirmMessage += "LASA Item";
                            }

                            lblConfirmHighValue.Text = strConfirmMessage + "<br/>Do you want to proceed!";
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
                            //chkIsInteraction(ItemId);
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
                //BindBlankItemDetailGrid();
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

    //protected void btnGetInfo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int ItemId = common.myInt(hdnItemId.Value);

    //        string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
    //        string CIMSType = common.myStr(hdnCIMSType.Value);
    //        int VIDALItemId = common.myInt(hdnVIDALItemId.Value);

    //        clearItemDetails();

    //        ddlFormulation.Enabled = true;
    //        ddlRoute.Enabled = true;
    //        ddlStrength.Enabled = true;
    //        int iOT = 3;

    //        if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString().Trim() == "OT"
    //            && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString().Trim() == "CO")
    //        {
    //            iOT = 2;
    //        }
    //        else if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString().Trim() == "WARD"
    //            && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString().Trim() == "CO")
    //        {
    //            iOT = 2;
    //        }
    //        else
    //        {
    //            iOT = 1;
    //        }

    //        SetStrengthFormUnitRouteOfSelectedDrug();

    //        if (ItemId > 0)
    //        {
    //            hdnGenericId.Value = "0";
    //            lblGenericName.Text = "";

    //            objPharmacy = new BaseC.clsPharmacy(sConString);

    //            DataSet ds = objPharmacy.getItemMaster(ItemId, "", "", 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), iOT);
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow DR = ds.Tables[0].Rows[0];

    //                hdnGenericId.Value = common.myInt(DR["GenericId"]).ToString();
    //                lblGenericName.Text = common.myStr(DR["GenericName"]);
    //                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
    //                //ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
    //                ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
    //                ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
    //                ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);

    //                hdnInfusion.Value = DR["IsInfusion"].ToString();
    //                ddlFormulation.Enabled = false;
    //                //ddlRoute.Enabled = false;
    //                ddlStrength.Enabled = false;

    //                //txtDose.Focus();
    //            }
    //        }
    //        BindBlankItemDetailGrid();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    private string getMonographXML(string CIMSType, string CIMSItemId)
    {
        string strXML = "";
        try
        {
            //<Request>
            //    <Content>
            //        <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
            //    </Content>
            //</Request>

            //strXML = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";

            // <MONOGRAPH>
            CIMSType = (CIMSType == "") ? "Product" : CIMSType;

            strXML = "<Request><Content><" + CIMSType + " reference=\"" + CIMSItemId + "\" /></Content></Request>";
        }
        catch
        { }

        return strXML;
    }

    private string getInterationXML(string strNewPrescribing)
    {
        string strXML = "";
        try
        {
            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
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
                //        <Allergies />
                //        <References/>
                //    </Interaction>
                //</Request>

                string strPrescribing = "";

                StringBuilder ItemIds = new StringBuilder();
                foreach (GridViewRow dataItem in gvPrevious.Rows)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if (common.myStr(CIMSItemId.Value).Trim().Length > 0
                        && lnkBtnInteractionCIMS.Visible)
                    {
                        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                        CIMSType = (CIMSType == "") ? "Product" : CIMSType;

                        if (strNewPrescribing != "" && strPrescribing == "")
                        {
                            strPrescribing = strNewPrescribing;
                        }

                        if (strPrescribing == "")
                        {
                            strPrescribing = "<Prescribing><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></Prescribing>";
                        }
                        else
                        {
                            ItemIds.Append("<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />");
                        }


                        //if (Prescribing == "")
                        //{
                        //    Prescribing = "Prescribing";
                        //}
                        //else
                        //{
                        //    Prescribing = "Prescribed";
                        //}

                        //ItemIds.Append("<" + Prescribing + "><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></" + Prescribing + ">");

                    }
                }

                if (ItemIds.ToString() == "")
                {
                    return "";
                }

                strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";

                // <Severity name
                //strXML = "<Request><Interaction><Prescribing><Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" /></Prescribing><Prescribed><Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" /><Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" /></Prescribed><Allergies /><References /></Interaction></Request>";

                strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
            }
        }
        catch
        { }

        return strXML;
    }

    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();

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
        catch
        {
        }
        return commonNameGroupIds;
    }

    private void setVisiblilityInteraction()
    {
        try
        {
            //if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            //{
            //    string strXML = getInterationXML("");

            //    string outputValues = string.Empty;

            //    if (strXML != "")
            //    {
            //        outputValues = objCIMS.getFastTrack5Output(strXML);

            //        foreach (GridViewRow dataItem in gvPrevious.Rows)
            //        {
            //            HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
            //            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

            //            if (!outputValues.ToUpper().Contains("<SEVERITY NAME"))
            //            {
            //                lnkBtnInteractionCIMS.Visible = false;
            //                continue;
            //            }
            //            else
            //            {
            //                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && lnkBtnInteractionCIMS.Visible)
            //                {
            //                    string strFindCIMSItemId = "<PRODUCT REFERENCE=\"" + common.myStr(CIMSItemId.Value).Trim() + "\" NAME=\"\"></PRODUCT>";

            //                    if (outputValues.ToUpper().Contains(strFindCIMSItemId.ToUpper()))
            //                    {
            //                        lnkBtnInteractionCIMS.Visible = false;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
            //    ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");

            //    strXML = getHealthOrAllergiesInterationXML("H");//Helth

            //    if (strXML != "")
            //    {
            //        outputValues = objCIMS.getFastTrack5Output(strXML);

            //        if (outputValues.Length > strXML.Length && outputValues.Length > 0)
            //        {
            //            if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
            //            {
            //                ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
            //            }
            //        }
            //    }

            //    strXML = getHealthOrAllergiesInterationXML("A");//Allergies

            //    if (strXML != "")
            //    {
            //        outputValues = objCIMS.getFastTrack5Output(strXML);

            //        if (outputValues.Length > strXML.Length && outputValues.Length > 0)
            //        {
            //            if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
            //            {
            //                ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");
            //            }
            //        }
            //    }

            //    lnkDrugAllergy.BackColor = (System.Drawing.Color)ViewState["DrugAllergyColorSet"];

            //    //int count = 0;
            //    //int rIdx = 0;
            //    //int rIdxDataFound = 0;

            //    //foreach (GridViewRow dataItem in gvStore.Rows)
            //    //{
            //    //    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

            //    //    if (common.myStr(CIMSItemId.Value).Trim().Length > 0)
            //    //    {
            //    //        if (rIdxDataFound == 0)
            //    //        {
            //    //            rIdxDataFound = count;
            //    //        }
            //    //        rIdx++;
            //    //    }
            //    //    count++;
            //    //}

            //    //if (rIdx == 1)
            //    //{
            //    //    LinkButton lnkBtnInteractionCIMS = (LinkButton)gvStore.Rows[rIdxDataFound].FindControl("lnkBtnInteractionCIMS");
            //    //    if (lnkBtnInteractionCIMS.Visible)
            //    //    {
            //    //        lnkBtnInteractionCIMS.Visible = false;
            //    //    }
            //    //}
            //}
            //else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            //{
            //    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
            //    lnkDrugAllergy.Visible = false;

            //    if (commonNameGroupIds.Length > 0)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        Hashtable collVitalItemIdFound = new Hashtable();
            //        objVIDAL = new clsVIDAL(sConString);

            //        sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);



            //        DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
            //        //int? weight = common.myInt(lbl_Weight.Text);//In kilograms
            //        //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            //        int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            //        int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            //        Hashtable collVitalItemIdFoundDH = new Hashtable();

            //        StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
            //                0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
            //                (ViewState["PatientDiagnosisXML"] != "") ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
            //                out collVitalItemIdFoundDH);

            //        foreach (GridViewRow dataItem in gvPrevious.Rows)
            //        {
            //            HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");

            //            if (common.myInt(VIDALItemId.Value) > 0)
            //            {
            //                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
            //                LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");

            //                if (collVitalItemIdFound.ContainsValue(common.myInt(VIDALItemId.Value)))
            //                {
            //                    lnkBtnInteractionVIDAL.Visible = true;
            //                }
            //                else
            //                {
            //                    lnkBtnInteractionVIDAL.Visible = false;
            //                }

            //                if (sbDHI.ToString().Length > 0 || collVitalItemIdFoundDH.ContainsValue(common.myInt(VIDALItemId.Value)))
            //                {
            //                    lnkBtnDHInteractionVIDAL.Visible = true;
            //                }
            //                else
            //                {
            //                    lnkBtnDHInteractionVIDAL.Visible = false;
            //                }
            //            }
            //        }

            //        int?[] allergyIds = null; //new int?[] { 114 };
            //        int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

            //        if (ViewState["PatientAllergyXML"] != "")
            //        {
            //            allergyIds = (int?[])ViewState["PatientAllergyXML"];
            //        }

            //        sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

            //        if (sb.ToString().Length > 0)
            //        {
            //            lnkDrugAllergy.Visible = true;
            //        }
            //        else
            //        {
            //            lnkDrugAllergy.Visible = false;
            //        }
            //    }
            //}
        }
        catch
        {
        }
    }

    protected void btnInteractionView_OnClick(object sender, EventArgs e)
    {
        showIntreraction();
    }

    protected void btnMonographView_OnClick(object sender, EventArgs e)
    {
        showMonograph(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
    }

    private void showMonograph(string CIMSItemId, string CIMSType)
    {
        if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
        {
            Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
        }

        string strXML = getMonographXML(CIMSType, common.myStr(CIMSItemId));

        if (strXML != "")
        {
            Cache.Insert("CIMSXML" + common.myStr(Session["UserId"]), strXML, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            openWindowsCIMS();
        }
    }

    private void showIntreraction()
    {
        if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
        {
            Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML("");

        if (strXML != "")
        {
            Cache.Insert("CIMSXML" + common.myStr(Session["UserId"]), strXML, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            openWindowsCIMS();
        }
    }

    private void showHealthOrAllergiesIntreraction(string HealthOrAllergies)
    {
        if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
        {
            if (Cache["CIMSXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("CIMSXML" + common.myStr(Session["UserId"]));
            }

            string strXML = getHealthOrAllergiesInterationXML("B");

            if (strXML != "")
            {
                Cache.Insert("CIMSXML" + common.myStr(Session["UserId"]), strXML, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsCIMS();
            }
        }
        else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
        {
            if (HealthOrAllergies == "H")//Health
            {

                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugHealthInteractionVidal(commonNameGroupIds);
                }
            }
            else if (HealthOrAllergies == "A")//Allergies
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();

                if (commonNameGroupIds.Length > 0)
                {
                    getDrugAllergyVidal(commonNameGroupIds);
                }
            }
        }
    }

    private void openWindowsCIMS()
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/Monograph1.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    private void openWindowsVIDAL(string parameters)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/MonographVidal.aspx" + parameters;
        RadWindow1.Height = 550;
        RadWindow1.Width = 800;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        if (parameters.Contains("UseFor=MO"))
        {
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnInteractionContinue_OnClick(object sender, EventArgs e)
    {
        addItem();

        dvInteraction.Visible = false;
    }

    protected void btnInteractionCancel_OnClick(object sender, EventArgs e)
    {
        dvInteraction.Visible = false;
    }

    private string getHealthOrAllergiesInterationXML(string useFor)
    {
        string strXML = "";
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


            string strPrescribing = "";

            StringBuilder ItemIds = new StringBuilder();
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0)
                {
                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
                    CIMSType = (CIMSType == "") ? "Product" : CIMSType;

                    if (strPrescribing == "")
                    {
                        strPrescribing = "<Prescribing><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></Prescribing>";
                    }
                    else
                    {
                        ItemIds.Append("<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />");
                    }

                    //if (Prescribing == "")
                    //{
                    //    Prescribing = "Prescribing";
                    //}
                    //else
                    //{
                    //    Prescribing = "Prescribed";
                    //}

                    //ItemIds.Append("<" + Prescribing + "><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></" + Prescribing + ">");

                }
            }

            if (ItemIds.ToString() == "")
            {
                return "";
            }

            strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";

            // <Severity name
            //strXML = "<Request><Interaction><Prescribing><Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" /></Prescribing><Prescribed><Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" /><Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" /></Prescribed><Allergies /><References /></Interaction></Request>";
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
        catch
        { }

        return strXML;
    }
    protected void lnkStopMedication_OnClick(object sender, EventArgs e)
    {
        string sRegId = common.myInt(Request.QueryString["RegId"]).Equals(0) ? common.myInt(Session["RegistrationId"]).ToString() : common.myInt(Request.QueryString["RegId"]).ToString();
        string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();

        ViewState["Mode"] = "S";
        RadWindow1.NavigateUrl = "/EMR/Medication/StopMedication.aspx?RegId=" + sRegId + "&EncId=" + sEncId;
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkDrugAllergy_OnClick(object sender, EventArgs e)
    {
        showHealthOrAllergiesIntreraction("A");
    }

    private void setDiagnosis()
    {
        DataSet ds = new DataSet();
        try
        {
            ViewState["PatientDiagnosisXML"] = "";
            if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                || common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                objEMR = new BaseC.clsEMR(sConString);

                ds = objEMR.getDiagnosis(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                    {
                        StringBuilder HealthIssueCodes = new StringBuilder();
                        StringBuilder HealthCode = new StringBuilder();

                        foreach (DataRow DR in ds.Tables[0].Rows)
                        {
                            if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                            {
                                HealthCode.Append("<HealthIssueCode code=\"" + common.myStr(DR["ICDCode"]).Trim() + "\" codeType=\"ICD10\" />");
                            }
                        }

                        if (HealthCode.ToString() == "")
                        {
                            HealthIssueCodes.Append("<HealthIssueCodes />");
                        }
                        else
                        {
                            HealthIssueCodes.Append("<HealthIssueCodes>" + HealthCode.ToString() + "</HealthIssueCodes>");
                        }

                        ViewState["PatientDiagnosisXML"] = HealthIssueCodes.ToString();
                    }
                    else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
                    {
                        List<string> list = new List<string>();

                        foreach (DataRow DR in ds.Tables[0].Rows)
                        {
                            if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                            {
                                list.Add(common.myStr(DR["ICDCode"]).Trim().Replace(".", ""));
                            }
                        }
                        ViewState["PatientDiagnosisXML"] = list;
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
        }
    }

    private void setAllergiesWithInterfaceCode()
    {
        DataSet ds = new DataSet();
        try
        {
            ViewState["PatientAllergyXML"] = "";

            if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                || common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                objEMR = new BaseC.clsEMR(sConString);
                ds = objEMR.getDrugAllergiesInterfaceCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]));
                DataView DV = ds.Tables[0].DefaultView;
                DataTable tbl = new DataTable();

                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
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

                        if (itemsDetails.ToString() == "")
                        {
                            Allergies.Append("<Allergies />");
                        }
                        else
                        {
                            Allergies.Append("<Allergies>" + itemsDetails.ToString() + "</Allergies>");
                        }

                        ViewState["PatientAllergyXML"] = Allergies.ToString();
                    }
                }
                else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
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

    private void getMonographVidal(int? commonNameGroupId)
    {
        try
        {
            DataTable tbl = new DataTable();

            objVIDAL = new clsVIDAL(sConString);
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
        catch
        {
        }
    }

    private void getDrugToDrugInteractionVidal(int?[] commonNameGroupIds)
    {
        try
        {
            //commonNameGroupIds = new int?[] { 15223, 15070, 1524, 4025, 4212, 516 };

            StringBuilder sb = new StringBuilder();

            Hashtable collVitalItemIdFound = new Hashtable();
            objVIDAL = new clsVIDAL(sConString);

            sb = objVIDAL.getVIDALDrugToDrugInteraction(false, commonNameGroupIds, out collVitalItemIdFound);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != "")
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=IN");
            }
        }
        catch
        {
        }
    }

    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };

            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(lbl_Weight.Text);//In kilograms
            //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            Hashtable collVitalItemIdFoundDH = new Hashtable();

            objVIDAL = new clsVIDAL(sConString);
            StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                    (ViewState["PatientDiagnosisXML"] != "") ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                    out collVitalItemIdFoundDH);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != "")
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=HI");
            }
        }
        catch
        {
        }
    }

    private void getDrugAllergyVidal(int?[] commonNameGroupIds)
    {
        try
        {
            //commonNameGroupIds = new int?[] { 4025, 4212, 516 };

            int?[] allergyIds = null; //new int?[] { 114 };
            int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

            if (ViewState["PatientAllergyXML"] != "")
            {
                allergyIds = (int?[])ViewState["PatientAllergyXML"];
            }

            StringBuilder sb = new StringBuilder();

            objVIDAL = new clsVIDAL(sConString);
            sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != "")
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=DA");
            }
        }
        catch
        {
        }
    }

    private void getWarningVidal(int? commonNameGroupId)
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            objVIDAL = new clsVIDAL(sConString);
            sb = objVIDAL.getVIDALDrugWarning(commonNameGroupId);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != "")
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=WS");
            }
        }
        catch
        {
        }
    }

    private void getSideEffectVidal(int?[] commonNameGroupIds)
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            objVIDAL = new clsVIDAL(sConString);
            sb = objVIDAL.getVIDALDrugSideEffect(commonNameGroupIds);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != "")
            {
                Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

                openWindowsVIDAL("?UseFor=SE");
            }
        }
        catch
        {
        }
    }

    private void setPatientInfo()
    {
        try
        {
            int? weight = null;

            objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Gender"))
                    {
                        ViewState["PatientGender"] = common.myStr(ds.Tables[0].Rows[i][1]);
                    }
                    else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Age"))
                    {
                        ViewState["PatientDOB"] = DateTime.Now.AddDays(-common.myInt(ds.Tables[0].Rows[i][1].ToString())).ToString("yyyy-MM-dd");
                    }
                }

                for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("WT")) //Weight
                    {
                        weight = common.myInt(ds.Tables[0].Rows[i][1]);
                    }
                }
            }

            ViewState["PatientWeight"] = weight;

        }
        catch
        {
        }
    }

    private string BindEditor(Boolean sign)
    {
        if (Request.QueryString["ifId"] != "")
        {
            string sEREncounterId = common.myStr(Session["EncounterId"]);

            StringBuilder sbTemplateStyle = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet dsTemplate = new DataSet();
            DataSet dsTemplateStyle = new DataSet();
            DataRow drTemplateStyle = null;
            DataTable dtTemplate = new DataTable();
            Hashtable hst = new Hashtable();
            string Templinespace = "";
            BaseC.DiagnosisDA fun;

            int RegId = common.myInt(Session["RegistrationID"]);
            int HospitalId = common.myInt(Session["HospitalLocationID"]);
            int EncounterId = common.myInt(Session["EncounterId"]);
            int UserId = common.myInt(Session["UserID"]);

            BindNotes bnotes = new BindNotes(sConString);
            fun = new BaseC.DiagnosisDA(sConString);

            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            //ds = bnotes.GetPatientEMRData(common.myInt(Session["encounterid"]));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            clsIVF objivf = new clsIVF(sConString);

            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
            DataView dvTemplate = dsTemplate.Tables[0].DefaultView;

            dvTemplate.RowFilter = "Sequence=1 OR (TemplateId>0 AND ShowInOrderPage=1)";

            dtTemplate = dvTemplate.ToTable();

            sb.Append("<span style='" + Fonts + "'>");

            string strTemplatePatient = "0";

            for (int i = 0; i < dtTemplate.Rows.Count; i++)
            {
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Complaints"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10003))
                    {
                        bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "", "", "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                   && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10002))
                    {
                        //bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                        //            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
                        //            "",
                        //            "",0);
                        bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                   common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]), "", "", 0, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10001))
                    {
                        bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            "",
                                            "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";

                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                    && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                    sb.Append(sbTemp);
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "", 0, sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);

                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "",
                                       "", Session["OPIP"].ToString(), ""
                                       );

                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                        "",
                                        "", Session["OPIP"].ToString(), "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Order"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10006))
                    {
                        bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
                                Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                "",
                                "", sEREncounterId, "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", "");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }

                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10008))
                    {
                        bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", "");

                        sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                sb.Append("</span>");
            }
            drTemplateStyle = dsTemplateStyle.Tables[0].Rows[0];

            //StringBuilder temp = new StringBuilder();
            //bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), common.myInt(Session["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
            //sb.Append(temp);



            //if (sign == true)
            //{
            //    sb.Append(hdnDoctorImage.Value);
            //}
            //else if (sign == false)
            //{
            //    if (RTF1.Content != null)
            //    {
            //        if (RTF1.Content.Contains("dvDoctorImage") == true)
            //        {
            //            string signData = RTF1.Content.Replace('"', '$');
            //            string st = "<div id=$dvDoctorImage$>";
            //            int start = signData.IndexOf(@st);
            //            if (start > 0)
            //            {
            //                int End = signData.IndexOf("</div>", start);
            //                StringBuilder sbte = new StringBuilder();
            //                sbte.Append(signData.Substring(start, (End + 6) - start));
            //                StringBuilder ne = new StringBuilder();
            //                ne.Append(signData.Replace(sbte.ToString(), ""));
            //                sb.Append(ne.Replace('$', '"').ToString());
            //            }
            //        }

            //    }
            //}

        }

        return sb.ToString();
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }

        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }

    protected void AddStr1(string type, string Saved_RTF_Content, StringBuilder sbTemp, string Lock)
    {
        //sbTemp.Append("<div id='" + type + "'><span style='color: Blue;'>");
        sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
        //if (Lock == "0")
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");            
        //else
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
    }

    protected void AddStr2(string type, ref string Saved_RTF_Content, StringBuilder sbTemp, string Lock, string Linespace, string ShowNote)
    {
        sbTemp.Append("</span></div>");
        if (common.myStr(sbTemp).Length > 49)
        {
            if (Linespace != "")
            {
                int ls = common.myInt(Linespace);
                for (int i = 1; i <= ls; i++)
                {
                    sbTemp.Append("<br/>");
                }
            }
            else
            {
                sbTemp.Append("<br />");
            }
        }
        if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
        {
            if (common.myStr(sbTemp).Length > 62)  //if (sbTemp.ToString().Length > 68)
                sb.Append(common.myStr(sbTemp));
        }
        else
        {
            //change
            Saved_RTF_Content += sbTemp.ToString();

            //if (sbTemp.ToString().Length > 62)//if (sbTemp.ToString().Length > 68)
            //{
            if (ShowNote == "True" && (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null))
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null)
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (common.myStr(ViewState["DefaultTemplate"]) != "")
            {
                if (common.myStr(ViewState["DefaultTemplate"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
            else if (common.myStr(ViewState["PullForward"]) != "")
            {
                if (common.myStr(ViewState["PullForward"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
        }

    }
    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        objStrTmp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //rafat
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        //hstInput.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hstInput.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        if (common.myInt(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (common.myInt(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", common.myStr(1));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", Session["RegistrationID"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
                strGender = "He";
            else
                strGender = "She";
        }
        //Review Of Systems

        hsProblems.Add("@intEncounterId", EncounterId);
        //hsProblems.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hsProblems.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();
            //Make font start

            if (common.myStr(drFont["TemplateBold"]) != "" || common.myStr(drFont["TemplateItalic"]) != "" || common.myStr(drFont["TemplateUnderline"]) != "" || common.myStr(drFont["TemplateFontSize"]) != "" || common.myStr(drFont["TemplateForecolor"]) != "" || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + common.myStr(sDisplayName) + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(common.myStr(sDisplayName));//Default Setting
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
            }

            // Make Font End

            //sb.Append("<u><Strong>Review of systems</Strong></u>");

        }

        // For Positive Symptoms
        if (dtPositiveRos.Rows.Count > 0)
        {
            string strSectionId = ""; // dtPositiveRos.Rows[0]["SectionId"].ToString();
            DataTable dt = new DataTable();
            for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
            {

                DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                        objStrTmp.Append(strGender + " has ");

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }

        // For Negative Symptoms
        if (dtNegativeRos.Rows.Count > 0)
        {
            //if (drFont["TemplateBold"].ToString() != "" || drFont["TemplateItalic"].ToString() != "" || drFont["TemplateUnderline"].ToString() != "" || drFont["TemplateFontSize"].ToString() != "" || drFont["TemplateForecolor"].ToString() != "" || drFont["TemplateListStyle"].ToString() != "")
            //{
            //    string sBegin = "", sEnd = "";
            //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
            //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
            //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}
            //else
            //{
            //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}          
            string strSectionId = ""; // 
            DataTable dt = new DataTable();
            for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
            {

                DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                        objStrTmp.Append(strGender + " does not have ");



                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myInt(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Hashtable hshtable = new Hashtable();
            StringBuilder sbDisplayName = new StringBuilder();
            BaseC.Patient bc = new BaseC.Patient(sConString);
            hshtable.Add("@intTemplateId", pageID);
            hshtable.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            string strDisplayUserName = "select DisplayUserName from EMRTemplate where ID=@intTemplateId and HospitalLocationID=@inyHospitalLocationID";
            DataSet dsDisplayName = DlObj.FillDataSet(CommandType.Text, strDisplayUserName, hshtable);
            if (dsDisplayName.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(dsDisplayName.Tables[0].Rows[0]["DisplayUserName"]).ToUpper() == "TRUE")
                {
                    Hashtable hshUser = new Hashtable();
                    hshUser.Add("@UserID", common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]));
                    hshUser.Add("@inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                    string strUser = "Select ISNULL(FirstName,'') + '' + ISNULL(MiddleName,'') + '' + ISNULL(LastName,'') AS EmployeeName  FROM Employee em INNER JOIN Users us ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";

                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet dsUser = dl.FillDataSet(CommandType.Text, strUser, hshUser);
                    DataTable dt = dsUser.Tables[0];
                    DataRow dr = dt.Rows[0];
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<br/>");
                        string sUBegin = "", sUEnd = "";
                        MakeFontWithoutListStyle("Sections", ref sUBegin, ref sUEnd, drFont);
                        sbDisplayName.Append(sUBegin + "Entered and Verified by " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " " + common.myStr(Convert.ToDateTime(ds.Tables[0].Rows[0]["EncodedDate"]).Date.ToString("MMMM dd yyyy")));
                    }
                    sb.Append(sbDisplayName);
                }
            }
        }
        return sb;
    }


    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myStr(Session["HospitalLocationId"]));
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
                sFontSize = " font-size: " + sFontSize + ";";
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        if (FontName != "")
            sBegin += " font-family: " + FontName + ";";
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }

    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {

        //if (t != null)
        //{
        //    t = t.Replace('"', '$');
        //    //if (Lock == "0")
        //    //{

        //    string st = "<div id=$" + Ttype + "$>";
        //    int RosSt = t.IndexOf(st);
        //    if (RosSt > 0 || RosSt == 0)
        //    {
        //        int RosEnd = t.IndexOf("</div>", RosSt);

        //        //// string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //        //string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //        //string ne = t.Replace(str, strNew);
        //        //t = ne.Replace('$', '"');


        //        if ((RosEnd - RosSt) < (strNew.Length))
        //        {
        //            if ((RosEnd - RosSt) < (strNew.Length))
        //            {
        //                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //                string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //                string ne = t.Replace(str, strNew);
        //                t = ne.Replace('$', '"');
        //            }
        //            //else
        //            //{
        //            //    StringBuilder  strOld = new StringBuilder();
        //            //    StringBuilder strNew1 = new StringBuilder();
        //            //    strOld.Append(t, RosSt, RosEnd);
        //            //    strOld.AppendLine(strNew);
        //            //}
        //        }
        //        else if ((RosEnd - RosSt) > (strNew.Length))
        //        {
        //            // No Action Performed (No Replacement)
        //            t = t.Replace('$', '"');
        //        }
        //    }
        //    else
        //    {
        //        //string st2 = "<div id='" + Ttype + "'>";
        //        //int RosSt2 = t.IndexOf(st2);
        //        //if (RosSt2 > 0)
        //        //{
        //        //    int RosEnd2 = t.IndexOf("</div>", RosSt2);
        //        //    string str2 = t.PadRight(20).Substring(RosSt2, (RosEnd2) - RosSt2);
        //        //    string ne2 = t.Replace(str2, strNew);
        //        //    //t = ne2.Replace('$', '"');
        //        //}
        //        //else
        //        t += strNew; // re-activated on 28 Feb 2011 by rafat
        //        t = t.Replace('$', '"');
        //    }

        //    //}
        //    //else
        //    //{
        //    //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
        //    //    int RosSt = t.IndexOf(st);

        //    //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
        //    //    t = ne.Replace('$', '"');
        //}
        //// }

        if (t != null)
        {
            t = t.Replace('"', '$');
            //if (Lock == "0")
            //{
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
                //Remarks - Case will not happen because all templates <div> tag is inserted at the time of creating encounter

            }
            //}
            //else
            //{
            //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
            //    int RosSt = t.IndexOf(st);

            //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
            //    t = ne.Replace('$', '"');
            //}
        }

        t = t.Replace('$', '"');
    }
    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sFontSize += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sFontSize += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sFontSize += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sFontSize += GetFontFamily(typ, item); };

            if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
            { sFontSize += " font-weight: bold;"; }
            if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
            { sFontSize += " font-style: italic;"; }
            if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
            { sFontSize += " text-decoration: underline;"; }

        }
        return sFontSize;
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();

                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {
                            if (EntryType != "M")
                            {
                                //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                                FieldsLength = objDs.Tables[0].Rows.Count;


                                if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                    && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                                }
                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                    dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                    if (dvValues.ToTable().Rows.Count > MaxLength)
                                    {
                                        MaxLength = dvValues.ToTable().Rows.Count;
                                    }
                                }
                            }
                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                                //objStr.Append("<tr>");
                                //for (int i = 0; i < FieldsLength; i++)
                                //{
                                //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                                //}
                                //objStr.Append("</tr></table>");
                            }
                            else
                            {
                                if (EntryType != "M")
                                {
                                    if (dsMain.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < MaxLength; i++)
                                        {
                                            objStr.Append("<tr>");
                                            if (common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != ""
                                                && common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != "0")
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) + "</td>");
                                            }
                                            //else
                                            //{
                                            //     objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                            //}

                                            for (int j = 0; j < dsMain.Tables.Count; j++)
                                            {
                                                if (dsMain.Tables[j].Rows.Count > i
                                                    && dsMain.Tables[j].Rows.Count > 0)
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                                }
                                                else
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Hashtable hstInput = new Hashtable();
                                    hstInput.Add("@intTemplateId", iRootId);

                                    if (common.myInt(Session["Gender"]) == 1)
                                    {
                                        hstInput.Add("chrGenderType", "F");
                                    }
                                    else if (common.myInt(Session["Gender"]) == 2)
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }
                                    else
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }

                                    hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                                    hstInput.Add("@intSecId", common.myInt(sectionId));
                                    hstInput.Add("@intRecordId", RecordId);
                                    DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                                    DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);
                                    StringBuilder sbCation = new StringBuilder();

                                    if (dvRowCaption.ToTable().Rows.Count > 0)
                                    {
                                        dvRowCaption.RowFilter = "RowNum>0";
                                        DataTable dt = dvRowCaption.ToTable();
                                        if (dt.Rows.Count > 0)
                                        {
                                            sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                            int column = dt.Columns.Count;
                                            int ColumnCount = 0;
                                            int count = 1;

                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            for (int k = 1; k < (column - 5); k++)
                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                                ColumnCount++;
                                            }
                                            sbCation.Append("</tr>");

                                            DataView dvRow = new DataView(dt);
                                            DataTable dtRow = dvRow.ToTable();
                                            for (int l = 1; l <= dtRow.Rows.Count - 3; l++)
                                            {
                                                sbCation.Append("<tr>");
                                                for (int i = 1; i < ColumnCount + 1; i++)
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "D")
                                                    {
                                                        DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                            if (dvDrop.ToTable().Rows.Count > 0)
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                            }
                                                            else
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                                sbCation.Append("</tr>");
                                            }
                                        }
                                        sbCation.Append("</table>");
                                    }
                                    objStr.Append(sbCation);
                                }

                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                        hstInput.Add("@intSecId", common.myInt(sectionId));
                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                //Commented by rakesh because caption tabular template showing last column missiong start
                                //for (int k = 1; k < (column - 5); k++)
                                //Commented by rakesh because caption tabular template showing last column missiong start

                                //Added by rakesh because caption tabular template showing last column missiong start
                                for (int k = 1; k < (column - 4); k++)
                                //Added by rakesh because caption tabular template showing last column missiong start
                                {
                                    if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                        && ColumnCount == 0)
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(" + ");
                                        sbCation.Append("</td>");
                                    }
                                    else
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                        sbCation.Append("</td>");
                                        count++;
                                    }
                                    ColumnCount++;
                                }
                                sbCation.Append("</tr>");

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }

                                    }
                                    sbCation.Append("</tr>");
                                }
                                sbCation.Append("</table>");
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;
                string FieldId = "";
                string sStaticTemplate = "";
                string sEnterBy = "";
                string sVisitDate = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        {
                            BeginList = "<ol>"; EndList = "</ol>";
                        }
                    }
                    if (common.myStr(item["FieldsBold"]) != ""
                        || common.myStr(item["FieldsItalic"]) != ""
                        || common.myStr(item["FieldsUnderline"]) != ""
                        || common.myStr(item["FieldsFontSize"]) != ""
                        || common.myStr(item["FieldsForecolor"]) != ""
                        || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (common.myBool(item["DisplayTitle"]))
                            {
                                // if (EntryType != "M")
                                // {
                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                //else
                                //{
                                //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    objStr.Append(sEnd + "</li>");
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (sStaticTemplate != "<br/><br/>")
                            {
                                objStr.Append(common.myStr(item["FieldName"]));
                            }
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {

                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            //if (EntryType == "M")
                            //{
                            //    objStr.Append("<br/>" + BeginList + sBegin + common.myStr(item["FieldName"]));
                            //}
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        // objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    //if (EntryType == "M")
                                                    //{
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    //}
                                                    //else
                                                    // objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else
                                            //{
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            // }
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else

                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                            sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                            sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                            //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                            //{
                            //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                            //}
                        }


                        // Cmt 25/08/2011
                        //if (objDt.Rows.Count > 0)
                        //{
                        //    if (objStr.ToString() != "")
                        //        objStr.Append(sEnd + "</li>");
                        //}
                    }

                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }
            }
        }

        return objStr.ToString();
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
            }
        }


        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }
        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
            sBegin += " '>";

    }

    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null && iFormId != "")
        {
            if (Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"] == null)
            {
                hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", 1);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }
    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        //DataView dv1 = new DataView(objDs.Tables[1]);
        //dv1.RowFilter = "ValueId='" + objDt.Rows[i]["FieldValue"].ToString() + "'";
        //DataTable dt1 = dv1.ToTable();
        //if (dt1.Rows[0]["MainText"].ToString().Trim() != "")
        //{
        //    if (i == 0)
        //        objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //    else
        //    {
        //        if (FType != "C")
        //            objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        else
        //        {
        //            if (i == 0)
        //                objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //            else if (i + 1 == objDs.Tables[2].Rows.Count)
        //                objStr.Append(" and " + dt1.Rows[i]["MainText"].ToString() + ".");
        //            else
        //                objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        }
        //    }
        //}
        //else
        //{
        if (i == 0)
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        else
        {
            if (FType != "C")
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            else
            {
                if (i == 0)
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                else
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
        }
        //}
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        int RegId = common.myInt(Session["RegistrationID"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(Session["encounterid"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, "0");
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + Fonts + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]), "", "", TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                            "",
                            "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        {
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
        }
        else
        {
            if (sEntryType == "S")
            {
                hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            }
            else
            {
                hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            }
        }

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

        //hstInput.Add("@intEREncounterId", Request.QueryString["EREncounterId"] == null ? Session["EREncounterId"].ToString() : Request.QueryString["EREncounterId"].ToString());

        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);

        //1234
        //if (txtFromDate.SelectedDate.Value != null && txtToDate.SelectedDate.Value != null)
        //{
        //    dv.RowFilter = "EntryDate>='" + Convert.ToDateTime(common.myDate(txtFromDate.SelectedDate.Value)).ToString("yyyy/MM/dd 00:00") +
        //        "' AND EntryDate<='" + Convert.ToDateTime(common.myDate(txtToDate.SelectedDate.Value)).ToString("yyyy/MM/dd 23:59")+"'";
        //}
        dv.Sort = "RecordId DESC";
        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;
        //dtEntry = dv.ToTable(true, "EntryDate");

        //string sEntryDate = "";

        for (int it = 0; it < dtEntry.Rows.Count; it++)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DataTable dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                DataTable dtFieldName = dv1.ToTable();

                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);

                    // dv2.RowFilter = "EntryDate='" + dtEntry.Rows[it]["EntryDate"].ToString() + "'";


                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]);
                    dtFieldValue = dv2.ToTable();
                }

                DataSet dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {
                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = "", sEnd = "";

                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                                item["SectionId"].ToString(), common.myStr(item["EntryType"]), common.myInt(dtEntry.Rows[it]["RecordId"]));
                            str += " ";
                            if (sEntryType == "M" && str.Trim() != "")
                            {
                                str += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + common.myStr(dtFieldValue.Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + ")</span>";
                            }
                            //}
                            if (iPrevId == common.myInt(item["TemplateId"]))
                            {
                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        //objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (t2 == 0)
                                {
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(item["SectionsListStyle"]) == "1")
                                        {
                                            BeginList3 = "<ul>"; EndList3 = "</ul>";
                                        }
                                        else if (common.myStr(item["SectionsListStyle"]) == "2")
                                        {
                                            BeginList3 = "<ol>"; EndList3 = "</ol>";
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                    BeginList3 = "";
                                }
                                else
                                {
                                    if (common.myBool(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                else
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(str);
                                    }
                                }
                            }
                            else
                            {

                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["TemplateListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (common.myStr(item["TemplateListStyle"]) == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["TemplateBold"]) != ""
                                    || common.myStr(item["TemplateItalic"]) != ""
                                    || common.myStr(item["TemplateUnderline"]) != ""
                                    || common.myStr(item["TemplateFontSize"]) != ""
                                    || common.myStr(item["TemplateForecolor"]) != ""
                                    || common.myStr(item["TemplateListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                    BeginList = "";
                                }
                                else
                                {
                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        // objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (common.myStr(item["TemplateListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                objStrTmp.Append(EndList);
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList2 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList2 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                    BeginList2 = "";
                                }
                                else
                                {
                                    if (common.myBool(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                        }
                                    }
                                }
                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["SectionsListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }

                                objStrTmp.Append(str);
                            }
                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                            iPrevId = common.myInt(item["TemplateId"]);
                        }
                    }
                }
            }
        }

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }

        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
        {
            sb.Append(objStrTmp.ToString());
        }
    }


    protected void lnkLabHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]);
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    private void BindItemWithItemDetail(DataTable dtItem, DataTable dtItemDetail)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["Item"];
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["ItemDetail"];
        DataTable dtNew = new DataTable();
        DataSet ds = new DataSet();

        DataRow DR;
        DataRow DR1;
        decimal dQty = 0;
        int countRow = 0;

        try
        {
            for (int counter = 0; counter < dtItem.Rows.Count; counter++)
            {
                DataTable dtTemp = new DataTable();
                dtTemp = (DataTable)ViewState["Item"];
                int exists = 0;
                for (int counter1 = 0; counter1 < dtTemp.Rows.Count; counter1++)
                {
                    if (common.myInt(dtItem.Rows[counter]["ItemId"].ToString()) == common.myInt(dtTemp.Rows[counter1]["ItemId"].ToString()))
                    {
                        exists = 1;
                        break;
                        //counter1 = dtTemp.Rows.Count;
                    }
                }
                if (exists == 0)
                {
                    DR = dt.NewRow();

                    if (ddlIndentType.SelectedValue == "")
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
                    DR["NotToPharmacy"] = dtItem.Rows[counter]["NotToPharmacy"];
                    DR["IsInfusion"] = dtItem.Rows[counter]["IsInfusion"];
                    DR["FormulationId"] = dtItem.Rows[counter]["FormulationId"];
                    ///Item Detail
                    //TextBox txtInstructions = new TextBox();
                    //RadComboBox ddlFoodRelation = new RadComboBox();

                    //Item Details start

                    if (dt1 != null)
                    {
                        DR1 = dt1.NewRow();

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

                        DR1["StartDate"] = dtItemDetail.Rows[counter]["StartDate"];
                        DR1["EndDate"] = dtItemDetail.Rows[counter]["EndDate"];

                        DR1["FoodRelationshipId"] = dtItemDetail.Rows[counter]["FoodRelationshipId"];
                        DR1["FoodRelationship"] = dtItemDetail.Rows[counter]["FoodRelationship"];
                        DR1["UnitId"] = 0;// dtItemDetail.Rows[counter]["UnitId"];
                        DR1["UnitName"] = dtItemDetail.Rows[counter]["UnitName"];
                        DR1["CIMSItemId"] = dtItemDetail.Rows[counter]["CIMSItemId"];
                        DR1["CIMSType"] = dtItemDetail.Rows[counter]["CIMSType"];
                        DR1["VIDALItemId"] = dtItemDetail.Rows[counter]["VIDALItemId"];
                        DR1["PrescriptionDetail"] = dtItemDetail.Rows[counter]["PrescriptionDetail"];
                        DR1["Instructions"] = dtItemDetail.Rows[counter]["Instructions"];
                        DR1["ReferanceItemId"] = dtItemDetail.Rows[counter]["ReferanceItemId"];
                        DR1["ReferanceItemName"] = dtItemDetail.Rows[counter]["ReferanceItemName"];
                        DR1["DoseTypeId"] = dtItemDetail.Rows[counter]["DoseTypeId"];
                        DR1["DoseTypeName"] = dtItemDetail.Rows[counter]["DoseTypeName"];

                        DR1["FrequencyId"] = dtItemDetail.Rows[counter]["FrequencyId"];
                        DR1["FrequencyName"] = dtItemDetail.Rows[counter]["FrequencyName"];
                        DR1["IsInfusion"] = true;
                        DR["IsInfusion"] = true;

                        dt1.Rows.Add(DR1);
                        dt1.AcceptChanges();

                        dt1.TableName = "ItemDetail";
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                        dt1.WriteXml(writer);

                        string xmlSchema = writer.ToString();
                        DR["XMLData"] = xmlSchema;
                        DR["PrescriptionDetail"] = emr.GetPrescriptionDetailString(dt1);
                    }
                    //Item Details end

                    dt.Rows.Add(DR);
                    dt.AcceptChanges();
                }

            }
            ViewState["Item"] = dt;
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
            emr = null;
        }

    }

    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Remarks can't be blank !";
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0)
            {
                Hashtable hshOutput = new Hashtable();

                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                          common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]),
                          common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                          common.myStr(btnStopMedication.Text).ToUpper().Equals("STOP") ? 0 : 1, txtStopRemarks.Text,
                          common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                BindGrid(string.Empty, string.Empty);

                ViewState["IndentDetailsId"] = "0";
                if (common.myStr(hshOutput["@chvOutPut"]).ToUpper().Equals("SUCCESSFULL"))
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


    protected void btnAddItem_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        BaseC.EMR objEMR2 = new BaseC.EMR(sConString);
        DataView dvDrugAllergy = new DataView();
        DataTable tblDrugAllergy = new DataTable();
        string Instructions = string.Empty;
        try
        {

            if (common.myBool(ViewState["IsReasonManditory"]).Equals(true) && common.myStr(ddlReason.SelectedValue).Equals(""))
            {
                lblMessage.Text = "Please select reason";
                Alert.ShowAjaxMsg("Please select reason", Page);
                return;
            }


            if (common.myStr(ViewState["IsItemExcludedItemAndCompanyWise"]).ToUpper().Equals("Y"))
            {
                if (common.myBool(ViewState["IsAllowToAddBlockedItem"]) == false && common.myInt(ViewState["BlockItemId"]) > 0)
                {
                    if (common.myInt(ViewState["BlockItemId"]) > 0)
                    {
                        Alert.ShowAjaxMsg("Not authorized to Prescribe this Item. Selected Item is blocked for this company.", this.Page);
                        return;
                    }
                }
            }
            else
            {
                if (common.myBool(hdnRestrictedItemForPanel.Value) && common.myInt(ddlIndentType.SelectedValue).Equals(3) && common.myBool(Session["IsPanelPatient"]))
                {
                    //lblMessage.Text = "This item is restricted  for panel patients";
                    lblMessage.Text = "Item category restricted for Indent for this Payor.";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    return;
                }
            }




            foreach (GridViewRow dataItem in gvItemDetails.Rows)
            {
                RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");
                TextBox txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

                RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
                TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");

                Instructions = common.myStr(txtInstructions.Text).Trim();

                if (common.myStr(ddlDoseType.SelectedItem.Text).ToUpper().Contains("PRN") && common.myStr(txtInstructions.Text).Trim().Equals(string.Empty))
                {
                    lblMessage.Text = "Instructions are manditory when Order Type is PRN";
                    Alert.ShowAjaxMsg("Instructions are manditory when Order Type is PRN", Page);
                    return;
                }

                if ((CalculateDays(common.myStr(txtDuration.Text), common.myStr(ddlPeriodType.SelectedValue)) > common.myInt(ViewState["WardDurationInDaysForConsumableOrder"])) && common.myInt(ddlIndentType.SelectedValue).Equals(3) && common.myBool(Session["IsPanelPatient"]))
                {
                    //lblMessage.Text = "Duration cannot be more than "+ common.myStr(ViewState["WardDurationInDaysForConsumableOrder"]) + " days";
                    lblMessage.Text = "Only " + common.myStr(ViewState["WardDurationInDaysForConsumableOrder"]) + " days of medicines can be indented for this Payor. Kindly change Duration.";
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    return;
                }
            }




            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

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
                //  return; //yogesh 26/08/2022
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
                    //   if (tblDrugAllergy.Rows.Count > 0)
                    //    {
                    //    for (int j = 0; j <= tblDrugAllergy.Rows.Count - 1; j++)
                    //    {
                    //      if (!(tblDrugAllergy.Rows[j]["ItemType"]).Equals("O"))
                    //       {
                    if (common.myInt(hdnItemId.Value) > 0)
                    {
                        dvDrugAllergy.RowFilter = "ISNULL(AllergyID,0)=" + common.myInt(hdnItemId.Value);
                        if (dvDrugAllergy.Count == 0)
                        {
                            dvDrugAllergy = tblDrugAllergy.Copy().DefaultView;
                            if (common.myInt(hdnGenericId.Value) > 0)
                            {
                                dvDrugAllergy.RowFilter = "ISNULL(Generic_Id,0)=" + common.myInt(hdnGenericId.Value);
                            }

                        }
                    }
                    else
                    {
                        dvDrugAllergy.RowFilter = "ISNULL(Generic_Id,0)=" + common.myInt(hdnGenericId.Value);
                        //         }
                        //     }
                        //  }
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

            if (!isSavedItem())
            {
                return;
            }

            bool IsInteraction = false;

            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                DataTable tblItem = new DataTable();
                tblItem = (DataTable)ViewState["GridDataItem"];

                DataView DVItem = tblItem.Copy().DefaultView;

                if (common.myInt(hdnItemId.Value) > 0)
                {
                    DVItem.RowFilter = "ItemId = " + common.myInt(hdnItemId.Value);
                }
                else
                {
                    DVItem.RowFilter = "GenericId = " + common.myInt(hdnGenericId.Value);
                }

                if (DVItem.ToTable().Rows.Count == 0)
                {
                    string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";

                    string strXML = getInterationXML(strPrescribing);

                    if (strXML != "")
                    {
                        string outputValues = objCIMS.getFastTrack5Output(strXML);

                        if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
                        {
                            string strFindCIMSItemId = "<PRODUCT REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=\"\"></PRODUCT>";

                            if (!outputValues.ToUpper().Contains(strFindCIMSItemId.ToUpper()))
                            {
                                IsInteraction = true;

                                ViewState["NewPrescribing"] = strXML;

                                dvInteraction.Visible = true;
                            }
                        }
                    }
                }
            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                addItem();

                return;
            }

            if (!IsInteraction)
            {
                addItem();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
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
                // dvAllergy.RowFilter = "AllergyType='Drug'";
                dvAllergy.RowFilter = "AllergyType='Drug' AND ItemType='P'";
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

    protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
    {
        try
        {
            addItem();
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
    protected void ChkReqestFromOtherWard_OnCheckedChanged(Object sender, EventArgs args)
    {
        try
        {
            //if (ChkReqestFromOtherWard.Checked)
            //{
            //    bindGetPreviousStores();
            //}
            //else
            //{
            //    bindDdlstore();
            //}

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

    private void bindPatientPreviousWards()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        RadComboBoxItem item;
        ddlReqestFromOtherWard.Items.Clear();
        ddlReqestFromOtherWard.Text = string.Empty;

        try
        {
            string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();

            ds = objEMR.GetPatientPreviousWards(common.myInt(sEncId));
            // ds = objEMR.GetPatientPreviousWards(common.myInt(4525));

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            item = new RadComboBoxItem();
                            item.Text = common.myStr(dr["WardName"]);
                            item.Value = common.myInt(dr["WardId"]).ToString();
                            //item.Attributes.Add("WardId", common.myStr(dr["WardId"]));
                            //item.Attributes.Add("BedId", common.myStr(dr["BedId"]));
                            ddlReqestFromOtherWard.Items.Add(item);
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


    private void bindDdlstore()
    {
        DataSet dsStoreMasterList = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //RadComboBoxItem item;
        //ddlStore.Items.Clear();
        //ddlStore.Text = string.Empty;

        try
        {
            string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();

            dsStoreMasterList = objEMR.getStoreMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                         common.myInt(Session["UserID"]), common.myInt(sEncId));

            //if (dsStoreMasterList.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in dsStoreMasterList.Tables[0].Rows)
            //    {
            //        item = new RadComboBoxItem();
            //        item.Text = common.myStr(dr["DepartmentName"]);
            //        item.Value = common.myInt(dr["StoreId"]).ToString();
            //        ddlStore.Items.Add
            //            (item);
            //    }
            //}

            ddlStore.DataSource = dsStoreMasterList.Tables[0];
            ddlStore.DataTextField = "DepartmentName";
            ddlStore.DataValueField = "StoreId";
            ddlStore.DataBind();

            if (dsStoreMasterList.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(Session["OPIP"]).Equals("I")
                    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("DO") && common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"))
                {
                    if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                            common.myInt(Session["FacilityId"]), "DefaultConsumableIndentStoreId", sConString))));
                    }
                    else
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                    common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString))));
                    }
                    ViewState["Consumable"] = false;
                }
                if (common.myStr(Session["OPIP"]).Equals("I")
                    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") && common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"))
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                    common.myInt(Session["FacilityId"]), "DefaultConsumableIndentStoreId", sConString))));

                    ViewState["Consumable"] = true;
                }
                if (common.myStr(Session["OPIP"]).Equals("O")
                   && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") && common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"))
                {
                    if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                common.myInt(Session["FacilityId"]), "DefaultConsumableIndentStoreId", sConString))));
                    }
                    else
                    {
                        ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString))));
                    }
                    ViewState["Consumable"] = true;
                }
                else if (common.myStr(Session["OPIP"]).Equals("O") && Request.QueryString["DRUGORDERCODE"] == null)
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "DefaultOPIndentStoreId", sConString))));
                    ViewState["Consumable"] = false;
                }
                else if (common.myStr(Session["OPIP"]).Equals("I")
                    && common.myStr(Request.QueryString["DRUGORDERCODE"]).Equals("CO") && common.myStr(Request.QueryString["LOCATION"]).Equals("OT"))
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                         common.myInt(Session["FacilityId"]), "DefaultOTIndentStoreId", sConString))));
                    ViewState["Consumable"] = true;
                }
                else if (common.myStr(Session["OPIP"]).Equals("E"))
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                         common.myInt(Session["FacilityId"]), "DefaultERIndentStoreId", sConString))));

                    ViewState["Consumable"] = false;
                }
                hdnStoreId.Value = ddlStore.SelectedValue;
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
            dsStoreMasterList.Dispose();
            objEMR = null;
        }
    }

    protected void btnMedicationOverride_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnIsOverride.Value) > 0)
            {
                addItem();
                hdnItemId.Value = "0";
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;
                hdnGenericId.Value = "0";
                ddlGeneric.Text = string.Empty;
                ddlGeneric.SelectedValue = null;
                txtCustomMedication.Text = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #region Transaction password validation
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
    #endregion


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
        if (common.myInt(dt.Rows.Count) > 0)
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

    private void PrintPrescription(int intPrescriptionId)
    {
        try
        {
            clsPrinter _printer = null;
            BaseC.User valUser = new BaseC.User(sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();
            if (common.myStr(ViewState["RemoteDirectPRintingURL"]) == "")
            {
                RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(Session["EncounterId"]) + "&PId=" + intPrescriptionId;

                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            }
            else
            {
                string[] hostName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                _printer = new clsPrinter(sConString, "OPIndent", common.myInt(0), hostName[0].ToUpper().Trim());
            }
            {
                string sUserName = string.Empty;
                sUserName = valUser.GetEmpName(Convert.ToInt32(Session["UserID"]));

                str.Append("inyHospitalLocationId/");
                str.Append(Session["HospitalLocationID"].ToString());
                str.Append("!");
                str.Append("intFacilityId/");
                str.Append(Session["FacilityID"].ToString());
                str.Append("!");
                str.Append("intEncounterId/");
                str.Append(common.myInt(sEncId));
                str.Append("!");
                str.Append("intIndentId/");
                str.Append(intPrescriptionId);
                str.Append("!");
                str.Append("UserName/");
                str.Append(sUserName);
                str.Append("!");
                str.Append("UHID/");
                str.Append((string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));

                if (common.myStr(ViewState["RemoteDirectPRintingURL"]) != "")
                {
                    string StrRemote = ViewState["RemoteDirectPRintingURL"] + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/" + _printer.ReportName + "$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + _printer.PageMargin + "$" + _printer.PrinterName;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + StrRemote + "';", true);
                }
                return;
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    #endregion


    private void bindUniqueCat()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet tbl = new DataSet();
        try
        {

            tbl = objPharmacy.GetPhrItemMasterMisc(0, "UC", 1, common.myInt(Session["FacilityId"]));
            ddlUniqueCategory.DataSource = tbl;
            ddlUniqueCategory.DataTextField = "Name";
            ddlUniqueCategory.DataValueField = "Id";
            ddlUniqueCategory.DataBind();
            ddlUniqueCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlUniqueCategory.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objPharmacy = null;
        }
    }

    public void getSubtituteItems(RadComboBoxItemsRequestedEventArgs e, int itemid)
    {
        BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);
        DataSet ds = objphr.GetGenericOfItem(common.myInt(itemid));
        int GenericId = common.myInt(ds.Tables[0].Rows[0]["GenericId"]);
        Boolean Monopoly = common.myBool(ds.Tables[0].Rows[0]["Monopoly"]);

        if (Monopoly.Equals(false))
        {
            ddlBrand.Items.Clear();
            //Change monopoly Item add

            if (GenericId == 0)
            {
                Alert.ShowAjaxMsg("This Item Is Not Allowed And No Subtitute For This Item !", Page);
                ddlBrand.Text = "";
                hdnItemId.Value = "0";
                return;
            }


            if (common.myInt(hdnStoreId.Value) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Store not selected !";

                return;
            }

            DataTable data = GetBrandData(e.Text, GenericId);


            if (data.Rows.Count > 0)
            {
                Alert.ShowAjaxMsg("This Item Is Not Allowed ! Please Select Subtitute Item ", Page);
                DataView DV = data.DefaultView;
                DV.RowFilter = "LowCost=true";
                data = DV.ToTable();
                data.AcceptChanges();
            }
            else
            {
                Alert.ShowAjaxMsg("This Item Is Not Allowed And No Subtitute For This Item !", Page);
                ddlBrand.Text = "";
                hdnItemId.Value = "0";
                return;
            }


            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);
                //  item.Attributes.Add("ItemSubCategoryShortName", data.Rows[i]["ItemSubCategoryShortName"].ToString());
                item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));
                item.Attributes.Add("CIMSItemId", common.myStr(data.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(data.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myStr(data.Rows[i]["VIDALItemId"]));
                item.Attributes.Add("RestrictedItemForPanel", common.myStr(data.Rows[i]["RestrictedItemForPanel"]));
                item.Attributes.Add("ItemFlagName", common.myStr(data.Rows[i]["ItemFlagName"]));
                item.Attributes.Add("ItemFlagCode", common.myStr(data.Rows[i]["ItemFlagCode"]));
                item.Attributes.Add("ConversionFactor2", common.myStr(data.Rows[i]["ConversionFactor2"]));
                item.Attributes.Add("LowCost", common.myStr(data.Rows[i]["LowCost"]));

                if (!common.myBool(data.Rows[i]["LowCost"]))
                {
                    item.BackColor = System.Drawing.Color.LightCoral;
                }
                this.ddlBrand.Items.Add(item);
                item.DataBind();
            }
            ddlBrand.Text = "";
            ddlBrand.OpenDropDownOnLoad = true;
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
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


}
