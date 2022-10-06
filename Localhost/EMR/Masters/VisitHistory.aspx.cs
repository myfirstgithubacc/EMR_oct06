using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;
using System.Diagnostics;

public partial class EMR_Masters_VisitHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISMaster objbc;
    BaseC.EMRMasters objbc1;
    DataSet ds;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else if (Request.QueryString["RegId"] != null)
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
                Response.Redirect("/Login.aspx?Logout=1", false);

            SetControls();
            BindPatientHiddenDetails();
            ddlrange_SelectedIndexChanged(sender, e);
            hdnRegId.Value = "";
            btnClose.Visible = false;
            BindTemplates();

            if (common.myStr(Request.QueryString["MP"]) == "NO")
                btnClose.Visible = true;

            if (Request.QueryString["RegId"] != null)
            {
                txtRegNo.ReadOnly = true;
                hdnRegId.Value = Request.QueryString["RegId"].ToString();
                txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                //btnFilter_Click(null, null);
                btnViewAll.Visible = false;
                lnkDownload.Visible = false;
                btnView.Visible = false;
                lbtnSearchPatient.Enabled = false;
                BindProvider();
                BindGrid();
            }
            else if (common.myStr(Request.QueryString["CF"]) == "PTA")
            {
                lbtnSearchPatient.Enabled = false;
                txtRegNo.ReadOnly = true;
                hdnRegId.Value = common.myStr(Request.QueryString["PId"]);
                txtRegNo.Text = common.myStr(Request.QueryString["PId"]);
                btnViewAll.Visible = true;
                lnkDownload.Visible = true;
                btnView.Visible = true;
                BindProvider();
                BindGrid();
            }
            else if (Session["RegistrationId"] != null && Session["EncounterId"] != null)
            {
                lbtnSearchPatient.Enabled = false;
                txtRegNo.ReadOnly = true;
                hdnRegId.Value = common.myStr(Session["RegistrationId"]);
                txtRegNo.Text = common.myStr(Session["RegistrationNo"]);

                btnViewAll.Visible = true;
                lnkDownload.Visible = true;
                btnView.Visible = true;
                BindProvider();
                BindGrid();
            }
            else
            {
                txtRegNo.ReadOnly = false;
                btnViewAll.Visible = true;
                lnkDownload.Visible = true;
                btnView.Visible = true;
                lbtnSearchPatient.Enabled = true;
                BindGrid();
            }
        }
    }
    private void EnableControls()
    {

    }
    void BindProvider()
    {
        try
        {
            DataTable dt = new DataTable();

            DataSet ds = new DataSet();
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            ds = objEmr.GetEMRDoctorPatientwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRegId.Value));
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ddlProvider.DataSource = dt;
                    ddlProvider.DataTextField = "DoctorName";
                    ddlProvider.DataValueField = "DoctorId";
                    ddlProvider.DataBind();
                    ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
                    ddlProvider.SelectedIndex = -1;
                    //ddlProvider.SelectedValue = common.myStr(Session["DoctorID"]);

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
    protected void ddlProvider_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    DataTable BindBlankGrid(int i)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("OPIP");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("EncounterDate");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("Name");
        dt.Columns.Add("FacilityName");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("ProblemDescription");
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("IsResource");
        dt.Columns.Add("SeenByDoctor");
        dt.Columns.Add("SeenByDoctorId");
        dt.Columns.Add("OTCount");
        dt.Columns.Add("DoctorId");
        dt.Columns.Add("HealthCheckup");
        DataRow dr = dt.NewRow();

        for (int j = 0; j <= i; j++)
        {
            dr["OPIP"] = DBNull.Value;
            dr["EncounterId"] = DBNull.Value;
            dr["EncounterDate"] = DBNull.Value;
            dr["EncounterNo"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["FacilityName"] = DBNull.Value;
            dr["DoctorName"] = DBNull.Value;
            dr["ProblemDescription"] = DBNull.Value;
            dr["RegistrationId"] = DBNull.Value;
            dr["OTCount"] = DBNull.Value;
            dr["DoctorId"] = DBNull.Value;
            dr["HealthCheckup"] = DBNull.Value;
        }
        dt.Rows.Add(dr);
        return dt;
    }
    private void BindTemplates()
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = master.GetAllTypeTemplates(Convert.ToInt16(Session["HospitalLocationId"]), "S");
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "SectionId<>787";
        DataTable dt = dv.ToTable();

        DataRow dr = dt.Rows.Add();
        dr["SectionId"] = 0;
        dr["SectionName"] = "Clinical Examination";

        dr["ParentId"] = 0;
        dr["Type"] = "D";
        dr["TemplateName"] = "";
        dr["TemplateTypeName"] = "Dynamic";
        //dt.Rows.Add(dr);

        if (dt.Rows.Count > 0)
        {
            ddlTemplates.DataSource = dt;
            ddlTemplates.DataTextField = "SectionName";
            ddlTemplates.DataValueField = "SectionId";
            ddlTemplates.DataBind();

            //ddlTemplates.Items.Insert(6, new Telerik.Web.UI.RadComboBoxItem("Clinical Examination"));
            //ddlTemplates.Items[0].Value = "0";
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindGrid()
    {
        objbc1 = new BaseC.EMRMasters(sConString);
        DataSet dsTemp = new DataSet();
        ds = new DataSet();
        DataSet dsDepartment = new DataSet();
        string sType = "";
        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataRow dr;
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        try
        {
            if (common.myStr(txtRegNo.Text) != "" || hdnRegId.Value != "")
            {
                if (ddlrange.SelectedValue == "4")
                {
                    dsTemp = objbc1.GetPatientHistoryDetails(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(ddlProvider.SelectedValue), common.myInt(hdnRegId.Value), common.myStr(ddlrange.SelectedValue), dtpfrmdate.SelectedDate.Value.ToString("yyyy/MM/dd"), dtpTodate.SelectedDate.Value.ToString("yyyy/MM/dd"), common.myStr(ddlSource.SelectedValue));
                }
                else
                {
                    dsTemp = objbc1.GetPatientHistoryDetails(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(ddlProvider.SelectedValue), common.myInt(hdnRegId.Value), common.myStr(ddlrange.SelectedValue), "", "", common.myStr(ddlSource.SelectedValue));
                }

                dsDepartment = objPhr.getDepartmentMain(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                if (Session["OPIP"] != null && Session["OPIP"] != "I")
                {
                    if (Session["LoginEmployeeType"] != null && (Session["LoginEmployeeType"].ToString() != "N" && Session["LoginEmployeeType"].ToString() != "SN"))
                    {
                        dv = new DataView(dsDepartment.Tables[0]);
                        dv.RowFilter = "ISNULL(DepartmentId,0)=" + Convert.ToInt32(Session["LoginDepartmentId"]);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            sType = dv.ToTable().Rows[0]["DepartmentIdendification"].ToString();
                        }
                        dv1 = new DataView(dsTemp.Tables[0]);
                        if (sType != "AN")
                        {
                            dv1.RowFilter = "ISNULL(DepartmentIdendification,'')<>'AN'";
                        }
                        ds.Tables.Add(dv1.ToTable());
                    }
                    else
                    {
                        ds = dsTemp;
                    }
                }
                else
                {
                    ds = dsTemp;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                    Session["IsMedicalAlert"] = dr["isMedicalAlert"].ToString();
                    if (common.myStr(Session["IsMedicalAlert"]) == "")
                    {
                        lnkAlerts.Enabled = false;
                        lnkAlerts.CssClass = "blinkNone";
                        lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
                    }
                    else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
                    {
                        lnkAlerts.Enabled = true;
                        lnkAlerts.Font.Bold = true;
                        lnkAlerts.CssClass = "blink";
                        lnkAlerts.Font.Size = 11;
                        lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
                    }

                    gvPatientHistory.DataSource = ds;
                    gvPatientHistory.DataBind();
                }
                else
                {
                    gvPatientHistory.DataSource = BindBlankGrid(1);
                    gvPatientHistory.DataBind();
                }
            }
            else
            {
                gvPatientHistory.DataSource = BindBlankGrid(1);
                gvPatientHistory.DataBind();
            }
            if (gvPatientHistory.Columns.Count > 0)
            {
                if (Request.QueryString["RegId"] != null)
                {
                    gvPatientHistory.Columns[0].Visible = false;
                }
                else
                {
                    gvPatientHistory.Columns[0].Visible = true;
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
            dsTemp.Dispose();
            ds.Dispose();
            dsDepartment.Dispose();
            dv.Dispose();
            dv1.Dispose();
            sType = "";
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        ddlrange_SelectedIndexChanged(sender, e);
        BindGrid();

    }
    private void SetControls()
    {
        ddlrange.Items.Clear();
        RadComboBoxItem ls = new RadComboBoxItem();
        ls.Text = "Select All";
        ls.Value = "";
        ls.Selected = true;
        ddlrange.Items.Add(ls);

        RadComboBoxItem lst3 = new RadComboBoxItem();
        lst3.Text = "Today";
        lst3.Value = "DD0";
        ddlrange.Items.Add(lst3);

        RadComboBoxItem lst10 = new RadComboBoxItem();
        lst10.Text = "This Week";
        lst10.Value = "WW0";
        ddlrange.Items.Add(lst10);



        //   ddlrange.Items.Add(lst3);




        RadComboBoxItem lst4 = new RadComboBoxItem();
        lst4.Text = "Last Week";
        lst4.Value = "WW-1";
        ddlrange.Items.Add(lst4);








        RadComboBoxItem lst5 = new RadComboBoxItem();
        lst5.Text = "Last Two Week";
        lst5.Value = "WW-2";
        ddlrange.Items.Add(lst5);

        RadComboBoxItem lst8 = new RadComboBoxItem();
        lst8.Text = "This Month";
        lst8.Value = "MM0";
        ddlrange.Items.Add(lst8);

        RadComboBoxItem lst9 = new RadComboBoxItem();
        lst9.Text = "Last One Month";
        lst9.Value = "MM-1";
        ddlrange.Items.Add(lst9);

        RadComboBoxItem lst6 = new RadComboBoxItem();
        lst6.Text = "Last Year";
        lst6.Value = "YY-1";
        ddlrange.Items.Add(lst6);
        RadComboBoxItem lst7 = new RadComboBoxItem();
        lst7.Text = "Date Range";
        lst7.Value = "4";
        ddlrange.Items.Add(lst7);

        //objbc1 = new BaseC.EMRMasters(sConString);
        //DataSet ds = objbc1.GETDateRangeStaticData();
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlrange.DataSource = ds.Tables[0];
        //    ddlrange.DataTextField = "rangename";
        //    ddlrange.DataValueField = "rangevalue";
        //    ddlrange.DataBind();

        //}

        ////commented below code and written the new function above to for code optimization : Sikandar

        ////ddlrange.Items.Clear();
        ////RadComboBoxItem ls = new RadComboBoxItem();
        ////ls.Text = "Select All";
        ////ls.Value = "";
        ////ls.Selected = true;
        ////ddlrange.Items.Add(ls);

        ////RadComboBoxItem lst3 = new RadComboBoxItem();
        ////lst3.Text = "Today";
        ////lst3.Value = "DD0";
        ////ddlrange.Items.Add(lst3);

        ////RadComboBoxItem lst10 = new RadComboBoxItem();
        ////lst10.Text = "This Week";
        ////lst10.Value = "WW0";
        ////ddlrange.Items.Add(lst10);

        //////   ddlrange.Items.Add(lst3);

        ////RadComboBoxItem lst4 = new RadComboBoxItem();
        ////lst4.Text = "Last Week";
        ////lst4.Value = "WW-1";
        ////ddlrange.Items.Add(lst4);

        ////RadComboBoxItem lst5 = new RadComboBoxItem();
        ////lst5.Text = "Last Two Week";
        ////lst5.Value = "WW-2";
        ////ddlrange.Items.Add(lst5);

        ////RadComboBoxItem lst8 = new RadComboBoxItem();
        ////lst8.Text = "This Month";
        ////lst8.Value = "MM0";
        ////ddlrange.Items.Add(lst8);

        ////RadComboBoxItem lst9 = new RadComboBoxItem();
        ////lst9.Text = "Last One Month";
        ////lst9.Value = "MM-1";
        ////ddlrange.Items.Add(lst9);

        ////RadComboBoxItem lst6 = new RadComboBoxItem();
        ////lst6.Text = "Last Year";
        ////lst6.Value = "YY-1";
        ////ddlrange.Items.Add(lst6);
        ////RadComboBoxItem lst7 = new RadComboBoxItem();
        ////lst7.Text = "Date Range";
        ////lst7.Value = "4";
        ////ddlrange.Items.Add(lst7);

        if (common.myStr(Session["IsMedicalAlert"]) == "")
        {
            lnkAlerts.Enabled = false;
            lnkAlerts.CssClass = "blinkNone";
            lnkAlerts.ForeColor = System.Drawing.Color.FromName("Blue");
        }
        else if (common.myStr(Session["IsMedicalAlert"]).ToUpper() == "YES")
        {
            lnkAlerts.Enabled = true;
            lnkAlerts.Font.Bold = true;
            lnkAlerts.CssClass = "blink";
            lnkAlerts.Font.Size = 11;
            lnkAlerts.ForeColor = System.Drawing.Color.FromName("Red");
        }
    }
    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrange.SelectedValue == "4")
        {
            tblDateRange.Visible = true;

            dtpfrmdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfrmdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

        }
        else
        {
            tblDateRange.Visible = false;

            dtpfrmdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.Text = "";

            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.Text = "";

        }
        //BindColorLegend();
    }

    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        if (ddlTemplates.SelectedValue != "")
        {
            if (common.myStr(common.myStr(Session["RegistrationId"])) != "")
            {
                string sEncounterIds = "";
                int count = 0;
                foreach (GridDataItem item in gvPatientHistory.Items)
                {
                    CheckBox chkEncounter = (CheckBox)item.FindControl("chkEncounter");
                    HiddenField hdnEncounterId = (HiddenField)item.FindControl("hdnEncounterId");
                    if (chkEncounter.Checked == true)
                    {
                        sEncounterIds = count == 0 ? hdnEncounterId.Value : sEncounterIds + "," + hdnEncounterId.Value;
                        count++;
                    }
                }
                if (sEncounterIds == "")
                {
                    Alert.ShowAjaxMsg("Please select Visit", Page);
                    return;
                }
                RadWindow3.NavigateUrl = "/EMR/Masters/ViewPatientHistory.aspx?RegId=" + common.myStr(Session["RegistrationId"]) + "&EncounterId=" + sEncounterIds + "&TemplateId=" + ddlTemplates.SelectedValue + "&ViewAll=All";
                RadWindow3.Height = 600;
                RadWindow3.Width = 900;
                RadWindow3.Top = 20;
                RadWindow3.Left = 20;
                // RadWindowForNew.Title = "Time Slot";
                //RadWindow3.OnClientClose = "OnClientClose";
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow3.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient!";
            }
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select Template", Page);
            return;
        }
    }
    protected void Encounter_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox lbtn = sender as CheckBox;
            GridTableRow row = lbtn.NamingContainer as GridTableRow;
            CheckBox chkAllDep = (CheckBox)row.FindControl("chkAllEncounter");
            ViewState["chkAllDep"] = ((CheckBox)row.FindControl("chkAllEncounter")).Checked;
            if (chkAllDep.Checked == true)
            {

                foreach (GridTableRow rw in gvPatientHistory.Items)
                {
                    ((CheckBox)rw.FindControl("chkEncounter")).Checked = true;
                }
            }
            else
            {
                foreach (GridTableRow rw in gvPatientHistory.Items)
                {
                    ((CheckBox)rw.FindControl("chkEncounter")).Checked = false;
                }
                ViewState["chkAllDep"] = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        String sbIdList = "";
        if (common.myStr(ViewState["chkAllDep"]).ToUpper() == "TRUE")
        {
            sbIdList = "";
        }
        else
        {

            foreach (GridDataItem item in gvPatientHistory.Items)
            {
                if (((CheckBox)item.FindControl("chkEncounter")).Checked == true)
                {
                    if (sbIdList == "")
                        sbIdList = ((HiddenField)item.FindControl("hdnEncounterId")).Value;
                    else
                        sbIdList = sbIdList + "," + ((HiddenField)item.FindControl("hdnEncounterId")).Value;
                }
            }
        }
        ViewState["setformula"] = sbIdList;

        //if (common.myStr(Session["EncounterId"]) != "" && common.myStr(Session["RegistrationId"]) != "")
        //{
        //    if (common.myStr(ddlmoduletype.SelectedValue) == "Pres")
        //    {

        //        RadWindow3.NavigateUrl = "/EMR/Masters/PrescriptionDetails.aspx?RegId=" + common.myStr(Session["RegistrationId"] + "&EncId=" + common.myStr(ViewState["setformula"]) + "&ProviderId=" + common.myStr(ddlProvider.SelectedValue) + "&Daterange=" + ddlrange.SelectedValue + "&FDate=" + dtpfrmdate.SelectedDate + "&TDate" + dtpTodate.SelectedDate + "&Reprtname=Pres");
        //        RadWindow3.Height = 600;
        //        RadWindow3.Width = 900;
        //        RadWindow3.Top = 20;
        //        RadWindow3.Left = 20;
        //        // RadWindowForNew.Title = "Time Slot";
        //        RadWindow3.OnClientClose = "OnClientClose";
        //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //        RadWindow3.Modal = true;
        //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        //        RadWindow3.VisibleStatusbar = false;
        //    }
        //    else if (common.myStr(ddlmoduletype.SelectedValue) == "Med")
        //    {

        //        RadWindow3.NavigateUrl = "/EMR/Masters/PrescriptionDetails.aspx?RegId=" + common.myStr(Session["RegistrationId"] + "&EncId=" + common.myStr(ViewState["setformula"]) + "&ProviderId=" + common.myStr(ddlProvider.SelectedValue) + "&Daterange=" + ddlrange.SelectedValue + "&FDate=" + dtpfrmdate.SelectedDate + "&TDate" + dtpTodate.SelectedDate + "&Reprtname=Med");
        //        RadWindow3.Height = 600;
        //        RadWindow3.Width = 900;
        //        RadWindow3.Top = 20;
        //        RadWindow3.Left = 20;
        //        // RadWindowForNew.Title = "Time Slot";
        //        RadWindow3.OnClientClose = "OnClientClose";
        //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //        RadWindow3.Modal = true;
        //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        //        RadWindow3.VisibleStatusbar = false;
        //    }
        //    else if (common.myStr(ddlmoduletype.SelectedValue) == "VT")
        //    {

        //        RadWindow3.NavigateUrl = "/EMR/Masters/VitalHistory.aspx?RegId=" + common.myStr(Session["RegistrationId"] + "&EncId=" + common.myStr(ViewState["setformula"]) + "&ProviderId=" + common.myStr(ddlProvider.SelectedValue) + "&Daterange=" + ddlrange.SelectedValue + "&FDate=" + dtpfrmdate.SelectedDate + "&TDate" + dtpTodate.SelectedDate + "&Reprtname=Vital");
        //        RadWindow3.Height = 600;
        //        RadWindow3.Width = 900;
        //        RadWindow3.Top = 20;
        //        RadWindow3.Left = 20;
        //        // RadWindowForNew.Title = "Time Slot";
        //        RadWindow3.OnClientClose = "OnClientClose";
        //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //        RadWindow3.Modal = true;
        //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        //        RadWindow3.VisibleStatusbar = false;
        //    }
        //    else if (common.myStr(ddlmoduletype.SelectedValue) == "Inv")
        //    {
        //        RadWindow3.NavigateUrl = "/EMR/Masters/InvestigationResult.aspx?RegId=" + common.myStr(Session["RegistrationId"]) + "&Fdate=" + dtpfrmdate.SelectedDate + "&Tdate=" + dtpfrmdate.SelectedDate + "&Source=" + ddlSource.SelectedValue + "";
        //        RadWindow3.Height = 600;
        //        RadWindow3.Width = 900;
        //        RadWindow3.Top = 20;
        //        RadWindow3.Left = 20;
        //        // RadWindowForNew.Title = "Time Slot";
        //        RadWindow3.OnClientClose = "OnClientClose";
        //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //        RadWindow3.Modal = true;
        //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        //        RadWindow3.VisibleStatusbar = false;

        //    }
        //    else if (common.myStr(ddlmoduletype.SelectedValue) == "CC")
        //    {
        //        RadWindow3.NavigateUrl = "/EMR/Problems/ViewPastPatientProblems.aspx?RegId=" + common.myStr(Session["RegistrationId"]) + "&Fdate=" + dtpfrmdate.SelectedDate + "&Tdate=" + dtpfrmdate.SelectedDate + "&Source=" + ddlSource.SelectedValue + "&MP=NO";
        //        RadWindow3.Height = 600;
        //        RadWindow3.Width = 900;
        //        RadWindow3.Top = 20;
        //        RadWindow3.Left = 20;
        //        // RadWindowForNew.Title = "Time Slot";
        //        RadWindow3.OnClientClose = "OnClientClose";
        //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //        RadWindow3.Modal = true;
        //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        //        RadWindow3.VisibleStatusbar = false;

        //    }
        //}
        //else
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Please Select Patient!";
        //}
    }
    protected void gvPatientHistory_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {

            LinkButton IbtnSelect = (LinkButton)e.Item.FindControl("IbtnSelect");
            Label lblOPIP = (Label)e.Item.FindControl("lblOPIP");
            LinkButton lnkview = (LinkButton)e.Item.FindControl("lnkview");
            LinkButton lnkDischargeSummary = (LinkButton)e.Item.FindControl("lnkDischargeSummary");
            HiddenField hdnIsResource = (HiddenField)e.Item.FindControl("hdnIsResource");
            HiddenField hdnOTCount = (HiddenField)e.Item.FindControl("hdnOTCount");
            LinkButton lnkOTNotes = (LinkButton)e.Item.FindControl("lnkOTNotes");
            Label lblSeenByDoctor = (Label)e.Item.FindControl("lblSeenByDoctor");
            LinkButton lnkHealthCheckup = (LinkButton)e.Item.FindControl("lnkHealthCheckup");
            HiddenField hdnHealthCheckup = (HiddenField)e.Item.FindControl("hdnHealthCheckup");

            lblSeenByDoctor.Visible = false;
            if (common.myStr(hdnIsResource.Value) == "1" || common.myStr(hdnIsResource.Value) == "True")
            {
                lblSeenByDoctor.Visible = true;
            }
            lnkview.Visible = true;
            lnkDischargeSummary.Visible = false;
            lnkOTNotes.Visible = false;

            if (common.myInt(hdnHealthCheckup.Value) != 0)
            {
                lnkHealthCheckup.Visible = true;
            }
            else
            {
                lnkHealthCheckup.Visible = false;
            }


            if ((lblOPIP.Text.Trim() == "I") || (lblOPIP.Text.Trim() == "E"))
            {
                lnkDischargeSummary.Visible = true;

            }
            if (common.myInt(hdnOTCount.Value) > 0)
                lnkOTNotes.Visible = true;

            if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I")
            {
                lblSeenByDoctor.Visible = false;
            }

            ////Added by rakesh on 26/06/2014 for alert the critical values in test start
            //Label lblCriticalIndication = (Label)e.Item.FindControl("lblCriticalIndication");
            //Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");


            //if (!(lblEncounterNo.Text.Equals(string.Empty)))
            //{
            //    BaseC.clsLISPhlebotomy Objstatus = new BaseC.clsLISPhlebotomy(sConString);
            //    string result;
            //    result = Objstatus.GetPatientHasCriticalParameterForOP(lblEncounterNo.Text, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0, 0);
            //    if (result.Equals("1"))
            //    {
            //        lblCriticalIndication.Visible = true;
            //        lblCriticalIndication.ToolTip = "Test results with panic values.";
            //    }
            //    else if (result.Equals("0"))
            //    {
            //        lblCriticalIndication.Visible = false;
            //        lblCriticalIndication.ToolTip = string.Empty;
            //    }
            //}
            ////Added by rakesh on 26/06/2014 for alert the critical values in test end
        }
    }
    protected void gvPatientHistory_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
            ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
            HiddenField hdnDoctorId = (HiddenField)e.Item.FindControl("hdnDoctorId");

            Label lblOPIP = (Label)e.Item.FindControl("lblOPIP");
            Label lblDate = (Label)e.Item.FindControl("lblDate");



            if (e.CommandName == "Add")
            {
                if (common.myStr(ViewState["EncounterId"]) != "" && common.myStr(ViewState["RegistrationId"]) != "")
                {
                    //RadWindow3.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&EREncounterId=" + common.myInt(ds.Tables[0].Rows[0]["EREncounterId"])
                    //           + "&AdmissionDate=" + Convert.ToDateTime(ds.Tables[0].Rows[0]["AdmissionDate"]).ToString("MM/dd/yyyy");
                    RadWindowForNew.NavigateUrl = "/Editor/WordProcessor.aspx?From=POPUP&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "&DoctorId=" + hdnDoctorId.Value + "&OPIP=" + lblOPIP.Text
                        + "&callby=MRD&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd");
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 900;
                    RadWindowForNew.Top = 20;
                    RadWindowForNew.Left = 20;
                    // RadWindowForNew.Title = "Time Slot";
                    // RadWindow3.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Patient!";
                }
            }
            else if (e.CommandName == "Inv")
            {

                if (common.myStr(ViewState["EncounterId"]) != "" && common.myStr(ViewState["RegistrationId"]) != "")
                {
                    //ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
                    //ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
                    //Session["RegistrationID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
                    RadWindow3.NavigateUrl = "/EMR/Masters/InvestigationResult.aspx?RegId=" + common.myStr(ViewState["RegistrationId"]);
                    RadWindow3.Height = 600;
                    RadWindow3.Width = 900;
                    RadWindow3.Top = 20;
                    RadWindow3.Left = 20;
                    // RadWindowForNew.Title = "Time Slot";
                    //RadWindow3.OnClientClose = "OnClientClose";
                    RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow3.Modal = true;
                    RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
                    //sViewState["RegistrationId"].VisibleStatusbar = false;
                }
            }
            //randhir//
            else if (e.CommandName == "DischargeSummary")
            {
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value.Trim()) > 0)
                {

                    //hdnRegistrationId.Value = common.myStr(ViewState["RegistrationId"]);
                    //hdnRegistrationNo.Value = common.myStr(txtRegNo.Text);
                    //hdnEncounterId.Value = common.myStr(ViewState["EncounterId"]);
                    Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
                    RadWindow3.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                        + "&RegNo=" + common.myStr(txtRegNo.Text)
                        + "&EncId=" + common.myInt(ViewState["EncounterId"])
                        + "&EncNo=" + common.myStr(lblEncounterNo.Text)
                        + "&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + "&OPIP=" + common.myStr(lblOPIP.Text) + "&MD=" + common.myStr(Request.QueryString["MD"]);

                    RadWindow3.Height = 590;
                    RadWindow3.Width = 1200;
                    RadWindow3.Top = 10;
                    RadWindow3.Left = 10;
                    RadWindow3.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow3.Modal = true;
                    RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow3.VisibleStatusbar = false;
                    RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    //return;
                }
            }                  //ran

            else if (e.CommandName == "HealthCheckup")
            {
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value.Trim()) > 0)
                {
                    Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
                    RadWindow3.NavigateUrl = "~/ICM/HealthCheckUpMedicalSummary.aspx?Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                    + "&RegNo=" + common.myStr(txtRegNo.Text) + "&HC=HC&EncId=" + common.myInt(ViewState["EncounterId"]) + "&EncNo=" + common.myStr(lblEncounterNo.Text);

                    RadWindow3.Height = 590;
                    RadWindow3.Width = 1200;
                    RadWindow3.Top = 10;
                    RadWindow3.Left = 10;
                    RadWindow3.OnClientClose = ""; //"Searc````````````hPatientOnClientClose";//
                    RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow3.Modal = true;
                    RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindow3.VisibleStatusbar = false;
                }
            }
            else if (e.CommandName == "Attach")
            {
                if (common.myInt(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value.Trim()) > 0)
                {

                    //hdnRegistrationId.Value = common.myStr(ViewState["RegistrationId"]);
                    //hdnRegistrationNo.Value = common.myStr(txtRegNo.Text);
                    //hdnEncounterId.Value = common.myStr(ViewState["EncounterId"]);


                    Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");


                    //RadWindow3.NavigateUrl = "~/EMR/AttachDocument.aspx?Regid=" + common.myInt(Session["RegistrationId"])
                    //    + "&RegNo=" + common.myStr(txtRegNo.Text)
                    //    + "&EncId=" + common.myInt(ViewState["EncounterId"])
                    //    + "&EncNo=" + common.myStr(lblEncounterNo.Text) + "&MASTER=No";

                    RadWindow3.NavigateUrl = "~/EMR/AttachDocumentFTP.aspx?Regid=" + common.myInt(Session["RegistrationId"])
                        + "&RegNo=" + common.myStr(txtRegNo.Text)
                        + "&EncId=" + common.myInt(ViewState["EncounterId"])
                        + "&EncNo=" + common.myStr(lblEncounterNo.Text) + "&MASTER=No";

                    RadWindow3.Height = 630;
                    RadWindow3.Width = 1200;
                    RadWindow3.Top = 10;
                    RadWindow3.Left = 10;
                    RadWindow3.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow3.Modal = true;
                    RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow3.VisibleStatusbar = false;

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    //return;
                }
            }                  //randhir


            else if (e.CommandName == "ProblemName")
            {
                RadWindow3.NavigateUrl = "/EMR/Masters/ViewDiagnosisDetails.aspx?RegId=" + ViewState["RegistrationId"].ToString() + "&EncId=" + ViewState["EncounterId"].ToString();
                RadWindow3.Height = 500;
                RadWindow3.Width = 620;
                RadWindow3.Top = 20;
                RadWindow3.Left = 20;
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow3.VisibleStatusbar = false;
            }
            else if (e.CommandName == "OtNotes")
            {
                ////////Session["OPIP"] = "I";
                ////////Session["EncounterId"] = common.myInt(ViewState["EncounterId"].ToString());
                ////////Session["RegistrationId"] = common.myInt(ViewState["RegistrationId"].ToString());

                RadWindow3.NavigateUrl = "/EMR/Templates/Default.aspx?MASTER=NO&Type=OT";
                RadWindow3.Top = 10;
                RadWindow3.Left = 10;
                RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow3.Modal = true;
                RadWindow3.VisibleStatusbar = false;
                RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnShowDetails_OnClick(object sender, EventArgs e)
    {

        if (common.myStr(txtRegNo.Text.Trim()) != "")
        {
            BaseC.Patient objp = new BaseC.Patient(sConString);
            hdnRegId.Value = (string)objp.getRegistrationIDFromRegistrationNo(common.myInt(txtRegNo.Text), common.myInt(Session["HospitalLocationId"]), 0);
            BindProvider();
            if (common.myInt(hdnRegId.Value) > 0)
                btnFilter_Click(null, null);
        }

        else
        {
            hdnRegId.Value = "";
            BindBlankGrid(0);
            Alert.ShowAjaxMsg("Please Enter Patient Registration No.!", Page.Page);
            return;
        }

    }
    protected void lnkDownload_OnClick(object sender, EventArgs e)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);        
        try
        {
            //BaseC.HospitalSetup objHc = new BaseC.HospitalSetup(sConString);
            //string strPath = objHc.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "FileLocation");
            string strFolderPath = "";
            string strRegOldNo = "";
            string fileName = "";
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            hsIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));

            if (common.myStr(txtRegNo.Text.Trim()) != "")
            {
                hsIn.Add("@RegistraitonNo", common.myStr(txtRegNo.Text.Trim()));
                ds = new DataSet();
                ds = dl.FillDataSet(CommandType.Text, " select isnull(RegistrationNoOld,RegistrationNo) RegistrationNoOld, f.CaseNotePath from Registration r inner join facilitymaster f on r.facilityid = f.facilityid  where   r.HospitalLocationId = @inyHospitalLocationID and RegistrationNo = @RegistraitonNo", hsIn);
                strRegOldNo = common.myStr(ds.Tables[0].Rows[0]["RegistrationNoOld"]).Trim();
                strFolderPath = common.myStr(ds.Tables[0].Rows[0]["CaseNotePath"]).Trim();
            }

            if (strRegOldNo != "" && strFolderPath != "")
            {
                if (strRegOldNo.Trim().Contains("/") == true)
                {
                    strRegOldNo = strRegOldNo.Replace('/', '-').Trim();
                }
                string[] filePaths = Directory.GetFiles(strFolderPath, strRegOldNo + "*.pdf");

                if (filePaths.Length > 0)
                {
                    if (filePaths[0].ToString() != "0")
                    {

                        fileName = filePaths[0].ToString();
                        if (fileName != "")
                        {
                            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                            if (file.Exists)
                            {
                                //file.CopyTo(Server.MapPath("/Images/EMRCaseNotes/" + file.Name), true);
                                //System.IO.FileInfo file1 = new System.IO.FileInfo(Server.MapPath("/Images/EMRCaseNotes/" + file.Name));

                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                Response.AddHeader("Content-Length", file.Length.ToString());
                                Response.ContentType = "application/octet-stream";
                                Response.Flush();
                                Response.WriteFile(file.FullName);
                                Response.End();

                            }
                            else
                            {
                                Response.Write("This file does not exist.");
                            }
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Patient file does not exist.";
                    Alert.ShowAjaxMsg("Patient file does not exist.", Page.Page);
                    return;
                }

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        ////added by sikandar for code optimize
        //finally
        //{
        //    dl = null;
        //}

    }
    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        hdnRegId.Value = "";
        RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=O&RegEnc=0";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            RadWindow3.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PN=&PAG=&PNo=" + txtRegNo.Text.Trim() + "&PId=" + common.myStr(Session["RegistrationId"]) + "&EId=" + common.myStr(Session["EncounterId"]);

            RadWindow3.Height = 600;
            RadWindow3.Width = 900;
            RadWindow3.Top = 10;
            RadWindow3.Left = 10;
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.VisibleStatusbar = false;
            RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }
    protected void btnReferral_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Session["OPIP"]) == "O")
        {
            RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=O&MASTER=NO&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EId=" + common.myInt(Session["EncounterID"]);
        }
        else if (common.myStr(Session["OPIP"]) == "I")
        {
            RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=I&MASTER=NO&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EId=" + common.myInt(Session["EncounterID"]);
        }
        else
        {
            return;
        }
        RadWindow3.Height = 600;
        RadWindow3.Width = 800;
        RadWindow3.Top = 20;
        RadWindow3.Left = 20;

        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow3.VisibleStatusbar = false;
    }
    protected void btnicca_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(txtRegNo.Text.Trim()) != "")
        {
            //System.Diagnostics.Process.Start("~/bin/ICCASSOBin/ICCA_SSO.exe");
            //System.Diagnostics.Process process1 = new System.Diagnostics.Process();
            //process1.StartInfo.WorkingDirectory = Request.MapPath("~/bin/");

            //process1.StartInfo.UseShellExecute = false;
            //process1.StartInfo.CreateNoWindow = false;
            //process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //process1.StartInfo.FileName = Request.MapPath("~/bin/ICCASSOBin/ICCA_SSO.exe  " + common.myStr(txtRegNo.Text.Trim()) + " ");

            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\ICCAS\\ICCA_SSO.exe");
            //   Process.Start(startInfo);

            startInfo.Arguments = "" + common.myStr(txtRegNo.Text.Trim()) + "";

            Process.Start(startInfo);



            //string Str =  txtRegNo.Text.Trim();
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
        }
    }

    //protected void btnCustomView_OnClick(object sender, EventArgs e)
    //{
    //    if (txtRegNo.Text.Trim().Length != 0)
    //    {
    //        RadWindow3.NavigateUrl = "/LIS/Phlebotomy/PatientLabHistoryDynamic.aspx?Master=Blank&EncId=" + common.myStr(Session["EncounterId"]) + "&RegNo=" + common.myStr(txtRegNo.Text.Trim()) + "";
    //        RadWindow3.Height = 600;
    //        RadWindow3.Width = 900;
    //        RadWindow3.Top = 20;
    //        RadWindow3.Left = 20;

    //        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindow3.Modal = true;
    //        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
    //        RadWindow3.VisibleStatusbar = false;
    //    }
    //}

    //It will be come in BaseC-clsLISPhlebotomy.cs
    //Added by rakesh to Get Patient Has Critical Parameter exists
    //public string GetPatientHasCriticalParameter(string EncounterNo, int HospitalLocationID, int FacilityId, int intEncodedBy, int labNo, int serviceId)
    //{
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable hshInput = new Hashtable();
    //    hshInput.Add("@EncounterNo", EncounterNo);
    //    hshInput.Add("@intHospitalLocationid", HospitalLocationID);
    //    hshInput.Add("@intFacilityID", FacilityId);
    //    hshInput.Add("@intEncodedBy", intEncodedBy);
    //    hshInput.Add("@LabNo", labNo);
    //    hshInput.Add("@ServiceId", serviceId);
    //    string strResult = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "uspDiagGetPatientHasCriticalParameter", hshInput);
    //    return strResult;
    //}
    //Added by rakesh to Get Patient Has Critical Parameter exists

    protected void lnkpatientFiles_OnClick(object sender, EventArgs e)
    {
        string navstring = "";
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strsql = "select Isnull(LocalFileScanFolder,'') from FileServersetup where FacilityID=" + common.myInt(Session["FacilityID"]);

            navstring = common.myStr(dl.ExecuteScalar(CommandType.Text, strsql));
            Session["LocFile"] = navstring;
        }
        catch (Exception ex)
        {

        }
        if (navstring.Trim() != "")
        {
            RadWindow3.NavigateUrl = "/EMR/viewDocs.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(txtRegNo.Text);

            RadWindow3.Top = 10;
            RadWindow3.Left = 10;
            RadWindow3.OnClientClose = ""; //"SearchPatientOnClientClose";//
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindow3.VisibleStatusbar = false;
            RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        }
    }
}
