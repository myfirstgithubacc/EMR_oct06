using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class Include_Components_MasterComponent_FindPatient : System.Web.UI.UserControl
{
    private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private enum enumEncounter : byte
    {
        Select = 0,
        AppointmentDate = 1,
        PatientName = 2,
        AgeGender = 3,
        MobileNo = 4,
        RegistrationNo = 5,
        EncounterNo = 6,
        EncounterDate = 7,
        PatientVisit = 8,
        WardNameBedNo = 9,
        DoctorName = 10,
        Status = 11,
        VisitType = 12,
        Notification = 13,
        Addendum = 14,
        CALL = 15,
        TokenNo = 16,
        Company = 17,
        InvoiceNo = 18,
        CPR = 19,
        ConsultationCharges = 20
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var headerCell = new TableHeaderCell { Width = Unit.Percentage(16) };
        var span = new HtmlGenericControl("span");
        span.InnerHtml = "From<br/>Date";
        headerCell.Controls.Add(span);
        dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
        if (!Page.IsCallback)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }
        }
        if (!IsPostBack)
        {
            BindGroupTaggingMenu();
            DataSet dsDD = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            DataSet dsEP = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            try
            {
                if (common.myLen(Session["FindPatientDefaultStatusIds"]).Equals(0))
                {
                    dsDD = objivf.getEMRDoctorDetails(common.myInt(Session["EmployeeId"]));
                    if (dsDD.Tables.Count > 0)
                    {
                        if (dsDD.Tables[0].Rows.Count > 0)
                        {
                            Session["FindPatientDefaultStatusIds"] = common.myStr(dsDD.Tables[0].Rows[0]["FindPatientDefaultStatusIds"]);
                            if (common.myLen(Session["FindPatientDefaultStatusIds"]) > 0)
                            {
                                List<string> lstFindPatientEMRSTATUSID = new List<string>();
                                lstFindPatientEMRSTATUSID = common.myStr(Session["FindPatientDefaultStatusIds"]).Trim().Split(',').ToList();
                                Session["FindPatientEMRSTATUSID"] = lstFindPatientEMRSTATUSID;
                            }
                            if (common.myLen(dsDD.Tables[0].Rows[0]["FindPatientDefaultVisitType"]) > 0)
                            {
                                Session["FindPatientVISITTYPE"] = common.myStr(dsDD.Tables[0].Rows[0]["FindPatientDefaultVisitType"]);
                            }
                            else
                            {
                                Session["FindPatientVISITTYPE"] = "O";
                            }
                        }
                    }
                }
                if (common.myLen(Session["IsAccessAllEncounter"]).Equals(0))
                {
                    dsEP = objEMR.getEmployeePermission(common.myInt(Session["UserId"]));
                    if (dsEP.Tables[0].Rows.Count > 0)
                    {
                        Session["IsAccessAllEncounter"] = common.myBool(dsEP.Tables[0].Rows[0]["IsAccessAllEncounter"]);
                        Session["IsAccessSpecialisationResource"] = common.myBool(dsEP.Tables[0].Rows[0]["IsAccessSpecialisationResource"]);
                    }
                }
                if (common.myInt(Session["FindPatientSelected"]).Equals(1))
                {
                    Session["FindPatientSelected"] = "0";
                    OnPageLoad();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                dsEP.Dispose();
                objEMR = null;
                dsDD.Dispose();
                objivf = null;
            }
        }
        try
        {
            ddlSpecilization.Enabled = false;
            ddlProvider.Enabled = false;
            if (common.myBool(Session["IsAccessAllEncounter"]))
            {
                ddlSpecilization.Enabled = true;
                ddlProvider.Enabled = true;
            }
            else
            {
                if (common.myBool(Session["IsAccessSpecialisationResource"]))
                {
                    ddlSpecilization.Enabled = false;
                    ddlProvider.Enabled = true;
                }
            }
            if (common.myBool(Session["isEMRSuperUser"]) || common.myBool(Session["LoginIsAdminGroup"]))
            {
                ddlSpecilization.Enabled = true;
                ddlProvider.Enabled = true;
            }
            //int nRowIndex = gvEncounter.Items.Count - 1;
            //gvEncounter.Items[0].Selected = true;
            //gvEncounter.Items[3].Cells[0].Focus();//  .Selected = true;
            //   gvEncounter.Items[0].Cells[0].Style.Add("font-weight", "bold");
            //((TableCell)item["Select"]).Style.Add("font-weight", "bold");
            BindcontainerAppointmentNew();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    protected void OnPageLoad()
    {
        if (common.myLen(Session["EMRFindPatientDefaultPage"]).Equals(0) || common.myLen(Session["FindPatientAccessToAdmittingDoctor"]).Equals(0))
        {
            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();
            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                        "EMRFindPatientDefaultPage,FindPatientAccessToAdmittingDoctor,EMRTriageModuleId", sConString);
            if (collHospitalSetupValues.ContainsKey("EMRFindPatientDefaultPage"))
                Session["EMRFindPatientDefaultPage"] = collHospitalSetupValues["EMRFindPatientDefaultPage"];
            if (collHospitalSetupValues.ContainsKey("FindPatientAccessToAdmittingDoctor"))
                Session["FindPatientAccessToAdmittingDoctor"] = collHospitalSetupValues["FindPatientAccessToAdmittingDoctor"];

            if (collHospitalSetupValues.ContainsKey("EMRTriageModuleId"))
                ViewState["EMRTriageModuleId"] = collHospitalSetupValues["EMRTriageModuleId"];


            collHospitalSetupValues = null;
        }
        ViewState["ddlProviderAllBind"] = true;
        txtSearchN.Visible = false;
        txtSearch.Visible = false;
        if (common.myStr(ddlName.SelectedValue).Equals("R") || common.myStr(ddlName.SelectedValue).Equals("IVF"))
        {
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
        }
        ViewState["UserSpecialisationId"] = Session["UserSpecialisationId"];
        ViewState["SelectedEnc"] = "";
        tblDateRange.Visible = false;

        bindFindPatientMasterList();

        //if (common.myStr(Session["FindPatientAccessToAdmittingDoctor"]) == "N")
        //{
        //    BindSpeciliazation();
        //}

        //BindVisitType();
        //bindEmploymentStatus();
        //bindWardName();

        dtpfromDate.SelectedDate = System.DateTime.Now;
        dtpToDate.SelectedDate = System.DateTime.Now;
        rblSearchCriteria.Items.Add(new ListItem("Appointments", "1"));
        rblSearchCriteria.Items.Add(new ListItem("Visits", "2")); //rblSearchCriteria.Items.Add(new ListItem("OP&nbsp;Encounter", "2"));
        rblSearchCriteria.Items[1].Selected = true;
        if (common.myInt(Session["FindPatientSEARCHCRITERIA"]).Equals(1))
        {
            rblSearchCriteria.SelectedIndex = rblSearchCriteria.Items.IndexOf(rblSearchCriteria.Items.FindByValue("1"));
        }
        else
        {
            rblSearchCriteria.SelectedIndex = rblSearchCriteria.Items.IndexOf(rblSearchCriteria.Items.FindByValue("2"));
        }
        ddlSpecilization_SelectedIndexChanged(null, null);
        txtSearchN.Visible = false;
        txtSearch.Visible = false;
        if (common.myStr(ddlName.SelectedValue).Equals("R") || common.myStr(ddlName.SelectedValue).Equals("IVF"))
        {
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
        }
        if (common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
        {
            btnCloseQMS.Visible = false;
            rblSearchCriteria.Visible = false;
            chkIsWalkInPatient.Visible = false;
            chkMHC.Visible = false;
            ChkReferralIP.Visible = false;
            ddlPatientAge.Visible = false;
            ddlSpecilization.SelectedIndex = 0;
            //lblPatientAge.Visible = false;
        }
    }
    protected void btnFillData_OnClick(object sender, EventArgs e)
    {
        Session["FindPatientSelected"] = "1";
        OnPageLoad();
        if (common.myBool(Session["EMRSingleScreenIsAllowPopup"]))
        {
            Session["EMRSingleScreenIsAllowPopup"] = "0";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "HideSlidingPane()", true);
            BindBlankEncounterGrid();
            Alert.ShowAjaxMsg("Please save record before proceed to another option.", this.Page);
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SingleScreen_OnClientExpanded()", true);
            if (common.myBool(Session["EMRSingleScreenIsAllowPopup"]))
            {
                Session["EMRSingleScreenIsAllowPopup"] = "0";
                BindBlankEncounterGrid();
                Alert.ShowAjaxMsg("Please save record before proceed to another option.", this.Page);
                return;
            }
        }
        btnSearch.Enabled = false;
        btn_ClearFilter.Enabled = false;
        try
        {
            if (Session["FindPatientchkMHC"] != null)
            {
                if (!common.myStr(Session["FindPatientchkMHC"]).Equals(string.Empty))
                {
                    chkMHC.Checked = Convert.ToBoolean(common.myStr(Session["FindPatientchkMHC"]));
                }
            }
            if (Session["FindPatientchkIsWalkInPatient"] != null)
            {
                if (!common.myStr(Session["FindPatientchkIsWalkInPatient"]).Equals(string.Empty))
                {
                    chkIsWalkInPatient.Checked = Convert.ToBoolean(common.myStr(Session["FindPatientchkIsWalkInPatient"]));
                }
            }
            if (Session["FindPatientChkMLC"] != null)
            {
                if (!common.myStr(Session["FindPatientChkMLC"]).Equals(string.Empty))
                {
                    ChkMLC.Checked = Convert.ToBoolean(common.myStr(Session["FindPatientChkMLC"]));
                }
            }
            Session["IsPatientSelectionRunning"] = false;
            btnSearch.Enabled = false;
            SetControls();
            #region set previous selected values
            if (common.myInt(Session["FindPatientSEARCHCRITERIA"]).Equals(1))
            {
                //rblSearchCriteria.Items[0].Selected = true;
                //rblSearchCriteria.SelectedValue  = "1";
                rblSearchCriteria.SelectedIndex = rblSearchCriteria.Items.IndexOf(rblSearchCriteria.Items.FindByValue("1"));
            }
            else if (common.myInt(Session["FindPatientSEARCHCRITERIA"]).Equals(2))
            {
                // rblSearchCriteria.Items[1].Selected = true;
                //  rblSearchCriteria.SelectedValue = "2";
                rblSearchCriteria.SelectedIndex = rblSearchCriteria.Items.IndexOf(rblSearchCriteria.Items.FindByValue("2"));
            }
            ddlName.SelectedIndex = ddlName.Items.IndexOf(ddlName.FindItemByValue(common.myStr(Session["FindPatientSEARCHOPTION"])));
            //ddlName_OnTextChanged(null, null);
            txtSearchN.Visible = false;
            txtSearch.Visible = false;
            if (common.myStr(ddlName.SelectedValue).Equals("R") || common.myStr(ddlName.SelectedValue).Equals("IVF"))
            {
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
            if (common.myInt(Session["FindPatientSPECIALISATIONID"]) > 0)
            {
                ddlSpecilization.SelectedIndex = ddlSpecilization.Items.IndexOf(ddlSpecilization.FindItemByValue(common.myStr(Session["FindPatientSPECIALISATIONID"])));
            }
            //   ddlSpecilization_SelectedIndexChanged(null, null);            
            if (common.myInt(Session["FindPatientSEARCHCRITERIA"]).Equals(1))
            {
                if (!common.myStr(Session["FindPatientPROVIDERID_S"]).Equals("A"))
                {
                    ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(Session["FindPatientPROVIDERID_S"])));
                }
            }
            else if (common.myInt(Session["FindPatientSEARCHCRITERIA"]).Equals(2))
            {
                BindProvider();
                if (!common.myStr(Session["FindPatientPROVIDERID"]).Equals(string.Empty))
                {
                    ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(Session["FindPatientPROVIDERID"])));
                }
                if (common.myLen(Session["FindPatientEMRSTATUSID"]) > 0)
                {
                    List<string> lstFindPatientEMRSTATUSID = new List<string>();
                    lstFindPatientEMRSTATUSID = (List<string>)Session["FindPatientEMRSTATUSID"];
                    foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
                    {
                        currentItem.Checked = false;
                        foreach (string AppointmentStatusValueChecked in lstFindPatientEMRSTATUSID)
                        {
                            if (currentItem.Value.Equals(AppointmentStatusValueChecked))
                            {
                                currentItem.Checked = true;
                            }
                        }
                    }
                }
            }
            switch (common.myStr(Session["FindPatientSEARCHOPTION"]))
            {
                case "R": //Reg No
                    txtSearchN.Text = common.myStr(Session["FindPatientREGISTRATIONNO"]);
                    break;
                case "ENC"://Encounter No.
                    txtSearch.Text = common.myStr(Session["FindPatientENCOUNTERNO"]);
                    break;
                case "O"://Old Reg No
                    txtSearch.Text = common.myStr(Session["FindPatientOLDREGISTRATIONNO"]);
                    break;
                case "N"://Patient Name
                    txtSearch.Text = common.myStr(Session["FindPatientPATIENTNAME"]);
                    break;
                case "EN"://Enrolle No.
                    txtSearch.Text = common.myStr(Session["FindPatientENROLLENO"]);
                    break;
                case "MN"://Mobile No.
                    txtSearch.Text = common.myStr(Session["FindPatientMOBILENO"]);
                    break;
                case "IVF"://IVF No.
                    txtSearchN.Text = common.myStr(Session["FindPatientIVFNO"]);
                    break;
            }
            if (common.myStr(Session["FindPatientVISITTYPE"]).Equals(string.Empty)
                && common.myLen(txtSearch.Text).Equals(0)
                && common.myLen(txtSearchN.Text).Equals(0))
            {
                if (drpVisitType.SelectedIndex.Equals(0))
                {
                    drpVisitType.SelectedIndex = drpVisitType.Items.IndexOf(drpVisitType.FindItemByValue("O"));
                }
            }
            else
            {
                drpVisitType.SelectedIndex = drpVisitType.Items.IndexOf(drpVisitType.FindItemByValue(common.myStr(Session["FindPatientVISITTYPE"])));
            }
            if (common.myLen(Session["FindPatientDATERANGE"]) > 0)
            {
                ddlrange.SelectedIndex = ddlrange.Items.IndexOf(ddlrange.FindItemByValue(common.myStr(Session["FindPatientDATERANGE"])));
                //     ddlrange_SelectedIndexChanged(null, null);
                if (ddlrange.SelectedValue == "4")
                {
                    tblDateRange.Visible = true;
                    dtpfromDate.Visible = true;
                    dtpToDate.Visible = true;
                    spTo.Visible = true;
                }
                else
                {
                    if (ddlrange.SelectedValue == "5")
                    {
                        tblDateRange.Visible = true;
                        dtpfromDate.Visible = true;
                        dtpToDate.Visible = false;
                        spTo.Visible = false;
                    }
                    else
                    {
                        tblDateRange.Visible = false;
                    }
                }
                if (common.myLen(Session["FindPatientFROMDATE"]) > 0 && common.myLen(Session["FindPatientTODATE"]) > 0)
                {
                    dtpfromDate.SelectedDate = Convert.ToDateTime(common.myStr(Session["FindPatientFROMDATE"]));
                    dtpToDate.SelectedDate = Convert.ToDateTime(common.myStr(Session["FindPatientTODATE"]));
                }
            }
            #endregion
            SetFindPatientVisitsColumnsVisibilitySetup();
            bindSearchData(true);
            //CountOpenReferral();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            btnSearch.Enabled = true;
            btn_ClearFilter.Enabled = true;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(drpVisitType.SelectedValue).Equals("L"))
            {
                if (common.myInt(ddlProvider.SelectedValue).Equals(0))
                {
                    Alert.ShowAjaxMsg("Please Select Provider!", this.Page);
                    //BindBlankEncounterGrid();
                    return;
                }
                if (common.myStr(ddlrange.SelectedValue) != "DD0" && common.myStr(ddlrange.SelectedValue) != "5")
                {
                    Alert.ShowAjaxMsg("Please Select Today or Go To Date!", this.Page);
                    //BindBlankEncounterGrid();
                    return;
                }
            }
            List<string> ddlAppointmentStatusValue = new List<string>();
            if (common.myInt(gvEncounter.Items.Count) - 1 >= common.myInt(Session["ItemIndex"]))
            {
                GridItem gv1 = gvEncounter.Items[common.myInt(Session["ItemIndex"])];
                LinkButton lnkSelect = (LinkButton)gv1.FindControl("lnkSelect");
                GridDataItem item = (GridDataItem)gvEncounter.Items[common.myInt(Session["ItemIndex"])];
                if (common.myStr(lnkSelect).Trim() != "")
                {
                    ((TableCell)item["Select"]).Style.Add("font-weight", "normal");
                    Session["ItemIndex"] = null;
                }
                item.Dispose();
                gv1.Dispose();
            }
            foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
            {
                if (currentItem.Checked)
                {
                    ddlAppointmentStatusValue.Add(currentItem.Value);
                }
            }
            bindSearchData(false);
            foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
            {
                currentItem.Checked = false;
                foreach (string AppointmentStatusValueChecked in ddlAppointmentStatusValue)
                {
                    if (currentItem.Value.Equals(AppointmentStatusValueChecked))
                    {
                        currentItem.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void bindSearchData(bool IsOnLoad)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        int TotalDays = 31;
        DataSet objDs = new DataSet();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        DataView dvPackage = new DataView();
        DataView dvReferral = new DataView();
        DataView dvReferralOP = new DataView();
        DataView dvReferralIP = new DataView();
        DataView dvMLC = new DataView();
        DataView dvIsWalkInPatient = new DataView();
        DataView dvIsPatientIP = new DataView();
        DataView dvReferralOPIP = new DataView();
        DataView dvMHC = new DataView();
        DataView dvMHCIsWalkInPatient = new DataView();
        DataView dvMHCMLC = new DataView();
        DataView dvIsWalkInPatientMLC = new DataView();
        DataView dvIsWalkInPatientMLCMHC = new DataView();
        DataView dvFilter = new DataView();
        DataView dvApp = new DataView();
        //DataSet dsF = new DataSet();
        try
        {
            ViewState["OPIP"] = null;
            if (Session["HospitalLocationID"] != null)
            {
                ViewState["Count"] = 0;
                hshInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                ViewState["SelectedEncounterId"] = "";
                ViewState["SelectedEnc"] = "";
                if (rblSearchCriteria.SelectedValue == "2")
                {
                    hshInput.Add("@intProviderId", common.myInt(ddlProvider.SelectedValue));
                }
                hshInput.Add("@intFacilityId", ddlLocation.SelectedValue);
                hshInput.Add("@chvDateRange", (common.myStr(ddlrange.SelectedValue).Equals("A") ? string.Empty : common.myStr(ddlrange.SelectedValue)));
                if (ddlrange.SelectedValue == "4" || ddlrange.SelectedValue == "5" || ddlrange.SelectedValue == "WW0" || ddlrange.SelectedValue == "MM-1" || ddlrange.SelectedValue == "MM0" ||
                     ddlrange.SelectedValue == "YD" || ddlrange.SelectedValue == "DD0" || ddlrange.SelectedValue == "WW-1" || ddlrange.SelectedValue == "MM-6" || ddlrange.SelectedValue == "YY-1")
                {
                    string[] str = getToFromDate((common.myStr(ddlrange.SelectedValue).Equals("A") ? string.Empty : common.myStr(ddlrange.SelectedValue)));
                    string sFromDate = str[0];
                    string sToDate = str[1];
                    hshInput.Add("@chrFromDate", sFromDate);
                    hshInput.Add("@chrToDate", sToDate);
                }
                if (ddlName.SelectedValue == "R")
                {
                    hshInput.Add("@chvRegistrationNo", txtSearchN.Text);
                }
                else if (ddlName.SelectedValue == "N")
                {
                    hshInput.Add("@chvName", txtSearch.Text);
                }
                else if (ddlName.SelectedValue == "O")
                {
                    hshInput.Add("@chvOldRegistrationNo", txtSearch.Text);
                }
                else if (ddlName.SelectedValue == "EN")
                {
                    hshInput.Add("@cEnrolleNo", txtSearch.Text);
                }
                else if (ddlName.SelectedValue == "CPR"
                        && !common.myStr(txtSearch.Text).Equals(string.Empty))
                {
                    hshInput.Add("@CPR", txtSearch.Text);
                }
                else if (ddlName.SelectedValue == "MN")
                {
                    hshInput.Add("@MobileNo", txtSearch.Text);
                }
                else if (ddlName.SelectedValue == "ENC")
                {
                    if (rblSearchCriteria.SelectedValue == "2" || rblSearchCriteria.SelectedValue == "5" || rblSearchCriteria.SelectedValue == "6")
                    {
                        hshInput.Add("@chvEncounterNo", txtSearch.Text);
                    }
                }
                if (rblSearchCriteria.SelectedValue == "2")
                {
                    gvEncounter.Visible = true;
                    hshInput.Add("@intLoginFacilityId", common.myInt(Session["FacilityID"]));
                    hshInput.Add("@intStatusId", "0");
                    hshInput.Add("@intemrStatusId", ShowVisitCheckedItems(ddlAppointmentStatus));
                    if (common.myStr(drpVisitType.SelectedValue) != "A")
                    {
                        hshInput.Add("@chrEncounterType", common.myStr(drpVisitType.SelectedValue));
                    }
                    hshInput.Add("@intSpecialisationId", ddlSpecilization.SelectedValue);
                    if (common.myStr(ddlrange.SelectedValue).Equals("4"))
                    {
                        if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                        {
                            TotalDays = common.myInt((Convert.ToDateTime(dtpToDate.SelectedDate) - Convert.ToDateTime(dtpfromDate.SelectedDate)).TotalDays);
                        }
                    }
                    if (!(
                        common.myStr(ddlrange.SelectedValue).Equals("DD0")
                        || common.myStr(ddlrange.SelectedValue).Equals("YD")
                        || common.myStr(ddlrange.SelectedValue).Equals("WW0")
                        || common.myStr(ddlrange.SelectedValue).Equals("WW-1")
                        || common.myStr(ddlrange.SelectedValue).Equals("WW+1")
                        || common.myStr(ddlrange.SelectedValue).Equals("WW+2")
                        || common.myStr(ddlrange.SelectedValue).Equals("YY+1")
                        || common.myStr(ddlrange.SelectedValue).Equals("MM-1")//Added by Shabana on 09-04-2022
                        || common.myStr(ddlrange.SelectedValue).Equals("MM-6")//Added by Shabana on 09-04-2022
                        || common.myStr(ddlrange.SelectedValue).Equals("YY-1")//Added by Shabana on 09-04-2022
                        || common.myStr(ddlrange.SelectedValue).Equals("4")//Added by Shabana on 09-04-2022
                        )
                        && common.myInt(ddlProvider.SelectedValue).Equals(0)
                        && common.myLen(txtSearchN.Text).Equals(0)
                        && common.myLen(txtSearch.Text).Equals(0)
                        && TotalDays >= 31)
                    {
                        if (!IsOnLoad)
                        {
                            Alert.ShowAjaxMsg("Please define the filter criteria!", this.Page);
                        }
                        BindBlankEncounterGrid();
                    }
                    else
                    {
                        hshInput.Add("@intWardId", ShowCheckedItems(ddlWard));
                        if (common.myInt(ddlPatientAge.SelectedValue) > 0)
                        {
                            hshInput.Add("@intPatientAge", common.myInt(ddlPatientAge.SelectedValue));
                        }
                        if (common.myStr(ddlName.SelectedValue).Equals("IVF"))
                        {
                            hshInput.Add("@intIVFNo", common.myInt(txtSearchN.Text));
                        }
                        hshInput.Add("@bitUnReviewed", common.myBool(CheckUnReviewedStatus(ddlAppointmentStatus)));
                        objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientListsV3", hshInput);
                        if (objDs != null)
                        {
                            if (objDs.Tables[0].Rows.Count > 0)
                            {
                                dv = objDs.Tables[0].DefaultView;
                                ViewState["Count"] = dv.ToTable().Rows.Count;
                                ViewState["AppUnConf"] = string.Empty;//U
                                ViewState["AppConfirm"] = string.Empty;//A
                                ViewState["AppChkIn"] = string.Empty;//P
                                ViewState["AppChkOut"] = string.Empty;//O
                                ViewState["AppNoShow"] = string.Empty;//M
                                ViewState["AppCancel"] = string.Empty;//C
                                ViewState["AppWaited"] = string.Empty;//W
                                ViewState["AppSeen"] = string.Empty;//Seen
                                ViewState["Discharged"] = string.Empty;//Closed/Discharged
                                ViewState["AppPayment"] = string.Empty;//PC
                                ViewState["VitalsTaken"] = string.Empty;//VT
                                ViewState["AppUnConf"] = objDs.Tables[0].Select("StatusCode='U'").Count();
                                ViewState["AppChkIn"] = objDs.Tables[0].Select("StatusCode='P'").Count();
                                int openCount = objDs.Tables[0].Select("StatusCode='O'").Count();
                                ViewState["AppNoShow"] = objDs.Tables[0].Select("StatusCode='M'").Count();
                                ViewState["AppCancel"] = objDs.Tables[0].Select("StatusCode='C'").Count();
                                ViewState["AppWaited"] = objDs.Tables[0].Select("StatusCode='W'").Count();
                                ViewState["AppSeen"] = objDs.Tables[0].Select("ISNULL(Status,'')='Seen'").Count();
                                ViewState["AppPayment"] = objDs.Tables[0].Select("StatusCode='PC'").Count();
                                ViewState["VitalsTaken"] = objDs.Tables[0].Select("StatusCode='VT'").Count();
                                int paymentCount = common.myInt(ViewState["AppPayment"] = objDs.Tables[0].Select("StatusCode='PC'").Count());
                                int confirmCount = common.myInt(objDs.Tables[0].Select("StatusCode='A'").Count());
                                int totalConfirmCount = confirmCount + common.myInt(openCount) + common.myInt(ViewState["AppSeen"]);
                                ViewState["AppPayment"] = common.myInt(paymentCount - common.myInt(ViewState["AppSeen"])) + " / " + paymentCount;
                                ViewState["AppConfirm"] = confirmCount.ToString() + " / " + totalConfirmCount.ToString();
                                ViewState["Discharged"] = objDs.Tables[0].Select("ISNULL(Status,'')='Closed/Discharged'").Count();
                                ViewState["AppChkOut"] = openCount + " / " + (openCount + common.myInt(ViewState["AppSeen"])).ToString();

                                //DataView dvCPR = new DataView(dv.ToTable());
                                //if (!common.myStr(txtCPR.Text).Equals(string.Empty))
                                //{
                                //dvCPR.RowFilter = "CPR='"+common.myStr(txtCPR.Text)+ "'";
                                //}

                                dvIsPatientIP = new DataView(dv.ToTable());
                                dvIsPatientIP.RowFilter = "OPIP='I'";
                                IPPatient.Text = string.Empty;
                                IPPatient.Text = "IP Patient (" + common.myStr(dvIsPatientIP.ToTable().Rows.Count) + ")";

                                dvPackage = new DataView(dv.ToTable());
                                dvPackage.RowFilter = "PackageId>0";
                                //lblLegendPackage.Text = "Wellness (" + common.myStr(dvPackage.ToTable().Rows.Count) + ")";
                                dvReferral = new DataView(dv.ToTable());
                                dvReferral.RowFilter = "IsReferralCase>0";
                                dvReferralOP = new DataView(dv.ToTable());
                                dvReferralOP.RowFilter = "IsReferralCase>0 AND OPIP<>'I'";
                                ChkReferralOP.Text = string.Empty;
                                ChkReferralOP.Text = "Refer OP (" + common.myStr(dvReferralOP.ToTable().Rows.Count) + ")";
                                dvReferralIP = new DataView(dv.ToTable());
                                dvReferralIP.RowFilter = "IsReferralCase>0 AND OPIP='I'";
                                ChkReferralIP.Text = string.Empty;
                                ChkReferralIP.Text = "Refer IP (" + common.myStr(dvReferralIP.ToTable().Rows.Count) + ")";
                                dvMLC = new DataView(dv.ToTable());
                                dvMLC.RowFilter = "MLC>0";
                                ChkMLC.Text = string.Empty;
                                ChkMLC.Text = "MLC (" + common.myStr(dvMLC.ToTable().Rows.Count) + ")";
                                dvIsWalkInPatient = new DataView(dv.ToTable());
                                dvIsWalkInPatient.RowFilter = "IsWalkInPatient=1";
                                chkIsWalkInPatient.Text = string.Empty;
                                chkIsWalkInPatient.Text = "Walk In (" + common.myStr(dvIsWalkInPatient.ToTable().Rows.Count) + ")";

                                dvReferralOPIP = new DataView(dv.ToTable());
                                dvReferralIP.RowFilter = "IsReferralCase>0";
                                dvMHC = new DataView(dv.ToTable());
                                dvMHC.RowFilter = "MHC=1";
                                dvMHCIsWalkInPatient = new DataView(dv.ToTable());
                                dvMHCIsWalkInPatient.RowFilter = "MHC=1 or IsWalkInPatient=1 ";
                                dvMHCMLC = new DataView(dv.ToTable());
                                dvMHCMLC.RowFilter = "MHC=1 or MLC>0 ";
                                dvIsWalkInPatientMLC = new DataView(dv.ToTable());
                                dvIsWalkInPatientMLC.RowFilter = "IsWalkInPatient=1 or MLC>0 ";
                                dvIsWalkInPatientMLCMHC = new DataView(dv.ToTable());
                                dvIsWalkInPatientMLCMHC.RowFilter = "IsWalkInPatient=1 or MLC>0 or MHC=1 ";
                                chkMHC.Text = string.Empty;
                                chkMHC.Text = common.myStr(Resources.PRegistration.MHC) + " (" + common.myStr(dvMHC.ToTable().Rows.Count) + ")";
                                dvFilter = objDs.Tables[0].DefaultView;
                                hdnFilter.Value = "0";
                                string strFilter = string.Empty;



                                if (chkIsWalkInPatient.Checked || chkMHC.Checked || ChkMLC.Checked || ChkReferralOP.Checked || ChkReferralIP.Checked)
                                {
                                    if (chkIsWalkInPatient.Checked)
                                    {
                                        if (!common.myStr(hdnFilter.Value).Equals("1"))
                                        {
                                            strFilter = strFilter + "IsWalkInPatient=1";
                                            hdnFilter.Value = "1";
                                        }
                                        else
                                        {
                                            strFilter = strFilter + " or IsWalkInPatient=1";
                                        }
                                    }
                                    if (chkMHC.Checked)
                                    {
                                        if (!common.myStr(hdnFilter.Value).Equals("1"))
                                        {
                                            strFilter = strFilter + "MHC=1";
                                            hdnFilter.Value = "1";
                                        }
                                        else
                                        {
                                            strFilter = strFilter + " or MHC=1";
                                        }
                                    }
                                    if (ChkMLC.Checked)
                                    {
                                        if (!common.myStr(hdnFilter.Value).Equals("1"))
                                        {
                                            strFilter = strFilter + "MLC>0";
                                            hdnFilter.Value = "1";
                                        }
                                        else
                                        {
                                            strFilter = strFilter + " or MLC>0";
                                        }
                                    }
                                    if (ChkReferralOP.Checked)
                                    {
                                        if (!common.myStr(hdnFilter.Value).Equals("1"))
                                        {
                                            strFilter = strFilter + "IsReferralCase>0 AND OPIP<>'I'";
                                            hdnFilter.Value = "1";
                                        }
                                        else
                                        {
                                            strFilter = strFilter + " or (IsReferralCase>0 AND OPIP<>'I')";
                                        }
                                    }
                                    if (ChkReferralIP.Checked)
                                    {
                                        if (!common.myStr(hdnFilter.Value).Equals("1"))
                                        {
                                            strFilter = strFilter + "IsReferralCase>0 AND OPIP='I'";
                                            hdnFilter.Value = "1";
                                        }
                                        else
                                        {
                                            strFilter = strFilter + "or (IsReferralCase>0 AND OPIP='I')";
                                        }
                                    }
                                    dvFilter.RowFilter = strFilter;
                                    ViewState["Count"] = dvFilter.ToTable().Rows.Count;
                                    gvEncounter.DataSource = dvFilter.ToTable();
                                    dt = dvFilter.ToTable();
                                    ds.Tables.Add(dt);
                                    //ViewState["dsSorting"] = ds;
                                    gvEncounter.DataBind();
                                    //bindStatus(rblSearchCriteria.SelectedValue);
                                }
                                else
                                {
                                    if (objDs.Tables[0].Rows.Count > 0)
                                    {
                                        ViewState["Count"] = objDs.Tables[0].Rows.Count;
                                        if (objDs.Tables[0].Rows.Count < 11 && gvEncounter.CurrentPageIndex > 0)
                                        {
                                            gvEncounter.CurrentPageIndex = 0;
                                        }
                                        gvEncounter.DataSource = objDs.Tables[0];

                                        gvEncounter.DataBind();
                                        ds.Dispose();
                                        dt.Dispose();
                                        //bindStatus(rblSearchCriteria.SelectedValue);
                                    }
                                    else
                                    {
                                        BindBlankEncounterGrid();
                                    }
                                }
                                ViewState["EncBlankGrid"] = "False";
                                //dsF.Tables.Add(dv.ToTable());
                                //Cache.Insert("Encounter_" + common.myInt(Session["UserID"]), dsF, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                                dvReferral.Dispose();
                            }
                            else
                            {
                                BindBlankEncounterGrid();
                            }
                        }
                        else
                        {
                            BindBlankEncounterGrid();
                        }
                    }
                    //CountOpenReferral();
                }
            }
            TotalEncounterRecords();
            //lblPageRecordCount.Text = "Record from " + common.myStr(gvEncounter.Items.Count) + " Of total record(s) " + common.myStr(ViewState["TotalEncounterRecords"]);
            BindcontainerAppointmentNew();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            if (chkMHC.Checked)
            {
                Session["FindPatientchkMHC"] = "true";
            }
            else if (!chkMHC.Checked)
            {
                Session["FindPatientchkMHC"] = "false";
            }
            if (chkIsWalkInPatient.Checked)
            {
                Session["FindPatientchkIsWalkInPatient"] = "true";
            }
            else if (!chkIsWalkInPatient.Checked)
            {
                Session["FindPatientchkIsWalkInPatient"] = "false";
            }
            if (ChkMLC.Checked)
            {
                Session["FindPatientChkMLC"] = "true";
            }
            else if (!ChkMLC.Checked)
            {
                Session["FindPatientChkMLC"] = "false";
            }
            Session["FindPatientSEARCHCRITERIA"] = common.myStr(rblSearchCriteria.SelectedValue);
            Session["FindPatientSEARCHOPTION"] = common.myStr(ddlName.SelectedValue);
            Session["FindPatientFINDWARDID"] = ShowCheckedItems(ddlWard);
            Session["FindPatientSPECIALISATIONID"] = string.Empty;
            Session["FindPatientPROVIDERID"] = string.Empty;
            Session["FindPatientPROVIDERID_S"] = string.Empty;
            Session["FindPatientENCOUNTERNO"] = string.Empty;
            Session["FindPatientREGISTRATIONNO"] = string.Empty;
            Session["FindPatientOLDREGISTRATIONNO"] = string.Empty;
            Session["FindPatientPATIENTNAME"] = string.Empty;
            Session["FindPatientENROLLENO"] = string.Empty;
            Session["FindPatientMOBILENO"] = string.Empty;
            Session["FindPatientIVFNO"] = string.Empty;
            Session["FindPatientSTATUSID"] = string.Empty;
            Session["FindPatientEMRSTATUSID"] = string.Empty;
            Session["FindPatientVISITTYPE"] = string.Empty;
            Session["FindPatientDATERANGE"] = common.myStr(ddlrange.SelectedValue);
            Session["FindPatientFROMDATE"] = string.Empty;
            Session["FindPatientTODATE"] = string.Empty;
            if (hshInput != null)
            {
                foreach (DictionaryEntry entry in hshInput)
                {
                    switch (common.myStr(entry.Key).ToUpper())
                    {
                        case "@INTSPECIALISATIONID":
                            Session["FindPatientSPECIALISATIONID"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@INTPROVIDERID":
                            Session["FindPatientPROVIDERID"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHVPROVIDERID":
                            Session["FindPatientPROVIDERID_S"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHVENCOUNTERNO":
                            Session["FindPatientENCOUNTERNO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHVREGISTRATIONNO":
                            Session["FindPatientREGISTRATIONNO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHVOLDREGISTRATIONNO":
                            Session["FindPatientOLDREGISTRATIONNO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHVNAME":
                            Session["FindPatientPATIENTNAME"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CENROLLENO":
                            Session["FindPatientENROLLENO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@MOBILENO":
                            Session["FindPatientMOBILENO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@INTIVFNO":
                            Session["FindPatientIVFNO"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@INTSTATUSID":
                            Session["FindPatientSTATUSID"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@INTEMRSTATUSID":
                            List<string> lstFindPatientEMRSTATUSID = new List<string>();
                            lstFindPatientEMRSTATUSID = common.myStr(entry.Value).Trim().Split(',').ToList();
                            Session["FindPatientEMRSTATUSID"] = lstFindPatientEMRSTATUSID;
                            break;
                        case "@CHRENCOUNTERTYPE":
                            Session["FindPatientVISITTYPE"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHRFROMDATE":
                            Session["FindPatientFROMDATE"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@CHRTODATE":
                            Session["FindPatientTODATE"] = common.myStr(entry.Value).Trim();
                            break;
                        case "@INTWARDID":
                            List<string> lstFindPatientWARDID = new List<string>();
                            lstFindPatientWARDID = common.myStr(entry.Value).Trim().Split(',').ToList();
                            Session["FindPatientFINDWARDID"] = lstFindPatientWARDID;
                            break;
                    }
                }
            }
            objDl = null;
            objDs.Dispose();
            ds.Dispose();
            dt.Dispose();
            dv.Dispose();
            dvPackage.Dispose();
            dvReferral.Dispose();
            dvReferralOP.Dispose();
            dvReferralIP.Dispose();
            dvMLC.Dispose();
            dvIsWalkInPatient.Dispose();
            dvReferralOPIP.Dispose();
            dvMHC.Dispose();
            dvMHCIsWalkInPatient.Dispose();
            dvMHCMLC.Dispose();
            dvIsWalkInPatientMLC.Dispose();
            dvIsWalkInPatientMLCMHC.Dispose();
            dvFilter.Dispose();
            dvApp.Dispose();
            //dsF.Dispose();
        }
    }
    private void BindProvider()
    {
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataSet objDs = new DataSet();
        try
        {
            objDs = objemr.getEmployeeWithResource(common.myInt(Session["FacilityId"]), common.myInt(ddlSpecilization.SelectedValue),
                                common.myInt(Session["UserId"]));


            ddlProvider.ClearSelection();
            ddlProvider.DataSource = objDs.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();
            CheckUserDoctorOrNot();

            if (objDs.Tables.Count > 1)
            {
                if (objDs.Tables[1].Rows.Count > 0)
                {
                    lnkOrderApproval.Text = "Pending Approval (" + common.myStr(objDs.Tables[1].Rows[0]["Total"]) + ")";
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDs.Dispose();
            objemr = null;
        }
    }

    private void bindFindPatientMasterList()
    {
        RadComboBoxItem lst = new RadComboBoxItem();
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();


        try
        {
            ds = objemr.getFindPatientMasterList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                common.myInt(Session["UserId"]));

            #region Table-0, Specilisation

            ddlSpecilization.Text = string.Empty;
            ddlSpecilization.Items.Clear();
            ddlSpecilization.ClearSelection();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.DataSource = ds.Tables[0];
                ddlSpecilization.DataTextField = "NAME";
                ddlSpecilization.DataValueField = "ID";
                ddlSpecilization.DataBind();
                ddlSpecilization.Items.Insert(0, new RadComboBoxItem("All", "0"));
                string x = common.myStr(Session["FindPatientSPECIALISATIONID"]);
                if (ViewState["UserSpecialisationId"] != null && common.myStr(Session["FindPatientSPECIALISATIONID"]).Equals(string.Empty))
                {
                    ddlSpecilization.SelectedValue = common.myStr(ViewState["UserSpecialisationId"]);
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Specialization not available", Page);
            }

            #endregion

            #region Table-1, Status
            lst = new RadComboBoxItem();

            ddlAppointmentStatus.Items.Clear();
            containerAppointment.Controls.Clear();
            containerPatientVisits.Controls.Clear();
            containerAppointmentNew.Controls.Clear();

            lst.Value = "";
            lst.Text = "Select All";
            lst.Selected = true;

            ddlAppointmentStatus.Items.Add(lst);
            lblAppointmentStatus.Text = "Status";

            ViewState["dsFinalData"] = ds.Tables[1];
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddlAppointmentStatus.Items.Clear();
                ddlAppointmentStatus.ClearSelection();
                ddlAppointmentStatus.DataSource = null;
                ddlAppointmentStatus.DataBind();
                //ddlAppointmentStatus.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
                //ddlAppointmentStatus.Items[0].Value = string.Empty;
                ddlAppointmentStatus.DataBind();
                HtmlGenericControl s = new HtmlGenericControl();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    RadComboBoxItem lsts = new RadComboBoxItem();
                    RadComboBoxItem lstStatusColor = new RadComboBoxItem();
                    lsts.Attributes.Add("style", "background-color:" + common.myStr(ds.Tables[1].Rows[i]["StatusColor"]) + ";");
                    lsts.Attributes.Add("Code", common.myStr(ds.Tables[1].Rows[i]["Code"]));
                    lsts.Text = common.myStr(ds.Tables[1].Rows[i]["Status"]);
                    lstStatusColor.Text = common.myStr(ds.Tables[1].Rows[i]["Status"]);
                    lsts.Value = common.myStr(ds.Tables[1].Rows[i]["StatusId"]);
                    lstStatusColor.Value = common.myStr(ds.Tables[1].Rows[i]["Status"]);
                    ddlAppointmentStatus.Items.Add(lsts);
                    s = new HtmlGenericControl("span");
                    //s.Attributes.Add("style", "background:#98AFC7; color:#fff");
                    s.Attributes["class"] = "mlabel";
                    s.Attributes.Add("style", "background:" + common.myStr(ds.Tables[1].Rows[i]["StatusColor"]) + ";" + " color:#000" + ";font-family:Arial;font-size:8pt;");
                    s.Attributes.Add("id", common.myStr(ds.Tables[1].Rows[i]["StatusId"]));
                    s.InnerHtml = common.myStr(ds.Tables[1].Rows[i]["Status"]) + "&nbsp;";
                    containerPatientVisits.Controls.Add(s);
                    lsts.Dispose();
                    lstStatusColor.Dispose();
                }
                s = new HtmlGenericControl("span");
                s.Attributes["class"] = "mlabel";
                s.Attributes.Add("style", "background:#FFF691; color:#000;");
                s.Attributes.Add("id", "99999");
                s.InnerHtml = "Appointment";
                containerPatientVisits.Controls.Add(s);
                BindcontainerAppointmentNew();
                s.Dispose();
            }
            foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
            {
                currentItem.Checked = true;
            }
            if (common.myLen(Session["FindPatientEMRSTATUSID"]) > 0)
            {
                List<string> lstFindPatientEMRSTATUSID = new List<string>();
                lstFindPatientEMRSTATUSID = (List<string>)Session["FindPatientEMRSTATUSID"];
                foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
                {
                    currentItem.Checked = false;
                    foreach (string AppointmentStatusValueChecked in lstFindPatientEMRSTATUSID)
                    {
                        if (currentItem.Value.Equals(AppointmentStatusValueChecked))
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
            }

            #endregion

            #region Table-2, WardList

            ddlWard.ClearSelection();

            ddlWard.DataSource = ds.Tables[2];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            if (common.myLen(Session["FindPatientFINDWARDID"]) > 0)
            {
                try
                {
                    List<string> lstFindPatientWARDID = new List<string>();
                    lstFindPatientWARDID = (List<string>)Session["FindPatientFINDWARDID"];
                    foreach (RadComboBoxItem currentItem in ddlWard.Items)
                    {
                        currentItem.Checked = false;
                        foreach (string WardValueChecked in lstFindPatientWARDID)
                        {
                            if (currentItem.Value.Equals(WardValueChecked))
                            {
                                currentItem.Checked = true;
                            }
                        }
                    }
                }
                catch
                {
                    foreach (RadComboBoxItem currentItem in ddlWard.Items)
                    {
                        currentItem.Checked = true;
                    }
                }
            }
            else
            {
                foreach (RadComboBoxItem currentItem in ddlWard.Items)
                {
                    currentItem.Checked = true;
                }
            }
            #endregion

            #region Table-3, VisitType


            DataView dvERFilter = new DataView(ds.Tables[3]);
            if (common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
            {
                dvERFilter.RowFilter = "Code not in ('O','I','L')";
            }
            else
            {

                string providingservice = common.Getprovidingservice(common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), sConString);

                if (providingservice.Equals("I"))                {                    dvERFilter.RowFilter = "Code='I'";                }                else if (providingservice.Equals("O"))                {                    dvERFilter.RowFilter = "Code='O'";                }                else                {                    drpVisitType.Items.Clear();                    drpVisitType.ClearSelection();                    lst.Value = "A";                    lst.Text = "All";                    lst.Selected = true;                    drpVisitType.Items.Add(lst);                    dvERFilter.RowFilter = "Code in ('O','I','L','E')";                }
                
                //lst = new RadComboBoxItem();

                //drpVisitType.Items.Clear();
                //drpVisitType.ClearSelection();
                //lst.Value = "A";
                //lst.Text = "All";
                //lst.Selected = true;
                //drpVisitType.Items.Add(lst);
            }

            for (int i = 0; i < dvERFilter.ToTable().Rows.Count; i++)
            {
                RadComboBoxItem lsts = new RadComboBoxItem();
                RadComboBoxItem lstStatusColor = new RadComboBoxItem();
                lsts.Attributes.Add("style", "background-color:" + common.myStr(dvERFilter.ToTable().Rows[i]["StatusColor"]) + ";");
                lsts.Text = common.myStr(dvERFilter.ToTable().Rows[i]["Status"]);
                lstStatusColor.Text = lsts.Text;
                lsts.Value = common.myStr(dvERFilter.ToTable().Rows[i]["Code"]);
                drpVisitType.Items.Add(lsts);

                lsts.Dispose();
                lstStatusColor.Dispose();
            }
            int dfadsi = drpVisitType.Items.Count;
            if (common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
            {
                drpVisitType.SelectedValue = "E";
            }
            lst = null;
            dvERFilter.Dispose();
            #endregion

            #region Table-4, ReferralCount

            if (ds.Tables[4].Rows.Count > 0)
            {
                lbtReferralCount.Text = "Refer OP ( " + common.myStr(common.myInt(ds.Tables[4].Rows[0]["OPReferralCount"])) + " ) , IP ( " + common.myStr(common.myInt(ds.Tables[4].Rows[0]["IPReferralCount"])) + " )";// + common.myStr(ViewState["MLCCount"]) ;
                lbtReferralCount.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }

            #endregion

            #region Table-5, CheckUserDoctorOrNot
            Session["CheckUserDoctorOrNotEmployeeId"] = "0";
            if (ds.Tables[5].Rows.Count > 0)
            {
                Session["CheckUserDoctorOrNotEmployeeId"] = common.myInt(ds.Tables[5].Rows[0]["EmpId"]);
            }

            #endregion

            #region Table-6, EmploymentStatus

            if (ds.Tables[6].Rows.Count > 0)
            {
                drpVisitType.Items.Remove(drpVisitType.Items.FindItemByValue("A"));
                drpVisitType.Items.Remove(drpVisitType.Items.FindItemByValue("O"));
                drpVisitType.Items.Remove(drpVisitType.Items.FindItemByValue("L"));
            }
            #endregion
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objemr = null;
            ds.Dispose();
        }
    }

    protected void ddlSpecilization_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (common.myStr(Session["FindPatientAccessToAdmittingDoctor"]) == "N")
        {
            BindProvider();
        }
        else
        {
            ViewState["UserSpecialisationId"] = null;
            BindProviderAndSpecialization(common.myInt(ddlSpecilization.SelectedValue));
            if (ddlSpecilization.SelectedValue != "0")
            {
                RadComboBoxItem Provider = ddlProvider.Items.FindItemByValue("0");
                if (Provider != null)
                {
                    ddlProvider.Items.Remove(0);
                }
            }
        }
        //containerPatientVisits.Attributes.Add("style", "display:none");
        //containerAppointment.Attributes.Add("style", "display:none");
        //bindStatus(rblSearchCriteria.SelectedValue);
    }
    private void BindProviderAndSpecialization(int iSpecializationId)
    {
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataSet objDs = new DataSet();
        Hashtable hsIn = new Hashtable();
        ddlSpecilization.Items.Clear();
        try
        {
            objDs = objemr.GetEMRGetPatientAccessRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), iSpecializationId, common.myInt(Session["UserId"]));
            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.ClearSelection();
                ddlSpecilization.DataSource = objDs.Tables[0];
                ddlSpecilization.DataTextField = "NAME";
                ddlSpecilization.DataValueField = "ID";
                ddlSpecilization.DataBind();
                ddlProvider.ClearSelection();
                ddlProvider.DataSource = objDs.Tables[1];
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataValueField = "DoctorID";
                ddlProvider.DataBind();
                CheckUserDoctorOrNot();
                if (objDs.Tables[0].Rows.Count > 1)
                {
                    ddlSpecilization.Items.Insert(0, new RadComboBoxItem("All", ""));
                    if (common.myStr(ViewState["UserSpecialisationId"]) != "")
                    {
                        ddlSpecilization.SelectedIndex = ddlSpecilization.Items.IndexOf(ddlSpecilization.Items.FindItemByValue(common.myStr(ViewState["UserSpecialisationId"])));
                    }
                    else
                    {
                        ddlSpecilization.SelectedIndex = ddlSpecilization.Items.IndexOf(ddlSpecilization.Items.FindItemByValue(common.myStr(iSpecializationId)));
                    }
                    ViewState["UserSpecialisationId"] = null;
                    //ddlSpecilization.Enabled = true;
                    ddlProvider.SelectedValue = common.myStr(Session["EmployeeId"]);
                    //ddlProvider.Enabled = true;
                }
                else if (objDs.Tables[1].Rows.Count > 1)
                {
                    ddlProvider.SelectedValue = common.myStr(Session["EmployeeId"]);
                    //ddlProvider.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objemr = null;
            objDs.Dispose();
            hsIn = null;
        }
    }
    private void CheckUserDoctorOrNot()
    {
        BaseC.EMR objEmr = new BaseC.EMR(sConString);
        try
        {
            if (Session["UserID"] != null)
            {
                if (common.myInt(Session["LoginIsAdminGroup"]).Equals(1))
                {
                    if (common.myBool(ViewState["ddlProviderAllBind"]))
                    {
                        ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                    }
                    ddlProvider.Items[0].Selected = true;
                    //ddlProvider.Enabled = true;
                    ViewState["IsDoctor"] = "0";
                }
                else
                {
                    if (common.myInt(Session["CheckUserDoctorOrNotEmployeeId"]) > 0 && common.myInt(Session["LoginIsAdminGroup"]).Equals(0))
                    {
                        if (common.myBool(ViewState["ddlProviderAllBind"]))
                        {
                            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                        }
                        ddlProvider.Items[0].Selected = false;
                        ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(Session["CheckUserDoctorOrNotEmployeeId"])));
                        //if (ViewState["OPIP"] != null && ViewState["OPIP"] == "E")
                        //{
                        //ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                        //ddlProvider.Items[0].Selected = true;
                        //}
                        ViewState["IsDoctor"] = "1";
                    }
                    else
                    {
                        if (common.myBool(ViewState["ddlProviderAllBind"]))
                        {
                            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                        }
                        ddlProvider.Items[0].Selected = true;
                        ViewState["IsDoctor"] = "0";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objEmr = null;
        }
    }
    private void BindLocation()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        try
        {
            if (Cache["FACILITY"] == null || ((DataSet)Cache["FACILITY"]).Tables[0].Rows.Count == 0)
            {
                //DataSet ds1 = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
                Cache["FACILITY"] = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            }
            ddlLocation.ClearSelection();
            ddlLocation.DataSource = Cache["FACILITY"];
            ddlLocation.DataTextField = "FacilityName";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new RadComboBoxItem("All", ""));
            ddlLocation.SelectedIndex = 0;
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objMaster = null;
        }
    }
    private string[] getToFromDate(string ddlTime)
    {
        //int timezone = common.myInt(Session["TimeZoneOffSetMinutes"]);
        //if (timezone.Equals(0))
        //{
        //    timezone = BindUTCTime();
        //    Session["TimeZoneOffSetMinutes"] = timezone.ToString();
        //}
        string sFromDate = "", sToDate = "";
        string[] str = new string[2];
        if (ddlTime == "4" || ddlTime == "5")
        {
            //tdDateRange.Visible = true;
            if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                //sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
                //sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";
                sFromDate = common.myStr(Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
                sToDate = common.myStr(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "DD0")
        {
            sFromDate = DateTime.Now.ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "YD")
        {
            sFromDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "WW-1")
        {
            sFromDate = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.AddDays(0).ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "WW0")
        {
            str = datecalculate();
        }
        else if (ddlTime == "MM0")
        {
            sFromDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-6")
        {
            sFromDate = DateTime.Now.AddMonths(-6).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month)) + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "YY-1")
        {
            sFromDate = DateTime.Now.AddDays(-365).ToString("yyyy/MM/dd") + " 00:00";
            sToDate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 23:59";
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime == "MM-1")
        {
            if ((DateTime.Now.Month - 1) != 0)
            {
                sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/01" + " 00:00";
                sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
            }
            else
            {
                sFromDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
                sToDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";
            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        return str;
    }
    private string[] datecalculate()
    {
        string DayName = DateTime.Now.DayOfWeek.ToString();
        string fromdate = "";
        string todate = "";
        string[] str = new string[2];
        switch (DayName)
        {
            case "Monday":
                fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Tuesday":
                fromdate = DateTime.Now.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Wednesday":
                fromdate = DateTime.Now.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Thursday":
                fromdate = DateTime.Now.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Friday":
                fromdate = DateTime.Now.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Saturday":
                fromdate = DateTime.Now.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Sunday":
                fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
                break;
        }
        str[0] = fromdate;
        str[1] = todate;
        return str;
    }
    private void BindBlankEncounterGrid()
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            dt.Columns.Add("Status");
            dt.Columns.Add("MedicalAlert");  //Added on 29-10-2014  
            dt.Columns.Add("AllergiesAlert");
            dt.Columns.Add("EncounterNo");
            dt.Columns.Add("EncounterDate");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("AgeGender");
            dt.Columns.Add("DoctorName");
            dt.Columns.Add("EncounterId");
            dt.Columns.Add("AppointmentID");
            dt.Columns.Add("DoctorID");
            dt.Columns.Add("RegistrationId");
            dt.Columns.Add("FacilityID");
            dt.Columns.Add("StatusColor");
            dt.Columns.Add("PackageId");
            dt.Columns.Add("CriticalLabResult");
            dt.Columns.Add("OPIP");
            dt.Columns.Add("IsReferralCase");
            dt.Columns.Add("ClinicalAddendumAllowed");
            dt.Columns.Add("MLC");
            dt.Columns.Add("UnReviewedLabResults");
            dt.Columns.Add("IsWalkInPatient");
            dt.Columns.Add("MHC");
            dt.Columns.Add("EncounterStatusColor");
            dt.Columns.Add("AppointmentDate");
            dt.Columns.Add("WardNameBedNo");
            dt.Columns.Add("VisitTypeStatusColor");
            dt.Columns.Add("EncounterStatus");
            dt.Columns.Add("QmsAppointmentLink");
            dt.Columns.Add("ReferralColor");
            dt.Columns.Add("AppointmentSlot");
            dt.Columns.Add("BreakId");
            dt.Columns.Add("VisitEntryDone");
            dt.Columns.Add("VisitEntryStatusColor");
            dt.Columns.Add("PatientToolTip");
            dt.Columns.Add("Contact");
            dt.Columns.Add("PatientVisit");
            dt.Columns.Add("TokenNo");
            dt.Columns.Add("QMSStatusId");
            dt.Columns.Add("QMSColor");
            dt.Columns.Add("QMSCode");
            dt.Columns.Add("AccountCategory");
            dt.Columns.Add("InvoiceNo");
            dt.Columns.Add("CPR");
            dt.Columns.Add("IsCarePlan");
            for (int i = 0; i < 1; i++)
            {
                DataRow Dr = dt.NewRow();
                Dr["Status"] = "";
                Dr["MedicalAlert"] = "";
                Dr["AllergiesAlert"] = "";
                Dr["encounterno"] = "";
                Dr["EncounterDate"] = "";
                Dr["registrationno"] = "";
                Dr["Name"] = "";
                Dr["AgeGender"] = "";
                Dr["DoctorName"] = "";
                Dr["EncounterId"] = "";
                Dr["AppointmentID"] = "";
                Dr["DoctorID"] = "";
                Dr["RegistrationId"] = "";
                Dr["FacilityID"] = "";
                Dr["StatusColor"] = "Blank";
                Dr["PackageId"] = "";
                Dr["CriticalLabResult"] = "";
                Dr["OPIP"] = "";
                Dr["IsReferralCase"] = "";
                Dr["ClinicalAddendumAllowed"] = "";
                Dr["MLC"] = "";
                Dr["UnReviewedLabResults"] = false;
                Dr["VisitTypeStatusColor"] = "";
                Dr["QmsAppointmentLink"] = "";
                Dr["ReferralColor"] = "";
                Dr["AppointmentSlot"] = "";
                Dr["BreakId"] = "";
                Dr["VisitEntryDone"] = "";
                Dr["VisitEntryStatusColor"] = "";
                Dr["PatientToolTip"] = "";
                Dr["Contact"] = "";
                Dr["PatientVisit"] = "";
                Dr["TokenNo"] = "";
                Dr["QMSStatusId"] = "";
                Dr["QMSColor"] = "";
                Dr["QMSCode"] = "";
                Dr["AccountCategory"] = "";
                Dr["InvoiceNo"] = "";
                Dr["CPR"] = "";
                Dr["IsCarePlan"] = "";
                dt.Rows.Add(Dr);
            }
            ViewState["EncBlankGrid"] = "True";
            gvEncounter.CurrentPageIndex = 0;
            gvEncounter.DataSource = dt;
            gvEncounter.DataBind();
            ds.Tables.Add(dt);
            //ViewState["dsSorting"] = ds;
            ds.Dispose();
            chkIsWalkInPatient.Text = "Walk In (0)";
            chkMHC.Text = Resources.PRegistration.MHC + " (0)";
            ChkMLC.Text = "MLC (0)";
            ChkReferralIP.Text = "Refer IP (0)";
            ChkReferralOP.Text = "Refer OP (0)";
            ViewState["Count"] = 0;
            TotalEncounterRecords();
            // ViewState["BlankGrid"] = null;

            //bindStatus(rblSearchCriteria.SelectedValue);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dt.Dispose();
            ds.Dispose();
        }
    }
    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myStr(drpVisitType.SelectedValue).Equals("L"))
        {
            if (common.myInt(ddlProvider.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please Select Provider!", this.Page);
                //BindBlankEncounterGrid();
                return;
            }
            if (common.myStr(ddlrange.SelectedValue) != "DD0" && common.myStr(ddlrange.SelectedValue) != "5")
            {
                Alert.ShowAjaxMsg("Please Select Today or Go To Date!", this.Page);
                //BindBlankEncounterGrid();
                return;
            }
        }
        // ViewState["ddlAppointmentStatusSelectedValue"] = common.myStr(ddlAppointmentStatus.SelectedValue);
        List<string> ddlAppointmentStatusValue = new List<string>();
        foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
        {
            if (currentItem.Checked)
            {
                ddlAppointmentStatusValue.Add(currentItem.Value);
            }
        }
        if (ddlrange.SelectedValue == "4")
        {
            tblDateRange.Visible = true;
            dtpfromDate.Visible = true;
            dtpToDate.Visible = true;
            spTo.Visible = true;
        }
        else
        {
            if (ddlrange.SelectedValue == "5")
            {
                tblDateRange.Visible = true;
                dtpfromDate.Visible = true;
                dtpToDate.Visible = true;
                spTo.Visible = true;
            }
            else
            {
                tblDateRange.Visible = false;
            }
        }
        ViewState["SelectedDate"] = (common.myStr(ddlrange.SelectedValue).Equals("A") ? string.Empty : common.myStr(ddlrange.SelectedValue));
        //bindStatus(rblSearchCriteria.SelectedValue);
        foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
        {
            currentItem.Checked = false;
            foreach (string AppointmentStatusValueChecked in ddlAppointmentStatusValue)
            {
                if (currentItem.Value.Equals(AppointmentStatusValueChecked))
                {
                    currentItem.Checked = true;
                }
            }
        }
    }
    public string AppointmentType(int WalkInPatient, int MHC, bool MLC)
    {
        if (WalkInPatient.Equals(0) && MHC.Equals(0) && !MLC)
        {
            if (common.myInt(ViewState["AppointmentId"]) != 0)
            {
                return "AppointmentType : Appointment";
            }
            else
            {
                return string.Empty;
            }
        }
        else if (WalkInPatient.Equals(0) && MHC.Equals(0) && MLC)
        {
            return "AppointmentType : MLC";
        }
        else if (WalkInPatient.Equals(0) && MHC.Equals(1) && !MLC)
        {
            return "AppointmentType : MHC";
        }
        else if (WalkInPatient.Equals(0) && MHC.Equals(1) && MLC)
        {
            return "AppointmentType : MHC / MLC";
        }
        else if (WalkInPatient.Equals(1) && MHC.Equals(0) && !MLC)
        {
            return "AppointmentType : Walk In Patient";
        }
        else if (WalkInPatient.Equals(1) && MHC.Equals(0) && MLC)
        {
            return "AppointmentType : Walk In Patient / MLC";
        }
        else if (WalkInPatient.Equals(1) && MHC.Equals(1) && !MLC)
        {
            return "AppointmentType : Walk In Patient / MHC";
        }
        else if (WalkInPatient.Equals(1) && MHC.Equals(1) && MLC)
        {
            return "AppointmentType : Walk In Patient / MHC / MLC";
        }
        return string.Empty;
    }
    protected void rblSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblGridStatus.Text = "No Record Found!";
        SetControls();
        bindSearchData(false);
        if (rblSearchCriteria.SelectedValue == "2")
        {
            containerPatientVisits.Attributes.Add("style", "display:none");
        }
        Session["FindPatientSEARCHCRITERIA"] = common.myStr(rblSearchCriteria.SelectedValue);
    }
    private void SetControls()
    {
        RadComboBoxItem ls = new RadComboBoxItem();
        RadComboBoxItem lst3 = new RadComboBoxItem();
        RadComboBoxItem lst10 = new RadComboBoxItem();
        RadComboBoxItem lst4 = new RadComboBoxItem();
        RadComboBoxItem lst9 = new RadComboBoxItem();
        RadComboBoxItem lst8 = new RadComboBoxItem();
        RadComboBoxItem lst6 = new RadComboBoxItem();
        RadComboBoxItem lst7 = new RadComboBoxItem();
        RadComboBoxItem lst12 = new RadComboBoxItem();
        RadComboBoxItem lst = new RadComboBoxItem();
        RadComboBoxItem lst1 = new RadComboBoxItem();
        RadComboBoxItem lst2 = new RadComboBoxItem();
        RadComboBoxItem lst11 = new RadComboBoxItem();
        try
        {
            BindLocation();
            //BindProviderAndSpecialization(common.myInt(ViewState["UserSpecialisationId"]));

            //BindProvider();

            //bindStatus(rblSearchCriteria.SelectedValue); //Mahendra Jasiwal
            ddlrange.Items.Clear();
            if (common.myInt(ViewState["IsDoctor"]) == 0)
            {
                //ddlProvider.Enabled = true;
            }
            if (rblSearchCriteria.SelectedValue == "2") //Encounter List
            {
                ls.Text = "Select All";
                ls.Value = "A";
                ddlrange.Items.Add(ls);
                lst3.Text = "Today";
                lst3.Value = "DD0";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "DD0")
                {
                    lst3.Selected = true;
                }
                else if (ViewState["SelectedDate"] == null)
                {
                    lst3.Selected = true;
                }
                ddlrange.Items.Add(lst3);
                lst10.Text = "Yesterday";
                lst10.Value = "YD";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YD")
                {
                    lst10.Selected = true;
                }
                ddlrange.Items.Add(lst10);
                lst4.Text = "Last Week";
                lst4.Value = "WW-1";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW-1")
                {
                    lst4.Selected = true;
                }
                ddlrange.Items.Add(lst4);
                lst9.Text = "Last Month";
                lst9.Value = "MM-1";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM-1")
                {
                    lst9.Selected = true;
                }
                ddlrange.Items.Add(lst9);
                if ((common.myStr(ddlrange.SelectedValue).Equals("A") ? string.Empty : common.myStr(ddlrange.SelectedValue)) == string.Empty)
                {
                    ddlrange.SelectedValue = "DD0";
                }
                lst8.Text = "Last Six Months";
                lst8.Value = "MM-6";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM-6")
                {
                    lst8.Selected = true;
                }
                ddlrange.Items.Add(lst8);
                lst6.Text = "Last One Year";
                lst6.Value = "YY-1";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY-1")
                {
                    lst6.Selected = true;
                }
                ddlrange.Items.Add(lst6);
                lst7.Text = "Date Range";
                lst7.Value = "4";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "4")
                {
                    lst7.Selected = true;
                }
                ddlrange.Items.Add(lst7);
                lst12.Text = "This Week Appoinment";
                lst12.Value = "WW0";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW0")
                {
                    lst12.Selected = true;
                }
                ddlrange.Items.Add(lst12);
                lst.Text = "Next Week Appoinment";
                lst.Value = "WW+1";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+1")
                {
                    lst.Selected = true;
                }
                ddlrange.Items.Add(lst);
                lst1.Text = "Next Two Week Appoinment";
                lst1.Value = "WW+2";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+2")
                {
                    lst1.Selected = true;
                }
                ddlrange.Items.Add(lst1);
                lst2.Text = "Next Year Appoinment";
                lst2.Value = "YY+1";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY+1")
                {
                    lst2.Selected = true;
                }
                ddlrange.Items.Add(lst2);
                lst11.Text = "Go To Date";
                lst11.Value = "5";
                if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "5")
                {
                    lst11.Selected = true;
                }
                ddlrange.Items.Add(lst11);
                if (ddlrange.SelectedValue == "4")
                {
                    tblDateRange.Visible = true;
                    dtpfromDate.Visible = true;
                    dtpToDate.Visible = true;
                    spTo.Visible = true;
                }
                else
                {
                    if (ddlrange.SelectedValue == "5")
                    {
                        tblDateRange.Visible = true;
                        dtpfromDate.Visible = true;
                        dtpToDate.Visible = false;
                        spTo.Visible = false;
                    }
                    else
                    {
                        tblDateRange.Visible = false;
                    }
                }
                //txtLegendPackage.Visible = true;
                //lblLegendPackage.Visible = true;
                //txtLegend.Visible = true;
                //lblLegend.Visible = true;
                //txtMlC.Visible = true;
                //lblMlC.Visible = true;
            }
            if ((common.myStr(Session["LoginEmployeeType"]) == "D" || common.myStr(Session["LoginEmployeeType"]) == "N") && (rblSearchCriteria.SelectedValue == "2" || rblSearchCriteria.SelectedValue == "8"))
            {
                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
                ddlLocation.Enabled = false;
            }
            else
            {
                ddlLocation.Enabled = true;
            }
        }
        catch
        {
        }
        finally
        {
            ls.Dispose();
            lst3.Dispose();
            lst10.Dispose();
            lst4.Dispose();
            lst9.Dispose();
            lst8.Dispose();
            lst6.Dispose();
            lst7.Dispose();
            lst12.Dispose();
            lst.Dispose();
            lst1.Dispose();
            lst2.Dispose();
            lst11.Dispose();
        }
    }
    private void BindcontainerAppointmentNew()
    {
        DataTable Colordt = new DataTable();
        try
        {
            if (!common.myStr(ViewState["dsFinalData"]).Equals(string.Empty))
            {
                Colordt = (DataTable)ViewState["dsFinalData"];
            }

            containerAppointmentNew.Controls.Clear();
            if (Colordt != null)
            {
                if (Colordt.Rows.Count > 0)
                {
                    for (int i = 0; i < Colordt.Rows.Count; i++)
                    {
                        HtmlGenericControl s = new HtmlGenericControl("span");
                        //s.Attributes.Add("style", "background:#98AFC7; color:#fff");
                        s.Attributes["class"] = "mlabel";
                        s.Attributes.Add("style", "background:" + common.myStr(Colordt.Rows[i]["StatusColor"]) + ";" + " color:#000" + ";font-family:Arial;font-size:8pt;");
                        s.Attributes.Add("id", common.myStr(Colordt.Rows[i]["StatusId"]));
                        switch (common.myStr(Colordt.Rows[i]["Code"]))
                        {
                            case "U"://UnConf
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppUnConf"]) + ")";
                                break;
                            case "A"://AppConfirm
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppConfirm"]) + ")";
                                break;
                            case "P"://AppChkIn
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppChkIn"]) + ")";
                                break;
                            case "O"://AppChkOut
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppChkOut"]) + ")";
                                break;
                            case "M"://AppNoShow
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppNoShow"]) + ")";
                                break;
                            case "C"://AppCancel
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppCancel"]) + ")";
                                break;
                            case "W"://AppWaited
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppWaited"]) + ")";
                                break;
                            case "PC"://AppPayment
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppPayment"]) + ")";
                                break;
                            case "S"://Seen
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["AppSeen"]) + ")";
                                break;
                            case "VT"://VitalsTaken
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["VitalsTaken"]) + ")";
                                break;
                            case "D"://Closed/Discharged
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]) + " (" + common.myStr(ViewState["Discharged"]) + ")";
                                break;
                            default:
                                s.InnerHtml = common.myStr(Colordt.Rows[i]["Status"]);
                                break;
                        }
                        containerAppointmentNew.Controls.Add(s);
                    }
                }
            }
        }
        catch
        {
        }
        finally
        {
            Colordt.Dispose();
        }
    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    protected void btn_ClearFilter_Click(object sender, EventArgs e)
    {
        DataSet dsDD = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        try
        {
            ViewState["ddlProviderAllBind"] = false;
            txtSearch.Text = string.Empty;
            txtSearchN.Text = string.Empty;
            foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
            {
                currentItem.Checked = true;
            }
            dsDD = objivf.getEMRDoctorDetails(common.myInt(Session["EmployeeId"]));
            Session["FindPatientDefaultStatusIds"] = string.Empty;
            Session["FindPatientEMRSTATUSID"] = string.Empty;
            if (dsDD.Tables[0].Rows.Count > 0)
            {
                Session["FindPatientDefaultStatusIds"] = common.myStr(dsDD.Tables[0].Rows[0]["FindPatientDefaultStatusIds"]);
                if (common.myLen(Session["FindPatientDefaultStatusIds"]) > 0)
                {
                    List<string> lstFindPatientEMRSTATUSID = new List<string>();
                    lstFindPatientEMRSTATUSID = common.myStr(Session["FindPatientDefaultStatusIds"]).Trim().Split(',').ToList();
                    Session["FindPatientEMRSTATUSID"] = lstFindPatientEMRSTATUSID;
                }
            }
            if (common.myLen(Session["FindPatientEMRSTATUSID"]) > 0)
            {
                List<string> lstFindPatientEMRSTATUSID = new List<string>();
                lstFindPatientEMRSTATUSID = (List<string>)Session["FindPatientEMRSTATUSID"];
                foreach (RadComboBoxItem currentItem in ddlAppointmentStatus.Items)
                {
                    currentItem.Checked = false;
                    foreach (string AppointmentStatusValueChecked in lstFindPatientEMRSTATUSID)
                    {
                        if (currentItem.Value.Equals(AppointmentStatusValueChecked))
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
            }
            ddlLocation.SelectedIndex = 0;
            dtpfromDate.Clear();
            dtpfromDate.DateInput.Clear();
            dtpToDate.Clear();
            dtpToDate.DateInput.Clear();
            if (common.myInt(ViewState["IsDoctor"]) == 0)
            {
                //ddlProvider.Enabled = true;
            }
            if (rblSearchCriteria.SelectedValue == "4")
            {
                ddlrange.SelectedIndex = 3;
            }
            else
            {
                ddlrange.SelectedIndex = 1;
            }
            if (rblSearchCriteria.SelectedValue == "5" || rblSearchCriteria.SelectedValue == "6" || rblSearchCriteria.SelectedValue == "7")
            {
                ddlrange.SelectedIndex = 3;
            }
            else
            {
                ddlrange.SelectedIndex = ddlrange.Items.IndexOf(ddlrange.Items.FindItemByValue("DD0"));
            }
            //ddlrange_SelectedIndexChanged(this, null);
            ddlName.SelectedIndex = 0;
            //ddlName_OnTextChanged(null, null);
            ddlSpecilization.SelectedIndex = 0;
            //ddlSpecilization_SelectedIndexChanged(null, null);
            drpVisitType.SelectedIndex = 0;
            ddlProvider.SelectedIndex = 0;
            CheckUserDoctorOrNot();
            Session["FindPatientSEARCHCRITERIA"] = common.myStr(rblSearchCriteria.SelectedValue);
            Session["FindPatientSEARCHOPTION"] = common.myStr(ddlName.SelectedValue);
            Session["FindPatientFINDWARDID"] = ShowCheckedItems(ddlWard);
            Session["FindPatientSPECIALISATIONID"] = string.Empty;
            Session["FindPatientPROVIDERID"] = string.Empty;
            Session["FindPatientPROVIDERID_S"] = string.Empty;
            Session["FindPatientENCOUNTERNO"] = string.Empty;
            Session["FindPatientREGISTRATIONNO"] = string.Empty;
            Session["FindPatientOLDREGISTRATIONNO"] = string.Empty;
            Session["FindPatientPATIENTNAME"] = string.Empty;
            Session["FindPatientENROLLENO"] = string.Empty;
            Session["FindPatientMOBILENO"] = string.Empty;
            Session["FindPatientIVFNO"] = string.Empty;
            Session["FindPatientSTATUSID"] = string.Empty;
            //Session["FindPatientEMRSTATUSID"] = string.Empty;
            Session["FindPatientVISITTYPE"] = "O";
            Session["FindPatientDATERANGE"] = common.myStr(ddlrange.SelectedValue);
            Session["FindPatientFROMDATE"] = string.Empty;
            Session["FindPatientTODATE"] = string.Empty;
            bindSearchData(false);
            //bindStatus(rblSearchCriteria.SelectedValue);
            ddlrange_SelectedIndexChanged(null, null);
            ViewState["ddlProviderAllBind"] = true;
        }
        catch
        {
        }
        finally
        {
            dsDD.Dispose();
            objivf = null;
        }
    }
    protected void lnkbtn_Refresh_OnClick(object sender, EventArgs e)
    {
        bindSearchData(false);
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSearch.Text = "";
            txtSearchN.Text = "";
            txtSearchN.Visible = false;
            txtSearch.Visible = false;
            ddlPatientAge.Visible = false;
            if (common.myStr(ddlName.SelectedValue).Equals("R") || common.myStr(ddlName.SelectedValue).Equals("IVF"))
            {
                txtSearchN.Visible = true;
            }
            else if (common.myStr(ddlName.SelectedValue).Equals("PA"))
            {
                ddlPatientAge.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
            //bindStatus(rblSearchCriteria.SelectedValue);
        }
        catch
        {
        }
    }
    protected void dtpfromDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (common.myStr(ddlrange.SelectedValue).Equals("5"))
        {
            dtpToDate.SelectedDate = dtpfromDate.SelectedDate;
        }
    }
    protected void gvEncounter_OnPreRender(object sender, EventArgs e)
    {
        try
        {
            if (common.myBool(Session["FindPatientPageIndexChanged"]))
            {
                bindSearchData(false);
                Session["FindPatientPageIndexChanged"] = 0;
            }
            SetFindPatientVisitsColumnsVisibilitySetup();
        }
        catch
        {
        }
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            gvEncounter.CurrentPageIndex = e.NewPageIndex;
            Session["FindPatientPageIndexChanged"] = 1;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }
    int idxL = 0;
    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (Session["ItemIndex"] != null && !common.myStr(Session["ItemIndex"]).Equals(string.Empty))
                {
                    if ((common.myInt(item.ItemIndex)).Equals(common.myInt(Session["ItemIndex"])))   // Get the  ItemIndex
                    {
                        item["Select"].Style.Add("font-weight", "bold");
                        item["Select"].Focus();
                    }
                    else
                    {
                        item["Select"].Style.Add("font-weight", "normal");
                    }
                }
            }
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;
                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
            //Byte A;
            //Byte R;
            //Byte G;
            //Byte B;
            //String htmlHexColorValue = "";
            string ToolTipAppointmentType = string.Empty;
            int sTotal = common.myInt(ViewState["Count"]);
            //  string AppointmentType = string.Empty;
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["PrevRow"] = null;
                ViewState["CurrRow"] = null;
                HiddenField hdnIsWalkInPatient = (HiddenField)item.FindControl("hdnIsWalkInPatient");
                HiddenField hdnMHC = (HiddenField)item.FindControl("hdnMHC");
                HiddenField hdnMLC = (HiddenField)item.FindControl("hdnMLC");
                //HiddenField hndCompanyColorStatus = (HiddenField)item.FindControl("hndCompanyColorStatus");
                HiddenField hdnIsReferralCase = (HiddenField)item.FindControl("hdnIsReferralCase");
                //HiddenField hdnIsReferredCase = (HiddenField)item.FindControl("hdnIsReferredCase");
                LinkButton lnkCall = (LinkButton)item.FindControl("lnkCall");
                HiddenField hdnEncounterId = (HiddenField)item.FindControl("hdnEncounterId");
                if (ViewState["Blank"] == null)
                {
                    HiddenField AppStatusColor = (HiddenField)item.FindControl("hdnStatusColor");
                    Label lbl_StatusColor = new Label();
                    int hdnPackageId = common.myInt(((HiddenField)item.FindControl("hdnEPackageId")).Value);
                    Label lblVisitType = (Label)item.FindControl("lblVisitType");
                    HiddenField hdnMedicalAlert = (HiddenField)item.FindControl("hdnMedicalAlert");
                    HiddenField hdnAllergiesAlert = (HiddenField)item.FindControl("hdnAllergiesAlert");
                    ImageButton imgMedicalAlert = (ImageButton)item.FindControl("imgMedicalAlert");
                    ImageButton imgAllergyAlert = (ImageButton)item.FindControl("imgAllergyAlert");
                    HiddenField hdnOPIP = (HiddenField)item.FindControl("hdnOPIP");
                    Label lblAppointmentDate = (Label)item.FindControl("lblAppointmentDate");
                    LinkButton lnkAddendum = (LinkButton)item.FindControl("lnkAddendum");
                    HiddenField hdnClinicalAddendumAllowed = (HiddenField)item.FindControl("hdnClinicalAddendumAllowed");
                    Label lblName = (Label)item.FindControl("lblName");
                    imgAllergyAlert.Visible = common.myBool(hdnAllergiesAlert.Value);
                    imgMedicalAlert.Visible = common.myBool(hdnMedicalAlert.Value);
                    lnkAddendum.Visible = common.myBool(hdnClinicalAddendumAllowed.Value);
                    HiddenField hdnUnReviewedLabResults = (HiddenField)item.FindControl("hdnUnReviewedLabResults");
                    ImageButton imgLabResult = (ImageButton)item.FindControl("imgLabResult");
                    HiddenField hdnQMSColor = (HiddenField)item.FindControl("hdnQMSColor");
                    HiddenField hdnQMSCode = (HiddenField)item.FindControl("hdnQMSCode");
                    HiddenField hdnEncounterStatus = (HiddenField)item.FindControl("hdnEncounterStatus");
                    ImageButton imgDiagnosticHistory = (ImageButton)item.FindControl("imgDiagnosticHistory");
                    ImageButton imgPastClinicalNote = (ImageButton)item.FindControl("imgPastClinicalNote");
                    HiddenField hdnRegistrationID = (HiddenField)item.FindControl("hdnRegistrationID");
                    HiddenField hdnEncounterNo = (HiddenField)item.FindControl("hdnEncounterNo");
                    Label lblRegistrationNo = (Label)item.FindControl("lblRegistrationNo");
                    HiddenField hdnAppointmentSlot = (HiddenField)e.Item.FindControl("hdnAppointmentSlot");
                    Label lblStatus = (Label)item.FindControl("lblStatus");
                    HiddenField hdnBreakId = (HiddenField)e.Item.FindControl("hdnBreakId");
                    HiddenField hdnPatientToolTip = (HiddenField)e.Item.FindControl("hdnPatientToolTip");

                    ImageButton imgCarePlan = (ImageButton)e.Item.FindControl("imgCarePlan");
                    HiddenField hdnIsCarePlan = (HiddenField)e.Item.FindControl("hdnIsCarePlan");

                    LinkButton lnkTokenNo = (LinkButton)e.Item.FindControl("lblTokenNo");

                    imgCarePlan.Visible = false;
                    if (common.myBool(hdnIsCarePlan.Value))
                    {
                        imgCarePlan.Visible = true;
                    }


                    if (common.myStr(drpVisitType.SelectedValue).Equals("L"))
                    {
                        if (idxL == 0)
                        {
                            ViewState["StartTime"] = lblAppointmentDate.Text.Split(' ')[1];
                            idxL++;
                        }
                        ViewState["EndTime"] = lblAppointmentDate.Text.Split(' ')[1];
                        string type = "";
                        if (!common.myStr(lblName.Text).Equals(""))
                        {
                            type = common.myStr(lblName.Text);
                            if (common.myStr(lblName.Text).Contains("-"))
                            {
                                type = common.myStr(lblName.Text).Split('-')[0];
                            }
                        }
                        if (common.myStr(type).Equals("Break") || common.myStr(type).Equals("Reserved") || common.myStr(type).Equals(""))
                        {
                            for (int j = 1; j < item.Cells.Count; j++)
                            {
                                item.Cells[j].Attributes.Add("OnClick", "showBreakBlock(event,'" + menuStatus.ClientID + "','" +
                                common.myStr(type).Trim() + "','" +// BREAK/BLOCK  
                                common.myStr(lblAppointmentDate.Text).Trim() + "','" +// AppointmentDate  
                                common.myStr(hdnAppointmentSlot.Value).Trim() + "','" +// AppointmentSlot 
                                common.myStr(hdnBreakId.Value).Trim() + "','" +
                                common.myInt(item.ItemIndex) + "')"); //RowIdx
                            }
                        }
                    }
                    if (common.myInt(hdnRegistrationID.Value) < 1)
                    {
                        lblRegistrationNo.Visible = false;
                    }
                    if (common.myInt(hdnRegistrationID.Value) > 0)
                    {
                        imgDiagnosticHistory.Visible = true;
                        imgPastClinicalNote.Visible = true;
                    }
                    else
                    {
                        imgDiagnosticHistory.Visible = false;
                        imgPastClinicalNote.Visible = false;
                    }
                    if (common.myBool(hdnUnReviewedLabResults.Value))
                    {
                        imgLabResult.Visible = true;
                    }
                    else
                    {
                        imgLabResult.Visible = false;
                    }
                    if (lblVisitType.Text == "E")
                    {
                        lblVisitType.Text = "ER";
                    }
                    else if (lblVisitType.Text == "O")
                    {
                        lblVisitType.Text = "OP";
                        lnkTokenNo.Visible = true;
                    }
                    else if (lblVisitType.Text == "I")
                    {
                        lblVisitType.Text = "IP";
                    }
                    HiddenField hdnAppointmentID = (HiddenField)item.FindControl("hdnAppointmentID");
                    ViewState["AppointmentId"] = 0;
                    if (common.myInt(hdnAppointmentID.Value) > 0)
                    {
                        lnkCall.Visible = true;
                    }
                    else
                    {
                        lnkCall.Visible = false;
                    }
                    Session["EncVisitPackageId"] = "";
                    ToolTipAppointmentType = AppointmentType(common.myInt(hdnIsWalkInPatient.Value), common.myInt(hdnMHC.Value), common.myBool(hdnMLC.Value));

                    item.ToolTip = common.myStr(hdnPatientToolTip.Value).Replace("\\r\\n", "\r\n") +
                                (lblVisitType.Text.Equals("IP") ? string.Empty : System.Environment.NewLine + common.myStr(ToolTipAppointmentType));

                    if (common.myStr(AppStatusColor.Value).Trim() != "Blank" && common.myStr(AppStatusColor.Value).Trim() != "&nbsp;")
                    {
                        ((TableCell)item["Visit"]).BackColor = System.Drawing.ColorTranslator.FromHtml(AppStatusColor.Value);
                    }
                    else
                    {
                        //MakeGridViewHeaderClickable(gvEncounter, e.Row);
                        if (item.ItemType == GridItemType.Item
                            || item.ItemType == GridItemType.AlternatingItem)
                        {
                            ViewState["PrevRow"] = null;
                            ViewState["CurrRow"] = null;
                        }
                    }
                }
                ImageButton imgReffHistory = (ImageButton)item.FindControl("imgReffHistory");
                ImageButton imgCritical = (ImageButton)item.FindControl("imgCritical");
                HiddenField hdnEncounterno = (HiddenField)item.FindControl("hdnencounterno");
                HiddenField hdnCriticalLabResult = (HiddenField)item.FindControl("hdnCriticalLabResult");
                HiddenField hdnOPIP2 = (HiddenField)item.FindControl("hdnOPIP");
                HiddenField hdnReferralColor = (HiddenField)item.FindControl("hdnReferralColor");
                HiddenField hdnVisitEntryDone = (HiddenField)e.Item.FindControl("hdnVisitEntryDone");  //Added on 03022020
                HiddenField hdnVisitEntryStatusColor = (HiddenField)e.Item.FindControl("hdnVisitEntryStatusColor");
                if (!common.myBool(hdnIsWalkInPatient.Value))
                {
                    if (common.myBool(hdnIsReferralCase.Value))
                    {
                        imgReffHistory.Visible = true;
                    }
                    else
                    {
                        imgReffHistory.Visible = false;
                    }
                }
                else
                {
                    imgReffHistory.Visible = false;
                }
                if (common.myBool(hdnCriticalLabResult.Value))
                {
                    imgCritical.Visible = true;
                    imgCritical.ToolTip = "Test results with " + common.myStr(Resources.PRegistration.PanicValue).ToLower();
                    imgCritical.Enabled = true;
                }
                else
                {
                    imgCritical.Visible = false;
                    imgCritical.ToolTip = string.Empty;
                    imgCritical.Enabled = false;
                }
                HiddenField hdnEncounterStatusColor = (HiddenField)item.FindControl("hdnEncounterStatusColor");
                HiddenField hdnVisitTypeStatusColor = (HiddenField)item.FindControl("hdnVisitTypeStatusColor");
                //((TableCell)item["Select"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["AppointmentDate"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["PatientName"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["AgeGender"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["RegistrationNo"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["EncounterNo"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["EncounterDate"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["WardNameBedNo"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["DoctorName"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["Notification"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                //((TableCell)item["CALL"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                ((TableCell)item["Status"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnEncounterStatusColor.Value);
                if (common.myLen(hdnReferralColor.Value) > 0)
                {
                    ((TableCell)item["DoctorName"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnReferralColor.Value);
                }
                if (common.myBool(hdnVisitEntryDone.Value)) //Added on 03022020
                {
                    ((TableCell)item["PatientName"]).BackColor = System.Drawing.ColorTranslator.FromHtml(hdnVisitEntryStatusColor.Value);
                }
                string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime sEncounterDate = common.myDate((common.myStr(((Label)e.Item.FindControl("lblEncounterDate")).Text).Split(' '))[0]);
                LinkButton lnkSelect = (LinkButton)e.Item.FindControl("lnkSelect");
                if (common.myDate(sEncounterDate) > DateTime.Today || common.myInt(hdnEncounterId.Value) == 0)
                {
                    lnkSelect.Visible = false;
                    lnkCall.Visible = false;
                }
                else
                {
                    lnkSelect.Visible = true;
                    lnkCall.Visible = true;
                    lnkSelect.Text = "Select";
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
        }
    }
    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        StringBuilder sbPatientDetail = new StringBuilder();
        DataSet dsPatientDetail = new DataSet();
        DataSet dsModule = new DataSet();
        DataSet dsDefaultTemplate = new DataSet();
        DataView dvMod = new DataView();
        Hashtable htIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        BaseC.EMRMasters objEMRMaster = new BaseC.EMRMasters(sConString);
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataSet dsAttributes = new DataSet();
        StringBuilder sbSQL = new StringBuilder();
        DataTable dt = new DataTable();
        string strPageUrl = "/EMR/Dashboard/PatientDashboardForDoctor.aspx";
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                return;
            }
            string sRegistrationId = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationID")).Value);
            string sRegistrationNo = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
            string sEncounterId = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
            string sEncounterNo = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text);
            string sEncounterDate = common.myStr((common.myStr(((Label)e.Item.FindControl("lblEncounterDate")).Text).Split(' '))[0]);
            string sDoctorId = common.myStr(((HiddenField)e.Item.FindControl("hdnDoctorID")).Value);
            string lnkSelect = common.myStr(((LinkButton)e.Item.FindControl("lnkSelect")).Text);
            HiddenField hdnOPIP = (HiddenField)e.Item.FindControl("hdnOPIP");
            HiddenField hdnAppointmentID = (HiddenField)e.Item.FindControl("hdnAppointmentID");
            LinkButton lnkCall = (LinkButton)e.Item.FindControl("lnkCall");
            if (e.CommandName.ToUpper().Equals("APPOINTMENT") && lnkCall.Text.Trim().ToUpper() == "CALL")
            {
                string str = objEMRMaster.SaveAQmsAppointment(common.myInt(hdnAppointmentID.Value), common.myInt(Session["UserId"]));
                lnkCall.Text = "Cancel";
            }
            else if (e.CommandName.ToUpper().Equals("APPOINTMENT") && lnkCall.Text.Trim().ToUpper() == "CANCEL")
            {
                htIn.Add("@intAppointmentId", common.myInt(hdnAppointmentID.Value));
                string strSQL = "delete FROM AQmsAppointment WHERE appNo = @intAppointmentId";
                objDl.ExecuteNonQuery(CommandType.Text, strSQL, htIn);
                lnkCall.Text = "Call";
            }
            if (e.CommandName.ToUpper().Equals("SELECTENCOUNTER"))
            {
                HiddenField hdnClinicalAddendumAllowed = (HiddenField)e.Item.FindControl("hdnClinicalAddendumAllowed");
                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    if (common.myStr(lnkSelect).Trim() != "")
                    {
                        ((TableCell)item["Select"]).Style.Add("font-weight", "bold");
                        Session["ItemIndex"] = item.ItemIndex;
                    }
                    item.Dispose();
                }
                //gvEncounter.ClearSelection();
                int nRowIndex = gvEncounter.Items.Count - 1;
                gvEncounter.Items[nRowIndex].Selected = true;
                gvEncounter.Items[nRowIndex].Cells[(byte)enumEncounter.RegistrationNo].Focus();//  .Selected = true;
                //GridDataItem dataItem = (GridDataItem)gvEncounter.SelectedItems[gvEncounter.SelectedItems.Count - 1];
                //string ID = dataItem.GetDataKeyValue("Select").ToString();
                //TableCell cell = dataItem["ColumnUniqueName"];
                if (common.myStr(lblStatus.Text).ToUpper().Contains("CLOSE") && common.myInt(hdnClinicalAddendumAllowed.Value) != 1 && !common.myBool(Session["isEMRSuperUser"]))
                {
                    int FilePermissionStatus = objEMRMaster.ValidateUserForEMRFile(common.myInt(Session["HospitalLocationId"]), common.myInt(sRegistrationId), common.myInt(sEncounterId), common.myInt(Session["UserId"]));
                    if (FilePermissionStatus == 1)
                    {
                        ViewState["isEMRAllow"] = 1;
                    }
                    else
                    {
                        ViewState["isEMRAllow"] = 0;
                    }
                    return;
                }
                else
                {
                    ViewState["isEMRAllow"] = 1;
                }
                if (common.myInt(ViewState["isEMRAllow"]).Equals(1))
                {
                    if (Session["UserID"] != null)
                    {
                        if (common.myBool(ViewState["EncBlankGrid"]))
                        {
                            return;
                        }
                        Session["IsPackagePatient"] = null;
                        Session["IsPanelPatient"] = null;
                        Session["AllowPanelExcludedItems"] = null;
                        Session["IsMedicalAlert"] = null;
                        Session["IsAllergiesAlert"] = null;

                        dsAttributes = objEMRMaster.getDoctorPatientListsAttributes(common.myInt(Session["FacilityId"]), common.myInt(sEncounterId),
                                       common.myStr(hdnOPIP.Value), common.myInt(Session["UserId"]), common.myInt(Session["GroupId"])); //UspEMRGetDoctorPatientListsAttributes

                        if (dsAttributes.Tables[0].Rows.Count > 0)
                        {
                            Session["MHCReportFormatId"] = common.myStr(dsAttributes.Tables[0].Rows[0]["MHCReportFormatId"]);
                            Session["Gender"] = common.myStr(dsAttributes.Tables[0].Rows[0]["Gender"]);
                            Session["IsPanelPatient"] = common.myStr(dsAttributes.Tables[0].Rows[0]["IsPanel"]);
                            Session["AllowPanelExcludedItems"] = common.myStr(dsAttributes.Tables[0].Rows[0]["AllowPanelExcludedItems"]);
                            Session["InvoiceNo"] = common.myStr(dsAttributes.Tables[0].Rows[0]["InvoiceNo"]);
                            Session["InvoiceId"] = common.myStr(dsAttributes.Tables[0].Rows[0]["InvoiceId"]);
                            Session["IsReferralCase"] = common.myStr(dsAttributes.Tables[0].Rows[0]["IsReferredCase"]);
                            Session["EncVisitPackageName"] = common.myStr(dsAttributes.Tables[0].Rows[0]["PackageName"]);

                            strPageUrl = common.myStr(dsAttributes.Tables[0].Rows[0]["PageUrl"]);
                        }
                        Session["Facility"] = common.myStr(((HiddenField)e.Item.FindControl("hdnFacilityID")).Value);
                        Session["AppointmentID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnAppointmentID")).Value);
                        Session["RegistrationID"] = sRegistrationId;
                        Session["RegistrationNo"] = sRegistrationNo;
                        Session["EncounterId"] = sEncounterId;
                        Session["EncounterNo"] = sEncounterNo;
                        Session["DoctorID"] = sDoctorId;
                        Session["FollowUpDoctorId"] = sDoctorId;
                        Session["EncounterStatus"] = common.myStr(((Label)e.Item.FindControl("lblStatus")).Text);
                        Session["OPIP"] = common.myStr(((HiddenField)e.Item.FindControl("hdnOPIP")).Value);
                        Session["PatientName"] = common.myStr(((Label)e.Item.FindControl("lblName")).Text.Trim());
                        Session["EncounterDate"] = common.myStr(((Label)e.Item.FindControl("lblEncounterDate")).Text);
                        Session["MedicalAlert"] = common.myStr(((HiddenField)e.Item.FindControl("hdnMedicalAlert")).Value);
                        Session["AllergiesAlert"] = common.myStr(((HiddenField)e.Item.FindControl("hdnAllergiesAlert")).Value);
                        Session["AgeGender"] = common.myStr(((Label)e.Item.FindControl("lblAgeGender")).Text);
                        try
                        {
                            if (common.myInt(((HiddenField)e.Item.FindControl("hdnEPackageId")).Value) > 0)
                            {
                                Session["IsPackagePatient"] = true;
                            }
                        }
                        catch
                        {
                        }
                        if (common.myStr(Session["OPIP"]) == "E")
                        {
                            Session["EREncounterId"] = Session["EncounterId"];
                        }
                        #region PatientDetailStringOP
                        Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                        Session["PatientDetailString"] = null;

                        dsPatientDetail = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]), common.myInt(Session["RegistrationId"]),
                                                common.myStr(Session["EncounterNo"]), common.myInt(Session["EncounterId"]));

                        Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dsPatientDetail;

                        if (dsPatientDetail != null)
                        {
                            if (dsPatientDetail.Tables.Count > 0)
                            {
                                if (dsPatientDetail.Tables[0].Rows.Count > 0)
                                {
                                    string sRegNoTitle = Resources.PRegistration.regno;
                                    string sDoctorTitle = Resources.PRegistration.Doctor;
                                    Session["CompanyId"] = common.myStr(dsPatientDetail.Tables[0].Rows[0]["PayorId"]);
                                    string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                                    sbPatientDetail = new StringBuilder();
                                    sbPatientDetail.Append("<b><span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]));
                                    sbPatientDetail.Append(", ");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["AgeGender"]));
                                    sbPatientDetail.Append("</span>&nbsp;");
                                    sbPatientDetail.Append(sRegNoTitle);
                                    sbPatientDetail.Append("&nbsp;<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]));
                                    sbPatientDetail.Append("</span>&nbsp;Enc #:");
                                    sbPatientDetail.Append("<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]));
                                    sbPatientDetail.Append("</span>&nbsp;");
                                    sbPatientDetail.Append(sDoctorTitle);
                                    sbPatientDetail.Append("&nbsp;<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]));
                                    sbPatientDetail.Append("</span>&nbsp;");
                                    sbPatientDetail.Append(DateTitle);
                                    sbPatientDetail.Append("<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(Session["EncounterDate"]));
                                    sbPatientDetail.Append("</span>&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]));
                                    sbPatientDetail.Append("</span>&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]));
                                    sbPatientDetail.Append("</span>&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]));
                                    sbPatientDetail.Append("</span>&nbsp;Company:<span style='color: #990066;font-weight: bold;'>");
                                    sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]));
                                    sbPatientDetail.Append("</span></b>");
                                    Session["PatientDetailString"] = sbPatientDetail.ToString();
                                    sRegNoTitle = string.Empty;
                                    sDoctorTitle = string.Empty;
                                    DateTitle = string.Empty;
                                }
                            }
                        }
                        #endregion
                        ViewState["SelectedEncounterId"] = "";
                        dsModule = (DataSet)Session["ModuleData"];
                        if (dsModule != null)
                        {
                            if (dsModule.Tables.Count > 0)
                            {
                                if (!common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                                {
                                    dvMod = dsModule.Tables[0].Copy().DefaultView;
                                    dvMod.RowFilter = "ModuleName='EMR'";
                                    if (dvMod.ToTable().Rows.Count > 0)
                                    {
                                        Session["ModuleId"] = common.myInt(dvMod.ToTable().Rows[0]["ModuleId"]).ToString();
                                        Session["ModuleName"] = common.myStr(dvMod.ToTable().Rows[0]["ModuleName"]);
                                    }
                                    dvMod.RowFilter = "";
                                    if (common.myStr(Session["EmployeeType"]).ToUpper().Trim().Equals("N"))
                                    {
                                        dvMod.RowFilter = "ModuleName='Nurse Workbench'";
                                        if (dvMod.ToTable().Rows.Count > 0)
                                        {
                                            Session["ModuleId"] = common.myInt(dvMod.ToTable().Rows[0]["ModuleId"]).ToString();
                                            Session["ModuleName"] = common.myStr(dvMod.ToTable().Rows[0]["ModuleName"]);
                                        }
                                        dvMod.RowFilter = "";
                                    }
                                }
                            }
                        }
                        Session["formId"] = "1";
                        if (Session["PreviousRegistrationID"] != null)
                        {
                            objSecurity.DeleteFiles(Server.MapPath("/PatientDocuments/") + common.myInt(Session["HospitalLocationID"]) + "/" + common.myInt(Session["PreviousRegistrationID"]));
                        }
                        if (common.myStr(Session["EmployeeType"]).Trim().Equals("N"))
                        {
                            Response.Redirect("~/emr/Vitals/Vitals.aspx?Mpg=P14", false);
                        }
                        else
                        {
                            //dt = objEMR.GetDefaultPageUserSpecific(0, string.Empty, common.myInt(Session["UserId"]), common.myStr(Session["OPIP"]));
                            //getURL = objEMR.GetDefaultPageByUser(common.myInt(Session["GroupId"]), "/EMR/Dashboard/PatientDashboardForDoctor.aspx");

                            Session["BindPatientLists"] = false;
                            Session["FromPatientLists"] = true;

                            if (common.myLen(strPageUrl).Equals(0))
                            {
                                strPageUrl = "/EMR/Dashboard/PatientDashboardForDoctor.aspx";
                            }

                            Response.Redirect("~" + strPageUrl, false);
                        }
                    }
                }
                Session["FindPatientSelected"] = "0";
            }
            else if (e.CommandName.ToUpper().Equals("CRITICALVIEW"))
            {
                RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + sEncounterId +
                                        "&RegNo=" + sRegistrationNo + "&PageSource=Ward";
                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 20;
                RadWindow1.Left = 20;
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            else if (e.CommandName.ToUpper().Equals("ADDBREAKBLOCK"))
            {
                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                Label lblAppointmentDate = (Label)e.Item.FindControl("lblAppointmentDate");
                HiddenField hdnAppointmentSlot = (HiddenField)e.Item.FindControl("hdnAppointmentSlot");
                DateTime dtpDateTime = common.myDate(lblAppointmentDate.Text);
                DateTime EndDateTime = dtpDateTime.AddMinutes(common.myDbl(hdnAppointmentSlot.Value));
                //DateTime dtpDate = common.myDate(lblAppointmentDate.Text.Split(' ')[0]);
                string StartHour = dtpDateTime.Hour.ToString();
                string StartMinute = dtpDateTime.Minute.ToString();
                string EndHour = EndDateTime.Hour.ToString();
                string EndMinutes = EndDateTime.Minute.ToString();
                string sBreakId = appoint.ExistBreakAndBlock(0, Convert.ToInt16(Session["Facility"]), Convert.ToInt32(ddlProvider.SelectedValue), Convert.ToDateTime(dtpDateTime).ToString("yyyy/MM/dd"), (StartHour.ToString().Length == 1 ? "0" + StartHour.ToString() : StartHour.ToString()) + ":" + (StartMinute.ToString().Length == 1 ? "0" + StartMinute.ToString() : StartMinute.ToString()),
                                                          (EndHour.ToString().Length == 1 ? "0" + EndHour.ToString() : EndHour.ToString()) + ":" + (EndMinutes.ToString().Length == 1 ? "0" + EndMinutes.ToString() : EndMinutes.ToString()));
                if (sBreakId != "" && sBreakId != null)
                {
                    Alert.ShowAjaxMsg("Already set Break and Block in this time.", Page);
                    return;
                }
                Boolean IsDoctor = false;
                if (Convert.ToInt32(ddlProvider.SelectedValue) > 0)
                {
                    IsDoctor = true;
                }
                ViewState["TimeTaken"] = hdnAppointmentSlot.Value.ToString();
                RadWindow1.NavigateUrl = "~/Appointment/BreakAndBlock_NewV1.aspx?MPG=P4&Category=PopUp&FacilityId=" + Session["Facility"] + "&StTime=" + StartHour.ToString() + "&EndTime=" + EndHour.ToString() + "&appDate=" + Convert.ToDateTime(dtpDateTime).ToString("dd/MM/yyyy") + "&appid=0&DoctorId=" + ddlProvider.SelectedValue + "&TimeInterval=" + hdnAppointmentSlot.Value.ToString() + "&FromTimeHour=" + StartHour.ToString() + "&FromTimeMin=" + StartMinute.ToString() + "&PageId=" + Request.QueryString["Mpg"] + "&recrule=" + null + "&IsDoctor=" + IsDoctor + "&TimeTaken=" + ViewState["TimeTaken"] + "&StartTime=" + ViewState["StartTime"] + "&EndTime=" + ViewState["EndTime"];
                RadWindow1.Height = 630;
                RadWindow1.Width = 730;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = "OnClientClose";
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow1.VisibleStatusbar = false;
                appoint = null;
                StartHour = string.Empty;
                StartMinute = string.Empty;
                EndHour = string.Empty;
                EndMinutes = string.Empty;
            }
            else if (e.CommandName.ToUpper().Equals("ADDENDUM"))
            {
                #region PatientDetailStringOP
                Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                Session["PatientDetailString"] = null;
                dsPatientDetail = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                    common.myStr(sRegistrationNo), common.myInt(sRegistrationId),
                                    common.myStr(sEncounterNo), common.myInt(sEncounterId));

                Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dsPatientDetail;

                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {
                        string sRegNoTitle = Resources.PRegistration.regno;
                        string sDoctorTitle = Resources.PRegistration.Doctor;
                        string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                        sbPatientDetail = new StringBuilder();
                        sbPatientDetail.Append("<b><span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]));
                        sbPatientDetail.Append(", ");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["AgeGender"]));
                        sbPatientDetail.Append("</span>&nbsp;");
                        sbPatientDetail.Append(sRegNoTitle);
                        sbPatientDetail.Append("&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]));
                        sbPatientDetail.Append("</span>&nbsp;Enc #:&nbsp;");
                        sbPatientDetail.Append("<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]));
                        sbPatientDetail.Append("</span>&nbsp;");
                        sbPatientDetail.Append(sDoctorTitle);
                        sbPatientDetail.Append("&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]));
                        sbPatientDetail.Append("</span>&nbsp;");
                        sbPatientDetail.Append(DateTitle);
                        sbPatientDetail.Append("&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(Session["EncounterDate"]));
                        sbPatientDetail.Append("</span>&nbsp;Bed:&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]));
                        sbPatientDetail.Append("</span>&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]));
                        sbPatientDetail.Append("</span>&nbsp;Mobile:&nbsp;<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]));
                        sbPatientDetail.Append("</span>&nbsp;Company:<span style='color: #990066;font-weight: bold;'>");
                        sbPatientDetail.Append(common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]));
                        sbPatientDetail.Append("</span></b>");
                        Session["PatientDetailString"] = sbPatientDetail.ToString();
                        sRegNoTitle = string.Empty;
                        sDoctorTitle = string.Empty;
                        DateTitle = string.Empty;
                    }
                }
                #endregion
                if (sRegistrationId != "")
                {
                    RadWindow1.NavigateUrl = "~/EMR/Templates/Default.aspx?MASTER=NO&RegistrationId=" + sRegistrationId +
                                                  "&EncounterId=" + sEncounterId + "&DoctorId=" + sDoctorId + "&IsAddendum=1";
                    RadWindow1.Height = 590;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Patient!", Page);
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("ALLERGYALERT"))
            {
                Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                            common.myStr(sRegistrationNo), common.myInt(sRegistrationId), common.myStr(sEncounterNo), common.myInt(sEncounterId));

                RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&FromEMR=1&EId=" + sEncounterId
                    + "&PId=" + sRegistrationId + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + sRegistrationNo
                    + "&PAG=" + common.myStr(Session["AgeGender"]) + "&EncNo=" + sEncounterNo + "&SepPat=Y";
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
            else if (e.CommandName.ToUpper().Equals("MEDICALALERT"))
            {
                if (common.myInt(Session["EncounterId"]) != common.myInt(sEncounterId))
                {
                    Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                        common.myStr(sRegistrationNo), common.myInt(sRegistrationId), common.myStr(sEncounterNo), common.myInt(sEncounterId));
                }
                RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + sEncounterId +
                                        "&PId=" + sRegistrationId +
                                        "&PN=" + common.myStr(Session["PatientName"]) +
                                        "&PNo=" + sRegistrationNo +
                                        "&PAG=" + common.myStr(Session["AgeGender"]) +
                                        "&EncNo=" + sEncounterNo +
                                        "&SepPat=Y&FromEMR=1";
                RadWindow1.Height = 400;
                RadWindow1.Width = 800;
                RadWindow1.Top = 20;
                RadWindow1.Left = 20;
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                // RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            else if (e.CommandName.ToUpper().Equals("CRITICALALERT"))
            {
                RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + sEncounterId +
                                        "&RegNo=" + sRegistrationNo + "&PageSource=Ward";
                RadWindow1.Height = 400;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 20;
                RadWindow1.Left = 20;
                //RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            else if (e.CommandName.ToUpper().Equals("SHOWLABRESULT"))
            {
                if (common.myInt(sRegistrationNo) > 0 && !string.IsNullOrEmpty(sEncounterDate))
                {
                    RadWindow2.NavigateUrl = "~/EMR/Dashboard/ProviderParts/LabResults.aspx?From=POPUP&RegNo=" + common.myInt(sRegistrationNo).ToString() + "&EncounterDate=" + sEncounterDate + "&FindPatientDoctorId=" + sDoctorId + "&OPIP=" + common.myStr(hdnOPIP.Value).Substring(0, 1);
                    RadWindow2.Height = 600;
                    RadWindow2.Width = 1200;
                    RadWindow2.Top = 40;
                    RadWindow2.Left = 100;
                    RadWindow2.OnClientClose = ""; //"OnClientClose";
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.ReloadOnShow = true;
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            else if (e.CommandName.ToUpper().Equals("SHOWREFERRAL"))
            {
                if (!string.IsNullOrEmpty(sRegistrationNo))
                {
                    Session["TopPanelMedicalAllergyAlert" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                        common.myStr(sRegistrationNo), common.myInt(sRegistrationId), common.myStr(sEncounterNo), common.myInt(sEncounterId));

                    RadWindow2.NavigateUrl = "~/EMR/ReferralSlipHistory.aspx?Mpg=P1081&AlertType=R&MASTER=NO&EncId=" + sEncounterId +
                                            "&RegId=" + sRegistrationId + "&RegNo=" + sRegistrationNo + "&BindDetail=Y" +
                                            "&EncNo=" + sEncounterNo + "&PNo=" + sRegistrationNo;
                    RadWindow2.Height = 600;
                    RadWindow2.Width = 1200;
                    RadWindow2.Top = 40;
                    RadWindow2.Left = 100;
                    RadWindow2.OnClientClose = ""; //"OnClientClose";
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.ReloadOnShow = true;
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            else if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                if (sEncounterId != "" && sRegistrationId != "")
                {
                    RadWindow1.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&RegId=" + sRegistrationId +
                                            "&EncId=" + sEncounterId +
                                            "&EncounterDate=" + sEncounterDate +
                                            "&callby=mrd&OPIP=I";
                    RadWindow1.Height = 600;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 20;
                    RadWindow1.Left = 20;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Patient!", Page);
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DISCHARGESUMMARY"))
            {
                string sStatus = "";
                string DischargeStatusName = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterStatusColor")).Value);
                if (DischargeStatusName == "EXPIRED")
                {
                    sStatus = "DthSum";
                }
                if (sRegistrationId != "")
                {
                    RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + sRegistrationId +
                                                "&RegNo=" + sRegistrationNo +
                                                "&EncId=" + sEncounterId +
                                                "&EncNo=" + sEncounterNo +
                                                "&For=" + sStatus +
                                                "&SummaryStatusName=" + DischargeStatusName;
                    RadWindow1.Height = 590;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow1.VisibleStatusbar = false;
                    sStatus = string.Empty;
                    DischargeStatusName = string.Empty;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Patient!", Page);
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DIAGHISTORY"))
            {
                if (common.myInt(sRegistrationNo) > 0)
                {
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]),
                            common.myInt(Session["FacilityId"]), common.myStr(sRegistrationNo), common.myInt(sRegistrationId),
                            common.myStr(sEncounterNo), common.myInt(sEncounterId));

                    RadWindow2.NavigateUrl = "~/EMR/PatientHistory.aspx?CF=&Master=Blank&Category=PopUp&EncId=" + sEncounterId + "&RegNo=" + sRegistrationNo + //"&Flag=LIS" +
                                            "&Station=All&CloseButtonShow=Yes&FromEMR=1&Source=" + common.myStr(hdnOPIP.Value).Substring(0, 1);
                    RadWindow2.Height = 600;
                    RadWindow2.Width = 1200;
                    RadWindow2.Top = 40;
                    RadWindow2.Left = 100;
                    RadWindow2.OnClientClose = ""; //"OnClientClose";
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.ReloadOnShow = true;
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
            else if (e.CommandName.ToUpper().Equals("PASTCLINICALNOTE"))
            {
                if (common.myInt(sRegistrationNo) > 0)
                {
                    //---Added on 07122020----
                    if (common.myStr(sRegistrationNo) != "")
                    {
                        Session["RegistrationNoReg"] = common.myStr(sRegistrationNo);
                    }
                    if (common.myStr(sEncounterNo) != "")
                    {
                        Session["EncounterNo"] = common.myStr(sEncounterNo);
                    }
                    //---Added on 07122020----

                    RadWindow2.NavigateUrl = "~/WardManagement/VisitHistory.aspx?Regid=" + sRegistrationId + "&RegNo=" + sRegistrationNo + "&EncId=" + sEncounterId + "&EncNo=" + sEncounterNo + "&Category=PopUp&CloseButtonShow=Yes&FromEMR=1&ManualRegNoFilter=1&OP_IP=" + common.myStr(hdnOPIP.Value).Substring(0, 1); RadWindow2.Height = 600;
                    RadWindow2.Width = 1200;
                    RadWindow2.Top = 40;
                    RadWindow2.Left = 100;
                    RadWindow2.OnClientClose = ""; //"OnClientClose";
                    RadWindow2.Modal = true;
                    RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.ReloadOnShow = true;
                    RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sbPatientDetail = null;
            dsPatientDetail.Dispose();
            if (dsModule != null)
            {
                dsModule.Dispose();
            }
            if (dsAttributes != null)
            {
                dsAttributes.Dispose();
            }
            dsDefaultTemplate.Dispose();
            dvMod.Dispose();
            bC = null;
            objSecurity = null;
            objEMRMaster = null;
            objEMR = null;
            //if (!e.CommandName.ToUpper().Equals("SELECTENCOUNTER") && !e.CommandName.ToUpper().Equals("PAGE"))
            //{
            //    bindStatus(rblSearchCriteria.SelectedValue);
            //}
            htIn = null;
            objDl = null;
            ds.Dispose();
            sbSQL = null;
            dt.Dispose();
        }
    }
    #region old method
    //private void SetFindPatientVisitsColumnsVisibilitySetup()
    //{
    //    DataSet dsCol = new DataSet();
    //    clsIVF objIVF = new clsIVF(sConString);
    //    DataView DV = new DataView();
    //    try
    //    {
    //        gvEncounter.Columns[(byte)enumEncounter.Select].Visible = true;
    //        gvEncounter.Columns[(byte)enumEncounter.Select].ItemStyle.Width = Unit.Pixel(50);
    //        gvEncounter.Columns[(byte)enumEncounter.Select].HeaderStyle.Width = Unit.Pixel(50);

    //        gvEncounter.Columns[(byte)enumEncounter.PatientName].Visible = true;

    //        gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = true;
    //        gvEncounter.Columns[(byte)enumEncounter.VisitType].ItemStyle.Width = Unit.Pixel(50);
    //        gvEncounter.Columns[(byte)enumEncounter.VisitType].HeaderStyle.Width = Unit.Pixel(50);

    //        gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = true;
    //        gvEncounter.Columns[(byte)enumEncounter.Notification].ItemStyle.Width = Unit.Pixel(120);
    //        gvEncounter.Columns[(byte)enumEncounter.Notification].HeaderStyle.Width = Unit.Pixel(120);

    //        gvEncounter.Columns[(byte)enumEncounter.AgeGender].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.MobileNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.EncounterDate].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.PatientVisit].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.DoctorName].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.Status].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.Addendum].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.Company].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.CPR].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.TokenNo].Visible = false;
    //        gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = false;

    //        btnCloseQMS.Visible = false;

    //        if (common.myInt(Session["isEMR"]).Equals(1))
    //        {
    //            gvEncounter.Columns[(byte)enumEncounter.Select].Visible = true;
    //            gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = true;
    //            gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = true;
    //        }
    //        else
    //        {
    //            gvEncounter.Columns[(byte)enumEncounter.Select].Visible = false;
    //            gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = false;
    //            gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = false;
    //        }

    //        if (common.myStr(Session["EMRFindPatientVisitsColumnsVisibilitySetup"]).Equals(string.Empty))
    //        {
    //            dsCol = objIVF.getEMRGridColumnsVisibilitySetup(common.myInt(Session["FacilityId"]), "FINDPATIENTVISITS", common.myInt(Session["ModuleId"]));
    //            Session["EMRFindPatientVisitsColumnsVisibilitySetup"] = dsCol;
    //        }

    //        dsCol = (DataSet)Session["EMRFindPatientVisitsColumnsVisibilitySetup"];

    //        if (dsCol.Tables.Count > 0)
    //        {
    //            if (dsCol.Tables[0].Rows.Count > 0)
    //            {
    //                gvEncounter.Columns[(byte)enumEncounter.PatientName].Visible = true;

    //                for (int rowIdx = 0; rowIdx < dsCol.Tables[0].Rows.Count; rowIdx++)
    //                {
    //                    //case "O":
    //                    //case "L":
    //                    //gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = false;
    //                    //gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = false;
    //                    //break;
    //                    //case "I":
    //                    //case "E":
    //                    //gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = false;
    //                    //gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = false;
    //                    //break;

    //                    DataRow DR = dsCol.Tables[0].Rows[rowIdx];
    //                    switch (common.myStr(DR["ColumnCode"]))
    //                    {


    //                        case "PTN"://Patient Name
    //                            gvEncounter.Columns[(byte)enumEncounter.PatientName].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.PatientName].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "AGN": //AgeGender

    //                            gvEncounter.Columns[(byte)enumEncounter.AgeGender].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.AgeGender].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.AgeGender].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "MOB"://MobileNo
    //                            gvEncounter.Columns[(byte)enumEncounter.MobileNo].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.MobileNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.MobileNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "REG"://RegistrationNo
    //                            gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "ENC"://EncounterNo
    //                            if (common.myStr(drpVisitType.SelectedValue) == ("I") || common.myStr(drpVisitType.SelectedValue) == ("A"))
    //                            {
    //                                gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = true;
    //                                gvEncounter.Columns[(byte)enumEncounter.EncounterNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                                gvEncounter.Columns[(byte)enumEncounter.EncounterNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));

    //                            }
    //                            break;
    //                        case "EDT"://EncounterDate
    //                            gvEncounter.Columns[(byte)enumEncounter.EncounterDate].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.EncounterDate].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.EncounterDate].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "WBN"://WardNameBedNo
    //                            if (common.myStr(drpVisitType.SelectedValue) == ("I") || common.myStr(drpVisitType.SelectedValue) == ("A"))
    //                            {
    //                                gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = true;
    //                                gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                                gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            }
    //                            break;
    //                        case "DRN"://DoctorName
    //                            gvEncounter.Columns[(byte)enumEncounter.DoctorName].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.DoctorName].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.DoctorName].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "STA"://Status
    //                            gvEncounter.Columns[(byte)enumEncounter.Status].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.Status].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.Status].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "ADM"://Addendum
    //                            gvEncounter.Columns[(byte)enumEncounter.Addendum].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.Addendum].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.Addendum].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "COM"://Company
    //                            gvEncounter.Columns[(byte)enumEncounter.Company].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.Company].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.Company].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "INV"://InvoiceNo
    //                            gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "CPR"://CPR
    //                            gvEncounter.Columns[(byte)enumEncounter.CPR].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.CPR].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.CPR].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "APD"://AppointmentDate
    //                            if (common.myStr(drpVisitType.SelectedValue) == ("O") || common.myStr(drpVisitType.SelectedValue) == ("A"))
    //                            {
    //                                gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = true;
    //                                gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                                gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));

    //                            }
    //                            break;
    //                        case "CCA"://ConsultationCharges
    //                            gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "PVT"://Patient Visit
    //                            gvEncounter.Columns[(byte)enumEncounter.PatientVisit].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.PatientVisit].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.PatientVisit].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;
    //                        case "CALL"://Patient Visit

    //                            gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = true;
    //                            gvEncounter.Columns[(byte)enumEncounter.CALL].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            gvEncounter.Columns[(byte)enumEncounter.CALL].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                            break;

    //                        case "TKN"://Token No
    //                            if (common.myStr(drpVisitType.SelectedValue) == ("O") || common.myStr(drpVisitType.SelectedValue) == ("A"))
    //                            {
    //                                gvEncounter.Columns[(byte)enumEncounter.TokenNo].Visible = true;
    //                                gvEncounter.Columns[(byte)enumEncounter.TokenNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                                gvEncounter.Columns[(byte)enumEncounter.TokenNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
    //                                if (!common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
    //                                {
    //                                    btnCloseQMS.Visible = true;
    //                                }
    //                                btnCloseQMS.Visible = true;
    //                            }
    //                            break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //    finally
    //    {
    //        objIVF = null;
    //        DV.Dispose();
    //        dsCol.Dispose();
    //    }
    //}
    #endregion
    private void SetFindPatientVisitsColumnsVisibilitySetup()
    {
        DataSet dsCol = new DataSet();
        clsIVF objIVF = new clsIVF(sConString);
        DataView DV = new DataView();
        try
        {
            gvEncounter.Columns[(byte)enumEncounter.Select].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.Select].ItemStyle.Width = Unit.Pixel(50);
            gvEncounter.Columns[(byte)enumEncounter.Select].HeaderStyle.Width = Unit.Pixel(50);

            gvEncounter.Columns[(byte)enumEncounter.PatientName].Visible = true;

            gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.VisitType].ItemStyle.Width = Unit.Pixel(50);
            gvEncounter.Columns[(byte)enumEncounter.VisitType].HeaderStyle.Width = Unit.Pixel(50);

            gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.Notification].ItemStyle.Width = Unit.Pixel(120);
            gvEncounter.Columns[(byte)enumEncounter.Notification].HeaderStyle.Width = Unit.Pixel(120);

            gvEncounter.Columns[(byte)enumEncounter.AgeGender].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.MobileNo].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.EncounterDate].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.PatientVisit].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.DoctorName].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.Status].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.Addendum].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.Company].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.CPR].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = true;
            gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.TokenNo].Visible = false;
            gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = false;

            btnCloseQMS.Visible = false;

            if (common.myInt(Session["isEMR"]).Equals(1))
            {
                gvEncounter.Columns[(byte)enumEncounter.Select].Visible = true;
                gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = true;
                gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = true;
            }
            else
            {
                gvEncounter.Columns[(byte)enumEncounter.Select].Visible = false;
                gvEncounter.Columns[(byte)enumEncounter.VisitType].Visible = false;
                gvEncounter.Columns[(byte)enumEncounter.Notification].Visible = false;
            }

            if (common.myStr(Session["EMRFindPatientVisitsColumnsVisibilitySetup"]).Equals(string.Empty))
            {
                dsCol = objIVF.getEMRGridColumnsVisibilitySetup(common.myInt(Session["FacilityId"]), "FINDPATIENTVISITS", common.myInt(Session["ModuleId"]));
                Session["EMRFindPatientVisitsColumnsVisibilitySetup"] = dsCol;
            }

            dsCol = (DataSet)Session["EMRFindPatientVisitsColumnsVisibilitySetup"];

            if (dsCol.Tables.Count > 0)
            {
                if (dsCol.Tables[0].Rows.Count > 0)
                {
                    gvEncounter.Columns[(byte)enumEncounter.PatientName].Visible = true;

                    for (int rowIdx = 0; rowIdx < dsCol.Tables[0].Rows.Count; rowIdx++)
                    {
                        switch (common.myStr(drpVisitType.SelectedValue))
                        {
                            case "O":
                            case "L":
                                gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = false;
                                gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = false;
                                break;
                            case "I":
                            case "E":
                                gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = false;
                                //gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = false;
                                break;
                        }

                        DataRow DR = dsCol.Tables[0].Rows[rowIdx];
                        switch (common.myStr(DR["ColumnCode"]))
                        {


                            case "PTN"://Patient Name
                                gvEncounter.Columns[(byte)enumEncounter.PatientName].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.PatientName].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "AGN": //AgeGender

                                gvEncounter.Columns[(byte)enumEncounter.AgeGender].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.AgeGender].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.AgeGender].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "MOB"://MobileNo
                                gvEncounter.Columns[(byte)enumEncounter.MobileNo].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.MobileNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.MobileNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "REG"://RegistrationNo
                                gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.RegistrationNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "ENC"://EncounterNo
                                if (common.myStr(drpVisitType.SelectedValue) == ("I") || common.myStr(drpVisitType.SelectedValue) == ("A"))
                                {
                                    //gvEncounter.Columns[(byte)enumEncounter.EncounterNo].Visible = true;
                                    gvEncounter.Columns[(byte)enumEncounter.EncounterNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    gvEncounter.Columns[(byte)enumEncounter.EncounterNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));

                                }
                                break;
                            case "EDT"://EncounterDate
                                gvEncounter.Columns[(byte)enumEncounter.EncounterDate].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.EncounterDate].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.EncounterDate].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "WBN"://WardNameBedNo
                                if (common.myStr(drpVisitType.SelectedValue) == ("I") || common.myStr(drpVisitType.SelectedValue) == ("A"))
                                {
                                    //gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].Visible = true;
                                    gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    gvEncounter.Columns[(byte)enumEncounter.WardNameBedNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                }
                                break;
                            case "DRN"://DoctorName
                                gvEncounter.Columns[(byte)enumEncounter.DoctorName].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.DoctorName].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.DoctorName].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "STA"://Status
                                gvEncounter.Columns[(byte)enumEncounter.Status].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.Status].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.Status].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "ADM"://Addendum
                                gvEncounter.Columns[(byte)enumEncounter.Addendum].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.Addendum].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.Addendum].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "COM"://Company
                                gvEncounter.Columns[(byte)enumEncounter.Company].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.Company].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.Company].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "INV"://InvoiceNo
                                gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.InvoiceNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "CPR"://CPR
                                gvEncounter.Columns[(byte)enumEncounter.CPR].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.CPR].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.CPR].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "APD"://AppointmentDate
                                if (common.myStr(drpVisitType.SelectedValue) == ("O") || common.myStr(drpVisitType.SelectedValue) == ("A"))
                                {
                                    //gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].Visible = true;
                                    gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    gvEncounter.Columns[(byte)enumEncounter.AppointmentDate].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));

                                }
                                break;
                            case "CCA"://ConsultationCharges
                                gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.ConsultationCharges].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "PVT"://Patient Visit
                                gvEncounter.Columns[(byte)enumEncounter.PatientVisit].Visible = true;
                                gvEncounter.Columns[(byte)enumEncounter.PatientVisit].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                gvEncounter.Columns[(byte)enumEncounter.PatientVisit].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                break;
                            case "CALL"://Patient Visit
                                if (common.myStr(drpVisitType.SelectedValue) == ("O") || common.myStr(drpVisitType.SelectedValue) == ("A"))
                                {
                                    gvEncounter.Columns[(byte)enumEncounter.CALL].Visible = true;
                                    gvEncounter.Columns[(byte)enumEncounter.CALL].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    gvEncounter.Columns[(byte)enumEncounter.CALL].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                }
                                break;

                            case "TKN"://Token No
                                if (common.myStr(drpVisitType.SelectedValue) == ("O") || common.myStr(drpVisitType.SelectedValue) == ("A"))
                                {
                                    gvEncounter.Columns[(byte)enumEncounter.TokenNo].Visible = true;
                                    gvEncounter.Columns[(byte)enumEncounter.TokenNo].ItemStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    gvEncounter.Columns[(byte)enumEncounter.TokenNo].HeaderStyle.Width = Unit.Pixel(common.myInt(DR["ColumnWidth"]));
                                    if (!common.myInt(Session["ModuleId"]).Equals(common.myInt(ViewState["EMRTriageModuleId"])))
                                    {
                                        btnCloseQMS.Visible = true;
                                    }
                                    btnCloseQMS.Visible = true;
                                }
                                break;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        finally
        {
            objIVF = null;
            DV.Dispose();
            dsCol.Dispose();
        }
    }


    private string ShowCheckedItems(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;
        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (SelectedStatusid.Equals(string.Empty))
                {
                    SelectedStatusid = common.myStr(item.Value);
                    item.Attributes.ToString();
                }
                else
                {
                    SelectedStatusid = SelectedStatusid + "," + common.myStr(item.Value);
                }
            }
        }
        return SelectedStatusid;
    }
    private string ShowVisitCheckedItems(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;
        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (common.myInt(item.Value) > 0)
                {
                    if (SelectedStatusid.Equals(string.Empty))
                    {
                        SelectedStatusid = common.myStr(item.Value);
                        item.Attributes.ToString();
                    }
                    else
                    {
                        SelectedStatusid = SelectedStatusid + "," + common.myStr(item.Value);
                    }
                }
            }
        }
        return SelectedStatusid;
    }
    private bool CheckUnReviewedStatus(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;
        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (common.myStr(item.Text).ToUpper().Contains("UNREVIEWED") || common.myInt(item.Value).Equals(2878))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void TotalEncounterRecords()
    {
        int iTotalRecords = common.myInt(ViewState["Count"]);
        int iPageSize = common.myInt(gvEncounter.PageSize);
        int iRowCount = common.myInt(gvEncounter.Items.Count);
        lblGridStatus.Text = "No Record Found!";
        if (iTotalRecords > 0)
        {
            if (iRowCount == iPageSize)
            {
                lblGridStatus.Text = "Showing " + ((gvEncounter.CurrentPageIndex *
                gvEncounter.PageSize) + 1) + " - " + ((gvEncounter.CurrentPageIndex + 1) *
                gvEncounter.Items.Count) + " of " + iTotalRecords + " Record(s)";
            }
            else
            {
                lblGridStatus.Text = "Showing " + ((gvEncounter.CurrentPageIndex * iPageSize) + 1)
                    + " - " + iTotalRecords + " of " + iTotalRecords + " Record(s)";
            }
        }
    }
    protected void lnkOrderApproval_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "~/EMRBILLING/Popup/OrderApproval.aspx";
        RadWindow1.Height = 590;
        RadWindow1.Width = 1200;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

    // Added By Akshay Al-Shifa 29-Aug-2022
    protected void lnkCalled_OnClick(object sender, EventArgs e)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        LinkButton lnkName = (LinkButton)sender;
        string sRegistrationId = ((HiddenField)lnkName.FindControl("hdnRegistrationID")).Value.ToString().Trim();
        int sEncounterId = common.myInt(((HiddenField)lnkName.FindControl("hdnEncounterId")).Value.ToString().Trim());
        string DoctorId = ((HiddenField)lnkName.FindControl("hdnDoctorID")).Value.ToString().Trim();
        string RegistrationNo = ((Label)lnkName.FindControl("lblRegistrationNo")).Text.ToString().Trim();
        string hdnEncounterId = ((HiddenField)lnkName.FindControl("hdnEncounterId")).Value.ToString().Trim();
        string lblName = ((Label)lnkName.FindControl("lblName")).Text.ToString().Trim();
        string Tokenno = ((HiddenField)lnkName.FindControl("hdnIsToken")).Value.ToString().Trim();

        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        if (sRegistrationId == "")
        {
            Alert.ShowAjaxMsg("Please select Patient", Page);
            return;
        }
        ViewState["CurrentRegistrationno"] = RegistrationNo.ToString();
        HttpContext htpObj = HttpContext.Current;
        string sDoctorName = common.myStr(Session["EmployeeName"]); //common.myStr(Request.QueryString["DoctorName"]);
        string sDeptName = common.myStr(Session["DepartmentName"]).Replace("&amp", "&");
        try
        {
            objEMR.EMRSaveDisplayCurrentPatient(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterId),
                  common.myInt(DoctorId),
                  System.Web.HttpContext.Current.Request.UserHostAddress.ToString(), "SN", sDoctorName, sDeptName, RegistrationNo, Tokenno, "");
            //
            var LinkButton = (LinkButton)sender;
            GridDataItem gvr = (GridDataItem)LinkButton.NamingContainer;
            int indx = common.myInt(gvr.RowIndex);
            DataSet DsColor = new DataSet();
            DsColor = GetQmsColor("PRC");

            DataSet DsColorCalled = new DataSet();
            DsColorCalled = GetQmsColor("CAL");

            foreach (GridDataItem row in gvEncounter.Items)
            {
                if (row.Cells[18].BackColor == System.Drawing.ColorTranslator.FromHtml(DsColor.Tables[0].Rows[0]["StatusColor"].ToString()))
                {
                    if (DsColorCalled.Tables[0].Rows.Count > 0)
                    {
                        row.Cells[18].BackColor = System.Drawing.ColorTranslator.FromHtml(DsColorCalled.Tables[0].Rows[0]["StatusColor"].ToString());
                    }
                }
            }

            if (DsColor.Tables[0].Rows.Count > 0)
            {
                gvr.Cells[18].BackColor = System.Drawing.ColorTranslator.FromHtml(DsColor.Tables[0].Rows[0]["StatusColor"].ToString());
            }
            //
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objemr = null;
        }
        try
        {
            if (sRegistrationId == "")
            {
                Alert.ShowAjaxMsg("Please select Patient", Page);
                return;
            }
        }
        finally
        {
            objemr = null;
        }
    }

    ///sandeep
    public bool EMRSaveDisplayCurrentPatient(int iHospitalLocationId, int iFacilityId, int intEncounterId,
           int intAppointmentResourceId, string chvIPAddress, string sFlag, string sDoctorName, string sDeptName, string sRegistrationNo, string TokenNo, string StatusCode)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsIn = new Hashtable();

        hsIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        hsIn.Add("@intFacilityId", iFacilityId);
        hsIn.Add("@intEncounterId", intEncounterId);
        hsIn.Add("@intAppointmentResourceId", intAppointmentResourceId);
        hsIn.Add("@chvIPAddress", chvIPAddress);
        hsIn.Add("@chvQMSFlag", sFlag);
        hsIn.Add("@chvDoctorName", sDoctorName);
        hsIn.Add("@chvDeptName", sDeptName);
        hsIn.Add("@chvRegistrationNo", sRegistrationNo);
        hsIn.Add("@intSource", 1);
        hsIn.Add("@Tokenno", TokenNo);
        hsIn.Add("@TokenStatus", StatusCode);


        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveDisplayCurrentPatient", hsIn);

        return true;

    }
    protected void btnCloseQMS_Click(object sender, EventArgs e)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        string msg = string.Empty;
        try
        {
            msg = objEMR.EMRCloseQMS(common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
            if (msg.Contains("QMS Closed"))
            {
                Alert.ShowAjaxMsg("QMS Closed!", this.Page);
                bindSearchData(false);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objEMR = null;
        }
    }
    protected void drpVisitType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindSearchData(false);
            SetFindPatientVisitsColumnsVisibilitySetup();
        }
        catch
        {
        }
    }
    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                            common.myInt(Session["GroupId"]), 3, "FindPatient_Break");
            //Session["ModuleId"],Session["GroupId"]
            if (common.myInt(ds.Tables[0].Rows.Count).Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            menuStatus.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                RadMenuItem menu = new RadMenuItem();
                menu.Value = common.myStr(ds.Tables[0].Rows[i]["OptionId"]);
                menu.Text = common.myStr(ds.Tables[0].Rows[i]["OptionName"]);
                menu.Attributes.Add("Code", common.myStr(ds.Tables[0].Rows[i]["OptionCode"]));
                menuStatus.Items.Add(menu);
                menu.Dispose();
            }
            if (ds.Tables[0].Rows.Count <= 20)
            {
                menuStatus.DefaultGroupSettings.Height = Unit.Percentage(100);
            }
            else
            {
                menuStatus.DefaultGroupSettings.Height = Unit.Pixel(470);
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        BaseC.Appointment appoint = new BaseC.Appointment(sConString);
        BaseC.HospitalSetup objHps = new BaseC.HospitalSetup(sConString);
        try
        {
            //Label lblAppointmentDate = (Label)e.Item.FindControl("lblAppointmentDate");
            //HiddenField hdnAppointmentSlot = (HiddenField)e.Item.FindControl("hdnAppointmentSlot");
            if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("Break"))
            {
                string hdnAppointmentDate1 = common.myStr(hdnAppointmentDate.Value);
                string hdnAppointmentSlot1 = common.myStr(hdnTimeSlot.Value);
                int hdnBreakId = common.myInt(hdnRefBreakId.Value);
                DateTime dtpDateTime = common.myDate(hdnAppointmentDate.Value);
                DateTime EndDateTime = dtpDateTime.AddMinutes(common.myDbl(hdnTimeSlot.Value));
                //DateTime dtpDate = common.myDate(lblAppointmentDate.Text.Split(' ')[0]);
                string StartHour = dtpDateTime.Hour.ToString();
                string StartMinute = dtpDateTime.Minute.ToString();
                string EndHour = EndDateTime.Hour.ToString();
                string EndMinutes = EndDateTime.Minute.ToString();
                string sBreakId = appoint.ExistBreakAndBlock(0, Convert.ToInt16(Session["Facility"]), Convert.ToInt32(ddlProvider.SelectedValue), Convert.ToDateTime(dtpDateTime).ToString("yyyy/MM/dd"), (StartHour.ToString().Length == 1 ? "0" + StartHour.ToString() : StartHour.ToString()) + ":" + (StartMinute.ToString().Length == 1 ? "0" + StartMinute.ToString() : StartMinute.ToString()),
                                                          (EndHour.ToString().Length == 1 ? "0" + EndHour.ToString() : EndHour.ToString()) + ":" + (EndMinutes.ToString().Length == 1 ? "0" + EndMinutes.ToString() : EndMinutes.ToString()));
                if (sBreakId != "" && sBreakId != null)
                {
                    Alert.ShowAjaxMsg("Already set Break and Block in this time.", Page);
                    return;
                }
                Boolean IsDoctor = false;
                if (Convert.ToInt32(ddlProvider.SelectedValue) > 0)
                {
                    IsDoctor = true;
                }
                ViewState["TimeTaken"] = hdnTimeSlot.Value.ToString();
                RadWindow1.NavigateUrl = "~/Appointment/BreakAndBlock_NewV1.aspx?MPG=P4&Category=PopUp&FacilityId=" + Session["Facility"] + "&StTime=" + StartHour.ToString() + "&EndTime=" + EndHour.ToString() + "&appDate=" + Convert.ToDateTime(dtpDateTime).ToString("dd/MM/yyyy") + "&appid=0&DoctorId=" + ddlProvider.SelectedValue + "&TimeInterval=" + hdnTimeSlot.Value.ToString() + "&FromTimeHour=" + StartHour.ToString() + "&FromTimeMin=" + StartMinute.ToString() + "&PageId=" + Request.QueryString["Mpg"] + "&recrule=" + null + "&IsDoctor=" + IsDoctor + "&TimeTaken=" + ViewState["TimeTaken"] + "&StartTime=" + ViewState["StartTime"] + "&EndTime=" + ViewState["EndTime"];
                RadWindow1.Height = 630;
                RadWindow1.Width = 730;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.OnClientClose = "OnClientClose";
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow1.VisibleStatusbar = false;
            }
            if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]).Equals("Delete"))
            {
                if (objHps.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCancelAppointment", common.myInt(Session["FacilityId"])).Equals("Y"))
                {
                    divDeleteBreak.Visible = true;
                }
                else
                {
                    Alert.ShowAjaxMsg("You are not authorise persion for cancel appointment", Page);
                    return;
                }
            }
        }
        catch
        {
        }
        finally
        {
            appoint = null;
            objHps = null;
        }
    }
    protected void btnDeleteBreak_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        try
        {
            int hdnBreakId = common.myInt(hdnRefBreakId.Value);
            hshIn.Add("@intBreakId", hdnBreakId);
            hshIn.Add("@intLastChangedBy", Session["UserId"]);
            dl.ExecuteNonQuery(CommandType.Text, "Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=GETUTCDATE() where Id=@intBreakId", hshIn);
            divDeleteBreak.Visible = false;
            btnSearch_Click(sender, e);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dl = null; hshIn = null;
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        divDeleteBreak.Visible = false;
    }

    protected void imgCarePlan_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnk = (ImageButton)sender;
            GridItem gv = (GridItem)lnk.NamingContainer;
            HiddenField hdnEncounterId = (HiddenField)gv.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)gv.FindControl("hdnRegistrationID");

            RadWindow1.NavigateUrl = "~/EMR/ClinicalPathway/PatientTreatmentPlan.aspx?From=POPUP&EncId=" + common.myInt(hdnEncounterId.Value)
                + "&RegId=" + common.myInt(hdnRegistrationId.Value);
            RadWindow1.Height = 300;
            RadWindow1.Width = 500;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            //RadWindow1.OnClientClose = "OnClearClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    protected void lblTokenNo_Click(object sender, EventArgs e)
    {

    }

    // Added By Akshay Al-Shifa 29-Aug-2022
    public DataSet GetQmsColor(string Code)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsIn = new Hashtable();
        hsIn.Add("@Code", Code);
        DataSet DsQms = new DataSet();

        DsQms = objDl.FillDataSet(CommandType.StoredProcedure, "GetQMSStatusColor", hsIn);
        return DsQms;

    }
}