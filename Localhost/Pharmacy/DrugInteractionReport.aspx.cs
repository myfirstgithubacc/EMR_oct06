using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using System.Xml.XPath;


public partial class Pharmacy_DrugInteractionReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    clsCIMS objCIMS = new clsCIMS();

    private enum enumColumns : byte
    {
        Select = 0,
        RegistrationNo = 1,
        EncounterNo = 2,
        PatientName = 3,
        GenderAge = 4,
        DrugName = 5,
        BrandDetailsCIMS = 6,
        MonographCIMS = 7,
        InteractionCIMS = 8,
        DHInteractionCIMS = 9,
        DAInteractionCIMS = 10,
        BrandDetailsVIDAL = 11,
        MonographVIDAL = 12,
        InteractionVIDAL = 13,
        DHInteractionVIDAL = 14,
        DAInteractionVIDAL = 15,
        IndentNo = 16,
        IndentDate = 17,
        SerumCreatine = 18,
        VisitType = 19,
        Location = 20,
        Provider = 21,
        OverrideDrugToDrugInteraction = 22,
        OverrideDrugHealthInteraction = 23,
        OverrideDrugAllergyInteraction = 24,
        PharmacistInteractionRemarks = 25,
        PrescriptionDetail = 26,
        IssueQty = 27
    }

    private enum enumCurrent : byte
    {
        DrugName = 0
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objException = new clsExceptionLog();
        objCIMS = new clsCIMS();

        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            txtFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            txtToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            txtFromDate.SelectedDate = DateTime.Now;
            txtToDate.SelectedDate = DateTime.Now;

            bindControl();

            clearControl();
            BindItemFlagMaster();

            #region Interface

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet dsInterface = new DataSet();

            dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                        BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.None);

            if (dsInterface.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]))
                {
                    Session["IsCIMSInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsCIMSInterfaceActive"]);
                    Session["CIMSDatabasePath"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]); //"G:\\Manmohan\\KochiTFS\\localhost\\CIMSDatabase\\";
                    Session["CIMSDatabasePassword"] = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePassword"]);
                }
                else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                {
                    Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);
                }

                getLegnedColor();
            }

            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                string CIMSDatabasePath = string.Empty;
                if (dsInterface.Tables[0].Rows.Count > 0)
                {
                    CIMSDatabasePath = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]); //"G:\\Manmohan\\KochiTFS\\localhost\\CIMSDatabase\\";
                }

                if (!File.Exists(CIMSDatabasePath + "FastTrackData.mrc") && !File.Exists(CIMSDatabasePath + "FastTrackData.mr2"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                    lblMessage.Text = "CIMS database not available !";
                    //Alert.ShowAjaxMsg("CIMS database not available !", this);
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

                    //    //Alert.ShowAjaxMsg(lblMessage.Text, this);
                    //}
                }
                catch
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "VIDAL web-services not running now !";

                    //Alert.ShowAjaxMsg(lblMessage.Text, this);
                }
            }

            #endregion

            bindData(true);
            // chkIsInteraction(0);
            // BoundField test = new BoundField();
            // test.DataField = "New DATAfield Name";
            // test.HeaderText = "New Header";
            //gvItem.Columns.Add(test)


            //GridBoundColumn boundColumn;

            ////Important: first Add column to the collection 
            //boundColumn = new GridBoundColumn();
            //this.gvItem.MasterTableView.Columns.Add(boundColumn);

            ////Then set properties 
            //boundColumn.DataField = "CustomerID";
            //boundColumn.HeaderText = "CustomerID";

        }

        setGridColor();
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            //bind provider
            ds = objP.getEmployeeWithResource(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), 0, common.myInt(Session["FacilityId"]));

            ddlProvider.DataSource = ds;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlProvider.SelectedIndex = 0;


            //bind ward
            ds = objadt.getWardMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlWard.SelectedIndex = 0;
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
            objP = null;
            objadt = null;
        }
    }

    private void bindData(bool IsPageLoad)
    {
        btnSearch.Enabled = false;
        btnSave.Enabled = false;

        lblMessage.Text = string.Empty;
        DataSet ds = new DataSet();
        DataTable tblAllData = new DataTable();
        DataTable tblCurrent = new DataTable();
        DataView DV = new DataView();
        BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);
        objCIMS = new clsCIMS();

        //  ViewState["VSForExport"] = null;

        try
        {
            string IndentNo = string.Empty;
            int RegistrationNo = 0;
            string EncounterNo = string.Empty;
            string PatientName = string.Empty;

            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "I":
                    IndentNo = txtSearch.Text.Trim();
                    break;
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "E":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            if (IsPageLoad)
            {
                ds = objP.getDrugInteraction(0, 0, string.Empty, 0, string.Empty, string.Empty, 0, 0, string.Empty, string.Empty,
                                    common.myInt(gvItem.PageSize), (gvItem.Items.Count > 0) ? gvItem.CurrentPageIndex + 1 : 1,
                                    Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                    Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                    common.myInt(Session["UserId"]), common.myInt(ddlItemcategory.SelectedValue));
            }
            else
            {
                ds = objP.getDrugInteraction(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        IndentNo, RegistrationNo, EncounterNo, PatientName, common.myInt(ddlWard.SelectedValue), common.myInt(ddlProvider.SelectedValue),
                                        common.myStr(ddlInteractionRemarksStatus.SelectedValue), common.myStr(ddlVisitType.SelectedValue),
                                        common.myInt(gvItem.PageSize), (gvItem.Items.Count > 0) ? gvItem.CurrentPageIndex + 1 : 1,
                                        Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                        common.myInt(Session["UserId"]), common.myInt(ddlItemcategory.SelectedValue));
            }

            tblAllData = ds.Tables[0];
            // ViewState["VSForExport"] = ds.Tables[0];

            if (ds.Tables.Count > 1)
            {
                ViewState["VSDiagnosis"] = ds.Tables[1];
            }
            if (ds.Tables.Count > 2)
            {
                ViewState["VSAllergy"] = ds.Tables[2];
            }

            #region Interaction

            try
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    objCIMS = new clsCIMS();
                    int intCounter = 1;
                    foreach (DataRow DR in tblAllData.Rows)
                    {
                        string strCIMSItemId = common.myStr(DR["CIMSItemId"]);
                        string strCIMSType = common.myStr(DR["CIMSType"]);

                        if (common.myLen(strCIMSItemId) < 3 || common.myLen(strCIMSType) < 3)
                        {
                            continue;
                        }

                        //Brand Details
                        string strXML = string.Empty;

                        if (strCIMSType.ToUpper().Equals("PRODUCT"))
                        {
                            strXML = getBrandDetailsXMLCIMS(strCIMSType, strCIMSItemId);

                            if (strXML != string.Empty)
                            {
                                string outputValues = objCIMS.getFastTrack5Output(strXML);


                                if (outputValues != null)
                                {
                                    if (common.myLen(outputValues) > 0)
                                    {
                                        string strPatternMatch = "<" + strCIMSType + " reference=\"" + strCIMSItemId + "\" name=";
                                        if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
                                        {
                                            DR["IsBD"] = 1;
                                        }
                                    }
                                }
                            }
                        }

                        //Monograph

                        strXML = getMonographXML(strCIMSType, strCIMSItemId);

                        if (strXML != string.Empty)
                        {
                            string outputValues = objCIMS.getFastTrack5Output(strXML);

                            if (outputValues != null)
                            {
                                if (common.myLen(outputValues) > 0)
                                {
                                    if (outputValues.ToUpper().Contains("<MONOGRAPH>"))
                                    {
                                        DR["IsMG"] = 1;
                                    }
                                }
                            }
                        }


                        //DD, DH, DA
                        string strOPIP = common.myStr(DR["OPIP"]);
                        int intRegistrationId = common.myInt(DR["RegistrationId"]);
                        int intEncounterId = common.myInt(DR["EncounterId"]);

                        setDiagnosis(intRegistrationId, intEncounterId);
                        setAllergiesWithInterfaceCode(intRegistrationId);

                        tblCurrent = new DataTable();
                        tblCurrent = getCurrentDrugData(strOPIP, intRegistrationId, intEncounterId, true); //chk

                        string strXMLDD = getInterationXML(tblCurrent, string.Empty);//DrugToDrug
                        string strXMLDH = getHealthOrAllergiesInterationXML(tblCurrent, "H", string.Empty);//Helth
                        string strXMLDA = getHealthOrAllergiesInterationXML(tblCurrent, "A", string.Empty);//Allergies

                        string outputValuesDD = string.Empty;
                        string outputValuesDH = string.Empty;
                        string outputValuesDA = string.Empty;

                        if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                        {
                            outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                            outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                            outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);

                            string strCIMSItemPatternMatch = "<" + ((common.myLen(strCIMSType) > 0) ? common.myStr(strCIMSType) : "Product") + " REFERENCE=\"" + common.myStr(strCIMSItemId).Trim() + "\" NAME=";

                            if (outputValuesDD != null)
                            {
                                if (common.myLen(outputValuesDD) > 0)
                                {
                                    if (objCIMS.IsDrugToDrugInteractionFound(strCIMSItemId, outputValuesDD, false) > 0)
                                    {
                                        if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                        {
                                            DR["IsDD"] = 1;
                                        }
                                    }
                                }
                            }

                            if (outputValuesDH != null)
                            {
                                if (common.myLen(outputValuesDH) > 0)
                                {
                                    if (objCIMS.IsDrugToHealthInteractionFound(strCIMSItemId, outputValuesDH) > 0)
                                    {
                                        if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                        {
                                            DR["IsDH"] = 1;
                                        }
                                    }
                                }
                            }

                            if (outputValuesDA != null)
                            {
                                if (common.myLen(outputValuesDA) > 0)
                                {
                                    if (objCIMS.IsDrugToAllergyInteractionFound(strCIMSItemId, outputValuesDA) > 0)
                                    {
                                        if (outputValuesDA.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                        {
                                            DR["IsDA"] = 1;
                                        }
                                    }
                                }
                            }
                        }

                        intCounter++;
                    }

                    DV = tblAllData.DefaultView;
                    //switch (common.myStr(ddlInteractionType.SelectedValue))
                    //{
                    //    case "DD":
                    //        DV.RowFilter = "IsDD=1";
                    //        break;

                    //    case "DH":
                    //        DV.RowFilter = "IsDH=1";
                    //        break;

                    //    case "DA":
                    //        DV.RowFilter = "IsDA=1";
                    //        break;
                    //}

                    tblAllData = DV.ToTable();
                }
            }
            catch (Exception ee)
            {
                objException.HandleException(ee);
            }

            #endregion

            if (tblAllData.Rows.Count == 0)
            {
                DataRow DR = tblAllData.NewRow();
                tblAllData.Rows.Add(DR);
            }

            gvItem.VirtualItemCount = common.myInt(tblAllData.Rows[0]["TotalRecordsCount"]);

            gvItem.DataSource = tblAllData;
            gvItem.DataBind();
            //  chkIsInteraction(0);

            bindCurrentDrug(string.Empty, 0, 0);
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
            tblAllData.Dispose();
            tblCurrent.Dispose();
            DV.Dispose();
            objP = null;

            btnSearch.Enabled = true;
            btnSave.Enabled = true;
        }
    }


    private void clearControl()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            ddlSearchCriteria.SelectedIndex = 0;
            txtSearchRegNo.Text = string.Empty;
            txtSearch.Text = string.Empty;
            ddlProvider.SelectedIndex = 0;
            ddlWard.SelectedIndex = 0;
            ddlInteractionRemarksStatus.SelectedValue = "U";
            ddlVisitType.SelectedIndex = 0;
            txtFromDate.SelectedDate = DateTime.Now;
            txtToDate.SelectedDate = DateTime.Now;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        gvItem.CurrentPageIndex = 0;
        bindData(false);
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData(false);
    }



    protected void gvItem_OnPageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvItem.CurrentPageIndex = e.NewPageIndex;
        bindData(false);
    }

    protected void btnSave_OnClick(Object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);
        try
        {
            StringBuilder strXMLItems = new StringBuilder();
            ArrayList coll = new ArrayList();

            foreach (GridItem item in gvItem.Items)
            {
                TextBox txtPharmacistInteractionRemarks = (TextBox)item.FindControl("txtPharmacistInteractionRemarks");
                HiddenField hdnIndentDetailsId = (HiddenField)item.FindControl("hdnIndentDetailsId");
                HiddenField hdnInteractionRemarksStatus = (HiddenField)item.FindControl("hdnInteractionRemarksStatus");

                if (common.myLen(txtPharmacistInteractionRemarks.Text) > 0
                    && common.myInt(hdnIndentDetailsId.Value) > 0
                    && !common.myStr(hdnInteractionRemarksStatus.Value).Equals("R"))
                {
                    HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
                    HiddenField hdnOPIP = (HiddenField)item.FindControl("hdnOPIP");

                    coll.Add(common.myStr(hdnOPIP.Value));//VisitType CHAR(1),      
                    coll.Add(common.myInt(hdnIndentDetailsId.Value));//IndentDetailsId INT,
                    coll.Add(common.myInt(hdnItemId.Value));//ItemId INT,
                    coll.Add(common.myStr(txtPharmacistInteractionRemarks.Text).Trim());//PharmacistInteractionRemarks VARCHAR(2000)

                    strXMLItems.Append(common.setXmlTable(ref coll));
                }
            }

            if (strXMLItems.ToString().Equals(string.Empty))
            {
                lblMessage.Text = "Pharmacist interaction remarks can't be blank!";
                return;
            }

            string strMsg = objP.saveDrugInteraction(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                strXMLItems.ToString(), common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindData(false);

                lblMessage.Text = strMsg;
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
            objP = null;
        }
    }

    protected void ddlInteractionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(false);
    }

    protected void ddlWard_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(false);
    }

    protected void ddlProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(false);
    }

    protected void ddlInteractionRemarksStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(false);
    }

    protected void ddlVisitType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(false);
    }

    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearchRegNo.Text = string.Empty;
        txtSearch.Text = string.Empty;

        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {

            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "I":
            case "E":
            case "P":
                txtSearch.Visible = true;
                break;
        }
    }

    protected void gvItem_OnItemCommand(object Sender, GridCommandEventArgs e)
    {
        ViewState["MainIndentId"] = string.Empty;

        if (e.CommandName == "Page")
        {
            return;
        }

        DataTable tblCurrent = new DataTable();

        try
        {
            //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            HiddenField hdnOPIP = (HiddenField)e.Item.FindControl("hdnOPIP");
            HiddenField hdnRegistrationId = (HiddenField)e.Item.FindControl("hdnRegistrationId");
            HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");

            tblCurrent = new DataTable();
            if (e.CommandName == "InteractionCIMS"
                || e.CommandName == "DHInteractionCIMS"
                || e.CommandName == "DAInteractionCIMS")
            {
                tblCurrent = getCurrentDrugData(common.myStr(hdnOPIP.Value), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), true);

                ////setPatientInfo(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value));
                setDiagnosis(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value));
                setAllergiesWithInterfaceCode(common.myInt(hdnRegistrationId.Value));
            }

            if (e.CommandName == "RowSelect")
            {
                HiddenField hdnIndentId = (HiddenField)e.Item.FindControl("hdnIndentId");

                ViewState["MainIndentId"] = common.myInt(hdnIndentId.Value).ToString();

                bindCurrentDrug(common.myStr(hdnOPIP.Value), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value));

                foreach (GridItem dataItem in gvItem.Items)
                {
                    dataItem.BackColor = System.Drawing.Color.White;
                }

                e.Item.BackColor = System.Drawing.Color.Pink;
            }
            else if (e.CommandName == "BrandDetailsCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                HiddenField hdnCIMSType = (HiddenField)e.Item.FindControl("hdnCIMSType");

                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                HiddenField hdnCIMSType = (HiddenField)e.Item.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction(tblCurrent);
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction(tblCurrent, "H");
            }
            else if (e.CommandName == "DAInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction(tblCurrent, "A");
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
                showHealthOrAllergiesIntreraction(tblCurrent, "H");
            }
            else if (e.CommandName == "DAInteractionVIDAL")
            {
                showHealthOrAllergiesIntreraction(tblCurrent, "A");
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
            tblCurrent.Dispose();
        }
    }


    protected void gvPrevious_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

            if (!common.myInt(ViewState["MainIndentId"]).Equals(common.myInt(hdnIndentId.Value)))
            {
                e.Row.BackColor = System.Drawing.Color.CadetBlue;
            }
        }
    }

    //protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    //{
    //}

    protected void bindCurrentDrug(string OPIP, int RegistrationId, int EncounterId)
    {
        DataTable tblDistinct = new DataTable();
        try
        {
            if (OPIP.Equals(string.Empty) && EncounterId.Equals(0))
            {
                DataColumn col = new DataColumn("ItemId", typeof(int));
                tblDistinct.Columns.Add(col);

                col = new DataColumn("ItemName", typeof(string));
                tblDistinct.Columns.Add(col);

                col = new DataColumn("IndentId", typeof(int));
                tblDistinct.Columns.Add(col);
                col = new DataColumn("IssuedItemName", typeof(string));
                tblDistinct.Columns.Add(col);
            }
            else
            {
                tblDistinct = getCurrentDrugData(OPIP, RegistrationId, EncounterId, false);
            }

            if (tblDistinct.Rows.Count.Equals(0))
            {
                DataRow DR = tblDistinct.NewRow();
                tblDistinct.Rows.Add(DR);
            }

            gvPrevious.DataSource = tblDistinct;
            gvPrevious.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            tblDistinct.Dispose();
        }
    }

    protected DataTable getCurrentDrugData(string OPIP, int RegistrationId, int EncounterId, bool IsChkDistinct)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        DataSet ds = new DataSet();
        DataView DV = new DataView();
        DataTable tblDistinct = new DataTable();
        try
        {
            if (OPIP.Equals("I"))
            {
                ds = objPharmacy.getPreviousMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                EncounterId, 0, 0, "P", string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                ds = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                EncounterId, RegistrationId, 0, 0, "P", string.Empty, string.Empty, string.Empty);
            }

            DV = new DataView(ds.Tables[0]);

            DV.RowFilter = "ISNULL(DoctorCategory,0)=1 AND ItemCategoryShortName IN ('MED')";
            DV.Sort = "IndentId DESC";

            tblDistinct = DV.ToTable();

            //remove repeated drug
            if (IsChkDistinct)
            {
                tblDistinct = DV.ToTable().Clone();
                DataView dvDistinct;
                foreach (DataRow item in DV.ToTable().Rows)
                {
                    dvDistinct = tblDistinct.Copy().DefaultView;

                    dvDistinct.RowFilter = "ItemName='" + common.myStr(item["ItemName"]) + "'";

                    if (dvDistinct.Count == 0)
                    {
                        dvDistinct.RowFilter = string.Empty;
                        tblDistinct.ImportRow(item);
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
            objPharmacy = null;
            objEMR = null;
            ds.Dispose();
            DV.Dispose();
        }

        return tblDistinct;
    }

    ///////////////CIMS/////////////////////////////
    private void setPatientInfo(int RegistrationId, int EncounterId)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            int? weight = null;

            ds = objEMR.getScreeningParameters(EncounterId, RegistrationId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["PatientWeight"] = string.Empty;
                ViewState["PatientHeight"] = string.Empty;

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
                            ViewState["PatientWeight"] = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                        else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("HT"))// Height
                        {
                            ViewState["PatientHeight"] = common.myStr(ds.Tables[0].Rows[i][1]);
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
        if (common.myBool(Session["IsCIMSInterfaceActive"])
            || common.myBool(Session["IsVIDALInterfaceActive"]))
        {
            foreach (GridItem dataItem in gvItem.Items)
            {
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)dataItem.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)dataItem.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");

                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
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

        }
    }

    private void setDiagnosis(int RegistrationId, int EncounterId)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            ViewState["PatientDiagnosisXML"] = string.Empty;

            if (ViewState["VSDiagnosis"] == null)
            {
                return;
            }

            if (ViewState["VSDiagnosis"] == string.Empty)
            {
                return;
            }

            //<HealthIssueCodes>
            //    <HealthIssueCode code="K22" codeType="ICD10" />
            //    <HealthIssueCode code="K22.0" codeType="ICD10" />
            //</HealthIssueCodes>

            //ViewState["PatientDiagnosisXML"] = "<HealthIssueCodes><HealthIssueCode code=\"J45\" codeType=\"ICD10\" /><HealthIssueCode code=\"N17\" codeType=\"ICD10\" /><HealthIssueCode code=\"I11\" codeType=\"ICD10\" /><HealthIssueCode code=\"F32\" codeType=\"ICD10\" /></HealthIssueCodes>";

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                //ds = objEMR.getDiagnosis(common.myInt(Session["HospitalLocationID"]), RegistrationId, EncounterId);
                DataTable tblDiagnosis = (DataTable)ViewState["VSDiagnosis"];

                if (tblDiagnosis.Rows.Count > 0)
                {
                    DV = tblDiagnosis.Copy().DefaultView;
                    DV.RowFilter = "RegistrationId=" + RegistrationId + " AND EncounterId=" + EncounterId;

                    tbl = DV.ToTable();

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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            DV.RowFilter = string.Empty;
        }
    }

    private void setAllergiesWithInterfaceCode(int RegistrationId)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            ViewState["PatientAllergyXML"] = string.Empty;

            if (ViewState["VSAllergy"] == null)
            {
                return;
            }

            if (ViewState["VSAllergy"] == string.Empty)
            {
                return;
            }

            if (common.myBool(Session["IsCIMSInterfaceActive"])
                || common.myBool(Session["IsVIDALInterfaceActive"]))
            {
                //ds = objEMR.getDrugAllergiesInterfaceCode(common.myInt(Session["HospitalLocationID"]), RegistrationId);

                DataTable tblAllergy = (DataTable)ViewState["VSAllergy"];

                DV = tblAllergy.Copy().DefaultView;
                tbl = new DataTable();

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    DV.RowFilter = "RegistrationId=" + RegistrationId + " AND AllergyType='CIMS'";
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
                    DV.RowFilter = "RegistrationId=" + RegistrationId + " AND AllergyType='VIDAL'";
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
            objEMR = null;
            DV.RowFilter = string.Empty;
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

    private void showIntreraction(DataTable tblCurrent)
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(tblCurrent, string.Empty);

        if (strXML != string.Empty)
        {
            Session["CIMSXMLInputData"] = strXML;
            openWindowsCIMS(false);
        }
    }

    private void showHealthOrAllergiesIntreraction(DataTable tblCurrent, string HealthOrAllergies)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            if (common.myLen(Session["CIMSXMLInputData"]) > 0)
            {
                Session["CIMSXMLInputData"] = string.Empty;
            }

            string strXML = getHealthOrAllergiesInterationXML(tblCurrent, "B", string.Empty);

            if (strXML != string.Empty)
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

    private string getInterationXML(DataTable tblCurrent, string strNewPrescribing)
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

                foreach (DataRow dataItem in tblCurrent.Rows)
                {
                    string CIMSItemId = common.myStr(dataItem["CIMSItemId"]);

                    if (common.myLen(CIMSItemId) > 2)
                    {
                        string CIMSType = common.myStr(dataItem["CIMSTYPE"]);
                        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

                        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId) + "\" />";
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

    private string getHealthOrAllergiesInterationXML(DataTable tblCurrent, string useFor, string strNewPrescribing)
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

            foreach (DataRow dataItem in tblCurrent.Rows)
            {
                string CIMSItemId = common.myStr(dataItem["CIMSItemId"]);

                if (common.myLen(CIMSItemId) > 2)
                {
                    string CIMSType = common.myStr(dataItem["CIMSTYPE"]);
                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";

                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId) + "\" />";
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

    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();

            foreach (GridItem dataItem in gvItem.Items)
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
            //commonNameGroupIds = new int?[] { 15223, 15070, 1524, 4025, 4212, 516 };

            StringBuilder sb = new StringBuilder();

            Hashtable collVitalItemIdFound = new Hashtable();

            sb = objVIDAL.getVIDALDrugToDrugInteraction(false, commonNameGroupIds, out collVitalItemIdFound);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
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
        }
    }

    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };

            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(ViewState["PatientWeight"]);//In kilograms
            //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
            int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
            int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

            Hashtable collVitalItemIdFoundDH = new Hashtable();

            StringBuilder sb = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                    0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                    (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                    out collVitalItemIdFoundDH);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
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

            if (ViewState["PatientAllergyXML"] != string.Empty)
            {
                allergyIds = (int?[])ViewState["PatientAllergyXML"];
            }

            StringBuilder sb = new StringBuilder();

            sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

            if (Cache["VIDALXML" + common.myStr(Session["UserId"])] != null)
            {
                Cache.Remove("VIDALXML" + common.myStr(Session["UserId"]));
            }

            if (sb.ToString() != string.Empty)
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

            if (sb.ToString() != string.Empty)
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
    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        try
        {
            //DataTable table1 = (DataTable)ViewState["VSForExport"];
            DataTable dt = new DataTable();
            string IndentNo = string.Empty;
            string EncounterNo = string.Empty;
            string PatientName = string.Empty;
            BaseC.clsPharmacy objP = new BaseC.clsPharmacy(sConString);

            //dt = objP.getDrugInteraction(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            //                        IndentNo, 0, EncounterNo, PatientName, common.myInt(ddlWard.SelectedValue), common.myInt(ddlProvider.SelectedValue),
            //                        common.myStr(ddlInteractionRemarksStatus.SelectedValue), common.myStr(ddlVisitType.SelectedValue),
            //                        common.myInt(gvItem.PageSize), (gvItem.Items.Count > 0) ? gvItem.CurrentPageIndex + 1 : 1,
            //                        Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
            //                        Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
            //                        common.myInt(Session["UserId"])).Tables[0];
            dt = objP.getDrugInteraction(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                   IndentNo, 0, EncounterNo, PatientName, common.myInt(ddlWard.SelectedValue), common.myInt(ddlProvider.SelectedValue),
                                   common.myStr(ddlInteractionRemarksStatus.SelectedValue), common.myStr(ddlVisitType.SelectedValue),
                                  0, 0,
                                   Convert.ToDateTime(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                   Convert.ToDateTime(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd"),
                                   common.myInt(Session["UserId"])).Tables[0];


            dt.Columns.RemoveColumns("RowNumber", "VisitSequence", "IndentDetailsId", "IndentId", "ItemId", "RegistrationId", "EncounterId", "OPIP", "CIMSItemId", "CIMSTYPE", "VIDALItemId", "WardId", "IsBD", "IsMG", "IsDD", "IsDH", "IsDA", "InteractionRemarksStatus", "SerumCreatine", "Location", "OverrideCommentsDrugToDrug", "OverrideCommentsDrugHealth", "OverrideComments", "TotalRecordsCount");

            // ExportToExcel(table1, "DrugInteractionReport");
            string FileName = "DrugInteractionReport.xls";


            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dt;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);

            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void BindItemFlagMaster()
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            DataSet ds = objPharmacy.GetItemFlagMaster(0, common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlItemcategory.DataSource = ds;
                ddlItemcategory.DataTextField = "ItemFlagName";
                ddlItemcategory.DataValueField = "ItemFlagId";
                ddlItemcategory.DataBind();

                ddlItemcategory.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlItemcategory.SelectedIndex = 0;
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    //private void chkIsInteraction()
    //{
    //    clsCIMS objCIMS = new clsCIMS();
    //    try
    //    {
    //        //if (common.myBool(Session["IsCIMSInterfaceActive"]))
    //        //{
    //        foreach (GridDataItem dataItem in gvItem.Items)
    //        {
    //            HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
    //            HiddenField hdnCIMSTYPE = (HiddenField)dataItem.FindControl("hdnCIMSTYPE");

    //            //string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";
    //            string strPrescribing = "<" + common.myStr(hdnCIMSTYPE.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";
    //            string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);
    //            if (!strXML.Equals(string.Empty))
    //            {
    //                //string outputValues = objCIMS.getFastTrack5Output(strXML);
    //                string outputValues = getFastTrack5Output(strXML);


    //                #region  Severity
    //                string Severity = IsDrugToDrugSeverity(common.myStr(hdnCIMSItemId.Value), outputValues);
    //                GridBoundColumn boundColumn;

    //                //Important: first Add column to the collection 
    //                boundColumn = new GridBoundColumn();
    //                this.gvItem.MasterTableView.Columns.Add(boundColumn);

    //                //Then set properties 
    //                boundColumn.DataField = Severity;
    //                boundColumn.HeaderText = "SeverityCode";
    //                #endregion

    //                //if (outputValues != null)
    //                //{
    //                //    //Drug to Drug Interation
    //                //    if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues, true) > 0)
    //                //    {
    //                //        string strPatternMatch = "<" + common.myStr(hdnCIMSTYPE.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
    //                //        if (outputValues.ToUpper().Contains(strPatternMatch.ToUpper()))
    //                //        {
    //                //            ViewState["NewPrescribing"] = strXML;
    //                //        }
    //                //    }

    //                //    //Drug Allergy Interation
    //                //    if (objCIMS.IsDrugToAllergyInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValues) > 0)
    //                //    {
    //                //        string strPatternMatch = "<" + common.myStr(hdnCIMSTYPE.Value) + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";
    //                //        ViewState["NewPrescribing"] = strXML;
    //                //    }
    //                //}
    //            }
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
    //        objCIMS = null;
    //    }
    //}

    private string getHealthOrAllergiesInterationXML(string useFor, string strNewPrescribing)
    {
        string strXML = string.Empty;
        try
        {

            string strPrescribing = string.Empty;
            StringBuilder ItemIds = new StringBuilder();
            if (!strNewPrescribing.Equals(string.Empty))
            {
                ItemIds.Append(strNewPrescribing);
            }
            //foreach (GridViewRow dataItem in gvPrevious.Rows)
            //{
            //    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
            //    if (common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0"))
            //    {
            //        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
            //        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
            //        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
            //        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
            //        {
            //            ItemIds.Append(strPres);
            //        }
            //    }
            //}
            foreach (GridDataItem dataItem in gvItem.Items)
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

    public string getFastTrack5Output(string CIMSXML)
    {
        string outputValues = string.Empty;
        try
        {
            string queryString = CIMSXML;

            string CIMSDatabasePath = common.myStr(HttpContext.Current.Session["CIMSDatabasePath"]);
            string CIMSDatabasePassword = common.myStr(HttpContext.Current.Session["CIMSDatabasePassword"]);


            //string CIMSDatabasePath = "C:\\Interface\\CIMSDatabase\\";
            //string CIMSDatabasePassword = common.myStr("62WY9X5U8s");

            //Monograph
            //string queryString = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";

            //Interaction
            //string queryString = "<Request><Interaction><Prescribing><Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" /></Prescribing><Prescribed><Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" /><Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" /></Prescribed><Allergies /><References/></Interaction></Request>";

            string retultInfo = string.Empty;
            string guid = string.Empty;

            //string dataPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) +
            //                    "CIMSDatabase\\FastTrackData.mrc";

            //string CIMSDatabasePassword = "GDDBDEMO";
            //string CIMSDatabasePassword = "5ksQ7b49AR";

            string initString = string.Empty;

            if (File.Exists(CIMSDatabasePath + "FastTrackData.mrc"))
            {
                initString = "<Initialize><DataFile password='" + CIMSDatabasePassword + "' path='" + CIMSDatabasePath + "FastTrackData.mrc" + "' /></Initialize>";
            }

            FastTrack5.FastTrack_Creator ftCreator = new FastTrack5.FastTrack_Creator();
            FastTrack5.IFastTrack_Server ftServer;

            ftServer = ftCreator.CreateServer(initString, out retultInfo, out guid);

            outputValues = ftServer.RequestXML(queryString, out retultInfo);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "getFastTrack5Output");
        }
        return outputValues;
    }



    protected void gvItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataTable tblCurrent = new DataTable();

            try
            {
                HiddenField hdnInteractionRemarksStatus = (HiddenField)e.Item.FindControl("hdnInteractionRemarksStatus");

                TextBox txtPharmacistInteractionRemarks = (TextBox)e.Item.FindControl("txtPharmacistInteractionRemarks");

                Label lblOverrideCommentsDrugToDrug = (Label)e.Item.FindControl("lblOverrideCommentsDrugToDrug");
                Label lblOverrideCommentsDrugHealth = (Label)e.Item.FindControl("lblOverrideCommentsDrugHealth");
                Label lblOverrideComments = (Label)e.Item.FindControl("lblOverrideComments");

                txtPharmacistInteractionRemarks.ToolTip = txtPharmacistInteractionRemarks.Text;
                lblOverrideCommentsDrugToDrug.ToolTip = lblOverrideCommentsDrugToDrug.Text;
                lblOverrideCommentsDrugHealth.ToolTip = lblOverrideCommentsDrugHealth.Text;
                lblOverrideComments.ToolTip = lblOverrideComments.Text;

                if (common.myStr(hdnInteractionRemarksStatus.Value).Equals("R"))
                {
                    e.Item.BackColor = System.Drawing.Color.LightGreen;
                    txtPharmacistInteractionRemarks.Enabled = false;
                }
                else
                {
                    txtPharmacistInteractionRemarks.Enabled = true;
                }



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

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.BrandDetailsCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.MonographCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionCIMS].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DAInteractionCIMS].Visible = true;

                    HiddenField hdnCIMSItemId = (HiddenField)e.Item.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnBrandDetailsCIMS = (LinkButton)e.Item.FindControl("lnkBtnBrandDetailsCIMS");
                    LinkButton lnkBtnMonographCIMS = (LinkButton)e.Item.FindControl("lnkBtnMonographCIMS");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)e.Item.FindControl("lnkBtnInteractionCIMS");
                    LinkButton lnkBtnDHInteractionCIMS = (LinkButton)e.Item.FindControl("lnkBtnDHInteractionCIMS");
                    LinkButton lnkBtnDAInteractionCIMS = (LinkButton)e.Item.FindControl("lnkBtnDAInteractionCIMS");

                    lnkBtnBrandDetailsCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                    lnkBtnBrandDetailsCIMS.Visible = false;
                    lnkBtnMonographCIMS.Visible = false;
                    lnkBtnInteractionCIMS.Visible = false;
                    lnkBtnDHInteractionCIMS.Visible = false;
                    lnkBtnDAInteractionCIMS.Visible = false;

                    if (common.myLen(hdnCIMSItemId.Value) > 2)
                    {
                        HiddenField hdnIsBD = (HiddenField)e.Item.FindControl("hdnIsBD");
                        HiddenField hdnIsMG = (HiddenField)e.Item.FindControl("hdnIsMG");
                        HiddenField hdnIsDD = (HiddenField)e.Item.FindControl("hdnIsDD");
                        HiddenField hdnIsDH = (HiddenField)e.Item.FindControl("hdnIsDH");
                        HiddenField hdnIsDA = (HiddenField)e.Item.FindControl("hdnIsDA");

                        if (common.myBool(hdnIsBD.Value))
                        {
                            lnkBtnBrandDetailsCIMS.Visible = true;
                        }
                        if (common.myBool(hdnIsMG.Value))
                        {
                            lnkBtnMonographCIMS.Visible = true;
                        }
                        if (common.myBool(hdnIsDD.Value))
                        {
                            lnkBtnInteractionCIMS.Visible = true;
                        }
                        if (common.myBool(hdnIsDH.Value))
                        {
                            lnkBtnDHInteractionCIMS.Visible = true;
                        }
                        if (common.myBool(hdnIsDA.Value))
                        {
                            lnkBtnDAInteractionCIMS.Visible = true;
                        }
                    }

                    switch (common.myStr(ddlInteractionType.SelectedValue))
                    {
                        case "DD":
                            if (!lnkBtnInteractionCIMS.Visible)
                            {
                                e.Item.Display = false;
                            }
                            break;

                        case "DH":
                            if (!lnkBtnDHInteractionCIMS.Visible)
                            {
                                e.Item.Display = false;
                            }
                            break;

                        case "DA":
                            if (!lnkBtnDAInteractionCIMS.Visible)
                            {
                                e.Item.Display = false;
                            }
                            break;
                    }

                }
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    gvItem.Columns[(byte)enumColumns.BrandDetailsVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.MonographVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.InteractionVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DHInteractionVIDAL].Visible = true;
                    gvItem.Columns[(byte)enumColumns.DAInteractionVIDAL].Visible = true;

                    HiddenField hdnVIDALItemId = (HiddenField)e.Item.FindControl("hdnVIDALItemId");
                    LinkButton lnkBtnBrandDetailsVIDAL = (LinkButton)e.Item.FindControl("lnkBtnBrandDetailsVIDAL");
                    LinkButton lnkBtnMonographVIDAL = (LinkButton)e.Item.FindControl("lnkBtnMonographVIDAL");
                    LinkButton lnkBtnInteractionVIDAL = (LinkButton)e.Item.FindControl("lnkBtnInteractionVIDAL");
                    LinkButton lnkBtnDHInteractionVIDAL = (LinkButton)e.Item.FindControl("lnkBtnDHInteractionVIDAL");
                    LinkButton lnkBtnDAInteractionVIDAL = (LinkButton)e.Item.FindControl("lnkBtnDAInteractionVIDAL");

                    lnkBtnBrandDetailsVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["BrandDetailsColor"]));
                    lnkBtnMonographVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugMonographColor"]));
                    lnkBtnInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));
                    lnkBtnDHInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));
                    lnkBtnDAInteractionVIDAL.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                    if (common.myInt(hdnVIDALItemId.Value) == 0)
                    {
                        lnkBtnBrandDetailsVIDAL.Visible = false;
                        lnkBtnMonographVIDAL.Visible = false;
                        lnkBtnInteractionVIDAL.Visible = false;
                        lnkBtnDHInteractionVIDAL.Visible = false;
                        lnkBtnDAInteractionVIDAL.Visible = false;
                    }
                }
                #region Check Severity
                if (e.Item is GridDataItem)
                {
                    if (common.myBool(Session["IsCIMSInterfaceActive"]))
                    {
                        GridDataItem dataBoundItem = e.Item as GridDataItem;
                        HiddenField hdnOPIP = (HiddenField)e.Item.FindControl("hdnOPIP");
                        HiddenField hdnRegistrationId = (HiddenField)e.Item.FindControl("hdnRegistrationId");
                        HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");

                        if (!common.myStr(ddlDrugSeverity.SelectedValue).Equals("A"))
                        {

                            //GridDataItem item = (GridDataItem)e.Item;
                            //if (item.GetDataKeyValue("EmployeeID").ToString() == "4")  //set your condition for hiding the row
                            //{
                            //    item.Display = false;  //hide the row
                            //}

                            HiddenField hdnCIMSItemId = (HiddenField)e.Item.FindControl("hdnCIMSItemId");
                            HiddenField hdnCIMSTYPE = (HiddenField)e.Item.FindControl("hdnCIMSTYPE");

                            //string strPrescribing = "<Prescribing><" + common.myStr(hdnCIMSType.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" /></Prescribing>";
                            string strPrescribing = "<" + common.myStr(hdnCIMSTYPE.Value) + " reference=\"" + common.myStr(hdnCIMSItemId.Value) + "\" />";
                            // string strXML = getHealthOrAllergiesInterationXML("B", strPrescribing);

                            string strOPIP = common.myStr(hdnOPIP.Value);
                            int intRegistrationId = common.myInt(hdnRegistrationId.Value);
                            int intEncounterId = common.myInt(hdnEncounterId.Value);

                            setDiagnosis(intRegistrationId, intEncounterId);
                            //setAllergiesWithInterfaceCode(intRegistrationId);

                            tblCurrent = new DataTable();
                            tblCurrent = getCurrentDrugData(strOPIP, intRegistrationId, intEncounterId, true); //chk



                            string strXML = getInterationXML(tblCurrent, string.Empty);//DrugToDrug
                            if (!strXML.Equals(string.Empty))
                            {
                                //string outputValues = objCIMS.getFastTrack5Output(strXML);
                                string outputValues = getFastTrack5Output(strXML);
                                string Severity = IsDrugToDrugSeverity(common.myStr(hdnCIMSItemId.Value), outputValues);

                                if (common.myStr(ddlDrugSeverity.SelectedValue).Equals(Severity))
                                {
                                    dataBoundItem.Display = true;
                                }
                                else
                                {
                                    dataBoundItem.Display = false;
                                }
                            }
                            else
                            {
                                dataBoundItem.Display = false;
                            }
                        }
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
                tblCurrent.Dispose();
            }
        }

    }

    public string IsDrugToDrugSeverity(string strCIMSItemId, string outputValues)
    {
        string dblOutput = string.Empty;

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();

                //count(//Interaction/*[@reference='{673FE2DD-4B75-4EFA-85DA-7957A90AF7A2}' and not(@Mirror)]/Allergy)
                string querySevere = "count (//Interaction/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Severe']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Severe'])";
                //string querySevere = "count (//Interaction/*[@reference='{497F3D6E-906D-4E0B-B3D5-CEC3C959865D}'and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Severe' or @name='Moderate' or @name='Minor' or @name='Caution']|//Interaction/*/Route/*[@reference='{497F3D6E-906D-4E0B-B3D5-CEC3C959865D}'and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Severe' or @name='Moderate' or @name='Minor'or @name='Caution']) ";

                string queryModerate = "count (//Interaction/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Moderate']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Moderate'])";
                string queryMinor = "count (//Interaction/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Minor']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Minor'])";
                string queryCaution = "count (//Interaction/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Caution']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "'and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Caution'])";

                var exprSevere = nav.Compile(querySevere);
                var exprModerate = nav.Compile(queryModerate);
                var exprMinor = nav.Compile(queryMinor);
                var exprCaution = nav.Compile(queryCaution);

                //switch (expr.ReturnType)
                //{
                //    case XPathResultType.Number:
                //        dblOutput = common.myDbl(nav.Evaluate(query));
                //}


                if (exprSevere.ReturnType.Equals(XPathResultType.Number))
                {
                    if (common.myDbl(nav.Evaluate(querySevere)) > 0)
                    {
                        dblOutput = "S";
                    }
                }

                if (exprModerate.ReturnType.Equals(XPathResultType.Number))
                {
                    if (common.myDbl(nav.Evaluate(queryModerate)) > 0)
                    {
                        dblOutput = "MO";
                    }
                }

                if (exprMinor.ReturnType.Equals(XPathResultType.Number))
                {
                    if (common.myDbl(nav.Evaluate(queryMinor)) > 0)
                    {
                        dblOutput = "MI";
                    }
                }

                if (exprCaution.ReturnType.Equals(XPathResultType.Number))
                {
                    if (common.myDbl(nav.Evaluate(queryCaution)) > 0)
                    {
                        dblOutput = "C";
                    }
                }
                else
                {
                    dblOutput = "A";
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    protected void ddlDrugSeverity_OnSelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            //if (!common.myInt(ddlDrugSeverity.SelectedIndex).Equals(0))
            //{
            ViewState["ddlDrugSeveritySelectedIndex"] = ddlDrugSeverity.SelectedIndex;
            ddlDrugSeverity.SelectedIndex = 0;
            bindData(false);
            ddlDrugSeverity.SelectedIndex = common.myInt(ViewState["ddlDrugSeveritySelectedIndex"]);
            bindData(false);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    //showIntreraction();

    //private void showIntreraction()
    //{
    //    try
    //    {
    //        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
    //        {
    //            Session["CIMSXMLInputData"] = string.Empty;
    //        }
    //        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(string.Empty);
    //        if (!strXML.Equals(string.Empty))
    //        {
    //            Session["CIMSXMLInputData"] = strXML;
    //            openWindowsCIMS(false);
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

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
    //            //            <Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" />
    //            //            <Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" />
    //            //        </Prescribing>
    //            //        <Allergies />
    //            //        <References/>
    //            //    </Interaction>
    //            //</Request>
    //            string strPrescribing = string.Empty;
    //            StringBuilder ItemIds = new StringBuilder();
    //            if (!strNewPrescribing.Equals(string.Empty))
    //            {
    //                ItemIds.Append(strNewPrescribing);
    //            }
    //            //foreach (GridViewRow dataItem in gvPrevious.Rows)
    //            //{
    //            //    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
    //            //    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
    //            //    if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
    //            //    //&& lnkBtnInteractionCIMS.Visible
    //            //    {
    //            //        string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
    //            //        CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
    //            //        string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
    //            //        if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
    //            //        {
    //            //            ItemIds.Append(strPres);
    //            //        }
    //            //    }
    //            //}
    //            foreach (GridDataItem dataItem in gvItem.Items)
    //            {
    //                HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
    //                LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
    //                if ((common.myLen(CIMSItemId.Value) > 0 && !common.myStr(CIMSItemId.Value).Trim().Equals("0")))
    //                //&& lnkBtnInteractionCIMS.Visible
    //                {
    //                    string CIMSType = common.myStr(((HiddenField)dataItem.FindControl("hdnCIMSType")).Value).Trim();
    //                    CIMSType = (common.myLen(CIMSType) > 0) ? CIMSType : "Product";
    //                    string strPres = "<" + CIMSType + " reference=\"" + common.myStr(CIMSItemId.Value) + "\" />";
    //                    if (!ItemIds.ToString().ToUpper().Contains(strPres.ToUpper()))
    //                    {
    //                        ItemIds.Append(strPres);
    //                    }
    //                }
    //            }
    //            if (ItemIds.ToString().Equals(string.Empty))
    //            {
    //                return string.Empty;
    //            }
    //            //strPrescribing = strPrescribing + "<Prescribed>" + ItemIds.ToString() + "</Prescribed>";
    //            strPrescribing = "<Prescribing>" + ItemIds.ToString() + "</Prescribing>";
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

    //void ExportToExcel(DataTable dt, string FileName)
    //{
    //    if (dt.Rows.Count > 0)
    //    {
    //        //FileName = FileName + ".csv";

    //        //Response.ContentType = "Application/x-msexcel";
    //        //Response.AddHeader("content-disposition", "attachment;filename=" + FileName + "");   
    //        //StringBuilder sbldr = new StringBuilder();
    //        //if (dt.Columns.Count != 0)
    //        //{
    //        //    foreach (DataColumn col in dt.Columns)
    //        //    {
    //        //        sbldr.Append(col.ColumnName + ',');
    //        //    }
    //        //    sbldr.Append("\r\n");
    //        //    foreach (DataRow row in dt.Rows)
    //        //    {
    //        //        foreach (DataColumn column in dt.Columns)
    //        //        {
    //        //            sbldr.Append(row[column].ToString().Replace(",", " ") + ',');
    //        //        }
    //        //        sbldr.Append("\r\n");
    //        //    }
    //        //}

    //        //Response.Write(sbldr.ToString());
    //        //Response.End();
    //    }
    //}
}
public static class DataColumnCollectionExtensions
{
    public static void RemoveColumns(this DataColumnCollection column, params string[] columns)
    {
        foreach (string c in columns)
        {
            column.Remove(c);
        }
    }
}