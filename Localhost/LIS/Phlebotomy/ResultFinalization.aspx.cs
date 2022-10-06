using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using Telerik.Web.UI;
using System.IO;
using BaseC;
using System.Web;
using System.Linq;
using System.Net;

public partial class LIS_Phlebotomy_ResultFinalization : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    string Flag = "";
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["Master"] == null)
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Flag == "")
                {
                    Flag = common.myStr(Request.QueryString["MD"]);
                }
                ViewState["IsPrintProvisional"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "WardAllowToPrintPRovisionalResult", sConString));
                ViewState["IsPrintUnCertifiedResult"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToPrintUnCertifiedResult", sConString));
                ViewState["IsTATReasonValidate"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "ShowTATReasonPopup", sConString));

                ViewState["EMRRestrictPrintResultForAllCase"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "EMRRestrictPrintResultForAllCase", sConString));

                ViewState["IsViewProvisional"] = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowToViewProvisionalResult", sConString));


                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Font.Bold = commonLabelSetting.cBold;
                if (commonLabelSetting.cFont != "")
                {
                    lblMessage.Font.Name = commonLabelSetting.cFont;
                }

                if (Request.QueryString["Master"] == null)
                {
                    lblHeader.Text = "Result&nbsp;Finalization&nbsp;-&nbsp;" + common.myStr(Session["StationName"]);
                    if (common.myInt(Session["StationId"]) == 0)
                    {
                        Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=FINALIZED&Module=" + Flag, false);
                    }
                    if ((common.myStr(Request.QueryString["LabNo"]) != null) && (common.myStr(Request.QueryString["LabNo"]) != "") && (common.myStr(Request.QueryString["Orderdate"]) != null) && (common.myStr(Request.QueryString["Orderdate"]) != ""))
                    {
                        ViewState["Orderdate"] = Convert.ToString(Request.QueryString["Orderdate"]);
                        ViewState["LabNo"] = Convert.ToString(Request.QueryString["LabNo"]);
                        if (common.myStr(Request.QueryString["Source"]) == "OPD")
                            ddlSource.SelectedValue = "OPD";
                        // else
                        // ddlSource.SelectedValue = "IPD";
                    }
                    else
                    {
                        ddlSource.SelectedValue = "OPD";
                    }

                    txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    ViewState["SelRow"] = "";
                    ViewState["RegistrationId"] = "0";
                    ViewState["EncounterId"] = "0";
                    ViewState["EncounterDate"] = DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"]));
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                }
                else
                {
                    lblHeader.Text = "Result&nbsp;View";
                    chkAssignedToMeOnly.Checked = false;
                    chkAssignedToMeOnly.Visible = false;
                    //  ddlSource.SelectedValue = "IPD";
                    chkNotFinalized.Checked = false;
                    lnkRelayDetails.Visible = false;
                    lnkPackageDetail.Visible = false;
                    btnResultFinalization.Visible = false;
                    btnCancelResult.Visible = false;
                    BtnCancelProvisionalResult.Visible = false;
                    //  dvAssignZone.Visible = false;

                    txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
                    txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

                    if (Session["StxtFromDate"] != null)
                    {
                        txtFromDate.SelectedDate = common.myDate(Session["StxtFromDate"]);
                        txtToDate.SelectedDate = common.myDate(Session["StxtToDate"]);

                        TimeSpan timeS = common.myDate(txtToDate.SelectedDate.Value) - common.myDate(txtFromDate.SelectedDate.Value);

                        if ((timeS.TotalDays + 1) > 10)
                        {
                            txtFromDate.SelectedDate = common.myDate(txtToDate.SelectedDate.Value).AddDays(-(10 - 1));
                            Session["StxtFromDate"] = txtFromDate.SelectedDate;
                        }
                    }

                    ViewState["SelRow"] = "";
                    ViewState["RegistrationId"] = "0";
                    ViewState["EncounterId"] = "0";
                    txtSearchCretria.Text = common.myStr(Request.QueryString["RegNo"]);

                    //Added by rakesh on 26/06/2014 for filter the tests according the reg no start
                    if (!(common.myStr(Request.QueryString["RegNo"]).Equals(string.Empty)))
                    {
                        txtSearchCretriaNumeric.Text = common.myStr(Request.QueryString["RegNo"]);
                    }
                    //Added by rakesh on 26/06/2014 for filter the tests according the reg no end

                    ddlSearch.SelectedValue = "RN";
                    ViewState["EncounterDate"] = DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"]));
                    txtFromDate.SelectedDate = common.myDate(Convert.ToDateTime(Request.QueryString["Admisiondate"]).ToString("dd/MM/yyyy"));
                    txtToDate.SelectedDate = DateTime.Now;
                }
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

                    if (Request.QueryString["Master"] == null)
                    {
                        lPAno.Selected = true;
                    }
                    ddlSearch.Items.Insert(ind, lPAno);
                }


                if (Request.QueryString["Master"] == "WARD")  //condtion Added for calling page from Ward
                {
                    Session.Remove("StxtFromDate");
                    Session.Remove("StxtToDate");
                    Session.Remove("source");
                    Session.Remove("Subdepartment");
                    trgrdfooter.Visible = false;
                    Panel1.Visible = false;
                }

                if (Session["StxtFromDate"] != null)
                {
                    txtFromDate.SelectedDate = common.myDate(Session["StxtFromDate"]);
                    txtToDate.SelectedDate = common.myDate(Session["StxtToDate"]);

                    TimeSpan timeS = common.myDate(txtToDate.SelectedDate.Value) - common.myDate(txtFromDate.SelectedDate.Value);

                    if ((timeS.TotalDays + 1) > 10)
                    {
                        txtFromDate.SelectedDate = common.myDate(txtToDate.SelectedDate.Value).AddDays(-(10 - 1));
                        Session["StxtFromDate"] = txtFromDate.SelectedDate;
                    }
                }

                if (Session["source"] != null)
                {
                    ddlSource.SelectedValue = common.myStr(Session["Source"]);
                }

                fillSubDepartment();

                if (Session["Subdepartment"] != null)
                {
                    ddlSubDepartment.SelectedValue = common.myStr(Session["Subdepartment"]);
                }

                gvResultFinal.CurrentPageIndex = 0;
                GetValueShowProfileDetailsInFinalization();
                bindControl();
                ddlFacility.SelectedValue = common.myStr(common.myInt(Session["FacilityId"]));

                bindMainData();

                string strEmployeeType = common.myStr(Session["EmployeeType"]);
                if (strEmployeeType == "LDIR" || strEmployeeType == "LD")
                {
                    ddlReleaseStage.SelectedValue = "F";
                    ddlReleaseStage.Enabled = true;
                    ddlEmployee.Enabled = true;
                }
                if (common.myStr(ddlSearch.SelectedValue) == "LN" || ddlSearch.SelectedValue == "RN")
                {
                    txtSearchCretria.Visible = false;
                    txtSearchCretriaNumeric.Visible = true;
                }
                else
                {
                    txtSearchCretria.Visible = true;
                    txtSearchCretriaNumeric.Visible = false;
                }
                if (common.myStr(Request.QueryString["Master"]) == "WARD")
                {
                    lnkConsolidateLabReport.Visible = true;
                }
                ViewState["IsEnableUserAuthentication"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "UserAuthenticationForLISAndPhlebotomy", sConString);

                BaseC.Security objSecurity = new Security(sConString);
                bool ConfidentialRight = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsViewConfidentialLabReports");
                ViewState["ConfidentialRight"] = ConfidentialRight;
                objSecurity = null;

                checkcolnreferbysequence();

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
            Legend1.loadLegend("LAB", "'RE','RP','RF','STAT'");
        }
    }

    private void checkcolnreferbysequence()
    {
        try
        {
            int b12 = gvResultFinal.Columns.FindByUniqueName("LabTechnician").OrderIndex;
            int b16 = gvResultFinal.Columns.FindByUniqueName("Result").OrderIndex;

            string flgg = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsAllowResultAfterFieldNameInLIS", sConString));
            if (flgg.ToUpper().Equals("Y"))
            {
                gvResultFinal.MasterTableView.SwapColumns(b12, b16);
            }
        }
        catch { }
    }


    private void GetValueShowProfileDetailsInFinalization()
    {
        clsLISSampleReceivingStation oclsLISSampleReceivingStation = new clsLISSampleReceivingStation(sConString);
        DataSet dsShowProfileDetailsInFinalization = new DataSet();
        dsShowProfileDetailsInFinalization = oclsLISSampleReceivingStation.GetStation(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["StationId"]));
        if (dsShowProfileDetailsInFinalization.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(Request.QueryString["Master"]) != "WARD")
            {
                if (dsShowProfileDetailsInFinalization.Tables[0].Columns.Contains("ShowProfileDetailsInFinalization"))
                {
                    ViewState["ShowProfileDetailsInFinalization"] = common.myStr(dsShowProfileDetailsInFinalization.Tables[0].Rows[0]["ShowProfileDetailsInFinalization"]) == "True" ? 1 : 0;

                }
                else { ViewState["ShowProfileDetailsInFinalization"] = 1; }
            }
            if (dsShowProfileDetailsInFinalization.Tables[0].Columns.Contains("DefaultResultEntryStatus"))
            {
                ViewState["DefaultResultEntryStatus"] = common.myStr(dsShowProfileDetailsInFinalization.Tables[0].Rows[0]["DefaultResultEntryStatus"]);
            }
        }
        dsShowProfileDetailsInFinalization = null;
    }

    public void fillSubDepartment()
    {
        try
        {
            BaseC.clsLISMaster objLabRequest = new BaseC.clsLISMaster(sConString);
            DataSet ds = new DataSet();
            ds = objLabRequest.GetSubDepartment(Convert.ToInt32(Session["StationId"]), 0);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.Sort = "SubName";
                    ddlSubDepartment.DataSource = ds.Tables[0].DefaultView;
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
    }

    private void bindMainData()
    {
        StringBuilder strXML = null;
        ArrayList coll = null;
        try
        {
            if (!IsValidSearchCriteria())
            {
                BindMainBlankGrid();
                return;
            }
            hdnAssignDiagSampleId.Value = "0";
            objval = new BaseC.clsLISPhlebotomy(sConString);
            int stationID = 0;
            int iShowAssignedToMe = 0;
            int iNotFinalized = 0;
            stationID = common.myInt(Session["StationId"]);
            if ((common.myStr(ViewState["LabNo"]) != null) && (common.myStr(ViewState["LabNo"]) != "") && (common.myStr(ViewState["Orderdate"]) != null) && (common.myStr(ViewState["Orderdate"]) != ""))
            {
                txtSearchCretria.Text = common.myStr(Request.QueryString["LabNo"]);
                txtFromDate.SelectedDate = common.myDate(Request.QueryString["Orderdate"]);
                txtToDate.SelectedDate = common.myDate(Request.QueryString["Orderdate"]);
                chkAssignedToMeOnly.Checked = false;
                ViewState["LabNo"] = null;
                ViewState["Orderdate"] = null;
            }
            if (chkAssignedToMeOnly.Checked == true)
            {
                iShowAssignedToMe = 1;
            }
            else
            {
                iShowAssignedToMe = 0;
            }
            if (chkNotFinalized.Checked == true)
            {
                iNotFinalized = 1;
            }
            else
            {
                iNotFinalized = 0;
            }
            int LabNo = 0; string RegistrationNo = ""; string PatientName = "", Mlabno = ""; string EncounterNo = "";
            if (ddlSearch.SelectedValue == "LN")//Accession#
            {
                if (!string.IsNullOrEmpty(txtSearchCretriaNumeric.Text))
                {
                    Int64 Search;
                    Int64.TryParse(txtSearchCretriaNumeric.Text, out Search);
                    if ((Search > 9223372036854775807 || Search.Equals(0)))
                    {
                        lblMessage.Text = "Invalid LAB No";
                        return;
                    }
                }
                LabNo = common.myInt(txtSearchCretriaNumeric.Text);
            }
            else if (ddlSearch.SelectedValue == "RN")//MR#
            {
                if (!string.IsNullOrEmpty(txtSearchCretriaNumeric.Text))
                {
                    Int64 Search;
                    Int64.TryParse(txtSearchCretriaNumeric.Text, out Search);
                    if ((Search > 9223372036854775807 || Search.Equals(0)))
                    {
                        lblMessage.Text = "Invalid UHID No";
                        return;
                    }
                }
                RegistrationNo = txtSearchCretriaNumeric.Text;
            }
            else if (ddlSearch.SelectedValue == "PN")
            {
                PatientName = txtSearchCretria.Text;
            }
            if (ddlSearch.SelectedValue == "IP")//IPNo
            {
                EncounterNo = txtSearchCretria.Text;
            }
            else if (ddlSearch.SelectedValue == "MLN")//LabNo
            {
                Mlabno = common.myStr(txtSearchCretria.Text);
            }

            int pageindex = 0;
            if (gvResultFinal.Items.Count > 0)
            {
                pageindex = gvResultFinal.CurrentPageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }
            string Source = common.myStr(ddlSource.SelectedValue);
            if (ddlSource.SelectedValue == "ER")
            {
                Source = "OPD";
            }
            int ERrequest = 0;
            if (ddlSource.SelectedValue == "ER")
                ERrequest = 1;
            bool iIsCallFromLab = true;

            if (Request.QueryString["Master"] == "WARD")  //condtion Added for calling page from Ward
            {
                iIsCallFromLab = false;
                gvResultFinal.Columns.FindByUniqueName("IsPrintPatientDiagnosis").Visible = false;
            }
            string sServiceName = string.Empty;
            strXML = new StringBuilder();
            coll = new ArrayList();
            foreach (RadComboBoxItem currentItem in ddlSubDepartment.Items)
            {
                if (currentItem.Checked == true)
                {
                    coll.Add(common.myInt(currentItem.Value));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }
            bool isOutSourceTest = common.myBool(chkOutSourceTest.Checked);
            //    DataSet ds = objval.GetResultFinalization(common.myStr(Source), common.myInt(Session["HospitalLocationID"]),
            //                 common.myInt(ddlFacility.SelectedValue), common.myInt(Session["FacilityID"]), stationID, common.myInt(ddlStatus.SelectedValue),
            //                 common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate), RegistrationNo,
            //                 0, 0, PatientName, iShowAssignedToMe, iNotFinalized, chkAssignedToMeOnly.Checked == true ? common.myInt(Session["EmployeeId"]) : common.myInt(Session["EmployeeId"]),
            //                common.myInt(ddlServiceName.SelectedValue), DdlResultType.SelectedValue,
            //                gvResultFinal.PageSize, pageindex, LabNo, common.myStr(strXML),
            //               ERrequest, common.myInt(ddlEntrySites.SelectedValue), Mlabno, common.myInt(ddlReportType.SelectedValue), EncounterNo, iIsCallFromLab, common.myInt(ddlEntrySitesActual.SelectedValue), common.myInt(ddlWard.SelectedValue), common.myInt(ViewState["ShowProfileDetailsInFinalization"]), isOutSourceTest);

            DataSet ds = objval.GetResultFinalization(common.myStr(Source), common.myInt(Session["HospitalLocationID"]),
                                    common.myInt(ddlFacility.SelectedValue), common.myInt(Session["FacilityID"]), stationID, common.myInt(ddlStatus.SelectedValue),
                                    common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate), RegistrationNo,
                                    0, 0, PatientName, iShowAssignedToMe, iNotFinalized, chkAssignedToMeOnly.Checked == true ? common.myInt(Session["EmployeeId"]) : 0,
                                    common.myInt(ddlServiceName.SelectedValue), DdlResultType.SelectedValue,
                                    gvResultFinal.PageSize, pageindex, LabNo, common.myStr(strXML),
                                    ERrequest, common.myInt(ddlEntrySites.SelectedValue), Mlabno, common.myInt(ddlReportType.SelectedValue), EncounterNo, iIsCallFromLab, common.myInt(ddlEntrySitesActual.SelectedValue), common.myInt(ddlWard.SelectedValue), common.myInt(ViewState["ShowProfileDetailsInFinalization"]), isOutSourceTest, sServiceName, common.myInt(ddlSubDepartment.SelectedValue));

            if (!ds.Tables[0].Columns.Contains("OrderDate"))
            {
                ds.Tables[0].Columns.Add("OrderDate");
            }

            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "EntrySiteId = " + common.myInt(ddlEntrySites.SelectedValue);
            if (Request.QueryString["Master"] != null) // for show result after provisional ( from ward)
            {
                //DataRow[] dr = ds.Tables[0].Select("DoNotShowInWard = True AND (AbnormalValue=True OR CriticalValue=True)");
                //if (dr.Length > 0)
                //{
                //    dr[0].Delete();
                //    ds.Tables[0].AcceptChanges();

                //}

                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "StatusCode IN ('RP','RF') AND IsRedone=0";
            }

            if (dv.Table.Rows.Count > 0)
            {
                gvResultFinal.VirtualItemCount = common.myInt(dv.Table.Rows[0]["TotalRecordsCount"]);
            }
            else
            {
                gvResultFinal.VirtualItemCount = 0;
            }
            // condition modified on 08-07-2014 Start Naushad
            if (common.myStr(ddlSource.SelectedValue) != "PACKAGE" || common.myStr(ddlSource.SelectedValue) != "A")
            {
                gvResultFinal.Columns.FindByUniqueName("PackageName").Visible = false;
            }
            else
            {
                gvResultFinal.Columns.FindByUniqueName("PackageName").Visible = true;
            }
            if (!common.myStr(Request.QueryString["MD"]).Equals("RIS"))
            {
                gvResultFinal.Columns.FindByUniqueName("ScanInTime").Visible = false;
                gvResultFinal.Columns.FindByUniqueName("ScanOuttime").Visible = false;
            }
            else
            {
                gvResultFinal.Columns.FindByUniqueName("ScanInTime").Visible = true;
                gvResultFinal.Columns.FindByUniqueName("ScanInTime").Visible = true;
                gvResultFinal.Columns.FindByUniqueName("LabSupervisor").Visible = false;
                gvResultFinal.Columns.FindByUniqueName("LabInCharge").Visible = false;
                gvResultFinal.Columns.FindByUniqueName("LabDirector").Visible = false;
            }
            if (common.myInt(ViewState["ShowProfileDetailsInFinalization"]) == 0) { gvResultFinal.Columns.FindByUniqueName("FieldName").Visible = false; } else { gvResultFinal.Columns.FindByUniqueName("FieldName").Visible = true; }
            gvResultFinal.DataSource = dv; //dv.ToTable().Copy();
            gvResultFinal.DataBind();
            SetGridColor();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            strXML = null;
            coll = null;
        }
    }

    protected void gvResultFinal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            (e.Column as GridGroupSplitterColumn).ExpandImageUrl = "../../Images/Plusbox.gif";
            (e.Column as GridGroupSplitterColumn).CollapseImageUrl = "../../Images/Minubox.gif";
            e.Column.HeaderStyle.Width = Unit.Percentage(3);
            e.Column.ItemStyle.Width = Unit.Percentage(3);
            e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            e.Column.ItemStyle.VerticalAlign = VerticalAlign.Top;
        }
    }
    protected void gvResultFinal_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvResultFinal.CurrentPageIndex = e.NewPageIndex;
        bindMainData();
    }

    protected void gvResultFinal_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        bool flag = true;
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Label lblresult = (Label)e.Item.FindControl("lblresult");
            LinkButton lnkResult = (LinkButton)e.Item.FindControl("lnkResult");
            ;
            if (lblresult.Text.Trim() == "Result")
            {
                lnkResult.CommandName = "Result";
                lnkResult.ForeColor = Color.Blue;
                lnkResult.Visible = true;
            }
            else if (lblresult.Text.Trim() == "Download")
            {
                lnkResult.CommandName = "Download";
                lnkResult.ForeColor = Color.Blue;
                lnkResult.Visible = true;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkResult);
            }

            Label lblAbnormalValue = (Label)e.Item.FindControl("lblAbnormalValue");
            Label lblCriticalValue = (Label)e.Item.FindControl("lblCriticalValue");

            if (common.myBool(lblAbnormalValue.Text) == true)
            {
                lnkResult.ForeColor = System.Drawing.Color.DarkViolet;
            }
            if (common.myBool(lblCriticalValue.Text) == true)
            {
                lnkResult.ForeColor = System.Drawing.Color.Red;
            }

            Label lblStatusID = (Label)e.Item.FindControl("lblStatusId");

            HiddenField hdnDiagSampleId = (HiddenField)e.Item.FindControl("hdnDiagSampleId");
            LinkButton lnkResultHistory = (LinkButton)e.Item.FindControl("lnkResultHistory");
            LinkButton lbtDetails = (LinkButton)e.Item.FindControl("lbtDetails");
            LinkButton lnkViewHistory = (LinkButton)e.Item.FindControl("lnkViewHistory");
            LinkButton lnkPatientViewHistory = (LinkButton)e.Item.FindControl("lnkPatientViewHistory");
            LinkButton lnkPrint = (LinkButton)e.Item.FindControl("lnkPrint");
            Label lblLabTechnician = (Label)e.Item.FindControl("lblLabTechnician");
            Label lblManualLabNo = (Label)e.Item.FindControl("lblManualLabNo");
            Label lblLabInCharge = (Label)e.Item.FindControl("lblLabInCharge");
            Label lblLabSupervisor = (Label)e.Item.FindControl("lblLabSupervisor");
            Label lblLabDoctor = (Label)e.Item.FindControl("lblLabDoctor");
            Label lblLabDirector = (Label)e.Item.FindControl("lblLabDirector");
            Label lblPackageName = (Label)e.Item.FindControl("lblPackageName");
            Label lblTAT = (Label)e.Item.FindControl("lblTAT");
            Label lblFieldName = (Label)e.Item.FindControl("lblFieldName");

            GridDataItem di = e.Item as GridDataItem;
            TableCell cell = di["chkCollection"];
            CheckBox CHK = (CheckBox)cell.Controls[0];

            CheckBox CHK13 = (CheckBox)e.Item.FindControl("chkIsPrintPatientDiagnosis");
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");

            DataView dvRE = ds.Tables[0].Copy().DefaultView;
            dvRE.RowFilter = "Code = 'RE'";

            DataView dvRP = ds.Tables[0].Copy().DefaultView;
            dvRP.RowFilter = "Code = 'RP'";

            DataView dvRF = ds.Tables[0].Copy().DefaultView;
            dvRF.RowFilter = "Code = 'RF'";
            if (common.myInt(hdnAssignDiagSampleId.Value) == common.myInt(hdnDiagSampleId.Value))
            {
                CHK13.Visible = false;
                CHK.Visible = false;
                lnkResultHistory.Visible = false;
                lnkPatientViewHistory.Visible = false;
                lbtDetails.Visible = false;
                lnkViewHistory.Visible = false;

                lblLabTechnician.Visible = false;
                lblManualLabNo.Visible = false;
                lblLabInCharge.Visible = false;
                lblLabSupervisor.Visible = false;
                lblLabDoctor.Visible = false;
                lblLabDirector.Visible = false;
                lblPackageName.Visible = false;
                lblTAT.Visible = false;
                flag = false;

                if (((Label)e.Item.FindControl("lblNotesAvailable")).Text == "1")
                {
                    ((Label)e.Item.FindControl("lblForNotes")).Visible = false;
                }
            }
            else
            {
                if (((Label)e.Item.FindControl("lblNotesAvailable")).Text == "1")
                {
                    ((Label)e.Item.FindControl("lblForNotes")).Visible = true;
                }
                CHK.Visible = true;
                HiddenField hdnIsPrintPatientDiagnosisMaster = (HiddenField)e.Item.FindControl("hdnIsPrintPatientDiagnosisMaster");
                if (common.myBool(hdnIsPrintPatientDiagnosisMaster.Value))
                {
                    CHK13.Visible = true;
                }
                else { CHK13.Visible = false; }
                lnkResultHistory.Visible = true;
                lbtDetails.Visible = true;
                lnkViewHistory.Visible = true;

                lblLabTechnician.Visible = true;
                lblManualLabNo.Visible = true;
                lblLabInCharge.Visible = true;
                lblLabSupervisor.Visible = true;
                lblLabDoctor.Visible = true;
                lblLabDirector.Visible = true;
                lblPackageName.Visible = true;
                flag = true;
            }
            hdnAssignDiagSampleId.Value = hdnDiagSampleId.Value;
            Label lblCriticalIndication = (Label)e.Item.FindControl("lblCriticalIndication");
            HiddenField hdnEncounterNo = (HiddenField)e.Item.FindControl("hdnEncounterNo");
            HiddenField hdnLabNo = (HiddenField)e.Item.FindControl("hdnLabNo");
            HiddenField hdnServiceId = (HiddenField)e.Item.FindControl("hdnServiceId");

            Label lblStatusColor = (Label)di.FindControl("lblStatusColor");
            Label lblStatusCode = (Label)di.FindControl("lblStatusCode");
            LinkButton lnkResultHTML = (LinkButton)di.FindControl("lnkResultHTML");

            di.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
            if (lblStatusCode.Text == "RF")
            {
                lblStatusCode.Visible = false;
                //lblStatusCode.Visible = CHK.Visible = false;
            }
            Label lblIsConfidential = (Label)e.Item.FindControl("lblIsConfidential");

            if ((common.myInt(lblStatusID.Text) == common.myInt(dvRE.ToTable().Rows[0]["StatusID"]) || common.myInt(lblStatusID.Text) == common.myInt(dvRP.ToTable().Rows[0]["StatusID"]) || common.myInt(lblStatusID.Text) == common.myInt(dvRF.ToTable().Rows[0]["StatusID"])))
            {
                if (common.myBool(lblIsConfidential.Text))
                {

                    if (common.myBool(ViewState["ConfidentialRight"]))
                    {
                        lnkViewHistory.Visible = true;
                        lnkPrint.Visible = true;
                    }
                    else
                    {
                        lnkViewHistory.Visible = false;
                        lnkPrint.Visible = false;
                    }
                }
            }

            if (common.myStr(lblStatusCode.Text) == "RP" && common.myStr(ViewState["IsPrintProvisional"]).Equals("N"))
            {
                lnkResultHTML.Visible = false;
                CHK.Visible = false;
                lnkResult.Visible = true; //changes
                lbtDetails.Enabled = false;
                lnkViewHistory.Enabled = false;
                lnkPatientViewHistory.Enabled = false;
                lblresult.Visible = false;
            }
            else
            {
                lnkResultHTML.Visible = true;
            }
            //palendra View Result Flag Base
            if (common.myStr(lblStatusCode.Text) == "RP" && common.myStr(ViewState["IsViewProvisional"]).Equals("N"))
            {
                lnkResult.Enabled = false; //changes
            }

            if (common.myStr(ViewState["EMRRestrictPrintResultForAllCase"]).ToUpper().Equals("Y"))
            {
                lnkViewHistory.Visible = false;
                lnkResultHTML.Visible = false;
            }
        }
        if (common.myStr(Request.QueryString["Master"]) == "WARD")
        {
            lnkConsolidateLabReport.Visible = true;
            gvResultFinal.Columns.FindByUniqueName("IsPrintPatientDiagnosis").Visible = false;
        }
    }

    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ResultHTML")
            {
                //Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                //Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                //Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                //Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                //HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                //HiddenField hdnResultHTML = (HiddenField)e.Item.FindControl("hdnResultHTML");
                //Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");

                //PrintReport(common.myInt(lblLabNo.Text), common.myInt(lblDiagSampleID.Text), common.myStr(lblServiceId.Text), common.myInt(lblRegId.Text), common.myStr(hdnSource.Value), common.myStr(hdnResultHTML.Value), common.myStr(lblStatusCode.Text));

                string strServiceIds = string.Empty;
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                //Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                HiddenField hdnResultHTML = (HiddenField)e.Item.FindControl("hdnResultHTML");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");

                foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
                {
                    TableCell cell = dataItem["chkCollection"];
                    CheckBox CHK = (CheckBox)cell.Controls[0];
                    if (CHK.Visible)
                    {
                        Label lblServiceId1 = (Label)dataItem.FindControl("lblServiceId");
                        if (strServiceIds == "")
                        {
                            strServiceIds = common.myStr(lblServiceId1.Text).Trim();
                        }
                        else
                        {
                            strServiceIds = strServiceIds + "," + common.myStr(lblServiceId1.Text).Trim();
                        }
                    }
                }
                PrintReport(common.myInt(lblLabNo.Text), common.myInt(lblDiagSampleID.Text), common.myStr(strServiceIds), common.myInt(lblRegId.Text), common.myStr(hdnSource.Value), common.myStr(hdnResultHTML.Value), common.myStr(lblStatusCode.Text));

            }
            if (e.CommandName == "Result")
            {
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblServiceId = (Label)e.Item.FindControl("lblServiceId");
                Label lblResultRemarksId = (Label)e.Item.FindControl("lblResultRemarksId");
                Label lblStatusCode = (Label)e.Item.FindControl("lblStatusCode");
                HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");

                string source = "";
                if (common.myStr(hdnSource.Value).ToUpper() == "IPD")
                {
                    source = "IPD";
                }
                else
                {
                    source = "OPD";
                }


                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/InvestigationResult.aspx?SOURCE=" + source +
                                            "&LABNO=" + common.myInt(lblLabNo.Text) + "&MASTER=Y" +
                                            "&SEL_DiagSampleID=" + common.myInt(lblDiagSampleID.Text) +
                                            "&SEL_ServiceId=" + common.myInt(lblServiceId.Text) +
                                            "&SEL_ResultRemarksId=" + common.myInt(lblResultRemarksId.Text) +
                                            "&SEL_StatusCode=" + common.myStr(lblStatusCode.Text) +
                                            "&Page=RF&FromMaster=" + common.myStr(Request.QueryString["MASTER"]) +
                                            "&MD=" + common.myStr(Request.QueryString["MD"]);

                RadWindowPopup.Height = 570;
                RadWindowPopup.Width = 850;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                //RadWindowPopup.OnClientClose = "OnClientResultEntryClose";
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
            if (e.CommandName == "Download")
            {
                string Source = common.myStr(((HiddenField)e.Item.FindControl("hdnSource")).Value);
                objval = new BaseC.clsLISPhlebotomy(sConString);
                DataSet ds = new DataSet();
                LinkButton lnkResult = (LinkButton)e.Item.FindControl("lnkResult");
                if ((Source.ToUpper() == "OPD") || (Source.ToUpper() == "PACKAGE") || (Source.ToUpper() == "ER"))
                {
                    ds = objval.FillAttachmentDownloadDropdownOP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");
                }
                else
                {
                    ds = objval.FillAttachmentDownloadDropdownIP(((Label)e.Item.FindControl("lblDiagSampleID")).Text, "");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        try
                        {
                            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
                            string key = "Word";
                            string sFileName = ds.Tables[0].Rows[0]["DocumentName"].ToString();
                            string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];
                            string path = Server.MapPath(sSavePath + sFileName);
                            string URLPath = "AttachementRender.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
                            URLPath = URLPath.Replace("@FTPFolder", en.Encrypt(ConfigurationManager.AppSettings["LabResultPath"], key, true, string.Empty)).Replace("@FileName", en.Encrypt(sFileName, key, true, string.Empty));
                            RadWindowPopup.NavigateUrl = URLPath.Replace("+", "%2B");
                            RadWindowPopup.Height = 570;
                            RadWindowPopup.Width = 850;
                            RadWindowPopup.Top = 10;
                            RadWindowPopup.Left = 10;
                            RadWindowPopup.VisibleOnPageLoad = true;
                            RadWindowPopup.Modal = true;
                            //RadWindowPopup.OnClientClose = "OnClientResultEntryClose";
                            RadWindowPopup.VisibleStatusbar = false;
                            RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;

                        }
                        catch
                        {
                        }
                    }
                    else
                    {

                        HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                        string source = "";
                        if (common.myStr(hdnSource.Value).ToUpper() == "IPD")
                        {
                            source = "IPD";
                        }
                        else
                        {
                            source = "OPD";
                        }

                        RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/Download.aspx?SampleId="
                            + ((Label)e.Item.FindControl("lblDiagSampleID")).Text + "&SOURCE=" + source;

                        RadWindowPopup.Height = 300;
                        RadWindowPopup.Width = 600;
                        RadWindowPopup.Top = 10;
                        RadWindowPopup.Left = 10;
                        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindowPopup.Modal = true;
                        RadWindowPopup.VisibleStatusbar = false;
                        RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
                else
                {
                    //  ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkResult);
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "File does not exist...";
                }
            }
            if (e.CommandName == "Details")
            {
                Label LBL = (Label)e.Item.FindControl("lblServiceId");
                Label lblDiagSampleId = (Label)e.Item.FindControl("lblDiagSampleId");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblRegNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblPatientName = (Label)e.Item.FindControl("lblPatientName");

                HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                string source = "";
                if (common.myStr(hdnSource.Value).ToUpper() == "IPD")
                {
                    source = "IPD";
                }
                else
                {
                    source = "OPD";
                }


                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/TestDetails.aspx?ServiceId=" + common.myInt(LBL.Text) + "&DiagSampleId=" + common.myInt(lblDiagSampleId.Text) + "&RegID=" + common.myStr(lblRegId.Text).Trim() + "&RegNo=" + common.myStr(lblRegNo.Text).Trim() + "&PName=" + common.myStr(lblPatientName.Text).Trim() + "&LabNo=" + common.myStr(lblLabNo.Text).Trim() + "&Source=" + source + "&MD=" + common.myStr(Request.QueryString["MD"]);

                RadWindowPopup.Height = 570;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
            if (e.CommandName == "ResultHistory")
            {
                Label LBL = (Label)e.Item.FindControl("lblServiceId");
                Label lblDiagSampleId = (Label)e.Item.FindControl("lblDiagSampleId");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblRegNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblPatientName = (Label)e.Item.FindControl("lblPatientName");
                Label lblAgeGender = (Label)e.Item.FindControl("lblAgeGender");
                LinkButton lnkResultHistory = (LinkButton)e.Item.FindControl("lnkResultHistory");

                HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                string source = string.Empty;
                if (common.myStr(hdnSource.Value).ToUpper() == "IPD")
                {
                    source = "IPD";
                }
                else
                {
                    source = "OPD";
                }
                dvPatientResultHistory.Visible = true;
                lblResultHistoryPatientName.Text = common.myStr(lblPatientName.Text).Trim() + ", " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + ": " + common.myStr(lblLabNo.Text);
                lblResultHistoryServiceName.Text = common.myStr(lnkResultHistory.Text);

                bindPatientResultHistory(common.myInt(lblRegId.Text), common.myInt(LBL.Text), common.myStr(source), common.myInt(lblDiagSampleId.Text));

                //SetGridColor();
            }
            if (e.CommandName == "VisitHistory")
            {
                Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                int iRegId = common.myInt(lblRegId.Text);
                int iLabNo = common.myInt(lblLabNo.Text);
                ViewPatientHisotry(iLabNo, iRegId);
            }
            else if (e.CommandName == "View")
            {
                Label LBL = (Label)e.Item.FindControl("lblServiceId");
                Label lblDiagSampleId = (Label)e.Item.FindControl("lblDiagSampleId");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lblRegId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblRegNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblPatientName = (Label)e.Item.FindControl("lblPatientName");
                HiddenField hdnEncounterNo = (HiddenField)e.Item.FindControl("hdnEncounterNo");
                LinkButton lnkResultHistory = (LinkButton)e.Item.FindControl("lnkResultHistory");

                HiddenField hdnSource = (HiddenField)e.Item.FindControl("hdnSource");
                string source = "";
                if (common.myStr(hdnSource.Value).ToUpper() == "IPD")
                {
                    source = "IPD";
                }
                else
                {
                    source = "OPD";
                }

                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/PatientHistory.aspx?Mpg=P607&MD=" + Flag + "&SOURCE="
                   + common.myStr(source)
                   + "&LABNO=" + common.myStr(lblLabNo.Text).Trim()
                   + "&RegId=" + common.myStr(lblRegId.Text).Trim()
                   + "&ServiceId=" + common.myInt(LBL.Text)
                   + "&ServiceName=" + common.myStr(lnkResultHistory.Text.Trim())
                   + "&PName=" + common.myStr(lblPatientName.Text).Trim()
                   + "&EncounterNo=" + common.myStr(hdnEncounterNo.Value)
                   + "&RegNo=" + common.myStr(lblRegNo.Text).Trim()
                   + "&PageID=SAck&POPUP=StaticTemplate"
                   + "&StationId=" + common.myStr(Session["StationId"]);

                //RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/TestDetails.aspx?ServiceId=" + common.myInt(LBL.Text) + "&DiagSampleId=" + common.myInt(lblDiagSampleId.Text) + "&RegID=" + common.myStr(lblRegId.Text).Trim() + "&RegNo=" + common.myStr(lblRegNo.Text).Trim() + "&PName=" + common.myStr(lblPatientName.Text).Trim() + "&LabNo=" + common.myStr(lblLabNo.Text).Trim() + "&Source=" + source;

                RadWindowPopup.Height = 570;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
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
            SetGridColor();
        }
    }

    private void ViewPatientHisotry(int iLabNo, int iRegistrationId)
    {

        if (iLabNo == 0)
        {
            if (Flag.ToString() == "RIS")
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
            else

                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
            return;
        }

        //RadWindowPopup.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?&MP=NO&RegNo=" + common.myStr(RegistrationId) + "&RegId=" + common.myInt(RegistrationId);
        RadWindowPopup.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?MD=" + Flag + "&MP=NO&CF=PTA&PId=" + iRegistrationId;
        RadWindowPopup.Height = 600;
        RadWindowPopup.Width = 900;
        RadWindowPopup.Top = 10;
        RadWindowPopup.Left = 10;
        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowPopup.Modal = true;
        RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowPopup.VisibleStatusbar = false;
    }

    private void bindControl()
    {
        try
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet dsReportType = objMaster.getReportType(common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["StationId"]));
            dsReportType.Tables[0].DefaultView.Sort = "ReportType ASC";
            ddlReportType.DataSource = dsReportType.Tables[0].DefaultView;
            ddlReportType.DataTextField = "ReportType";
            ddlReportType.DataValueField = "Id";
            ddlReportType.DataBind();
            ddlReportType.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlReportType.SelectedIndex = 0;

            BaseC.User valUser = new BaseC.User(sConString);
            objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = new DataSet();
            if (Cache["FACILITY"] == null)
            {
                ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
                Cache["FACILITY"] = ds;
            }
            ddlFacility.SelectedIndex = -1;
            ddlFacility.DataSource = (DataSet)Cache["FACILITY"];
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();
            ddlFacility.SelectedValue = common.myStr(Session["FacilityId"]);
            //  ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            //  ddlFacility.SelectedIndex = 0;

            DataTable dt = GetLegend();
            DataView dv = dt.DefaultView;
            if (Request.QueryString["Master"] == "WARD")  //condtion Added for calling page from Ward
            {
                dv.RowFilter = "Code IN('RP','RF')";
            }
            else
            {
                if (common.myStr(ViewState["DefaultResultEntryStatus"]).Equals("RP")) { dv.RowFilter = "Code IN('RP','RF')"; }
                else
                {
                    dv.RowFilter = "Code IN('RE','RP','RF')";
                }
            }
            ddlStatus.DataSource = dv;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new RadComboBoxItem("", "0"));
            if (common.myStr(Request.QueryString["Master"]).ToUpper() == string.Empty)
                ddlStatus.SelectedIndex = 1;


            objval = new BaseC.clsLISPhlebotomy(sConString);
            StringBuilder strType = new StringBuilder();
            ArrayList coll = new ArrayList();
            ds = new DataSet();
            string strEmployeeType = common.myStr(Session["EmployeeType"]);
            if (strEmployeeType == "LDIR")
            {
                coll.Add("");
                strType.Append(common.setXmlTable(ref coll));
            }
            else if (strEmployeeType == "LIC")
            {
                coll.Add("LDIR");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LS");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LD");
                strType.Append(common.setXmlTable(ref coll));
            }
            else if (strEmployeeType == "LS")
            {
                coll.Add("LDIR");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LD");
                strType.Append(common.setXmlTable(ref coll));
            }
            else if (strEmployeeType == "LD")
            {
                coll.Add("LDIR");
                strType.Append(common.setXmlTable(ref coll));

            }
            else
            {
                coll.Add("LDIR");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LIC");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LS");
                strType.Append(common.setXmlTable(ref coll));
                coll.Add("LD");
                strType.Append(common.setXmlTable(ref coll));
            }
            int iHospId = common.myInt(Session["HospitalLocationID"]);
            int iStationId = common.myInt(Session["StationId"]);
            ds = objMaster.getEmployeeData(iHospId, iStationId, 0, strType.ToString(), "", 0, common.myInt(Session["UserId"]), "", 0);
            ddlEmployee.SelectedIndex = -1;
            ddlEmployee.DataSource = ds.Tables[0].Copy();
            ddlEmployee.DataValueField = "userId";
            ddlEmployee.DataTextField = "EmployeeNameWithNo";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlEmployee.SelectedIndex = 0;
            BaseC.InvestigationFormat invService = new BaseC.InvestigationFormat(sConString);
            DataSet objDs = invService.GetInvestigationServices("", "'IS','I'", common.myStr(Session["HospitalLocationID"]), common.myInt(Session["StationId"]), common.myInt(Session["FacilityId"]));
            objDs.Tables[0].DefaultView.Sort = "ServiceName Asc";
            ddlServiceName.DataSource = objDs.Tables[0].DefaultView;
            ddlServiceName.DataValueField = "ServiceID";
            ddlServiceName.DataTextField = "ServiceName";
            ddlServiceName.DataBind();
            ddlServiceName.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlServiceName.SelectedIndex = 0;

            if (Cache["ENTRYSITES"] == null || ((DataSet)Cache["ENTRYSITES"]).Tables[0].Rows.Count == 0)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                DataSet dsENTRYSITES = objval.getEntrySites(common.myInt(Session["HospitalLocationID"]));
                Cache["ENTRYSITES"] = dsENTRYSITES;
            }

            DataView dvEntry = ((DataSet)Cache["ENTRYSITES"]).Tables[0].DefaultView;
            dvEntry.RowFilter = " StationId  = " + common.myInt(Session["StationId"]) + " and FacilityId = " + common.myInt(Session["FacilityId"]) + " AND IsResultEntryLocation = 1";
            ddlEntrySites.DataSource = ((DataTable)dvEntry.ToTable());
            ddlEntrySites.DataValueField = "EntrySiteId";
            ddlEntrySites.DataTextField = "EntrySiteName";
            ddlEntrySites.DataBind();
            //  ddlEntrySites.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlEntrySites.SelectedIndex = 0;

            ddlReleaseStage.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlReleaseStage.SelectedIndex = 0;
            BaseC.clsEMRBilling objval1 = new BaseC.clsEMRBilling(sConString);
            DataSet DSS = objval1.getEntrySite(common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));
            ddlEntrySitesActual.DataSource = DSS;
            ddlEntrySitesActual.DataValueField = "ESId";
            ddlEntrySitesActual.DataTextField = "ESName";
            ddlEntrySitesActual.DataBind();
            //ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlEntrySitesDiag.SelectedIndex = 0;

            ddlEntrySitesActual.SelectedIndex = 0;
            //ddlEntrySitesActual.SelectedIndex = ddlEntrySitesActual.Items.IndexOf(ddlEntrySitesActual.Items.FindItemByValue(common.myStr(Session["EntrySite"])));

            ds = new DataSet();

            BaseC.ATD objadt = new BaseC.ATD(sConString);
            ds = objadt.GetReportTypes(common.myInt(Session["HospitalLocationId"]), "Ward", common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "Name";
            ddlWard.DataValueField = "ID";
            ddlWard.DataBind();
            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlWard.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void SetGridColor()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            DataSet ds = new DataSet();
            foreach (GridDataItem dataItem in gvResultFinal.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
                    if (((Label)dataItem.FindControl("lblStat")).Text.ToUpper().Equals("TRUE"))
                    {
                        ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Stat");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dataItem.Cells[4].BackColor = System.Drawing.Color.FromName(common.myStr(ds.Tables[0].Rows[0]["StatusColor"]));
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
            // Legend1.loadLegend("LabOthers", "'Stat'");
        }
    }

    private DataTable GetLegend()
    {
        DataTable DT = new DataTable();
        DataSet ds = new DataSet();
        if (Cache["LEGEND"] == null)
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
            Cache["LEGEND"] = ds;
        }
        DataView dv = ((DataSet)Cache["LEGEND"]).Tables[0].DefaultView;
        string strStatusType = "'RE','RP','RF'";
        dv.RowFilter = "Code IN(" + strStatusType + ")";
        DT = dv.ToTable();
        return DT;
    }

    protected void btnRefresh_OnClick(Object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        gvResultFinal.CurrentPageIndex = 0;
        ViewState["SelRow"] = "";

        bindMainData();
    }
    private bool IsValidSearchCriteria()
    {
        bool IsValid = false;
        try
        {
            if (common.myLen(txtSearchCretriaNumeric.Text) > 0 || common.myLen(txtSearchCretria.Text) > 0)
            {
                IsValid = true;
            }
            else
            {
                TimeSpan timeS = common.myDate(txtToDate.SelectedDate.Value) - common.myDate(txtFromDate.SelectedDate.Value);

                if ((timeS.TotalDays + 1) > 10)
                {
                    Alert.ShowAjaxMsg("Search by criteria is mandatory, If the date range for more than 10 days!", this.Page);
                    txtSearchCretriaNumeric.Focus();
                }
                else
                {
                    IsValid = true;
                }
            }
        }
        catch
        {
        }
        return IsValid;
    }
    private void BindMainBlankGrid()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("LabNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("FacilityName");
            dt.Columns.Add("PatientName");
            dt.Columns.Add("ManualLabNo");
            dt.Columns.Add("EncounterNo");
            dt.Columns.Add("AgeGender");
            dt.Columns.Add("ReferredDoctor");
            dt.Columns.Add("ServiceName");
            dt.Columns.Add("ServiceId");
            dt.Columns.Add("Source");
            dt.Columns.Add("DiagSampleId");
            dt.Columns.Add("FieldName");
            dt.Columns.Add("LabTechnician");
            dt.Columns.Add("LabInCharge");
            dt.Columns.Add("LabSupervisor");
            dt.Columns.Add("LabDoctor");
            dt.Columns.Add("LabDirector");
            dt.Columns.Add("PackageName");
            dt.Columns.Add("Result");
            dt.Columns.Add("NotesAvailable");
            dt.Columns.Add("RegistrationId");
            dt.Columns.Add("ResultAlert");
            dt.Columns.Add("StatusColor");
            dt.Columns.Add("StatusID");
            dt.Columns.Add("AbnormalValue");
            dt.Columns.Add("CriticalValue");
            dt.Columns.Add("ResultRemarksId");
            dt.Columns.Add("StatusCode");
            dt.Columns.Add("Print");
            dt.Columns.Add("OrderDate");

            gvResultFinal.VirtualItemCount = 0;
            gvResultFinal.DataSource = dt;
            gvResultFinal.DataBind();
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

    private bool isSaveCancel()
    {
        bool isSave = true;
        string strmsg = "";
        if (common.myStr(ddlReleaseStage.SelectedValue) == "F")
        {
            //if (common.myStr(Session["ReportingStage"]) != "F")
            //{
            //    strmsg += "Not Authorized To Finalize Results!<br />";
            //    lblMessage.Text = strmsg;
            //    return isSave = false;
            //}
        }
        if (common.myStr(ddlReleaseStage.SelectedValue) == "P" || common.myStr(ddlReleaseStage.SelectedValue) == "")
        {
            string strEmployeeType = common.myStr(Session["EmployeeType"]);
            if (strEmployeeType != "LDIR" && strEmployeeType != "LD")
            {
                if (ddlEmployee.SelectedValue == null || ddlEmployee.SelectedValue == "0")
                {
                    strmsg += "Please Select Assign To employee !";
                    ddlEmployee.Focus();
                    isSave = false;
                }
            }
        }
        if (common.myStr(ddlSource.SelectedValue) == "")
        {
            strmsg += " Please Select Source !";
            isSave = false;
        }
        if (gvResultFinal.SelectedItems.Count < 1)
        {
            strmsg += " Please Select Sample !";
            isSave = false;
        }
        lblMessage.Text = strmsg;

        return isSave;
    }

    protected void btnTatDelayFinalization_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["Check"] = "Save";
            if (!isSaveCancel())
            {
                SetGridColor();
                return;
            }


            //if (Flag == "RIS")
            if (hdnTatDealyReason.Value != "")
            {
                if (common.myBool(Session["multipledoctor"]))
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
                    Session["EncodedBy"] = common.myStr(Session["UserId"]);
                    SaveRecord();
                    bindMainData();
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
    protected void btnResultFinalization_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["Check"] = "Save";
            if (!isSaveCancel())
            {
                SetGridColor();
                return;
            }


            //if (Flag == "RIS")
            if (!ValidateTATDelay())
            {
                Session["EncodedBy"] = common.myStr(Session["UserId"]);
                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/TatDelayReason.aspx";
                RadWindowPopup.Height = 200;
                RadWindowPopup.Width = 480;
                RadWindowPopup.Top = 50;
                RadWindowPopup.Left = 100;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.Behaviors = WindowBehaviors.Move | WindowBehaviors.Close;
                RadWindowPopup.OnClientClose = "onClientSaveTatDelayReason";

            }
            else
            {
                if (common.myBool(Session["multipledoctor"]))
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
                    Session["EncodedBy"] = common.myStr(Session["UserId"]);
                    SaveRecord();
                    bindMainData();
                }
            }
            //if (common.myStr(ddlReleaseStage.SelectedValue) == "F")
            //{
            //    string strTest = "0";
            //    if (common.myInt(strTest) == 0) //User Authentication
            //    {
            //        Session["UserAuthenticated"] = 0;
            //        RadWindowPopup.NavigateUrl = "~/Authenticate.aspx?CurrentUserAuthentication=Yes";
            //        RadWindowPopup.Height = 100;
            //        RadWindowPopup.Width = 280;
            //        RadWindowPopup.Top = 50;
            //        RadWindowPopup.Left = 100;
            //        RadWindowPopup.Modal = true;
            //        RadWindowPopup.VisibleOnPageLoad = true;
            //        RadWindowPopup.VisibleStatusbar = false;
            //        RadWindowPopup.Behaviors = WindowBehaviors.Move | WindowBehaviors.Close;
            //        RadWindowPopup.OnClientClose = "OnClientAuthenticationSaveClose";
            //    }
            //    else
            //    {
            //        Session["EncodedBy"] = common.myStr(Session["UserId"]);
            //        SaveRecord();
            //    }
            //}
            //else
            //{

            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnAuthenticateSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(Session["UserAuthenticated"]) == 1) // Authentication passed
            {
                if (common.myStr(ViewState["Check"]) == "Save")
                {
                    SaveRecord();
                }
                else
                {
                    Cancel();
                }
                Session["UserAuthenticated"] = 0;
                bindMainData();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Authentication failed! Release finalization failed...";
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

    private bool ValidateTATDelay()
    {
        if (ViewState["IsTATReasonValidate"] != null && common.myStr(ViewState["IsTATReasonValidate"]) == "Y")
        {
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                Label lblServiceId = (Label)dataItem.FindControl("lblTAT");
                if (lblServiceId.Text != "00:00")
                {

                    return false;

                }



            }
        }
        return true;
    }
    private void SaveRecord()
    {
        try
        {
            string strhdnTatDelayReason = hdnTatDealyReason.Value;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            ViewState["Check"] = "Save";
            StringBuilder strXML = new StringBuilder();

            ArrayList xmlServiceId = new ArrayList();
            ArrayList xmlServiceDesc = new ArrayList();
            StringBuilder strServiceId = new StringBuilder();
            StringBuilder strServiceDesc = new StringBuilder();

            ArrayList coll = new ArrayList();
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                HiddenField hdnResulAlert = (HiddenField)dataItem.FindControl("hdnResulAlert");
                Label lblServiceId = (Label)dataItem.FindControl("lblServiceId");
                Label lblDiagSampleID = (Label)dataItem.FindControl("lblDiagSampleID");
                Label lblLabNo = (Label)dataItem.FindControl("lblLabNo");
                Label lblRegistrationId = (Label)dataItem.FindControl("lblRegistrationId");

                //If Service result Alert is set true
                if ((common.myInt(hdnResulAlert.Value) == 1) || (common.myStr(hdnResulAlert.Value) == "True"))
                {
                    xmlServiceId.Add(common.myInt(lblServiceId.Text));

                    xmlServiceDesc.Add(common.myInt(lblServiceId.Text));
                    xmlServiceDesc.Add(common.myInt(0));
                    xmlServiceDesc.Add(common.myInt(0));
                    xmlServiceDesc.Add(common.myInt(lblDiagSampleID.Text));
                    xmlServiceDesc.Add(common.myInt(lblLabNo.Text));
                    xmlServiceDesc.Add(common.myInt(lblRegistrationId.Text));
                    xmlServiceDesc.Add(common.myInt(Session["FacilityId"]));

                    strServiceDesc.Append(common.setXmlTable(ref xmlServiceDesc));
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

                        RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/CalculationCheckShow.aspx";
                        RadWindowPopup.Height = 250;
                        RadWindowPopup.Width = 600;
                        RadWindowPopup.Top = 45;
                        RadWindowPopup.Left = 10;
                        RadWindowPopup.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize | WindowBehaviors.Pin | WindowBehaviors.Minimize;
                        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindowPopup.Modal = true;
                        RadWindowPopup.VisibleStatusbar = false;
                        return;
                    }
                }
            }







            DataSet dsColor = new DataSet();
            if (Cache["LEGEND"] == null)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                dsColor = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
                Cache["LEGEND"] = dsColor;
            }
            dsColor = (DataSet)Cache["LEGEND"];
            dsColor.Tables[0].DefaultView.RowFilter = "Code='RF'";

            string OPIP = "", IsPrintPatientDiagnosis = string.Empty;
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];
                Label lblStatusID = (Label)dataItem.FindControl("lblStatusId");

                HiddenField hdnTatlimitationV = (HiddenField)dataItem.FindControl("hdnTatlimitation");
                HiddenField hdnSampleCollectedDateV = (HiddenField)dataItem.FindControl("hdnSampleCollectedDate");

                HiddenField hdnSource = (HiddenField)dataItem.FindControl("hdnSource");
                CheckBox chkPrintDiag = (CheckBox)dataItem.FindControl("chkIsPrintPatientDiagnosis");
                if (hdnSource.Value == "IPD")
                {
                    OPIP = "IPD";
                }
                else
                {
                    OPIP = "OPD";
                }
                if (common.myBool(chkPrintDiag.Checked)) { IsPrintPatientDiagnosis = "1"; } else { IsPrintPatientDiagnosis = "0"; }
                //IsPrintPatientDiagnosis
                if (CHK.Visible)
                {
                    if (common.myInt(lblStatusID.Text) != common.myInt(dsColor.Tables[0].DefaultView[0]["StatusID"]))
                    {
                        Label lblDiagSampleID = (Label)dataItem.FindControl("lblDiagSampleID");
                        if (lblDiagSampleID.Text != "")
                        {
                            coll.Add(common.myInt(lblDiagSampleID.Text));
                            coll.Add(OPIP);
                            coll.Add(IsPrintPatientDiagnosis);
                            coll.Add(hdnTatlimitationV.Value);
                            coll.Add(hdnSampleCollectedDateV.Value);
                            strXML.Append(common.setXmlTable(ref coll));

                        }
                    }
                }
            }
            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            //if (common.myStr(ddlReleaseStage.SelectedValue) != "F")
            //{
            //    ViewState["LeftDoctorId"] = common.myInt(Session["EncodedBy"]);
            //}
            int flagTatValidate = 0;

            if (ViewState["IsTATReasonValidate"] != null && common.myStr(ViewState["IsTATReasonValidate"]) == "Y")
            {
                flagTatValidate = 1;
            }
            string strSource = ddlSource.SelectedValue.ToString().Trim();
            if ((strSource == "PACKAGE") || (strSource == "ER"))
                strSource = "OPD";
            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strMsg = objval.SaveResultFinalization(common.myStr(strSource), strXML.ToString(), common.myInt(Session["EncodedBy"]), common.myStr(ddlReleaseStage.SelectedValue), common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["StationId"]), common.myInt(ViewState["CenterDoctorId"]), common.myInt(ViewState["RightDoctorId"]), common.myInt(ViewState["LeftDoctorId"]), common.myInt(strhdnTatDelayReason), flagTatValidate, common.myInt(ddlFacility.SelectedValue));
            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["LeftDoctorId"] = "";
                ViewState["RightDoctorId"] = "";
                ViewState["CenterDoctorId"] = "";
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

    protected void ddlReleaseStage_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlReleaseStage.SelectedValue == "F")
        {

            if (ddlEmployee.SelectedIndex != -1)
            {
                ddlEmployee.SelectedIndex = -1;
            }
            ddlEmployee.Enabled = false;
        }
        else
        {
            ddlEmployee.Enabled = true;
        }
    }

    protected void btnCancelResult_OnClick(object sender, EventArgs e)
    {
        if (!validate())
        {
            return;
        }
        if (common.myStr(ViewState["IsEnableUserAuthentication"]) == "Y")
        {
            Session["UserAuthenticated"] = 0;
            ViewState["Check"] = "Cancel";
            RadWindowPopup.NavigateUrl = "~/Authenticate.aspx?CurrentUserAuthentication=Yes";
            RadWindowPopup.Height = 120;
            RadWindowPopup.Width = 320;
            RadWindowPopup.Top = 40;
            RadWindowPopup.Left = 100;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.VisibleStatusbar = false;
            RadWindowPopup.Behaviors = WindowBehaviors.Move;
            RadWindowPopup.OnClientClose = "OnClientAuthenticationSaveClose";
        }
        else
        {
            Cancel();
            bindMainData();
        }

    }

    public bool validate()
    {
        bool isSave = true;
        try
        {
            string strmsg = "";
            if ((common.myStr(Session["CancelFinalizedResult"]) == "0") || (common.myStr(Session["CancelFinalizedResult"]) == "False"))
            {
                strmsg += "Not Authorized To Cancel Results!<br />";
                lblMessage.Text = strmsg;
                return isSave = false;
            }
            int i = 0;
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    i = 1;
                }
            }
            if (i == 0)
            {
                strmsg += "Please Select Sample!<br />";
                lblMessage.Text = strmsg;
                return isSave = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        return isSave;
    }

    public void Cancel()
    {
        try
        {
            Session["EncodedBy"] = common.myStr(Session["UserId"]);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            bool CancelResultEntry = true;

            DataSet dsColor = new DataSet();
            if (Cache["LEGEND"] == null)
            {
                objval = new BaseC.clsLISPhlebotomy(sConString);
                dsColor = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LAB", "");
                Cache["LEGEND"] = dsColor;
            }
            dsColor = (DataSet)Cache["LEGEND"];
            dsColor.Tables[0].DefaultView.RowFilter = "Code='RF'";

            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];
                Label lblDiagSampleID = (Label)dataItem.FindControl("lblDiagSampleID");
                Label lblStatusCode = (Label)dataItem.FindControl("lblStatusCode");
                Label lblStatusID = (Label)dataItem.FindControl("lblStatusId");
                HiddenField hdnSource = (HiddenField)dataItem.FindControl("hdnSource");

                if (common.myInt(lblStatusID.Text) != common.myInt(dsColor.Tables[0].DefaultView[0]["StatusID"]))
                {
                    if (common.myStr(ViewState["Check"]) == "CancelProvisional")
                    {
                        CancelResultEntry = false;
                        if (lblStatusCode.Text != "RP")
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Please Select Provisional Result only !";
                            return;
                        }
                        else
                        {

                            if (lblDiagSampleID.Text != "")
                            {
                                coll.Add(common.myInt(lblDiagSampleID.Text));
                                if (common.myStr(hdnSource.Value) == "IPD")
                                    coll.Add(common.myStr(hdnSource.Value));
                                else
                                    coll.Add(common.myStr("OPD"));
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                        }
                    }
                    else
                    {
                        if (CHK.Checked == true && CHK.Visible == true)
                        {
                            if (lblDiagSampleID.Text != "")
                            {
                                coll.Add(common.myInt(lblDiagSampleID.Text));
                                if (common.myStr(hdnSource.Value) == "IPD")
                                    coll.Add(common.myStr(hdnSource.Value));
                                else
                                    coll.Add(common.myStr("OPD"));
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                        }
                    }
                }
            }

            if (strXML.Length == 0)
            {
                lblMessage.Text = "Please Select Sample !";
                return;
            }
            objval = new BaseC.clsLISPhlebotomy(sConString);
            string strSource = ddlSource.SelectedValue.ToString().Trim();
            if ((strSource == "PACKAGE") || (strSource == "ER"))
                strSource = "OPD";

            string strMsg = objval.CancelResultFinalization(common.myStr(strSource), common.myInt(Session["StationId"]), common.myInt(Session["HospitalLocationID"]), strXML.ToString(), common.myStr(""), common.myInt(Session["EncodedBy"]), CancelResultEntry, false, true);

            ViewState["Check"] = "";
            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            }
            lblMessage.Text = strMsg;
            strXML = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if (ddlSubDepartment.SelectedValue != "-1")
            if (ddlSubDepartment.SelectedIndex >= 1)
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
            else if (ddlSubDepartment.SelectedIndex < 1)
            {
                fillSubDepartment();
                bindMainData();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void BtnCancelProvisionalResult_OnClick(object sender, EventArgs e)
    {
        string strTest = "0";

        if (!validate())
        {
            return;
        }
        if (common.myStr(ViewState["IsEnableUserAuthentication"]) == "Y") //User Authentication
        {
            Session["UserAuthenticated"] = 0;
            ViewState["Check"] = "CancelProvisional";
            RadWindowPopup.NavigateUrl = "~/Authenticate.aspx?CurrentUserAuthentication=Yes";
            RadWindowPopup.Height = 120;
            RadWindowPopup.Width = 320;
            RadWindowPopup.Top = 40;
            RadWindowPopup.Left = 100;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.VisibleStatusbar = false;
            RadWindowPopup.Behaviors = WindowBehaviors.Move;
            RadWindowPopup.OnClientClose = "OnClientAuthenticationSaveClose";
        }
        else
        {
            ViewState["Check"] = "CancelProvisional";
            Cancel();
            Session["UserAuthenticated"] = 0;
            bindMainData();
        }
    }

    //Link Button Relay Click Event
    protected void lnkRelayDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            int labNo = 0;

            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {
                    Label lblLabNo = (Label)dataItem.FindControl("lblLabNo");
                    if (lblLabNo.Text != "")
                    {
                        labNo = common.myInt(lblLabNo.Text);
                        break;
                    }
                }
            }

            lblMessage.Text = "";
            if (common.myInt(labNo) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else

                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            string Source = common.myStr(ddlSource.SelectedValue);
            if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            {
                Source = "OPD";
            }

            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/RelayDetails.aspx?MD=" + Flag + "&SOURCE=" + common.myStr(Source) + "&LABNO=" + common.myInt(labNo);
            RadWindowPopup.Height = 560;
            RadWindowPopup.Width = 900;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
            strXML = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    //Link Button Package Click Event
    protected void lnkPackageDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();
            int labNo = 0, RegistrationId = 0;
            string PatientName = "";
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {

                    Label lblRegistrationId = (Label)dataItem.FindControl("lblRegistrationId");
                    Label lblLabNo = (Label)dataItem.FindControl("lblLabNo");
                    Label lblPatientName = (Label)dataItem.FindControl("lblPatientName");

                    if (lblLabNo.Text != "")
                    {
                        labNo = common.myInt(lblLabNo.Text);
                        RegistrationId = common.myInt(lblRegistrationId.Text);
                        PatientName = lblPatientName.Text;
                        break;
                    }
                }
            }
            if (common.myInt(labNo) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else

                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }
            lblMessage.Text = "";
            string Source = common.myStr(ddlSource.SelectedValue);
            if ((ddlSource.SelectedValue == "PACKAGE") || (ddlSource.SelectedValue == "ER"))
            {
                Source = "OPD";
            }
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

            RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/PackageDetails.aspx?SOURCE=" + common.myStr(Source)
                + "&EncounterNo=" + common.myStr("")
                + "&RegNo=" + common.myStr(RegistrationId)
                + "&PName=" + common.myStr(PatientName).Trim()
                + "&LABNO=" + common.myInt(labNo)
                + "&FacilityName=" + common.myStr(DefaultFacilityName);
            RadWindowPopup.Height = 620;
            RadWindowPopup.Width = 900;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
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
            if (ddlSearch.Items.Count > 3)
            {
                ddlSearch.Items.Remove(3);
                if (Flag != common.myStr("RIS"))
                    ddlSearch.SelectedValue = "LN";
                else
                    ddlSearch.SelectedValue = "RN";
                txtSearchCretria.Text = "";
            }
        }
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "WARD")
        {
            ddlSearch.SelectedValue = "RN";
        }
        bindMainData();
    }

    protected void btnEmployee_Click(object sender, EventArgs e)
    {
        ViewState["LeftDoctorId"] = hdnLeftDoctor.Value;
        ViewState["RightDoctorId"] = hdnRightDoctor.Value;
        ViewState["CenterDoctorId"] = hdnCenterDoctor.Value;
        if ((common.myInt(hdnLeftDoctor.Value) != 0) || (common.myInt(hdnRightDoctor.Value) != 0) || (common.myInt(hdnCenterDoctor.Value) != 0))
        {
            SaveRecord();
        }
    }
    protected void lnkConsolidateLabReport_OnClick(object sender, EventArgs e)
    {
        RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/ConsolidateLabReport.aspx?RT=CLR&Master=NO&EncNo=" + common.myStr(Session["Encno"])
            + "&RegNo=" + common.myInt(Session["RegistrationNo"]);// +"&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");


        RadWindowPopup.Height = 600;
        RadWindowPopup.Width = 900;
        RadWindowPopup.Top = 10;
        RadWindowPopup.Left = 10;
        RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowPopup.Modal = true;
        RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowPopup.VisibleStatusbar = false;

        SetGridColor();
    }
    protected void lnkVisitHistory_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            int labNo = 0, RegistrationId = 0;
            string PatientName = "";
            foreach (GridDataItem dataItem in gvResultFinal.SelectedItems)
            {
                TableCell cell = dataItem["chkCollection"];
                CheckBox CHK = (CheckBox)cell.Controls[0];

                if (CHK.Checked == true && CHK.Visible == true)
                {

                    Label lblRegistrationId = (Label)dataItem.FindControl("lblRegistrationId");
                    Label lblLabNo = (Label)dataItem.FindControl("lblLabNo");
                    Label lblPatientName = (Label)dataItem.FindControl("lblPatientName");

                    if (lblLabNo.Text != "")
                    {
                        labNo = common.myInt(lblLabNo.Text);
                        RegistrationId = common.myInt(lblRegistrationId.Text);
                        PatientName = lblPatientName.Text;
                        break;
                    }
                }
            }
            if (common.myInt(labNo) == 0)
            {
                if (Flag.ToString() == "RIS")
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")) + " ! ";
                else

                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " ! ";
                return;
            }

            RadWindowPopup.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?&MP=NO&RegNo=" + common.myStr(RegistrationId) + "&RegId=" + common.myInt(RegistrationId);


            RadWindowPopup.Height = 600;
            RadWindowPopup.Width = 900;
            RadWindowPopup.Top = 10;
            RadWindowPopup.Left = 10;
            //RadWindow1.OnClientClose = "wndAddService_OnClientClose";
            RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowPopup.Modal = true;
            RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowPopup.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnprint_OnClick(object sender, EventArgs e)
    {
        string strServiceIds = "";
        int iLabNo = 0;
        string sStatusCode = "";
        ArrayList coll = new ArrayList();
        StringBuilder strXml = new StringBuilder();
        string source = common.myStr(ddlSource.SelectedValue);
        foreach (GridDataItem item in gvResultFinal.Items)
        {
            TableCell cell = item["chkCollection"];
            CheckBox CHK = (CheckBox)cell.Controls[0];
            if (!CHK.Checked || CHK.Visible == false)
            {
                continue;
            }
            Label lblServiceId = (Label)item.FindControl("lblServiceId");
            Label lblDiagSampleID = (Label)gvResultFinal.SelectedItems[0].FindControl("lblDiagSampleID");
            int SampleId = common.myInt(lblDiagSampleID.Text);
            coll.Add(common.myStr(lblServiceId.Text));
            coll.Add(common.myStr(SampleId));

            strXml.Append(common.setXmlTable(ref coll));
            if (strServiceIds == "")
            {
                strServiceIds = common.myStr(lblServiceId.Text).Trim();
            }
            else
            {
                strServiceIds = strServiceIds + "," + common.myStr(lblServiceId.Text).Trim();
            }
        }

        if (strServiceIds == "")
        {
            lblMessage.Text = "Please Select Service(s) !";
            SetGridColor();
            return;
        }
        else
        {
            int iSampleId = 0;
            if (gvResultFinal.SelectedItems.Count > 0)
            {
                Label lblLabNo = (Label)gvResultFinal.SelectedItems[0].FindControl("lblLabNo");
                iLabNo = common.myInt(lblLabNo.Text);

                Label lblDiagSampleID = (Label)gvResultFinal.SelectedItems[0].FindControl("lblDiagSampleID");
                iSampleId = common.myInt(lblDiagSampleID.Text);

                Label lblStatusCode = (Label)gvResultFinal.SelectedItems[0].FindControl("lblStatusCode");
                sStatusCode = common.myStr(lblStatusCode.Text);
            }
            if (iLabNo == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            BaseC.clsLISPhlebotomy objclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
            objclsLISPhlebotomy.UpdateprintStatus(common.myStr(source), common.myInt(iLabNo), common.myStr(strXml.ToString()), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]));


            //if (common.myStr(ddlSource.SelectedValue) != "IPD")
            //{
            // source = "OPD";
            //}

            if (common.myStr(sStatusCode).Trim().ToUpper().Equals("RP"))   //Added on 09032020
            {
                string URLPath = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + source +
                    "&LABNO=" + iLabNo +
                    "&ServiceIds=" + strServiceIds +
                    "&StationId=" + common.myInt(Session["StationId"]) +
                    "&Flag=" + Flag +
                    "&DiagSampleId=" + common.myInt(0) +
                    "&pagecode=" + common.myStr("RF");
                URLPath = URLPath.Replace("+", "%2B") + "#toolbar=0";
                Session["src"] = URLPath;
                RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/ViewProvisionalLabResults.aspx";
            }
            else
            {
                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + source +
                "&LABNO=" + iLabNo +
                "&ServiceIds=" + strServiceIds +
                "&StationId=" + common.myInt(Session["StationId"]) +
                "&Flag=" + Flag +
                "&DiagSampleId=" + common.myInt(0) +
                "&pagecode=" + common.myStr("RF");
            }

            RadWindowPopup.Height = 550;
            RadWindowPopup.Width = 800;
            RadWindowPopup.Top = 45;
            RadWindowPopup.Left = 10;
            RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowPopup.VisibleOnPageLoad = true;
            RadWindowPopup.Modal = true;
            RadWindowPopup.VisibleStatusbar = false;
        }
    }
    private void PrintReport(int iLabNo, int iSampleId, string strServiceIds, int iRegId, string sSource, string sResultHTML, string lblStatusCode)
    {

        if (strServiceIds == "")
        {
            lblMessage.Text = "Please Select Service(s) !";
            SetGridColor();
            return;
        }
        else
        {
            if (sResultHTML == "False")
            {
                if (iLabNo == 0)
                {
                    lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                    return;
                }
                BaseC.clsLISPhlebotomy objclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);

                if (common.myStr(lblStatusCode).Trim().ToUpper().Equals("RP"))   //Added on 09032020
                {
                    string URLPath = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + sSource +
                                            "&LABNO=" + iLabNo +
                                            "&ServiceIds=" + strServiceIds +
                                            "&StationId=" + common.myInt(Session["StationId"]) +
                                            "&Flag=" + Flag +
                                            "&DiagSampleId=" + common.myInt(iSampleId) +
                                            "&pagecode=" + common.myStr("RF") +
                                        "&intSampleStatusId=" + common.myInt(ddlStatus.SelectedValue)
                                        ; ;
                    URLPath = URLPath.Replace("+", "%2B") + "#toolbar=0";
                    Session["src"] = URLPath;
                    RadWindowPopup.NavigateUrl = "~/LIS/Phlebotomy/ViewProvisionalLabResults.aspx";
                }
                else
                {
                    RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/PrintLabReport.aspx?SOURCE=" + sSource +
                                            "&LABNO=" + iLabNo +
                                            "&ServiceIds=" + strServiceIds +
                                            "&StationId=" + common.myInt(Session["StationId"]) +
                                            "&Flag=" + Flag +
                                            "&DiagSampleId=" + common.myInt(iSampleId) +
                                            "&pagecode=" + common.myStr("RF") +
                                        "&intSampleStatusId=" + common.myInt(ddlStatus.SelectedValue)
                                        ;
                }
                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;
                RadWindowPopup.Left = 10;
                RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
            }
            else if (common.myInt(iRegId) > 0)
            {
                ArrayList coll = new ArrayList();
                StringBuilder objXML = new StringBuilder();
                coll.Add(common.myStr(iSampleId));
                coll.Add(common.myStr(sSource));
                coll.Add(common.myStr(strServiceIds));
                objXML.Append(common.setXmlTable(ref coll));

                if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                {

                    RadWindowPopup.NavigateUrl = "/EMRReports/PrintPdfForHTMLVenkateshwar.aspx?SOURCE=" + sSource +
                                  "&LABNO=" + iLabNo +
                                   "&ServiceIds=" + common.myStr(strServiceIds) +
                                   "&StationId=" + common.myInt(Session["StationId"]) +
                                   "&Flag=" + Flag +
                                   "&RegId=" + common.myInt(iRegId) +
                                    // "&EncId=" + common.myInt(ViewState["EncounterId"]) +
                                    "&EncId=" + common.myInt(Session["EncounterId"]) +
                                   // "&EncId=190021" +
                                   "&EMail=" +
                                   "&iSampleId=" + common.myStr(iSampleId) +
                                   "&DiagSampleId=" + objXML;
                }
                else
                {
                    RadWindowPopup.NavigateUrl = "/EMRReports/PrintPdfForHTML.aspx?SOURCE=" + sSource +
                                   "&LABNO=" + iLabNo +
                                   "&ServiceIds=" + common.myStr(strServiceIds) +
                                   "&StationId=" + common.myInt(Session["StationId"]) +
                                   "&Flag=" + Flag +
                                   "&RegId=" + common.myInt(iRegId) +
                                   "&EncId=" + common.myInt(ViewState["EncounterId"]) +
                                   "&EMail=" +
                                   "&iSampleId=" + common.myStr(iSampleId) +
                                   "&DiagSampleId=" + objXML;
                }


                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 800;
                RadWindowPopup.Top = 45;
                RadWindowPopup.Left = 10;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
            }
        }

    }
    protected void ddlSearch_OnTextChanged(object sender, EventArgs e)
    {
        txtSearchCretria.Text = "";
        txtSearchCretriaNumeric.Text = "";

        if (common.myStr(ddlSearch.SelectedValue) == "LN" || common.myStr(ddlSearch.SelectedValue) == "RN")
        {
            txtSearchCretria.Visible = false;
            txtSearchCretriaNumeric.Visible = true;
        }
        else
        {
            txtSearchCretria.Visible = true;
            txtSearchCretriaNumeric.Visible = false;
        }

    }

    //It will be come in BaseC-clsLISPhlebotomy.cs
    //Added by rakesh to Get Patient Has Critical Parameter exists
    public string GetPatientHasCriticalParameter(string EncounterNo, int HospitalLocationID, int FacilityId, int intEncodedBy, int labNo, int serviceId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@EncounterNo", EncounterNo);
        hshInput.Add("@intHospitalLocationid", HospitalLocationID);
        hshInput.Add("@intFacilityID", FacilityId);
        hshInput.Add("@intEncodedBy", intEncodedBy);
        hshInput.Add("@LabNo", labNo);
        hshInput.Add("@ServiceId", serviceId);
        string strResult = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "uspDiagGetPatientHasCriticalParameter", hshInput);
        return strResult;
    }
    //Added by rakesh to Get Patient Has Critical Parameter exists

    protected void btnResultHistoryClose_OnClick(object sender, EventArgs e)
    {
        dvPatientResultHistory.Visible = false;
        SetGridColor();
    }

    private void bindPatientResultHistory(int RegistrationId, int ServiceId, string sSource, int iDiagSampleId)
    {
        BaseC.clsLISPhlebotomy objLis = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objLis.getPatientLabResultHistory(common.myDate(txtFromDate.SelectedDate),
                                common.myDate(txtToDate.SelectedDate), RegistrationId, ServiceId, 5, sSource, iDiagSampleId);

            if (ds.Tables.Count > 0)
            {
                if (gvPatientResultHistory.Columns.Count > 0)
                {
                    gvPatientResultHistory.Columns.Clear();
                }
                gvPatientResultHistory.DataSource = ds;
                gvPatientResultHistory.AutoGenerateColumns = false;

                GridBoundColumn boundColumn;
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    boundColumn = new GridBoundColumn();
                    boundColumn.DataField = dc.ColumnName;
                    boundColumn.HeaderText = dc.ColumnName;

                    this.gvPatientResultHistory.MasterTableView.Columns.Add(boundColumn);
                }
                gvPatientResultHistory.DataBind();
            }
            else
            {
                gvPatientResultHistory.DataSource = null;
                gvPatientResultHistory.DataBind();
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
            objLis = null;
            ds.Dispose();
        }
    }

    protected void gvPatientResultHistory_OnRowDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem || e.Item is GridDataItem || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                int ColumnCount = e.Item.Cells.Count;
                for (int i = 1; i != ColumnCount; i++)
                {
                    i = i + 1;
                    if (i > 2)
                    {
                        e.Item.Cells[i].Visible = false;
                    }
                }

                e.Item.Cells[0].Visible = true;
                e.Item.Cells[0].Width = Unit.Pixel(150);
            }
            if (e.Item is GridDataItem || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                e.Item.Cells[0].Width = Unit.Pixel(150);

                int ColumnCount = e.Item.Cells.Count;

                for (int i = 1; i != ColumnCount; i++)
                {
                    e.Item.Cells[i].Width = Unit.Pixel(130);
                    i = i + 1;
                    if (common.myBool(e.Item.Cells[i].Text.Contains('A')))
                    {
                        e.Item.Cells[i - 1].ForeColor = System.Drawing.Color.DarkViolet;
                    }
                    if (common.myBool(e.Item.Cells[i].Text.Contains('C')))
                    {
                        e.Item.Cells[i - 1].ForeColor = System.Drawing.Color.Red;
                    }

                    if (common.myBool(e.Item.Cells[i].Text.Contains("-1")))
                    {
                        e.Item.Cells[i - 1].BackColor = System.Drawing.Color.LightBlue;
                    }
                    if (i > 2)
                    {
                        e.Item.Cells[i].Visible = false;
                    }
                }

                e.Item.Cells[0].Visible = true;

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
