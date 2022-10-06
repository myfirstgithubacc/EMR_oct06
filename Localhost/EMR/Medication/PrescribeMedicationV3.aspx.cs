using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Medication_PrescribeMedicationV3 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    int count = 0;
    clsCIMS objCIMS = new clsCIMS();

    private string DellBoomi = ConfigurationManager.AppSettings["AFG_DellBoomi"].ToString();


    private enum enumColumns : byte
    {
        Sno = 0,
        BrandName = 1,
        StartDate = 2,
        EndDate = 3,
        IndentType = 4,
        TotalQty = 5,
        PrescriptionDetail = 6,
        MonographCIMS = 7,
        InteractionCIMS = 8,
        Edit = 9,
        Delete = 10

        //BrandDetailsCIMS = 7,
        //MonographCIMS = 8,
        //InteractionCIMS = 9,
        //DHInteractionCIMS = 10,
        //DAInteractionCIMS = 11,
        //BrandDetailsVIDAL = 12,
        //MonographVIDAL = 13,
        //InteractionVIDAL = 14,
        //DHInteractionVIDAL = 15,
        //DAInteractionVIDAL = 16,
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
        DHInteractionCIMS = 12,
        DAInteractionCIMS = 13,
        BrandDetailsVIDAL = 14,
        MonographVIDAL = 15,
        InteractionVIDAL = 16,
        DHInteractionVIDAL = 17,
        DAInteractionVIDAL = 18,
        StopRemarks = 19,
        Stop = 20
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {

        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["PT"]).Equals("IPEMR"))
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP") || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
        {
            // this.MasterPageFile = "~/Include/Master/BlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMasterPopup.master";
        }

        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            //Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMasterPopup.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (common.myInt(Session["EncounterId"]).Equals(0))
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
        }
        if (Request.QueryString["Op"] == "PreAdmAdvice")
        {
            ViewState["EncId"] = Session["EncounterId"];
            ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
            ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
            ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
            ViewState["EncId"] = common.myStr(Session["EncounterId"]);
        }
        // Commint by Kuldeep Kumar after discussed with Mr. Manmohan.
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //int cheifcomplaint = common.myInt(dl.ExecuteScalar(CommandType.Text, "uspCheckPatientProblem @encounterID=" + common.myInt(Session["EncounterId"])));
        //if (cheifcomplaint == 0)
        //{
        //    Response.Redirect("/EMR/Dashboard/PatientDashboardForDoctor.aspx");
        //}

        if (common.myStr(Session["OPIP"]) == "O")
        {
            if (PatientandClinicianValidateion() != string.Empty)
            {
                dverx.Visible = true;
                return;
            }
        }

        if (common.myStr(Session["OPIP"]).Equals("I"))
        {
            btnCopyLastPrescription.Visible = false;
        }

        if (!IsPostBack)
        {
            objCIMS = new clsCIMS();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            DataSet dsInterface = new DataSet();
            DataSet dsENC = new DataSet();
            try
            {
                ViewState["CopyLastPresc"] = false;
                ViewState["GridDataItem"] = null;
                ViewState["Item"] = null;
                ViewState["ItemDetail"] = null;
                ViewState["StopItemDetail"] = null;
                ViewState["Edit"] = null;
                ViewState["UnAppPrescriptionId"] = null;

                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

                ViewState["RouteMandatory"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(Session["FacilityId"]), "RouteMandatory", sConString);


                if (common.myStr(ViewState["RouteMandatory"]).Equals("Y"))
                {
                    spnRoute.Visible = true;
                }
                else
                {
                    spnRoute.Visible = false;
                }

                ViewState["AllFormulationUnits"] = objPharmacy.getAllFormulationUnits();
                ViewState["AllFormulationRoutes"] = objPharmacy.getAllFormulationRoutes();

                dvConfirmStop.Visible = false;

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

                ViewState["PrescriptionAllowForItemsNotInStock"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                          common.myInt(Session["FacilityId"]), "PrescriptionAllowForItemsNotInStock", sConString);

                //setTemplateData();

                if (common.myStr(Session["OPIP"]).Equals("I"))
                {
                    rdoSearchMedication.SelectedValue = "C";
                    rdoSearchMedication_OnSelectedIndexChanged(null, null);
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
                    //lblDrugDetail.Text = "Consumable Indent";
                    //lblPrescription.Text = "Selected Indent";
                    //lnkCurrentMedication.Visible = false;
                    // ddlFormulation.Enabled = false;

                    //ddlStrength.Enabled = false;
                    //txtStrengthValue.Enabled = false;

                    ddlRoute.Items.Insert(0, new RadComboBoxItem(string.Empty));
                    ddlRoute.Items[0].Value = "0";
                    //ddlRoute.SelectedValue = "0";
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
                    ddlRoute.Enabled = false;
                }

                bindControl();
                SetSaveButtonText();

                ShowPatientDetails();

                //PostBackOptions optionsSubmit = new PostBackOptions(btnSave);
                //btnSave.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";
                //btnSave.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);
                //Session["DrugReqSavecheck"] = 0;


                #region Interface

                chkShowDetails.Visible = false;
                btnMonographViewOnItemBased.Visible = false;
                btnInteractionViewOnItemBased.Visible = false;

                setPatientInfo();

                dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForEMRDrugOrder);
                if (Request.IsLocal)
                {

                    // Session["IsVIDALInterfaceActive"] = true;

                    chkShowDetails.Visible = true;
                    chkShowDetails.Text = "Show VIDAL Details";
                }
                if (dsInterface.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                    {
                        Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                        Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                        Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                        Session["CIMSDatabaseName"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabaseName"]);

                        chkShowDetails.Visible = true;
                        chkShowDetails.Text = "Show CIMS Details";
                    }
                    //else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                    //{
                    //    Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);

                    //    chkShowDetails.Visible = true;
                    //    chkShowDetails.Text = "Show VIDAL Details";
                    //}

                    getLegnedColor();
                }

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    string CIMSDatabasePath = string.Empty;
                    if (dsInterface.Tables[0].Rows.Count > 0)
                    {
                        CIMSDatabasePath = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    }
                    string CIMSDatabaseName = common.myStr(Session["CIMSDatabaseName"]);

                    if (common.myLen(CIMSDatabaseName).Equals(0))
                    {
                        CIMSDatabaseName = "FastTrackData.mrc";
                    }

                    if (!File.Exists(CIMSDatabasePath + CIMSDatabaseName))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                        lblMessage.Text = "CIMS database not available !";
                        //Alert.ShowAjaxMsg("CIMS database not available !", this);
                    }
                }
                //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //{
                //    try
                //    {
                //        VSDocumentService.documentServiceClient objDocumentService;

                //        objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");

                //        WebClient client = new WebClient();
                //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sVidalConString + "DocumentService");
                //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //        if (response.StatusCode != HttpStatusCode.OK)
                //        {
                //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //            lblMessage.Text = "VIDAL web-services not running now !";

                //            //Alert.ShowAjaxMsg(lblMessage.Text, this);
                //        }
                //    }
                //    catch
                //    {
                //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //        lblMessage.Text = "VIDAL web-services not running now !";

                //        //Alert.ShowAjaxMsg(lblMessage.Text, this);
                //    }
                //}

                setDiagnosis();
                setAllergiesWithInterfaceCode();

                #endregion

                btnCopyLastPrescription.Enabled = true;

                BindGrid(string.Empty, string.Empty);
                BindBlankItemGrid();
                bindUnApprovedPrescriptions();
                //BindBlankItemDetailGrid();
                BindPatientProvisionalDiagnosis();
                //ddlGeneric.Focus();
                txtFavouriteItemName.Focus();
                getCurrentICDCodes();

                BindICDPanel();
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnSave.Visible = false;
                    btnCopyLastPrescription.Visible = false;
                }
                if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP") || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
                {
                    btnclose.Visible = true;
                }

                //lnkLabHistory.Visible = false;
                txtStartDate.SelectedDate = DateTime.Now;
                txtStartDate.MinDate = DateTime.Now;
                txtEndDate.MinDate = DateTime.Now;
                //   txtEndDate.SelectedDate = DateTime.Now;
                GetBrandFavouriteData(string.Empty);
                BindPatientHiddenDetails();

                //txtDose.Attributes.Add("onblur", "javascript:CalcChange()");
                //txtDuration.Attributes.Add("onblur", "javascript:CalcChange()");
                CheckBox exkx = this.FindControl("ChkAutoSuggestion") as CheckBox;
                SetPermission();
                dvConfirmAlreadyExistOptions.Visible = false;
                if (exkx != null)
                {
                    if (common.myBool(Cache["EnableMSuggession_" + common.myInt(Session["EncounterId"])]) == true)
                    {
                        exkx.Checked = true;
                    }
                    else
                        exkx.Checked = false;

                    if (common.myStr(txtICDCode.Text) != "" && exkx.Checked == true)
                    {
                        ShowAIMedicineWindow();
                    }
                }
                //if (txtICDCode.Text!="")
                //    ShowAIMedicineWindow();
                dsENC = objEMR.getEncounterDoctor(common.myInt(Session["EncounterId"]));

                if (dsENC.Tables.Count > 0)
                {
                    if (dsENC.Tables[0].Rows.Count > 0)
                    {
                        lblPrescribedByValue.Text = common.myStr(dsENC.Tables[0].Rows[0]["DoctorName"]);
                    }
                }
                //  lblPrescribedByValue.Text = common.myStr(objSurgery.GetDoctorName(common.myInt(Session["DoctorID"])));

                //added by bhakti
                ViewState["isRequireIPBillOfflineMarking"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isRequireIPBillOfflineMarking", sConString);

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
                dsInterface.Dispose();
                dsENC.Dispose();
            }


        }

        // btnPrint.Visible = false;

    }

    private void ShowAIMedicineWindow()
    {
        RadWindow1.NavigateUrl = "AIMedicationList.aspx?EncId=" + common.myInt(Session["EncounterId"]);
        RadWindow1.Height = 750;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientCloseAIMedicineWindow";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    private void setUnitAndRouteBasedOnFormulation(int FormulationId)
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        try
        {
            if (FormulationId > 0)
            {
                if (ViewState["AllFormulationUnits"] != null)
                {
                    ds = (DataSet)ViewState["AllFormulationUnits"];
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DV = ds.Tables[0].DefaultView;
                                DV.RowFilter = "FormulationId=" + FormulationId;

                                if (DV.ToTable().Rows.Count > 0)
                                {
                                    ddlUnit.Items.Clear();

                                    ddlUnit.DataSource = DV.ToTable();
                                    ddlUnit.DataValueField = "UnitId";
                                    ddlUnit.DataTextField = "UnitName";
                                    ddlUnit.DataBind();

                                    ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
                                    ddlUnit.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }

                ds = new DataSet();
                DV = new DataView();

                if (ViewState["AllFormulationRoutes"] != null)
                {
                    ds = (DataSet)ViewState["AllFormulationRoutes"];
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DV = ds.Tables[0].DefaultView;
                                DV.RowFilter = "FormulationId=" + FormulationId;

                                if (DV.ToTable().Rows.Count > 0)
                                {
                                    ddlRoute.Items.Clear();

                                    ddlRoute.DataSource = DV.ToTable();
                                    ddlRoute.DataValueField = "RouteId";
                                    ddlRoute.DataTextField = "RouteName";
                                    ddlRoute.DataBind();

                                    ddlRoute.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                                    ddlRoute.SelectedIndex = 0;
                                }
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
            ds.Dispose();
            DV.Dispose();
        }
    }
    protected string PatientandClinicianValidateion()
    {
        System.Text.StringBuilder strb = new StringBuilder();
        string ValidationFail = string.Empty;
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = dl.FillDataSet(CommandType.Text, "Exec uspValidateErxPatientXML " + common.myStr(Session["RegistrationId"]) + "," + common.myStr(Session["UserId"]));

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    ValidationFail = ds.Tables[0].Rows[0]["ValidationStatus"].ToString();
                    if (ValidationFail.Equals(""))
                    {
                        ValidationFail= string.Empty;
                        return ValidationFail;
                    }
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
            }
            dvInfo.InnerHtml = strb.ToString();
            ViewState["IsInsuranceCompany"] = ds.Tables[0].Rows[0]["IsInsuranceCompany"];
            if (common.myBool(ViewState["IsInsuranceCompany"]) == false)
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
            dl = null;
        }

        return ValidationFail;
    }
    protected string PatientandMedicineValidateion(int indentid)
    {
        System.Text.StringBuilder strb = new StringBuilder();
        string ValidationFail = string.Empty;
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = dl.FillDataSet(CommandType.Text, "Exec UspCheckGenerateeOPRxXml " + common.myStr(Session["FacilityId"]) + "," + common.myStr(indentid));
            if (ds.Tables.Count > 0)
            {
                if (common.myInt(ds.Tables[0].Rows[0][0]) == 0)
                {
                    ValidationFail = "Failed";
                }
                else
                {
                    ValidationFail = string.Empty;
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
            dl = null;
        }

        return ValidationFail;
    }
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }
    void BindPatientHiddenDetails()
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
    private void bindControl()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        BaseC.clsPharmacy objMaster = new BaseC.clsPharmacy(sConString);
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsBb status = new BaseC.clsBb(sConString);

        DataSet dsRoute = new DataSet();
        DataSet dsENC = new DataSet();
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataView dv = new DataView();

        try
        {
            //----------------------- Checked By Sushil Saini------------------------
            string DocType = "CASH SALE";
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                DocType = "IPD Issue";
            }
            //------------------- Done By Sushil Saini -------------------------
            /*************** Store ***************/
            ds = objPharmacy.GetStoreToChangefromWard(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["GroupID"]), common.myInt(Session["FacilityId"]), DocType, 0);

            RadComboBoxItem itemStore;
            if (common.myInt(ds.Tables.Count) > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    itemStore = new RadComboBoxItem();
                    itemStore.Text = common.myStr(dr["DepartmentName"]);
                    itemStore.Value = common.myInt(dr["StoreId"]).ToString();
                    itemStore.Attributes.Add("OrderEntryStore", common.myStr(dr["OrderEntryStore"]));

                    ddlStore.Items.Add(itemStore);
                    ddlStore.DataBind();
                }
            }

            //ddlStore.DataSource = ds.Tables[0];
            //ddlStore.DataTextField = "DepartmentName";
            //ddlStore.DataValueField = "StoreId";
            //ddlStore.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "I" && Request.QueryString["DRUGORDERCODE"] == null)
                {
                    //ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //                                    common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);

                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                        common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString))));

                    ViewState["Consumable"] = false;
                }
                if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "I"
                    && Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "WARD")
                {
                    //ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //                                    common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);

                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                        common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString))));

                    ViewState["Consumable"] = true;
                }
                else if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "O" && Request.QueryString["DRUGORDERCODE"] == null)
                {
                    //ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //                                    common.myInt(Session["FacilityId"]), "DefaultOPIndentStoreId", sConString);

                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                          common.myInt(Session["FacilityId"]), "DefaultOPIndentStoreId", sConString))));

                    ViewState["Consumable"] = false;
                }
                else if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "I"
                    && Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "OT")
                {
                    //ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //                                    common.myInt(Session["FacilityId"]), "DefaultOTIndentStoreId", sConString);

                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                        common.myInt(Session["FacilityId"]), "DefaultOTIndentStoreId", sConString))));

                    ViewState["Consumable"] = true;
                }
                else if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "E")
                {
                    //ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                    //                                    common.myInt(Session["FacilityId"]), "DefaultERIndentStoreId", sConString);

                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                                                                        common.myInt(Session["FacilityId"]), "DefaultERIndentStoreId", sConString))));

                    ViewState["Consumable"] = false;
                }
                hdnStoreId.Value = ddlStore.SelectedValue;
            }

            /*************** Advising Doctor ***************/
            tbl = objlis.getDoctorList(0, string.Empty, common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);

            ddlAdvisingDoctor.DataSource = tbl;
            ddlAdvisingDoctor.DataTextField = "DoctorName";
            ddlAdvisingDoctor.DataValueField = "DoctorId";
            ddlAdvisingDoctor.DataBind();

            ddlAdvisingDoctor.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            dsENC = objEMR.getEncounterDoctor(common.myInt(Session["EncounterId"]));

            if (dsENC.Tables[0].Rows.Count > 0)
            {
                ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myInt(dsENC.Tables[0].Rows[0]["DoctorId"]).ToString()));
            }
            else
            {
                ddlAdvisingDoctor.SelectedIndex = 0;
            }

            /*************** Doctor Insructions ***************/
            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId == 0)
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0) && ddlAdvisingDoctor.SelectedItem != null && common.myInt(ddlAdvisingDoctor.SelectedItem.Value) > 0)
            {
                DoctorId = common.myInt(ddlAdvisingDoctor.SelectedItem.Value);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }
            tbl = objEMR.GetEMRDoctorPrescriptionInstructions(DoctorId).Tables[0];

            ddlInstructions.DataSource = tbl;
            ddlInstructions.DataTextField = "Instructions";
            ddlInstructions.DataValueField = "Id";
            ddlInstructions.DataBind();
            ddlInstructions.Items.Insert(0, new RadComboBoxItem("", ""));

            /*************** Formulation ***************/
            ds = new DataSet();
            ds = objPharmacy.GetFormulationMaster(0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["FormulationName"]);
                    item.Value = common.myStr(common.myInt(dr["FormulationId"]));
                    item.Attributes.Add("DefaultRouteId", common.myStr(dr["DefaultRouteId"]));
                    item.DataBind();
                    ddlFormulation.Items.Add(item);
                }
                ddlFormulation.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlFormulation.SelectedIndex = 0;
            }

            /*************** Route ***************/
            if (Request.QueryString["DRUGORDERCODE"] == null)
            {
                OpPrescription OpPre = new OpPrescription(sConString);
                dsRoute = new DataSet();
                dsRoute = (DataSet)OpPre.dsGetRouteDetails();

                ddlRoute.DataSource = dsRoute;
                ddlRoute.DataValueField = "Id";
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataBind();

                ddlRoute.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlRoute.SelectedIndex = 0;

                //dv = new DataView(dsRoute.Tables[0]);
                //dv.RowFilter = "IsDefault=1";

                //if (dv.ToTable().Rows.Count > 0)
                //{
                //    ddlRoute.SelectedValue = dv.ToTable().Rows[0]["Id"].ToString();
                //}
            }

            /*************** Strength ***************/
            //ds = new DataSet();
            //ds = objPharmacy.GetItemStrength(0, 0, common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));

            //ddlStrength.DataSource = ds.Tables[0];
            //ddlStrength.DataValueField = "StrengthId";
            //ddlStrength.DataTextField = "Strength";
            //ddlStrength.DataBind();

            //ddlStrength.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            //ddlStrength.SelectedIndex = 0;

            /*************** Frequency ***************/
            ds = new DataSet();
            ds = objPharmacy.getFrequencyMaster();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["Description"]);
                item.Value = common.myStr(common.myInt(dr["Id"]));
                item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));
                item.DataBind();
                ddlFrequencyId.Items.Add(item);
            }
            ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFrequencyId.SelectedIndex = 0;

            ds = objEMR.getEMRPrescriptionRemarks(0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, 0);
            ViewState["InstructionData"] = ds.Tables[0];

            ds = new DataSet();
            ds = objPharmacy.GetFood(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlFoodRelation.DataSource = ds.Tables[0];
            ddlFoodRelation.DataValueField = "Id";
            ddlFoodRelation.DataTextField = "FoodName";
            ddlFoodRelation.DataBind();
            ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFoodRelation.SelectedIndex = 0;


            ds = new DataSet();
            ds = status.GetStatusMaster("DoseType");

            ddlDoseType.DataSource = ds.Tables[0];
            ddlDoseType.DataValueField = "StatusId";
            ddlDoseType.DataTextField = "Status";
            ddlDoseType.DataBind();
            ddlDoseType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlDoseType.SelectedIndex = 0;

            ds = new DataSet();
            ds = objEMR.GetIndentType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            ddlIndentType.DataSource = ds.Tables[0];
            ddlIndentType.DataValueField = "Id";
            ddlIndentType.DataTextField = "IndentType";
            ddlIndentType.DataBind();
            if (Request.QueryString["Op"] != null)
            {
                ddlIndentType.SelectedIndex = ddlIndentType.Items.Count - 1;
                ddlIndentType.Enabled = false;
            }
            else
            {
                ddlIndentType.Enabled = true;
            }

            ds = new DataSet();
            ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ddlUnit.DataSource = ds.Tables[0];
            ddlUnit.DataValueField = "Id";
            ddlUnit.DataTextField = "UnitName";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlUnit.SelectedIndex = 0;

            ViewState["UnitMaster"] = ds.Tables[0];


            ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);
            ddlVolumeUnit.DataSource = ds.Tables[0];
            ddlVolumeUnit.DataValueField = "Id";
            ddlVolumeUnit.DataTextField = "UnitName";
            ddlVolumeUnit.DataBind();
            ddlVolumeUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlVolumeUnit.SelectedIndex = 0;

            ddlFlowRateUnit.DataSource = ds.Tables[0];
            ddlFlowRateUnit.DataValueField = "Id";
            ddlFlowRateUnit.DataTextField = "UnitName";
            ddlFlowRateUnit.DataBind();
            ddlFlowRateUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFlowRateUnit.SelectedIndex = 0;

            /*************** OrderSet ***************/
            bindOrderSet();
            BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
            ds = new DataSet();
            ds = objOT.CheckOTRequestForPatient(common.myInt(Session["EncounterId"]));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                dvDrugAdmin.Visible = true;
            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ddlDrugAdminIn.Items.Clear();
                ddlDrugAdminIn.Items.Add(new RadComboBoxItem("IP", "4"));
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
            objPharmacy = null;
            objMaster = null;
            objlis = null;
            objEMR = null;
            status = null;

            dsENC.Dispose();
            ds.Dispose();
            dv.Dispose();
            dsRoute.Dispose();
            tbl.Dispose();
        }
    }
    private void bindOrderSet()
    {
        BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        try
        {
            if (common.myStr(rdoSearchMedication.SelectedValue).Equals("OS"))
            {
                ds = objMst.GetEMRDrugSet(common.myInt(Session["HospitalLocationId"]), 0);

                if (ds.Tables.Count > 0)
                {
                    DV = ds.Tables[0].DefaultView;
                    DV.RowFilter = "DetailActive=1 AND SetName LIKE '%" + common.myStr(txtOrderSetName.Text) + "%'";

                    gvOrderSet.DataSource = DV.ToTable();
                    gvOrderSet.DataBind();
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
            objMst = null;
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

        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx";

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

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                lblMessage.Text = "Invalid Password !";
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

    private bool CheckDiagnosisPrimary()
    {

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckPrimaryDiagnosisForEncounter";
        APIRootClass.CheckPrimaryDiagnosisForEncounter objRoot = new global::APIRootClass.CheckPrimaryDiagnosisForEncounter();
        objRoot.EncounterId = common.myInt(Session["EncounterId"]);
        objRoot.RegistraionId = common.myInt(Session["RegistrationId"]);

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        bool CheckDiagnosisPrimaryForPatient = JsonConvert.DeserializeObject<bool>(sValue);

        return CheckDiagnosisPrimaryForPatient;
        //if (phr.CheckDiagnosisPrimaryForPatient(common.myInt(Session["EncounterId"])) == false)
        //if (!CheckDiagnosisPrimaryForPatient)
        //{
        //    //Alert.ShowAjaxMsg("Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..", this);
        //    //return;
        //    lblERXNo.Text = "Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..";
        //    return;
        //}
    }


    protected void btnSave_Onclick(object sender, EventArgs e)
    {
        try
        {
            //BaseC.clsPharmacy phr = new BaseC.clsPharmacy(sConString);
            if (!common.myStr(Session["OPIP"]).ToUpper().Equals("I"))
            {

                //if (!CheckDiagnosisPrimary())
                //{
                //    //Alert.ShowAjaxMsg("Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..", this);
                //    //return;
                //    lblERXNo.Text = "Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue..";
                //    return;
                //}

            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                lblERXNo.Text = "Patient has no appointment !";
                return;
            }
            //if (common.myInt(ddlAdvisingDoctor.SelectedValue) == 0)
            //{
            //    lblERXNo.Text = "Advising Doctor not selected!";
            //    return;
            //}

            //Password required only for intreaction found
            //if (IsPasswordRequired())
            //{
            //    IsValidPassword();
            //    return;
            //}

            saveData();
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


    protected void saveData()
    {
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //BaseC.WardManagement objwd = new BaseC.WardManagement();

        DataSet dsXml = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
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

            //if (common.myInt(Session["DrugReqSavecheck"]) == 0)
            //{
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Patient has no appointment !";
                return;
            }
            //if (common.myInt(ddlAdvisingDoctor.SelectedValue) == 0)
            //{
            //    lblMessage.Visible = true;
            //    lblMessage.Text = "Advising Doctor not selected!";
            //    return;
            //}
            //if (lbl_Weight.Text.ToString() == "" && !common.myStr(Session["OPIP"]).Equals("I"))
            //{
            //    lblMessage.Visible = true;
            //    lblMessage.Text = "Patient Weight is Required.";
            //    return;
            //}
            double iConversionFactor = 0;
            double sQuantity = 0;
            if (common.myStr(Session["OPIP"]) == "O" || common.myStr(Session["OPIP"]) == "E")
            {
                #region OP Drug
                foreach (GridViewRow dataItem in gvItem.Rows)
                {
                    HiddenField hdnGDGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnGDItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnUnAppPrescriptionId = (HiddenField)dataItem.FindControl("hdnUnAppPrescriptionId");

                    if ((ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                        || common.myInt(hdnReturnIndentDetailsIds.Value) > 0
                        || common.myBool(ViewState["CopyLastPresc"])
                        || common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        //Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        lblERXNo.Text = "Quantity should not be Zero!";
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");

                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        if (common.myInt(hdnRouteId.Value) <= 0)
                        {
                            lblERXNo.Text = "Please Edit and fill Route before saving";
                            return;
                        }

                        coll1.Add(common.myInt(hdnGDGenericId.Value));//GenericId int
                        if (common.myInt(hdnGDItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnGDItemId.Value));//ItemId INT,
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
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (common.myStr(hdnXMLData.Value).Trim() == string.Empty)
                        {
                            lblERXNo.Text = "Please add drug to save.";
                            // Alert.ShowAjaxMsg("Please add drug to save", Page);
                            return;
                        }
                        xmlSchema = common.myStr(hdnXMLData.Value);
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
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
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = common.myStr(dt.Rows[i]["Dose"]);

                                //string sFrequency = dt.Rows[i]["Frequency"].ToString();
                                string sDuration = common.myStr(dt.Rows[i]["Duration"]);
                                string sType = common.myStr(dt.Rows[i]["Type"]);

                                if (common.myDbl(sDose) <= 0)
                                {
                                    lblERXNo.Text = "Quantity should not be Zero, Please Edit Drug Details !";
                                    //Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    return;
                                }
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]).Trim() != string.Empty)//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
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
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]).Trim() != string.Empty)//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
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
                                            if (i == 0)
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
                                            if (i == dt.Rows.Count - 1)
                                            {
                                                if (ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                                                {
                                                    sEndDate = Convert.ToDateTime(DateTime.Now.AddDays(common.myInt(sDuration))).ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                                    //   sStartDate = dt.Rows[i]["StartDate"].ToString();
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
                                    if (i == 0)
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
                                    if (i == dt.Rows.Count - 1)
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
                                            sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
                                            //   sStartDate = dt.Rows[i]["StartDate"].ToString();
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
                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            HiddenField hdnCommentsDrugAllergy = (HiddenField)dataItem.FindControl("hdnCommentsDrugAllergy");
                            HiddenField hdnCommentsDrugToDrug = (HiddenField)dataItem.FindControl("hdnCommentsDrugToDrug");
                            HiddenField hdnCommentsDrugHealth = (HiddenField)dataItem.FindControl("hdnCommentsDrugHealth");

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
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
                    HiddenField hdnGDGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
                    HiddenField hdnGDItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

                    HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
                    Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                    HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");
                    HiddenField hdnStartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
                    HiddenField hdnUnAppPrescriptionId = (HiddenField)dataItem.FindControl("hdnUnAppPrescriptionId");

                    if ((ViewState["Stop"] != null && common.myBool(ViewState["Stop"]))
                        || common.myInt(hdnReturnIndentDetailsIds.Value) > 0
                        || common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        hdnIndentId.Value = "0";
                    }

                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
                        && common.myInt(txtTotalQty.Text) == 0)
                    {
                        // Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
                        lblERXNo.Text = "Quantity should not be Zero!";
                        return;
                    }

                    if (hdnIndentId.Value == "0" || hdnIndentId.Value == string.Empty)
                    {
                        HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
                        HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
                        HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
                        HiddenField hdnStrengthValue = (HiddenField)dataItem.FindControl("hdnStrengthValue");
                        HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
                        Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

                        coll1.Add(common.myInt(hdnIndentId.Value));//FrequencyId TINYINT,
                        if (common.myInt(hdnGDItemId.Value) != 0)
                        {
                            coll1.Add(common.myInt(hdnGDItemId.Value));//ItemId INT,
                            coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
                        }
                        else
                        {
                            coll1.Add(DBNull.Value);//ItemId INT,
                            coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
                        }
                        coll1.Add(common.myInt(hdnGDGenericId.Value));//GenericId int
                        coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
                        coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
                        coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT
                        coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT

                        dsXml = new DataSet();
                        string xmlSchema = string.Empty;
                        dt = new DataTable();
                        if (common.myStr(hdnXMLData.Value).Trim() == string.Empty)
                        {
                            //  lblMessage.Text = "Please add drug to save.";
                            // Alert.ShowAjaxMsg("Please add drug to save", Page);
                            lblERXNo.Text = "Please add drug to save";
                            return;
                        }

                        xmlSchema = common.myStr(hdnXMLData.Value).Trim();
                        StringReader sr = new StringReader(xmlSchema);

                        dsXml.ReadXml(sr);
                        dv = new DataView(dsXml.Tables[0]);
                        // dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value) ;
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value) + " And StartDate = '" + hdnStartDate.Value + "'";
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
                                    {

                                    }

                                    if (!dt.Columns.Contains("XMLFrequencyTime"))
                                    {
                                        dt.Columns.Add("XMLFrequencyTime", typeof(string));
                                    }
                                    if (!dt.Columns.Contains("IsSubstituteNotAllowed"))
                                    {
                                        dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                    }
                                    if (!dt.Columns.Contains("ICDCode"))
                                    {
                                        dt.Columns.Add("ICDCode", typeof(string));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (dt.Rows.Count > 0)
                        {
                            RadDatePicker dtStartTime = new RadDatePicker();
                            RadDatePicker dtEndTime = new RadDatePicker();
                            string sStartDate = string.Empty;
                            string sEndDate = string.Empty;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int iFrequencyId = common.myInt(dt.Rows[i]["FrequencyId"]);
                                string sDose = common.myStr(dt.Rows[i]["Dose"]);
                                string sDuration = common.myStr(dt.Rows[i]["Duration"]);
                                string sType = common.myStr(dt.Rows[i]["Type"]);

                                if (common.myDbl(sDose) <= 0)
                                {
                                    //   Alert.ShowAjaxMsg("Quantity should not be Zero, Please Edit Drug Details !", this.Page);
                                    lblERXNo.Text = "Quantity should not be Zero, Please Edit Drug Details !";
                                    return;
                                }
                                if (common.myStr(dt.Rows[i]["XMLFrequencyTime"]).Trim() != string.Empty)//&& (common.myStr(dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    string xmlFrequencySchema = common.myStr(dt.Rows[i]["XMLFrequencyTime"]);
                                    StringReader srFrequency = new StringReader(xmlFrequencySchema);
                                    DataSet dsXmlFrequency = new DataSet();
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
                                if (common.myStr(dt.Rows[i]["XMLVariableDose"]).Trim() != string.Empty)//&& (common.myStr( dt.Rows[i]["Type"]) == "D") && (ddlDoseType.Text != "PRN") && (ddlDoseType.Text != "STAT"))// for variable dose
                                {
                                    StringReader srVariableDose = new StringReader(common.myStr(dt.Rows[i]["XMLVariableDose"]));
                                    DataSet dsVariableDose = new DataSet();
                                    dsVariableDose.ReadXml(srVariableDose);

                                    for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                                    {
                                        for (int Col = 1; Col < common.myInt(dsVariableDose.Tables[0].Columns.Count); Col++)
                                        {
                                            string variableDoseDate = common.myStr(dsVariableDose.Tables[0].Rows[row]["Date"]);
                                            string variableDose = common.myStr(dsVariableDose.Tables[0].Rows[row][Col]);

                                            int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                            if (i == 0)
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
                                            if (i == dt.Rows.Count - 1)
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
                                                    sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
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
                                            coll.Add(Col);// variable sequence no                                             
                                            coll.Add("");// FlowrateTime/
                                            strXML.Append(common.setXmlTable(ref coll));
                                        }
                                    }


                                }
                                else// without variable dose
                                {

                                    int iUnit = common.myInt(dt.Rows[i]["UnitId"]);
                                    if (i == 0)
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
                                    if (i == dt.Rows.Count - 1)
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
                                            sEndDate = common.myStr(dt.Rows[i]["EndDate"]);
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
                                    coll.Add(string.Empty);// variable sequence no 
                                    coll.Add("");// FlowrateTime/
                                    strXML.Append(common.setXmlTable(ref coll));
                                }
                            }

                            coll1.Add(sStartDate != string.Empty ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
                            coll1.Add(sEndDate != string.Empty ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
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

                        if (common.myBool(ViewState["ConversioinFactor"]))
                        {
                            //ds = objwd.GetItemConversionFactor(common.myInt(hdnGDItemId.Value));

                            //WebClient client1 = new WebClient();
                            //client1.Headers["Content-type"] = "application/json";
                            //client1.Encoding = Encoding.UTF8;
                            //string ServiceURL1 = WebAPIAddress.ToString() + "api/Masters/GetItemConversionFactor";
                            //APIRootClass.GetItemConversionFactor objRoot = new global::APIRootClass.GetItemConversionFactor();
                            //objRoot.ItemId = common.myInt(hdnGDItemId.Value);
                            ////string strtriageID = string.Empty;
                            //string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot);
                            //string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                            //sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                            //sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                            //ds = JsonConvert.DeserializeObject<DataSet>(sValue1);


                            string ServiceURL1 = WebAPIAddress.ToString() + "api/Masters/GetItemConversionFactor";
                            APIRootClass.GetItemConversionFactor objRoot1 = new global::APIRootClass.GetItemConversionFactor();
                            objRoot1.ItemId = common.myInt(hdnGDItemId.Value);
                            WebClient client1 = new WebClient();
                            client1.Headers["Content-type"] = "application/json";
                            client1.Encoding = Encoding.UTF8;
                            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
                            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                            ds = JsonConvert.DeserializeObject<DataSet>(sValue1);




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
                        if (common.myBool(ViewState["ConversioinFactor"]) && iConversionFactor != 0)
                        {
                            sQuantity = common.myDbl(sQuantity.ToString("F2"));
                        }
                        else
                        {
                            sQuantity = common.myDbl(txtTotalQty.Text);
                        }
                    }
                }
                #endregion
            }

            if (strXML.ToString() == string.Empty)
            {
                lblERXNo.Text = "Please add medicine before saving !";
                return;
            }
            bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" ? true : false;
            string sXMLFre = strXMLFre != null ? strXMLFre.ToString() : string.Empty;
            //Hashtable hshOutput = new Hashtable();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = "";

            string inputJson = "";


            if (Session["OPIP"] != null && common.myStr(Session["OPIP"]) == "I")
            {
                //hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
                //    common.myInt(ddlAdvisingDoctor.SelectedValue), strXML1.ToString(), strXML.ToString(), string.Empty,
                //    Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0, common.myInt(Session["UserId"]),
                //    isConsumable, sXMLFre, strXMLUnAppPrescIds.ToString());

                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveMedicineOrderIP";
                APIRootClass.SaveMedicineOrderIP objRoot = new global::APIRootClass.SaveMedicineOrderIP();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.RegistrationId = common.myInt(ViewState["RegId"]);
                objRoot.EncounterId = common.myInt(ViewState["EncId"]);
                objRoot.IndentType = common.myInt(ddlIndentType.SelectedValue);
                objRoot.StoreId = common.myInt(ddlStore.SelectedValue);
                objRoot.DoctorId = common.myInt(ddlAdvisingDoctor.SelectedValue);
                objRoot.xmlItems = strXML1.ToString();
                objRoot.xmlItemDetail = strXML.ToString();
                objRoot.Remarks = string.Empty;
                objRoot.DrugOrderType = Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.IsConsumable = isConsumable;
                objRoot.xmlFrequencyTime = sXMLFre;
                objRoot.xmlUnApprovedPrescriptionIds = strXMLUnAppPrescIds.ToString();
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            }
            else
            {
                //hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //                    common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
                //                    common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ddlAdvisingDoctor.SelectedValue),
                //                    0, 0, strXML1.ToString(), strXML.ToString(), string.Empty, common.myInt(Session["UserId"]),
                //                    sXMLFre, isConsumable, strXMLUnAppPrescIds.ToString());

                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveMedicineOrderOP";
                APIRootClass.SaveMedicineOrderOP objRoot = new global::APIRootClass.SaveMedicineOrderOP();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.RegistrationId = common.myInt(ViewState["RegId"]);
                objRoot.EncounterId = common.myInt(ViewState["EncId"]);
                objRoot.IndentType = common.myInt(ddlIndentType.SelectedValue);
                objRoot.StoreId = common.myInt(ddlStore.SelectedValue);
                objRoot.DoctorId = common.myInt(ddlAdvisingDoctor.SelectedValue);
                objRoot.IsPregnant = 0;
                objRoot.IsBreastFeeding = 0;
                objRoot.xmlItems = strXML1.ToString();
                objRoot.xmlItemDetail = strXML.ToString();
                objRoot.xmlPatientAlerts = string.Empty;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.IsConsumable = isConsumable;
                objRoot.xmlFrequencyTime = sXMLFre;
                objRoot.xmlUnApprovedPrescriptionIds = strXMLUnAppPrescIds.ToString();
                objRoot.intDrugAdminIn = common.myInt(ddlDrugAdminIn.SelectedValue);
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            }
            DataSet dshOutput = new DataSet();
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dshOutput = JsonConvert.DeserializeObject<DataSet>(sValue);

            //if ((common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("UPDATE") || common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("SAVED"))
            //    && !common.myStr(hshOutput["@chvErrorStatus"]).ToUpper().Contains("USP"))
            if ((common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]).ToUpper().Contains("UPDATE") || common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]).ToUpper().Contains("SAVED"))
                && !common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]).ToUpper().Contains("USP"))
            {
                string strMsg = common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]);

                hdnReturnIndentDetailsIds.Value = string.Empty;


                #region ePrescription

                if ((common.myStr(Session["OPIP"]) == "O" || common.myStr(Session["OPIP"]) == "E") && ddlStore.SelectedValue != "859")
                {
                    string OutRefno = "0";
                    int indentid = common.myInt(common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"]).Split('$')[1]);
                    string active = common.GetFlagValueHospitalSetupV3(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EpresActive", string.Empty);
                    //string s = "Operation is successful";
                    string s = "RECORD SAVED";
                    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //int isactive = common.myInt(dl.ExecuteScalar(CommandType.Text, "selecT convert(int,isnull(EprescriptionEnabled,0))  From doctorDetails with(nolock) where DoctorID=" + common.myInt(Session["EmployeeId"]).ToString()));

                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/IsDOctorEprescriptionEnabled";
                    APIRootClass.IsDOctorEprescriptionEnabled objRoot = new global::APIRootClass.IsDOctorEprescriptionEnabled();
                    objRoot.DoctorId = common.myInt(Session["EmployeeId"]);

                    inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    int isactive = JsonConvert.DeserializeObject<int>(sValue);

                    if (isactive == 1)
                    {
                        if (common.myInt(active) == 1)
                        {

                            if (isactive == 1)
                            {
                                s = PostePrescription(common.myInt(common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"]).Split('$')[1]), "PRODUCTION", out OutRefno);
                                //  Alert.ShowAjaxMsg(s, this);
                                lblERXNo.Text = s;
                                //        dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                            }
                            else
                            {
                                //   DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                                //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                                client = new WebClient();
                                client.Headers["Content-type"] = "application/json";
                                client.Encoding = Encoding.UTF8;
                                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                                APIRootClass.UpdateDHARefNo objRoot1 = new global::APIRootClass.UpdateDHARefNo();
                                objRoot1.DHARefNo = OutRefno;
                                objRoot1.IndentId = indentid;

                                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                                sValue = client.UploadString(ServiceURL, inputJson);

                            }
                        }
                        else
                        {
                            //DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                            //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                            client = new WebClient();
                            client.Headers["Content-type"] = "application/json";
                            client.Encoding = Encoding.UTF8;
                            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                            APIRootClass.UpdateDHARefNo objRoot1 = new global::APIRootClass.UpdateDHARefNo();
                            objRoot1.DHARefNo = OutRefno;
                            objRoot1.IndentId = indentid;

                            inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                            sValue = client.UploadString(ServiceURL, inputJson);
                        }
                        if (s.Contains("RECORD SAVED") || s.Contains("Operation is successful"))
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            strMsg = "Prescription No : " +
                                ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"])).Split('$')[0] +
                                " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
                            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                            //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                            client = new WebClient();
                            client.Headers["Content-type"] = "application/json";
                            client.Encoding = Encoding.UTF8;
                            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                            APIRootClass.UpdateDHARefNo objRoot1 = new global::APIRootClass.UpdateDHARefNo();
                            objRoot1.DHARefNo = OutRefno;
                            objRoot1.IndentId = indentid;

                            inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                            sValue = client.UploadString(ServiceURL, inputJson);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = s;
                        }
                    }
                    else
                    {
                        //DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        //dl1.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                        APIRootClass.UpdateDHARefNo objRoot1 = new global::APIRootClass.UpdateDHARefNo();
                        objRoot1.DHARefNo = OutRefno;
                        objRoot1.IndentId = indentid;

                        inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                        sValue = client.UploadString(ServiceURL, inputJson);
                    }
                    if (s.Contains("RECORD SAVED") || s.Contains("Operation is successful"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        strMsg = "Prescription No : " +
                            ((common.myInt(ddlPrescription.SelectedValue) > 0) ? common.myStr(ddlPrescription.SelectedItem.Text) : common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"])).Split('$')[0] +
                            " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
                        //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                        client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                        APIRootClass.UpdateDHARefNo objRoot1 = new global::APIRootClass.UpdateDHARefNo();
                        objRoot1.DHARefNo = OutRefno;
                        objRoot1.IndentId = indentid;

                        inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                        sValue = client.UploadString(ServiceURL, inputJson);
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = s;
                    }
                    if (OutRefno != "0")
                    {
                        lblERXNo.Text = "Rx No: " + OutRefno;
                    }

                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strMsg;
                }

                #endregion


                //if (common.myBool(Request.QueryString["IsEMRPopUp"]))
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);

                //    return;
                //}
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                txtCustomMedication.Text = string.Empty;
                hdnGenericId.Value = "0";
                hdnGenericName.Value = string.Empty;
                hdnItemId.Value = "0";
                hdnItemName.Value = string.Empty;
                //ddlIndentType.SelectedValue = "0";
                ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByValue("0"));
                hdnCIMSItemId.Value = string.Empty;
                hdnCIMSType.Value = string.Empty;
                hdnVIDALItemId.Value = "0";
                txtTotQty.Text = string.Empty;
                ddlFormulation.SelectedIndex = 0;
                ddlRoute.SelectedIndex = 0;
                txtStrengthValue.Text = string.Empty;
                txtDose.Text = string.Empty;
                ddlFrequencyId.SelectedIndex = 0;
                txtDuration.Text = string.Empty;
                ddlPeriodType.SelectedIndex = 0;
                txtStartDate.SelectedDate = DateTime.Now;
                txtEndDate.SelectedDate = DateTime.Now;
                txtInstructions.Text = "";
                clearItemDetails();
                dvPharmacistInstruction.Visible = false;
                lnkPharmacistInstruction.Visible = false;
                Session["CurentSavedPrescriptionId"] = string.Empty;
                if (common.myInt(dshOutput.Tables[0].Rows[0]["intPrescriptionId"]) > 0)
                {
                    Session["CurentSavedPrescriptionId"] = common.myInt(dshOutput.Tables[0].Rows[0]["intPrescriptionId"]).ToString();
                    //dvConfirmPrint.Visible = true;
                }
                ViewState["CopyLastPresc"] = null;
                ViewState["ItemDetail"] = null;
                ViewState["UnAppPrescriptionId"] = null;

            }

            BindGrid(string.Empty, string.Empty);
            BindBlankItemGrid();
            ViewState["Item"] = null;
            ViewState["Stop"] = null;
            ViewState["Edit"] = null;


            btnCopyLastPrescription.Enabled = true;
            btnPrint_Click(null, null);
            lblMessage.Text = common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            //objEMR = null;
            //objwd = null;

            dsXml.Dispose();
            ds.Dispose();
            dt.Dispose();
            dv.Dispose();
        }
    }
    protected void ddlGeneric_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable ds = new DataTable();

        try
        {
            RadComboBox ddl = sender as RadComboBox;

            ds = GetGenericData(e.Text);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, ds.Rows.Count);
            e.EndOfItems = endOffset == ds.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                //ddl.Items.Add(new RadComboBoxItem(common.myStr(ds.Rows[i]["GenericName"]), common.myStr(ds.Rows[i]["GenericId"])));

                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(ds.Rows[i]["GenericName"]);
                item.Value = common.myStr(ds.Rows[i]["GenericId"]);

                item.Attributes.Add("CIMSItemId", common.myStr(ds.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(ds.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myInt(ds.Rows[i]["VIDALItemId"]).ToString());

                ddl.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, ds.Rows.Count);
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
    private DataTable GetGenericData(string text)
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            ds = objPharmacy.GetGenericDetails(0, text, 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPharmacy = null;
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
            if (ddlFormulation.SelectedValue != null && ddlFormulation.SelectedItem.Text != string.Empty)
            {
                //ddlRoute.SelectedValue = ddlFormulation.SelectedItem.Attributes["DefaultRouteId"].ToString();
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(ddlFormulation.SelectedItem.Attributes["DefaultRouteId"])));
            }
            else
            {
                ddlRoute.Text = string.Empty;
                //ddlRoute.SelectedValue = "0";
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
            }
            // ddlFormulation.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    private bool CheckDDCExpiry(int itemid)
    {
        bool isddcexpire = false;

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsdoctor = dl.FillDataSet(CommandType.Text, "Exec UspCheckDDCExpiry " + common.myInt(itemid).ToString());
        if (dsdoctor.Tables[0].Rows.Count > 0)
        {
            isddcexpire = false;
        }
        else
        {
            isddcexpire = true;
        }
        return isddcexpire;
    }
    protected void ddlBrand_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        try
        {
            RadComboBox ddl = sender as RadComboBox;
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

            if (common.myInt(hdnStoreId.Value) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Store not selected !";

                return;
            }

            data = GetBrandData(e.Text, GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
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
                item.Attributes.Add("VIDALItemId", common.myInt(data.Rows[i]["VIDALItemId"]).ToString());

                this.ddlBrand.Items.Add(item);
                item.DataBind();
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
        DataSet dsSearch = new DataSet();
        DataTable dt = new DataTable();

        //BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            int StoreId = common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
            int ItemId = 0;

            int itemBrandId = 0;
            int WithStockOnly = 0;

            int iOT = 3;

            if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]) == "OT"
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO")
            {
                iOT = 2;
            }
            else if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]) == "WARD"
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO")
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

            int Alldrug = 0;
            if (chkAllbrand.Checked)
            {
                Alldrug = 1;
            }


            //dsSearch = objPharmacy.getItemsWithStock_Network(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId, GenericId,
            //                                        common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0, text.Replace("'", "''"),
            //                                        WithStockOnly, string.Empty, iOT, "P", Alldrug, common.myInt(Session["RegistrationNo"]));

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Masters/GetNetworkWiseItemsWithStock";
            APIRootClass.GetNetworkWiseItemsWithStock objRoot = new global::APIRootClass.GetNetworkWiseItemsWithStock();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            objRoot.StoreId = StoreId;
            objRoot.ItemId = ItemId;
            objRoot.GenericId = GenericId;
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.SupplierId = 0;
            objRoot.ItemName = text.Replace("'", "''");
            objRoot.WithStockOnly = WithStockOnly;
            objRoot.ItemNo = string.Empty;
            objRoot.ItemSubCategoryId = iOT;
            objRoot.Usedtype = "P";
            objRoot.AllBrand = Alldrug;
            objRoot.RegistratioNo = common.myInt(Session["RegistrationNo"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);

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
            //objPharmacy = null;
        }

        return dt;
    }

    //protected void ddlUnit_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    calcTotalQty();
    //}

    //protected void ddlPeriodType_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        endDateChange();
    //        ddlReferanceItem.Focus();
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void ddlUnit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFrequencyId.Focus();

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

                if (common.myStr(ddlFrequencyId.SelectedItem.Text).StartsWith("STAT"))
                {
                    txtDuration.Text = "1";
                    ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                }
            }

            // txtDuration.Focus();
            calcTotalQtySelectedMed();
            //txtInstructionsHeader.Text = common.myStr(CalSelectedPrescriptionDetails());
            setPrescriptionInstructoins();
            ddlRoute.Focus();
        }
        catch
        {
        }
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();
        SetSaveButtonText();
        if (common.myInt(tbDiagnosis.ActiveTabIndex).Equals(0))
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindFavourite();", true);
        }
        else if (common.myInt(tbDiagnosis.ActiveTabIndex).Equals(1))
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindCurrentMedication();", true);
        }
        else if (common.myInt(tbDiagnosis.ActiveTabIndex).Equals(2))
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindOrderSet();", true);
        }
    }
    private void SetSaveButtonText()
    {
        try
        {
            btnSave.Text = "Generate Rx (F3)";
            if (common.myBool(ddlStore.SelectedItem.Attributes["OrderEntryStore"]))
            {
                btnSave.Text = "Save";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = string.Empty;
        string newCurrentDate = string.Empty;

        try
        {
            string currentdate = formatdate.FormatDateDateMonthYear(outputCurrentDate);
            string strformatCurrDate = formatdate.FormatDate(currentdate, "MM/dd/yyyy", "yyyy/MM/dd");
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
            formatdate = null;
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
            //  calcTotalQty();

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
            if (common.myDbl(txtDuration.Text) == 0)
            {
                //txtDuration.Text = "1";
            }

            double dose = common.myDbl(txtDose.Text);

            ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ddlFrequencyId.SelectedValue).ToString()));

            double frequency = (common.myInt(ddlFrequencyId.SelectedValue) == 0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            double days = common.myDbl(txtDuration.Text);
            double totalQty = 0;
            string unitname = string.Empty;
            unitname = (common.myStr(ddlUnit.Text).Trim() == string.Empty) ? "(Advised Unit) " : " (" + ddlUnit.Text.ToString() + ") ";
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
            if (common.myBool(ViewState["ISCalculationRequired"]))
            {
                totalQty = frequency * days * dose;
                txtTotQty.Text = totalQty.ToString("F2");
            }
            else
            {
                txtTotQty.Text = "1";
            }

            txtSpecialInstrucation.Attributes.Add("Style", "Width:100%");
            txtSpecialInstrucation.Text = dose.ToString() + " " + unitname + ddlFrequencyId.Text + " For " + txtDuration.Text + " " + ddlPeriodType.Text;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void calcTotalQtyForVariableDose(string sVariableDoseString, int noOfDose)
    {
        try
        {

            if (common.myLen(sVariableDoseString) > 0)
            {
                StringReader srVariableDose = new StringReader(common.myStr(sVariableDoseString));
                DataSet dsVariableDose = new DataSet();
                dsVariableDose.ReadXml(srVariableDose);
                double Duration = 0, totalQty = 0, Dose = 0;
                for (int row = 0; row < common.myInt(dsVariableDose.Tables[0].Rows.Count); row++)
                {
                    //Duration = common.myDbl(dsVariableDose.Tables[0].Rows[row]["Duration"]);
                    //switch (common.myStr(ddlPeriodType.SelectedValue))
                    //{
                    //    case "H":
                    //        Duration = Duration * 1;
                    //        break;
                    //    case "D":
                    //        Duration = Duration * 1;
                    //        break;
                    //    case "W":
                    //        Duration = Duration * 7;
                    //        break;
                    //    case "M":
                    //        Duration = Duration * 30;
                    //        break;
                    //    case "Y":
                    //        Duration = Duration * 365;
                    //        break;
                    //    default:
                    //        Duration = Duration * 1;
                    //        break;
                    //}
                    for (int Col = 1; Col <= noOfDose; Col++)
                    {
                        Dose = common.myDbl(dsVariableDose.Tables[0].Rows[row]["Dose" + Col]);
                        totalQty = totalQty + (Duration * Dose);
                    }
                }

                if (common.myBool(ViewState["ISCalculationRequired"]))
                {
                    txtTotQty.Text = totalQty.ToString("F2");
                }
                else
                {
                    txtTotQty.Text = "1";
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

    private void calcTotalQtySelectedMed()
    {
        try
        {
            if (common.myBool(Session["ISCalculationRequired"]))
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
                //if (common.myDbl(txtDuration.Text) == 0)
                //{
                //    //txtDuration.Text = "1";
                //}

                double dose = common.myDbl(txtDose.Text);

                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ddlFrequencyId.SelectedValue).ToString()));

                double frequency = (common.myInt(ddlFrequencyId.SelectedValue) == 0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                double days = common.myDbl(txtDuration.Text);
                double totalQty = 0;
                string unitname = string.Empty;
                unitname = (common.myStr(ddlUnit.Text).Trim() == string.Empty) ? "(Advised Unit) " : " (" + ddlUnit.Text.ToString() + ") ";
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

                //if (common.myBool(ViewState["ISCalculationRequired"]))
                //{

                totalQty = frequency * days * dose;
                // txtTotQty.Text = totalQty.ToString("F2");
                txtTotQty.Text = totalQty.ToString(("0.##") + " ");
            }
            else
            {
                txtTotQty.Text = "1";
            }

            //txtSpecialInstrucation.Attributes.Add("Style", "Width:100%");
            //txtSpecialInstrucation.Text = dose.ToString() + " " + unitname + ddlFrequencyId.Text + " For " + txtDuration.Text + " " + ddlPeriodType.Text;
            //  txtTotalQuantity.Text =  txtTotQty.Text;
            //ddlPeriodType.Focus();
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
                && common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" &&
                (common.myStr(Request.QueryString["LOCATION"]) == "OT" || common.myStr(Request.QueryString["LOCATION"]) == "WARD"))
            {
                e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
            }
            else
            {
                e.Row.Cells[(byte)enumColumns.TotalQty].Visible = false;
                //   e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
            }

            e.Row.Cells[(byte)enumColumns.Sno].Visible = false;
            e.Row.Cells[(byte)enumColumns.IndentType].Visible = false;
            //e.Row.Cells[(byte)enumColumns.PrescriptionDetail].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            DataTable dt = new DataTable();
            try
            {
                if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
                    && common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" &&
                    (common.myStr(Request.QueryString["LOCATION"]) == "OT" || common.myStr(Request.QueryString["LOCATION"]) == "WARD"))
                {
                    e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
                }
                else
                {
                    e.Row.Cells[(byte)enumColumns.TotalQty].Visible = false;
                    //e.Row.Cells[(byte)enumColumns.TotalQty].Visible = true;
                }

                e.Row.Cells[(byte)enumColumns.Sno].Visible = false;
                e.Row.Cells[(byte)enumColumns.IndentType].Visible = false;
                //e.Row.Cells[(byte)enumColumns.PrescriptionDetail].Visible = false;


                TextBox txtTotalQty = (TextBox)e.Row.FindControl("txtTotalQty");
                txtTotalQty.Text = common.myDbl(txtTotalQty.Text).ToString("F2");

                gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;

                //gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;
                //gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
                //gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;

                //gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
                //gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                //gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                //gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
                //gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;

                    //gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = true;                    
                    //gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;
                    //gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    ImageButton lnkBtnMonographCIMS = (ImageButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)e.Row.FindControl("lnkBtnInteractionCIMS");

                    //LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");                    
                    //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                    //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                    //lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0
                        || common.myStr(hdnCIMSItemId.Value).Trim() == "0")
                    {
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;

                        //lnkBtnBrandDetailsCIMS.Visible = false;                        
                        //lnkBtnDHInteractionCIMS.Visible = false;
                        //lnkBtnDAInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        //lnkBtnBrandDetailsCIMS.Visible = false;

                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");
                        string strXML = string.Empty;

                        //if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        //{
                        //    strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        //    if (strXML != string.Empty)
                        //    {
                        //        string outputValues = objCIMS.getFastTrack5Output(strXML);

                        //        if (outputValues != null)
                        //        {
                        //            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" name=";
                        //            if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                        //            {
                        //                lnkBtnBrandDetailsCIMS.Visible = true;
                        //            }
                        //        }
                        //    }
                        //}

                        lnkBtnMonographCIMS.Visible = false;
                        strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        if (strXML != string.Empty)
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

                //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //{
                //    gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = true;
                //    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                //    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                //    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;
                //    gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = true;

                //    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                //    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsVIDAL");
                //    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                //    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                //    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");
                //    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionVIDAL");

                //    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                //    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                //    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                //    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                //    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                //    lnkBtnBrandDetailsVIDAL.Visible = true;
                //    lnkBtnMonographVIDAL.Visible = true;
                //    lnkBtnInteractionVIDAL.Visible = true;
                //    lnkBtnDHInteractionVIDAL.Visible = true;
                //    //lnkBtnDAInteractionVIDAL.Visible = true;

                //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                //    {
                //        lnkBtnBrandDetailsVIDAL.Visible = false;
                //        lnkBtnMonographVIDAL.Visible = false;
                //        lnkBtnInteractionVIDAL.Visible = false;
                //        lnkBtnDHInteractionVIDAL.Visible = false;
                //        lnkBtnDAInteractionVIDAL.Visible = false;
                //    }
                //}

                //if (!chkShowDetails.Checked)
                //{
                //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                //    {
                //        gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = false;
                //        gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = false;

                //        //gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = false;                        
                //        //gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = false;
                //        //gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = false;
                //    }
                //    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //    //{
                //    //    gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = false;
                //    //    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = false;
                //    //    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = false;
                //    //    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = false;
                //    //    gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = false;
                //    //}
                //}

                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnGDItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

                HiddenField hdnXMLData = (HiddenField)e.Row.FindControl("hdnXMLData");


                Label lblItemName = (Label)e.Row.FindControl("lblItemName");
                HiddenField hdnGenericName = (HiddenField)e.Row.FindControl("hdnGenericName");

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
                                if (!dt.Columns.Contains("XMLFrequencyTime"))
                                {
                                    dt.Columns.Add("XMLFrequencyTime", typeof(string));
                                }
                                if (!dt.Columns.Contains("IsSubstituteNotAllowed"))
                                {
                                    dt.Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                }
                                if (!dt.Columns.Contains("ICDCode"))
                                {
                                    dt.Columns.Add("ICDCode", typeof(string));
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
                        //if (hdnCustomMedication.Value == "1")
                        //{
                        //    dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                        //}
                        //else
                        //{
                        dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                        //}

                        HiddenField hdnUnAppPrescriptionId = (HiddenField)e.Row.FindControl("hdnUnAppPrescriptionId");

                        if (dv.ToTable().Rows.Count > 0)
                        {
                            DataTable dtData = dv.ToTable().Copy();

                            foreach (DataRow item in dtData.Rows)
                            {
                                item["XMLData"] = string.Empty;
                            }

                            if (common.myInt(hdnUnAppPrescriptionId.Value).Equals(0))
                            {
                                lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringV2(dtData);
                                lblItemName.ToolTip = lblPrescriptionDetail.Text;
                            }
                            // dt.TableName = "ItemDetail";
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
                                DataTable dtData1 = dv1.ToTable().Copy();

                                foreach (DataRow item in dtData1.Rows)
                                {
                                    item["XMLData"] = string.Empty;
                                }

                                if (common.myInt(hdnUnAppPrescriptionId.Value).Equals(0))
                                {
                                    lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringV2(dtData1);
                                    lblItemName.ToolTip = lblPrescriptionDetail.Text;
                                }

                                dt.TableName = "ItemDetail";
                                System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                                System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                                dtData1.WriteXml(writer, XmlWriteMode.IgnoreSchema);

                                string xmlSchema = writer.ToString();
                                xmlSchema = xmlSchema.Replace("&lt;", "<");
                                xmlSchema = xmlSchema.Replace("&gt;", ">");

                                hdnXMLData.Value = xmlSchema;
                            }
                            else
                            {
                                lblPrescriptionDetail.Text = string.Empty;
                            }
                        }

                        dv.RowFilter = string.Empty;
                        dv1.RowFilter = string.Empty;
                    }

                }

                //HiddenField hdnIsInfusion = (HiddenField)e.Row.FindControl("hdnIsInfusion");

                //    dtPrevious = (DataTable)ViewState["Item"];
                //    if (common.myInt(DR["IsInfusion"]) == 1)
                //    {
                //        RadComboBoxItem item = new RadComboBoxItem();
                //        item.Text = common.myStr(DR["ItemName"]);
                //        item.Value = common.myStr(common.myInt(DR["ItemId"]));
                //        item.Attributes.Add("IsInfusion", common.myStr(DR["IsInfusion"]));
                //        item.DataBind();
                //        ddlReferanceItem.Items.Add(item);

                //    }
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
                dv.Dispose();
                dv1.Dispose();
                dt.Dispose();
            }
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
            txtStrengthValue.Enabled = true;

            ddlBrand.Focus();
            ddlBrand.Items.Clear();
            ddlBrand.Text = string.Empty;
            ddlBrand.Enabled = true;
            ddlBrand.SelectedValue = null;

            ddlGeneric.Items.Clear();
            ddlGeneric.Text = string.Empty;
            ddlGeneric.SelectedValue = null;

            txtInstructions.Text = string.Empty;
            //ddlFoodRelation.SelectedValue = "0";
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
            ddlFoodRelation.Text = string.Empty;
            chkNotToPharmacy.Checked = false;


            ddlFrequencyId.SelectedIndex = 0;
            txtDose.Text = string.Empty; //"1"
            txtDuration.Text = string.Empty;
            //ddlPeriodType.SelectedValue = "D";
            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
            //ddlUnit.SelectedValue = "0";
            ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));
            txtInstructions.Text = string.Empty;
            //ddlReferanceItem.SelectedValue = "0";
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));
            //ddlFoodRelation.SelectedValue = "0";
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));
            //ddlDoseType.SelectedValue = "0";
            ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue("0"));

            //ddlFormulation.SelectedValue = "0";
            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue("0"));
            //ddlRoute.SelectedValue = "0";
            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));
            txtEndDate.SelectedDate = null;

            txtStartDate.SelectedDate = DateTime.Now;
            txtEndDate.SelectedDate = null;

            ViewState["Edit"] = null;
            txtCustomMedication.Text = string.Empty;
            hdnXmlVariableDoseString.Value = string.Empty;
            hdnvariableDoseDuration.Value = string.Empty;
            hdnvariableDoseFrequency.Value = string.Empty;
            hdnVariabledose.Value = string.Empty;
            Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
            Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = null;
        }
        catch
        { }
    }
    protected void gvItem_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet dsCalisreq = new DataSet();

        DataSet dsXml = new DataSet();
        DataTable dtDetail = new DataTable();
        DataView DV = new DataView();
        DataView DV1 = new DataView();
        DataTable dt = new DataTable();
        DataView dvFilter = new DataView();
        DataTable tbl = new DataTable();
        DataTable tbl1 = new DataTable();

        try
        {
            if (e.CommandName == "ItemDelete")
            {
                //int ItemId = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);


                int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);
                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);

                HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");

                if (ViewState["ItemDetail"] != null && (ItemId > 0 || GenericId > 0 || common.myBool(hdnCustomMedication.Value)))
                {
                    tbl = (DataTable)ViewState["Item"];
                    tbl1 = (DataTable)ViewState["ItemDetail"];
                    DV = tbl.Copy().DefaultView;
                    DV1 = tbl1.Copy().DefaultView;

                    if (common.myBool(hdnCustomMedication.Value))
                    {
                        Label lblItemName = (Label)row.FindControl("lblItemName");
                        DV.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                        DV1.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
                    }
                    else
                    {
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
                    }

                    tbl = DV.ToTable();

                    ViewState["GridDataItem"] = DV.ToTable();
                    ViewState["Item"] = DV.ToTable();
                    ViewState["ItemDetail"] = DV1.ToTable();

                    if (tbl.Rows.Count == 0)
                    {
                        DataRow DR = tbl.NewRow();
                        tbl.Rows.Add(DR);
                        tbl.AcceptChanges();
                    }

                    DataTable dtData = DV.ToTable();
                    dtData = addColumnInItemGrid(dtData);

                    #region delete from EMRUnApprovedPrescriptions

                    HiddenField hdnUnAppPrescriptionId = (HiddenField)row.FindControl("hdnUnAppPrescriptionId");

                    if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                    {
                        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                        string strMsg = objEMR.deleteUnApprovedPrescriptions(common.myInt(hdnUnAppPrescriptionId.Value), common.myInt(ViewState["EncId"]),
                                        common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]));
                    }

                    #endregion

                    gvItem.DataSource = dtData;
                    gvItem.DataBind();

                    if (DV1.ToTable().Rows.Count == 0)
                    {
                        ViewState["GridDataItem"] = null;
                        ViewState["Item"] = null;
                        ViewState["ItemDetail"] = null;

                        BindBlankItemGrid();
                    }

                    ViewState["StopItemDetail"] = null;
                    ViewState["Edit"] = null;
                    setVisiblilityInteraction();
                }
            }
            //else if (e.CommandName == "BrandDetailsCIMS")
            //{
            //    if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            //    {
            //        Session["CIMSXMLInputData"] = string.Empty;
            //    }

            //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            //    HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

            //    showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            //}
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            //else if (e.CommandName == "DHInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("A");
            //}
            //else if (e.CommandName == "BrandDetailsVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getBrandDetailsVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "MonographVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getMonographVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "InteractionVIDAL")
            //{
            //    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds().ToArray();

            //    if (commonNameGroupIds.Length > 0)
            //    {
            //        getDrugToDrugInteractionVidal(commonNameGroupIds);
            //    }
            //}
            //else if (e.CommandName == "DHInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("A");
            //}
            else if (e.CommandName == "Select")
            {
                dsXml = new DataSet();
                dtDetail = new DataTable();
                DV = new DataView();
                dt = new DataTable();
                dvFilter = new DataView();
                int ItemId = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                string GenericName = common.myStr(((HiddenField)row.FindControl("hdnGenericName")).Value);

                //txtCommentsDrugAllergy.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugAllergy")).Value);
                txtCommentsDrugToDrug.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugToDrug")).Value);
                //txtCommentsDrugHealth.Text = common.myStr(((HiddenField)row.FindControl("hdnCommentsDrugHealth")).Value);

                hdnCIMSItemId.Value = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                hdnCIMSType.Value = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);
                hdnVIDALItemId.Value = common.myInt(((HiddenField)row.FindControl("hdnVIDALItemId")).Value).ToString();
                HiddenField hdnStrengthValue = (HiddenField)row.FindControl("hdnStrengthValue");

                HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");
                HiddenField hdnXmlVariableDose = (HiddenField)row.FindControl("hdnXmlVariableDose");
                Label lblItemName = (Label)row.FindControl("lblItemName");
                HiddenField hdnUnAppPrescriptionId = (HiddenField)row.FindControl("hdnUnAppPrescriptionId");

                ViewState["UnAppPrescriptionId"] = common.myInt(hdnUnAppPrescriptionId.Value).ToString();

                chkCustomMedication.Checked = common.myBool(hdnCustomMedication.Value);
                setCustomMedicationChange();

                if (ItemId.Equals(0) && common.myBool(hdnCustomMedication.Value))
                {
                    txtCustomMedication.Text = common.myStr(lblItemName.Text);
                }
                else
                {
                    try
                    {
                        ddlBrand.Text = lblItemName.Text;
                        ddlBrand.SelectedValue = ItemId.ToString();
                        ddlBrand.Enabled = false;
                        ddlGeneric.SelectedValue = GenericId.ToString();
                        ddlGeneric.Text = GenericName;
                    }
                    catch
                    {
                    }
                }

                hdnItemId.Value = ItemId.ToString();
                hdnGenericId.Value = GenericId.ToString();

                txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                if (ViewState["ItemDetail"] != null)
                {
                    dt = (DataTable)ViewState["ItemDetail"];
                }
                else
                {
                    return;
                }

                DV = new DataView(dt);
                if (ItemId.Equals(0) && common.myBool(hdnCustomMedication.Value))
                {
                    DV.RowFilter = "ISNULL(ItemId,0)=" + ItemId + " AND CustomMedication=1 ";
                }
                else
                {
                    DV.RowFilter = "ISNULL(ItemId,0)=" + ItemId;
                }

                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["StopItemDetail"] = DV.ToTable();
                    HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
                    string xmlSchema = common.myStr(hdnXMLData.Value);
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
                                try
                                {


                                    if (!dsXml.Tables[0].Columns.Contains("XMLVariableDose"))
                                    {
                                        dsXml.Tables[0].Columns.Add("XMLVariableDose", typeof(string));
                                    }
                                }
                                catch
                                {

                                }
                                if (!dsXml.Tables[0].Columns.Contains("XMLFrequencyTime"))
                                {
                                    dsXml.Tables[0].Columns.Add("XMLFrequencyTime", typeof(string));
                                }
                                if (!dsXml.Tables[0].Columns.Contains("IsSubstituteNotAllowed"))
                                {
                                    dsXml.Tables[0].Columns.Add("IsSubstituteNotAllowed", typeof(bool));
                                }
                                if (!dsXml.Tables[0].Columns.Contains("ICDCode"))
                                {
                                    dsXml.Tables[0].Columns.Add("ICDCode", typeof(string));
                                }
                                if (!dsXml.Tables[0].Columns.Contains("Instructions"))
                                {
                                    dsXml.Tables[0].Columns.Add("Instructions", typeof(string));
                                }
                                if (!dsXml.Tables[0].Columns.Contains("Qty"))
                                {
                                    dsXml.Tables[0].Columns.Add("Qty", typeof(string));
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

                    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + ItemId;

                    dtDetail = dvFilter.ToTable();

                    if (dtDetail.Rows.Count > 0)
                    {
                        //ddlFrequencyId.SelectedValue = common.myStr(dtDetail.Rows[0]["FrequencyId"]);
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["FrequencyId"])));
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
                            //ddlPeriodType.SelectedValue = "D";
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                        }
                        else
                        {
                            //ddlPeriodType.SelectedValue = common.myStr(dtDetail.Rows[0]["Type"]);
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["Type"])));
                        }

                        txtInstructions.Text = common.myStr(dtDetail.Rows[0]["Instructions"]);

                        //ddlReferanceItem.SelectedValue = common.myStr(dtDetail.Rows[0]["ReferanceItemId"]);
                        ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["ReferanceItemId"])));

                        //ddlFoodRelation.SelectedValue = common.myStr(dtDetail.Rows[0]["FoodRelationshipID"]);
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["FoodRelationshipID"])));

                        //ddlDoseType.SelectedValue = common.myStr(dtDetail.Rows[0]["DoseTypeId"]);
                        ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["DoseTypeId"])));

                        //ddlUnit.SelectedValue = common.myInt(dtDetail.Rows[0]["UnitId"]).ToString();
                        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["UnitId"]).ToString()));
                        //ddlUnit.Enabled = false;
                        //ddlFormulation.SelectedValue = common.myInt(dtDetail.Rows[0]["FormulationId"]).ToString();
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["FormulationId"]).ToString()));

                        //ddlFrequencyId.SelectedValue = common.myStr(dtDetail.Rows[0]["FrequencyId"]);
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["FrequencyId"])));

                        //ddlRoute.SelectedValue = common.myInt(dtDetail.Rows[0]["RouteId"]).ToString();
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(dtDetail.Rows[0]["RouteId"]).ToString()));

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
                        //ddlVolumeUnit.SelectedValue = common.myStr(dtDetail.Rows[0]["VolumeUnitId"]);
                        ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["VolumeUnitId"])));

                        //ddlTimeUnit.SelectedValue = common.myStr(dtDetail.Rows[0]["TimeUnit"]);
                        ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["TimeUnit"])));

                        txtFlowRateUnit.Text = common.myStr(dtDetail.Rows[0]["FlowRate"]);

                        //ddlFlowRateUnit.SelectedValue = common.myStr(dtDetail.Rows[0]["FlowRateUnit"]);
                        ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue(common.myStr(dtDetail.Rows[0]["FlowRateUnit"])));

                        //hdnXmlVariableDoseString.Value = common.myStr(dtDetail.Rows[0]["XMLVariableDose"]).Trim();
                        hdnXmlVariableDoseString.Value = common.myStr(hdnXmlVariableDose.Value);
                        hdnXmlFrequencyTime.Value = common.myStr(dtDetail.Rows[0]["XMLFrequencyTime"]);
                        chkSubstituteNotAllow.Checked = common.myBool(dtDetail.Rows[0]["IsSubstituteNotAllowed"]);
                        txtICDCode.Text = common.myStr(dtDetail.Rows[0]["ICDCode"]);
                        //txtStartDate.SelectedDate = common.myDate(dtDetail.Rows[0]["StartDate"]);
                        //txtEndDate.SelectedDate = endDateChange(txtStartDate, txtDuration.Text, ddlPeriodType.SelectedValue);
                        //txtEndDate.SelectedDate = common.myDate(dtDetail.Rows[0]["EndDate"]);

                        txtTotQty.Text = common.myDec(dtDetail.Rows[0]["qty"]).ToString("F" + common.myInt(0));
                    }
                }

                BindStopItemDetail();

                dsCalisreq = objPharmacy.ISCalculationRequired(ItemId);
                if (dsCalisreq.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISCalculationRequired"] = common.myBool(dsCalisreq.Tables[0].Rows[0]["CalculationRequired"]);
                }
                ViewState["Edit"] = true;

                endDateChange();
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
            objPharmacy = null;
            dsCalisreq.Dispose();

            dsXml.Dispose();
            dtDetail.Dispose();
            DV.Dispose();
            DV1.Dispose();
            dt.Dispose();
            dvFilter.Dispose();
            tbl.Dispose();
            tbl1.Dispose();
        }
    }
    protected void ibtnFavourite_Click(object sender, EventArgs e)
    {
        if (common.myInt(hdnItemId.Value) > 0)
        {
            string sDoctorId = Request.QueryString["DoctorId"] == null ? common.myStr(Session["LoginDoctorId"]) : common.myStr(Request.QueryString["DoctorId"]);
            RadWindow1.NavigateUrl = "~/EMR/Medication/DrugFavourite.aspx?DoctorId=" + sDoctorId + "&ItemId=" + common.myInt(hdnItemId.Value)
                + "&ItemName=" + ddlBrand.Text + "&GenericId=" + common.myInt(hdnGenericId.Value);
            RadWindow1.Height = 500;
            RadWindow1.Width = 500;
            RadWindow1.Top = 200;
            RadWindow1.Left = 500;
            RadWindow1.OnClientClose = "OnClientCloseFavourite";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please select drug to add favourite", Page);
            return;
        }
    }
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
                        dr["FrequencyName"] = string.Empty;
                        dr["Duration"] = common.myStr(dt1.Rows[i]["Duration"]);
                        dr["Type"] = common.myStr(dt1.Rows[i]["Type"]);
                        dr["DurationText"] = common.myStr(dt1.Rows[i]["DurationText"]);
                        dr["Dose"] = common.myDbl(dt1.Rows[i]["Dose"]);
                        dr["UnitId"] = common.myInt(dt1.Rows[i]["UnitId"]);
                        dr["UnitName"] = string.Empty;
                        dr["FoodRelationshipId"] = 0;
                        dr["FoodRelationship"] = string.Empty;


                        dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(1);
                        dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[i]["Duration"]) - 1) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(common.myInt(dt1.Rows[i]["Duration"]));


                        //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                        //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
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


                    dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(1);
                    dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(common.myInt(dt1.Rows[0]["Duration"]) - 1) : Convert.ToDateTime(dt.Rows[0]["EndDate"]).AddDays(common.myInt(dt1.Rows[0]["Duration"]));


                    //dr["StartDate"] = count == 0 ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[i-1]["EndDate"]).AddDays(1);
                    //dr["EndDate"] = count == 0 ? System.DateTime.Now.AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"])) : Convert.ToDateTime(dt.Rows[i - 1]["EndDate"]).AddDays(Convert.ToInt16(dt1.Rows[i]["Duration"]));
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
            //gvItemDetails.DataSource = dt;
            //gvItemDetails.DataBind();
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
            lblMessage.Text = string.Empty;
            RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(Session["EncounterId"]) + "&PId=" + common.myInt(Session["CurentSavedPrescriptionId"]);
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
    protected void gvItemDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dtPrevious = new DataTable();
        DataTable dt = new DataTable();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadComboBox ddlFrequencyId = (RadComboBox)e.Row.FindControl("ddlFrequencyId");
                if (ViewState["FrequencyData"] != null)
                {
                    dt = (DataTable)ViewState["FrequencyData"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(dr["Description"]);
                        item.Value = common.myStr(common.myInt(dr["Id"]));
                        item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));
                        item.DataBind();
                        ddlFrequencyId.Items.Add(item);
                    }

                }

                RadComboBox ddlFoodRelation = (RadComboBox)e.Row.FindControl("ddlFoodRelation");
                if (ViewState["FoodRelation"] != null)
                {
                    dt = (DataTable)ViewState["FoodRelation"];
                    if (dt.Rows.Count > 0)
                    {
                        ddlFoodRelation.DataSource = dt;
                        ddlFoodRelation.DataValueField = "Id";
                        ddlFoodRelation.DataTextField = "FoodName";
                        ddlFoodRelation.DataBind();
                        ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
                        ddlFoodRelation.Items[0].Value = "0";
                    }
                }
                RadComboBox ddlUnit = (RadComboBox)e.Row.FindControl("ddlUnit");
                if (ViewState["UnitMaster"] != null)
                {
                    dt = (DataTable)ViewState["UnitMaster"];
                    ddlUnit.DataSource = dt;
                    ddlUnit.DataValueField = "Id";
                    ddlUnit.DataTextField = "UnitName";
                    ddlUnit.DataBind();
                    ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
                    ddlUnit.Items[0].Value = "0";
                    //ddlUnit.SelectedValue = "0";
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));
                }

                RadComboBox ddlDoseType = (RadComboBox)e.Row.FindControl("ddlDoseType");
                if (ViewState["DoseType"] != null)
                {
                    dt = (DataTable)ViewState["DoseType"];
                    ddlDoseType.DataSource = dt;
                    ddlDoseType.DataValueField = "StatusId";
                    ddlDoseType.DataTextField = "Status";
                    ddlDoseType.DataBind();
                    ddlDoseType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
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


                if (ViewState["Item"] != null)
                {
                    dtPrevious = (DataTable)ViewState["Item"];
                    foreach (DataRow dr in dtPrevious.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(dr["ItemName"]);
                        item.Value = common.myStr(common.myInt(dr["ItemId"]));

                        if (common.myBool(dr["IsInfusion"]))
                        {
                            item.Attributes.Add("IsInfusion", common.myStr(dr["IsInfusion"]));
                        }
                        else if (common.myBool(dr["IsInjection"]))
                        {
                            item.Attributes.Add("IsInjection", common.myStr(dr["IsInjection"]));
                        }

                        item.DataBind();
                        ddlReferanceItem.Items.Add(item);
                    }
                }

                if (ViewState["ItemDetail"] != null && ViewState["Edit"] != null)
                {
                    dt = (DataTable)ViewState["ItemDetail"];
                    if (dt != null && count < dt.Rows.Count)
                    {
                        //ddlFrequencyId.SelectedValue = common.myStr(dt.Rows[count]["FrequencyId"]);
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myStr(dt.Rows[count]["FrequencyId"])));

                        //ddlFoodRelation.SelectedValue = common.myStr(dt.Rows[count]["FoodRelationshipId"]);
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myStr(dt.Rows[count]["FoodRelationshipId"])));

                        //ddlPeriodType.SelectedValue = common.myStr(dt.Rows[count]["Type"]);
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(dt.Rows[count]["Type"])));

                        //ddlUnitId.SelectedValue = common.myStr(dt.Rows[count]["UnitId"]);
                        ddlUnitId.SelectedIndex = ddlUnitId.Items.IndexOf(ddlUnitId.Items.FindItemByValue(common.myStr(dt.Rows[count]["UnitId"])));

                        //ddlReferanceItem.SelectedValue = common.myStr(dt.Rows[count]["ReferanceItemId"]);
                        ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue(common.myStr(dt.Rows[count]["ReferanceItemId"])));

                        //ddlFoodRelation.SelectedValue = common.myStr(dt.Rows[count]["FoodRelationshipID"]);
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myStr(dt.Rows[count]["FoodRelationshipID"])));

                        txtInstructions.Text = common.myStr(dt.Rows[count]["Instructions"]);

                        //ddlDoseType.SelectedValue = common.myStr(dt.Rows[count]["DoseTypeId"]);
                        ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myStr(dt.Rows[count]["DoseTypeId"])));
                    }

                }
                else if (ViewState["DataTableDetail"] != null)
                {
                    dt = (DataTable)ViewState["DataTableDetail"];
                    if (dt != null && count < dt.Rows.Count)
                    {
                        //ddlFrequencyId.SelectedValue = common.myStr(dt.Rows[count]["FrequencyId"]);
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myStr(dt.Rows[count]["FrequencyId"])));

                        //ddlFoodRelation.SelectedValue = common.myStr(dt.Rows[count]["FoodRelationshipId"]);
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myStr(dt.Rows[count]["FoodRelationshipId"])));

                        //ddlPeriodType.SelectedValue = common.myStr(dt.Rows[count]["Type"]);
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(dt.Rows[count]["Type"])));

                        //ddlUnitId.SelectedValue = common.myStr(dt.Rows[count]["UnitId"]);
                        ddlUnitId.SelectedIndex = ddlUnitId.Items.IndexOf(ddlUnitId.Items.FindItemByValue(common.myStr(dt.Rows[count]["UnitId"])));

                        //ddlReferanceItem.SelectedValue = common.myStr(dt.Rows[count]["ReferanceItemId"]) == string.Empty ? "0" : common.myStr(dt.Rows[count]["ReferanceItemId"]);
                        ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue(common.myStr(dt.Rows[count]["ReferanceItemId"]) == string.Empty ? "0" : common.myStr(dt.Rows[count]["ReferanceItemId"])));

                        //ddlFoodRelation.SelectedValue = common.myStr(dt.Rows[count]["FoodRelationshipID"]);
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myStr(dt.Rows[count]["FoodRelationshipID"])));

                        txtInstructions.Text = common.myStr(dt.Rows[count]["Instructions"]);

                        //ddlDoseType.SelectedValue = common.myStr(dt.Rows[count]["DoseTypeId"]);
                        ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue(common.myStr(dt.Rows[count]["DoseTypeId"])));
                    }
                }
                count++;
                if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO")
                {
                    ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    ddlFrequencyId.Items[0].Value = "0";
                    //ddlFrequencyId.SelectedValue = "0";
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));
                    ddlFrequencyId.Enabled = false;

                    ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    ddlRoute.Items[0].Value = "0";


                    ddlPeriodType.Enabled = false;
                    //txtDose.Text = "1";
                    txtDose.Enabled = false;
                    //txtDuration.Text = "1";
                    txtDuration.Enabled = false;
                    txtStartDate.Enabled = false;
                    txtEndDate.Enabled = false;
                    ddlDoseType.Enabled = false;
                    ddlUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    ddlUnit.Items[0].Value = "0";
                    ddlUnit.Enabled = false;
                    ddlReferanceItem.Enabled = false;
                    ddlFoodRelation.Enabled = false;
                    lnkAddNewRow.Enabled = false;

                }
                else if (ddlDoseType.SelectedValue != "0")
                {
                    ddlPeriodType.Enabled = false;
                    txtDose.Enabled = true;
                    ddlReferanceItem.Enabled = false;

                    ddlFrequencyId.Enabled = false;
                    ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                    ddlFrequencyId.Items[0].Value = "0";
                    //ddlFrequencyId.SelectedValue = "0";
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));

                    txtDuration.Enabled = false;
                    txtStartDate.Enabled = true;
                    ddlDoseType.Enabled = true;
                    ddlUnit.Enabled = true;
                    ddlFoodRelation.Enabled = true;
                    lnkAddNewRow.Enabled = true;
                }
                else
                {
                    ddlReferanceItem.Enabled = true;
                    ddlPeriodType.Enabled = true;
                    txtDose.Enabled = true;
                    txtDuration.Enabled = true;
                    txtStartDate.Enabled = true;
                    ddlDoseType.Enabled = true;
                    ddlUnit.Enabled = true;

                    ddlFoodRelation.Enabled = true;
                    lnkAddNewRow.Enabled = true;

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
            dtPrevious.Dispose();
        }
    }
    protected void BindPatientProvisionalDiagnosis()
    {
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
                txtProvisionalDiagnosis.ToolTip = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
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
            objDiag = null;
            ds.Dispose();
        }
    }
    protected void gvItemDetails_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        DataView DV = new DataView();
        DataTable tbl = new DataTable();
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

                int VIDALItemId = common.myInt(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

                if (ItemId > 0)
                {
                    //  ddlFormulation.Enabled = false;
                    //ddlRoute.Enabled = false;

                    //ddlStrength.Enabled = false;
                    //txtStrengthValue.Enabled = false;
                }
                else
                {
                    ddlFormulation.Enabled = true;
                    ddlRoute.Enabled = true;

                    //ddlStrength.Enabled = true;
                    txtStrengthValue.Enabled = true;
                }

                hdnGenericId.Value = common.myStr(GenericId);
                hdnItemId.Value = common.myStr(ItemId);

                hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                hdnCIMSType.Value = common.myStr(CIMSType);

                hdnVIDALItemId.Value = common.myStr(VIDALItemId);

                Label lblGenericName = (Label)row.FindControl("lblGenericName");
                Label lblItemName = (Label)row.FindControl("lblItemName");
                Label lblTotalQty = (Label)row.FindControl("lblTotalQty");

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

                hdnGenericName.Value = common.myStr(lblGenericName.Text);
                hdnItemName.Value = common.myStr(lblItemName.Text);
                lblGenericName.Text = common.myStr(lblGenericName.Text);
                //txtTotQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));

                //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));

                txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

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
                    tbl = (DataTable)ViewState["DataTableDetail"];

                    DV = tbl.Copy().DefaultView;
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

                    //gvItemDetails.DataSource = tbl;
                    //gvItemDetails.DataBind();

                    //setVisiblilityInteraction();
                }
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            //else if (e.CommandName == "DHInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("A");
            //}
            //else if (e.CommandName == "BrandDetailsVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getBrandDetailsVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "MonographVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getMonographVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "InteractionVIDAL")
            //{
            //    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds().ToArray();

            //    if (commonNameGroupIds.Length > 0)
            //    {
            //        getDrugToDrugInteractionVidal(commonNameGroupIds);
            //    }
            //}
            //else if (e.CommandName == "DHInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("A");
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
            DV.Dispose();
            tbl.Dispose();
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
                e.Row.Cells[(byte)enumCurrent.PrescriptionDetail].Visible = false;
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
                e.Row.Cells[(byte)enumCurrent.PrescriptionDetail].Visible = false;
                e.Row.Cells[(byte)enumCurrent.StartDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.EndDate].Visible = false;
                e.Row.Cells[(byte)enumCurrent.StopRemarks].Visible = false;

                gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = false;
                gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = false;

                //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = false;

                //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = false;
                //gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = false;


                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = true;
                    gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = true;

                    //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = true;                    
                    //gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = true;
                    //gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Row.FindControl("hdnCIMSItemId");
                    ImageButton lnkBtnMonographCIMS = (ImageButton)e.Row.FindControl("lnkBtnMonographCIMS");
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)e.Row.FindControl("lnkBtnInteractionCIMS");

                    //LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsCIMS");                   
                    //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionCIMS");
                    //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                    //lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));                    
                    //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0
                        || common.myStr(hdnCIMSItemId.Value).Trim() == "0")
                    {
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;

                        //lnkBtnBrandDetailsCIMS.Visible = false;                        
                        //lnkBtnDHInteractionCIMS.Visible = false;
                        //lnkBtnDAInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        //lnkBtnBrandDetailsCIMS.Visible = false;

                        HiddenField hdnCIMSType = (HiddenField)e.Row.FindControl("hdnCIMSType");

                        string strXML = string.Empty;

                        //if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        //{
                        //    strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        //    if (strXML != string.Empty)
                        //    {
                        //        string outputValues = objCIMS.getFastTrack5Output(strXML);

                        //        if (outputValues != null)
                        //        {
                        //            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" name=";
                        //            if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                        //            {
                        //                lnkBtnBrandDetailsCIMS.Visible = true;
                        //            }
                        //        }
                        //    }
                        //}

                        lnkBtnMonographCIMS.Visible = false;
                        strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                        if (strXML != string.Empty)
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
                //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //{
                //    gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = true;
                //    gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = true;
                //    gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = true;
                //    gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = true;
                //    gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = true;

                //    HiddenField hdnVIDALItemId = (HiddenField)e.Row.FindControl("hdnVIDALItemId");
                //    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Row.FindControl("lnkBtnBrandDetailsVIDAL");
                //    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Row.FindControl("lnkBtnMonographVIDAL");
                //    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnInteractionVIDAL");
                //    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDHInteractionVIDAL");
                //    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Row.FindControl("lnkBtnDAInteractionVIDAL");

                //    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                //    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                //    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                //    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                //    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                //    lnkBtnBrandDetailsVIDAL.Visible = true;
                //    lnkBtnMonographVIDAL.Visible = true;
                //    lnkBtnInteractionVIDAL.Visible = true;
                //    lnkBtnDHInteractionVIDAL.Visible = true;
                //    //lnkBtnDAInteractionVIDAL.Visible = true;

                //    if (common.myInt(hdnVIDALItemId.Value) == 0)
                //    {
                //        lnkBtnBrandDetailsVIDAL.Visible = false;
                //        lnkBtnMonographVIDAL.Visible = false;
                //        lnkBtnInteractionVIDAL.Visible = false;
                //        lnkBtnDHInteractionVIDAL.Visible = false;
                //        lnkBtnDAInteractionVIDAL.Visible = false;
                //    }
                //}

                if (!chkShowDetails.Checked)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = false;
                        gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = false;

                        //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = false;                        
                        //gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = false;
                        //gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = false;
                    }
                    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = false;
                    //    gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = false;
                    //    gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = false;
                    //    gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = false;
                    //    gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = false;
                    //}
                }

                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
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
                    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value) + " AND ISNULL(ItemId,0)=" + common.myInt(hdnGDItemId.Value);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringV2(dv.ToTable());
                        lnkItemName.ToolTip = lblPrescriptionDetail.Text;
                    }
                    else
                    {
                        dv1 = new DataView(dt);
                        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            lblPrescriptionDetail.Text = objEMR.GetPrescriptionDetailStringV2(dv1.ToTable());
                            lnkItemName.ToolTip = lblPrescriptionDetail.Text;
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
                dt.Dispose();
                dv.Dispose();
                dv1.Dispose();
            }
        }
    }
    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName == "selectItem")
            {
                chkCustomMedication.Checked = false;
                setCustomMedicationChange();

                //  GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                GridViewRow row = gvPrevious.Rows[common.myInt(hdnFavouriteIndex.Value)];


                int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);
                int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);

                if (ItemId == 0 && GenericId == 0)
                {
                    return;
                }

                string CIMSItemId = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                string CIMSType = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);

                int VIDALItemId = common.myInt(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

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
                    txtStrengthValue.Enabled = true;
                }

                hdnGenericId.Value = common.myStr(GenericId);
                hdnItemId.Value = common.myStr(ItemId);

                ////hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                ////hdnCIMSType.Value = common.myStr(CIMSType);

                ////hdnVIDALItemId.Value = common.myStr(VIDALItemId);

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
                // HiddenField hdnIsInfusion = (HiddenField)row.FindControl("hdnIsInfusion");

                try
                {
                    //ddlBrand.Text = common.myStr(lnkItemName.Text);
                    //ddlBrand.SelectedValue = ItemId.ToString();

                    if (!ItemId.Equals(0))
                    {
                        ddlBrand.Text = common.myStr(lnkItemName.Text);
                        ddlBrand.SelectedValue = ItemId.ToString();
                    }
                    else if (!GenericId.Equals(0))
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
                hdnItemName.Value = common.myStr(lnkItemName.Text);
                lblGenericName.Text = common.myStr(GenericName.Text);
                //txtTotQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                //txtDose.Text = "1";
                if (common.myStr(Session["OPIP"]) == "I")
                {
                    //txtDuration.Text = "1";
                }

                setDefaultFavourite(ItemId);

                //ddlFrequency.SelectedIndex = ddlFrequency.Items.IndexOf(ddlFrequency.Items.FindItemByValue(common.myStr(hdnFrequencyId.Value)));
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
            else if (e.CommandName == "ItemStop") // || e.CommandName == "ItemCancel")
            {
                //int ItemId = common.myInt(e.CommandArgument);
                int ItemId = common.myInt(hdnFavouriteItemId.Value);
                int GenericId = common.myInt(hdnFavouriteGenericId.Value);
                //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow row = gvPrevious.Rows[common.myInt(hdnFavouriteIndex.Value)];


                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnIndentDetailsId = (HiddenField)row.FindControl("hdnIndentDetailsId");
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                if ((hdnIndentId.Value == string.Empty || hdnIndentId.Value == "0") && ItemId == 0)
                {
                    return;
                }
                ViewState["IndentDetailsId"] = common.myInt(hdnIndentDetailsId.Value).ToString();
                ViewState["StopItemId"] = ItemId.ToString();
                ViewState["StopGenericId"] = common.myStr(GenericId);
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                //Hashtable hshOutput = new Hashtable();

                if (e.CommandName == "ItemStop")//Stop
                {
                    txtStopRemarks.Text = string.Empty;
                    dvConfirmStop.Visible = true;

                    //hshOutput = emr.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    //               common.myInt(hdnIndentId.Value), ItemId, common.myInt(Session["UserId"]), common.myInt(Session["RegistrationId"]),
                    //               common.myInt(Session["EncounterId"]), 0, txtRemarks.Text, common.myStr(Session["OPIP"]));

                    //lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
                }
                else//Cancel
                {
                    //hshOutput = emr.StopCancelPreviousMedication(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),
                    //              Convert.ToInt32(hdnIndentId.Value), ItemId, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["RegistrationId"]),
                    //              Convert.ToInt32(Session["EncounterId"]), 1, txtRemarks.Text, Session["OPIP"].ToString());
                }


                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindGrid(string.Empty, string.Empty);
                //setVisiblilityInteraction();
            }
            else if (e.CommandName.Equals("PRINT"))
            {
                GridViewRow row = gvPrevious.Rows[common.myInt(hdnFavouriteIndex.Value)];
                HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");


                //int intPrescriptionId = common.myInt(e.CommandArgument);
                //if (intPrescriptionId.Equals(0)   )
                //{
                //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //    lblMessage.Text = "Prescription not Selected !";
                //    return;
                //}


                //int intPrescriptionId = common.myInt(hdnItemId.Value);
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);




                int intPrescriptionId = common.myInt(hdnIndentId.Value);
                if (intPrescriptionId.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Prescription not Selected !";
                    return;
                }


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
            else if (e.CommandName == "BrandDetailsCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)row.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            //else if (e.CommandName == "DHInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionCIMS")
            //{
            //    ViewState["NewPrescribing"] = string.Empty;
            //    showHealthOrAllergiesIntreraction("A");
            //}
            //else if (e.CommandName == "BrandDetailsVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getBrandDetailsVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "MonographVIDAL")
            //{
            //    if (common.myInt(e.CommandArgument) > 0)
            //    {
            //        getMonographVidal((int?)common.myInt(e.CommandArgument));
            //    }
            //}
            //else if (e.CommandName == "InteractionVIDAL")
            //{
            //    int?[] commonNameGroupIds = getVIDALCommonNameGroupIds().ToArray();

            //    if (commonNameGroupIds.Length > 0)
            //    {
            //        getDrugToDrugInteractionVidal(commonNameGroupIds);
            //    }
            //}
            //else if (e.CommandName == "DHInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("H");
            //}
            //else if (e.CommandName == "DAInteractionVIDAL")
            //{
            //    showHealthOrAllergiesIntreraction("A");
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
            emr = null;
        }
    }
    protected void lstFavourite_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllFavourite('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //HiddenField hdnCalculationBase = (HiddenField)e.Row.FindControl("hdnCalculationBase");
            if (e.Row.RowIndex.Equals(0))
            {
                LinkButton lnkItemName = e.Row.FindControl("lnkItemName") as LinkButton;
                lnkItemName.Focus();
            }
        }
    }
    protected void btnRefresh_OnClick(object sender, System.EventArgs e)
    {
        BindGrid(common.myStr(hdnReturnIndentIds.Value), common.myStr(hdnReturnItemIds.Value));
    }
    protected void btnGetFavourite_OnClick(object sender, System.EventArgs e)
    {
        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            try
            {
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
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dsSearch.Tables[0].Rows[i]["ItemName"];
                    item.Value = common.myStr(dsSearch.Tables[0].Rows[i]["ItemId"]);
                    item.Attributes.Add("ClosingBalance", common.myStr(dsSearch.Tables[0].Rows[i]["ClosingBalance"]));
                    item.Attributes.Add("CIMSItemId", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSItemId"]));
                    item.Attributes.Add("CIMSType", common.myStr(dsSearch.Tables[0].Rows[i]["CIMSType"]));
                    item.Attributes.Add("VIDALItemId", common.myInt(dsSearch.Tables[0].Rows[i]["VIDALItemId"]).ToString());

                    this.ddlBrand.Items.Add(item);
                    item.DataBind();
                }
            }
            try
            {
                ddlBrand.SelectedValue = hdnItemId.Value;
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
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    private void ShowPatientDetails()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Weight.Text = string.Empty;
                txtHeight.Text = string.Empty;
                //lbl_BSA.Text = "";
                for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == "WT")// Weight
                    {
                        lbl_Weight.Text = ds.Tables[0].Rows[i][1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i][0].ToString() == "HT")// Height
                    {
                        txtHeight.Text = ds.Tables[0].Rows[i][1].ToString();
                    }
                    //else if (ds.Tables[0].Rows[i][0].ToString() == "BSA")
                    //{
                    //    lbl_BSA.Text = ds.Tables[0].Rows[i][1].ToString();
                    //}
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
            ds.Dispose();
        }
    }
    protected void BindGrid(string IdentId, string ItemId)
    {
        //if (rdoSearchMedication.SelectedValue != "C")
        //{
        //    return;
        //}

        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dvConsumable = new DataView();
        DataView dvDistinct = new DataView();
        DataTable tblDistinct = new DataTable();
        DataTable dtData = new DataTable();
        try
        {
            ViewState["Mode"] = IdentId == string.Empty && ItemId == string.Empty ? "P" : ViewState["Mode"];
            if (common.myStr(Session["OPIP"]) == "I")
            {
                ds = objPharmacy.getPreviousMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                common.myInt(Session["EncounterId"]), 0, 0, ViewState["Mode"] == null ? "P" : common.myStr(ViewState["Mode"]),
                                common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty);
            }
            else
            {
                if (common.myLen(hdnReturnIndentDetailsIds.Value) > 0 && IdentId != string.Empty && ItemId != string.Empty)
                {
                    ds = objEMR.getPreviousMedicationOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, "P",
                                    common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty, 0);
                }
                else
                {

                    ds = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, ViewState["Mode"] == null ? "P" : common.myStr(ViewState["Mode"]),
                                    common.myStr(txtCurrentItemName.Text).Trim(), string.Empty, string.Empty);
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                dv = new DataView(ds.Tables[0]);
                dv1 = new DataView(ds.Tables[1]);
                dvConsumable = new DataView();

                if (IdentId != string.Empty && ItemId != string.Empty)
                {
                    if (common.myLen(hdnReturnIndentDetailsIds.Value) > 0)
                    {
                        ViewState["Stop"] = true;
                        //dv.RowFilter = " ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";
                        //dv1.RowFilter = " ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";
                        dv.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ") AND Source IN(" + common.myStr(hdnReturnIndentOPIPSource.Value) + ")";
                        dv1.RowFilter = "ISNULL(DetailsId,0) IN (" + common.myStr(hdnReturnIndentDetailsIds.Value) + ") AND ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";




                    }
                    else
                    {
                        dv.RowFilter = "ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";
                        dv1.RowFilter = "ISNULL(ItemId,0) IN (" + ItemId + ") AND ISNULL(IndentId,0) IN (" + IdentId + ")";
                    }
                    DataTable dtfind = dv.ToTable();
                    if (dtfind.Rows.Count > 0)
                    {
                        for (int ifind = 0; ifind < dtfind.Rows.Count; ifind++)
                        {
                            if (CheckDDCExpiry(common.myInt(dtfind.Rows[ifind]["ItemId"])))
                            {
                                dv.Sort = "ItemId";
                                dv1.Sort = "ItemId";
                                int dvindex = dv.Find(common.myInt(dtfind.Rows[ifind]["ItemId"]));
                                int dv1index = dv1.Find(common.myInt(dtfind.Rows[ifind]["ItemId"]));
                                if (dvindex >= 0)
                                {
                                    dv.Table.Rows[dvindex].Delete();

                                }
                                if (dv1index >= 0)
                                {
                                    dv1.Table.Rows[dv1index].Delete();
                                }
                            }
                        }
                    }



                    DataTable dt = dv.ToTable();
                    DataTable dt1 = dv1.ToTable();
                    int duration = 1, Dose = 0;
                    if (IdentId != string.Empty && ItemId != string.Empty)
                    {
                        try
                        {


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

                                Dose = Dose + common.myInt(dt1.Rows[idx]["Dose"]);

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
                            DataTable tblReorderOverrideComments = (DataTable)Session["TblReorderOverrideComments"];
                            if (tblReorderOverrideComments != null)
                            {
                                if (ViewState["Item"] != null)
                                {
                                    DataTable tblDataItemDetails = (DataTable)ViewState["Item"];
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
                        DataTable dtDataItem = (DataTable)ViewState["Item"];
                        dtDataItem = addColumnInItemGrid(dtDataItem);
                        ViewState["Item"] = dtDataItem;

                        dtData = (DataTable)ViewState["ItemDetail"];
                        dtData = addColumnInItemGrid(dtData);
                        ViewState["ItemDetail"] = dtData;
                        // DataTable dtfinal = new DataTable();
                        if (common.myBool(dtData.Rows[0]["IsVariableDose"]) == true)
                        {
                            string Type = "";
                            if (common.myStr(dtData.Rows[0]["Type"]) == "D")
                            {
                                Type = " Day(s)";
                            }
                            else if (common.myStr(dtData.Rows[0]["Type"]) == "W")
                            {
                                Type = " Week(s)";
                            }
                            else if (common.myStr(dtData.Rows[0]["Type"]) == "Y")
                            {
                                Type = " Year(s)";
                            }
                            dtData.Rows[0]["Dose"] = Dose;
                            dtData.Rows[0]["Duration"] = duration;
                            dtData.Rows[0]["DurationText"] = duration + Type;
                            dtData.Rows[0]["IsVariableDose"] = false;
                            if (common.myInt(dtData.Rows[0]["UnAppPrescriptionId"]).Equals(0))
                            {
                                int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(dtData.Rows[0]);

                                dtData.Rows[0]["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                            }
                            dv = new DataView(dtData);
                            dv.RowFilter = "ISNULL(Dose,0)=" + common.myInt(Dose) + " AND ISNULL(Duration,0)=" + common.myInt(duration);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                dtData = dv.ToTable();
                                dtData.Rows[0]["PrescriptionDetail"] = objEMR.GetPrescriptionDetailStringV2(dv.ToTable());
                            }
                        }
                        else
                        {
                            foreach (DataRow item in dtData.Rows)
                            {


                                if (common.myInt(item["UnAppPrescriptionId"]).Equals(0))
                                {
                                    int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(item);

                                    item["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
                                }
                                dv = new DataView(dtData);
                                dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(item["IndentId"]) + " AND ISNULL(ItemId,0)=" + common.myInt(item["ItemId"]);
                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    item["PrescriptionDetail"] = objEMR.GetPrescriptionDetailStringV2(dv.ToTable());

                                    //santosh
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }


                    ViewState["Item"] = dtData;
                    ViewState["ItemDetail"] = dtData;
                    gvItem.DataSource = ViewState["Item"];
                    gvItem.DataBind();
                }
                else
                {
                    ViewState["GridDataItem"] = ds.Tables[0];
                    ViewState["GridDataDetail"] = ds.Tables[1];
                    if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO")
                    {
                        gvPrevious.Visible = false;
                    }
                    else
                    {
                        if (ViewState["Consumable"] != null && common.myBool(ViewState["Consumable"]))
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF','OMC')";
                            dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 ";
                        }
                        else
                        {
                            dvConsumable = new DataView(ds.Tables[0]);
                            //dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED','NUT')";
                            dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1 ";
                        }

                        //remove repeated drug
                        tblDistinct = dvConsumable.ToTable().Clone();

                        foreach (DataRow item in dvConsumable.ToTable().Rows)
                        {
                            dvDistinct = tblDistinct.Copy().DefaultView;

                            dvDistinct.RowFilter = "ItemName='" + common.myStr(item["ItemName"], true) + "'";
                            //    dvDistinct.RowFilter = "ItemName LIKE '%" + common.myStr(item["ItemName"]) + "%'" ;

                            if (dvDistinct.Count == 0)
                            {
                                dvDistinct.RowFilter = string.Empty;
                                tblDistinct.ImportRow(item);
                            }
                        }
                        //var currentMedicationList = new List<CurrentMedicationItem>();
                        //foreach (DataRow dr in tblDistinct.Rows)
                        //{
                        //    var cm = new CurrentMedicationItem()
                        //    {
                        //        ItemId = dr["ItemId"].ToString(),
                        //        ItemName = dr["ItemName"].ToString().Replace("'", "")
                        //    };
                        //    currentMedicationList.Add(cm);
                        //}


                        gvPrevious.DataSource = tblDistinct;
                        gvPrevious.DataBind();
                        //if (currentMedicationList.Count > 0)
                        //{
                        //    Session["currentMedication"] = currentMedicationList;
                        //}

                        setVisiblilityInteraction();
                    }
                }

            }
            else
            {
                if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO")
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
        finally
        {
            objPharmacy = null;
            objEMR = null;
            ds.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dvConsumable.Dispose();
            dvDistinct.Dispose();
            tblDistinct.Dispose();
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

        return dt;
    }
    //private DataTable CreateItemDetailTable()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("Id", typeof(int));
    //    dt.Columns.Add("IndentTypeId", typeof(int));
    //    dt.Columns.Add("IndentType", typeof(string));
    //    dt.Columns.Add("GenericId", typeof(int));
    //    dt.Columns.Add("GenericName", typeof(string));
    //    dt.Columns.Add("ItemId", typeof(int));
    //    dt.Columns.Add("IndentId", typeof(int));
    //    dt.Columns.Add("ItemName", typeof(string));
    //    dt.Columns.Add("FormulationId", typeof(int));
    //    dt.Columns.Add("RouteId", typeof(int));
    //    dt.Columns.Add("RouteName", typeof(string));
    //    dt.Columns.Add("StrengthId", typeof(int));
    //    dt.Columns.Add("CIMSItemId", typeof(string));
    //    dt.Columns.Add("CIMSType", typeof(string));
    //    dt.Columns.Add("VIDALItemId", typeof(string));
    //    dt.Columns.Add("Frequency", typeof(decimal));
    //    dt.Columns.Add("FrequencyName", typeof(string));
    //    dt.Columns.Add("FrequencyId", typeof(int));
    //    dt.Columns.Add("Duration", typeof(string));
    //    dt.Columns.Add("Type", typeof(string));
    //    dt.Columns.Add("DurationText", typeof(string));
    //    dt.Columns.Add("Dose", typeof(string));
    //    dt.Columns.Add("UnitId", typeof(int));
    //    dt.Columns.Add("UnitName", typeof(string));
    //    dt.Columns.Add("FoodRelationshipId", typeof(int));
    //    dt.Columns.Add("FoodRelationship", typeof(string));
    //    dt.Columns.Add("StartDate", typeof(string));
    //    dt.Columns.Add("EndDate", typeof(string));
    //    dt.Columns.Add("Qty", typeof(decimal));
    //    dt.Columns.Add("PrescriptionDetail", typeof(string));
    //    dt.Columns.Add("ReferanceItemId", typeof(int));
    //    dt.Columns.Add("ReferanceItemName", typeof(string));
    //    dt.Columns.Add("Instructions", typeof(string));
    //    dt.Columns.Add("DoseTypeId", typeof(int));
    //    dt.Columns.Add("DoseTypeName", typeof(string));
    //    dt.Columns.Add("CustomMedication", typeof(bool));
    //    dt.Columns.Add("NotToPharmacy", typeof(bool));
    //    dt.Columns.Add("XMLData", typeof(String));
    //    dt.Columns.Add("IsInfusion", typeof(bool));
    //    dt.Columns.Add("IsInjection", typeof(bool));

    //    dt.Columns.Add("Volume", typeof(string));
    //    dt.Columns.Add("VolumeUnitId", typeof(int));
    //    dt.Columns.Add("InfusionTime", typeof(string));
    //    dt.Columns.Add("TimeUnit", typeof(string));
    //    dt.Columns.Add("TotalVolume", typeof(string));
    //    dt.Columns.Add("FlowRate", typeof(string));
    //    dt.Columns.Add("FlowRateUnit", typeof(int));

    //    dt.Columns.Add("XMLVariableDose", typeof(String));



    //    return dt;

    //}
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
        dr["NotToPharmacy"] = 0;
        dr["IsInfusion"] = false;
        dr["IsInjection"] = false;

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
        dr["NotToPharmacy"] = 0;
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
    //private void BindBlankItemDetailGrid()
    //{
    //    DataTable dt = CreateItemDetailTable();
    //    DataRow dr = dt.NewRow();
    //    dr["Id"] = 1;
    //    dr["IndentTypeId"] = 0;
    //    dr["IndentType"] = string.Empty;
    //    dr["GenericId"] = 0;
    //    dr["ItemId"] = 0;
    //    dr["IndentId"] = 0;
    //    dr["GenericName"] = string.Empty;
    //    dr["ItemName"] = string.Empty;
    //    dr["FormulationId"] = 0;
    //    dr["RouteId"] = 0;
    //    dr["RouteName"] = string.Empty;
    //    dr["StrengthId"] = 0;
    //    dr["CIMSItemId"] = string.Empty;
    //    dr["CIMSType"] = string.Empty;
    //    dr["VIDALItemId"] = string.Empty;
    //    dr["Frequency"] = 0.00;
    //    dr["FrequencyId"] = DBNull.Value;
    //    dr["FrequencyName"] = string.Empty;
    //    dr["Duration"] = string.Empty;
    //    dr["Type"] = "D";
    //    dr["DurationText"] = string.Empty;
    //    dr["Dose"] = string.Empty;
    //    dr["UnitId"] = 0;
    //    dr["UnitName"] = string.Empty;
    //    dr["FoodRelationshipId"] = DBNull.Value;
    //    dr["FoodRelationship"] = string.Empty;
    //    dr["StartDate"] = System.DateTime.Now;
    //    dr["EndDate"] = System.DateTime.Now;
    //    dr["Qty"] = 0.00;
    //    dr["PrescriptionDetail"] = string.Empty;
    //    dr["ReferanceItemId"] = DBNull.Value;
    //    dr["ReferanceItemName"] = string.Empty;
    //    dr["Instructions"] = string.Empty;
    //    dr["DoseTypeId"] = 0;
    //    dr["DoseTypeName"] = string.Empty;
    //    dr["XMLData"] = string.Empty;
    //    dr["CustomMedication"] = 0;
    //    dr["IsInfusion"] = false;
    //    dr["IsInjection"] = false;

    //    dr["Volume"] = "";
    //    dr["VolumeUnitId"] = 0;
    //    dr["InfusionTime"] = "";
    //    dr["TimeUnit"] = "";
    //    dr["TotalVolume"] = "";
    //    dr["FlowRate"] = "";
    //    dr["FlowRateUnit"] = 0;
    //    dr["XMLVariableDose"] = "";

    //    dt.Rows.Add(dr);
    //    //gvItemDetails.DataSource = dt;
    //    //gvItemDetails.DataBind();
    //    ViewState["DataTableDetail"] = dt;
    //}
    private void clearItemDetails()
    {
        try
        {
            lblGenericName.Text = string.Empty;
            txtTotQty.Text = "0";
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;
            //ddlStrength.SelectedIndex = 0;
            txtStrengthValue.Text = string.Empty;
            //txtDose.Text = "";
            // ddlFrequency.SelectedIndex = 0;
            // txtDays.Text = "";
            // ddlPeriodType.SelectedIndex = 0;
            //txtStartDate.SelectedDate = DateTime.Now;
            // txtEndDate.SelectedDate = DateTime.Now;
            //txtRemarks.Text = "";
            //txtICDCode.Text = string.Empty;
        }
        catch
        {
        }
    }

    private void clearControls()
    {
        try
        {
            lblGenericName.Text = string.Empty;
            txtTotQty.Text = "0";
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;

            txtStrengthValue.Text = string.Empty;
            txtDose.Text = "";

            ddlPeriodType.SelectedIndex = 0;

            txtInstructions.Text = "";
            ddlUnit.SelectedIndex = 0;
            ddlFrequencyId.SelectedIndex = 0;
            txtDuration.Text = string.Empty;
            ddlFoodRelation.SelectedIndex = 0;
            chkSubstituteNotAllow.Checked = false;
            txtStrengthValue.Text = string.Empty;
            ddlFormulation.SelectedIndex = 0;
            ddlRoute.SelectedIndex = 0;

        }
        catch
        {
        }
    }
    private void getCurrentICDCodes()
    {
        try
        {
            DataSet ds = new DataSet();

            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            ds = order.GetPatientDiagnosis(common.myInt(Session["RegistrationID"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EncounterId"]));
            string sICDCodes = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (sICDCodes == string.Empty)
                    {
                        sICDCodes += common.myStr(ds.Tables[0].Rows[i]["ICDCode"]);
                    }
                    else
                    {
                        sICDCodes += "," + common.myStr(ds.Tables[0].Rows[i]["ICDCode"]);
                    }
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ICDCodes"] = sICDCodes;
                txtICDCode.Text = sICDCodes;
            }
            else
            {
                ViewState["ICDCodes"] = null;
                txtICDCode.Text = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindICDPanel()
    {
        try
        {
            DataSet dsTemp = new DataSet();
            if (ViewState["ICDCodes"] != null)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("ICDCodes");
                dt.Columns.Add("Description");
                dt.Columns["ID"].AutoIncrement = true;
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;

                char[] chArray = { ',' };
                string[] serviceIdXml = common.myStr(ViewState["ICDCodes"]).Split(chArray);
                DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                foreach (string item in serviceIdXml)
                {
                    DataRow drdt = dt.NewRow();
                    BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                    dsTemp = order.GetICDCode(item.ToString());
                    if (dsTemp.Tables.Count > 0)
                    {
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            drdt["ICDCodes"] = item.ToString();
                            drdt["Description"] = common.myStr(dsTemp.Tables[0].Rows[0]["Description"]);
                            dt.Rows.Add(drdt);
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
    }
    protected void AddOrderSet(string sICDCode)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            string Type = string.Empty;

            DataTable tblItem = new DataTable();
            tblItem = (DataTable)ViewState["ItemDetail"];//"GridData"
            if (tblItem == null)
            {
                tblItem = CreateItemTable();
                ViewState["ItemDetail"] = tblItem;
            }

            DataView dvRemoveBlank = tblItem.Copy().DefaultView;
            //dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR ISNULL(CustomMedication,0)<>0";
            dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR CustomMedication<>1";
            tblItem = dvRemoveBlank.ToTable();
            ViewState["ItemDetail"] = tblItem;
            DataTable dt1 = tblItem;
            DataView DVItem = tblItem.Copy().DefaultView;
            string customMedication = common.myStr(txtCustomMedication.Text).Trim();

            if (customMedication.Length > 0)
            {
                DVItem.RowFilter = "CustomMedication=" + (chkCustomMedication.Checked ? "1" : "0");
            }
            else
            {
                if (common.myInt(hdnItemId.Value) > 0)
                {
                    DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value);
                }
                else if (common.myInt(hdnGenericId.Value) > 0)
                {
                    DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value);
                }
            }

            bool IsAdded = false;

            if (DVItem.ToTable().Rows.Count == 0)
            {
                string strIndentId = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (strIndentId == string.Empty)
                        strIndentId = strIndentId + dr["ItemId"];
                    else
                        strIndentId = strIndentId + "," + dr["ItemId"];
                }

                BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
                DataSet dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                DataView dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;

                        if (strIndentId != string.Empty)
                        {
                            dv.RowFilter = "ItemId NOT IN(" + strIndentId + ")";
                        }
                    }
                }
                if (dv.Count > 0)
                {
                    DataTable dt = dv.ToTable();
                    foreach (DataRow drM in dt.Rows)
                    {
                        if (CheckDDCExpiry(common.myInt(drM["itemid"])))
                        {
                            lblMessage.Text = "DDC Code Expire For This Item...";
                            continue;
                        }

                        DataRow DR1 = dt1.NewRow();
                        DataColumnCollection columns = tblItem.Columns;
                        if (columns.Contains("Id"))
                        {
                            DR1["Id"] = 0;
                        }
                        DR1["IndentNo"] = 0;
                        DR1["IndentDate"] = string.Empty;
                        DR1["IndentTypeId"] = 0;
                        DR1["IndentType"] = string.Empty;
                        DR1["IndentId"] = 0;
                        DR1["OverrideComments"] = string.Empty;
                        DR1["OverrideCommentsDrugToDrug"] = string.Empty;
                        DR1["OverrideCommentsDrugHealth"] = string.Empty;
                        DR1["IsSubstituteNotAllowed"] = 0;
                        DR1["ICDCode"] = txtICDCode.Text;
                        DR1["StrengthValue"] = string.Empty;
                        DR1["ReferanceItemId"] = 0;
                        DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR1["NotToPharmacy"] = 0;
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

                        DR1["GenericId"] = DBNull.Value;
                        DR1["GenericName"] = string.Empty;
                        DR1["ItemId"] = common.myInt(drM["itemid"]);
                        DR1["ItemName"] = common.myStr(drM["itemName"]);
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

                        DR1["Qty"] = common.myDbl(drM["Qty"]);

                        DR1["UnitName"] = common.myStr(drM["DoseUnit"]);
                        DR1["CustomMedication"] = 0;

                        //DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
                        //DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
                        //DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);

                        dt1.Rows.Add(DR1);
                        dt1.AcceptChanges();

                        dt1.TableName = "ItemDetail";
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

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
                        DR["ICDCode"] = txtICDCode.Text;
                        DR["StrengthValue"] = string.Empty;
                        DR["ReferanceItemId"] = 0;
                        DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR["NotToPharmacy"] = 0;
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

                        DR["GenericId"] = DBNull.Value;
                        DR["GenericName"] = string.Empty;
                        DR["ItemId"] = common.myInt(drM["itemid"]);
                        DR["ItemName"] = common.myStr(drM["itemName"]);
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
                        DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringV2(dt1);
                                                                                 //  DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dt1);
                        tblItem.Rows.Add(DR);
                    }
                    tblItem.AcceptChanges();
                    DataView dvRemoveSameColumn = tblItem.Copy().DefaultView;
                    dvRemoveSameColumn.RowFilter = "XMLData <> ''";
                    tblItem = dvRemoveSameColumn.ToTable();
                    ViewState["ItemDetail"] = tblItem.Copy();
                    ViewState["Item"] = tblItem.Copy();
                }
            }
            else
            {
                string strIndentId = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (strIndentId == string.Empty)
                        strIndentId = strIndentId + dr["ItemId"];
                    else
                        strIndentId = strIndentId + "," + dr["ItemId"];
                }

                BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
                DataSet dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                DataView dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;

                        if (strIndentId != string.Empty)
                        {
                            dv.RowFilter = "ItemId NOT IN(" + strIndentId + ")";
                        }
                    }
                }
                if (dv.Count > 0)
                {
                    string[] TobeDistinct = { };
                    DataTable dt = dv.ToTable();
                    foreach (DataRow drM in dt.Rows)
                    {
                        bool IsExists = false;
                        DataTable dtt = new DataTable();
                        dtt = (DataTable)ViewState["Item"];//"GridData"
                        if (dtt != null)
                        {
                            DataView dvv = dtt.Copy().DefaultView;
                            dvv.RowFilter = "ISNULL(ItemId,0) = " + common.myStr(drM["itemid"]);
                            IsExists = common.myBool(dvv.ToTable().Rows.Count > 0);
                        }
                        if (!IsExists)
                        {

                            if (CheckDDCExpiry(common.myInt(drM["itemid"])))
                            {
                                lblMessage.Text = "DDC Code Expire For This Item...";
                                continue;
                            }

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
                            DR1["ICDCode"] = txtICDCode.Text;
                            DR1["StrengthValue"] = string.Empty;
                            DR1["ReferanceItemId"] = 0;
                            DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR1["NotToPharmacy"] = 0;
                            DR1["IsInfusion"] = 0;
                            DR1["IsInjection"] = 0;
                            DR1["RouteName"] = common.myStr(drM["RouteName"]);
                            DR1["Duration"] = common.myInt(drM["Duration"]);
                            DR1["Type"] = common.myStr(drM["DtypeId"]);//
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
                            DR1["GenericId"] = DBNull.Value;
                            DR1["GenericName"] = string.Empty;
                            DR1["ItemId"] = common.myInt(drM["itemid"]);
                            DR1["ItemName"] = common.myStr(drM["itemName"]);
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
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

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
                            DR["ICDCode"] = txtICDCode.Text;
                            DR["StrengthValue"] = string.Empty;
                            DR["ReferanceItemId"] = 0;
                            DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR["NotToPharmacy"] = 0;
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

                            DR["GenericId"] = DBNull.Value;
                            DR["GenericName"] = string.Empty;
                            DR["ItemId"] = common.myInt(drM["itemid"]);
                            DR["ItemName"] = common.myStr(drM["itemName"]);
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
                            // DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dt1);
                            DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringV2(dt1);
                            tblItem.Rows.Add(DR);
                        }

                        tblItem.AcceptChanges();
                        DataView dvRemoveSameColumn = tblItem.Copy().DefaultView;
                        //dvRemoveSameColumn.RowFilter = "((ISNULL(XMLData,'') <> '' and Id = 0) OR (ISNULL(XMLData,'') = '' AND ID is null))";
                        dvRemoveSameColumn.RowFilter = "((ISNULL(XMLData,'') <> '' and GenericId is null) OR (ISNULL(XMLData,'') = '' AND GenericId >=0))";
                        tblItem = dvRemoveSameColumn.ToTable();
                        ViewState["ItemDetail"] = tblItem.Copy();
                        ViewState["Item"] = tblItem.Copy();
                    }
                }
            }
            ddlGeneric.Text = string.Empty;
            ddlBrand.Text = string.Empty;
            clearControls();
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

            ViewState["ItemDetail"] = tblItem.Copy();
            ViewState["Item"] = tblItem.Copy();
            gvItem.DataSource = tblItem.Copy();
            gvItem.DataBind();

            setVisiblilityInteraction();

            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;

            ddlBrand.Focus();
            ViewState["OrderSetId"] = "0";
            ////btnCopyLastPrescription.Enabled = false;

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

    }
    private bool isSavedItem()
    {
        bool isSave = true;
        string strmsg = string.Empty;

        try
        {
            //if (common.myInt(ddlGeneric.SelectedValue) == 0 && common.myInt(ddlBrand.SelectedValue) == 0 && !chkCustomMedication.Checked)
            if (common.myInt(hdnGenericId.Value) == 0 && common.myInt(hdnItemId.Value) == 0 && !chkCustomMedication.Checked)
            {
                if (common.myInt(hdnGenericId.Value) == 0 && common.myInt(hdnItemId.Value) == 0 && common.myInt(ddlBrand.SelectedValue) > 0)
                {
                    hdnItemId.Value = common.myInt(ddlBrand.SelectedValue).ToString();
                }
                else
                {
                    strmsg += "Generic Or Item not selected !";
                    isSave = false;
                }
            }

            //if (common.myInt(ddlGeneric.SelectedValue) == 0 && common.myInt(ddlBrand.SelectedValue) == 0
            //    && (chkCustomMedication.Checked && common.myLen(txtCustomMedication.Text) == 0))
            if (common.myInt(hdnGenericId.Value) == 0 && common.myInt(hdnItemId.Value) == 0
                && (chkCustomMedication.Checked && common.myLen(txtCustomMedication.Text) == 0))
            {
                strmsg += "Custom medication can't be blank !";
                isSave = false;
            }

            if (common.myDbl(txtDose.Text) == 0)
            {
                strmsg += "Dose can not be zero or blank !";
                isSave = false;
            }

            if (common.myDbl(txtDuration.Text) == 0)
            {
                strmsg += "Duration can not be zero or blank !";
                isSave = false;
            }

            //if (common.myDbl(txtTotQty.Text) == 0)
            //{
            //    strmsg += "Total quantity can not be zero or blank !";
            //    isSave = false;
            //}
            if (strmsg != "")
                // Alert.ShowAjaxMsg(strmsg, this);
                lblERXNo.Text = strmsg;
        }
        catch
        { }

        // lblMessage.Text = strmsg;
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
        try
        {

            ds = objD.getAllergies(common.myInt(Session["RegistrationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dvAllergy = new DataView(ds.Tables[0]);
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
        //if (common.myStr(ViewState["ICDCodes"]).Equals(string.Empty))
        //{
        //    if (!common.myStr(Session["OPIP"]).ToUpper().Equals("I"))
        //    {
        //        Alert.ShowAjaxMsg("Please Select ICDcode First!..", Page.Page);
        //        lblMessage.Text = "Please Select ICDcode First!..";
        //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //        return;
        //    }
        //}
        DataTable tblItem = new DataTable();
        DataView DVItem = new DataView();
        BaseC.EMR emr = new BaseC.EMR(sConString);
        try
        {
            //added by bhakti
            if (ViewState["isRequireIPBillOfflineMarking"].ToString() == "Y" && common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["PatientCompanyType"]).Equals("C") && common.myStr(Session["IsOffline"]).Equals("True"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Patient is Offline, No transaction allow !')", true);
                return;
            }

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
                    Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = null;
                }

                DataTable dt = emr.GetEMRExistingMedicationOrder(common.myInt(Session["HospitalLocationid"]), common.myInt(Session["FacilityId"]),
                                                            common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]),
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

                if (customMedication.Length == 0)
                {
                    RetrievePatientAllergies();
                    if (ViewState["DrugAllergy"] != null)
                    {
                        DataTable tblDrugAllergy = (DataTable)ViewState["DrugAllergy"];
                        DataView dvDrugAllergy = tblDrugAllergy.Copy().DefaultView;

                        if (common.myInt(hdnItemId.Value) > 0)
                        {
                            dvDrugAllergy.RowFilter = "AllergyID=" + common.myInt(hdnItemId.Value);
                        }
                        else
                        {
                            dvDrugAllergy.RowFilter = "Generic_Id=" + common.myInt(hdnGenericId.Value);
                        }

                        if (dvDrugAllergy.ToTable().Rows.Count > 0)
                        {
                            IsAllergy = true;
                        }
                    }
                }

                if (IsAllergy)
                {
                    lblMessage.Text = "Patient is allergy to this drug !";

                    //RadWindow1.NavigateUrl = "/EMR/Medication/MedicationOverride.aspx?" +
                    //                        "D=" + common.myStr(ddlBrand.Text).Trim() +
                    //                        "&IsO=" + common.myInt(hdnIsOverride.Value) +
                    //                        "&OC=" + common.myStr(hdnOverrideComments.Value) +
                    //                        "&DASR=" + common.myStr(hdnDrugAllergyScreeningResult.Value);


                    RadWindow1.NavigateUrl = "/EMR/Medication/MedicationOverride.aspx?" +
                                            "D=" + common.myStr(ddlBrand.Text).Trim() +
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
                addItem(common.myStr(ViewState["ICDCodes"]));


                if (common.myStr(ViewState["ProceedValue"]) == "F")
                {
                    proceedFavourite(true);
                }
                else if (common.myStr(ViewState["ProceedValue"]) == "C")
                {
                    proceedCurrent(true);
                }

                lblMessage.Text = string.Empty;

                btnMonographViewOnItemBased.Visible = false;
                btnInteractionViewOnItemBased.Visible = false;

                // txtTotalQuantity.Text = string.Empty;
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
        }
    }
    protected void ddlDoseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //RadComboBox DoseType = (RadComboBox)sender;
            //GridViewRow item = (GridViewRow)DoseType.NamingContainer;
            //  RadComboBox ddlFrequencyId = (RadComboBox)item.FindControl("ddlFrequencyId");
            //  RadComboBox ddlReferanceItem = (RadComboBox)item.FindControl("ddlReferanceItem");
            // TextBox txtDuration = (TextBox)item.FindControl("txtDuration");
            // RadComboBox ddlPeriodType = (RadComboBox)item.FindControl("ddlPeriodType");
            // if (DoseType.SelectedValue != string.Empty && DoseType.SelectedValue != "0")
            if (ddlDoseType.SelectedValue != string.Empty && ddlDoseType.SelectedValue != "0")
            {
                ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty));
                ddlFrequencyId.Items[0].Value = "0";
                //ddlFrequencyId.SelectedValue = "0";
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue("0"));

                ddlFrequencyId.Enabled = false;
                ddlReferanceItem.Enabled = false;
                txtDuration.Enabled = false;
                ddlPeriodType.Enabled = false;
            }
            else
            {
                ddlFrequencyId.Items.Remove(0);
                //ddlFrequencyId.SelectedValue = "0";
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
        if (txtDuration.Text == "" && Session["OPIP"].ToString() == "I")
        {
            txtDuration.Text = "2";
        }
    }
    //private void addItem(string sICDCode)
    //{
    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

    //    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //    DataTable dtold = new DataTable();
    //    DataTable dt1old = new DataTable();

    //    DataTable dt = new DataTable();
    //    DataTable dt1 = new DataTable();
    //    DataTable dtNew = new DataTable();
    //    DataSet ds = new DataSet();
    //    DataView dv = new DataView();
    //    DataView dv1 = new DataView();
    //    try
    //    {
    //        DataRow DR;
    //        DataRow DR1;
    //        decimal dQty = 0;
    //        int countRow = 0;

    //        if (common.myInt(hdnItemId.Value).Equals(0) && common.myInt(ddlBrand.SelectedValue) > 0)
    //        {
    //            hdnItemId.Value = common.myInt(ddlBrand.SelectedValue).ToString();
    //        }

    //        if (common.myInt(hdnItemId.Value).Equals(0)
    //            && common.myInt(hdnGenericId.Value).Equals(0)
    //            && common.myLen(txtCustomMedication.Text).Equals(0))
    //        {
    //            Alert.ShowAjaxMsg("Please select drug", Page);
    //            return;
    //        }

    //        if (!chkCustomMedication.Checked)
    //        {
    //            DataTable tblItem = new DataTable();
    //            tblItem = (DataTable)ViewState["ItemDetail"];

    //            if (tblItem != null)
    //            {
    //                DataView dvRemoveBlank = tblItem.Copy().DefaultView;
    //                dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR CustomMedication<>1";
    //                tblItem = dvRemoveBlank.ToTable();

    //                DataView DVItem = tblItem.Copy().DefaultView;

    //                if (common.myInt(hdnItemId.Value) > 0)
    //                {
    //                    DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value) + " AND CustomMedication<>1 ";
    //                }
    //                else
    //                {
    //                    DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value) + " AND CustomMedication<>1 ";
    //                }

    //                if (!common.myBool(ViewState["Edit"]))
    //                {
    //                    if (DVItem.ToTable().Rows.Count > 0)
    //                    {
    //                        Alert.ShowAjaxMsg("Brand name already added", Page);
    //                        return;
    //                    }
    //                }
    //            }

    //            //if (ddlBrand.SelectedValue == string.Empty || ddlBrand.SelectedValue == "0")
    //            //{
    //            //    Alert.ShowAjaxMsg("Please select drug", Page); 
    //            //    return;
    //            //}

    //            if (Request.QueryString["DRUGORDERCODE"] == null)
    //            {
    //                //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
    //                //{
    //                //    Alert.ShowAjaxMsg("Please select Formulation", Page);
    //                //    return;
    //                //}


    //                if (common.myStr(ViewState["RouteMandatory"]).Equals("Y"))
    //                {
    //                    if (ddlRoute.SelectedValue == string.Empty || ddlRoute.SelectedValue == "0")
    //                    {
    //                        Alert.ShowAjaxMsg("Please select Route", Page);
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        if (ViewState["Item"] == null && ViewState["Edit"] == null)
    //        {
    //            dt = CreateItemTable();
    //            dt1 = CreateItemTable();
    //        }
    //        else
    //        {
    //            dtold = (DataTable)ViewState["Item"];

    //            //BaseC.clsIndent objIndent = new BaseC.clsIndent(sConString);
    //            //if (objIndent.IsAllergyWithPrescribingMedicine(common.myInt(Session["EncounterId"]), common.myInt(ViewState["ItemId"])))
    //            //{

    //            //    return;
    //            //}

    //            if (ViewState["Edit"] != null && common.myBool(ViewState["Edit"]) && ViewState["Item"] != null)
    //            {
    //                dv = new DataView(dtold);
    //                dv.RowFilter = "ItemId<>" + common.myStr(hdnItemId.Value);
    //                dt = dv.ToTable();
    //            }
    //            else
    //            {
    //                dt = (DataTable)ViewState["Item"];
    //            }
    //            if (dt.Rows.Count > 0)
    //            {
    //                if (ViewState["Edit"] == null)
    //                {
    //                    foreach (GridViewRow dataItem in gvItem.Rows)
    //                    {
    //                        TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
    //                        dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
    //                        countRow++;
    //                        dt.AcceptChanges();
    //                    }
    //                }
    //            }
    //            if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
    //            {
    //                dt1 = CreateItemTable();
    //            }
    //            else
    //            {
    //                dt1old = (DataTable)ViewState["ItemDetail"];
    //                if (ViewState["Edit"] != null && common.myBool(ViewState["Edit"]) && ViewState["ItemDetail"] != null)
    //                {
    //                    dv1 = new DataView(dt1old);
    //                    dv1.RowFilter = "ItemId<>" + common.myStr(hdnItemId.Value);
    //                    dt1 = dv1.ToTable();
    //                }
    //                else
    //                {
    //                    dt1 = (DataTable)ViewState["ItemDetail"];
    //                }
    //            }
    //        }

    //        if (!chkCustomMedication.Checked)
    //        {
    //            foreach (GridViewRow row in gvItem.Rows)
    //            {
    //                HiddenField hdnGDItemId = (HiddenField)row.FindControl("hdnItemId");

    //                if (common.myInt(hdnGDItemId.Value).Equals(hdnItemId.Value) && ViewState["Edit"] == null)
    //                {
    //                    DataSet dsXmlData = new DataSet();
    //                    DataView dvFilter = new DataView();
    //                    DataTable dtDetail = new DataTable();
    //                    HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
    //                    StringReader sr = new StringReader(hdnXMLData.Value);

    //                    dsXmlData.ReadXml(sr);
    //                    dvFilter = new DataView(dsXmlData.Tables[0]);
    //                    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + hdnGDItemId.Value + " AND CustomMedication<>1 ";
    //                    dtDetail = dvFilter.ToTable();

    //                    if (dtDetail.Rows.Count > 0)
    //                    {
    //                        RadDatePicker startdate = new RadDatePicker();
    //                        startdate.SelectedDate = Convert.ToDateTime(common.myDate(dtDetail.Rows[0]["StartDate"]));

    //                        endDateChange();

    //                        DateTime endDateForNewDose = Convert.ToDateTime(txtEndDate.SelectedDate);
    //                        if (txtStartDate.SelectedDate <= endDateForNewDose)
    //                        {
    //                            Alert.ShowAjaxMsg("Brand name already added", Page);
    //                            return;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        if (!dt.Columns.Contains("Frequency"))
    //        {
    //            dt.Columns.Add("Frequency", typeof(decimal));
    //        }

    //        DataView dvChkCustomMedication = dt.DefaultView;
    //        try
    //        {
    //            if (chkCustomMedication.Checked)
    //            {
    //                dvChkCustomMedication.RowFilter = "ISNULL(CustomMedication,0)=1";
    //                if (dvChkCustomMedication.ToTable().Rows.Count > 0)
    //                {
    //                    if (ViewState["Edit"] == null || (!ddlBrand.Enabled && common.myLen(txtCustomMedication.Text) > 0))
    //                    {
    //                        Alert.ShowAjaxMsg("Multiple custom medications not allowed in one prescription!", this.Page);
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        {
    //        }
    //        finally
    //        {
    //            dvChkCustomMedication.RowFilter = string.Empty;
    //            dvChkCustomMedication.Dispose();
    //        }

    //        //Item
    //        DR = dt.NewRow();

    //        if (ddlIndentType.SelectedValue == string.Empty)
    //        {
    //            Alert.ShowAjaxMsg("Please select Indent Type", Page);
    //            return;
    //        }
    //        DR["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
    //        DR["IndentType"] = ddlIndentType.SelectedValue == string.Empty ? string.Empty : ddlIndentType.Text;
    //        DR["IndentId"] = 0;
    //        //DR["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
    //        DR["GenericId"] = common.myInt(hdnGenericId.Value);

    //        if (common.myInt(hdnItemId.Value).Equals(0) && chkCustomMedication.Checked)
    //        {
    //            DR["ItemId"] = DBNull.Value;
    //        }
    //        else if (common.myInt(hdnItemId.Value) > 0 && !chkCustomMedication.Checked)
    //        {
    //            DR["ItemId"] = common.myInt(hdnItemId.Value);
    //        }
    //        DR["GenericName"] = common.myStr(ddlGeneric.Text);

    //        DR["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) != 0) ? ddlBrand.Text : txtCustomMedication.Text;

    //        DR["StrengthValue"] = common.myStr(txtStrengthValue.Text);

    //        DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
    //        DR["CustomMedication"] = chkCustomMedication.Checked;
    //        DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
    //        DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;
    //        DR["IsInfusion"] = common.myBool(hdnIsInjection.Value);

    //        DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
    //        if (ddlFormulation.SelectedItem != null)
    //        {
    //            DR["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
    //        }
    //        DR["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
    //        DR["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");


    //        ///Item Detail
    //        // TextBox txtInstructions = new TextBox();
    //        //RadComboBox ddlFoodRelation = new RadComboBox();
    //        #region Item Add
    //        //foreach (GridViewRow dataItem in gvItemDetails.Rows)
    //        //{

    //        if (!dt1.Columns.Contains("DurationText"))
    //        {
    //            dt1.Columns.Add("DurationText", typeof(string));
    //        }

    //        DR1 = dt1.NewRow();
    //        string sFrequency = "0";
    //        //RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
    //        //TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
    //        if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
    //        {
    //            sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
    //        }

    //        //TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
    //        //RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
    //        //RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");
    //        //RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

    //        if (common.myInt(ddlUnit.SelectedValue).Equals(0))
    //        {
    //            Alert.ShowAjaxMsg("Please select unit !", this.Page);
    //            return;
    //        }

    //        if (Request.QueryString["DRUGORDERCODE"] == null)
    //        {
    //            //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
    //            //{
    //            //    Alert.ShowAjaxMsg("Please select Formulation", Page);
    //            //    return;
    //            //}

    //            if (ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0")
    //            {
    //                if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == string.Empty)
    //                {
    //                    Alert.ShowAjaxMsg("Please select Frequency", Page);
    //                    return;
    //                }

    //                if (common.myInt(ddlReferanceItem.SelectedValue) <= 0 || ddlReferanceItem.SelectedValue == "0" || ddlReferanceItem.SelectedValue == string.Empty)
    //                {
    //                    if (common.myDbl(txtDose.Text) == 0.0)
    //                    {
    //                        Alert.ShowAjaxMsg("Please type Dose", Page);
    //                        return;
    //                    }

    //                    if (txtDuration.Text == string.Empty || txtDuration.Text == "0"
    //                        || txtDuration.Text == "0." || txtDuration.Text == ".0"
    //                        || txtDuration.Text == ".")
    //                    {
    //                        Alert.ShowAjaxMsg("Please type Duration", Page);
    //                        ddlFormulation.Focus();
    //                        return;
    //                    }
    //                }
    //            }
    //        }

    //        //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
    //        string Type = string.Empty;
    //        decimal dDuration = 0;

    //        switch (common.myStr(ddlPeriodType.SelectedValue))
    //        {
    //            case "N":
    //                Type = txtDuration.Text + " Minute(s)";
    //                dDuration = 1;
    //                break;
    //            case "H":
    //                Type = txtDuration.Text + " Hour(s)";
    //                dDuration = 1;
    //                break;
    //            case "D":
    //                Type = txtDuration.Text + " Day(s)";
    //                dDuration = 1;
    //                break;
    //            case "W":
    //                Type = txtDuration.Text + " Week(s)";
    //                dDuration = 7;
    //                break;
    //            case "M":
    //                Type = txtDuration.Text + " Month(s)";
    //                dDuration = 30;
    //                break;
    //            case "Y":
    //                Type = txtDuration.Text + " Year(s)";
    //                dDuration = 365;
    //                break;
    //        }

    //        if (Request.QueryString["DRUGORDERCODE"] == null)
    //        {
    //            if (common.myBool(ViewState["ISCalculationRequired"]))
    //            {
    //                dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
    //            }
    //            else
    //            {
    //                dQty = 1;
    //            }
    //            DR1["RouteId"] = ddlRoute.SelectedValue;
    //            DR1["RouteName"] = ddlRoute.SelectedItem.Text;
    //        }
    //        else
    //        {
    //            dQty = 0;
    //        }

    //        DR1["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");

    //        //DR1["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
    //        DR1["GenericId"] = common.myInt(hdnGenericId.Value);
    //        DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
    //        DR1["GenericName"] = common.myStr(ddlGeneric.Text);
    //        //DR1["ItemName"] = ddlBrand.SelectedValue == string.Empty ? string.Empty : ddlBrand.Text;
    //        DR1["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) > 0) ? ddlBrand.Text : txtCustomMedication.Text;

    //        DR1["StrengthValue"] = common.myStr(txtStrengthValue.Text);

    //        DR1["Dose"] = txtDose.Text;
    //        DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
    //        DR1["Duration"] = txtDuration.Text;
    //        DR1["DurationText"] = Type;
    //        DR1["Type"] = ddlPeriodType.SelectedValue;

    //        DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
    //        DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

    //        DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
    //        DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);
    //        DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
    //        DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;
    //        DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
    //        DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
    //        DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
    //        DR1["PrescriptionDetail"] = string.Empty;
    //        DR1["CustomMedication"] = chkCustomMedication.Checked;
    //        DR1["Instructions"] = common.clearHTMLTags(txtInstructions.Text);
    //        DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
    //        DR1["ReferanceItemName"] = ddlReferanceItem.SelectedValue == string.Empty || ddlReferanceItem.SelectedValue == "0" ? string.Empty : ddlReferanceItem.Text;
    //        DR1["DoseTypeId"] = common.myInt(ddlDoseType.SelectedValue);
    //        DR1["DoseTypeName"] = ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0" ? string.Empty : ddlDoseType.Text;
    //        DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
    //        if (ddlFormulation.SelectedItem != null)
    //        {
    //            DR1["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
    //        }
    //        if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
    //        {
    //            DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
    //            DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
    //            DR1["IsInfusion"] = true;
    //            DR["IsInfusion"] = true;
    //        }
    //        else if (ddlDoseType.SelectedValue != "0")
    //        {
    //            DR1["IsInfusion"] = false;
    //            DR1["FrequencyId"] = 0;
    //            DR1["FrequencyName"] = string.Empty;
    //        }
    //        else
    //        {
    //            DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
    //            DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
    //            DR1["IsInfusion"] = false;
    //        }
    //        DR1["Volume"] = common.myStr(txtVolume.Text);
    //        DR1["VolumeUnitId"] = common.myInt(ddlVolumeUnit.SelectedValue);
    //        DR1["InfusionTime"] = common.myStr(txtTimeInfusion.Text);
    //        DR1["TimeUnit"] = common.myStr(ddlTimeUnit.SelectedValue);
    //        DR1["TotalVolume"] = common.myStr(txtTotalVolumn.Text);
    //        if (!string.IsNullOrEmpty(common.myStr(txtFlowRateUnit.Text)))
    //            DR1["FlowRate"] = common.myStr(txtFlowRateUnit.Text);
    //        else
    //            DR1["FlowRate"] = "0";
    //        DR1["FlowRateUnit"] = common.myInt(ddlFlowRateUnit.SelectedValue);
    //        DR1["XMLVariableDose"] = common.myStr(hdnXmlVariableDoseString.Value).Trim();
    //        DR1["XMLFrequencyTime"] = common.myStr(hdnXmlFrequencyTime.Value).Trim();

    //        DR1["OverrideComments"] = common.myStr(txtCommentsDrugAllergy.Text);
    //        DR1["OverrideCommentsDrugToDrug"] = common.myStr(txtCommentsDrugToDrug.Text);
    //        DR1["OverrideCommentsDrugHealth"] = common.myStr(txtCommentsDrugHealth.Text);
    //        DR1["IsSubstituteNotAllowed"] = chkSubstituteNotAllow.Checked;
    //        DR1["ICDCode"] = common.myStr(txtICDCode.Text);
    //        dt1.Rows.Add(DR1);
    //        dt1.AcceptChanges();
    //        //ddlReferanceItem.SelectedValue = "0";
    //        ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));

    //        txtInstructions.Text = string.Empty;
    //        // }
    //        #endregion
    //        dt1.TableName = "ItemDetail";
    //        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

    //        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
    //        dt1.WriteXml(writer);

    //        string xmlSchema = writer.ToString();
    //        xmlSchema = xmlSchema.Replace("&lt;", "<");
    //        xmlSchema = xmlSchema.Replace("&gt;", ">");

    //        DR["XMLData"] = xmlSchema;

    //        DataTable dtPD = dt1.Copy().Clone();
    //        dtPD.Rows.Add(DR1.ItemArray);

    //        DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dtPD);

    //        DR1["PrescriptionDetail"] = common.myStr(DR["PrescriptionDetail"]);
    //        DR1["XMLData"] = common.myStr(DR["XMLData"]);

    //        DR["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");

    //        //DR["Qty"] = 0;
    //        dt.Rows.Add(DR);
    //        dt.AcceptChanges();

    //        try
    //        {
    //            int ReturnUnAppPrescriptionId = saveAsUnApprovedPrescriptions(DR1);

    //            if (dt1 != null)
    //            {
    //                int maxRow = dt1.Rows.Count;
    //                if (maxRow > 0)
    //                {
    //                    dt1.Rows[maxRow - 1]["UnAppPrescriptionId"] = common.myInt(ReturnUnAppPrescriptionId);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //        }

    //        ViewState["DataTableDetail"] = null;

    //        dt1 = addColumnInItemGrid(dt1);

    //        ViewState["ItemDetail"] = dt1;
    //        ViewState["Item"] = dt1;

    //        gvItem.DataSource = dt1;
    //        gvItem.DataBind();
    //        //lblPrescribedBy.Text = "Prescribed By: "+ common.myStr(Session["EmployeeName"]);


    //        ViewState["Edit"] = null;

    //        setVisiblilityInteraction();

    //        dvPharmacistInstruction.Visible = false;
    //        lnkPharmacistInstruction.Visible = false;

    //        ddlFormulation.Enabled = true;
    //        ddlRoute.Enabled = true;
    //        //ddlStrength.Enabled = true;
    //        txtStrengthValue.Enabled = true;

    //        try
    //        {
    //            hdnItemId.Value = "0";
    //            ddlBrand.Focus();
    //            ddlBrand.Items.Clear();
    //            ddlBrand.Text = string.Empty;
    //            ddlBrand.Enabled = true;
    //            ddlBrand.SelectedValue = null;

    //            hdnGenericId.Value = "0";
    //            ddlGeneric.Items.Clear();
    //            ddlGeneric.Text = string.Empty;
    //            ddlGeneric.SelectedValue = null;

    //            txtCustomMedication.Text = string.Empty;
    //        }
    //        catch
    //        { }

    //        txtStrengthValue.Text = string.Empty;

    //        txtInstructions.Text = string.Empty;
    //        //ddlFoodRelation.SelectedValue = "0";
    //        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));

    //        ddlFoodRelation.Text = string.Empty;
    //        chkNotToPharmacy.Checked = false;

    //        ddlFrequencyId.Enabled = true;
    //        ddlReferanceItem.Enabled = true;
    //        txtDuration.Enabled = true;
    //        //ddlPeriodType.Enabled = true;


    //        ddlFrequencyId.SelectedIndex = 0;
    //        txtDose.Text = string.Empty; //"1";
    //        txtDuration.Text = string.Empty;
    //        //ddlPeriodType.SelectedValue = "D";
    //        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));

    //        //ddlUnit.SelectedValue = "0";
    //        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));

    //        txtInstructions.Text = string.Empty;
    //        //ddlReferanceItem.SelectedValue = "0";
    //        ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));

    //        //ddlFoodRelation.SelectedValue = "0";
    //        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));

    //        //ddlDoseType.SelectedValue = "0";
    //        ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue("0"));

    //        txtVolume.Text = string.Empty;
    //        txtTotalVolumn.Text = string.Empty;
    //        txtTimeInfusion.Text = string.Empty;
    //        txtFlowRateUnit.Text = string.Empty;

    //        //ddlFlowRateUnit.SelectedValue = "0";
    //        ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue("0"));

    //        //ddlVolumeUnit.SelectedValue = "0";
    //        ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue("0"));

    //        //ddlTimeUnit.SelectedValue = "0";
    //        ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue("0"));

    //        //ddlFormulation.SelectedValue = "0";
    //        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue("0"));

    //        //ddlRoute.SelectedValue = "0";
    //        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));

    //        txtStartDate.SelectedDate = DateTime.Now;
    //        txtEndDate.SelectedDate = null;

    //        ViewState["Edit"] = null;
    //        txtCustomMedication.Text = string.Empty;
    //        hdnXmlVariableDoseString.Value = string.Empty;
    //        hdnvariableDoseDuration.Value = string.Empty;
    //        hdnvariableDoseFrequency.Value = string.Empty;
    //        hdnVariabledose.Value = string.Empty;
    //        hdnXmlFrequencyTime.Value = string.Empty;

    //        txtCommentsDrugAllergy.Text = string.Empty;
    //        txtCommentsDrugToDrug.Text = string.Empty;
    //        txtCommentsDrugHealth.Text = string.Empty;

    //        lblGenericName.Text = string.Empty;
    //        chkSubstituteNotAllow.Checked = false;
    //        //txtICDCode.Text = string.Empty;

    //        txtTotQty.Text = string.Empty;
    //        txtSpecialInstrucation.Text = string.Empty;

    //        btnCopyLastPrescription.Enabled = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        dtold.Dispose();
    //        dt1old.Dispose();
    //        dt.Dispose();
    //        dt1.Dispose();
    //        dtNew.Dispose();
    //        ds.Dispose();
    //        emr = null;
    //        dv.Dispose();
    //        dv1.Dispose();
    //    }
    //}


    private void addItem(string sICDCode)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtold = new DataTable();
        DataTable dt1old = new DataTable();

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtNew = new DataTable();
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataView dv1 = new DataView();
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
            /*if (common.myInt(hdnItemId.Value) > 0)
            {
                if (CheckDDCExpiry(common.myInt(hdnItemId.Value)))
                {
                    ddlBrand.Text = string.Empty;
                    ddlBrand.SelectedValue = null;
                    lblMessage.Text = "DDC Code Expire For This Item...";
                    return;
                }
            }*/

            if (common.myInt(hdnItemId.Value).Equals(0)
                && common.myInt(hdnGenericId.Value).Equals(0)
                && common.myLen(txtCustomMedication.Text).Equals(0))
            {
                // Alert.ShowAjaxMsg("Please select drug", Page);
                lblERXNo.Text = "Please select drug";
                return;
            }

            if (!chkCustomMedication.Checked)
            {
                DataTable tblItem = new DataTable();
                tblItem = (DataTable)ViewState["ItemDetail"];

                if (tblItem != null)
                {

                    DataView dvRemoveBlank = tblItem.Copy().DefaultView;

                    dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR CustomMedication<>1";
                    tblItem = dvRemoveBlank.ToTable();

                    DataView DVItem = tblItem.Copy().DefaultView;

                    if (common.myInt(hdnItemId.Value) > 0)
                    {
                        // DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value) + " AND CustomMedication<>0";
                        DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value) + "";
                    }
                    else
                    {
                        // DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value) + " AND CustomMedication<>0";
                        DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value) + "";
                    }

                    if (!common.myBool(ViewState["Edit"]))
                    {
                        if (DVItem.ToTable().Rows.Count > 0)
                        {
                            // Alert.ShowAjaxMsg("Brand name already added", Page);
                            lblERXNo.Text = "Brand name already added";
                            return;
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


                    if (common.myStr(ViewState["RouteMandatory"]).Equals("Y"))
                    {
                        if (ddlRoute.SelectedValue == string.Empty || ddlRoute.SelectedValue == "0")
                        {
                            // Alert.ShowAjaxMsg("Please select Route", Page);
                            lblERXNo.Text = "Please select Route";
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
                //if (objIndent.IsAllergyWithPrescribingMedicine(common.myInt(Session["EncounterId"]), common.myInt(ViewState["ItemId"])))
                //{

                //    return;
                //}

                if (ViewState["Edit"] != null && common.myBool(ViewState["Edit"]) && ViewState["Item"] != null)
                {
                    dv = new DataView(dtold);
                    dv.RowFilter = "ItemId<>" + common.myStr(hdnItemId.Value);
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
                        //foreach (GridViewRow dataItem in gvItem.Rows)
                        //{
                        //    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
                        //    dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
                        //    countRow++;
                        //    dt.AcceptChanges();
                        //}
                    }
                }
                if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
                {
                    dt1 = CreateItemTable();
                }
                else
                {
                    dt1old = (DataTable)ViewState["ItemDetail"];
                    if (ViewState["Edit"] != null && common.myBool(ViewState["Edit"]) && ViewState["ItemDetail"] != null)
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

            //if (!chkCustomMedication.Checked)
            //{
            //    foreach (GridViewRow row in gvItem.Rows)
            //    {
            //        HiddenField hdnGDItemId = (HiddenField)row.FindControl("hdnItemId");

            //        if (common.myInt(hdnGDItemId.Value).Equals(hdnItemId.Value) && ViewState["Edit"] == null)
            //        {
            //            DataSet dsXmlData = new DataSet();
            //            DataView dvFilter = new DataView();
            //            DataTable dtDetail = new DataTable();
            //            HiddenField hdnXMLData = (HiddenField)row.FindControl("hdnXMLData");
            //            if (common.myLen(hdnXMLData.Value) > 0)
            //            {
            //                StringReader sr = new StringReader(hdnXMLData.Value);

            //                dsXmlData.ReadXml(sr);
            //                dvFilter = new DataView(dsXmlData.Tables[0]);
            //                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + hdnGDItemId.Value + " AND CustomMedication<>1 ";
            //                dtDetail = dvFilter.ToTable();

            //                if (dtDetail.Rows.Count > 0)
            //                {
            //                    RadDatePicker startdate = new RadDatePicker();
            //                    startdate.SelectedDate = Convert.ToDateTime(common.myDate(dtDetail.Rows[0]["StartDate"]));

            //                    endDateChange();

            //                    DateTime endDateForNewDose = Convert.ToDateTime(txtEndDate.SelectedDate);
            //                    if (txtStartDate.SelectedDate <= endDateForNewDose)
            //                    {
            //                        Alert.ShowAjaxMsg("Brand name already added", Page);
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            if (!dt.Columns.Contains("Frequency"))
            {
                dt.Columns.Add("Frequency", typeof(decimal));
            }

            DataView dvChkCustomMedication = dt.DefaultView;
            try
            {
                if (chkCustomMedication.Checked)
                {
                    dvChkCustomMedication.RowFilter = "ISNULL(CustomMedication,0)=1";
                    if (dvChkCustomMedication.ToTable().Rows.Count > 0)
                    {
                        if (ViewState["Edit"] == null || (!ddlBrand.Enabled && common.myLen(txtCustomMedication.Text) > 0))
                        {
                            //  Alert.ShowAjaxMsg("Multiple custom medications not allowed in one prescription!", this.Page);
                            lblERXNo.Text = "Multiple custom medications not allowed in one prescription!";
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                dvChkCustomMedication.RowFilter = string.Empty;
                dvChkCustomMedication.Dispose();
            }



            //Item
            DR = dt.NewRow();

            if (ddlIndentType.SelectedValue == string.Empty)
            {
                //  Alert.ShowAjaxMsg("Please select Indent Type", Page);
                lblERXNo.Text = "Please select Indent Type";
                return;
            }
            DR["IndentTypeId"] = common.myInt(ddlIndentType.SelectedValue);
            DR["IndentType"] = ddlIndentType.SelectedValue == string.Empty ? string.Empty : ddlIndentType.Text;
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

            DR["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) != 0) ? ddlBrand.Text : txtCustomMedication.Text;

            DR["StrengthValue"] = common.myStr(txtStrengthValue.Text);

            DR["RouteId"] = common.myInt(ddlRoute.SelectedValue);
            DR["CustomMedication"] = chkCustomMedication.Checked;
            DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
            DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;
            DR["IsInfusion"] = common.myBool(hdnIsInjection.Value);

            DR["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            if (ddlFormulation.SelectedItem != null)
            {
                DR["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
            }
            DR["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            DR["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");


            ///Item Detail
            // TextBox txtInstructions = new TextBox();
            //RadComboBox ddlFoodRelation = new RadComboBox();
            #region Item Add
            //foreach (GridViewRow dataItem in gvItemDetails.Rows)
            //{

            if (!dt1.Columns.Contains("DurationText"))
            {
                dt1.Columns.Add("DurationText", typeof(string));
            }

            DR1 = dt1.NewRow();
            string sFrequency = "0";
            //RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
            //TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
            if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
            {
                sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            }

            //TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
            //RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
            //RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");
            //RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

            if (common.myInt(ddlUnit.SelectedValue).Equals(0))
            {
                //   Alert.ShowAjaxMsg("Please select unit !", this.Page);
                lblERXNo.Text = "Please select unit !";
                return;
            }

            if (Request.QueryString["DRUGORDERCODE"] == null)
            {
                //if (common.myInt(ddlFormulation.SelectedValue).Equals(0))
                //{
                //    Alert.ShowAjaxMsg("Please select Formulation", Page);
                //    return;
                //}

                if (ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0")
                {
                    if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == string.Empty)
                    {
                        //   Alert.ShowAjaxMsg("Please select Frequency", Page);
                        lblERXNo.Text = "Please select Frequency";
                        return;
                    }

                    if (common.myInt(ddlReferanceItem.SelectedValue) <= 0 || ddlReferanceItem.SelectedValue == "0" || ddlReferanceItem.SelectedValue == string.Empty)
                    {
                        if (common.myDbl(txtDose.Text) == 0.0)
                        {
                            //   Alert.ShowAjaxMsg("Please type Dose", Page);
                            lblERXNo.Text = "Please type Dose";
                            return;
                        }

                        if (txtDuration.Text == string.Empty || txtDuration.Text == "0"
                            || txtDuration.Text == "0." || txtDuration.Text == ".0"
                            || txtDuration.Text == ".")
                        {
                            // Alert.ShowAjaxMsg("Please type Duration", Page);
                            lblERXNo.Text = "Please type Duration";
                            ddlFormulation.Focus();
                            return;
                        }
                    }
                }
            }

            //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
            string Type = string.Empty;
            decimal dDuration = 0;

            if (common.myStr(hdnXmlVariableDoseString.Value) != "")
            {
                calcTotalQtyForVariableDose(common.myStr(hdnXmlVariableDoseString.Value), common.myInt(sFrequency));
            }
            else
            {

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

                if (Request.QueryString["DRUGORDERCODE"] == null)
                {
                    if (common.myBool(ViewState["ISCalculationRequired"]))
                    {
                        dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
                    }
                    else
                    {
                        dQty = 1;
                    }
                    //DR1["RouteId"] = ddlRoute.SelectedValue;
                    //DR1["RouteName"] = ddlRoute.SelectedItem.Text;
                }
                else
                {
                    dQty = 0;
                }
            }
            DR1["RouteId"] = ddlRoute.SelectedValue;
            DR1["RouteName"] = ddlRoute.SelectedItem.Text;
            DR1["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");

            //DR1["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
            DR1["GenericId"] = common.myInt(hdnGenericId.Value);
            DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
            DR1["GenericName"] = common.myStr(ddlGeneric.Text);
            //DR1["ItemName"] = ddlBrand.SelectedValue == string.Empty ? string.Empty : ddlBrand.Text;
            DR1["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) > 0) ? ddlBrand.Text : txtCustomMedication.Text;

            DR1["StrengthValue"] = common.myStr(txtStrengthValue.Text);

            DR1["Dose"] = txtDose.Text;
            DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            DR1["Duration"] = txtDuration.Text;
            DR1["DurationText"] = Type;
            DR1["Type"] = ddlPeriodType.SelectedValue;

            DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

            DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
            DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.Text);
            DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
            DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.Text;
            DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
            DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
            DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
            DR1["PrescriptionDetail"] = string.Empty;
            DR1["CustomMedication"] = chkCustomMedication.Checked;
            DR1["Instructions"] = common.clearHTMLTags(txtInstructions.Text);
            DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
            DR1["ReferanceItemName"] = ddlReferanceItem.SelectedValue == string.Empty || ddlReferanceItem.SelectedValue == "0" ? string.Empty : ddlReferanceItem.Text;
            DR1["DoseTypeId"] = common.myInt(ddlDoseType.SelectedValue);
            DR1["DoseTypeName"] = ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0" ? string.Empty : ddlDoseType.Text;
            DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            if (ddlFormulation.SelectedItem != null)
            {
                DR1["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
            }
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
                DR1["FrequencyName"] = string.Empty;
            }
            else
            {
                DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                DR1["IsInfusion"] = false;
            }
            DR1["Volume"] = common.myStr(txtVolume.Text);
            DR1["VolumeUnitId"] = common.myInt(ddlVolumeUnit.SelectedValue);
            DR1["InfusionTime"] = common.myStr(txtTimeInfusion.Text);
            DR1["TimeUnit"] = common.myStr(ddlTimeUnit.SelectedValue);
            DR1["TotalVolume"] = common.myStr(txtTotalVolumn.Text);
            if (!string.IsNullOrEmpty(common.myStr(txtFlowRateUnit.Text)))
                DR1["FlowRate"] = common.myStr(txtFlowRateUnit.Text);
            else
                DR1["FlowRate"] = "0";
            DR1["FlowRateUnit"] = common.myInt(ddlFlowRateUnit.SelectedValue);
            DR1["XMLVariableDose"] = common.myStr(hdnXmlVariableDoseString.Value).Trim();
            DR1["XMLFrequencyTime"] = common.myStr(hdnXmlFrequencyTime.Value).Trim();

            //DR1["OverrideComments"] = common.myStr(txtCommentsDrugAllergy.Text);
            DR1["OverrideCommentsDrugToDrug"] = common.myStr(txtCommentsDrugToDrug.Text);
            //DR1["OverrideCommentsDrugHealth"] = common.myStr(txtCommentsDrugHealth.Text);
            DR1["IsSubstituteNotAllowed"] = chkSubstituteNotAllow.Checked;
            DR1["ICDCode"] = common.myStr(txtICDCode.Text);
            dt1.Rows.Add(DR1);
            dt1.AcceptChanges();
            //ddlReferanceItem.SelectedValue = "0";
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));

            txtInstructions.Text = string.Empty;
            // }
            #endregion
            dt1.TableName = "ItemDetail";

            dtTempLastData = dt1.Copy().Clone();
            dtTempLastData.Rows.Add(DR1.ItemArray);

            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            dtTempLastData.WriteXml(writer);

            string xmlSchema = writer.ToString();
            xmlSchema = xmlSchema.Replace("&lt;", "<");
            xmlSchema = xmlSchema.Replace("&gt;", ">");

            DR["XMLData"] = xmlSchema;

            DataTable dtPD = dt1.Copy().Clone();
            dtPD.Rows.Add(DR1.ItemArray);
            if (common.myStr(hdnXmlDoseString.Value) == "")
            {
                DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dtPD);
            }
            else
            {
                DR["PrescriptionDetail"] = common.myStr(hdnXmlDoseString.Value);
            }

            DR1["PrescriptionDetail"] = common.myStr(DR["PrescriptionDetail"]);
            DR1["XMLData"] = common.myStr(DR["XMLData"]);

            DR["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");

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

            foreach (DataRow dr in dt1.Rows) // Santosh
            {
                if (common.myInt(dr["IndentId"]) > 0)
                {
                    dv = new DataView(dt1);
                    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(dr["IndentId"]) + " AND ISNULL(ItemId,0)=" + common.myInt(dr["ItemId"]);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dr["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dv.ToTable());

                    }
                }
                //    dt1.AcceptChanges();

            }





            ViewState["ItemDetail"] = dt1;
            ViewState["Item"] = dt1;

            gvItem.DataSource = dt1;
            gvItem.DataBind();
            lblERXNo.Text = string.Empty;
            //lblPrescribedBy.Text = "Prescribed By: "+ common.myStr(Session["EmployeeName"]);


            ViewState["Edit"] = null;

            setVisiblilityInteraction();

            dvPharmacistInstruction.Visible = false;
            lnkPharmacistInstruction.Visible = false;

            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            txtStrengthValue.Enabled = true;

            try
            {
                hdnItemId.Value = "0";
                ddlBrand.Focus();
                ddlBrand.Items.Clear();
                ddlBrand.Text = string.Empty;
                ddlBrand.Enabled = true;
                ddlBrand.SelectedValue = null;

                hdnGenericId.Value = "0";
                ddlGeneric.Items.Clear();
                ddlGeneric.Text = string.Empty;
                ddlGeneric.SelectedValue = null;

                txtCustomMedication.Text = string.Empty;
            }
            catch
            { }

            txtStrengthValue.Text = string.Empty;

            txtInstructions.Text = string.Empty;
            //ddlFoodRelation.SelectedValue = "0";
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));

            ddlFoodRelation.Text = string.Empty;
            chkNotToPharmacy.Checked = false;

            ddlFrequencyId.Enabled = true;
            ddlReferanceItem.Enabled = true;
            txtDuration.Enabled = true;
            //ddlPeriodType.Enabled = true;


            ddlFrequencyId.SelectedIndex = 0;
            txtDose.Text = string.Empty; //"1";
            txtDuration.Text = string.Empty;
            //ddlPeriodType.SelectedValue = "D";
            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));

            //ddlUnit.SelectedValue = "0";
            ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue("0"));

            txtInstructions.Text = string.Empty;
            //ddlReferanceItem.SelectedValue = "0";
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));

            //ddlFoodRelation.SelectedValue = "0";
            ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue("0"));

            //ddlDoseType.SelectedValue = "0";
            ddlDoseType.SelectedIndex = ddlDoseType.Items.IndexOf(ddlDoseType.Items.FindItemByValue("0"));

            txtVolume.Text = string.Empty;
            txtTotalVolumn.Text = string.Empty;
            txtTimeInfusion.Text = string.Empty;
            txtFlowRateUnit.Text = string.Empty;

            //ddlFlowRateUnit.SelectedValue = "0";
            ddlFlowRateUnit.SelectedIndex = ddlFlowRateUnit.Items.IndexOf(ddlFlowRateUnit.Items.FindItemByValue("0"));

            //ddlVolumeUnit.SelectedValue = "0";
            ddlVolumeUnit.SelectedIndex = ddlVolumeUnit.Items.IndexOf(ddlVolumeUnit.Items.FindItemByValue("0"));

            //ddlTimeUnit.SelectedValue = "0";
            ddlTimeUnit.SelectedIndex = ddlTimeUnit.Items.IndexOf(ddlTimeUnit.Items.FindItemByValue("0"));

            //ddlFormulation.SelectedValue = "0";
            ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue("0"));

            //ddlRoute.SelectedValue = "0";
            ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue("0"));

            txtStartDate.SelectedDate = DateTime.Now;
            txtEndDate.SelectedDate = null;

            ViewState["Edit"] = null;
            txtCustomMedication.Text = string.Empty;
            hdnXmlVariableDoseString.Value = string.Empty;
            hdnvariableDoseDuration.Value = string.Empty;
            hdnvariableDoseFrequency.Value = string.Empty;
            hdnVariabledose.Value = string.Empty;
            hdnXmlFrequencyTime.Value = string.Empty;

            //txtCommentsDrugAllergy.Text = string.Empty;
            txtCommentsDrugToDrug.Text = string.Empty;
            //txtCommentsDrugHealth.Text = string.Empty;

            lblGenericName.Text = string.Empty;
            chkSubstituteNotAllow.Checked = false;
            //txtICDCode.Text = string.Empty;

            txtTotQty.Text = string.Empty;
            txtSpecialInstrucation.Text = string.Empty;

            btnCopyLastPrescription.Enabled = false;
            hdnXmlDoseString.Value = "";
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
            //   dsXmlData.Dispose();
            emr = null;
            dv.Dispose();
            dv1.Dispose();
            // dvRemoveBlank.Dispose();
            // DVItem.Dispose();
            //dvFilter.Dispose();
            //if (tblItem != null)
            //{
            //    tblItem.Dispose();
            //}
            //dtDetail.Dispose();
            //  dtPD.Dispose();
            dtTempLastData.Dispose();
        }
    }

    protected void chkCustomMedication_OnCheckedChanged(object sender, EventArgs e)
    {
        setCustomMedicationChange();
    }
    private void setCustomMedicationChange()
    {
        try
        {
            if (chkCustomMedication.Checked)
            {
                chkNotToPharmacy.Visible = true;
                txtCustomMedication.Text = string.Empty;
            }
            else
            {
                hdnGenericId.Value = "0";
                ddlGeneric.Text = string.Empty;
                ddlGeneric.SelectedValue = null;

                hdnItemId.Value = "0";
                ddlBrand.Text = string.Empty;
                ddlBrand.SelectedValue = null;

                //ddlBrand.SelectedValue = "*";
                ////hdnCIMSItemId.Value = "";
                ////hdnCIMSType.Value = "";
                ////hdnVIDALItemId.Value = "";
                ViewState["ISCalculationRequired"] = null;
                chkNotToPharmacy.Visible = false;
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

                txtStrengthValue.Text = common.myStr(ds.Tables[0].Rows[0]["StrengthValue"]);
                txtStrengthValue.Enabled = true;
                //}
                //else
                //{
                //    //ddlStrength.SelectedIndex = 0;
                //    //ddlStrength.Enabled = true;
                //}

                if (!common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).Equals(0))
                {
                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).ToString()));
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

                    ddlRoute.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
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
        }
    }
    protected void btnGetInfoGeneric_Click(object sender, EventArgs e)
    {
        try
        {
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            txtStrengthValue.Enabled = true;

            int GenericId = common.myInt(hdnGenericId.Value);
            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);

            ddlBrand.Focus();

            if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    if (common.myLen(hdnCIMSItemId.Value) < 2)
                    {
                        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //lblMessage.Text = "This Drug is not mapped with CIMS.";
                        //return;
                    }
                    chkIsInteraction(0);
                }
                //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                //{
                //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                //    {
                //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                //        //return;
                //    }
                //}
            }
        }
        catch
        {
        }
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
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

            string CIMSItemId = common.myStr(hdnCIMSItemId.Value);
            string CIMSType = common.myStr(hdnCIMSType.Value);
            int VIDALItemId = common.myInt(hdnVIDALItemId.Value);

            clearItemDetails();
            BindICDPanel();
            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;
            txtStrengthValue.Enabled = true;

            int iOT = 3;

            if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim() == "OT"
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim() == "CO")
            {
                iOT = 2;
            }
            else if (Request.QueryString["LOCATION"] != null && common.myStr(Request.QueryString["LOCATION"]).Trim() == "WARD"
                && Request.QueryString["DRUGORDERCODE"] != null && common.myStr(Request.QueryString["DRUGORDERCODE"]).Trim() == "CO")
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

                //ds = objPharmacy.getItemMaster(ItemId, string.Empty, string.Empty, 1, common.myInt(Session["HospitalLocationID"]),
                //                                common.myInt(Session["UserId"]), iOT, common.myInt(hdnStoreId.Value), common.myInt(Session["FacilityId"]));

                ds = objPharmacy.getItemAttributes(common.myInt(Session["HospitalLocationID"]), ItemId, common.myInt(hdnStoreId.Value),
                                            common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];

                    //comment for not set unit and route based on formulation
                    //setUnitAndRouteBasedOnFormulation(common.myInt(DR["FormulationId"]));

                    hdnGenericId.Value = common.myInt(DR["GenericId"]).ToString();
                    lblGenericName.Text = common.myStr(DR["GenericName"]);

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));

                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));

                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["ItemUnitId"]).ToString()));
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DR["FrequencyId"]).ToString()));

                    txtStrengthValue.Text = common.myStr(DR["StrengthValue"]);
                    txtDose.Text = common.myStr(DR["Dose"]);
                    txtDuration.Text = common.myStr(DR["Duration"]);

                    ViewState["UnitName"] = common.myStr(DR["ItemUnitName"]);
                    ViewState["ISCalculationRequired"] = common.myBool(DR["IsCalculated"]);
                    txtStrengthValue.Text = common.myStr(DR["StrengthName"]);

                    hdnInfusion.Value = common.myStr(DR["IsInfusion"]);
                    hdnIsInjection.Value = common.myStr(DR["IsInjection"]).ToString();
                    //ddlFormulation_OnSelectedIndexChanged(this, null);                   
                    //txtDose.Text = "1";

                    dvPharmacistInstruction.Visible = false;
                    if (common.myStr(DR["FieldCode"]) == "INSPH")
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
                    if (common.myStr(Session["OPIP"]) == "I")
                    {
                        //txtDuration.Text = "1";
                    }

                    //ddlStrength.Enabled = false;
                    //txtStrengthValue.Enabled = false;

                    if (common.myBool(DR["IsInfusion"]) || common.myBool(DR["IsInjection"]))
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = common.myStr(DR["ItemName"]);
                        item.Value = common.myStr(common.myInt(DR["ItemId"]));
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
                    }

                    ViewState["ISCalculationRequired"] = common.myBool(DR["CalculationRequired"]);
                    Session["ISCalculationRequired"] = common.myBool(DR["CalculationRequired"]);

                }

                if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        if (common.myLen(hdnCIMSItemId.Value) < 2)
                        {
                            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            //lblMessage.Text = "This Drug is not mapped with CIMS.";
                            //return;
                        }

                        chkIsInteraction(ItemId);
                    }
                    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    //    {
                    //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                    //        //return;
                    //    }
                    //}
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objPharmacy = null;
        }
    }
    protected void lnkStopMedication_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sRegId = Request.QueryString["RegId"] == null ? common.myStr(Session["RegistrationId"]) : common.myStr(Request.QueryString["RegId"]);
            string sEncId = Request.QueryString["EncId"] == null ? common.myStr(Session["EncounterId"]) : common.myStr(Request.QueryString["EncId"]);
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
        catch
        {
        }
    }
    protected void lnkLabHistory_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]);
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.OnClientClose = string.Empty;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
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
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
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
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HshIn.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            HshIn.Add("@chvPreviousMedication", "A");

            if (common.myStr(Session["OPIP"]) == "I")
            {
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
            }
            else
            {
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPMedicines", HshIn);
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
                                dvFilter.RowFilter = "IndentId =" + common.myInt(dr["IndentId"]).ToString();
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(dr["ItemId"]).ToString() + " AND IndentId =" + common.myInt(dr["IndentId"]).ToString();
                            }
                            string sPrescriptionString = objEMR.GetPrescriptionDetailStringV2(dvFilter.ToTable());
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
            objPharmacy = null;
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
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (hdnItemId.Value != string.Empty)
            {
                int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
                if (DoctorId == 0)
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
                //string strMsg = objEMR.SaveFavouriteDrugs(DoctorId, common.myInt(hdnItemId.Value), string.Empty, common.myInt(Session["UserID"]),
                //                         common.myInt(txtDose.Text), common.myInt(ddlUnit.SelectedValue), 0,
                //                        common.myInt(ddlFormulation.SelectedValue), common.myInt(ddlRoute.SelectedValue),
                //                        common.myInt(ddlFrequencyId.SelectedValue), common.myInt(txtDuration.Text),
                //                        common.myStr(ddlPeriodType.SelectedValue), common.myInt(ddlFoodRelation.SelectedValue),
                //                        common.myStr(txtStrengthValue.Text), common.myStr(txtInstructions.Text), common.myInt(ddlGeneric.SelectedValue));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveFavoriteMedicine";
                APIRootClass.SaveFavoriteMedicine objRoot = new global::APIRootClass.SaveFavoriteMedicine();
                objRoot.DoctorId = DoctorId;
                objRoot.ItemId = common.myInt(hdnItemId.Value);
                objRoot.FormularyType = string.Empty;
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.DoseId = common.myInt(txtDose.Text);
                objRoot.UnitId = common.myInt(ddlUnit.SelectedValue);
                objRoot.StrengthId = 0;
                objRoot.FormulationId = common.myInt(ddlFormulation.SelectedValue);
                objRoot.RouteId = common.myInt(ddlRoute.SelectedValue);
                objRoot.FrequencyId = common.myInt(ddlFrequencyId.SelectedValue);
                objRoot.Duration = common.myInt(txtDuration.Text);
                objRoot.DurationType = common.myStr(ddlPeriodType.SelectedValue);
                objRoot.FoodRelationshipId = common.myInt(ddlFoodRelation.SelectedValue);
                objRoot.StrengthValue = common.myStr(txtStrengthValue.Text);
                objRoot.Instructions = common.myStr(txtInstructions.Text);
                objRoot.GenericId = common.myInt(ddlGeneric.SelectedValue);
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                string strMsg = JsonConvert.DeserializeObject<string>(sValue);

                lblMessage.Text = strMsg;
                lblERXNo.Text = string.Empty;
                GetBrandFavouriteData(string.Empty);
                //clearControls();
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
            //objEMR = null;
        }
    }


    protected void lstFavourite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lstFavourite.PageIndex = e.NewPageIndex;
        GetBrandFavouriteData(txtFavouriteItemName.Text);
    }

    private void GetBrandFavouriteData(string ItemName)
    {
        if (rdoSearchMedication.SelectedValue != "F")
        {
            return;
        }

        int GenericId = 0;
        int ItemId = 0;
        int DoctorId = common.myInt(Request.QueryString["DoctorId"]);

        if (DoctorId == 0)
        {
            DoctorId = common.myInt(Session["LoginDoctorId"]);
        }
        if (DoctorId == 0)
        {
            DoctorId = common.myInt(Session["EmployeeId"]);
        }

        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
                                GenericId, DoctorId, string.Empty, ItemName.Replace("'", "''"));

            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                //lstFavourite.DataSource = dsSearch.Tables[0];
                //lstFavourite.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindFavourite();", true);
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
            objEMR = null;
        }
    }

    private void BlankFavouriteGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ItemId", typeof(int));
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
        dt.Columns.Add("GenericName", typeof(string));
        dt.Columns.Add("XmlVariableDose", typeof(string));
        DataRow dr = dt.NewRow();
        dr["ItemId"] = 0;
        dr["ItemName"] = string.Empty;
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        lstFavourite.DataSource = dt;
        lstFavourite.DataBind();
    }
    #endregion
    //protected void chkRemoveFavourite_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    lstFavourite.Columns[(byte)enumFavourite.Remove].Visible = chkRemoveFavourite.Checked;
    //}
    protected void btnSearchFavourite_OnClick(object sender, EventArgs e)
    {
        try
        {
            GetBrandFavouriteData(txtFavouriteItemName.Text);
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
            BindGrid(string.Empty, string.Empty);
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
        try
        {
            int ItemId = common.myInt(e.CommandArgument);

            if (e.CommandName == "selectItem")
            {
                if (ItemId > 0)
                {
                    chkCustomMedication.Checked = false;
                    //   setCustomMedicationChange();

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    LinkButton lnkItemName = (LinkButton)row.FindControl("lnkItemName");

                    HiddenField hdnFCIMSItemId = (HiddenField)row.FindControl("hdnFCIMSItemId");
                    HiddenField hdnFCIMSType = (HiddenField)row.FindControl("hdnFCIMSType");
                    HiddenField hdnFVIDALItemId = (HiddenField)row.FindControl("hdnFVIDALItemId");
                    HiddenField hdnXmlVariableDose = (HiddenField)row.FindControl("hdnXmlVariableDose");

                    hdnCIMSItemId.Value = common.myStr(hdnFCIMSItemId.Value);
                    hdnCIMSType.Value = common.myStr(hdnFCIMSType.Value);
                    hdnVIDALItemId.Value = common.myStr(hdnFVIDALItemId.Value);

                    ddlBrand.Text = common.myStr(lnkItemName.Text);
                    try
                    {
                        ddlBrand.SelectedValue = ItemId.ToString();
                    }
                    catch
                    { }
                    hdnItemId.Value = ItemId.ToString();

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
                    HiddenField hdnInstructions = (HiddenField)row.FindControl("hdnInstructions");

                    //comment for not set unit and route based on formulation
                    //setUnitAndRouteBasedOnFormulation(common.myInt(hdnFormulationId.Value));

                    //SetStrengthFormUnitRouteOfSelectedDrug();

                    hdnXmlVariableDoseString.Value = hdnXmlVariableDose.Value;

                    if (common.myDbl(hdnDose.Value) > 0)
                    {
                        txtDose.Text = common.myDbl(hdnDose.Value).ToString();
                    }
                    else
                    {
                        txtDose.Text = string.Empty;
                    }
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(hdnStrengthId.Value).ToString()));
                    txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));

                    ddlFormulation_OnSelectedIndexChanged(this, null);

                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                    txtInstructions.Text = common.myStr(hdnInstructions.Value);
                    if (common.myInt(hdnDuration.Value) > 0)
                    {
                        txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                    }
                    else
                    {
                        //txtDuration.Text = "1";
                    }

                    if (common.myLen(hdnDurationType.Value) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                    }

                    endDateChange();

                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));

                    //if (common.myInt(txtDose.Text).Equals(0))
                    //{
                    //    //txtDose.Text = "1";
                    //}

                    if (common.myStr(Session["OPIP"]) == "I")
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            //txtDuration.Text = "1";
                        }
                    }

                    //if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    //    {
                    //        if (common.myLen(hdnCIMSItemId.Value) < 2)
                    //        {
                    //            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //            //lblMessage.Text = "This Drug is not mapped with CIMS.";
                    //        }
                    //        else
                    //        {
                    //            chkIsInteraction(ItemId);
                    //        }
                    //    }
                    //    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //    //{
                    //    //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    //    //    {
                    //    //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //    //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                    //    //    }
                    //    //}
                    //}

                    if (common.myStr(hdnXmlVariableDose.Value) != "")
                    {
                        CallVariableDose(false);
                    }
                }
            }
            else if (e.CommandName == "ItemDelete")
            {
                int doctorid = Session["OPIP"] != null && common.myStr(Session["OPIP"]).Trim() == "I" ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["LoginDoctorId"]);
                if (doctorid == 0)
                {
                    doctorid = common.myInt(Session["LoginDoctorId"]);
                }
                //string sResult = objEMR.DeleteFavoriteDrugs(Session["OPIP"] != null && Session["OPIP"].ToString().Trim() == "I" ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["DoctorID"]),
                //                     ItemId, string.Empty, common.myInt(Session["UserId"]));
                string sResult = objEMR.DeleteFavoriteDrugs(doctorid, ItemId, string.Empty, common.myInt(Session["UserId"]));
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
    protected void lstFavourite_OnRowCommand1(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            //  int ItemId = common.myInt(e.CommandArgument);

            int ItemId = common.myInt(hdnFavouriteItemId.Value);
            int GenericId = common.myInt(hdnFavouriteGenericId.Value);
            if (e.CommandName == "selectItem")
            {
                if (ItemId > 0 || GenericId > 0)
                {
                    chkCustomMedication.Checked = false;
                    // setCustomMedicationChange();

                    // GridViewRow row = (GridViewRow)(((GridView)e.CommandSource).NamingContainer);
                    // Convert the row index stored in the CommandArgument
                    // property to an Integer.
                    int index = common.myInt(hdnFavouriteIndex.Value);

                    // Retrieve the row that contains the button clicked 
                    // by the user from the Rows collection.
                    GridViewRow row = lstFavourite.Rows[index];

                    LinkButton lnkItemName = (LinkButton)row.FindControl("lnkItemName");

                    HiddenField hdnFCIMSItemId = (HiddenField)row.FindControl("hdnFCIMSItemId");
                    HiddenField hdnFCIMSType = (HiddenField)row.FindControl("hdnFCIMSType");
                    HiddenField hdnFVIDALItemId = (HiddenField)row.FindControl("hdnFVIDALItemId");
                    //  HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                    HiddenField hdnGenericName = (HiddenField)row.FindControl("GenericName");

                    HiddenField hdnXmlVariableDose = (HiddenField)row.FindControl("hdnXmlVariableDose");

                    hdnCIMSItemId.Value = common.myStr(hdnFCIMSItemId.Value);
                    hdnCIMSType.Value = common.myStr(hdnFCIMSType.Value);
                    hdnVIDALItemId.Value = common.myStr(hdnFVIDALItemId.Value);



                    try
                    {


                        if (!ItemId.Equals(0))
                        {
                            ddlBrand.Text = common.myStr(lnkItemName.Text);
                            ddlBrand.SelectedValue = ItemId.ToString();
                        }
                        else if (!GenericId.Equals(0))
                        {
                            ddlGeneric.Text = common.myStr(lnkItemName.Text);
                            ddlGeneric.SelectedValue = GenericId.ToString();
                            hdnGenericId.Value = common.myStr(GenericId);
                        }


                        //ddlBrand.SelectedValue = ItemId.ToString();
                        //ddlGeneric.SelectedValue = common.myStr(GenericId);

                    }
                    catch (Exception Ex)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Error: " + Ex.Message;
                        objException.HandleException(Ex);
                    }
                    hdnItemId.Value = ItemId.ToString();
                    //hdnGenericIdAddToFav.Value= common.myStr(hdnGenericId.Value);

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
                    HiddenField hdnInstructions = (HiddenField)row.FindControl("hdnInstructions");

                    //comment for not set unit and route based on formulation
                    //setUnitAndRouteBasedOnFormulation(common.myInt(hdnFormulationId.Value));

                    //SetStrengthFormUnitRouteOfSelectedDrug();

                    hdnXmlVariableDoseString.Value = hdnXmlVariableDose.Value;
                    if (common.myDbl(hdnDose.Value) > 0)
                    {
                        txtDose.Text = common.myDbl(hdnDose.Value).ToString();
                    }
                    else
                    {
                        txtDose.Text = string.Empty;
                    }
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                    //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(hdnStrengthId.Value).ToString()));
                    txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));

                    ddlFormulation_OnSelectedIndexChanged(this, null);

                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(hdnFrequencyId.Value).ToString()));
                    txtInstructions.Text = common.myStr(hdnInstructions.Value);
                    if (common.myInt(hdnDuration.Value) > 0)
                    {
                        txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                    }
                    else
                    {
                        //txtDuration.Text = "1";
                    }

                    if (common.myLen(hdnDurationType.Value) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                    }

                    endDateChange();

                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));

                    //if (common.myInt(txtDose.Text).Equals(0))
                    //{
                    //    //txtDose.Text = "1";
                    //}

                    if (common.myStr(Session["OPIP"]) == "I")
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            //txtDuration.Text = "1";
                        }
                    }

                    //if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    //    {
                    //        if (common.myLen(hdnCIMSItemId.Value) < 2)
                    //        {
                    //            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //            //lblMessage.Text = "This Drug is not mapped with CIMS.";
                    //        }
                    //        else
                    //        {
                    //            chkIsInteraction(ItemId);
                    //        }
                    //    }
                    //    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //    //{
                    //    //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                    //    //    {
                    //    //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //    //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                    //    //    }
                    //    //}
                    //}
                }
            }
            else if (e.CommandName == "ItemDelete")
            {
                int doctorid = Session["OPIP"] != null && common.myStr(Session["OPIP"]).Trim() == "I" ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["LoginDoctorId"]);
                if (doctorid == 0)
                {
                    doctorid = common.myInt(Session["LoginDoctorId"]);
                }
                //string sResult = objEMR.DeleteFavoriteDrugs(Session["OPIP"] != null && Session["OPIP"].ToString().Trim() == "I" ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["DoctorID"]),
                //                     ItemId, string.Empty, common.myInt(Session["UserId"]));
                string sResult = objEMR.DeleteFavoriteDrugs(doctorid, ItemId, string.Empty, common.myInt(Session["UserId"]), GenericId);
                lblMessage.Text = "Favourite Deleted";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                GetBrandFavouriteData(string.Empty);
            }
            // ddlGeneric.Focus();
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
    protected void btnProceedFavourite_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["ProceedValue"] = "F";
            proceedFavourite(false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void proceedFavourite(bool IsInContinue)
    {
        try
        {
            bool isFirst = true;
            foreach (GridViewRow dataItem in lstFavourite.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnFItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                    if (common.myInt(hdnFItemId.Value) > 0)
                    {
                        LinkButton lnkItemName = (LinkButton)dataItem.FindControl("lnkItemName");

                        ddlBrand.Text = common.myStr(lnkItemName.Text);
                        try
                        {
                            ddlBrand.SelectedValue = hdnFItemId.Value;
                        }
                        catch
                        { }
                        hdnItemId.Value = hdnFItemId.Value;

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
                            txtDose.Text = common.myDbl(hdnDose.Value).ToString();
                        }
                        else
                        {
                            txtDose.Text = string.Empty;
                        }

                        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(hdnUnitId.Value).ToString()));
                        //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(hdnStrengthId.Value).ToString()));
                        txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(hdnFormulationId.Value).ToString()));

                        ddlFormulation_OnSelectedIndexChanged(this, null);

                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(hdnRouteId.Value).ToString()));
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(hdnFrequencyId.Value).ToString()));

                        if (common.myInt(hdnDuration.Value) > 0)
                        {
                            txtDuration.Text = common.myInt(hdnDuration.Value).ToString();
                        }
                        else
                        {
                            //txtDuration.Text = "1";
                        }

                        if (common.myLen(hdnDurationType.Value) > 0)
                        {
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(hdnDurationType.Value).ToString()));
                        }
                        else
                        {
                            ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                        }

                        endDateChange();

                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(hdnFoodRelationshipId.Value).ToString()));

                        //if (common.myInt(txtDose.Text).Equals(0))
                        //{
                        //    //txtDose.Text = "1";
                        //}

                        if (common.myStr(Session["OPIP"]) == "I")
                        {
                            if (common.myInt(txtDuration.Text).Equals(0))
                            {
                                //txtDuration.Text = "1";
                            }
                        }

                        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                        {
                            if (common.myBool(Session["IsCIMSInterfaceActive"]))
                            {
                                if (common.myLen(hdnCIMSItemId.Value) < 2)
                                {
                                    //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                    //lblMessage.Text = "This Drug is not mapped with CIMS.";
                                }
                                else
                                {
                                    chkIsInteraction(common.myInt(hdnFItemId.Value));
                                }
                            }
                            //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                            //{
                            //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                            //    {
                            //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                            //    }
                            //}
                        }

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
                        if (ItemId == 0 && GenericId == 0)
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
                            txtStrengthValue.Enabled = true;
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
                            if (ItemId > 0)
                            {
                                ddlBrand.Text = common.myStr(lnkItemName.Text);
                                ddlBrand.SelectedValue = ItemId.ToString();
                            }
                            else if (GenericId > 0)
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

                        hdnGenericName.Value = common.myStr(lnkItemName.Text);
                        hdnItemName.Value = common.myStr(lnkItemName.Text);
                        lblGenericName.Text = common.myStr(lnkItemName.Text);
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myStr(hdnFormulationId.Value)));
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myStr(hdnRouteId.Value)));
                        //ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myStr(hdnStrengthId.Value)));
                        txtStrengthValue.Text = common.myStr(hdnStrengthValue.Value);
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

                    if (ddlIndentType.SelectedValue == string.Empty)
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
                    DR1["Qty"] = dtItemDetail.Rows[counter]["Qty"];
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
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    dt1.WriteXml(writer);

                    string xmlSchema = writer.ToString();
                    xmlSchema = xmlSchema.Replace("&lt;", "<");
                    xmlSchema = xmlSchema.Replace("&gt;", ">");

                    DR["XMLData"] = xmlSchema;
                    DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dt1);
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
            emr = null;
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
                    BindGrid(string.Empty, string.Empty);

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
        finally
        {
        }
    }
    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0)
            {
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0 || common.myInt(ViewState["StopGenericId"]) > 0)
            {
                Hashtable hshOutput = new Hashtable();

                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                               common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]), common.myInt(Session["RegistrationId"]),
                               common.myInt(Session["EncounterId"]), 0, txtStopRemarks.Text, common.myStr(Session["OPIP"]),
                               common.myInt(ViewState["IndentDetailsId"]), common.myInt(ViewState["StopGenericId"]), common.myInt(Session["UserId"]));

                BindGrid(string.Empty, string.Empty);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindCurrentMedication();", true);
                txtStopRemarks.Text = string.Empty;
                ViewState["IndentDetailsId"] = "0";
                if (common.myStr(hshOutput["@chvOutPut"]).Contains("at least 10 Character"))
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
                }
                else
                {
                    lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
                }



            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);

                return;
            }

            ViewState["StopItemId"] = string.Empty;
            ViewState["StopIndentId"] = string.Empty;
            ViewState["StopGenericId"] = string.Empty;

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
        if (common.myStr(hdnDateWise.Value) == "1")
        {
            if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
            {
                Cache.Insert("VariableDose" + common.myStr(Session["UserId"]), common.myStr(hdnXmlVariableDoseString.Value), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = common.myStr(hdnXmlVariableDoseString.Value);
            }
            else
            {
                Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = null;
            }
        }
        else
        {
            if (!common.myStr(hdnXmlVariableDoseString.Value).Trim().Equals(string.Empty))
            {
                Cache.Insert("VariableMultipleDose" + common.myStr(Session["UserId"]), common.myStr(hdnXmlVariableDoseString.Value), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                Cache.Remove("VariableMultipleDose" + common.myStr(Session["UserId"]));
            }
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

        RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?Day=" + common.myInt(txtDuration.Text) + "&DurationType=" + common.myStr(ddlPeriodType.Text) + "&FrequencyId=" + common.myInt(ddlFrequencyId.SelectedValue)
            + "&Dose=" + txtDose.Text + "&UnitName=" + common.myStr(ddlUnit.Text) + "&IName=" + ddlBrand.Text + "&DisplayMsg=" + dosemessage + "&Route=" + common.myStr(ddlRoute.Text)
            + "&StartDate=" + common.myDate(txtStartDate.SelectedDate).ToString("yyyy/MM/dd") + "&Instructions=" + txtInstructions.Text.Replace("\n", "<br/>");
        RadWindow1.Height = 500;
        RadWindow1.Width = 800;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;



        RadWindow1.OnClientClose = "OnClientCloseVariableDose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
        return 0;
    }

    protected void btnVariableDoseClose_OnClick(object sender, EventArgs e)
    {
        calcTotalQtyForVariableDose(common.myStr(hdnXmlVariableDoseString.Value), common.myInt(hdnNoOfDose.Value));
    }
    #region comments for Variable dose
    //protected void btnVariableDose_OnClick(object sender, EventArgs e)
    //{
    //    CallVariableDose(false);
    //}
    //private int CallVariableDose(bool checkforDose)
    //{
    //    lblMessage.Text = string.Empty;
    //    if (common.myStr(ddlDoseType.Text) == "PRN" || common.myStr(ddlDoseType.Text) == "STAT")
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Variable Dose not applicable for " + common.myStr(ddlDoseType.Text);
    //        return 0;
    //    }
    //    if (common.myStr(ddlDoseType.Text) != "PRN" || common.myStr(ddlDoseType.Text) != "STAT")
    //    {
    //        if (common.myInt(ddlFrequencyId.SelectedValue) == 0)
    //        {
    //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //            lblMessage.Text = "Please select frequency.";
    //            return 0;
    //        }
    //        if (common.myInt(txtDuration.Text) == 0)
    //        {
    //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //            lblMessage.Text = "Please enter duration values.";
    //            return 0;
    //        }
    //        if (common.myStr(ddlPeriodType.SelectedValue) != "D")
    //        {
    //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //            lblMessage.Text = "Variable dose allow only for Days(s).";
    //            return 0;
    //        }
    //    }
    //    int dosemessage = 0;
    //    if (common.myStr(hdnXmlVariableDoseString.Value).Trim() != string.Empty)
    //    {
    //        Cache.Insert("VariableDose" + common.myStr(Session["UserId"]), hdnXmlVariableDoseString.Value, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
    //    }
    //    else
    //    {
    //        Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
    //    }
    //    if (checkforDose && common.myStr(hdnXmlVariableDoseString.Value).Trim() != string.Empty)
    //    {
    //        dosemessage = 1;
    //        if (common.myInt(ddlFrequencyId.SelectedValue) == common.myInt(hdnvariableDoseFrequency.Value))
    //        {
    //            if (common.myDbl(txtDose.Text) == common.myDbl(hdnVariabledose.Value))
    //            {
    //                if (common.myDbl(txtDuration.Text) == common.myDbl(hdnvariableDoseDuration.Value))
    //                {
    //                    // Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
    //                    return 1;
    //                }
    //            }
    //        }
    //    }

    //    // RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text+ "&IName=" + ddlBrand.Text;
    //    RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text + "&IName=" + ddlBrand.Text + "&DisplayMsg=" + dosemessage;
    //    RadWindow1.Height = 500;
    //    RadWindow1.Width = 500;
    //    RadWindow1.Top = 20;
    //    RadWindow1.Left = 20;
    //    RadWindow1.OnClientClose = "OnClientCloseVariableDose";
    //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //    RadWindow1.Modal = true;
    //    // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    //    RadWindow1.VisibleStatusbar = false;
    //    return 0;
    //    //}
    //    //else
    //    //{
    //    //    // RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text+ "&IName=" + ddlBrand.Text;
    //    //    RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text + "&IName=" + ddlBrand.Text + "&DisplayMsg=" + dosemessage;
    //    //    RadWindow1.Height = 500;
    //    //    RadWindow1.Width = 500;
    //    //    RadWindow1.Top = 20;
    //    //    RadWindow1.Left = 20;
    //    //    RadWindow1.OnClientClose = "OnClientCloseVariableDose";
    //    //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //    //    RadWindow1.Modal = true;
    //    //    // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    //    //    RadWindow1.VisibleStatusbar = false;
    //    //    return 0;
    //    //}

    //}
    #endregion
    protected void lbtnFrequencyTime_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlFrequencyId.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please select frequency.", Page);
            return;
        }
        if (common.myInt(txtDuration.Text) == 0)
        {
            Alert.ShowAjaxMsg("Please enter duration values.", Page);
            return;
        }
        //if (common.myInt(ddlBrand.SelectedValue).Equals(0))
        if (common.myInt(hdnItemId.Value).Equals(0))
        {
            Alert.ShowAjaxMsg("Please select Drug", Page);
            return;
        }
        int days = 0;
        if (common.myStr(ddlPeriodType.SelectedValue) == "D")
        {
            days = common.myInt(txtDuration.Text);
        }
        else if (common.myStr(ddlPeriodType.SelectedValue) == "W")
        {
            days = common.myInt(txtDuration.Text) * 7;
        }
        else if (common.myStr(ddlPeriodType.SelectedValue) == "M")
        {
            days = common.myInt(txtDuration.Text) * 30;
        }
        else
        {
            days = 1;
        }

        RadWindow1.NavigateUrl = "/EMR/Medication/DoseTime.aspx?Day=" + days +
                                "&FrequencyId=" + ddlFrequencyId.SelectedValue +
                                "&ItemId=" + common.myInt(hdnItemId.Value) +
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
    private void CallDoseTime(bool checkforDose)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ddlDoseType.Text) == "PRN" || common.myStr(ddlDoseType.Text) == "STAT")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Variable Dose not applicable for " + common.myStr(ddlDoseType.Text);
                // return 0;
            }
            if (common.myStr(ddlDoseType.Text) != "PRN" || common.myStr(ddlDoseType.Text) != "STAT")
            {
                if (common.myInt(ddlFrequencyId.SelectedValue) == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select frequency.";
                    // return 0;
                }
                if (common.myInt(txtDuration.Text) == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter duration values.";
                    //  return 0;
                }
                if (common.myStr(ddlPeriodType.SelectedValue) != "D")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Variable dose allow only for Days(s).";
                    // return 0;
                }
            }
            int dosemessage = 0;
            if (common.myStr(hdnXmlVariableDoseString.Value).Trim() != string.Empty)
            {
                Cache.Insert("VariableDose" + common.myStr(Session["UserId"]), hdnXmlVariableDoseString.Value, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);
                Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = common.myStr(hdnXmlVariableDoseString.Value);
            }
            else
            {
                Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                Session["VariableDose_" + common.myStr(Session["UserId"]) + "_" + common.myStr(Session["EncounterId"])] = null;
            }
            if (checkforDose == true && common.myStr(hdnXmlVariableDoseString.Value).Trim() != string.Empty)
            {
                dosemessage = 1;
                if (common.myInt(ddlFrequencyId.SelectedValue) == common.myInt(hdnvariableDoseFrequency.Value))
                {
                    if (common.myDbl(txtDose.Text) == common.myDbl(hdnVariabledose.Value))
                    {
                        if (common.myDbl(txtDuration.Text) == common.myDbl(hdnvariableDoseDuration.Value))
                        {
                            // Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                            // return 1;
                        }
                    }
                }
            }

            //  return 0;
            //}
            //else
            //{
            //    // RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text+ "&IName=" + ddlBrand.Text;
            //    RadWindow1.NavigateUrl = "/EMR/Medication/VariableDose.aspx?Day=" + txtDuration.Text + "&FrequencyId=" + ddlFrequencyId.SelectedValue + "&Dose=" + txtDose.Text + "&IName=" + ddlBrand.Text + "&DisplayMsg=" + dosemessage;
            //    RadWindow1.Height = 500;
            //    RadWindow1.Width = 500;
            //    RadWindow1.Top = 20;
            //    RadWindow1.Left = 20;
            //    RadWindow1.OnClientClose = "OnClientCloseVariableDose";
            //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //    RadWindow1.Modal = true;
            //    // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
            //    RadWindow1.VisibleStatusbar = false;
            //    return 0;
            //}
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
    private void setPatientInfo()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            int? weight = null;

            ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Weight.Text = string.Empty;
                txtHeight.Text = string.Empty;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Gender"))
                    {
                        ViewState["PatientGender"] = common.myStr(ds.Tables[0].Rows[i][1]);
                    }
                    else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("Age"))
                    {
                        ViewState["PatientDOB"] = DateTime.Now.AddDays(-common.myInt(ds.Tables[0].Rows[i][1])).ToString("yyyy-MM-dd");
                    }
                    if (i > 1)
                    {
                        if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("WT"))// Weight
                        {
                            weight = common.myInt(ds.Tables[0].Rows[i][1]);
                            lbl_Weight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                        else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("HT"))// Height
                        {
                            txtHeight.Text = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                    }
                }
            }

            ViewState["PatientWeight"] = weight;
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
    private void getLegnedColor()
    {
        BaseC.clsBb objBb = new BaseC.clsBb(sConString);
        DataSet ds = new DataSet();
        try
        {
            ViewState["BrandDetailsColor"] = "#FFC48A";
            ViewState["DrugMonographColor"] = "#98AFC7";
            ViewState["DrugtoDrugInteractionColor"] = "#ECBBBB";
            ViewState["DrugHealthInteractionColor"] = "#82AB76";
            ViewState["DrugAllergyColor"] = "#82CAFA";

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                ds = objBb.GetStatusMaster("CIMSInterface");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "Code='BD'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["BrandDetailsColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='MO'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugMonographColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='IN'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugtoDrugInteractionColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='HI'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugHealthInteractionColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;

                    ds.Tables[0].DefaultView.RowFilter = "Code='DA'";

                    if (ds.Tables[0].DefaultView.Count > 0)
                    {
                        ViewState["DrugAllergyColor"] = ds.Tables[0].DefaultView[0]["StatusColor"];
                    }

                    ds.Tables[0].DefaultView.RowFilter = string.Empty;
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
            objBb = null;
            ds.Dispose();
        }
    }
    private void setGridColor()
    {
        //if (common.myBool(Session["IsCIMSInterfaceActive"])
        //    || common.myBool(Session["IsVIDALInterfaceActive"]))
        //{
        //    foreach (GridViewRow dataItem in gvItem.Rows)
        //    {
        //        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        //        {
        //            ImageButton lnkBtnMonographCIMS = (ImageButton)dataItem.FindControl("lnkBtnMonographCIMS");
        //            ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

        //            //LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");                    
        //            //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
        //            //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");

        //            //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
        //            //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

        //            //lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));                    
        //            //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
        //            //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        //        }
        //        //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
        //        //{
        //        //    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsVIDAL");
        //        //    LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
        //        //    LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
        //        //    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
        //        //    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionVIDAL");

        //        //    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
        //        //    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
        //        //    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
        //        //    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
        //        //    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        //        //}
        //    }

        //    //foreach (GridViewRow dataItem in gvPrevious.Rows)
        //    //{
        //    //    if (common.myBool(Session["IsCIMSInterfaceActive"]))
        //    //    {
        //    //        //ImageButton lnkBtnMonographCIMS = (ImageButton)dataItem.FindControl("lnkBtnMonographCIMS");
        //    //        //ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

        //    //        //LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");                    
        //    //        //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
        //    //        //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");

        //    //        //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
        //    //        //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

        //    //        //lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));                    
        //    //        //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
        //    //        //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        //    //    }
        //    //    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
        //    //    //{
        //    //    //    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsVIDAL");
        //    //    //    LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
        //    //    //    LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
        //    //    //    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");
        //    //    //    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionVIDAL");

        //    //    //    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
        //    //    //    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
        //    //    //    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
        //    //    //    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
        //    //    //    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        //    //    //}
        //    //}

        //    lnkDrugAllergy.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        //}
    }
    private void setDiagnosis()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
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
                ds = objEMR.getDiagnosis(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
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
                    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    List<string> list = new List<string>();

                    //    foreach (DataRow DR in ds.Tables[0].Rows)
                    //    {
                    //        if (common.myStr(DR["ICDCode"]).Trim().Length > 0)
                    //        {
                    //            list.Add(common.myStr(DR["ICDCode"]).Trim().Replace(".", string.Empty));
                    //        }
                    //    }
                    //    ViewState["PatientDiagnosisXML"] = list;
                    //}
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
            ds.Dispose();
        }
    }
    private void setAllergiesWithInterfaceCode()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            ViewState["PatientAllergyXML"] = string.Empty;

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                ds = objEMR.getDrugAllergiesInterfaceCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]));
                DV = ds.Tables[0].DefaultView;
                tbl = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
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
                    //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    //{
                    //    DV.RowFilter = "AllergyType='VIDAL'";
                    //    tbl = DV.ToTable();

                    //    if (tbl.Rows.Count > 0)
                    //    {
                    //        List<int?> list = new List<int?>();

                    //        foreach (DataRow DR in tbl.Rows)
                    //        {
                    //            if (common.myStr(DR["InterfaceCode"]).Trim().Length > 0)
                    //            {
                    //                list.Add(common.myInt(DR["InterfaceCode"]));
                    //            }
                    //        }

                    //        int?[] allergyIds = list.ToArray();

                    //        ViewState["PatientAllergyXML"] = allergyIds;
                    //    }
                    //}


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
            ds.Dispose();
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
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemId.Value) < 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Generic or Brnad not selected ";
                return;
            }

            showMonograph(common.myStr(hdnCIMSItemId.Value).Trim(), common.myStr(hdnCIMSType.Value).Trim());
        }
    }

    protected void btnMonographViewClick_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            if (common.myLen(hdnCIMSItemIdClick.Value) < 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Generic or Brnad not selected ";
                return;
            }

            showMonograph(common.myStr(hdnCIMSItemIdClick.Value).Trim(), common.myStr(hdnCIMSTypeClick.Value).Trim());
        }
    }

    private void showMonograph(string CIMSItemId, string CIMSType)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = getMonographXML(CIMSType, common.myStr(CIMSItemId));

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }
    private void showBrandDetails(string CIMSItemId, string CIMSType)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = getBrandDetailsXMLCIMS(CIMSType, common.myStr(CIMSItemId));

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(true);
        }
    }
    protected void btnInteractionView_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";

            string strXML = getAllInterationXML(strPrescribing);

            if (strXML != string.Empty)
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
    }
    private void showIntreraction()
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getAllInterationXML(string.Empty);

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }
    private void showHealthOrAllergiesIntreraction(string HealthOrAllergies)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }

            string strXML = getHealthOrAllergiesInterationXML("B", string.Empty);

            if (strXML != string.Empty)
            {
                Session["CIMSXMLInputData"] = strXML;
                openWindowsCIMS(false);
            }
        }
        //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
        //{
        //    if (common.myStr(HealthOrAllergies).Equals("H"))//Health
        //    {
        //        int?[] commonNameGroupIds = getVIDALCommonNameGroupIds().ToArray();

        //        if (commonNameGroupIds.Length > 0)
        //        {
        //            getDrugHealthInteractionVidal(commonNameGroupIds);
        //        }
        //    }
        //    else if (common.myStr(HealthOrAllergies).Equals("A"))//Allergies
        //    {
        //        int?[] commonNameGroupIds = getVIDALCommonNameGroupIds().ToArray();

        //        if (commonNameGroupIds.Length > 0)
        //        {
        //            getDrugAllergyVidal(commonNameGroupIds);
        //        }
        //    }
        //}
    }
    private void chkIsInteraction(int ItemId)
    {
        try
        {
            if (common.myStr(hdnCIMSItemId.Value).ToUpper().Equals("UNDEFINED"))
            {
                return;
            }

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

                btnMonographViewOnItemBased.Visible = false;
                btnInteractionViewOnItemBased.Visible = false;

                btnMonographView.Visible = false;
                btnInteractionView.Visible = false;

                //spnCommentsDrugAllergy.Visible = false;
                spnCommentsDrugToDrug.Visible = false;
                //spnCommentsDrugHealth.Visible = false;

                //txtCommentsDrugAllergy.Enabled = false;
                txtCommentsDrugToDrug.Enabled = false;
                //txtCommentsDrugHealth.Enabled = false;

                //txtCommentsDrugAllergy.Text = string.Empty;
                txtCommentsDrugToDrug.Text = string.Empty;
                //txtCommentsDrugHealth.Text = string.Empty;

                lblIntreactionMessage.Text = string.Empty;
                txtInteractionBetweenMessage.Text = string.Empty;

                //string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";
                string strPrescribing = "<" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";

                string strXML = getAllInterationXML(strPrescribing);
                //mmb
                //strXML = cimsFullXml();
                if (strXML != string.Empty)
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
                        if (objCIMS.IsDrugToAllInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues, true) > 0)
                        {
                            string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                            {
                                ViewState["NewPrescribing"] = strXML;

                                //if (outputValues.ToUpper().Contains("<SEVERITY NAME=\"SEVERE\" RANKING=\"5\">SEVERE</SEVERITY>"))

                                dvInteraction.Visible = true;

                                //btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));

                                spnCommentsDrugToDrug.Visible = true;
                                txtCommentsDrugToDrug.Enabled = true;

                                btnInteractionView.Visible = true;
                                btnInteractionViewOnItemBased.Visible = true;
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
                        //if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
                        //{
                        //    string strPatternMatch = "<" + common.myStr(hdnCIMSType.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                        //    ViewState["NewPrescribing"] = strXML;

                        //    dvInteraction.Visible = true;

                        //    btnBrandDetailsView.Visible = (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"));

                        //    spnCommentsDrugAllergy.Visible = true;
                        //    txtCommentsDrugAllergy.Enabled = true;
                        //}

                        if (dvInteraction.Visible)
                        {
                            txtInteractionBetweenMessage.Text = getInterfaceBetweenAllInteraction(strXML);
                            txtInteractionBetweenMessage.ToolTip = txtInteractionBetweenMessage.Text;

                            if (common.myLen(txtInteractionBetweenMessage.Text).Equals(0))
                            {
                                dvInteraction.Visible = false;
                                spnCommentsDrugToDrug.Visible = false;
                                txtCommentsDrugToDrug.Enabled = false;
                            }
                            else
                            {
                                btnBrandDetailsViewOnItemBased.Visible = false;
                                btnMonographViewOnItemBased.Visible = false;

                                strXML = getMonographXML(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                                if (strXML != string.Empty)
                                {
                                    outputValues = objCIMS.getFastTrack5Output(strXML);

                                    if (outputValues != null)
                                    {
                                        if (outputValues.ToUpper().Contains("<MONOGRAPH>"))
                                        {
                                            btnMonographViewOnItemBased.Visible = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            //{
            //    //return;
            //}

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string cimsFullXml()
    {
        StringBuilder sbPrescribing = new StringBuilder();


        sbPrescribing.Append("<Request>");
        sbPrescribing.Append("<Interaction>");
        sbPrescribing.Append("<Prescribing>");
        sbPrescribing.Append("<GGPI reference=\"{488F9F61-5D37-4989-925E-1742FFFDAA9E}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{2AA7FC83-2E12-496E-98A8-5F0B8D548907}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{BF33752F-6062-0589-E034-0003BA299378}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{B3E696CF-71F8-6518-E034-080020E1DD8C}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{C1DDD75F-FD9D-535C-E034-0003BA299378}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{AA0D6353-F6E1-4AB1-AF1E-5E5F109A0706}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{B3E6B75E-9519-6AE7-E034-080020E1DD8C}\"/>");
        sbPrescribing.Append("<GGPI reference=\"{B7AD6ED9-C22D-561C-E034-080020E1DD8C}\"/>");
        sbPrescribing.Append("</Prescribing>");
        sbPrescribing.Append("<Allergies>");
        sbPrescribing.Append("<Molecule reference=\"{5143B2B1-DBED-45A2-8308-61636C3A2F0B}\"/>");
        sbPrescribing.Append("<Molecule reference=\"{7142A19F-EDE2-4A7C-A30B-06AB60B6B71F}\"/>");
        sbPrescribing.Append("<Molecule reference=\"{74DCDDAF-9349-430F-A2F2-B7F2BC5CD8AA}\"/>");
        sbPrescribing.Append("<SubstanceClass reference=\"{8B9ECB52-CDC9-4460-AB40-6E3C7DF38B1E}\"/>");
        sbPrescribing.Append("<SubstanceClass reference=\"{9E775009-231D-728D-E034-080020E1DD8C}\"/>");
        sbPrescribing.Append("</Allergies>");
        sbPrescribing.Append("<HealthIssueCodes>");
        sbPrescribing.Append("<HealthIssueCode code=\"J45\" codeType=\"ICD10\"/>");
        sbPrescribing.Append("<HealthIssueCode code=\"N17\" codeType=\"ICD10\"/>");
        sbPrescribing.Append("<HealthIssueCode code=\"I49.5\" codeType=\"ICD10\"/>");
        sbPrescribing.Append("</HealthIssueCodes>");
        sbPrescribing.Append("<References/>");
        sbPrescribing.Append("<DuplicateTherapy checkSameDrug=\"true\" />");
        sbPrescribing.Append("<DuplicateIngredient checkSameDrug=\"true\" />");
        sbPrescribing.Append("</Interaction>");
        sbPrescribing.Append("<PatientProfile>");
        sbPrescribing.Append("<Gender>F</Gender>");
        sbPrescribing.Append("<Age><Year>34</Year><Month>8</Month><Day>11</Day></Age>");
        sbPrescribing.Append("<Pregnancy><Week>20</Week></Pregnancy>");
        sbPrescribing.Append("<Nursing>true</Nursing>");
        sbPrescribing.Append("</PatientProfile>");
        sbPrescribing.Append("</Request>");

        return sbPrescribing.ToString();
    }

    private string getInterfaceBetweenAllInteraction(string strXML)
    {
        StringBuilder sbOutput = new StringBuilder();

        Session["CIMSXMLInputData"] = strXML;
        objCIMS = new clsCIMS();
        bool isExecute = false;


        int DrugInteraction = 0;
        int Allergy = 0;
        int HealthConditions = 0;
        int DuplicateTherapy = 0;
        int DuplicateIngredient = 0;
        int Pregnancy = 0;
        int Lactation = 0;

        try
        {
            string strFinalOutput = objCIMS.getCIMSFinalOutupt(false);

            using (StringReader reader = new StringReader(strFinalOutput))
            {
                string linePrevious = string.Empty;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!isExecute)
                    {
                        isExecute = true;
                        #region tab

                        /*    
                        

                        All Tab
                        try
                        {
                            int startIdx = line.IndexOf("<div id=\"tabs\" class=\"tab-container\">") + 37;
                            if (startIdx > 37)
                            {
                                string strTemp = line.Substring(startIdx, line.Length - startIdx);
                                strTemp = strTemp.Substring(0, strTemp.IndexOf("</ul>"));

                                if (common.myLen(strTemp) > 10)
                                {
                                    startIdx = strTemp.IndexOf("<a href=\"#tab-interaction\">") + 27;
                                    string strTab = string.Empty;
                                    if (startIdx > 27)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }


                                    startIdx = strTemp.IndexOf("<a href=\"#tab-allergy\">") + 23;
                                    if (startIdx > 23)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }

                                    startIdx = strTemp.IndexOf("<a href=\"#tab-health\">") + 22;
                                    if (startIdx > 22)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }


                                    startIdx = strTemp.IndexOf("<a href=\"#tab-dup\">") + 19;
                                    if (startIdx > 19)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }


                                    startIdx = strTemp.IndexOf("<a href=\"#tab-duping\">") + 22;
                                    if (startIdx > 22)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }


                                    startIdx = strTemp.IndexOf("<a href=\"#tab-preg\">") + 20;

                                    if (startIdx > 20)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }


                                    startIdx = strTemp.IndexOf("<a href=\"#tab-lact\">") + 20;
                                    if (startIdx > 20)
                                    {
                                        strTab = strTemp.Substring(startIdx, strTemp.Length - startIdx);
                                        strTab = strTab.Substring(0, strTab.IndexOf("</a>"));

                                        if (common.myLen(strTab) > 2)
                                        {
                                            if (!sbOutput.ToString().Equals(string.Empty))
                                            {
                                                sbOutput.Append(Environment.NewLine);
                                            }
                                            sbOutput.Append(strTab.Trim());
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        */

                        #endregion
                    }

                    if (line.Contains(" vs "))
                    {
                        if (!sbOutput.ToString().Equals(string.Empty))
                        {
                            sbOutput.Append(Environment.NewLine);
                        }

                        if (line.Trim().StartsWith("vs "))
                        {
                            //D2H

                            HealthConditions++;

                            if (HealthConditions.Equals(1))
                            {
                                sbOutput.Append(Environment.NewLine + "Health Conditions" + Environment.NewLine);
                            }
                            sbOutput.Append(HealthConditions.ToString() + ". " + (linePrevious.Trim() + " " + line.Trim()).Trim());
                        }
                        else
                        {
                            //D2D

                            if (line.Trim().Length < 400)
                            {
                                DrugInteraction++;

                                if (DrugInteraction.Equals(1))
                                {
                                    sbOutput.Append(Environment.NewLine + "Drug Interaction" + Environment.NewLine);
                                }

                                sbOutput.Append(DrugInteraction.ToString() + ". " + line.Trim());
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

                            if (strTemp.Trim().Length > 0)
                            {
                                Allergy++;

                                if (Allergy.Equals(1))
                                {
                                    sbOutput.Append(Environment.NewLine + "Allergy" + Environment.NewLine);
                                }

                                sbOutput.Append(Allergy.ToString() + ". " + strTemp.Trim());
                            }
                        }
                        catch
                        {
                        }
                    }

                    else if (line.Contains("[ATC Code:")) //DuplicateTherapy
                    {
                        if (!sbOutput.ToString().Equals(string.Empty))
                        {
                            sbOutput.Append(Environment.NewLine);
                        }

                        //DuplicateTherapy

                        DuplicateTherapy++;

                        if (DuplicateTherapy.Equals(1))
                        {
                            sbOutput.Append(Environment.NewLine + "Duplicate Therapy" + Environment.NewLine);
                        }

                        sbOutput.Append(DuplicateTherapy.ToString() + ". " + line.Trim());
                    }

                    else if (line.ToUpper().Contains("\"TAB-DUPING")) //DuplicateIngredient
                    {
                        try
                        {
                            //StringBuilder sbBatch = new StringBuilder();
                            //DataSet dsBatch = new DataSet();

                            //sbBatch.Append(common.myStr(strXML));
                            //StringReader srBatch = new StringReader(sbBatch.ToString());
                            //dsBatch.ReadXml(srBatch);

                            int startIdx = strFinalOutput.IndexOf("<div id=\"tab-duping\"") + 300;

                            string strTemp = strFinalOutput.Substring(startIdx, strFinalOutput.Length - startIdx);
                            strTemp = strTemp.Substring(0, strTemp.IndexOf("</a>"));

                            startIdx = strTemp.IndexOf("</table>") + 8;
                            strTemp = strTemp.Substring(startIdx, strTemp.Length - startIdx);

                            if (!sbOutput.ToString().Equals(string.Empty))
                            {
                                sbOutput.Append(Environment.NewLine);
                            }

                            if (strTemp.Trim().Length > 0)
                            {
                                DuplicateIngredient++;

                                if (DuplicateIngredient.Equals(1))
                                {
                                    sbOutput.Append(Environment.NewLine + "Duplicate Ingredient" + Environment.NewLine);
                                }

                                sbOutput.Append(DuplicateIngredient.ToString() + ". " + common.clearHTMLTags(strTemp.Trim()).Replace("\r\n", string.Empty).Trim());
                            }
                        }
                        catch (Exception Ex)
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
        return sbOutput.ToString();
    }
    private string getInterfaceBetweenDrug(string strXML)
    {
        StringBuilder sbOutput = new StringBuilder();

        Session["CIMSXMLInputData"] = strXML;
        objCIMS = new clsCIMS();
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
        return sbOutput.ToString();
    }
    protected void btnInteractionContinue_OnClick(object sender, EventArgs e)
    {
        lblIntreactionMessage.Text = string.Empty;

        //if (spnCommentsDrugAllergy.Visible && common.myLen(txtCommentsDrugAllergy.Text).Equals(0))
        //{
        //    lblIntreactionMessage.Text += "Drug Allergy Comments can't be blank ! ";
        //}
        if (spnCommentsDrugToDrug.Visible && common.myLen(txtCommentsDrugToDrug.Text).Equals(0))
        {
            lblIntreactionMessage.Text += "Drug To Drug Interaction Comments can't be blank ! ";
        }
        //if (spnCommentsDrugHealth.Visible && common.myLen(txtCommentsDrugHealth.Text).Equals(0))
        //{
        //    lblIntreactionMessage.Text += "Drug Health Interaction Comments can't be blank ! ";
        //}

        if (common.myLen(lblIntreactionMessage.Text).Equals(0))
        {
            dvInteraction.Visible = false;
        }
    }
    protected void btnInteractionCancel_OnClick(object sender, EventArgs e)
    {
        btnMonographViewOnItemBased.Visible = false;
        btnInteractionViewOnItemBased.Visible = false;

        btnMonographView.Visible = false;
        btnInteractionView.Visible = false;

        hdnGenericId.Value = "0";
        hdnGenericName.Value = string.Empty;
        hdnItemId.Value = "0";
        hdnItemName.Value = string.Empty;
        //ddlIndentType.SelectedValue = "0";
        ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByValue("0"));

        hdnCIMSItemId.Value = string.Empty;
        hdnCIMSType.Value = string.Empty;
        hdnVIDALItemId.Value = "0";

        Removeitem();

        dvInteraction.Visible = false;
    }
    private void setVisiblilityInteraction()
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                lnkDrugAllergy.Visible = false;
                //mmb
                string strXMLAll = getAllInterationXML(string.Empty);//DrugToDrug
                //string strXMLDD = getInterationXML(string.Empty);//DrugToDrug
                //string strXMLDH = getHealthOrAllergiesInterationXML("H", string.Empty);//Helth
                //string strXMLDA = getHealthOrAllergiesInterationXML("A", string.Empty);//Allergies

                string outputValuesAll = string.Empty;
                //string outputValuesDD = string.Empty;
                //string outputValuesDH = string.Empty;
                //string outputValuesDA = string.Empty;

                //if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                if (common.myLen(strXMLAll) > 0)
                {
                    outputValuesAll = objCIMS.getFastTrack5Output(strXMLAll);

                    //outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                    //outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                    //outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);

                    foreach (GridViewRow dataItem in gvPrevious.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");

                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            //lnkBtnDHInteractionCIMS.Visible = false;
                            //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                            //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            //lnkBtnDAInteractionCIMS.Visible = false;
                            //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                            if (outputValuesAll != null)
                            {
                                //mmb
                                if (objCIMS.IsDrugToAllInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesAll, false) > 0) //false
                                {
                                    if (outputValuesAll.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            //if (outputValuesDD != null)
                            //{
                            //    if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                            //    {
                            //        if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

                            //if (outputValuesDH != null)
                            //{
                            //    if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                            //    {
                            //        if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnDHInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

                            //if (outputValuesDA != null)
                            //{
                            //    if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDA) > 0)
                            //    {
                            //        if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnDAInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

                        }
                    }

                    foreach (GridViewRow dataItem in gvItem.Rows)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");

                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            //lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            //LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            //lnkBtnDHInteractionCIMS.Visible = false;
                            //lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                            //LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            //lnkBtnDAInteractionCIMS.Visible = false;
                            //lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                            if (outputValuesAll != null)
                            {
                                if (objCIMS.IsDrugToAllInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesAll, false) > 0)// false
                                {
                                    if (outputValuesAll.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            //if (outputValuesDD != null)
                            //{
                            //    if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                            //    {
                            //        if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

                            //if (outputValuesDH != null)
                            //{
                            //    if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                            //    {
                            //        if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnDHInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

                            //if (outputValuesDA != null)
                            //{
                            //    if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDA) > 0)
                            //    {
                            //        if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                            //        {
                            //            lnkBtnDAInteractionCIMS.Visible = true;
                            //        }
                            //    }
                            //}

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
            //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            //{
            //    VSInteractionService.ArrayOfInt commonNameGroupIds = getVIDALCommonNameGroupIds();
            //    lnkDrugAllergy.Visible = false;

            //    if (commonNameGroupIds.Count > 0)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        Hashtable collVitalItemIdFound = new Hashtable();

            //        sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);

            //        DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
            //        //int? weight = common.myInt(lbl_Weight.Text);//In kilograms 
            //        //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            //        int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            //        int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            //        Hashtable collVitalItemIdFoundDH = new Hashtable();
            //        VSContraIndicationService.ArrayOfInt tempx = new VSContraIndicationService.ArrayOfInt();
            //        for (int i = 0; i < commonNameGroupIds.Count; i++)
            //        {
            //            tempx.Add(commonNameGroupIds[i]);

            //        }
            //        StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(tempx, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
            //                        0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
            //                        (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
            //                        out collVitalItemIdFoundDH);

            //        foreach (GridViewRow dataItem in gvItem.Rows)
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


            //        VSAllergyService.ArrayOfInt allergyIds = null; //new int?[] { 114 };
            //        int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

            //        if (ViewState["PatientAllergyXML"] != string.Empty)
            //        {
            //            allergyIds = (VSAllergyService.ArrayOfInt)ViewState["PatientAllergyXML"];
            //        }
            //        VSAllergyService.ArrayOfInt temp = new VSAllergyService.ArrayOfInt();
            //        for (int i = 0; i < commonNameGroupIds.Count; i++)
            //        {

            //            temp.Add(commonNameGroupIds[i]);
            //        }
            //        sb = objVIDAL.getVIDALDrugAllergyInteraction(temp, allergyIds, moleculeIds);

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
    //private string getInterationXML(string strNewPrescribing)
    //{
    //    string strXML = string.Empty;
    //    try
    //    {
    //        if (common.myBool(Session["IsCIMSInterfaceActive"]))
    //        {
    //            //<Request>
    //            //    <Interaction>
    //            //        <Prescribing>
    //            //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
    //            //        </Prescribing>
    //            //        <Prescribed>
    //            //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
    //            //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
    //            //        </Prescribed>
    //            //        <Allergies />
    //            //        <References/>
    //            //    </Interaction>
    //            //</Request>

    //            string strPrescribing = string.Empty;

    //            StringBuilder ItemIds = new StringBuilder();
    //            foreach (GridViewRow dataItem in gvItem.Rows)
    //            {
    //                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
    //                LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

    //                if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
    //                    && lnkBtnInteractionCIMS.Visible)
    //                {
    //                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
    //                    CIMSType = (CIMSType == string.Empty) ? "Product" : CIMSType;

    //                    if (strNewPrescribing != string.Empty && strPrescribing == string.Empty)
    //                    {
    //                        strPrescribing = strNewPrescribing;
    //                    }

    //                    if (strPrescribing == string.Empty)
    //                    {
    //                        strPrescribing = "<Prescribing><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></Prescribing>";
    //                    }
    //                    else
    //                    {
    //                        ItemIds.Append("<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />");
    //                    }
    //                }
    //            }

    //            if (ItemIds.ToString() == string.Empty)
    //            {
    //                return string.Empty;
    //            }

    //            strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";

    //            strXML = "<Request><Interaction>" + strPrescribing + "<Allergies /><References /></Interaction></Request>";
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }

    //    return strXML;
    //}
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
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
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
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
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
    //private string getHealthOrAllergiesInterationXML(string useFor)
    //{
    //    string strXML = string.Empty;
    //    try
    //    {
    //        //<Request>
    //        //    <Interaction>
    //        //        <Prescribing>
    //        //            <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
    //        //        </Prescribing>
    //        //        <Prescribed>
    //        //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
    //        //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
    //        //        </Prescribed>
    //        //        <Allergies>
    //        //            <Product reference="{8A4E15CD-ACE3-41D9-A367-55658256C2D4}" />
    //        //            <Product reference="{6D8F3E40-FA33-49C9-9D34-7C13F88E00FD}" />
    //        //        </Allergies>
    //        //        <HealthIssueCodes>
    //        //            <HealthIssueCode code="K22" codeType="ICD10" />
    //        //            <HealthIssueCode code="K22.0" codeType="ICD10" />
    //        //        </HealthIssueCodes>
    //        //        <References/>
    //        //    </Interaction>
    //        //</Request>

    //        string strPrescribing = string.Empty;

    //        StringBuilder ItemIds = new StringBuilder();
    //        foreach (GridViewRow dataItem in gvItem.Rows)
    //        {
    //            HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

    //            if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
    //            {
    //                string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
    //                CIMSType = (CIMSType == string.Empty) ? "Product" : CIMSType;

    //                if (strPrescribing == string.Empty)
    //                {
    //                    strPrescribing = "<Prescribing><" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" /></Prescribing>";
    //                }
    //                else
    //                {
    //                    ItemIds.Append("<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />");
    //                }
    //            }
    //        }

    //        if (ItemIds.ToString() == string.Empty)
    //        {
    //            return string.Empty;
    //        }

    //        strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";

    //        switch (useFor)
    //        {
    //            case "H"://Helth Interaction
    //                strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
    //                break;
    //            case "A"://Allergies
    //                strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + "<References /></Interaction></Request>";
    //                break;
    //            case "B"://Both
    //                strXML = "<Request><Interaction>" + strPrescribing + common.myStr(ViewState["PatientAllergyXML"]) + common.myStr(ViewState["PatientDiagnosisXML"]) + "<References /></Interaction></Request>";
    //                break;
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }

    //    return strXML;
    //}
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

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
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

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
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

    private string getAllInterationXML(string strNewPrescribing)
    {
        StringBuilder strXML = new StringBuilder();

        try
        {
            /*<Request>
		        <Interaction>
                    <Prescribing>
                        <Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" />
                    </Prescribing>
			        <Prescribed>
				        <GGPI reference="{488F9F61-5D37-4989-925E-1742FFFDAA9E}"/>
				        <GGPI reference="{2AA7FC83-2E12-496E-98A8-5F0B8D548907}"/>     
			        </Prescribed>
			        <Allergies>
				        <Molecule reference="{5143B2B1-DBED-45A2-8308-61636C3A2F0B}"/>
				        <Molecule reference="{7142A19F-EDE2-4A7C-A30B-06AB60B6B71F}"/>
				        <Molecule reference="{74DCDDAF-9349-430F-A2F2-B7F2BC5CD8AA}"/>
				        <SubstanceClass reference="{8B9ECB52-CDC9-4460-AB40-6E3C7DF38B1E}"/>
				        <SubstanceClass reference="{9E775009-231D-728D-E034-080020E1DD8C}"/>
			        </Allergies>
			        <HealthIssueCodes>
				        <HealthIssueCode code="J45" codeType="ICD10"/>
				        <HealthIssueCode code="N17" codeType="ICD10"/>
				        <HealthIssueCode code="I49.5" codeType="ICD10"/>
			        </HealthIssueCodes>
			        <References/>
			        <DuplicateTherapy checkSameDrug="true" />
			        <DuplicateIngredient checkSameDrug="true" />
		        </Interaction>
		        <PatientProfile>
			        <Gender>F</Gender>
			        <Age><Year>34</Year><Month>8</Month><Day>11</Day></Age>
			        <Pregnancy><Week>20</Week></Pregnancy>
			        <Nursing>true</Nursing>
		        </PatientProfile>
	        </Request>*/

            string strPrescribing = string.Empty;

            StringBuilder ItemIds = new StringBuilder();

            if (!strNewPrescribing.Equals(string.Empty))
            {
                ItemIds.Append(strNewPrescribing);
            }

            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
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

                if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0")
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

            strXML.Append("<Request>");
            strXML.Append("<Interaction>");
            strXML.Append(strPrescribing);
            strXML.Append(common.myStr(ViewState["PatientAllergyXML"]));
            strXML.Append(common.myStr(ViewState["PatientDiagnosisXML"]));
            strXML.Append("<References />");
            strXML.Append("<DuplicateTherapy checkSameDrug=\"true\" />");
            strXML.Append("<DuplicateIngredient checkSameDrug=\"true\" />");
            strXML.Append("</Interaction>");

            //if (common.myInt(Session["Gender"]).Equals(1))
            //{
            //    strXML.Append("<PatientProfile>");
            //    strXML.Append("<Gender>F</Gender>");
            //    if (common.myLen(Session["PatientAgeYear"]) > 0)
            //    {
            //        strXML.Append("<Age><Year>" + common.myStr(Session["PatientAgeYear"]) + "</Year></Age>"); //<Month>1</Month><Day>1</Day>
            //    }
            //    strXML.Append("<Pregnancy><Week>20</Week></Pregnancy>");
            //    strXML.Append("<Nursing>true</Nursing>");
            //    strXML.Append("</PatientProfile>");
            //}

            strXML.Append("</Request>");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return strXML.ToString();
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
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
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
                    ImageButton lnkBtnInteractionCIMS = (ImageButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                    if ((common.myStr(CIMSItemId.Value).Trim().Length > 0 && common.myStr(CIMSItemId.Value).Trim() != "0"))
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

        hdnCIMSOutput.Value = objCIMS.getCIMSFinalOutupt(IsBrandDetails);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "OpenCIMSWindow();", true);
        return;

        //RadWindow1.NavigateUrl = "/EMR/Medication/Monograph1.aspx?IsBD=" + IsBrandDetails;
        //RadWindow1.Height = 600;
        //RadWindow1.Width = 900;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        ////RadWindow1.OnClientClose = "";
        //RadWindow1.VisibleOnPageLoad = true;
        //RadWindow1.Modal = true;
        //RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        //RadWindow1.VisibleStatusbar = false;
    }
    ///////////////VIDAL/////////////////////////////
    /*
    private VSInteractionService.ArrayOfInt getVIDALCommonNameGroupIds()
    {
        VSInteractionService.ArrayOfInt commonNameGroupIds = new VSInteractionService.ArrayOfInt();
        //commonNameGroupIds.Add(0);
        try
        {
            List<VSInteractionService.ArrayOfInt> list = new List<VSInteractionService.ArrayOfInt>();

            foreach (GridViewRow dataItem in gvItem.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");

                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    commonNameGroupIds.Add(common.myInt(VIDALItemId.Value));
                }
            }

            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                HiddenField VIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
                LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");

                if (common.myInt(VIDALItemId.Value) > 0)
                //&& (lnkBtnInteractionVIDAL.Visible || lnkBtnDHInteractionVIDAL.Visible))
                {
                    commonNameGroupIds.Add(common.myInt(VIDALItemId.Value));
                }
            }

            //commonNameGroupIds = list.ToArray();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return commonNameGroupIds;
    }
    */

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
        try
        {
            ////commonNameGroupIds = new int?[] { 15223, 15070, 1524, 4025, 4212, 516 };
            //VSInteractionService.ArrayOfInt temp = null;
            //for (int i = 0; i < commonNameGroupIds.Length; i++)
            //{

            //    temp.Add(commonNameGroupIds[i]);
            //}
            //StringBuilder sb = new StringBuilder();

            //Hashtable collVitalItemIdFound = new Hashtable();

            //sb = objVIDAL.getVIDALDrugToDrugInteraction(false, temp, out collVitalItemIdFound);

            //if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            //{
            //    Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            //}

            //if (sb.ToString() != string.Empty)
            //{
            //    Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            //    openWindowsVIDAL("?UseFor=IN");
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
        }
    }
    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            ////int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };

            //DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            ////weight = common.myInt(lbl_Weight.Text);//In kilograms
            ////'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            //int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            //int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            //Hashtable collVitalItemIdFoundDH = new Hashtable();
            //VSContraIndicationService.ArrayOfInt contrainx = null;
            //for (int i = 0; i < commonNameGroupIds.Length; i++)
            //{

            //    contrainx.Add(commonNameGroupIds[i]);
            //}
            //StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(contrainx, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
            //        0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
            //        (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
            //        out collVitalItemIdFoundDH);

            //if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            //{
            //    Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            //}

            //if (sb.ToString() != string.Empty)
            //{
            //    Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            //    openWindowsVIDAL("?UseFor=HI");
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
        }
    }
    private void getDrugAllergyVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            ////commonNameGroupIds = new int?[] { 4025, 4212, 516 };

            //VSAllergyService.ArrayOfInt allergyIds = null; //new int?[] { 114 };
            //int?[] moleculeIds = null;  //new int?[] { 202, 96, 36 };

            //if (ViewState["PatientAllergyXML"] != string.Empty)
            //{
            //    allergyIds = (VSAllergyService.ArrayOfInt)ViewState["PatientAllergyXML"];
            //}

            //StringBuilder sb = new StringBuilder();
            //VSAllergyService.ArrayOfInt allergyx = null;
            //for (int i = 0; i < commonNameGroupIds.Length; i++)
            //{

            //    allergyx.Add(commonNameGroupIds[i]);
            //}
            //sb = objVIDAL.getVIDALDrugAllergyInteraction(allergyx, allergyIds, moleculeIds);

            //if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            //{
            //    Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            //}

            //if (sb.ToString() != string.Empty)
            //{
            //    Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            //    openWindowsVIDAL("?UseFor=DA");
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

            if (sb.ToString() != string.Empty)
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
    /*
    private void getSideEffectVidal(VSSideEffectService.ArrayOfInt commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //StringBuilder sb = new StringBuilder();

            //sb = objVIDAL.getVIDALDrugSideEffect(commonNameGroupIds);

            //if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            //{
            //    Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            //}

            //if (sb.ToString() != string.Empty)
            //{
            //    Cache.Insert("VIDALXML" + common.myStr(Session["UserId"]), sb.ToString(), null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration);

            //    openWindowsVIDAL("?UseFor=SE");
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
        }
    }
    */
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
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
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
                gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = chkShowDetails.Checked;
                gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = chkShowDetails.Checked;

                //gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = chkShowDetails.Checked;                
                //gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = chkShowDetails.Checked;
                //gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = chkShowDetails.Checked;

                gvPrevious.Columns[(byte)enumCurrent.MonographCIMS].Visible = chkShowDetails.Checked;
                gvPrevious.Columns[(byte)enumCurrent.InteractionCIMS].Visible = chkShowDetails.Checked;

                //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsCIMS].Visible = chkShowDetails.Checked;                
                //gvPrevious.Columns[(byte)enumCurrent.DHInteractionCIMS].Visible = chkShowDetails.Checked;
                //gvPrevious.Columns[(byte)enumCurrent.DAInteractionCIMS].Visible = chkShowDetails.Checked;
            }
            //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
            //{
            //    gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = chkShowDetails.Checked;
            //    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = chkShowDetails.Checked;
            //    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = chkShowDetails.Checked;
            //    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = chkShowDetails.Checked;
            //    gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = chkShowDetails.Checked;

            //    gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = chkShowDetails.Checked;
            //    gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = chkShowDetails.Checked;
            //    gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = chkShowDetails.Checked;
            //    gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = chkShowDetails.Checked;
            //    gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = chkShowDetails.Checked;
            //}

            //setGridColor();

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

        if (DoctorId == 0)
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
                        txtDose.Text = common.myDbl(DR["Dose"]).ToString();
                    }
                    if (common.myInt(DR["UnitId"]) > 0)
                    {
                        ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(DR["UnitId"]).ToString()));
                    }

                    //if (common.myInt(DR["StrengthId"]) > 0)
                    //{
                    //    ddlStrength.SelectedIndex = ddlStrength.Items.IndexOf(ddlStrength.Items.FindItemByValue(common.myInt(DR["StrengthId"]).ToString()));
                    //}

                    txtStrengthValue.Text = common.myStr(DR["StrengthValue"]);

                    if (common.myInt(DR["FormulationId"]) > 0)
                    {
                        ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(DR["FormulationId"]).ToString()));
                        ddlFormulation_OnSelectedIndexChanged(this, null);
                    }
                    if (common.myInt(DR["RouteId"]) > 0)
                    {
                        ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(DR["RouteId"]).ToString()));
                    }
                    if (common.myInt(DR["FrequencyId"]) > 0)
                    {
                        ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(DR["FrequencyId"]).ToString()));
                    }
                    if (common.myInt(DR["Duration"]) > 0)
                    {
                        txtDuration.Text = common.myInt(DR["Duration"]).ToString();
                    }
                    if (!common.myStr(DR["DurationType"]).Equals(string.Empty))
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(DR["DurationType"]).ToString()));
                        endDateChange();
                    }
                    if (common.myInt(DR["FoodRelationshipId"]) > 0)
                    {
                        ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(DR["FoodRelationshipId"]).ToString()));
                    }

                    //if (common.myInt(txtDose.Text).Equals(0))
                    //{
                    //    //txtDose.Text = "1";
                    //}

                    if (common.myStr(Session["OPIP"]) == "I")
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            //txtDuration.Text = "1";
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
        addItem(common.myStr(ViewState["ICDCodes"]));
        dvConfirmAlreadyExistOptions.Visible = false;
    }
    protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmAlreadyExistOptions.Visible = false;
    }
    protected void txtICDCode_TextChanged(object sender, EventArgs e)
    {
        ViewState["ICDCodes"] = null;
        if (!common.myStr(txtICDCode.Text).Equals(""))
            ViewState["ICDCodes"] = common.myStr(txtICDCode.Text);
    }
    protected void ddlPrescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myInt(ddlPrescription.SelectedValue) > 0)
            {
                //btnCancel.Visible = true;
                btnPrint.Visible = true;
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
            hdnGenericName.Value = "";
            hdnItemId.Value = "0";
            hdnItemName.Value = "";

            ddlGeneric.Text = "";
            ddlBrand.Text = "";
            txtCustomMedication.Text = "";

            hdnCIMSItemId.Value = "";
            hdnCIMSType.Value = "";
            hdnVIDALItemId.Value = "";

            //ddlIndentType.SelectedValue = "0";
            ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByValue("0"));

            //chkNoMedicine.Checked = false;

            clearItemDetails();

            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            ds = objEMR.getMedicinesOPList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["EncounterId"]), common.myInt(ddlPrescription.SelectedValue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];

                ddlIndentType.SelectedIndex = ddlIndentType.Items.IndexOf(ddlIndentType.Items.FindItemByValue(common.myStr(DR["IndentType"])));
                ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myStr(DR["AdvisingDoctorId"])));

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
    }
    protected void btnPrintYes_OnClick(object sender, EventArgs e)
    {
        int PrescriptionId = common.myInt(Session["CurentSavedPrescriptionId"]);

        if (PrescriptionId == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Prescription not Selected !";
            dvConfirmPrint.Visible = false;
            return;
        }

        RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(Session["EncounterId"]) + "&PId=" + PrescriptionId;
        RadWindow1.Height = 650;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.Modal = true;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

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
    protected void btnGenericHelpClose_Click(object sender, EventArgs e)
    {
    }
    private string PostePrescription(int PrescriptionID, string XMLflag, out string refno)
    {
        var outerrormessage = "";
        refno = "0";
        try
        {
            if (PatientandClinicianValidateion() != "")
            {
                dverx.Visible = true;
                refno = "0";
                return "Failed";
            }
            if (PatientandMedicineValidateion(PrescriptionID) != "")
            {

                refno = "0";
                return "DDC Code/ Emirates Id Missing";
            }
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UspGenerateeOPRxXml";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prm = new SqlParameter("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
            cmd.Parameters.Add(prm);

            SqlParameter prm1 = new SqlParameter("@iFacilityId", common.myInt(Session["FacilityId"]).ToString());
            cmd.Parameters.Add(prm1);

            SqlParameter prm2 = new SqlParameter("@PrescriptionID", PrescriptionID);
            cmd.Parameters.Add(prm2);

            SqlParameter prm3 = new SqlParameter("@DispositionFlag", XMLflag);
            cmd.Parameters.Add(prm3);

            SqlParameter prm4 = new SqlParameter("@returnXML", "");
            prm4.Direction = ParameterDirection.Output;
            prm4.DbType = DbType.Xml;

            cmd.Parameters.Add(prm4);

            cmd.Connection = con;

            Hashtable hsin = new Hashtable();
            Hashtable hsout = new Hashtable();
            hsin.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
            hsin.Add("@iFacilityId", common.myInt(Session["FacilityID"]).ToString());
            hsin.Add("@PrescriptionID", common.myInt(PrescriptionID));
            hsin.Add("@DispositionFlag", XMLflag);

            hsout.Add("@returnXML", "");

            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet("ds");
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            string filename = PrescriptionID.ToString() + "_" + DateTime.Now.ToString("ddMMyyHHmm") + ".xml";
            string fileLoc = Server.MapPath("~/PatientDocuments/" + filename);
            // var fileData=File.reof
            FileStream fs = null;
            if (File.Exists(fileLoc))
            {
                File.Delete(fileLoc);
            }

            try
            {

                string strxmltxt = common.myStr(cmd.Parameters["@returnXML"].Value);
                //  Alert.ShowAjaxMsg(strxmltxt, this);
                var xdoc = new XmlDocument();
                xdoc.LoadXml(strxmltxt);
                xdoc.Save(fileLoc);
            }
            catch (Exception ex)
            {
                lblERXNo.Text = ex.Message;
                Alert.ShowAjaxMsg(ex.Message, this);
            }
            //Alert.ShowAjaxMsg(fileLoc, this);
            con.Close();

            decimal outputrefno;
            string outputfile = "";

            byte[] outfile = null;


            //if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "useWebserviceForEclaim", sConString).Equals("Y"))
            //{
            //    string uname = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "eclaimWebServiceLoginID", sConString);
            //    string password = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "eclaimWebServicePassword", sConString);
            //    System.Net.ServicePointManager.CertificatePolicy = new ClsPolicy();
            //    AFG_Request_ValidateTransaction.WebServiceProvider v = new AFG_Request_ValidateTransaction.WebServiceProvider();
            //    AFG_Request_ValidateTransaction.UploadERxRequest Request = new AFG_Request_ValidateTransaction.UploadERxRequest();
            //    AFG_Request_ValidateTransaction.UploadERxRequestResponse response = new AFG_Request_ValidateTransaction.UploadERxRequestResponse();
            //    string[] DellBoomiuserpwd = DellBoomi.Split('!');

            //    v.Credentials = new NetworkCredential(DellBoomiuserpwd[0], DellBoomiuserpwd[1]);

            //    // NewErx.eRxValidateTransactionSoapClient vt = new NewErx.eRxValidateTransactionSoapClient();
            //    //int postrncount = vt.UploadERxRequest(uname, password, "DHA-P-0242455", "DHA-P-0242455", common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
            //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //    DataSet dsdoctor = dl.FillDataSet(CommandType.Text, "Exec uspGetClinicianLoginforErx " + common.myInt(Session["UserID"]).ToString());
            //    if (dsdoctor.Tables[0].Rows.Count > 0)
            //    {

            //        string cluname = common.myStr(dsdoctor.Tables[0].Rows[0]["UserName"]);
            //        string clpwd = common.myStr(dsdoctor.Tables[0].Rows[0]["Password"]);
            //        v.UseDefaultCredentials = true;
            //        v.Credentials = new NetworkCredential(DellBoomiuserpwd[0], DellBoomiuserpwd[1]);


            //        Request.facilityLogin = uname;
            //        Request.facilityPwd = password;
            //        Request.clinicianLogin = cluname;// "eRxClinTest02";
            //        Request.clinicianPwd = clpwd;// "pwderxclintest02";
            //        Request.fileContent = Convert.ToBase64String(common.FileToByteArray(fileLoc), 0, common.FileToByteArray(fileLoc).Length);
            //        Request.fileName = filename;
            //        response = v.AFG_UploadERxRequest(Request);

            //        outerrormessage = response.errorMessage;
            //        outputrefno = response.eRxReferenceNo;
            //        outputfile = response.errorReport;

            //        //   int trncount = vt.UploadERxRequest("Aster002", "insurance002", cluname, clpwd, common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
            //        refno = outputrefno.ToString();
            //        if (outputfile != "" && outputfile != "null" && outputfile != null)
            //        {

            //            outfile = Convert.FromBase64String(outputfile);
            //        }
            //    }
            //    else
            //    {

            //        Alert.ShowAjaxMsg("No Doctor Login Found", this);
            //        lblERXNo.Text = "No Doctor Login Found";
            //        refno = "0";
            //        return "No Doctor Login Found";
            //    }


            //    //if (returnfilename == true)
            //    //{
            //    if (outfile != null)
            //    {
            //        string erfilename = Server.MapPath("~/PatientDocuments/Error.Csv");
            //        if (File.Exists(erfilename))
            //        {
            //            File.Delete(erfilename);
            //        }

            //        Response.BinaryWrite(outfile);


            //        String err = System.Text.Encoding.UTF8.GetString(outfile);

            //        err = err.Replace("'", string.Empty);

            //        outerrormessage = outerrormessage.Replace("'", string.Empty);

            //        BaseC.clsErx objerx = new BaseC.clsErx(sConString);
            //        objerx.SaveDhaError(common.myInt(PrescriptionID), err, "PrescripeMedication", "PostPrescription", "UploadTransaction", common.myInt(Session["FacilityID"]),
            //            common.myInt(Session["ModuleId"]), outerrormessage);
            //        //dl.ExecuteNonQuery(CommandType.Text, "Exec  UspSaveDhaError " + PrescriptionID.ToString() + ",'" + err + "','PrescripeMedication','PostPrescription','UploadERxRequest'," + common.myInt(Session["FacilityID"]).ToString() + ",'" + outerrormessage + "'");

            //        common.ByteArrayToFile(erfilename, outfile, out erfilename);
            //    }
            //}
            //else
            //{
            //    Alert.ShowAjaxMsg("E claim is not running. Please contact Admin", Page);
            //    return "";
            //}
        }
        catch (Exception ex)
        {
            lblERXNo.Text = ex.Message;
            outerrormessage = ex.Message;
        }
        return outerrormessage;
        //}
    }
    protected void gvOrderSet_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName == "SelectOrderSet")
            {
                //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int ItemId = common.myInt(((HiddenField)row.FindControl("hdnItemId")).Value);

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
            emr = null;
        }
    }
    protected void btnCopyLastPrescription_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        try
        {
            ViewState["Stop"] = true;

            //if (common.myStr(ViewState["PatientOPIPType"]) == "I")
            //{
            //    ds = objEMR.getCopyPreviousMedicinesIP(common.myInt(Session["HospitalLocationID"]),
            //                        common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
            //                        common.myInt(Session["EncounterId"]));
            //}
            //else
            //{
            ds = objEMR.getCopyPreviousMedicinesOP(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));
            //}

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record not found!";

                ds.AcceptChanges();

                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                ViewState["Item"] = null;
                ViewState["ItemDetail"] = null;

                BindBlankItemGrid();
            }
            else
            {
                try
                {
                    DataTable dtData1 = ds.Tables[0];
                    dtData1 = addColumnInItemGrid(dtData1);
                    ViewState["Item"] = dtData1;

                    DataTable dtData2 = ds.Tables[1];
                    dtData2 = addColumnInItemGrid(dtData2);
                    ViewState["ItemDetail"] = dtData2;

                    ViewState["CopyLastPresc"] = true;

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
            objEMR = null;
        }
    }
    protected void bindUnApprovedPrescriptions()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

        try
        {
            ds = objEMR.getUnApprovedPrescriptions(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                common.myInt(ViewState["EncId"]), common.myInt(Session["UserId"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //int ItemId = 0;
                    ////santosh
                    //DataView dv = new DataView(ds.Tables[0]);
                    //foreach (DataRow DR in ds.Tables[0].Rows)
                    //{
                    //    ItemId = common.myInt(DR["ItemId"]);
                    //    dv.RowFilter = "ItemId=" + ItemId;
                    //    //DR["PrescriptionDetail"] = objEMR.GetPrescriptionDetailStringV2(dv.ToTable());
                    //    ds.Tables[0].AcceptChanges();
                    //}

                    ViewState["Item"] = ds.Tables[0];
                    ViewState["ItemDetail"] = ds.Tables[0];

                    gvItem.DataSource = ds.Tables[0];
                    gvItem.DataBind();
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
            ds.Dispose();
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
        }
        catch
        {
        }

        return dtData;
    }
    private int saveAsUnApprovedPrescriptions(DataRow DRData)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        string ReturnUnAppPrescriptionId = common.myInt(ViewState["UnAppPrescriptionId"]).ToString();

        try
        {
            if (DRData != null)
            {
                if (common.myInt(DRData["RouteId"]) > 0)
                {
                    string PrescriptionDetail = common.myStr(DRData["PrescriptionDetail"]);

                    if (common.myLen(PrescriptionDetail).Equals(0))
                    {
                        DataTable dtData = DRData.Table.Clone();
                        dtData.Rows.Add(DRData.ItemArray);

                        PrescriptionDetail = objEMR.GetPrescriptionDetailStringV2(dtData);
                    }

                    string strMsg = objEMR.SaveUnApprovedPrescriptions(common.myInt(ViewState["UnAppPrescriptionId"]), common.myInt(Session["HospitalLocationId"]),
                                        common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncId"]), common.myInt(DRData["IndentId"]),
                                        common.myStr(DRData["IndentNo"]).Equals("0") ? string.Empty : common.myStr(DRData["IndentNo"]),
                                        common.myStr(DRData["IndentDate"]) != string.Empty ? common.myDate(DRData["IndentDate"]).ToString("yyyy-MM-dd") : null,
                                        common.myInt(DRData["IndentTypeId"]),
                                        common.myStr(DRData["IndentType"]), common.myInt(DRData["GenericId"]), common.myStr(DRData["GenericName"]),
                                        common.myInt(DRData["ItemId"]), common.myStr(DRData["ItemName"]), common.myBool(DRData["CustomMedication"]),
                                        common.myInt(DRData["FrequencyId"]), common.myStr(DRData["FrequencyName"]), common.myDbl(DRData["Frequency"]),
                                        common.myDbl(DRData["Dose"]), common.myStr(DRData["Duration"]), common.myStr(DRData["DurationText"]),
                                        common.myStr(DRData["Instructions"]), common.myInt(DRData["UnitId"]), common.myStr(DRData["UnitName"]),
                                        common.myStr(DRData["Type"]),
                                        common.myStr(DRData["StartDate"]) != string.Empty ? common.myDate(DRData["StartDate"]).ToString("yyyy-MM-dd") : null,
                                        common.myStr(DRData["EndDate"]) != string.Empty ? common.myDate(DRData["EndDate"]).ToString("yyyy-MM-dd") : null,
                                        common.myStr(DRData["CIMSItemId"]), common.myStr(DRData["CIMSType"]), common.myInt(DRData["VIDALItemId"]),
                                        common.myStr(DRData["XMLData"]), PrescriptionDetail, common.myInt(DRData["FormulationId"]),
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
                                        ref ReturnUnAppPrescriptionId);

                    if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
                    {
                        ViewState["UnAppPrescriptionId"] = null;
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
        }

        return common.myInt(ReturnUnAppPrescriptionId);
    }
    protected void btnInsulingSliding_Click(object sender, EventArgs e)
    {

    }
    protected void lnkInsulinDose_Click(object sender, EventArgs e)
    {
        Session["Trigger"] = "INSULIN";
        RadWindow1.NavigateUrl = "/EMR/Medication/InsulinDose.aspx?&ItemId=" + common.myInt(hdnItemId.Value) +
                              "&ItemName=" + common.myStr(ddlBrand.Text).Replace(":", "-");// +"&FrequencyData=" + hdnXmlFrequencyTime.Value;
        RadWindow1.Height = 500;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientCloseInsulingTime";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }


    protected void lnkTappereddose_Click(object sender, EventArgs e)
    {
        if (common.myInt(ddlBrand.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please Select An Item", this);
            return;
        }
        if (common.myInt(txtDuration.Text) == 0)
        {
            Alert.ShowAjaxMsg("Please Select Duration", this);
            return;
        }
        if (common.myInt(txtDuration.Text) == 1)
        {
            Alert.ShowAjaxMsg("At least 2 Days Duration is needed to Tapered Dose", this);
            return;
        }
        Session["Trigger"] = "Tappereddose";
        RadWindow1.NavigateUrl = "/EMR/Medication/Tappereddose.aspx?dt=" + txtStartDate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&Duration=" + txtDuration.Text + "&ItemId=" + common.myInt(hdnItemId.Value) +
                              "&ItemName=" + common.myStr(ddlBrand.Text).Replace(":", "-");// +"&FrequencyData=" + hdnXmlFrequencyTime.Value;
        RadWindow1.Height = 500;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientCloseInsulingTime";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void AddAIMedicationList()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        try
        {
            string Type = string.Empty;

            DataTable tblItem = new DataTable();
            tblItem = (DataTable)ViewState["ItemDetail"];//"GridData"
            if (tblItem == null)
            {
                tblItem = CreateItemTable();
                ViewState["ItemDetail"] = tblItem;
            }

            DataView dvRemoveBlank = tblItem.Copy().DefaultView;
            //dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR ISNULL(CustomMedication,0)<>0";
            dvRemoveBlank.RowFilter = "ISNULL(GenericId,0)>0 OR ISNULL(ItemId,0)>0 OR CustomMedication<>1";
            tblItem = dvRemoveBlank.ToTable();
            ViewState["ItemDetail"] = tblItem;
            DataTable dt1 = tblItem;
            DataView DVItem = tblItem.Copy().DefaultView;
            string customMedication = common.myStr(txtCustomMedication.Text).Trim();

            if (customMedication.Length > 0)
            {
                DVItem.RowFilter = "CustomMedication=" + (chkCustomMedication.Checked ? "1" : "0");
            }
            else
            {
                if (common.myInt(hdnItemId.Value) > 0)
                {
                    DVItem.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value);
                }
                else if (common.myInt(hdnGenericId.Value) > 0)
                {
                    DVItem.RowFilter = "GenericId=" + common.myInt(hdnGenericId.Value);
                }
            }

            bool IsAdded = false;

            if (DVItem.ToTable().Rows.Count == 0)
            {
                string strIndentId = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (strIndentId == string.Empty)
                        strIndentId = strIndentId + dr["ItemId"];
                    else
                        strIndentId = strIndentId + "," + dr["ItemId"];
                }

                //BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
                //DataSet dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                DataSet dsDetail = (DataSet)Session["AIMedicationList"];
                DataView dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;

                        if (strIndentId != string.Empty)
                        {
                            dv.RowFilter = "ItemId NOT IN(" + strIndentId + ")";
                        }
                    }
                }
                if (dv.Count > 0)
                {
                    DataTable dt = dv.ToTable();
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
                        DR1["ICDCode"] = txtICDCode.Text;
                        DR1["StrengthValue"] = string.Empty;
                        DR1["ReferanceItemId"] = 0;
                        DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR1["NotToPharmacy"] = 0;
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
                        DR1["ReferanceItemName"] = DBNull.Value;
                        DR1["Instructions"] = string.Empty;
                        DR1["DoseTypeName"] = DBNull.Value;
                        DR1["Volume"] = string.Empty;
                        DR1["InfusionTime"] = string.Empty;
                        DR1["TimeUnit"] = string.Empty;
                        DR1["TotalVolume"] = string.Empty;
                        DR1["FlowRate"] = "0";

                        DR1["GenericId"] = DBNull.Value;
                        DR1["GenericName"] = string.Empty;
                        DR1["ItemId"] = common.myInt(drM["itemid"]);
                        DR1["ItemName"] = common.myStr(drM["itemName"]);
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
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

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
                        DR["ICDCode"] = txtICDCode.Text;
                        DR["StrengthValue"] = string.Empty;
                        DR["ReferanceItemId"] = 0;
                        DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                        DR["NotToPharmacy"] = 0;
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
                        DR["DetailsId"] = 0;

                        DR["FoodRelationship"] = DBNull.Value;
                        DR["ReferanceItemName"] = DBNull.Value;
                        DR["Instructions"] = string.Empty;
                        DR["DoseTypeName"] = DBNull.Value;
                        DR["Volume"] = string.Empty;
                        DR["InfusionTime"] = string.Empty;
                        DR["TimeUnit"] = string.Empty;
                        DR["TotalVolume"] = string.Empty;
                        DR["FlowRate"] = "0";

                        DR["GenericId"] = DBNull.Value;
                        DR["GenericName"] = string.Empty;
                        DR["ItemId"] = common.myInt(drM["itemid"]);
                        DR["ItemName"] = common.myStr(drM["itemName"]);
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
                        DataTable dtPD = dt1.Copy().Clone();
                        dtPD.Rows.Add(DR1.ItemArray);


                        DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]);// emr.GetPrescriptionDetailStringV2(dtPD);
                                                                                // DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dtPD);// common.myStr(drM["Remarks"]);
                                                                                //common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringV2(dt1);
                        tblItem.Rows.Add(DR);
                    }
                    tblItem.AcceptChanges();
                    DataView dvRemoveSameColumn = tblItem.Copy().DefaultView;
                    dvRemoveSameColumn.RowFilter = "XMLData <> ''";
                    tblItem = dvRemoveSameColumn.ToTable();
                    ViewState["ItemDetail"] = tblItem.Copy();
                    ViewState["Item"] = tblItem.Copy();
                }
            }
            else
            {
                string strIndentId = string.Empty;
                foreach (DataRow dr in DVItem.Table.Rows)
                {
                    if (strIndentId == string.Empty)
                        strIndentId = strIndentId + dr["ItemId"];
                    else
                        strIndentId = strIndentId + "," + dr["ItemId"];
                }

                //BaseC.EMRMasters objMst = new BaseC.EMRMasters(sConString);
                //DataSet dsDetail = objMst.GetEMRDrugSetDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["OrderSetId"]), 0);
                DataSet dsDetail = (DataSet)Session["AIMedicationList"];
                DataView dv = new DataView();
                dv.RowFilter = string.Empty;
                if (dsDetail.Tables.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows.Count > 0)
                    {
                        dv = dsDetail.Tables[0].DefaultView;

                        if (strIndentId != string.Empty)
                        {
                            dv.RowFilter = "ItemId NOT IN(" + strIndentId + ")";
                        }
                    }
                }
                if (dv.Count > 0)
                {
                    string[] TobeDistinct = { };
                    DataTable dt = dv.ToTable();
                    foreach (DataRow drM in dt.Rows)
                    {
                        bool IsExists = false;
                        DataTable dtt = new DataTable();
                        dtt = (DataTable)ViewState["Item"];//"GridData"
                        if (dtt != null)
                        {
                            DataView dvv = dtt.Copy().DefaultView;
                            dvv.RowFilter = "ISNULL(ItemId,0) = " + common.myStr(drM["itemid"]);
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
                            DR1["ICDCode"] = txtICDCode.Text;
                            DR1["StrengthValue"] = string.Empty;
                            DR1["ReferanceItemId"] = 0;
                            DR1["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR1["NotToPharmacy"] = 0;
                            DR1["IsInfusion"] = 0;
                            DR1["IsInjection"] = 0;
                            DR1["RouteName"] = common.myStr(drM["RouteName"]);
                            DR1["Duration"] = common.myInt(drM["Duration"]);
                            DR1["Type"] = common.myStr(drM["DtypeId"]);//
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
                            DR1["GenericId"] = DBNull.Value;
                            DR1["GenericName"] = string.Empty;
                            DR1["ItemId"] = common.myInt(drM["itemid"]);
                            DR1["ItemName"] = common.myStr(drM["itemName"]);
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
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

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
                            DR["ICDCode"] = txtICDCode.Text;
                            DR["StrengthValue"] = string.Empty;
                            DR["ReferanceItemId"] = 0;
                            DR["UnitId"] = common.myInt(drM["DoseUnitId"]);
                            DR["NotToPharmacy"] = 0;
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

                            DR["GenericId"] = DBNull.Value;
                            DR["GenericName"] = string.Empty;
                            DR["ItemId"] = common.myInt(drM["itemid"]);
                            DR["ItemName"] = common.myStr(drM["itemName"]);
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
                            DataTable dtPD = dt1.Copy().Clone();
                            dtPD.Rows.Add(DR1.ItemArray);


                            // DR["PrescriptionDetail"] = emr.GetPrescriptionDetailStringV2(dtPD);

                            DR["PrescriptionDetail"] = common.myStr(drM["Remarks"]); //emr.GetPrescriptionDetailStringV2(dt1);
                            tblItem.Rows.Add(DR);
                        }

                        tblItem.AcceptChanges();
                        DataView dvRemoveSameColumn = tblItem.Copy().DefaultView;
                        //dvRemoveSameColumn.RowFilter = "((ISNULL(XMLData,'') <> '' and Id = 0) OR (ISNULL(XMLData,'') = '' AND ID is null))";
                        dvRemoveSameColumn.RowFilter = "((ISNULL(XMLData,'') <> '' and GenericId is null) OR (ISNULL(XMLData,'') = '' AND GenericId >=0))";
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

            gvItem.DataSource = tblItem.Copy();
            gvItem.DataBind();

            setVisiblilityInteraction();

            ddlFormulation.Enabled = true;
            ddlRoute.Enabled = true;
            //ddlStrength.Enabled = true;

            ddlBrand.Focus();
            ViewState["OrderSetId"] = "0";
            ////btnCopyLastPrescription.Enabled = false;

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

    }

    protected void btnAddAIMedicineList_Click(object sender, EventArgs e)
    {
        if (Session["AIMedicationList"] != null)
        {
            AddAIMedicationList();
            Session["AIMedicationList"] = null;
        }
    }

    protected void cbEnableSuggession_CheckedChanged(object sender, EventArgs e)
    {
        Cache.Insert("EnableMSuggession_" + common.myInt(Session["EncounterId"]), (sender as CheckBox).Checked, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
    }

    protected void lbtnShowSuggession_Click(object sender, EventArgs e)
    {
        ShowAIMedicineWindow();
    }

    protected void imgSaveInstuctions_Click(object sender, ImageClickEventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable tbl = new DataTable();
        try
        {
            int DoctorId = common.myInt(Request.QueryString["DoctorId"]);
            if (DoctorId == 0)
            {
                DoctorId = common.myInt(Session["LoginDoctorId"]);
            }
            if (DoctorId.Equals(0) && ddlAdvisingDoctor.SelectedItem != null && common.myInt(ddlAdvisingDoctor.SelectedItem.Value) > 0)
            {
                DoctorId = common.myInt(ddlAdvisingDoctor.SelectedItem.Value);
            }
            if (DoctorId.Equals(0))
            {
                DoctorId = common.myInt(Session["EmployeeId"]);
            }
            if (DoctorId.Equals(0))
            {
                lblMessage.Text = "Doctor Name is Required...";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            lblMessage.Text = objEMR.SaveEMRDoctorPrescriptionInstructions(DoctorId, txtInstructions.Text, common.myInt(Session["UserId"]));

            if (lblMessage.Text.ToUpper().Contains("SUCCESSFULLY"))
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            else
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            /*************** Doctor Insructions ***************/

            tbl = objEMR.GetEMRDoctorPrescriptionInstructions(DoctorId).Tables[0];

            ddlInstructions.DataSource = tbl;
            ddlInstructions.DataTextField = "Instructions";
            ddlInstructions.DataValueField = "Id";
            ddlInstructions.DataBind();
            ddlInstructions.Items.Insert(0, new RadComboBoxItem("", ""));


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



    protected void ddlInstructions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtInstructions.Text))
            txtInstructions.Text = ddlInstructions.SelectedItem.Text;
        else
            txtInstructions.Text = txtInstructions.Text + ddlInstructions.SelectedItem.Text;
    }





    protected void lnkClearAll_Click(object sender, EventArgs e)
    {
        try
        {

            #region delete all from EMRUnApprovedPrescriptions
            for (int i = 0; i < gvItem.Rows.Count; i++)
            {
                GridViewRow gv1 = gvItem.Rows[i];
                HiddenField hdnUnAppPrescriptionId = (HiddenField)gv1.FindControl("hdnUnAppPrescriptionId");

                if (common.myInt(hdnUnAppPrescriptionId.Value) > 0)
                {
                    BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                    string strMsg = objEMR.deleteUnApprovedPrescriptions(common.myInt(hdnUnAppPrescriptionId.Value), common.myInt(ViewState["EncId"]),
                                    common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]));
                }

            }
            #endregion
            ViewState["Item"] = null;
            BindBlankItemGrid();
            lblGenericName.Text = string.Empty;
            lblMessage.Text = string.Empty;
            lblERXNo.Text = string.Empty;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }

    protected void btnFavoriteRowCommand_Click(object sender, EventArgs e)
    {
        var index = common.myInt(hdnFavouriteIndex.Value);

        // CommandEventArgs commandArgs =new CommandEventArgs(hdnFavouriteCommand.Value, hdnFavouriteItemId.Value);

        CommandEventArgs commandArgs = commandArgs = new CommandEventArgs(hdnFavouriteCommand.Value, hdnFavouriteItemId.Value); ;

        if (common.myInt(hdnFavouriteItemId.Value).Equals(0))
        {
            commandArgs = new CommandEventArgs(hdnFavouriteCommand.Value, hdnFavouriteGenericId.Value);
        }

        if (hdnCommandType.Value.Equals("Favourite"))
        {
            if (hdnFavouriteCommand.Value == "selectItem")
            {
                BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);

                int ItemId = common.myInt(hdnFavouriteItemId.Value);
                int GenericId = common.myInt(hdnFavouriteGenericId.Value);
                if (ItemId > 0 || GenericId > 0)
                {
                    DataSet ds = new DataSet();
                    ds = objemr.getFavoriteDrugWithStockWithFavoriteId(common.myInt(hdnFavoriteId.Value));

                    chkCustomMedication.Checked = false;
                    hdnCIMSItemId.Value = common.myStr(ds.Tables[0].Rows[0]["CIMSItemId"]);
                    hdnCIMSType.Value = common.myStr(ds.Tables[0].Rows[0]["CIMSType"]);
                    hdnVIDALItemId.Value = common.myStr(ds.Tables[0].Rows[0]["VIDALItemId"]);



                    try
                    {
                        if (!ItemId.Equals(0))
                        {
                            /*if (CheckDDCExpiry(common.myInt(ds.Tables[0].Rows[0]["ItemId"])))
                            {
                                ddlBrand.Text = string.Empty;
                                ddlBrand.SelectedValue = null;
                                lblMessage.Text = "DDC Code Expire For This Item...";
                                return;
                            }
			    */
                            ddlBrand.Text = common.myStr(ds.Tables[0].Rows[0]["ItemName"]);
                            ddlBrand.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["ItemId"]);
                        }
                        else if (!GenericId.Equals(0))
                        {
                            ddlGeneric.Text = common.myStr(ds.Tables[0].Rows[0]["ItemName"]);
                            ddlGeneric.SelectedValue = GenericId.ToString();
                            hdnGenericId.Value = common.myStr(GenericId);
                        }
                    }
                    catch (Exception Ex)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Error: " + Ex.Message;
                        objException.HandleException(Ex);
                    }
                    hdnItemId.Value = ItemId.ToString();



                    if (common.myDbl(ds.Tables[0].Rows[0]["Dose"]) > 0)
                    {
                        txtDose.Text = common.myDbl(common.myStr(ds.Tables[0].Rows[0]["Dose"])).ToString();
                    }
                    else
                    {
                        txtDose.Text = string.Empty;
                    }
                    ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["UnitId"]).ToString()));
                    txtStrengthValue.Text = common.myStr(ds.Tables[0].Rows[0]["StrengthId"]);

                    ddlFormulation.SelectedIndex = ddlFormulation.Items.IndexOf(ddlFormulation.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FormulationId"]).ToString()));

                    ddlFormulation_OnSelectedIndexChanged(this, null);

                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["RouteId"]).ToString()));
                    ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FrequencyId"]).ToString()));
                    txtInstructions.Text = common.myStr(ds.Tables[0].Rows[0]["Instructions"]);
                    if (common.myInt(ds.Tables[0].Rows[0]["Duration"]) > 0)
                    {
                        txtDuration.Text = common.myInt(ds.Tables[0].Rows[0]["Duration"]).ToString();
                    }
                    else
                    {
                        //txtDuration.Text = "1";
                    }

                    if (common.myLen(ds.Tables[0].Rows[0]["DurationType"]) > 0)
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["DurationType"]).ToString()));
                    }
                    else
                    {
                        ddlPeriodType.SelectedIndex = ddlPeriodType.Items.IndexOf(ddlPeriodType.Items.FindItemByValue("D"));
                    }

                    endDateChange();

                    ddlFoodRelation.SelectedIndex = ddlFoodRelation.Items.IndexOf(ddlFoodRelation.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FoodRelationshipId"]).ToString()));

                    if (common.myStr(Session["OPIP"]) == "I")
                    {
                        if (common.myInt(txtDuration.Text).Equals(0))
                        {
                            //txtDuration.Text = "1";
                        }
                    }

                    if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        if (common.myBool(Session["IsCIMSInterfaceActive"]))
                        {
                            if (common.myLen(hdnCIMSItemId.Value) < 2)
                            {
                                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                //lblMessage.Text = "This Drug is not mapped with CIMS.";
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
                                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                            }
                        }
                    }
                }
            }
            else if (hdnFavouriteCommand.Value == "ItemDelete")
            {
                // lblMessage.Text = objemr.DeleteFavoriteDrugsWithFavoriteId(common.myInt(hdnFavoriteId.Value), common.myInt(Session["UserId"]));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteFavoriteDrugsWithFavoriteId";
                APIRootClass.DeleteFavoriteDrugsWithFavoriteId objRoot = new global::APIRootClass.DeleteFavoriteDrugsWithFavoriteId();
                objRoot.FavoriteId = common.myInt(hdnFavoriteId.Value);
                objRoot.EncodedBy = common.myInt(Session["UserId"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                string sResult = JsonConvert.DeserializeObject<string>(sValue);

                lblMessage.Text = sResult;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                GetBrandFavouriteData(string.Empty);
            }
            //if (lstFavourite.Rows.Count > 0)
            //{
            //    GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(lstFavourite.Rows[index], lstFavourite, commandArgs);
            //    lstFavourite_OnRowCommand1(lstFavourite, eventArgs);
            //}
        }
        else if (hdnCommandType.Value.Equals("OrderSet"))
        {
            if (gvOrderSet.Rows.Count > 0)
            {
                GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(gvOrderSet.Rows[index], gvOrderSet, commandArgs);
                gvOrderSet_OnRowCommand(gvOrderSet, eventArgs);
            }

        }
        else if (hdnCommandType.Value.Equals("Current"))
        {
            if (gvPrevious.Rows.Count > 0)
            {
                int ItemId = common.myInt(hdnFavouriteItemId.Value);
                int GenericId = common.myInt(hdnFavouriteGenericId.Value);
                if (ItemId > 0 || GenericId > 0)
                {
                    GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(gvPrevious.Rows[index], gvPrevious, commandArgs);
                    gvPrevious_OnRowCommand(gvPrevious, eventArgs);

                    if (common.myBool(Session["IsCIMSInterfaceActive"]) || common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        if (common.myBool(Session["IsCIMSInterfaceActive"]))
                        {
                            if (common.myLen(hdnCIMSItemId.Value) < 2)
                            {
                                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                //lblMessage.Text = "This Drug is not mapped with CIMS.";
                            }
                            else
                            {
                                chkIsInteraction(ItemId);
                            }
                        }
                        //else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                        //{
                        //    if (common.myInt(hdnVIDALItemId.Value).Equals(0))
                        //    {
                        //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //        //lblMessage.Text = "This Drug is not mapped with VIDAL.";
                        //    }
                        //}
                    }
                }
            }
        }

        calcTotalQtySelectedMed();
        //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();
        setPrescriptionInstructoins();
    }

    protected void btnOrderSet_Click(object sender, EventArgs e)
    {
        rdoSearchMedication.SelectedValue = "OS";
        rdoSearchMedication.SelectedIndex = 2;
        rdoSearchMedication_OnSelectedIndexChanged(null, null);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindOrderSet();", true);


    }

    protected void btnFavourite_Click(object sender, EventArgs e)
    {
        rdoSearchMedication.SelectedValue = "F";
        rdoSearchMedication.SelectedIndex = 0;
        rdoSearchMedication_OnSelectedIndexChanged(null, null);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindFavourite();", true);
    }

    protected void btnCurrentMedication_Click(object sender, EventArgs e)
    {

        rdoSearchMedication.SelectedValue = "C";
        rdoSearchMedication.SelectedIndex = 1;
        rdoSearchMedication_OnSelectedIndexChanged(null, null);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "bindCurrentMedication();", true);
    }

    protected void imgMedicationPopup_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationPopup.aspx?Tab=F";

        RadWindow1.Height = 500;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "setFavorateItem_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void imgCurrentMedicationPopup_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationPopup.aspx?Tab=CM";

        RadWindow1.Height = 500;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "setCurrentMedication_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void imgOrderSet_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationPopup.aspx?Tab=OS";

        RadWindow1.Height = 500;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "setOrderSet_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void tbDiagnosis_ActiveTabChanged(object sender, EventArgs e)
    {
        //txtSearch.Visible = false;
        //if (common.myInt(tbDiagnosis.ActiveTabIndex).Equals(0))
        //{
        //    txtSearch.Visible = true;
        //}

        txtFavouriteItemName.Visible = false;
        if (common.myInt(tbDiagnosis.ActiveTabIndex).Equals(0))
        {
            txtFavouriteItemName.Visible = true;
        }
    }

    //protected void txtDuration_TextChanged(object sender, EventArgs e)
    //{
    //    calcTotalQtySelectedMed();
    //    //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();
    //    setPrescriptionInstructoins();
    //    // txtDuration.Focus();
    //    ddlPeriodType.Focus();
    //}

    //protected void txtDose_TextChanged(object sender, EventArgs e)
    //{
    //    calcTotalQtySelectedMed();
    //    //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();
    //    setPrescriptionInstructoins();
    // //   ddlUnit.Focus();
    //}

    //protected void ddlPeriodType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    calcTotalQtySelectedMed();
    //    //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();

    //    btnAddItem.Enabled = false;
    //    setPrescriptionInstructoins();
    //    btnAddItem.Enabled = true;

    //    // ddlPeriodType.Focus();
    //    //ddlPeriodType.TabIndex = 7;
    //    //ddlFoodRelation.TabIndex = 8;
    //    btnAddItem.Focus();
    //}

    protected void ddlFoodRelation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();
            setPrescriptionInstructoins();
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    public void setPrescriptionInstructoins()
    {
        txtInstructionsHeader.Text = "Take " + txtDose.Text + " " + ddlUnit.Text + ", " + ddlFrequencyId.Text + ", (" + ddlRoute.Text + "), For " + txtDuration.Text + " " + ddlPeriodType.Text;
    }
    private string CalSelectedPrescriptionDetails()
    {
        txtInstructionsHeader.Text = string.Empty;
        DataRow DR1;
        DataTable dt1 = new DataTable();
        decimal dQty = 0;
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {

            dt1 = CreateItemTable();


            DR1 = dt1.NewRow();
            string sFrequency = "0";
            //RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
            //TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
            if (ddlFrequencyId.SelectedValue != "0" && ddlFrequencyId.SelectedValue != string.Empty)
            {
                sFrequency = common.myStr(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            }

            //TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");
            //RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");
            //RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");
            //RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

            //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");
            string Type = string.Empty;
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

            if (Request.QueryString["DRUGORDERCODE"] == null)
            {
                if (common.myBool(ViewState["ISCalculationRequired"]))
                {
                    dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
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
            }
            else
            {
                dQty = 0;
            }

            //  DR1["Qty"] = common.myDbl(txtTotQty.Text).ToString("F2"); //dQty.ToString("F2");
            DR1["Qty"] = common.myDbl(txtTotQty.Text).ToString(("0.##") + " ");  //dQty.ToString("F2");

            //DR1["GenericId"] = ddlGeneric.SelectedValue == string.Empty ? 0 : common.myInt(ddlGeneric.SelectedValue);
            DR1["GenericId"] = common.myInt(hdnGenericId.Value);
            DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
            DR1["GenericName"] = common.myStr(ddlGeneric.Text);
            //DR1["ItemName"] = ddlBrand.SelectedValue == string.Empty ? string.Empty : ddlBrand.Text;
            DR1["ItemName"] = (!chkCustomMedication.Checked && common.myInt(ddlBrand.SelectedValue) > 0) ? ddlBrand.Text : txtCustomMedication.Text;

            DR1["StrengthValue"] = common.myStr(txtStrengthValue.Text);

            DR1["Dose"] = common.myDbl(txtDose.Text);
            DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
            DR1["Duration"] = txtDuration.Text;
            DR1["DurationText"] = Type;
            DR1["Type"] = ddlPeriodType.SelectedValue;

            DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
            DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

            DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
            DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? string.Empty : common.myStr(ddlFoodRelation.SelectedItem.Text);
            DR1["UnitId"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
            DR1["UnitName"] = ddlUnit.SelectedValue == string.Empty || ddlUnit.SelectedValue == "0" ? string.Empty : ddlUnit.SelectedItem.Text;
            DR1["CIMSItemId"] = common.myStr(hdnCIMSItemId.Value);
            DR1["CIMSType"] = common.myStr(hdnCIMSType.Value);
            DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
            DR1["PrescriptionDetail"] = string.Empty;
            DR1["CustomMedication"] = chkCustomMedication.Checked;
            //DR1["Instructions"] = common.clearHTMLTags(txtInstructions.Text);
            DR1["Instructions"] = string.Empty;
            DR1["ReferanceItemId"] = common.myInt(ddlReferanceItem.SelectedValue);
            DR1["ReferanceItemName"] = ddlReferanceItem.SelectedValue == string.Empty || ddlReferanceItem.SelectedValue == "0" ? string.Empty : ddlReferanceItem.Text;
            DR1["DoseTypeId"] = common.myInt(ddlDoseType.SelectedValue);
            DR1["DoseTypeName"] = ddlDoseType.SelectedValue == string.Empty || ddlDoseType.SelectedValue == "0" ? string.Empty : ddlDoseType.Text;
            DR1["FormulationId"] = common.myInt(ddlFormulation.SelectedValue);
            if (ddlFormulation.SelectedItem != null)
            {
                DR1["FormulationName"] = common.myStr(ddlFormulation.SelectedItem.Text);
            }
            if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
            {
                DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                DR1["IsInfusion"] = true;
                //  DR["IsInfusion"] = true;
            }
            else if (ddlDoseType.SelectedValue != "0")
            {
                DR1["IsInfusion"] = false;
                DR1["FrequencyId"] = 0;
                DR1["FrequencyName"] = string.Empty;
            }
            else
            {
                DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
                DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
                DR1["IsInfusion"] = false;
            }
            DR1["Volume"] = common.myStr(txtVolume.Text);
            DR1["VolumeUnitId"] = common.myInt(ddlVolumeUnit.SelectedValue);
            DR1["InfusionTime"] = common.myStr(txtTimeInfusion.Text);
            DR1["TimeUnit"] = common.myStr(ddlTimeUnit.SelectedValue);
            DR1["TotalVolume"] = common.myStr(txtTotalVolumn.Text);
            if (!string.IsNullOrEmpty(common.myStr(txtFlowRateUnit.Text)))
                DR1["FlowRate"] = common.myStr(txtFlowRateUnit.Text);
            else
                DR1["FlowRate"] = "0";
            DR1["FlowRateUnit"] = common.myInt(ddlFlowRateUnit.SelectedValue);
            DR1["XMLVariableDose"] = common.myStr(hdnXmlVariableDoseString.Value).Trim();
            DR1["XMLFrequencyTime"] = common.myStr(hdnXmlFrequencyTime.Value).Trim();

            //DR1["OverrideComments"] = common.myStr(txtCommentsDrugAllergy.Text);
            DR1["OverrideCommentsDrugToDrug"] = common.myStr(txtCommentsDrugToDrug.Text);
            //DR1["OverrideCommentsDrugHealth"] = common.myStr(txtCommentsDrugHealth.Text);
            DR1["IsSubstituteNotAllowed"] = chkSubstituteNotAllow.Checked;
            DR1["ICDCode"] = common.myStr(txtICDCode.Text);
            dt1.Rows.Add(DR1);
            dt1.AcceptChanges();
            //ddlReferanceItem.SelectedValue = "0";
            ddlReferanceItem.SelectedIndex = ddlReferanceItem.Items.IndexOf(ddlReferanceItem.Items.FindItemByValue("0"));

            //  txtInstructions.Text = string.Empty;
            // }

            dt1.TableName = "ItemDetail";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            dt1.WriteXml(writer);

            string xmlSchema = writer.ToString();
            xmlSchema = xmlSchema.Replace("&lt;", "<");
            xmlSchema = xmlSchema.Replace("&gt;", ">");



            DataTable dtPD = dt1.Copy().Clone();
            dtPD.Rows.Add(DR1.ItemArray);
            return emr.GetPrescriptionDetailStringV2(dtPD);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            DR1 = null;
            dt1.Dispose();
            dQty = 0;
            emr = null;
        }
        return "";
    }



    //protected void ddlRoute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    //txtInstructionsHeader.Text = CalSelectedPrescriptionDetails();
    //    setPrescriptionInstructoins();
    //    //btnAddItem.Focus();
    //    txtDuration.Focus();
    //}
    protected void chkAllbrand_CheckedChanged(object sender, EventArgs e)
    {
        //ddlBrand_OnItemsRequested(sender,e);
        GetBrandData("", common.myInt(ddlGeneric.SelectedValue));
    }
}