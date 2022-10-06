using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using Telerik.Web.UI;
using BaseC;
using System.IO;
using System.Net;
using System.Globalization;

public partial class Pharmacy_ItemIssueSaleReturn : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    //    private static VSDocumentService.documentServiceClient objDocumentService;
    clsExceptionLog objException = new clsExceptionLog();
    clsCIMS objCIMS = new clsCIMS();
    BaseC.HospitalSetup objHospitalSetup;//my
    string IsRequiredBLKDiscountPolicy;
    string IsAgeMadatory;
    string isCreditLimitApplicableforOPSale;
    string IsAllowCashSaleforCreditpatient;
    string IsWardIndentfromOPSale = "N";
    string IsApprovalCodeMandatoryforOPCreditSale = "N";
    string IsAllowChangeCompanyInOPCreditSale = "N";//f31082017
    string IsRequiredDirectPrintTwoTimesOPSale = "N";//f06092017


    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    //private string SysDomain1 = ConfigurationManager.AppSettings["SysDomain1"];

    private enum GridService : byte
    {
        Sno = 0,
        ItemName = 1,
        BatchNo = 2,
        ExpiryDate = 3,
        RequiredQty = 4,
        Qty = 5,
        Unit = 6,
        SellingPrice = 7,
        DiscountAmount = 8,
        Tax = 9,
        NetAmount = 10,
        PatientPayable = 11,
        PayerPayable = 12,
        MonographCIMS = 13,
        InteractionCIMS = 14,
        DHInteractionCIMS = 15,
        MonographVIDAL = 16,
        InteractionVIDAL = 17,
        DHInteractionVIDAL = 18,
        Delete = 19
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("Ward"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            //done by rakesh for user authorisation start
            SetPermission();
            //done by rakesh for user authorisation end
            if (!Page.IsCallback)
            {
                objHospitalSetup = new HospitalSetup(sConString);
                ViewState["CardNoRequired"] = objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "PaymentModesCardNumberRequired");
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }
            }
            
            Session["PreviousPageUrl"] = Request.Url;
            //if (common.myInt(Session["StoreId"]) == 0)
            //{
            //    if (!Page.IsCallback)
            //    {
            //        Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + common.myStr(Request.Url.PathAndQuery), false);
            //    }
            //}

            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            if (!IsPostBack)
            {
                Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
                Session["ForBLKDiscountPolicy"] = 0;
                Session["IsFromRequestFromWard"] = 0;
                Session["NetAmount"] = 0;
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                clsVIDAL objVIDAL = new clsVIDAL(sConString);
                objCIMS = new clsCIMS();

                string IsDisplayReqestfromWardinOPSale;




                if (common.myStr(Request.QueryString["Code"]) != "")
                {
                    hdnPageId.Value = common.myStr(Request.QueryString["Code"]);
                }
                if (common.myStr(Request.QueryString["IR"]) == "S"
                     && common.myStr(Request.QueryString["Code"]) == "CSI")
                {
                    tblPrescription.Visible = true;
                }
                dvConfirmProfileItem.Visible = false;
                btnAddNewItem.Visible = true;
                ddlPatientType.Visible = true;
                dvConfirm.Visible = false;
                ViewState["RequestFromWardItems"] = null;
                hdnSelectedGenericId.Value = "";
                hdnSelectedItemId.Value = "";
                //done by rakesh for user authorisation start
                //btnPrint.Visible = false;
                SetPermission(btnPrint, false);
                //done by rakesh for user authorisation end
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }

                //  hdnDecimalPlaces.Value = common.myStr(Cache["DecimalPlace"]);
                if (common.myInt(Session["HospitalLocationId"]) != 0)
                {
                    hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (common.myInt(hdnDecimalPlaces.Value) == 0)
                    {
                        hdnDecimalPlaces.Value = "2";
                    }
                }
                #region Interface

                if (common.myStr(Request.QueryString["IR"]) == "S"
                     && (common.myStr(Request.QueryString["Code"]) == "CSI")
                        || (common.myStr(Request.QueryString["Code"]) == "IPI"))
                {
                    setPatientInfo();
                    getLegnedColor();

                    ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
                    ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");
                    objEMR = new BaseC.clsEMR(sConString);
                    DataSet dsInterface = new DataSet();
                    if (common.myStr(Request.QueryString["Code"]) == "CSI")//op sale
                    {
                        dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForOPSale);
                    }
                    else if (common.myStr(Request.QueryString["Code"]) == "IPI")//ip issue
                    {
                        dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForIPIssue);
                    }
                    ViewState["IsCIMSInterfaceActive"] = false;
                    ViewState["IsVIDALInterfaceActive"] = false;
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

                        if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc"))
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
                            //     objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");

                            WebClient client = new WebClient();
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sVidalConString + "DocumentService");
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "VIDAL web-services not running now !";

                                //Alert.ShowAjaxMsg(lblMessage.Text, this);

                                return;
                            }
                        }
                        catch
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "VIDAL web-services not running now !";

                            //Alert.ShowAjaxMsg(lblMessage.Text, this);

                            return;
                        }
                    }
                }

                #endregion
                clearControl();
                BindDoctor();
                isCreditLimitApplicableforOPSale = common.myStr(objBill.getHospitalSetupValue("IsCreditLimitApplicableforOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                hdnIsStripWiseSaleRequired.Value = common.myStr(objBill.getHospitalSetupValue("IsStripWiseSaleRequired", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                hdnIsDoctorMandatoryinOPSale.Value = common.myStr(objBill.getHospitalSetupValue("IsDoctorMandatoryinOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                hdnIsAgeMandatoryinOPSale.Value = common.myStr(objBill.getHospitalSetupValue("IsAgeMandatoryinOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                IsApprovalCodeMandatoryforOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsApprovalcodeMandatoryforCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

                IsAllowCashSaleforCreditpatient = common.myStr(objBill.getHospitalSetupValue("IsAllowCashSaleforCreditpatient", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

                dtpDocDate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
                hdnItemIssueId.Value = "0";
                bindPatientTypeDDL();
                InsurancecompanyBind("", 0);
                hdnDefaultCompanyId.Value = common.myInt(objBill.getDefaultCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))).ToString();
                ddlPatientType.SelectedIndex = 0;
                ddlPatientType_SelectedIndexChanged(null, null);
                if (common.myStr(Request.QueryString["RegNo"]) != "")
                {
                    txtRegistrationNo.Text = common.myStr(Request.QueryString["RegNo"]);
                    btnSearchByUHID_OnClick(this, null);
                }
                Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                RadTabStrip1.Tabs[0].Visible = true;
                //HtmlGenericControl theDiv = new HtmlGenericControl("idMarkForDischarge");

                if (common.myStr(objBill.getHospitalSetupValue("IsAllowCashSaleOutstanding", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y"
                    && common.myStr(Request.QueryString["Code"]) == "CSI")
                {
                    chkCashOutStanding.Visible = true;
                }

                if (Request.QueryString["Code"] == "IPI")//Ipd sale
                {

                    //idMarkForDischarge.Attributes.Add("class","col-md-3 pull-right text-right");
                    //BLKDiscount.Attributes.Add("class", "col-md-5 hide");
                    lblForIp.Visible = true;
                    lblAgeStar.Visible = true;

                    btnRequestFromWard.Visible = true;
                    btndDiscount.Visible = false;
                    txtProvider.Visible = false;
                    chkDoctor.Visible = false;
                    ddlDoctor.Visible = true;
                    ViewState["TransactionType"] = "IPISS";
                    txtReceived.Visible = false;
                    Literal4.Visible = false;
                    Label1.Visible = false;
                    txtAmountCollected.Visible = false;
                    Label22.Visible = false;
                    txtRefundamount.Visible = false;
                    //btnSurgicalKit.Visible = true;
                    grdPaymentMode.Visible = false;
                    RadTabStrip1.Tabs[1].Visible = false;
                    rpvPayment.Visible = false;
                    //btnBarcodePrinting.Visible = false;
                    btndDiscount.Visible = true;
                    lblDiscountType.Visible = false;
                    ddlDiscountType.Visible = false;
                    btnDiscountApply.Visible = false;
                    lblDiscPerc.Visible = false;
                    txtDiscPerc.Visible = false;

                }
                else // op sale
                {


                    if (IsApprovalCodeMandatoryforOPCreditSale == "Y")
                    {
                        if ((ddlPatientType.Text.ToUpper() == "CREDIT SALE"))
                        {
                            lblApprovalCodeS.Visible = true;
                            lblRemarkS.Visible = true;
                        }
                        else
                        {
                            lblApprovalCodeS.Visible = false;
                            lblRemarkS.Visible = false;
                        }
                    }

                    //idMarkForDischarge.Attributes.Add("class", "col-md-1 pull-right text-right");
                    //BLKDiscount.Attributes.Add("class", "col-md-5");
                    if (hdnIsDoctorMandatoryinOPSale.Value == "Y")
                    {
                        lblForIp.Visible = true;
                    }
                    else
                    {
                        lblForIp.Visible = false;
                    }

                    if (hdnIsAgeMandatoryinOPSale.Value == "Y")
                    {
                        lblAgeStar.Visible = true;
                    }
                    else
                    {
                        lblAgeStar.Visible = false;
                    }

                    //for child trust by tony start
                    if (common.myStr(objBill.getHospitalSetupValue("isDisplayRequestFromWardinOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                    {
                        btnRequestFromWard.Visible = true;
                    }
                    else
                    {
                        btnRequestFromWard.Visible = false;
                    }
                    //for child trust by tony end
                    lblBedNo.Visible = false;
                    txtBedNo.Visible = false;
                    chkDoctor.Visible = true;
                    ViewState["TransactionType"] = "OPISS";
                    ddlDoctor.Visible = true;
                    txtReceived.Visible = true;
                    Literal4.Visible = true;
                    Label1.Visible = true;
                    txtAmountCollected.Visible = true;
                    Label22.Visible = true;
                    txtRefundamount.Visible = true;
                    BindStaffCompanyId();
                    //btnSurgicalKit.Visible = false;
                    btnPendingRequestFromWard.Visible = false;
                    grdPaymentMode.Visible = true;
                    RadTabStrip1.Tabs[1].Visible = true;
                    rpvPayment.Visible = true;
                    //btnBarcodePrinting.Visible = false;
                    IsRequiredBLKDiscountPolicy = common.myStr(objBill.getHospitalSetupValue("IsRequiredBLKDiscountPolicy", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (common.myStr(IsRequiredBLKDiscountPolicy) == "Y")
                    {
                        btndDiscount.Visible = false;
                        lblDiscountType.Visible = true;
                        ddlDiscountType.Visible = true;
                        btnDiscountApply.Visible = true;
                        lblDiscPerc.Visible = true;
                        txtDiscPerc.Visible = true;
                    }
                    else
                    {
                        btndDiscount.Visible = true;
                        lblDiscountType.Visible = false;
                        ddlDiscountType.Visible = false;
                        btnDiscountApply.Visible = false;
                        lblDiscPerc.Visible = false;
                        txtDiscPerc.Visible = false;
                    }


                    if (Request.QueryString["ER"] != null)
                    {
                        btnViewOrders.Visible = true;
                        //lnkPatientHistory.Visible = false;
                    }
                    else
                    {
                        btnViewOrders.Visible = false;
                        //lnkPatientHistory.Visible = true;
                    }
                }
                ddlPayer_OnSelectedIndexChanged(sender, e);
                BindDoctor();
                txtRegistrationNo.Focus();

                if (common.myStr(Request.QueryString["IR"]) == "S"
                     && common.myStr(Request.QueryString["Code"]) == "IPI")
                {
                    tblMarkedForDischarge.Visible = true;
                    NoofMarkfordischare();
                }

                txtBarCodeValue.Attributes.Add("autocomplete", "off");
                NoofPendingPharmacyClearance();
                BindDiscountType();
                objHospitalSetup = new BaseC.HospitalSetup(sConString);//my
                hdnSaveIssueId.Value = hdnIssueId.Value;

                if (common.myStr(objBill.getHospitalSetupValue("IsDirectPrintOptionRequiredOPIPIssue", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                {
                    chktoprinter.Checked = true;
                }
                else
                {
                    chktoprinter.Checked = false;
                }

                BindStore();
                
            }
            else
            {
                hdnSaveIssueId.Value = "";
            }

            if (common.myStr(objBill.getHospitalSetupValue("IsDiscountEnableOpSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) != "Y")
            {
                BLKDiscount.Visible = false;
                btndDiscount.Visible = false;
            }
            if (common.myStr(objBill.getHospitalSetupValue("PharmacyCleranceDisable", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
            {
                Divphrclear.Visible = false;
            }

            String IsGSTApplicable = common.myStr(objBill.getHospitalSetupValue("IsGSTApplicable", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (IsGSTApplicable == "Y")
            {
                gvService.Columns[9].Visible = false;
            }

            IsWardIndentfromOPSale = "N";

            dtpFollowupDate.MinDate = DateTime.Today.Date;
            dtpFollowupDate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
            Legend1.loadLegend("CIMSInterface", "");
            setGridColor();
            Session["SourceCardPayment"] = "OPS";

            //HideColumn();

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
                txtProvisionalDiagnosis.Text = ds.Tables[0].Rows[0]["ProvisionalDiagnosis"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #region Page Controls
    public void bindPatientTypeDDL()
    {
        try
        {
            int storId = common.myInt(ddlStore.SelectedValue);
            DataSet ds = new DataSet();
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            ddlPatientType.Items.Clear();
            ds = objPharmacy.GetSaleSetupMaster(0, common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserID"]));

            DataView DV = ds.Tables[0].Copy().DefaultView;

            switch (common.myStr(hdnPageId.Value))
            {
                case "CSI":
                    DV.RowFilter = "SetupTypeCode = 'OP-ISS'";
                    break;
                case "IPI":
                    DV.RowFilter = "SetupTypeCode = 'IP-ISS'";
                    break;

                default:
                    break;
            }
            DV.RowFilter = "Code in ('IP-ISS','IP-RET')";
            foreach (DataRow dr in DV.ToTable().Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["SaleSetupName"]);
                item.Value = common.myStr(common.myInt(dr["SaleSetupId"]));
                item.Attributes.Add("StatusCode", common.myStr(dr["SetupTypeCode"]));
                this.ddlPatientType.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    public void BindDiscountType()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            ddlDiscountType.Items.Clear();
            ds = objPharmacy.GetDiscountType();

            DataView DV = ds.Tables[0].Copy().DefaultView;

            foreach (DataRow dr in DV.ToTable().Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["DiscountTypeName"]);
                item.Value = common.myStr(dr["DiscountType"]);
                item.Attributes.Add("DiscountPercentage", common.myStr(dr["DiscountPrecentage"]));
                this.ddlDiscountType.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        ViewState["AutoPresc"] = "N";
        Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
        if (Request.QueryString["ER"] != null)
        {
            btnViewOrders.Visible = true;
            //lnkPatientHistory.Visible = false;
        }
        else
        {
            btnViewOrders.Visible = false;
            //lnkPatientHistory.Visible = true;
        }

        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    public void setControlHospitalbased()
    {
        string IsPasswordRequired = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "IsPasswordRequired", sConString);
        ViewState["IsPasswordRequired"] = IsPasswordRequired;
        if (IsPasswordRequired != "")
        {
            hdnIsValidPassword.Value = "1";
            btnIsValidPasswordClose_OnClick(null, null);
        }
        else
        {
            IsValidPassword();

        }

    }
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                //BindPreviousData();
                lblMessage.Text = "Invalid Password !";
                return;
            }

            if (hdnbtnStatus.Value == "N")
            {
                hdnbtnStatus.Value = "";
                return;

            }


            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                lblMessage.Text = "";
                if (common.myInt(Session["ItemIssueSaleReturnDuplicateCheck"]) == 0)
                {
                    SaveData();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        Session["SpecialRightID"] = null;
        txtNetAmount.Text = Request[txtNetAmount.UniqueID];
        txtLAmt.Text = Request[txtLAmt.UniqueID];
        txtRounding.Text = Request[txtRounding.UniqueID];
        txtReceived.Text = Request[txtReceived.UniqueID];

        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (strpatienttype != "DP" && strpatienttype != "CN")
        {
            if (!IsSave())
            {
                return;
            }
        }
        BaseC.HospitalSetup objsec = new BaseC.HospitalSetup(sConString);
        string IsUserAllowAuthentication = common.myStr(objsec.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "UserAuthenticationForInventory", common.myInt(Session["FacilityId"])));
        if (IsUserAllowAuthentication == "Y")
        {
            IsValidPassword();
        }
        else
        {
            hdnIsValidPassword.Value = "1";
            Session["SaveUserId"] = common.myInt(Session["UserId"]);
            btnIsValidPasswordClose_OnClick(sender, e);

        }
    }
    private void SaveData()
    {
        try
        {
            Double CashTotAmt = 0;
            Double TotAmt = 0;
            StringBuilder objXML = new StringBuilder();
            StringBuilder objXMLdesp = new StringBuilder();
            StringBuilder sXMLPaymentMode = new StringBuilder();
            ArrayList coll = new ArrayList();
            ArrayList col2 = new ArrayList();
            //string IsApprovalCodeMandatoryforOPCreditSale = "N";
            string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            isCreditLimitApplicableforOPSale = common.myStr(objBill.getHospitalSetupValue("IsCreditLimitApplicableforOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            IsApprovalCodeMandatoryforOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsApprovalcodeMandatoryforCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));


            DataTable dtgrd = new DataTable();
            if (Convert.ToString(ViewState["Servicetable"]) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Fill Item Details !";
                return;
            }
            dtgrd = ((DataTable)ViewState["Servicetable"]).Clone();
            dtgrd.Columns.Add("Sno");
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            int iSno = 0;
            string Source = "P";
            if ((btnAddNewItem.Visible) || (common.myStr(ViewState["ByIndent"]) == "Y"))
            {
                //Normal Save Issue
                foreach (GridViewRow gvRow in gvService.Rows)
                {
                    TextBox txtQuantity = (TextBox)gvRow.FindControl("txtQty");

                    if (common.myDbl(txtQuantity.Text) <= 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Quantity is zero or less than zero, please check !.";
                        txtQuantity.Focus();
                        return;
                    }

                    coll.Add(++iSno);//SequenceNo INT
                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnItemId")).Value.Trim()));//ItemId INT
                    coll.Add(common.myStr(((Label)gvRow.FindControl("lblRequestedItemId")).Text.Trim()));//RequestedItemId INT
                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnBatchId")).Value.Trim()));//BatchId INT
                    coll.Add(common.myStr(((Label)gvRow.FindControl("lblBatchNo")).Text.Trim()));//BatchNo varchar(20)
                    coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtQty")).Text.Trim()));//Qty MONEY
                    coll.Add(common.myDec(((TextBox)gvRow.FindControl("txtPatient")).Text.Trim())); //EmpAmt MONEY
                    coll.Add(common.myDec(((TextBox)gvRow.FindControl("txtPayer")).Text.Trim())); //CompAmt MONEY
                    coll.Add(common.myDec(((HiddenField)gvRow.FindControl("hdnPercentDiscount")).Value.Trim()));//DiscPerc MONEY
                    coll.Add(common.myDec(((TextBox)gvRow.FindControl("txtDiscountAmt")).Text.Trim()));//DiscAmt MONEY
                    coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtTax")).Text.Trim()));//SaleTaxPerc MONEY
                    coll.Add(common.myDbl(((TextBox)gvRow.FindControl("txtTax")).Text.Trim()));//SaleTaxAmt MONEY
                    coll.Add(common.myDec(((HiddenField)gvRow.FindControl("hdnCostPrice")).Value.Trim()));//CostPrice MONEY
                    coll.Add(common.myDec(((TextBox)gvRow.FindControl("txtCharge")).Text.Trim()));//MRP MONEY
                    coll.Add(common.myDec(((TextBox)gvRow.FindControl("txtNetAmt")).Text.Trim()));//NetAmt MONEY
                    coll.Add(0); //RefIssueId INT = 0 when Issue and Issue Id When Return

                    Label lblExpiryDate = (Label)gvRow.FindControl("lblExpiryDate");
                    string day, month, year, ExpiryDate;
                    ExpiryDate = lblExpiryDate.Text;
                    //ExpiryDate = Convert.ToDateTime(ExpiryDate).ToString("MM/dd/yyyy");
                    //day = ExpiryDate.Substring(0, 2);
                    //month = ExpiryDate.Substring(3, 2);
                    //year = ExpiryDate.Substring(6, 4);
                    //ExpiryDate = month + "/" + day + "/" + year;

                    if (ExpiryDate.IndexOf('/') == 4 || ExpiryDate.IndexOf('-') == 4)
                    {
                        ExpiryDate = Convert.ToDateTime(ExpiryDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        day = ExpiryDate.Substring(0, 2);
                        month = ExpiryDate.Substring(3, 2);
                        year = ExpiryDate.Substring(6, 4);
                        ExpiryDate = month + "/" + day + "/" + year;
                    }

                    coll.Add(common.myStr(ExpiryDate));//ItemExpiryDate SMALLDATETIME
                    coll.Add(common.myDec(((HiddenField)gvRow.FindControl("hdnCopayPerc")).Value.Trim()));//CoPayPerc INT
                    coll.Add(common.myDec(((HiddenField)gvRow.FindControl("hdnCopayAmt")).Value.Trim()));//CopayAmt INT
                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnPrescriptionDetailsId")).Value.Trim())); //PrescriptionDetailsId INT
                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnGenericId")).Value.Trim())); //GenericId INT
                    if (((TextBox)gvRow.FindControl("txtReusableRemarks")).Text != "")//my 13102016
                    {
                        coll.Add(common.myStr(((TextBox)gvRow.FindControl("txtReusableRemarks")).Text));
                    }
                    else
                    {
                        coll.Add(common.myStr(((TextBox)gvRow.FindControl("txtReusableRemarks")).Text.Trim()));//my 13102016
                    }


                    objXML.Append(common.setXmlTable(ref coll));
                }

            }
            else
            {
                //Request From Ward Save Issue 

                if (ViewState["RequestFromWardItems"] != null)
                {
                    Source = "W";
                    DataTable dt = new DataTable();
                    dt = ((DataTable)ViewState["RequestFromWardItems"]).Copy();

                    foreach (GridViewRow gvRow in gvService.Rows)
                    {
                        TextBox txtQuantity = (TextBox)gvRow.FindControl("txtQty");

                        if (common.myDbl(txtQuantity.Text) <= 0)
                        {
                            continue;
                        }

                        HiddenField hdnBatchXML = (HiddenField)gvRow.FindControl("hdnBatchXML");

                        string xmlSchema = common.myStr(hdnBatchXML.Value).Trim();
                        if (xmlSchema == "")
                        {
                            continue;
                        }

                        StringReader sr = new StringReader(xmlSchema);
                        DataSet dsXml = new DataSet();
                        dsXml.ReadXml(sr);

                        if (dsXml.Tables.Count > 0)
                        {
                            for (int rowIdx = 0; rowIdx < dsXml.Tables[0].Rows.Count; rowIdx++)
                            {

                                DataRow DR = dsXml.Tables[0].Rows[rowIdx];
                                if (common.myDbl(DR["Qty"]) > 0)
                                {
                                    HiddenField hdnPercentDiscount = (HiddenField)gvRow.FindControl("hdnPercentDiscount");
                                    double discAmt = (common.myDbl(DR["Qty"]) * common.myDbl(DR["MRP"]) * common.myDbl(hdnPercentDiscount.Value)) / 100.00;
                                    //common.myDbl(DR["DiscAmtPercent"])
                                    double netAmt = (common.myDbl(DR["Qty"]) * common.myDbl(DR["MRP"])) - discAmt;

                                    double PatientAmount = 0;
                                    double PayerAmount = 0;

                                    ////if (common.myInt(ddlPayer.SelectedValue) == common.myInt(hdnDefaultCompanyId.Value))
                                    ////{
                                    ////    PatientAmount = netAmt;
                                    ////}
                                    ////else
                                    ////{
                                    ////    PayerAmount = netAmt;
                                    ////}

                                    HiddenField hdnPrescriptionDetailsId = (HiddenField)gvRow.FindControl("hdnPrescriptionDetailsId");
                                    int isDhaApproved = common.myInt(((HiddenField)gvRow.FindControl("hdnISDHAApproved")).Value.Trim());
                                    if (hdnPrescriptionDetailsId.Value.ToString().Trim() == "")
                                        isDhaApproved = 1;
                                    double hdnCopayPerc = common.myDbl(((HiddenField)gvRow.FindControl("hdnCopayPerc")).Value.Trim());
                                    double copayAmt = 0;

                                    if (hdnPaymentType.Value == "C")// || (isDhaApproved == 0 && common.myStr(txtApprovalCode.Text.Trim()).Length <= 4))//cash sale
                                    {
                                        PatientAmount = netAmt;
                                    }
                                    else
                                    {
                                        if (hdnCopayPerc > 0)
                                        {
                                            copayAmt = (hdnCopayPerc / 100) * (netAmt);

                                            PatientAmount = copayAmt;
                                            PayerAmount = (netAmt) - copayAmt;

                                            //PatientAmount = Math.Round(copayAmt, 2);
                                            //PayerAmount = netAmt - Math.Round(copayAmt, 2);
                                        }
                                        else
                                            PayerAmount = netAmt;
                                    }

                                    coll.Add(++iSno);//SequenceNo INT
                                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnItemId")).Value));//ItemId INT
                                    coll.Add(common.myStr(((Label)gvRow.FindControl("lblRequestedItemId")).Text.Trim()));//RequestedItemId INT
                                    coll.Add(common.myInt(DR["BatchId"]));//BatchId INT
                                    coll.Add(common.myStr(DR["BatchNo"]));//BatchNo varchar(20)
                                    coll.Add(common.myDbl(DR["Qty"]));//Qty MONEY
                                    //coll.Add(common.myStr(((TextBox)gvRow.FindControl("txtQty")).Text.Trim()));//Qty MONEY
                                    coll.Add(PatientAmount); //EmpAmt MONEY
                                    coll.Add(PayerAmount); //CompAmt MONEY
                                    coll.Add(common.myDbl(hdnPercentDiscount.Value));//DiscPerc MONEY
                                    coll.Add(discAmt);//DiscAmt MONEY
                                    coll.Add(common.myDbl(DR["Tax"]));//SaleTaxPerc MONEY
                                    coll.Add(common.myDbl(DR["Tax"]));//SaleTaxAmt MONEY
                                    coll.Add(common.myDbl(DR["CostPrice"]));//CostPrice MONEY
                                    coll.Add(common.myDbl(DR["MRP"]));//MRP MONEY
                                    coll.Add(netAmt);//NetAmt MONEY
                                    coll.Add(0); //RefIssueId INT = 0 when Issue and Issue Id When Return
                                    coll.Add(common.myStr(DR["ExpiryDate"]));//ItemExpiryDate SMALLDATETIME
                                    coll.Add(common.myDbl(hdnCopayPerc)); //CopayPerc Money,
                                    coll.Add(common.myDbl(copayAmt)); //CopayAmt Money,
                                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnPrescriptionDetailsId")).Value.Trim())); //PrescriptionDetailsId INT
                                    coll.Add(common.myInt(((HiddenField)gvRow.FindControl("hdnGenericId")).Value.Trim())); //GenericId INT
                                    if (((TextBox)gvRow.FindControl("txtReusableRemarks")).Text != "")//my 13102016
                                    {
                                        coll.Add(common.myStr(((TextBox)gvRow.FindControl("txtReusableRemarks")).Text));
                                    }
                                    else
                                    {
                                        coll.Add(common.myStr(((TextBox)gvRow.FindControl("txtReusableRemarks")).Text.Trim()));//my 13102016
                                    }
                                    objXML.Append(common.setXmlTable(ref coll));
                                }
                            }
                        }
                    }
                }
            }

            if (objXML.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Fill Item Details !";
                return;
            }



            DataTable dtdesp = new DataTable();
            if (Session["despenseDetails"] != null)
            {
                dtdesp = (DataTable)Session["despenseDetails"];

                foreach (DataRow row in dtdesp.Rows)
                {
                    coll.Add(common.myInt(row["IndentId"]));
                    coll.Add(common.myInt(row["ItemId"]));
                    coll.Add(common.myInt(row["DespenseQty"]));
                    coll.Add(common.myBool(row["IsClose"]));
                    objXMLdesp.Append(common.setXmlTable(ref coll));
                }

            }

            int cntCash = 0;
            int cntDate = 0;
            int cntAmt = 0;
            double dblCashAmt = 0;
            int iDefaultCompanyId = common.myInt(hdnDefaultCompanyId.Value);
            int iPayerId = common.myInt(ddlPayer.SelectedValue);
            // saving for payment mode for OP sale
            if (strpatienttype != "IP-ISS")
            {
                if (ddlPatientType.Text.ToUpper() != "CREDIT SALE" || ddlPatientType.Text.ToUpper() == "CREDIT SALE")
                {
                    Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                    foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
                    {
                        DropDownList ddlModeType = (DropDownList)gvr.FindControl("ddlMode");
                        DropDownList ddlBank = (DropDownList)gvr.FindControl("ddlBankName");
                        DropDownList ddlCredit = (DropDownList)gvr.FindControl("ddlCreditCard");
                        TextBox txtCheque = (TextBox)gvr.FindControl("txtChequeNo");
                        RadDatePicker txtChqDate = (RadDatePicker)gvr.FindControl("txtChequeDate");
                        DropDownList ddlClientBankName = (DropDownList)gvr.FindControl("ddlClientBankName");
                        HiddenField hdnPercentDiscount = (HiddenField)gvService.Rows[0].FindControl("hdnPercentDiscount");
                        TextBox txtTransactionRefNo = (TextBox)gvr.FindControl("txtTransactionRefNo");
                        HiddenField hdnTypeMappingCode = (HiddenField)gvr.FindControl("hdnTypeMappingCode");
                        TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                        if (txtAmt.Text == "")
                            txtAmt.Text = "0";
                        TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                        if (txtBal.Text == "")
                            txtBal.Text = "0";
                        TextBox txtDesc = (TextBox)gvr.FindControl("txtDescription");
                        if (common.myStr(hdnPercentDiscount.Value) == "")
                        {
                            hdnPercentDiscount.Value = "0";
                        }

                        if (ddlModeType.SelectedItem.Value != "1")
                        {
                            if (txtChqDate.SelectedDate.ToString().Trim() == "")
                            {
                                cntDate += 1;
                                if (cntDate > 1)
                                {
                                    Alert.ShowAjaxMsg("Date Cannot Be Blank For The Mode Type " + ddlModeType.Text + "...", this.Page);
                                    return;
                                }
                            }
                        }
                        if (txtAmt.Text == "0" && common.myInt(hdnPercentDiscount.Value) != 100)
                        {
                            cntAmt += 1;
                            if (cntAmt != 0)
                            {
                                if (chkCashOutStanding.Visible == true)
                                {
                                    if (chkCashOutStanding.Checked == false)
                                    {
                                        Alert.ShowAjaxMsg("Amount Cannot Be Blank For The Mode Type " + ddlModeType.Text + "...", this.Page);
                                        return;
                                    }
                                }
                                else
                                {
                                    Alert.ShowAjaxMsg("Amount Cannot Be Blank For The Mode Type " + ddlModeType.Text + "...", this.Page);
                                    return;
                                }
                            }
                        }
                        if (ddlModeType.SelectedItem.Value == "1")
                        {
                            cntCash += 1;
                            dblCashAmt += Convert.ToDouble(txtAmt.Text);
                            if (cntCash > 1)
                            {
                                Alert.ShowAjaxMsg("Can Have Only One Cash Selection...", this.Page);
                                return;
                            }
                        }

                        if (ddlModeType.SelectedItem.Value != "1")
                        {
                            if (txtCheque.Text.ToString().Trim().Length == 0)
                            {
                                txtCheque.Focus();
                                Alert.ShowAjaxMsg("Enter Mode No...", this.Page);
                                return;
                            }
                            if (txtChqDate.SelectedDate.ToString().Trim().Length == 0)
                            {
                                txtChqDate.Focus();
                                Alert.ShowAjaxMsg("Enter Mode Date...", this.Page);
                                return;
                            }

                            if (common.myStr(ViewState["CardNoRequired"]) == "Y")
                            {
                                if (hdnTypeMappingCode.Value.Equals("2") || hdnTypeMappingCode.Value.Equals("3"))
                                {
                                    if (txtTransactionRefNo.Text.Equals(string.Empty))
                                    {
                                        Alert.ShowAjaxMsg("Required Card No/Cheque No. For The Mode Type " + ddlModeType.SelectedItem.Text + "...", this.Page);
                                        return;
                                    }
                                }
                            }

                        }
                        TotAmt = TotAmt + Convert.ToDouble(txtAmt.Text);
                        if (ddlPatientType.Text.ToUpper() != "STAFF SALE")
                        {
                            col2.Add(common.myDbl(txtAmt.Text));//</c1>Amount<c2>
                            col2.Add(common.myInt(ddlModeType.SelectedValue));//</c2>ModeId<c3>
                        }
                        else
                        {
                            col2.Add("0");
                            col2.Add("0");
                        }

                        col2.Add("");
                        if (ddlModeType.SelectedItem.Value == "1")
                        {
                            CashTotAmt = CashTotAmt + Convert.ToDouble(txtAmt.Text);
                            col2.Add(common.myDbl(txtReceived.Text));
                            //col2.Add(common.myDbl(txtAmt.Text));
                            col2.Add("");
                            col2.Add(DBNull.Value);
                            col2.Add(DBNull.Value);
                        }
                        else
                        {

                            //col2.Add(common.myDbl(txtAmt.Text));
                            col2.Add("0");
                            col2.Add("0");
                            col2.Add(txtCheque.Text);
                            col2.Add(common.myDate(txtChqDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
                        }
                        if (ddlModeType.SelectedItem.Value == "1")
                        {
                            col2.Add(DBNull.Value);
                        }
                        // else if (ddlModeType.SelectedItem.Value == "3")
                        else if (hdnTypeMappingCode.Value.Equals("2"))
                        {
                            if (ddlCredit.SelectedIndex == 0)
                            {
                                Alert.ShowAjaxMsg("Please Select Any Credit Card Bank...", this.Page);
                                return;
                            }
                            else
                            {
                                //col2.Add(DBNull.Value);
                                col2.Add(ddlCredit.SelectedValue);
                            }
                        }
                        else
                        {
                            if ((ddlBank.SelectedIndex == 0) && (hdnTypeMappingCode.Value != "1"))
                            {
                                Alert.ShowAjaxMsg("Please Select Any Bank...", this.Page);
                                return;
                            }
                            else
                            {
                                col2.Add(ddlBank.SelectedItem.Value);
                            }
                        }
                        col2.Add(txtDesc.Text.ToString().Trim());//Description
                        if (ddlModeType.SelectedItem.Value != "1")
                        {
                            if (ddlClientBankName.SelectedIndex == 0)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "Please Select Any Beneficiary Name...";
                                Alert.ShowAjaxMsg("Please Select Any Beneficiary Name...", this.Page);
                                return;
                            }
                            else
                            {
                                col2.Add(ddlClientBankName.SelectedValue);//ClientBankId

                            }
                        }
                        else
                        {
                            col2.Add(DBNull.Value);//ClientBankId 
                        }

                        if (!common.myStr(txtTransactionRefNo.Text).Equals(string.Empty))
                        {
                            col2.Add(txtTransactionRefNo.Text);
                        }
                        else
                        {
                            col2.Add(DBNull.Value);
                        }


                        sXMLPaymentMode.Append(common.setXmlTable(ref col2));
                    }
                }
            }

            if (sXMLPaymentMode.ToString() == "" && strpatienttype == "OP-ISS" && ddlPatientType.Text.ToUpper() != "CREDIT SALE")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Enter Payment Amount !";
                return;
            }

            if (Math.Round(common.myDbl(txtNetAmount.Text), MidpointRounding.AwayFromZero) != common.myDbl(TotAmt) && strpatienttype != "IP-ISS" &&
                iPayerId == iDefaultCompanyId && (ddlPatientType.Text.ToUpper() != "CREDIT SALE" ||
                (ddlPatientType.Text.ToUpper() == "CREDIT SALE")))
            {
                if (chkCashOutStanding.Visible == true)
                {
                    if (chkCashOutStanding.Checked == false)
                    {
                        Alert.ShowAjaxMsg("Amount Collected And Total Invoice Amount Is Not Equal! Partial Payment is not Permitted.", Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Amount Collected And Total Invoice Amount Is Not Equal! Partial Payment is not Permitted.", Page);
                    return;
                }
            }

            if (common.myStr(hdnFacilityId.Value) != "0" && common.myStr(hdnFacilityId.Value) != "")
            {
                ViewState["FacilitiId"] = common.myStr(hdnFacilityId.Value);
            }
            else
            {
                ViewState["FacilitiId"] = common.myStr(Session["FacilityId"]);
            }
            int issuedid;
            string docNo = "";
            bool btReturnError = false;
            string Doctorname = "";
            int doctorId = 0;
            if (strpatienttype != "IP-ISS")
            {
                if (chkDoctor.Checked == true)
                {
                    Doctorname = txtProvider.Text;
                }
                else
                {
                    doctorId = common.myInt(ddlDoctor.SelectedValue);
                }
            }
            else
            {
                doctorId = common.myInt(ddlDoctor.SelectedValue);

            }
            double Copayment = 0;
            double Copercent = 0;
            string checkCopayment = "N";
            DataTable dt1 = (DataTable)ViewState["Servicetable"];
            string TotalCoPayAmt, TotalPayerAmount = "";
            TotalCoPayAmt = dt1.Compute("Sum(CopayAmt)", "").ToString();
            TotalPayerAmount = dt1.Compute("Sum(PayerAmount)", "").ToString();
            if (common.myInt(ViewState["PharmacyCreditLimit"]) > 0)
            {
                if ((common.myDbl(TotalPayerAmount) > common.myDbl(ViewState["PharmacyCreditlimit"])) && (txtApprovalCode.Text.Trim() == ""))
                {
                    Alert.ShowAjaxMsg("Please Enter the Approval Code", Page);
                    return;
                }
            }
            if (IsApprovalCodeMandatoryforOPCreditSale == "Y")
            {
                if ((ddlPatientType.Text.ToUpper() == "CREDIT SALE") && (txtApprovalCode.Text.Trim() == ""))
                {
                    Alert.ShowAjaxMsg("Please Enter the Approval Code", Page);
                    txtApprovalCode.Focus();
                    return;
                }
                if ((ddlPatientType.Text.ToUpper() == "CREDIT SALE") && (txtRemark.Text.Trim() == ""))
                {
                    Alert.ShowAjaxMsg("Please Enter the Remark", Page);
                    txtRemark.Focus();
                    return;
                }
            }
            int PayerId = 0;
            PayerId = common.myStr(ViewState["OPIP"]) == "E" ? common.myInt(hdnERCompanyId.Value) : common.myInt(ddlPayer.SelectedValue);
            
            bool bitValidateDepositExhaust = true;
            if (common.myInt(Session["SpecialRightID"]) > 0)
            {
                bitValidateDepositExhaust = false;
            }
            else
            {
                bitValidateDepositExhaust = true;

            }
            if (isCreditLimitApplicableforOPSale == "Y")
            {
                if (Math.Round(common.myDbl(txtNetAmount.Text), MidpointRounding.AwayFromZero) > common.myDbl(lblAvailLimit.Text) &&
                    ddlPatientType.Text.ToUpper() == "CREDIT SALE" && Request.QueryString["Code"] != "IPI") //iPayerId == iDefaultCompanyId &&
                {
                    Alert.ShowAjaxMsg("Credit Limit Exceeded..", Page);
                }
            }
            int intUserId = 0;
            if (common.myInt(Session["SaveUserId"]) == 0)
            {
                intUserId = common.myInt(Session["UserId"]);
            }
            else
            {
                intUserId = common.myInt(Session["SaveUserId"]);
            }
            bool bCashOutStanding = false;
            if (chkCashOutStanding.Visible == true && chkCashOutStanding.Checked == true)
            {
                bCashOutStanding = true;
            }
            BaseC.clsPharmacy objPharmacy = new clsPharmacy(sConString);
            //if (common.myStr(Session["SaveDuplicate"]) == "0")
            //{

            Session["ItemIssueSaleReturnDuplicateCheck"] = 1;
            string strMsg = objPharmacy.SaveSaleIssue(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(ddlPatientType.SelectedValue), common.myDate(dtpDocDate.SelectedDate),
                                    common.myInt(ddlStore.SelectedValue), common.myInt(hdnRegistrationId.Value),
                                    common.myInt(hdnEncounterId.Value), common.myStr(txtPatientName.Text.Trim().ToUpper()),
                                    common.myStr(txtAge.Text.Trim()), common.myStr(ddlAgeType.SelectedValue),
                                    common.myStr(ddlGender.SelectedValue), PayerId, doctorId,
                                    common.myStr(Doctorname), "I", "P", common.myInt(hdnIndentId.Value),
                                    common.myInt(hdnAuthorizedId.Value), common.myStr(hdnNarration.Value), 0,
                                    common.myInt(Session["SaveUserId"]), common.myDate(dtpDocDate.SelectedDate),
                                    common.myStr(txtRemark.Text.Trim()), objXML.ToString(), sXMLPaymentMode.ToString(),
                                    common.myDbl(txtReceived.Text), common.myBool(1), intUserId,
                                    strpatienttype, out docNo, 0, out issuedid, txtRegistrationNo.Text, txtEmpNo.Text,
                                    common.myDbl(TotalCoPayAmt), Copercent, Source, common.myDbl(txtRounding.Text),
                                    checkCopayment, txtApprovalCode.Text,
                                    common.myDbl(hdnRoundOffPatient.Value), common.myDbl(hdnRoundOffPayer.Value),
                                    common.myStr(System.Web.HttpContext.Current.Request.UserHostAddress), out btReturnError,
                                    common.myStr(txtMobile.Text), common.myStr(txtaddress.Text), common.myDate(dtpFollowupDate.SelectedDate),
                                    objXMLdesp.ToString(), bCashOutStanding, common.myStr(ddlDiscountType.SelectedValue), common.myStr(txtBLKId.Text), bitValidateDepositExhaust);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                Session["ItemIssueSaleReturnDuplicateCheck"] = 1;
                txtDocNo.Text = "";// docNo;
                hdnIssueId.Value = common.myStr(issuedid);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                if (common.myDbl(txtNetAmount.Text) > 0)
                {
                    lblMessage.Text = strMsg + " With Document No: " + docNo + " And Net Amount: " + Math.Round(common.myDbl(txtNetAmount.Text), 0);
                }
                else
                {
                    lblMessage.Text = strMsg + " With Document No: " + docNo;
                }

                ViewState["Servicetable"] = null;
                clearControl();
                hdnRegistrationId.Value = "0";
                hdnEncounterId.Value = "0";
                hdnAuthorizedId.Value = "0";
                hdnNarration.Value = "";
                lblIndentNo.Text = "";
                TextBox Ftxttotqty = (TextBox)gvService.FooterRow.FindControl("txtTotQty");
                TextBox Ftxttotunit = (TextBox)gvService.FooterRow.FindControl("txtTotUnit");
                TextBox FtxttotSellingprice = (TextBox)gvService.FooterRow.FindControl("txtTotCharge");
                TextBox Ftxttottax = (TextBox)gvService.FooterRow.FindControl("txtTotTax");
                TextBox Ftxttotdiscamt = (TextBox)gvService.FooterRow.FindControl("txtTotDiscount");
                TextBox Ftxttotnetamt = (TextBox)gvService.FooterRow.FindControl("txtTotNetamt");
                TextBox FtxttotPatientamt = (TextBox)gvService.FooterRow.FindControl("txtTotalPatient");
                TextBox Ftxttotpayeramt = (TextBox)gvService.FooterRow.FindControl("txtTotalPayer");

                Ftxttotqty.Text = "";
                FtxttotSellingprice.Text = "";
                Ftxttottax.Text = "";
                Ftxttotdiscamt.Text = "";
                Ftxttotnetamt.Text = "";
                FtxttotPatientamt.Text = "";
                Ftxttotpayeramt.Text = "";
                Session["IsFromRequestFromWard"] = 0;
                Session["ForBLKDiscountPolicy"] = 0;
                gvService.DataSource = CreateTable();
                gvService.DataBind();

                setVisiblilityInteraction();

                ddlDoctor.Visible = true;
                if (btnPrint.Visible)
                {
                    Print();
                }
                //dvLabelPrint.Visible = true;
                //divBarcode.Visible = true;
                Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
                {
                    DropDownList ddlModeType = (DropDownList)gvr.FindControl("ddlMode");
                    DropDownList ddlBank = (DropDownList)gvr.FindControl("ddlBankName");
                    DropDownList ddlCredit = (DropDownList)gvr.FindControl("ddlCreditCard");
                    DropDownList ddlClientBankName = (DropDownList)gvr.FindControl("ddlClientBankName");
                    TextBox txtCheque = (TextBox)gvr.FindControl("txtChequeNo");
                    RadDatePicker txtChqDate = (RadDatePicker)gvr.FindControl("txtChequeDate");
                    TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                    TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                    TextBox txtDesc = (TextBox)gvr.FindControl("txtDescription");
                    TextBox txtChequeNo = (TextBox)gvr.FindControl("txtChequeNo");
                    TextBox txtTransactionRefNo = (TextBox)gvr.FindControl("txtTransactionRefNo");
                    ddlModeType.SelectedIndex = -1;
                    ddlBank.SelectedIndex = -1;
                    ddlCredit.SelectedIndex = -1;
                    ddlClientBankName.SelectedIndex = -1;
                    txtCheque.Text = "";
                    txtChqDate.SelectedDate = System.DateTime.Now;
                    txtAmt.Text = "";
                    txtBal.Text = "";
                    txtDesc.Text = "";
                    txtChequeNo.Text = "";
                    txtTransactionRefNo.Text = "";
                }
                hdnIndentId.Value = "";
                txtIndentNo.Text = "";

                hdnCIMSItemId.Value = "";
                hdnCIMSType.Value = "";
                hdnVIDALItemId.Value = "";

                //btnNew_OnClick(null, null);
                if (txtPatientName.Text == "" && txtAge.Text == "" && common.myInt(hdnRegistrationId.Value) == 0 && common.myInt(hdnEncounterId) == 0)
                {
                    Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
                }

            }
            else
            {
                //callnewpage
                BaseC.HospitalSetup objsec = new BaseC.HospitalSetup(sConString);
                string IsUserAllowAuthentication = common.myStr(objsec.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), "UserAuthenticationForInventory", common.myInt(Session["FacilityId"])));

                if (common.myBool(btReturnError) && IsUserAllowAuthentication == "Y")
                {
                    hdnIsValidPassword.Value = "0";
                    RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/DepositExhaustedAuthentication.aspx?msg=" + strMsg + "&Pass=Y";
                    RadWindowForNew.Height = 120;
                    RadWindowForNew.Width = 340;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = "OnClientIsValidClose";
                    RadWindowForNew.VisibleOnPageLoad = true;
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }

                else if (common.myBool(btReturnError) && IsUserAllowAuthentication != "Y")
                {
                    hdnIsValidPassword.Value = "1";
                    RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/DepositExhaustedAuthentication.aspx?msg=" + strMsg + "&Pass=N";
                    RadWindowForNew.Height = 120;
                    RadWindowForNew.Width = 340;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.OnClientClose = "OnClientIsValidClose";
                    RadWindowForNew.VisibleOnPageLoad = true;
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strMsg;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

    }

    //private bool IsAuthorized()
    //{


    //}

    public bool IsSave()
    {
        bool issave = true;
        string strmsg = "";
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (common.myStr(strpatienttype) == "BO")
        {
            if (common.myStr(txtRegistrationNo.Text) == "")
                issave = false;
            strmsg += "Please select Patient ! ";
        }
        else if (common.myStr(strpatienttype) == "IP-ISS" && common.myInt(ddlDoctor.SelectedValue) == 0)
        {
            issave = false;
            strmsg += "Please select Provider ! ";
        }
        else if (ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE" && txtRegistrationNo.Text == "") //s for Staff 
        {
            issave = false;
            strmsg += "Please Enter Registration Number ! ";
            txtRegistrationNo.Focus();
        }
        else if (ddlPatientType.SelectedItem.Text == "STAFF SALE" && txtEmpNo.Text == "") //s for Staff 
        {
            issave = false;
            strmsg += "Please Enter Employee Number ! ";
            txtEmpNo.Focus();
        }

        else if (common.myStr(txtPatientName.Text) == "")
        {
            issave = false;
            strmsg += "Please Enter Patient Name ! ";
            txtPatientName.Focus();
        }

        else if (common.myStr(txtProvider.Text) == "")
        {
            if (chkDoctor.Checked == true)
            {
                if (hdnIsDoctorMandatoryinOPSale.Value == "Y")
                {
                    issave = false;
                    strmsg += "Please Enter " + Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()) + "Name ! ";
                    txtProvider.Focus();
                }
            }
            else
            {
                if (common.myInt(ddlDoctor.SelectedValue) == 0)
                {
                    if (hdnIsDoctorMandatoryinOPSale.Value == "Y")
                    {
                        issave = false;
                        strmsg += "Please Select " + Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()) + "Name ! ";
                        ddlDoctor.Focus();
                    }
                }
            }
        }

        //else if (common.myStr(ddlDoctor.SelectedValue) == "")
        //{
        //    if (chkDoctor.Checked == false)
        //    {
        //        if (hdnIsDoctorMandatoryinOPSale.Value == "Y")
        //        {
        //            issave = false;
        //            strmsg += "Please Select " + Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()) + "Name ! ";
        //            ddlDoctor.Focus();
        //        }
        //    }
        //}

        if (common.myStr(txtAge.Text) == "")
        {
            if (hdnIsAgeMandatoryinOPSale.Value == "Y")
            {
                issave = false;
                strmsg += "Please Enter Patient Age ! ";
                txtAge.Focus();
            }
        }

        else if (ddlAgeType.SelectedValue == "0")
        {
            if (hdnIsAgeMandatoryinOPSale.Value == "Y")
            {
                issave = false;
                strmsg += "Please Select Patient AgeType Day(s) or Month or Year ! ";
                ddlAgeType.Focus();
            }
        }

        //else if (common.myStr(ddlGender.SelectedValue) == "")
        //{
        //    issave = false;
        //    strmsg += "Please Select Gender! ";
        //    ddlGender.Focus();
        //}
        else if (common.myStr(ddlPayer.SelectedValue) == "")
        {
            issave = false;
            strmsg += "Please Select Payer! ";
            ddlPayer.Focus();
        }




        if (CheckFractionIssue())
        {
            issave = false;
            strmsg += "Decimal value of  Quantity is not allow! ";
        }
        else if (ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE")
        {
            Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
            TextBox txtNetBAmount = (TextBox)paymentMode.FindControl("txtNetBAmount");
            double payment = 0;
            foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
            {
                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                payment = payment + Math.Round(common.myDbl(txtAmt.Text), MidpointRounding.AwayFromZero);

            }
            if (Math.Round(payment, MidpointRounding.AwayFromZero) != Math.Round(common.myDbl(txtReceived.Text), MidpointRounding.AwayFromZero))
            {
                issave = false;
                strmsg += "Amount Collected And Total Invoice Amount Is Not Equal! ";
            }
        }
        if (ddlPatientType.SelectedItem.Text.ToUpper() == "CASH SALE")
        {
            Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
            TextBox txtNetBAmount = (TextBox)paymentMode.FindControl("txtNetBAmount");
            double payment = 0;
            foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
            {
                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                payment = payment + Math.Round(common.myDbl(txtAmt.Text), MidpointRounding.AwayFromZero);

            }
            if (Math.Round(payment, MidpointRounding.AwayFromZero) < Math.Round(common.myDbl(txtReceived.Text), MidpointRounding.AwayFromZero))
            {
                issave = false;
                strmsg += "Received amounnt can not greater than total payment amount ! ";
            }
        }
        objHospitalSetup = new BaseC.HospitalSetup(sConString);//my
                                                               // if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "EnableItemRemarksAlways", common.myInt(Session["FacilityId"]))) == "Y")//my
                                                               // {

        //if(common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsApprovalcodeMandatoryforCreditSale", common.myInt(Session["FacilityId"]))) == "Y")
        //{

        //    foreach (GridViewRow dataItem in gvService.Rows)
        //    {
        //        TextBox txtReusableRemarks = (TextBox)dataItem.FindControl("txtReusableRemarks");
        //        if (txtReusableRemarks.Text == string.Empty)
        //        {
        //            issave = false;
        //            strmsg += "Please enter the item remarks..";
        //            txtReusableRemarks.Focus();
        //        }
        //    }
        //}
        //else 

        if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "MandatoryItemRemarks", common.myInt(Session["FacilityId"]))) == "Y")//f01122016
        {
            foreach (GridViewRow dataItem in gvService.Rows)
            {
                TextBox txtReusableRemarks = (TextBox)dataItem.FindControl("txtReusableRemarks");
                if (txtReusableRemarks.Text == string.Empty)
                {
                    issave = false;
                    strmsg += "Please enter the item remarks..";
                    txtReusableRemarks.Focus();
                }
            }
        }
        // }

        //f08092017b
        if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IPIssueWithoutReqMandateRemarks", common.myInt(Session["FacilityId"]))) == "Y")//f01122016
        {

            if (txtRemark.Text == string.Empty && common.myInt(hdnIndentId.Value) == 0 && ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim() == "IP-ISS")
            {
                issave = false;
                strmsg += "Issue without request,please enter remarks !";
                txtRemark.Focus();
            }

        }
        //f0809207e
        //if (common.myStr(ddlStore.SelectedValue).Equals("0") || common.myStr(ddlStore.SelectedValue).Equals(string.Empty))
        //{
        //    issave = false;
        //    strmsg += " Please select store !";
        //    ddlStore.Focus();
        //}
        lblMessage.Text = "";
        if (strmsg != "")
        {
            lblMessage.Text = strmsg;
        }

        

        return issave;
    }
    private bool CheckFractionIssue()
    {
        bool FractionIssue = false;
        try
        {
            string itemid = "";
            foreach (GridViewRow dataItem in gvService.Rows)
            {
                HiddenField hditemid = (HiddenField)dataItem.FindControl("hdnItemId");
                TextBox txtqty = (TextBox)dataItem.FindControl("txtQty");
                string[] decimalqty = txtqty.Text.Split('.');
                if (decimalqty.Length > 1)
                {
                    if (common.myInt(decimalqty[1]) > 0)
                    {
                        if (itemid == "")
                        {
                            itemid = hditemid.Value;
                        }
                        else
                        {
                            itemid = itemid + "," + hditemid.Value;
                        }
                    }
                }
            }

            if (itemid != "")
            {
                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                if (objPharmacy.getItemFraction(itemid).Tables[0].Rows.Count > 0)
                {
                    FractionIssue = true;
                }
            }

        }
        catch (Exception Ex)
        {
            FractionIssue = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        return FractionIssue;
    }
    protected void btnFindItem_Click(object sender, EventArgs e)
    {

    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        BindPatientDetails();
    }
    void BindPatientDetails()
    {
        try
        {
            lblPackagePatient.Visible = false;
            btnAddNewItem.Visible = true;
            //Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();
            DataSet dsLimit = new DataSet();
            BaseC.ATD objbc = new BaseC.ATD(sConString);
            BaseC.Patient bC = new BaseC.Patient(sConString);
            string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();

            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            isCreditLimitApplicableforOPSale = common.myStr(objBill.getHospitalSetupValue("IsCreditLimitApplicableforOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            IsAllowCashSaleforCreditpatient = common.myStr(objBill.getHospitalSetupValue("IsAllowCashSaleforCreditpatient", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

            ds = new DataSet();
            ViewState["OPIP"] = "O";
            int RegEnc = 0; //0 for Registration,  1 for Encounter and 2 for ER.
            BaseC.clsEMRBilling objVal = new clsEMRBilling(sConString);
            if (strpatienttype == "IP-ISS")
            {

                ViewState["OPIP"] = "I";
                RegEnc = 1;
                #region validation
                if (common.myStr(txtRegistrationNo.Text).Trim().Length > 0
                    && common.myStr(txtEncounter.Text).Trim().Length == 0)
                {
                    DataSet dsR = new DataSet();
                    dsR = objPharmacy.getEncBasedOnRegNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtRegistrationNo.Text));
                    if (dsR.Tables[0].Rows.Count > 0)
                    {
                        txtEncounter.Text = common.myStr(dsR.Tables[0].Rows[0]["EncounterNo"]);
                    }
                    else
                    {
                        lblMessage.Text = "Patient currently not admitted !";
                        txtRegistrationNo.Text = "";
                        return;
                    }
                }

                DataSet dsValid = new DataSet();
                dsValid = objPharmacy.getEncounterStatus(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(txtEncounter.Text));

                if (dsValid.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "Invalid IP No. !";
                    return;
                }

                if (common.myStr(dsValid.Tables[0].Rows[0]["PatientStatus"]) == "C") // Close/Discharge Patient
                {
                    lblMessage.Text = "Patient already discharged !";
                    return;
                }
                if (txtEncounter.Text != "")
                {
                    if (objPharmacy.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                    {
                        lblPackagePatient.Visible = true;
                    }
                    else
                    {
                        lblPackagePatient.Visible = false;
                    }
                }
                #endregion
            }

            if (Request.QueryString["ER"] != null)
            {
                ViewState["OPIP"] = "E";
                RegEnc = 4;
                if (common.myInt(objPharmacy.uspGetEmergencyPatientStatus(common.myInt(Request.QueryString["EncId"]), common.myStr(txtRegistrationNo.Text))) == 1)
                {
                    lblMessage.Text = "Selected emergency patient is Discharged. Do not allow to make medicine issue for this patient...";
                    return;
                }
            }

            if (ddlPatientType.SelectedItem.Text == "CASH SALE" || ddlPatientType.SelectedItem.Text == "CREDIT SALE" || ddlPatientType.SelectedItem.Text == "IPD Issue")
            {

                // passed this condition for ip sale and op sale(cash and credit only)
                ds = objVal.getOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                     common.myStr(ViewState["OPIP"]), common.myInt(hdnRegistrationId.Value), 0,
                                     RegEnc, common.myStr(txtRegistrationNo.Text), common.myStr(txtEncounter.Text == "0" ? "" : txtEncounter.Text),
                                     common.escapeCharString(common.myStr(txtPatientName.Text), false), common.myDate("1950-01-01"), common.myDate("2059-12-31"),
                                     "F", 10, common.myInt(10), common.myInt(Session["UserId"]), 0, "", 0, "", "", "", "", "", "", "", "", "", 0,
                                     string.Empty, string.Empty, string.Empty, 0, 0, 0, false, string.Empty,0);
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    lblMessage.Text = "Invalid " + common.myStr(ViewState["OPIP"]) + "P No. !";
                //    return;
                //}
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["OPPharmacyCoPayPercent"] = common.myStr(ds.Tables[0].Rows[0]["OPPharmacyCoPayPercent"]);
                        ViewState["IPPharmacyCoPayPercent"] = common.myStr(ds.Tables[0].Rows[0]["IPPharmacyCoPayPercent"]);
                        ViewState["PharmacyCreditLimit"] = common.myStr(ds.Tables[0].Rows[0]["PharmacyCreditLimit"]);

                        if (strpatienttype != "IP-ISS")
                        {
                            ViewState["OPPharmacyCoPayMaxLimit"] = common.myStr(ds.Tables[0].Rows[0]["OPPharmacyCoPayMaxLimit"]);
                        }
                        hdnRegistrationId.Value = ds.Tables[0].Rows[0]["REGID"].ToString().Trim();
                        txtRegistrationNo.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();

                        if (strpatienttype == "IP-ISS")
                        {
                            //ddlDoctor.SelectedValue = ds.Tables[0].Rows[0]["DoctorId"].ToString().Trim();
                            ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["DoctorId"])));
                        }
                        if (strpatienttype == "OP-ISS")
                        {
                            if (common.myStr(ViewState["OPIP"]) == "E")
                            {


                                //ddlDoctor.SelectedValue = common.myStr(objPharmacy.uspGetEmergencyDoctor(common.myInt(Request.QueryString["EncId"]), common.myStr(txtRegistrationNo.Text)));
                                ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(common.myStr(objPharmacy.uspGetEmergencyDoctor(common.myInt(Request.QueryString["EncId"]), common.myStr(txtRegistrationNo.Text)))));
                            }
                            else
                            {
                                BindPreviousDoctor();
                            }
                        }

                        if (common.myStr(ds.Tables[0].Rows[0]["ENCID"]) != "")
                        {
                            if (common.myInt(ds.Tables[0].Rows[0]["ENCID"]) != 0)
                            {
                                hdnEncounterId.Value = ds.Tables[0].Rows[0]["ENCID"].ToString().Trim();
                            }
                        }
                        txtEncounter.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();

                        txtPatientName.Text = ds.Tables[0].Rows[0]["Name"].ToString().Trim();
                        txtPatientName.ToolTip = ds.Tables[0].Rows[0]["Name"].ToString().Trim();
                        hdnPaymentType.Value = ds.Tables[0].Rows[0]["PaymentType"].ToString().Trim();

                        txtMobile.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString().Trim();
                        txtMobile.ToolTip = ds.Tables[0].Rows[0]["MobileNo"].ToString().Trim();

                        txtaddress.Text = ds.Tables[0].Rows[0]["PatientAddress"].ToString().Trim();
                        txtaddress.ToolTip = ds.Tables[0].Rows[0]["PatientAddress"].ToString().Trim();

                        lblWardName.Text = ds.Tables[0].Rows[0]["CurrentWard"].ToString().Trim();
                        lblBedNo1.Text = ds.Tables[0].Rows[0]["CurrentBedNo"].ToString().Trim();

                        if (common.myStr(ds.Tables[0].Rows[0]["CompanyCode"].ToString()) != "0" && common.myStr(ds.Tables[0].Rows[0]["CompanyCode"].ToString()) != "")
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim())));
                            //if (strpatienttype == "IP-ISS")
                            ddlPayer.Enabled = false;
                            //else
                            //    ddlPayer.Enabled = false;
                        }
                        else
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                        }
                        lblPayer.Text = ddlPayer.SelectedItem.Text;
                        if (strpatienttype == "IP-ISS")
                        {
                            if (common.myBool(ds.Tables[0].Rows[0]["IsColorCodingRequired"]) == true)
                            {
                                lblPayer.BackColor = System.Drawing.Color.Yellow;
                            }
                            else
                            {
                                lblPayer.BackColor = System.Drawing.Color.Transparent;
                            }
                        }
                        txtBedNo.Text = ds.Tables[0].Rows[0]["CurrentBedNo"].ToString().Trim();

                        if (ds.Tables[0].Rows[0]["GenderAge"].ToString().Trim() != "")
                        {
                            txtAge.Text = common.myStr(ds.Tables[0].Rows[0]["GenderAge"]);
                            string[] agetype = common.myStr(ds.Tables[0].Rows[0]["GenderAge"].ToString().Trim()).Split(' ');
                            if (agetype[1] != "")
                            {
                                ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                            }
                            txtAge.Text = agetype[0];
                        }
                        if (common.myStr(ds.Tables[0].Rows[0]["Gender"].ToString()) != "")
                        {
                            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["Gender"].ToString().Trim())));
                        }
                        if (ddlPatientType.SelectedItem.Text == "CASH SALE")
                        {
                            txtReceived.ReadOnly = false;
                        }
                        else
                        {
                            txtReceived.ReadOnly = true;
                        }

                        if (isCreditLimitApplicableforOPSale == "Y")
                        {
                            if (ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE" && Request.QueryString["Code"] != "IPI")
                            {
                                dsLimit = objPharmacy.getPhrCreditLimit(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(hdnRegistrationId.Value), common.myInt(ddlPayer.SelectedValue));
                                if (dsLimit.Tables.Count > 0)
                                {
                                    if (dsLimit.Tables[0].Rows.Count > 0)
                                    {
                                        lblTreatLimit.Text = common.myDbl(dsLimit.Tables[0].Rows[0]["TreatmentLimit"]).ToString("F", CultureInfo.InvariantCulture);
                                        lblAvailLimit.Text = "0.00";
                                        if (common.myDbl(lblTreatLimit.Text) > 0)
                                        {
                                            lblAvailLimit.Text = (common.myDbl(lblTreatLimit.Text) - common.myDbl(dsLimit.Tables[0].Rows[0]["AvailableLimit"])).ToString("F", CultureInfo.InvariantCulture);
                                        }
                                    }
                                    else
                                    {
                                        lblTreatLimit.Text = "0.00";
                                        lblAvailLimit.Text = "0.00";
                                    }
                                }
                            }
                            else
                            {
                                lblTreatLimit.Text = "0.00";
                                lblAvailLimit.Text = "0.00";
                            }
                        }
                    }
                    else
                    {
                        if (ViewState["OPIP"] == "E")
                        {
                            Alert.ShowAjaxMsg("Patient Not Found, Please select Emergency patient", Page);

                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Patient Not Found", Page);

                        }
                    }
                }
            }
            else // STAFF SALE 
            {
                txtReceived.ReadOnly = true;
                Patient objpatient = new Patient(sConString);
                ds = objpatient.GetEmployeeTagging(common.myInt(Session["HospitalLocationId"]), common.myStr(hdnEmployeeId.Value));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtEmpNo.Text = common.myStr(ds.Tables[0].Rows[0]["EmployeeNo"]);
                        txtPatientName.Text = common.myStr(ds.Tables[0].Rows[0]["EmpName"]);
                        txtPatientName.ToolTip = common.myStr(ds.Tables[0].Rows[0]["EmpName"]);
                        if (ds.Tables[0].Rows[0]["GenderAge"].ToString().Trim() != "")
                        {
                            txtAge.Text = common.myStr(ds.Tables[0].Rows[0]["GenderAge"]);
                            string[] agetype = common.myStr(ds.Tables[0].Rows[0]["GenderAge"].ToString().Trim()).Split(' ');
                            if (agetype[1] != "")
                            {
                                ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                            }
                            txtAge.Text = agetype[0];
                        }
                        if (common.myStr(ds.Tables[0].Rows[0]["Gender"].ToString()) != "")
                        {
                            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["Gender"].ToString().Trim())));
                        }


                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Employee is not available ! ";


                    }
                }
            }

            ShowCashCredit();
            txtRegistrationNo.Enabled = false;
            txtEncounter.Enabled = false;
            txtPatientName.Enabled = false;
            txtAge.Enabled = false;
            hdnERCompanyId.Value = "1";
            if (common.myStr(txtIndentNo.Text) == "")
            {
                if (ddlPatientType.SelectedItem.Text.ToUpper() == "CASH SALE" && lblPayertype.Text.ToUpper() == "CREDIT")
                {
                    ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                    lblPayertype.Text = "Cash";
                    hdnPaymentType.Value = "C";
                }
                else if (ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE" && lblPayertype.Text.ToUpper() == "CASH")
                {
                    //f31082017b
                    IsAllowChangeCompanyInOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsAllowChangeCompanyInOPCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (IsAllowChangeCompanyInOPCreditSale == "Y" && common.myStr(Request.QueryString["Code"]) == "CSI" && common.myStr(Request.QueryString["ER"]) != "True")
                    {
                        btnAddNewItem.Visible = true;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Patient Type Cash, Credit Sale not allowed ! ";
                        btnAddNewItem.Visible = false;
                    }
                    //f31082017e

                    //f31082017b comment
                    //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //lblMessage.Text = "Patient Type Cash, Credit Sale not allowed ! ";
                    //btnAddNewItem.Visible = false;
                    //f31082017e comment
                }
                else if (ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE" && lblPayertype.Text.ToUpper() == "CREDIT")
                {
                    //ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                    lblPayertype.Text = "CREDIT";
                    hdnPaymentType.Value = "B";
                }
                if (common.myStr(ViewState["OPIP"]) == "E")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnERCompanyId.Value = common.myStr(ds.Tables[0].Rows[0]["CompanyCode"]);
                    }
                }
            }
            BindMessage();
            ShowPatientDetails();
            BindPatientProvisionalDiagnosis();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void BindPreviousDoctor()
    {
        try
        {
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();
            ds = objPharmacy.GetLastPrescribedDocter(common.myInt(hdnRegistrationId.Value));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDoctor.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void BindPreviousData()
    {
        try
        {
            DataTable tbl = new DataTable();
            //if (btnAddNewItem.Visible)
            //{
            //f11072017bcomment
            //if (ViewState["RequestFromWardItems"] != null)
            //{
            //    tbl = (DataTable)ViewState["RequestFromWardItems"];
            //}
            //else if (ViewState["Servicetable"] != null)
            //{
            //    tbl = (DataTable)ViewState["Servicetable"];
            //}
            //f11072017ecomment
            //f11072017bwrite
            if (ViewState["Servicetable"] != null)
            {
                tbl = (DataTable)ViewState["Servicetable"];
            }
            else if (ViewState["RequestFromWardItems"] != null)
            {
                tbl = (DataTable)ViewState["RequestFromWardItems"];
            }

            //f11072017ewrite

            //if (ViewState["Servicetable"] != null)
            //{
            //    tbl = (DataTable)ViewState["Servicetable"];
            //}

            foreach (GridViewRow dataItem in gvService.Rows)
            {
                if (common.myInt(((HiddenField)dataItem.FindControl("hdnItemId")).Value) > 0)
                {
                    DataRow DR;
                    DR = tbl.Rows[dataItem.RowIndex];
                    TextBox txtQty = (TextBox)dataItem.FindControl("txtQty");
                    TextBox txtCharge = (TextBox)dataItem.FindControl("txtCharge");
                    TextBox txtDiscountAmt = (TextBox)dataItem.FindControl("txtDiscountAmt");
                    TextBox txtTax = (TextBox)dataItem.FindControl("txtTax");
                    TextBox txtNetAmt = (TextBox)dataItem.FindControl("txtNetAmt");
                    TextBox txtPatient = (TextBox)dataItem.FindControl("txtPatient");
                    TextBox txtPayer = (TextBox)dataItem.FindControl("txtPayer");

                    HiddenField hdnReusable = (HiddenField)dataItem.FindControl("hdnReusable");

                    DR["Qty"] = txtQty.Text;
                    if (common.myInt(hdnDiscount.Value) != 0)
                    {
                        DR["PercentDiscount"] = common.myDbl(hdnDiscount.Value);
                    }
                    else
                    {
                        DR["PercentDiscount"] = common.myDbl(((HiddenField)dataItem.FindControl("hdnPercentDiscount")).Value);
                    }
                    //DR["PercentDiscount"] = common.myDbl(hdnDiscount.Value);
                    DR["DiscAmt"] = common.myDbl(txtDiscountAmt.Text);
                    DR["NetAmt"] = common.myDbl(txtNetAmt.Text);
                    DR["PatientAmount"] = common.myDbl(txtPatient.Text);
                    DR["PayerAmount"] = common.myDbl(txtPayer.Text);

                    //DR["Reusable"] = DBNull.Value; 
                    //DR["Reusable"] =0;
                    DR["Reusable"] = common.myStr(hdnReusable.Value);
                    // DR["Reusable"] = "";

                    tbl.AcceptChanges();

                    hdnTotQty.Value = "0";
                    hdnTotUnit.Value = "0";
                    hdnTotCharge.Value = "0";
                    hdnTotTax.Value = "0";
                    hdnTotDiscAmt.Value = "0";
                    hdnTotPatientAmt.Value = "0";
                    hdnTotPayerAmt.Value = "0";
                    hdnTotNetAmt.Value = "0";

                    if (btnAddNewItem.Visible)
                    {
                        ViewState["Servicetable"] = tbl;
                    }
                    else
                    {
                        ViewState["RequestFromWardItems"] = tbl;
                    }
                    Session["IsFromRequestFromWard"] = 0;
                    Session["ForBLKDiscountPolicy"] = 0;
                    gvService.DataSource = tbl;
                    gvService.DataBind();

                    //setVisiblilityInteraction();
                    if (common.myBool(ViewState["IsCIMSInterfaceActive"]) || common.myBool(ViewState["IsVIDALInterfaceActive"]))
                    {
                        setVisiblilityInteraction();
                    }
                }
            }
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btndDiscount_OnClick(object Sender, EventArgs e)
    {
        try
        {
            BindPreviousData();
            foreach (GridViewRow dataItem in gvService.Rows)
            {
                TextBox txtQty = (TextBox)dataItem.FindControl("txtQty");
                if (common.myDbl(txtQty.Text) <= 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Item ! ";
                    return;
                }
            }

            Session.Remove("OPSaleServicetable");
            Session["OPSaleServicetable"] = ViewState["Servicetable"];

            lblMessage.Text = "";
            RadWindow1.NavigateUrl = "~/Pharmacy/SaleIssue/IssueDiscount.aspx?DiscAmt=" + hdnDiscount.Value + "&AuthoId=" + hdnAuthorizedId.Value + "&Narration=" + hdnNarration.Value;
            RadWindow1.Height = 400;
            RadWindow1.Width = 700;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "wndDiscount_OnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnAddNewItem_OnClick(object Sender, EventArgs e)
    {
        try
        {
            //string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
            if (common.myStr(txtPatientName.Text) != "")
            {
                if (gvService.Rows.Count > 0)
                {
                    BindPreviousData();
                }
                // Rahul's Change 
                String Code = string.Empty;
                string registrationid = string.Empty;
                if (Request.QueryString["Code"] != null && Request.QueryString["Code"] != "")
                {
                    Code = common.myStr(Request.QueryString["Code"]);
                    registrationid = common.myStr(hdnRegistrationId.Value);
                }

                lblMessage.Text = "";
                if (txtEncounter.Text == "")
                {
                    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItem.aspx?SetupId=" + ddlPatientType.SelectedValue + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&TrnId=" + ddlPatientType.SelectedValue + "&IssueType=" + Code + "&RegId=" + registrationid + "&StoreId=" + common.myStr(ddlStore.SelectedValue);
                }
                else
                {
                    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItem.aspx?SetupId=" + ddlPatientType.SelectedValue + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&TrnId=" + ddlPatientType.SelectedValue + "&EncounterNo=" + common.myStr(txtEncounter.Text) + "&IssueType=" + Code + "&RegId=" + registrationid + "&StoreId=" + common.myStr(ddlStore.SelectedValue);
                }
                RadWindow1.Height = 590;
                RadWindow1.Width = 990;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "wndAddService_OnClientClose";
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient or Enter Patient Name ! ";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    #endregion
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
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void setGridColor()
    {
        // if (common.myBool(Application["IsCIMSInterfaceActive"])
        //   || common.myBool(Application["IsVIDALInterfaceActive"]))
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet dsFlag = new DataSet();
        if (common.myBool(ViewState["IsCIMSInterfaceActive"])
              || common.myBool(ViewState["IsVIDALInterfaceActive"]))
        {

            if (gvService == null)
            {
                return;
            }

            if (gvService.Rows.Count == 0)
            {
                return;
            }

            foreach (GridViewRow dataItem in gvService.Rows)
            {
                // if (common.myBool(Application["IsCIMSInterfaceActive"]))
                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                {
                    LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");

                    //lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                }
                //else if (common.myBool(Application["IsVIDALInterfaceActive"]))
                else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
                {
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)dataItem.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionVIDAL");

                    //lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                }

                HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                //Label lblLASA = (Label)dataItem.FindControl("lblLASA_Grid");
                //Label lblHighRisk = (Label)dataItem.FindControl("lblHR_Grid");
                dsFlag = objPharmacy.GetItemRequiredAlertColor(common.myInt(hdnItemId.Value), "P");
                if (dsFlag.Tables.Count > 0)
                {
                    if (dsFlag.Tables[0].Rows.Count > 0)
                    {
                        for (int rIdx = 0; rIdx < dsFlag.Tables[0].Rows.Count; rIdx++)
                        {
                            if (common.myStr(dsFlag.Tables[0].Rows[rIdx]["Code"]) == "LASA")
                            {
                                dataItem.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                                //lblLASA.BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                            }
                            if (common.myStr(dsFlag.Tables[0].Rows[rIdx]["Code"]) == "HighRisk")
                            {
                                dataItem.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                                //lblHighRisk.BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                            }
                        }

                    }
                }
            }
        }
        else
        {
            if (gvService == null)
            {
                return;
            }

            if (gvService.Rows.Count == 0)
            {
                return;
            }

            foreach (GridViewRow dataItem in gvService.Rows)
            {
                HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
                //Label lblLASA = (Label)dataItem.FindControl("lblLASA_Grid");
                /*Label lblHighRisk = (Label)dataItem.FindControl("lblHR_Grid")*/
                ;
                dsFlag = objPharmacy.GetItemRequiredAlertColor(common.myInt(hdnItemId.Value), "P");
                if (dsFlag.Tables.Count > 0)
                {
                    if (dsFlag.Tables[0].Rows.Count > 0)
                    {
                        for (int rIdx = 0; rIdx < dsFlag.Tables[0].Rows.Count; rIdx++)
                        {
                            if (common.myStr(dsFlag.Tables[0].Rows[rIdx]["Code"]) == "LASA")
                            {
                                dataItem.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                                //lblLASA.BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                            }
                            if (common.myStr(dsFlag.Tables[0].Rows[rIdx]["Code"]) == "HighRisk")
                            {
                                dataItem.Cells[1].BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                                //lblHighRisk.BackColor = System.Drawing.Color.FromName(common.myStr(dsFlag.Tables[0].Rows[rIdx]["Color"]));
                            }
                        }

                    }
                }
            }
        }
    }
    private void clearControl()
    {
        try
        {
            //Session["SaveDuplicate"] = "0";
            hdnTotCharge.Value = "0";
            hdnTotDiscAmt.Value = "0";
            hdnTotPatientAmt.Value = "0";
            hdnTotPayerAmt.Value = "0";
            hdnTotNetAmt.Value = "0";
            hdnTotQty.Value = "0";
            hdnTotUnit.Value = "0";
            hdnTotTax.Value = "0";
            txtProvider.Text = "";
            txtRegistrationNo.Text = "";
            txtEmpNo.Text = "";
            txtEncounter.Text = "";
            txtBedNo.Text = "";
            txtPatientName.Text = "";
            txtPatientName.ToolTip = "";
            txtAge.Text = "";
            txtNetAmount.Text = "0.00";
            txtReceived.Text = "0.00";
            txtAmountCollected.Text = "0.00";
            txtRefundamount.Text = "0.00";
            hdnDiscount.Value = "";
            hdnAuthorizedId.Value = "";
            hdnNarration.Value = "";
            txtRemark.Text = "";
            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
            lblPayer.Text = "";
            //ddlAgeType.SelectedValue = "0";
            ddlGender.SelectedIndex = 0;
            txtRegistrationNo.Enabled = true;
            txtEncounter.Enabled = true;

            lblTreatLimit.Text = "0.00";
            lblAvailLimit.Text = "0.00";

            if (common.myStr(Session["ModuleFlag"]) == "ER")
            {
                txtPatientName.Enabled = false;
            }
            else
            {
                txtPatientName.Enabled = true;
            }
            if (Request.QueryString["ER"] != null)
            {
                btnViewOrders.Visible = true;
               // lnkPatientHistory.Visible = false;
            }
            else
            {
                btnViewOrders.Visible = false;
                //lnkPatientHistory.Visible = true;
            }
            txtAge.Enabled = true;
            chkDoctor.Checked = false;
            ddlDoctor.Text = "";
            txtProvider.Visible = false;
            // ddlDoctor.SelectedIndex = 0;
            BindDoctor();
            ViewState["Servicetable"] = "";
            BindgvItemSevice();
            txtReceived.ReadOnly = true;
            chkCashOutStanding.Checked = false;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void ddlPatientType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            //Added on 29-09-2014 Start Nausshad Ali-New Condtion           

            if (ViewState["PaymentType"] != null && lblPayertype.Text != "Cash")
            {

                hdnPaymentType.Value = "C";
                lblPayertype.Text = "Cash";
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                lblPayer.Text = ddlPayer.SelectedItem.Text;
                ViewState["CompanyCodeFromPrescription"] = null;

            }
            else
            {
                hdnSaleType.Value = ddlPatientType.SelectedItem.Text;
                lblMessage.Text = "";
                BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                hdnDefaultCompanyId.Value = common.myInt(objBill.getDefaultCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))).ToString();
                IsApprovalCodeMandatoryforOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsApprovalcodeMandatoryforCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

                clearControl();
                chkDoctor.Enabled = true;
                Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                //done by rakesh for user authorisation start
                //btnSaveData.Visible = true;
                SetPermission(btnSaveData, "N", true);
                //done by rakesh for user authorisation end
                txtDocNo.Text = "";
                DataSet ds = new DataSet();

                BaseC.clsPharmacy objPharmacy = new clsPharmacy(sConString);


                ds = objPharmacy.GetSaleSetupMaster(common.myInt(ddlPatientType.SelectedValue), common.myInt(Session["HospitalLocationID"]), 1, common.myInt(Session["UserID"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnPriceType.Value = common.myStr(ds.Tables[0].Rows[0].ItemArray[5]);
                        hdnDiscountPerc.Value = common.myStr(ds.Tables[0].Rows[0].ItemArray[6]);
                        hdnServiceTaxPerc.Value = common.myStr(ds.Tables[0].Rows[0].ItemArray[7]);
                        hdnIsReceiptAllow.Value = common.myStr(ds.Tables[0].Rows[0].ItemArray[9]);
                    }
                    string strpatienttype = common.myStr(ddlPatientType.SelectedItem.Attributes["StatusCode"]).Trim();
                    ViewState["saletype"] = strpatienttype;
                    txtpatientaatributes.Text = strpatienttype;
                    txtPatientName.Enabled = false;
                    lblShowStar.Visible = true;
                    grdPaymentMode.Enabled = true;
                    switch (strpatienttype)
                    {
                        case "OP-ISS":

                            txtRegistrationNo.Focus();
                            Page.Title = "Item Sale";
                            lblHeader.Text = "Item Sale";
                            lblHeader.ToolTip = "Item Sale";


                            txtRegistrationNo.Text = "";
                            Panel1.Visible = true;
                            Panel2.Visible = true;
                            tdPdetail.Visible = true;
                            lbtnSearchPatient.Visible = true;
                            //btnFindPatient.Visible = true;
                            tdPname.Visible = true;
                            trCreditDetail.Visible = true;
                            lblPayor.Visible = true;
                            lblEncNo.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "EncounterNo").ToString());

                            ddlDoctor.Visible = true;
                            txtReceived.ReadOnly = true;

                            if (IsApprovalCodeMandatoryforOPCreditSale == "Y")
                            {
                                if ((ddlPatientType.Text.ToUpper() == "CREDIT SALE"))
                                {
                                    lblApprovalCodeS.Visible = true;
                                    lblRemarkS.Visible = true;
                                }
                                else
                                {
                                    lblApprovalCodeS.Visible = false;
                                    lblRemarkS.Visible = false;
                                }
                            }


                            if (ddlPatientType.SelectedItem.Text.ToUpper() == "CASH SALE" || ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE")
                            {
                                lblEncNo.Visible = true;
                                txtEncounter.Visible = true;
                                txtRegistrationNo.Visible = true;
                                txtEmpNo.Visible = false;
                                txtEmpNo.Text = "";

                                lbtnSearchPatient.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Regno").ToString());

                                if (ddlPatientType.SelectedItem.Text == "CREDIT SALE")
                                {


                                    ddlPayer.Enabled = false;
                                    txtReceived.ReadOnly = true;
                                    if (ViewState["CompanyCodeFromPrescription"] != null)
                                    {
                                        ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(ViewState["CompanyCodeFromPrescription"])));
                                    }
                                    //else
                                    //{
                                    //    ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                                    //}
                                    lblPayer.Text = ddlPayer.SelectedItem.Text;

                                    //f31082017b
                                    IsAllowChangeCompanyInOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsAllowChangeCompanyInOPCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                                    if (IsAllowChangeCompanyInOPCreditSale == "Y" && common.myStr(Request.QueryString["Code"]) == "CSI" && common.myStr(Request.QueryString["ER"]) != "True")
                                    {
                                        if (txtIndentNo.Text == "")//for prescription
                                        {
                                            ddlPayer.Visible = true;
                                            ddlPayer.Enabled = true;
                                        }
                                        else
                                        {
                                            ddlPayer.Visible = true;
                                            ddlPayer.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        ddlPayer.Visible = false;
                                        ddlPayer.Enabled = false;
                                    }

                                    //f31082017e
                                }
                                else
                                {
                                    ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                                    lblPayer.Text = ddlPayer.SelectedItem.Text;
                                    grdPaymentMode.Enabled = true;
                                    ddlPayer.Enabled = false;
                                    if (common.myStr(Session["ModuleFlag"]) != "ER")
                                    {
                                        txtPatientName.Enabled = true;
                                    }
                                    lblShowStar.Visible = false;
                                    txtReceived.ReadOnly = false;
                                }
                                ComboEmployeeSearch.Visible = false;
                                lblEmployee.Visible = false;
                            }
                            else if (ddlPatientType.SelectedItem.Text.ToUpper() == "STAFF SALE") //s for Staff 
                            {
                                txtReceived.ReadOnly = true;
                                txtEmpNo.Focus();
                                txtEmpNo.Focus();
                                lbtnSearchPatient.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "employeeno").ToString());
                                lblEncNo.Visible = false;
                                txtEncounter.Visible = false;
                                txtRegistrationNo.Visible = false;
                                txtEmpNo.Visible = true;
                                txtRegistrationNo.Text = "";
                                ddlPayer.SelectedValue = common.myStr(ViewState["StaffCompanyId"]); //common.myStr(Cache["StaffCompanyId"]);
                                hdnDefaultCompanyId.Value = common.myStr(ViewState["StaffCompanyId"]);
                                lblPayer.Text = ddlPayer.SelectedItem.Text;
                                ddlPayer.Enabled = false;
                                hdnRegistrationId.Value = "0";
                                //  btnFindPatient.Visible = false;
                                ComboEmployeeSearch.Visible = true;
                                lblEmployee.Visible = true;
                            }

                            break;

                        case "IP-ISS":
                            txtRegistrationNo.Focus();
                            Page.Title = "IPD Issue Sale";
                            lblHeader.Text = "IPD Issue";
                            lblHeader.ToolTip = "IPD Issue Sale";




                            txtRegistrationNo.Text = "";
                            lblEncNo.Text = Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString());

                            Panel1.Visible = true;
                            Panel2.Visible = true;
                            tdPdetail.Visible = true;
                            trCreditDetail.Visible = true;
                            lblPayor.Visible = true;
                            lbtnSearchPatient.Visible = true;
                            // btnFindPatient.Visible = true;
                            tdPname.Visible = true;
                            //done by rakesh for user authorisation start
                            //btnPrint.Visible = false;
                            SetPermission(btnPrint, false);
                            //done by rakesh for user authorisation end
                            lblPayer.Text = ddlPayer.SelectedItem.Text;

                            break;

                        default:
                            break;
                    }
                    ShowCashCredit();
                    ddlPayer_OnSelectedIndexChanged(sender, e);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    void InsurancecompanyBind(string sType, Int32 iRegID)
    {
        try
        {
            DataSet ds = new DataSet();
            string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
            ddlPayer.Items.Clear();
            BaseC.clsPharmacy objPharmacy = new clsPharmacy(sConString);
            ds = objPharmacy.GetAllCompany(common.myInt(Session["HospitalLocationID"]));
            ddlPayer.DataSource = ds;
            ddlPayer.DataTextField = "Name";
            ddlPayer.DataValueField = "CompanyId";
            ddlPayer.DataBind();
            ddlPayer.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlPayer.SelectedIndex = 0;
            ViewState["CompanyList"] = ds;
            if (strpatienttype == "OP-ISS")
            {
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                lblPayer.Text = ddlPayer.SelectedItem.Text;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void gvService_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        objHospitalSetup = new BaseC.HospitalSetup(sConString);//my
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        double @dblMarginPerc = 0.0;
        string @strQtyEditable = "N";
        @dblMarginPerc = common.myDbl(objPhr.getHospitalSetupValue("MarginPercforBLKDiscount", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
        @strQtyEditable = common.myStr(objPhr.getHospitalSetupValue("IsSaleQtyEditable", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

        if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "EnableItemRemarksAlways", common.myInt(Session["FacilityId"]))) == "Y")//my
        {

        }
        else
        {
            gvService.Columns[19].HeaderText = "Reusable Remarks";
        }

        DataTable mytbl = new DataTable();//my
        DataSet dsXml = new DataSet();
        DataSet ds1 = new DataSet();
        StringReader sr;
        DataView dvBatchNo = new DataView();
        DataTable dtBatchNo = new DataTable();
        DataView dvNonZeroBatch = new DataView();
        string xmlSchema = "";
        string sBatchNo = "";
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#5EA0F4";
                }

                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                Label lblRequiredQty = (Label)e.Row.FindControl("lblRequiredQty");
                HiddenField hdnGenericId = (HiddenField)e.Row.FindControl("hdnGenericId");
                TextBox txtReusableRemarks = (TextBox)e.Row.FindControl("txtReusableRemarks");//my
                if (common.myInt(hdnGenericId.Value) > 0)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (cell == e.Row.Cells[1])
                        {
                            cell.BackColor = System.Drawing.Color.Bisque;
                        }
                    }
                }

                lblRequiredQty.Text = common.myDbl(lblRequiredQty.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                gvService.Columns[(byte)GridService.RequiredQty].Visible = !btnAddNewItem.Visible;

                txtQty.Enabled = btnAddNewItem.Visible;

                if (common.myStr(ViewState["RequestFromWard"]) != "")
                {
                    txtQty.Enabled = true;
                }

                if (@strQtyEditable == "Y")
                {
                    txtQty.Enabled = true;
                    txtQty.ReadOnly = false;
                }

                if (common.myStr(hdnSaveIssueId.Value) != "" || common.myStr(txtDocNo.Text) != "")
                {
                    gvService.Columns[(byte)GridService.Delete].Visible = false; //12
                    //e.Row.Cells[(byte)GridService.Delete].Visible = true; //12
                    ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                    ibtndaDelete.Visible = false;
                    //txtQty.Enabled = false;
                }
                else
                {
                    gvService.Columns[(byte)GridService.Delete].Visible = true; //12
                    //e.Row.Cells[(byte)GridService.Delete].Visible = true; //12
                    ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
                    ibtndaDelete.Visible = true;
                }

                Label lblServiceName = (Label)e.Row.FindControl("lblServiceName");
                //            Label lblUnit = (Label)e.Row.FindControl("lblUnit");

                HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                Label lblItemUnitName = (Label)e.Row.FindControl("lblItemUnitName");

                TextBox txtCharge = (TextBox)e.Row.FindControl("txtCharge");
                HiddenField hdnPercentDiscount = (HiddenField)e.Row.FindControl("hdnPercentDiscount");
                TextBox txtDiscountAmt = (TextBox)e.Row.FindControl("txtDiscountAmt");
                TextBox txtPatient = (TextBox)e.Row.FindControl("txtPatient");
                TextBox txtPayer = (TextBox)e.Row.FindControl("txtPayer");

                TextBox txtTax = (TextBox)e.Row.FindControl("txtTax");
                TextBox txtNetAmt = (TextBox)e.Row.FindControl("txtNetAmt");
                HiddenField hdnStockQty = (HiddenField)e.Row.FindControl("hdnStockQty");
                HiddenField hdnCostPrice = (HiddenField)e.Row.FindControl("hdnCostPrice");

                DropDownList ddlDoctor = (DropDownList)e.Row.FindControl("ddlDoctor");
                HiddenField hdnDetailId = (HiddenField)e.Row.FindControl("hdnDetailId");
                HiddenField hdnServiceId = (HiddenField)e.Row.FindControl("hdnServiceId");
                HiddenField hdnUPack = (HiddenField)e.Row.FindControl("hdnUnderPack");
                HiddenField hdnPackage = (HiddenField)e.Row.FindControl("hdnPackageId");
                HiddenField hdnServType = (HiddenField)e.Row.FindControl("hdnServiceType");
                HiddenField hdnDeptId = (HiddenField)e.Row.FindControl("hdnDeptId");
                HiddenField hdnDocReq = (HiddenField)e.Row.FindControl("hdnDocReq");
                HiddenField hdnDoctorID = (HiddenField)e.Row.FindControl("hdnDocReq");
                HiddenField hdnCopayPerc = (HiddenField)e.Row.FindControl("hdnCopayPerc");

                HiddenField hdnBatchXML = (HiddenField)e.Row.FindControl("hdnBatchXML");
                HiddenField hdnBatchId = (HiddenField)e.Row.FindControl("hdnBatchId");
                Label lblBatchNo = (Label)e.Row.FindControl("lblBatchNo");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnDenialCode = (HiddenField)e.Row.FindControl("hdnDenialCode");
                HiddenField hdnPrescriptionDetailsId = (HiddenField)e.Row.FindControl("hdnPrescriptionDetailsId");
                HiddenField hdnISDHAApproved = (HiddenField)e.Row.FindControl("hdnISDHAApproved");
                HiddenField hdnReusable = (HiddenField)e.Row.FindControl("hdnReusable");

                if (common.myInt(Session["ForBLKDiscountPolicy"]) == 1)
                {
                    ds1 = objPhr.GetItemCategoryDetails(common.myInt(hdnItemId.Value));
                    double ItemValue = 0.0;
                    double marginperc = 0.0;
                    double PatientAmountLimit = common.myDbl(objPhr.getHospitalSetupValue("OPSalePatientDiscountAmountLimit", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

                    ItemValue = common.myDbl(txtCharge.Text) - common.myDbl(hdnCostPrice.Value);
                    marginperc = (ItemValue * 100) / common.myDbl(txtCharge.Text);

                    if (ddlDiscountType.SelectedValue == "SD")
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsPharmaceutical"]) == 0)
                            {
                                e.Row.Cells[8].BackColor = System.Drawing.Color.OrangeRed;
                            }
                            if (marginperc < @dblMarginPerc)
                            {
                                e.Row.Cells[8].BackColor = System.Drawing.Color.OrangeRed;
                            }
                        }
                    }
                    if (ddlDiscountType.SelectedValue == "PD")
                    {
                        if (marginperc < @dblMarginPerc)
                        {
                            e.Row.Cells[8].BackColor = System.Drawing.Color.OrangeRed;
                        }
                    }

                }

                if (common.myStr(hdnPrescriptionDetailsId.Value) != "" || ViewState["RequestFromWardItems"] != null)
                {
                    txtQty.Enabled = true;
                    objHospitalSetup = new BaseC.HospitalSetup(sConString);//my
                    if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "EnableItemRemarksAlways", common.myInt(Session["FacilityId"]))) == "Y")//my
                    {
                        txtReusableRemarks.Text = string.Empty;
                        txtReusableRemarks.Enabled = true;
                    }
                    else
                    {
                        //my b 07112016 
                        DataTable dtRe = (DataTable)ViewState["RequestFromWardItems"];

                        if (common.myStr(hdnReusable.Value) == common.myStr(1))
                        {
                            txtReusableRemarks.Text = string.Empty;
                            txtReusableRemarks.Enabled = true;
                        }
                        else
                        {
                            txtReusableRemarks.Text = string.Empty;
                            txtReusableRemarks.Enabled = false;
                        }
                        //my e 07112016 
                    }
                }

                Label lblExpiryDate = (Label)e.Row.FindControl("lblExpiryDate");

                //if (common.myStr(lblBatchNo.Text) != "")
                //{
                //    e.Row.Cells[2].BackColor = System.Drawing.Color.Green;
                //}
                //if (common.myStr(hdnISDHAApproved.Value) == "0")
                //    e.Row.BackColor = System.Drawing.Color.LightCoral;
                e.Row.ToolTip = "";


                #region Display Batch No and ToolTip
                if (common.myStr(ViewState["ByIndent"]) != "Y")
                {
                    if (hdnBatchXML.Value != "")
                    {
                        xmlSchema = common.myStr(hdnBatchXML.Value);
                        sr = new StringReader(xmlSchema);
                        dsXml.ReadXml(sr);
                        if (dsXml.Tables.Count > 0)
                        {
                            if (dsXml.Tables[0].Rows.Count > 0)
                            {
                                dvBatchNo = new DataView(dsXml.Tables[0]);
                                dvBatchNo.RowFilter = "Qty<>'0'";
                                dvNonZeroBatch = new DataView(dvBatchNo.ToTable());
                                dvNonZeroBatch.RowFilter = "itemId=" + hdnItemId.Value;
                                dtBatchNo = dvNonZeroBatch.ToTable(true, "BatchNo");
                                if (dtBatchNo.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtBatchNo.Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sBatchNo = dtBatchNo.Rows[i]["BatchNo"].ToString();
                                        }
                                        else
                                        {
                                            sBatchNo = dtBatchNo.Rows[i]["BatchNo"].ToString();
                                            //sBatchNo = sBatchNo + ", " + dtBatchNo.Rows[i]["BatchNo"].ToString();
                                        }
                                    }
                                }
                                //  if (dsXml.Tables[0].Rows[0]["Reusable"].ToString() == common.myStr(1)) //my 13102016


                                if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "EnableItemRemarksAlways", common.myInt(Session["FacilityId"]))) == "Y")//my
                                {
                                    txtReusableRemarks.Text = string.Empty;
                                    txtReusableRemarks.Enabled = true;
                                }
                                else
                                {
                                    if (common.myStr(hdnReusable.Value) == common.myStr(1))
                                    {
                                        txtReusableRemarks.Text = string.Empty;
                                        txtReusableRemarks.Enabled = true;
                                    }
                                    else
                                    {
                                        txtReusableRemarks.Text = string.Empty;
                                        txtReusableRemarks.Enabled = false;
                                    }
                                }
                            }
                        }
                        //lblBatchNo.Text = sBatchNo;
                        lblBatchNo.ToolTip = sBatchNo;

                        //try
                        //{
                        //    if (dvNonZeroBatch.ToTable() != null)
                        //    {
                        //        if (dvNonZeroBatch.ToTable().Rows.Count > 0)
                        //        {
                        //            lblExpiryDate.Text = Convert.ToDateTime(dvNonZeroBatch.ToTable().Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy");
                        //        }
                        //    }
                        //}
                        //catch
                        //{
                        //}
                    }
                }
                #endregion

                //if (common.myStr(Session["IsFromRequestFromWard"]) == "1")
                //{

                //    objHospitalSetup = new BaseC.HospitalSetup(sConString);//f22122016
                //    if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCheckNearestExpiryItem", common.myInt(Session["FacilityId"]))) == "Y")//f22122016
                //    {


                //        BaseC.clsPharmacy objphr = new clsPharmacy(sConString);//f22122016
                //        DataSet dataset = new DataSet(); //f22122016
                //                                         //HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId"); //f22122016
                //        LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");//f22122016
                //        Label lblExpiryDateOfItem = (Label)e.Row.FindControl("lblExpiryDate");//f22122016
                //        string strExpiryDate;
                //        string day, month, year, ExpiryDate;
                //        if (lnkItemName.Text != string.Empty && lblExpiryDateOfItem.Text != string.Empty) //f22122016
                //        {
                //            dataset = objphr.GetphrCheckBatchNoNearByExpiry(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                //           common.myInt(ddlStore.SelectedValue), common.myInt(hdnItemId.Value));
                //            if (dataset.Tables.Count > 0)
                //            {
                //                if (dataset.Tables[0].Rows.Count > 0)
                //                {

                //                    strExpiryDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("MM/dd/yyyy").ToString();
                //                    ExpiryDate = lblExpiryDateOfItem.Text;
                //                    if (ExpiryDate.IndexOf('/') == 4 || ExpiryDate.IndexOf('-') == 4)
                //                    {
                //                        ExpiryDate = Convert.ToDateTime(ExpiryDate).ToString("MM/dd/yyyy");
                //                    }
                //                    else
                //                    {
                //                        day = ExpiryDate.Substring(0, 2);
                //                        month = ExpiryDate.Substring(3, 2);
                //                        year = ExpiryDate.Substring(6, 4);
                //                        ExpiryDate = month + "/" + day + "/" + year;
                //                    }

                //                    //if (lblExpiryDateOfItem.Text == Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy").ToString())
                //                    if (ExpiryDate == strExpiryDate)
                //                    {

                //                    }
                //                    else
                //                    {
                //                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Nearest expiry item is available in stock.Please select nearest expiry item');", true);
                //                    }
                //                }
                //            }


                //        }
                //    }
                //}

                if (common.myStr(hdnDenialCode.Value) != "")
                    e.Row.ToolTip = common.myStr(hdnDenialCode.Value);

                hdnStockQty.Value = common.myDbl(hdnStockQty.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtQty.Text = common.myStr(common.myDbl(txtQty.Text));// ("F" + common.myInt(hdnDecimalPlaces.Value));
                //hdnCostPrice.Value = common.myDbl(hdnCostPrice.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtCharge.Text = common.myDbl(txtCharge.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));//common.myInt(hdnDecimalPlaces.Value)); //MRP

                if (common.myInt(hdnDiscount.Value) != 0)
                {
                    hdnPercentDiscount.Value = common.myDbl(hdnDiscount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                }
                else
                {
                    hdnPercentDiscount.Value = common.myDbl(hdnPercentDiscount.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                }
                txtDiscountAmt.Text = common.myDbl(txtDiscountAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //txtDiscountAmt.Text = common.TruncateDecimal(common.myDec(txtDiscountAmt.Text), 2).ToString();
                //txtTax.Text = common.myDbl(txtTax.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTax.Text = common.myDbl(txtTax.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtNetAmt.Text = common.myDbl(txtNetAmt.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtPatient.Text = common.myDbl(txtPatient.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtPayer.Text = common.myDbl(txtPayer.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));


                hdnTotCharge.Value = common.myStr(common.myDbl(hdnTotCharge.Value) + common.myDbl(txtCharge.Text));
                hdnTotDiscAmt.Value = common.myStr(common.myDbl(hdnTotDiscAmt.Value) + common.myDbl(txtDiscountAmt.Text));
                hdnTotPatientAmt.Value = common.myStr(common.myDbl(hdnTotPatientAmt.Value) + common.myDbl(txtPatient.Text));
                hdnTotPayerAmt.Value = common.myStr(common.myDbl(hdnTotPayerAmt.Value) + common.myDbl(txtPayer.Text));
                hdnTotNetAmt.Value = common.myStr(common.myDbl(hdnTotNetAmt.Value) + common.myDbl(txtNetAmt.Text));
                hdnTotQty.Value = common.myStr(common.myDbl(hdnTotQty.Value) + common.myDbl(txtQty.Text));

                //   hdnTotUnit.Value = common.myStr(common.myDbl(hdnTotUnit.Value) + common.myDbl(lblUnit.Text));
                hdnTotTax.Value = common.myStr(common.myDbl(hdnTotTax.Value) + common.myDbl(txtTax.Text));

                txtCharge.Enabled = false;
                txtPatient.Enabled = false;
                txtPayer.Enabled = false;


                BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
                if (common.myInt(hdnItemId.Value) != 0)
                {
                    DataSet ds = new DataSet();
                    ds = objPharmacy.GetUnitIssueUnit(common.myInt(hdnItemId.Value));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblItemUnitName.Text = ds.Tables[0].Rows[0]["IssueUnitName"].ToString();
                            hdnUnitId.Value = ds.Tables[0].Rows[0]["IssueUnitId"].ToString();
                        }
                    }
                }
                if (common.myInt(hdnPrescriptionDetailsId.Value) == 0)
                {
                    hdnISDHAApproved.Value = "1";
                }

                txtQty.Attributes.Add("onkeyup", "javascript:chekQty('" + hdnStockQty.ClientID + "','" + txtQty.ClientID + "','" + txtCharge.ClientID + "','" + hdnPercentDiscount.ClientID + "','" + txtDiscountAmt.ClientID + "','" + txtNetAmt.ClientID + "','" + txtPatient.ClientID + "','" + txtPayer.ClientID + "','" + hdnCopayPerc.ClientID + "','" + hdnISDHAApproved.ClientID + "','" + txtApprovalCode.ClientID + "',event );");

                gvService.Columns[(byte)GridService.MonographCIMS].Visible = false;
                gvService.Columns[(byte)GridService.InteractionCIMS].Visible = false;
                gvService.Columns[(byte)GridService.DHInteractionCIMS].Visible = false;

                gvService.Columns[(byte)GridService.MonographVIDAL].Visible = false;
                gvService.Columns[(byte)GridService.InteractionVIDAL].Visible = false;
                gvService.Columns[(byte)GridService.DHInteractionVIDAL].Visible = false;

                if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
                {
                    gvService.Columns[(byte)GridService.MonographCIMS].Visible = true;
                    gvService.Columns[(byte)GridService.InteractionCIMS].Visible = true;
                    gvService.Columns[(byte)GridService.DHInteractionCIMS].Visible = true;

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
                    gvService.Columns[(byte)GridService.MonographVIDAL].Visible = true;
                    gvService.Columns[(byte)GridService.InteractionVIDAL].Visible = true;
                    gvService.Columns[(byte)GridService.DHInteractionVIDAL].Visible = true;

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

                //    objHospitalSetup = new BaseC.HospitalSetup(sConString);//f22122016
                //    if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCheckNearestExpiryItem", common.myInt(Session["FacilityId"]))) == "Y")//f22122016
                //    {


                //    BaseC.clsPharmacy objphr = new clsPharmacy(sConString);//f22122016
                //    DataSet dataset = new DataSet(); //f22122016
                //    //HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId"); //f22122016
                //    LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");//f22122016
                //    Label lblExpiryDateOfItem = (Label)e.Row.FindControl("lblExpiryDate");//f22122016
                //    if (lnkItemName.Text != string.Empty && lblExpiryDateOfItem.Text != string.Empty) //f22122016
                //    {
                //        dataset = objphr.GetphrCheckBatchNoNearByExpiry(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                //       common.myInt(ddlStore.SelectedValue), common.myInt(hdnItemId.Value));
                //        if (dataset.Tables.Count > 0)
                //        {
                //            if (dataset.Tables[0].Rows.Count > 0)
                //            {
                //                if (lblExpiryDateOfItem.Text == Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy").ToString())
                //                {

                //                }
                //                else
                //                {
                //                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Nearest expiry item is available in stock.Please select nearest expiry item');", true);
                //                }
                //            }
                //        }


                //    }
                //}

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#104E8B";
                }
                TextBox txtTotQty = (TextBox)e.Row.FindControl("txtTotQty");
                TextBox txtTotalUnit = (TextBox)e.Row.FindControl("txtTotUnit");
                TextBox txtTotCharge = (TextBox)e.Row.FindControl("txtTotCharge");
                TextBox txtTotTax = (TextBox)e.Row.FindControl("txtTotTax");
                TextBox txtTotalDiscount = (TextBox)e.Row.FindControl("txtTotDiscount");
                TextBox txtTotalNetAmt = (TextBox)e.Row.FindControl("txtTotNetamt");
                TextBox txtTotalPatient = (TextBox)e.Row.FindControl("txtTotalPatient");
                TextBox txtTotalPayer = (TextBox)e.Row.FindControl("txtTotalPayer");

                txtTotQty.Text = common.myStr(hdnTotQty.Value);
                //  txtTotalUnit.Text = common.myStr(hdnTotUnit.Value);
                txtTotCharge.Text = common.myDbl(hdnTotCharge.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                txtTotTax.Text = common.myDbl(hdnTotTax.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTotalDiscount.Text = common.myDbl(hdnTotDiscAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTotalPatient.Text = common.myDbl(hdnTotPatientAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTotalPayer.Text = common.myDbl(hdnTotPayerAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                txtTotalNetAmt.Text = common.myDbl(hdnTotNetAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //  double receivamt = common.myDbl(common.myDbl(hdnTotNetAmt.Value).ToString("F" + common.myInt(hdnDecimalPlaces.Value)));
                txtNetAmount.Text = Math.Round(common.myDbl(hdnTotNetAmt.Value), MidpointRounding.AwayFromZero).ToString();

                double amt = Math.Round(common.myDbl(txtNetAmount.Text), MidpointRounding.AwayFromZero);
                txtLAmt.Text = common.myDbl(amt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //double rounding = amt - common.myDbl(txtNetAmount.Text);
                //txtRounding.Text = common.myDbl(rounding).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                hdnRoundOffPatient.Value = "0";
                hdnRoundOffPayer.Value = "0";
                txtRounding.Text = "";
                if (common.myDbl(txtTotalPatient.Text) > 0)
                {
                    double fraction = common.myDbl(txtTotalPatient.Text) - Math.Floor(common.myDbl(txtTotalPatient.Text));
                    if (fraction > 0)
                    {
                        if (fraction >= 0.5)
                        {
                            hdnRoundOffPatient.Value = (1 - fraction).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        }
                        else
                        {
                            hdnRoundOffPatient.Value = "-" + (fraction).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        }
                    }
                }
                //if (common.myDbl(txtTotalPayer.Text) > 0)
                //{
                //    double fraction = common.myDbl(txtTotalPayer.Text) - Math.Floor(common.myDbl(txtTotalPayer.Text));
                //    if (fraction > 0)
                //    {
                //        if (fraction >= 0.5)
                //        {
                //            hdnRoundOffPayer.Value = (1 - fraction).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //        }
                //        else
                //        {
                //            hdnRoundOffPayer.Value = "-" + (fraction).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //        }
                //    }
                //}


                txtRounding.Text = hdnRoundOffPatient.Value.ToString();
                int iDefaultCompanyId = common.myInt(hdnDefaultCompanyId.Value);

                //if (common.myInt(ddlPayer.SelectedValue) == iDefaultCompanyId)
                //{
                //    txtReceived.Text = common.myDbl(amt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //}
                //else
                //{
                //    txtReceived.Text = txtReceived.Text = Math.Round(common.myDbl(txtCopaymentAmount.Text)).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                //}
                if (ddlPatientType.Items.Count > 0)
                {
                    //if (common.myStr(ddlPatientType.SelectedItem.Text).ToUpper() != "CREDIT SALE")
                    //{
                    //    txtReceived.Text = common.myDbl(amt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                    //}
                    //else
                    //{
                    //if (amt == 0)
                    //{
                    //    txtReceived.Text = Math.Round(common.myDbl(amt), MidpointRounding.AwayFromZero).ToString();
                    //}
                    //else
                    //{
                    txtReceived.Text = Math.Round(common.myDbl(txtTotalPatient.Text), MidpointRounding.AwayFromZero).ToString();
                    //}
                    // }
                }

                //txtTotCharge.Text = common.FormatNumber(common.myStr(hdnTotCharge.Value), common.myInt(Session["HospitalLocationID"]), sConString); ;
                //txtTotTax.Text = common.FormatNumber(common.myStr(hdnTotTax.Value), common.myInt(Session["HospitalLocationID"]), sConString); ;
                //txtTotalDiscount.Text = common.FormatNumber(common.myStr(hdnTotDiscAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString);
                //txtTotalPatient.Text = common.FormatNumber(common.myStr(hdnTotPatientAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString);
                //txtTotalPayer.Text = common.FormatNumber(common.myStr(hdnTotPayerAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString); ;
                //txtTotalNetAmt.Text = common.FormatNumber(common.myStr(hdnTotNetAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString); ;
                //double receivamt = common.myDbl(common.FormatNumber(common.myStr(hdnTotNetAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString));
                //txtNetAmount.Text = common.FormatNumber(common.myStr(hdnTotNetAmt.Value), common.myInt(Session["HospitalLocationID"]), sConString);

                if (common.myStr(ViewState["saletype"]) == "OP-ISS")
                {
                    if (ddlPatientType.Text.ToUpper() != "CREDIT SALE")//|| (ddlPatientType.Text.ToUpper() == "CREDIT SALE" && common.myDbl(txtCopaymentAmount.Text) > 0))
                    {
                        Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");

                        TextBox txtNetBAmount = (TextBox)paymentMode.FindControl("txtNetBAmount");
                        int PaymentModeCount = 0;
                        foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
                        {
                            if (PaymentModeCount == 0)
                            {
                                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                                txtAmt.Text = Math.Round(common.myDbl(txtNetAmount.Text), MidpointRounding.AwayFromZero).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                txtNetBAmount.Text = common.myStr(Math.Round(Convert.ToDouble(txtNetAmount.Text), MidpointRounding.AwayFromZero));
                                TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                                txtBal.Text = "0";
                            }
                            else
                            {
                                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                                txtAmt.Text = "0";
                                TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                                txtBal.Text = "0";
                            }
                            PaymentModeCount = PaymentModeCount + 1;
                        }
                    }
                    else //"CREDIT SALE"
                    {
                        int PaymentModeCount = 0;
                        Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                        TextBox txtNetBAmount = (TextBox)paymentMode.FindControl("txtNetBAmount");
                        foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
                        {
                            if (PaymentModeCount == 0)
                            {
                                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                                txtAmt.Text = common.myDbl(txtReceived.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                txtNetBAmount.Text = common.myDbl(txtReceived.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                                txtBal.Text = "0";
                            }
                            else
                            {
                                TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
                                txtAmt.Text = "0";
                                TextBox txtBal = (TextBox)gvr.FindControl("txtBalance");
                                txtBal.Text = "0";
                            }
                            PaymentModeCount = PaymentModeCount + 1;
                        }
                    }
                }
                txtTotQty.Enabled = false;
                txtTotCharge.Enabled = false;
                txtTotTax.Enabled = false;
                txtTotalDiscount.Enabled = false;
                txtTotalNetAmt.Enabled = false;
                txtTotalPatient.Enabled = false;
                txtTotalPayer.Enabled = false;

                ///HtmlInputButton btnCalculate = (HtmlInputButton)e.Row.FindControl("btnCalculate");

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dsXml.Dispose();
            xmlSchema = "";
            dtBatchNo.Dispose();
            dvBatchNo.Dispose();
            dvNonZeroBatch.Dispose();
        }
    }
    private void BindgvItemSevice()
    {
        try
        {
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 0;
            gvService.DataSource = CreateTable();
            gvService.DataBind();

            setVisiblilityInteraction();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.TableName = "myTable";
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("PrescriptionDetailsId");
        dt.Columns.Add("GenericId");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("ExpiryDate");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("BatchXML");
        dt.Columns.Add("StockQty", typeof(double));
        dt.Columns.Add("RequiredQty");
        dt.Columns.Add("Qty");
        dt.Columns.Add("Units");
        dt.Columns.Add("CostPrice", typeof(double));
        dt.Columns.Add("MRP", typeof(double));
        dt.Columns.Add("PercentDiscount", typeof(double));
        dt.Columns.Add("DiscAmt", typeof(double));
        dt.Columns.Add("TaxAmt", typeof(double));
        dt.Columns.Add("NetAmt", typeof(double));
        dt.Columns.Add("PatientAmount", typeof(double));
        dt.Columns.Add("PayerAmount", typeof(double));
        dt.Columns.Add("UnitName");
        dt.Columns.Add("RequestedItemId");
        dt.Columns.Add("CopayAmt", typeof(double));
        dt.Columns.Add("CIMSItemId");
        dt.Columns.Add("CIMSType");
        dt.Columns.Add("VIDALItemId");
        dt.Columns.Add("ISDHAApproved");
        dt.Columns.Add("DenialCode");
        dt.Columns.Add("ItemCategory");
        dt.Columns.Add("ItemSubCategory");
        dt.Columns.Add("IsPharmacutical");

        dt.Columns.Add("CopayPerc", typeof(double));
        dt.Columns.Add("IsClose", typeof(bool));
        dt.Columns.Add("Reusable");//my13102016

        DataRow dr = dt.NewRow();
        dr["PrescriptionDetailsId"] = DBNull.Value;
        dr["GenericId"] = DBNull.Value;
        dr["ItemId"] = DBNull.Value;
        dr["ItemName"] = DBNull.Value;
        dr["ExpiryDate"] = DBNull.Value;
        dr["BatchId"] = DBNull.Value;
        dr["BatchNo"] = DBNull.Value;
        dr["BatchXML"] = DBNull.Value;
        dr["StockQty"] = DBNull.Value;
        dr["RequiredQty"] = DBNull.Value;
        dr["Qty"] = DBNull.Value;
        dr["Units"] = DBNull.Value;
        dr["CostPrice"] = DBNull.Value;
        dr["MRP"] = DBNull.Value;
        dr["PercentDiscount"] = DBNull.Value;
        dr["DiscAmt"] = DBNull.Value;
        dr["TaxAmt"] = DBNull.Value;
        dr["NetAmt"] = DBNull.Value;
        dr["PatientAmount"] = 0;
        dr["PayerAmount"] = 0;
        dr["UnitName"] = DBNull.Value;
        dr["RequestedItemId"] = DBNull.Value;
        dr["CopayAmt"] = DBNull.Value;
        dr["CopayPerc"] = DBNull.Value;
        dr["CIMSItemId"] = DBNull.Value;
        dr["CIMSType"] = DBNull.Value;
        dr["VIDALItemId"] = DBNull.Value;
        dr["ISDHAApproved"] = DBNull.Value;
        dr["DenialCode"] = DBNull.Value;
        dr["ItemCategory"] = DBNull.Value;
        dr["ItemSubCategory"] = DBNull.Value;
        dr["IsPharmacutical"] = DBNull.Value;
        dr["Reusable"] = DBNull.Value;//my13102016
        dr["IsClose"] = false;
        dt.Rows.Add(dr);
        ViewState["Servicetable"] = dt;

        ddlDiscountType.Enabled = false;
        btnDiscountApply.Enabled = false;

        return dt;
    }
    protected DataTable CreateTable1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;

        dt.Columns.Add("PrescriptionDetailsId");
        dt.Columns.Add("GenericId");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("ExpiryDate");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("StockQty", typeof(double));
        dt.Columns.Add("Qty");
        dt.Columns.Add("Units");
        dt.Columns.Add("CostPrice", typeof(double));
        dt.Columns.Add("MRP", typeof(double));
        dt.Columns.Add("PercentDiscount");
        dt.Columns.Add("DiscAmt", typeof(double));
        dt.Columns.Add("TaxAmt", typeof(double));
        dt.Columns.Add("NetAmt", typeof(double));
        dt.Columns.Add("PatientAmount", typeof(double));
        dt.Columns.Add("PayerAmount", typeof(double));
        dt.Columns.Add("UnitName");
        dt.Columns.Add("RequestedItemId");
        dt.Columns.Add("CopayAmt", typeof(double));
        dt.Columns.Add("CopayPerc", typeof(double));
        dt.Columns.Add("CIMSItemId");
        dt.Columns.Add("CIMSType");
        dt.Columns.Add("VIDALItemId");
        dt.Columns.Add("ISDHAApproved");
        dt.Columns.Add("DenialCode");
        dt.Columns.Add("ItemCategory");
        dt.Columns.Add("ItemSubCategory");
        dt.Columns.Add("IsPharmacutical");
        dt.Columns.Add("Reusable");//my13102016

        ViewState["Servicetable"] = dt;

        ddlDiscountType.Enabled = false;
        btnDiscountApply.Enabled = false;

        return dt;
    }
    protected void btnFindDoc_Click(object sender, EventArgs e)
    {
        clearControl();
        btnGetSaleDetails_OnClick(sender, e);
    }
    protected void btngetdiscount_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (hdndiscountxml.Value != "")
            {
                hdnTotCharge.Value = "0";
                hdnTotDiscAmt.Value = "0";
                hdnTotPatientAmt.Value = "0";
                hdnTotPayerAmt.Value = "0";
                hdnTotNetAmt.Value = "0";
                hdnTotQty.Value = "0";



                //    string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
                //    StringReader sr = new StringReader(xmlSchema);
                //    DataSet dsXml = new DataSet();
                //    dsXml.ReadXml(sr);

                //    if (dsXml.Tables.Count > 0)
                //    {
                //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
                //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
                //    }
                string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
                StringReader sr = new StringReader(xmlSchema);
                DataSet dsXml = new DataSet();
                dsXml.ReadXml(sr);

                string xmlSchemaItemWise = ((common.myStr(hdndiscountxmlItemWise.Value)).Split('^'))[0];
                StringReader sr1 = new StringReader(xmlSchemaItemWise);
                DataSet dsXmlItemWise = new DataSet();
                dsXmlItemWise.ReadXml(sr1);


                //raghuvir
                hdnDiscount.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
                hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
                hdnNarration.Value = dsXml.Tables[0].Rows[0]["c3"].ToString().Trim();
                //if (dsXml.Tables.Count > 0)
                //{
                //    if (dsXmlItemWise.Tables.Count > 0)
                //    {
                //        if (dsXmlItemWise.Tables[0].Rows.Count > 0)
                //        {
                //            hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
                //            hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
                //        }
                //    }
                //    else
                //    {
                //        hdnDiscount.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
                //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
                //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c3"].ToString().Trim();
                //    }



                //}

                DataTable tbl = new DataTable();

                if (ViewState["Servicetable"] != null)
                {
                    tbl = (DataTable)ViewState["Servicetable"];
                }

                DataRow DR;
                DataSet ds = (DataSet)ViewState["CompanyList"];
                DataView dv = ds.Tables[0].Copy().DefaultView;
                dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                for (int RowIdx = 0; RowIdx < tbl.Rows.Count; RowIdx++)
                {
                    HiddenField hdnBatchXML = (HiddenField)gvService.Rows[RowIdx].FindControl("hdnBatchXML");
                    HiddenField hdnItemId = (HiddenField)gvService.Rows[RowIdx].FindControl("hdnItemId");
                    HiddenField hdnBatchId = (HiddenField)gvService.Rows[RowIdx].FindControl("hdnBatchId");
                    double discPercentageItemWise = 0.0;
                    if (dsXmlItemWise.Tables.Count > 0)
                    {
                        if (dsXmlItemWise.Tables[0].Rows.Count > 0)
                        {
                            //DataRow[] dr1 = dsXmlItemWise.Tables[0].Select("hdnItemId=" + common.myInt(hdnItemId.Value) + "");
                            dsXmlItemWise.Tables[0].DefaultView.RowFilter = "hdnItemId=" + common.myInt(hdnItemId.Value) + " and hdnBatchId=" + common.myInt(hdnBatchId.Value) + "";
                            if (dsXmlItemWise.Tables[0].DefaultView.Count > 0)
                            {
                                discPercentageItemWise = common.myDbl(dsXmlItemWise.Tables[0].DefaultView[0]["hdnPercentDiscount"]);
                                dsXmlItemWise.Tables[0].DefaultView.RowFilter = "";
                            }
                            else
                            {
                                discPercentageItemWise = 0.0;
                            }
                        }
                        else
                        {
                            discPercentageItemWise = 0.0;
                        }
                    }
                    else
                    {
                        discPercentageItemWise = common.myDbl(hdnDiscount.Value);
                    }


                    double disc = 0.0;
                    double netamt = 0.0;
                    string xmlSchemabatch = "";
                    if (hdnBatchXML.Value != "")
                    {
                        string xmlBatch = common.myStr(hdnBatchXML.Value);
                        StringReader srBatch = new StringReader(xmlBatch);
                        DataSet dsBatch = new DataSet();
                        DataSet dsBatch1 = new DataSet();
                        //DataView dvRecord;
                        DataTable dtRecords = new DataTable();
                        dsBatch1.ReadXml(srBatch);

                        dsBatch1.Tables[0].DefaultView.RowFilter = "ItemId=" + common.myInt(hdnItemId.Value) + " and BatchId=" + common.myInt(hdnBatchId.Value) + "";
                        DataView dvRecord = dsBatch1.Tables[0].DefaultView;
                        dtRecords = dvRecord.ToTable();
                        dsBatch.Tables.Add(dtRecords);

                        DataTable tblbatch = new DataTable();
                        tblbatch = tbl.Clone();
                        tblbatch.Columns.Add("SalePrice");
                        tblbatch.Columns.Add("DiscAmtPercent");
                        tblbatch.Columns.Add("Tax");

                        if (dsBatch.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsBatch.Tables[0].Rows.Count; i++)
                            {

                                //DataRow[] dr1 = dsXmlItemWise.Tables[0].Select("hdnItemId=" + common.myInt(dsBatch.Tables[0].Rows[i]["ItemId"]) + "");
                                //if (dr1.Length > 0)
                                //{
                                //    discPercentageItemWise = common.myDbl(dr1[0]["hdnPercentDiscount"]);
                                //}
                                //else
                                //{
                                //    discPercentageItemWise = 0.0;
                                //}

                                DataRow dr = tblbatch.NewRow();
                                dr["ItemId"] = common.myInt(dsBatch.Tables[0].Rows[i]["ItemId"]);
                                dr["ItemName"] = common.myStr(dsBatch.Tables[0].Rows[i]["ItemName"]);
                                dr["BatchId"] = common.myInt(dsBatch.Tables[0].Rows[i]["BatchId"]);
                                dr["BatchNo"] = common.myStr(dsBatch.Tables[0].Rows[i]["BatchNo"]);
                                dr["ExpiryDate"] = common.myStr(dsBatch.Tables[0].Rows[i]["ExpiryDate"]);
                                dr["StockQty"] = common.myInt(dsBatch.Tables[0].Rows[i]["StockQty"]);
                                dr["MRP"] = common.myDbl(dsBatch.Tables[0].Rows[i]["SalePrice"]);
                                dr["SalePrice"] = common.myDbl(dsBatch.Tables[0].Rows[i]["SalePrice"]);
                                dr["CostPrice"] = common.myDbl(dsBatch.Tables[0].Rows[i]["CostPrice"]);
                                dr["DiscAmtPercent"] = common.myDbl(discPercentageItemWise);

                                dr["Tax"] = common.myDbl(dsBatch.Tables[0].Rows[i]["Tax"]);
                                dr["Qty"] = common.myStr(dsBatch.Tables[0].Rows[i]["Qty"]);

                                double QtyBatch = common.myDbl(dsBatch.Tables[0].Rows[i]["Qty"]);
                                double MRPBatch = common.myDbl(dsBatch.Tables[0].Rows[i]["MRP"]);




                                double discBatch = (QtyBatch * MRPBatch * common.myDbl(discPercentageItemWise) / 100);
                                double netamtBatch = (QtyBatch * MRPBatch) - discBatch;

                                disc += discBatch;
                                netamt += netamtBatch;

                                if (common.myDbl(discBatch) != 0)
                                {
                                    dr["DiscAmt"] = common.myDbl(discBatch);
                                }
                                else
                                {
                                    dr["DiscAmt"] = common.myStr(dsBatch.Tables[0].Rows[i]["DisAmt"]);
                                }
                                if (common.myDbl(netamtBatch) != 0)
                                {
                                    dr["NetAmt"] = common.myDbl(netamtBatch);
                                    //dr["NetAmt"] = Math.Round(netamtBatch, 2); common.myDbl(netamtBatch);
                                }
                                else
                                {
                                    dr["NetAmt"] = common.myStr(dsBatch.Tables[0].Rows[i]["NetAmt"]);
                                }


                                dr["RequestedItemId"] = common.myStr(dsBatch.Tables[0].Rows[i]["ItemId"]);
                                tblbatch.Rows.Add(dr);
                                tblbatch.AcceptChanges();

                            }
                            tblbatch.TableName = "ItemBatch";
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                            tblbatch.WriteXml(writer);
                            xmlSchemabatch = writer.ToString();
                            tblbatch.Dispose();
                            dsBatch.Dispose();
                        }

                    }
                    DR = tbl.Rows[RowIdx];
                    if (xmlSchemabatch != "")
                    {
                        DR["BatchXML"] = xmlSchemabatch;
                        hdnBatchXML.Value = xmlSchemabatch;
                    }

                    double Qty = common.myDbl(DR["Qty"]);
                    double Rate = common.myDbl(DR["MRP"]);

                    //double disc=0.0;
                    //double netamt=0.0;
                    if (xmlSchemabatch == "")
                    {
                        disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPercentageItemWise) / 100), 2);
                        netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;
                    }
                    DR["PercentDiscount"] = common.myDbl(discPercentageItemWise);
                    //DR["DiscAmt"] = common.myDbl(disc).ToString("F" + common.myInt(hdnDecimalPlaces.Value));// , common.myInt(hdnDecimalPlaces.Value));
                    // DR["NetAmt"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                    DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));
                    // DR["DiscAmt"] = common.myDbl(disc);// , common.myInt(hdnDecimalPlaces.Value));
                    //DR["NetAmt"] = common.myDbl(netamt);
                    DR["NetAmt"] = Math.Round(netamt, 2);

                    //txtPercentDiscount.Text = common.myDbl(txtPercentDiscount.Text).ToString("F" + common.myInt(hdnDecimalPlaces.Value));

                    if (hdnPaymentType.Value == "C")//cash sale
                    {
                        //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        DR["PatientAmount"] = netamt;
                        DR["PayerAmount"] = 0.00;
                    }
                    else //credit sale
                    {
                        if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                        {
                            if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                            {
                                //DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                            }
                            else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                            {
                                // DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                DR["PatientAmount"] = 0;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;
                            }
                            else
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                //DR["PatientAmount"] = netamt;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;

                                //DR["CopayAmt"] = netamt;
                                DR["PatientAmount"] = 0;
                            }

                            //if (common.myStr(DR["ISDHAApproved"]) == "1")
                            //{
                            //    DR["PayerAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                            //    DR["PatientAmount"] = 0;
                            //}
                            //else
                            //{
                            //    DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                            //    DR["PayerAmount"] = 0;
                            //}
                        }
                        else
                        {
                            //  DR["PayerAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                            DR["PayerAmount"] = Math.Round(netamt, 2);
                            //DR["PayerAmount"] = netamt;
                            DR["PatientAmount"] = 0;
                        }

                    }




                }
                tbl.AcceptChanges();
                DataTable dt = applyCoPayment(tbl, 0);
                ViewState["Servicetable"] = dt;
                Session.Remove("OPSaleServicetable");
                Session["OPSaleServicetable"] = dt;
                Session["IsFromRequestFromWard"] = 0;
                Session["ForBLKDiscountPolicy"] = 0;
                gvService.DataSource = dt;
                gvService.DataBind();

                setVisiblilityInteraction();
                //}
                //else
                //{
                //    string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
                //    StringReader sr = new StringReader(xmlSchema);
                //    DataSet dsXml = new DataSet();
                //    dsXml.ReadXml(sr);

                //    if (dsXml.Tables.Count > 0)
                //    {
                //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
                //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
                //    }
                //    //SetPercentage();
                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btngetdiscount1_OnClick(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsPharmacy obj = new BaseC.clsPharmacy(sConString);
            DataTable dtbl = new DataTable();
            DataTable dtPhItems = new DataTable();
            DataSet ds1;
            ds1 = new DataSet();
            DataRow DR1;
            double discPerc;


            hdnTotCharge.Value = "0";
            hdnTotDiscAmt.Value = "0";
            hdnTotPatientAmt.Value = "0";
            hdnTotPayerAmt.Value = "0";
            hdnTotNetAmt.Value = "0";
            hdnTotQty.Value = "0";
            lblMessage.Text = "";

            double @dblMarginPerc = 0.0;
            @dblMarginPerc = common.myDbl(obj.getHospitalSetupValue("MarginPercforBLKDiscount", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));


            if (ViewState["Servicetable"] != null)
            {
                dtbl = (DataTable)ViewState["Servicetable"];
            }
            //if (dtbl.Rows.Count == 0)
            //{
            //    dtbl = CreateTable();
            //}

            for (int Idx = 0; Idx < dtbl.Rows.Count; Idx++)
            {
                int ItemId = common.myInt(dtbl.Rows[Idx]["ItemId"]);
                ds1 = obj.GetItemCategoryDetails(common.myInt(ItemId));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DR1 = dtbl.Rows[Idx];

                    DR1["ItemCategory"] = ds1.Tables[0].Rows[0]["ItemCategoryId"];
                    DR1["ItemSubCategory"] = ds1.Tables[0].Rows[0]["ItemSubCategoryId"];
                    DR1["IsPharmacutical"] = ds1.Tables[0].Rows[0]["IsPharmaceutical"];

                }
            }
            dtbl.AcceptChanges();


            if (ddlDiscountType.SelectedValue == "ND")
            {

                txtBLKId.Text = "";
                discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());
                //txtDiscPerc.Text = common.myStr(discPerc);
                //txtDiscPerc.ReadOnly = true;

                //DataView dv1 = dtbl.Copy().DefaultView;
                //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                //dtPhItems = dv1.ToTable();

                dtPhItems = dtbl;

                DataRow DR;
                DataSet ds = (DataSet)ViewState["CompanyList"];
                DataView dv = ds.Tables[0].Copy().DefaultView;
                dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                {

                    double disc = 0.0;
                    double netamt = 0.0;

                    DR = dtPhItems.Rows[RowIdx];

                    double Qty = common.myDbl(DR["Qty"]);
                    double Rate = common.myDbl(DR["MRP"]);


                    disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                    netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                    DR["PercentDiscount"] = common.myDbl(discPerc);

                    DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                    DR["NetAmt"] = Math.Round(netamt, 2);


                    if (hdnPaymentType.Value == "C")//cash sale
                    {
                        //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        DR["PatientAmount"] = Math.Round(netamt, 2);
                        DR["PayerAmount"] = 0.00;
                    }
                    else //credit sale
                    {
                        if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                        {
                            if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                            {
                                //DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                            }
                            else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                            {
                                // DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                DR["PatientAmount"] = 0;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;
                            }
                            else
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                //DR["PatientAmount"] = netamt;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;

                                //DR["CopayAmt"] = netamt;
                                DR["PatientAmount"] = 0;
                            }


                        }
                        else
                        {
                            DR["PayerAmount"] = Math.Round(netamt, 2);
                            //DR["PayerAmount"] = netamt;
                            DR["PatientAmount"] = 0;
                        }

                    }
                }
                dtPhItems.AcceptChanges();
            }
            else if (ddlDiscountType.SelectedValue == "SD")
            {

                if (txtBLKId.Text == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please enter valid Employee No for allowing Staff Discount..";

                    discPerc = 0.0;
                    //txtDiscPerc.Text = common.myStr(discPerc);
                    //txtDiscPerc.ReadOnly = true;

                    //DataView dv1 = dtbl.Copy().DefaultView;
                    //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                    dtPhItems = dtbl;

                    DataRow DR;
                    DataSet ds = (DataSet)ViewState["CompanyList"];
                    DataView dv = ds.Tables[0].Copy().DefaultView;
                    dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                    hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                    for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                    {

                        double disc = 0.0;
                        double netamt = 0.0;

                        DR = dtPhItems.Rows[RowIdx];

                        double Qty = common.myDbl(DR["Qty"]);
                        double Rate = common.myDbl(DR["MRP"]);


                        disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                        netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                        DR["PercentDiscount"] = common.myDbl(discPerc);

                        DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                        DR["NetAmt"] = Math.Round(netamt, 2);


                        if (hdnPaymentType.Value == "C")//cash sale
                        {
                            //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                            DR["PatientAmount"] = Math.Round(netamt, 2);
                            DR["PayerAmount"] = 0.00;
                        }
                        else //credit sale
                        {
                            if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                            {
                                if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                {
                                    //DR["PayerAmount"] = netamt;
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                }
                                else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                {
                                    // DR["PayerAmount"] = netamt;
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    DR["PatientAmount"] = 0;
                                    DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                    DR["CopayAmt"] = 0;
                                }
                                else
                                {
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    //DR["PatientAmount"] = netamt;
                                    DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                    DR["CopayAmt"] = 0;

                                    //DR["CopayAmt"] = netamt;
                                    DR["PatientAmount"] = 0;
                                }


                            }
                            else
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                //DR["PayerAmount"] = netamt;
                                DR["PatientAmount"] = 0;
                            }

                        }
                    }

                    dtPhItems.AcceptChanges();
                }

                else
                {

                    if (obj.GetValidateEmployeeNo(txtBLKId.Text) == "0")
                    {

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Invalid Employee No. Please enter valid Employee No..";

                        discPerc = 0.0;
                        //txtDiscPerc.Text = common.myStr(discPerc);
                        //txtDiscPerc.ReadOnly = true;

                        //DataView dv1 = dtbl.Copy().DefaultView;
                        //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                        dtPhItems = dtbl;

                        DataRow DR;
                        DataSet ds = (DataSet)ViewState["CompanyList"];
                        DataView dv = ds.Tables[0].Copy().DefaultView;
                        dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                        hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                        for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                        {

                            double disc = 0.0;
                            double netamt = 0.0;

                            DR = dtPhItems.Rows[RowIdx];

                            double Qty = common.myDbl(DR["Qty"]);
                            double Rate = common.myDbl(DR["MRP"]);


                            disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                            netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                            DR["PercentDiscount"] = common.myDbl(discPerc);

                            DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                            DR["NetAmt"] = Math.Round(netamt, 2);


                            if (hdnPaymentType.Value == "C")//cash sale
                            {
                                //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                DR["PatientAmount"] = Math.Round(netamt, 2);
                                DR["PayerAmount"] = 0.00;
                            }
                            else //credit sale
                            {
                                if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                {
                                    if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                    {
                                        //DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                    }
                                    else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                    {
                                        // DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        DR["PatientAmount"] = 0;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;
                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PatientAmount"] = netamt;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;

                                        //DR["CopayAmt"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }


                                }
                                else
                                {
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    //DR["PayerAmount"] = netamt;
                                    DR["PatientAmount"] = 0;
                                }

                            }
                        }

                        dtPhItems.AcceptChanges();
                    }
                    else
                    {
                        //tony1111

                        //discPerc = common.myDbl(obj.getHospitalSetupValue("OPSaleStaffDiscount", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                        discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());
                        //txtDiscPerc.Text = common.myStr(discPerc);
                        //txtDiscPerc.ReadOnly = true;

                        DataView dv1 = dtbl.Copy().DefaultView;
                        dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                        dtPhItems = dv1.ToTable();

                        DataRow DR;
                        DataSet ds = (DataSet)ViewState["CompanyList"];
                        DataView dv = ds.Tables[0].Copy().DefaultView;
                        dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                        hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                        for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                        {

                            double disc = 0.0;
                            double netamt = 0.0;

                            double ItemValue = 0.0;
                            double marginperc = 0.0;

                            DR = dtPhItems.Rows[RowIdx];

                            double Qty = common.myDbl(DR["Qty"]);
                            double Rate = common.myDbl(DR["MRP"]);
                            double costprice = common.myDbl(DR["CostPrice"]);

                            ItemValue = Rate - costprice;
                            marginperc = (ItemValue * 100) / Rate;
                            if (marginperc > @dblMarginPerc)
                            {
                                discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());

                                disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                                netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                                DR["PercentDiscount"] = common.myDbl(discPerc);

                                DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                                DR["NetAmt"] = Math.Round(netamt, 2);


                                if (hdnPaymentType.Value == "C")//cash sale
                                {
                                    //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                    DR["PatientAmount"] = Math.Round(netamt, 2);
                                    DR["PayerAmount"] = 0.00;
                                }
                                else //credit sale
                                {
                                    if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                    {
                                        if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                        {
                                            //DR["PayerAmount"] = netamt;
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                        }
                                        else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                        {
                                            // DR["PayerAmount"] = netamt;
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                            DR["PatientAmount"] = 0;
                                            DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            DR["CopayAmt"] = 0;
                                        }
                                        else
                                        {
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                            //DR["PatientAmount"] = netamt;
                                            DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            DR["CopayAmt"] = 0;

                                            //DR["CopayAmt"] = netamt;
                                            DR["PatientAmount"] = 0;
                                        }


                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PayerAmount"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }

                                }
                            }
                            else
                            {
                                discPerc = 0.0;

                                disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                                netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                                DR["PercentDiscount"] = common.myDbl(discPerc);

                                DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                                DR["NetAmt"] = Math.Round(netamt, 2);


                                if (hdnPaymentType.Value == "C")//cash sale
                                {
                                    //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                    DR["PatientAmount"] = Math.Round(netamt, 2);
                                    DR["PayerAmount"] = 0.00;
                                }
                                else //credit sale
                                {
                                    if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                    {
                                        if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                        {
                                            //DR["PayerAmount"] = netamt;
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                        }
                                        else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                        {
                                            // DR["PayerAmount"] = netamt;
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                            DR["PatientAmount"] = 0;
                                            DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            DR["CopayAmt"] = 0;
                                        }
                                        else
                                        {
                                            DR["PayerAmount"] = Math.Round(netamt, 2);
                                            //DR["PatientAmount"] = netamt;
                                            DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            DR["CopayAmt"] = 0;

                                            //DR["CopayAmt"] = netamt;
                                            DR["PatientAmount"] = 0;
                                        }
                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PayerAmount"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }
                                }
                            }
                        }
                        dtPhItems.AcceptChanges();

                        DataTable dt2 = new DataTable();
                        DataView dv2 = dtbl.Copy().DefaultView;
                        dv2.RowFilter = "IsPharmacutical='" + 0 + "'";
                        dt2 = dv2.ToTable();

                        //foreach (DataRow dr in dv2.ToTable().Rows)
                        //{
                        //    dtPhItems.Rows.Add(dr);
                        //    dtPhItems.AcceptChanges();
                        //}

                        if (dt2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {

                                //DataRow[] dr1 = dsXmlItemWise.Tables[0].Select("hdnItemId=" + common.myInt(dsBatch.Tables[0].Rows[i]["ItemId"]) + "");
                                //if (dr1.Length > 0)
                                //{
                                //    discPercentageItemWise = common.myDbl(dr1[0]["hdnPercentDiscount"]);
                                //}
                                //else
                                //{
                                //    discPercentageItemWise = 0.0;
                                //}

                                DataRow dr = dtPhItems.NewRow();
                                dr["ItemId"] = common.myInt(dt2.Rows[i]["ItemId"]);
                                dr["ItemName"] = common.myStr(dt2.Rows[i]["ItemName"]);
                                dr["BatchId"] = common.myInt(dt2.Rows[i]["BatchId"]);
                                dr["BatchNo"] = common.myStr(dt2.Rows[i]["BatchNo"]);
                                dr["ExpiryDate"] = common.myStr(dt2.Rows[i]["ExpiryDate"]);
                                dr["StockQty"] = common.myInt(dt2.Rows[i]["StockQty"]);
                                dr["MRP"] = common.myDbl(dt2.Rows[i]["MRP"]);
                                dr["CostPrice"] = common.myDbl(dt2.Rows[i]["CostPrice"]);
                                dr["PercentDiscount"] = common.myDbl(0.0);

                                //dr["Tax"] = common.myDbl(dt2.Rows[i]["Tax"]);
                                dr["Qty"] = common.myStr(dt2.Rows[i]["Qty"]);

                                double QtyBatch = common.myDbl(dt2.Rows[i]["Qty"]);
                                double MRPBatch = common.myDbl(dt2.Rows[i]["MRP"]);




                                //double discBatch = (QtyBatch * MRPBatch * common.myDbl(0.0) / 100);
                                double netamtBatch = (QtyBatch * MRPBatch);

                                //disc += discBatch;
                                double netamt = netamtBatch;

                                //if (common.myDbl(discBatch) != 0)
                                //{
                                //    dr["DiscAmt"] = common.myDbl(discBatch);
                                //}
                                //else
                                //{
                                dr["DiscAmt"] = common.myStr(dt2.Rows[i]["DiscAmt"]);
                                //}
                                //if (common.myDbl(netamtBatch) != 0)
                                //{
                                //    dr["NetAmt"] = common.myDbl(netamtBatch);
                                //    //dr["NetAmt"] = Math.Round(netamtBatch, 2); common.myDbl(netamtBatch);
                                //}
                                //else
                                //{
                                dr["NetAmt"] = Math.Round(netamt, 2);
                                //}

                                dr["RequestedItemId"] = common.myStr(dt2.Rows[i]["ItemId"]);


                                if (hdnPaymentType.Value == "C")//cash sale
                                {
                                    //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                    dr["PatientAmount"] = Math.Round(netamt, 2);
                                    dr["PayerAmount"] = 0.00;
                                }
                                else //credit sale
                                {
                                    if (common.myStr(dr["PrescriptionDetailsId"]) != "")
                                    {
                                        if ((common.myStr(dr["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                        {
                                            //DR["PayerAmount"] = netamt;
                                            dr["PayerAmount"] = Math.Round(netamt, 2);
                                        }
                                        else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(dr["ISDHAApproved"]) == "0"))
                                        {
                                            // DR["PayerAmount"] = netamt;
                                            dr["PayerAmount"] = Math.Round(netamt, 2);
                                            dr["PatientAmount"] = 0;
                                            dr["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            dr["CopayAmt"] = 0;
                                        }
                                        else
                                        {
                                            dr["PayerAmount"] = Math.Round(netamt, 2);
                                            //DR["PatientAmount"] = netamt;
                                            dr["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                            dr["CopayAmt"] = 0;

                                            //DR["CopayAmt"] = netamt;
                                            dr["PatientAmount"] = 0;
                                        }


                                    }
                                    else
                                    {
                                        dr["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PayerAmount"] = netamt;
                                        dr["PatientAmount"] = 0;
                                    }

                                }

                                dtPhItems.Rows.Add(dr);
                                dtPhItems.AcceptChanges();

                            }

                        }

                    }
                }
            }
            else if (ddlDiscountType.SelectedValue == "PD")
            {
                txtBLKId.Text = "";
                double netamt1 = 0.0;
                double Qty1 = 0.0;
                double Rate1 = 0.0;
                double PatientAmountLimit = 0.0;

                for (int j = 0; j < dtbl.Rows.Count; j++)
                {
                    Qty1 = common.myDbl(dtbl.Rows[j]["Qty"]);
                    Rate1 = common.myDbl(dtbl.Rows[j]["MRP"]);
                    netamt1 = (common.myDbl(Qty1) * common.myDbl(Rate1));

                }
                Session["NetAmount"] = netamt1;
                PatientAmountLimit = common.myDbl(obj.getHospitalSetupValue("OPSalePatientDiscountAmountLimit", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));

                if (netamt1 >= PatientAmountLimit)
                {
                    //discPerc = common.myDbl(obj.getHospitalSetupValue("OPSalePatientDiscount", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());
                    //txtDiscPerc.Text = common.myStr(discPerc);
                    //txtDiscPerc.ReadOnly = true;
                    //DataView dv1 = dtbl.Copy().DefaultView;
                    //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                    //dtPhItems = dv1.ToTable();

                    dtPhItems = dtbl;

                    DataRow DR;
                    DataSet ds = (DataSet)ViewState["CompanyList"];
                    DataView dv = ds.Tables[0].Copy().DefaultView;
                    dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                    hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                    for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                    {

                        double disc = 0.0;
                        double netamt = 0.0;

                        double ItemValue = 0.0;
                        double marginperc = 0.0;

                        DR = dtPhItems.Rows[RowIdx];

                        double Qty = common.myDbl(DR["Qty"]);
                        double Rate = common.myDbl(DR["MRP"]);

                        double costprice = common.myDbl(DR["CostPrice"]);

                        ItemValue = Rate - costprice;
                        marginperc = (ItemValue * 100) / Rate;
                        if (marginperc > @dblMarginPerc)
                        {
                            discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());

                            disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                            netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                            DR["PercentDiscount"] = common.myDbl(discPerc);

                            DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                            DR["NetAmt"] = Math.Round(netamt, 2);


                            if (hdnPaymentType.Value == "C")//cash sale
                            {
                                //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                DR["PatientAmount"] = Math.Round(netamt, 2);
                                DR["PayerAmount"] = 0.00;
                            }
                            else //credit sale
                            {
                                if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                {
                                    if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                    {
                                        //DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                    }
                                    else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                    {
                                        // DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        DR["PatientAmount"] = 0;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;
                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PatientAmount"] = netamt;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;

                                        //DR["CopayAmt"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }


                                }
                                else
                                {
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    //DR["PayerAmount"] = netamt;
                                    DR["PatientAmount"] = 0;
                                }

                            }
                        }
                        else
                        {
                            discPerc = 0.0;

                            disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                            netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                            DR["PercentDiscount"] = common.myDbl(discPerc);

                            DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                            DR["NetAmt"] = Math.Round(netamt, 2);


                            if (hdnPaymentType.Value == "C")//cash sale
                            {
                                //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                DR["PatientAmount"] = Math.Round(netamt, 2);
                                DR["PayerAmount"] = 0.00;
                            }
                            else //credit sale
                            {
                                if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                {
                                    if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                    {
                                        //DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                    }
                                    else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                    {
                                        // DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        DR["PatientAmount"] = 0;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;
                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PatientAmount"] = netamt;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;

                                        //DR["CopayAmt"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }


                                }
                                else
                                {
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    //DR["PayerAmount"] = netamt;
                                    DR["PatientAmount"] = 0;
                                }

                            }
                        }
                    }
                    dtPhItems.AcceptChanges();
                }
                else
                {
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Gross amount should be more than " + PatientAmountLimit;

                        discPerc = 0.0;
                        //txtDiscPerc.Text = common.myStr(discPerc);
                        //txtDiscPerc.ReadOnly = true;
                        //DataView dv1 = dtbl.Copy().DefaultView;
                        //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                        //dtPhItems = dv1.ToTable();
                        dtPhItems = dtbl;

                        DataRow DR;
                        DataSet ds = (DataSet)ViewState["CompanyList"];
                        DataView dv = ds.Tables[0].Copy().DefaultView;
                        dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                        hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                        for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                        {

                            double disc = 0.0;
                            double netamt = 0.0;

                            DR = dtPhItems.Rows[RowIdx];

                            double Qty = common.myDbl(DR["Qty"]);
                            double Rate = common.myDbl(DR["MRP"]);


                            disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                            netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                            DR["PercentDiscount"] = common.myDbl(discPerc);

                            DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                            DR["NetAmt"] = Math.Round(netamt, 2);


                            if (hdnPaymentType.Value == "C")//cash sale
                            {
                                //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                                DR["PatientAmount"] = Math.Round(netamt, 2);
                                DR["PayerAmount"] = 0.00;
                            }
                            else //credit sale
                            {
                                if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                                {
                                    if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                    {
                                        //DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                    }
                                    else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                                    {
                                        // DR["PayerAmount"] = netamt;
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        DR["PatientAmount"] = 0;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;
                                    }
                                    else
                                    {
                                        DR["PayerAmount"] = Math.Round(netamt, 2);
                                        //DR["PatientAmount"] = netamt;
                                        DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                        DR["CopayAmt"] = 0;

                                        //DR["CopayAmt"] = netamt;
                                        DR["PatientAmount"] = 0;
                                    }


                                }
                                else
                                {
                                    DR["PayerAmount"] = Math.Round(netamt, 2);
                                    //DR["PayerAmount"] = netamt;
                                    DR["PatientAmount"] = 0;
                                }

                            }
                        }
                    }
                    dtPhItems.AcceptChanges();

                }
            }
            else if (ddlDiscountType.SelectedValue == "MD")
            {
                txtBLKId.Text = "";
                //discPerc = common.myDbl(obj.getHospitalSetupValue("OPSaleManagementDiscount", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                //discPerc = common.myDbl(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim());
                discPerc = common.myDbl(txtDiscPerc.Text);
                //txtDiscPerc.Text = common.myStr("0.00");
                //txtDiscPerc.ReadOnly = false;
                //DataView dv1 = dtbl.Copy().DefaultView;
                //dv1.RowFilter = "IsPharmacutical='" + 1 + "'";
                //dtPhItems = dv1.ToTable();

                dtPhItems = dtbl;

                DataRow DR;
                DataSet ds = (DataSet)ViewState["CompanyList"];
                DataView dv = ds.Tables[0].Copy().DefaultView;
                dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

                for (int RowIdx = 0; RowIdx < dtPhItems.Rows.Count; RowIdx++)
                {

                    double disc = 0.0;
                    double netamt = 0.0;

                    DR = dtPhItems.Rows[RowIdx];

                    double Qty = common.myDbl(DR["Qty"]);
                    double Rate = common.myDbl(DR["MRP"]);


                    disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(discPerc) / 100), 2);
                    netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                    DR["PercentDiscount"] = common.myDbl(discPerc);

                    DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));

                    DR["NetAmt"] = Math.Round(netamt, 2);


                    if (hdnPaymentType.Value == "C")//cash sale
                    {
                        //DR["PatientAmount"] = common.myDbl(netamt).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        DR["PatientAmount"] = Math.Round(netamt, 2);
                        DR["PayerAmount"] = 0.00;
                    }
                    else //credit sale
                    {
                        if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                        {
                            if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                            {
                                //DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                            }
                            else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                            {
                                // DR["PayerAmount"] = netamt;
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                DR["PatientAmount"] = 0;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;
                            }
                            else
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                //DR["PatientAmount"] = netamt;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;

                                //DR["CopayAmt"] = netamt;
                                DR["PatientAmount"] = 0;
                            }


                        }
                        else
                        {
                            DR["PayerAmount"] = Math.Round(netamt, 2);
                            //DR["PayerAmount"] = netamt;
                            DR["PatientAmount"] = 0;
                        }

                    }
                }
                dtPhItems.AcceptChanges();
            }

            ////if (hdndiscountxml.Value != "")
            ////{




            ////    //    string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
            ////    //    StringReader sr = new StringReader(xmlSchema);
            ////    //    DataSet dsXml = new DataSet();
            ////    //    dsXml.ReadXml(sr);

            ////    //    if (dsXml.Tables.Count > 0)
            ////    //    {
            ////    //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
            ////    //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
            ////    //    }
            ////    string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
            ////    StringReader sr = new StringReader(xmlSchema);
            ////    DataSet dsXml = new DataSet();
            ////    dsXml.ReadXml(sr);

            ////    string xmlSchemaItemWise = ((common.myStr(hdndiscountxmlItemWise.Value)).Split('^'))[0];
            ////    StringReader sr1 = new StringReader(xmlSchemaItemWise);
            ////    DataSet dsXmlItemWise = new DataSet();
            ////    dsXmlItemWise.ReadXml(sr1);


            ////    //raghuvir
            ////    hdnDiscount.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
            ////    hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
            ////    hdnNarration.Value = dsXml.Tables[0].Rows[0]["c3"].ToString().Trim();
            ////    //if (dsXml.Tables.Count > 0)
            ////    //{
            ////    //    if (dsXmlItemWise.Tables.Count > 0)
            ////    //    {
            ////    //        if (dsXmlItemWise.Tables[0].Rows.Count > 0)
            ////    //        {
            ////    //            hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
            ////    //            hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
            ////    //        }
            ////    //    }
            ////    //    else
            ////    //    {
            ////    //        hdnDiscount.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
            ////    //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
            ////    //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c3"].ToString().Trim();
            ////    //    }



            ////    //}

            ////    DataTable tbl = new DataTable();

            ////    if (ViewState["Servicetable"] != null)
            ////    {
            ////        tbl = (DataTable)ViewState["Servicetable"];
            ////    }



            DataTable dt = applyCoPayment(dtPhItems, 0);
            ViewState["Servicetable"] = dt;
            Session.Remove("OPSaleServicetable");
            Session["OPSaleServicetable"] = dt;
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 1;
            gvService.DataSource = null;
            gvService.DataSource = dt;
            gvService.DataBind();

            setVisiblilityInteraction();
            setGridColor();
            //}
            //else
            //{
            //    string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[0];
            //    StringReader sr = new StringReader(xmlSchema);
            //    DataSet dsXml = new DataSet();
            //    dsXml.ReadXml(sr);

            //    if (dsXml.Tables.Count > 0)
            //    {
            //        hdnAuthorizedId.Value = dsXml.Tables[0].Rows[0]["c1"].ToString().Trim();
            //        hdnNarration.Value = dsXml.Tables[0].Rows[0]["c2"].ToString().Trim();
            //    }
            //    //SetPercentage();
            //}

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }

    public void SetPercentage()
    {
        string xmlSchema = ((common.myStr(hdndiscountxml.Value)).Split('^'))[1];
        StringReader sr = new StringReader(xmlSchema);
        DataSet dsXml = new DataSet();
        dsXml.ReadXml(sr);
        int PrescriptionDetailsId = 0, GenericId = 0, ItemId = 0;
        foreach (DataRow row in dsXml.Tables[0].Rows)
        {
            PrescriptionDetailsId = common.myInt(row["c1"].ToString().Trim());
            GenericId = common.myInt(row["c2"].ToString().Trim());
            ItemId = common.myInt(row["c3"].ToString().Trim());
            hdnDiscount.Value = row["c4"].ToString().Trim();


            DataTable tbl = new DataTable();

            if (ViewState["Servicetable"] != null)
            {
                tbl = (DataTable)ViewState["Servicetable"];
            }

            DataRow DR;
            DataSet ds = (DataSet)ViewState["CompanyList"];
            DataView dv = ds.Tables[0].Copy().DefaultView;
            dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
            hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);

            for (int RowIdx = 0; RowIdx < tbl.Rows.Count; RowIdx++)
            {
                if (common.myInt(((HiddenField)gvService.Rows[RowIdx].FindControl("hdnPrescriptionDetailsId")).Value) == PrescriptionDetailsId &&
                    common.myInt(((HiddenField)gvService.Rows[RowIdx].FindControl("hdnGenericId")).Value) == GenericId &&
                    common.myInt(((HiddenField)gvService.Rows[RowIdx].FindControl("hdnItemId")).Value) == ItemId)
                {
                    HiddenField hdnBatchXML = (HiddenField)gvService.Rows[RowIdx].FindControl("hdnBatchXML");
                    HiddenField hdnItemId = (HiddenField)gvService.Rows[RowIdx].FindControl("hdnItemId");
                    double disc = 0.0;
                    double netamt = 0.0;
                    string xmlSchemabatch = "";
                    if (hdnBatchXML.Value != "")
                    {
                        string xmlBatch = common.myStr(hdnBatchXML.Value);
                        StringReader srBatch = new StringReader(xmlBatch);
                        DataSet dsBatch = new DataSet();
                        dsBatch.ReadXml(srBatch);

                        DataTable tblbatch = new DataTable();
                        tblbatch = tbl.Clone();
                        tblbatch.Columns.Add("SalePrice");
                        tblbatch.Columns.Add("DiscAmtPercent");
                        tblbatch.Columns.Add("Tax");

                        if (dsBatch.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsBatch.Tables[0].Rows.Count; i++)
                            {
                                DataRow dr = tblbatch.NewRow();
                                dr["ItemId"] = common.myInt(dsBatch.Tables[0].Rows[i]["ItemId"]);
                                dr["ItemName"] = common.myStr(dsBatch.Tables[0].Rows[i]["ItemName"]);
                                dr["BatchId"] = common.myInt(dsBatch.Tables[0].Rows[i]["BatchId"]);
                                dr["BatchNo"] = common.myStr(dsBatch.Tables[0].Rows[i]["BatchNo"]);
                                dr["ExpiryDate"] = common.myStr(dsBatch.Tables[0].Rows[i]["ExpiryDate"]);
                                dr["StockQty"] = common.myInt(dsBatch.Tables[0].Rows[i]["StockQty"]);
                                dr["MRP"] = common.myDbl(dsBatch.Tables[0].Rows[i]["SalePrice"]);
                                dr["SalePrice"] = common.myDbl(dsBatch.Tables[0].Rows[i]["SalePrice"]);
                                dr["CostPrice"] = common.myDbl(dsBatch.Tables[0].Rows[i]["CostPrice"]);
                                dr["DiscAmtPercent"] = common.myDbl(hdnDiscount.Value);

                                dr["Tax"] = common.myDbl(dsBatch.Tables[0].Rows[i]["Tax"]);
                                dr["Qty"] = common.myStr(dsBatch.Tables[0].Rows[i]["Qty"]);

                                double QtyBatch = common.myDbl(dsBatch.Tables[0].Rows[i]["Qty"]);
                                double MRPBatch = common.myDbl(dsBatch.Tables[0].Rows[i]["MRP"]);
                                double discBatch = (QtyBatch * MRPBatch * common.myDbl(hdnDiscount.Value) / 100);
                                double netamtBatch = (QtyBatch * MRPBatch) - discBatch;

                                disc += discBatch;
                                netamt += netamtBatch;

                                if (common.myDbl(discBatch) != 0)
                                {
                                    dr["DiscAmt"] = common.myDbl(discBatch);
                                }
                                else
                                {
                                    dr["DiscAmt"] = common.myStr(dsBatch.Tables[0].Rows[i]["DisAmt"]);
                                }
                                if (common.myDbl(netamtBatch) != 0)
                                {
                                    dr["NetAmt"] = common.myDbl(netamtBatch);
                                }
                                else
                                {
                                    dr["NetAmt"] = common.myStr(dsBatch.Tables[0].Rows[i]["NetAmt"]);
                                }
                                dr["RequestedItemId"] = common.myStr(dsBatch.Tables[0].Rows[i]["ItemId"]);
                                tblbatch.Rows.Add(dr);
                                tblbatch.AcceptChanges();

                            }
                            tblbatch.TableName = "ItemBatch";
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                            tblbatch.WriteXml(writer);
                            xmlSchemabatch = writer.ToString();
                            tblbatch.Dispose();
                            dsBatch.Dispose();
                        }

                    }
                    DR = tbl.Rows[RowIdx];
                    if (xmlSchemabatch != "")
                    {
                        DR["BatchXML"] = xmlSchemabatch;
                        hdnBatchXML.Value = xmlSchemabatch;
                    }

                    double Qty = common.myDbl(DR["Qty"]);
                    double Rate = common.myDbl(DR["MRP"]);

                    if (xmlSchemabatch == "")
                    {
                        disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(hdnDiscount.Value) / 100), 2);
                        netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;
                    }
                    DR["PercentDiscount"] = common.myDbl(hdnDiscount.Value);

                    DR["DiscAmt"] = Math.Round(disc, 2);// , common.myInt(hdnDecimalPlaces.Value));
                    DR["NetAmt"] = Math.Round(netamt, 2);



                    if (hdnPaymentType.Value == "C")//cash sale
                    {
                        DR["PatientAmount"] = netamt;
                        DR["PayerAmount"] = 0.00;
                    }
                    else //credit sale
                    {
                        if (common.myStr(DR["PrescriptionDetailsId"]) != "")
                        {
                            if ((common.myStr(DR["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                            }
                            else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(DR["ISDHAApproved"]) == "0"))
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                DR["PatientAmount"] = 0;
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;
                            }
                            else
                            {
                                DR["PayerAmount"] = Math.Round(netamt, 2);
                                DR["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                DR["CopayAmt"] = 0;
                                DR["PatientAmount"] = 0;
                            }
                        }
                        else
                        {
                            DR["PayerAmount"] = Math.Round(netamt, 2);
                            DR["PatientAmount"] = 0;
                        }
                    }
                }
            }
            tbl.AcceptChanges();
            DataTable dt = applyCoPayment(tbl, 0);
            ViewState["Servicetable"] = dt;
            Session.Remove("OPSaleServicetable");
            Session["OPSaleServicetable"] = dt;
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 0;
            gvService.DataSource = dt;
            gvService.DataBind();
            setVisiblilityInteraction();
        }
    }


    protected void btnGetSaleDetails_OnClick(object sender, EventArgs e)
    {
        try
        {

            BaseC.clsPharmacy objphr = new clsPharmacy(sConString);
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            if (common.myStr(hdnIssueId.Value) != "" || common.myStr(txtDocNo.Text) != "")
            {
                string sOPIP = "O";
                string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
                if (strpatienttype == "IP-ISS")
                {
                    sOPIP = "I";
                    //btnBarcodePrinting.Visible = true;
                }
                ds = objphr.GetphrSaleIssueItem("I", common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(ddlStore.SelectedValue), common.myInt(Session["FacilityID"]),
                                            common.myInt(ddlPatientType.SelectedValue), common.myInt(0),
                                            common.myStr(txtDocNo.Text), sOPIP);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Text = "";
                    hdnIssueId.Value = common.myStr(ds.Tables[0].Rows[0]["IssueId"]);
                    txtRegistrationNo.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();
                    if (ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim() != "")
                        txtEncounter.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                    hdnRegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString().Trim();
                    Session["RegistrationID"] = common.myInt(hdnRegistrationId.Value);
                    txtPatientName.Text = ds.Tables[0].Rows[0]["PatientName"].ToString().Trim();
                    txtPatientName.ToolTip = ds.Tables[0].Rows[0]["PatientName"].ToString().Trim();

                    if (txtEncounter.Text != "")
                    {
                        if (objphr.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                        {
                            lblPackagePatient.Visible = true;
                        }
                        else
                        {
                            lblPackagePatient.Visible = false;
                        }
                    }

                    txtAge.Text = ds.Tables[0].Rows[0]["Age"].ToString().Trim();
                    ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(ds.Tables[0].Rows[0]["AgeType"].ToString().Trim()));
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(ds.Tables[0].Rows[0]["Gender"].ToString().Trim()));
                    //ddlIssuedEmpId.SelectedIndex = ddlIssuedEmpId.Items.IndexOf(ddlIssuedEmpId.Items.FindItemByValue(ds.Tables[0].Rows[0]["IssueEmpId"].ToString().Trim()));

                    ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(ds.Tables[0].Rows[0]["SponsorId"].ToString().Trim()));
                    lblPayer.Text = ddlPayer.SelectedItem.Text;
                    if (ds.Tables[0].Rows[0]["PaymentType"].ToString().Trim() == "B")
                    {
                        //lblPayertype.Text = common.myStr(ds.Tables[0].Rows[0]["PaymentMode"]);
                        lblPayertype.Text = "Credit";
                    }
                    txtDocNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString().Trim();
                    dtpDocDate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["IssueDate"].ToString());
                    txtRemark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString().Trim();
                    ddlDoctor.Visible = false;
                    txtProvider.Text = ds.Tables[0].Rows[0]["RefByName"].ToString().Trim();
                    txtProvider.Visible = true;
                    // lblProvider.Visible = true;
                    //26072017b
                    txtMobile.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString().Trim();
                    txtMobile.ToolTip = ds.Tables[0].Rows[0]["MobileNo"].ToString().Trim();

                    txtaddress.Text = ds.Tables[0].Rows[0]["PatientAddress"].ToString().Trim();
                    txtaddress.ToolTip = ds.Tables[0].Rows[0]["PatientAddress"].ToString().Trim();
                    //26072017e
                    if (sOPIP == "O")
                    {

                        chkDoctor.Visible = true;
                        ddlPayer.Enabled = false;
                        chkDoctor.Checked = true;
                        chkDoctor.Enabled = false;
                        //btnBarcodePrinting.Visible = false;

                        ShowCashCredit();

                        //if (ddlPatientType.SelectedItem.Text == "CASH SALE" || ddlPatientType.SelectedItem.Text == "CREDIT SALE")
                        //{

                        //}
                        //else 
                        if (ddlPatientType.SelectedItem.Text == "STAFF SALE") //s for Staff 
                        {
                            txtEmpNo.Text = txtRegistrationNo.Text = ds.Tables[0].Rows[0]["EmployeeNo"].ToString().Trim();
                        }
                        lblIndentNo.Visible = false;
                        lblIndentNo.Text = "";
                    }
                    else
                    {
                        chkDoctor.Visible = false;
                        lblIndentNo.Visible = true;
                        lblIndentNo.Text = ds.Tables[0].Rows[0]["IndentNo"].ToString().Trim(); ;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        Session["IsFromRequestFromWard"] = 0;
                        Session["ForBLKDiscountPolicy"] = 0;
                        gvService.DataSource = ds.Tables[1].DefaultView;
                        gvService.DataBind();

                        setVisiblilityInteraction();

                        DataTable dt = (DataTable)ds.Tables[1];
                        ViewState["Servicetable"] = dt;
                    }

                    ds1 = objphr.GetphrSaleIssuePaymentmode(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ddlStore.SelectedValue), common.myInt(hdnIssueId.Value));
                    double recievamt = 0;

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (common.myStr(ds1.Tables[0].Rows[0]["ModeId"]) != "")
                        {
                            Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
                            grdPaymentMode.DataSource = ds1.Tables[0];
                            grdPaymentMode.DataBind();
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                recievamt = recievamt + common.myDbl(dr["Amount"].ToString());
                            }
                            txtReceived.Text = common.myDbl(Math.Round(recievamt, 0)).ToString("F" + common.myInt(hdnDecimalPlaces.Value));
                        }
                    }
                    txtRegistrationNo.Enabled = false;
                    txtEncounter.Enabled = false;
                    txtPatientName.Enabled = false;
                    txtAge.Enabled = false;
                    ddlAgeType.Enabled = false;
                    ddlGender.Enabled = false;
                    txtProvider.Enabled = false;

                    txtRemark.Enabled = false;
                    txtNetAmount.Enabled = false;
                    txtReceived.Enabled = false;
                    //btnFindPatient.Enabled = false;
                    //done by rakesh for user authorisation start
                    //btnSaveData.Visible = false;
                    SetPermission(btnSaveData, false);
                    //done by rakesh for user authorisation end
                    btnAddNewItem.Visible = false;
                    btndDiscount.Visible = false;

                    //done by rakesh for user authorisation start
                    //btnPrint.Visible = true;
                    SetPermission(btnPrint, "P", true);
                    //done by rakesh for user authorisation end
                    //f270717b
                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    if (common.myStr(objBill.getHospitalSetupValue("isDisplayLabelPrintInIPIssue", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                    {
                        btnLabelPrint.Visible = true;
                    }
                    else
                    {
                        btnLabelPrint.Visible = false;
                    }

                    //f270717e
                }

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnBindGridWithXml_OnClick(object sender, EventArgs e)
    {
        BindGridWithXml();
    }
    //void BindGridWithXml()
    //{
    //    try
    //    {
    //        if (common.myStr(hdnxmlString.Value) != "")
    //        {
    //            DataSet ds = (DataSet)ViewState["CompanyList"];
    //            DataView dvPaymentType = ds.Tables[0].Copy().DefaultView;
    //            dvPaymentType.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
    //            hdnPaymentType.Value = common.myStr(dvPaymentType.ToTable().Rows[0]["PaymentType"]);

    //            hdnTotCharge.Value = "0";
    //            hdnTotDiscAmt.Value = "0";
    //            hdnTotPatientAmt.Value = "0";
    //            hdnTotPayerAmt.Value = "0";
    //            hdnTotNetAmt.Value = "0";
    //            hdnTotQty.Value = "0";
    //            hdnTotUnit.Value = "0";
    //            hdnTotTax.Value = "0";

    //            string xmlSchema = common.myStr(hdnxmlString.Value);
    //            StringReader sr = new StringReader(xmlSchema);
    //            DataSet dsXml = new DataSet();
    //            DataTable dtXml = new DataTable();
    //            dsXml.ReadXml(sr);
    //            if (dsXml.Tables.Count > 0)
    //            {
    //                if (dsXml.Tables[0].Rows.Count > 0)
    //                {
    //                    DataView dv = new DataView(dsXml.Tables[0]);
    //                    dv.RowFilter = "BatchId > 0";
    //                    dtXml = dv.ToTable();
    //                }
    //                else
    //                    return;
    //            }
    //            else
    //                return;
    //            //---------------------------------------
    //            DataTable dtPreviousServices = new DataTable();
    //            if (common.myStr(ViewState["Servicetable"]) == "")
    //                dtPreviousServices = CreateTable1();
    //            else
    //            {
    //                dtPreviousServices = ((DataTable)ViewState["Servicetable"]);
    //            }


    //            if (dtPreviousServices.Rows.Count > 0)// != null)
    //            {
    //                //Check duplicate Item and remove start------------------------------------------------------------------
    //                List<DataRow> rowsToRemove = new List<DataRow>();
    //                List<DataRow> rowsToRemove1 = new List<DataRow>();
    //                List<DataRow> rowsToRemove2 = new List<DataRow>();
    //                for (int i = 0; i < dtPreviousServices.Rows.Count; i++)
    //                {
    //                    for (int j = 0; j < dtXml.Rows.Count; j++)
    //                    {
    //                        if (common.myStr(ViewState["ByIndent"]) == "Y")
    //                        {

    //                            if ((common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper()
    //                                == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
    //                                && (common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper() == ""))
    //                            {
    //                                rowsToRemove.Add(dtPreviousServices.Rows[i]);
    //                                //drow.Delete();
    //                            }
    //                            if (common.myStr(dtXml.Rows[j]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper()
    //                             && common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
    //                            {
    //                                rowsToRemove1.Add(dtXml.Rows[j]);
    //                                //drow.Delete();
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (common.myStr(dtXml.Rows[j]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper()
    //                                && common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
    //                            {
    //                                //rowsToRemove.Add(dtXml.Rows[j]);
    //                                ////drow.Delete();
    //                            }
    //                        }
    //                    }
    //                }

    //                //foreach (var dr in rowsToRemove)
    //                //{
    //                //    dtXml.Rows.Remove(dr);
    //                //}
    //                foreach (var dr in rowsToRemove)
    //                {
    //                    if (common.myStr(ViewState["ByIndent"]) == "Y")
    //                    {
    //                        dtPreviousServices.Rows.Remove(dr);
    //                    }
    //                    else
    //                    {
    //                        dtXml.Rows.Remove(dr);
    //                    }
    //                }
    //                foreach (var dr in rowsToRemove1)
    //                {
    //                    dtXml.Rows.Remove(dr);
    //                }
    //                foreach (var dr in rowsToRemove2)
    //                {
    //                    dtXml.Rows.Remove(dr);
    //                }
    //            }
    //            dtXml.AcceptChanges();
    //            //Check duplicate Item and remove end-------------------------------------------------------------------------

    //            if (dtXml.Rows.Count == 0)
    //            {
    //                return;
    //            }

    //            //For Prescription only Done by Raghuvir
    //            int pId = 0;
    //            if (txtIndentNo.Text.Trim().Length > 0)
    //            {
    //                if (dtXml.Rows.Count > 1)
    //                {
    //                    Alert.ShowAjaxMsg("Please Scan The Barcode Properly", Page);
    //                    return;
    //                }
    //                else
    //                {
    //                    DataRow[] dr = dtPreviousServices.Select("ItemId=" + dtXml.Rows[0]["ItemId"] + "");
    //                    if (dr.Length == 0)
    //                    {
    //                        Alert.ShowAjaxMsg("Item Is Not Available In The Prescription", Page);
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        pId = common.myInt(dr[0]["PrescriptionDetailsId"]);
    //                        DataRow[] dr2 = dtPreviousServices.Select("ItemId=" + dtXml.Rows[0]["ItemId"] + " and batchid=0");
    //                        if (dr2.Length > 0)
    //                        {
    //                            dtPreviousServices.Rows.Remove(dr2[0]);
    //                            dtPreviousServices.AcceptChanges();
    //                        }
    //                    }
    //                }



    //            }
    //            ////For Prescription only Done by Raghuvir 

    //            for (int i = 0; i < dtXml.Rows.Count; i++)
    //            {
    //                DataRow dr = dtPreviousServices.NewRow();
    //                dr["ItemId"] = common.myStr(dtXml.Rows[i]["ItemId"]);
    //                dr["ItemName"] = common.myStr(dtXml.Rows[i]["ItemName"]);
    //                dr["BatchId"] = common.myStr(dtXml.Rows[i]["BatchId"]);
    //                dr["BatchNo"] = common.myStr(dtXml.Rows[i]["BatchNo"]);
    //                dr["ExpiryDate"] = common.myStr(dtXml.Rows[i]["ExpiryDate"]);
    //                dr["StockQty"] = common.myStr(dtXml.Rows[i]["StockQty"]);
    //                dr["Qty"] = common.myStr(dtXml.Rows[i]["Qty"]);
    //                //   dr["Units"] = common.myStr(dtXml.Rows[i]["Units"]);
    //                dr["CostPrice"] = common.myStr(dtXml.Rows[i]["CostPrice"]);
    //                dr["MRP"] = common.myStr(dtXml.Rows[i]["SalePrice"]);
    //                dr["RequestedItemId"] = common.myStr(dtXml.Rows[i]["ItemId"]);
    //                dr["BatchXML"] = common.myStr(hdnxmlString.Value);

    //                if (common.myInt(hdnDiscount.Value) != 0)
    //                {
    //                    dr["PercentDiscount"] = common.myDbl(hdnDiscount.Value);
    //                }
    //                else
    //                {
    //                    dr["PercentDiscount"] = common.myDbl(dtXml.Rows[i]["DiscAmtPercent"]);
    //                }


    //                double Qty = common.myDbl(dr["Qty"]);
    //                double Rate = common.myDbl(dr["MRP"]);
    //                double PercentDiscount = common.myDbl(dr["PercentDiscount"]);
    //                double disc = (common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(PercentDiscount) / 100);
    //                double netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

    //                if (common.myDbl(disc) != 0)
    //                {
    //                    dr["DiscAmt"] = common.myDbl(disc);
    //                }
    //                else
    //                {
    //                    dr["DiscAmt"] = common.myStr(dtXml.Rows[i]["DisAmt"]);
    //                }
    //                dr["TaxAmt"] = common.myStr(dtXml.Rows[i]["Tax"]);
    //                if (common.myDbl(netamt) != 0)
    //                {
    //                    dr["NetAmt"] = common.myDbl(netamt);
    //                }
    //                else
    //                {
    //                    dr["NetAmt"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
    //                }

    //                BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);

    //                if (hdnPaymentType.Value == "C")
    //                {
    //                    if (common.myDbl(netamt) != 0)
    //                    {
    //                        dr["PatientAmount"] = common.myDbl(netamt);
    //                        dr["PayerAmount"] = 0;
    //                    }
    //                    else
    //                    {
    //                        dr["PatientAmount"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
    //                        dr["PayerAmount"] = 0;
    //                    }

    //                }
    //                else // PaymentType = B 
    //                {
    //                    dr["PayerAmount"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
    //                    dr["PatientAmount"] = 0;
    //                }
    //                dr["CopayAmt"] = 0;
    //                if (Request.QueryString["Code"] == "IPI")//Ipd sale
    //                {
    //                    dr["CopayPerc"] = common.myDbl(ViewState["IPPharmacyCoPayPercent"]);
    //                }
    //                else
    //                {
    //                    dr["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
    //                }

    //                if (common.myBool(ViewState["IsCIMSInterfaceActive"])
    //                    || common.myBool(ViewState["IsVIDALInterfaceActive"]))
    //                {
    //                    BaseC.clsEMR objEMR = new clsEMR(sConString);

    //                    DataSet dsInterface = objEMR.getInterfaceItemDetails(common.myInt(dtXml.Rows[i]["ItemId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

    //                    if (dsInterface.Tables[0].Rows.Count > 0)
    //                    {
    //                        dr["CIMSItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSItemId"]);
    //                        dr["CIMSType"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSType"]);
    //                        dr["VIDALItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["VIDALItemId"]);
    //                    }
    //                }
    //                if (pId > 0)
    //                {
    //                    dr["PrescriptionDetailsId"] = common.myInt(pId);
    //                }
    //                dtPreviousServices.Rows.Add(dr);
    //            }

    //            //----------------------------------------
    //            if (dtPreviousServices.Rows.Count > 0 && pId == 0)
    //            {
    //                DataView dvRecord = dtPreviousServices.DefaultView;
    //                dvRecord.RowFilter = "itemid<>''";

    //                dtPreviousServices = new DataTable();
    //                dtPreviousServices = dvRecord.ToTable();
    //            }
    //            DataTable dt = applyCoPayment(dtPreviousServices, 0);
    //            ViewState["Servicetable"] = dt;
    //            Session.Remove("OPSaleServicetable");
    //            Session["OPSaleServicetable"] = dt;
    //            gvService.DataSource = dt;
    //            gvService.DataBind();

    //            setVisiblilityInteraction();

    //            hdnxmlString.Value = "";
    //        }
    //        else
    //        {
    //            if (common.myStr(ViewState["Servicetable"]) == "")
    //            {
    //                gvService.DataSource = CreateTable();
    //                gvService.DataBind();

    //                setVisiblilityInteraction();
    //            }
    //            lblMessage.Text = "";
    //            return;
    //        }


    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        clsExceptionLog objException = new clsExceptionLog();
    //        objException.HandleException(Ex);
    //    }
    //}
    void BindGridWithXml()
    {
        try
        {
            BaseC.clsPharmacy objphr = new clsPharmacy(sConString);
            Boolean IsSubstitute = false;
            int IsStockAvailable = 0;
            int RequiredSubstituteCheck = 0;
            lblMessage.Text = "";

            if (common.myStr(hdnxmlString.Value) != "")
            {
                ddlDiscountType.Enabled = true;
                btnDiscountApply.Enabled = true;

                DataSet ds = (DataSet)ViewState["CompanyList"];
                DataView dvPaymentType = ds.Tables[0].Copy().DefaultView;
                dvPaymentType.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                if (dvPaymentType.ToTable().Rows.Count > 0)
                {
                    hdnPaymentType.Value = common.myStr(dvPaymentType.ToTable().Rows[0]["PaymentType"]);
                }
                hdnTotCharge.Value = "0";
                hdnTotDiscAmt.Value = "0";
                hdnTotPatientAmt.Value = "0";
                hdnTotPayerAmt.Value = "0";
                hdnTotNetAmt.Value = "0";
                hdnTotQty.Value = "0";
                hdnTotUnit.Value = "0";
                hdnTotTax.Value = "0";

                string xmlSchema = common.myStr(hdnxmlString.Value);
                StringReader sr = new StringReader(xmlSchema);
                DataSet dsXml = new DataSet();
                DataTable dtXml = new DataTable();
                dsXml.ReadXml(sr);
                if (dsXml.Tables.Count > 0)
                {
                    if (dsXml.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView(dsXml.Tables[0]);
                        dv.RowFilter = "BatchId > 0";
                        dtXml = dv.ToTable();
                    }
                    else
                        return;
                }
                else
                    return;
                //---------------------------------------
                DataTable dtPreviousServices = new DataTable();
                if (common.myStr(ViewState["Servicetable"]) == "")
                    dtPreviousServices = CreateTable1();
                else
                {
                    dtPreviousServices = ((DataTable)ViewState["Servicetable"]);
                }
                //if (dtPreviousServices.Rows.Count > 0)// != null)
                //{
                //    //Check duplicate Item and remove start------------------------------------------------------------------
                //    List<DataRow> rowsToRemove = new List<DataRow>();
                //    List<DataRow> rowsToRemove1 = new List<DataRow>();
                //    List<DataRow> rowsToRemove2 = new List<DataRow>();

                //    List<DataRow> rowsToRemoveP1 = new List<DataRow>();

                //    for (int i = 0; i < dtPreviousServices.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < dtXml.Rows.Count; j++)
                //        {
                //            if (common.myStr(ViewState["ByIndent"]) == "Y")
                //            {

                //                if ((common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper()
                //                    == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
                //                    && (common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper() == ""))
                //                {
                //                    rowsToRemove.Add(dtPreviousServices.Rows[i]);
                //                    //drow.Delete();
                //                }
                //                if (common.myStr(dtXml.Rows[j]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper()
                //                 && common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
                //                {
                //                    rowsToRemove1.Add(dtXml.Rows[j]);
                //                    //drow.Delete();
                //                }
                //            }
                //            else
                //            {
                //                if (common.myStr(dtXml.Rows[j]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["BatchNo"]).ToUpper()
                //                 && common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
                //                {
                //                    rowsToRemove.Add(dtXml.Rows[j]);
                //                    //drow.Delete();
                //                }
                //            }
                //            if (common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper() != common.myStr(dtPreviousServices.Rows[i]["ItemId"]).ToUpper())
                //            {
                //                RequiredSubstituteCheck = 1;
                //            }
                //        }
                //    }
                //    //for (int i = 0; i < dtPreviousServices.Rows.Count; i++)
                //    //{
                //    if (common.myInt(ViewState["IsWardRequest"]) == 1)
                //    {
                //        DataSet dsSub = new DataSet();

                //        for (int j = 0; j < dtXml.Rows.Count; j++)
                //        {
                //            if (RequiredSubstituteCheck == 1)
                //            {
                //                dsSub = objphr.GetSubstituteItems(common.myInt(Session["HospitalLocationId"]),
                //                               common.myInt(Session["FacilityID"]), common.myInt(dtXml.Rows[j]["ItemId"]), common.myInt(ddlStore.SelectedValue));
                //                if (dsSub.Tables[0].Rows.Count > 0)
                //                {
                //                    for (int k = 0; k < dtPreviousServices.Rows.Count; k++)
                //                    {
                //                        for (int h = 0; h < dsSub.Tables[0].Rows.Count; h++)
                //                        {
                //                            if (common.myStr(dtPreviousServices.Rows[k]["ItemId"]).ToUpper() == common.myStr(dsSub.Tables[0].Rows[h]["ItemId"]).ToUpper())
                //                            //&& common.myStr(dtPreviousServices.Rows[k]["BatchNo"]).ToUpper() == common.myStr(dsSub.Tables[0].Rows[h]["BatchNo"]).ToUpper())
                //                            {
                //                                if (common.myInt(dsSub.Tables[0].Rows[h]["Stock"]) > 0)
                //                                {
                //                                    lblMessage.Text = "Stock available for ordered Item. Don't allow to bill substitute item.";
                //                                    IsStockAvailable = 1;
                //                                    return;
                //                                }
                //                            }
                //                        }
                //                        if (IsStockAvailable == 0)
                //                        {
                //                            rowsToRemoveP1.Add(dtPreviousServices.Rows[k]);
                //                        }
                //                    }
                //                    for (int k = 0; k < dtXml.Rows.Count; k++)
                //                    {
                //                        for (int h = 0; h < dsSub.Tables[0].Rows.Count; h++)
                //                        {
                //                            if (common.myStr(dtXml.Rows[k]["ItemId"]).ToUpper() == common.myStr(dsSub.Tables[0].Rows[h]["ItemId"]).ToUpper())
                //                            {
                //                                foreach (var dr in rowsToRemoveP1)
                //                                {
                //                                    dtPreviousServices.Rows.Remove(dr);
                //                                }
                //                                IsSubstitute = true;
                //                            }
                //                        }
                //                    }


                //                }
                //                if (IsSubstitute == false)
                //                {
                //                    dtPreviousServices.DefaultView.RowFilter = "ItemId=" + common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper();
                //                    if (dtPreviousServices.DefaultView.Count == 0)
                //                    {
                //                        rowsToRemove2.Add(dtXml.Rows[j]);
                //                        break;
                //                    }
                //                    dtPreviousServices.DefaultView.RowFilter = "";
                //                }
                //            }
                //            else
                //            {
                //                dtPreviousServices.DefaultView.RowFilter = "ItemId=" + common.myStr(dtXml.Rows[j]["ItemId"]).ToUpper();
                //                if (dtPreviousServices.DefaultView.Count == 0)
                //                {
                //                    rowsToRemove2.Add(dtXml.Rows[j]);
                //                    break;
                //                }
                //                dtPreviousServices.DefaultView.RowFilter = "";
                //            }
                //        }
                //    }
                //    //}
                //    foreach (var dr in rowsToRemove)
                //    {
                //        if (common.myStr(ViewState["ByIndent"]) == "Y")
                //        {
                //            dtPreviousServices.Rows.Remove(dr);
                //        }
                //        else
                //        {
                //            dtXml.Rows.Remove(dr);
                //        }
                //    }
                //    foreach (var dr in rowsToRemove1)
                //    {
                //        dtXml.Rows.Remove(dr);
                //    }
                //    foreach (var dr in rowsToRemove2)
                //    {
                //        dtXml.Rows.Remove(dr);
                //    }
                //}
                //dtXml.AcceptChanges();
                //if (RequiredSubstituteCheck == 1)
                //{
                //    dtPreviousServices.AcceptChanges();
                //}
                ////Check duplicate Item and remove end-------------------------------------------------------------------------

                if (dtXml.Rows.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < dtXml.Rows.Count; i++)
                {
                    DataSet dsConversion = new DataSet();
                    int intConversionFactor = 0;
                    double dbQty;
                    int intConvertedQty;
                    int IsTopUp = 0;
                    dsConversion = objphr.getItemConversionFactor(common.myInt(dtXml.Rows[i]["ItemId"]));

                    if (dsConversion.Tables.Count > 0)
                    {
                        if (dsConversion.Tables[0].Rows.Count > 0)
                        {
                            intConversionFactor = common.myInt(dsConversion.Tables[0].Rows[0]["ConversionFactor2"]);
                        }
                    }
                    for (int j = 0; j < dtPreviousServices.Rows.Count; j++)
                    {

                        if (common.myStr(dtXml.Rows[i]["BatchNo"]).ToUpper() == common.myStr(dtPreviousServices.Rows[j]["BatchNo"]).ToUpper()
                                 && common.myStr(dtXml.Rows[i]["ItemId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[j]["ItemId"]).ToUpper()
                                 && common.myStr(dtXml.Rows[i]["BatchId"]).ToUpper() == common.myStr(dtPreviousServices.Rows[j]["BatchId"]).ToUpper())

                        {
                            if (hdnIsStripWiseSaleRequired.Value == "Y")
                            {
                                dtPreviousServices.Rows[j]["Qty"] = common.myStr(common.myInt(dtPreviousServices.Rows[j]["Qty"]) + common.myInt(intConversionFactor));
                            }
                            else
                            {
                                dtPreviousServices.Rows[j]["Qty"] = common.myStr(common.myInt(dtPreviousServices.Rows[j]["Qty"]));
                            }


                            double Qty = common.myDbl(dtPreviousServices.Rows[j]["Qty"]);
                            double Rate = common.myDbl(dtPreviousServices.Rows[j]["MRP"]);
                            double PercentDiscount = common.myDbl(dtPreviousServices.Rows[j]["PercentDiscount"]);
                            double disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(PercentDiscount) / 100), 2);
                            double netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                            if (common.myDbl(disc) != 0)
                            {
                                dtPreviousServices.Rows[j]["DiscAmt"] = common.myDbl(disc);
                            }

                            if (common.myDbl(netamt) != 0)
                            {
                                dtPreviousServices.Rows[j]["NetAmt"] = common.myDbl(netamt);
                            }

                            BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);

                            if (hdnPaymentType.Value == "C")
                            {
                                if (common.myDbl(netamt) != 0)
                                {
                                    dtPreviousServices.Rows[j]["PatientAmount"] = common.myDbl(netamt);
                                    dtPreviousServices.Rows[j]["PayerAmount"] = 0;
                                }

                            }
                            else // PaymentType = B 
                            {
                                if (common.myDbl(netamt) != 0)
                                {
                                    dtPreviousServices.Rows[j]["PayerAmount"] = common.myDbl(netamt);
                                    dtPreviousServices.Rows[j]["PatientAmount"] = 0;
                                }
                            }
                            dtPreviousServices.Rows[j]["CopayAmt"] = 0;
                            if (Request.QueryString["Code"] == "IPI")//Ipd sale
                            {
                                dtPreviousServices.Rows[j]["CopayPerc"] = common.myDbl(ViewState["IPPharmacyCoPayPercent"]);
                            }
                            else
                            {
                                dtPreviousServices.Rows[j]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]); ;
                            }

                            if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                                || common.myBool(ViewState["IsVIDALInterfaceActive"]))
                            {
                                BaseC.clsEMR objEMR = new clsEMR(sConString);

                                DataSet dsInterface = objEMR.getInterfaceItemDetails(common.myInt(dtXml.Rows[i]["ItemId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

                                if (dsInterface.Tables[0].Rows.Count > 0)
                                {
                                    dtPreviousServices.Rows[j]["CIMSItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSItemId"]);
                                    dtPreviousServices.Rows[j]["CIMSType"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSType"]);
                                    dtPreviousServices.Rows[j]["VIDALItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["VIDALItemId"]);
                                }
                            }

                            dtPreviousServices.AcceptChanges();
                            IsTopUp = 1;
                        }
                    }

                    if (IsTopUp == 0)
                    {
                        DataRow dr = dtPreviousServices.NewRow();
                        dr["ItemId"] = common.myStr(dtXml.Rows[i]["ItemId"]);
                        dr["ItemName"] = common.myStr(dtXml.Rows[i]["ItemName"]);
                        dr["BatchId"] = common.myStr(dtXml.Rows[i]["BatchId"]);
                        dr["BatchNo"] = common.myStr(dtXml.Rows[i]["BatchNo"]);
                        dr["ExpiryDate"] = common.myStr(dtXml.Rows[i]["ExpiryDate"]);
                        dr["StockQty"] = common.myStr(dtXml.Rows[i]["StockQty"]);
                        //dr["Qty"] = common.myStr(dtXml.Rows[i]["Qty"]);
                        if (hdnIsStripWiseSaleRequired.Value == "Y")
                        {
                            if (intConversionFactor > 0)
                            {
                                if (common.myInt(dtXml.Rows[i]["Qty"]) == 0)
                                {
                                    dr["Qty"] = common.myInt(intConversionFactor);
                                }
                                else
                                {
                                    dbQty = Math.Ceiling(common.myDbl(common.myDec(dtXml.Rows[i]["Qty"]) / common.myDec(intConversionFactor)));
                                    intConvertedQty = common.myInt(dbQty) * common.myInt(intConversionFactor);
                                    dr["Qty"] = intConvertedQty;
                                }
                            }
                            else
                            {
                                dr["Qty"] = common.myStr(dtXml.Rows[i]["Qty"]);
                            }
                        }
                        else
                        {
                            dr["Qty"] = common.myStr(dtXml.Rows[i]["Qty"]);
                        }
                        //dr["RequiredQty"] = common.myStr(dtXml.Rows[i]["RequiredQty"]);
                        //   dr["Units"] = common.myStr(dtXml.Rows[i]["Units"]);
                        dr["CostPrice"] = common.myStr(dtXml.Rows[i]["CostPrice"]);
                        //dr["MRP"] = common.myStr(dtXml.Rows[i]["SalePrice"]);
                        dr["MRP"] = common.myStr(dtXml.Rows[i]["MRP"]);
                        dr["RequestedItemId"] = common.myStr(dtXml.Rows[i]["ItemId"]);
                        dr["BatchXML"] = common.myStr(hdnxmlString.Value);


                        if (common.myInt(hdnDiscount.Value) != 0)
                        {
                            dr["PercentDiscount"] = common.myDbl(hdnDiscount.Value);
                        }
                        else
                        {
                            dr["PercentDiscount"] = common.myDbl(dtXml.Rows[i]["DiscAmtPercent"]);
                        }


                        double Qty = common.myDbl(dr["Qty"]);
                        double Rate = common.myDbl(dr["MRP"]);
                        double PercentDiscount = common.myDbl(dr["PercentDiscount"]);
                        double disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(PercentDiscount) / 100), 2);
                        double netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                        if (common.myDbl(disc) != 0)
                        {
                            dr["DiscAmt"] = common.myDbl(disc);
                        }
                        else
                        {
                            dr["DiscAmt"] = common.myStr(dtXml.Rows[i]["DisAmt"]);
                        }
                        dr["TaxAmt"] = common.myStr(dtXml.Rows[i]["Tax"]);
                        if (common.myDbl(netamt) != 0)
                        {
                            dr["NetAmt"] = common.myDbl(netamt);
                        }
                        else
                        {
                            dr["NetAmt"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
                        }

                        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);

                        if (hdnPaymentType.Value == "C")
                        {
                            if (common.myDbl(netamt) != 0)
                            {
                                dr["PatientAmount"] = common.myDbl(netamt);
                                dr["PayerAmount"] = 0;
                            }
                            else
                            {
                                dr["PatientAmount"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
                                dr["PayerAmount"] = 0;
                            }

                        }
                        else // PaymentType = B 
                        {
                            dr["PayerAmount"] = common.myStr(dtXml.Rows[i]["NetAmt"]);
                            dr["PatientAmount"] = 0;
                        }
                        dr["CopayAmt"] = 0;
                        if (Request.QueryString["Code"] == "IPI")//Ipd sale
                        {
                            dr["CopayPerc"] = common.myDbl(ViewState["IPPharmacyCoPayPercent"]);
                        }
                        else
                        {
                            dr["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]); ;
                        }

                        if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                            || common.myBool(ViewState["IsVIDALInterfaceActive"]))
                        {
                            BaseC.clsEMR objEMR = new clsEMR(sConString);

                            DataSet dsInterface = objEMR.getInterfaceItemDetails(common.myInt(dtXml.Rows[i]["ItemId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

                            if (dsInterface.Tables[0].Rows.Count > 0)
                            {
                                dr["CIMSItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSItemId"]);
                                dr["CIMSType"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSType"]);
                                dr["VIDALItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["VIDALItemId"]);
                            }
                        }
                        dr["Reusable"] = common.myStr(dtXml.Rows[i]["Reusable"]); //my13102016
                        dtPreviousServices.Rows.Add(dr);
                    }
                }

                //----------------------------------------
                if (common.myStr(ViewState["AutoPresc"]) == "N")
                {
                    if (dtPreviousServices.Rows.Count > 0)
                    {
                        DataView dvRecord = dtPreviousServices.DefaultView;
                        dvRecord.RowFilter = "isnull(itemid,0)<>0";

                        dtPreviousServices = new DataTable();
                        dtPreviousServices = dvRecord.ToTable();
                    }
                }
                else
                {
                    if (dtPreviousServices.Rows.Count > 0)
                    {
                        DataView dvRecord = dtPreviousServices.DefaultView;
                        dvRecord.RowFilter = "isnull(itemid,0)<>0";

                        dtPreviousServices = new DataTable();
                        dtPreviousServices = dvRecord.ToTable();
                    }
                }
                DataTable dt = applyCoPayment(dtPreviousServices, 0);
                ViewState["Servicetable"] = dt;
                ViewState["RequestFromWardItems"] = dt; //20042017
                Session["IsFromRequestFromWard"] = 0;
                Session["ForBLKDiscountPolicy"] = 0;
                gvService.DataSource = dt;
                gvService.DataBind();

                setVisiblilityInteraction();

                hdnxmlString.Value = "";
            }
            else
            {
                if (common.myStr(ViewState["Servicetable"]) == "")
                {
                    Session["IsFromRequestFromWard"] = 0;
                    Session["ForBLKDiscountPolicy"] = 0;
                    gvService.DataSource = CreateTable();
                    gvService.DataBind();

                    setVisiblilityInteraction();
                }
                lblMessage.Text = "";
                return;
            }

            setGridColor();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void imgBtnSearchDoc_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //clearControl();
            hdnIssueId.Value = "";
            string UseFor = "I";
            if (common.myStr(Request.QueryString["IR"]) == "R")
            {
                UseFor = "R";
            }
            else if (common.myStr(Request.QueryString["IR"]) == "S")
            {
                UseFor = "I";
            }
            string opip = "O";
            if (common.myStr(Request.QueryString["OPIP"]) == "I")
            {
                opip = "I";
            }

            string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
            lblMessage.Text = "";
            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/ViewDocument.aspx?SetupId=" + ddlPatientType.SelectedValue + "&DocNo=" + txtDocNo.Text + "&OPIP=" + opip + "&UseFor=" + UseFor + "&StoreId=" + common.myStr(ddlStore.SelectedValue) + "&StoreName=" + common.myStr(ddlStore.SelectedItem.Text);
            RadWindow1.Height = 550;
            RadWindow1.Width = 950;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "SearchDocOnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (txtDocNo.Text == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter Issue No.";
            return;

        }
        if (strpatienttype == "OP-ISS" && hdnIssueId.Value != "")
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.Text, "select IssueReturn from PhrIssueSaleMain where RefIssueId =" + hdnIssueId.Value + " and IssueReturn='R' AND ProcessStatus ='P' AND Active=1 ");

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text + "&RetType=R";
            //}
            //else
            //{

            if (chktoprinter.Checked == true)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                string printURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "DirectPRintingURL", sConString));
                //BaseC.User valUser = new BaseC.User(sConString);
                string pagesetting = "";
                try
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    ds = dl.FillDataSet(CommandType.Text, "Exec uspGetprintPagesize 'ItemIssueSaleReturn.aspx','phrIssueSaleItem'");
                    pagesetting = ds.Tables[0].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }
                if (common.myStr(printURL) != "")
                {

                    str.Append("inyHospitalLocationID/");
                    str.Append(Session["HospitalLocationID"].ToString());
                    str.Append("!");
                    str.Append("intFacilityId/");
                    str.Append(Session["FacilityID"].ToString());
                    str.Append("!");
                    str.Append("intSaleSetupType/");
                    str.Append(common.myStr(common.myInt(ddlPatientType.SelectedValue)));
                    str.Append("!");
                    if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                    {
                        str.Append("intInvoiceId/");
                        str.Append(Request.QueryString["InvoiceId"]);
                    }
                    else
                    {
                        str.Append("intIssueId/");
                        str.Append(hdnIssueId.Value);
                    }
                    str.Append("!");
                    str.Append("Reportheader/");
                    str.Append(ddlPatientType.SelectedItem.Text.ToString());
                    str.Append("!");
                    str.Append("DoctorName/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()));
                    str.Append("!");
                    str.Append("User/");
                    str.Append(common.myStr(Session["UserName"]));
                    str.Append("!");
                    if (common.myStr(ddlPatientType.SelectedItem.Text).ToUpper() == "STAFF SALE")
                    {
                        str.Append("UHID/");
                        str.Append("Employee No.");
                    }
                    else
                    {
                        str.Append("UHID/");
                        str.Append(Resources.PRegistration.UHID);
                    }
                    string Str;
                    if (Request.QueryString["RetType"] == "R")
                    {
                        //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemReturn";
                        //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItemReturn$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemReturn$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                    }
                    else
                    {
                        if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                        {
                            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemForER";
                            //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItemForER$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                            Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemForER$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        }
                        else
                        {
                            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItem";
                            //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItem$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                            Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItem$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        }
                    }

                    //string Str = printURL + "http://" + reportServer + "/ReportServer$/EMRReports/OPDReceipt$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                    //f06092017b 
                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    IsRequiredDirectPrintTwoTimesOPSale = common.myStr(objBill.getHospitalSetupValue("IsRequiredDirectPrintTwoTimesOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (IsRequiredDirectPrintTwoTimesOPSale == "Y")
                    {
                        dvConfirm.Visible = true;
                    }
                    else
                    {
                        dvConfirm.Visible = false;
                    }
                    //f06092017e
                    return;
                }
            }
            else
            {
                RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text;
                //}
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }

        if (strpatienttype == "IP-ISS" && hdnIssueId.Value != "")
        {
            string rptName = "";
            if (common.myStr(Request.QueryString["IssueReturn"]) == "R")
            {
                if (common.myStr(Request.QueryString["SetupId"]) == "204")
                {
                    rptName = "EMERGENCY REFUND";
                }
                else
                {
                    rptName = "IP REFUND";
                }
            }
            else
            {
                if (lblPayertype.Text.ToUpper() == "CREDIT")
                {
                    rptName = "BILL / INVOICE (CREDIT)";
                }
                else
                {
                    rptName = "BILL / INVOICE";
                }
            }

            string StoreId = "0";
            if (common.myInt(Request.QueryString["StoreId"]) == 0)
                StoreId = common.myStr(ddlStore.SelectedValue);
            else
                StoreId = common.myStr(Request.QueryString["StoreId"]);

            if (chktoprinter.Checked == true)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                string printURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "DirectPRintingURL", sConString));
                //BaseC.User valUser = new BaseC.User(sConString);
                string pagesetting = "";
                try
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet ds = dl.FillDataSet(CommandType.Text, "Exec uspGetprintPagesize 'ItemIssueSaleReturn.aspx','IPSaleIssueDoc'");
                    pagesetting = ds.Tables[0].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }
                if (common.myStr(printURL) != "")
                {

                    str.Append("intIssueId/");
                    str.Append(hdnIssueId.Value);
                    str.Append("!");
                    str.Append("inyHospitalLocationId/");
                    str.Append(common.myStr(Session["HospitalLocationID"]));
                    str.Append("!");
                    str.Append("intFacilityId/");
                    str.Append(common.myStr(Session["FacilityId"]));
                    str.Append("!");
                    str.Append("intSaleSetupId/");
                    str.Append(common.myStr(common.myInt(ddlPatientType.SelectedValue)));
                    str.Append("!");
                    str.Append("intStoreId/");
                    str.Append(common.myStr(StoreId));
                    str.Append("!");
                    str.Append("UHID/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "RegNo").ToString()));
                    str.Append("!");
                    str.Append("chvSNo/");
                    str.Append(common.myStr(GetGlobalResourceObject("PRegistration", "SerialNo")));
                    str.Append("!");
                    str.Append("ReportHeaderName/");
                    str.Append(rptName);
                    str.Append("!");
                    str.Append("IPNO/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
                    str.Append("!");
                    str.Append("User/");
                    str.Append(common.myStr(Session["UserName"]));
                    str.Append("!");
                    str.Append("chrIssueReturn/");
                    str.Append(common.myStr("I"));
                    str.Append("!");
                    str.Append("intLoginFacilityId/");
                    str.Append(common.myStr(Session["FacilityId"]));
                    str.Append("!");
                    str.Append("chvOPIP/");
                    str.Append(common.myStr(Request.QueryString["OPIP"]) == "E" ? "E" : "I");

                    //string Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/IPSaleIssueDoc$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                    string Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/IPSaleIssueDoc$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable HshIn;
                    HshIn = new Hashtable();
                    HshIn.Add("@intIssueId", Convert.ToInt32(common.myInt(hdnIssueId.Value)));
                    objDl.ExecuteNonQuery(CommandType.Text, "update phripissuemain set documentprinted = 1 where issueid = @intIssueId", HshIn);

                    return;
                }
            }
            else
            {
                //RadWindow1.NavigateUrl = "~/Pharmacy/Reports/PrintReport.aspx?rptType=IPID&IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "";
                //RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&UseFor=DOC&IssueReturn=I";
                RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" 
                    + ddlPatientType.SelectedValue + "&UseFor=DOC&IssueReturn=I &StoreId=" + common.myStr(ddlStore.SelectedValue);
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn;
                HshIn = new Hashtable();
                HshIn.Add("@intIssueId", Convert.ToInt32(common.myInt(hdnIssueId.Value)));
                objDl.ExecuteNonQuery(CommandType.Text, "update phripissuemain set documentprinted = 1 where issueid = @intIssueId", HshIn);
            }
        }
    }
    protected void btnSearchByEmpNo_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        txtEncounter.Text = "";
        //done by rakesh for user authorisation start
        //btnSaveData.Visible = true;
        SetPermission(btnSaveData, "N", true);
        //btnPrint.Visible = false;
        SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        btnAddNewItem.Visible = true;
        //btndDiscount.Visible = true;
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        if (Request.QueryString["Code"] == "IPI")//Ipd sale
        {
            btndDiscount.Visible = true;
            lblDiscountType.Visible = false;
            ddlDiscountType.Visible = false;
            btnDiscountApply.Visible = false;
            lblDiscPerc.Visible = false;
            txtDiscPerc.Visible = false;
        }
        else
        {
            IsRequiredBLKDiscountPolicy = common.myStr(objBill.getHospitalSetupValue("IsRequiredBLKDiscountPolicy", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myStr(IsRequiredBLKDiscountPolicy) == "Y")
            {
                btndDiscount.Visible = false;
                lblDiscountType.Visible = true;
                ddlDiscountType.Visible = true;
                btnDiscountApply.Visible = true;
                lblDiscPerc.Visible = true;
                txtDiscPerc.Visible = true;
            }
            else
            {
                btndDiscount.Visible = true;
                lblDiscountType.Visible = false;
                ddlDiscountType.Visible = false;
                btnDiscountApply.Visible = false;
                lblDiscPerc.Visible = false;
                txtDiscPerc.Visible = false;
            }
        }
        if (common.myStr(hdnEmployeeId.Value) != "")
        {
            BindPatientDetails();
        }
    }
    protected void btnSearchByEncounterNo_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        txtRegistrationNo.Text = "";
        //done by rakesh for user authorisation start
        //btnSaveData.Visible = true;
        SetPermission(btnSaveData, "N", true);
        //btnPrint.Visible = false;
        SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        btnAddNewItem.Visible = true;
        //btndDiscount.Visible = true;

        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        if (Request.QueryString["Code"] == "IPI")//Ipd sale
        {
            btndDiscount.Visible = true;
            lblDiscountType.Visible = false;
            ddlDiscountType.Visible = false;
            btnDiscountApply.Visible = false;
            lblDiscPerc.Visible = false;
            txtDiscPerc.Visible = false;
        }
        else
        {
            IsRequiredBLKDiscountPolicy = common.myStr(objBill.getHospitalSetupValue("IsRequiredBLKDiscountPolicy", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myStr(IsRequiredBLKDiscountPolicy) == "Y")
            {
                btndDiscount.Visible = false;
                lblDiscountType.Visible = true;
                ddlDiscountType.Visible = true;
                btnDiscountApply.Visible = true;
                lblDiscPerc.Visible = true;
                txtDiscPerc.Visible = true;
            }
            else
            {
                btndDiscount.Visible = true;
                lblDiscountType.Visible = false;
                ddlDiscountType.Visible = false;
                btnDiscountApply.Visible = false;
                lblDiscPerc.Visible = false;
                txtDiscPerc.Visible = false;
            }
        }

        if (common.myStr(txtEncounter.Text) != "")
        {
            BindPatientDetails();
        }
    }
    protected void btnFindPatient_OnClick(object sender, EventArgs e)
    {

        string strpatienttype = "IP";
        string strRegEnc = "1";
        string OPIP = "I";
        if (common.myStr(ddlPatientType.SelectedItem.Attributes["StatusCode"]) == "OP-ISS")
        {
            strpatienttype = "OP";
            strRegEnc = "0";
            OPIP = "O";

        }
        if (Request.QueryString["ER"] != null)
        {
            strRegEnc = "4";
            strpatienttype = "ER";
            OPIP = "E";
        }
        clearControl();

        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?SalType=" + strpatienttype + "&RegEnc=" + strRegEnc;
        RadWindow1.Height = 600;
        RadWindow1.Width = 990;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }


    protected void chkCashOutStanding_OnCheckedChanged(object sender, EventArgs e)
    {
        Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
        foreach (GridDataItem gvr in grdPaymentMode.MasterTableView.Items)
        {
            //DropDownList ddlModeType = (DropDownList)gvr.FindControl("ddlMode");
            //DropDownList ddlBank = (DropDownList)gvr.FindControl("ddlBankName");
            //DropDownList ddlCredit = (DropDownList)gvr.FindControl("ddlCreditCard");
            //TextBox txtCheque = (TextBox)gvr.FindControl("txtChequeNo");
            //RadDatePicker txtChqDate = (RadDatePicker)gvr.FindControl("txtChequeDate");
            //DropDownList ddlClientBankName = (DropDownList)gvr.FindControl("ddlClientBankName");
            //HiddenField hdnPercentDiscount = (HiddenField)gvService.Rows[0].FindControl("hdnPercentDiscount");
            //TextBox txtTransactionRefNo = (TextBox)gvr.FindControl("txtTransactionRefNo");
            //HiddenField hdnTypeMappingCode = (HiddenField)gvr.FindControl("hdnTypeMappingCode");
            TextBox txtAmt = (TextBox)gvr.FindControl("txtAmount");
            //if (txtAmt.Text == "")
            txtAmt.Text = "0";

            txtReceived.Text = "0";
        }
    }


    //private void Print()
    //{
    //    string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
    //    if (strpatienttype == "OP-ISS" && hdnIssueId.Value != "")
    //    {

    //        RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text;
    //        RadWindow1.Height = 600;
    //        RadWindow1.Width = 900;
    //        RadWindow1.Top = 40;
    //        RadWindow1.Left = 100;
    //        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindow1.Modal = true;
    //        RadWindow1.VisibleStatusbar = false;
    //    }
    //    else if (hdnPageId.Value == "IPI" && hdnIssueId.Value != "")
    //    {
    //        RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&UseFor=DOC&IssueReturn=I";
    //        RadWindow1.Height = 600;
    //        RadWindow1.Width = 900;
    //        RadWindow1.Top = 40;
    //        RadWindow1.Left = 100;
    //        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindow1.Modal = true;
    //        RadWindow1.VisibleStatusbar = false;
    //    }
    //}
    private void Print()
    {
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();

        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(Convert.ToInt32(Session["UserID"]));

        if (strpatienttype == "OP-ISS" && hdnIssueId.Value != "")
        {
            if (chktoprinter.Checked == true)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                string printURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "DirectPRintingURL", sConString));
                //BaseC.User valUser = new BaseC.User(sConString);
                string pagesetting = "";
                try
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet ds = dl.FillDataSet(CommandType.Text, "Exec uspGetprintPagesize 'ItemIssueSaleReturn.aspx','phrIssueSaleItem'");
                    pagesetting = ds.Tables[0].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }
                if (common.myStr(printURL) != "")
                {

                    str.Append("inyHospitalLocationID/");
                    str.Append(Session["HospitalLocationID"].ToString());
                    str.Append("!");
                    str.Append("intFacilityId/");
                    str.Append(Session["FacilityID"].ToString());
                    str.Append("!");
                    str.Append("intSaleSetupType/");
                    str.Append(common.myStr(common.myInt(ddlPatientType.SelectedValue)));
                    str.Append("!");
                    if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                    {
                        str.Append("intInvoiceId/");
                        str.Append(Request.QueryString["InvoiceId"]);
                    }
                    else
                    {
                        str.Append("intIssueId/");
                        str.Append(common.myStr(common.myInt(hdnIssueId.Value)));
                    }
                    str.Append("!");
                    str.Append("Reportheader/");
                    str.Append(ddlPatientType.SelectedItem.Text.ToString());
                    str.Append("!");
                    str.Append("DoctorName/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()));
                    str.Append("!");
                    str.Append("User/");
                    str.Append(common.myStr(Session["UserName"]));
                    str.Append("!");
                    if (common.myStr(ddlPatientType.SelectedItem.Text).ToUpper() == "STAFF SALE")
                    {
                        str.Append("UHID/");
                        str.Append("Employee No.");
                    }
                    else
                    {
                        str.Append("UHID/");
                        str.Append(Resources.PRegistration.UHID);
                    }
                    string Str;
                    if (Request.QueryString["RetType"] == "R")
                    {
                        //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemReturn";
                        Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemReturn$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                    }
                    else
                    {
                        if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                        {
                            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemForER";
                            Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemForER$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        }
                        else
                        {
                            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItem";
                            Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItem$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        }
                    }

                    //string Str = printURL + "http://" + reportServer + "/ReportServer$/EMRReports/OPDReceipt$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                    //f06092017b-- 
                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    IsRequiredDirectPrintTwoTimesOPSale = common.myStr(objBill.getHospitalSetupValue("IsRequiredDirectPrintTwoTimesOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (IsRequiredDirectPrintTwoTimesOPSale == "Y")
                    {
                        dvConfirm.Visible = true;
                    }
                    else
                    {
                        dvConfirm.Visible = false;
                    }
                    //f06092017e--
                    return;
                }
            }
            else
            {

                RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text;
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }
        else if (hdnPageId.Value == "IPI" && hdnIssueId.Value != "")
        {
            string rptName = "";
            if (common.myStr(Request.QueryString["IssueReturn"]) == "R")
            {
                if (common.myStr(Request.QueryString["SetupId"]) == "204")
                {
                    rptName = "EMERGENCY REFUND";
                }
                else
                {
                    rptName = "IP REFUND";
                }
            }
            else
            {
                rptName = "BILL / INVOICE";
            }

            string StoreId = "0";
            if (common.myInt(Request.QueryString["StoreId"]) == 0)
                StoreId = common.myStr(ddlStore.SelectedValue);
            else
                StoreId = common.myStr(Request.QueryString["StoreId"]);

            if (chktoprinter.Checked == true)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                string printURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "DirectPRintingURL", sConString));
                //BaseC.User valUser = new BaseC.User(sConString);
                string pagesetting = "";
                try
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet ds = dl.FillDataSet(CommandType.Text, "Exec uspGetprintPagesize 'ItemIssueSaleReturn.aspx','IPSaleIssueDoc'");
                    pagesetting = ds.Tables[0].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }
                if (common.myStr(printURL) != "")
                {
                    str.Append("intIssueId/");
                    str.Append(common.myStr(common.myInt(hdnIssueId.Value)));
                    str.Append("!");
                    str.Append("inyHospitalLocationId/");
                    str.Append(common.myStr(Session["HospitalLocationID"]));
                    str.Append("!");
                    str.Append("intFacilityId/");
                    str.Append(common.myStr(Session["FacilityId"]));
                    str.Append("!");
                    str.Append("intSaleSetupId/");
                    str.Append(common.myStr(common.myInt(ddlPatientType.SelectedValue)));
                    str.Append("!");
                    str.Append("intStoreId/");
                    str.Append(common.myStr(StoreId));
                    str.Append("!");
                    str.Append("UHID/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "RegNo").ToString()));
                    str.Append("!");
                    str.Append("chvSNo/");
                    str.Append(common.myStr(GetGlobalResourceObject("PRegistration", "SerialNo")));
                    str.Append("!");
                    str.Append("ReportHeaderName/");
                    str.Append(rptName);
                    str.Append("!");
                    str.Append("IPNO/");
                    str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "ipno").ToString()));
                    str.Append("!");
                    str.Append("User/");
                    str.Append(common.myStr(Session["UserName"]));
                    str.Append("!");
                    str.Append("chrIssueReturn/");
                    str.Append(common.myStr("I"));
                    str.Append("!");
                    str.Append("intLoginFacilityId/");
                    str.Append(common.myStr(Session["FacilityId"]));
                    str.Append("!");
                    str.Append("chvOPIP/");
                    str.Append(common.myStr(Request.QueryString["OPIP"]) == "E" ? "E" : "I");

                    string Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/IPSaleIssueDoc$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

                    return;
                }
            }
            else
            {
                RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&UseFor=DOC&IssueReturn=I";
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
        }
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (strpatienttype == "OP-ISS" && hdnIssueId.Value != "")
        {
            //f06092017b
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            IsRequiredDirectPrintTwoTimesOPSale = common.myStr(objBill.getHospitalSetupValue("IsRequiredDirectPrintTwoTimesOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (IsRequiredDirectPrintTwoTimesOPSale == "Y")
            {

                if (chktoprinter.Checked == true)
                {

                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    string printURL = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "DirectPRintingURL", sConString));
                    //BaseC.User valUser = new BaseC.User(sConString);
                    string pagesetting = "";
                    try
                    {
                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        DataSet ds = new DataSet();
                        ds = dl.FillDataSet(CommandType.Text, "Exec uspGetprintPagesize 'ItemIssueSaleReturn.aspx','phrIssueSaleItem'");
                        pagesetting = ds.Tables[0].Rows[0][0].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (common.myStr(printURL) != "")
                    {

                        str.Append("inyHospitalLocationID/");
                        str.Append(Session["HospitalLocationID"].ToString());
                        str.Append("!");
                        str.Append("intFacilityId/");
                        str.Append(Session["FacilityID"].ToString());
                        str.Append("!");
                        str.Append("intSaleSetupType/");
                        str.Append(common.myStr(common.myInt(ddlPatientType.SelectedValue)));
                        str.Append("!");
                        if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                        {
                            str.Append("intInvoiceId/");
                            str.Append(Request.QueryString["InvoiceId"]);
                        }
                        else
                        {
                            str.Append("intIssueId/");
                            str.Append(hdnIssueId.Value);
                        }
                        str.Append("!");
                        str.Append("Reportheader/");
                        str.Append(ddlPatientType.SelectedItem.Text.ToString());
                        str.Append("!");
                        str.Append("DoctorName/");
                        str.Append(Convert.ToString(HttpContext.GetGlobalResourceObject("PRegistration", "Doctor").ToString()));
                        str.Append("!");
                        str.Append("User/");
                        str.Append(common.myStr(Session["UserName"]));
                        str.Append("!");
                        if (common.myStr(ddlPatientType.SelectedItem.Text).ToUpper() == "STAFF SALE")
                        {
                            str.Append("UHID/");
                            str.Append("Employee No.");
                        }
                        else
                        {
                            str.Append("UHID/");
                            str.Append(Resources.PRegistration.UHID);
                        }
                        string Str;
                        if (Request.QueryString["RetType"] == "R")
                        {
                            //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemReturn";
                            //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItemReturn$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                            Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemReturn$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                        }
                        else
                        {
                            if (common.myInt(Request.QueryString["InvoiceId"]) > 0)
                            {
                                //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItemForER";
                                //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItemForER$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                                Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItemForER$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                            }
                            else
                            {
                                //ReportViewer1.ServerReport.ReportPath = "/EMRReports/phrIssueSaleItem";
                                //Str = printURL + "http://" + reportServer + "/ReportServer$/MotherVersionReports/phrIssueSaleItem$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                                Str = printURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/phrIssueSaleItem$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;
                            }
                        }

                        //string Str = printURL + "http://" + reportServer + "/ReportServer$/EMRReports/OPDReceipt$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + pagesetting;

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                        //f06092017b 
                        dvConfirm.Visible = false;
                        //f06092017e
                        return;
                    }
                }
            }
            else
            {
                RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text;
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
            //f06092017e

            //f06092017bcomment
            //RadWindow1.NavigateUrl = "~/Pharmacy/Reports/CashSaleReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&seltype=" + common.myStr(txtpatientaatributes.Text) + "&RptHeader=" + ddlPatientType.SelectedItem.Text;
            //RadWindow1.Height = 600;
            //RadWindow1.Width = 900;
            //RadWindow1.Top = 40;
            //RadWindow1.Left = 100;
            //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //RadWindow1.Modal = true;
            //RadWindow1.VisibleStatusbar = false;
            //f06092017ecomment
        }
        else if (hdnPageId.Value == "IPI" && hdnIssueId.Value != "")
        {
            RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&SetupId=" + ddlPatientType.SelectedValue + "&UseFor=DOC&IssueReturn=I";
            RadWindow1.Height = 600;
            RadWindow1.Width = 900;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirm.Visible = false;
        hdnIssueId.Value = "0";
        clearControl();
    }
    protected void btnLabelPrintYes_OnClick(object sender, EventArgs e)
    {
        string sOPIP = "O";
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (strpatienttype == "IP-ISS")
        {
            sOPIP = "I";
        }

        RadWindow1.NavigateUrl = "/EMR/Medication/MedicationLabelPrint.aspx?OPIP=" + sOPIP + "&IndentId=" + hdnIssueId.Value;
        RadWindow1.Height = 580;
        RadWindow1.Width = 830;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        // RadWindow1.OnClientClose = "OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        dvLabelPrint.Visible = false;
        divBarcode.Visible = true;
    }
    protected void btnLabelPrintCancel_OnClick(object sender, EventArgs e)
    {
        dvLabelPrint.Visible = false;
        divBarcode.Visible = true;

    }

    protected void btnCancel1_OnClick(object sender, EventArgs e)
    {
        divBarcode.Visible = false;
        hdnIssueId.Value = "0";
        clearControl();
    }
    protected void btnWarningCancel_OnClick(object sender, EventArgs e)
    {
        divWarnigMessage.Visible = false;
    }
    /******************************************/
    protected void btnRequestFromWard_OnClick(object sender, EventArgs e)
    {
        if (Request.QueryString["Code"] != "IPI")
        {
            if (divWarnigMessage.Visible == true)
            {
                hdnSelectedGenericId.Value = "";
                hdnSelectedItemId.Value = "";
                RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientRequestFromWard.aspx";
                RadWindow1.Height = 620;
                RadWindow1.Width = 1200;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = "SearchPatientWardOnClientClose";
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                divWarnigMessage.Visible = false;
            }
            else
            {
                divWarnigMessage.Visible = true;
            }
        }
        else
        {
            hdnSelectedGenericId.Value = "";
            hdnSelectedItemId.Value = "";
            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientRequestFromWard.aspx";
            RadWindow1.Height = 620;
            RadWindow1.Width = 1200;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.OnClientClose = "SearchPatientWardOnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        }
    }
    protected void lnkLabelPrint_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(hdnIssueId.Value) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Enter Issue No.";
            return;

        }
        string sOPIP = "O";
        string strpatienttype = ddlPatientType.SelectedItem.Attributes["StatusCode"].ToString().Trim();
        if (strpatienttype == "IP-ISS")
        {
            sOPIP = "I";
        }

        //f270717b
        RadWindow1.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(hdnIssueId.Value) + "&UseFor=LabelPrintIP";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        //f270717e
        //f270717bcomment

        //RadWindow1.NavigateUrl = "/EMR/Medication/MedicationLabelPrint.aspx?OPIP=" + sOPIP + "&IndentId=" + hdnIssueId.Value;
        //RadWindow1.Height = 580;
        //RadWindow1.Width = 830;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        //// RadWindow1.OnClientClose = "OnClientClose";
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;

        //f270717ecomment
    }

    // add by Balkishan Start
    void IPDER(string IR, string Code, string ER)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        objCIMS = new clsCIMS();

        if (common.myStr(Code) != "")
        {
            hdnPageId.Value = common.myStr(Code);
        }
        if (common.myStr(IR) == "S"
             && common.myStr(Code) == "CSI")
        {
            tblPrescription.Visible = true;
        }
        dvConfirmProfileItem.Visible = false;
        btnAddNewItem.Visible = true;
        ddlPatientType.Visible = true;
        dvConfirm.Visible = false;
        ViewState["RequestFromWardItems"] = null;
        hdnSelectedGenericId.Value = "";
        hdnSelectedItemId.Value = "";
        //done by rakesh for user authorisation start
        //btnPrint.Visible = false;
        SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Font.Bold = commonLabelSetting.cBold;
        if (commonLabelSetting.cFont != "")
        {
            lblMessage.Font.Name = commonLabelSetting.cFont;
        }
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        //  hdnDecimalPlaces.Value = common.myStr(Cache["DecimalPlace"]);
        hdnDecimalPlaces.Value = common.myStr(objBill.getHospitalSetupValue("DecimalPlaces", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
        if (common.myInt(hdnDecimalPlaces.Value) == 0)
        {
            hdnDecimalPlaces.Value = "2";
        }
        #region Interface

        if (common.myStr(IR) == "S"
             && (common.myStr(Code) == "CSI")
                || (common.myStr(Code) == "IPI"))
        {
            setPatientInfo();
            getLegnedColor();

            ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
            ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");
            objEMR = new BaseC.clsEMR(sConString);
            DataSet dsInterface = new DataSet();
            if (common.myStr(Code) == "CSI")//op sale
            {
                dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForOPSale);
            }
            else if (common.myStr(Code) == "IPI")//ip issue
            {
                dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForIPIssue);
            }
            ViewState["IsCIMSInterfaceActive"] = false;
            ViewState["IsVIDALInterfaceActive"] = false;
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

                if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc"))
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
                    //objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");

                    WebClient client = new WebClient();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sVidalConString + "DocumentService");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "VIDAL web-services not running now !";

                        //Alert.ShowAjaxMsg(lblMessage.Text, this);

                        return;
                    }
                }
                catch
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "VIDAL web-services not running now !";

                    //Alert.ShowAjaxMsg(lblMessage.Text, this);

                    return;
                }
            }
        }

        #endregion
        clearControl();
        BindDoctor();

        dtpDocDate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy"));
        hdnItemIssueId.Value = "0";
        bindPatientTypeDDL();
        InsurancecompanyBind("", 0);
        hdnDefaultCompanyId.Value = common.myInt(objBill.getDefaultCompany(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))).ToString();
        ddlPatientType.SelectedIndex = 0;
        ddlPatientType_SelectedIndexChanged(null, null);
        if (common.myStr(Request.QueryString["RegNo"]) != "")
        {
            txtRegistrationNo.Text = common.myStr(Request.QueryString["RegNo"]);
            btnSearchByUHID_OnClick(this, null);
        }
        Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
        RadTabStrip1.Tabs[0].Visible = true;
        if (Code == "IPI")//Ipd sale
        {
            lblForIp.Visible = true;
            lblAgeStar.Visible = true;
            btnRequestFromWard.Visible = true;
            btndDiscount.Visible = false;
            txtProvider.Visible = false;
            chkDoctor.Visible = false;
            ddlDoctor.Visible = true;
            ViewState["TransactionType"] = "IPISS";
            txtReceived.Visible = false;
            Literal4.Visible = false;
            Label1.Visible = false;
            txtAmountCollected.Visible = false;
            Label22.Visible = false;
            txtRefundamount.Visible = false;
            //btnSurgicalKit.Visible = true;
            grdPaymentMode.Visible = false;
            RadTabStrip1.Tabs[1].Visible = false;
            rpvPayment.Visible = false;


        }
        else // op sale
        {
            //lblForIp.Visible = false;
            if (hdnIsDoctorMandatoryinOPSale.Value == "Y")
            {
                lblForIp.Visible = true;
            }
            else
            {
                lblForIp.Visible = false;
            }
            if (hdnIsAgeMandatoryinOPSale.Value == "Y")
            {
                lblAgeStar.Visible = true;
            }
            else
            {
                lblAgeStar.Visible = false;
            }
            btnRequestFromWard.Visible = false;
            lblBedNo.Visible = false;
            txtBedNo.Visible = false;
            chkDoctor.Visible = true;
            ViewState["TransactionType"] = "OPISS";
            ddlDoctor.Visible = true;
            txtReceived.Visible = true;
            Literal4.Visible = true;
            Label1.Visible = true;
            txtAmountCollected.Visible = true;
            Label22.Visible = true;
            txtRefundamount.Visible = true;
            BindStaffCompanyId();
            //btnSurgicalKit.Visible = false;
            btnPendingRequestFromWard.Visible = false;
            grdPaymentMode.Visible = true;
            RadTabStrip1.Tabs[1].Visible = true;
            rpvPayment.Visible = true;
            lnkBtnPrescription.Enabled = false;

        }
        ddlPayer_OnSelectedIndexChanged(null, null);
        BindDoctor();
        txtRegistrationNo.Focus();

        if (common.myStr(IR) == "S"
             && common.myStr(Code) == "IPI")
        {
            tblMarkedForDischarge.Visible = true;
            NoofMarkfordischare();
        }

    }
    // add by Balkishan End

    protected DataTable CreateTableXML()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("GenericId");
        dt.Columns.Add("ItemId");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("ExpiryDate");
        dt.Columns.Add("StockQty");
        dt.Columns.Add("Qty");
        dt.Columns.Add("CostPrice");
        dt.Columns.Add("SalePrice");
        dt.Columns.Add("MRP");
        dt.Columns.Add("DiscAmtPercent");
        dt.Columns.Add("Tax");
        dt.Columns.Add("DisAmt");
        dt.Columns.Add("NetAmt");


        //DataRow dr = dt.NewRow();

        //dr["GenericId"] = DBNull.Value;
        //dr["ItemId"] = DBNull.Value;
        //dr["ItemName"] = DBNull.Value;
        //dr["BatchId"] = DBNull.Value;
        //dr["BatchNo"] = DBNull.Value;
        //dr["ExpiryDate"] = DBNull.Value;
        //dr["StockQty"] = DBNull.Value;
        //dr["Qty"] = DBNull.Value;
        //dr["CostPrice"] = DBNull.Value;
        //dr["SalePrice"] = DBNull.Value;
        //dr["MRP"] = DBNull.Value;
        //dr["DiscAmtPercent"] = DBNull.Value;
        //dr["Tax"] = DBNull.Value;
        //dr["DisAmt"] = DBNull.Value;
        //dr["NetAmt"] = DBNull.Value;

        //dt.Rows.Add(dr);

        return dt;
    }
    void bindPatientWardRequest()
    {
        try
        {
            IsWardIndentfromOPSale = "N";
            if (common.myInt(hdnIndentId.Value) == 0)
            {
                return;
            }

            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();
            int NoStockItemCount = 0;

            //Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
            //ds = objPharmacy.getIPPatientRequest(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            //                    0, 0, "", "", "", "", common.myDate("1753-01-01"), common.myDate("2099-12-31"), common.myInt(Session["UserId"]), 0,"");
            ds = objPharmacy.getIPPatientRequestInv(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                               0, 0, 0, "", "", "", common.myDate("1753-01-01"), common.myDate("2099-12-31"), common.myInt(Session["UserId"]), 0, "",
                               common.myInt(ddlStore.SelectedValue), string.Empty, "A");//255 means all patient

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "Patient not found !";
                    return;
                }

                DataRow DR = ds.Tables[0].Rows[0];
                rwIndent.Visible = true;
                lblIndentNo.Text = common.myStr(DR["IndentNo"]);
                // add by Balkishan Start
                if (common.myStr(Request.QueryString["Code"]) == "IPI")
                {
                    int intType = common.myInt(DR["Type"]);
                    string IR = "", Code = "", ER = "";

                    if (intType == 1 || intType == 2)
                    {
                        IR = "S"; Code = "CSI"; ER = "True";
                        IPDER(IR, Code, ER);
                        ViewState["IsPrescriptionHelpSearch"] = true;
                        btnAddNewItem.Visible = false;
                        btnRequestFromWard.Visible = true;
                        btnPendingRequestFromWard.Visible = true;
                        btnPrescriptionDetails_OnClick(null, null);
                    }
                    else if (intType == 3)
                    {
                        IR = "S"; Code = "IPI"; ER = "False";
                        IPDER(IR, Code, ER);
                        btnAddNewItem.Visible = false;
                        btnRequestFromWard.Visible = true;
                        btnPendingRequestFromWard.Visible = true;
                        //Start

                        txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
                        txtEncounter.Text = common.myStr(DR["EncounterNo"]);
                        txtPatientName.Text = common.myStr(DR["PatientName"]);
                        txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
                        // txtProvider.Text = common.myStr(DR["RequestedBy"]);
                        if (txtEncounter.Text != "")
                        {
                            if (objPharmacy.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                            {
                                lblPackagePatient.Visible = true;
                            }
                            else
                            {
                                lblPackagePatient.Visible = false;
                            }

                        }
                        ddlDoctor.SelectedValue = common.myStr(DR["AdvisingDoctorId"]);
                        ddlDoctor.Enabled = false;
                        ddlDoctor.Visible = true;
                        txtProvider.Visible = false;
                        //ViewState["AdvisingDoctorId"] = common.myStr(DR["AdvisingDoctorId"]);
                        if (common.myStr(DR["CompanyCode"]) != "0" && common.myStr(DR["CompanyCode"]) != "")
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
                        }
                        else
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                        }
                        ShowCashCredit();
                        ddlPayer_OnSelectedIndexChanged(null, null);
                        lblPayer.Text = ddlPayer.SelectedItem.Text;
                        txtBedNo.Text = common.myStr(DR["BedNo"]);

                        lblWardName.Text = ds.Tables[0].Rows[0]["WardName"].ToString().Trim();
                        lblBedNo1.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();


                        if (common.myStr(DR["GenderAge"]) != "")
                        {
                            txtAge.Text = common.myStr(DR["GenderAge"]);

                            string[] agetype = common.myStr(DR["GenderAge"]).Split(' ');
                            if (agetype[1] != "")
                            {
                                ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                            }
                            txtAge.Text = agetype[0];
                        }
                        ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(DR["Gender"])));

                        hdnTotQty.Value = "0";
                        hdnTotUnit.Value = "0";
                        hdnTotCharge.Value = "0";
                        hdnTotTax.Value = "0";
                        hdnTotDiscAmt.Value = "0";
                        hdnTotPatientAmt.Value = "0";
                        hdnTotPayerAmt.Value = "0";
                        hdnTotNetAmt.Value = "0";

                        DataSet dsItem = new DataSet();
                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        if (common.myStr(objBill.getHospitalSetupValue("IsRequiredAutoBatchFillinginIPIssue", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                        {
                            if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                            {

                                dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "A", common.myInt(Session["FacilityId"]));
                            }
                            else
                            {
                                dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "O", common.myInt(Session["FacilityId"]));
                            }

                            for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
                            {
                                DataTable dtgrd = new DataTable();
                                dtgrd = CreateTableXML();
                                if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) > 0)
                                {
                                    DataRow DR1;
                                    DR1 = dsItem.Tables[0].Rows[rIdx];

                                    DataRow dRow = dtgrd.NewRow();
                                    dRow["GenericId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["GenericId"]);
                                    dRow["ItemId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["ItemId"]);
                                    dRow["ItemName"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["ItemName"]);
                                    dRow["BatchId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["BatchId"]);
                                    dRow["BatchNo"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["BatchNo"]);
                                    dRow["ExpiryDate"] = dsItem.Tables[0].Rows[rIdx]["ExpiryDate"];//common.myDate(common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim())).ToString("yyyy-MM-dd");
                                    dRow["StockQty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["StockQty"]);
                                    dRow["Qty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Qty"]);
                                    dRow["SalePrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["SalePrice"]);
                                    dRow["MRP"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["MRP"]);
                                    dRow["CostPrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["CostPrice"]);
                                    dRow["DiscAmtPercent"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DiscAmtPercent"]);
                                    dRow["DisAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DisAmt"]);
                                    dRow["Tax"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Tax"]);
                                    dRow["NetAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["NetAmt"]);

                                    dtgrd.Rows.Add(dRow);

                                    DataTable dt = new DataTable();
                                    dt = dtgrd;
                                    DataSet dss = new DataSet();
                                    dss.Tables.Add(dt.Copy());

                                    System.Text.StringBuilder builder = new System.Text.StringBuilder();

                                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);

                                    dss.WriteXml(writer);

                                    string xmlSchema = writer.ToString();
                                    DR1["BatchXML"] = xmlSchema;

                                    dsItem.Tables[0].AcceptChanges();

                                }
                                if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) == 0)
                                {
                                    NoStockItemCount += 1;
                                }
                            }
                            ViewState["Servicetable"] = dsItem.Tables[0];
                        }
                        else
                        {
                            if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                            {

                                dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), "A", common.myInt(Session["FacilityId"]));
                            }
                            else
                            {
                                dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                            }
                        }



                        if (!dsItem.Tables[0].Columns.Contains("ISDHAApproved"))
                            dsItem.Tables[0].Columns.Add("ISDHAApproved");
                        if (!dsItem.Tables[0].Columns.Contains("DenialCode"))
                            dsItem.Tables[0].Columns.Add("DenialCode");

                        dsItem.Tables[0].AcceptChanges();

                        ViewState["RequestFromWardItems"] = null;


                        if (dsItem.Tables[0].Rows.Count == 0)
                        {
                            DataRow newdr = dsItem.Tables[0].NewRow();
                            dsItem.Tables[0].Rows.Add(newdr);
                        }
                        else
                        {
                            ViewState["RequestFromWardItems"] = dsItem.Tables[0];
                            ViewState["Servicetable"] = dsItem.Tables[0];
                        }
                        //RequestedWardItemsWithSchema(dsItem);
                        Session["IsFromRequestFromWard"] = 0;
                        Session["ForBLKDiscountPolicy"] = 0;
                        gvService.DataSource = dsItem.Tables[0];
                        gvService.DataBind();

                        setVisiblilityInteraction();

                        txtRegistrationNo.Enabled = false;
                        txtEncounter.Enabled = false;
                        txtPatientName.Enabled = false;
                        txtAge.Enabled = false;
                        ViewState["EMRPrescription"] = false;

                        //End

                    }


                }
                else
                {

                    //Add by Tony start
                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    if (common.myStr(objBill.getHospitalSetupValue("isDisplayRequestFromWardinOPSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                    {
                        int intType = common.myInt(DR["Type"]);
                        string IR = "", Code = "", ER = "";

                        if (intType == 1 || intType == 2)
                        {
                            IR = "S"; Code = "CSI"; ER = "True";
                            IPDER(IR, Code, ER);
                            ViewState["IsPrescriptionHelpSearch"] = true;
                            btnAddNewItem.Visible = false;
                            btnRequestFromWard.Visible = true;
                            btnPendingRequestFromWard.Visible = true;
                            btnPrescriptionDetails_OnClick(null, null);
                        }
                        else if (intType == 3)
                        {
                            IR = "S"; Code = "CSI"; ER = "False";
                            IPDER(IR, Code, ER);
                            btnAddNewItem.Visible = false;
                            btnRequestFromWard.Visible = true;
                            btnPendingRequestFromWard.Visible = false;

                            txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
                            txtEncounter.Text = common.myStr(DR["EncounterNo"]);
                            txtPatientName.Text = common.myStr(DR["PatientName"]);
                            txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
                            // txtProvider.Text = common.myStr(DR["RequestedBy"]);

                            if (txtEncounter.Text != "")
                            {
                                if (objPharmacy.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                                {
                                    lblPackagePatient.Visible = true;
                                }
                                else
                                {
                                    lblPackagePatient.Visible = false;
                                }

                            }

                            ddlDoctor.SelectedValue = common.myStr(DR["AdvisingDoctorId"]);
                            ddlDoctor.Enabled = false;
                            ddlDoctor.Visible = true;
                            txtProvider.Visible = false;
                            //ViewState["AdvisingDoctorId"] = common.myStr(DR["AdvisingDoctorId"]);
                            if (common.myStr(DR["CompanyCode"]) != "0" && common.myStr(DR["CompanyCode"]) != "")
                            {
                                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
                            }
                            else
                            {
                                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                            }
                            ddlPayer_OnSelectedIndexChanged(null, null);
                            lblPayer.Text = ddlPayer.SelectedItem.Text;
                            txtBedNo.Text = common.myStr(DR["BedNo"]);

                            if (common.myStr(DR["GenderAge"]) != "")
                            {
                                txtAge.Text = common.myStr(DR["GenderAge"]);

                                string[] agetype = common.myStr(DR["GenderAge"]).Split(' ');
                                if (agetype[1] != "")
                                {
                                    ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                                }
                                txtAge.Text = agetype[0];
                            }
                            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(DR["Gender"])));

                            hdnTotQty.Value = "0";
                            hdnTotUnit.Value = "0";
                            hdnTotCharge.Value = "0";
                            hdnTotTax.Value = "0";
                            hdnTotDiscAmt.Value = "0";
                            hdnTotPatientAmt.Value = "0";
                            hdnTotPayerAmt.Value = "0";
                            hdnTotNetAmt.Value = "0";

                            DataSet dsItem = new DataSet();
                            //dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

                            if (common.myStr(objBill.getHospitalSetupValue("IsRequiredAutoBatchFillinginIPIssue", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                            {
                                if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                                {

                                    dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "A", common.myInt(Session["FacilityId"]));
                                }
                                else
                                {
                                    dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "O", common.myInt(Session["FacilityId"]));
                                }

                                for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
                                {
                                    DataTable dtgrd = new DataTable();
                                    dtgrd = CreateTableXML();
                                    if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) > 0)
                                    {
                                        DataRow DR1;
                                        DR1 = dsItem.Tables[0].Rows[rIdx];

                                        DataRow dRow = dtgrd.NewRow();
                                        dRow["GenericId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["GenericId"]);
                                        dRow["ItemId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["ItemId"]);
                                        dRow["ItemName"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["ItemName"]);
                                        dRow["BatchId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["BatchId"]);
                                        dRow["BatchNo"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["BatchNo"]);
                                        dRow["ExpiryDate"] = dsItem.Tables[0].Rows[rIdx]["ExpiryDate"];//common.myDate(common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim())).ToString("yyyy-MM-dd");
                                        dRow["StockQty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["StockQty"]);
                                        dRow["Qty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Qty"]);
                                        dRow["SalePrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["SalePrice"]);
                                        dRow["MRP"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["MRP"]);
                                        dRow["CostPrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["CostPrice"]);
                                        dRow["DiscAmtPercent"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DiscAmtPercent"]);
                                        dRow["DisAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DisAmt"]);
                                        dRow["Tax"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Tax"]);
                                        dRow["NetAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["NetAmt"]);

                                        dtgrd.Rows.Add(dRow);

                                        DataTable dt = new DataTable();
                                        dt = dtgrd;
                                        DataSet dss = new DataSet();
                                        dss.Tables.Add(dt.Copy());

                                        System.Text.StringBuilder builder = new System.Text.StringBuilder();

                                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);

                                        dss.WriteXml(writer);

                                        string xmlSchema = writer.ToString();
                                        DR1["BatchXML"] = xmlSchema;

                                        dsItem.Tables[0].AcceptChanges();

                                    }
                                    if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) == 0)
                                    {
                                        NoStockItemCount += 1;
                                    }
                                }
                                ViewState["Servicetable"] = dsItem.Tables[0];
                            }
                            else
                            {
                                if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                                {

                                    dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), "A", common.myInt(Session["FacilityId"]));
                                }
                                else
                                {
                                    dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                                }
                            }


                            if (!dsItem.Tables[0].Columns.Contains("ISDHAApproved"))
                                dsItem.Tables[0].Columns.Add("ISDHAApproved");
                            if (!dsItem.Tables[0].Columns.Contains("DenialCode"))
                                dsItem.Tables[0].Columns.Add("DenialCode");

                            dsItem.Tables[0].AcceptChanges();

                            ViewState["RequestFromWardItems"] = null;


                            if (dsItem.Tables[0].Rows.Count == 0)
                            {
                                DataRow newdr = dsItem.Tables[0].NewRow();
                                dsItem.Tables[0].Rows.Add(newdr);
                            }
                            else
                            {
                                ViewState["RequestFromWardItems"] = dsItem.Tables[0];
                                ViewState["Servicetable"] = dsItem.Tables[0];
                            }
                            //RequestedWardItemsWithSchema(dsItem);
                            Session["IsFromRequestFromWard"] = 0;
                            Session["ForBLKDiscountPolicy"] = 0;
                            gvService.DataSource = dsItem.Tables[0];
                            gvService.DataBind();

                            setVisiblilityInteraction();

                            txtRegistrationNo.Enabled = false;
                            txtEncounter.Enabled = false;
                            txtPatientName.Enabled = false;
                            txtAge.Enabled = false;
                            IsWardIndentfromOPSale = "Y";
                        }
                    }
                    //Add by Tony End
                    else
                    {
                        txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
                        txtEncounter.Text = common.myStr(DR["EncounterNo"]);
                        txtPatientName.Text = common.myStr(DR["PatientName"]);
                        txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
                        // txtProvider.Text = common.myStr(DR["RequestedBy"]);


                        if (txtEncounter.Text != "")
                        {
                            if (objPharmacy.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                            {
                                lblPackagePatient.Visible = true;
                            }
                            else
                            {
                                lblPackagePatient.Visible = false;
                            }

                        }

                        ddlDoctor.SelectedValue = common.myStr(DR["AdvisingDoctorId"]);
                        ddlDoctor.Enabled = false;
                        ddlDoctor.Visible = true;
                        txtProvider.Visible = false;
                        //ViewState["AdvisingDoctorId"] = common.myStr(DR["AdvisingDoctorId"]);
                        if (common.myStr(DR["CompanyCode"]) != "0" && common.myStr(DR["CompanyCode"]) != "")
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
                        }
                        else
                        {
                            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
                        }
                        ddlPayer_OnSelectedIndexChanged(null, null);
                        lblPayer.Text = ddlPayer.SelectedItem.Text;
                        txtBedNo.Text = common.myStr(DR["BedNo"]);

                        if (common.myStr(DR["GenderAge"]) != "")
                        {
                            txtAge.Text = common.myStr(DR["GenderAge"]);

                            string[] agetype = common.myStr(DR["GenderAge"]).Split(' ');
                            if (agetype[1] != "")
                            {
                                ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                            }
                            txtAge.Text = agetype[0];
                        }
                        ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(DR["Gender"])));

                        hdnTotQty.Value = "0";
                        hdnTotUnit.Value = "0";
                        hdnTotCharge.Value = "0";
                        hdnTotTax.Value = "0";
                        hdnTotDiscAmt.Value = "0";
                        hdnTotPatientAmt.Value = "0";
                        hdnTotPayerAmt.Value = "0";
                        hdnTotNetAmt.Value = "0";

                        DataSet dsItem = new DataSet();
                        //dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

                        if (common.myStr(objBill.getHospitalSetupValue("IsRequiredAutoBatchFillinginIPIssue", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                        {
                            if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                            {

                                dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "A", common.myInt(Session["FacilityId"]));
                            }
                            else
                            {
                                dsItem = objPharmacy.getIPPatientItemDetailsAuto(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), "O", common.myInt(Session["FacilityId"]));
                            }

                            for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
                            {
                                DataTable dtgrd = new DataTable();
                                dtgrd = CreateTableXML();
                                if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) > 0)
                                {
                                    DataRow DR1;
                                    DR1 = dsItem.Tables[0].Rows[rIdx];

                                    DataRow dRow = dtgrd.NewRow();
                                    dRow["GenericId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["GenericId"]);
                                    dRow["ItemId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["ItemId"]);
                                    dRow["ItemName"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["ItemName"]);
                                    dRow["BatchId"] = common.myInt(dsItem.Tables[0].Rows[rIdx]["BatchId"]);
                                    dRow["BatchNo"] = common.myStr(dsItem.Tables[0].Rows[rIdx]["BatchNo"]);
                                    dRow["ExpiryDate"] = dsItem.Tables[0].Rows[rIdx]["ExpiryDate"];//common.myDate(common.myStr(((Label)gvRow.FindControl("lblExpiryDate")).Text.Trim())).ToString("yyyy-MM-dd");
                                    dRow["StockQty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["StockQty"]);
                                    dRow["Qty"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Qty"]);
                                    dRow["SalePrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["SalePrice"]);
                                    dRow["MRP"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["MRP"]);
                                    dRow["CostPrice"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["CostPrice"]);
                                    dRow["DiscAmtPercent"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DiscAmtPercent"]);
                                    dRow["DisAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["DisAmt"]);
                                    dRow["Tax"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["Tax"]);
                                    dRow["NetAmt"] = common.myDbl(dsItem.Tables[0].Rows[rIdx]["NetAmt"]);

                                    dtgrd.Rows.Add(dRow);

                                    DataTable dt = new DataTable();
                                    dt = dtgrd;
                                    DataSet dss = new DataSet();
                                    dss.Tables.Add(dt.Copy());

                                    System.Text.StringBuilder builder = new System.Text.StringBuilder();

                                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);

                                    dss.WriteXml(writer);

                                    string xmlSchema = writer.ToString();
                                    DR1["BatchXML"] = xmlSchema;

                                    dsItem.Tables[0].AcceptChanges();

                                }
                                if (common.myInt(dsItem.Tables[0].Rows[rIdx]["Qty"]) == 0)
                                {
                                    NoStockItemCount += 1;
                                }
                            }
                            ViewState["Servicetable"] = dsItem.Tables[0];
                        }
                        else
                        {
                            if (common.myStr(objBill.getHospitalSetupValue("IsWardRequestApproved", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]))) == "Y")
                            {

                                dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), "A", common.myInt(Session["FacilityId"]));
                            }
                            else
                            {
                                dsItem = objPharmacy.getIPPatientItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
                            }
                        }


                        if (!dsItem.Tables[0].Columns.Contains("ISDHAApproved"))
                            dsItem.Tables[0].Columns.Add("ISDHAApproved");
                        if (!dsItem.Tables[0].Columns.Contains("DenialCode"))
                            dsItem.Tables[0].Columns.Add("DenialCode");

                        dsItem.Tables[0].AcceptChanges();




                        ViewState["RequestFromWardItems"] = null;

                        if (dsItem.Tables[0].Rows.Count == 0)
                        {
                            DataRow newdr = dsItem.Tables[0].NewRow();
                            dsItem.Tables[0].Rows.Add(newdr);
                        }
                        else
                        {
                            ViewState["RequestFromWardItems"] = dsItem.Tables[0];
                            //ViewState["Servicetable"] = dsItem.Tables[0];
                        }
                        //RequestedWardItemsWithSchema(dsItem);
                        Session["IsFromRequestFromWard"] = 0;
                        Session["ForBLKDiscountPolicy"] = 0;
                        gvService.DataSource = dsItem.Tables[0];
                        gvService.DataBind();

                        setVisiblilityInteraction();

                        txtRegistrationNo.Enabled = false;
                        txtEncounter.Enabled = false;
                        txtPatientName.Enabled = false;
                        txtAge.Enabled = false;
                    }
                }
            }

            setGridColor();
            if (NoStockItemCount > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('No Stock item count is:" + NoStockItemCount + "');", true);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            BindPreviousData();

            if (e.CommandName == "ItemSelect")
            {
                //string ItemSearchFor = "O"; // search item to O use for sale and p use for po and grn  and i use for issue return .... .
                hdnxmlString.Value = "";
                lblMessage.Text = "";


                //GridViewRow row = gvService.SelectedRow;


                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");
                HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                Label lblRequiredQty = (Label)row.FindControl("lblRequiredQty");
                LinkButton lnkItemName = (LinkButton)row.FindControl("lnkItemName");

                TextBox txtQty = (TextBox)row.FindControl("txtQty");

                string CIMSItemId = common.myStr(((HiddenField)row.FindControl("hdnCIMSItemId")).Value);
                string CIMSType = common.myStr(((HiddenField)row.FindControl("hdnCIMSType")).Value);

                string VIDALItemId = common.myStr(((HiddenField)row.FindControl("hdnVIDALItemId")).Value);

                hdnCIMSItemId.Value = common.myStr(CIMSItemId);
                hdnCIMSType.Value = common.myStr(CIMSType);

                hdnVIDALItemId.Value = common.myStr(VIDALItemId);


                if ((common.myInt(hdnGenericId.Value) == 0 && common.myInt(hdnItemId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
                    || btnAddNewItem.Visible == true && common.myInt(hdnIndentId.Value) == 0 && common.myInt(txtQty.Text) == 0
                    || common.myDbl(lblRequiredQty.Text) == 0 && common.myInt(hdnIndentId.Value) == 0 && common.myInt(txtQty.Text) == 0)
                {
                    return;
                }

                hdnSelectedGenericId.Value = common.myInt(hdnGenericId.Value).ToString();
                hdnSelectedItemId.Value = common.myInt(hdnItemId.Value).ToString();

                int GenericId = common.myInt(hdnGenericId.Value);
                if (common.myInt(hdnItemId.Value) > 0)
                {
                    GenericId = 0;
                }

                int sBalanceQty = 0;
                if (ViewState["EMRPrescription"] != null && Convert.ToBoolean(ViewState["EMRPrescription"]) == true)
                {
                    sBalanceQty = common.myInt(txtQty.Text) == 0 ? common.myInt(lblRequiredQty.Text) : common.myInt(txtQty.Text);
                }
                else
                {
                    sBalanceQty = common.myInt(lblRequiredQty.Text);
                }



                //RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItem.aspx?ItemId=" + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(lblRequiredQty.Text) + "&IName=" + lnkItemName.Text;
                // RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId=" + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance=" + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId;



                if (common.myInt(hdnEncounterId.Value) != 0)
                {
                    BaseC.clsPharmacy phr = new clsPharmacy(sConString);
                    string OPIP = phr.getOPIPEncounter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value));
                    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId="
                        + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance="
                        + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue
                        + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId +
                        "&EncId=" + hdnEncounterId.Value + "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&IndentId=" + hdnIndentId.Value + "&OPIP=" + OPIP;
                }
                else
                {
                    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId="
                       + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance="
                       + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue
                       + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId +
                       "&EncId=" + common.myInt(hdnEncounterId.Value) + "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&IndentId=" + hdnIndentId.Value;
                }

                RadWindow1.Height = 600;
                RadWindow1.Width = 960;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = "RequestedWardItemsOnClientClose";
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.VisibleStatusbar = false;
            }
            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnGenericId = (HiddenField)row.FindControl("hdnGenericId");

                if (common.myStr(e.CommandArgument) != "" || common.myInt(hdnGenericId.Value) > 0)
                {
                    int intId = common.myInt(e.CommandArgument);
                    if (intId != 0 || common.myInt(hdnGenericId.Value) > 0)
                    {
                        DataTable dt = new DataTable();
                        if (btnAddNewItem.Visible == true)
                        {
                            dt = (DataTable)ViewState["Servicetable"];
                        }
                        else
                        {
                            dt = (DataTable)ViewState["RequestFromWardItems"];
                            if (ViewState["RequestFromWardItems"] == null)
                            {
                                dt = (DataTable)ViewState["Servicetable"];
                            }
                        }
                        dt.Rows.RemoveAt(row.RowIndex);
                        //rahul 02
                        if (Session["despenseDetails"] != null)
                        {
                            DataTable dtdesp = new DataTable();
                            dtdesp = (DataTable)Session["despenseDetails"];
                            DataView dv = dtdesp.DefaultView;
                            dv.RowFilter = "Itemid <>" + intId;
                            dtdesp = dv.ToTable();
                            Session["despenseDetails"] = dtdesp;
                        }
                        Session["ForBLKDiscountPolicy"] = 0;

                        if (dt.Rows.Count > 0)
                        {
                            gvService.DataSource = dt;
                            Session["OPSaleServicetable"] = dt;
                        }
                        else
                        {
                            Session.Remove("OPSaleServicetable");
                            gvService.DataSource = CreateTable();
                        }
                        hdnTotCharge.Value = "0";
                        hdnTotDiscAmt.Value = "0";
                        hdnTotPatientAmt.Value = "0";
                        hdnTotPayerAmt.Value = "0";
                        hdnTotNetAmt.Value = "0";
                        hdnTotQty.Value = "0";
                        hdnTotUnit.Value = "0";
                        hdnTotTax.Value = "0";

                        gvService.DataBind();

                        setVisiblilityInteraction();
                        //20042017
                        if (ViewState["RequestFromWardItems"] == null)
                        {
                            ViewState["RequestFromWardItems"] = dt;
                        }
                        ViewState["Servicetable"] = dt;
                    }
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
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void RequestedWardItemsWithSchema(DataSet DSWardItems)
    {
        try
        {
            lblMessage.Text = "";
            DataTable dt = DSWardItems.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                DataTable dtXml = new DataTable();
                DataView dv = new DataView(DSWardItems.Tables[1]);
                dv.RowFilter = "ItemId <> " + common.myInt(item["ItemId"]);
                dtXml = dv.ToTable();
                if (dtXml.Rows.Count > 0)
                {
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    dtXml.WriteXml(writer);
                    string xmlSchema = writer.ToString();

                    dt.DefaultView.RowFilter = "";
                    dt.DefaultView.RowFilter = "ItemID = " + common.myInt(item["ItemId"]);
                    if (dt.DefaultView.Count > 0)
                    {
                        dt.DefaultView[0]["BatchXML"] = common.myStr(xmlSchema);
                        dt.DefaultView[0]["Qty"] = common.myDbl(item["Qty"]);

                        if (dtXml.Rows.Count > 0)
                        {
                            dt.DefaultView[0]["ItemName"] = dtXml.Rows[0]["ItemName"];
                            dt.DefaultView[0]["ItemId"] = dtXml.Rows[0]["ItemId"];

                        }
                        dt.AcceptChanges();
                        dt.DefaultView.RowFilter = "";
                        dt.AcceptChanges();

                        hdnTotCharge.Value = "0";
                        hdnTotDiscAmt.Value = "0";
                        hdnTotPatientAmt.Value = "0";
                        hdnTotPayerAmt.Value = "0";
                        hdnTotNetAmt.Value = "0";
                        hdnTotQty.Value = "0";
                        hdnTotUnit.Value = "0";
                        hdnTotTax.Value = "0";
                    }
                }
            }
            Session["ForBLKDiscountPolicy"] = 0;
            gvService.DataSource = dt;
            gvService.DataBind();

            setVisiblilityInteraction();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnRequestedWardItems_OnClick(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsPharmacy objphr = new clsPharmacy(sConString);
            lblMessage.Text = "";
            DataTable dt = new DataTable();
            if (ViewState["RequestFromWardItems"] != null
                && (common.myInt(hdnSelectedGenericId.Value) > 0 || common.myInt(hdnSelectedItemId.Value) > 0))
            {

                dt = ((DataTable)ViewState["RequestFromWardItems"]).Copy();

                if (dt.Rows.Count > 0)
                {
                    if (common.myStr(hdnxmlString.Value) != "")
                    {
                        //---------Convert into dataset of xml data----------
                        string xmlSchema = common.myStr(hdnxmlString.Value);
                        StringReader sr = new StringReader(xmlSchema);
                        DataSet dsXml = new DataSet();
                        DataTable dtXml = new DataTable();
                        dsXml.ReadXml(sr);

                        if (dsXml.Tables.Count > 0)
                        {
                            if (dsXml.Tables[0].Rows.Count > 0)
                            {
                                //objHospitalSetup = new BaseC.HospitalSetup(sConString);//f22122016
                                //if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCheckNearestExpiryItem", common.myInt(Session["FacilityId"]))) == "Y")//f22122016
                                //{
                                //    DataSet dataset = new DataSet(); //f22122016
                                //    //HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId"); //f22122016
                                //    LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");//f22122016
                                //    Label lblExpiryDateOfItem = (Label)e.Row.FindControl("lblExpiryDate");//f22122016
                                //    if (lnkItemName.Text != string.Empty && lblExpiryDateOfItem.Text != string.Empty) //f22122016
                                //    {
                                //        dataset = objphr.GetphrCheckBatchNoNearByExpiry(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                                //       common.myInt(ddlStore.SelectedValue), common.myInt(hdnItemId.Value));
                                //        if (dataset.Tables.Count > 0)
                                //        {
                                //            if (dataset.Tables[0].Rows.Count > 0)
                                //            {
                                //                if (lblExpiryDateOfItem.Text == Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy").ToString())
                                //                {

                                //                }
                                //                else
                                //                {
                                //                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Nearest expiry item is available in stock.Please select nearest expiry item');", true);
                                //                }
                                //            }
                                //        }


                                //    }
                                //}

                                DataView dv = new DataView(dsXml.Tables[0]);
                                if (common.myInt(hdnIndentId.Value) > 0 && common.myStr(txtIndentNo.Text).Length > 0)
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0) <> " + common.myInt(hdnSelectedItemId.Value);
                                    //+ " AND ISNULL(GenericId,0) <> " + common.myInt(hdnSelectedGenericId.Value);
                                }
                                else
                                {
                                    dv.RowFilter = "ISNULL(ItemId,0) <> " + common.myInt(hdnSelectedItemId.Value);
                                }

                                dtXml = dv.ToTable();
                            }
                        }
                        //---------------------
                        dt.DefaultView.RowFilter = "";
                        if (common.myInt(hdnIndentId.Value) > 0)//&& common.myStr(txtIndentNo.Text).Length > 0)
                        {
                            dt.DefaultView.RowFilter = "ISNULL(ItemID,0) = " + common.myInt(hdnSelectedItemId.Value) +
                                                    " AND ISNULL(GenericId,0) = " + common.myInt(hdnSelectedGenericId.Value) +
                                                    " or ISNULL(RequestedItemId,0) = " + common.myInt(hdnSelectedItemId.Value);
                        }
                        else
                        {
                            dt.DefaultView.RowFilter = "ISNULL(ItemID,0) = " + common.myInt(hdnSelectedItemId.Value) + " or ISNULL(RequestedItemId,0) = " + common.myInt(hdnSelectedItemId.Value);
                        }

                        if (dt.DefaultView.Count > 0)
                        {
                            dt.DefaultView[0]["BatchXML"] = common.myStr(hdnxmlString.Value);
                            dt.DefaultView[0]["Qty"] = common.myDbl(hdnIssueQty.Value);

                            if (dtXml.Rows.Count > 0)
                            {
                                dt.DefaultView[0]["ItemName"] = dtXml.Rows[0]["ItemName"];
                                dt.DefaultView[0]["ItemId"] = dtXml.Rows[0]["ItemId"];
                                dt.DefaultView[0]["BatchId"] = dtXml.Rows[0]["BatchId"];
                                dt.DefaultView[0]["BatchNo"] = dtXml.Rows[0]["BatchNo"];
                                dt.DefaultView[0]["ExpiryDate"] = dtXml.Rows[0]["ExpiryDate"];
                                //dt.DefaultView[0]["RequestedItemId"] = hdnSelectedItemId.Value;

                            }

                            //DataView dvPresc = dt.DefaultView;
                            //dvPresc.RowFilter = "ISNULL(PrescriptionDetailsId,0)>0";

                            //if (dvPresc.Count > 0)
                            //{
                            if (dsXml.Tables.Count > 0)
                            {
                                if (dsXml.Tables[0].Rows.Count > 0)
                                {
                                    if (dsXml.Tables[0].Rows.Count > 1) //for multiple batch and different sale price
                                    {
                                        double Qty = 0.0;
                                        double Rate = 0.0;
                                        double PercentDiscount = 0.0;
                                        double disc = 0.0;
                                        double netamt = 0.0;
                                        double SingleDisc = 0.00;
                                        string sBatchNo = "";
                                        string dtExpDate = "";
                                        foreach (DataRow DRITEM in dsXml.Tables[0].Rows)
                                        {

                                            Qty += common.myDbl(DRITEM["Qty"]);

                                            DataSet dsConversion = new DataSet();
                                            int intConversionFactor = 0;
                                            double dbQty;
                                            int intConvertedQty;
                                            dsConversion = objphr.getItemConversionFactor(common.myInt(DRITEM["ItemId"]));

                                            if (dsConversion.Tables.Count > 0)
                                            {
                                                if (dsConversion.Tables[0].Rows.Count > 0)
                                                {
                                                    intConversionFactor = common.myInt(dsConversion.Tables[0].Rows[0]["ConversionFactor2"]);
                                                }
                                            }
                                            if (hdnIsStripWiseSaleRequired.Value == "Y")
                                            {
                                                if (intConversionFactor > 0)
                                                {
                                                    if (common.myInt(common.myDbl(Qty)) == 0)
                                                    {
                                                        //hdnIssueQty.Value = common.myStr(intConversionFactor);
                                                        dt.DefaultView[0]["Qty"] = common.myDbl(Qty);//common.myDbl(DRITEM["Qty"]);
                                                        Qty = common.myDbl(Qty);//common.myDbl(DRITEM["Qty"]);
                                                    }
                                                    else
                                                    {
                                                        dbQty = Math.Ceiling(common.myDbl(common.myDec(Qty) / common.myDec(intConversionFactor)));
                                                        intConvertedQty = common.myInt(dbQty) * common.myInt(intConversionFactor);
                                                        //hdnIssueQty.Value = common.myStr(intConvertedQty);
                                                        dt.DefaultView[0]["Qty"] = common.myStr(intConvertedQty);
                                                        Qty = common.myDbl(intConvertedQty);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                dt.DefaultView[0]["Qty"] = common.myDbl(Qty);
                                                Qty = common.myDbl(Qty);
                                            }
                                            //Qty = common.myDbl(DRITEM["Qty"]);

                                            if (sBatchNo == "")
                                            {
                                                sBatchNo = DRITEM["BatchNo"].ToString();
                                            }
                                            else
                                            {
                                                sBatchNo = sBatchNo + ", " + DRITEM["BatchNo"].ToString();
                                            }
                                            dt.DefaultView[0]["BatchNo"] = sBatchNo;

                                            if (dtExpDate == "")
                                            {
                                                dtExpDate = Convert.ToDateTime(DRITEM["ExpiryDate"]).ToString("yyyy-MM-dd").ToString();
                                            }
                                            else
                                            {
                                                if (Convert.ToDateTime(Convert.ToDateTime(DRITEM["ExpiryDate"]).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dtExpDate))
                                                {
                                                    dtExpDate = Convert.ToDateTime(DRITEM["ExpiryDate"]).ToString("yyyy-MM-dd").ToString();
                                                }
                                            }
                                            //dt.DefaultView[0]["ExpiryDate"] = Convert.ToDateTime(DRITEM["ExpiryDate"]).ToString("dd/MM/yyyy").ToString();
                                            dt.DefaultView[0]["ExpiryDate"] = dtExpDate;
                                            Rate = common.myDbl(DRITEM["SalePrice"]);
                                            PercentDiscount = common.myDbl(DRITEM["DiscAmtPercent"]);
                                            SingleDisc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(PercentDiscount) / 100), 2);
                                            disc = disc + SingleDisc;
                                            netamt += (common.myDbl(Qty) * common.myDbl(Rate)) - SingleDisc;

                                            dt.DefaultView[0]["CostPrice"] = common.myDbl(DRITEM["CostPrice"]);
                                            dt.DefaultView[0]["MRP"] = common.myDbl(DRITEM["SalePrice"]);
                                            dt.DefaultView[0]["PercentDiscount"] = common.myDbl(DRITEM["DiscAmtPercent"]);

                                            if (common.myDbl(disc) != 0)
                                            {
                                                dt.DefaultView[0]["DiscAmt"] = common.myDbl(disc);
                                            }
                                            else
                                            {
                                                dt.DefaultView[0]["DiscAmt"] = common.myDbl(DRITEM["DisAmt"]);
                                            }

                                            dt.DefaultView[0]["TaxAmt"] = common.myDbl(DRITEM["Tax"]);

                                            if (common.myDbl(netamt) != 0)
                                            {
                                                dt.DefaultView[0]["NetAmt"] = common.myDbl(netamt);
                                            }
                                            else
                                            {
                                                dt.DefaultView[0]["NetAmt"] = common.myDbl(DRITEM["NetAmt"]);
                                            }

                                            if (hdnPaymentType.Value == "C")
                                            {
                                                if (common.myDbl(netamt) != 0)
                                                {
                                                    dt.DefaultView[0]["PatientAmount"] = common.myDbl(netamt);
                                                    dt.DefaultView[0]["PayerAmount"] = 0;
                                                }
                                                else
                                                {
                                                    dt.DefaultView[0]["PatientAmount"] = common.myDbl(DRITEM["NetAmt"]);
                                                    dt.DefaultView[0]["PayerAmount"] = 0;
                                                }
                                            }
                                            else // PaymentType = B 
                                            {
                                                dt.DefaultView[0]["PatientAmount"] = 0;
                                                dt.DefaultView[0]["PayerAmount"] = common.myDbl(DRITEM["NetAmt"]);
                                            }
                                            dt.DefaultView[0]["CopayAmt"] = 0;
                                            if (Request.QueryString["Code"] == "IPI")//Ipd sale
                                            {
                                                dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["IPPharmacyCoPayPercent"]);
                                            }
                                            else
                                            {
                                                dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]); ;
                                            }
                                            //New code 
                                            if (hdnPaymentType.Value == "C")
                                            {
                                                if (common.myDbl(netamt) != 0)
                                                {
                                                    dt.DefaultView[0]["PatientAmount"] = common.myDbl(netamt);
                                                    dt.DefaultView[0]["PayerAmount"] = 0;
                                                }
                                                else
                                                {
                                                    //dt.DefaultView[0]["PatientAmount"] = common.myDbl(Qty * Rate);
                                                    dt.DefaultView[0]["PatientAmount"] = Math.Round(common.myDbl(Qty * Rate), 2);
                                                    dt.DefaultView[0]["PayerAmount"] = 0;
                                                }
                                            }
                                            else // PaymentType = B 
                                            {
                                                dt.DefaultView[0]["PatientAmount"] = 0;
                                                if ((common.myStr(dt.DefaultView[0]["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                                {
                                                    dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                                    //dt.DefaultView[0]["PayerAmount"] = netamt;
                                                }
                                                else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(dt.DefaultView[0]["ISDHAApproved"]) == "0"))
                                                {
                                                    dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                                    //dt.DefaultView[0]["PayerAmount"] = netamt;
                                                    dt.DefaultView[0]["PatientAmount"] = 0;
                                                    dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                                    dt.DefaultView[0]["CopayAmt"] = 0;
                                                }
                                                else
                                                {
                                                    dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                                    //dt.DefaultView[0]["PatientAmount"] = netamt;
                                                    dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                                    dt.DefaultView[0]["CopayAmt"] = 0;
                                                    // dt.DefaultView[0]["CopayAmt"] = netamt;
                                                    dt.DefaultView[0]["PatientAmount"] = 0;
                                                }
                                            }

                                            if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                                                 || common.myBool(ViewState["IsVIDALInterfaceActive"]))
                                            {
                                                BaseC.clsEMR objEMR = new clsEMR(sConString);

                                                DataSet dsInterface = objEMR.getInterfaceItemDetails(common.myInt(hdnSelectedItemId.Value), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

                                                if (dsInterface.Tables[0].Rows.Count > 0)
                                                {
                                                    dt.DefaultView[0]["CIMSItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSItemId"]);
                                                    dt.DefaultView[0]["CIMSType"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSType"]);
                                                    dt.DefaultView[0]["VIDALItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["VIDALItemId"]);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                        objHospitalSetup = new BaseC.HospitalSetup(sConString);//f22122016
                                        if (common.myStr(objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCheckNearestExpiryItem", common.myInt(Session["FacilityId"]))) == "Y")
                                        {

                                            DataSet dataset = new DataSet();
                                            string strItemName = "";
                                            string ExpiryDateOfItem = "";
                                            strItemName = common.myStr(dsXml.Tables[0].Rows[0]["ItemName"]);
                                            ExpiryDateOfItem = common.myStr(dsXml.Tables[0].Rows[0]["ExpiryDate"]);
                                            //HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
                                            //LinkButton lnkItemName = (LinkButton)e.Row.FindControl("lnkItemName");
                                            //Label lblExpiryDateOfItem = (Label)e.Row.FindControl("lblExpiryDate");
                                            string strExpiryDate;
                                            string day, month, year, ExpiryDate;
                                            if (strItemName != string.Empty && ExpiryDateOfItem != string.Empty)
                                            {
                                                dataset = objphr.GetphrCheckBatchNoNearByExpiry(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]),
                                               common.myInt(ddlStore.SelectedValue), common.myInt(dsXml.Tables[0].Rows[0]["ItemId"]));
                                                if (dataset.Tables.Count > 0)
                                                {
                                                    if (dataset.Tables[0].Rows.Count > 0)
                                                    {

                                                        strExpiryDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("MM/dd/yyyy").ToString();
                                                        ExpiryDate = ExpiryDateOfItem;
                                                        if (ExpiryDate.IndexOf('/') == 4 || ExpiryDate.IndexOf('-') == 4)
                                                        {
                                                            ExpiryDate = Convert.ToDateTime(ExpiryDate).ToString("MM/dd/yyyy");
                                                        }
                                                        else
                                                        {
                                                            day = ExpiryDate.Substring(0, 2);
                                                            month = ExpiryDate.Substring(3, 2);
                                                            year = ExpiryDate.Substring(6, 4);
                                                            ExpiryDate = month + "/" + day + "/" + year;
                                                        }

                                                        //if (lblExpiryDateOfItem.Text == Convert.ToDateTime(dataset.Tables[0].Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy").ToString())
                                                        if (ExpiryDate == strExpiryDate)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Nearest expiry item is available in stock.Please select nearest expiry item');", true);
                                                        }
                                                    }
                                                }

                                            }
                                        }


                                        DataSet dsConversion = new DataSet();
                                        int intConversionFactor = 0;
                                        double dbQty;
                                        int intConvertedQty;
                                        int IsTopUp = 0;
                                        dsConversion = objphr.getItemConversionFactor(common.myInt(dsXml.Tables[0].Rows[0]["ItemId"]));

                                        if (dsConversion.Tables.Count > 0)
                                        {
                                            if (dsConversion.Tables[0].Rows.Count > 0)
                                            {
                                                intConversionFactor = common.myInt(dsConversion.Tables[0].Rows[0]["ConversionFactor2"]);
                                            }
                                        }
                                        if (hdnIsStripWiseSaleRequired.Value == "Y")
                                        {
                                            if (intConversionFactor > 0)
                                            {
                                                if (common.myInt(hdnIssueQty.Value) == 0)
                                                {
                                                    hdnIssueQty.Value = common.myStr(intConversionFactor);
                                                    dt.DefaultView[0]["Qty"] = common.myStr(hdnIssueQty.Value);
                                                }
                                                else
                                                {
                                                    dbQty = Math.Ceiling(common.myDbl(common.myDec(hdnIssueQty.Value) / common.myDec(intConversionFactor)));
                                                    intConvertedQty = common.myInt(dbQty) * common.myInt(intConversionFactor);
                                                    hdnIssueQty.Value = common.myStr(intConvertedQty);
                                                    dt.DefaultView[0]["Qty"] = common.myStr(hdnIssueQty.Value);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dt.DefaultView[0]["Qty"] = common.myStr(hdnIssueQty.Value);
                                        }


                                        dt.DefaultView[0]["BatchNo"] = common.myStr(dsXml.Tables[0].Rows[0]["BatchNo"]);
                                        dt.DefaultView[0]["ExpiryDate"] = Convert.ToDateTime(dsXml.Tables[0].Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy").ToString();

                                        double Qty = common.myDbl(hdnIssueQty.Value);
                                        double Rate = common.myDbl(dsXml.Tables[0].Rows[0]["SalePrice"]);
                                        double PercentDiscount = common.myDbl(dsXml.Tables[0].Rows[0]["DiscAmtPercent"]);
                                        double disc = Math.Round((common.myDbl(Qty) * common.myDbl(Rate) * common.myDbl(PercentDiscount) / 100), 2);
                                        double netamt = (common.myDbl(Qty) * common.myDbl(Rate)) - disc;

                                        dt.DefaultView[0]["CostPrice"] = common.myDbl(dsXml.Tables[0].Rows[0]["CostPrice"]);
                                        dt.DefaultView[0]["MRP"] = common.myDbl(dsXml.Tables[0].Rows[0]["SalePrice"]);
                                        dt.DefaultView[0]["PercentDiscount"] = common.myDbl(dsXml.Tables[0].Rows[0]["DiscAmtPercent"]);

                                        if (common.myDbl(disc) != 0)
                                        {
                                            dt.DefaultView[0]["DiscAmt"] = common.myDbl(disc);
                                        }
                                        else
                                        {
                                            dt.DefaultView[0]["DiscAmt"] = common.myDbl(dsXml.Tables[0].Rows[0]["DisAmt"]);
                                        }

                                        dt.DefaultView[0]["TaxAmt"] = common.myDbl(dsXml.Tables[0].Rows[0]["Tax"]);

                                        if (common.myDbl(netamt) != 0)
                                        {
                                            dt.DefaultView[0]["NetAmt"] = common.myDbl(netamt);
                                        }
                                        else
                                        {
                                            dt.DefaultView[0]["NetAmt"] = common.myDbl(dsXml.Tables[0].Rows[0]["NetAmt"]);
                                        }

                                        if (hdnPaymentType.Value == "C")
                                        {
                                            if (common.myDbl(netamt) != 0)
                                            {
                                                dt.DefaultView[0]["PatientAmount"] = common.myDbl(netamt);
                                                dt.DefaultView[0]["PayerAmount"] = 0;
                                            }
                                            else
                                            {
                                                dt.DefaultView[0]["PatientAmount"] = common.myDbl(dsXml.Tables[0].Rows[0]["NetAmt"]);
                                                dt.DefaultView[0]["PayerAmount"] = 0;
                                            }
                                        }
                                        else // PaymentType = B 
                                        {
                                            dt.DefaultView[0]["PatientAmount"] = 0;
                                            dt.DefaultView[0]["PayerAmount"] = common.myDbl(dsXml.Tables[0].Rows[0]["NetAmt"]);
                                        }
                                        dt.DefaultView[0]["CopayAmt"] = 0;
                                        if (Request.QueryString["Code"] == "IPI")//Ipd sale
                                        {
                                            dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["IPPharmacyCoPayPercent"]);
                                        }
                                        else
                                        {
                                            dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]); ;
                                        }
                                        //New code 
                                        if (hdnPaymentType.Value == "C")
                                        {
                                            if (common.myDbl(netamt) != 0)
                                            {
                                                dt.DefaultView[0]["PatientAmount"] = common.myDbl(netamt);
                                                dt.DefaultView[0]["PayerAmount"] = 0;
                                            }
                                            else
                                            {
                                                //dt.DefaultView[0]["PatientAmount"] = Math.Round(common.myDbl(Qty * Rate), 2);
                                                //dt.DefaultView[0]["PatientAmount"] = common.myDbl(Qty * Rate);
                                                dt.DefaultView[0]["PatientAmount"] = 0;
                                                dt.DefaultView[0]["PayerAmount"] = 0;
                                            }
                                        }
                                        else // PaymentType = B 
                                        {
                                            dt.DefaultView[0]["PatientAmount"] = 0;
                                            if ((common.myStr(dt.DefaultView[0]["ISDHAApproved"]) == "1") || (Request.QueryString["Code"] == "IPI"))
                                            {
                                                dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                            }
                                            else if ((txtApprovalCode.Text.Trim().Length > 4) && (common.myStr(dt.DefaultView[0]["ISDHAApproved"]) == "0"))
                                            {
                                                dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                                //dt.DefaultView[0]["PayerAmount"] = netamt;
                                                dt.DefaultView[0]["PatientAmount"] = 0;
                                                dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                                dt.DefaultView[0]["CopayAmt"] = 0;
                                            }
                                            else
                                            {
                                                //dt.DefaultView[0]["PatientAmount"] = Math.Round(netamt, 2);
                                                //dt.DefaultView[0]["CopayPerc"] = 100.00;
                                                //dt.DefaultView[0]["CopayAmt"] = Math.Round(netamt, 2);
                                                //dt.DefaultView[0]["PayerAmount"] = 0;

                                                dt.DefaultView[0]["PayerAmount"] = Math.Round(netamt, 2);
                                                //dt.DefaultView[0]["PayerAmount"] = netamt;
                                                dt.DefaultView[0]["CopayPerc"] = common.myDbl(ViewState["OPPharmacyCoPayPercent"]);
                                                dt.DefaultView[0]["CopayAmt"] = 0;
                                                dt.DefaultView[0]["PatientAmount"] = 0;
                                            }
                                        }
                                        if (common.myBool(ViewState["IsCIMSInterfaceActive"])
                                             || common.myBool(ViewState["IsVIDALInterfaceActive"]))
                                        {
                                            BaseC.clsEMR objEMR = new clsEMR(sConString);

                                            DataSet dsInterface = objEMR.getInterfaceItemDetails(common.myInt(hdnSelectedItemId.Value), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));

                                            if (dsInterface.Tables[0].Rows.Count > 0)
                                            {
                                                dt.DefaultView[0]["CIMSItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSItemId"]);
                                                dt.DefaultView[0]["CIMSType"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSType"]);
                                                dt.DefaultView[0]["VIDALItemId"] = common.myStr(dsInterface.Tables[0].Rows[0]["VIDALItemId"]);
                                            }
                                        }
                                    }

                                }
                            }
                            if (dtXml.Rows.Count > 0)
                            {
                                dt.DefaultView[0]["ItemName"] = dtXml.Rows[0]["ItemName"];
                                dt.DefaultView[0]["ItemId"] = dtXml.Rows[0]["ItemId"];
                                dt.DefaultView[0]["RequestedItemId"] = hdnSelectedItemId.Value;
                            }
                            //}

                            dt.AcceptChanges();
                            dt.DefaultView.RowFilter = "";
                            dt.AcceptChanges();
                            //if (ViewState["RequestFromWardItems"] != null && common.myInt(hdnSelectedItemId.Value) > 0)
                            //{
                            DataTable dt1 = applyCoPayment(dt, 0);
                            ViewState["Servicetable"] = dt;
                            ViewState["RequestFromWardItems"] = dt1;
                            //}
                            //else
                            //{
                            //    ViewState["Servicetable"] = dt;
                            //}
                            hdnTotCharge.Value = "0";
                            hdnTotDiscAmt.Value = "0";
                            hdnTotPatientAmt.Value = "0";
                            hdnTotPayerAmt.Value = "0";
                            hdnTotNetAmt.Value = "0";
                            hdnTotQty.Value = "0";
                            hdnTotUnit.Value = "0";
                            hdnTotTax.Value = "0";

                            Session["IsFromRequestFromWard"] = 1;
                            Session["ForBLKDiscountPolicy"] = 0;
                            gvService.DataSource = dt;
                            gvService.DataBind();

                            setVisiblilityInteraction();
                            ViewState["EMRPrescription"] = false;
                        }
                        hdnSelectedItemId.Value = "";
                    }
                    else
                    {
                        DataTable dtt = new DataTable();

                        dtt = ((DataTable)ViewState["RequestFromWardItems"]);
                        Session["IsFromRequestFromWard"] = 1;
                        Session["ForBLKDiscountPolicy"] = 0;

                        if (dtt.Rows.Count > 0)
                        {
                            gvService.DataSource = dtt;
                            ViewState["Servicetable"] = dtt;
                        }
                        else
                        {
                            Session.Remove("Servicetable");
                            gvService.DataSource = CreateTable();
                        }
                        hdnTotCharge.Value = "0";
                        hdnTotDiscAmt.Value = "0";
                        hdnTotPatientAmt.Value = "0";
                        hdnTotPayerAmt.Value = "0";
                        hdnTotNetAmt.Value = "0";
                        hdnTotQty.Value = "0";
                        hdnTotUnit.Value = "0";
                        hdnTotTax.Value = "0";

                        gvService.DataBind();

                        setVisiblilityInteraction();

                        ViewState["Servicetable"] = dtt;
                    }
                }

            }
            setGridColor();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    private void ShowCashCredit()
    {
        DataSet ds = (DataSet)ViewState["CompanyList"];
        if (ds.Tables.Count > 0)
        {
            DataView dv = ds.Tables[0].Copy().DefaultView;
            dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
            if (dv.ToTable().Rows.Count > 0)
            {
                if (common.myStr(dv.ToTable().Rows[0]["PaymentType"]) == "B") //B for credit
                {

                    lblPayertype.Text = "Credit";
                    if (ddlPatientType.SelectedItem.Text.ToUpper() == "CASH SALE")
                    {
                        if (IsAllowCashSaleforCreditpatient == "N")
                        {
                            ddlPatientType.SelectedIndex = ddlPatientType.Items.IndexOf(ddlPatientType.Items.FindItemByText("CREDIT SALE"));
                        }
                        else
                        {
                            ddlPatientType.SelectedIndex = ddlPatientType.Items.IndexOf(ddlPatientType.Items.FindItemByText("CASH SALE"));
                        }
                    }
                    if ((ddlPatientType.SelectedItem.Text.ToUpper() == "CREDIT SALE") || (ddlPatientType.SelectedItem.Text.ToUpper() == "IPD ISSUE"))
                    {
                        pnlCopayment.Visible = true;
                        lnkInsuranceDetails.Visible = true;
                    }
                    else
                    {
                        lnkInsuranceDetails.Visible = false;
                        pnlCopayment.Visible = false;
                    }
                }
                else //C for cash 
                {
                    lblPayertype.Text = "Cash";
                    pnlCopayment.Visible = false;
                    // hdnPaymentType.Value = "C";
                }
            }
            else
            {
                lblPayertype.Text = "Cash";
                pnlCopayment.Visible = false;
            }
        }
    }
    private void ChangeCompany()
    {
        try
        {
            DataSet ds = (DataSet)ViewState["CompanyList"];
            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].Copy().DefaultView;
                dv.RowFilter = "CompanyId='" + ddlPayer.SelectedValue + "'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    hdnPaymentType.Value = common.myStr(dv.ToTable().Rows[0]["PaymentType"]);
                }
                else
                {
                    hdnPaymentType.Value = "C";
                }
                if (hdnPaymentType.Value == "C") // for cash          
                {
                    lblPayertype.Text = "Cash";
                    rpvItem.Visible = true;
                }
                else // B for credit  
                {
                    rpvItem.Visible = true;
                    rpvItem.Selected = true;
                    lblPayertype.Text = "Credit";

                }
                if (gvService.Rows.Count > 0)
                {
                    DataTable dtPrevious = (DataTable)ViewState["Servicetable"];
                    TextBox txtTotalPatient = (TextBox)gvService.FooterRow.FindControl("txtTotalPatient");
                    TextBox txtTotalPayer = (TextBox)gvService.FooterRow.FindControl("txtTotalPayer");
                    foreach (GridViewRow row in gvService.Rows)
                    {

                        TextBox txtPatient = (TextBox)row.FindControl("txtPatient");
                        TextBox txtPayer = (TextBox)row.FindControl("txtPayer");

                        if (hdnPaymentType.Value == "C" && common.myStr(ViewState["PreviousPaymentType"]) != "C")
                        {
                            txtPatient.Text = txtPayer.Text;
                            txtPayer.Text = "0";
                            txtTotalPatient.Text = txtTotalPayer.Text;
                            txtTotalPayer.Text = "0";
                            txtReceived.Text = txtNetAmount.Text;

                        }
                        else if (hdnPaymentType.Value == "B" && common.myStr(ViewState["PreviousPaymentType"]) != "B")
                        {
                            txtPayer.Text = txtPatient.Text;
                            txtPatient.Text = "0";
                            txtTotalPayer.Text = txtTotalPatient.Text;
                            txtTotalPatient.Text = "0";
                            txtReceived.Text = "0.00";

                        }
                    }
                    // change the value into gird for cash or credit company of selected payer 
                    for (int i = 0; i < dtPrevious.Rows.Count; i++)
                    {
                        string PatientAmt = common.myStr(dtPrevious.Rows[i]["PatientAmount"]);
                        string PayerAmt = common.myStr(dtPrevious.Rows[i]["PayerAmount"]);

                        if (hdnPaymentType.Value == "C" && common.myStr(ViewState["PreviousPaymentType"]) != "C")
                        {
                            dtPrevious.Rows[i]["PatientAmount"] = PayerAmt;
                            dtPrevious.Rows[i]["PayerAmount"] = "0";
                        }
                        else if (hdnPaymentType.Value == "B" && common.myStr(ViewState["PreviousPaymentType"]) != "B")
                        {

                            dtPrevious.Rows[i]["PayerAmount"] = PatientAmt;
                            dtPrevious.Rows[i]["PatientAmount"] = "0";
                        }
                    }
                    ViewState["Servicetable"] = dtPrevious;
                    ViewState["PreviousPaymentType"] = hdnPaymentType.Value;
                }

                if (ddlPatientType.SelectedItem.Text == "CASH SALE" || ddlPatientType.SelectedItem.Text == "STAFF SALE")
                {
                    ddlPayer.Enabled = false;
                }
                else if (ddlPatientType.SelectedItem.Text == "CREDIT SALE" || ddlPatientType.SelectedItem.Text == "IPD Issue") //s for Staff 
                {
                    ddlPayer.Enabled = false;
                    //f31082017b
                    BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                    IsAllowChangeCompanyInOPCreditSale = common.myStr(objBill.getHospitalSetupValue("IsAllowChangeCompanyInOPCreditSale", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
                    if (IsAllowChangeCompanyInOPCreditSale == "Y" && common.myStr(Request.QueryString["Code"]) == "CSI" && common.myStr(Request.QueryString["ER"]) != "True")
                    {
                        lblPayer.Text = ddlPayer.SelectedItem.Text;
                        if (txtIndentNo.Text == "")//for prescription
                        {
                            ddlPayer.Enabled = true;
                        }
                        else
                        {
                            ddlPayer.Enabled = false;

                        }


                        if (ddlPayer.SelectedItem.Text.ToUpper() == "CASH")
                        {
                            lblPayertype.Text = "CASH";////
                        }
                        else
                        {
                            lblPayertype.Text = "CREDIT";////
                        }

                    }
                    else
                    {
                        ddlPayer.Enabled = false;
                    }

                    //f31082017e
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

    }
    protected void ddlPayer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["PaymentType"]) != "B" && ViewState["PaymentType"] != null)
        {
            ShowCashCredit();
        } //IF Condition Added onlu for Change Patient Company Type Credit to Cash
        else
        {
            ChangeCompany();
        }


    }
    protected void btnBounce_OnClick(object sender, EventArgs e)
    {
        txtNetAmount.Text = Request[txtNetAmount.UniqueID];
        txtLAmt.Text = Request[txtLAmt.UniqueID];
        txtRounding.Text = Request[txtRounding.UniqueID];
        txtReceived.Text = Request[txtReceived.UniqueID];


        RadWindow1.NavigateUrl = "~/Pharmacy/addBounceItem.aspx?TransactionType=" + common.myStr(ViewState["TransactionType"]);
        RadWindow1.Height = 500;
        RadWindow1.Width = 800;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        // RadWindowForNew.Title = "Time Slot";
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    private void BindDoctor()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsInput = new Hashtable();
            hsInput.Add("intHospitalId", common.myInt(Session["HospitalLocationID"]));
            hsInput.Add("chvRefType", "I");
            hsInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferByDoctor", hsInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctor.DataSource = ds;
                ddlDoctor.DataTextField = "Name";
                ddlDoctor.DataValueField = "Id";
                ddlDoctor.DataBind();
                ddlDoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlDoctor.Items[0].Value = "0";
                ddlDoctor.SelectedIndex= ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(common.myStr(Request.QueryString["DoctorId"])));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void chkDoctor_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkDoctor.Checked == true)
        {
            ddlDoctor.Visible = false;
            txtProvider.Visible = true;
            txtProvider.Text = "";
        }
        else
        {
            ddlDoctor.Visible = true;
            txtProvider.Visible = false;
            ddlDoctor.ClearSelection();
            ddlDoctor.Text = "";
            BindDoctor();
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
    protected void btnSurgicalKit_OnClick(object Sender, EventArgs e)
    {
        try
        {
            txtNetAmount.Text = Request[txtNetAmount.UniqueID];
            txtLAmt.Text = Request[txtLAmt.UniqueID];
            txtRounding.Text = Request[txtRounding.UniqueID];
            //txtReceived.Text = Request[txtReceived.UniqueID];

            dvConfirmProfileItem.Visible = false;
            if (common.myStr(txtPatientName.Text).Trim() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Patient not selected ! ";
                return;
            }
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
    void bindPatientProfileItemDetails()
    {
        try
        {
            hdnTotQty.Value = "0";
            hdnTotUnit.Value = "0";
            hdnTotCharge.Value = "0";
            hdnTotTax.Value = "0";
            hdnTotDiscAmt.Value = "0";
            hdnTotPatientAmt.Value = "0";
            hdnTotPayerAmt.Value = "0";
            hdnTotNetAmt.Value = "0";

            DataSet dsItem = new DataSet();
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            dsItem = objPharmacy.getPatientProfileItemDetails(common.myInt(ddlProfileItem.SelectedValue), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

            if (dsItem.Tables[0].Rows.Count == 0)
            {
                lblMessage.Text = "Item not found !";
                return;
            }

            for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
            {
                dsItem.Tables[0].Rows[rIdx]["Qty"] = 0;
            }

            dsItem.Tables[0].AcceptChanges();

            //ViewState["RequestFromWardItems"] = null;

            if (dsItem.Tables[0].Rows.Count == 0)
            {
                DataRow newdr = dsItem.Tables[0].NewRow();
                dsItem.Tables[0].Rows.Add(newdr);
            }
            else
            {
                ViewState["RequestFromWardItems"] = dsItem.Tables[0];

            }
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 0;
            gvService.DataSource = dsItem.Tables[0];
            gvService.DataBind();

            setVisiblilityInteraction();

            txtRegistrationNo.Enabled = false;
            txtEncounter.Enabled = false;
            txtPatientName.Enabled = false;
            txtAge.Enabled = false;
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
        dvConfirmProfileItem.Visible = false;

        if (common.myInt(ddlProfileItem.SelectedValue) > 0)
        {
            lblMessage.Text = "";
            //done by rakesh for user authorisation start
            //btnSaveData.Visible = true;
            SetPermission(btnSaveData, "N", true);
            //btnPrint.Visible = false;
            SetPermission(btnPrint, false);
            //done by rakesh for user authorisation end
            btnAddNewItem.Visible = false;

            if (common.myInt(hdnRegistrationId.Value) != 0)
            {
                //  btnFindPatient.Visible = false;
                bindPatientProfileItemDetails();
            }
        }
    }
    protected void btnCancelProfileItem_OnClick(object sender, EventArgs e)
    {
        dvConfirmProfileItem.Visible = false;
    }
    protected void btnPreviousDate_OnClick(object sender, EventArgs e)
    {
        BindPreviousData();

    }
    private void BindStaffCompanyId()
    {
        BaseC.HospitalSetup objHosp = new BaseC.HospitalSetup(sConString);
        ViewState["StaffCompanyId"] = objHosp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "StaffCompanyId", common.myInt(Session["FacilityId"]));
    }
    protected void btnPrintRequest_OnClick(object sender, EventArgs e)
    {
        dtFromDate.SelectedDate = DateTime.Now;
        dtToDate.SelectedDate = DateTime.Now;
        dvPending.Visible = true;

        //// RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/PendingPrint.aspx?rptType=PRW";
        //// RadWindowForNew.Height = 100;
        //// RadWindowForNew.Width = 200;
        //// RadWindowForNew.Top = 40;
        //// RadWindowForNew.Left = 100;
        //// // RadWindowForNew.Title = "Time Slot";
        ////// RadWindowForNew.OnClientClose = "OnClientClose";
        //// RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        //// RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //// RadWindowForNew.Modal = true;
        //// RadWindowForNew.VisibleStatusbar = false;





    }
    protected void btnPendingPreview_OnClick(object sender, EventArgs e)
    {
        dvPending.Visible = false;
        RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/PrintReport.aspx?rptType=PRW&FDate=" + dtFromDate.SelectedDate.Value.ToString("dd/MM/yyyy") + "&TDate=" + dtToDate.SelectedDate.Value.ToString("dd/MM/yyyy") + "&preview=PP";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        // RadWindowForNew.Title = "Time Slot";
        // RadWindowForNew.OnClientClose = "OnClientClose";
        // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;

    }
    protected void btnPendingCancel_OnClick(object sender, EventArgs e)
    {
        dvPending.Visible = false;
    }
    string pageURL = string.Empty;
    //done by rakesh for user authorisation start
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnCancel, false);
        ua1.DisableEnableControl(btnNew, false);
        ua1.DisableEnableControl(btnPrint, false);

        if (Request.QueryString["Code"] == "IPI")
            pageURL = "ItemIssueSaleReturn.aspx?Code=IPI&IR=S";
        else if (Request.QueryString["Code"] == "CSI")
            pageURL = "ItemIssueSaleReturn.aspx?IR=S&Code=CSI";

        if (ua1.CheckPermissions("N", pageURL, true))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
        }
        if (ua1.CheckPermissions("C", pageURL, true))
        {
            ua1.DisableEnableControl(btnCancel, true);
        }
        if (ua1.CheckPermissions("P", pageURL, true))
        {
            ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (Request.QueryString["Code"] == "IPI")
            pageURL = "ItemIssueSaleReturn.aspx?Code=IPI&IR=S";
        else if (Request.QueryString["Code"] == "CSI")
            pageURL = "ItemIssueSaleReturn.aspx?IR=S&Code=CSI";

        if (ua1.CheckPermissions(mode, pageURL, true))
        {
            ua1.DisableEnableControl(btnID, action);
        }
        else
        {
            ua1.DisableEnableControl(btnID, !action);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, action);
        ua1.Dispose();
    }
    //done by rakesh for user authorisation end
    protected void btnSearchPatientWard_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        //done by rakesh for user authorisation start
        //btnSaveData.Visible = true;
        SetPermission(btnSaveData, "N", true);
        //btnPrint.Visible = false;
        SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        btnAddNewItem.Visible = true;

        if (common.myInt(hdnRegistrationId.Value) != 0)
        {
            // btnFindPatient.Visible = false;
            bindPatientWardRequest();

        }
    }
    protected void btnSearchPatient_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            //done by rakesh for user authorisation start
            //btnSaveData.Visible = true;
            SetPermission(btnSaveData, "N", true);
            //btnPrint.Visible = false;
            SetPermission(btnPrint, false);
            //done by rakesh for user authorisation end
            btnAddNewItem.Visible = true;
            ddlDoctor.Visible = true;
            txtProvider.Visible = false;
            if (common.myInt(hdnRegistrationId.Value) != 0)
            {
                btnRequestFromWard.Visible = false;
                BindPatientDetails();
            }
            ddlPayer_OnSelectedIndexChanged(sender, e);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }

    }
    protected void btnSearchByUHID_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        txtEncounter.Text = "";
        //done by rakesh for user authorisation start
        //btnSaveData.Visible = true;
        SetPermission(btnSaveData, "N", true);
        //btnPrint.Visible = false;
        SetPermission(btnPrint, false);
        //done by rakesh for user authorisation end
        btnAddNewItem.Visible = true;
        //btndDiscount.Visible = true;
        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
        if (Request.QueryString["Code"] == "IPI")//Ipd sale
        {
            btndDiscount.Visible = true;
            lblDiscountType.Visible = false;
            ddlDiscountType.Visible = false;
            btnDiscountApply.Visible = false;
            lblDiscPerc.Visible = false;
            txtDiscPerc.Visible = false;
        }
        else
        {
            IsRequiredBLKDiscountPolicy = common.myStr(objBill.getHospitalSetupValue("IsRequiredBLKDiscountPolicy", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            if (common.myStr(IsRequiredBLKDiscountPolicy) == "Y")
            {
                btndDiscount.Visible = false;
                lblDiscountType.Visible = true;
                ddlDiscountType.Visible = true;
                btnDiscountApply.Visible = true;
                lblDiscPerc.Visible = true;
                txtDiscPerc.Visible = true;
            }
            else
            {
                btndDiscount.Visible = true;
                lblDiscountType.Visible = false;
                ddlDiscountType.Visible = false;
                btnDiscountApply.Visible = false;
                lblDiscPerc.Visible = false;
                txtDiscPerc.Visible = false;
            }
        }
        if (common.myStr(txtRegistrationNo.Text) != "")
        {
            BindPatientDetails();
        }
        ddlPayer_OnSelectedIndexChanged(sender, e);
    }
    void bindEMRPrescriptionDetails()
    {
        try
        {
            if (common.myInt(hdnIndentId.Value) == 0)
            {
                return;
            }

            btnAddNewItem.Visible = false;

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = new DataSet();

            ds = objEMR.getPrescriptionOPRequest(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                               0, 0, "", "", "", "", common.myDate("1753-01-01"), common.myDate("2069-01-01"), common.myInt(Session["UserId"]), 255, "");//255 means all patient

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMessage.Text = "Patient not found !";
                return;
            }
            DataRow DR = ds.Tables[0].Rows[0];
            ViewState["CompanyCodeFromPrescription"] = DR["CompanyCode"].ToString();
            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
            if (DR["PaymentType"].ToString() == "B")
            {
                //Added on 29-09-2014 Start Nasuhad 
                ViewState["PaymentType"] = DR["PaymentType"].ToString();
                //Added on 29-09-2014 End  Naushad
                ddlPatientType.SelectedIndex = 1;
            }
            else if (DR["PaymentType"].ToString() == "C")
            {
                ddlPatientType.SelectedIndex = 0;
            }
            hdnPaymentType.Value = DR["PaymentType"].ToString();
            ddlPatientType_SelectedIndexChanged(ddlPatientType, null);
            //hdnPaymentType.Value = DR["PaymentType"].ToString();         
            txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
            txtEncounter.Text = common.myStr(DR["EncounterNo"]);
            txtPatientName.Text = common.myStr(DR["PatientName"]);
            txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
            hdnEncounterId.Value = common.myStr(DR["EncounterId"]);
            ddlDoctor.SelectedValue = common.myStr(DR["AdvisingDoctorId"]);
            ddlDoctor.Enabled = false;
            ddlDoctor.Visible = true;
            txtProvider.Visible = false;

            if (txtEncounter.Text != "")
            {
                if (objPharmacy.IsPackagePatient(0, common.myStr(txtEncounter.Text)) == 1)
                {
                    lblPackagePatient.Visible = true;
                }
                else
                {
                    lblPackagePatient.Visible = false;
                }
            }

            //ViewState["AdvisingDoctorId"] = common.myStr(DR["AdvisingDoctorId"]);
            if (common.myStr(DR["CompanyCode"]) != "0" && common.myStr(DR["CompanyCode"]) != "")
            {
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
            }
            else
            {
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
            }
            //ddlPatientType_SelectedIndexChanged(ddlPatientType, null);
            //Added on 30-09-2014 Start Naushad Ali
            ShowCashCredit();

            //Added on 30-09-2014 End Nauahd ali
            lblPayer.Text = ddlPayer.SelectedItem.Text;
            txtBedNo.Text = common.myStr(DR["BedNo"]);

            if (common.myStr(DR["GenderAge"]) != "")
            {
                txtAge.Text = common.myStr(DR["GenderAge"]);

                string[] agetype = common.myStr(DR["GenderAge"]).Split(' ');
                if (agetype[1] != "")
                {
                    ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                }
                txtAge.Text = agetype[0];
            }
            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(DR["Gender"])));

            hdnTotQty.Value = "0";
            hdnTotUnit.Value = "0";
            hdnTotCharge.Value = "0";
            hdnTotTax.Value = "0";
            hdnTotDiscAmt.Value = "0";
            hdnTotPatientAmt.Value = "0";
            hdnTotPayerAmt.Value = "0";
            hdnTotNetAmt.Value = "0";
            DataSet dsItemDetail = new DataSet();
            DataSet dsItem = new DataSet();
            dsItemDetail = objEMR.getPrescriptionOPRequestItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
            //raghuvir


            DataView DV = new DataView(dsItemDetail.Tables[0]);
            // DV.RowFilter = "Qty<>0.00";
            dsItem.Tables.Add(DV.ToTable());
            //for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
            //{
            //    dsItem.Tables[0].Rows[rIdx]["Qty"] = 0;
            //}
            dsItem.Tables[0].AcceptChanges();

            ViewState["RequestFromWardItems"] = null;

            if (dsItem.Tables[0].Rows.Count == 0)
            {
                DataRow newdr = dsItem.Tables[0].NewRow();
                dsItem.Tables[0].Rows.Add(newdr);
            }
            else
            {
                ViewState["RequestFromWardItems"] = dsItem.Tables[0]; //20042017
                // ViewState["Servicetable"] = dsItem.Tables[0];
            }
            ViewState["EMRPrescription"] = true;
            //RequestedWardItemsWithSchema(dsItem);
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 0;
            gvService.DataSource = dsItem.Tables[0];
            gvService.DataBind();

            ViewState["Servicetable"] = dsItem.Tables[0];

            setVisiblilityInteraction();

            txtRegistrationNo.Enabled = false;
            txtEncounter.Enabled = false;
            txtPatientName.Enabled = false;
            txtAge.Enabled = false;
            ShowPatientDetails();
            //BindPatientDetails();
            BindPatientProvisionalDiagnosis();


            //-----------------------------------------Start
            //    //BindPreviousData();
            //    //gvService_RowCommand(null,null);
            //    hdnxmlString.Value = "";
            //    lblMessage.Text = "";

            //    //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            //    //foreach (GridViewRow dataItem in gvService.Rows)
            //    //{
            //    int i,j =0;
            //Process:

            //    for (i = j; i < gvService.Rows.Count;)
            //    {
            //        CommandEventArgs commandArgs = new CommandEventArgs("ItemSelect", "GridViewCommandEventArgs");
            //        GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(gvService.Rows[i], gvService, commandArgs);
            //        gvService.SelectedIndex = i;
            //        gvService_RowCommand(gvService, eventArgs);
            //        break;
            //        //HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
            //        //HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
            //        //Label lblRequiredQty = (Label)dataItem.FindControl("lblRequiredQty");
            //        //LinkButton lnkItemName = (LinkButton)dataItem.FindControl("lnkItemName");

            //        //TextBox txtQty = (TextBox)dataItem.FindControl("txtQty");

            //        //string CIMSItemId = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSItemId")).Value);
            //        //string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value);

            //        //string VIDALItemId = common.myStr(((HiddenField)dataItem.FindControl("hdnVIDALItemId")).Value);

            //        //hdnCIMSItemId.Value = common.myStr(CIMSItemId);
            //        //hdnCIMSType.Value = common.myStr(CIMSType);

            //        //hdnVIDALItemId.Value = common.myStr(VIDALItemId);


            //        //if ((common.myInt(hdnGenericId.Value) == 0 && common.myInt(hdnItemId.Value) == 0 && common.myInt(hdnIndentId.Value) == 0)
            //        //    || btnAddNewItem.Visible == true && common.myInt(hdnIndentId.Value) == 0
            //        //    || common.myDbl(lblRequiredQty.Text) == 0 && common.myInt(hdnIndentId.Value) == 0)
            //        //{
            //        //    return;
            //        //}

            //        //hdnSelectedGenericId.Value = common.myInt(hdnGenericId.Value).ToString();
            //        //hdnSelectedItemId.Value = common.myInt(hdnItemId.Value).ToString();

            //        //int GenericId = common.myInt(hdnGenericId.Value);
            //        //if (common.myInt(hdnItemId.Value) > 0)
            //        //{
            //        //    GenericId = 0;
            //        //}

            //        //int sBalanceQty = 0;
            //        //if (ViewState["EMRPrescription"] != null && Convert.ToBoolean(ViewState["EMRPrescription"]) == true)
            //        //{
            //        //    sBalanceQty = common.myInt(txtQty.Text) == 0 ? common.myInt(lblRequiredQty.Text) : common.myInt(txtQty.Text);
            //        //}
            //        //else
            //        //{
            //        //    sBalanceQty = common.myInt(lblRequiredQty.Text);
            //        //}



            //        ////RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItem.aspx?ItemId=" + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(lblRequiredQty.Text) + "&IName=" + lnkItemName.Text;
            //        //// RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId=" + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance=" + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId;



            //        //if (common.myInt(hdnEncounterId.Value) != 0)
            //        //{
            //        //    BaseC.clsPharmacy phr = new clsPharmacy(sConString);
            //        //    string OPIP = phr.getOPIPEncounter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value));
            //        //    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId="
            //        //        + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance="
            //        //        + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue
            //        //        + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId +
            //        //        "&EncId=" + hdnEncounterId.Value + "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&IndentId=" + hdnIndentId.Value + "&OPIP=" + OPIP;
            //        //}
            //        //else
            //        //{
            //        //    RadWindow1.NavigateUrl = "~/Pharmacy/Components/AddItemWithQty.aspx?ItemId="
            //        //       + common.myInt(hdnItemId.Value) + "&QtyBal=" + common.myDbl(sBalanceQty) + "&ActualBalance="
            //        //       + lblRequiredQty.Text + "&IName=" + lnkItemName.Text + "&TrnId=" + ddlPatientType.SelectedValue
            //        //       + "&PageName=Sale&ComId=" + ddlPayer.SelectedValue + "&GenericId=" + GenericId +
            //        //       "&EncId=" + common.myInt(hdnEncounterId.Value) + "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&IndentId=" + hdnIndentId.Value;
            //        //}


            //        //RadWindow1.Height = 600;
            //        //RadWindow1.Width = 960;
            //        //RadWindow1.Top = 10;
            //        //RadWindow1.Left = 10;
            //        //RadWindow1.OnClientClose = "RequestedWardItemsOnClientClose";
            //        //RadWindow1.VisibleOnPageLoad = true;
            //        //RadWindow1.Modal = true;
            //        //RadWindow1.VisibleStatusbar = false;
            //    }
            //    if (j < gvService.Rows.Count)
            //    {
            //        j = j + 1;
            //        goto Process;
            //    }
            //-----------------------------------------Stop
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    void BindMessage()
    {
        BaseC.Patient objbc = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        ds = objbc.GetAlertBlockMessage(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegistrationId.Value));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = "NoteType=4";
            if (ds.Tables[0].DefaultView.Count > 0)
            {
                btnSaveData.Visible = false;
            }
            else
            {
                btnSaveData.Visible = true;
            }
            ds.Tables[0].DefaultView.RowFilter = "";
            dlMissingDocument.DataSource = ds.Tables[0];
            dlMissingDocument.DataBind();
            dvMessage.Visible = true;

        }
        else
        {
            dvMessage.Visible = false;
        }
    }
    protected void btnOk_OnClick(object sender, EventArgs e)
    {
        dvMessage.Visible = false;
    }
    public DataTable applyCoPayment(DataTable dt, decimal MaxcoPayamt)
    {

        decimal CopayPercentage = 0, CopayAmt = 0;
        decimal totalAmount = 0, TotalAmount = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (common.myInt(dr["CopayPerc"]) > 0)
            {
                totalAmount = totalAmount + common.myDec(dr["NetAmt"]);
            }
        }
        foreach (DataRow dr1 in dt.Rows)
        {
            if ((common.myInt(dr1["CopayPerc"]) > 0))
            {
                TotalAmount = common.myDec(common.myDec(dr1["PatientAmount"]) + common.myDec(dr1["PayerAmount"]));
                CopayPercentage = common.myDec(dr1["CopayPerc"]);
                CopayAmt = CopayPercentage / 100 * (TotalAmount);
                if (MaxcoPayamt > 0)
                {
                    CopayPercentage = common.myDec(dr1["CopayPerc"]) * 100 / totalAmount;
                    CopayAmt = CopayPercentage / 100 * (TotalAmount);
                    //CopayPercentage = common.myDec(MaxDecutableAmt) * 100 / TotalNetCharge;
                    //CopayAmt = (CopayPercentage / 100 * (TotalAmount * common.myDec(dr1["Units"])));
                }
                decimal AmountPayableByPatient = 0, AmountPayableByPayer = 0;
                AmountPayableByPatient = CopayAmt;
                AmountPayableByPayer = TotalAmount - common.myDec(CopayAmt);
                dr1["CopayAmt"] = common.myDbl(CopayAmt).ToString("F" + hdnDecimalPlaces.Value);
                dr1["CopayPerc"] = common.myDbl(CopayPercentage).ToString("F" + hdnDecimalPlaces.Value);
                dr1["PatientAmount"] = common.myDbl(AmountPayableByPatient).ToString("F" + hdnDecimalPlaces.Value);
                dr1["PayerAmount"] = common.myDbl(AmountPayableByPayer).ToString("F" + hdnDecimalPlaces.Value);
                dt.AcceptChanges();
            }
        }
        if (common.myInt(ViewState["OPPharmacyCoPayMaxLimit"]) > 0)
        {
            string TotalCoPayAmt;
            TotalCoPayAmt = dt.Compute("Sum(CopayAmt)", "").ToString();
            if (common.myDbl(TotalCoPayAmt) > common.myDbl(ViewState["OPPharmacyCoPayMaxLimit"]))
            {
                ViewState["ReCalc"] = 1;
                applyCoPayment(dt, common.myDec(ViewState["OPPharmacyCoPayMaxLimit"]));
            }
        }
        return dt;

    }
    protected void btnCoPay_OnClick(object sender, EventArgs e)
    {
        //if (ddlCompany.SelectedItem.Attributes["PaymentType"].ToString() == "C")
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Co Payment is applicable for Credit Company only  !";
        //    return;
        //}.
        hdnTotQty.Value = "0";
        hdnTotUnit.Value = "0";
        hdnTotCharge.Value = "0";
        hdnTotTax.Value = "0";
        hdnTotDiscAmt.Value = "0";
        hdnTotPatientAmt.Value = "0";
        hdnTotPayerAmt.Value = "0";
        hdnTotNetAmt.Value = "0";
        if ((DataTable)ViewState["Servicetable"] == null)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select Patient  !";
            return;
        }

        Session["Servicetable"] = (DataTable)ViewState["Servicetable"];


        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/ManualCoPayPharmacy.aspx?RegId=" + common.myInt(hdnRegistrationId.Value) + "&pn=" + txtPatientName.Text + "&dob=" + hdnDOB.Value
            + "&mob=" + hdnMobileNo.Value + "&eno=" + hdnEncounterNo.Value + "&edt=" + hdnEncounterDate.Value;
        RadWindow1.Height = 640;
        RadWindow1.Width = 1020;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "btnCoPay_OnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;


    }
    protected void btnCopayRefresh_OnClick(object sender, EventArgs e)
    {
        hdnTotQty.Value = "0";
        hdnTotUnit.Value = "0";
        hdnTotCharge.Value = "0";
        hdnTotTax.Value = "0";
        hdnTotDiscAmt.Value = "0";
        hdnTotPatientAmt.Value = "0";
        hdnTotPayerAmt.Value = "0";
        hdnTotNetAmt.Value = "0";
        DataTable dt = (DataTable)Session["Servicetable"];
        Session["IsFromRequestFromWard"] = 0;
        Session["ForBLKDiscountPolicy"] = 0;

        gvService.DataSource = dt;
        gvService.DataBind();

        setVisiblilityInteraction();

        ViewState["Servicetable"] = dt;
    }
    private void ShowPatientDetails()
    {
        try
        {
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = objEMR.getScreeningParameters(common.myInt(hdnEncounterId.Value), common.myInt(hdnRegistrationId.Value));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Weight.Text = "";
                txtHeight.Text = "";
                lbl_BSA.Text = "";
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
                    else if (ds.Tables[0].Rows[i][0].ToString() == "BSA")
                    {
                        lbl_BSA.Text = ds.Tables[0].Rows[i][1].ToString();
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
    protected void lnkBtnPrescription_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        string sER = Request.QueryString["ER"] != null ? Request.QueryString["ER"].ToString() : "False";

        ViewState["IsPrescriptionHelpSearch"] = true;
        btnAddNewItem.Visible = false;
        RadWindowForNew.NavigateUrl = "/EMR/Medication/MedicationDispense.aspx?PT=P&ER=" + sER;

        //RadWindowForNew.NavigateUrl = "~/EMR/Medication/MedicationDispense.aspx?PT=P&ER=" + Request.QueryString["ER"] != null ? Request.QueryString["ER"].ToString() : "False";
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "SearchPrescriptionOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }
    void bindEMRPrescriptionDetailsAuto()
    {
        try
        {
            if (common.myInt(hdnIndentId.Value) == 0)
            {
                return;
            }

            btnAddNewItem.Visible = false;

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();

            ds = objEMR.getPrescriptionOPRequest(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                               0, 0, "", "", "", "", common.myDate("1753-01-01"), common.myDate("2069-01-01"), common.myInt(Session["UserId"]), 255, "");//255 means all patient

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMessage.Text = "Patient not found !";
                return;
            }
            DataRow DR = ds.Tables[0].Rows[0];
            ViewState["CompanyCodeFromPrescription"] = DR["CompanyCode"].ToString();
            ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
            if (DR["PaymentType"].ToString() == "B")
            {
                //Added on 29-09-2014 Start Nasuhad 
                ViewState["PaymentType"] = DR["PaymentType"].ToString();
                //Added on 29-09-2014 End  Naushad
                ddlPatientType.SelectedIndex = 1;
            }
            else if (DR["PaymentType"].ToString() == "C")
            {
                ddlPatientType.SelectedIndex = 0;
            }
            hdnPaymentType.Value = DR["PaymentType"].ToString();
            ddlPatientType_SelectedIndexChanged(ddlPatientType, null);
            //hdnPaymentType.Value = DR["PaymentType"].ToString();         
            txtRegistrationNo.Text = common.myStr(DR["RegistrationNo"]);
            txtEncounter.Text = common.myStr(DR["EncounterNo"]);
            txtPatientName.Text = common.myStr(DR["PatientName"]);
            txtPatientName.ToolTip = common.myStr(DR["PatientName"]);
            hdnEncounterId.Value = common.myStr(DR["EncounterId"]);
            ddlDoctor.SelectedValue = common.myStr(DR["AdvisingDoctorId"]);
            ddlDoctor.Enabled = false;
            ddlDoctor.Visible = true;
            txtProvider.Visible = false;
            //ViewState["AdvisingDoctorId"] = common.myStr(DR["AdvisingDoctorId"]);
            if (common.myStr(DR["CompanyCode"]) != "0" && common.myStr(DR["CompanyCode"]) != "")
            {
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(DR["CompanyCode"])));
            }
            else
            {
                ddlPayer.SelectedIndex = ddlPayer.Items.IndexOf(ddlPayer.Items.FindItemByValue(common.myStr(hdnDefaultCompanyId.Value)));
            }
            //ddlPatientType_SelectedIndexChanged(ddlPatientType, null);
            //Added on 30-09-2014 Start Naushad Ali
            ShowCashCredit();

            //Added on 30-09-2014 End Nauahd ali
            lblPayer.Text = ddlPayer.SelectedItem.Text;
            txtBedNo.Text = common.myStr(DR["BedNo"]);

            if (common.myStr(DR["GenderAge"]) != "")
            {
                txtAge.Text = common.myStr(DR["GenderAge"]);

                string[] agetype = common.myStr(DR["GenderAge"]).Split(' ');
                if (agetype[1] != "")
                {
                    ddlAgeType.SelectedIndex = ddlAgeType.Items.IndexOf(ddlAgeType.Items.FindItemByValue(common.myStr(agetype[1]).Substring(0, 1).ToUpper()));
                }
                txtAge.Text = agetype[0];
            }
            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindItemByValue(common.myStr(DR["Gender"])));

            hdnTotQty.Value = "0";
            hdnTotUnit.Value = "0";
            hdnTotCharge.Value = "0";
            hdnTotTax.Value = "0";
            hdnTotDiscAmt.Value = "0";
            hdnTotPatientAmt.Value = "0";
            hdnTotPayerAmt.Value = "0";
            hdnTotNetAmt.Value = "0";
            //DataSet dsItemDetail = new DataSet();
            //DataSet dsItem = new DataSet();
            //dsItemDetail = objEMR.getPrescriptionOPRequestItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));
            ////raghuvir


            //DataView DV = new DataView(dsItemDetail.Tables[0]);
            //// DV.RowFilter = "Qty<>0.00";
            //dsItem.Tables.Add(DV.ToTable());
            ////for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
            ////{
            ////    dsItem.Tables[0].Rows[rIdx]["Qty"] = 0;
            ////}
            //dsItem.Tables[0].AcceptChanges();

            //ViewState["RequestFromWardItems"] = null;

            //if (dsItem.Tables[0].Rows.Count == 0)
            //{
            //    DataRow newdr = dsItem.Tables[0].NewRow();
            //    dsItem.Tables[0].Rows.Add(newdr);
            //}
            //else
            //{
            //    ViewState["RequestFromWardItems"] = dsItem.Tables[0];
            //    // ViewState["Servicetable"] = dsItem.Tables[0];
            //}
            //ViewState["EMRPrescription"] = true;
            //gvService.DataSource = dsItem.Tables[0];
            //gvService.DataBind();

            //ViewState["Servicetable"] = dsItem.Tables[0];

            //setVisiblilityInteraction();

            //txtRegistrationNo.Enabled = false;
            //txtEncounter.Enabled = false;
            //txtPatientName.Enabled = false;
            //txtAge.Enabled = false;
            //ShowPatientDetails();
            //BindPatientProvisionalDiagnosis();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void btnPrescriptionDetails_OnClick(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            lblMessage.Text = "";
            txtIndentNo.Text = common.myStr(hdnIndentNo.Value);
            if (common.myStr(txtIndentNo.Text).Trim() == "")
            {
                return;
            }
            clearControl();
            //hdnIssueId.Value = "";
            //Session["ItemIssueSaleReturnDuplicateCheck"] = 0;
            if (common.myBool(ViewState["IsPrescriptionHelpSearch"]))
            {
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

                ds = objEMR.getMedicinesOPList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                    0, common.myInt(hdnIndentId.Value), common.myInt(hdnRegistrationId.Value), common.myStr(txtIndentNo.Text).Trim());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "Invalid Prescription No !";

                    txtIndentNo.Text = "";
                    return;
                }
                else
                {
                    hdnIndentId.Value = common.myStr(ds.Tables[0].Rows[0]["IndentId"]);
                    hdnRegistrationId.Value = common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]);
                    //hdnEncounterId.Value = common.myStr(ds.Tables[0].Rows[0]["EncounterId"]);
                }

                if (common.myInt(hdnRegistrationId.Value) != 0)
                {
                    //btnFindPatient.Visible = false;
                    btnAddNewItem.Visible = false;
                    //bindEMRPrescriptionDetails();
                    bindEMRPrescriptionDetailsAuto();


                    ds = objPharmacy.getPrescriptionOPRequestItemDetails(common.myInt(hdnIndentId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), common.myInt(ddlStore.SelectedValue), common.myInt(Session["FacilityId"]));

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataSet dsItem = new DataSet();
                        DataSet dsItem1 = new DataSet();
                        DataSet dsItem2 = new DataSet();

                        //if (gvService.Rows.Count > 0)
                        //{
                        //    BindPreviousData();
                        //}

                        DataView DVItemsFind = new DataView(ds.Tables[0]);
                        DVItemsFind.RowFilter = "ItemId IN (" + hdnSelectedItemsFromOPPrescription.Value + ")";
                        dsItem2.Tables.Add(DVItemsFind.ToTable());
                        //for (int rIdx = 0; rIdx < dsItem.Tables[0].Rows.Count; rIdx++)
                        //{
                        //    dsItem.Tables[0].Rows[rIdx]["Qty"] = 0;
                        //}
                        dsItem2.Tables[0].AcceptChanges();

                        DataView dvRecord = dsItem2.Tables[0].DefaultView;
                        dvRecord.RowFilter = "isnull(BatchId,0)=0";

                        dsItem.Tables.Add(dvRecord.ToTable());

                        dsItem.Tables[0].AcceptChanges();

                        Session["IsFromRequestFromWard"] = 0;
                        Session["ForBLKDiscountPolicy"] = 0;

                        gvService.DataSource = dsItem.Tables[0];
                        gvService.DataBind();

                        ViewState["Servicetable"] = dsItem.Tables[0];

                        if (gvService.Rows.Count > 0)
                        {
                            BindPreviousData();
                        }

                        DataView dvRecord1 = dsItem2.Tables[0].DefaultView;
                        dvRecord1.RowFilter = "isnull(BatchId,0)<>0";

                        dsItem1.Tables.Add(dvRecord1.ToTable());

                        dsItem1.Tables[0].AcceptChanges();

                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.

                        dsItem1.WriteXml(writer);

                        hdnxmlString.Value = writer.ToString();
                        //hdnBatchXML.Value = writer.ToString();
                        ViewState["ByIndent"] = "Y";
                        ViewState["AutoPresc"] = "Y";
                        BindGridWithXml();

                        txtRegistrationNo.Enabled = false;
                        txtEncounter.Enabled = false;
                        txtPatientName.Enabled = false;
                        txtAge.Enabled = false;
                        ShowPatientDetails();
                        BindPatientProvisionalDiagnosis();
                    }
                }
            }

            ViewState["IsPrescriptionHelpSearch"] = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void lnkInsuranceDetails_Click(object sender, EventArgs e)
    {
        if (common.myStr(txtRegistrationNo.Text) != "")
        {
            RadWindow1.NavigateUrl = "~/PRegistration/InsuranceDetail.aspx?OPIP=O&RegistrationNo=" + txtRegistrationNo.Text + "&PayerId=" + ddlPayer.SelectedValue;
            RadWindow1.Height = 650;
            RadWindow1.Width = 960;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.OnClientClose = "BindInsuranceOnclose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select The Patient", Page);
            return;
        }
    }
    protected void btnFillInsurance_Click(object sender, EventArgs e)
    {
        hdnTotQty.Value = "0";
        hdnTotUnit.Value = "0";
        hdnTotCharge.Value = "0";
        hdnTotTax.Value = "0";
        hdnTotDiscAmt.Value = "0";
        hdnTotPatientAmt.Value = "0";
        hdnTotPayerAmt.Value = "0";
        hdnTotNetAmt.Value = "0";
        if (common.myStr(hdnCardValidDate.Value) == "")
        {
            hdnCardValidDate.Value = common.myStr(ViewState["CardValidDate"]);
        }
        else
        {
            ViewState["CardValidDate"] = hdnCardValidDate.Value;
        }
        if (common.myInt(hdnPayer.Value) != 0)
        {
            ViewState["OpCreditLimit"] = common.myStr(hdnOPCreditLimit.Value);
            ViewState["CoPaymentMaxLimit"] = common.myStr(hdnOPCopayMaxlimit.Value);
            ViewState["PharmacyCreditlimit"] = common.myStr(hdnPharmacyCreditlimit.Value);
            DataTable dt = (DataTable)ViewState["Servicetable"];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Request.QueryString["Code"] == "IPI")//Ipd sale
                    {
                        dr["CopayPerc"] = hdnphrIPCopay.Value;

                        ViewState["IPPharmacyCoPayPercent"] = hdnphrIPCopay.Value;

                    }
                    else
                    {
                        dr["CopayPerc"] = hdnphrOPCopay.Value;
                        ViewState["OPPharmacyCoPayPercent"] = hdnphrOPCopay.Value;
                    }
                }
            }

            dt = applyCoPayment(dt, common.myDec(ViewState["OPPharmacyCoPayMaxLimit"]));
            Session["IsFromRequestFromWard"] = 0;
            Session["ForBLKDiscountPolicy"] = 0;

            gvService.DataSource = dt;
            gvService.DataBind();

            setVisiblilityInteraction();

            ViewState["Servicetable"] = dt;
            //ddlPayParty.SelectedValue = "2";
            //ddlCompany.SelectedValue = hdnPayer.Value;
            //ddlCompany_OnSelectedIndexChanged(null, null);
            //ddlSponsor.SelectedValue = hdnSponsor.Value;
            //ddlCardId.SelectedValue = hdnCardId.Value;
            //ddlCardId_OnSelectedIndexChanged(null, null);
        }
    }
    protected void btnCopayHidden_Click(object sender, EventArgs e)
    {

    }
    private void setVisiblilityInteraction()
    {
        try
        {
            if (common.myBool(ViewState["IsCIMSInterfaceActive"]))
            {
                string strXML = getInterationXML("");

                string outputValues = string.Empty;

                if (strXML != "")
                {
                    outputValues = objCIMS.getFastTrack5Output(strXML);

                    foreach (GridViewRow dataItem in gvService.Rows)
                    {
                        HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

                        if (!outputValues.ToUpper().Contains("<SEVERITY NAME"))
                        {
                            lnkBtnInteractionCIMS.Visible = false;
                            continue;
                        }
                        else
                        {
                            if (common.myStr(CIMSItemId.Value).Trim().Length > 0 && lnkBtnInteractionCIMS.Visible)
                            {
                                string strFindCIMSItemId = "<PRODUCT REFERENCE=\"" + common.myStr(CIMSItemId.Value).Trim() + "\" NAME=\"\"></PRODUCT>";

                                if (outputValues.ToUpper().Contains(strFindCIMSItemId.ToUpper()))
                                {
                                    lnkBtnInteractionCIMS.Visible = false;
                                }
                            }
                        }
                    }
                }

                ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
                ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");

                strXML = getHealthOrAllergiesInterationXML("H");//Helth

                if (strXML != "")
                {
                    outputValues = objCIMS.getFastTrack5Output(strXML);

                    if (outputValues.Length > strXML.Length && outputValues.Length > 0)
                    {
                        if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
                        {
                            ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
                        }
                    }
                }

                strXML = getHealthOrAllergiesInterationXML("A");//Allergies

                if (strXML != "")
                {
                    outputValues = objCIMS.getFastTrack5Output(strXML);

                    if (outputValues.Length > strXML.Length && outputValues.Length > 0)
                    {
                        if (outputValues.ToUpper().Contains("<SEVERITY NAME"))
                        {
                            ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");
                        }
                    }
                }

                lnkDrugAllergy.BackColor = (System.Drawing.Color)ViewState["DrugAllergyColorSet"];


            }
            else if (common.myBool(ViewState["IsVIDALInterfaceActive"]))
            {
                int?[] commonNameGroupIds = getVIDALCommonNameGroupIds();
                lnkDrugAllergy.Visible = false;

                if (commonNameGroupIds.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable collVitalItemIdFound = new Hashtable();
                    clsVIDAL objVIDAL = new clsVIDAL(sConString);

                    sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);



                    DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
                    //int? weight = common.myInt(lbl_Weight.Text);//In kilograms
                    //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
                    int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
                    int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

                    Hashtable collVitalItemIdFoundDH = new Hashtable();

                    StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                            0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                            (ViewState["PatientDiagnosisXML"] != "") ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                            out collVitalItemIdFoundDH);

                    foreach (GridViewRow dataItem in gvService.Rows)
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

                    if (ViewState["PatientAllergyXML"] != "")
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
        catch
        {
        }
    }
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
            if (common.myBool(Application["IsCIMSInterfaceActive"]))
            {


                string strPrescribing = "";

                StringBuilder ItemIds = new StringBuilder();
                foreach (GridViewRow dataItem in gvService.Rows)
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
        //addToListFinal();

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

            string strPrescribing = "";

            StringBuilder ItemIds = new StringBuilder();
            foreach (GridViewRow dataItem in gvService.Rows)
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
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

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
            clsExceptionLog objException = new clsExceptionLog();
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
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
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
            clsExceptionLog objException = new clsExceptionLog();
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

            clsVIDAL objVIDAL = new clsVIDAL(sConString);
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
            StringBuilder sb = new StringBuilder();
            Hashtable collVitalItemIdFound = new Hashtable();
            clsVIDAL objVIDAL = new clsVIDAL(sConString);
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

            clsVIDAL objVIDAL = new clsVIDAL(sConString);
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

            clsVIDAL objVIDAL = new clsVIDAL(sConString);
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
    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();

            foreach (GridViewRow dataItem in gvService.Rows)
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

    private void setPatientInfo()
    {
        try
        {
            int? weight = null;

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet ds = objEMR.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == "Gender")
                    {
                        ViewState["PatientGender"] = ds.Tables[0].Rows[i][1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i][0].ToString() == "Age")
                    {
                        ViewState["PatientDOB"] = DateTime.Now.AddDays(-common.myInt(ds.Tables[0].Rows[i][1].ToString())).ToString("yyyy-MM-dd");
                    }
                }

                for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == "WT")// Weight
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
    protected void lnkPatientHistory_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(txtRegistrationNo.Text) != "")
        {
            RadWindow1.NavigateUrl = "~/EMRBilling/PatientHistory.aspx?Popup=Y&RegNo=" + txtRegistrationNo.Text;
            RadWindow1.Height = 650;
            RadWindow1.Width = 960;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select The Patient", Page);
            return;
        }
    }

    public void btnMarkedForDischarge_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMRBILLING/Popup/ExpectedDateOfDischarge.aspx";
        RadWindow1.Height = 610;
        RadWindow1.Width = 1050;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }

    public void NoofMarkfordischare()
    {
        BaseC.WardManagement objwm = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        ds = objwm.GetDischargeStatusNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblNoOfDischarge.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["mfd"]) + ")";
        }
        else
        {
            lblNoOfDischarge.Text = "(" + 0 + ")";
        }
    }

    protected void btnBarCodeValue_OnClick(object sender, EventArgs e)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(txtBarCodeValue.Text) != "")
            {
                if (common.myLen(txtPatientName.Text).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Patient or Enter Patient Name ! ";

                    return;
                }

                if (gvService.Rows.Count > 0)
                {
                    BindPreviousData();
                }

                ds = objPharmacy.getItemMasterBarCode(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                            common.myInt(ddlStore.SelectedValue), common.myStr(txtBarCodeValue.Text).Trim(), common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.

                    ds.WriteXml(writer);

                    hdnxmlString.Value = writer.ToString();
                    ViewState["ByIndent"] = "Y";
                    BindGridWithXml();

                    txtBarCodeValue.Text = string.Empty;
                    txtBarCodeValue.Focus();
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
            objPharmacy = null;
            ds.Dispose();
        }
    }



    protected void btnPendingPharmacyClearance_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=PHR", false);
        //RadWindow1.NavigateUrl = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=PHR";
        //RadWindow1.Height = 610;
        //RadWindow1.Width = 1050;
        //RadWindow1.Top = 10;
        //RadWindow1.Left = 10;
        //RadWindow1.OnClientClose = "";
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;
        //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }
    public void NoofPendingPharmacyClearance()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        ds = objPharmacy.GetPendingPharmacyClearanceCount(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblPharmacyClearance.Text = "(" + common.myStr(ds.Tables[0].Rows[0]["SFB"]) + ")";
        }
        else
        {
            lblPharmacyClearance.Text = "(" + 0 + ")";
        }
    }

    public void HideColumn()
    {
        Telerik.Web.UI.RadGrid grdPaymentMode = (Telerik.Web.UI.RadGrid)paymentMode.FindControl("grdPaymentMode");
        if (grdPaymentMode != null)
        {
            grdPaymentMode.Columns[8].Visible = false;
        }
    }


    protected void btnBarcodePrinting_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnIssueId.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select any Issue to Print !";
            return;
        }
        //string ItemId = "";


        string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

        //if (Act == "Y")
        //{
        //string BarcodeValue = "";
        //string ItemNo = "";
        //string BatchNo = "";
        //string ItemName = "";
        //string MRP = "";
        //string UnitName = "";
        //string ExpiryDate = "";
        //int NoofPrint = 0;
        //string NoofLabels = "";
        //foreach (GridViewRow rw in gvItem.Rows)
        //{
        //    HiddenField hdnItemID = (HiddenField)rw.FindControl("hdnItemID");
        //    if (((CheckBox)rw.FindControl("chkBarcode")).Checked == true)
        //    {
        //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        DataSet ds = dl.FillDataSet(CommandType.Text, "uspGetPhrItemBarcodePrinting " + common.myInt(Session["HospitalLocationID"]) + "," + common.myInt(Session["FacilityID"]).ToString() + "," + common.myInt(hdnGRNID.Value) + "," + common.myStr(hdnItemID.Value));
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            //For BarcodeValue
        //            if (BarcodeValue == "")
        //            {
        //                BarcodeValue = ds.Tables[0].Rows[0]["BarCodeValue"].ToString();
        //            }
        //            else
        //            {
        //                BarcodeValue = BarcodeValue + ";" + ds.Tables[0].Rows[0]["BarCodeValue"].ToString();
        //            }

        //            //For ItemNo
        //            if (ItemNo == "")
        //            {
        //                ItemNo = ds.Tables[0].Rows[0]["ItemNo"].ToString();
        //            }
        //            else
        //            {
        //                ItemNo = ItemNo + ";" + ds.Tables[0].Rows[0]["ItemNo"].ToString();
        //            }

        //            //For BatchNo
        //            if (BatchNo == "")
        //            {
        //                BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
        //            }
        //            else
        //            {
        //                BatchNo = BatchNo + ";" + ds.Tables[0].Rows[0]["BatchNo"].ToString();
        //            }

        //            //For ItemName
        //            if (ItemName == "")
        //            {
        //                ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
        //            }
        //            else
        //            {
        //                ItemName = ItemName + ";" + ds.Tables[0].Rows[0]["ItemName"].ToString();
        //            }

        //            //For MRP
        //            if (MRP == "")
        //            {
        //                MRP = ds.Tables[0].Rows[0]["MRP"].ToString();
        //            }
        //            else
        //            {
        //                MRP = MRP + ";" + ds.Tables[0].Rows[0]["MRP"].ToString();
        //            }

        //            //For UnitName
        //            if (UnitName == "")
        //            {
        //                UnitName = ds.Tables[0].Rows[0]["UnitName"].ToString();
        //            }
        //            else
        //            {
        //                UnitName = UnitName + ";" + ds.Tables[0].Rows[0]["UnitName"].ToString();
        //            }
        //            //For ExpiryDate
        //            if (ExpiryDate == "")
        //            {
        //                ExpiryDate = ds.Tables[0].Rows[0]["ExpiryDate"].ToString();
        //            }
        //            else
        //            {
        //                ExpiryDate = ExpiryDate + ";" + ds.Tables[0].Rows[0]["ExpiryDate"].ToString();
        //            }

        //            if (NoofLabels == "")
        //            {
        //                NoofLabels = ((TextBox)rw.FindControl("txtNoofLabels")).Text.ToString();
        //            }
        //            else
        //            {
        //                NoofLabels = NoofLabels + ";" + ((TextBox)rw.FindControl("txtNoofLabels")).Text.ToString();
        //            }


        //            NoofPrint = NoofPrint + 1;

        //            //string Str = "Parm=" + ds.Tables[0].Rows[0]["BarCodeValue"].ToString() + "#" + ds.Tables[0].Rows[0]["ItemNo"].ToString() + "#" + ds.Tables[0].Rows[0]["BatchNo"].ToString() + "#" + ds.Tables[0].Rows[0]["ItemName"].ToString() + "#" + ds.Tables[0].Rows[0]["MRP"].ToString() + "#" + ds.Tables[0].Rows[0]["UnitName"].ToString() + "#" + ds.Tables[0].Rows[0]["ExpiryDate"].ToString() + "#" + "0#GRN";
        //            //Str = "ASPLPrint:Doprint?" + Str;

        //            //ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
        //            ////return;
        //            //Thread.Sleep(200);
        //        }
        //        else
        //        {
        //            Alert.ShowAjaxMsg("No record Found", this);
        //            return;
        //        }
        //    }
        //}
        //if (NoofPrint == 1)
        //{
        //    string Str = "Parm=" + BarcodeValue + "#" + ItemNo + "#" + BatchNo + "#" + ItemName + "#" + MRP + "#" + UnitName + "#" + ExpiryDate + "#" + "0#GRN#" + NoofLabels;
        //    Str = "ASPLPrint:Doprint?" + Str;

        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
        //    return;
        //}
        //else if (NoofPrint > 1)
        //{
        //    string Str = "Parm=" + BarcodeValue + "#" + ItemNo + "#" + BatchNo + "#" + ItemName + "#" + MRP + "#" + UnitName + "#" + ExpiryDate + "#" + NoofPrint + "#GRN#" + NoofLabels;
        //    Str = "ASPLPrint:Doprint?" + Str;

        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
        //    return;
        //}
        //}
        //else
        //{
        //foreach (GridViewRow rw in gvItem.Rows)
        //{
        //    HiddenField hdnItemID = (HiddenField)rw.FindControl("hdnItemID");
        //    if (((CheckBox)rw.FindControl("chkBarcode")).Checked == true)
        //    {
        //        if (ItemId == "")
        //            ItemId += hdnItemID.Value;
        //        else
        //            ItemId += "," + hdnItemID.Value;
        //    }
        //}

        //if (ItemId == "")
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Item Not Selected";
        //    return;
        //}

        RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/PrintReport.aspx?rptType=IBP&RN=Issue Barcode Printing&IssueId=" + common.myInt(hdnIssueId.Value) + "&FacilityId=" + common.myInt(Session["FacilityId"]) + "&preview=P";

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        // RadWindowForNew.Title = "Time Slot";
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        divBarcode.Visible = false;
        //}

    }

    protected void ddlDiscountType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlDiscountType.SelectedValue == "SD")
        {
            lblEmpNo.Visible = true;
            txtBLKId.Visible = true;
            txtDiscPerc.ReadOnly = true;
            txtDiscPerc.Text = common.myStr(common.myInt(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim()));
            txtBLKId.Focus();
        }
        else if (ddlDiscountType.SelectedValue == "MD")
        {
            lblEmpNo.Visible = false;
            txtBLKId.Visible = false;
            txtDiscPerc.ReadOnly = false;
            txtDiscPerc.Text = "0";
            txtBLKId.Focus();
        }
        else
        {
            lblEmpNo.Visible = false;
            txtBLKId.Visible = false;
            txtDiscPerc.ReadOnly = true;
            txtDiscPerc.Text = common.myStr(common.myInt(ddlDiscountType.SelectedItem.Attributes["DiscountPercentage"].ToString().Trim()));
        }

    }

    protected void btnViewOrders_Click(object sender, EventArgs e)
    {
        //LinkButton lnkName = (LinkButton)sender;
        string sServiceId = "0";

        //HiddenField hdnServiceDetailId = (HiddenField)lnkName.FindControl("hdnDetailId");
        //HiddenField hdnClinicalDetailFound = (HiddenField)lnkName.FindControl("hdnClinicalDetailFound");
        //if (common.myInt(sServiceId) > 0)
        //{
        RadWindowForNew.NavigateUrl = "~/EMRBilling/Popup/ServicedetailsER.aspx?MPG=P22350&ServId=" + sServiceId + "&ServName=" +
           "" + "&RegNo=" + common.myStr(hdnRegistrationId.Value) + "&EncounterId=" +
           common.myStr(hdnEncounterId.Value) + "&RegId=" + common.myStr(hdnRegistrationId.Value) + "&InvoiceId=0";

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 20;
        RadWindowForNew.Left = 20;
        // RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
        //}
    }

    private void BindStore()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.clsPharmacy objPharmacy = new clsPharmacy(sConString);

            if (Request.QueryString["Wardid"] != null && !common.myStr(Request.QueryString["Wardid"]).Equals(string.Empty))
            {
                ds = objPharmacy.GetWardStoreTag(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myInt(Request.QueryString["Wardid"]), 3);
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlStore.DataSource = ds;
                    ddlStore.DataTextField = "DepartmentName";
                    ddlStore.DataValueField = "StoreId";
                    ddlStore.DataBind();
                    

                    if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I"
                         && Request.QueryString["DRUGORDERCODE"] == "DO" && Request.QueryString["LOCATION"] == "WARD")
                    {
                        ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
                        ViewState["Consumable"] = false;
                    }
                    if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I"
                        && Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "WARD")
                    {
                        ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                        common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);
                        ViewState["Consumable"] = true;
                    }
                    else if (Session["OPIP"] != null && Session["OPIP"].ToString() == "O" && Request.QueryString["DRUGORDERCODE"] == null)
                    {
                        ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), "DefaultOPIndentStoreId", sConString);
                        ViewState["Consumable"] = false;
                    }
                    else if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I"
                        && Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "OT")
                    {
                        ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), "DefaultOTIndentStoreId", sConString);
                        ViewState["Consumable"] = true;
                    }
                    else if (Session["OPIP"] != null && Session["OPIP"].ToString() == "E")
                    {
                        ddlStore.SelectedValue = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), "DefaultERIndentStoreId", sConString);
                        ViewState["Consumable"] = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
}
