using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using BaseC;
using System.Web;
using System.IO;

public partial class LIS_Phlebotomy_Phlebotomy : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    StringBuilder strXML = new StringBuilder();
    ArrayList coll = new ArrayList();
    
    string Flag = "", IsAllowBackDateSampleCollection = "", IsAllowBackDateToResultDate = "", hospitalSetupFlags = "";
    private void LoadFlagHospitalsetup()
    {
        clsExceptionLog objException = new clsExceptionLog();
        HospitalSetup objHospitalSetUP = new HospitalSetup(sConString);
        DataSet dsFlags = new DataSet();
        try
        {
            hospitalSetupFlags = "'IsAllowBackDateSampleCollection','IsAllowBackDateToResultDate'";
            dsFlags = objHospitalSetUP.getHospitalSetupValueMultiple(hospitalSetupFlags, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (dsFlags.Tables[0].Rows.Count > 0)
            {


                if (dsFlags.Tables[0].Select("Flag = 'IsAllowBackDateSampleCollection'").Length > 0)
                {
                    IsAllowBackDateSampleCollection = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsAllowBackDateSampleCollection'")[0].ItemArray[1]);
                    ViewState["IsAlwBDSmplColl"] = IsAllowBackDateSampleCollection;
                }

                if (dsFlags.Tables[0].Select("Flag = 'IsAllowBackDateToResultDate'").Length > 0)
                {
                    IsAllowBackDateToResultDate = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsAllowBackDateToResultDate'")[0].ItemArray[1]);
                    ViewState["IsAlwBDToResultDate"] = IsAllowBackDateToResultDate;
                }

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objHospitalSetUP = null;
            dsFlags.Dispose();
            objException = null;
        }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myLen(Request.QueryString["IpNo"]) > 0)   // Sample Collection from Ward
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Flag = common.myStr(Request.QueryString["MD"]);
        LoadFlagHospitalsetup();
        if (!IsPostBack)
        {
            try
            {
                if (common.myStr(Session["OutputDateFormat"]).Trim().Equals(string.Empty))
                {
                    Session["OutputDateFormat"] = "dd/MM/yyyy";
                }

                txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;

                string value = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToTagECInPhlebotomy", sConString));
                if (Flag == "")
                {
                    Flag = common.myStr(Request.QueryString["MD"]);
                }
                Cache.Remove("PHLEBOTOMY");
                ViewState["Selected"] = "0";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }

                if (string.Compare(common.myStr(ddlSearch.SelectedValue), "LN") == 0 || string.Compare(common.myStr(ddlSearch.SelectedValue), "RN") == 0)
                {
                    txtSearchCretria.Visible = false;
                    txtLabNo.Visible = true;
                }
                else
                {
                    txtSearchCretria.Visible = true;
                    txtLabNo.Visible = false;
                }

                ViewState["RegistrationId"] = "0";
                ViewState["EncounterId"] = "0";
                ViewState["EncounterDate"] = DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"]));
                ViewState["PT"] = "COLL";
                if (Request.QueryString["PT"] != null)
                {
                    ViewState["PT"] = common.myStr(Request.QueryString["PT"]);
                }
                ViewState["StationRequiredForPhlebotomy"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "StationRequiredForPhlebotomy", sConString);
                ViewState["IsPrintLabelAfterReportFinalization"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsPrintLabelAfterReportFinalization", sConString);
                ddlSource.SelectedValue = "A";
                btnInvResult.Visible = false;
                btnCollect.Visible = false;
                btnPrintLabel.Visible = true;
                btnSampleDeCollect.Visible = false;
                btnSampleDeAck.Visible = false;
                txtRemarks.Visible = false;
                lblRemarks.Visible = false;
                lnkRelayDetails.Visible = false;
                btnPatientResultHistory.Visible = false;
                tblFurtherAck.Visible = false;
                tblFurtherAck1.Visible = false;
                tblEntrySite.Visible = false;
                tblEntrySite1.Visible = false;
                fillSubDepartment();

                btnTagExternalCenter.Visible = false;
                gvTestDetails.Columns[7].Visible = false;
                lblSampleDispatch.Visible = false;
                if (common.myStr(Request.QueryString["IpNo"]).Trim().Length > 0)   // Sample Collection from Ward
                {
                    lblSampleDispatch.Visible = true;
                    btnCollect.Visible = true;
                    ViewState["PT"] = "";
                    ddlSource.SelectedIndex = 2;
                    ddlSource.Enabled = false;
                    ddlSearch.Enabled = false;
                }
                if (Flag.ToString() == "RIS")
                {
                    gvTestDetails.Columns[3].HeaderText = "Scan In Time";
                    gvTestDetails.Columns[4].HeaderText = "Scan Out Time";
                    gvTestDetails.Columns[8].Visible = false;

                }
                else
                {
                    gvTestDetails.Columns[3].Visible = true;
                    gvTestDetails.Columns[4].Visible = true;
                    gvTestDetails.Columns[8].Visible = true;
                }
                if (common.myStr(Request.QueryString["PType"]).ToUpper().Equals("WD"))
                {
                    btnVisitHistory.Visible = false;
                    btnPrintReferal.Visible = false;
                    ViewState["PT"] = "COLL";
                }
                switch (common.myStr(ViewState["PT"]))
                {
                    case "COLL":
                        if (common.myInt(Session["StationId"]) == 0)
                        {
                            if (common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "Y")
                            {
                                Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=" + common.myStr(ViewState["PT"]) + "&Module=" + Flag, false);
                            }
                        }
                        btnCollect.Visible = true;
                        gvTestDetails.Columns[7].Visible = true;
                        tblEntrySite.Visible = false;
                        tblEntrySite1.Visible = true;
                        if (value == "Y")
                        {
                            btnTagExternalCenter.Visible = true;
                        }
                        else if (value == "N")
                        {
                            btnTagExternalCenter.Visible = false;
                        }

                        chkManualrequest.Visible = true;
                        chkOutsourceInvestigations.Visible = true;
                        fillSampleCollectedStatus();
                        gvTestDetails.Columns[7].Visible = true;
                        lnkManualLabNo.Visible = true;
                        lblHeader.Text = "Sample&nbsp;Collection";
                        break;
                    case "DECOLL":

                        btnSampleDeCollect.Visible = true;
                        txtRemarks.Visible = true;
                        lblRemarks.Visible = true;
                        btnInvReport.Visible = false;
                        btnSampleDeCollect.Attributes.Add("onclick", "return ConfirmDeCollect();");
                        lblRemarks.Text = "&nbsp;Reason&nbsp;for&nbsp;Sample&nbsp;De&nbsp;Collection<span style='color: Red'>*</span>";

                        BindReason();
                        gvTestDetails.Columns[7].Visible = false;
                        lblHeader.Text = "Sample&nbsp;De&nbsp;Collection";
                        break;
                    case "STATUS":
                        if (common.myInt(Session["StationId"]) == 0)
                        {
                            Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=" + common.myStr(ViewState["PT"]) + "&Module=" + Flag, false);
                        }
                        fillSampleCollectedStatus();
                        DisplayStat();
                        btnPatientResultHistory.Visible = true;
                        btnInvResult.Visible = true;
                        tblEntrySite.Visible = false;
                        tblEntrySite1.Visible = true;
                        btnTagExternalCenter.Visible = true;
                        lnkRelayDetails.Visible = true;
                        chkManualrequest.Visible = true;
                        chkOutsourceInvestigations.Visible = true;
                        lnkClinicalDetails.Visible = true;
                        lnkPackageDetail.Visible = false;
                        lnkCancerScreening.Visible = true;
                        lnkAddendum.Visible = true;
                        lnkManualLabNo.Visible = true;
                        gvTestDetails.Columns[7].Visible = true;

                        lblHeader.Text = "Sample&nbsp;Status&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                        break;
                    case "DEACK":
                        if (common.myInt(Session["StationId"]) == 0)
                        {
                            Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=" + common.myStr(ViewState["PT"]) + "&Module=" + Flag, false);
                        }
                        txtRemarks.Visible = true;
                        lblRemarks.Visible = true;
                        lblRemarks.Text = "Reason for Sample UnAcknowledge<span style='color: Red'>*</span>";
                        btnSampleDeAck.Visible = true;
                        btnInvReport.Visible = false;
                        tblFurtherAck.Visible = false;
                        tblFurtherAck1.Visible = false;
                        if (common.myStr(Request.QueryString["Type"]) == "")
                        {
                            tblEntrySite.Visible = false;
                            tblEntrySite1.Visible = false;
                            ddlFurtherAck.SelectedValue = "N";
                            lblHeader.Text = "Sample&nbsp;UnAcknowledge&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        else if (common.myStr(Request.QueryString["Type"]) == "RSD")
                        {
                            tblEntrySite.Visible = false;
                            tblEntrySite1.Visible = false;
                            ddlFurtherAck.SelectedValue = "C";
                            btnSampleDeAck.Text = "Cancel Dispatch";
                            lblHeader.Text = "Sample&nbsp;Dispatch&nbsp;Cancel-&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        else if (common.myStr(Request.QueryString["Type"]) == "EN")
                        {
                            tblEntrySite.Visible = true;
                            tblEntrySite1.Visible = true;
                            ddlFurtherAck.SelectedValue = "Y";
                            btnSampleDeAck.Text = "Sample Dispatch";
                            lblHeader.Text = "Sample&nbsp;Dispatch To Other&nbsp;Location -&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        gvTestDetails.Columns[7].Visible = false;
                        btnSampleDeAck.Attributes.Add("onclick", "return ConfirmDeAcknowledge();");
                        lblHeader.Text = "Sample&nbsp;UnAcknowledge&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                        BindReason();
                        break;

                    case "DEACKSD":
                        if (common.myInt(Session["StationId"]) == 0)
                        {
                            Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=" + common.myStr(ViewState["PT"]) + "&Module=" + Flag, false);
                        }
                        lblEntrySitesDiag.Text = "Result Site";
                        txtRemarks.Visible = true;
                        lblRemarks.Visible = true;
                        lblRemarks.Text = "Reason for Sample Dispatch<span style='color: Red'>*</span>";
                        btnSampleDeAck.Visible = true;
                        btnInvReport.Visible = false;
                        tblFurtherAck.Visible = false;
                        tblFurtherAck1.Visible = false;
                        if (common.myStr(Request.QueryString["Type"]) == "")
                        {
                            tblEntrySite.Visible = false;
                            tblEntrySite1.Visible = true;
                            ddlFurtherAck.SelectedValue = "Y";
                            btnSampleDeAck.Text = "Sample Dispatch";
                            btnSampleDeAck.ToolTip = "Sample Dispatch";
                            lblHeader.Text = "Sample&nbsp;Dispatch&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        else if (common.myStr(Request.QueryString["Type"]) == "RSD")
                        {
                            tblEntrySite.Visible = false;
                            tblEntrySite1.Visible = false;
                            ddlFurtherAck.SelectedValue = "C";
                            btnSampleDeAck.Text = "Cancel Dispatch";
                            lblHeader.Text = "Sample&nbsp;Dispatch&nbsp;Cancel-&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        else if (common.myStr(Request.QueryString["Type"]) == "EN")
                        {
                            tblEntrySite.Visible = true;
                            tblEntrySite1.Visible = true;
                            ddlFurtherAck.SelectedValue = "Y";
                            btnSampleDeAck.Text = "Sample Dispatch";
                            lblHeader.Text = "Sample&nbsp;Dispatch To Other&nbsp;Location -&nbsp;" + common.myStr(Session["StationName"]);
                        }
                        gvTestDetails.Columns[7].Visible = false;
                        btnSampleDeAck.Attributes.Add("onclick", "return ConfirmDeAcknowledge();");
                        lblHeader.Text = "Sample&nbsp;Dispatch&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                        BindReason();
                        break;
                }

                objval = new BaseC.clsLISPhlebotomy(sConString);

                if (common.myInt(ddlSearch.Items.FindItemIndexByValue("LN")) >= 0)
                {
                    int ind = common.myInt(ddlSearch.Items.FindItemIndexByValue("LN"));
                    ddlSearch.Items.Remove(ddlSearch.Items.FindItemIndexByValue("LN"));


                    RadComboBoxItem lPAno = new RadComboBoxItem();
                    if (Flag.ToString() == "RIS")
                        lPAno.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
                    else
                        lPAno.Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
                    lPAno.Value = "LN";
                    lPAno.Enabled = true;
                    lPAno.Selected = true;
                    ddlSearch.Items.Insert(ind, lPAno);
                }

                objval = new BaseC.clsLISPhlebotomy(sConString);

                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;

                //Addedon 30-Jul-2014 Start Naushad    
                if (common.myLen(Session["StxtFromDate"]) > 0)
                {
                    txtFromDate.SelectedDate = common.myDate(Session["StxtFromDate"]);
                    txtToDate.SelectedDate = common.myDate(Session["StxtToDate"]);
                }

                if (common.myLen(Session["Source"]) > 0)
                {
                    ddlSource.SelectedValue = common.myStr(Session["Source"]);
                }

                if (common.myLen(Session["Subdepartment"]) > 0)
                {
                    ddlSubDepartment.SelectedValue = common.myStr(Session["Subdepartment"]);
                }
                //Added by Ujjwal 08 June 2015 to Collect sample from unperformed services screen start
                if (common.myStr(Request.QueryString["PageFrom"]).ToUpper().Equals("WARD"))// Sample Collection from Ward
                {
                    lblSampleDispatch.Visible = true;
                    btnCollect.Visible = true;
                    ddlSource.SelectedIndex = ddlSource.Items.IndexOf(ddlSource.Items.FindItemByValue("IPD"));
                    ddlSource.Enabled = false;
                    ddlSearch.SelectedIndex = ddlSearch.Items.IndexOf(ddlSearch.Items.FindItemByValue("RN"));
                    ddlStatus.Enabled = false;
                    ddlSearch.Enabled = false;
                    ddlEntrySitesDiag.Enabled = false;
                    txtLabNo.Text = common.myStr(Request.QueryString["UHID"]).Trim();
                    txtFromDate.SelectedDate = common.myDate(Request.QueryString["DT"]);
                    txtLabNo.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                    ddlEntrySites.Enabled = false;
                    ddlSubDepartment.Enabled = false;
                    ddlReportType.Enabled = false;
                    ViewState["PT"] = "COLL";
                    ddlReviewStatus.Enabled = false;
                    DdlTestPriority.Enabled = false;

                }
                //Added by Ujjwal 08 June 2015 to Collect sample from unperformed services screen end

                gvDetails.CurrentPageIndex = 0;
                bindControl();
                bindMainData();
                BlankGrid();
                if (common.myStr(Request.QueryString["pagefrom"]).ToUpper().Equals("WARD") && gvDetails.Items.Count > 0)
                {
                    BindFirstTestData();
                }
                if (common.myStr(ViewState["lblRegistrationNo"]) == "") { ViewState["lblRegistrationNo"] = common.myStr(Request.QueryString["RegNo"]); }
                if (common.myStr(Request.QueryString["IpNo"]).Trim().Length > 0)   // Sample Collection from Ward
                {
                    if (common.myInt(ddlStatus.Items.Count) > 0)
                    {
                        ddlStatus.SelectedIndex = 1;
                    }
                }
                string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);
                if (Act == "Y")
                {
                    chkAutoLable.Visible = true;
                    txtNooflbl.Visible = true;
                }
                else
                {
                    chkAutoLable.Visible = false;
                    txtNooflbl.Visible = false;
                }

                if (common.myStr(Request.QueryString["PType"]).ToUpper().Equals("WD"))
                {
                    lnkManualLabNo.Visible = false;
                    btnTagExternalCenter.Visible = false;
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
                if (Flag.ToString() == "RIS")
                {
                    Legend1.loadLegend("LAB", "'DA','RE','RP','RF', 'Stat', 'Redone'");
                }
                else
                {
                    Legend1.loadLegend("LAB", "");
                }
            }
        }
    }

    private enum GridColgvTestDetails : byte
    {

        COL0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4,

        COL5 = 5,
        COL6 = 6,
        COL7 = 7,
        COL8 = 8,
        COL9 = 9,

        COL10 = 10,
        COL11 = 11,
        COL12 = 12,
        COL13 = 13,
        COL14 = 14,

        COL15 = 15,
        COL16 = 16,
        COL17 = 17,
        COL18 = 18



    }

    private void BindFirstTestData()
    {

        ViewState["Selected"] = "1";
        lblMessage.Text = "";
        Label lblRegistrationNo = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblRegistrationNo");
        Label lblEncounterNo = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblEncounterNo");
        LinkButton lnkPatientName = (LinkButton)gvDetails.MasterTableView.Items[0].FindControl("lnkPatientName");
        Label lblFacilityId = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblFacilityId");
        Label lblFacilityName = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblFacilityName");
        Label lblManualLabNo = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblManualLabNo");
        Label lblAgeGender = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblAgeGender");
        ViewState["hdnEncounterId"] = ((HiddenField)gvDetails.MasterTableView.Items[0].FindControl("hdnEncounterId")).Value.ToString();
        Label lblSource = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblSource");

        ViewState["FacilityName"] = lblFacilityName.Text;
        ViewState["FacilityId"] = lblFacilityId.Text;
        ViewState["EncounterNo"] = lblEncounterNo.Text;
        ViewState["lblRegistrationNo"] = lblRegistrationNo.Text;
        ViewState["lblAgeGender"] = lblAgeGender.Text;

        ViewState["PatientName"] = lnkPatientName.Text;
        ViewState["ManualLabNo"] = lblManualLabNo.Text;
        ViewState["PatientSelected"] = "1";
        Label LBL = new Label();
        int RegistrationId = 0;
        int EncounterId = 0;
        int LabNo = 0;
        string EncounterDate = DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])); //DateTime.Now.ToString("MM-dd-yyyy");
        LBL = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblRegistrationId");
        ViewState["lblRegistrationId"] = LBL.Text;

        if (LBL.Text != "")
        {
            RegistrationId = common.myInt(LBL.Text);
        }
        LBL = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblEncounterId");
        if (LBL.Text != "")
        {
            EncounterId = common.myInt(LBL.Text);
        }
        LBL = (Label)gvDetails.MasterTableView.Items[0].FindControl("lblLabNo");
        if (LBL.Text != "")
        {
            LabNo = common.myInt(LBL.Text);
        }
        GridDataItem item = (GridDataItem)gvDetails.MasterTableView.Items[0];
        string value = common.myStr(item["EncodedDate"].Text);
        if (value != "")
        {
            EncounterDate = value;
        }
        ViewState["RegistrationId"] = RegistrationId;
        ViewState["EncounterId"] = EncounterId;
        ViewState["LabNo"] = LabNo;
        ViewState["EncounterDate"] = EncounterDate;
        gvDetails.Items[gvDetails.MasterTableView.Items[0].ItemIndex].Selected = true;
        ViewState["Source"] = lblSource.Text;
        bindTestDetailsData();
        gvTestDetails.Focus();
    }
    private void bindControl()
    {
        DataView dvFilter = new DataView();
        try
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);


            int iStationId = common.myStr(ViewState["PT"]) == "COLL"
                && common.myStr(ViewState["StationRequiredForPhlebotomy"]) != "Y" ? 0 : common.myInt(Session["StationId"]);


            DataSet dsReportType = objMaster.getReportType(common.myInt(Session["HospitalLocationID"]), 0, iStationId);
            if (dsReportType.Tables.Count > 0)
            {
                if (dsReportType.Tables[0].Rows.Count > 0)
                {
                    dsReportType.Tables[0].DefaultView.Sort = "ReportType ASC";


                    dvFilter = new DataView(dsReportType.Tables[0]);
                    if ((common.myStr(ViewState["PT"]) == "COLL" || common.myStr(ViewState["PT"]) == "DECOLL") && common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "N")
                    {
                        dvFilter.RowFilter = "LabType='G'";
                    }

                    ddlReportType.DataSource = dvFilter.ToTable();
                    ddlReportType.DataTextField = "ReportType";
                    ddlReportType.DataValueField = "Id";
                    ddlReportType.DataBind();
                    ddlReportType.Items.Insert(0, new RadComboBoxItem("", ""));
                    ddlReportType.SelectedIndex = 0;
                }
            }


            if (Cache["FACILITY"] == null || ((DataSet)Cache["FACILITY"]).Tables[0].Rows.Count == 0)
            {
                objMaster = new BaseC.clsLISMaster(sConString);
                DataSet ds1 = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
                Cache["FACILITY"] = ds1;
            }

            ddlFacility.DataSource = (DataSet)Cache["FACILITY"];
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlFacility.SelectedValue = common.myInt(Session["FacilityId"]).ToString();

            DataView dvEntryF = ((DataSet)Cache["FACILITY"]).Tables[0].DefaultView;
            dvEntryF.RowFilter = "";
            if (common.myStr(Request.QueryString["Type"]) != "" && common.myStr(Request.QueryString["Type"]) == "EN")
            {
                dvEntryF.RowFilter = "FacilityID <> " + common.myInt(Session["FacilityId"]) + "";
            }
            ddlFacilityEntrySite.DataSource = ((DataTable)dvEntryF.ToTable());
            ddlFacilityEntrySite.DataTextField = "FacilityName";
            ddlFacilityEntrySite.DataValueField = "FacilityID";
            ddlFacilityEntrySite.DataBind();

            if (common.myStr(Request.QueryString["Type"]) != "" && common.myStr(Request.QueryString["Type"]) == "EN")
                ddlFacilityEntrySite.SelectedIndex = 0;
            else
                ddlFacilityEntrySite.SelectedValue = common.myStr(Session["FacilityId"]);
            ddlFacilityEntrySite.Enabled = false;


            if (ViewState["LEGEND"] == null)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                DataSet ds = new DataSet();

                ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
                DataView dvL = ds.Tables[0].DefaultView;

                if (Flag.ToString() == "RIS")
                {
                    dvL.RowFilter = "Code IN('DA','RE','RP','RF')";
                }
                ViewState["LEGEND"] = dvL.ToTable();
            }

            ddlStatus.DataSource = (DataTable)ViewState["LEGEND"];
            ddlStatus.DataValueField = "StatusId";
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataBind();

            ddlStatus.Items.Insert(0, new RadComboBoxItem("", "0"));

            //if (common.myStr(Request.QueryString["MD"]) == "LIS")
            //{
            //    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue("Dept. Acknowledged"));
            //}
            //else
            //{
            ddlStatus.SelectedIndex = 0;
            //}



            DataTable dsS = (DataTable)ViewState["LEGEND"];
            DataView dv = dsS.DefaultView;
            switch (common.myStr(ViewState["PT"]))
            {

                case "DECOLL":
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    dv.RowFilter = "Code='SC'";
                    if (dv.Count > 0)
                    {
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(dv[0]["StatusId"])));
                    }
                    break;
                case "COLL":
                    if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
                    {
                        objval = new BaseC.clsLISPhlebotomy(sConString);
                        DataSet ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                        Cache["ENTRYSITES"] = ds;
                    }
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    dv.RowFilter = "Code='SNC'";
                    if (dv.Count > 0)
                    {
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(dv[0]["StatusId"])));
                    }
                    ddlEntrySitesDiag.DataSource = (DataSet)Cache["ENTRYSITES"];
                    ddlEntrySitesDiag.DataValueField = "EntrySiteId";
                    ddlEntrySitesDiag.DataTextField = "EntrySiteName";
                    ddlEntrySitesDiag.DataBind();
                    ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
                    ddlEntrySitesDiag.SelectedIndex = 0;

                    break;
                case "STATUS":
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    if (common.myStr(Request.QueryString["MD"]) == "LIS")
                    {
                        dv.RowFilter = "Code='DA'";
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(dv[0]["StatusId"])));
                    }
                    if (Cache["ENTRYSITES"] == null)
                    {
                        objval = new BaseC.clsLISPhlebotomy(sConString);
                        DataSet ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                        Cache["ENTRYSITES"] = ds;
                    }

                    ddlEntrySitesDiag.DataSource = (DataSet)Cache["ENTRYSITES"];
                    ddlEntrySitesDiag.DataValueField = "EntrySiteId";
                    ddlEntrySitesDiag.DataTextField = "EntrySiteName";
                    ddlEntrySitesDiag.DataBind();
                    ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
                    ddlEntrySitesDiag.SelectedIndex = 0;

                    break;
                case "DEACK":
                    objval = new BaseC.clsLISPhlebotomy(sConString);
                    if (common.myStr(Request.QueryString["Type"]) == "RSD")
                        dv.RowFilter = "Code='SD'";
                    else
                        dv.RowFilter = "Code='DA'";
                    if (dv.Count > 0)
                    {
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(dv[0]["StatusId"])));
                    }
                    break;
            }

            if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                DataSet ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                Cache["ENTRYSITES"] = ds;
            }

            DataView dvEntry = new DataView(((DataSet)Cache["ENTRYSITES"]).Tables[0]);
            if (common.myStr(Request.QueryString["PT"]) == "DEACK")
                ddlFacilityEntrySite.Enabled = true;

            dvEntry.RowFilter = " StationId  = " + common.myInt(Session["StationId"]) + " and FacilityId = " + common.myInt(ddlFacilityEntrySite.SelectedValue) + "";
            ddlEntrySitesDiag.DataSource = ((DataTable)dvEntry.ToTable());
            ddlEntrySitesDiag.DataValueField = "EntrySiteId";
            ddlEntrySitesDiag.DataTextField = "EntrySiteName";
            ddlEntrySitesDiag.DataBind();
            //ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlEntrySitesDiag.SelectedIndex = 0;
            dvEntry.Dispose();

            BaseC.clsEMRBilling objval1 = new BaseC.clsEMRBilling(sConString);
            DataSet DSS = objval1.getEntrySite(common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));

            ddlEntrySites.DataSource = DSS;
            ddlEntrySites.DataValueField = "ESId";
            ddlEntrySites.DataTextField = "ESName";
            ddlEntrySites.DataBind();

            ddlEntrySites.SelectedIndex = 0;

            //ddlEntrySites.SelectedIndex = ddlEntrySites.Items.IndexOf(ddlEntrySites.Items.FindItemByValue(common.myStr(Session["EntrySite"])));

            BaseC.InvestigationFormat invService = new BaseC.InvestigationFormat(sConString);
            DataSet objDs = invService.GetInvestigationServices("", "'IS','I'", common.myStr(Session["HospitalLocationID"]), common.myInt(Session["StationId"]), common.myInt(Session["FacilityId"]));
            objDs.Tables[0].DefaultView.Sort = "ServiceName Asc";
            ddlServiceName.DataSource = objDs.Tables[0].DefaultView;
            ddlServiceName.DataValueField = "ServiceID";
            ddlServiceName.DataTextField = "ServiceName";
            ddlServiceName.DataBind();
            ddlServiceName.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlServiceName.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dvFilter.Dispose();
        }
    }

    private void BindReason()
    {
        lblToWhomInformed.Text = "&nbsp;To&nbsp;whom&nbsp;Informed<span style='color: Red'>*</span>";
        txtToWhomInformed.Visible = true;
        ddlReason.Visible = true;
        ddlReason.Items.Clear();
        try
        {
            Flag = common.myStr(Request.QueryString["MD"]);
            DataSet ds = new DataSet();


            objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LIS Sample Decollect Reason", "");
            DataView dv = new DataView(ds.Tables[0]);

            if (Flag == "RIS")
            {
                dv.RowFilter = "code<>1";
            }

            ddlReason.DataSource = dv;
            ddlReason.DataValueField = "StatusId";
            ddlReason.DataTextField = "Status";
            ddlReason.DataBind();
            ddlReason.Items.Insert(0, new ListItem("-- Select Reason --", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objval = null;
        }
    }

    public void fillSubDepartment()
    {
        BaseC.clsLISMaster objLabRequest = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        int iStationId = common.myStr(ViewState["PT"]) == "COLL"
                     && common.myStr(ViewState["StationRequiredForPhlebotomy"]) != "Y" ? 0 : common.myInt(Session["StationId"]);
        DataView dvFilter = new DataView();
        try
        {
            ds = objLabRequest.GetSubDepartment(iStationId, 0);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dvFilter = new DataView(ds.Tables[0]);
                    if ((common.myStr(ViewState["PT"]) == "COLL" || common.myStr(ViewState["PT"]) == "DECOLL") && common.myStr(ViewState["StationRequiredForPhlebotomy"]) == "N")
                    {
                        dvFilter.RowFilter = "LabType='G'";
                    }
                    ds.Tables[0].DefaultView.Sort = "SubName";
                    ddlSubDepartment.DataSource = dvFilter.ToTable();
                    ddlSubDepartment.DataTextField = "SubName";
                    ddlSubDepartment.DataValueField = "SubDeptId";
                    ddlSubDepartment.DataBind();
                    ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));
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
            objLabRequest = null;
            ds.Dispose();
            iStationId = 0;
            dvFilter.Dispose();
        }
    }

    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubDepartment.SelectedValue != "-1")
            {
                DataSet ds = new DataSet();
                BaseC.clsLISMaster objmaster = new BaseC.clsLISMaster(sConString);
                ds = objmaster.GetHospitalServices(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(0), Convert.ToInt16(ddlSubDepartment.SelectedValue), common.myInt(Session["FacilityId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.Sort = "ServiceName";
                        ddlServiceName.DataSource = ds.Tables[0].DefaultView;
                        ddlServiceName.DataTextField = "ServiceNameWithCode";
                        ddlServiceName.DataValueField = "ServiceId";
                        ddlServiceName.DataBind();
                        ddlServiceName.Items.Insert(0, new RadComboBoxItem("", "0"));
                        ddlServiceName.SelectedIndex = 0;
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
    protected void ddlFacilityEntrySite_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
            Cache["ENTRYSITES"] = ds;
        }

        DataView dvEntry = new DataView(((DataSet)Cache["ENTRYSITES"]).Tables[0]);
        dvEntry.RowFilter = "";
        dvEntry.RowFilter = " StationId  = " + common.myInt(Session["StationId"]) + " and FacilityId = " + common.myInt(ddlFacilityEntrySite.SelectedValue) + "";

        ddlEntrySitesDiag.DataSource = ((DataTable)dvEntry.ToTable());
        ddlEntrySitesDiag.DataValueField = "EntrySiteId";
        ddlEntrySitesDiag.DataTextField = "EntrySiteName";
        ddlEntrySitesDiag.DataBind();
        //  ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
        ddlEntrySitesDiag.SelectedIndex = 0;
    }
    private bool isCancel()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myStr(ViewState["PT"]) == "DECOLL")
        {
            if (common.myStr(txtToWhomInformed.Text) == "")
            {
                strmsg += " Please Enter To Whom Informed for De Collection !";
                Page.SetFocus(txtToWhomInformed);
                isSave = false;
            }
            if (ddlReason.SelectedIndex <= 0 && common.myStr(txtRemarks.Text) == "")
            {
                strmsg += " Either Select Reason from dropdown or enter Remarks in textbox for De Collection !";
                Page.SetFocus(ddlReason);
                isSave = false;
            }

            //if (ddlReason.SelectedIndex <= 0)
            //{
            //    strmsg += " Please select Reason for De Collection !";
            //    Page.SetFocus(ddlReason);
            //    isSave = false;
            //}

            //if (common.myStr(txtRemarks.Text) == "")
            //{
            //    strmsg += " Please Enter Remarks for De Collection !";
            //    Page.SetFocus(txtRemarks);
            //    isSave = false;
            //}            
        }
        if (common.myStr(ViewState["PT"]) == "DEACK")
        {
            if (common.myStr(ddlFurtherAck.SelectedValue) == "")
            {
                strmsg += " Please Select Further Acknowledge !";
                isSave = false;
            }

            if (common.myStr(ddlFurtherAck.SelectedValue) == "Y"
                    && common.myInt(ddlEntrySitesDiag.SelectedValue) == 0)
            {
                strmsg += " Please Select Entry Site !";
                isSave = false;
            }

            if (common.myStr(txtToWhomInformed.Text) == "" && common.myStr(Request.QueryString["Type"]) != "EN")
            {
                strmsg += " Please Enter To Whom Informed for UnAcknowledge !";
                Page.SetFocus(txtToWhomInformed);
                isSave = false;
            }

            if (common.myStr(Request.QueryString["Type"]) != "EN" && ddlReason.SelectedIndex <= 0 && common.myStr(txtRemarks.Text) == "")
            {
                strmsg += " Either Select Reason from dropdown or enter Remarks in textbox for UnAcknowledge !";
                Page.SetFocus(ddlReason);
                isSave = false;
            }


            //if (ddlReason.SelectedIndex <= 0 && common.myStr(Request.QueryString["Type"]) != "EN")
            //{
            //    strmsg += " Please select Reason for UnAcknowledge !";
            //    Page.SetFocus(ddlReason);
            //    isSave = false;
            //}

            //if (common.myStr(txtRemarks.Text) == "" && common.myStr(Request.QueryString["Type"]) != "EN")
            //{
            //    strmsg += " Please Enter Remarks for UnAcknowledge !";
            //    Page.SetFocus(txtRemarks);
            //    isSave = false;
            //}
        }

        if (gvTestDetails.SelectedItems.Count < 1)
        {
            strmsg += " Please Select Sample !";
            isSave = false;
        }

        if (strmsg != "")
        {
            lblMessage.Text = strmsg;
        }
        return isSave;
    }

    protected void btnSampleDeCollect_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isCancel())
            {
                return;
            }

            foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label lblDiagSampleId = (Label)dataItem.FindControl("lblDiagSampleId");

                    if (lblDiagSampleId.Text != "")
                    {
                        coll.Add(common.myInt(lblDiagSampleId.Text));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.saveCancelSampleCollection(common.myStr(Source),
                                    common.myInt(Session["HospitalLocationID"]), strXML.ToString(),
                                    common.myStr(txtRemarks.Text), common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"]),
                                    common.myStr(txtToWhomInformed.Text), common.myStr(ddlReason.SelectedItem.Text),
                                    common.myInt(ddlReason.SelectedValue));



            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindMainData();
                BlankGrid();
            }

            txtRemarks.Text = "";
            txtToWhomInformed.Text = string.Empty;
            ddlReason.SelectedIndex = 0;
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected void btnSampleDeAck_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isCancel())
            {
                return;
            }

            foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label lblDiagSampleId = (Label)dataItem.FindControl("lblDiagSampleId");

                    if (lblDiagSampleId.Text != "")
                    {
                        coll.Add(common.myInt(lblDiagSampleId.Text));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}

            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.saveCancelSampleAck(common.myStr(Source), common.myInt(Session["HospitalLocationID"]),
                                   common.myStr(ddlFurtherAck.SelectedValue), common.myInt(ddlEntrySitesDiag.SelectedValue),
                                   strXML.ToString(), common.myInt(Session["UserID"]), txtRemarks.Text, common.myStr(txtToWhomInformed.Text), common.myStr(ddlReason.SelectedItem.Text), common.myInt(ddlReason.SelectedValue), 0);



            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindTestDetailsData();
            }

            txtRemarks.Text = "";
            txtToWhomInformed.Text = string.Empty;
            ddlReason.SelectedIndex = 0;
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindMainData()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        DataView dvE = new DataView();
        DataView dvFilter = new DataView();
        int labno = 0;
        int stationID = 0;
        int pageindex = 0;
        string EncounterNo = string.Empty;
        string RegistrationNo = string.Empty;
        string PatientName = string.Empty;
        string BedNo = string.Empty;
        string wardNo = string.Empty;
        string Mlabno = string.Empty;
        int entrySiteId = 0;
        int MannualRequest = 0;
        bool OutsourceInv = false;
        int ERrequest = 0;

        try
        {
            if (!common.myStr(Session["TATLabNo"]).Equals(string.Empty))
            {
                ddlSearch.SelectedValue = "LN";
                txtSearchCretria.Text = common.myStr(Session["TATLabNo"]);
                txtToDate.SelectedDate = common.myDate(Session["TATtxtToDate"]);
                txtFromDate.SelectedDate = common.myDate(Session["TATtxtFromDate"]);

                if (common.myStr(Session["TATSource"]).Equals("OPD"))
                {
                    ddlSource.SelectedValue = "OPD";
                }
                else
                {
                    ddlSource.SelectedValue = "IPD";
                }

                Session["TATSource"] = string.Empty;
                Session["TATtxtToDate"] = string.Empty;
                Session["TATLabNo"] = string.Empty;
                Session["TATtxtFromDate"] = string.Empty;
                ViewState["PT"] = "STATUS";
            }

            Session["StxtFromDate"] = txtFromDate.SelectedDate;
            Session["StxtToDate"] = txtToDate.SelectedDate;
            Session["Source"] = ddlSource.SelectedValue;

            if (common.myInt(ddlSubDepartment.SelectedValue) > 0)
            {
                Session["Subdepartment"] = ddlSubDepartment.SelectedValue;
            }
            else
            {
                Session.Remove("Subdepartment");
            }

            stationID = 0;
            if (common.myStr(ViewState["PT"]).Equals("STATUS") || common.myStr(ViewState["PT"]).Equals("DEACK"))
            {
                stationID = common.myInt(Session["StationId"]);
            }
            stationID = common.myInt(Session["StationId"]);

            pageindex = 0;
            if (gvDetails.Items.Count > 0)
            {
                pageindex = gvDetails.CurrentPageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }

            switch (common.myStr(ddlSearch.SelectedValue))
            {
                case "LN"://LabNo
                    labno = common.myInt(txtLabNo.Text.Trim());
                    break;
                case "MLN"://MLN
                    Mlabno = common.myStr(txtSearchCretria.Text).Trim();
                    break;
                case "RN"://MR#
                    RegistrationNo = common.myLong(txtLabNo.Text.Trim()).ToString();
                    break;
                case "PN"://PatientName
                    PatientName = common.myStr(txtSearchCretria.Text).Trim();
                    break;
                case "BN"://bedNo
                    BedNo = common.myStr(txtSearchCretria.Text).Trim();
                    break;
                case "WN"://WardNo
                    wardNo = common.myStr(txtSearchCretria.Text).Trim();
                    break;
                case "IP"://IPNo
                    EncounterNo = common.myStr(txtSearchCretria.Text).Trim();
                    break;
            }

            entrySiteId = 0;
            if (common.myStr(ddlSource.SelectedValue).Equals("IPD") || common.myStr(ddlSource.SelectedValue).Equals("A"))
            {
                entrySiteId = common.myInt(ddlEntrySitesDiag.SelectedValue);
            }

            MannualRequest = (chkManualrequest.Checked) ? 1 : 0;
            OutsourceInv = (chkOutsourceInvestigations.Checked) ? true : false;

            ERrequest = 0;
            if (!common.myStr(Request.QueryString["IpNo"]).Equals(string.Empty)) // sample collection from ward 
            {
                EncounterNo = common.myStr(Request.QueryString["IpNo"]).Trim();
            }

            if (common.myStr(ddlSource.SelectedValue).Equals("ER") || common.myStr(ddlSource.SelectedValue).Equals("A"))
            {
                ERrequest = 1;
            }

            ds = objval.getInvSampleMainData(common.myStr(ddlSource.SelectedValue), common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(ddlFacility.SelectedValue), common.myInt(Session["FacilityID"]), stationID,
                                    common.myInt(ddlStatus.SelectedValue), labno,
                                    common.myStr(DdlTestPriority.SelectedValue).Equals(string.Empty) ? "A" : common.myStr(DdlTestPriority.SelectedValue),
                                    common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate), gvDetails.PageSize,
                                    pageindex, RegistrationNo, EncounterNo, PatientName, BedNo, wardNo, MannualRequest, entrySiteId, common.myInt(ddlServiceName.SelectedValue),
                                    common.myInt(ddlSubDepartment.SelectedValue), ERrequest, Mlabno, common.myInt(ddlReportType.SelectedValue),
                                    string.Empty, common.myStr(ddlReviewStatus.SelectedValue), common.myInt(ddlEntrySites.SelectedValue), OutsourceInv);

            dvE = ds.Tables[0].DefaultView;
            if (common.myStr(Request.QueryString["Type"]).Equals("EN"))
            {
                dvE.RowFilter = "FacilityId=" + common.myInt(ddlFacility.SelectedValue);
            }

            dvFilter = dvE;
            if (common.myStr(common.myStr(ViewState["PT"])).Equals("COLL"))
            {
                dvFilter.RowFilter = "LabType='G'";
            }

            if (dvFilter.Count > 0)
            {
                gvDetails.VirtualItemCount = common.myInt(((DataTable)dvFilter.ToTable()).Rows[0]["TotalRecordsCount"]);
            }
            else
            {
                gvDetails.VirtualItemCount = common.myInt(0);
            }

            dvFilter.Sort = "EncodedDate Desc";
            gvDetails.DataSource = (DataTable)dvFilter.ToTable();
            gvDetails.DataBind();

            if (common.myStr(ddlSource.SelectedValue).Equals("IPD") || common.myStr(ddlSource.SelectedValue).Equals("A"))
            {
                gvDetails.Columns.FindByUniqueName("WardName").Visible = true;
                gvDetails.Columns.FindByUniqueName("BedNo").Visible = true;
            }
            else
            {
                gvDetails.Columns.FindByUniqueName("WardName").Visible = false;
                gvDetails.Columns.FindByUniqueName("BedNo").Visible = false;
            }

            if (common.myStr(ViewState["PT"]).Equals("COLL"))
            {
                fillSampleCollectedStatus();
            }
            //if (common.myStr(ViewState["PT"]).Equals("STATUS") && common.myStr(Request.QueryString["MD"])=="LIS" )
            if (common.myStr(ViewState["PT"]).Equals("STATUS"))
            {
                DisplayStat();
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
            dvFilter.Dispose();
            dvE.Dispose();
            ds.Dispose();
        }
    }

    private void SetGridColor()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        try
        {
            foreach (GridDataItem dataItem in gvTestDetails.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    Label lblStat = (Label)dataItem.FindControl("lblStat");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));

                    string str = "";
                    Label lbl = (Label)dataItem.FindControl("lblReviewedStatus");
                    str = lbl.Text.Trim();
                    if (str == "Yes")
                    {
                        dataItem.Cells[4].BackColor = lgreviewed.BackColor;
                    }

                    if (Convert.ToBoolean(lblStat.Text))
                    {
                        ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Stat");
                        if (!ds.Equals(null))
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dataItem.Cells[(byte)GridColgvTestDetails.COL4].BackColor = System.Drawing.Color.FromName(common.myStr(ds.Tables[0].Rows[0]["StatusColor"]));
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
    }
    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvDetails.CurrentPageIndex = e.NewPageIndex;
        ViewState["LabNo"] = null;
        bindMainData();
        BlankGrid();
    }
    protected void gvDetails_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ViewState["Selected"] = "1";
                lblMessage.Text = "";
                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
                LinkButton lnkPatientName = (LinkButton)e.Item.FindControl("lnkPatientName");
                Label lblFacilityId = (Label)e.Item.FindControl("lblFacilityId");
                Label lblFacilityName = (Label)e.Item.FindControl("lblFacilityName");
                Label lblManualLabNo = (Label)e.Item.FindControl("lblManualLabNo");
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                ViewState["hdnEncounterId"] = ((HiddenField)e.Item.FindControl("hdnEncounterId")).Value.ToString();
                Label lblSource = (Label)e.Item.FindControl("lblSource");
                Label lblEncodedDate = (Label)e.Item.FindControl("lblEncodedDate");
                ViewState["lblEncodedDate"] = lblEncodedDate.Text;   //jitendra kumar
                ViewState["FacilityName"] = lblFacilityName.Text;
                ViewState["FacilityId"] = lblFacilityId.Text;
                ViewState["EncounterNo"] = lblEncounterNo.Text;
                ViewState["lblRegistrationNo"] = lblRegistrationNo.Text;
                ViewState["lblAgeGender"] = lblAgeGender.Text;

                ViewState["PatientName"] = lnkPatientName.Text;
                ViewState["ManualLabNo"] = lblManualLabNo.Text;
                ViewState["PatientSelected"] = "1";
                Label LBL = new Label();
                int RegistrationId = 0;
                int EncounterId = 0;
                int LabNo = 0;
                string EncounterDate = DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])); //DateTime.Now.ToString("MM-dd-yyyy");
                LBL = (Label)e.Item.FindControl("lblRegistrationId");
                ViewState["lblRegistrationId"] = LBL.Text;

                if (LBL.Text != "")
                {
                    RegistrationId = common.myInt(LBL.Text);
                }
                LBL = (Label)e.Item.FindControl("lblEncounterId");
                if (LBL.Text != "")
                {
                    EncounterId = common.myInt(LBL.Text);
                }
                LBL = (Label)e.Item.FindControl("lblLabNo");
                ViewState["Labno"] = LBL.Text;
                if (LBL.Text != "")
                {
                    LabNo = common.myInt(LBL.Text);
                }
                GridDataItem item = (GridDataItem)e.Item;
                string value = common.myStr(item["EncodedDate"].Text);
                if (value != "")
                {
                    EncounterDate = value;
                }
                ViewState["RegistrationId"] = RegistrationId;
                ViewState["EncounterId"] = EncounterId;
                ViewState["LabNo"] = LabNo;
                ViewState["EncounterDate"] = EncounterDate;
                gvDetails.Items[e.Item.ItemIndex].Selected = true;
                ViewState["Source"] = lblSource.Text;

                bindTestDetailsData();
                gvTestDetails.Focus();

            }
            else if (e.CommandName == "PatientDetails")
            {
                LinkButton lnkPatientName = (LinkButton)e.Item.FindControl("lnkPatientName");
                Label lblFacilityName = (Label)e.Item.FindControl("lblFacilityName");
                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblRegistrationId = (Label)e.Item.FindControl("lblRegistrationId");

                if (lblRegistrationNo.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PatientDetails.aspx?RegNo=" + common.myStr(lblRegistrationNo.Text) + "&PName=" + lnkPatientName.Text + "&facility=" + common.myStr(lblFacilityName.Text) + "&RId=" + lblRegistrationId.Text;

                    RadWindowForNew.Height = 300;
                    RadWindowForNew.Width = 900;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;

                }
                else
                {
                    Alert.ShowAjaxMsg("MR# No. Not Exist", Page);
                }

            }
            else
            {
                ViewState["LabNo"] = null;
                bindTestDetailsData();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");

            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }

        if (e.Item.ItemType == GridItemType.Header)
        {
            if (Flag.ToString() == "RIS")
                ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "RadiologyNo"));
            else
                ((Label)e.Item.FindControl("lblLabHeader")).Text = common.myStr(HttpContext.GetGlobalResourceObject("PRegistration", "LabNo"));
        }

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
        {
            //if (common.myStr(ddlSource.SelectedValue) == "IPD")
            //{
            //    gvDetails.Columns[6].Visible = true;
            //}          
            //else
            //{
            //    gvDetails.Columns[6].Visible = false;
            //}
            if (common.myBool(DataBinder.Eval(e.Item.DataItem, "ManualRequest")) == true)
            {
                e.Item.BackColor = System.Drawing.Color.FromName("ControlLight");
            }
            if (((Label)e.Item.FindControl("lblNotesAvailable")).Text == "True")
            {
                ((Label)e.Item.FindControl("lblForNotes")).Visible = true;
            }
            LinkButton lnkalert = (LinkButton)e.Item.FindControl("lnkAlerts");
            LinkButton lnkPName = (LinkButton)e.Item.FindControl("lnkPatientName");
            if (common.myStr(lnkalert.Text).ToUpper() == "YES")
            {
                lnkPName.ForeColor = System.Drawing.Color.FromName("Red");
                lnkPName.Font.Bold = true;
                lnkalert.ForeColor = System.Drawing.Color.FromName("Red");
                lnkalert.Font.Bold = true;
                lnkPName.Font.Size = 12;
            }
            else if (common.myStr(lnkalert.Text).ToUpper() == "")
            {
                lnkalert.ForeColor = System.Drawing.Color.FromName("Blue");
                lnkalert.Text = "Alert";
            }
            Label lblLabStat = (Label)e.Item.FindControl("lblLabStat");
            string sStatColor = string.Empty;
            DataSet dsLabStat = new DataSet();
            if (common.myBool(lblLabStat.Text))
            {
                if (ViewState["StatColor"] != null)
                {
                    sStatColor = common.myStr(ViewState["StatColor"]);
                }
                else
                {
                    dsLabStat = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Stat");

                    if (!dsLabStat.Equals(null))
                    {
                        if (dsLabStat.Tables[0].Rows.Count > 0)
                        {
                            ViewState["StatColor"] = common.myStr(dsLabStat.Tables[0].Rows[0]["StatusColor"]);
                            sStatColor = common.myStr(dsLabStat.Tables[0].Rows[0]["StatusColor"]);
                        }
                    }
                    dsLabStat = null;
                }
                e.Item.Cells[(byte)GridColgvTestDetails.COL8].BackColor = System.Drawing.ColorTranslator.FromHtml(sStatColor);
            }
        }
    }
    private void bindTestDetailsData()
    {
        DataView dvFilter = new DataView();

        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            int stationID = common.myInt(Session["StationId"]);
            if (common.myStr(ViewState["PT"]) == "STATUS" || common.myStr(ViewState["PT"]) == "DEACK")
            {
                stationID = common.myInt(Session["StationId"]);
            }

            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }
            if (Source == "")
            {
                Source = "OPD";
            }

            int iEntrySite = common.myInt(ddlEntrySitesDiag.SelectedValue);
            if (common.myStr(ViewState["PT"]) == "COLL" || common.myStr(Request.QueryString["Type"]) == "RSD")
                iEntrySite = 0;


            string strTestPriority = "";

            if (DdlTestPriority.SelectedValue == "D")
                strTestPriority = "A";
            else
                strTestPriority = DdlTestPriority.SelectedValue;
            bool bitOutsourceInv = false;
            bitOutsourceInv = chkOutsourceInvestigations.Checked ? true : false;
            DataSet ds = objval.getInvSamplesDetailData(0, common.myInt(Session["HospitalLocationID"]), Source,
                  common.myInt(ViewState["LabNo"]), common.myInt(ddlStatus.SelectedValue), strTestPriority, 0, stationID,
                  common.myInt(Session["FacilityId"]), common.myStr(ViewState["ManualLabNo"]), "", iEntrySite, common.myInt(ddlSubDepartment.SelectedValue), common.myBool(bitOutsourceInv));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Finalized"] = common.myStr(ds.Tables[0].Rows[0]["StatusCode"]);

                    DataView dvPackage = new DataView();
                    try
                    {
                        dvPackage = ds.Tables[0].Copy().DefaultView;
                        dvPackage.RowFilter = "ISNULL(PackageId,0)>0";

                        ViewState["ServiceDetailsPackageIds"] = (DataTable)dvPackage.ToTable(true, "PackageId");
                    }
                    catch (Exception Ex)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Error: " + Ex.Message;

                        objException.HandleException(Ex);
                    }
                    finally
                    {
                        dvPackage.Dispose();
                    }





                    //if (ddlSource.SelectedValue == "PACKAGE")
                    //{
                    //    ds.Tables[0].DefaultView.RowFilter = "";
                    //    ds.Tables[0].DefaultView.RowFilter = "PackageId > 0";
                    //}
                    if (common.myStr(ViewState["Source"]) == "PACKAGE")
                    {
                        ds.Tables[0].DefaultView.RowFilter = "";
                        ds.Tables[0].DefaultView.RowFilter = "PackageId > 0";
                    }

                    DataView dvM = ds.Tables[0].DefaultView;
                    if (common.myStr(Request.QueryString["Type"]) == "RSD")
                    {
                        dvM.RowFilter = "EntrySiteId <>  " + common.myInt(ddlEntrySitesDiag.SelectedValue) + "";
                    }
                    dvFilter = new DataView(dvM.ToTable());

                    if (common.myStr(ViewState["PT"]) == "COLL" && common.myStr(ViewState["StationRequiredForPhlebotomy"]) != "Y")
                    {
                        dvFilter.RowFilter = "LabType ='G'";
                    }

                    gvTestDetails.DataSource = dvFilter.ToTable();
                    gvTestDetails.DataBind();
                    SetGridColor();
                    //if (ddlSource.SelectedValue == "PACKAGE")
                    //{
                    //    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = true;
                    //}
                    //else
                    //{
                    //    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = false;
                    //}
                }
                else
                {
                    bindMainData();
                    BlankGrid();
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

    protected void gvTestDetails_PreRender(object sender, EventArgs e)
    {
        SetGridColor();
    }

    protected void gvTestDetails_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                Label LBL = (Label)e.Item.FindControl("lblServiceId");
                Label lblRefServiceCode = (Label)e.Item.FindControl("lblRefServiceCode");
                Label lblDiagSampleId = (Label)e.Item.FindControl("lblDiagSampleId");


                //string Source = common.myStr(ddlSource.SelectedValue);
                //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
                //{
                //    Source = "OPD";
                //}

                string Source = common.myStr(ViewState["Source"]);
                if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
                {
                    Source = "OPD";
                }

                RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/TestDetails.aspx?ServiceId=" + common.myInt(LBL.Text) + "&DiagSampleId=" + common.myInt(lblDiagSampleId.Text) + "&RegID=" + common.myStr(ViewState["RegistrationId"]).Trim() + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"]).Trim() + "&PName=" + common.myStr(ViewState["PatientName"]).Trim() + "&LabNo=" + common.myStr(ViewState["LabNo"]).Trim() + "&RefServiceCode=" + common.myStr(lblRefServiceCode.Text).Trim() + "&Source=" + Source + "&MD=" + common.myStr(Request.QueryString["MD"]);

                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 800;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            //if (e.CommandName == "Print")
            //{
            //    hdnDailySerialNo.Value = Convert.ToString(((Label)e.Item.FindControl("lblDailySerialNo")).Text);
            //    btnPrintLabel_OnClick(source, e);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvTestDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            (e.Column as GridGroupSplitterColumn).ExpandImageUrl = "../../Images/Plusbox.gif";
            (e.Column as GridGroupSplitterColumn).CollapseImageUrl = "../../Images/Minubox.gif";

            e.Column.HeaderStyle.Width = Unit.Pixel(3);
            e.Column.ItemStyle.Width = Unit.Pixel(3);
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            e.Column.ItemStyle.VerticalAlign = VerticalAlign.Top;
        }
    }

    protected void gvTestDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            gvTestDetails.Columns[(byte)GridColgvTestDetails.COL18].Visible = false;

            if (e.Item is GridHeaderItem)
            {
                if (common.myStr(ViewState["Source"]) == "Package")
                {
                    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = true;
                }
                else
                {
                    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                GridDataItem di = e.Item as GridDataItem;
                TableCell cell = di["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];
                CHK.Visible = false;

                Label lblStatusID = (Label)e.Item.FindControl("lblStatusId");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                string iEntrySiteId = ((HiddenField)e.Item.FindControl("hdnEntrySiteId")).Value.ToString();
                LinkButton lblServiceName = (LinkButton)e.Item.FindControl("lblServiceName");
                HiddenField hdnIsClinicalTemplateRequired = (HiddenField)e.Item.FindControl("hdnIsClinicalTemplateRequired");
                LinkButton lnkprint = (LinkButton)e.Item.FindControl("ibtnPrintLabel");
                HiddenField hddstatus = (HiddenField)e.Item.FindControl("HdnPrintLabelStatus");

                if (common.myStr(hddstatus.Value) == "True")
                {
                    if (lblStatusCode.Text == "RF")
                    {
                        if (common.myStr(ViewState["IsPrintLabelAfterReportFinalization"]) == "N")
                        {
                            lnkprint.Enabled = false;
                        }
                        else
                        {
                            lnkprint.OnClientClick = "return confirm('Do you want to reprint ?');";
                        }
                    }
                    else
                    {
                        lnkprint.OnClientClick = "return confirm('Do you want to reprint ?');";
                    }
                }

                if (lblStatusCode.Text == "RF")
                {
                    if (common.myStr(ViewState["IsPrintLabelAfterReportFinalization"]) == "N")
                    {
                        lnkprint.Enabled = false;
                    }
                }

                if (common.myStr(ViewState["Source"]) == "Package")
                {
                    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = true;
                }
                else
                {
                    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = false;
                }
                string str = "";
                Label lbl = (Label)e.Item.FindControl("lblReviewedStatus");
                str = lbl.Text.Trim();
                if (str == "Yes")
                {
                    e.Item.Cells[1].BackColor = lgreviewed.BackColor;
                }
                if (common.myInt(hdnIsClinicalTemplateRequired.Value) == 1 || common.myStr(hdnIsClinicalTemplateRequired.Value) == "True")
                {
                    lblServiceName.Enabled = true;
                    lblServiceName.ToolTip = "Click To Show Sample Details..";
                }
                else
                    lblServiceName.Enabled = false;

                DataSet ds = new DataSet();
                if (ViewState["LEGEND"] == null)
                {
                    objval = new BaseC.clsLISPhlebotomy(sConString);

                    ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
                    DataView dv = ds.Tables[0].DefaultView;

                    if (Flag.ToString() == "RIS")
                    {
                        dv.RowFilter = "Code IN('DA','RE','RP','RF')";
                    }
                    ViewState["LEGEND"] = dv.ToTable();
                }

                DataTable dst = (DataTable)ViewState["LEGEND"];

                DataView dvSNC = dst.Copy().DefaultView;
                dvSNC.RowFilter = "Code = 'SNC'";

                DataView dvSC = dst.Copy().DefaultView;
                dvSC.RowFilter = "Code = 'SC'";

                DataView dvDA = dst.Copy().DefaultView;
                dvDA.RowFilter = "Code = 'DA'";

                DataView dvRE = dst.Copy().DefaultView;
                dvRE.RowFilter = "Code = 'RE'";

                DataView dvRP = dst.Copy().DefaultView;
                dvRP.RowFilter = "Code = 'RP'";
                if (common.myStr(Request.QueryString["IpNo"]).Trim().Length > 0)   // Sample Collection from Ward
                {
                    if (common.myInt(lblStatusID.Text) == common.myInt(dvSNC.ToTable().Rows[0]["StatusID"]))
                    {
                        CHK.Visible = true;
                    }

                }
                Label lblRemarks = (Label)e.Item.FindControl("lblRemarks");
                Label lblInstForPhlebotomy = (Label)e.Item.FindControl("lblInstForPhlebotomy");
                if (common.myStr(lblRemarks.Text) != "")
                {
                    lblRemarks.Text = "Instruction by Doctor - " + lblRemarks.Text;
                }
                if (common.myStr(lblInstForPhlebotomy.Text) != "")
                {
                    lblInstForPhlebotomy.Text = "Instruction for Phlebotomist - " + lblInstForPhlebotomy.Text;
                }

                switch (common.myStr(ViewState["PT"]))
                {
                    case "COLL":
                        if (common.myInt(lblStatusID.Text) == common.myInt(dvSNC.ToTable().Rows[0]["StatusID"]))
                        {
                            CHK.Visible = true;
                        }
                        break;
                    case "DECOLL":
                        if (common.myInt(lblStatusID.Text) == common.myInt(dvSC.ToTable().Rows[0]["StatusID"]))
                        {
                            CHK.Visible = true;
                        }
                        break;
                    case "STATUS":
                        if ((common.myInt(lblStatusID.Text) == common.myInt(dvDA.ToTable().Rows[0]["StatusID"])
                        || common.myInt(lblStatusID.Text) == common.myInt(dvRE.ToTable().Rows[0]["StatusID"])
                        || common.myInt(lblStatusID.Text) == common.myInt(dvRP.ToTable().Rows[0]["StatusID"])))
                        {
                            if (common.myStr(iEntrySiteId) == common.myStr(ddlEntrySitesDiag.SelectedValue))
                                CHK.Visible = true;
                            //gvTestDetails.Columns.FindByUniqueName("Print").Visible = true;
                        }
                        else
                        {
                            // gvTestDetails.Columns.FindByUniqueName("Print").Visible = false;
                        }
                        break;
                    case "DEACK":
                        if (common.myInt(lblStatusID.Text) == common.myInt(dvDA.ToTable().Rows[0]["StatusID"]))
                            CHK.Visible = true;

                        if (common.myStr(Request.QueryString["Type"]) == "RSD")
                            CHK.Visible = true;

                        break;

                    case "DEACKSD":
                        if (common.myInt(lblStatusID.Text) == common.myInt(dvDA.ToTable().Rows[0]["StatusID"]))
                            CHK.Visible = true;

                        if (common.myStr(Request.QueryString["Type"]) == "RSD")
                            CHK.Visible = true;

                        break;
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }

    protected void btnclose_OnClick(object sender, EventArgs e)
    {
        //bindTestDetailsData();
    }

    protected void btnCloseCollect_OnClick(object sender, EventArgs e)
    {
        bindMainData();
        BlankGrid();

        if (common.myInt(hdnIsValidPassword.Value) == 1)
        {
            btnPrintLabel1_OnClick(sender, e); //-- SATEN    Open later on demand to prin
            lblMessage.Text = "Successfully Collected";
        }
    }

    private bool isSavedCollection()
    {
        DataTable dtServiceDetailsPackageIds = new DataTable();
        bool IsValid = true;
        bool IsChecked = false;
        string UncheckedServiceName = string.Empty;
        StringBuilder AllUncheckedServiceNames = new StringBuilder();
        bool isSave = true;
        string strmsg = "";
        try
        {
            if (gvTestDetails.SelectedItems.Count < 1)
            {
                strmsg += "Please Select Sample !";
                isSave = false;
            }

            if (strmsg != "")
            {
                lblMessage.Text = strmsg;
            }

            if (ViewState["ServiceDetailsPackageIds"] != null)
            {
                dtServiceDetailsPackageIds = (DataTable)ViewState["ServiceDetailsPackageIds"];
                if (dtServiceDetailsPackageIds != null)
                {
                    if (dtServiceDetailsPackageIds.Rows.Count > 0)
                    {
                        for (int rowIdx = 0; rowIdx < dtServiceDetailsPackageIds.Rows.Count; rowIdx++)
                        {
                            IsValid = true;
                            IsChecked = false;
                            UncheckedServiceName = string.Empty;

                            foreach (GridDataItem dataItem in gvTestDetails.Items)
                            {
                                TableCell cell = dataItem["chkCollection"];
                                CheckBox CHK = (CheckBox)cell.Controls[0];

                                HiddenField hndPackageId = (HiddenField)dataItem.FindControl("hndPackageId");


                                if (common.myInt(dtServiceDetailsPackageIds.Rows[rowIdx]["PackageId"]).Equals(common.myInt(hndPackageId.Value)))
                                {
                                    LinkButton lblServiceName = (LinkButton)dataItem.FindControl("lblServiceName");

                                    IsValid = IsValid & CHK.Checked & CHK.Visible;

                                    if (CHK.Checked)
                                    {
                                        IsChecked = true;
                                    }
                                    else
                                    {
                                        if (common.myLen(UncheckedServiceName) > 0)
                                        {
                                            UncheckedServiceName += "," + common.myStr(lblServiceName.Text);
                                        }
                                        else
                                        {
                                            UncheckedServiceName = common.myStr(lblServiceName.Text);
                                        }
                                    }
                                }
                            }

                            if (!IsChecked)
                            {
                                IsValid = true;
                                UncheckedServiceName = string.Empty;
                            }

                            if (!IsValid && common.myLen(UncheckedServiceName) > 0)
                            {
                                if (common.myLen(AllUncheckedServiceNames.ToString()) > 0)
                                {
                                    AllUncheckedServiceNames.Append("," + UncheckedServiceName);
                                }
                                else
                                {
                                    AllUncheckedServiceNames.Append(UncheckedServiceName);
                                }
                            }
                        }

                        if (common.myLen(AllUncheckedServiceNames.ToString()) > 0)
                        {
                            lblMessage.Text = "Please select panel service(s) (" + AllUncheckedServiceNames.ToString() + ") in one go!";
                            isSave = false;
                        }

                    }
                }
            }

        }
        catch
        {
        }
        return isSave;
    }

    protected void btnCollect_OnClick(Object sender, EventArgs e)
    {
        try
        {
            Session["SampleCollectionIds"] = "";
            ViewState["Collect"] = 1;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSavedCollection())
            {
                return;
            }

            StringBuilder strXml = new StringBuilder();
            ArrayList coll = new ArrayList();

            Label LBL;
            ViewState["diagSampleId"] = "";

            foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    LBL = (Label)dataItem.FindControl("lblDiagSampleId");

                    if (common.myStr(ViewState["diagSampleId"]) == "")
                    {
                        ViewState["diagSampleId"] = LBL.Text;
                    }
                    else
                    {
                        ViewState["diagSampleId"] = common.myStr(ViewState["diagSampleId"]) + "," + LBL.Text;
                    }

                    coll.Add(LBL.Text);
                    LBL = (Label)dataItem.FindControl("lblServiceId");
                    coll.Add(LBL.Text);
                    HiddenField HdnSampleId = (HiddenField)dataItem.FindControl("HdnSampleId");
                    if (HdnSampleId.Value != "")
                    {
                        coll.Add(HdnSampleId.Value);
                    }
                    else
                    {
                        coll.Add(null);
                    }

                    strXml.Append(common.setXmlTable(ref coll));

                }
            }

            if (strXml.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }
            // common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}

            lblMessage.Text = "";
            Session["SampleCollectionIds"] = strXml.ToString();
            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/SampleCollection.aspx?SOURCE=" + (Source) + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"]) + "&PName=" + common.myStr(ViewState["PatientName"]).Trim() + "&PType=" + common.myStr(Request.QueryString["PType"]) + "&IsAllowBackDateSampleCollection=" + common.myStr(ViewState["IsAlwBDSmplColl"]).Trim() + "&lblEncodedDate=" + common.myStr(ViewState["lblEncodedDate"]).Trim();
            RadWindowForNew.Height = 320;
            RadWindowForNew.Width = 500;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientCloseCollect";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnPrintLabel_OnClick(object sender, EventArgs e)
    {
        StringBuilder strXml = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {

            string diagSampleId = "0";
            string PrintTRFLabel = "0";  //Both Service + TRF

            if (common.myInt(ViewState["LabNo"]) == 0 || (common.myStr(ViewState["Selected"]) == "0"))
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }

            LinkButton lnk = (LinkButton)sender;
            GridDataItem gdi = (GridDataItem)lnk.NamingContainer;

            Label lblDiagSampleId = (Label)gdi.FindControl("lblDiagSampleId");
            coll.Add("");
            coll.Add(lblDiagSampleId.Text);
            if (common.myStr(lblDiagSampleId.Text) != "")
            {
                diagSampleId = common.myStr(lblDiagSampleId.Text);
            }
            string sMD = string.Empty;
            if (Request.QueryString["MD"] != null)
            {
                sMD = common.myStr(Request.QueryString["MD"]);
            }
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }
            string IsPrintlblPRN = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

            //if (Request.QueryString["MD"] != null && sMD == "LIS")
            //{
            //    if (chkTRFLabel.Checked.Equals(true))
            //    {
            //        PrintTRFLabel = "1";   //Only TRF Label
            //    }
            //    else
            //    {
            //        PrintTRFLabel = "2";   //Only Service Label
            //    }
            //}

            if (IsPrintlblPRN == "Y" && sMD != "RIS")
            {
                //if (common.myStr(AllowToPrintTRFLabelinLIS) != null)    //Added on 15042020
                //{
                //if (common.myStr(AllowToPrintTRFLabelinLIS).Trim().ToUpper().Equals("Y"))
                //{
                diagSampleId = diagSampleId + "," + PrintTRFLabel;
                //    }
                //}

                string Str = common.myInt(ddlFacility.SelectedValue) + "$" + common.myStr(ViewState["LabNo"]) + "$" + diagSampleId + "$" + "PrintPRN_Lab" + "$" + common.myInt(txtNooflbl.Text);
                Str = "asplprint:" + Str;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
                return;
            }
            else
            {
                RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
                                                            "&LABNO=" + common.myInt(ViewState["LabNo"]) +
                                                            "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
                                                            "&DSNo=" + Convert.ToString(hdnDailySerialNo.Value)
                                                            + "&STATION=" + Flag
                                                            + "&DiagSampleId=" + diagSampleId;
                RadWindowForNew.Height = 580;
                RadWindowForNew.Width = 830;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            strXml = null;
            coll = null;
        }
    }
    protected void btnPrintLabel1_OnClick(object sender, EventArgs e)
    {

        if (common.myInt(ViewState["Collect"]) != 1)
        {
            ViewState["diagSampleId"] = "";

            foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label LBL = (Label)dataItem.FindControl("lblDiagSampleId");

                    if (common.myStr(ViewState["diagSampleId"]) == "")
                    {
                        ViewState["diagSampleId"] = LBL.Text;
                    }
                    else
                    {
                        ViewState["diagSampleId"] = common.myStr(ViewState["diagSampleId"]) + "," + LBL.Text;

                    }
                }
            }
        }

        lblMessage.Text = "";
        string Source = common.myStr(ViewState["Source"]);
        if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
        {
            Source = "OPD";
        }
        if (common.myStr(ViewState["diagSampleId"]) == "")
        {
            lblMessage.Text = "Please Select Sample !";
            return;
        }
        string sMD = string.Empty;

        string PrintTRFLabel = "0";  //Both Service + TRF

        if (Request.QueryString["MD"] != null)
        {
            sMD = common.myStr(Request.QueryString["MD"]);
        }


        string IsPrintlblPRN = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

        if (IsPrintlblPRN == "Y" && sMD != "RIS")
        {
            //            string Autolable = "0";
            string diagsampleID = "0";
            if (chkAutoLable.Checked == true)
            {
                //   Autolable = "1";
            }
            else
            {
                diagsampleID = common.myStr(ViewState["diagSampleId"]);
            }
            ViewState["diagSampleId"] = null;

            //if (common.myStr(AllowToPrintTRFLabelinLIS) != null)  //Added on 15042020
            //{
            //    if (common.myStr(AllowToPrintTRFLabelinLIS).Trim().ToUpper().Equals("Y"))
            //    {
            diagsampleID = diagsampleID + "," + PrintTRFLabel;
            //    }
            //}

            string Str = common.myInt(ddlFacility.SelectedValue) + "$" + common.myStr(ViewState["LabNo"]) + "$" + diagsampleID + "$" + "PrintPRN_Lab" + "$" + common.myInt(txtNooflbl.Text);
            Str = "asplprint:" + Str;
            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
            return;
        }
        else
        {

            RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
                                                        "&LABNO=" + common.myInt(ViewState["LabNo"]) +
                                                        "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
                                                        "&DSNo=" + Convert.ToString(hdnDailySerialNo.Value)
                                                        + "&STATION=" + Flag
                                                        + "&DiagSampleId=" + common.myStr(ViewState["diagSampleId"]);
            RadWindowForNew.Height = 580;
            RadWindowForNew.Width = 830;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
    }

    //protected void btnPrintLabel_OnClick(object sender, EventArgs e)
    //{
    //    int diagSampleId = 0;
    //    if (common.myInt(ViewState["LabNo"]) == 0 || (common.myStr(ViewState["Selected"]) == "0"))
    //    {
    //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
    //        return;
    //    }
    //    StringBuilder strXml = new StringBuilder();
    //    ArrayList coll = new ArrayList();
    //    //foreach (GridDataItem dataItem in gvTestDetails.Items)
    //    //{
    //    LinkButton lnk = (LinkButton)sender;
    //    GridDataItem gdi = (GridDataItem)lnk.NamingContainer;

    //    Label lblDiagSampleId = (Label)gdi.FindControl("lblDiagSampleId");
    //    coll.Add("");
    //    coll.Add(lblDiagSampleId.Text);
    //    if (common.myStr(lblDiagSampleId.Text) != "")
    //    {
    //        diagSampleId = common.myInt(lblDiagSampleId.Text);

    //    }

    //    string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

    //    string sMD = string.Empty;
    //    if (Request.QueryString["MD"] != null)
    //    {
    //        sMD = common.myStr(Request.QueryString["MD"]);
    //    }
    //    //string Source = common.myStr(ddlSource.SelectedValue);
    //    //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
    //    //{
    //    //    Source = "OPD";
    //    //}   

    //    string Source = common.myStr(ViewState["Source"]);
    //    if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
    //    {
    //        Source = "OPD";
    //    }
    //    if (Act == "Y" && sMD != "RIS")
    //    {


    //        string Str = common.myInt(ddlFacility.SelectedValue) + "$" + common.myStr(ViewState["LabNo"]) + "$" + diagSampleId + "$" + "PrintPRN_Lab";
    //        Str = "asplprint:" + Str;
    //        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
    //        return;

    //    }
    //    else
    //    {
    //        RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
    //                                                    "&LABNO=" + common.myInt(ViewState["LabNo"]) +
    //                                                    "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
    //                                                    "&DSNo=" + Convert.ToString(hdnDailySerialNo.Value)
    //                                                    + "&STATION=" + Flag
    //                                                    + "&DiagSampleId=" + diagSampleId;
    //        RadWindowForNew.Height = 580;
    //        RadWindowForNew.Width = 830;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        RadWindowForNew.OnClientClose = "OnClientClose";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //    }
    //}
    //protected void btnPrintLabel1_OnClick(object sender, EventArgs e)
    //{
    //    int diagSampleId = 0;

    //    if (common.myInt(ViewState["Collect"]) != 1)
    //    {
    //        ViewState["diagSampleId"] = "";

    //        foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
    //        {
    //            TableCell cell = dataItem["chkCollection"];
    //            CheckBox CHK = (CheckBox)cell.Controls[0];

    //            if (CHK.Checked == true && CHK.Visible == true)
    //            {
    //                Label LBL = (Label)dataItem.FindControl("lblDiagSampleId");

    //                if (common.myStr(ViewState["diagSampleId"]) == "")
    //                {
    //                    ViewState["diagSampleId"] = LBL.Text;
    //                }
    //                else
    //                {
    //                    ViewState["diagSampleId"] = common.myStr(ViewState["diagSampleId"]) + "," + LBL.Text;

    //                }
    //            }
    //        }
    //    }

    //    lblMessage.Text = "";
    //    string Source = common.myStr(ViewState["Source"]);
    //    if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
    //    {
    //        Source = "OPD";
    //    }
    //    //string Source = common.myStr(ddlSource.SelectedValue);
    //    //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
    //    //{
    //    //    Source = "OPD";
    //    //}

    //    if (common.myStr(ViewState["diagSampleId"]) == "")
    //    {
    //        lblMessage.Text = "Please Select Sample !";
    //        return;
    //    }

    //    string Act = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "PrintlblPRN", sConString);

    //    string sMD = string.Empty;
    //    if (Request.QueryString["MD"] != null)
    //    {
    //        sMD = common.myStr(Request.QueryString["MD"]);
    //    }
    //    if (Act == "Y" && sMD != "RIS")
    //    {
    //        string Autolable = "0";
    //        string diagsampleID = "0";
    //        if (chkAutoLable.Checked == true)
    //        {
    //            Autolable = "1";
    //        }
    //        else
    //        {
    //            diagsampleID = "'" + common.myStr(ViewState["diagSampleId"]) + "'";
    //        }
    //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        DataSet ds = dl.FillDataSet(CommandType.Text, "USPGetinforPRNlbl " + common.myInt(Session["FacilityID"]).ToString() + "," + common.myInt(ViewState["LabNo"]) + "," + diagsampleID.ToString() + "," + Autolable);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            if (chkAutoLable.Checked == true)
    //            {
    //                txtNooflbl.Text = ds.Tables[0].Rows.Count.ToString();
    //            }
    //            string strlblprm = "";

    //            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
    //            {
    //                string strrowval = "";
    //                for (int xi = 0; xi < ds.Tables[0].Rows.Count; xi++)
    //                {
    //                    if (strrowval == "")
    //                    {
    //                        strrowval = strrowval + ds.Tables[0].Rows[xi][i].ToString();
    //                    }
    //                    else
    //                    {
    //                        strrowval = strrowval + ";" + ds.Tables[0].Rows[xi][i].ToString();
    //                    }
    //                }
    //                if (strlblprm == "")
    //                {
    //                    strlblprm = strlblprm + strrowval;
    //                }
    //                else
    //                {
    //                    strlblprm = strlblprm + "#" + strrowval;
    //                }
    //            }
    //            int parseval = 1;
    //            if (!int.TryParse(txtNooflbl.Text, out parseval))
    //            {
    //                txtNooflbl.Text = "1";
    //            }
    //            string Str = "Parm=" + txtNooflbl.Text + "#" + strlblprm;
    //            Str = "asplprint:Doprint?" + Str;
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
    //            return;
    //        }
    //        else
    //        {
    //            Alert.ShowAjaxMsg("No record Found", this);
    //            return;
    //        }
    //    }
    //    else
    //    {

    //        RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PrintInvestigationLabelsReport.aspx?SOURCE=" + common.myStr(Source) +
    //                                                    "&LABNO=" + common.myInt(ViewState["LabNo"]) +
    //                                                    "&LoginFacilityId=" + common.myInt(Session["FacilityId"]) +
    //                                                    "&DSNo=" + Convert.ToString(hdnDailySerialNo.Value)
    //                                                    + "&STATION=" + Flag
    //                                                    + "&DiagSampleId=" + common.myStr(ViewState["diagSampleId"]);
    //        RadWindowForNew.Height = 580;
    //        RadWindowForNew.Width = 830;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        RadWindowForNew.OnClientClose = "OnClientClose";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //    }
    //}






    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtLabNo.Text))
        {
            Int64 UHID = 0;

            Int64.TryParse(txtLabNo.Text, out UHID);
            if ((UHID > 9223372036854775807 || UHID.Equals(0)))
            {
                if (common.myStr(ddlSearch.SelectedValue).Equals("LN"))
                {
                    lblMessage.Text = "Invalid Lab No";
                }
                else if (common.myStr(ddlSearch.SelectedValue).Equals("RN"))
                {
                    lblMessage.Text = "Invalid UHID No";
                }
                else
                {
                    lblMessage.Text = "Invalid Search Criteria";
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
        }
        lblMessage.Text = string.Empty;
        ViewState["Source"] = "";
        gvDetails.CurrentPageIndex = 0;
        bindMainData();
        ViewState["LabNo"] = null;
        ViewState["Selected"] = "0";
        BlankGrid();
    }


    protected void lnkRelayDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ViewState["Selected"]) == "0")
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            if (common.myInt(ViewState["LabNo"]) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/RelayDetails.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source) + "&LABNO=" + common.myInt(ViewState["LabNo"]);

            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnInvResult_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ViewState["Selected"]) == "0")
            {
                lblMessage.Text = "Please Select Patient !";
                return;
            }
            StringBuilder strXml = new StringBuilder();
            ArrayList coll = new ArrayList();

            Label LBL;

            for (int idx = 0; idx < gvDetails.SelectedItems.Count; idx++)
            {
                LBL = (Label)gvDetails.SelectedItems[idx].FindControl("lblLabNo");
                ViewState["LabNo"] = common.myInt(LBL.Text);
            }

            Session["xmlServices"] = "";

            foreach (GridDataItem dataItem in gvTestDetails.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label lblDiagSampleId = (Label)dataItem.FindControl("lblDiagSampleId");
                    Label lblServiceId = (Label)dataItem.FindControl("lblServiceId");

                    if (common.myStr(lblDiagSampleId.Text) != ""
                        && common.myStr(lblServiceId.Text) != "")
                    {
                        coll.Add(common.myInt(lblDiagSampleId.Text));
                        coll.Add(common.myInt(lblServiceId.Text));

                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            Session["xmlServices"] = strXML.ToString();
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}

            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            Response.Redirect("~/LIS/Phlebotomy/InvestigationResult.aspx?MD=" + Flag + "&SOURCE=" + Source +
                                "&LABNO=" + common.myInt(ViewState["LabNo"]), false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnPatientResultHistory_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["Selected"]) == "0")
        {
            Alert.ShowAjaxMsg("Please select a patient !", Page);
            return;
        }

        if (common.myStr(ViewState["lblRegistrationNo"]).Trim() != "")
        {
            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PatientHistory.aspx?MD=" + Flag + "&Master=Blank&RegID=" + common.myStr(ViewState["RegistrationId"]) + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"]) + "&PName=" + common.myStr(ViewState["PatientName"]) + "&FacilityId=" + common.myStr(ViewState["FacilityId"]);
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 930;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        else
        {
            lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
            return;
        }
    }

    protected void btnTagExternalCenter_OnClick(object sender, EventArgs e)
    {
        //strXML = new StringBuilder();

        if (common.myStr(ViewState["Selected"]) == "1")
        {
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/TagExternalCenter.aspx?MD=" + Flag + "&LabNo=" + common.myInt(common.myInt(ViewState["LabNo"])) + "&DiagSampleIds=" + strXML + "&source=" + Source;
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 750;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
            return;
        }
    }

    protected void btnVisitHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            //ViewState["FacilityName"] = lblFacilityName.Text;
            //ViewState["FacilityId"] = lblFacilityId.Text;
            //ViewState["EncounterNo"] = lblEncounterNo.Text;
            //ViewState["lblRegistrationNo"] = lblRegistrationNo.Text;
            //ViewState["lblAgeGender"] = lblAgeGender.Text;

            //ViewState["PatientName"] = lnkPatientName.Text;
            //ViewState["ManualLabNo"] = lblManualLabNo.Text;
            //ViewState["PatientSelected"] = "1";


            //LinkButton lnk = (LinkButton)sender;
            if (common.myInt(ViewState["lblRegistrationId"]) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }
            RadWindowForNew.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?MD=" + Flag + "&MP=NO&CF=PTA&PId=" + common.myInt(ViewState["lblRegistrationId"]);
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
    protected void btnPrintReferal_OnClick(object sender, EventArgs e)
    {
        //strXML = new StringBuilder();
        lblMessage.Text = "";
        if (common.myStr(ViewState["Selected"]) == "0")
        {
            if (Flag.ToString() == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }

        if (common.myInt(ViewState["LabNo"]) == 0)
        {
            if (Flag.ToString() == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }

        string Source = common.myStr(ViewState["Source"]);
        if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
        {
            Source = "OPD";
        }

        RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PrintReferalReport.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source) + "&LABNO=" + common.myInt(ViewState["LabNo"]) + "&EncId=" + ViewState["EncounterId"];

        RadWindowForNew.Height = 560;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnInvReport_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (common.myStr(ViewState["Selected"]) == "0")
        {
            if (Flag.ToString() == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }

        if (common.myInt(ViewState["LabNo"]) == 0)
        {
            if (Flag.ToString() == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }
        //string Source = common.myStr(ddlSource.SelectedValue);
        //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
        //{
        //    Source = "OPD";
        //}

        string Source = common.myStr(ViewState["Source"]);
        if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
        {
            Source = "OPD";
        }

        RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PrintInvestigationList.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source) + "&LABNO=" + common.myInt(ViewState["LabNo"]);

        RadWindowForNew.Height = 560;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    private void BlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("SampleCollectedDate");
            dt.Columns.Add("DispatchDate");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("EntrySiteName");
            dt.Columns.Add("DiagSampleId");
            dt.Columns.Add("ServiceId");
            dt.Columns.Add("RefServiceCode");
            dt.Columns.Add("StatusColor");
            dt.Columns.Add("StatusID");
            dt.Columns.Add("StatusCode");
            dt.Columns.Add("PackageName");
            dt.Columns.Add("DailySerialNo");
            dt.Columns.Add("EntrySiteId");
            dt.Columns.Add("IsClinicalTemplateRequired");
            dt.Columns.Add("Source");
            dt.Columns.Add("ServiceDetailId");
            dt.Columns.Add("InstructionForPhlebotomy");
            gvTestDetails.DataSource = dt;
            gvTestDetails.DataBind();
            //if (common.myStr(ViewState["Source"]) == "Package")
            //{
            //    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = true;
            //}
            //else
            //{
            //    gvTestDetails.Columns.FindByUniqueName("PackageName").Visible = false;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void fillSampleCollectedStatus()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        int stationID = common.myInt(Session["StationId"]);
        DataSet ds = new DataSet();
        try
        {
            ds = objval.GetSampleCollectedStatus(common.myInt(Session["FacilityId"]), common.myDate(txtToDate.SelectedDate), common.myDate(txtFromDate.SelectedDate), 0);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myStr(ViewState["PT"]).Equals("COLL"))
                    {
                        lblSampleCollectedStatus.Text = "(&nbsp;SNC&nbsp;OP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["OPSNC"]) + "&nbsp;&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPSNC"]) + "&nbsp;)&nbsp;";
                    }
                    if (!Flag.Equals("RIS"))
                    {
                        lblRejectedSampleStart.Text = "&nbsp;(&nbsp;Rejected&nbsp;Sample&nbsp;";
                        lblRejectedSampleEnd.Text = "&nbsp;)";

                        lnkBtnRejectedSampleOPCount.Text = "OP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["OPRS"]);
                        lnkBtnRejectedSampleIPCount.Text = "&nbsp;&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPRS"]);
                    }
                    //lblStat.Text = "(&nbsp;Stat&nbsp;OP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["OPStat"]) + "&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPStat"]) + "&nbsp;)&nbsp;";
                }
                lblStat.Text = "(&nbsp;Stat&nbsp;OP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["OPStat"]) + "&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPStat"]) + "&nbsp;)&nbsp;";
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

    private void DisplayStat()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        int stationID = common.myInt(Session["StationId"]);
        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(ViewState["PT"]).Equals("STATUS"))// && common.myStr(Request.QueryString["MD"]) == "LIS")
            {

                ds = objval.GetSampleCollectedStatus(common.myInt(Session["FacilityId"]), common.myDate(txtToDate.SelectedDate), common.myDate(txtFromDate.SelectedDate), 0);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblStat.Text = "(&nbsp;Stat&nbsp;OP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["OPStat"]) + "&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPStat"]) + "&nbsp;)&nbsp;";
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

    protected void lnkClinicalDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ViewState["Selected"]) == "1")
            {
                lblMessage.Text = "";
                //string Source = common.myStr(ddlSource.SelectedValue);
                //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
                //{
                //    Source = "OPD";
                //}
                string Source = common.myStr(ViewState["Source"]);
                if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
                {
                    Source = "OPD";
                }


                RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/ClinicalDetails.aspx?MD=" + Flag + "&SOURCE="
                    + common.myStr(Source) + "&EncounterNo="
                    + common.myStr(ViewState["EncounterNo"])
                    + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"])
                    + "&PName=" + common.myStr(ViewState["PatientName"]).Trim()
                    + "&LABNO=" + common.myInt(ViewState["LabNo"]).ToString()
                    + "&EnID=" + common.myStr(ViewState["hdnEncounterId"]);
                RadWindowForNew.Height = 560;
                RadWindowForNew.Width = 900;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
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

    protected void lnkPackageDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ViewState["Selected"]) == "1")
            {
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

                lblMessage.Text = "";
                //string Source = common.myStr(ddlSource.SelectedValue);
                //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
                //{
                //    Source = "OPD";
                //}

                string Source = common.myStr(ViewState["Source"]);
                if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
                {
                    Source = "OPD";
                }

                RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/PackageDetails.aspx?MD=" + Flag + "&SOURCE="
                    + common.myStr(Source)
                    + "&EncounterNo=" + common.myStr(ViewState["EncounterNo"])
                    + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"])
                    + "&PName=" + common.myStr(ViewState["PatientName"]).Trim()
                    + "&LABNO=" + Convert.ToInt64(ViewState["LabNo"])
                    + "&FacilityName=" + common.myStr(DefaultFacilityName);
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 950;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
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

    protected void ddlSource_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSource.SelectedValue == "IPD")
        {
            ddlSearch.Items.Insert(3, new RadComboBoxItem(common.myStr(GetGlobalResourceObject("PRegistration", "ipno")), "IP"));
        }
        else
        {
            RadComboBoxItem IPNo = ddlSearch.Items.FindItemByValue("IP");
            if (IPNo != null)
            {
                if (common.myStr(IPNo.Text) == ddlSearch.Items[3].Text)
                {
                    ddlSearch.Items.Remove(3);
                    if (Flag != common.myStr("RIS"))
                        ddlSearch.SelectedValue = "LN";
                    else
                        ddlSearch.SelectedValue = "RN";
                    txtSearchCretria.Text = "";
                }
            }
        }
        bindMainData();
        BlankGrid();
        ViewState["FacilityName"] = "";
        ViewState["FacilityId"] = "";
        ViewState["EncounterNo"] = "";
        ViewState["lblRegistrationNo"] = "";
        ViewState["PatientName"] = "";
        ViewState["PatientSelected"] = "0";
        ViewState["RegistrationId"] = "";
        ViewState["EncounterId"] = "";
        ViewState["LabNo"] = "";
        ViewState["EncounterDate"] = "";
    }

    protected void lnkManualLabNo_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ViewState["Selected"]) == "0")
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }

            if (common.myInt(ViewState["LabNo"]) == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/AttachManualLabno.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source)
                + "&EncounterNo=" + common.myStr(ViewState["EncounterNo"])
                + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"])
                + "&PName=" + common.myStr(ViewState["PatientName"]).Trim()
                + "&LABNO=" + Convert.ToInt64(ViewState["LabNo"])
                + "&PT=" + common.myStr(ViewState["PT"]);

            RadWindowForNew.Height = 620;
            RadWindowForNew.Width = 1300;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkCancerScreening_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ViewState["Selected"]) == "0")
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }

            if (common.myInt(ViewState["LabNo"]) == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            //string Source = common.myStr(ddlSource.SelectedValue);
            //if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            //{
            //    Source = "OPD";
            //}
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/AttachCancerScreening.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source)
                + "&EncounterNo=" + common.myStr(ViewState["EncounterNo"])
                + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"])
                + "&PName=" + common.myStr(ViewState["PatientName"]).Trim()
                + "&LABNO=" + Convert.ToInt64(ViewState["LabNo"]);

            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkAddendum_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ViewState["Selected"]) == "0")
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            if (common.myInt(ViewState["LabNo"]) == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/AddendumResult.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source)
                + "&EncounterNo=" + common.myStr(ViewState["EncounterNo"])
                + "&RegNo=" + common.myStr(ViewState["lblRegistrationNo"])
                + "&PName=" + common.myStr(ViewState["PatientName"]).Trim()
                + "&LABNO=" + Convert.ToInt64(ViewState["LabNo"]);

            RadWindowForNew.Height = 560;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lbtnNotes_OnClick(object sender, EventArgs e)
    {
        try
        {
            ImageButton lnk = (ImageButton)sender;

            lblMessage.Text = "";
            //if (common.myStr(ViewState["Selected"]) == "0")
            //{
            //    if (Flag.ToString() == "RIS")
            //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            //    else
            //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            //    return;
            //}

            if (common.myInt(((Label)lnk.FindControl("lblLabNo")).Text) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            string Source = common.myStr(ViewState["Source"]);
            if ((common.myStr(ViewState["Source"]) == "PACKAGE") || (common.myStr(ViewState["Source"]) == "ER"))
            {
                Source = "OPD";
            }


            RadWindowForNew.NavigateUrl = "~/LIS/Format/LISNotes.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source)
                + "&eno=" + common.myStr(((Label)lnk.FindControl("lblEncounterNo")).Text)
                + "&RegNo=" + common.myStr(((Label)lnk.FindControl("lblRegistrationNo")).Text)
                + "&pn=" + common.myStr(((LinkButton)lnk.FindControl("lnkPatientName")).Text)
                + "&LABNO=" + common.myStr(((Label)lnk.FindControl("lblLabNo")).Text);

            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkVisitHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            LinkButton lnk = (LinkButton)sender;

            //if (common.myStr(ViewState["Selected"]) == "0")
            //{
            //    if (Flag.ToString() == "RIS")
            //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            //    else
            //        lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            //    return;
            //}

            if (common.myInt(((Label)lnk.FindControl("lblLabNo")).Text) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            RadWindowForNew.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?MD=" + Flag + "&MP=NO&CF=PTA&PId=" + common.myStr(((Label)lnk.FindControl("lblRegistrationId")).Text);

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

    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkAlerts = (LinkButton)sender;
            string lblRegistrationNo = common.myStr(((Label)lnkAlerts.FindControl("lblRegistrationNo")).Text);
            string lnkPatientName = common.myStr(((LinkButton)lnkAlerts.FindControl("lnkPatientName")).Text);
            string lblAgeGender = common.myStr(((Label)lnkAlerts.FindControl("lblAgeGender")).Text);
            int iRegistrationId = common.myInt(((Label)lnkAlerts.FindControl("lblRegistrationId")).Text);
            int iEncounterId = common.myInt(((Label)lnkAlerts.FindControl("lblEncounterId")).Text);

            if (common.myStr(lnkAlerts.Text).ToUpper() == "YES")
            {
                RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&EId=" + common.myStr(iEncounterId) + "&PId=" + common.myStr(iRegistrationId) + "&PN=" + common.myStr(lnkPatientName) + "&PNo=" + common.myStr(lblRegistrationNo) + "&PAG=" + common.myStr(lblAgeGender) + "";
            }
            else if (common.myStr(lnkAlerts.Text).ToUpper() != "YES")
            {
                if (common.myStr(Session["LoginEmployeeType"]) != "D")
                    return;

                RadWindowForNew.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&Type=PatientAlert&EncounterId=" + common.myStr(iEncounterId) + "&RegistrationId=" + common.myStr(iRegistrationId);
                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            }
            RadWindowForNew.Height = 400;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
            RadWindowForNew.VisibleStatusbar = false;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

    protected void lblServiceName_OnClick(object sender, EventArgs e)
    {

        LinkButton lnkName = (LinkButton)sender;
        string sServiceId = ((Label)lnkName.FindControl("lblServiceId")).Text.ToString().Trim();
        HiddenField hdnServiceDetailId = (HiddenField)lnkName.FindControl("hdnServiceDetailId");
        Session["EncounterId"] = ViewState["EncounterId"];
        Session["RegistrationId"] = ViewState["RegistrationId"];

        if (common.myInt(sServiceId) > 0)
        {
            //StringBuilder strXML = new StringBuilder();
            //strXML.Append("<Table1><c1>");
            //strXML.Append(common.myInt(hdnServiceDetailId.Value));
            //strXML.Append("</c1></Table1>");

            //commented by rakesh start
            //RadWindowForNew.NavigateUrl = "~/EMR/Orders/ViewDetails.aspx?MASTER=No&ServId=" + sServiceId + "&ServName=" +
            //   lnkName.Text + "&From=POPUP&RegNo=" + common.myStr(ViewState["lblRegistrationNo"]) + "&EncounterId=" +
            //   common.myStr(ViewState["EncounterId"]) + "&PatientType=" + common.myStr(ViewState["Source"]) + "&RegID=" + common.myStr(ViewState["RegistrationId"]) +
            //   "&sOrdDtlId=" + common.myStr(hdnServiceDetailId.Value)+ "&FacliityID=" + common.myStr(ViewState["FacilityId"]);// +"&TagType=D";
            //commented by rakesh end
            //RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?Source=ProcedureOrder&ServiceIds=" + common.myStr(sServiceId.ToString() + "&RegNo=" + common.myStr(Session["RegistrationId"])) + "&TagType=S";

            //lnkName.Text

            RadWindowForNew.NavigateUrl = "~/EMR/Templates/Default.aspx?MASTER=No&CF=LAB&ServId=" + sServiceId + "&ServName=" +
                                        "&ServiceOrderId=" + common.myStr(hdnServiceDetailId.Value) + "&EncounterId=" + common.myStr(ViewState["EncounterId"]) + "&TagType=L" +
                                        "&TemplateRequiredServices=" + sServiceId + "&SourceForLIS=LIS&sOrdDtlId=" + common.myStr(hdnServiceDetailId.Value) +
                                        "&ManualLabNo=" + common.myStr(ViewState["ManualLabNo"] +
                                        "&Finalized=" + common.myStr(ViewState["Finalized"]));

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

    protected void ddlSearch_OnTextChanged(object sender, EventArgs e)
    {
        txtLabNo.Text = "";
        txtSearchCretria.Text = "";

        if (common.myStr(ddlSearch.SelectedValue).Equals("LN")
            || common.myStr(ddlSearch.SelectedValue).Equals("RN"))
        {
            txtSearchCretria.Visible = false;
            txtLabNo.Visible = true;
        }
        else
        {
            txtSearchCretria.Visible = true;
            txtLabNo.Visible = false;
        }
    }

    protected void lnkBtnRejectedSampleOPCount_OnClick(object sender, EventArgs e)
    {
        try
        {
            showRejectedSample("OPD");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkBtnRejectedSampleIPCount_OnClick(object sender, EventArgs e)
    {
        try
        {
            showRejectedSample("IPD");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void showRejectedSample(string Source)
    {
        try
        {
            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/RejectedSampleDetails.aspx?SOURCE=" + common.myStr(Source) +
                                        "&FDate=" + common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") +
                                        "&TDate=" + common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + "&CallFrom=0";

            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void chkAutolable_click(object sender, EventArgs e)
    {
        if (chkAutoLable.Checked == true)
        {
            txtNooflbl.ReadOnly = true;
        }
        else
        {
            txtNooflbl.ReadOnly = false;
        }
    }
    protected void lblSampleDispatch_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(ViewState["lblRegistrationNo"]) != "")
            {
                RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/SampleDispatch.aspx?RegNo=" + common.myStr(ViewState["lblRegistrationNo"]) + "&PName=SampleCollect&CallFrom=0&PType=" + common.myStr(Request.QueryString["PType"]);

                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1100;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient !";
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
}
