using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Configuration;
using BaseC;
using System.Web;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.IO;
using System.Net;



[Serializable]
public class clsLabTemplate
{
    public bool isResultEntered = false;
    public int ServiceId = 0;
    public int DiagSampleId = 0;
    public int RegistrationId = 0;
    public int ResultRemarksId = 0;
    public int ResultEntered = 0;
    public int ResultAlert = 0;
    public string StatusCode = "";
    public string ReviewRemark = string.Empty;
    public DataSet dsField = new DataSet();
    public DataSet dsOrganism = new DataSet();
    public int SampleTypeId = 0;
}

[Serializable]
public partial class LIS_Phlebotomy_InvestigationResult : System.Web.UI.Page
{
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //Boolean IsPageRefresh;

    #region Global values
    //Global Class objects
    BaseC.clsLISPhlebotomy objval;
    BaseC.clsMicrobiology objMicrobiology;
    clsExceptionLog objException = new clsExceptionLog();
    //Global list for storing Data Temporarily
    private List<clsLabTemplate> collLabTemplate;
    clsLabTemplate objLabTemplate;
    //Constant Values For Retriving the DataTable from list
    private const int FIELDS = 0;
    private const int OPTIONS = 1;
    private const int VALUES = 2;

    private int countControls = 1;


    //DataSet and Datatable user fro Temporariy transactions
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();

    //Globals Enum used in retereving Data from Global List
    private enum eServicesGrid
    {
        FieldID = 0,
        Image = 1,
        FieldName = 2,
        FieldType = 3,
        Values = 4,
        Remarks = 5,
        ServiceId = 6,
        ShowAfterResultFinalization = 7,
        ShowTextFormatInPopupPage = 8
    }
    private enum eRowGrid
    {
        FieldId = 0,
        RowId = 1,
        RowCaption = 2
    }
    private enum eFieldType : int
    {
        [StringValue("N")]
        Numeric = 1,
        [StringValue("T")]
        TextSingleLine = 2,
        [StringValue("M")]
        TextMultiLine = 3,
        [StringValue("W")]
        TextFormats = 4,
        [StringValue("D")]
        ListofChoices = 5,
        [StringValue("H")]
        Heading = 6,
        [StringValue("TA")]
        Tabular = 7,
        [StringValue("F")]
        Formula = 8,
        [StringValue("S")]
        Date = 9,
        [StringValue("C")]
        CheckBox = 10,
        [StringValue("B")]
        Boolean = 11,
        [StringValue("O")]
        Organism = 12,
        [StringValue("E")]
        Enzyme = 13,
        [StringValue("SN")]
        Sensitivity = 14,
        [StringValue("TM")]
        Time = 15,
        [StringValue("I")]
        Image = 16
    }


    #endregion

