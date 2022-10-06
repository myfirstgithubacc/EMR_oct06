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
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using System.Collections.Generic;
using System.Drawing;

public partial class ICM_DrugAdministered : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds = new DataSet();
    clsCIMS objCIMS = new clsCIMS();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]) == "NO")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.ICM icm = new BaseC.ICM(sConString);
        DataTable dtMaxDuration = new DataTable();
        try
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("~/Login.aspx?Logout=1", false);
                    return;
                }
                ViewState["encounterid"] = Request.QueryString["EncId"] == null ? common.myStr(Session["EncounterId"]) : common.myStr(Request.QueryString["EncId"]);
                dtMaxDuration = icm.GetPatientDrugMaxDurationForEncounter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                 common.myInt(Session["RegistrationId"]), common.myInt(ViewState["encounterid"]), common.myInt(Session["FacilityId"]));
                if (dtMaxDuration.Rows.Count > 0)
                {
                    ViewState["StartDate"] = common.myDate(dtMaxDuration.Rows[0]["OrderDate"]);
                    ViewState["EndDate"] = common.myDate(dtMaxDuration.Rows[0]["EndDate"]);
                }
                lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                #region interface
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                DataSet dsInterface = objEMR.getFacilityInterfaceDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                        BaseC.clsEMR.enumCIMSorVIDALInterfaceFor.InterfaceForDrugAdministered);

                chkShowDetails.Visible = false;
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
                    else if (common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]))
                    {
                        Session["IsVIDALInterfaceActive"] = common.myBool(dsInterface.Tables[0].Rows[0]["IsVIDALInterfaceActive"]);

                        chkShowDetails.Visible = true;
                        chkShowDetails.Text = "Show VIDAL Details";
                    }


                }

                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    string CIMSDatabasePath = string.Empty;
                    if (dsInterface.Tables[0].Rows.Count > 0)
                    {
                        CIMSDatabasePath = common.myStr(dsInterface.Tables[0].Rows[0]["CIMSDatabasePath"]);
                    }
                    string CIMSDatabaseName = common.myStr(HttpContext.Current.Session["CIMSDatabaseName"]);

                    if (common.myLen(CIMSDatabaseName).Equals(0))
                    {
                        CIMSDatabaseName = "FastTrackData.mrc";
                    }

                    if (!File.Exists(CIMSDatabasePath + CIMSDatabaseName))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                        lblMessage.Text = "CIMS database not available !";
                        Alert.ShowAjaxMsg("CIMS database not available !", this);
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

                        //    Alert.ShowAjaxMsg(lblMessage.Text, this);
                        //}
                    }
                    catch
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "VIDAL web-services not running now !";

                        Alert.ShowAjaxMsg(lblMessage.Text, this);
                    }
                }

                #endregion


                ViewState["AllowMinutesForDoseTimeBeforeFrequencyDoseTime"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                           common.myInt(Session["FacilityId"]), "AllowMinutesForDoseTimeBeforeFrequencyDoseTime", sConString);

                ViewState["AllowMinutesForDoseTimeAfterFrequencyDoseTime"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                           common.myInt(Session["FacilityId"]), "AllowMinutesForDoseTimeAfterFrequencyDoseTime", sConString);



                // BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            }
            BindLegends();
            BindGrid();
            getLegnedColor();

            SetCIMS();//  setPatientInfo();
            setDiagnosis();
            setAllergiesWithInterfaceCode();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            icm = null;
            dtMaxDuration.Dispose();
        }
    }

    //void BindPatientHiddenDetails(string RegistrationNo)
    //{
    //    //try
    //    //{
    //    //    if (Session["PatientDetailString"] != null)
    //    //    {
    //    //        lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
    //    //    }

    //    //    if (common.myLen(lblPatientDetail.Text).Equals(0))
    //    //    {
    //    //        if (common.myInt(RegistrationNo) > 0)
    //    //        {
    //    //            lblPatientDetail.Text = common.setPatientDetails(0, common.myInt(RegistrationNo), string.Empty, sConString);
    //    //        }
    //    //    }
    //    //}
    //    //catch (Exception Ex)
    //    //{
    //    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //    //    lblMessage.Text = "Error: " + Ex.Message;
    //    //    objException.HandleException(Ex);
    //    //}
    //}
    private void BindLegends()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet dsStatus = new DataSet();
        try
        {
            dsStatus = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "DrugAdministrator", "");
            if (dsStatus != null)
            {
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    //dsStatus.Tables[0].DefaultView.RowFilter = "Code='DAS'";
                    //ViewState["DASColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];

                    //dsStatus.Tables[0].DefaultView.RowFilter = "Code='GT'";//Given Timely 
                    //ViewState["GTColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    //txtGivenTimely.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));
                    dsStatus.Tables[0].DefaultView.RowFilter = "Code='DAS'";//Due Dose
                    ViewState["DASColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    txtDueDose.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));


                    dsStatus.Tables[0].DefaultView.RowFilter = "Code='DANT'";//Delay
                    ViewState["DANTColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    txtDelay.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));

                    dsStatus.Tables[0].DefaultView.RowFilter = "Code='TS'"; // Next Due Dose
                    ViewState["TSColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    txtTS.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));

                    //dsStatus.Tables[0].DefaultView.RowFilter = "Code='NAD'";//Not Administered 
                    //ViewState["NXTColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    //txtNotAdministratedTime.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));

                    dsStatus.Tables[0].DefaultView.RowFilter = "Code='NAD'";//Not Administered 
                    ViewState["NADColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    txtNotAdministratedTime.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));


                    dsStatus.Tables[0].DefaultView.RowFilter = "Code='AD'";//Administered 
                    ViewState["ADMColorCode"] = dsStatus.Tables[0].DefaultView[0]["StatusColor"];
                    txtAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(dsStatus.Tables[0].DefaultView[0]["StatusColor"]));
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            objval = null;
            dsStatus.Dispose();
        }
    }
    private void BindGrid()
    {
        BaseC.ICM icm = new BaseC.ICM(sConString);

        try
        {
            string from = common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + " 00:00:00";
            string to = common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + " 23:00:00";
            lblMessage.Text = "";
            ds = icm.GetDrugAdministrationTimings(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), common.myInt(ViewState["encounterid"]), common.myInt(Session["RegistrationId"]),
                "04:00:00", from, to, common.myInt(Session["FacilityId"]), common.myStr(Session["OPIP"]));

            //ds = icm.GetDrugAdministrationTimings(Convert.ToInt16(Session["HospitalLocationId"]),
            //Convert.ToInt16(Session["FacilityId"]), common.myInt(ViewState["encounterid"]), common.myInt(Session["RegistrationId"]),
            //"04:00:00", from, to, Convert.ToInt16(Session["FacilityId"]), "ER");

            if (ds.Tables.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{                   
                gvDrugAdministered.DataSource = ds.Tables[0];
                gvDrugAdministered.DataBind();
                if (chkShowDetails.Checked)
                {
                    setVisiblilityInteraction();
                }
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
            ds.Dispose();
            icm = null;
        }
    }
    protected void gvDrugAdministered_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataView dvDrugAdministered = new DataView();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtAdministerdata = new DataTable();
        DataView dvAdministerFiler = new DataView();
        try
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = common.myStr(item.DataCell.Text.Replace("DrugTypeName:", ""));
                if (item.DataCell.Text.Contains("PRN") || item.DataCell.Text.Contains("STAT"))
                {
                    item.ForeColor = System.Drawing.Color.Red;
                }
            }
            if (e.Item is GridDataItem)
            {
                #region Start CIMS
                if (common.myBool(Session["IsCIMSInterfaceActive"]))
                {
                    //gvDrugAdministered.Columns[1].Visible = true;
                    //gvDrugAdministered.Columns[2].Visible = true;
                    //gvDrugAdministered.Columns[3].Visible = true;
                    //gvDrugAdministered.Columns[4].Visible = true;
                    //gvDrugAdministered.Columns[5].Visible = true;

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

                    if (common.myStr(hdnCIMSItemId.Value).Trim().Length == 0
                        || common.myStr(hdnCIMSItemId.Value).Trim() == "0")
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;
                        lnkBtnMonographCIMS.Visible = false;
                        lnkBtnInteractionCIMS.Visible = false;
                        lnkBtnDHInteractionCIMS.Visible = false;
                        lnkBtnDAInteractionCIMS.Visible = false;
                    }
                    else
                    {
                        lnkBtnBrandDetailsCIMS.Visible = false;

                        HiddenField hdnCIMSType = (HiddenField)e.Item.FindControl("hdnCIMSType");

                        string strXML = string.Empty;

                        if (common.myStr(hdnCIMSType.Value).ToUpper().Equals("PRODUCT"))
                        {
                            strXML = getBrandDetailsXMLCIMS(common.myStr(hdnCIMSType.Value), common.myStr(hdnCIMSItemId.Value));

                            if (strXML != string.Empty)
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
                else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                {
                    //gvPrevious.Columns[(byte)enumCurrent.BrandDetailsVIDAL].Visible = true;
                    //gvPrevious.Columns[(byte)enumCurrent.MonographVIDAL].Visible = true;
                    //gvPrevious.Columns[(byte)enumCurrent.InteractionVIDAL].Visible = true;
                    //gvPrevious.Columns[(byte)enumCurrent.DHInteractionVIDAL].Visible = true;
                    //gvPrevious.Columns[(byte)enumCurrent.DAInteractionVIDAL].Visible = true;

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

                #endregion End CIMS
                Button btnDrugAdministered;
                HiddenField hdnIndentId = (HiddenField)e.Item.FindControl("hdnIndentId");
                HiddenField hdnItemId = (HiddenField)e.Item.FindControl("hdnItemId");
                Label lblItemName = (Label)e.Item.FindControl("lblItemName");
                HiddenField hdnFrequency = (HiddenField)e.Item.FindControl("hdnFrequency");
                HiddenField hdnFrequencyId = (HiddenField)e.Item.FindControl("hdnFrequencyId");
                HiddenField hdnDrugType = (HiddenField)e.Item.FindControl("hdnDrugType");
                HiddenField hdnDrugTypeName = (HiddenField)e.Item.FindControl("hdnDrugTypeName");
                HiddenField hdnDoseName = (HiddenField)e.Item.FindControl("hdnDoseName");
                HiddenField hdnIsHighAlert = (HiddenField)e.Item.FindControl("hdnIsHighAlert");
                HiddenField hdnIsInfusion = (HiddenField)e.Item.FindControl("hdnIsInfusion");
                HiddenField hdnDoseTypeName = (HiddenField)e.Item.FindControl("hdnDoseTypeName");
                HiddenField hdnFrequencyDetailId = (HiddenField)e.Item.FindControl("hdnFrequencyDetailId");
                HiddenField hdnIsStop = (HiddenField)e.Item.FindControl("hdnIsStop");
                HiddenField hdnOrderDateTime = (HiddenField)e.Item.FindControl("hdnOrderDateTime");
                HiddenField hdnInstructions = (HiddenField)e.Item.FindControl("hdnInstructions");
                HiddenField hdnFoodRelationship = (HiddenField)e.Item.FindControl("hdnFoodRelationship");
                HiddenField hdnInfusionOrder = (HiddenField)e.Item.FindControl("hdnInfusionOrder");
                Label lblStartDate = (Label)e.Item.FindControl("lblStartDate");
                Label lblEndDate = (Label)e.Item.FindControl("lblEndDate");
                HiddenField hdnLinkMedicine = (HiddenField)e.Item.FindControl("hdnLinkMedicine");
                LinkButton lbtnMedicineLink = (LinkButton)e.Item.FindControl("lbtnMedicineLink");
                Label lblInstruction = (Label)e.Item.FindControl("lblInstruction");
                Label lblInfusionOrder = (Label)e.Item.FindControl("lblInfusionOrder");
                HiddenField hdnMedComDisTime = (HiddenField)e.Item.FindControl("hdnMedComDisTime");//MedCompletedDiscontinuedTime
                LinkButton lbtnFrequencyTime = (LinkButton)e.Item.FindControl("lbtnFrequencyTime");
                LinkButton lbtnShowVariableDose = (LinkButton)e.Item.FindControl("lbtnShowVariableDose");
                HiddenField hdnShowVariable = (HiddenField)e.Item.FindControl("hdnShowVariable");
                HiddenField hdnStopDate = (HiddenField)e.Item.FindControl("hdnStopDate");
                //if (hdnLinkMedicine.Value == "1")
                //{
                //    lbtnMedicineLink.Visible = true;
                //}
                //else
                //{
                //    lbtnMedicineLink.Visible = false;
                //}
                if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                {
                    e.Item.BackColor = System.Drawing.Color.LightGray;
                }
                dvDrugAdministered = new DataView(ds.Tables[0]);
                dvDrugAdministered.RowFilter = "IndentId=" + hdnIndentId.Value + " AND ItemId=" + hdnItemId.Value;

                if (dvDrugAdministered.ToTable().Rows.Count > 0)
                {
                    DataView dvInfusionOrder = dvDrugAdministered.ToTable().Copy().DefaultView;
                    try
                    {
                        dvInfusionOrder.RowFilter = "LinkMedicine = 1 AND ReferanceItemId > 0";

                        if (dvInfusionOrder.ToTable().Rows.Count > 0)
                        {
                            string ReferanceItemName = common.myStr(dvInfusionOrder.ToTable().Rows[0]["ReferanceItemName"]).Trim();
                            if (common.myLen(ReferanceItemName) > 0)
                            {
                                lblItemName.Text = lblItemName.Text + "</br>add to " + ReferanceItemName;
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        dvInfusionOrder.Dispose();
                    }
                }
                //string sPrescriptionDetail = emr.GetPrescriptionDetailStringNew(dvDrugAdministered.ToTable());
                //if (sPrescriptionDetail.StartsWith(lblItemName.Text))
                //{
                //    lblItemName.Text = sPrescriptionDetail;
                //}
                //else
                //{
                //    lblItemName.Text = lblItemName.Text + " - " + sPrescriptionDetail;
                //}

                if (common.myStr(hdnIsStop.Value) == "True")
                {
                    lblItemName.Text = lblItemName.Text + "</br>  Stop Date - " + common.myStr(hdnStopDate.Value);
                }
                //if (common.myInt(hdnIsInfusion.Value) == 1)
                //{
                if (hdnInstructions.Value.Length > 0)
                {
                    lblInstruction.Visible = true;
                    lblInstruction.Text = "<br/>Instruction - " + hdnInstructions.Value;
                }
                //if (hdnFoodRelationship.Value.Length > 0)
                //{
                //    lblInstruction.Visible = false;
                //    lblInstruction.Text = "Food Relationship - " + hdnFoodRelationship.Value;
                //}               

                if (common.myInt(hdnIsInfusion.Value) == 1)
                {
                    if (hdnInfusionOrder.Value.Length > 0)
                    {
                        lblInfusionOrder.Visible = true;
                        lblInfusionOrder.Text = "<br/>Infusion Order - " + hdnInfusionOrder.Value;
                    }
                }
                DataView dvDrugFrequency = new DataView(ds.Tables[1]);
                dvDrugFrequency.RowFilter = "FrequencyId=" + hdnFrequencyId.Value + " And IndentId=" + hdnIndentId.Value + " And ItemId=" + hdnItemId.Value;
                //  dvDrugFrequency.Sort = " FrequencyTime asc";
                int FrequencyValue = common.myInt(hdnFrequency.Value);

                int nextdose = 0;
                for (int i = 0; i < dvDrugFrequency.ToTable().Rows.Count; i++)
                {
                    #region CreateButton

                    btnDrugAdministered = new Button();
                    btnDrugAdministered.ID = "btn" + i;
                    btnDrugAdministered.Width = Unit.Pixel(30);
                    btnDrugAdministered.Height = Unit.Pixel(30);
                    btnDrugAdministered.BorderStyle = BorderStyle.Double;

                    btnDrugAdministered.BorderColor = System.Drawing.Color.White;
                    btnDrugAdministered.Attributes["width"] = "50%";
                    //btnDrugAdministered.Click(
                    btnDrugAdministered.Click += new EventHandler(btnDrugAdministered_Click);

                    btnDrugAdministered.Attributes.Add("indentid", hdnIndentId.Value);
                    btnDrugAdministered.Attributes.Add("itemid", hdnItemId.Value);
                    btnDrugAdministered.Attributes.Add("frequencyid", hdnFrequencyId.Value);
                    btnDrugAdministered.Attributes.Add("timeSlot", common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm:ss"));// "04:00:00");
                    btnDrugAdministered.Attributes.Add("Prescription", lblItemName.Text);//sPrescriptionDetail);
                    btnDrugAdministered.Attributes.Add("DrugType", hdnDrugType.Value);
                    btnDrugAdministered.Attributes.Add("DrugTypeName", hdnDrugTypeName.Value);
                    btnDrugAdministered.Attributes.Add("DoseName", hdnDoseName.Value);
                    btnDrugAdministered.Attributes.Add("IsHighAlert", hdnIsHighAlert.Value);
                    btnDrugAdministered.Attributes.Add("IsInfusion", hdnIsInfusion.Value);
                    btnDrugAdministered.Attributes.Add("OrderDateTime", hdnOrderDateTime.Value);
                    btnDrugAdministered.Attributes.Add("DoseTypeName", hdnDoseTypeName.Value);
                    // btnDrugAdministered.Attributes.Add("dateTime", dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"].ToString());//drug schedule date time
                    btnDrugAdministered.Attributes.Add("dateTime", common.myStr((common.myDate(lblDate.Text).ToString("MM/dd/yyyy")) + " " + Convert.ToDateTime(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm:ss tt")));//drug schudle date time
                    btnDrugAdministered.Attributes.Add("Frequency", hdnFrequency.Value);
                    btnDrugAdministered.Attributes.Add("FrequencyDetailId", common.myStr(dvDrugFrequency.ToTable().Rows[i]["FrequencyDetailId"]));
                    btnDrugAdministered.Attributes.Add("IsStop", hdnIsStop.Value);
                    btnDrugAdministered.Attributes.Add("Instructions", hdnInstructions.Value);

                    btnDrugAdministered.Attributes.Add("SD", lblStartDate.Text);//Start Date
                    btnDrugAdministered.Attributes.Add("ED", lblEndDate.Text);//End Date
                    //btnDrugAdministered.Attributes.Add("DoseEnable", common.myStr(dvDrugFrequency.ToTable().Rows[i]["DoseEnable"]));//Dose Enabled
                    if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DoseEnable"]) == "1")
                    {
                        btnDrugAdministered.Enabled = true;
                        #region for new Colour coding
                        if (common.myStr(hdnDrugType.Value).ToUpper().Equals("PRN") || common.myStr(hdnDrugType.Value).ToUpper().Equals("STAT"))
                        {
                            if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")//Means before save drug administered
                            {
                                if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                {
                                    btnDrugAdministered.BackColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    if (nextdose == 0)
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DASColorCode"]));//Due Dose
                                        nextdose = nextdose + 1;
                                    }
                                    else
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["TSColorCode"]));//Next Due Dose
                                    }
                                }

                            }
                            else //after save drug administered
                            {
                                if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                {
                                    btnDrugAdministered.BackColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "1")//Administered
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["ADMColorCode"]));
                                    }
                                    else if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["NADColorCode"]));
                                    }

                                }
                            }
                        }
                        else // Without PRN
                        {
                            DateTime FrequencyTime = Convert.ToDateTime(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]);
                            if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")//in case of blank time
                            {
                                //if (nextdose == 0)
                                //{
                                //    nextdose = nextdose + 1;
                                //    if (System.DateTime.Now.ToString("dd/MM/yyyy") == lblDate.Text
                                //        && common.myDate(FrequencyTime.ToString("hh:mm:ss tt")) < common.myDate(System.DateTime.Now)
                                //        && common.myDate(FrequencyTime.AddHours(1).ToString("hh:mm:ss tt")) < common.myDate(System.DateTime.Now))
                                //    { // Delay ViewState["DANTColorCode"]
                                //        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DANTColorCode"]));
                                //    }
                                //    else  // if (System.DateTime.Now.ToString("dd/MM/yyyy") == lblDate.Text && (common.myDate(FrequencyTime.ToString("hh:mm:ss tt")) >=
                                //    //    common.myDate(System.DateTime.Now) || common.myDate(FrequencyTime.ToString("hh:mm:ss tt")) <=
                                //    //    common.myDate(System.DateTime.Now)))
                                //    {
                                //        //Due Dose
                                //        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DASColorCode"]));
                                //    }

                                //}
                                //else//Next Due Dose
                                //{
                                //    btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["TSColorCode"]));
                                //}

                                if (nextdose == 0)
                                {
                                    nextdose = nextdose + 1;
                                }
                                if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                {
                                    btnDrugAdministered.BackColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    if (System.DateTime.Now.ToString("dd/MM/yyyy") == lblDate.Text
                                    && common.myDate(FrequencyTime.ToString("hh:mm:ss tt")) < common.myDate(System.DateTime.Now)
                                    && common.myDate(FrequencyTime.AddMinutes(common.myInt(ViewState["AllowMinutesForDoseTimeBeforeFrequencyDoseTime"])).ToString("hh:mm:ss tt"))
                                    < common.myDate(System.DateTime.Now))
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DANTColorCode"]));
                                    }
                                    else if (((System.DateTime.Now.ToString("dd/MM/yyyy") == lblDate.Text)
                                            && common.myDate(FrequencyTime.AddMinutes(-common.myInt(ViewState["AllowMinutesForDoseTimeAfterFrequencyDoseTime"])).ToString("hh:mm:ss tt"))
                                            > common.myDate(System.DateTime.Now)
                                            //common.myDate(FrequencyTime.ToString("hh:mm:ss tt")) > common.myDate(System.DateTime.Now))
                                            || common.myDate(System.DateTime.Now.Date) < common.myDate(lblDate.Text)
                                            ))
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["TSColorCode"]));
                                    }
                                    else
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DASColorCode"]));
                                    }
                                }
                            }
                            else
                            {

                                if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "1")//Administered
                                {
                                    if (hdnDrugType.Value.ToUpper() == "SINFU")
                                    {
                                        if (hdnMedComDisTime.Value == "")
                                        {
                                            btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DASColorCode"]));
                                        }
                                        else
                                        {
                                            btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["ADMColorCode"]));
                                        }

                                    }
                                    else
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["ADMColorCode"]));
                                    }

                                }
                                else if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                                {
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.White;
                                    }
                                    else
                                    {
                                        btnDrugAdministered.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["NADColorCode"]));
                                    }
                                }
                            }
                        }
                        #endregion
                        #region For Tooltip and  Time scheduler
                        //if (hdnDrugType.Value.ToUpper() == "STAT")
                        //{
                        //    lbtnFrequencyTime.Visible = false;
                        //    lbtnShowVariableDose.Visible = false;
                        //    if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")
                        //    {
                        //        //btnDrugAdministered.Text = "S - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt") + "\r " + " ";
                        //        btnDrugAdministered.Text = "S - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt") + " " + " ";
                        //        btnDrugAdministered.ToolTip = "Drug Ordering Time - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt");
                        //    }
                        //    else
                        //    {
                        //        if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                        //        {
                        //            btnDrugAdministered.Text = "S - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt") + " " + " ";
                        //            btnDrugAdministered.ToolTip = "Drug Ordering Time - " + common.myDate(hdnOrderDateTime.Value).ToString("hh:mm tt");
                        //        }
                        //        else
                        //        {
                        //            btnDrugAdministered.Text = "S - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt") + " " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                        //            btnDrugAdministered.ToolTip = "Drug Ordering Time - " + Convert.ToDateTime(hdnOrderDateTime.Value).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                        //        }
                        //    }
                        //}
                        //else 
                        if (common.myStr(hdnDrugType.Value).ToUpper().Equals("PRN") || common.myStr(hdnDrugType.Value).ToUpper().Equals("STAT"))//not set any schedule time for PRN
                        {
                            lbtnFrequencyTime.Visible = false;
                            lbtnShowVariableDose.Visible = false;
                            if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")
                            {
                                // btnDrugAdministered.Text = "S - N/A  \r " + " ";
                                btnDrugAdministered.Text = " ";
                                btnDrugAdministered.ToolTip = "Drug Schedule Time - N/A";
                            }
                            else
                            {
                                if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                                {
                                    //  btnDrugAdministered.Text = "S - N/A  \r " + " ";
                                    // btnDrugAdministered.Text = " ";
                                    // btnDrugAdministered.ToolTip = "Drug Schedule Time - N/A";

                                    btnDrugAdministered.Text = "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.ToolTip = "";
                                    }
                                    else
                                    {
                                        btnDrugAdministered.ToolTip = "Drug Schedule Time - N/A  " + " And Drug Not Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                    }
                                }
                                else
                                {
                                    // btnDrugAdministered.Text = "S - N/A  \r " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    btnDrugAdministered.Text = "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.ToolTip = "";
                                    }
                                    else
                                    {
                                        btnDrugAdministered.ToolTip = "Drug Schedule Time - N/A  " + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                    }
                                }
                            }
                        }
                        else if (hdnDrugType.Value.ToUpper() == "SINFU")
                        {
                            lbtnFrequencyTime.Visible = false;
                            lbtnShowVariableDose.Visible = false;
                            if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")
                            {
                                btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + " ";
                                btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt");
                            }
                            else
                            {
                                if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                                {
                                    btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + " ";
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.ToolTip = "";
                                    }
                                    else
                                    {
                                        btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt");
                                    }
                                }
                                else
                                {
                                    //btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + "\r " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    //btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    if (hdnMedComDisTime.Value == "")
                                    {
                                        if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["MedCompleteTime"]) != "00:00:00")
                                        {
                                            btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + "C - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedContinueTime"]).ToString("hh:mm tt") + " " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedCompleteTime"]).ToString("hh:mm tt");
                                            if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                            {
                                                btnDrugAdministered.ToolTip = "";
                                            }
                                            else
                                            {
                                                btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedContinueTime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                            }
                                        }
                                        else
                                        {
                                            btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + "C - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedContinueTime"]).ToString("hh:mm tt");
                                            if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                            {
                                                btnDrugAdministered.ToolTip = "";
                                            }
                                            else
                                            {
                                                btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt");
                                            }
                                        }
                                        //btnDrugAdministered.ForeColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        //btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + "\r " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                        //btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");

                                        btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + "C - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedContinueTime"]).ToString("hh:mm tt") + " " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedCompleteTime"]).ToString("hh:mm tt");
                                        if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                        {
                                            btnDrugAdministered.ToolTip = "";
                                        }
                                        else
                                        {
                                            btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["MedCompleteTime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lbtnFrequencyTime.Visible = true;
                            if (hdnShowVariable.Value == "1")
                            {
                                lbtnShowVariableDose.Visible = true;
                            }
                            else
                            {
                                lbtnShowVariableDose.Visible = false;
                            }
                            if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")
                            {
                                btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + " ";
                                if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                {
                                    btnDrugAdministered.ToolTip = "";
                                }
                                else
                                {
                                    btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                }
                            }
                            else
                            {
                                if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["IsMedicationAdminister"]) == "0")//False=Not IsMedicationAdminister
                                {
                                    btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + " ";
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.ToolTip = "";
                                    }
                                    else
                                    {
                                        btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                    }
                                }
                                else
                                {
                                    btnDrugAdministered.Text = "S - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " " + "A - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt");
                                    if (common.myStr(hdnDrugTypeName.Value) == "Discontinued")
                                    {
                                        btnDrugAdministered.ToolTip = "";
                                    }
                                    else
                                    {
                                        btnDrugAdministered.ToolTip = "Drug Schedule Time - " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["FrequencyTime"]).ToString("hh:mm tt") + " And Drug Administered Time " + common.myDate(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]).ToString("hh:mm tt") + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayReason"]) + "\n" + common.myStr(dvDrugFrequency.ToTable().Rows[i]["DelayRemarks"]);
                                    }
                                }
                            }
                            if (hdnDrugType.Value.ToUpper() == "STOP")
                            {
                                //lbtnShowVariableDose.Visible = false;
                                lbtnFrequencyTime.Visible = false;
                                //if (common.myStr(dvDrugFrequency.ToTable().Rows[i]["DrugAdministertime"]) == "00:00:00")
                                //{
                                //    btnDrugAdministered.Enabled = false;
                                //}
                                //else
                                //{
                                //    btnDrugAdministered.Enabled = true;
                                //}
                                btnDrugAdministered.Enabled = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        btnDrugAdministered.Text = " N/A " + " " + " "; ;
                        btnDrugAdministered.ToolTip = "Drug Not required for this slot ";
                        btnDrugAdministered.Enabled = false;
                    }

                    btnDrugAdministered.Font.Size = 7;
                    btnDrugAdministered.Width = Unit.Pixel(79);
                    btnDrugAdministered.Height = Unit.Pixel(40);

                    btnDrugAdministered.CssClass = "wrap";

                    e.Item.Cells[17].Controls.Add(btnDrugAdministered);
                    #endregion
                    if (e.Item is GridDataItem)
                    {
                        //GridDataItem dataBoundItem = e.Item as GridDataItem;
                        //if (!common.myBool(hdnItemIssuedStatus.Value))
                        //{
                        //    dataBoundItem.BackColor = Color.Gold; // Change the row Color where issue item not same as requested item
                        //}
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
            dtAdministerdata.Dispose();
            dvAdministerFiler.Dispose();
        }
    }
    protected void gvDrugAdministered_OnItemCommand(object Sender, GridCommandEventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {

            if (e.CommandName == "BrandDetailsCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                //  Radg row = (RadGrid)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)e.Item.FindControl("hdnCIMSType");

                showBrandDetails(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "MonographCIMS")
            {
                if (common.myLen(Session["CIMSXMLInputData"]) > 0)
                {
                    Session["CIMSXMLInputData"] = string.Empty;
                }

                // GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnCIMSType = (HiddenField)e.Item.FindControl("hdnCIMSType");

                showMonograph(common.myStr(e.CommandArgument).Trim(), common.myStr(hdnCIMSType.Value).Trim());
            }
            else if (e.CommandName == "InteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showIntreraction();
            }
            else if (e.CommandName == "DHInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("H");
            }
            else if (e.CommandName == "DAInteractionCIMS")
            {
                ViewState["NewPrescribing"] = string.Empty;
                showHealthOrAllergiesIntreraction("A");
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
            else if (e.CommandName == "DAInteractionVIDAL")
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
            emr = null;
        }
    }


    protected void btnDrugAdministered_Click(object sender, EventArgs e)
    {
        DataTable dtFrequency = new DataTable();
        DataView dvFrequency = new DataView();
        try
        {
            Button btn = (Button)sender;
            string indentid = btn.Attributes["indentid"].ToString();
            string itemid = btn.Attributes["itemid"].ToString();
            string frequencyid = btn.Attributes["frequencyid"].ToString();
            string dateTime = btn.Attributes["dateTime"].ToString();
            string PrescriptionDetail = btn.Attributes["Prescription"].ToString();
            string DrugType = btn.Attributes["DrugType"].ToString();
            string DoseTypeName = btn.Attributes["DrugTypeName"].ToString();
            string newDoseTypeName = DoseTypeName.Replace("&", "and");
            string DoseName = btn.Attributes["DoseName"].ToString();
            string IsHighAlert = btn.Attributes["IsHighAlert"].ToString();
            string IsInfusion = btn.Attributes["IsInfusion"].ToString();
            string OrderDateTime = btn.Attributes["OrderDateTime"].ToString();
            int iFrequency = common.myInt(btn.Attributes["Frequency"]);
            string FrequencyDetailId = btn.Attributes["FrequencyDetailId"].ToString();
            string IsStop = common.myStr(btn.Attributes["IsStop"]);
            string strInstructions = common.myStr(btn.Attributes["Instructions"]);

            string SD = btn.Attributes["SD"].ToString();//start date 
            string ED = btn.Attributes["ED"].ToString();// end date 
            if (DrugType.ToUpper() == "STAT")// For STAT -  schedule time always happend drug ordering time
            {
                dateTime = OrderDateTime;
            }
            //if (ddlInterval.SelectedValue == "02:00:00")
            //{
            //    if (common.myDate(dateTime).AddHours(2) <= common.myDate(OrderDateTime))
            //    {
            //        Alert.ShowAjaxMsg("Back date & time drug administer not allow", Page);
            //        return;
            //    }
            //}
            //else if (ddlInterval.SelectedValue == "01:00:00")
            //{
            //    if (common.myDate(dateTime).AddHours(1) <= common.myDate(OrderDateTime))
            //    {
            //        Alert.ShowAjaxMsg("Back date & time drug administer not allow", Page);
            //        return;
            //    }
            //}
            //else if (ddlInterval.SelectedValue == "00:30:00")
            //{
            //    if (common.myDate(dateTime).AddMinutes(30) <= common.myDate(OrderDateTime))
            //    {
            //        Alert.ShowAjaxMsg("Back date & time drug administer not allow", Page);
            //        return;
            //    }
            //}


            if (IsStop == "True")
            {
                Alert.ShowAjaxMsg("Stop this medicine !!", Page);
                return;
            }
            if (common.myDate(lblDate.Text) > common.myDate(System.DateTime.Now.ToString("dd/MM/yyyy")))
            {
                //Next day not allowed
                Alert.ShowAjaxMsg("Drug administrating allow only current date !!", Page);
                return;
            }
            if (common.myDate(lblDate.Text) < common.myDate(System.DateTime.Now.ToString("dd/MM/yyyy")))
            {//Previous day not allowed
                Alert.ShowAjaxMsg("Drug administrating allow only current date !!", Page);
                return;
            }
            if (common.myDate(lblDate.Text + " " + (Convert.ToDateTime(dateTime).ToString("hh:mm:ss tt"))) > System.DateTime.Now
               && common.myDate(lblDate.Text + " " + (Convert.ToDateTime(dateTime).ToString("hh:mm:ss tt"))) > System.DateTime.Now.AddMinutes(common.myInt(ViewState["AllowMinutesForDoseTimeBeforeFrequencyDoseTime"])))
            {
                //not allow before administered date and time with 30 minites
                Alert.ShowAjaxMsg("You are administrating before schedule.", Page);
                return;
            }
            //if (common.myDate(lblDate.Text + " " + (Convert.ToDateTime(dateTime).ToString("hh:mm:ss tt"))) < System.DateTime.Now
            //    && common.myDate(lblDate.Text + " " + (Convert.ToDateTime(dateTime).ToString("hh:mm:ss tt"))) < System.DateTime.Now.AddHours(-1))
            //{
            //    //not allow before administered date and time with one hour
            //    Alert.ShowAjaxMsg("You are administrating after schedule!!", Page);
            //}           
            //DateTime dt = common.myDate(dateTime);

            string ForPrnNew = "";
            if (DrugType == "PRN" && btn.Text.Trim() == "")
            {
                ForPrnNew = "Y";
            }
            string sEncounterId = ViewState["encounterid"] == null ? common.myStr(Session["EncounterId"]) : common.myStr(ViewState["encounterid"]);
            string sRegistrationId = Request.QueryString["RegNo"] == null ? common.myStr(Session["RegistrationNo"]) : common.myStr(Request.QueryString["RegNo"]);

            string[] sInterval = "04:00:00".Split(' ');// ddlInterval.SelectedItem.Text.Split(' ');
            string StrDelay = "";
            if (btn.BackColor.Name == "Red")
            {
                StrDelay = "Y";
            }

            Session["PrescriptionDetail"] = PrescriptionDetail;
            RadWindow1.NavigateUrl = "/ICM/DoseDetails.aspx?EncId=" + sEncounterId
                + "&RegNo=" + sRegistrationId + "&indentid=" + indentid + "&itemid=" + itemid + "&frequencyid=" + frequencyid
                + "&dateTime=" + dateTime + "&timeSlot=" + sInterval[0] + "&Presc=" + "&DT=" + DrugType
                + "&DoseName=" + DoseName + "&DTN=" + newDoseTypeName + "&IsHighAlert=" + IsHighAlert + "&IsInfusion=" + IsInfusion
                + "&FrequencyDetailId=" + FrequencyDetailId + "&Inst=" + strInstructions + "&PrnNew=" + ForPrnNew + "&SD=" + SD + "&ED=" + ED + "&Delay=" + StrDelay;

            RadWindow1.Height = 580;
            RadWindow1.Width = 900;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "wndAddService_OnClientClose";
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dtFrequency.Dispose();
            dvFrequency.Dispose();
        }
    }
    public void btnUpdate_OnClick(object sender, EventArgs e)
    {
        DataTable dtMaxDuration = new DataTable();
        BaseC.ICM icm = new BaseC.ICM(sConString);
        try
        {
            dtMaxDuration = icm.GetPatientDrugMaxDurationForEncounter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                     common.myInt(Session["RegistrationId"]), common.myInt(ViewState["encounterid"]), common.myInt(Session["FacilityId"]));

            if (dtMaxDuration.Rows.Count > 0)
            {
                ViewState["StartDate"] = common.myDate(dtMaxDuration.Rows[0]["OrderDate"]);
                ViewState["EndDate"] = common.myDate(dtMaxDuration.Rows[0]["EndDate"]);
            }

            if (common.myDate(ViewState["EndDate"]) < common.myDate(lblDate.Text).AddDays(1))
            {
                Alert.ShowAjaxMsg("There are no any prescription in next date", Page);
                return;
            }
            //ViewState["CurrentDate"] = common.myDate(lblDate.Text).AddDays(1);       
            lblDate.Text = common.myDate(lblDate.Text).AddDays(1).ToString("dd/MM/yyyy");
            BindGrid();
        }
        catch (Exception)
        {
        }
        finally
        {
            dtMaxDuration.Dispose();
            icm = null;
        }
    }
    public void btnPrevious_OnClick(object sender, EventArgs e)
    {
        if (common.myDate(ViewState["StartDate"]) > common.myDate(lblDate.Text).AddDays(-1))
        {
            Alert.ShowAjaxMsg("There are no any prescription in previous date ", Page);
            return;
        }
        //ViewState["CurrentDate"] = common.myDate(lblDate.Text).AddDays(-1);       
        lblDate.Text = common.myDate(lblDate.Text).AddDays(-1).ToString("dd/MM/yyyy");
        BindGrid();
    }
    public void btnBindGridWithXml_OnClick(object sender, EventArgs e)
    {
        BindGrid();  //due to comment for on page load this method all ready calling so not required it
    }
    public void btnAddDosageTime_OnClick(object sender, EventArgs e)
    {
        BindGrid();
    }

    public void lbtnMedicineLink_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridItem gvRow = (GridItem)lb.NamingContainer;
        HiddenField hdnIndentId = (HiddenField)gvRow.FindControl("hdnIndentId");
        HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
        RadWindow1.NavigateUrl = "/ICM/InfusionDetails.aspx?indentid=" + hdnIndentId.Value + "&itemid=" + hdnItemId.Value;
        RadWindow1.Height = 420;
        RadWindow1.Width = 400;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        // RadWindow1.OnClientClose = "wndAddService_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

    }

    public void lbtnFrequencyTime_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridItem gvRow = (GridItem)lb.NamingContainer;
        HiddenField hdnIndentId = (HiddenField)gvRow.FindControl("hdnIndentId");
        HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
        Label lblItemName = (Label)gvRow.FindControl("lblItemName");
        HiddenField hdnFrequencyId = (HiddenField)gvRow.FindControl("hdnFrequencyId");
        HiddenField hdnDuration = (HiddenField)gvRow.FindControl("hdnDuration");
        Label lblStartDate = (Label)gvRow.FindControl("lblStartDate");

        string rid = common.myInt(Request.QueryString["Regid"]) == 0 ? common.myStr(Session["RegistrationId"]) : common.myStr(Request.QueryString["Regid"]);
        string eid = common.myInt(Request.QueryString["EncId"]) == 0 ? common.myStr(Session["EncounterId"]) : common.myStr(Request.QueryString["EncId"]);
        Session["ItemName"] = common.myStr(lblItemName.Text).Replace(":", "-");

        RadWindow1.NavigateUrl = "~/EMR/Medication/DoseTime.aspx?Day=" + common.myInt(hdnDuration.Value) +
                                "&FrequencyId=" + common.myInt(hdnFrequencyId.Value) +
                                "&ItemId=" + common.myInt(hdnItemId.Value) +
                                "&IndentId=" + common.myInt(hdnIndentId.Value) +
                                "&RegId=" + rid +
                                "&EncId=" + eid +
                                "&From=Ward" +
                                "&InitialDate=" + common.myDate(lblStartDate.Text).ToString("yyyy/MM/dd") +
                                "&ItemName=";

        //RadWindow1.NavigateUrl = "~/EMR/Medication/DoseTime.aspx?Day=" + common.myInt(hdnDuration.Value) +
        //                     "&FrequencyId=" + common.myInt(hdnFrequencyId.Value) +
        //                     "&ItemId=" + common.myInt(hdnItemId.Value) +
        //                     "&IndentId=" + common.myInt(hdnIndentId.Value) +
        //                     "&RegId=" + common.myStr(rid) +
        //                     "&EncId=" + common.myStr(eid) +
        //                     "&From=Ward" +
        //                     "&InitialDate=" + common.myDate(lblStartDate.Text).ToString("yyyy/MM/dd") +
        //                     "&ItemName=" + common.myStr(lblItemName.Text).Replace(":", "-");

        RadWindow1.Height = 580;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "wndAdddosetime_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

    }

    public void lbtnShowVariableDose_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridItem gvRow = (GridItem)lb.NamingContainer;
        HiddenField hdnIndentId = (HiddenField)gvRow.FindControl("hdnIndentId");
        HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
        Label lblItemName = (Label)gvRow.FindControl("lblItemName");
        HiddenField hdnFrequencyId = (HiddenField)gvRow.FindControl("hdnFrequencyId");

        HiddenField hdnDuration = (HiddenField)gvRow.FindControl("hdnDuration");
        HiddenField hdnFrequency = (HiddenField)gvRow.FindControl("hdnFrequency");


        RadWindow1.NavigateUrl = "~/EMR/Medication/VariableDose.aspx?From=Ward&Day=" + hdnDuration.Value + "&FrequencyId="
            + hdnFrequencyId.Value + "&IName=" + lblItemName.Text + "&ItemId=" + hdnItemId.Value + "&IndentId=" + hdnIndentId.Value
           + "&EncId=" + common.myStr(Request.QueryString["EncId"]) + "&Frequency=" + hdnFrequency.Value;

        RadWindow1.Height = 500;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;

    }


    ///////////////CIMS/////////////////////////////
    private void SetCIMS()// setPatientInfo
    {

        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            int? weight = null;

            ds = objEMR.getScreeningParameters(common.myInt(ViewState["encounterid"]), common.myInt(Session["RegistrationId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                //lbl_Weight.Text = string.Empty;
                //txtHeight.Text = string.Empty;
                hdnWeight.Value = string.Empty;
                hdnHeight.Value = string.Empty;

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
                            hdnWeight.Value = common.myStr(ds.Tables[0].Rows[i][1]);
                        }
                        else if (common.myStr(ds.Tables[0].Rows[i][0]).Equals("HT"))// Height
                        {
                            hdnHeight.Value = common.myStr(ds.Tables[0].Rows[i][1]);
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
            foreach (GridItem dataItem in gvDrugAdministered.Items)
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

            foreach (GridItem dataItem in gvDrugAdministered.Items)
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

            //  lnkDrugAllergy.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));
        }
    }

    protected void chkShowDetails_OnCheckedChanged(object sender, EventArgs e)
    {
        if (common.myBool(Session["IsCIMSInterfaceActive"]))
        {
            gvDrugAdministered.Columns[1].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[2].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[3].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[4].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[5].Visible = chkShowDetails.Checked;

            gvDrugAdministered.Columns[6].Visible = false;
            gvDrugAdministered.Columns[7].Visible = false;
            gvDrugAdministered.Columns[8].Visible = false;
            gvDrugAdministered.Columns[9].Visible = false;
            gvDrugAdministered.Columns[10].Visible = false;

        }
        else if (common.myBool(Session["IsVIDALInterfaceActive"]))
        {

            gvDrugAdministered.Columns[1].Visible = false;
            gvDrugAdministered.Columns[2].Visible = false;
            gvDrugAdministered.Columns[3].Visible = false;
            gvDrugAdministered.Columns[4].Visible = false;
            gvDrugAdministered.Columns[5].Visible = false;

            gvDrugAdministered.Columns[6].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[7].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[8].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[9].Visible = chkShowDetails.Checked;
            gvDrugAdministered.Columns[10].Visible = chkShowDetails.Checked;
        }
        BindGrid();
        //setVisiblilityInteraction();
        //if (chkShowDetails.Checked == true)
        //{
        setGridColor();
        //}

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

    private void showIntreraction()
    {
        if (common.myLen(Session["CIMSXMLInputData"]) > 0)
        {
            Session["CIMSXMLInputData"] = string.Empty;
        }

        string strXML = (common.myStr(ViewState["NewPrescribing"]).Trim().Length > 0) ? common.myStr(ViewState["NewPrescribing"]).Trim() : getInterationXML(string.Empty);

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
    private int?[] getVIDALCommonNameGroupIds()
    {
        int?[] commonNameGroupIds = null;
        try
        {
            List<int?> list = new List<int?>();

            foreach (GridItem dataItem in gvDrugAdministered.Items)
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

    private void getDrugHealthInteractionVidal(int?[] commonNameGroupIds)
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            //int?[] commonNameGroupIds = new int?[] { 1524, 4025, 4212, 516, 28, 29, 30 };

            DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);//yyyy-mm-ddThh:mm:ss 
            //weight = common.myInt(lbl_Weight.Text);//In kilograms
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

                foreach (GridItem dataItem in gvDrugAdministered.Items)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

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

                foreach (GridItem dataItem in gvDrugAdministered.Items)
                {
                    HiddenField CIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                    LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");

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

            foreach (GridItem dataItem in gvDrugAdministered.Items)
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

            foreach (GridItem dataItem in gvDrugAdministered.Items)
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
    private void setVisiblilityInteraction()
    {
        clsVIDAL objVIDAL = new clsVIDAL(sConString);
        try
        {
            if (common.myBool(Session["IsCIMSInterfaceActive"]))
            {
                // lnkDrugAllergy.Visible = false;

                string strXMLDD = getInterationXML(string.Empty);//DrugToDrug
                string strXMLDH = getHealthOrAllergiesInterationXML("H", string.Empty);//Helth
                string strXMLDA = getHealthOrAllergiesInterationXML("A", string.Empty);//Allergies

                string outputValuesDD = string.Empty;
                string outputValuesDH = string.Empty;
                string outputValuesDA = string.Empty;

                if (common.myLen(strXMLDD) > 0 || common.myLen(strXMLDH) > 0 || common.myLen(strXMLDA) > 0)
                {
                    outputValuesDD = objCIMS.getFastTrack5Output(strXMLDD);
                    outputValuesDH = objCIMS.getFastTrack5Output(strXMLDH);
                    outputValuesDA = objCIMS.getFastTrack5Output(strXMLDA);

                    foreach (GridItem dataItem in gvDrugAdministered.Items)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        Label lblItemName = (Label)dataItem.FindControl("lblItemName");



                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            lnkBtnDHInteractionCIMS.Visible = false;
                            lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                            LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            lnkBtnDAInteractionCIMS.Visible = false;
                            lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                            if (outputValuesDD != null)
                            {
                                if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                                {
                                    if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDH != null)
                            {
                                string in1 = lblItemName.Text;
                                if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                                {
                                    if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDHInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDA != null)
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

                    foreach (GridItem dataItem in gvDrugAdministered.Items)
                    {
                        HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
                        HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
                        Label lblItemName = (Label)dataItem.FindControl("lblItemName");
                        if (common.myLen(hdnCIMSItemId.Value) > 2)
                        {
                            string strCIMSItemPatternMatch = "<" + ((common.myLen(hdnCIMSType.Value) > 0) ? common.myStr(hdnCIMSType.Value) : "Product") + " REFERENCE=\"" + common.myStr(hdnCIMSItemId.Value).Trim() + "\" NAME=";

                            LinkButton lnkBtnInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnInteractionCIMS");
                            lnkBtnInteractionCIMS.Visible = false;
                            lnkBtnInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugtoDrugInteractionColor"]));

                            LinkButton lnkBtnDHInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDHInteractionCIMS");
                            lnkBtnDHInteractionCIMS.Visible = false;
                            lnkBtnDHInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugHealthInteractionColor"]));

                            LinkButton lnkBtnDAInteractionCIMS = (LinkButton)dataItem.FindControl("lnkBtnDAInteractionCIMS");
                            lnkBtnDAInteractionCIMS.Visible = false;
                            lnkBtnDAInteractionCIMS.BackColor = System.Drawing.Color.FromName(common.myStr(ViewState["DrugAllergyColor"]));

                            if (outputValuesDD != null)
                            {
                                if (objCIMS.IsDrugToDrugInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDD, false) > 0)
                                {
                                    if (outputValuesDD.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDH != null)
                            {
                                string ina = lblItemName.Text;
                                if (objCIMS.IsDrugToHealthInteractionFound(common.myStr(hdnCIMSItemId.Value), outputValuesDH) > 0)
                                {
                                    if (outputValuesDH.ToUpper().Contains(strCIMSItemPatternMatch.ToUpper()))
                                    {
                                        lnkBtnDHInteractionCIMS.Visible = true;
                                    }
                                }
                            }

                            if (outputValuesDA != null)
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
                // lnkDrugAllergy.Visible = false;

                if (commonNameGroupIds.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable collVitalItemIdFound = new Hashtable();

                    sb = objVIDAL.getVIDALDrugToDrugInteraction(true, commonNameGroupIds, out collVitalItemIdFound);

                    DateTime dateOfBirth = Convert.ToDateTime(ViewState["PatientDOB"]);  //Convert.ToDateTime("1980-01-01 00:00:00"); //yyyy-mm-ddThh:mm:ss 
                    //int? weight = common.myInt(lbl_Weight.Text);//In kilograms
                    //'NONE','LESS_THAN_ONE_MONTH','MORE_THAN_ONE_MONTH','ALL'
                    int? weeksOfAmenorrhoea = null;//21 Number of weeks of amenorrhea (usually, pregnancy + 2 weeks)
                    int? creatinClearance = null;//The creatin clearance, expressed in ml/min. 120 is the normal value

                    Hashtable collVitalItemIdFoundDH = new Hashtable();

                    StringBuilder sbDHI = objVIDAL.getVIDALDrugHealthInteraction(commonNameGroupIds, dateOfBirth, common.myInt(ViewState["PatientWeight"]),
                            0, weeksOfAmenorrhoea, creatinClearance, common.myStr(ViewState["PatientGender"]),
                            (ViewState["PatientDiagnosisXML"] != string.Empty) ? (List<string>)ViewState["PatientDiagnosisXML"] : null,
                            out collVitalItemIdFoundDH);

                    foreach (GridItem dataItem in gvDrugAdministered.Items)
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

                    if (ViewState["PatientAllergyXML"] != string.Empty)
                    {
                        allergyIds = (int?[])ViewState["PatientAllergyXML"];
                    }

                    sb = objVIDAL.getVIDALDrugAllergyInteraction(commonNameGroupIds, allergyIds, moleculeIds);

                    //if (sb.ToString().Length > 0)
                    //{
                    //    lnkDrugAllergy.Visible = true;
                    //}
                    //else
                    //{
                    //    lnkDrugAllergy.Visible = false;
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
            objVIDAL = null;
        }
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
                    else if (common.myBool(Session["IsVIDALInterfaceActive"]))
                    {
                        List<string> list = new List<string>();

                        foreach (DataRow DR in ds.Tables[0].Rows)
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
    protected void btnHistory_OnClick(object sender, EventArgs e)
    {
        string sEncounterId = ViewState["encounterid"] == null ? common.myStr(Session["EncounterId"]) : common.myStr(ViewState["encounterid"]);

        RadWindow1.NavigateUrl = "/ICM/DrugAdministrationHistory.aspx?EncId=" + sEncounterId + "&RegId=" + common.myInt(Session["RegistrationId"]) + "&Master=No";

        RadWindow1.Height = 450;
        RadWindow1.Width = 700;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        // RadWindow1.OnClientClose = "wndAddService_OnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;

        RadWindow1.VisibleStatusbar = false;
    }
}