    //Changing the Master Page if it is open as Popup window
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["OT"]) == "OT")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    //Pageload event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }
            if (!IsPostBack)
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");

                ViewState["MDFlag"] = common.myStr(Request.QueryString["MD"]).Trim().ToUpper();

                Session["TextFormatDataResultEntry"] = "";
                Session["FieldIdResultEntry"] = "0";

                hdnIsCallFromLab.Value = "1";
                if (common.myStr(Request.QueryString["SEL_StatusCode"]) != "RF"
                && common.myStr(Session["ModuleName"]) != "LIS")
                {
                    hdnIsCallFromLab.Value = "0";
                }

                //Craeting a new list for temporary Data Hold
                collLabTemplate = new List<clsLabTemplate>();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }

                lblHeader.Text = "Inv.&nbsp;Result&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                ViewState["STATION_ID"] = Session["StationId"];

                //if (common.myStr(ViewState["MDFlag"]) == "RIS")
                //{
                //    lblLabNo.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
                //}
                //else
                //{
                //    lblLabNo.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
                //}

                if (string.Compare(common.myStr(ddlSearch.SelectedValue), "LN") == 0)
                {
                    int ind = common.myInt(ddlSearch.Items.FindItemIndexByValue("LN"));
                    ddlSearch.Items.Remove(ddlSearch.Items.FindItemIndexByValue("LN"));
                    RadComboBoxItem lPAno = new RadComboBoxItem();
                    if (common.myStr(Request.QueryString["MD"]) == "RIS")
                        lPAno.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
                    else
                        lPAno.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
                    lPAno.Value = "LN";
                    lPAno.Enabled = true;
                    lPAno.Selected = true;
                    ddlSearch.Items.Insert(ind, lPAno);
                    txtSearchCretria.Visible = false;
                    txtLabNo.Visible = true;
                }
                else
                {
                    txtSearchCretria.Visible = true;
                    txtLabNo.Visible = false;
                }
                ddlResultId.SelectedValue = "1";
                pnlMultipleResult.Visible = false;
                lblInfoResult.Visible = false;
                ddlResultId.Visible = false;
                lnkPackageDetail.Visible = true;
                lnkVisitHistory.Visible = true;
                lblInfoMultipleResultCount.Visible = false;
                lblInfoMultipleResultCount.Text = "";

                //If SEL_DiagSampleID is Not 0 then this page if open as Popup
                if (common.myStr(Request.QueryString["OT"]) != "OT")
                {
                    if (common.myLen(Request.QueryString["SEL_DiagSampleID"]) == 0)
                    {
                        if (common.myInt(Request.QueryString["StationId"]) == 0)
                        {
                            if (common.myInt(Session["StationId"]) == 0)
                            {
                                Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=RESULT&Module=" + common.myStr(ViewState["MDFlag"]), false);
                            }
                        }
                        else
                        {
                            ViewState["STATION_ID"] = common.myInt(Request.QueryString["StationId"]);
                        }
                    }
                }
                else
                {
                    BtnRelease.Visible = false;
                    lnkRelayDetails.Visible = false;
                    lnkPackageDetail.Visible = false;
                    lnkVisitHistory.Visible = false;
                    lnkPatientResultHistory.Visible = false;
                    btnNew.Visible = false;
                }

                if (common.myStr(Request.QueryString["Page"]).Trim() != string.Empty)
                {
                    //If Page is open from Patient History Page then StationId will be Passed As 0
                    if (common.myStr(Request.QueryString["Page"]) == "PH")
                    {
                        ViewState["STATION_ID"] = common.myInt(Request.QueryString["StationId"]);
                    }
                    //If Page is called from Result Finalization Page Then We can add result And Comment
                    if (common.myStr(Request.QueryString["Page"]) == "RF")
                    {
                        Session["xmlServices"] = "";
                        btnSave.Visible = true;
                        btnAddComments.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = false;
                        btnAddComments.Visible = false;
                        btnNew.Visible = false;
                    }
                }
                objval = new BaseC.clsLISPhlebotomy(sConString);
                //Binding Remarks DropDown
                bindRemarks();

                //If page is opened As popup window then the Source is paased byDefault is set to OPD 

                //if (common.myLen(Request.QueryString["SOURCE"]) > 0)
                //{
                //    rblSource.SelectedValue = common.myStr(Request.QueryString["SOURCE"]);
                //}
                txtLabNo.Text = "";
                //If page is opened As popup window then the LABNO is paased byDefault is nothing
                txtLabNo.Text = "";
                if (common.myLen(Request.QueryString["LABNO"]) > 0)
                {
                    txtLabNo.Text = common.myStr(Request.QueryString["LABNO"]);
                    txtLabNo.Enabled = false;
                    txtLabNo.Focus();
                }
                else
                {
                    txtLabNo.Focus();
                    Session["xmlServices"] = "";
                }
                if (common.myStr(Request.QueryString["Page"]) == "RF" && common.myStr(Request.QueryString["SEL_StatusCode"]) != "RF")
                {
                    btnResultFinalization.Visible = true;
                }
                //If page is opened As popup window then the following Control of master Page are Disabled
                if (common.myStr(Request.QueryString["MASTER"]) != "")
                {
                    RadPane mpLeftPannel = (RadPane)Master.FindControl("LeftPnl");
                    mpLeftPannel.Visible = false;
                    RadSplitBar mpSplitBar = (RadSplitBar)Master.FindControl("Radsplitbar1");
                    mpSplitBar.Visible = false;
                    RadPane mpTopPnl = (RadPane)Master.FindControl("TopPnl");
                    mpTopPnl.Visible = false;
                    RadPane mpEndPane = (RadPane)Master.FindControl("EndPane");
                    RadMenu RadMenu1 = (RadMenu)Master.FindControl("RadMenu1");
                    RadMenu1.Visible = false;
                    if (mpEndPane != null)
                    {
                        mpEndPane.Visible = false;
                    }
                    BtnRelease.Visible = false;
                    lnkRelayDetails.Visible = false;
                    rblSource.Enabled = false;
                    lnkVisitHistory.Visible = false;
                    txtLabNo.Enabled = false;
                    btnRefresh.Visible = false;
                    lnkPackageDetail.Visible = false;

                    if (common.myStr(Request.QueryString["FromMaster"]) == "WARD")
                    {
                        btnSave.Visible = false;
                        BtnRelease.Visible = false;
                        btnAddComments.Visible = false;
                        btnNew.Visible = false;
                        btnAddFootNote.Visible = false;
                        lnkPatientResultHistory.Visible = false;
                    }

                    btnResultFinalization.Visible = false;
                }

                //Setting the Default Value of ViewState
                ViewState["PRESERVICEID"] = 0;
                ViewState["PREDIAG_SAMPLEID"] = 0;
                ViewState["PRERegistrationId"] = 0;
                ViewState["PRERESULT_REMARKSID"] = 0;
                ViewState["DIAG_SAMPLEID"] = 0;
                ViewState["SERVICEID"] = 0;
                ViewState["RegistrationId"] = "0";
                ViewState["RESULT_REMARKSID"] = 0;
                ViewState["RESULT_REVIEWREMARKS"] = string.Empty;
                ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);

                if (common.myLen(Request.QueryString["EncounterId"]) > 0)
                {
                    ViewState["EncounterId"] = common.myStr(Request.QueryString["EncounterId"]);
                }

                getInformation();

                //If page is opened As popup window then the Retereving the Querystring Value

                #region set selected node

                int SEL_DiagSampleID = 0;
                if (common.myLen(Request.QueryString["SEL_DiagSampleID"]) > 0)
                {
                    SEL_DiagSampleID = common.myInt(Request.QueryString["SEL_DiagSampleID"]);
                }
                int SEL_ServiceId = 0;
                if (common.myLen(Request.QueryString["SEL_ServiceId"]) > 0)
                {
                    SEL_ServiceId = common.myInt(Request.QueryString["SEL_ServiceId"]);
                }
                int SEL_ResultRemarksId = 0;
                if (common.myLen(Request.QueryString["SEL_ResultRemarksId"]) > 0)
                {
                    SEL_ResultRemarksId = common.myInt(Request.QueryString["SEL_ResultRemarksId"]);
                }
                string SEL_StatusCode = "";
                if (common.myLen(Request.QueryString["SEL_StatusCode"]) > 0)
                {
                    SEL_StatusCode = common.myStr(Request.QueryString["SEL_StatusCode"]);
                }
                //Selecting the Service Whose Result Is Clicked
                if (SEL_DiagSampleID > 0 && SEL_ServiceId > 0)
                {
                    char[] slaceChar = new char[1];
                    slaceChar[0] = '/';
                    if (tvCategory.Nodes.Count > 1)
                    {
                        int treeIdx = 0;
                        foreach (TreeNode node1 in tvCategory.Nodes)
                        {
                            if (treeIdx > 0)
                            {
                                string[] splitPart = node1.Value.Split(slaceChar);
                                if (SEL_DiagSampleID == common.myInt(splitPart[1]))
                                {
                                    node1.Selected = true;
                                    tvCategory.Nodes.IndexOf(node1);

                                    ViewState["SelectedNode"] = SEL_ServiceId;
                                    ViewState["SERVICEID"] = SEL_ServiceId;
                                    ViewState["DIAG_SAMPLEID"] = SEL_DiagSampleID;
                                    ViewState["RESULT_REMARKSID"] = SEL_ResultRemarksId;
                                    ViewState["RESULT_REVIEWREMARKS"] = string.Empty;
                                    ViewState["StatusCode"] = SEL_StatusCode;
                                    tvCategory_SelectedNodeChanged(null, null);
                                    break;
                                }
                            }
                            treeIdx++;
                        }
                    }

                }
                #endregion

                if (tvCategory.Nodes.Count < 1)
                {
                    lblMessage.Text = "No Data Found.";
                }
                //Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
                ViewState["IsEnableUserAuthentication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "UserAuthenticationForLISAndPhlebotomy", sConString);
                if (ViewState["IsAskReviewRemark"] == null)
                {
                    ViewState["IsAskReviewRemark"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "IsAskReviewRemark", sConString);
                }

                ViewState["SampleTypeId"] = "0";
                ViewState["PreSampleTypeId"] = "0";
                ViewState["NewSampleTypeId"] = "0";
                ViewState["SaveFor"] = "0";
                //string sPrintProvisional = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToPrintProvisionalResult", sConString));
                //if (sPrintProvisional.Equals("N") && common.myStr(Request.QueryString["SEL_StatusCode"]) == "RP") { btnPrint.Visible = false; } else { btnPrint.Visible = true;}

                if (common.myStr(Request.QueryString["SEL_StatusCode"]) == "RP")
                {
                    btnPrint.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }

    private void bindddlSampleType(int serviceid)
    {
        BaseC.clsLISSampleMaster objLISSampleMaster = new BaseC.clsLISSampleMaster(sConString);
        DataSet dsSampleMaster = objLISSampleMaster.GetSampleTypeTagWithService(common.myInt(Session["HospitalLocationID"]), common.myInt(serviceid));

        ddlSampleType.Items.Clear();
        ddlSampleType.DataSource = dsSampleMaster.Tables[0];
        ddlSampleType.DataTextField = "Name";
        ddlSampleType.DataValueField = "SampleID";
        ddlSampleType.DataBind();
        ddlSampleType.Items.Insert(0, new ListItem("Select", "0"));

    }


    //Region for User Define method to fill and validate Data
    #region User Define method
    //Method To Check Whether Comments Exist or Not
    void CheckComments()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        ds = objval.getDigInvResultRemarks(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue), common.myInt(txtLabNo.Text), common.myInt(Session["StationId"]), 0, common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            btnAddComments.Text = "Edit Comments";
            btnAddComments.ForeColor = Color.Brown;
            btnAddFootNote.ForeColor = Color.Brown;
        }
        else
        {
            btnAddComments.Text = "Add Comments";
            btnAddComments.ForeColor = Color.Black;
            btnAddFootNote.ForeColor = Color.Black;
        }
    }
    //method to Bind the Header Section Patient Details Section
    private void getInformation()
    {
        try
        {
            tvCategory.Visible = false;
            gvSelectedServices.Visible = false;
            int labno = common.myInt(txtLabNo.Text);
            if (labno > 0)
            {
                BaseC.clsLISPhlebotomy objP = new BaseC.clsLISPhlebotomy(sConString);
                DataSet ds = objP.GetheaderResultEntry(common.myStr(rblSource.SelectedValue),
                    common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), labno, common.myInt(ViewState["STATION_ID"]));

                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            tvCategory.Visible = true;
                            ViewState["RegistrationId"] = common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]);
                            ViewState["EncounterId"] = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                            ViewState["EncId"] = common.myStr(ds.Tables[0].Rows[0]["EncounterId"]);
                            lblPatientName.Text = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
                            lblAgeGender.Text = common.myStr(ds.Tables[0].Rows[0]["AgeGender"]);
                            lblRegNo.Text = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                            lblFacilityName.Text = common.myStr(ds.Tables[0].Rows[0]["FacilityName"]);
                            ////manojchauhan
                            //ddlMultiStageResultFinalized.SelectedIndex= ddlMultiStageResultFinalized.Items.IndexOf(ddlMultiStageResultFinalized.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["IsMultiStageResultFinalized"])));

                            //lblManualLabNo.Text = common.myStr(ds.Tables[0].Rows[0]["ManualLabNo"]);

                            //Checking Whether it is Opd or Ipd Case
                            if ((common.myStr(ds.Tables[0].Rows[0]["WardName"]) != "") && (common.myStr(ds.Tables[0].Rows[0]["BedNo"]) != ""))
                            {
                                lblWardBedNo.Text = common.myStr(ds.Tables[0].Rows[0]["WardName"]) + "/" + common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
                            }
                            lblOrderDate.Text = common.myStr(ds.Tables[0].Rows[0]["OrderDate"]);
                            ViewState["OrderDate"] = common.myStr(ds.Tables[0].Rows[0]["EncodedDate"]);
                            ViewState["Gender"] = common.myStr(ds.Tables[0].Rows[0]["Gender"]);
                            ViewState["AgeInDays"] = common.myStr(ds.Tables[0].Rows[0]["AgeInDays"]);

                            ViewState["SelectedSource"] = common.myStr(ds.Tables[0].Rows[0]["Source"]);

                            //Method to fill the Service Details
                            BindCategoryTree();
                            //Method to check Commets Exist or Not
                            CheckComments();
                            if (tvCategory.Nodes.Count > 0)
                            {
                                //If Service Exist treeview SelectedNodeChanged event is Called to Make the First Service Template
                                gvSelectedServices.Visible = true;
                                tvCategory_SelectedNodeChanged(null, null);
                                tvCategory.ExpandAll();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Method To Bind The Remarks
    private void bindRemarks()
    {
        try
        {
            BaseC.InvestigationFormat format = new BaseC.InvestigationFormat(sConString);
            int iRemarksId = 0; //for all Remarks formats
            DataSet objDs = (DataSet)format.getServiceRemarksTemplate(common.myInt(Session["HospitalLocationId"]), iRemarksId, common.myInt(Session["StationId"]));
            DataView objDv = objDs.Tables[0].DefaultView;
            objDv.RowFilter = "Active = 1";
            objDv.Sort = "RemarksName Asc";
            if (objDv.Count > 0)
            {
                ddlRemarks.DataSource = objDv;
                ddlRemarks.DataValueField = "RemarksId";
                ddlRemarks.DataTextField = "RemarksName";
                ddlRemarks.DataBind();
                ddlRemarks.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlRemarks.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }
    //Method to Populate the Template result Grid

    protected void menuTemplate_OnItemDataBound(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        DataTable dtHorizantalMenu = new DataTable();
        DataView dvHorizantalMenu = new DataView();
        try
        {
            if (ViewState["HorizantalMenuData"] != null)
            {
                dtHorizantalMenu = (DataTable)ViewState["HorizantalMenuData"];
                dvHorizantalMenu = new DataView(dtHorizantalMenu);
                dvHorizantalMenu.RowFilter = "FieldId=" + e.Item.Value + " AND ISNULL(FieldValue,'')<>'' AND ISNULL(TextValue,'')<>''";
                if (dvHorizantalMenu.ToTable().Rows.Count > 0)
                {
                    e.Item.ForeColor = System.Drawing.Color.Fuchsia;
                    //e.Item.BackColor= System.Drawing.Color.Fuchsia;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
        finally
        {
            dtHorizantalMenu.Dispose();
            dvHorizantalMenu.Dispose();
        }
    }
    protected void BindTemplateGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            objval = new BaseC.clsLISPhlebotomy(sConString);
            //Checking Whether the Global List collLabTemplate Is Stored in ViewState or Not
            if (ViewState["collLabTemplate"] == null)
            {
                ViewState["PRESERVICEID"] = common.myInt(ViewState["SERVICEID"]);
                ViewState["PREDIAG_SAMPLEID"] = common.myInt(ViewState["DIAG_SAMPLEID"]);
                ViewState["PRERegistrationId"] = common.myInt(ViewState["RegistrationId"]);
                ViewState["PRERESULT_REMARKS_ID"] = common.myInt(ViewState["RESULT_REMARKSID"]);
                ViewState["SampleTypeId"] = common.myInt(ddlSampleType.SelectedValue);
                ViewState["PreSampleTypeId"] = common.myInt(ViewState["SampleTypeId"]);

                /***************** get data for fields *****************/
                objDs = objval.getLabNoInvFormats(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue), common.myInt(Session["FacilityID"]),
                        common.myInt(ViewState["DIAG_SAMPLEID"]), common.myInt(ViewState["SERVICEID"]), common.myStr(ViewState["Gender"]), common.myInt(ViewState["AgeInDays"]), common.myStr(ViewState["StatusCode"]), common.myInt(Session["HospitalLocationId"]), common.myInt(hdnIsCallFromLab.Value), common.myInt(ddlSampleType.SelectedValue));
                //Putting the Data in Global List
                collLabTemplate = new List<clsLabTemplate>();
                objLabTemplate = new clsLabTemplate();
                objLabTemplate.ServiceId = common.myInt(ViewState["SERVICEID"]);
                objLabTemplate.DiagSampleId = common.myInt(ViewState["DIAG_SAMPLEID"]);
                objLabTemplate.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                objLabTemplate.ResultRemarksId = common.myInt(ViewState["RESULT_REMARKSID"]);
                objLabTemplate.ReviewRemark = common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
                objLabTemplate.ResultAlert = common.myInt(ViewState["ResultAlert"]);
                objLabTemplate.StatusCode = common.myStr(ViewState["StatusCode"]);
                objLabTemplate.dsField = new DataSet();
                objLabTemplate.dsField = objDs;
                objLabTemplate.SampleTypeId = common.myInt(ViewState["SampleTypeId"]);

                //Done by ujjwal 01 july 2015 introduced one new button for addendum report start
                hdnUseAddendum.Value = "0";
                foreach (DataRow dr in objDs.Tables[0].Rows)
                {
                    if (common.myBool(dr["IsAddendum"]))
                    {
                        hdnUseAddendum.Value = common.myStr(dr["IsAddendum"]);
                        break;
                    }
                }

                if (common.myStr(ViewState["StatusCode"]) == "RF")
                {

                    if (!getAddendum().ToLower().Equals("addendumreleased") && common.myBool(hdnUseAddendum.Value) && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                    {
                        lnkAddendumRelease.Visible = true;
                        hdnIsAddendumReleased.Value = "0";
                    }
                    else
                    {
                        lnkAddendumRelease.Visible = false;
                        hdnIsAddendumReleased.Value = "1";
                    }
                }
                else
                {
                    hdnIsAddendumReleased.Value = "0";
                }
                //Done by ujjwal 01 july 2015 introduced one new button for addendum report end
                ddlResultId.Items.Clear();
                //If Datatable 2 has Rows means result Already Entered
                if (objDs.Tables[2].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt = objDs.Tables[2];
                    //Populating the Result Dropdown
                    int maxResultId = Convert.ToInt32(dt.Compute("max(ResultId)", string.Empty));
                    objLabTemplate.ResultEntered = maxResultId;

                    for (int i = 1; i <= maxResultId + 1; i++)
                    {
                        RadComboBoxItem Item = new RadComboBoxItem(common.myStr(i), common.myStr(i));
                        ddlResultId.Items.Add(Item);
                    }
                }
                else
                {
                    //Populating the Result Dropdown
                    RadComboBoxItem Item = new RadComboBoxItem(common.myStr(1), common.myStr(1));
                    ddlResultId.Items.Add(Item);
                }
                ddlResultId.SelectedValue = "1";
                //Adding the List in global List
                collLabTemplate.Add(objLabTemplate);
                //Selecting the Remarks
                ddlRemarks.SelectedIndex = ddlRemarks.Items.IndexOf(ddlRemarks.Items.FindItemByValue(common.myStr(ViewState["RESULT_REMARKSID"])));//rafat               
                //Storing the Global List in ViewState For futher reference
                ViewState["collLabTemplate"] = collLabTemplate;
                if (!string.IsNullOrEmpty(common.myStr(ViewState["RESULT_REVIEWREMARKS"])))
                    lblOldReviewRemark.Text = "<b>Review Remark: </b>" + common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
                else
                    lblOldReviewRemark.Text = string.Empty;
            }
            else
            {
                string ReviewRemark = string.Empty;
                if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
                    ReviewRemark = txtReviewRemark.Text;

                //ViewState["SampleTypeId"] = common.myInt(ddlSampleType.SelectedValue);
                //Method to add the Current Result Data into the List
                holdPreviousData(common.myInt(ViewState["PRESERVICEID"]), common.myInt(ViewState["PREDIAG_SAMPLEID"]), common.myInt(ViewState["PRERegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));
                bool allowFind = true;

                foreach (clsLabTemplate LT in collLabTemplate)
                {
                    if (LT.ServiceId == common.myInt(ViewState["SERVICEID"])
                        && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]) && common.myInt(ddlSampleType.SelectedValue) == common.myInt(ViewState["PreSampleTypeId"]))
                    {
                        allowFind = false;

                        objDs = new DataSet();
                        objDs = LT.dsField;

                        ViewState["RESULT_REMARKSID"] = common.myInt(LT.ResultRemarksId);
                        ViewState["RESULT_REVIEWREMARKS"] = common.myStr(LT.ReviewRemark);
                        ddlRemarks.SelectedIndex = ddlRemarks.Items.IndexOf(ddlRemarks.Items.FindItemByValue(common.myStr(ViewState["RESULT_REMARKSID"])));//rafat               
                        if (!string.IsNullOrEmpty(common.myStr(ViewState["RESULT_REVIEWREMARKS"])))
                            lblOldReviewRemark.Text = "<b>Review Remark: </b> " + common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
                        else
                            lblOldReviewRemark.Text = string.Empty;
                        break;
                    }
                }
                if (allowFind)
                {

                    /***************** get data for fields *****************/
                    objDs = objval.getLabNoInvFormats(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue), common.myInt(Session["FacilityID"]),
                                    common.myInt(ViewState["DIAG_SAMPLEID"]), common.myInt(ViewState["SERVICEID"]),
                                    common.myStr(ViewState["Gender"]), common.myInt(ViewState["AgeInDays"]), common.myStr(ViewState["StatusCode"]), common.myInt(Session["HospitalLocationId"]), common.myInt(hdnIsCallFromLab.Value), common.myInt(ViewState["SampleTypeId"]));
                    objLabTemplate = new clsLabTemplate();
                    objLabTemplate.ServiceId = common.myInt(ViewState["SERVICEID"]);
                    objLabTemplate.DiagSampleId = common.myInt(ViewState["DIAG_SAMPLEID"]);
                    objLabTemplate.RegistrationId = common.myInt(ViewState["RegistrationId"]);
                    objLabTemplate.ResultRemarksId = common.myInt(ViewState["RESULT_REMARKSID"]);
                    objLabTemplate.ReviewRemark = common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
                    objLabTemplate.ResultAlert = common.myInt(ViewState["ResultAlert"]);
                    objLabTemplate.StatusCode = common.myStr(ViewState["StatusCode"]);
                    objLabTemplate.dsField = new DataSet();
                    objLabTemplate.dsField = objDs;
                    objLabTemplate.SampleTypeId = common.myInt(ViewState["SampleTypeId"]);
                    objLabTemplate.SampleTypeId = common.myInt(ViewState["IsMultiStageResultFinalized"]);
                    ddlResultId.Items.Clear();
                    if (objDs.Tables[2].Rows.Count > 0)
                    {
                        dt = new DataTable();
                        dt = objDs.Tables[2];
                        int maxResultId = Convert.ToInt32(dt.Compute("max(ResultId)", string.Empty));
                        objLabTemplate.ResultEntered = maxResultId;
                        for (int i = 1; i <= maxResultId + 1; i++)
                        {
                            RadComboBoxItem Item = new RadComboBoxItem(common.myStr(i), common.myStr(i));
                            ddlResultId.Items.Add(Item);
                        }

                    }
                    else
                    {
                        RadComboBoxItem Item = new RadComboBoxItem(common.myStr(1), common.myStr(1));
                        ddlResultId.Items.Add(Item);

                    }
                    //Done by ujjwal 01 july 2015 introduced one new button for addendum report start
                    hdnUseAddendum.Value = "0";
                    foreach (DataRow dr in objDs.Tables[0].Rows)
                    {
                        if (common.myBool(dr["IsAddendum"]))
                        {
                            hdnUseAddendum.Value = common.myStr(dr["IsAddendum"]);
                            break;
                        }
                    }

                    if (common.myStr(ViewState["StatusCode"]) == "RF")
                    {

                        if (!getAddendum().ToLower().Equals("addendumreleased") && common.myBool(hdnUseAddendum.Value) && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                        {
                            lnkAddendumRelease.Visible = true;
                            hdnIsAddendumReleased.Value = "0";
                        }
                        else
                        {
                            lnkAddendumRelease.Visible = false;
                            hdnIsAddendumReleased.Value = "1";
                        }
                    }
                    else
                    {
                        hdnIsAddendumReleased.Value = "0";
                    }
                    //Done by ujjwal 01 july 2015 introduced one new button for addendum report end
                    ddlResultId.SelectedValue = "1";
                    //Adding the List in global List
                    collLabTemplate.Add(objLabTemplate);
                    //Selecting the Remarks
                    ddlRemarks.SelectedIndex = ddlRemarks.Items.IndexOf(ddlRemarks.Items.FindItemByValue(common.myStr(ViewState["RESULT_REMARKSID"])));//rafat               
                    //Storing the Global List in ViewState For futher reference
                    ViewState["collLabTemplate"] = collLabTemplate;
                    if (!string.IsNullOrEmpty(common.myStr(ViewState["RESULT_REVIEWREMARKS"])))
                        lblOldReviewRemark.Text = "<b>Review Remark: </b>" + common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
                    else
                        lblOldReviewRemark.Text = string.Empty;
                }
                ViewState["PRESERVICEID"] = common.myInt(ViewState["SERVICEID"]);
                ViewState["PREDIAG_SAMPLEID"] = common.myInt(ViewState["DIAG_SAMPLEID"]);
                ViewState["PRERegistrationId"] = common.myInt(ViewState["RegistrationId"]);
                ViewState["PreSampleTypeId"] = common.myInt(ViewState["SampleTypeId"]);
            }
            if (objDs.Tables[0].Rows.Count > 0)
            {
                //Done by ujjwal 01 july 2015 introduced one new button for addendum report start
                hdnUseAddendum.Value = "0";
                foreach (DataRow dr in objDs.Tables[0].Rows)
                {
                    if (common.myBool(dr["IsAddendum"]))
                    {
                        hdnUseAddendum.Value = common.myStr(dr["IsAddendum"]);
                        break;
                    }
                }

                if (common.myStr(ViewState["StatusCode"]) == "RF")
                {

                    if (!getAddendum().ToLower().Equals("addendumreleased") && common.myBool(hdnUseAddendum.Value) && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                    {
                        lnkAddendumRelease.Visible = true;
                        hdnIsAddendumReleased.Value = "0";
                    }
                    else
                    {
                        lnkAddendumRelease.Visible = false;
                        hdnIsAddendumReleased.Value = "1";
                    }
                }
                else
                {
                    hdnIsAddendumReleased.Value = "0";
                }
                //Done by ujjwal 01 july 2015 introduced one new button for addendum report end


                //Cheking Whether Result Template To show in Vertical or Horizontal Format 
                if (common.myInt(ViewState["HorizontalView"]) == 1)
                {
                    ViewState["HorizantalMenuData"] = objDs.Tables[2];
                    DataView dv = objDs.Tables[0].DefaultView;
                    //Excluding the Header Column in Horizontal View
                    dv.Sort = "SequenceNo Asc";
                    dv.RowFilter = "FieldType <> 'H'";
                    menuTemplate.DataSource = dv;
                    menuTemplate.DataTextField = "FieldName";
                    menuTemplate.DataValueField = "FieldId";
                    menuTemplate.DataBind();
                    menuTemplate.Visible = true;
                    gvSelectedServices.DataSource = GetHorizontalFormatData(common.myInt(dv.Table.Rows[0]["FieldId"]));
                    gvSelectedServices.DataBind();
                    txtLabNo.Enabled = false;
                    rblSource.Enabled = false;
                }
                else
                {
                    DataView dv = objDs.Tables[0].DefaultView;
                    dv.Sort = "SequenceNo Asc";
                    gvSelectedServices.DataSource = dv; //  s
                    gvSelectedServices.DataBind();
                    txtLabNo.Enabled = false;
                    rblSource.Enabled = false;

                    menuTemplate.DataSource = null;
                    menuTemplate.DataBind();
                    menuTemplate.Visible = false;
                }
            }
            else
            {
                gvSelectedServices.DataSource = null;
                gvSelectedServices.DataBind();
                pnlMultipleResult.Visible = false;
                lblInfoResult.Visible = false;
                ddlResultId.Visible = false;
                lblInfoMultipleResultCount.Visible = false;
                txtLabNo.Enabled = true;
                rblSource.Enabled = true;
            }
            //Checking Whether multiple Rsult Entry Allowed Or not
            if (common.myStr(ViewState["MultipleResultEntry"]) != "")
            {
                if (common.myBool(ViewState["MultipleResultEntry"]))
                {
                    foreach (clsLabTemplate LT in collLabTemplate)
                    {
                        if (LT.ServiceId == common.myInt(ViewState["SERVICEID"])
                            && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                        {
                            if (LT.ResultEntered != 0)
                            {
                                pnlMultipleResult.Visible = true;
                                lblInfoResult.Visible = true;
                                ddlResultId.Visible = true;
                                lblInfoMultipleResultCount.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                lblInfoMultipleResultCount.Visible = true;
                                lblInfoMultipleResultCount.Text = "(Total Result(s) Entered: " + common.myStr(LT.ResultEntered) + ")";
                                //manojchauhan
                                break;
                            }
                            else
                            {
                                pnlMultipleResult.Visible = false;
                                lblInfoResult.Visible = false;
                                ddlResultId.Visible = false;
                                lblInfoMultipleResultCount.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                lblInfoMultipleResultCount.Visible = false;
                                break;
                            }
                        }
                    }
                }

                else
                {
                    ddlResultId.SelectedValue = "1";
                    pnlMultipleResult.Visible = false;
                    lblInfoResult.Visible = false;
                    ddlResultId.Visible = false;
                    lblInfoMultipleResultCount.Visible = false;
                    lblInfoMultipleResultCount.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Method to Bind the Treeview Nodes Showed in Left Panel Of screen
    private void BindCategoryTree()
    {
        DataView dvTree = new DataView();
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {
            tvCategory.Nodes.Clear();
            objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.GetLabNoInvestigations(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue),
                        common.myInt(ViewState["STATION_ID"]), common.myInt(txtLabNo.Text),
                        common.myInt(Session["HospitalLocationId"]), common.myStr(Session["xmlServices"]),
                        common.myInt(Session["FacilityId"]), common.myInt(hdnIsCallFromLab.Value), common.myInt(Session["EmployeeId"]));

            dv = ds.Tables[0].DefaultView;

            if (common.myStr(Request.QueryString["Page"]) == "RF")
            {
                dv.RowFilter = "DiagSampleId=" + common.myInt(Request.QueryString["SEL_DiagSampleID"]);
            }
            else
            {
                dv.RowFilter = "";
            }
            dv.Sort = "DiagSampleId Asc";
            if (dv.Table.Rows.Count > 0)
            {
                for (int rowIdx = 0; rowIdx < dv.Count; rowIdx++)
                {
                    DataRow dr = dv[rowIdx].Row;
                    AddNodes(tvCategory, common.myStr(dr["ServiceId"]) + "/" + common.myStr(dr["DiagSampleId"]) + "/" + common.myInt(dr["ResultRemarksId"]) + "/" + common.myStr(dr["StatusCode"]) + "/" + common.myStr(dr["MultipleResultEntry"]) + "/" + common.myStr(dr["HorizontalView"]) + "/" + common.myStr(dr["ResultAlert"]) + "/" + common.myStr(dr["centerName"]) + "/" + common.myBool(dr["ResultHTML"]) + "/" + common.myStr(dr["ManualLabNo"]).Trim().Replace("/", "^^") + "/" + common.myBool(dr["IsClinicalTemplateRequired"]) + "/" + common.myInt(dr["ServiceDetailId"]) + "/" + common.myBool(dr["PrintReferenceRangeInHTML"]) + "/" + common.myStr(dr["ReviewRemark"]) + "/" + common.myStr(dr["SampleAckDate"]).Trim().Replace("/", "^^") + "/" + common.myStr(dr["ScanInTime"]).Trim().Replace("/", "^^") + "/" + common.myStr(dr["ScanOutTime"]).Trim().Replace("/", "^^") + "/" + common.myBool(dr["IsScanTimeMandatoryForResult"]) + "/" + common.myBool(dr["IsMultiStageResultFinalized"]), common.myStr(dr["ServiceName"]));
                }
                if (tvCategory.Nodes.Count > 0)
                {
                    tvCategory.Nodes[0].Selected = true;
                    tvCategory.CollapseAll();
                    tvCategory.PopulateNodesFromClient = true;
                    tvCategory.ShowLines = true;
                }
                //Color Coding Service Name
                dvTree = new DataView(dv.ToTable());
                foreach (TreeNode tNote in tvCategory.Nodes)
                {
                    string[] values = tNote.Value.Split('/');
                    dvTree.RowFilter = "DiagSampleId=" + values[1] + " AND ServiceId=" + values[0] + " AND StatusCode IN ('RE','RP','RF')";
                    if (dvTree.ToTable().Rows.Count > 0)
                    {
                        string fs = "<span style=\"background-color:" + common.myStr(dvTree.ToTable().Rows[0]["StatusColor"]) + "\">" + tNote.Text + "</span>";
                        tNote.Text = fs;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dvTree.Dispose();
        }
    }
    //Method to add the Current Dispayed Result To the Global List
    private void holdPreviousData(int serviceId, int diagSampleId, int registrationId, int ResultRemarksId, string ReviewRemark, int SampleTypeId)
    {
        try
        {
            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
            foreach (clsLabTemplate LT in collLabTemplate)
            {
                if (LT.ServiceId == serviceId && LT.DiagSampleId == diagSampleId && LT.SampleTypeId == SampleTypeId)
                {

                    LT.isResultEntered = false;
                    LT.ResultRemarksId = ResultRemarksId;
                    LT.ReviewRemark = ReviewRemark;
                    #region nonTabular data
                    int valuesRows = LT.dsField.Tables[FIELDS].Rows.Count;
                    for (int rIdx = 0; rIdx < LT.dsField.Tables[FIELDS].Rows.Count; rIdx++)
                    {
                        if (common.myStr(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldType"]) == common.getEnumStrVal(eFieldType.CheckBox))
                        {
                            DataRow[] drOptions = LT.dsField.Tables[OPTIONS].Select("FieldId=" + common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]));
                            if (drOptions.Length > 1)
                            {
                                valuesRows += drOptions.Length - 1;
                            }
                        }
                    }
                    /***********  new record ***********/
                    DataView dvResultValue = new DataView(LT.dsField.Tables[VALUES]);
                    dvResultValue.RowFilter = "ResultId=" + ddlResultId.SelectedValue;
                    DataTable dtFieldvalue = dvResultValue.ToTable();
                    if (dtFieldvalue.Rows.Count == 0 || dtFieldvalue.Rows.Count < valuesRows)
                    {
                        for (int rIdx = 0; rIdx < LT.dsField.Tables[FIELDS].Rows.Count; rIdx++)
                        {
                            DataRow[] drVaue = LT.dsField.Tables[VALUES].Select("FieldId=" + common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]) + " and ResultId=" + common.myInt(ddlResultId.SelectedValue));
                            DataRow[] drField = LT.dsField.Tables[FIELDS].Select("FieldId=" + common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]));

                            if (common.myStr(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldType"]) != common.getEnumStrVal(eFieldType.CheckBox))
                            {
                                if (drVaue.Length == 0 || drField.Length > 1)
                                {
                                    DataRow DR = LT.dsField.Tables[VALUES].NewRow();
                                    DR["FieldId"] = common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]);
                                    DR["FieldValue"] = "";
                                    DR["TextValue"] = "";
                                    DR["ResultFromDB"] = 0;
                                    DR["ResultId"] = ddlResultId.SelectedValue;
                                    LT.dsField.Tables[VALUES].Rows.Add(DR);

                                    LT.dsField.Tables[VALUES].AcceptChanges();
                                    LT.dsField.AcceptChanges();
                                }
                            }
                            else if (common.myStr(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldType"]) == common.getEnumStrVal(eFieldType.CheckBox))
                            {
                                DataRow[] drOptions = LT.dsField.Tables[OPTIONS].Select("FieldId=" + common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]));

                                int insertRowIdx = -1;

                                if (drVaue.Length > 0)
                                {
                                    for (int alredyIdx = 0; alredyIdx < LT.dsField.Tables[VALUES].Rows.Count; alredyIdx++)
                                    {
                                        if (common.myInt(LT.dsField.Tables[VALUES].Rows[alredyIdx]["FieldId"]) ==
                                                    common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]))
                                        {
                                            insertRowIdx = alredyIdx;
                                        }
                                    }
                                }
                                for (int optionRowIdx = 0; optionRowIdx < drOptions.Length; optionRowIdx++)
                                {
                                    drVaue = LT.dsField.Tables[VALUES].Select("FieldId=" + common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]));

                                    if (drOptions.Length > drVaue.Length)
                                    {
                                        DataRow DR = LT.dsField.Tables[VALUES].NewRow();
                                        DR["FieldId"] = common.myInt(LT.dsField.Tables[FIELDS].Rows[rIdx]["FieldId"]);
                                        DR["FieldValue"] = "";
                                        DR["TextValue"] = "";
                                        //DR["ServiceId"] = serviceId;
                                        if (insertRowIdx == -1)
                                        {
                                            LT.dsField.Tables[VALUES].Rows.Add(DR);
                                        }
                                        else
                                        {
                                            if (LT.dsField.Tables[VALUES].Rows.Count == (insertRowIdx + 1))
                                            {
                                                LT.dsField.Tables[VALUES].Rows.Add(DR);
                                            }
                                            else
                                            {
                                                LT.dsField.Tables[VALUES].Rows.InsertAt(DR, insertRowIdx + 1);
                                            }
                                            insertRowIdx++;
                                        }
                                        LT.dsField.Tables[VALUES].AcceptChanges();
                                        LT.dsField.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    int fieldId = 0;
                    int resultId = 1;
                    foreach (GridViewRow item2 in gvSelectedServices.Rows)
                    {
                        int fieldRowIdx = -1;
                        if (item2.RowType == DataControlRowType.DataRow)
                        {
                            string fieldType = common.myStr(item2.Cells[(int)eServicesGrid.FieldType].Text);
                            fieldId = common.myInt(item2.Cells[(int)eServicesGrid.FieldID].Text);
                            resultId = common.myInt(ddlResultId.SelectedValue);
                            for (int rIdx = 0; rIdx < LT.dsField.Tables[VALUES].Rows.Count; rIdx++)
                            {
                                if (common.myInt(LT.dsField.Tables[VALUES].Rows[rIdx]["FieldId"]) == fieldId && common.myInt(LT.dsField.Tables[VALUES].Rows[rIdx]["ResultId"]) == resultId)
                                {
                                    fieldRowIdx = rIdx;
                                    break;
                                }
                            }
                            if (fieldRowIdx == -1)
                            {
                                continue;
                            }
                            if (fieldType == common.getEnumStrVal(eFieldType.TextSingleLine))
                            {
                                TextBox txtT = (TextBox)item2.FindControl("txtT");

                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(txtT.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(txtT.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);

                                if (common.myLen(txtT.Text) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Numeric))
                            {
                                TextBox txtN = (TextBox)item2.FindControl("txtN");
                                HiddenField hdnScale = (HiddenField)item2.FindControl("hdnScale");

                                RadComboBox ddlRange = (RadComboBox)item2.FindControl("ddlRange");
                                if (common.myDbl(hdnScale.Value) > 0)
                                {
                                    //mmb
                                    //if (common.myInt(txtN.Text) == 0)
                                    if (common.myLen(txtN.Text) == 0)
                                    {
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(txtN.Text).Trim();
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(txtN.Text).Trim();
                                    }
                                    else
                                    {
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = txtN.Text == "" ? txtN.Text : common.myStr(txtN.Text).Trim();
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = txtN.Text == "" ? txtN.Text : common.myStr(txtN.Text).Trim();

                                        //LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = txtN.Text == "" ? txtN.Text : (common.myDbl(txtN.Text).ToString("F" + common.myInt(hdnScale.Value)));
                                        //LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = txtN.Text == "" ? txtN.Text : (common.myDbl(txtN.Text).ToString("F" + common.myInt(hdnScale.Value)));
                                    }
                                }
                                else
                                {
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(txtN.Text).Trim();
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(txtN.Text).Trim();
                                }

                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["RefRangeMachineId"] = common.myInt(ddlRange.SelectedValue);

                                if (common.myLen(txtN.Text) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Formula))
                            {
                                HiddenField hdnScale = (HiddenField)item2.FindControl("hdnScale");
                                TextBox txtF = (TextBox)item2.FindControl("txtF");
                                TextBox TxtFRemarks = (TextBox)item2.FindControl("TxtFRemarks");

                                if (common.myDbl(hdnScale.Value) > 0)
                                {
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = txtF.Text == "" ? txtF.Text : (common.myDbl(txtF.Text).ToString("F" + common.myInt(hdnScale.Value)));
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = txtF.Text == "" ? txtF.Text : (common.myDbl(txtF.Text).ToString("F" + common.myInt(hdnScale.Value)));
                                }
                                else
                                {
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = txtF.Text == "" ? txtF.Text : (common.myStr(common.myDbl(txtF.Text)).Trim());
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = txtF.Text == "" ? txtF.Text : (common.myStr(common.myDbl(txtF.Text)).Trim());
                                }
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FindingRemarks"] = common.myStr(TxtFRemarks.Text);

                                if (common.myLen(txtF.Text) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.TextMultiLine))
                            {
                                TextBox txtM = (TextBox)item2.FindControl("txtM");

                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(txtM.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(txtM.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);

                                if (common.myLen(txtM.Text) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Image))
                            {
                                #region Save Image on DB
                                HiddenField hdnFileAddress = (HiddenField)item2.FindControl("hdnFileAddress");
                                HiddenField hdnFileName = (HiddenField)item2.FindControl("hdnFileName");
                                if (common.myStr(ViewState["IsSaveClk"]) == "1")
                                {


                                    //System.IO.FileStream fs = new System.IO.FileStream(hdnFileAddress.Value, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                                    //System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                    //byte[] image = br.ReadBytes((int)fs.Length);
                                    //br.Close();
                                    //fs.Close();
                                    //

                                    byte[] image = Convert.FromBase64String(hdnFileAddress.Value);

                                    try
                                    {
                                        if (image.Length > 5)
                                        {
                                            StringBuilder strSQL = new StringBuilder();
                                            SqlConnection con = new SqlConnection(sConString);
                                            SqlCommand cmdTemp;
                                            SqlParameter prm1, prm2, prm3, prm4, prm5, prm6, prm7;
                                            strSQL.Append("Exec uspDiagSaveInvResultImage @iDiagSampleId,@iServiceId,@iFieldId,@iSampleImage,@sImageUrl,@iEncodedBy,@Source");
                                            cmdTemp = new SqlCommand(strSQL.ToString(), con);
                                            cmdTemp.CommandType = CommandType.Text;

                                            prm1 = new SqlParameter();
                                            prm1.ParameterName = "@iDiagSampleId";
                                            prm1.Value = common.myInt(diagSampleId);
                                            prm1.SqlDbType = SqlDbType.Int;
                                            prm1.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm1);

                                            prm2 = new SqlParameter();
                                            prm2.ParameterName = "@iServiceId";
                                            prm2.Value = common.myInt(serviceId);
                                            prm2.SqlDbType = SqlDbType.Int;
                                            prm2.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm2);

                                            prm3 = new SqlParameter();
                                            prm3.ParameterName = "@iFieldId";
                                            prm3.Value = common.myInt(fieldId);
                                            prm3.SqlDbType = SqlDbType.Int;
                                            prm3.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm3);

                                            prm4 = new SqlParameter();
                                            prm4.ParameterName = "@iSampleImage";
                                            prm4.Value = image;
                                            prm4.SqlDbType = SqlDbType.Image;
                                            prm4.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm4);

                                            prm5 = new SqlParameter();
                                            prm5.ParameterName = "@sImageUrl";
                                            prm5.Value = common.myStr(hdnFileName.Value.Trim());
                                            prm5.SqlDbType = SqlDbType.VarChar;
                                            prm5.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm5);

                                            prm6 = new SqlParameter();
                                            prm6.ParameterName = "@iEncodedBy";
                                            prm6.Value = common.myInt(Session["UserId"]);
                                            prm6.SqlDbType = SqlDbType.Int;
                                            prm6.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm6);

                                            prm7 = new SqlParameter();
                                            prm7.ParameterName = "@Source";
                                            prm7.Value = common.myStr(ViewState["SelectedSource"]);
                                            prm7.SqlDbType = SqlDbType.Char;
                                            prm7.Direction = ParameterDirection.Input;
                                            cmdTemp.Parameters.Add(prm7);

                                            con.Open();
                                            cmdTemp.ExecuteNonQuery();
                                            if (common.myLen(hdnFileAddress.Value) > 0)
                                            {
                                                LT.isResultEntered = true;
                                            }
                                        }
                                    }
                                    catch (Exception Ex)
                                    {
                                    }
                                }
                                #endregion
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.TextFormats))
                            {
                                RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                                //if (txtW.Text.Length > 0)
                                //{
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(txtW.Content).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(txtW.Content).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                if (common.myInt(ViewState["HorizontalView"]) == 0)
                                {
                                    if (common.myLen(txtW.Content) > 0)
                                    {
                                        LT.isResultEntered = true;
                                    }
                                }
                                // }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.ListofChoices))
                            {
                                RadComboBox ddl = (RadComboBox)item2.FindControl(common.getEnumStrVal(eFieldType.ListofChoices));
                                if (common.myInt(ddl.SelectedValue) > 0)
                                {
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(ddl.SelectedValue);
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(ddl.SelectedItem.Text);
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FindingRemarks"] = common.myStr(ddl.SelectedItem.Attributes["FindingRemarks"]);
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                }

                                if (common.myInt(ddl.SelectedValue) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Heading))
                            {
                                Label fieldName = (Label)item2.FindControl("lblFieldName");
                                Label lblHeaderStartSno = (Label)item2.FindControl("lblHeaderStartSno");
                                Label lblHeaderEndSno = (Label)item2.FindControl("lblHeaderEndSno");
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(fieldName.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(fieldName.Text).Trim();
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);

                                LT.isResultEntered = true;
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Boolean))
                            {
                                DropDownList ddlB = (DropDownList)item2.FindControl(common.getEnumStrVal(eFieldType.Boolean));
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(ddlB.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(ddlB.SelectedItem.Text);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);

                                if (common.myInt(ddlB.SelectedValue) > 0)
                                {
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.CheckBox))
                            {
                                Repeater rptC = (Repeater)item2.FindControl(common.getEnumStrVal(eFieldType.CheckBox));
                                foreach (RepeaterItem rptItem in rptC.Items)
                                {
                                    CheckBox chk = (CheckBox)rptItem.FindControl(common.getEnumStrVal(eFieldType.CheckBox));

                                    if (chk.Checked)
                                    {
                                        HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(hdn.Value);
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(chk.Text);  //common.myStr(CT.Value);
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                    }
                                    else
                                    {
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = "";
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = "";
                                        LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                    }
                                    LT.isResultEntered = true;
                                    fieldRowIdx++;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Date))
                            {
                                TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                                if (common.myStr(txtDate.Text).Trim() != "__/__/____"
                                   && common.myStr(txtDate.Text).Trim() != "")
                                {

                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myDate(txtDate.Text).ToString("dd/MM/yyyy");
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myDate(txtDate.Text).ToString("dd/MM/yyyy");
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);

                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Organism))
                            {
                                RadComboBox ddl = (RadComboBox)item2.FindControl(common.getEnumStrVal(eFieldType.Organism));
                                RadComboBox ddlMultilineFormatsOrganism = (RadComboBox)item2.FindControl("ddlMultilineFormatsOrganism");
                                TextBox txtMOrg = (TextBox)item2.FindControl("txtMOrg");
                                //if (common.myInt(ddl.SelectedValue) > 0)
                                //{
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(ddl.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(ddl.SelectedItem.Text);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FindingRemarks"] = common.myStr(txtMOrg.Text);
                                LT.isResultEntered = true;
                                //}
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Enzyme))
                            {
                                RadComboBox ddl = (RadComboBox)item2.FindControl(common.getEnumStrVal(eFieldType.Enzyme));
                                if (common.myInt(ddl.SelectedValue) > 0)
                                {
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = common.myStr(ddl.SelectedValue);
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = common.myStr(ddl.SelectedItem.Text);
                                    LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                    LT.isResultEntered = true;
                                }
                            }
                            else if (fieldType == common.getEnumStrVal(eFieldType.Time))
                            {

                                RadComboBox ddlHr = (RadComboBox)item2.FindControl("ddlHr");
                                RadComboBox ddlMin = (RadComboBox)item2.FindControl("ddlMin");
                                RadComboBox ddlSec = (RadComboBox)item2.FindControl("ddlSec");
                                Label lblTimeString = (Label)item2.FindControl("lblTimeString");
                                string Time = "";
                                int intConvertedTime = 0;
                                if (ddlHr.SelectedIndex > 0)
                                {
                                    Time = ddlHr.SelectedValue + " hr" + " ";
                                    intConvertedTime = common.myInt(ddlHr.SelectedValue) * 3600;

                                }
                                if (ddlMin.SelectedIndex > 0)
                                {
                                    Time += ddlMin.SelectedValue + " min" + " ";
                                    intConvertedTime += common.myInt(ddlMin.SelectedValue) * 60;
                                }
                                if (ddlSec.SelectedIndex > 0)
                                {
                                    Time += ddlSec.SelectedValue + " sec" + " ";
                                    intConvertedTime += common.myInt(ddlSec.SelectedValue);
                                }

                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FieldValue"] = Time;
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["TextValue"] = Time;
                                LT.dsField.Tables[0].Rows[fieldRowIdx]["UnitId"] = intConvertedTime;
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["ResultId"] = common.myInt(ddlResultId.SelectedValue);
                                LT.dsField.Tables[VALUES].Rows[fieldRowIdx]["FindingRemarks"] = common.myStr(lblTimeString.Text);
                                LT.isResultEntered = true;

                            }

                            LT.dsField.Tables[VALUES].AcceptChanges();
                        }
                    }

                    LT.dsField.Tables[VALUES].AcceptChanges();
                    LT.dsField.AcceptChanges();

                    #endregion nonTabular data
                    break;
                }
            }
        }
        catch (Exception)
        {
        }
    }
    //Method Used In case of Multiple Result entry Allowed
    private DataSet GetData()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = objval.getLabNoInvFormats(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue), common.myInt(Session["FacilityID"]),
                        common.myInt(ViewState["DIAG_SAMPLEID"]), common.myInt(ViewState["SERVICEID"]), common.myStr(ViewState["Gender"]), common.myInt(ViewState["AgeInDays"]), common.myStr(ViewState["StatusCode"]), common.myInt(Session["HospitalLocationId"]), common.myInt(hdnIsCallFromLab.Value));
        return ds;
    }
    //Method To check All Mandotary Condition are There Before Saving Data
    private bool isSave()
    {
        bool isSaved = true;
        string strMsg = "";
        if (common.myStr(rblSource.SelectedValue) == "")
        {
            strMsg += "Please Select Source ! <br />";
            isSaved = false;
        }
        if (common.myInt(txtLabNo.Text) == 0)
        {
            strMsg += "Please Fill " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            isSaved = false;
        }
        if (trReviewRemark.Visible && txtReviewRemark.Enabled && string.IsNullOrEmpty(common.myStr(txtReviewRemark.Text)))
        {
            strMsg += "Please Fill Review Remark ! ";
            txtReviewRemark.Focus();
            isSaved = false;
        }
        lblMessage.Text = strMsg;
        return isSaved;
    }
    //method is Used to Create A temp Datatable for the Organism Type Field
    protected DataTable CreateTableOrganism()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("FieldValue");
        Dt.Columns.Add("TextValue");
        return Dt;
    }
    //Method is to Get the Horizontal View Format data 
    private DataView GetHorizontalFormatData(int FieldId)
    {
        DataView dv = null;
        try
        {
            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
            int TableId = tvCategory.Nodes.IndexOf(tvCategory.SelectedNode);
            //holdPreviousData(common.myInt(ViewState["PRESERVICEID"]), common.myInt(ViewState["PREDIAG_SAMPLEID"]), common.myInt(ViewState["PRERegistrationId"]), common.myInt(ddlRemarks.SelectedValue));

            // 
            foreach (clsLabTemplate LT in collLabTemplate)
            {
                if (LT.ServiceId == common.myInt(ViewState["SERVICEID"]) && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                {

                    DataRow[] dr = LT.dsField.Tables[0].Select("FieldId=" + FieldId);
                    if (dr.Length > 0)
                    {
                        DataTable dt = LT.dsField.Tables[0].Clone();
                        dt.Rows.Add(dr[0].ItemArray);
                        objDs = new DataSet();
                        objDs.Tables.Add(dt);
                        objDs.Tables.Add(LT.dsField.Tables[1].Copy());
                        objDs.Tables.Add(LT.dsField.Tables[2].Copy());

                        dv = objDs.Tables[0].DefaultView;

                    }
                }
            }
        }
        catch (Exception)
        {

        }
        return dv;
    }
    //Method to set the Selected Test Values
    private void setSelectedNode(string Node)
    {
        int length = Node.Length;
        try
        {
            lnkProvisionalRelease.Visible = false;
            lnkFinalRelease.Visible = false;
            //Done 01 july 2015 by ujjwal to hide addendum link button start
            lnkAddendumRelease.Visible = false;
            //Done 01 july 2015 by ujjwal to hide addendum link button end
            char[] slaceChar = new char[1];
            slaceChar[0] = '/';
            string[] splitPart = Node.Split(slaceChar);
            ViewState["SelectedNode"] = common.myStr(splitPart[0]);
            ViewState["SERVICEID"] = common.myStr(splitPart[0]);
            ViewState["DIAG_SAMPLEID"] = common.myInt(splitPart[1]);
            ViewState["RESULT_REMARKSID"] = common.myInt(splitPart[2]);
            ViewState["RESULT_REVIEWREMARKS"] = common.myStr(splitPart[13]);
            ViewState["StatusCode"] = common.myStr(splitPart[3]);
            ViewState["MultipleResultEntry"] = common.myStr(splitPart[4]);
            ViewState["ResultHTML"] = common.myBool(splitPart[8]);

            ViewState["ManualLabNo"] = common.myStr(splitPart[9]).Replace("^^", "/");
            ViewState["IsClinicalTemplateRequired"] = common.myBool(splitPart[10]);
            ViewState["ServiceDetailId"] = common.myStr(splitPart[11]);
            ViewState["PrintReferenceRangeInHTML"] = common.myBool(splitPart[12]);
            lblOrderDate.Text = common.myStr(splitPart[14]).Replace("^^", "/");
            ddlMultiStageResultFinalized.SelectedIndex = ddlMultiStageResultFinalized.Items.IndexOf
(ddlMultiStageResultFinalized.Items.FindItemByValue(common.myStr(splitPart[18] == "True" ? "1" : "0")));

            if (!common.myStr(Request.QueryString["MD"]).Equals("RIS"))
            {
                lblScanIn1.Visible = false;
                lblScanOut1.Visible = false;
            }
            else
            {
                lblScanIn.Text = ":" + common.myStr(splitPart[15]).Replace("^^", "/");
                lblScanOut.Text = ":" + common.myStr(splitPart[16]).Replace("^^", "/");
                if (common.myBool(splitPart[17]) && string.IsNullOrEmpty(common.myStr(splitPart[15])))
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Result cannot be saved. Scan Time details not found!";
                }
            }
            if (common.myBool(splitPart[5]) == true)
            {
                ViewState["HorizontalView"] = 1;
            }
            else
            {
                ViewState["HorizontalView"] = 0;
            }
            if (common.myBool(splitPart[6]) == true)
            {
                ViewState["ResultAlert"] = 1;
            }
            else
            {
                ViewState["ResultAlert"] = 0;
            }
            if (common.myStr(splitPart[7]).Trim() != "")
            {
                ViewState["AlreadyTagWithEC"] = common.myStr(splitPart[7]);
            }
            else
            {
                ViewState["AlreadyTagWithEC"] = "0";
            }


            if (common.myStr(ViewState["StatusCode"]) == "RF")
            {
                gvSelectedServices.Enabled = true;
                ddlRemarks.Enabled = false;
                lblResultStatus.Text = "&nbsp;&nbsp;&nbsp;&nbsp;Result Finalized.";
                txtReviewRemark.Enabled = false;
            }
            else if (common.myStr(ViewState["StatusCode"]) == "RP" && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
            {
                gvSelectedServices.Enabled = true;
                ddlRemarks.Enabled = true;
                lblResultStatus.Text = "&nbsp;&nbsp;&nbsp;&nbsp;Result Provisional.";
                lnkFinalRelease.Visible = true;
                txtReviewRemark.Enabled = true;
            }
            else
            {
                gvSelectedServices.Enabled = true;
                ddlRemarks.Enabled = true;
                lblResultStatus.Text = "";
                if (common.myStr(ViewState["StatusCode"]) == "RE" && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                {
                    lnkProvisionalRelease.Visible = true;
                }
                txtReviewRemark.Enabled = true;
            }


            string strEditMacResult = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EditMachineResultLevel", sConString);

            if (strEditMacResult != "M")
            {
                ViewState["EditMachineWise"] = "N";
                if ((common.myStr(Session["EditMachineResult"]) == "True") || (common.myStr(Session["EditMachineResult"]) == "1"))
                {
                    ViewState["Permission"] = "Y";
                }
                else
                {
                    ViewState["Permission"] = "N";
                }
            }
            else
            {
                ViewState["EditMachineWise"] = "Y";
            }

            ViewState["EditProvisionalResult"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EditProvisionalResult", sConString);

            lblManualLabNo.Text = common.myStr(ViewState["ManualLabNo"]);

            lnkClinicalDetails.Visible = common.myBool(ViewState["IsClinicalTemplateRequired"]);

            //Session[""] = common.myStr(dsPermission.Tables[0].Rows[0]["EditProvisionalResult"]);
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Method To add The Nodes in the Treeview 
    private void AddNodes(TreeView tvName, string iNodeID, string sNodeText)
    {
        try
        {
            TreeNode masternode = new TreeNode(common.myStr(sNodeText), common.myStr(iNodeID));
            masternode.ToolTip = common.myStr(sNodeText);
            tvName.Nodes.Add(masternode);
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    #endregion
    //Region for the events outside the format grid
    #region Events for Outside Result Grid controls
    //Treeview SelectedNodeChanged event
    protected void tvCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }

            gvSelectedServices.Visible = true;
            lblMessage.Text = "";

            setSelectedNode(tvCategory.SelectedValue);

            //Checking Whether Sample IF Sample Is Alredy Collected in External Center or Not
            if (ViewState["AlreadyTagWithEC"] != null && common.myStr(ViewState["AlreadyTagWithEC"]) != "0")
            {
                btnResultFinalization.Visible = false;

                lblMessage.Text = "Sample Already Sent to External Center " + common.myStr(ViewState["AlreadyTagWithEC"]);
                gvSelectedServices.DataSource = null;
                gvSelectedServices.DataBind();
                tblSampleType.Visible = false;
            }
            else
            {
                tblSampleType.Visible = true;
                bindddlSampleType(common.myInt(ViewState["SERVICEID"]));
                ddlSampleType.SelectedIndex = ddlSampleType.Items.IndexOf(ddlSampleType.Items.FindByValue(common.myStr(ViewState["NewSampleTypeId"])));
                ViewState["NewSampleTypeId"] = "0";
                BindTemplateGrid();
            }
            if (gvSelectedServices.Rows.Count > 0)
            {
                for (int i = 0; i < gvSelectedServices.Rows.Count; i++)
                {
                    RadEditor rad1 = (RadEditor)gvSelectedServices.Rows[i].FindControl("txtW");

                    //rad1.RealFontSizes.Clear();
                    //rad1.RealFontSizes.Add("10px");
                    //rad1.RealFontSizes.Add("13px");
                    //rad1.RealFontSizes.Add("16px");
                    //rad1.RealFontSizes.Add("18px");
                    //rad1.RealFontSizes.Add("24px");
                    //rad1.RealFontSizes.Add("32px");
                    //rad1.RealFontSizes.Add("48px");

                    rad1.RealFontSizes.Clear();
                    rad1.RealFontSizes.Add("9pt");
                    rad1.RealFontSizes.Add("10pt");
                    rad1.RealFontSizes.Add("11pt");
                    rad1.RealFontSizes.Add("12pt");
                    rad1.RealFontSizes.Add("14pt");
                    rad1.RealFontSizes.Add("18pt");
                    rad1.RealFontSizes.Add("20pt");
                    rad1.RealFontSizes.Add("24pt");
                    rad1.RealFontSizes.Add("26pt");
                    rad1.RealFontSizes.Add("36pt");
                    rad1.EnsureToolsFileLoaded();

                }
            }

            if (ViewState["IsAskReviewRemark"] == null)
            {
                ViewState["IsAskReviewRemark"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                            common.myInt(Session["FacilityId"]), "IsAskReviewRemark", sConString);
            }

            if ((common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y")) &&
                    (common.myStr(ViewState["StatusCode"]).Equals("RE") || common.myStr(ViewState["StatusCode"]).Equals("RF")
                            || common.myStr(ViewState["StatusCode"]).Equals("RP")))
            {
                trReviewRemark.Visible = true;
            }
            else
            {
                trReviewRemark.Visible = false;
            }
            Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }

    //Template Gridview RowDatbound Event
    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //Hiding the Bound Fields -FieldID,FieldType,Remarks,ServiceId
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[(int)eServicesGrid.FieldID].Visible = false;
                e.Row.Cells[(int)eServicesGrid.FieldType].Visible = false;
                e.Row.Cells[(int)eServicesGrid.Remarks].Visible = false;
                e.Row.Cells[(int)eServicesGrid.ServiceId].Visible = false;
                e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Visible = false;
                e.Row.Cells[(int)eServicesGrid.ShowTextFormatInPopupPage].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (common.myInt(Session["HospitalLocationId"]) > 0)
                {
                    string cCtlType = string.Empty;

                    DataView objDv = null;
                    DataView objDvValue;
                    DataTable objDt = null;
                    //Getting Whether The Result Varies Machine Wise Or nOt
                    ViewState["MachineWise"] = common.myBool(DataBinder.Eval(e.Row.DataItem, "LimitMachineWise"));
                    //Getting Whether the field is tagged with machine or not
                    ViewState["LinkedWithMachine"] = common.myBool(DataBinder.Eval(e.Row.DataItem, "LinkedWithMachine"));

                    DataView ddv = new DataView(dt);
                    //Filtering the Particular Row FieldId Data 
                    if (ddv.Count > 0)
                    {
                        ddv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                        objDt = ddv.ToTable();
                        if (objDt.Rows.Count > 0)
                        {
                            e.Row.Visible = true;
                        }
                    }
                    else
                    {
                        //getting  the Particular Row FieldId Data 
                        if (objDs.Tables.Count > 2)
                        {

                            if (objDs.Tables[2].Rows.Count > 0)
                            {

                                objDvValue = objDs.Tables[2].DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId='" +
                                        common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'" + " AND ResultId='" + ddlResultId.SelectedValue + "'"; //0
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    e.Row.Visible = true;
                                }
                            }
                        }
                    }
                    HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                    if (e.Row.Cells[(int)eServicesGrid.FieldType].Text != null)
                    {
                        string fieldType = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldType].Text);
                        string FieldID = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                        //Block For Heading Type Field
                        #region Heading Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.Heading))
                        {
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            fieldName.Font.Bold = true;
                            fieldName.Font.Underline = true;
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            lnkTextFormat.Visible = false;

                            e.Row.Cells[(int)eServicesGrid.FieldName].ColumnSpan = 4;
                            e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Visible = false;
                        }
                        #endregion
                        // Santosh Write code for disable control when previous data exist.
                        DataTable tbl2 = new DataTable();
                        bool PreviousLabResult = true;
                        if (objDs != null)
                        {
                            if (objDs.Tables.Count > 1)
                            {
                                tbl2 = objDs.Tables[2];

                                tbl2.DefaultView.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "' AND PreviousLabResult=1";
                                if (tbl2.DefaultView.Count > 0)
                                {
                                    PreviousLabResult = false;
                                }
                            }
                        }

                        #region Image Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.Image))
                        {
                            cCtlType = fieldType;
                            Button ibtnUpload = (Button)e.Row.FindControl("ibtnUpload");
                            ImageButton imgRIS = (ImageButton)e.Row.FindControl("imgRIS");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            FileUpload iFileUploader = (FileUpload)e.Row.FindControl("iFileUploader");
                            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                            ibtnUpload.Visible = true;
                            btnRemoveimage.Visible = true;
                            BaseC.clsLISPhlebotomy objbs = new BaseC.clsLISPhlebotomy(sConString);
                            DataSet ds = objbs.Getimageresult(common.myInt(ViewState["DIAG_SAMPLEID"]), common.myInt(FieldID));
                            ibtnUpload.CommandName = common.myInt(ViewState["DIAG_SAMPLEID"]) + "_" + common.myInt(FieldID);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                btnRemoveimage.Visible = true;
                                imgRIS.Visible = true;

                                imgRIS.CommandName = ds.Tables[0].Rows[0]["imageUrl"].ToString();
                                btnRemoveimage.CommandName = ds.Tables[0].Rows[0]["ID"].ToString();
                                ibtnUpload.Visible = false;
                                iFileUploader.Visible = false;
                            }
                            else
                            {
                                btnRemoveimage.CommandName = "0";
                                imgRIS.Visible = false;
                                ibtnUpload.Visible = true;
                                iFileUploader.Visible = true;
                                btnRemoveimage.Visible = false;
                            }
                            //imgRIS.Visible = true;
                            txtW.Visible = false;
                            //iFileUploader.Visible = true;

                        }
                        #endregion

                        #region Single Line Textbox Type Field

                        //Block For Single Line Textbox Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.TextSingleLine))
                        {
                            cCtlType = fieldType;

                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                            HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                            TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");

                            //Added on 29-08-2014  Start Naushad
                            Label lblN = (Label)e.Row.FindControl("lblN");
                            RadToolTip ttSpecialReferenceRange = (RadToolTip)e.Row.FindControl("ttSpecialReferenceRange");
                            HiddenField hdnSpecialReferenceRange = (HiddenField)e.Row.FindControl("hdnSpecialReferenceRange");

                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");

                            ttSpecialReferenceRange.Visible = false;
                            lblN.Visible = false;
                            //Added on 29-08-2014 End Nausahd

                            lnkTextFormat.Visible = false;

                            bool ResultFromDb = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            txtT.Visible = true;

                            if (common.myInt(txtT.MaxLength) == 0)
                            {
                                txtT.MaxLength = 100;
                            }
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                txtT.Enabled = false;
                                txtT.CssClass = "MachineResultTxt"; // Data non editable
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                txtT.Enabled = true;
                                txtT.CssClass = "clsTextbox"; // Data editable
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                txtT.Enabled = false;
                                txtT.CssClass = "Disabledtxt"; // Data non editable
                            }
                            if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value)))//Result Finalized or Result Provisonal
                            {
                                txtT.Enabled = false;
                                txtT.CssClass = "Disabledtxt"; // Data non editable
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtT.CssClass = "MachineResultTxt";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtT.Enabled = true;
                                            }
                                            else
                                            {
                                                txtT.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtT.Enabled = false;
                                            }
                                            else
                                            {
                                                txtT.Enabled = true;
                                            }
                                        }
                                    }
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) == 0)
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtT.Enabled = false;
                                            txtT.CssClass = "MachineResultTxt"; // Data non editable
                                        }

                                    }

                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                    {
                                        txtT.Enabled = true;
                                        txtT.CssClass = "clsTextbox"; // Data editable
                                    }
                                    else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                    {
                                        txtT.Enabled = false;
                                        txtT.CssClass = "Disabledtxt"; // Data non editable
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value)))//Result Finalized or Result Provisonal
                                    {
                                        txtT.Enabled = false;
                                        txtT.CssClass = "Disabledtxt"; // Data non editable
                                    }


                                }

                            }
                            if (txtT.Text.Length == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtT.Enabled = true;
                                    txtT.CssClass = "clsTextbox"; // Data non editable
                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtT.Enabled = true;
                                        txtT.CssClass = "clsTextbox"; // Data non editable
                                    }
                                }
                            }
                            lblN.Text = "";
                            if (common.myLen(hdnUnitName.Value) > 0)
                            {
                                lblN.Visible = true;
                                lblN.Text = common.myStr(hdnUnitName.Value);
                            }
                            //Added on 29-08-2014 Start Nausahd Ali
                            if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                            {
                                lblN.Visible = true;
                                lblN.Text += " (Range:&nbsp;Special Reference Range)";
                                lblN.ToolTip = hdnSpecialReferenceRange.Value;
                                ttSpecialReferenceRange.Visible = true;
                            }
                            else
                            {
                                lblN.Text += " (Range:&nbsp;Undefined)";
                            }
                            //Added on 29-08-2014 End  Naushad Ali
                            //if (common.myBool(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization]) == true && common.myStr(ViewState["StatusCode"]) != "RF")
                            //{
                            //    image.Visible = true;
                            //    image.ImageUrl ="~/Images/close_new.JPEG";
                            //}


                            txtT.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtT.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtT.Focus();
                            //Santosh
                            //if (txtT.Enabled)
                            //{
                            //    txtT.Enabled = PreviousLabResult;
                            //}
                        }
                        #endregion
                        #region Numeric Textbox Type Field
                        //Block For Numeric Textbox Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Numeric))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");

                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            TextBox txtN = (TextBox)e.Row.FindControl("txtN");
                            Label lblN = (Label)e.Row.FindControl("lblN");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Label lblLocation = (Label)e.Row.FindControl("lblLocation");
                            RadComboBox ddlRange = (RadComboBox)e.Row.FindControl("ddlRange");
                            HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                            HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                            HiddenField hdnMinValue = (HiddenField)e.Row.FindControl("hdnMinValue");
                            HiddenField hdnMaxValue = (HiddenField)e.Row.FindControl("hdnMaxValue");
                            HiddenField hdnSymbol = (HiddenField)e.Row.FindControl("hdnSymbol");
                            TextBox txtFinalizedDislpayMachine = (TextBox)e.Row.FindControl("txtFinalizedDislpayMachine");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnScale = (HiddenField)e.Row.FindControl("hdnScale");
                            HiddenField hdnSpecialReferenceRange = (HiddenField)e.Row.FindControl("hdnSpecialReferenceRange");
                            RadToolTip ttSpecialReferenceRange = (RadToolTip)e.Row.FindControl("ttSpecialReferenceRange");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            ttSpecialReferenceRange.Visible = false;

                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            txtN.Visible = true;
                            lblN.Visible = true;
                            bool ResultFromDb = false;
                            txtFinalizedDislpayMachine.Visible = false;
                            string Range = "", Symbol = "";
                            double? minValue = null, maxValue = null;
                            if (common.myInt(txtN.MaxLength) == 0)
                            {
                                txtN.MaxLength = 10;
                            }
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddlRange.Enabled = false;
                                txtN.Enabled = false;
                                txtN.CssClass = "MachineResultTxt";  // Data non editable
                                ddlRange.Skin = "DisabledCombo";
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                //txtN.BackColor = Color.Empty;
                                //ddlRange.Enabled = true;
                                //ddlRange.Skin = "";
                                //txtN.Enabled = true;
                                //txtN.CssClass = "clsTextbox"; // Data  editable
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                txtN.BackColor = Color.Empty;
                                ddlRange.Enabled = false;
                                ddlRange.Skin = "DisabledCombo";
                                txtN.Enabled = false;
                                txtN.CssClass = "Disabledtxt"; // Data Not editable
                            }
                            if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value)))//Result Finalized or Result Provisonal
                            {
                                txtN.BackColor = Color.Empty;
                                ddlRange.Enabled = false;
                                ddlRange.Skin = "DisabledCombo";
                                txtN.Enabled = false;
                                txtN.CssClass = "Disabledtxt"; // Data Not editable

                            }


                            //Checkin whether result Varies machine Wise or not
                            if (common.myBool(ViewState["MachineWise"]) == true)
                            {

                                lblLocation.Visible = true;
                                ddlRange.Visible = true;
                                txtN.Width = 180;
                                if (objDs.Tables[3].Rows.Count > 0)
                                {
                                    objDv = objDs.Tables[3].DefaultView;
                                    objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                    if (objDv.Count > 0)
                                    {
                                        foreach (DataRowView dr in objDv)
                                        {

                                            RadComboBoxItem item = new RadComboBoxItem();
                                            item.Text = (string)dr["MachineName"];
                                            item.Value = dr["machineId"].ToString();
                                            item.Attributes.Add("FieldId", common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim());
                                            ddlRange.Items.Add(item);
                                            item.DataBind();
                                        }

                                        ViewState["Range"] = objDv.Table;
                                        objDv.RowFilter = "MachineId=" + ddlRange.SelectedValue + "And FieldId ='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'";
                                        if (objDv.Count > 0)
                                        {
                                            if (common.myStr(objDv[0]["minValue"]) != "")
                                            {
                                                minValue = common.myDbl(objDv[0]["minValue"]);
                                            }
                                            if (common.myStr(objDv[0]["maxValue"]) != "")
                                            {
                                                maxValue = common.myDbl(objDv[0]["maxValue"]);
                                            }

                                            Symbol = common.myStr(objDv[0]["Symbol"]);
                                            if (common.myStr(minValue) != "")
                                            {
                                                minValue = common.myDbl(minValue);
                                            }

                                            if (common.myStr(maxValue) != "")
                                            {
                                                maxValue = common.myDbl(maxValue);
                                            }

                                            Range = " (" + common.myDbl(minValue).ToString("F" + hdnScale.Value) + " " + common.myStr(Symbol) + " " + common.myDbl(maxValue).ToString("F" + hdnScale.Value) + ") ";
                                            txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                            txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                            lblN.Text = Range;
                                        }
                                    }
                                    else
                                    {
                                        if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                                        {
                                            lblN.Text += " (Range:&nbsp;Special Reference Range)";
                                            lblN.ToolTip = hdnSpecialReferenceRange.Value;
                                            ttSpecialReferenceRange.Visible = true;
                                        }
                                        else
                                        {
                                            lblN.Text += " (Range:&nbsp;Undefined)";
                                        }
                                    }
                                }
                            }
                            else
                            {

                                lblN.Text = common.myStr(hdnUnitName.Value);
                                if (common.myStr(hdnMinValue.Value).Trim() == ""
                                    && common.myStr(hdnMaxValue.Value).Trim() == "")
                                {
                                    if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                                    {
                                        lblN.Text += " (Range:&nbsp;Special Reference Range)";
                                        lblN.ToolTip = hdnSpecialReferenceRange.Value;
                                        ttSpecialReferenceRange.Visible = true;
                                    }
                                    else
                                    {
                                        lblN.Text += " (Range:&nbsp;Undefined)";
                                    }
                                }
                                else
                                {
                                    if (common.myStr(hdnMinValue.Value) != "")
                                    {
                                        minValue = common.myDbl(hdnMinValue.Value);
                                    }

                                    if (common.myStr(hdnMaxValue.Value) != "")
                                    {
                                        maxValue = common.myDbl(hdnMaxValue.Value);
                                    }

                                    lblN.Text += " (" + common.myDbl(minValue).ToString("F" + hdnScale.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(maxValue).ToString("F" + hdnScale.Value) + ") ";
                                }
                                if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                                {
                                    if (common.myStr(txtN.Text).Trim() != "")
                                    {
                                        if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtN.Text)
                                            && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtN.Text))
                                        {
                                            txtN.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            txtN.ForeColor = Color.Red;
                                        }
                                    }
                                    txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                    txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                }
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtN.CssClass = "MachineResultTxt";  // Data non editable
                                        ddlRange.Skin = "DisabledCombo";
                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtN.Enabled = true;
                                                ddlRange.Enabled = true;
                                            }
                                            else
                                            {
                                                txtN.Enabled = false;
                                                ddlRange.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtN.Enabled = false;
                                                ddlRange.Enabled = false;
                                            }
                                            else
                                            {
                                                txtN.Enabled = true;
                                                ddlRange.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtN.CssClass = "MachineResultTxt";  // Data non editable
                                            ddlRange.Skin = "DisabledCombo";
                                            ddlRange.Enabled = false;
                                            txtN.Enabled = false;
                                            txtFinalizedDislpayMachine.Visible = false;
                                            //ddlRange.Skin = "DisabledComboMachine";
                                        }
                                    }

                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                    {
                                        txtN.BackColor = Color.Empty;
                                        ddlRange.Enabled = true;
                                        ddlRange.Skin = "";
                                        txtN.Enabled = true;
                                        txtN.CssClass = "clsTextbox"; // Data  editable
                                    }
                                    else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                    {
                                        txtN.BackColor = Color.Empty;
                                        ddlRange.Enabled = false;
                                        ddlRange.Skin = "DisabledCombo";
                                        txtN.Enabled = false;
                                        txtN.CssClass = "Disabledtxt"; // Data Not editable
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                                    {
                                        txtN.CssClass = "Disabledtxt";  // Data non editable
                                        ddlRange.Skin = "DisabledCombo";
                                        ddlRange.Enabled = false;
                                        txtN.Enabled = false;
                                        txtN.CssClass = "Disabledtxt"; // Data non editable
                                        txtFinalizedDislpayMachine.CssClass = "Disabledtxt";

                                    }
                                    if ((common.myStr(objDt.Rows[0]["RefRangeMachineId"]) != "") && (common.myStr(objDt.Rows[0]["RefRangeMachineId"]) != "0"))
                                    {
                                        if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                        {
                                            txtN.Width = 180;
                                            txtN.CssClass = "Disabledtxt";
                                            txtFinalizedDislpayMachine.Visible = true;
                                            txtFinalizedDislpayMachine.Enabled = false;
                                            txtFinalizedDislpayMachine.CssClass = "Disabledtxt";
                                            lblLocation.Visible = true;
                                            ddlRange.Visible = false;
                                            txtFinalizedDislpayMachine.Text = common.myStr(objDt.Rows[0]["RefRangeMachineName"]);
                                            lblN.Text = common.myStr(hdnUnitName.Value);
                                            if (common.myStr(hdnMinValue.Value).Trim() == ""
                                                && common.myStr(hdnMaxValue.Value).Trim() == "")
                                            {
                                                if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                                                {
                                                    lblN.Text += " (Range:&nbsp;Special Reference Range)";
                                                    lblN.ToolTip = hdnSpecialReferenceRange.Value;
                                                    ttSpecialReferenceRange.Visible = true;
                                                }
                                                else
                                                {
                                                    lblN.Text += " (Range:&nbsp;Undefined)";
                                                }
                                            }
                                            else
                                            {
                                                if (common.myStr(hdnMinValue.Value) != "")
                                                {
                                                    minValue = common.myDbl(hdnMinValue.Value);
                                                }

                                                if (common.myStr(hdnMaxValue.Value) != "")
                                                {
                                                    maxValue = common.myDbl(hdnMaxValue.Value);
                                                }

                                                lblN.Text += " (" + common.myDbl(minValue).ToString("F" + hdnScale.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(maxValue).ToString("F" + hdnScale.Value) + ") ";
                                            }
                                        }
                                    }

                                    if (common.myBool(ViewState["MachineWise"]))
                                    {
                                        ddlRange.SelectedValue = common.myStr(objDt.Rows[0]["RefRangeMachineId"]);
                                        txtN.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                                        if (ddlRange.Items.Count > 0)
                                        {
                                            objDv.RowFilter = "MachineId=" + ddlRange.SelectedValue + "And FieldId ='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'";
                                            if (objDv.Count > 0)
                                            {
                                                if (common.myStr(objDv[0]["minValue"]) != "")
                                                {
                                                    minValue = common.myDbl(objDv[0]["minValue"]);
                                                }
                                                if (common.myStr(objDv[0]["maxValue"]) != "")
                                                {
                                                    maxValue = common.myDbl(objDv[0]["maxValue"]);
                                                }
                                                Symbol = common.myStr(objDv[0]["Symbol"]);
                                                Range = " (" + common.myDbl(minValue).ToString("F" + hdnScale.Value) + " " + common.myStr(Symbol) + " " + common.myDbl(maxValue).ToString("F" + hdnScale.Value) + ") ";
                                                txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                                txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                                lblN.Text = Range;
                                            }

                                            if (txtN.Text.Trim() != "")
                                            {
                                                if (minValue <= common.myDbl(txtN.Text) &&
                                                    maxValue >= common.myDbl(txtN.Text))
                                                {
                                                    txtN.ForeColor = Color.Black;
                                                }
                                                else
                                                {
                                                    txtN.ForeColor = Color.Red;
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        txtN.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                                        lblN.Text = common.myStr(hdnUnitName.Value);
                                        if (common.myStr(hdnMinValue.Value).Trim() == ""
                                            && common.myStr(hdnMaxValue.Value).Trim() == "")
                                        {
                                            if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                                            {
                                                lblN.Text += " (Range:&nbsp;Special Reference Range)";
                                                lblN.ToolTip = hdnSpecialReferenceRange.Value;
                                                ttSpecialReferenceRange.Visible = true;
                                            }
                                            else
                                            {
                                                lblN.Text += " (Range:&nbsp;Undefined)";
                                            }
                                        }
                                        else
                                        {
                                            if (common.myStr(hdnMinValue.Value) != "")
                                            {
                                                minValue = common.myDbl(hdnMinValue.Value);
                                            }

                                            if (common.myStr(hdnMaxValue.Value) != "")
                                            {
                                                maxValue = common.myDbl(hdnMaxValue.Value);
                                            }

                                            lblN.Text += " (" + common.myDbl(hdnMinValue.Value).ToString("F" + hdnScale.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(maxValue).ToString("F" + hdnScale.Value) + ") ";

                                        }
                                        if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                                        {
                                            if (common.myStr(txtN.Text) != "")
                                            {
                                                if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtN.Text)
                                                    && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtN.Text))
                                                {
                                                    txtN.ForeColor = Color.Black;
                                                }
                                                else
                                                {
                                                    txtN.ForeColor = Color.Red;
                                                }
                                            }
                                            txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                            txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");

                                            //txtN.TabIndex
                                        }
                                    }


                                }

                            }

                            if (common.myLen(txtN.Text) == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtFinalizedDislpayMachine.Visible = false;
                                    txtN.Enabled = true;
                                    txtN.CssClass = "clsTextbox"; // Data non editable
                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpayMachine.Visible = false;
                                        txtN.Enabled = true;
                                        txtN.CssClass = "clsTextbox"; // Data non editable
                                    }
                                }
                            }
                            txtN.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtN.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtN.Focus();
                            //santosh
                            //if (txtN.Enabled)
                            //{
                            //     txtN.Enabled = PreviousLabResult;
                            //}
                        }
                        #endregion
                        #region Formula type field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Formula))
                        {
                            Label lblFieldName = (Label)e.Row.FindControl("lblFieldName");
                            lblFieldName.Text = common.myStr(lblFieldName.Text) + "&nbsp;(Fx)";
                            bool ResultFromDb = false;
                            cCtlType = fieldType;


                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }

                            //End
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            TextBox txtF = (TextBox)e.Row.FindControl("txtF");
                            Button btnF = (Button)e.Row.FindControl("btnF");
                            Label lblF = (Label)e.Row.FindControl("lblF");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox TxtFRemarks = (TextBox)e.Row.FindControl("TxtFRemarks");
                            HiddenField hdnEditFormulaField = (HiddenField)e.Row.FindControl("hdnEditFormulaField");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            txtF.Visible = true;
                            lblF.Visible = true;
                            btnF.Visible = true;
                            TxtFRemarks.Visible = true;
                            //txtF.Attributes.Add("onfocus", "onFocusTxtF('" + btnF.ClientID + "');");

                            //txtF.Style.Add("Width", "50px");

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {


                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtF.CssClass = "Disabledtxt";

                                        TxtFRemarks.CssClass = "Disabledtxt";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtF.Enabled = true;
                                                TxtFRemarks.Enabled = true;
                                            }
                                            else
                                            {
                                                txtF.Enabled = false;
                                                TxtFRemarks.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtF.Enabled = false;
                                                TxtFRemarks.Enabled = false;
                                            }
                                            else
                                            {
                                                txtF.Enabled = true;
                                                TxtFRemarks.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            btnF.Enabled = false;
                                            btnF.CssClass = "Disabledbtn";
                                            txtF.Enabled = false;
                                            txtF.CssClass = "MachineResultTxt"; // Data non editable
                                            TxtFRemarks.Enabled = false;
                                            TxtFRemarks.CssClass = "MachineResultTxt"; // Data non editable
                                        }


                                    }


                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                    {
                                        txtF.Enabled = false;
                                        txtF.CssClass = "clsTextbox"; // Data non editable
                                        btnF.Enabled = true;
                                        TxtFRemarks.Enabled = false;
                                        TxtFRemarks.CssClass = "clsTextbox"; // Data  editable
                                    }
                                    else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                    {
                                        txtF.Enabled = false;
                                        txtF.CssClass = "Disabledtxt"; // Data non editable
                                        btnF.Enabled = false;
                                        btnF.CssClass = "Disabledbtn";
                                        TxtFRemarks.Enabled = false;
                                        TxtFRemarks.CssClass = "Disabledtxt"; // Data non editable
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                                    {
                                        txtF.Enabled = false;
                                        txtF.CssClass = "Disabledtxt"; // Data non editable
                                        btnF.Enabled = false;
                                        btnF.CssClass = "Disabledbtn";
                                        TxtFRemarks.Enabled = false;
                                        TxtFRemarks.CssClass = "Disabledtxt"; // Data non editable
                                    }
                                    txtF.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    TxtFRemarks.Text = common.myStr(objDt.Rows[0]["FindingRemarks"]);

                                }

                            }

                            if (txtF.Text.Length == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtF.Enabled = true;
                                    txtF.CssClass = "clsTextbox"; // Data non editable
                                    TxtFRemarks.Enabled = true;
                                    TxtFRemarks.CssClass = "clsTextbox"; // Data non editable

                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtF.Enabled = true;
                                        txtF.CssClass = "clsTextbox"; // Data non editable
                                        TxtFRemarks.Enabled = true;
                                        TxtFRemarks.CssClass = "clsTextbox"; // Data non editable
                                    }
                                }
                            }
                            HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                            HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                            HiddenField hdnMinValue = (HiddenField)e.Row.FindControl("hdnMinValue");
                            HiddenField hdnMaxValue = (HiddenField)e.Row.FindControl("hdnMaxValue");
                            HiddenField hdnSymbol = (HiddenField)e.Row.FindControl("hdnSymbol");
                            lblF.Text = common.myStr(hdnUnitName.Value);
                            if (common.myStr(hdnMinValue.Value).Trim() == ""
                                && common.myStr(hdnMaxValue.Value).Trim() == "")
                            {
                                lblF.Text += " (Range:&nbsp;Undefined)";
                            }
                            else
                            {
                                lblF.Text += " (" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ") ";
                            }

                            if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                            {
                                if (common.myStr(txtF.Text) != "")
                                {
                                    if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtF.Text)
                                        && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtF.Text))
                                    {
                                        txtF.ForeColor = Color.Black;
                                    }
                                    else
                                    {
                                        txtF.ForeColor = Color.Red;
                                    }
                                }
                                txtF.Attributes.Add("onkeypress", "checkRange('" + txtF.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                txtF.Attributes.Add("onkeyup", "checkRange('" + txtF.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");

                            }
                            if (hdnEditFormulaField.Value == "True")
                            {
                                txtF.ReadOnly = false;
                                txtF.Enabled = true;
                            }
                            else
                            {
                                txtF.ReadOnly = true;
                            }

                            txtF.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtF.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtF.Focus();
                            //santosh
                            //if (txtF.Enabled)
                            //{
                            //    txtF.Enabled = PreviousLabResult;
                            //}
                        }
                        #endregion
                        #region for Multiline Textbox
                        else if (fieldType == common.getEnumStrVal(eFieldType.TextMultiLine))
                        {
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                            bool ResultFromDb = false;
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl("ddlMultilineFormats"); //3
                            //Added on 29-08-2014  Start Naushad
                            Label lblTM = (Label)e.Row.FindControl("lblTM");
                            RadToolTip ttMSpecialReferenceRange = (RadToolTip)e.Row.FindControl("ttMSpecialReferenceRange");
                            HiddenField hdnSpecialReferenceRange = (HiddenField)e.Row.FindControl("hdnSpecialReferenceRange");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            ttMSpecialReferenceRange.Visible = false;
                            lblTM.Visible = false;
                            //Added on 29-08-2014 End Nausahd


                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                ddl.Enabled = true;
                                txtM.Enabled = true;
                                txtM.CssClass = "clsTextbox"; // Data non editable
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                ddl.Enabled = false;
                                txtM.Enabled = false;
                                txtM.CssClass = "Disabledtxt"; // Data non editable
                            }
                            if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                            {
                                ddl.Enabled = false;
                                txtM.Enabled = false;
                                txtM.CssClass = "Disabledtxt"; // Data non editable

                            }
                            if (objDs.Tables[1] != null)
                            {
                                DataRow[] drCheck = objDs.Tables[1].Select("ValueId=0 and FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'");
                                if (drCheck.Length == 0)
                                {
                                    DataRow dr = objDs.Tables[1].NewRow();
                                    dr["ValueId"] = 0;
                                    dr["ValueName"] = "[Select Text Format]";
                                    dr["FieldId"] = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                                    dr["SequenceNo"] = 0;
                                    objDs.Tables[1].Rows.InsertAt(dr, 0);
                                }
                                objDv = objDs.Tables[1].DefaultView;
                                objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                objDv.Sort = "SequenceNo Asc";
                                ddl.Visible = true;
                                ddl.DataSource = objDv; //0
                                ddl.DataValueField = "ValueId";
                                ddl.DataTextField = "ValueName";
                                ddl.DataBind();
                                objDv = objDs.Tables[1].DefaultView;

                            }

                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            //mmb
                            if (!common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                            {
                                btnHelp.Visible = true;
                            }
                            btnHelp.Attributes.Add("onclick", "openRadWindowW('" + txtM.ClientID + "','M','" + common.myStr(e.Row.RowIndex) + "','" + e.Row.Cells[0].Text + "','Lab')");
                            //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");

                            txtM.Visible = true;
                            txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtM.CssClass = "Disabledtxt";
                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtM.Enabled = true;
                                            }
                                            else
                                            {
                                                txtM.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtM.Enabled = false;
                                            }
                                            else
                                            {
                                                txtM.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtM.CssClass = "MachineResultTxt";
                                            txtM.Enabled = true;
                                        }
                                    }

                                }

                            }
                            if (txtM.Text.Length == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    ddl.Enabled = true;
                                    txtM.Enabled = true;
                                    txtM.CssClass = "clsTextbox"; // Data non editable

                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        ddl.Enabled = true;
                                        txtM.Enabled = true;
                                        txtM.CssClass = "clsTextbox"; // Data non editable

                                    }
                                }
                            }

                            //Added on 29-08-2014 Start Nausahd Ali
                            if (common.myLen(hdnSpecialReferenceRange.Value) > 0)
                            {
                                lblTM.Visible = true;
                                lblTM.Text += " (Range:&nbsp;Special Reference Range)";
                                lblTM.ToolTip = hdnSpecialReferenceRange.Value;
                                ttMSpecialReferenceRange.Visible = true;
                            }
                            else
                            {
                                lblTM.Text += " (Range:&nbsp;Undefined)";
                            }
                            //Added on 29-08-2014 End  Naushad Ali

                            ddl.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddl.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddl.Focus();

                            txtM.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtM.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtM.Focus();
                            //santosh
                            //if (txtM.Enabled)
                            //{
                            //    txtM.Enabled = PreviousLabResult;
                            //}
                        }
                        #endregion
                        #region text Format Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.TextFormats))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl("ddlTemplateFieldFormats"); //3
                            bool ResultFromDb = false;
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myBool(common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim())
                                && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            if (objDs.Tables[1] != null)
                            {
                                DataRow[] drCheck = objDs.Tables[1].Select("ValueId=0 and FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'");
                                if (drCheck.Length == 0)
                                {
                                    DataRow dr = objDs.Tables[1].NewRow();
                                    dr["ValueId"] = 0;
                                    dr["ValueName"] = "[Select Text Format]";
                                    dr["FieldId"] = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                                    dr["SequenceNo"] = 0;
                                    objDs.Tables[1].Rows.InsertAt(dr, 0);
                                }
                                objDv = objDs.Tables[1].DefaultView;
                                objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                objDv.Sort = "SequenceNo Asc";


                                // ddl.Visible = false;//add the functionlity of linkbutton

                                ddl.DataSource = objDv; //0
                                ddl.DataValueField = "ValueId";
                                ddl.DataTextField = "ValueName";
                                ddl.DataBind();
                            }
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            //   btnHelp.Visible = true;
                            btnHelp.Attributes.Add("onclick", "openRadWindowW('" + txtW.ClientID + "','W','" + common.myStr(e.Row.RowIndex) + "','" + e.Row.Cells[0].Text + "','Lab')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            if (!common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                            {
                                lnkTextFormat.Visible = true;
                            }
                            if (common.myInt(ViewState["HorizontalView"]) == 1)
                            {
                                txtW.Height = Unit.Pixel(1000);
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                //btnHelp.Visible = true;    santosh
                                //btnHelp.Attributes.Add("onclick", "openRadWindowW('" + txtW.ClientID + "','W','" + common.myStr(e.Row.RowIndex) + "')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                                ddl.Enabled = true;
                                txtW.Enabled = true; // Data non editable santosh

                                lnkTextFormat.Enabled = true;
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                ddl.Enabled = false;
                                txtW.Enabled = false; // Data non editable
                                lnkTextFormat.Enabled = false;
                            }
                            if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                            {
                                ddl.Enabled = false;
                                txtW.Enabled = false; // Data non editable
                                lnkTextFormat.Enabled = true; //Condition Modified on 06-09-2014 Nauahad 
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    //string Content = common.myStr(objDt.Rows[0]["FieldValue"].ToString().Replace("&gt;", ""));
                                    string Content = common.myStr(objDt.Rows[0]["FieldValue"].ToString());//.Replace("&gt;", ""));
                                    //string Content1 = Content.Replace("&lt;", "");
                                    //Content = Content.Replace("font size=1", "<span style=\"font-size: 10px");
                                    // Content = Content.Replace("font size=2", "<span style=\"font-size: 13px");
                                    // Content = Content.Replace("font size=3", "<span style=\"font-size: 16px");
                                    // Content = Content.Replace("font size=4", "<span style=\"font-size: 18px");
                                    //Content = Content.Replace("font size=5", "<span style=\"font-size: 24px");
                                    //Content = Content.Replace("font size=6", "<span style=\"font-size: 32px");
                                    //Content = Content.Replace("font size=7", "<span style=\"font-size: 48px");
                                    //Content = Content.Replace("<u>", "<span style=\"text-decoration: underline;\">");
                                    //Content = Content.Replace("</u>", "</span>");
                                    //Content = Content.Replace("</font>", "</span>");
                                    //Content = Content.Replace("</font>", "</span>");
                                    txtW.Content = Content;
                                    if (Content != "")
                                    {
                                        lnkTextFormat.BackColor = System.Drawing.Color.YellowGreen;
                                        lnkTextFormat.Font.Bold = true;
                                    }
                                }
                            }
                            if (txtW.Text.Length == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false) && (!common.myBool(hdnIsAddendum.Value)))//Result Finalized or Result Provisonal
                                {
                                    ddl.Enabled = true;
                                    ddl.Skin = "Outlook";
                                    ddl.EnableEmbeddedSkins = false;
                                    txtW.Enabled = true;

                                    txtW.CssClass = "clsTextbox"; // Data non editable
                                    lnkTextFormat.Enabled = true;
                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        ddl.Enabled = true;
                                        ddl.Skin = "Outlook";
                                        ddl.EnableEmbeddedSkins = false;
                                        txtW.Enabled = true;
                                        txtW.CssClass = "clsTextbox"; // Data non editable
                                        lnkTextFormat.Enabled = true;
                                    }
                                }
                            }
                            if (e.Row.Cells[(int)eServicesGrid.ShowTextFormatInPopupPage].Text == "False" && !common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                            {
                                ddl.Visible = true;
                                txtW.Visible = true;
                                btnHelp.Visible = true;
                                lnkTextFormat.Visible = false;
                            }
                            else
                            {
                                ddl.Visible = false;
                                txtW.Visible = false;
                                btnHelp.Visible = false;
                                if (!common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                                {
                                    lnkTextFormat.Visible = true;
                                }
                                else
                                {
                                    ddl.Visible = true;
                                    txtW.Visible = true;
                                    btnHelp.Visible = true;
                                    lnkTextFormat.Visible = false;
                                }
                            }
                            ddl.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddl.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddl.Focus();

                            txtW.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtW.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtW.Focus();

                            //santosh
                            //if (txtW.Enabled)
                            //{
                            //    txtW.Enabled = PreviousLabResult;
                            //}


                            txtW.RealFontSizes.Clear();
                            txtW.RealFontSizes.Add("9pt");
                            txtW.RealFontSizes.Add("10pt");
                            txtW.RealFontSizes.Add("11pt");
                            txtW.RealFontSizes.Add("12pt");
                            txtW.RealFontSizes.Add("14pt");
                            txtW.RealFontSizes.Add("18pt");
                            txtW.RealFontSizes.Add("20pt");
                            txtW.RealFontSizes.Add("24pt");
                            txtW.RealFontSizes.Add("26pt");
                            txtW.RealFontSizes.Add("36pt");
                            txtW.StripFormattingOptions = Telerik.Web.UI.EditorStripFormattingOptions.MSWordRemoveAll | Telerik.Web.UI.EditorStripFormattingOptions.ConvertWordLists;
                        }
                        #endregion
                        #region  For Checkbox type Fields
                        else if (fieldType == common.getEnumStrVal(eFieldType.CheckBox))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "' AND ValueType='V'"; //0
                            objDv.Sort = "SequenceNo Desc";

                            Repeater rpt = (Repeater)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.CheckBox));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            tbl1.Visible = false;
                            rpt.Visible = true;
                            rpt.DataSource = objDv;
                            rpt.DataBind();
                            HtmlTextArea txtRemarks = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                            txtRemarks.Visible = false;
                            foreach (RepeaterItem item in rpt.Items)
                            {
                                HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                                CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                                CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                                if (objDt != null)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {
                                        HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                                        CheckBox chk = (CheckBox)item.FindControl(common.getEnumStrVal(eFieldType.CheckBox));
                                        foreach (DataRow drow in objDt.Rows)
                                        {
                                            if (common.myStr(drow["FieldValue"]).Trim() == common.myStr(hdn.Value).Trim())
                                            {
                                                chk.Checked = true; //rafat

                                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                                {
                                                    chk.Enabled = true;
                                                }
                                                else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                                {
                                                    chk.Enabled = false;
                                                }
                                                if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                                                {
                                                    chk.Enabled = false;
                                                    //txtM.CssClass = "Disabledtxt"; // Data non editable
                                                }
                                            }
                                        }
                                        chk.TabIndex = (short)countControls++;
                                        hdnTotalTabIndexValue.Value = countControls.ToString();
                                        chk.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                                        if ((countControls - 1) == 1)
                                            chk.Focus();

                                        //santosh
                                        // if (chk.Enabled)
                                        //{
                                        //     chk.Enabled = PreviousLabResult;
                                        // }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region for Boolean Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Boolean))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End
                            RadComboBox ddlB = (RadComboBox)e.Row.FindControl(common.getEnumStrVal(eFieldType.Boolean));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            tbl1.Visible = false;
                            ddlB.Visible = true;
                            bool ResultFromDb = false;
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                    {
                                        ddlB.Enabled = false;
                                        ddlB.EnableEmbeddedSkins = false;
                                        ddlB.Skin = "DisabledCombo"; // Data non editable
                                    }

                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                    {
                                        ddlB.Enabled = true;
                                    }
                                    else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                    {
                                        ddlB.Enabled = false;
                                        ddlB.EnableEmbeddedSkins = false;
                                        ddlB.Skin = "DisabledCombo"; // Data non editable
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                                    {
                                        ddlB.Enabled = false;
                                        ddlB.EnableEmbeddedSkins = false;
                                        ddlB.Skin = "DisabledCombo"; // Data non editable
                                    }
                                    objDvValue = objDt.DefaultView;
                                    if (objDvValue.Table.Columns["FieldId"] != null)
                                    {
                                        objDvValue.RowFilter = "FieldId='" + e.Row.Cells[(int)eServicesGrid.FieldID].Text.Trim() + "'";
                                    }

                                    objDt = objDvValue.ToTable();
                                    if (objDt.Rows.Count > 0)
                                    {
                                        if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                        {

                                            ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                            ddlB.EnableEmbeddedSkins = false;
                                            ddlB.Skin = "DisabledCombo";

                                            if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                                {
                                                    ddlB.Enabled = true;
                                                }
                                                else
                                                {
                                                    ddlB.Enabled = false;
                                                }
                                            }
                                            else
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                                {
                                                    ddlB.Enabled = false;
                                                }
                                                else
                                                {
                                                    ddlB.Enabled = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                            {
                                                ddlB.Enabled = false;
                                                ddlB.EnableEmbeddedSkins = false;
                                                ddlB.Skin = "DisabledCombo"; // Data non editable
                                            }
                                        }

                                        if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                        {
                                            ddlB.Enabled = true;
                                        }
                                        else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                        {
                                            ddlB.Enabled = false;
                                            ddlB.EnableEmbeddedSkins = false;
                                            ddlB.Skin = "DisabledCombo"; // Data non editable
                                        }
                                        if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                                        {
                                            ddlB.Enabled = false;
                                            ddlB.EnableEmbeddedSkins = false;
                                            ddlB.Skin = "DisabledCombo"; // Data non editable
                                        }
                                        if (common.myStr(objDt.Rows[0]["FieldValue"]) == "1")//rafat
                                        {
                                            ddlB.SelectedValue = "1";
                                        }
                                        else if (common.myStr(objDt.Rows[0]["FieldValue"]) == "0")
                                        {
                                            ddlB.SelectedValue = "0";
                                        }
                                        else
                                        {
                                            ddlB.SelectedValue = "-1";
                                        }


                                    }



                                }


                            }
                            if (ddlB.SelectedValue == "0")
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    ddlB.Enabled = true;
                                }
                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        ddlB.Enabled = true;
                                    }
                                }
                            }
                            ddlB.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddlB.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddlB.Focus();

                            //santosh
                            // if (ddlB.Enabled)
                            //{
                            //     ddlB.Enabled = PreviousLabResult;
                            // }
                        }
                        #endregion
                        #region for Dropdown Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.ListofChoices))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "' AND ValueType='V'"; //0
                            objDv.Sort = "SequenceNo Asc";
                            bool ResultFromDb = false;
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.ListofChoices));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            Label lblD = (Label)e.Row.FindControl("lblD");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            lnkTextFormat.Visible = false;
                            lblD.Visible = true;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            txtFinalizedDislpay.Visible = false;
                            foreach (DataRowView dr in objDv)
                            {
                                RadComboBoxItem item = new RadComboBoxItem();
                                item.Text = (string)dr["ValueName"];
                                item.Value = dr["ValueId"].ToString();
                                item.Attributes.Add("FindingRemarks", dr["Remarks"].ToString());
                                ddl.Items.Add(item);
                                item.DataBind();
                            }

                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                ddl.Enabled = true;
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }
                            if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myStr(objDt.Rows[0]["textValue"]) != "")
                                    {

                                        if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                                        {
                                            ddl.Enabled = true;
                                            ddl.Visible = true;
                                        }
                                        else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                                        {
                                            txtFinalizedDislpay.Visible = true;
                                            txtFinalizedDislpay.Enabled = false;
                                            txtFinalizedDislpay.CssClass = "Disabledtxt";
                                            ddl.Visible = false;
                                            ddl.Enabled = false;
                                            ddl.Skin = "DisabledCombo"; // Data non editable
                                            txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["textValue"]);
                                        }
                                        if (common.myStr(ViewState["StatusCode"]) == "RF") //Result Finalized or Result Provisonal
                                        {
                                            txtFinalizedDislpay.Visible = true;
                                            txtFinalizedDislpay.Enabled = false;
                                            txtFinalizedDislpay.CssClass = "Disabledtxt";
                                            ddl.Visible = false;
                                            ddl.Enabled = false;
                                            ddl.Skin = "DisabledCombo"; // Data non editable
                                            txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["textValue"]);
                                        }

                                        ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                        if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                        {
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.Skin = "DisabledCombo"; // Data non editable

                                            if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                                {
                                                    ddl.Enabled = true;
                                                }
                                                else
                                                {
                                                    ddl.Enabled = false;
                                                }
                                            }
                                            else
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                                {
                                                    ddl.Enabled = false;
                                                }
                                                else
                                                {
                                                    ddl.Enabled = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                            {
                                                ddl.Enabled = false;
                                                ddl.EnableEmbeddedSkins = false;
                                                ddl.Skin = "DisabledCombo";

                                            }
                                        }
                                    }

                                }


                            }

                            if ((common.myStr(ViewState["StatusCode"]) != "RF") || (common.myStr(ViewState["StatusCode"]) != "RP"))//Result Finalized or Result Provisonal
                            {
                                string strRemarks = common.myStr(ddl.SelectedItem.Attributes["FindingRemarks"]);
                                if (strRemarks != "")
                                {
                                    lblD.Text = " (" + strRemarks + ")";
                                }
                                else
                                {
                                    lblD.Text = "";
                                }
                            }
                            if (ddl.SelectedIndex == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtFinalizedDislpay.Visible = false;
                                    txtFinalizedDislpay.CssClass = "clsTextbox";
                                    ddl.Visible = true;
                                    ddl.Enabled = true;
                                    ddl.Skin = "Outlook";
                                }

                            }
                            else
                            {
                                if (ResultFromDb == false)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = false;
                                        txtFinalizedDislpay.CssClass = "clsTextbox";
                                        ddl.Visible = true;
                                        ddl.Enabled = true;
                                        ddl.Skin = "Outlook";
                                    }
                                }
                            }

                            ddl.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddl.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddl.Focus();

                            //santosh
                            //if (ddl.Enabled)
                            //{
                            //    ddl.Enabled = PreviousLabResult;
                            //}
                        }
                        #endregion
                        #region Date type field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Date))
                        {

                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }

                            //End

                            HtmlTable tblDate = (HtmlTable)e.Row.Cells[(int)eServicesGrid.Values].FindControl("tblDate");
                            TextBox txtDate = e.Row.Cells[(int)eServicesGrid.Values].FindControl("txtDate") as TextBox;
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    txtDate.Enabled = true;
                                    txtDate.CssClass = "clsTextbox";
                                    txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]);//rafat
                                }
                            }

                            txtDate.Attributes.Add("onblur", "validateDate('" + txtDate.ClientID + "');");
                            tblDate.Visible = true;

                            txtDate.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            txtDate.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                txtDate.Focus();

                            //santosh
                            // if (txtDate.Enabled)
                            //{
                            //     txtDate.Enabled = PreviousLabResult;
                            // }
                        }
                        #endregion
                        #region Organism Dropdown start
                        else if (fieldType == common.getEnumStrVal(eFieldType.Organism))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId=0 And ValueType='O'"; //0
                            objDv.Sort = "SequenceNo,ValueName Asc";
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Organism));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            HtmlTable tblOrganism = (HtmlTable)e.Row.FindControl("tblOrganism");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            TextBox txtMOrg = (TextBox)e.Row.FindControl("txtMOrg");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            lnkTextFormat.Visible = false;
                            ddl.DataSource = objDv;
                            ddl.DataTextField = "ValueName";
                            ddl.DataValueField = "ValueId";
                            ddl.DataBind();
                            bool ResultFromDb = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            txtFinalizedDislpay.Visible = false;
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.Skin = "DisabledCombo";
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    txtMOrg.Text = common.myStr(objDt.Rows[0]["FindingRemarks"]);
                                }
                            }


                            if (common.myStr(ViewState["StatusCode"]) == "RF") //Result Finalized or Result Provisonal
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }
                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                ddl.Visible = true;
                                ddl.Enabled = true;
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {

                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        ddl.EnableEmbeddedSkins = false;
                                        ddl.Skin = "DisabledCombo";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                ddl.Enabled = true;
                                            }
                                            else
                                            {
                                                ddl.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                ddl.Enabled = false;
                                            }
                                            else
                                            {
                                                ddl.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            ddl.Enabled = false;
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.Skin = "DisabledCombo";
                                        }
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Enabled = false;
                                        txtFinalizedDislpay.Visible = true;
                                        txtFinalizedDislpay.CssClass = "Disabledtxt";
                                        ddl.Visible = false;
                                        ddl.Enabled = false;
                                        ddl.Skin = "DisabledCombo"; // Data non editable
                                    }
                                    if (common.myBool(hdnIsAddendumReleased.Value) || (common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["TextValue"]);

                                    }

                                }
                                if (ddl.SelectedIndex == 0)
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = false;
                                        txtFinalizedDislpay.CssClass = "clsTextbox";
                                        txtFinalizedDislpay.Enabled = false;
                                        ddl.Visible = true;
                                        ddl.Enabled = true;
                                        ddl.Skin = "Outlook";
                                    }
                                }
                                else
                                {
                                    if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = false;
                                        txtFinalizedDislpay.CssClass = "clsTextbox";
                                        txtFinalizedDislpay.Enabled = false;
                                        ddl.Visible = true;
                                        ddl.Enabled = true;
                                        ddl.Skin = "Outlook";
                                    }
                                }
                            }
                            ddl.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddl.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddl.Focus();
                        }
                        #endregion
                        #region Enzyme Dropdown start
                        else if (fieldType == common.getEnumStrVal(eFieldType.Enzyme))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId=0 And ValueType='E'"; //0
                            objDv.Sort = "ValueName Asc";
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Enzyme));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            lnkTextFormat.Visible = false;
                            ddl.DataSource = objDv;
                            ddl.DataTextField = "ValueName";
                            ddl.DataValueField = "ValueId";
                            ddl.DataBind();
                            bool ResultFromDb = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            txtFinalizedDislpay.Visible = false;
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.Skin = "DisabledCombo";
                            }

                            if (common.myStr(ViewState["StatusCode"]) == "RF") //Result Finalized or Result Provisonal
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }
                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                ddl.Visible = true;
                                ddl.Enabled = true;
                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "DisabledCombo"; // Data non editable
                            }

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {

                                    ResultFromDb = common.myBool(objDt.Rows[0]["ResultFromDB"]);
                                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        ddl.EnableEmbeddedSkins = false;
                                        ddl.Skin = "DisabledCombo";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                ddl.Enabled = true;
                                            }
                                            else
                                            {
                                                ddl.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                ddl.Enabled = false;
                                            }
                                            else
                                            {
                                                ddl.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            ddl.Enabled = false;
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.Skin = "DisabledCombo";
                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = true;
                                        txtFinalizedDislpay.CssClass = "Disabledtxt";
                                        txtFinalizedDislpay.Enabled = false;
                                        ddl.Enabled = false;
                                        ddl.Visible = false;
                                        ddl.Skin = "Telerik"; // Data non editable
                                        txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["TextValue"]);
                                    }
                                }

                            }
                            if (ddl.SelectedIndex == 0)
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtFinalizedDislpay.Visible = false;
                                    txtFinalizedDislpay.CssClass = "clsTextbox";
                                    ddl.Visible = true;
                                    ddl.Enabled = true;
                                    ddl.Skin = "Outlook";
                                }
                            }
                            else
                            {
                                //if (ResultFromDb == false)
                                //{
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    txtFinalizedDislpay.Visible = false;
                                    txtFinalizedDislpay.CssClass = "clsTextbox";
                                    ddl.Visible = true;
                                    ddl.Enabled = true;
                                    ddl.Skin = "Outlook";
                                }
                                //}
                            }
                            ddl.TabIndex = (short)countControls++;
                            hdnTotalTabIndexValue.Value = countControls.ToString();
                            ddl.Attributes.Add("onkeydown", "Javascript: if (event.keyCode==9) searching();");
                            if ((countControls - 1) == 1)
                                ddl.Focus();
                        }
                        #endregion
                        #region Sensitivity Dropdown End
                        else if (fieldType == common.getEnumStrVal(eFieldType.Sensitivity))
                        {
                            cCtlType = fieldType;
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");

                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            Button btnSN = (Button)e.Row.FindControl("btnSN");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;
                            if (!common.myStr(Request.QueryString["FromMaster"]).Equals("WARD"))
                            {
                                btnSN.Visible = true;
                            }
                            btnSN.Enabled = true;
                        }
                        #endregion
                        #region for Time type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Time))
                        {
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");
                            //Show the close image if ShowAfterResultFinalization is true
                            if (common.myStr(e.Row.Cells[(int)eServicesGrid.ShowAfterResultFinalization].Text).Trim() == "True" && common.myStr(ViewState["StatusCode"]) != "RF")
                            {
                                ImageButton imgShowImage = (ImageButton)e.Row.Cells[(int)eServicesGrid.Image].FindControl("imgShowImage");
                                imgShowImage.Visible = true;
                            }
                            //End

                            Label lblTimeString = (Label)e.Row.FindControl("lblTimeString");
                            HtmlTable tblDate = (HtmlTable)e.Row.FindControl("Tabletm");
                            RadComboBox ddlHr = (RadComboBox)e.Row.FindControl("ddlHr");
                            RadComboBox ddlMin = (RadComboBox)e.Row.FindControl("ddlMin");
                            RadComboBox ddlSec = (RadComboBox)e.Row.FindControl("ddlSec");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                            LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                            HiddenField hdnIsAddendum = (HiddenField)e.Row.FindControl("hdnIsAddendum");
                            lnkTextFormat.Visible = false;
                            txtW.Visible = false;
                            btnHelp.Visible = false;

                            if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "YES"))
                            {
                                ddlHr.Enabled = true;
                                ddlMin.Enabled = true;
                                ddlSec.Enabled = true;

                            }
                            else if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myStr(ViewState["EditProvisionalResult"]) == "NO"))
                            {
                                ddlHr.Enabled = false;
                                ddlHr.Skin = "Outlook"; // Data non editable
                                ddlHr.EnableEmbeddedSkins = false;

                                ddlMin.Enabled = false;
                                ddlMin.Skin = "Outlook"; // Data non editable
                                ddlMin.EnableEmbeddedSkins = false;

                                ddlSec.Enabled = false;
                                ddlSec.Skin = "Outlook"; // Data non editable
                                ddlSec.EnableEmbeddedSkins = false;
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RF" && !common.myBool(hdnIsAddendum.Value)) || (!common.myStr(ViewState["StatusCode"]).Equals("RF") && common.myBool(hdnIsAddendum.Value))) //Result Finalized or Result Provisonal
                            {
                                ddlHr.Enabled = false;
                                ddlHr.Skin = "Outlook"; // Data non editable
                                ddlHr.EnableEmbeddedSkins = false;

                                ddlMin.Enabled = false;
                                ddlMin.Skin = "Outlook"; // Data non editable
                                ddlMin.EnableEmbeddedSkins = false;

                                ddlSec.Enabled = false;
                                ddlSec.Skin = "Outlook"; // Data non editable
                                ddlSec.EnableEmbeddedSkins = false;
                            }


                            if (common.myStr(objDs.Tables[0].Rows[0]["FormulaDefinition"]) != "")
                            {
                                lblTimeString.Text = "(" + common.myStr(objDs.Tables[0].Rows[0]["FormulaDefinition"]).Trim() + ")";
                            }

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count != 0)
                                {
                                    if (common.myStr(objDt.Rows[0]["FieldValue"]) != "")
                                    {
                                        if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                        {
                                            ddlHr.Enabled = false;
                                            ddlHr.Skin = "Outlook"; // Data non editable
                                            ddlHr.EnableEmbeddedSkins = false;

                                            ddlMin.Enabled = false;
                                            ddlMin.Skin = "Outlook"; // Data non editable
                                            ddlMin.EnableEmbeddedSkins = false;

                                            ddlSec.Enabled = false;
                                            ddlSec.Skin = "Outlook"; //
                                            ddlSec.EnableEmbeddedSkins = false;

                                        }
                                        string[] FieldValue = common.myStr(objDt.Rows[0]["FieldValue"]).Split(' ');
                                        if (FieldValue.Length == 7)
                                        {
                                            ddlHr.SelectedValue = FieldValue[0];
                                            ddlMin.SelectedValue = FieldValue[2];
                                            ddlSec.SelectedValue = FieldValue[4];
                                        }
                                        if (FieldValue.Length == 5)
                                        {
                                            if (FieldValue[1] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[0];
                                            }

                                            if (FieldValue[3] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[2];
                                            }
                                            if (FieldValue[3] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[2];
                                            }
                                            if (FieldValue[3] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[2];
                                            }
                                        }
                                        if (FieldValue.Length == 3)
                                        {
                                            if (FieldValue[1] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[0];
                                            }
                                        }

                                    }
                                }

                            }
                            if ((ddlHr.SelectedIndex == 0) && (ddlMin.SelectedIndex == 0) && (ddlSec.SelectedIndex == 0))
                            {
                                if ((common.myStr(ViewState["StatusCode"]) == "RP") && (common.myBool(ViewState["LinkedWithMachine"]) == false))//Result Finalized or Result Provisonal
                                {
                                    ddlHr.Enabled = true;
                                    ddlHr.Skin = "Outlook"; // Data non editable
                                    ddlHr.EnableEmbeddedSkins = false;

                                    ddlMin.Enabled = true;
                                    ddlMin.Skin = "Outlook"; // Data non editable
                                    ddlMin.EnableEmbeddedSkins = false;

                                    ddlSec.Enabled = true;
                                    ddlSec.Skin = "Outlook"; //
                                    ddlSec.EnableEmbeddedSkins = false;

                                }
                            }

                            tblDate.Visible = true;
                        }
                        #endregion
                    }
                    else
                    {
                        e.Row.Cells[(int)eServicesGrid.Values].Text = "No Record Found!";
                    }
                    if (cCtlType == common.getEnumStrVal(eFieldType.TextSingleLine)
                        || cCtlType == common.getEnumStrVal(eFieldType.Numeric)
                        || cCtlType == common.getEnumStrVal(eFieldType.Formula)
                        || cCtlType == common.getEnumStrVal(eFieldType.TextMultiLine))
                    {
                        HtmlTextArea txtMulti1 = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        LinkButton lnkTextFormat = (LinkButton)e.Row.FindControl("lnkTextFormat");
                        lnkTextFormat.Visible = false;
                        txtW.Visible = false;

                        //mmb
                        if (cCtlType == common.getEnumStrVal(eFieldType.TextMultiLine))
                        {
                            btnHelp.Visible = true;
                        }
                        else
                        {
                            btnHelp.Visible = false;
                        }

                        txtMulti1.Visible = false;
                    }
                    HtmlTextArea txt = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                    txt.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txt.ClientID + "');");
                    txt.Attributes.Add("onkeydown", "javascript:return AutoChange('" + txt.ClientID + "');");

                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Santosh

    protected void btnCheck_Onclick(object sender, EventArgs e)
    {
        int row = common.myInt(hdnSelCell.Text);
        RadEditor rad1 = (RadEditor)gvSelectedServices.Rows[row].FindControl("txtW");
        rad1.Content += common.myStr(Session["rtfText"]);
    }

    //Button Refresh Click Event

    protected void btnRefresh_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (ddlSearch.SelectedValue == "LN")
            {
                lblMessage.Text = "No Data Found.";
                if (!string.IsNullOrEmpty(txtLabNo.Text))
                {
                    int LabNo;
                    int.TryParse(txtLabNo.Text, out LabNo);
                    if ((LabNo > 2147483647 || LabNo.Equals(0)))
                    {
                        lblMessage.Text = "Invalid Lab No";
                        return;
                    }
                }
            }
            if (ddlSearch.SelectedValue == "MLN" && string.IsNullOrEmpty(txtSearchCretria.Text))
            {
                lblMessage.Text = "No Data Found.";
                return;
            }
            refreshData();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }

    protected void refreshData()
    {
        DataTable dt = new DataTable();
        BaseC.clsLISPhlebotomy lis = new clsLISPhlebotomy(sConString);
        try
        {
            ddlResultId.SelectedValue = "1";
            ViewState["collLabTemplate"] = null;
            if (ddlSearch.SelectedValue == "MLN")
            {
                ddlLabNo.Text = "";
                ddlLabNo.Visible = true;
                dt = lis.GetLabNoByManualLabNo(common.myInt(Session["HospitalLocationId"]),
                       common.myInt(Session["FacilityId"]), common.myStr(txtSearchCretria.Text), common.myStr(rblSource.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    ViewState["SelectedLabNo"] = ddlLabNo.SelectedValue;
                    ddlLabNo.DataSource = dt;
                    ddlLabNo.DataTextField = "LabNo";
                    ddlLabNo.DataValueField = "LabNo";
                    ddlLabNo.DataBind();
                    lblTotalManualLabNo.Text = "<b>(Total Orders: </b>" + common.myStr(dt.Rows.Count) + ")";

                    if (common.myStr(txtSearchCretria.Text) == common.myStr(ViewState["PSearchCretria"]))
                    {
                        ddlLabNo.SelectedValue = common.myStr(ViewState["SelectedLabNo"]);
                    }
                    else
                    {
                        ViewState["PSearchCretria"] = null;
                        ddlLabNo.SelectedIndex = 0;
                    }
                    ViewState["PSearchCretria"] = common.myStr(txtSearchCretria.Text);
                }
                ddlLabNo_OnSelectedIndexChanged(this, null);
            }
            else
            {
                getInformation();
                ddlLabNo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            dt.Dispose();
            lis = null;
        }
    }

    //in case of Multiple Result Allowed this Event Get or Set the Next result Template
    protected void ddlResultId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        DataSet ds = new DataSet();
        ds = GetData();
        if (ds.Tables.Count > 2)
        {
            objDs = new DataSet();
            objDs = ds;
            collLabTemplate = new List<clsLabTemplate>();
            objLabTemplate = new clsLabTemplate();
            objLabTemplate.ServiceId = common.myInt(ViewState["SERVICEID"]);
            objLabTemplate.DiagSampleId = common.myInt(ViewState["DIAG_SAMPLEID"]);
            objLabTemplate.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objLabTemplate.ResultRemarksId = common.myInt(ViewState["RESULT_REMARKSID"]);
            objLabTemplate.ReviewRemark = common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
            objLabTemplate.dsField = new DataSet();
            objLabTemplate.dsField = ds;
            objLabTemplate.SampleTypeId = common.myInt(ViewState["SampleTypeId"]);
            collLabTemplate.Add(objLabTemplate);
            ddlRemarks.SelectedIndex = ddlRemarks.Items.IndexOf(ddlRemarks.Items.FindItemByValue(common.myStr(ViewState["RESULT_REMARKSID"])));//rafat               

            if (!string.IsNullOrEmpty(common.myStr(ViewState["RESULT_REVIEWREMARKS"])))
                lblOldReviewRemark.Text = "<b>Review Remark: </b>" + common.myStr(ViewState["RESULT_REVIEWREMARKS"]);
            else
                lblOldReviewRemark.Text = string.Empty;
            if (common.myInt(ViewState["HorizontalView"]) == 1)
            {
                DataView dv = objDs.Tables[0].DefaultView;
                dv.RowFilter = "FieldType <> 'H'";
                dv.Sort = "SequenceNo Asc";
                menuTemplate.DataSource = dv;
                menuTemplate.DataTextField = "FieldName";
                menuTemplate.DataValueField = "FieldId";
                menuTemplate.DataBind();
                gvSelectedServices.DataSource = GetHorizontalFormatData(common.myInt(objDs.Tables[0].Rows[0]["FieldId"]));
                gvSelectedServices.DataBind();
            }
            else
            {
                try
                {
                    DataView dv = objDs.Tables[0].DefaultView;
                    dv.Sort = "SequenceNo Asc";
                    gvSelectedServices.DataSource = dv;
                    gvSelectedServices.DataBind();
                }
                catch { }
            }
        }
    }

    //Save Button Click Event
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSave())
            {
                return;
            }
            //IsValidPassword();

            if (common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y"))
            {
                ViewState["SaveFor"] = "1";
                IsValidPassword();
            }
            else
            {
                ViewState["IsSaveClk"] = "1";

                SaveData();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    public void SaveData()
    {
        int SelectedID = tvCategory.Nodes.IndexOf(tvCategory.SelectedNode);
        int LabNo = 0;
        if (ddlSearch.SelectedValue == "MLN")
        {
            LabNo = common.myInt(ddlLabNo.SelectedValue);
        }
        else { LabNo = common.myInt(txtLabNo.Text.Trim()); }
        if (common.myBool(hdnIsAddendumReleased.Value))
        {
            lblMessage.Text = "Addendum Result Released Can not save data !";
        }
        ViewState["ClearFields"] = null;
        BaseC.Patient bC = new BaseC.Patient(sConString);
        string ReviewRemark = string.Empty;
        if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
            ReviewRemark = txtReviewRemark.Text;

        //Adding current Dispalyed Data to the Global List
        holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                        common.myInt(ViewState["RegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));

        if (collLabTemplate == null || collLabTemplate.Count == 0)
        {
            lblMessage.Text = "Data Not Avalible !";
            return;
        }
        bool isSuccessSave = false;
        string strMsg = "";
        ArrayList xmlServiceId = new ArrayList();
        ArrayList xmlServiceDesc = new ArrayList();
        StringBuilder strServiceId = new StringBuilder();
        StringBuilder strServiceDesc = new StringBuilder();
        //Getting the Data from the Global List
        collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
        //if Result Alert is set true from the Service Tagging Option
        foreach (clsLabTemplate LT in collLabTemplate)
        {
            //If Service result Alert is set true
            if (LT.ResultAlert == 1)
            {
                xmlServiceId.Add(LT.ServiceId);
                DataRow[] drstage = LT.dsField.Tables[VALUES].Select("ResultId=" + common.myInt(ddlResultId.SelectedValue));
                foreach (DataRow item2 in drstage)
                {
                    xmlServiceDesc.Add(LT.ServiceId);
                    xmlServiceDesc.Add(common.myInt(item2["FieldId"]));
                    xmlServiceDesc.Add(item2["FieldValue"]);
                    xmlServiceDesc.Add(common.myInt(LT.DiagSampleId)); // add by manojchauhan
                    xmlServiceDesc.Add(LabNo); // add by manojchauhan
                    xmlServiceDesc.Add(common.myInt(LT.RegistrationId)); // add by manojchauhan
                    xmlServiceDesc.Add(common.myInt(Session["FacilityId"])); // add by manojchauhan

                    strServiceDesc.Append(common.setXmlTable(ref xmlServiceDesc));
                }
                strServiceId.Append(common.setXmlTable(ref xmlServiceId));
            }
        }
        //Calling Procedure To Calcluate Value for result Alert
        if (strServiceId.ToString().Length > 0)
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.GetCalculateResultAlert(strServiceDesc.ToString(), strServiceId.ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Showing the Popup With result Alert Value
                    Session["CheckDt"] = ds.Tables[0];
                    rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/CalculationCheckShow.aspx";
                    rwPrintLabReport.Height = 250;
                    rwPrintLabReport.Width = 600;
                    rwPrintLabReport.Top = 45;
                    rwPrintLabReport.Left = 10;
                    rwPrintLabReport.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                    rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    rwPrintLabReport.Modal = true;
                    rwPrintLabReport.VisibleStatusbar = false;
                    return;
                }
            }
        }

        //Getting the Data From the Global List
        foreach (clsLabTemplate LT in collLabTemplate)
        {
            //Checking Whether Result Is Entered Or Changed 
            //This Flag is Maintained in Hold Previous Data
            if (common.myInt(ViewState["HorizontalView"]) == 0)
            {
                if (!LT.isResultEntered)
                {
                    continue;
                }
            }

            string fieldValue = "";
            string ValueId = "";
            string FindingRemarks = "";

            StringBuilder strTabular = new StringBuilder();
            StringBuilder strNonTabular = new StringBuilder();
            ArrayList coll = new ArrayList();
            ArrayList col2 = new ArrayList();

            #region nonTabular data
            DataTable dtOrganism = CreateTableOrganism();
            dtOrganism.Rows.Clear();

            DataRow[] drstage = LT.dsField.Tables[VALUES].Select("ResultId=" + common.myInt(ddlResultId.SelectedValue));

            //Checking is Result Is Finalized or not if Result iS finalized it will not be added in the xml
            //if (LT.StatusCode != "RF")
            //{
            foreach (DataRow item2 in drstage)
            {
                DataRow[] DRItem0 = LT.dsField.Tables[FIELDS].Select("FieldId=" + common.myInt(item2["FieldId"]));// + " and ResultId=" + common.myInt(ddlResultId.SelectedValue));
                if (DRItem0.Length == 0)
                {
                    break;
                }
                string fieldType = common.myStr(DRItem0[0]["FieldType"]);
                int fieldId = common.myInt(DRItem0[0]["FieldId"]);

                double? MinValue = null, MaxValue = null, MinPanicValue = 0, MaxPanicValue = 0;
                string Symbol = "";
                int MachineReferneceRangeId = 0;
                //Checking Whether Limit Varies Machine wise or not
                if (common.myBool(DRItem0[0]["LimitMachineWise"]) == true)
                {
                    MachineReferneceRangeId = common.myInt(item2["RefRangeMachineId"]);
                    DataView dv = LT.dsField.Tables[3].DefaultView;
                    dv.RowFilter = "MachineId=" + MachineReferneceRangeId + "AND fieldId=" + fieldId;
                    if (dv.Count > 0)
                    {
                        if (common.myStr(dv[0]["MinValue"]) != "")
                        {
                            MinValue = common.myDbl(dv[0]["MinValue"]);
                        }
                        if (common.myStr(dv[0]["MaxValue"]) != "")
                        {
                            MaxValue = common.myDbl(dv[0]["MaxValue"]);
                        }

                        MinValue = MinValue;
                        MaxValue = MaxValue;
                        MinPanicValue = common.myDbl(dv[0]["MinPanicValue"]);
                        MaxPanicValue = common.myDbl(dv[0]["MaxPanicValue"]);
                        Symbol = common.myStr(dv[0]["Symbol"]);

                    }
                }
                else
                {
                    if (common.myStr(DRItem0[0]["MinValue"]) != "")
                    {
                        MinValue = common.myDbl(DRItem0[0]["MinValue"]);
                    }

                    if (common.myStr(DRItem0[0]["MaxValue"]) != "")
                    {
                        MaxValue = common.myDbl(DRItem0[0]["MaxValue"]);
                    }
                    MinValue = MinValue;
                    MaxValue = MaxValue;
                    MinPanicValue = common.myDbl(DRItem0[0]["MinPanicValue"]);
                    MaxPanicValue = common.myDbl(DRItem0[0]["MaxPanicValue"]);
                    Symbol = common.myStr(DRItem0[0]["Symbol"]);
                }
                //Xml Sequence In which data have to send
                //ServiceId, FieldId, FieldType, FieldValue, ValueId, UnitId,  MinValue, MaxValue, Symbol, 
                //MinPanicValue, MaxPanicValue, FindingRemarks, SequenceNo,  RefRangeMachineId


                //Block to add single line textbox result to xml
                if (fieldType == common.getEnumStrVal(eFieldType.TextSingleLine))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0);
                        if (common.myInt(DRItem0[0]["UnitId"]) > 0)
                        {
                            coll.Add(common.myInt(DRItem0[0]["UnitId"])); //UnitId
                        }
                        else
                        {
                            coll.Add(DBNull.Value); //UnitId
                        }
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));

                    }
                }
                //Block to Numeric textbox result to xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Numeric))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    //mmb
                    //if (fieldValue != "" && common.myDbl(fieldValue) >= 0)
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0); //RowNo
                        if (common.myInt(DRItem0[0]["UnitId"]) > 0)
                        {
                            coll.Add(common.myInt(DRItem0[0]["UnitId"])); //UnitId
                        }
                        else
                        {
                            coll.Add(DBNull.Value); //UnitId
                        }
                        coll.Add(MinValue);//MinValue
                        coll.Add(MaxValue);//MaxValue
                        coll.Add(Symbol);//Symbol
                        coll.Add(MinPanicValue);//MinPanicValue
                        coll.Add(MaxPanicValue);//MaxPanicValue
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to Formula textbox result to xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Formula))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    // if (common.myLen(fieldValue) > 0)
                    if (fieldValue != "" && common.myDbl(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0); //RowNo                          
                        if (common.myInt(DRItem0[0]["UnitId"]) > 0)
                        {
                            coll.Add(common.myInt(DRItem0[0]["UnitId"])); //UnitId
                        }
                        else
                        {
                            coll.Add(DBNull.Value); //UnitId
                        }
                        coll.Add(common.myDec(DRItem0[0]["MinValue"]));//MinValue
                        coll.Add(common.myDec(DRItem0[0]["MaxValue"]));//MaxValue
                        coll.Add(common.myStr(DRItem0[0]["Symbol"]));//Symbol
                        coll.Add(common.myDec(DRItem0[0]["MinPanicValue"]));//MinPanicValue
                        coll.Add(common.myDec(DRItem0[0]["MaxPanicValue"]));//MaxPanicValue
                        coll.Add(0);
                        coll.Add(0);
                        // coll.Add("");
                        coll.Add(common.myStr(item2["FindingRemarks"]));//FindingRemarks
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to Multiline textbox result to xml
                else if (fieldType == common.getEnumStrVal(eFieldType.TextMultiLine))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to TextFormat textbox field result to xml
                else if (fieldType == common.getEnumStrVal(eFieldType.TextFormats))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        fieldValue = fieldValue.Replace("style=\"border: 1px solid black;\"", "border=\"1\"");//html
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else
                    {
                        if (common.myLen(fieldValue) > 0)
                        {
                            fieldValue = fieldValue.Replace("style=\"border: 1px solid black;\"", "border=\"1\"");//html
                            coll.Add(LT.ServiceId);
                            coll.Add(fieldId);
                            coll.Add(fieldType);
                            coll.Add(fieldValue.Trim());
                            coll.Add(0);
                            coll.Add(0);
                            coll.Add(DBNull.Value);
                            coll.Add(0.0);
                            coll.Add(0.0);
                            coll.Add("");
                            coll.Add(0.0);
                            coll.Add(0.0);
                            coll.Add(0);
                            coll.Add(0);
                            coll.Add("");
                            coll.Add(DRItem0[0]["SequenceNo"]);
                            coll.Add(MachineReferneceRangeId);
                            coll.Add(LT.SampleTypeId);
                            strNonTabular.Append(common.setXmlTable(ref coll));

                        }
                    }
                }
                //Block to add Dropdown Field to the xml
                else if (fieldType == common.getEnumStrVal(eFieldType.ListofChoices))
                {
                    ValueId = common.myStr(item2["fieldValue"]);
                    fieldValue = common.myStr(item2["TextValue"]);
                    FindingRemarks = common.myStr(item2["FindingRemarks"]).Trim();
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(ValueId);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(FindingRemarks);
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add heading type Field To xml
                if (fieldType == common.getEnumStrVal(eFieldType.Heading))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue.Trim());
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(common.myDec(DRItem0[0]["MinValue"]));//MinValue
                        coll.Add(common.myDec(DRItem0[0]["MaxValue"]));//MaxValue
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add Boolean type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Boolean))
                {
                    ValueId = common.myStr(item2["fieldValue"]);
                    fieldValue = common.myStr(item2["TextValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(ValueId);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add Checkbox type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.CheckBox))
                {
                    ValueId = common.myStr(item2["fieldValue"]);
                    fieldValue = common.myStr(item2["TextValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(ValueId);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add date type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Date))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add Organism type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Organism))
                {
                    ValueId = common.myStr(item2["fieldValue"]);
                    fieldValue = common.myStr(item2["TextValue"]);
                    if (fieldValue != "")
                    {
                        DataRow dr;
                        dr = dtOrganism.NewRow();
                        dr["FieldValue"] = ValueId;
                        dr["TextValue"] = fieldValue;
                        dtOrganism.Rows.Add(dr);

                        DataRow[] duplicateOrganismRow = dtOrganism.Select("FieldValue='" + ValueId + "'");
                        if (duplicateOrganismRow.Length > 1)
                        {
                            lblMessage.Text = "Duplicate Organism(s) cannot be Saved!";
                            return;
                        }
                        if (common.myLen(fieldValue) > 0)
                        {
                            coll.Add(LT.ServiceId);
                            coll.Add(fieldId);
                            coll.Add(fieldType);
                            coll.Add(fieldValue);
                            coll.Add(ValueId);
                            coll.Add(0);
                            coll.Add(DBNull.Value);
                            coll.Add(0.0);
                            coll.Add(0.0);
                            coll.Add("");
                            coll.Add(0.0);
                            coll.Add(0.0);
                            coll.Add(0);
                            coll.Add(0);
                            coll.Add(common.myStr(item2["FindingRemarks"]));
                            coll.Add(DRItem0[0]["SequenceNo"]);
                            coll.Add(MachineReferneceRangeId);
                            coll.Add(LT.SampleTypeId);
                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                }
                //Block to add Enzyme type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Enzyme))
                {
                    ValueId = common.myStr(item2["fieldValue"]);
                    fieldValue = common.myStr(item2["TextValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(ValueId);
                        coll.Add(0);
                        coll.Add(DBNull.Value);
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add("");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add("");
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
                //Block to add Sensitivity type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Sensitivity))
                {
                    coll.Add(LT.ServiceId);
                    coll.Add(fieldId);
                    coll.Add(fieldType);
                    coll.Add(0);
                    coll.Add(0);
                    coll.Add(0);
                    coll.Add(DBNull.Value);
                    coll.Add(0.0);
                    coll.Add(0.0);
                    coll.Add("");
                    coll.Add(0.0);
                    coll.Add(0.0);
                    coll.Add(0);
                    coll.Add(0);
                    coll.Add("");
                    coll.Add(DRItem0[0]["SequenceNo"]);
                    coll.Add(MachineReferneceRangeId);
                    coll.Add(LT.SampleTypeId);
                    strNonTabular.Append(common.setXmlTable(ref coll));
                    ViewState["ClearFields"] = false;
                }
                //Block to add time type Field To xml
                else if (fieldType == common.getEnumStrVal(eFieldType.Time))
                {
                    fieldValue = common.myStr(item2["fieldValue"]);
                    if (common.myLen(fieldValue) > 0)
                    {
                        string findingRemarks = "";
                        if (common.myStr(item2["FindingRemarks"]) != "")
                        {
                            findingRemarks = common.myStr(item2["FindingRemarks"]).Replace("(", "");
                            findingRemarks = findingRemarks.Replace(")", "");
                        }
                        coll.Add(LT.ServiceId);
                        coll.Add(fieldId);
                        coll.Add(fieldType);
                        coll.Add(fieldValue);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(common.myInt(DRItem0[0]["UnitId"]));
                        coll.Add(common.myDec(DRItem0[0]["MinValue"]));//MinValue
                        coll.Add(common.myDec(DRItem0[0]["MaxValue"]));//MaxValue
                        coll.Add("-");
                        coll.Add(0.0);
                        coll.Add(0.0);
                        coll.Add(0);
                        coll.Add(0);
                        coll.Add(findingRemarks);//FindingRemarks
                        coll.Add(DRItem0[0]["SequenceNo"]);
                        coll.Add(MachineReferneceRangeId);
                        coll.Add(LT.SampleTypeId);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            //calling procedure to Save Result
            objval = new BaseC.clsLISPhlebotomy(sConString);
            string sReleaseStage = "";
            int iResultFromMachine = 0, iMachineId = 0, iResultAssignedTo = 0, iUserId = common.myInt(Session["UserId"]);
            if (common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y"))
            {
                iUserId = common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y") ? common.myInt(Session["SaveUserId"]) : common.myInt(Session["UserId"]);
            }
            bool bMultiStageResultFinalized = common.myInt(ddlMultiStageResultFinalized.SelectedValue) == 1 ? true : false;
            strMsg = objval.saveInvResult(ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue), common.myInt(txtLabNo.Text),
                        common.myInt(ddlResultId.SelectedValue), common.myInt(Session["StationId"]), common.myInt(LT.DiagSampleId), strNonTabular.ToString(), iResultFromMachine,
                        iMachineId, sReleaseStage, iResultAssignedTo, common.myInt(LT.ResultRemarksId), iUserId, "", LT.ReviewRemark, bMultiStageResultFinalized);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                txtReviewRemark.Text = string.Empty;
                RefershTV(SelectedID);
                refreshData();
                RefershTV(SelectedID);
                isSuccessSave = true;
            }
            if (common.myStr(lblMessage.Text).Equals("Mandatory clinical details are missing. Unable to Save Result."))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
            }
            else if (common.myStr(strMsg).Contains("Scan Time details not found"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
            }
            else
            {
                lblMessage.Text = strMsg;
            }
            //}
            #endregion nonTabular data
        }
        if (isSuccessSave)
        {
            if (common.myStr(lblMessage.Text).Equals("Mandatory clinical details are missing. Unable to Save Result."))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
            }
            else
            {
                BindTemplateGrid();
                lblMessage.Text = strMsg;
            }
        }
        txtLabNo.Focus();
        txtLabNo.Enabled = true;
        //if (ViewState["ClearFields"] == null)
        //{
        //    ClearControl();
        //}
        //Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
    }
    private void ClearControl()
    {
        txtLabNo.Text = "";
        gvSelectedServices.DataSource = null;
        gvSelectedServices.DataBind();


        tvCategory.Nodes.Clear();

        tvCategory.DataSource = null;
        tvCategory.DataBind();
        lblRegNo.Text = "";
        lblPatientName.Text = "";
        lblAgeGender.Text = "";
        lblOrderDate.Text = "";
        lblFacilityName.Text = "";
        lblWardBedNo.Text = "";
        hdnTextFormatData.Value = "";
        hdnTotalTabIndexValue.Value = "";
        ViewState["RegistrationId"] = null;
        ViewState["EncounterId"] = null;

        ViewState["OrderDate"] = null;
        ViewState["Gender"] = null;
        ViewState["AgeInDays"] = null;
    }
    //Save data with Valid Passowrd
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                Session["EncodedBy"] = common.myStr(Session["UserId"]);
                lblMessage.Text = "Invalid Password !";
                return;
            }
            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                lblMessage.Text = "";
                ViewState["IsSaveClk"] = "1";

                if (common.myInt(ViewState["SaveFor"]) == 1)
                {
                    SaveData();
                }
                else if (common.myInt(ViewState["SaveFor"]) == 2)
                {
                    SaveProvisionalRelease();
                }
                else if (common.myInt(ViewState["SaveFor"]) == 3)
                {
                    SaveFinalRelease();
                }

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
            //return false;
        }
    }
    //Method to check the password
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/SampleCollection.aspx?Show=ResultEntry&IsEnableUserAuthentication=Y";
        RadWindowForNew.Height = 320;
        RadWindowForNew.Width = 500;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    //Print Button Click Event
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (common.myInt(txtLabNo.Text) == 0)
        {
            lblMessage.Text = "Please Enter " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO"));
            return;
        }

        string str = common.myStr(rblSource.SelectedValue);
        if (common.myStr(ViewState["SelectedSource"]) != "")
            str = common.myStr(ViewState["SelectedSource"]);

        if (!common.myBool(ViewState["ResultHTML"]))
        {
            rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + str +
                                            "&LABNO=" + common.myInt(txtLabNo.Text) +
                                            "&StationId=" + common.myInt(ViewState["STATION_ID"]) +
                                            "&Flag=" + common.myStr(ViewState["MDFlag"]) +
                                            "&ServiceIds=" + common.myStr(ViewState["SERVICEID"]) +
                                            "&DiagSampleId=" + common.myInt(ViewState["DIAG_SAMPLEID"]);

            rwPrintLabReport.Height = 550;
            rwPrintLabReport.Width = 800;
            rwPrintLabReport.Top = 45;
            rwPrintLabReport.Left = 10;
            rwPrintLabReport.InitialBehaviors = WindowBehaviors.Maximize;
            rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            rwPrintLabReport.Modal = true;
            rwPrintLabReport.VisibleStatusbar = false;
        }
        else
        {
            StringBuilder objXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            if (tvCategory.Nodes.Count > 0)
            {
                string[] values = tvCategory.SelectedNode.Value.Split('/');
                int iSampleId = common.myInt(values[1]);
                coll.Add(common.myInt(values[1]));//SampleID INT
                coll.Add(str);//Source VARCHAR
                objXML.Append(common.setXmlTable(ref coll));

                if (ViewState["EncounterId"] != null && ViewState["RegistrationId"] != null)
                {
                    rwPrintLabReport.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + str +
                                        "&LABNO=" + common.myInt(txtLabNo.Text) + "&ServiceIds=" + common.myStr(ViewState["SERVICEID"]) +
                                        "&StationId=" + common.myInt(ViewState["STATION_ID"]) + "&Flag=" + common.myStr(ViewState["MDFlag"]) + "&RegId=" + ViewState["RegistrationId"] +
                                        "&EncId=" + ViewState["EncId"] + "&DiagSampleId=" + objXML.ToString() + "&iSampleId=" + common.myInt(iSampleId) + "&ShowReference=" + common.myBool(ViewState["PrintReferenceRangeInHTML"]);

                    rwPrintLabReport.Height = 550;
                    rwPrintLabReport.Width = 800;
                    rwPrintLabReport.Top = 45;
                    rwPrintLabReport.Left = 10;
                    rwPrintLabReport.InitialBehaviors = WindowBehaviors.Maximize;
                    rwPrintLabReport.VisibleOnPageLoad = true;
                    rwPrintLabReport.Modal = true;
                    rwPrintLabReport.VisibleStatusbar = false;
                }
            }
        }
        Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
    }
    //Add or edit comment button click event
    protected void btnAddComments_Click(object sender, EventArgs e)
    {

        if (common.myInt(txtLabNo.Text) == 0)
        {
            lblMessage.Text = "Please Enter " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO"));
            return;
        }
        string str = common.myStr(rblSource.SelectedValue);
        if (common.myStr(ViewState["SelectedSource"]) != "")
            str = common.myStr(ViewState["SelectedSource"]);

        rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/Comments.aspx?Source=" + str + "&LabNo=" + common.myInt(txtLabNo.Text) + "&RegNo=" + common.myStr(lblRegNo.Text) + "&PName=" + common.myStr(lblPatientName.Text);
        rwPrintLabReport.Height = 450;
        rwPrintLabReport.Width = 750;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.OnClientClose = "OnClientCloseComments";
        rwPrintLabReport.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
        rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleStatusbar = false;
    }
    //Add or edit comment button click event
    protected void btnAddFootNote_Click(object sender, EventArgs e)
    {

        if (common.myInt(txtLabNo.Text) == 0)
        {
            lblMessage.Text = "Please Enter " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO"));
            return;
        }
        string str = common.myStr(rblSource.SelectedValue);
        if (common.myStr(ViewState["SelectedSource"]) != "")
            str = common.myStr(ViewState["SelectedSource"]);

        rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/Comments.aspx?Source=" + str + "&LabNo=" + common.myInt(txtLabNo.Text) + "&RegNo=" + common.myStr(lblRegNo.Text) + "&CF=FNOTE&PName=" + common.myStr(lblPatientName.Text);
        rwPrintLabReport.Height = 450;
        rwPrintLabReport.Width = 750;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.OnClientClose = "OnClientCloseComments";
        rwPrintLabReport.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
        rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleStatusbar = false;
    }

    //Hidden Button called when the comment popup closed through Javascript
    protected void btnCommentsRefresh_Click(object sender, EventArgs e)
    {
        CheckComments();
        Session["CommentRefresh"] = null;
        Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
    }
    //Button New Click Event
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
        Response.Redirect("InvestigationResult.aspx", false);
        getInformation();
    }
    //Link Button Release Click Event
    protected void BtnRelease_OnClick(object sender, EventArgs e)
    {
        string Source = ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue);

        Response.Redirect("/LIS/Phlebotomy/ResultFinalization.aspx?OrderDate=" + common.myStr(ViewState["OrderDate"]) +
                          "&LabNo=" + txtLabNo.Text + "&Source=" + Source + "&MD=" + common.myStr(ViewState["MDFlag"]), false);
    }
    //Link Button Relay Click Event
    protected void lnkRelayDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myInt(txtLabNo.Text) == 0)
            {
                if (common.myStr(ViewState["MDFlag"]) == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            string Source = ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue);

            rwPrintLabReport.NavigateUrl = "~/LIS/Phlebotomy/RelayDetails.aspx?MD=" + common.myStr(ViewState["MDFlag"]) +
                                            "&SOURCE=" + common.myStr(Source) + "&LABNO=" + common.myInt(txtLabNo.Text);
            rwPrintLabReport.Height = 560;
            rwPrintLabReport.Width = 900;
            rwPrintLabReport.Top = 10;
            rwPrintLabReport.Left = 10;
            rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            rwPrintLabReport.Modal = true;
            rwPrintLabReport.VisibleStatusbar = false;
        }
        catch (Exception)
        {
        }
    }
    //Link Button Package Click Event
    protected void lnkPackageDetails_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(txtLabNo.Text) == 0)
        {
            if (common.myStr(ViewState["MDFlag"]) == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }
        lblMessage.Text = "";
        string Source = ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue);
        string DefaultFacilityName = "";
        int FacilityId = common.myInt(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DefaultFacility", sConString));

        if (Cache["FACILITY"] == null)
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            Cache["FACILITY"] = ds;
        }
        DataSet dsFacility = (DataSet)Cache["FACILITY"];
        dsFacility.Tables[0].DefaultView.RowFilter = "";
        dsFacility.Tables[0].DefaultView.RowFilter = "FacilityId=" + FacilityId;
        if (dsFacility.Tables[0].DefaultView.Count > 0)
        {
            DefaultFacilityName = common.myStr(dsFacility.Tables[0].DefaultView[0]["FacilityName"]);
        }
        rwPrintLabReport.NavigateUrl = "~/LIS/Phlebotomy/PackageDetails.aspx?MD=" + common.myStr(ViewState["MDFlag"]) +
                                        "&SOURCE=" + common.myStr(Source) +
                                        "&EncounterNo=" + common.myStr(ViewState["EncounterId"]) +
                                        "&RegNo=" + common.myStr(ViewState["RegistrationId"]) +
                                        "&PName=" + common.myStr(lblPatientName.Text).Trim() +
                                        "&LABNO=" + common.myInt(txtLabNo.Text) +
                                        "&FacilityName=" + common.myStr(DefaultFacilityName);

        rwPrintLabReport.Height = 620;
        rwPrintLabReport.Width = 900;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleStatusbar = false;

    }
    protected void lnkPatientResultHistory_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(txtLabNo.Text) == 0)
        {
            if (common.myStr(ViewState["MDFlag"]) == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }
        string Source = ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue);


        rwPrintLabReport.NavigateUrl = "~/LIS/Phlebotomy/PatientHistory.aspx?Mpg=P607&MD=" + common.myStr(ViewState["MDFlag"]) + "&SOURCE="
                  + common.myStr(Source)
                  + "&LABNO=" + common.myStr(lblLabNo.Text).Trim()
                  + "&RegId=" + common.myInt(ViewState["RegistrationId"])
                  + "&ServiceId=" + common.myInt(ViewState["SERVICEID"])
                  + "&ServiceName=" + common.myStr(tvCategory.SelectedNode.ToolTip)
                  + "&PName=" + common.myStr(lblPatientName.Text).Trim()
                  + "&EncounterNo=" + common.myStr(ViewState["EncounterId"])
                  + "&RegNo=" + common.myStr(lblRegNo.Text).Trim()
                  + "&PageID=SAck&POPUP=StaticTemplate"
                  + "&StationId=" + common.myStr(Session["StationId"]);


        //rwPrintLabReport.NavigateUrl = "~/LIS/Phlebotomy/PatientResultHistory.aspx?MD=" + common.myStr(ViewState["MDFlag"]) +
        //                                "&SOURCE=" + common.myStr(Source) +
        //                                "&LABNO=" + common.myInt(txtLabNo.Text) +
        //                                "&RegId=" + common.myInt(ViewState["RegistrationId"]) +
        //                                "&ServiceId=" + ViewState["SERVICEID"] +
        //                                "&ServiceName=" + common.myStr(tvCategory.SelectedNode.ToolTip) +
        //                                "&PName=" + common.myStr(lblPatientName.Text).Trim() +
        //                                "&EncounterNo=" + common.myStr(ViewState["EncounterId"]);

        rwPrintLabReport.Height = 620;
        rwPrintLabReport.Width = 900;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleStatusbar = false;
        rwPrintLabReport.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void btnResultFinalization_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            int DiagSampleId = common.myInt(Request.QueryString["SEL_DiagSampleID"]);
            if (DiagSampleId > 0)
            {
                coll.Add(DiagSampleId);
                strXML.Append(common.setXmlTable(ref coll));
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }

            BaseC.User valUser = new BaseC.User(sConString);
            int EmployeeId = common.myInt(valUser.getEmployeeId(common.myInt(Session["UserID"])));

            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.SaveResultFinalization(common.myStr(Request.QueryString["SOURCE"]), strXML.ToString(),
                                  common.myInt(Session["UserId"]), common.myStr("F"),
                                  EmployeeId, common.myInt(Session["StationId"]),
                                  0, 0, EmployeeId);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                refreshData();

                btnResultFinalization.Visible = false;
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    #endregion

    protected void SaveProvisionalRelease()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        if (common.myInt(ViewState["DIAG_SAMPLEID"]) != 0)
        {
            coll.Add(common.myInt(ViewState["DIAG_SAMPLEID"]));
            coll.Add(common.myStr(ViewState["SelectedSource"]));
            strXML.Append(common.setXmlTable(ref coll));
        }

        if (strXML.Length == 0)
        {
            lblMessage.Text = "Please Select Sample !";
            return;
        }

        BaseC.User valUser = new BaseC.User(sConString);
        int EmployeeId = common.myInt(valUser.getEmployeeId(common.myInt(Session["UserID"])));
        int iEncodedUserId = common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y") ? common.myInt(Session["SaveUserId"]) : common.myInt(Session["UserId"]);

        objval = new BaseC.clsLISPhlebotomy(sConString);
        string strMsg = objval.SaveResultFinalization(common.myStr(rblSource.SelectedValue), strXML.ToString(), iEncodedUserId, common.myStr("P"),
                              EmployeeId, common.myInt(Session["StationId"]), 0, 0, EmployeeId);

        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            refreshData();

            btnResultFinalization.Visible = false;
        }
        lblMessage.Text = strMsg;
    }
    protected void btnEmployee_Click(object sender, EventArgs e)
    {
        ViewState["LeftDoctorId"] = hdnLeftDoctor.Value;
        ViewState["RightDoctorId"] = hdnRightDoctor.Value;
        ViewState["CenterDoctorId"] = hdnCenterDoctor.Value;
        if ((common.myInt(hdnLeftDoctor.Value) != 0) || (common.myInt(hdnRightDoctor.Value) != 0) || (common.myInt(hdnCenterDoctor.Value) != 0))
        {
            SaveFinalRelease();
        }
    }
    protected void lnkProvisionalRelease_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y"))
            {
                ViewState["SaveFor"] = "2";
                IsValidPassword();
            }
            else
            {
                ViewState["SaveFor"] = "2";
                SaveProvisionalRelease();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void SaveFinalRelease()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();

        if (common.myInt(ViewState["DIAG_SAMPLEID"]) != 0)
        {
            coll.Add(common.myInt(ViewState["DIAG_SAMPLEID"]));
            coll.Add(common.myStr(ViewState["SelectedSource"]));
            strXML.Append(common.setXmlTable(ref coll));
        }

        if (strXML.Length == 0)
        {
            lblMessage.Text = "Please Select Sample !";
            return;
        }

        BaseC.User valUser = new BaseC.User(sConString);
        int EmployeeId = 0, iLeftDoctor = 0, iCenterDoctor = 0, iRightDoctor = 0;
        EmployeeId = common.myInt(valUser.getEmployeeId(common.myInt(Session["UserId"])));

        if (common.myBool(Session["multipledoctor"]) == true)
        {

            iLeftDoctor = common.myInt(ViewState["LeftDoctorId"]);
            iCenterDoctor = common.myInt(ViewState["CenterDoctorId"]);
            iRightDoctor = common.myInt(ViewState["RightDoctorId"]);
        }
        int iEncodedUserId = common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y") ? common.myInt(Session["SaveUserId"]) : common.myInt(Session["UserId"]);

        objval = new BaseC.clsLISPhlebotomy(sConString);
        string strMsg = objval.SaveResultFinalization(common.myStr(rblSource.SelectedValue), strXML.ToString(),
                              iEncodedUserId, common.myStr("F"), EmployeeId, common.myInt(Session["StationId"]),
                              iCenterDoctor, iRightDoctor, iLeftDoctor);

        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            refreshData();

            btnResultFinalization.Visible = false;
            ViewState["LeftDoctorId"] = "";
            ViewState["RightDoctorId"] = "";
            ViewState["CenterDoctorId"] = "";
        }
        lblMessage.Text = strMsg;
    }

    protected void lnkFinalRelease_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["SaveFor"] = "3";
            if (common.myStr(ViewState["IsEnableUserAuthentication"]).Equals("Y"))
            {
                IsValidPassword();
            }
            else
            {
                if (common.myBool(Session["multipledoctor"]) == true)
                {
                    Session["EncodedBy"] = common.myStr(Session["UserId"]);
                    RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/EmployeeSignatureSetup.aspx";
                    RadWindowPopup.Height = 200;
                    RadWindowPopup.Width = 480;
                    RadWindowPopup.Top = 50;
                    RadWindowPopup.Left = 100;
                    RadWindowPopup.Modal = true;
                    RadWindowPopup.VisibleOnPageLoad = true;
                    RadWindowPopup.VisibleStatusbar = false;
                    RadWindowPopup.Behaviors = WindowBehaviors.Move | WindowBehaviors.Close;
                    RadWindowPopup.OnClientClose = "OnClientDoctorSaveClose";
                }
                else
                {
                    SaveFinalRelease();
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

    protected void lnkAddendumRelease_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            if (common.myInt(ViewState["DIAG_SAMPLEID"]) != 0)
            {
                coll.Add(common.myInt(ViewState["DIAG_SAMPLEID"]));
                coll.Add(common.myStr(ViewState["SelectedSource"]));
                strXML.Append(common.setXmlTable(ref coll));
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }

            BaseC.User valUser = new BaseC.User(sConString);
            int EmployeeId = common.myInt(valUser.getEmployeeId(common.myInt(Session["UserID"])));

            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.UpdateResultAddendum(common.myStr(ViewState["SelectedSource"]), strXML.ToString(),
                                  0, 0, EmployeeId, common.myInt(Session["StationId"]), common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                refreshData();

                btnResultFinalization.Visible = false;
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    //done by ujjwal 01 july 2015 introduced one new button for addendum report start
    protected string getAddendum()
    {
        try
        {

            objval = new BaseC.clsLISPhlebotomy(sConString);
            return objval.GetAddendum(common.myStr(ViewState["SelectedSource"]), common.myInt(ViewState["DIAG_SAMPLEID"]));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
            return "ERROR";
        }
    }
    //Done by ujjwal 01 july 2015 introduced one new button for addendum report start
    //Region for the events inside the format grid
    #region Events for Inside Result Grid controls
    //Dropdown Used in Mutltiline format type and this event is get the format description and show it in textbox
    protected void ddlMultilineFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            TextBox txtM = (TextBox)row.FindControl("txtM");
            string fieldId = common.myStr(row.Cells[(int)eServicesGrid.FieldID].Text);
            string ReviewRemark = string.Empty;
            if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
            {
                ReviewRemark = txtReviewRemark.Text;
            }

            holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                     common.myInt(ViewState["RegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));

            if (collLabTemplate == null)
            {
                BindTemplateGrid();
            }

            if (collLabTemplate != null)
            {
                foreach (clsLabTemplate LT in collLabTemplate)
                {
                    if (LT.ServiceId == common.myInt(ViewState["SERVICEID"]) && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                    {
                        DataRow[] dr = LT.dsField.Tables[OPTIONS].Select("FieldId=" + fieldId + "and ValueId=" + ddl.SelectedValue + "and ValueType= 'F'");
                        if (dr.Length > 0)
                        {
                            if (common.myInt(ddl.SelectedValue) > 0)
                            {
                                txtM.Text = common.myStr(dr[0]["Remarks"]);
                            }
                            else
                            {
                                txtM.Text = "";
                            }
                            txtM.Focus();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Dropdown Used in text format type and this event is get the format description and show it in textbox
    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");
            string fieldId = common.myStr(row.Cells[(int)eServicesGrid.FieldID].Text);
            string ReviewRemark = string.Empty;
            if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
            {
                ReviewRemark = txtReviewRemark.Text;
            }

            holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                     common.myInt(ViewState["RegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));

            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];

            if (collLabTemplate == null)
            {
                BindTemplateGrid();
            }

            if (collLabTemplate != null)
            {
                foreach (clsLabTemplate LT in collLabTemplate)
                {
                    if (LT.ServiceId == common.myInt(ViewState["SERVICEID"]) && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                    {
                        DataRow[] dr = LT.dsField.Tables[OPTIONS].Select("FieldId=" + fieldId + "and ValueId=" + ddl.SelectedValue + "and ValueType= 'F'");
                        if (dr.Length > 0)
                        {
                            if (common.myInt(ddl.SelectedValue) > 0)
                            {
                                txtW.Content = common.myStr(dr[0]["Remarks"]);
                            }
                            else
                            {
                                txtW.Content = "";
                            }
                        }
                        //else
                        //{
                        //    txtW.Content = "";
                        //}
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Dropdown Used in List of Choices type and this event is get the Range and Show it in Label
    protected void D_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadComboBox ddlD = row.FindControl(common.getEnumStrVal(eFieldType.ListofChoices)) as RadComboBox;
            Label lblD = (Label)row.FindControl("lblD");
            lblD.Visible = true;
            string strRemarks = common.myStr(ddlD.SelectedItem.Attributes["FindingRemarks"]);
            if (strRemarks != "")
            {
                lblD.Text = " (" + strRemarks + ")";
            }
            else
            {
                lblD.Text = "";
            }
            int Count = row.Controls.Count;
            //if (row.RowIndex == Count)
            //{
            //    //int i= tvCategory.Nodes.Count;
            //    //int j =Convert.ToInt32(tvCategory.SelectedNode.Value);
            //}
            //else
            //{
            //   row.Focus();
            //}
            int totalrowcount = gvSelectedServices.Rows.Count;
            if (totalrowcount > 1)
            {
                if (row.RowIndex + 1 < totalrowcount)
                {
                    gvSelectedServices.Rows[row.RowIndex + 1].Focus();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Button used in formula type Field and This click event used fro formula calculation
    protected void btnF_Click(object sender, EventArgs e)
    {
        try
        {
            ArrayList coll = new ArrayList();
            StringBuilder strXml = new StringBuilder();
            int formulaFieldID = 0;
            string fieldType = "";
            int fieldID = 0;
            string fieldText = "";
            string currentBtnID = common.myStr(((Button)sender).ClientID);
            Button btnF = (Button)sender;
            int FieldId = common.myInt(btnF.CommandArgument.ToString());
            GridViewRow formulaRow = (GridViewRow)btnF.NamingContainer;
            //GridViewRow formulaRow = ;
            foreach (GridViewRow item2 in gvSelectedServices.Rows)
            {
                if (item2.RowType == DataControlRowType.DataRow)
                {
                    fieldType = common.myStr(item2.Cells[(int)eServicesGrid.FieldType].Text);
                    fieldID = common.myInt(item2.Cells[(int)eServicesGrid.FieldID].Text);
                    if ((fieldType == common.getEnumStrVal(eFieldType.Numeric)) || (fieldType == common.getEnumStrVal(eFieldType.Formula)))
                    {
                        TextBox txtN = (TextBox)item2.FindControl("txtN");
                        TextBox txtF = (TextBox)item2.FindControl("txtF");

                        if (FieldId != fieldID)
                        {
                            if (common.myLen(txtN.Text) > 0)
                            {
                                coll.Add(fieldID);
                                coll.Add(common.myStr(txtN.Text).Trim());
                                strXml.Append(common.setXmlTable(ref coll));
                            }
                            if (common.myLen(txtF.Text) > 0)
                            {
                                coll.Add(fieldID);
                                coll.Add(common.myStr(txtF.Text).Trim());
                                strXml.Append(common.setXmlTable(ref coll));
                            }
                        }

                    }

                }
            }
            //string ReviewRemark = string.Empty;
            //if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
            //    ReviewRemark = txtReviewRemark.Text;
            holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                          common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["RESULT_REMARKSID"]), common.myStr(ViewState["RESULT_REVIEWREMARKS"]), common.myInt(ViewState["SampleTypeId"]));
            TextBox txtFormula = (TextBox)formulaRow.FindControl("txtF");
            if ((formulaRow != null) && (strXml.ToString() != ""))
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);


                string Result = objval.calcFormula(FieldId, strXml.ToString());
                if ((Result != "0.00000") && (Result != ""))
                {
                    Result = Decimal.Round(Convert.ToDecimal(Result), 2).ToString("#.##");
                }
                else
                {
                    Result = "0.00";
                }
                txtFormula.Text = Result;
            }
            else
            {
                txtFormula.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //Button Sensivity Click event to open Sensivity 
    protected void btnSN_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        int iFieldId = common.myInt(gvSelectedServices.Rows[index].Cells[0].Text);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        objMicrobiology = new BaseC.clsMicrobiology(sConString);
        string source = ViewState["SelectedSource"] != null ? common.myStr(ViewState["SelectedSource"]) : common.myStr(rblSource.SelectedValue);
        int iLabNo = common.myInt(txtLabNo.Text);
        int iResultId = common.myInt(ddlResultId.SelectedValue);
        int iDiagSampleId = common.myInt(ViewState["DIAG_SAMPLEID"]);
        int iServiceId = common.myInt(ViewState["SERVICEID"]);
        string sFieldType = "SN";
        string strOrganismIds = "";
        int iOrganismCount = 1;

        for (int i = 0; i < gvSelectedServices.Rows.Count; i++)
        {
            string fieldType = common.myStr(gvSelectedServices.Rows[i].Cells[(int)eServicesGrid.FieldType].Text);
            if (fieldType == common.getEnumStrVal(eFieldType.Organism))
            {
                RadComboBox ddl = (RadComboBox)gvSelectedServices.Rows[i].Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Organism));
                if (common.myInt(ddl.SelectedIndex) != 0)
                {
                    if (strOrganismIds == "")
                    {
                        strOrganismIds = ddl.SelectedValue;
                    }
                    else
                    {
                        strOrganismIds = strOrganismIds + "," + ddl.SelectedValue;
                    }
                    iOrganismCount = iOrganismCount + 1;
                }
            }
        }
        if (common.myStr(ViewState["StatusCode"]) != "RF")
        {
            if (strOrganismIds == "")
            {
                lblMessage.Text = "Please Select Organism(s)!";
                return;
            }
            bool IsSaved = objMicrobiology.CheckMicrobiologyResult(source, iDiagSampleId, sFieldType, strOrganismIds, iOrganismCount, iResultId);
            if (IsSaved == false)
            {
                lblMessage.Text = "Please Save Before Entering Sensitivity Data!";
                return;
            }
        }
        //string ServiceName = common.myStr(tvCategory.SelectedNode.Text); //comment on 04-09-2014 to avoid html format tags

        string ServiceName = common.myStr(tvCategory.SelectedNode.ToolTip.ToString());
        ServiceName = ServiceName.Replace("&", "%20/%20");
        //rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/SensitivityResult.aspx?LabNo=" + iLabNo + "&DiagSampleId=" + iDiagSampleId + "&OrganismIds=" + strOrganismIds + "&ResultId=" + iResultId + "&Source=" + source + "&ServiceName=" + ServiceName + "&ServiceId=" + iServiceId + "&FieldId=" + iFieldId + "&RStatus=" + common.myStr(ViewState["StatusCode"]) + "";
        rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/SensitivityResult.aspx?LabNo=" + iLabNo + "&DiagSampleId=" + iDiagSampleId + "&OrganismIds=" + strOrganismIds + "&ResultId=" + iResultId + "&Source=" + source + "&ServiceId=" + iServiceId + "&FieldId=" + iFieldId + "&RStatus=" + common.myStr(ViewState["StatusCode"]) + "&ServiceName=" + ServiceName + "";
        rwPrintLabReport.Height = 630;
        rwPrintLabReport.Width = 1000;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleOnPageLoad = true;
        rwPrintLabReport.VisibleStatusbar = false;
        rwPrintLabReport.Behaviors = WindowBehaviors.Move | WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Resize;
    }
    //Menu showed in horizontal view Click event of the Menu
    protected void menuTemplate_OnItemClick(object sender, RadMenuEventArgs e)
    {
        string ReviewRemark = string.Empty;
        if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
            ReviewRemark = txtReviewRemark.Text;
        holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                     common.myInt(ViewState["RegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));
        int FieldId = common.myInt(e.Item.Value);
        gvSelectedServices.DataSource = GetHorizontalFormatData(FieldId);
        gvSelectedServices.DataBind();
    }
    //Organism Type field Dropdown change event
    protected void ddlMultilineFormatsOrganism_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            TextBox txtMOrg = (TextBox)row.FindControl("txtMOrg");
            string fieldId = common.myStr(row.Cells[(int)eServicesGrid.FieldID].Text);
            string ReviewRemark = string.Empty;
            if (common.myStr(ViewState["IsAskReviewRemark"]).Equals("Y"))
                ReviewRemark = txtReviewRemark.Text;
            holdPreviousData(common.myInt(ViewState["SERVICEID"]), common.myInt(ViewState["DIAG_SAMPLEID"]),
                     common.myInt(ViewState["RegistrationId"]), common.myInt(ddlRemarks.SelectedValue), ReviewRemark, common.myInt(ViewState["SampleTypeId"]));
            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
            foreach (clsLabTemplate LT in collLabTemplate)
            {
                if (LT.ServiceId == common.myInt(ViewState["SERVICEID"]) && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                {
                    DataRow[] dr = LT.dsField.Tables[OPTIONS].Select("FieldId=" + fieldId + "and ValueId=" + ddl.SelectedValue + "and ValueType= 'F'");
                    if (dr.Length > 0)
                    {
                        if (common.myInt(ddl.SelectedValue) > 0)
                        {
                            txtMOrg.Text = common.myStr(dr[0]["Remarks"]);
                        }
                        else
                        {
                            txtMOrg.Text = "";
                        }
                    }


                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    //locationwise dropdown change event
    protected void ddlRange_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string Range = "", Symbol = "";
            double minValue, maxValue;
            foreach (GridViewRow row1 in gvSelectedServices.Rows)
            {
                RadComboBox ddlRange = row1.FindControl("ddlRange") as RadComboBox;
                Label lblN = (Label)row1.FindControl("lblN");
                TextBox txtN = (TextBox)row1.FindControl("txtN");
                lblN.Visible = true;
                if (ddlRange.Visible)
                {
                    if (ddlRange.Items.Count > 0)
                    {
                        string FieldId = common.myStr(ddlRange.SelectedItem.Attributes["FieldId"]);

                        dt = (DataTable)ViewState["Range"];
                        if (dt.Rows.Count > 0)
                        {
                            DataView objDv;
                            objDv = dt.DefaultView;
                            objDv.RowFilter = "MachineId=" + ddlRange.SelectedValue + "And FieldId ='" + FieldId + "'";
                            if (objDv.Count > 0)
                            {
                                minValue = common.myDbl(objDv[0]["minValue"]);
                                maxValue = common.myDbl(objDv[0]["maxValue"]);
                                Symbol = common.myStr(objDv[0]["Symbol"]);
                                Range = " (" + common.myDbl(minValue) + " " + common.myStr(Symbol) + " " + common.myDbl(maxValue) + ") ";
                                txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                lblN.Text = Range;
                                if (txtN.Text.Trim() != "")
                                {
                                    if (minValue <= common.myDbl(txtN.Text) &&
                                        maxValue >= common.myDbl(txtN.Text))
                                    {
                                        txtN.ForeColor = Color.Black;
                                    }
                                    else
                                    {
                                        txtN.ForeColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    #endregion

    protected void lnkTextFormat_OnClick(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        GridViewRow Row = lnk.NamingContainer as GridViewRow;
        RadEditor txtW = (RadEditor)Row.FindControl("txtW");
        int fieldId = common.myInt(Row.Cells[(int)eServicesGrid.FieldID].Text);

        if (fieldId == 0)
        {
            return;
        }

        Session["TextFormatDataResultEntry"] = common.myStr(txtW.Content);
        Session["FieldIdResultEntry"] = fieldId.ToString();
        string sMRD = common.myStr(lblRegNo.Text);
        string sPatientName = common.myStr(lblPatientName.Text);
        RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/TextFormatResultEntry.aspx?FieldId=" + fieldId + "&RegNo=" + sMRD + "&PName=" + sPatientName;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        //Added on 06-09-2014 Start Naushad conditionla based
        if (common.myStr(ViewState["StatusCode"]) != "RF")
        {

            RadWindowForNew.OnClientClose = "OnClientTextFormatClose";
        }
        //Added on 06-09-2014 End Naushad conditionla based
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnTextFormatClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            objDs = new DataSet();

            int FieldId = common.myInt(Session["FieldIdResultEntry"]);

            string strTextFormatData = common.myStr(Session["TextFormatDataResultEntry"]);

            collLabTemplate = new List<clsLabTemplate>();
            collLabTemplate = (List<clsLabTemplate>)ViewState["collLabTemplate"];
            int TableId = tvCategory.Nodes.IndexOf(tvCategory.SelectedNode);

            foreach (clsLabTemplate LT in collLabTemplate)
            {
                if (LT.ServiceId == common.myInt(ViewState["SERVICEID"]) && LT.DiagSampleId == common.myInt(ViewState["DIAG_SAMPLEID"]))
                {
                    LT.dsField.Tables[0].DefaultView.RowFilter = "";
                    LT.dsField.Tables[0].DefaultView.RowFilter = "FieldId=" + FieldId;

                    if (LT.dsField.Tables[0].DefaultView.Count > 0)
                    {
                        LT.dsField.Tables[2].DefaultView.RowFilter = "";
                        LT.dsField.Tables[2].DefaultView.RowFilter = "DiagSampleId=" + common.myInt(ViewState["DIAG_SAMPLEID"]) + " AND FieldId=" + FieldId;
                        if (LT.dsField.Tables[2].DefaultView.Count > 0)
                        {
                            LT.dsField.Tables[2].DefaultView[0]["FieldValue"] = strTextFormatData;
                            LT.dsField.Tables[2].DefaultView[0]["TextValue"] = strTextFormatData;
                            LT.dsField.Tables[2].AcceptChanges();
                        }
                        else
                        {
                            DataRow DR2 = LT.dsField.Tables[2].NewRow();
                            DR2["DiagSampleId"] = common.myInt(ViewState["DIAG_SAMPLEID"]);
                            DR2["FieldId"] = FieldId;
                            //DR2["FieldValue"] = Server.HtmlEncode(strTextFormatData);
                            //DR2["TextValue"] = Server.HtmlEncode(strTextFormatData);
                            DR2["FieldValue"] = strTextFormatData;
                            DR2["TextValue"] = strTextFormatData;
                            DR2["MachineId"] = 0;
                            DR2["FindingRemarks"] = "";
                            DR2["ResultId"] = 1;
                            DR2["EditMachineResult"] = 0;
                            DR2["ResultFromDB"] = 1;

                            LT.dsField.Tables[2].Rows.Add(DR2);
                            LT.dsField.Tables[2].AcceptChanges();
                        }

                        LT.dsField.Tables[2].AcceptChanges();
                        LT.dsField.Tables[2].DefaultView.RowFilter = "";
                    }

                    LT.dsField.Tables[0].AcceptChanges();
                    LT.dsField.Tables[0].DefaultView.RowFilter = "";
                }

                objDs = new DataSet();
                objDs.Tables.Add(LT.dsField.Tables[0].Copy());
                objDs.Tables.Add(LT.dsField.Tables[1].Copy());
                objDs.Tables.Add(LT.dsField.Tables[2].Copy());
            }

            ViewState["collLabTemplate"] = collLabTemplate;

            if (objDs != null)
            {
                //Added on 06-09-2014 Start  for Make sorting
                //objDs.Tables[0];
                DataView dv = new DataView(objDs.Tables[0]);
                dv.Sort = "SequenceNo Asc";
                gvSelectedServices.DataSource = dv.ToTable();
                //Added on 06-09-2014 End 
                //gvSelectedServices.DataSource = objDs.Tables[0];
                gvSelectedServices.DataBind();
            }
            else
            {
                gvSelectedServices.DataSource = GetHorizontalFormatData(FieldId);
                gvSelectedServices.DataBind();
            }
            Legend1.loadLegend("LAB", "'RE','RP','RF','MR'");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
            //return false;
        }
    }

    protected void lnkVisitHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myInt(txtLabNo.Text) == 0)
            {
                if (common.myStr(ViewState["MDFlag"]) == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            RadWindowForNew.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?MD=" + common.myStr(ViewState["MDFlag"]) +
                                        "&MP=NO&RegNo=" + common.myStr(lblRegNo.Text) + "&RegId=" + common.myInt(ViewState["RegistrationId"]);

            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindow1.OnClientClose = "wndAddService_OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        int totalNodes = tvCategory.Nodes.Count;
        int TableId = tvCategory.Nodes.IndexOf(tvCategory.SelectedNode);

        if (((totalNodes - 1) - TableId) > 0)
        {
            lblMessage.Text = "hello" + totalNodes.ToString() + " and " + TableId.ToString();
            tvCategory.Nodes[TableId + 1].Selected = true;
            tvCategory_SelectedNodeChanged(null, null);
        }
    }

    public void RefershTV(int SelectedID)
    {
        tvCategory.Nodes[SelectedID].Selected = true;
        tvCategory_SelectedNodeChanged(null, null);

    }

    protected void lnkClinicalDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sServiceId = common.myInt(ViewState["SERVICEID"]).ToString();
            Session["EncounterId"] = ViewState["EncounterId"];
            Session["RegistrationId"] = ViewState["RegistrationId"];

            if (common.myInt(sServiceId) > 0)
            {
                RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?MASTER=No" +
                                            "&CF=LAB" +
                                            "&ServId=" + sServiceId +
                                            "&ServName=" + common.myStr(tvCategory.SelectedNode.ToolTip) +
                                            "&ServiceOrderId=" + common.myInt(ViewState["ServiceDetailId"]).ToString() +
                                            "&EncounterId=" + common.myStr(ViewState["EncId"]) +
                                            "&TagType=L" +
                                            "&TemplateRequiredServices=" + sServiceId +
                                            "&SourceForLIS=LIS" +
                                            "&sOrdDtlId=" + common.myInt(ViewState["ServiceDetailId"]).ToString() +
                                            "&ManualLabNo=" + common.myStr(ViewState["ManualLabNo"]);

                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                // RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlLabNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLabNo.SelectedValue != "")
        {
            txtSearchCretria.Visible = true;
            ddlLabNo.Visible = true;
            txtLabNo.Text = ddlLabNo.SelectedValue;
            getInformation();
        }
    }
    protected void ddlSearch_OnTextChanged(object sender, EventArgs e)
    {
        ClearControls();
        if (string.Compare(common.myStr(ddlSearch.SelectedValue), "LN") == 0)
        {
            txtSearchCretria.Visible = false;
            txtLabNo.Visible = true;
            ddlLabNo.Visible = false;
            txtLabNo.Enabled = true;
        }
        else
        {
            txtSearchCretria.Visible = true;
            txtLabNo.Visible = false;
        }
    }
    private void ClearControls()
    {
        lblTotalManualLabNo.Text = "";
        txtLabNo.Text = "";
        txtSearchCretria.Text = "";
        ddlLabNo.Text = "";
        lblRegNo.Text = "";
        lblPatientName.Text = "";
        lblAgeGender.Text = "";
        lblManualLabNo.Text = "";
        lblOrderDate.Text = "";
        lblFacilityName.Text = "";
        lblWardBedNo.Text = "";
        tvCategory.Nodes.Clear();
        menuTemplate.Items.Clear();
        gvSelectedServices.DataSource = null;
        gvSelectedServices.DataBind();
    }
    protected void RemoveImage(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsLISPhlebotomy phlb = new clsLISPhlebotomy(sConString);
            string str = phlb.Deleteimageresult(0, common.myInt(((Button)sender).CommandName));
            Alert.ShowAjaxMsg(str, this);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void RenderAttachedImage(object sender, EventArgs e)
    {

        DataSet ds = new DataSet();
        objval = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        string key = "Word";
        string sFileName = common.myStr(((ImageButton)sender).CommandName);
        string sSavePath = common.myStr(ConfigurationManager.AppSettings["LabResultPath"]);
        string path = sSavePath + sFileName;
        string URLPath = "AttachementRender.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
        URLPath = URLPath.Replace("@FTPFolder", en.Encrypt(ConfigurationManager.AppSettings["LabResultPath"], key, true, string.Empty)).Replace("@FileName", en.Encrypt(sFileName, key, true, string.Empty));
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + URLPath.Replace("+", "%2B") + "','_blank')", true);


    }
    protected void ibtnUpload_Click(object sender, EventArgs e)
    {
        Button ibtnUpload = (Button)sender;
        FileUpload iFileUploader = (FileUpload)ibtnUpload.FindControl("iFileUploader");
        Label lblStatus = (Label)ibtnUpload.FindControl("lblStatus");
        HiddenField hdnFileAddress = (HiddenField)ibtnUpload.FindControl("hdnFileAddress");
        HiddenField hdnFileName = (HiddenField)ibtnUpload.FindControl("hdnFileName");
        ImageButton imgRIS = (ImageButton)ibtnUpload.FindControl("imgRIS");
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Visible = true;
        if (iFileUploader.HasFile)
        {
            string sExt = common.myStr(System.IO.Path.GetExtension(iFileUploader.PostedFile.FileName)).ToLower();
            if (sExt != ".jpg" || sExt != ".png" || sExt != ".gif" || sExt != ".jpeg" || sExt != ".bmp")
            {
                if (iFileUploader.PostedFile.ContentLength < 500000)
                {
                    HttpPostedFile myFile = iFileUploader.PostedFile;
                    string sFileName = "";
                    sFileName = common.myStr(((Button)sender).CommandName);
                    string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
                    sFileName = sFileName + System.IO.Path.GetExtension(myFile.FileName).ToLower();

                    hdnFileName.Value = sFileName.Trim();

                    int fileLen;
                    // Get the length of the file.
                    fileLen = iFileUploader.PostedFile.ContentLength;
                    // Create a byte array to hold the contents of the file.
                    byte[] image = new byte[fileLen - 1];
                    image = iFileUploader.FileBytes;

                    // Hold it in hidden field to insert to diaginvresultimage on save button click.
                    hdnFileAddress.Value = Convert.ToBase64String(image);

                    bool ret = UploadtoFTP(iFileUploader, sSavePath + sFileName);
                    if (ret)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = "Upload status: Sucessfully Update!" + sFileName.ToString();
                    }
                }
                else
                {
                    lblStatus.Text = "Upload status: The file has to be less than 400 kb!";
                }

            }
            else
            {

                lblStatus.Text = "Upload status: Upload only image files!";
            }

        }
        else
        {
            lblStatus.Text = "Upload status: Please browse image files!";
        }
    }



    private bool UploadtoFTP(FileUpload FileUpload1, string fileName)
    {
        bool ret = true;

        string Splitter = ConfigurationManager.AppSettings["Split"];
        if (common.myLen(Splitter).Equals(0))
        {
            Splitter = "!";
        }

        var csplitter = Splitter.ToCharArray();

        string ftp = ftppath.Split(csplitter)[0].ToString();

        try
        {
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(ftppath.Split(csplitter)[0].ToString() + fileName);
            ftpClient.Credentials = new System.Net.NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;

            ftpClient.ContentLength = FileUpload1.FileBytes.Length;
            byte[] buffer = new byte[4097];
            // int bytes = 0;
            int total_bytes = (int)FileUpload1.FileBytes.Length;

            using (Stream requestStream = ftpClient.GetRequestStream())
            {
                requestStream.Write(FileUpload1.FileBytes, 0, FileUpload1.FileBytes.Length);
                requestStream.Close();
            }

            FtpWebResponse response = (FtpWebResponse)ftpClient.GetResponse();

            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            string value = uploadResponse.StatusDescription;
            uploadResponse.Close();

            //lblMessage.Text += fileName + " uploaded.<br />";
            ret = true;
        }
        catch (WebException ex)
        {
            ret = false;
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }
        return ret;
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["NewSampleTypeId"] = common.myInt(ddlSampleType.SelectedValue);
            ViewState["SampleTypeId"] = common.myInt(ddlSampleType.SelectedValue);
            tvCategory_SelectedNodeChanged(null, null);
        }
        catch (Exception ex)
        {

        }

    }
}